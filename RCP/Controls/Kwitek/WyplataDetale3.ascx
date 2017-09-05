<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WyplataDetale3.ascx.cs" Inherits="HRRcp.Controls.Kwitek.WyplataDetale3" %>

<asp:DataList ID="dlHeader" runat="server" DataSourceID="SqlDataSource3" CssClass="kwitek_header">
    <ItemTemplate>
        <div class="ramka">
            <div>
                <asp:Label ID="LpLogoLabel" runat="server" CssClass="nrew" Text='<%# Eval("LpLogo") %>' />&nbsp;&nbsp;&nbsp;
                <asp:Label ID="ImieLabel" runat="server" cssclass="nazwisko" Text='<%# Eval("Imie") %>' />
                <asp:Label ID="NazwiskoLabel" runat="server" cssclass="nazwisko" Text='<%# Eval("Nazwisko") %>' /><br />
                <span class="col1">Pesel:</span> <asp:Label ID="PeselLabel" cssclass="pesel" runat="server" Text='<%# Eval("Pesel") %>' /><br />
                <hr />
                <span class="col1">Lista:</span> <asp:Label ID="NrListyLabel" runat="server" Text='<%# Eval("LpNrListy") %>' />, <asp:Label ID="KategoriaListyLabel" runat="server" Text='<%# Eval("KategoriaListy") %>' /><br />                
                <span class="col1">Rok:</span> <asp:Label ID="RokLabel" runat="server" Text='<%# Eval("Rok") %>' /><br /> 
                <span class="col1">Miesiac:</span> <asp:Label ID="MiesiacLabel" runat="server" Text='<%# Eval("Miesiac") %>' /><br />
                <span class="col1">Data:</span> <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' /><br />
            </div>
        </div>
    </ItemTemplate>
</asp:DataList>
<br />

<%

 %>

<asp:HiddenField ID="hidHiddenField1" runat="server"></asp:HiddenField>

<asp:GridView ID="GridView1" runat="server" CellPadding="4" CssClass="GridView1 gvWyplataDetale"
    AutoGenerateColumns="true"
    DataSourceID="SqlDataSource2" ForeColor="#333333" GridLines="None" 
    onrowdatabound="GridView1_RowDataBound">
    <RowStyle BackColor="#EFF3FB" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#2461BF" />
    <AlternatingRowStyle BackColor="White" />
    <EmptyDataTemplate>
        Brak danych
    </EmptyDataTemplate>
</asp:GridView>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ASSECO %>" >
</asp:SqlDataSource>

<asp:HiddenField ID="gvHeader" runat="server" Value="col|N2"/>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ASSECO %>" onselected="SqlDataSource1_Selected"
    SelectCommand="
select 
case 
	when A.Kod = 'ASZCZEPIONKA' then 'Badania' 
	when A.Kod = 'WYPLATA' then 'Do wypłaty'
--else S.Nazwa 
else REPLACE(REPLACE(S.Nazwa, 
	'Wynagrodzenie w naturze', 'świadczenie rzeczowe'),
	'Natura', 'świadczenie rzeczowe')
end as [Nazwa składnika],
A.Wartosc as [Wartość]
--,A.Nazwa 
from Listy where 
A.Kod not in (
'ANAGRODARZECZ',
'ANAGRODARZECZ',
'ANAGRZECZ_NARZ',
'ANAGRZECZ_NARZ',
'APREMIA',
'APREMIA',
'PNAGRODARZECZ'

,'ASZCZEPIONKA'
,'ABENEFIT')

and not A.Kod like '%_N'

and A.Nazwa not in (
'Brutto',
'Suma Prac.',
'Skł.emer.Z',
'Skł.rent..Z',
'Skł.wyp..Z',
'Skł.FP',
'Skł.FGSP',
'Suma Zakład',
'Netto',
'ROR1',
'Pakiet Benefit',
'Zwrot ZUS',
'Premia')
order by A.kolejnosc
    ">    
</asp:SqlDataSource>
