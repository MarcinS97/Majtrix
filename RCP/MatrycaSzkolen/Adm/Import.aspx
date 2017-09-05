<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Import" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntWyliczenie.ascx" TagPrefix="cc" TagName="cntWyliczenie" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntImport.ascx" TagPrefix="cc" TagName="cntImport" %>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="cc" TagName="cntReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <uc2:PageTitle ID="PageTitle1" runat="server" Title="Import" SubText1="Import danych"  />
    <div class="pgKartaZgloszenie center960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <h3>Import danych</h3>
                <hr />
                <cc:cntImport ID="cImport" runat="server" />
                <br /><br />
                <h3>Wyliczenie ocen</h3>
                <hr />
                <cc:cntWyliczenie id="cWyliczenie" runat="server" />
                <br /><br />
                <h3>Nieprzypisanie linie</h3>
                <hr />
                <center><cc:cntReport ID="cRaport" runat="server" CssClass="GridView1" SQL="
select
  ROW_NUMBER() over (order by l.Nazwa) - 1 No
, l.Nazwa
into #aaa
from msLinie l
where l.Id not in (select IdLinii from msLinieStanowiska) and l.Aktywny = 1

declare @max int = (select MAX(No) from #aaa)

;with enum as
(
	select 0 a
	union all
	select a + 5 from enum where a &lt;= @max - 5
)
select
  (select Nazwa from #aaa where No = enum.a + 0) [Linie]
, (select Nazwa from #aaa where No = enum.a + 1) [ ]
, (select Nazwa from #aaa where No = enum.a + 2) [  ]
, (select Nazwa from #aaa where No = enum.a + 3) [   ]
, (select Nazwa from #aaa where No = enum.a + 4) [    ]
from enum

drop table #aaa

"/></center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="cImport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
