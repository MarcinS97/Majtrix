using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;
using System.Data;
using System.Text;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntScorecard : System.Web.UI.UserControl
    {
        public event EventHandler ScAccepted;
        public event EventHandler TasksEmptySpr;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Prepare(String EmployeeId, String ScorecardTypeId, String Date, String OutsideJob, String Genre, String ObserverId, Boolean Accepted, String OwnerId)
        {
            this.EmployeeId = EmployeeId;
            this.ScorecardTypeId = ScorecardTypeId;
            this.Date = Date;
            this.OutsideJob = OutsideJob;
            this.Genre = Genre;
            // UWAGA ACHTUNG ATENZIONE
            this.ObserverId = OwnerId;
            this.Accepted = Accepted;
            this.TeamLeader = (Genre == Dictionaries.Spreadsheets.Team) ? GetTL(this.ObserverId) : (/*(this.ObserverId == EmployeeId) ? "1" : "0"*/GetTL(EmployeeId)); // "0";//GetTL();
            Prepare();
        }

        protected void OutsideClick(object sender, EventArgs e)
        {
            Save();
        }

        public void Prepare()
        {
            switch (Genre)
            {
                case Dictionaries.Spreadsheets.Individual: break;
                case Dictionaries.Spreadsheets.Team: if(OutsideJob == "0") EmployeeId = "-" + ScorecardTypeId; break;
                default: EmployeeId = "NULL"; break;
            }
            Header.Prepare(EmployeeId, ScorecardTypeId, Date, Genre, Accepted, ObserverId, TeamLeader);
            CosmicInfo.Prepare(OutsideJob, Genre, /*TeamLeader*/GetTL(EmployeeId));
            Tasks.Prepare(EmployeeId, ScorecardTypeId, Date, OutsideJob);

            DataView daysDv = GetDaysDataView();

            Values.Prepare(EmployeeId, ScorecardTypeId, Date, InEdit, OutsideJob, daysDv);
            Days.Prepare(EmployeeId, ScorecardTypeId, Date, InEdit, OutsideJob, Genre, ObserverId, /*TeamLeader*/GetTL(EmployeeId), daysDv);
            CoolStuff.Prepare(EmployeeId, ScorecardTypeId, Date, InEdit, OutsideJob, daysDv);
            CosmicInfo2.Prepare(InEdit);

            btnBackToMotherScorecard.Visible = OutsideJob == "1";

        }

        protected void BackToMotherScorecard(object sender, EventArgs e)
        {
            Save();
            Response.Redirect(ResolveUrl("~/Scorecards/Scorecards.aspx"));
        }

        protected void dsDays_Selected(object sender, EventArgs e)
        {

        }

        protected DataView GetDaysDataView()
        {
            DataView dv = (DataView)dsDays.Select(DataSourceSelectArguments.Empty);
            return dv;
        }

        public void Save()
        {
            Days.Save();    
            Values.Save();
            CoolStuff.Save();
            if(Genre == Dictionaries.Spreadsheets.Team) Header.Save();
            Prepare();
        }

        public void SaveAndCheckPGIO()
        {
            Save();             
            DataSet Set = db.Select.Set(dsCanAccept, ScorecardTypeId, EmployeeId, db.strParam(Date), ObserverId);
            StringBuilder sb = new StringBuilder();
            DataTable Table = Set.Tables[0];
            
            if (Table != null)
            {
                if (Table.Rows.Count > 0)
                {
                    //sb.Append("Nie można zaakceptować arkusza.\\n");
                    //String Message = "Nie można zaakceptować arkusza.\\nIstnieją nierozpisane godziny na pracy na inny arkusz w dniach:\\n ";
                    sb.Append("Istnieją nierozpisane godziny na pracy na inny arkusz w dniach:\\n");
                    foreach (DataRow Row in Table.Rows)
                    {
                        /*Message += String.*/
                        sb.AppendFormat("{0}\\n", Row["Data"]);
                    }
                    Tools.ShowMessage(sb.ToString());
                    //return;
                }
            }
        }

        protected void RowDeleted(object sender, EventArgs e)
        {
            Prepare();
        }

        public void Accept()
        {
            Tools.ShowConfirm("Czy na pewno chcesz zaakceptować arkusz i zablokować go do edycji?", btnAccept, null);
        }

        public void AcceptAll()
        {
            Tools.ShowConfirm("Czy na pewno chcesz zaakceptować wszystkie arkusze i zablokować je do edycji?", btnAcceptAll, null);
        }

        public void UnAccept()
        {
            Header.UnacceptConfirm2(); 
            if (ScAccepted != null) ScAccepted(null, EventArgs.Empty);
        }

        protected int AcceptScorecard(String ScorecardTypeId, String EmployeeId, String Date, String ObserverId)
        {
            //
            /*DataTable Table = db.Select.Table*/DataSet Set = db.Select.Set(dsCanAccept, ScorecardTypeId, EmployeeId, db.strParam(Date), ObserverId);
            StringBuilder sb = new StringBuilder();
            DataTable Table = Set.Tables[0];
            DataTable Table2 = Set.Tables[1];
            Boolean Showp = false;
            if (Table != null)
            {
                if (Table.Rows.Count > 0)
                {
                    if (!Showp)
                    {
                        sb.AppendFormat("Nie można zaakceptować arkusza {0}{1}.\\n", db.Select.Scalar("select Nazwa from scTypyArkuszy where Id = {0}", ScorecardTypeId), (EmployeeId[0] != '-') ? db.Select.Scalar("select ' - ' + Nazwisko + ' ' + Imie from Pracownicy where Id = {0}", EmployeeId) : "");
                        Showp = true;
                    }
                    //String Message = "Nie można zaakceptować arkusza.\\nIstnieją nierozpisane godziny na pracy na inny arkusz w dniach:\\n ";
                    sb.Append("Istnieją nierozpisane godziny na pracy na inny arkusz w dniach:\\n");
                    foreach (DataRow Row in Table.Rows)
                    {
                        /*Message += String.*/sb.AppendFormat("{0}\\n", Row["Data"]);
                    }
                    //Tools.ShowMessage(Message);
                    //return;
                }
            }
            if (Table2 != null)
            {
                if (Table2.Rows.Count > 0)
                {
                    if (!Showp)
                    {
                        sb.AppendFormat("Nie można zaakceptować arkusza {0}{1}.\\n", db.Select.Scalar("select Nazwa from scTypyArkuszy where Id = {0}", ScorecardTypeId), (EmployeeId[0] != '-') ? db.Select.Scalar("select ' - ' + Nazwisko + ' ' + Imie from Pracownicy where Id = {0}", EmployeeId) : "");
                        Showp = true;
                    }
                    sb.Append("Czas nieproduktywny jest większy od czasu pracy w dniach:\\n");
                    foreach (DataRow Row in Table2.Rows)
                    {
                        /*Message += String.*/sb.AppendFormat("{0}\\n", Row["Data"]);
                    }
                    //Tools.ShowMessage(Message);
                    //return;
                }
            }
            if (Showp)
            {
                Log.Error(1337, sb.ToString(), GetScInfo());
                Tools.ShowMessage(sb.ToString());
                return 1;
            }
            //

            String Premia = RazemPremia;
            //String PremiaSql = db.Select.Scalar(dsRazemPremia, ScorecardTypeId, EmployeeId, db.strParam(Date), 0, ObserverId);
            DataRow PremData = db.Select.Row(dsRazemPremia, ScorecardTypeId, EmployeeId, db.strParam(Date), TeamLeader, ObserverId, GetTL(EmployeeId));
            String PremiaSql = PremData["Premia"].ToString();
            String GodzProd = PremData["GodzProd"].ToString();
            String CzasPracy = PremData["CzasPracy"].ToString();
            String IloscSztuk = PremData["IloscSztuk"].ToString();
            String IloscBledow = PremData["IloscBledow"].ToString();
            String KlinkeAbsencjaKorekta = PremData["AbsencjaKorekta"].ToString();
            if (PremiaSql != null) PremiaSql = PremiaSql.Replace(',', '.');
            if (GodzProd != null) GodzProd = GodzProd.Replace(',', '.');
            if (CzasPracy != null) CzasPracy = CzasPracy.Replace(',', '.');
            if (Premia != null) Premia = Premia.Replace(',', '.');
            if (KlinkeAbsencjaKorekta != null) KlinkeAbsencjaKorekta = KlinkeAbsencjaKorekta.Replace(',', '.');
            try
            {
                if ((int)Double.Parse(Premia) != (int)Double.Parse(PremiaSql))
                {
                    Log.Info(1337, "Wartości premii przeliczone w JS i SQL nie zgadzają się!", String.Format("JS: {0}, SQL: {1}", Premia, PremiaSql));
                }
            }
            catch
            {
                Log.Info(1337, "Nie udało się porównać wartości premii!", String.Format("JS: {0}, SQL: {1}", Premia, PremiaSql));
            }
            Log.Error(1337, "Zaakceptowano scorecard", GetScInfo());
            db.Execute(dsRequest, ScorecardTypeId, db.strParam(Date), EmployeeId, ObserverId, PremiaSql, TeamLeader, GodzProd, CzasPracy, IloscSztuk, IloscBledow, GetTL(EmployeeId), KlinkeAbsencjaKorekta);
            if (ScAccepted != null) ScAccepted(null, EventArgs.Empty);
            return 0;
        }

        protected void AcceptAll(object sender, EventArgs e)
        {
            Save();
            DataTable Goyim = db.Select.Table(@"
declare @ObserverId int = {0}
declare @Date datetime = '{1}'
declare @IdCommodity int = {2}

select pr.Id/*, p.IdCommodity, CONVERT(varchar, p.IdCommodity) + ';' + CONVERT(varchar, pr.Id) + ';' + CONVERT(varchar, k.Id) as TeamEmployeeId
, (pr.Nazwisko + ' ' + pr.Imie + isnull(' (' + pr.KadryId + ')', ''))  as Name
, case when k.Id != @ObserverId then 1 else 0 end as Sort
, ta.Nazwa COLLATE SQL_Polish_CP1250_CI_AS + case when k.Id != @ObserverId then (', TL: ' + k.Nazwisko + ' ' + k.Imie) else '' end as Description
, case when k.Id = @ObserverId then 1 else 0 end as TL*/
from dbo.fn_GetTreeOkres(@ObserverId, dbo.bom(@Date), dbo.eom(@Date), @Date) p
left join scTypyArkuszy ta on p.IdCommodity = ta.Id
left join Pracownicy pr on p.IdPracownika = pr.Id
left join scSlowniki s on ta.Rodzaj = s.Id
left join Pracownicy k on k.Id = p.IdKierownika
where s.Nazwa2 = 'ARKI' and p.IdCommodity = @IdCommodity and k.Id = @ObserverId
--order by TL desc, ta.Nazwa, Sort, pr.Nazwisko, pr.Imie
", ObserverId, Date, ScorecardTypeId);
            foreach(DataRow Goy in Goyim.Rows)
            {
                String CurrentGoy = Goy[0].ToString();
                if (AcceptScorecard(ScorecardTypeId, CurrentGoy, Date, ObserverId) != 0) break;
            }
        }

        protected void Accept(object sender, EventArgs e)
        {
            Save();
            AcceptScorecard(ScorecardTypeId, EmployeeId, Date, ObserverId);
        }

        protected String GetTL(String Who)
        {
            try
            {
                if (!String.IsNullOrEmpty(Who)) return db.Select.Scalar(dsTeamLeader, db.strParam(Date), Who/*EmployeeId*//*ObserverId <- actual*/, ScorecardTypeId);
                else return "0";
            }
            catch 
            {
                return "0";
            }
        }

        public String GetScInfo()
        {
            return String.Format("ScorecardTypeId: {0}, Date: {1}, EmployeeId: {2}, ObserverId: {3}", ScorecardTypeId, Date, EmployeeId, ObserverId);
        }

        protected void EmptyTasks(object sender, EventArgs e)
        {
            if (TasksEmptySpr != null) TasksEmptySpr(null, EventArgs.Empty);   
        }

        protected void UnAccepted(object sender, EventArgs e)
        {
            if (ScAccepted != null) ScAccepted(null, EventArgs.Empty);
        }

        public String RazemPremia
        {
            get { return hidRazemPremia.Value; }
        }

        public Boolean InEdit
        {
            get { return Tools.GetViewStateBool(ViewState["vInEdit"], false); }
            set { ViewState["vInEdit"] = value; }
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

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }

        public String Date
        {
            get { return hidDate.Value; }
            set { hidDate.Value = value; }
        }

        public String OutsideJob
        {
            get { return hidOutsideJob.Value; }
            set { hidOutsideJob.Value = value; }
        }

        public String Genre
        {
            get { return ViewState["vGenre"] as String; }
            set { ViewState["vGenre"] = value; }
        }

        public Boolean Accepted
        {
            get { return (Boolean)ViewState["vAccepted"]; }
            set { ViewState["vAccepted"] = value; }
        }

        public Boolean HeaderVisible
        {
            get { return Header.Visible; }
            set { Header.Visible = value; }
        }

        public String TeamLeader
        {
            get { return hidTeamLeader.Value; }
            set { hidTeamLeader.Value = value; }
        }
    }
}