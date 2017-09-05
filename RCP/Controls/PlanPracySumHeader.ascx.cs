using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PlanPracySumHeader : System.Web.UI.UserControl
    {
        int FMode = PlanPracy.moZmiany;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //----- SUMY --------------------------
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (SumyVisible)
                {
                    HtmlTableCell th = (HtmlTableCell)e.Item.FindControl("thSuma");
                    if (th != null)
                    {
                        if (e.Item.ItemIndex == 0)
                            Tools.AddClass(th, "suma1");
                        if (FMode == PlanPracy.moKartaRoczna)
                            th.RowSpan = 2;
                    }                    
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

        protected void Repeater2_PreRender(object sender, EventArgs e)
        {
            if (SumyVisible)
            {
                string[] sumy = SumHeader.Split('|');
                Repeater2.DataSource = sumy;
                Repeater2.DataBind();       // w Render binduje
            }
        }

        //-------------------
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        /*
        public string[] _HeaderData          // zeby PPline wiedział co to za dzień
        {
            get { return hidHeaderData.Value.Split('|'); }
        }
        */

        public string SumHeader
        {
            set { hidSumHeader.Value = value; }
            get { return hidSumHeader.Value; }
        }

        public bool SumyVisible
        {
            set
            {
                if (!value)
                {
                    SumHeader = null;
                    //Repeater2.DataBind();
                }
            }
            get { return !String.IsNullOrEmpty(SumHeader); }
        }
    }
}