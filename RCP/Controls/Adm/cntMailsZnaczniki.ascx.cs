using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Mails
{
    public partial class cntMailsZnaczniki : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvZnaczniki, 0);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string grupa)   //, string mailTextBoxID)
        {
            hidGrupa.Value = grupa;
            lvZnaczniki.DataBind();
        }

        //-------------------------------
        protected void lvZnaczniki_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            switch (Tools.GetListItemMode(e, lvZnaczniki))
            {
                case Tools.limSelect:
                    bool ed = lvZnaczniki.EditIndex == -1;
                    Tools.SetControlEnabled(e.Item, "EditButton", ed);
                    Button bt = (Button)Tools.SetControlEnabled(e.Item, "btSelect", ed);
                    if (bt != null)
                    {
                        DataRowView drv = Tools.GetDataRowView(e);
                        bt.Attributes["onclick"] = Mailing.znacznikOnClick2(drv["Znacznik"].ToString()) + "return false;";
                    }
                    break;
            }
        }

        protected void lvZnaczniki_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.NewValues["Znacznik"] = Mailing.AsZnacznik(e.NewValues["Znacznik"].ToString());
        }

        protected void lvZnaczniki_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Values["Znacznik"] = Mailing.AsZnacznik(e.Values["Znacznik"].ToString());
        }
        //-------------------------------
        
        public string Grupa
        {
            get { return hidGrupa.Value; }
            set { Prepare(value); }
        }
    }
}