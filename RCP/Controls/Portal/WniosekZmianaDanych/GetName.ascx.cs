using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Configuration;
using System.Data.Sql;
using HRRcp.App_Code;
using System.Data.SqlClient;
namespace HRRcp.Controls.WnioseZmianaDanych
{
    public partial class GetName : System.Web.UI.UserControl
    {

        public string Lvl { get; set; }
        /* user     1 - pracownik
         *          2 - Admin HR
         *               
         */
        public bool SetDDL = false;
        public const int moLines = 1;
        public const int moScreen = 2;
        public EventHandler Zoombind;
        public EventHandler GetPracId;
        public EventHandler HidenPrac;
        public bool NonVisible = false;
        private string PrepareParams2(string sql)        // podmiana parametrów @p1
        {
            if (!String.IsNullOrEmpty(sql))
            {

                AppUser user = AppUser.CreateOrGetSession();
                sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej

                sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
                sql = sql.Replace("@Login", db.strParam(user.Login));
            }
            return sql;
        }
        private string PrepareParams(string sql)        // podmiana parametrów @p1
        {
            if (!String.IsNullOrEmpty(sql))
            {

                AppUser user = AppUser.CreateOrGetSession();
                sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
                if (Lvl == "2")
                {
                    sql = sql.Replace("@KadryId2", Nrewid.Value);// to najpierw
                }
                else
                {

                    sql = sql.Replace("@KadryId2", db.strParam(user.KadryId2));// to najpierw

                }
                sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
                sql = sql.Replace("@Login", db.strParam(user.Login));
            }
            return sql;
        }
        protected void Del(object sender, EventArgs e)
        {

            if (Lvl != "2") Zoombind(1, null);
            //Listview2.Visible = false;
            ListView1.Visible = true;
            //App_Code.Tools.ShowMessage("Wniosek został skasowany !");
        }
        protected void Close(object sender, EventArgs e)
        {
            if (Lvl != "2") Zoombind(1, null); /// ja pracownik zamyka nowy to trzeba odsiezyc liste
            //Listview2.Visible = false;
            ListView1.Visible = true;
        }
        public void Query(string Id, String Type)
        {
            try
            {
                DataRow dr = db.getDataRow(String.Format("select * from {0}..SqlContent where Id = {1}", App.dbPORTAL, Id));
                string constr = db.getValue(dr, "ConStr");
                string sql = db.getValue(dr, "Sql");
                String MenuText = db.getValue(dr, "MenuText");
                int typ = db.getInt(dr, "Typ", moLines);
                int Typ = typ;
                SqlConnection con;
                    if (string.IsNullOrEmpty(constr))
                    {
                        con = db.Connect(db.conStr);
                    }
                    else
                    {
                        con = db.Connect(ConfigurationManager.ConnectionStrings[constr].ConnectionString);
                    }



                    DataSet ds = db.getDataSet(con, PrepareParams(sql));
                    db.Disconnect(con);
                bool nul = false;
                int typwniosek = 1;
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    nul = true;
                    typwniosek = 0;


                }
                switch (int.Parse(Type))
                {
                    case moLines:
                        (((GetID.FindControl("sqlcontent1") as Control).FindControl("cntMasterLines") as Control).FindControl("MenuText") as HiddenField).Value = MenuText;
                        GetID.Visible = true;
                        GetID.GetId(Id);

                        break;
                    case moScreen:
                        DataSet ds2 = db.select(string.Format(@"
insert into poWnioskiDaneOsoboweInfo (Autor,DataDod,StatusId,Typ,MenuText) OUTPUT Inserted.ID values ({0},GETDATE(),0,{1},'{2}')
                ", App.User.Id
                     , typwniosek
                     ,MenuText));
                        int Id2 = (int)ds2.Tables[0].Rows[0]["ID"];
                        int Type2 = 0;
                        String Name = "";
                        foreach (DataColumn column in ds.Tables[0].Columns)
                        {
                            Name = column.ColumnName;
                            String[] GetType = Name.Split(':');
                            if (GetType.Length - 1 < 1) { Name = GetType[0]; Type2 = 0; }
                            else
                            {
                                Name = GetType[0];
                                if (GetType[1] == "D") Type2 = 1;
                                if (GetType.Length > 2)
                                    if (GetType[2] == "-") NonVisible = true;


                            }
                            // Type 0 - literal 
                            // Type 1 - Data
                            //
                            if (NonVisible == false)
                            {
                                switch (nul)
                                {
                                    case false:
                                        db.insert(String.Format(@"
insert into poWnioskiDaneOsobowePola (WniosekId,Nazwa_pola,Obecna_wartosc,Typ) values ({0},'{1}','{2}',{3})

", Id2
                                    , Name
                                    , ds.Tables[0].Rows[0][column.ColumnName]
                                    , Type2
                                    ));
                                        break;
                                    case true:
                                        db.insert(String.Format(@"
insert into poWnioskiDaneOsobowePola (WniosekId,Nazwa_pola,Typ) values ({0},'{1}',{2})

", Id2
                                    , Name
                                    , Type2
                                    ));
                                        break;
                                }
                            }
                            else
                            {
                                NonVisible = false;
                            }
                        }
                        Listview2.Pokaz(Id2.ToString());
                        //Listview2.Visible = true;
                        Zoombind(1, null); // odswieza zoom'a
                        break;

                }


            }
            catch {
                App_Code.Tools.ShowMessage("Brak potrzebnych danych o pracowniku ! Uzupełnij ");
            }



        }




        protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownList1.SelectedIndex != 0)
            {
                sender = DropDownList1.SelectedValue;
                ListView1.Visible = true;
                (((GetID.FindControl("sqlcontent1") as Control).FindControl("cntMasterLines") as Control).FindControl("PracId") as HiddenField).Value = DropDownList1.SelectedValue;
                PracId.Value = DropDownList1.SelectedValue;
                DataRow dr = db.getDataRow(string.Format(@"select KadryId2 as NrEwid from Pracownicy where Id={0}", PracId.Value));
                Nrewid.Value = db.getValue(dr, "NrEwid");
                GetPracId(sender,null);
            }
            else
            {
                sender = " ";
                ListView1.Visible = false;
                HidenPrac(sender, null);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("Wybierz", String.Empty));
                DropDownList1.SelectedIndex = 0;
                PracId.Value = "0";
                SetDDL = true;

            }
            if (Lvl == "2")
            {

                ListView1.Visible = false;
            }
            else
            {
                paFilter.Visible = false;
            }
            SqlDataSource1.SelectCommand = string.Format("select MenuText [Nazwa] ,Id [Id] , sql from {0}..SqlContent where Aktywny=1 and Grupa='DANEP' and Wnioski=1 ", App.dbPORTAL);
            Listview2.Zoombind2 += new EventHandler(ZoombindFromLv);
            GetID.TurboClickUltimate2 += new EventHandler(TurboClickUltimate2);
            Listview2.Close1 += new EventHandler(Close);
            Listview2.del2 += new EventHandler(Del);
            if (PracId.Value != "0") ListView1.Visible = true;

        }
        protected void Page_Init(object sender, EventArgs e)
        {

            Tools.PrepareDicListView(ListView1, 0, true, false, true);

        }

        protected void ZoombindFromLv(object sender, EventArgs e)
        {
            if (Lvl != "2") Zoombind(1, null);
            ListView1.Visible = true;
            SqlDataSource1.SelectCommand = string.Format("select MenuText [Nazwa] ,Id [Id] , sql from {0}..SqlContent where Aktywny=1 and Grupa='DANEP' and Wnioski=1 ", App.dbPORTAL);
            ListView1.DataBind();



        }

        protected void TurboClickUltimate2(object sender, EventArgs e)
        {

            //GetID.Visible = false;
            //Tools.CloseDialog("DivZoom_Report");
            Listview2.Pokaz(sender.ToString());
            //Listview2.Visible = true;


            if (Lvl != "2") Zoombind(1, null);

        }

        protected void GetId(object sender, CommandEventArgs e)
        {

            if (e.CommandArgument == null)
            {

            }
            else
            {

                try
                {
                    string id = e.CommandArgument.ToString();
                    DataRow dr = db.getDataRow(String.Format("select * from {0}..SqlContent where Id = {1}", App.dbPORTAL, id));
                    string constr = db.getValue(dr, "ConStr");
                    string sql = db.getValue(dr, "Sql");
                    String MenuText = db.getValue(dr, "MenuText");

                    int typ = db.getInt(dr, "Typ", moLines);
                    int Typ = typ;
                    bool nul = false;
                    int typwniosek = 1;

                    SqlConnection con;
                    if (string.IsNullOrEmpty(constr))
                    {
                        con = db.Connect(db.conStr);
                    }
                    else
                    {
                        con = db.Connect(ConfigurationManager.ConnectionStrings[constr].ConnectionString);
                    }



                    DataSet ds = db.getDataSet(con, PrepareParams(sql));
                    db.Disconnect(con);

                    //DataSet ds = db.select(PrepareParams(sql));
                    //ListView1.Visible = false;
                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                        nul = true;
                        typwniosek = 0;


                    }
                    switch (typ)
                    {

                        default:
                        case moLines:
                            GetID.Visible = true;
                            (((GetID.FindControl("sqlcontent1") as Control).FindControl("cntMasterLines") as Control).FindControl("MenuText") as HiddenField).Value = MenuText;
                            GetID.GetId(id);
                            if (nul == true) Tools.CloseDialog("DivZoom_Report");
                            break;

                        case moScreen:
                            DataSet ds2;

                            if (Lvl == "2")
                            {
                                ds2 = db.select(string.Format(@"
insert into poWnioskiDaneOsoboweInfo (Autor,DataDod,StatusId,Typ,MenuText) OUTPUT Inserted.ID values ({0},GETDATE(),0,{1},{2})
                ", PracId.Value
                        , typwniosek
                        ,MenuText));
                            }
                            else
                            {

                                ds2 = db.select(string.Format(@"
insert into poWnioskiDaneOsoboweInfo (Autor,DataDod,StatusId,Typ,MenuText) OUTPUT Inserted.ID values ({0},GETDATE(),0,{1},'{2}')
                ", App.User.Id
                        , typwniosek
                        , MenuText));

                            }

                            int Id = (int)ds2.Tables[0].Rows[0]["ID"];
                            int Type = 0;
                            String Name = "";
                            int i = 0;

                            foreach (DataColumn column in ds.Tables[0].Columns)
                            {

                                Name = column.ColumnName;
                                String[] GetType = Name.Split(':');
                                if (GetType.Length - 1 < 1) { Name = GetType[0]; Type = 0; }
                                else
                                {
                                    Name = GetType[0];
                                    if (GetType[1] == "D") Type = 1;
                                    if (GetType.Length > 2)
                                        if (GetType[2] == "-") NonVisible = true;

                                }
                                // Type 0 - literal 
                                // Type 1 - Data
                                //

                                if (NonVisible == false)
                                {
                                    switch (nul)
                                    {
                                        case false:
                                            db.insert(String.Format(@"
insert into poWnioskiDaneOsobowePola (WniosekId,Nazwa_pola,Obecna_wartosc,Typ) values ({0},'{1}','{2}',{3})

", Id
                                        , Name
                                        , ds.Tables[0].Rows[0][column.ColumnName]
                                        , Type
                                        ));
                                            break;
                                        case true:
                                            db.insert(String.Format(@"
insert into poWnioskiDaneOsobowePola (WniosekId,Nazwa_pola,Typ) values ({0},'{1}',{2})

", Id
                                        , Name
                                        , Type
                                        ));
                                            break;
                                    }




                                }
                                else
                                {
                                    NonVisible = false;
                                }
                                i++;
                            }
                            Listview2.Pokaz(Id.ToString());
                            Listview2.Visible = true;
                            if (Lvl != "2") Zoombind(1, null); // odswieza zoom'a

                            break;

                    }



                    //Label1.Text = "";
                    //DataSet ds = db.select(PrepareParams(e.CommandArgument.ToString()));
                    //foreach (DataColumn column in ds.Tables[0].Columns)
                    //{
                    //  Label1.Text += "<br/>" + column.ColumnName.ToString();
                    //} 
                    //Label1.Text = e.CommandArgument.ToString();
                    //SqlContent.DoSelectTab_wn(e.CommandArgument.ToString());



                }
                catch {

                    App_Code.Tools.ShowMessage("Brak potrzebnych danych o pracowniku ! Uzupełnij ");
                }
            }
        }


        /*
        protected void DDL1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDL1.SelectedIndex.ToString() != "1")
            {
                SqlDataSource1.SelectCommand = "select MenuText [Nazwa] ,Id [Id] , sql from SqlContent where Aktywny=1 and Grupa='DANEP' and Typ!=2";
                
            }
            else {
                SqlDataSource1.SelectCommand = "select MenuText [Nazwa] ,Id [Id] , sql from SqlContent where Aktywny=1 and Grupa='DANEP'";
              
            }
            ListView1.DataBind();  
        }

*/



    }
}