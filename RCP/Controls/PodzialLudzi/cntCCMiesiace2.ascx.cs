using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.App_Code;
using System.Globalization;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace HRRcp.Controls
{
    public partial class cntCCMiesiace2 : System.Web.UI.UserControl
    {
        public AppUser currentUser
        {
            get { return App.User; }
        }

        public bool canEdit { get; set; }


        protected override void OnInit(EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, 0);
            base.OnInit(e);
        }

        void RefreshRokMiesiac()
        {
            //string debug = Tools.getControlsTree(ListView1);
            HFMiesiac.Value = string.Format("{0}-{1}-1", SelectRokMiesiac1.Rok, SelectRokMiesiac1.Miesiac);
            DataBind();

            Control[] cnts = new Control[] {
                ListView1.FindControl("thCnts"),
                ListView1.FindControl("EDTInsert"),
                ListView1.FindControl("ctrl0").FindControl("EDTInsert")
            };
            foreach (var cnt in cnts)
                if (cnt != null)
                    cnt.Visible = canEdit;
        }

        protected void SelectRokMiesiac1_ValueChanged(object sender, EventArgs e)
        {
            RefreshRokMiesiac();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                RefreshRokMiesiac();
        }

        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            TextBox tbLimit = e.Item.FindControl("ETBLimit") as TextBox;
            Double? f = isValidLimit(tbLimit);
            if (!f.HasValue)
            {
                e.Cancel = true;
                return;
            }
            e.Values["Limit"] = f;
            DropDownList DDL = e.Item.FindControl("DDL1") as DropDownList;
            if (DDL == null)
            {
                e.Cancel = true;
                return;
            }
            if(string.IsNullOrEmpty(DDL.SelectedValue))
            {
                Tools.ShowError("Nie wybrano CC");
                e.Cancel = true;
                return;
            }
            SqlDataSource1.InsertParameters["ccId"].DefaultValue = DDL.SelectedValue;
        }

        protected void SelectRokMiesiac1_BackAll(object sender, EventArgs e)
        {
            cntSelectRokMiesiac SRM = (sender as cntSelectRokMiesiac);

            DateTime? dt = db.getScalar<DateTime>("SELECT TOP 1 miesiac FROM ccLimity where isLast = 1 AND Limit IS NOT NULL ORDER BY miesiac");

            if (dt.HasValue)
            {
                SRM.Rok = dt.Value.Year;
                SRM.Miesiac = dt.Value.Month;
            }
            else
            {
                SRM.SelectNow();
            }
        }

        protected void SelectRokMiesiac1_NextAll(object sender, EventArgs e)
        {
            cntSelectRokMiesiac SRM = (sender as cntSelectRokMiesiac);
            SRM.SelectNow();
        }

        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            int? z = db.getScalar<int>(string.Format(
                "SELECT TOP 1 Status FROM OkresyRozl where '{0}-{1}-1' BETWEEN DataOd AND DataDo AND Status = 1", SelectRokMiesiac1.Rok, SelectRokMiesiac1.Miesiac));
            canEdit = currentUser.IsAdmin || (!z.HasValue && currentUser.HasRight(AppUser.rEditCCLim));
        }

        protected Double? isValidLimit(TextBox tb)
        {
            if (tb != null)
            {
                try
                {
                    return Double.Parse(tb.Text.Replace(',', '.'), CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    Tools.ShowError("Nieprawidłowy format limitu.");
                    return null;
                }
            }
            else
                return null;
        }

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            TextBox tbLimit = ListView1.Items[e.ItemIndex].FindControl("ETBLimit2") as TextBox;
            Double? f = isValidLimit(tbLimit);
            if (!f.HasValue)
            {
                e.Cancel = true;
                return;
            }
            e.NewValues["Limit"] = f;
        }

        protected void ListView1_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {

        }
    }
}

//Stare Query

//WITH A AS 
//    (SELECT id, ccId, ROW_NUMBER() OVER(PARTITION BY ccId ORDER BY miesiac DESC) as rn
//        FROM ccLimity
//        where (miesiac = @miesiac OR miesiac = DATEADD(mm,-1,@miesiac)) AND isLast = 1)
//SELECT L.id, L.id2, L.ccId, CC.cc, CC.Nazwa, L.miesiac, 
//    (case miesiac when @miesiac then L.Limit else NULL end) as Limit
//    FROM A 
//JOIN ccLimity as L on A.id = L.id 
//LEFT JOIN CC on L.ccId = CC.id
//    WHERE (@showAll = 1 OR 
//    A.ccId IN(SELECT idCC as ccId FROM ccPrawa where UserId = @userId AND idCC != 0)) AND
//    rn = 1 AND NOT (miesiac = @miesiac AND Limit IS NULL) AND Limit IS NOT NULL
