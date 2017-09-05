using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.Drawing;
using AjaxControlToolkit;
using System.Web;
using System.Web.UI;
using HRRcp.App_Code;
using HRRcp;
using HRRcp.Portal.Controls;

namespace Portal.Controls
{
    public partial class cntOgloszenia : System.Web.UI.UserControl
    {
        public delegate void ERefresh(cntOgloszenia sender, int action);
        public event ERefresh Refresh;
        public delegate void EEdit(cntOgloszenia sender, string oid, bool resend);
        public event EEdit Edit;
        public event EventHandler DataBound;

        public const string ImagesPath = "~/Portal/Ogloszenia/";       // <<< do web.config
        public const string thumb = "thumb_";

        public const int moOgloszenia = 0;
        public const int moDoAkceptacji = 1;
        public const int moMoje = 2;
        public const int moArchiwum = 3;

        public const int stOczekujace = 1;
        public const int stZaakceptowane = 2;
        public const int stOdrzucone = 3;
        public const int stZakonczone = 4;
        public const int stOdrzuconeUsuniete = 13;
        public const int stZakonczoneUsuniete = 14;

        //----- start actions -----
        public const int actNone    = 0;     
        public const int actEdit    = 1;     
        public const int actDodaj   = 2;     
        public const int actResend  = 3;     
        public const int actDelete  = 4;     
        public const int actFinish  = 5;     //start, refresh
        //---- refresh actions -----
        public const int refNew     = 1;
        public const int refSave    = 1;
        public const int refAccept  = 2;     
        public const int refReject  = 3;     
        public const int refRemove  = 4;     
        public const int refDelete  = 5;     //start, refresh
        public const int refFinish  = 6;     //start, refresh

        //bool noPassLogin = true;            // testy !!! normalnie false
        bool noPassLogin = false;           

        bool adm, wyst, blok, acc;
        int mode;

        const string errStatus = "Wystąpił błąd podczas aktualizacji statusu ogłoszenia.";

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvOgloszenia, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserId = App.User.Id;
            mode = Mode;
            adm = IsAdmin;
            wyst = IsWystawiajacy;
            blok = IsZablokowany;
            acc = mode == moDoAkceptacji && adm;
            if (!IsPostBack)
            {
                //paDodaj.Visible  = mode == moMoje && wyst;
                //paFilter.Visible = mode == moOgloszenia;
                paFilter.Visible = true;
                ddlPracownik.Visible = adm;
                cbMoje.Visible = !adm && mode != moMoje;
                ddlStatus.Visible = mode == moMoje;
                btDodajOgloszenie.Visible = mode.IsAny(moOgloszenia, moMoje) && wyst;
            }
            else
            {
                if (DoRefresh)
                {
                    DoRefresh = false;
                    lvOgloszenia.DataBind();
                }
                UpdateFilter();
            }
        }

        //--------------------------------------
        public static bool IsAdmin
        {
            get { return App.User.HasRight(AppUser.rOgloszeniaAdm); }
        }

        public static bool IsWystawiajacy
        {
            get
            {
                return true;   // na razie wszyscy
                return App.User.HasRight(AppUser.rOgloszeniaWyst);
            }
        }

        public static bool IsZablokowany //blokada wystawiania
        {
            get { return App.User.HasRight(AppUser.rOgloszeniaBlokada); }
        }

        public void Search(string s)
        {
            hidSearch.Value = s;
        }








        protected bool IsZdjecie(object name)
        {
            return (!db.isNull(name));
        }


        //protected string GetThumbImage(object oid)
        //{
        //    int id = (int)oid;
        //    return PhotoPath + thumb + id.ToString();
        //    /*
        //    string[] files = Directory.GetFiles(Server.MapPath("~/Portal/Ogloszenia/"), "thumb_" + notice_id + "*");
        //    foreach (string file in files)
        //    {
        //        FileInfo fi = new FileInfo(file);
        //        return "Portal/Ogloszenia/" + fi.Name;
        //    }
        //    return "null";
        //    */
        //}

        //protected string GetFullImage(object oid)
        //{
        //    int id = (int)oid;
        //    return PhotoPath + id.ToString();
        //    /*
        //    string[] files = Directory.GetFiles(Server.MapPath("~/Portal/Ogloszenia/"), "" + notice_id + "*");
        //    foreach (string file in files)
        //    {
        //        FileInfo fi = new FileInfo(file);
        //        return "Portal/Ogloszenia/" + fi.Name;
        //    }
        //    return "null";
        //     */ 
        //}
        //-----------------------------------------
        //-----------------------------------------
        private bool CheckIsLogged(int act, int mode, string oid, bool resend)
        {
            if (!noPassLogin && !adm && App.NeedPassLogin(App.User, 1))
            {
                SetStartAction(act, mode, oid, resend);
                 HRRcp.Portal.PracLogin.Show();
                return false;
            }
            else
                return true;
        }

        private void EdytujOgloszenie(string oid, bool resend)
        {
            if (Edit != null)
                Edit(this, oid, resend);
        }

        protected void btDodajOgloszenie_Click(object sender, EventArgs e)
        {
            if (CheckIsLogged(actDodaj, Mode, null, false))
                EdytujOgloszenie(null, false);
        }
        //-----------------------------------------
        public static string GetImageUrl(object name)
        {
            if (!db.isNull(name))
            {
                //return "~/" + ImagesPath + name;
                //return "../../" + ImagesPath + name;
                //return "../" + ImagesPath + name;
                Page page = HttpContext.Current.Handler as Page;
                return page.ResolveUrl(ImagesPath + name);
            }
            else
                return null;
        }

        public static string GetThumbUrl(object name)
        {
            if (!db.isNull(name))
            {
                //return "~/" + ImagesPath + thumb + name;
                //return "../../" + ImagesPath + thumb + name;
                //return "../" + ImagesPath + thumb + name;
                Page page = HttpContext.Current.Handler as Page;
                return page.ResolveUrl(ImagesPath + thumb + name);
            }
            else
                return null;
        }

        public static string GetThumbUrlNoCache(object name)
        {
            string p = GetThumbUrl(name);
            if (!String.IsNullOrEmpty(p))
                p += "?" + DateTime.Now.Ticks.ToString();
            return p;
        }

        public static string GetImageUrlNoCache(object name)
        {
            string p = GetImageUrl(name);
            if (!String.IsNullOrEmpty(p))
                p += "?" + DateTime.Now.Ticks.ToString();
            return p;
        }

        public static string GetThumbFileName(string imgurl)
        {
            string fn = Path.GetFileName(imgurl);
            if (!String.IsNullOrEmpty(fn))
            {
                int p = fn.IndexOf("?");
                if (p != -1)
                    fn = fn.Substring(0, p);
            }
            return fn;
        }

        //-----------------------------------------
        protected void lvOgloszenia_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView drv = Tools.GetDataRowView(e);
                int status = db.getInt(drv["Status"], -1);
                bool arch = db.getBool(drv["IsArch"], false);
                string pid = drv["IdPracownika"].ToString();
                bool zwol = db.getBool(drv["IsZwolniony"], false);
                bool moje = pid == App.User.Id;
                //----- info -----
                bool kv = db.getBool(drv["KategoriaVisible"], false);
                Tools.SetControlVisible(e.Item, "lbKategoria", kv);
                Tools.SetControlVisible(e.Item, "lbKategoriaV", kv);

                bool sv = db.getBool(drv["StatusVisible"], false);
                Tools.SetControlVisible(e.Item, "lbDataDodania", sv);
                Tools.SetControlVisible(e.Item, "lbDataDodaniaV", sv);
                Tools.SetControlVisible(e.Item, "lbStatus", sv);
                Tools.SetControlVisible(e.Item, "lbStatusV", sv);

                Tools.SetControlVisible(e.Item, "trInfo", kv || sv);
                //----- buttons -----
                /*
                bool a = db.getBool(drv["btAccept"], false);
                bool o = db.getBool(drv["btReject"], false);
                bool d = db.getBool(drv["btDelete"], false);
                bool f = db.getBool(drv["btFinish"], false);
                bool r = db.getBool(drv["btResend"], false);
                 */
                //bool r = arch && (adm || moje && status.IsAny(stUsuniete, stZaakceptowane));    // admin może każde przywrócić, prac tylko swoje zaakceptowane
                bool a = mode == moDoAkceptacji && status == stOczekujace && adm;
                bool o = mode.IsAny(moDoAkceptacji, moOgloszenia) && status.IsAny(stOczekujace, stZaakceptowane) && adm;
                bool f = status == stZaakceptowane && (adm || moje);
                bool d = (status.IsAny(stOczekujace, stOdrzucone, stZakonczone) || status == stZaakceptowane && arch) && moje                          // usuwa (przenosi do archiwum) przez zmianę statusu
                      || (status.IsAny(stOdrzucone, stZakonczone) || status == stZaakceptowane && arch) && adm;
                bool r = (status == stZakonczone || status == stZaakceptowane && arch) && moje
                      || (status.IsAny(stZakonczone, stOdrzucone) || status == stZaakceptowane && arch) && !zwol && adm;
                /*
                Admin:
                1 --acc--> 2 --rej--> 3  --del--> 13
                1 --rej-------------> 3  --del--> 13
                           2 --data-> *4 --del--> 14
                           2 --fin--> 4  --del--> 14  
                                      3  --res--> 1
                                      4  --res--> 1
                                        
                User:
                1 --del-------------------------------> remove
                                      3  --del--> 13
                1          2 --data-> *4 --del--> 14  
                1          2 --fin--> 4  --del--> 14 
                                      4  --res--> 1

                public const int stOczekujace         = 1;
                public const int stZaakceptowane      = 2;
                public const int stOdrzucone          = 3;
                public const int stZakonczone         = 4;
                public const int stOdrzuconeUsuniete  = 13;
                public const int stZakonczoneUsuniete = 14;
                */

                Tools.SetButtonVisible(e.Item, "btAccept", a, "Potwierdź wystawienie ogłoszenia.");
                Tools.SetButtonVisible(e.Item, "btReject", o, "Potwierdź odrzucenie ogłoszenia.");  // admin może odrzucić wystawione, bo np. juz po wystawieniu okazało się, ze jednak nerusza regulamin
                Tools.SetButtonVisible(e.Item, "btFinish", f, "Potwierdź zakończenie publikacji ogłoszenia.");
                Tools.SetButtonVisible(e.Item, "btDelete", d, "Potwierdź usunięcie ogłoszenia.");   // do archiwum
                //Tools.SetButtonVisible(e.Item, "btResend", r, "Potwierdź ponowne wystawienie ogłoszenia.");
                Tools.SetControlVisible(e.Item, "btResend", r);
                Tools.SetControlVisible(e.Item, "btEdit", adm);
                Tools.SetControlVisible(e.Item, "paControl", a || o || d || f || r || adm);
            }
        }

        public static void SetStartAction(int act, int mode, string oid, bool resend)
        {
            if (act == actNone)
                HttpContext.Current.Session["ogstart"] = null;
            else
                HttpContext.Current.Session["ogstart"] = String.Format("{0}|{1}|{2}|{3}", act, mode, oid, resend ? 1 : 0);
        }

        public static void ClearStartAction()
        {
            SetStartAction(actNone, -1, null, false);
        }

        public static int GetStartAction(out int mode, out string oid, out bool resend)
        {
            string am = Tools.GetStr(HttpContext.Current.Session["ogstart"]);
            if (!String.IsNullOrEmpty(am))
            { 
                string[] p = Tools.GetLineParams(am);
                int a = Tools.StrToInt(p[0], actNone);
                if (a != actNone && p.Length == 4)
                {
                    mode = Tools.StrToInt(p[1], -1);
                    oid = p[2];
                    resend = p[3] == "1";
                    return a;
                }
            }
            mode = -1;
            oid = null;
            resend = false;
            return actNone;
        }

        protected void lvOgloszenia_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string oid = Tools.GetDataKey(lvOgloszenia, e);
            switch (e.CommandName)
            {
                case "edit":
                    if (CheckIsLogged(actEdit, Mode, oid, false))
                        EdytujOgloszenie(oid, false);
                    break;
                case "resend":
                    if (CheckIsLogged(actResend, Mode, oid, true))
                        EdytujOgloszenie(oid, true);
                    break;
                case "accept":
                    oid = Tools.GetDataKey(lvOgloszenia, e);
                    if (db.execSQL(db.conP, String.Format(SqlDataSource1.UpdateCommand, stZaakceptowane, oid)))
                    {
                        lvOgloszenia.DataBind();
                        TriggerRefresh(refAccept);
                    }
                    else
                        Tools.ShowError(errStatus);
                    break;
                case "reject":
                    oid = Tools.GetDataKey(lvOgloszenia, e);
                    if (db.execSQL(db.conP, String.Format(SqlDataSource1.UpdateCommand, stOdrzucone, oid)))
                    {
                        lvOgloszenia.DataBind();
                        TriggerRefresh(refReject);
                    }
                    else
                        Tools.ShowError(errStatus);
                    break;
                case "finish":
                    if (CheckIsLogged(actFinish, Mode, oid, false))
                    {
                        oid = Tools.GetDataKey(lvOgloszenia, e);
                        if (db.execSQL(db.conP, String.Format(SqlDataSource1.UpdateCommand, stZakonczone, oid)))
                        {
                            lvOgloszenia.DataBind();
                            TriggerRefresh(actFinish);
                        }
                        else
                            Tools.ShowError(errStatus);
                    }
                    break;
                case "del":
                    if (CheckIsLogged(actDelete, Mode, oid, false))
                    {
                        oid = Tools.GetDataKey(lvOgloszenia, e);
                        int st = Tools.StrToInt(e.CommandArgument.ToString(), -1);
                        if (st == stOczekujace)   // tu usuwamy permanentnie
                        {
                            bool ok = cntOgloszenieEdit.Remove(oid);
                            if (ok)
                            {
                                lvOgloszenia.DataBind();
                                TriggerRefresh(refRemove);
                            }
                        }
                        else
                        {
                            switch (st)
                            {
                                case stZakonczone:
                                    st = stZakonczoneUsuniete;
                                    break;
                                case stOdrzucone:
                                    st = stOdrzuconeUsuniete;
                                    break;
                            }
                            if (db.execSQL(db.conP, String.Format(SqlDataSource1.UpdateCommand, st, oid)))
                            {
                                lvOgloszenia.DataBind();
                                TriggerRefresh(actDelete);
                            }
                            else
                                Tools.ShowError(errStatus);
                        }
                    }
                    break;
            }
        }

        private void TriggerDataBound()
        {
            if (DataBound != null)
                DataBound(lvOgloszenia, EventArgs.Empty);
        }

        private void TriggerRefresh(int action)
        {
            if (Refresh != null)
                Refresh(this, action);
        }

        protected void lvOgloszenia_DataBound(object sender, EventArgs e)
        {
            TriggerDataBound();
        }

        //---------
        private void UpdateFilter()
        {
            bool f = String.IsNullOrEmpty(ddlPracownik.SelectedValue)
                  && String.IsNullOrEmpty(ddlKategoria.SelectedValue)
                  && String.IsNullOrEmpty(ddlStatus.SelectedValue)
                  && !cbMoje.Checked;
            btFilterClear.Visible = !f;
        }

        private void ClearFilter()
        {
            Tools.SelectItem(ddlPracownik, null);
            Tools.SelectItem(ddlKategoria, null);
            Tools.SelectItem(ddlStatus, null);
            cbMoje.Checked = false;
        }

        protected void btFilterClear_Click(object sender, EventArgs e)
        {
            ClearFilter();
            btFilterClear.Visible = false;
        }
        //-----------------------------------------
        //public bool LineMode
        //{
        //    get { return (Session["ViewSwitch"] == null) ? false : bool.Parse(Session["ViewSwitch"].ToString()); }
        //    set { Session["ViewSwitch"] = value; }
        //}

        public int Mode
        {
            set { hidMode.Value = value.ToString(); }
            get { return Tools.StrToInt(hidMode.Value, moOgloszenia); }
        }

        public string UserId
        {
            set { hidUserId.Value = value; }
            get { return hidUserId.Value; }
        }

        //public string Title
        //{
        //    set { lbTitle.Text = value; }
        //    get { return lbTitle.Text; }
        //}

        public ListView List
        {
            get { return lvOgloszenia; }
        }

        public string KategoriaFilter
        {
            get { return ddlKategoria.SelectedValue;  }
        }

        public int Count
        {
            get
            {
                DataPager dp = Tools.Pager(lvOgloszenia);
                return dp != null ? dp.TotalRowCount : -1;
            }
        }

        public bool DoRefresh
        {
            set { ViewState["dorefr"] = value; }
            get { return Tools.GetBool(ViewState["dorefr"], false); }
        }

    }
}