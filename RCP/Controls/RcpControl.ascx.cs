using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class RcpControl : System.Web.UI.UserControl
    {
        Worktime wt = null;
        //Worktime2 wt = null;
        Ustawienia settings;
        //int cnt1 = 0;
        //int cnt2 = 0;
#if SIEMENS
        const bool showSumaWStrefie = false;
#else
        const bool showSumaWStrefie = true;
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            settings = Ustawienia.CreateOrGetSession();
            if (!IsPostBack)
            {
                App.FillTimeRound(ddlTimeRound, Round, settings.Zaokr.ToString());
                App.FillRoundType(ddlRoundType, RoundType, settings.ZaokrType.ToString());

                DataSet ds = Base.getDataSet("select Id, Nazwa from Strefy order by Nazwa");  // wszystkie, nie tylko aktywne
                Tools.BindData(ddlStrefa, ds, "Nazwa", "Id", false, _StrefaId);
            }
        }
        /*
        private void ShowDebug()
        {
            lbDebug.Text = String.Format("{0} {1}", cnt1, cnt2);
        }
        */
        protected void Page_Unload(object sender, EventArgs e)
        {
            //if (wt != null) wt.Unload();
        }
        //------------------

        public void Prepare(string dateFrom, string dateTo)
        {
            hidDateFrom.Value = dateFrom;
            hidDateTo.Value = dateTo;
            ListView1.SelectedIndex = -1;
            Prepare();
        }

        public void Prepare(string pracId, /*string algRCP, string algPar, */ string _rcpId, string dateFrom, string dateTo, string _strefaId)
        {
            hidPracId.Value = pracId;
            _hidRcpId.Value = _rcpId;
            hidDateFrom.Value = dateFrom;
            hidDateTo.Value = dateTo;
            hidDetails.Value = null;
            _hidStrefaId.Value = _strefaId;
            //hidAlgRcp.Value = algRCP;
            //hidAlgPar.Value = algPar;

            Tools.SelectItem(ddlStrefa, _strefaId, _strefaId, true);
            //----- ponowne ustawienie na wartosci poczatkowe ------            
            SetRound(settings.Zaokr.ToString());    // bez odświeżania !
            Tools.SelectItem(ddlTimeRound, Round);
            SetRoundType(settings.ZaokrType.ToString());
            Tools.SelectItem(ddlRoundType, RoundType);

            ListView1.SelectedIndex = -1;
            Prepare();
        }

        public void Prepare()
        {
            //Ustawienia settings = Ustawienia.CreateOrGetSession();
            //cnt1++;
            //ShowDebug();
            int zaokr = Int32.Parse(Round);
            int zaokrType = Int32.Parse(RoundType);

            wt = new Worktime();
#if WT2
            wt.Prepare(null, hidPracId.Value, null, null, _hidRcpId.Value, _StrefaId, hidDateFrom.Value, hidDateTo.Value,
                       0, 0, 0, 0, 0, 0);  // czas nocny - jak 0 to nie pobiera bo tu mi to niepotrzebne, dsDays=null to przerwy i zaokr nie są brane pod uwagę
            ListView1.DataSourceID = null;
            ListView1.DataSource = wt.GetRcpData(Int32.Parse(Round), Int32.Parse(RoundType));
#else
            wt.Prepare(null, hidPracId.Value, hidRcpId.Value, StrefaId, hidDateFrom.Value, hidDateTo.Value, 
                        0, 0,  // czas nocny - jak 0 to nie pobiera bo tu mi to niepotrzebne
                        ID.Substring(ID.Length - 1));
            hidSesId.Value = wt.SesId;
#endif

            ListView1.DataBind();   // UWAGA !!! tu się wywalało ładowanie ViewState - pomogło ustawienie lbNoDataInfo.EnableViewState na false !!!

            if (String.IsNullOrEmpty(_hidRcpId.Value) && !String.IsNullOrEmpty(hidPracId.Value))  // brak rcp i jest pracId
                Tools.SetControlVisible(ListView1.Controls[0], "lbNoDataInfo", true);

            string sum, sumR;
            string sum2, sum2R;
            wt.SumTime(out sum, out sumR, out sum2, out sum2R, 
                       zaokr, 
                       zaokrType,
                       settings.ZaokrSum,
                       settings.ZaokrSumType);  // wynik zawsze do pełnej godziny bo zmiany sa godzinowo


            //!!!!!!!!!!!!!!!!
            // zastanowic sie z informacja nt zaokraglania sum bo userowi moze sie nie zgadzac a nie bedzie wiedzial z czego to wynika !!!
            // ... albo dac kolejne ddl z wyborem ...




            Label lb = (Label)ListView1.FindControl("lbSuma");
            if (lb != null)
            {
                lb.Text = sumR;
                lb.ToolTip = sum;
            }
            lb = (Label)ListView1.FindControl("lbSuma2");
            if (lb != null)
            {
                lb.Text = sum2R;
                lb.ToolTip = sum2;
            }
        }

        public void x_SetStrefaId(string sId)
        {
            Tools.SelectItem(ddlStrefa, sId);
        }


        //------------------
        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            //int x = 0;
        }

        protected void ListView1_Init(object sender, EventArgs e)
        {
            //int x = 0;
        }

        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {
            //int x = 0;
        }

        protected void ListView1_Load(object sender, EventArgs e)
        {
            //int x = 0;
        }

        //------------------
        protected void ddlStrefa_SelectedIndexChanged(object sender, EventArgs e)
        {
            _StrefaId = ddlStrefa.SelectedValue;
        }

        protected void ddlTimeRound_SelectedIndexChanged(object sender, EventArgs e)
        {
            Round = ddlTimeRound.SelectedValue;
        }

        protected void ddlRoundType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RoundType = ddlRoundType.SelectedValue;
        }
        //------------------
        private Label TimeLabel(DataRow dr, string tname)
        {
            Label lb = new Label();
            DateTime? dt = Base.getDateTime(dr, tname);  // zawsze byc musi
            if (!Base.isNull(dt))
            {
                lb.Text = Base.TimeToStr((DateTime)dt);
                lb.ToolTip = Base.DateToStr(dt);
                lb.ForeColor = Color.Gray;
            }
            return lb;
        }
        /*
        private Label TimeLabel(DataRow dr, string tname)
        {
            Label lb = new Label();
            DateTime dt = Base.getDateTime(dr, tname, DateTime.MinValue);  // zawsze byc musi
            lb.Text = Base.TimeToStr(dt);
            lb.ToolTip = Base.DateToStr(dt);
            lb.ForeColor = Color.Gray;
            return lb;
        }
        */
        private Label WorkTimeLabel(DataRow dr, string tname)
        {
            Label lb = new Label();
            lb.Text = Base.getValue(dr, tname);
            lb.ForeColor = Color.Gray;
            return lb;
        }

        private void ShowAnalizeRcp(PlaceHolder ph, DataSet ds)
        {
            Tools.AddLiteral(ph, "<table><tr>");
            Tools.AddLiteral(ph,    "<th>Nazwa</th>");
            Tools.AddLiteral(ph,    "<th>Czas od</th>");
            Tools.AddLiteral(ph,    "<th>Czas do</th>");
            Tools.AddLiteral(ph,    "<th>Czas łączny</th>");
            Tools.AddLiteral(ph,    "<th>Czas nocny</th>");
            Tools.AddLiteral(ph, "</tr>");            
            foreach (DataRow dr in Base.getRows(ds))
            {
                string nazwa = Base.getValue(dr, "Nazwa");
                DateTime? dt1 = Base.getDateTime(dr, "TimeOd");
                DateTime? dt2 = Base.getDateTime(dr, "TimeDo");
                string czas = Base.getValue(dr, "Czas");
                string nocne = Base.getValue(dr, "Nocne");
                Tools.AddLiteral(ph, "<tr>");
                Tools.AddLiteral(ph, String.Format("<td>{0}</td>", nazwa));
                Tools.AddLiteral(ph, String.Format("<td>{0}</td>", Base.TimeToStr((DateTime)dt1)));
                Tools.AddLiteral(ph, String.Format("<td>{0}</td>", Base.TimeToStr((DateTime)dt2)));
                Tools.AddLiteral(ph, String.Format("<td>{0}</td>", czas));
                Tools.AddLiteral(ph, String.Format("<td>{0}</td>", nocne));
                Tools.AddLiteral(ph, "</tr>");
            }
            Tools.AddLiteral(ph, "</table>");
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem ditem = (ListViewDataItem)e.Item;
                DataRowView rowView = (DataRowView)ditem.DataItem;
                //string dataOd = rowView["TimeIn"].ToString();//"yyyy-MM-dd hh:mm:ss"
                //string dataDo = rowView["TimeOut"].ToString();
                object odtOd = rowView["TimeIn"];
                object odtDo = rowView["TimeOut"];
                string dataOd = Base.DateTimeToStr(odtOd);//"yyyy-MM-dd hh:mm:ss"
                string dataDo = Base.DateTimeToStr(odtDo);
#if WT2
                object o1 = rowView["idx1"];
                int? idx1 = Base.isNull(o1) ? null : (int?)o1;
                object o2 = rowView["idx2"];
                int? idx2 = Base.isNull(o2) ? null : (int?)o2;
                DateTime firstTime, lastTime;
                DataSet ds = wt.GetDetails(idx1, idx2, Int32.Parse(ddlTimeRound.SelectedValue), settings.ZaokrType, out firstTime, out lastTime);
                /* tu nie ma sensu bo dane bez zmian są
                //----- analiza -----
                o1 = rowView["idx1rcp"];
                idx1 = Base.isNull(o1) ? null : (int?)o1;
                o2 = rowView["idx2rcp"];
                idx2 = Base.isNull(o2) ? null : (int?)o2;
                DataSet dsAnalize = wt.GetRcpAnalize(idx1, idx2);
                */ 
#else
                DataSet ds = wt.GetDetails(dataOd, dataDo, Int32.Parse(Round), settings.ZaokrType);
#endif
                if (ds != null && ds.Tables[0].Rows.Count > 1)  // powinno zawsze być przynajmniej 1!!!
                {
                    PlaceHolder phTimeIn = (PlaceHolder)e.Item.FindControl("phTimeIn");
                    PlaceHolder phTimeOut = (PlaceHolder)e.Item.FindControl("phTimeOut");
                    PlaceHolder phCzas1R = (PlaceHolder)e.Item.FindControl("phCzas1R");

                    if (showSumaWStrefie)   // dla SIEMENS false czyli nie pokazuje
                    {
                        string br = null;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Tools.AddControl(phTimeIn, br, TimeLabel(dr, "TimeIn"), null);
                            Tools.AddControl(phTimeOut, br, TimeLabel(dr, "TimeOut"), null);
                            Tools.AddControl(phCzas1R, br, WorkTimeLabel(dr, "Czas"), null);
                            if (br == null) br = "<br />"; // w pierwszej linii nie ma bo w div wyswietlam
                        }
                    }
                }
                //----- selected item - Readers events -----
                PlaceHolder phEvents;
                if (ListView1.SelectedIndex == ditem.DisplayIndex)
                {
                    //HiddenField hid = (HiddenField)e.Item.FindControl("hidDetailsData");
                    DateTime data = (DateTime)rowView["Data"];
                    if (firstTime == DateTime.MinValue) firstTime = data;               // przy braku danych zeby wyswietlał ok - na szaro dane spoza dnia
                    if (lastTime == DateTime.MaxValue) lastTime = data.AddDays(1);
                    bool details2 = String.IsNullOrEmpty(hidDetails.Value);
                    if (details2)
                    {
                        //object d = rowView["Data"];
                        //DateTime? data = Base.isNull(d) ? null : (DateTime?)d;
                        ds = wt.GetDetails2(data);
                        //ds = wt.GetDetails2(dataOd, dataDo);
                    }
                    else
                    {
                        //string dOd = Base.DateToStr(data);
                        //string dDo = Base.DateToStr(data.AddDays(2));
                        DateTime d1 = Base.isNull(odtOd) ? data : ((DateTime)odtOd).AddHours(-12);
                        DateTime d2 = Base.isNull(odtDo) ? data.AddDays(2) : ((DateTime)odtDo).AddHours(24);
                        string dOd = Base.DateTimeToStr(d1);
                        string dDo = Base.DateTimeToStr(d2);
                        ds = wt.GetDetails3(dOd, dDo, firstTime, lastTime);
                    }
                    
                    phEvents = (PlaceHolder)e.Item.FindControl("phEvents");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string td1 = null;
                        string td2 = null;
                        string td3 = null;
                        string td4 = null;
                        string br = null;
                        DateTime dt1 = DateTime.MinValue;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DateTime dt = Base.getDateTime(dr, "Czas", DateTime.MinValue);
                            int inout = Base.getInt(dr, "InOut2", -1);
                            string rid = Base.getValue(dr, "ECReaderId");
                            string name = Base.getValue(dr, "Name");
                            bool day = Base.getBool(dr, "Day", false);
                            const string otherday = "class=\"otherday\" ";
                            string css = !day ? otherday : null;
                            //td1 += br + Base.DateToStr(dt);
                            //td1 += br + rid;

                            if (!details2)  // details3 - tylko tu daję info o dacie
                                if (dt.Date != dt1)
                                {
                                    dt1 = dt.Date;
                                    td1 += br + "<span " + otherday + ">" + Base.DateToStr(dt1) + "</span>";
                                }
                                else td1 += br;

                            if (inout == 0)
                            {
                                td2 += br + "<span " + css + "title=\"" + Base.DateToStr(dt) + "\">" + Base.TimeToStr(dt) + "</span>";
                                td3 += br;
                            }
                            else if (inout == 1)
                            {
                                td2 += br;
                                td3 += br + "<span " + css + "title=\"" + Base.DateToStr(dt) + "\">" + Base.TimeToStr(dt) + "</span>";
                            }
                            else // -1
                            {
                                int io = Base.getInt(dr, "InOut", -1);
                                td2 += br + "<span " + css + "title=\"" + Base.DateToStr(dt) + "\">[" + Base.TimeToStr(dt) + "]</span>";
                                td3 += br + "<span " + css + ">" + (io == 0 ? "IN" : io == 1 ? "OUT" : "PASS") + "</span>";
                            }
                            //td4 += br + "<span " + css + ">" + rid + " " + name + "</span>";
                            td4 += br + String.Format("<span {0} title=\"{2}\">{1} {2}</span>", css, rid, name);

                            if (br == null) br = "<br />";
                        }
                        //Tools.AddLiteral(phEvents, "<td>" + td1 + "</td>");
                        Tools.AddLiteral(phEvents, "<td>" + td1 + "</td>");
                        Tools.AddLiteral(phEvents, "<td>" + td2 + "</td>");
                        Tools.AddLiteral(phEvents, "<td>" + td3 + "</td>");

                        Tools.AddLiteral(phEvents, String.Format("<td class=\"rogername\" colspan=\"{0}\">{1}</td>", showSumaWStrefie ? 2 : 1, td4));

                    }
                    else
                    {
                        int cspan = showSumaWStrefie ? 5 : 4;
                        Tools.AddLiteral(phEvents, String.Format("<td colspan=\"{0}\">Brak danych</td>", cspan));
                    }
                }
                /*
                //----- rcp analiza -----
                phEvents = (PlaceHolder)e.Item.FindControl("phEvents");
                ShowAnalizeRcp(phEvents, dsAnalize);
                */
                //Label lb = e.Item.FindControl("Czas2rLabel");



                if (!showSumaWStrefie)
                {
                    Tools.SetControlVisible(e.Item, "tdSumaWStrefie", false);
                }
            }
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    //Prepare();
                    break;
                case "Unselect":
                    if (String.IsNullOrEmpty(hidDetails.Value))
                    {
                        hidDetails.Value = "1";
                    }
                    else
                    {
                        hidDetails.Value = null;
                        ListView1.SelectedIndex = -1;
                    }
                    Prepare();  // selindexchanged sie nie wywołuje
                    break;
            }
        }

        protected void ListView1_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
#if WT2
            ListView1.SelectedIndex = e.NewSelectedIndex;   // to trzeba dodać zeby nie było błędu ze SelIdxchanged nie obsłużona jeżeli DataSource podstawiam
#endif
        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidDetails.Value = null;
            Prepare();
        }

        //------------------
        public void SetRound(string round)
        {
            hidRound.Value = round;
            Session[ID + "_round"] = round;
        }

        public void SetRoundType(string roundType)
        {
            hidRoundType.Value = roundType;
            Session[ID + "_roundtype"] = roundType;
        }

        public void SetStrefaId(string strefaId)
        {
            _hidStrefaId.Value = strefaId;
            Session[ID + "_strefa"] = strefaId;
        }
 
        //------------------
        public string _StrefaId
        {
            get 
            {
                string strefa = _hidStrefaId.Value; 
                if (String.IsNullOrEmpty(strefa))
                {
                    strefa = (string)Session[ID + "_strefa"];
                    _hidStrefaId.Value = strefa;  // moze byc null
                }
                return strefa;
            }
            set 
            {
                SetStrefaId(value);
                Prepare();
            }
        }

        public string Round
        {
            get 
            {
                string tround = hidRound.Value;
                if (String.IsNullOrEmpty(tround))
                {
                    tround = (string)Session[ID + "_round"];
                    if (String.IsNullOrEmpty(tround))
                    {
                        Ustawienia settings = Ustawienia.CreateOrGetSession();
                        tround = settings.Zaokr.ToString();
                    }
                    hidRound.Value = tround;
                }
                return tround;
            }
            set 
            {
                SetRound(value);
                Prepare();
            }
        }

        public string RoundType
        {
            get
            {
                string type = hidRoundType.Value;
                if (String.IsNullOrEmpty(type))
                {
                    type = (string)Session[ID + "_roundtype"];
                    if (String.IsNullOrEmpty(type))
                    {
                        Ustawienia settings = Ustawienia.CreateOrGetSession();
                        type = settings.ZaokrType.ToString();
                    }
                    hidRoundType.Value = type;
                }
                return type;
            }
            set
            {
                SetRoundType(value);
                Prepare();
            }
        }
        //--------------       
        public bool StrefaSelect
        {
            set
            {
                tdStrefa.Visible = value;
                tbCzasPracyParams.Visible = value || RoundSelect || RoundTypeSelect;
            }
            get { return tdStrefa.Visible; }
        }

        public bool RoundSelect
        {
            set
            {
                tdZaokr.Visible = value;
                tbCzasPracyParams.Visible = value || StrefaSelect || RoundTypeSelect;
            }
            get { return tdZaokr.Visible; }
        }

        public bool RoundTypeSelect
        {
            set
            {
                tdZaokrTyp.Visible = value;
                tbCzasPracyParams.Visible = value || StrefaSelect || RoundSelect;
            }
            get { return tdZaokrTyp.Visible; }
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            if (!showSumaWStrefie)
            {
                Tools.SetControlVisible(ListView1, "thSumaWStrefie", false);
                Tools.SetControlVisible(ListView1, "thSumaWStrefieSuma", false);
            }
        }

    }
}