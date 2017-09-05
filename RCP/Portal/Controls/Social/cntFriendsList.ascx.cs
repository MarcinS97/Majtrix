using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Social
{
    public partial class cntFriendsList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUser.Value = App.User.Id;
                Prepare();
            }
        }

        public void Prepare()
        {
            switch (Mode)
            {
                case EMode.Search:
                    lvFriends.DataSource = dsSearch;
                    break;
                case EMode.My:
                    lvFriends.DataSource = dsMy;
                    divSearch.Visible = false;
                    break;
                case EMode.InvitationsToMe:
                    lvFriends.DataSource = dsInvitationsToMe;
                    divSearch.Visible = false;
                    break;
                case EMode.InvitationsFromMe:
                    lvFriends.DataSource = dsInvitationsFromMe;
                    divSearch.Visible = false;
                    break;
                default:
                    lvFriends.DataSource = dsSearch;
                    break;
            }
            lvFriends.DataBind();

            SetPager(Mode == EMode.Search || Mode == EMode.My);

        }

        void SetPager(bool b)
        {
            HtmlGenericControl div = lvFriends.FindControl("divPager") as HtmlGenericControl;
            if (div != null)
            {
                div.Visible = b;
            }
        }

        public String GetAvatar(String kadryId)
        {
            return Tools.GetUserAvatar(kadryId);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Prepare();
            //string text = tbSearchFriend.Text;
            //lvFriends.DataBind();
            //upMain.Update();
        }


        public enum EMode { Search, My, InvitationsToMe, InvitationsFromMe };
        private EMode _mode = EMode.Search;
        public EMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        protected void dsSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //e.Command.Parameters["@search"].Value = tbSearchFriend.Text;
        }

        protected void btnInviteFriend_Click(object sender, EventArgs e)
        {
            string id = Tools.GetCommandArgument(sender);
            db.Execute(dsInviteFriend, App.User.Id, id);
            Prepare();
        }

        protected void btnAcceptInvitation_Click(object sender, EventArgs e)
        {
            string id = Tools.GetCommandArgument(sender);
            db.Execute(dsAcceptInvitation, id);
            Prepare();
        }

        protected void lnkProfile_Click(object sender, EventArgs e)
        {
            String Id = (sender as LinkButton).CommandArgument;
            Response.Redirect("~/Portal/Social/Profil.aspx?p=" + Id);
        }

        
    }
}