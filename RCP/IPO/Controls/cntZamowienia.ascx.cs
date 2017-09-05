using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.ComponentModel;
using System.Data;
using HRRcp.Controls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using AjaxControlToolkit;
using HRRcp.IPO.App_Code;

namespace HRRcp.IPO.Controls
{
    public partial class cntZamowienia : System.Web.UI.UserControl
    {
        public string Zakladka { get; set; }
        public string Rola { get; set; }
        public bool Dodawanie { get; set; }
        public bool Edycja { get; set; }
        public bool Wysylanie { get; set; }
        public bool Akceptacja { get; set; }
        AppUser user;

        const string PlikiPath = @"~/IPO/pliki/";
        const string PlikiPathRel = @"pliki/";
        public string dataSourceId {
            get{
                return Tools.GetStr(ViewState["dsId"]);
            }
            set{
                ViewState["dsId"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(zamowieniaListView, 0);
            Tools.PrepareSorting(zamowieniaListView, 1, 10);

            zamowieniaListView.ItemCreated += new EventHandler<ListViewItemEventArgs>(utworzZamowienieButtonChanger);

            Tools.PrepareDicListView(pozycjeListView, 0);
            Tools.PrepareSorting(pozycjeListView, 1, 10);

            Tools.PrepareDicListView(szczegolyListView, 0);
            Tools.PrepareSorting(szczegolyListView, 1, 10);

            Tools.PrepareDicListView(sciezkaAkceptacjiListView, 0);
            Tools.PrepareSorting(sciezkaAkceptacjiListView, 1, 10);

            Tools.PrepareDicListView(staraSciezkaAkceptacjiListView, 0);
            Tools.PrepareSorting(staraSciezkaAkceptacjiListView, 1, 10);

            Tools.PrepareDicListView(nowaSciezkaAkceptacjiListView, 0);
            Tools.PrepareSorting(nowaSciezkaAkceptacjiListView, 1, 10);
        }

        protected static void utworzZamowienieButtonChanger(object sender, ListViewItemEventArgs e)
        {
            Button InsertButton = Tools.FindButton(e.Item, "InsertButton");
            if(InsertButton != null)
                InsertButton.Text = "Utwórz zamówienie";
        }

        protected void Page_Load(object sender, EventArgs e)
        {


                user = AppUser.CreateOrGetSession();
                CCDataSource.SelectParameters["UserId"].DefaultValue = user.Id;
                CCOptDataSource.SelectParameters["UserId"].DefaultValue = user.Id;
                pozycjeDataSource.SelectParameters["UserId"].DefaultValue = user.Id;
                ustawWidocznoscSzczegolowZamowienia();
            
        }

        public void refresh()
        {
            zamowieniaListView.DataBind();
            if (zamowieniaListView.Items.Count > 0)
            {
                zamowieniaListView.SelectedIndex = 0;
            }
            szczegolyListView.DataBind();
            pozycjeListView.DataBind();
            sciezkaAkceptacjiListView.DataBind();
            nowaSciezkaAkceptacjiListView.DataBind();
            staraSciezkaAkceptacjiListView.DataBind();            
        }

        public bool CEREditVisible(bool CER, int PoziomAkceptacji)
        {
            return "1".Equals(Zakladka) && "2".Equals(Rola) && CER && PoziomAkceptacji == 3;
        }

        public bool DodawanieVisible()
        {
            return Dodawanie;
        }

        public bool EdycjaVisible()
        {
            return Edycja;
        }

        public bool WysylanieVisible()
        {
            return Wysylanie;
        }

        public bool AkceptacjaVisible()
        {
            return Akceptacja;
        }

        public bool RejectVisible(int odrzuc)
        {
            return Akceptacja && odrzuc == 1;
        }

        public bool RevertVisible(int przywroc)
        {
            return Akceptacja && przywroc == 1;
        }

        public bool DrukujVisible()
        {
            return "3".Equals(Zakladka) && "3".Equals(Rola);
        }

        public bool ZamowionoVisible(int status)
        {
            return "3".Equals(Zakladka) && "3".Equals(Rola) && status == 3;
        }

        public bool DostarczonoVisible(int status)
        {
            return "3".Equals(Zakladka) && "3".Equals(Rola) && status == 4;
        }

        public bool OdebranoVisible(int status)
        {
            return "4".Equals(Zakladka) && "3".Equals(Rola) && status == 5;
        }

        public bool EdycjaSciezkiAkceptacjiVisible()
        {
            return "5".Equals(Rola);
        }

        public bool EdycjaStatusuPozycjiVisible()
        {
            return "5".Equals(Rola);
        }

        public bool ZapiszProduktVisible(object produktId)
        {
            return "3".Equals(Rola) && (produktId == System.DBNull.Value);
        }

        public string GetCssPozycji(int status)
        {
            if ("6".Equals(Zakladka))
            {
                if (status == 7)
                {
                    return "it black_row";
                }
                else
                {
                    return "it gray_row";
                }

            }
            else
            {
                if (status == 7)
                {
                    return "it gray_row";
                }
                else
                {
                    return "it black_row";
                }
            }
        }

        // zamawiajacy
        private void rola1(AppUser user)
        {
            switch (Zakladka)
            {
                case "1":
                    
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status"].DefaultValue = "1";
                    zamowieniaDataSource.SelectCommand = zamawiajacyDataSource.SelectCommand;
                    break;
                case "2":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status"].DefaultValue = "2";
                    zamowieniaDataSource.SelectCommand = zamawiajacyDataSource.SelectCommand;
                    break;
                case "3":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "3";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "4";
                    zamowieniaDataSource.SelectCommand = zamawiajacyZODataSource.SelectCommand;
                    break;
                case "4":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "5";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "5";
                    zamowieniaDataSource.SelectCommand = zamawiajacyZODataSource.SelectCommand;
                    break;
                case "5":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "6";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "6";
                    zamowieniaDataSource.SelectCommand = zamawiajacyZODataSource.SelectCommand;
                    break;
                case "6":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "7";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "7";
                    zamowieniaDataSource.SelectCommand = zamawiajacyZODataSource.SelectCommand;
                    break;
            }
        }

        // akceptujacy
        private void rola2(AppUser user)
        {
            switch (Zakladka)
            {
                case "1":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status"].DefaultValue = "2";
                    zamowieniaDataSource.SelectCommand = akceptujacy1DataSource.SelectCommand;
                    break;
                case "2":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectCommand = akceptujacy2DataSource.SelectCommand;
                    break;
                case "3":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status"].DefaultValue = "6";
                    zamowieniaDataSource.SelectCommand = akceptujacyZOSqlDataSource.SelectCommand;
                    break;
                case "4":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status"].DefaultValue = "7";
                    zamowieniaDataSource.SelectCommand = akceptujacyZOSqlDataSource.SelectCommand;
                    break;
            }
        }

        // kupiec
        private void rola3(AppUser user)
        {
            switch (Zakladka)
            {
                case "1":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "1";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "1";
                    zamowieniaDataSource.SelectCommand = kupiecDataSource.SelectCommand;
                    break;
                case "2":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "2";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "0";
                    zamowieniaDataSource.SelectCommand = kupiecDataSource.SelectCommand;
                    break;
                case "3":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "3";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "4";
                    zamowieniaDataSource.SelectParameters["Status3"].DefaultValue = "0";
                    zamowieniaDataSource.SelectCommand = kupiecZODataSource.SelectCommand;
                    break;
                case "4":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "5";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "5";
                    zamowieniaDataSource.SelectParameters["Status3"].DefaultValue = "5";
                    zamowieniaDataSource.SelectCommand = kupiecZODataSource.SelectCommand;
                    break;
                case "5":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "6";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "6";
                    zamowieniaDataSource.SelectParameters["Status3"].DefaultValue = "6";
                    zamowieniaDataSource.SelectCommand = kupiecZODataSource.SelectCommand;
                    break;
                case "6":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "7";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "7";
                    zamowieniaDataSource.SelectParameters["Status3"].DefaultValue = "7";
                    zamowieniaDataSource.SelectCommand = kupiecZODataSource.SelectCommand;
                    break;
            }
        }

        // notyfikowany
        private void rola4(AppUser user)
        {
            switch (Zakladka)
            {
                case "1":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status"].DefaultValue = "2";
                    zamowieniaDataSource.SelectCommand = notyfikowanyDataSource.SelectCommand;
                    break;
                case "2":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "3";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "4";
                    zamowieniaDataSource.SelectCommand = notyfikowanyZODataSource.SelectCommand;
                    break;
                case "3":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "5";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "5";
                    zamowieniaDataSource.SelectCommand = notyfikowanyZODataSource.SelectCommand;
                    break;
                case "4":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "6";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "6";
                    zamowieniaDataSource.SelectCommand = notyfikowanyZODataSource.SelectCommand;
                    break;
                case "5":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "7";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "7";
                    zamowieniaDataSource.SelectCommand = notyfikowanyZODataSource.SelectCommand;
                    break;
            }
        }

        // administrator
        private void rola5(AppUser user)
        {
            switch (Zakladka)
            {
                case "1":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "1";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "1";
                    zamowieniaDataSource.SelectCommand = administratorDataSource.SelectCommand;
                    break;
                case "2":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "2";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "2";
                    zamowieniaDataSource.SelectCommand = administratorDataSource.SelectCommand;
                    break;
                case "3":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "3";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "3";
                    zamowieniaDataSource.SelectCommand = administratorZODataSource.SelectCommand;
                    break;
                case "4":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "4";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "4";
                    zamowieniaDataSource.SelectCommand = administratorZODataSource.SelectCommand;
                    break;
                case "5":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "5";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "5";
                    zamowieniaDataSource.SelectCommand = administratorZODataSource.SelectCommand;
                    break;
                case "6":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "6";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "6";
                    zamowieniaDataSource.SelectCommand = administratorZODataSource.SelectCommand;
                    break;
                case "7":
                    zamowieniaDataSource.SelectParameters["IdPracownika"].DefaultValue = user.Id;
                    zamowieniaDataSource.SelectParameters["Status1"].DefaultValue = "7";
                    zamowieniaDataSource.SelectParameters["Status2"].DefaultValue = "7";
                    zamowieniaDataSource.SelectCommand = administratorZODataSource.SelectCommand;
                    break;
            }
        }

        private void ustawWidocznoscSzczegolowZamowienia()
        {
            szczegolyListView.Visible = zamowieniaListView.SelectedDataKey != null;
            pozycjeListView.Visible = zamowieniaListView.SelectedDataKey != null;
            sciezkaAkceptacjiListView.Visible = zamowieniaListView.SelectedDataKey != null && !WyborSciezki();
            staraSciezkaAkceptacjiListView.Visible = zamowieniaListView.SelectedDataKey != null;
            nowaSciezkaAkceptacjiListView.Visible = zamowieniaListView.SelectedDataKey != null;
        }

        protected void zamowieniaListView_DataBound(object sender, EventArgs e)
        {
            ustawWidocznoscSzczegolowZamowienia();
        }

        protected void zamowieniaListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ustawWidocznoscSzczegolowZamowienia();
        }
        bool checkboxState = false;
        protected void CERCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox CERCheckBox = zamowieniaListView.InsertItem.FindControl("CERCheckBox") as CheckBox;
            checkboxState = CERCheckBox.Checked;
        }
        protected void CEREdit_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox CERCheckBox = szczegolyListView.EditItem.FindControl("CERCheckBox") as CheckBox;
            Tools.SetControlEnabled(szczegolyListView.EditItem, "CERTextBox", CERCheckBox.Checked);
            Tools.SetControlEnabled(szczegolyListView.EditItem, "CERValidator", CERCheckBox.Checked);  
        }

        protected void zamowieniaListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "NewRecord":
                    zamowieniaListView.InsertItemPosition = InsertItemPosition.FirstItem;
                    break;
                case "CancelInsert":
                    zamowieniaListView.InsertItemPosition = InsertItemPosition.None;
                    break;
            }
        }

        protected void szczegolyListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Send":
                    wyslij();
                    break;
                case "Accept":
                    akceptuj();
                    break;
                case "Reject":
                    odrzuc();
                    break;
                case "Edit":
                    Tools.SetControlEnabled(zamowieniaListView, "btNewRecord", false);
                    break;
                case "Update":
                case "Cancel":
                    Tools.SetControlEnabled(zamowieniaListView, "btNewRecord", true);
                    break;
            }
        }
        protected void szczegolyListView_OnItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            
            cntSplityCC splity = szczegolyListView.EditItem.FindControl("cntSplityCC") as cntSplityCC;
            if (splity != null)
            {
                bool v = splity.Validate();
                if (!v)
                {
                    Tools.ShowError("Suma udziałów w CC musi być równa 1");
                    e.Cancel = true;
                }
                else
                {
                    splity.Update();
                }
            }            
        }
        
        protected void zamowieniaListView_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            AppUser user = AppUser.CreateOrGetSession();
            zamowieniaDataSource.InsertParameters["Pracownik"].DefaultValue = user.Id;

            TextBox KlientTextBox = zamowieniaListView.InsertItem.FindControl("KlientTextBox") as TextBox;
            zamowieniaDataSource.InsertParameters["Klient"].DefaultValue = KlientTextBox.Text;

            DropDownList MagazynDropDownList = zamowieniaListView.InsertItem.FindControl("MagazynDropDownList") as DropDownList;
            zamowieniaDataSource.InsertParameters["Magazyn"].DefaultValue = MagazynDropDownList.SelectedValue;

            CheckBox CERCheckBox = zamowieniaListView.InsertItem.FindControl("CERCheckBox") as CheckBox;
            zamowieniaDataSource.InsertParameters["CER"].DefaultValue = "" + CERCheckBox.Checked;

            if (CERCheckBox.Checked)
            {
                TextBox CERNrTextBox = zamowieniaListView.InsertItem.FindControl("CERNrTextBox") as TextBox;
                zamowieniaDataSource.InsertParameters["CERNr"].DefaultValue = CERNrTextBox.Text;
            }

            zamowieniaDataSource.InsertParameters["Status"].DefaultValue = "" + 1;

            cntSplityCC splity = zamowieniaListView.InsertItem.FindControl("cntSplityCC") as cntSplityCC;
                if (splity != null)
                {
                    bool v = splity.Validate();
                    if (!v)
                    {
                        Tools.ShowError("Suma udziałów w CC musi być równa 1");
                        e.Cancel = true;
                    }
                }
     
        
        }

        protected void zamowieniaListView_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            if (zamowieniaListView.Items.Count > 0)
            {
                zamowieniaListView.SelectedIndex = 0;
            }
            else
            {
                zamowieniaListView.SelectedIndex = -1;
            }
            zamowieniaListView.InsertItemPosition = InsertItemPosition.None;
            pozycjeListView.EditIndex = -1;
            pozycjeListView.InsertItemPosition = InsertItemPosition.FirstItem;
        }

        protected void pozycjeListView_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            pozycjeListView.InsertItemPosition = InsertItemPosition.None;
        }

        protected void pozycjeListView_OnItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {

            sciezkaAkceptacjiListView.DataBind();
        }

        protected void pozycjeListView_OnItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            // ----------------------------------------------------- TODO
//            try
//            {
//                TextBox CenaTextBox = pozycjeListView.EditItem.FindControl("CenaTextBox") as TextBox;
//                TextBox IloscTextBox = pozycjeListView.EditItem.FindControl("IloscTextBox") as TextBox;
//                DropDownList WalutaDropdownList = pozycjeListView.EditItem.FindControl("WalutaDropDownList") as DropDownList;
//                IPO_db.getScalar(@"Select count(1) from IPO_PozycjeZamowien 
//            }
//            catch(Exception ex)
//            {
//            }

        }

        protected void sciezkaAkceptacjiListView_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            sciezkaAkceptacjiListView.InsertItemPosition = InsertItemPosition.None;
        }

        protected void szczegolyListView_OnItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            zamowieniaListView.SelectedIndex = -1;
            zamowieniaListView.InsertItemPosition = InsertItemPosition.None;
        }

        protected void szczegolyListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Button DeleteButton = (Button)e.Item.FindControl("DeleteButton");
            if (DeleteButton != null)
                Tools.MakeConfirmButton(DeleteButton, "Potwierdzasz usunięcie zamówienia ?");
            Button WyslijButton = (Button)e.Item.FindControl("WyslijButton");
            if (WyslijButton != null)
                Tools.MakeConfirmButton(WyslijButton, "Potwierdzasz wysłanie zamówienia ?");
            Button AcceptButton = (Button)e.Item.FindControl("AcceptButton");
            if (AcceptButton != null)
                Tools.MakeConfirmButton(AcceptButton, "Potwierdzasz akceptacje zamówienia ?");
            Button RejectButton = (Button)e.Item.FindControl("RejectButton");
            if (RejectButton != null)
                Tools.MakeConfirmButton(RejectButton, "Potwierdzasz odrzucenie zamówienia ?");
        }
        protected void sciezkaAkceptacjiListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }
        
        protected void pozycjeListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Button DeleteButton = (Button)e.Item.FindControl("DeleteButton");
            if (DeleteButton != null)
                Tools.MakeConfirmButton(DeleteButton, "Potwierdzasz usunięcie pozycji zamówienia ?");
            Button RejectButton = (Button)e.Item.FindControl("RejectButton");
            if (RejectButton != null)
                Tools.MakeConfirmButton(RejectButton, "Potwierdzasz odrzucenie pozycji zamówienia ?");
            Button RevertButton = (Button)e.Item.FindControl("RevertButton");
            if (RevertButton != null)
                Tools.MakeConfirmButton(RevertButton, "Potwierdzasz przywrócenie pozycji zamówienia ?");
        }

        protected void pozycjeListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    pozycjeListView.InsertItemPosition = InsertItemPosition.None;
                    Tools.SetControlEnabled(zamowieniaListView, "btNewRecord", false);
                    Tools.SetControlEnabled(pozycjeListView, "btNewRecord", false);
                    break;
                case "Update":
                case "Cancel":
                    Tools.SetControlEnabled(zamowieniaListView, "btNewRecord", true);
                    Tools.SetControlEnabled(pozycjeListView, "btNewRecord", true);
                    break;
                case "NewRecord":
                    pozycjeListView.EditIndex = -1;
                    pozycjeListView.InsertItemPosition = InsertItemPosition.FirstItem;
                    Tools.SetControlEnabled(zamowieniaListView, "btNewRecord", false);
                    Tools.SetControlEnabled(pozycjeListView, "btNewRecord", false);
                    break;
                case "CancelInsert":
                    pozycjeListView.InsertItemPosition = InsertItemPosition.None;
                    Tools.SetControlEnabled(zamowieniaListView, "btNewRecord", true);
                    Tools.SetControlEnabled(pozycjeListView, "btNewRecord", true);
                    break;
                case "Insert":
                    Tools.SetControlEnabled(zamowieniaListView, "btNewRecord", true);
                    Tools.SetControlEnabled(pozycjeListView, "btNewRecord", true);
                    break;
                case "Reject":
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        odrzucPozycje("" + pozycjeListView.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value);
                        pozycjeListView.DataBind();
                        ustawWidocznoscSzczegolowZamowienia();
                    }
                    break;
                case "Revert":
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        przywrocPozycje("" + pozycjeListView.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value);
                        pozycjeListView.DataBind();
                        ustawWidocznoscSzczegolowZamowienia();
                    }
                    break;
                case "Zamowiono":
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        DateEdit DataDostawyDateEdit = e.Item.FindControl("DataDostawyDateEdit") as DateEdit;
                        if (DataDostawyDateEdit != null && DataDostawyDateEdit.Date != null)
                            zamowiono("" + pozycjeListView.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value, (DateTime)DataDostawyDateEdit.Date);
                        else
                            zamowiono("" + pozycjeListView.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value);
                        pozycjeListView.DataBind();
                        ustawWidocznoscSzczegolowZamowienia();
                    }
                    break;
                case "Dostarczono":
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        TextBox LokalizacjaOdbioruTextBox = e.Item.FindControl("LokalizacjaOdbioruTextBox") as TextBox;
                        string t = "";
                        if (LokalizacjaOdbioruTextBox != null)
                            t = LokalizacjaOdbioruTextBox.Text;
                        dostarczono("" + pozycjeListView.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value, t);
                        pozycjeListView.DataBind();
                        ustawWidocznoscSzczegolowZamowienia();
                    }
                    break;
                case "Odebrano":
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        odebrano("" + pozycjeListView.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value);
                        pozycjeListView.DataBind();
                        ustawWidocznoscSzczegolowZamowienia();
                    }
                    break;
                case "ZapiszProdukt":
                    if (e.Item.ItemType == ListViewItemType.DataItem)
                    {
                        zapiszProdukt("" + pozycjeListView.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value);
                        pozycjeListView.DataBind();
                        ustawWidocznoscSzczegolowZamowienia();
                    }
                    break;
            }

        }

        protected void sciezkaAkceptacjiListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    sciezkaAkceptacjiListView.InsertItemPosition = InsertItemPosition.None;
                    break;
                case "NewRecord":
                    sciezkaAkceptacjiListView.EditIndex = -1;
                    sciezkaAkceptacjiListView.InsertItemPosition = InsertItemPosition.FirstItem;
                    break;
                case "CancelInsert":
                    sciezkaAkceptacjiListView.InsertItemPosition = InsertItemPosition.None;
                    break;
            }
        }

        protected void pozycjeListView_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            DropDownList JednostkaDropDownList = pozycjeListView.InsertItem.FindControl("JednostkaDropDownList") as DropDownList;
            pozycjeDataSource.InsertParameters["Jednostka"].DefaultValue = JednostkaDropDownList.SelectedValue;

            DropDownList WalutaDropDownList = pozycjeListView.InsertItem.FindControl("WalutaDropDownList") as DropDownList;
            pozycjeDataSource.InsertParameters["Waluta"].DefaultValue = WalutaDropDownList.SelectedValue;

            DropDownList DostawcaDropDownList = pozycjeListView.InsertItem.FindControl("DostawcaDropDownList") as DropDownList;
            pozycjeDataSource.InsertParameters["Dostawca"].DefaultValue = DostawcaDropDownList.SelectedValue;

            DropDownList RodzajProduktuDropDownList = pozycjeListView.InsertItem.FindControl("RodzajProduktuDropDownList") as DropDownList;
            pozycjeDataSource.InsertParameters["RodzajProduktu"].DefaultValue = RodzajProduktuDropDownList.SelectedValue;
        }

        protected void sciezkaAkceptacjiListView_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            DropDownList PracownicyDropDownList = sciezkaAkceptacjiListView.InsertItem.FindControl("PracownicyDropDownList") as DropDownList;
            SciezkaAkceptacjiDataSource.InsertParameters["UserId"].DefaultValue = PracownicyDropDownList.SelectedValue;
        }

        protected void sciezkaAkceptacjiListView_OnItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            DropDownList StatusAkceptacjiDropDownList = sciezkaAkceptacjiListView.EditItem.FindControl("StatusAkceptacjiDropDownList") as DropDownList;
            SciezkaAkceptacjiDataSource.UpdateParameters["Status"].DefaultValue = StatusAkceptacjiDropDownList.SelectedValue;
        }

        protected void ProduktHiddenField_OnValueChanged(object sender, EventArgs e)
        {
            Regex rgx = new Regex("_[0-9]+");
            string produktId = rgx.Replace(((HiddenField)sender).Value, "");

            pozycjeDataSource.InsertParameters["IdProduktu"].DefaultValue = produktId;

            DataRow dr = IPO_db.getDataRow("SELECT Nazwa, Opis, ProduktStockowy, RoHS, IdRodzajuProduktu, Cena, Waluta, IdDostawcy, KontoKsiegowe, Jednostka FROM IPO_Produkty WHERE Id = " + produktId);

            string dostawcaId = IPO_db.getScalar(@"
SELECT TOP 1 IPO_PreferowaniDostawcy.IdDostawcy
FROM IPO_PreferowaniDostawcy
JOIN IPO_Zamowienia ON IPO_Zamowienia.Id = " + zamowieniaListView.SelectedDataKey.Value + @"
JOIN IPO_CCZamowienia ON IPO_CCZamowienia.IdZamowienia = IPO_Zamowienia.Id
JOIN IPO_CCRegiony ON IPO_CCRegiony.IdCC = IPO_CCZamowienia.IdCC
WHERE IPO_PreferowaniDostawcy.IdProduktu = " + produktId + @"
AND IPO_PreferowaniDostawcy.IdRegionu = IPO_CCRegiony.IdRegionu
ORDER BY IPO_PreferowaniDostawcy.IdRegionu");

            TextBox NazwaTextBox = pozycjeListView.InsertItem.FindControl("NazwaTextBox") as TextBox;
            NazwaTextBox.Text = dr["Nazwa"].ToString();

            TextBox OpisTextBox = pozycjeListView.InsertItem.FindControl("OpisTextBox") as TextBox;
            OpisTextBox.Text = dr["Opis"].ToString();

            CheckBox ProduktStockowyCheckBox = pozycjeListView.InsertItem.FindControl("ProduktStockowyCheckBox") as CheckBox;
            ProduktStockowyCheckBox.Checked = (bool)dr["ProduktStockowy"];

            CheckBox RoHSCheckBox = pozycjeListView.InsertItem.FindControl("RoHSCheckBox") as CheckBox;
            RoHSCheckBox.Checked = (bool)dr["RoHS"];

            DropDownList RodzajProduktuDropDownList = pozycjeListView.InsertItem.FindControl("RodzajProduktuDropDownList") as DropDownList;
            ListItem rodzajProduktu = RodzajProduktuDropDownList.Items.FindByValue(dr["IdRodzajuProduktu"].ToString());
            if (rodzajProduktu != null)
                RodzajProduktuDropDownList.SelectedIndex = RodzajProduktuDropDownList.Items.IndexOf(rodzajProduktu);

            TextBox IloscTextBox = pozycjeListView.InsertItem.FindControl("IloscTextBox") as TextBox;
            IloscTextBox.Text = "1";

            TextBox CenaTextBox = pozycjeListView.InsertItem.FindControl("CenaTextBox") as TextBox;
            CenaTextBox.Text = dr["Cena"].ToString();

            DropDownList JednostkaDropDownList = pozycjeListView.InsertItem.FindControl("JednostkaDropDownList") as DropDownList;
            ListItem jednostka = JednostkaDropDownList.Items.FindByValue(dr["Jednostka"].ToString());
            if (jednostka != null)
                JednostkaDropDownList.SelectedIndex = JednostkaDropDownList.Items.IndexOf(jednostka);

            DropDownList WalutaDropDownList = pozycjeListView.InsertItem.FindControl("WalutaDropDownList") as DropDownList;
            ListItem waluta = WalutaDropDownList.Items.FindByValue(dr["Waluta"].ToString());
            if (waluta != null)
                WalutaDropDownList.SelectedIndex = WalutaDropDownList.Items.IndexOf(waluta);

            DropDownList DostawcaDropDownList = pozycjeListView.InsertItem.FindControl("DostawcaDropDownList") as DropDownList;
            ListItem dostawca = DostawcaDropDownList.Items.FindByValue(dostawcaId);
            if (dostawca != null)
                DostawcaDropDownList.SelectedIndex = DostawcaDropDownList.Items.IndexOf(dostawca);

            TextBox KontoKsiegoweTextBox = pozycjeListView.InsertItem.FindControl("KontoKsiegoweTextBox") as TextBox;
            KontoKsiegoweTextBox.Text = dr["KontoKsiegowe"].ToString();

            IPO_db.execSQL("IF EXISTS(SELECT 1 FROM IPO_ProduktyStat WHERE IdProduktu = " + produktId + " AND UserId = " + user.Id
                + ") BEGIN UPDATE IPO_ProduktyStat SET Licznik = Licznik+1 WHERE IdProduktu = " + produktId + " AND UserId = " + user.Id
                + " END  ELSE BEGIN INSERT INTO IPO_ProduktyStat(IdProduktu, UserId, Licznik) VALUES (" + produktId + "," + user.Id + ",1) END");
        }

        private void wyslij()
        {
            String IdZamowienia = "" + zamowieniaListView.SelectedDataKey.Value;

          //  IPO_db.execSQL("EXEC IPO_UtworzSciezkeAkceptacji @IdZamowienia = " + IdZamowienia);

            IPO_db.update("IPO_Zamowienia", 0, "PoziomAkceptacji", "Id=" + IdZamowienia, 1);
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu", "IdZamowienia=" + IdZamowienia, 2);

            IPO_Mailing.EventIPO("IPO_ZAM_UTW", IdZamowienia, null);
            IPO_Mailing.EventIPO("IPO_AKC_ACC", IdZamowienia, null);
            IPO_Mailing.EventIPO("IPO_KUP_UTW", IdZamowienia, null);
        }

        private void akceptuj()
        {
            String IdZamowienia = "" + zamowieniaListView.SelectedDataKey.Value;

            TextBox CERTextBox = szczegolyListView.Items[0].FindControl("CERTextBox") as TextBox;
            if (CERTextBox != null)
            {
                IPO_db.update("IPO_Zamowienia", 0, "CERNr", "Id=" + IdZamowienia, IPO_db.strParam(CERTextBox.Text));
            }

            IPO_db.execSQL("EXEC IPO_AkceptujZamowienie @IdZamowienia = " + IdZamowienia + ", @UserId = " + user.Id);
            string status = IPO_db.getScalar("SELECT IdStatusu FROM IPO_Zamowienia WHERE Id = " + IdZamowienia);

            IPO_Mailing.EventIPO("IPO_ZAM_ACC", IdZamowienia, null);
            IPO_Mailing.EventIPO("IPO_AKC_ACC", IdZamowienia, null);
            IPO_Mailing.EventIPO("IPO_KUP_ACC", IdZamowienia, null);
            if ("3".Equals(status))
            {
                IPO_Mailing.EventIPO("IPO_ZAM_RLZ", IdZamowienia, null);
                IPO_Mailing.EventIPO("IPO_KUP_RLZ", IdZamowienia, null);
            }
        }

        private void odrzuc()
        {
            String IdZamowienia = "" + zamowieniaListView.SelectedDataKey.Value;

            IPO_db.execSQL("EXEC IPO_OdrzucZamowienie @IdZamowienia = " + IdZamowienia + ", @UserId = " + user.Id);
        }

        private void odrzucPozycje(string id)
        {
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu,IdOdrzucajacego", "Id=" + id, 7, user.Id);
        }

        private void przywrocPozycje(string id)
        {
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu,IdOdrzucajacego", "Id=" + id, 2, 0);
        }

        private void zamowiono(string id)
        {
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu", "Id=" + id, 4);
        }
        private void zamowiono(string id, DateTime dataDostawy)
        {
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu,DataDostawy", "Id=" + id, 4, IPO_db.paramStr("" + dataDostawy));
            IPO_Mailing.EventIPO("IPO_ZAM_SPD", id, null);
        }
        private void dostarczono(string id, string lokalizacjaOdbioru)
        {
            //IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu,LokalizacjaOdbioru", "Id=" + id, 5, IPO_db.paramStr(lokalizacjaOdbioru));
            IPO_db.execSQL("UPDATE IPO_PozycjeZamowien SET IdStatusu = 5, DataDoOdbioru = GETDATE(), LokalizacjaOdbioru = " + IPO_db.paramStr(lokalizacjaOdbioru) + " WHERE Id = " + id);
            IPO_Mailing.EventIPO("IPO_ZAM_ODB", id, null);
        }
        private void odebrano(string id)
        {
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu", "Id=" + id, 6);
        }
        private void zapiszProdukt(string id)
        {
            IPO_db.execSQL("EXEC IPO_ZapiszProdukt @IdPozycjiZamowienia = " + id);
        }

        public bool IsPliki(int id)
        {
            string pliki = IPO_db.getScalar(@"DECLARE @IdNazwaList varchar(MAX)
                           SET @IdNazwaList = ''
                            SELECT @IdNazwaList = @IdNazwaList + CONVERT(varchar, Id) + '|' + NazwaPliku + ':' FROM IPO_Pliki WHERE IdPozycjiZamowienia = " + id
                            + "SELECT @IdNazwaList");
            return pliki != null && pliki.Length > 2;
        }

        public string[] GetPlikiDataSource(int id)
        {
            string pliki = IPO_db.getScalar(@"DECLARE @IdNazwaList varchar(MAX)
                           SET @IdNazwaList = ''
                            SELECT @IdNazwaList = @IdNazwaList + CONVERT(varchar, IPO_Pliki.Id) + '|' + NazwaPliku + '|' + Pracownicy.Nazwisko + ' ' + Pracownicy.Imie + ' ' + LEFT(CONVERT(VARCHAR, DataDodania, 120), 10) + ':' 
                            FROM IPO_Pliki
                            JOIN Pracownicy ON IPO_Pliki.UserId = Pracownicy.Id
                            WHERE IdPozycjiZamowienia = " + id
                            + "SELECT @IdNazwaList");
            if (pliki != null && (pliki).Contains("|") && (pliki).Contains(":"))
            {
                if (pliki.EndsWith(":"))
                {
                    pliki = pliki.Substring(0, pliki.Length - 1);
                }
                return pliki.Split(':');
            }
            else
            {
                return new string[] { };
            }
        }

        public string GetFileId(object data)
        {
            if (data != null && ((string)data).Contains("|"))
            {
                string[] d = ((string)data).Split('|');
                return d[0];
            }
            else
            {
                return null;
            }
        }

        public string GetFilename(object data)
        {
            if (data != null && ((string)data).Contains("|"))
            {
                string[] d = ((string)data).Split('|');
                return d[1];
            }
            else
            {
                return null;
            }
        }

        public string GetTooltip(object data)
        {
            if (data != null && ((string)data).Contains("|"))
            {
                string[] d = ((string)data).Split('|');
                return d[2];
            }
            else
            {
                return null;
            }
        }

        protected void pliki_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string plikId = (string)e.CommandArgument;                    
            switch (e.CommandName)
            {                    
                case "Download":
                    DownloadFile(plikId);
                    break;
                case "DeletePlik":
                    IPO_db.execSQL("delete from IPO_Pliki where Id = " + plikId);
                    foreach (ListViewItem i in pozycjeListView.Items)
                    {
                        try
                        {
                            UpdatePanel UpdatePanel1 = i.FindControl("UpdatePanel1") as UpdatePanel;
                            ListView plikiListView = UpdatePanel1.FindControl("plikiListView") as ListView;
                            plikiListView.DataBind();
                            UpdatePanel1.Update();

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    break;
            }
        }
       
        protected void DownloadFile(string plikId)
        {
            byte[] bytes;
            string fileName, contentType = "application/oc-stream";
            string constr = ConfigurationManager.ConnectionStrings["IPO"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select NazwaPliku, Plik from IPO_Pliki where Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", plikId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Plik"];
                        //contentType = sdr["ContentType"].ToString();
                        fileName = sdr["NazwaPliku"].ToString();
                    }
                    con.Close();
                }
            }            
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            AsyncFileUpload fileUpload = sender as AsyncFileUpload;
            HiddenField hidIdZamowienia = fileUpload.Parent.FindControl("hidIdZamowienia") as HiddenField;
            HiddenField hidIdPozycji = fileUpload.Parent.FindControl("hidIdPozycji") as HiddenField;
            string constr = ConfigurationManager.ConnectionStrings["IPO"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "insert into IPO_Pliki(IdZamowienia, IdPozycjiZamowienia, NazwaPliku, Plik, UserId) values (@IdZamowienia, @IdPozycjiZamowienia, @NazwaPliku, @Plik, @UserId)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@IdZamowienia", hidIdZamowienia.Value);
                    cmd.Parameters.AddWithValue("@IdPozycjiZamowienia", hidIdPozycji.Value);
                    cmd.Parameters.AddWithValue("@NazwaPliku", fileUpload.FileName);
                    cmd.Parameters.AddWithValue("@Plik", fileUpload.FileBytes);
                    cmd.Parameters.AddWithValue("@UserId", user.Id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                ClearContents(sender as Control);
            }
            foreach (ListViewItem i in pozycjeListView.Items)
            {
                try
                {
                    UpdatePanel UpdatePanel1 = i.FindControl("UpdatePanel1") as UpdatePanel;
                    ListView plikiListView = UpdatePanel1.FindControl("plikiListView") as ListView;
                    plikiListView.DataBind();
                    UpdatePanel1.Update();

                }
                catch (Exception ex)
                {
                }
            }
            //fileUpload.Parent.DataBind();
            //Tools.ExecOnStart2("refreshPozycje", "location.reload();");
        }

        private void ClearContents(Control control)
        {
            for (var i = 0; i < Session.Keys.Count; i++)
            {
                if (Session.Keys[i].Contains(control.ClientID))
                {
                    Session.Remove(Session.Keys[i]);
                    break;
                }
            }
        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
         //   Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }

        protected void btRefreshPozycje_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem i in pozycjeListView.Items)
            //{
            //    //ListView plikiListView = i.FindControl("plikiListView") as ListView;
            //    //plikiListView.DataBind();
            //    UpdatePanel updatePanel = i.FindControl("UpdatePanel1") as UpdatePanel;
            //    updatePanel.Update();
            //}
        }

        protected bool WyborSciezki() 
        {
            nowaSciezkaAkceptacjiListView.DataBind();
            staraSciezkaAkceptacjiListView.DataBind();
            
            return zamowieniaListView.SelectedDataKey != null && Convert.ToInt32(IPO_db.getScalar(@"SELECT COUNT(*)
                    FROM IPO_SciezkaAkceptacji
                    JOIN IPO_Zamowienia ON IPO_Zamowienia.Id = IPO_SciezkaAkceptacji.IdZamowienia
                    WHERE IdZamowienia = " +zamowieniaListView.SelectedDataKey.Value+@" 
                    AND IPO_Zamowienia.DataAkceptacji < (SELECT TOP 1 DataSciezki FROM IPO_SciezkaAkceptacji WHERE IdZamowienia = " + zamowieniaListView.SelectedDataKey.Value + @" ORDER BY DataSciezki DESC)")) > 0;
        }

        protected void NowaSciezka_OnClick(object sender, EventArgs e)
        {
            String IdZamowienia = "" + zamowieniaListView.SelectedDataKey.Value;
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu", "IdZamowienia=" + IdZamowienia + " AND IdStatusu=0", 2);   
            IPO_db.execSQL("UPDATE IPO_Zamowienia SET DataAkceptacji = (SELECT TOP 1 DataSciezki FROM IPO_SciezkaAkceptacji WHERE IdZamowienia = " + IdZamowienia + @" ORDER BY DataSciezki DESC), WartoscDoAkceptacji=Wartosc WHERE Id=" + IdZamowienia);
            IPO_Mailing.EventIPO("IPO_KUP_SCN", IdZamowienia, null);
            pozycjeListView.DataBind();
            sciezkaAkceptacjiListView.DataBind();
        }

        protected void StaraSciezka_OnClick(object sender, EventArgs e)
        {
            String IdZamowienia = "" + zamowieniaListView.SelectedDataKey.Value;
            IPO_db.update("IPO_PozycjeZamowien", 0, "IdStatusu", "IdZamowienia=" + IdZamowienia + " AND IdStatusu=0", 2);
   
            IPO_db.execSQL(@"DECLARE @NowaData DateTime
                         DECLARE @StaraData DateTime
                        Select @Nowadata = getDate()
                        Select @StaraData = DataAkceptacji FROM IPO_Zamowienia where id=" + IdZamowienia + @"
                        UPDATE IPO_Zamowienia SET DataAkceptacji = @NowaData, WartoscDoAkceptacji=Wartosc where Id=" + IdZamowienia + @"
                        UPDATE IPO_SciezkaAkceptacji SET DataSciezki = @NowaData where DataSciezki = @StaraData
                        AND IdZamowienia=" + IdZamowienia);

            IPO_Mailing.EventIPO("IPO_KUP_SCS", IdZamowienia, null);
            sciezkaAkceptacjiListView.DataBind();
            pozycjeListView.DataBind();
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
           // DoSearch(true);
        }

        public string GetSciezkaClass(object id)
        {
            return "red";
        }


        protected void zamowieniaListView_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                Tools.SetControlEnabled(e.Item, "CERNrTextBox", checkboxState);
                Tools.SetControlEnabled(e.Item, "CERValidator", checkboxState);
                Tools.SetChecked(e.Item, "CERCheckBox", checkboxState);
            }
        }

        protected void zamowieniaListView_LayoutCreated(object sender, EventArgs e)
        {
                
                switch (Rola)
                {
                    case "1": rola1(App.User); break;
                    case "2": rola2(App.User); break;
                    case "3": rola3(App.User); break;
                    case "4": rola4(App.User); break;
                    case "5": rola5(App.User); break;
                }
        }

        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                cntSplityCC splity = zamowieniaListView.InsertItem.FindControl("cntSplityCC") as cntSplityCC;
                if (splity != null)
                {
                    System.Data.Common.DbCommand command = e.Command;
                    //string pracId = command.Parameters["@IdPracownika"].Value.ToString();
                    string przId = command.Parameters["@LastId"].Value.ToString();
                    splity.IdPrzypisania = przId;
                    splity.Update();
                }
            }
        }

    }
}