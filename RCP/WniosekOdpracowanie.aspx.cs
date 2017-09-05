using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class WniosekOdpracowanie : System.Web.UI.Page
    {
        const string sesId = "wniosekIdsieOdpr";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if SIEMENS
                cntWniosekOdpracowanie1.Prepare(Tools.GetStr(Session[sesId]));
                cntWniosekOdpracowanie1.Visible = true;
#elif DBW
                cntWniosekOdpracowanie2.Prepare(Tools.GetStr(Session[sesId]));
                cntWniosekOdpracowanie2.Visible = true;
#elif VICIM
                cntWniosekOdpracowanie2.Prepare(Tools.GetStr(Session[sesId]));
                cntWniosekOdpracowanie2.Visible = true;
#elif VC
                cntWniosekOdpracowanie2.Prepare(Tools.GetStr(Session[sesId]));
                cntWniosekOdpracowanie2.Visible = true;
#else
                cntWniosekOdpracowanie1.Visible = false;
                cntWniosekOdpracowanie2.Visible = false;
#endif
                //App.Master.
                Tools.MakeConfirmButton(btDelete, "Potwierdź usunięcie wniosku.");

                lbPrintTime.Text = 
                    //Base.DateTimeToStr(DateTime.Now) + " " + 
                    App.User.ImieNazwisko;
                lbPrintVersion.Text = Tools.GetAppVersion();
            }
        }

        public static void Show(string wnId)
        {
            HttpContext.Current.Session[sesId] = wnId;
#if RCP
            App.Redirect("RCP/WniosekOdpracowanie.aspx");
#else
            App.Redirect("WniosekOdpracowanie.aspx");
#endif
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            /*
            db.execSQL("delete from poWnioskiUrlopowe where Id = " + cntWniosekWolneZaNadg1.WniosekId);
            Tools.ExecOnStart2("wnback", "history.back();");
            */
        }
    }
}
