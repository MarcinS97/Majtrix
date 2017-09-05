using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Custom
{
    public class cntAutoComplete : TextBox
    {
        class Options
        {
            public List<string> local { get; set; }

        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.CssClass = "form-control";
            
            DataTable dt = db.Select.Table(SQL);
            Options opt = new Options();

            opt.local = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                opt.local.Add(db.getStr(dr[0], ""));
            }
            Tools.ExecuteJavascript(String.Format(@"$('#{0}').typeahead({1});$('.tt-query').css('background-color','#fff');", this.ClientID, Newtonsoft.Json.JsonConvert.SerializeObject(opt)));
        }

        public String SQL
        {
            get { return Tools.GetStr(ViewState["vSQL"], ""); }
            set { ViewState["vSQL"] = value; }
        }


    }
}