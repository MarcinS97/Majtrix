using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class RPracownicyCC: System.Web.UI.Page
    {
        const string title = "Raporty PM - Zoom Pracownicy";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
                bool PM = App.User.HasRight(AppUser.rPodzialLudziPM);
                if (adm || PM)
                {
                    bool all = false;
                    //if (App.User.HasRight(AppUser.rRaportyAll))
                    //    all = Tools.StrToInt(Tools.GetStr(Request.QueryString["a"]), -1) == 1;   // przy okazji kontrola parametru
                    /*
                    hidAll.Value = all ? "1" : "0";
                    string userId = App.User.Id;
                    hidUserId.Value = userId;
                    paKier.Visible = adm;

                    if (adm)
                    {
                        ddlPM.DataBind();
                        Tools.SelectItem(ddlPM, userId);
                    }
                    string kid = adm ? ddlPM.SelectedValue : App.User.Id;
                    hidKierId.Value = string.IsNullOrEmpty(kid) ? "-99" : kid;

                    DateTime eom = Tools.eom(DateTime.Today);
                    deOd.Date = Tools.bom(DateTime.Today);
                    deDo.Date = DateTime.Today > eom ? eom : DateTime.Today;
                    */
                    Tools.SetNoCache();
                }
                else App.ShowNoAccess(title, null);
            }
            
            //ReportMaster rm = Page.Master as ReportMaster;
            //rm.SetBtExcel(btExcel_Click);
        }
    }
}
