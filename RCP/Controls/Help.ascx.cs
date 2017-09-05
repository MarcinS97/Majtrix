using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class Help : System.Web.UI.UserControl
    {
        public event EventHandler HideClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btEdit.Visible = IsHelpEditor();
            }
        }
        //--------------------------------------
        protected void btHelpHide_Click(object sender, EventArgs e)
        {
            Show(false, true);  // przywróć poprzedni po ukryciu jeśli jest jakiś w store
            if (HideClick != null)
                HideClick(this, EventArgs.Empty);
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            GetHelpEdit();
            ShowEditor(true);
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (UpdateEditor())
            {
                GetHelpText();
                ShowEditor(false);
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            ShowEditor(false);
        }
        //--------------------------------------
        private bool IsHelpEditor()
        {
            return App.User.HasRight(AppUser.rSuperuser);
        }

        private string GetCurrentHelp()
        {
            string typ = HelpContext;
            if (String.IsNullOrEmpty(typ))
            {
                if (IsHelpEditor())
                    //return "Brak informacji o typie pomocy";
                    return String.Format("Typ: <b>{0}</b>", "Nie zdefiniowano");
                else
                    return Info.GetInfoText(Info.NOHELP);
            }
            else
            {
                string help = Info.GetInfoText(typ);
                if (String.IsNullOrEmpty(help))
                    if (IsHelpEditor())
                    {
                        DataRow dr = Info.GetInfo(typ);
                        string opis = db.getValue(dr, "Opis");
                        return String.Format("Typ: <b>{0}</b><br />Opis: <b>{1}</b><br />Treść:", typ, opis);
                    }
                    else
                        //return Info.GetInfoText(Info.NOHELP);
                        return help;
                else
                    return help;
            }
        }

        private void GetHelpText()
        {
            ltHelp.Text = GetCurrentHelp();
        }

        private void GetHelpEdit()
        {
            DataRow dr = Info.GetInfo(HelpContext);
            HelpContextEdit = HelpContext;
            lbTyp.Text = HelpContext;
            tbOpis.Text = db.getValue(dr, "Opis");
            edHelp.Content = db.getValue(dr, "Tekst");
        }
        //--------------------------------------
        public bool Show(bool visible, bool restore)
        {
            if (restore && !String.IsNullOrEmpty(hidStoreContext.Value))
            {
                hidContext.Value = hidStoreContext.Value;
                hidStoreContext.Value = null;
            }
            if (visible != Visible)
            {
                Visible = visible;
                if (visible)
                    GetHelpText();
            }
            return visible;
        }

        public void Show(string context)
        {
            hidStoreContext.Value = null;   // nie będzie już potrzebny

            if (Visible && HelpContext == context) 
                Show(false, false);
            else
            {
                HelpContext = context;
                Show(true, false);
            }
        }

        public void StoreHelpContext()
        {
            hidStoreContext.Value = HelpContext;
        }

        private void ShowEditor(bool visible)
        {
            paEditor.Visible = visible;
            ltHelp.Visible = !visible;
            helpHideButton.Visible = !visible;
        }

        private bool UpdateEditor()
        {
            return Info.SetInfo(HelpContextEdit, tbOpis.Text, edHelp.Content);
        }
        //--------------------------------------
        public string HelpClientID
        {
            get { return paHelp.ClientID; }
        }

        public string HelpContext
        {
            get { return hidContext.Value; }
            set
            {
                if (hidContext.Value != value)
                {
                    hidContext.Value = value;
                    if (this.Visible)
                        GetHelpText();
                }
            }
        }

        public string HelpContextEdit
        {
            set { ViewState["hedcontext"] = value; }
            get { return Tools.GetStr(ViewState["hedcontext"]); }
        }
    }
}