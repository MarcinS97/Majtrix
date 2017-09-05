<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRequest.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Requests.cntRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Scorecards/Controls/Requests/cntRequestHeader.ascx" TagPrefix="leet" TagName="RequestHeader" %>
<%@ Register Src="~/Scorecards/Controls/Requests/cntEmployeeList.ascx" TagPrefix="leet" TagName="EmployeeList" %>

<div id="ctRequest" runat="server" class="cntRequest">

    <asp:HiddenField ID="hidRequestId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidUpr" runat="server" Visible="false" />
    <asp:HiddenField ID="hidMode" runat="server" Visible="false" />
    <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidStatus" runat="server" Visible="false" />

    <h3 class="breakTitle"><span>Dane</span></h3>

    <leet:RequestHeader ID="RequestHeader" runat="server" />

    <h3 class="breakTitle"><span>Pracownicy</span></h3>
    <div class="tb_scroll">
    <asp:ListView ID="lvRequests" runat="server" DataSourceID="dsRequests" DataKeyNames="Id"
        InsertItemPosition="None" OnDataBound="lvRequests_DataBound" OnItemDataBound="lvRequests_ItemDataBound" OnDataBinding="lvRequests_DataBinding" >
        <ItemTemplate>
            <tr id="" class='<%# GetColorClass(GetTLClass("it", Eval("TLA").ToString()), Eval("color").ToString()) %>' >
                <td>
                    <asp:Label ID="lblLp" runat="server" />
                </td>
                <td class="pracownik">
                    <asp:HiddenField ID="hid_id" runat="server" Visible="false" Value='<%# Eval("_id") %>' />
                    <asp:HiddenField ID="hidOldPremia" runat="server" Visible="false" Value='<%# Eval("PremiaUznaniowaActual") %>' />
                    <asp:HiddenField ID="hidOldUwagi" runat="server" Visible="false" Value='<%# Eval("UwagiActual") %>' />
                    <asp:HiddenField ID="hidOldCC" runat="server" Visible="false" Value='<%# Eval("CC") %>' />
                    <asp:HiddenField ID="hidRejected" runat="server" Visible="false" Value='<%# Eval("color") %>' />
                    <asp:HiddenField ID="hidpracId" runat="server" Visible="false" Value='<%# Eval("EmployeeId") %>' />
                    
                    <input type="hidden" runat="server" class="czasPracy" value='<%# Eval("CzasPracy") %>' />
                    <input type="hidden" runat="server" class="iloscSztuk" value='<%# Eval("IloscSztuk") %>' />
                    <input type="hidden" runat="server" class="iloscBledow" value='<%# Eval("IloscBledow") %>' />
                    
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Pracownik") %>' />
                </td>
                <td id="tdCC" runat="server" visible='<%# IsCustom() %>'>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCC" ErrorMessage="" ValidationGroup="sendvg"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblCC" runat="server" Text='<%# Eval("CC") %>' CssClass="cc" Visible='<%# !IsCustomEditable() %>' />
                    <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="dsCC" DataValueField="Id" DataTextField="Name" Visible='<%# IsCustomEditable() %>' />
                    <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
--select null as Id, 'wybierz ...' as Name, 0 as Sort 
--union all 
select CC.Id, CC.cc + ISNULL(' - ' + CC.Nazwa,'') as Name, S.Sort as Sort 
from CC
outer apply
(
    select top 1 CC2.Id from Przypisania R
    left join SplityWspP W on W.IdPrzypisania = R.Id
    left join CC CC2 on CC2.Id = W.IdCC
    where R.IdPracownika = @pracId and GETDATE() between R.Od and ISNULL(r.Do, '20990909') and R.Status = 1
    order by W.Wsp desc
) aoe
outer apply (select case when GETDATE() between CC.AktywneOd and ISNULL(CC.AktywneDo,'20990909') then case when aoe.Id = CC.Id then 0 else 1 end else 2 end as Sort) S
order by Sort, Name
                ">
                    <SelectParameters>
                        <asp:ControlParameter Name="pracId" Type="Int32"  ControlID="hidpracId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </td>
                <td id="td1" class="godz_prod num" runat="server" visible='<%# IsIndividual() %>'>
                    <asp:Label ID="lblGodzProd" runat="server" Text='<%# Eval("GodzProd") %>' CssClass="godzProd" />
                </td>
                <td id="Td2" class="czas_pracy num" runat="server" visible="false"> 
                    <asp:Label ID="lblCzasPracy" runat="server" Text='<%# Eval("CzasPracy") %>' CssClass="czasPracy" />
                </td>
                <td id="Td3" class="prod num" runat="server" visible='<%# IsIndividual() %>'>
                    <asp:Label ID="lblProd" runat="server" Text='<%# Eval("Produktywnosc") %>' CssClass="prod" />
                </td>
                <td id="Td4" class="ilosc_sztuk num" runat="server" visible="false">
                    <asp:Label ID="lblIloscSztuk" runat="server" Text='<%# Eval("IloscSztuk") %>' CssClass="iloscSztuk" />
                </td>
                <td id="Td5" class="ilosc_bledow num" runat="server" visible="false">
                    <asp:Label ID="lblIloscBledow" runat="server" Text='<%# Eval("IloscBledow") %>' CssClass="iloscBledow" />
                </td>
                <td id="Td6" class="fpy num" runat="server" visible='<%# IsIndividual() %>'>
                    <asp:Label ID="lblFpy" runat="server" Text='<%# Eval("FPY") %>' CssClass="fpy" />
                </td>
                <td class="premmies num" runat="server" visible='<%# !IsCustom() %>'>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("PremiaMiesieczna") %>' CssClass="premiaMiesieczna" />
                </td>
                <td class="abs num" runat="server" visible='<%# IsGroup() %>'>
                    <asp:Label ID="lblAbs" runat="server" Text='<%# Eval("Absencja") %>' />
                </td>
                <td class="absKor num" runat="server" visible='<%# IsGroup() %>'>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("AbsencjaKorekta") %>' />
                </td>
                <td id="Td7" class="czas num" runat="server" visible='<%# !IsCustom() %>'> <%--visible='<%# IsGroup() %>'--%>
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Czas") %>' />
                </td>
                <td runat="server" class='<%# GetNumClass("premuzn premiaUznaniowa", !(IsInEdit() && !IsTL(Eval("TL").ToString()))) %>'>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbPremiaUznaniowa" ErrorMessage="" ValidationGroup="sendvg"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblPremiaUznaniowa" runat="server" Text='<%# Eval("PremiaUznaniowaActual") %>' ToolTip='<%# GetToolTip(Eval("PremiaUznaniowa")) %>' Visible='<%# !(IsInEdit() && !IsTL(Eval("TL").ToString())) %>' />
                    <asp:TextBox ID="tbPremiaUznaniowa" runat="server" Text='<%# Eval("PremiaUznaniowaActual") %>' ToolTip='<%# GetToolTip(Eval("PremiaUznaniowa")) %>' MaxLength="8" Visible='<%# IsInEdit() && !IsTL(Eval("TL").ToString()) %>' autocomplete="off" />
                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" TargetControlID="tbPremiaUznaniowa" FilterType="Custom" ValidChars="0123456789" />
                </td>
                <td class="num" runat="server" visible='<%# !IsCustom() %>' >
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("RazemPremia") %>' CssClass="razemPremia" />
                </td>
                <td class="uwagi" style="white-space: normal; width: 400px;">
                    <asp:Label ID="lblUwagi" runat="server" Text='<%# Eval("UwagiActual") %>' ToolTip='<%# GetToolTip(Eval("Uwagi")) %>' Visible='<%# !(IsInEdit() && !IsTL(Eval("TL").ToString())) %>' />
                    <asp:TextBox ID="tbUwagi" runat="server" Text='<%# Eval("UwagiActual") %>' ToolTip='<%# GetToolTip(Eval("Uwagi")) %>' MaxLength="200" TextMode="SingleLine" Visible='<%# IsInEdit() && !IsTL(Eval("TL").ToString()) %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbUwagi" ErrorMessage="Pole nie może być puste" ValidationGroup='<%# Eval("Id") + "empRej" %>'></asp:RequiredFieldValidator>
                </td>
                <td class="control" runat="server" visible='<%# IsInEdit() %>'>
                    <asp:Button ID="btnReject" runat="server" OnClick="RejectItem" Text="Odrzuć" CommandArgument='<%# Eval("Id") %>' Visible='<%# RejectButtonVisible(Eval("color").ToString()) && IsInEdit() && IsToAcc()  %>' ValidationGroup='<%# Eval("Id") + "empRej" %>' />
                    <asp:Button ID="btnUnreject" runat="server" OnClick="UnrejectItem" Text="Przywróć" CommandArgument='<%# Eval("Id") %>' Visible='<%# !RejectButtonVisible(Eval("color").ToString()) && IsInEdit()  %>' />
                    <asp:Button ID="btnRemoveEmployee" runat="server" CssClass="" style="xcolor: Red;" Text="Usuń"  Visible='<%# DeleteButtonVisible() && !IsTL(Eval("TL").ToString()) %>' OnClick="RemoveEmployee" CommandArgument='<%# Eval("Id") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" class="table0">
                <tr class="edt">
                    <td>
                        <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                        <br />
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="Table1" runat="server" class="ListView4 ListView1 tbRequest ">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table id="itemPlaceholderContainer" runat="server">
                            <tr id="Tr2" runat="server" style="">
                                <th id="th0" runat="server"><asp:LinkButton ID="LinkButton0" runat="server" Text="Lp" CommandName="" CommandArgument="Lp" /></th>
                                <th id="th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Pracownik" CommandName="Sort" CommandArgument="Pracownik" /></th>
                                <th id="th1c" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="CC" CommandName="Sort" CommandArgument="CC" /></th>
                                <th id="th1i" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Godz. Prod." CommandName="Sort" CommandArgument="GodzProd" /></th>
                                <th id="th2i" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Czas pracy" CommandName="Sort" CommandArgument="CzasPracy" /></th>
                                <th id="th3i" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" Text="Produktywność" CommandName="Sort" CommandArgument="Produktywność" /></th>
                                <th id="th4i" runat="server"><asp:LinkButton ID="LinkButton6" runat="server" Text="Ilość sztuk" CommandName="Sort" CommandArgument="IloscSztuk" /></th>
                                <th id="th5i" runat="server"><asp:LinkButton ID="LinkButton7" runat="server" Text="Ilość błędów" CommandName="Sort" CommandArgument="IlsocBledow" /></th>
                                <th id="th6i" runat="server"><asp:LinkButton ID="LinkButton8" runat="server" Text="FPY" CommandName="Sort" CommandArgument="fpy" /></th>
                                <th id="th2c" runat="server"><asp:LinkButton ID="LinkButton9" runat="server" Text="Premia Miesięczna (zł)" CommandName="Sort" CommandArgument="PremiaMiesieczna" /></th>
                                <th id="th1g" runat="server"><asp:LinkButton ID="LinkButton10" runat="server" Text="Ilość dni absencji" CommandName="Sort" CommandArgument="Absencja" /></th>
                                <th id="th2g" runat="server"><asp:LinkButton ID="LinkButton11" runat="server" Text="Korekta premii za dyspozycjność (zł)" CommandName="Sort" CommandArgument="AbsencjaKorekta" /></th>
                                <th id="th3g" runat="server"><asp:LinkButton ID="LinkButton12" runat="server" Text="Czas produktywny bez przerwy" CommandName="Sort" CommandArgument="Czas" /></th>
                                <th id="th2" runat="server"><asp:LinkButton ID="LinkButton13" runat="server" Text="Premia Uznaniowa (zł)" CommandName="Sort" CommandArgument="PremiaUznaniowa" /></th>
                                <th id="th3c" runat="server"><asp:LinkButton ID="LinkButton14" runat="server" Text="Razem Premia (zł)" CommandName="Sort" CommandArgument="RazemPremia" /></th>
                                <th id="th3" runat="server"><asp:LinkButton ID="LinkButton15" runat="server" Text="Uwagi" CommandName="Sort" CommandArgument="Uwagi" /></th>
                                <th id="thControl" runat="server">
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                            <tr class="it footer">
                                <td colspan="2">
                                   
                                </td>
                                <td id="ft1c" runat="server">
                                
                                </td>
                                <td class="num ind" id="ft1i" runat="server">
                                    <span class="sumGodzProd"></span>
                                </td>
                                <td class="num ind" id="ft2i" runat="server" visible="false">
                                    <span class="sumCzasPracy"></span>
                                </td>
                                <td class="num ind" id="ft3i" runat="server">
                                    <span class="sumProd"></span>
                                </td>
                                <td class="num ind" id="ft4i" runat="server" visible="false">
                                    <span class="sumIloscSztuk"></span>
                                </td>
                                <td class="num ind" id="ft5i" runat="server" visible="false">
                                    <span class="sumIloscBledow"></span>
                                </td>
                                <td class="num ind" id="ft6i" runat="server">
                                    <span class="sumFpy"></span>
                                </td>
                                <td class="num" id="ft1" runat="server" >
                                    <span class="sumPremiaMiesieczna"></span>
                                </td>
                                <td class="num" id="ft1g" runat="server">
                                    <span class="sumAbs"></span>
                                </td>
                                <td class="num" id="ft2g" runat="server">
                                    <span class="sumAbsKor"></span>
                                </td>
                                <td class="num" id="ft3g" runat="server">
                                    <span class="sumCzas"></span>
                                </td>
                                <td class="num">
                                    <span class="sumPremiaUznaniowa"></span>
                                </td>
                                <td class="num" id="ft2" runat="server">
                                    <span class="sumRazemPremia"></span>
                                </td>
                                <td></td>
                                <td colspan="1" class="control" id="ftControl" runat="server">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
    
<%--        <div id="divUwagi" runat="server">
            <asp:TextBox ID="tbUwagiWniosek" runat="server" TextMode="MultiLine" />
        </div>--%>
    </div>
    
<%--
    <h3 class="breakTitle"></h3>
--%>
    
    <asp:SqlDataSource ID="dsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
declare @abs nvarchar(MAX)
select @abs = k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = 'SCABSENCJE'

	select p._id, p.Id, pr.KadryId, pr.Imie, pr.Id as EmployeeId, pr.Nazwisko, p.PremiaMiesieczna, p.PremiaUznaniowa, p.Uwagi, p.Status, p2.PremiaUznaniowa as PremiaUznaniowaActual, p2.Uwagi as UwagiActual, p2.GodzProd, p2.CzasPracy, p2.IloscSztuk, p2.IloscBledow, cc.cc, p2.IdCC, w.IdPracownika, w.Data, w.IdTypuArkuszy as typark, p2.Czas into #aaa from scPremie p
	left join Pracownicy pr on pr.Id = p.IdPracownika
	left join scPremie p2 on p2.Id = p.Id and p2._do is null
	left join CC cc on cc.Id = p2.IdCC
	left join scWnioski w on w.Id = p.IdWniosku
	where p.IdWniosku= @wniosekId and p.IdPracownika &gt; 0 and w.IdPracownika != pr.Id order by pr.Id, p._id

select
  MAX(prem._id) as _id
, prem.Id
, prem.EmployeeId
, prem.Nazwisko + ' ' + prem.Imie + isnull(' (' + prem.KadryId + ')', '') as Pracownik
, convert(varchar, round(prem.PremiaMiesieczna, 0)) as PremiaMiesieczna
, dbo.cat(CONVERT(varchar, round(prem.PremiaUznaniowa, 2)), '¬', 1) as PremiaUznaniowa
, convert(varchar, round(prem.PremiaMiesieczna + prem.PremiaUznaniowaActual, 2)) as RazemPremia
, dbo.cat(prem.Uwagi, '¬', 1) as Uwagi
, prem.Status
, convert(varchar, prem.PremiaUznaniowaActual) as PremiaUznaniowaActual
, prem.UwagiActual
, case when prem.Status is null then 0 else 1 end as color
, round(prem.GodzProd, 2) as GodzProd
, round(prem.CzasPracy, 2) as CzasPracy
, convert(varchar, case when prem.CzasPracy &gt; 0 then round((prem.GodzProd / prem.CzasPracy), 2) * 100 else 0 end) + '%' as Produktywnosc
, prem.IloscSztuk
, prem.IloscBledow
, convert(varchar, case when prem.IloscSztuk &gt; 0 then (convert(float, (prem.IloscSztuk - prem.IloscBledow)) / prem.IloscSztuk) * 100 else 0 end) + '%' as FPY
, prem.cc
, prem.IdCC
, case when prem.EmployeeId = @observerId then 1 else 0 end as TL
, case when prem.EmployeeId = prem.IdPracownika then 1 else 0 end as TLA --ta sytuacja jest i tak niemozliwa do osiagniecia, legacy support
--, ISNULL(SUM(Ilosc), 0) as Absencja
, ISNULL(MAX(Ilosc), 0) as Absencja
, ROUND(aoa.AbsencjaKorekta, 0) as AbsencjaKorekta
, round(ISNULL(prem.Czas, prem.CzasPracy), 2) as Czas -- Hoffmann's Averse
from #aaa prem
outer apply
(
	select COUNT(*) as Ilosc from dbo.GetDates2(dbo.bom(prem.Data), dbo.eom(prem.Data)) d
	inner join Absencja a on d.Data between a.DataOd and a.DataDo and a.Kod in (select items from dbo.SplitInt(@abs, ',')) and a.IdPracownika = prem.EmployeeId
	inner join PlanPracy pp on pp.Data = d.Data and pp.IdPracownika = prem.EmployeeId and ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1
	left join Zmiany z on z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
	where z.Typ not in (1, 2, 3)

	--select DATEDIFF(DAY,
	--		case when dbo.bom(prem.Data) &lt; a.DataOd then a.DataOd else dbo.bom(prem.Data) end,
	--		case when dbo.eom(prem.Data) &gt; a.DataDo then a.DataDo else dbo.eom(prem.Data) end
	--	) + 1 as Ilosc from Absencja a
	--left join AbsencjaKody ak on ak.Kod = a.Kod
	--outer apply (select k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = 'SCABSENCJE') oa
	--where a.IdPracownika = prem.EmployeeId and a.DataOd &lt;= /*@do*/dbo.eom(prem.Data) and a.DataDo &gt;= dbo.bom(prem.Data) and ak.Kod in (select items from dbo.SplitInt(oa.Parametr, ','))
) oa
outer apply
(
	select top 1 case when ISNULL(Ile, 1) &gt;= 1337 then ISNULL(Ile, 1)  - 1337 else (ISNULL(Ile, 1) - 1) * prem.PremiaMiesieczna end as AbsencjaKorekta from scAbsencje a where a.IdTypuArkuszy = prem.typark and (prem.Data between a.Od and ISNULL(a.Do, '20990909')) and TL = 0
		and DlaIlu &lt;= ISNULL(oa.Ilosc, 0) order by DlaIlu desc
)
aoa
group by prem.Id, prem.KadryId, prem.EmployeeId, prem.Imie, prem.Nazwisko, prem.PremiaMiesieczna, prem.Status, prem.PremiaUznaniowaActual, prem.UwagiActual, prem.GodzProd, prem.CzasPracy, prem.IloscSztuk, prem.IloscBledow, prem.cc, prem.IdCC, prem.EmployeeId, prem.IdPracownika, prem.Data, typark, aoa.AbsencjaKorekta, prem.Czas
order by TLA desc, Pracownik asc

">
        <SelectParameters>
            <asp:ControlParameter Name="wniosekId" Type="Int32" ControlID="hidRequestId" PropertyName="Value" />
            <asp:ControlParameter Name="observerId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    
    <div class="bottom_buttons">
    
        <div id="divNewEmployee" runat="server" style="float: left;" visible="false">
            <asp:Label ID="lblNewEmployee" runat="server" Text="Dodaj pracownika: " />
            <asp:DropDownList ID="ddlEmployees" runat="server" DataValueField="Id" DataTextField="Name" DataSourceID="dsEmployees" />
            <asp:SqlDataSource ID="dsEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
/*select pr.Id, case when oa.e = 1 then '+ ' else '' end + pr.Nazwisko + ' ' + pr.Imie + isnull(' (' + pr.KadryId + ')', '') as Name, oa.e as exist
from dbo./*fn_GetTree2*/fn_GetSubPrzypisania(@ObserverId, /*0,*/ GETDATE()) /*Przypisania*/ p
left join Pracownicy pr on p.IdPracownika = pr.Id
outer apply (select case when pr.Id in (select distinct IdPracownika from scPremie where IdWniosku = @wniosekId) then 1 else 0 end as e) oa
where --p.IdKierownika = @ObserverId and GETDATE() between p.Od and ISNULL(p.Do, '20990909')
/*and*/ /*pr.Id not in (select distinct IdPracownika from scPremie where IdWniosku = @wniosekId) and*/ p.IdCommodity is not null
order by pr.Nazwisko*/

select distinct pr.Id, case when oa.e = 1 then '+ ' else '' end + pr.Nazwisko + ' ' + pr.Imie + isnull(' (' + pr.KadryId + ')', '') as Name, oa.e as exist, pr.Nazwisko
from dbo.fn_GetSubPrzypisania(@ObserverId, GETDATE()) p
left join Pracownicy pr on p.IdPracownika = pr.Id or p.IdKierownika = pr.Id
outer apply (select case when pr.Id in (select distinct IdPracownika from scPremie where IdWniosku = @wniosekId) then 1 else 0 end as e) oa
--where p.IdCommodity is not null
order by pr.Nazwisko
" >
    <SelectParameters>
        <asp:ControlParameter Name="ObserverId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
        <asp:ControlParameter Name="wniosekId" Type="Int32" ControlID="hidRequestId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
            <asp:Button ID="btnAddEmployee" runat="server" Text="Dodaj" OnClick="AddEmployee" CssClass="button100" />
        </div>
    
    
        <%--<asp:Button ID="btnEditEmployees" runat="server" Text="Edytuj pracowników" CssClass="button150" OnClick="ShowEmployeeList" style="float:left;" Visible="false" />--%>
        
        
        <asp:Button ID="btnSendConfirm" runat="server" Text="Wyślij" OnClick="SendConfirm" CssClass="button100" />
        <asp:Button ID="btnSend" runat="server" Text="Wyślij" OnClick="Send" style="display: none;"  />
        <asp:Button ID="btnRejectConfirm" runat="server" Text="Cofnij do wyjaśnienia" OnClick="RejectConfirm" CssClass="button150" />
        
        <asp:Button ID="btnReject" runat="server" Text="asd" OnClick="Reject" style="display: none;" /> 
        
        <asp:Button ID="btnDeleteGroup" runat="server" style="display: none;" OnClick="DeleteGroup" />
        <asp:Button ID="btnDestroyConfirm" runat="server" Text="Odrzuć" CssClass="button100" OnClick="DestroyConfirm"  />
        <asp:Button ID="btnDestroy" runat="server" Text="" style="display: none;" OnClick="Destroy"  />
        <asp:Button ID="btnDeleteGroupConfirm" runat="server" Text="Usuń" OnClick="DeleteGroupConfirm" CssClass="button100" />
        <asp:Button ID="btnSave" runat="server" Text="Zapisz" OnClick="Save" CssClass="button100" />
        <asp:Button ID="btnClose" runat="server" Text="Zamknij" CssClass="button100" Visible="true" OnClick="Close" />
    </div>
 <%--               <asp:UpdatePanel ID="upZoomEmp" runat="server">
                    <ContentTemplate>--%>
              <%--          <leet:EmployeeList ID="EmployeeList" runat="server" Visible="false" />--%>
<%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>
    
   
    <asp:SqlDataSource id="dsSave" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" 
    SelectCommand="
update scPremie set _do = GETDATE() where _id = {2}
insert into scPremie select Id, IdPracownika, IdWniosku, PremiaMiesieczna, {0}, {1}, Status, DataZmianyStatusu, GETDATE(), NULL, Akceptacja, TL, GodzProd, CzasPracy, IloscSztuk, IloscBledow, {3}, Czas, AbsencjaKorekta from scPremie where _id = {2}
" />


    <asp:SqlDataSource id="dsRejectItem" runat="server" SelectCommand="update scPremie set Status = 0 where Id = {0}" />
    <asp:SqlDataSource id="dsUnrejectItem" runat="server" SelectCommand="update scPremie set Status = null where Id = {0}" /> 
    <asp:SqlDataSource ID="dsSend" runat="server" SelectCommand="update scWnioski set Status = 1 where Id = {0}" />
    <asp:SqlDataSource ID="dsSendTL" runat="server" SelectCommand="update scWnioski set Status = 1, Kacc = -1, Pacc = -1, IdAkceptujacego = {1}, DataAkceptacji = GETDATE() where Id = {0}" />
    <asp:SqlDataSource ID="dsSendKieras" runat="server" SelectCommand="update scWnioski set Status = 2, Kacc = 1, Pacc = -1, DataAkceptacjiK = GETDATE(), DataAkceptacjiP = null, IdK = {1} where Id = {0}" />
    <asp:SqlDataSource ID="dsSendPrezes" runat="server" SelectCommand="update scWnioski set Status = 3, Kacc = 1, Pacc = 1, DataAkceptacjiP = GETDATE(), IdP = {1} where Id = {0}" />
    <asp:SqlDataSource ID="dsRejectKieras" runat="server" SelectCommand="update scWnioski set Status = 0, Kacc = 0, Pacc = -1, DataAkceptacjiK = null, DataAkceptacjiP = null where Id = {0}" />
    <asp:SqlDataSource ID="dsRejectPrezes" runat="server" SelectCommand="update scWnioski set Status = 1, Kacc = -1, Pacc = 0, DataAkceptacjiP = null where Id = {0}" />

    <asp:SqlDataSource ID="dsAddEmployee" runat="server" SelectCommand="insert into scPremie (/*Id, */IdPracownika, IdWniosku, _Od) values (/*CONVERT(int, RAND() * 10000),*/ {0}, {1}, GETDATE())" />
    <asp:SqlDataSource ID="dsRemoveEmployee" runat="server" SelectCommand="delete from scPremie where Id = {0}" />

    <asp:SqlDataSource ID="dsDeleteGroup" runat="server" SelectCommand="delete from scPremie where IdWniosku = {0}" />
    <asp:SqlDataSource ID="dsDeleteRequest" runat="server" SelectCommand="delete from scWnioski where Id = {0}" />
    <asp:SqlDataSource ID="dsDestroy" runat="server" SelectCommand="update scWnioski set Status = -1, IdOdrzucajacego = {1}, DataOdrzucenia = {2} where Id = {0}" />

    <asp:SqlDataSource ID="dsSendButton" runat="server" SelectCommand="
declare @wniosek int = {0}
declare @typark int
declare @date datetime
declare @pracId int
declare @status int

select @typark = IdTypuArkuszy, @date = Data, @pracId = IdPracownika, @status = Status from scWnioski where Id = @wniosek
--zaremuj to jak chcesz skads indziej ten caly syf

--declare @WPI float = (select Parametr from scParametry where Typ = 'WPI' and Parametr2 = @pracId and IdTypuArkuszy = @typark and @date between Od and isnull(Do, '20990909') and TL = 1)

declare @tl int = dbo.GetRight(@pracId, 57)

--declare @a int --pracownicy na wniosku
--declare @b int --pracownicy w przypisaniach
----      ^- nie uzywac

declare @wniosekList nvarchar(MAX)
declare @commodityList nvarchar(MAX)

--select @a = COUNT(*) from scPremie where _do is null and IdWniosku = @wniosek
--select @b = COUNT(*) + case when @WPI &gt; 0 then 1 else 0 end from Przypisania where Status = 1 and @date between Od and ISNULL(Do, '20990909') and IdKierownika = @pracId and IdCommodity is not null

--select @a =
--COUNT(*)
--from scPremie where _do is null and IdWniosku = @wniosek and IdPracownika &gt; 0
--select @b =
--COUNT(*)
--from Przypisania where Status = 1 and @date between Od and ISNULL(Do, '20990909') and IdKierownika = @pracId and IdCommodity is not null

select IdPracownika into #wniosekEkipa from scPremie where _do is null and IdWniosku = @wniosek and IdPracownika &gt; 0 /*and IdPracownika != @pracId*/ order by IdPracownika
select IdPracownika into #commodityEkipa from Przypisania where Status = 1 and /*dbo.eom(@date) between Od and ISNULL(Do, '20990909')*/ dbo.bom(@date) &lt;= ISNULL(Do, '20990909') and dbo.eom(@date) &gt;= Od and IdKierownika = @pracId and IdCommodity = @typark/*is not null*/ order by IdPracownika

select @wniosekList = dbo.cat(CONVERT(varchar, IdPracownika), ' ', 1) from #wniosekEkipa where IdPracownika != @pracId
select @commodityList = dbo.cat(CONVERT(varchar, IdPracownika), ' ', 1) from #commodityEkipa

--sprawdzajka

select case
	when
		(
			@wniosekList = @commodityList
		)
		and
		(
			(
				@Status = 0
			)
			or
			(
				@Status != 0 and (select COUNT(*) from #wniosekEkipa where IdPracownika = @pracId) = @tl
			)
		)
		then 1
		else 0
	end

drop table #wniosekEkipa
drop table #commodityEkipa

--debug area
--select @pracId
--select @a, @b
--select @wniosekList
--select @commodityList

--select case when (@status = 0 and (@a = @b or @a = @b + case when /*@WPI &gt; 0*/@tl = 1 then 1 else 0 end)) or (@Status != 0 and @a = @b + case when /*@WPI &gt; 0*/ @tl = 1 then 1 else 0 end) then 1 else 0 end
" />

    <asp:SqlDataSource ID="dsData" runat="server" SelectCommand="
declare @wniosekId int = {0}

select distinct
  w.Status
, case when w.IdTypuArkuszy = -1337 then 'TRUE' else 'FALSE' end as Custom
, case when ta.Rodzaj = 4 then 'FALSE' else 'TRUE' end as Team
from scWnioski w
left join scTypyArkuszy ta on w.IdTypuArkuszy = ta.Id
where w.Id = @wniosekId    
" />

<asp:SqlDataSource ID="dsKlinke" runat="server" SelectCommand="
declare @abs nvarchar(MAX)
select @abs = k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = 'SCABSENCJE'

select
  w.Id [Id:-]
, p.GodzProd [gp:-]
, p.CzasPracy [cp:-]
, p.IloscSztuk [is:-]
, p.IloscBledow [ib:-]
into #aaa
from scWnioski w
left join scTypyArkuszy ta on ta.Id = w.IdTypuArkuszy
left join Pracownicy k on k.Id = w.IdPracownika
left join
(
	select
	  prem.IdWniosku
	, round(prem.GodzProd, 2) as GodzProd
	, round(prem.CzasPracy, 2) as CzasPracy
	, prem.IloscSztuk
	, prem.IloscBledow
	from
	(
		 select  p._id, p.Id, p.IdWniosku, pr.KadryId, pr.Imie, pr.Id as EmployeeId, pr.Nazwisko, p.PremiaMiesieczna, p.PremiaUznaniowa, p.Uwagi, p.Status, p2.PremiaUznaniowa as PremiaUznaniowaActual, p2.Uwagi as UwagiActual, p2.GodzProd, p2.CzasPracy, p2.IloscSztuk, p2.IloscBledow, cc.cc, p2.IdCC, w.IdPracownika, w.Data, w.IdTypuArkuszy as typark, p2.Czas /*into #aaa*/ from scPremie p
		 left join Pracownicy pr on pr.Id = p.IdPracownika
		 left join scPremie p2 on p2.Id = p.Id and p2._do is null
		 left join CC cc on cc.Id = p2.IdCC
		 left join scWnioski w on w.Id = p.IdWniosku
  
	   where p.IdPracownika &gt; 0 and w.IdPracownika != pr.Id and p._do is null and w.Id = {0}
	) prem
	outer apply
	(
		 select COUNT(*) as Ilosc from dbo.GetDates2(dbo.bom(prem.Data), dbo.eom(prem.Data)) d
		 inner join Absencja a on d.Data between a.DataOd and a.DataDo and a.Kod in (select items from dbo.SplitInt(@abs, ',')) and a.IdPracownika = prem.EmployeeId
		 inner join PlanPracy pp on pp.Data = d.Data and pp.IdPracownika = prem.EmployeeId and ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1
		 left join Zmiany z on z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
		 where z.Typ not in (1, 2, 3)
	) oa
	outer apply
	(
		 select top 1 case when ISNULL(Ile, 1) &gt;= 1337 then ISNULL(Ile, 1)  - 1337 else (ISNULL(Ile, 1) - 1) * prem.PremiaMiesieczna end as AbsencjaKorekta from scAbsencje a where a.IdTypuArkuszy = prem.typark and (prem.Data between a.Od and ISNULL(a.Do, '20990909')) and TL = 0
			  and DlaIlu &lt;= ISNULL(oa.Ilosc, 0) order by DlaIlu desc
	) aoa
) p on w.Id = p.IdWniosku

/*select
  p.Id
, poa.mies*/
update scWnioski
  set PremiaTL = poa.Mies
, KlinkeQuatro = ISNULL(q0.a, 0) + ISNULL(q1.a, 0)
from scWnioski w
left join scParametry p0 on p0.IdTypuArkuszy = w.IdTypuArkuszy and p0.TL = 1 and p0.Parametr2 = w.IdPracownika and dbo.eom(w.Data) between p0.Od and ISNULL(p0.Do, '20990909') and p0.Typ = 'WPI'
left join scParametry p1 on p1.IdTypuArkuszy = w.IdTypuArkuszy and p1.TL = 1 and p1.Parametr2 = w.IdPracownika and dbo.eom(w.Data) between p1.Od and ISNULL(p1.Do, '20990909') and p1.Typ = 'WPZ'
left join Pracownicy p on p.Id = w.IdPracownika
left join scTypyArkuszy ta on w.IdTypuArkuszy = ta.Id
outer apply (select top 1 * from Przypisania prz where prz.IdPracownika = p.Id and prz.Status = 1 and prz.Od &lt;= dbo.eom(w.Data) order by prz.Od desc) prz
left join Pracownicy k on k.Id = prz.IdKierownika
outer apply (select dbo.fn_GetCC(prz.Id, 5, ', ') as cc) CC
left join scPremie prem on prem.IdWniosku = w.Id and prem._do is null and prem.IdPracownika = w.IdPracownika
outer apply (select SUM(aoe.[gp:-]) / SUM(aoe.[cp:-]) Prod, case when SUM(aoe.[is:-]) != 0 then (SUM(aoe.[is:-]) - SUM(aoe.[ib:-])) / SUM(aoe.[is:-]) else 0 end QC from #aaa aoe where aoe.[Id:-] = w.Id) IND
outer apply (select (aoe.GodzProd / aoe.CzasPracy) Prod, case when aoe.IloscSztuk != 0 then ((aoe.IloscSztuk - aoe.IloscBledow) / aoe.IloscSztuk) else 0 end QC from scPremie aoe where aoe.IdWniosku = w.Id and aoe._do is null and aoe.IdPracownika = 0 - ta.Id) GRUP
outer apply (select prem.PremiaMiesieczna Premia, (prem.GodzProd / prem.CzasPracy) Prod, case when prem.IloscSztuk != 0 then ((prem.IloscSztuk - prem.IloscBledow) / prem.IloscSztuk) else 0 end QC) DED
outer apply (select top 1 Ile from scProduktywnosc where IdTypuArkuszy = w.IdTypuArkuszy and Rodzaj = 10    and (dbo.eom(w.Data) between Od and ISNULL(Do, '20990909')) and OkresProbny = 0 and TL = 1 and DlaIlu &lt;= GRUP.Prod                                                order by DlaIlu desc) t1
outer apply (select top 1 Ile from scQC            where IdTypuArkuszy = w.IdTypuArkuszy and Rodzaj = ta.QC and (dbo.eom(w.Data) between Od and ISNULL(Do, '20990909'))                     and TL = 1 and DlaIlu &lt;= GRUP.QC                                                  order by DlaIlu desc) t2
outer apply (select top 1 Ile from scProduktywnosc where IdTypuArkuszy = w.IdTypuArkuszy and Rodzaj = 10    and (dbo.eom(w.Data) between Od and ISNULL(Do, '20990909')) and OkresProbny = 0 and TL = 1 and DlaIlu &lt;= IND.Prod                                                 order by DlaIlu desc) t3
outer apply (select top 1 Ile from scProduktywnosc where IdTypuArkuszy = w.IdTypuArkuszy and Rodzaj = 9     and (dbo.eom(w.Data) between Od and ISNULL(Do, '20990909')) and OkresProbny = 0 and TL = 1 and DlaIlu &lt;= case when ta.Rodzaj = 5 then GRUP.Prod else DED.Prod end order by DlaIlu desc) aga0
outer apply (select top 1 Ile from scQC            where IdTypuArkuszy = w.IdTypuArkuszy and Rodzaj = ta.QC and (dbo.eom(w.Data) between Od and ISNULL(Do, '20990909'))                     and TL = 1 and DlaIlu &lt;= IND.QC                                                   order by DlaIlu desc) aga1
outer apply (select top 1 Parametr a from scParametry where dbo.eom(w.Data) between Od and ISNULL(Do, '20990909') and IdTypuArkuszy = w.IdTypuArkuszy and Id = w.Quatro0 and Typ = 'QUATRO') q0
outer apply (select top 1 Parametr a from scParametry where dbo.eom(w.Data) between Od and ISNULL(Do, '20990909') and IdTypuArkuszy = w.IdTypuArkuszy and Id = w.Quatro1 and Typ = 'QUATRO') q1
outer apply
(
select
	/*(
		ISNULL(q0.a, 0) + ISNULL(q1.a, 0) + ISNULL(w.PremiaZadaniowa, 0)
	) +*/ (ISNULL(p0.Parametr, 0) * --WPI
		case when ta.Rodzaj = 5 then ISNULL(aga0.Ile, 0) else ISNULL(DED.Premia, 0) end
	) + (ISNULL(p1.Parametr, 0) * --WPZ
		case when ta.Rodzaj = 5 then ISNULL(t1.Ile, 0) * ISNULL(t2.Ile, 1) else ISNULL(t3.Ile, 0) * ISNULL(aga1.Ile, 1) end
	)
	mies,
	ISNULL(prem.PremiaUznaniowa, 0) uzn
) poa
where ((ISNULL(p0.Parametr, 0) &gt; 0) or (ISNULL(p1.Parametr, 0) &gt; 0)) and w.Id = {0}

drop table #aaa
" />

</div>
