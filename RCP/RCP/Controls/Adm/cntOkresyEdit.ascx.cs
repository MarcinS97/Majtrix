using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Adm
{
    public partial class cntOkresyEdit : System.Web.UI.UserControl
    {
        public event EventHandler Saved;

        [Serializable]
        public class OkresRozliczeniowy
        {
            public OkresRozliczeniowy() { }
            public OkresRozliczeniowy(DataRow dr)
            {
                this.Id = db.getInt(dr, "Id", -1);
                this.DataOd = db.getDateTime(dr, "DataOd", DateTime.Today);
                this.DataDo = db.getDateTime(dr, "DataDo", DateTime.Today);
                this.Status = db.getInt(dr, "Status", 0);
                this.Zamknal = db.getInt(dr, "Zamknal", 0);
                this.IloscMiesiecy = db.getInt(dr, "IloscMiesiecy", 0);
                this.DataZamkniecia = db.getDateTime(dr, "DataZamkniecia");
                this.Typ = db.getInt(dr, "Typ", 0);
            }
            public int Id { get; set; }
            public DateTime DataOd { get; set; }
            public DateTime DataDo { get; set; }
            public int Status { get; set; }
            public int Zamknal { get; set; }
            public int IloscMiesiecy { get; set; }
            public DateTime? DataZamkniecia { get; set; }
            public int Typ
            {
                get;
                set;
            }
            public List<CzasNom> CzasyNom { get; set; }
        }

        [Serializable]
        public class CzasNom
        {
            public CzasNom() { }
            public CzasNom(DataRow dr)
            {
                this.Data = db.getDateTime(dr, "Data", DateTime.Now);
                this.DniPrac = db.getInt(dr, "DniPrac", 0);
            }
            public DateTime Data { get; set; }
            public String Friendly
            {
                get { return Tools.DateFriendlyName(0, Data); }
            }
            public int? DniPrac { get; set; }
            public int Kalendarz { get; set; }
            public int DniPracValue { get { return DniPrac ?? Kalendarz; } }
            public bool IsEnabled { get; set; }
        }

        public OkresRozliczeniowy Okres
        {
            get { return ViewState["vOkres"] as OkresRozliczeniowy; }
            set { ViewState["vOkres"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show()
        {
            this.Mode = EMode.Insert;
            Prepare(null);
            cntModal.Show();
        }

        public void Show(String OkresId)
        {
            this.Mode = EMode.Edit;
            Prepare(OkresId);
            cntModal.Show(false);
        }

        void Prepare(String OkresId)
        {
            switch (Mode)
            {
                case EMode.Insert:
                    divTypOkresu.Visible = true;
                    divNom.Visible = false;
                    cntModal.Title = "Dodaj okres";
                    Okres = new OkresRozliczeniowy();
                    break;
                case EMode.Edit:
                    cntModal.Title = "Edytuj okres";
                    divTypOkresu.Visible = false;

                    Okres = new OkresRozliczeniowy(db.Select.Row(dsGetOkresData, OkresId));
                    Okres.CzasyNom = GetCzasyNom(OkresId);
                    deStart.Date = Okres.DataOd;
                    deEnd.Date = Okres.DataDo;


                    divNom.Visible = true;
                    rpNom.DataSource = Okres.CzasyNom;
                    rpNom.DataBind();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        public void Close()
        {
            cntModal.Close();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case EMode.Insert:
                    Insert();
                    break;
                case EMode.Edit:
                    Update();
                    break;
            }
        }

        public List<CzasNom> GetCzasyNom(String OkresId)
        {
            DataTable dt = db.Select.Table(dsGetOkresCzasyNom, OkresId);
            List<CzasNom> list = new List<CzasNom>();
            foreach (DataRow dr in dt.Rows)
            {
                CzasNom czasNom = new CzasNom(dr);
                int kalendarz = Convert.ToInt32(db.Select.Scalar(dsGetCzasNom, db.strParam(czasNom.Data.ToString())));
                czasNom.Kalendarz = kalendarz;
                list.Add(czasNom);
            }
            return list;
        }

        private bool SetCzasyNomVisible()
        {
            if (Okres.Typ != 0 && Okres.DataOd != DateTime.MinValue && deStart.Date != null)  // wyszło z obserwacji, deStart na wszeki wypadek ...
            {
                divNom.Visible = true;
                BindCzasyNom();
                return true;
            }
            else
            {
                divNom.Visible = false;
                return false;
            }
        }

        protected void ddlTypOkresu_SelectedIndexChanged(object sender, EventArgs e)
        {
            String type, mies;
            Tools.GetLineParams(ddlTypOkresu.SelectedValue, out type, out mies);
            if (!String.IsNullOrEmpty(type))
            {
                Okres.Typ = Convert.ToInt32(type);
                int ilmies = Tools.StrToInt(mies, 1);
                //if (deStart.Date == null)    // inicjator <<< zawsze przyzmianie
                {
                    DataRow dr = db.selectRow(dsNextOkres.SelectCommand, type, ilmies);
                    if (dr != null)
                        deStart.Date = (DateTime)db.getDateTime(dr, 0);
                    else
                        deStart.Date = Tools.bom(DateTime.Today).AddMonths(1);   // następny miesiąc
                }
                if (deStart.Date != null)
                {
                    deEnd.Date = ((DateTime)deStart.Date).AddMonths(ilmies).AddDays(-1);
                    Okres.DataOd = (DateTime)deStart.Date;
                    Okres.DataDo = (DateTime)deEnd.Date;
                }
                else
                {
                    deEnd.Date = null;
                }
            }
            SetCzasyNomVisible();
        }

        protected void deStart_DateChanged(object sender, EventArgs e)
        {
            if (deStart.Date != null)
            {
                String type, mies;
                int ilmies = 0;
                switch (Mode)
                {
                    case EMode.Insert:
                        Tools.GetLineParams(ddlTypOkresu.SelectedValue, out type, out mies);
                        ilmies = Tools.StrToInt(mies, 1);
                        break;
                    case EMode.Edit:
                        ilmies = Okres.IloscMiesiecy;
                        break;
                }
                deEnd.Date = ((DateTime)deStart.Date).AddMonths(ilmies).AddDays(-1);
                Okres.DataOd = (DateTime)deStart.Date;
                Okres.DataDo = (DateTime)deEnd.Date;
            }
            else
            {
                deEnd.Date = null;
            }
            SetCzasyNomVisible();
        }

        public void BindCzasyNom()
        {
            List<CzasNom> czasyNom = new List<CzasNom>();
            for (var month = Okres.DataOd; month.Date <= Okres.DataDo; month = month.AddMonths(1))
            {
                int kalendarz = Convert.ToInt32(db.Select.Scalar(dsGetCzasNom, db.strParam(Tools.DateToStrDb(month))));  //   month.ToString())));
                CzasNom czasNom = new CzasNom();
                czasNom.Data = month;
                czasNom.Kalendarz = kalendarz;
                czasNom.IsEnabled = true;
                czasyNom.Add(czasNom);
            }
            if (czasyNom.Count == 1)
                ((CzasNom)czasyNom[0]).IsEnabled = false;   // jak jest jeden to bez edycji
            Okres.CzasyNom = czasyNom;
            rpNom.DataSource = czasyNom;
            rpNom.DataBind();
        }

        public bool Update()
        {
            for (int i = 0; i < rpNom.Items.Count; i++)
            {
                int dni =  Convert.ToInt32(Tools.GetText(rpNom.Items[i], "tbNom"));

                Okres.CzasyNom[i].DniPrac = dni;
            }

            int preferedSum = Okres.CzasyNom.Sum(x => x.Kalendarz);
            int sum = Okres.CzasyNom.Sum(x => x.DniPrac.Value);


            if (preferedSum == sum)
            {
                foreach (CzasNom czas in Okres.CzasyNom)
                {
                    if (!db.Execute(dsUpdateCzas, czas.DniPrac, db.strParam(czas.Data.ToString()), Okres.Id))
                        return false;
                }
                TriggerSaved();
                Close();
                return true;
            }
            else
            {
                Tools.ShowError("Suma czasu nominalnego nie zgadza się!");
            }
            return false;
        }

        public void Insert()
        {
            for (int i = 0; i < rpNom.Items.Count; i++)
            {
                int dni = Convert.ToInt32(Tools.GetText(rpNom.Items[i], "tbNom"));

                Okres.CzasyNom[i].DniPrac = dni;
            }

            int preferedSum = Okres.CzasyNom.Sum(x => x.Kalendarz);
            int sum = Okres.CzasyNom.Sum(x => x.DniPrac.Value);

            if (preferedSum == sum)
            {
                String NewOkresId = AddOkres();
                bool good = !String.IsNullOrEmpty(NewOkresId);
                good &= AddCzasy(NewOkresId);
                if (good)
                {
                    TriggerSaved();
                    Close();
                }
                else
                {
                    Tools.ShowMessage("Wystąpił błąd!");
                }
            }
            else
            {
                Tools.ShowError("Suma czasu nominalnego nie zgadza się!");
            }
        }

        String AddOkres()
        {
            db.Execute(dsLegacyInsert, db.nullStrParam(Okres.DataOd.ToString()), db.nullStrParam(Okres.DataDo.ToString())/*, db.nullStrParam(Okres.Typ.ToString())*/);
            return db.Select.Scalar(dsInsert, db.nullStrParam(Okres.DataOd.ToString()), db.nullStrParam(Okres.DataDo.ToString()), db.nullStrParam(Okres.Typ.ToString()));
        }

        bool AddCzasy(String OkresId)
        {
            foreach (CzasNom czas in Okres.CzasyNom)
            {
                if (!db.Execute(dsAddCzas, db.strParam(czas.Data.ToString()), czas.DniPrac, OkresId))
                    return false;
            }
            return true;
        }

        public void TriggerSaved()
        {
            if (Saved != null)
                Saved(null, EventArgs.Empty);
        }

        public enum EMode { Insert, Edit };
        private EMode _mode = EMode.Insert;
        public EMode Mode
        {
            get { return (EMode)ViewState["vMode"]; }
            set { ViewState["vMode"] = value; }
        }
    }
}