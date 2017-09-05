using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Uprawnienia
{
    public partial class cntAdmUprawnienia : System.Web.UI.UserControl
    {
        const bool editable = true;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvTypy, 1337);
            Tools.PrepareDicListView(lvKwalifikacje, 1337);
            Tools.PrepareDicListView(lvGrupy, 1337);
            Tools.PrepareDicListView(lvUprawnienia, 1337);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!editable)
                    lvTypy.InsertItemPosition = InsertItemPosition.None;
            }
        }

        public InsertItemPosition GetInsertItemPositionKwal()
        {
            if (!editable)
                return InsertItemPosition.None;

            if (lvTypy.SelectedIndex != -1)
                return InsertItemPosition.LastItem;
            else
                return InsertItemPosition.None;
        }

        public InsertItemPosition GetInsertItemPositionGrupy()
        {
            if (!editable)
                return InsertItemPosition.None;

            if (lvKwalifikacje.SelectedIndex != -1)
                return InsertItemPosition.LastItem;
            else
                return InsertItemPosition.None;
        }

        public InsertItemPosition GetInsertItemPositionUpr()
        {
            if (!editable)
                return InsertItemPosition.None;

            if (lvTypy.SelectedIndex != -1)
                return InsertItemPosition.LastItem;
            else
                return InsertItemPosition.None;
        }

        /* edycja pól */

        protected void btnEditFields_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            if (btn != null)
            {
                string arg = btn.CommandArgument;
                if (!String.IsNullOrEmpty(arg))
                {
                    string id = arg;
                    ShowPola(id, L.p("Edycja pól"), true);
                    //Tools.ShowDialog("divZoom", upUprawnienia.ClientID, L.p("Edycja pól"), 500, btClose.ClientID);
                }
            }
        }

        protected void ShowPola(object sender, EventArgs e)
        {
            LinkButton lnk = (sender as LinkButton);
            if (lnk != null)
            {
                string id = lnk.CommandArgument;
                if (!String.IsNullOrEmpty(id))
                {
                    ShowPola(id, L.p("Pola"), false);
                }
            }
        }

        void ShowPola(string id, string title, bool editable)
        {
            Pola.Visible = true;
            Tools.ShowDialog(this, "divZoom", 300, btClose, title);
            Pola.Prepare(id, editable);
        }

        public bool IsSuperUser
        {
            get {
                bool b = App.User.IsSuperuser;
                return App.User.IsSuperuser; 
            }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if(!String.IsNullOrEmpty(id))
            {
                cTransfer.Visible = true;
                cTransfer.UprId = id;
                Tools.ShowDialog(this, "divTransfer", 400, btCloseTransfer, "Przenieś do");

            }
        }

        protected void cTransfer_Transfered(object sender, EventArgs e)
        {
            lvUprawnienia.DataBind();

            UpdatePanel up = Tools.FindUpdatePanel(this);
            if (up != null && up.UpdateMode == UpdatePanelUpdateMode.Conditional)
            {
                up.Update();
            }

            Tools.CloseDialog("divTransfer");
            Tools.CloseDialogOverlay();
            cTransfer.Visible = false;
  
        }
    }
}     