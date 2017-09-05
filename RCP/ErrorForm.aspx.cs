using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class ErrorForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbInfo.Text = AppError.Info;
                if (AppError.InfoEx != null)
                    lbInfoEx.Text = AppError.InfoEx.Replace("\r\n", "<br />");
                switch (AppError.Button)
                {
                    default:
                    case AppError.btDefault:
                        lbInfoOk.Visible = true;
                        btOk.Visible = true;
                        break;
                    case AppError.btClose:
                        btClose.Visible = true;
                        break;
                    case AppError.btBack:
                        btBack.Visible = true;
                        Tools.MakeBackButton(btBack);
                        break;
                    case AppError.btNone:
                        break;
                }
#if MS
                cssMS.Visible = true;
#elif SIEMENS
                css1.Visible = true;
                //cssSIE.Visible = true;
                HeaderControl1.Logo.Visible = true;
#elif OKT || APATOR
                cssPortal.Href = App.StylePath + "custom.css";
                cssPortal.Visible = true;
#else
                HeaderControl1.LogoImage = "~/images/iqor1a.png";
                HeaderControl1.Logo.Visible = true;
#endif      
#if PORTAL
    #if SPX
                cssPortal_SPX.Visible = true;
    #elif KDR
                cssPortal_KDR.Visible = true;
    #else
                cssPortal_iQor.Visible = true;
    #endif 
                FooterControl1.RegulaminButton.Visible = false;
#else   // RCP
    #if SIEMENS
    #else
                HeaderControl1.LogoImage = "~/images/iqor/iqor2b.png";
                cssPortal_iQor.Visible = true;
    #endif
#endif
            }
        }
    }
}


/*
  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en"> 
<head> 
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />     
    <title>Sorry, an error has occurred</title> 
    <style type="text/css">
        body { margin:0;font-family:Arial;letter-spacing:-0.02em;text-align:center }
        h2 { font-size:1.6em;font-weight:normal;letter-spacing:-0.06em; color:#69D2E7; }
        p { font-size:0.9em;color:#888 }
        .content { text-align:left;width:600px;margin:0 auto }
    </style>
</head> 
<body>
    <div class="content">
        <h2>Sorry, an error has occurred...</h2>
        <p>
            Unfortunately an error has occurred during the processing of your page request.  Please be assured we log and review all errors, even if you do not report this error
            we will endeavor to correct it.
        </p>
    </div>
</body>
</body> 
</html>
 */