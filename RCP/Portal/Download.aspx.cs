using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRApp.Portal
{
    public partial class Download : System.Web.UI.Page
    {
        const string pageId = "Download";

        protected void Page_Load(object sender, EventArgs e)
        {
            //string p = Tools.GetStr(Request.QueryString["p"].Replace(' ', '+'));   // wywala exception jak bez parametrów
            string p = Tools.GetStr(Request.QueryString["p"]);
            if (!String.IsNullOrEmpty(p))
            {
                p = p.Replace(' ', '+');
                /*
                int pp;
                if (Int32.TryParse(p, out pp))   // plomb, coś więcej niż plomba ... żeby można było przekazać nieszyfrowane
                    p = pp.ToString();
                else
                    p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                */
                p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                if (!String.IsNullOrEmpty(p))
                {
                    string[] par = Tools.GetLineParams(p);   // 0-repId
                    string repId = par[0];
                    int fid = Tools.StrToInt(repId, -1);
                    AppUser user = AppUser.CreateOrGetSession();  // nie ma masterpage !!!!!
                    string HR_DB = db.GetDbName(db.conStr);
                    SqlConnection con = db.Connect(db.PORTAL);
                    DataRow dr = db.getDataRow(con, String.Format(dsGetFile.SelectCommand, fid, user.NR_EW, HR_DB));
                    db.Disconnect(con);
                    if (CanDownload(dr))
                    {
                        Exception ex;
                        if (!DownloadFile(dr, out ex))
                            ShowError(fid, ex);
                    }
                    else ShowError(user, fid);
                }
            }
            ShowError();        // to co za tym jest właściwie niepotrzebne ...
            Response.Write("<script language=javascript>this.window.opener = null;window.open('','_self'); window.close();</script>");
            Response.End();   // po prostu zamykam jak bez parametrów
        }

        private void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(pageId);
        }

        public bool CanDownload(DataRow dr)
        {            
            return db.getValue(dr, "CanDownload") == "1";
                                //if (App.User.NR_EW == db.getValue(dr, "Idx")
                                // || App.User.IsPortalAdmin
        }

        //-------------------------------
        private void ShowError(AppUser user, int fid)
        {
            AppError.Show(String.Format("Użytkownik {0} nie ma uprawnień do pobrania dokumentu.", user.ImieNazwisko), fid.ToString());
        }

        private void ShowError(int fid, Exception ex)
        {
            //AppError.Show(String.Format("Błąd podczas pobierania pliku. Id = {0}.", fid));
            AppError.Show("Wystąpił błąd podczas pobierania pliku.", null, String.Format("Id={0} {1}", fid, ex.Message), null, AppError.btDefault);
        }

        private void ShowError()
        {
            AppError.Show(pageId, "Niepoprawne parametry uruchomienia.");
        }
        //-------------------------------
        public string GetContentType(string FileName)
        {
            string ext = Path.GetExtension(FileName).ToLower();
            switch (ext)
            {
                case ".doc":
                case ".docx":
                    return "application/msword";
                case ".xls":
                    return "application/excel";
                //return "application/vnd.ms-excel";
                case ".pdf":
                    return "application/pdf";
                case ".zip":
                    return "application/zip";
                case ".txt":
                    return "text/plain";
                // graphics
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".tif":
                    return "image/tif";
                case ".bmp":
                    return "image/bmp";

                default:
                    return "application/unknown";
            }
        }

        //http://refactoringaspnet.blogspot.com/2008/11/how-to-get-content-type-mimetype-of.html

        private bool DownloadFile(DataRow dr, out Exception retex)
        {
            string path  = db.getValue(dr, "Sciezka");
            string fname = db.getValue(dr, "NazwaPliku");
            retex = null;
            try
            {
                using (var fileStream = new FileStream(Server.MapPath(Tools.Slash(path) + fname), FileMode.Open))
                {
                    //string fn = HttpUtility.UrlEncode(Base.getValue(dr, 0), System.Text.Encoding.UTF8).Replace('+', ' ');  //UrlEncode zamienia ' ' na '+', więc trzeba odwrotnie ...
                    //string fn = Tools.UnicodeToUtf8(Base.getValue(dr, 0));
                    //string ct = Base.getValue(dr, 1);
                    //int size = Base.getInt(dr, 2, 0);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = GetContentType(fname);
                    //Response.ContentType = ct;
                    Response.AddHeader("content-disposition", "attachment; filename=" + fname);
                    //Response.OutputStream.Write((byte[])dr[3], 0, size);


                    byte[] bufferWrite = new byte[fileStream.Length];
                    fileStream.Read(bufferWrite, 0, bufferWrite.Length);
                    Response.OutputStream.Write(bufferWrite, 0, bufferWrite.Length);
                    Response.End();
                    return true;
                }
            }
            catch (Exception ex)
            {
                retex = ex;
            }
            return false;
        }

        private bool DownloadFile_1(string FileId)
        {
            DataRow dr = Base.getDataRow("select FileName, MIMEType, DATALENGTH(FileData) as Size, FileData from AnkietyDokumenty where Id = " + FileId);
            if (dr != null)
            {
                //string fn = HttpUtility.UrlEncode(Base.getValue(dr, 0), System.Text.Encoding.UTF8).Replace('+', ' ');  //UrlEncode zamienia ' ' na '+', więc trzeba odwrotnie ...
                string fn = Tools.UnicodeToUtf8(Base.getValue(dr, 0));
                string ct = Base.getValue(dr, 1);
                int size = Base.getInt(dr, 2, 0);
                Response.Clear();
                Response.Buffer = true;
                //Response.ContentType = GetContentType(fn);
                Response.ContentType = ct;
                Response.AddHeader("content-disposition", "attachment; filename=" + fn);
                Response.OutputStream.Write((byte[])dr[3], 0, size);
                Response.End();
                return true;
            }
            else return false;
        }

        /*
        private bool DownloadFile2(string FileId)
        {
            bool ret = false;
            string sql = "select FileName, DATALENGTH(FileData) as Size, FileData from AnkietyDokumenty where Id = " + FileId;
            SqlConnection con = Base.Connect();
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                SqlDataReader data = sqlCmd.ExecuteReader();
                data.Read();
                //??????

                //if (data.RecordsAffected >= 0)
                {
                    string fn = (string)data["FileName"];
                    int size = (int)data["Size"];
                    Response.Clear();
                    //Response.Buffer = true;
                    Response.ContentType = GetContentType(fn);
                    //EnableViewState = false;
                    Response.AddHeader("content-disposition", "attachment; filename=" + fn);

                    Response.OutputStream.Write((byte[])data["FileData"], 0, size);

                    //Response.Write("<script language=javascript>this.window.opener = null;window.open('','_self'); window.close();</script>");

                    Response.End();
                    ret = true;
                }
            }
            finally
            {
                Base.Disconnect(con);
            }
            return ret;
        }
        */
    }
}
