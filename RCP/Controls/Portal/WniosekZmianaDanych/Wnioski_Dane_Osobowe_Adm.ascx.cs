using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
namespace HRRcp.Controls.WniosekZmianaDanych
{
    public partial class Wnioski_Dane_Osobowe_Adm : System.Web.UI.UserControl
    {
        
        
        
        private const string active_menu = "mn_wndoA";
        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(mnLeft, mvPrzesuniecia, active_menu, setHelp);

           
        }
        protected void GetPracId(object sender, EventArgs e)
        {
            zoom7.Visible = true;
            string asd = sender.ToString();
            zoom7.SetPracId(sender.ToString());
            //(zoom7.FindControl("PracIdGetName")as HiddenField).Value=e.ToString();
            zoom7.GetSelectCommand(true);
        }
        protected void HidenPrac(object sender, EventArgs e)
        {
            zoom7.SetPracId("H");
            zoom7.Visible = false;
            //(zoom7.FindControl("PracIdGetName") as HiddenField).Value =" ";
        }
        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
           
            }
        protected void Page_Load(object sender, EventArgs e)
        {
            GetName1.GetPracId += new EventHandler(GetPracId);
            GetName1.HidenPrac += new EventHandler(HidenPrac);
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            string filename = String.Format("Wnioski Zmiany Danych {0}", Tools.DateToStrDb(DateTime.Now));
            //Report.ExportExcel(hidReport.Value, filename, null);

            Report.ExportCSV(filename, zoom6.DataSource, null, null, true, false);
        }


       

    }
}