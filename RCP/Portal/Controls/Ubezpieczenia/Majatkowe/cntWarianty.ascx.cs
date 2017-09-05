using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe
{
    public partial class cntWarianty : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Tools.ExecuteJavascript("cntWarianty();");
        }

        public String GetSelectedParId()
        {
            String parId = String.Empty;
            foreach (RepeaterItem item in rpItems.Items)
            {
                CheckBox cb = item.FindControl("cbSkladka") as CheckBox;
                if (cb != null)
                {
                    if (cb.Checked)
                    {
                        String Id = Tools.GetText(item, "hidId");
                        if (!String.IsNullOrEmpty(Id))
                        {
                            return Id;
                        }
                        break;
                    }
                }
            }
            return null;
        }

        public bool GetSelectedPlus()
        {
            foreach (RepeaterItem item in rpItems.Items)
            {
                CheckBox cbPlus = item.FindControl("cbSkladkaPlus") as CheckBox;
                if (cbPlus != null)
                {
                    String Id = Tools.GetText(item, "hidId");
                    if (!String.IsNullOrEmpty(Id))
                    {
                        if (cbPlus.Checked)
                            return true;
                    }
                }
            }
            return false;
        }

        public String GetSelectedPlusId()
        {
            foreach (RepeaterItem item in rpItems.Items)
            {
                CheckBox cbPlus = item.FindControl("cbSkladkaPlus") as CheckBox;
                if (cbPlus != null && cbPlus.Checked)
                {
                    String Id = Tools.GetText(item, "hidId");
                    if (!String.IsNullOrEmpty(Id))
                    {
                        return Id;
                    }
                    break;
                }

            }
            return null;
        }


        //public void Prepare(String Rodzaj, String ParId, bool Plus, Boolean Edit)
        //{
        //    this.Edit = Edit;
        //    this.Rodzaj = Rodzaj;
        //    rpItems.DataBind();
        //    if (!String.IsNullOrEmpty(ParId))
        //    {
        //        foreach (RepeaterItem item in rpItems.Items)
        //        {
        //            String SID = Tools.GetText(item, "hidId");
        //            if (SID == ParId)
        //            {
        //                Tools.SetChecked(item, "cbSkladka", true);
        //                Tools.SetChecked(item, "cbSkladkaPlus", true);
        //            }
        //        }
        //    }
        //}

        public void Prepare(String Rodzaj, String ParId, String PlusId, Boolean Edit)
        {
            this.Edit = Edit;
            this.Rodzaj = Rodzaj;
            rpItems.DataBind();
            if (!String.IsNullOrEmpty(ParId))
                CheckPar(ParId);
            if (!String.IsNullOrEmpty(PlusId))
                CheckPlus(PlusId);
        }

        private void CheckPar(String Id)
        {
            foreach (RepeaterItem item in rpItems.Items)
            {
                String SID = Tools.GetText(item, "hidId");
                if (SID == Id)
                {
                    Tools.SetChecked(item, "cbSkladka", true);
                }
            }

        }

        private void CheckPlus(String Id)
        {
            foreach (RepeaterItem item in rpItems.Items)
            {
                String SID = Tools.GetText(item, "hidId");
                if (SID == Id)
                {
                    Tools.SetChecked(item, "cbSkladkaPlus", true);
                }
            }
        }

        public void Update(string rodzaj)
        {
            Rodzaj = rodzaj;
            rpItems.DataBind();
        }


        public Boolean Edit
        {
            get { return Tools.GetViewStateBool(ViewState["vEdit"], false); }
            set { ViewState["vEdit"] = value; }
        }

        public String ExcludedId
        {
            get { return hidExcludedId.Value; }
            set { hidExcludedId.Value = value; }
        }
        
        public String Rodzaj
        {
            get { return hidRodzaj.Value; }
            set { hidRodzaj.Value = value; }
        }

    }
}