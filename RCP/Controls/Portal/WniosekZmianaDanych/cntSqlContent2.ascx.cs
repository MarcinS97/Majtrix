using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;

namespace HRRcp.Controls.Portal
{
    public partial class cntSqlContent3 : System.Web.UI.UserControl
    {
        public EventHandler TurboClick3;

        public event EventHandler SelectTab;

        public const int moLines    = 1;
        public const int moScreen   = 2;
        public const int moMDLines  = 3;    // master lines - details lines  <<< rozwój :)
        public const int moMDScreen = 4;    // master lines - details lines


        protected void TurboClick2(object sender, EventArgs e)
        {
           
            if (TurboClick3 != null)
                TurboClick3(sender, null);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cntMasterLines.TurboClick += new EventHandler(TurboClick2);
            if (!IsPostBack)
            {
                    
                
            }
        }

        public void Prepare(string grupa)
        {
            Grupa = grupa;
            
        }



        private void TriggerSelectTab()
        {
            if (SelectTab != null)
                SelectTab(this, EventArgs.Empty);
        }

        //---- parametry do OnSelectTab
        public int Typ = -1;

        private string PrepareParams(string sql)        // podmiana parametrów @p1
        {
            if (!String.IsNullOrEmpty(sql))
            {

                AppUser user = AppUser.CreateOrGetSession();
                sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
                if (!string.IsNullOrEmpty((cntMasterLines.FindControl("PracId") as HiddenField).Value.ToString()))
                {
                    DataRow dr = db.getDataRow(string.Format(@"select KadryId2 as NrEwid from Pracownicy where Id={0}", (cntMasterLines.FindControl("PracId") as HiddenField).Value ));
                    
                    sql = sql.Replace("@KadryId2", db.getValue(dr, "NrEwid"));// to najpierw
                }
                else
                {

                    sql = sql.Replace("@KadryId2", db.strParam(user.KadryId2));// to najpierw

                }
                sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
                sql = sql.Replace("@Login", db.strParam(user.Login));
            }
            return sql;
        }

        public void DoSelectTab_wn(String Id)
        {

            string id = Id;
            DataRow dr = db.getDataRow(String.Format("select * from {0}..SqlContent where Id = {1}", App.dbPORTAL, id));
            string constr = db.getValue(dr, "ConStr");
            string sql = db.getValue(dr, "Sql");
            int typ = db.getInt(dr, "Typ", moLines);
            Typ = typ;
            cntMasterLines.Visible = false;
            
            switch (typ)
            {
                default:
                case moLines:
                    
                    cntMasterLines.Visible = true;
                    cntMasterLines.SQL = sql;
                    cntMasterLines.ConStr = constr;
                    TriggerSelectTab();
                    cntMasterLines.Prepare();
                    break;

                
            }
        }





        //------------------------------------------
        public string Grupa
        {
            set { ViewState["grupa"] = value; }
            get { return Tools.GetStr(ViewState["grupa"]); }
        }

        

    }
}