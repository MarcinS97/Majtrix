using System; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class _testControl1 : System.Web.UI.UserControl
    {
        SqlConnection con;
        string sid;
        bool fBinded = false;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void Prepare()
        {
            sid = Base.UniqueId();
            hidSesId.Value = sid;
            

            con = Base.Connect();
            Base.execSQL(con, "delete from tmpStrefy where sesId = " + Base.strParam(sid));
            Base.execSQL(con, "insert into tmpStrefy select " + Base.strParam(sid) + " as sesId, * from Strefy");
            fBinded = true;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (fBinded)
            {
                Base.execSQL(con, "delete from tmpStrefy where sesId = " + Base.strParam(sid));
                Base.Disconnect(con);
            }
        }
    
    }
}