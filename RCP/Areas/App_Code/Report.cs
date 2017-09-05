using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Security.Cryptography;
//using HRRcp.App_Code;

namespace HRRcp.App_Code
{
    public class Report
    {
        //------------------------------------------------------
        public static void ThrowBadParams()
        {
            throw new Exception("Niepoprawne parametry uruchomienia modułu.");
        }
        //------------------------------------------------------
        
        //public static void Show(string page, string progId, string pracId)
        
        
        
        
        //------------------------------------------------------
        public static void MakeLink(LinkButton lbt, string url)
        {
            lbt.PostBackUrl = url;
        }

        public static void MakeLink(LinkButton lbt, string page, string progId)
        {
            lbt.PostBackUrl = page + "?p=" + progId;
        }

        public static string Link(string text, string report, string par)
        {
            return "<a href=" + report + ".aspx?" + par + ">" + text + "</a>";
        }

        public static string Link(string text, string report, /*string progId, */string p1, string p2, string p3)
        {
            return "<a href=" + Url(report + ".aspx", /*progId, */p1, p2, p3) + ">" + text + "</a>";
        }
        
        //------------------------------------------------------
        public static string Url(string page, /*string progId, */string p1, string p2, string p3)
        {
            string par = "";
            //if (!String.IsNullOrEmpty(progId)) par += "&p=" + progId;   // program
            if (!String.IsNullOrEmpty(p1)) par += "&a=" + p1;
            if (!String.IsNullOrEmpty(p2)) par += "&b=" + p2;
            if (!String.IsNullOrEmpty(p3)) par += "&c=" + p3;
            if (!String.IsNullOrEmpty(par)) par = "?" + par.Substring(1);
            return page + par;
        }

        public static string Url(string page, /*string progId, */string par)  // par = p1|p2|p3
        {
            string p1, p2, p3;
            Tools.GetLineParams(par, out p1, out p2, out p3);
            return Url(page, /*progId, */p1, p2, p3);
        }
        //------------
        public static string GetParProgId()
        {
            return HttpContext.Current.Request.QueryString["p"];
        }

        public static string GetParA()
        {
            return HttpContext.Current.Request.QueryString["a"];
        }

        public static string GetParB()
        {
            return HttpContext.Current.Request.QueryString["b"];
        }

        public static string GetParC()
        {
            return HttpContext.Current.Request.QueryString["c"];
        }
        //------------
        public static void Show(string page, /*string progId, */string par) // par = p1|p2|p3
        {
            HttpContext.Current.Response.Redirect(Url(page, /*progId, */par));
        }

        /* to działa:
       CommandArgument='<%# Eval("IdPracownika") + " " + Eval("IdPracownika") %>'>
        */

        //public static bool HandleZoomCommand(object sender, ListViewCommandEventArgs e, string progId)
        public static bool HandleZoomCommand(object sender, CommandEventArgs e/*, string progId*/)
        {
            if (e.CommandName.ToLower().StartsWith("zoom"))
            {
                string page = e.CommandName.Substring(5);   // ZOOM:ReportName (nazwa bez rozszerzenia .aspx !)
                Report.Show(page + ".aspx", /*progId, */(string)e.CommandArgument);
                return true;
            }
            else return false;
        }

        //----- sortowanie --------------------------------------------
        public const string ascSortMarkerChar = "▲"; //↑↓▲▼˄˅
        public const string descSortMarkerChar = "▼";  /**/
        public const string ascSortMarker = "<span class=\"sortmarker\">" + ascSortMarkerChar + "</span>";
        public const string descSortMarker = "<span class=\"sortmarker\">" + descSortMarkerChar + "</span>";  /**/
        /*public const string ascSortMarker = "↑"; 
        public const string descSortMarker = "↓";  /**/
        /*public const string ascSortMarker = "˄"; 
        public const string descSortMarker = "˅";  /**/

        public static string SortButtonId(int col)
        {
            return "LinkButton" + Math.Abs(col).ToString();
        }

        public static void ShowSort(LinkButton lbt, bool set, bool? asc)  // kazdemu usuwam ale ustawiam tylko jak set i asc != null
        {
            if (lbt.Text.StartsWith(ascSortMarker))
                lbt.Text = lbt.Text.Remove(0, ascSortMarker.Length);
            else if (lbt.Text.StartsWith(descSortMarker))
                lbt.Text = lbt.Text.Remove(0, descSortMarker.Length);
            if (set && asc != null)
                if ((bool)asc)
                    lbt.Text = ascSortMarker + lbt.Text;
                else
                    lbt.Text = descSortMarker + lbt.Text;
        }
        
        // do ustawienia początkowego buttona - wywołać w po base.OnPreRender pod warunkiem !IsPostback
        public static void ShowSort(ListView lv, int col, bool? asc) 
        {
            LinkButton lbt = (LinkButton)lv.FindControl(SortButtonId(col));
            if (lbt != null)
            {
                ShowSort(lbt, true, asc);
            }
        }

        // wywołać w ListView1_Sorting; LinkButtony powinny miec nazwy od: "LinkButton1" ...
        public static void ShowSort(object sender, ListViewSortEventArgs e, int ColCount, int defColSorted)  // defColSorted - koryguje asc/desc jesli mam lv posortowane z sql po 1 kolumnie ASC i klikam w nagłówek to chce sortować też ASC - dlatego trzeba skorygować za pierwszym razem, jak nie ma tego robić to przekazać 0
        {
            ListView lv = (ListView)sender;
            for (int i = 1; i <= ColCount; i++)  // ilość kolumn
            {
                /*
                if (i == defColSorted && String.IsNullOrEmpty(lv.SortExpression)) // korygujemy sortowanie po początkowej kolumnie
                    if (e.SortDirection == SortDirection.Ascending)
                        e.SortDirection = SortDirection.Descending;
                    else
                        e.SortDirection = SortDirection.Ascending;
                */ 
                LinkButton lbt = (LinkButton)lv.FindControl(SortButtonId(i));
                if (lbt != null)
                    ShowSort(lbt,
                             e.SortExpression == lbt.CommandArgument,  // klikniety button
                             e.SortDirection == SortDirection.Ascending);
            }
        }

        public static void ShowSort(object sender, ListViewSortEventArgs e, int ColCount, int defColSorted, out int colSorted)  // defColSorted - koryguje asc/desc jesli mam lv posortowane z sql po 1 kolumnie ASC i klikam w nagłówek to chce sortować też ASC - dlatego trzeba skorygować za pierwszym razem, jak nie ma tego robić to przekazać 0
        {
            colSorted = defColSorted;
            ListView lv = (ListView)sender;
            for (int i = 1; i <= ColCount; i++)  // ilość kolumn
            {
                LinkButton lbt = (LinkButton)lv.FindControl(SortButtonId(i));
                if (lbt != null)
                {
                    bool s = e.SortExpression == lbt.CommandArgument; // klikniety button
                    bool asc = e.SortDirection == SortDirection.Ascending;
                    ShowSort(lbt, s, asc);
                    if (s) colSorted = asc ? i : -i;
                }
            }
        }

        /*
        public static void ShowSort(object sender, ListViewSortEventArgs e, int ColCount, int defColSorted, out int colSorted)  // defColSorted - koryguje asc/desc jesli mam lv posortowane z sql po 1 kolumnie ASC i klikam w naglówek to chce sortowac tez ASC - dlatego trzeba skorygowac za pierwszym razem, jak nie ma tego robic to przekazac 0
        {
            colSorted = defColSorted;
            ListView lv = (ListView)sender;
            for (int i = 1; i <= ColCount; i++)  // ilosc kolumn
            {
                LinkButton lbt = (LinkButton)lv.FindControl(SortButtonId(i));
                if (lbt == null && lv.Items.Count == 0 && lv.EmptyDataTemplate != null) // Jezeli nie znalazl buttona, a listview jest w trybie empty template
                {
                    lbt = (LinkButton)lv.Controls[0].FindControl(SortButtonId(i));
                }

                if (lbt != null)
                {
                    bool s = e.SortExpression == lbt.CommandArgument; // klikniety button
                    bool asc = e.SortDirection == SortDirection.Ascending;
                    ShowSort(lbt, s, asc);
                    if (s) colSorted = asc ? i : -i;
                }
            }
        }
        */



        public static string GetSortField(ListView lv, int col)
        {
            int c = Math.Abs(col);
            LinkButton lbt = (LinkButton)lv.FindControl(SortButtonId(col));
            if (lbt != null)
            {
                if (col > 0)
                    return lbt.CommandArgument;
                else
                    return lbt.CommandArgument + " desc";
            }
            else return null;
        }



        /*
        // do ustawienia początkowego buttona - wywołać w po base.OnPreRender
        public static void ShowSort(LinkButton lbt, bool? asc) 
        {
            if (lbt.Text.StartsWith(ascSortMarker) || lbt.Text.StartsWith(descSortMarker))
                lbt.Text = lbt.Text.Remove(0, 1);
            if (asc != null) 
                if ((bool)asc)
                    lbt.Text = ascSortMarker + lbt.Text;
                else
                    lbt.Text = descSortMarker + lbt.Text;
        }

        // wywołać w ListView1_Sorting
        public static void ShowSort(object sender, ListViewSortEventArgs e, int ColCount)
        {
            ListView lv = (ListView)sender;
            for (int i = 1; i <= ColCount; i++)  // ilość kolumn
            {
                string lbtId = "LinkButton" + i.ToString();
                LinkButton lbt = (LinkButton)lv.FindControl(lbtId);
                if (lbt != null)
                    if (e.SortExpression == lbt.CommandArgument)    // klikniety button
                        ShowSort(lbt, e.SortDirection == SortDirection.Ascending);
            }
        }
         */
        //----- SORTOWANIE 2 --------------------------------------
        /*
        public static void sortOnLayoutCreated(ListView lv, int sort)
        {
            Report.ShowSort(lv, sort, sort > 0);
        }

        public static void sortOnSorting(object sender, ListViewSortEventArgs e)
        {
            int sort;
            Report.ShowSort(sender, e, maxSortCol, FDefSortColumn, out sort);
            Session[ID + sesSortId] = sort;  // unikalne co do kontrolki

        }
          
http://www.codeproject.com/Articles/26039/ListView-Header-Sort-Direction-Indicators          
protected override void OnDataBound(EventArgs e)
   2:  {
   3:      if (base.SortExpression.Length == 0)
   4:      {
   5:          if (SortExpressionDefault.Length > 0)
   6:          {
   7:              base.Sort(SortExpressionDefault, SortDirectionDefault);
   8:          }
   9:      }
  10:   
  11:      List<Control> controls = Helpers.GetControlsByType(this, 
                                          typeof(ListViewSortColumnHeader));
  12:      foreach (Control control in controls)
  13:      {
  14:          ListViewSortColumnHeader header = (ListViewSortColumnHeader)control;
  15:          if (header.HasSortDirectionIndicator() == true)
  16:          {
  17:              header.ResetSortDirectionIndicator();
  18:          }
  19:      }
  20:   
  21:      foreach (Control control in controls)
  22:      {
  23:          ListViewSortColumnHeader header = (ListViewSortColumnHeader)control;
  24:          if (header.Key == base.SortExpression)
  25:          {
  26:              header.SetSortDirectionIndicator(base.SortExpression, 
                                                    base.SortDirection);
  27:              break;
  28:          }
  29:      }
  30:   
  31:      base.OnDataBound(e);
  32:  }          
          
          
        */

        //---------------------------------------------------------
        public static string RemoveSortMarkerChar(string txt)
        {
            if (txt.StartsWith(ascSortMarkerChar) || txt.StartsWith(descSortMarkerChar))
                return txt.Remove(0, 1);
            else
                return txt;
        }

        //----- EXPORT EXCEL ----------------------------------------------------
        public delegate List<string> GetRowItemsListFunc(string[] items, int lineNo);  //lineNo 0,1,2...- numer wiersza, od 0, żeby raport mógł np rozróżnić nagłówek

        public static string[] SplitMailTel(string mt)
        {
            string[] ret = new string[2];
            char[] ch = mt.ToCharArray();
            for (int i = mt.Length - 1; i >= 0; i--)
            {
                if (Char.IsLetter(ch[i]))
                {
                    ret[0] = mt.Substring(0, i + 1).Trim();
                    ret[1] = mt.Substring(i + 1).Trim();
                    return ret;
                }
            }
            ret[0] = null;
            ret[1] = null;
            return ret;
        }

        public static string ExcelIntAsText(string i)
        {
            return "=\"" + i + "\"";
        }
        //---------------------------
        private static bool getState(string line, ref int state)   // state moze nie ulec zmianie
        {
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                switch (line)
                {
                    case "[header]": state = 1; break;      // typ przetwarzania danych
                    case "[header1]": state = 11; break;    // span w kolejnych liniach - zamienione na kolejne pola w 1 linii
                    case "[header4]": state = 14; break;    // span w kolejnych liniach - sklejone w 1 linie
                    
                    case "[report]": state = 2; break;
                    case "[report1]": state = 21; break;    // ankieta - skala ocen (2 linie)
                    case "[report2]": state = 22; break;    // ankieta - szkolenia  (wcięcia)
                    case "[report3]": state = 23; break;    // ankieta - podsumowanie, wnioski                    
                    case "[report4]": state = 24; break;    // rcp karta roczna dane pracownika
                    
                    case "[footer2]": state = 32; break;    // ankieta akceptacja
                    case "[footer1]": state = 0; break;
                    case "[footer]": state = 3; break;
                    default: state = 0; break;
                }
                return true;      // znalazł
            }
            else return false;
        }

        private static void addRepLine(List<string> _lines, string[] items, int cnt, GetRowItemsListFunc GetRowItemsList)  // cnt - numer wiersza, od 0, żeby raport mógł np rozróżnić nagłówek
        {
            List<string> _items;

            if (GetRowItemsList != null)
                _items = GetRowItemsList(items, cnt);
            else
                _items = new List<string>(items);// wszystkie kolumny jak leci
          
            string item = String.Join("\t", _items.ToArray());
            _lines.Add(item);
        }

        private static void addBrLine(List<string> _lines, string line)    // 1 item w linii !!!
        {
            string[] items = line.Split('\b');     // img sa "" wiec mam /t caption/r __line1/r __line2/t/r; __ spacje bo brane z html z <br> i jest wcięcie z przodu, którego normalnie nie widać
            for (int i = 0; i < items.Count(); i++)
                _lines.Add(items[i].Trim());
        }

        private static void addBrLineHeader(List<string> _lines, string line)
        {
            string[] items = line.Split('\b');     // img sa "" wiec mam /t caption/r __line1/r __line2/t/r; __ spacje bo brane z html z <br> i jest wcięcie z przodu, którego normalnie nie widać
            for (int i = 0; i < items.Count(); i++)
                if (!String.IsNullOrEmpty(items[i]))
                    _lines.Add(items[i].Trim());    // nie dodaje pustej linii
        }
        //-----------------
        public static string PrepareRepFileName(string title)
        {
            return HttpUtility.UrlEncode(
                title.Replace(':', ' ').Replace('/', ' ').Replace('\\', ' ').Replace('*', ' ').Replace('?', ' '),
                System.Text.Encoding.UTF8).Replace('+', ' ').Replace("%2c", ",");  //UrlEncode zamienia ' ' na '+', więc trzeba odwrotnie ...
        }

        public static void ExportExcel(string rep, string filename, GetRowItemsListFunc GetRowItemsList)  //hidReport.Value;
        {
            int state = 0;                          // sposób przetwarzania danych

            //string[] lines = rep.Split(new char[]{'\r','\n'});  
            string[] lines = rep.Split('\r');       // z js przychodzi cr lf wiec trzeba jeszcze cr obciac z konca lub lf z poczatku w zaleznosci od separatora
            int linesCount = lines.Count();         // jest tez pusta linia na końcu, ale na razie niech zostanie
            for (int i = 1; i < linesCount; i++)    // odcinam lf z początku poza pierwszym wierszem gdzie go nie ma
                lines[i] = lines[i].Substring(1);

            string[] items;
            //string item;
            int cnt = 0;
            List<string> _lines = new List<string>();   // kolejne linie raportu z polami oddzielonymi TAB

            /* realizacja - typ
            string typ = null;
            PracownicyList pl = (PracownicyList)ReportPlaceHolder.FindControl("PracownicyList");
            if (pl != null)
                typ = pl.Typ;
             */

            for (int i = 0; i < linesCount; i++)
            {
                if (getState(lines[i], ref state))
                {
                    if (_lines.Count > 0) _lines.Add("");   // 1 linia rozdzielenia
                    cnt = 0;
                }
                else
                {
                    switch (state)
                    {
                        case 1: // header - rozdzielam na kolejne linie [img][caption][img];  // img sa "" wiec mam /t caption/r __line1/r __line2/t/r; __ spacje bo brane z html z <br> i jest wcięcie z przodu, którego normalnie nie widać
                            addBrLineHeader(_lines, lines[i].Replace("&amp;", "&").Replace("\t", ""));  //D&G - pomyslec nad funcją ktora to trłumaczy !!!
                            break;
                        case 11: // div header  // kolejne linie zawierają itemy, które powiny być obok siebie - elementy <span>, w środku span moze być br wtedy przychodzi \b, osobne pola
                            items = lines[i].Split('\b');
                            for (int k = 0; k < items.Count(); k++)
                                if (cnt == 0 || k > 0)
                                    _lines.Add(items[k]);
                                else
                                    _lines[_lines.Count - 1] += "\t" + items[k];
                            break;
                        case 14: // div header  // kolejne linie zawierają itemy, które powiny być obok siebie - elementy <span>, w środku span moze być br wtedy przychodzi \b, sklejam w 1 pole
                            items = lines[i].Split('\b');
                            for (int k = 0; k < items.Count(); k++)
                                if (cnt == 0 || k > 0)
                                    _lines.Add(items[k]);
                                else
                                    _lines[_lines.Count - 1] += " " + items[k];
                            break;
                        case 2: // report - przepakowuje z mozliwoscia zmiany np. pozycji 
                            items = lines[i].Replace("&nbsp;", " ").Replace("\b", " ").Replace("&amp;", "&").Split('\t');    // tak juz prosciej niz w js
                            addRepLine(_lines, items, cnt, GetRowItemsList);  
                            break;
                        case 21: // report1 - ankieta skala ocen 1 wiersz, wartości w td oddzielone br
                            int idx = _lines.Count;
                            items = lines[i].Split('\t');
                            int itemsCount = items.Count();
                            for (int x = 0; x < itemsCount; x++)
                            {
                                string[] sublines = items[x].Split('\b');
                                int sublinesCount = sublines.Count();
                                for (int y = 0; y < sublinesCount; y++)
                                {
                                    if (idx + y >= _lines.Count)
                                    {
                                        if (!String.IsNullOrEmpty(sublines[y])) // jak pusto to pomijam
                                            _lines.Add(sublines[y]);
                                    }
                                    else
                                        _lines[idx + y] += "\t" + sublines[y];  // a tu nie pomijam jak pusto
                                    /*
                                    if (x == itemsCount - 1)                 // następna celka zeby obcinało poprawnie
                                        _lines[idx + y] += "\t" + " ";
                                     */
                                }
                            }
                            break;
                        case 22: // report2 - ankieta szkolenia
                            idx = _lines.Count;
                            items = lines[i].Replace("&nbsp;", " ").Split('\t');
                            itemsCount = items.Count();
                            for (int x = 0; x < itemsCount; x++)
                            {
                                string[] sublines = items[x].Split('\b');
                                int sublinesCount = sublines.Count();
                                for (int y = 0; y < sublinesCount; y++)
                                {
                                    if (idx + y >= _lines.Count)    // nowa linia
                                        if (y == 0)                 // pierwszy element z linii z br
                                            _lines.Add(sublines[y]);
                                        else                        // kolejne elementy - tu opis szkolenia, mozna by jeszcze sprawdzać x 
                                            _lines.Add("\t    " + sublines[y] + "\t ");
                                    else                            // istniejące linie - dokładam 
                                        _lines[idx + y] += "\t" + sublines[y];
                                }
                            }
                            break;
                        case 23: // report3 - ankieta podsumowanie, uwagi
                            List<string> _items = new List<string>();
                            items = lines[i].Split('\t');
                            itemsCount = items.Count();
                            for (int x = 0; x < itemsCount; x++)
                            {
                                _items.Add("\"" + items[x].Replace("\"", "“").Replace("\b", "\n") + "\"");
                            }
                            string item = String.Join("\t", _items.ToArray());
                            _lines.Add(item);
                            break;

                        case 24: // report4 - RCP-karta roczna tabela dane pracownika
                            items = lines[i].Replace("&nbsp;", " ").Replace("\b", "").Split('\t');    // \b - br z th dni, nic nie ma w 2 linii
                            addRepLine(_lines, items, cnt, GetRowItemsList);  
                            break;

                        case 3:     // footer
                            addBrLine(_lines, lines[i]);
                            break;
                        case 32:    // ankieta - akceptacja
                            _items = new List<string>();
                            items = lines[i].Replace("&nbsp;", "").Split('\t');
                            for (int k = 0; k < items.Count(); k++)
                            {
                                int p = items[k].IndexOf(':');
                                if (p == -1)
                                    _items.Add(items[k]);
                                else
                                {
                                    _items.Add(items[k].Substring(0, p + 1));
                                    _items.Add(items[k].Substring(p + 1).Trim());
                                }
                            }
                            item = String.Join("\t", _items.ToArray());
                            _lines.Add(item);
                            break;
                        default:
                            _lines.Add(lines[i]);
                            break;
                    }
                    cnt++;
                }
            }
            rep = String.Join("\r\n", _lines.ToArray());

            if (String.IsNullOrEmpty(filename))
                filename = "RCP-Raport";
            else
                filename = PrepareRepFileName(filename);
                    /*
                    HttpUtility.UrlEncode(
                        filename.Replace(':',' ').Replace('/',' ').Replace('\\',' ').Replace('*',' ').Replace('?',' '), 
                        System.Text.Encoding.UTF8).Replace('+', ' ').Replace("%2c", ",");  //UrlEncode zamienia ' ' na '+', więc trzeba odwrotnie ...
                    */
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
            HttpContext.Current.Response.Charset = "unicode";

//            this.EnableViewState = false;

            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}{1}\"", filename,
                //".xls"
                    ".csv"     // nowy excel wyświetla warninga jak mu się format nie zgadza
                    ));

            HttpContext.Current.Response.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
            HttpContext.Current.Response.Write(rep);
            HttpContext.Current.Response.End();
        }
        //------------------------------------------------------------------------------
        public static void ClearControls(Control control)
        {
            for (int i = control.Controls.Count - 1; i >= 0; i--)
            {
                ClearControls(control.Controls[i]);
            }
            if (control.ID == "control")
                control.Parent.Controls.Remove(control);
            else if (control.ID == "h_control")
                control.Parent.Controls.Remove(control);
        }

        public static void ExportExcel(string filename, ListView lv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Charset = "utf-8";
            //this.EnableViewState = false;
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}.xls\"", filename));

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            ClearControls(lv);

            lv.RenderControl(htmlTextWriter);
            HttpContext.Current.Response.Write(stringWriter.ToString());
            HttpContext.Current.Response.End();
        }

        public static void ExportCSV(string filename, string sql, string header, string footer)
        {
            //DataSet ds = Base.getDataSet("select * from Teksty");
            DataSet ds = Base.getDataSet(sql);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
            HttpContext.Current.Response.Charset = "unicode";
            //this.EnableViewState = false;
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}.csv\"", filename));
            StringWriter stringWriter = new StringWriter();

            if (!String.IsNullOrEmpty(header)) stringWriter.WriteLine(header);
            string d;
            string line = null;
            int cnt = ds.Tables[0].Columns.Count;
            for (int i = 0; i < cnt; i++)
            {
                d = Tools.CtrlToText(ds.Tables[0].Columns[i].ToString());
                if (i == 0)
                    line = d;
                else
                    line += Tools.TAB + d;
            }
            stringWriter.WriteLine(line);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < cnt; i++)
                {
                    d = Tools.CtrlToText(dr[i].ToString());
                    if (i == 0)
                        line = d;
                    else
                        line += Tools.TAB + d;
                }
                stringWriter.WriteLine(line);
            }
            if (!String.IsNullOrEmpty(footer)) stringWriter.WriteLine(footer);

            HttpContext.Current.Response.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
            HttpContext.Current.Response.Write(stringWriter.ToString());
            HttpContext.Current.Response.End();
        }

        public static string PrepareColumnTitle(string title)
        {
            if (!String.IsNullOrEmpty(title))
            {
                string[] hh = title.Split('|');
                string[] tt = hh[0].Split(':');
                return tt[0];
            }
            return null;
        }

        public static string PrepareColumnTitle(string title, out bool hide)
        {
            hide = false;
            if (!String.IsNullOrEmpty(title))
            {
                string[] hh = title.Split('|');
                string[] tt = hh[0].Split(':');
                if (tt.Length > 1)
                {
                    string[] cc = tt[1].Split(';');
                    if (cc.Length > 1)
                        hide = cc[0] == "-" || cc[1].ToLower() == "control";
                    else
                        hide = cc[0] == "-";    // :-
                }
                return tt[0];
            }
            return null;
        }

        public static string PrepareColumnTitle(string title, out string typ, out string css)
        {
            typ = null;
            css = null;
            if (!String.IsNullOrEmpty(title))
            {
                string[] hh = title.Split('|');
                string[] tt = hh[0].Split(':');
                if (tt.Length > 1)
                {
                    string[] cc = tt[1].Split(';');
                    typ = cc[0];
                    if (cc.Length > 1)
                        css = cc[1];
                }
                return tt[0];
            }
            return null;
        }

        public static void ExportCSV(string filename, string sql, string header, string footer, bool prepareColumns)   // wycian po | - do raportów
        {
            //DataSet ds = Base.getDataSet("select * from Teksty");
            DataSet ds = Base.getDataSet(sql);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
            HttpContext.Current.Response.Charset = "unicode";
            //this.EnableViewState = false;
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}.csv\"", filename));
            StringWriter stringWriter = new StringWriter();

            if (!String.IsNullOrEmpty(header)) stringWriter.WriteLine(header);
            string d;
            string line = null;
            int cnt = ds.Tables[0].Columns.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (prepareColumns)
                    d = Tools.CtrlToText(PrepareColumnTitle(ds.Tables[0].Columns[i].ToString()));
                else
                    d = Tools.CtrlToText(ds.Tables[0].Columns[i].ToString());
                if (i == 0)
                    line = d;
                else
                    line += Tools.TAB + d;
            }
            stringWriter.WriteLine(line);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < cnt; i++)
                {
                    d = Tools.CtrlToText(dr[i].ToString());
                    if (i == 0)
                        line = d;
                    else
                        line += Tools.TAB + d;
                }
                stringWriter.WriteLine(line);
            }
            if (!String.IsNullOrEmpty(footer)) stringWriter.WriteLine(footer);

            HttpContext.Current.Response.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
            HttpContext.Current.Response.Write(stringWriter.ToString());
            HttpContext.Current.Response.End();
        }
        //---------------------------
        public static string ToXls(string v)
        {
            if (v.StartsWith("0"))
                return String.Format("=\"{0}\"", v);
            else
                return String.Format("\"{0}\"", v);
        }

        public static string ToXls(string typ, string v)
        {
            string t1 = null;
            string t2 = null;
            if (!String.IsNullOrEmpty(typ))
            {
                if (typ.Length > 0) t1 = typ.Substring(0, 1);
                if (typ.Length > 1) t2 = typ.Substring(1, 1);
            }
            switch (t1)
            {
                case "N":
                    return v;
                case "D":
                    if (t2 == "T")
                        return String.Format("=\"{0}\"", v);
                    else
                        return String.Format("=\"{0}\"", Tools.Substring(v, 0, 10));
                default:
                    return String.Format("=\"{0}\"", v);
            }
        }

        public static void ExportCSV(string filename, SqlDataSource sqlDs, string header, string footer, bool prepareColumns, bool showHiddenColumns)   // wycian po | - do raportów
        {
            ExportCSV(filename, sqlDs, header, footer, prepareColumns, showHiddenColumns, false);
        }

/* przykład użycia: do csv będzie bez --:CUT do :CSV, ale z odkomentowanym :CSV
select
--:CUT
T.Symbol, C.*, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as IdPracownika, P.KadryId 
/*:CSV
P.Nazwisko + ' ' + P.Imie [Pracownik],
P.KadryId [Nr ewid.],
T.Symbol, 
--T.Nazwa [Nazwa typu], 
C.NazwaCertyfikatu [Nazwa szkolenia], 
C.Numer [Numer zaświadczenia],
C.DodatkoweWarunki [Dodatkowe warunki],
C.DataRozpoczecia [Data rozpoczęcia],
C.DataZakonczenia [Data zakończenia],
C.DataWaznosci [Data ważności],
C.Uwagi [Uwagi]
* /
from ...
         */

        private static bool SqlToCsv(ref string sql)
        {
            string sqlL = sql.ToUpper();

            int p2 = sqlL.IndexOf(":CSV");
            if (p2 != -1)
            {
                int p1 = sqlL.IndexOf("--:CUT");
                if (p1 == -1)
                {
                    p1 = sqlL.IndexOf("SELECT");
                    if (p1 != -1) p1 += 6;
                }
                if (p1 != -1 && p2 != -1)
                {
                    p2 += 4;
                    sql = sql.Remove(p1, p2 - p1 + 1);
                    int p3 = sql.IndexOf("*/", p1);
                    if (p3 != -1)
                        sql = sql.Remove(p3, 2);
                    return true;
                }
            }
            return false;
        }

        public static void ExportCSV(string filename, SqlDataSource sqlDs, string header, string footer, bool prepareColumns, bool showHiddenColumns, bool replaceSQL)   // wycian po | - do raportów
        {
            if (replaceSQL)
            {
                string sql = sqlDs.SelectCommand;

                if (SqlToCsv(ref sql))   // może być kilka sekcji ?
                    SqlToCsv(ref sql);


                //string sqlL = sql.ToUpper();

                //int p1 = sqlL.IndexOf(":CUT");
                //if (p1 != -1) p1 += 4;
                //else
                //{
                //    p1 = sqlL.IndexOf("SELECT");
                //    if (p1 != -1) p1 += 6;
                //}
                //int p2 = sqlL.IndexOf(":CSV");
                //if (p1 != -1 && p2 != -1)
                //{
                //    p2 += 4;
                //    sql = sql.Remove(p1, p2 - p1 + 1);
                //    int p3 = sql.IndexOf("*/", p1);
                //    if (p3 != -1)
                //        sql = sql.Remove(p3, 2);
                //}

                sqlDs.SelectCommand = sql;
            }

            DataView dv = (DataView)sqlDs.Select(DataSourceSelectArguments.Empty);
            //DataView dv = (DataView)sqlDs.Select(sqlDs.SelectParameters);
            var dt = dv.ToTable();

            StringWriter sw = StartExportCSV(filename);

            if (!String.IsNullOrEmpty(header)) sw.WriteLine(header);
            string d;
            string line = null;
            int cnt = dt.Columns.Count;
            string[] ctyp = new string[cnt];
            int c = 0;
            //----- nagłówek -----
            for (int i = 0; i < cnt; i++)
            {
                bool show;
                if (prepareColumns)
                {
                    string typ, css;
                    d = Tools.CtrlToText(PrepareColumnTitle(dt.Columns[i].ToString(), out typ, out css));
                    string t = css == "control" && String.IsNullOrEmpty(typ) ? "-" : typ;
                    ctyp[i] = t;
                    show = t != "-";
                }
                else
                {
                    d = Tools.CtrlToText(dt.Columns[i].ToString());
                    show = true;
                }
                if (showHiddenColumns || show)
                {
                    if (c == 0)
                        line = ToXls(d);
                    else
                        line += Tools.TAB + ToXls(d);
                    c++;
                }
            }
            sw.WriteLine(line);
            //----- dane -----
            foreach (DataRow dr in dt.Rows)
            {
                line = null;
                c = 0;
                for (int i = 0; i < cnt; i++)
                {
                    string t = ctyp[i];
                    if (showHiddenColumns || t != "-")
                    {
                        d = Tools.CtrlToText(dr[i].ToString());
                        if (c == 0)
                            line = ToXls(t, d);
                        else
                            line += Tools.TAB + ToXls(t, d);
                        c++;
                    }
                }
                sw.WriteLine(line);
            }
            if (!String.IsNullOrEmpty(footer)) sw.WriteLine(footer);

            EndExportCSV(sw);
        }










        public static void ExportCSV(string filename, string header, string sepline, string footer, bool prepareColumns, bool showHiddenColumns, bool replaceSQL, params SqlDataSource[] dslist)   // wycian po | - do raportów
        {
            StringWriter sw = StartExportCSV(filename);
            if (!String.IsNullOrEmpty(header)) sw.WriteLine(header);
                        
            foreach (SqlDataSource sds in dslist)
            {
                if (sds == null)
                {
                    sw.WriteLine(sepline);
                    continue;
                }
                if (replaceSQL)
                {
                    string sql = sds.SelectCommand;
                    if (SqlToCsv(ref sql))   // może być kilka sekcji ?
                        SqlToCsv(ref sql);
                    sds.SelectCommand = sql;
                }
                DataView dv = (DataView)sds.Select(DataSourceSelectArguments.Empty);
                var dt = dv.ToTable();

                string d;
                string line = null;
                int cnt = dt.Columns.Count;
                string[] ctyp = new string[cnt];
                int c = 0;
                //----- nagłówek -----
                for (int i = 0; i < cnt; i++)
                {
                    bool show;
                    if (prepareColumns)
                    {
                        string typ, css;
                        d = Tools.CtrlToText(PrepareColumnTitle(dt.Columns[i].ToString(), out typ, out css));
                        string t = css == "control" && String.IsNullOrEmpty(typ) ? "-" : typ;
                        ctyp[i] = t;
                        show = t != "-";
                    }
                    else
                    {
                        d = Tools.CtrlToText(dt.Columns[i].ToString());
                        show = true;
                    }
                    if (showHiddenColumns || show)
                    {
                        if (c == 0)
                            line = ToXls(d);
                        else
                            line += Tools.TAB + ToXls(d);
                        c++;
                    }
                }
                sw.WriteLine(line);
                //----- dane -----
                foreach (DataRow dr in dt.Rows)
                {
                    line = null;
                    c = 0;
                    for (int i = 0; i < cnt; i++)
                    {
                        string t = ctyp[i];
                        if (showHiddenColumns || t != "-")
                        {
                            d = Tools.CtrlToText(dr[i].ToString());
                            if (c == 0)
                                line = ToXls(t, d);
                            else
                                line += Tools.TAB + ToXls(t, d);
                            c++;
                        }
                    }
                    sw.WriteLine(line);
                }
            }
            if (!String.IsNullOrEmpty(footer)) sw.WriteLine(footer);
            EndExportCSV(sw);
        }

        
        
        
        
        
        




        
        /*
        public static void ExportCSV(string filename, SqlDataSource sqlDs, string header, string footer, bool prepareColumns, bool showHiddenColumns)   // wycian po | - do raportów
        {
            DataView dv = (DataView)sqlDs.Select(DataSourceSelectArguments.Empty);
            var dt = dv.ToTable();

            StringWriter sw = StartExportCSV(filename);

            if (!String.IsNullOrEmpty(header)) sw.WriteLine(header);
            string d;
            string line = null;
            int cnt = dt.Columns.Count;
            string[] ctyp = new string[cnt];
            int c = 0;
            //----- nagłówek -----
            for (int i = 0; i < cnt; i++)
            {
                bool show;
                if (prepareColumns)
                {
                    string typ, css;
                    d = Tools.CtrlToText(PrepareColumnTitle(dt.Columns[i].ToString(), out typ, out css));
                    string t = css == "control" && String.IsNullOrEmpty(typ) ? "-" : typ;
                    ctyp[i] = t;
                    show = t != "-";
                }
                else
                {
                    d = Tools.CtrlToText(dt.Columns[i].ToString());
                    show = true;
                }
                if (showHiddenColumns || show)
                {
                    if (c == 0)
                        line = ToXls(d);
                    else
                        line += Tools.TAB + ToXls(d);
                    c++;
                }
            }
            sw.WriteLine(line);
            //----- dane -----
            foreach (DataRow dr in dt.Rows)
            {
                line = null;
                c = 0;
                for (int i = 0; i < cnt; i++)
                {
                    string t = ctyp[i];
                    if (showHiddenColumns || t != "-")
                    {
                        d = Tools.CtrlToText(dr[i].ToString());
                        if (c == 0)
                            line = ToXls(t, d);
                        else
                            line += Tools.TAB + ToXls(t, d);
                        c++;
                    }
                }
                sw.WriteLine(line);
            }
            if (!String.IsNullOrEmpty(footer)) sw.WriteLine(footer);

            EndExportCSV(sw);
        }
        */
        //---------------------------
        public static void ExportCSV(string filename, DataSet ds)   //params string[] sql) 
        {
            //DataSet ds = Base.getDataSet("select * from Teksty");
            //DataSet ds = Base.getDataSet(sql);

            StringWriter sw = StartExportCSV(filename);
            foreach (DataTable dt in ds.Tables)
            {
                string line = null;
                int cnt = dt.Columns.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        string d = Tools.CtrlToText(dr[i].ToString());
                        if (i == 0)
                            line = d;
                        else
                            line += Tools.TAB + d;
                    }
                    sw.WriteLine(line);
                }
            }
            EndExportCSV(sw);
        }

        public static void ExportCSV(string filename, params SqlDataSource[] dslist)   //params string[] sql) 
        {
            ExportCSV(filename, null, null, null, true, false, true, dslist);
        }

        //---------------------------
        public static StringWriter StartExportCSV(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                filename = "RCP-Raport";
            else
                filename = HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8).Replace('+', ' ').Replace("%2c", ",");  //UrlEncode zamienia ' ' na '+', więc trzeba odwrotnie ...
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
            HttpContext.Current.Response.Charset = "unicode";
            //this.EnableViewState = false;
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}.csv\"", filename));
            return new StringWriter();
        }

        public static void AddLineCSV(StringWriter sw, string line)
        {
            sw.WriteLine(line);
        }

        public static void EndExportCSV(StringWriter sw)
        {
            HttpContext.Current.Response.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        //--------------------------------
        public static StringWriter StartExportTXT(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                filename = "RCP-Raport";
            else
                filename = HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8).Replace('+', ' ').Replace("%2c", ",");  //UrlEncode zamienia ' ' na '+', więc trzeba odwrotnie ...
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.ContentEncoding = Encoding.Unicode;
            HttpContext.Current.Response.Charset = "unicode";
            //this.EnableViewState = false;
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}.txt\"", filename));
            return new StringWriter();
        }

        public static void EndExportTXT(StringWriter sw)
        {
            HttpContext.Current.Response.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        //----------------------------------------------------
        public static string EncryptQueryString(string inputText, string key, string salt)
        {
            byte[] plainText = Encoding.UTF8.GetBytes(inputText);

            using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
            {
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));
                using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.FlushFinalBlock();
                            string base64 = Convert.ToBase64String(memoryStream.ToArray());

                            // Generate a string that won't get screwed up when passed as a query string.
                            string urlEncoded = HttpUtility.UrlEncode(base64);
                            return urlEncoded;
                        }
                    }
                }
            }
        }

        public static string DecryptQueryString(string inputText, string key, string salt)
        {
            try
            {
                if (!String.IsNullOrEmpty(inputText))   //20150110
                {
                    byte[] encryptedData = Convert.FromBase64String(inputText);
                    PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));

                    using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                    {
                        using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                        {
                            using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    byte[] plainText = new byte[encryptedData.Length];
                                    cryptoStream.Read(plainText, 0, plainText.Length);
                                    string utf8 = Encoding.UTF8.GetString(plainText);
                                    return utf8;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Log.PARAMS, String.Format("Report.DecryptQueryString('{0}')", inputText), ex.Message);
            }
            return null;
        }

        /*
        public const int paString = 1;
        public const int paDate = 2;
        public const int paInt = 3;

        public static bool CheckParameters(params object[] list)
        {
            for (object p in list)
                if (p is int)
                {
                }
                else if (p is string)
                {
                }

            return true;
        }
         */
        public static bool CheckParametersCount(int min, int max)
        {
            int cnt = HttpContext.Current.Request.QueryString.Count;
            return min <= cnt && cnt <= max;
        }

        public static bool CheckParameterString(int no, int min, int max)
        {
            string p = Tools.GetStr(HttpContext.Current.Request.QueryString[no-1]);
            int len = String.IsNullOrEmpty(p) ? 0 : p.Length;
            return min <= len && len <= max;
        }

        public static bool CheckParameterData(int no)
        {
            string p = Tools.GetStr(HttpContext.Current.Request.QueryString[no-1]);
            return Tools.DateIsValid(p);
        }

        public static bool CheckParameterInt(int no, int min, int max)
        {
            string p = Tools.GetStr(HttpContext.Current.Request.QueryString[no-1]);
            int i;
            bool ok = Int32.TryParse(p, out i);
            if (ok)
                return min <= i && i <= max;
            else
                return false;
        }

        public static bool CheckParameterToken(int no, string tokens)   // "a|b|..."
        {
            const string tt = "|";
            string t = tt + tokens + tt;
            string p = Tools.GetStr(HttpContext.Current.Request.QueryString[no-1]);
            return t.Contains(tt + p + tt);
        }

        public static Stream ExportStreamCSV(string sql, string header)
        {
            //DataSet ds = Base.getDataSet("select * from Teksty");
            DataSet ds = Base.getDataSet(sql);

            StringWriter stringWriter = new StringWriter();
            stringWriter.Write((char)65279);

            if (!String.IsNullOrEmpty(header)) stringWriter.WriteLine(header);
            string d;
            string line = null;
            int cnt = ds.Tables[0].Columns.Count;
            for (int i = 0; i < cnt; i++)
            {
                d = Tools.CtrlToText(ds.Tables[0].Columns[i].ToString());
                if (i == 0)
                    line = d;
                else
                    line += Tools.TAB + d;
            }
            stringWriter.WriteLine(line);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < cnt; i++)
                {
                    d = Tools.CtrlToText(dr[i].ToString());
                    if (i == 0)
                        line = d;
                    else
                        line += Tools.TAB + d;
                }
                stringWriter.WriteLine(line);
            }
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
            writer.Write(stringWriter);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static void WriteStreamToFile(Stream s, string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                if (s is MemoryStream) ((MemoryStream)s).WriteTo(file);
                else
                    throw new NotImplementedException();   // sukcesywnie dodawać
            }
        }

    }
}
