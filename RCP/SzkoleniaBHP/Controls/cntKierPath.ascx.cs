using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

// ---- niewykorzystywana, i niedokończona


namespace HRRcp.SzkoleniaBHP.Controls
{
    public partial class cntKierPath : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //-----
        /*       
        private static Stack<string> Path
        {
            set { ViewState["zklist"] = value; }
            get
            {
                object o = ViewState["zklist"];
                if (o == null)
                {
                    Stack<string> z = new Stack<string>();
                    Path = z;
                    return z;
                }
                else
                    return (Stack<string>)o;
            }
        }

        private static string Current
        {
            get { return Path.Count == 0 ? null : Path.First(); }
        }

        public static bool IsKierUpVisible
        {
            get { return Path.Count > 0; }
        }

        private static void Show()
        {
            string kid, kn;
            Tools.GetLineParams(Current, out kid, out kn);
            Show(kid, kn);
        }

        private static void Show(string kid, string kn)
        {
            ImageButton ibt = lvUprawnienia.FindControl("ibtUp") as ImageButton;
            if (ibt != null)
            {
                LinkButton lbt = lvUprawnienia.FindControl("lbtUp") as LinkButton;  // musi być
                if (!String.IsNullOrEmpty(kid))
                {
                    ibt.CommandArgument = kid;
                    //ibt.ToolTip = String.Format("Cofnij: {0} ", kn);
                    lbt.Text = String.Format("Cofnij: {0} ", kn);
                    ibt.Visible = true;
                    lbt.Visible = true;
                }
                else
                {
                    ibt.CommandArgument = null;
                    ibt.ToolTip = null;
                    ibt.Visible = false;
                }
            }
        }

        //private static void Show(string kid, string kn)
        //{
        //    ImageButton ibt = lvUprawnienia.FindControl("ibtUp") as ImageButton;
        //    if (ibt != null)
        //    {
        //        LinkButton lbt = lvUprawnienia.FindControl("lbtUp") as LinkButton;  // musi być
        //        if (!String.IsNullOrEmpty(kid))
        //        {
        //            ibt.CommandArgument = kid;
        //            //ibt.ToolTip = String.Format("Cofnij: {0} ", kn);
        //            lbt.Text = String.Format("Cofnij: {0} ", kn);
        //            ibt.Visible = true;
        //            lbt.Visible = true;
        //        }
        //        else
        //        {
        //            ibt.CommandArgument = null;
        //            ibt.ToolTip = null;
        //            ibt.Visible = false;
        //        }
        //    }
        //}

        private static void Push(string kid, string kn)
        {
            //  samo sie wyswietli w lvDataBound
            //if (ZoomKierCurrent == null) 
            //{
            //    ZoomKierShow(kid, kn);
            //}
            //
            Path.Push(Tools.SetLineParams(kid, kn));
            Path = Path;
        }

        private static bool Pop(out string kid, out string kn)
        {
            string p = Current;
            if (Path.Count > 0) Path.Pop();
            Path = Path;
            if (String.IsNullOrEmpty(p))
            {
                kid = null;
                kn = null;
                //ZoomKierShow(null, null);
                return false;
            }
            else
            {
                Tools.GetLineParams(p, out kid, out kn);
                return true;
            }
        }

        private static void ZoomKierClear()
        {
            Path.Clear();
            Path = Path;
            Show();
        }
        */
    }
}