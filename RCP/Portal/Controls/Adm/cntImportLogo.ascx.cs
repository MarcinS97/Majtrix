using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HRRcp.Portal.Controls.Adm
{
    public partial class cntImportLogo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ShowModal()
        {
            cntModal3.Show();            
        }

        protected void btnLogo_Click(object sender, EventArgs e)
        {
            if (logoFile.HasFile)
            {                
                string fileName = logoFile.FileName;
                string savePath = Server.MapPath(@"~\styles\User\") + "logo.png";  
                logoFile.SaveAs(savePath);               
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  
            }
        }
    }
}