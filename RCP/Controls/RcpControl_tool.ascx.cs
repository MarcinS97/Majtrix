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
    public partial class RcpControl_tool : System.Web.UI.UserControl
    {
        string FStrefaId;
        Worktime wt = null;
        Ustawienia settings;

        /*
        "select D.Data, 
            convert(varchar,DATEDIFF(SECOND, A.Czas, B.CzasKoniec)/86400) + ' ' + convert(varchar, B.CzasKoniec - A.Czas, 8) as CzasPracy
            null as CzasPracy2,
            A.Czas as TimeIn, 
            B.CzasKoniec as TimeOut
        from GetDates('@hidDateFrom','@hidDateTo') D
        left outer join tmpRCP2 A on A.sesId = @sesId and A.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X1 where X1.sesId = @sesId and X1.Czas >= D.Data and X1.Czas < DATEADD(DAY, 1, D.Data) order by pprzed desc)
        left outer join tmpRCP2 B on B.sesId = @sesId and B.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X2 where X2.sesId = @sesId and X2.CzasKoniec > A.Czas order by X2.CzasKoniec asc,ppo desc)"
         */



        protected void Page_Load(object sender, EventArgs e)
        {
            settings = Ustawienia.CreateOrGetSession();
            if (!IsPostBack)
            {
                string strefa = (string)Session[ID + "_strefa"];
                string tround = (string)Session[ID + "_timeround"];
                if (String.IsNullOrEmpty(tround)) tround = "30";
                string alg = (string)Session[ID + "_timeround"];
                if (String.IsNullOrEmpty(alg)) alg = "2";

                DataSet ds = Base.getDataSet("select Id, Nazwa from Strefy");  // wszystkie, nie tylko aktywne
                Tools.BindData(ddlStrefa, ds, "Nazwa", "Id", false, strefa);

                App.FillTimeRound(ddlTimeRound, tround, settings.Zaokr.ToString());
            }
        }

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

        public void Prepare(string pracId, string rcpId, string dateFrom, string dateTo)
        {
            hidPracId.Value = pracId;
            hidRcpId.Value = rcpId;
            hidDateFrom.Value = dateFrom;
            hidDateTo.Value = dateTo;
            hidDetails.Value = null;
            ListView1.SelectedIndex = -1;
            Prepare();
        }

        public void Prepare()
        {
            //Ustawienia settings = Ustawienia.CreateOrGetSession();
            int zaokr = Int32.Parse(ddlTimeRound.SelectedValue);

            wt = new Worktime();
#if WT2
            wt.Prepare(null, hidPracId.Value, null, null, hidRcpId.Value, ddlStrefa.SelectedValue, hidDateFrom.Value, hidDateTo.Value,
                    0, 0, 0, 0, 0, 0);  // czas nocny - jak 0 to nie pobiera bo tu mi to niepotrzebne, dsDays=null przerwy i zaokr dowolne
            ListView1.DataSourceID = null;
            ListView1.DataSource = wt.GetRcpData(Int32.Parse(ddlTimeRound.SelectedValue), settings.ZaokrType);
#else
            wt.Prepare(null, hidPracId.Value, hidRcpId.Value, ddlStrefa.SelectedValue, hidDateFrom.Value, hidDateTo.Value, 
                    //settings.NocneOdSec, settings.NocneDoSec,
                    0, 0,  // jak 0 to nie pobiera bo tu mi to niepotrzebne
                    ID.Substring(ID.Length - 1));
            hidSesId.Value = wt.SesId;
#endif
            ListView1.DataBind();

            string sum, sumR;
            string sum2, sum2R;

            wt.SumTime(out sum, out sumR, out sum2, out sum2R, 
                       zaokr, 
                       settings.ZaokrType,
                       settings.ZaokrSum,
                       settings.ZaokrSumType);

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

        public void SetStrefaId(string sId)
        {
            Tools.SelectItem(ddlStrefa, sId);
        }

        //------------------
        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            if (ID.EndsWith("5"))
            {
                int x = 0;
            }
            /*
            if (!String.IsNullOrEmpty(hidRcpId.Value))
            {
                wt = new Worktime(null, hidRcpId.Value, ddlStrefa.SelectedValue, hidDateFrom.Value, hidDateTo.Value);
                hidSesId.Value = wt.SesId;
            }
             */
        }

        protected void ListView1_Init(object sender, EventArgs e)
        {
            int x = 0;
        }

        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {
            int x = 0;
        }

        protected void ListView1_Load(object sender, EventArgs e)
        {
            int x = 0;
        }

        //------------------
        protected void ddlStrefa_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[ID + "_strefa"] = ddlStrefa.SelectedValue;
            Prepare();
        }

        protected void ddlTimeRound_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[ID + "_timeround"] = ddlTimeRound.SelectedValue;
            Prepare();
        }
 
        //------------------
        public string StrefaId
        {
            get { return FStrefaId; }
            set { FStrefaId = value; }
        }

        //------------------
        private Label TimeLabel(DataRow dr, string tname)
        {
            Label lb = new Label();
            DateTime dt = Base.getDateTime(dr, tname, DateTime.MinValue);  // zawsze byc musi
            lb.Text = Base.TimeToStr(dt);
            lb.ToolTip = Base.DateToStr(dt);
            lb.ForeColor = Color.Gray;
            return lb;
        }

        private Label WorkTimeLabel(DataRow dr, string tname)
        {
            Label lb = new Label();
            lb.Text = Base.getValue(dr, tname);
            lb.ForeColor = Color.Gray;
            return lb;
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem ditem = (ListViewDataItem)e.Item;
                DataRowView rowView = (DataRowView)ditem.DataItem;
                string dataOd = rowView["TimeIn"].ToString();//"yyyy-MM-dd hh:mm:ss"
                string dataDo = rowView["TimeOut"].ToString();
#if WT2
                int? idx1 = (int?)rowView["idx1"];
                int? idx2 = (int?)rowView["idx2"];
                DateTime first, last;
                DataSet ds = wt.GetDetails(idx1, idx2, Int32.Parse(ddlTimeRound.SelectedValue), settings.ZaokrType, out first, out last);
#else
                DataSet ds = wt.GetDetails(dataOd, dataDo, Int32.Parse(ddlTimeRound.SelectedValue), settings.ZaokrType);
#endif
                if (ds != null && ds.Tables[0].Rows.Count > 1)  // powinno zawsze być przynajmniej 1!!!
                {
                    PlaceHolder phTimeIn = (PlaceHolder)e.Item.FindControl("phTimeIn");
                    PlaceHolder phTimeOut = (PlaceHolder)e.Item.FindControl("phTimeOut");
                    PlaceHolder phCzas1R = (PlaceHolder)e.Item.FindControl("phCzas1R");

                    string br = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Tools.AddControl(phTimeIn, br, TimeLabel(dr, "TimeIn"), null);
                        Tools.AddControl(phTimeOut, br, TimeLabel(dr, "TimeOut"), null);
                        Tools.AddControl(phCzas1R, br, WorkTimeLabel(dr, "Czas"), null);
                        if (br == null) br = "<br />"; // w pierwszej linii nie ma bo w div wyswietlam
                    }
                }
                //----- selected item - Readers events -----
                if (ListView1.SelectedIndex == ditem.DisplayIndex)
                {
                    //HiddenField hid = (HiddenField)e.Item.FindControl("hidDetailsData");
                    if (String.IsNullOrEmpty(hidDetails.Value))
                    {
                        object d = rowView["Data"];
                        DateTime? data = Base.isNull(d) ? null : (DateTime?)d;
                        ds = wt.GetDetails2(data);
                        //ds = wt.GetDetails2(dataOd, dataDo);
                    }
                    else
                    {
                        DateTime data = (DateTime)rowView["Data"];
                        dataOd = Base.DateToStr(data);
                        dataDo = Base.DateToStr(data.AddDays(2));
                        ds = wt.GetDetails3(dataOd, dataDo, first, last); //DateTime.MinValue, DateTime.MaxValue);
                    }
                    
                    PlaceHolder phEvents = (PlaceHolder)e.Item.FindControl("phEvents");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string td1 = null;
                        string td2 = null;
                        string td3 = null;
                        string td4 = null;
                        string br = null;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DateTime dt = Base.getDateTime(dr, "Czas", DateTime.MinValue);
                            int inout = Base.getInt(dr, "InOut2", -1);
                            string rid = Base.getValue(dr, "ECReaderId");
                            string name = Base.getValue(dr, "Name");

                            //td1 += br + Base.DateToStr(dt);
                            //td1 += br + rid;
                            if (inout == 0)
                            {
                                td2 += br + "<span title=\"" + Base.DateToStr(dt) + "\">" + Base.TimeToStr(dt) + "</span>";
                                td3 += br;
                            }
                            else if (inout == 1)
                            {
                                td2 += br;
                                td3 += br + "<span title=\"" + Base.DateToStr(dt) + "\">" + Base.TimeToStr(dt) + "</span>";
                            }
                            else // -1
                            {
                                int io = Base.getInt(dr, "InOut", -1);
                                td2 += br + "<span title=\"" + Base.DateToStr(dt) + "\">[" + Base.TimeToStr(dt) + "]</span>";
                                td3 += br + (io == 0 ? "IN" : io == 1 ? "OUT" : "PASS");
                            }
                            td4 += br + rid + " " + name;
                            if (br == null) br = "<br />"; 
                        }
                        Tools.AddLiteral(phEvents, "<td>" + td1 + "</td>");
                        //Tools.AddLiteral(phEvents, "<td align=\"right\">" + td1 + "</td>");
                        Tools.AddLiteral(phEvents, "<td align=\"right\">" + td2 + "</td>");
                        Tools.AddLiteral(phEvents, "<td align=\"right\">" + td3 + "</td>");
                        Tools.AddLiteral(phEvents, "<td colspan=\"2\">" + td4 + "</td>");
                    }
                    else
                        Tools.AddLiteral(phEvents, "<td colspan=\"5\">Brak danych</td>");
                }

                //Label lb = e.Item.FindControl("Czas2rLabel");
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

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidDetails.Value = null;
            Prepare();
        }

        //---------------------------------------
    }
}