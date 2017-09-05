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
    public partial class RcpControl2 : System.Web.UI.UserControl
    {
        Worktime wt = null;
        Ustawienia settings;

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
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
        }
        //------------------

        public void Prepare(string dateFrom, string dateTo)
        {
            DataOd = dateFrom;
            DataDo = dateTo;
            Prepare();
        }

        public void Prepare(string kierId, string dateFrom, string dateTo)
        {
            DataOd = dateFrom;
            DataDo = dateTo;
            KierId = kierId;
            Prepare();
        }

        public void Prepare()
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            int zaokr = settings.Zaokr;
            int zaokrType = settings.ZaokrType;
            DataTable dtRcpData = null;
            wt = new Worktime();

#if WT2
            string kid = KierId;
            if (kid == "-100") kid = "0";  // wszyscy pracownicy w RepCzasPracy2
            DataSet ds = db.getDataSet(String.Format(@"
declare 
	@od datetime,
	@do datetime,
	@kierId int
set @od = '{0}'
set @do = '{1}'
set @kierId = {2}

--select * from Przypisania where Od <= @do and ISNULL(Do, '20990909') >= @od and Status = 1 and IdKierownika = @kierId
select distinct IdPracownika, Nazwisko + ' ' + Imie as Pracownik, KadryId from dbo.fn_GetTreeOkres(@kierId, @od, @do, @do) order by Pracownik, KadryId
                ", DataOd, DataDo, db.nullParam(kid)));
                //", DataOd, DataDo, db.nullParam(KierId)));

/*--- cc042 ---------------
            ds = db.getDataSet(String.Format(@"
declare 
	@od datetime,
	@do datetime,
	@kierId int
set @od = '{0}'
set @do = '{1}'
set @kierId = {2}

--select * from Przypisania where Od <= @do and ISNULL(Do, '20990909') >= @od and Status = 1 and IdKierownika = @kierId
select distinct IdPracownika, Nazwisko + ' ' + Imie as Pracownik, KadryId from dbo.fn_GetTreeOkres(@kierId, @od, @do, @do) 

where KadryId in (select distinct KadryId from VSplity2 where cc = '042' and DataOd = @od)

order by Pracownik, KadryId
                ", DataOd, DataDo, db.nullParam(kid)));
/*------------------*/

            foreach (DataRow dr in db.getRows(ds))
            {
                string pracId = db.getValue(dr, 0);
                string prac = db.getValue(dr, 1);
                string nrew = db.getValue(dr, 2);

                wt.Prepare(null, pracId, null, null, null, null, DataOd, DataDo, 0, 0, 0, 0, 0, 0);  // czas nocny - jak 0 to nie pobiera bo tu mi to niepotrzebne, dsDays=null to przerwy i zaokr nie są brane pod uwagę
                wt.AppendRcpData(pracId, prac, nrew, zaokr, zaokrType, ref dtRcpData);
            }


            ListView1.DataSource = dtRcpData;
#else
#endif

            ListView1.DataBind();   // UWAGA !!! tu się wywalało ładowanie ViewState - pomogło ustawienie lbNoDataInfo.EnableViewState na false !!!

            /*
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
            */
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




                /*
#if WT2
                object o1 = rowView["idx1"];
                int? idx1 = Base.isNull(o1) ? null : (int?)o1;
                object o2 = rowView["idx2"];
                int? idx2 = Base.isNull(o2) ? null : (int?)o2;
                DateTime firstTime, lastTime;
                DataSet ds = wt.GetDetails(idx1, idx2, settings.Zaokr, settings.ZaokrType, out firstTime, out lastTime);
                /* tu nie ma sensu bo dane bez zmian są
                //----- analiza -----
                o1 = rowView["idx1rcp"];
                idx1 = Base.isNull(o1) ? null : (int?)o1;
                o2 = rowView["idx2rcp"];
                idx2 = Base.isNull(o2) ? null : (int?)o2;
                DataSet dsAnalize = wt.GetRcpAnalize(idx1, idx2);
                * / 
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
                */


                if (!showSumaWStrefie)
                {
                    Tools.SetControlVisible(e.Item, "tdSumaWStrefie", false);
                }
            }
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
        }

        protected void ListView1_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //------------------
        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            if (!showSumaWStrefie)
            {
                Tools.SetControlVisible(ListView1, "thSumaWStrefie", false);
                Tools.SetControlVisible(ListView1, "thSumaWStrefieSuma", false);
            }
        }

        //-----------------------
        public string DataOd
        {
            set { ViewState["dod"] = value; }
            get { return Tools.GetStr(ViewState["dod"]); }
        }

        public string DataDo
        {
            set { ViewState["ddo"] = value; }
            get { return Tools.GetStr(ViewState["ddo"]); }
        }

        public string KierId
        {
            set { ViewState["kierId"] = value; }
            get { return Tools.GetStr(ViewState["kierId"]); }
        }
    }
}