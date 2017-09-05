<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRequestHeader.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Requests.cntRequestHeader" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:HiddenField ID="hidRequestId" runat="server" Visible="false" />
<asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="ctRequestHeader" class="cntRequestHeader">
    <asp:ListView ID="lvHeader" runat="server" DataKeyNames="Id" DataSourceID="dsHeader" 
        InsertItemPosition="None" OnItemDataBound="lvHeader_ItemDataBound" >
        <ItemTemplate>
            <table class="ListView4">
                <tr>
                    <td class="tdWrapper">
                        <table class="tbZoomPracownik">
                            <tr runat="server" Visible='<%# IsCustomEditable() && IsAdmin() %>'>
                                <td class="col1" >
                                    <asp:Literal ID="aoe" runat="server" Text="Team Leader"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTeamLeader" runat="server" DataSourceID="dsTL" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
                                    <asp:SqlDataSource ID="dsTL" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                        SelectCommand="select Id, Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')', '') Name from Pracownicy p where (dbo.GetRightId(p.Rights, 57)  = 1 or dbo.GetRightId(p.Rights, 65) = 1 or dbo.GetRightId(p.Rights, 58) = 1) and Status != -2"
                                        />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal7" runat="server" Text="Nazwa"></asp:Literal>
                                    <input type="hidden" id="hidData" runat="server" class="hidData" value='<%# Eval("Table1") %>' />
                                    <input type="hidden" id="hidData2" runat="server" class="hidData2" value='<%# Eval("Table2") %>' />
                                    <input type="hidden" id="hidData3" runat="server" class="hidData3" value='<%# Eval("Table3") %>' />
                                    <input type="hidden" id="hidGodzProd" runat="server" class="hidGodzProd" value='<%# Eval("GodzProd") %>' />
                                    <input type="hidden" id="hidCzasPracy" runat="server" class="hidCzasPracy" value='<%# Eval("CzasPracy") %>' />
                                    <input type="hidden" id="hidIlosSztuk" runat="server" class="hidIlosSztuk" value='<%# Eval("IloscSztuk") %>' />
                                    <input type="hidden" id="hidIloscBledow" runat="server" class="hidIloscBledow" value='<%# Eval("IloscBledow") %>' />
                                    <input type="hidden" id="hidWPI" runat="server" class="hidWPI" value='<%# Eval("WPI") %>' />
                                    <input type="hidden" id="hidWPZ" runat="server" class="hidWPZ" value='<%# Eval("WPZ") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("NazwaArkusza") %>' Visible='<%# !IsCustomEditable() %>' />
                                    <asp:TextBox ID="tbName" runat="server" Text='<%# Eval("NazwaArkusza") %>' Visible='<%# IsCustomEditable() %>' TextMode="MultiLine" />
                                    <br />
                                </td>
                            </tr>
                            <tr id="Tr1" runat="server">
                                <td class="col1">
                                    <asp:Literal ID="Literal1" runat="server" Text="Miesiąc wypracowania premii"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Data") %>' ToolTip='<%# Eval("DataTooltip") %>' CssClass="lblMiesiacWypracowania"   />
                                    
                                    <%--<asp:Label ID="Label12" runat="server" Text='<%# Eval("Data") %>' ToolTip='<%# Eval("DataTooltip") %>'  Visible='<%# !IsCustomEditable() %>'   />--%>
                                    <%--<asp:DropDownList ID="ddlData" runat="server" DataSourceID="dsMonths" DataValueField="Id" DataTextField="Name" Visible='<%# IsCustomEditable() %>' />--%>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal2" runat="server" Text="Miesiąc wypłaty"></asp:Literal> 
                                </td>
                                <td class="data" style="text-align: left;">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("DataWyplaty") %>' ToolTip='<%# Eval("DataWyplatyTooltip") %>' Visible='<%# !IsCustomEditable() %>' />
                                    <%--<uc1:DateEdit id="deDataWyplaty" runat="server" Date='<%# Eval("Data") %>' Visible='<%# IsEditable() %>' />--%>
                                    
                                    <asp:DropDownList ID="ddlDataWyplaty" runat="server" DataSourceID="dsMonths" DataValueField="Id" DataTextField="Name" Visible='<%# IsCustomEditable() %>' CssClass="ddlDataWyplaty" />
                                    <asp:SqlDataSource ID="dsMonths" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                        SelectCommand="declare @Date datetime = DATEADD(M, 3, GETDATE()	)
--declare @wniosekId int = 21

;with Months
as
(
select CONVERT(datetime, Convert(varchar, YEAR(@Date)) + '-' + convert(varchar, MONTH(@Date)) + '-01' )  as tDate
union all
select DATEADD(M, -1, tDate) from Months where DATEADD(M, -1, tDate) &gt; CONVERT(datetime, DATEADD(M, -2, tDate))
)
select top 3 tDate as Id, CONVERT(Varchar(7), Convert(datetime, tDate), 20) as Name from Months
union
select DataWyplaty as Id, CONVERT(Varchar(7), Convert(datetime, DataWyplaty), 20) as Name from scWnioski where Id = @wniosekId
order by 1 desc
"
                                    >
                                        <SelectParameters>
                                            <asp:ControlParameter Name="wniosekId" Type="Int32" ControlID="hidRequestId" PropertyName="Value" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    
                                    <br />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# !IsCustom() %>'>
                                <td class="col1">
                                    <asp:Literal ID="Literal4" runat="server" Text="Ilość pracowników do puli premiowej"></asp:Literal>
                                </td>
                                <td class="employeeCount">
                                    <asp:Label ID="lblEmployeeCount" runat="server" Text='<%# Eval("IloscPracownikow") %>' />
                                    <%--<asp:TextBox ID="tbEmployeeCount" runat="server" Text='<%# Eval("IloscPracownikow") %>'
                                        Visible='<%# IsEditable() %>' MaxLength="3" />
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbEmployeeCount"
                                        FilterType="Custom" ValidChars="0123456789" />--%>
<%--                                    <asp:Button ID="btnEdit" runat="server" Text="Edytuj" CssClass="control btnEdit"
                                        Visible='<%# !IsInEdit() && IsEditable() %>' OnCommand="EmployeeCountClick" CommandName="Edit" />
                                    <asp:Button ID="btnSave" runat="server" Text="Zapisz" CssClass="control" Visible='<%# IsInEdit() && IsEditable() %>'
                                        OnCommand="EmployeeCountClick" CommandName="Save" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Anuluj" CssClass="control" Visible='<%# IsInEdit() && IsEditable() %>'
                                        OnCommand="EmployeeCountClick" CommandName="Cancel" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal10" runat="server" Text="Data akceptacji kierownika"></asp:Literal>
                                </td>
                                <td class="">
                                    <asp:Label ID="Label9" runat="server" Text='<%# Eval("DataKierAcc") %>' ToolTip='<%# Eval("DataKierAccTooltip") %>' /><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal11" runat="server" Text="Data akceptacji zarządu"></asp:Literal>
                                </td>
                                <td class="">
                                    <asp:Label ID="Label10" runat="server" Text='<%# Eval("DataPrezAcc") %>' ToolTip='<%# Eval("DataPrezAccTooltip") %>' /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="tdWrapper" runat="server" visible='<%# IsCustom() %>'>
                        <table class="tbZoomPracownik">
                            <tr id="Tr2" runat="server">
                                <td class="col1">
                                    <asp:Literal ID="Literal12" runat="server" Text="Uwagi"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text='<%# Eval("UwagiWniosek") %>' Visible='<%# !IsCustomEditableNotHeader() %>' />
                                    <asp:TextBox ID="tbUwagiWniosek" runat="server" Text='<%# Eval("UwagiWniosek") %>' Visible='<%# IsCustomEditableNotHeader() %>'
                                        TextMode="MultiLine" Style="width: 300px;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="tdWrapper" runat="server" visible='<%# !IsCustom() %>' >
                        <table class="tbZoomPracownik col2">
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal3" runat="server" Text="Bilans otwarcia"></asp:Literal>
                                </td>
                                <td class="num">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("BilansOtwarcia") %>' /><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal5" runat="server" Text="Wyliczenie za bieżący okres"></asp:Literal>
                                </td>
                                <td class="num">
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Wyliczenie") %>' /><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal8" runat="server" Text="Pula premii uznaniowej"></asp:Literal>
                                </td>
                                <td class="num">
                                    <asp:HiddenField ID="hidPulaUznaniowa" runat="server" Visible="false" Value='<%# Eval("PulaUznaniowaHid") %>' />
                                    <asp:HiddenField ID="hidPulaZad" runat="server" Visible="false" Value='<%# Eval("PulaPremiiZad") %>' />
                                    <asp:Label ID="lblPulaPremiiUznaniowej" runat="server" Text='<%# Eval("PulaUznaniowa") %>' CssClass="pulaUznaniowa" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal9" runat="server" Text="Pula na kolejny okres"></asp:Literal>
                                </td>
                                <td class="num">
                                    <span class="premiaNaKolejny"></span><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    <asp:Literal ID="Literal6" runat="server" Text="Pula na pracownika"></asp:Literal>
                                </td>
                                <td class="num">
                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("Pula") %>' /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="tdWrapper" runat="server" visible='<%# IsTL(Eval("TL").ToString()) %>'>
                        <table class="tbZoomPracownik col3">
                            <tr>
                                <th>
                                    <asp:Label Id="lblTLName" runat="server" Text='<%# Eval("TLName") %>' />
                                </th>
                                <th>
                                    Podstawa
                                </th>
                                <th>
                                    Proporcjonalnie
                                </th>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Średniomiesięczna produktywność zespołowa
                                </td>
                                <td>
                                    <span class="prodPodst"></span>
                                </td>
                                <td>
                                    <span class="prodProp prodPremiaPodst"></span>
                                </td>
                            </tr>
<%--                            <tr>
                                <td class="col1">
                                    Premia za produktywność zespołową
                                </td>
                                <td>
                                    <span class="prodPremiaPodst" ></span>
                                </td>
                                <td>
                                    <span class="prodPremiaProp xwpzProp prodPremiaPodst" ></span>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="col1">
                                    Korekta o współczynnik jakości premii zespołowej
                                </td>
                                <td>
                                    <span class="qcKorektaPodst" ></span>
                                </td>
                                <td>
                                    <span class="qcKorektaProp" ></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Premia zespołowa (WPZ)
                                </td>
                                <td>
                                    <asp:Label ID="lblWPZ" runat="server" Text='<%# Eval("WPZ") %>' CssClass="wpzPodst" />
                                </td>
                                <td>
                                    <span class="wpzProp"></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Średniomiesięczna produktywność indywidualna
                                </td>
                                <td>
                                    <%--<span class="prodPodstInd" ></span>--%>
                                    <asp:Label ID="lblProdTL" runat="server" Text='<%# Eval("ProdTL") %>' CssClass="prodTl" />
                                </td>
                                <td>
                                    <%--<span class="prodPremiaPodstInd"></span>--%>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("PremiaTL") %>' data-value='<%# Eval("PremiaTL") %>' CssClass="prodPremiaPodstInd" />
                                
                                </td>
                            </tr>
<%--                            <tr>
                                <td class="col1">
                                    Premia za produktywność indywidualną
                                </td>
                                <td>
                                    <asp:Label ID="lblProdPremiaPodstInd" runat="server" Text='<%# Eval("PremiaTL") %>' CssClass="prodPremiaPodstInd" />
                                    <%--<span class="prodPremiaPodstInd" ></span>--%asd> 
                                </td>
                                <td>
                                    <span class="prodPremiaPropInd wpiProp"></span>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="col1">
                                    Premia indywidualna (WPI)
                                </td>
                                <td>
                                    <asp:Label ID="lblWPI" runat="server" Text='<%# Eval("WPI") %>' CssClass="wpiPodst" />
                                </td>
                                <td>
                                    <span class="wpiProp"></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Premia uznaniowa (zł)
                                </td>
                                <td class="premia_uzn_tl" data-value='<%# Eval("PremiaUznTL") %>' >
                                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("PremiaUznTL") %>' Visible='<%# !IsEditable() || IsTL(Eval("ISTL").ToString()) %>' />
                                    <asp:TextBox ID="tbPremiaUznTL" runat="server" Text='<%# Eval("PremiaUznTL") %>' Visible='<%# IsEditable() && !IsTL(Eval("ISTL").ToString()) %>' style="width: 95%;" MaxLength="4" />
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbPremiaUznTL" FilterType="Custom" ValidChars="0123456789" />
                                    <%--<span class="qcKorektaProp" ></span>--%>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("PremiaUznTLName") %>' Visible='<%# !IsEditable() || IsTL(Eval("ISTL").ToString()) %>' />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Premia zadaniowa (zł)
                                </td>
                                <td class="premiaZad">
                                    <%--<asp:Label ID="lblPremiaZad" runat="server" Text='<%# Eval("PremiaZad") %>' Visible='<%# !IsEditable() || IsTL(Eval("ISTL").ToString()) %>' />--%>
                                    <%--<asp:TextBox ID="tbPremiaZad" runat="server" Text='<%# Eval("PremiaZad") %>' Visible='<%# IsEditable() && !IsTL(Eval("ISTL").ToString()) %>' style="width: 95%;" MaxLength="4" />--%>
                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbPremiaZad" FilterType="Custom" ValidChars="0123456789" />--%>
                                    <%--<span class="qcKorektaProp" ></span>--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblPremiaZad2" runat="server" Text='<%# Eval("PremiaZadName") %>' Visible='<%# IsTL(Eval("ISTL").ToString()) %>' />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Kwartalny dodatek TL
                                </td>
                                <td class="quatro0td" runat="server" data-value='<%# Eval("Quatro0") %>'>
                                    <asp:DropDownList ID="ddlQuatro0" runat="server" DataSourceID="dsQuatro0" DataValueField="Id" DataTextField="Name" CssClass="ddlQuatro0" Visible='<%# IsEditable() && !IsTL(Eval("ISTL").ToString()) %>'  />
                                    <asp:SqlDataSource ID="dsQuatro0" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                        SelectCommand="
--declare @wniosekId int = 14
declare @typark int

select @typark = IdTypuArkuszy from scWnioski where Id = @wniosekId

select null as Id, 'wybierz ...' as Name, 1 as Sort
union all 
select convert(varchar, p.Parametr) + '|' + convert(varchar, p.Id) as Id, Nazwa as Name, 2 as Sort
from scSlowniki s 
left join scParametry p on p.Parametr2 = s.Id 
where s.Typ = 'QUATRO' and s.Nazwa2 = '0' and s.Aktywny = 1 and p.IdTypuArkuszy = @typark
order by Sort, Name
"
                                    >
                                    
                                        <SelectParameters>
                                            <asp:ControlParameter Name="wniosekId" Type="Int32" ControlID="hidRequestId" PropertyName="Value" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    
                                    <asp:Label ID="lblQuatro0Name" runat="server" Text='<%# Eval("Quatro0Name") %>' Visible='<%# !IsEditable() || IsTL(Eval("ISTL").ToString()) %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblQuatro0" runat="server"  CssClass="quatro0" />
                                    <%--<span class="wpiProp" ></span>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Kwartalny dodatek trenera
                                </td>
                                <td class="quatro1td" runat="server" data-value='<%# Eval("Quatro1") %>'>
                                    <asp:DropDownList ID="ddlQuatro1" runat="server" DataSourceID="dsQuatro1" DataValueField="Id" DataTextField="Name" CssClass="ddlQuatro1" Visible='<%# IsEditable() && !IsTL(Eval("ISTL").ToString()) %>' />
                                    <asp:SqlDataSource ID="dsQuatro1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                        SelectCommand="
--declare @wniosekId int = 14
declare @typark int

select @typark = IdTypuArkuszy from scWnioski where Id = @wniosekId

select null as Id, 'wybierz ...' as Name, 1 as Sort
union all 
select convert(varchar, p.Parametr) + '|' + convert(varchar, p.Id) as Id, Nazwa as Name, 2 as Sort
from scSlowniki s 
left join scParametry p on p.Parametr2 = s.Id 
where s.Typ = 'QUATRO' and s.Nazwa2 = '1' and s.Aktywny = 1 and p.IdTypuArkuszy = @typark
order by Sort, Name
"
                                    >
                                        <SelectParameters>
                                            <asp:ControlParameter Name="wniosekId" Type="Int32" ControlID="hidRequestId" PropertyName="Value" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    
                                    <asp:Label ID="lblQuatro1Name" runat="server" Text='<%# Eval("Quatro1Name") %>' Visible='<%# !IsEditable() || IsTL(Eval("ISTL").ToString()) %>' />
                                    
                                    <%--<asp:Label ID="Label7" runat="server" Text='<%# Eval("WPI") %>' CssClass="wpiPodst" />--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblQuatro1" runat="server"  CssClass="quatro1" />
                                </td>
                            </tr>
                            <tr>
                                <td class="col1">
                                    Razem Premia
                                </td>
                                <td></td>
                                <td >
                                    <span class="razemPremia" ></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        <asp:Label ID="NoDataLabel" runat="server" Text="No data" />                        
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="Table1" runat="server" class="tbRequestHeader">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server" colspan="2">
                        <table ID="itemPlaceholderContainer" runat="server" name="report">
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
</div>

<asp:SqlDataSource ID="dsHeader" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
--declare @wniosekId int = 8
declare @date datetime = (select Data from scWnioski where Id = @wniosekId)
declare @od as datetime = CONVERT(datetime, Convert(varchar, YEAR(@date)) + '-' + convert(varchar, MONTH(@date)) + '-01' )
declare @do as datetime = DATEADD(D, -1, DATEADD(M,1,@od))
declare @typark int
declare @ownark int

select @typark = IdTypuArkuszy, @ownark = IdPracownika from scWnioski where Id = @wniosekId

--declare @headcount  int = (select COUNT(*) from scPremie where _do is null and IdWniosku = @wniosekId and IdPracownika != @ownark and ISNULL(Czas, ISNULL(CzasPracy, 0)) &gt; 0 and IdPracownika &gt; 0)
declare @headcount2 int = (select COUNT(*) from scPremie where _do is null and IdWniosku = @wniosekId and IdPracownika != @ownark and ISNULL(Czas, ISNULL(CzasPracy, 0)) &gt; 0 and IdPracownika in
    (select IdPracownika from Przypisania where IdKierownika = @ownark and dbo.eom(@date) between Od and ISNULL(Do, '20990909')))

declare @pracId int = @ownark
declare @qc int
declare @prod int
declare @tl bit = 1
declare @WPI float
declare @WPZ float
declare @PREMIAZAD float

--zabij mnie
declare @a float
declare @b float
declare @c int
declare @d int

select @a = GodzProd, @b = CzasPracy, @c = IloscSztuk, @d = IloscBledow from scPremie p where p.IdWniosku = @wniosekId and p.IdPracownika &lt; 0

select @qc = QC, @prod = Produktywnosc from scTypyArkuszy where Id = @typark

select @prod = 10

select @WPI = Parametr from scParametry par
where par.Typ = 'WPI' and par.TL = 1 and par.IdTypuArkuszy = @typark and par.Parametr2 = @pracId and @date between par.Od and ISNULL(par.Do, '20990909')

select @WPZ = Parametr from scParametry par
where par.Typ = 'WPZ' and par.TL = 1 and par.IdTypuArkuszy = @typark and par.Parametr2 = @pracId and @date between par.Od and ISNULL(par.Do, '20990909')

select @PREMIAZAD = Parametr from scParametry par
where par.Typ = 'ZAD' and par.TL = 1 and par.IdTypuArkuszy = @typark and @date between par.Od and ISNULL(par.Do, '20990909')

declare @t1 as nvarchar(512), @t2 as nvarchar(512), @t3 as nvarchar(512)
select @t2 = COALESCE(@t2 + ';', '') + CONVERT(varchar, DlaIlu) + '|' + CONVERT(varchar, Ile) from scQC where IdTypuArkuszy = @typark and Rodzaj = @qc and (@date between Od and ISNULL(Do, '20990909')) and TL = @tl order by DlaIlu
select @t1 = COALESCE(@t1 + ';', '') + CONVERT(varchar, DlaIlu) + '|' + CONVERT(varchar, Ile) from scProduktywnosc where IdTypuArkuszy = @typark and Rodzaj = @prod and (@date between Od and ISNULL(Do, '20990909')) and OkresProbny = 0 and TL = @tl order by DlaIlu --TU CO INNEGO POTEM
select @t3 = COALESCE(@t3 + ';', '') + CONVERT(varchar, DlaIlu) + '|' + CONVERT(varchar, Ile) from scProduktywnosc where IdTypuArkuszy = @typark and Rodzaj = 9 and (@date between Od and ISNULL(Do, '20990909')) and OkresProbny = 0 and TL = @tl order by DlaIlu --TU CO INNEGO POTEM

declare @tlPremia float
declare @tlProd float

select 
@tlPremia = p.PremiaMiesieczna
, @tlProd = (p.GodzProd / p.CzasPracy) * 100
from scPremie p
left join scWnioski w on w.Id = p.IdWniosku
where IdWniosku = @wniosekId and _do is null and p.IdPracownika = w.IdPracownika

if ((select Rodzaj from scTypyArkuszy where Id = @typark) = 5) select @tlPremia = -1

select distinct w.Id
, LEFT(CONVERT(varchar, w.Data, 20), 7) Data
, w.Data as DataTooltip
, LEFT(CONVERT(varchar, w.DataWyplaty, 20), 7) DataWyplaty
, w.DataWyplaty as DataWyplatyTooltip
, convert(varchar, ISNULL(w.BilansOtwarcia, 0)) + ' zł' as BilansOtwarcia
, ISNULL(/*w.IloscPracownikow, oa.HeadCount*/@headcount2, 0) as IloscPracownikow
, convert(varchar, oaw.Wyliczenie) + ' zł' as Wyliczenie
, convert(varchar, p.Parametr) + ' zł' as Pula
, convert(varchar, oaw.Wyliczenie + ISNULL(w.BilansOtwarcia, 0)) + ' zł' as PulaUznaniowa 
, oaw.Wyliczenie + ISNULL(w.BilansOtwarcia, 0) as PulaUznaniowaHid 
, ISNULL(w.Nazwa, ta.Nazwa) as NazwaArkusza
, @t1 as Table1, @t2 as Table2, @t3 as Table3, @WPI as WPI, @WPZ as WPZ
, case when (isnull(@WPZ, 0) &gt; 0) or (isnull(@WPI, 0) &gt; 0) then 1 else 0 end as TL
, isnull(@a, -1) as GodzProd
, isnull(@b, -1) as CzasPracy
, isnull(@c, -1) as IloscSztuk
, isnull(@d, -1) as IloscBledow
, convert(varchar, pq0.Parametr) + '|' + convert(varchar, w.Quatro0) as Quatro0
, convert(varchar, pq1.Parametr) + '|' + convert(varchar, w.Quatro1) as Quatro1
, sq0.Nazwa as Quatro0Name
, sq1.Nazwa as Quatro1Name
, convert(varchar, round(@tlProd, 2)) + '%' as ProdTL
, convert(varchar, round(@tlPremia, 2)) + ' zł' as PremiaTL
, w.PremiaZadaniowa as PremiaZad
, convert(varchar, w.PremiaZadaniowa) + ' zł' as PremiaZadName
, case when @ownark = @observerId then 1 else 0 end as ISTL
, pr.Nazwisko + ' ' + pr.Imie + isnull(' (' + pr.KadryId + ')','') as TLName
, @PREMIAZAD as PulaPremiiZad
, w.PremiaUznaniowa as PremiaUznTL
, CONVERT(varchar, w.PremiaUznaniowa) + ' zł' as PremiaUznTLName
, LEFT(CONVERT(varchar, w.DataAkceptacjiK, 20), 10) DataKierAcc
, w.DataAkceptacjiK as DataKierAccTooltip
, LEFT(CONVERT(varchar, w.DataAkceptacjiP, 20), 10) DataPrezAcc
, w.DataAkceptacjiP as DataPrezAccTooltip
, w.Uwagi as UwagiWniosek
, w.DataWyplaty as DataWyplatyId
, w.Data as DataId
from scWnioski w
left join scParametry p on w.IdTypuArkuszy = p.IdTypuArkuszy and p.Typ = 'PREM' and w.Data between p.Od and ISNULL(p.Do, '20990909')
left join scParametry pq0 on pq0.Id = w.Quatro0
left join scParametry pq1 on pq1.Id = w.Quatro1
left join scTypyArkuszy ta on ta.Id = @typark
left join scSlowniki sq0 on sq0.Id = pq0.Parametr2 and sq0.Aktywny = 1
left join scSlowniki sq1 on sq1.Id = pq1.Parametr2 and sq1.Aktywny = 1
left join Pracownicy pr on pr.Id = w.IdPracownika
outer apply (select COUNT(*) as HeadCount from Przypisania p where p.IdCommodity = @typark and p.Od &lt;= @do and @od &lt;= ISNULL(p.Do, '20990909') and p.Status = 1 and p.IdkIerownika = w.IdPracownika) oa
outer apply (select ISNULL(/*w.IloscPracownikow, oa.HeadCount*/@headcount2, 0) * p.Parametr as Wyliczenie) oaw
where w.Id = @wniosekId


">
    <SelectParameters>
        <asp:ControlParameter Name="wniosekId" Type="Int32" ControlID="hidRequestId" PropertyName="Value" />
        <asp:ControlParameter Name="observerId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:Button ID="btnRecalculate" runat="server" OnClientClick="prepareRequestsSums();" style="display: none;" />

<asp:SqlDataSource ID="dsSave" runat="server" 
    SelectCommand="update scWnioski set IdPracownika = {6}, Nazwa = {0} /*, DataWyplaty =*/ {1}, Quatro0 = {3}, Quatro1 = {4}, PremiaZadaniowa = {5}, PremiaUznaniowa = {7}, Uwagi = {8} {9} where Id = {2}" />
