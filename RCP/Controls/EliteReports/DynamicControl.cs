using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

using System.IO;
using System.Text;

using System.Collections.Specialized;

namespace HRRcp.Controls.EliteReports
{
    public static class DynamicControl
    {
        public static List<LinkButton> GetLinkButtonList(Control Container, String Id)
        {
            List<LinkButton> Output = new List<LinkButton>();
            
            foreach(LinkButton Control in Container.Controls)
            {
                if (Control.ID.StartsWith(Id))
                {
                    Output.Add(Control);
                }
            }
            return Output;
        }
        public static String RenderControl(Control control, Control Parent, int index)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Render(control)); 
            return sb.ToString();
        }
        public static String RenderControl(Control control)
        {
            return Render(control);
        }
        public static String Render(Control control)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            control.RenderControl(writer);
            return sb.ToString();
        }
        public static String Render(Control Container, String Id, int Index)
        {
            Control Control = GetControl(Container, Id, Index);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            Control.RenderControl(writer);
            return sb.ToString();

        }
        public static Control GetControl(Control Container, String Id, int Index)
        {
            foreach (Control Control in Container.Controls)
            {
                if (Control.ID.Equals(Id + Index.ToString()))
                {
                    return Control;
                }
            }
            return null;
        }




    }
}
