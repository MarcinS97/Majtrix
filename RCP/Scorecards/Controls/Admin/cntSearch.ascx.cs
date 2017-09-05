using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

/* SPOSÓB UŻYCIA

 *  1. Na wewnętrzną tabelke w listview kładziemy classe jako identyfikator tabelki (TableName)
 *  2. Na td, który chcemy wyszukiwać kładziemy id (ColumnName)
 *  3. W td musi być span
 *
*/

namespace HRRcp.Scorecards.Controls.Admin
{
    public partial class cntSearch : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Tools.ExecOnStart2("searchS" + this.ClientID, String.Format("prepareSearch('{0}', '{1}', '{2}'); ", tbSearch.ClientID, TableName, ColumnName));
        }

        public String TableName
        {
            get { return ViewState["vTableId"] as String; }
            set { ViewState["vTableId"] = value; }
        }

        public String ColumnName
        {
            get { return ViewState["vColumnName"] as String; }
            set { ViewState["vColumnName"] = value; }
        }
    }
}