<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntLog.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.cntLog" %>

<div class="cntLog">

<asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="5000" ontick="Timer1_Tick">
</asp:Timer>

<asp:ListView ID="lvEvents" runat="server" DataKeyNames="Id" 
    DataSourceID="dsLog" 
    onitemdatabound="lvEvents_ItemDataBound">
    <ItemTemplate>
        <tr class="it" style="">
            <td class="col1">
                <asp:Label ID="DataCzasLabel" runat="server" Text='<%# Eval("DataCzas", "{0:d}") %>' /><br />
                <asp:Label ID="Label1" runat="server" CssClass="line2" Text='<%# Eval("DataCzas", "{0:T}") %>' /><br />
                <asp:Label ID="LoginLabel" CssClass="login" runat="server" Text='<%# Eval("Login") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td class="col3">
                <asp:Label ID="Typ2Label" runat="server" Text='<%# Eval("Typ2") %>' />
            </td>
            <td class="col4">
                <asp:Label ID="ParLabel" runat="server" Text='<%# Eval("Par") %>' /><br />
                <asp:Label ID="KodLabel" runat="server" Text='<%# Eval("Kod") %>' />
            </td>
            <td class="col5">
                <asp:TextBox class="textbox noborder" ID="InfoLabel" runat="server" Text='<%# Eval("Info") %>' TextMode="MultiLine" Rows="3" ReadOnly="True" />
            </td>
            <td class="col6">
                <asp:TextBox class="textbox noborder" ID="Info2TextBox" runat="server" Text='<%# Eval("Info2") %>' TextMode="MultiLine" Rows="3" ReadOnly="True" />
            </td>
            <td class="col7">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table3" class="edt" runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div id="divLog" runat="server">
            <table id="Table4" class="xListView1 xhoverline tbLog" runat="server" style="">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server" class="pager pager_top" >
                        <asp:DataPager ID="DataPager2" runat="server" PageSize="15">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="«" PreviousPageText="‹" />
                                  <asp:NumericPagerField ButtonType="Link" />
                                  <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="›" LastPageText="»" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                    <td class="xtbLogRefresh" style="text-align: right;">
                        <span>
                            <asp:CheckBox ID="cbAutoRefresh" runat="server" oncheckedchanged="cbAutoRefresh_CheckedChanged" AutoPostBack="True" />
                            Odświeżaj
                        </span>
                    </td>
                </tr>
                <tr id="Tr4" runat="server">
                    <td id="Td3" colspan="2" runat="server">
                        <table class="tbLog table table-bordered" ID="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr id="Tr5" runat="server" style="">
                                <th id="Th8" class="col1" runat="server">Czas/Login</th>
                                <th id="Th9" class="col2" runat="server">Typ</th>
                                <th id="Th10" class="col3" runat="server">Typ2</th>
                                <th id="Th11" class="col4" runat="server">Par/Kod</th>
                                <th id="Th12" class="col5" runat="server">Info</th>
                                <th id="Th13" class="col6" runat="server">Info2</th>
                                <th id="Th14" class="col7" runat="server">Status</th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr6" runat="server">
                    <td id="Td2" runat="server" class="pager" >
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="«" PreviousPageText="‹" />
                                <asp:NumericPagerField ButtonType="Link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="›" LastPageText="»" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                    <td align="right">
                    </td>
                </tr>
            </table>
        </div>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsLog" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT * FROM [Log] ORDER BY [Id] DESC" >
</asp:SqlDataSource>

</div>