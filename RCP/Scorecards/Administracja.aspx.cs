using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards
{
    public partial class AdminForm : System.Web.UI.Page
    {
        private const string active_tab = "atabScAdm";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        private const string FormName = "Scorecards - Administracja";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.IsScAdmin)
                {
                    Tools.SetNoCache();
                    Tools.EnableUpload();
                    
                    int p = Tools.StrToInt(Tools.GetStr(Request.QueryString["p"]), -1);
                    if (0 <= p && p < tabAdmin.Items.Count)
                    {
                        tabAdmin.Items[p].Selected = true;
                    }
                    else
                    {
                        Tools.SelectMenuFromSession(tabAdmin, active_tab);
                    }
                    SelectTab();

                    Tools.ExecOnStart2("resize2", "resize();");
                }
                else
                    App.ShowNoAccess(FormName, null);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }
        //----------------------------------        
        protected void tabAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {
            /*aa
            const string msg = "Trwa edycja danych pracownika.\\nPrzed zmianą zakładki proszę ją zakończyć.";

            switch (mvPracownicy.ActiveViewIndex)
            {
                case 1:
                    if (cntPracownicy.List.EditIndex != -1)
                    {
                        Tools.SelectMenu(tabAdmin, mvPracownicy.ActiveViewIndex.ToString()); // nie zmieniam taba
                        Tools.ShowMessage(msg);
                        return;
                    }
                    break;
            }
             */
            SelectTab();
        }

        private void SelectTab()
        {
            Session[active_tab] = tabAdmin.SelectedValue;
            Tools.SelectTab(tabAdmin, mvAdministracja, active_tab, false);
        }

        //------------------------------------------------
        protected void btExportTeksty_Click(object sender, EventArgs e)
        {
            //ExportExcel(DateTime.Today.ToString("yyyymmdd") + "_prp_teksty");
            Report.ExportCSV(DateTime.Today.ToString("yyyyMMdd") + "_sc_teksty", "select * from Teksty", null, null);
        }

        protected void btImportTeksty_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(@"uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUpload1.SaveAs(savePath);
                AdmInfoControl.ImportData(savePath);
                AdmInfoControl.List.DataBind();
                Tools.ShowMessage("Import zakończony.");
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }
        //--------------------------------------------
    }
}
