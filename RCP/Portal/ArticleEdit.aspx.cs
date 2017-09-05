using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class ArticleEdit : System.Web.UI.Page
    {
        const string FormName = "Edycja artykułu";
        const string key = "5714259853115";
        const string salt = "9564111231213268487891561651656";   // na razie, potem dac losowe i zapisywane w sesji

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.IsPortalAdmin || App.User.HasRight(AppUser.rPortalArticles))
                {
                    Tools.SetNoCache();

                    string p = Tools.GetStr(Request.QueryString["p"]);
                    p = Report.DecryptQueryString(p, key, salt);                           
                    string grupa, aId;
                    Tools.GetLineParams(p, out grupa, out aId);
                    
                    if (!String.IsNullOrEmpty(grupa) && Tools.StrToInt(aId, -1) >= 0)
                    {
                        if (aId == "0")
                            cntArticleEdit.Prepare(grupa);
                        else 
                            cntArticleEdit.Prepare(grupa, aId);
                    }
                    else AppError.Show(FormName, "Niepoprawne parametry wywołania.");
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }
        //---------------------------------------------
        const string ArticleEditForm = "~/Portal/ArticleEdit.aspx?p="; 

        public static void Create(string grupa)
        {
            string p = Tools.SetLineParams(3, grupa, "0", "", null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, key, salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            HttpContext.Current.Response.Redirect(ArticleEditForm + e);
        }

        public static void Edit(string grupa, string aId)
        {
            string p = Tools.SetLineParams(3, grupa, aId, "", null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, key, salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            HttpContext.Current.Response.Redirect(ArticleEditForm + e);
        }
    }
}
