using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe
{
    public partial class cntEksportyWyslane : System.Web.UI.UserControl
    {
        const string ExportPath = "~/Export/";
        string ExportEmail = UbezpieczeniaParametry.Email;

        const int ACTUAL = -9;

        const string schGRUPA = "UBEZPIECZENIA";
        const string schTYP   = "EXPORT";

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvEksporty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Tools.MakeConfirmButton(btExport, String.Format("Potwierdź wykonanie eksportu danych. Plik zostanie wysłany na adres: {0}.", ExportEmail));
                Tools.MakeConfirmButton(btExport, String.Format("Potwierdź wykonanie eksportu danych. Plik zostanie wysłany na wskazany w konfiguracji adres email."));
            }
        }
        //----------------------
        private bool Export(int wid, bool mail)
        {
            if (wid == ACTUAL)
            {
                string id = db.Select.Scalar("select top 1 Id from Scheduler where Grupa = '{0}' and Typ = '{1}'", schGRUPA, schTYP);
                if (!String.IsNullOrEmpty(id))
                    Service.CheckExecAuto(db.con, 0, id);
                else
                    Tools.ShowErrorLog(Log._PORTAL, "Ubezpieczenia.Export - brak definicji Schedulera", "Brak konfiguracji eksportu danych.");
            }
            else
            {
                DataSet dsMails = db.Select.Set(dsResend, wid, db.strParam(App.dbPORTAL));   //GetPortalDbName()));   //20170514 T
                if (dsMails != null)
                    if (Mailing.SendMail3(dsMails))    // <<<<<< tu może przygotować raport do csv i go wysłać
                        Tools.ShowMessage("Plik został wysłany.");
                    else
                        Tools.ShowError("Wystąpił błąd podczas wysyłania pliku.");
            }
            return true;
        }

        //private string GetPortalDbName()
        //{
        //    return String.Empty;
        //}

        private void Download(int wid)
        {
            DataRow dr = db.getDataRow(db.conP, String.Format("select * from RaportyWysylki where Id = {0}", wid));
            string file = db.getValue(dr, "Plik");
            string name = db.getValue(dr, "Nazwa");
            file = MapPath(file);
            Tools.DownloadFile(file, name, null);
        }
        //----------------------
        protected void gvEksportyCmd_Click(object sender, EventArgs e)
        {
            string[] p = Tools.GetLineParams(gvEksportyCmdPar.Value);
            switch (p[0])
            {
                case "resend":
                    int wid = Tools.StrToInt(p[1], -1);   // bez kontroli, [1] musi być!
                    if (wid != -1)
                        Export(wid, true);
                    else
                        Log.Error(Log.HAKER, "Ubezpieczenia.Export - Niepoprawna wartość parametru Id", p[1]);
                    break;
                case "download":
                    //wid = Tools.StrToInt(p[1], -1);   // bez kontroli, [1] musi być!
                    //if (wid != -1)
                    //    Download(wid);
                    //else
                    //    Log.Error(Log.HAKER, "Ubezpieczenia.Export.Download - Niepoprawna wartość parametru Id", p[1]);
                    Tools.ExecOnStart2("downld", String.Format("doClick('{0}');", btDownload.ClientID));  // <<< na js pozniej zmienic
                    break;
            }
        }

        protected void btExport_Click(object sender, EventArgs e)
        {
            Export(ACTUAL, true);
            gvEksporty.DataBind();
        }

        protected void btDownload_Click(object sender, EventArgs e)
        {
            string[] p = Tools.GetLineParams(gvEksportyCmdPar.Value);
            int wid = Tools.StrToInt(p[1], -1);   // bez kontroli, [1] musi być!
            if (wid != -1)
                Download(wid);
            else
                Log.Error(Log.HAKER, "Ubezpieczenia.Export.Download - Niepoprawna wartość parametru Id", p[1]);
        }
    }
}