using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Social
{
    public partial class Profil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String UserId = App.User.Id;
                if (Request.QueryString["p"] != null)
                {
                    UserId = Request.QueryString["p"];
                }
                cntSqlTabs.SQL = String.Format(cntSqlTabs.SQL, App.dbPORTAL);
                Prepare(UserId);
            }

            Tools.ExecuteJavascript(String.Format("prepareTinyMce('#{0}');", htmlEditor.ClientID));
            if (!IsPostBack)
            {
                btSave.OnClientClick = String.Format("getEditor('{0}','{1}');", hidEditorData.ClientID, htmlEditor.ClientID);
                btCancel.OnClientClick = String.Format("getEditor('{0}','{1}');", hidEditorData.ClientID, htmlEditor.ClientID);
            }
            //Tools.ExecuteJavascript(String.Format("initEditor('{0}','{1}');", hidEditorData.ClientID, htmlEditor.ClientID));
        }

        void Prepare(String UserId)
        {
            hidUser.Value = UserId;

            DataRow data = GetUserData(UserId);
            lblName.Text = db.getValue(data, "ImieNazwisko");
            lblImieNazwisko.Text = db.getValue(data, "ImieNazwisko");
            lblEmail.Text = db.getValue(data, "Email");
            lblStanowisko.Text = db.getValue(data, "Stan");
            cntAvatar.NrEw = db.getValue(data, "KadryId");
        }

        private void SelectTab()
        {
            string typ = cntSqlTabs.SelectedValue;
            DataRow dr = db.getDataRow(db.conP, String.Format("select * from poProfile where IdPracownika = {0} and Typ = {1}", App.User.Id, typ));
            ProfileId = db.getValue(dr, "Id");
            string s = db.getValue(dr, "Tekst");
            s = System.Web.HttpUtility.HtmlDecode(s);
            litContent.Text = s;
        }
        
        private string ProfileId
        {
            set { ViewState["profid"] = value; }
            get { return Tools.GetStr(ViewState["profid"]); }
        }

        protected void cntSqlTabs_SelectTab(object sender, EventArgs e)
        {
            //Tools.SetViewById(mvViews, cntSqlTabs.SelectedValue);
            SelectTab();
        }

        private bool Save()
        {
            string text = db.sqlPut(hidEditorData.Value);
            string pid = ProfileId;
            bool ok;
            if (String.IsNullOrEmpty(pid))
                ok = db.insert(db.conP, "poProfile", 0, "IdPracownika,Typ,Tekst", App.User.Id, cntSqlTabs.SelectedValue, db.nullStrParam(text));
            else
                ok = db.update(db.conP, "poProfile", 0, "Tekst,DataAktualizacji", "Id=" + pid, db.nullStrParam(text), "GETDATE()");
            text = System.Web.HttpUtility.HtmlDecode(text);
            if (ok)
                litContent.Text = text;
            else
                htmlEditor.Text = text;
            return ok;
        }

        private void SetEditMode(bool edit)
        {
            Editable = edit;
            htmlEditor.Visible = edit;
            litContent.Visible = !edit;
            btnEditContent.Visible = !edit;
            btSave.Visible = edit;
            btCancel.Visible = edit;
            cntSqlTabs.Tabs.Enabled = !edit;
        }

        protected void btnEditContent_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
            htmlEditor.Text = litContent.Text;
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Save())
                SetEditMode(false);
            else
                Tools.ShowError("Wystąpił błąd podczas zapisu.");
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
        }

        DataRow GetUserData(String Id)
        {
            DataRow data = db.Select.Row(dsData, Id);
            return data;
        }

        public bool Editable
        {
            get { return Tools.GetViewStateBool(ViewState["vEditable"], false); }
            set { ViewState["vEditable"] = value; }
        }

        protected void lnkSearchFriends_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Portal/Social/Znajomi.aspx");
        }

        protected void lnkSendMsg_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Portal/Social/Chat.aspx");
        }

        protected void cntSqlTabs_DataBound(object sender, EventArgs e)
        {
            SelectTab();
        }


        protected void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            cntAvatarEdit.Show();
        }

    }
}