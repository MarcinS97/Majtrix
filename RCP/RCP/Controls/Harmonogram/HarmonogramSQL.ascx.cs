using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.RCP.Controls.Harmonogram
{
    public partial class HarmonogramSQL : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //public static SQL Load()
        //{
        //    UserControl cnt = new UserControl();
        //    SQL c = (SQL)cnt.LoadControl("~/Controls/PodzialLudzi/SQL.ascx");
        //    return c;
        //}

        private static HarmonogramSQL Frep = null;

        public static HarmonogramSQL Repository
        {
            get
            {
                //if (Frep == null)   // wyłączam zapamiętuje dopóki aktywny klient więc jak sie cos zmieni to nie uaktualnia
                {
                    UserControl cnt = new UserControl();
                    Frep = (HarmonogramSQL)cnt.LoadControl("~/RCP/Controls/Harmonogram/HarmonogramSQL.ascx");
                }
                return Frep;
            }
        }
        //----------------------------------
        public static string SetPracAktywny(int pracId, int act, DateTime data, out string retFmt)
        {
            HarmonogramSQL h = Repository;
            retFmt = h.dsSetPracAktywny.UpdateCommand;
            return String.Format(h.dsSetPracAktywny.SelectCommand, pracId, act, Tools.DateToStrDb(data)); 
        }
    }
}