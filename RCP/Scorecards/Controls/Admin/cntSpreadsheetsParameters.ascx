<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSpreadsheetsParameters.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntSpreadsheetsParameters" %>

<%@ Register Src="~/Scorecards/Controls/Admin/SpreadsheetsParameters/cntProductivity.ascx" TagPrefix="leet" TagName="Productivity" %>
<%@ Register Src="~/Scorecards/Controls/Admin/SpreadsheetsParameters/cntQC.ascx" TagPrefix="leet" TagName="QC" %>
<%@ Register Src="~/Scorecards/Controls/Admin/SpreadsheetsParameters/cntAbsence.ascx" TagPrefix="leet" TagName="Absence" %>
<%@ Register Src="~/Scorecards/Controls/Admin/SpreadsheetsParameters/cntOther.ascx" TagPrefix="leet" TagName="Other" %>


<div id="ctSpreadsheetsParameters" runat="server" class="cntSpreadsheetsParameters">

    <asp:DropDownList ID="ddlScorecards" runat="server" DataSourceID="dsScorecards" DataValueField="Id" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlScorecards_SelectedIndexChanged" CssClass="ddl" />
    <asp:SqlDataSource ID="dsScorecards" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
select null as Id, 'Wybierz arkusz...' as Name, 0 as Sort
union all
select Id, Nazwa as Name, 1 as Sort from scTypyArkuszy
order by Sort
" />



<div id="divImport" runat="server" visible="false" class="import">
    <span>Zaimportuj z innego arkusza: </span>

<asp:DropDownList ID="ddlScorecardsImport" runat="server" DataSourceID="dsScorecardsImport" DataValueField="Id" DataTextField="Name"  />
    <asp:SqlDataSource ID="dsScorecardsImport" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
select null as Id, 'Wybierz arkusz...' as Name, 0 as Sort
union all
select Id, Nazwa as Name, 1 as Sort from scTypyArkuszy where Id != @SelId
order by Sort
" >
    <SelectParameters>
        <asp:ControlParameter Name="SelId" Type="Int32" ControlID="ddlScorecards" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>
    <asp:Button ID="btnImport" runat="server" Text="Importuj" OnClick="Import" CssClass="button100" />
</div>

<asp:SqlDataSource id="dsImport" runat="server" SelectCommand=
"
declare @to int = {0}
declare @from int = {1}

insert into scProduktywnosc select @to , DlaIlu, Ile, OkresProbny, Rodzaj, Od, Do, TL from scProduktywnosc where IdTypuArkuszy = @from and GETDATE() between Od and isnull(Do, '20990909')--Do is null
insert into scQC select @to , DlaIlu, Ile, Rodzaj, Od, Do, TL from scQC where IdTypuArkuszy = @from and GETDATE() between Od and isnull(Do, '20990909')
insert into scAbsencje select @to , DlaIlu, Ile, Od, Do, TL from scAbsencje where IdTypuArkuszy = @from and GETDATE() between Od and isnull(Do, '20990909')
insert into scParametry select Typ, Parametr, Parametr2, Od, Do, @to, TL from scParametry where IdTypuArkuszy = @from and GETDATE() between Od and isnull(Do, '20990909')
"/>



                    <asp:Menu ID="tabType" runat="server" Orientation="Horizontal"  
                        onmenuitemclick="tabType_MenuItemClick" Visible="false" >
                        <StaticMenuStyle CssClass="tabsStrip" />
                        <StaticMenuItemStyle CssClass="tabItem" />
                        <StaticSelectedStyle CssClass="tabSelected" />
                        <StaticHoverStyle CssClass="tabHover" />
                        <Items>
                            <asp:MenuItem Text="Arkusz" Value="0" Selected="true"></asp:MenuItem>
                            <asp:MenuItem Text="Team Leader" Value="1"></asp:MenuItem>
                        </Items>
                        <StaticItemTemplate>
                            <div class="tabCaption">
                                <div class="tabLeft">
                                    <div class="tabRight">
                                        <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                                    </div>
                                </div>
                            </div>
                        </StaticItemTemplate>
                    </asp:Menu>
        <div id="paramWrapper">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <leet:Productivity ID="Productivity" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <leet:QC ID="QC" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <leet:Absence ID="Absence" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <leet:Other ID="Other" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</div>