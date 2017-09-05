using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls;
using System.Globalization;
using System.Data;

namespace HRRcp.Scorecards.Controls.Requests
{
    public partial class cntRequestHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String RequestId, String ObserverId, Boolean Editable, Boolean Custom, Boolean TL, Boolean EditableNotHeader)
        {
            this.RequestId = RequestId;
            this.ObserverId = ObserverId;
            this.Editable = Editable;
            this.Custom = Custom;
            this.TL = TL;
            this.EditableNotHeader = EditableNotHeader;
            lvHeader.DataBind();
            if (lvHeader.Items.Count > 0) Tools.SelectItem(lvHeader.Items[0], "ddlTeamLeader", App.User.Id);
        }

        protected void lvHeader_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv = null;
            int li = Tools.GetListItemMode(e, lvHeader, out drv);
            if (drv != null)
            {
                Tools.SelectItem(e.Item, "ddlData", drv["DataId"]);
                Tools.SelectItem(e.Item, "ddlDataWyplaty", drv["DataWyplatyId"]);
                Tools.SelectItem(e.Item, "ddlQuatro0", drv["Quatro0"]);
                Tools.SelectItem(e.Item, "ddlQuatro1", drv["Quatro1"]);
            }
        }

        public Boolean Save()
        {
            if (lvHeader.Items.Count == 1)
            {
                String Name = Tools.GetText(lvHeader.Items[0], "tbName");
                String DataWyplaty = Tools.GetDdlSelectedValue(lvHeader.Items[0], "ddlDataWyplaty");
                String Data = Tools.GetDdlSelectedValue(lvHeader.Items[0], "ddlData");
                String Quatro0 = Tools.GetLineParam(Tools.GetDdlSelectedValue(lvHeader.Items[0], "ddlQuatro0"), 1);
                String Quatro1 = Tools.GetLineParam(Tools.GetDdlSelectedValue(lvHeader.Items[0], "ddlQuatro1"), 1);
                String PremiaZad = Tools.GetText(lvHeader.Items[0], "tbPremiaZad");
                String PremiaUzn = Tools.GetText(lvHeader.Items[0], "tbPremiaUznTL");
                String Uwagi = Tools.GetText(lvHeader.Items[0], "tbUwagiWniosek");

                Double PUzn = 0;
                if(!String.IsNullOrEmpty(PremiaUzn))
                    PUzn = Double.Parse(PremiaUzn.Replace(',', '.'), CultureInfo.InvariantCulture);
                
                Double MaxPulaZad = GetPulaZad();
                Double PZad = 0;
                try
                {
                    PZad = Double.Parse(PremiaZad.Replace(',', '.'), CultureInfo.InvariantCulture);
                }
                catch { }

                if (PZad > MaxPulaZad)
                {
                    Tools.ShowConfirm("Pula zadaniowa przekracza dopuszczalną wartość!", btnRecalculate, null);
                    return false;
                }

                String aoe = Tools.GetDdlSelectedValue(lvHeader.Items[0], "ddlTeamLeader");
                db.Execute(dsSave, db.strParam(Name), (Custom) ? ", DataWyplaty = " + db.strParam(DataWyplaty) : "", RequestId, db.nullParam(Quatro0), db.nullParam(Quatro1), db.nullStrParam(PremiaZad), (ObserverId == "0" && IsCustomEditable()) ? aoe : "IdPracownika", PUzn, db.nullStrParam(Uwagi), (Custom) ? ", Data = DATEADD(M, -1, " + db.strParam(DataWyplaty) + ")" : "");
            }
            return true;
        }

        public Boolean IsCustomEditable()
        {
            return Editable && Custom;
        }

        public Boolean IsCustomEditableNotHeader()
        {
            return EditableNotHeader && Custom;
        }

        public Boolean IsEditable()
        {
            return Editable;
        }

        public Boolean IsCustom()
        {
            return Custom;
        }

        public Boolean IsTL(String TL)
        {
            return TL == "1";
        }

        public Boolean IsAdmin()
        {
            return App.User.IsScAdmin;
        }

        public Boolean Custom
        {
            get { return Tools.GetViewStateBool(ViewState["vCustom"], false); }
            set { ViewState["vCustom"] = value; }
        }

        public Boolean TL
        {
            get { return Tools.GetViewStateBool(ViewState["vTL"], false); }
            set { ViewState["vTL"] = value; }
        }

        public String RequestId
        {
            get { return hidRequestId.Value; }
            set { hidRequestId.Value = value; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }

        public Boolean Editable
        {
            get { return (Boolean)ViewState["vEditable"]; }
            set { ViewState["vEditable"] = value; }
        }

        public Boolean EditableNotHeader
        {
            get { return Tools.GetViewStateBool(ViewState["vENH"], false); }
            set { ViewState["vENH"] = value; }
        }

        public Double GetPulaZad()
        {
            Double Pula = 0;
            HiddenField hidPula = lvHeader.Items[0].FindControl("hidPulaZad") as HiddenField;
            try
            {
                Pula = Double.Parse(hidPula.Value.Replace(',', '.'), CultureInfo.InvariantCulture);
            }
            catch { }
            return Pula;
        }

        public Double GetPulaPremii()
        {
            Double Pula = 0;
            HiddenField hidPula = lvHeader.Items[0].FindControl("hidPulaUznaniowa") as HiddenField;
            try
            {
                Pula = Double.Parse(hidPula.Value.Replace(',','.'), CultureInfo.InvariantCulture);
            }
            catch { }
            return Pula;
        }
    }
}