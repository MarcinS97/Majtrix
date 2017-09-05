using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls;

namespace HRRcp.App_Code
{
    public class L
    {
        public const string lngPL = "PL";
        public const string lngUS = "US";

        const string defLng = !Lic.lang ? lngPL : lngPL;    // jak bez mozliwosci zaminy jezyka to PL
        //const string defLng = !Lic.lang ? lngPL : lngUS;  
        //const string defLng = lngUS;

        private static string currLng = Lang;   // jak nie ma breakpoint tu to się moze nie wykonać ... 
        private static Dictionary<string, string> _T = null;

        public static void InitLang(string lng)
        {
            Lang = lng;
        }

        public static void SetLang(string lng)  // wywołać w Default.aspx !!!
        {
            Lang = lng;
            LoadLng(lng);
        }
        //----------------------------------
        private static void LoadLng(string lng)
        {
            _T = new Dictionary<string, string>();
            string sql = "select Msg, Trans from Lang where Lang = " + db.strParam(lng);
            using (SqlCommand cmd = new SqlCommand(sql, db.con))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _T[(string)reader["Msg"]] = (string)reader["Trans"];
                    }
                }
            }
            T = _T;   // do sesji
        }
        //----------------------------------
        private static bool tr(string msg, out string trans)
        {
            /*----- NA RAZIE BEZ FAKTYCZNEJ TRANSLACJI -----*/
            trans = msg;
            return true;
            /*----------------------------------------------*/






            string ret;
            try
            {
                ret = T[msg];
            }
            catch (Exception ex)
            {
                bool ok = db.insert("Lang", 0, "Lang,Msg,Trans", db.strParam(currLng), db.strParam(db.sqlPut(msg)), db.strParam(""));
                /*
                bool ok = true;
                if (currLng == "PL")
                {
                    ok = db.insert("Lang", 0, "Lang,Msg,Trans", db.strParam("PL"), db.strParam(db.sqlPut(msg)), db.strParam(""));
                    ok = db.insert("Lang", 0, "Lang,Msg,Trans", db.strParam("US"), db.strParam(db.sqlPut(msg)), db.strParam(""));
                }
                 */
                trans = msg;
                if (ok)     // nie było 
                {
                    _T[msg] = null;   
                    T = _T;
                }
                return false;
            }

            if (String.IsNullOrEmpty(ret))
                trans = msg;
            else
                trans = ret;
            return true;
        }

        public static string p(string msg)   // lub id
        {
            string trans;
            tr(msg, out trans);
            return trans;
        }

        public static string p(string msg, params object[] par)   // lub id
        {
            string trans;
            tr(msg, out trans);
            return String.Format(trans, par);
        }

        public static string id(string id, string msg)
        {
            string trans;
            bool exists = tr(id, out trans);
            if (String.IsNullOrEmpty(trans))
                if (String.IsNullOrEmpty(msg))
                    return id;
                else
                    return msg;
            else
                return trans;
        }

        public static string id(string id, string msg, params object[] par)
        {
            string trans;
            bool exists = tr(id, out trans);
            if (String.IsNullOrEmpty(trans))
                if (String.IsNullOrEmpty(msg))
                    return id;
                else
                    return String.Format(msg, par);
            else
                return String.Format(trans, par);
        }
        //------------------------
        public static void p(Label lb)
        {
            lb.Text = p(lb.Text);
            if (!String.IsNullOrEmpty(lb.ToolTip)) lb.ToolTip = L.p(lb.ToolTip);
        }

        public static void p(Button lb)
        {
            lb.Text = p(lb.Text);
            if (!String.IsNullOrEmpty(lb.ToolTip)) lb.ToolTip = L.p(lb.ToolTip);
        }

        public static void p(LinkButton lb)
        {
            lb.Text = p(lb.Text);
            if (!String.IsNullOrEmpty(lb.ToolTip)) lb.ToolTip = L.p(lb.ToolTip);
        }

        public static void p(RequiredFieldValidator lb)
        {
            lb.ErrorMessage = p(lb.ErrorMessage);
        }

        public static void p(Menu menu)
        {
            foreach (MenuItem mi in menu.Items)
            {
                mi.Text = p(mi.Text);
            }
        }

        public static void p(MenuItem mitem)
        {
            mitem.Text = p(mitem.Text);
        }

        public static void p(Literal lb)
        {
            lb.Text = p(lb.Text);
        }

        public static void p(CheckBox lb)
        {
            lb.Text = p(lb.Text);
            if (!String.IsNullOrEmpty(lb.ToolTip)) lb.ToolTip = L.p(lb.ToolTip);
        }

        public static void p(HiddenField lb)
        {
            lb.Value = p(lb.Value);
        }

        public static void p(DropDownList ddl)
        {
            foreach (ListItem item in ddl.Items)
                item.Text = L.p(item.Text);
        }
         
        public static void p(DataPager dp)
        {
            foreach (DataPagerField field in dp.Fields)
                if (field is NextPreviousPagerField) 
                {
                    NextPreviousPagerField f = (NextPreviousPagerField)field;
                    /*
                    f.FirstPageText = L.p(f.FirstPageText);
                    f.LastPageText = L.p(f.LastPageText);
                    f.PreviousPageText = L.p(f.PreviousPageText);
                    f.NextPageText = L.p(f.NextPageText);
                    */
                    /*
                    const string Pierwsza = "Pierwsza";
                    if (f.FirstPageText != L.p(Pierwsza))
                    {
                        f.FirstPageText = L.p(Pierwsza);
                        f.LastPageText = L.p("Ostatnia");
                        f.PreviousPageText = L.p("Poprzednia");
                        f.NextPageText = L.p("Następna");
                    }
                     */
                    
                    if (f.ShowFirstPageButton)      f.FirstPageText     = L.p("Pierwsza");
                    if (f.ShowLastPageButton)       f.LastPageText      = L.p("Ostatnia");
                    if (f.ShowPreviousPageButton)   f.PreviousPageText  = L.p("Poprzednia");
                    if (f.ShowNextPageButton)       f.NextPageText      = L.p("Następna");
                }
        }

        public static void p(PageTitle pt)
        {
            pt.Title = L.p(pt.Title);
        }

        //--------
        public static void phint(WebControl cnt)
        {
            cnt.ToolTip = p(cnt.ToolTip);
        }


        /*
        public static void Translate(Control c)
        {
            if (c is Label)         t(c as Label);
            else if (c is Button)   t(c as Button);
            else if (c is LinkButton) t(c as LinkButton);
            else if (c is Menu)     t(c as Menu);
            else
            {
                string typ = c.GetType().ToString();
            }
            foreach (Control cc in c.Controls)
                Translate(cc);
        }
         */ 
        //------------------------
        public static Control p(Control item, string cntName)
        {
            Control c = item.FindControl(cntName);
            if (c != null)
                if      (c is Label)        p((Label)c);
                else if (c is LinkButton)   p((LinkButton)c);
                else if (c is Button)       p((Button)c);
                else if (c is Literal)      p((Literal)c);
                else if (c is CheckBox)     p((CheckBox)c);
                else if (c is DropDownList) p((DropDownList)c);
                else if (c is DataPager)    p((DataPager)c);
            return c;
        }

        public static void p(Control item, string cntName, int start, int last)
        {
            for (int i = start; i <= last; i++)
                L.p(item, "LinkButton" + i.ToString());
        }
        //------------------------
        public static Dictionary<string, string> T
        {
            set { HttpContext.Current.Session["__TR"] = value; }
            get 
            {
                if (_T == null)
                {
                    object o = HttpContext.Current.Session["__TR"];
                    if (o == null)
                        LoadLng(Lang);                     
                }
                return _T; 
            }
        }

        public static string Lang
        {
            set 
            {
                string v = Lic.lang ? value : defLng;     // jak nie ma prawa to niezaleznie co, zawsze ustawi domyslne
                HttpContext.Current.Session["LNG"] = v;
                currLng = v;
            }
            get
            {
                if (String.IsNullOrEmpty(currLng))
                {
                    object o = HttpContext.Current.Session["LNG"];
                    if (o == null || !Lic.lang && o.ToString() != defLng)  // jak nie ma licencji to m a zwasze wymusic ustawienie w sesji bo inna app moze ustawic
                    {
                        HttpContext.Current.Session["LNG"] = defLng;
                        currLng = defLng;
                    }
                    else
                    {
                        currLng = o.ToString();
                    }
                }
                return currLng;
            }
        }


        /*
        public static string LNG
        {
            set 
            { 
                HttpContext.Current.Session["__LNG"] = value;
                currLng = value;
            }
            get { return currLng; }
        }
         */ 
    }
}



/*
 
 
 */