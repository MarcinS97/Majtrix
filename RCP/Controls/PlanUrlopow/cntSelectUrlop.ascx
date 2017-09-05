<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSelectUrlop.ascx.cs" Inherits="HRRcp.Controls.cntSelectUrlop" %>

<asp:HiddenField ID="hidEditMode" runat="server" />
<asp:HiddenField ID="hidMode" runat="server" />
<asp:HiddenField ID="hidPlanParams" runat="server" />

<asp:ListView ID="lvZmiany" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Kod" 
    onselectedindexchanged="lvZmiany_SelectedIndexChanged" 
    ondatabound="lvZmiany_DataBound" 
    onitemdatabound="lvZmiany_ItemDataBound" 
    onitemcommand="lvZmiany_ItemCommand" >
    <ItemTemplate>
        <asp:Literal ID="ltNewLine" runat="server" Visible="false"><br /></asp:Literal>
        <span id="spanZmiana" class="zmiana it round5" runat="server">        
            <table class="zmiana">
                <tr>
                    <td class="col1">
                        <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol absencji" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("KolorPU").ToString()) %>' />
                    </td>
                    <td class="col2">
                        <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# FirstUpper(Eval("Nazwa")) %>' /><br />
                        <asp:Label ID="lbOpis" runat="server" CssClass="opis" Text='<%# Eval("Opis") %>' />
                    </td>
                </tr>
                <tr class="control">
                    <td colspan="2">
                        <asp:CheckBox ID="cbSelect" runat="server" CssClass="checkbox" /><asp:Button ID="btSelect" runat="server" CssClass="control" CommandName="Select" Text="Wybierz" />
                    </td>
                </tr>
            </table>
        </span>
    </ItemTemplate>
    <SelectedItemTemplate>
        <asp:Literal ID="ltNewLine" runat="server" Visible="false"><br /></asp:Literal>
        <span id="spanZmiana" class="zmiana sit round5" runat="server">
            <table class="zmiana">
                <tr>
                    <td class="col1">
                        <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol absencji" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("KolorPU").ToString()) %>' />
                    </td>
                    <td class="col2">
                        <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# FirstUpper(Eval("Nazwa")) %>' /><br />
                        <asp:Label ID="lbOpis" runat="server" CssClass="opis" Text='<%# Eval("Opis") %>' />
                    </td>
                </tr>
                <tr class="control">
                    <td colspan="2" >
                        <asp:CheckBox ID="cbSelect" runat="server" CssClass="checkbox" /><asp:Button ID="btSelect" runat="server" CssClass="control" CommandName="Unselect" Text="Wybierz" />
                    </td>
                </tr>
            </table>
        </span>
    </SelectedItemTemplate>
    <EmptyDataTemplate>
        <span>Brak danych</span>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div ID="itemPlaceholderContainer" runat="server" class="cntSelectUrlop zmiany_select">
            <span ID="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="
select Kod, Symbol, Nazwa, KolorPU, Opis, PokazSymbolPU, WyborPU, Typ, NowaLinia, Kolejnosc 
from AbsencjaKody K where K.WidocznyPU = 1
union
select -1, null, 'Korekta', null, null, 0, 0, 0, 0, 9999999 as Kolejnosc 
order by K.Kolejnosc
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidMode" Name="Mode" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
