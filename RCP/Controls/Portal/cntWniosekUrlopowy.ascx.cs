using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AjaxControlToolkit;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntWniosekUrlopowy : System.Web.UI.UserControl
    {
        public event EventHandler Close;
        public bool Updated = false;

        public const int stNew          = 0;
        public const int stWaitingAcc   = 1;
        public const int stRejected     = 2;
        public const int stAccepted     = 3;
        public const int stAcceptedHR   = 4;    // ostatni status powinien być tylko do podglądu (queryMode) - wprowadzony do Asseco !!!

        public const int stDeleted      = -1;   // jeszcze nie ma zastosowania     

        public const int stAcceptedAll  = 34;        
        public const int stMoje         = 9;
        public const int stAll          = 99;
        public const int stByMe         = 8;    // wprowadzone przeze mnie dla innych

        public const int wtUW  = 1;  // Urlop wypoczynkowy
        public const int wtUZ  = 2;  // Urlop na żądanie
        public const int wtUO  = 3;  // Urlop okolicznościowy 
        public const int _wtO2  = 4;  // Opieka nad zdrowym dzieckiem do lat 14 (art. 188 KP)
        public const int wtSZ  = 5;  // Urlop szkoleniowy
        public const int wtOD  = 6;  // Czas wolny za pracę w godzinach nadliczbowych (151(2) par.1) KP
        public const int wtUB  = 7;  // Urlop bezpłatny
        public const int wtNU  = 8;  // Nieobecność usprawiedliwiona
        public const int wtNN  = 9;  // Nieobecność nieusprawiedliwiona
        public const int wtSW  = 10; // wolne za święto
        public const int wtUD  = 11; // urlop dodatkowy
        public const int wtO2h = 4;  //to samo co 4!!! będzie jeden wniosek //12; // Opieka nad zdrowym dzieckiem do lat 14 (art. 188 KP) + godzinowo

        public const int wtODPR = 10;  // o zgodę na wyjście w godzinach pracy z deklaracją odpracowania - SIEMENS !!!! <<<< zmienić i przekonwertować w SIEMENS'ie


        public const int osPracownik = 0;
        public const int osKierownik = 1;
        public const int osAdmin = 2;
        //public const int osWnioskujacy = 0;
        //public const int osAkceptujacy = 1;
        //public const int osAdmin = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (paWniosekU.Visible)
            {
                if (!Initialized)
                {
                    Initialized = true;
                }
            }
            if (!IsPostBack) // jak jest na innej zakładce niż widoczna to się nie pokazuje więc Initialized
            {
                //ClientScriptManager cs = Page.ClientScript;
                //btSend2.Attributes.Add("onclick", String.Format("this.disabled=true;{0};", cs.GetPostBackEventReference(btSend2, null).ToString()));        // a to i tak działa tak ze jest pytanie czy potwierdzić wysłanie
                //btAccept2.Attributes.Add("onclick", String.Format("this.disabled=true;{0};", cs.GetPostBackEventReference(btAccept2, null).ToString()));    // w sumei to ten chyba nie musi być ...
            }
            else
            {
                Godzin.AsString = hidGodz.Value;
                if (popupVisible) popupShow(true);
            }
        }
        //-----
        public void Show(int wnId, int osoba, bool queryMode)
        {
            WniosekId = wnId;
            Osoba = osoba;
            QueryMode = queryMode;
            FillData(wnId, -1, osoba, queryMode);
        }

        public void ShowPopup(ModalPopupExtender mpe, int wnId, int osoba, bool queryMode)
        {
            Show(wnId, osoba, queryMode);
            popupId = mpe.ID;
            mpe.Show();
        }
        //-----
        public void PrepareNew(int wnTyp, int osoba)
        {
            WniosekId = -1;
            FillData(-1, wnTyp, osoba, false);
        }

        public void PrepareNewPopup(ModalPopupExtender mpe, int wnTyp, int osoba)
        {
            PrepareNew(wnTyp, osoba);
            popupId = mpe.ID;
            mpe.Show();
        }
        //------------------------------------
        //------------------------------------
        private void popupShow(bool show)
        {
            if (!String.IsNullOrEmpty(popupId))
            {
                ModalPopupExtender mpe = Parent.FindControl(popupId) as ModalPopupExtender;
                if (mpe != null)
                    if (show)
                        mpe.Show();
                    else
                    {
                        mpe.Hide();
                        popupId = null;
                    }
            }
        }
        
        private bool popupVisible
        {
            get { return !String.IsNullOrEmpty(popupId); }
        }

        private string popupId
        {
            set { ViewState["popid"] = value; }
            get { return Tools.GetStr(ViewState["popid"]); }
        }

        private void ShowError(string msg)
        {
            if (popupVisible)
                Tools.popupShowMessage(2, msg);
            else
                Tools.ShowError(msg);
        }

        private void ShowMessage(string msg)
        {
            if (popupVisible)
                Tools.popupShowMessage(1, msg);
            else
                Tools.ShowMessage(msg);
        }

        private void ShowConfirm(string msg, Button btOk, Button btCancel)
        {
            if (popupVisible)
                Tools.popupShowConfirm(msg, btOk, btCancel);
            else
                Tools.ShowConfirm(msg, btOk, btCancel);
        }
        //-----------------------------------
        private void StartDateCheck()
        {
            Tools.ExecOnStart2("wnoddoil", String.Format("checkUrlopDni2('{0}','{1}','{2}','{3}','{4}','{5}','{6}');",
                Od.DeValue.EditBox.ClientID,
                Do.DeValue.EditBox.ClientID,
                Dni.TbValue.ClientID,
                hidMnoznik.ClientID,
                hidGodz.ClientID,
                Godzin.LbValue.ClientID,
                ResolveUrl("~/" + App.MainSvc)
                ));
        }

        private void StopDateCheck()
        {
            Tools.ExecOnStart2("wnoddoils", "checkUrlopDni2Stop();");
        }

        private void FillData(int wnId, int wnTyp, int osoba, bool queryMode)
        {
            DataSet ds;
            if (wnId == -1)
            {
                //---- nowy wniosek -----               // <<< kier z Przypisań !!!, GodzinZm z dodatkowych ustawień !!!
                ds = db.getDataSet(String.Format(dsWniosek.InsertCommand
                    /*
                    @"
                    declare @pracId int
                    declare @autorId int
                    declare @status int
                    declare @wtyp int
                    declare @data varchar(20)
                    set @pracId = '{0}'
                    set @status = {1}
                    set @wtyp = {2}
                    set @data = {3}
                    set @autorId = {4}
                    select @wtyp as TypId, T.Typ, T.TypNapis, @status as StatusId, ST.Status
                        ,@pracId as IdPracownika, ISNULL(8 * P.EtatL / P.EtatM, 8) as GodzinZm
                        ,P.KadryId as PracLogo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Email
                        ,K.Id as KierId
                        ,K.KadryId as KierLogo, K.Nazwisko + ' ' + K.Imie as Kierownik
                        ,D.Nazwa as ProjektDzial
                        ,S.Nazwa as Stanowisko
                        ,@data as DataWniosku
                        ,@autorId as AutorId
                        ,AT.Nazwisko + ' ' + AT.Imie as Autor
                        ,KA.Id as KierAccId
                        ,KA.Nazwisko + ' ' + KA.Imie as KierAcc
                        ,null as PodTyp
                        ,null as Info
                        ,null as UzasadnieniePrac
                    from poWnioskiUrlopoweTypy T
                    left join poWnioskiUrlopoweStatusy ST on ST.Id = @status
                    left join Pracownicy P on P.Id = @pracId
                    left join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1 
                    left join Pracownicy K on K.Id = R.IdKierownika
                    left join Dzialy D on D.Id = P.IdDzialu
                    left join Stanowiska S on S.Id = P.IdStanowiska
                    left join Pracownicy AT on AT.Id = @autorId
                    outer apply (select * from dbo.fn_GetUpKierWithRight(@pracId, GETDATE(), 13, 1)) KA
                    where T.Id = @wtyp
                    "*/
                     , osoba == osPracownik ? App.KwitekPracId : "-1"
                     , stNew
                     , wnTyp
                     , db.strParam(Tools.DateToStr(DateTime.Today))   // podległość na dziś
                     , App.User.Id
                     ));
            }
            else
            {
                //---- edycja wniosku -----
                ds = db.getDataSet(String.Format(dsWniosek.UpdateCommand
                    /*
                    @"
                    select T.Typ, T.TypNapis, ST.Status
                        ,W.IdPracownika, ISNULL(8 * P.EtatL / P.EtatM, 8) as GodzinZm
                        ,P.KadryId as PracLogo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Email
                        ,K.Id as KierId
                        ,K.KadryId as KierLogo, K.Nazwisko + ' ' + K.Imie as Kierownik
                        ,D.Nazwa as Dzial
                        ,S.Nazwa as Stanowisko
                        ,Z.KadryId as ZastLogo 
                        ,W.IdZastepuje, ISNULL(Zastepuje, Z.Nazwisko + ' ' + Z.Imie + ISNULL(' (' + Z.KadryId + ')', '')) as Zastepuje
                        ,KA.Id as KierAccId
                        ,KA.Nazwisko + ' ' + KA.Imie as KierAcc
                        ,KZ.Nazwisko + ' ' + KZ.Imie as KierAccZast
                        ,A.Nazwisko + ' ' + A.Imie as KadryAcc
                        ,AT.Nazwisko + ' ' + AT.Imie as Autor
                        ,W.*
                    from poWnioskiUrlopowe W
                    left join poWnioskiUrlopoweTypy T on T.Id = W.TypId
                    left join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
                    left join Pracownicy P on P.Id = W.IdPracownika
                    left join Przypisania R on R.IdPracownika = P.Id and W.Od between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1 
                    left join Pracownicy K on K.Id = R.IdKierownika
                    left join Pracownicy Z on Z.Id = W.IdZastepuje
                    left join Pracownicy KA on KA.Id = ISNULL(W.IdKierAcc, (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)))
                    left join Pracownicy KZ on KZ.Id = W.IdKierAccZast
                    left join Pracownicy A on A.Id = W.DataKadryAcc
                    left join Pracownicy AT on AT.Id = W.AutorId
                    left join Dzialy D on D.Id = P.IdDzialu
                    left join Stanowiska S on S.Id = P.IdStanowiska
                    where W.Id = {0}
                    "*/
                    , wnId));  // podległość na początek urlopu
            }
            //-------------------------------------
            DataRow dr = db.getRow(ds);
            int status = db.getInt(dr, "StatusId", stRejected);
            if (wnTyp == -1) wnTyp = db.getInt(dr, "TypId", -1);
            WniosekTyp = wnTyp;
            WniosekId = wnId;
            StatusId = status;
            Mnoznik = db.getValue(dr, "GodzinZm").Replace(',', '.');  // <<< potem uwzględnić 7h inwalidów

            paWniosekU2.Attributes["class"] = "status" + status.ToString();

            //SqlDataSource4.SelectParameters["rootId"].DefaultValue = "0"; // <<< na razie, potem uzależnić od osoby
            SqlDataSource4.SelectParameters["rootId"].DefaultValue = App.User.Id; // moi pracownicy
            SqlDataSource4.SelectParameters["all"].DefaultValue = osoba == osAdmin ? "2" : "0";
            SqlDataSource1.SelectParameters["pracId"].DefaultValue = db.getValue(dr, "IdPracownika");

            //---------------------------------------
            dbField.FillData(wnControl, dr, wnTyp, osoba, status, queryMode ? dbField.moQuery : dbField.moNormal);

            if (status == stWaitingAcc && osoba == osKierownik && !queryMode)
                UzasadnienieKier.AddRqStar(true);
            if (String.IsNullOrEmpty(Email.Value)) Email.Visible = false;

            //---- powinno się to zrobić jescze jedną osobą: kierAcc lub prac -> wnioskujący (z rozróżnieniem czy mam ludzi pod i wchodzę jako kier ...), kier -> akceptujący
            bool acc = App.User.HasRight(AppUser.rWnioskiUrlopoweAcc);
            if (!acc)
            {
                btAccept.Visible = false;
                btReject.Visible = false;
            }

            btPrint.Visible = Lic.portalPrint && Lic.portalPrintWU;





            string autorId = db.getValue(dr, "AutorId");
            string pracId = db.getValue(dr, "IdPracownika");
            bool autor = autorId != pracId;
            //Autor.Visible = autor;
            if (!autor) Autor.State = dbField.stUnvisible;

            string kierId = db.getValue(dr, "KierId");
            string accId = db.getValue(dr, "KierAccId");
            bool vacc = accId != kierId;
            if (!vacc) KierAcc.State = dbField.stUnvisible;
            div1.Visible = autor || KierAcc.State != dbField.stUnvisible;  // bo moze byc niewidoczne ze statusów

            if (status == stWaitingAcc && osoba == osKierownik && autorId != App.User.Id)
                btDelete.Visible = false;

            if (!App.User.HasRight(AppUser.rWnioskiUrlopoweAcc))
                btAcceptSend.Visible = false;

            string podtyp = db.getValue(dr, "PodTyp");
            rblOkolDni.SelectedIndex = -1;

            paDniGodzin.Attributes["class"] = null;

            switch (wnTyp)
            {
                case wtUO:  // urlop okolicznościowy
                    /*
                    string v = Dni.Value;
                    if (v == "1" || v == "2")
                        rblOkolDni.SelectedValue = Dni.Value;
                    */

                    rblOkolDni.DataBind();
                    Tools.SelectItemByParam(rblOkolDni, 0, podtyp);

                    Dni.Enabled = false;
                    Do.Enabled = false;
                    bool ed = WnVisible3.State == dbField.stEdit;
                    rblOkolDni.Enabled = ed;
                    lbStar1.Visible = ed;
                    break;
                case wtO2h:
                    //ddlOpiekaDni.DataBind();
                    paDniGodzin.Attributes["class"] = "NoDisplay";

                    Dni.Enabled = false;
                    Do.Enabled = false;

                    ed = WnVisible8.State == dbField.stEdit;
                    ShowOpieka(ed);

                    int dni, godz;
                    if (wnId == -1)
                    {
                        dni = 1;
                        godz = 8;
                    }
                    else
                    {
                        dni = db.getInt(dr, "Dni", 1);
                        godz = db.getInt(dr, "Godzin", 8);
                    }
                    string sel = godz < 8 ? String.Format("{0}h{1}", dni, godz) : dni.ToString();   // 1,2 -> dni, 1h1,1h2..1h7 -> dni h godz
                    Tools.SelectItem(ddlOpiekaDni, sel);
                    SelectOpieka();
                    lbOpiekaDni.Text = godz < 8 ? String.Format("{0}h", godz) : dni.ToString();
                    break;
                case wtUZ:  // urlop na żądanie
                    Dni.Value = "1";
                    Dni.Enabled = false;
                    Do.Enabled = false;
                    break;
                case wtSW:  // wolne za święto
                    Dni.Value = "1";
                    Dni.Enabled = false;
                    Do.Enabled = false;
                    Swieto.DeValue.DateStr = db.getValue(dr, "Info");
                    //20150816 inicjujemy
                    if (wnId == -1)
                    {
                        DataRow drS = db.getDataRow(String.Format("select * from Kody where Typ='WNURLOP' and Kod = {0}", wtSW));   //11
                        if (drS != null)
                        {
                            DateTime? dt = db.getDateTime(drS["Parametr"]);
                            if (dt != null)
                            {
                                Swieto.Value = Tools.DateToStr(dt);
                                Swieto.Enabled = false;
                            }
                        }
                    }
                    break;
                default:
                    Dni.Enabled = true;
                    Do.Enabled = true;
                    break;
            }


            //---------------------------------------
            if (Od.State == dbField.stEdit && wnTyp != wtSW)   // dla odbioru za święto nie sprawdzam !!!
                StartDateCheck();
        }


        private void SetButtonVisible(bool edit)
        {
            /*
            btEdit.Visible = !edit;
            btSave.Visible = edit;
            btCancelEdit.Visible = edit;
             */
        }


        private void SetEditMode(bool edit)
        {
            //EditMode = edit; wywoływana w set property
            SetButtonVisible(edit);
            dbField.SetMode(wnControl, edit ? dbField.moEdit : dbField.moNormal);
            if (edit)
                Tools.AddClass(paWniosekU2, "editmode");
            else
                Tools.RemoveClass(paWniosekU2, "editmode");
            //----------------------------
            switch (WniosekTyp)
            {
                case wtUO:
                    rblOkolDni.Enabled = edit;
                    break;
                case wtO2h:
                    ShowOpieka(edit);
                    break;
            }

            btPrint.Visible = Lic.portalPrint && Lic.portalPrintWU;

            if (edit && WniosekTyp != wtSW)   // dla odbioru za święto nie sprawdzam !!!
                StartDateCheck();

            /*
            dbField.SetEditVisible(wnControl, WniosekTyp, Osoba, StatusId, edit);
            if (edit)
            {
            }
            else
            {
            }
             */ 
        }

        private string GetPodTyp()
        {
            switch (WniosekTyp)
            {
                case wtUO:
                    if (rblOkolDni.SelectedIndex == -1)
                        return null;
                    else
                        return Tools.GetLineParam(rblOkolDni.SelectedValue, 0);
                case wtOD:
                    // rozróżnić przełożony / pracownik
                    return null;

                default: 
                    return null;
            }
        }


        //-----------------------------------------------------------
        private void TriggerClose()
        {
            StopDateCheck();
            if (popupVisible) popupShow(false);
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

        public void Reload()
        {
            FillData(WniosekId, -1, Osoba, QueryMode);
        }

        private void DoClose(bool updated)
        {
            Updated = updated;
            TriggerClose();
        }

        public void _Clear()
        {
            WniosekId = -1;
            WniosekTyp = -2;
            //Reload();  <<< po co to ?
        }
        //-----------------------
        private int SumAbsencja(string pracId, int kodAbs, DateTime dOd, DateTime dDo)   // zakładam, ze jest dzielona na przełomie roku
        {
            string dni = db.getScalar(String.Format(@"
select SUM(IleDni) from Absencja 
where IdPracownika = {0} and Kod = {1}
and DataOd >= {2} and DataDo <= {3}
                ", pracId, kodAbs,
                 dOd.boy().ToStringDb(),
                 dOd.eoy().ToStringDb()));
            return Tools.StrToInt(dni, 0);
        }

#if IQOR
        private float? _GetUrlopLimit(string pracId, DateTime dOd, DateTime dDo)   // zaległy + proporcjonalnie do końca umowy - po zmianie zwraca przysługujący na dzien uwzględniając I pracę
        {
            return _GetUrlopLimit(pracId, dOd, dDo, "w");
        }

        private float? _GetUrlopLimit(string pracId, DateTime dOd, DateTime dDo, string typ2)   // limit w okresie
        {
            DataRow dr = db.getDataRow(String.Format(dsGetLimit.SelectCommand, pracId, typ2));
            if (dr == null)
                return null;  // nie ma akiego typu w limitach
            else
                return (float)db.getDouble(dr, "LimitDni");   // z godzin jest liczony, jeżeli godziny, to powinien n * 1/24 zwrócić
        }

        private float? GetUrlopLimitGodz(string pracId, DateTime dOd, DateTime dDo, string typ2)   // limit w okresie
        {
            DataRow dr = db.getDataRow(String.Format(dsGetLimit.SelectCommand, pracId, typ2));
            if (dr == null)
                return null;  // nie ma akiego typu w limitach
            else
                return (float)db.getDouble(dr, "LimitGodz");   // z godzin jest liczony, jeżeli godziny, to powinien n * 1/24 zwrócić
        }
#else
        private int GetUrlopLimit(string pracId, DateTime dOd, DateTime dDo)   // zaległy + proporcjonalnie do końca umowy
        {
            string sql = RepUrlop.GetPracUrlopSql(pracId, dOd, dDo);
            DataRow dr = db.getDataRow(sql);
            return db.getInt(dr, "Limit", 0);
        }

        private int GetUrlopLimit(string pracId, DateTime dOd, DateTime dDo, string typ2)   // limit w okresie
        {
            DataRow dr = db.getDataRow(String.Format(@"
select top 1 * from UrlopLimity 
where IdPracownika = {0} and UrlopTyp = '{1}' and DataOd <= {3} and {2} <= DataDo 
order by DataOd desc
                ", pracId, typ2, 
                 dOd.boy().ToStringDb(),
                 dOd.eoy().ToStringDb()));
            if (dr == null)
                return 0;
            else
                return db.getInt(dr, "Limit", 0);  // Wymiar - w roku, Limit ile jest do wykorzystania
        }
#endif
        //--------------------
        private bool KolidujeAbsencja(string pracId, DateTime dOd, DateTime dDo, int typ, out DataRow dr) 
        {
            dr = db.getDataRow(String.Format(@"
declare @dOd datetime  
declare @dDo datetime 
declare @pracId int
set @pracId = {0}
set @dOd = {1}
set @dDo = {2}
select top 1 A.DataOd as Od, A.DataDo as Do, A.IleDni as Dni, K.Symbol, K.Nazwa as Typ 
from Absencja A
left outer join AbsencjaKody K on K.Kod = A.Kod
where A.IdPracownika = @pracId 
and @dOd <= A.DataDo and @dDo >= A.DataOd
                ", pracId, dOd.ToStringDb(), dDo.ToStringDb()));

            return dr != null;  // wniosek powinien być najpierw - można póki co tylko tak

            /*
            if (dr != null)
            {
                DateTime aod = (DateTime)db.getDateTime(dr, "DataOd");
                DateTime ado = (DateTime)db.getDateTime(dr, "DataDo");
                string symbol = db.getValue(dr, "Symbol");
                if (aod != dOd || ado != dDo || symbol != 
            }
            return false;
             */
        }

        private bool KolidujeAbsencja2(string pracId, DateTime dOd, DateTime dDo, int typ, out DataRow dr)   //20150816 jak wniosek jest już zapisany, to absencja powinna być sprawdzana z uwzględnieniem <> typ 
        {
            dr = db.getDataRow(String.Format(@"
declare @od datetime  
declare @do datetime 
declare @pracId int
declare @kod int
set @pracId = {0}
set @od = {1}
set @do = {2}
select @kod = IdKodyAbs from poWnioskiUrlopoweTypy where Id = {3}
 
select top 1 *
from dbo.GetDates2(@od, @do) D
inner join Absencja A on A.IdPracownika = @pracId and D.Data between A.DataOd and A.DataDo and A.Kod != @kod 
                ", pracId, dOd.ToStringDb(), dDo.ToStringDb(), typ));

            return dr != null;  // wniosek powinien być najpierw - można póki co tylko tak
        }

        private int _CheckOkres(string pracId, DateTime dOd, DateTime dDo, out string info)  // na innych wnioskach, 1-absencja, 2-wniosek zatwierdzony, 3-wniosek oczekujący, 0-ok
        {
            const string fmt = "{0} do {1} {2} - {3}";
            int ret = 0;            
            info = null;
            DataRow dr;
            //----- absencje wprowadzone do systemu -----
            if (KolidujeAbsencja(pracId, dOd, dDo, WniosekTyp, out dr)) ret = 1;    // trzeba zablokować nawet na etapie składania
            else
            {
                //----- wnioski zaakceptowane -----
                const string sql = @"
declare @dOd datetime  
declare @dDo datetime 
declare @pracId int
declare @typ int
declare @status int
set @pracId = {0}
set @dOd = {1}
set @dDo = {2}
set @typ = {4}
set @status = {5}
select top 1 W.DataWniosku, W.Od, W.Do, W.Dni, T.Symbol, T.Typ from poWnioskiUrlopowe W 
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
where IdPracownika = @pracId and StatusId in ({3})
and @dOd <= Do and @dDo >= Od
and (W.Od <> @dOd or W.Do <> @dDo or W.TypId <> @typ or W.StatusId <> @status)
                ";
                dr = db.getDataRow(String.Format(sql, pracId, dOd.ToStringDb(), dDo.ToStringDb(), stAccepted, WniosekTyp, StatusId));  // nie ma sensu brac 4 bo powinny byc juz wprowadzone
                if (dr != null) ret = 2;   // trzeba zablokować na etapie składania lub nie
                else
                {
                    //----- wnioski czekające na akceptację -----
                    dr = db.getDataRow(String.Format(sql, pracId, dOd.ToStringDb(), dDo.ToStringDb(), stWaitingAcc, WniosekTyp, StatusId));
                    if (dr != null) ret = 3;   // nie blokuję
                }
            }
            if (dr != null)
                info = String.Format(
                    fmt, 
                    Tools.DateToStr(db.getDateTime(dr, "Od")), 
                    Tools.DateToStr(db.getDateTime(dr, "Do")), 
                    db.getValue(dr, "Symbol"), 
                    db.getValue(dr, "Typ"));
            return ret;
        }

#if IQOR
        private bool CheckLimit(string pracId, int wnTypId, DateTime dOd, DateTime dDo, int dni, out string limitstr)  // dawna CheckWymiar, out wymiar(usuwam) - przysługujący w roku/wg typu wniosku, limit - pozostały do wykorzystania
        {
            float? limit;
            float? limitgodz;
            
            bool naGodziny = false;
            int godz = 0;

            DataRow dr = db.getDataRow(String.Format("select * from poWnioskiUrlopoweTypy where Id = {0}", wnTypId));
            limit = null;
            if (dr != null)  // raczej niemożliwe zeby było inaczej
            {
                int? wnlimit = db.getInt(dr, "Limit");      // limit ogólny dla wniosku UW-26,UD-10,OP188-2,null - nielimitowany
                int kodAbs = db.getInt(dr, "IdKodyAbs", 0); // powinien być 
                string typ2 = db.getValue(dr, "Typ2");

                switch (wnTypId)
                {
                    case wtUW:
                    case wtUD:
                    //case wtSZ:    // = 5;  // Urlop szkoleniowy - czy też limitowane ? na razie wyłączam
                        limit = _GetUrlopLimit(pracId, dOd, dDo, typ2);
                        if (limit == null) limit = 0;   // jak nie ma limitu to nie przysługuje, docelowo dać parametr w poWnioskiUrlopoweTypy
                        break;
                    case _wtO2:     // = 4;  // Opieka nad zdrowym dzieckiem do lat 14 (art. 188 KP)
                    //case wtO2h:   // to samo co _wtO2
                        string sel = ddlOpiekaDni.SelectedValue;
                        if (IsGodz(sel))
                        {
                            naGodziny = true;
                            godz = GetGodz(sel);
                            limit = GetUrlopLimitGodz(pracId, dOd, dDo, typ2);
                            if (limit == null) limit = 0;   // jak nie ma limitu to nie przysługuje, docelowo dać parametr w poWnioskiUrlopoweTypy
                        }
                        else
                        {
                            limit = _GetUrlopLimit(pracId, dOd, dDo, typ2);
                            if (limit == null) limit = 0;   // jak nie ma limitu to nie przysługuje, docelowo dać parametr w poWnioskiUrlopoweTypy
                        }
                        break;
                    /*
                    case wtUO: // = 3;  // Urlop okolicznościowy 
                        break;
                    case wtOD: // = 6;  // Czas wolny za pracę w godzinach nadliczbowych (151(2) par.1) KP
                        break;
                    case wtUB: // = 7;  // Urlop bezpłatny
                        break;
                    case wtNU: // = 8;  // Nieobecność usprawiedliwiona
                        break;
                    case wtNN: // = 9;  // Nieobecność nieusprawiedliwiona                  
                        break;
                         */
                    case wtUZ: // = 2;  // Urlop na żądanie
                        float? limitUW = _GetUrlopLimit(pracId, dOd, dDo);   // UW pozostały do wykorzystania
                        if (limitUW != null && wnlimit != null)            // 4, jeżeli nie to standardowa ścieżka
                        {
                            /*
                            int wyk = SumAbsencja(pracId, kodAbs, dOd, dDo);   //UŻ
                            float minlimit = (float)(wnlimit < limitUW ? wnlimit : limitUW);
                            limit = minlimit - wyk;
                            if (limit < 0) limit = 0;                      // zabezpieczenie bo moze być UW pozostało 2 dni, UZ wykorzystałem 3 więc wynik < 0 a nie jest to nadbranie
                            */
                            int wyk = SumAbsencja(pracId, kodAbs, dOd, dDo); //UŻ
                            wnlimit -= wyk;
                            if (wnlimit < 0) wnlimit = 0;
                            limit = (float)(wnlimit < limitUW ? wnlimit : limitUW);
                            if (limit < 0) limit = 0;                      // zabezpieczenie bo moze być UW pozostało 2 dni, UZ wykorzystałem 3 więc wynik < 0 a nie jest to nadbranie <<<< 20160830 po staremu było potrzebne - chyba można usunąć
                        }
                        break;
                    default:
                        limit = _GetUrlopLimit(pracId, dOd, dDo, typ2);  // null - bez limitu, chyba ze by był wprowadzony
                        break;
                }

                if (naGodziny)
                {                
                    limitstr = String.Format("{0} godz.", limit);
                    return godz <= limit;
                }
                else
                {
                    if (limit == null)          // jak nie ma to za limit przyjmujemy z typu wniosku
                        if (wnlimit == null)    // nie ma 
                            limit = 9999;       // lub dni w roku ?
                        else                        
                            limit = wnlimit; 
                    //if (limit < 0) limit = 0;                   // zabezpieczenie, chociaz to pokazuje nadbrane urlopy ... - wyłączam
                    limitstr = String.Format("{0}", limit);
                    return dni <= limit;                
                }
            }
            limitstr = "0";      // tak lepiej bo sie szybko wyłapie błedy w konfiguracji
            return false;
        }
#else
        private bool CheckWymiar(string pracId, int wnTypId, DateTime dOd, DateTime dDo, int dni, out int wymiar, out int? limit)
        {
            DataRow dr = db.getDataRow(String.Format("select * from poWnioskiUrlopoweTypy where Id = {0}", wnTypId));
            if (dr != null)  // raczej niemożliwe zeby było inaczej
            {
                int wyk;
                limit = db.getInt(dr, "Limit");
                int kodAbs = db.getInt(dr, "IdKodyAbs", 0);  // powinien być 
                string typ2 = db.getValue(dr, "Typ2");
                switch (wnTypId)
                {
                    case wtUW: // = 1;  // Urlop wypoczynkowy
                        //wyk = SumAbsencja(pracId, kodAbs, dOd, dDo);
                        wymiar = GetUrlopLimit(pracId, dOd, dDo);   // nom+zal-wyk
                        return dni <= wymiar;
                    case wtUD: // = 11;  // Urlop dodatkowy
                        wyk = SumAbsencja(pracId, kodAbs, dOd, dDo);
                        wymiar = GetUrlopLimit(pracId, dOd, dDo, typ2) - wyk;
                        if (wymiar < 0) wymiar = 0;
                        return dni <= wymiar;
                    case wtUZ: // = 2;  // Urlop na żądanie
                        wymiar = GetUrlopLimit(pracId, dOd, dDo);   // nom+zal-wyk
                        if (limit != null)
                        {
                            wyk = SumAbsencja(pracId, kodAbs, dOd, dDo);
                            if ((int)limit < wymiar)
                                wymiar = (int)limit - wyk;
                            else
                            {
                                int przysluguje = (int)limit - wyk;
                                if (przysluguje < wymiar)
                                    wymiar = przysluguje;
                                //wymiar -= wyk;
                            }
                        }
                        return dni <= wymiar;
                        /*
                    case wtUO: // = 3;  // Urlop okolicznościowy 
                        break;
                    case wtO2: // = 4;  // Opieka nad zdrowym dzieckiem do lat 14 (art. 188 KP)
                        break;
                    case wtSZ: // = 5;  // Urlop szkoleniowy
                        break;
                    case wtOD: // = 6;  // Czas wolny za pracę w godzinach nadliczbowych (151(2) par.1) KP
                        break;
                    case wtUB: // = 7;  // Urlop bezpłatny
                        break;
                    case wtNU: // = 8;  // Nieobecność usprawiedliwiona
                        break;
                    case wtNN: // = 9;  // Nieobecność nieusprawiedliwiona                  
                        break;
                         */ 
                    default:
                        if (limit != null)   // jest podany limit
                        {
                            wyk = SumAbsencja(pracId, kodAbs, dOd, dDo);
                            wymiar = (int)limit - wyk;
                            return dni <= wymiar;
                        }
                        else                 // bez limitu
                        {
                            wymiar = 9999;   // ilość dni w roku ?
                            return true;
                        }
                }
            }
            wymiar = 0;
            limit = 0;
            return false;
        }
#endif

        private bool Validate(int status, bool saveOnly)   // errory-msg i return, ostrzezenia-confirm, tylko zapis np przy edycji zeby nie pytał o akceptację
        {
            bool valid = dbField.Validate(wnControl);
            DateTime? d1;
            DateTime? d2;
            int? dni;
            switch (WniosekTyp)
            {
                case wtSW:
                    d1 = Odbior.AsDate;
                    d2 = d1;
                    dni = 1;
                    break;
                /*
                case wtO2h:
                    d1 = Od.AsDate;
                    d2 = Do.AsDate;
                    if (IsGodz(Dni.AsString))  xxxx nie tak - dodać pole Godzin i dodać je do sprawdzania zmian - tam wypełniać ilości if będzie Ok!
                        dni = 1;
                    else
                        dni = Dni.AsInt;                    
                    break;
                */ 
                default:
                    d1 = Od.AsDate;
                    d2 = Do.AsDate;
                    dni = Dni.AsInt;
                    break;
            }
            int il;
            bool e1, e2, e3, e4, e5, e6, e7, e;
            DateTime dt1, dt2;
            if (valid && d1 != null && d2 != null && dni != null)
            {
                dt1 = (DateTime)d1;
                dt2 = (DateTime)d2;
                il = Worktime.GetIloscDniPrac(db.con, dt1, dt2);
                DateTime d0 = DateTime.Today.AddDays(-365 / 2);    //abstrakcyjna długość urlopu
                e1 = dt1 > dt2;
                e2 = (dt2 - dt1).TotalDays > 365 * 3;
                e3 = dt1 < d0;



                //testy 
                //e3 = false;




                e4 = WniosekTyp != wtSW && il != (int)dni;
                if (e4) Dni.AsInt = il;
                e5 = (int)dni == 0;

                e6 = WniosekTyp == wtUZ && StatusId == stNew && d1 > DateTime.Today;
                e7 = false;
                if (WniosekTyp == wtSW)
                {
                    DateTime? sw = Swieto.AsDate;
                    if (sw != null)
                    {
                        DateTime dsw = (DateTime)sw;
                        e7 = dsw == d1;
                    }
                }

                e = e1 || e2 || e3 || e5 || e6 || e7;   //e4 to warning
                Od.SetError(e1 || e2 || e3 || e6 || e7);
                Do.SetError(e1 || e2);
                Dni.SetError(e4 || e5);
            }
            else
            {
                e1 = false;
                e2 = false;
                e3 = false;
                e4 = false;
                e5 = false;
                e6 = false;
                e7 = false;
                e = false;
                il = 0;
                valid = false; // na wszelki wypadek
            }
            if (e3) ShowError("Niepoprawna data początkowa.");
            else if (e1) ShowError("Data końca wcześniejsza od początkowej.");
            else if (e2) ShowError("Zbyt długi okres trwania urlopu.");
            else if (e5) ShowError("Ilość dni musi być większa od zera.");
            else if (e6) ShowError("Data nie może być większa od dzisiejszej.");
            else if (e7) ShowError("Niepoprawna data.");
            else if (!valid) ShowError("Niepoprawne parametry.");
            else
            {
                //----- 2 poziom walidacji -----
                //----- ostrzeżenia -----  
                string msg = null;
                string info;
                
                dt1 = (DateTime)d1;
                dt2 = (DateTime)d2;
                int wymiar;
                
                int ilDniRob = (int)Dni.AsInt;
                //xxxxint ilDniRob = (int)dni;

#if IQOR
                string limit;
                bool w = CheckLimit(IdPracownika.Value, WniosekTyp, dt1, dt2, ilDniRob, out limit);   // w środku sprawdza też godziny
                if (!w)
                {
                    info = String.Format("Podana ilość dni jest większa niż przysługujący limit ({0}).", limit);
                    Tools.ShowError(info);
                    return false;
                }
#else
                int? limit;
                bool w = CheckWymiar(IdPracownika.Value, WniosekTyp, dt1, dt2, ilDniRob, out wymiar, out limit);
                if (!w)
                {
                    if (limit != null && ilDniRob > (int)limit)   // jezeli przekroczenie limitu to zawsze błąd
                    {
                        info = String.Format("Podana ilość dni jest większa niż przysługujący limit ({0}).", (int)limit);
                        Tools.ShowError(info);
                        return false;
                    }
                    else
                    {
                        info = String.Format("Podana ilość dni jest większa niż przysługująca/możliwa do wykorzystania ({0}).", wymiar);
                        if (false)//status == stAccepted)
                        {
                            Tools.ShowError(info);
                            return false;
                        }
                        else msg += info + "\n";
                    }
                }
#endif
                if (e4) msg += String.Format("Ilość dni roboczych wynikająca z dat różni się od wprowadzonej. Ustawiono wartość: {0}.\n", il);
                int w1 = _CheckOkres(IdPracownika.Value, dt1, dt2, out info);
                switch (w1)
                {
                    case 1:
                        Od.SetError(true);
                        Do.SetError(true);
                        ShowError(String.Format("Okres pokrywa się z wprowadzoną do systemu absencją: {0}.", info));
                        return false;
                    case 2:
                        if (status == stWaitingAcc)
                        {
                            Od.SetError(true);
                            Do.SetError(true);
                            ShowError(String.Format("Okres pokrywa się z zaakceptowanym wnioskiem: {0}.", info));
                            return false;
                        }
                        else
                            msg += String.Format("Okres pokrywa się z innym wnioskiem: {0}.\n", info);  // pracownik może klika do wyboru, kierownik nie zaakceptuje
                        break;
                    case 3:
                        msg += String.Format("Okres pokrywa się z innym wnioskiem: {0}.\n", info);  // pracownik może klika do wyboru, kierownik nie zaakceptuje
                        break;
                }
                //----- komunikat -----
                if (saveOnly)
                    return true;
                else
                    switch (status)
                    {
                        case stNew:
                            if (!String.IsNullOrEmpty(msg))
                                ShowConfirm("Uwaga!\n" + msg + "\nWysłać wniosek do akceptacji ?", btSend2, null);
                            else
                                ShowConfirm("Wysłać wniosek do akceptacji ?", btSend2, null);
                            break;
                        case stWaitingAcc:
                            if (!String.IsNullOrEmpty(msg))
                                ShowConfirm("Uwaga!\n" + msg + "\nZaakceptować wniosek ?", btAccept2, null);
                            else
                                ShowConfirm("Zaakceptować wniosek ?", btAccept2, null);
                            break;
                        default:
                            return true;   //<<<<<<<< wyjście z ok !!!!
                    }
            }
            return false;
        }

        /*
        private bool Validate(int status)   // errory-msg i return, ostrzezenia-confirm
        {
            bool valid = dbField.Validate(wnControl);
            DateTime? d1 = Od.AsDate;
            DateTime? d2 = Do.AsDate;
            int? dni = Dni.AsInt;
            int il;
            bool e1, e2, e3, e4, e5, e;
            DateTime dt1, dt2;
            if (valid && d1 != null && d2 != null && dni != null)
            {
                dt1 = (DateTime)d1;
                dt2 = (DateTime)d2;
                il = Worktime.GetIloscDniPrac(db.con, dt1, dt2);
                DateTime d0 = DateTime.Today.AddDays(-365 / 2);
                e1 = dt1 > dt2;
                e2 = (dt2 - dt1).TotalDays > 365 * 3;
                e3 = dt1 < d0;
                e4 = il != (int)dni;
                if (e4) Dni.AsInt = il;
                e5 = (int)dni == 0;
                e = e1 || e2 || e3 || e5;   //e4 to warning
                Od.SetError(e1 || e2 || e3);
                Do.SetError(e1 || e2);
                Dni.SetError(e4 || e5);
            }
            else
            {
                e1 = false;
                e2 = false;
                e3 = false;
                e4 = false;
                e5 = false;
                e = false;
                il = 0;
                valid = false; // na wszelki wypadek
            }
            if (e3) ShowError("Niepoprawna data początkowa.");
            else if (e1) ShowError("Data końca wcześniejsza od początkowej.");
            else if (e2) ShowError("Zbyt długi okres trwania urlopu.");
            else if (e5) ShowError("Ilość dni musi być większa od zera.");
            else if (!valid) ShowError("Niepoprawne parametry.");
            else
            {
                //----- 2 poziom walidacji -----
                dt1 = (DateTime)d1;
                dt2 = (DateTime)d2;
                int wymiar;
                if (!CheckWymiar(IdPracownika.Value, WniosekTyp, dt1, dt2, (int)Dni.AsInt, out wymiar))
                    ShowMessage(String.Format("Podana ilość dni jest większa niż przysługująca/możliwa do wykorzystania ({0}).", wymiar));
                else
                {
                    //----- ostrzeżenia -----  
                    string info;
                    string msg = null;
                    if (e4) msg += String.Format("Ilość dni wynikająca z dat różni się od wprowadzonej. Ustawiono wartość: {0}.\n", il);
                    int w1 = CheckOkres(IdPracownika.Value, dt1, dt2, out info);
                    switch (w1)
                    {
                        case 1:
                            Od.SetError(true);
                            Do.SetError(true);
                            ShowError(String.Format("Okres pokrywa się z wprowadzoną do systemu absencją: {0}.", info));
                            return false;
                        case 2:
                            if (status == stWaitingAcc)
                            {
                                Od.SetError(true);
                                Do.SetError(true);
                                ShowError(String.Format("Okres pokrywa się z zaakceptowanym wnioskiem: {0}.", info));
                                return false;
                            }
                            else
                                msg += String.Format("Okres pokrywa się z innym wnioskiem: {0}.\n", info);  // pracownik może klika do wyboru, kierownik nie zaakceptuje
                            break;
                        case 3:
                            msg += String.Format("Okres pokrywa się z innym wnioskiem: {0}.\n", info);  // pracownik może klika do wyboru, kierownik nie zaakceptuje
                            break;
                    }
                    //----- komunikat -----
                    switch (status)
                    {
                        case stNew:
                            if (!String.IsNullOrEmpty(msg))
                                ShowConfirm("Uwaga!\n" + msg + "\nWysłać wniosek do akceptacji ?", btSend2, null);
                            else
                                ShowConfirm("Wysłać wniosek do akceptacji ?", btSend2, null);
                            break;
                        case stWaitingAcc:
                            if (!String.IsNullOrEmpty(msg))
                                ShowConfirm("Uwaga!\n" + msg + "\nZaakceptować wniosek ?", btAccept2, null);
                            else
                                ShowConfirm("Zaakceptować wniosek ?", btAccept2, null);
                            break;
                        default:
                            return true;   //<<<<<<<< wyjście z ok !!!!
                    }
                }
            }
            return false;
        }
         */



        private bool _ValidateAcc()
        {
            bool e2 = IdPracownika.Value == App.User.OriginalId;
            if (e2) ShowError("Akceptacja własnego wniosku nie jest możliwa. Proszę skontaktować się z działem HR.");
            else
                return Validate(stWaitingAcc, false);
            return false;
        }

        private bool ValidateRej()
        {
            bool e1 = String.IsNullOrEmpty(UzasadnienieKier.Value.Trim());
            bool e2 = IdPracownika.Value == App.User.OriginalId;
            if (e2) ShowError("Odrzucenie własnego wniosku nie jest możliwe. Proszę skontaktować się z działem HR.");
            else
            {
                UzasadnienieKier.SetError(e1);
                if (e1) ShowError("Proszę podać uzasadnienie odrzucenia wniosku.");
                else
                    return true;
            }
            return false;
        }
        //----------------------
        /*
        private object getValue(string field)
        {
            Control cnt = FindControl("ed" + field);
            if (cnt != null)
                return getValue(cnt);
            else
                return db.NULL;
        }
        */
        private object getValue(Control cnt)
        {
            if (cnt is TextBox)
            {
                TextBox tb = cnt as TextBox;
                if (tb.MaxLength != 0)
                    return db.strParam(db.sqlPut(tb.Text, tb.MaxLength));
                else
                    return db.strParam(db.sqlPut(tb.Text));
            }
            else if (cnt is DateEdit) return db.nullStrParam(((DateEdit)cnt).DateStr);
            else if (cnt is CheckBox) return ((CheckBox)cnt).Checked ? 1 : 0;
            else if (cnt is DropDownList) return db.nullParam(((DropDownList)cnt).SelectedValue);
            else return db.NULL;
        }

        /*      
            string dd = Worktime.Round05(d1 + d2 - d3, 2).ToString().Replace(".", ",");
            if (dd.EndsWith(".00")) dd = dd.Remove(dd.Length - 3);
        */



        private bool Update1()
        {
            if (WniosekId == -1)
            {
                /*
                int id = db.insert("strWnioski", true, true, 0,
                    "Typ,Status,Nazwa,DataOd,DataDo,StrParentId,StrId," +
                    "Kod,ccId,Cel,Zadania,RaportujeDoId,Uwagi," +
                    "WnioskujacyId,DataWniosku,DataWyslania,Opinia,AkceptujacyId,DataAcc," +
                    "KlientId,LiniaId,BULId,PMId,CMId,DzialInzId,DzialJakosciId,DzialZakupowId,DzialPlanowaniaId," +
                    "Foto,PublikujacyId,DataPublikacji,SchematId,IdPublikacji,ZarzadzajacyId,StrTypId,StrChildren,SubDelete",
                    WniosekTyp, Wniosek.stTemp, getValue(edNazwa), getValue(edDataOd), getValue(edDataDo),
                    db.nullParam(cntStrParent.SelectedId), db.nullParam(cntStrSelect.SelectedId),
                    getValue(edKod), getValue(edccId),
                    getValue(edCel), getValue(edZadania), db.NULL,
                    getValue(edUwagi),
                    //App.User._OriginalId, 
                    App.User.Id,
                    "GETDATE()", db.NULL,
                    getValue(edOpinia), db.NULL, db.NULL,
                    db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL,
                    db.NULL, db.NULL, db.NULL,
                    SchematId, db.NULL, getValue(edZarzadzajacyId), getValue(edStrTypId),
                    db.strParam(GetStrChildren()), getValue(edSubDelete));
                return id != -1;
                 */ 
            }
            else
            {
                /*
                bool ok = db.update("strWnioski", 0,
                    "Nazwa,DataOd,DataDo,StrParentId,StrId,Kod,ccId,Cel,Zadania,RaportujeDoId,Uwagi," +
                    "DataWyslania,Opinia,AkceptujacyId,DataAcc," +
                    "KlientId,LiniaId,BULId,PMId,CMId,DzialInzId,DzialJakosciId,DzialZakupowId,DzialPlanowaniaId," +
                    "Foto,ZarzadzajacyId,StrTypId,StrChildren,SubDelete",
                    "Id=" + WniosekId,
                    getValue(edNazwa), getValue(edDataOd), getValue(edDataDo),
                    db.nullParam(cntStrParent.SelectedId), db.nullParam(cntStrSelect.SelectedId),
                    getValue(edKod), getValue(edccId),
                    getValue(edCel), getValue(edZadania), db.NULL,
                    getValue(edUwagi),
                    db.NULL,
                    getValue(edOpinia), db.NULL, db.NULL,
                    db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL,
                    db.NULL,
                    getValue(edZarzadzajacyId), getValue(edStrTypId),
                    db.strParam(GetStrChildren()), getValue(edSubDelete));
                return ok;
                 */ 
            }
            return true;
        }
        //----------
        private bool UpdateSend()
        {
            int dni = (int)Dni.AsInt;
            
            double godz = dni * Tools.StrToDouble(Mnoznik, 8);  //<<<<< uwaga 8 jak nie ma 

            int typ = WniosekTyp;
            string podtyp = GetPodTyp();
            switch (typ)
            {
                case wtO2h:
                    string sel = ddlOpiekaDni.SelectedValue;
                    if (IsGodz(sel))
                        godz = GetGodz(sel);
                    break;
            }
            
            
            if (IdPracownika.dbValue == "0")
            {
                Log.Error(Log.WNIOSEKURLOPOWY, "IdPracownika = 0", String.Format("1:{0} 2:{1} 3:{2}", App.User.Id, App.KwitekPracId, WniosekTyp));
            }


            int id;
            switch (typ)
            {
                default:
                    if (IdPracownika.State == dbField.stEdit)
                        id = dbField.dbInsert(wnControl, "poWnioskiUrlopowe",
                            "TypId,DataWniosku,AutorId,IdPrzelozony,ProjektDzial,Stanowisko,Godzin,StatusId,PodTyp",
                            WniosekTyp, "GETDATE()", App.User.Id, 0, 0, 0, godz, stWaitingAcc, db.nullParam(podtyp));
                    else
                        id = dbField.dbInsert(wnControl, "poWnioskiUrlopowe",
                            "TypId,DataWniosku,AutorId,IdPrzelozony,ProjektDzial,Stanowisko,Godzin,StatusId,IdPracownika,PodTyp",
                            WniosekTyp, "GETDATE()", App.User.Id, 0, 0, 0, godz, stWaitingAcc, IdPracownika.dbValue, db.nullParam(podtyp));
                    break;
                case wtSW:
                    if (IdPracownika.State == dbField.stEdit)
                        id = dbField.dbInsert(wnControl, "poWnioskiUrlopowe",
                            "TypId,DataWniosku,AutorId,IdPrzelozony,ProjektDzial,Stanowisko,Godzin,StatusId,PodTyp,Do,Dni",
                            WniosekTyp, "GETDATE()", App.User.Id, 0, 0, 0, godz, stWaitingAcc, db.nullParam(podtyp), db.strParam(Tools.DateToStrDb((DateTime)Odbior.AsDate)), dni);
                    else
                        id = dbField.dbInsert(wnControl, "poWnioskiUrlopowe",
                            "TypId,DataWniosku,AutorId,IdPrzelozony,ProjektDzial,Stanowisko,Godzin,StatusId,IdPracownika,PodTyp,Do,Dni",
                            WniosekTyp, "GETDATE()", App.User.Id, 0, 0, 0, godz, stWaitingAcc, IdPracownika.dbValue, db.nullParam(podtyp), db.strParam(Tools.DateToStrDb((DateTime)Odbior.AsDate)), dni);
                    break;            
            }


            if (id == -1)
                Tools.ShowErrorLog(Log.WNIOSEKURLOPOWY, "Wniosek Urlopowy - Błąd podczas zapisu", db.LastInsertSql);
            else
                Mailing.EventWniosekUrlopowy(Mailing.maWU_SENT, id.ToString());
            return id != -1;
        }

        private void GetDniGodz(out int dni, out double godz)
        {
            dni = (int)Dni.AsInt;
            godz = dni * Tools.StrToDouble(Mnoznik, 8);  //<<<<< uwaga 8 jak nie ma 
            switch (WniosekTyp)
            {
                case wtO2h:
                    string sel = ddlOpiekaDni.SelectedValue;
                    if (IsGodz(sel))
                        godz = GetGodz(sel);
                    break;
            }
        }

        private bool UpdateAcc(bool _acc)
        {
            bool ok;
            string sql;
            switch (StatusId)
            {                                               // do schedulera dodać aktualizacje stanowiska/zmienić pobieranie/wyszukać IdStanowiska
                case stNew: // jak kier albo adm            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< dorobić zapis jak nie są wszystkie pola widoczne !!!, można też dodać inicjowanie daty dla odbiow dnia wolnego za świeto
                    /*
                    int dni = (int)Dni.AsInt;
                    double godz = dni * Tools.StrToDouble(Mnoznik, 8);  //<<<<< uwaga 8 jak nie ma 

                    int typ = WniosekTyp;
                    string podtyp = GetPodTyp();
                    switch (typ)
                    {
                        case wtO2h:
                            string sel = ddlOpiekaDni.SelectedValue;
                            if (IsGodz(sel))
                                godz = GetGodz(sel);
                            break;
                    }
                    */
                    int typ = WniosekTyp;
                    string podtyp = GetPodTyp();
                    int dni;
                    double godz;
                    GetDniGodz(out dni, out godz);

                    string fields = "TypId,DataWniosku,AutorId," +
                        "IdPrzelozony,ProjektDzial,Stanowisko," +
                        "Godzin,StatusId," +
                        "IdKierAcc,IdKierAccZast,DataKierAcc,PodTyp";
                    int id;
                    if (typ == wtSW)
                        id = dbField.dbInsert(wnControl, "poWnioskiUrlopowe", fields + ",Do,Dni",
                            WniosekTyp, "GETDATE()", App.User.Id, 0, 0, 0, godz, stAccepted, 
                            App.User.Id,                        // jak zastepstwo to id zastepowanego
                            db.nullParam(App.User.OriginalId),  // id zastepującego
                            "GETDATE()", db.nullParam(podtyp)
                            ,Odbior.dbValue
                            ,dni.ToString()
                            );
                    else
                        id = dbField.dbInsert(wnControl, "poWnioskiUrlopowe", fields,
                            WniosekTyp, "GETDATE()", App.User.Id, 0, 0, 0, godz, stAccepted,
                            App.User.Id,                        // jak zastepstwo to id zastepowanego
                            db.nullParam(App.User.OriginalId),  // id zastepującego
                            "GETDATE()", db.nullParam(podtyp)
                            );
                    WniosekId = id;
                    ok = id != -1;
                    sql = db.LastInsertSql;
                    if (ok)
                    {
                        //if (Osoba != osAdmin)   // nie ma sensu wysyłać sobie maila ...
                        Mailing.EventWniosekUrlopowy(Mailing.maWU_ACCEPTED, WniosekId.ToString());      // tu zawsze akceptacja bo to nowy wniosek
                    }
                    break;
                case stAccepted:      // tu zawsze będzie odrzucenie, acc ustawia import z asseco i pokojarzenie
                case stAcceptedHR:
                    ok = dbField.dbUpdate(wnControl, "poWnioskiUrlopowe", "Id=" + WniosekId,
                        "StatusId,IdKierAcc,DataKierAcc,IdKadryAcc,DataKadryAcc",
                        _acc ? stAccepted : stRejected,


                        db.nullParam(App.User.OriginalId),  // id zastepującego  <<< do weryfikacji !!! bo nadpisuje przełożonego !!!
                        "GETDATE()",


                        db.nullParam(App.User.OriginalId),  // id zastepującego
                        "GETDATE()");
                    sql = db.LastUpdateSql;
                    if (ok)
                        Mailing.EventWniosekUrlopowy(_acc ? Mailing.maWU_ACCEPTED : Mailing.maWU_REJECTED, WniosekId.ToString());      // tu zawsze akceptacja bo to nowy wniosek
                    break;
                default:
                    ok = dbField.dbUpdate(wnControl, "poWnioskiUrlopowe", "Id=" + WniosekId,
                        "StatusId,IdKierAcc,IdKierAccZast,DataKierAcc",
                        _acc ? stAccepted : stRejected,
                        App.User.Id,                        // jak zastepstwo to id zastepowanego
                        db.nullParam(App.User.OriginalId),  // id zastepującego
                        "GETDATE()");
                    sql = db.LastUpdateSql;
                    if (ok)
                        Mailing.EventWniosekUrlopowy(_acc ? Mailing.maWU_ACCEPTED : Mailing.maWU_REJECTED, WniosekId.ToString());      // tu zawsze akceptacja bo to nowy wniosek
                    break;
            }
            if (!ok)
                Tools.ShowErrorLog(Log.WNIOSEKURLOPOWY, "Wniosek Urlopowy - Błąd podczas zapisu", sql);
            return ok;
        }

        private bool Update()
        {
            DataRow prevdr = db.getDataRow("select * from poWnioskiUrlopowe where Id = " + WniosekId);

            int dni;
            double godz;
            GetDniGodz(out dni, out godz);
            
            bool ok;
            int typ = WniosekTyp;
            string podtyp = GetPodTyp();
            switch (typ)
            {
                default:
                    //ok = dbField.dbUpdate(wnControl, "poWnioskiUrlopowe", "Id=" + WniosekId, "PodTyp", db.nullParam(podtyp));
                    ok = dbField.dbUpdate(wnControl, "poWnioskiUrlopowe", "Id=" + WniosekId, "PodTyp,Godzin", db.nullParam(podtyp), godz);
                    break;
                case wtSW:
                    ok = dbField.dbUpdate(wnControl, "poWnioskiUrlopowe", "Id=" + WniosekId, "PodTyp,Do", db.nullParam(podtyp), db.strParam(Tools.DateToStrDb((DateTime)Odbior.AsDate)));
                    break;
            }
            
            string sql = db.LastUpdateSql;
            if (ok)
            {
                if (StatusId != stAcceptedHR)    // na tym etapie nie ma już potrzeby informować o zmianach ... <<<< do weryfikacji 
                    Mailing.EventWniosekUrlopowy(Mailing.maWU_CHANGED, WniosekId.ToString(), prevdr);
            }
            else
                Tools.ShowErrorLog(Log.WNIOSEKURLOPOWY, "Wniosek Urlopowy - Błąd podczas zapisu (update)", sql);
            return ok;
        }

        private void RestoreValues()
        {
            dbField.Restore(wnControl, false);
        }
        /*
        private bool UpdateSend()
        {
            int dni = (int)Dni.AsInt;
            double godz = dni * Tools.StrToDouble(Mnoznik, 8);  //<<<<< uwaga 8 jak nie ma 
            int id = db.insert("poWnioskiUrlopowe", true, true, 0,
                "TypId,DataWniosku,AutorId,IdPracownika,IdZastepuje,Zastepuje,Od,Do,Dni,Godzin," +
                "IdPrzelozony,ProjektDzial,Stanowisko,Info,UzasadnieniePrac," +
                "StatusId",
                WniosekTyp, "GETDATE()", App.User.Id, IdPracownika.dbValue, 
                db.NULL,
                Zastepuje.dbValue, 
                Od.dbValue, Do.dbValue, dni, godz, 
                0,0,0, 
                Info.dbValue, UzasadnieniePrac.dbValue, 
                stWaitingAcc);
            if (id == -1)
                Tools.ShowErrorLog(Log.WNIOSEKURLOPOWY, "Wniosek Urlopowy - Błąd podczas wysyłania", db.LastInsertSql);
            else
                Mailing.EventWniosekUrlopowy(Mailing.maWUSENT, id.ToString());
            return id != -1;
        }
        
        private bool UpdateAcc(bool acc)
        {
            bool ok = db.update("poWnioskiUrlopowe", 0,
                "StatusId,IdKierAcc,IdKierAccZast,DataKierAcc",
                "Id=" + WniosekId,
                acc ? stAccepted : stRejected,
                App.User.Id,                        // jak zastepstwo to id zastepowanego
                db.nullParam(App.User.OriginalId),  // id zastepującego
                "GETDATE()");
            return ok;
        }
        */

        private bool UpdateCofnij()
        {
            switch (StatusId)
            {
                default:
                case stNew:
                    return false;  // nie ma gdzie cofać
                case stWaitingAcc:
                    return db.update("poWnioskiUrlopowe", 0, "StatusId,DataWniosku", "Id=" + WniosekId,
                                     stNew, db.NULL);
                case stAccepted:
                case stRejected:
                    return db.update("poWnioskiUrlopowe", 0, "StatusId,IdKierAcc,IdKierAccZast,DataKierAcc,UzasadnienieKier", "Id=" + WniosekId,
                                     stWaitingAcc, db.NULL, db.NULL, db.NULL, db.NULL, db.NULL);
                case stAcceptedHR:
                    return db.update("poWnioskiUrlopowe", 0, "StatusId,IdKadryAcc,DataKadryAcc,Wprowadzony", "Id=" + WniosekId,
                                     stAccepted, db.NULL, db.NULL, 0);
            }
        }

        private bool Delete()
        {
            string wn = db.ToString(String.Format("select * from poWnioskiUrlopowe where Id = {0}", WniosekId), null, "\r\n", null);
            Log.Info(Log.WNIOSEKURLOPOWYDEL, "Usunięcie wniosku urlopowego", wn);

            Mailing.EventWniosekUrlopowy(Mailing.maWU_DEL, WniosekId.ToString());  // powinno być po jak wszystko pojdzie ok, tylko trzeba mail_data zebrac wczesniej ... <<<< na razie tak

            bool ok = db.execSQL(String.Format("delete from poWnioskiUrlopowe where Id = {0}", WniosekId));
            if (ok)
            {
                //Mailing.EventWniosekUrlopowy(Mailing.maWU_DEL, WniosekId.ToString(), prev);
            }
            else
                Tools.ShowErrorLog(Log.WNIOSEKURLOPOWY, "Wniosek Urlopowy - Błąd podczas usuwania", WniosekId.ToString());
            return ok;
        }
        //-----------------------------------------------
        protected void IdPracownika_Changed(object sender, EventArgs e)
        {
            switch (IdPracownika.Value)
            {
                case "-99":
                    //IdPracownika.DdlValue.Items[1].Value = "-88";
                    //IdPracownika.DdlValue.Items[1].Text = "Pokaż moich pracowników";
                    SqlDataSource4.SelectParameters["all"].DefaultValue = "1";
                    IdPracownika.DdlValue.DataBind();
                    break;
                case "-88":
                    //IdPracownika.DdlValue.Items[1].Value = "-99";
                    //IdPracownika.DdlValue.Items[1].Text = "Pokaż wszystkich pracowników";
                    SqlDataSource4.SelectParameters["all"].DefaultValue = "0";
                    IdPracownika.DdlValue.DataBind();
                    break;
                default:
                    string pid = IdPracownika.Value;
                    if (!String.IsNullOrEmpty(pid))
                    {
                        DataRow dr = db.getDataRow(String.Format(@"
select ISNULL(8 * P.EtatL / P.EtatM, 8) as GodzinZm
    ,P.KadryId as PracLogo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Email
    ,P.KierId
    ,P.KierKadryId as KierLogo, P.KierownikNI as Kierownik
    ,D.Nazwa as Dzial
    ,S.Nazwa as Stanowisko
    ,KA.Id as KierAccId
    ,KA.Nazwisko + ' ' + KA.Imie as KierAcc
from VPrzypisaniaNaDzis P 
left outer join Dzialy D on D.Id = P.IdDzialu
left outer join Stanowiska S on S.Id = P.IdStanowiska
outer apply (select * from dbo.fn_GetUpKierWithRight({0}, GETDATE(), 13, 1)) KA
where P.Id = {0}", pid));
                        if (dr != null)
                        {
                            PracLogo.Value = db.getValue(dr, "PracLogo");
                            string email = db.getValue(dr, "Email");
                            Email.Value = email;
                            Email.Visible = !String.IsNullOrEmpty(email);

                            //ProjektDzial.Value = db.getValue(dr, "Dzial");
                            Dzial.Value = db.getValue(dr, "Dzial");
                        
                            Stanowisko.Value = db.getValue(dr, "Stanowisko");
                            Kierownik.Value = db.getValue(dr, "Kierownik");

                            string kierId = db.getValue(dr, "KierId");
                            string accId = db.getValue(dr, "KierAccId");

                            KierAcc.Value = db.getValue(dr, "KierAcc");
                            if (kierId == accId)
                                KierAcc.State = dbField.stUnvisible;
                            else
                                KierAcc.State = dbField.stQuery;                        
                        }
                    }
                    else
                    {
                        PracLogo.Value = null;
                        Email.Value = null;
                        Email.Visible = false;
                        
                        //ProjektDzial.Value = null;
                        Dzial.Value = null;
                        
                        Stanowisko.Value = null;
                        Kierownik.Value = null;
                        KierAcc.State = dbField.stUnvisible;
                    }
                    break;
            }
        }
        //-----------------------------------------------

        //-----------------------------------------------
        protected void btSend_Click(object sender, EventArgs e)
        {
            if (Validate(stNew, false))  // js wywoła doClick(btSend2)
                if (UpdateSend())
                    DoClose(true);
        }

        protected void btSend2_Click(object sender, EventArgs e)
        {
            if (UpdateSend())
                DoClose(true);
        }
        //---
        protected void btAccept_Click(object sender, EventArgs e)
        {
            if (App.User.HasRight(AppUser.rWnioskiUrlopoweAcc))
            {
                //if (Validate(astWaitingAcc, false))
                if (_ValidateAcc())
                    if (UpdateAcc(true))
                        DoClose(true);
            }
            /*
            else
            {
                if (Validate(stNew))    // to samo co btSend_Click
                    if (UpdateSend())
                        DoClose(true);
            }
             */ 
        }

        protected void btAccept2_Click(object sender, EventArgs e)
        {
            if (UpdateAcc(true))
                DoClose(true);
        }
        //---
        protected void btReject_Click(object sender, EventArgs e)
        {
            if (ValidateRej())
                if (UpdateAcc(false))
                    DoClose(true);
        }
        //---
        protected void btCofnij_Click(object sender, EventArgs e)
        {
            if (UpdateCofnij())
                Reload();
        }

        protected void btCancelWniosek_Click(object sender, EventArgs e)
        {
            // anulowanie wniosku na dowolnym z etapów przez pracownika/kierownika <<<< dodać zamiast usuń!!!
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Validate(StatusId, true))
                if (Update())
                {
                    EditMode = false;
                    DoClose(true);
                }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            EditMode = true;
            //DataBind();
        }

        protected void btCancelEdit_Click(object sender, EventArgs e)
        {
            RestoreValues();
            EditMode = false;
            //DataBind();
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (Delete())
                DoClose(true);
        }

        protected void btAbsencje_Click(object sender, EventArgs e)
        {
            string pid = IdPracownika.Value;
            if (!String.IsNullOrEmpty(pid))
                zoomUrlopy.Show(pid);
            else
                Tools.ShowMessage("Proszę wybrać pracownika.");
            //string upa = Parent.Parent.Parent.ClientID;
            //upa = upa.Substring(0, upa.Length - 1) + "6";            
            //zoomUrlopy.Show(IdPracownika.Value, UpdatePanel1.ClientID);  // updatePanel
        }

        //---
        protected void btCancel_Click(object sender, EventArgs e)
        {
            DoClose(false);
        }

        protected void btClose_Click(object sender, EventArgs e)
        {
            DoClose(false);
        }
        //--------------------------------------
        protected void rblOkolDni_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Dni.Value = rblOkolDni.SelectedValue;
            Dni.Value = Tools.GetLineParam(rblOkolDni.SelectedValue, 1);    // 0 - kod, 1 - ilość
        }

        private bool IsGodz(string dni)
        {
            return Tools.Substring(dni, 1, 1) == "h";
        }

        private int GetGodz(string dni)
        {                                                   // 0   1 2
            return Tools.StrToInt(dni.Substring(2, 1), 8);  // dni h godzin
        }

        private void ShowOpieka(bool edit)
        {
            ddlOpiekaDni.Visible = edit;
            lbStar2.Visible = edit;
            lbOpiekaDni.Visible = !edit;
        }

        private void SelectOpieka()
        {
            string sel = ddlOpiekaDni.SelectedValue;
            bool g = IsGodz(sel);
            Dni.Value = sel.Substring(0, 1);
            if (g)
            {
                //Mnoznik = ((float)0.125 * Tools.StrToInt(ilosc, 8)).ToString();
                Godzin.Value = GetGodz(sel).ToString();  // to w sumie też nie jest potrzebne...
            }
            else
            {
            }
        }

        protected void ddlOpiekaDni_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectOpieka();
        }

        //--------------------------------------
        public int WniosekTyp
        {
            set { ViewState["wntyp"] = value; }
            get { return Tools.GetInt(ViewState["wntyp"], -1); }
        }

        public int WniosekId
        {
            set { ViewState["wnid"] = value; }
            get { return Tools.GetInt(ViewState["wnid"], -1); }
        }

        public int Osoba
        {
            set { ViewState["osoba"] = value; }
            get { return Tools.GetInt(ViewState["osoba"], -1); }
        }

        public int StatusId
        {
            set { ViewState["statusid"] = value; }
            get { return Tools.GetInt(ViewState["statusid"], -1); }
        }



        //-----------------
        public bool IsEditMode()
        {
            return EditMode;
        }

        public bool EditMode
        {
            set 
            { 
                ViewState["editing"] = value;
                SetEditMode(value);
            }
            get { return Tools.GetBool(ViewState["editing"], false); }
        }

        public bool QueryMode
        {
            set { ViewState["querym"] = value; }
            get { return Tools.GetBool(ViewState["querym"], false); }
        }
        //-----------------

       


        /*
        public double Mnoznik
        {
            set { hidMnoznik.Value = value.ToString(); }
            get { return Tools.StrToDouble(hidMnoznik.Value, 0); }
        }
        */
        public string Mnoznik
        {
            set { hidMnoznik.Value = value; }
            get { return hidMnoznik.Value; }
        }

        public bool Initialized
        {
            set { ViewState["initok"] = value; }
            get { return Tools.GetBool(ViewState["initok"], false); }
        }

        public Control wnControl
        {
            get { return Controls[0]; }
        }
    }
}





/*








        private void SolveOd()
        {
            int? dni = Dni.AsInt;
            if (Od.DeValue.IsValid && dni != null && dni >= 0 && dni <= 999)
            {
                DateTime dt = Worktime.GetIloscDniPracDataOd(db.con, (DateTime)Do.DeValue.Date, (int)dni);
                Do.AsDate = dt;
                Godzin.AsDouble = dni * 8; // <<<< na razie zeby cos pokazac
                Tools.ExecOnStart2("wnsolvdni", "checkUrlopDniFreeze();");
            }
        }

        private void SolveDo()
        {
            int? dni = Dni.AsInt;
            if (Od.DeValue.IsValid && dni != null && dni >= 0 && dni <= 999)
            {
                DateTime dt = Worktime.GetIloscDniPracDataDo(db.con, (DateTime)Od.DeValue.Date, (int)dni);
                Do.AsDate = dt;
                Godzin.AsDouble = dni * 8; // <<<< na razie zeby cos pokazac
                Tools.ExecOnStart2("wnsolvdni", "checkUrlopDniFreeze();");
            }
        }

        private void SolveDni()
        {
            if (Od.DeValue.IsValid && Do.DeValue.IsValid && (DateTime)Do.AsDate >= (DateTime)Od.AsDate)
            {
                int dni = Worktime.GetIloscDniPrac(db.con, Od.DeValue.DateStr, Do.DeValue.DateStr);
                Dni.AsInt = dni;
                Godzin.AsDouble = dni * 8; // <<<< na razie zeby cos pokazac
                Tools.ExecOnStart2("wnsolvdni", "checkUrlopDniFreeze();");
            }
        }

















 
        protected void btPrzelicz_Click(object sender, EventArgs e)
        {
            DateTime? dataOd = Od.AsDate;                        
            DateTime? dataDo = Do.AsDate;                        
            int ? ilDni = Dni.AsInt;

            DateEdit deOd = Od.DeValue;
            DateEdit deDo = Do.DeValue;
            TextBox tb = Dni.TbValue;   
            
            /*
            switch (hidFocused.Value)
            {
                default: 
                case "1":
                    if (Do.DeValue.IsValid) SolveDni();
                    else SolveDo();
                    Od.DeValue.EditBox.Focus();
                    //Tools.ExecOnStart2("wnsetfoc", String.Format("focusEnd('{0}')", Od.DeValue.EditBox.ClientID));
                    break;
                case "2":
                    if (Od.DeValue.IsValid) SolveDni();
                    else SolveOd();
                    Do.DeValue.EditBox.Focus();
                    break;
                case "3":
                    if (Od.DeValue.IsValid) SolveDo();
                    else SolveOd();
                    Dni.TbValue.Focus();
                    break;
            }
            * /
            
            //StartDateCheck();
        }
 
 */


/*
 

            // 0-nowy 1-waitacc 2-rej 3-acc 4-acc,wprowadzony
            // 0-niewidoczne 1-query 2-edycja, na później: 3-edycja required
            // prac, kier, admin

            /*
            lbDataWniosku.Text = 
            lbPracownik
            lbPracLogo
            lbProjektDzial
            lbStanowisko
            lbTypNazwa
            */

            /*
            bool v1 = false;
            GetValue(this, dr, "Status");
            //GetValue(this, dr, "DataWniosku");
            GetValue(this, dr, "Pracownik");
            GetValue(this, dr, "PracLogo");
            GetValue(this, dr, "ProjektDzial");
            GetValue(this, dr, "Stanowisko");
            GetValue(this, dr, "Kierownik");
            //GetValue(this, dr, "Typ");
            //GetValue(this, dr, "TypNapis");

            GetValue(ref v1, this, dr, osoba, status, "11111", "11111", "10000", "TypNapis");
            GetValue(ref v1, this, dr, osoba, status, "00000", "00000", "02222", "TypId");


            GetValue(ref v1, this, dr, osoba, status, "21111", "01111", "11111", "Od");
            GetValue(ref v1, this, dr, osoba, status, "21111", "01111", "11111", "Do");
            GetValue(ref v1, this, dr, osoba, status, "21111", "01111", "11111", "Dni");
            GetValue(ref v1, this, dr, osoba, status, "11111", "01111", "11111", "Godzin");

            GetValue(ref v1, this, dr, osoba, status, "00100", "02100", "02100", "Uzasadnienie");
            GetValue(ref v1, this, dr, osoba, status, "21111", "02111", "12111", "IdZastepuje,Zastepuje");

            GetValue(ref v1, this, dr, osoba, status, "01111", "01111", "11111", "KierAcc");
            GetValue(ref v1, this, dr, osoba, status, "01111", "01111", "11111", "DataKierAcc");

            if (wnTyp == -1) wnTyp = db.getInt(dr, "TypId", -1);
            switch (wnTyp)
            {
                case wtWN:  // wolne za nadgodziny
                    GetValue(ref v1, this, dr, osoba, status, "21111", "01111", "11111", "Info");
                    GetValue(ref v1, this, dr, osoba, status, "00000", "00000", "00000", "UzasadnieniePrac");
                    break;
                case wtUB:  // urlop bezpłatny
                    GetValue(ref v1, this, dr, osoba, status, "00000", "00000", "00000", "Info");
                    GetValue(ref v1, this, dr, osoba, status, "21111", "01111", "11111", "UzasadnieniePrac");
                    break;
                default:    // 
                    GetValue(ref v1, this, dr, osoba, status, "00000", "00000", "00000", "Info");
                    GetValue(ref v1, this, dr, osoba, status, "00000", "00000", "00000", "UzasadnieniePrac");
                    break;
            }

            */

            
            /*
            bool v2 = false;
            SetVisible(ref v2, btAccept, osoba, status, "00000", "01000", "01000", "Potwierdź akceptację wniosku.");
            SetVisible(ref v2, btReject, osoba, status, "00000", "01000", "01000", "Potwierdź odrzucenie wniosku.");
            SetVisible(ref v2, btSave,   osoba, status, "00000", "00000", "11111", "");
            SetVisible(ref v2, btCofnij, osoba, status, "00000", "00000", "01111", "Potwierdź cofnięcie wniosku do poprzedniego statusu.");

            SetVisible(ref v2, btSend,   osoba, status, "10000", "00000", "00000", "Potwierdź złożenie wniosku.");
            SetVisible(ref v2, btCancel, osoba, status, "10000", "01000", "00000", "");
            SetVisible(ref v2, btClose,  osoba, status, "01111", "10111", "11111", "");
            */


//            FillData(dr, "")
            


/*

"Typ,TypNapis,Status,PracLogo,Pracownik,KierLogo,Kierownik,Dzial,Stanowisko,
            
ZastLogo,Zastepuje,
            
KierAcc,KierAccZast,KadryAcc
    
    Id	Typ	DataWniosku	IdPracownika	IdZastepuje	Od	Do	Godzin	Dni	IdPrzelozony	ProjektDzial	Stanowisko	Info	UzasadnieniePrac	IdKierAcc	IdKierAccZast	DataKierAcc	IdKadryAcc	DataKadryAcc	Uzasadnienie	Status

            
            
            NULL	NULL	Nowy wniosek	02223	Owieśny Leszek	01282	Jędryczka Adrian	HP Lap Cons	NULL	NULL	NULL	NULL	NULL	NULL	7	1	2013-10-01 00:00:00.000	1240	NULL	2011-01-01 00:00:00.000	2011-01-01 00:00:00.000	NULL	NULL	221	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	0
            

* /


-------------------
        public static bool GetValue(ref bool paVisible, Control cnt, DataRow dr, int vindex, int index, params string[] vis)  // ostatni vis to lista pól oddzielona przecinkiem !!!
        {
            if (vis.Length > 0)
            {
                bool visible, edit;
                char v = getVisible(vindex, index, out visible, out edit, vis);  // nadmiarowo o 1 element ale to nic ...
                string fields = vis[vis.Length - 1];
                GetValue(cnt, dr, visible, edit, fields);
                paVisible = visible;
                return visible;
            }
            return false;
        }

        public static void GetValue(Control cnt, DataRow dr, string fields)  // lista pól oddzielona przecinkiem !!!
        {
            GetValue(cnt, dr, true, false, fields);
        }

        public static void GetValue(Control cnt, DataRow dr, bool visible, bool edit, string fields)  // lista pól oddzielona przecinkiem !!!
        {
            string[] fa = fields.Split(',');
            if (fa.Length > 0)
            {
                bool isDateCnt = false;
                //----- panel -----
                Control pa = cnt.FindControl("pa" + fa[0]);
                if (pa != null) pa.Visible = visible;
                //----- edycja -----
                Control ed = cnt.FindControl("ed" + fa[0]);
                if (ed != null)
                {
                    ed.Visible = visible && edit;
                    if (dr != null && fieldExists(dr, fa[0]))
                    {
                        if (ed is TextBox) ((TextBox)ed).Text = db.getValue(dr, fa[0]);
                        else if (ed is DropDownList) Tools.SelectItem((DropDownList)ed, db.getValue(dr, fa[0]));
                        else if (ed is DateEdit)
                        {
                            ((DateEdit)ed).DateStr = Tools.DateToStr(db.getDateTime(dr, fa[0]));
                            isDateCnt = true;
                        }
                        else if (ed is CheckBox)
                        {
                            CheckBox cb = ed as CheckBox;
                            cb.Checked = db.getBool(dr, fa[0], false);
                            if (!edit) cb.Enabled = false;
                        }
                    }
                    else
                        if (ed is TextBox) ((TextBox)ed).Text = null;
                        else if (ed is DropDownList) Tools.SelectItem((DropDownList)ed, null);
                        else if (ed is DateEdit)
                        {
                            ((DateEdit)ed).Date = null;
                            isDateCnt = true;
                        }
                        else if (ed is CheckBox)
                        {
                            CheckBox cb = ed as CheckBox;
                            cb.Checked = false;
                            if (!edit) cb.Enabled = false;
                        }
                }
                //----- podgląd -----
                Control lb = cnt.FindControl("lb" + fa[0]);
                if (lb != null)
                {
                    bool vv = visible && !edit;
                    lb.Visible = vv;
                    if (vv && dr != null)
                    {
                        if (fa.Length > 1)
                        {
                            if (fieldExists(dr, fa[1]))
                                if (isDateCnt)
                                    ((Label)lb).Text = Tools.DateToStr(db.getValue(dr, fa[1]));
                                else
                                    ((Label)lb).Text = db.getValue(dr, fa[1]);
                        }
                        else
                        {
                            if (fieldExists(dr, fa[0]))
                                if (isDateCnt)
                                    ((Label)lb).Text = Tools.DateToStr(db.getValue(dr, fa[0]));
                                else
                                    ((Label)lb).Text = db.getValue(dr, fa[0]);
                        }
                    }
                }
            }
        }

        /*
        public static bool GetValue(ref bool paVisible, Control cnt, DataRow dr, int vindex, int index, params string[] vis)  // ostatni vis to lista pól oddzielona przecinkiem !!!
        {
            if (vis.Length > 0)
            {
                bool visible, edit;
                char v = getVisible(vindex, index, out visible, out edit, vis);  // nadmiarowo o 1 element ale to nic ...
                string fields = vis[vis.Length - 1];
                string[] fa = fields.Split(',');
                if (fa.Length > 0)
                {
                    //----- panel -----
                    Control pa = cnt.FindControl("pa" + fa[0]);
                    if (pa != null) pa.Visible = visible;
                    //----- edycja -----
                    Control ed = cnt.FindControl("ed" + fa[0]);
                    if (ed != null)
                    {
                        ed.Visible = visible && edit;
                        if (dr != null)
                        {
                            if (fieldExists(dr, fa[0]))
                            {
                                if (ed is TextBox) ((TextBox)ed).Text = db.getValue(dr, fa[0]);
                                else if (ed is DropDownList) Tools.SelectItem((DropDownList)ed, db.getValue(dr, fa[0]));
                                else if (ed is DateEdit) ((DateEdit)ed).Date = db.getDateTime(dr, fa[0]);
                                else if (ed is CheckBox)
                                {
                                    CheckBox cb = ed as CheckBox;
                                    cb.Checked = db.getBool(dr, fa[0], false);
                                    if (!edit) cb.Enabled = false;
                                }
                            }
                            else
                                if (!edit && ed is CheckBox)
                                {
                                    CheckBox cb = ed as CheckBox;
                                    cb.Enabled = false;
                                }
                        }
                    }
                    //----- podgląd -----
                    Control lb = cnt.FindControl("lb" + fa[0]);
                    if (lb != null)
                    {
                        bool vv = visible && !edit;
                        lb.Visible = vv;
                        if (vv && dr != null)
                        {
                            if (fa.Length > 1)
                            {
                                if (fieldExists(dr, fa[1]))
                                    ((Label)lb).Text = db.getValue(dr, fa[1]);
                            }
                            else
                            {
                                if (fieldExists(dr, fa[0]))
                                    ((Label)lb).Text = db.getValue(dr, fa[0]);
                            }
                        }
                    }
                }
                if (visible) paVisible = true;
                return visible;
            }
            return false;
        }
         * /


        public static bool SetVisible(ref bool paVisible, Button bt, int vindex, int index, params string[] vis)  // ostatni vis to question
        {
            if (vis.Length > 0)
            {
                bool visible, edit;
                string question = vis[vis.Length - 1];
                char v = getVisible(vindex, index, out visible, out edit, vis);
                bt.Visible = visible;
                if (visible)
                {
                    if (!String.IsNullOrEmpty(question))
                        Tools.MakeConfirmButton(bt, question);  
                    paVisible = true;
                    return true;
                }
            }
            return false;
        }

        public static char getVisible(int vindex, int index, out bool visible, out bool edit, params string[] vis) // zazwyczaj vindex to tryb/osoba, index to status
        {
            if (0 <= vindex && 0 < vis.Length)
            {
                string v = vis[vindex];
                char[] va = v.ToCharArray();
                if (0 <= index && index < va.Length)
                {
                    char vas = va[index];
                    visible = vas != '0';
                    edit = vas == '2';
                    return vas;
                }
            }
            visible = false;
            edit = false;
            return (char)0;
        }


        public static bool fieldExists(DataRow dr, string field)  //<<<< inaczej powinno być zrobione, ale na szybko ...
        {
            try
            {
                object o = dr[field];
                return true;
            }
            catch
            {
                return false;
            }
        }
        //-----------------------------------------------------------
        /*
        private void SetVisible(ref bool paVisible, string vNew, string vModify, string vDelete, params Control[] cntList)   /// musi być odpalona po GetValues bo korzysta ze status !!!
        {
            bool visible, edit;
            char vas = getVisible(vNew, vModify, vDelete, (int)status, out visible, out edit);
            foreach (Control cnt in cntList)
                cnt.Visible = visible;
            if (visible) paVisible = true;
        }
        * /




 */