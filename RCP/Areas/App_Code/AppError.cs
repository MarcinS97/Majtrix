using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.App_Code
{
    public class AppError
    {
        public static string sesErrInfo = "errInfo";
        public static string sesErrInfoEx = "errInfoEx";
#if RCP
        public static string errForm = "ErrorForm3.aspx";
#elif PORTAL
        public static string errForm = "Portal/ErrorForm.aspx";
#else   
        public static string errForm = "ErrorForm.aspx";
#endif
        public const int btDefault = 0;     // ok -> Default.aspx
        public const int btClose = 1;       
        public const int btBack = 2;
        public const int btNone = 3;
        //public const int btDefault2 = 4;
        
        public static void Show(string info, string info2, string infoEx, string par, int bt)  // btClose - klawisz zamknij widoczny zamiast powrót, dla błędów pokazywanych z formatek, które są modalne (np PrintAnkietaForm)
        {
            Info = info;
            InfoEx = info2;
            Log.Error(Log.t2APP, Tools.RemoveHtmlTags(Info), Tools.RemoveHtmlTags(info2) + infoEx, par);
            string p = bt == btDefault ? "" : "?c=" + bt.ToString();
            HttpContext.Current.Response.Redirect("~/" + errForm + p);
        }

        public static int Button
        {
            get 
            {
                int b = btDefault; 
                string par = HttpContext.Current.Request.QueryString["c"];
                if (!String.IsNullOrEmpty(par))
                    Int32.TryParse(par, out b);
                return b;
            }
        }

        //----------------------------
        public static void Show(string info, Exception ex)
        {
            Show(info, ex.Message, null, null, btDefault);
        }

        public static void Show(string info, string info2)
        {
            Show(info, info2, null, null, btDefault);
        }

        public static void Show(string info)
        {
            Show(info, btDefault);
        }

        public static void Show(string info, string info2, int bt)
        {
            Show(info, info2, null, null, bt);
        }

        public static void Show(string info, int bt)
        {
            Exception ex = HttpContext.Current.Server.GetLastError();
            string info2;
            string info3;
            if (ex != null)
            {
                info2 = ex.Message;
                info3 = "\r\n\r\n" + ex.StackTrace.ToString().Replace("\r\n   w ", "\r\n");  // formatowanie
            }
            else
            {
                info2 = "";
                info3 = "";
            }
            HttpContext.Current.Server.ClearError();
            Show(info, info2, info3, null, bt);
        }

        //------------------------------------------------
        public static string Info
        {
            get { return (string)HttpContext.Current.Session[sesErrInfo]; }
            set { HttpContext.Current.Session[sesErrInfo] = value; }
        }

        public static string InfoEx
        {
            get { return (string)HttpContext.Current.Session[sesErrInfoEx]; }
            set { HttpContext.Current.Session[sesErrInfoEx] = value; }
        }
    }
}
