using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.Drawing;
using AjaxControlToolkit;
using System.Web;
using System.Web.UI;

public enum lPermissions
{
    showQueue,
    editNotices,
    removeNotices,
    setSpecial,
    addNotices,
    showArchive
}

namespace NoticeBoard.Controls
{
    public partial class cntNoticeBoard : System.Web.UI.UserControl
    {
        public bool ViewSwitch
        {
            get { return (Session["ViewSwitch"] == null) ? false : bool.Parse(Session["ViewSwitch"].ToString()); }
            set { Session["ViewSwitch"] = value; }
        }
        
        private AccountManager user { set; get; }
        private NoticesManager notices { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            QueryManager.Open();

            this.user = new AccountManager();
            this.notices = new NoticesManager();

            
            // Pozostałe
            bind_ListView1();

            // Moje
            bind_ListView2();

            // Kolejka
            bind_ListView3();


            if (!IsPostBack)
            {

                search_Dropdown.DataSource = this.notices.lCategories;
                search_Dropdown.DataBind();
            }
           
        }

        public bool assigned(lPermissions p)
        {
            return true;


            return user.getListOfPermissions().Contains(p);
        }

        protected void addNotice_Click(object sender, EventArgs e)
        {
            
            if(assigned(lPermissions.addNotices))
            {
                if (notices.count(user.id) < 5)
                {
                    modal_Kategoria.DataSource = this.notices.lCategories;
                    modal_Kategoria.DataBind();
                    modal_DataDodania.Text = DateTime.Now.ToString();
                    modal_DataZakonczenia.Text = DateTime.Now.AddDays(14).ToString();

                    if (assigned(lPermissions.editNotices))
                    {
                        modal_Status.Visible = true;
                        modal_Status.DataSource = this.notices.lStatuses;
                        modal_Status.DataBind();
                    }

                    cntModal.Show(true);
                }
                else
                {
                    showAlert("Wystawiłeś maksymalną ilość ogłoszeń jaka przysługuje użytkownikowi.");
                }
            }
            else
            {
                showAlert("Nie masz prawa, aby dodać nowe ogłoszenie.");
            }
        }
        
        protected void showAlert(string text)
        {
            if (text != "")
            {
                alertText.Text = text;
                modalAlerts.Show(false);
            }
        }

        protected void editNotice_Click(object sender, EventArgs e)
        {
            int id = int.Parse(((LinkButton)sender).CommandArgument.ToString());
            Notice row = this.notices.actual.FirstOrDefault(notice => notice.Id.Equals(id));

            modal_Id.Text = row.Id.ToString();
            modal_Tytul.Text = row.Tytul;
            modal_Opis.Text = row.Opis;
            modal_Cena.Text = row.Cena.ToString();
            modal_DataDodania.Text = row.DataDodania.ToString();
            modal_DataZakonczenia.Text = row.DataZakonczenia.ToString();
            modal_Status.SelectedValue = row.Status.ToString();
            modal_Kategoria.DataSource = this.notices.lCategories;
            modal_Kategoria.DataBind();
            modal_Kategoria.SelectedValue = row.Kategoria.ToString();
            modal_Status.DataSource = this.notices.lStatuses;
            modal_Status.DataBind();
            modal_Status.SelectedValue = row.Status.ToString();

            cntModal.Show(false);
        }

        // TODO
        // Limity megabajtów, dodać do wpisu
        protected void FileUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            if(Int32.Parse(e.FileSize) > 1000000)
            {
                // TODO: wstawić labela do Modala
                tooBigFile.Visible = true;
                return;
            }

            // Get new ID from database
            int created_id = this.notices.getNextID();
            string extension = System.IO.Path.GetExtension(AsyncFileUpload1.PostedFile.FileName);

            // Upload File
            //string filename = System.IO.Path.GetFileName(AsyncFileUpload.FileName);
            string filename = created_id + extension;
            AsyncFileUpload1.SaveAs(Server.MapPath("~/Upload/Images/") + filename);

            // Main image
            System.Drawing.Image big = System.Drawing.Image.FromFile(Server.MapPath("~/Upload/Images/") + filename);
            int width = big.Width; int height = big.Height;

            // Thumbnail
            System.Drawing.Image small = big.GetThumbnailImage(width / 5, height / 5, () => false, IntPtr.Zero);
            small.Save(Server.MapPath("~/Upload/Thumbnails/") + "thumb_" +filename);
        }

        // TODO
        // List of extensiosn
        protected string GetNoticeImageThumbnail(int notice_id)
        {
            string[] files = Directory.GetFiles(Server.MapPath("~/Portal/Ogloszenia/"), "thumb_" + notice_id + "*");
            foreach(string file in files)
            {
                FileInfo fi = new FileInfo(file);
                return "Portal/Ogloszenia/" + fi.Name;
            }
            return "null";
        }

        protected string GetNoticeFullImage(int notice_id)
        {
            string[] files = Directory.GetFiles(Server.MapPath("~/Portal/Ogloszenia/"), "" + notice_id + "*");
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                return "Portal/Ogloszenia/" + fi.Name;
            }
            return "null";
        }

        protected void modalAccept_Click(object sender, EventArgs e)
        {
            Notice notice = new Notice();
            
            notice.Tytul = modal_Tytul.Text;
            notice.Kategoria = int.Parse(modal_Kategoria.Text);
            notice.Opis = modal_Opis.Text;

            int val;
            if (int.TryParse(modal_Cena.Text, out val))
            {
                notice.Cena = val;
            }

            notice.Uzytkownik = user.id;

            if (assigned(lPermissions.editNotices))
                notice.Status = int.Parse(modal_Status.SelectedValue);

            int id = (modal_Id.Text == "") ? 0 : int.Parse(modal_Id.Text);

            if (id > 0)
            {
                notice.Id = id;
                this.notices.Edit(notice);
            }
            else this.notices.Add(notice);

            bind_ListView1();
            bind_ListView2();
            bind_ListView3();

            cntModal.Close();
        }

        protected void archive_Click(object sender, EventArgs e)
        {
            bind_ListView4();
            archiveModal.Show(false);
        }
        
        protected void removeNotice_Click(object sender, EventArgs e)
        {
            this.notices.Remove(int.Parse(((LinkButton)sender).CommandArgument.ToString()));
            bind_ListView1();
            bind_ListView2();
            bind_ListView3();
        }
        
        protected void setViewSwitch(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            this.ViewSwitch = btn.ID.Equals("list");
        }
        
        protected void OnSearchButtonClick(object sender, EventArgs e)
        {
            ListView1.DataSource = this.notices.actual.Where(l => l.Status == 2)
                .Where(l => l.Kategoria == int.Parse(search_Dropdown.SelectedValue))
                .Where(l => l.Tytul.Contains(TextBoxSearch.Text)).ToList();
            
            ListView1.DataBind();
        }
        
        protected void setSpecial(object sender, EventArgs e)
        {
            notices.Special(int.Parse(((LinkButton)sender).CommandArgument.ToString()));

            bind_ListView1();
            bind_ListView2();
        }

        protected void setAsActive(object sender, EventArgs e)
        {
            // Set notice as active from Queue
            int notice_id = int.Parse(((LinkButton)sender).CommandArgument.ToString());
            notices.Active(notice_id);

            bind_all();
        }

        // Global notices
        private void bind_ListView1()
        {
            //ListView1.DataSource = this.notices.actual.Where(l => !l.IdUzytkownika.Equals(user.id)).Where(l => l.Status == 2).ToList();
            //ListView1.DataBind();
        }
        
        // User notices
        private void bind_ListView2()
        {
        //    ListView2.DataSource = this.notices.actual.Where(l => l.IdUzytkownika.Equals(user.id)).ToList();
        //    ListView2.DataBind();
        }

        // Queue notices
        private void bind_ListView3()
        {
            //ListView3.DataSource = this.notices.actual.Where(l => l.Status == 1 ).ToList();
            //ListView3.DataBind();
        }

        // Archive notices
        private void bind_ListView4()
        {
            //ListView4.DataSource = this.notices.archive;
            //ListView4.DataBind();
        }

        private void bind_all()
        {
            bind_ListView1();
            bind_ListView2();
            bind_ListView3();
        }
    }

    #region Modele danych
    public class Status
    {
        public int Id { set; get; }
        public string Nazwa { set; get; }
    }

    public class Notice
    {
        public int Id { set; get; }
        public string Tytul { set; get; }
        public int Kategoria { set; get; }
        public int Cena { set; get; }
        public string Opis { set; get; }
        public string IdUzytkownika { set; get; }
        public string Uzytkownik { set; get; }
        public DateTime DataDodania { set; get; }
        public DateTime DataZakonczenia { set; get; }
        public bool Wyroznione { set; get; }
        public int Status { set; get; }

        public Notice()
        {
            Cena = 0;
            DataDodania = DateTime.Now;
            DataZakonczenia = DateTime.Now.AddDays(14);
            Wyroznione = false;
            Status = 1;
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public bool Aktywne { get; set; }
    }
    #endregion

    #region Zarządzanie notatkami
    public partial class NoticesManager
    {
        private List<Notice> lNotices { get; set; }
        public List<Status> lStatuses { get; set; }
        public List<Category> lCategories { get; set; }

        public List<Notice> archive
        {
            get
            {
                fetchArchiveList();
                return lNotices;
            }
        }

        public List<Notice> actual
        {
            get
            {
                fetchNoticeList();
                return lNotices;
            }
        }


        public NoticesManager()
        {
            fetchStatusList();
            fetchCategoryList();
        }

        private void fetchCategoryList()
        {
            SqlCommand sqlCmd = new SqlCommand("SELECT [Id], [Nazwa], [Aktywne] FROM [poOgloszeniaKategorie];", QueryManager.c_handler);

            SqlDataReader reader = sqlCmd.ExecuteReader();

            lCategories = new List<Category>();

            while (reader.Read())
            {
                Category category = new Category();

                category.Id = int.Parse(reader["Id"].ToString());
                category.Nazwa = reader["Nazwa"].ToString();
                category.Aktywne = bool.Parse(reader["Aktywne"].ToString());
                    
                lCategories.Add(category);
            }

            reader.Close();
        }

        private void fetchStatusList()
        {
            SqlCommand sqlCmd = new SqlCommand("SELECT [Id], [Nazwa] FROM [poOgloszeniaStatusy];", QueryManager.c_handler);

            SqlDataReader reader = sqlCmd.ExecuteReader();

            lStatuses = new List<Status>();

            while (reader.Read())
            {
                Status status = new Status();

                status.Id = int.Parse(reader["Id"].ToString());
                status.Nazwa = reader["Nazwa"].ToString();

                lStatuses.Add(status);
            }

            reader.Close();
        }

        private void fetchNoticeList()
        {
            SqlCommand sqlCmd = new SqlCommand(@"
SELECT O.[Id], O.[Tytul], O.[Kategoria], O.[Cena], O.[Opis], U.[Id] as [IdUzytkownika], U.Nazwisko + ' ' + U.Imie as [Uzytkownik], O.[DataDodania], O.[DataZakonczenia], O.[Wyroznione], O.[Status] 
FROM poOgloszenia as O 
INNER JOIN [Pracownicy] as U ON U.[Id]=O.[Uzytkownik] 
WHERE (O.[Status]=2 OR O.[Status]=1);
                ", QueryManager.c_handler);

            SqlDataReader reader = sqlCmd.ExecuteReader();

            lNotices = new List<Notice>();

            while (reader.Read())
            {
                Notice notice = new Notice();

                notice.Id = int.Parse(reader["Id"].ToString());
                notice.Tytul = reader["Tytul"].ToString();
                notice.Kategoria = int.Parse(reader["Kategoria"].ToString());
                notice.Cena = int.Parse(reader["Cena"].ToString());
                notice.Opis = reader["Opis"].ToString();
                notice.Uzytkownik = reader["Uzytkownik"].ToString();
                notice.IdUzytkownika = reader["IdUzytkownika"].ToString();
                notice.DataDodania = DateTime.Parse(reader["DataDodania"].ToString());
                notice.DataZakonczenia = DateTime.Parse(reader["DataZakonczenia"].ToString());
                notice.Wyroznione = bool.Parse(reader["Wyroznione"].ToString());
                notice.Status = int.Parse(reader["Status"].ToString());
                
                lNotices.Add(notice);
            }

            reader.Close();
        }

        private void fetchArchiveList()
        {
            SqlCommand sqlCmd = new SqlCommand(@"
SELECT O.[Id], O.[Tytul], O.[Kategoria], O.[Cena], O.[Opis], U.[Id] as [IdUzytkownika], U.[NazwaUzytkownika] as [Uzytkownik], O.[DataDodania], O.[DataZakonczenia], O.[Wyroznione], O.[Status] 
FROM [poOgloszenia] as O 
INNER JOIN [Pracownicy] as U ON U.[Id]=O.[Uzytkownik] 
WHERE (O.[Status]!=2 AND O.[Status]!=1);
                ", QueryManager.c_handler);

            SqlDataReader reader = sqlCmd.ExecuteReader();

            lNotices = new List<Notice>();

            while (reader.Read())
            {
                Notice notice = new Notice();

                notice.Id = int.Parse(reader["Id"].ToString());
                notice.Tytul = reader["Tytul"].ToString();
                notice.Kategoria = int.Parse(reader["Kategoria"].ToString());
                notice.Cena = int.Parse(reader["Cena"].ToString());
                notice.Opis = reader["Opis"].ToString();
                notice.Uzytkownik = reader["Uzytkownik"].ToString();
                notice.IdUzytkownika = reader["IdUzytkownika"].ToString();
                notice.DataDodania = DateTime.Parse(reader["DataDodania"].ToString());
                notice.DataZakonczenia = DateTime.Parse(reader["DataZakonczenia"].ToString());
                notice.Wyroznione = bool.Parse(reader["Wyroznione"].ToString());
                notice.Status = int.Parse(reader["Status"].ToString());

                lNotices.Add(notice);
            }

            reader.Close();
        }

        public int getNextID()
        {
            SqlCommand sqlCmd = new SqlCommand("SELECT MAX(Id+1) AS NextID FROM [Ogloszenia].[dbo].[Ogloszenia]", QueryManager.c_handler);
            int next_id = (int)sqlCmd.ExecuteScalar();
            return next_id;
        }
        
        public void Add(Notice notice)
        {
            SqlCommand sqlCmd = new SqlCommand(@"
INSERT INTO poOgloszenia(Tytul, Kategoria, Cena, Opis, DataDodania, DataZakonczenia, Uzytkownik, Status) 
        VALUES (@Tytul, @Kategoria, @Cena, @Opis, @DataDodania, @DataZakonczenia, @Uzytkownik, @Status)
                ", QueryManager.c_handler);

            sqlCmd.Parameters.AddWithValue("Tytul", notice.Tytul);
            sqlCmd.Parameters.AddWithValue("Kategoria", notice.Kategoria);
            sqlCmd.Parameters.AddWithValue("Cena", notice.Cena);
            sqlCmd.Parameters.AddWithValue("Opis", notice.Opis);
            sqlCmd.Parameters.AddWithValue("DataDodania", notice.DataDodania);
            sqlCmd.Parameters.AddWithValue("DataZakonczenia", notice.DataZakonczenia);
            sqlCmd.Parameters.AddWithValue("Uzytkownik", notice.Uzytkownik);
            sqlCmd.Parameters.AddWithValue("Status", notice.Status);

            // TRY CATCH
            try
            {
                if (sqlCmd.ExecuteNonQuery() > 0)
                {
                    actual.Add(notice);
                }
            }
            catch(SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("cos sie... cos sie zepsuło i nie było mnie widać");
            }
        }
        
        public void Remove(int id)
        {
            SqlCommand sqlCmd = new SqlCommand("UPDATE [poOgloszenia] SET [Status]=4 WHERE [Id]=@Id", QueryManager.c_handler);
            sqlCmd.Parameters.Add("@Id", SqlDbType.Int);
            sqlCmd.Parameters["@Id"].Value = id;

            if (sqlCmd.ExecuteNonQuery() > 0)
            {
                lNotices.Remove(lNotices.First(l => l.Id == id));
            }
        }

        public void Edit(Notice notice)
        {
            SqlCommand sqlCmd = new SqlCommand(@"
UPDATE poOgloszenia SET Tytul=@Tytul, Kategoria=@Kategoria, Cena=@Cena, Opis=@Opis, DataDodania=@DataDodania, DataZakonczenia=@DataZakonczenia, Status=@Status 
WHERE Id=@Id
                ", QueryManager.c_handler);

            sqlCmd.Parameters.AddWithValue("Id", notice.Id);
            sqlCmd.Parameters.AddWithValue("Tytul", notice.Tytul);
            sqlCmd.Parameters.AddWithValue("Kategoria", notice.Kategoria);
            sqlCmd.Parameters.AddWithValue("Cena", notice.Cena);
            sqlCmd.Parameters.AddWithValue("Opis", notice.Opis);
            sqlCmd.Parameters.AddWithValue("DataDodania", notice.DataDodania);
            sqlCmd.Parameters.AddWithValue("DataZakonczenia", notice.DataZakonczenia);
            sqlCmd.Parameters.AddWithValue("Status", notice.Status);

            if (sqlCmd.ExecuteNonQuery() > 0)
            {
                lNotices.First(l => l.Id == notice.Id).Tytul = notice.Tytul;
                lNotices.First(l => l.Id == notice.Id).Kategoria = notice.Kategoria;
                lNotices.First(l => l.Id == notice.Id).Cena = notice.Cena;
                lNotices.First(l => l.Id == notice.Id).Opis = notice.Opis;
                lNotices.First(l => l.Id == notice.Id).DataDodania = notice.DataDodania;
                lNotices.First(l => l.Id == notice.Id).DataZakonczenia = notice.DataZakonczenia;
                lNotices.First(l => l.Id == notice.Id).Status = notice.Status;
            }
        }

        public void Special(int id)
        {
            SqlCommand sqlCmd = new SqlCommand("UPDATE [poOgloszenia] SET [Wyroznione]=@Wyroznij WHERE [Id]=@Id;", QueryManager.c_handler);

            sqlCmd.Parameters.Add("@Id", SqlDbType.Int);
            sqlCmd.Parameters["@Id"].Value = id;

            bool special = isSpecial(id);
            sqlCmd.Parameters.Add("@Wyroznij", SqlDbType.Bit);
            sqlCmd.Parameters["@Wyroznij"].Value = !special;

            if (sqlCmd.ExecuteNonQuery() > 0)
            {
                lNotices.First(l => l.Id == id).Wyroznione = !special;
            }
        }

        public void Active(int id)
        {
            SqlCommand sqlCmd = new SqlCommand("UPDATE [poOgloszenia] SET [Status]=2 WHERE [Id]=@Id", QueryManager.c_handler);

            sqlCmd.Parameters.Add("@Id", SqlDbType.Int);
            sqlCmd.Parameters["@Id"].Value = id;

            if(sqlCmd.ExecuteNonQuery() > 0)
            {
                // TODO
                lNotices.First(l => l.Id == id).Status = 2;
            }
        }

        public bool isSpecial(int id)
        {
            return lNotices.First(l => l.Id == id).Wyroznione;
        }

        public int count(string userId)
        {
            SqlCommand sqlCmd = new SqlCommand("SELECT COUNT(*) FROM [poOgloszenia] WHERE [Uzytkownik]=@userId AND [Status]<3;", QueryManager.c_handler);

            sqlCmd.Parameters.Add("@userId", SqlDbType.NVarChar);
            sqlCmd.Parameters["@userId"].Value = userId;

            int ret;
            return (int.TryParse(sqlCmd.ExecuteScalar().ToString(), out ret)) ? ret : 0;
        }
    }
    #endregion

    #region Zarządzanie SQL
    public static class QueryManager
    {
        public static SqlConnection c_handler { set; get; }
        
        public static void Open()
        {
            c_handler = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PORTAL"].ConnectionString);
            c_handler.Open();
        }

        public static void Close()
        {
            c_handler.Close();
        }
    }
    #endregion

    #region Zarządzanie kontem
    public partial class AccountManager
    {
        private List<lPermissions> listOfPermissions { get; set; }

        public string id
        {
            get { return Hash(MD5.Create(), this.userName); }
        }

        public string userName
        {
            get { return System.Web.HttpContext.Current.User.Identity.Name; }
        }

        public List<lPermissions> getListOfPermissions()
        {
            return this.listOfPermissions;
        }

        public AccountManager()
        {
            Authentication();
        }

        static public string Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2").ToLower());
            }

            return sBuilder.ToString();
        }

        public void Authentication()
        {
            if (!HasAccount())
            {
                Registration();
            }

            AssignPermissions();
        }

        private void Registration()
        {
            if (MakeUser())
            {
                AssignDefaultPermissions();
            }
        }

        private bool HasAccount()
        {
            return true;



            SqlCommand sqlCmd = new SqlCommand("SELECT 1 FROM [Pracownicy] WHERE [Id] = @id;", QueryManager.c_handler);

            sqlCmd.Parameters.Add("@id", SqlDbType.NVarChar);
            sqlCmd.Parameters["@id"].Value = this.id;
            
            return !(sqlCmd.ExecuteScalar() == null);
        }

        private bool MakeUser()
        {
            SqlCommand sqlCmd = new SqlCommand("INSERT [Uzytkownicy] (Id, NazwaUzytkownika) VALUES (@id, @userName);", QueryManager.c_handler);

            sqlCmd.Parameters.Add("@id", SqlDbType.NVarChar);
            sqlCmd.Parameters["@id"].Value = this.id;

            sqlCmd.Parameters.Add("@userName", SqlDbType.NVarChar);
            sqlCmd.Parameters["@userName"].Value = this.userName;
            
            return sqlCmd.ExecuteNonQuery() > 0;
        }

        private void AssignDefaultPermissions()
        {
            return;

            SqlCommand sqlCmd = new SqlCommand("INSERT [MaskiUprawnien] (Id) VALUES (@id);", QueryManager.c_handler);

            sqlCmd.Parameters.Add("@id", SqlDbType.NVarChar);
            sqlCmd.Parameters["@id"].Value = this.id;

            sqlCmd.ExecuteNonQuery();
        }

        private void AssignPermissions()
        {
            AssignDefaultPermissions();
            return;
            
            
            
            
            
            listOfPermissions = new List<lPermissions>();

            SqlCommand sqlCmd = new SqlCommand("SELECT * FROM [MaskiUprawnien] WHERE [Id] = @id;", QueryManager.c_handler);

            sqlCmd.Parameters.Add("@id", SqlDbType.NVarChar);
            sqlCmd.Parameters["@id"].Value = this.id;

            SqlDataReader reader = sqlCmd.ExecuteReader();
            reader.Read();

            if (reader.HasRows)
            {
                for (var i = 1; i < reader.FieldCount; i++)
                {
                    bool isAssigned = bool.Parse(reader[i].ToString());
                    if (isAssigned)
                    {
                        listOfPermissions.Add((lPermissions)Enum.Parse(typeof(lPermissions), reader.GetName(i)));
                    }
                }
            }
            else
            {
                AssignDefaultPermissions();
                AssignPermissions();
            }

            reader.Close();
        }
    }
    #endregion

}