using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRRcp.App_Code;


// UWAGA !!!
// cntMailsAdm <- nowa kontrolka !!!


namespace HRRcp.Controls
{
    public partial class AdmMailsControl : System.Web.UI.UserControl
    {
        Ustawienia settings;
        protected void Page_Load(object sender, EventArgs e)
        {
            settings = Ustawienia.CreateOrGetSession();
            if (!IsPostBack)
            {
                FillData();
                SetEditMode(false);
                //Mailing.CheckUpdate();
            }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            FillData();   // odczytuje wartosci z bazy na wypadek gdyby byly przez kogos zmienione
            SetEditMode(true);
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (CheckAndUpdate())
                SetEditMode(false);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            FillData();   //odczytujemy warosci z bazy - przywracamy edity
        }

        protected void btGetAppAddr_Click(object sender, EventArgs e)
        {
            tbAppAddr.Text = Tools.GetAppAddr2(null);
        }
        //-----------------------------------------------
        private void FillData()
        {
            //tbSMTP.Text = settings.SMTPSerwer;    //20150105
            tbSMTP.Text = settings.SMTPData;

            tbEmail.Text = settings.Email;
            tbMonitIlDni.Text = settings.MonitDniPrzed.ToString();
            tbAppAddr.Text = settings.AppAddr;
            Tools.MakeConfirmButton(btGetAppAddr, "Potwierdź pobranie adresu www aplikacji.");
        }

        private void SetEditMode(bool fEdit)
        {
            Tools.SetEdit(tbSMTP, fEdit);
            Tools.SetEdit(tbEmail, fEdit);
            Tools.SetEdit(tbMonitIlDni, fEdit);
            Tools.SetEdit(tbAppAddr, fEdit);
            btGetAppAddr.Enabled = fEdit;
            btCancel.Visible = fEdit;
            btSave.Visible = fEdit;
            btEdit.Visible = !fEdit;
        }

        private void clearErrorMarkers()
        {
            //Tools.SetErrorMarker(lbColCzasZast, false);
        }

        private bool CheckAndUpdate()
        {
            clearErrorMarkers();
            int err = settings.Update(tbSMTP.Text, tbEmail.Text, tbAppAddr.Text, tbMonitIlDni.Text); // walidacja wartości w środku !!!
            switch (err)
            {
                case 0: return true;  // ok!
                case -2:
                    Tools.ShowMessage("Niepoprawna wartość ilości dni.");
                    break;
                default:
                    Tools.ShowMessage("Błąd podczas zapisu do bazy.");
                    break;
            }
            return false;
        }
        //------------------------------------
        protected void lvMails_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Send")
            {
                string typ_grupa = e.CommandArgument.ToString();
                if (!String.IsNullOrEmpty(typ_grupa))
                {
                    AppUser user = AppUser.CreateOrGetSession();
                    string typ;
                    string grupa;
                    DataSet maildata;
                    Tools.GetLineParams(typ_grupa, out typ, out grupa);
                    switch (grupa.Trim())  // poniewaz to jest char to dokłada ' ' 
                    {   
                        /*
                        case Mailing.grPROGRAM:
                            maildata = Mailing.GetProgramData(true);
                            break;
                        case Mailing.grANKIETA:
                            maildata = Mailing.GetAnkietaData(null, user.Id, -1, true);
                            break;
                        case Mailing.grZAST:
                            maildata = Mailing.GetZastData(user.Id, null, null, true);
                            break;
                         */
                        case Mailing.grOKRES:
                            maildata = Mailing.GetOkresData(Okres.Current(db.con), true);
                            break;
                        case Mailing.grZASTEPSTWA:
                            maildata = Mailing.GetZastData(user.Id, null, null, null, Mailing.zaStart, true);
                            break;
                        case Mailing.grPRZESUNIECIA:
                            maildata = Mailing.GetPrzesData(null, true);
                            break;
                        case Mailing.grWNIOSKIURLOPOWE:
                            maildata = Mailing.GetWniosekUrlopowyData(null, true);
                            break;



                        case Mailing.grSYSTEM:
                            maildata = Mailing.GetPassResetData(null, App.User.NazwiskoImie, App.User.NR_EW, App.User.Nick, "Password");
                            break;
                        
                        
                        default:
                            maildata = null;
                            break;
                    }
                    /*
                    if (Mailing.SendMail(typ, user.Id, maildata))
                        Tools.ShowMessage("Mail do: " + user.EMail + " zostal wysłany poprawnie.");
                    else
                        Tools.ShowMessage("Nie udało się wysłać maila do: " + user.EMail + ".");
                     */

                    if (Mailing.SendMail(typ, null, user.Id, maildata))
                        Tools.ShowMessage("Mail do: " + user.EMail + " zostal wysłany poprawnie.");
                    else
                        if (String.IsNullOrEmpty(user.EMail))
                            Tools.ShowMessage(String.Format("Brak adresu e-mail u użytkownika: {0}.", user.ImieNazwisko));
                        else
                            Tools.ShowMessage(String.Format("Nie udało się wysłać maila do: {0}.", user.EMail));
 
                }
            }
        }

        //-----------------------------------------------------------------------
        private void xxxaddZnacznik(PlaceHolder ph, string defaultTextBoxId, string znacznik, string opis)
        {
            Label z = new Label();
            z.Text = "%" + znacznik + "% - " + opis;
            //z.Attributes.Add("onclick", "selectZnaczniki('" + defaultTextBoxId + "','%" + znacznik + "% ');");
            z.Attributes.Add("onclick", "insertZnacznik('" + defaultTextBoxId + "','%" + znacznik + "% ');");
            z.CssClass = "znacznik";
            Tools.AddControl(ph, null, z, "<br />");
            //ph.Controls.Add(z);
        }

        private void xxxFillZnaczniki(ListViewItem Item, string grupa)
        {
            /*
            PlaceHolder ph = (PlaceHolder)Item.FindControl("phZnaczniki");
            if (ph != null)
            {
                TextBox tb = (TextBox)Item.FindControl("TrescTextBox");
                string tbID = tb != null ? tb.ClientID : "";

                switch (grupa)
                {
                    case Mailing.grPROGRAM:
                        addZnacznik(ph, tbID, Mailing.idSTART, "Data rozpoczęcia programu");
                        addZnacznik(ph, tbID, Mailing.idSTOP, "Data zakończenia programu");
                        addZnacznik(ph, tbID, Mailing.idTERMIN, "Termin wypełnienia ankiety przez pracownika");
                        addZnacznik(ph, tbID, Mailing.idLINK_PRP, "Link do aplikacji");

                        addZnacznik(ph, tbID, Mailing.idDniP, "Ilość dni na wypełnienie ankiety przez pracownika");
                        addZnacznik(ph, tbID, Mailing.idDataP, "Termin wypełniania ankiety przez pracownika");
                        addZnacznik(ph, tbID, Mailing.idDniK, "Ilość dni na wypełnienie ankiety przez kierownika");
                        addZnacznik(ph, tbID, Mailing.idDataK, "Termin wypełniania ankiety przez kierownika");
                        addZnacznik(ph, tbID, Mailing.idDniPK, "Ilość dni na spotkanie kierownika z pracownikiem");
                        addZnacznik(ph, tbID, Mailing.idDataPK, "Termin przeprowadzenia spotkania kierownika z pracownikiem");
                        addZnacznik(ph, tbID, Mailing.idDniA, "Ilość dni na akceptacje ankiety przez pracownika");
                        addZnacznik(ph, tbID, Mailing.idDataA, "Termin akceptacji ankiety przez pracownika");
                        addZnacznik(ph, tbID, Mailing.idDniSK, "Ilość dni na wypełnienie ścieżki kariery pracownika przez kierownika");
                        addZnacznik(ph, tbID, Mailing.idDataSK, "Termin wypełnienia ścieżki kariery pracownika przez kierownika");
                        break;
                    case Mailing.grANKIETA:
                        addZnacznik(ph, tbID, Mailing.idPRACOWNIK, "Imię i nazwisko pracownika");
                        addZnacznik(ph, tbID, Mailing.idKIEROWNIK, "Imię i nazwisko kierownika");
                        addZnacznik(ph, tbID, Mailing.idDATA, "Data spotkania");
                        addZnacznik(ph, tbID, Mailing.idTERMIN, "Termin wypełnienia/akceptacji ankiety przez pracownika lub kierownika");
                        addZnacznik(ph, tbID, Mailing.idPOWOD, "Powód zamknięcia ankiety");
                        addZnacznik(ph, tbID, Mailing.idSTART, "Data rozpoczęcia programu");
                        addZnacznik(ph, tbID, Mailing.idSTOP, "Data zakończenia programu");
                        addZnacznik(ph, tbID, Mailing.idLINK_PRP, "Link do aplikacji");
                        addZnacznik(ph, tbID, Mailing.idLINK_ANKIETA, "Link do ankiety pracownika");
                        addZnacznik(ph, tbID, Mailing.idLINK_KIEROWNIK, "Link do panelu kierownika");
                        break;
                    case Mailing.grZAST:
                        addZnacznik(ph, tbID, Mailing.idZASTEPOWANY, "Imię i nazwisko zastępowanego kierownika");
                        addZnacznik(ph, tbID, Mailing.idZASTEPUJACY, "Imię i nazwisko kierownika, który zastępuje");
                        addZnacznik(ph, tbID, Mailing.idDATA, "Data obowiązywania zastępstwa");
                        addZnacznik(ph, tbID, Mailing.idLINK_PRP, "Link do aplikacji");
                        addZnacznik(ph, tbID, Mailing.idLINK_KIEROWNIK, "Link do panelu kierownika");
                        //addZnacznik(ph, tbID, Mailing., "");
                        break;
                    default:
                        HtmlTableRow row = (HtmlTableRow)Item.FindControl("rowZnaczniki");
                        if (row != null)
                            row.Visible = false;
                        break;
                }
            }
             */
        }



        /*
        protected void lvMails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (lvMails.EditIndex > -1)
            {
                ListViewDataItem ditem = (ListViewDataItem)e.Item;
                if (ditem.DataItemIndex == lvMails.EditIndex)
                {
                    DataRowView rowView = (DataRowView)ditem.DataItem;
                    string grupa = rowView["Grupa"].ToString().Trim();
                    FillZnaczniki(e.Item, grupa);
                }
            }
        }
         */

        protected void lvMails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem && lvMails.EditIndex != -1)
            {
                ListViewDataItem ditem = (ListViewDataItem)e.Item;
                if (ditem.DataItemIndex == lvMails.EditIndex)
                {
                    Tools.SetControlVisible(e.Item, "rowZnaczniki", true);
                    DataRowView rv = (DataRowView)ditem.DataItem;
                    string grupa = rv["Grupa"].ToString().Trim();
                    Mailing.FillZnaczniki(e.Item, grupa);
                }
            }
        }

    }
}