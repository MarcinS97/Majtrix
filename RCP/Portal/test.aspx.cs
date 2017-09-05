using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            Log.Info(Log.t2APP, "PAGELOAD", String.Format("{0} {1}", Request.QueryString["preview"], Request.QueryString["fileId"]));

            if (Request.QueryString["preview"] == "1" && !string.IsNullOrEmpty(Request.QueryString["fileId"]))
            {
                var fileId = Request.QueryString["fileId"];
                var fileContents = (byte[])Session["fileContents_" + fileId];
                var fileContentType = (string)Session["fileContentType_" + fileId];

                if (fileContents != null)
                {
                    Response.Clear();
                    Response.ContentType = fileContentType;
                    Response.BinaryWrite(fileContents);
                    Response.End();
                }
            }
             */ 
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string htmlContents = System.Web.HttpUtility.HtmlDecode(TextBox1.Text);
            //TextBox1.Text = htmlContents;
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
