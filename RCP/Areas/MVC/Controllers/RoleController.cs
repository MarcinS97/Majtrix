using HRRcp.App_Code;
using HRRcp.Areas.MVC.Models;
using HRRcp.Areas.MVC.Models.customModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.SqlClient;

namespace HRRcp.Areas.MVC.Controllers
{
    public class RoleController : Controller
    {
        //
        // GET: /MVC/Role/
        ObiegDokumentow ob = new ObiegDokumentow();
        const string SQLFilter = @"
select
  rd.Id
, rd.Nazwa
, rd.SQL
, fw.Id FiltrWartoscId
, fd.Nazwa as NazwaFiltr
, fd.WarunekSQL
, fw.Wartosc WartoscFiltr
, fw.WarunekAlterSQL
into #aaa
from obdRoleDict rd
left join obdFiltrPoleWartosci fw on fw.IdRoliDict = rd.Id
left join obdFiltrPoleDict fd on fd.Id = fw.IdFiltrPoleDict
where rd.Id = @IdRoli and @FiltryId is null


if @FiltryId is not null
begin
	declare @FiltryCount int = LEN(@FiltryId) - LEN(REPLACE(@FiltryId, '|', ''))

	select CONVERT(int, null) Id into #ccc where 0=1

	declare @i int = 0;
	while @i < @FiltryCount
	begin
		delete from #ccc where Id is null
		insert #ccc(Id) values( CONVERT(int, LEFT( @FiltryId, CHARINDEX('|', @FiltryId, 0)-1 )) )
		set @FiltryId = RIGHT( @FiltryId, LEN(@FiltryId)-CHARINDEX('|', @FiltryId, 0) )
		set @i = @i + 1;
	end
	
	select c.* into #bbb from #ccc c
	left join obdFiltrPoleDict fd on fd.Id = c.Id
	drop table #ccc

	if (select Count(Id) from #bbb) = 0
	begin
		insert #bbb(Id) values(null)
	end

	insert #aaa
	select distinct
	  ISNULL(rd.Id, @IdRoli)
	, ISNULL(rd.Nazwa, '')
	, ISNULL(rd.SQL, 'select * from Pracownicy pr')
	, b.Id FiltrWartoscId
	, fd.Nazwa as NazwaFiltr
	, fd.WarunekSQL
	, case when b.Id is null then null else '' end WartoscFiltr
	, null WarunekAlterSQL
	from #bbb b
	left join obdRoleDict rd on rd.Id = @IdRoli
	left join obdFiltrPoleDict fd on fd.Id = b.Id
	where rd.Id = @IdRoli or @IdRoli = -1

	drop table #bbb
end

declare @ZapytanieZFiltrami nvarchar(max)

if (select top 1 FiltrWartoscId from #aaa) is not null
begin
	select top 1 @ZapytanieZFiltrami = 'select * from Pracownicy pr where ' + STUFF((
				select ' and (
				
				' +
				case when WarunekAlterSQL is not null then WarunekAlterSQL else WarunekSQL end + '
				
				)'
				from #aaa
				FOR XML PATH('')
				), 1, 5, '')
	from #aaa
end
else
begin
	select top 1 @ZapytanieZFiltrami =  'select * from Pracownicy pr' from #aaa
end

drop table #aaa

set @ZapytanieZFiltrami = replace(@ZapytanieZFiltrami, '&gt;', '>')
set @ZapytanieZFiltrami = replace(@ZapytanieZFiltrami, '&lt;', '<')
set @ZapytanieZFiltrami = replace(@ZapytanieZFiltrami, '&#x0D;', '')


select @ZapytanieZFiltrami";

        public ActionResult Index(int RoleId, int? Page)
        {
            if( ob.obdRoleDict.ToList().FindIndex(it => it.Id == RoleId) < 0 && RoleId != -1 )
                return Redirect("/MVC/obdRoleDicts");


            int pageSize = 10;
            int pageNo = Page ?? 1;

            ViewData["obdFiltrPoleWartosci"] = ob.obdFiltrPoleWartosci.ToList();
            ViewData["obdFiltrPoleDict"] = ob.Database.SqlQuery<obdFiltrPoleDict>("select * from obdFiltrPoleDict order by Typ, Nazwa").ToList();
            ViewData["obdRoleDict"] = ob.obdRoleDict.ToList();
            ViewData["RoleId"] = RoleId;

            foreach(obdFiltrPoleDict filtrDict in ob.obdFiltrPoleDict)
            {
                if(Request.HttpMethod=="POST" || RoleId == -1)
                {
                    if(Request[filtrDict.Nazwa] != null)
                        ViewData[filtrDict.Nazwa] = Request[filtrDict.Nazwa];
                }
                else
                {
                    obdFiltrPoleWartosci wartosc = ob.obdFiltrPoleWartosci.ToList().Find(it => it.IdFiltrPoleDict == filtrDict.Id && it.IdRoliDict == RoleId);
                    if(wartosc != null)
                    {
                        if(filtrDict.Typ == 2)
                        {
                            DateTime val;
                            if(DateTime.TryParseExact(wartosc.Wartosc, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out val))
                                ViewData[filtrDict.Nazwa] = val.ToString("yyyy-MM-dd");
                        }
                        else
                            ViewData[filtrDict.Nazwa] = wartosc.Wartosc;
                    }
                }
            }
            
            return View( PracownicyZRoli(RoleId, Request.HttpMethod=="POST" || RoleId == -1).ToPagedList(pageNo, pageSize) );
        }

        public ActionResult Save()
        {
            int RoleId = -1;
            if(Request["RoleId"] == "-1")
            {
                obdRoleDict rolaDict = new obdRoleDict();
                rolaDict.Nazwa = Request["RoleNazwa"] ?? "";
                rolaDict.SQL = "select * from Pracownicy pr";
                ob.obdRoleDict.Add(rolaDict);
                ob.SaveChanges();
                RoleId = rolaDict.Id;
            }
            else 
                RoleId = Tools.StrToInt(Request["RoleId"], -1);

            if(RoleId != -1)
            {
                UpdateDB(RoleId);
                Session["IdSkladajacego"] = Request["IdSkladajacego"];
            }
            return Redirect("/MVC/Role/Index?RoleId=" + RoleId); // jeżeli RoleId = -1 Index przekierowuje na stronę z błędem
        }

        public ActionResult Delete(int? IdRoli)
        {
            if(IdRoli.HasValue)
            {
                obdRoleDict rola = ob.obdRoleDict.ToList().Find(it => it.Id == IdRoli.Value);
                if(rola != null)
                {
                    List<obdFiltrPoleWartosci> polaWartosci = ob.obdFiltrPoleWartosci.ToList().FindAll(it => rola.Id == it.IdRoliDict);
                    foreach(obdFiltrPoleWartosci item in polaWartosci)
                        ob.obdFiltrPoleWartosci.Remove(item);
                    ob.obdRoleDict.Remove(rola);
                    ob.SaveChanges();
                }
            }
            return RedirectToAction("Index", "obdRoleDicts", null);
        }
       
        public ActionResult EdytujUwagiModal(int id, string CommentTextValue)
        {

            Modal modal = new Modal()
            {
                Header = L.p("Uwaga!"),
                Text = L.p("Dodaj lub zmień uwagi do pozycji zamówienia"),
                ConfirmButtonController = "Role",
                ConfirmButtonRouteValues = new { id = id, komentarz2 = "_komentarz2" },
                ConfirmButtonAction = "EdytujUwagi",
                ShowXButton = false,
                CommentText2 = "Uwagi do zamówienia:",
                ShowTextBox2 = true,
                CommentText2Value = CommentTextValue
            };


            return PartialView("_Modal", modal);
        }

        public ActionResult EdytujUwagi(int id, string komentarz2)
        {

            return RedirectToAction("Index", "Role", null);
        }

        List<Pracownicy> PracownicyZRoli(int RoleId, bool fromRequest=false)
        {
            List<SqlParameter> parameters = GetSqlParams(RoleId);
            if(fromRequest)
            {
                string filtryId = "";
                foreach(obdFiltrPoleDict poleDict in ob.obdFiltrPoleDict)
                {
                    if(Request[poleDict.Nazwa] != null && (string)Request[poleDict.Nazwa] != "")
                    {
                        filtryId += poleDict.Id + "|";
                    }
                }
                parameters.Add(new SqlParameter("FiltryId", filtryId));
            }
            else
                parameters.Add(new SqlParameter("FiltryId", DBNull.Value));

            string selectQuery = ob.Database.SqlQuery<string>(SQLFilter, parameters.ToArray()).ToList()[0];
            List<Pracownicy> menuSel = ob.Database.SqlQuery<Pracownicy>(selectQuery, parameters.Select(x => new SqlParameter(x.ParameterName, x.Value)).ToArray()).ToList();
            return menuSel;
        }

        List<SqlParameter> GetSqlParams(int RoleId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("IdRoli", RoleId));

            const string IdSkladajacego = "IdSkladajacego";
            int ids = Tools.StrToInt(Tools.GetStr(Request[IdSkladajacego]), -1);
            if(ids == -1)
                ids = Tools.StrToInt(Tools.GetStr(Session[IdSkladajacego]), -1);

            if(ids != -1)
                ViewData[IdSkladajacego] = ids;

            parameters.Add(new SqlParameter("IdSkladajacego", ids));

            const string RoleNazwa = "RoleNazwa";
            if(Request.HttpMethod=="POST" || RoleId == -1)
                ViewData[RoleNazwa] = Request[RoleNazwa];
            else
                ViewData[RoleNazwa] = ob.obdRoleDict.ToList().Find(it => it.Id == RoleId).Nazwa;


            foreach(obdFiltrPoleDict poleDict in ob.obdFiltrPoleDict)
            {
                obdFiltrPoleWartosci poleWartosc = ob.obdFiltrPoleWartosci.ToList().Find(it => it.IdFiltrPoleDict == poleDict.Id && it.IdRoliDict == RoleId);
                SqlParameter param;
                if(Request.HttpMethod=="POST" || RoleId == -1)
                {
                    if(Request[poleDict.Nazwa] != null)
                    {
                        param = new SqlParameter(poleDict.ZmiennaSQL, Request[poleDict.Nazwa]);
                        parameters.Add(param);
                    }
                }
                else
                {
                    if(poleWartosc != null)
                    {
                        param = new SqlParameter(poleDict.ZmiennaSQL, poleWartosc.Wartosc);
                        parameters.Add(param);
                    }
                }
            }

            return parameters;
        }

        void UpdateDB(int RoleId)
        {
            List<obdFiltrPoleWartosci> myValues = ob.obdFiltrPoleWartosci.ToList().FindAll( it => it.IdRoliDict == RoleId );
            List<obdFiltrPoleDict> polaW = ob.obdFiltrPoleDict.ToList();
            foreach (obdFiltrPoleDict item in polaW)
            {
                obdFiltrPoleWartosci poleWartosc = myValues.Find( it => it.IdFiltrPoleDict == item.Id );
                if(Request[item.Nazwa] != null)
                {   
                    string value = (string)Request[item.Nazwa];
                    if(item.Typ == 3) value = "1";
                    if(item.Typ == 2) value = value.Replace("-", "");

                    if(value == "" && poleWartosc != null )
                        ob.obdFiltrPoleWartosci.Remove(poleWartosc);
                    else if(value != "" && poleWartosc != null)
                        poleWartosc.Wartosc = value;
                    else if(value != "" && poleWartosc == null)
                    {
                        obdFiltrPoleWartosci pole = new obdFiltrPoleWartosci();
                        pole.IdRoliDict = RoleId;
                        pole.IdFiltrPoleDict = item.Id;
                        pole.Wartosc = value;
                        ob.obdFiltrPoleWartosci.Add(pole);
                    }
                }
                else if(poleWartosc != null)
                {
                    ob.obdFiltrPoleWartosci.Remove(poleWartosc);
                }
            }
            if(Request["RoleNazwa"] != null)
            {
                ob.obdRoleDict.ToList().Find(it => it.Id == RoleId).Nazwa = (string)Request["RoleNazwa"];
            }

            try
            {
                ob.SaveChanges();
            }
            catch(Exception ex)
            {
                Log.Error(Log.OBIEG, "RoleController.UpdateDB", ex.Message);
            }
        }
	}
}