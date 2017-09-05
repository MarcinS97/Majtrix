using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using HRRcp.App_Code;
using System.IO;

namespace HRRcp.Controls.Portal
{
    public partial class cntArtykulyTest : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        const string ArtykulyPath = @"~/portal/artykuly/";

        protected void HtmlEditorExtender1_ImageUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            Log.Info(Log.t2APP, "UPLOAD_IN", String.Format("FieldId: {0} FileName: {1} FileSize: {2} PostedUrl: {3} ContentType: {4}", e.FileId, e.FileName, e.FileSize, e.PostedUrl, e.ContentType));

            if (e.ContentType.Contains("jpg") ||
                e.ContentType.Contains("gif") ||
                e.ContentType.Contains("png") ||
                e.ContentType.Contains("jpeg"))
            {
                Session["fileContentType_" + e.FileId] = e.ContentType;
                Session["fileContents_" + e.FileId] = e.GetContents();

                //string fn = e.FileName;
                string ext = Path.GetExtension(e.FileName);
                string fn = e.FileId + ext;

                string fullpath = Server.MapPath(ArtykulyPath) + fn;
                ((AjaxFileUpload)sender).SaveAs(fullpath);
                e.PostedUrl = Page.ResolveUrl(ArtykulyPath + fn);
            }

            switch (e.State)
            {
                case AjaxFileUploadState.Failed:
                    break;
                case AjaxFileUploadState.Success:
                    break;
                case AjaxFileUploadState.Unknown:
                    break;
            }

            // Set PostedUrl to preview the uploaded file.         
            //e.PostedUrl = string.Format("?preview=1&fileId={0}", e.FileId);

            Log.Info(Log.t2APP, "UPLOAD_OUT", String.Format("PostedUrl: {0}", e.PostedUrl));
        }

    }
}