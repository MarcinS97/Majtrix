using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using HRRcp.App_Code;
using HRRcp.SzkoleniaBHP.Controls;


// chcekced dropdownlist, DataTable jako DataSource
//http://www.dotnetspeaks.com/DisplayArticle.aspx?ID=63


namespace HRRcp.SzkoleniaBHP.Controls
{

    
    //szkolenia VC - zmiany w sql dokończyć
    
    
    public partial class cntUprawnienia : System.Web.UI.UserControl
    {
        public const string utProdukcyjne       = "1";
        public const string utNieprodukcyjne    = "2";
        public const string utProdNiep          = "3";   //utProdukcyjne + utNieprodukcyjne;
        public const string utPassSpaw          = "4";
        public const string utElektryczne       = "8";

        public const string utOther             = "9";

        public const string utSzkolenia          = "1024";    //VC    
        public const string utStatusSamokontroli = "2048";    //VC

        public const int utProdukcyjneInt       = 1;
        public const int utNieprodukcyjneInt    = 2;
        public const int utPassSpawInt          = 4;
        public const int utElektryczneInt       = 8;
        public const int utSzkoleniaInt         = 1024;
        public const int utStatusSamokontroliInt= 2048;

        const string schASSECO = "ASSECO";
        const string schIMPORT = "IMPORT_SZKOLENIABHP";
        const string lockIMPORT = "IMPORTBHP";
        
        public const string ALL = "-99";

        private bool showStrKier = true;    // pokazuj str org i info o kierowniku
        private bool showBrak = true;       // pokaz linie podsumowania brak uprawnień/szkoleń

        const int defMonit = 30;            // dni         
        
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvUprawnienia, 0);
            Tools.PrepareSorting(lvUprawnienia, 1, 50);
            //Tools.PrepareRights(null, rights, AppUser.maxRight, 2);

            
            if (Szkoleniap)
                showBrak = true;
        }

        private bool CanEdit
        {
            set { ViewState["caned"] = value; }
            get { return Tools.GetBool(ViewState["caned"], false); }
        }

        public static bool GetRights(string typ, out bool access, out bool all, out bool edit)
        {
//#if IQOR || KDR
#if (RCP || PORTAL) && (CO || IQOR || KDR || PRON)
            bool adm = App.User.HasRight(AppUser.rSzkoleniaBHPAdm);
            all = adm;
            edit = adm;
            access = App.User.HasRight(AppUser.rSzkoleniaBHP) || adm; 
//#elif SIEMENS || MS || DBW
#elif (RCP || MS)
            all = false;
            edit = false;
            access = false;
#elif SPX
            all = false;
            edit = false;
            access = false;
#else
            bool adm = App.User.IsAdmin;
            all  = false;
            edit = false;
            switch (typ)
            {
                case utElektryczne:
                    access = Lic.UprElektryczne;
                    if (access)
                    {
                        all  = adm || App.User.HasRight(AppUser.rAllElektryczne);
                        edit = true;
                    }
                    break;
                case utPassSpaw:
                    access = Lic.PassSpaw;
                    if (access)
                    {
                        all  = adm || App.User.HasRight(AppUser.rAllSpawalnicze);
                        edit = true;
                    }
                    break;
                case utProdukcyjne:
                case utNieprodukcyjne:
                case utProdNiep:
    #if SPX
                    access = true;
                    all  = adm;
                    edit = true;
    #else
                    access = adm || App.User.HasRight(AppUser.rUprawnienia);
                    if (access)
                    {
                        all  = adm || App.User.HasRight(AppUser.rUprawnieniaAll);
                        edit = adm || App.User.HasRight(AppUser.rUprawnieniaEdit);
                    }
    #endif
                    break;
                case utSzkolenia:
                    access = Lic.Szkolenia && (adm || App.User.HasRight(AppUser.rSzkolenia));
                    if (access)
                    {
                        all  = adm || App.User.HasRight(AppUser.rSzkoleniaAll);
                        edit = adm || App.User.HasRight(AppUser.rSzkoleniaEdit);
                    }
                    break;
                case utStatusSamokontroli:
                    access = Lic.StatusSamokontroli && (adm || App.User.HasRight(AppUser.rSK));
                    if (access)
                    {
                        all  = adm || App.User.HasRight(AppUser.rSKAll);
                        edit = adm || App.User.HasRight(AppUser.rSKEdit);
                    }
                    break;
                default:
                    access = false;
                    break;
            }
#endif
            return access;
        }

        bool v = false;

        private void InitDateMonit()
        {
            DateTime dt = DateTime.Today;
            Data = dt;
            hidFilter.Value = ALL;
            Monit = defMonit;
            deNaDzien.Date = Data;
            tbMonit.Text = Monit.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //hidShowStrKier.Value = showStrKier ? "1" : "0";

                //DateTime dt = DateTime.Today;
                //Data = dt;
                //----- struktura -----
                bool access, showall, canedit;
                if (GetRights(Typ, out access, out showall, out canedit))
                {
                    CanEdit = canedit;

                    hidKierId.Value = showall ? ALL : App.User.Id;

                    /*                    
                    cntStrSelect._Prepare(App.User.Id, true, App.User.IsAdmin || showall, Tools.DateToStrDb(dt));
                    //hidStrOrg.Value = cntStrSelect.StrOrgList;
                    hidStatus.Value = cntStrSelect.Zakres;
                    //hidKierId.Value = cntStrSelect.KierId == LiniaSelect2.ALL ? null : cntStrSelect.KierId;
                    hidKierId.Value = cntStrSelect.KierId == LiniaSelect2.ALL ? "-99" : cntStrSelect.KierId;
                    */
                    
                    //----- uprawnienia -----
                    const string kwSql = "select case when @lang = 'PL' then Nazwa else NazwaEN end as Nazwa, Id from UprawnieniaKwalifikacje where Typ = {0} and Aktywna = 1 order by Kolejnosc";
                    if (String.IsNullOrEmpty(hidTyp.Value)) Typ = utProdukcyjne;

                    bool k = Typ == utPassSpaw;
                    switch (Typ)
                    {
                        case utProdukcyjne:
                        case utNieprodukcyjne:
                            cntSqlKwalifikacje.Prepare(String.Format(kwSql, utProdukcyjne));
                            cntSqlKwalifikacje.Visible = false;  // zawsze zaznaczy 1 zakładkę
                            break;
                        case utPassSpaw:
                            cbPassSpaw.Visible = true;
                            cbPassSpaw.Checked = true; // domyślnie włączone
                            cntSqlKwalifikacje.Visible = true;
                            cntSqlKwalifikacje.Prepare(String.Format(kwSql, utPassSpaw));
                            break;
                        case utElektryczne:
                            cntSqlKwalifikacje.Visible = true;
                            cntSqlKwalifikacje.Prepare(String.Format(kwSql, utElektryczne));
                            break;
                        default:
                            int cnt = cntSqlKwalifikacje.Prepare(String.Format(kwSql, Typ));   // wyżej jest sprawdzane czy na pewno jest to int
                            cntSqlKwalifikacje.Visible = cnt > 1;
                            break;
                    }
                    hidKwal.Value = cntSqlKwalifikacje.SelectedValue;
                    //----- szkolenia -----
                    if (Szkoleniap)
                    {
                        tabFilter.Items[1].Text = L.p("Aktualne szkolenia");
                        tabFilter.Items[3].Text = L.p("Nieaktualne");
                        cbWithOnly.Text = L.p("Pokaż tylko posiadających szkolenia");
                    }
                    /*
                    cntSqlKwalifikacje.Visible = k;
                    if (k)
                    {
                        cntSqlKwalifikacje.DataBind();
                        hidKwal.Value = cntSqlKwalifikacje.SelectedValue;
                    }
                    else hidKwal.Value = ALL;
                    */


                    //----- filtr -----
                    InitDateMonit();
                    //----- wyszukaj -----
                    DoSearch(true);     // ustawienie filtra domyślnego

                    if (Lic.UprExcel)
                    {
                        //paExcel.Visible = true;
                        //btExcel.Attributes["onclick"] = String.Format("javascript:exportExcel('{0}');return true;", hidReport.ClientID);
                        btExcel.Attributes["onclick"] = String.Format("javascript:exportExcel('{0}');", hidReport.ClientID);    // po onclick jest dokładany __postback, jak return to się nie wywoła przy usesubmitbehavior = false - jQuery uwaga w ascx
                    }

                    bool adm = App.User.HasRight(AppUser.rSzkoleniaBHPAdm);
                    btAdd.Visible = canedit;
                    btAdmin.Visible = adm;
                    btImport.Visible = adm;
                    if (adm)
                        Tools.MakeConfirmButton(btImport, "Potwierdź import szkoleń.");

                    btAbsDl.Visible = App.User.HasRight(AppUser.rAbsencjeDlugotrwale);

                    TranslatePage();
                }
                else
                    //App.ShowNoAccess(String.Format("Uprawnienia ({0})", Typ), App.User);
                    App.ShowNoAccess("Szkolenia BHP", App.User);
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
            
            //L.p(lvUprawnienia, "DataPager1");
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }

        public int GetColSpan(int no)
        {
            int cs;
            switch (no)
            {
                case 1:
                    cs = Typ == utPassSpaw ? 3 : 2;
                    break;
                default:
                    cs = 3;
                    break;
            }
            if (GetStrKierVisible())
                cs += 2;

            cs += 4;  // data zatr, zwol, cc, lokalizacja

            return cs;
            //return cs.ToString();
        }
        
        //--------------------

        public string Lp(string msg)
        {
            if (!IsPostBack)
                return L.p(msg);
            else
                return msg;
        }

        private void TranslatePage()
        {
            L.p(lbSearch);
            L.p(btClear);
            L.p(lbNaDzien);
            L.p(lbMonit);
            L.p(btShow);

            L.p(btClose);
            L.p(btExcel);

            L.p(tabFilter);
        }

        private void TranslateLV()
        {
            //----- tłumaczenie kontrolek nagłówkowych -----
            for (int i = 1; i <= 6; i++)
                L.p(lvUprawnienia, "Label" + i.ToString());
            for (int i = 1; i <= 5; i++)
                L.p(lvUprawnienia, "LinkButton" + i.ToString());

            L.p(lvUprawnienia, "lbCountLabel");
            L.p(lvUprawnienia, "lbPageSize");

            //L.p(lvUprawnienia, "ddlLines");
            //L.p(lvUprawnienia, "DataPager1");
        }
        
        //--------------------
        public bool IsTyp(int typ)
        {
            return Tools.StrToInt(Typ, -1) == typ;
        }
        //-----
        public string GetItem(object item, int idx)
        {
            if (item != null)
                return Tools.GetLineParam(item.ToString(), idx);
            else
                return null;
        }
        //-----
        public string GetHeaderClass()
        {
            uprCounter++;
            if (uprCounter == uprCount)
                return " " + divR;
            else
                return null;
        }

        public string GetData(object item)     //data|certId|css|pracId|uprId
        {
            //string cid, data;
            //Tools.GetLineParams(item.ToString(), out data, out cid);

            string cid, pid, uid, css, data, u1, u2, u3, data2, skip1, skip2;
            Tools.GetLineParams(item.ToString(), out data, out cid, out css, out pid, out uid, out u1, out u2, out data2, out skip1, out skip2);
            
            if (cid != "x")
            {
                return data == "x" ? L.p("bezterminowo") : data2;
            }
            else
            {
                return L.p("Dodaj");
            }
        }

        public string GetDataCss(object item)    
        {
            string cid, data, css;
            Tools.GetLineParams(item.ToString(), out data, out cid, out css);
            uprCounter++;
            if (uprCounter == uprCount)
                return css + " " + divR;
            else
                return css;
        }

        public string GetDataCmd(object item)    
        {
            string cid, pid, uid, css, data;
            Tools.GetLineParams(item.ToString(), out data, out cid, out css, out pid, out uid);
            //if (!String.IsNullOrEmpty(cid))
            if (cid != "x")
                return Tools.SetLineParams(cid, pid, uid);      // podgląd
            else 
                return Tools.SetLineParams(null, pid, uid);     // dopisz 
        }

        int typ = -1;
        int ed = -1;
        public bool GetVisible(object item)
        {
            if (ed  == -1) ed  = CanEdit ? 1 : 0;
            if (typ == -1) typ = Tools.StrToInt(Typ, 3);  // 1|2
            string cid, pid, uid, css, data;
            Tools.GetLineParams(item.ToString(), out data, out cid, out css, out pid, out uid);
            if (typ == utPassSpawInt)
            {
                //string cid, pid, uid, css, data, u1, u2, data2, skip1, skip2;
                //Tools.GetLineParams(item.ToString(), out data, out cid, out css, out pid, out uid, out u1, out u2, out data2, out skip1, out skip2);
                return cid != "x";
                //return true;
            }
            else
                return ed == 1 || cid != "x";    // mogę edytować (Dodaj) lub jest certyfikat
        }
        //-----
        public string GetNumClass(object num)
        {
            if (num != null)
            {
                string n = num.ToString();
                if (n == "0")
                    return "zero";
            }
            return null;
        }

        //-----
        private int colCnt = 0;
        private int sumCount = 0;

        //public string GetSumClass()
        //{
        //    string css = null;
        //    if (colCnt < sumCount) css = null;
        //    else if (colCnt == sumCount) css = " total divL"; 
        //    else css = " total";
        //    colCnt++;
        //    return css;
        //}

        public string GetSumClass()
        {
            string css = null;
            colCnt++;            
            if (colCnt < sumCount) css = null;
            else if (colCnt == sumCount) css = " " + divR;
            else css = " total";
            return css;
        }
        //-----        
        protected void rpUprawnienia_Init(object source, EventArgs e)
        {
        }

        protected bool GetStrKierVisible()
        {
            return showStrKier;
        }

        protected bool GetStrKierVisibleNot()
        {
            return !showStrKier;
        }

        protected string GetKierCss()
        {
            return showStrKier ? "kier" : "nrew";
        }
        //-----

        //-----------------------------------------
        protected void btExcel_Click(object sender, EventArgs e)
        {
            /*
            int ccc = Controls.Count;
            string filename = null;
            foreach (Control cnt in ContentPlaceHolderReport.Controls)
                if (cnt is cntReport)
                {
                    filename = ((cntReport)cnt).Title;
                    break;
                }

            if (String.IsNullOrEmpty(filename))
            {
                filename = hidReportTitle.Value;
                if (String.IsNullOrEmpty(filename))
                    filename = "report.csv";
            }
            */
            switch (ShowZoomId)
            {
                case zidCertyfikat:
                case zidCertyfikatAdd:
                    string filename = "report";
                    Report.ExportCSV(filename, cntCertyfikatAdd.DataSource, null, null, false, false, true);
                    break;
                default:
                    filename = "report";
                    Report.ExportExcel(hidReport.Value, filename, null);   // >>>>>> musi jak redirect do nowej strony bo się nie ściąga
                    break;
            }

        }

        const int zidNone           = 0;
        const int zidCertyfikat     = 1;
        const int zidCertyfikatAdd  = 5;

        protected void btClose_Click(object sender, EventArgs e)
        {
            switch (ShowZoomId)
            {
                case zidCertyfikat:
                    cntCertyfikat.Close();
                    if (cntCertyfikat.Changed)
                    {
                        UpdateList();
                    }
                    break;
                case zidCertyfikatAdd:
                    cntCertyfikatAdd.Close();
                    if (cntCertyfikatAdd.Changed)
                    {
                        UpdateList();
                    }
                    break;
            }
            ShowZoomId = zidNone;
        }

        private int ShowZoomId
        {
            set { ViewState["zoomid"] = value; }
            get { return Tools.GetInt(ViewState["zoomid"], 0); }
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            int ok = 0;
            string msg = null;
            try
            {
                if (db.Lock(lockIMPORT, App.User))
                {
                    try
                    {
                        string sql = db.getScalar(String.Format("select SQL from Scheduler where Grupa = '{0}' and Typ = '{1}' and Aktywny = 1", schASSECO, schIMPORT));
                        if (!String.IsNullOrEmpty(sql))
                            db.execSQL(sql);
                        else
                            db.execSQL(AssecoImportSql.SelectCommand);
                    }
                    finally
                    {
                        db.Unlock(lockIMPORT);
                    }
                    UpdateList();
                }
                else
                    ok = 2;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ok = 1;
                //throw;      // nie wyswietla sie showmessage !! wiec na razie zeby pokazac
            }
            switch (ok)
            {
                case 0:
                    Tools.ShowMessage("Import zakończony poprawnie.");
                    break;
                case 1:
                    Tools.ShowMessage("Wystąpił bąd podczas importu danych. Szczegóły znajdują się w logu zadarzeń.");
                    break;
                case 2:
                    Tools.ShowMessage("Trwa import danych. Proszę spróbować ponownie za chwilę.");
                    break;
            }
        }
        //-----------------
        protected void SqlDataSource2_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            // zeby policzyc ile razy sie odpala - 1 !
        }

        /*
        po
        monit
        ok
        bezterm
        brak
        */

        public string x_GetDataCss(object item)
        {
            string id, data;
            if (item != null)
            {
                Tools.GetLineParams(item.ToString(), out id, out data);
                if (!String.IsNullOrEmpty(id))
                {
                    if (!String.IsNullOrEmpty(data))
                    {
                        DateTime dt = (DateTime)Tools.StrToDateTime(data);
                        DateTime dtT = (DateTime)dt;
                        DateTime dtM = dtT.AddDays(Monit);
                        DateTime day = Data;
                        if (dtT < day) return " po";
                        else if (dtM < day) return " monit";
                        else return " ok";
                    }
                    else return " bezterm";
                }
            }
            return " brak";
        }
        //--------------------------------
        bool first = true;
        int uprCount = 0;
        int uprCounter = 0;   // licznik do css divR
        const string divR = "divR";  // divL nie rysuje się w RCP :(

        protected void lvUprawnienia_DataBinding(object sender, EventArgs e)
        {
            first = true;
            uprCount = 0;
        }

        protected void lvUprawnienia_DataBound(object sender, EventArgs e)
        {
            if (resetLetterPager) Tools.ResetLetterPager(lvUprawnienia); 
            Tools.UpdateLetterPager(lvUprawnienia);    // może być wywoływane automatycznie przy Tools.PrepareDicListView, ale szkoda czasu bo przy każdym dic
            L.p(lvUprawnienia, "ddlLines");

            switch (Typ)
            {
                case utPassSpaw:
                    int cs = GetColSpan(1);
                    Tools.SetColSpan(lvUprawnienia, "thUPS", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum1", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum2", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum3", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum4", cs);
                    Tools.SetControlVisible(lvUprawnienia, "thNrEw2", true);
                    break;
                default:
                    cs = GetColSpan(1);
                    Tools.SetColSpan(lvUprawnienia, "thUPS", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum1", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum2", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum3", cs);
                    Tools.SetColSpan(lvUprawnienia, "tdSum4", cs);
                    Tools.SetControlVisible(lvUprawnienia, "thNrEw2", false);
                    break;
            }

            if (GetStrKierVisible())
            {
                Tools.SetControlVisible(lvUprawnienia, "th1", true);
                Tools.SetControlVisible(lvUprawnienia, "th2", true);
                Tools.SetControlVisible(lvUprawnienia, "LinkButton2", false);
                Tools.SetControlVisible(lvUprawnienia, "LinkButton7", true);
            }

            ZoomKierShow();
        }
        //---
        public static string[] GetHeaderData(DataRowView drv, int skip)
        {
            string[] ds = new string[drv.Row.Table.Columns.Count - skip];
            for (int i = skip; i < drv.Row.Table.Columns.Count; i++)
                ds[i - skip] = drv.Row.Table.Columns[i].Caption;
            return ds;
        }

        /*
DataTable GetListItem()
{
      DataTable table = new DataTable();
      table.Columns.Add("ID", typeof(int));
      table.Columns.Add("Value", typeof(string));
      table.Rows.Add(1, "ListItem1");
      table.Rows.Add(2, "ListItem2");
      table.Rows.Add(3, "ListItem3");
      table.Rows.Add(4, "My ListItem Wraps also");
      table.Rows.Add(5, "My New ListItem5");
      table.Rows.Add(6, "ListItem6");
      table.Rows.Add(7, "ListItem7");
      table.Rows.Add(8, "ListItem8");
      return table;
}

 
         */







        private void SetDataSource(string rpId, DataView dv)
        {
            Repeater rp = lvUprawnienia.FindControl(rpId) as Repeater;
            if (rp != null)
            {
                rp.DataSource = dv;
                uprCounter = 0;
                rp.DataBind();
            }
        }

        private void SetDataSource(string rpId, string[] dv)
        {
            Repeater rp = lvUprawnienia.FindControl(rpId) as Repeater;
            if (rp != null)
            {
                rp.DataSource = dv;
                rp.DataBind();
            }
        }

        private void SetDataSourceSum(string rpId, string[] dv, int cntSum, int cntTotal)
        {
            Repeater rp = lvUprawnienia.FindControl(rpId) as Repeater;
            if (rp != null)
            {
                rp.DataSource = dv;
                colCnt = 0;
                sumCount = cntSum;
                rp.DataBind();
            }
        }



        private void GetSumy(DataView dv, out string[][] s)
        {
            int cntC = dv.Table.Columns.Count;
            int cntR = dv.Count;
            s = new string[cntC][];

            for (int c = 0; c < cntC; c++)
            {
                s[c] = new string[cntR];
                for (int r = 0; r < cntR; r++)
                    switch (c)
                    {
                        case 0:
                            s[c][r] = dv.Table.Rows[r].ItemArray[c].ToString();
                            break;
                        default:
                            s[c][r] = s[0][r] + "|" + dv.Table.Rows[r].ItemArray[c].ToString();
                            break;
                    }
            }
        }

        private void GetSumy(DataView dvSumy, DataView dvTotal, out string[][] s, out int sumCount, out int totalCount)
        {
            if (dvSumy == null || dvTotal == null)
            {
                s = null;
                sumCount = 0;
                totalCount = 0;
                return;
            }

            int cntC = dvSumy.Table.Columns.Count;
            int cntR = dvSumy.Count;
            int cntT = dvTotal.Table.Columns.Count;   // cntT == cntC !!!
            s = new string[cntC][];

            for (int c = 0; c < cntC; c++)
            {
                s[c] = new string[cntR + cntT];
                //----- sumy -----
                for (int r = 0; r < cntR; r++)
                    switch (c)
                    {
                        case 0:
                            s[c][r] = dvSumy.Table.Rows[r].ItemArray[c].ToString();
                            break;
                        default:
                            s[c][r] = Tools.SetLineParams(
                                            s[0][r], 
                                            dvSumy.Table.Rows[r].ItemArray[c].ToString());
                            break;
                    }
                //----- total -----
                switch (c)
                {
                    case 0:
                        break;
                    default:
                        for (int r = 0; r < cntT; r++)
                            if (c - 1 == r)  // w 0 wierszu są Id
                                s[c][cntR + r] = Tools.SetLineParams(
                                    null,
                                    dvTotal.Table.Rows[0].ItemArray[r].ToString());
                            else
                                s[c][cntR + r] = null;
                        break;
                }
            }
            sumCount = cntR;
            totalCount = cntT;
        }

        protected void lvUprawnienia_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    //int skip = GetStrKierVisible() ? 10 : 8;   //pomijam pierwsze kolumny: nazwisko, nrew, id ...
                    int skip = 8;   //pomijam pierwsze kolumny: nazwisko, nrew, id ...

                    Repeater rpLine = e.Item.FindControl("rpLine") as Repeater;
                    if (rpLine != null)
                    {
                        if (first)
                        {
                            first = false;

                            DataView dvHeader = (DataView)SqlDataSourceHeader.Select(DataSourceSelectArguments.Empty);
                            uprCount = dvHeader.Count;
                            SetDataSource("rpHeader1", dvHeader);
                            SetDataSource("rpHeader2", dvHeader);
                            SetDataSource("rpHeader3", dvHeader);
                            SetDataSource("rpHeader4", dvHeader);   // sortowanie

                            //Tools.SetColSpan(lvUprawnienia, "thUprTitle", uprCount);
                            Tools.SetColSpan(lvUprawnienia, "tdNav", uprCount);
                            Tools.SetColSpan(lvUprawnienia, "tdNavS", 3);

                            DataView dvSumy = (DataView)SqlDataSourceSum.Select(DataSourceSelectArguments.Empty);
                            DataView dvTotal = (DataView)SqlDataSourceTotal.Select(DataSourceSelectArguments.Empty);
                            string[][] s;
                            int cntSum, cntTotal;
                            GetSumy(dvSumy, dvTotal, out s, out cntSum, out cntTotal);
                            if (cntTotal > 0 && cntSum > 0)
                            {
                                SetDataSourceSum("rpSumA", s[1], cntSum, cntTotal);
                                SetDataSourceSum("rpSumW", s[2], cntSum, cntTotal);
                                SetDataSourceSum("rpSumP", s[3], cntSum, cntTotal);
                                if (showBrak)
                                    SetDataSourceSum("rpSumB", s[4], cntSum, cntTotal);
                            }
                        }
                        DataRowView drv = Tools.GetDataRowView(e);
                        switch (Typ)
                        {
                            case utProdukcyjne:
                            case utNieprodukcyjne:
                                break;
                            case utPassSpaw:
                                Tools.SetControlVisible(e.Item, "tdNrEw2", true);
                                break;
                            case utElektryczne:
                                break;
                        }                 
                        
                        
                        /*
                        if (Typ == utPassSpaw)   // <<< zmienić na stronę sql - nie wyszukuje
                        {
                            const string no = "-";
                            string p = no;
                            string c = null;
                            string d = null;
                            string id = null;
                            for (int i = skip + uprCount - 1; i >= skip; i--)
                            {
                                string item = drv.Row[i].ToString();
                                string cid, pid, uid, css, data, pozId, poz;
                                Tools.GetLineParams(item, out data, out cid, out css, out pid, out uid, out pozId, out poz);
                                //----- ustawiacz ------
                                if (p == pozId)
                                {
                                    if (cid == "x")
                                    {
                                        css = c + " poziom";
                                        data = d;
                                        cid = id;
                                    }
                                    else
                                    {
                                        css = c;
                                    }
                                    drv.Row[i] = Tools.SetLineParams(data, cid, css, pid, uid, pozId, poz);
                                }
                                else
                                {
                                    p = no;
                                    //----- starter -----
                                    if (cid != "x" && !String.IsNullOrEmpty(pozId))   // w else żeby nie przełączał jak będzie jakieś wczesniejsze uprawnienie
                                    {
                                        p = pozId;
                                        c = css;
                                        d = data;
                                        id = cid;
                                    }
                                }
                            }
                        }
                        */
                        rpLine.DataSource = drv.Row.ItemArray.Skip(skip).Take(uprCount);
                        uprCounter = 0;
                        rpLine.DataBind();
                    }
                    break;

            }
        }

        protected void lvUprawnienia_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.InsertItem:
                    break;
                case ListViewItemType.EmptyItem:
                    L.p(e.Item, "NoDataLabel");
                    break;
            }
        }

        protected void lvUprawnienia_LayoutCreated(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterExpression;

            if (!IsPostBack)  // w PageLoad jeszcze nie istnieje
            {
            }

            TranslateLV();
            if (Szkoleniap)
            {
                Label Label1 = lvUprawnienia.FindControl("Label1") as Label;
                Label Label6 = lvUprawnienia.FindControl("Label6") as Label;
                LinkButton LinkButton5 = lvUprawnienia.FindControl("LinkButton5") as LinkButton;
                Label1.Text = L.p("Szkolenie");
                Label6.Text = L.p("Nieaktualne");
                LinkButton5.Text = L.p("Nieaktualne");
            }
            //L.p(lvUprawnienia, "DataPager1");
            if (showBrak)
                Tools.SetControlVisible(lvUprawnienia, "trSumB", true);
        }

        protected void lvUprawnienia_Sorted(object sender, EventArgs e)
        {

        }

        protected void lvUprawnienia_Sorting(object sender, ListViewSortEventArgs e)
        {

        }
        //------------------------------
        private void UpdateList()
        {
            lvUprawnienia.DataBind();
            ((UpdatePanel)Parent.Parent).Update();  // bo jest Conditional
        }
        
        private void CancelAll()
        {
            cntPracownik.Cancel();
            cntCertyfikat.Cancel();
            cntCertyfikatAdd.Cancel();
            //cntCertyfikatSpaw.Cancel();
            cntPracownikUprawnienia.Cancel();
            cntUprawnieniePracownicy.Cancel();
            cntUprPracownicyTotal.Cancel();
            //cntUprCzynnosciHeader.Cancel();
        }

        private void UpdateFilterPanel()
        {
            UpdatePanel up = Tools.FindUpdatePanel(this);
            if (up != null) up.Update();
        }

        //-----
        private Stack<string> ZoomKierList
        {
            set { ViewState["zklist"] = value; }
            get 
            {
                object o = ViewState["zklist"];
                if (o == null)
                {
                    Stack<string> z = new Stack<string>();
                    ZoomKierList = z;
                    return z;
                }
                else
                    return (Stack<string>)o;
            }
        }

        private string ZoomKierCurrent
        {
            get { return ZoomKierList.Count == 0 ? null : ZoomKierList.First(); }
        }

        public bool IsKierUpVisible
        {
            get { return ZoomKierList.Count > 0; }
        }

        private void ZoomKierShow()
        {
            string kid, kn;
            Tools.GetLineParams(ZoomKierCurrent, out kid, out kn);
            ZoomKierShow(kid, kn);
        }

        private void ZoomKierShow(string kid, string kn)
        {
            HtmlGenericControl pa = lvUprawnienia.FindControl("paKierUp") as HtmlGenericControl;            
            if (pa != null)
            {
                ImageButton ibt = lvUprawnienia.FindControl("ibtUp") as ImageButton;
                LinkButton lbt = lvUprawnienia.FindControl("lbtUp") as LinkButton;  // musi być
                if (!String.IsNullOrEmpty(kid))
                {
                    ibt.CommandArgument = kid;
                    //lbt.Text = String.Format("Cofnij: {0} ", kn);
                    lbt.Text = kn;
                    pa.Visible = true;
                }
                else
                {
                    ibt.CommandArgument = null;
                    lbt.Text = null;
                    pa.Visible = false;
                }
            }
        }

        private void ZoomKierPush(string kid, string kn)
        {
            /*  samo sie wyswietli w lvDataBound
            if (ZoomKierCurrent == null) 
            {
                ZoomKierShow(kid, kn);
            }
            */
            ZoomKierList.Push(Tools.SetLineParams(kid, kn));
            ZoomKierList = ZoomKierList;
        }

        private bool ZoomKierPop(out string kid, out string kn)
        {
            string p = ZoomKierCurrent;
            if (ZoomKierList.Count > 0) ZoomKierList.Pop();
            ZoomKierList = ZoomKierList;
            if (String.IsNullOrEmpty(p))
            {
                kid = null;
                kn = null;
                //ZoomKierShow(null, null);
                return false;
            }
            else
            {
                Tools.GetLineParams(p, out kid, out kn);
                return true;
            }
        }

        private void ZoomKierClear()
        {
            ZoomKierList.Clear();
            ZoomKierList = ZoomKierList;
            ZoomKierShow();
        }

        /*        
        private string ZoomKierList
        {
            set { ViewState["zklist"] = value; }
            get { return Tools.GetStr(ViewState["zklist"]); }
        }

        private string ZoomKierCurrent
        {
            get
            {
                string z = ZoomKierList;
                if (String.IsNullOrEmpty(z))
                    return null;
                else
                {
                    int p = z.LastIndexOf("|");
                    if (p >= 0)
                        return z.Substring(p + 1);  // nie może być sam | na końcu
                    else
                        return z;
                }
            }
        }

        private bool IsUpVisible
        {
            get { return !String.IsNullOrEmpty(ZoomKierList); }
        }

        private void ZoomKierPush(string kierId)
        {
            if (String.IsNullOrEmpty(ZoomKierList))
            {
                ZoomKierList = kierId;
                ImageButton ibt = lvUprawnienia.FindControl("ibtUp") as ImageButton;
                ibt.Visible = true;
            }
            else
                ZoomKierList += "|" + kierId;
        }

        private string ZoomKierPop()
        {
            string z = ZoomKierList;
            if (String.IsNullOrEmpty(z))
                return null;
            else
            {
                int p = z.LastIndexOf("|");
                if (p >= 0)
                    z.Remove(p);
                else
                {
                    z = null;
                    ImageButton ibt = lvUprawnienia.FindControl("ibtUp") as ImageButton;
                    ibt.Visible = false;
                }
                ZoomKierList = z;
                return ZoomKierCurrent;
            }
        }
         */ 
        //-----

        private void Zoom(string cmd, string par)
        {
            string awp, pracId, uprId, certId;
            switch (cmd)
            {
                case "zoomPrac":
                case "zoomNrEw":
                    CancelAll();
                    cntPracownik.Prepare(par);
                    cntPracownik.Visible = true;
                    cntPracownik.CloseButton.Visible = false;
                    Tools.ShowDialog(this, "divZoom", 600, btClose, L.p("Dane pracownika"));
                    break;
                case "zoomKier":
                    CancelAll();
                    ZoomKierPush(ddlKier.SelectedValue, ddlKier.SelectedItem.Text);
                    resetLetterPager = true;
                    Tools.SelectItem(ddlKier, par);
                    UpdateFilterPanel();
                    break;
                case "zoomKierUp":
                    CancelAll();
                    string kid, kin;
                    ZoomKierPop(out kid, out kin);
                    resetLetterPager = true;
                    Tools.SelectItem(ddlKier, kid);
                    UpdateFilterPanel();
                    break;
                case "zoomCert":
                    CancelAll();
                    ShowZoomId = zidCertyfikat;
                    Tools.GetLineParams(par, out certId, out pracId, out uprId);
                    /*RCP
                    if (Typ == utPassSpaw)
                        cntCertyfikatSpaw.Prepare(certId, pracId, uprId, CanEdit);
                    else
                        */
                    cntCertyfikat.Prepare(certId, pracId, uprId, Typ, CanEdit);

                    Tools.ShowDialog(this, "divZoom", 1300, btClose, true, L.p((Szkoleniap) ? "Szkolenie BHP" : "Certyfikat"));
                    break;
                case "zoomCertAdd":
                    CancelAll();
                    ShowZoomId = zidCertyfikatAdd;
                    cntCertyfikatAdd.Prepare(null, null, null, Typ, CanEdit);
                    Tools.ShowDialog(this, "divZoom", 1300, btClose, true, L.p((Szkoleniap) ? "Dodaj szkolenie BHP" : "Dodaj certyfikat"));
                    break;
                case "zoomLinia":       // Pracownik - Uprawnienia (prawa strona)
                    CancelAll();
                    Tools.GetLineParams(par, out awp, out pracId);
                    cntPracownikUprawnienia.Prepare(pracId, hidTyp.Value, "-99", awp, hidData.Value, hidMonit.Value);
                    //Tools.ShowDialog(this, "divZoom", 1100, btClose, L.p("Uprawnienia pracownika - {0}", AppUser.GetNazwiskoImieNREW(pracId)));
                    Tools.ShowDialog(this, "divZoom", 1400, btClose, L.p(Szkoleniap ? "Szkolenia pracownika - {0}" : "Uprawnienia pracownika - {0}", AppUser.GetNazwiskoImieNREW(pracId)));
                    break;
                case "zoomSumy":        // Uprawnienie - Pracownicy i Total (dół tabeli) 
                    CancelAll();
                    Tools.GetLineParams(par, out awp, out uprId);
                    if (!String.IsNullOrEmpty(uprId))
                    {
                        //---- sumy -----
                        string symbol,nazwa,title;
                        DataRow dr = db.getDataRow("select * from Uprawnienia where Id = " + uprId);
                        nazwa = String.Format("{0} - {1}", db.getValue(dr, "Symbol"), db.getValue(dr, L.Lang == L.lngPL ? "Nazwa" : "NazwaEN"));

                        //title = L.p(Szkoleniap ? "Szkolenie - {0}" : "Uprawnienie - {0}", nazwa);                       
                        title = nazwa;

                        /* i tak wchodze na zakładki
                        switch (awp)
                        {
                            case "A":
                                title = L.p(Szkoleniap ? "Szkolenie aktualne - {0}" :       "Uprawnienie aktualne - {0}", nazwa);
                                break;
                            case "W":
                                title = L.p(Szkoleniap ? "Szkolenie wygasające - {0}" :     "Uprawnienie wygasające - {0}", nazwa);
                                break;
                            case "P":
                                title = L.p(Szkoleniap ? "Szkolenie przeterminowane - {0}" : "Uprawnienie przeterminowane - {0}", nazwa);
                                break;
                            case "B":
                                title = L.p(Szkoleniap ? "Pracownicy bez szkolenia - {0}" : "Pracownicy bez uprawnienia - {0}", nazwa);
                                break;
                            default:
                                title = L.p(Szkoleniap ? "Szkolenie - {0}" :                "Uprawnienie - {0}", nazwa);
                                break;
                        }
                        */
                        cntUprawnieniePracownicy._Prepare(ddlKier.SelectedValue, ddlCC.SelectedValue, uprId, cbShowSub.Checked, ddlStatus.SelectedValue, awp, hidData.Value, hidMonit.Value);
                        Tools.ShowDialog(this, "divZoom", 1600, btClose, title);
                    }
                    else
                    {
                        //----- total -----
                        cntUprPracownicyTotal._Prepare(hidTyp.Value, hidKwal.Value, ddlKier.SelectedValue, ddlCC.SelectedValue, cbShowSub.Checked, ddlStatus.SelectedValue, awp, hidData.Value, hidMonit.Value);
                        Tools.ShowDialog(this, "divZoom", 1600, btClose, L.p(Szkoleniap ? "Szkolenia BHP" : "Uprawnienia"));
                    }
                    break;
                case "zoomUpr":         // header 
                    /*RCP
                    CancelAll();
                    cntUprCzynnosciHeader.Prepare(par);
                    string p = db.getScalar(String.Format("select {0} from Uprawnienia where Id = {1}", L.Lang == L.lngPL ? "Nazwa" : "NazwaEN",par));
                    Tools.ShowDialog(this, "divZoom", 1100, btClose, L.p("Czynności do uprawnienia - {0}", p));
                    */ 
                    break;
            }
        }

        protected void lvUprawnienia_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView lv = lvUprawnienia;
            switch (e.CommandName)
            {
                //----- letter data pager -----
                case "JUMP":
                    //EnableControls(lv, false, false);
                    
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lv.FindControl("DataPager1");
                    lv.Sort("Pracownik", SortDirection.Ascending);
                    //lvPracownicy.SelectedIndex = idx;
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);
                    /*
                    lvPracownicy.SelectedIndex = -1;
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                    */
                    break;
                //----- custom -----
                case "up":

                    break;
                default:
                    Zoom(e.CommandName, e.CommandArgument.ToString());
                    break;
            }
        }

        protected void rpUprawnienia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Sort":
                    //Tools.lvSort(lvUprawnienia, e.CommandArgument.ToString(), 0);
                    lvUprawnienia.Sort(e.CommandArgument.ToString(), SortDirection.Ascending);
                    break;
                default:
                    Zoom(e.CommandName, e.CommandArgument.ToString());
                    break;
            }
        }
        //--------------------------
        bool resetLetterPager = false;

        protected void tabContent_MenuItemClick(object sender, MenuEventArgs e)
        {
            //hidTyp.Value = tabContent.SelectedValue;
        }

        protected void tabFilter_MenuItemClick(object sender, MenuEventArgs e)
        {
            //FilterExpression = tabFilter.SelectedValue;
            //SetFilterExpr(true);

            hidFilter.Value = tabFilter.SelectedValue;
        }

        private void SetDefaultSort()
        {
            lvUprawnienia.Sort("Pracownik", SortDirection.Ascending);
        }

        protected void cntSqlKwalifikacje_SelectTab(object sender, EventArgs e)
        {
            hidKwal.Value = cntSqlKwalifikacje.SelectedValue;
            SetDefaultSort();
        }

        protected void cntStrSelect_SelectedChanged(object sender, EventArgs e)
        {
            /*
            //hidStrOrg.Value = cntStrSelect.StrOrgList;
            hidStatus.Value = cntStrSelect.Zakres;

            //hidKierId.Value = cntStrSelect.KierId == LiniaSelect2.ALL ? null : cntStrSelect.KierId;
            hidKierId.Value = cntStrSelect.KierId == LiniaSelect2.ALL ? "-99" : cntStrSelect.KierId;
            
            SetDefaultSort();
            Tools.ResetLetterPager(lvUprawnienia);

            Tools.SetPageSize(lvUprawnienia, true);

            //lvUprawnienia.DataBind();
             */ 
        }
        //----
        private void DateChanged()
        {
            hidData.Value = deNaDzien.DateStr;
            hidMonit.Value = tbMonit.Text;
            resetLetterPager = true;
            lvUprawnienia.DataBind();
        }

        protected void btShow_Click(object sender, EventArgs e)
        {
            DateChanged();
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
            ddlKier.SelectedIndex = 0;
            ddlCC.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            cbWithOnly.Checked = false;
            cbShowSub.Checked = false;
            InitDateMonit();
            ZoomKierClear();
            DoSearch(false);
            FocusSearch();
        }

        protected void tbMonit_TextChanged(object sender, EventArgs e)
        {
            DateChanged();
        }

        protected void deNaDzien_DateChanged(object sender, EventArgs e)
        {
            DateChanged();
        }
        //-----
        protected void btExcelF_Click(object sender, EventArgs e)
        {
            string upr = db.getScalar(String.Format("select {0} from UprawnieniaTypy where Id = {1}", L.Lang == L.lngPL ? "TypNazwa" : "TypNazwaEN", Typ));
            string filename = String.Format("{0} - {1}", upr, deNaDzien.DateStr);
            Report.ExportCSV(filename, SqlDataSource1, null, null, true, false, true);
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            Zoom("zoomCertAdd", null);
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetLetterPager = true;
        }

        protected void ddlKier_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetLetterPager = true;
            ZoomKierClear();
        }

        protected void ddlKier_DataBound(object sender, EventArgs e)
        {
            if (ddlKier.Items.Count == 1)
                ddlKier.Enabled = false;
        }

        protected void ddlCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetLetterPager = true;
        }

        protected void cbShowSub_CheckedChanged(object sender, EventArgs e)
        {
            resetLetterPager = true;
        }

        protected void cbWithOnly_CheckedChanged(object sender, EventArgs e)
        {
            resetLetterPager = true;
        }

        protected void cbPassSpaw_CheckedChanged(object sender, EventArgs e)
        {
            resetLetterPager = true;
        }














        //--------------------------
        private void Deselect()
        {
            lvUprawnienia.SelectedIndex = -1;

            /*
            SelectedRcpId = null;
            SelectedStrefaId = null;
            if (lvPracownicy.SelectedIndex != -1)
            {
                lvPracownicy.SelectedIndex = -1;
                TriggerSelectedChanged();
            }
             */ 
        }

        //----- FILTER ----------------------------------
        private void PrepareSearch()
        {
            //xbtClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID, btSearch.ClientID);
            //btClear.Attributes["onclick"] = String.Format("return {0}_clearFilter();", ClientID);   // jest postback bo js nie ma sensu... ale mechanizm do wykorzystania
            Tools.ExecOnStart2("searchtrigprac", String.Format("startSearch('{0}','{1}');",
                tbSearch.ClientID, btSearch.ClientID));
            tbSearch.Focus();
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        protected void tbSearch_TextChanged(object sender, EventArgs e)
        {
        }

        /*
        private string GetSearch(string field, string search)
        {
            bool s1 = search.StartsWith(" ");
            bool s2 = search.EndsWith(" ");
            int len = search.Length;

            if (s1 && s2 && len > 2) return String.Format("{0}='{1}'", field, search.Substring(1, len-2));
            else
                if (s1 && len > 1) 
                    return String.Format("{0} like '{1}%'", field, search.Substring(1, len-1));
                else if (s2 && len > 1) 
                    return String.Format("{0} like '%{1}'", field, search.Substring(0, len-1));
                else
                    return String.Format("{0} like '%{1}%'", field, search.Trim());
        }
        */

        private string SetFilterExpr(bool resetPager)
        {
            //return null;
            string filter;
            //string f1 = tabFilter.SelectedValue.Trim();
            string f1 = null;
            SqlDataSource1.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                filter = f1;
            }
            else
            {
                //Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string f2;
                string[] words = Tools.RemoveDblSpaces(tbSearch.Text.Trim()).Split(' ');   // nie trzeba sprawdzać czy words[i] != ''
                if (words.Length == 1)
                {
                    /*
                    string search = tbSearch.Text;
                    bool s1 = search.StartsWith(" ");
                    bool s2 = search.EndsWith(" ");
                    int len = search.Length;
                    */
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or Nr_Ewid like '{0}%')";
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or Nr_Ewid like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or Nr_Ewid like '{{{0}}}%')", i);
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            if (resetPager) Tools.ResetLetterPager(lvUprawnienia);       //resetPager nie robić kiedy !IsPostback - w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
            return filter;
        }

        private void DoSearch(bool init)  //init = !IsPostback, w SteFilter był ResetLetterPager który w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
        {
            resetLetterPager = true;
            SetFilterExpr(!init);
            if (!init)
            {
                //lvPracownicy.DataBind();
                Deselect();
                /*
                if (lvPracownicy.Items.Count == 1) Select(0);
                else if (lvPracownicy.SelectedIndex != -1) Select(-1);
                 */
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            DoSearch(false);
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
            DoSearch(false);
        }



        /*      
        public void Select(int index)
        {
            lvPrzypisania.SelectedIndex = index;
            CheckSelectedChanged();
        }

        private bool CheckSelectedChanged()
        {
            string oldId = SelectedId;
            string newId = lvPrzypisania.SelectedDataKey == null ? null : lvPrzypisania.SelectedDataKey.Value.ToString();
            if (oldId != newId)
            {
                SelectedId = newId;
                if (SelectedChanged != null)
                    SelectedChanged(this, EventArgs.Empty);
                return true;
            }
            else
                return false;
        }
        */


        //--------------------------
        public DateTime Data
        {
            set { hidData.Value = Tools.DateToStr(value); }
            get { return (DateTime)Tools.StrToDateTime(hidData.Value); }
        }

        public int Monit
        {
            set { hidMonit.Value = value.ToString(); }
            get { return Tools.StrToInt(hidMonit.Value, 0); }
        }
        /*
        public int Typ
        {
            set { hidTyp.Value = value.ToString(); }
            get { return Tools.StrToInt(hidTyp.Value, utProdukcyjne); }
        }
        */
        public string Typ
        {
            set { hidTyp.Value = value; }
            get { return hidTyp.Value; }
        }

        private string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                /*
                Deselect();
                lv.EditIndex = -1;
                lv.InsertItemPosition = InsertItemPosition.None;
                */
                SqlDataSource1.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        protected void lvUprawnienia_Init(object sender, EventArgs e)
        {
            

        }

        protected void lvUprawnienia_PreRender(object sender, EventArgs e)
        {

            //L.p(lvUprawnienia, "DataPager1");

        }

        protected void DataPager1_Load(object sender, EventArgs e)
        {
            //L.p(sender as DataPager);
        }


        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //string s;
            //s = hidTyp.Value;
            //s = hidFilter.Value;
            //s = hidKwal.Value;
            //s = hidData.Value;
            //s = hidMonit.Value;
            //s = hidStrOrg.Value;
            //s = hidStatus.Value;
            //bool c;
            //c = cbWithOnly.Checked;
            //c = cbPassSpaw.Checked;
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            //int cnt = e.AffectedRows;
        }

        public Boolean Szkoleniap
        {
            get
            {
                try
                {
                    //return Request.QueryString["t"] == utSzkolenia;
                    return Typ == utSzkolenia;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string Title
        {
            set { lbTitle.Text = value; }
        }







        /*
        Repeater FrpHeader1 = null;
        private Repeater rpHeader1
        {
            get 
            {
                if (FrpHeader1 == null)
                    FrpHeader1 = lvUprawnienia.FindControl("rpHeader1") as Repeater;
                return FrpHeader1;
            }
        }

        Repeater FrpHeader2 = null;
        private Repeater rpHeader2
        {
            get
            {
                if (FrpHeader2 == null)
                    FrpHeader2 = lvUprawnienia.FindControl("rpHeader2") as Repeater;
                return FrpHeader2;
            }
        }

        Repeater FrpHeader3 = null;
        private Repeater rpHeader3
        {
            get
            {
                if (FrpHeader3 == null)
                    FrpHeader3 = lvUprawnienia.FindControl("rpHeader3") as Repeater;
                return FrpHeader3;
            }
        }
        */
    }
}