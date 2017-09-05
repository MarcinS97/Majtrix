using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class AssecoSQL : System.Web.UI.UserControl
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

        private static AssecoSQL Frep = null;

        public static AssecoSQL Repository
        {
            get
            {
                //if (Frep == null)   // wyłączam zapamiętuje dopóki aktywny klient więc jak sie cos zmieni to nie uaktualnia
                {
                    UserControl cnt = new UserControl();
                    Frep = (AssecoSQL)cnt.LoadControl("~/Controls/Adm/AssecoSQL.ascx");
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

        public static string ImportUrlopZbior(int mode, string data)
        {
            return String.Format(Repository.dsImportUrlopZbior.SelectCommand, mode, data);
        }

        public static string ImportUmowy(int mode)
        {
            return String.Format(Repository.dsImportUmowy.SelectCommand, mode);
        }

        public static string ImportUpdateZalegDod(int mode, string data)
        {
            return String.Format(Repository.dsImportUpdateZalegDod.SelectCommand, mode, data);
        }

        /*
        public static void GetImport(out string data, out string imp)
        {
            SQL c = Load();
            data = c.dsGetData.SelectCommand;
            imp = c.dsImport.SelectCommand;
        }
        */
        public static bool GetSql(out string sql, string dsSql, params object[] par)
        {
            string s = dsSql.Trim();
            if (!String.IsNullOrEmpty(s))
            {
                sql = String.Format(s, par);
                return true;
            }
            else
            {
                sql = null;
                return false;
            }
        }

        public static bool ExportAfterRCP(out string sql, int mode, string dOd, string dDo)
        {
            return GetSql(out sql, Repository.dsExportAfterRCP.SelectCommand, mode, dOd, dDo);
        }

        public static bool ExportAfterAsseco(out string sql, int mode, string dOd, string dDo)
        {
            return GetSql(out sql, Repository.dsExportAfterAsseco.SelectCommand, mode, dOd, dDo);
        }

        public static string ImportFindPracSql
        {
            get { return Repository.dsImportFindPrac.SelectCommand; }
        }
    }
}