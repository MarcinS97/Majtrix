using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

/*
 UWAGA !!!
  
 div'y w ItemTemplate muszą być runat=server bo inaczej znika zawartość  
 */


namespace HRRcp.Portal.Controls
{
    public partial class cntWnioskiUrlopoweSelect : System.Web.UI.UserControl
    {
        public event EventHandler Select;
        public string SelectedTyp = null;

        int FMode = moPracownik;
        const int moPracownik = 0;
        const int moKierownik = 1;
        const int moAdmin = 2;
        const int moConfig = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                hidPracId.Value = App.User.Id;
            Tools.ExecuteJavascript("cntWnioskiUrlopoweSelect();");
        }

        protected string GetPath(object ico)
        {
            return App.ImagesPathUrlopy + ico.ToString(); 
        }

        protected string GetText(object text)
        {
            return text.ToString().Replace("\n", "<br />");
        }

        protected bool IsAdmin()
        {
            return FMode == moAdmin ? true : false;
        }

        protected bool IsConfig()
        {
            return FMode == moConfig ? true : false;
        }

        protected InsertItemPosition GetInsertPosition()
        {
            return FMode == moConfig ? InsertItemPosition.LastItem : InsertItemPosition.None;
        }
        
        public int Mode
        {
            set 
            { 
                FMode = value;
                SqlDataSource1.SelectParameters["mode"].DefaultValue = value.ToString();
            }
            get { return FMode; }
        }

        protected void lvTypy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "select":
                    if (Select != null)
                    {
                        SelectedTyp = e.CommandArgument.ToString();
                        Select(this, EventArgs.Empty);
                    }
                    break;
            }
        }

        protected void lvTypy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}