using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.PodzialLudzi
{
    public partial class SQL : System.Web.UI.UserControl
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

        private static SQL Frep = null;

        public static SQL Repository
        {
            get
            {
                //if (Frep == null)   // wyłączam zapamiętuje dopóki aktywny klient więc jak sie cos zmieni to nie uaktualnia
                {
                    UserControl cnt = new UserControl();
                    Frep = (SQL)cnt.LoadControl("~/Controls/PodzialLudzi/SQL.ascx");
                }
                return Frep;
            }
        }

        public static string Get(string dsId, int typ)
        {
            SqlDataSource ds = (SqlDataSource)Repository.FindControl(dsId);
            if (ds != null)
                switch (typ)
                {
                    case 1: return ds.SelectCommand;
                    case 2: return ds.UpdateCommand;
                    case 3: return ds.InsertCommand;
                    case 4: return ds.DeleteCommand;
                }
            return null;
        }

        public static string Backup(string od, string nadzien, string db)  // od - zaweza zakres, nie jest wykorzystywana, nadzien,db - db..tablica_nadzien
        {
            return String.Format(Repository.dsBackup.SelectCommand, od, nadzien, db);
        }

        public static string GetSplity(string pracId, string kierId, DateTime naDzien)  // od - zaweza zakres, nie jest wykorzystywana, nadzien,db - db..tablica_nadzien
        {
            return String.Format(Repository.dsGetSplity.SelectCommand, pracId, kierId, Tools.DateToStrDb(naDzien));
        }

        public static string GetData()
        {
            return Repository.dsGetData.SelectCommand;
        }

        public static string Import(int mode, string dod, string ddo)
        {
            return String.Format(Repository.dsImport.SelectCommand, mode, dod, ddo);
        }

        public static string OpenSplity(string dod)
        {
            return String.Format(Repository.dsOpenSplity.SelectCommand, dod);
        }

        /*
        public static void GetImport(out string data, out string imp)
        {
            SQL c = Load();
            data = c.dsGetData.SelectCommand;
            imp = c.dsImport.SelectCommand;
        }
        */
    }
}