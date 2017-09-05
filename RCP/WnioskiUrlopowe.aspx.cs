using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls.Portal;

namespace HRRcp
{
    public partial class WnioskiUrlopoweForm : System.Web.UI.Page
    {
        const string active_tab = "wuseltab";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();




                App.KwitekKadryId = App.User.NR_EW;
                App.KwitekPracId = db.getScalar("select Id from Pracownicy where KadryId = " + App.KwitekKadryId);
                


                
                
                
                Urlop1.Prepare(null, false);
            }
            //mvWnioskiUrlopowe.ActiveViewIndex = Tools.GetInt(Session[active_tab], 0);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Wnioski Urlopowe Form");
        }

        //-------------------------------
        protected void mvWnioskiUrlopowe_ActiveViewChanged(object sender, EventArgs e)
        {
            //Session[active_tab] = mvWnioskiUrlopowe.ActiveViewIndex;
        }

        protected void cntWnioskiUrlopoweSelect1_Select(object sender, EventArgs e)
        {
            int wtyp = Tools.StrToInt(cntWnioskiUrlopoweSelect1.SelectedTyp, -1);
            if (wtyp != -1)
            {
                //mvWnioskiUrlopowe.SetActiveView(vWniosek);
                //cntWniosekUrlopowy1.PrepareNew(wtyp, cntWniosekUrlopowy.osPracownik);

                cntWniosekUrlopowy1.PrepareNewPopup(extWniosekPopup, wtyp, cntWniosekUrlopowy.osPracownik);
                UpdatePanel5.Update();    // jezeli wywołanie jest z updatepanel to trzeba też zrobić update
            }
        }
        
        protected void cntWnioskiUrlopowe1_Show(object sender, EventArgs e)
        {
            //mvWnioskiUrlopowe.SetActiveView(vWniosek);
            //cntWniosekUrlopowy1.Show(cntWnioskiUrlopowe1.ShowWniosekId, cntWniosekUrlopowy.osPracownik, false);

            cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, cntWnioskiUrlopowe1.ShowWniosekId, cntWniosekUrlopowy.osPracownik, false);
            UpdatePanel5.Update();    // jezeli wywołanie jest z updatepanel to trzeba też zrobić update
        }

        protected void cntWnioskiUrlopowe1_Hide(object sender, EventArgs e)
        {
            cntWniosekUrlopowy1._Clear();
            //mvWnioskiUrlopowe.SetActiveView(vSelect);
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
            cntWniosekUrlopowy1._Clear();
            //mvWnioskiUrlopowe.SetActiveView(vSelect);
            
            if (cntWniosekUrlopowy1.Updated)
            {
                cntWnioskiUrlopowe1.DataBind();   // mozna sprawdzić czy dodał
                UpdatePanel1.Update();
            }
        }
    }
}
