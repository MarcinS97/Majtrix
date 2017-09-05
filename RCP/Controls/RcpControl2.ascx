<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RcpControl2.ascx.cs" Inherits="HRRcp.Controls.RcpControl2" %>

<asp:ListView ID="ListView1" runat="server" 
    ondatabinding="ListView1_DataBinding" 
    oninit="ListView1_Init" 
    onlayoutcreated="ListView1_LayoutCreated" 
    onload="ListView1_Load" 
    onitemdatabound="ListView1_ItemDataBound" 
    onitemcommand="ListView1_ItemCommand" 
    onselectedindexchanged="ListView1_SelectedIndexChanged" 
    onselectedindexchanging="ListView1_SelectedIndexChanging" 
    ondatabound="ListView1_DataBound">
    <ItemTemplate>
        <tr class="it">
            <td class="nazwisko">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Pracownik") %>' />
            </td>
            <td class="nrew">
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("KadryId") %>' />
            </td>
            <td class="col1">
                <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' />
                <%--
                <asp:LinkButton ID="DataLinkButton" runat="server" Text='<%# Eval("Data", "{0:d}") %>' CommandName="Select" ></asp:LinkButton>
                --%>
                <asp:HiddenField ID="hidDetailsData" runat="server" />
            </td><td class="col2">
                <asp:Label ID="TimeInLabel" runat="server" Text='<%# Eval("TimeIn", "{0:T}") %>' ToolTip='<%# Eval("TimeIn", "{0:d}") %>'/>&nbsp;
                <%--
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeIn" runat="server"></asp:PlaceHolder>
                </div>
                --%>
            </td><td class="col3">
                <asp:Label ID="TimeOutLabel" runat="server" Text='<%# Eval("TimeOut", "{0:T}") %>' ToolTip='<%# Eval("TimeOut", "{0:d}") %>'/>&nbsp;
                <%--
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phTimeOut" runat="server"></asp:PlaceHolder>
                </div>
                --%>
            </td><td class="col4" align="right">
                &nbsp;<asp:Label ID="Czas1RLabel" runat="server" Text='<%# Eval("Czas1R") %>' ToolTip='<%# Eval("Czas1") %>' />
            </td><td class="col5" align="right" runat="server" id="tdSumaWStrefie">
                &nbsp;<asp:Label ID="Czas2RLabel" runat="server" Text='<%# Eval("Czas2R") %>' ToolTip='<%# Eval("Czas2") %>' />
                <%--
                <div style="width: 100%; text-align: right;">
                    <asp:PlaceHolder ID="phCzas1R" runat="server"></asp:PlaceHolder>
                </div>
                --%>
            </td>
        </tr>
    </ItemTemplate>
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
                    <table ID="itemPlaceholderContainer" name="report" class="tbCzasPracy" runat="server" border="0" style="">
                        <%--
                        <tr class="itsumy">
                            <td colspan="5" align="right">Suma [h]:</td>
                            <td align="right">
                                <asp:Label ID="lbSuma" runat="server" />
                            </td>
                            <td align="right" runat="server" id="thSumaWStrefieSuma" >
                                <asp:Label ID="lbSuma2" runat="server" />
                            </td>
                        </tr>
                        --%>
                        <tr runat="server" style="">
                            <th runat="server">Pracownik</th>
                            <th runat="server">Nr ew.</th>
                            <th runat="server">Data</th>
                            <th runat="server">Wejście</th>
                            <th runat="server">Wyjście</th>
                            <th runat="server">Czas pracy we&nbsp;-&nbsp;wy</th>
                            <th runat="server" id="thSumaWStrefie">Czas pracy<br />suma w strefie</th>
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

