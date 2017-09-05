using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using AjaxControlToolkit;
using HRRcp.App_Code;
using System.Globalization;

namespace HRRcp.Portal
{
    public partial class Newsletter : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //zzzz
                if (Lic.portalPrint && App.User.IsKiosk)
                {
                    SqlConnection con = db.Connect(db.PORTAL);
                    DataRow dr = db.getDataRow(con, String.Format("select top 1 Id from SqlMenu where Grupa = 'GAZETKA' and Kolejnosc = 1 order by Id desc", GAZETKA));
                    Session["DocID"] = db.getValue(dr, "Id");
                    db.Disconnect(con);

                    //zzzz
                    Response.Redirect("PDFViewer.aspx");
                }
                Init();
            }
        }
        //----------------------
        const string GAZETKA = "GAZETKA";

        private void Init()
        {
            string fn;
            Literal1.Text = GetGazetka(out fn);
            bool adm = App.User.IsPortalAdmin || App.User.HasRight(AppUser.rPortalArticles);
            paAddPlik.Visible = adm;
            if (adm) lbFile.Text = fn;
        }

        private string GetGazetka(out string file)
        {
            SqlConnection con = db.Connect(db.PORTAL);
            DataRow dr = db.getDataRow(con, String.Format("select top 1 * from SqlMenu where Grupa = '{0}' and Kolejnosc = 1 order by Id desc", GAZETKA));
            db.Disconnect(con);
            if (dr != null)
            {
                file = db.getValue(dr, "Command");
                return GetGazetkaHTML(ref file);
                //return "<embed src=\"http://localhost:50675/Portal/MS_Win8-v.5_tcm3-163290.pdf\" class=\"viewer\" >";    
            }
            else
            {
                file = null;
                return String.Format("<span class=\"nodata\">{0}</span>", "Brak danych");
            }
        }

        private string GetGazetkaHTML(ref string file)
        {
            if (file.StartsWith("~")) file = file.Substring(1);

            string b1 = ((HttpCapabilitiesBase)(Request.Browser)).Browser;
            //string b2 = Request.ServerVariables["HTTP_USER_AGENT"];
            
            
#if SPX
            file += "#toolbar=0";
#endif
            
            
            if (b1 == "InternetExplorer")
            {
                return String.Format("<embed src=\"{0}\" class=\"viewer\" >", file);
            }
            else
            {
                return String.Format("<embed src=\"{0}\" class=\"viewer\" >", file);
            }
        }

        private bool Update(string cmd)
        {
            SqlConnection con = db.Connect(db.PORTAL);
            bool ok = db.update(con, "SqlMenu", 1, "Command", "Grupa='{0}' and Kolejnosc = 1", GAZETKA, db.nullStrParam(cmd));
            if (!ok)
                ok = db.insert(con, "SqlMenu", 0, "Grupa,MenuText,Command,Kolejnosc", db.strParam(GAZETKA), db.strParam("Newsletter"), db.nullStrParam(cmd), 1);
            db.Disconnect(con);
            return ok;
        }
        //-----------------------------------
        const string PlikiPath = @"../portal/gazetka/";

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            AsyncFileUploadEventArgs args = e as AsyncFileUploadEventArgs;
            AsyncFileUpload fu = sender as AsyncFileUpload;

            //Log.Info(Log._PORTAL, "upload", String.Format("{0} {1}", args.FileName, args.StatusMessage));

            string fileName = Path.GetFileName(fu.PostedFile.FileName);
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".pdf")
            {
                string savePath = Server.MapPath(PlikiPath);

                string fn = fileName;   //GetFileName(, fileName) 
                fu.SaveAs(savePath + fn);

                string cmd = PlikiPath + fn;  // link
                //string ico = GetIco(fileName);

                bool ok = Update(cmd);

                if (ok)
                {
                    Tools.ExecOnStart2("gupdate", String.Format("top.document.getElementById('{0}').click();", btUpdate.ClientID));
                    //ShowEdit(false);
                    //Literal1.Text = GetGazetkaHTML(ref cmd);
                }
                else Tools.ShowError("Wystąpił błąd podczas zapisu do bazy.");
            }
            else
                Tools.ShowError("Plik powinien być w formacie pdf.");
        }

        private string GetIco(string fname)
        {
            string ext = Path.GetExtension(fname).ToLower();  // potem zmienić na baze !!!
            switch (ext)
            {
                case ".pdf":
                    return "../../images/fileext/pdf.png";
                case ".doc":
                case ".docx":
                    return "../../images/fileext/doc.png";
                case ".xls":
                case ".xlsx":
                    return "../../images/fileext/xls.png";
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                    return "../../images/fileext/img.png";
                default:
                    return "../../images/fileext/dok.png";
            }
        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
            Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }

        private void ShowEdit(bool show)
        {
            paButton.Visible = !show;
            paEdit.Visible = show;
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            ShowEdit(true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            ShowEdit(false);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            App.Redirect(App.PortalGazetkaForm);
        }



        //----------------------------------

    }
}
