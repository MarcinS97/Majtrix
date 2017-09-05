using HRRcp.App_Code;
using HRRcp.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Uprawnienia
{
    public partial class cntOceny : System.Web.UI.UserControl
    {
        /*
            Statusy:
         *  1. > -1 : hierarhiczna lista statusów, im większy tym ocena jest ważniejsza w danym okresie czasu
         *  2. < 0 : statusy techniczne, nie brane pod uwagę przy wyświetlaniu ocen
         *  Statusy w słowniku: msOcenyStatus
         */

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUserId.Value = App.User.Id;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvOceny, 1337);
        }

        public void Prepare(string certId, string pracId, string uprId, string typ)
        {
            this.Visible = true;

            CertyfikatId = certId;
            PracId = pracId;
            UprId = uprId;
        }

        public bool CanEdit()
        {
            return App.User.IsMSAdmin || App.User.HasRight(AppUser.rMSKorekty);
        }

        public String CertyfikatId
        {
            get { return hidCertyfikatId.Value; }
            set { hidCertyfikatId.Value = value; }
        }

        public String PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public String UprId
        {
            get { return hidUprId.Value; }
            set { hidUprId.Value = value; }
        }

        protected void lvOceny_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            bool b = false;
            DateEdit de1 = lvOceny.InsertItem.FindControl("deOd") as DateEdit;
            DateEdit de2 = lvOceny.InsertItem.FindControl("deDo") as DateEdit;

            if(de1 != null && de2 != null)
            {
                if(de2.Date != null)
                    b = !DateEdit.ValidateRange(de1, de2, 1337); 
            }

            bool correct = Correct(de1.DateStr, de2.DateStr, null);
            if (!correct)
                Tools.ShowMessage("Uwaga! Na podane daty istnieje już korekta oceny. Zmień daty obowiązywania aktualnej korekty aby kontynuować.");
            b = !correct;

            e.Cancel = b;
        }

        protected void lvOceny_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            bool b = false;

            string id = Tools.GetDataKey(lvOceny, e);

            DateEdit de1 = lvOceny.EditItem.FindControl("deOd") as DateEdit;
            DateEdit de2 = lvOceny.EditItem.FindControl("deDo") as DateEdit;

            if (de1 != null && de2 != null)
            {
                if (de2.Date != null)
                    b = !DateEdit.ValidateRange(de1, de2, 1337);
            }


            bool correct = Correct(de1.DateStr, de2.DateStr, id);
            if (!correct)
                Tools.ShowMessage("Uwaga! Na podane daty istnieje już korekta oceny. Zmień daty obowiązywania aktualnej korekty aby kontynuować.");
            b = !correct;


            e.Cancel = b;
        }

        protected void btnZoom_Click(object sender, EventArgs e)
        {
            string p = (sender as LinkButton).CommandArgument;

            if(!String.IsNullOrEmpty(p))
            {
                if (p.Split(';').Length == 3)
                {
                    string pid = p.Split(';')[0];
                    string ood = p.Split(';')[1];
                    string odo = p.Split(';')[2];

                    string x = Tools.SetLineParams("1", pid, ood, odo, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
                    string z = Report.EncryptQueryString(x, Grid.key, Grid.salt);
                    App.Redirect("RaportF.aspx" + "?p=" + z);

                    //Response.Redirect(ResolveUrl(String.Format("~/RaportF.aspx?p={0}", Report.EncryptQueryString(encp, Grid.key, Grid.salt))));
                
                }
                
            }
        }

        public bool Correct(string from, string to, string id)
        {
            return db.Select.Scalar(dsCorrect, PracId, UprId, db.nullParamStr(from), db.nullParamStr(to), db.nullParam(id)) == "1";

        }


    }
}