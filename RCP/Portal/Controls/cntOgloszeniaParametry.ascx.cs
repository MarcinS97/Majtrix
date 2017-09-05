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
    public class PortalUstawienia       // straszna klasa statyczna ...
    {
        public const string poOgloszeniaParametry = "poOgloszeniaParametry";

        public static int FmaxOgloszenia = -1;
        public static int FmaxDni = -1;
        public static int FmaxFileSizeMB = -1;
        public static string FFileRegulamin = null;

        const int defMaxOgloszenia = 7;
        const int defMaxDni        = 30;
        const int defMaxFileSizeMB = 2;

        public static DataRow GetData()
        {
            SqlConnection c = db.Connect(db.PORTAL);   // tak ma być bo się wykonuje bez MasterPage
            DataRow dr = db.getDataRow(c, String.Format("select top 1 * from {0}", poOgloszeniaParametry));
            if (dr != null)
            {
                FmaxOgloszenia = db.getInt(dr, "MaxOgloszenia", -1);
                FmaxDni        = db.getInt(dr, "MaxDni", -1);
                FmaxFileSizeMB = db.getInt(dr, "MaxFileSizeMB", -1);
                FFileRegulamin = db.getValue(dr, "Regulamin");
            }
            else
            {
                FmaxOgloszenia = defMaxOgloszenia;
                FmaxDni        = defMaxDni;
                FmaxFileSizeMB = defMaxFileSizeMB;
                FFileRegulamin = null;
                db.insert(c, poOgloszeniaParametry, 0, "MaxOgloszenia,MaxDni,MaxFileSizeMB,Regulamin", FmaxOgloszenia, FmaxDni, FmaxFileSizeMB, db.NULL);
            }
            db.Disconnect(c);
            return dr;
        }

        public static int maxOgloszenia
        {
            get
            {
                if (FmaxOgloszenia == -1) GetData();
                return FmaxOgloszenia;
            }
        }

        public static int maxDni
        {
            get
            {
                if (FmaxDni == -1) GetData();
                return FmaxDni;
            }
        }

        public static int maxFileSizeMB
        {
            get
            {
                if (FmaxFileSizeMB == -1) GetData();
                return FmaxFileSizeMB;
            }
        }

        public static int maxFileSizeB
        {
            get { return maxFileSizeMB * 1024; }
        }

        public static string FileRegulamin
        {
            set { FFileRegulamin = value; }
            get { return FFileRegulamin; }
        }
    }

    public partial class cntOgloszeniaParametry : System.Web.UI.UserControl
    {
        public event EventHandler Changed;
        public event EventHandler Regulamin;

        public static PortalUstawienia Ustawienia;   // zmienna public static!!!

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillData(false);
                if (App.User.IsPortalAdmin)
                    Tools.EnableUpload();
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

        private void TriggerRegulamin()
        {
            if (Regulamin != null)
                Regulamin(this, EventArgs.Empty);
        }
        //------
        private bool FillData(bool edit)
        {
            DataRow dr = PortalUstawienia.GetData();
            //DataRow dr = db.getDataRow(db.conP, dsParametry.SelectCommand);
            //dbField.FillData(this, dr, 0, 0, 0, dbField.moQuery);   // wymuszenie podglądu
            dbField.FillData(this, dr, 0, 0, 0, edit ? dbField.moEdit : dbField.moNormal);    // tryb normalny, tj. co ma być w podglądzie jest w podglądzie, co edycji to w edycji
            return true;
        }

        private bool Validate()
        {
            return dbField.Validate(this);
        }

        private bool Update()
        {
            return dbField.dbUpdate(db.conP, this, PortalUstawienia.poOgloszeniaParametry, null, null, null);
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
                    PortalUstawienia.GetData();
                    TriggerChanged();
                }
        }

        protected void wbtCancel_Click(object sender, EventArgs e)
        {
            FillData(false);   // przywracam poprzednie wartości, ustawia query
        }

        private bool UploadRegulamin()
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;                
                string savePath = Server.MapPath(cntOgloszenia.ImagesPath); 
                FileUpload1.SaveAs(savePath + fileName);
                db.update(db.conP, PortalUstawienia.poOgloszeniaParametry, "1=1", "Regulamin", fileName);
                PortalUstawienia.FileRegulamin = fileName;
                return true;
            }
            else
            {
                Tools.ShowMessage("Brak pliku do załadowania.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
                return false;
            }
        }

        public string FileRegulamin
        {
            get { return PortalUstawienia.FileRegulamin; }
        }

        public string UrlRegulamin
        {
            get { return cntOgloszenieEdit.UrlRegulamin; }
        }

        protected void btRegulaminUpload_Click(object sender, EventArgs e)
        {
            if (UploadRegulamin())
                TriggerRegulamin();
        }
    }
}