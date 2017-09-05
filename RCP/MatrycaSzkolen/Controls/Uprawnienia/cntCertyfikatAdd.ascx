<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCertyfikatAdd.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Uprawnienia.cntCertyfikatAdd" %>
<%@ Register src="~/MatrycaSzkolen/Controls/Uprawnienia/cntCertyfikatHeader.ascx" tagname="cntCertyfikatHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>

<asp:HiddenField ID="hidCertId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidTypId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidPracId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidUprId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidPola" runat="server" Visible="false"/>

<asp:HiddenField ID="hidAktualnyVisible" runat="server" Visible="false" Value="0"/>

<asp:HiddenField ID="hidIdGrupy" runat="server" Visible="false" />

<script type="text/javascript">
    function <%=ClientID%>_click(ddl, tbId) {
        var tb = document.getElementById(tbId);
        if (tb != null) {
            var par = ddl.value.split('|');
            if (par.length >= 2) {
                tb.value = par[1];
                //tb.focus();
                //alert(par[1]);
            }
        }        
    }

    var <%=ClientID%>_timer = null;
    
    function <%=ClientID%>_setchange(tbId, ddlId) {
        startSearch3(<%=ClientID%>_timer, tbId, 250, function(search){
            var found = false;
            var ddl = document.getElementById(ddlId);
            if (ddl != null) {            
                //wnDebug('c');            
                for (var i = 0; i < ddl.length; i++) {
                    var optA = ddl.options[i].value.split('|');
                    if (optA.length >= 2) {
                        var opt = optA[1];
                        if (opt.startsWith(search)) {
                            //wnDebug('!');
                            ddl.selectedIndex = i;
                            found = true;
                            break;
                        }
                    }
                }
                if (!found) {
                    ddl.selectedIndex = 0;
                    //wnDebug('x');
                } else {
                    //wnDebug('y');
                }
            }
        });
    }
</script>

<div id="paCertyfikatAdd" runat="server" class="cntCertyfikat cntCertyfikatAdd">
    <%--
    <div id="paInfo" runat="server" class="paInfo" >
        <h3><asp:Label ID="Label1" runat="server" Text="Uprawnienie"></asp:Label></h3>
        <asp:Label ID="lbUprawnienie" runat="server" /><br />&nbsp;

        <h3><asp:Label ID="Label2" runat="server" Text="Dane pracownika"></asp:Label></h3>
        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Pracownik:"></asp:Label>
        <asp:Label ID="lbPracownik" runat="server" /><br />
        <asp:Label ID="Label6" runat="server" CssClass="label" Text="Numer ewidencyjny:"></asp:Label>
        <asp:Label ID="lbNrEw" CssClass="value" runat="server" ></asp:Label><br />&nbsp;

        <h3><asp:Label ID="Label4" runat="server" Text="Certyfikaty"></asp:Label></h3>
    </div>
    --%>
        
    <uc1:cntCertyfikatHeader ID="cntCertyfikatHeader" runat="server" Visible="false"/>
        
    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" oncheckedchanged="CheckBox1_CheckedChanged" Text="Pokaż wszystkie pola" Visible="false"/>
    
    <div id="paFilter" runat="server" visible="false" class="filter">
        <asp:Label ID="Label5" runat="server" Text="Pokaż zmienione w dniu:"></asp:Label>
        <uc2:DateEdit ID="deNadzien" runat="server" AutoPostBack="true" OnDateChanged="deNaDzien_Changed"/>    
        <asp:Button ID="btShow" runat="server" CssClass="button75" Text="Pokaż" onclick="btShow_Click" />
    </div>
    <asp:ListView ID="lvCertyfikat" runat="server" DataSourceID="SqlDataSource3" 
        DataKeyNames="Id" InsertItemPosition="none" 
        ondatabound="lvCertyfikat_DataBound" 
        onlayoutcreated="lvCertyfikat_LayoutCreated" 
        onitemcreated="lvCertyfikat_ItemCreated" 
        onitemdeleted="lvCertyfikat_ItemDeleted" 
        oniteminserted="lvCertyfikat_ItemInserted" 
        onitemupdated="lvCertyfikat_ItemUpdated" 
        onitemdatabound="lvCertyfikat_ItemDataBound" 
        oniteminserting="lvCertyfikat_ItemInserting" 
        onitemupdating="lvCertyfikat_ItemUpdating" 
        onitemdeleting="lvCertyfikat_ItemDeleting">
        <ItemTemplate>
            <tr class="it">
                <td id="tdPracownik" class="pracownik" runat="server" visible='<%# IsEdit %>' >
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("Pracownik") %>' />
                    <asp:HiddenField ID="hidKadryId" runat="server" Visible="false" Value='<%# Eval("KadryId") %>'/>
                </td>                
                <td id="tdNrEw" class="nrew" runat="server" visible='<%# IsEdit %>' >
                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("KadryId") %>' />
                </td>                

                <td id="td18" class="symbol" runat="server" visible='<%# IsVisible(18) %>' >
                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("Symbol") %>' />
                </td>                

                <td id="Td12" class="nazwa" runat="server" visible='<%# IsVisible(16) %>' >
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("NazwaCertyfikatu") %>' />
                </td>
                <td id="Td15" class="numer" runat="server" visible='<%# IsVisible(2) %>' >
                    <asp:Label ID="NumerLabel" runat="server" Text='<%# Eval("Numer") %>' />
                </td>                
                <td id="Td16" class="warunki" runat="server" visible='<%# IsVisible(17) %>' >
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("DodatkoweWarunki") %>' />
                </td>

                <td id="Td14" class="kategoria" runat="server" visible='<%# IsVisible(5) %>' >
                    <asp:Label ID="KategoriaLabel" runat="server" Text='<%# Eval("Kategoria") %>' />
                </td>

                <td id="Td1" class="data" runat="server" visible='<%# IsVisible(14) %>'>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("DataRozpoczecia", "{0:d}") %>' />
                </td>
                <td id="Td11" class="data" runat="server" visible='<%# IsVisible(15) %>'>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("DataZakonczenia", "{0:d}") %>' />
                </td>

                <td class="data" runat="server" visible='<%# IsVisible(6) %>'>
                    <asp:Label ID="DataZdobyciaUprawnienLabel" runat="server" Text='<%# Eval("DataZdobyciaUprawnien", "{0:d}") %>' />
                </td>
                <td class="data" runat="server" visible='<%# IsVisible(3) %>'>
                    <asp:Label ID="DataWaznosciLabel" runat="server" Text='<%# Eval("DataWaznosci", "{0:d}") %>' />
                </td>
                <td class="data" runat="server" visible='<%# IsVisible(7) %>'>
                    <asp:Label ID="DataWaznosciPsychotestowLabel" runat="server" Text='<%# Eval("DataWaznosciPsychotestow", "{0:d}") %>' />
                </td>
                <td class="data" runat="server" visible='<%# IsVisible(8) %>'>
                    <asp:Label ID="DataWaznosciBadanLekarskichLabel" runat="server" Text='<%# Eval("DataWaznosciBadanLekarskich", "{0:d}") %>' />
                </td>
                <td class="check" runat="server" visible='<%# IsVisible(10) %>'>
                    <asp:CheckBox ID="UmowaLojalnosciowaCheckBox" runat="server" Checked='<%# Eval("UmowaLojalnosciowa") %>' Enabled="false" />
                </td>
                <td class="data" runat="server" visible='<%# IsVisible(9) %>'>
                    <asp:Label ID="DataWaznosciUmowyLabel" runat="server" Text='<%# Eval("DataWaznosciUmowy", "{0:d}") %>' />
                </td>
                
                <td id="Td4" class="check" runat="server" visible='<%# IsAktualnyVisible %>'>   <%-- wyłączyć po testach --%>
                    <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Eval("Aktualny") %>' Enabled="false" />
                </td>
   
                <td class="uwagi" runat="server" visible='<%# IsVisible(13) %>'>
                    <asp:Label ID="UwagiLabel" runat="server" Text='<%# Eval("Uwagi") %>' />
                </td>
                <td id="tdControl" runat="server" class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="" class="table0">
                <tr>
                    <td>
                        <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                        <asp:Button ID="btNewRecord" runat="server" CssClass="button100" CommandName="NewRecord" Text="Dodaj" />
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr class="iit">



<%--                <td id="tdNrEw" runat="server" class="nrew" visible="false">
                    <asp:TextBox ID="tbNrEw" runat="server" MaxLength="20" CssClass="textbox width80" />
                
                </td>
                <td id="tdPracownik" runat="server" class="pracownik" visible="false">
                    <asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownicy" DataTextField="Pracownik" DataValueField="Id" CssClass="width300">
                    </asp:DropDownList>
                
                </td>
            
                <td id="td18" runat="server" class="symbol">
                </td>                

                <td id="td16" runat="server" class="nazwa">
                    <asp:TextBox ID="tbNazwa" runat="server" MaxLength="200" Text='<%# Bind("NazwaCertyfikatu") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbNazwa" ValidationGroup="ivg" ErrorMessage="Błąd" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>

                <td id="td2" runat="server" class="numer">
                    <asp:TextBox ID="NumerTextBox" runat="server" MaxLength="200" Text='<%# Bind("Numer") %>' />
                </td>

                <td id="td17" runat="server" class="warunki">
                    <asp:TextBox ID="tbDodatkoweWarunki" runat="server" MaxLength="200" Text='<%# Bind("DodatkoweWarunki") %>' />
                </td>

                <td id="td5" runat="server" class="kategoria">
                    <asp:TextBox ID="KategoriaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Kategoria") %>' />
                </td>

                <td id="td14" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit6" runat="server" Date='<%# Bind("DataRozpoczecia") %>' ValidationGroup="ivg" AutoPostBack="true" OnDateChanged="deRozp_Changed"/>
                </td>
                <td id="td15" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit7" runat="server" Date='<%# Bind("DataZakonczenia") %>' AutoPostBack="true" OnDateChanged="deRozp_Changed"/>
                </td>
                
                <td id="td6" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataZdobyciaUprawnien") %>' />
                </td>
                <td id="td3" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWaznosci") %>' />
                </td>
                <td id="td7" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataWaznosciPsychotestow") %>' />
                </td>
                <td id="td8" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit4" runat="server" Date='<%# Bind("DataWaznosciBadanLekarskich") %>' />
                </td>
                <td id="td10" runat="server" class="check">
                    <asp:CheckBox ID="UmowaLojalnosciowaCheckBox" runat="server" Checked='<%# Bind("UmowaLojalnosciowa") %>' ValidationGroup="ivg"/>
                </td>
                <td id="td9" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit5" runat="server" Date='<%# Bind("DataWaznosciUmowy") %>' />
                </td>
   
                <td id="Td19" runat="server" class="check" visible='<%# IsAktualnyVisible %>'>
                    <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Bind("Aktualny") %>' />
                </td>
   
                <td id="td13" runat="server" class="uwagi">
                    <asp:TextBox ID="UwagiTextBox" runat="server" MaxLength="500" Text='<%# Bind("Uwagi") %>' />
                </td>
--%>




<%--
                <td colspan="<%= GetEditColSpan() %>" class="col1">
                <td colspan="10" class="col1" >
--%>
                <td colspan="<%= GetEditColSpan(10,8) %>" class="col1">
                    <table class="edit">
                        <tr>
                            <td class="coled1">
                                <div id="paPracownik" runat="server" visible='<%# GetIsEdit() %>'>
                                    <span class="label">Numer ewidencyjny, pracownik:</span>
                                    <asp:TextBox ID="tbNrEw" runat="server" MaxLength="20" CssClass="textbox width80" />
                                    <asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownicy" DataTextField="Text" DataValueField="Value" CssClass="width300" >
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddlPracownik" ValidationGroup="ivg" ErrorMessage="Błąd" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <br />
                                </div>
                                <span class="label">Symbol szkolenia, nazwa:</span>
                                <asp:DropDownList ID="ddlSymbol" runat="server" DataSourceID="dsSymbole" DataTextField="Text" DataValueField="Value" CssClass="width80" OnDataBound="ddlSymbol_DataBound">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddlSymbol" ValidationGroup="ivg" ErrorMessage="Błąd" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
<%--
                                <asp:DropDownList ID="ddlSymbol" runat="server" DataSourceID="dsSymbole" DataTextField="Uprawnienie" DataValueField="Id" CssClass="width80">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddlSymbol" ValidationGroup="ivg" ErrorMessage="Błąd" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
--%>
                                <asp:TextBox ID="tbNazwa" runat="server" MaxLength="200" Text='<%# Bind("NazwaCertyfikatu") %>' CssClass="textbox width300" />
                                <asp:RequiredFieldValidator ControlToValidate="tbNazwa" ValidationGroup="ivg" ErrorMessage="Błąd" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                <br />                                                            
                                <span class="label">Numer:</span>                                    
                                <asp:TextBox ID="NumerTextBox" runat="server" MaxLength="200" Text='<%# Bind("Numer") %>' CssClass="textbox full"/>
                                <br />
                                <span class="label">Dodatkowe warunki:</span>                                    
                                <asp:TextBox ID="tbDodatkoweWarunki" runat="server" MaxLength="200" Text='<%# Bind("DodatkoweWarunki") %>' CssClass="textbox full"/>
                                <br />
                                <span class="label">Data rozpoczęcia - zakończenia:</span>                                                                    
                                <uc2:DateEdit ID="DateEdit6" runat="server" Date='<%# Bind("DataRozpoczecia") %>' ValidationGroup="ivg" AutoPostBack="true" OnDateChanged="deRozpInsert_Changed"/> -                                
                                <uc2:DateEdit ID="DateEdit7" runat="server" Date='<%# Bind("DataZakonczenia") %>' AutoPostBack="true" OnDateChanged="deRozpInsert_Changed"/>
                                <br />
                                <span class="label">Data ważności:</span>                                                                    
                                <uc2:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWaznosci") %>' ValidationGroup="ivg" />
                                <br />
                                <span class="label">Uwagi:</span>                                                                    
                                <asp:TextBox ID="UwagiTextBox" runat="server" MaxLength="500" Text='<%# Bind("Uwagi") %>' CssClass="textbox full"/>            

                                <asp:TextBox ID="KategoriaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Kategoria") %>' Visible="false"/>
                                <uc2:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataZdobyciaUprawnien") %>' Visible="false"/>
                                <uc2:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataWaznosciPsychotestow") %>' Visible="false"/>
                                <uc2:DateEdit ID="DateEdit4" runat="server" Date='<%# Bind("DataWaznosciBadanLekarskich") %>' Visible="false"/>
                                <asp:CheckBox ID="UmowaLojalnosciowaCheckBox" runat="server" Checked='<%# Bind("UmowaLojalnosciowa") %>' ValidationGroup="ivg" Visible="false"/>
                                <uc2:DateEdit ID="DateEdit5" runat="server" Date='<%# Bind("DataWaznosciUmowy") %>' Visible="false"/>
                                <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Bind("Aktualny") %>' Visible="false"/>
                            </td>                
                        </tr>
                    </table>
                </td>                
                <td class="control">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Cancel" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td id="tdPracownik" class="pracownik" runat="server" visible='<%# IsEdit %>' >
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("Pracownik") %>' />
                    <asp:HiddenField ID="hidKadryId" runat="server" Visible="false" Value='<%# Eval("KadryId") %>'/>
                </td>                
                <td id="tdNrEw" class="nrew" runat="server" visible='<%# IsEdit %>' >
                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("KadryId") %>' />
                </td>                
                <td colspan="8" class="col1">
                    <table class="edit">
                        <tr>
                            <td class="coled1">
                                <asp:HiddenField ID="hidPracId" runat="server" Value='<%# Eval("IdPracownika") %>'/>
                                <asp:HiddenField ID="hidUprId" runat="server" Value='<%# Eval("IdUprawnienia") %>'/>                            
<%--
                                <span class="label">Numer ewidencyjny, pracownik:</span>
                                <asp:Label ID="Label9" runat="server" Text='<%# Eval("KadryId") %>'></asp:Label>
                                <asp:Label ID="Label10" runat="server" Text='<%# Eval("Pracownik") %>'></asp:Label>
                                <br />
--%>
                                <span class="label">Symbol szkolenia, nazwa:</span>
<%--
                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("Symbol") %>'></asp:Label>
--%>
                                <asp:DropDownList ID="ddlSymbol" runat="server" DataSourceID="dsSymbole" DataTextField="Text" DataValueField="Value" CssClass="width80">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ControlToValidate="ddlSymbol" ValidationGroup="evg" ErrorMessage="Błąd" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>

                                <asp:TextBox ID="tbNazwa" runat="server" MaxLength="200" Text='<%# Bind("NazwaCertyfikatu") %>' CssClass="textbox width300" />
                                <asp:RequiredFieldValidator ControlToValidate="tbNazwa" ValidationGroup="evg" ErrorMessage="Błąd" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                <br />                                                            
                                <span class="label">Numer:</span>                                    
                                <asp:TextBox ID="NumerTextBox" runat="server" MaxLength="200" Text='<%# Bind("Numer") %>' CssClass="textbox full"/>
                                <br />
                                <span class="label">Dodatkowe warunki:</span>                                    
                                <asp:TextBox ID="tbDodatkoweWarunki" runat="server" MaxLength="200" Text='<%# Bind("DodatkoweWarunki") %>' CssClass="textbox full"/>
                                <br />
                                <span class="label">Data rozpoczęcia - zakończenia:</span>                                                                    
                                <uc2:DateEdit ID="DateEdit6" runat="server" Date='<%# Bind("DataRozpoczecia") %>' ValidationGroup="evg" AutoPostBack="true" OnDateChanged="deRozpUpdate_Changed"/> -                                
                                <uc2:DateEdit ID="DateEdit7" runat="server" Date='<%# Bind("DataZakonczenia") %>' AutoPostBack="true" OnDateChanged="deRozpUpdate_Changed"/>
                                <br />
                                <span class="label">Data ważności:</span>                                                                    
                                <uc2:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWaznosci") %>' ValidationGroup="evg" />
                                <br />
                                <span class="label">Uwagi:</span>                                                                    
                                <asp:TextBox ID="UwagiTextBox" runat="server" MaxLength="500" Text='<%# Bind("Uwagi") %>' CssClass="textbox full"/>            

                                <asp:TextBox ID="KategoriaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Kategoria") %>' Visible="false"/>
                                <uc2:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataZdobyciaUprawnien") %>' Visible="false"/>
                                <uc2:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataWaznosciPsychotestow") %>' Visible="false"/>
                                <uc2:DateEdit ID="DateEdit4" runat="server" Date='<%# Bind("DataWaznosciBadanLekarskich") %>' Visible="false"/>
                                <asp:CheckBox ID="UmowaLojalnosciowaCheckBox" runat="server" Checked='<%# Bind("UmowaLojalnosciowa") %>' ValidationGroup="evg" Visible="false"/>
                                <uc2:DateEdit ID="DateEdit5" runat="server" Date='<%# Bind("DataWaznosciUmowy") %>' Visible="false"/>
                                <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Bind("Aktualny") %>' Visible="false"/>
                            </td>                
                        </tr>
                    </table>
                </td>                
            
            
            
            
            
            
<%--            
                <td id="tdNrEw" runat="server" class="nrew" visible="false">
                
                </td>
                <td id="tdPracownik" runat="server" class="pracownik" visible="false">
                
                </td>
            
                <td id="td18" runat="server" class="symbol">
                    <asp:Label ID="lbSymbol" runat="server" Text='<%# Eval("Symbol") %>' />
                </td>                

                <td id="td16" runat="server" class="nazwa">
                    <asp:TextBox ID="tbNazwa" runat="server" MaxLength="200" Text='<%# Bind("NazwaCertyfikatu") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbNazwa" ValidationGroup="evg" ErrorMessage="Błąd" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>

                <td id="td2" runat="server" class="numer">
                    <asp:TextBox ID="NumerTextBox" runat="server" MaxLength="200" Text='<%# Bind("Numer") %>' />
< %--
                    <asp:RequiredFieldValidator ControlToValidate="NumerTextBox" ValidationGroup="evg" ErrorMessage="Error" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
--% >
                </td>

                <td id="td17" runat="server" class="warunki">
                    <asp:TextBox ID="tbDodatkoweWarunki" runat="server" MaxLength="200" Text='<%# Bind("DodatkoweWarunki") %>' />
                </td>

                <td id="td5" runat="server" class="kategoria">
                    <asp:TextBox ID="KategoriaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Kategoria") %>' />
                </td>

                <td id="td14" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit6" runat="server" Date='<%# Bind("DataRozpoczecia") %>' ValidationGroup="evg" AutoPostBack="true" OnDateChanged="deRozp_Changed"/>
                </td>
                <td id="td15" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit7" runat="server" Date='<%# Bind("DataZakonczenia") %>' AutoPostBack="true" OnDateChanged="deRozp_Changed"/>
                </td>
                
                <td id="td6" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataZdobyciaUprawnien") %>' />
                </td>
                <td id="td3" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWaznosci") %>' />
<% --                    <br />
                    <asp:CheckBox ID="cbUnlimited" runat="server" Text="Beztermiowo" Checked='<%# Bind("Unlimited") %>' />
-- %>
                </td>
                <td id="td7" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataWaznosciPsychotestow") %>' />
                </td>
                <td id="td8" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit4" runat="server" Date='<%# Bind("DataWaznosciBadanLekarskich") %>' />
                </td>
                <td id="td10" runat="server" class="check">
                    <asp:CheckBox ID="UmowaLojalnosciowaCheckBox" runat="server" Checked='<%# Bind("UmowaLojalnosciowa") %>' ValidationGroup="evg"/>
                </td>
                <td id="td9" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit5" runat="server" Date='<%# Bind("DataWaznosciUmowy") %>' />
                </td>
   
                <td runat="server" class="check" visible='<%# IsAktualnyVisible %>'>
                    <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Bind("Aktualny") %>' />
                </td>
   
                <td id="td13" runat="server" class="uwagi">
                    <asp:TextBox ID="UwagiTextBox" runat="server" MaxLength="500" Text='<%# Bind("Uwagi") %>' />
                </td>
--%>
                <td class="control">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
<%--
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
--%>
                </td>
            </tr>
        </EditItemTemplate>
        <LayoutTemplate>
            <table runat="server" class="xnarrow xtbCertyfikaty">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="" name="report" class="table">
                            <tr runat="server" style="">
                                <th id="thPracownik" runat="server" visible="false">
                                    <asp:LinkButton ID="LinkButton16" runat="server" CommandName="Sort" CommandArgument="Pracownik" Text="Pracownik"></asp:LinkButton></th>
                                <th id="thNrEw" runat="server" visible="false">
                                    <asp:LinkButton ID="LinkButton17" runat="server" CommandName="Sort" CommandArgument="KadryId" Text="Nr ew."></asp:LinkButton></th>

                                <th id="th18" runat="server" >
                                    <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="Symbol" Text="Symbol"></asp:LinkButton></th>

                                <th id="th16" runat="server" >
                                    <asp:LinkButton ID="LinkButton13" runat="server" CommandName="Sort" CommandArgument="NazwaCertyfikatu" Text="Nazwa"></asp:LinkButton></th>

                                <th id="th2" runat="server" >
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Numer" Text="Numer zaświadczenia"></asp:LinkButton></th>
                                <th id="th17" runat="server" >
                                    <asp:LinkButton ID="LinkButton14" runat="server" CommandName="Sort" CommandArgument="DodatkoweWarunki" Text="Dodatkowe warunki"></asp:LinkButton></th>
                                    
                                <th id="th5" runat="server" >
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Kategoria" Text="Kategoria"></asp:LinkButton></th>

                                <th id="th14" runat="server" >
                                    <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="DataRozpoczecia" Text="Data rozpoczęcia"></asp:LinkButton></th>
                                <th id="th15" runat="server" >
                                    <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="DataZakonczenia" Text="Data zakończenia"></asp:LinkButton></th>

                                <th id="th6" runat="server" >
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="DataZdobyciaUprawnien" Text="Data Uzyskania"></asp:LinkButton></th>
                                <th id="th3" runat="server" >
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="DataWaznosci" Text="Data ważnosci"></asp:LinkButton></th>
                                
                                <th id="th7" runat="server" >
                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="DataWaznosciPsychotestow" Text="Data ważności psychotestów"></asp:LinkButton></th>
                                <th id="th8" runat="server" >
                                    <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="DataWaznosciBadanLekarskich" Text="Data ważności badań lekarskich"></asp:LinkButton></th>

                                <th id="th10" runat="server" >
                                    <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="UmowaLojalnosciowa" Text="Umowa lojalnościowa"></asp:LinkButton>
                                    </th>
                                <th id="th9" runat="server" >
                                    <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="DataWaznosciUmowy" Text="Data ważności umowy" ></asp:LinkButton></th>

                                <th id="thAktualny" runat="server" visible='<%# IsAktualnyVisible %>'>
                                    <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Aktualny" Text="Aktualny"></asp:LinkButton></th>

                                <th id="th13" runat="server" >
                                    <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Uwagi" Text="Uwagi"></asp:LinkButton></th>
                                    
                                <th id="thControl" runat="server" class="control">
                                    &nbsp;
                                    <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />
                                    &nbsp;
                                </th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="pager">
                    <td class="left">
                        <div class="pager1">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="10" >
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="|◄" PreviousPageText="◄" />
                                    <asp:NumericPagerField ButtonType="Link" />
                                    <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="►" LastPageText="►|" />
                                </Fields>
                            </asp:DataPager>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <span class="count"><asp:Literal ID="lbCountLabel" runat="server" Text="Ilość:"></asp:Literal><asp:Label ID="lbCount" runat="server" /></span>
                        </div>
                    </td>
                    <td class="right">                  
<%--
                        <asp:Label ID="lbPageSize" runat="server" CssClass="count" Text="Pokaż na stronie:"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlLines" runat="server" >
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
--%>                    
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>



    <asp:SqlDataSource ID="dsPracownicy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"         
        SelectCommand="
select 'wybierz ...' as Text, null as Value, 1 as Sort
union all        
select case S.Sort when 3 then '* ' else '' end +
P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')','') as Text, convert(varchar,P.Id) + '|' + ISNULL(P.KadryId,'') as Value, S.Sort
from VPrzypisaniaNaDzis P 
outer apply (select case when P.KierId is null and P.Status != 1 then 3 else 2 end as Sort) S
where P.KadryId &lt; 80000 and P.Status != -2 
--where S.Sort = 2
order by Sort, Text
    "/> 

    <asp:SqlDataSource ID="dsSymbole" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"         
        SelectCommand="
select 'wybierz ...' as Text, null as Value, 1 as Sort, 0 as Kolejnosc
union all        
select case when Aktywne = 0 then '* ' else '' end +
U.Symbol + ' - ' + U.Nazwa as Text,
convert(varchar,U.Id) + '|' + U.Nazwa as Value,
case when Aktywne = 0 then 3 else 2 end as Sort, U.Kolejnosc
from Uprawnienia U
where /*U.KwalifikacjeId = 23 and*/ U.IdGrupy = @grupa
order by Sort, Text
    " >
    <SelectParameters>
        <asp:ControlParameter Name="grupa" Type="Int32" ControlID="hidIdGrupy" PropertyName="Value" />
    </SelectParameters>    
</asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" oninserted="SqlDataSource3_Inserted" CancelSelectOnNullParameter="false"
        SelectCommand="        
select 
--:CUT
T.Symbol, C.*, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as IdPracownika, P.KadryId 
/*:CSV
P.Nazwisko + ' ' + P.Imie [Pracownik],
P.KadryId [Nr ewid.],
T.Symbol, 
--T.Nazwa [Nazwa typu], 
C.NazwaCertyfikatu [Nazwa szkolenia], 
C.Numer [Numer zaświadczenia],
C.DodatkoweWarunki [Dodatkowe warunki],
C.DataRozpoczecia [Data rozpoczęcia],
C.DataZakonczenia [Data zakończenia],
C.DataWaznosci [Data ważności],
C.Uwagi [Uwagi]
*/
from [Certyfikaty] C 
left join Pracownicy P on P.Id = C.IdPracownika
left join Uprawnienia T on T.Id = C.IdUprawnienia
where T.Typ = @typ 
and (
    @nadzien is null or
    dbo.getdate(C.DataModyfikacji) = dbo.getdate(@nadzien)
    )
and (
    @IdPracownika is null or 
    C.IdPracownika = @IdPracownika 
    )
and (
    @IdUprawnienia is null or 
    C.IdUprawnienia = @IdUprawnienia
    )
order by C.Aktualny desc, C.DataWaznosci
" 
        DeleteCommand="
DELETE FROM [Certyfikaty] WHERE [Id] = @Id
--update Certyfikaty set Aktualny = 0 where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia
--update Certyfikaty set Aktualny = 1 where Id = (select top 1 Id from Certyfikaty where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia order by ISNULL(DataWaznosci,'20990909') desc, Id desc)
" 
        InsertCommand="
if @IdUprawnienia = 8 and @DataWaznosci is null        
   set @DataWaznosci = @DataWaznosciUmowy
        
INSERT INTO [Certyfikaty] ([IdUprawnienia], [IdPracownika], [Numer], [DataWaznosci], [Kategoria], [DataZdobyciaUprawnien], [DataWaznosciPsychotestow], [DataWaznosciBadanLekarskich], [DataWaznosciUmowy], [UmowaLojalnosciowa], [Aktualny], [Uwagi], DataRozpoczecia, DataZakonczenia, NazwaCertyfikatu, DodatkoweWarunki, IdAutora, IdAutoraZast) 
VALUES (@IdUprawnienia, @IdPracownika, @Numer, @DataWaznosci, @Kategoria, 

--@DataZdobyciaUprawnien, 
ISNULL(@DataZakonczenia, @DataRozpoczecia),

@DataWaznosciPsychotestow, @DataWaznosciBadanLekarskich, @DataWaznosciUmowy, @UmowaLojalnosciowa, @Aktualny, @Uwagi, @DataRozpoczecia, @DataZakonczenia, @NazwaCertyfikatu, @DodatkoweWarunki, @IdAutora, @IdAutoraZast)

--update Certyfikaty set Aktualny = 0 where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia
--update Certyfikaty set Aktualny = 1 where Id = (select top 1 Id from Certyfikaty where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia order by ISNULL(DataWaznosci,'20990909') desc, Id desc)

set @IdCertyfikatu = (select @@Identity)
" 
        UpdateCommand="
if @IdUprawnienia = 8 and @DataWaznosci is null        
   set @DataWaznosci = @DataWaznosciUmowy

UPDATE [Certyfikaty] SET [IdUprawnienia] = @IdUprawnienia, [IdPracownika] = @IdPracownika, [Numer] = @Numer, [DataWaznosci] = @DataWaznosci, [Kategoria] = @Kategoria, 

--[DataZdobyciaUprawnien] = @DataZdobyciaUprawnien, 
[DataZdobyciaUprawnien] = ISNULL(@DataZakonczenia, @DataRozpoczecia), 

[DataWaznosciPsychotestow] = @DataWaznosciPsychotestow, [DataWaznosciBadanLekarskich] = @DataWaznosciBadanLekarskich, [DataWaznosciUmowy] = @DataWaznosciUmowy, [UmowaLojalnosciowa] = @UmowaLojalnosciowa, 
[Aktualny] = @Aktualny, [Uwagi] = @Uwagi 
,DataRozpoczecia = @DataRozpoczecia, DataZakonczenia = @DataZakonczenia, NazwaCertyfikatu = @NazwaCertyfikatu, DodatkoweWarunki = @DodatkoweWarunki

,DataModyfikacji = @DataModyfikacji, IdAutora = @IdAutora, IdAutoraZast = @IdAutoraZast 

WHERE [Id] = @Id

--update Certyfikaty set Aktualny = 0 where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia
--update Certyfikaty set Aktualny = 1 where Id = (select top 1 Id from Certyfikaty where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia order by ISNULL(DataWaznosci,'20990909') desc, Id desc)
    ">

<%--
[DataWaznosciSet], 
@DataWaznosciSet, 
[ImportId] = @ImportId, [DataWaznosciSet] = @DataWaznosciSet, 
[DataZdobyciaUprawnienOk] = @DataZdobyciaUprawnienOk, [DataWaznosciPsychotestowOk] = @DataWaznosciPsychotestowOk, [DataWaznosciBadanLekarskichOk] = @DataWaznosciBadanLekarskichOk, [UmowaLojalnosciowaOk] = @UmowaLojalnosciowaOk, 
            <asp:Parameter Name="DataZdobyciaUprawnienOk" Type="Boolean" />
            <asp:Parameter Name="DataWaznosciPsychotestowOk" Type="Boolean" />
            <asp:Parameter Name="DataWaznosciBadanLekarskichOk" Type="Boolean" />
            <asp:Parameter Name="UmowaLojalnosciowaOk" Type="Boolean" />
            <asp:Parameter Name="DataWaznosciSet" Type="Boolean" />
            <asp:Parameter Name="ImportId" Type="Int32" />
            <asp:Parameter Name="IdUprawnienia" Type="Int32" />
            <asp:Parameter Name="IdPracownika" Type="Int32" />
--%>
        <SelectParameters>
            <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="hidUprId" Name="IdUprawnienia" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="hidTypId" Name="typ" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="deNadzien" Name="nadzien" PropertyName="Date" Type="DateTime" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
            <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="hidUprId" Name="IdUprawnienia" PropertyName="Value" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="hidUprId" Name="IdUprawnienia" PropertyName="Value" Type="Int32" />
            <asp:Parameter Name="Numer" Type="String" />
            <asp:Parameter Name="DataWaznosci" Type="DateTime" />
            <asp:Parameter Name="Kategoria" Type="String" />
            <asp:Parameter Name="DataZdobyciaUprawnien" Type="DateTime" />
            <asp:Parameter Name="DataWaznosciPsychotestow" Type="DateTime" />
            <asp:Parameter Name="DataWaznosciBadanLekarskich" Type="DateTime" />
            <asp:Parameter Name="DataWaznosciUmowy" Type="DateTime" />
            <asp:Parameter Name="UmowaLojalnosciowa" Type="Boolean" />
            <asp:Parameter Name="Aktualny" Type="Boolean" />
            <asp:Parameter Name="Uwagi" Type="String" />
            <asp:Parameter Name="DataRozpoczecia" Type="DateTime" />
            <asp:Parameter Name="DataZakonczenia" Type="DateTime" />
            <asp:Parameter Name="NazwaCertyfikatu" Type="String" />
            <asp:Parameter Name="DodatkoweWarunki" Type="String" />
            
            <asp:Parameter Name="IdAutora" Type="Int32" />
            <asp:Parameter Name="IdAutoraZast" Type="Int32" />
            <asp:Parameter Name="DataModyfikacji" Type="DateTime" />
            
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Numer" Type="String" />
            <asp:Parameter Name="DataWaznosci" Type="DateTime" />
            <asp:Parameter Name="Kategoria" Type="String" />
            <asp:Parameter Name="DataZdobyciaUprawnien" Type="DateTime" />
            <asp:Parameter Name="DataWaznosciPsychotestow" Type="DateTime" />
            <asp:Parameter Name="DataWaznosciBadanLekarskich" Type="DateTime" />
            <asp:Parameter Name="DataWaznosciUmowy" Type="DateTime" />
            <asp:Parameter Name="UmowaLojalnosciowa" Type="Boolean" />
            <asp:Parameter Name="Aktualny" Type="Boolean" />
            <asp:Parameter Name="Uwagi" Type="String" />
            <asp:Parameter Name="DataRozpoczecia" Type="DateTime" />
            <asp:Parameter Name="DataZakonczenia" Type="DateTime" />
            <asp:Parameter Name="NazwaCertyfikatu" Type="String" />
            <asp:Parameter Name="DodatkoweWarunki" Type="String" />
            <asp:Parameter Name="IdAutora" Type="Int32" />
            <asp:Parameter Name="IdAutoraZast" Type="Int32" />
            <asp:Parameter Name="IdCertyfikatu" Type="Int32" Direction="Output" DefaultValue="0"/>
        </InsertParameters>
    </asp:SqlDataSource>
</div>

<%--
            <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
            <asp:ControlParameter ControlID="hidUprId" Name="IdUprawnienia" PropertyName="Value" Type="Int32" />
--%>







<asp:SqlDataSource ID="dsAssecoSql" runat="server"
    InsertCommand="
declare @id int
declare @logo varchar(15)
declare @kurs varchar(15)
declare @rozp datetime, @zak datetime, @waz datetime
declare @nazwa nvarchar(100)
declare @opis nvarchar(100)
declare @uwagi nvarchar(255)
declare @numer nvarchar(40)
declare @dodwar nvarchar(100)

--set @id   =  {0}
set @logo   = {1}
set @kurs   = {2}
set @nazwa  = {3}
set @rozp   = {4}
set @zak    = {5}
set @waz    = {6}
set @opis   = {7}
set @numer  = {8}
set @dodwar = {9}
set @uwagi  = {10}

-- TESTY
--select -@id as lp_KursyId 

declare @umowa int
select @umowa = UmowaNumer from dbo.lp_fn_BasePracExLow(@rozp) where LpLogo = @logo 
and @rozp between UmowaOd and ISNULL(UmowaDo,'20990909')  -- moga byc 2 w odwrotnej kolejnosci

delete from lp_vv_KursyEditTmp
insert into lp_vv_KursyEditTmp (LpLogo,KursDefinicja,Nazwa,Opis,DataRozpoczecia,DataZakonczenia,DataWaznosci, UmowaNumer, NumerZaswiadczenia, DodatkoweWarunki, Uwagi)
values (@logo, @kurs, @nazwa, @opis, @rozp, @zak, @waz, @umowa, @numer, @dodwar, @uwagi)
exec lp_KursyFillTmp;
exec lp_KursyInsert

select @id = lp_KursyId from lp_vv_KursyEditTmp
select * from lp_vv_KursyExt where lp_KursyId = @id
    "
    SelectCommand="
-- aktualizacja Id --
declare @oldId int, @newId int
set @oldId = {0}
set @newId = {1}

select * into #ccc from Certyfikaty where Id = @oldId
;
disable trigger Certyfikaty_insert on Certyfikaty;
disable trigger Certyfikaty_delete on Certyfikaty;

delete from Certyfikaty where Id = @oldId

SET IDENTITY_INSERT Certyfikaty ON 
insert into Certyfikaty
  (Id,IdUprawnienia,IdPracownika,Numer,DataWaznosci,Kategoria,DataZdobyciaUprawnien,DataWaznosciPsychotestow,DataWaznosciBadanLekarskich,DataWaznosciUmowy,UmowaLojalnosciowa,
  ImportId,DataZdobyciaUprawnienOk,DataWaznosciPsychotestowOk,DataWaznosciBadanLekarskichOk,UmowaLojalnosciowaOk,DataWaznosciSet,
  Aktualny,Uwagi,
  DataRozpoczecia,DataZakonczenia,NazwaCertyfikatu,DodatkoweWarunki)
select 
  @newId,IdUprawnienia,IdPracownika,Numer,DataWaznosci,Kategoria,DataZdobyciaUprawnien,DataWaznosciPsychotestow,DataWaznosciBadanLekarskich,DataWaznosciUmowy,UmowaLojalnosciowa,
  ImportId,DataZdobyciaUprawnienOk,DataWaznosciPsychotestowOk,DataWaznosciBadanLekarskichOk,UmowaLojalnosciowaOk,DataWaznosciSet,
  Aktualny,Uwagi,
  DataRozpoczecia,DataZakonczenia,NazwaCertyfikatu,DodatkoweWarunki
from #ccc

SET IDENTITY_INSERT Certyfikaty OFF;
enable trigger Certyfikaty_delete on Certyfikaty;
enable trigger Certyfikaty_insert on Certyfikaty;

drop table #ccc
    "
    UpdateCommand="
declare @id int
declare @logo varchar(15)
declare @kurs varchar(15)
declare @rozp datetime, @zak datetime, @waz datetime
declare @nazwa nvarchar(100)
declare @opis nvarchar(100)
declare @uwagi nvarchar(255)
declare @numer nvarchar(40)
declare @dodwar nvarchar(100)

set @id     = {0}
set @logo   = {1}
set @kurs   = {2}
set @nazwa  = {3}
set @rozp   = {4}
set @zak    = {5}
set @waz    = {6}
set @opis   = {7}
set @numer  = {8}
set @dodwar = {9}
set @uwagi  = {10}

-- TESTY
--select -@id as lp_KursyId 

delete from lp_vv_KursyEditTmp
insert into lp_vv_KursyEditTmp (lp_KursyId) values (@id)
exec lp_KursyFillTmp;
update lp_vv_KursyEditTmp set 
--LpLogo = @logo, 
--KursDefinicja = @kurs, 
Nazwa = @nazwa, Opis = @opis, DataRozpoczecia = @rozp, DataZakonczenia = @zak, DataWaznosci = @waz, 
--UmowaNumer = @umowa, 
NumerZaswiadczenia = @numer, DodatkoweWarunki = @dodwar, Uwagi = @uwagi
where lp_KursyId = @id
exec lp_KursyUpdate;

select * from lp_vv_KursyExt where lp_KursyId = @id
    "
    DeleteCommand="
declare @id int
set @id = {0}

-- TESTY
--select * from lp_vv_KursyExt where lp_KursyId is null

delete from lp_vv_KursyEditTmp
insert into lp_vv_KursyEditTmp (lp_KursyId) values (@id)
exec lp_KursyFillTmp;
exec lp_KursyDelete

select * from lp_vv_KursyExt where lp_KursyId = @id
    "
>
</asp:SqlDataSource>




























<%--

<div class="cntCertyfikat">
    <div id="paInfo" runat="server" class="paInfo" visible="false">
        <h3><asp:Label ID="Label1" runat="server" Text="Uprawnienie"></asp:Label></h3>        
        <h3><asp:Label ID="Label2" runat="server" Text="Dane pracownika"></asp:Label></h3>
    </div>
    <div id="paDetails" runat="server" class="paDetails">        
        <h3><asp:Label ID="Label3" runat="server" Text="Szczegóły certyfikatu"></asp:Label></h3>
        <asp:DetailsView ID="DetailsView1" runat="server" 
            DataSourceID="SqlDataSource2"
            AutoGenerateEditButton="true"
            AutoGenerateInsertButton="true"
            AutoGenerateDeleteButton="true"
             >
        </asp:DetailsView>
    </div>
    <div id="paHistory" runat="server" class="paHistory">
        <br />
        <h3><asp:Label ID="Label4" runat="server" Text="Historia"></asp:Label></h3>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id"
            AutoGenerateSelectButton="true">
        </asp:GridView> 
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:KDR %>" 
    SelectCommand="
SELECT 
    Id, 
    CONVERT(varchar(10), DataZdobyciaUprawnien, 20) [Data uzyskania], 
    CONVERT(varchar(10), DataWaznosci, 20) [Data ważności], 
    [Numer], 
    [Kategoria], 
    [Aktualny] 
FROM [Certyfikaty] 
WHERE (([IdPracownika] = @IdPracownika) AND ([IdUprawnienia] = @IdUprawnienia)) ORDER BY [Aktualny] DESC, [DataZdobyciaUprawnien] DESC">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidUprId" Name="IdUprawnienia" PropertyName="Value" Type="Int32" />
        <asp:SessionParameter DefaultValue="PL" Name="lang" SessionField="LNG" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:KDR %>" 
    SelectCommand="
select 
    --C.Id [Id:-],
	P.Nazwisko + ' ' + P.Imie [Pracownik],
	P.Nr_Ewid [Nr ew.],
    U.Symbol [Symbol uprawnienia], 
    case when @lang='PL' then U.Nazwa else U.NazwaEN end [Nazwa uprawnienia], 
    case when @lang='PL' then U.Poziom else U.PoziomEN end [Poziom uprawnienia],
	C.Aktualny [Aktualne],
	C.Numer [Numer zaświadczenia],
	C.Kategoria, 
	case when C.DataWaznosci is null then 'bezterminowo' else CONVERT(varchar(10), C.DataWaznosci, 20) end as [Data ważności],
	CONVERT(varchar(10), C.DataZdobyciaUprawnien, 20) as [Data uzyskania],
	CONVERT(varchar(10), C.DataWaznosciBadanLekarskich, 20) [Data ważności badań lekarskich],
	CONVERT(varchar(10), C.DataWaznosciPsychotestow, 20) [Data ważności psychotestów],
	C.UmowaLojalnosciowa [Umowa lojalnościowa],
	CONVERT(varchar(10), C.DataWaznosciUmowy, 20) [Data ważności umowy],
	C.Uwagi
from Certyfikaty C
left join Uprawnienia U on U.Id = C.IdUprawnienia
left join Pracownicy P on P.Id_Pracownicy = C.IdPracownika
where C.Id = @certId 
    "
    DeleteCommand="DELETE FROM [Certyfikaty] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Certyfikaty] ([IdUprawnienia], [IdPracownika], [Numer], [DataWaznosci], [Kategoria], [DataZdobyciaUprawnien], [DataWaznosciPsychotestow], [DataWaznosciBadanLekarskich], [DataWaznosciUmowy], [UmowaLojalnosciowa], [ImportId], [DataZdobyciaUprawnienOk], [DataWaznosciPsychotestowOk], [DataWaznosciBadanLekarskichOk], [UmowaLojalnosciowaOk], [DataWaznosciSet], [Aktualny]) VALUES (@IdUprawnienia, @IdPracownika, @Numer, @DataWaznosci, @Kategoria, @DataZdobyciaUprawnien, @DataWaznosciPsychotestow, @DataWaznosciBadanLekarskich, @DataWaznosciUmowy, @UmowaLojalnosciowa, @ImportId, @DataZdobyciaUprawnienOk, @DataWaznosciPsychotestowOk, @DataWaznosciBadanLekarskichOk, @UmowaLojalnosciowaOk, @DataWaznosciSet, @Aktualny)" 
    UpdateCommand="UPDATE [Certyfikaty] SET [IdUprawnienia] = @IdUprawnienia, [IdPracownika] = @IdPracownika, [Numer] = @Numer, [DataWaznosci] = @DataWaznosci, [Kategoria] = @Kategoria, [DataZdobyciaUprawnien] = @DataZdobyciaUprawnien, [DataWaznosciPsychotestow] = @DataWaznosciPsychotestow, [DataWaznosciBadanLekarskich] = @DataWaznosciBadanLekarskich, [DataWaznosciUmowy] = @DataWaznosciUmowy, [UmowaLojalnosciowa] = @UmowaLojalnosciowa, [ImportId] = @ImportId, [DataZdobyciaUprawnienOk] = @DataZdobyciaUprawnienOk, [DataWaznosciPsychotestowOk] = @DataWaznosciPsychotestowOk, [DataWaznosciBadanLekarskichOk] = @DataWaznosciBadanLekarskichOk, [UmowaLojalnosciowaOk] = @UmowaLojalnosciowaOk, [DataWaznosciSet] = @DataWaznosciSet, [Aktualny] = @Aktualny WHERE [Id] = @Id"
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="certId" PropertyName="SelectedValue" Type="Int32" />
        <asp:SessionParameter DefaultValue="PL" Name="lang" SessionField="LNG" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdUprawnienia" Type="Int32" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Numer" Type="String" />
        <asp:Parameter Name="DataWaznosci" Type="DateTime" />
        <asp:Parameter Name="Kategoria" Type="String" />
        <asp:Parameter Name="DataZdobyciaUprawnien" Type="DateTime" />
        <asp:Parameter Name="DataWaznosciPsychotestow" Type="DateTime" />
        <asp:Parameter Name="DataWaznosciBadanLekarskich" Type="DateTime" />
        <asp:Parameter Name="DataWaznosciUmowy" Type="DateTime" />
        <asp:Parameter Name="UmowaLojalnosciowa" Type="Boolean" />
        <asp:Parameter Name="ImportId" Type="Int32" />
        <asp:Parameter Name="DataZdobyciaUprawnienOk" Type="Boolean" />
        <asp:Parameter Name="DataWaznosciPsychotestowOk" Type="Boolean" />
        <asp:Parameter Name="DataWaznosciBadanLekarskichOk" Type="Boolean" />
        <asp:Parameter Name="UmowaLojalnosciowaOk" Type="Boolean" />
        <asp:Parameter Name="DataWaznosciSet" Type="Boolean" />
        <asp:Parameter Name="Aktualny" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdUprawnienia" Type="Int32" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Numer" Type="String" />
        <asp:Parameter Name="DataWaznosci" Type="DateTime" />
        <asp:Parameter Name="Kategoria" Type="String" />
        <asp:Parameter Name="DataZdobyciaUprawnien" Type="DateTime" />
        <asp:Parameter Name="DataWaznosciPsychotestow" Type="DateTime" />
        <asp:Parameter Name="DataWaznosciBadanLekarskich" Type="DateTime" />
        <asp:Parameter Name="DataWaznosciUmowy" Type="DateTime" />
        <asp:Parameter Name="UmowaLojalnosciowa" Type="Boolean" />
        <asp:Parameter Name="ImportId" Type="Int32" />
        <asp:Parameter Name="DataZdobyciaUprawnienOk" Type="Boolean" />
        <asp:Parameter Name="DataWaznosciPsychotestowOk" Type="Boolean" />
        <asp:Parameter Name="DataWaznosciBadanLekarskichOk" Type="Boolean" />
        <asp:Parameter Name="UmowaLojalnosciowaOk" Type="Boolean" />
        <asp:Parameter Name="DataWaznosciSet" Type="Boolean" />
        <asp:Parameter Name="Aktualny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

--%>