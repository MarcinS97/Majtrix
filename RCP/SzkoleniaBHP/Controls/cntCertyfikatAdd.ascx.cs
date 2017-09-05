using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;
using HRRcp.Controls;
using HRRcp.SzkoleniaBHP.Controls;

namespace HRRcp.SzkoleniaBHP.Controls
{
    public partial class cntCertyfikatAdd : System.Web.UI.UserControl
    {
        const int po0                           = 0;
        const int po1                           = 1;
        const int poNumer                       = 2;
        const int poDataWaznosci                = 3;
        const int po4                           = 4;
        const int poKategoria                   = 5;
        const int poDataZdobycia                = 6;
        const int poDataWaznosciPsychotestow    = 7;
        const int poDataWaznosciBadanLekarskich = 8;
        const int poDataWaznosciUmowy           = 9;
        const int poUmowaLojalnosciowa          = 10;
        const int po11                          = 11;
        const int po12                          = 12;
        const int poUwagi                       = 13;
        //----- asseco -----
        const int poDataRozpoczecia             = 14;
        const int poDataZakonczenia             = 15;    
        const int poNazwa                       = 16;
        const int poDodatkoweWarunki            = 17;
        const int poSymbol                      = 18;

        const int poLastId = 18;
        const int deMaxId = 7;         // ilość DateEdit

        //0123456789012345678
        //1111111011111111111
        //0011000000000111111




        protected void Page_Init(object sender, EventArgs e)
        {
            //Grid.Prepare(DetailsView1, "DetailsView1");
            //Grid.Prepare(GridView1, "GridView2");

            Tools.PrepareDicListView(lvCertyfikat, 0);
            Tools.PrepareSorting(lvCertyfikat, 4, 20);

            //http://www.codeproject.com/Articles/26039/ListView-Header-Sort-Direction-Indicators
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TranslatePage();
            }
        }

        private void TranslatePage()
        {
            //L.p(Label1);
            //L.p(Label2);
            //L.p(Label3);
            //L.p(Label4);
            //L.p(Label5);
            //L.p(Label6);
        }

        private void TranslateLV()
        {
            L.p(lvCertyfikat, "LinkButton", 1, 10);
            L.p(lvCertyfikat, "lbCountLabel");
        }

        public void Prepare(string certId, string pracId, string uprId, string typId, bool editable)
        {
            Changed = false;
            Editable = editable;
            hidCertId.Value = certId;
            hidTypId.Value = typId;
            Typ = typId;
            ShowInsert = true;
            if (String.IsNullOrEmpty(certId))
            {
                //----- create -----
                hidUprId.Value = uprId;
                hidPracId.Value = pracId;
                //paHistory.Visible = false;
                //DetailsView1.ChangeMode(DetailsViewMode.Insert);
            }
            else
            {
                //----- create -----
                if (String.IsNullOrEmpty(pracId))
                {
                    DataRow dr = db.getDataRow("select * from Certyfikaty where Id = " + certId);
                    pracId = db.getValue(dr, "IdPracownika");
                    uprId = db.getValue(dr, "IdUprawnienia");
                }
                hidUprId.Value = uprId;
                hidPracId.Value = pracId;

                //paHistory.Visible = true;

                //GridView1.DataBind();
                //if (GridView1.Rows.Count > 0)
                //    GridView1.SelectedIndex = 0;
            }
            string pola;
            cntCertyfikatHeader.Prepare(typId, hidUprId.Value, hidPracId.Value, out pola);
            hidPola.Value = pola;

            lvCertyfikat.InsertItemPosition = InsertItemPosition.None;
            lvCertyfikat.EditIndex = -1;
            lvCertyfikat.SelectedIndex = -1;
            
            if (IsEdit)
            {
                cntCertyfikatHeader.Visible = false;
                paFilter.Visible = true;
                deNadzien.Date = DateTime.Today;
                lvCertyfikat.InsertItemPosition = InsertItemPosition.FirstItem;
            }
            else
            {
                cntCertyfikatHeader.Visible = true;
                paFilter.Visible = false;
            }
            Visible = true;
        }

        public void Cancel()
        {
            Visible = false;   // jak były 2 kontrolki to sie wywalało ze niezgodne tree kontrolek - ustawiam na wejście
            //hidPracId.Value = null; // databind na pusto
            //lvCertyfikat.InsertItemPosition = InsertItemPosition.None;
            //lvCertyfikat.EditIndex = -1;
            //lvCertyfikat.SelectedIndex = -1;
        }

        public void Close()
        {
            //Visible = false;
            //hidPracId.Value = null; // databind na pusto
            //lvCertyfikat.InsertItemPosition = InsertItemPosition.None;
            //lvCertyfikat.EditIndex = -1;
            //lvCertyfikat.SelectedIndex = -1;
        }
        //-----------------------------------------

        public bool IsVisible(int pos)
        {
            if (CheckBox1.Checked) return true;
            
            string p = hidPola.Value;
            if (!String.IsNullOrEmpty(p))
            {
                return pos < p.Length ? p[pos] != '0' : false;
            }
            else
                return true;
        }

        public bool IsAktualnyVisible
        {
            get { return hidAktualnyVisible.Value == "1"; }
        }

        public string GetEditColSpan(int cs1, int cs2)   // celka na tabelkę przy edycji 
        {
            return (IsEdit ? cs1 : cs2).ToString();
        }

        public bool GetIsEdit()
        {
            return IsEdit;
        }
        //-----------------------------------------
        protected void lvCertyfikat_LayoutCreated(object sender, EventArgs e)
        {
            TranslateLV();
            if (_Szkoleniap)
            {
                LinkButton LinkButton3 = lvCertyfikat.FindControl("LinkButton3") as LinkButton;
                if(LinkButton3 != null)
                    LinkButton3.Text = L.p("Termin przeprowadzenia");
            }
            if (_Statusyp)
            {
                LinkButton LinkButton3 = lvCertyfikat.FindControl("LinkButton3") as LinkButton;
                if(LinkButton3 != null)
                    LinkButton3.Text = L.p("Data utracenia");

                LinkButton LinkButton4 = lvCertyfikat.FindControl("LinkButton4") as LinkButton;
                if (LinkButton4 != null)
                    LinkButton4.Text = L.p("Utracony do");
            }
        }

        private bool ShowInsert
        {
            set { ViewState["shins"] = value; }
            get { return Tools.GetBool(ViewState["shins"], true); }
        }

        protected void lvCertyfikat_DataBound(object sender, EventArgs e)
        {
            if (Editable)
            {
                if (lvCertyfikat.Items.Count == 0)
                {
                    if (ShowInsert)
                        lvCertyfikat.InsertItemPosition = InsertItemPosition.FirstItem;
                }
                else
                {
                    if (lvCertyfikat.InsertItemPosition == InsertItemPosition.None && lvCertyfikat.EditIndex == -1)
                        Tools.SetControlVisible(lvCertyfikat, "InsertButton", true);      // bo się nie pokazywał jak najpierw wejscie Count=0 i podem edycja istniejacego
                    ShowInsert = false;
                }
            }
            else
            {
                if (lvCertyfikat.Items.Count == 0)
                    Tools.SetControlVisible(lvCertyfikat.Controls[0], "btNewRecord", false);
            }

            for (int i = 0; i < poLastId; i++)
                Tools.SetControlVisible(lvCertyfikat, "th" + i.ToString(), IsVisible(i));
            Tools.SetControlVisible(lvCertyfikat, "thControl", Editable);
            Tools.SetControlVisible(lvCertyfikat, "thAktualny", IsAktualnyVisible);
            Tools.SetControlVisible(lvCertyfikat, "thPracownik", IsEdit);
            Tools.SetControlVisible(lvCertyfikat, "thNrEw", IsEdit);

            bool ed = IsEdit;
            Control item = null;
            if (lvCertyfikat.InsertItemPosition != InsertItemPosition.None)
            {
                item = lvCertyfikat.InsertItem;
                /*
                if (item != null)
                    if (ed)
                    {
                        Tools.SelectItem(item, "ddlSymbol", LastSymbol);
                    }
                    else
                    {
                        Tools.SelectItem(item, "ddlSymbol", hidUprId.Value);
                    }
                 */ 
                //Tools.PreventReClick(item, "InsertButton");
            }
            else if (lvCertyfikat.EditIndex != -1)
            {
                item = lvCertyfikat.EditItem;
                //Tools.PreventReClick(item, "UpdateButton");
            }
            if (item != null)
            {
                Tools.SetControlVisible(item, "paPracownik", ed);
                PrepareJs(item, "tbNrEw", "ddlPracownik", "ddlSymbol", "tbNazwa");  // UWAGA !!! nie może być w OnItemCreate bo ustawienie Attributes[] coś psuje z postbackami - nie działa Oncommand
            }
        }

        protected void ddlSymbol_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (IsEdit)
            {
                Tools.SelectItemByParam(ddl, 0, LastSymbolId);
            }
            else
            {
                Tools.SelectItemByParam(ddl, 0, hidUprId.Value);
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            lvCertyfikat.DataBind();
        }

        private void PrepareJs(Control cnt, string idNrEw, string idPracownik, string idSymbol, string idNazwa)
        {
            TextBox tbNrEw = cnt.FindControl(idNrEw) as TextBox;
            DropDownList ddlPracownik = cnt.FindControl(idPracownik) as DropDownList;
            if (tbNrEw != null && ddlPracownik != null)
            {
                ddlPracownik.Attributes["onchange"] = String.Format("{0}_click(this,'{1}');", ClientID, tbNrEw.ClientID);
                Tools.ExecOnStart2("setjs", String.Format("{0}_setchange('{1}','{2}');", ClientID, tbNrEw.ClientID, ddlPracownik.ClientID));
            }
            DropDownList ddlSymbol = cnt.FindControl(idSymbol) as DropDownList;
            TextBox tbNazwa = cnt.FindControl(idNazwa) as TextBox;
            if (ddlSymbol != null && tbNazwa != null)
            {
                ddlSymbol.Attributes["onchange"] = String.Format("{0}_click(this,'{1}');", ClientID, tbNazwa.ClientID);
            }
        }

        protected void lvCertyfikat_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            bool insert = e.Item.ItemType == ListViewItemType.InsertItem;
            bool data = e.Item.ItemType == ListViewItemType.DataItem;
            bool edit = data && ((ListViewDataItem)e.Item).DisplayIndex == lvCertyfikat.EditIndex;

            if (insert || edit)
            {                
                for (int i = 0; i < poLastId; i++)
                    Tools.SetControlVisible(e.Item, "td" + i.ToString(), IsVisible(i));

                if (_Statusyp)
                    if (e.Item.ItemType == ListViewItemType.InsertItem)
                    {
                        DateEdit DateEdit1 = lvCertyfikat.InsertItem.FindControl("DateEdit1") as DateEdit;
                        DateEdit DateEdit2 = lvCertyfikat.InsertItem.FindControl("DateEdit2") as DateEdit;
                        if (DateEdit1 != null && DateEdit2 != null)
                        {
                            //DateEdit2.Date = DateTime.Now;
                            //DateEdit1.Date = DateTime.Now.AddMonths(1);
                            DateEdit2.Date = DateTime.Today;
                            DateEdit1.Date = DateTime.Today.AddDays(30 - 1);
                            DateEdit1.ValidationGroup = "ivg";
                            DateEdit2.ValidationGroup = "ivg";
                        }
                    }
                    else   // edit
                    {
                        DateEdit DateEdit1 = e.Item.FindControl("DateEdit1") as DateEdit;
                        DateEdit DateEdit2 = e.Item.FindControl("DateEdit2") as DateEdit;
                        if (DateEdit1 != null && DateEdit2 != null)
                        {
                            DateEdit1.ValidationGroup = "evg";
                            DateEdit2.ValidationGroup = "evg";
                        }
                    }
                //PrepareJs(e.Item, "tbNrEw", "ddlPracownik", "ddlSymbol111", "tbNazwa");
            }
            if (insert || data)
            {
                Tools.SetControlVisible(e.Item, "tdPracownik", IsEdit);
                Tools.SetControlVisible(e.Item, "tdNrEw", IsEdit);
                Tools.SetControlVisible(e.Item, "tdAktualny", IsAktualnyVisible);
            }
            if (insert)
            {
                if (!IsEdit)
                {
                    Tools.SetText2(e.Item, "lbSymbol", cntCertyfikatHeader.SymbolUprawnienia);
                    Tools.SetText2(e.Item, "tbNazwa", cntCertyfikatHeader.NazwaUprawnienia);
                }
            }
            if (data)
            {
                Tools.SetControlVisible(e.Item, "tdControl", Editable);
            }
        }

        public string LastSymbolId
        {
            set { Session["lastsym"] = value; }
            get { return Tools.GetStr(Session["lastsym"]); }
        }
        //------------------------------------------
        private void UpdateDataWaznosci(bool insert)
        {
            string t = cntCertyfikatHeader.OkresWaznosciTyp;
            int d = cntCertyfikatHeader.OkresWaznosci;
            if (!String.IsNullOrEmpty(t) && d != -1)
            {
                DateEdit deWazn;
                DateEdit deRozp;
                DateEdit deZak;
                if (insert)
                {
                    deWazn = lvCertyfikat.InsertItem.FindControl("DateEdit1") as DateEdit;
                    deRozp = lvCertyfikat.InsertItem.FindControl("DateEdit6") as DateEdit;
                    deZak = lvCertyfikat.InsertItem.FindControl("DateEdit7") as DateEdit;
                }
                else
                {
                    deWazn = lvCertyfikat.EditItem.FindControl("DateEdit1") as DateEdit;
                    deRozp = lvCertyfikat.EditItem.FindControl("DateEdit6") as DateEdit;
                    deZak = lvCertyfikat.EditItem.FindControl("DateEdit7") as DateEdit;
                }
                if (deWazn != null && deRozp != null && deZak != null)
                {
                    object dt = null;
                    if (deZak.IsValid)
                        dt = deZak.Date;
                    else if (deRozp.IsValid)
                        dt = deRozp.Date;
                    else return;
                    switch (t)
                    {
                        case "Y":
                            deWazn.Date = ((DateTime)dt).AddYears(d).AddDays(-1);
                            break;
                        case "M":
                            deWazn.Date = ((DateTime)dt).AddMonths(d).AddDays(-1);
                            break;
                        case "D":
                            deWazn.Date = ((DateTime)dt).AddDays(d);
                            break;
                    }
                }
            }
        }

        protected void deRozpInsert_Changed(object sender, EventArgs e)
        {
            UpdateDataWaznosci(true);
        }

        protected void deRozpUpdate_Changed(object sender, EventArgs e)
        {
            UpdateDataWaznosci(false);
        }
        //------------------------------------------
        string insertId = null;

        private bool AssecoInsert(ListViewInsertEventArgs e)
        {
            /*
            string logo;
            string symbol;
            if (IsEdit)
            {
                logo = Tools.GetDdlSelectedValue(e.Item, "ddlPracownik", 1);    // Id|KadryId
                string uid = Tools.GetDdlSelectedValue(e.Item, "ddlSymbol", 0); // Id|Nazwa
                symbol = db.getScalar(String.Format("select Symbol from Uprawnienia where Id = {0}", uid));
            }
            else
            {
                logo = db.getScalar(String.Format("select KadryId from Pracownicy where Id = {0}", hidPracId.Value));
                symbol = db.getScalar(String.Format("select Symbol from Uprawnienia where Id = {0}", hidUprId.Value));
            }
             */
            string logo   = db.getScalar(String.Format("select KadryId from Pracownicy where Id = {0}", e.Values["IdPracownika"].ToString()));
            string symbol = db.getScalar(String.Format("select Symbol from Uprawnienia where Id = {0}", e.Values["IdUprawnienia"].ToString()));
            DataSet ds;
            bool ok = Asseco.Exec(Log.EXPORT_ASSECO_BHP, "Asseco.ExportBHP.Insert", "Logo: {1}, Kurs: {2}, Data: {4}", "Wystąpił błąd podczas eksportu szkolenia: {0}", true, out ds, AssecoSql.InsertCommand,
                null,
                db.strParam(logo), 
                db.strParam(symbol),
                db.nullParamStr(e.Values["NazwaCertyfikatu"]),
                db.nullParamStr(Tools.DateToStr(e.Values["DataRozpoczecia"])),
                db.nullParamStr(Tools.DateToStr(e.Values["DataZakonczenia"])),
                db.nullParamStr(Tools.DateToStr(e.Values["DataWaznosci"])),
                db.nullParamStr(e.Values["Uwagi"]),
                db.nullParamStr(e.Values["Numer"]),
                db.nullParamStr(e.Values["DodatkoweWarunki"]),
                db.strParam("import RCP")
                );
            if (ok)
                insertId = db.getValue(db.getRow(ds), "lp_KursyId");
            return ok;
        }

        private bool AssecoUpdate(ListViewUpdateEventArgs e)
        {
            //string logo = db.getScalar(String.Format("select KadryId from Pracownicy where Id = {0}", hidPracId.Value));
            //string symbol = db.getScalar(String.Format("select Symbol from Uprawnienia where Id = {0}", hidUprId.Value));
            string logo   = db.getScalar(String.Format("select KadryId from Pracownicy where Id = {0}", e.NewValues["IdPracownika"].ToString()));
            string symbol = db.getScalar(String.Format("select Symbol from Uprawnienia where Id = {0}", e.NewValues["IdUprawnienia"].ToString()));
            string id = Tools.GetDataKey(lvCertyfikat, e);

            DataSet ds;
            bool ok = Asseco.Exec(Log.EXPORT_ASSECO_BHP, "Asseco.ExportBHP.Update", "Id: {0}, Logo: {1}, Kurs: {2}, Data: {4}", "Wystąpił błąd podczas eksportu szkolenia: {0}", true, out ds, AssecoSql.UpdateCommand,
                id,
                db.strParam(logo),
                db.strParam(symbol),
                db.nullParamStr(e.NewValues["NazwaCertyfikatu"]),
                db.nullParamStr(Tools.DateToStr(e.NewValues["DataRozpoczecia"])),
                db.nullParamStr(Tools.DateToStr(e.NewValues["DataZakonczenia"])),
                db.nullParamStr(Tools.DateToStr(e.NewValues["DataWaznosci"])),
                db.nullParamStr(e.NewValues["Uwagi"]),
                db.nullParamStr(e.NewValues["Numer"]),
                db.nullParamStr(e.NewValues["DodatkoweWarunki"]),
                db.strParam("update RCP")
                );
            return ok;
        }

        private bool AssecoDelete(ListViewDeleteEventArgs e)
        {
            DataSet ds;
            string id = Tools.GetDataKey(lvCertyfikat, e);
            bool ok = Asseco.Exec(Log.EXPORT_ASSECO_BHP, "Asseco.ExportBHP.Delete", "Id: {0}, ", "Wystąpił błąd podczas usuwania szkolenia: {0}", true, out ds, AssecoSql.DeleteCommand,
                id
                );
            return ok;
        }

        private bool AssecoChangeId(string oldId, string newId)
        {
            return db.execSQL(String.Format(AssecoSql.SelectCommand, oldId, newId));  //powinno zwrócić 1 elbo exception
        }
        //------------------------------------------

        public bool Changed
        {
            set { ViewState["chg"] = value; }
            get { return Tools.GetBool(ViewState["chg"], false); }
        }

        public bool Editable
        {
            set { ViewState["editable"] = value; }
            get { return Tools.GetBool(ViewState["editable"], false); }
        }

        protected void lvCertyfikat_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.InsertItem)
            {
                L.p(e.Item, "RequiredFieldValidator1");
                //Tools.PreventReClick(e.Item, "DeleteButton");
            }
            if (e.Item.ItemType == ListViewItemType.DataItem && ((ListViewDataItem)e.Item).DisplayIndex == lvCertyfikat.EditIndex)
            {
                DataRowView drv = Tools.GetDataRowView(e);
                bool ed = IsEdit;
                if (ed)
                {
                    Tools.SelectItemByParam(e.Item, "ddlPracownik", 0, drv["IdPracownika"]);
                }
                Tools.SelectItemByParam(e.Item, "ddlSymbol", 0, drv["IdUprawnienia"]);
                Tools.SetControlEnabled(e.Item, "ddlSymbol", ed);   // dla mode = 0 nie ma zmiany
            }
        }

        private bool Validate(Control item, IOrderedDictionary values)
        {
            bool v = true;
            for (int i = 1; i <= deMaxId; i++)
            {
                DateEdit de = item.FindControl("DateEdit" + i.ToString()) as DateEdit;
                if (de != null)
                    if (de.Visible && !String.IsNullOrEmpty(de.DateStr))
                        if (!de.Validate())
                            v = false;
            }
            return v;
        }

        protected void lvCertyfikat_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Values["IdAutora"] = App.User.Id;
            e.Values["IdAutoraZast"] = App.User.OriginalId;
            if (IsEdit)
            {
                e.Values["IdPracownika"] = Tools.GetDdlSelectedValueInt(e.Item, "ddlPracownik", 0);
                int? uid = Tools.GetDdlSelectedValueInt(e.Item, "ddlSymbol", 0);
                e.Values["IdUprawnienia"] = uid;
                LastSymbolId = uid == null ? null : uid.ToString();
            }
            else
            {
                e.Values["IdPracownika"] = hidPracId.Value;
                e.Values["IdUprawnienia"] = hidUprId.Value;
            }
            bool ok = Validate(e.Item, e.Values);
            if (ok) ok = AssecoInsert(e);
            e.Cancel = !ok;
        }
        
        protected void lvCertyfikat_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.NewValues["DataModyfikacji"] = DateTime.Now;
            e.NewValues["IdAutora"] = App.User.Id;
            e.NewValues["IdAutoraZast"] = App.User.OriginalId;
            if (IsEdit)
            {
                //e.NewValues["IdPracownika"] = Tools.GetDdlSelectedValueInt(lvCertyfikat.EditItem, "ddlPracownik", 0);
                e.NewValues["IdUprawnienia"] = Tools.GetDdlSelectedValueInt(lvCertyfikat.EditItem, "ddlSymbol", 0);
                e.NewValues["IdPracownika"] = Tools.GetText(lvCertyfikat.EditItem, "hidPracId");
                //e.NewValues["IdUprawnienia"] = Tools.GetText(lvCertyfikat.EditItem, "hidUprId");
            }
            else
            {
                e.NewValues["IdPracownika"] = hidPracId.Value;
                e.NewValues["IdUprawnienia"] = hidUprId.Value;
            }
            bool ok = Validate(lvCertyfikat.EditItem, e.NewValues);
            if (ok) ok = AssecoUpdate(e);
            e.Cancel = !ok;
        }

        protected void lvCertyfikat_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            bool ok = AssecoDelete(e);
            e.Cancel = !ok;
        }
        //-----
        protected void lvCertyfikat_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            Changed = true;
        }

        protected void lvCertyfikat_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            Changed = true;
        }

        protected void lvCertyfikat_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            Changed = true;
        }
        //-----
        protected void SqlDataSource3_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                System.Data.Common.DbCommand command = e.Command;
                string certId = command.Parameters["@IdCertyfikatu"].Value.ToString();
                if (!String.IsNullOrEmpty(certId) && !String.IsNullOrEmpty(insertId))
                {
                    bool ok = AssecoChangeId(certId, insertId);
                }
            }
        }
        //-----
        protected void btShow_Click(object sender, EventArgs e)
        {

        }

        protected void deNaDzien_Changed(object sender, EventArgs e)
        {

        }
        //-----------------------------------------------------
        public string Typ
        {
            set { ViewState["typid"] = value; }
            get { return Tools.GetStr(ViewState["typid"]); }
        }

        protected Boolean _Szkoleniap
        {
            get
            {
                try
                {
                    return Typ == cntUprawnienia.utSzkolenia;
                    //return Lic.Szkolenia && Request.QueryString["t"] == cntUprawnienia.utSzkolenia;
                }
                catch
                {
                    return false;
                }
            }
        }
        protected Boolean _Statusyp
        {
            get
            {
                try
                {
                    return Lic.StatusSamokontroli && Request.QueryString["t"] == cntUprawnienia.utStatusSamokontroli;
                }
                catch
                {
                    return false;
                }
            }
        }


        //------------------------------------------
        /*
        private void EnableControls(ListView lv, bool edit, bool insert)
        {
            if (edit)
            {
                Tools.SetControlEnabled(lv, "btNewRecord", false);
            }
            else
            {
                Tools.SetControlEnabled(lv, "btNewRecord", true);
                lv.EditIndex = -1;
            }
            if (insert)
            {
                Tools.SetControlEnabled(lv, "btNewRecord", false);
                lv.InsertItemPosition = InsertItemPosition.FirstItem;
            }
            else
            {
                lv.InsertItemPosition = InsertItemPosition.None;
                Tools.SetControlEnabled(lv, "btNewRecord", true);
                Tools.SetControlVisible(lv, "InsertButton", true);  // na empty data template ?
            }
        }


        protected void lvCertyfikat_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView lv = lvCertyfikat;
            switch (e.CommandName)
            {
                case "NewRecord":
                    EnableControls(lv, false, true);
                    break;
                case "Edit":
                    EnableControls(lv, true, false);
                    break;
                case "CancelInsert":
                    EnableControls(lv, false, false);
                    break;
                //----- custom -----



            }
        }
        */
        public bool IsEdit
        {
            //get { return true; }
            get { return Mode == 1; }
        }

        public int Mode
        {
            get;
            set;
        }
        /*
        public SqlDataSource AssecoSql
        {
            get
            {
                cntCertyfikat3 c3 = Parent.FindControl("cntCertyfikat") as cntCertyfikat3;
                if (c3 != null)
                    return c3.AssecoSql;
                else
                    return null;
            }
        }
        */
        public SqlDataSource AssecoSql
        {
            get { return dsAssecoSql; }
        }

        public ListView List
        {
            get { return lvCertyfikat; }
        }

        public SqlDataSource DataSource
        {
            get { return SqlDataSource3; }
        }
    }
}