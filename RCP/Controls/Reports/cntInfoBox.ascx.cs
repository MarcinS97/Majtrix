using HRRcp.App_Code;
using HRRcp.Controls.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class cntInfoBox : System.Web.UI.UserControl
    {
        bool adm = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            adm = App.User.HasRight(AppUser.rSuperuser);
            if (!IsPostBack)
            {
                paEdit.Visible = adm;
            }
            else
            {
                GetDataAjax();
            }
        }

        public object Data
        {
            set 
            {
                const string cssNEWLINE = "newline";
                const string cssUNACTIVE = "unactive";

                DataRowView drv = (DataRowView)value;
                int    ibid     = db.getInt(drv["Id"], -1);
                string cssclass = drv["CssClass"].ToString();
                string css      = drv["Css"].ToString();
                string script   = drv["Script"].ToString();
                bool   nl       = db.getBool(drv["NowaLinia"], false);
                bool   active   = db.getBool(drv["Aktywny"], true);
                string cmd      = db.getStr(drv["Command"]);
                string htmlEmpty = drv["HtmlEmpty"].ToString();

                id1 = ibid;
                if (!String.IsNullOrEmpty(cssclass))
                    Tools.AddClass(paInfoBox, cssclass);
                if (!String.IsNullOrEmpty(css))
                    ltCss.Text = String.Format(@"
<style type=""text/css"">
    {0}
</style>", css);
                if (!String.IsNullOrEmpty(script))
                    ltScript.Text = String.Format(@"
<script type=""text/javascript"">function iboxPrepare_{0}(){{
    {1}
}};</script>", ibid, script); // będzie uruchamiany eval'em
                if (nl)
                {
                    Tools.AddClass(paInfoBox, cssNEWLINE);
                    br.Visible = true;
                }
                if (!active)
                    Tools.AddClass(paInfoBox, cssUNACTIVE);

                lbtBox.CommandArgument = cmd;
                if (String.IsNullOrEmpty(htmlEmpty))
                {
                    html1.Text = html2.Text = ltWaiting.Text;
                }
                else
                {
                    html1.Text = html2.Text = htmlEmpty;
                }

                /*
                id2 = db.getInt(drv["bId"], -1);
                phtml1 = drv["Html"].ToString();
                phtml2 = drv["bHtml"].ToString();
                lbtBox.CommandArgument = drv["Command"].ToString();
                */               
                //Tools.ExecOnStart2("sc" + id1.ToString(), String.Format("doClick('{0}');", btGet.ClientID));
                //lbtBox.OnClientClick = String.Format("UpdateInfoBox({0},'{1}','{2}'); return false;", id1, pa1.ClientID, pa2.ClientID);
                if (adm)
                {
                    string id = id1.ToString();
                    btEdit.CommandArgument = id;
                    btNew.CommandArgument = id;
                    btDelete.CommandArgument = id;
                    btPrev.CommandArgument = id;
                    btNext.CommandArgument = id;
                    btUp.CommandArgument = id;
                    btDown.CommandArgument = id;
                    Tools.MakeConfirmButton(btDelete, "Potwierdź usunięcie panelu informacyjnego.", "Uwaga!!!\\nPanel zostanie permanentnie usunięty i nie będzie można go przywrócić.\\nPotwierdź ponownie usunięcie panelu.");
                }
                GetDataAjax();
            }
        }

        private void GetDataAjax()
        {
            cntInfoBoard b = Tools.FindParentControl<cntInfoBoard>(this);
            bool all = b != null ? b.ShowAll : false;
            Tools.ExecOnStart2("upd" + ClientID, String.Format("UpdateInfoBox({0},'{1}','{2}','{3}');", id1, all ? "null" : paInfoBox.ClientID, pa1.ClientID, pa2.ClientID));  // jak all -> null i nie schowa
        }

        protected void lbtBox_Click(object sender, EventArgs e)
        {

        }

        protected void btGet_Click(object sender, EventArgs e)
        {

        }
        //------------------------------------
        private class row
        {
            public int pos;
            public string html;
        }

        private static bool GetTag(string s, string tag, string tagx, out int pos, out int len, out string line)
        {
            line = null;
            pos = s.IndexOf(tag);
            len = 0;
            if (pos >= 0)
            {
                int px = s.IndexOf(tagx);
                if (px > pos)
                {
                    int taglen = tag.Length;
                    int taglenx = tagx.Length;
                    len = px - pos + taglenx;
                    line = s.Substring(pos + taglen, len - taglenx - taglen);   // <tag>asdf</tag> -> asdf
                    return true;
                }
            }
            return false;
        }

        private static string PrepareZnacznikiRepeater(DataSet data, string row)
        {
            string lines = null;
            for (int i = 0; i < db.getCount(data); i++)
            {
                DataRow dr = db.getRow(data, i);
                string line = row;
                Tools.PrepareZnaczniki(ref line, data, i, 0, -1);
                lines += line;  // string builder później ..., #13 jako separator ?
            }
            return lines;
        }

        public static void PrepareZnaczniki(ref string html, DataSet data)
        {
            const string ROW = "{row}";
            const string ROWX = "{/row}";
            const int max = 100;  // max ilość tagów

            int pos, len;
            string line;
            int cnt = 0;
            while (GetTag(html, ROW, ROWX, out pos, out len, out line))
            {
                string lines = PrepareZnacznikiRepeater(data, line);
                html = html.Remove(pos, len);
                if (!String.IsNullOrEmpty(lines))
                    html = html.Insert(pos, lines);
                cnt++;
                if (cnt > max) break;  // zabezpieczenie
            } 
            Tools.PrepareZnaczniki(ref html, data);  //pozostałe znacznikia

            /*
            List<row> rows = new List<row>();
            for (int i = rows.Count - 1; i > 0; i--)
            {
                row r = rows[i];
                string lines = null;
                for(int j = 0; i < db.getCount(data); j++)
                {
                    DataRow dr = db.getRow(data, j);
                    string line = r.html;
                    Tools.PrepareZnaczniki(ref line, data, j, 0, -1);
                    lines += line;  // string builder później ..., #13 jako separator ?
                }
                html.Insert(r.pos, lines);
            }
            */
        }
        //------------------------------------
        public string phtml1
        {
            set { ViewState["pht1"] = value; }
            get { return Tools.GetStr(ViewState["pht1"]); }
        }

        public string phtml2
        {
            set { ViewState["pht2"] = value; }
            get { return Tools.GetStr(ViewState["pht2"]); }
        }

        public int id1
        {
            set { ViewState["id1"] = value; }
            get { return Tools.GetInt(ViewState["id1"], -1); }
        }

        public int id2
        {
            set { ViewState["id2"] = value; }
            get { return Tools.GetInt(ViewState["id2"], -1); }
        }

        //------------------------------------
        public void Hide()
        {
            pa1.Style["display"] = "none";
            pa2.Style["display"] = "none";
        }

        public void Show()
        {
            pa1.Style["display"] = null;   // uzależnić od klikniętego
            //pa2.Style["display"] = null;
        }

        //--------------------       
        protected void btEdit_Click(object sender, EventArgs e)
        {
        }

        protected void btNew_Click(object sender, EventArgs e)
        {
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
        }
    }
}