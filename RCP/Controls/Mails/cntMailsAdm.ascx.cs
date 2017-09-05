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
using HRRcp.Controls;
using HRRcp.Controls.Mails;

namespace HRRcp.Controls
{
    public partial class cntMailsAdm : System.Web.UI.UserControl
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

#if PORTAL || IPO
                trOkres.Visible = false;
#endif
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
            FillData();   //odczytuje warosci z bazy - przywracamy edity
        }
        //--------        
        protected void btGetAppAddr_Click(object sender, EventArgs e)
        {
            //tbAppAddr.Text = Tools.GetAppAddr2(null);
            tbAppAddr.Text = Tools.GetHostAppAddr();
        }
        //-----------------------------------------------
        private void FillData()
        {
            //tbSMTP.Text = settings.SMTPSerwer;    //20150105
            var SMPTtemp = settings.SMTPData;
            string[] SMPTtempSplit = SMPTtemp.Split(';');

            tbSMTP1.Text = SMPTtempSplit[0];
            tbSMTP2.Text = SMPTtempSplit[1];
            tbSMTP3.Text = SMPTtempSplit[2];
           


            tbEmail.Text = settings.Email;
            tbAppAddr.Text = settings.AppAddr;

            tbMonitIlDni.Text = settings.MonitDniPrzed.ToString();
            Tools.MakeConfirmButton(btGetAppAddr, "Potwierdź pobranie adresu www aplikacji.");
        }

        private void SetEditMode(bool fEdit)
        {
            Tools.SetEdit(tbSMTP1, fEdit);
            Tools.SetEdit(tbSMTP2, fEdit);
            Tools.SetEdit(tbSMTP3, fEdit);

            Tools.SetEdit(tbEmail, fEdit);
            Tools.SetEdit(tbAppAddr, fEdit);
            Tools.SetEdit(tbMonitIlDni, fEdit);
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
            string smtpTEMP = tbSMTP1.Text+";" + tbSMTP2.Text + ";" + tbSMTP3.Text;
            int err = settings.Update(smtpTEMP, tbEmail.Text, tbAppAddr.Text, tbMonitIlDni.Text); // walidacja wartości w środku !!!
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
                    switch (grupa.Trim())  // poniewaz to jest char to dokłada ' ' <<< był 
                    {
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
                        case Mailing.grPLANURLOPOW:
                            maildata = Mailing.GetPlanUrlopowData(null, true);
                            break;
                        case Mailing.grSYSTEM:
                            maildata = Mailing.GetPassResetData(null, App.User.NazwiskoImie, App.User.NR_EW, App.User.Nick, "Password");
                            break;
                        case Mailing.grPRACAZDALNA:
                            maildata = Mailing.GetWniosekUrlopowyData(null, true);
                            break;
#if SCARDS
                        case Mailing.grSCARDS:
                            maildata = Scorecards.App_Code.Mailing.GetWniosekData(null, null, true);
                            break;
#endif
                        default:
                            maildata = null;
                            break;
                    }
                    if (App.User.IsMailing && !String.IsNullOrEmpty(user.EMail))
                        if (Mailing.SendMail(typ, null, user.Id, maildata))
                            Tools.ShowMessage("Wiadomość do: " + user.EMail + " została wysłana poprawnie." +
                                             (!App.IsMailing ? "\\n*** wysyłka maili wyłączona ***" : null));
                        else Tools.ShowError(String.Format("Nie udało się wysłać wiadomości do: {0}.", user.EMail));
                    else Tools.ShowError(String.Format("Brak adresu e-mail lub opcja powiadomień jest wyłączona u użytkownika: {0}.", user.ImieNazwisko));
                }
            }
        }

        //-----------------------------
        //cntMailsZnaczniki cntZnaczniki = null;
 
        protected void lvMails_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            /*
            if (e.Item.ItemType == ListViewItemType.DataItem && lvMails.EditIndex != -1)
                if (((ListViewDataItem)e.Item).DataItemIndex == lvMails.EditIndex)
                {
                    bool su = IsSuperuser;
                    if (su)
                    {
                        
                        TextBox tbGrupa = e.Item.FindControl("tbGrupa") as TextBox;
                        TextBox tbTresc = e.Item.FindControl("TrescTextBox") as TextBox;
                        cntMailsZnaczniki zn = e.Item.FindControl("cntMailsZnaczniki1") as Mails.cntMailsZnaczniki;
                        
                        if (tbGrupa != null && tbTresc != null && cntZnaczniki != null)
                        {
                            cntZnaczniki.tbGrupa = tbGrupa;
                            //cntZnaczniki.MailTextBoxId = tbTresc.ClientID;

                            //string cid = tbTresc.ClientID;
                            //cntZnaczniki.tbMail = tbTresc;
                        }
                    }
                }
             */ 
        }

        protected void lvMails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            int lim = Tools.GetListItemMode(e, lvMails);
            switch (lim)
            {
                case Tools.limSelect:
                    break;
                case Tools.limEdit:
                    bool su = IsSuperuser;
                    Tools.SetControlVisible(e.Item, "rowZnaczniki", true);
                    Tools.SetControlVisible(e.Item, "trOpis", su);
                    Tools.SetControlVisible(e.Item, "trTyp", su);
                    Tools.SetControlVisible(e.Item, "trGrupa", su);
                    if (su)
                    {
                        Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                    }
                    else
                    {
                        DataRowView drv = Tools.GetDataRowView(e);
                        string grupa = drv["Grupa"].ToString().Trim();
                        Mailing.FillZnaczniki(e.Item, grupa);
                    }
                    break;
                case Tools.limInsert:
                    break;
            }
        }

        protected void lvMails_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.NewValues["Opis"] = Tools.GetText(lvMails.EditItem, "tbOpis");
            e.NewValues["Typ"] = Tools.GetText(lvMails.EditItem, "tbTyp");
            string grp = Tools.GetText(lvMails.EditItem, "tbGrupa");
            e.NewValues["Grupa"] = grp;
            DataRow dr = db.getDataRow("select * from MailingGrupy where Grupa = " + db.strParam(grp));
            if (dr == null)
                db.insert("MailingGrupy", 0, "Grupa", db.strParam(grp));
        }

        protected void tbGrupa_TextChanged(object sender, EventArgs e)
        {
            cntMailsZnaczniki zn = GetMailsZnaczniki(lvMails.EditItem);
            if (zn != null)
                zn.Prepare(((TextBox)sender).Text);
        }

        protected void ddlGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox tb = lvMails.EditItem.FindControl("tbGrupa") as TextBox;
            cntMailsZnaczniki zn = GetMailsZnaczniki(lvMails.EditItem);
            if (tb != null && zn != null)
            {
                string grp = ((DropDownList)sender).SelectedValue;
                tb.Text = grp;
                zn.Prepare(grp);
            }
        }

        protected void ddlGrupaInsert_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox tb = lvMails.InsertItem.FindControl("tbGrupa") as TextBox;
            cntMailsZnaczniki zn = GetMailsZnaczniki(lvMails.InsertItem);
            if (tb != null && zn != null)
            {
                string grp = ((DropDownList)sender).SelectedValue;
                tb.Text = grp;
                zn.Prepare(grp);
            }
        }
        
        cntMailsZnaczniki GetMailsZnaczniki(ListViewItem item)
        {
            if (item != null)
                return item.FindControl("cntMailsZnaczniki1") as Mails.cntMailsZnaczniki;
            else
                return null;
        }

        protected void lvMails_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }

        protected void lvMails_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }
        //---------------------------
        public bool IsSuperuser
        {
            get { return App.User.IsSuperuser; }
        }

        public InsertItemPosition InsertItemPosition
        {
            get { return IsSuperuser ? InsertItemPosition.LastItem : InsertItemPosition.None; }
        }
    }
}