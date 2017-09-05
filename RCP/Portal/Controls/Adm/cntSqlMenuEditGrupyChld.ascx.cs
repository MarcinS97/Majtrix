using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Adm
{
    public partial class cntSqlMenuEditGrupyChld : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show()
        {
            cntSqlMenuEditGrupy.Show(null, null);
        }

        protected void cntSqlMenuEditGrupy_ShowChildren(cntSqlMenuEditGrupy sender, string grupa, string id, cntSqlMenuEditGrupy.CMode cmode)
        {
            cntSqlMenuEditGrupy1.Show(grupa, id);
        }

        protected void cntSqlMenuEditGrupy1_ShowChildren(cntSqlMenuEditGrupy sender, string grupa, string id, cntSqlMenuEditGrupy.CMode cmode)
        {
            cntSqlMenuEditGrupy2.Show(grupa, id);
        }

        protected void cntSqlMenuEditGrupy2_ShowChildren(cntSqlMenuEditGrupy sender, string grupa, string id, cntSqlMenuEditGrupy.CMode cmode)
        {
            Tools.ShowMessage("Zbyt duży poziom zagłębienia.");   // >>> dodać kolejną kontrolkę
            //cntSqlMenuEditGrupy.Show(grupa, id);                // <<<< do zmiany !!! i TESTÓW bo to raczej tak nie powinno być zrobione
        }

        protected void cntSqlMenuEditGrupy_Edit(cntSqlMenuEditGrupy sender, string grupa, string id, cntSqlMenuEditGrupy.CMode cmode)
        {
            if      (sender == cntSqlMenuEditGrupy)  Level = 0;
            else if (sender == cntSqlMenuEditGrupy1) Level = 1;
            else if (sender == cntSqlMenuEditGrupy2) Level = 2;

            if (cmode == Adm.cntSqlMenuEditGrupy.CMode.New)
                cntSqlMenuEdit.Show(null, id, grupa);   // jak childa dodajemy, to w id jest parent
            else
                cntSqlMenuEdit.Show(id, null, grupa);
        }

        protected void cntSqlMenuEdit_Save(object sender, EventArgs e)
        {
            int level = Level;
            if (level >= 2) cntSqlMenuEditGrupy2.DataBind();
            if (level >= 1) cntSqlMenuEditGrupy1.DataBind();
            if (level >= 0) cntSqlMenuEditGrupy.DataBind();      
        }

        //-----------------
        public cntSqlMenuEditGrupy Menu
        {
            get { return cntSqlMenuEditGrupy; }
        }

        public cntSqlMenuEditGrupy Menu1
        {
            get { return cntSqlMenuEditGrupy1; }
        }

        public cntSqlMenuEditGrupy Menu2
        {
            get { return cntSqlMenuEditGrupy2; }
        }

        public int Level
        {
            set { ViewState["level"] = value; }
            get { return Tools.GetInt(ViewState["level"], 0); }
        }
    }
}