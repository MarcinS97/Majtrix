using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PlanPracyInHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        PlanPracyLineHeader plHeader = null;

        public void DataBind(PlanPracyLineHeader ph, int typ, bool vis1)
        {
            plHeader = ph;
            string[] data1 = ph.HeaderData;
            Repeater1.DataSource = data1;
            Repeater1.DataBind();
            switch (typ)
            {
                case PlanPracy.moZmiany:
                    
                    //ltThCheck.Text = "<th class\"colselect\"></th>";
                    //ltThCheck.Visible = true;
                    break;
                case PlanPracy.moAccept:
                    string[] data2 = ph.SumData;
                    Repeater2.Visible = true;
                    Repeater2.DataSource = data2;
                    Repeater2.DataBind();
                    break;
            }
        }

        //----- DNI --------------------------
        int dcnt = 0;

        protected void Repeater1_DataBinding(object sender, EventArgs e)
        {
            dcnt = 0;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbDay = Tools.FindLabel(e.Item, "DayLabel");
                Label lbDate = Tools.FindLabel(e.Item, "DataLabel");
                //HtmlTableCell th = (HtmlTableCell)e.Item.FindControl("thData");
                Literal th = (Literal)e.Item.FindControl("ltThDay"); 
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgAccepted");
                if (lbDay != null && lbDate != null && th != null && img != null)  
                {
                    string css = null;
                    string info = (string)e.Item.DataItem;
                    //if (info.Contains("h")) css += " holiday";
                    //if (info.Contains("t")) css += " today";

                    Control item = plHeader.Repeater.Items[dcnt++];
                    lbDay.Text = Tools.GetText(item, "DataLabel");
                    lbDate.Text = Tools.GetText(item, "DayLabel");
                    
                    HtmlTableCell th1 = (HtmlTableCell)item.FindControl("thData");
                    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)item.FindControl("imgAccepted");
                    if (th1 != null && img1 != null)
                    {
                        th.Text = String.Format("<th class=\"{0}\" title=\"{1}\" >", th1.Attributes["class"], th1.Attributes["title"]);
                        img.Visible = img1.Visible;
                        img.ImageUrl = img1.ImageUrl;
                    }
                }
            }
        }
        //----- SUMY --------------------------
        int scnt = 0;

        protected string GetSumaCss()
        {
            return scnt++ == 0 ? " suma1" : null;
        }

        protected void Repeater2_DataBinding(object sender, EventArgs e)
        {
            scnt = 0;
        }

        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lb1 = (Label)e.Item.FindControl("lb1");
                Label lb2 = (Label)e.Item.FindControl("lb2");
                if (lb1 != null && lb2 != null)
                {
                    string dataItem = (string)e.Item.DataItem;
                    string p1, p2;//, p3, p4, p5, p6;
                    Tools.GetLineParams(dataItem, out p1, out p2);//, out p3, out p4, out p5, out p6);
                    lb1.Text = p1;
                    lb2.Text = p2;
                }
            }
        }

    }
}