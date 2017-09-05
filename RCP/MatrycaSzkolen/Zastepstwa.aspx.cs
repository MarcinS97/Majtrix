using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen
{
    public partial class Zastepstwa : System.Web.UI.Page
    {
        const string FormName = "Zastępstwa";

        protected void Page_PreInit(object sender, EventArgs e)
        {
#if RCP
            this.MasterPageFile = App.MasterPageRCP;
#endif
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasAccess)
                {
                    if (HasFormAccess)
                    {
                        if (!HandleParams())
                        {
                            Tools.SetNoCache();
                            Info.SetHelp(FormName);
                        }
                    }
                    else
                        App.ShowNoAccess(FormName, App.User);
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        private bool HandleParams()
        {
            string p = Request.QueryString["p"];
            if (!String.IsNullOrEmpty(p))
            {
                int id = Tools.StrToInt(p, -1);
                if (id != -1)
                {
                    DataRow dr = db.selectRow("select * from Zastepstwa Z where Z.IdZastepujacy = {0} and Z.IdZastepowany = {1} and dbo.getdate(GETDATE()) between Z.Od and ISNULL(Z.Do,'20990909')", App.User.Id, id);
                    if (dr != null)
                    {
                        App.User.LoginAsUserId2(id.ToString());
                        App.Redirect(App.DefaultForm);
                    }
                    else
                    {
                        string prac = db.selectScalar("select Nazwisko + ' ' + Imie from Pracownicy where Id = {0}", id);
                        AppError.Show(String.Format("Brak ustanowionego zastepstwa dla użytkownika {0}", prac), null, AppError.btDefault);
                    }
                }
                App.ShowBadRepParameters("Zastępstwa");
            }
            return false;
        }

        public static bool HasFormAccess
        {
            get { return true; }
        }
    }
}