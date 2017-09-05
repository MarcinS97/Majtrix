using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
namespace HRRcp.Controls.WnioseZmianaDanych
{
    public partial class Wniosek : System.Web.UI.UserControl
    {
        protected void Zoom(object sender, EventArgs e)
        {
            Zoom1.bind();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

            
            GetName1.Zoombind += new EventHandler(Zoom);
            String Type = Request.QueryString["Type"];
            String Id = Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Id)) GetName1.Query(Id, Type);
            PropertyInfo isreadonly =
   typeof(System.Collections.Specialized.NameValueCollection).GetProperty(
   "IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove("Id");
            this.Request.QueryString.Remove("Type");
        }
    }
}