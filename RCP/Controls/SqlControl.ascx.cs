using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class SqlControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btExec_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = Base.Connect();                
                string sql = tbSQL.Text.Trim();
                Log.Info(Log.SQL, "Exec script", sql);
                if (sql.Substring(0, 6).ToLower() == "select")
                {
                    DataSet ds = Base.getDataSet(con, tbSQL.Text);
                    ltResult.Text = DataSetAsTable(ds);
                }
                else
                {
                    int cnt = Base.execSQLcnt(con, sql);
                    ltResult.Text = "Przetworzono wierszy: " + cnt.ToString();
                }
                //SetSqlInfoCallback(con, SqlInfoCallback);
                Base.Disconnect(con);
            }
            catch (Exception ex)
            {
                ltResult.Text = ex.Message;
            }
        }
        //----------------------------
        /*
        public void SetSqlInfoCallback(SqlConnection con, SqlInfoMessageEventHandler func)
        {
            con.InfoMessage += new SqlInfoMessageEventHandler(func);
            con.FireInfoMessageEventOnUserErrors = true;
        }

        public void SqlInfoCallback(object sender, SqlInfoMessageEventArgs e)
        {
            ltMessage.Text = e.Message;
        }
        */
        //----------------------------
        public string tag(string t, int colspan, int col, string classSuffix)     // t=td/th ...
        {       // <td colspan="2" class="col1 lastcol">
            string cs = colspan > 1 ? " colspan=\"" + colspan.ToString() + "\"" : "";
            return "<" + t + cs + " class=\"col" + col.ToString() + classSuffix + "\">";
        }
        public string tag(string t, int colspan, int col, int lastcol, int fullcol)
        {
            string lc = null;
            if (lastcol == col || (colspan > 1 && col + colspan - 1 >= lastcol))
                lc = " lastcol";
            if (fullcol == col)
                lc += " fullwidth";
            return tag(t, colspan, col, lc);
        }
        //----- <th> -----
        public string th(int colspan, int col, int lastcol, int fullcol)
        {
            return tag("th", colspan, col, lastcol, fullcol);
        }
        public string th(int colspan, int col, string text, int lastcol, int fullcol)
        {
            return th(colspan, col, lastcol, fullcol) + text + "</th>";
        }
        //----- <td> -----
        public string td(int colspan, int col, int lastcol, int fullcol)
        {
            return tag("td", colspan, col, lastcol, fullcol);
        }
        public string td(int colspan, int col, string text, int lastcol, int fullcol)
        {
            return td(colspan, col, lastcol, fullcol) + text + "</td>";
        }
        //----- <table> -----
        //----- <tr> -----
        //----- <th> -----
        //----- <td> -----
        //----------------
        public static string DataSetAsTable(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                StringWriter sw = new StringWriter();
                int cnt = ds.Tables[0].Columns.Count;
                sw.Write("<table>");
                sw.Write("<tr>");
                for (int i = 0; i < cnt; i++)
                {
                    sw.Write("<td>");
                    sw.Write(Tools.CtrlToText(ds.Tables[0].Columns[i].ToString()));
                    sw.Write("</td>");
                }
                sw.Write("</tr>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sw.Write("<tr>");
                    for (int i = 0; i < cnt; i++)
                    {
                        sw.Write("<td>");
                        sw.Write(Tools.CtrlToText(dr[i].ToString()));
                        sw.Write("</td>");
                    }
                    sw.Write("<tr>");
                }
                sw.Write("</table>");
                return sw.ToString();
            }
            else return "brak danych";
        }
    }
}