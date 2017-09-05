<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RcpAnalizeControl.ascx.cs" Inherits="HRRcp.Controls.RcpAnalizeControl" %>

<asp:ListView ID="ListView1" runat="server" >
    <ItemTemplate>
        <tr class="it typ<%# Eval("Typ") %>">
            <td class="col1">
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="od">
                <asp:Label ID="TimeOdLabel" runat="server" Text='<%# Eval("TimeOd", "{0:T}") %>' ToolTip='<%# Eval("TimeOd", "{0:d}") %>'/>
            </td>
            <td class="do">
                <asp:Label ID="TimeDoLabel" runat="server" Text='<%# Eval("TimeDo", "{0:T}") %>' ToolTip='<%# Eval("TimeDo", "{0:d}") %>'/>
            </td>
            <td class="czas">
                <asp:Label ID="CzasLabel" runat="server" Text='<%# Eval("Czas") %>' />
            </td>
            <td class="nocne">
                <asp:Label ID="NocneLabel" runat="server" Text='<%# Eval("Nocne") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table class="tbRcpAnalize_edt">
            <tr>
                <td>
                    Brak zarejestrowanego czasu pracy
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbRcpAnalize" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">Składnik</th>
                            <th runat="server">Czas od</th>
                            <th runat="server">Czas do</th>
                            <th runat="server">Czas</th>
                            <th runat="server" class="noc">W tym w nocy</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <tr class="suma">
                            <td colspan="3" class="label1">Łączny czas pracy:</td>
                            <td class="czas">
                                <asp:Label ID="SumCzasPrac" runat="server" />
                            </td>
                            <td class="noc">
                                <asp:Label ID="SumNoc" runat="server" />
                            </td>
                        </tr>
                        <tr class="suma" id="trPrzerwa1" runat="server">
                            <td colspan="3" class="label1">Łączny czas przerw:</td>
                            <td class="czas">
                                <asp:Label ID="SumPrzerwLabel" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr class="suma" id="trPrzerwa2" runat="server">
                            <td colspan="3" class="label1">Wliczony czas przerw:</td>
                            <td class="czas">
                                <asp:Label ID="SumPrzerwNomLabel" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr class="suma" id="trAbsGodz" runat="server" visible="false">
                            <td colspan="3" class="label1"><asp:Literal ID="ltAbsGodzNazwa" runat="server"></asp:Literal>:</td>
                            <td class="czas">
                                <asp:Label ID="lbAbsGodz" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr class="suma">
                            <td colspan="3" class="label1">Nadgodziny '50:</td>
                            <td class="czas">
                                <asp:Label ID="lbNadg50" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr class="suma nadg50">
                            <td colspan="3" class="label1">Nadgodziny '100:</td>
                            <td class="czas">
                                <asp:Label ID="lbNadg100" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>


<%--
                        <tr class="suma suma_firstline">
                            <td colspan="2">Czas pracy na zmianie</td>
                            <td align="right">
                                <asp:Label ID="SumCzasLabel" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr class="suma">
                            <td colspan="2">Czas pracy na zmianie</td>
                            <td align="right">
                                <asp:Label ID="Label1" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        
                        
                "praca na zmianie",
                "przerwa zmianowa wliczona",
                "przerwa zmianowa niewliczona",
                "nadgodziny",
                "przerwa w czasie nadgodzin wliczona",
                "przerwa w czasie nadgodzin niewliczona"};
--%>