using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.IO;

namespace HRRcp.Portal.Controls
{
    public partial class cntSearch : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvSearch, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Search(string search, int logId)
        {
            hidSearch.Value = search;
            hidLogId.Value = logId.ToString();
            hidPrac.Value = App.User.IsKierownik || App.User.IsPortalAdmin ? "0" : "1";
        }
        //-----------------
        public bool IsVisible(object image)
        {
            //return !db.isNull(image);
            return true;
        }

        public string GetPath(object path)
        {
            return path.ToString();
        }

        public string GetImage(object path, string defIco)
        {
            if (String.IsNullOrEmpty(path.ToString()))
                return defIco;
            else
                return path.ToString();
        }

        public string GetLink(object cmd)
        {
            return Tools.Substring(cmd.ToString(), 16, 9999);
        }
        //------------------
        protected void lvSearch_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "download":
                    string id = Tools.GetText(e.Item, "hidId");
                    LinkButton lbt = e.Item.FindControl("LinkButton1") as LinkButton;
                    string cmd = lbt != null ? lbt.CommandArgument : null;
                    int pid = Tools.StrToInt(hidLogId.Value, 0);
                    Log.Info(Log.SEARCHSELECT, pid, id, null, hidSearch.Value, cmd, Log.OK);
                    cntPlikiPliki.HandleCommand(id);
                    break;
                case "article":
                    id = Tools.GetText(e.Item, "hidId");
                    lbt = e.Item.FindControl("LinkButton1") as LinkButton;
                    cmd = lbt != null ? lbt.CommandArgument : null;

                    if (String.IsNullOrEmpty(cmd))
                        cmd = "url:Portal.aspx";    // aktualności

                    pid = Tools.StrToInt(hidLogId.Value, 0);
                    Log.Info(Log.SEARCHSELECT, pid, id, null, hidSearch.Value, cmd, Log.OK);

                    string url;
                    if (Tools.IsUrl(cmd, out url))
                    {
                        //if (!String.IsNullOrEmpty(grp))
                        //    HttpContext.Current.Session["lmenugrp"] = grp;   // symulacja wyboru menu
                        App.Redirect(url);
                    }

                    break;
            }
        }

        protected void lvSearch_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        protected void lvSearch_DataBound(object sender, EventArgs e)
        {
            Tools.RegisterPostBackControls(lvSearch, "LinkButton1");
        }

        public string GetIco(object ofname)
        {
            string fname = ofname.ToString();

            string ext = Path.GetExtension(fname).ToLower();  // potem zmienić na baze !!!
            switch (ext)
            {
                case ".pdf":
                    //return "../../images/fileext/pdf.png";
                    return "fa fa-file-pdf-o";
                case ".doc":
                case ".docx":
                    //return "../../images/fileext/doc.png";
                    return "fa fa-file-text-o";
                case ".xls":
                case ".xlsx":
                    //return "../../images/fileext/xls.png";
                    return "fa fa-file-excel-o";
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                    //return "../../images/fileext/img.png";
                    return "fa fa-file-image-o";
                default:
                    //return "../../images/fileext/dok.png";
                    return "fa fa-file-o";
            }
        }

    }
}