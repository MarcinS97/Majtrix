using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntModalPopup2 : System.Web.UI.UserControl
    {
        private ITemplate headerTemplate = null;
        private ITemplate contentTemplate = null;
        private ITemplate footerTemplate = null;

        [TemplateContainer(typeof(HeaderContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate HeaderTemplate
        {
            get { return headerTemplate; }
            set { headerTemplate = value; }
        }

        [TemplateContainer(typeof(ContentContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate ContentTemplate
        {
            get { return contentTemplate; }
            set { contentTemplate = value; }
        }

        [TemplateContainer(typeof(FooterContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate FooterTemplate
        {
            get { return footerTemplate; }
            set { footerTemplate = value; }
        }


        void Page_Init()
        {
            if (headerTemplate != null)
            {
                HeaderContainer Container = new HeaderContainer();
                HeaderTemplate.InstantiateIn(Container);
                phHeader.Controls.Add(Container);
            }

            if (contentTemplate != null)
            {
                ContentContainer Container = new ContentContainer();
                ContentTemplate.InstantiateIn(Container);
                phContent.Controls.Add(Container);
            }

            if (footerTemplate != null)
            {
                ContentContainer Container = new ContentContainer();
                FooterTemplate.InstantiateIn(Container);
                phFooter.Controls.Add(Container);
            }
        }


        public void Show(bool clear)
        {
            this.Visible = true;
            if(clear)
                ClearControl(this);
            Tools.ExecuteJavascript(String.Format("mp2.showPopup('{0}');", popup.ClientID));
        }

        public void Show()
        {
            Show(true);
        }

        private void ClearControl(Control control)
        {
            var textbox = control as TextBox;
            if (textbox != null)
                textbox.Text = string.Empty;

            var dropDownList = control as DropDownList;
            if (dropDownList != null)
                dropDownList.SelectedIndex = 0;

            var hiddenField = control as HiddenField;
            if (hiddenField != null)
                hiddenField.Value = string.Empty;

            foreach (Control childControl in control.Controls)
            {
                ClearControl(childControl);
            }
        }


        public String Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public String CssClass
        {
            get { return popup.Attributes["class"].ToString(); }
            set { popup.Attributes["class"] = "popup" + (!String.IsNullOrEmpty(value) ? " " + value : String.Empty); }
        }

        protected void lnkPopupCloser_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }




    }

    public class HeaderContainer : Control, INamingContainer
    {
    }

    public class ContentContainer : Control, INamingContainer
    {
    }

    public class FooterContainer : Control, INamingContainer
    {
    }
}