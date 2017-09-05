using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.RCP.Controls
{
    public interface IModallable
    {
        void Show();
        void Hide();
    }

    public partial class cntModal : System.Web.UI.UserControl
    {
        private ITemplate headerTemplate = null;
        private ITemplate contentTemplate = null;
        private ITemplate footerTemplate = null;

        [TemplateContainer(typeof(HeaderContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
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
                ContentContainer Container = new ContentContainer(); ;
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

        public static void ExecOnStart2(string scname, string script)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, typeof(Page), scname, script, true);
        }

        public void Show(bool clear)
        {
            this.Visible = true;
            if (clear)
                ClearControl(this);
            //Tools.ShowBSDialog(cntModalDiv.ClientID, RawOptions ?? GetOptions());
            //ExecOnStart2(this.ClientID + "_showscript1337", String.Format("javascript:$('#{0}').modal({{{1}}});prepareModal('{0}');", cntModalDiv.ClientID, RawOptions ?? GetOptions()));
            ExecOnStart2(this.ClientID + "_showscript1337", String.Format("prepareModal('{0}', {{{1}}});", cntModalDiv.ClientID, RawOptions ?? GetOptions()));
        }

        public void Show()
        {
            Show(true);
        }

        public void Close()
        {
            //ExecOnStart2("_closescript1337", "javascript:$('.modal.in').modal('hide');");
            ExecOnStart2("_closescript1337", String.Format("javascript:$('#{0}').modal('hide');", cntModalDiv.ClientID));
        }

        public void Update()
        {
            upModal.Update();
        }

        private void ClearControl(Control control)
        {
            var textbox = control as TextBox;
            if (textbox != null)
                textbox.Text = string.Empty;

            var dropDownList = control as DropDownList;
            if (dropDownList != null)
                if(dropDownList.Items.Count > 0)
                    dropDownList.SelectedIndex = -1;

            var hiddenField = control as HiddenField;
            if (hiddenField != null)
                hiddenField.Value = string.Empty;

            foreach (Control childControl in control.Controls)
            {
                ClearControl(childControl);
            }
        }

        public string GetOptions()
        {
            string opt = "";
            if (!Keyboard)
                opt += "keyboard: false,";
            if (!Backdrop)
                opt += "backdrop: 'static'";

            if (opt.EndsWith(","))
                opt.Substring(opt.Length - 1, 1);
            return opt;
        }


        public String Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public String CssClass
        {
            get { return cntModalDiv.Attributes["class"].ToString(); }
            set { cntModalDiv.Attributes["class"] = "modal fade" + (!String.IsNullOrEmpty(value) ? " " + value : String.Empty); }
        }

        public Boolean ShowCloseButton
        {
            set { btnClose.Visible = value; }
        }

        public string CloseButtonText
        {
            set { btnClose.Text = value; }
            get { return btnClose.Text; }
        }

        public Boolean ShowXCloseButton
        {
            set { btnXCloseButton.Visible = value; }
        }

        public string RawOptions
        {
            get;
            set;
        }

        public bool Keyboard
        {
            get { return Tools.GetViewStateBool(ViewState["vKeyboard"], true); }
            set { ViewState["vKeyboard"] = value; }
        }

        public bool Backdrop
        {
            get { return Tools.GetViewStateBool(ViewState["vBackdrop"], true); }
            set { ViewState["vBackdrop"] = value; }
        }

        public bool ShowHeader
        {
            set { divHeader.Visible = value; }
        }

        public bool ShowFooter
        {
            set { divFooter.Visible = value; }
        }

        public enum EWidthType { Default, Small, Large };
        private EWidthType _widthType = EWidthType.Default;
        public EWidthType WidthType
        {
            get { return _widthType; }
            set
            {
                _widthType = value;
                switch(_widthType)
                {
                    case EWidthType.Small:
                        modalDialog.Attributes["class"] = "modal-dialog modal-sm";
                        break;
                    case EWidthType.Large:
                        modalDialog.Attributes["class"] = "modal-dialog modal-lg";
                        break;
                    case EWidthType.Default:
                    default:
                        modalDialog.Attributes["class"] = "modal-dialog";
                        break;
                }
            }
        }

        public String Width
        {
            get { return Tools.GetViewStateStr(ViewState["vWidth"]); }
            set 
            { 
                ViewState["vWidth"] = value;
                modalDialog.Attributes["style"] = String.Format("width: {0} !important", String.IsNullOrEmpty(value) ? "auto" : value);
            }
        }

       
        public UpdatePanel upConditional
        {
            get { return upModal; }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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