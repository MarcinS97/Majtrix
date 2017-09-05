using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using HRRcp.App_Code;

namespace HRRcp.Scorecards
{
    public partial class JSON_read : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String input = null;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd ();
            }
            if (!String.IsNullOrEmpty(input))
            {
                Response.Write("\n----BEGIN DATA READ----\n");
                Response.Write(input);
                Response.Write("\n----END DATA READ----\n");
                //Log.Info(-1337, "JSON input", input);
                Base.execSQL(String.Format("insert into Log (DataCzas, ParentId, Typ, Typ2, PracId, Login, Info, Info2, Status, AppID, UserIP, SesID) values (GETDATE(), 0, 0, -1337, 1, 'aoe', 'JSON input', '{0}', 0, 'R', '0.0.0.0', '0')", input));
            }
        }
    }
}
