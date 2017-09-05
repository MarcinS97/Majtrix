<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pracownicy2.ascx.cs" Inherits="HRRcp.Controls.Kiosk.Pracownicy2" %>
<%@ Register src="~/Controls/LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidKierId" runat="server" />
<asp:HiddenField ID="hidLiniaId" runat="server" />
<asp:HiddenField ID="hidParams" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />
<asp:HiddenField ID="hidFunc" runat="server" />


<div id="paFilter" runat="server" class="tbPracownicyFilter input-group" visible="false">
    <span class="input-group-addon addon-default">Wyszukaj:</span>
    <asp:TextBox ID="tbSearch" CssClass="textbox form-control" runat="server" ></asp:TextBox>
    
    <div class="input-group-btn">
        <asp:Button ID="btClear" runat="server" CssClass="button75 btn btn-default" Text="Czyść" />
    </div>
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />

<asp:ListView ID="lvPracownicy" runat="server" DataKeyNames="Id_Pracownicy" DataSourceID="SqlDataSource1" 
    InsertItemPosition="None" 
    onitemcreated="lvPracownicy_ItemCreated" 
    onlayoutcreated="lvPracownicy_LayoutCreated" 
    onsorting="lvPracownicy_Sorting" 
    onitemcommand="lvPracownicy_ItemCommand" 
    onitemdatabound="lvPracownicy_ItemDataBound" 
    ondatabound="lvPracownicy_DataBound" 
    oniteminserting="lvPracownicy_ItemInserting" 
    onitemupdating="lvPracownicy_ItemUpdating" 
    ondatabinding="lvPracownicy_DataBinding" 
    onpagepropertieschanged="lvPracownicy_PagePropertiesChanged"     
    oniteminserted="lvPracownicy_ItemInserted" 
    onitemdeleted="lvPracownicy_ItemDeleted" 
    onitemdeleting="lvPracownicy_ItemDeleting" 
    onitemupdated="lvPracownicy_ItemUpdated" 
    onselectedindexchanged="lvPracownicy_SelectedIndexChanged">
    <ItemTemplate>
        <tr id="trLine" runat="server">
            <td id="tdSelect" runat="server" class="select" visible="false">
                <asp:Button ID="btSelect" runat="server" CommandName="Select" Text="Wybierz" CssClass="btn btn-default" />
            </td>
            <td class="nazwisko_nrew">
                <asp:Label ID="NazwiskoLabel" runat="server" CssClass="nazwisko" Text='<%# Eval("NazwiskoImie") %>' /><br />
                <span class="line2 obok">
                    <asp:Label ID="lbRcpId" runat="server" CssClass="left kartarcp" Text='<%# Eval("NrKarty") %>' />
                    <asp:Label ID="Nr_EwidLabel" runat="server" CssClass="right" Text='<%# Eval("Nr_Ewid") %>' />
                </span>
                <asp:HiddenField ID="hidObcy" runat="server" />
            </td>
            <td id="tdAPT" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbAPT" runat="server" CssClass="check" Enabled="false" Checked='<%# Eval("APT") %>'/>                
            </td>
            <td>
                <asp:Label ID="lbStanowisko" runat="server" Text='<%# Eval("Nazwa_Stan") %>' /><br />
                <asp:Label ID="lbUmowa" runat="server" CssClass="line2" Text='<%# Eval("Rodzaj_Umowy") %>' /><br />
                <span class="line2">
                    <asp:Label ID="lbDataZatr" runat="server" Text='<%# Eval("DataZatr", "{0:d}") %>' /> - 
                    <asp:Label ID="lbDataUmDo" runat="server" Text='<%# Eval("DataUmDo", "{0:d}") %>' />
                </span>
            </td>
            
            <td id="tdJednM" runat="server" class="jednM">
                <div id="paJednM" runat="server">
                    <asp:Label ID="lbStrumienM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaM") %>' ToolTip='<%# Eval("NazwaStrumieniaM") %>' /> / 
                    <asp:Label ID="lbLiniaM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiM") %>' ToolTip='<%# Eval("NazwaLiniiM") %>' /><br />
                    <asp:Label ID="lbKierownikM" runat="server" class="line2" Text='<%# Eval("KierownikM") %>' />                
                </div>
                <div id="paPrzJednM" runat="server" visible="false">
                    <asp:Label ID="lbStrumien" runat="server" CssClass="tooltip wholeline" Text='<%# Eval("SymbolStrumieniaM") %>' ToolTip='<%# Eval("NazwaStrumieniaM")%>' />
                    <asp:DropDownList ID="ddlJednM" CssClass="jednM" runat="server" 
                        OnChange="javascript:if (this.value == '') this.title=null; else this.title=this.options[this.selectedIndex].text;"/>
                </div>
            </td>

            <td id="tdJednA" runat="server" class="jednA">
                <asp:Label ID="lbStrumienA" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaA") %>' ToolTip='<%# Eval("NazwaStrumieniaA") %>' /> / 
                <asp:Label ID="lbLiniaA" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiA") %>' ToolTip='<%# Eval("NazwaLiniiA") %>' /><br />
                <asp:Label ID="lbKierownikA" runat="server" class="line2" Text='<%# Eval("KierownikA") %>' />                
            </td>
            <td id="tdJednAod" runat="server" class="data jednA">
                <asp:Label ID="lbOd" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /><br />
                <asp:Label ID="lbDo" runat="server" CssClass="line2" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>

            <%--
            <td id="tdJednAod" runat="server" class="data jednA">
                <asp:Label ID="lbOd" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td id="tdJednAdo" runat="server" class="data jednA">
                <asp:Label ID="lbDo" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>

            OnSelectedIndexChanged="ddlJednM_SelectedIndexChanged" 
            
            <td id="tdRating" runat="server" class="num">
                <asp:Rating ID="Rating1" runat="server" ReadOnly="true"
                    CurrentRating="4"
                    MaxRating="5"
                    StarCssClass="rating"
                    FilledStarCssClass="rating_filled"
                    EmptyStarCssClass="rating_empty"
                    WaitingStarCssClass="rating_wating"
                    style="float: left;"
                    ></asp:Rating>
            </td>
            --%>
            <td id="tdOcena" runat="server" class="num" visible="false">
                <asp:Label ID="lbOcena" runat="server" Text='<%# Eval("Ocena") %>' />
            </td>
            <%--
            <td id="tdAbsencja" runat="server" >
                <asp:Label ID="Id_StatusLabel" runat="server" Text='<%# Eval("Absencja") %>' />
            </td>
            --%>
            <td id="tdStatus" class="status" runat="server" >
                <asp:Label ID="StatusLabel" runat="server" Text='<%# PrepareStatus(Eval("Status")) %>' /><br />
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("marker") %>' />
                <asp:Label ID="lbDataZwol" runat="server" CssClass="line2" Text='<%# Eval("DataZwol", "{0:d}") %>' />
            </td>

            <td id="tdControl" runat="server" class="control1" >
                <asp:Button ID="btOcen" runat="server" CommandName="Ocena" CommandArgument='<%# Eval("Id_Pracownicy") %>' Text="Oceń" Visible="false" />
                <asp:Button ID="btOddeleguj" runat="server" CommandName="Deleg" CommandArgument='<%# Eval("Id_Pracownicy") %>' Text="Oddeleguj" Visible="false" />
                <asp:Button ID="btPrzypisz" runat="server" CommandName="Przypisz" Text="Przypisz" Visible="false"/>

                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" Visible="false"/>
                <%--
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false"/>
                --%>    
                <asp:Button ID="btEdit2" runat="server" CommandName="Edit2" Text="Edycja" Visible="false"/>
                <asp:Button ID="btUpdate2" runat="server" CommandName="Update2" Text="Zapisz" Visible="false"/>
                <asp:Button ID="btCancel2" runat="server" CommandName="Cancel2" Text="Anuluj" Visible="false"/>
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>

    <InsertItemTemplate>
        <tr class="iit">
            <td colspan="6" class="data">
                <table class="table0">
                    <tr>
                        <td class="col1">
                            <div class="title">Dane podstawowe</div>
                            <span class="col1">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" MaxLength="200" Text='<%# Bind("Nazwisko") %>' />
                            <asp:RequiredFieldValidator ControlToValidate="NazwiskoTextBox" ValidationGroup="ivg" ErrorMessage="<br />Pole wymagane" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                            <span class="col1">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" MaxLength="200" Text='<%# Bind("Imie") %>' />
                            <asp:RequiredFieldValidator ControlToValidate="ImieTextBox" ValidationGroup="ivg" ErrorMessage="<br />Pole wymagane" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                            
                            <span class="col1">Nr ewid.:</span>
                            <asp:TextBox ID="Nr_EwidTextBox" runat="server" MaxLength="20" CssClass="nrew" Text="" ReadOnly="true"/><br />

                            <span class="col1">Numer karty RCP:</span>
                            <asp:TextBox ID="tbRcpId" runat="server" MaxLength="30" Text='<%# Bind("NrKarty") %>'/><br />                            

                            <asp:Label ID="lbLogin" class="col1" runat="server" Text="Login:" />
                            <asp:TextBox ID="tbLogin" runat="server" MaxLength="30" Text='<%# Bind("Login") %>' /><br />

                            <span class="col1">Hasło:</span>
                            <asp:TextBox ID="tbPass" runat="server" MaxLength="30" /><br />

                            <%--
                            <span class="col1">Nr ewid.:</span>
                            <asp:TextBox ID="Nr_EwidTextBox" runat="server" MaxLength="20" CssClass="nrew" Text='<%# Bind("Nr_Ewid") %>' />
                            <asp:RequiredFieldValidator ControlToValidate="Nr_EwidTextBox" ValidationGroup="ivg" ErrorMessage="<br />Pole wymagane" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                            <span class="col1">Pracownik APT:</span>
                            <asp:CheckBox ID="cbAPT" runat="server" CssClass="check" Checked='<%# Bind("APT") %>'/>
                            --%>
                        </td>
                        <td class="col2">
                            <div class="title">Organizacja</div>

                            <span class="col1">Grupa zatrudnienia:</span>
                            <asp:DropDownList ID="ddlGrupaZatr" runat="server" Enabled="false"/><br />
                            <span class="col1">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server" Enabled="false" /><br />

                            <span class="col1">Data zatrudnienia:</span>
                            <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataZatr") %>' Opis="yyyy-mm-dd" /><br />
                            <span class="col1">Umowa do:</span>
                            <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataUmDo") %>' Opis="yyyy-mm-dd" /><br />

                            <span class="col1">Jednostka macierzysta:</span>
                            <asp:DropDownList ID="ddlJednM" CssClass="jednM" runat="server" Enabled="false" /><br />

                            <asp:Label ID="lbStatusLabel" CssClass="col1" runat="server" Text="Status:"></asp:Label>
                            <asp:DropDownList ID="ddlStatus" CssClass="status" Enabled="true" runat="server" /><br />
                            <span class="col1">Data zwolnienia:</span>
                            <uc1:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataZwol") %>' Opis="yyyy-mm-dd" />
                        </td>
                    </tr>
                </table>               
            </td>
            <td class="control1">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" ValidationGroup="ivg" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr id="trOddeleguj1" runat="server" class="eit" visible="false">
            <td class="nazwisko_nrew">
                <asp:HiddenField ID="hidPracId" runat="server" Value='<%# Eval("Id_Pracownicy") %>' />
                <asp:Label ID="NazwiskoLabel" runat="server" Text='<%# PrepNazwisko(Eval("NazwiskoImie")) %>' /><br />
                <asp:Label ID="Nr_EwidLabel" runat="server" CssClass="line2" Text='<%# Eval("Nr_Ewid") %>' />
            </td>
            <td>
                <asp:Label ID="lbStanowisko" runat="server" Text='<%# Eval("Nazwa_Stan") %>' /><br />
                <asp:Label ID="lbUmowa" runat="server" CssClass="line2" Text='<%# Eval("Rodzaj_Umowy") %>' />
            </td>
            
            <td id="tdJednM" runat="server" class="jednM">
                <div id="paLiniaM" runat="server" visible="false" >
                    <span class="col1">Linia:</span><asp:DropDownList ID="ddlLiniaM" runat="server" ></asp:DropDownList>
                </div>
                <asp:Label ID="lbStrumienM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaM") %>' ToolTip='<%# Eval("NazwaStrumieniaM") %>' /> /
                <asp:Label ID="lbLiniaM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiM") %>' ToolTip='<%# Eval("NazwaLiniiM") %>' /><br />
                <div id="paKierM" runat="server" visible="false" >
                    <span class="col1">Kierownik:</span><asp:DropDownList ID="ddlKierownik" runat="server" ></asp:DropDownList><br />
                </div>
            </td>

            <td id="tdOddeleguj" runat="server" colspan="3" class="jednA" visible="false">
                <span class="col1">Linia:</span><asp:DropDownList ID="ddlLinia" runat="server" ></asp:DropDownList><br />
                <span class="col1">Od dnia:</span><uc1:DateEdit ID="deOd" runat="server" /><br />
                <span class="col1">Do dnia:</span><uc1:DateEdit ID="deDo" runat="server" />
            </td>
            <%--
            <td id="tdRating" runat="server" class="num">
                <asp:Rating ID="Rating1" runat="server" ReadOnly="true"
                    CurrentRating="4"
                    MaxRating="5"
                    StarCssClass="rating"
                    FilledStarCssClass="rating_filled"
                    EmptyStarCssClass="rating_empty"
                    WaitingStarCssClass="rating_wating"
                    style="float: left;"
                    ></asp:Rating>
            </td>
            --%>
            <td id="tdOcena" runat="server" class="num">
                <asp:Label ID="lbOcena" runat="server" Text='<%# Eval("Ocena") %>' />
            </td>
            <td>
                <asp:Label ID="Id_StatusLabel" runat="server" Text='<%# Eval("Absencja") %>' />
            </td>
            <td id="tdControl" runat="server" class="control1" >
                <asp:Button ID="UpdateButton1" CssClass="control" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton1" CssClass="control" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>        
        <!---------------------------->
        <tr class="eit" >
            <td colspan="6" class="data">
                <table class="table0">
                    <tr>
                        <td class="col1">
                            <div class="title">Dane podstawowe</div>
                            <span class="col1">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" MaxLength="200" Text='<%# Bind("Nazwisko") %>' ReadOnly="true" />
                            <asp:RequiredFieldValidator ControlToValidate="NazwiskoTextBox" ValidationGroup="evg" ErrorMessage="<br />Pole wymagane" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                            <span class="col1">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" MaxLength="200" Text='<%# Bind("Imie") %>' ReadOnly="true"/>
                            <asp:RequiredFieldValidator ControlToValidate="ImieTextBox" ValidationGroup="evg" ErrorMessage="<br />Pole wymagane" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                            <span class="col1">Nr ewid.:</span>
                            <asp:TextBox ID="Nr_EwidTextBox" runat="server" MaxLength="20" CssClass="nrew" Text='<%# Bind("Nr_Ewid") %>' ReadOnly="true"/>
                            <asp:RequiredFieldValidator ControlToValidate="Nr_EwidTextBox" ValidationGroup="evg" ErrorMessage="<br />Pole wymagane" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                            <%--
                            <span class="col1">Pracownik APT:</span>
                            <asp:CheckBox ID="cbAPT" runat="server" CssClass="check" Checked='<%# Bind("APT") %>'/>
                            --%>
                            <%--
                            <div class="title">Dane podstawowe</div>
                            <span class="col1">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' MaxLength="50" /><br />
                            <span class="col1">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" Text='<%# Bind("Imie") %>' MaxLength="50" /><br />
                            <span class="col1">Nr ewid.:</span>
                            <asp:TextBox ID="Nr_EwidTextBox" runat="server" class="nrew" Text='<%# Bind("Nr_Ewid") %>' MaxLength="50" /><br />
                            <span class="col1">Pracownik APT:</span>
                            <asp:CheckBox ID="cbAPT" runat="server" CssClass="check" Checked='<%# Bind("APT") %>'/>
                            --%>
                            <span class="col1">Numer karty RCP:</span>
                            <asp:TextBox ID="tbRcpId" runat="server" MaxLength="30" Text='<%# Bind("NrKarty") %>'/><br />                            

                            <asp:Label ID="lbLogin" class="col1" runat="server" Text="Login:" Visible='<%# IsEnabled(Eval("marker"),2) %>'/>
                            <asp:TextBox ID="tbLogin" runat="server" MaxLength="30" Text='<%# Bind("Login") %>' Visible='<%# IsEnabled(Eval("marker"),2) %>'/><br />

                            <span class="col1">Nowe hasło:</span>
                            <asp:TextBox ID="tbPass" runat="server" MaxLength="30" /><br />
                            <%--
                            <span class="col1">Powtórz hasło:</span>
                            <asp:TextBox ID="tbPass2" runat="server" MaxLength="30" /><br />
                            --%>
                            <span class="col1">&nbsp;</span>
                            <asp:Button ID="btSetPass" CssClass="control" Width="100" runat="server" CommandName="SetPass" Text="Ustaw hasło" />
                        </td>
                        <td class="col2">
                            <div class="title">Zatrudnienie</div>

                            <span class="col1">Grupa zatrudnienia:</span>
                            <asp:DropDownList ID="ddlGrupaZatr" runat="server" Enabled="false"/><br />
                            <span class="col1">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server" Enabled="false"/><br />

                            <span class="col1">Data zatrudnienia:</span>
                            <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataZatr") %>' Opis="yyyy-mm-dd" ReadOnly='<%# IsReadOnly(Eval("marker")) %>'/><br />
                            <span class="col1">Umowa do:</span>
                            <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataUmDo") %>' Opis="yyyy-mm-dd" ReadOnly='<%# IsReadOnly(Eval("marker")) %>'/><br />

                            <span class="col1">Jednostka macierzysta:</span>
                            <asp:DropDownList ID="ddlJednM" CssClass="jednM" runat="server" Enabled="false"/><br />

                            <asp:Label ID="lbStatusLabel" CssClass="col1" runat="server" Text="Status:"></asp:Label>
                            <asp:DropDownList ID="ddlStatus" CssClass="status" Enabled='<%# IsEnabled(Eval("marker")) %>' runat="server" /><br />

                            <span class="col1">Data zwolnienia:</span>
                            <uc1:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataZwol") %>' Opis="yyyy-mm-dd" ReadOnly='<%# IsReadOnly(Eval("marker")) %>'/>
                        </td>
                    </tr>
                </table>               
            </td>
            <td id="td2" runat="server" class="control1" >
                <asp:Button ID="UpdateButton" CssClass="control" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="evg" />
                <asp:Button ID="CancelButton" CssClass="control" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>        
    </EditItemTemplate>
    <SelectedItemTemplate>
        <tr id="tr3" runat="server" class="sit">
            <td id="tdSelect" runat="server" class="select" visible="false">
            </td>
            <td class="nazwisko_nrew">
                <asp:Label ID="Label2" runat="server" CssClass="nazwisko" Text='<%# Eval("NazwiskoImie") %>' /><br />
                <span class="line2 obok">
                    <asp:Label ID="lbRcpId" runat="server" CssClass="left kartarcp" Text='<%# Eval("NrKarty") %>' />
                    <asp:Label ID="Label3" runat="server" CssClass="right" Text='<%# Eval("Nr_Ewid") %>' />
                </span>
                <asp:HiddenField ID="hidObcy" runat="server" />
            </td>
            <td id="tdAPT" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbAPT" runat="server" CssClass="check" Enabled="false" Checked='<%# Eval("APT") %>'/>                
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Nazwa_Stan") %>' /><br />
                <asp:Label ID="Label5" runat="server" CssClass="line2" Text='<%# Eval("Rodzaj_Umowy") %>' /><br />
                <span class="line2">
                    <asp:Label ID="lbDataZatr" runat="server" Text='<%# Eval("DataZatr", "{0:d}") %>' /> - 
                    <asp:Label ID="lbDataUmDo" runat="server" Text='<%# Eval("DataUmDo", "{0:d}") %>' />
                </span>
            </td>
            
            <td id="td3" runat="server" class="jednM">
                <div id="paJednM" runat="server">
                    <asp:Label ID="Label6" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaM") %>' ToolTip='<%# Eval("NazwaStrumieniaM") %>' /> / 
                    <asp:Label ID="Label7" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiM") %>' ToolTip='<%# Eval("NazwaLiniiM") %>' /><br />
                    <asp:Label ID="Label8" runat="server" class="line2" Text='<%# Eval("KierownikM") %>' />                
                </div>
                <div id="paPrzJednM" runat="server" visible="false">
                    <asp:Label ID="lbStrumien" runat="server" CssClass="tooltip wholeline" Text='<%# Eval("SymbolStrumieniaM") %>' ToolTip='<%# Eval("NazwaStrumieniaM")%>' />
                    <asp:DropDownList ID="ddlJednM" CssClass="jednM" runat="server" 
                        OnChange="javascript:if (this.value == '') this.title=null; else this.title=this.options[this.selectedIndex].text;"/>
                </div>
            </td>

            <td id="td4" runat="server" class="jednA">
                <asp:Label ID="Label9" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaA") %>' ToolTip='<%# Eval("NazwaStrumieniaA") %>' /> / 
                <asp:Label ID="Label10" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiA") %>' ToolTip='<%# Eval("NazwaLiniiA") %>' /><br />
                <asp:Label ID="Label11" runat="server" class="line2" Text='<%# Eval("KierownikA") %>' />                
            </td>
            <td id="td5" runat="server" class="data jednA">
                <asp:Label ID="Label12" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /><br />
                <asp:Label ID="Label13" runat="server" CssClass="line2" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>

            <%--
            <td id="tdJednAod" runat="server" class="data jednA">
                <asp:Label ID="lbOd" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td id="tdJednAdo" runat="server" class="data jednA">
                <asp:Label ID="lbDo" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>

            OnSelectedIndexChanged="ddlJednM_SelectedIndexChanged" 
            
            <td id="tdRating" runat="server" class="num">
                <asp:Rating ID="Rating1" runat="server" ReadOnly="true"
                    CurrentRating="4"
                    MaxRating="5"
                    StarCssClass="rating"
                    FilledStarCssClass="rating_filled"
                    EmptyStarCssClass="rating_empty"
                    WaitingStarCssClass="rating_wating"
                    style="float: left;"
                    ></asp:Rating>
            </td>
            --%>
            <td id="td6" runat="server" class="num" visible="false">
                <asp:Label ID="Label14" runat="server" Text='<%# Eval("Ocena") %>' />
            </td>
            <%--
            <td id="tdAbsencja" runat="server" >
                <asp:Label ID="Id_StatusLabel" runat="server" Text='<%# Eval("Absencja") %>' />
            </td>
            --%>
            <td id="tdStatus" class="status" runat="server" >
                <asp:Label ID="StatusLabel" runat="server" Text='<%# PrepareStatus(Eval("Status")) %>' /><br />
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("marker") %>' />
                <asp:Label ID="lbDataZwol" runat="server" CssClass="line2" Text='<%# Eval("DataZwol", "{0:d}") %>' />
            </td>

            <td id="tdControl" runat="server" class="control1" >
                <asp:Button ID="Button1" runat="server" CommandName="Ocena" CommandArgument='<%# Eval("Id_Pracownicy") %>' Text="Oceń" Visible="false" />
                <asp:Button ID="Button2" runat="server" CommandName="Deleg" CommandArgument='<%# Eval("Id_Pracownicy") %>' Text="Oddeleguj" Visible="false" />
                <asp:Button ID="Button3" runat="server" CommandName="Przypisz" Text="Przypisz" Visible="false"/>

                <asp:Button ID="Button4" runat="server" CommandName="Edit" Text="Edycja" Visible="false"/>
                <%--
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false"/>
                --%>    
                <asp:Button ID="btEdit2" runat="server" CommandName="Edit2" Text="Edycja" Visible="false"/>
                <asp:Button ID="btUpdate2" runat="server" CommandName="Update2" Text="Zapisz" Visible="false"/>
                <asp:Button ID="btCancel2" runat="server" CommandName="Cancel2" Text="Anuluj" Visible="false"/>
            </td>
        </tr>
    </SelectedItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbPracownicy narrow">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server" colspan="2">
                    <table ID="itemPlaceholderContainer" runat="server" class="table">
                        <tr id="Tr2" runat="server" style="">
                            <th id="thSelect" runat="server" rowspan="2" visible="false"></th>
                            <th id="thPracownik" class="top" runat="server">
                                <asp:LinkButton ID="LinkButton1" Text="Pracownik" CommandArgument="NazwiskoImie" runat="server" CommandName="Sort" />
                            </th>
                                    <%--
                            <th id="thAPT" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="APT">
                                    APT</asp:LinkButton></th>
                                    --%>
                            <th id="thStanowisko" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton3" Text="Stanowisko" runat="server" CommandName="Sort" CommandArgument="Nazwa_Stan" /> / 
                                <asp:LinkButton ID="LinkButton4" Text="Umowa" runat="server" CommandName="Sort" CommandArgument="Rodzaj_Umowy" /><br />
                                <asp:LinkButton ID="LinkButton6" Text="Data zatrudnienia" runat="server" CommandName="Sort" CommandArgument="DataZatr" /> -
                                <asp:LinkButton ID="LinkButton17" Text="Umowa do" runat="server" CommandName="Sort" CommandArgument="DataUmDo" />
                            </th>
                            <th id="thJednM" colspan="1" class="top" runat="server">
                                    Jednostka macierzysta</th>
                            <th id="thJednA" colspan="2" class="top" runat="server">
                                    Jednostka aktualna</th>
                                    <%--
                            <th id="thOcena" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Ocena" ToolTip="Wielkość ważona">
                                    Ocena</asp:LinkButton></th>
                                    --%>
                            <th id="thStatus" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton18" Text="Status" runat="server" CommandName="Sort" CommandArgument="Status" /><br />
                                <asp:LinkButton ID="LinkButton16" Text="Data zwolnienia" runat="server" CommandName="Sort" CommandArgument="DataZwol" />
                            </th>
                            <th id="thControl" rowspan="2" runat="server" class="control1">
                                <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj pracownika" Visible="true"/>
                            </th>
                            
                                    <%--
                            <th id="thRating" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="Ocena">
                                    Ranking</asp:LinkButton></th>
                            <th id="thAbsencja" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Status">
                                    Absencja</asp:LinkButton></th>
                                    --%>
                        </tr>
                        <tr id="trThLine2" runat="server" class="bottom">
                            <th class="login">
                                <span class="obok">
                                    <span class="left">
                                        <asp:LinkButton ID="LinkButton15" Text="Nr karty RCP" runat="server" CommandName="Sort" CommandArgument="NrKarty" />
                                    </span>
                                    <span class="right">
                                        <asp:LinkButton ID="LinkButton2" Text="Nr ewid" runat="server" CommandName="Sort" CommandArgument="nrew" />
                                    </span>
                                </span>    
                            </th>
                            
                            <th id="thJednM2" runat="server">
                                <asp:LinkButton ID="LinkButton7" Text="Strumień" runat="server" CommandName="Sort" CommandArgument="NazwaStrumieniaM" /> / 
                                <asp:LinkButton ID="LinkButton8" Text="Linia" runat="server" CommandName="Sort" CommandArgument="NazwaLiniiM" /> /
                                <asp:LinkButton ID="LinkButton13" Text="Przełożony" runat="server" CommandName="Sort" CommandArgument="KierownikM" />
                            </th>
                            <th id="thJednA2" runat="server">
                                <asp:LinkButton ID="LinkButton9" Text="Strumień" runat="server" CommandName="Sort" CommandArgument="NazwaStrumieniaA" /> / 
                                <asp:LinkButton ID="LinkButton10" Text="Linia" runat="server" CommandName="Sort" CommandArgument="NazwaLiniiA" /> /
                                <asp:LinkButton ID="LinkButton14" Text="Przełożony" runat="server" CommandName="Sort" CommandArgument="KierownikA" />
                            </th>
                            <th id="thOd2" runat="server">
                                <asp:LinkButton ID="LinkButton11" Text="Od" runat="server" CommandName="Sort" CommandArgument="Od" /> /
                                <asp:LinkButton ID="LinkButton12" Text="Do" runat="server" CommandName="Sort" CommandArgument="Do" />
                            </th>
                            <%--
                            <th id="thOd2" runat="server">
                                <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="Od">
                                    Od</asp:LinkButton>
                            </th>
                            <th id="thDo2" runat="server">
                                <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="Do">
                                    Do</asp:LinkButton>
                            </th>
                            --%>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                    <uc1:LetterDataPager ID="LetterDataPager1" runat="server" Visible="false" />
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="10">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <h3>Pokaż na stronie:&nbsp;&nbsp;&nbsp;</h3>
                    <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" 
                        OnChange="javascript:showAjaxProgress();"
                        OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                        <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <span class="count">Pracowników:<asp:Label ID="lbPracCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                </td>
                <td class="right">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<%--
    UpdateCommand="UPDATE [Pracownicy] SET [Nazwisko] = @Nazwisko, [Imie] = @Imie, [Nr_Ewid] = @Nr_Ewid, [Id_Gr_Zatr] = @Id_Gr_Zatr, [Id_Stanowiska] = @Id_Stanowiska, [Id_Str_OrgM] = @Id_Str_OrgM, [IdKierownika] = @IdKierownika, [Status] = @Status, [APT] = @APT, [DataZatr] = @DataZatr, [DataUmDo] = @DataUmDo, [DataZwol] = @DataZwol WHERE [Id_Pracownicy] = @Id_Pracownicy"
    <UpdateParameters>
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nr_Ewid" Type="String" />
        <asp:Parameter Name="Id_Gr_Zatr" Type="Int32" />
        <asp:Parameter Name="Id_Stanowiska" Type="Int32" />
        <asp:Parameter Name="Id_Str_OrgM" Type="Int32" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Id_Pracownicy" Type="Int32" />
        <asp:Parameter Name="DataZatr" Type="DateTime" />
        <asp:Parameter Name="DataUmDo" Type="DateTime" />
        <asp:Parameter Name="DataZwol" Type="DateTime" />
    </UpdateParameters>
--%>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    DeleteCommand="DELETE FROM [Pracownicy] WHERE [Id_Pracownicy] = @Id_Pracownicy" 
    UpdateCommand="
        UPDATE KDR_RPP..wnPracownikAttributes SET [NrKarty] = @NrKarty WHERE [Pracownik_ID] = @Id_Pracownicy
        update KDR_RPP..PracownicyRPP set Nazwisko = @Nazwisko, Imie = @Imie, Nr_Ewid = @Nr_Ewid, Status = @Status where Id_Pracownicy = @Id_Pracownicy"
    InsertCommand="
        declare @id int
        select @id=isnull(min(Id_Pracownicy) - 1, -1000) from KDR_RPP..PracownicyRPP where Id_Pracownicy &lt; 0
        INSERT INTO KDR_RPP..PracownicyRPP (Id_Pracownicy, Nazwisko, Imie, Nr_Ewid, Status, marker, APT) VALUES (@id, @Nazwisko, @Imie, @id, @Status, 2, 0)
        insert into KDR_RPP..wnPracownikAttributes 
           (NrKarty, Pracownik_ID, Pass) values 
           (@NrKarty, @id, master.dbo.fn_varbintohexstr(HashBytes('SHA1', convert(nvarchar, convert(varchar,@id) + @Pass))))"     
    FilterExpression="(Nazwisko like '{0}%' or Imie like '{0}%' or Nr_Ewid like '{0}%')" 
            onfiltering="SqlDataSource1_Filtering" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidKierId" Name="IdKierownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidLiniaId" Name="IdLinii" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id_Pracownicy" Type="Int32" />
    </DeleteParameters>
    <FilterParameters>
        <asp:ControlParameter ControlID="tbSearch" Name="search" PropertyName="Text" />
    </FilterParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id_Pracownicy" Type="Int32" />
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nr_Ewid" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="NrKarty" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nr_Ewid" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="NrKarty" Type="String" />
        <asp:Parameter Name="Pass" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>



    </ContentTemplate>
</asp:UpdatePanel>






<%--
<!--
    InsertCommand="INSERT INTO KDR_RPP..PracownicyRPP ([Nazwisko], [Imie], [Nr_Ewid], [Id_Gr_Zatr], [Id_Stanowiska], [Id_Str_OrgM], [IdKierownika], [Status], [APT], [DataZatr], [DataUmDo], [DataZwol]) VALUES (@Nazwisko, @Imie, @Nr_Ewid, @Id_Gr_Zatr, @Id_Stanowiska, @Id_Str_OrgM, @IdKierownika, @Status, @APT, @DataZatr, @DataUmDo, @DataZwol)" >

"SELECT *, Nazwisko + ' ' + Imie as NazwiskoImie FROM [Pracownicy] ORDER BY [Nazwisko], [Imie]"


    <SelectParameters>
        <asp:ControlParameter ControlID="hidData" Name="data" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
    SelectCommand="select 
            P.Id_Pracownicy,
            P.Nazwisko + ' ' + P.Imie as NazwiskoImie,
            P.Nazwisko,
            P.Imie,
            P.Imie2, 
            P.Nr_Ewid,

            P.Id_Gr_Zatr,
            G.Rodzaj_Umowy,

            P.Id_Stanowiska,
            S.Nazwa_Stan,

            M.Id_Parent as IdStrumieniaM,
            MP.Symb_Jedn as SymbolStrumieniaM,
            MP.Nazwa_Jedn as NazwaStrumieniaM,
            MP.ID_Upr_Przel as IdUprPrzelStrumieniaM,
            MP.Id_Parent as IdParentaStrumieniaM,

            P.Id_Str_OrgM as IdLiniiM,
            M.Symb_Jedn as SymbolLiniiM,
            M.Nazwa_Jedn as NazwaLiniiM,
            M.ID_Upr_Przel as IdUprPrzelLiniiM,

            ISNULL(A.Id_Parent, M.Id_Parent) as IdStrumieniaA,
            ISNULL(AP.Symb_Jedn, MP.Symb_Jedn) as SymbolStrumieniaA,
            ISNULL(AP.Nazwa_Jedn, MP.Nazwa_Jedn) as NazwaStrumieniaA,
            ISNULL(AP.ID_Upr_Przel, MP.ID_Upr_Przel) as IdUprPrzelStrumieniaA,
            ISNULL(AP.Id_Parent, MP.Id_Parent) as IdParentaStrumieniaA,

            ISNULL(O.IdStruktury, P.Id_Str_OrgM) as IdLiniiA,
            ISNULL(A.Symb_Jedn, M.Symb_Jedn) as SymbolLiniiA,
            ISNULL(A.Nazwa_Jedn, MP.Nazwa_Jedn) as NazwaLiniiA,
            ISNULL(A.ID_Upr_Przel, M.ID_Upr_Przel) as IdUprPrzelLiniiA,
            O.Od, O.Do,

            0 as Ocena,

            P.Id_Status,
            T.Status

        from Pracownicy P
            left outer join GrZatr G on G.Id_Gr_Zatr = P.Id_Gr_Zatr
            left outer join Stanowiska S on S.Id_Stanowiska = P.Id_Stanowiska
            left outer join StrOrg M on M.Id_Str_Org = P.Id_Str_OrgM
            left outer join StrOrg MP on MP.Id_Str_Org = M.Id_Parent
            left outer join Oddelegowania O on O.IdPracownika = P.Id_Pracownicy and @data between O.Od and O.Do and O.Akceptacja = 1
            left outer join StrOrg A on A.Id_Str_Org = O.IdStruktury
            left outer join StrOrg AP on AP.Id_Str_Org = A.Id_Parent
            left outer join StatusPrac T on T.Id_Status_Prac = P.Id_Status
        order by NazwiskoImie"

-->
--%>


<%--

        <tr id="trLine" runat="server" class="sit">
            <td class="nazwisko_nrew">
                <asp:Label ID="NazwiskoLabel" runat="server" Text='<%# PrepNazwisko(Eval("NazwiskoImie")) %>' /><br />
                <asp:Label ID="Nr_EwidLabel" runat="server" CssClass="line2" Text='<%# Eval("Nr_Ewid") %>' />
            </td>
            <td>
                <asp:Label ID="lbStanowisko" runat="server" Text='<%# Eval("Nazwa_Stan") %>' /><br />
                <asp:Label ID="lbUmowa" runat="server" CssClass="line2" Text='<%# Eval("Rodzaj_Umowy") %>' />
            </td>
            
            <td id="tdJednM" runat="server" class="jednM">
                <asp:Label ID="lbStrumienM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaM") %>' ToolTip='<%# Eval("NazwaStrumieniaM") %>' /> / 
                <asp:Label ID="lbLiniaM" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiM") %>' ToolTip='<%# Eval("NazwaLiniiM") %>' /><br />
                <asp:Label ID="lbKierownikM" runat="server" class="line2" Text='<%# Eval("KierownikM") %>' />                
            </td>

            <td id="tdJednA" runat="server" class="jednA">
                <asp:Label ID="lbStrumienA" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolStrumieniaA") %>' ToolTip='<%# Eval("NazwaStrumieniaA") %>' /> / 
                <asp:Label ID="lbLiniaA" runat="server" CssClass="tooltip" Text='<%# Eval("SymbolLiniiA") %>' ToolTip='<%# Eval("NazwaLiniiA") %>' /><br />
                <asp:Label ID="lbKierownikA" runat="server" class="line2" Text='<%# Eval("KierownikA") %>' />                
            </td>
            <td id="tdJednAod" runat="server" class="data jednA">
                <asp:Label ID="lbOd" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td id="tdJednAdo" runat="server" class="data jednA">
                <asp:Label ID="lbDo" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>

            <td id="tdOcena" runat="server" class="num" visible="false">
                <asp:Label ID="lbOcena" runat="server" Text='<%# Eval("Ocena") %>' />
            </td>
            <td id="tdAbsencja" runat="server" >
                <asp:Label ID="Id_StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td id="tdControl" runat="server" class="control1" >
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" Visible="false"/>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false"/>
                
                <asp:Button ID="btOcen" runat="server" CommandName="Ocena" CommandArgument='<%# Eval("Id_Pracownicy") %>' Text="Oceń" Visible="false" />
                <asp:Button ID="btOddeleguj" runat="server" CommandName="Deleg" CommandArgument='<%# Eval("Id_Pracownicy") %>' Text="Oddeleguj" Visible="false" />
                <asp:Button ID="btPrzypisz" runat="server" CommandName="Przypisz" Text="Przypisz" Visible="false"/>
            </td>
        </tr>
--%>