<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCertyfikat3.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Uprawnienia.cntCertyfikat3" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Uprawnienia/cntCertyfikatHeader.ascx" TagName="cntCertyfikatHeader" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc2" %>

<asp:HiddenField ID="hidCertId" runat="server" Visible="false" />
<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
<asp:HiddenField ID="hidUprId" runat="server" Visible="false" />
<asp:HiddenField ID="hidPola" runat="server" Visible="false" />

<asp:HiddenField ID="hidAktualnyVisible" runat="server" Visible="false" Value="0" />

<div id="paCertyfikat3" runat="server" class="cntCertyfikat cntCertyfikat3">
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

    <uc1:cntCertyfikatHeader ID="cntCertyfikatHeader" runat="server" />

    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Pokaż wszystkie pola" Visible="false" />

    <asp:ListView ID="lvCertyfikat" runat="server" DataSourceID="SqlDataSource3"
        DataKeyNames="Id" InsertItemPosition="none" OnItemCommand="lvCertyfikat_ItemCommand"
        OnDataBound="lvCertyfikat_DataBound"
        OnLayoutCreated="lvCertyfikat_LayoutCreated"
        OnItemCreated="lvCertyfikat_ItemCreated"
        OnItemDeleted="lvCertyfikat_ItemDeleted"
        OnItemInserted="lvCertyfikat_ItemInserted"
        OnItemUpdated="lvCertyfikat_ItemUpdated"
        OnItemDataBound="lvCertyfikat_ItemDataBound"
        OnItemInserting="lvCertyfikat_ItemInserting"
        OnItemUpdating="lvCertyfikat_ItemUpdating"
        OnItemDeleting="lvCertyfikat_ItemDeleting"
        OnItemCanceling="lvCertyfikat_ItemCanceling">
        <ItemTemplate>
            <tr class="it">
                <td id="td18" class="symbol" runat="server" visible='<%# IsVisible(18) %>'>
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("Symbol") %>' />
                </td>

                <td id="Td12" class="nazwa" runat="server" visible='<%# IsVisible(16) %>'>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("NazwaCertyfikatu") %>' />
                </td>
                <td id="Td23" class="nazwa" runat="server" visible='<%# IsVisible(23) %>'>
                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("Trener") %>' />
                </td>
                <td id="Td15" class="numer" runat="server" visible='<%# IsVisible(2) %>'>
                    <asp:Label ID="NumerLabel" runat="server" Text='<%# Eval("Numer") %>' />
                </td>
                <td id="Td16" class="warunki" runat="server" visible='<%# IsVisible(17) %>'>
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("DodatkoweWarunki") %>' />
                </td>

                <td id="Td14" class="kategoria" runat="server" visible='<%# IsVisible(5) %>'>
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

                <td id="Td4" class="check" runat="server" visible='<%# IsAktualnyVisible %>'><%-- wyłączyć po testach --%>
                    <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Eval("Aktualny") %>' Enabled="false" />
                </td>
                <%--                <td class="data" runat="server" visible='<%# IsVisible(21) %>'>
                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("EwaluacjaMonitDni") %>' />
                </td>--%>
                <td class="uwagi" runat="server" visible='<%# IsVisible(19) %>'>
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("StatusName") %>' />
                </td>

                <td class="uwagi" runat="server" visible='<%# IsVisible(13) %>'>
                    <asp:Label ID="UwagiLabel" runat="server" Text='<%# Eval("Uwagi") %>' />
                </td>
                <td id="tdControl" runat="server" class="control" style="width: 200px;">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" Visible='<%# OrAdmin(Eval("EditVisible")) %>'  />    <%-- Convert.ToBoolean(Eval("EditVisible")) --%>

                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" 
                        CssClass="btn btn-sm btn-danger" Visible='<%# OrAdmin(Eval("DeleteVisible")) %>' />    <%-- Convert.ToBoolean(Eval("DeleteVisible")) --%>


                    <asp:LinkButton ID="btnAnkiety" runat="server" PostBackUrl="~/MatrycaSzkolen/Ewaluacja.aspx" ToolTip="Ankiety"
                        Visible='<%# Convert.ToBoolean(Eval("Trained")) %>' CssClass="btn btn-default btn-sm" ><i class="glyphicon glyphicon-list"></i></asp:LinkButton> 
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="" class="table0">
                <tr>
                    <td>
                        <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                        <%--<asp:Button ID="btNewRecord" runat="server" CssClass="button100" CommandName="NewRecord" Text="Dodaj" />--%>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td id="td18" runat="server" class="symbol">
                    <asp:Label ID="lbSymbol" runat="server" Text='<%# Eval("Symbol") %>' />
                </td>

                <td id="td16" runat="server" class="nazwa">
                    <asp:TextBox ID="tbNazwa" runat="server" MaxLength="200" Text='<%# Bind("NazwaCertyfikatu") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbNazwa" ValidationGroup="ivg" ErrorMessage="Błąd" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                <td id="td23" runat="server" class="nazwa">
                    <asp:DropDownList ID="ddlTrener" runat="server" DataSourceID="dsTrener" DataValueField="Id" DataTextField="Name" />
                    <asp:SqlDataSource ID="dsTrener" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="

select null as Id, 'wybierz ...' as Name, 0 as Sort
union all                        
select
  p.Id
, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') Name, 1 as Sort
from msMapaUprawnien mu
left join Pracownicy p on p.Id = mu.IdPracownika
where GETDATE() between mu.DataOd and ISNULL(mu.DataDo, '20990909') and mu.IdUprawnienia = @uprId --19940321 plomba
order by Sort, Name
                        ">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hidUprId" PropertyName="Value" Name="uprId" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td id="td2" runat="server" class="numer">
                    <asp:TextBox ID="NumerTextBox" runat="server" MaxLength="200" Text='<%# Bind("Numer") %>' />
                    <%--
                    <asp:RequiredFieldValidator ControlToValidate="NumerTextBox" ValidationGroup="ivg" ErrorMessage="Error" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    --%>
                </td>
                <td id="td17" runat="server" class="warunki">
                    <asp:TextBox ID="tbDodatkoweWarunki" runat="server" MaxLength="200" Text='<%# Bind("DodatkoweWarunki") %>' />
                </td>

                <td id="td5" runat="server" class="kategoria">
                    <asp:TextBox ID="KategoriaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Kategoria") %>' />
                </td>

                <td id="td14" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit6" runat="server" Date='<%# Bind("DataRozpoczecia") %>' ValidationGroup="ivg" AutoPostBack="true" OnDateChanged="deRozpInsert_Changed" />
                </td>
                <td id="td15" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit7" runat="server" Date='<%# Bind("DataZakonczenia") %>' AutoPostBack="true" OnDateChanged="deRozpInsert_Changed" />
                </td>

                <td id="td6" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataZdobyciaUprawnien") %>' />
                </td>
                <td id="td3" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWaznosci") %>' />
                    <%--                    <br />
                    <asp:CheckBox ID="cbUnlimited" runat="server" Text="Beztermiowo" Checked='<%# Bind("Unlimited") %>' />
                    --%>
                </td>
                <td id="td7" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataWaznosciPsychotestow") %>' />
                </td>
                <td id="td8" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit4" runat="server" Date='<%# Bind("DataWaznosciBadanLekarskich") %>' />
                </td>
                <td id="td10" runat="server" class="check">
                    <asp:CheckBox ID="UmowaLojalnosciowaCheckBox" runat="server" Checked='<%# Bind("UmowaLojalnosciowa") %>' />
                </td>
                <td id="td9" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit5" runat="server" Date='<%# Bind("DataWaznosciUmowy") %>' ValidationGroup="ivg" />
                </td>

                <td id="tdAktualny" runat="server" class="check" visible='<%# IsAktualnyVisible %>'>
                    <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Bind("Aktualny") %>' />
                </td>
                <%--                <td id="td21" runat="server" class="">
                    <asp:TextBox ID="tbEwaluacjaMonitDni" runat="server" MaxLength="6" Text='<%# Bind("EwaluacjaMonitDni") %>' />
                </td>--%>
                <%--                <td id="td19" runat="server" class="">
                   <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="dsStatus" DataValueField="Id" DataTextField="Name" />
                    <asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from msCertyfikatyStatus order by Sort, Name" />
                </td>--%>
                <td id="td19" class="uwagi" runat="server" visible='<%# IsVisible(19) %>'>
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("StatusName") %>' />
                </td>

                <td id="td13" runat="server" class="uwagi">
                    <asp:TextBox ID="UwagiTextBox" runat="server" MaxLength="500" Text='<%# Bind("Uwagi") %>' />
                </td>
                <td class="control" style="width: 140px;">

                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                    <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelI" Text="Cancel" />--%>
                    <asp:Button ID="btnCancel" runat="server" Text="Anuluj" CssClass="btn-default" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td id="td18" runat="server" class="symbol">
                    <asp:Label ID="lbSymbol" runat="server" Text='<%# Eval("Symbol") %>' />
                </td>

                <td id="td16" runat="server" class="nazwa">
                    <asp:TextBox ID="tbNazwa" runat="server" MaxLength="200" Text='<%# Bind("NazwaCertyfikatu") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbNazwa" ValidationGroup="evg" ErrorMessage="Błąd" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>

                <td id="td23" runat="server" class="nazwa">
                    <asp:DropDownList ID="ddlTrener" runat="server" DataSourceID="dsTrener" DataValueField="Id" DataTextField="Name" />
                    <asp:SqlDataSource ID="dsTrener" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="

select null as Id, 'wybierz ...' as Name, 0 as Sort
union all                        
select
  p.Id
, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') Name, 1 as Sort
from msMapaUprawnien mu
left join Pracownicy p on p.Id = mu.IdPracownika
where GETDATE() between mu.DataOd and ISNULL(mu.DataDo, '20990909') and mu.IdUprawnienia = @uprId --19940321 plomba
order by Sort, Name
                        ">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hidUprId" PropertyName="Value" Name="uprId" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>

                <td id="td2" runat="server" class="numer">
                    <asp:TextBox ID="NumerTextBox" runat="server" MaxLength="200" Text='<%# Bind("Numer") %>' />
                    <%--
                    <asp:RequiredFieldValidator ControlToValidate="NumerTextBox" ValidationGroup="evg" ErrorMessage="Error" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    --%>
                </td>

                <td id="td17" runat="server" class="warunki">
                    <asp:TextBox ID="tbDodatkoweWarunki" runat="server" MaxLength="200" Text='<%# Bind("DodatkoweWarunki") %>' />
                </td>

                <td id="td5" runat="server" class="kategoria">
                    <asp:TextBox ID="KategoriaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Kategoria") %>' />
                </td>

                <td id="td14" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit6" runat="server" Date='<%# Bind("DataRozpoczecia") %>' ValidationGroup="evg" AutoPostBack="true" OnDateChanged="deRozpUpdate_Changed" />
                </td>
                <td id="td15" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit7" runat="server" Date='<%# Bind("DataZakonczenia") %>' AutoPostBack="true" OnDateChanged="deRozpUpdate_Changed" ValidationGroup="przeszkol" />
                </td>

                <td id="td6" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataZdobyciaUprawnien") %>' />
                </td>
                <td id="td3" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWaznosci") %>' />
                    <%--                    <br />
                    <asp:CheckBox ID="cbUnlimited" runat="server" Text="Beztermiowo" Checked='<%# Bind("Unlimited") %>' />
                    --%>
                </td>
                <td id="td7" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit3" runat="server" Date='<%# Bind("DataWaznosciPsychotestow") %>' />
                </td>
                <td id="td8" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit4" runat="server" Date='<%# Bind("DataWaznosciBadanLekarskich") %>' />
                </td>
                <td id="td10" runat="server" class="check">
                    <asp:CheckBox ID="UmowaLojalnosciowaCheckBox" runat="server" Checked='<%# Bind("UmowaLojalnosciowa") %>' ValidationGroup="evg" />
                </td>
                <td id="td9" runat="server" class="data">
                    <uc2:DateEdit ID="DateEdit5" runat="server" Date='<%# Bind("DataWaznosciUmowy") %>' />
                </td>

                <td runat="server" class="check" visible='<%# IsAktualnyVisible %>'>
                    <asp:CheckBox ID="AktualnyCheckBox" runat="server" Checked='<%# Bind("Aktualny") %>' />
                </td>
                <%--                <td id="td21" runat="server" class="">
                    <asp:TextBox ID="tbEwaluacjaMonitDni" runat="server" MaxLength="6" Text='<%# Bind("EwaluacjaMonitDni") %>' />
                </td>--%>
                <%--              <td id="td19" runat="server" class="">
                   <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="dsStatus" DataValueField="Id" DataTextField="Name" />
                    <asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from msCertyfikatyStatus order by Sort, Name" />
                </td>--%>

                <td id="td19" class="uwagi" runat="server" visible='<%# IsVisible(19) %>'>
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("StatusName") %>' />
                </td>
                <td id="td13" runat="server" class="uwagi">
                    <asp:TextBox ID="UwagiTextBox" runat="server" MaxLength="500" Text='<%# Bind("Uwagi") %>' />
                </td>
                <td class="control" style="width: 240px;">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" CssClass="btn-success" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />

                    <asp:Button ID="PrzeszkolonoButton" runat="server" CommandName="Update" Text="Przeszkolono" CommandArgument="PRZESZKOL" ValidationGroup="przeszkol" Visible='<%# IsVisible(22) && PrzeszkolonoVisible(Eval("Status")) %>' />
                    
                    <asp:Button ID="btnReject" runat="server" CommandName="Update" Text="Odrzuć" 
                        CssClass="btn btn-sm btn-danger" CommandArgument="REJECT" Visible='<%# Convert.ToBoolean(Eval("RejectVisible")) %>'  />
                    
                    <%--                
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                    --%>
                </td>
            </tr>
        </EditItemTemplate>
        <LayoutTemplate>
            <table runat="server" class="xnarrow xtbCertyfikaty" style="width: 100%;">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="" name="report" class="table">
                            <tr runat="server" style="">
                                <th id="th18" runat="server">
                                    <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="Symbol" Text="Symbol"></asp:LinkButton></th>

                                <th id="th16" runat="server">
                                    <asp:LinkButton ID="LinkButton13" runat="server" CommandName="Sort" CommandArgument="NazwaCertyfikatu" Text="Nazwa"></asp:LinkButton></th>
                                <th id="th23" runat="server">
                                    <asp:LinkButton ID="LinkButton30" runat="server" CommandName="Sort" CommandArgument="Trener" Text="Trener"></asp:LinkButton></th>
                                <th id="th2" runat="server">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Numer" Text="Numer zaświadczenia"></asp:LinkButton></th>
                                <th id="th17" runat="server">
                                    <asp:LinkButton ID="LinkButton14" runat="server" CommandName="Sort" CommandArgument="DodatkoweWarunki" Text="Dodatkowe warunki"></asp:LinkButton></th>

                                <th id="th5" runat="server">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Kategoria" Text="Kategoria"></asp:LinkButton></th>

                                <th id="th14" runat="server">
                                    <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="DataRozpoczecia" Text="Data rozpoczęcia"></asp:LinkButton></th>
                                <th id="th15" runat="server">
                                    <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="DataZakonczenia" Text="Data zakończenia"></asp:LinkButton></th>

                                <th id="th6" runat="server">
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="DataZdobyciaUprawnien" Text="Data Uzyskania"></asp:LinkButton></th>
                                <th id="th3" runat="server">
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="DataWaznosci" Text="Data ważnosci"></asp:LinkButton></th>

                                <th id="th7" runat="server">
                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="DataWaznosciPsychotestow" Text="Data ważności psychotestów"></asp:LinkButton></th>
                                <th id="th8" runat="server">
                                    <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="DataWaznosciBadanLekarskich" Text="Data ważności badań lekarskich"></asp:LinkButton></th>

                                <th id="th10" runat="server">
                                    <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="UmowaLojalnosciowa" Text="Umowa lojalnościowa"></asp:LinkButton>
                                </th>
                                <th id="th9" runat="server">
                                    <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="DataWaznosciUmowy" Text="Data ważności umowy"></asp:LinkButton></th>

                                <th id="thAktualny" runat="server" visible='<%# IsAktualnyVisible %>'>
                                    <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Aktualny" Text="Aktualny"></asp:LinkButton></th>

                                <%--<th id="th21" runat="server" ><asp:LinkButton ID="LinkButton17" runat="server" CommandName="Sort" CommandArgument="EwaluacjaMonitDni" Text="Monit - ewaluacja (dni)"></asp:LinkButton></th>--%>
                                <th id="th19" runat="server">
                                    <asp:LinkButton ID="LinkButton16" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status"></asp:LinkButton></th>
                                <th id="th13" runat="server">
                                    <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Uwagi" Text="Uwagi"></asp:LinkButton></th>
                                <th id="thControl" runat="server" class="control">&nbsp;
                                    <%--<asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />--%>
                                    &nbsp;
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="pager">
                    <td class="left">
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="10">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="|◄" PreviousPageText="◄" />
                                <asp:NumericPagerField ButtonType="Link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="►" LastPageText="►|" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                    <td class="right">
                        <%--
                        <asp:Label ID="lbPageSize" runat="server" CssClass="count" Text="Pokaż na stronie:"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlLines" runat="server" >
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        --%>
                        <span class="count">
                            <asp:Literal ID="lbCountLabel" runat="server" Text="Ilość:"></asp:Literal><asp:Label ID="lbCount" runat="server" /></span>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>

    <asp:Button ID="btnInsert" runat="server" Text="Dodaj" OnClick="btnInsert_Click" CssClass="btn btn-sm btn-success pull-right" />

    <asp:Button ID="btnRejectConfirm" runat="server" CssClass="button_postback" OnClick="btnRejectConfirm_Click" />
   


    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
        ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
select 
 T.Symbol
, C.*
, cs.Nazwa as StatusName
, isnull(p.Nazwisko + ' ' + p.Imie, 'Brak') as Trener
, case when c.Status = 3 then 1 else 0 end as Trained
, uk.Szkolenie IsSzkolenie
, case when uk.Szkolenie = 1 and (C.Status = 1 or C.Status = 2) then 1 else 0 end RejectVisible
, CAST(case when (uk.Szkolenie = 1 and (C.Status = 1 or C.Status = 2)) or (uk.Szkolenie = 0) then 1 else 0 end as bit) DeleteVisible
, CAST(case when (uk.Szkolenie = 1 and (C.Status = 1 or C.Status = 2)) or (uk.Szkolenie = 0) then 1 else 0 end as bit) EditVisible
from [Certyfikaty] C 
left join Uprawnienia T on T.Id = C.IdUprawnienia
left join UprawnieniaKwalifikacje uk on uk.Id = T.KwalifikacjeId
left join msCertyfikatyStatus cs on cs.Id = C.Status
left join Pracownicy p on p.Id = c.IdTrenera
where C.IdPracownika = @IdPracownika AND C.[IdUprawnienia] = @IdUprawnienia
order by C.Aktualny desc, C.DataWaznosci desc
"
        DeleteCommand="
DELETE FROM [Certyfikaty] WHERE [Id] = @Id
--update Certyfikaty set Aktualny = 0 where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia
--update Certyfikaty set Aktualny = 1 where Id = (select top 1 Id from Certyfikaty where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia order by ISNULL(DataWaznosci,'20990909') desc, Id desc)
"
        InsertCommand="
INSERT INTO [Certyfikaty] ([IdUprawnienia], [IdPracownika], [Numer], [DataWaznosci], [Kategoria], [DataZdobyciaUprawnien], [DataWaznosciPsychotestow]
        , [DataWaznosciBadanLekarskich], [DataWaznosciUmowy], [UmowaLojalnosciowa], [Aktualny], [Uwagi]
        , DataRozpoczecia, DataZakonczenia, NazwaCertyfikatu, DodatkoweWarunki, IdAutora, IdAutoraZast
        , Status, IdTrenera
        ) 
VALUES (@IdUprawnienia, @IdPracownika, @Numer, @DataWaznosci, @Kategoria, 

--@DataZdobyciaUprawnien, 
ISNULL(@DataZdobyciaUprawnien, ISNULL(@DataZakonczenia, @DataRozpoczecia)),

@DataWaznosciPsychotestow, @DataWaznosciBadanLekarskich, @DataWaznosciUmowy, @UmowaLojalnosciowa
        , @Aktualny, @Uwagi, @DataRozpoczecia, @DataZakonczenia, @NazwaCertyfikatu, @DodatkoweWarunki
        , @IdAutora, @IdAutoraZast, 
        
        case when (select Szkolenie from UprawnieniaKwalifikacje where Id = (select KwalifikacjeId from Uprawnienia where Id = @IdUprawnienia)) = 1 then
            case when @DataZakonczenia is not null then case when dbo.GetRightId((select Rights from Pracownicy where Id = @IdAutora), 85) = 1 then 3 else 2 end else 1 end
        else null end
        , @IdTrenera)

--update Certyfikaty set Aktualny = 0 where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia
--update Certyfikaty set Aktualny = 1 where Id = (select top 1 Id from Certyfikaty where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia order by ISNULL(DataWaznosci,'20990909') desc, Id desc)

set @IdCertyfikatu = (select @@Identity)
"
        UpdateCommand="
if @IdUprawnienia = 8 and @DataWaznosci is null        
   set @DataWaznosci = @DataWaznosciUmowy

UPDATE [Certyfikaty] SET [IdUprawnienia] = @IdUprawnienia, [IdPracownika] = @IdPracownika, [Numer] = @Numer, [DataWaznosci] = @DataWaznosci, [Kategoria] = @Kategoria, 

--[DataZdobyciaUprawnien] = @DataZdobyciaUprawnien, 
[DataZdobyciaUprawnien] = ISNULL(@DataZdobyciaUprawnien, ISNULL(@DataZakonczenia, @DataRozpoczecia)), 

[DataWaznosciPsychotestow] = @DataWaznosciPsychotestow, [DataWaznosciBadanLekarskich] = @DataWaznosciBadanLekarskich, [DataWaznosciUmowy] = @DataWaznosciUmowy, [UmowaLojalnosciowa] = @UmowaLojalnosciowa, 
[Aktualny] = @Aktualny, [Uwagi] = @Uwagi 
,DataRozpoczecia = @DataRozpoczecia, DataZakonczenia = @DataZakonczenia, NazwaCertyfikatu = @NazwaCertyfikatu, DodatkoweWarunki = @DodatkoweWarunki
,DataModyfikacji = @DataModyfikacji, IdAutora = @IdAutora, IdAutoraZast = @IdAutoraZast, Status = isnull(@Status, Status), IdTrenera = @IdTrenera
WHERE [Id] = @Id

--update Certyfikaty set Aktualny = 0 where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia
--update Certyfikaty set Aktualny = 1 where Id = (select top 1 Id from Certyfikaty where IdPracownika = @IdPracownika and IdUprawnienia = @IdUprawnienia order by ISNULL(DataWaznosci,'20990909') desc, Id desc)
"
        OnInserted="SqlDataSource3_Inserted" CancelSelectOnNullParameter="false">

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
            <asp:Parameter Name="IdTrenera" Type="Int32" />
            <%--<asp:Parameter Name="Status" Type="Int32" />--%>
            <%--<asp:Parameter Name="EwaluacjaMonitDni" Type="Int32" />--%>
        </UpdateParameters>
        <InsertParameters>
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
            <asp:Parameter Name="IdCertyfikatu" Type="Int32" Direction="Output" DefaultValue="0" />
            <asp:Parameter Name="IdTrenera" Type="Int32" />
            <%--<asp:Parameter Name="Status" Type="Int32" />--%>
            <%--<asp:Parameter Name="EwaluacjaMonitDni" Type="Int32" />--%>
        </InsertParameters>
    </asp:SqlDataSource>
</div>

<asp:SqlDataSource ID="dsPrzeszkolono" runat="server" SelectCommand="update Certyfikaty set Status = 2, DataAkceptacji1 = {1}, IdAkceptujacego1 = {2}, IdAkceptujacegoZast1 = {3} where Id = {0}" />
<asp:SqlDataSource ID="dsReject" runat="server" SelectCommand="update Certyfikaty set Status = -1, Aktualny = 0 where Id = {0}" />


<%--
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
set @logo   = '{1}'
set @kurs   = '{2}'
set @nazwa  = '{3}'
set @rozp   = '{4}'
set @zak    = '{5}'
set @waz    = '{6}'
set @opis   = '{7}'
set @numer  = '{8}'
set @dodwar = '{9}'
set @uwagi  = '{10}'

-- TESTY
select -@id as lp_KursyId 

/*
declare @umowa int
select @umowa = UmowaNumer from dbo.lp_fn_BasePracExLow(@rozp) where LpLogo = @logo 
and @rozp between UmowaOd and ISNULL(UmowaDo,'20990909')  -- moga byc 2 w odwrotnej kolejnosci

delete lp_vv_KursyEditTmp
insert into lp_vv_KursyEditTmp (LpLogo,KursDefinicja,Nazwa,Opis,DataRozpoczecia,DataZakonczenia,DataWaznosci, UmowaNumer, NumerZaswiadczenia, DodatkoweWarunki, Uwagi)
values (@logo, @kurs, @nazwa, @opis, @rozp, @zak, @waz, @umowa, @numer, @dodwar, @uwagi)
exec lp_KursyFillTmp;
exec lp_KursyInsert

select @id = lp_KursyId from lp_vv_KursyEditTmp
select * from lp_vv_KursyExt where lp_KursyId = @id
*/
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
set @logo   = '{1}'
set @kurs   = '{2}'
set @nazwa  = '{3}'
set @rozp   = '{4}'
set @zak    = '{5}'
set @waz    = '{6}'
set @opis   = '{7}'
set @numer  = '{8}'
set @dodwar = '{9}'
set @uwagi  = '{10}'

-- TESTY
select -@id as lp_KursyId 

/*
delete lp_vv_KursyEditTmp
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
*/
    "
    DeleteCommand="
declare @id int
set @id = {0}

-- TESTY
select * from lp_vv_KursyExt where lp_KursyId is null

/*
delete lp_vv_KursyEditTmp
insert into lp_vv_KursyEditTmp (lp_KursyId) values (@id)
exec lp_KursyFillTmp;
exec lp_KursyDelete

select * from lp_vv_KursyExt where lp_KursyId = @id
*/
    "
>
</asp:SqlDataSource>
--%>




































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