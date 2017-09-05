using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace HRRcp
{
    public partial class PrePrint : System.Web.UI.Page
    {
        public string docName {
            get
            {
                return crPrintName.Value;
            }
            set
            {
                crPrintName.Value = value;
            }
        }
        string[] _files;
        public string[] files
        {
            get
            {
                if (_files == null)
                    _files = GetPages(docName);
                return _files;
            }
        }
        public int currentPage
        {
            get
            {
                return Tools.StrToInt(crPrint.Value, 1);
            }
            set
            {
                crPrint.Value = value.ToString();
                IMGPRT.ImageUrl = "pliki/" + Path.GetFileName(files[value - 1]);
                CPr.Text = value.ToString();
            }
        }
        public int lastPageNum
        {
            get
            {
                return Tools.StrToInt(crPrintMax.Value, GetLastPageNumber(files));
            }
            set
            {
                crPrintMax.Value = value.ToString();
                MPr.Text = value.ToString();
            }
        }

        void FirstLoad()
        {
            string docId = (string)Session["DocID"];
            if (string.IsNullOrEmpty(docId)) Response.Redirect("~/default.aspx");
            SqlConnection con = db.Connect(db.PORTAL);
            DataRow dr = db.getDataRow(con, "select * from SqlMenu where Id = " + docId);
            string fn = Path.GetFileNameWithoutExtension(Path.GetFileName(db.getValue(dr, "Command")));
            db.Disconnect(con);
            docName = fn;
            currentPage = 1;
            lastPageNum = GetLastPageNumber(files);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                FirstLoad();
        }

        string[] GetPages(string name)
        {
            return Directory.GetFiles(Server.MapPath(@"~/portal/pliki/"), Path.GetFileNameWithoutExtension(name) + "*.png");
        }
        int GetLastPageNumber(string[] data)
        {
            return (from file in data select Tools.GetPageNum(file)).DefaultIfEmpty(0).Max();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("PDFViewer.aspx");
        }
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            if(currentPage + 1 > lastPageNum)
                Response.Redirect("PDFViewer.aspx");
            else
                currentPage++;
        }
    }
}
