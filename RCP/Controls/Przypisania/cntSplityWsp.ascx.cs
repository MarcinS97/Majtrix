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

//http://www.nullskull.com/q/10472395/storing-data-in-viewstate.aspx
//http://forums.asp.net/t/1725790.aspx/1

namespace HRRcp.Controls.Przypisania
{
    public partial class cntSplityWsp : System.Web.UI.UserControl
    {
        private int FMode = moReadOnly;
        public const int moReadOnly = 0;
        public const int moEditable = 1;

        private int FType = tySplityWspP;
        public const int tySplityWspP = 0;
        public const int tySplityWsp = 1;

        private int FNoGroup = 0;  // 0 razem z grupami
        public int NoGroup
        {
            set { FNoGroup = value; }
            get { return FNoGroup; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource2.SelectParameters["noGroup"].DefaultValue = FNoGroup.ToString(); 
            
            if (lvSplityWsp.Visible)  
            {
                if (!Initialized)
                {
                    Initialized = true;
                    lvSplityWsp.DataSource = dsSplityWsp;
                    lvSplityWsp.DataBind();
                    //Tools.ExecOnStart2("wspsum", String.Format("cntSplityWspSum('{0}','{1}');", "sumwsp", "sum"));
                    ExecSumuj();
                }
                if (!IsPostBack) // jak jest na innej zakładce niż widoczna to się nie pokazuje więc Initialized
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
            Mode = editable ? moEditable : moReadOnly;
            ExecSumuj();
        }
        //------------------------------------------
        protected bool IsReadOnly
        {
            get { return FMode == moReadOnly; }
        }

        protected string IsLastColClass
        {
            get { return FMode == moReadOnly ? " lastcol" : null; }
        }

        protected InsertItemPosition InsertPosition
        {
            get { return FMode == moReadOnly ? InsertItemPosition.None : InsertItemPosition.LastItem; }
        }

        //------------------------------------------
        public void Assign(cntSplityWsp splity)
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
            if (ds != null) // jak nie ma przypisaniaId
            {
                DataTable dt = ds.Tables[0];
                foreach (ListViewItem item in lvSplityWsp.Items)
                {
                    string c1 = Tools.GetText(item, "hidIdCC");
                    string c3 = Tools.GetText(item, "WspTextBox");
                    if (!String.IsNullOrEmpty(c1))   //<<<<<<<<<<<< updatepanel, zabezpiwczenie ale nie powinno mieć miejsca
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

            switch (FType)
            {
                default:
                case tySplityWspP:
                    foreach (ListViewItem item in lvSplityWsp.Items)
                    {
                        string c1 = Tools.GetText(item, "hidIdCC");
                        string c3 = Tools.GetText(item, "WspTextBox");
                        double wsp;
                        if (Double.TryParse(c3, out wsp))
                        {
                            db.execSQL(tr, db.insertCmd("SplityWspP", 0,
                                "IdPrzypisania,IdCC,Wsp",
                                przypId, c1, wsp));
                        }
                        else
                            return false;
                    }
                    break;
                case tySplityWsp:
                    foreach (ListViewItem item in lvSplityWsp.Items)
                    {
                        string c1 = Tools.GetText(item, "hidIdCC");
                        string c3 = Tools.GetText(item, "WspTextBox");
                        double wsp;
                        if (Double.TryParse(c3, out wsp))
                        {
                            db.execSQL(tr, db.insertCmd("SplityWsp", 0,
                                "IdSplitu,IdCC,Wsp",
                                przypId, c1, wsp));
                        }
                        else
                            return false;
                    }
                    break;
            }
            return true;
        }

        //---------------------------
        public static string GetSplitSqlLog(int typ, string przypId)
        {
            switch (typ)
            {
                default:
                case tySplityWspP:
                    return String.Format(@"
select '''' + cc + '''=' + CONVERT(varchar, Wsp) 
from SplityWspP W 
left outer join CC on CC.Id = W.IdCC
where W.IdPrzypisania = {0}", przypId);
                case tySplityWsp:
                    return String.Format(@"
select '''' + cc + '''=' + CONVERT(varchar, Wsp) 
from SplityWsp W 
left outer join CC on CC.Id = W.IdCC
where W.IdSplitu = {0}", przypId);
                    break;
            }
        }

        public static string GetSplitLog(int typ, string przypId)
        {
            DataSet ds = db.getDataSet(GetSplitSqlLog(typ, przypId));
            return db.Join(ds, 0, ",");
        }

        public static string GetSplitLog(SqlTransaction tr, int typ, string przypId)
        {
            DataSet ds = db.getDataSet(tr, GetSplitSqlLog(typ, przypId));
            return db.Join(ds, 0, ",");
        }
        //-----------------------------
        public bool Update(SqlTransaction tr)
        {
            int przypId = Tools.StrToInt(IdPrzypisania, -1);
            if (przypId != -1)
            {
                bool ret;
                string prev = GetSplitLog(tr, FType, IdPrzypisania);
                string tb;
                switch (FType)
                {
                    default:
                    case tySplityWspP:
                        tb = "SplityWspP";
                        db.execSQL(tr, String.Format("delete from SplityWspP where IdPrzypisania = {0}", przypId));
                        ret = Insert(tr, przypId);
                        break;
                    case tySplityWsp:
                        tb = "SplityWsp";
                        db.execSQL(tr, String.Format("delete from SplityWsp where IdSplitu = {0}", przypId));
                        ret = Insert(tr, przypId);
                        break;
                }
                if (ret)
                {
                    string curr = GetSplitLog(tr, FType, IdPrzypisania);
                    Log.InfoTr(Log.SPLITY, tb + ".Update", String.Format("SplitId/PrzypisanieId: {2}\nOld: {0}\nCurr: {1}", prev, curr, przypId));
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
                            //tb.Focus();
                            Tools.ExecOnStart2("wspfocus", String.Format("focus('{0}');", tb.ClientID));
                    }
                }
            }
        }

        protected void lvSplityWsp_DataBound(object sender, EventArgs e)
        {
            /*
            if (String.IsNullOrEmpty(hidPrevValues.Value))
                hidPrevValues.Value = GetValues();
             */
        }

        protected void lvSplityWsp_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            /*
            string ccId = Tools.GetDdlSelectedValue(e.Item, "ddlCC");
            if (!String.IsNullOrEmpty(ccId))
            {
                string cc = Tools.GetDdlSelectedText(e.Item, "ddlCC");
                DataSet ds = dsSplityWsp;
                DataTable dt = ds.Tables[0];
                DataRow[] exists = dt.Select("IdCC=" + ccId);
                if (exists.Length == 0)
                {
                    string wsp = Tools.GetText(e.Item, "WspTextBox");

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

                    lvSplityWsp.DataSource = ds;
                    lvSplityWsp.DataBind();
                    return;
                }
                else Tools.ShowMessage("Centrum kosztów jest już na liście.");
            }
            else Tools.ShowMessage("Proszę wybrać centrum kosztów.");
            e.Cancel = true;
             */ 
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

        protected void lvSplityWsp_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    break;
            }
        }

        protected void lvSplityWsp_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            /*
            if (e.Exception != null)
            {                   
                e.ExceptionHandled = true;
            }
            */ 
        }

        private DataSet GetData()
        {
            string przId = IdPrzypisania;
            if (String.IsNullOrEmpty(przId)) przId = "-1";  // coś musi być
            DataSet ds;
            switch (FType)
            {
                default:
                case tySplityWspP:
                    ds = db.getDataSet(String.Format(@"
select W.Id, W.IdCC, CC.cc + ' - ' + CC.Nazwa as cc, convert(varchar, W.Wsp) as Wsp
from SplityWspP W
left outer join CC on CC.Id = W.IdCC
where IdPrzypisania = {0}
order by cc
                    ", przId));
                    break;
                case tySplityWsp:
                    ds = db.getDataSet(String.Format(@"
select W.Id, W.IdCC, CC.cc + ' - ' + CC.Nazwa as cc, convert(varchar, W.Wsp) as Wsp
from SplityWsp W
left outer join CC on CC.Id = W.IdCC
where IdSplitu = {0}
order by cc
                    ", przId));
                    break;
            }
            return ds;
        }
        //-------------------------
        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        public int Type
        {
            set { FType = value; }
            get { return FType; }
        }

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
                bool load = (!String.IsNullOrEmpty(pid) && pid != "-1") || db.getCount(dsSplityWsp) == 0;   // jeżeli coś jest to jest to nowy do zapisania
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
        /*
        public string IdPrzypisaniaSet
        {
            set { ViewState["IdPrzypisania"] = value; }
        }
        */
    }
}










/*
List<string> mylist = new List<string>() { "stealthy", "ninja", "panda"};
listView.DataSource = mylist;
listView.DataBind();

aspx
<asp:ListView ID="listView" runat="server">
    <ItemTemplate>
        <asp:Label Text="<%#Container.DataItem %>" runat="server" />
    </ItemTemplate>
</asp:ListView>
 */