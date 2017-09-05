<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKonfig.ascx.cs" Inherits="HRRcp.IPO.Controls.cntKonfig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<style type="text/css">
    .labelHead
    {
        border: 0;
        font-weight:bold;
    }
    
    .labelText
    {
        border: 0;
    }
    .halfLine
    {
    	width:75%; 
    	float:left;
    	
    }
    
    table.konfigTabHalf
    {
    	max-width:75%; 
    }
</style>

<asp:Label class="labelHead" ID="Parametr" runat="server" Text='IPO - Ustawienia ogólne:' /><br />
<asp:Label class="labelText" ID="Label1" runat="server" Text='Ustawienia systemowe, wprowadzone w nich zmiany spowodują przeorganizowanie logiki działania aplikacji.' /><br />
<br />
<asp:ListView ID="lvOgolne" runat="server" DataSourceID="OgolneDataSource" 
    DataKeyNames="Id"
    onitemdatabound="lvOgolne_ItemDataBound" >
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Opis") %>' />
            </td>
            <td>
                <asp:Label class="labelHead" ID="Par1Label" runat="server" Text='<%# Eval("Wartosc") %>' />
            </td>
            <td class="num">
                <asp:Label ID="Lp" runat="server" Text='<%# Eval("Lp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
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
        <table class="tbMarginesy hoverline konfigTabHalf" runat="server" >
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">  <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" /></th>
                            <th id="Th9" class="col1" runat="server">  <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="Opis" Text="Opis" /></th>  
                            <th id="Th4" class="col1" runat="server">  <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Wartość" Text="Wartość" /></th>                          
                            <th id="Th6" class="col1" runat="server">  <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="Lp" Text="Lp" /></th>
                            <th id="Th3" class="control" runat="server"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col2">
                 <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBox4" Rows="5" TextMode="multiline" class="textbox"
                         MaxLength="500" runat="server" Text='<%# Bind("Opis") %>' /> 
            <td class="col2">
                <asp:TextBox ID="TextBox2" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Wartosc") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TextBox2" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="num">
                <asp:TextBox ID="TextBox1" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Lp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<br /><br />
<hr class="halfLine">
<br /><br />
<asp:Label class="labelHead" ID="Label4" runat="server" Text='IPO - Rodzaje Produktów:' /><br />
<asp:Label class="labelText halfLine" ID="Label5" runat="server" Text='Rodzaje produktów dostępnych w systemie. Jeśli zostanie zaznaczona opcja dodatkowa akceptajca pamiętaj, aby dodać wpis w tabeli IPO - akceptacje oraz uzupełnić uprawnienia dla tego produktu w matrycy uprawnień' /><br />

<br /><br />

<asp:ListView ID="lvRodzajeProduktow" runat="server" DataSourceID="SqlDataSource3" 
    DataKeyNames="Id"
    InsertItemPosition="LastItem" 
    onitemdatabound="lvRodzajeProduktow_ItemDataBound" onitemcommand="lvRodzajeProduktow_ItemCommand" 
    >
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="Par1Label" runat="server" Text='<%# Eval("Opis") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("DodatkowaAkceptacja") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Usun" Text="Usuń" />
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
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th7" class="col1" runat="server">  <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Id" Text="Id" /></th>
                            <th id="Th1" class="col1" runat="server">  <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" /></th>
                            <th id="Th4" class="col1" runat="server">  <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Opis" Text="Opis" /></th>
                            <th id="Th5" class="check" runat="server">   <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="DodatkowaAkceptacja" Text="Dodatkowa Akceptacja" /></th>  
                            <th id="Th9" class="check" runat="server">  <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Aktywny" Text="Aktywny" /></th>                            
                            <th id="Th3" class="control" runat="server"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col2"></td>
            <td class="col2">
                <asp:TextBox ID="TypTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TypTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBox2" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Opis") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TextBox2" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("DodatkowaAkceptacja") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>

            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit"/>
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
        <td></td>
            <td class="col2">
                <asp:TextBox ID="TypTextBox" class="textbox" runat="server" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="TypTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBox3" class="textbox" runat="server" Text='<%# Bind("Opis") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="TextBox3" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# Bind("DodatkowaAkceptacja") %>' />
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


<br /><br />
<hr class="halfLine">
<br /><br />
<asp:Label class="labelHead" ID="Label6" runat="server" Text='IPO - Akceptacje:' /><br />
<asp:Label class="labelText halfLine" ID="Label7" runat="server" Text='Akceptacje zdefiniowane w systemie. Możesz dodawać dowolną ilość akceptacji powiązanych z rodzajami produktów. Uwaga - nazwa akceptacji musi być unikalna.' /><br />
<br /><br />
<asp:ListView ID="lvPodstawoweAkceptacje" runat="server" DataSourceID="PodstawoweAkceptacjeDataSource" 
    DataKeyNames="Id"
    onitemdatabound="lvPodstawoweAkceptacje_ItemDataBound" >
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="Par1Label" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td class="num">
                <asp:Label ID="Lp" runat="server" Text='<%# Eval("Lp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
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
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">  <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" /></th>
                            <th id="Th4" class="col1" runat="server">  <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Symbol" Text="Symbol" /></th>                         
                            <th id="Th6" class="col1" runat="server">  <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Lp" Text="Lp" /></th>
                            <th id="Th3" class="control" runat="server"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col2">
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBox2" class="textbox" runat="server" MaxLength="50" Text='<%# Bind("Symbol") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TextBox2" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>

            <td class="num">
                <asp:TextBox ID="TextBox1" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Lp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<br />
<asp:ListView ID="lvAkceptacje" runat="server" DataSourceID="SqlDataSource2" 
    DataKeyNames="Id"
    InsertItemPosition="LastItem" 
    onitemdatabound="lvAkceptacje_ItemDataBound" 
      OnItemInserting="lvItemIserting">
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="Par1Label" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td>
                <asp:Label ID="Kod" runat="server" Text='<%# Eval("RodzajProduktu") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
            </td>
            <td class="num">
                <asp:Label ID="Lp" runat="server" Text='<%# Eval("Lp") %>' />
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
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">  <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" /></th>
                            <th id="Th4" class="col1" runat="server">  <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Symbol" Text="Symbol" /></th>
                            <th id="Th2" class="col1" runat="server">  <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="RodzajProduktu" Text="Rodzaj Produktu" /></th>
                            <th id="Th9" class="check" runat="server">  <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Aktywny" Text="Aktywny" /></th>                            
                            <th id="Th6" class="col1" runat="server">  <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Lp" Text="Lp" /></th>
                            <th id="Th3" class="control" runat="server"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col2">
                <asp:TextBox ID="TextBox5" class="textbox" runat="server" MaxLength="50" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TextBox5" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBox2" class="textbox" runat="server" MaxLength="50" Text='<%# Bind("Symbol") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TextBox2" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:DropDownList ID="RodzajProduktuDropDownList" runat="server" DataTextField="Nazwa"
                    DataValueField="Id" DataSourceID="rodzajeProduktowDataSource" SelectedValue='<%# Bind("IdRodzajuProduktu") %>'>
                </asp:DropDownList>
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="num">
                <asp:TextBox ID="TextBox1" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Lp") %>' />
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
                 <asp:TextBox ID="TextBox6" class="textbox" runat="server" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert2" ControlToValidate="TextBox6" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBox3" class="textbox" runat="server" Text='<%# Bind("Symbol") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert2" ControlToValidate="TextBox3" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:DropDownList ID="RodzajProduktuDropDownList" runat="server" DataTextField="Nazwa"
                    DataValueField="Id" DataSourceID="rodzajeProduktowDataSource" >
                </asp:DropDownList>
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="num">
                <asp:TextBox ID="LpTextBox" class="textbox" runat="server" Text='<%# Bind("Lp") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true" 
                    TargetControlID="LpTextBox" 
                    FilterType="Custom" 
                    ValidChars="-0123456789" />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert2" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>


<br /><br />
<hr class="halfLine">
<br /><br />
<asp:Label class="labelHead" ID="Label8" runat="server" Text='IPO - Jednostki:' /><br />
<asp:Label class="labelText halfLine" ID="Label9" runat="server" Text='Słownik jednostek. Pozwala zdefiniować dowolny rodzaj jednostek w jakich mogą być zamawiane produkty.' /><br />
</br><br />
<asp:ListView ID="lvJednostki" runat="server" DataSourceID="SqlDataSourceJednostki" 
    DataKeyNames="Id"
        InsertItemPosition="LastItem" 
    onitemdatabound="lvOgolne_ItemDataBound" >
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="Par1Label" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td>
                <asp:Label width="150" ID="Data" runat="server" Text='<%# Eval("DataUtworzenia") %>' />
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
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">  <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" /></th>
                            <th id="Th4" class="col1" runat="server">  <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Symbol" Text="Symbol" /></th>
                            <th id="Th9" class="check" runat="server">  <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="DataUtworzenia" Text="Data Dodania" /></th>                            
                            <th id="Th3" class="control" runat="server"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col2">
                <asp:TextBox ID="TextBoxNazwa" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TextBoxNazwa" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBoxSymbol" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Symbol") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TextBoxSymbol" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td></td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td class="col2">
                <asp:TextBox ID="TextBoxNazwa" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert4" ControlToValidate="TextBoxNazwa" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="TextBoxSymbol" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Symbol") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert4" ControlToValidate="TextBoxSymbol" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td></td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert4" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>


<asp:SqlDataSource ID="OgolneDataSource" runat="server" 
    ConnectionString="<%$ ConnectionStrings:IPO %>" 
    SelectCommand="SELECT * from [IPO_Konfiguracja] ORDER BY  Lp" 
    UpdateCommand="UPDATE [IPO_Konfiguracja] SET [Lp] = @Lp, [Opis] = @Opis, [Wartosc] = @Wartosc WHERE [Id] = @Id"
 >

    <UpdateParameters>
        <asp:Parameter Name="Lp" Type="Int32" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Wartosc" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:IPO %>" 
    SelectCommand="SELECT IPO_Role.*, IPO_RodzajeProduktow.Nazwa as RodzajProduktu from IPO_Role
join IPO_RodzajeProduktow on IPO_Role.IdRodzajuProduktu = IPO_RodzajeProduktow.Id
WHERE TYP=3  ORDER BY  Lp" 
    UpdateCommand="UPDATE [IPO_Role] SET [Nazwa] = @Nazwa, [Lp] = @Lp, [Symbol] = @Symbol, [IdRodzajuProduktu] = @IdRodzajuProduktu, [Aktywny] = @Aktywny, [Typ] = '3', [IdParent] = '2' WHERE [Id] = @Id"
    DeleteCommand="DELETE FROM [IPO_Role] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [IPO_Role] ([Lp], [Nazwa], [Symbol], [Aktywny], [IdRodzajuProduktu], [Typ], [IdParent] ) VALUES (@Lp, @Nazwa, @Symbol, @Aktywny, @IdRodzajuProduktu, '3', '2')" >

    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Lp" Type="Int32" />
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="IdRodzajuProduktu" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Lp" Type="Int32" />
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="IdRodzajuProduktu" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:IPO %>" 
    SelectCommand="SELECT * from [IPO_RodzajeProduktow] " 
    UpdateCommand="UPDATE [IPO_RodzajeProduktow] SET  [Nazwa] = @Nazwa, [Opis] = @Opis, [DodatkowaAkceptacja] = @DodatkowaAkceptacja, [Aktywny] = @Aktywny WHERE [Id] = @Id"
    InsertCommand="INSERT INTO [IPO_RodzajeProduktow] ([Nazwa], [Opis], [DodatkowaAkceptacja], [Aktywny] ) VALUES (@Nazwa, @Opis, @DodatkowaAkceptacja, @Aktywny)" >

    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="DodatkowaAkceptacja" Type="Boolean" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="DodatkowaAkceptacja" Type="Boolean" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlDataSourceJednostki" runat="server" 
    ConnectionString="<%$ ConnectionStrings:IPO %>" 
    SelectCommand="select * from IPO_Jednostki " 
    UpdateCommand="UPDATE IPO_Jednostki SET  [Nazwa] = @Nazwa, [Symbol] = @Symbol WHERE [Id] = @Id"
    DeleteCommand="DELETE FROM IPO_Jednostki WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO IPO_Jednostki ([Nazwa], [Symbol]) VALUES (@Nazwa, @Symbol)" >
    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Symbol" Type="String" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Symbol" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="rodzajeProduktowDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Nazwa FROM IPO_RodzajeProduktow"></asp:SqlDataSource>
    
    
    <asp:SqlDataSource ID="PodstawoweAkceptacjeDataSource" runat="server" 
    ConnectionString="<%$ ConnectionStrings:IPO %>" 
    SelectCommand="SELECT IPO_Role.*  from IPO_Role
WHERE TYP=2  ORDER BY  Lp" 
    UpdateCommand="UPDATE [IPO_Role] SET [Lp] = @Lp, [Symbol] = @Symbol WHERE [Id] = @Id"
    DeleteCommand="DELETE FROM [IPO_Role] WHERE [Id] = @Id" 
>
    <UpdateParameters>
        <asp:Parameter Name="Lp" Type="Int32" />
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>