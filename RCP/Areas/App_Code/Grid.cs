using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;

/*
formaty dla liczb:
http://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx

 * [:NN2] -> 123 456,12
 * [:NF2] -> 123456,12
 * [:NX4] -> 00FF  
 * [:Nx4] -> 00ff
 * [:NP1] -> 55,2 %
 * [:ND5] -> 00123
    
*/

namespace HRRcp.App_Code
{
    public class Grid
    {
        public const bool cryptParams = true;
        public const string key  = "148169751135946";
        public const string salt = "96654182751586";   // na razie, potem dac losowe i zapisywane w sesji

        public const string cssSumRow   = "sum";
        public const string cssAltRow   = "alt";
        public const string cssNormRow  = "norm";
        public const string cssHidden   = "hidden";

        public const string cssSort     = "sortmarker";
        public const string cssSortAsc  = "sortasc";
        public const string cssSortDesc = "sortdesc";
        public const string sortUP      = "▲";
        public const string sortDN      = "▼";

        public const string tHide       = "-";      // [:-] 
        public const string tDate       = "D";      // [:D;css] css opcjonalny
        public const string _tDateTime   = "DT";     // [:DT;css]
        public const string tNum        = "N";      // [:N;css] [:NT] - separator tysiąca [N0.00S] - precyzja 2 miejsca po przecinku, [:N2] -<<< moze kiedyś ...
        public const string tSum        = "S";      // [:NS;css] pod tabelą - sumuje tylko widoczne !!!
        public const string tCount      = "C";      // [:C;css] pod tabelą - ilość 
        public const string tSelect     = "CB";     // [:CB;css] checkbox do selekcji
        public const string tSelectAll  = "H";      // [:CBH;css] checkbox do selekcji z select all w header

        public const string tLp         = "Lp";      // [:Lp;css] liczba porządkowa, startuje od podanej 1 [Lp:Lp]

        public const string tRowClass   = "class";  // [:class] dodatkowa kolumna niewidoczna, dokłada text zwrócony w polu do class tr
        public const string tRowClick   = "click";  // [:click|cmd:komenda @par|hint] dodatkowa kolumna niewidoczna, dokłada tr.td.onclick -> cmd

        private const string nbsp   = "&nbsp;"; //


        // [nazwa_kolumny:format,css|link/javascript:/cmd:|hint]   - max 128 znaków !!!    
        /*
        [..|link.aspx p1 p2|hint]
        [..|javascript], [..|js]
        [..|cmd:]        
        */

        const string parGridId   = "__GridId";
        const string lbCount     = "{0}Count";
        const string btCmd       = "{0}Cmd";
        const string hidCmdPar   = "{0}CmdPar";
        const string hidSelected = "{0}Selected";   //Podział Ludzi - checkbox'y
        const string cbSelectAll = "{0}SelectAll"; 

        public static void Prepare(GridView grid)
        {
            grid.RowDataBound += new GridViewRowEventHandler(gv_RowDataBound);
            grid.PreRender += new EventHandler(gv_PreRender);
            grid.DataBound += new EventHandler(gv_DataBound);
            grid.RowCreated += new GridViewRowEventHandler(gv_RowCreated);    //sortowanie
            if (!String.IsNullOrEmpty(grid.DataSourceID))
            {
                Control ds = grid.Parent.FindControl(grid.DataSourceID);
                if (ds != null && ds is SqlDataSource)
                {
                    ((SqlDataSource)ds).SelectParameters.Add(new Parameter(parGridId, DbType.String, grid.ID));
                    ((SqlDataSource)ds).Selected += new SqlDataSourceStatusEventHandler(gv_Selected);
                }
            }
            //grid.EmptyDataText = L.p("Brak danych");
            if (String.IsNullOrEmpty(grid.EmptyDataText))
                grid.EmptyDataText = "Brak danych";
            grid.RowStyle.CssClass          = "it";
            grid.SelectedRowStyle.CssClass  = "sit";
            grid.EditRowStyle.CssClass      = "eit";
            grid.EmptyDataRowStyle.CssClass = "edt";
            grid.GridLines = GridLines.None;

            grid.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            grid.PagerSettings.FirstPageText    = "|◄";
            grid.PagerSettings.LastPageText     = "►|";
            grid.PagerSettings.NextPageText     = "►";
            grid.PagerSettings.PreviousPageText = "◄";
            grid.PagerStyle.CssClass = "pager";
        }

        public static void Prepare(GridView grid, string css)
        {
            Prepare(grid, css, true, 20, true);
        }
        //----- Prepare2 - do cntReport2 -----
        public static void Prepare2(GridView grid, string css, bool paging, int pgSize, bool sorting, bool count)
        {
            grid.RowDataBound += new GridViewRowEventHandler(gv_RowDataBound2);
            grid.PreRender += new EventHandler(gv_PreRender2);
            grid.DataBound += new EventHandler(gv_DataBound);
            grid.RowCreated += new GridViewRowEventHandler(gv_RowCreated);    //sortowanie

            //grid.EmptyDataText = L.p("Brak danych");
            if (String.IsNullOrEmpty(grid.EmptyDataText))
                grid.EmptyDataText = "Brak danych";
            if (!String.IsNullOrEmpty(css))
                if (String.IsNullOrEmpty(grid.CssClass))
                    grid.CssClass = css;
                else
                    grid.CssClass += " " + css;
            grid.AllowSorting = sorting;
            grid.AllowPaging = paging;
            if (paging) grid.PageSize = pgSize;
            grid.RowStyle.CssClass = "it";
            grid.SelectedRowStyle.CssClass = "sit";
            grid.EditRowStyle.CssClass = "eit";
            grid.EmptyDataRowStyle.CssClass = "edt";
            grid.GridLines = GridLines.None;

            grid.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            grid.PagerSettings.FirstPageText = "|◄";
            grid.PagerSettings.LastPageText = "►|";
            grid.PagerSettings.NextPageText = "►";
            grid.PagerSettings.PreviousPageText = "◄";
            grid.PagerStyle.CssClass = "pager";

            //grid.Attributes["name"] = "report";  jest dodawany w OnPrerender

            /*
            if (count)
            {
                Label lb = grid.Parent.FindControl("lbCount") as Label;
                if (lb != null)
                    grid.DataSource.Selected += new EventHandler() ...
                }
            }
            */
        }

        public static void Prepare2(GridView grid, string css, bool paging, int pgSize, bool sorting)
        {
            Prepare2(grid, css, paging, pgSize, sorting, false);
        }
        //-----
        public static void Prepare(GridView grid, string css, bool paging, int pgSize, bool sorting, string thFmt, string tdFmt)
        {
            Prepare(grid, css, paging, pgSize, sorting);
            grid.ToolTip = Tools.SetLineParams(thFmt, tdFmt);     //<<<<< testy, docelowo nie w ToolTip !!!, w gv_Prerender jest = null !!!
        }

        public static void Prepare(GridView grid, string css, bool paging, int pgSize, bool sorting)
        {
            grid.RowDataBound += new GridViewRowEventHandler(gv_RowDataBound);
            grid.PreRender += new EventHandler(gv_PreRender);   //<<<<<<<<<<<< 20150210 było zaremowane !!!! dlaczego ????
            grid.DataBound += new EventHandler(gv_DataBound);
            grid.RowCreated += new GridViewRowEventHandler(gv_RowCreated);    //sortowanie

            //grid.EmptyDataText = L.p("Brak danych");
            if (String.IsNullOrEmpty(grid.EmptyDataText))
                grid.EmptyDataText = "Brak danych";
            if (!String.IsNullOrEmpty(css))
                if (String.IsNullOrEmpty(grid.CssClass))
                    grid.CssClass = css;
                else
                    grid.CssClass += " " + css;
            grid.AllowSorting = sorting;
            grid.AllowPaging = paging;
            if (paging) grid.PageSize = pgSize;

            grid.RowStyle.CssClass = "it";
            grid.SelectedRowStyle.CssClass = "sit";
            grid.EditRowStyle.CssClass = "eit";
            grid.EmptyDataRowStyle.CssClass = "edt";
            grid.GridLines = GridLines.None;

            grid.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            grid.PagerSettings.FirstPageText = "|◄";
            grid.PagerSettings.LastPageText = "►|";
            grid.PagerSettings.NextPageText = "►";
            grid.PagerSettings.PreviousPageText = "◄";
            grid.PagerStyle.CssClass = "pager";

            //grid.Attributes["name"] = "report";  jest dodawany w OnPrerender
        }

        public static void Prepare(DetailsView details, string css)
        {
            details.CssClass = css;

            details.AutoGenerateRows = true;

            //details.ShowFooter = true;
            //details.EmptyDataText = L.p("Brak danych");
            if (String.IsNullOrEmpty(details.EmptyDataText))
                details.EmptyDataText = "Brak danych";
            details.GridLines = GridLines.None;
            details.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            details.PagerSettings.FirstPageText = "|◄";
            details.PagerSettings.LastPageText = "►|";
            details.PagerSettings.NextPageText = "►";
            details.PagerSettings.PreviousPageText = "◄";
            details.PagerStyle.CssClass = "pager";
            details.FooterStyle.CssClass = "footer";
            details.CommandRowStyle.CssClass = "control";
            details.FieldHeaderStyle.CssClass = "label";
            details.EmptyDataRowStyle.CssClass = "edt";
            details.HeaderStyle.CssClass = "header";
            details.InsertRowStyle.CssClass = "iit";
            details.EditRowStyle.CssClass = "eit";
            details.AlternatingRowStyle.CssClass = "alt";

            details.DataBound += new EventHandler(dv_DataBound);
            details.PreRender += new EventHandler(dv_PreRender);
        }


        /*         
        UWAGA !!! 
        * 
        * decrypt zawsze zwraca cos więcej /0/0/0, co wypada obciąć ...         
        */

        public static string DecryptParam(string par)
        {
            return Report.DecryptQueryString(par, key, salt);
        }

        public static string[] DecryptParams(string par)
        {
            string p = DecryptParam(par);
            int i = p.LastIndexOf("|");
            if (i != -1) p = p.Remove(i);
            return Tools.GetLineParams(p);
        }
        //-------------------------------------------
        public static bool IsEmpty(string s)
        {
            return String.IsNullOrEmpty(s) || s.ToLower() == nbsp;
        }

        //-------------------------------------------
        public class CellData
        {
            public const string errValue = "error";
            public string header = null;     // napis
            public string typ = null;     // 
            public string format = null;
            public bool withSum = false;
            public bool withCount = false;
            public bool withZoom = false;
            public string zoom = null;
            public string hint = null;
            public string colcss = null;
            public string colattrkey = null;
            public string colattrvalue = null;
            public double sum = 0;
            public int _count = 0;
            public int numPrec = -1;           // tak jak jest
            public bool num1000Sep = false;
            public string[] hlines = null;     // linie nagłówka
            public int hlinescnt = 0;     // ilość linii nagłówka
            public bool rowclick = false;
            public string rowclickcmd = null;
            public string rowclickhint = null; 

            public CellData(TableCell cell)
            {
                AddAttr(cell);
                if (cell.Controls.Count > 0)        // tu jest dynamiczna kontrolka LinkButton jezeli autogenerate columns
                {
                    LinkButton c = cell.Controls[0] as LinkButton;
                    if (c != null)
                    {
                        header = PrepareHeader(c.Text);
                        c.Text = header;
                        cell.CssClass = css;
                        return;
                    }
                }
                header = PrepareHeader(cell.Text);      // zwykły tekst
                cell.Text = header;
                cell.CssClass = css;
            }
            //---------
            // header:typ format S|page param1 param2 ..)

            // formaty dla liczb:
            //http://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx

            private string RevertHtmlCharCodes(string s)
            {
                string ret = HttpUtility.HtmlDecode(s);    // zamiana &#234; na ó bo ; rozdziela wiersze nagłówka
                return ret;
            }

            public void AddAttr(TableCell cell)
            {
                if (!String.IsNullOrEmpty(colattrkey))
                    cell.Attributes.Add(colattrkey, colattrvalue);
            }

            private string PrepareHeader(string h)
            {
                string[] hh = h.Split('|');         // [nazwa_kolumny:format;css|link/javascript:/cmd:|hint]   - max 128 znaków !!!
                string[] tt = hh[0].Split(':');     // [nazwa:format;css;attr]  <<<<<< potem do zastanowienia styl (ma ; w sobie)
                header = RevertHtmlCharCodes(tt[0]);
                hlines = header.Split(';');         // [linia1;linia2;nazwa_kolumny:format;css]
                hlinescnt = hlines.Length;
                if (hlinescnt > 1)
                    header = hlines[hlinescnt - 1];
                //----- format -----
                if (tt.Length > 1)
                {
                    string[] cc = RevertHtmlCharCodes(tt[1]).Split(';');
                    string f = cc[0];                   // [format]
                    if (cc.Length > 1) colcss  = cc[1]; // [;css]
                    if (cc.Length > 2)
                    {
                        string[] attr = cc[2].Split('='); // [;;attr]
                        colattrkey = attr[0].Trim();
                        if (attr.Length > 1)
                        {
                            string v = attr[1].Trim();
                            if (v.StartsWith("\"")) v = v.Substring(1);   // odcinam "value"
                            if (v.EndsWith("\"")) v = v.Substring(0, v.Length - 1);
                            colattrvalue = v;
                        }
                    }
                    if (!String.IsNullOrEmpty(f))       // ...moze być tylko css
                    {
                        switch (f)
                        {
                            case tRowClass:
                                typ = f;
                                break;
                            case tRowClick:
                                typ = f;
                                rowclick = true;
                                break;
                            default:
                                int len = f.Length;
                                withSum = f.EndsWith(tSum);
                                if (withSum) len--;
                                withCount = f.EndsWith(tCount);
                                if (withCount) len--;

                                switch (Tools.Substring(f, 0, 2))    // dwuliterowe, jeśli nie to jednoliterowe, string moze być krótszy
                                {
                                    case _tDateTime:
                                        typ = f.Substring(0, 2);
                                        format = f.Substring(2, len - 2);
                                        break;

                                    case tLp:
                                        typ = f.Substring(0, 2);
                                        format = null;
                                        break;

                                    case tSelect:
                                        typ = f.Substring(0, 2);
                                        format = f.Substring(2, len - 2);
                                        break;

                                    default:
                                        typ = f.Substring(0, 1);
                                        format = len > 0 ? f.Substring(1, len - 1) : null;
                                        /*
                                        switch (typ)
                                        {
                                            case tNum:
                                                if (len > 0)
                                                {
                                                    string f1 = f.Substring(1, len - 1);
                                                    format = f1;    
                                                }
                                                else
                                                    format = null;
                                                break;
                                            default:
                                                format = len > 0 ? f.Substring(1, len - 1) : null;
                                                break;
                                        }
                                        */
                                        break;
                                }
                                break;
                        }
                    }
                }
                //----- zoom -----
                if (hh.Length > 1)
                {
                    withZoom = true;
                    zoom = hh[1];
                }
                //----- hint -----
                if (hh.Length > 2)
                    hint = hh[2];

                switch (typ)
                {
                    case tHide:
                    case tRowClass:
                    case tRowClick:
                        return header;
                    default:
                        //return L.p(header);
                        return header;
                }
            }

            private string N(string num, out bool zero)
            {
                double d;
                string n;
                if (Double.TryParse(num, out d))
                {
                    if (String.IsNullOrEmpty(format))
                    {
                        n = d.ToString();
                        sum += d;
                    }
                    else
                    {
                        n = d.ToString(format);
                        //sum += Double.Parse(n);    //20130829 <<< po co to ??? zeby sumy były z wyświetlonych zaokrągleń ?
                        sum += d;


                        //n = d.ToString("#'" + format.Replace("0", "#"));

                    }
                    zero = d == 0.0;
                    return n;
                }
                else
                {
                    zero = false;
                    return num;
                }
            }

            private string D(string d)
            {
                if (d.Length > 10)
                    return d.Substring(0, 10);
                else
                    return d;
            }

            private string D2(object dt)
            {
                if (db.isNull(dt))
                    return null;
                else
                    return Tools.DateToStr(dt);
            }

            private string DT2(object dt)
            {
                if (db.isNull(dt))
                    return null;
                else
                    return Tools.DateTimeToStr(dt);
            }
            private bool FieldIs(string colName, string fieldName)
            {
                if (colName.Length == fieldName.Length)
                    return colName == fieldName;
                else if (colName.Length > fieldName.Length)
                    return colName.StartsWith(fieldName + ":") ||
                           colName.StartsWith(fieldName + "|");
                else
                    return false;
            }

            private string GetColumnName(string colNameFormat)
            {
                int p = colNameFormat.IndexOfAny(":|".ToCharArray());
                if (p >= 0)
                    return colNameFormat.Substring(0, p);
                else
                    return colNameFormat;
            }

            private string PrepareParamsJs(string par)
            {
                const char comma = ',';
                const char space = ' ';
                const char quota1 = '\'';
                const char quota2 = '"';

                int i1 = par.IndexOf('(');
                int i2 = par.LastIndexOf(')');
                string p1 = par.Substring(0, i1 + 1);
                string p2 = par.Substring(i1 + 1, i2 - i1 - 1);
                string p3 = par.Substring(i2);
                char[] p = p2.ToCharArray();
                int mode = 0;
                for (int i = 0; i < p.Length; i++)          // (|   'xc v'  p1 p2   'aas sds'   p3 |)
                {
                    switch (mode)
                    {
                        case 0:     //szukam końca tekstu, wycinam '' lub ""
                            switch (p[i])
                            {
                                case quota1:
                                    mode = 2;
                                    break;
                                case quota2:
                                    mode = 3;
                                    break;
                                case space:
                                    p[i] = comma;
                                    mode = 1;
                                    break;
                            }
                            break;
                        case 1:     //pomijam spacje spacji do zamiany
                            switch (p[i])
                            {
                                case space:
                                    break;
                                case quota1:
                                    mode = 2;
                                    break;
                                case quota2:
                                    mode = 3;
                                    break;
                                default:        // znak
                                    mode = 0;
                                    break;
                            }
                            break;
                        case 2:     // skip quota1
                            switch (p[i])
                            {
                                case quota1:
                                    mode = 0;
                                    break;
                            }
                            break;
                        case 3:     // skip quota2
                            switch (p[i])
                            {
                                case quota2:
                                    mode = 0;
                                    break;
                            }
                            break;
                    }
                }
                return p1 + new string(p) + p3;
            }






            private string FormatPipeParams(GridView gv, string value)
            {
                string fmt = Tools.GetLineParam(gv.ToolTip, 1);
                return FormatPipeParams(fmt, value);
            }

            private string FormatPipeParams(string fmt, string value)
            {
                if (!String.IsNullOrEmpty(fmt) && value.Contains('|'))
                {
                    string[] v = Tools.GetLineParams(value);
                    return String.Format(fmt, v);
                }
                else
                    return value;
            }









            private void Zoom(GridView gv, TableCell cell, string value, DataRowView drv)
            {
                //bool cryptCmd = cryptParams;
                bool cryptCmd = false;    // !!!! na razie poki testów sie nie zrobi <<< cntReport2 sie wywala




                if (withZoom)
                {
                    string title = null;
                    if (!String.IsNullOrEmpty(hint))
                        //title = String.Format(" title=\"{0}\"", L.p(hint)); //pozniej dac parsowanie 
                        title = String.Format(" title=\"{0}\"", hint); //pozniej dac parsowanie 


                    if (typ == tSelect)     //SEL.items [:CBH;check|@pracId|Zaznacz osobę],
                    {
                        string par = zoom.Trim();
                        for (int idx = 0; idx < drv.Row.ItemArray.Length; idx++)
                        {
                            string h = GetColumnName(drv.Row.Table.Columns[idx].Caption);
                            string v = drv[idx].ToString();
                            if (par == "@" + h)   // jeden parametr tu
                            {
                                par = v;
                                break;
                            }
                            //par = par.Replace("@" + h, v);
                        }
                        //bool chk = !String.IsNullOrEmpty(value) && value.ToLower() != nbsp;
                        bool chk = !IsEmpty(value);
                        //cell.Text = CheckBoxHtml("s" + par, "cbselect", String.Format("gvRowSelect(this,'{0}');", gv.ClientID + "Selected"), chk);
                        cell.Text = CbSelectHtml(gv, "s" + par, chk);
                    }
                    else
                    {
                        string cmd = zoom.Trim().ToLower();
                        bool j2 = cmd.StartsWith("js:");
                        if (j2 || cmd.StartsWith("javascript:"))
                        {
                            //----- javascript -----
                            //string js = j2 ? ("javascript:" + zoom.Substring(3)) : zoom;
                            string js = j2 ? zoom.Substring(3) : zoom.Substring(11);
                            js = PrepareParamsJs(js);
                            if (!js.EndsWith(";")) js += ";";


                            //int i1 = js.IndexOf('(');
                            //int i2 = js.LastIndexOf(')');
                            //if (i1 > 0 && i2 > 0)
                            //    js = js.Substring(0, i1) + js.Substring(i1, i2 - i1).Replace(' ', ',') + js.Substring(i2);                                // najprościej ale nie może być zbędnych spacji!!!
                            //    //js = js.Substring(0, i1+1) + js.Substring(i1+1, i2 - i1 - 1).Trim().Replace("  ",",").Replace(' ', ',') + js.Substring(i2);  // ale to nie rozwiązuje wszystkich problemów np 3 spacji


                            for (int idx = 0; idx < drv.Row.ItemArray.Length; idx++)
                            {
                                string h = GetColumnName(drv.Row.Table.Columns[idx].Caption);
                                string v = drv[idx].ToString();
                                js = js.Replace("@" + h, v);
                            }
                            //cell.Text = String.Format("<a href=\"{0}\"{2}>{1}</a>", js, value, title);
                            //if (String.IsNullOrEmpty(value) || value.ToLower() == nbsp)
                            if (IsEmpty(value))
                                cell.Text = null;
                            else
                                cell.Text = String.Format("<a href=\"#\" onclick=\"{0}return false;\"{2}>{1}</a>", js, FormatPipeParams(gv, value), title);
                        }
                        else
                        {
                            string[] zz = zoom.Split(' ');
                            cmd = zz[0].Trim().ToLower();

                            if (String.IsNullOrEmpty(cmd))
                            {
                                //----- hint -----
                                cell.Text = String.Format("<span {1}>{0}</span>", FormatPipeParams(gv, value), title);
                            }
                            else
                            {
                                //----- link -----
                                cmd = zz[0].Trim().ToLower();
                                bool iscmd = cmd.StartsWith("cmd:");
                                string par = null;
                                for (int i = 1; i < zz.Length; i++)
                                {
                                    string p = zz[i];
                                    if (p.StartsWith("@"))
                                    {
                                        string p1 = p.Substring(1);
                                        int idx = Tools.StrToInt(p1, -99);
                                        if (idx == -99)
                                        {
                                            for (int j = 0; j < drv.Row.ItemArray.Length; j++)
                                                if (FieldIs(drv.Row.Table.Columns[j].Caption, p1))    //drv.Row.Table.Columns[j].Caption.StartsWith(p1))
                                                {
                                                    idx = j;
                                                    break;
                                                }
                                        }
                                        if (idx != -99)
                                            p = drv[idx].ToString();
                                    }




                                    if (iscmd)
                                        if (cryptCmd)
                                            par += p.Replace("|", "_").Replace("'", "\'") + "|";      //formatuje do postaci xxx|yyy|...    // zeby za był pusty parametr, bo tam 0x0 przychodzą
                                        else
                                            par += String.Format("|{0}", p.Replace("|", "_").Replace("'", "\'"));   //formatuje do postaci |xxx|yyy...
                                    else
                                        if (cryptParams)
                                            par += p + "|";   // zeby za był pusty parametr, bo tam 0x0 przychodzą
                                        else
                                            par += String.Format("&p{0}={1}", i, p.Replace("&", "%26"));   //formatuje do postaci p1=xxx&p2=yyy...



                                }

                                string href;
                                if (iscmd)
                                {
                                    cmd = zz[0].Substring(4);  // cmd:
                                    if (cryptCmd)
                                        href = String.Format("javascript:doClickPar('{0}','{1}');", gv.ClientID + "Cmd", Report.EncryptQueryString(cmd + '|' + par, key, salt));
                                    else
                                        href = String.Format("javascript:doClickPar('{0}','{1}');", gv.ClientID + "Cmd", cmd + par);
                                }
                                else
                                {
                                    href = zz[0] + ".aspx";
                                    if (!String.IsNullOrEmpty(par))
                                        if (cryptParams)
                                            href += "?p=" + Report.EncryptQueryString(par, key, salt);      // + pusty parametr
                                        else
                                            href += "?" + par.Substring(1);
                                }

                                //if (String.IsNullOrEmpty(value) || value.ToLower() == nbsp)
                                if (typ == tRowClick)
                                {
                                    cell.Text = null;
                                    rowclickcmd = href;
                                    rowclickhint = hint;
                                }
                                else if (IsEmpty(value))
                                    cell.Text = null;
                                else
                                    cell.Text = String.Format("<a href=\"{0}\"{2}>{1}</a>", href, FormatPipeParams(gv, value), title);
                            }
                        }
                    }
                }
                else
                {
                    /**/
                    //if (!Editable)
                    cell.Text = FormatPipeParams(gv, value);    // zeby edycja działała nie mozna nadpisać, ale inaczej nie da się poformatować wi włączone.... 
                    /**/
                    if (!String.IsNullOrEmpty(hint))
                        cell.ToolTip = hint;
                }
            }




























            /*
        else if (cmd.StartsWith("xxxcmd:"))
        {
            //----- command -----
            string gvClientId = gv.ClientID.Replace("_", "$");   // <<< to jest do zmiany z ustawień



            cell.Text = String.Format("<a href=\"javascript:__doPostBack('{0}','{1}');\"{3}>{2}</a>", gvClientId, "Select$0", value, title);




            /*
            LinkButton lb = new LinkButton();
            lb.Text = value;
            lb.ID = "someId";
            lb.CommandName = "test";
            lb.CommandArgument = "123";
            //lb.OnClientClick = "javascript: return confirm('Are you sure that you want to do this and that?'); ";

            //TableCell cell = new TableCell();
            cell.Controls.Add(lb);
            //e.Row.Cells.Add(cell);
             * / 
        }
             */

            /*

            private string Zoom(string value, DataRowView drv)
            {
                if (withZoom)
                {
                    string title = null;
                    if (!String.IsNullOrEmpty(hint))
                        title = String.Format(" title=\"{0}\"", hint); //pozniej dac parsowanie 

                    string zzz = zoom.Trim().ToLower();
                    if (zzz.StartsWith("javascript") || zzz.StartsWith("js"))
                    {
                        //----- javascript -----
                        string js = zoom;
                        for (int idx = 0; idx < drv.Row.ItemArray.Length; idx++)
                        {
                            string h = GetColumnName(drv.Row.Table.Columns[idx].Caption);
                            string v = drv[idx].ToString();
                            js = js.Replace("@" + h, v);
                        }
                        return String.Format("<a href=\"{0}\"{2}>{1}</a>", js, value, title);
                    }
                    else if (zzz.StartsWith("cmd"))
                    {
                        //----- command -----

                    }
                    else
                    {
                        string[] zz = zoom.Split(' ');

                        if (String.IsNullOrEmpty(zz[0]))
                        {
                            //----- hint -----
                            return String.Format("<span {1}>{0}</span>", value, title);
                        }
                        else
                        {
                            //----- link -----
                            string page = zz[0] + ".aspx";
                            string par = null;
                            for (int i = 1; i < zz.Length; i++)
                            {
                                string p = zz[i];
                                if (p.StartsWith("@"))
                                {
                                    string p1 = p.Substring(1);
                                    int idx = Tools.StrToInt(p1, -99);
                                    if (idx == -99)
                                    {
                                        for (int j = 0; j < drv.Row.ItemArray.Length; j++)
                                            if (FieldIs(drv.Row.Table.Columns[j].Caption, p1))    //drv.Row.Table.Columns[j].Caption.StartsWith(p1))
                                            {
                                                idx = j;
                                                break;
                                            }
                                    }
                                    if (idx != -99)
                                        p = drv[idx].ToString();
                                }
                                par += String.Format("&p{0}={1}", i, p.Replace("&", "%26"));   //formatuje do postaci p1=xxx&p2=yyy...
                            }
                            if (!String.IsNullOrEmpty(par))
                                page += "?" + par.Substring(1);

                            return String.Format("<a href=\"{0}\"{2}>{1}</a>", page, value, title);
                        }
                    }
                }
                return value;
            }
            */
            //---------
            public void PrepareCell2(GridView gv, GridViewRow row, int cellindex) // TableCell cell, DataRowView drv)     // rozbija value na elementy klasy
            {
                TableCell cell = row.Cells[cellindex];
                DataRowView drv = (DataRowView)row.DataItem;


                //DataControlFieldCell cc = (DataControlFieldCell)cell;
                //DataControlField df = cc.ContainingField;
                if (((DataControlFieldCell)cell).ContainingField is CommandField)
                {
                }
                else
                {
                    if (gv.EditIndex == row.DataItemIndex)
                        return;

                    cell.CssClass = css;
                    AddAttr(cell);

                    string c;
                    switch (typ)
                    {
                        case tHide:
                            cell.Visible = false;
                            return;
                        case tRowClass:
                            string cs = cell.Text;
                            //if (!String.IsNullOrEmpty(cs) && cs.ToLower() != nbsp)
                            if (!IsEmpty(cs))
                                row.CssClass = (row.CssClass + " " + cs).TrimStart();
                            cell.Visible = false;
                            return;
                        case tRowClick:
                            c = cell.Text;                            
                            cell.Visible = false;
                            break;
                        case tDate:
                            c = D(cell.Text);
                            break;
                        case tNum:
                            bool zero;
                            c = N(cell.Text, out zero);
                            if (zero)
                                cell.CssClass += " zero";
                            break;
                        default:
                            c = cell.Text;
                            break;
                    }

                    Zoom(gv, cell, c, drv);

                    if (!withZoom)
                        cell.ToolTip = hint;

                    //if (withCount)  // zawsze
                        _count++;
                }
                //if (!String.IsNullOrEmpty(typ))
                //{
                //}
            }
            public void PrepareCell(GridView gv, GridViewRow row, int cellindex) // TableCell cell, DataRowView drv)     // rozbija value na elementy klasy
            {
                TableCell cell = row.Cells[cellindex];
                DataRowView drv = (DataRowView)row.DataItem;


                //DataControlFieldCell cc = (DataControlFieldCell)cell;
                //DataControlField df = cc.ContainingField;
                if (((DataControlFieldCell)cell).ContainingField is CommandField)
                {
                }
                else
                {
                    if (gv.EditIndex == row.DataItemIndex)
                        return;

                    cell.CssClass = css;
                    AddAttr(cell);

                    string c;
                    switch (typ)
                    {
                        case tHide:
                            cell.Visible = false;
                            return;
                        case tRowClass:
                            string cs = cell.Text;
                            //if (!String.IsNullOrEmpty(cs) && cs.ToLower() != nbsp)
                            if (!IsEmpty(cs))
                                row.CssClass = (row.CssClass + " " + cs).TrimStart();
                            cell.Visible = false;
                            return;
                        case tRowClick:
                            c = cell.Text;
                            Zoom(gv, cell, c, drv);
                            cell.Visible = false;
                            return;
                        case tDate:
                            //c = D(cell.Text);
                            c = D2(drv[cellindex]);
                            break;
                        case _tDateTime:
                            c = DT2(drv[cellindex]);
                            break;
                        case tLp:
                            c = (Tools.StrToInt(cell.Text, 1) + _count).ToString();   // _count jest od 0 tu
                            break;
                        case tNum:
                            bool zero;
                            c = N(cell.Text, out zero);
                            if (zero)
                                cell.CssClass += " zero";
                            break;
                        default:
                            c = cell.Text;
                            break;
                    }

                    Zoom(gv, cell, c, drv);

                    //if (withCount)  // zawsze
                        _count++;
                }
                //if (!String.IsNullOrEmpty(typ))
                //{
                //}
            }



            /*
protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;


            List<TableCell> cells = new List<TableCell>();


            foreach (DataControlField column in GridView1.Columns)
            {
                // Retrieve first cell
                TableCell cell = row.Cells[0];


                // Remove cell
                row.Cells.Remove(cell);


                // Add cell to list
                cells.Add(cell);
            }


            // Add cells
            row.Cells.AddRange(cells.ToArray());
        }             */

            public double FooterValue
            {
                get
                {
                    if (withSum) return sum;
                    else if (withCount) return _count;
                    else return 0;
                }
            }

            public void PrepareFooter(TableCell cell)   // <<<<<<< poprzestawiać ustawianie css 
            {
                cell.CssClass = css;
                AddAttr(cell);

                switch (typ)
                {
                    case tHide:
                    case tRowClass:
                    case tRowClick:
                        cell.Visible = false;
                        return;
                    default:
                        if (typ == tNum && withSum)
                        {
                            cell.CssClass = css;
                            cell.Text = sum.ToString(format);
                        }
                        else if (withCount)
                        {
                            cell.CssClass = css;
                            cell.Text = _count.ToString();
                        }
                        break;
                }
            }

            public string css
            {
                get
                {
                    switch (typ)
                    {
                        case tHide:
                        case tRowClass:
                            return cssHidden + (String.IsNullOrEmpty(colcss) ? null : " ") + colcss;
                        case tRowClick:
                            return null;
                        default:
                            return typ + (String.IsNullOrEmpty(typ) || String.IsNullOrEmpty(colcss) ? null : " ") + colcss;
                    }
                }
            }
        }
        //------------------------------------
        private static CellData[] cellData = null;
        private static int rcnt = 0;

        /*
        public static string selectInputHtml(CellData cd, string value)   // cbval = checked(0/1)|id, cbval = null -> header
        {
            string id;
            bool h = String.IsNullOrEmpty(value);
            bool chk;
            if (h)
            {
                chk = false;   // !!!! docelowo skądś odczytać !!! checked
                id = null;     // pracId
            }
            else
            {
                string c;
                Tools.GetLineParams(value, out c, out id);
                chk = !String.IsNullOrEmpty(c) && c != "0";    // '',0 -> false 
            }

            return CheckBoxHtml();


            return String.Format("<input type=\"checkbox\" {1} id=\"cb{0}\" {2} onclick=\"{3}\"></input>",
                id,
                h ? "" : "class=\"cbselect\"",
                chk ? "checked" : "",
                //String.Format("gvRowSelect(this);", null)
                "gvRowSelect(this);"
                );
        }
        */

        public static string CheckBoxHtml(string id, string css, string onClick, bool check)
        {
            return String.Format("<input type=\"checkbox\" id=\"{0}\"{1}{2}{3}></input>",
                id,
                String.IsNullOrEmpty(css) ? "" : String.Format(" class=\"{0}\"", css),
                String.IsNullOrEmpty(onClick) ? "" : String.Format(" onclick=\"{0}\"", onClick),
                check ? " checked=\"checked\"" : ""
                );
        }

        public static string CbSelectHtml(GridView gv, string id, bool chk)
        {
            return CheckBoxHtml(id, "cbselect", String.Format("gvRowSelect(this,'{0}');", gv.ClientID + "Selected"), chk);
        }


        //--------------------------
        /* hlines[]: line-0 (bottom), line-1, line-2 (top) 
+---------------------------+--------------+
|       | CCCCC             |              |
|       +---------+---------+              |
|       | BB1     | BB2     | BB3          |
|.......+---------+---------+--------------+
| AAAAA | A2 | A3 | A4 | A5 | A6 | A7 | A8 |
+-------+----+----+----+----+--------------+
        */

        private static void AddTh(GridViewRow tr, ref int no, string text, int colspan, int rowspan, string css)
        {
            no++;
            TableHeaderCell th = new TableHeaderCell();
            th.Text = text;
            th.ColumnSpan = colspan;
            th.RowSpan = rowspan;
            th.CssClass = String.Format("col{0}{1}", no, (" " + css).TrimEnd());
            tr.Cells.Add(th);
        }


        private class Cell
        {
            public string Text;
            public string Hint;
            public int colspan;
            public int rowspan;
            public string css;

            public Cell(string text, int cspan, int rspan, string acss)
            {
                Text = text;
                colspan = cspan;
                rowspan = rspan;
                css = acss;
            }
        }

        private static void AddMultilineHeader(GridView gv, GridViewRow row, int hcnt)
        {
            if (hcnt > 1)   // są jakieś wiersze dodatkowe
            {
                hcnt--;     // bez właściwego th
                int cnt = cellData.Count();
                Cell[,] cells = new Cell[hcnt, cnt];
                int[] first = new int[hcnt];

                for (int r = 0; r < hcnt; r++)
                    for (int c = 0; c < cnt; c++)      // zerowanie
                        cells[r, c] = null;

                for (int r = 0; r < hcnt; r++)
                {
                    int _c0 = -1;
                    int cno = 1;
                    for (int c = 0; c < cnt; c++)
                    {
                        CellData cd = cellData[c];
                        switch (cd.typ)
                        {
                            case tHide:
                            case tRowClass:
                            case tRowClick:
                                break;
                            default:
                                int hlcnt = cd.hlinescnt - 1;       // bez własciwego th
                                if (r == 0 && hlcnt == 0)
                                {
                                    Cell cell = new Cell(null, 1, hcnt, "no-bottom-border");
                                    row.Cells[c].CssClass = (row.Cells[c].CssClass + " no-top-border").TrimStart();
                                    cells[r, c] = cell;
                                }
                                else if (r < hlcnt)
                                {
                                    string text = cd.hlines[r];
                                    string text0 = null;
                                    if (_c0 != -1)   // pierwsza kolumna
                                    {
                                        CellData cd0 = cellData[_c0];
                                        text0 = r < cd0.hlinescnt - 1 ? cd0.hlines[r] : null;
                                    }
                                    int rowspan = hcnt - hlcnt + 1;
                                    int rr = r == 0 ? 0 : r + rowspan - 1;
                                    if (text0 != text)
                                    {
                                        Cell cell = new Cell(text, 1, r == 0 ? rowspan : 1, null);
                                        cells[rr, c] = cell;
                                        _c0 = c;
                                    }
                                    else
                                    {
                                        Cell cell = cells[rr, _c0];
                                        if (cell != null)
                                            cell.colspan++;
                                    }
                                }
                                break;
                        }
                    }
                }


                for (int r = 0; r < hcnt; r++)
                {
                    GridViewRow tr = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    tr.CssClass = String.Format("header{0}", r + 1);
                    int cno = 0;
                    for (int c = 0; c < cnt; c++)
                    {
                        Cell cell = cells[r, c];
                        if (cell != null)
                            AddTh(tr, ref cno, cell.Text, cell.colspan, cell.rowspan, cell.css);
                    }
                    gv.Controls[0].Controls.AddAt(r, tr);
                }
                gv.EnableViewState = false;   // pomaga naprawić ViewState tree validate, chociaż za każdym postback jest databind, tyle, że przy raportach to nie robi ... *do momentu dodania zoom w popup :(
                gv.CssClass = (gv.CssClass + " multipleheader").TrimStart();
            }
        }






        /*
        for (int c = 0; c < cnt; c++)
        {
            CellData cd = cellData[c];
                    
            for (int r = 0; r < hcnt; r++)      // zerowanie
                cells[r, c] = null;

            int rowspan = 1;
            int hlcnt = cd.hlinescnt - 1;       // bez własciwego th
            for (int r = 0; r < hlcnt || hlcnt == 0 && r < cd.hlinescnt; r++)   // z uwzględnieniem pojedynczego wiersza
            {
                if (c == 0) first[r] = 0;       // zerowanie

                Cell cell = new Cell();
                cell.Text = hlcnt == 0 ? null : cd.hlines[r];                   // z uwzględnieniem pojedynczego wiersza
                cell.colspan = 1;
                if (r == 0)
                {
                    rowspan = hlcnt == 0 ? hcnt : hcnt - hlcnt + 1;             // z uwzględnieniem pojedynczego wiersza
                    cell.rowspan = rowspan;
                    cells[0, c] = cell;
                }
                else
                {
                    cell.rowspan = 1;
                    cells[r + rowspan - 1, c] = cell;
                }
            }
        }
         */





        /*
                
        hcnt--;     // bez wiersza z nazwami kolumn
        int cnt = cellData.Count();
        GridViewRow[] tr = new GridViewRow[hcnt];    
        for (int r = 0; r < hcnt; r++)
        {
            GridViewRow tr1 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            tr1.CssClass = String.Format("header{0}", r + 1);
            tr[r] = tr1;
            string hlast = null;
            string htext = null;
            int colspan = 0;
            int rowspan = 1;
            int rslast = 1;
            string css = null;
            int cno = 0;
            for (int c = 0; c < cnt; c++)
            {
                CellData cd = cellData[c];
                bool add = false;
                bool onerow = cd.hlinescnt == 1;
                if (cd.hlinescnt == 1)              // tylko 1 nagłówek 
                {
                    htext = hlast = null;
                    rowspan = rslast = hcnt;
                    if (c > 0)                      // taki sam mechanizm wszędzie - dodaję "po"
                    {
                        add = true;
                        css = "no-bottom-border";
                    }
                }

                if (r < cd.hlinescnt - 1)           // bez wiersza z nazwami kolumn
                {
                    htext = cd.hlines[r];
                    rowspan = r == 0 ? (hcnt + 1) - cd.hlinescnt + 1 : 1;   // w pierwszym wierszu zawsze dokładam wysokość
                    if (c == 0 || add)              // pierwsza kolumna lub poprzednio 1 linia
                    {
                        hlast = htext;
                        rslast = rowspan;
                    }
                    else if (htext != hlast)       
                    {
                        add = true;
                        css = null;
                    }
                }

                if (add)  // dodaję po zmianie textu
                {
                    AddTh(tr[r], ref cno, hlast, colspan, rslast, css);
                    colspan = 0;
                    rslast = rowspan;
                    hlast = htext;
                }

                if (r < cd.hlinescnt - 1) 
                    colspan++;        
            }
            AddTh(tr[r], ref cno, htext, colspan, rowspan, css);     //if (cnt > 0)  zawsze musi być
        }
        for (int r = 0; r < hcnt; r++)                      // bez wiersza z nazwami kolumn
        {
            gv.Controls[0].Controls.AddAt(r, tr[r]);
        }
         */

















        /*
                 private static void AddMultilineHeader(GridView gv, GridViewRow row, int hcnt)
                {
                    if (hcnt > 1)   // są jakieś wiersze dodatkowe
                    {
                        hcnt--;     // bez wiersza z nazwami kolumn
                        int cnt = cellData.Count();
                        GridViewRow[] tr = new GridViewRow[hcnt];    
                        for (int r = 0; r < hcnt; r++)
                        {
                            GridViewRow tr1 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                            tr1.CssClass = String.Format("header{0}", r + 1);
                            tr[r] = tr1;
                            string hlast = null;
                            string htext = null;
                            int colspan = 0;
                            int rowspan = 1;
                            int rslast = 1;
                            string css = null;
                            int cno = 0;
                            for (int c = 0; c < cnt; c++)
                            {
                                CellData cd = cellData[c];
                                bool add = false;
                                bool onerow = cd.hlinescnt == 1;
                                if (cd.hlinescnt == 1)              // tylko 1 nagłówek 
                                {
                                    htext = hlast = null;
                                    rowspan = rslast = hcnt;
                                    if (c > 0)                      // taki sam mechanizm wszędzie - dodaję "po"
                                    {
                                        add = true;
                                        css = "no-bottom-border";
                                    }
                                }

                                if (r < cd.hlinescnt - 1)           // bez wiersza z nazwami kolumn
                                {
                                    htext = cd.hlines[r];
                                    rowspan = r == 0 ? (hcnt + 1) - cd.hlinescnt + 1 : 1;   // w pierwszym wierszu zawsze dokładam wysokość
                                    if (c == 0 || add)              // pierwsza kolumna lub poprzednio 1 linia
                                    {
                                        hlast = htext;
                                        rslast = rowspan;
                                    }
                                    else if (htext != hlast)       
                                    {
                                        add = true;
                                        css = null;
                                    }
                                }

                                if (add)  // dodaję po zmianie textu
                                {
                                    AddTh(tr[r], ref cno, hlast, colspan, rslast, css);
                                    colspan = 0;
                                    rslast = rowspan;
                                    hlast = htext;
                                }

                                if (r < cd.hlinescnt - 1) 
                                    colspan++;        
                            }
                            AddTh(tr[r], ref cno, htext, colspan, rowspan, css);     //if (cnt > 0)  zawsze musi być
                        }
                        for (int r = 0; r < hcnt; r++)                      // bez wiersza z nazwami kolumn
                        {
                            gv.Controls[0].Controls.AddAt(r, tr[r]);
                        }
                        gv.EnableViewState = false;   // pomaga naprawić ViewState tree validate, chociaż za każdym postback jest databind, tyle, że przy raportach to nie robi ... *do momentu dodania zoom w popup :(
                    }
                }
        */



        private static void AddMultilineHeader3(GridView gv, GridViewRow row, int hcnt)
        {
            if (hcnt > 1)
            {
                int cnt = cellData.Count();
                GridViewRow[] tr = new GridViewRow[hcnt];
                for (int r = 0; r < hcnt; r++)
                {
                    GridViewRow tr1 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    //tr1.EnableViewState = false;
                    tr1.CssClass = String.Format("header{0}", r + 1);
                    tr[r] = tr1;
                    string last = null;
                    int colspan = 0;
                    int cno = 0;
                    for (int c = 0; c < cnt; c++)
                    {
                        CellData cd = cellData[c];
                        if (r < cd.hlinescnt)
                        {
                            string h = cd.hlines[r];
                            if (c == 0) last = h;
                            bool lastcol = c == cnt - 1;
                            if (c > 0 && h != last || lastcol)  // po zmianie lub jak ostatnia
                            {
                                TableHeaderCell th = new TableHeaderCell();
                                th.Text = last;
                                th.ColumnSpan = lastcol ? colspan + 1 : colspan;
                                //th.EnableViewState = false;
                                colspan = 0;
                                cno++;
                                th.CssClass = String.Format("col{0}", cno);
                                tr[r].Cells.Add(th);
                                last = h;
                            }
                        }
                        colspan++;
                    }
                }
                for (int r = 0; r < hcnt; r++)
                {
                    gv.Controls[0].Controls.AddAt(r, tr[r]);
                }
                gv.EnableViewState = false;   // wychodzi na to ze nie ma to zadnego wpływu, a pomaga naprawić ViewState tree validate
            }
        }

        private static void AddMultilineHeader2(GridView gv, GridViewRow row, int hcnt)
        {
            GridViewRow tr = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell th = new TableHeaderCell();
            th.Text = "Department";
            th.ColumnSpan = 2;
            th.CssClass = "col1";
            th.EnableViewState = false;
            tr.Cells.Add(th);

            th = new TableHeaderCell();
            th.Text = "Employee";
            th.ColumnSpan = 2;
            th.CssClass = "col2";
            th.EnableViewState = false;
            tr.Cells.Add(th);

            tr.EnableViewState = false;
            gv.EnableViewState = false;

            gv.Controls[0].Controls.AddAt(0, tr);
        }


        /*
        private static void AddMultilineHeader(GridView gv, GridViewRow row, int hcnt)
        {
            if (hcnt > 1)
            {
                int cnt = cellData.Count();
                GridViewRow[] tr = new GridViewRow[hcnt];  
                for (int r = 0; r < hcnt; r++)
                {
                    GridViewRow tr1 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    TableHeaderCell th;
                    tr1.CssClass = String.Format("header{0}", r + 1);
                    tr[r] = tr1;
                    string last = null;
                    int colspan = 0;
                    int cno = 0;
                    int state = 0;
                    for (int c = 0; c < cnt; c++)
                    {
                        bool lastcol = c == cnt - 1;
                        CellData cd = cellData[c];
                        switch (state)
                        {
                            case 0:
                                string h = r < cd.hlinescnt ? cd.hlines[r] : null;
                                th = new TableHeaderCell();
                                th.Text = h;
                                cno++;
                                th.CssClass = String.Format("col{0}", cno);
                                tr[r].Cells.Add(th);
                                last = h;
                                state = lastcol ? 2 : 1; 
                                break;
                        }
                        switch (state)
                        {
                            case 1:
                                break;
                        }
                        switch (state)
                        {
                            case 2:
                                th.ColumnSpan = colspan;
                                tr[r].Cells.Add(th);
                                colspan = 0;
                                state = 0;
                                break;
                        }






                        CellData cd = cellData[c];
                        if (r < cd.hlinescnt)
                        {
                            string h = cd.hlines[r];
                            if (c == 0) last = h;
                            bool lastcol = c == cnt - 1;
                            if (c > 0 && h != last || lastcol)  // po zmianie lub jak ostatnia
                            {
                                TableHeaderCell th = new TableHeaderCell();
                                th.Text = last;
                                th.ColumnSpan = lastcol ? colspan + 1 : colspan;
                                colspan = 0;
                                cno++;
                                th.CssClass = String.Format("col{0}", cno);
                                tr[r].Cells.Add(th);
                                last = h;
                            }                            
                        }
                        colspan++;
                    }
                }
                for (int r = 0; r < hcnt; r++)
                {
                    gv.Controls[0].Controls.AddAt(r, tr[r]);
                }
                gv.EnableViewState = false;   // wychodzi na to ze nie ma to zadnego wpływu, a pomaga naprawić ViewState tree validate
            }
        }
        */


        /*
        private static void AddMultilineHeader(GridView gv, GridViewRow row, int hcnt)
        {
            if (hcnt > 1)
            {
                int cnt = cellData.Count();
                GridViewRow[] tr = new GridViewRow[hcnt];  // z idx = 0 niewykorzystuje
                //string[] last = new string[hcnt];
                for (int r = 1; r < hcnt; r++)
                {
                    int rr = hcnt - r;
                    tr[r] = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    string last = null;
                    int colspan = 0;
                    for (int c = 0; c < cnt; c++)
                    {
                        colspan++;
                        CellData cd = cellData[c];
                        if (rr < cd.hlinescnt)
                        {
                            string h = cd.hlines[rr];
                            if (h != last)
                            {
                                TableHeaderCell th = new TableHeaderCell();
                                th.Text = h;
                                th.ColumnSpan = 2;
                                th.CssClass = "col1";
                                tr[r].Cells.Add(th);
                                last = h;
                            }
                            
                        }
                    }
                }
                for (int r = 1; r < hcnt; r++)
                {
                    gv.Controls[0].Controls.AddAt(r - 1, tr[r]);
                }
            }
        }
        */



        public static void PrepareHeader(GridView grid, GridViewRow row)
        {
            bool footer = false;
            int cnt = row.Cells.Count;
            cellData = new CellData[cnt];           // tabelka
            int hcnt = 1;                           // ilość wierszy nagłówka
            for (int i = 0; i < cnt; i++)
            {
                CellData cd = new CellData(row.Cells[i]);
                if (cd.hlinescnt > hcnt) hcnt = cd.hlinescnt;
                cellData[i] = cd;
                if (cd.withSum || cd.withCount)
                    footer = true;

                TableCell cell = row.Cells[i];
                if (!String.IsNullOrEmpty(cd.hint))
                {
                    if (!String.IsNullOrEmpty(cell.Text))
                        cell.Attributes.Add("title", cd.hint);
                }

                /*
                if (cd.typ == tSelect && cd.format == tSelectAll)
                    cell.Text = selectInputHtml(cd, null);
                */


                /*
                string h, t;                
                TableCell cell = row.Cells[i];
                Tools.GetLineParams(cell.Text, out h, out t);
                if (!String.IsNullOrEmpty(t))
                {
                    cell.Text = h;
                    cell.Attributes.Add("title", t);
                }
                */
            }
            grid.ShowFooter = footer;

            AddMultilineHeader(grid, row, hcnt);
            AddSortMarker(grid, row);
        }

        public static void PrepareCells2(GridView grid, GridViewRow row)
        {
            int cnt = row.Cells.Count;
            for (int i = 0; i < cnt; i++)
            {
                CellData cd = cellData[i];
                cd.PrepareCell2(grid, row, i);
            }


            //string css = row.CssClass;
            /*
            if (!String.IsNullOrEmpty(css)) css += " ";
            if (rcnt % 2 == 0) 
                row.CssClass = css + cssAltRow;
            else 
                row.CssClass = css + cssNormRow;
            */

            if (row.Visible) rcnt++;
        }

        public static void PrepareCells(GridView grid, GridViewRow row)
        {
            int rclick = -1;
            string rclickcmd  = null;
            string rclickhint = null;

            int cnt = row.Cells.Count;
            for (int i = 0; i < cnt; i++)
            {
                CellData cd = cellData[i];
                cd.PrepareCell(grid, row, i);
                if (cd.rowclick)
                {
                    rclick = i;
                    rclickcmd = cd.rowclickcmd;
                    rclickhint = cd.rowclickhint;
                }
            }

            if (rclick != -1)
            {
                row.CssClass = (row.CssClass + " rowclick").TrimStart();
                for (int i = 0; i < cnt; i++)
                {
                    CellData cd = cellData[i];
                    if (!cd.withZoom)
                    {
                        row.Cells[i].Attributes.Add("onClick", rclickcmd);
                        row.Cells[i].ToolTip = rclickhint;
                    }
                }
            }

            //string css = row.CssClass;
            /*
            if (!String.IsNullOrEmpty(css)) css += " ";
            if (rcnt % 2 == 0) 
                row.CssClass = css + cssAltRow;
            else 
                row.CssClass = css + cssNormRow;
            */
            if (row.Visible) rcnt++;
        }

        public static void PrepareFooter(GridView grid, GridViewRow row)
        {
            for (int i = 0; i < row.Cells.Count; i++)
            {
                CellData cd = cellData[i];
                cd.PrepareFooter(row.Cells[i]);
            }
            row.CssClass = cssSumRow;
        }
        //--------------------------------------------
        private static string GetFooterSql(GridView grid)
        {
            //return "select @1 [1], @2 [3], @3 [5], @4 [6]";
            //return ((SqlDataSource)grid.DataSource).UpdateCommand;

            if (!String.IsNullOrEmpty(grid.DataSourceID))
            {
                Control ds = grid.Parent.FindControl(grid.DataSourceID);
                if (ds != null && ds is SqlDataSource)
                    return ((SqlDataSource)ds).UpdateCommand;
            }
            return null;
        }

        public static void PrepareFooterSql(GridView grid, GridViewRow row)
        {
            PrepareFooter(grid, row);
            //-----
            string sql = GetFooterSql(grid);
            if (!String.IsNullOrEmpty(sql))
            {
                string par = null;
                int p = 0;
                int[] IX = new int[row.Cells.Count];   // indexy widocznych kolumn -> index w Cells
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    IX[i] = -1;     // zeruję na -1 zeby wyróżnić
                    CellData cd = cellData[i];
                    switch (cd.typ)
                    {
                        case tHide:
                        case tRowClass:
                        case tRowClick:
                            break;
                        default:
                            IX[p] = i;  // zawsze nadpisze wartość -1 ( bieżący lub wcześniejszy index)
                            p++;
                            if (cd.withSum || cd.withCount)
                                par += String.Format("declare @{0} float\r\nset @{0}={1}\r\n", p, cd.FooterValue.ToString().Replace(',', '.'));
                            break;
                    }
                }
                DataSet ds = db.getDataSet(par + sql);
                int ccnt = ds.Tables[0].Columns.Count;
                for (int i = 0; i < ccnt; i++)
                {
                    int c = Tools.StrToInt(ds.Tables[0].Columns[i].Caption, -1) - 1; // index od 1; sorry ;)  ... bo 0 sie moze kiedys do czegos przydac, poza tym w sql jest od 1
                    if (0 <= c && c < row.Cells.Count)
                    {
                        int ic = IX[c];   //index c-tej widocznej kolumny
                        if (ic != -1)
                        {
                            CellData cd = cellData[ic];
                            double d = db.getDouble(db.getRow(ds), i, 0);
                            row.Cells[ic].Text = String.IsNullOrEmpty(cd.format) ? d.ToString() : d.ToString(cd.format);
                            //row.Cells[ic].Text = db.getValue(db.getRow(ds), i);
                        }
                    }
                }
            }
        }
        //--------------------------------------------
        protected static void gv_RowDataBound2(object sender, GridViewRowEventArgs e)
        {
            GridView gv = (GridView)sender;

            switch (e.Row.RowType)
            {
                case DataControlRowType.EmptyDataRow:
                    break;
                case DataControlRowType.Header:
                    PrepareHeader(gv, e.Row);
                    break;
                case DataControlRowType.DataRow:
                    //PrepareCells2(gv, e.Row);
                    break;
                case DataControlRowType.Footer:
                    //PrepareFooter(gv, e.Row);  20160106
                    PrepareFooterSql(gv, e.Row);
                    break;
                case DataControlRowType.Separator:
                    break;
                case DataControlRowType.Pager:
                    break;
            }
        }

        protected static void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gv = (GridView)sender;

            switch (e.Row.RowType)
            {
                case DataControlRowType.EmptyDataRow:
                    break;
                case DataControlRowType.Header:
                    PrepareHeader(gv, e.Row);
                    break;
                case DataControlRowType.DataRow:
                    PrepareCells(gv, e.Row);
                    break;
                case DataControlRowType.Footer:
                    //PrepareFooter(gv, e.Row);  20160106
                    PrepareFooterSql(gv, e.Row);
                    break;
                case DataControlRowType.Separator:
                    break;
                case DataControlRowType.Pager:
                    break;
            }
        }


        private static string SortMarker(string htext, SortDirection sort)
        {
            const string SORT = "<span class=\"{1} {2}\">{3}</span>{0}";

            return String.Format(SORT, htext, cssSort,
                sort == SortDirection.Ascending ? cssSortAsc : cssSortDesc,
                sort == SortDirection.Ascending ? sortUP : sortDN);
        }

        private static void AddSortMarker(GridView gv, GridViewRow row)
        {
            bool sorted = false;
            LinkButton firstlnk = null;
            foreach (TableCell tc in row.Cells)
            {
                if (tc.HasControls())
                {
                    LinkButton lnk = (LinkButton)tc.Controls[0];    // search for the header link
                    if (lnk != null)
                    {
                        if (firstlnk == null) firstlnk = lnk;
                        if (gv.SortExpression == lnk.CommandArgument)
                        {
                            sorted = true;
                            lnk.Text = SortMarker(lnk.Text, gv.SortDirection);

                            //LiteralControl lc = new LiteralControl(sort);
                            //tc.Controls.Add(lc);
                            //int idx = tc.Controls.IndexOf(lnk);
                            //tc.Controls.AddAt(0, lc);
                            //tc.Controls.Add(lc);

                            /*
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();    // inizialize a new image                                
                            img.ImageUrl = "~/img/ico_" + (gv.SortDirection == SortDirection.Ascending ? "asc" : "desc") + ".gif";      // setting the dynamically URL of the image                                
                            tc.Controls.Add(new LiteralControl(" "));       // adding a space and the image to the header link
                            tc.Controls.Add(img);
                            */
                        }
                    }
                }
            }
            if (!sorted)
            {
                //firstlnk.Text = SortMarker(firstlnk.Text, SortDirection.Ascending);  // chociaż może lepiej nie pokazywać bo nie jest ustawiony
            }
        }


        protected static void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
            }
        }

        protected static void gv_PreRender2(object sender, EventArgs e)
        {
            ((GridView)sender).Attributes["name"] = "report";
            GridView grid = (GridView)sender;
            int rcnt = 0;
            foreach (GridViewRow row in grid.Rows)
                if (row.Visible)
                {
                    //List<string> css = new List<string>();
                    //row.CssClass = String.Join(" ", css.ToArray());

                    string css;
                    if (row.DataItemIndex == grid.EditIndex)
                        css = grid.EditRowStyle.CssClass;
                    else if (row.DataItemIndex == grid.SelectedIndex)
                        css = grid.SelectedRowStyle.CssClass;
                    else
                        css = grid.RowStyle.CssClass;

                    //if (!String.IsNullOrEmpty(css)) css += " ";
                    if (rcnt % 2 == 0)
                        Tools.AddClass(ref css, cssNormRow);
                        //css += cssNormRow;
                    else
                        Tools.AddClass(ref css, cssAltRow);
                        //css += cssAltRow;

                    if (String.IsNullOrEmpty(row.CssClass))
                        row.CssClass = css;
                    else
                        row.CssClass = Tools.AddClass(row.CssClass, css);
                        //row.CssClass += " " + css;

                    rcnt++;
                }
            /*
            GridView grid = (GridView)sender;
            int rcnt = 0;
            foreach (GridViewRow row in grid.Rows)
            {
                if (row.Visible)
                {
                    string css = row.CssClass;
                    if (!String.IsNullOrEmpty(css)) css += " ";
                    if (rcnt % 2 == 0)
                        row.CssClass = css + cssNormRow;
                    else
                        row.CssClass = css + cssAltRow;
                    rcnt++;
                }
            }
            */
        }

        protected static void gv_PreRender(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.Attributes["name"] = "report";
            //----- header -----
            if (grid.HeaderRow != null)
                for (int i = 0; i < grid.HeaderRow.Cells.Count; i++)
                {



                    //20150202 zabezpieczenie przy PL - spr z czego wynika !!!
                    if (cellData != null && i < cellData.Length)
                    {
                        CellData cd = cellData[i];
                        if (cd.typ == tSelect && cd.format == tSelectAll)
                            //grid.HeaderRow.Cells[i].Text = CheckBoxHtml("sAll", null, String.Format("gvRowSelect(this,'{0}');", grid.ClientID + "Selected"), false);
                            grid.HeaderRow.Cells[i].Text = CbSelectHtml(grid, "sAll", false);
                    }
                }
            //----- details -----
            int rcnt = 0;
            foreach (GridViewRow row in grid.Rows)
                if (row.Visible)
                {
                    //List<string> css = new List<string>();
                    //row.CssClass = String.Join(" ", css.ToArray());


                    string css = row.CssClass;
                    if (row.DataItemIndex == grid.EditIndex)
                        Tools.AddClass(ref css, grid.EditRowStyle.CssClass);
                    else if (row.DataItemIndex == grid.SelectedIndex)
                        Tools.AddClass(ref css, grid.SelectedRowStyle.CssClass);
                    else
                        Tools.AddClass(ref css, grid.RowStyle.CssClass);

                    if (rcnt % 2 == 0)
                        Tools.AddClass(ref css, cssNormRow);
                    else
                        Tools.AddClass(ref css, cssAltRow);

                    row.CssClass = css;






                    /*
                    string css;
                    if (row.DataItemIndex == grid.EditIndex)
                        css = grid.EditRowStyle.CssClass;
                    else if (row.DataItemIndex == grid.SelectedIndex)
                        css = grid.SelectedRowStyle.CssClass;
                    else
                        css = grid.RowStyle.CssClass;

                    //if (!String.IsNullOrEmpty(css)) css += " ";
                    if (rcnt % 2 == 0)
                        Tools.AddClass(ref css, cssNormRow);
                        //css += cssNormRow;
                    else
                        Tools.AddClass(ref css, cssAltRow);
                        //css += cssAltRow;

                    if (String.IsNullOrEmpty(row.CssClass))
                        row.CssClass = css;
                    else
                        row.CssClass = Tools.AddClass(row.CssClass, css);
                        //row.CssClass += " " + css;
                    */


                    rcnt++;
                }
            /*
            GridView grid = (GridView)sender;
            int rcnt = 0;
            foreach (GridViewRow row in grid.Rows)
            {
                if (row.Visible)
                {
                    string css = row.CssClass;
                    if (!String.IsNullOrEmpty(css)) css += " ";
                    if (rcnt % 2 == 0)
                        row.CssClass = css + cssNormRow;
                    else
                        row.CssClass = css + cssAltRow;
                    rcnt++;
                }
            }
            */
        }

        protected static void gv_DataBound(object sender, EventArgs e)
        {
        }

        protected static void gv_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            string gridID = e.Command.Parameters["@" + parGridId].Value.ToString();

            SqlDataSource ds;  // przykłądy użycia:
            //ds = Ext.GetPrivateField<SqlDataSource>(sender, "_owner");
            //ds = ((SqlDataSourceView)sender).GetPrivateField<SqlDataSource>("_owner");
            //ds = sender.GetPrivateField<SqlDataSource>("_owner");
            ds = Tools.GetPrivateField(sender, "_owner") as SqlDataSource;

            //FieldInfo fi = typeof(SqlDataSourceView).GetField("_owner", BindingFlags.NonPublic | BindingFlags.Instance);
            //ds = fi.GetValue(sender) as SqlDataSource;

            Label lb = ds.Parent.FindControl(String.Format(lbCount, gridID)) as Label;
            if (lb != null)
                lb.Text = e.AffectedRows.ToString();
        }

        //--------------------------------------------
        protected static void dv_PreRender(object sender, EventArgs e)
        {
            ((DetailsView)sender).Attributes["name"] = "report";

            DetailsView dv = (DetailsView)sender;

            /*
            int rcnt = 0;
            foreach (GridViewRow row in grid.Rows)
                if (row.Visible)
                {
                    //List<string> css = new List<string>();
                    //row.CssClass = String.Join(" ", css.ToArray());

                    string css;
                    if (row.DataItemIndex == grid.EditIndex)
                        css = grid.EditRowStyle.CssClass;
                    else if (row.DataItemIndex == grid.SelectedIndex)
                        css = grid.SelectedRowStyle.CssClass;
                    else
                        css = grid.RowStyle.CssClass;

                    if (!String.IsNullOrEmpty(css)) css += " ";
                    if (rcnt % 2 == 0)
                        css += cssNormRow;
                    else
                        css += cssAltRow;

                    row.CssClass = css;
                    rcnt++;
                }
             */
        }

        protected static void dv_DataBound(object sender, EventArgs e)
        {
            /*
            DetailsView dv = (DetailsView)sender;

            for (int i = 0; i < dv.Rows.Count; i++)
            {
                dv.Rows[i].Cells[0].Text = L.p(dv.Rows[i].Cells[0].Text);
            }
            */
        }
    }
}
