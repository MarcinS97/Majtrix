using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal
{
    public partial class PdfContent : System.Web.UI.Page
    {
        string typ = null;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            typ = Request.QueryString["t"];
 
            if (String.IsNullOrEmpty(typ))  
                typ = Request.QueryString["p"];    //20170513 T:kompatybilność z Content.aspx 

            cntPDFViewer.Grupa = typ;     
        }
    }
}