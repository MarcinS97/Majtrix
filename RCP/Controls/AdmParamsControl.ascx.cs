using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class AdmParamsControl : System.Web.UI.UserControl
    {
        private const string active_menu = "admparlmenu";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if SIEMENS || DBW || VC || VICIM
                Tools.RemoveMenu(mnLeft, "DIC3");
                Tools.RemoveMenu(mnLeft, "vCommodity");
                Tools.RemoveMenu(mnLeft, "vArea");
                Tools.RemoveMenu(mnLeft, "vPosition");
                Tools.SetMenu(mnLeft, "vDzialy", "Działy i stanowiska");

                Tools.RemoveMenu(mnLeft, "DIC4");
                Tools.RemoveMenu(mnLeft, "ABSDL");
                Tools.RemoveMenu(mnLeft, "PUADM");
#else
#endif
#if DBW
                Tools.RemoveMenu(mnLeft, "DIC1");
                Tools.RemoveMenu(mnLeft, "CC");
                Tools.RemoveMenu(mnLeft, "SP");
#endif
#if VC || VICIM
                Tools.RemoveMenu(mnLeft, "DIC1");
                Tools.RemoveMenu(mnLeft, "SP");
                Tools.RemoveMenu(mnLeft, "OR");
#endif
                if(!Lic.GrupyUprawnien)
                    Tools.RemoveMenu(mnLeft, "vGrupyUpr");
                mnLeft.Items[0].Selected = true;
                SelectTab(false);
            }
            /*
            Tools.SelectMenuFromSession(mnLeft, active_tab);
            SelectTab();
            */
        }

        protected void lvAlgorytmy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Button b = (Button)e.Item.FindControl("DeleteButton");
                if (b != null) Tools.MakeConfirmDeleteRecordButton(b);
            }
        }


        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab(true);
        }
        //--------------------------------------
        private void selectLeftMenu()
        {
            /*
            clearErrors();
            bool ed = lvEndEditing(true);
            if (ed)
                Tools.ExecOnStart("selectLeftMenu");
            else
                DoSelectLeftMenu();
             */
        }

        private void SelectTab(bool setHelp)
        {
            switch (mnLeft.SelectedValue)
            {
                default:
                    Tools.SelectTab(mnLeft, mvParams, active_menu, true);
                    break;
            
                case "PA":
                    mvParams.SetActiveView(pgParams);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMPARAMETRY);
                    break;
                case "OR":
                    mvParams.SetActiveView(pgOkresy);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMOKRESY);
                    break;
                case "SG":
                    mvParams.SetActiveView(pgStawki);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMSTAWKI);
                    break;
                case "AL":
                    mvParams.SetActiveView(pgAlgorytmy);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMALGORYTMY);
                    break;
                case "KA":
                    mvParams.SetActiveView(pgKodyAbsencji);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMKODYABS);
                    break;
                case "KP":
                    mvParams.SetActiveView(pgKierParams);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMKIERPARAMS);
                    break;
                case "CC":
                    mvParams.SetActiveView(pgCC);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMMPK);
                    break;
                case "CM":
                    mvParams.SetActiveView(pgLinie);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMMPK);
                    break;
                case "SP":
                    mvParams.SetActiveView(pgSplity);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMMPK);
                    break;
                case "ABSDL":
                    mvParams.SetActiveView(vAbsDl);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMABSDL);
                    break;
                case "PUADM":
                    mvParams.SetActiveView(vPlanUAdm);
                    if (setHelp) Info.SetHelp(Info.HELP_ADMPUADM);
                    break;
            }
        }

        private void clearErrors()
        {
            /*
            Tools.SetError(lbGrupyError, null);
            Tools.SetError(lbSzkolError, null);
             */
        }

        protected void btLeftMenuSelect_Click(object sender, EventArgs e)
        {
            //DoSelectLeftMenu();
        }

        protected void lvAbsencjaKody_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

    }
}