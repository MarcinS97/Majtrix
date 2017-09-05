using AjaxControlToolkit;
using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntImageEditModal : System.Web.UI.UserControl
    {


        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        public void Show()
        {
            cntModal.Show(false);
        }





        private void GetImageSize(int origwidth, int origheight, int maxwidth, int maxheight, out int width, out int height)  // na razie założenie maxwidth = maxheight
        {
            if (origwidth > origheight)
            {
                width = maxwidth;
                height = origheight * maxwidth / origwidth;
            }
            else
            {
                height = maxheight;
                width = origwidth * maxheight / origheight;
            }
        }


        protected void FileUploadComplete(object sender, AsyncFileUploadEventArgs e)
        {
            string empId = App.User.Id;
            
            string file_name = empId + ".png";
            string thumb_name = empId + "_thumb.png";
            AsyncFileUpload1.SaveAs(Server.MapPath("uploads/avatars/") + file_name);

            

        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
            if (e.State == AsyncFileUploadState.Failed)
                Tools.ShowError("Ładownie zdjęcia nie powiodło się");
        }
       


    }
}