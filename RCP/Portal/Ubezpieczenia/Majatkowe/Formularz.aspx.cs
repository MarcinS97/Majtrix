using HRRcp.App_Code;
using HRRcp.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Ubezpieczenia
{
    public partial class Formularz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btFormularz_Click(object sender, EventArgs e)
        {
            cntWnioskiMajatkoweModal.Show(null);
        }
     

        protected void cntWnioskiMajatkoweLista_Show(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            Tools.ShowMessage(lv.SelectedValue.ToString());
        }


        bool showing = false;

        protected void cntWnioskiMajatkoweLista_Show1(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            String id = lv.SelectedValue.ToString();
            cntWnioskiMajatkoweModal.Show(id);
            showing = true;
        }

        protected void cntWnioskiMajatkoweModal_Saved(object sender, EventArgs e)
        {
            Tools.ShowMessage("Dziękujemy za złożenie wniosku, polisa zostanie wysłana na adres korespondencyjny. Już dziś przyjdź do działu HR po niespodziankę.");
            UpdatePanel1.Update();
            cntWnioskiMajatkoweLista.DataBind();
        }

        protected void cntLista_ListDataBound(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            if(lv != null && !showing)
            {
                if(lv.Items.Count > 0)
                {
                    string force = Request.QueryString["force"];
                    if(!String.IsNullOrEmpty(force))
                        cntWnioskiMajatkoweModal.Show(null);
                }
                else
                {
                    cntWnioskiMajatkoweModal.Show(null);
                }
            }

        }
    }
}