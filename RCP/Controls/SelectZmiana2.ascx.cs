using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Data;
using System.Collections.Specialized;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class SelectZmiana2 : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
        int FMode = moAll;
        const int moAll = 0;
        const int moSelectable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //------------------------
        protected void lvZmiany_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                
                bool v = Base.getBool(drv["Visible"], false) && EditMode;
                Button bt = (Button)Tools.SetControlVisible(e.Item, "btSelect", v);
                CheckBox cb = (CheckBox)Tools.SetControlVisible(e.Item, "cbSelect", v);
                if (bt != null && cb != null && v)
                {
                    cb.Attributes["onClick"] = "javascript:doClick('" + bt.ClientID + "');";
                    cb.Checked = dataItem.DisplayIndex == lvZmiany.SelectedIndex;
                }

                //HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("divZmiana");
                HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("spanZmiana");
                if (div != null)
                {
                    div.Attributes["onClick"] = "javascript:doClick('" + bt.ClientID + "');";
                    if (v) div.Attributes["class"] += " clickable";
                    bool nl = db.getBool(drv["NowaLinia"], false);
                    if (nl) Tools.SetControlVisible(e.Item, "ltNewLine", true);
                        //div.Attributes["class"] += " newline";
                }

                //----- zgoda -----
                bool zgoda = db.getBool(drv["ZgodaNadg"], false);
                bool hide = db.getBool(drv["HideZgoda"], false);
                Tools.SetControlVisible(e.Item, "lbZgoda", !hide && zgoda);
                Tools.SetControlVisible(e.Item, "lbBrakZgody", !hide && !zgoda);

                //----- nadgodziny -----
                Label lb = (Label)e.Item.FindControl("lbNadgodziny");
                if (lb != null)
                {
                    int? ztyp = (int?)drv["TypZmiany"];
                    if (Base.isNull(ztyp)) ztyp = 0;
                    switch (ztyp)
                    {
                        default:
                        case 0:
                            string n = drv["Nadgodziny"].ToString();
                            if (!String.IsNullOrEmpty(n))
                            {
                                lb.Text = "N: " + n + "%" + "<br />";
                                lb.ToolTip = "kolejne nadgodziny";
                                lb.Visible = true;
                            }
                            break;
                        case 1:
                            string d = drv["NadgodzinyDzien"].ToString();
                            n = drv["NadgodzinyNoc"].ToString();
                            if (!String.IsNullOrEmpty(d) || !String.IsNullOrEmpty(n))
                            {
                                lb.Text = String.Format("N: {0} / {1}<br />", 
                                    String.IsNullOrEmpty(d) ? "brak" : d + "%",
                                    String.IsNullOrEmpty(n) ? "brak" : n + "%");
                                lb.ToolTip = "nadgodziny zwykłe/nocne";
                                lb.Visible = true;
                            }
                            break;
                        case 2:
                            Tools.SetControlVisible(e.Item, "OdLabel", false);
                            Tools.SetControlVisible(e.Item, "DoLabel", false);
                            Tools.SetControlVisible(e.Item, "lbMargines", false);
                            Tools.SetControlVisible(e.Item, "StawkaLabel", false);
                            Tools.SetControlVisible(e.Item, "lbNadgodziny", false);
                            Tools.SetControlVisible(e.Item, "lbBrakZgody", false);
                            Tools.SetControlVisible(e.Item, "lbZgoda", false);
                            break;
                    }
                    if (hide) lb.Visible = true;
                }
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

        protected void lvZmiany_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Unselect":
                    if (lvZmiany.SelectedIndex != -1)
                    {
                        lvZmiany.SelectedIndex = -1;
                        if (SelectedChanged != null)
                            SelectedChanged(this, EventArgs.Empty);
                    }
                    break;
                default:
                    break;
            }
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
                    if (lb != null)
                    {
                        symbol = lb.Text;
                        color = ColorTranslator.ToHtml(lb.BackColor);
                    }
                    string czas;
                    try
                    {
                        int tod = Int32.Parse(((Label)item.FindControl("OdLabel")).Text.Substring(0, 2));
                        int tdo = Int32.Parse(((Label)item.FindControl("DoLabel")).Text.Substring(0, 2));
                        czas = String.Format("{0}-{1}", tod, tdo);
                    }
                    catch
                    {
                        czas = null;
                    }
                    return Tools.SetLineParams(5, zid, symbol, color, null, czas, null);   // i info ale jest null
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
                if (!value)
                {
                    lvZmiany.SelectedIndex = -1;
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                }
                lvZmiany.DataBind();
            }
        }

        public int Mode
        {
            set 
            { 
                FMode = value;
                hidMode.Value = value.ToString();
            }
            get { return FMode; }
        }
    }
}