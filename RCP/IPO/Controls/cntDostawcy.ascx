<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDostawcy.ascx.cs" Inherits="HRRcp.IPO.Controls.cntDostawcy" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="paDostawcy" runat="server" class="cntDostawcy">
    <div id="paFilter" runat="server" class="paFilter tbPracownicy2_filter" visible="true">
        <div class="left">
            <span class="label">Wyszukaj:</span>
            <asp:TextBox ID="tbSearch" CssClass="search textbox" runat="server" ></asp:TextBox>
            <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" />
        </div>
    </div>

    <div class="tabsLine">
        <asp:Menu ID="tabFilter" runat="server" Orientation="Horizontal"  >
            <StaticMenuStyle CssClass="tabsStrip mnFilter" />
            <StaticMenuItemStyle CssClass="tabItem" />
            <StaticSelectedStyle CssClass="tabSelected" />
            <StaticHoverStyle CssClass="tabHover" />
            <Items>
                <asp:MenuItem Text="Aktywni" Value="1" Selected="True"></asp:MenuItem>
                <asp:MenuItem Text="Nieaktywni" Value="0" ></asp:MenuItem>
                <asp:MenuItem Text="Wszyscy" Value="-99" ></asp:MenuItem>
            </Items>
            <StaticItemTemplate>
                <div class="tabCaption">
                    <div class="tabLeft">
                        <div class="tabRight">
                            <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                        </div>
                    </div>
                </div>
            </StaticItemTemplate>
        </asp:Menu>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />
    
<asp:ListView ID="lvKody" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id" InsertItemPosition="None" 
    onitemdatabound="lvKody_ItemDataBound" onitemcommand="lvKody_ItemCommand"
    >
    <ItemTemplate>
        <tr class="it">
            <td class="col2">
                <asp:Label ID="Nazwa" runat="server" Text='<%# Eval("Nazwa") %>' />  <br />
            </td>
            <td class="col2">
                <div class="line1">
                <asp:Label ID="Label122" runat="server" Text='<%# Eval("Adres1") %>' />
                <asp:Label ID="Label4" runat="server"  Text='<%# Eval("Adres2") %>' />
                 </div>
                <div class="line2">
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("KodPocztowy") %>' /> <asp:Label ID="Label1" runat="server" Text='<%# Eval("Miejscowosc") %>' />                
                </div>
            </td>
            <td class="col2">
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Telefon") %>' />
                <asp:Label ID="Label5" runat="server" CssClass="line2" Text='<%# Eval("Email") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Label6" runat="server" Text='<%# Eval("Opis") %>' />
                <asp:Label ID="Label7" runat="server" CssClass="line2" Text='<%# Eval("Uwagi") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
            </td>
            <td id="Td1" class="control" runat="server" visible='<%# GetEditable %>'>
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Usun" Text="Usuń" />
            </td>
        </tr>
    </ItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col2">
                <asp:Label ID="Label12" runat="server" Text='Nazwa Dostawcy' />
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd" ValidationGroup="evg" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:Label ID="Label9" runat="server" Text='Adres' />
                <asp:TextBox ID="Adres1TextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Adres1") %>' />
                <asp:Label ID="Label8" runat="server" Text='Dane dodatkowe' />
                <asp:TextBox ID="Adres2TextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Adres2") %>' /><br />
                <asp:Label ID="Label10" runat="server" Text='Miejscowość' />
                <asp:TextBox ID="MiejscowoscTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Miejscowosc") %>' />
                <asp:Label ID="Label11" runat="server" Text='Kod pocztowy' />
                <asp:TextBox ID="KodPocztowyTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("KodPocztowy") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true" 
                    TargetControlID="KodPocztowyTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789-" />
            </td>
            <td class="col2">
                <asp:Label ID="Label13" runat="server" Text='Telefon' />
                <asp:TextBox ID="TelefonTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Telefon") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="TelefonTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789+" />
                <asp:Label ID="Label14" runat="server" Text='Mail' />
                <asp:TextBox ID="EmailTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Email") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Label15" runat="server" Text='Opis' />
                <asp:TextBox ID="TextBox3" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Opis") %>' />
                <asp:Label ID="Label16" runat="server" Text='Uwagi' />
                <asp:TextBox ID="TextBox1" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Uwagi") %>' />
            </td>  
                                            
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td class="col2">
                <asp:Label ID="Label15" runat="server" Text='Nazwa Dostawcy' />
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd" ValidationGroup="ivg" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:Label ID="Label17" runat="server" Text='Adres' />
                <asp:TextBox ID="Adres1TextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Adres1") %>' />
                <asp:Label ID="Label18" runat="server" Text='Dane Dodatkowe' />
                <asp:TextBox ID="Adres2TextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Adres2") %>' />
                <asp:Label ID="Label19" runat="server" Text='Miejscowość' />
                <asp:TextBox ID="MiejscowoscTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Miejscowosc") %>' />
                <asp:Label ID="Label20" runat="server" Text='Kod Pocztowy' />
                <asp:TextBox ID="KodPocztowyTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("KodPocztowy") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true" 
                    TargetControlID="KodPocztowyTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789-" />
            </td>
             <td class="col2">
                <asp:Label ID="Label21" runat="server" Text='Telefon' />
                <asp:TextBox ID="TelefonTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Telefon") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="TelefonTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789+" />
                <asp:Label ID="Label22" runat="server" Text='Mail' />
                <asp:TextBox ID="EmailTextBox" class="textbox" runat="server" MaxLength="100" Text='<%# Bind("Email") %>' />
            </td> 
            <td class="col2">
                <asp:Label ID="Label23" runat="server" Text='Opis' />
                <asp:TextBox ID="TextBox3" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Opis") %>' />
                <asp:Label ID="Label24" runat="server" Text='Uwagi' />                           
                <asp:TextBox ID="TextBox1" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Uwagi") %>' />
            </td>  
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Zapisz" />
                <asp:Button ID="ClearButton" runat="server" CommandName="Clear" Text="Czyść" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" style="">
            <tr>
                <td>
                    Brak danych<br />                    
                    <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table class="tbDostawcy" runat="server">
            <tr id="Tr1" runat="server">
                <td id="Td2" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" >
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" />
                            </th>
                            <th id="Th4" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Adres1" Text="Adres" /> /
                                <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Adres2" Text="Dane dodatkowe" /><br />                    
                                <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="KodPocztowy" Text="Kod Pocztowy - " />                     
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Miejscowosc" Text="Miejscowość" /> 
                            </th>
                            <th id="Th2" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Telefon" Text="Telefon" />  <br />
                                <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Email" Text="Email" />                   
                            </th>
                            <th id="Th6" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Uwagi" Text="Uwagi" />  <br />
                                <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Opis" Text="Opis" />                   
                            </th>
                            <th id="Th5" class="col1" runat="server">
                                <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Aktywny" Text="Aktywny" />                    
                            </th>
                            <th id="Th3" class="control" runat="server">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr18" class="pager" runat="server">
                <td id="Td3" class="left" runat="server">
<%--                    <uc1:LetterDataPager ID="LetterDataPager1" Letter="NazwiskoLetter" runat="server" />
--%>
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                    &nbsp;&nbsp;&nbsp;
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>
 

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
 
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:IPO %>" 
    onload="SqlDataSource1_Load" 
    onselected="SqlDataSource1_Selected"  
    SelectCommand="
select * 
from IPO_dostawcy
where 
    @filter = -99
 or @filter = cast(Aktywny as int)    
    " 
    
    UpdateCommand="UPDATE IPO_dostawcy SET 
    [Nazwa] = @Nazwa, 
    [Adres1] = @Adres1, 
    [Adres2] = @Adres2, 
    [Miejscowosc] = @Miejscowosc, 
    [KodPocztowy] = @KodPocztowy,
    [Telefon] = @Telefon, 
    [Email] = @Email, 
    [Aktywny] = @Aktywny,
    [Opis] = @Opis,
    [Uwagi] = @Uwagi
      WHERE [Id] = @Id"

    
    InsertCommand="INSERT INTO [IPO_dostawcy]
     ([Nazwa], [Adres1], [Adres2], [Miejscowosc], [KodPocztowy], [Telefon], [Email], [Aktywny], [Opis], [Uwagi])
     VALUES (@Nazwa, @Adres1, @Adres2, @Miejscowosc, @KodPocztowy, @Telefon, @Email, @Aktywny, @Opis, @Uwagi)" 
    onfiltering="SqlDataSource1_Filtering" 
    onprerender="SqlDataSource1_PreRender">
    
    <SelectParameters>
        <asp:ControlParameter ControlID="tabFilter" Name="filter" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    
    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Adres1" Type="String" />
        <asp:Parameter Name="Adres2" Type="String" />
        <asp:Parameter Name="Miejscowosc" Type="String" />
        <asp:Parameter Name="KodPocztowy" Type="String" />
        <asp:Parameter Name="Telefon" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Adres1" Type="String" />
        <asp:Parameter Name="Adres2" Type="String" />
        <asp:Parameter Name="Miejscowosc" Type="String" />
        <asp:Parameter Name="KodPocztowy" Type="String" />
        <asp:Parameter Name="Telefon" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>


