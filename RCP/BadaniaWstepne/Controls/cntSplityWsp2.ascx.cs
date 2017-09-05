using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using HRRcp.App_Code;

namespace HRRcp.BadaniaWstepne.Controls
{
    public partial class cntSplityWsp2 : System.Web.UI.UserControl
    {
        public enum FModes : int
        {
            ReadOnly = default(int),
            Editable = 1
        };
        public FModes Mode { get; set; }
        public int NoGroup { get; set; }
        public string TableName { get; set; }
        public string IdPrzypColName { get; set; }
        public int Count
        {
            get
            {
                return lvSplityWsp.Items.Count;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource2.SelectParameters["noGroup"].DefaultValue = NoGroup.ToString(); 
            
            if (lvSplityWsp.Visible)
            {
                if (!Initialized)
                {
                    Initialized = true;
                    lvSplityWsp.DataSource = dsSplityWsp;
                    lvSplityWsp.DataBind();
                    ExecSumuj();
                }
                if (!IsPostBack)
                {
                }
                else
                {
                    UpdateDataSet();
                }
            }
        }

        public void ExecSumuj()
        {
            Tools.ExecOnStart2("wspsum", String.Format("cntSplityWspSum('{0}','{1}');", "sumwsp", "sum"));
        }

        public void Prepare(string przId)
        {
            IdPrzypisania = przId;
            ExecSumuj();
        }

        public void Prepare(string przId, bool editable)
        {
            IdPrzypisania = przId;
            Mode = editable ? FModes.Editable : FModes.ReadOnly;
            ExecSumuj();
        }
        //------------------------------------------
        protected bool IsReadOnly
        {
            get { return Mode == FModes.ReadOnly; }
        }

        protected string IsLastColClass
        {
            get { return Mode == FModes.ReadOnly ? " lastcol" : null; }
        }

        protected InsertItemPosition InsertPosition
        {
            get { return Mode == FModes.ReadOnly ? InsertItemPosition.None : InsertItemPosition.LastItem; }
        }

        //------------------------------------------
        public void Assign(cntSplityWsp2 splity)
        {
            if (splity == null)
            {
                dsSplityWsp = null;
            }
            else
            {
                dsSplityWsp = splity.dsSplityWsp;
                lvSplityWsp.DataSource = dsSplityWsp;
            }
            lvSplityWsp.DataBind();
        }

        private void UpdateDataSet()
        {
            DataSet ds = dsSplityWsp;
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                foreach (ListViewItem item in lvSplityWsp.Items)
                {
                    string c1 = Tools.GetText(item, "hidIdCC");
                    string c3 = Tools.GetText(item, "WspTextBox");
                    if (!String.IsNullOrEmpty(c1))
                    {
                        DataRow[] dr = dt.Select("IdCC=" + c1);
                        if (dr.Length != 0)
                            dr[0]["Wsp"] = c3;
                    }
                }
            }
        }

        public bool _Validate()
        {
            //----- comma -----
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
            //-----------------

            double sum = 0;
            foreach (ListViewItem item in lvSplityWsp.Items)
            {
                string c1 = Tools.GetText(item, "hidIdCC");
                string c3 = Tools.GetText(item, "WspTextBox");
                double wsp;
                if (Double.TryParse(c3, out wsp))
                    sum += wsp;
                else
                    return false;
            }
            return Worktime.Round05(sum, 6) == 1;
        }

        public bool Insert(SqlTransaction tr, int przypId)
        {
            //----- comma -----
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
            //-----------------

            foreach (ListViewItem item in lvSplityWsp.Items)
            {
                string c1 = Tools.GetText(item, "hidIdCC");
                string c3 = Tools.GetText(item, "WspTextBox");
                double wsp;
                if (Double.TryParse(c3, out wsp))
                {
                    db.execSQL(tr, db.insertCmd(TableName, 0,
                        IdPrzypColName + ",IdCC,Wsp",
                        przypId, c1, wsp));
                }
                else
                    return false;
            }
            return true;
        }

        //---------------------------
        public static string GetSplitSqlLog(string TableName, string IdPrzypColName, string przypId)
        {
            return String.Format(@"
                select '''' + cc + '''=' + CONVERT(varchar, Wsp) 
                from {1} W 
                left outer join CC on CC.Id = W.IdCC
                where W.{2} = {0}", przypId, TableName, IdPrzypColName);
        }

        public static string GetSplitLog(string TableName, string IdPrzypColName, string przypId)
        {
            DataSet ds = db.getDataSet(GetSplitSqlLog(TableName, IdPrzypColName, przypId));
            return db.Join(ds, 0, ",");
        }

        public static string GetSplitLog(SqlTransaction tr, string TableName, string IdPrzypColName, string przypId)
        {
            DataSet ds = db.getDataSet(tr, GetSplitSqlLog(TableName, IdPrzypColName, przypId));
            return db.Join(ds, 0, ",");
        }
        //-----------------------------
        public bool Update(SqlTransaction tr)
        {
            int przypId = Tools.StrToInt(IdPrzypisania, -1);
            if (przypId != -1)
            {
                bool ret;
                string prev = GetSplitLog(tr, TableName, IdPrzypColName, IdPrzypisania);
                db.execSQL(tr, String.Format("delete from {1} where {2} = {0}", przypId, TableName, IdPrzypColName));
                ret = Insert(tr, przypId);
                if (ret)
                {
                    string curr = GetSplitLog(tr, TableName, IdPrzypColName, IdPrzypisania);
                    Log.InfoTr(Log.SPLITY, TableName + ".Update", String.Format("SplitId/PrzypisanieId: {2}\nOld: {0}\nCurr: {1}", prev, curr, przypId));
                }
                return ret;
            }
            else
                return false;
        }

        public bool Update()
        {
            SqlTransaction tr = db.con.BeginTransaction();
            bool ok = Update(tr);
            if (ok)
                tr.Commit();
            else
                tr.Rollback();
            return ok;
        }
        //----------------------------------------------
        string insertedId = null;

        private bool InsertCC(string ccId, string cc, string wsp)
        {
            if (!String.IsNullOrEmpty(ccId))
            {
                DataSet ds = dsSplityWsp;
                DataTable dt = ds.Tables[0];
                DataRow[] exists = dt.Select("IdCC=" + ccId);
                if (exists.Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["cc"] = cc;
                    dr["IdCC"] = ccId;
                    dr["Wsp"] = wsp;
                    dt.Rows.Add(dr);

                    DataView view = ds.Tables[0].DefaultView;
                    view.Sort = "cc";
                    ds.Tables.RemoveAt(0);
                    ds.Tables.Add(view.ToTable());

                    dsSplityWsp = ds;

                    insertedId = ccId;
                    lvSplityWsp.DataSource = ds;
                    lvSplityWsp.DataBind();
                    return true;
                }
                else Tools.ShowMessage("Centrum kosztów jest już na liście.");
            }
            else Tools.ShowMessage("Proszę wybrać centrum kosztów.");
            return false;
        }
        //--------------------        
        protected void ddlCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            InsertCC(ddl.SelectedValue, ddl.SelectedItem.Text, null);
        }

        protected void lvSplityWsp_LayoutCreated(object sender, EventArgs e)
        {
            Tools.SetControlVisible(lvSplityWsp, "thControl", !IsReadOnly);
        }

        protected void lvSplityWsp_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (!String.IsNullOrEmpty(insertedId))
                {
                    DataRowView drv = Tools.GetDataRowView(e);
                    if (drv["IdCC"].ToString() == insertedId)
                    {
                        TextBox tb = e.Item.FindControl("WspTextBox") as TextBox;
                        if (tb != null) 
                            Tools.ExecOnStart2("wspfocus", String.Format("focus('{0}');", tb.ClientID));
                    }
                }
            }
        }

        protected void lvSplityWsp_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            DataSet ds = dsSplityWsp;
            DataTable dt = ds.Tables[0];
            dt.Rows[e.ItemIndex].Delete();
            dsSplityWsp = ds;

            lvSplityWsp.DataSource = ds;
            lvSplityWsp.DataBind();
        }

        private DataSet GetData()
        {
            string przId = IdPrzypisania;
            if (String.IsNullOrEmpty(przId)) przId = "-1";
            return db.getDataSet(String.Format(@"
                select W.Id, W.IdCC, CC.cc + ' - ' + CC.Nazwa as cc, convert(varchar, W.Wsp) as Wsp
                from {1} W
                left outer join CC on CC.Id = W.IdCC
                where {2} = {0}", przId, TableName, IdPrzypColName));
        }
        //-------------------------

        public bool Initialized
        {
            set { ViewState["initok"] = value; }
            get { return Tools.GetBool(ViewState["initok"], false); }
        }

        public DataSet dsSplityWsp
        {
            get
            {
                object o = ViewState["splitywsp"];
                if (o == null)
                    return GetData();
                else
                    return (DataSet)o;
            }
            set { ViewState["splitywsp"] = value; }
        }

        public string IdPrzypisania
        {
            set
            {
                string pid = IdPrzypisania;
                bool load = (!String.IsNullOrEmpty(pid) && pid != "-1") || db.getCount(dsSplityWsp) == 0;
                ViewState["IdPrzypisania"] = value;
                if (load)
                {
                    dsSplityWsp = GetData();
                    lvSplityWsp.DataSource = dsSplityWsp;
                    lvSplityWsp.DataBind();
                }
            }
            get { return Tools.GetStr(ViewState["IdPrzypisania"], "-1"); }
        }
    }
}
