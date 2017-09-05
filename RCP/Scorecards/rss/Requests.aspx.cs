using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.rss
{
    public partial class Requests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*String RequestsQuery = @"
select
'<?xml version=""1.0"" encoding=""UTF-8""?>
<rss xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:atom=""http://www.w3.org/2005/Atom"" version=""2.0"">
    <channel>
        <title>Scorecards</title>
        <link>https://gnu.org</link>
        <description>Requests</description>
        <atom:link href=""http://localhost:50675/Scorecards/rss/requests.aspx"" rel=""self"" type=""application/rss+xml""/>
'
+
(select dbo.cat(
'        <item>
            <title><![CDATA[' + w.Nazwa + ']]></title>
            <link>https://gnu.org</link>
            <guid>' + CONVERT(varchar, w.Id) + '</guid>
            <comments>test comment</comments>
            <pubDate>' + CONVERT(varchar, w.DataUtworzenia, 20) + '</pubDate>
            <description>desc</description>
        </item>', '
', 0) from scWnioski w)
+
'
	</channel>
</rss>'

";*/
            String Header = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<rss xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:atom=""http://www.w3.org/2005/Atom"" version=""2.0"">
    <channel>
        <title>Scorecards</title>
        <link>https://gnu.org</link>
        <description>Requests</description>
        <atom:link href=""http://localhost:50675/Scorecards/rss/requests.aspx"" rel=""self"" type=""application/rss+xml""/>
";
            String Footer = @"
	</channel>
</rss>
";
            /*String ContentQuery = @"
select ISNULL(w.Nazwa, 'Empty!!!') title, 'https://gnu.org' link, CONVERT(varchar, w.Id) guid, 'aoe' comments, CONVERT(varchar, w.DataUtworzenia, 20) pubDate, 'aoe' description from scWnioski w
";*/

            StringBuilder Content = new StringBuilder();

            /* DataTable dt = db.Select.Table(db.Connect(ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString), 0, ContentQuery); */

            DataTable dt = db.Select.TableCon(dsRequests, App.User.Id);

            List<String> names = new List<String>();

            foreach (DataColumn dc in dt.Columns)
            {
                names.Add(dc.ColumnName);
            }

            foreach (DataRow rw in dt.Rows)
            {
                Content.Append(@"
        <item>");
                for (int i = 0; i < names.Count; ++i)
                {
                    Content.AppendFormat(@"
            <{0}>{1}</{0}>", names[i], /*((names[i] == "title") ? "<![CDATA[" : "") +*/ rw[i].ToString() /*+ ((names[i] == "title") ? "]]>" : "")*/);
                }
                Content.Append(@"
        </item>");
            }

            /* Response.Write(db.Select.Table(db.Connect(ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString), 0, RequestsQuery).Rows[0][0].ToString()); */
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/rss+xml";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            //HttpContext.Current.Response.Charset = "1250";//"unicode";

            /* HttpContext.Current.Response.Write(db.Select.Table(db.Connect(ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString), 0, RequestsQuery).Rows[0][0].ToString()); */
            HttpContext.Current.Response.Write(Header + Content.ToString() + Footer);
            HttpContext.Current.Response.End();
        }
    }
}
