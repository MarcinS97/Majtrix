<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntProdukty.ascx.cs"
    Inherits="HRRcp.IPO.Controls.cntProdukty" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<div id="paFilter" runat="server" class="paFilter tbPracownicy2_filter" visible="true">
    <div class="left">
        <span class="label">Wyszukaj:</span>
        <asp:TextBox ID="tbSearch" CssClass="search textbox" runat="server"></asp:TextBox>
        <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" />
        <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj"
            OnClick="btSearch_Click" />
    </div>
</div>
<br />
<asp:ListView ID="lvKody" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id"
    InsertItemPosition="LastItem" OnItemDataBound="lvKody_ItemDataBound" OnItemInserting="lvItemIserting">
    <ItemTemplate>
        <tr class="it">
            <td class="col2">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' /><br />
                <asp:Label ID="Label3" runat="server" CssClass="line2" Text='<%# Eval("Opis") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Label7" runat="server" Text='<%# Eval("PartNo") %>' /><br />
                <asp:Label ID="Kod" runat="server" CssClass="line2" Text='<%# Eval("RodzajProduktu") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Label9" runat="server" Text='<%# Eval("ProductClass") %>' /><br />
                <asp:Label ID="Label10" runat="server" CssClass="line2" Text='<%# Eval("ProductSubClass") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Nazwa" runat="server" Text='<%# Eval("Cena") %>' />
                <asp:Label ID="Label5" runat="server" Text='<%# Eval("Waluta") %>' /><br />
                <asp:Label ID="Label2" runat="server" CssClass="line2" Text='<%# Eval("Jednostka") %>' />
            </td>
            <td class="col4">
                <asp:Label ID="Label6" runat="server" Text='<%# Eval("DostawcaBYD") %>' /><br />
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("DostawcaZG") %>' /><br />
            </td>
            <td class="col4">
                <asp:Label ID="Label8" runat="server" CssClass="line2" Text='<%# Eval("KontoKsiegowe") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Eval("ProduktStockowy") %>'
                    Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("RoHS") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table class="tbMarginesy hoverline" runat="server">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Nazwa"
                                    Text="Nazwa" /><br />
                                <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Opis"
                                    Text="Opis" />
                            </th>
                            <th id="Th4" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="PartNo"
                                    Text="PartNo" /><br />
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="RodzajProduktu"
                                    Text="Rodzaj Produktu" />
                            </th>
                            <th id="Th7" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton13" runat="server" CommandName="Sort" CommandArgument="ProductClass"
                                    Text="Product Class" /><br />
                                <asp:LinkButton ID="LinkButton14" runat="server" CommandName="Sort" CommandArgument="ProductSubclass"
                                    Text="Product Subclass" />
                            </th>
                            <th id="Th8" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Cena"
                                    Text="Cena" /><br />
                                <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="Jednostka"
                                    Text="Jednostka" />
                            </th>
                            <th id="Th2" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="DostawcaBYD"
                                    Text="DostawcaBYD" /><br />
                                <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="DostawcaZG"
                                    Text="DostawcaZG" /><br />
                            </th>
                            <th id="Th6" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="KontoKsiegowe"
                                    Text="Konto Księgowe" />
                            </th>
                            <th id="Th5" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="ProduktStockowy"
                                    Text="Produkt Stockowy" />
                            </th>
                            <th id="Th9" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="RoHS"
                                    Text="RoHS" />
                            </th>
                            <th id="Th10" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Aktywny"
                                    Text="Aktywny" />
                            </th>
                            <th id="Th3" class="control" runat="server">
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:DataPager ID="DataPager1" runat="server">
            <Fields>
                <asp:NumericPagerField />
            </Fields>
        </asp:DataPager>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col2">
                <asp:TextBox Width="300" ID="NazwaTextBox" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd"
                    ValidationGroup="vgEdit" ControlToValidate="NazwaTextBox" SetFocusOnError="True"
                    Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBox Width="300" ID="OpisTextBox" TextMode="multiline" class="textbox" runat="server"
                    MaxLength="500" Text='<%# Bind("Opis") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Błąd"
                    ValidationGroup="vgEdit" ControlToValidate="OpisTextBox" SetFocusOnError="True"
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox Width="100" ID="TextBox1" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("PartNo") %>' /><br />
                <asp:DropDownList ID="RodzajProduktuDropDownList" runat="server" DataTextField="Nazwa"
                    DataValueField="Id" DataSourceID="rodzajeProduktowDataSource" SelectedValue='<%# Bind("IdRodzajuProduktu") %>'>
                </asp:DropDownList>
            </td>
            <td class="col2">
                <asp:TextBox Width="100" ID="TextBox2" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("ProductClass") %>' /><br />
                <asp:TextBox Width="100" ID="TextBox3" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("ProductSubclass") %>' />
            </td>
            <td class="col2" width="150">
                <asp:TextBox Width="80" ID="CenaTextBox" class="textbox" runat="server" MaxLength="20"
                    Text='<%# Bind("Cena") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true"
                    TargetControlID="CenaTextBox" FilterType="Custom" ValidChars="0123456789.," />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd"
                    ValidationGroup="vgEdit" ControlToValidate="CenaTextBox" SetFocusOnError="True"
                    Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:DropDownList Width="86" ID="WalutaDropDownList" runat="server" DataTextField="SymbolWaluta"
                    DataValueField="SymbolWaluta" DataSourceID="walutyDataSource" SelectedValue='<%# Bind("Waluta") %>'>
                </asp:DropDownList>
                <asp:DropDownList Width="86" ID="JednostkaDropDownList" runat="server" DataTextField="SymbolJednostka"
                    DataValueField="SymbolJednostka" DataSourceID="jednostkaDataSource" SelectedValue='<%# Bind("Jednostka") %>'>
                </asp:DropDownList>
            </td>
            <td class="col2">
                <asp:DropDownList Width="206" ID="DostawcyDropDownList" runat="server" DataTextField="NazwaDostawcy"
                    DataValueField="Id" DataSourceID="dostawcyDataSource" SelectedValue='<%# Bind("IdDostawcaBYD") %>'>
                </asp:DropDownList>
                <asp:DropDownList Width="206" ID="DostawcyDropDownList2" runat="server" DataTextField="NazwaDostawcy"
                    DataValueField="Id" DataSourceID="dostawcyDataSource" SelectedValue='<%# Bind("IdDostawcaZG") %>'>
                </asp:DropDownList>
            </td>
            <td class="col2">
                <asp:TextBox Width="100" ID="KontoTextBox" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("KontoKsiegowe") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# Bind("ProduktStockowy") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox4" runat="server" Checked='<%# Bind("RoHS") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td class="col2">
                <asp:TextBox Width="300" ID="NazwaTextBox" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd"
                    ValidationGroup="vgInsert" ControlToValidate="NazwaTextBox" SetFocusOnError="True"
                    Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBox Width="300" ID="OpisTextBox" TextMode="multiline" class="textbox" runat="server"
                    MaxLength="500" Text='<%# Bind("Opis") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Błąd"
                    ValidationGroup="vgInsert" ControlToValidate="OpisTextBox" SetFocusOnError="True"
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox Width="100" ID="TextBox4" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("PartNo") %>' /><br />
                <asp:DropDownList ID="RodzajProduktuDropDownList" runat="server" DataTextField="Nazwa"
                    DataValueField="Id" DataSourceID="rodzajeProduktowDataSource">
                </asp:DropDownList>
            </td>
            <td class="col2">
                <asp:TextBox Width="100" ID="TextBox5" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("ProductClass") %>' /><br />
                <asp:TextBox Width="100" ID="TextBox6" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("ProductSubclass") %>' />
            </td>
            <td class="col2" width="150">
                <asp:TextBox Width="80" ID="CenaTextBox" class="textbox" runat="server" MaxLength="20"
                    Text='<%# Bind("Cena") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true"
                    TargetControlID="CenaTextBox" FilterType="Custom" ValidChars="0123456789." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd"
                    ValidationGroup="vgInsert" ControlToValidate="CenaTextBox" SetFocusOnError="True"
                    Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:DropDownList Width="86" ID="WalutaDropDownList" runat="server" DataTextField="SymbolWaluta"
                    DataValueField="SymbolWaluta" DataSourceID="walutyDataSource">
                </asp:DropDownList>
                <asp:DropDownList Width="86" ID="JednostkaDropDownList" runat="server" DataTextField="SymbolJednostka"
                    DataValueField="SymbolJednostka" DataSourceID="jednostkaDataSource">
                </asp:DropDownList>
            </td>
            <td class="col2">
                <asp:DropDownList Width="206" ID="DostawcyDropDownList" runat="server" DataTextField="NazwaDostawcy"
                    DataValueField="Id" DataSourceID="dostawcyDataSource">
                </asp:DropDownList>
                <asp:DropDownList Width="206" ID="DostawcyDropDownList2" runat="server" DataTextField="NazwaDostawcy"
                    DataValueField="Id" DataSourceID="dostawcyDataSource">
                </asp:DropDownList>
            </td>
            <td class="col2">
                <asp:TextBox Width="100" ID="KontoTextBox" class="textbox" runat="server" MaxLength="100"
                    Text='<%# Bind("KontoKsiegowe") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# Bind("ProduktStockowy") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox4" runat="server" Checked='<%# Bind("RoHS") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>
<div class="pager">
    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server"></asp:Label></span>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span class="count">Pokaż na stronie:</span>
    <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" OnChange="showAjaxProgress();"
        OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
        <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
        <asp:ListItem Text="50" Value="50"></asp:ListItem>
        <asp:ListItem Text="100" Value="100"></asp:ListItem>
        <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
    </asp:DropDownList>
</div>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    OnLoad="SqlDataSource1_Load" OnSelected="SqlDataSource1_Selected" SelectCommand="WITH DostawcyTMP as(
SELECT IPO_Dostawcy.Id, Nazwa, IPO_PreferowaniDostawcy.IdRegionu, IPO_PreferowaniDostawcy.IdProduktu FROM IPO_Dostawcy 
JOIN IPO_PreferowaniDostawcy 
	ON IPO_PreferowaniDostawcy.IdDostawcy = IPO_Dostawcy.Id
)
Select prd.Id, prd.Nazwa, prd.Opis, prd.ProduktStockowy, 
    IPO_RodzajeProduktow.Nazwa as RodzajProduktu, prd.IdRodzajuProduktu, prd.Cena, prd.Waluta,
    prd.KontoKsiegowe, prd.Aktywny, prd.RoHS, prd.Jednostka, prd.DataUtworzenia, prd.PartNo,
    prd.ProductClass, prd.ProductSubclass,
	(SELECT TOP 1 Nazwa FROM DostawcyTMP WHERE DostawcyTMP.IdProduktu = prd.Id
			and DostawcyTMP.IdRegionu = 2) as DostawcaBYD,
	(SELECT TOP 1 Id FROM DostawcyTMP WHERE DostawcyTMP.IdProduktu = prd.Id
			and DostawcyTMP.IdRegionu = 2) as IdDostawcaBYD,
	(SELECT TOP 1 Nazwa FROM DostawcyTMP WHERE DostawcyTMP.IdProduktu = prd.Id
			and DostawcyTMP.IdRegionu = 3) as DostawcaZG,
	(SELECT TOP 1 Id FROM DostawcyTMP WHERE DostawcyTMP.IdProduktu = prd.Id
			and DostawcyTMP.IdRegionu = 3) as IdDostawcaZG
	from IPO_Produkty prd
       left join IPO_RodzajeProduktow on IPO_RodzajeProduktow.Id=prd.IdRodzajuProduktu"
    UpdateCommand="UPDATE IPO_Produkty SET 
    [Nazwa] = @Nazwa, 
    [Opis] = @Opis, 
    [ProduktStockowy] = @ProduktStockowy, 
    [IdRodzajuProduktu] = @IdRodzajuProduktu, 
    [Cena] = @Cena,
    [Waluta] = @Waluta, 
    [KontoKsiegowe] = @KontoKsiegowe, 
    [PartNo] = @PartNo, 
    [ProductClass] = @ProductClass, 
    [ProductSubclass] = @ProductSubClass,
    [Aktywny] = @Aktywny,
    [RoHS] = @RoHS, 
    [Jednostka] = @Jednostka
      WHERE [Id] = @Id
      
     UPDATE IPO_PreferowaniDostawcy SET
     [IdDostawcy] = @IdDostawcaBYD
        WHERE [IdRegionu] = 2 AND [IdProduktu] = @Id
     
     UPDATE IPO_PreferowaniDostawcy SET
     [IdDostawcy] = @IdDostawcaZG
        WHERE [IdRegionu] = 3 AND [IdProduktu] = @Id" DeleteCommand="DELETE FROM [IPO_Produkty] WHERE [Id] = @Id"
    InsertCommand="
    DECLARE @IdProd int
    
    INSERT INTO [IPO_Produkty]
     ([Nazwa], [Opis], [ProduktStockowy], [IdRodzajuProduktu], [Cena], [Waluta], [KontoKsiegowe], [PartNo], [ProductClass], [ProductSubclass], [Aktywny], [RoHS], [Jednostka])
     VALUES (@Nazwa, @Opis, @ProduktStockowy, @IdRodzajuProduktu, @Cena, @Waluta, @KontoKsiegowe, @PartNo, @ProductClass, @ProductSubclass, @Aktywny, @RoHS, @Jednostka)
     
     Select @idProd = SCOPE_IDENTITY()
     
     INSERT INTO [IPO_PreferowaniDostawcy]
     ([IdProduktu], [IdDostawcy], [IdRegionu])  
     VALUES (@idProd, @IdDostawcyBYD, 2)
     
     INSERT INTO [IPO_PreferowaniDostawcy]
     ([IdProduktu], [IdDostawcy], [IdRegionu])  
     VALUES (@idProd, @IdDostawcyZG, 3)
     " OnFiltering="SqlDataSource1_Filtering" OnPreRender="SqlDataSource1_PreRender">
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="IdRodzajuProduktu" Type="Int32" />
        <asp:Parameter Name="Cena" Type="Decimal" />
        <asp:Parameter Name="Waluta" Type="String" />
        <asp:Parameter Name="Jednostka" Type="String" />
        <asp:Parameter Name="IdDostawcaBYD" Type="Int32" />
        <asp:Parameter Name="IdDostawcaZG" Type="Int32" />
        <asp:Parameter Name="KontoKsiegowe" Type="String" />
        <asp:Parameter Name="PartNo" Type="String" />
        <asp:Parameter Name="ProductClass" Type="String" />
        <asp:Parameter Name="ProductSubclass" Type="String" />
        <asp:Parameter Name="ProduktStockowy" Type="Boolean" />
        <asp:Parameter Name="RoHS" Type="Boolean" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="IdRodzajuProduktu" Type="Int32" />
        <asp:Parameter Name="Cena" Type="Decimal" />
        <asp:Parameter Name="Waluta" Type="String" />
        <asp:Parameter Name="Jednostka" Type="String" />
        <asp:Parameter Name="IdDostawcyBYD" Type="Int32" />
        <asp:Parameter Name="IdDostawcyZG" Type="Int32" />
        <asp:Parameter Name="KontoKsiegowe" Type="String" />
        <asp:Parameter Name="PartNo" Type="String" />
        <asp:Parameter Name="ProductClass" Type="String" />
        <asp:Parameter Name="ProductSubclass" Type="String" />
        <asp:Parameter Name="ProduktStockowy" Type="Boolean" />
        <asp:Parameter Name="RoHS" Type="Boolean" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="rodzajeProduktowDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Nazwa FROM IPO_RodzajeProduktow"></asp:SqlDataSource>
<asp:SqlDataSource ID="walutyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Lp, Symbol as SymbolWaluta FROM IPO_Waluty ORDER BY Lp">
</asp:SqlDataSource>
<asp:SqlDataSource ID="jednostkaDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Symbol as SymbolJednostka FROM IPO_Jednostki"></asp:SqlDataSource>
<asp:SqlDataSource ID="dostawcyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="select Id,Nazwa as NazwaDostawcy from IPO_Dostawcy"></asp:SqlDataSource>
