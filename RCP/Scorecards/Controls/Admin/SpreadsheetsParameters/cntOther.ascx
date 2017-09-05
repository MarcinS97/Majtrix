<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOther.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.SpreadsheetsParameters.cntOther" %>

<%@ Register Src="~/Scorecards/Controls/Admin/SpreadsheetsParameters/cntOtherList.ascx" TagPrefix="leet" TagName="OtherList" %>

<asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />

<div id="ctOther" runat="server" class="cntOther">
    <leet:OtherList Id="OtherListPGIO" runat="server" Type="PGIO" Title="PGIO" ValidationGroup="PGIO" Percent="0" Unit="zł" Employee="false" Subtitle="Premia za pracę na rzecz innego arkusza" />
    <leet:OtherList Id="OtherListPREM" runat="server" Type="PREM" Title="Pula premiowa na osobę" ValidationGroup="PREM" Percent="0" Unit="zł" Employee="false" Subtitle="Pula premiowa przeznaczona na osobę na wniosku premiowym" />
    <leet:OtherList ID="OtherListPREMZAD" runat="server" Type="ZAD" Title="Premia zadaniowa" ValidationGroup="PREMZAD" Percent="0" Unit="zł" Employee="false" Subtitle="Pula zadaniaowa dla TL" />
    <leet:OtherList Id="OtherListSANDWHICH" runat="server" Type="SANDWICH" Title="Długość przerwy" ValidationGroup="SANDWICH" Percent="0" Unit="h" Employee="false" Subtitle="Długość przerwy" />
    <leet:OtherList Id="OtherListPROD" runat="server" Type="PROD" Title="Cel produktywności" ValidationGroup="PROD" Percent="1" Unit="%" Employee="false" Subtitle="Dzienny cel produktywności"  />
    <leet:OtherList Id="OtherListQC" runat="server" Type="QC" Title="Cel jakościowy" ValidationGroup="OQC" Percent="1" Unit="%" Employee="false" Subtitle="Dzienny cel jakości" />
    <leet:OtherList Id="OtherListWPI" runat="server" Type="WPI" Title="WPI" ValidationGroup="WPI" Percent="1" Unit="%" Employee="true" Subtitle="Współczynnik premii indywidualnej" />
    <leet:OtherList Id="OtherListWPZ" runat="server" Type="WPZ" Title="WPZ" ValidationGroup="WPZ" Percent="1" Unit="%" Employee="true" Subtitle="Współczynnik premii zbiorowej" />
    <leet:OtherList Id="OtherListQUATRO" runat="server" Type="QUATRO" Title="Dodatek kwartalny" ValidationGroup="QUATRO" Percent="0" Unit="zł" Employee="false" Subtitle="Dodatek kwartalny dla Team Leadera, Young Team Leadera lub Trenera" 
                            DropDownListQuery="select null as Id, 'wybierz ...' as Name, 0 as Sort 
union all select Id, Nazwa as Name, 1 as Sort from scSlowniki where Typ = 'QUATRO' and Aktywny = 1 order by Sort, Name" Parametr2Label="Dodatek"  />
    <leet:OtherList Id="OtherListTLPRAC" runat="server" Type="TLPRAC" Title="Praca Team Leader'a" ValidationGroup="TLPRAC" Percent="0" Unit="h" Employee="true" Subtitle="Ilość czasu przepracowanego jako Team Leader" />
</div>