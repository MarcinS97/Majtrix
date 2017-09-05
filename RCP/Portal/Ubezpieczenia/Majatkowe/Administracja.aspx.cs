using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Ubezpieczenia.Majatkowe
{
    public partial class UbezpAdministracja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            //get { return App.User.IsKierownik || App.User.IsAdmin; }
            get
            {
                //return App.User.IsAdmin; 
                //return db.SqlMenuHasRights(db.conP, 100019, App.User);   // <<< DO ZMIANY !!! to muszą być stałe identyfiktaory !!!, teraz od tego zależy koleność w menu :'(
                return App.User.HasRight(AppUser.rUbezpieczeniaAdm);        
            }
        }

    }
}