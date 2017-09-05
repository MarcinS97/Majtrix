<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZmianyList.ascx.cs" Inherits="HRRcp.RCP.Controls.Adm.cntZmianyList" %>
<%@ Register Src="~/RCP/Controls/Adm/cntZmianyEditModal.ascx" TagPrefix="uc1" TagName="cntZmianyEditModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>


<asp:Button ID="btnInsert" runat="server" CssClass="btn btn-success" OnClick="btnInsert_Click" Text="Dodaj zmianę" />
<br /><br />


<table class="table table-hovered zmiana">
    <tr>
        <th>Symbol</th>
                <th>Nazwa</th>
                <th>Czas Od</th>
                <th>Czas Do</th>
                <th>Stawka</th>
                <th>Typ zmiany</th>
                <th>Kolejność</th>
                <th></th>
    </tr>
    <asp:Repeater ID="rpZmiany" runat="server" DataSourceID="dsZmianyRep">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                </td>
                <td>
                    <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# Eval("Nazwa") %>' /><br />
                </td>
                <td>
                    <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("CzasOd", "") %>' />
                </td>
                <td>
                    <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("CzasDo", "") %>' />
                </td>
                <td>
                    <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") %>' />%
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("TypNazwa") %>' />
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Kolejnosc0") %>' />
                </td>
                <td>
                    <asp:LinkButton ID="EditButton" runat="server"  OnClick="EditButton_Click" CommandArgument ='<%# Eval("Id") %>'><span aria-hidden="true" class="glyphicon glyphicon-edit"></span></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>





<asp:SqlDataSource ID="dsZmianyRep" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
SELECT 
    ISNULL(STUFF((
	select  ISNULL(';' + dbo.ToTimeHMM(s.Items),'')
		from dbo.SplitIntSort(z.InneCzasy,';') s
 for XML PATH('')
), 1, 1, ''), '') InneCzasyDisp,

    
    Z.*, ISNULL(Z.Kolejnosc, 0) as Kolejnosc0,

--LEFT(convert(varchar, Z.Od, 8),5) as CzasOd,
--LEFT(convert(varchar, Z.Do, 8),5) as CzasDo,
convert(varchar(5), Z.Od, 8) as CzasOd,
convert(varchar(5), Z.Do, 8) as CzasDo,
K.Nazwa as TypNazwa                   
FROM Zmiany Z
left join Kody K on K.Typ = 'ZMIANA.TYP' and K.Kod = Z.Typ 
ORDER BY Widoczna desc, Visible desc, Kolejnosc, TypZmiany, Symbol">
    <SelectParameters>
    </SelectParameters>
</asp:SqlDataSource>
