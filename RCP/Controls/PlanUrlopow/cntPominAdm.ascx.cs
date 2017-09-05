using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntPominAdm : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, 0);
            Tools.PrepareSorting(ListView1, 1, 10);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetNaDzien(true);
            }
        }
        //--------------------
        private void SetNaDzien(bool set)
        {
            dtNaDzien.ReadOnly = !set;
            dtNaDzien.EditBox.Enabled = set;
            if (set)
            {
                if (!dtNaDzien.IsValid)
                    dtNaDzien.Date = DateTime.Today;
                hidNaDzien.Value = dtNaDzien.DateStr;
            }
            else
            {
                hidNaDzien.Value = null;
            }
        }

        protected void dtNaDzien_Changed(object sender, EventArgs e)
        {
            SetNaDzien(true);
        }

        protected void cbNaDzien_CheckedChanged(object sender, EventArgs e)
        {
            SetNaDzien(cbNaDzien.Checked);
        }

        private bool cbNaDzienTmp
        {
            set { ViewState["cbnad"] = value; }
            get { return Tools.GetBool(ViewState["cbnad"], cbNaDzien.Checked); }
        }

        protected void ddlPracFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlPracFilter.SelectedValue))
            {
                bool b = cbNaDzienTmp;
                cbNaDzien.Checked = b;
                SetNaDzien(b);
            }
            else
            {
                cbNaDzienTmp = cbNaDzien.Checked;
                cbNaDzien.Checked = false;
                SetNaDzien(false);
            }
        }

        //------------------
        private void InitItem(ListViewItemEventArgs e, bool create)
        {
            bool insertItem = e.Item.ItemType == ListViewItemType.InsertItem;
            if (insertItem ||
                e.Item.ItemType == ListViewItemType.DataItem && ListView1.EditIndex == Tools.GetDisplayIndex(e))
            {
                string pracId = null;
                string powodKod = null;
                string prac = null;
                string powod = null;
                if (create)
                {
                    if (insertItem)
                    {
                        pracId = ddlPracFilter.SelectedValue;
                        if (!String.IsNullOrEmpty(pracId))
                        {
                            DataRow dr = db.getDataRow(String.Format("select top 1 * from PlanUrlopowPomin where IdPracownika = {0} and Od > DATEADD(YEAR, -1, GETDATE()) order by Od desc",pracId));
                            if (dr != null)
                            {
                                powodKod = db.getValue(dr, "PowodKod");
                                Tools.SetText2(e.Item, "PowodTextBox", db.getValue(dr, "Powod"));
                                DateTime? dDo = db.getDateTime(dr, "Do");
                                if (dDo != null)
                                {
                                    DateTime dt = ((DateTime)dDo).AddDays(1);
                                    DateEdit deOd = e.Item.FindControl("deOd") as DateEdit;
                                    if (deOd != null)
                                        deOd.Date = dt;
                                }
                            }
                        }
                    }
                }
                else // update
                {
                    DataRowView drv = Tools.GetDataRowView(e);
                    pracId = drv["IdPracownika"].ToString();
                    prac = "* " + drv["Pracownik"].ToString();
                    powodKod = drv["PowodKod"].ToString();
                    powod = drv["PowodKodNapis"].ToString();
                }
                DataSet ds = db.getDataSet("select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Pracownik, Id from VPrzypisaniaNaDzis where IdPrzypisania is not null and Status >= 0 order by 1");
                Tools.BindData(e.Item, "ddlPracownik", ds, "Pracownik", "Id", true, pracId, prac);

                ds = db.getDataSet("select ISNULL(Nazwa,'') + ISNULL(' (' + Nazwa2 + ')','') as Powod, Kod from Kody where Typ = 'ABSDL' and Aktywny = 1 order by Lp");
                Tools.BindData(e.Item, "ddlPowod", ds, "Powod", "Kod", true, powodKod, powod);
            }
        }

        private bool Update(Control item, IOrderedDictionary values, bool insert)
        {
            values["IdPracownika"] = Tools.GetDdlSelectedValueInt(item, "ddlPracownik");
            values["PowodKod"] = Tools.GetDdlSelectedValueInt(item, "ddlPowod");
            return true;
        }

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            InitItem(e, true);
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            InitItem(e, false);
        }

        bool stop = false;

        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = stop || !Update(e.Item, e.Values, true);
        }

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = stop || !Update(ListView1.EditItem, e.NewValues, false);
        }

        //private string GetMessage()
        //{
        //    Tools.ShowConfirm("Pracownik już istnieje z okresem: {0} do {1}"
 
        //}

        private bool CheckPrac(string id, ListViewItem item)
        {
            DropDownList ddl = item.FindControl("ddlPracownik") as DropDownList;
            DateEdit deOd = item.FindControl("deOd") as DateEdit;
            DateEdit deDo = item.FindControl("deDo") as DateEdit;
            if (ddl != null && deOd != null && deDo != null)
                return CheckPrac(id, ddl.SelectedValue, deOd.DateStr, deDo.DateStr, true);
            else
                return false;
        }
        
        private bool CheckPrac(string id, string pracId, string dOd, string dDo, bool showMessage)
        {
            DataRow dr = db.getDataRow(String.Format(@"
declare @od datetime
declare @do datetime
set @od = {1}
set @do = {2}
select X.*, 
	ISNULL(K.Nazwa + ' (' + K.Nazwa2 + ')', '') + 
	case when K.Kod is not null and X.Powod is not null then ' - ' else '' end +
	ISNULL(X.Powod, '') as PowodNapis  
from PlanUrlopowPomin X 
left join Kody K on K.Typ = 'ABSDL' and K.Kod = X.PowodKod
where X.IdPracownika = {0}
and @od <= ISNULL(X.Do, '20990909') and ISNULL(@do, '20990909') >= X.Od 
{3}
            ", pracId,
               db.strParam(dOd), 
               db.nullStrParam(dDo), 
               String.IsNullOrEmpty(id) ? null : "and X.Id != " + id));
            if (dr != null)
            {
                if (showMessage)
                {
                    DateTime? dtOd = db.getDateTime(dr, "Od");
                    DateTime? dtDo = db.getDateTime(dr, "Do");
                    Tools.ShowError("Okres pokrywa się z już istniejącym wpisem:\\n{0} do {1}\\n{2}",
                        dtOd != null ? Tools.DateToStr(dtOd) : "",
                        dtDo != null ? Tools.DateToStr(dtDo) : "- bez terminu",
                        db.getValue(dr, "PowodNapis")
                        );
                }
                return false;
            }
            else
                return true;
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Insert":
                    if (((Button)e.CommandSource).ID == "InsertButton")
                    {
                        stop = !CheckPrac(null, e.Item);
                    }
                    break;
                case "Update":
                    if (((Button)e.CommandSource).ID == "UpdateButton")
                    {
                        stop = !CheckPrac(Tools.GetDataKey(ListView1, e), e.Item);
                    }
                    break;
            }
        }
        //------------------------------
    }
}