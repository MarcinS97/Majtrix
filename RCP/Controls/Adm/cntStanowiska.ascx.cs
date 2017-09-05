using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class cntStanowiska : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //----------------------------------
        public DataRow GetCurrent()
        {
            string data = DateTime.Today.ToStringDb();
            return db.getDataRow(String.Format(@"
select top 1 R.*, D.Nazwa as Dzial, S.Nazwa as Stanowisko 
from PracownicyStanowiska R 
left outer join Dzialy D on D.Id = R.IdDzialu
left outer join Stanowiska S on S.Id = R.IdStanowiska
where R.IdPracownika = {0} and {1} between R.Od and ISNULL(R.Do, '20990909')
                ", PracId, data));
        }
        //----------------------------------
        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {

        }

        private void PrepareItem(ListViewItemEventArgs e, bool create)
        {
            bool select, edit, insert;
            int lim = Tools.GetListItemMode(e, ListView1, out select, out edit, out insert);
            switch (lim)
            {
                case Tools.limSelect:
                    if (!create)
                    {
                    }
                    break;
                case Tools.limEdit:
                    if (!create)
                    {
                        DataRowView drv = Tools.GetDataRowView(e);
                        if (Lic.DzialStanowisko)
                        {
                            Tools.SelectItem(e.Item, "ddlStanowisko", drv["IdStanowiska"]);
                        }
                        else
                        {
                            Tools.SelectItem(e.Item, "ddlDzial2", drv["IdDzialu"]);
                            Tools.SelectItem(e.Item, "ddlStanowisko2", drv["IdStanowiska"]);
                        }
                        Tools.SelectItem(e.Item, "ddlGrupa", drv["Grupa"]);
                        Tools.SelectItem(e.Item, "ddlKlasyfikacja", drv["Klasyfikacja"]);
                        Tools.SelectItem(e.Item, "ddlGrade", drv["Grade"]);
                        /*
                        TextBox tb = e.Item.FindControl("tbGrupa") as TextBox;
                        DropDownList ddl = e.Item.FindControl("ddlGrupa") as DropDownList;
                        if (tb != null && ddl != null)
                            ddl.Attributes["OnChange"] = String.Format("javascript:ddlUpdateText('{0}','{1}');", ddl.ClientID, tb.ClientID);
                         */

                        Tools.SetControlVisible(e.Item, "tdDzial", Lic.DzialStanowisko);
                        Tools.SetControlVisible(e.Item, "tdDzial2", !Lic.DzialStanowisko);
                        Tools.SetControlVisible(e.Item, "tdStanowisko2", !Lic.DzialStanowisko);
                    }
                    break;
                case Tools.limInsert:
                    if (create)
                    {
                        Tools.SetControlVisible(e.Item, "tdDzial", Lic.DzialStanowisko);
                        Tools.SetControlVisible(e.Item, "tdDzial2", !Lic.DzialStanowisko);
                        Tools.SetControlVisible(e.Item, "tdStanowisko2", !Lic.DzialStanowisko);
                    }
                    break;
            }
        }

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            PrepareItem(e, true);
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            PrepareItem(e, false);
        }
        //----------------------------------
        private bool Validate(ListViewItem item, IOrderedDictionary values, string id)
        {
            return cntKartyRcp.Validate("PracownicyStanowiska", "Id", 0, PracId, values["Od"], values["Do"], id, null);
        }
        //----------------------------------
        private void SetDzialStanowisko(Control cnt, IOrderedDictionary values)
        {
            if (Lic.DzialStanowisko)
            {
                values["IdDzialu"] = -9;
                values["IdStanowiska"] = Tools.GetDdlSelectedValueInt(cnt, "ddlStanowisko");   // zeby automat nie nadpisał trzeba było zmienić ValueFieldName
            }
            else
            {
                values["IdDzialu"] = Tools.GetDdlSelectedValueInt(cnt, "ddlDzial2");
                values["IdStanowiska"] = Tools.GetDdlSelectedValueInt(cnt, "ddlStanowisko2");
            }
        }

        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            SetDzialStanowisko(e.Item, e.Values);
            e.Cancel = !Validate(e.Item, e.Values, null);
        }

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            SetDzialStanowisko(ListView1.EditItem, e.NewValues);
            e.Cancel = !Validate(ListView1.EditItem, e.NewValues, Tools.GetDataKey((ListView)sender, e));
        }
        //------
        protected void ListView1_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            Updated = true;
        }

        protected void ListView1_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            Updated = true;
        }

        protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            Updated = true;
        }
        //----------------------------------
        public string PracId
        {
            set 
            {
                ViewState["pracid"] = value;
                hidPracId.Value = value;
                Updated = false;
                ListView1.InsertItemPosition = InsertItemPosition.None;
                ListView1.EditIndex = -1;
                ListView1.SelectedIndex = -1;
            }
            get { return Tools.GetStr(ViewState["pracid"]); }
        }

        public bool Updated
        {
            set { ViewState["updated"] = value; }
            get { return Tools.GetBool(ViewState["updated"], false); }
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            DataPager dp = Tools.Pager(ListView1);
            if (dp != null)
            {
                Tools.SetControlVisible(ListView1, "trPager1", dp.TotalRowCount > dp.PageSize);
                dp.Visible = dp.TotalRowCount > dp.PageSize;
            }
        }

    }
}