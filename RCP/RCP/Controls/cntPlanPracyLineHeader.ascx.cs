using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.RCP.Controls
{
    public partial class cntPlanPracyLineHeader : System.Web.UI.UserControl
    {
        int FMode = cntPlanPracy.moZmiany;

        int[] _days = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        int[] days;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void DataBind(int okresDo)           // cntPlanPracy.moKartaRoczna
        {
            days = new int[31];
            FMode = cntPlanPracy.moKartaRoczna;
            for (int d = 0; d < 31; d++)
                days[d] = (okresDo + d) % 31 + 1;
            Repeater1.DataSourceID = null;
            Repeater1.DataSource = days;
            Repeater1.DataBind();
        }

        public int DataBind(int okresOd, int okresDo)           // cntPlanPracy.moKartaRoczna
        {
            int cnt;
            switch (FMode)
            {
                case cntPlanPracy.moPlanUrlopowRok:
                    cnt = okresDo - okresOd + 1;
                    days = new int[cnt];
                    int idx = okresOd;
                    for (int d = 0; d < cnt; d++)
                    {
                        days[d] = idx;
                        idx++;
                    }
                    break;
                default:
                    if (okresDo < okresOd)                  // 21 - 20
                        cnt = 31;
                    else
                    {
                        cnt = okresDo - okresOd + 1;        // 1 - 31, 21 - 31
                        if (okresOd > 1)
                            cnt += 31;
                    }
                    days = new int[cnt];
                    FMode = cntPlanPracy.moKartaRoczna;
                    idx = okresOd;
                    for (int d = 0; d < cnt; d++)
                    {
                        days[d] = idx;
                        idx++;
                        if (idx > 31) idx = 1;
                    }
                    break;
            }
            
            Repeater1.DataSourceID = null;
            Repeater1.DataSource = days;
            Repeater1.DataBind();
            return cnt;
        }
        /*
        public string FromDate
        {
            get { return hidFrom.Value; }
            set { hidFrom.Value = value; }
        }

        public string ToDate
        {
            get { return hidTo.Value; }
            set { hidTo.Value = value; }
        }
        */

        public void DataBind(string dFrom, string dTo, DateTime dtAccFrom, DateTime dtAccTo, DateTime dtBlockedTo)   // akceptacja
        {
            hidFrom.Value = dFrom;
            hidTo.Value = dTo;
            AccFromDate = dtAccFrom;
            _AccToDate = dtAccTo;
            BlockedToDate = dtBlockedTo;
            Repeater1.DataBind();
        }

        public void _DataBind(DateTime dtAccTo)   // po akceptacji przez kierownika
        {
            _AccToDate = dtAccTo;
            Repeater1.DataBind();
        }

        public void DataBind(string dFrom, string dTo)   // zmiany
        {
            DataBind(dFrom, dTo, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        }

        public void SelectAll(bool select)
        {
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("cbDay");
                if (cb != null) cb.Checked = select;  // nie ustawiam class tutaj ...
            }
        }

        public bool IsAnySelected()
        {
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("cbDay");
                if (cb != null && cb.Checked)
                    return true;
            }
            return false;
        }
        //--------------------------------
        /*
        private cntPlanPracy GetParentPP()
        {
            cntPlanPracy pp = (cntPlanPracy)Parent.NamingContainer.Parent;
            return pp;
        }
        */

        protected void Repeater1_DataBinding(object sender, EventArgs e)
        {
            hidHeaderData.Value = null;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                switch (FMode)
                {
                    default: 
                    case cntPlanPracy.moZmiany:
                    case cntPlanPracy.moAccept:
                    case cntPlanPracy.moPlanUrlopow:
                        Label lbDay = Tools.FindLabel(e.Item, "DayLabel");
                        Label lbDate = Tools.FindLabel(e.Item, "DataLabel");
                        HtmlTableCell th = (HtmlTableCell)e.Item.FindControl("thData");
                        if (lbDay != null && lbDate != null && th != null)  // moze to i nie optymalne ale bardziej przejrzyste
                        {
                            DataRowView drv = (DataRowView)e.Item.DataItem;
                            //----- wartości -----
                            int i = (Int32)drv["Day"];
                            object o = drv["Rodzaj"];
                            bool free = !o.Equals(DBNull.Value); // zakladam ze tam sa tylko dni wolne ...

                            string date = Base.DateToStr(drv["Data"]);
                            string opis = drv["Opis"].ToString();
                            //----- daty - czy wolne i czy dzisiaj -----
                            if (i >= 1 && i <= 7)  // uwaga na @@DATEFIRST !
                                lbDay.Text = Tools.DayName[i].Substring(0, 2);
                            else
                                lbDay.Text = null;
                            lbDate.Text = ((DateTime)drv["Data"]).Day.ToString();

                            string css = null;
                            string info = null;
                            if (i == 1 || i == 7 || free)   // sobota, niedziela i dzień wolny
                            {
                                css += " holiday";
                                info += "h";
                            }
                            if (date == Base.DateToStr(DateTime.Today))
                            {
                                css += " today";
                                info += "t";
                            }
                            th.Attributes["class"] += css;
                            th.Attributes["title"] = date + (String.IsNullOrEmpty(opis) ? null : "\n" + opis);
                            //----- dane HeaderData -----
                            hidHeaderData.Value += e.Item.ItemIndex == 0 ? info : "," + info;
                            //----- ico [v] accepted -----
                            if (FMode == cntPlanPracy.moAccept)
                            {
                                DateTime dt = (DateTime)drv["Data"];
                                if (AccFromDate <= dt && dt <= _AccToDate)
                                {
                                    System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)Tools.SetControlVisible(e.Item, "imgAccepted", true);
                                    if (img != null)
                                        if (dt <= BlockedToDate)
                                            img.ImageUrl = "../../images/ok_small_silver.png";   // ../images...
                                        else
                                            img.ImageUrl = "../../images/ok_small.png";    
                                }
                            }
                            //-----
                            if (FMode == cntPlanPracy.moPlanUrlopow)
                            {
                                th.RowSpan = 2;
                            }
                        }
                        break;
                    case cntPlanPracy.moPlanUrlopowRok:
                        lbDay = Tools.FindLabel(e.Item, "DayLabel");
                        lbDate = Tools.FindLabel(e.Item, "DataLabel");
                        th = (HtmlTableCell)e.Item.FindControl("thData");
                        if (lbDay != null && lbDate != null && th != null)  // moze to i nie optymalne ale bardziej przejrzyste
                        {
                            int idx = days[e.Item.ItemIndex];
                            int wday = idx % 7 + 1;    // 1-niedz, 2-pon ... 7-sob, a tu startuje od 1-pon
                            string dname = Tools.DayName[wday];
                            lbDay.Text = dname.Substring(0, 2);

                            string css = null;
                            string info = null;
                            if (wday == 1 || wday == 7)   // sobota, niedziela 
                            {
                                css += " holiday";
                                info += "h";
                            }
                            th.Attributes["class"] += css;
                            th.Attributes["title"] = dname;
                            //----- dane HeaderData -----
                            hidHeaderData.Value += e.Item.ItemIndex == 0 ? info : "," + info;
                        }
                        break;
                    case cntPlanPracy.moKartaRoczna:
                        lbDate = Tools.FindLabel(e.Item, "DataLabel");
                        if (lbDate != null)
                            lbDate.Text = days[e.Item.ItemIndex].ToString();
                        break;
                    case cntPlanPracy.moRozliczenie:
                        break;
                }
                //----- check box -----
                CheckBox cb = Tools.FindCheckBox(e.Item, "cbDay");
                if (cb != null) 
                    switch (FMode)
                    {
                        default:
                        case cntPlanPracy.moZmiany:
                        case cntPlanPracy.moPlanUrlopow:
                            Control c = Parent.NamingContainer.Parent;
                            bool edit = false;
                            if (c is cntPlanPracy) edit = ((cntPlanPracy)c).EditMode;
                            else if (c is HRRcp.Controls.cntPlanRoczny) edit = ((HRRcp.Controls.cntPlanRoczny)c).EditMode;
                            if (edit)
                            {
                                cb.Attributes["idx"] = e.Item.ItemIndex.ToString();
                                cb.Attributes["onclick"] = String.Format("javascript:cbDayClickPP(this,{0});", e.Item.ItemIndex);
                            }
                            else cb.Visible = false;
                            break;
                        case cntPlanPracy.moKartaRoczna:
                        case cntPlanPracy.moAccept:
                            cb.Visible = false;
                            break;
                        case cntPlanPracy.moRozliczenie:
                            cb.Visible = false;
                            break;
                    }
            }
        }

        /*
                    lb.ForeColor = Color.Red;
                    lb = Tools.FindLabel(e.Item, "DataLabel");
                    if (lb != null) lb.ForeColor = Color.Red;
                    //HtmlTableCell th = (HtmlTableCell)e.Item.FindControl("thData");
                    if (th != null)
                    {
                        th.Style.Add("background-image", "none");
                        th.BgColor = ColorTranslator.ToHtml(Tools.warnColor);
                        //th.Attributes("");
         */
        //----- SUMY --------------------------
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (SumyVisible)
                {
                    HtmlTableCell th = (HtmlTableCell)e.Item.FindControl("thSuma");
                    if (th != null)
                    {
                        if (e.Item.ItemIndex == 0)
                            Tools.AddClass(th, "suma1");
                        if (FMode == cntPlanPracy.moKartaRoczna)
                            th.RowSpan = 2;
                    }                    
                    Label lb1 = (Label)e.Item.FindControl("lb1");
                    Label lb2 = (Label)e.Item.FindControl("lb2");
                    if (lb1 != null && lb2 != null)
                    {
                        string dataItem = (string)e.Item.DataItem;
                        string p1, p2;//, p3, p4, p5, p6;
                        Tools.GetLineParams(dataItem, out p1, out p2);//, out p3, out p4, out p5, out p6);
#if RCP
                        if (p1 != null && p1.Contains("<") || p2 != null && p2.Contains("<"))
                        {
                            int ggggggg = 0;
                        }

                        if (p1 != null) p1 = p1.Replace("\n", "<br />");
                        if (p2 != null) p2 = p2.Replace("\n", "<br />");
#endif
                        lb1.Text = p1;
                        lb2.Text = p2;
                    }
                    //-----
                    if (FMode == cntPlanPracy.moPlanUrlopow)
                    {
                        th.RowSpan = 2;
                    }
                }
            }
        }

        protected void Repeater2_PreRender(object sender, EventArgs e)
        {
            if (SumyVisible)
            {
                string[] sumy = SumHeader.Split(',');
                Repeater2.DataSource = sumy;
                Repeater2.DataBind();       // w Render binduje
            }
        }

        //-------------------
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        public string[] HeaderData          // zeby PPline wiedział co to za dzień
        {
            get { return hidHeaderData.Value.Split(','); }
        }

        public string[] SumData          // zeby PPline wiedział co to za dzień
        {
            get { return SumHeader.Split(','); }
        }

        /*
        public string HeaderDataValues
        {
            get { return Tools.GetViewStateStr(ViewState["hdata"]); }
            set { ViewState["hdata"] = value; }
        }
        
        /**/
#if RCP
        public string SumHeader
        {
            set { hidSumHeader.Value = cntPlanPracyLine2.EncodeHtml(value); }
            get { return cntPlanPracyLine2.DecodeHtml(hidSumHeader.Value); }
        }
#else
        public string SumHeader
        {
            set { hidSumHeader.Value = value; }
            get { return hidSumHeader.Value; }
        }
#endif
        public bool SumyVisible
        {
            set
            {
                if (!value)
                {
                    SumHeader = null;
                    //Repeater2.DataBind();
                }
            }
            get { return !String.IsNullOrEmpty(SumHeader); }
        }

        public Repeater Repeater
        {
            get { return Repeater1; }
        }
        //--------------------
        public DateTime AccFromDate
        {
            get { return (DateTime)ViewState[ID + "_accFrom"]; }
            set { ViewState[ID + "_accFrom"] = value; }
        }

        public DateTime _AccToDate
        {
            get { return (DateTime)ViewState[ID + "_accTo"]; }
            set { ViewState[ID + "_accTo"] = value; }
        }

        public DateTime BlockedToDate
        {
            get { return Tools.GetViewStateDateTime(ViewState[ID + "_blockTo"], AccFromDate.AddDays(-1)); }
            set { ViewState[ID + "_blockTo"] = value; }
        }

        //public bool CheckVisible
        //{
        //    get { return thCheck.Visible; }
        //    set { thCheck.Visible = value; }
        //}

        //public bool Checked
        //{
        //    get { return cbSelectAll.Checked; }
        //    set { cbSelectAll.Checked = value; }
        //}
    }
}