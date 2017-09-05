<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LiniaSelect2.ascx.cs" Inherits="HRRcp.SzkoleniaBHP.Controls.LiniaSelect2" %>

<table class="table0 cntLiniaSelect2" >
    <tr>
        <td id="paKier" runat="server" class="kier" visible="false">
            <asp:Label ID="lbKierCaption" runat="server" CssClass="label" Text="Przełożony:" ></asp:Label>
            <asp:DropDownList ID="ddlPrzelozony" runat="server" AutoPostBack="true" onselectedindexchanged="ddlPrzelozony_SelectedIndexChanged"></asp:DropDownList>    
        </td>
<%--
        <td class="str">
            <asp:Label ID="lbCaption" runat="server" CssClass="label" Text="Struktura organizacyjna:"></asp:Label>
            <asp:DropDownList ID="ddlLinie" runat="server" AutoPostBack="true" onselectedindexchanged="ddlLinie_SelectedIndexChanged"> </asp:DropDownList>
            <asp:Label ID="lbCaption1" CssClass="label caption1" runat="server" Text="Struktura organizacyjna:" Visible="false"></asp:Label>
            <asp:Label ID="lbLinia1" CssClass="linia1" runat="server" Visible="false"></asp:Label>
        </td>
--%>
        <td id="paSubStr" runat="server" visible="true" class="sub">
            <asp:CheckBox ID="cbSubStr" runat="server" Text="Pokaż dane z podstruktury" Checked="true" AutoPostBack="true" oncheckedchanged="cbSubStr_CheckedChanged" 
                ToolTip="Filtruje pracowników i czynności z całej podstruktury, niezaznaczony - wyswietla pracowników i czynności przypisane do wybranej jednostki"
                />
                <%--
                ToolTip="Filtr pracowników i czynności: zaznaczony prezentuje dane z całej podstruktury, niezaznaczony tylko pracowników i czynności przypisane do wybranej jednostki"
                --%>
        </td>
        <td class="status">
            <asp:Label ID="lbZakres" runat="server" CssClass="label" Text="Pracownicy:"></asp:Label>
            <asp:DropDownList ID="ddlZakres" runat="server" AutoPostBack="true" onselectedindexchanged="ddlZakres_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
</table>
