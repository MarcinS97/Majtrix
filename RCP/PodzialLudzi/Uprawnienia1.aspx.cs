using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class Uprawnienia1 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasRight(AppUser.rPodzialLudziAdm))
                {
                    /* wersja bez ajaxa z odwolaniem do strony 
                    string p1 = Request.QueryString["p1"];
                    if (!String.IsNullOrEmpty(p1))
                        Update(p1, Request.QueryString["p2"], Request.QueryString["p3"]);
                     */
                }
                else
                    App.ShowNoAccess("Podział Ludzi - Uprawnienia", App.User);
            }
        }
        //----------------------------
        private int UpdateRights(string nrew, string r)
        {
            DataRow drUser = db.getDataRow("select Rights, Nazwisko + ' ' + Imie as Pracownik from Pracownicy where KadryId = " + db.strParam(nrew));
            
            if (drUser != null)
            {
                string rights = db.getValue(drUser, 0);
                if (rights.Trim().Length < AppUser.maxRight) rights = (rights.Trim() + Tools.StrRepeat("0", AppUser.maxRight)).Substring(0, AppUser.maxRight);
                char[] ra = rights.ToCharArray();
                int p = Tools.StrToInt(r, -1);
                if (0 <= p && p < ra.Length)
                {
                    bool b = ra[p] == '1';
                    ra[p] = b ? '0' : '1';
                    rights = new string(ra);
                    bool ok = db.update("Pracownicy", 1, "Rights", "KadryId='{0}'", nrew, db.strParam(rights));

                    if (ok)
                    {
                        Log.Info(Log.RIGHTS,
                            b ? "Usunięcie uprawnienia" : "Nadanie uprawnienia",
                            String.Format("Pracownik: {0} {1} Uprawnienie: {2} {3}", nrew, db.getValue(drUser, 1), r, AppUser.GetRightName(p)));
                        return b ? 0 : 1;
                    }
                    else
                    {
                        Tools.ShowErrorLog(Log.RIGHTS,
                            String.Format("RepCCUprawnienia.UpdateRights('{0}','{1}')", nrew, r),
                            "Błąd podczas nadawania / usuwania uprawnienia.");
                        return -2;
                    }
                }
            }
            Tools.ShowErrorLog(Log.RIGHTS, 
                String.Format("RepCCUprawnienia.UpdateRights('{0}','{1}')", nrew, r), 
                "Niepoprawne parametry wywołania.");
            return -1;
        }

        private int UpdateClass(bool cc, string nrew, string c)
        {
            DataRow drUser = db.getDataRow(String.Format(
                "select Id, Login, Nazwisko + ' ' + Imie as Pracownik from Pracownicy where KadryId = '{0}'", nrew));
            if (drUser != null)
            {
                DataRow drPrawa = db.getDataRow(String.Format(
                    "select Id from ccPrawa where UserId={0} and CC='{1}'",
                    db.getValue(drUser, 0), c));
                bool b;  // zdjecie uprawnien
                bool ok;
                if (drPrawa == null)
                {
                    b = false;
                    string idcc = null;
                    if (cc) idcc = db.getScalar(String.Format("select Id from CC where cc = '{0}'", c));
                    if (String.IsNullOrEmpty(idcc)) idcc = "0";
                    ok = db.insert("ccPrawa", 0, "UserId,Login,IdCC,CC",
                        db.getInt(drUser, 0),
                        db.strParam(db.getValue(drUser, 1)),
                        idcc,
                        db.strParam(c));
                }
                else
                {
                    b = true;
                    ok = db.execSQL("delete from ccPrawa where Id = " + db.getValue(drPrawa, 0));

                }
                if (ok)
                {
                    Log.Info(Log.RIGHTS,
                        b ? "Usunięcie uprawnienia" : "Nadanie uprawnienia",
                        String.Format("Pracownik: {0} {1} Klasyfikacja: {2}", nrew, db.getValue(drUser, 2), c));
                    return b ? 0 : 1;
                }
                else
                {
                    Tools.ShowErrorLog(Log.RIGHTS,
                        String.Format("RepCCUprawnienia.UpdateClass({0},'{1}','{2}')", cc ? 1 : 0, nrew, c),
                        "Błąd podczas nadawania / usuwania uprawnienia.");
                    return -2;
                }
            }
            Tools.ShowErrorLog(Log.RIGHTS,
                String.Format("RepCCUprawnienia.UpdateRights('{0}','{1}')", nrew, c),
                "Niepoprawne parametry wywołania.");
            return -1;
        }

        private void Update(string mode, string nrew, string right)
        {
            string p1 = hidP1.Value;
            if (!String.IsNullOrEmpty(p1))
            {
                string p2 = hidP2.Value;
                string p3 = hidP3.Value;
                switch (p1)
                {
                    case "1":
                        UpdateRights(p2, p3);
                        break;
                    case "2":
                        UpdateClass(false, p2, p3);
                        break;
                    case "3":
                        UpdateClass(true, p2, p3);
                        break;
                }
                cntReport1.DataBind();
            }
        }
        //----------------------------
        protected void btChange_Click(object sender, EventArgs e)
        {
            //Tools.ShowMessage("Change {0} {1} {2}", hidP1.Value, hidP2.Value, hidP3.Value);
            Update(hidP1.Value, hidP2.Value, hidP3.Value);    
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            string filename = lbTitle.Text;
            App_Code.Report.ExportExcel(hidReport.Value, filename, null);
        }
    }
}
