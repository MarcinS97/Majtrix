using HRRcp.App_Code;
using HRRcp.Areas.ME.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRRcp.Areas.ME.Models.CustomModels
{
    public class ToDoOddelegowania : IToDoGet
    {
        MatrycaMVC bazaMatryca = new MatrycaMVC();

        public List<Pracownik> listaPracownikow;
        public List<Linie> listaLinii;

        public IEnumerable<Linie> getLinie()
        {
            return listaLinii;
        }

        public Pracownik getPracownik(int id)
        {
            return listaPracownikow.ElementAt(id);
        }

        public bool setToDo()
        {
            string sql = @"declare @kierId int
declare @strId int 
declare @subStr int
declare @status nvarchar(4000)
declare @data datetime
declare @filter int
declare @lng nvarchar(2)


select
P.Id_Pracownicy,
P.Nazwisko + '' '' + P.Imie as NazwiskoImie,
UPPER(LEFT(LTRIM(P.Nazwisko), 1)) as NazwiskoLetter,
P.Nazwisko, P.Imie, P.Imie2, P.Nr_Ewid,
P.Id_Gr_Zatr, G.Rodzaj_Umowy, P.DataZatr, P.DataUmDo, P.DataZwol,
P.Id_Stanowiska, S.Nazwa_Stan,
P.Id_Str_OrgM, P.IdKierownika,
/*P.Id_Status,*/ P.APT, P.Status,
P.Login, P.Email, P.Nr_Ewid2, P.Id2,
P.IdStrumienia,
M.Id_Parent as IdStrumieniaM,
MP.Symb_Jedn as SymbolStrumieniaM,
MP.Nazwa_Jedn as NazwaStrumieniaM,
MP.Id_Parent as IdParentaStrumieniaM,
P.Id_Str_OrgM as IdLiniiM,
M.Symb_Jedn as SymbolLiniiM,
M.Nazwa_Jedn as NazwaLiniiM,
MK.Nazwisko + '' '' + MK.Imie as KierownikM,
case when D.Id is not null then A.Id_Parent       else M.Id_Parent        end as IdStrumieniaA,
case when D.Id is not null then AP.Symb_Jedn      else MP.Symb_Jedn       end as SymbolStrumieniaA,
case when D.Id is not null then AP.Nazwa_Jedn     else MP.Nazwa_Jedn      end as NazwaStrumieniaA,
case when D.Id is not null then D.IdStruktury     else P.Id_Str_OrgM      end as IdLiniiA,
case when D.Id is not null then A.Symb_Jedn       else M.Symb_Jedn        end as SymbolLiniiA,
case when D.Id is not null then A.Nazwa_Jedn      else MP.Nazwa_Jedn      end as NazwaLiniiA,
case when D.Id is not null then AK.Nazwisko + '' '' + AK.Imie else MK.Nazwisko + '' '' + MK.Imie end as KierownikA,
case when D.Id is not null then AK.Nazwisko       else MK.Nazwisko        end as KierNazwisko,
case when D.Id is not null then AK.Imie           else MK.Imie            end as KierImie,
D.Od, D.Do,
(select sum(SO.Ocena * SZ.Waga) from Oceny SO left outer join Zadania SZ on SZ.Id_Zadania = SO.Id_Zadania where SO.Id_Pracownicy = P.Id_Pracownicy and SO.Aktualna = 1) as Ocena
/*,T.Status as Absencja*/


,null as OOStatus
,null as OOWynik


from Pracownicy P
left outer join GrZatr G on G.Id_Gr_Zatr = P.Id_Gr_Zatr
left outer join Stanowiska S on S.Id_Stanowiska = P.Id_Stanowiska
left outer join StrOrg M on M.Id_Str_Org = P.Id_Str_OrgM

--left outer join StrOrg MP on MP.Id_Str_Org = M.Id_Parent
left outer join StrOrg MP on MP.Id_Str_Org = P.IdStrumienia

left outer join Przelozeni MK on MK.Id_Przelozeni = P.IdKierownika
left outer join Oddelegowania D on D.IdPracownika = P.Id_Pracownicy and @Data between D.Od and D.Do and D.Status = 2
left outer join StrOrg A on A.Id_Str_Org = D.IdStruktury
left outer join StrOrg AP on AP.Id_Str_Org = A.Id_Parent
left outer join Przelozeni AK on AK.Id_Przelozeni = D.IdKierownika
/*left outer join StatusPrac T on T.Id_Status_Prac = P.Id_Status */

where P.Id_Str_OrgM is null and P.Status >= 0
and
    (
    ISNULL(@kierId, -99) = -99 or-- admin
    P.IdStrumienia is null or-- nie wskazano w TETA
    P.IdStrumienia in (--jednostka powyżej i wszystkie z podstruktury przełożonego

        select distinct T.Id

        from StrukturaPrzelozeni SP

        outer apply(select * from dbo.fn_GetStrOrgTree(ISNULL(SP.IdStruktury, 0), 1, GETDATE())) T

        where SP.IdPrzelozonego = @kierId

            union

        select distinct ISNULL(S.Id_Parent, 0)

        from StrukturaPrzelozeni SP

        inner
        join StrOrg S on S.Id_Str_Org = SP.IdStruktury

        where SP.IdPrzelozonego = @kierId

        )
    )
order by NazwiskoImie";
            string sql2 = @"exec sp_executesql N'
--declare @kierId int
--set @kierId = 3

select 
P.Id_Pracownicy,
P.Nazwisko + '' '' + P.Imie as NazwiskoImie,
UPPER(LEFT(LTRIM(P.Nazwisko), 1)) as NazwiskoLetter,
P.Nazwisko, P.Imie, P.Imie2, P.Nr_Ewid,
P.Id_Gr_Zatr, G.Rodzaj_Umowy, P.DataZatr, P.DataUmDo, P.DataZwol,
P.Id_Stanowiska, S.Nazwa_Stan,
P.Id_Str_OrgM, P.IdKierownika,
/*P.Id_Status,*/ P.APT, P.Status,
P.Login, P.Email, P.Nr_Ewid2, P.Id2,
P.IdStrumienia,
M.Id_Parent     as IdStrumieniaM, 
MP.Symb_Jedn    as SymbolStrumieniaM, 
MP.Nazwa_Jedn   as NazwaStrumieniaM,
MP.Id_Parent    as IdParentaStrumieniaM,
P.Id_Str_OrgM   as IdLiniiM,
M.Symb_Jedn     as SymbolLiniiM,
M.Nazwa_Jedn    as NazwaLiniiM,
MK.Nazwisko + '' '' + MK.Imie as KierownikM, 
case when D.Id is not null then A.Id_Parent       else M.Id_Parent        end as IdStrumieniaA,
case when D.Id is not null then AP.Symb_Jedn      else MP.Symb_Jedn       end as SymbolStrumieniaA,
case when D.Id is not null then AP.Nazwa_Jedn     else MP.Nazwa_Jedn      end as NazwaStrumieniaA,
case when D.Id is not null then D.IdStruktury     else P.Id_Str_OrgM      end as IdLiniiA,
case when D.Id is not null then A.Symb_Jedn       else M.Symb_Jedn        end as SymbolLiniiA,
case when D.Id is not null then A.Nazwa_Jedn      else MP.Nazwa_Jedn      end as NazwaLiniiA,
case when D.Id is not null then AK.Nazwisko + '' '' + AK.Imie else MK.Nazwisko + '' '' + MK.Imie end as KierownikA, 
case when D.Id is not null then AK.Nazwisko       else MK.Nazwisko        end as KierNazwisko, 
case when D.Id is not null then AK.Imie           else MK.Imie            end as KierImie, 
D.Od, D.Do,
(select sum(SO.Ocena * SZ.Waga) from Oceny SO left outer join Zadania SZ on SZ.Id_Zadania = SO.Id_Zadania where SO.Id_Pracownicy = P.Id_Pracownicy and SO.Aktualna = 1) as Ocena
/*,T.Status as Absencja*/


,null as OOStatus
,null as OOWynik


from Pracownicy P 
left outer join GrZatr G on G.Id_Gr_Zatr = P.Id_Gr_Zatr 
left outer join Stanowiska S on S.Id_Stanowiska = P.Id_Stanowiska   
left outer join StrOrg M on M.Id_Str_Org = P.Id_Str_OrgM 

--left outer join StrOrg MP on MP.Id_Str_Org = M.Id_Parent 
left outer join StrOrg MP on MP.Id_Str_Org = P.IdStrumienia 

left outer join Przelozeni MK on MK.Id_Przelozeni = P.IdKierownika 
left outer join Oddelegowania D on D.IdPracownika = P.Id_Pracownicy and @Data between D.Od and D.Do and D.Status = 2 
left outer join StrOrg A on A.Id_Str_Org = D.IdStruktury 
left outer join StrOrg AP on AP.Id_Str_Org = A.Id_Parent 
left outer join Przelozeni AK on AK.Id_Przelozeni = D.IdKierownika 
/*left outer join StatusPrac T on T.Id_Status_Prac = P.Id_Status */ 

where P.Id_Str_OrgM is null and P.Status >= 0
and 
    (
    ISNULL(@kierId,-99) = -99 or    -- admin 
    P.IdStrumienia is null or	    -- nie wskazano w TETA
    P.IdStrumienia in (			    -- jednostka powyżej i wszystkie z podstruktury przełożonego
	    select distinct T.Id 
	    from StrukturaPrzelozeni SP
	    outer apply (select * from dbo.fn_GetStrOrgTree(ISNULL(SP.IdStruktury, 0), 1, GETDATE())) T
	    where SP.IdPrzelozonego = @kierId
		    union
	    select distinct ISNULL(S.Id_Parent, 0)
	    from StrukturaPrzelozeni SP
	    inner join StrOrg S on S.Id_Str_Org = SP.IdStruktury
	    where SP.IdPrzelozonego = @kierId
	    )
    )
order by NazwiskoImie
    ',N'@kierId int,@strId int,@subStr int,@status nvarchar(4000),@data datetime,@filter int,@lng nvarchar(2)',@kierId=NULL,@strId=NULL,@subStr=NULL,@status=NULL,@data=NULL,@filter=NULL,@lng=N'PL'";
            //var pracownikZalogowany = bazaMatryca.Pracownicy.Where(x => x.Login == AppUser.GetLogin());

            
            //var query = bazaMatryca.Database.SqlQuery(sql2).ToList();
            
            


            return false;
        }

    }
}