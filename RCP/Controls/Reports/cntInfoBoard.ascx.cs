using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntInfoBoard : System.Web.UI.UserControl
    {
        const string STARTPAGE = "STARTPAGE";

        public delegate void ESelectedChanged(string id, string value, string par1);
        public event ESelectedChanged SelectedChanged;

        string FGrupa = null;
        string FSelectCommand = null;   // lowercase

        bool adm = true;

        int FMode = moKier;
        const int moKier = 1;
        const int moAll = 2;   // -> na Par1 jest filtr co ma się gdzie pokazywać 1,2,3

        //--------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cntInfoBoxEdit.Visible = adm;
                hidRights.Value = App.User.Rights;
                UpdateAll();
                Tools.SetMainServiceUrl();
            }
            else
            {
                Tools.ExecOnStart2("ibshow", "showInfoBoxes();prepareInfoBoxes();");
            }
        }

        private void UpdateAll()
        {
            string s = null;

        }
        //--------------------------------
        protected void SqlDataSource3_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }
        //--------------------------------
        public string Grupa
        {
            set
            {
                FGrupa = value;
                hidGrupa.Value = value;
            }
            get { return FGrupa; }
        }

        public int Mode
        {
            set
            {
                hidMode.Value = value.ToString();
                FMode = value;
            }
            get { return FMode; }
        }

        public int ClickedIndex   // z acordeon'a
        {
            set { ViewState["clidx"] = value; }
            get { return Tools.GetInt(ViewState["clidx"], -1); }
        }

        public string SelectCommand   // komenda do zaznaczenia, ustawiana przed DataBind - powoduje rozwinięcie acordeonu i zaznaczenie zgodniej opcji 
        {
            set { FSelectCommand = Tools.GetRedirectUrl(value).ToLower(); }
        }

        protected void rpBoxes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string par = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "BOXCLICK":
                    //string cmd = db.selectScalar("select Cmd from SqlBoxes where Id = {0}", id);
                    string url;
                    if (Tools.IsUrl(par, out url))  // url:, http:
                        App.Redirect(url);
                    else
                        App.Redirect(url);  // to samo to nie ma znaczenie 
                    break;
                case "BOXEDIT":
                    cntInfoBoxEdit.Show(par, STARTPAGE);   //id
                    break;
                case "BOXNEW":
                    cntInfoBoxEdit.Add(par, STARTPAGE);
                    break;
                case "BOXDELETE":
                    cntInfoBoxEdit.Delete(par);
                    break;
                case "BOXPREV":
                    break;
                case "BOXNEXT":
                    break;
                case "BOXUP":
                    break;
                case "BOXDOWN":
                    break;
                case "BOXSHOWALL":  // pokazuje/ukrywa nieaktywne
                    //hidShowAll.Value = hidShowAll.Value == "1" ? "0" : "1";
                    ShowAll = !ShowAll;
                    UpdateView();
                    break;
                case "BOXNEWLINE":  // nie ma takiego klawisza jeszcze  
                    bool ok = db.execSQL(String.Format("", par));
                    if (ok) UpdateView();
                    break;
            }
        }

        private void UpdateView()
        {
            rpBoxes.DataBind();
            UpdatePanel up = Tools.FindUpdatePanel(this);    // nie znika jquery dialog background
            if (up != null && up.UpdateMode == UpdatePanelUpdateMode.Conditional)
            {
                up.Update();
                Tools.CloseDialogOverlay();   // troche plomba ale działa ...
            }
        }

        protected void cntInfoBoxEdit_Save(object sender, EventArgs e)
        {
            UpdateView();
        }
        //-----------------------
        public bool ShowAll
        {
            set { hidShowAll.Value = value ? "1" : "0"; }
            get { return hidShowAll.Value == "1"; }
        }
    }
}