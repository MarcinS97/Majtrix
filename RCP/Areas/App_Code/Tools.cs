using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Data;
using System.Data.SqlClient;

//using System.Configuration;
//using System.Collections;
//using System.ComponentModel;
//using System.Drawing;
//using HRApp.App_Code;

using System.DirectoryServices;

using System.Web.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Drawing;

using System.Reflection;

using System.Globalization;
using System.Threading;
using System.Web.Security;

using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using AjaxControlToolkit;

//wersja programu - zmienić w Properties.AssemblyInfo.cs -> AssemblyVersion


namespace HRRcp.App_Code
{
    public static class DateTimeExtensions
    {
        public static DateTime bom(this DateTime dt)
        {
            return Tools.bom(dt);
        }

        public static DateTime eom(this DateTime dt)
        {
            return Tools.eom(dt);
        }

        public static DateTime bow(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime boy(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime eoy(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31);
        }

        //------------------------
        public static string ToStringDb(this DateTime dt)  // wywala błąd: The call is ambiguous between the following methods or properties type extension
        {
            return db.strParam(Tools.DateToStrDb(dt));
        }
    }
    //----------------------------------------------------
    public static class Ext
    {
        //http://www.codeproject.com/Articles/80343/Accessing-private-members
        public static T GetPrivateField<T>(this object obj, string name)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            FieldInfo field = type.GetField(name, flags);
            return (T)field.GetValue(obj);
        }

        public static T GetPrivateProperty<T>(this object obj, string name)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            PropertyInfo field = type.GetProperty(name, flags);
            return (T)field.GetValue(obj, null);
        }
        //----
        public static void SetPrivateField(this object obj, string name, object value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            FieldInfo field = type.GetField(name, flags);
            field.SetValue(obj, value);
        }

        public static void SetPrivateProperty(this object obj, string name, object value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            PropertyInfo field = type.GetProperty(name, flags);
            field.SetValue(obj, value, null);
        }

        public static T CallPrivateMethod<T>(this object obj, string name, params object[] param)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(name, flags);
            return (T)method.Invoke(obj, param);
        }
        //-----
        public static bool IsAny<T>(this object id, params T[] list)
        {
            return list.Any(a => a.Equals(id));
        }
        //-----
        /* jako funkcja rozszerzająca zwraca błąd, że istnieje w 2 miejscach, jezeli wywołanie jest z cs w katalogu App_Code ...
        public static void WriteToFile2(this Stream s, string filename)   
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                if (s is MemoryStream) ((MemoryStream)s).WriteTo(file);
                else
                    throw new NotImplementedException();
            }
        }
        */
    }

    //----------------------------------------------------
    public class Tools
    {
        public static Color warnColor = Color.Red;
            
        public static string[] DayName = { "", "Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota" };
        public static string[] MonthName = { "", "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
        //public static string[] DayShortName = { "", "Ni", "Po", "Wt", "Śr", "Cz", "Pi", "So" };
        

        public static class ListViewMode
        {
            public const int Default = 0;
            public const int Bootstrap = 1337;
        }


        public Tools()
        {
        }

        //------------------------------
        // podmienia znaczniki zeby nie byly interpretowane dla wszystkich kontrolek typu Label
        // do bazy zapisujemy ze znacznikami, textbox potrafi je wyswietlic, labele interpretuja - wiec musimy wywalić ...
        // wywołanie: ListView_ItemDataBound (np. PracNotesControl.ascx.cs)
        // wymaga użycia w aspx formatki: <%@ Page ... ValidateRequest="false" zeby mozna bylo do bazy zapisać
        public static void HtmlEncodeControls(ControlCollection cc)
        {
            for (int i = 0; i < cc.Count; i++)
                if (cc[i] is Label)
                {
                    Label lb = (Label)cc[i];
                    //lb.Text = HttpContext.Current.Server.HtmlEncode(lb.Text);
                    lb.Text = HttpUtility.HtmlEncode(lb.Text);
                }
        }

        public static void HtmlEncodeControl(ListViewItem li, string cname)    // wywołanie j.w., parametr e.Item 
        {
            Label lb = (Label)li.FindControl(cname);
            if (lb != null)
                //lb.Text = HttpContext.Current.Server.HtmlEncode(lb.Text);
                lb.Text = HttpUtility.HtmlEncode(lb.Text);
        }

        public static string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        /*  nie moze byc zadeklarowana wewnatrz "generic class" ...
        public static string HtmlEncode(this string s)   // rozszerzenie funckcji string - wywołanie zmienna.HtmlEncode
        { 
            return HttpUtility.HtmlEncode(s); 
        } 
        */

        /*
            ListViewDataItem di = (ListViewDataItem)e.Item;
            DataRowView drv = (DataRowView)di.DataItem;
            int cnt = drv.DataView.Table.Columns.Count;
            for (int i = 0; i < cnt; i++)
                drv[i] = Server.HtmlEncode(drv[i].ToString());
            */

        /*
        Label lb = (Label)e.Item.FindControl("Label1");
        if (lb != null)
            lb.Text = Server.HtmlEncode(lb.Text);
        */

        /*
        <asp:Label ID="NotatkaLabel" runat="server" Text='<%# Server.HtmlEncode((string)Eval("Notatka")) %>' />
        */

        //------------------------------
        public static string RemoveHtmlTags(string html)
        {
            if (String.IsNullOrEmpty(html))
                return null;
            else
                return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
            //return Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        public const string TAB = "\t";
        public const char chTAB = (char)9;
        public const char CR = (char)13;
        public const char LF = (char)10;
        public const string CRLF = "\r\n";

        public static string CtrlToText(string s)
        {
            string d = s.Replace(CRLF, "\\n");
            d = d.Replace(LF.ToString(), "\\n");
            d = d.Replace(TAB, "\\t");
            return d;
        }

        public static string TextToCtrl(string s)
        {
            string d = s.Replace("\\n", CRLF);
            d = d.Replace("\\t", TAB);
            d = d.Replace("'", "''");
            return d;
        }

        public static string AddCRLF(string s)
        {
            if (!String.IsNullOrEmpty(s))
                return s + CRLF;
            else
                return s;
        }

        public static string RemoveDblSpaces(string s)
        {
            return Regex.Replace(s, @"\s+", " ");
        }

        //------------------------------
        public static Literal AddLiteral(PlaceHolder ph, string literal)
        {
            Literal lt = new Literal();
            lt.Text = literal;
            ph.Controls.Add(lt);
            return lt;
        }

        public static void AddControl(PlaceHolder ph, string literalBefore, Control c, string literalAfter)
        {
            if (!String.IsNullOrEmpty(literalBefore))
                AddLiteral(ph, literalBefore);
            ph.Controls.Add(c);
            if (!String.IsNullOrEmpty(literalAfter))
                AddLiteral(ph, literalAfter);
        }
        //---------------------------------------------------
        public const char LineParamSeparator = '|';

        public static string[] ResizeStringArray(ref string[] sArray, int newSize)
        {
            int oldSize = sArray.Length;
            string[] newArray = new string[newSize];
            int size = System.Math.Min(oldSize, newSize);
            if (size > 0)
                System.Array.Copy(sArray, newArray, size);
            sArray = newArray;
            return sArray;
        }

        public static string SetLineParam(ref string lineParam, int no, string value)  // no: 0..
        {
            if (lineParam == null) lineParam = "";
            string[] values = lineParam.Split(LineParamSeparator);
            if (no >= 0)
            {
                if (no >= values.Length)
                    ResizeStringArray(ref values, no + 1);
                values[no] = value;
                lineParam = String.Join(LineParamSeparator.ToString(), values);
            }
            return lineParam;
        }

        public static string SetLineParams(int count, string p1, string p2, string p3, string p4, string p5, string p6)
        {
            string[] values = new string[count];
            if (0 < count) values[0] = p1;
            if (1 < count) values[1] = p2;
            if (2 < count) values[2] = p3;
            if (3 < count) values[3] = p4;
            if (4 < count) values[4] = p5;
            if (5 < count) values[5] = p6;
            return String.Join(LineParamSeparator.ToString(), values);
        }

        public static string SetLineParams(int count, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10)
        {
            string[] values = new string[count];
            if (0 < count) values[0] = p1;
            if (1 < count) values[1] = p2;
            if (2 < count) values[2] = p3;
            if (3 < count) values[3] = p4;
            if (4 < count) values[4] = p5;
            if (5 < count) values[5] = p6;
            if (6 < count) values[6] = p7;
            if (7 < count) values[7] = p8;
            if (8 < count) values[8] = p9;
            if (9 < count) values[9] = p10;
            return String.Join(LineParamSeparator.ToString(), values);
        }

        public static string SetLineParams(params string[] par)
        {
            int count = par.Length; 
            string[] values = new string[count];
            for (int i = 0; i < count; i++)
                values[i] = par[i];
            return String.Join(LineParamSeparator.ToString(), values);
        }

        public static string GetLineParam(string lineParam, int no)  // 0, 1, ...
        {
            string[] values = lineParam.Split(LineParamSeparator);
            if (no >= 0 && no < values.Length)
                return values[no];
            else
                return null;
        }

        public static void GetLineParams(string lineParam, out string p1, out string p2, out string p3, out string p4, out string p5)
        {
            string[] values = lineParam.Split(LineParamSeparator);
            int count = values.Length;
            if (0 < count) p1 = values[0]; else p1 = null;
            if (1 < count) p2 = values[1]; else p2 = null;
            if (2 < count) p3 = values[2]; else p3 = null;
            if (3 < count) p4 = values[3]; else p4 = null;
            if (4 < count) p5 = values[4]; else p5 = null;
        }

        public static void GetLineParams(string lineParam, out string p1, out string p2, out string p3, out string p4, out string p5, out string p6)
        {
            string[] values = lineParam.Split(LineParamSeparator);
            int count = values.Length;
            if (0 < count) p1 = values[0]; else p1 = null;
            if (1 < count) p2 = values[1]; else p2 = null;
            if (2 < count) p3 = values[2]; else p3 = null;
            if (3 < count) p4 = values[3]; else p4 = null;
            if (4 < count) p5 = values[4]; else p5 = null;
            if (5 < count) p6 = values[5]; else p6 = null;
        }

        public static void GetLineParams(string lineParam, out string p1, out string p2, out string p3, out string p4, out string p5, out string p6, out string p7)
        {
            string[] values = lineParam.Split(LineParamSeparator);
            int count = values.Length;
            if (0 < count) p1 = values[0]; else p1 = null;
            if (1 < count) p2 = values[1]; else p2 = null;
            if (2 < count) p3 = values[2]; else p3 = null;
            if (3 < count) p4 = values[3]; else p4 = null;
            if (4 < count) p5 = values[4]; else p5 = null;
            if (5 < count) p6 = values[5]; else p6 = null;
            if (6 < count) p7 = values[6]; else p7 = null;
        }

        public static void GetLineParams(string lineParam, out string p1, out string p2, out string p3, out string p4, out string p5, out string p6, out string p7, out string p8, out string p9, out string p10)
        {
            string[] values = lineParam.Split(LineParamSeparator);
            int count = values.Length;
            if (0 < count) p1 = values[0]; else p1 = null;
            if (1 < count) p2 = values[1]; else p2 = null;
            if (2 < count) p3 = values[2]; else p3 = null;
            if (3 < count) p4 = values[3]; else p4 = null;
            if (4 < count) p5 = values[4]; else p5 = null;
            if (5 < count) p6 = values[5]; else p6 = null;
            if (6 < count) p7 = values[6]; else p7 = null;
            if (7 < count) p8 = values[7]; else p8 = null;
            if (8 < count) p9 = values[8]; else p9 = null;
            if (9 < count) p10= values[9]; else p10= null;
        }

        public static string[] GetLineParams(string lineParam)
        {
            return lineParam.Split(LineParamSeparator);
        }

        public static void GetLineParams(string lineParam, out string p1, out string p2)
        {
            if (lineParam != null)
            {
                string[] values = lineParam.Split(LineParamSeparator);
                int count = values.Length;
                if (0 < count) p1 = values[0]; else p1 = null;
                if (1 < count) p2 = values[1]; else p2 = null;
            }
            else
            {
                p1 = null;
                p2 = null;
            }
        }

        public static void GetLineParams(string lineParam, out string p1, out string p2, out string p3)
        {
            string[] values = lineParam.Split(LineParamSeparator);
            int count = values.Length;
            if (0 < count) p1 = values[0]; else p1 = null;
            if (1 < count) p2 = values[1]; else p2 = null;
            if (2 < count) p3 = values[2]; else p3 = null;
        }        
        //----------------------------------------------
        public static string GetPostBackControlName(bool clientID)
        {
            Page page = HttpContext.Current.Handler as Page;

            Control control = null;
            //first we will check the "__EVENTTARGET" because if post back made by       the controls 
            //which used "_doPostBack" function also available in Request.Form collection. 
            string ctrlname = page.Request.Params["__EVENTTARGET"];

            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            // if __EVENTTARGET is null, the control is a button type and we need to 
            // iterate over the form collection to find it
            else
            {
                string ctrlStr = String.Empty;
                Control c = null;
                foreach (string ctl in page.Request.Form)
                {
                    //handle ImageButton they having an additional "quasi-property" in their Id which identifies 
                    //mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        ctrlStr = ctl.Substring(0, ctl.Length - 2);
                        c = page.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = page.FindControl(ctl);
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                             c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            if (control == null)
                return null;
            else
                return clientID ? control.ClientID : control.ID;
        }
        //----------------------------------------------
        public static void Redirect(string form, string par1, string par2, string par3, string par4, string par5)
        {
            HttpContext.Current.Items["__p1"] = par1;
            HttpContext.Current.Items["__p2"] = par2;
            HttpContext.Current.Items["__p3"] = par3;
            HttpContext.Current.Items["__p4"] = par4;
            HttpContext.Current.Items["__p5"] = par5;
            HttpContext.Current.Server.Transfer(form, false);  // false domyslna - czyscimy formatkę - nie wiem jeszcze jakie różnice, z true nie było zbytniej różnicy
        }

        public static void ShowModal(string form, string winparams, string par1, string par2, string par3, string par4, string par5)
        {
            HttpContext.Current.Items["__p1"] = par1;
            HttpContext.Current.Items["__p2"] = par2;
            HttpContext.Current.Items["__p3"] = par3;
            HttpContext.Current.Items["__p4"] = par4;
            HttpContext.Current.Items["__p5"] = par5;
            //ExecOnStart("showModal", form);
        }

        public static string GetRedirectUrl(string url)
        {
            string u = url.ToLower();
            if (!u.StartsWith("http") && !u.StartsWith("www"))
                if (url.StartsWith("/") || url.StartsWith("\\"))
                    return "~" + url;
                else if (!url.StartsWith("~"))
                    return "~/" + url;
            return url;
        }

        public static bool IsUrl(string cmd, out string url)  // SqlMenu, SqlContext, SqlCarousel: url:_link_, _link_ = http|https|www|podstrona_aplikacji.aspx
        {
            if (!String.IsNullOrEmpty(cmd) && cmd.ToLower().StartsWith("url:"))
            {
                url = Tools.GetRedirectUrl(cmd.Substring(4));
                return true;
            }
            else
            {
                url = cmd;
                return false;
            }
        }


        //----- jQuery modal dialog -----

        /*
        UpdatePanel-Conditional (form)  <--- tego szukam, musi być Conditional zeby sortowania na kontrolka chodziły (Postback nie ukrywał formatki)
            UserControl                 <--- to jako parametr cnt
                <div id=""> (modal)     <--- divModalId
                    UpdatePanel 
                        kontrolka
                        btClose
        */

        public static UpdatePanel FindUpdatePanel(Control cnt)  // szuka powyżej
        {
            if (cnt is UpdatePanel)
                return cnt as UpdatePanel;
            else
                if (cnt.Parent == null)
                    return null;
                else
                    return FindUpdatePanel(cnt.Parent);
        }

        /*
        public static Control FindParentControl(Control cnt, Type typ)  // szuka powyżej
        {
            if (cnt is typ)
                return cnt as UpdatePanel;
            else
                if (cnt.Parent == null)
                    return null;
                else
                    return FindParentControl(cnt.Parent, type);
        }
        */

        public static T FindParentControl<T>(Control cnt) where T : Control
        {
            if (cnt.Parent == null)
                return null;
            if (cnt.Parent is T)
                return cnt.Parent as T;
            else
                return FindParentControl<T>(cnt.Parent);
        }


        public static void ShowDialog(string divModalId, string parentUpdPanelId, string title) // używa jquery, może dac tu cały script ???
        {                                                                                       // UWAGA !!! update panel musi być Conditional żeby sortowania na listahc chodziły !!!
            Tools.ExecOnStart2(divModalId + "_script", String.Format("showDialog('{0}','{1}','{2}');",
                divModalId, parentUpdPanelId, title));
        }

        public static void ShowDialog(string divModalId, string parentUpdPanelId, string title, int? width, string btCloseId) // używa jquery, może dac tu cały script ???
        {                                                                                       // UWAGA !!! update panel musi być Conditional żeby sortowania na listahc chodziły !!!
            if (width == null) width = 700;  // default
            Tools.ExecOnStart2(divModalId + "_script", String.Format("showDialog2('{0}','{1}','{2}',{3},'{4}');",
                divModalId, parentUpdPanelId, title, (int)width, btCloseId));
        }

        //----
        public static void ShowDialog(Control cnt, string divModalId, int? width, Button btClose, string title)  // umieść kontrolkę cnt w div modalnym i ustaw klawisz zamykający
        {
            ShowDialog(cnt, divModalId, width, btClose, true, title);

            /*
            if (btClose != null) MakeDialogCloseButton(btClose, divModalId);
            Control upa = FindUpdatePanel(cnt);
            if (upa == null) upa = cnt.Parent;  // nie powinno mieć miejsca
            //ShowDialog(divModalId, upa.ClientID, title);
            if (btClose != null)
                ShowDialog(divModalId, upa.ClientID, title, width, btClose.ClientID);
            else
                ShowDialog(divModalId, upa.ClientID, title);
            */ 
        }

        public static void ShowDialog(Control cnt, string divModalId, int? width, Button btClose, bool doClick, string title)  // umieść kontrolkę cnt w div modalnym i ustaw klawisz zamykający
        {
            if (btClose != null && doClick) MakeDialogCloseButton(btClose, divModalId);
            Control upa = FindUpdatePanel(cnt);
            if (upa == null) upa = cnt.Parent;  // nie powinno mieć miejsca
            //ShowDialog(divModalId, upa.ClientID, title);
            if (btClose != null)
                ShowDialog(divModalId, upa.ClientID, title, width, btClose.ClientID);
            else
                ShowDialog(divModalId, upa.ClientID, title);
        }

        public static void ShowDialogF(Control cnt, string divModalId, int? width, Button btClose, bool doClick, string title)  // umieść kontrolkę cnt w div modalnym i ustaw klawisz zamykający
        {
            if (btClose != null && doClick) MakeDialogCloseButton2(btClose, divModalId);  // bez return zeby sie postback wykonal
            Control upa = FindUpdatePanel(cnt);
            if (upa == null) upa = cnt.Parent;  // nie powinno mieć miejsca
            //ShowDialog(divModalId, upa.ClientID, title);
            if (btClose != null)
                ShowDialog(divModalId, upa.ClientID, title, width, btClose.ClientID);
            else
                ShowDialog(divModalId, upa.ClientID, title);
        }


        
        
        
        
        
        
        
        public static void MakeDialogCloseButton(Button bt, string divModalId)
        {
            Tools.MakeButton(bt, String.Format("javascript:$('#{0}').dialog('close');return true;", divModalId));
        }

        public static void MakeDialogCloseButton2(Button bt, string divModalId)   // bez return true, np jak ma się jeszcze postback wykonac, na razie zdaje sie nie będzie wykorzystywana ...
        {
            Tools.MakeButton(bt, String.Format("javascript:$('#{0}').dialog('close');", divModalId));
        }

        public static void CloseDialog(string divModalId)
        {
            ExecOnStart2(divModalId + "_closescript", String.Format("javascript:$('#{0}').dialog('close');", divModalId));
        }

        public static void CloseDialogOverlay()
        {
            ExecOnStart2("remover", "$('div.ui-widget-overlay').remove();");   //20151227 jak w ListView to po update nie ma już edit template (i modala) ale zostaje overlay
        }
        //-----
        //public static void ShowDialog(Control cnt, string divModalId, int? width, Button btClose, string title)  // umieść kontrolkę cnt w div modalnym i ustaw klawisz zamykający <<<< do ListView !!!, ItemCommand dodac modal.close
        //{
        //    Control upa = FindUpdatePanel(cnt);
        //    if (upa == null) upa = cnt.Parent;  // nie powinno mieć miejsca
        //    ShowDialog(divModalId, upa.ClientID, title, width, btClose.ClientID);   // bez klawisza - i tak kliknie postback
        //}
        //-------------------------------
        // pobiera z Items - wywoływać w !PostBack bo znikają ... 
        public static string GetParam(int no)   //1..
        {
            return (string)HttpContext.Current.Items["__p" + no.ToString()];
        }

        // wywołać koniecznie w Page_Load !IsPostBack, przepisuje parametry do Viewstate zeby nie zniknęły
        public static void CatchParams(StateBag ViewState)      // System.Web.UI.StateBag
        {
            for (int i = 1; i <= 5; i++)
            {
                string id = "__p" + i.ToString();
                ViewState[id] = HttpContext.Current.Items[id];
            }
        }

        // jak wywołamy CatchParams to tą funkcją dostajemy się do parametrów w dowolym czasie bo sa już w ViewState
        public static string GetParam(StateBag ViewState, int no)   //1..
        {
            return (string)ViewState["__p" + no.ToString()];
        }
        //-----------------------------------------------------
        public static int GetViewStateInt(object vs, int def)
        {
            if (vs == null) return def;
            else return (int)vs;
        }

        public static bool GetViewStateBool(object vs, bool def)
        {
            if (vs == null) return def;
            else return (bool)vs;
        }

        public static string GetViewStateStr(object vs)
        {
            if (vs == null) return "";  // nie null !!!
            else return (string)vs;
        }

        public static double GetViewStateDouble(object vs, double def)
        {
            if (vs == null) return def;
            else return (double)vs;
        }

        public static DateTime GetViewStateDateTime(object vs, DateTime def)
        {
            if (vs == null) return def;
            else return (DateTime)vs;
        }
        //-----------------------------------------------------
        public static int GetInt(object vs, int def)
        {
            if (vs == null) return def;
            else return (int)vs;
        }

        public static UInt32 GetUInt(object vs, UInt32 def)
        {
            if (vs == null) return def;
            else return (UInt32)vs;
        }

        public static bool GetBool(object vs, bool def)
        {
            if (vs == null) return def;
            else return (bool)vs;
        }

        public static char GetChar(object vs, char def)
        {
            if (vs == null) return def;
            else return (char)vs;
        }

        public static string GetStr(object vs)
        {
            if (vs == null) return "";  // nie null !!!
            else return (string)vs;
        }

        public static string GetStr(object vs, string def)
        {
            if (vs == null) return def;
            else return (string)vs;
        }

        public static string[] GetStrA(object vs)
        {
            if (vs == null) return null;
            else return (string[])vs;
        }

        public static double GetDouble(object vs, double def)
        {
            if (vs == null) return def;
            else return (double)vs;
        }

        public static DateTime GetDateTime(object vs, DateTime def)
        {
            if (vs == null) return def;
            else return (DateTime)vs;
        }

        public static DateTime[] GetDateTimeA(object vs)
        {
            if (vs == null) return null;
            else return (DateTime[])vs;
        }
        //-----
        public static bool FirstExecute(StateBag vs, string id)
        {
            if (!Tools.GetViewStateBool(vs[id], false))
            {
                vs[id] = true;
                return true;
            }
            else 
                return false;
        }
        //-----------------------------------------------------
        public static void MakeBackButton(Button bt)
        {
            bt.Attributes.Add("onClick", "javascript:history.back(); return false;");
        }

        public static void MakeRedirectButton(Button bt, string url)
        {
            bt.Attributes.Add("onClick", "javascript:window.location='" + url + "'; return false;");
        }

       

        public static void MakeConfirmButton(Button bt, string question, HiddenField result)  // 1 true, 0 false
        {
            if (String.IsNullOrEmpty(question))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", String.Format("javascript:return doConfirm('{0}','{1}');", question.Replace("\n", "\\n"), result.ClientID));
        }

        //------
        public static void MakeConfirmButton(Button bt, string question, string scriptYes, string scriptNo)
        {
            if (String.IsNullOrEmpty(question))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", "javascript:if (confirm('" + question.Replace("\n", "\\n") + "')) {{{0} return true;}} else {{{1} return false;}};");
        }


        public static void MakeConfirmButton(LinkButton bt, string question, string scriptYes, string scriptNo)
        {
            if (String.IsNullOrEmpty(question))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", "javascript:if (confirm('" + question.Replace("\n", "\\n") + String.Format("')) {{{0} return true;}} else {{{1} return false;}};", scriptYes, scriptNo));
        }

        public static void MakeConfirmButton(Button bt, string question, Button btYes, Button btNo)
        {
            string scriptYes = btYes != null ? String.Format("doClick('{0}');", btYes.ClientID) : null;
            string scriptNo = btNo != null ? String.Format("doClick('{0}');", btNo.ClientID) : null;
            MakeConfirmButton(bt, question, scriptYes, scriptNo);
        }

        public static void MakeConfirmButton(LinkButton bt, string question, Button btYes, Button btNo)
        {
            string scriptYes = btYes != null ? String.Format("doClick('{0}');", btYes.ClientID) : null;
            string scriptNo = btNo != null ? String.Format("doClick('{0}');", btNo.ClientID) : null;
            MakeConfirmButton(bt, question, scriptYes, scriptNo);
        }

        //------
        
 

        public static void MakeConfirmDeleteRecordButton(WebControl bt)
        {
            MakeConfirmButton(bt, "Potwierdzasz usunięcie rekordu danych ?");
        }

        public static void MakeConfirmDeleteRecordButton(Control cnt, string btname)
        {
            WebControl bt = (WebControl)cnt.FindControl(btname);
            if (bt != null) MakeConfirmDeleteRecordButton(bt);
        }
        //------------------------------------



        const string WarningTitle = "Uwaga!\\n\\n";
        const string ErrorTitle = "Błąd!\\n\\n";
        const string InfoTitle = "Informacja";
        const string ConfirmTitle = "Pytanie";


#if MODAL

        public static String GetConfirmFunction(string msg, string title, WebControl btnOk, WebControl btnCancel, int doOldClick)
        {
            Page page = HttpContext.Current.Handler as Page;
            ClientScriptManager cs = page.ClientScript;
            String pst = null;
            if (btnOk != null)
                pst = cs.GetPostBackEventReference(btnOk, null).ToString();
            String OkId = (btnOk != null) ? btnOk.ClientID : String.Empty;
            String CancelId = (btnCancel != null) ? btnCancel.ClientID : String.Empty;
#if BOOTSTRAP
            //return String.Format("wmb.showConfirm('{0}', '{1}', '{2}', '{3}', \"{4}\", {5});", msg.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\\n", "<br />"), title, OkId, CancelId, pst, doOldClick);
            return String.Format("wmb.showConfirm('{0}', '{1}', '{2}', '{3}', \"{4}\", {5});", msg.Replace("\n", "\\n").Replace("\\n", "<br />"), title, OkId, CancelId, pst, doOldClick);
#else
            return String.Format("wm.showConfirm('{0}', '{1}', '{2}', '{3}', \"{4}\", {5});", msg.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\\n", "<br />"), title, OkId, CancelId, pst, doOldClick);
#endif
        }

        public static String GetConfirmFunction2(string msg, string title, WebControl btnOk, WebControl btnCancel, int doOldClick)
        {
            String OkId = (btnOk != null) ? /*btnOk.ClientID*/ String.Empty : String.Empty;
            String CancelId = (btnCancel != null) ? btnCancel.ClientID : String.Empty;
#if BOOTSTRAP
            //return String.Format("return wmb.showConfirm2('{0}', '{1}', this.id, '{2}');", msg.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\\n", "<br />"), title, /*OkId,*/ CancelId, null, doOldClick);
            return String.Format("return wmb.showConfirm2('{0}', '{1}', this.id, '{2}');", msg.Replace("\n", "\\n").Replace("\\n", "<br />"), title, /*OkId,*/ CancelId, null, doOldClick);
#else
            return String.Format("return wm.showConfirm2('{0}', '{1}', this.id, '{2}');", msg.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\\n", "<br />"), title, /*OkId,*/ CancelId, null, doOldClick);
#endif
        }

        public static String GetMessageFunction(string msg, string title, Button btnOk, string closeFunction)
        {
            Page page = HttpContext.Current.Handler as Page;
            ClientScriptManager cs = page.ClientScript;
            String OkId = (btnOk != null) ? btnOk.ClientID : String.Empty;
#if BOOTSTRAP
            //return String.Format(@"wmb.showMessage('{0}', '{1}', '{2}', {3});", msg.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\\n", "<br />"), title, OkId, !String.IsNullOrEmpty(closeFunction) ? @"""" + closeFunction + @"""" : "null");
            return String.Format(@"wmb.showMessage('{0}', '{1}', '{2}', {3});", msg.Replace("\n", "\\n").Replace("\\n", "<br />"), title, OkId, !String.IsNullOrEmpty(closeFunction) ? @"""" + closeFunction + @"""" : "null");
#else
            return String.Format(@"wm.showMessage('{0}', '{1}', '{2}', {3});", msg.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\\n", "<br />"), title, OkId, !String.IsNullOrEmpty(closeFunction) ? @"""" + closeFunction + @"""" : "null");
#endif
        }





        // OD TEGO MIEJSCA JEST PRZETESTOWANE
        public static void MakeConfirmButton(WebControl bt, string question)
        {
            if (String.IsNullOrEmpty(question))
                bt.Attributes.Remove("onClick");
            else
            {
                bt.Attributes.Add("onClick", GetConfirmFunction2(question, ConfirmTitle, bt, null, 0));
            }
        }
        //public static void MakeConfirmButton(WebControl bt, string question)
        //{
        //    if (String.IsNullOrEmpty(question))
        //        bt.Attributes.Remove("onClick");
        //    else
        //    {
        //        bt.Attributes.Add("onClick", "javascript:return confirm('" + question.Replace("\n", "\\n") + "');");
        //    }
        //}





        public static void ShowConfirm(string msg, string title, Button btnOk, Button btnCancel)  //T: ponieważ standardowe formatki wyświetlają klawisze OK i Cancel, ta funkcja też powinna tak wyświetlać, gdyż komunikaty mogą zawierać "... naciśnij Ok w celu, Cancel, żeby ...". Jak nie ma zgodności, to user będzie mieć problem. 
        {                                                                            // funkcja yes/no powinna być nazwana inaczej ShowConfirm3 / ShowConfirmYesNo lub pozwolić na przekazanie parametru tak jak w okienkach modalnych windows - btYesNo, btOk, btOkCancel, btCancel, btClose, btRetry, btOkNoRetryCancel itd.
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "showConfirm", GetConfirmFunction(msg, title, btnOk, btnCancel, 1), true);
        }

        public static void ShowConfirm(string msg, string title, Button btnOk)
        {
            ShowConfirm(msg, title, btnOk, null);
        }

        public static void ShowConfirm(string msg, Button btnOk)
        {
            ShowConfirm(msg, ConfirmTitle, btnOk);
        }

        public static void ShowConfirm(string msg, Button btnOk, Button btnCancel)
        {
            ShowConfirm(msg, ConfirmTitle, btnOk, btnCancel);
        }


        public static void ShowMessageTitle(string msg, string title, Button btnOk)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "showConfirm", GetMessageFunction(msg, title, btnOk, null), true);
        }


        public static void ShowMessage(string msg)
        {
            ShowMessageTitle(msg, InfoTitle, null);
        }


        public static void ShowMessage(int id, string msg)  // NIE WIEM CZY TO DZIAŁA DOBRZE
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "showConfirm" + id.ToString(), GetMessageFunction(msg, "Informacja", null, null), true);
        }


        public static void ShowMessage(Control cnt, string msg) // dla ajaxa zalecają przekazac update panel bo moze czasem nie dzialac ale nie zauwazylem
        {
            ScriptManager.RegisterStartupScript(cnt, typeof(Page), "alert", GetMessageFunction(msg, InfoTitle, null, null), true);
        }




        //T: NIE DZIAŁA :( - wyświetla oba naraz, i jest problem przy przekazywaniu parametrów 'sklejanych' gdzie wystepują ograniczajace '' np. przy akceptacji czasu pracy do daty - trzeba dodać sprawdzenie czy w przekazanym parametrze sa juz ''
        //public static void ShowMessages(string msg1, string msg2)
        //{
        //    Page page = HttpContext.Current.Handler as Page;
        //    ScriptManager.RegisterStartupScript(page, typeof(Page), "alert11", GetMessageFunction(msg1, InfoTitle, null, GetMessageFunction(msg2, InfoTitle, null, null)), true);
        //}

        public static void ShowMessages(string msg1, string msg2)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "alert11",
                "alert('" + msg1 + "');alert('" + msg2 + "');", true);
        }





        public static void ShowMessage(string msgfmt, params object[] parlist)
        {
            ShowMessageTitlePar(String.Format(msgfmt, parlist), InfoTitle);
        }

        public static void ShowMessageTitlePar(string msgfmt, string title, params object[] parlist)
        {
            ShowMessageTitle(String.Format(msgfmt, parlist), title, null);
        }

        public static void ShowError(string msg)
        {
            ShowMessageTitle(msg, ErrorTitle, null);
        }

        public static void ShowError(string msgfmt, params object[] parlist)
        {
            ShowMessageTitlePar(msgfmt, ErrorTitle, parlist);
        }

        public static void ShowWarning(string msg)
        {
            ShowMessageTitle(msg, WarningTitle, null);
        }

        public static void ShowWarning(string msgfmt, params object[] parlist)
        {
            ShowMessageTitlePar(msgfmt, WarningTitle, parlist);
        }


        //---------
        public static void ShowMessage2(string msg)
        {
            ShowMessage(msg);
            //Page page = HttpContext.Current.Handler as Page;
            //ScriptManager.RegisterStartupScript(page, typeof(Page), "alert2", String.Format(
            //     "$(window).load(function() {{ alert('{0}'); }});", msg), true);
            //"$(document).ready(function() {{ alert('{0}'); }});", msg), true);
        }


        public static void ShowMessage2(string msg, Button btOk)
        {
            ShowMessageTitle(msg, InfoTitle, btOk);
        }

        // D:

        
        public static void MakeConfirmButton2(Button bt, string question)  // wymaga podania question w '' ale mozna np wplesc w to jquery zapytanie o wartość ;)
        {
            if (String.IsNullOrEmpty(question))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", GetConfirmFunction2(question, ConfirmTitle, bt, null, 0));
        }

        // DO TEGO JEST PRZETESTOWANE

        public static void MakeInfoButton(Button bt, string info)
        {
            if (String.IsNullOrEmpty(info))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", GetMessageFunction(info, InfoTitle, bt, null));
        }

        public static void MakeButton(Button bt, string script)
        {
            if (String.IsNullOrEmpty(script))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", script);
        }

        //public static void MakeConfirmButton(Button bt, string question1, string question2)
        public static void MakeConfirmButton(WebControl bt, string question1, string question2)
        {
            if (String.IsNullOrEmpty(question1))
                bt.Attributes.Remove("onClick");
            else
                //bt.Attributes.Add("onClick", "javascript:return confirm('" + question1 + "') && confirm('" + question2 + "');");
                bt.Attributes.Add("onClick", "javascript:if (confirm('" + question1.Replace("\n", "\\n") + "')) return confirm('" + question2.Replace("\n", "\\n") + "'); else return false;");
        }


#else

        public static void MakeConfirmButton(WebControl bt, string question)
        {
            if (String.IsNullOrEmpty(question))
                bt.Attributes.Remove("onClick");
            else
            {
                bt.Attributes.Add("onClick", "javascript:return confirm('" + question.Replace("\n", "\\n") + "');");
            }
        }
        public static void ShowConfirm(string msg, Button btOk, Button btCancel)
        {
            ExecOnStart2("confirmbt2", String.Format("javascript:confirmClick('{0}','{1}','{2}');",
                msg.Replace("\n", "\\n"), 
                btOk != null ? btOk.ClientID : null, 
                btCancel != null ? btCancel.ClientID : null));
        }

        public static void ShowMessage(string msg)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "alert",
                "alert('" + msg + "');", true);
        }

        /* bład wywala
        public static void ShowConfirm(string msg, Button btOk, Button btCancel)
        {
            string jok = btOk != null ? String.Format("btClick('{0}');", btOk.ClientID) : null;
            string jcancel = btCancel != null ? String.Format("btClick('{0}');", btCancel.ClientID) : null;
            ExecOnStart2("confirmbt", String.Format("javascript:if (confirm('{0}')) {{{1}return true;}} else {{{2}return false;}}",
                msg, jok, jcancel));
        }
         */ 

        public static void ShowMessage(int id, string msg)  // jak trzeba kilka po sobie wyświetlić
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "alert" + id.ToString(),
                "alert('" + msg + "');", true);
        }

        public static void ShowMessages(string msg1, string msg2)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "alert11",
                "alert('" + msg1 + "');alert('" + msg2 + "');", true);
        }

        public static void ShowMessage(string msgfmt, params object[] parlist)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "alert",
                "alert('" + String.Format(msgfmt, parlist) + "');", true);
        }

        public static void ShowMessage(Control cnt, string msg) // dla ajaxa zalecają przekazac update panel bo moze czasem nie dzialac ale nie zauwazylem
        {
            ScriptManager.RegisterStartupScript(cnt, typeof(Page), "alert",
                "alert('" + msg + "');", true);
        }

        public static void ShowWarning(string msg)
        {
            Tools.ShowMessage(WarningTitle + msg);
        }

        public static void ShowWarning(string msgfmt, params object[] parlist)
        {
            Tools.ShowMessage(WarningTitle + msgfmt, parlist);
        }

        //---------

        public static void ShowError(string msg)
        {
            Tools.ShowMessage(ErrorTitle + msg);
        }

        public static void ShowError(string msgfmt, params object[] parlist)
        {
            Tools.ShowMessage(ErrorTitle + msgfmt, parlist);
        }

        //---------
        public static void ShowMessage2(string msg)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "alert2", String.Format(
                 "$(window).load(function() {{ alert('{0}'); }});", msg), true);
            //"$(document).ready(function() {{ alert('{0}'); }});", msg), true);
        }

        public static void ShowMessage2(string msg, Button btOk)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "alert2bt", String.Format(
                 "$(window).load(function() {{ alert('{0}');doClick('{1}'); }});", msg, btOk.ClientID), true);
            //"$(document).ready(function() {{ alert('{0}'); }});", msg), true);
        }

        public static void MakeConfirmButton2(Button bt, string question)  // wymaga podania question w '' ale mozna np wplesc w to jquery zapytanie o wartość ;)
        {
            if (String.IsNullOrEmpty(question))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", "javascript:return confirm(" + question + ");");
        }

        public static void MakeInfoButton(Button bt, string info)
        {
            if (String.IsNullOrEmpty(info))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", "javascript:alert('" + info.Replace("\n", "\\n") + "');return false;");
        }

        public static void MakeButton(Button bt, string script)
        {
            if (String.IsNullOrEmpty(script))
                bt.Attributes.Remove("onClick");
            else
                bt.Attributes.Add("onClick", script);
        }

        //public static void MakeConfirmButton(Button bt, string question1, string question2)
        public static void MakeConfirmButton(WebControl bt, string question1, string question2)
        {
            if (String.IsNullOrEmpty(question1))
                bt.Attributes.Remove("onClick");
            else
                //bt.Attributes.Add("onClick", "javascript:return confirm('" + question1 + "') && confirm('" + question2 + "');");
                bt.Attributes.Add("onClick", "javascript:if (confirm('" + question1.Replace("\n", "\\n") + "')) return confirm('" + question2.Replace("\n", "\\n") + "'); else return false;");
        }

#endif




        public static void ShowMessageLog(int logTyp2, string info2, string msg)
        {
            Log.Info(logTyp2, msg, info2);
            ShowMessage(msg);
        }



        public static void popupShowMessage(int typ, string msg)
        {
            switch (typ)
            {
                case 2:
                    msg = ErrorTitle + msg;
                    break;
            }
            ExecOnStart2("popmsg", String.Format("popupShowMessage({0},'{1}');", typ, msg.Replace("\n", "\\n")));
        }
        //---------

        public static void popupShowConfirm(string msg, Button btOk, Button btCancel)
        {
            ExecOnStart2("popquestion", String.Format("javascript:popupShowConfirm('{0}','{1}','{2}');",
                msg.Replace("\n", "\\n"),
                btOk != null ? btOk.ClientID : null,
                btCancel != null ? btCancel.ClientID : null));
        }

        //--------------------------------------
        public static void ShowConfirm2(string msg, Button btOk, Button btCancel)
        {
            ExecOnStart2("confirmbt2", String.Format(
                "$(window).load(function() {{ confirmClick('{0}','{1}','{2}'); }});",
                msg.Replace("\n", "\\n"),
                btOk != null ? btOk.ClientID : null,
                btCancel != null ? btCancel.ClientID : null));
        }

        //------
        public static void PrepareScrollButtons(Button btUp, Button btDown, string scrollerId, string scrollId, int pcent)
        {
            btUp.Attributes["onclick"] = String.Format("javascript:scroll('{0}', '{1}', '{2}');return false;", scrollerId, scrollId, -pcent);
            btDown.Attributes["onclick"] = String.Format("javascript:scroll('{0}', '{1}', '{2}');return false;", scrollerId, scrollId, pcent);
        }

        public static void ScrollRegister(string script, string cntId)  // script = ClientID
        {
            ExecOnStart2(script + "_scrollreg", String.Format("scrollRegister('{0}');", cntId));
        }
        //--------------
        public static void SetBodyScrollBar(bool visible)
        {
            if (visible)
                //Tools.RemoveClass(htmlbody, "noscroll");
                Tools.ExecOnStart2("bodyscroll", "$('body').removeClass('noscroll');");   // uruchamia jak jest, bez -2 dokłada ();
            else
                //Tools.AddClass(htmlbody, "noscroll");
                Tools.ExecOnStart2("bodyscroll", "$('body').addClass('noscroll');");
        }

        //--------------
        public static void FixMultiLineMaxLen(TextBox tb)
        {
            if (tb.TextMode == TextBoxMode.MultiLine && tb.MaxLength > 0)
            {
                tb.Attributes.Add("onkeypress", "return isMaxLen(this, " + tb.MaxLength + ");");
                //tb.Attributes.Add("onchange", "cutMaxLen(this, " + tb.MaxLength + ");");
                tb.Attributes.Add("onpaste", "cutMaxLen(this, " + tb.MaxLength + ");");
                tb.Attributes.Add("onblur", "return checkMaxLen(this, " + tb.MaxLength + ");");
            }
        }

        //------
        // RegisterClientStartupScript - umieszcza po tagu <form> - cz. na początku strony
        // RegisterStartupScript - umieszcza przed tagiem </form> - cz. na końcu strony

        public static void ShowErrorLog(int logTyp2, string info2, string msg)
        {
            Log.Error(logTyp2, msg, info2);
            ShowError(msg);
        }

        public static void ShowErrorLog(int logTyp2, string info2, string msgfmt, params object[] parlist)
        {
            string msg = String.Format(msgfmt, parlist);
            ShowErrorLog(logTyp2, msg, info2);
        }

        public static void ShowErrorLogTr(int logTyp2, string info2, string msg)
        {
            Log.ErrorTr(logTyp2, msg, info2);
            ShowError(msg);
        }

        public static void ShowErrorLogTr(int logTyp2, string info2, string msgfmt, params object[] parlist)
        {
            string msg = String.Format(msgfmt, parlist);
            ShowErrorLogTr(logTyp2, msg, info2);
        }
        //---

        public static string ToScript(string msg)
        {
            return msg.Replace("\r", "").Replace("\n", "\\n").Replace("'", "\\'"); //Replace("\\", "\\\\").
        }

        //---------------
        public static void ExecOnStart(string funcName)   // nazwa funkcji bez () i ;
        {
            ExecOnStart(funcName, "");
        }

        public static void ExecOnStart(string funcName, string par)   // lista parametrów oddzielona , tekst, który będzie wpisany w javascript
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), funcName,
                funcName + "(" + par + ");", true);
        }

        /*
        public static void ExecOnStartExist(string funcName, string par)   // lista parametrów oddzielona , tekst, który będzie wpisany w javascript
        {
            Page page = HttpContext.Current.Handler as Page;
            ClientScriptManager sm = page.ClientScript;
            if (sm.IsStartupScriptRegistered(funcName))
                sm.


            ScriptManager.RegisterStartupScript(page, typeof(Page), funcName,
                funcName + "(" + par + ");", true);
        }
        */



        public static void ExecOnStart(Control cnt, string funcName)  // dla ajaxa zalecają przekazac update panel bo moze czasem nie dzialac ale nie zauwazylem
        {
            ScriptManager.RegisterStartupScript(cnt, typeof(Page), funcName,
                funcName + "();", true);
        }

        public static void ExecOnStart(Control cnt, string funcName, string par)  // dla ajaxa zalecają przekazac update panel bo moze czasem nie dzialac ale nie zauwazylem
        {
            ScriptManager.RegisterStartupScript(cnt, typeof(Page), funcName,
                funcName + "(" + par + ");", true);
        }

        public static void ExecOnStart2(string scname, string script)   // scname np "script1" ale powinno to być unikane
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), scname, script, true);
            //page.ClientScript.RegisterStartupScript(typeof(Page), scname, script, true);  nie działa
        }

        //------------------
        public static void Back()
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "back",
                "history.back();", true);
        }

        public static void GoTo(string a_name)  // a_name bez # na początku !!!
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "goto",
                //"document.location.href('#" + a_name + "');"

                "window.navigate('#" + a_name + "');"

                , true);
        }

        public static void GoTo(Control c, string a_name)  // a_name bez # na początku !!!
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(c, typeof(Page), "goto",
                //"document.location.href('#" + a_name + "');"

                "window.navigate('#" + a_name + "');"

                , true);
        }

        public static void HideLabelAfter(Label label, int delayms)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), "hideLabel",
                //"setTimeout(\"test()\", " + delayms.ToString() + ");"
                "setTimeout(function() {var lb = document.getElementById('" + label.ClientID + "'); if (lb != null) {lb.style.display = 'none'; lb.value = '';}}, " + delayms.ToString() + ");"
                , true);
        }

        public static void x_HideListViewControls(ListView lv, string tbName, bool hide)
        {
            HtmlTable tb = (HtmlTable)lv.FindControl(tbName);
            if (tb != null)
                tb.Attributes["class"] = hide ? "HideControl" : null;
        }

        public static void HideControls(ListView lv, string hideClass)
        {
            HtmlTable tb = (HtmlTable)lv.FindControl("itemPlaceholderContainer");
            if (tb != null)
            {
                if (String.IsNullOrEmpty(hideClass))
                    hideClass = "HideControl";   // domyslna
                if (String.IsNullOrEmpty(tb.Attributes["class"]))
                    tb.Attributes["class"] = hideClass;
                else
                    tb.Attributes["class"] += " " + hideClass;
            }
        }

        public static Control SetControlVisible(Control item, string cntName, bool fVisible)
        {
            Control c = item.FindControl(cntName);
            if (c != null)
                c.Visible = fVisible;
            return c;
        }

        public static WebControl SetButtonVisible(Control item, string cntName, bool fVisible, string confirm)
        {
            WebControl c = item.FindControl(cntName) as WebControl;
            if (c != null)
            {
                c.Visible = fVisible;
                if (fVisible)
                    MakeConfirmButton(c, confirm);
            }
            return c;
        }


        /*
        public static Control SetControlVisible(ListViewItem item, string cntName, bool fVisible)
        {
            Control c = item.FindControl(cntName);
            if (c != null)
                c.Visible = fVisible;
            return c;
        }
        /*
        public static Control SetControlVisible(ListView lv, string cntName, bool fVisible)
        {
            Control c = lv.FindControl(cntName);
            if (c != null)
                c.Visible = fVisible;
            return c;
        }

        public static WebControl SetControlEnabled(ListView lv, string cntName, bool fEnabled)
        {
            WebControl c = (WebControl)lv.FindControl(cntName);
            if (c != null)
                c.Enabled = fEnabled;
            return c;
        }
        */

        //public static WebControl SetControlEnabled(Control item, string cntName, bool fEnabled)
        //{
        //    WebControl c = (WebControl)item.FindControl(cntName);
        //    if (c != null)
        //        c.Enabled = fEnabled;
        //    return c;
        //}

        public static Control SetControlEnabled(Control item, string cntName, bool fEnabled)  //20160424
        {
            Control c = (Control)item.FindControl(cntName);
            if (c != null)
                if (c is WebControl) ((WebControl)c).Enabled = fEnabled;
                else if (c is ExtenderControlBase) ((ExtenderControlBase)c).Enabled = fEnabled;  // validator 0123456789
            return c as Control;
        }
        //------------------
        public static Control FindControl(Control cnt, string id)  //przeszukuje dzieci
        {
            Control c = cnt.FindControl(id);
            if (c == null)
                foreach (Control cc in cnt.Controls)
                {
                    c = FindControl(cc, id);
                    if (c != null)
                        return c;
                }
            return c;
        }

        public static Control SetControlVisibleSub(Control item, string cntName, bool fVisible)
        {
            Control c = FindControl(item, cntName);
            if (c != null)
                c.Visible = fVisible;
            return c;
        }

        //------------------
        public static HtmlTableCell SetRowSpan(Control item, string cellName, int rowspan)
        {
            HtmlTableCell c = (HtmlTableCell)item.FindControl(cellName);
            if (c != null)
                c.RowSpan = rowspan;
            return c;
        }

        public static HtmlTableCell SetColSpan(Control item, string cellName, int colspan)
        {
            HtmlTableCell c = (HtmlTableCell)item.FindControl(cellName);
            if (c != null)
                c.ColSpan = colspan;
            return c;
        }
        //------------------
        public static bool FindTextBox(Control item, string name, out TextBox tb)
        {
            tb = (TextBox)item.FindControl(name);
            return tb != null;
        }
        //------------------
        public static Label FindLabel(Control item, string name)
        {
            return (Label)item.FindControl(name);
        }

        public static TextBox FindTextBox(Control item, string name)
        {
            return (TextBox)item.FindControl(name);
        }

        public static CheckBox FindCheckBox(Control item, string name)
        {
            return (CheckBox)item.FindControl(name);
        }

        public static DropDownList FindDropDownList(Control item, string name)
        {
            return (DropDownList)item.FindControl(name);
        }

        public static bool FindDropDownList(Control item, string name, out DropDownList ddl)
        {
            ddl = (DropDownList)item.FindControl(name);
            return ddl != null;
        }

        public static HiddenField FindHidden(Control item, string name)
        {
            return (HiddenField)item.FindControl(name);
        }

        public static Button FindButton(Control item, string name)
        {
            return (Button)item.FindControl(name);
        }

        public static LinkButton FindLinkButton(Control item, string name)
        {
            return (LinkButton)item.FindControl(name);
        }

        public static ListView FindListView(Control item, string name)
        {
            return (ListView)item.FindControl(name);
        }
        //------------------
        public static Label SetText(Control item, string name, string text)
        {
            Label lb = (Label)item.FindControl(name);
            if (lb != null) lb.Text = text;
            return lb;
        }

        public static Control SetText2(Control item, string name, string text)
        {
            Control c = item.FindControl(name);
            if (c != null)
            {
                if (c is HiddenField) ((HiddenField)c).Value = text;
                else if (c is TextBox) ((TextBox)c).Text = text;
                else if (c is Label) ((Label)c).Text = text;
                else if (c is Literal) ((Literal)c).Text = text;
            }
            return c;
        }

        public static string GetText(Control item, string name)
        {
            Control c = item.FindControl(name);
            if (c != null)
            {
                if (c is HiddenField) return ((HiddenField)c).Value;
                else if (c is TextBox) return ((TextBox)c).Text;
                else if (c is Label) return ((Label)c).Text;
            }
            return null;
        }

        public static HiddenField SetValue(Control item, string name, string value)
        {
            HiddenField hid = (HiddenField)item.FindControl(name);
            if (hid != null) hid.Value = value;
            return hid;
        }
        /* -> GetText
        public static string GetValue(Control item, string name)
        {
            HiddenField hid = (HiddenField)item.FindControl(name);
            if (hid != null)
                return hid.Value;
            else
                return null;
        }
        */

        public static CheckBox SetChecked(Control item, string name, bool chk)
        {
            CheckBox cb = (CheckBox)item.FindControl(name);
            if (cb != null)
                cb.Checked = chk;
            return cb;
        }

        public static TextBox SetTextBox(Control item, string name, string text)
        {
            TextBox tb = (TextBox)item.FindControl(name);
            if (tb != null)
                tb.Text = text;
            return tb;
        }

        public static TextBox SetTextBox(Control item, string name, string text, string tooltip)
        {
            TextBox tb = (TextBox)item.FindControl(name);
            if (tb != null)
            {
                tb.Text = text;
                tb.ToolTip = tooltip;
            }
            return tb;
        }

        public static Control SetButton(Control item, string name, string text, string vgroup)
        {
            Control bt = item.FindControl(name);
            if (bt != null)
                if (bt is Button) {
                    Button b = bt as Button;
                    b.Text = text;
                    if (String.IsNullOrEmpty(b.ValidationGroup)) b.ValidationGroup = vgroup;
                }
                else if (bt is LinkButton)
                {
                    LinkButton b = bt as LinkButton;
                    if(!b.Text.Contains("</i>")) // plomba juana
                        b.Text = text;
                    if (String.IsNullOrEmpty(b.ValidationGroup)) b.ValidationGroup = vgroup;
                }
                else if (bt is ImageButton)
                {
                    ImageButton b = bt as ImageButton;
                    if (String.IsNullOrEmpty(b.ValidationGroup)) b.ValidationGroup = vgroup;
                }
            return bt;
        }

        //public static Control SetButton(Control item, string name, string text)
        //{
        //    Control bt = item.FindControl(name);
        //    if (bt != null)
        //        if (bt is Button) ((Button)bt).Text = text;
        //        else if (bt is LinkButton) ((LinkButton)bt).Text = text;
        //    return bt;
        //}

        public static Button SetButton(Control item, string name, string text)
        {
            Button bt = (Button)item.FindControl(name);
            if (bt != null) bt.Text = text;
            return bt;
        }

        //------------------
        public static int? GetDdlSelectedValueInt(Control item, string name)  // w Value są int
        {
            DropDownList ddl = (DropDownList)item.FindControl(name);
            if (ddl != null)
                if (!String.IsNullOrEmpty(ddl.SelectedValue))
                    return Convert.ToInt32(ddl.SelectedValue);
            return null;
        }

        public static int? GetDdlSelectedValueInt(Control item, string name, int par)  // w Value są int, par: 1,2..
        {
            DropDownList ddl = (DropDownList)item.FindControl(name);
            if (ddl != null)
                if (!String.IsNullOrEmpty(ddl.SelectedValue))
                    return Convert.ToInt32(GetLineParam(ddl.SelectedValue, par));
            return null;
        }

        public static string GetDdlSelectedValue(Control item, string name)  // w Value są int
        {
            DropDownList ddl = (DropDownList)item.FindControl(name);
            if (ddl != null)
                return ddl.SelectedValue;
            return null;
        }

        public static string GetDdlSelectedValue(Control item, string name, int par)  // w Value są int
        {
            DropDownList ddl = (DropDownList)item.FindControl(name);
            if (ddl != null)
                return GetLineParam(ddl.SelectedValue, par);
            return null;
        }

        public static string GetDdlSelectedText(Control item, string name)  // w Value są int
        {
            DropDownList ddl = (DropDownList)item.FindControl(name);
            if (ddl != null && ddl.SelectedItem != null)
                return ddl.SelectedItem.Text;
            return null;
        }
        //------------------
        public static bool GetControlValue(Control c, out string value)
        {
            value = null;
            if (c != null)
            {
                if (c is HiddenField) value = ((HiddenField)c).Value;
                else if (c is TextBox) value = ((TextBox)c).Text;
                else if (c is Label) value = ((Label)c).Text;
                else return false;
                return true;
            }
            return false;
        }

        public static bool GetControlValue(Control cnt, string cntName, out string value)  // e.Item i ListView 
        {
            Control c = cnt.FindControl(cntName);
            return GetControlValue(c, out value);
        }
        /*
        public static bool GetControlValue(ListView lv, string cntName, out string value)
        {
            Control c = lv.FindControl(cntName);
            return GetControlValue(c, out value);
        }
        */
        //------------------
        public static void FillTime(DropDownList ddl, int start, int interval)   // od której godziny startujemy, z jakim interwałem [min]
        {
            if (interval <= 0) interval = 60;
            int i = 60 / interval;
            for (int h = 0; h < 24; h++)
            {
                for (int m = 0; m < i; m++)
                {
                    ListItem t = new ListItem(start.ToString("D2") + ":" + (m * interval).ToString("D2"));
                    ddl.Items.Add(t);
                }
                if (start == 23) start = 0;
                else start++;
            }
        }

        public static void FillTime2(DropDownList ddl, int start, int interval)   // od której godziny startujemy, z jakim interwałem [min]
        {
            if (interval <= 0) interval = 60;
            int i = 60 / interval;
            for (int h = 0; h < 24; h++)
            {
                for (int m = 0; m < i; m++)
                {
                    string time = start.ToString("D2") + ":" + (m * interval).ToString("D2");
                    DateTime tod = DateTime.Today;
                    tod = tod.Add(TimeSpan.Parse(time));

                    ListItem t = new ListItem(time, tod.ToString());
                    ddl.Items.Add(t);
                }
                if (start == 23) start = 0;
                else start++;
            }
        }
        //-------------------------
        public static int GetDaysCount(DateTime d1, DateTime d2)    // d2 > d1
        {
            TimeSpan dd = d2.Date.Subtract(d1.Date);
            double d = dd.TotalDays + 1;   // od 10 do 10 to jest 1 dzień
            int i = Convert.ToInt32(d);
            if (i <= 0) return 0;
            else return i;
        }

        public static DateTime bom(DateTime dt)
        {
            return dt.AddDays(-dt.Day + 1);
        }

        public static DateTime eom(DateTime dt)
        {
            return dt.AddDays(-dt.Day + 1).AddMonths(1).AddDays(-1);
        }

        public static DateTime bow(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }

        public static DateTime boy(DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime eoy(DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31);
        }

        //---------------------------
        public static DateTime StrToDateTime(string date, string time)
        {
            //DateTime dt = String.IsNullOrEmpty(date) ? DateTime.Today : DateTime.Parse(date);
            DateTime dt = String.IsNullOrEmpty(date) ? Base.DateMinValue : DateTime.Parse(date);
            TimeSpan ts = String.IsNullOrEmpty(time) ? TimeSpan.MinValue : TimeSpan.Parse(time);
            return dt.Add(ts);
        }
        /*
        private DateTime StrToDateTime(string date, string time)
        {
            DateTime dt = String.IsNullOrEmpty(date) ? DateTime.MinValue : DateTime.Parse(date);
            TimeSpan ts = String.IsNullOrEmpty(time) ? TimeSpan.MinValue : TimeSpan.Parse(time);
            return dt.Add(ts);
        }
        */

        public static string DateFriendlyName(int typ, DateTime dt)
        {
            switch (typ)
            {
                default:
                case 1:
                    return Tools.MonthName[dt.Month] + " '" + dt.Year.ToString();
            }
        }

        public static string MonthFriendlyName(int month)
        {
            return Tools.MonthName[month];
        }

        public static string GetDayName(DateTime dt)
        {
            return DayName[(int)dt.DayOfWeek + 1];  // 0-sunday, 1-monday .. 6-saturday
        }

        public static string GetDayName(DayOfWeek d)
        {
            return DayName[(int)d + 1];  // 0-sunday, 1-monday .. 6-saturday
        }

        public static string GetOdDo(object dOd, object dDo)
        {
            if (!db.isNull(dOd))
                return String.Format("{0} - {1}", Tools.DateToStr(dOd), db.isNull(dDo) ? "bez terminu" : Tools.DateToStr(dDo));
            else
                return null;
        }
        //----------------------------------------------------------------------------------------
        // p = HttpContext.Current.Request.Url.AbsoluteUri; zwraca pełną ścieżkę jak poniżej z nazwą formatki - tez monaz wykorzystać

        public static string PrepareHostAddr(string addr)
        {
            string laddr = addr.ToLower();
            if (!laddr.EndsWith(".aspx") && !laddr.EndsWith(".html"))  // jak nazwa strony to nie ruszam
                if (!addr.EndsWith("/"))            // jak nie to dokładam / na końcu
                    addr += "/";
            return addr;
        }

        public static string AddrSetPage(string addr, string page)
        {
            string laddr = addr.ToLower();
            if (laddr.EndsWith(".aspx") || laddr.EndsWith(".html"))
            {
                int idx = addr.LastIndexOf('/');
                if (idx > 0)
                    addr.Remove(idx);
            }
            if (addr.EndsWith("/"))
                return addr + page;
            else
                return addr + "/" + page;
        }

        public static string GetHostAppAddr()  // http://host:port/path/ z / na koncu, ale uwaga - takie jak wpisano w poasek adresu cz. np nazwa komputera lub nr ip
        {
            HttpContext context = HttpContext.Current;
            if (context != null)     //Checking the current context content
            {
                string path;         //Formatting the fully qualified website url/name
                path = string.Format("{0}://{1}{2}{3}",
                    context.Request.Url.Scheme,
                    context.Request.Url.Host,
                    context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port.ToString(),
                    context.Request.ApplicationPath);
                if (!path.EndsWith("/")) path += "/";
                return path;
            }
            else return null;
        }

        /*
        public static string GetAppAddr2(string page)  // http://host:port/path/ z / na koncu, ale uwaga - takie jak wpisano w poasek adresu cz. np nazwa komputera lub nr ip
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            string addr = settings.AppAddr;
            if (String.IsNullOrEmpty(addr))
                addr = GetHostAppAddr();
            if (!String.IsNullOrEmpty(page))
                addr = AddrSetPage(addr, page);
            return addr;
        }
        */
        public static string GetAppPath()  // z bs na końciu
        {
            return GetAppPath(null);
        }

        public static string GetAppPath(string FileName)
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                string path = context.Request.PhysicalPath;
                return path.Substring(0, path.LastIndexOf("\\") + 1) + FileName;
            }
            else return null;
        }

        public static string Slash(string path)
        {
            if (path.EndsWith("/"))
                return path;
            else
                return path + "/";
        }

        public static string BackSlash(string path)
        {
            if (path.EndsWith("\\"))
                return path;
            else
                return path + "\\";
        }
        //----------------------------------
        public static string GetAppVersion()    // wersja - Properties.AssemblyInfo.cs -> AssemblyVersion
        {
#if KWITEK
            return "1.1.0.0 beta";
#else
            //return typeof(PRPMasterPage).Assembly.GetName().Version.ToString();
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version.ToString();
#endif
        }

        public static string GetAppName()       // nazwa - Properties.AssemblyInfo.cs -> AssemblyTitle
        {
#if KWITEK
            return "Panel Pracownika";
#elif OKT
            return "Planowanie czasu pracy";
#else
            Assembly assembly = Assembly.GetExecutingAssembly();
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length == 1)
                return ((AssemblyTitleAttribute)attributes[0]).Title;
            else
                return null;
#endif
        }
        /*
        public static string GetAppVersion()    // wersja - Properties.AssemblyInfo.cs -> AssemblyVersion
        {
            if (App.IsApp(App._Kwitek))
            {
                return "1.1.0.0 beta";
            }
            else
            {
                //return typeof(PRPMasterPage).Assembly.GetName().Version.ToString();
                Assembly assembly = Assembly.GetExecutingAssembly();
                return assembly.GetName().Version.ToString();
            }
        }

        public static string GetAppName()       // nazwa - Properties.AssemblyInfo.cs -> AssemblyTitle
        {
            if (App.IsApp(App._Kwitek))
            {
                return "Panel Pracownika";
            }
            else
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length == 1)
                    return ((AssemblyTitleAttribute)attributes[0]).Title;
                else
                    return null;
            }
        }
        */

        public static string GetAppNameShort()       // nazwa - Properties.AssemblyInfo.cs -> AssemblyTitle
        {
#if PORTAL
            return "PORTAL";
#elif KWITEK
            return "KWITEK";
#elif OKT
            return "HARM";
#else
            return "RCP";
#endif
        }

        public static string GetAppAddr2(string page)  // http://host:port/path/ z / na koncu, ale uwaga - takie jak wpisano w poasek adresu cz. np nazwa komputera lub nr ip
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            string addr = settings.AppAddr;
            if (String.IsNullOrEmpty(addr))
                addr = GetHostAppAddr();
            if (!String.IsNullOrEmpty(page))
                addr = AddrSetPage(addr, page);
            return addr;
        }

        public static bool GetDbVerInfo(out string appName, out string dbName)  // true jak inna niż standardowa <<<<<< db.GetDbName 
        {
            //appName = GetAppPath();
            appName = HttpContext.Current.Server.MapPath("~");
            dbName = db.conStr;

            string[] a = appName.Split('/','\\');
            appName = "???";
            for (int i = a.Length - 1; i > 0; i--)
                if (!String.IsNullOrEmpty(a[i]))
                {
                    appName = a[i];
                    break;
                }

            int p = dbName.ToLower().IndexOf("catalog");
            bool f = false;
            if (p >= 0)
            {
                dbName = dbName.Remove(0, p + 7);
                p = dbName.IndexOf("=");
                if (p >= 0)
                {
                    dbName = dbName.Remove(0, p + 1);
                    p = dbName.IndexOf(";");
                    if (p >= 0)
                    {
                        dbName = dbName.Substring(0, p).Trim();
                        f = true;
                    }
                }
            }
            if (!f) dbName = "???";
            return true;
        }

        /*
        C:\Documents and Settings\wojciowt\My Documents\Visual Studio 2008\Projects\RCP2\
        Data Source=jgbhr02;Initial Catalog=HR_DB;User ID=hruser;Password=3cAPfd@7
        */
        //------------------------------------------------------------------------------
        private const string sesExp = "sesExp";

        public static void InitSessionExpired()     // powinna być wywołana w !IsPostBack w Page_Init w master page
        {
            HttpContext.Current.Session[sesExp] = sesExp;
        }

        public static bool IsSessionExpired()
        {
            return HttpContext.Current.Session[sesExp] == null;
        }

        public static void CheckSessionExpired()    // wywoływać w IsPostBack w PageInit w formatkach (Ankieta)
        {
            if (IsSessionExpired())
            {
                //Exception ex = new Exception("Sesja wygasła");
                //HttpContext.Current.Session.RemoveAll();
                //throw ex;

                HttpContext.Current.Session.RemoveAll();
                AppError.Show("Sesja wygasła");
            }
        }
        //----------------------------------------------------------------------------
        public static void SetNoCache()
        {
            HttpContext.Current.Response.AppendHeader("Cache-Control", "no-store");

            
            //HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Current.Response.Cache.SetNoStore();

            /*
            //HttpCachePolicy.SetNoStore(); 

            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            */ 
        }

        public static void EnableUpload()
        {
            Page page = HttpContext.Current.Handler as Page;
            page.Form.Attributes.Add("enctype", "multipart/form-data");  // ponieważ mamy FileUpload1 na formatce to musze to dodać bo za pierwszym razem nie widzi wpisanego pliku
        }
        //---
        public static void RePostCancel(Page p, string sesId)
        {
            if (!p.IsPostBack)
            {
                if (p.Session[sesId] == null)
                {
                    p.Session[sesId] = 1;
                    p.Response.Redirect(p.Request.Url.AbsoluteUri);
                }
                else
                    p.Session[sesId] = null;
            }
        }

        public static void RePostCancel()    // w Page_Init albo jeszcze wcześniej ...
        {
            Page p = HttpContext.Current.Handler as Page;
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(p.Request.Url.AbsoluteUri, "md5");
            RePostCancel(p, hash);
        }
        //-----
        public static WebControl PreventReClick(Control item, string btId)
        {
            if (item != null)
            {
                WebControl bt = item.FindControl(btId) as WebControl;
                PreventReClick(bt);
                return bt;
            }
            return null;
        }

        public static void PreventReClick(WebControl bt)
        {
            if (bt != null)
            {
                //Tools.MakeButton(bt, String.Format("document.getElementById('{0}').disabled = 'true';return true;", bt.ClientID));  // nie robi postback
                Page p = HttpContext.Current.Handler as Page;
                ClientScriptManager cs = p.ClientScript;
                bt.Attributes["onclick"] += String.Format("this.disabled=true;{0};", cs.GetPostBackEventReference(bt, null).ToString());   // zeby zostawił co było 
            }
        }
        //-----------------------------------------------------------------------------
        public static void Delay(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }

        public static string UniqueId()
        {
            string d = DateTime.Now.ToString("u");
            return d.Replace("-", "").Replace(".", "").Replace(":", "").Replace(" ","");
        }
        //----------------------------------------------------------------------------
        public const int lbADDLAST = 1;
        public const int lbADDFIRST = 2;
        public const int lbADDSORT = 3;

        public static int MoveListItem(ListBox lbFrom, ListBox lbTo, int add)
        {
            int[] sel = lbFrom.GetSelectedIndices();
            int cnt = sel.Count();
            if (cnt == 0) return -1;    // nic nie jest zaznaczone
            else
            {
                lbTo.ClearSelection();
                int idx = -1;
                int idx2;
                switch (add)
                {
                    case lbADDFIRST:
                        idx2 = 0;
                        for (int i = sel.Count() - 1; i >= 0; i--)
                        {
                            ListItem li1 = lbFrom.Items[sel[i]];
                            string st = li1.Text;
                            ListItem li = lbTo.Items.FindByText(st);
                            if (li == null)
                            {
                                ListItem li2 = new ListItem(st);
                                li2.Selected = true;
                                lbTo.Items.Insert(idx2++, li2);
                            }
                            else
                            {
                                li.Selected = true;
                                if (idx == -1)
                                    idx = lbTo.Items.IndexOf(li);
                            }
                            lbFrom.Items.Remove(li1);
                        }
                        break;
                    case lbADDLAST:
                        idx2 = lbTo.Items.Count;
                        for (int i = sel.Count() - 1; i >= 0; i--)
                        {
                            ListItem li1 = lbFrom.Items[sel[i]];
                            string st = lbFrom.Items[sel[i]].Text;
                            ListItem li = lbTo.Items.FindByText(st);
                            if (li == null)
                            {
                                ListItem li2 = new ListItem(st);
                                li2.Selected = true;
                                lbTo.Items.Insert(idx2, li2);
                            }
                            else
                            {
                                li.Selected = true;
                                if (idx == -1)
                                    idx = lbTo.Items.IndexOf(li);
                            }
                            lbFrom.Items.Remove(li1);
                        }
                        break;
                    case lbADDSORT:  // na razie nie ma
                        break;
                }
                return 0;
            }
        }

        public static int MoveListItem1(ListBox lbFrom, ListBox lbTo, int add)
        {
            if (lbFrom.SelectedIndex == -1) return -1;
            else
            {
                ListItem li = lbTo.Items.FindByText(lbFrom.SelectedItem.Text);
                if (li != null) return lbTo.Items.IndexOf(li);
                else
                {
                    switch (add)
                    {
                        case lbADDFIRST:
                            lbTo.Items.Insert(0, lbFrom.SelectedItem.Text);
                            lbTo.SelectedIndex = 0;
                            break;
                        case lbADDLAST:
                            lbTo.Items.Add(lbFrom.SelectedItem.Text);
                            lbTo.SelectedIndex = lbTo.Items.Count - 1;
                            break;
                        case lbADDSORT:
                            bool b = false;
                            for (int idx = 0; idx < lbTo.Items.Count; idx++)
                                if (lbTo.Items[idx].Text.CompareTo(lbFrom.SelectedItem.Text) > 0)
                                {
                                    lbTo.Items.Insert(idx, lbFrom.SelectedItem.Text);
                                    lbTo.SelectedIndex = idx;
                                    b = true;
                                }
                            if (!b)
                            {
                                lbTo.Items.Add(lbFrom.SelectedItem.Text);
                                lbTo.SelectedIndex = lbTo.Items.Count - 1;
                            }
                            break;
                    }
                    lbFrom.Items.Remove(lbFrom.SelectedItem);
                    return 0;
                }
            }
        }
        //----------------------------------------------
        public static void CollapseDepth1(TreeView tv)
        {
            tv.CollapseAll();
            for (int i = 0; i < tv.Nodes.Count; i++)
                tv.Nodes[i].Expand();
        }

        public static int SelectMenu(Menu menu, string selValue)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                string sv, help;
                GetLineParams(menu.Items[i].Value, out sv, out help);
                if (sv == selValue)
                {
                    menu.Items[i].Selected = true;
                    return i;
                }
            }
            return -1;
        }

        public static int SelectedIndex(Menu menu)
        {
            for (int i = 0; i < menu.Items.Count; i++)
                if (menu.Items[i].Selected)
                    return i;
            return -1;
        }
        
        public static int RemoveMenu(Menu menu, string selValue)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                string sv, help;
                GetLineParams(menu.Items[i].Value, out sv, out help);
                if (sv == selValue)
                {
                    menu.Items.RemoveAt(i);
                    return i;
                }
            }
            return -1;
        }
        
        public static int RemoveMenu(Menu menu, params string[] values)
        {
            int ret = -1;
            for (int i = menu.Items.Count - 1; i >= 0 ; i--)
            {
                string sv, help;
                GetLineParams(menu.Items[i].Value, out sv, out help);
                //if (sv.IsAny(values))  konflikt 2 miejsc :(
                if (values.Any(a => a.Equals(sv)))
                {
                    menu.Items.RemoveAt(i);
                    ret = i;
                }
            }
            return ret;
        }

        public static MenuItem GetMenuItem(Menu menu, string selValue)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                string sv, help;
                Tools.GetLineParams(menu.Items[i].Value, out sv, out help);
                if (sv == selValue)
                {
                    return menu.Items[i];
                }
            }
            return null;
        }

        public static MenuItem SkipMenuItemClick(Menu menu, string selValue)
        {
            MenuItem mi = GetMenuItem(menu, selValue);
            if (mi != null) mi.NavigateUrl = "javascript:nop();";
            return mi;
        }

        public static int SetMenu(Menu menu, string value, string setText)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                string sv, help;
                GetLineParams(menu.Items[i].Value, out sv, out help);
                if (sv == value)
                {
                    menu.Items[i].Text = setText;
                    return i;
                }
            }
            return -1;
        }

        public static int EnableMenu(Menu menu, string selValue, bool enabled)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                string sv, help;
                GetLineParams(menu.Items[i].Value, out sv, out help);
                if (sv == selValue)
                {
                    menu.Items[i].Enabled = enabled;
                    return i;
                }
            }
            return -1;
        }

        public static int _SelectMenu(Menu menu, string selValue, string sesId)  // ustawia w Sesji sesId zeby mogl do tego wrócić jak selValue = null (np main menu -> raporty -> zoom)
        {
            if (String.IsNullOrEmpty(selValue))
                selValue = (string)HttpContext.Current.Session[sesId];
            for (int i = 0; i < menu.Items.Count; i++)
            {
                string sv, help;
                GetLineParams(menu.Items[i].Value, out sv, out help);
                if (sv == selValue)
                {
                    menu.Items[i].Selected = true;
                    HttpContext.Current.Session[sesId] = selValue;
                    return i;
                }
                else
                    menu.Items[i].Selected = false;  //20131016 zostawiał zaznaczenie Index = 0
            }
            return -1;
        }

        public static int SelectMenu(Menu menu, string selValue, int defIndex)
        {
            int mn = SelectMenu(menu, selValue);
            if (mn == -1)
                if (defIndex >= 0 && defIndex < menu.Items.Count)
                {
                    menu.Items[defIndex].Selected = true;
                    mn = defIndex;
                }
            return mn;
        }

        public static int SelectMenuFromSession(Menu menu, string sesId, string def)
        {
            string s = (string)HttpContext.Current.Session[sesId];
            if (String.IsNullOrEmpty(s)) s = def;
            return SelectMenu(menu, s);
        }

        public static int SelectMenuFromSession(Menu menu, string sesId)
        {
            string s = (string)HttpContext.Current.Session[sesId];
            if (String.IsNullOrEmpty(s))
            {
                if (menu.Items.Count > 0)
                {
                    menu.Items[0].Selected = true;   // zakładam ze jest choć jedna pozycja
                    return 0;
                }
                else
                    return -1;
            }
            else
                return SelectMenu(menu, s);
        }

        public static int SelectTab(Menu mnTabs, MultiView mvTabs, string ses_active_tab, params string[] helps)
        {
            if (!String.IsNullOrEmpty(ses_active_tab))
                HttpContext.Current.Session[ses_active_tab] = mnTabs.SelectedValue;
            int idx = Tools.StrToInt(mnTabs.SelectedValue, 0);
            if (idx >= mvTabs.Views.Count)
                idx = 0;
            if (helps != null && idx < helps.Count())
                Info.SetHelp(helps[idx]);
            mvTabs.ActiveViewIndex = idx;
            return idx;
        }

        public static int SelectTab(Menu mnTabs, MultiView mvTabs, string ses_active_tab, bool setHelp)
        {
            string sv, help;
            GetLineParams(mnTabs.SelectedValue, out sv, out help);
            if (!String.IsNullOrEmpty(ses_active_tab))
                HttpContext.Current.Session[ses_active_tab] = sv;
            int idx = -1;
            for (int i = 0; i < mvTabs.Views.Count; i++)
            {
                View v = mvTabs.Views[i];
                if (v.ID == sv)
                {
                    idx = i;
                    break;
                }
            }
            if (setHelp && !String.IsNullOrEmpty(help))
                Info.SetHelp(help);
            mvTabs.ActiveViewIndex = idx;
            return idx;
        }
        //----------------------------------------------
        public static void SetError(Label lb, string msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                lb.Text = null;
                lb.Visible = false;
            }
            else
            {
                lb.Text = msg;
                lb.Visible = true;
            }
        }

        public static bool SetErrorMarker(Label lb, bool error)
        {
            lb.ForeColor = error ? warnColor : Color.Black;
            return error;
        }
        /*
        public static bool DateOk(string date)
        {
            try
            {
                //Convert.ToDateTime(date);
                //string s = Base.getScalar("select CONVERT(datetime," + Base.strParam(date) + ",20)");
                if (String.IsNullOrEmpty(date.Trim()))
                    return false;
                else
                {
                    string s = Base.getScalar("select " + Base.sqlGetDateAsDateTime(Base.strParam(date)));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        */
        public static bool DateOk(string date)
        {
            DateTime d;
            return DateOk(date, out d);
        }

        public static bool DateOk(string date, out DateTime d)
        {
            d = DateTime.MinValue;
            try
            {
                //Convert.ToDateTime(date);
                //string s = Base.getScalar("select CONVERT(datetime," + Base.strParam(date) + ",20)");
                if (String.IsNullOrEmpty(date.Trim()))
                    return false;
                else
                {
                    string s = Base.getScalar("select " + Base.sqlGetDateAsDateTime(Base.strParam(date)));
                    if (DateTime.TryParse(s, out d))
                        return true;
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IntOk(string value)
        {
            try
            {
                Int32.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IntOk(string value, int min, int max)
        {
            try
            {
                int i = Int32.Parse(value);
                return min <= i && i <= max;
            }
            catch
            {
                return false;
            }
        }

        public static bool CurrencyOk(string value)
        {
            try
            {
                double d = Double.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //-------------------
        public static void SetReadOnly(TextBox tb, bool fEdit)
        {
            tb.Visible = true;
            tb.ReadOnly = !fEdit;
            if (fEdit)
                RemoveClass(tb, "readonly");
            else
                AddClass(tb, "readonly");
        }

        public static void SetEdit(TextBox tb, bool fEdit)
        {
            tb.Visible = true;
            tb.ReadOnly = !fEdit;
#if BOOTSTRAP
            if (fEdit)
                tb.CssClass = "form-control editable";
            else
                tb.CssClass = "form-control readonly";
#else
            if (fEdit)
                tb.CssClass = "textbox editable";
            else
                tb.CssClass = "textbox readonly";
#endif
        }
        //-------------------
        /*
        public static bool AddClass(WebControl cnt, string css)
        {
            string c = cnt.Attributes["class"].ToLower();
            if (c.IndexOf(css.ToLower()) == -1)     // nie ma 
            {
                cnt.Attributes["class"] += String.IsNullOrEmpty(c) ? css : (" " + css);
                return false;
            }
            else              
                return true;                        // klasa już była
        }

        public static bool RemoveClass(WebControl cnt, string css)
        {
            string c = cnt.Attributes["class"].ToLower();
            if (c.IndexOf(css.ToLower()) == -1)     // nie ma 
                return false;
            else
            {
                c = cnt.Attributes["class"].Replace(css, "");
                return true;
            }
        }
         */
        public static bool AddClass(ref string css, string addcss)
        {
            bool ret = false;
            if (!String.IsNullOrEmpty(addcss))
            {
                string[] cc = css.Split(' ');   //new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] ac = addcss.Split(' ');
                for (int i = 0; i < ac.Length; i++)
                    if (!cc.Contains(ac[i]))
                    {
                        css = (css + " " + ac[i]).Trim();
                        ret = true;
                    }
            }
            return ret;
        }

        public static string AddClass(string css, string addcss)
        {
            AddClass(ref css, addcss);
            return css;
        }

        public static void RemoveClass(ref string css, string remcss)
        {
            bool ret = false;
            if (!String.IsNullOrEmpty(css))
            {
                string[] rc = remcss.Split(' ');
                for (int i = 0; i < rc.Length; i++)
                    css = (" " + css + " ").Replace(" " + rc[i] + " ", "").Replace("  ", " ").Trim();
            }
        }

        public static string RemoveClass(string css, string remcss)
        {
            RemoveClass(ref css, remcss);
            return css;
        }
        //-------------------
        public static void AddClass(HtmlControl cnt, string css)
        {
            string c = cnt.Attributes["class"];
            if (String.IsNullOrEmpty(c))
                cnt.Attributes["class"] = css;
            else
            {
                string[] ca = c.ToLower().Split(' ');
                if (!ca.Contains(css.ToLower()))
                    cnt.Attributes["class"] += " " + css;
            }
        }

        public static void AddClass(WebControl cnt, string css)
        {
            string c = cnt.Attributes["class"];
            if (String.IsNullOrEmpty(c))
                cnt.Attributes["class"] = css;
            else
            {
                string[] ca = c.ToLower().Split(' ');
                if (!ca.Contains(css.ToLower()))
                    cnt.Attributes["class"] += " " + css;
            }
        }

        public static void AddCss(WebControl cnt, string css)
        {
            string c = cnt.CssClass;
            if (String.IsNullOrEmpty(c))
                cnt.CssClass = css;
            else
            {
                string[] ca = c.ToLower().Split(' ');
                if (!ca.Contains(css.ToLower()))
                    cnt.CssClass += " " + css;
            }
        }

        public static void RemoveClass(HtmlControl cnt, string css)
        {
            string c = cnt.Attributes["class"];
            if (!String.IsNullOrEmpty(c))
                cnt.Attributes["class"] = c.Replace(css, "").Trim();
        }

        public static void RemoveClass(WebControl cnt, string css)
        {
            string c = cnt.Attributes["class"];
            if (!String.IsNullOrEmpty(c))
                cnt.Attributes["class"] = c.Replace(css, "").Trim();
        }

        public static void RemoveCss(WebControl cnt, string css)
        {
            string c = cnt.CssClass;
            if (!String.IsNullOrEmpty(c))
                cnt.CssClass = c.Replace(css, "").Trim();
        }

        public static void RemoveClassByPrefix(HtmlControl cnt, string cssPrefix)
        {
            string css = cnt.Attributes["class"];
            if (!String.IsNullOrEmpty(css))
            {
                string[] cA = css.Split(' ');
                css = null;
                foreach (string c in cA)
                    if (!c.StartsWith(cssPrefix))
                        css += c + " ";
                cnt.Attributes["class"] = css.Trim();
            }
        }

        public static void RemoveCssByPrefix(WebControl cnt, string cssPrefix)
        {
            string[] cA = cnt.CssClass.Split(' ');
            string css = null;
            foreach (string c in cA)
                if (!c.StartsWith(cssPrefix))
                    css += c + " ";
            cnt.CssClass = css.Trim();
        }
        //-------------------
        public static int StrToInt(string value, int def)
        {
            int ret;
            if (Int32.TryParse(value, out ret))
                return ret;
            else
                return def;
        }

        public static int? StrToInt(string value)
        {
            int ret;
            if (Int32.TryParse(value, out ret))
                return ret;
            else
                return null;
        }

        public static double StrToDouble(string value, double def)
        {
            double ret;
            if (Double.TryParse(value, out ret))
                return ret;
            else
                return def;
        }

        public static double? StrToDouble(string value)
        {
            double ret;
            if (Double.TryParse(value, out ret))
                return ret;
            else
                return null;
        }

        public static double ToDouble(object value, double def)
        {
            try
            {
                return db.isNull(value) ? def : Convert.ToDouble(value);
            }
            catch (Exception ex)
            {
                return def;
            }
        }

        //------------------------------------------------
        /*
        public static void dec2frac(double dbl, out int C, out int L, out int M)
        {
            char neg = ' ';
            double dblDecimal = dbl;
            if (dblDecimal == (int)dblDecimal) return dblDecimal.ToString(); //return no if it's not a decimal
            if (dblDecimal < 0)
            {
                dblDecimal = Math.Abs(dblDecimal);
                neg = '-';
            }
            var whole = (int)Math.Truncate(dblDecimal);
            string decpart = dblDecimal.ToString().Replace(Math.Truncate(dblDecimal) + ".", "");
            double rN = Convert.ToDouble(decpart);
            double rD = Math.Pow(10, decpart.Length);

            string rd = recur(decpart);
            int rel = Convert.ToInt32(rd);
            if (rel != 0)
            {
                rN = rel;
                rD = (int)Math.Pow(10, rd.Length) - 1;
            }
            //just a few prime factors for testing purposes
            var primes = new[] { 41, 43, 37, 31, 29, 23, 19, 17, 13, 11, 7, 5, 3, 2 };
            foreach (int i in primes) reduceNo(i, ref rD, ref rN);

            rN = rN + (whole * rD);
            return string.Format("{0}{1}/{2}", neg, rN, rD);
        }
         */

        public static double GreatestCommonDivisor(double m, double n) 
        { 
            var x = Math.Truncate(m); 
            var y = Math.Truncate(n); 
            return GreatestCommonDivisorWorker(x, y); 
        }

        static double GreatestCommonDivisorWorker(double m, double n) 
        { 
            return (n == 0) ? m : GreatestCommonDivisorWorker(n, m % n); 
        }

        public static void ReduceFraction(ref double m, ref double n) 
        { 
            double x = Math.Truncate(m);
            double y = Math.Truncate(n);
            double gcd = GreatestCommonDivisorWorker(m, n); 
            m = x / gcd; 
            n = y / gcd; 
        }

        public static void DecimalToFraction(double value, out int sign, out int numerator, out int denominator)
        {
            const double maxValue = double.MaxValue / 10;

            // e.g. .25/1 = (.25 * 100)/(1 * 100) = 25/100 = 1/4
            double tmpSign = value < 0 ? -1 : 1;
            double tmpNumerator = Math.Abs(value);
            double tmpDenominator = 1;

            // While numerator has a decimal value
            while ((tmpNumerator - Math.Truncate(tmpNumerator)) > 0 &&
                tmpNumerator < maxValue && tmpDenominator < maxValue)
            {
                tmpNumerator = tmpNumerator * 10;
                tmpDenominator = tmpDenominator * 10;
            }

            tmpNumerator = Math.Truncate(tmpNumerator); // Just in case maxValue boundary was reached.
            ReduceFraction(ref tmpNumerator, ref tmpDenominator);
            sign = (int)tmpSign;
            numerator = (int)tmpNumerator;
            denominator = (int)tmpDenominator;
        }
        /*
        public static string DecimalToFraction(decimal value)
        {
            var sign = decimal.One;
            var numerator = decimal.One;
            var denominator = decimal.One;
            DecimalToFraction(value, ref sign, ref numerator, ref denominator);
            //return string.Format("{0}/{1}", (sign * numerator).ToString().TruncateDecimal(), denominator.ToString().TruncateDecimal());
            return string.Format("{0}/{1}", (sign * numerator).ToString(), denominator.ToString());
        }
        */
        //------------------------------------------------
        public static bool DateIsValid(string d, out DateTime dt)
        {
            return DateTime.TryParse(d, out dt);
        }

        public static bool DateIsValid(string d)
        {
            DateTime dt;
            return DateIsValid(d, out dt);
        }

        public static string DateToStr(object d, string format)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            if (d.Equals(DBNull.Value))
                return null;
            else
                return Convert.ToDateTime(d).ToString(format);
        }

        public static string DateToStr(object d)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            if (d == null || d.Equals(DBNull.Value))
                return null;
            else
                return DateToStr(Convert.ToDateTime(d));
        }

        public static string DateTimeToStr(object d, string format)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            if (d == null || d.Equals(DBNull.Value))
                return null;
            else
                return ((DateTime)d).ToString(format);
            //return d.ToString();  // plomba
            //return DateToStr(d, "yyyy-MM-dd HH:mm:ss");
        }


        public static string DateTimeToStr(object d)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            return DateTimeToStr(d, "yyyy-MM-dd HH:mm:ss");
        }

        public static string DateTimeToStrHHMM(object d)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            return DateTimeToStr(d, "yyyy-MM-dd HH:mm");
        }

        public static string DateToStr(DateTime date)   // dla zmiennej typu data
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string DateToStrDb(DateTime date)   // do bazy bez ''
        {
            return date.ToString("yyyyMMdd");
        }

        public static string TimeToStr(DateTime time)
        {
            return time.ToString("HH:mm:ss");
        }

        public static string PrepareDate(string date)
        {
            char[] d = date.ToCharArray();
            if (date.Length >= 10 && d[2] == '/' && d[5] == '/')  // sukcesywnie dokładać inne formaty
                return String.Format("{0}-{1}-{2}", date.Substring(6, 4), date.Substring(3, 2), date.Substring(0, 2));
            else
                return date;
        }

        //----------------
        public static DateTime StrToDateTime(string date, DateTime defValue)
        {
            DateTime dt;
            if (StrToDateTime(date, out dt))
                return dt;
            else
                return defValue;
        }

        public static DateTime? StrToDateTime(string value)
        {
            DateTime ret;
            if (DateTime.TryParse(value, out ret))
                return ret;
            else
                return null;
        }

        public static bool StrToDateTime(string date, out DateTime dt)
        {
            return DateTime.TryParse(date, out dt);
        }

        public static TimeSpan StrToTimeSpan(string time)
        {
            TimeSpan ts;
            if (StrToTimeSpan(time, out ts))
                return ts;
            else
                return TimeSpan.MinValue;
        }

        public static bool StrToTimeSpan(string time, out TimeSpan ts)
        {
            return TimeSpan.TryParse(time, out ts);
        }
        //-----------------
        public static string SetDecimalSeparator(string sep)
        {
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            string prev = ci.NumberFormat.NumberDecimalSeparator;
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
            return prev;
        }

        //-----------------
        public static byte aSetBit(ref byte flag, byte bit, bool set)  // bits tez
        {
            if (set) flag = (byte)(flag | bit);
            else flag = (byte)(flag & (byte)~bit);
            return flag;
        }

        public static byte aSetBit(byte flag, byte bit, bool set)  // bits tez
        {
            if (set) flag = (byte)(flag | bit);
            else flag = (byte)(flag & (byte)~bit);
            return flag;
        }

        public static int aSetBit(ref int flag, int bit, bool set)  // bits tez
        {
            if (set) flag = flag | bit;
            else flag = flag & ~bit;
            return flag;
        }

        public static int aSetBit(int flag, int bit, bool set)  // bits tez
        {
            if (set) flag = flag | bit;
            else flag = flag & ~bit;
            return flag;
        }
        //---------------------------------------------------
        /*
         idea jest taka, że kontrolka ustawia FFFF a inne kontrolki sprawdzają i zerują swoje id zeby ponownie się nie odświeżyć
         session miało na celu danie możliwości odświeżenia w różnych oknach, ale się nie sprawdzi bo pierwsze i tak wyzeruje
         -> można dać do ViewState, alebo jeszcze coś wymyslić...
         póki co nie przeszkadza takie rozwiązanie
         */
        public static void ClearUpdated(string id)  // wywołać w !IsPostBack formatki/kontrolki
        {
            SetUpdated(id, 0xFFFFFFFF);
        }

        public static void SetUpdated(string id)    // po aktualizacji 
        {
            SetUpdated(id, 0);
        }

        public static void SetUpdated(string id, UInt32 excl_module)   // po aktualizacji jezeli kontrolka wywołująca nie mam być odświeżana
        {
            HttpContext.Current.Session["updid_" + id] = ~excl_module;
        }

        public static bool IsUpdated(string id, UInt32 module)     // sprawdzenie zeruje
        {
            string sesid = "updid_" + id;
            UInt32 u = GetUInt(HttpContext.Current.Session[sesid], 0);
            bool ret = (u & module) != 0;
            u = u & ~module;
            HttpContext.Current.Session[sesid] = u;
            return ret;
        }
        //-----
        public static uint GetUpdated(StateBag vs)                              // 1-odświeżaj
        {
            return Tools.GetUInt(vs["tabsupd"], 0); //0xFFFFFFFF);
        }

        public static void SetUpdated(StateBag vs, uint mask, uint module)      // po aktualizacji 
        {
            uint u = Tools.GetUInt(vs["tabsupd"], 0);
            u |= mask & module;
            vs["tabsupd"] = u;
        }

        public static void SetUpdated(StateBag vs)                              // po aktualizacji 
        {
            SetUpdated(vs, 0xFFFFFFFF, 0xFFFFFFFF);
        }

        public static void ClearUpdated(StateBag vs, uint module)               // wywołać w !IsPostBack formatki/kontrolki
        {
            SetUpdated(vs, module, 0);
        }

        public static bool IsUpdated(StateBag vs, uint module, bool clear)     // sprawdzenie zeruje
        {
            uint u = GetUpdated(vs);
            bool ret = (u & module) != 0;
            if (clear)
                SetUpdated(vs, module, 0);
            return ret;
        }

        public static bool IsUpdated(StateBag vs, uint module)     // sprawdzenie zeruje
        {
            return IsUpdated(vs, module, true);
        }
 
        //---------------------------------------------------
        public static string StrRepeat(string str, int ntimes)
        {
            if (ntimes == 0 || String.IsNullOrEmpty(str))
                return null;
            else
            {
                string ret = null;
                for (int i = 0; i < ntimes; i++)
                    ret += str;
                return ret;
                //return String.Join("", Enumerable.Repeat(str, ntimes));
                //return String.Concat(Enumerable.Repeat(str, ntimes));  wywala błąd
            }
        }

        /*
        public static class StringExtensions {     public static string Repeat(this string input, int count)     {         StringBuilder builder = new StringBuilder(             (input == null ? 0 : input.Length) * count);          for(int i = 0; i < count; i++) builder.Append(input);          return builder.ToString();     } }          
         */
        //---------------------------------------------------
        public static string UnicodeToUtf8(string ustr)
        {
            byte[] buff = Encoding.Convert(Encoding.Unicode,
                                           Encoding.UTF8,
                                           Encoding.Unicode.GetBytes(ustr));
            return Encoding.UTF8.GetString(buff);
        }

        public static string Utf8ToUnicode(string utf8)
        {
            byte[] buff = Encoding.Convert(Encoding.UTF8,
                                           Encoding.Unicode,
                                           Encoding.UTF8.GetBytes(utf8));
            return Encoding.Unicode.GetString(buff);
        }

        public static string Cp1250ToUnicode(string cps)
        {
            Encoding cp1250 = Encoding.GetEncoding(1250);
            byte[] buff = Encoding.Convert(cp1250,
                                           Encoding.Unicode,
                                           cp1250.GetBytes(cps));
            return Encoding.Unicode.GetString(buff);
        }

        public static string UnicodeToCp1250(string us)  // sprawdzić!!!
        {
            Encoding unc = Encoding.Unicode;
            byte[] buff = Encoding.Convert(unc,
                                           Encoding.GetEncoding(1250),
                                           unc.GetBytes(us));
            return Encoding.GetEncoding(1250).GetString(buff);
        }

        /*
        string unicodeString = "This string contains the unicode character Pi (\u03a0)";

        // Create two different encodings.
        Encoding ascii = Encoding.ASCII;
        Encoding unicode = Encoding.Unicode;

        // Convert the string into a byte array.
        byte[] unicodeBytes = unicode.GetBytes(unicodeString);

        // Perform the conversion from one encoding to the other.
        byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

        // Convert the new byte[] into a char[] and then into a string.
        char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
        ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
        string asciiString = new string(asciiChars);

        // Display the strings created before and after the conversion.
        Console.WriteLine("Original string: {0}", unicodeString);
        Console.WriteLine("Ascii converted string: {0}", asciiString);
         */

        //http://www.west-wind.com/weblog/posts/2007/Nov/28/Detecting-Text-Encoding-for-StreamReader
        public static Encoding GetFileEncoding(string srcFile, Encoding def)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)
            //Encoding enc = Encoding.Default;  // moze nie być 1250bo to sie na serwerze odpala
            Encoding enc = def; 
            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();
            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;
            return enc;
        }

        //------ IMPORT ------------------------------------------
        public static string FirstUpperWords(string s)
        {

            bool u = true;
            char[] chars = s.ToCharArray();
            for (int i = 0; i < s.Length; i++)
                if (u)
                {
                    char U = s.Substring(i, 1).ToUpper()[0];
                    if (s.Substring(i, 1).ToLower()[0] != U)  // jest to litera!
                    {
                        chars[i] = U;
                        u = false;
                    }
                }
                else
                    if (s.Substring(i, 1).ToLower() == s.Substring(i, 1).ToUpper())  // nie jest to litera!
                        u = true;
            return new string(chars);
        }

        public static string FirstUpper(string s)
        {
            switch (s.Length)
            {
                case 0:
                    return null;
                case 1: 
                    return s.ToUpper();
                default:
                    return s.Substring(0,1).ToUpper() + s.Substring(1);
            }
        }

        public static string Substring(string s, int start, int len)
        {
            if (start < 0) start = 0;
            if (s.Length < start)
                return null;
            else if (s.Length >= start + len)
                return s.Substring(start, len);
            else
                return s.Substring(start, s.Length - start);
        }

        public static string Left(string s, int len)
        {
            return Substring(s, 0, len);
        }

        public static string Right(string s, int len)
        {
            return Substring(s, s.Length - len, len);
        }

        public static string PrepareName(string s)
        {

            bool sp = true;
            char[] chars = s.ToLower().ToCharArray();
            for (int i = 0; i < s.Length; i++)
                if (sp)
                {
                    if (chars[i] != ' ' && chars[i] != '-')
                    {
                        sp = false;
                        chars[i] = chars[i].ToString().ToUpper()[0];
                    }
                }
                else
                    if (chars[i] == ' ' || chars[i] == '-')
                        sp = true;
            return new string(chars);
        }

        public static void GetNazwiskoImie(string NazwiskoImie, out string Nazwisko, out string Imie)
        {
            string s = NazwiskoImie.Trim();
            int p = s.LastIndexOf(" ");
            if (p > -1)
            {
                Nazwisko = FirstUpperWords(s.Substring(0, p).TrimEnd().ToLower());
                Imie = FirstUpperWords(s.Substring(p + 1, s.Length - p - 1).TrimStart().ToLower());
            }
            else
            {
                Nazwisko = NazwiskoImie;
                Imie = "";  // nie ustawiać null !!!
            }
        }

        public static string PreparePhoneNo(string phone)
        {
            string p = phone.Replace(" ", "").Replace("+","").Replace("-","").Replace(".","");
            if (p.Length >= 9)
            {
                p = p.Substring(p.Length - 9);
                //p = p.Insert(5, " ");    //521234567
                //p = p.Insert(2, " ");    //012345678
                try
                {
                    p = String.Format("{0:## ### ####}", Convert.ToInt64(p)); //52 123 4567
                }
                catch (Exception ex)
                {
                    p = null;
                }
            }
            return p;
        }

        //------------------------------------------------
        public static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
                return searchResult.Properties[PropertyName][0].ToString();
            else
                return string.Empty;
        }

        //http://stackoverflow.com/questions/246520/how-do-i-find-a-users-active-directory-display-name-in-a-c-web-application





        public static List<List<string>> GetADUsers(string adController, string Localization)
        {
            /*
             * pobiera dane-znajduje po loginie
             * -> nie ma to dopisuje: nazwisko, imie, login, mail 
             * -> jest to aktualizauje jak wyzej jesli pole = '' 
             */
            //DirectoryEntry entry = new DirectoryEntry("LDAP://jgbdc01.corp.jabil.org");
            //DirectoryEntry entry = new DirectoryEntry("LDAP://jgbdc01");


            List<List<string>> AD = new List<List<string>>();
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + adController);

                //entry.AuthenticationType = AuthenticationTypes.Secure; 

                DirectorySearcher dSearch = new DirectorySearcher(entry);

                //String Name = "Richmond";
                //dSearch.Filter = "(&(objectClass=user)(l=" + Name + "))";
                //dSearch.Filter = "(&(objectClass=user)(l=Bydgoszcz))";

                //dSearch.Filter = "(&(objectClass=user)(l=" + Localization + "))";

                dSearch.Filter = "(&(objectClass=user)(OU=JGSBydgoszcz))";
                SearchResultCollection sr = dSearch.FindAll();

                foreach (SearchResult sResultSet in sr)
                {
                    List<string> data = new List<string>();
                    data.Add(GetProperty(sResultSet, "cn"));  //name
                    data.Add(GetProperty(sResultSet, "sAMAccountName"));  // login
                    data.Add(GetProperty(sResultSet, "mail"));  // mail 

                    //string logindomain = GetProperty(sResultSet, "userPrincipalName");  // login@domain
                    //string manager = GetProperty(sResultSet, "manager");

                    data.Add(GetProperty(sResultSet, "givenName"));  //firstname
                    data.Add(GetProperty(sResultSet, "SN"));    // lastanme

                    AD.Add(data);
                }

                return AD;
                //return sr.Count;
            }
            catch (Exception ex)
            {
                throw;      // tu zdaje sie i tak wywalał exception, którego nie dąło się ukryć ...
                //return -1;
            }

            /*
            // Login Name
            Console.WriteLine(GetProperty(sResultSet, "cn"));
            // First Name
            Console.WriteLine(GetProperty(sResultSet, "givenName"));
            // Middle Initials
            Console.Write(GetProperty(sResultSet, "initials"));
            // Last Name
            Console.Write(GetProperty(sResultSet, "sn"));
            // Address
            string tempAddress = GetProperty(sResultSet, "homePostalAddress");
            if (tempAddress != string.Empty)
            {
                string[] addressArray = tempAddress.Split(';');
                string taddr1, taddr2;
                taddr1 = addressArray[0];
                Console.Write(taddr1);
                taddr2 = addressArray[1];
                Console.Write(taddr2);
            }
            // title
            Console.Write(GetProperty(sResultSet, "title"));
            // company
            Console.Write(GetProperty(sResultSet, "company"));
            //state
            Console.Write(GetProperty(sResultSet, "st"));
            //city
            Console.Write(GetProperty(sResultSet, "l"));
            //country
            Console.Write(GetProperty(sResultSet, "co"));
            //postal code
            Console.Write(GetProperty(sResultSet, "postalCode"));
            // telephonenumber
            Console.Write(GetProperty(sResultSet, "telephoneNumber"));
            //extention
            Console.Write(GetProperty(sResultSet, "otherTelephone"));
            //fax
            Console.Write(GetProperty(sResultSet, "facsimileTelephoneNumber"));
            // email address
            Console.Write(GetProperty(sResultSet, "mail"));
            // Challenge Question
            Console.Write(GetProperty(sResultSet, "extensionAttribute1"));
            // Challenge Response
            Console.Write(GetProperty(sResultSet, "extensionAttribute2"));
            //Member Company
            Console.Write(GetProperty(sResultSet, "extensionAttribute3"));
            // Company Relation ship Exits
            Console.Write(GetProperty(sResultSet, "extensionAttribute4"));
            //status
            Console.Write(GetProperty(sResultSet, "extensionAttribute5"));
            // Assigned Sales Person
            Console.Write(GetProperty(sResultSet, "extensionAttribute6"));
            // Accept T and C
            Console.Write(GetProperty(sResultSet, "extensionAttribute7"));
            // jobs
            Console.Write(GetProperty(sResultSet, "extensionAttribute8"));
            String tEamil = GetProperty(sResultSet, "extensionAttribute9");
            // email over night
            if (tEamil != string.Empty)
            {
                string em1, em2, em3;
                string[] emailArray = tEmail.Split(';');
                em1 = emailArray[0];
                em2 = emailArray[1];
                em3 = emailArray[2];
                Console.Write(em1 + em2 + em3);
            }
            // email daily emerging market
            Console.Write(GetProperty(sResultSet, "extensionAttribute10"));
            // email daily corporate market
            Console.Write(GetProperty(sResultSet, "extensionAttribute11"));
            // AssetMgt Range
            Console.Write(GetProperty(sResultSet, "extensionAttribute12"));
            // date of account created
            Console.Write(GetProperty(sResultSet, "whenCreated"));
            // date of account changed
            Console.Write(GetProperty(sResultSet, "whenChanged"));
             */
        }



        //----------------------------------------------------------------------------------------
        /* cos nie wychodzi, zwraca większy rozmiar
        public static string ZipFile(string source, string dest)
        {
            FileStream sourceFile = File.OpenRead(source);
            if (String.IsNullOrEmpty(dest))
                dest = Path.ChangeExtension(source, ".zip");
            FileStream destFile = File.Create(dest);
            GZipStream compStream = new GZipStream(destFile, CompressionMode.Compress);
            try
            {
                int theByte = sourceFile.ReadByte();
                while (theByte != -1)
                {
                    compStream.WriteByte((byte)theByte);
                    theByte = sourceFile.ReadByte();
                }
            }
            finally
            {
                sourceFile.Close();
                destFile.Close();

                compStream.Dispose();
            }
            return dest;
        }
        */

        public static string FileToGZip1(string source, string dest)
        {
            if (String.IsNullOrEmpty(dest))
                dest = Path.ChangeExtension(source, ".zip");  // chociaż to powinno być .gz ale to nawet lepiej bo nie wszystko otworzy 7zip otwiera ...
            FileStream fsSource = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                byte[] bufferWrite = new byte[fsSource.Length];
                fsSource.Read(bufferWrite, 0, bufferWrite.Length);
                FileStream fsDest = new FileStream(dest, FileMode.OpenOrCreate, FileAccess.Write);
                try
                {
                    GZipStream gzCompressed = new GZipStream(fsDest, CompressionMode.Compress, true);
                    try
                    {
                        gzCompressed.Write(bufferWrite, 0, bufferWrite.Length);
                    }
                    finally { gzCompressed.Close(); }
                }
                finally { fsDest.Close(); }
            }
            finally { fsSource.Close(); }
            return dest;
        }
        //--------------------------------------------
        static void CopyStream(Stream input, Stream output)
        {
            const int bufferSize = 4096;
            int read;
            byte[] buffer = new byte[bufferSize];
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);
        }

        public static string FileToGZip(string source, string dest)
        {
            if (String.IsNullOrEmpty(dest))
                dest = Path.ChangeExtension(source, ".zip");  // chociaż to powinno być .gz ale to nawet lepiej bo nie wszystko otworzy 7zip otwiera ...
            using (Stream input = File.OpenRead(source))
            using (Stream output = File.OpenWrite(dest))
            using (Stream gz = new NamedGZipStream(output, Path.GetFileName(source), true))
                CopyStream(input, gz);
            return dest;
        }

        public static MemoryStream FileToGZipStream(string source)
        {
            MemoryStream output = new MemoryStream();
            using (Stream input = File.OpenRead(source))
            using (Stream gz = new NamedGZipStream(output, Path.GetFileName(source), true))
                CopyStream(input, gz);
            output.Position = 0;
            return output;
        }

        public static void GZipToFile(string source, string dest) // >>> przetestować !!!
        {
            using (Stream input = File.OpenRead(source))
            using (Stream output = File.OpenWrite(dest))
            using (Stream gz = new GZipStream(input, CompressionMode.Decompress, true))
                CopyStream(gz, output);
        }

        public static MemoryStream GZipToStream(string source) // >>> przetestować !!!
        {
            MemoryStream output = new MemoryStream();

            using (Stream input = File.OpenRead(source))
            using (Stream gz = new GZipStream(input, CompressionMode.Decompress, true))
                CopyStream(gz, output);

            output.Position = 0;
            return output;
        }

        //-------------------------------------------
        public static bool GetDBBackup_1(out MemoryStream zip, out string zipname)  // plik .bak sie zapisuje a plik .zip tworzy w pamięci
        {
            string fname = Tools.GetAppPath(@"uploads\HR_PRP_" + DateTime.Today.ToString("yyyyMMdd") + ".bak");
            File.Delete(fname);
            Base.execSQL("backup database HR_PRP to disk = " + Base.strParam(fname));
            FileInfo file = new FileInfo(fname);
            if (file.Exists)
            {
                zipname = Path.ChangeExtension(Path.GetFileName(fname), ".zip");  // chociaż to powinno być .gz ale to nawet lepiej bo nie wszystko otworzy 7zip otwiera ...
                zip = Tools.FileToGZipStream(fname);
                return true;
            }
            else
            {
                zipname = null;
                zip = null;
                return false;
            }
        }

        public static bool GetDBBackup(out MemoryStream zip, out string zipname)  // plik .bak sie zapisuje a plik .zip tworzy w pamięci
        {
            string fname = Tools.GetAppPath(@"uploads\HR_PRP_" + DateTime.Today.ToString("yyyyMMdd") + ".bak");
            SqlConnection con = Base.Connect();
            /*
            Base.ToFile(con, fname, true, "Ankiety");
            Base.ToFile(con, fname, false, "AnkietyDokumenty");
            Base.ToFile(con, fname, false, "AnkietyOdpowiedzi");
            Base.ToFile(con, fname, false, "AnkietyPytania");
            Base.ToFile(con, fname, false, "AnkietySkalaOcen");
            Base.ToFile(con, fname, false, "Informacje");
            Base.ToFile(con, fname, false, "Log");
            Base.ToFile(con, fname, false, "Mailing");
            Base.ToFile(con, fname, false, "Monity");
            Base.ToFile(con, fname, false, "Notes");
            Base.ToFile(con, fname, false, "Pracownicy");
            Base.ToFile(con, fname, false, "Programy");
            Base.ToFile(con, fname, false, "Stanowiska");
            Base.ToFile(con, fname, false, "Szkolenia");
            Base.ToFile(con, fname, false, "SzkoleniaGrupy");
            Base.ToFile(con, fname, false, "Ustawienia");
             */
            Base.Disconnect(con);
            FileInfo file = new FileInfo(fname);
            if (file.Exists)
            {
                zipname = Path.ChangeExtension(Path.GetFileName(fname), ".zip.txt");  // chociaż to powinno być .gz ale to nawet lepiej bo nie wszystko otworzy 7zip otwiera ...
                zip = FileToGZipStream(fname);
                return true;
            }
            else
            {
                zipname = null;
                zip = null;
                return false;
            }
        }

        public static void DbImport(string zipname)
        {
            MemoryStream zip = GZipToStream(zipname);
            Base.ImportData(zip);
        }

        public static bool DBBackupDownload()  // plik .bak sie zapisuje a plik .zip tworzy w pamięci
        {
            string zipname;
            MemoryStream zip;
            if (GetDBBackup(out zip, out zipname))
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", zipname));
                HttpContext.Current.Response.AddHeader("Content-Length", zip.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/zip";
                byte[] bzip = zip.ToArray();
                HttpContext.Current.Response.OutputStream.Write(bzip, 0, bzip.Length);
                HttpContext.Current.Response.End();
                return true; // tu sie to nie wykona i tak, ale zeby kompilator był szczęśliwy ...
            }
            else
                return false;
        }
        /*
        public static bool DBMail(string email)
        {
            string zipname;
            MemoryStream zip;
            if (GetDBBackup(out zip, out zipname))
            {
                string subject = "PRP Data";
                string body = "PRP Database backup: " + zipname;
                Log.Info(Log.t2MAILTOSEND, email + (char)13 + subject, body, Log.OK);
                return Mailing.SendMail2(email, subject, body, zip, zipname, "application/zip");
            }
            else
            {
                Log.Error(Log.t2APP, "DBMail", email + (char)13 + zipname);
                return false;
            }
        }
        */
        /*
        public static bool DBBackupDownload()  // plik .bak sie zapisuje a plik .zip tworzy w pamięci
        {
            string fname = Tools.GetAppPath(@"uploads\HR_PRP_" + DateTime.Today.ToString("yyyyMMdd") + ".bak");
            File.Delete(fname);

            Base.execSQL("backup database HR_PRP to disk = " + Base.strParam(fname));
            FileInfo file = new FileInfo(fname);
            if (file.Exists)
            {
                string zipname = Path.ChangeExtension(Path.GetFileName(fname), ".zip");  // chociaż to powinno być .gz ale to nawet lepiej bo nie wszystko otworzy 7zip otwiera ...

                MemoryStream zip = Tools.FileToGZipStream(fname);
                File.Delete(fname);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + zipname);
                HttpContext.Current.Response.AddHeader("Content-Length", zip.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/zip";
                byte[] bzip = zip.ToArray();
                HttpContext.Current.Response.OutputStream.Write(bzip, 0, bzip.Length);
                HttpContext.Current.Response.End();
                return true; // tu sie to nie wykona i tak, ale zeby kompilator był szczęśliwy ...
            }
            else
                return false;
        }

        public static bool DBMail(string email)
        {
            string fname = Tools.GetAppPath(@"uploads\HR_PRP_" + DateTime.Today.ToString("yyyyMMdd") + ".bak");
            File.Delete(fname);
            Base.execSQL("backup database HR_PRP to disk = " + Base.strParam(fname));
            FileInfo file = new FileInfo(fname);
            if (file.Exists)
            {
                string zipname = Path.ChangeExtension(Path.GetFileName(fname), ".zip");  // chociaż to powinno być .gz ale to nawet lepiej bo nie wszystko otworzy 7zip otwiera ...
                MemoryStream zip = FileToGZipStream(fname);
                File.Delete(fname);
                string subject = "PRP Data";
                string body = "PRP Database backup: " + zipname;
                Log.Info(Log.t2MAILTOSEND, email + (char)13 + subject, body, Log.OK);
                return Mailing.SendMail2(email, subject, body, zip, zipname, "application/zip");
            }
            else
            {
                Log.Error(Log.t2APP, "DBMail", email + (char)13 + fname);
                return false;
            }
        }
         */

        //----------------------------------------------------------------------------------------
        public static string SelectItem(ListControl lc, string selectedValue)
        {
            lc.SelectedIndex = -1;   // gasze wsyzstkie inne selected
            ListItem li = lc.Items.FindByValue(selectedValue);
            if (li != null)
            {
                //lc.SelectedIndex = -1;   // gasze wsyzstkie inne selected
                li.Selected = true;
                return li.Text;
            }
            else
                return null;
        }

        public static int SelectItem2(ListControl lc, string selectedValue)
        {
            lc.SelectedIndex = -1;   // gasze wsyzstkie inne selected
            ListItem li = lc.Items.FindByValue(selectedValue);
            if (li != null) li.Selected = true;
            return lc.SelectedIndex;
        }

        public static string SelectItemByParam(ListControl lc, int param, string selectedValue)
        {
            lc.SelectedIndex = -1;   // gasze wszystkie inne selected
            foreach (ListItem li in lc.Items)
            {
                string p = Tools.GetLineParam(li.Value, param);
                if (p == selectedValue)
                {
                    li.Selected = true;
                    return li.Text;
                }
            }
            return null;
        }

        public static string SelectItemByParam(Control c, string cname, int param, object selectedValue)
        {
            ListControl lc = (ListControl)c.FindControl(cname);
            if (lc != null)
                return SelectItemByParam(lc, param, selectedValue.ToString());
            else
                return null;
        }

        public static string SelectItem(Control c, string cname, object selectedValue)
        {
            ListControl lc = (ListControl)c.FindControl(cname);
            if (lc != null)
            {
                lc.SelectedIndex = -1;   // gasze wsyzstkie inne selected
                if (selectedValue != null)
                {
                    string v = selectedValue.ToString();
                    ListItem li = lc.Items.FindByValue(v);
                    if (li != null)
                    {
                        //lc.SelectedIndex = -1;   // gasze wsyzstkie inne selected
                        li.Selected = true;
                        return li.Text;
                    }
                }
                return null;
            }
            else 
                return null;
        }

        public static string SelectItem(ListControl lc, object selectedValue)
        {
            if (lc != null)
            {
                lc.SelectedIndex = -1;   // gasze wsyzstkie inne selected
                string v = selectedValue.ToString();
                ListItem li = lc.Items.FindByValue(v);
                if (li != null)
                {
                    //lc.SelectedIndex = -1;   // gasze wsyzstkie inne selected
                    li.Selected = true;
                    return li.Text;
                }
                else
                    return null;
            }
            else
                return null;
        }


        /*
        public static bool SelectItem(ListControl lc, string selectedValue)
        {
            ListItem li = lc.Items.FindByValue(selectedValue);
            if (li != null)
            {
                lc.SelectedIndex = -1;
                li.Selected = true;
                return true;
            }
            else
                return false;
        }
         */

        public static ListItem SetDefaultItem(ListControl lc, string defValue, string defPrefix, string defSuffix)
        {
            if (!String.IsNullOrEmpty(defValue))
            {
                ListItem li = lc.Items.FindByValue(defValue);
                if (li != null)
                {
                    li.Text = defPrefix + li.Text + defSuffix;
                    return li;
                }
            }
            return null;
        }

        public static void SelectItem(ListControl lc, string selValue, string defValue, bool unsetFirst)
        {               //•√●«»
            const string defPrefix = "» ";
            const string defSuffix = " «";
            SelectItem(lc, selValue, defValue, unsetFirst, defPrefix, defSuffix);
        }

        public static ListItem SelectItem(ListControl lc, string selValue, string defValue, bool unsetFirst, string prefix, string suffix)
        {                   // zwraca defaultItem - slected mozna sobie wziac z ddl !
            bool fs = !String.IsNullOrEmpty(selValue);
            bool fd = !String.IsNullOrEmpty(defValue);
            lc.SelectedIndex = -1;   // gasze wszystkie inne selected
            ListItem defItem = null;

            if (unsetFirst)
            {
                foreach (ListItem li in lc.Items)
                {
                    string t = li.Text;
                    if (!String.IsNullOrEmpty(prefix)) t = t.Replace(prefix, "");   // string EndsWith moze lepsze ?
                    if (!String.IsNullOrEmpty(suffix)) t = t.Replace(suffix, "");
                    li.Text = t;
                    //li.Text = li.Text.Replace(prefix, "").Replace(suffix, "");
                    if (fd && li.Value == defValue)
                    {
                        li.Text = prefix + li.Text + suffix;
                        defItem = li;
                    }
                    if (fs && li.Value == selValue)
                        li.Selected = true;
                }
            }
            else
            {
                ListItem li;
                if (fd)
                {
                    li = lc.Items.FindByValue(defValue);
                    if (li != null)
                    {
                        li.Text = prefix + li.Text + suffix;
                        defItem = li;
                    }
                }
                if (fs)
                {
                    li = lc.Items.FindByValue(selValue);
                    if (li != null)
                        li.Selected = true;
                }
            }
            return defItem;
        }

        /*
        public static void SelectItem(ListControl lc, string selValue, string defValue, bool unsetFirst, string prefix, string suffix)
        {
            //•√●«»
            const string defPrefix = "» ";
            const string defSuffix = " «";

            bool fs = !String.IsNullOrEmpty(selValue);
            bool fd = !String.IsNullOrEmpty(defValue);
            if (fs) lc.SelectedIndex = -1;   // gasze wszystkie inne selected

            if (unsetFirst)
            {
                foreach (ListItem li in lc.Items)
                {
                    li.Text = li.Text.Replace(defPrefix, "").Replace(defSuffix, "");
                    if (fd && li.Value == defValue)
                        li.Text = defPrefix + li.Text + defSuffix;
                    if (fs && li.Value == selValue)
                        li.Selected = true;
                }
            }
            else
            {
                ListItem li;
                if (fd)
                {
                    li = lc.Items.FindByValue(defValue);
                    if (li != null)
                        li.Text = defPrefix + li.Text + defSuffix;
                }
                if (fs)
                {
                    li = lc.Items.FindByValue(selValue);
                    if (li != null)
                        li.Selected = true;
                }
            }
        }
         */
        //---------------------
        public static ListControl BindData(ListViewItem item, string cname, DataSet ds, string textField, string valueField, bool fAddChooseStr, string selectedValue)
        {
            ListControl lc = (ListControl)item.FindControl(cname);
            if (lc != null)
                BindData(lc, ds, textField, valueField, fAddChooseStr, selectedValue);
            return lc;
        }

        public static ListControl BindData(ListViewItem item, string cname, DataSet ds, string textField, string valueField, bool fAddChooseStr, string selectedValue, string selectedText)  // na wypadek kiedy nie ma selected na liscie - jest on dodawany na 1 pozycji po wybierz...
        {
            ListControl lc = (ListControl)item.FindControl(cname);
            if (lc != null)
                BindData(lc, ds, textField, valueField, fAddChooseStr, selectedValue, selectedText);
            return lc;
        }

        public static void AddChooseStr(ListControl lc, bool select)
        {
            lc.Items.Insert(0, new ListItem(L.p("wybierz ..."), ""));   // jak null zamiast "" to przyjmie Value=Text, a tak selected zwroci ""
            //lc.Items[0].Value = null;                                   // tak się nie da - ustawia tu value = text
            if (select)
            {
                lc.SelectedIndex = -1;                                  // trzeba zgasić obecnie zaznaczone
                lc.Items[0].Selected = true;
            }
        }

        public static void BindData(ListControl lc, DataSet ds, string textField, string valueField, bool fAddChooseStr, string selectedValue)
        {
            lc.DataSource = ds;
            lc.DataTextField = textField;
            lc.DataValueField = valueField;
            lc.DataBind();
            if (fAddChooseStr)
                //lc.Items.Insert(0, new ListItem("wybierz ...", ""));  // jak null zamiast "" to przyjmie Value=Text, a tak selected zwroci ""
                AddChooseStr(lc, false);
            if (selectedValue != null)
                SelectItem(lc, selectedValue);
            else
            {
                lc.SelectedIndex = 0;
                //lc.Text = lc.SelectedItem.Text;  wywala błąd ze element nie znaleziony na liscie
            }
        }

        public static void BindData(ListControl lc, DataSet ds, string textField, string valueField, bool fAddChooseStr, string selectedValue, string selectedText)
        {
            lc.DataSource = ds;
            lc.DataTextField = textField;
            lc.DataValueField = valueField;
            lc.DataBind();
            if (fAddChooseStr)
                //lc.Items.Insert(0, new ListItem("wybierz ...", ""));  // jak null zamiast "" to przyjmie Value=Text, a tak selected zwroci ""
                AddChooseStr(lc, false);
            if (selectedValue != null)
            {
                string sel = SelectItem(lc, selectedValue);
                if (String.IsNullOrEmpty(sel))
                {
                    int idx = fAddChooseStr ? 1 : 0;
                    lc.Items.Insert(idx, new ListItem(selectedText, selectedValue));
                    lc.SelectedIndex = idx;
                }
            }
            else
            {
                lc.SelectedIndex = 0;
                //lc.Text = lc.SelectedItem.Text;  wywala błąd ze element nie znaleziony na liscie
            }
        }

        public static void BindData2(ListControl lc, DataSet ds, string textField, string valueField, bool fAddChooseStr, string selectedValue)
        {
            lc.ClearSelection();
            lc.Items.Clear();
            if (fAddChooseStr)
                //lc.Items.Add(new ListItem("wybierz ...", ""));  // jak null zamiast "" to przyjmie Value=Text, a tak selected zwroci ""
                AddChooseStr(lc, false);
            foreach (DataRow dr in db.getRows(ds))
                lc.Items.Add(new ListItem(db.getValue(dr, textField), db.getValue(dr, valueField)));
            if (selectedValue != null)
                SelectItem(lc, selectedValue);
            else
            {
                lc.SelectedIndex = 0;
                //lc.Text = lc.SelectedItem.Text;    wywala błąd ze element nie znaleziony na liscie
            }
        }

        public static int BindData(ListControl lc, DataSet ds, string textField, string valueField1, string valueField2, string valueField3, string selectedValue1)  // Value ma listę parametrów, niewykorzytsane parametry - ustawić na null, przyjmuje "" jako poprawny parametr
        {
            int idx = -1;
            int pcnt = 1;
            if (valueField2 != null) pcnt = 2;
            else if (valueField3 != null) pcnt = 3;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string v1 = dr[valueField1].ToString();
                ListItem li = new ListItem(dr[textField].ToString(),
                                           Tools.SetLineParams(pcnt,
                                                v1,
                                                pcnt >= 2 ? dr[valueField2].ToString() : null,
                                                pcnt >= 3 ? dr[valueField3].ToString() : null,
                                                null, null, null)
                                           );
                lc.Items.Add(li);
                if (v1 == selectedValue1)
                {
                    li.Selected = true;
                    idx = lc.Items.Count - 1;
                }
            }
            return idx;     // zwraca selected index lub -1
        }

        public static void PrepareHTML(DataSet ds, int col)  // jak tree z wcięciami i &nbsp;
        {
            foreach (DataRow dr in db.getRows(ds))
                dr[col] = HttpUtility.HtmlDecode(dr[col].ToString());
        }
        //---------
        public static Repeater RepeaterDataBind(Control item, string name, DataTable td)  // np do Repeater, ListView itd
        {
            Repeater rp = (Repeater)item.FindControl(name);
            if (rp != null)
            {
                rp.DataSource = td;
                rp.DataBind();
            }
            return rp;
        }

        public static Repeater RepeaterDataBind(Control item, string name, DataSet ds)
        {
            return RepeaterDataBind(item, name, ds.Tables[0]);
        }
        //---------------------------------------------------------------------------
        public static bool x_SendMail(string to_email, string subject, string message)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            MailMessage objMail = new MailMessage();
            objMail.From = settings.Email;
            objMail.To = to_email;
            objMail.Subject = subject;
            objMail.Body = message;

            objMail.BodyEncoding = Encoding.UTF8;
            //SmtpMail.SmtpServer = "corimc04.corp.jabil.org";


            SmtpMail.SmtpServer = settings._SMTPSerwer;
            
            try
            {
                SmtpMail.Send(objMail);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "SendMail", ex.Message);
                return false;
            }
        }

        public static void PrepareZnaczniki(ref string text, DataSet data)   
        {
            PrepareZnaczniki(ref text, data, 0, 0, -1);   // wszystkie pola
        }

        public static void PrepareZnaczniki(ref string text, DataSet data, int row, int ofs, int cnt)   //ofs = -1 bierze z pierwszej kolumny (zgodność z Mailing.PrepareMailText3), cnt = -1 wszystko
        {
            if (data != null)
            {
                DataRow dr = db.getRow(data, row);
                int c = data.Tables[0].Columns.Count;
                if (dr != null)
                {
                    int zofs = ofs == -1 ? db.getInt(dr, 0, 0) : ofs;    //na 0 jest offset, ilość zawsze do końca
                    int zcnt = cnt == -1 ? c : cnt;
                    if (zofs + zcnt > c) zcnt = c - zofs;  //kontrola
                    int zlast = zofs + zcnt;
                    for (int i = ofs; i < zlast; i++)
                    {
                        DataColumn dc = data.Tables[0].Columns[i];
                        text = text.Replace("%" + dc.Caption + "%", dr[i].ToString());
                    }
                }
                else  //20160612 SqlBoxes
                {
                    for (int i = 0; i < c; i++)
                    {
                        DataColumn dc = data.Tables[0].Columns[i];
                        text = text.Replace("%" + dc.Caption + "%", "?");
                    }
                }
            }
        }
        //---------------------------------------------------------------------------
        public static bool Equals(IOrderedDictionary v1, IOrderedDictionary v2)
        {
            if (v1.Count != v2.Count) return false;
            else
                foreach (string key in v1.Keys)
                {
                    object o1 = v1[key];
                    object o2 = v2[key];
                    if (o1 == null || o2 == null) return false;
                    else
                        if (o1.ToString() != o2.ToString()) return false;
                }
            return true;
        }
        //----------------------------------------------------------------------------------------
        public const int limSelect = 0;
        public const int limEdit = 1;
        public const int limInsert = 2;
        public const int limOther = -1;

        /*
        protected void lv_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            int lim = Tools.GetListItemMode(e, lv, out drv);
            switch (lim)
            {
                case Tools.limSelect:
                    break;
                case Tools.limEdit:
                    break;
                case Tools.limInsert:
                    break;
                case Tools.limOther:
                    break;
            }
        }
         */

        public static int GetListItemMode(ListViewItemEventArgs e, ListView lv)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.InsertItem:
                    return limInsert;
                case ListViewItemType.DataItem:
                    if (((ListViewDataItem)e.Item).DisplayIndex == lv.EditIndex)// && lv.EditItem != null) <<<< w lv_DataBound po kliknięciu Zapisz EditItem == null i nie idzie np wypełnienie combo konieczne do przywrócenia z viewstate selecteditem
                        return limEdit;
                    else
                        return limSelect;
                default:
                    return limOther;
            }
        }

        public static int GetListItemMode(ListViewItemEventArgs e, ListView lv, out DataRowView drv)
        {
            int lim = GetListItemMode(e, lv);
            drv = null;
            switch (lim)
            {
                case limSelect:
                case limEdit:
                    ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                    drv = (DataRowView)dataItem.DataItem;
                    break;
            }
            return lim;
        }

        public static int GetListItemMode(ListViewItemEventArgs e, ListView lv, out bool select, out bool edit, out bool insert)
        {
            select = false;
            edit = false;
            insert = false;
            int lim = GetListItemMode(e, lv);
            switch (lim)
            {
                case limSelect:
                    select = true;
                    break;
                case limEdit:
                    edit = true;
                    break;
                case limInsert:
                    insert = true;
                    break;
            }
            return lim;
        }
        
        public static DataRowView GetDataRowView(ListViewItemEventArgs e)
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            return (DataRowView)dataItem.DataItem;
        }

        public static int GetDisplayIndex(ListViewCommandEventArgs e)
        {
            return ((ListViewDataItem)e.Item).DisplayIndex;
        }

        public static int GetDisplayIndex(ListViewItemEventArgs e)
        {
            return ((ListViewDataItem)e.Item).DisplayIndex;
        }

        public static string GetDataKey(ListView lv, ListViewCommandEventArgs e)
        {
            //return lv.DataKeys[((ListViewDataItem)e.Item).DataItemIndex].Value.ToString();
            return lv.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value.ToString();
        }

        public static string GetDataKey(ListView lv, ListViewItem item)
        {
            return lv.DataKeys[((ListViewDataItem)item).DisplayIndex].Value.ToString();
        }

        public static string GetDataKey(ListView lv, ListViewDeleteEventArgs e)
        {
            return lv.DataKeys[e.ItemIndex].Value.ToString();
        }

        public static string GetDataKey(ListView lv, ListViewUpdateEventArgs e)
        {
            return lv.DataKeys[e.ItemIndex].Value.ToString();
        }

        public static string GetDataKeyEdited(ListView lv)
        {
            if (lv.EditIndex != -1)
                return lv.DataKeys[lv.EditIndex].Value.ToString();
            else
                return null;
        }

        public static string GetDataKeySelected(ListView lv)
        {
            if (lv.SelectedIndex != -1)
                return lv.DataKeys[lv.SelectedIndex].Value.ToString();      // SelctedDataKey
            else
                return null;
        }

        //-------------------------------------------------
        public static void SetData(Control cnt, DataRowView drv)   // ustawia wartości kontrolkom na podstawie DataRowView, kontrolki muszą mieć ID jak pola !!!
        {
            foreach (Control c in cnt.Controls)
            {
                if (c is HtmlTable) SetData(c, drv);
                else if (c is HtmlTableRow) SetData(c, drv);
                else if (c is HtmlTableCell) SetData(c, drv);
                else if (c is TextBox)
                    ((TextBox)c).Text = drv[c.ID].ToString();
                else if (c is CheckBox)
                    ((CheckBox)c).Checked = (bool)drv[c.ID];
                else if (c is DropDownList)
                    Tools.SelectItem(c as DropDownList, drv[c.ID]);
            }
        }

        public static void SetData(Control cnt, DataSet ds)   // ustawia wartości kontrolkom na podstawie DataRowView, kontrolki muszą mieć ID jak pola !!!
        {
            DataRow dr = db.getRow(ds);
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                Control c = Tools.FindControl(cnt, col.Caption);
                if (c != null)
                    if (c is TextBox)
                        ((TextBox)c).Text = dr[c.ID].ToString();
                    else if (c is CheckBox)
                        ((CheckBox)c).Checked = (bool)dr[c.ID];
                    else if (c is DropDownList)
                        Tools.SelectItem(c as DropDownList, dr[c.ID]);
            }
        }
        //-----
        public static void GetData(Control cnt, IOrderedDictionary values)   // pobiera wartości z kontrolek i zwraca jak do ListView, kontrolki muszą mieć ID jak pola !!!
        {
            foreach (Control c in cnt.Controls)
            {
                if (c is HtmlTable) GetData(c, values);
                else if (c is HtmlTableRow) GetData(c, values);
                else if (c is HtmlTableCell) GetData(c, values);
                else if (c is TextBox)
                    values[c.ID] = ((TextBox)c).Text;
                else if (c is CheckBox)
                    values[c.ID] = ((CheckBox)c).Checked;
                else if (c is DropDownList)
                    values[c.ID] = ((DropDownList)c).SelectedValue;
            }
        }

        public static object[] GetData(Control cnt, string fields, params string[] values)   // powinno zwracać db.nullStrParam, ale zrobię to powyżej ..., values - wartości dla pierwszych pól na fields
        {
            int vlen = values != null ? values.Length : 0;
            string[] ff = fields.Split(',');
            object[] val = new object[ff.Length];
            for (int i = 0; i < ff.Length; i++)
            {
                string f = ff[i].Trim();
                if (!String.IsNullOrEmpty(f))
                {
                    val[i] = null;
                    if (i < vlen)
                    {
                        val[i] = values[i];
                    }
                    else
                    {
                        Control c = Tools.FindControl(cnt, f);
                        if (c is TextBox)
                            val[i] = ((TextBox)c).Text;
                        else if (c is CheckBox)
                            val[i] = ((CheckBox)c).Checked;
                        else if (c is DropDownList)
                            val[i] = ((DropDownList)c).SelectedValue;
                    }
                }
            }
            return val;
        }

        public static object[] GetData(Control cnt, string fields)   // powinno zwracać db.nullStrParam, ale zrobię to powyżej ..., values - wartości dla pierwszych pól na fields
        {
            return GetData(cnt, fields, null);
            /*
            string[] ff = fields.Split(',');
            object[] val = new object[ff.Length];
            for (int i = 0; i < ff.Length; i++)
            {
                string f = ff[i].Trim();
                val[i] = null;
                if (!String.IsNullOrEmpty(f))
                {
                    Control c = Tools.FindControl(cnt, f);
                    if (c is TextBox)
                        val[i] = ((TextBox)c).Text;
                    else if (c is CheckBox)
                        val[i] = ((CheckBox)c).Checked;
                    else if (c is DropDownList)
                        val[i] = ((DropDownList)c).SelectedValue;
                }
            }
            return val;
            */
        }

        //-----------------------------------------------------------------------------------------
        /*
        public static void ListViewInitItem(ListView lv, ListViewItemEventArgs e, bool create)
        {
            if (create)
            {
                bool select, edit, insert;
                int lim = Tools._GetListItemMode(e, lv, out select, out edit, out insert);
                switch (lim)
                {
                    case limSelect:
                        MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        SetButton(e.Item, "EditButton", "Edytuj");
                        SetButton(e.Item, "DeleteButton", "Usuń");
                        //SetControlVisible(e.Item, "DeleteButton", false);
                        MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        break;
                    case limEdit:
                        Button bt = SetButton(e.Item, "UpdateButton", "Zapisz");
                        if (bt != null)
                            bt.ValidationGroup = "vge";
                        SetButton(e.Item, "CancelButton", "Anuluj");
                        SetButton(e.Item, "DeleteButton", "Usuń");
                        MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        break;
                    case limInsert:
                        bt = SetButton(e.Item, "InsertButton", "Dodaj");
                        if (bt != null)
                            bt.ValidationGroup = "vgi";
                        SetButton(e.Item, "CancelButton", "Czyść");
                        SetControlVisible(e.Item, "CancelButton", false);
                        break;
                }
            }
        }
        //------
        protected static void lvDic_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            Tools.ListViewInitItem((ListView)sender, e, true);
        }

        protected static void lvDic_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Tools.ListViewInitItem((ListView)sender, e, false);
        }

        protected static void lvDic_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
            ((ListView)sender).EditIndex = -1;
        }

        protected static void lvDic_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
        }

        protected static void lvDic_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
        }
        //------
        public static void PrepareDicListView(ListView lvDic)
        {
            lvDic.ItemCreated += new EventHandler<ListViewItemEventArgs>(lvDic_ItemCreated);
            lvDic.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvDic_ItemDataBound);
            lvDic.ItemInserting += new EventHandler<ListViewInsertEventArgs>(lvDic_ItemInserting);
            lvDic.ItemUpdating += new EventHandler<ListViewUpdateEventArgs>(lvDic_ItemUpdating);
            lvDic.ItemDeleting += new EventHandler<ListViewDeleteEventArgs>(lvDic_ItemDeleting);
        }
         */


        //-----------------------------------------------------------------------------------------------
        const string InsertButton = "InsertButton";   // NewRecord i Insert
        const string UpdateButton = "UpdateButton";
        const string CancelButton = "CancelButton";
        const string EditButton = "EditButton";
        const string DeleteButton = "DeleteButton";


        //--- z RPP -------------------------------------------------------------------------------------
        public static void ListViewInitItem(ListView lv, ListViewItemEventArgs e, bool create, bool editDelete)
        {
            Button bt;
            if (create)
            {
                bool select, edit, insert;
                int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
                switch (lim)
                {
#if !MODAL
                    case limSelect:
                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);
                        SetButton(e.Item, EditButton, "Edytuj");
                        if (editDelete)
                            SetControlVisible(e.Item, DeleteButton, false);
                        else SetButton(e.Item, DeleteButton, "Usuń");
                        break;
                    case limEdit:
                        bt = SetButton(e.Item, UpdateButton, "Zapisz");
                        if (bt != null && String.IsNullOrEmpty(bt.ValidationGroup)) bt.ValidationGroup = "evg";
                        SetButton(e.Item, CancelButton, "Anuluj");
                        SetButton(e.Item, DeleteButton, "Usuń");
                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);
                        Tools.SetControlVisible(lv, InsertButton, false);
                        break;
#endif
                    case limInsert:
                        bt = (Button)Tools.SetControlVisible(lv, InsertButton, false);
                        if (bt != null)
                            bt = SetButton(e.Item, InsertButton, "Zapisz");
                        else
                            bt = SetButton(e.Item, InsertButton, "Dodaj");
                        if (bt != null && String.IsNullOrEmpty(bt.ValidationGroup)) bt.ValidationGroup = "ivg";

                        //SetButton(e.Item, "CancelButton", "Czyść");
                        //SetControlVisible(e.Item, CancelButton, false);
                        bt = e.Item.FindControl(CancelButton) as Button;
                        if (bt != null)
                            if (bt.CommandName == "Cancel")     // "Czyść"
                                bt.Visible = false;
                            else
                                bt.Text = "Anuluj";
                        break;
                }
            }
            else
            {
#if MODAL
                bool select, edit, insert;
                int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
                switch (lim)
                {
                    case limSelect:
                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);
                        SetButton(e.Item, EditButton, "Edytuj", null);
                        if (editDelete)
                            SetControlVisible(e.Item, DeleteButton, false);
                        else SetButton(e.Item, DeleteButton, "Usuń", null);
                        break;
                    case limEdit:
                        //bt = SetButton(e.Item, UpdateButton, "Zapisz", null);
                        //if (bt != null && String.IsNullOrEmpty(bt.ValidationGroup)) bt.ValidationGroup = "evg";
                        SetButton(e.Item, UpdateButton, "Zapisz", null);
                        SetButton(e.Item, CancelButton, "Anuluj", null);
                        SetButton(e.Item, DeleteButton, "Usuń", null);
                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);

                        Tools.SetControlVisible(lv, InsertButton, false);
                        break;
                }
#endif
            }
        }

        public static void ListViewInitItem_2(ListView lv, ListViewItemEventArgs e, bool create, bool editDelete)
        {
            if (create)
            {
                Button bt;
                bool select, edit, insert;
                int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
                switch (lim)
                {
#if !MODAL
                    case limSelect:
                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);
                        SetButton(e.Item, EditButton, "Edytuj", null);
                        if (editDelete)
                            SetControlVisible(e.Item, DeleteButton, false);
                        else SetButton(e.Item, DeleteButton, "Usuń", null);
                        break;
                    case limEdit:
                        SetButton(e.Item, UpdateButton, "Zapisz", "evg");
                        SetButton(e.Item, CancelButton, "Anuluj", null);
                        SetButton(e.Item, DeleteButton, "Usuń", null);
                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);
                        Tools.SetControlVisible(lv, InsertButton, false);
                        break;
#endif
                    case limInsert:
                        Control cbt = Tools.SetControlVisible(lv, InsertButton, false);
                        if (cbt != null)
                            SetButton(e.Item, InsertButton, "Zapisz", "ivg");
                        else
                            SetButton(e.Item, InsertButton, "Dodaj", "ivg");

                        //SetButton(e.Item, "CancelButton", "Czyść");
                        //SetControlVisible(e.Item, CancelButton, false);
                        cbt = e.Item.FindControl(CancelButton);
                        if (cbt != null)
                        {
                            if (cbt is Button)
                            {
                                Button b = cbt as Button;
                                if (b.CommandName == "Cancel")     // "Czyść"
                                    b.Visible = false;
                                else
                                    b.Text = "Anuluj";
                            }
                            else if (cbt is LinkButton)
                            {
                                LinkButton b = cbt as LinkButton;
                                if (b.CommandName == "Cancel")     // "Czyść"
                                    b.Visible = false;
                                else
                                    b.Text = "Anuluj";
                            }
                            else if (cbt is ImageButton)
                            {
                                ImageButton b = cbt as ImageButton;
                                if (b.CommandName == "Cancel")     // "Czyść"
                                    b.Visible = false;
                            }
                        }
                        break;
                }
            }
            else
            {
#if MODAL
                bool select, edit, insert;
                int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
                switch (lim)
                {
                    case limSelect:

                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);

                        SetButton(e.Item, EditButton, "Edytuj", null);
                        if (editDelete)
                            SetControlVisible(e.Item, DeleteButton, false);
                        else SetButton(e.Item, DeleteButton, "Usuń", null);
                        break;
                    case limEdit:
                        SetButton(e.Item, UpdateButton, "Zapisz", "evg");
                        SetButton(e.Item, CancelButton, "Anuluj", null);
                        SetButton(e.Item, DeleteButton, "Usuń", null);
                        MakeConfirmDeleteRecordButton(e.Item, DeleteButton);
                        Tools.SetControlVisible(lv, InsertButton, false);
                        break;
                }
#endif
            }
        }
        //----- paginator i ilość rekordów -----
        public static DataPager Pager(ListView lv)
        {
            return (DataPager)lv.FindControl("DataPager1");
        }

        public static Controls.LetterDataPager LetterPager(ListView lv)
        {
            return lv.FindControl("LetterDataPager1") as Controls.LetterDataPager;
        }

        public static SqlDataSource GetSqlDataSource(ListView lv)
        {
            if (lv.DataSource != null)
                return lv.DataSource as SqlDataSource;
            else
                return lv.NamingContainer.FindControl(lv.DataSourceID) as SqlDataSource;
        }

        public static void UpdateCount(ListView lv)
        {
            DataPager dp = Pager(lv);
            if (dp != null)
                Tools.SetText(lv, "lbCount", dp.TotalRowCount.ToString());
        }
        //---------
        public static void ResetLetterPager(ListView lv)
        {
            Controls.LetterDataPager ldp = LetterPager(lv);
            if (ldp != null) ldp.Reset();
        }
        
        public static void UpdateLetterPager(ListView lv)
        {
            Controls.LetterDataPager ldp = LetterPager(lv);
            if (ldp != null) ldp.Update(lv, true);
        }
        //----------
        public static void SetPageSize(DropDownList ddlLines, DataPager dp, bool databind)  // wywoływana na zmianę w ddl wyboru - musi przybindować
        {
            if (ddlLines.SelectedValue == "all")
            {
                int size = dp.TotalRowCount;
                if (size == 0) size = Int32.MaxValue;
                dp.SetPageProperties(0, size, databind);
                //dp.SetPageProperties(0, dp.TotalRowCount, true);
                //dp.SetPageProperties(0, dp.MaximumRows, true);
            }
            else
            {
                int size = Tools.StrToInt(ddlLines.SelectedValue, 10);
                if (size == 0) size = 10;
                dp.SetPageProperties((dp.StartRowIndex / size) * size, size, databind);
            }
        }

        public static void SetPageSize(DropDownList ddlLines, bool databind)  // wywoływana na zmianę w ddl wyboru - musi przybindować
        {
            //DropDownList ddl = (DropDownList)lv.FindControl("ddlLines");
            ListView lv = ddlLines.NamingContainer as ListView;
            DataPager dp = Pager(lv);
            if (lv != null && lv is ListView && dp != null && ddlLines != null)
            {
                SetPageSize(ddlLines, dp, databind);
            }
        }

        public static void SetPageSize(ListView lv, bool databind)  // wywoływać po databind lub zamiast
        {
            DropDownList ddlLines = lv.FindControl("ddlLines") as DropDownList;
            if (ddlLines != null)
            {
                DataPager dp = Pager(lv);
                if (lv != null && lv is ListView && dp != null && ddlLines != null)
                {
                    SetPageSize(ddlLines, dp, databind);
                }
            }
        }
        //----------
        private static void SetPageSize(DropDownList ddlLines)  // wywoływana na zmianę w ddl wyboru - musi przybindować
        {
            //DropDownList ddl = (DropDownList)lv.FindControl("ddlLines");
            ListView lv = ddlLines.NamingContainer as ListView;
            DataPager dp = Pager(lv);
            if (lv != null && lv is ListView && dp != null && ddlLines != null)
            {
                if (ddlLines.SelectedValue == "all")
                    dp.SetPageProperties(0, dp.TotalRowCount, true);
                    //dp.SetPageProperties(0, dp.MaximumRows, true);
                else
                {
                    int size = Tools.StrToInt(ddlLines.SelectedValue, 10);
                    if (size == 0) size = 10;
                    dp.SetPageProperties((dp.StartRowIndex / size) * size, size, true);
                }
            }
        }

        protected static void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ViewState["pagesize"] = ((DropDownList)sender).SelectedValue;  // jak Brak danych to nie ustawia i trzeba samemu
            SetPageSize(sender as DropDownList);
        }
        //----- sortowanie ------
        public const string sortCol = "_scol";
        public const string sortDef = "_sdef";
        public const string sortMin = "_smin";
        public const string sortMax = "_smax";

        protected static void lvSorting_LayoutCreated(object sender, EventArgs e)
        {
            //foreach (Control c in (sender as ListView).Controls) -- ustawić Sort
            int sort = GetSortColumn(sender as ListView, sortCol);
            Report.ShowSort(sender as ListView, sort, sort > 0);
        }

        protected static void lvSorting_Sorting(object sender, ListViewSortEventArgs e)
        {
            int sort;
            Report.ShowSort(sender, e, GetSortColumn(sender as ListView, sortMax), GetSortColumn(sender as ListView, sortDef), out sort);
            SetSortColumn(sender as ListView, sortCol, sort);
        }

        public static int GetSortColumn(ListView lv, string id)
        {
            return GetViewStateInt(HttpContext.Current.Session[lv.ClientID + id], 0);
        }

        public static void SetSortColumn(ListView lv, string id, int col)
        {
            HttpContext.Current.Session[lv.ClientID + id] = col;
        }

        //----- Rights ---------------------------------------------------
        private static object[,] FRights = null;
        private static int rightsCount = 0;     // ilosc dostępna w konfiguracji
        private static int maxRight = 0;        // index ostatniego prawa 
        private static int extraRights = 0;     // dodatkowe kolumny (do colspan)

        // muszą mieć kolejne LinkButton1..max
        public static void prepareRightTh(ListView lv, int idx)    // 0..last, LinkButton 1..last+1, sql.substring(1..
        {
            string c = (idx + 1).ToString();
            Tools.SetControlVisible(lv, "thR" + c, true);
            LinkButton lbt = (LinkButton)lv.FindControl("LinkButton" + c);
            if (lbt != null)
            {
                lbt.Visible = true;

                int right = (int)FRights[idx, 0];
                string name = FRights[idx, 1].ToString();
                if (String.IsNullOrEmpty(lbt.Text))
                    lbt.Text = name.Substring(0, 2).Trim();     // nn - nnnnnn

                if (String.IsNullOrEmpty(lbt.ToolTip))
                    lbt.ToolTip = name;

                if (String.IsNullOrEmpty(lbt.CommandName))
                    lbt.CommandName = "Sort";
                if (String.IsNullOrEmpty(lbt.CommandArgument))
                    lbt.CommandArgument = String.Format("[{0}]", c);
            }
        }

        public static bool GetSymbolName(string str, out string symbol, out string name)
        {
            int p = str.IndexOf('-');
            if (p != -1)
            {
                symbol = str.Substring(0, p).Trim();
                name = str.Substring(p + 1).Trim();
                return true;
            }
            else
            {
                symbol = null;
                name = str;
                return false;
            }
        }

        public static CheckBox prepareRightTd(ListViewItem item, bool setText, int idx, char[] dbRights, bool enabled)    // td
        {
            string c = (idx + 1).ToString();          // dla select
            Control td = Tools.SetControlVisible(item, "tdR" + c, true);

            bool r = false;                     // dla select i edit/insert
            int right = (int)FRights[idx, 0];
            string name = FRights[idx, 1].ToString();
            CheckBox cb = (CheckBox)item.FindControl("cbR" + c);
            if (cb != null)
            {
                if (0 <= right && right < dbRights.Length)
                    r = dbRights[right] == AppUser.chHasRight;
                cb.Checked = r;
                if (setText)
                {
                    string s, n;
                    if (GetSymbolName(name, out s, out n))
                        cb.Text = String.Format("<span>{0}</span> - {1}", s, n);
                    else
                        cb.Text = name;   // lub n - na jedno wyjdzie
                    /*
                    string[] nn = name.Split('-');
                    if (nn.Length > 1)
                        cb.Text = String.Format("<span>{0}</span> - {1}", nn[0], nn[1]);
                    else
                        cb.Text = name;
                     */
                }
                else
                    cb.ToolTip = name;

                //cb.Enabled = liMode != Tools.limSelect && canSetRights;
                cb.Enabled = enabled;
                cb.Visible = true;
            }

            if (td == null)
                Tools.SetControlVisible(item, "br" + c, true);

            return cb;
        }
        //-----
        public static void applyRight(ListViewItem item, int idx, ref char[] dbRights)
        {
            string c = (idx + 1).ToString();
            int right = (int)FRights[idx, 0];
            CheckBox cb = (CheckBox)item.FindControl("cbR" + c);
            if (cb != null)
                if (0 <= right && right < dbRights.Length)
                    dbRights[right] = cb.Checked ? '1' : '0';
        }

        public static string applyRights(ListViewItem item)
        {
            char[] ra = new char[maxRight + 1];
            for (int i = 0; i <= maxRight; i++)
                ra[i] = '0';

            for (int i = 0; i < rightsCount; i++)
                applyRight(item, i, ref ra);

            return new string(ra);
        }

        public static string applyRights(ListViewItem item, string rights0)
        {
            int len = rights0.Length;
            int rlen = maxRight + 1;
            if (len > rlen) rlen = len;
            char[] ra = new char[rlen];
            for (int i = 0; i < rlen; i++)
                ra[i] = i < len ? rights0[i] : '0';

            for (int i = 0; i < rightsCount; i++)
                applyRight(item, i, ref ra);

            return new string(ra);
        }
        /* 20170417 nie obcina rightsów22
        public static string applyRights(ListViewItem item, string rights0)
        {
            int len = rights0.Length;
            char[] ra = new char[maxRight + 1];
            for (int i = 0; i <= maxRight; i++)
                ra[i] = i < len ? rights0[i] : '0';

            for (int i = 0; i < rightsCount; i++)
                applyRight(item, i, ref ra);

            return new string(ra);
        }
        */ 
        //-----
        public static string RightsToSelectSql(string colRights)
        {
            string[] s = new string[rightsCount];
            for (int i = 0; i < rightsCount; i++)
                //s[i] = String.Format("substring({0}, {1}, 2) as [{2}],", colRights, (int)FRights[i, 0] + 1, i + 1);   // w sql substring zaczyna się od 1, pola do sortowania też od [1]
                s[i] = String.Format("substring({0}, {1}, 1) as [{2}],", colRights, (int)FRights[i, 0] + 1, i + 1);   // w sql substring zaczyna się od 1, pola do sortowania też od [1]
            return String.Join(null, s);
        }
        //------
        protected static void lvRights_LayoutCreated(object sender, EventArgs e)
        {
            PrepareRightsTh((ListView)sender);
        }

        public static void PrepareRights(ListView lv, object[,] rights, int maxR, int extraR)   // App.maxRight, lv = null jak nie ma ustawiać zdarzenia
        {
            FRights = rights;
            rightsCount = rights.Length / 2;                            // dwuwymiarowa tablica
            maxRight = maxR;
            extraRights = extraR;
            if (lv != null) lv.LayoutCreated += new EventHandler(lvRights_LayoutCreated);
            //lv.Sorting += new EventHandler<ListViewSortEventArgs>(lvSorting_Sorting);
        }

        public static void PrepareRightsTh(ListView lv)
        {
            HtmlTableCell th = (HtmlTableCell)Tools.SetControlVisible(lv, "thRights", true);
            if (th != null) th.ColSpan = rightsCount + extraRights;       // admin, mailing, raporty  <<<<<<< dać też jako parametr przekazany 
            for (int i = 0; i < rightsCount; i++)
                prepareRightTh(lv, i);
        }
        //----- ListView ------
        protected static void lvDic_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            ListViewInitItem((ListView)sender, e, true, false);
        }

        protected static void lvDic_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewInitItem((ListView)sender, e, false, false);
        }
        //------
        protected static void lvDic_ItemCreated_1(object sender, ListViewItemEventArgs e)
        {
            ListViewInitItem((ListView)sender, e, true, true);
        }

        protected static void lvDic_ItemDataBound_1(object sender, ListViewItemEventArgs e)
        {
            ListViewInitItem((ListView)sender, e, false, true);
        }
        //------
        protected static void lvDic_ItemCreated_2(object sender, ListViewItemEventArgs e)
        {
            ListViewInitItem_2((ListView)sender, e, true, false);
        }

        protected static void lvDic_ItemDataBound_2(object sender, ListViewItemEventArgs e)
        {
            ListViewInitItem_2((ListView)sender, e, false, false);
        }
        //------
        public static void UpdateDdlValues(Control item, IOrderedDictionary values)   // item przekazać ListViewItem
        {
            if (item != null)   // moze sie zdarzyć UpdateItem wywoływana na zwykłym Item, a nie Edit
                foreach (Control cnt in item.Controls)
                    if (cnt is DropDownList)
                    {
                        string dvf = ((DropDownList)cnt).DataValueField;
                        if (!String.IsNullOrEmpty(dvf))
                        {
                            string sv = ((DropDownList)cnt).SelectedValue;
                            values[dvf] = sv.ToLower() == db.NULL ? null : sv;
                        }
                    }
                    else if (cnt is HtmlTableCell)   // td runat="server"
                        UpdateDdlValues(cnt, values);
        }

        protected static void lvDic_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            UpdateDdlValues(e.Item, e.Values);
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
            ((ListView)sender).EditIndex = -1;
        }

        protected static void lvDic_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateDdlValues(((ListView)sender).EditItem, e.NewValues);
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
        }

        protected static void lvDic_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
        }
        //------ no log -----
        protected static void lvDic_ItemInserting_noLog(object sender, ListViewInsertEventArgs e)
        {
            UpdateDdlValues(e.Item, e.Values);
            ((ListView)sender).EditIndex = -1;
        }

        protected static void lvDic_ItemUpdating_noLog(object sender, ListViewUpdateEventArgs e)
        {
            UpdateDdlValues(((ListView)sender).EditItem, e.NewValues);
        }
        //------        
        protected static void lvDic_LayoutCreated(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv != null)
            {
                HtmlTable tb = lv.FindControl("ctl01") as HtmlTable;   // jak nie znajduje to LayoutTemplate był przeniesiony - dodał nazwe Table1
                if (tb != null && tb is HtmlTable)
                {
                    AddClass(tb, "ListView1 tbBrowser hoverline " + lv.Parent.ID);
                    Label lb = (Label)lv.FindControl("lbCount");
                    if (lb != null)
                    {
                        int cols = 1;
                        if (tb.Rows.Count > 1)
                            cols = tb.Rows[1].Cells.Count;
                            //cols = tb.Rows[1].Cells.Cast<HtmlTableCell>().Sum(a => a.ColSpan); // Cast<HtmlTableCell> poniewaz HtmlTableRow zawierac moze tylko HtmlTableCell
                        tb.Rows[0].Cells[0].ColSpan = cols;
                    }
                }
                DataPager dp = Pager(lv);
                DropDownList ddlLines = lv.FindControl("ddlLines") as DropDownList;
                if (dp != null && ddlLines != null)
                {
                    ddlLines.Items.Clear();
                    ddlLines.Items.Add(new ListItem(dp.PageSize.ToString()));
                    ddlLines.Items[0].Selected = true;
                    if (dp.PageSize <= 10) ddlLines.Items.Add(new ListItem("20"));
                    else if (dp.PageSize <= 15) ddlLines.Items.Add(new ListItem("25"));
                    ddlLines.Items.Add(new ListItem("50"));
                    ddlLines.Items.Add(new ListItem("100"));
                    ddlLines.Items.Add(new ListItem("WSZYSTKO", "all"));
                    ddlLines.SelectedIndexChanged += new EventHandler(ddlLines_SelectedIndexChanged);
                    ddlLines.AutoPostBack = true;
                    ddlLines.Attributes["OnChange"] = "javascript:showAjaxProgress();";
                }
                if (dp != null)
                {
                    /*
                    dp.Fields.Clear();
                    DataPagerFieldItem f1 = new DataPagerFieldItem();
                    dp.Fields.Add();
                    */
                }

                Button bt = (Button)Tools.SetControlVisible(lv, InsertButton, true);  //NewRecord
                if (bt != null)
                    if (bt.Text == "Insert") 
                        bt.Text = "Dodaj";
            }
            /*
            int sort = Sort;
            Report.ShowSort(lvPracownicy, sort, sort > 0);
            */
        }

        protected static void lvDic_DataBound(object sender, EventArgs e)
        {
            UpdateCount((ListView)sender);
            //UpdateLetterPager((ListView)sender);
        }

        private static void ShowInsertLine(object sender, bool visible)
        {
            ListView lv = (ListView)sender;
            if (visible)
            {
                lv.EditIndex = -1;      // alert ?
                lv.SelectedIndex = -1;  // event onChanged ?
                Tools.SetControlVisible(lv, InsertButton, false);
                lv.InsertItemPosition = InsertItemPosition.FirstItem;
            }
            else
            {
                Control bt = Tools.SetControlVisible(lv, InsertButton, true);
                if (bt != null) lv.InsertItemPosition = InsertItemPosition.None;    // jak nie ma to nic nie robię
            }
        }

        protected static void lvDic_ItemCommand(object sender, ListViewCommandEventArgs e)      // NewRecord
        {
            ListView lv = (ListView)sender;
            switch (e.CommandName)
            {
                case "NewRecord":
                    ShowInsertLine(sender, true);
                    break;
                case "Insert":  // <<<< dodać odświeżenie LetterPagera !!!
                    break;
                case "CancelInsert":
                    ShowInsertLine(sender, false);
                    break;
                case "Edit":
                    ShowInsertLine(sender, false);  // pytanie czy zapisać ?
                    break;
                case "Update":
                case "Cancel":
                    Tools.SetControlVisible(lv, InsertButton, true);
                    break;
            }
        }

        protected static void lvDic_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            ShowInsertLine(sender, false);  // pytanie
        }

        protected static void lvDic_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            ShowInsertLine(sender, false);
        }

        protected static void lvDic_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            //ShowInsertLine(sender, false);
        }

        //-------------------------------------------------------------------
        public static Control OnClick(ListViewItem item, string cntOnClick, string btToClick)
        {
            Control cnt = item.FindControl(cntOnClick) as Control;
            Control bt = item.FindControl(btToClick);
            if (cnt != null && bt != null)
            {
                string click = String.Format("javascript:doClick('{0}');", bt.ClientID);
                if (cnt is HtmlControl)
                    ((HtmlControl)cnt).Attributes["onclick"] = click;
                else if (cnt is WebControl)
                    ((WebControl)cnt).Attributes["onclick"] = click;
            }
            return bt;
        }

        public static Control OnClick(ListViewItem item, string cntOnClick, string jsfmt, params object[] par)
        {
            Control cnt = item.FindControl(cntOnClick) as Control;
            if (cnt != null)
            {
                if (cnt is HtmlControl)
                    ((HtmlControl)cnt).Attributes["onclick"] = String.Format(jsfmt, par);
                else if (cnt is WebControl)
                    ((WebControl)cnt).Attributes["onclick"] = String.Format(jsfmt, par);
            }
            return cnt;
        }

        //-------------------------------------------------------------------
        public static void PrepareDicListView(ListView lvDic, int mode)  // mode 0 - std ListView, 1 - Del z Edycji
        {
            PrepareDicListView(lvDic, mode, true, true, true);
        }

        public static void PrepareDicListView(ListView lvDic, int mode, bool logInsert, bool logUpdate, bool logDelete)  // mode 0 - std ListView, 1 - Del z Edycji
        {
            switch (mode)
            {
                case 2:   // ImageButtons ale jak 0
                    lvDic.ItemCreated += new EventHandler<ListViewItemEventArgs>(lvDic_ItemCreated_2);
                    lvDic.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvDic_ItemDataBound_2);
                    break;
                case 1:
                    lvDic.ItemCreated += new EventHandler<ListViewItemEventArgs>(lvDic_ItemCreated_1);
                    lvDic.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvDic_ItemDataBound_1);
                    break;
                default:
                    lvDic.ItemCreated += new EventHandler<ListViewItemEventArgs>(lvDic_ItemCreated);
                    lvDic.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvDic_ItemDataBound);
                    break;
            }


            if (mode == 1337)
                lvDic.LayoutCreated += new EventHandler(lvDicBS_LayoutCreated);
            else
                lvDic.LayoutCreated += new EventHandler(lvDic_LayoutCreated);
            
            
            lvDic.DataBound += new EventHandler(lvDic_DataBound);

            if (logInsert)
                lvDic.ItemInserting += new EventHandler<ListViewInsertEventArgs>(lvDic_ItemInserting);
            else
                lvDic.ItemInserting += new EventHandler<ListViewInsertEventArgs>(lvDic_ItemInserting_noLog);

            if (logUpdate)
                lvDic.ItemUpdating += new EventHandler<ListViewUpdateEventArgs>(lvDic_ItemUpdating);
            else
                lvDic.ItemUpdating += new EventHandler<ListViewUpdateEventArgs>(lvDic_ItemUpdating_noLog);

            if (logDelete)
                lvDic.ItemDeleting += new EventHandler<ListViewDeleteEventArgs>(lvDic_ItemDeleting);

            lvDic.ItemCommand += new EventHandler<ListViewCommandEventArgs>(lvDic_ItemCommand);
            lvDic.ItemDeleted += new EventHandler<ListViewDeletedEventArgs>(lvDic_ItemDeleted);
            lvDic.ItemInserted += new EventHandler<ListViewInsertedEventArgs>(lvDic_ItemInserted);
            lvDic.ItemUpdated += new EventHandler<ListViewUpdatedEventArgs>(lvDic_ItemUpdated);
        }

        //------
        public static void PrepareSorting(ListView lv, int defSortColumn, int maxSortColumns)
        {
            SetSortColumn(lv, sortDef, defSortColumn);
            SetSortColumn(lv, sortCol, defSortColumn);
            SetSortColumn(lv, sortMax, maxSortColumns);
            lv.LayoutCreated += new EventHandler(lvSorting_LayoutCreated);
            lv.Sorting += new EventHandler<ListViewSortEventArgs>(lvSorting_Sorting);
        }

        // Badnia Wstępne
        public static void PrepareSorting2(ListView lv, int defSortColumn, int maxSortColumns)
        {
            if (!(HttpContext.Current.Handler as Page).IsPostBack)
            {
                SetSortColumn(lv, sortDef, defSortColumn);
                SetSortColumn(lv, sortCol, defSortColumn);
                SetSortColumn(lv, sortMax, maxSortColumns);
            }
            lv.LayoutCreated += new EventHandler(lvSorting_LayoutCreated);
            lv.Sorting += new EventHandler<ListViewSortEventArgs>(lvSorting_Sorting);
        }
    
    
    

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /* old
        public static void ListViewInitItem(ListView lv, ListViewItemEventArgs e, bool create, bool editDelete)
        {
            if (create)
            {
                bool select, edit, insert;
                int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
                switch (lim)
                {
                    case limSelect:
                        MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        SetButton(e.Item, "EditButton", "Edytuj");
                        if (editDelete)
                            SetControlVisible(e.Item, "DeleteButton", false);
                        else SetButton(e.Item, "DeleteButton", "Usuń");
                        break;
                    case limEdit:
                        Button bt = SetButton(e.Item, "UpdateButton", "Zapisz");
                        if (bt != null) bt.ValidationGroup = "evg";
                        SetButton(e.Item, "CancelButton", "Anuluj");
                        SetButton(e.Item, "DeleteButton", "Usuń");
                        MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        break;
                    case limInsert:
                        bt = SetButton(e.Item, "InsertButton", "Dodaj");
                        if (bt != null) bt.ValidationGroup = "ivg";
                        //SetButton(e.Item, "CancelButton", "Czyść");
                        SetControlVisible(e.Item, "CancelButton", false);
                        break;
                }
            }
        }

        public static void UpdateCount(ListView lv)
        {
            DataPager dp = (DataPager)lv.FindControl("DataPager1");
            if (dp != null)
                Tools.SetText(lv, "lbCount", dp.TotalRowCount.ToString());
        }

        //------
        protected static void lvDic_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            Tools.ListViewInitItem((ListView)sender, e, true, false);
        }

        protected static void lvDic_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Tools.ListViewInitItem((ListView)sender, e, false, false);
        }
        //------
        protected static void lvDic_ItemCreated_1(object sender, ListViewItemEventArgs e)
        {
            Tools.ListViewInitItem((ListView)sender, e, true, true);
        }

        protected static void lvDic_ItemDataBound_1(object sender, ListViewItemEventArgs e)
        {
            Tools.ListViewInitItem((ListView)sender, e, false, true);
        }
        //------
        public static void UpdateDdlValues(ListViewItem item, IOrderedDictionary values)
        {
            foreach (Control cnt in item.Controls)
                if (cnt is DropDownList)
                {
                    string dvf = ((DropDownList)cnt).DataValueField;
                    if (!String.IsNullOrEmpty(dvf))
                        values[dvf] = ((DropDownList)cnt).SelectedValue;
                }
        }

        protected static void lvDic_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            UpdateDdlValues(e.Item, e.Values);
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
            ((ListView)sender).EditIndex = -1;
        }

        protected static void lvDic_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateDdlValues(((ListView)sender).EditItem, e.NewValues);
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
        }

        protected static void lvDic_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Log.LogChanges(Log.SLOWNIK, ((ListView)sender).ClientID, e);
        }
        //------        
        protected static void lvDic_LayoutCreated(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv != null)
            {
                HtmlTable tb = lv.FindControl("ctl01") as HtmlTable;
                if (tb != null)
                {
                    AddClass(tb, "ListView1 tbBrowser " + lv.Parent.ID);
                    Label lb = (Label)lv.FindControl("lbCount");
                    if (lb != null)
                    {
                        tb.Rows[0].Cells[0].ColSpan = 2;
                    }
                }
            }
        }

        protected static void lvDic_DataBound(object sender, EventArgs e)
        {
            UpdateCount((ListView)sender);
        }
        //------
        public static void PrepareDicListView(ListView lvDic, int mode)  // mode 0 - std ListView, 1 - Del z Edycji
        {
            switch (mode)
            {
                case 1:
                    lvDic.ItemCreated += new EventHandler<ListViewItemEventArgs>(lvDic_ItemCreated_1);
                    lvDic.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvDic_ItemDataBound_1);
                    break;
                default:
                    lvDic.ItemCreated += new EventHandler<ListViewItemEventArgs>(lvDic_ItemCreated);
                    lvDic.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvDic_ItemDataBound);
                    break;
            }
            lvDic.LayoutCreated += new EventHandler(lvDic_LayoutCreated);
            lvDic.DataBound += new EventHandler(lvDic_DataBound);

            lvDic.ItemInserting += new EventHandler<ListViewInsertEventArgs>(lvDic_ItemInserting);
            lvDic.ItemUpdating += new EventHandler<ListViewUpdateEventArgs>(lvDic_ItemUpdating);
            lvDic.ItemDeleting += new EventHandler<ListViewDeleteEventArgs>(lvDic_ItemDeleting);
        }
        */
        
        
        
        
        //---------------------------------------------------
        public static void RegisterPostBackControls(ListView lv, string lbtId)   // btId - LinkButton w Item
        {
            Page page = HttpContext.Current.Handler as Page;
            UpdatePanel up = FindUpdatePanel(lv);
            if (up != null)
                foreach (ListViewItem item in lv.Items)
                {
                    LinkButton lbt = item.FindControl(lbtId) as LinkButton;
                    if (lbt != null)
                    {
                        //ScriptManager.GetCurrent(Page).RegisterPostBackControl(lbt);

                        PostBackTrigger trigger = new PostBackTrigger();
                        trigger.ControlID = lbt.UniqueID;
                        up.Triggers.Add(trigger);
                        ScriptManager.GetCurrent(page).RegisterPostBackControl(lbt);
                    }
                }
        }

        public static void DownloadFile(string file, string fileName, string dataType)
        {
            bool ok = false;
            HttpResponse response = HttpContext.Current.Response;
            try
            {
                response.ClearContent();
                response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", fileName));  // numer hex
                //response.WriteFile(Server.MapPath(fileName));
                response.WriteFile(file);
                response.ContentType = dataType;
                ok = true;
            }
            catch (Exception ex)
            {
                //Tools.ShowErrorLog(Log._PORTAL, ex.Message, "Wystąpił błąd podczas pobierania pliku.");  nie moze byc bo response laduje do pliku
                AppError.Show("Pobieranie pliku", ex);
            }
            if (ok) response.End();   // to wywala exception ze thread aborted i to jest ok, wiec nie moge przechwycic
        }

        //------------------------------------------------------
        public static void SetMainServiceUrl()
        {
            Page page = HttpContext.Current.Handler as Page;
            Tools.ExecOnStart2("smsurl", String.Format("setServiceUrl('{0}');", page.ResolveUrl("~/" + App.MainSvc)));
        }

        //-----------------------------------------------------------
        public static bool GetDbVerInfo(out string appName, out string dbServer, out string dbName)  // true jak inna niż standardowa 
        {
            //appName = GetAppPath();
            appName = HttpContext.Current.Server.MapPath("~");
            string conStr = db.conStr;

            string[] a = appName.Split('/', '\\');
            appName = "???";
            for (int i = a.Length - 1; i > 0; i--)
                if (!String.IsNullOrEmpty(a[i]))
                {
                    appName = a[i];
                    break;
                }
            //-----
            dbServer = null;
            dbName = null;
            int p = conStr.ToLower().IndexOf("source");
            bool f = false;
            if (p >= 0)
            {
                dbServer = conStr.Remove(0, p + 6);
                p = dbServer.IndexOf("=");
                if (p >= 0)
                {
                    dbServer = dbServer.Remove(0, p + 1);
                    p = dbServer.IndexOf(";");
                    if (p >= 0)
                    {
                        dbServer = dbServer.Substring(0, p).Trim();
                        f = true;
                    }
                }
            }
            if (!f) dbServer = "???";
            //-----
            p = conStr.ToLower().IndexOf("catalog");
            f = false;
            if (p >= 0)
            {
                dbName = conStr.Remove(0, p + 7);
                p = dbName.IndexOf("=");
                if (p >= 0)
                {
                    dbName = dbName.Remove(0, p + 1);
                    p = dbName.IndexOf(";");
                    if (p >= 0)
                    {
                        dbName = dbName.Substring(0, p).Trim();
                        f = true;
                    }
                }
            }
            if (!f) dbName = "???";

            return true;
        }

        public static bool IsTestDb(string db, string app, out string text, out string tooltip)
        {
            //const string app = "rcp";
            //const string db = "hr_db";
            app = app.ToLower();
            db = db.ToLower();
            string dbName, dbServer, appName;
            Tools.GetDbVerInfo(out appName, out dbServer, out dbName);
            string sdbName;
            if (db.Contains("."))
                sdbName = dbServer + '.' + dbName;
            else
                sdbName = dbName;

            bool b1 = appName.ToLower() != app;
            bool b2 = sdbName.ToLower() != db;
            bool b3 = Mailing.NoMails || !App.IsMailing;
            if (b1 || b2 || b3)
            {
                string t = String.Format("{0}, {1}", appName, sdbName);
                appName = b1 ? "Testowa" : "Produkcyjna";
                dbName = b2 ? "Testowa" : "Produkcyjna";
                String CompileTime = String.Empty;

                String FilePath = System.Reflection.Assembly.GetCallingAssembly().Location;
                DateTime CreationTime = System.IO.File.GetCreationTime(FilePath);
                CompileTime = String.Format("&nbsp;&nbsp;&nbsp;Wersja: <b>{0}</b>", CreationTime);

                //string nomails = b3 ? "&nbsp;&nbsp;&nbsp;Mailing: <b>NIE</b>" : null;
                string nomails = String.Format("&nbsp;&nbsp;&nbsp;Mailing: <b>{0}</b>", b3 ? "NIE" : "TAK");
                text = String.Format("Aplikacja: <b>{0}</b>&nbsp;&nbsp;&nbsp;Baza danych: <b>{1}</b>{2}{3}", appName, dbName, nomails, CompileTime);
                tooltip = t;
                return true;
            }
            else
            {
                text = null;
                tooltip = null;
                return false;
            }
        }






        //-------------------------------------------------
        // wnioski zmiana danych osobowych

        public static void ShowDialogAuto(string divModalId, string parentUpdPanelId, string title, string width, string btCloseId) // używa jquery, może dac tu cały script ???
        {                                                                                       // UWAGA !!! update panel musi być Conditional żeby sortowania na listahc chodziły !!!
            Tools.ExecOnStart2(divModalId + "_script", String.Format("showDialog2('{0}','{1}','{2}','{3}','{4}');",
                divModalId, parentUpdPanelId, title, width, btCloseId));
        }

        public static void ShowDialogAuto(Control cnt, string divModalId, Button btClose, string title)  // umieść kontrolkę cnt w div modalnym i ustaw klawisz zamykający
        {
            if (btClose != null) MakeDialogCloseButton(btClose, divModalId);
            Control upa = FindUpdatePanel(cnt);
            if (upa == null) upa = cnt.Parent;  // nie powinno mieć miejsca
            //ShowDialog(divModalId, upa.ClientID, title);
            if (btClose != null)
                ShowDialogAuto(divModalId, upa.ClientID, title, "auto", btClose.ClientID);
            else
                ShowDialog(divModalId, upa.ClientID, title);
        }

        public static void ShowDialogConfirm(string divModalId, string parentUpdPanelId, string title, string width, string btCloseId, string btyId, string btnId, String message) // używa jquery, może dac tu cały script ???
        {                                                                                       // UWAGA !!! update panel musi być Conditional żeby sortowania na listahc chodziły !!!

            Tools.ExecOnStart2(divModalId + "_script", String.Format("showDialog2confirm('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');",
                divModalId, parentUpdPanelId, title, width, btCloseId, btyId, btnId, message));
        }
        //----
        public static void ShowDialogConfirm(Control cnt, string divModalId, int? width, Button btClose, string title, Button BTY, Button BTN, String Message)  // umieść kontrolkę cnt w div modalnym i ustaw klawisz zamykający
        {
            if (btClose != null) MakeDialogCloseButton(btClose, divModalId);
            Control upa = FindUpdatePanel(cnt);
            if (upa == null) upa = cnt.Parent;  // nie powinno mieć miejsca
            //ShowDialog(divModalId, upa.ClientID, title);
            if (btClose != null)
                ShowDialogConfirm(divModalId, upa.ClientID, title, "700", btClose.ClientID, BTY.ClientID, BTN.ClientID, Message);
            else
                ShowDialog(divModalId, upa.ClientID, title);
        }

        public static void DistableButtonDialog()
        {
            Tools.ExecOnStart2("distablebuttonDialog", string.Format("DistableButtonDialog()"));
        }

        //--------------------------------------------------
        //------ wydruki portal ------
        public static int GetPageNum(string path)
        {
            return int.Parse(Regex.Match(path, @".*_(.*)\.png").Groups.Cast<Group>().Last().Value);
        }

        public static string ChangeImgFormatToPNG(string path)
        {
            try
            {
                string newFile;
                using (Bitmap bm = new Bitmap(path))
                {
                    newFile = Path.ChangeExtension(path, "png");
                    if (File.Exists(newFile)) File.Delete(newFile);
                    bm.Save(newFile, ImageFormat.Png);
                }
                File.Delete(path);
                return newFile;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool IsAllNotNull(params object[] obj)
        {
            return !obj.Any(a => a == null);
        }

        //------------------------------------------------------
        //------ limity CC, debug -----
        public static string[] getHiddenFieldsValues(Control parent, params string[] names)
        {
            return names.Select(a => {
                    HiddenField hf = parent.FindControl(a) as HiddenField;
                    return hf == null ? string.Empty : hf.Value;
            }
                ).ToArray();
        }

        public static string getControlsTree(Control cnt, int tabsN)
        {
            string tabs = new string('\t', tabsN);
            StringBuilder sb = new StringBuilder(string.Format("{2}{0} {1}", cnt.GetType().Name, cnt.ID, tabs));
            foreach(var Item in cnt.Controls)
            {
                if(Item is Control)
                {
                    sb.AppendLine(getControlsTree((Control)Item, tabsN + 1));
                }
                else
                {
                    sb.AppendLine(tabs + "\t<object>");
                }
            }
            return sb.ToString();
        }
        //----- Badania Wstępne -----
        //public static string getStringFromNChar(string str, int n)
        //{
        //    if (n < 0)
        //        throw new ArgumentOutOfRangeException();
        //    if (n == 0)
        //        return "";
        //    return string.Concat(Enumerable.Repeat(str, n));
        //}

        public static string getStringFromNChar(string str, int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException();
            if (n == 0)
                return "";
            return string.Concat(Enumerable.Repeat(str, n).ToArray());
        }


        //public static string[] BadWstRightStrs = new string[] { "-", "pokaż", "edycja" };

        public static Bitmap ResizeBitmap(Bitmap bm, int width, int height)
        {
            if (width == 0)
            {
                width = (int)((double)bm.Width * height / bm.Height);
            }
            else if (height == 0)
            {
                height = (int)((double)bm.Height * width / bm.Width);
            }
            Rectangle rect = new Rectangle(0, 0, width, height);
            Bitmap newBm = new Bitmap(width, height);

            newBm.SetResolution(bm.HorizontalResolution, bm.VerticalResolution);
            using (var graphics = System.Drawing.Graphics.FromImage(newBm))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(bm, rect, 0, 0, bm.Width, bm.Height, System.Drawing.GraphicsUnit.Pixel, wrapMode);
                }
            }
            return newBm;
        }
        //--------------------------------------------------------
        // inna definicja funckji z klacy Ext - nie jako rozszerzenia
        public static object GetPrivateField(object obj, string name)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            FieldInfo field = type.GetField(name, flags);
            return field.GetValue(obj);
        }

        public static object GetPrivateProperty(object obj, string name)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            PropertyInfo field = type.GetProperty(name, flags);
            return field.GetValue(obj, null);
        }
        //----
        public static void SetPrivateField(object obj, string name, object value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            FieldInfo field = type.GetField(name, flags);
            field.SetValue(obj, value);
        }

        public static void SetPrivateProperty(object obj, string name, object value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            PropertyInfo field = type.GetProperty(name, flags);
            field.SetValue(obj, value, null);
        }

        public static T CallPrivateMethod<T>(object obj, string name, params object[] param)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(name, flags);
            return (T)method.Invoke(obj, param);
        }


        // ------------------------

        public static String PrepareFilename(String Param)
        {
            char[] chars = Param.ToCharArray();
            for (int i = 0; i < Param.Length; i++)
            {
                int p = @"/\:*?<>|".IndexOf(chars[i]);
                if (p > -1)
                    chars[i] = '_';
            }
            return new String(chars);
        }

        public static void ExecuteJavascript(String Script)
        {
            Page Page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), Guid.NewGuid().ToString(), Script, true);
        }

        /* bootstrap */

        protected static void lvDicBS_LayoutCreated(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv != null)
            {
                HtmlTable tb = lv.FindControl("ctl01") as HtmlTable;   // jak nie znajduje to LayoutTemplate był przeniesiony - dodał nazwe Table1
                if (tb != null && tb is HtmlTable)
                {
                    AddClass(tb, "ListView1337 table" + (String.IsNullOrEmpty(lv.Parent.ID) ? "1337" : lv.Parent.ID));

                    /* new */

                    HtmlTable tbInner = lv.FindControl("itemPlaceholderContainer") as HtmlTable;
                    if (tbInner != null && tbInner is HtmlTable)
                    {
                        AddClass(tbInner, "table");
                    }

                    Label lb = (Label)lv.FindControl("lbCount");
                    if (lb != null)
                    {
                        int cols = 1;
                        if (tb.Rows.Count > 1)
                            cols = tb.Rows[1].Cells.Count;
                        //cols = tb.Rows[1].Cells.Cast<HtmlTableCell>().Sum(a => a.ColSpan); // Cast<HtmlTableCell> poniewaz HtmlTableRow zawierac moze tylko HtmlTableCell
                        tb.Rows[0].Cells[0].ColSpan = cols;
                    }
                }
                DataPager dp = Pager(lv);
                DropDownList ddlLines = lv.FindControl("ddlLines") as DropDownList;
                if (dp != null && ddlLines != null)
                {
                    ddlLines.Items.Clear();
                    ddlLines.Items.Add(new ListItem(dp.PageSize.ToString()));
                    ddlLines.Items[0].Selected = true;
                    if (dp.PageSize <= 10) ddlLines.Items.Add(new ListItem("20"));
                    else if (dp.PageSize <= 15) ddlLines.Items.Add(new ListItem("25"));
                    ddlLines.Items.Add(new ListItem("50"));
                    ddlLines.Items.Add(new ListItem("100"));
                    ddlLines.Items.Add(new ListItem("WSZYSTKO", "all"));
                    ddlLines.SelectedIndexChanged += new EventHandler(ddlLines_SelectedIndexChanged);
                    ddlLines.AutoPostBack = true;
                    ddlLines.Attributes["OnChange"] = "javascript:showAjaxProgress();";
                }
                if (dp != null)
                {
                    /*
                    dp.Fields.Clear();
                    DataPagerFieldItem f1 = new DataPagerFieldItem();
                    dp.Fields.Add();
                    */
                }

                Button bt = (Button)Tools.SetControlVisible(lv, InsertButton, true);  //NewRecord
                if (bt != null)
                    if (bt.Text == "Insert")
                        bt.Text = "Dodaj";
            }
            /*
            int sort = Sort;
            Report.ShowSort(lvPracownicy, sort, sort > 0);
            */
        }


        public static void ShowBSDialog(string divId, string options)
        {
            ExecOnStart2("_showscript1337", String.Format("javascript:$('#{0}').modal({{{1}}});", divId, options));
        }

        public static void ShowBSDialog(string divId)
        {
            ShowBSDialog(divId, "");
        }

        public static void ShowBSDialog()
        {
            ShowBSDialog("myModal");
        }

        public static void CloseBSDialog()
        {
            ExecOnStart2("_closescript1337", "javascript:$('.modal.in').modal('hide');");
        }

        public static void GenerateDuration(DropDownList ddl, int mOffset, int lastHour, bool addChooseStr)
        {
            if (ddl == null || ddl.Items.Count != 0) return;
            for (int i = 0; i <= lastHour; i++)
                for (int j = 0; j < 60; j += mOffset)
                    ddl.Items.Add(new ListItem(i.ToString() + ":" + j.ToString().PadLeft(2, '0'), (i * 60 + j).ToString()));
            if (addChooseStr)
                //ddl.Items.Insert(0, new ListItem(L.p("Wybierz..."), string.Empty));
                AddChooseStr(ddl, false);
        }

        public static String GetParam(String[] ParamList, int index)
        {
            if (index < ParamList.Length)
                return ParamList[index];
            return null;
        }

        public static Boolean GetBoolean(String S, bool Def)
        {
            if (String.IsNullOrEmpty(S))
                return Def;
            return S == "1";
        }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }

        public static String GetShortDayName(DateTime day)
        {
            int i = (int)day.DayOfWeek;

            switch(i)
            {
                case 0: return "Ni";
                case 1: return "Pn";
                case 2: return "Wt";
                case 3: return "Śr";
                case 4: return "Cz";
                case 5: return "Pi";
                case 6: return "So";
            }
            return "???";
        }





































        //-----------------------------------------------------------------------
        //-----------------------------------------------------------------------
        /*
        public static void ResizeImage(string imageFile, string outputFile, double scaleFactor)
        {
            using (var srcImage = System.Drawing.Image.FromFile(imageFile))
            {
                var newWidth = (int)(srcImage.Width * scaleFactor);
                var newHeight = (int)(srcImage.Height * scaleFactor);
                using (var newImage = new Bitmap(newWidth, newHeight))
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                    newImage.Save(outputFile);
                }
            }
        }

        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(System.Drawing.Imaging.ImageFormat format)
        {
            return System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        public static void ResizeImage(string imageFile, string outputFile, int newWidth, int newHeight)
        {
            using (var srcImage = System.Drawing.Image.FromFile(imageFile))
            {
                using (var newImage = new Bitmap(newWidth, newHeight))
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));

                    //const byte quality = 100;
                    System.Drawing.Imaging.ImageCodecInfo imageCodecInfo = GetEncoderInfo(System.Drawing.Imaging.ImageFormat.Jpeg);

                    System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                    System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                    System.Drawing.Imaging.EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(encoder, 100L);
                    encoderParameters.Param[0] = encoderParameter;

                    newImage.Save(outputFile, imageCodecInfo, encoderParameters);
                    //newImage.Save(outputFile);


                    //System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

                    //System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    //System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                    //System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 0L);
                    //myEncoderParameters.Param[0] = myEncoderParameter;


                    //bmp1.Save(@"c:\TestPhotoQualityZero.jpg", jgpEncoder, myEncoderParameters);
                }
            }
        }


        //-------------------------------------------------------
        private static string AvatarPicturesBigFolder = null;

        public static string GetAvatarPath(string avatar)   // ścieżka do katalogu ze cache zdjęć - "~/images/photos/"
        {
            return HttpContext.Current.Server.MapPath(Consts.AvatarPicturesFolder + avatar);
        }

        public static string GetUsersPhotosPath(string avatar)   // absolutna ścieżka do katalogu ze zdjęciami "c:\inetpub\wwwroot\portalres\photos\"
        {
            if (AvatarPicturesBigFolder == null)
                AvatarPicturesBigFolder = Tools.GetStr(ConfigurationSettings.AppSettings["AvatarsBigPath"]);
            return App.AvatarPicturesBigFolder + avatar;
        }
        */
        /* spr w cache
         * spr w images/photos i zapis do cache jak nie ma
         * generowanie z cache lub avatar domyslny
         */

        //if (!File.Exists(fn))
        //{
        //    string bn = //App.GetUsersPhotosPath(avatar);
        //    if (!File.Exists(bn))
        //    {
        //        //----- brak avatara -----
        //        if (def)
        //            fn = App.GetAvatarPath(SchemaLayoutManager.Consts.DefaultAvatar);
        //        else
        //            img = false;
        //    }
        //    else
        //    {
        //        //----- generowanie miniatury -----
        //        //using (Impersonator impersonator = new Impersonator())
        //        //{
        //        //    try
        //        //    {
        //        //        Tools.ResizeImage(bn, fn, 100, 128);
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        img = false;
        //        //        Log.Error(Log.ERROR, "Bład podczas tworzenia miniatury avatara:\n" + bn + "\n" + fn, ex.Message);
        //        //    }
        //        //}
        //    }
        //}


        public const string AvatarPath = "~/uploads/Avatars/";
        public const string AvatarExtension = ".jpg";

        public static String GetAvatarFilename(String Avatar)
        {
            return AvatarPath + Avatar;
        }

        public static string GetAvatarImage(string avatar, bool def)
        {
            string fn = HttpContext.Current.Server.MapPath(GetAvatarFilename(avatar));
            bool img = true;

            if (!File.Exists(fn))
            {
                fn = HttpContext.Current.Server.MapPath(GetAvatarFilename("default" + AvatarExtension));
            }
            
            if (img)
            {
                byte[] bytes = (byte[])File.ReadAllBytes(fn);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return "data:image/jpg;base64," + base64String;
            }
            else
                return null;
        }

        public static string GetUserAvatar(string kadryId)
        {
            return Tools.GetAvatarImage(kadryId + AvatarExtension, true);
        }

        public static string GetUserAvatar(AppUser user)
        {
            return GetUserAvatar(user.NR_EW);
        }


        public static View SetViewById(MultiView multiViewToSelect, String id)
        {
            View viewToSelect = multiViewToSelect.FindControl(id) as View;
            multiViewToSelect.SetActiveView(viewToSelect);
            return viewToSelect;
        }

        public static String GetCommandArgument(object sender)
        {
            Button btn = sender as Button;
            if (btn != null)
                return btn.CommandArgument;

            LinkButton lnk = sender as LinkButton;
            if (lnk != null)
                return lnk.CommandArgument;

            ImageButton img = sender as ImageButton;
            if (img != null)
                return img.CommandArgument;

            return null;

        }
    }
}




