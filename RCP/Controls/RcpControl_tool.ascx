<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RcpControl_tool.ascx.cs" Inherits="HRRcp.Controls.RcpControl_tool" %>

<asp:HiddenField ID="hidSesId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidRcpId" runat="server" />
<asp:HiddenField ID="hidDateFrom" runat="server" />
<asp:HiddenField ID="hidDateTo" runat="server" />
<asp:HiddenField ID="hidDetails" runat="server" />

<table class="tbCzasPracyParams">
    <tr>
        <td class="col1">
            <span class="t1">Strefa:</span>
            <asp:DropDownList ID="ddlStrefa" runat="server" AutoPostBack="True" onselectedindexchanged="ddlStrefa_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td class="col2">
            <span class="t1">Zaokrąglenia:</span>
            <asp:DropDownList ID="ddlTimeRound" runat="server" AutoPostBack="True" onselectedindexchanged="ddlTimeRound_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
</table>
<asp:ListView ID="ListView1" runat="server" 
    DataSourceID="SqlDataSource1" ondatabinding="ListView1_DataBinding" 
    oninit="ListView1_Init" onlayoutcreated="ListView1_LayoutCreated" 
    onload="ListView1_Load" onitemdatabound="ListView1_ItemDataBound" 
    onitemcommand="ListView1_ItemCommand" 
    onselectedindexchanged="ListView1_SelectedIndexChanged">
    <ItemTemplate>
        <tr style="" class="it">
            <td>
                <%--
                <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' />
                --%>
                <asp:LinkButton ID="DataLinkButton" runat="server" Text='<%# Eval("Data", "{0:d}") %>' CommandName="Select" ></asp:LinkButton>
                <asp:HiddenField ID="hidDetailsData" runat="server" />
            </td>
            <td>
                <asp:Label ID="TimeInLabel" runat="server" Text='<%# Eval("TimeIn", "{0:T}") %>' ToolTip='<%# Eval("TimeIn", "{0:d}") %>'/>
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeIn" runat="server"></asp:PlaceHolder>
                </div>
            </td>
            <td>
                <asp:Label ID="TimeOutLabel" runat="server" Text='<%# Eval("TimeOut", "{0:T}") %>' ToolTip='<%# Eval("TimeOut", "{0:d}") %>'/>
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeOut" runat="server"></asp:PlaceHolder>
                </div>
            </td>
            <td align="right">
                <asp:Label ID="Czas1RLabel" runat="server" Text='<%# Eval("Czas1R") %>' ToolTip='<%# Eval("Czas1") %>' />
            </td>
            <td align="right">
                <asp:Label ID="Czas2RLabel" runat="server" Text='<%# Eval("Czas2R") %>' ToolTip='<%# Eval("Czas2") %>' />
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phCzas1R" runat="server"></asp:PlaceHolder>
                </div>
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr style="" class="it">
            <td>
                <%--
                <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' />
                --%>
                <asp:LinkButton ID="DataLinkButton" runat="server" Text='<%# Eval("Data", "{0:d}") %>' CommandName="Unselect" ></asp:LinkButton>
            </td>
            <td>
                <asp:Label ID="TimeInLabel" runat="server" Text='<%# Eval("TimeIn", "{0:T}") %>' ToolTip='<%# Eval("TimeIn", "{0:d}") %>'/>
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeIn" runat="server"></asp:PlaceHolder>
                </div>
            </td>
            <td>
                <asp:Label ID="TimeOutLabel" runat="server" Text='<%# Eval("TimeOut", "{0:T}") %>' ToolTip='<%# Eval("TimeOut", "{0:d}") %>'/>
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeOut" runat="server"></asp:PlaceHolder>
                </div>
            </td>
            <td align="right">
                <asp:Label ID="Czas1RLabel" runat="server" Text='<%# Eval("Czas1R") %>' ToolTip='<%# Eval("Czas1") %>' />
            </td>
            <td align="right">
                <asp:Label ID="Czas2RLabel" runat="server" Text='<%# Eval("Czas2R") %>' ToolTip='<%# Eval("Czas2") %>' />
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phCzas1R" runat="server"></asp:PlaceHolder>
                </div>
            </td>
        </tr>
        <tr class="it details">
            <asp:PlaceHolder ID="phEvents" runat="server"></asp:PlaceHolder>
        </tr>
    </SelectedItemTemplate>
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
        <table runat="server" class="ListView1 hoverline">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbCzasPracy" runat="server" border="0" style="">
                        <tr class="itxxx">
                            <td colspan="3" align="right">Suma [h]:</td>
                            <td align="right">
                                <asp:Label ID="lbSuma" runat="server" />
                            </td>
                            <td align="right">
                                <asp:Label ID="lbSuma2" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" style="">
                            <th runat="server">Data</th>
                            <th runat="server">Wejście</th>
                            <th runat="server">Wyjście</th>
                            <th runat="server">Czas pracy</th>
                            <th runat="server">Jako suma</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 
                        Data, 
                        TimeIn, 
                        TimeOut, 
                        dbo.ToTime(worktime) as Czas1, 
                        dbo.RoundTime(worktime, @round, @rtype) as Czas1R, 
                        dbo.ToTime(worktime2) as Czas2, 
                        dbo.RoundTime(worktime2, @round, @rtype) as Czas2R 
                    from tmpRCP3 where sesId = @sesId and Data between @dFrom and @dTo 
                    order by Data">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlTimeRound" Name="round" PropertyName="SelectedValue" Type="String" />
        <asp:ControlParameter ControlID="hidSesId" Name="sesId" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidDateFrom" Name="dFrom" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidDateTo" Name="dTo" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
