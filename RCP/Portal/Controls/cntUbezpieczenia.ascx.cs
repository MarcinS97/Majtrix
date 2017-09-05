using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntUbezpieczenia : System.Web.UI.UserControl
    {
        public const int actNone = 0;
        public const int actRem = 1;
        bool noPassLogin = false;    

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int action = GetStartAction();
                if (action != actNone)
                {
                    ClearStartAction();

                    if (App.User.IsPortalAdmin || !App.NeedPassLogin(App.User, 1))
                    {
                        switch (action)
                        {
                            case actRem:
                                modalRemindShow();
                                break;
                        }
                    }
                }
            }
        }

        public static int GetStartAction()
        {
            string am = Tools.GetStr(HttpContext.Current.Session["ubezpstart"]);
            //string am = HttpContext.Current.Session["ubezpstart"].ToString();
            if (!String.IsNullOrEmpty(am))
            {
                int a = Tools.StrToInt(am, actNone);
                return a;
            }
            return actNone;
        }

        public void Prepare(String Typ)
        {
            this.Typ = Typ;

            if (Typ != LastTyp)
            {
                SelectedItem = null;
                Typ2 = null;
                LastTyp = Typ;
            }

            if (SelectedItem != null)
            {
                Typ2 = SelectedItem;
                upMain.Update();
                rpItems.DataBind();
            }

            litTitle.Text = GetTitle(Typ);//db.Select.Scalar(dsTitle, db.strParam(Typ));//PortalMasterPage3.LeftMenuSelectedItem;
        }

        public String GetTitle(String typ)
        {
            /* T: potem dać z selecta, tylko uwaga - typ jest przekazany jako parametr, trzeba odseparować od Sql!!!
            Table = =db.Select.Table(SqlDataSource1.UpdateCommand)
            return db.Select.Scalar(SqlDataSource1.UpdateCommand, typ);
            */
            string[] t = hidTitle.Value.Split('|');
            string[] p = hidPar.Value.Split('|');
            for (int i = 0; i < t.Length; i++)
                if (p[i] == typ)
                    return t[i];
            return null;
            /*
            switch (typ)
            {
                case "UBEZP_ZDROW":
                    return u[0]; //"Ubezpieczenie zdrowotne";
                case "UBEZP_ZYC":
                    return u[1]; //"Ubezpieczenia na życie";
                case "UBEZP_MAJ":
                    return u[2]; //"Ubezpieczenia mieszkaniowe";
                default:
                    return "";
            }
            */ 
        }

        public string GetText(object omenu, object osql)
        {
            string menu = omenu.ToString();
            string sql = osql.ToString();
            if (String.IsNullOrEmpty(sql))
                return menu;
            else
            {
                DataSet ds = db.getDataSet(db.conP, String.Format(sql, App.User.Id));
                Mailing.PrepareMailText(ref menu, ds);
                return menu;
            }
        }
        //-------------------------------------------
        private bool IsRemindCreate
        {
            set { ViewState["remcre"] = value; }
            get { return Tools.GetBool(ViewState["remcre"], false); }
        }

        private void modalRemindShow()
        {
            DataRow dr = db.Select.Row(dsRemind, App.User.Id);
            bool c = dr == null;
            IsRemindCreate = c;
            lbRemindUstaw.Visible = c;
            lbRemindZmien.Visible = !c;
            lbRemind.Text = String.Format(lbRemind.Text, App.User.EMail);
            if (dr != null)
                deRemind.Date = (DateTime)db.getDateTime(dr, "DataStartu");
            modalRemind.Show(false);
        }

        private bool RemindSave()
        {
            const string err = "Wystapił błąd podczas zapisu.";
            bool ok = false;
            string d = deRemind.DateStr;
            if (String.IsNullOrEmpty(d))
            {
                ok = db.ExecuteDelete(dsRemind, App.User.Id);  // może nie być ok
                modalRemind.Close();
                if (!IsRemindCreate)
                    if (ok)
                        Tools.ShowMessage("Przypomnienia zostały wyłączone.");
                    else
                        Tools.ShowMessage(err);
                ok = false;   // zawsze zeby się nie zamknęło raz jeszcze
            }
            else
            {
                if (IsRemindCreate)
                    ok = db.ExecuteInsert(dsRemind, App.User.Id, Tools.DateToStrDb((DateTime)deRemind.Date));
                else
                    ok = db.ExecuteUpdate(dsRemind, App.User.Id, Tools.DateToStrDb((DateTime)deRemind.Date));
                if (!ok)
                    Tools.ShowMessage(err);
            }
            return ok;
        }

        protected void btRemindSave_Click(object sender, EventArgs e)
        {
            if (RemindSave())
                modalRemind.Close();
            rpItems.DataBind();
            upMain.Update();
        }

        //-------------------------------------------
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "click":
                    string c = e.CommandArgument.ToString();
                    this.Typ2 = c;
                    SelectedItem = c;
                    //Session["ubezpnaszybko"] = c;
                    if (true)//Parent is UbezpieczeniaForm)
                    {
                        //((UbezpieczeniaForm)Parent).Prepare(c);
                        upMain.Update();
                        rpItems.DataBind();
                    }
                    else
                    {
                        App.Redirect("Portal/Ubezpieczenia.aspx");
                        //string url;
                        //if (Tools.IsUrl(c, out url))
                        //    App.Redirect(url);
                        //else
                        //{
                        //    url = Tools.GetRedirectUrl(c);
                        //    App.Redirect(url);
                        //    switch (c)
                        //    {
                        //        case "":
                        //            break;
                        //    }
                        //}
                    }
                    break;
            }
        }

        protected void rpItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "click":
                    //string c = e.CommandArgument.ToString();
                    string id = e.CommandArgument.ToString();
                    DataRow drSelectedData = db.Select.Row(dsSelectedItemData, id);
                    
                    //Session["ubezpnaszybko"] = c;
                    string command = db.getValue(drSelectedData, "Command");
                    string par2 = db.getValue(drSelectedData, "Par2");
                    string c = GetCommand(command, par2);
                    string rights = db.getValue(drSelectedData, "Rights");

                    //if (rights == "1")
                    //{
                    //    if (!CheckIsLogged(actNone))
                    //        return;
                    //}

                    bool cmd = false;
                    if (c.StartsWith("cmd:"))
                    {
                        cmd = true;
                        rpItems.DataBind();   // bo ginęło
                        string cc = c.Substring(4);
                        switch (cc)
                        {
                            case "zgoda":
                                modalZgoda.Show();
                                break;
                            case "remind":
                                if(CheckIsLogged(actRem))
                                    modalRemindShow();
                                break;
                        }
                    }

                    if (!cmd)
                    {
                        string url;
                        if (Tools.IsUrl(c, out url))
                            App.Redirect(url);
                        else
                        {
                            url = Tools.GetRedirectUrl(c);
                            App.Redirect(url);
                            switch (c)
                            {
                                case "":
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        //---------------------
        public string GetCommand(object cmd, object par)
        {
            if (db.isNull(par))
                return cmd.ToString();
            else
                return String.Format("{0}?p={1}", cmd, par);
        }
       
        private bool CheckIsLogged(int act)
        {
            if (!noPassLogin && !App.User.IsPortalAdmin && App.NeedPassLogin(App.User, 1))
            {
                //SetStartAction(act, mode, oid, resend);
                SetStartAction(act);
                HRRcp.Portal.PracLogin.Show();
                return false;
            }
            else
                return true;
        }

        public static void SetStartAction(int act)
        {
            if (act == actNone)
                HttpContext.Current.Session["ubezpstart"] = null;
            else
                HttpContext.Current.Session["ubezpstart"] = act.ToString();
        }

        public static void ClearStartAction()
        {
            SetStartAction(actNone);
        }

        public String GetSelectedClass(object o)
        {
            string id = Convert.ToString(o);
            if (SelectedItem != null)
            {

                if (id == SelectedItem)
                {
                    return "small selected";
                }
                return "small";
            }
            return "";
        }

        public String SelectedItem
        {
            //get { return (String)HttpContext.Current.Session["sesSelectedUbezpItem" + Typ]; }
            //set { HttpContext.Current.Session["sesSelectedUbezpItem" + Typ] = value; }
            get { return (String)HttpContext.Current.Session["sesSelectedUbezpItem"]; }
            set { HttpContext.Current.Session["sesSelectedUbezpItem"] = value; }
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            string file = MapPath(@"~/Portal/Pliki/Zgoda na potracenie z wynagrodzenia.pdf");  // <<<<< tymczas !!!!
            modalZgoda.Close();
            Tools.ExecOnStart2("zdownload",String.Format("doClick('{0}');", btDownload.ClientID));
        }

        protected void btNot_Click(object sender, EventArgs e)
        {
            modalZgoda.Close();
        }

        protected void btDownload_Click(object sender, EventArgs e)
        {
            string file = MapPath(@"~/Portal/Pliki/Zgoda na potracenie z wynagrodzenia.pdf");  // <<<<< tymczas !!!!
            Tools.DownloadFile(file, Path.GetFileName(file), null);
        }

        public String Typ
        {
            get { return hidTyp.Value; }
            set { hidTyp.Value = value; }
        }

        public String LastTyp
        {
            set { HttpContext.Current.Session["lastTyp"] = value; }
            get { return Tools.GetStr(HttpContext.Current.Session["lastTyp"]); }
        }

        public String Typ2
        {
            get { return hidTyp2.Value; }
            set { hidTyp2.Value = value; }
        }
    }
}