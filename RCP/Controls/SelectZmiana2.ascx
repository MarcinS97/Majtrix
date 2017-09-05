<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectZmiana2.ascx.cs" Inherits="HRRcp.Controls.SelectZmiana2" %>

<asp:HiddenField ID="hidEditMode" runat="server" />
<asp:HiddenField ID="hidMode" runat="server" />

<asp:ListView ID="lvZmiany" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id" 
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
                        <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' /><br />
                    </td>
                    <td class="col2" >
                        <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# Eval("Nazwa") %>' /><br />
                        <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:t}") + " - " %>' /> 
                        <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:t}") %>' />
                        <asp:Label ID="lbMargines" runat="server" Text='<%# Eval("MarginesLabel") %>' /><br />
                        <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") + "%<br />" %>' Visible='<%# Eval("StawkaVisible") %>' ToolTip="Stawka w godzinach zmiany"/>
                        <asp:Label ID="lbNadgodziny" CssClass="nadgodziny" runat="server" Text="brak nadgodzin" Visible="false"/>
                        <asp:Label ID="lbBrakZgody" CssClass="brakzgody" runat="server" Text="brak nadgodzin" />
                        <asp:Label ID="lbZgoda" CssClass="zgoda" runat="server" Text="zgoda na nadgodziny" />
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
                        <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' /><br />
                    </td>
                    <td class="col2" >
                        <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# Eval("Nazwa") %>' /><br />
                        <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:t}") + " - "  %>' />
                        <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:t}") %>' />
                        <asp:Label ID="lbMargines" runat="server" Text='<%# Eval("MarginesLabel") %>' /><br />
                        <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") + "%<br />" %>' Visible='<%# Eval("StawkaVisible") %>' ToolTip="Stawka w godzinach zmiany"/>
                        <asp:Label ID="lbNadgodziny" CssClass="nadgodziny" runat="server" Text="brak nadgodzin" Visible="false"/>
                        <asp:Label ID="lbBrakZgody" CssClass="brakzgody" runat="server" Text="brak nadgodzin" />
                        <asp:Label ID="lbZgoda" CssClass="zgoda" runat="server" Text="zgoda na nadgodziny" />
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
        <div ID="itemPlaceholderContainer" runat="server" class="zmiany_select">
            <span ID="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="
SELECT *, 
    case when Margines &lt; 0 then '' else
        ' ±' + convert(varchar, Margines) + ' min.'
    end as MarginesLabel,
    cast(case when Stawka = 100 then 0 else 1 end as bit) as StawkaVisible,
    LEFT(convert(varchar, Od, 8),5) as CzasOd,
    LEFT(convert(varchar, Do, 8),5) as CzasDo
FROM Zmiany
where Widoczna=1 and (
    @Mode = 0 or
    @Mode = 1 and Visible = 1 or
    @Mode = 2 and Visible = 0
)
ORDER BY Widoczna desc, Visible desc, Kolejnosc, TypZmiany, Symbol" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidMode" Name="Mode" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>


<%--
    --@Mode = 1 and (Visible = 1 or Id > 16) or
    --@Mode = 2 and (Visible = 0 and Id < 16)



    SelectCommand="SELECT *, 
                        case when Margines &lt;= 0 then '' else
                            ' ±' + convert(varchar, Margines) + ' min.'
                        end as MarginesLabel,
                        cast(case when Stawka = 100 then 0 else 1 end as bit) as StawkaVisible,
                        LEFT(convert(varchar, Od, 8),5) as CzasOd,
                        LEFT(convert(varchar, Do, 8),5) as CzasDo
                   FROM [Zmiany] 
                   where Widoczna=1
                   ORDER BY Widoczna desc, Visible desc, Kolejnosc, TypZmiany, Symbol" >
                   
--%>