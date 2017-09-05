using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Collections.Specialized;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class SelectZmiana : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //------------------------
        protected void lvZmiany_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdSelect");
                if (td != null)
                    td.Visible = EditMode;
            }
        }

        protected void lvZmiany_DataBound(object sender, EventArgs e)
        {
            HtmlTableCell th = (HtmlTableCell)lvZmiany.FindControl("thSelect");
            if (th != null)
                th.Visible = EditMode;
        }

        protected void lvZmiany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }

        //------------------------
        public string StawkaText(string stawka)
        {
            return App.StawkaText(stawka);
        }

        public Color GetColorNull(string color)
        {
            try
            {
                return ColorTranslator.FromHtml(color);
            }
            catch
            {
                return Color.Transparent;
            }
        }
        //------------------------
        public string SelectedZmiana
        {
            get
            {
                if (lvZmiany.SelectedIndex != -1)
                {
                    string zid = lvZmiany.DataKeys[lvZmiany.SelectedIndex].Value.ToString();
                    string symbol = null;
                    string color = null;
                    ListViewItem item = lvZmiany.Items[lvZmiany.SelectedIndex];
                    Label lb = (Label)item.FindControl("SymbolLabel");
                    if (lb != null) symbol = lb.Text;
                    Panel pa = (Panel)item.FindControl("KolorSample");
                    if (pa != null) color = ColorTranslator.ToHtml(pa.BackColor);
                    return Tools.SetLineParams(4, zid, symbol, color, null, null, null); 
                }
                else return null;
            }
        }

        public bool EditMode
        {
            get { return hidEditMode.Value == "E"; }
            set 
            {
                hidEditMode.Value = value ? "E" : "Q";
                if (!value) lvZmiany.SelectedIndex = -1;
                lvZmiany.DataBind();
            }
        }
    }
}