using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntRejestracjaRCP : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            { 
                Uri r = HttpContext.Current.Request.UrlReferrer;
                string s = "/Portal/PracLogin.aspx"; 
                if(r == null || r.AbsolutePath == s)
                {
                    Rejestracja();
                }            
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Rejestracja();
        }

        private void Rejestracja()
        {
            //var id = App._User.Id;
            var id = App.User.Id;
            DateTime d = DateTime.Today;
            string dzis = Tools.DateToStrDb(d);   // uwaga co ze zmiana 3 jak pracownik kliknie po północy ????
            string s = db.Select.Scalar(dsRejestruj.InsertCommand, id, dzis);
            if (!String.IsNullOrEmpty(s))
            {
                Label1.Text = "Dziękujemy!";
                Label2.Text = String.Format("Obecność w dniu {0} została już potwierdzona w systemie RCP.", Tools.DateToStr(d));
            }else
            {
                db.Execute(dsRejestruj.SelectCommand, id, dzis);
                Label1.Text = "Dziękujemy!";
                Label2.Text = String.Format("Obecność w dniu {0} została potwierdzona w systemie RCP.", Tools.DateToStr(d));
            }
            Button1.Visible = false;
        }
    }
}