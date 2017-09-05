<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RcpControl.ascx.cs" Inherits="HRRcp.Controls.RcpControl" %>

<asp:HiddenField ID="hidSesId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<!--
<asp:HiddenField ID="hidAlgRcp" runat="server" />
<asp:HiddenField ID="hidAlgPar" runat="server" />
-->
<asp:HiddenField ID="_hidRcpId" runat="server" />
<asp:HiddenField ID="hidDateFrom" runat="server" />
<asp:HiddenField ID="hidDateTo" runat="server" />
<asp:HiddenField ID="hidDetails" runat="server" />
<asp:HiddenField ID="_hidStrefaId" runat="server" />
<asp:HiddenField ID="hidRound" runat="server" />
<asp:HiddenField ID="hidRoundType" runat="server" />

<table id="tbCzasPracyParams" runat="server" class="tbCzasPracyParams">
    <tr>
        <td id="tdStrefa" runat="server" class="col1">
            <span class="t1">Strefa:</span>
            <asp:DropDownList ID="ddlStrefa" runat="server" AutoPostBack="True" onselectedindexchanged="ddlStrefa_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td id="tdZaokr" runat="server" class="col2">
            <span class="t1">Zaokrąglenia:</span>
            <asp:DropDownList ID="ddlTimeRound" runat="server" AutoPostBack="True" onselectedindexchanged="ddlTimeRound_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td id="tdZaokrTyp" runat="server" class="col3">
            <span class="t1">Typ:</span>
            <asp:DropDownList ID="ddlRoundType" runat="server" AutoPostBack="True" onselectedindexchanged="ddlRoundType_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
</table>

<asp:Label ID="lbDebug" runat="server" Text="debug" Visible="false"/>

<asp:ListView ID="ListView1" runat="server" 
    DataSourceID="SqlDataSource1" ondatabinding="ListView1_DataBinding" 
    oninit="ListView1_Init" onlayoutcreated="ListView1_LayoutCreated" 
    onload="ListView1_Load" onitemdatabound="ListView1_ItemDataBound" 
    onitemcommand="ListView1_ItemCommand" 
    onselectedindexchanged="ListView1_SelectedIndexChanged" 
    onselectedindexchanging="ListView1_SelectedIndexChanging" 
    ondatabound="ListView1_DataBound">
    <ItemTemplate>
        <tr class="it"><td class="col1">
                <%--
                <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' />
                --%>
                <asp:LinkButton ID="DataLinkButton" runat="server" Text='<%# Eval("Data", "{0:d}") %>' CommandName="Select" ></asp:LinkButton>
                <asp:HiddenField ID="hidDetailsData" runat="server" />
            </td><td class="col2">
                <asp:Label ID="TimeInLabel" runat="server" Text='<%# Eval("TimeIn", "{0:T}") %>' ToolTip='<%# Eval("TimeIn", "{0:d}") %>'/>&nbsp;
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeIn" runat="server"></asp:PlaceHolder>
                </div>
            </td><td class="col3">
                <asp:Label ID="TimeOutLabel" runat="server" Text='<%# Eval("TimeOut", "{0:T}") %>' ToolTip='<%# Eval("TimeOut", "{0:d}") %>'/>&nbsp;
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeOut" runat="server"></asp:PlaceHolder>
                </div>
            </td><td class="col4" align="right">
                &nbsp;<asp:Label ID="Czas1RLabel" runat="server" Text='<%# Eval("Czas1R") %>' ToolTip='<%# Eval("Czas1") %>' />
            </td><td class="col5" align="right" runat="server" id="tdSumaWStrefie">
                &nbsp;<asp:Label ID="Czas2RLabel" runat="server" Text='<%# Eval("Czas2R") %>' ToolTip='<%# Eval("Czas2") %>' />
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phCzas1R" runat="server"></asp:PlaceHolder>
                </div>
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr style="" class="it"><td class="col1">
                <%--
                <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' />
                --%>
                <asp:LinkButton ID="DataLinkButton" runat="server" Text='<%# Eval("Data", "{0:d}") %>' CommandName="Unselect" ></asp:LinkButton>
            </td><td class="col2">
                <asp:Label ID="TimeInLabel" runat="server" Text='<%# Eval("TimeIn", "{0:T}") %>' ToolTip='<%# Eval("TimeIn", "{0:d}") %>'/>&nbsp;
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeIn" runat="server"></asp:PlaceHolder>
                </div>
            </td><td class="col3">
                <asp:Label ID="TimeOutLabel" runat="server" Text='<%# Eval("TimeOut", "{0:T}") %>' ToolTip='<%# Eval("TimeOut", "{0:d}") %>'/>&nbsp;
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeOut" runat="server"></asp:PlaceHolder>
                </div>
            </td><td class="col4" align="right">
                &nbsp;<asp:Label ID="Czas1RLabel" runat="server" Text='<%# Eval("Czas1R") %>' ToolTip='<%# Eval("Czas1") %>' />
            </td><td class="col5" align="right" runat="server" id="tdSumaWStrefie" >
                &nbsp;<asp:Label ID="Czas2RLabel" runat="server" Text='<%# Eval("Czas2R") %>' ToolTip='<%# Eval("Czas2") %>' />
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
        <table class="tbCzasPracy">
            <tr class="edt">
                <td>
                    Brak danych<br />
                    <asp:Label ID="lbNoDataInfo" CssClass="info" runat="server" Visible="false" EnableViewState="false" Text="Sprawdź ustawienia strefy, algorytmu i identyfikatora pracownika." />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbCzasPracy" runat="server" border="0" style="">
                        <tr class="itsumy">
                            <td colspan="3" align="right">Suma:</td>
                            <td align="right">
                                <asp:Label ID="lbSuma" runat="server" />
                            </td>
                            <td align="right" runat="server" id="thSumaWStrefieSuma" >
                                <asp:Label ID="lbSuma2" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" style="">
                            <th runat="server">Data</th>
                            <th runat="server">Wejście</th>
                            <th runat="server">Wyjście</th>
                            <th runat="server" class="col4">Czas pracy we&nbsp;-&nbsp;wy</th>
                            <th runat="server" class="col5" id="thSumaWStrefie"><span>Czas pracy<br />suma w strefie</span></th>
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
        <asp:ControlParameter ControlID="hidRound" Name="round" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidRoundType" Name="rtype" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidSesId" Name="sesId" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidDateFrom" Name="dFrom" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidDateTo" Name="dTo" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
