<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSpreadsheet.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntSpreadsheet" %>

<%--<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntScorecardSelect.ascx" TagPrefix="leet" TagName="ScorecardSelect" %>--%>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntScorecard.ascx" TagPrefix="leet" TagName="Scorecard" %>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntEmployeeSelect.ascx" TagPrefix="leet" TagName="EmployeeSelect" %>
<%@ Register src="~/Controls/PodzialLudzi/cntSelectRokMiesiac.ascx" tagname="cntSelectRokMiesiac" tagprefix="uc2" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div id="ctSpreadsheet" runat="server" class="cntSpreadsheet">
            <table class="caption">
                <tr>
                    <td class="left">
                        <span class="caption4">
                            <asp:Image ID="imgTitle" runat="server" alt="" ImageUrl="~/images/captions/layout_edit.png" />
                            <asp:Label ID="lblTitle" runat="server" />
                        </span>
                    </td>
                    <td class="middle">
                        <div id="EmployeeChanger" runat="server" class="employeeChanger">
                            <asp:Button ID="btnPreviousEmployee" runat="server" Text="◄" OnClick="EmployeeChangedLeft" />
                            <asp:Label ID="lblCurrentEmployee" runat="server" Text="Brak pracowników." />
                            <asp:Button ID="btnNextEmployee" runat="server" Text="►" OnClick="EmployeeChangedRight" />
                        </div>
                    </td>
                    <td class="right">
                        <uc2:cntselectrokmiesiac id="DateChanger" runat="server" canBackAll="false"
                            canNextAll="false" OnValueChanged="DateChanged" Quatro="true" />
                    </td>
                </tr>
            </table>
            <leet:EmployeeSelect ID="EmployeeSelect" runat="server" OnEmployeeChanged="EmployeeChanged"
                OnSuperiorChanged="SuperiorChanged" OnTeamChanged="TeamChanged" OnSaved="SaveScorecard"
                OnAccepted="AcceptScorecard" OnAcceptAlled="AcceptAllScorecards" Visible="true" OnUnAccepted="UnAccept" />
            <div class="pageContent">
                <leet:Scorecard ID="Scorecard" runat="server" InEdit="true" OnScAccepted="Accepted" OnTasksEmptySpr="TasksEmpty" />
                <div id="divTasksEmpty" runat="server" Visible="false">
                    <h3 style="margin-left: 16px;">Brak przypisanych zadań</h3>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>



<asp:SqlDataSource ID="dsTeamLeader" runat="server" SelectCommand="
declare @date datetime = {0}
declare @pracId int = {1}
declare @typark int = {2}

declare @WPI float

declare @sanity bit = case when (select COUNT(*) from Przypisania where IdCommodity = @typark and IdKierownika = @pracId and @date between Od and ISNULL(Do, '20990909')) = 0 then 0 else 1 end

select @WPI = Parametr from /*Przypisania p
left join*/ scParametry par /*on p.IdCommodity = par.IdTypuArkuszy and @date between par.Od and ISNULL(par.Do, '20990909')*/
where /*p.IdKierownika = @pracId and @date between p.Od and ISNULL(p.Do, '20990909') and p.Status = 1 and*/ par.Typ = 'WPI' and par.TL = 1 and par.IdTypuArkuszy = @typark and par.Parametr2 = @pracId and par.Parametr2 = @pracId and @date between par.Od and ISNULL(par.Do, '20990909')

select case when ISNULL(@WPI, 0) &gt; 0 and @sanity = 1 then 1 else 0 end
" />