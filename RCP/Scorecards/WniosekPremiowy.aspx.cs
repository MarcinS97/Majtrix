using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;
using HRRcp.Controls.EliteReports;

namespace HRRcp.Scorecards
{
    public partial class WniosekPremiowy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!App.User.IsScAdmin)
                    App.ShowNoAccess("1337", App.User);

                String Msg = "Row1 \\n Row2";
                //Tools.ShowMessage(Msg.Replace("\n", "\\n").Replace("\\n", "<br />"));
                //Tools.MakeConfirmDeleteRecordButton(btnTest);
                
                //Productivity.Prepare("1", "1");
                
                //Tools.ShowConfirm("asdasdas", btnYes, btnNo);
                //Tools.ShowMessages("asd", "asd2");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            // ScriptManager asd = ScriptManager.GetCurrent(this.Page);
            // your code
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            //canvasImg.ImageUrl = hidImg.Value;
            PDF PDF = new PDF();
            PDF.Download(UpdatePanel1, Server, Response, Request, "asd");
        }

        protected void Yes(object sender, EventArgs e)
        {
            Tools.ShowMessage("Yes");
        }

        protected void No(object sender, EventArgs e)
        {
            Tools.ShowMessage("No");
        }

        protected void SomeFunction(object sender, EventArgs e)
        {
            Tools.ShowMessage("ad");


        }
    }
}
