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
    public partial class PDFViewer : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
            }
        }
        //----------------------
        const string GAZETKA = "GAZETKA";

        private void Init()
        {
            //cPDFReader1.PDFPath = 
        }
    }
}
