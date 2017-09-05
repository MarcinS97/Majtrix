using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;
using System.Text;

namespace HRRcp.Controls.Portal
{
    public partial class cntSqlMenu3 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderMenu();
            }
        }

        void RenderMenu()
        {
            DataTable dt = db.Select.TableCon(dsData, Grupa);
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                sb.Append("<ul class='sql-menu'>");
                sb.Append(RenderNode(dt, null));
                sb.Append("</ul>");
            }
            litMenu.Text = sb.ToString();
        }

        string RenderNode(DataTable dt, int? id)
        {
            DataRow[] rows = dt.Select(String.Format(id == null ? "ParentId is NULL" : String.Format("ParentId = {0}", id)));
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in rows)
            {
                int? pid = (int?)dr["Id"];
                string node = HasChildren(dt, pid) ? "<ul>{0}</ul>" : "{0}";
                string cmd = db.getValue(dr, "Command");
                string link = "";
                string pm1 = db.getValue(dr, "Par1");
                Tools.IsUrl(cmd, out link);
                link = !String.IsNullOrEmpty(link) ? link : "javascript://";
                if (!String.IsNullOrEmpty(pm1)) link += "?p1=" + pm1;

                sb.Append("<li>");
                sb.AppendFormat("<a class='toggler' href='{1}'>{0}</a>", dr["MenuText"], ResolveUrl(link));
                sb.AppendFormat(node, RenderNode(dt, pid));
                sb.Append("</li>");
            }
            return sb.ToString();//String.Format("<li><a href=''>{0}</a></li>", dr["MenuText"]);
        }

        bool HasChildren(DataTable dt, int? id)
        {
            DataRow[] rows = dt.Select(String.Format(id == null ? "ParentId is NULL" : String.Format("ParentId = {0}", id)));
            return rows.Length > 0;
        }

        public String Grupa
        {
            get { return hidGrupa.Value; }
            set { hidGrupa.Value = value; }
        }
    }
}