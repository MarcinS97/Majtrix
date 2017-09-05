using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Przypisania
{
    public partial class cntSplity : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvSplity, 0);
        }

        protected void ddlCC_DataBound(object sender, EventArgs e)
        {
            ddlCC_SelectedIndexChanged(sender, e);
        }

        protected void ddlCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCC = (DropDownList)sender;
            string gs = ddlCC.SelectedValue;
            DateEdit deOd = ddlCC.Parent.FindControl("deDataOd") as DateEdit;
            if (deOd != null)
            {
                if (!String.IsNullOrEmpty(gs))
                {
                    //DataRow dr = db.getDataRow(String.Format("select DATEADD(MM, 1, DataOd) as od from Splity where GrSplitu = {0} and DataDo is null", gs));
                    DataRow dr = db.getDataRow(String.Format("select top 1 DATEADD(MM, 1, DataOd) as od from Splity where GrSplitu = {0} order by DataDo desc", gs));
                    if (dr != null)
                        deOd.Date = db.getDateTime(dr, "od");
                    else
                        deOd.Date = DateTime.Today.bom();
                }
                else
                    deOd.Date = DateTime.Today.bom();
            }
        }

        DateTime okOd;      // biezacy okres od
        DateTime okPrevOd;  // poprzedni okres od        
        int okStatus = 0;

        protected void lvSplity_DataBinding(object sender, EventArgs e)
        {
            Okres ok = Okres.Current(db.con);
            okOd = ok.DateFrom;
            okStatus = ok.Status;
            ok.Prev();
            okPrevOd = ok.DateFrom;
        }

        protected void lvSplity_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            int lim = Tools.GetListItemMode(e, lvSplity, out drv);
            switch (lim)
            {
                case Tools.limSelect:
                    bool last = db.isNull(drv["DataDo"]);  // powinno byc inaczej sprawdzane, ale zadziała
                    Tools.SetControlVisible(e.Item, "cntSplityWsp1", last);
                    Tools.SetControlVisible(e.Item, "SelectButton", !last);
                    Tools.SetControlVisible(e.Item, "DeleteButton", last || App.User.IsAdmin);

                    DateTime dOd = (DateTime)drv["DataOd"];
                    bool conf = dOd < okOd ;                            // poprzednie okresy
                    bool edA = dOd >= okPrevOd || App.User.IsAdmin;     // 
                    Button btEdit = (Button)Tools.SetControlVisible(e.Item, "EditButton", edA);
                    if (btEdit != null && conf)
                        if (okStatus == Okres.stClosed)
                            Tools.MakeConfirmButton(btEdit, "Potwierdź edycję danych za okres, który jest już zamknięty.");
                        else
                            Tools.MakeConfirmButton(btEdit, "Potwierdź edycję danych za poprzedni okres.");
                    break;
                case Tools.limEdit:         // na razie tylko tak - problem jezeli ktos wejdzie po północy, to mu pozwoli !!! dodać sprawdzenie jeżeli btEdit ma confirm ustawione to ok, jezeli nie to warningi
                    /*
                    DateTime dOd = (DateTime)drv["DataOd"];
                    Okres ok1 = Okres.Current(db.con);
                    if (dOd < ok.DateFrom)
                        if (ok.Status == Okres.stClosed)
                            Tools.ShowWarning("Edytujesz dane za okres, który jest już zamknięty.");
                        else
                            Tools.ShowWarning("Edytujesz dane za poprzedni okres.");
                    
                     */ 
                     break;
            }
        }

        protected void lvSplity_DataBound(object sender, EventArgs e)
        {

        }

        protected void lvSplity_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            cntSplityWsp sw = lvSplity.EditItem.FindControl("cntSplityWsp1") as cntSplityWsp;
            if (sw != null)
                if (sw._Validate())
                    sw.Update();
                else
                {
                    e.Cancel = true;
                    Tools.ShowError("Suma współczynników splitu jest różna od 1.");
                }
        }

        protected void lvSplity_ItemEditing(object sender, ListViewEditEventArgs e)
        {
        }

        string delSplit = null;
        string delId = null;
 
        protected void lvSplity_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            delId = Tools.GetDataKey(lvSplity, e);
            delSplit = cntSplityWsp.GetSplitLog(cntSplityWsp.tySplityWsp, delId);   // te odwołują się do SplityWsp !!!
        }

        protected void lvSplity_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            Log.Info(Log.SPLITY, "SplityWsp.Delete", String.Format("SplitId: {1}\nDel: {0}", delSplit, delId));
        }


        /*
        protected void ddlCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCC = (DropDownList)sender;
            string gs = ddlCC.SelectedValue;
            Label lbOd = ddlCC.Parent.FindControl("lbDataOd") as Label;
            if (lbOd != null)
            {
                if (!String.IsNullOrEmpty(gs))
                {
                    //DataRow dr = db.getDataRow(String.Format("select DATEADD(MM, 1, DataOd) as od from Splity where GrSplitu = {0} and DataDo is null", gs));
                    DataRow dr = db.getDataRow(String.Format("select top 1 DATEADD(MM, 1, DataOd) as od from Splity where GrSplitu = {0} order by DataDo desc", gs));
                    if (dr != null)
                        lbOd.Text = Tools.DateToStr(db.getDateTime(dr, "od"));
                    else
                        lbOd.Text = Tools.DateToStr(DateTime.Today.bom());
                }
                else
                    lbOd.Text = Tools.DateToStr(DateTime.Today.bom());
            }
        }
         */
    }
}