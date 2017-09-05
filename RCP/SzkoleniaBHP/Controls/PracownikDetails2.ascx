<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PracownikDetails2.ascx.cs" Inherits="HRRcp.SzkoleniaBHP.Controls.PracownikDetails2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:HiddenField ID="hidPracId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidData" runat="server" Visible="false"/>

<div class="PracownikDetails">
    <asp:ListView ID="lvPracownik" runat="server" DataKeyNames="Id_Pracownicy" DataSourceID="SqlDataSource1" 
        InsertItemPosition="None" 
        onlayoutcreated="lvPracownicy_LayoutCreated" 
        onitemcommand="lvPracownicy_ItemCommand" 
        onitemdatabound="lvPracownicy_ItemDataBound" 
        >
        <ItemTemplate>
            <tr runat="server">
                <td class="col1">
                    <asp:Literal ID="Literal1" runat="server" Text="Pracownik:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="NazwiskoLabel" runat="server" Text='<%# Eval("NazwiskoImie") %>' /><br />
                </td>
            </tr>
            <tr runat="server">
                <td class="col1">                
                    <asp:Literal ID="Literal2" runat="server" Text="Nr ewidencyjny:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Nr_Ewid") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">APT:
                </td>
                <td id="td3" runat="server">
                    <asp:CheckBox ID="cbAPT" runat="server" CssClass="check" Enabled="false" Checked='<%# Eval("APT") %>'/>                
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal3" runat="server" Text="Stanowisko:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="lbStanowisko" runat="server" Text='<%# Eval("Nazwa_Stan") %>' /><br />
                </td>
            </tr>                
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal4" runat="server" Text="Umowa:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Rodzaj_Umowy") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal5" runat="server" Text="Data zatrudnienia:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("DataZatr", "{0:d}") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal6" runat="server" Text="Data obowiązywania umowy:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("DataUmDo", "{0:d}") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal7" runat="server" Text="Jedn. macierzysta:"></asp:Literal>
                    <br />
                    <span class="help">
                    <asp:Literal ID="Literal8" runat="server" Text="Obszar/Podobszar/Kierownik"></asp:Literal>
                    </span>
<%--
                    <span class="help">Strumień/Linia/Kierownik</span>
--%>
                </td>                    
                <td id="tdJednM" runat="server">
                    <asp:Label ID="lbStrumienM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaM") %>' ToolTip='<%# Eval("NazwaStrumieniaM") %>' /> / 
                    <asp:Label ID="lbLiniaM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiM") %>' ToolTip='<%# Eval("NazwaLiniiM") %>' /><br />
                    <asp:Label ID="lbKierownikM" runat="server" class="line2" Text='<%# Eval("KierownikM") %>' />                
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal9" runat="server" Text="Jedn. aktualna:"></asp:Literal>
                    <br />
                    <span class="help">
                    <asp:Literal ID="Literal10" runat="server" Text="Obszar/Podobszar/Kierownik"></asp:Literal>
                    </span>
<%--                    <span class="help">Strumień/Linia/Kierownik</span>
--%>                </td>
                <td id="tdJednA" runat="server">
                    <asp:Label ID="lbStrumienA" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaA") %>' ToolTip='<%# Eval("NazwaStrumieniaA") %>' /> / 
                    <asp:Label ID="lbLiniaA" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiA") %>' ToolTip='<%# Eval("NazwaLiniiA") %>' /><br />
                    <asp:Label ID="lbKierownikA" runat="server" class="line2" Text='<%# Eval("KierownikA") %>' />                
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal11" runat="server" Text="Czas oddelegowania:"></asp:Literal>
                </td>
                <td id="tdJednAod" runat="server">
                    <asp:Label ID="lbOd" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /> -
                    <asp:Label ID="lbDo" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
                </td>
            </tr>
            <%--
            <tr>
                <td class="col1">Ocena łączna</td>
                <td id="tdRating" runat="server">
                    <asp:Rating ID="Rating1" runat="server" ReadOnly="true"
                        CurrentRating="4"
                        MaxRating="5"
                        StarCssClass="rating"
                        FilledStarCssClass="rating_filled"
                        EmptyStarCssClass="rating_empty"
                        WaitingStarCssClass="rating_wating"
                        style="float: left;"
                        ></asp:Rating>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbOcena" runat="server" Text='<%# Eval("Ocena") %>' />
                </td>
            </tr>
            --%>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal12" runat="server" Text="Ocena:"></asp:Literal>
                </td>
                <td id="td2" runat="server">
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Ocena") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal13" runat="server" Text="Status:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <asp:Literal ID="Literal14" runat="server" Text="Data zwolnienia:"></asp:Literal>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("DataZwol", "{0:d}") %>' />
                </td>
            </tr>
            
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        <asp:Label ID="NoDataLabel" runat="server" Text="No data" />                        
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>

        <LayoutTemplate>
            <table id="Table1" runat="server" class="ListView1 tbZoomPracownik">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server" colspan="2">
                        <table ID="itemPlaceholderContainer" runat="server" name="report">
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>

    <div class="buttons">
        <asp:Button ID="btZoomClose" runat="server" CssClass="button75" Text="Zamknij" />
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
