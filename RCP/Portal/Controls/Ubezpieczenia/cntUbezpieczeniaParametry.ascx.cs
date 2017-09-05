using HRRcp.App_Code;
using HRRcp.Controls.Portal;
using Portal.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public class UbezpieczeniaParametry // straszna klasa statyczna ...
    {
        public const string poTableParametry = "poUbezpieczeniaParametry";

        public static int FId = -1;
        public static string FEmail = null;
        public static string FSkladnik = null;
        public static DateTime? FStartEksportu = null;

        public static DataRow GetData()
        {
            SqlConnection c = db.Connect(db.PORTAL);   // tak ma być bo się wykonuje bez MasterPage
            DataRow dr = db.getDataRow(c, String.Format("select top 1 * from {0}", poTableParametry));
            if (dr != null)
            {
                FId         = db.getInt(dr, "Id", -1);
                FEmail      = db.getValue(dr, "Email");
                FSkladnik   = db.getValue(dr, "Skladnik");
                FStartEksportu = db.getDateTime(dr, "StartEksportu");
            }
            else
            {
                string id;
                FId     = 1;
                FEmail  = null;
                FSkladnik = null;
                FStartEksportu = null;
                //db.insert(c, poTableParametry, 0, "Id,Email,Skladnik,StartEksportu", FId, FEmail, FSkladnik, FStartEksportu);
                db.insert(c, out id, poTableParametry, "Id,Email,Skladnik,StartEksportu", FId, FEmail, FSkladnik, FStartEksportu);
            }
            db.Disconnect(c);
            return dr;
        }

        public static int Id
        {
            get
            {
                if (FId == -1) GetData();
                return FId;
            }
        }

        public static string Email
        {
            set { FEmail = value; }
            get
            {
                if (FId == -1) GetData();
                return FEmail;
            }
        }

        public static string Skladnik
        {
            set { FSkladnik = value; }
            get
            {
                if (FId == -1) GetData();
                return FSkladnik;
            }
        }

        public static DateTime? StartEksportu
        {
            set { FStartEksportu = value; }
            get
            {
                if (FId == -1) GetData();
                return FStartEksportu;
            }
        }
    }
    //--------------------------------------
    //--------------------------------------
    //--------------------------------------
    public partial class cntUbezpieczeniaParametry : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        public static UbezpieczeniaParametry Ustawienia;   // zmienna public static!!!

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillData(false);
            }
        }

        /*
        private void SetEdit(bool edit)
        {
            //btEdit.Visible = !edit;
            //btSave.Visible = edit;
            //btCancel.Visible = edit;

            //dbField.SetMode(this, edit ? dbField.moEdit : dbField.moQuery);
            dbField.SetMode(this, edit ? dbField.moEdit : dbField.moNormal);
        }
        */

        private void TriggerChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        //------
        private Control dbControl
        {
            get { return paParams; }
        }

        private bool FillData(bool edit)
        {
            DataRow dr = UbezpieczeniaParametry.GetData();
            //DataRow dr = db.getDataRow(db.conP, dsParametry.SelectCommand);
            //dbField.FillData(this, dr, 0, 0, 0, dbField.moQuery);   // wymuszenie podglądu
            int mode = edit ? dbField.moEdit : dbField.moNormal;
            dbField.FillData(dbControl, dr, 0, 0, 0, mode);    // tryb normalny, tj. co ma być w podglądzie jest w podglądzie, co edycji to w edycji
            dbField.FillData(paButtons, null, 0, 0, 0, mode);    
            cntReportSchedulerEdit.FillData(mode);


            //plomba
            //string skl = db.getValue(dr, "Skladnik");
            //Tools.SelectItem(Skladnik.DdlValue, skl);


            return true;
        }

        private bool Validate()
        {
            return dbField.Validate(dbControl) 
                && cntReportSchedulerEdit.Validate();
        }

        private bool Update()
        {
            return dbField.dbUpdate(db.conP, dbControl, UbezpieczeniaParametry.poTableParametry, null, null, null)
                && cntReportSchedulerEdit.Update();
        }


        /*
        protected void btEdit_Click(object sender, EventArgs e)
        {
            FillData();
            SetEdit(true);
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Validate())
                if (Update())
                {
                    SetEdit(false);
                    PortalUstawienia.GetData();
                    TriggerChanged();
                }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            FillData();   // przywracam poprzednie wartości, ustawia query
            SetEdit(false);
        }
        */ 
        //-----------------
        protected void wbtEdit_Click(object sender, EventArgs e)
        {
            FillData(true);
        }

        protected void wbtSave_Click(object sender, EventArgs e)
        {
            if (Validate())
                if (Update())
                {
                    FillData(false);
                    UbezpieczeniaParametry.GetData();
                    TriggerChanged();
                }
        }

        protected void wbtCancel_Click(object sender, EventArgs e)
        {
            FillData(false);   // przywracam poprzednie wartości, ustawia query
        }
    }
}