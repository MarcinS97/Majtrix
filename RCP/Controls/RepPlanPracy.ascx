<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepPlanPracy.ascx.cs" Inherits="HRRcp.Controls.RepPlanPracy" %>
<%@ Register src="PlanPracyLineHeader.ascx" tagname="PlanPracyLineHeader" tagprefix="uc1" %>
<%@ Register src="PlanPracyLine2.ascx" tagname="PlanPracyLine2" tagprefix="uc1" %>
<%@ Register src="RepPracInfo.ascx" tagname="RepPracInfo" tagprefix="uc1" %>

<asp:HiddenField ID="hidRok" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />

<asp:ListView ID="lvPlanPracy" runat="server" 
    ondatabound="lvPlanPracy_DataBound" 
    onitemdatabound="lvPlanPracy_ItemDataBound" 
    onunload="lvPlanPracy_Unload" 
    ondatabinding="lvPlanPracy_DataBinding" 
    onitemcreated="lvPlanPracy_ItemCreated" 
    onlayoutcreated="lvPlanPracy_LayoutCreated" 
    onitemcommand="lvPlanPracy_ItemCommand" 
    onprerender="lvPlanPracy_PreRender" DataSourceID="SqlDataSource1" DataKeyNames="Month">
    <ItemTemplate>
        <tr class="it">
            <td id="tdMonth" class="month" runat="server">
                <%--
                <asp:Label ID="MonthLabel" runat="server" Text='<%# (Int32)Container.DataItem %>' />
                --%>
                <asp:Label ID="MonthLabel" runat="server" />
            </td>
            <uc1:PlanPracyLine2 ID="PlanPracyLine" runat="server" />                            
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1" id="lvOuterTable">
            <tr runat="server">
                <td colspan="2" runat="server">
                    <table id="itemPlaceholderContainer" class="tbPlanPracy tbKartaRoczna" name="report4">  <%-- runat="server" --%>
                        <tr>
                            <th id="thHeader1" colspan="32" runat="server" class="header">
                                <uc1:RepPracInfo ID="cntRepPracInfo" runat="server" />
                            </th>
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Czas nominalny</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Godz. przepracowane</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Nadgodziny 50</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Nadgodziny 100</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Niedziele i święta</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Nocne</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Dyżury</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Wolne za nadgodziny</span></div></th>                            
                        </tr>
                        <tr>
                            <th class="month">Miesiąc</th>
                            <uc1:PlanPracyLineHeader ID="PlanPracyLineHeader" Mode="2" runat="server" />
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <tr class="it">
                            <td id="tdSumy" runat="server" colspan="32" class="right">Sumy:</td>
                            
                            <td id="td16" class="suma" runat="server">
                                <asp:Label ID="lbSumNominalny" runat="server" />
                            </td>
                            <td id="td14" class="suma" runat="server">
                                <asp:Label ID="lbSumSumaryczny" runat="server" />
                            </td>
                            <td id="td5" class="suma" runat="server">
                                <asp:Label ID="lbSumNadg50" runat="server" />
                            </td>
                            <td id="td6" class="suma" runat="server">
                                <asp:Label ID="lbSumNadg100" runat="server" />
                            </td>
                            <td id="td3" class="suma" runat="server">
                                <asp:Label ID="lbSumNiedzieleSwieta" runat="server" />
                            </td>
                            <td id="td18" class="suma" runat="server">
                                <asp:Label ID="lbSumNocny" runat="server" />
                            </td>
                            <td id="td1" class="suma" runat="server">
                                <asp:Label ID="lbSumDyzury" runat="server" Text="0"/>
                            </td>
                            <td id="td2" class="suma" runat="server">
                                <asp:Label ID="lbSumZaNadg" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:Button ID="btSelectCell" CssClass="button_postback" runat="server" Text="Select" onclick="btSelectCell_Click" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select DAY(D.Data) as Month, R.Id, 
ISNULL(R.DataOd, DATEADD(MONTH, DAY(D.Data)-1, DATEADD(YEAR, @year-1900, 0))) as OkresOd, 
ISNULL(R.DataDo, DATEADD(DAY, -1, DATEADD(MONTH, DAY(D.Data), DATEADD(YEAR, @year-1900, 0)))) as OkresDo
from dbo.GetDates2('20120101','20120112') D
left outer join OkresyRozl R on R.DataDo between 
DATEADD(MONTH, DAY(D.Data)-1, DATEADD(YEAR, @year-1900, 0)) and 
DATEADD(DAY, -1, DATEADD(MONTH, DAY(D.Data), DATEADD(YEAR, @year-1900, 0)))">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidRok" Name="year" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
