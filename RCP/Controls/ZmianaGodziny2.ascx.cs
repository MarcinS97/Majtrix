using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{

    /*
1. Instantiate
2. Initialize
3. TrackViewState
4. LoadViewState (postback)
5. Load postback data (postback, IPostBackDatahandler.LoadPostdata)
6. Load
7. Load postback data for dynamical controls added on Page_Load (postback)
8. Raise Changed Events (postback,
IPostBackDatahandler.RaisePostDataChanged)
9. Raise postback event (postback, IPostBackEventHandler.RaisePostBackEvent)
10.PreRender
11. SaveViewState
12. Render
13. Unload
14. Dispose

 */

    public partial class ZmianaGodziny2 : System.Web.UI.UserControl
    {
        bool FEditable;

        protected void Page_Load(object sender, EventArgs e)
        {
            Render();  // dla PostBack to jest po to zeby się odtworzyły wartości !
        }

        /*
        private string DynClientId(string id)
        {
            return btAdd.ClientID.Substring(0, btAdd.ClientID.Length - btAdd.ID.Length) + id;
        }

        private void HandlePostback()
        {
            int p;
            string pc = Tools.GetPostBackControlName(true);
            if (pc == btAdd.ClientID)
            {
                //Stawki = GetStawki();
                if (String.IsNullOrEmpty(Stawki)) Stawki = "100";
                else
                {
                    p = Stawki.LastIndexOf(",");
                    if (p == -1) Stawki += "," + Stawki;
                    else         Stawki += "," + Stawki.Substring(p + 1);
                }
            }
            else if (pc == DynClientId("btDel"))
            {
                //Stawki = GetStawki();
                p = Stawki.LastIndexOf(",");
                if (p == -1) Stawki = "";
                else         Stawki = Stawki.Substring(0, p);
            }
        }
        */

        protected void btAdd_Click(object sender, EventArgs e)
        {
            hOvertimes = GetStawki();   // uaktualniam zmienną
            int p;
            if (String.IsNullOrEmpty(hOvertimes)) hOvertimes = "100";
            else
            {
                p = hOvertimes.LastIndexOf(",");
                if (p == -1) hOvertimes += "," + hOvertimes;
                else hOvertimes += "," + hOvertimes.Substring(p + 1);
            }
            Render();
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            hOvertimes = GetStawki();
            int p;
            p = hOvertimes.LastIndexOf(",");
            if (p == -1) hOvertimes = "";
            else hOvertimes = hOvertimes.Substring(0, p);
            Render();
        }

        protected void ddlStawkaLast_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label lb = (Label)FindControl("lbStawkaNext");
            if (lb != null)
                lb.Text = ((DropDownList)sender).SelectedItem.Text;
        }
        //-------------------
        private bool FindStawka(out string stawka, ref int index)
        {
            ControlCollection c = Controls;
            DropDownList ddl = (DropDownList)FindControl("ddlStawka" + index.ToString());
            if (ddl != null)
            {
                stawka = ddl.SelectedValue;
                index++;
                return true;
            }
            else
            {
                stawka = null;
                return false;
            }
        }

        private string GetStawki()
        {
            int i = 0;
            string stawka;
            string ret = null;
            while (FindStawka(out stawka, ref i))
            {
                if (i == 1) ret = stawka;  // znalazł pierwszy
                else ret += "," + stawka;
            }
            return ret;
        }

        private void Render()
        {
            phHours.Controls.Clear();

            Button btAdd = new Button();
            btAdd.Text = "Dodaj";
            btAdd.ID = "btAdd";
            btAdd.CssClass = "control";
            btAdd.Click += new EventHandler(btAdd_Click);

            if (String.IsNullOrEmpty(hOvertimes))
            {
                Tools.AddLiteral(phHours, "<tr class=\"nadgodziny_nodata\">");
                Tools.AddLiteral(phHours, "<td class=\"col1\"></td>");
                /*
                Tools.AddLiteral(phHours, "<td colspan=\"2\">Brak nadgodzin</td>");
                if (Editable) Tools.AddControl(phHours, "<td class=\"col4\">", btAdd, "</td>");
                else          Tools.AddLiteral(phHours, "<td class=\"col4\"></td>");
                */
                Tools.AddLiteral(phHours, "<td class=\"col2\">Brak nadgodzin</td>");
                if (Editable) Tools.AddControl(phHours, "<td class=\"col3\">", btAdd, "</td>");
                else          Tools.AddLiteral(phHours, "<td class=\"col3\"></td>");
                Tools.AddLiteral(phHours, "<td class=\"col4\"></td>");

                Tools.AddLiteral(phHours, "</tr>");
            }
            else
            {
                Button btDel = new Button();
                btDel.Text = "Usuń";
                btDel.ID = "btDel";
                btDel.CssClass = "control";
                btDel.Click += new EventHandler(btDelete_Click);

                string[] hh = hOvertimes.Split(',');
                int cnt = hh.Count();
                for (int i = 0; i < cnt; i++)
                {
                    bool first = i == 0;
                    bool last = i == cnt - 1;
                    bool one = cnt == 1;
                    string ncss = "nadgodziny";
                    if (one) ncss += " nadgodziny_one";
                    else if (first) ncss += " nadgodziny_first";
                    Tools.AddLiteral(phHours, "<tr class=\"" + ncss + "\">");

                    Tools.AddLiteral(phHours, "<td class=\"col1\"></td>");

                    if (last) Tools.AddLiteral(phHours, "<td class=\"col2\">+" + (i + 1).ToString() + " i kolejne</td>");
                    else      Tools.AddLiteral(phHours, "<td class=\"col2\">+" + (i + 1).ToString() + "</td>");

                    if (Editable)
                    {
                        DropDownList ddl = new DropDownList();
                        ddl.ID = "ddlStawka" + i.ToString();
                        App.FillStawki(ddl, hh[i]);
                        Tools.AddControl(phHours, "<td class=\"col3\">", ddl, "</td>");

                        if (one)
                        {
                            Tools.AddControl(phHours, "<td class=\"col4\">", btAdd, "<br />");
                            Tools.AddControl(phHours, null, btDel, "</td>");
                        }
                        else
                        {
                            if (first)     Tools.AddControl(phHours, "<td class=\"col4\">", btAdd, "</td>");
                            else if (last) Tools.AddControl(phHours, "<td class=\"col4\">", btDel, "</td>");
                            else           Tools.AddLiteral(phHours, "<td class=\"col4\"></td>");
                        }
                    }
                    else
                    {
                        Tools.AddLiteral(phHours, "<td class=\"col3\">" + hh[i] + "%" + "</td>");
                        Tools.AddLiteral(phHours, "<td class=\"col4\"></td>");
                    }
                    
                    Tools.AddLiteral(phHours, "</tr>");
                }
            }
        }
        //-------------------
        /*
        public bool Editable
        {
            get { return hidMode.Value == "E"; }
            set { hidMode.Value = value ? "E" : "Q"; }
        }
         */ 
        public bool Editable
        {
            get { return FEditable; }
            set { FEditable = value; }
        }

        public string Overtimes // do ustawiania i pobierania do zapisu
        {
            get
            {
                if (Editable)
                    hidOvertimes.Value = GetStawki();
                return hidOvertimes.Value; 
            }
            set 
            {
                if (String.IsNullOrEmpty(hidOvertimes.Value))
                {
                    hidOvertimes.Value = value;
                    Render();
                }
            }
        }

        private string hOvertimes   // to samo co Overtimes tylko bez przeliczeń
        {
            get { return hidOvertimes.Value; }
            set { hidOvertimes.Value = value; }
        }

        public string x_ZmianaDo
        {
            get { return x_hidZmianDo.Value; }
            set { x_hidZmianDo.Value = value; }
        }
    }
}