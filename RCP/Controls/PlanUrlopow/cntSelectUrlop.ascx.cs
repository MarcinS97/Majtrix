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
    public partial class cntSelectUrlop : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
        int FMode = moAll;
        const int moAll = 0;
        const int moSelectable = 1;

        const string defKodUW       = "1000090080";     // Jabil
        const string defKolorUW     = "#FFFF00";
        const string defKodUW14     = "1100";
        const string defKolorUW14   = "#CCCC00";

        const int defKodUW14int     = 1100;

        const int puUW   = 1;   // typ planowanej absencji
        const int puUW14 = 2;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //------------------------
        string firstColor = null;
        string kodUW      = null;
        string kolorUW    = null;
        string kodUW14    = null; 
        string kolorUW14  = null; 

        protected void lvZmiany_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;

                bool sym = db.getBool(drv["PokazSymbolPU"], false);
                if (!sym) Tools.SetText(e.Item, "SymbolLabel", null);

                string color = drv["KolorPU"].ToString();
                if (String.IsNullOrEmpty(firstColor))
                    firstColor = color;                     

                bool wybor = Base.getBool(drv["WyborPU"], false) && EditMode;
                Button bt = (Button)Tools.SetControlVisible(e.Item, "btSelect", wybor);
                CheckBox cb = (CheckBox)Tools.SetControlVisible(e.Item, "cbSelect", wybor);
                if (bt != null && cb != null && wybor)
                {
                    cb.Attributes["onClick"] = "javascript:doClick('" + bt.ClientID + "');";
                    cb.Checked = dataItem.DisplayIndex == lvZmiany.SelectedIndex;
                }

                int typ = db.getInt(drv["Typ"], 0);
                switch (typ)
                {
                    case puUW:
                        kodUW = drv["Kod"].ToString();
                        kolorUW = drv["KolorPU"].ToString();
                        break;
                    case puUW14:
                        kodUW14 = drv["Kod"].ToString();
                        kolorUW14 = drv["KolorPU"].ToString();
                        break;
                }

                //HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("divZmiana");
                HtmlGenericControl div = (HtmlGenericControl)e.Item.FindControl("spanZmiana");
                if (div != null)
                {
                    int kod = db.getInt(drv["Kod"], 0);
                    if (kod == -1)
                    {
                        div.Attributes["class"] += " korekta";
                        if (!String.IsNullOrEmpty(firstColor))
                        {
                            Label lb = e.Item.FindControl("SymbolLabel") as Label;
                            if (lb != null)
                                lb.BackColor = ColorTranslator.FromHtml(firstColor);
                        }
                    }

                    if (wybor)
                    {
                        div.Attributes["onClick"] = "javascript:doClick('" + bt.ClientID + "');";
                        div.Attributes["class"] += " clickable";
                    }

                    if (kod == defKodUW14int && EditMode)
                    {
                        div.Attributes["title"] = "Urlop zostanie zaznaczony automatycznie";                    
                    }

                    bool nl = db.getBool(drv["NowaLinia"], false);
                    if (nl) Tools.SetControlVisible(e.Item, "ltNewLine", true);
                }
            }
        }

        protected void lvZmiany_DataBound(object sender, EventArgs e)
        {
            HtmlTableCell th = (HtmlTableCell)lvZmiany.FindControl("thSelect");
            if (th != null)
                th.Visible = EditMode;
            PlanParams = Tools.SetLineParams(
                String.IsNullOrEmpty(kodUW) ? defKodUW : kodUW,
                String.IsNullOrEmpty(kolorUW) ? defKolorUW : kolorUW,
                String.IsNullOrEmpty(kodUW14) ? defKodUW14 : kodUW14,
                String.IsNullOrEmpty(kolorUW14) ? defKolorUW14 : kolorUW14
                );
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
                    /*
                    if (lvZmiany.SelectedIndex != -1)
                    {
                        lvZmiany.SelectedIndex = -1;
                        if (SelectedChanged != null)
                            SelectedChanged(this, EventArgs.Empty);
                    }
                    */
                    lvZmiany.DataBind();   // nie usuwam zmieniam zaznaczenia tylko odswiezam zeby sie cb spowrotem zaznaczyl, jak bedzie > 1 zaznaczany to przywrocic to co powyzej
                    break;
                default:
                    break;
            }
        }

        //------------------------
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

        public string FirstUpper(object nazwa)
        {
            return Tools.FirstUpper(nazwa.ToString());
        }
        //------------------------
        public string SelectedUrlop
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
                    string nazwa = Tools.GetText(item, "NazwaLabel");
                    return Tools.SetLineParams(5, zid, symbol, color, 
                        null, 
                        nazwa, null);   // i info ale jest null
                }
                else return null;
            }
        }
        /*
        public string SelectedInfo
        {
            set { ViewState["sinfo"] = value; }
            get { return Tools.GetStr(ViewState["sinfo"]); }
        }
        */
        public string PlanParams    // kod | kolor | kod | kolor dla js
        {
            //set { ViewState["pupar"] = value; }
            //get { return Tools.GetStr(ViewState["pupar"]); }//, Tools.SetLineParams(defKodUW, defKolorUW, defKodUW14, defKolorUW14)); }
            set { hidPlanParams.Value = value; }
            get { return hidPlanParams.Value; }
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
                if (value)
                {
                    lvZmiany.SelectedIndex = 0;
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                }
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