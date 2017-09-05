using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.Portal.Controls.Ubezpieczenia
{
    public static class Ubezpieczenia
    {
        public const string skInsert = "UB_SKLADNIK_INS";
        public const string skUpdate = "UB_SKLADNIK_UPD";
        public const string skUpdateKwota = "UB_SKLADNIK_UPD_KWOTA";

        public static bool InsertSkladnik(string kadryId, string kwota, string dataOd)
        {
            Log.Info(1337, "InsertSkladnik", String.Format("ew: {0} kwota: {1} dataOd: {2}", kadryId, kwota, dataOd));
            if (db.ExecuteClientData(skInsert, kadryId, kwota, dataOd)) { }
            else
            {
                Tools.ShowErrorLog(1337, String.Format("Utworzenie składnika płacowego nie powiodło się. KadryId: {0}, Kwota: {1}, DataOd: {2}", kadryId, kwota, dataOd), "Utworzenie składnika płacowego nie powiodło się. Skontaktuj się z działem HR.");
                return false;
            }
            return true;
        }

        public static bool UpdateSkladnik(string kadryId, string dataDo)
        {
            Log.Info(1337, "UpdateSkladnik", String.Format("ew: {0} dataDo: {2}", kadryId, dataDo));
            if (db.ExecuteClientData(skUpdate, kadryId, dataDo)) { }
            else
            {
                Tools.ShowErrorLog(1337, String.Format("Zaktualizowanie składnika płacowego nie powiodło się. KadryId: {0}, DataDo: {1}", kadryId, dataDo), "Zaktualizowanie składnika płacowego nie powiodło się. Skontaktuj się z działem HR.");
                return false;
            }
            return true;
        }

        public static bool UpdateSkladnikKwota(string kadryId, string kwota, string dataOd)
        {
            Log.Info(1337, "UpdateSkladnikKwota", String.Format("ew: {0} kwota: {1} dataOd: {2}", kadryId, kwota, dataOd));
            if (db.ExecuteClientData(skUpdateKwota, kadryId, kwota, dataOd)) { }
            else
            {
                Tools.ShowErrorLog(1337, String.Format("Aktualizacja kwoty składnika płacowego nie powiodło się. KadryId: {0}, Kwota: {1}, DataOd: {2}", kadryId, kwota, dataOd), "Zaktualizowanie składnika płacowego nie powiodło się. Skontaktuj się z działem HR.");
                return false;
            }
            return true;
        }

        public static string GetSkladkaSumWniosek(string pracId, string data)
        {
            string skladka = db.Select.Scalar(db.conP, @"
declare @pracId int
declare @date datetime 

set @pracId = {0}
set @date = '{1}'

select sum(Skladka + isnull(SkladkaPlus, 0)) s 
from poWnioskiMajatkowe 
where 
  ZglaszajacyId = @pracId 
and Status > -1 
and dbo.getdate(@date) between DataOd and isnull(DataZakonczenia, '20990909')
", pracId, data);
            return skladka;
        }

        public static string GetSkladkaSumZmianaWariantu(string pracId, string data, string excludedId, string newSkladka)
        {
            string skladka = db.Select.Scalar(db.conP, @"
declare @pracId int
declare @date datetime 
declare @excludedId int
declare @skladka int

set @pracId = {0}
set @date = '{1}'
set @excludedId = {2}
set @skladka = {3}


select sum(Skladka + isnull(SkladkaPlus, 0)) + @skladka s 
from poWnioskiMajatkowe 
where 
  ZglaszajacyId = @pracId
and Status > -1 
and dbo.getdate(@date) between DataOd and isnull(DataZakonczenia, '20990909') and Id != @excludedId
", pracId, data, excludedId, newSkladka);
            return skladka;
        }

        public static string GetSkladkaSumWypowiedzenie(string pracId, string dataDo, string excludedId)
        {
            string skladka = db.Select.Scalar(db.conP, @"
declare @pracId int
declare @date datetime 
declare @excludedId int

set @pracId = {0}
set @date = '{1}'
set @excludedId = {2}

select sum(Skladka + isnull(SkladkaPlus, 0)) s 
from poWnioskiMajatkowe 
where 
  ZglaszajacyId = @pracId
and Status > -1 
and dbo.getdate(@date) between DataOd and isnull(DataZakonczenia, '20990909') and Id != @excludedId
", pracId, dataDo, excludedId);
            return skladka;
        }


    }
}