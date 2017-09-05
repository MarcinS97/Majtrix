<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTasks.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntTasks1" %>

<%--<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
<div id="ctTasks" runat="server" class="cntTasks cnt">
    <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
    <asp:HiddenField ID="hidOutsideJob" runat="server" Visible="false" />
<%--    <asp:Repeater ID="rpTasks" runat="server" DataSourceID="dsTasks">
        <ItemTemplate>
            <div class="task hid2" data-qc='<%# Eval("QC") %>'>
                <div class="name">
                    <asp:Label ID="lblName" CssClass="bunker" runat="server" Text='<%# Eval("Nazwa") %>' />
                    <asp:Image ID="imgStar" CssClass="qcStar" runat="server" ToolTip="Czynność brana pod uwagę w QC"
                        ImageUrl="~/Scorecards/images/QC.png" Visible='<%# (bool)Eval("QC") %>' />
                </div>
                <div class="cc">
                    <asp:Label ID="lblCC" runat="server" Text='<%# Eval("cc") %>' />
                </div>
                <div class="time">
                    <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Czas") %>' CssClass="timeLabel" />
                </div>
                <div class="sum">
                    <span class="iSum"></span>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>--%>
    
    
    
    <table class="tbTasks task hid2">
        <tr>
            <asp:Repeater ID="rpNames" runat="server">
                <ItemTemplate>
                    <td class="name" data-qc='<%# Eval("QC") %>' >
                        <asp:Label ID="lblName" CssClass="bunker" runat="server" Text='<%# Eval("Nazwa") %>' />
                        <asp:Image ID="imgStar" CssClass="qcStar" runat="server" ToolTip="Czynność brana pod uwagę w QC" ImageUrl="~/Scorecards/images/QC.png" Visible='<%# (bool)Eval("QC") %>' />
                    </td>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
        <tr>
            <asp:Repeater ID="rpCC" runat="server">
                <ItemTemplate>
                    <td class="cc">
                        <asp:Label ID="lblCC" runat="server" Text='<%# Eval("cc") %>' ToolTip='<%# Eval("NazwaCC") %>' />
                    </td>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
        <tr>
            <asp:Repeater ID="rpTime" runat="server">
                <ItemTemplate>
                    <td class="time">
                        <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Czas") %>' ToolTip="Czas" CssClass="timeLabel" />
                    </td>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
        <tr class="sum">
            <asp:Repeater ID="rpSum" runat="server">
                <ItemTemplate>
                    <td class="sum">
                        <span class="iSum"></span>
                    </td>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
    </table>
    
    
    
    
    
    
    
    
    
    
    
    
    <asp:SqlDataSource ID="dsTasks" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" OnSelected="dsTasks_Selected"
        SelectCommand="
declare @od as datetime = CONVERT(datetime, Convert(varchar, YEAR(@date)) + '-' + convert(varchar, MONTH(@date)) + '-01' )
declare @do as datetime = DATEADD(D, -1, DATEADD(M,1,@od))
select cc.cc, c.Nazwa, c.Czas, tac.QC, 'CC - ' + cc.Nazwa as NazwaCC
from scTypyArkuszyCzynnosci tac
left join scCzynnosci c on c.Id = tac.IdCzynnosci
left join CC cc on cc.Id = c.IdCC
where tac.IdTypuArkuszy = @typark and tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, '20990909')
order by c.Nazwa
">
        <SelectParameters>
            <asp:ControlParameter Name="typark" ControlID="hidScorecardTypeId" PropertyName="Value"
                Type="String" />
            <asp:ControlParameter Name="date" ControlID="hidDate" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>