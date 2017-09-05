using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using HRRcp.Controls.Przypisania;

namespace HRRcp.Controls.PodzialLudzi
{
    public partial class cntPodzialLudziMies : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //hidStatusPar.ClientID

            cntSplityWsp1.Mode = Editable ? cntSplityWsp.moEditable : cntSplityWsp.moReadOnly;
            if (!IsPostBack)
            {
            }
            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
            cntMies.SQL1 = adm ? "1" : "0";

            btChangeStatus.Visible = adm;
            btImport.Visible = adm;
        }

        //-----------------------
        private int OpenOkresId
        {
            set { ViewState["okid"] = value; }
            get { return Tools.GetInt(ViewState["okid"], -1); }
        }

        private bool ChangeStatusPL(int okresid, int statusPL)
        {
            if (statusPL >= cntPodzialLudzi.stNone && statusPL <= cntPodzialLudzi.stClosed)
            {
                bool ok = true;
                //----- tworzę splity grup na nowy okres -----
                DataRow dr = db.getDataRow(String.Format("select * from OkresyRozl where Id = {0}", okresid));
                int currStatus = db.getInt(dr, "StatusPL", -1);
                if (currStatus == cntPodzialLudzi.stNone && statusPL == cntPodzialLudzi.stOpen)
                {
                    DateTime od = (DateTime)db.getDateTime(dr, "DataOd");
                    ok = db.execSQL(SQL.OpenSplity(Tools.DateToStrDb(od)));
                    if (ok)
                    {
                        OpenOkresId = okresid;
                        Tools.ShowConfirm("Podział Ludzi został otwarty.\nCzy wykonać import splitów?", btImport);
                    }
                }
                //--------------
                if (ok)
                    ok = db.update("OkresyRozl", 1, "StatusPL", "Id={0}", okresid, statusPL);
                return ok;

                //DataRow dr = db.getDataRow("select * from OkresyRozl where Id = " + okresid);
                //if (dr != null)
                //{
                //    int st = db.getInt(dr, "StatusPL", cntPodzialLudzi.stNone);

                //}
            }
            return false;
        }

        protected void cntMies_OnCommand(object sender, EventArgs e)
        {
            switch (cntMies.CommandName)
            {
                case "status":
                    string[] par = Tools.GetLineParams(cntMies.CommandParameter);
                    if (par.Length == 3)
                    {
                        int oid = Tools.StrToInt(par[1], -1);
                        int ost = Tools.StrToInt(par[2], -1);
                        if (ChangeStatusPL(oid, ost))
                            cntMies.DataBind();
                    }
                    break;
                case "split":
                    par = Tools.GetLineParams(cntMies.CommandParameter);
                    string dOd = par[1];
                    string status = par[2];
                    string gid = par[3];
                    const string sql = @"
select CC.cc + ' - ' + CC.Nazwa as CNazwa, S.Id from Splity S 
left join CC on CC.GrSplitu = S.GrSplitu
where S.GrSplitu = {0} 
and S.DataOd = '{1}'";
                    DataRow dr = db.getDataRow(String.Format(sql, gid, dOd));

                    bool edSG = App.User.HasRight(AppUser.rPodzialLudziEditSGrupy);
                    bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
                    bool editable = (adm || edSG) && Tools.StrToInt(status, Okres.stClosed) == Okres.stOpen;
                    if (dr == null)
                        if (editable)
                        {
                            // na podstawie poprzedniego okresu - jak nie będzie to ok, bo jak będą pootwierane miesiące wprzod zeby nie zamknęło bieżącego
                            db.execSQL(String.Format(@"
declare @grSplitu int
declare @nazwa nvarchar(500)
declare @od datetime 
set @grSplitu = {0}
set @od = '{1}'

declare @sid int, @nsid int
--select top 1 @sid = Id, @nazwa = Nazwa from Splity where GrSplitu = @grSplitu and DataOd = DATEADD(M, -1, @od) and DataDo is null order by DataOd desc
select @sid = Id, @nazwa = Nazwa from Splity where GrSplitu = @grSplitu and DataOd = DATEADD(M, -1, @od) order by DataOd desc

if @sid is not null begin
	update Splity set DataDo = DATEADD(D, -1, @od) where Id = @sid
	insert into Splity values (@grSplitu, @nazwa, @od, null, 0)
	select @nsid = @@IDENTITY
	insert into SplityWsp 
		select @nsid, IdCC, Wsp from SplityWsp where IdSplitu = @sid	
end
                        ", gid, dOd));
                            dr = db.getDataRow(String.Format(sql, gid, dOd));
                        }
                        else   // nie ma w wybrany miesiacu - biorę do wyświetlenia split z ostatniego miesiąca
                        {
                            string dOd2 = db.getScalar(String.Format("select top 1 DataOd from Splity where GrSplitu = {0} and DataOd < '{1}' and DataDo is null order by DataOd desc", gid, dOd));
                            if (!String.IsNullOrEmpty(dOd2))
                                dr = db.getDataRow(String.Format(sql, gid, dOd));
                        }

                    if (dr != null)
                    {
                        string splitId = db.getValue(dr, "Id");
                        string sName = db.getValue(dr, "CNazwa");

                        Editable = editable;
                        cntSplityWsp1.Prepare(splitId, editable);
                        cntSplityWsp1.DataBind();
                        btSave.Visible = editable;

                        //Tools.ShowDialog(this, "divZoom", 600, btClose, String.Format("{0} - split pracownika: {1}", dOd.Substring(0, 7), AppUser.GetNazwiskoImieNREW(pracId)));
                        Tools.ShowDialog(paContainer, "divZoom", 600, btClose, String.Format("{0} - Split: {1}", dOd.Substring(0, 7), sName));
                    }
                    else
                        Tools.ShowMessage("Brak splitu za wybrany okres.");
                    break;
            }
        }

        protected void btChangeStatus_Click(object sender, EventArgs e)
        {

        }
        //-----------------------
        protected void btExcel_Click(object sender, EventArgs e)
        {
            //string filename = "PodzialLudzi.csv";
            //App_Code.Report.ExportExcel(hidReport.Value, filename, null);   // >>>>>> musi jak redirect do nowej strony bo się nie ściąga
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (cntSplityWsp1._Validate())
            {
                if (cntSplityWsp1.Update())
                {
                    Tools.CloseDialog("divZoom");
                    //gvPodzial.DataBind();
                }
                else
                    Tools.ShowError("Wystąpił błąd podczas zapisu współczynników.");
            }
            else
                Tools.ShowError("Suma współczynników splitu jest różna od 1.");
        }

        protected void btClose_Click(object sender, EventArgs e)
        {
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            DataRow dr = db.getDataRow(String.Format("select * from OkresyRozl where Id = {0}", OpenOkresId));
            if (dr != null)
            {
                DateTime dod = (DateTime)db.getDateTime(dr, "DataOd");
                DateTime ddo = (DateTime)db.getDateTime(dr, "DataDo");
                DateTime naDzien = DateTime.Today;
                if (naDzien < dod)
                    naDzien = dod;
                else if (naDzien > ddo)
                    naDzien = ddo;
                int err = HRRcp.PodzialLudzi.ImportOnOpen(3, Tools.DateToStr(dod), Tools.DateToStr(ddo), naDzien);
                if (err == 0)
                    cntMies.GridDataBind();
            }
            else
                Tools.ShowMessage("Wystąpił błąd podczas importu.");
        }
        //---------------------
        public bool Editable
        {
            set { ViewState["edmode"] = value; }
            get { return Tools.GetBool(ViewState["edmode"], false); }
        }

    }
}