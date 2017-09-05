using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls
{
    public partial class cntNadgodzinyOdbior : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //Grid.Prepare(gvList);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cbSelect_CheckedChanged(object sender, EventArgs e)
        {
            SetButtonsVisible();
        }

        protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool check = (sender as CheckBox).Checked;

            // Iterate through the Products.Rows property
            foreach (GridViewRow row in gvList.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null)
                    cb.Checked = check;
            }
            SetButtonsVisible();
        }
        
        void SetButtonsVisible()
        {
            bool anyChecked = false;

            foreach (GridViewRow row in gvList.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null && cb.Checked)
                {
                    anyChecked = true;
                    break;
                }
            }

            btnCollectMany.Enabled = anyChecked;
        }

        string[] GetSelected()
        {
            List<string> list = new List<string>();
            foreach (GridViewRow row in gvList.Rows)
            {
                string id = gvList.DataKeys[row.RowIndex].Value.ToString();
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null && cb.Checked)
                    list.Add(id);
            }
            return list.ToArray();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = Tools.GetText(e.Row, "hidStatus");
                switch (status)
                {
                    //case "2":
                    //    e.Row.CssClass = "success";
                    //    break;
                    //case "-1":
                    //    e.Row.CssClass = "danger";
                    //    break;
                    default:
                        break;
                }
            }
        }

        protected void btnCollect_Click(object sender, EventArgs e)
        {
            string id = (sender as Button).CommandArgument;

            if(!String.IsNullOrEmpty(id))
            {

            }
        }

        protected void btnCollectMany_Click(object sender, EventArgs e)
        {

        }






    }
}