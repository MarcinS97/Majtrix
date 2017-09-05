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
    public partial class cntSplityWsp2 : System.Web.UI.UserControl
    {
        private int FMode = moReadOnly;
        public const int moReadOnly = 0;
        public const int moEditable = 1;

        private int FType = tySplityWspP;
        public const int tySplityWspP = 0;
        public const int tySplityWsp = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (lvSplityWsp.Visible)  
            {
                if (!Initialized)
                {
                    //Initialized = true;
                    //lvSplityWsp.DataSource = dsSplityWsp;
                    //lvSplityWsp.DataBind();

                    //Tools.ExecOnStart2("wspsum", String.Format("cntSplityWspSum('{0}','{1}');", "sumwsp", "sum"));
                    //ExecSumuj();
                }
                if (!IsPostBack) // jak jest na innej zakładce niż widoczna to się nie pokazuje więc Initialized
                {
                }
                else
                {
                    if (Initialized)
                        UpdateDataSet();
                }
            }
        }

        public void ExecSumuj(bool once)
        {
            if (once)
                Tools.ExecOnStart2("wspsumPL1", String.Format("cntSplityWspSumPLsum('{0}','{1}');", "sumwsp", "sum"));
            else
                Tools.ExecOnStart2("wspsumPL", String.Format("cntSplityWspSumPL('{0}','{1}');", "sumwsp", "sum"));
        }

        public void StopExecSumuj()
        {
            Tools.ExecOnStart2("wspsumPLstop", "cntSplityWspSumPLStop();");
        }

        public void Prepare(string przId, string kierId, string dOd, string dDo, string pracId)  // prac id tylko do zapisu do log bo PodzialLudziImport jest usuwany przy imporcie i powiazanie po IdPrzypisania znika
        {
            hidKierId.Value = kierId;
            hidOd.Value = dOd;
            hidDo.Value = dDo;
            IdPrzypisania = przId;   // tu się zrobi databind
            PracId = pracId;
            ExecSumuj(false);
            Initialized = true;
        }

        public void Prepare(DataSet ds)
        {
            dsSplityWsp = ds;
            lvSplityWsp.DataSource = dsSplityWsp;
            lvSplityWsp.DataBind();
            ExecSumuj(false);
        }

        public void _Prepare(string przId, bool editable)
        {
            IdPrzypisania = przId;
            Mode = editable ? moEditable : moReadOnly;
            ExecSumuj(false);
        }

        protected bool GetBool(object b)
        {
            if (db.isNull(b))
                return false;
            else
                return (int)b == 0 ? false : true;
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

        public int Validate()
        {
            //----- comma -----
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
            //-----------------
            int ret = 0;
            double sum = 0;
            foreach (ListViewItem item in lvSplityWsp.Items)
            {
                string c1 = Tools.GetText(item, "hidIdCC");
                string c3 = Tools.GetText(item, "WspTextBox");
                double wsp;
                if (Double.TryParse(c3, out wsp))
                {
                    if (0 <= wsp && wsp <= 1)
                        sum += wsp;
                    else
                    {
                        ret = -3;    // niepoprawna wartość
                        break;
                    }
                }
                else
                {
                    ret = -2;   // nie liczba
                    break;
                }
            } 
            if (ret == 0 && Worktime.Round05(sum, 6) != 1)
                ret = -1;           // ostrzeżenie ze != 1, tylko jak nie ma innych błędów
            if (ret != 0)
                ExecSumuj(true);    // to powoduje ze suma się wyświetli mimo alertu
            return ret;
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
                    double sum = 0;
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
                            sum += wsp;
                        }
                        else
                            return false;
                    }
                    double cc499 = 1.0 - Worktime.Round05(sum, 4);
                    if (cc499 != 0)
                    {
                        string c1 = db.getScalar(tr, "select top 1 Id from CC where Surplus = 1");
                        if (!String.IsNullOrEmpty(c1))
                            db.execSQL(tr, db.insertCmd("SplityWsp", 0,
                                "IdSplitu,IdCC,Wsp",
                                przypId, c1, cc499));
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
        public bool Update(SqlTransaction tr, int przypId)   
        {
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
                    Log.InfoTr(Log.SPLITY, tb + ".Update", String.Format("SplitId/PrzypisanieId: {2} PracId: {3} DataOd: {4}\nOld: {0}\nCurr: {1}", prev, curr, przypId, PracId, hidOd.Value));
                }
                return ret;
            }
            else
                return false;
        }

        public bool Update(SqlTransaction tr, DataSet dsSplity)
        {
            if (dsSplity == null)
                return Update(tr, Tools.StrToInt(IdPrzypisania, -1));
            else
            {
                foreach (DataRow dr in db.getRows(dsSplity))
                {
                    int sid = db.getInt(dr, "Id", -1);
                    if (sid == -1 || !Update(tr, sid))
                        return false;
                }
                return true;
            }
        }

        public bool Update(DataSet dsSplity)
        {
            SqlTransaction tr = db.con.BeginTransaction();
            bool ok = Update(tr, dsSplity);
            if (ok)
                tr.Commit();
            else
                tr.Rollback();
            return ok;
        }
        //----------------------------------------------
        string insertedId = null;

        private bool InsertCC(string ccId, string cc, string wsp, bool moje, string klist)
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
                    dr["MojeCC"] = moje;
                    dr["KierList"] = klist;
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
                else Tools.ShowMessage("Centrum kosztów jest już na liście.");   //ShowMessage2 nie działa jak jest w jQuery popup
            }
            else Tools.ShowMessage("Proszę wybrać centrum kosztów.");
            return false;
        }
        //--------------------        
        protected void ddlCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string id, klist;
            Tools.GetLineParams(ddl.SelectedValue, out id, out klist);
            InsertCC(id, ddl.SelectedItem.Text, null, true, klist);
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
            ds.AcceptChanges();
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
            string kierId = hidKierId.Value;
            if (String.IsNullOrEmpty(przId)) przId = "-1";  // coś musi być
            DataSet ds;
            switch (FType)
            {
                default:
                case tySplityWspP:
                    ds = db.getDataSet(String.Format(@"
select W.Id, W.IdCC, CC.cc + ' - ' + CC.Nazwa as cc, convert(varchar, W.Wsp) as Wsp,

1 as MojeCC,
null as KierList

from SplityWspP W
left outer join CC on CC.Id = W.IdCC
where IdPrzypisania = {0}", przId));
                    break;
                case tySplityWsp:
                    ds = db.getDataSet(String.Format(@"
declare @sid int
declare @kid int
set @sid = {0}
set @kid = {1}

select W.Id, W.IdCC, CC.cc + ' - ' + CC.Nazwa as cc, convert(varchar, W.Wsp) as Wsp,
case when @kid = -99 or R.Id is not null then 1 else 0 end as MojeCC,
dbo.fn_GetccPrawaKierList(CC.Id, 1, ',') as KierList
from SplityWsp W
inner join CC on CC.Id = W.IdCC and CC.Surplus = 0
left join ccPrawa R on R.IdCC = W.IdCC and R.UserId = @kid
where IdSplitu = @sid", 
                     przId, kierId));
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
                ViewState["IdPrzypisania"] = value;
                dsSplityWsp = GetData();
                lvSplityWsp.DataSource = dsSplityWsp;
                lvSplityWsp.DataBind();
            }
            get { return Tools.GetStr(ViewState["IdPrzypisania"], "-1"); }
        }

        public string PracId
        {
            set { ViewState["pracid"] = value; }
            get { return Tools.GetStr("pracid"); }
        }
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