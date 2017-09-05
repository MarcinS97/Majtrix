using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Adm
{
    public partial class cntSqlMenuEditGrupy : System.Web.UI.UserControl
    {
        public delegate void ESelected(cntSqlMenuEditGrupy sender, string grupa, string id, CMode cmode); 
        public event ESelected ShowChildren;
        public event ESelected Edit;

        public enum CMode { Del, Query, Edit, New };

        const int cmDel   = -1;
        const int cmQuery = 0;
        const int cmEdit  = 1;
        const int cmNew   = 2;

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvSqlMenu);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Show(string grupa, string parentId)
        {
            Grupa = grupa;
            ParentId = parentId;

            bool f = String.IsNullOrEmpty(grupa);            
            paFilter.Visible = f;
            if (f)
            {
                if (ddlGrupa.Items.Count == 0)
                {
                    ddlGrupa.DataBind();
                    if (ddlGrupa.Items.Count > 0)
                    {
                        ddlGrupa.SelectedIndex = 0;
                    }
                }
                SelectGrupa();
            }
            else
                lbTitleNazwa.Text = grupa;

            bool p = !String.IsNullOrEmpty(parentId);
            paParent.Visible = p;
            if (p) 
                lbParent.Text = db.getScalar(db.conP, "select MenuText from SqlMenu where Id = " + parentId);
            else 
                lbParent.Text = null;
            
            gvSqlMenu.DataBind();  // za 2 razem nie binduje jak ta sama Group przekazana
            SetUpDown();
            cntModal.Show(false);
        }

        private void TriggerShowChildren(string id)
        {
            if (ShowChildren != null)
            {
                ShowChildren(this, Grupa, id, cmQuery);
            }
        }

        private void TriggerEdit(string id, CMode cmode)
        {
            if (Edit != null)
            {
                Edit(this, Grupa, id, cmode);
            }
        }

        protected void gvSqlMenuCmd_Click(object sender, EventArgs e)
        {
            string[] p = Tools.GetLineParams(gvSqlMenuCmdPar.Value);
            int mid = p.Length > 1 ? Tools.StrToInt(p[1], -1) : -1;
            switch (p[0])
            {
                case "select":
                    SelectedId = mid;
                    break;
                case "edit":
                    SelectedId = mid;
                    TriggerEdit(p[1], CMode.Edit);
                    break;
                case "chld":
                    SelectedId = mid;
                    TriggerShowChildren(p[1]);
                    break;
            }
        }

        //----------------------
        public string Grupa
        {
            set { hidGrupa.Value = value; }
            get { return hidGrupa.Value; }
        }

        public string ParentId
        {
            set { hidParentId.Value = value; }
            get { return hidParentId.Value; }
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            TriggerEdit(ParentId, CMode.New);
        }

        protected void btAddSub_Click(object sender, EventArgs e)
        {
            TriggerEdit(SelectedId.ToString(), CMode.New);
        }

        private void HideSelection()
        {
            if (gvSqlMenu.SelectedIndex != -1)
            {
                gvSqlMenu.SelectedRow.CssClass = Tools.RemoveClass(gvSqlMenu.SelectedRow.CssClass, gvSqlMenu.SelectedRowStyle.CssClass);  // bo zostawala :/
                gvSqlMenu.SelectedIndex = -1;
            }
        }

        private void SelectGrupa()
        {
            string g = ddlGrupa.SelectedValue;
            lbTitleNazwa.Text = g;
            Grupa = g;
            HideSelection();
        }

        protected void ddlGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectGrupa();
        }

        //----------------------
        /*
if (e.Row.RowType == DataControlRowType.DataRow)
    {
        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
        e.Row.ToolTip = "Click to select this row.";
    }
         */
        public int SelectedId  
        {
            set
            {
                HideSelection();
                for (int i = 0; i <= gvSqlMenu.DataKeys.Count - 1; i++)
                {
                    if ((int)gvSqlMenu.DataKeys[i].Value == value)
                    {
                        gvSqlMenu.SelectedIndex = i;
                        break;
                    }
                }
                SetUpDown();
            }
            get { return Tools.GetInt(gvSqlMenu.SelectedValue, -1); }
        }

        protected void dsSqlMenu_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
        }

        protected void gvSqlMenu_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gvSqlMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        private void SetUpDown()
        {
            bool isAny = gvSqlMenu.SelectedIndex != -1; //!String.IsNullOrEmpty(ClickedId);
            bool isFirst = gvSqlMenu.SelectedIndex == 0;
            bool isLast = gvSqlMenu.SelectedIndex == gvSqlMenu.DataKeys.Count - 1;
            lbtUp.Enabled = isAny && !isFirst;
            lbtDown.Enabled = isAny && !isLast;
            btAddSub.Enabled = isAny;
        }

        private bool UpdateKolejnosc(int[] list, int selId)    // aktualizauje całość na wypadek gdyby były null'e lub dziury, przy małej ilości do przyjęcia
        {
            bool ok = true;
            for (int i = 0; i < list.Length; i++)
                if (!db.update(db.conP, "SqlMenu", 0, "Kolejnosc", String.Format("Id={0}", list[i]), (i + 1) * 10))
                    ok = false;
            gvSqlMenu.DataBind();
            SelectedId = selId;
            SetUpDown();
            return ok;
        }

        private void MoveUp()
        {
            int idx = gvSqlMenu.SelectedIndex;
            int cnt = gvSqlMenu.DataKeys.Count;
            if (0 < idx && idx < cnt)
            {
                int[] list = new int[cnt];
                for (int i = 0; i < cnt; i++)
                    list[i] = (int)gvSqlMenu.DataKeys[i].Value;
                int tmp = list[idx - 1];
                list[idx - 1] = list[idx];
                list[idx] = tmp;
                UpdateKolejnosc(list, (int)gvSqlMenu.DataKeys[idx].Value);
            }
        }

        private void MoveDown()
        {
            int idx = gvSqlMenu.SelectedIndex;
            int cnt = gvSqlMenu.DataKeys.Count;
            if (0 <= idx && idx < cnt - 1)
            {
                int[] list = new int[cnt];
                for (int i = 0; i < cnt; i++)
                    list[i] = (int)gvSqlMenu.DataKeys[i].Value;
                int tmp = list[idx + 1];
                list[idx + 1] = list[idx];
                list[idx] = tmp;
                UpdateKolejnosc(list, (int)gvSqlMenu.DataKeys[idx].Value);
            }
        }

        protected void lbtUp_Click(object sender, EventArgs e)
        {
            MoveUp();
        }

        protected void lbtDown_Click(object sender, EventArgs e)
        {
            MoveDown();
        }

        protected void logo_Click(object sender, EventArgs e)
        {
            cntImportLogo2.ShowModal();           
        }
    }
}