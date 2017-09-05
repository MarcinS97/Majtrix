using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Kiosk
{
    public partial class cntPrzypiszRCP : System.Web.UI.UserControl
    {
        const string cssPoint = "point";
        const string cssPointDisabled = "point point_disabled";
        const string cssPointBlinking = "point point_blinking";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPoint(lbPoint1, Image1, 1);
                SetPoint(lbPoint2, Image2, 0);
                Tools.ExecOnStart2("blinki", "blinkPoints();");

                CheckKartaRCP();
                
                
                
                
                
                
                cntPracownicy.Prepare();
            }
        }
        //-------------------------------
        private void SetPoint(Label lbPoint, Image img, int state)
        {
            switch (state)
            {
                default:
                case 0:
                    lbPoint.CssClass = cssPointDisabled;
                    img.Visible = false;
                    break;
                case 1: 
                    lbPoint.CssClass = cssPointBlinking;
                    img.Visible = false;
                    break;
                case 2:
                    lbPoint.CssClass = cssPoint;
                    img.Visible = true;
                    break;
            }
        }

        public void SetSelected(int index)
        {
            cntPracownicy.Select(index);
        }
        //-------------------------------
        protected void btLogin_Click(object sender, EventArgs e)
        {

        }

        protected void btLogout_Click(object sender, EventArgs e)
        {

        }

        protected void cntPracownicy_SelectedChanged(object sender, EventArgs e)
        {
            bool s = cntPracownicy.List.SelectedIndex != -1;
            SetPoint(lbPoint1, Image1, s ? 2 : 1);
            SetPoint(lbPoint2, Image2, s ? 1 : 0);
            //if (s) Tools.ExecOnStart2("focusRCP", "$('#kartaRCP').focus();");
        }
        //--------------------------------
        private void CheckKartaRCP()
        {
            string kartaRCP = App._KartaRCP;
            App._KartaRCP = null;
            if (!String.IsNullOrEmpty(kartaRCP))
            {
                const string title = "Przypisywanie RCP";
                string pid = App.SelectedPracId;
                if (String.IsNullOrEmpty(pid))
                {
                    //Tools.ShowMessage("Proszę wybrać pracownika.");
                    SetPoint(lbPoint1, Image1, 1);
                    SetPoint(lbPoint2, Image2, 2);
                    cntPracownicy.FocusSearch();
                }
                else
                {                                   // 0                                   1        2     
                    //DataRow dr = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, Nr_Ewid, NrKarty from Pracownicy where Id_Pracownicy = " + pid);
                    DataRow dr = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, KadryId as Nr_Ewid, NrKarty1 as NrKarty from Pracownicy where Id = " + pid);
                    if (dr == null)
                    {
                        Tools.ShowErrorLog(Log.PRZYPISZRCP,
                            String.Format("PracId: {0}", pid),
                            "Nie znaleziono pracownika.");
                    }
                    else
                    {
                        const string fmt = "Pracownik: {0} ({1}), Id: {4}\nObecna karta RCP:{2}\nNowa karta RCP: {3}";
                        string prac = db.getValue(dr, 0);
                        string nrew = db.getValue(dr, 1);
                        string karta = db.getValue(dr, 2);
                        if (AppUser.UpdateNrKartyRCP(pid, kartaRCP))
                        {
                            Log.Info(Log.PRZYPISZRCP, title,
                                String.Format(fmt, prac, nrew, karta, kartaRCP, pid));
                            ShowOk();
                        }
                        else
                        {
                            Log.Error(Log.PRZYPISZRCP, title + " - błąd podczas zapisu",
                                String.Format(fmt, prac, nrew, karta, kartaRCP, pid));
                            ShowError();
                        }
                    }
                    kartaRCP = null;
                }
            }
            Tools.ExecOnStart2("setKartaRCP", String.Format("$('#kartaRCPprz').val('{0}');", kartaRCP));
        }

        private void ShowOk()
        {
            Tools.ExecOnStart2("showOk", String.Format("showInfo('{0}','{1}');", lbPoint2.ClientID, lbInfo.ClientID));
        }

        private void ShowError()
        {
            Tools.ExecOnStart2("showError", String.Format("showInfo('{0}','{1}');", lbPoint2.ClientID, lbError.ClientID));
        }

    }
}