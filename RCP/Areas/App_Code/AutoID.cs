using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HRRcp.App_Code
{
    public class AutoID
    {
        public static string AUTOID = ConfigurationManager.ConnectionStrings["AUTOID"].ConnectionString;

        const string LogInfo  = "AUTOID.ImportRCP";
        const string LogError = "AUTOID.ImportRCP - ERROR";

        const string tmpTerminale = "tmpAUTOID_Terminale";
        const string tmpZdarzenia = "tmpAUTOID_Zdarzenia";

#if KDR
        const string Terminale = "AUTOID_Terminale";
        const string Zdarzenia = "AUTOID_Zdarzenia";
#else
        const string Terminale = "Terminale";
        const string Zdarzenia = "Zdarzenia";
#endif

        const int ofsIn   = 100;
        const int ofsOut  = 200;
        const int ofsDuty = 300;
        const int ofsBack = 400;
        
        public static void ImportData()
        {
            SqlConnection conAUTOID = null;
            int pid = Log.Info(Log.t2APP_IMPORTRCP, LogInfo + " - START", null, Log.PENDING);
            try
            {
                bool ok = false;
                string fid = null;
                string lid = null;
                string lastId  = db.getScalar("select LastId from RCPLastId where Id = 1");
                if (String.IsNullOrEmpty(lastId)) lastId = "0";
                conAUTOID = db.Connect(AUTOID);
                int cntT = db.ImportTable(tmpTerminale, true, conAUTOID, String.Format("select * from {0}", Terminale));
                //int cntZ = db.ImportTable(tmpZdarzenia, true, conAUTOID, String.Format("select [ID],[nrPersonalny],[nrRCP],[DataCzas],[Terminal],[Tryb],[Karta],null as Karta2 from Zdarzenia where ID > {0}", lastId));
                int cntZ = db.ImportTable(tmpZdarzenia, true, conAUTOID, String.Format("select *, null as Karta2 from {1} where ID > {0}", lastId, Zdarzenia));  // * dzieki temu strukture view i tabeli mozna mozna zmienic poza app, karta2 to pozostałosć ...
                db.Disconnect(conAUTOID);
                //----- info -----
                DataRow dr = db.getDataRow(String.Format("select min(ID), max(ID) from {0}", tmpZdarzenia));
                fid = db.getValue(dr, 0);
                lid = db.getValue(dr, 1);
                string info = String.Format("Terminale: {0}, Zdarzenia: {1} (lastId: {2}, id: {3}-{4})", cntT, cntZ, lastId, String.IsNullOrEmpty(fid) ? "?" : fid, String.IsNullOrEmpty(lid) ? "?" : lid);
                Log.Info(Log.t2APP_IMPORTRCP, pid, LogInfo, info, Log.OK);
                //----- import -----                
                if (cntT >= 0 && cntZ >= 0)
                {
                    //----- Readers -----
                    int ret = db.execSQLEx(String.Format(@"
declare @ofsIn int
declare @ofsOut int
declare @ofsDuty int
declare @ofsBack int
set @ofsIn   = {0} --100
set @ofsOut  = {1} --200
set @ofsDuty = {2} --300
set @ofsBack = {3} --400

create table #ttt (
	ID int 
	,Nazwa nvarchar(200)
	,Opis nvarchar(500)
	,InOut bit
	,Duty bit)

insert into #ttt select ID + @ofsIn   as ID, 'WE - '	 + Nazwa as Nazwa, Opis, 0 as InOut, 0 as Duty from tmpAUTOID_Terminale -- we AUTOID 1 (przycisk F1)
insert into #ttt select ID + @ofsOut  as ID, 'WY - '	 + Nazwa as Nazwa, Opis, 1 as InOut, 0 as Duty from tmpAUTOID_Terminale -- wy AUTOID 2 (przycisk F2)
insert into #ttt select ID + @ofsDuty as ID, 'WY SŁ. - ' + Nazwa as Nazwa, Opis, 1 as InOut, 1 as Duty from tmpAUTOID_Terminale -- wy AUTOID 4 (służbowe, przycisk F3)
insert into #ttt select ID + @ofsBack as ID, 'POWRÓT - ' + Nazwa as Nazwa, Opis, 0 as InOut, 0 as Duty from tmpAUTOID_Terminale -- we AUTOID 5 (powtrót po F4)

----- nieaktywne -----
update Readers set Active = 0
--select * 
from Readers R
left join #ttt T on T.Id = R.Id
where T.ID is null and R.Active = 1

----- zmiana nazwy, aktywności -----
update Readers set Name = T.Nazwa, Zone = T.Opis, InOut = T.InOut, Duty = T.Duty, Active = 1
--select * 
from Readers R
inner join #ttt T on T.Id = R.Id
where R.Name != T.Nazwa or R.Zone != T.Opis or R.InOut != T.InOut or R.Duty != T.Duty or R.Active != 1

----- nowe -----
insert into Readers
select T.ID, T.Nazwa, T.Opis, T.ID, 32, 0, 32, T.InOut, T.Duty, 1 
from #ttt T
left join Readers R on R.Id = T.ID
where R.Id is null

--select * from #ttt
drop table #ttt
                        ", ofsIn, ofsOut, ofsDuty, ofsBack));
                    ok = ret >= 0;
                    if (!ok) Log.Error(Log.t2APP_IMPORTRCP, pid, LogError, String.Format("Readers, error: {0}", ret));
                    //----- RCP -----
                    if (cntZ > 0)    // cos jest do zapisania
                    {
                        if (ok)
                        {
#if KDR
                            ret = db.execSQLEx(String.Format(@"
declare @ofsIn int
declare @ofsOut int
declare @ofsDuty int
declare @ofsBack int
set @ofsIn   = {0} --100
set @ofsOut  = {1} --200
set @ofsDuty = {2} --300
set @ofsBack = {3} --400

insert into RCP (Czas, ECCode, ECUserId, ECReaderId, ECDoorType, InOut, Duty)
select Z.DataCzas, 1, Z.ECUserId, Z.ECReaderId, 32, Z.InOut, Z.Duty
--select *
from 
(
select Z.DataCzas, P1.ECUserId,  -- jak nie znajdzie to z -
Z.nrRCP, Z.Tryb, 
case 
when Z.Tryb = 3 then 
	case when Z2.SetAsOut = 1 then @ofsOut
	else @ofsIn 
	end
when Z.Tryb = 1 then @ofsIn
when Z.Tryb = 2 then @ofsOut
when Z.Tryb = 4 then @ofsDuty
when Z.Tryb = 5 then @ofsBack
else @ofsIn
end + Z.Terminal as ECReaderId, 
case when Z.Tryb in (2,4) or (Z.Tryb = 3 and Z2.SetAsOut = 1) then 1 else 0 end as InOut,
case when Z.Tryb in (4,5) then 1 else 0 end as Duty
,Z2.SetAsOut  --, Z1.*, R1.*, dbo.getdate(Z.DataCzas)
from tmpAUTOID_Zdarzenia Z
left join Pracownicy P on P.KadryId = Z.nrPersonalny
outer apply (select ISNULL(P.Id, -Z.nrPersonalny) as ECUserId) P1
outer apply (select top 1 * from tmpAUTOID_Zdarzenia where nrRCP = Z.nrRCP and DataCzas < Z.DataCzas and DataCzas >= dbo.getdate(Z.DataCzas) order by DataCzas desc) Z1   
outer apply (select top 1 * from RCP where ECUserId = P1.ECUserId and Czas < Z.DataCzas and Czas >= dbo.getdate(Z.DataCzas) and Z1.ID is null order by Czas desc) R1   -- optymalizacja
outer apply (select case when Z1.ID is not null or R1.ECUniqueId is not null then 1 else 0 end as SetAsOut) Z2
) Z
left join RCP R on R.ECUserId = Z.ECUserId and R.Czas = Z.DataCzas and R.ECReaderId = Z.ECReaderId
where R.ECUniqueId is null      -- tylko nieistniejące, InOut, Duty nieistotne

--and dbo.getdate(Z.DataCzas) = '20160401' order by Z.ECUserId, DataCzas
                            ", ofsIn, ofsOut, ofsDuty, ofsBack));
#else
                            ret = db.execSQLEx(String.Format(@"
declare @ofsIn int
declare @ofsOut int
declare @ofsDuty int
declare @ofsBack int
set @ofsIn   = {0} --100
set @ofsOut  = {1} --200
set @ofsDuty = {2} --300
set @ofsBack = {3} --400

insert into RCP (Czas, ECCode, ECUserId, ECReaderId, ECDoorType, InOut, Duty)
select Z.DataCzas, 1, Z.ECUserId, Z.ECReaderId, 32, Z.InOut, Z.Duty from 
(
select Z.DataCzas, ISNULL(P.Id, -Z.nrPersonalny) as ECUserId,  -- jak nie znajdzie to z - 
case Z.Tryb 
when 1 then @ofsIn 
when 2 then @ofsOut
when 4 then @ofsDuty
when 5 then @ofsBack
else @ofsIn
end + Z.Terminal as ECReaderId, 
case when Z.Tryb in (2,4) then 1 else 0 end as InOut,
case when Z.Tryb in (4,5) then 1 else 0 end as Duty
from tmpAUTOID_Zdarzenia Z
left join Pracownicy P on P.KadryId = Z.nrPersonalny
) Z
left join RCP R on R.ECUserId = Z.ECUserId and R.Czas = Z.DataCzas and R.ECReaderId = Z.ECReaderId
where R.ECUniqueId is null      -- tylko nieistniejące, InOut, Duty nieistotne
                        ", ofsIn, ofsOut, ofsDuty, ofsBack));
#endif
                            ok = ret >= 0;   // moze nie być rekordów do dodania
                            if (!ok) Log.Error(Log.t2APP_IMPORTRCP, pid, LogError, String.Format("RCP, error: {0}", ret));
                        }
                        //----- spr. nieistniejących -----
                        if (ok)
                        {
                            DataSet ds = db.getDataSet(String.Format(@"
select distinct Z.nrPersonalny, Z.nrRCP, null as Karta 
from tmpAUTOID_Zdarzenia Z
left join Pracownicy P on P.KadryId = Z.nrPersonalny
where P.Id is null
order by 1
                            ", tmpZdarzenia));
                            if (db.getCount(ds) > 0)
                            {
                                Log.Info(Log.t2APP_IMPORTRCP, pid, LogInfo + " - BRAK POWIĄZAŃ", "KadryId: " + db.Join(ds, 0, ","), Log.OK);
                            }
                        }
                        //----- karty -----
                        if (ok)
                        {
                            ret = db.execSQLEx(@"
insert into PracownicyKarty (IdPracownika, Od, Do, RcpId, NrKarty)
select Z.IdPracownika, dbo.getdate(Z1.DataCzas), null as Do, Z.IdPracownika, Z.Karta from 
(
select distinct Z.nrPersonalny, Z.nrRCP, Z.Karta, P.Id as IdPracownika
from tmpAUTOID_Zdarzenia Z
inner join Pracownicy P on P.KadryId = Z.nrPersonalny
where Z.Karta is not null
) Z
outer apply (select top 1 * from tmpAUTOID_Zdarzenia where nrPersonalny = Z.nrPersonalny and Karta = Z.Karta order by DataCzas) Z1
left join PracownicyKarty PK on PK.IdPracownika = Z.IdPracownika and PK.NrKarty = Z.Karta and Z1.DataCzas between PK.Od and ISNULL(PK.Do,'20990909')
where PK.Id is null 
                        ");
                            ok = ret >= 0;
                            if (!ok) Log.Error(Log.t2APP_IMPORTRCP, pid, LogError, String.Format("Karty, error: {0}", ret));
                        }
                        //----- LastId -----
                        if (ok) // cos jest do zapisania
                        {
                            //lastId = db.getScalar(String.Format("select max(ID) from {0}", tmpZdarzenia));
                            if (!String.IsNullOrEmpty(lid))
                            {
                                ok = db.update("RCPLastId", 1, "LastId", "Id={0}", 1, lid);
                                if (!ok) ok = db.insert("RCPLastId", 0, "Id,LastId", 1, lid);
                            }
                            else ok = false;
                            if (!ok) Log.Error(Log.t2APP_IMPORTRCP, pid, LogError, String.Format("LastId, error: {0}", -1));
                        }
                    }
                }

                if (ok)
                {
                    Log.Info(Log.t2APP_IMPORTRCP, pid, LogInfo + " - END", "OK", Log.OK);
                    Log.Update(pid, Log.OK);
                }
                else
                {
                    Log.Error(Log.t2APP_IMPORTRCP, pid, LogError, null);
                    Log.Update(pid, Log.ERROR);
                }
            }
            catch (Exception ex)
            {
                Log.Update(pid, Log.ERROR);
                Log.Error(Log.t2APP_IMPORTRCP, pid, LogError, ex.Message);
                db.Disconnect(conAUTOID);
            }
        }
    }
}
