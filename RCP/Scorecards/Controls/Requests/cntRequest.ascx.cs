using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Data;

namespace HRRcp.Scorecards.Controls.Requests
{
    public partial class cntRequest : System.Web.UI.UserControl
    {
        public event EventHandler SomethingChanged;
        public event EventHandler Closed;

        //T: statusy wniosku - nie wiem czy to nie jest gdzies indziej dodane, najwyzej zmiencie
        public const int stNew  = 0;    // nowy wniosek, u składającego w przypadku cofnięcia, może to być TL, Kier, Zarz (Admin nie, powinien wskazać przełożonego w imieniu którego wnioskuje)
        public const int stKier = 1;    // wniosek wysłany, czeka na akceptację Kierownika
        public const int stZarz = 2;    // wniosek po akceptacji Kierownika, czeka na akceptację Zarządu
        public const int stHR   = 3;    // 

        public const int accNull = -1;
        public const int accRej  = 0;
        public const int accAcc = 1;

        // role kto ogląda dany wniosek ViewFor...
        public const int vfSkip     = 0;     // na tymczas póki nie zostaną ustawione poprawne wartosci, potem usunac i wywalic if w kodzie   
        public const int vfWnioskujacy = 1;  
        public const int vfKier     = 2;
        public const int vfZarz     = 3;
        public const int vfAdmin    = 4;
        public const int vfHR       = 5;
        public const int vfReadOnly = 6;    //być moze to samo co hr




        const String moMine = "0";
        const String moToAcc = "1";
        const String moAccepted = "2";
        const String moDoWyjasnienia = "3";
        const String moRejected = "4";
        const String moAll = "99";

        public const String uprTL = "0";
        public const String uprKier = "1";
        public const String uprPrezes = "2";

        const String wnAddress = "Scorecards/Wnioski.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            Tools.ExecOnStart2("prepReq", "prepareRequests();");
            if (!IsPostBack) {
                //ClientScriptManager cs = Page.ClientScript;
                //.Attributes.Add("onclick", String.Format("this.disabled=true;{0};", cs.GetPostBackEventReference(bt, null).ToString()));
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvRequests, 0);
            Tools.PrepareSorting2(lvRequests, 1, 30);
        }

        public void Prepare(String RequestId, String ObserverId, String Mode, String Upr, Boolean Editable, Boolean Custom, Boolean Group)
        {
            //DataRow Data = db.Select.Row(dsData, RequestId);

            this.RequestId = RequestId;
            this.Mode = Mode;
            this.Upr = Upr;
            this.Custom = Custom;
            this.Editable = Editable;
            this.ObserverId = (App.User.IsScAdmin || App.User.IsSuperuser || App.User.IsScZarz) ? "0" : ObserverId;
            this.Group = Group;

            //switch (Mode)
            //{
            //    case moMine:
            //        btnReject.Visible = false;
            //        break;
            //    case moToAcc:
            //        break;
            //    default:
            //        btnReject.Visible = true;
            //        break;
            //}


            //T: pewnie mozna to wyciagnąc z zapytania ale tam jest za dużo linków więc moze nieoptymalnie teraz, ale pewnie
            if (!String.IsNullOrEmpty(RequestId))
            {
                DataRow dr = db.getDataRow(String.Format("select * from scWnioski where Id = {0}", RequestId));
                if (dr != null)
                {
                    Status = db.getInt(dr, "Status", -1);
                    Kacc = db.getInt(dr, "Kacc", -1);
                    Pacc = db.getInt(dr, "Pacc", -1);
                }
            }
            ViewFor = vfSkip;  // ustwić kto ogląda Wnioskujący, Kierownik, Zarząd, Admin, HR, ReadOnly 


            lvRequests.DataBind();
            PrepareButtons();
            if (Custom) PrepareCustom();
            else divNewEmployee.Visible = false;
        }

        void PrepareCustom()
        {
            ddlEmployees.DataBind();
            divNewEmployee.Visible = Custom && IsCustomEditable() && ddlEmployees.Items.Count > 0;
            btnSendConfirm.ValidationGroup = "sendvg";
        }

        void PrepareButtons()
        {
            //Boolean Reject = Editable && (Mode != moMine);
            Boolean Reject = Editable && (Mode != moMine) && Status > 0;    //T: na razie tak, jak omówimy to trzeba będzie zmienić na Status == stKier and Kacc == 1 and (observer == IdKierownika or observer.IsScAdmin) or  
                                                                            //                                                         Status == stZarz and Pacc == 1 and (observer.HarRight(Zarząd) or observer.IsScAdmin)
                                                                            //uwaga: mam wrażenie, że Status trzymający wszystkie możliwe wartosci bez uwzglednienia Kacc i Pacc byłby dużo prostszy w obsłudze i bardziej jednoznaczny                                                         

            Boolean DeleteGroupConfirm = (Group && Mode == moMine && Editable && App.User.IsScWnRej) || (Custom && Mode == moMine && Editable && Kacc == -1 && Pacc == -1); /*(Group || Custom) && Mode == moMine && Editable;&& App.User.IsScWnRej;*/  //20160210
            //Boolean Close = !DeleteGroupConfirm && !Editable;
            Boolean DestroyConfirm = Custom /*&& App.User.HasRight(AppUser.rScorecardsWnDstr)*/ && /*IsCustomEditable()*/IsInEdit(); //Mode == moToAcc;
            Boolean Save = Editable;


            //Boolean SendConfirm = Editable;
            int status = Status;    //T:
            int vfor = ViewFor;
            Boolean SendConfirm = Editable && (status == stNew || status == stKier && vfor == vfKier && Kacc != accAcc || status == stZarz && vfor == vfZarz && Pacc != accAcc
                
                || vfor == vfSkip   // do usunięcia później !!!
                
                );
            
            
            
            String SendConfirmText = (Upr == uprTL) || (Mode == moMine && Custom) ? "Wyślij" : "Akceptuj";




            Boolean HeaderEditable = IsCustomEditable() || (!Custom && Editable);

            RequestHeader.Prepare(RequestId, ObserverId, HeaderEditable, Custom, true, Editable);

            btnRejectConfirm.Visible = Reject;
            btnSave.Visible = Editable;
            btnSendConfirm.Visible = SendConfirm;
            btnDeleteGroupConfirm.Visible = DeleteGroupConfirm;
            ShowHideSendButton();

            if (Group && lvRequests.Items.Count == 0)
            {
                btnDeleteGroupConfirm.Visible = btnSendConfirm.Visible = btnSave.Visible = false;
            }

            //btnClose.Visible = Close;
            btnDestroyConfirm.Visible = DestroyConfirm;
            btnSendConfirm.Text = SendConfirmText;
        }

        protected void RejectItem(object sender, EventArgs e)
        {
            SaveAll();
            Button btn = sender as Button;
            String Id = btn.CommandArgument;
            if (!String.IsNullOrEmpty(Id))
            {
                db.Execute(dsRejectItem, Id);
                lvRequests.DataBind();
            }
            ShowHideSendButton();
        }

        protected void UnrejectItem(object sender, EventArgs e)
        {
            SaveAll();
            Button btn = sender as Button;
            String Id = btn.CommandArgument;
            if (!String.IsNullOrEmpty(Id))
            {
                db.Execute(dsUnrejectItem, Id);
                lvRequests.DataBind();
            }
            ShowHideSendButton();
        }

        protected void ShowHideSendButton()
        {
            Boolean Show = true;
            foreach (ListViewDataItem Item in lvRequests.Items)
            {
                String Rejected = Tools.GetText(Item, "hidRejected");
                if (Rejected == "1")
                {
                    Show = false;
                    break;
                }
            }
            String Show2 = db.Select.Scalar(dsSendButton, RequestId);
            
            
            //btnSendConfirm.Visible = Show && Editable;
            int status = Status;        //T:
            int vfor = ViewFor;
            bool sendConfirmVisible = Show && Editable && (status == stNew || status == stKier && vfor == vfKier && Kacc != accAcc || status == stZarz && vfor == vfZarz && Pacc != accAcc

                || vfor == vfSkip   // do usunięcia później !!!

                );
            btnSendConfirm.Visible = sendConfirmVisible;


            //if (IsIndividual() && btnSendConfirm.Visible) btnSendConfirm.Visible = (Show2 == "1");  
            if (IsIndividual() && sendConfirmVisible) btnSendConfirm.Visible = (Show2 == "1");  //T: nie powinno się odwoływać do właściwości Visible !!! nie zwracają rzeczywistych danych
            if (Custom)
            {
                String Show3 = db.Select.Scalar("select case when 0 < COUNT(Id) then 1 else 0 end from scPremie where IdWniosku = {0}", RequestId);
                btnSendConfirm.Visible = (Show3 == "1") && sendConfirmVisible;
            }


            //T: 2 powyższe ustawienie btnSendConfirm są do zastąpienia frazą z użyciem ViewFor
        }

        protected void Save(object sender, EventArgs e)
        {
            if (SaveAll()) Close();
            //if (SomethingChanged != null) SomethingChanged(null, EventArgs.Empty);
            //if (Closed != null) Closed(null, EventArgs.Empty);
        }

        Boolean SaveAll()
        {
            if (RequestHeader.Save()) 
                return Save();
            return false;
        }

        Boolean Save()
        {
            Double Pula = RequestHeader.GetPulaPremii();
            Double PremiaAll = 0;

            foreach (ListViewDataItem item in lvRequests.Items)
            {
                Double Premia = 0;
                String PremiaUznaniowa = Tools.GetText(item, "tbPremiaUznaniowa");
                try
                {
                    Premia = Double.Parse(PremiaUznaniowa.Replace(',', '.'), CultureInfo.InvariantCulture);
                }
                catch { }
                PremiaAll += Premia;
            }

            if (PremiaAll > Pula && !Custom)
            {
                Tools.ShowMessage("Premia przekracza pulę!");
                return false;
            }

            foreach (ListViewDataItem item in lvRequests.Items)
            {
                String PremiaUznaniowa = Tools.GetText(item, "tbPremiaUznaniowa");
                String Uwagi = Tools.GetText(item, "tbUwagi");
                String CC = Tools.GetDdlSelectedValue(item, "ddlCC");
                String _id = Tools.GetText(item, "hid_id");
                String OldPremia = Tools.GetText(item, "hidOldPremia");
                String OldUwagi = Tools.GetText(item, "hidOldUwagi");
                String OldCC = Tools.GetText(item, "hidOldCC");

                if (!String.IsNullOrEmpty(_id) && (PremiaUznaniowa != OldPremia || Uwagi != OldUwagi || CC != OldCC))
                {
                    Double Uzn = (String.IsNullOrEmpty(PremiaUznaniowa) ? 0 : Double.Parse(PremiaUznaniowa.Replace(',', '.'), CultureInfo.InvariantCulture));
                    db.Execute(dsSave, Uzn, db.nullStrParam(Uwagi), _id, db.nullParam(CC));
                }
            }
            return true;
        }

        void Close()
        {
            Tools.ExecOnStart2("doc", String.Format("doClick('{0}');", btnClose.ClientID));
        }

        protected void Close(object sender, EventArgs e)
        {
            if (SomethingChanged != null) SomethingChanged(null, EventArgs.Empty);
        }

        //public static void ShowConfirm(String Content, Button btnYes)
        //{
        //    Page page = HttpContext.Current.Handler as Page;
        //    ScriptManager.RegisterStartupScript(page, typeof(Page), "showConfirm", St"showConfirm('" + msg + "');", true);


        //    //Tools.ExecOnStart(String.Format("showConfirm('{0}', '{1}');", Content, btnYes.ClientID));
        //}

        //protected void ShowPopup(object sender, EventArgs e)
        //{
        //    ShowConfirm("Czy na penwo coś tam?", btnClose);
        //}


        protected void SendConfirm(object sender, EventArgs e)
        {
            Tools.ShowConfirm(((Upr == uprTL) || (Mode == moMine && Custom)) ? "Czy na pewno chesz wysłać wniosek?" : "Czy na pewno chcesz zaakceptować wniosek?", btnSend, null);
        }

        protected void Send(object sender, EventArgs e)
        {
            Send();
        }

        void Send()
        {
            if (!SaveAll()) return;
            // czy aby?
            //if (Mode == moMine)
            //{
            //    db.Execute(dsSend, RequestId);
            //}
            //else

            String MailType = null;

            switch (Upr)
            {
                case uprTL:
                    db.Execute(dsSendTL, RequestId, App.User.OriginalId);
                    MailType = HRRcp.Scorecards.App_Code.Mailing.maSC_WNACC;
                    break;
                case uprKier:
                    db.Execute(dsSendKieras, RequestId, App.User.OriginalId);
                    //zatrzask
                    db.Execute(dsKlinke, RequestId);
                    MailType = HRRcp.Scorecards.App_Code.Mailing.maSC_WNACC;
                    break;
                case uprPrezes:
                    db.Execute(dsSendPrezes, RequestId, App.User.OriginalId);
                    MailType = HRRcp.Scorecards.App_Code.Mailing.maSC_WNACCHR;
                    break;
            }

            //T: mailing
            HRRcp.Scorecards.App_Code.Mailing.EventWniosekPremiowy(MailType, Upr, wnAddress, RequestId);    // po Upr mozna bedzie w prosty sposob rozpoznac do kogo mail słać 
            Log.Info(1337, "Wysłano wniosek", GetRequestInfo());
            Tools.ShowMessage(((Upr == uprTL) || (Mode == moMine && Custom)) ? "Wniosek został wysłany." : "Wniosek został zaakceptowany.");
            Close();
         
            //if (SomethingChanged != null) SomethingChanged(null, EventArgs.Empty);
            //if (Closed != null) Closed(null, EventArgs.Empty);
        }

        protected void DestroyConfirm(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz odrzucić wniosek?", btnDestroy, null);
        }

        protected void Destroy(object sender, EventArgs e)
        {
            if (!SaveAll()) return;
            db.Execute(dsDestroy, RequestId, App.User.Id, db.nullParamStr(DateTime.Now.ToString()));
            // mail
            HRRcp.Scorecards.App_Code.Mailing.EventWniosekPremiowy(HRRcp.Scorecards.App_Code.Mailing.maSC_WNREJ, Upr, wnAddress, RequestId);
            Log.Info(1337, "Odrzucono wniosek", GetRequestInfo());
            if (SomethingChanged != null) SomethingChanged(null, EventArgs.Empty);
            if (Closed != null) Closed(null, EventArgs.Empty);
        }

        protected void RejectConfirm(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz cofnąć wniosek do wyjaśnienia?", btnReject, null);
        }

        protected void Reject(object sender, EventArgs e)
        {
            if (!SaveAll()) return; ;
            switch (Upr)
            {
                case uprKier:
                    db.Execute(dsRejectKieras, RequestId);
                    break;
                case uprPrezes:
                    db.Execute(dsRejectPrezes, RequestId);
                    break;
            }
            // mail
            HRRcp.Scorecards.App_Code.Mailing.EventWniosekPremiowy(HRRcp.Scorecards.App_Code.Mailing.maSC_WNBACK, Upr, wnAddress, RequestId);
            Log.Info(1337, "Cofnięto wniosek do wyjaśnienia", GetRequestInfo());
            Close();
            //if (SomethingChanged != null) SomethingChanged(null, EventArgs.Empty);
            //if (Closed != null) Closed(null, EventArgs.Empty);
        }

        protected void DeleteGroupConfirm(object sender, EventArgs e)
        {
            Tools.ShowConfirm(Custom ? "Czy na pewno chcesz usunąć wniosek?" : "Czy na pewno chcesz usunąć pracowników z wniosku?", btnDeleteGroup, null);
        }

        protected void DeleteGroup(object sender, EventArgs e)
        {
            DeleteGroup();
            PrepareButtons();
        }

        void DeleteGroup()
        {
            db.Execute(dsDeleteGroup, RequestId);
            if (Custom)
            {
                db.Execute(dsDeleteRequest, RequestId);
                if (SomethingChanged != null) SomethingChanged(null, EventArgs.Empty);
                //Tools.ShowMessage("Usunięto wniosek!");
            }
            else
            {
                Tools.ShowMessage("Usunięto pracowników z wniosku!");
            }
            Log.Info(1337, "Usunięto wniosek", GetRequestInfo());
            lvRequests.DataBind();
        }

        public String GetLast(Object ToSplit)
        {
            String Split = ToSplit.ToString();
            return Split.Split('¬').Last();
        }

        public String GetToolTip(Object O)
        {
            String S = O.ToString();
            String Output = String.Empty;

            foreach (String Sp in S.Split('¬'))
            {
                Output += Sp + "\n";
            }
            return Output;
        }
        int counter;
        protected void lvRequests_DataBinding(object sender, EventArgs e)
        {
            counter = 1;
        }

        protected void lvRequests_DataBound(object sender, EventArgs e)
        {
            SetVisible("th1c", Custom);
            SetVisible("ft1c", Custom);

            SetVisible("th2c", !Custom);
            SetVisible("th3c", !Custom);
            SetVisible("ft1", !Custom);
            SetVisible("ft2", !Custom);

            Boolean Ind = IsIndividual();

            SetVisible("ft1i", Ind);
            SetVisible("ft2i", /*Ind*/false);
            SetVisible("ft3i", Ind);
            SetVisible("ft4i", /*Ind*/false);
            SetVisible("ft5i", /*Ind*/false);
            SetVisible("ft6i", Ind);
            SetVisible("th1i", Ind);
            SetVisible("th2i", /*Ind*/false);
            SetVisible("th3i", Ind);
            SetVisible("th4i", /*Ind*/false);
            SetVisible("th5i", /*Ind*/false);
            SetVisible("th6i", Ind);

            SetVisible("th1g", Group);
            SetVisible("th2g", Group);
            SetVisible("th3g", !Custom/*Group*/);

            SetVisible("ft1g", Group);
            SetVisible("ft2g", Group);
            SetVisible("ft3g", !Custom/*Group*/);

            SetVisible("thControl", IsInEdit());
            SetVisible("ftControl", IsInEdit());


            Tools.SetText(lvRequests.Controls[0], "lbNoData", GetEmptyDataLabel());
        }

        protected String GetEmptyDataLabel()
        {
            return (IsCustom()) ? "Brak pracowników" : "Brak pracowników. Proszę zaakceptować arkusze.";
        }

        protected void lvRequests_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

            Tools.SetText(e.Item, "lblLp", counter.ToString());
            counter++;

            DataRowView drv = null;
            int li = Tools.GetListItemMode(e, lvRequests, out drv);

            if (drv != null) Tools.SelectItem(e.Item, "ddlCC", drv["IdCC"]);
        }

        void SetVisible(String Id, Boolean B)
        {
            HtmlTableCell Cell = lvRequests.FindControl(Id) as HtmlTableCell;
            if (Cell != null) Cell.Visible = B;
        }

        protected void RemoveEmployee(object sender, EventArgs e)
        {
            SaveAll();
            Button btnRemoveEmployee = (sender as Button);
            String Id = btnRemoveEmployee.CommandArgument;
            if (!String.IsNullOrEmpty(Id))
            {
                db.Execute(dsRemoveEmployee, Id);
                if(Kacc != -1)
                    Log.Info(1337, String.Format("Usunięto pracownika o id: {0} z wniosku", Id), GetRequestInfo());
                lvRequests.DataBind();
            }
            ShowHideSendButton();
            ddlEmployees.DataBind();

            divNewEmployee.Visible = Custom && IsCustomEditable() && ddlEmployees.Items.Count > 0;
        }

        protected void AddEmployee(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ddlEmployees.SelectedValue))
            {
                SaveAll();
                db.Execute(dsAddEmployee, ddlEmployees.SelectedValue, RequestId);
                lvRequests.DataBind();
                ddlEmployees.DataBind();
                divNewEmployee.Visible = ddlEmployees.Items.Count > 0;


                //T:
                ShowHideSendButton();
            }
        }

        public String GetRequestInfo()
        {
            return String.Format("RequestId: {0}, ObserverId: {1}", RequestId, ObserverId);
        }

        public String GetColorClass(String Default, String Rej)
        {
            return Default + (Rej == "1" ? " rej" : "");
        }

        public String GetTLClass(String Default, String TLA)
        {
            return Default + (TLA == "1" ? " tl" : " ntl");
        }

        public String GetNumClass(String Default, Boolean B)
        {
            return Default + (B ? " num" : String.Empty);
        }

        public Boolean RejectButtonVisible(String Color)
        {
            return Color != "1";
        }

        public Boolean IsToAcc()
        {
            return Mode == moToAcc;
        }

        public Boolean DeleteButtonVisible()
        {
            return ((!Group && Mode == moMine && Editable && App.User.IsScWnRej) || (Custom && Editable));
        }

        public Boolean IsIndividual()
        {
            return !Custom && !Group;
        }

        public Boolean IsGroup()
        {
            return Group;
        }

        public String RequestId
        {
            get { return hidRequestId.Value; }
            set { hidRequestId.Value = value; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }

        public Boolean IsTL(String TL)
        {
            return TL == "1";
        }

        public String Mode
        {
            get { return hidMode.Value; }
            set { hidMode.Value = value; }
        }

        public String Upr
        {
            get { return hidUpr.Value; }
            set { hidUpr.Value = value; }
        }

        public Boolean Editable
        {
            get { return (Boolean)ViewState["vEditable"]; }
            set { ViewState["vEditable"] = value; }
        }

        public Boolean IsInEdit()
        {
            return Editable;
        }

        public Boolean IsCustomEditable()
        {
            return Custom && Mode == moMine && IsInEdit();
        }

        public Boolean Custom
        {
            get { return Tools.GetViewStateBool(ViewState["vCustom"], false); }
            set { ViewState["vCustom"] = value; }
        }

        public Boolean Group
        {
            get { return Tools.GetViewStateBool(ViewState["vGroup"], false); }
            set { ViewState["vGroup"] = value; }
        }

        public Boolean IsCustom()
        {
            return Custom;
        }

        public Button SaveButton
        {
            get { return btnSave; }
        }

        public Button SendButton
        {
            get { return btnSend; }
        }

        public Button RejectButton
        {
            get { return btnReject; }
        }

        public Button DestroyButton
        {
            get { return btnDestroy; }
        }

        public Button DeleteButton
        {
            get { return btnDeleteGroup; }
        }

        public Button CloseButton
        {
            get { return btnClose; }
        }


        //----------------------------------
        public int Status
        {
            set { ViewState["status"] = value; }
            get { return Tools.GetInt(ViewState["status"], stNew); }
        }

        public int Kacc
        {
            set { ViewState["kacc"] = value; }
            get { return Tools.GetInt(ViewState["kacc"], accNull); }
        }

        public int Pacc
        {
            set { ViewState["pacc"] = value; }
            get { return Tools.GetInt(ViewState["pacc"], accNull); }
        }

        public int ViewFor
        {
            set { ViewState["viewfor"] = value; }
            get { return Tools.GetInt(ViewState["viewfor"], vfWnioskujacy); }
        }


    }
}