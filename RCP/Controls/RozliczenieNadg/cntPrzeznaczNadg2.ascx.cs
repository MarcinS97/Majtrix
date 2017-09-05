using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using HRRcp.App_Code;
using HRRcp.Controls.Portal;

//UWAGA - do testów zmienić kontrolkę na TimeEdit2 i typy w kodzie


namespace HRRcp.Controls.RozliczenieNadg
{
    public partial class cntPrzeznaczNadg2 : System.Web.UI.UserControl
    {
        //private const bool doWyplatyAll = false;  //20150516 dla Siemensa - na razie wyłączam ...
        public const bool doWyplatyAll = 
#if DBW || VICIM     // tylko przez wnioski o nadgodziny 
            false
#else
            true
#endif
            ;  //20150516 dla Siemensa - na razie wyłączam ...
#if SIEMENS
        private const bool wnWyjscie = false;  //20150528 dla Siemensa brak wniosku o wcześniejsze wyjście
#else
        private const bool wnWyjscie = true;  
#endif


        
        
        
        public event EventHandler Changed;

        public const int ttCzasPracy             = -10;

        //----- nadgodziny ------
        //public const int _ttWybranePrac         = 2;    // komplementarny do 12
        //public const int _ttWybraneKier         = 4;    // komplementarny do 14 -         

        public const int _ttDoWyplaty             = 1;    // wybór
        
        public const int ttDoWybraniaPrac        = 2;    // wybór    6
        public const int ttDoWybraniaKier        = 4;    //          7
        public const int ttDoWybraniaSobota      = 5;    //          8

        public const int _ttOdpracowanie          = 3;    // wybór/komplementarny - odpracownie wcześniejszego wyjścia w dniu 
        public const int ttOdpracowanieSobota    = 6;    // 5 wybór/komplementarny - odpracowanie absencji całodniowej w sobotę

        public const int ttDoWyplatyWnNadg       = 7;
        public const int ttOdpracowanieWnNadg    = 8;    // wybór/komplementarny - odpracownie wcześniejszego wyjścia w dniu z wniosków o nadgodziny

        //----- niedomiar -----
        public const int ttWolneZaNadgPrac       = 12;   // w dniu
        public const int ttWolneZaNadgKier       = 14;   // w dniu
        public const int ttWolneZaSobote         = 15;   // w dniu

        public const int _ttDoOdpracowania        = 13;   // w dniu
        public const int ttDoOdpracowaniaWSobote = 16;   // w dniu, w przyszłości

        public const int ttDoOdpracowaniaWnNadg  = 18;   // w dniu z wniosku o nadgodziny

        public const int ttKorekta = 9;
        public const int ttSuma                  = 100;

        //---------------------
        public const int par1BezKomplement  = 0;   // do wypłaty i korekta - nie generuje komplementarnego wpisu
        public const int par1Komplement     = 1;   // 1:1
        public const int par1Sobota         = 8;   // 8h
        public const int par1WolneKier      = 15;  // wolne na wniosek przełożonego = x1.5

        public const int par2DoWyplaty      = 1;    // do wypłaty
        public const int par2Wolne          = 2;    // do wybrania - z tego są wnioski o wolne za nadgodziny
        public const int par2Odpracowanie   = 3;    // odpracowanie
        public const int par2Korekta        = 9;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public void Prepare(string pracId, string okresOd, string okresDo, int status)
        {
            PracId = pracId;
            OkresOd = okresOd;
            OkresDo = okresDo;
            OkresRozlOd = okresOd;
            OkresRozlDo = okresDo;
            ReadOnly = status == 1 && !App.User.HasRight(AppUser.rRozlNadgPo);
            Updated = false;    
        }

        /*
        public void Prepare(string pracId, string okresOd, string okresDo)
        {
            PracId = pracId;
            OkresOd = okresOd;
            OkresDo = okresDo;

            DateTime oOd, oDo;
            int status;
            if (Okres.GetRozliczeniowy(okresOd, out oOd, out oDo, out status) != null)
            {
                OkresRozlOd = Tools.DateTimeToStr(oOd);
                OkresRozlDo = Tools.DateTimeToStr(oDo);
                ReadOnly = status == 1 && !App.User.HasRight(AppUser.rRozlNadgPo);
            }
            else
            {
                OkresRozlOd = OkresOd;
                OkresRozlDo = OkresDo;
                ReadOnly = false;
            }
            Updated = false;    
        }
         */

        public void Clear()
        {
            PracId = null;
            Updated = false;
        }






















        /*
        public bool Prepare(string planId, string pracId, string data, string okresOd, string okresDo, int? czasZmPP, int? nadgDPP, int? nadgNPP, int? nocnePP, bool noedit)
        {
            _PlanId = planId;
            PracId = pracId;
            Data = data;
            OkresOd = okresOd;
            OkresDo = okresDo;
            CzasZmPP = czasZmPP == -1 ? 0 : czasZmPP;
            NadgDPP = nadgDPP;
            NadgNPP = nadgNPP;
            NocnePP = nocnePP;
            ReadOnly = noedit;
            lvPodzial.InsertItemPosition = noedit ? InsertItemPosition.None : InsertItemPosition.LastItem;
            lvPodzial.DataBind();
            return lvPodzial.Items.Count > 0;
        }

        public void Prepare(int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            CzasZm = czasZm;
            NadgD = nadgD;
            NadgN = nadgN;
            Nocne = nocne;
            ShowCzasPracy();
        }

        public void PrepareReadOnly()
        {
            CzasZm = CzasZmPP;
            NadgD = NadgDPP;
            NadgN = NadgNPP;
            Nocne = NocnePP;
            ShowCzasPracy();
        }
        */




        public void Update()
        {
            lvPodzial.DataBind();
        }

        /*
        public void Prepare(string czasZm, string nadgD, string nadgN, string nocne)
        {
            SetCzasPracy(czasZm, nadgD, nadgN, nocne);
        }
        */


















        //-------------------------------------
        const int t2PlanPracy   = 0;
        const int t2Rozliczenie = 1;
        const int t2Dopelnienie = 2;

        private bool CheckSetGray(ListViewItem item, string lbName, object value)   // jak 0 to true !!! 
        {
            int v = db.getInt(value, -1);
            if (v == 0)
            {
                Label lb = item.FindControl(lbName) as Label;
                if (lb != null) Tools.AddClass(lb, "gray");
                return true;
            }
            else return false;
        }

        bool firstLine = true;
        bool firstPoSumie = true;
        string lastData = null;

        Button btWniosek = null;
        bool   btWniosekVisible = false;
        string wniosekId = null;
        string wniosekTyp = null;
        string wniosekData = null;
        
        Button btWniosek2 = null;
        bool   btWniosekVisible2 = false;
        string wniosekId2 = null;

        HiddenField hidSumNiedomiar = null;

        public void InitItem(ListView lv, ListViewItemEventArgs e, bool create)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.InsertItem)
            {
                Button bt;
                bool select, edit, insert;
                int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
                if (!create)  // item data bound
                {
                    DataRowView drv = Tools.GetDataRowView(e);
                    int typ = db.getInt(drv["Typ"], -99);       // 1,2,3,4,5,6,9,12,13,14,15,16
                    int typ2 = db.getInt(drv["Typ2"], -99);     // 0 - PlanPracy, 1 - rozliczenie, 2 - dopełnienie
                    switch (lim)
                    {
                        case Tools.limSelect:
                            if (ReadOnly)
                            {
                                Tools.SetControlVisible(e.Item, "tdControl", false);
                                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdLastCol");
                                Tools.AddClass(td, "lastcol");
                            }

                            /*
                            bool ed = typ == ttWybrane || 
                                      typ == ttDoWyplaty || typ == ttOdpracowanie || 
                                      typ == ttWolneZaNadg || typ == ttGodzinyOdpracowane ||
                                      typ == ttKorekta;
                            */
                            bool ed = typ2 == 1;


                            //bool del = ed;
                            //bool del = typ == ttDoWyplaty || 
                            //          typ == ttWybrane || typ == ttOdpracowanie ||
                            //          typ == ttWolneZaNadg || typ == ttGodzinyOdpracowane ||
                            //          typ == ttKorekta;

                            bool del = typ != ttCzasPracy && typ != ttSuma
                                //&& typ != ttDoWyplatyWnNadg 
                                //&& typ != ttOdpracowanieWnNadg
                                //&& typ != ttDoOdpracowaniaWnNadg
                                && !typ.IsAny(ttDoWyplatyWnNadg, ttOdpracowanieWnNadg, ttDoOdpracowaniaWnNadg)
                                ;
                            
                            bool ins = typ == ttCzasPracy;

                            //bool _wn = typ == _ttWybranePrac || typ == _ttWybraneKier;

                            bt = Tools.SetControlVisible(e.Item, "DeleteButton", del) as Button;

                            bool bed = false;
                            // na razie wyłączamy, wpis komplementarny musi być, bo się będzie bilansować, a to niepoprawne!, stąd datę zawsz trzeba podać na wniosku, do roważenia jak data pusto to dodajemy do ostatniego dnia, dodajemy coś co "wisi" bez dat ???
                            if (typ == ttDoOdpracowaniaWnNadg)
                            {
                                string zaDzien = drv["ZaDzien"].ToString();
                                bed = true;// String.IsNullOrEmpty(zaDzien);                           
                            }
                             
                            //Tools.SetControlVisible(e.Item, "EditButton", ed);
                            Tools.SetControlVisible(e.Item, "EditButton2", bed);


                            Tools.SetControlVisible(e.Item, "InsertButton", ins);
                            if (del && bt != null) Tools.MakeConfirmDeleteRecordButton(bt);
                            //----- widoczność buttonów do wniosków ----------
                            if (firstPoSumie)
                            {
                                btWniosek = e.Item.FindControl("btWniosek") as Button;
                                btWniosekVisible = false;
                                wniosekId = null;
                                wniosekTyp = null;
                                wniosekData = null;

                                btWniosek2 = e.Item.FindControl("btWniosekOdpr") as Button;
                                wniosekId2 = drv["WniosekId"].ToString(); 

                                hidSumNiedomiar = e.Item.FindControl("hidSumNiedomiar") as HiddenField;

                                firstPoSumie = false;
                            }
                        
                            switch (typ)
                            {
                                case ttCzasPracy:   //-10
                                    /*                                    
                                    btWniosek = e.Item.FindControl("btWniosek") as Button;
                                    btWniosekVisible = false;
                                    wniosekId = null;
                                    wniosekTyp = null;

                                    btWniosek2 = e.Item.FindControl("btWniosekOdpr") as Button;
                                    wniosekId2 = drv["WniosekId"].ToString();

                                    hidSumNiedomiar = e.Item.FindControl("hidSumNiedomiar") as HiddenField;
                                    */
                                    break;
                                case ttWolneZaNadgPrac:         // 12
                                    //if (typ2 == t2Dopelnienie)
                                    {
                                        btWniosekVisible = true;
                                        wniosekTyp = "1";
                                    }
                                    break;
                                case ttDoWybraniaPrac:          // 2
                                    break;
                                case ttWolneZaNadgKier:         // 14
                                    //if (typ2 == t2Dopelnienie)
                                    {
                                        btWniosekVisible = true;
                                        wniosekTyp = "2";
                                    }
                                    break;
                                case ttDoWybraniaKier:          // 4
                                    break;
                                case ttWolneZaSobote:           // 15
                                    //if (typ2 == t2Dopelnienie)
                                    {
                                        //string d1 = drv["Data"].ToString();
                                        //string d2 = drv["ZaDzien"].ToString();                                        
                                        btWniosekVisible = true;
                                        wniosekTyp = "1";
                                        wniosekId = drv["WniosekId"].ToString();
                                    }
                                    break;
                                case ttDoWybraniaSobota:        // 5
                                    if (typ2 == t2Rozliczenie)
                                    {
                                        //string d1 = drv["Data"].ToString();
                                        //string d2 = drv["ZaDzien"].ToString();
                                        btWniosekVisible = true;
                                        wniosekTyp = "1";
                                        wniosekId = drv["WniosekId"].ToString();
                                        wniosekData = drv["ZaDzien"].ToString();     // wniosek jest wprzód - był problem bo generował odwrotnie jeżeli z tego klawisza się kliknęło, z dopełnienia było ok
                                    }
                                    break;
                                case ttSuma:                    // 100
                                    if (btWniosekVisible && btWniosek != null)
                                    {
                                        btWniosek.Visible = true;
                                        if (String.IsNullOrEmpty(wniosekData))
                                            btWniosek.CommandArgument = String.Format("{0}|{1}", wniosekId, wniosekTyp) + btWniosek.CommandArgument;
                                        else
                                            btWniosek.CommandArgument = String.Format("{0}|{1}|{2}", wniosekId, wniosekTyp, wniosekData);  // podmiana daty przy ttDoWybraniaSobota
                                    }

                                    if (btWniosek2 != null)
                                    {
                                        int n = Tools.GetInt(drv["Niedomiar"], 0);
                                        if (n < 0)
                                        {
                                            //btWniosek2.Visible = true;
                                            btWniosek2.Visible = wnWyjscie;
                                            //xbtWniosek2.CommandArgument = wniosekId2 + btWniosek2.CommandArgument;
                                        }
                                    }

                                    if (hidSumNiedomiar != null)
                                    {
                                        string n = drv["Niedomiar"].ToString();
                                        hidSumNiedomiar.Value = String.IsNullOrEmpty(n) ? "0" : n;
                                        hidSumNiedomiar = null;
                                    }

                                    firstPoSumie = true;
                                    break;
                            }




                            /*
                            Tools.SetControlVisible(e.Item, "btWniosek", wn);
                            if (wn)
                            {
                                HtmlTableCell td = e.Item.FindControl("tdData") as HtmlTableCell;
                                if (td != null) Tools.AddClass(td, "vcenter");
                            }
                            */
                              
                            HtmlTableRow tr = e.Item.FindControl("trLine") as HtmlTableRow;
                            if (tr != null)
                            {
                                Tools.AddClass(tr, "typ" + typ.ToString());
                                if (firstLine)
                                {
                                    Tools.AddClass(tr, "firstline");
                                    firstLine = false;
                                }
                            }

                            bool nied = !CheckSetGray(e.Item, "lbNiedomiar", drv["Niedomiar"]);
                            bool n50  = !CheckSetGray(e.Item, "lbN50", drv["N50"]);
                            bool n100 = !CheckSetGray(e.Item, "lbN100", drv["N100"]);

                            /**/
                            
                            CheckSetGray(e.Item, "lbN50WnWypl", drv["N50WnWypl"]);
                            CheckSetGray(e.Item, "lbN100WnWypl", drv["N100WnWypl"]);
                            CheckSetGray(e.Item, "lbN50WnPrzezn", drv["N50WnPrzezn"]);
                            CheckSetGray(e.Item, "lbN100WnPrzezn", drv["N100WnPrzezn"]);

                            /**/

                            Tools.SetValue(e.Item, "hidTyp10", nied ? "10" : (n50 || n100 ? "0" : null));
                            switch (typ)
                            {
                                case ttCzasPracy:
                                    lastData = Tools.DateToStr(drv["Data"]);
                                    Label lb = e.Item.FindControl("lbTyp") as Label;
                                    if (lb != null)
                                    {
                                        DateTime? dt = Tools.StrToDateTime(lastData);
                                        if (dt != null)
                                        {
                                            DayOfWeek d = ((DateTime)dt).DayOfWeek;
                                            if (d == DayOfWeek.Saturday)
                                                lb.Text += " - " + Tools.GetDayName(d);
                                        }
                                    }
                                    break;
                                default:
                                    string data = Tools.DateToStr(drv["Data"]);
                                    if (data == lastData)
                                        Tools.SetControlVisible(e.Item, "lbData", false);
                                    lastData = data;
                                    break;
                            }
                            break;
                        case Tools.limEdit:
                            DropDownList ddlKlas = e.Item.FindControl("ddlKlas") as DropDownList;
                            DropDownList ddlDaty = e.Item.FindControl("ddlZaDzien") as DropDownList;
                            if (ddlKlas != null && ddlDaty != null)
                            {
                                string data = Tools.GetText(e.Item, "hidData");
                                if (typ >= 10)
                                {
                                    ddlKlas.DataSourceID = "SqlDataSource2B";
                                    ddlDaty.DataSourceID = "SqlDataSource4B";
                                }
                                Tools.SelectItem(ddlKlas, typ);
                                hidDzien.Value = data;
                                ddlDaty.DataBind();
                            }


                            //Tools.SelectItem(e.Item, "ddlKlas", typ);
                            //ShowKlasVisible(e.Item, typ);
                            ShowKlasVisible(lvPodzial.EditItem, typ);
                            break;
                        case Tools.limInsert:
                            ddlKlas = e.Item.FindControl("ddlKlas") as DropDownList;
                            ddlDaty = e.Item.FindControl("ddlZaDzien") as DropDownList;
                            if (ddlKlas != null && ddlDaty != null)
                            {
                                string data = Tools.GetText(e.Item, "hidData");
                                if (typ >= 10)
                                {
                                    ddlKlas.DataSourceID = "SqlDataSource2B";
                                    ddlDaty.DataSourceID = "SqlDataSource4B";
                                }
                                Tools.SelectItem(ddlKlas, typ);
                                hidDzien.Value = data;
                                ddlDaty.DataBind();
                            }




                            /*                                                                          !!! podstawienie Atributes zmienia ID ddl !!! dlatego nie działa Insert !!! <<< robię z postback
                            DropDownList ddl = e.Item.FindControl("ddlNadg") as DropDownList;
                            TimeEdit te = e.Item.FindControl("teNadgD") as TimeEdit;
                            if (ddl != null && te != null)
                                ddl.Attributes["onChange"] = String.Format("javascript:return ddlKlasNadgChange('{0}','{1}');", ddl.ClientID, te.EditBox.ClientID);
                                //ddl.Attributes.Add("OnChange", String.Format("javascript:return ddlKlasNadgChange('{0}','{1}');", ddl.ClientID, te.EditBox.ClientID));
                            */

                            break;
                    }
                }
            }
        }

        protected void lvPodzial_LayoutCreated(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterExpression;
            if (!wnWyjscie)
            {
                Tools.SetControlVisible(lvPodzial, "paWniosekOdpr", false);
                Tools.SetControlVisible(lvPodzial, "btWniosekOdpr", false);
            }
        }

        protected void lvPodzial_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            InitItem(lvPodzial, e, true);
        }

        protected void lvPodzial_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            InitItem(lvPodzial, e, false);
        }

        protected void lvPodzial_DataBinding(object sender, EventArgs e)
        {
            lastData = null;
            firstLine = true;
        }

        protected void lvPodzial_DataBound(object sender, EventArgs e)
        {
            bool ro = ReadOnly;
            Tools.SetControlVisible(lvPodzial, "thControl", !ro);
            Tools.SetControlVisible(lvPodzial, "tdControl", !ro);
            HtmlTableCell th = (HtmlTableCell)lvPodzial.FindControl("thLastCol");
            if (th != null)
                if (ro)
                    Tools.AddClass(th, "lastcol");
                else
                    Tools.RemoveClass(th, "lastcol");
            //-------------------
            DateTime? dt = Tools.StrToDateTime(lastData);
            if (dt != null)
            {
                DateEdit de = (DateEdit)lvPodzial.FindControl("deWnNaDzien");
                if (de != null)
                    de.Date = ((DateTime)dt).AddDays(1);
            }

            if (doWyplatyAll)
            {
                Button bt = (Button)Tools.SetControlVisible(lvPodzial, "btDoWyplatyAll50", true);
                if (bt != null) Tools.MakeConfirmButton(bt, "UWAGA !!!\\nWszystkie nierozliczone nadgodziny 50 zostaną zaklasyfikowane do wypłaty.\\n\\nPotwierdzasz wykonanie operacji ?");
                bt = (Button)Tools.SetControlVisible(lvPodzial, "btDoWyplatyAll100", true);
                if (bt != null) Tools.MakeConfirmButton(bt, "UWAGA !!!\\nWszystkie nierozliczone nadgodziny 100 zostaną zaklasyfikowane do wypłaty.\\n\\nPotwierdzasz wykonanie operacji ?");
            }
        }

        //--------------------
        private void ShowKlasVisible(DropDownList ddl, bool dzien, bool ddldzien, bool nied)
        {
            Tools.SetControlVisibleSub(ddl.Parent.Parent, "deZaDzien", dzien);
            Tools.SetControlVisibleSub(ddl.Parent.Parent, "ddlZaDzien", ddldzien);
            Tools.SetControlVisibleSub(ddl.Parent.Parent, "teNiedomiar", nied);
        }

        private void ShowInfox15Visible(DropDownList ddl, bool infox1, bool infox15)
        {
            Tools.SetControlVisible(ddl.Parent, "lbInfox1", infox1);
            Tools.SetControlVisible(ddl.Parent, "lbInfox15", infox15);
        }

        private void ShowInfox15Visible(DropDownList ddl)
        {
            switch (Tools.StrToInt(ddl.SelectedValue, -1))
            {
                default:
                    ShowInfox15Visible(ddl, false, false);
                    break;
                case ttWolneZaNadgPrac:
                    ShowInfox15Visible(ddl, true, false);
                    break;
                case ttWolneZaNadgKier:
                    ShowInfox15Visible(ddl, false, true);
                    break;
                case ttDoWybraniaPrac:      // w przyszłości 
                    ShowInfox15Visible(ddl, true, false);
                    break;
                case ttDoWybraniaKier:      // 
                    ShowInfox15Visible(ddl, false, true);
                    break;
            }
        }

        private void _ShowKlasVisible(DropDownList ddl)
        {
            switch (Tools.StrToInt(ddl.SelectedValue, -1))
            {
                case _ttDoWyplaty:
                case ttDoWyplatyWnNadg:
                    ShowKlasVisible(ddl, false, false, false);
                    break;

                case ttWolneZaNadgPrac:
                case ttWolneZaNadgKier:
                case ttWolneZaSobote:
                    ShowKlasVisible(ddl, true, true, false);
                    break;

                case _ttOdpracowanie:
                case ttOdpracowanieWnNadg:
                case ttOdpracowanieSobota:
                    ShowKlasVisible(ddl, true, true, false);
                    break;
                    
                case ttDoWybraniaPrac:      // w przyszłości 
                case ttDoWybraniaKier:      // 
                case ttDoWybraniaSobota:    // 
                    ShowKlasVisible(ddl, true, false, false);
                    break;

                case _ttDoOdpracowania:      // w przyszłości
                case ttDoOdpracowaniaWnNadg:      // w przyszłości
                case ttDoOdpracowaniaWSobote:
                    ShowKlasVisible(ddl, true, false, false);
                    break;

                case ttKorekta:
                    ShowKlasVisible(ddl, false, false, true);
                    break;
            }
            ShowInfox15Visible(ddl);
        }

        protected void ddlKlas_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            HiddenField hidTyp10 = ddl.Parent.FindControl("hidTyp10") as HiddenField;
            DropDownList ddlDaty = ddl.Parent.FindControl("ddlZaDzien") as DropDownList;
            if (ddlDaty != null && hidTyp10 != null)
            {
                if (hidTyp10.Value == "10")
                {
                    ddlDaty.DataSourceID = "SqlDataSource4B";
                }

            }
            _ShowKlasVisible(ddl);
            //------------------------------
            TimeEdit te50 = ddl.Parent.FindControl("teN50") as TimeEdit;
            TimeEdit te100 = ddl.Parent.FindControl("teN100") as TimeEdit;
            Label lb50 = ddl.Parent.FindControl("lbN50") as Label;
            Label lb100 = ddl.Parent.FindControl("lbN100") as Label;

            if (te50 != null && te100 != null)
            {
                int typ = Tools.StrToInt(ddl.SelectedValue, -1);
                switch (typ)
                {
                    case ttWolneZaNadgKier:
                        HiddenField hidSumNiedomiar = ddl.Parent.FindControl("hidSumNiedomiar") as HiddenField;
                        if (hidSumNiedomiar != null)
                        {
                            int c = -Tools.StrToInt(hidSumNiedomiar.Value, 0);
                            if (c > 0)
                            {
                                //teD.Seconds = Worktime.RoundSec((int)Math.Round(c / 1.5), 1, 2);
                                te50.Seconds = (int)Math.Round(c / 1.5);
                            }
                        }
                        break;
                    case ttWolneZaSobote:
                        HiddenField hidWymiarEtat = ddl.Parent.FindControl("hidWymiarEtat") as HiddenField;
                        if (hidWymiarEtat != null)
                        {
                            //teN.Seconds = Tools.StrToInt(hidWymiarEtat.Value, 28800);
                        }
                        break;
                    case ttDoWybraniaSobota:
                        hidWymiarEtat = ddl.Parent.FindControl("hidWymiarEtat") as HiddenField;
                        if (hidWymiarEtat != null)
                        {
                            int s = TimeEdit.StrToSec(lb100.Text, 0);
                            int w = Tools.StrToInt(hidWymiarEtat.Value, 28800);
                            te100.Seconds = s > w ? w : s;
                        } 
                        break;
                }
            }
        }
        //---
        private void ShowKlasVisible(ListViewItem item, bool dzien, bool ddldzien, bool nied)
        {
            Tools.SetControlVisible(item, "deZaDzien", dzien);
            Tools.SetControlVisible(item, "ddlZaDzien", dzien);
            Tools.SetControlVisible(item, "teNiedomiar", nied);
        }

        private void ShowKlasVisible(ListViewItem item, int typ)
        {
            ListViewItem ddl = lvPodzial.EditItem;   // zeby mozna bylo przekopiowac
            switch (typ)
            {
                case _ttDoWyplaty:
                case ttDoWyplatyWnNadg:
                    ShowKlasVisible(ddl, false, false, false);
                    break;

                case ttWolneZaNadgPrac:
                case ttWolneZaNadgKier:
                case ttWolneZaSobote:
                    ShowKlasVisible(ddl, true, true, false);
                    break;

                case _ttOdpracowanie:
                case ttOdpracowanieWnNadg:
                case ttOdpracowanieSobota:
                    ShowKlasVisible(ddl, true, true, false);
                    break;

                case ttDoWybraniaPrac:      // w przyszłości 
                case ttDoWybraniaKier:      // 
                case ttDoWybraniaSobota:    // 
                    ShowKlasVisible(ddl, true, false, false);
                    break;

                case _ttDoOdpracowania:      // w przyszłości
                case ttDoOdpracowaniaWnNadg:      // w przyszłości
                case ttDoOdpracowaniaWSobote:
                    ShowKlasVisible(ddl, true, false, false);
                    break;

                case ttKorekta:
                    ShowKlasVisible(ddl, false, false, true);
                    break;
            }
            //ShowInfox15Visible(ddl);
        }

        protected void ddlKlasEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            int typ = Tools.StrToInt(ddl.SelectedValue, -1);
            ShowKlasVisible(lvPodzial.EditItem, typ);
        }
        //--------------------
        protected void ddlZaDzien_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            string _kod, data, pid, czas, czasHMM, p6;   // _kod: 0 niedomiar, 50, 100 - nadgodziny
            Tools.GetLineParams(ddl.SelectedValue, out _kod, out data, out pid, out czas, out czasHMM, out p6);

            int par1 = par1Komplement;  //1:1
            DropDownList ddlKlas = ddl.Parent.FindControl("ddlKlas") as DropDownList;
            if (ddlKlas != null && !String.IsNullOrEmpty(ddlKlas.SelectedValue))
            {
                string p1 = db.getScalar("select Par1 from Kody where Typ = 'PODZNADG' and Kod = " + ddlKlas.SelectedValue);
                par1 = Tools.StrToInt(p1, par1);
            }

            DateEdit de = ddl.Parent.FindControl("deZaDzien") as DateEdit;
            TimeEdit te50 = ddl.Parent.FindControl("teN50") as TimeEdit;
            TimeEdit te100 = ddl.Parent.FindControl("teN100") as TimeEdit;
            Label lb50 = ddl.Parent.FindControl("lbN50") as Label;
            Label lb100 = ddl.Parent.FindControl("lbN100") as Label;

            if (te50 != null && te100 != null && de != null)
            {
                //if (String.IsNullOrEmpty(teD.TimeStr))
                de.DateStr = data;
                switch (par1)
                {
                    default:
                        switch (_kod)
                        {
                            case "50":
                                te50.TimeStr = czasHMM;
                                break;
                            case "100":
                                te100.TimeStr = czasHMM;
                                break;
                            default:
                                if (lb50.Text != "0:00")
                                    te50.TimeStr = czasHMM;
                                else if (lb100.Text != "0:00")
                                    te100.TimeStr = czasHMM;
                                else
                                    te50.TimeStr = czasHMM;
                                break;

                        }
                        break;
                    case par1WolneKier:  // nie podstawiam bo w ddlKlas
                    case par1Sobota:
                        break;
                }
            }
        }
        //--
        protected void ddlZaDzienEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            string kod, data, pid, czas, czasHMM, p6;
            Tools.GetLineParams(ddl.SelectedValue, out kod, out data, out pid, out czas, out czasHMM, out p6);

            DateEdit de = ddl.Parent.FindControl("deZaDzien") as DateEdit;
            TimeEdit teD = ddl.Parent.FindControl("teN50") as TimeEdit;
            TimeEdit teN = ddl.Parent.FindControl("teN100") as TimeEdit;
            if (teD != null && teN != null && de != null)
            {
                //if (String.IsNullOrEmpty(teD.TimeStr))
                teD.TimeStr = czasHMM;
                de.DateStr = data;
            }
        }
        //--------------------
        protected void ddlKlas_ValidateInsert(object source, ServerValidateEventArgs args)
        {
            //args.IsValid = IsValid(lvPodzial.InsertItem, null);
            args.IsValid = true;
        }

        protected void ddlKlas_ValidateEdit(object source, ServerValidateEventArgs args)
        {
            //args.IsValid = IsValid(lvPodzial.EditItem, lvPodzial.DataKeys[lvPodzial.EditIndex].Value.ToString());
        }


        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        protected void ddlNadgInsert_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kod, data, pid, czas, czasHMM, p6;
            Tools.GetLineParams(((DropDownList)sender).SelectedValue, out kod, out data, out pid, out czas, out czasHMM, out p6);

            
            DropDownList ddl = (DropDownList)sender;

            TimeEdit teD = ddl.Parent.FindControl("teN50") as TimeEdit;
            TimeEdit teN = ddl.Parent.FindControl("teN100") as TimeEdit;
            if (teD != null && teN != null)
                if (String.IsNullOrEmpty(teD.TimeStr))
                    teD.TimeStr = czasHMM;
        }

        //-----------------------
        private bool UpdateItem(EventArgs ea, ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            values["RodzajId"] = Tools.GetDdlSelectedValueInt(item, "ddlKlas");
            values["AutorId"] = App.User.OriginalId;
            return true;
        }

        protected void lvPodzial_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e, e.Item, null, e.Values);
        }

        protected void lvPodzial_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(e, lvPodzial.EditItem, e.OldValues, e.NewValues);
        }

        private void HideInserted(HtmlTableRow tr1)
        {
            //----- ukryj inne insert -----
            foreach (ListViewItem lit in lvPodzial.Items)
            {
                HtmlTableRow tr = lit.FindControl("trLineInsert") as HtmlTableRow;
                if (tr != null && tr != tr1)
                    if (tr.Visible) tr.Visible = false;
                ShowInsertButton(lit, true);
            }
        }

        private void HideEdited()
        {
            //----- ukryj inne insert -----
            foreach (ListViewItem lit in lvPodzial.Items)
            {

            }
        }

        protected void lvPodzial_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            //lvPodzial.InsertItemPosition = InsertItemPosition.None;
            HideInserted(null);
        }

        private void TriggerChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        protected void lvPodzial_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            //lvPodzial.InsertItemPosition = InsertItemPosition.LastItem;
            TriggerChanged();
        }

        protected void lvPodzial_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            TriggerChanged();

        }

        protected void lvPodzial_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            TriggerChanged();

        }
        protected void lvPodzial_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            //lvPodzial.InsertItemPosition = InsertItemPosition.LastItem;
        }
        //-------------
        private void ShowInsertButton(ListViewItem item, bool visible)
        {
            if (visible)
            {
                int typ = db.getInt(Tools.GetText(item, "hidTyp"), -99);       // 1,2,3,4,5,6,9,12,13,14,15,16
                visible = typ == ttCzasPracy;
            }
            Tools.SetControlVisible(item, "InsertButton", visible);                
        }
       
        private void ShowInsert(ListViewItem item, bool visible)
        {
            HtmlTableRow tr1 = Tools.SetControlVisible(item, "trLineInsert", visible) as HtmlTableRow;
            if (visible)
            {
                //lvPodzial.EditIndex = -1;

                //----- ukryj inne insert -----
                HideInserted(tr1);
                HideEdited();

                DropDownList ddlKlas = item.FindControl("ddlKlas") as DropDownList;
                DropDownList ddlDaty = item.FindControl("ddlZaDzien") as DropDownList;
                hidDzien.Value = Tools.GetText(item, "hidData");
                hidIsAbsencja.Value = Tools.GetText(item, "hidIsAbsencja");

                TimeEdit te = item.FindControl("teNiedomiar") as TimeEdit;
                if (te != null) te.TimeStr = null;
                te = item.FindControl("teN50") as TimeEdit;
                if (te != null) te.TimeStr = null;
                te = item.FindControl("teN100") as TimeEdit;
                if (te != null) te.TimeStr = null;
                Tools.SetText2(item, "tbUwagi", null);

                if (ddlKlas != null && ddlDaty != null)
                {
                    string typ10 = Tools.GetText(item, "hidTyp10");
                    if (typ10 == "10")
                    {
                        ddlKlas.DataSourceID = "SqlDataSource2B";
                        ddlDaty.DataSourceID = "SqlDataSource4B";
                    }
                }
                ddlDaty.DataBind();
            }

            ShowInsertButton(item, !visible);
        }

        private void ShowEdit(ListViewItem item, bool visible)
        {
            if (visible) HideInserted(null);
            Tools.SetControlVisible(item, "deZaDzien2", visible);
            Tools.SetControlVisible(item, "lbZaDzien", !visible);
            Tools.SetControlVisible(item, "EditButton2", !visible);
            Tools.SetControlVisible(item, "btEditSave", visible);
            Tools.SetControlVisible(item, "btEditCancel", visible);
        }

        private int x_GetSobota100(string data)
        {
            DataRow dr = db.getDataRow(String.Format("select * from VRozliczenieNadgodzinDzienSuma where IdPracownika = {0} and Data = '{1}'", PracId, data));  // jak nie ma dnia to 0 !!!
            return db.getInt(dr, "N100", 0);
        }

        private bool GetNadgodziny(string data, out int n50, out int n100)
        {
            DataRow dr = db.getDataRow(String.Format("select * from VRozliczenieNadgodzinDzienSuma where IdPracownika = {0} and Data = '{1}'", PracId, data));   // jak nie ma dnia to 0 !!!
            if (dr != null)
            {
                n50 = db.getInt(dr, "N50", 0);
                n100 = db.getInt(dr, "N100", 0);
                return true;
            }
            else
            {
                n50 = 0;
                n100 = 0;
                return false;
            }
        }

        
        
        
        
        
        
        
        
        
        private bool PozaOkresem(string zadzien)
        {
            if (!String.IsNullOrEmpty(zadzien))
            {
                DateTime? dt = Tools.StrToDateTime(zadzien);
                if (dt != null)
                {
                    DateTime ood = (DateTime)Tools.StrToDateTime(OkresRozlOd);   // podwójna konwerjsa ... zmienić docelowo na DateTime? (nullable)
                    DateTime odo = (DateTime)Tools.StrToDateTime(OkresRozlDo);
                    //return ood <= (DateTime)dt && (DateTime)dt <= odo;
                    return (DateTime)dt < ood || odo < (DateTime)dt;
                }
            }
            return false;
        }
        
        
        
        private int DayInsert(ListViewItem item)
        {
            int ret = -2;
            int? typ = Tools.GetDdlSelectedValueInt(item, "ddlKlas");
            if (typ != null)
            {
                int t = (int)typ;
                string zadzien = null;
                int sNiedomiar = 0;
                int sN50 = 0;
                int sN100 = 0;

                TimeEdit te = item.FindControl("teN50") as TimeEdit;
                if (te != null) sN50 = db.getInt(te.Seconds, 0);
                te = item.FindControl("teN100") as TimeEdit;
                if (te != null) sN100 = db.getInt(te.Seconds, 0);

                int limit50w = 0;
                int limit100w = 0;
                int limit50p = 0;
                int limit100p = 0;

                try
                {
                    limit50w = int.Parse((item.FindControl("lbFN50WnWypl") as HiddenField).Value);
                    limit100w = int.Parse((item.FindControl("lbFN100WnWypl") as HiddenField).Value);
                    limit50p = int.Parse((item.FindControl("lbFN50WnPrzezn") as HiddenField).Value);
                    limit100p = int.Parse((item.FindControl("lbFN100WnPrzezn") as HiddenField).Value);
                }
                catch { }

                if (sN50 == 0 && sN100 == 0) ret = -3;   // 
                else
                {
                    string uwagi = Tools.GetText(item, "tbUwagi");
                    string data = Tools.GetText(item, "hidData");

                    switch (t)   // <<< wypadałoby to uzależnić od Kody.Par1
                    {
                        //----- Par1 = 0 -----
                        case _ttDoWyplaty:
                        case ttDoWyplatyWnNadg:
                            if (sN50 > limit50w || sN100 > limit100w) return -5;
                            break;
                        //----- Par1 = 1 -----
                        case _ttOdpracowanie:        //3 ok
                        case ttOdpracowanieWnNadg:        //8 ok
                        case _ttDoOdpracowania:      //13
                        case ttDoOdpracowaniaWnNadg:      //18
                        case ttWolneZaNadgPrac:     //12
                        case ttDoWybraniaPrac:      //2 ok w przyszłości 
                            sNiedomiar = sN50 + sN100;
                            DateEdit de = item.FindControl("deZaDzien") as DateEdit;
                            if (de != null)
                            {
                                zadzien = de.DateStr;
                                if (PozaOkresem(zadzien)) return -4;
                            }
                            if (sN50 > limit50p || sN100 > limit100p) return -5;
                            break;
                        //----- Par1 = 15 -----
                        case ttWolneZaNadgKier:     //14
                        case ttDoWybraniaKier:      //4 ok
                            sNiedomiar = Worktime.RoundSec((sN50 + sN100) * 3 / 2, 1, 2);
                            de = item.FindControl("deZaDzien") as DateEdit;
                            if (de != null)
                            {
                                zadzien = de.DateStr;
                                if (PozaOkresem(zadzien)) return -4;
                            }
                            if (sN50 > limit50p || sN100 > limit100p) return -5;
                            break;
                        //----- Par1 = 8 -----
                        case ttWolneZaSobote:       //15
                            de = item.FindControl("deZaDzien") as DateEdit;
                            if (de != null)
                            {
                                zadzien = de.DateStr;
                                if (PozaOkresem(zadzien)) return -4;
                            }
                            // za sobotę: - niedomiar 8h, 50 - 0, 100 - niezbilansowanie z soboty, do ilosc 8h ?, potem nocne
                            string wym = Tools.GetText(item, "hidWymiarEtat");
                            sNiedomiar = Tools.StrToInt(wym, 28800);
                            sN50 = 0;

                            int ss50, ss100;
                            GetNadgodziny(zadzien, out ss50, out ss100);    // nadpisuje ...
                            if (ss100 > 28800) ss100 = 28800;               // tylko 100 do wymiaru 8h są rozliczane za cały dzień, reszta możliwa do wybrania 1:1
                            if (ss100 != 0) sN100 = ss100;
                            break;
                        case ttDoOdpracowaniaWSobote://16
                            de = item.FindControl("deZaDzien") as DateEdit;
                            if (de != null)
                            {
                                zadzien = de.DateStr;
                                if (PozaOkresem(zadzien)) return -4;
                            }
                            wym = Tools.GetText(item, "hidWymiarEtat");
                            sNiedomiar = Tools.StrToInt(wym, 28800);
                            break;
                        case ttOdpracowanieSobota:  //6 ok
                        case ttDoWybraniaSobota:    //5 ok    
                            de = item.FindControl("deZaDzien") as DateEdit;
                            if (de != null)
                            {
                                zadzien = de.DateStr;                   // za sobotę: - niedomiar 8h, 50 - 0, 100 - niezbilansowanie z soboty, do ilosc 8h ?, potem nocne
                                if (PozaOkresem(zadzien)) return -4;
                            }
                            wym = Tools.GetText(item, "hidWymiarEtat");
                            sNiedomiar = Tools.StrToInt(wym, 28800);
                            sN50 = 0;

                            GetNadgodziny(zadzien, out ss50, out ss100);    // nadpisuje ...
                            if (ss100 > 28800) ss100 = 28800;               // tylko 100 do wymiaru 8h są rozliczane za cały dzień, reszta możliwa do wybrania 1:1
                            if (ss100 != 0) sN100 = ss100;
                            break;

                        //----- Par1 = 9 -----
                        case ttKorekta:
                            te = item.FindControl("teNiedomiar") as TimeEdit;
                            if (te != null) sNiedomiar = db.getInt(te.Seconds, 0);
                            break;
                    }

                    //if (PozaOkresem(zadzien)) ret = -4;
                    //else

                    ret = db.insert("PodzialNadgodzin", true, true, 0, "IdPracownika,Data,RodzajId,Uwagi,ZaDzien,CzasZm,n50,n100,DataWpisu,AutorId",
                        PracId, db.strParam(data), t, db.strParam(uwagi), db.nullStrParam(zadzien), sNiedomiar, sN50, sN100, "GETDATE()", App.User.OriginalId);
                }
            }
            return ret;
        }








        //------------------------
        private void DoWyplatyAll(int nadg)
        {
            DataSet ds = db.getDataSet(String.Format(@"
select *, 
CONVERT(varchar(10), Data, 20) + ' - ' + CONVERT(varchar, cast(N50 as float) / 3600) as DataN50,
CONVERT(varchar(10), Data, 20) + ' - ' + CONVERT(varchar, cast(N50 as float) / 3600) as DataN100
from VRozliczenieNadgodzinKartoteka 
where IdPracownika = {0} 
and Data between '{1}' and '{2}'
and Typ = 100
and {3}
            ", PracId, OkresOd, OkresDo, nadg == 100 ? "N100 > 0" : "N50 > 0"));
            if (db.getCount(ds) > 0)
            {
                bool ok = db.execSQL(String.Format(@"
insert into PodzialNadgodzin (IdPracownika,Data,RodzajId,Uwagi,ZaDzien,CzasZm,n50,n100,DataWpisu,AutorId)
select IdPracownika, Data, 1, 'nierozliczone, do wypłaty', null, null, {4}, {5}, GETDATE(), {6} 
from VRozliczenieNadgodzinKartoteka 
where IdPracownika = {0} 
and Data between '{1}' and '{2}'
and Typ = 100
and {3}
                  ", PracId, OkresOd, OkresDo, 
                     nadg == 100 ? "N100 > 0" : "N50 > 0",
                     nadg == 100 ? "0" : "N50",
                     nadg == 100 ? "N100" : "0",
                     App.User.OriginalId));
                switch (FilterId)
                {
                    case "5":
                        ReloadFilter();
                        break;
                }
                lvPodzial.DataBind();
                Updated = true;
                TriggerChanged();

                if (ok)
                    Log.Info(Log.ROZLICZENIENG, String.Format("Do wypłaty {0}", nadg), db.Join(ds, nadg == 100 ? "DataN100" : "DataN50", "; "));
                else
                    Tools.ShowError("Wystąpił błąd podczas wykonywania operacji.");
            }
            else Tools.ShowMessage("Brak nierozliczonych nadgodzin.");
        }
        //------------------------
        private bool UpdateWniosek(ListViewItem item)
        {
            bool ok = false;
            DateEdit de = Tools.FindControl(item, "deZaDzien2") as DateEdit;
            if (de != null)
                if (de.IsValid)
                {
                    string wid = Tools.GetDataKey(lvPodzial, item);
                    if (wid.StartsWith("-"))
                    {
                        wid = wid.Substring(1);
                        ok = db.execSQL(String.Format(dsWniosekZaDzien.UpdateCommand, wid, de.DateStr));
                        if (!ok)
                            Tools.ShowError("Wystąpił błąd podczas aktualizacji wniosku.");
                    }
                }
            return ok;
        }

        protected void lvPodzial_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "daynewrecord":
                    ShowInsert(e.Item, true);
                    break;
                case "daycancel":
                    ShowInsert(e.Item, false);
                    break;
                case "dayinsert":
                    int err = DayInsert(e.Item);
                    switch (err)
                    {
                        default: 
                            ShowInsert(e.Item, false);
                            ReloadFilter();
                            lvPodzial.DataBind();
                            Updated = true;
                            TriggerChanged();
                            break;
                        case -1:
                            Tools.ShowError("Wpis za podany dzień już istnieje.");
                            break;
                        case -2:
                            Tools.ShowError("Wystąpił błąd podczas dodawania rekordu.");
                            break;
                        case -3:
                            Tools.ShowError("Brak ilości nadgodzin.");
                            break;
                        case -4:
                            Tools.ShowError("Data nie może być spoza okresu rozliczeniowego.");
                            break;
                        case -5:
                            Tools.ShowError("Wpisane wartości przekraczają limit nadgodzin wyznaczony przez wnioski.\n\nZawnioskuj o dodatkowe nadgodziny lub dokonaj korekty czasu pracy."/*"Podane nadgodziny przekraczają ilości wyznaczone przez wnioski. Zawnioskuj o dodatkowe nadgodziny, aby dokończyć operację."*/);
                            break;
                    }
                    break;
                case "wniosek":
                    PrepareWniosek(e.CommandArgument.ToString(), cntWniosekUrlopowy.wtOD);
                    break;
                case "wniosek2":
                    PrepareWniosekOdpr(e.CommandArgument.ToString(), cntWniosekUrlopowy.wtODPR);
                    break;
                case "wniosek2a":
                    DateEdit de = (DateEdit)lvPodzial.FindControl("deWnNaDzien");
                    if (de != null)
                        if (de.IsValid)
                            PrepareWniosekOdpr(String.Format("|{0}", de.DateStr), cntWniosekUrlopowy.wtODPR);
                        else
                            Tools.ShowError("Proszę podać poprawną datę.");
                    break;
                case "dowyplatyall50":
                    if (doWyplatyAll) DoWyplatyAll(50);
                    break;
                case "dowyplatyall100":
                    if (doWyplatyAll) DoWyplatyAll(100);
                    break;
                //--------------
                case "Edit2":
                    ShowEdit(e.Item, true);
                    break;
                case "daysave2":
                    if (UpdateWniosek(e.Item))
                    {
                        ShowEdit(e.Item, false);
                        lvPodzial.DataBind();
                        Updated = true;
                        TriggerChanged();
                    }
                    break;
                case "daycancel2":
                    ShowEdit(e.Item, false);
                    break;
            }
        }

        //-------------
        /*
        private void ShowCzasPracy()
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            Tools.SetText(lvPodzial, "lbCzasZm", CzasZm == null ? null : Worktime.SecToTime((int)CzasZm, settings.Zaokr));
            Tools.SetText(lvPodzial, "lbNadgD", NadgD == null ? null : Worktime.SecToTime((int)NadgD, settings.Zaokr)); 
            Tools.SetText(lvPodzial, "lbNadgN", NadgN == null ? null : Worktime.SecToTime((int)NadgN, settings.Zaokr));
            Tools.SetText(lvPodzial, "lbNocne", Nocne == null ? null : Worktime.SecToTime((int)Nocne, settings.Zaokr)); 
        }
        */



        /*
        private void SetCzasPracy(string czasZm, string nadgD, string nadgN, string nocne)
        {
            Tools.SetText(lvPodzial, "lbCzasZm", String.IsNullOrEmpty(czasZm) ? hidCzasZm.Value : czasZm);
            Tools.SetText(lvPodzial, "lbNadgD", String.IsNullOrEmpty(nadgD) ? hidNadgD.Value : nadgD);
            Tools.SetText(lvPodzial, "lbNadgN", String.IsNullOrEmpty(nadgN) ? hidNadgN.Value : nadgN);
            Tools.SetText(lvPodzial, "lbNocne", String.IsNullOrEmpty(nocne) ? hidNocne.Value : nocne);
        }

        private void InitCzasPracy(int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            //wartości startowe
            hidCzasZm.Value = czasZm == null ? null : Worktime.SecToTime((int)czasZm, settings.Zaokr);
            hidNadgD.Value = nadgD == null ? null : Worktime.SecToTime((int)nadgD, settings.Zaokr);
            hidNadgN.Value = nadgN == null ? null : Worktime.SecToTime((int)nadgN, settings.Zaokr);
            hidNocne.Value = nocne == null ? null : Worktime.SecToTime((int)nocne, settings.Zaokr);
            SetCzasPracy(null, null, null, null);
        }
        */
        //---------------------
        /*
        public bool IsValid(string excludeId, IOrderedDictionary values)
        {
            if (String.IsNullOrEmpty(PlanId)) return false;
            {
                string excl = String.IsNullOrEmpty(excludeId) ? "" : " and Id <> " + excludeId;
                DataRow dr = db.getDataRow(String.Format(
                    //      0                     1                                 2                               3 
                    "select sum(CzasZm) as SumZm, sum(NadgodzinyDzien) as SumNadgD, sum(NadgodzinyNoc) as SumNadgN, sum(Nocne) as Nocne " +
                    "from PodzialKosztow where IdPlanPracy = {0}{1}",
                    PlanId, excl));
                int zm = db.getInt(dr, 0, 0) + (values == null ? 0 : db.ISNULL(values["CzasZm"], 0));
                int nd = db.getInt(dr, 1, 0) + (values == null ? 0 : db.ISNULL(values["NadgodzinyDzien"], 0));
                int nn = db.getInt(dr, 2, 0) + (values == null ? 0 : db.ISNULL(values["NadgodzinyNoc"], 0));
                int noc = db.getInt(dr, 3, 0) + (values == null ? 0 : db.ISNULL(values["Nocne"], 0));
                bool c1 = zm <= CzasZm;
                bool c2 = nd <= NadgD;
                bool c3 = nn <= NadgN;
                bool c4 = noc <= Nocne;
                bool c5 = nn <= noc;

                if (c1 && c2 && c3 && c4 && c5) return true;
                else
                {
                    string pp = null;
                    if (!c1) pp += "\\n- czas na zmianie";
                    if (!c2) pp += "\\n- nadgodziny w dzień";
                    if (!c3) pp += "\\n- nadgodziny w nocy";
                    if (!c4) pp += "\\n- czas pracy w nocy";
                    if (!c5) pp += "\\n- nadgodziny w nocy większe od czasu pracy w nocy";
                    Tools.ShowMessage("Przekroczony czas pracy:{0}", pp);
                    return false;
                }
            }
        }
        */







        /*
        public static void GetSumy(string pracId, string data, string excludeId, out int zm, out int nadgD, out int nadgN, out int noc)
        {
            string excl = String.IsNullOrEmpty(excludeId) ? "" : " and Id <> " + excludeId;
            DataRow dr = db.getDataRow(String.Format(
                //      0            1                     2                   3 
                "select sum(CzasZm), sum(NadgodzinyDzien), sum(NadgodzinyNoc), sum(Nocne) " +
                "from PodzialNadgodzin where IdPracownika={0} and Data='{1}'{2}",
                pracId, data, excl));
            if (dr != null)
            {
                zm = db.getInt(dr, 0, 0);
                nadgD = db.getInt(dr, 1, 0);
                nadgN = db.getInt(dr, 2, 0);
                noc = db.getInt(dr, 3, 0);
            }
            else
            {
                zm = 0;
                nadgD = 0;
                nadgN = 0;
                noc = 0;
            }
        }
        */





        /*
        public static int IsValid(string pracId, string data, string excludeId,     // exclude - którą pozycje pominąć przy sumowaniu, null jak wszystkie wziąć
                                  int kCzasZm, int kNadgD, int kNadgN, int kNocne,  // wartości rcp lub skorygowane przez kierownik
                                  int? czasZm, int? nadgD, int? nadgN, int? nocne)  // wartość wprowadzane
        {
            /* 20131203
            string excl = String.IsNullOrEmpty(excludeId) ? "" : " and Id <> " + excludeId;
            DataRow dr = db.getDataRow(String.Format(
                //      0                     1                                 2                               3 
                "select sum(CzasZm) as SumZm, sum(NadgodzinyDzien) as SumNadgD, sum(NadgodzinyNoc) as SumNadgN, sum(Nocne) as Nocne " +
                "from PodzialNadgodzin where IdPracownika={0} and Data='{1}'{2}",
                pracId, data, excl));
            int zm = db.getInt(dr, 0, 0) + (czasZm == null ? 0 : (int)czasZm);
            int nd = db.getInt(dr, 1, 0) + (nadgD == null ? 0 : (int)nadgD);
            int nn = db.getInt(dr, 2, 0) + (nadgN == null ? 0 : (int)nadgN);
            int noc = db.getInt(dr, 3, 0) + (nocne == null ? 0 : (int)nocne);
            * /
            int zm, nd, nn, noc;
            GetSumy(pracId, data, excludeId, out zm, out nd, out nn, out noc);
            if (czasZm != null) zm += (int)czasZm;
            if (nadgD != null)  nd += (int)nadgD;
            if (nadgN != null)  nn += (int)nadgN;
            if (nocne != null)  noc += (int)nocne; 
            
            bool c1 = zm <= kCzasZm;
            bool c2 = nd <= kNadgD;
            bool c3 = nn <= kNadgN;
            bool c4 = noc <= kNocne;
            bool c5 = nn <= noc;  
            int ret = 0;
            if (!c1) ret |= 0x0001;
            if (!c2) ret |= 0x0002;
            if (!c3) ret |= 0x0004;
            if (!c4) ret |= 0x0008;
            if (!c5) ret |= 0x0010;
            return ret;
        }
        */







        /*
        public int IsValid(string excludeId, int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            return IsValid(PracId, Data, excludeId, 
                           db.ISNULL(CzasZm, 0), db.ISNULL(NadgD, 0), db.ISNULL(NadgN, 0), db.ISNULL(Nocne, 0), 
                           czasZm, nadgD, nadgN, nocne);
        }
        */








        /*
        public static int IsValid(string pracId, string data, string excludeId, 
                                  int kCzasZm, int kNadgD, int kNadgN, int kNonce,
                                  int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            string excl = String.IsNullOrEmpty(excludeId) ? "" : " and Id <> " + excludeId;
            DataRow dr = db.getDataRow(String.Format(
                //      0                     1                                 2                               3 
                "select sum(CzasZm) as SumZm, sum(NadgodzinyDzien) as SumNadgD, sum(NadgodzinyNoc) as SumNadgN, sum(Nocne) as Nocne " +
                "from PodzialKosztow where IdPracownika={0} and Data='{1}'{2}",
                PracId, Data, excl));
            int zm = db.getInt(dr, 0, 0) + (czasZm == null ? 0 : (int)czasZm);
            int nd = db.getInt(dr, 1, 0) + (nadgD == null ? 0 : (int)nadgD);
            int nn = db.getInt(dr, 2, 0) + (nadgN == null ? 0 : (int)nadgN);
            int noc = db.getInt(dr, 3, 0) + (nocne == null ? 0 : (int)nocne); 
            bool c1 = zm <= CzasZm;
            bool c2 = nd <= NadgD;
            bool c3 = nn <= NadgN;
            bool c4 = noc <= Nocne;
            bool c5 = nn <= noc;  
            int ret = 0;
            if (!c1) ret |= 0x0001;
            if (!c2) ret |= 0x0002;
            if (!c3) ret |= 0x0004;
            if (!c4) ret |= 0x0008;
            if (!c5) ret |= 0x0010;
            return ret;
        }
         */

        private bool kRound(TimeEdit2 te, ref bool rounded)
        {
            if (te.IsEntered && te.Seconds != null)
            {
                /*
                int ss = (int)te.Seconds;
                int s = Worktime.RoundSec(ss, 30, 2);
                if (s != ss) rounded = true;
                te.Seconds = s;
                */
                return true;
            }
            else 
                return false;
        }

        private bool kRound(TimeEdit te, ref bool rounded)
        {
            if (te.IsEntered && te.Seconds != null)
            {
                /*
                int ss = (int)te.Seconds;
                int s = Worktime.RoundSec(ss, 30, 2);
                if (s != ss) rounded = true;
                te.Seconds = s;
                */
                return true;
            }
            else
                return false;
        }

        private bool DatesEq(string d1, string d2)
        {
            bool e1 = String.IsNullOrEmpty(d1);
            bool e2 = String.IsNullOrEmpty(d2);
            if (e1 && e2) return true;
            else if (!e1 && !e2) return false;
            else
            {
                DateTime? dt1 = Tools.StrToDateTime(d1);
                DateTime? dt2 = Tools.StrToDateTime(d2);
                if (dt1 != null && dt2 != null)
                    return dt1 == dt2;
            }
            return false;
        }








        /*
        private bool IsValid(ListViewItem item, string id)  //id == null przy insert
        {
            bool c0 = true;

            string kod, zadzien;
            Tools.GetLineParams(Tools.GetDdlSelectedValue(item, "ddlNadg"), out kod, out zadzien);


            if (!String.IsNullOrEmpty(id) && Tools.GetText(item, "hidRodzaj") == kod && DatesEq(Tools.GetText(item, "hidZaDzien"), zadzien)) 
                c0 = true;
            else
            {       
                string zad = null;
                if (!String.IsNullOrEmpty(zadzien))
                    zad = String.Format(" and ZaDzien = '{0}'", zadzien);

                DataRow dr = db.getDataRow(String.Format(
                    "select Id from PodzialNadgodzin where IdPracownika={0} and Data='{1}' and RodzajId={2}{3}",
                    PracId, Data, kod, zad));
                c0 = dr == null;   // nie jest jeszcze wybrane
            }


            /*
            TimeEdit2 te1 = (TimeEdit2)item.FindControl("teCzasZm");
            TimeEdit2 te2 = (TimeEdit2)item.FindControl("teNadgD");
            TimeEdit2 te3 = (TimeEdit2)item.FindControl("teNadgN");
            TimeEdit2 te4 = (TimeEdit2)item.FindControl("teNocne");
            /** /
            /** /
            TimeEdit te1 = (TimeEdit)item.FindControl("teCzasZm");
            TimeEdit te2 = (TimeEdit)item.FindControl("teNadgD");
            TimeEdit te3 = (TimeEdit)item.FindControl("teNadgN");
            TimeEdit te4 = (TimeEdit)item.FindControl("teNocne");
            /** /
            bool r = false;
            bool c1 = kRound(te1, ref r);   // IsEntered, ref rounded
            bool c2 = kRound(te2, ref r);
            bool c3 = kRound(te3, ref r);
            bool c4 = kRound(te4, ref r);
            bool c = !c1 && !c2 && !c3 && !c4;

            int err = 0;
            //----- weryfikacja - jak są dane -----
            if (Tools.StrToDateTime(Data, DateTime.MaxValue) <= DateTime.Today)  // jak błędna konwersja to ma byc weryfikacja
                err = IsValid(id, (int?)te1.Seconds, (int?)te2.Seconds, (int?)te3.Seconds, (int?)te4.Seconds);
            bool v1 = te1.Validate();
            bool v2 = te2.Validate();
            bool v3 = te3.Validate();
            bool v4 = te4.Validate(); 
            if (v1 && v2 && v3 && v4)
                if (c)
                {
                    te1.SetError(true, "Błąd");
                    te2.SetError(true, "Błąd");
                    te3.SetError(true, "Błąd");
                    te4.SetError(true, "Błąd");
                }
                else
                {
                    te1.SetError((err & 0x0001) > 0, "Przekroczenie");
                    te2.SetError((err & 0x0002) > 0, "Przekroczenie");
                    te3.SetError((err & 0x0004) > 0, "Przekroczenie");
                    if ((err & 0x0008) > 0)      te4.SetError(true, "Przekroczenie");
                    else if ((err & 0x0010) > 0) te4.SetError((err & 0x0018) > 0, "Brak");
                    else                         te4.SetError(false, null);
                }
            CustomValidator cv = (CustomValidator)item.FindControl("cvNadg");
            if (!c0) cv.ErrorMessage = "Powtórzona klasyfikacja";
            else cv.ErrorMessage = null;
            return c0 && !c && err == 0;
        }
        */









        //---------------------
        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string OkresOd
        {
            get { return hidOkresOd.Value; }
            set { hidOkresOd.Value = value; }
        }

        public string OkresDo
        {
            get { return hidOkresDo.Value; }
            set { hidOkresDo.Value = value; }
        }

        public string OkresRozlOd
        {
            get { return Tools.GetStr(ViewState["orozlod"]); }
            set { ViewState["orozlod"] = value; }
        }

        public string OkresRozlDo
        {
            get { return Tools.GetStr(ViewState["orozldo"]); }
            set { ViewState["orozldo"] = value; }
        }

        //---------------------
        public bool Updated
        {
            get { return Tools.GetBool(ViewState["updated"], false); }
            set { ViewState["updated"] = value; }
        }

        public bool ReadOnly
        {
            get { return Tools.GetBool(ViewState["readonly"],  true); }
            set { ViewState["readonly"] = value; }
        }





















        /*
        public bool InEditMode
        {
            get { return lvPodzial.EditIndex != -1; }
            set 
            {
                if (value)
                    lvPodzial.EditIndex = lvPodzial.SelectedIndex;
                else
                    lvPodzial.EditIndex = -1;
            }  
        }
        
        public string _PlanId
        {
            get { return hidPlanId.Value; }
            set { hidPlanId.Value = value; }
        }

        public string _Data
        {
            get { return hidData.Value; }
            set { hidData.Value = value; }
        }

        //-----
        public int? _CzasZmPP
        {
            get { return (int?)ViewState["czaszmpp"]; }
            set { ViewState["czaszmpp"] = value; }
        }

        public int? _NadgDPP
        {
            get { return (int?)ViewState["nadgdpp"]; }
            set { ViewState["nadgdpp"] = value; }
        }

        public int? _NadgNPP
        {
            get { return (int?)ViewState["nadgnpp"]; }
            set { ViewState["nadgnpp"] = value; }
        }

        public int? _NocnePP
        {
            get { return (int?)ViewState["nocnepp"]; }
            set { ViewState["nocnepp"] = value; }
        }
        //-----
        public int? _CzasZm
        {
            get { return ViewState["czaszm"] != null ? (int?)ViewState["czaszm"] : CzasZmPP; }
            set { ViewState["czaszm"] = value; }
        }

        public int? _NadgD
        {
            get { return ViewState["nadgd"] != null ? (int?)ViewState["nadgd"] : NadgDPP; }
            set { ViewState["nadgd"] = value; }
        }

        public int? _NadgN
        {
            get { return ViewState["nadgn"] != null ? (int?)ViewState["nadgn"] : NadgNPP; }
            set { ViewState["nadgn"] = value; }
        }

        public int? _Nocne
        {
            get { return ViewState["nocne"] != null ? (int?)ViewState["nocne"] : NocnePP; }
            set { ViewState["nocne"] = value; }
        }
        */
        //-----------------------------------------------
        /*
        private int PrepareWniosek()
        {
            string dni = null;
            string idList = null;

            int cnt = -1;
            foreach (ListViewItem item in lvPodzial.Items)
            {
                CheckBox cb = item.FindControl("cbSelect") as CheckBox;
                if (cb != null)
                    if (cb.Visible)
                    {
                        if (cnt == -1) cnt = 0;
                        if (cb.Checked)
                        {
                            Label lbData = item.FindControl("lbZaDzien") as Label;
                            HiddenField lbId = item.FindControl("hidId") as HiddenField;
                            if (lbData != null && lbId != null)
                            {
                                if (String.IsNullOrEmpty(dni))
                                {
                                    dni = lbData.Text;
                                    idList = lbId.Value;
                                }
                                else
                                {
                                    dni += ", " + lbData.Text;
                                    idList += "," + lbId.Value;
                                }
                                cnt++;
                            }
                        }
                    }
            }
            if (cnt > 0)
            {
                bool ok = false;
                /*
                int id = db.insert("poWnioskiUrlopowe", true, true, 0, 
                            "", 
                            );
                if (id != -1)
                    ok = db.update("PodzialNadgodzin", 0, "WniosekId", 
                        String.Format("Id in ({0})", idList), 
                        id);
                * /
                if (ok) 
                {
                    Tools.ShowMessage("Wniosek został utworzony i jest dostępny na zakładce wnioski urlopowe.");

                    // pokaż wniosek
                }
                else
                    Tools.ShowMessage("Wystąpił błąd podczas generowania wniosku");
            }
            return cnt;
        }
         
        protected void btWniosek_Click(object sender, EventArgs e)
        {
            switch (PrepareWniosek())
            {
                case 0:
                    Tools.ShowMessage("Proszę zaznaczyć pozycje do umieszczenia na wniosku");
                    break;
                case -1:
                    Tools.ShowMessage("Brak pozycji do przygotowania wniosku");
                    break;
                default:
                    break;
            }
        }
        */




        private int PrepareWniosek(string iddata, int wntypid)  // wnioskuje przełożony PodTyp = 2, jak pracownik to PodTyp = 1
        {
            int cnt = 0;
            string wid, wtyp, data;
            Tools.GetLineParams(iddata, out wid, out wtyp, out data);

            if (!String.IsNullOrEmpty(wid))
            {
                DataRow dr = db.getDataRow("select * from poWnioskiUrlopowe where Id = " + wid);
                if (dr == null) wid = null;
            }

            if (String.IsNullOrEmpty(wid))
            {
                DataSet ds = db.getDataSet(String.Format(@"
select PN.*, convert(varchar(10), PN.Data, 20) +  
    case when dbo.dow(PN.Data) = 5 then ' - Sobota' else '' end + 
    ' (' + dbo.ToTimeHMM(PN.n50 + PN.n100) +')' as Info 
from PodzialNadgodzin PN
left join Kody K on K.Typ = 'PODZNADG' and Kod = PN.RodzajId
where PN.IdPracownika = {0} 
and K.Par2 = 2 
--and PN.RodzajId in (2,4,5)
and PN.ZaDzien = '{1}'
union 
select PN.*, convert(varchar(10), PN.ZaDzien, 20) + 
    case when dbo.dow(PN.Data) = 5 then ' - Sobota' else '' end + 
    ' (' + dbo.ToTimeHMM(PN.n50 + PN.n100) +')' as Info  
from PodzialNadgodzin PN
left join Kody K on K.Typ = 'PODZNADG' and Kod = PN.RodzajId
where PN.IdPracownika = {0} 
and K.Par2 = 2 
--and PN.RodzajId in (12,14,15)
and PN.Data = '{1}'
order by Info
                ", PracId, data));

                /*
select PN.*, convert(varchar(10), PN.Data, 20) + ' (' + dbo.ToTimeHMM(PN.n50 + PN.n100) +')' as Info 
from PodzialNadgodzin PN
left join Kody K on K.Typ = 'PODZNADG' and Kod = PN.RodzajId
where PN.IdPracownika = {0} and K.Par2 = 2 and PN.ZaDzien = '{1}'
union 
select PN.*, convert(varchar(10), PN.ZaDzien, 20) + ' (' + dbo.ToTimeHMM(PN.n50 + PN.n100) +')' as Info  
from PodzialNadgodzin PN
left join Kody K on K.Typ = 'PODZNADG' and Kod = PN.RodzajId
where PN.IdPracownika = {0} and K.Par2 = 2 and PN.Data = '{1}'
order by Info
                 */

                cnt = db.getCount(ds);
                if (cnt > 0)
                {
                    if (String.IsNullOrEmpty(wtyp)) wtyp = "1";  // 1 - na wniosek pracownika, 2 - na wniosek kierownika -> x1.5

                    string idList = db.Join(ds, "Id", ",");
                    string info = db.Join(ds, "Info", ", ");
                    //int godz = Tools.StrToInt(db.getScalar(String.Format("select dbo.RoundSec(sum(n50 + n100), 60, 2) / 3600 from PodzialNadgodzin where Id in ({0})", idList)), 0);  // wgórę na korzyść
                    //int godz = Tools.StrToInt(db.getScalar(String.Format("select dbo.RoundSec(sum(n50 + n100), 1, 2) from PodzialNadgodzin where Id in ({0})", idList)), 0);  // wgórę na korzyść

                    int godzNadg = Tools.StrToInt(db.getScalar(String.Format("select sum(n50 + n100) from PodzialNadgodzin where Id in ({0})", idList)), 0);
                    int godz = Tools.StrToInt(db.getScalar(String.Format("select sum(CzasZm) from PodzialNadgodzin where Id in ({0})", idList)), 0);

                    //string kierId = db.getScalar(String.Format("select KierId from VPrzypisaniaNaDzis where Id = {0}", PracId));
                    string kierId = db.getScalar(String.Format("select top 1 IdKierownika from Przypisania where IdPracownika = {0} and Od <= '{1}' and Status = 1 order by Od desc", PracId, data));
                    if (String.IsNullOrEmpty(kierId)) kierId = "0";

                    bool ok = false;
                    int id = db.insert("poWnioskiUrlopowe", true, true, 0,
                                "TypId,PodTyp,AutorId,DataWniosku,IdPracownika,IdPrzelozony,ProjektDzial,Stanowisko,Info,UzasadnieniePrac," +
                                "Od,Do,Godzin,Godzin2,Dni,IdZastepuje,Zastepuje," +
                                "IdKierAcc,IdKierAccZast,DataKierAcc,UzasadnienieKier,IdKadryAcc,DataKadryAcc,UwagiKadry,StatusId,Wprowadzony,DataImportu",
                                wntypid, wtyp, App.User.Id, "GETDATE()", PracId, kierId, db.NULL, db.NULL, db.strParam(info), db.strParam(info),
                                db.strParam(data), db.strParam(data), godz, godzNadg, 1, db.NULL, db.NULL,
                                App.User.Id, db.NULL, "GETDATE()", db.NULL, db.NULL, db.NULL, db.NULL, 1, 0, db.NULL);
                    if (id != -1)
                        ok = db.update("PodzialNadgodzin", 0, "WniosekId",
                            String.Format("Id in ({0})", idList),
                            id);
                    if (ok)
                    {
                        wid = id.ToString();
                        //Tools.ShowMessage("Wniosek został utworzony i jest dostępny na zakładce wnioski urlopowe.");

                        // pokaż wniosek
                    }
                    else
                        Tools.ShowMessage("Wystąpił błąd podczas generowania wniosku");
                }
                else
                    Tools.ShowMessage("Brak pozycji do zamieszczenia na wniosku");

            }
            if (!String.IsNullOrEmpty(wid))
            {
                /*
                cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, Tools.StrToInt(wid, -1), HRRcp.Controls.Portal.cntWniosekUrlopowy.osPracownik, false);
                UpdatePanel5.Update();    // jezeli wywołanie jest z updatepanel to trzeba też zrobić update
                */
                switch (wntypid)
                {
                    case cntWniosekUrlopowy.wtOD:
                        WniosekWolneZaNadg.Show(wid);
                        break;
                    case cntWniosekUrlopowy.wtODPR:
                        WniosekOdpracowanie.Show(wid);
                        break;
                }
            }

            return cnt;
        }

        private int PrepareWniosekOdpr(string iddata, int wntypid)
        {
            int cnt = 0;
            string wid, data;
            Tools.GetLineParams(iddata, out wid, out data);

            wid = db.getScalar(String.Format("select Id from poWnioskiUrlopowe where IdPracownika = {0} and Od = '{1}' and TypId = {2}", PracId, data, cntWniosekUrlopowy.wtODPR));

            //string kierId = db.getScalar(String.Format("select KierId from VPrzypisaniaNaDzis where Id = {0}", PracId));
            string kierId = db.getScalar(String.Format("select top 1 IdKierownika from Przypisania where IdPracownika = {0} and Od <= '{1}' and Status = 1 order by Od desc", PracId, data));
            if (String.IsNullOrEmpty(kierId)) kierId = "0";

            bool ok = false;
            string info = null;
            
            
            int godz = 0;
 

            if (String.IsNullOrEmpty(wid))
            {
                int id = db.insert("poWnioskiUrlopowe", true, true, 0,
                            "TypId,AutorId,DataWniosku,IdPracownika,IdPrzelozony,ProjektDzial,Stanowisko,Info,UzasadnieniePrac," +
                            "Od,Do,Godzin,Dni,IdZastepuje,Zastepuje," +
                            "IdKierAcc,IdKierAccZast,DataKierAcc,UzasadnienieKier,IdKadryAcc,DataKadryAcc,UwagiKadry,StatusId,Wprowadzony,DataImportu",
                            wntypid, App.User.Id, "GETDATE()", PracId, kierId, db.NULL, db.NULL, db.strParam(info), db.strParam(info),
                            db.strParam(data), db.strParam(data), godz, 1, db.NULL, db.NULL,
                            App.User.Id, db.NULL, "GETDATE()", db.NULL, db.NULL, db.NULL, db.NULL, 1, 0, db.NULL);
                if (id != -1)
                {
                    wid = id.ToString();
                    ok = true;
                }
            }
            else
            {
                ok = db.update("poWnioskiUrlopowe", 0,
                            "TypId,AutorId,DataWniosku,IdPracownika,IdPrzelozony,ProjektDzial,Stanowisko,Info,UzasadnieniePrac," +
                            "Od,Do,Godzin,Dni,IdZastepuje,Zastepuje," +
                            "IdKierAcc,IdKierAccZast,DataKierAcc,UzasadnienieKier,IdKadryAcc,DataKadryAcc,UwagiKadry,StatusId,Wprowadzony,DataImportu",
                            "Id=" + wid,
                            wntypid, App.User.Id, "GETDATE()", PracId, kierId, db.NULL, db.NULL, db.strParam(info), db.strParam(info),
                            db.strParam(data), db.strParam(data), godz, 1, db.NULL, db.NULL,
                            App.User.Id, db.NULL, "GETDATE()", db.NULL, db.NULL, db.NULL, db.NULL, 1, 0, db.NULL);
            }
            if (ok)
            {
                //Tools.ShowMessage("Wniosek został utworzony i jest dostępny na zakładce wnioski urlopowe.");

                WniosekOdpracowanie.Show(wid);
            }
            else
                Tools.ShowMessage("Wystąpił błąd podczas generowania wniosku");

            return cnt;
        }

        protected void cntWnioskiUrlopowe1_Hide(object sender, EventArgs e)
        {
            cntWniosekUrlopowy1._Clear();
            //mvWnioskiUrlopowe.SetActiveView(vSelect);
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
            cntWniosekUrlopowy1._Clear();
            //mvWnioskiUrlopowe.SetActiveView(vSelect);
            /*
            if (cntWniosekUrlopowy1.Updated)
            {
                cntWnioskiUrlopowe1.DataBind();   // mozna sprawdzić czy dodał
                UpdatePanel1.Update();
            }
             */ 
        }

        private void ReloadFilter()
        {
            SetFilter(FilterId);
        }

        public void SetFilter(string fid)
        {
            const string select = "select '''x''' as Data union select '''' + convert(varchar(10), Data, 20) + '''' as Data from VRozliczenieNadgodzinKartoteka where IdPracownika = {0} and Data between '{1}' and '{2}' ";

            FilterId = fid;
            switch (fid)
            {
                default:
                case "1":   //Wszystko
                    FilterExpression = null;
                    break;
                case "2":   //Nierozliczone
                    //FilterExpression = String.Format("Data in (select Data from VRozliczenieNadgodzinKartoteka where IdPracownika = {0} and Typ = 100 and (Niedomiar <> 0 or N50 <> 0 or N100 <> 0))", PracId);
                    //FilterExpression = String.Format("DataStr in ('2014-01-01','2014-01-02','2014-01-03')", PracId);
                    DataSet ds = db.getDataSet(String.Format(select + "and Typ = 100 and (Niedomiar <> 0 or N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;

                case "21":  // nierozliczony niedomiar
                    ds = db.getDataSet(String.Format(select + "and Typ = 100 and (Niedomiar <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                case "22":  // nierozliczone nadgodziny
                    ds = db.getDataSet(String.Format(select + "and Typ = 100 and (N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;

                case "3":   //Niedomiar
                    //FilterExpression = String.Format("Data in (select Data from VRozliczenieNadgodzinKartoteka where IdPracownika = {0} and Typ = 100 and (Niedomiar <> 0))", PracId);
                    ds = db.getDataSet(String.Format(select + "and Typ = -10 and (Niedomiar <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                case "4":   //Nadgodziny
                    //FilterExpression = String.Format("Data in (select Data from VRozliczenieNadgodzinKartoteka where IdPracownika = {0} and Typ = 100 and (N50 <> 0 or N100 <> 0))", PracId);
                    ds = db.getDataSet(String.Format(select + "and Typ = -10 and (N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                case "5":   //Do wypłaty
                    //FilterExpression = String.Format("Data in (select Data from VRozliczenieNadgodzinKartoteka where IdPracownika = {0} and Typ = 1 and (N50 <> 0 or N100 <> 0))", PracId);
                    ds = db.getDataSet(String.Format(select + "and Typ = 1 and (N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                case "6":   //Do odpracowania
                    //FilterExpression = String.Format("Data in (select Data from VRozliczenieNadgodzinKartoteka where IdPracownika = {0} and Typ in (3,13) and (N50 <> 0 or N100 <> 0))", PracId);
                    ds = db.getDataSet(String.Format(select + "and Typ in (3,13,6,16) and (Niedomiar <> 0 or N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                case "7":   // wnioski o odboir dni wolnych
                    //FilterExpression = String.Format("Data in (select Data from VRozliczenieNadgodzinKartoteka where IdPracownika = {0} and Typ in (2,12) and (N50 <> 0 or N100 <> 0))", PracId);
                    ds = db.getDataSet(String.Format(select + "and Typ in (2,12,4,14,5,15) and (Niedomiar <> 0 or N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                case "8":   //Do rozliczenia: Niedomiar + Nadgodziny
                    ds = db.getDataSet(String.Format(select + "and Typ in (-10,100) and ((Niedomiar <> 0) or N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                    /*  >>> najprosciej byloby po PodzialNadgodzin i komplemantarne daty 
                case "9":   //Rozliczone
                    ds = db.getDataSet(String.Format(select + "and Typ = -10 and ((Niedomiar <> 0) or N50 <> 0 or N100 <> 0)", PracId, OkresOd, OkresDo));
                    FilterExpression = String.Format("DataStr in ({0})", db.Join(ds, 0, ","));
                    break;
                     * */
            }
        }

        public string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                //Deselect();
                lvPodzial.EditIndex = -1;
                lvPodzial.InsertItemPosition = InsertItemPosition.None;
                SqlDataSource1.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        public string FilterId
        {
            set { ViewState["filterid"] = value; }
            get { return Tools.GetStr(ViewState["filterid"]); }
        }

        //------------------------------------------------------------------------------
        public static bool CheckDoWyplaty(SqlDataSource ds, int typn, string pids, string dFrom, string dTo, string kierId, Button btDoWyplaty)
        {
            string msg;
            if (!String.IsNullOrEmpty(pids))
            {
                if (ds == null) ds = SQL.dsRozlNadgDoWyplaty;

                //DataSet ds = db.Select.Set(dsRozlNadgDoWyplaty.SelectCommand, DateFrom, DateTo, ids);
                //string err = db.Join(ds, "DoWyplatyNiezgodne", "\\n");   // są już w części rozpisane - będzie dup index
                //string inf = db.Join(ds, "Niedomiary", "\\n");           // są niezbilansowania
                DataTable dt = db.Select.Table(ds.SelectCommand, typn, pids, dFrom, dTo, kierId);
                if (dt.Rows.Count > 0)
                {
                    //DataTable td2 = dt.DefaultView.ToTable(true, "DoWyplatyNiezgodne", "Niedomiary");
                    DataRow[] drX = dt.Select("DoWyplatyNiezgodne is not null");
                    msg = String.Join("\\n", drX.Select(row => row[1].ToString()).ToArray());
                    if (String.IsNullOrEmpty(msg))
                    {
                        DataRow[] drN = dt.Select("Niedomiary is not null");
                        msg = String.Join("\\n, ", drN.Select(row => row[0].ToString()).ToArray());
                        if (!String.IsNullOrEmpty(msg))
                            msg = "Uwaga!\nIstnieją niezbilansowane niedomiary:\n" + msg + "\n\n";
                        Tools.ShowConfirm(msg + String.Format("Potwierdź przeznaczenie wszystkich nierozliczonych nadgodzin '{0} do wypłaty.", typn), btDoWyplaty);
                        return true;
                    }
                    else Tools.ShowMessage("Istnieją nadgodziny '{0} przeznaczone do wypłaty. Przed uruchomieniem automatycznej klasyfikacji należy je usunąć:\n{1}", typn, msg);
                }
                else Tools.ShowMessage("Brak nadgodzin '{0}.", typn);
            }
            else Tools.ShowMessage("Brak pracowników.");
            return false;
        }

        public static bool MakeDoWyplaty(SqlDataSource ds, int typn, string pids, string dFrom, string dTo, string kierId)
        {
            if (!String.IsNullOrEmpty(pids))
            {
                if (ds == null) ds = SQL.dsRozlNadgDoWyplaty;
                bool ok = db.Execute(ds.UpdateCommand, typn, pids, dFrom, dTo, kierId, App.User.OriginalId);
                if (ok)
                    Tools.ShowMessage("Nadgodziny '{0} zostały przeznaczone do wypłaty.", typn);
                else
                    Tools.ShowError("Wystąpił błąd podczas zapisu.");
                return ok;
            }
            else
            {
                Tools.ShowMessage("Brak pracowników.");
                return false;
            }
        }
        //-----------------
        private string KierId
        {
            get { return db.getScalar(String.Format("select KierId from VPrzypisaniaNaDzis where Id = {0}", PracId)); }
        }
        private void DoWyplatyAll2(int typn)
        {
            bool ok = cntPrzeznaczNadg2.CheckDoWyplaty(dsRozlNadgDoWyplaty, typn, PracId, OkresOd, OkresDo, KierId, typn == 50 ? btDoWyplaty50 : typn == 100 ? btDoWyplaty100 : null);
        }

        protected void btDoWyplaty50_Click(object sender, EventArgs e)
        {
            MakeDoWyplaty(dsRozlNadgDoWyplaty, 50, PracId, OkresOd, OkresDo, KierId);
        }

        protected void btDoWyplaty100_Click(object sender, EventArgs e)
        {
            MakeDoWyplaty(dsRozlNadgDoWyplaty, 100, PracId, OkresOd, OkresDo, KierId);
        }
        //------------------------------------------------------------------------------
        private static cntPrzeznaczNadg2 FSQL = null;

        public static cntPrzeznaczNadg2 SQL
        {
            get
            {
                //if (FSQL == null)   // wyłączam zapamiętuje dopóki aktywny klient więc jak sie cos zmieni to nie uaktualnia
                {
                    UserControl cnt = new UserControl();
                    FSQL = (cntPrzeznaczNadg2)cnt.LoadControl("~/Controls/RozliczenieNadg/cntPrzeznaczNadg2.ascx");
                }
                return FSQL;
            }
        }


    }
}