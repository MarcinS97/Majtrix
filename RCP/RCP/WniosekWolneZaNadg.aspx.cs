using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.RCP
{
    public partial class WniosekWolneZaNadg : System.Web.UI.Page
    {
        const string sesId = "wniosekIdsie";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool vK = false;
                string wnId = Tools.GetStr(Session[sesId]);
#if SIEMENS
                if (!String.IsNullOrEmpty(wnId))
                {
                    string podtyp = db.getScalar("select PodTyp from poWnioskiUrlopowe where Id = " + wnId);
                    vK = podtyp == "2";
                    cntWniosekWolneZaNadg1.Visible = !vK;
                    cntWniosekWolneZaNadgKier.Visible = vK;
                }
                
                if (vK)
                    cntWniosekWolneZaNadgKier.Prepare(wnId);
                else
                    cntWniosekWolneZaNadg1.Prepare(wnId);
#elif RCP
                if (!String.IsNullOrEmpty(wnId))
                {
                    string podtyp = db.getScalar("select PodTyp from poWnioskiUrlopowe where Id = " + wnId);
                    vK = podtyp == "2";
                    cntWniosekWolneZaNadg1.Visible = !vK;
                    cntWniosekWolneZaNadgKier.Visible = vK;
                }
                
                if (vK)
                    cntWniosekWolneZaNadgKier.Prepare(wnId);
                else
                    cntWniosekWolneZaNadg1.Prepare(wnId);
#else
                cntWniosekWolneZaNadg1.Visible = false;
                cntWniosekWolneZaNadgKier.Visible = false;
#endif

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
            App.Redirect("RCP/WniosekWolneZaNadg.aspx");
#else
            App.Redirect("WniosekWolneZaNadg.aspx");
#endif
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string wnId = cntWniosekWolneZaNadg1.Visible ? cntWniosekWolneZaNadg1.WniosekId : cntWniosekWolneZaNadgKier.WniosekId;
            db.execSQL("delete from poWnioskiUrlopowe where Id = " + wnId);
            Tools.ExecOnStart2("wnback", "history.back();");
        }
    }
}
