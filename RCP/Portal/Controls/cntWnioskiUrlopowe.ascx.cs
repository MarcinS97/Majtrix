using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using AjaxControlToolkit;
using HRRcp.App_Code;

namespace HRRcp.Portal.Controls
{
    public partial class cntWnioskiUrlopowe : System.Web.UI.UserControl
    {
        public event EventHandler Show;
        public event EventHandler Hide;
        public event EventHandler DataBound;
        public event EventHandler Changed;
        public int ShowWniosekId;

        public const int moPrac = 0;
        public const int moKier = 1;
        public const int moAdm  = 2;
        public const int moAdmToEnter       = 21;
        public const int moAdmWprowadzone   = 22;
        public const int moAdmDoSpr         = 23;

        public const int moPracaZdalnaAcc   = 1337;
        private int FMode = moPrac;

        public const int stAll      = 99;
        public const int stMoje     = 9;
        public const int stAccepted = 34;
        public const int stToEnter  = 8;
        public const int stEntered  = 88;
        private int FStatus = -1;

        public bool FInit = true;

        const string pager = "DataPager1";

        //----- export typ -----
        public const int etNoExport     = 0;
        public const int etManualExport = 1;
        public const int etAutoExport   = 2;  // auto & manual

        //----- filter ------
        public const int fiOff = 0;
        public const int fiOn = 1;

        private int FFilter = fiOff;

        


        protected void Page_Init(object sender, EventArgs e)
        {
            bool logUpdate = FMode != moAdmToEnter;
            Tools.PrepareDicListView(lvWnioski, Tools.ListViewMode.Bootstrap, true, logUpdate, true);
            Tools.PrepareSorting(lvWnioski, -3, 15);
        }

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        InitDataSource();
        //        DataPager dp = Pager;
        //        if (dp != null && PageSize != -1) dp.PageSize = PageSize;
        //    }
        //}


        bool v = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                paStatus.Visible = Status == stAll || Status == stMoje;

                if (FInit)
                    InitDataSource(false);

                DataPager dp = Pager;
                if (dp != null && PageSize != -1) dp.PageSize = PageSize;

                if (PrepareFilter())
                    DoSearch(false);  // ustawienie filtra domyślnego

#if SPX
                btImport.Visible = false;
                btImportBAAN.Visible = false;
                btUpdateEntered.Visible = true;
#endif

            }
            else
            {
                v = Visible;      // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką

                DoSearch(false);  // ustawienie filtra domyślnego //20160503
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }





        public int Prepare(bool bind)
        {
            //if (PageSize != -1) PageSize = PageSize;
            InitDataSource(bind);
            return lvWnioski.Items.Count;  // z pagera powinno się brac
        }



        public int PrepareOnShow()
        {
            //if (PageSize != -1) PageSize = PageSize;
            bool b = Initialized;
            if (!b) Initialized = true;
            InitDataSource(!b);
            return lvWnioski.Items.Count;  // z pagera powinno się brac
        }




        private void TriggerChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        public override void DataBind()
        {
            //DoSearch(false);
            base.DataBind();
        }
        //-----------------------------
        public static bool UpdateEntered()
        {
            bool ok = db.execSQL(String.Format(@"
declare @admId int
declare @stAcc int
declare @stEnt int
set @admId = {0}
set @stAcc = {1}
set @stEnt = {2}

--drop table #aaa
select * into #aaa from Absencja

update poWnioskiUrlopowe set 
    StatusId = @stEnt,
    IdKadryAcc = @admId,
    DataKadryAcc = GETDATE(),
    Wprowadzony = 1
where StatusId = @stAcc and Id not in   -- wszystkie zaakceptowane ktore maja pelne pokrycie w absencjach z Asseco
(                                       -- te nie mają wprowadzonych absencji lub różni się okres lub typ
select distinct W.Id 
from poWnioskiUrlopowe W
left join poWnioskiUrlopoweTypy T on T.Id = W.TypId
outer apply (select * from dbo.GetDates2(W.Od, W.Do)) D
left join Kalendarz K on K.Data = D.Data
left join #aaa A on A.IdPracownika = W.IdPracownika and D.Data between A.DataOd and A.DataDo
left join AbsencjaKody AK on AK.Kod = A.Kod and (AK.Symbol = T.Symbol " +
#if SPX
    "or T.Symbol = 'UW' and AK.Symbol = 'UD' " +
    "or T.Symbol = 'UŻ' and AK.Symbol = 'UW' " +  //nie ma rozroznienia w BAAN
#else
    //"or T.Symbol = 'UW' and AK.Symbol = 'UD' " +      //20150719 po dołożeniu wniosku UD nie będzie to potrzebne
#endif
 @") 
where (K.Rodzaj is null or W.TypId = 10) and (A.Id is null or AK.Kod is null)  -- brak absencji lub inny kod, z uwzględnieniem wniosków wolne za święto
and W.StatusId = @stAcc
)
                ", App.User.OriginalId, cntWniosekUrlopowy.stAccepted, cntWniosekUrlopowy.stAcceptedHR));
            return ok;
            /*
--where K.Rodzaj is null and (A.Id is null or AK.Kod is null)  -- brak absencji lub inny kod
            */
            /*
declare @admId int
declare @stAcc int
declare @stEnt int
set @admId = {0}
set @stAcc = {1}
set @stEnt = {2}

update poWnioskiUrlopowe set 
StatusId = @stEnt,
IdKadryAcc = @admId,
DataKadryAcc = GETDATE()
from poWnioskiUrlopowe W
left outer join Absencja A on A.IdPracownika = W.IdPracownika and 
(A.DataOd = W.Od and A.IleDni = W.Dni or
 A.DataDo = W.Do and A.IleDni = W.Dni)
where W.StatusId = @stAcc and W.Wprowadzony = 1 and A.Id is not null
             */
        }
        //-----------------------------
        public void x_SetWprowadzony(bool wpr)
        {
            if (lvWnioski.SelectedIndex != -1)
            {
                ListViewItem item = lvWnioski.Items[lvWnioski.SelectedIndex];
                string wid = lvWnioski.SelectedDataKey.Value.ToString();
                db.update("poWnioskiUrlopowe", 0, "Wprowadzony", "Id=" + wid, wpr ? 1 : 0);
                lvWnioski.DataBind();    
            }
        }

        public void SetWprowadzony(int index, bool wpr)
        {
            ListViewItem item = lvWnioski.Items[lvWnioski.SelectedIndex];
            CheckBox cb = item.FindControl("cbEntered") as CheckBox;
            if (cb != null)
            {
                cb.Checked = wpr;
                lvWnioski.UpdateItem(index, false);
                //SetRowEntered(index, wpr);
                TriggerChanged();
            }
        }

        private void DisablePager()
        {
            DataPager dp = Pager;
            if (dp != null) 
            {
                dp.PageSize = int.MaxValue;
                dp.Visible = false;
            }
        }

        public string GetRowClass(string css, object wpr)
        {
            if (FMode == moAdmToEnter && (bool)wpr)
                return css + " " + css + "_entered entered";  // sit sit_enterd entered
            else
                return css;
        }
        //-----------------------------
        private SqlDataSource GetDataSource()
        {
            switch (FMode)
            {
                default:
                case moPrac:
                    return SqlDataSource1;
                case moKier:
                    return SqlDataSource2;
                case moAdm:
                    return SqlDataSource3;
                case moAdmToEnter:
                    return SqlDataSourceToEnter;
                case moAdmWprowadzone:
                    return SqlDataSourceEntered;
                case moAdmDoSpr:
                    return SqlDataSourceToVerify;
                case moPracaZdalnaAcc:
                    return dsPracaZdalna;
            }
        }

        private void InitDataSource(bool dataBind)
        {
            switch (FMode)
            {
                case moPrac:
                    lvWnioski.DataSourceID = SqlDataSource1.ID;
                    SqlDataSource1.SelectParameters["IdPracownika"].DefaultValue = App.User.Id;
                    SqlDataSource1.SelectParameters["Status"].DefaultValue = FStatus.ToString();
                    if (dataBind) lvWnioski.DataBind();
                    break;
                case moKier:
                    lvWnioski.DataSourceID = SqlDataSource2.ID;
                    SqlDataSource2.SelectParameters["IdKierownika"].DefaultValue = App.User.Id;
                    SqlDataSource2.SelectParameters["Status"].DefaultValue = FStatus.ToString();
                    SqlDataSource2.SelectParameters["rSub"].DefaultValue = App.User.HasRight(AppUser.rWnioskiUrlopoweSub) ? "1" : "0";
                    if (dataBind) lvWnioski.DataBind();
                    break;
                case moAdm:
                    lvWnioski.DataSourceID = SqlDataSource3.ID;
                    SqlDataSource3.SelectParameters["IdAdm"].DefaultValue = App.User.Id;
                    SqlDataSource3.SelectParameters["Status"].DefaultValue = FStatus.ToString();
                    if (dataBind) lvWnioski.DataBind();
                    break;
                case moAdmToEnter:
                    lvWnioski.DataSourceID = SqlDataSourceToEnter.ID;
                    DisablePager();
                    if (dataBind) lvWnioski.DataBind();
#if SPX
                    paButtonsTop.Visible = true;
#else
                    paButtons.Visible = true;
#endif
                    Tools.MakeConfirmButton(btImport, "Potwierdź import danych z systemu Asseco HR.");
                    break;
                case moAdmWprowadzone:
                    lvWnioski.DataSourceID = SqlDataSourceEntered.ID;
                    if (dataBind) lvWnioski.DataBind();
                    break;
                case moAdmDoSpr:
                    lvWnioski.DataSourceID = SqlDataSourceToVerify.ID;
                    DisablePager();
                    if (dataBind) lvWnioski.DataBind();
                    break;
                case moPracaZdalnaAcc:
                    lvWnioski.DataSourceID = dsPracaZdalna.ID;
                    dsPracaZdalna.SelectParameters["IdKierownika"].DefaultValue = App.User.Id;
                    dsPracaZdalna.SelectParameters["Status"].DefaultValue = FStatus.ToString();
                    if (dataBind) lvWnioski.DataBind();
                    break;
            }
        }
        
        private void InitLayout()
        {
            switch (FMode)
            {
                case moPrac:
                    Tools.SetControlVisible(lvWnioski, "thKier", true);
                    break;
                case moKier:
                case moPracaZdalnaAcc:
                    Tools.SetControlVisible(lvWnioski, "thPrac", true);
                    Tools.SetControlVisible(lvWnioski, "thKier", true);
                    break;
                case moAdm:
                    Tools.SetControlVisible(lvWnioski, "thPrac", true);
                    Tools.SetControlVisible(lvWnioski, "thKier", true);
                    break;
                case moAdmToEnter:
                    Tools.SetControlVisible(lvWnioski, "thPrac", true);
                    Tools.SetControlVisible(lvWnioski, "thEntered", true);
                    Tools.SetControlVisible(lvWnioski, "thStatus", false);
                    Tools.SetControlVisible(lvWnioski, "thTyp", false);
                    Tools.SetControlVisible(lvWnioski, "thOd", false);
                    Tools.SetControlVisible(lvWnioski, "thDo", false);
                    Tools.SetControlVisible(lvWnioski, "thDni", false);
                    Tools.SetControlVisible(lvWnioski, "thGodzin", false);
                    Tools.SetControlVisible(lvWnioski, "thTypOkres", true);
                    break;
                case moAdmWprowadzone:
                    Tools.SetControlVisible(lvWnioski, "thPrac", true);
                    Tools.SetControlVisible(lvWnioski, "thKier", true);
                    break;
                case moAdmDoSpr:
                    Tools.SetControlVisible(lvWnioski, "thPrac", true);
                    Tools.SetControlVisible(lvWnioski, "thKier", true);
                    Tools.SetControlVisible(lvWnioski, "thEntered", true);
                    break;
            }
        }

        private void InitItem(ListViewItemEventArgs e)  //-> ItemCreated, ItemDataBound
        {
            switch (FMode)
            {
                case moPrac:
                    Tools.SetControlVisible(e.Item, "tdKier", true);
                    break;
                case moKier:
                case moPracaZdalnaAcc:
                    Tools.SetControlVisible(e.Item, "tdPrac", true);
                    Tools.SetControlVisible(e.Item, "tdKier", true);
                    break;
                case moAdm:
                    Tools.SetControlVisible(e.Item, "tdPrac", true);
                    Tools.SetControlVisible(e.Item, "tdKier", true);
                    break;
                case moAdmToEnter:
                    Tools.SetControlVisible(e.Item, "tdPrac", true);
                    Tools.SetControlVisible(e.Item, "tdEntered", true);
                    Tools.SetControlVisible(e.Item, "tdStatus", false);
                    Tools.SetControlVisible(e.Item, "tdTyp", false);
                    Tools.SetControlVisible(e.Item, "tdOd", false);
                    Tools.SetControlVisible(e.Item, "tdDo", false);
                    Tools.SetControlVisible(e.Item, "tdDni", false);
                    Tools.SetControlVisible(e.Item, "tdGodzin", false);
                    Tools.SetControlVisible(e.Item, "tdTypOkres", true);
                    break;
                case moAdmWprowadzone:
                    Tools.SetControlVisible(e.Item, "tdPrac", true);
                    Tools.SetControlVisible(e.Item, "tdKier", true);
                    break;
                case moAdmDoSpr:
                    Tools.SetControlVisible(e.Item, "tdPrac", true);
                    Tools.SetControlVisible(e.Item, "tdKier", true);
                    Tools.SetControlVisible(e.Item, "tdEntered", true);
                    break;
            }
        }
        //---------------------------
        protected void lvWnioski_LayoutCreated(object sender, EventArgs e)
        {
            InitLayout();
        }
        
        protected void lvWnioski_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                InitItem(e);
            }
        }

        protected void lvWnioski_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            int lim = Tools.GetListItemMode(e, lvWnioski, out drv);
            switch (lim)
            {
                case Tools.limSelect:
                    int status = (int)drv["StatusId"];
                    switch (FMode)
                    {
                        case moPrac:
                            //Tools.SetControlVisible(e.Item, "DeleteButton", status == cntWniosekUrlopowy.stWaitingAcc || status == cntWniosekUrlopowy.stNew);
                            break;
                        case moKier:
                            break;
                        case moAdm:
                            //Tools.SetControlVisible(e.Item, "DeleteButton", true);
                            break;
                        case moAdmToEnter:
                        case moAdmWprowadzone:
                        case moAdmDoSpr:
                            break;
                    }
                    InitItem(e);
                    //----- select row by click -----
                    switch (FMode)
                    {
                        case moAdmToEnter:
                            //Tools.SetControlVisible(e.Item, "btSelect1", true);
                            CheckBox cb = e.Item.FindControl("btSelect1") as CheckBox;                            
                            Button bt = e.Item.FindControl("btSelect") as Button;
                            if (cb != null && bt != null)
                            {
                                cb.Visible = true;
                                cb.Attributes["onclick"] = String.Format("javascript:doClick('{0}');", bt.ClientID);
                            }
#if SPX
                            Tools.SetControlVisible(e.Item, "btEksport", false);
#endif
                            break;
                        case moAdmDoSpr:  // bez zaznaczania
                            break;
                        default:
                            HtmlTableRow tr = e.Item.FindControl("trLine") as HtmlTableRow;  // !!! nie działa na IE !!!
                            bt = e.Item.FindControl("btSelect") as Button;
                            if (tr != null && bt != null)
                                tr.Attributes["onclick"] = String.Format("javascript:doClick('{0}');", bt.ClientID);
                            break;
                    }
                    break;
            }
        }

        protected void lvWnioski_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lvWnioski.Items[lvWnioski.SelectedIndex];
        }

        public int RecCount = 0;

        protected void lvWnioski_DataBound(object sender, EventArgs e)
        {
            if (DataBound != null)
            {
                DataPager dp = Pager;
                if (dp != null)
                    RecCount = dp.TotalRowCount;
                DataBound(this, EventArgs.Empty);
            }
        }

        protected void lvWnioski_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "show":
                case "Select":
                    if (Show != null)
                    {
                        if (!db.isNull(e.CommandArgument))   // w przeciwnym wypadku tylko select
                        {
                            ShowWniosekId = Convert.ToInt32(e.CommandArgument);
                            Show(this, EventArgs.Empty);
                        }
                    }
                    break;
                case "Unselect":
                    lvWnioski.SelectedIndex = -1;
                    break;
                case "hide":  // na razie nieobsłużone
                    if (Hide != null)
                    {
                        Convert.ToInt32(Tools.GetDataKey(lvWnioski, e));
                        Hide(this, EventArgs.Empty);
                    }
                    break;
            }
        }

        protected void lvWnioski_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ShowWniosekId = Convert.ToInt32(Tools.GetDataKey(lvWnioski, e));
            Mailing.EventWniosekUrlopowy(Mailing.maWU_DEL, ShowWniosekId.ToString());   // musi być tu żeby dane zebrać, jak sie nie powiedzie usunięcie to trudno
        }

        protected void lvWnioski_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            if (Hide != null && e.AffectedRows > 0)
            {
                Hide(this, EventArgs.Empty);
                TriggerChanged();
            }
        }

        protected void lvWnioski_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            TriggerChanged();
        }

        protected void lvWnioski_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            TriggerChanged();
        }


        protected void lvWnioski_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            CheckBox cb = lvWnioski.Items[e.ItemIndex].FindControl("cbEntered") as CheckBox;
            if (cb != null)
                e.NewValues["Wprowadzony"] = cb.Checked;
        }

        /*
        private void SetRowEntered(int index, bool wpr)
        {
            const string wprowadzony = "wprowadzony";
            HtmlTableRow tr = lvWnioski.Items[index].FindControl("trLine") as HtmlTableRow;
            if (tr != null)
                if (wpr)
                    Tools.AddClass(tr, wprowadzony);
                else
                    Tools.RemoveClass(tr, wprowadzony);
        }
        */
        bool firsttime = true;

        protected void cbEntered_CheckedChanged(object sender, EventArgs e)
        {
            if (firsttime)  // nie wiem czemu się czasem 2x wykonuje
            {
                firsttime = false;
                CheckBox cb = (CheckBox)sender;
                ListViewItem item = (ListViewItem)cb.NamingContainer;
                ListViewDataItem dataItem = (ListViewDataItem)item;
                //string code = lvWnioski.DataKeys[dataItem.DisplayIndex].Value.ToString();
                lvWnioski.UpdateItem(dataItem.DisplayIndex, false);
                //SetRowEntered(dataItem.DisplayIndex, cb.Checked);
            }
        }

        protected void btNext_Click(object sender, EventArgs e)
        {
            int idx = lvWnioski.SelectedIndex;
            if (idx != -1) SetWprowadzony(idx, true);
            idx++;
            if (idx < lvWnioski.Items.Count)
            {
                lvWnioski.SelectedIndex = idx;
            }
            else
            {
                lvWnioski.SelectedIndex = -1;
#if !SPX
                Tools.ShowConfirm("Koniec.\n\nCzy wykonać import danych ?", btImport2, null);
#endif
            }
        }

        protected void btEksport_Click(object sender, EventArgs e)
        {
            int idx = lvWnioski.SelectedIndex;
            if (idx != -1)
                if (EksportWniosek(idx))
                {
                    SetWprowadzony(idx, true);
                    idx++;
                    if (idx < lvWnioski.Items.Count)
                    {
                        lvWnioski.SelectedIndex = idx;
                    }
                    else
                    {
                        lvWnioski.SelectedIndex = -1;
                        Tools.ShowConfirm("Koniec.\n\nCzy wykonać import danych ?", btImport2, null);
                    }
                }
        }














        //-----
        /*
        private void ImportBAAN(SqlConnection con, int ParentId, int mode)    // 0 - all, 1 - absencje
        {
            const string info = "Import danych z sysytemu BAAN - Service";
            const string error = "BŁĄD PODCZAS IMPORTU, Kod: {0}";
            try
            {
                Log.Info(Log.APP, ParentId, info, "START", Log.OK);

                string o1;
                string clientVPN = ConfigurationSettings.AppSettings["BAAN_VPN_CLIENT"];
                string startVPN = ConfigurationSettings.AppSettings["BAAN_VPN_START"];
                string stopVPN = ConfigurationSettings.AppSettings["BAAN_VPN_STOP"];
                //----- start VPN -----
                int err = 0;
                if (!String.IsNullOrEmpty(clientVPN) && !String.IsNullOrEmpty(startVPN))
                {
                    Log.Info(Log.APP, ParentId, "Start VPN", null, Log.OK);
                    err = Tools.Execute(clientVPN, startVPN, out o1);
                    if (err == 0) Log.Info(Log.APP, ParentId, "Start VPN", o1, Log.OK);
                    else Log.Error(Log.APP, ParentId, String.Format("Start VPN ERROR: {0}", err), o1);
                }
                if (err == 0)
                {
                    try
                    {
                        //----- import -----
                        switch (mode)
                        {
                            case moAll:
                                App.MakeBackup();
                                Log.Info(Log.APP, ParentId, "Wykonanie kopii bezpieczeństwa", null, Log.OK);
                                bool ok = Baan.Replicate(ParentId, mode);
                                if (ok) cntWnioskiUrlopowe_UpdateEntered();
                                if (ok)
                                {
                                    ok = Baan.ImportData(con, ParentId);
                                    if (!ok) err = 3;
                                }
                                break;
                            case moAbs:
                                ok = Baan.Replicate(ParentId, mode);
                                if (ok) cntWnioskiUrlopowe_UpdateEntered();
                                break;
                            case moAbsUpdate:
                                cntWnioskiUrlopowe_UpdateEntered();
                                break;
                        }
                    }
                    finally
                    {
                        //----- stop VPN ------
                        if (!String.IsNullOrEmpty(clientVPN) && !String.IsNullOrEmpty(stopVPN))
                        {
                            Log.Info(Log.APP, ParentId, "Stop VPN", null, Log.OK);
                            err = Tools.Execute(clientVPN, stopVPN, out o1);
                            if (err == 0) Log.Info(Log.APP, ParentId, "Stop VPN", o1, Log.OK);
                            else Log.Error(Log.APP, ParentId, String.Format("Stop VPN ERROR: {0}", err), o1);
                        }
                    }
                    if (err == 0)
                        Log.Info(Log.APP, ParentId, info, "KONIEC OK", Log.OK);
                }
                if (err != 0)
                    Log.Error(Log.APP, ParentId, info, String.Format(error, err));
            }
            catch (Exception ex)
            {
                Log.Error(Log.APP, ParentId, info, String.Format(error, 0));
            }
        }
        */


        protected void btImport_Click(object sender, EventArgs e)
        {
#if SPX
            //Baan.ImportAbsencje();
#else
            //Asseco.ImportAll();
            Asseco.ImportAbsencje();
#endif
            lvWnioski.DataBind();
        }

        protected void btUpdateEntered_Click(object sender, EventArgs e)
        {
#if SPX
            UpdateAbsencje();
            lvWnioski.DataBind();
#endif
        }

        //------------
        public static bool importInProgress = false;  // to z opisu będzie instancja zmiennej widziana we wszystkich sesjach/procesach PRP

        public static void UpdateAbsencje()
        {
            if (!importInProgress)
                try
                {
                    importInProgress = true;

                    const string info = "Aktualizacja wniosków urlopowych z systemu BAAN";
                    int lid = Log.Info(Log.IMPORT_ASSECO, info, null, Log.PENDING);

                    bool ok1 = UpdateEntered();  // jak 0 to też jest błąd
                    ok1 = true;

                    //if (c1 >= 0 && c2 >= 0)// && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0 && c7 >= 0 && c8 >= 0)// && ok1)
                    if (ok1)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.IMPORT_ASSECO, info, "zakończony OK", Log.OK);
                        Tools.ShowMessage("Aktualizacja absencji zakończona.");
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        //Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas aktualizacji absencji, kody: {0} {1} {2}", c1, c2, ok1 ? "1" : "0"), Log.OK);
                        //xLog.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1}", c1, c2), Log.OK);
                        Tools.ShowMessage("Wystąpił błąd podczas aktualizacji absencji. Szczegóły błędu znajdują się w logu systemowym.");
                    }
                }
                finally
                {
                    importInProgress = false;
                }
            else
                Tools.ShowMessage("Trwa już aktualizacja danych.\\n\\n" + App.User.Imie + ", poczekaj na jej zakończenie.");
        }

        //-------------------------------
        private bool EksportWniosek(int index)
        {
            ListViewItem item = lvWnioski.Items[index];
            string wid = lvWnioski.SelectedDataKey.Value.ToString();
            return Asseco.ExportWniosekUrlopowy(wid, true);
        }

        //---------------------------------------------------

        private void PrepareSearch()
        {
            /* nie działa na IE8
            Tools.ExecOnStart2("searchtrigger", String.Format(@"
                        $('#{0}').on('input', function(){{
                            alert(0);
                            doClick('{1}');
                        }});",
                tbSearch.ClientID, btSearch.ClientID));
             */
            /*
            string ev = String.Format("onChanging(this, '{0}');", btSearch.ClientID);
            //tbSearch.Attributes.Add("onkeypress", ev);
            tbSearch.Attributes.Add("onpaste", ev);
            tbSearch.Attributes.Add("onkeyup", ev);
            tbSearch.Attributes.Add("onblur", ev);
             */

            //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');$('#{2}').val('0');$('#{3}').val('0');$('#{4}').val('-1');$('#{5}').val('');$('#{6}').val('');doClick('{1}');return false;", 
                tbSearch.ClientID,          // 0
                btSearch.ClientID,          // 1

                ddlPracownik.ClientID,      // 2
                ddlTyp.ClientID,            // 3
                ddlStatus.ClientID,         // 4

                deOd.EditBox.ClientID,      // 5
                deDo.EditBox.ClientID);     // 6
            Tools.ExecOnStart2("searchwnU", String.Format("startSearch('{0}','{1}');",
                tbSearch.ClientID, 
                btSearch.ClientID));
            tbSearch.Focus();
        }

        private bool PrepareFilter()
        {
            switch (FFilter)
            {
                case fiOff:
                    paFilter.Visible = false;
                    return false;
                default:
                case fiOn:
                    return true;
            }
        }

        public void ClearFilter()
        {
            tbSearch.Text = null;
            ddlPracownik.SelectedIndex = -1;
            ddlTyp.SelectedIndex = -1;
            ddlStatus.SelectedIndex = -1;
            deOd.DateStr = null;
            deDo.DateStr = null;
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        protected void tbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private string FilterStr
        {
            set { ViewState["filter"] = value; }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        private void DoSearch(bool bind)
        {
            SqlDataSource SqlDataSource1 = GetDataSource();

            string filter;
            string f1 =
                (String.IsNullOrEmpty(ddlPracownik.SelectedValue) ? null : (" and IdPracownika = " + ddlPracownik.SelectedValue)) +
                (String.IsNullOrEmpty(ddlTyp.SelectedValue) ? null : (" and TypId = " + ddlTyp.SelectedValue)) +
                (String.IsNullOrEmpty(ddlStatus.SelectedValue) ? null : (" and StatusId = " + ddlStatus.SelectedValue)) +
                (!deOd.IsValid ? null : String.Format(" and Od >= '{0}'", deOd.DateStr)) +
                (!deDo.IsValid ? null : String.Format(" and Do <= '{0}'", deDo.DateStr));
            
            SqlDataSource1.FilterParameters.Clear();

            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                filter = String.IsNullOrEmpty(f1) ? null : f1.Substring(5);
            }
            else
            {
                //Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string f2;
                string[] words = Tools.RemoveDblSpaces(tbSearch.Text.Trim()).Split(' ');   // nie trzeba sprawdzać czy words[i] != ''
                if (words.Length == 1)
                {
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%')";
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%')", i);
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + f1;
            }
            FilterStr = filter;
            SqlDataSource1.FilterExpression = filter;

            if (bind)
            {
                lvWnioski.DataBind();
                if (lvWnioski.Items.Count == 1) Select(0);
                else if (lvWnioski.SelectedIndex != -1) Select(-1);
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            DoSearch(true);
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
            DoSearch(true);
        }

        public void Select(int index)
        {
            lvWnioski.SelectedIndex = index;
            //CheckSelectedChanged();
        }

        //-------------------------------
        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        public int Init
        {
            set { FInit = value != 0; }
            get { return FInit ? 1 : 0; }
        }

        public int Filter
        {
            set { FFilter = value; }
            get { return FFilter; }
        }

        public int Status
        {
            set { FStatus = value; }
            get { return FStatus; }
        }

        public int PageSize
        {
            set { ViewState["pagesize"] = value; }
            get { return Tools.GetInt(ViewState["pagesize"], -1); }
        }

        public bool Initialized
        {
            set { ViewState["init"] = value; }
            get { return Tools.GetBool(ViewState["init"], false); }
        }

        public ListView List
        {
            get { return lvWnioski; }
        }

        public DataPager Pager
        {
            get { return lvWnioski.FindControl(pager) as DataPager; }
        }
        //----- debug -----
        protected void SqlDataSource3_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void lvWnioski_DataBinding(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource3_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        public String Typy
        {
            get { return hidTypy.Value; }
            set { hidTypy.Value = value; }
        }

        public String Rodzaj
        {
            get { return hidRodzaj.Value; }
            set { hidRodzaj.Value = value; }
        }

    }
}