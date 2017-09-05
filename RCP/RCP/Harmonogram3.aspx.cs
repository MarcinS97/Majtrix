using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using HRRcp.RCP.Controls.Harmonogram;
using HRRcp.App_Code;

namespace HRRcp.RCP
{
    public partial class Harmonogram3 : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)
                {
                    Tools.SetNoCache();
                    cntHarmonogramWrapper.Prepare();
                    //----------------------
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            get
            {

                return App.User.HasRight(AppUser.rPlanPracy) || App.User.HasRight(AppUser.rPlanPracySwoj) || App.User.IsAdmin;
            }
        }

        [WebMethod]
        public static void SaveSchedule(cntHarmonogram.SaveObject[] data)
        {
            cntHarmonogram.SaveSchedule(data);
        }

        [WebMethod]
        public static String GetErrors(string emp, string dataOd, string dataDo)
        {
            return cntHarmonogram.GetErrors(emp, dataOd, dataDo);
        }

        [WebMethod]
        public static String GetInstantErrors(string emp, string dataOd, string dataDo)
        {
            return cntErrorsPanel.GetInstantErrors(emp, dataOd, dataDo);
        }

        [WebMethod]
        public static String GetErrorsCount(string emp, string dataOd, string dataDo)
        {
            return cntErrorsPanel.GetErrorsCount(emp, dataOd, dataDo);
        }

        [WebMethod]
        public static String GetShifts(string message)
        {
            return cntHarmonogram.GetShifts();
        }

        [WebMethod]
        public static String GetHistory(string empId, string dateFrom, string dateTo)
        {
            return cntHarmonogram.GetHistory(empId, dateFrom, dateTo);
        }
    }
}