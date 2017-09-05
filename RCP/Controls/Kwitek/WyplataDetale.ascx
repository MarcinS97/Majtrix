<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WyplataDetale.ascx.cs" Inherits="HRRcp.Controls.Kwitek.WyplataDetale" %>

<asp:HiddenField ID="xhidKadryId" runat="server" />
<asp:HiddenField ID="xhidListaId" runat="server" />
<asp:HiddenField ID="xhidDataSzabl" runat="server" />


<asp:DataList ID="dlHeader" runat="server" DataSourceID="SqlDataSource1" CssClass="kwitek_header">
    <ItemTemplate>
        <div class="ramka">
            <div>           
                <asp:Label ID="LpLogoLabel" runat="server" CssClass="nrew" Text='<%# Eval("LpLogo") %>' />
                <asp:Label ID="ImieLabel" runat="server" CssClass="nazwisko" Text='<%# Eval("Imie") %>' />
                <asp:Label ID="NazwiskoLabel" runat="server" CssClass="nazwisko" Text='<%# Eval("Nazwisko") %>' />
                <br />
                Pesel: <asp:Label ID="PeselLabel" runat="server" Text='<%# Eval("Pesel") %>' /><br />
                
                Lista: <asp:Label ID="NrListyLabel" runat="server" Text='<%# Eval("NrListy") %>' />, <asp:Label ID="KategoriaListyLabel" runat="server" Text='<%# Eval("KategoriaListy") %>' /><br />
                
                Rok: <asp:Label ID="RokLabel" runat="server" Text='<%# Eval("Rok") %>' /> Miesiac: <asp:Label ID="MiesiacLabel" runat="server" Text='<%# Eval("Miesiac") %>' /><br />
                <br />
                Data: <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' /><br />
                <br />
                <br />
            </div>
        </div>
    </ItemTemplate>
</asp:DataList>
<br />

<asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" 
    DataSourceID="SqlDataSource1" ondatabound="DetailsView1_DataBound" 
    onitemcreated="DetailsView1_ItemCreated"
    CssClass="GridView1">
</asp:DetailsView>

<asp:HiddenField ID="NumFields" runat="server" Value=""/>
<asp:HiddenField ID="FirstField" runat="server" Value="1"/>
<asp:HiddenField ID="LastField" runat="server" Value="524"/>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ASSECO %>" onselected="SqlDataSource1_Selected">
</asp:SqlDataSource>

