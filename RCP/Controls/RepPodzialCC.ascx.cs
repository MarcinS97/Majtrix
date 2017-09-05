using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;

namespace HRRcp.Controls
{
    public partial class RepPodzialCC : System.Web.UI.UserControl
    {
        const string sesid = "_class_sel";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Tools.SelectMenuFromSession(tabWyniki, sesid);
                //Tools.SelectMenuFromSession(cblClass, sesid);

                object o = Session[sesid];
                if (o == null)
                    rblClass.SelectedIndex = 0;
                else
                    Tools.SelectItem(rblClass, o);
                SelectTab();

                btUprawnienia.Visible = App.User.IsAdmin && App.User.HasRight(AppUser.rRights);
            }
        }

        public void Execute()
        {
            //cntReport1.Execute();
        }

        //----------
        protected void tabWyniki_MenuItemClick(object sender, MenuEventArgs e)
        {
            //SelectTab();
            //Session[sesid] = tabWyniki.SelectedValue;
        }

        protected void clbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SelectTab();
            //Session[sesid] = cblClass.SelectedValue;
        }

        protected void rblClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectTab();
            Session[sesid] = rblClass.SelectedValue;
        }
        //----------
        private void SelectTab()
        {
            if (App.User.HasRight(AppUser.rRepPodzialCC))
            {
                if (App.User.HasRight(AppUser.rRepPodzialCCAll))        // uprawnienia - usuwam joiny do uprawnień
                {
                    cntReport1.SQL1 = null;
                    //SqlDataSource1.FilterExpression = null;
                }
                else
                {
                    /*
                    SqlDataSource1.FilterExpression = String.Format(    // zakres tylko do uprawnień
                        "Class in (select CC from ccPrawa where IdCC = 0 and UserId = {0})",
                        App.User.Id);
                     */ 
                }
                if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))      // są kwoty - ustawiam joiny do kwot
                    cntReport1.SQL2 = cntReport1.P2;
                else
                    cntReport1.SQL5 = null;                             // nie ma kwot - tabela tmp bez pól

                //string c = tabWyniki.SelectedValue;
                //string c = cblClass.SelectedValue;

                /*
                string c = rblClass.SelectedValue;
                cntReport1.SQL4 = rblClass.SelectedItem.Text;
                cntReport1.SQL5 = c;
                if (c == "all")
                */                  
                cntReport1.Title3 = App.GetccPrawaList(App.User.Id);
                cntReport1.NoDataInfo = null;                           // domyslny
                cntReport1.Prepare();
            }
            else
            {
                cntReport1.SQL = null;
                cntReport1.NoDataInfo = "Brak uprawnień";
                cntReport1.Prepare();
                //App.ShowNoAccess("Raporty", App.User);
            }
        }
        //---------------------
        public cntReport Report
        {
            get { return cntReport1; }
        }

        public string Title
        {
            get { return cntReport1.Title; }
        }

    }
}