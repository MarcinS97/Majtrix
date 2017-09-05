using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntHeader : System.Web.UI.UserControl
    {
        public event EventHandler UnAccept;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String EmployeeId, String ScorecardTypeId, String Date, String Genre, Boolean Accepted, String ObserverId, String TeamLeader)
        {
            this.EmployeeId = EmployeeId;
            this.ScorecardTypeId = ScorecardTypeId;
            this.Date = Date;
            this.ObserverId = ObserverId;
            this.TeamLeader = TeamLeader;
            this.Genre = Genre;
            scAccepted.Visible = Accepted;
            scAccpeptedAdmin.Visible = App.User.IsScAdmin && Accepted && App.User.IsScWnRej;

            DataSet ds = db.getDataSet(String.Format(dsHeader.SelectCommand, ScorecardTypeId, EmployeeId, Date, ObserverId, TeamLeader, GetTL(EmployeeId)));

            hidData.Value = GetValue(ds, 3, "Table1");
            hidData2.Value = GetValue(ds, 3, "Table2");
            hidData3.Value = GetValue(ds, 3, "Table3");
            hidPrInnyArkusz.Value = GetValue(ds, 3, "PrInnyArkusz");

            hidAbs.Value = GetValue(ds, 4, "GodzNieob");

            tdPremiaProd.Attributes["data-value"] = GetValue(ds, 9, "Nominal");
            
            

            switch (Genre){
                case Dictionaries.Spreadsheets.Team:
                    trDisposition.Visible = false;
                    info.Visible = false;
                    infoGroup.Visible = true;

                    if (Accepted)
                    {

                        lblTeamSize.Text = GetValue(ds, 7, "IloscPracownikow");
                        lblTeamSize.Visible = true;
                        tbTeamSize.Visible = false;
                        RequiredFieldValidator3.Visible = false;
                    }
                    else
                    {
                        tbTeamSize.Text = GetValue(ds, 7, "IloscPracownikow");
                        tbTeamSize.Visible = true;
                        lblTeamSize.Visible = false;
                        RequiredFieldValidator3.Visible = true;
                    }
                    //lblTeamSize.Text = GetValue(ds, 5, "HeadCount");
                    lblQCGroup.Text = GetValue(ds, 2, "QC");//ds.Tables[2].Rows[0]["QC"].ToString();
                    lblTeamLeaderGroup.Text = GetValue(ds, 1, "TeamLeader");//ds.Tables[1].Rows[0]["TeamLeader"].ToString();
                    break;
                case Dictionaries.Spreadsheets.Individual:
                    String TLS = (Convert.ToInt32(EmployeeId) < 0) ? TeamLeader : (GetTL(EmployeeId) == "1" ? "1" : TeamLeader);
                    String Cosmos = GetValue(ds, 8, "Cosmos");

                    trDisposition.Visible = (TLS == "0") && (Cosmos == "1");
                    info.Visible = true;
                    infoGroup.Visible = false;

                    lblNrEwid.Text = GetValue(ds, 0, "KadryId"); //ds.Tables[0].Rows[0]["KadryId"].ToString();
                    lblName.Text = GetValue(ds, 0, "Name");
                    lblTeamLeader.Text = GetValue(ds, 1, "TeamLeader");//ds.Tables[1].Rows[0]["TeamLeader"].ToString();
                    lblHireDate.Text = GetValue(ds, 0, "DataZatr");//ds.Tables[0].Rows[0]["DataZatr"].ToString();
                    lblQC.Text = GetValue(ds, 2, "QC");//ds.Tables[2].Rows[0]["QC"].ToString();

                    break;
            }

            if(ds.Tables.Count > 6)
            {
                if (ds.Tables[6].Rows.Count > 0)
                {
                    rpSums.Visible = true;
                    rpSums.DataSource = ds.Tables[6];
                    rpSums.DataBind();
                }
                else
                {
                    rpSums.Visible = false;
                }
             }
        }

        public void Save()
        {
            if (tbTeamSize != null)
                if(tbTeamSize.Visible) 
                    db.Execute(dsSave, ScorecardTypeId, db.strParam(Date), ObserverId, tbTeamSize.Text);
        }

        protected void Prepare()
        {

        }


        protected void UnacceptConfirm(object sender, EventArgs e)
        {
            UnacceptConfirm();
        }

        public void UnacceptConfirm()
        {
            Tools.ShowConfirm("Czy na pewno chcesz cofnąć akceptację? Wiążę się to z usunięciem pracownika / pracowników z wniosku premiowego!", btnUnnacept, null);
        }

        public void UnacceptConfirm2()
        {
            Tools.ShowConfirm("Czy na pewno chcesz cofnąć akceptację? Wiążę się to z usunięciem pracownika / pracowników z wniosku premiowego!", btnUnaccept2, null);
        }

        protected void Unaccept(object sender, EventArgs e)
        {
            Unaccept();
        }

        protected void Unaccept2(object sender, EventArgs e)
        {
            Unaccept(EmployeeId);
        }

        public void Unaccept()
        {
            switch (Genre)
            {
                case Dictionaries.Spreadsheets.Individual: db.Execute(dsUnAccept, ScorecardTypeId, db.strParam(Date), ObserverId, EmployeeId); break;
                case Dictionaries.Spreadsheets.Team: db.Execute(dsUnAcceptTeam, ScorecardTypeId, db.strParam(Date), ObserverId); break;
            }
            if (UnAccept != null) UnAccept(null, EventArgs.Empty);
        }

        public void Unaccept(String ObserverId)
        {
            switch (Genre)
            {
                case Dictionaries.Spreadsheets.Individual: db.Execute(dsUnAccept, ScorecardTypeId, db.strParam(Date), ObserverId, EmployeeId); break;
                case Dictionaries.Spreadsheets.Team: db.Execute(dsUnAcceptTeam, ScorecardTypeId, db.strParam(Date), ObserverId); break;
            }
            if (UnAccept != null) UnAccept(null, EventArgs.Empty);
        }

        private String GetValue(DataSet ds, int table, string property)
        {
            string value = string.Empty;
            if (table <= ds.Tables.Count)
            {
                try
                {
                    return ds.Tables[table].Rows[0][property].ToString();
                }
                catch { }
            }
            return value;
        }

        protected String GetTL(String Who)
        {
            try
            {
                return db.Select.Scalar(dsTeamLeader, db.strParam(Date), Who/*EmployeeId*//*ObserverId <- actual*/, ScorecardTypeId);
            }
            catch
            {
                return "0";
            }
        }

        public String ScorecardTypeId
        {
            get { return hidScorecardTypeId.Value; }
            set { hidScorecardTypeId.Value = value; }
        }

        public String EmployeeId
        {
            get { return hidEmployeeId.Value; }
            set { hidEmployeeId.Value = value; }
        }

        public String Date
        {
            get { return hidDate.Value; }
            set { hidDate.Value = value; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }

        public String TeamLeader
        {
            get { return hidTeamLeader.Value; }
            set { hidTeamLeader.Value = value; }
        }

        public String Genre
        {
            get { return ViewState["vGenre"] as String; }
            set { ViewState["vGenre"] = value; }
        }
    }
}