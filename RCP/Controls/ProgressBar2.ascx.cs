using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace HRRcp
{
    public partial class ProgressBar2 : System.Web.UI.UserControl
    {
        private const int bitmapOfs = -400;
        private int width;
        private int pcent;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //--------------------------------------------------

        private void setProgress(HtmlGenericControl div, int p)
        {
            if (p < 0) pcent = 0;
            else if (p > 100) pcent = 100;
            else pcent = p;
            int x = bitmapOfs + width * p / 100;
            div.Style["background-position"] = x.ToString() + "px top";
        }

        //--------------------------------------------------

        public int Width
        {
            set 
            { 
                width = value;
                pbar.Style["width"] = width.ToString() + "px";
            }
            get { return width; }
        }

        public int Pcent
        {
            set { setProgress(pbar, value); }
            get { return pcent; }
        }

        /*
        public double Pcent
        {
            set { setProgress(ProgressBar, Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero))); }
            get { return pcent; }
        }
         */
    }
}