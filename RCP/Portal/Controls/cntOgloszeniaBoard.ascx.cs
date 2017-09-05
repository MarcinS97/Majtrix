using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using Portal.Controls;

namespace HRRcp.Portal.Controls
{
    public partial class cntOgloszeniaBoard : System.Web.UI.UserControl
    {
        int FRozwinMode = cntOgloszenia.moOgloszenia;
        bool adm = false;
        bool wyst = false;

        int action = cntOgloszenia.actNone;

        const int actRozwin     = 99;
        const int moNone        = -1;
        const int moUstawienia  = 99;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            adm = cntOgloszenia.IsAdmin;
            wyst = cntOgloszenia.IsWystawiajacy && !cntOgloszenia.IsZablokowany;
            if (!IsPostBack)
            {
                int mode;
                string oid;
                bool resend;
                action = cntOgloszenia.GetStartAction(out mode, out oid, out resend);

                paDoAkceptacji.Visible  = adm;
                paArchiwum.Visible      = adm;
                paAdministracja.Visible = adm;
                if (adm)
                {
                    cntDoAkceptacji.DataBind();
                    if (cntDoAkceptacji.List.Items.Count > 0)
                        FRozwinMode = cntOgloszenia.moDoAkceptacji;
                }

                if (action != cntOgloszenia.actNone)
                {
                    cntOgloszenia.ClearStartAction();     // zawsze zerujemy, nawet jak nie został zaoolgowany, bo wtedy po co ?
                    if (adm || !App.NeedPassLogin(App.User, 1))  // spr czy na pewno zalogowany
                    {
                        FRozwinMode = mode;
                        switch (action)
                        {
                            case cntOgloszenia.actDodaj:
                                if (adm || wyst) cntOgloszenieEdit.Show(GetFromMode(mode), oid, resend);
                                break;
                            case cntOgloszenia.actEdit:
                                if (adm || wyst) cntOgloszenieEdit.Show(GetFromMode(mode), oid, resend);
                                break;
                            case cntOgloszenia.actResend:
                                if (adm || wyst) cntOgloszenieEdit.Show(GetFromMode(mode), oid, resend);
                                break;
                            case cntOgloszenia.actFinish:
                                break;
                            case cntOgloszenia.actDelete:
                                break;
                        }
                    }
                }
            }
            else
            {
                //FRozwinMode = RozwinMode;
                //RozwinMode = moNone;
            }
        }

        private cntOgloszenia GetFromMode(int mode)
        {
            switch (mode)
            {
                case cntOgloszenia.moDoAkceptacji:
                    return cntDoAkceptacji;
                default:
                case cntOgloszenia.moOgloszenia:
                    return cntOgloszenia;
                case cntOgloszenia.moMoje:
                    return cntMoje;
                case cntOgloszenia.moArchiwum:
                    return cntArchiwum;
            }
        }

        public string RozwinCss(int mode)  //mode
        {
            return FRozwinMode == mode ? " in" : null;
        }

        public void Search(string s)
        {
            cntOgloszenia.Search(s);
            cntMoje.Search(s);
            if (adm)
            {
                cntArchiwum.Search(s);
                cntDoAkceptacji.Search(s);
            }
        }

        protected void cntDoAkceptacji_DataBound(object sender, EventArgs e)
        {
            int cnt = cntDoAkceptacji.Count;  // -1 ja nie znajdzie pager'a
            lbCount1.Visible = cnt > 0;
            lbCount1.Text = cnt.ToString();
            //UpdatePanel1.Update();
        }

        private void Refresh(int mode)
        {
            if (adm)
            {
                if (mode != cntOgloszenia.moDoAkceptacji) cntDoAkceptacji.List.DataBind();
                if (mode != cntOgloszenia.moOgloszenia) cntOgloszenia.List.DataBind();
                if (mode != cntOgloszenia.moMoje) cntMoje.List.DataBind();
                if (mode != cntOgloszenia.moArchiwum) cntArchiwum.List.DataBind();
            }
            else
            {
                if (mode != cntOgloszenia.moOgloszenia) cntOgloszenia.List.DataBind();
                if (mode != cntOgloszenia.moMoje) cntMoje.List.DataBind();
            }
        }

        protected void cntOgloszenia_Refresh(cntOgloszenia ogl, int action)
        {
            Refresh(ogl.Mode);
            /*
            switch (ogl.Mode)
            {
                case cntOgloszenia.moDoAkceptacji:
                    break;
                case cntOgloszenia.moMoje:
                    switch (action)
                    {
                        case cntOgloszenia.refNew:
                            cntDoAkceptacji.List.DataBind();
                            break;
                        case cntOgloszenia.refSave:
                        case cntOgloszenia.refAccept:
                        case cntOgloszenia.refReject:
                        case cntOgloszenia.refRemove:
                        case cntOgloszenia.refDelete:
                        case cntOgloszenia.refFinish:
                            break;
                    }
                    break;
                case cntOgloszenia.moOgloszenia:
                    break;
                case cntOgloszenia.moArchiwum:
                    break;
            }
             */ 
        }

        protected void cntOgloszenia_Edit(cntOgloszenia ogl, string oid, bool resend)
        {
            cntOgloszenieEdit.Show(ogl, oid, resend);
        }
        /*
        private int RozwinMode
        {
            //set { ViewState["rozmode"] = value; }
            //get { return Tools.GetInt(ViewState["rozmode"], moNone); }
            set { Session["rozmode"] = value; }
            get { return Tools.GetInt(Session["rozmode"], moNone); }
        }
        */
        protected void cntOgloszeniaParametry_Changed(object sender, EventArgs e)
        {
            cntDoAkceptacji.List.DataBind();
        }

        protected void cntOgloszeniaParametry_Regulamin(object sender, EventArgs e)
        {
            cntOgloszenia.SetStartAction(actRozwin, moUstawienia, null, false);
            App.Redirect(App.Ogloszenia);
        }

        protected void cntOgloszenieEdit_Save(object sender, EventArgs e)
        {
            Refresh(-1);  // mode które nie istnieje - all lv
        }

        protected void cntOgloszenieEdit_Regulamin(object sender, EventArgs e)
        {
            //cntRegulamin.Show(false);
            //Tools.ShowDialog("paRegulamin", "upRegulamin", "Regulamin", null, btCloseRegulamin.ClientID);
        }
    }
}