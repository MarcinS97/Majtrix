<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZamowienia.ascx.cs"
    Inherits="HRRcp.IPO.Controls.cntZamowienia" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc3" %>
<%@ Register src="~/IPO/Controls/cntSplityCC.ascx" tagname="cntSplityCC" tagprefix="uc3" %>
<style type="text/css">
    table.IPOleftMenuCard
    {
        border: 0;
        width: 180px;
        line-height: 20px;
        float: right;
    }
    table.IPOleftMenuCard td.right
    {
        text-align: right;
        border: 0;
        font-weight: normal;
    }
    table.IPOleftMenuCard td.left
    {
        text-align: left;
        border: 0;
        font-weight: bold;
    }
    table.IPOleftMenuCard td.center
    {
        text-align: center;
        border: 0;
    }
    div.IPOleftMenuCardRow
    {
        white-space: nowrap;
        width: 180px;
        overflow: hidden;
        text-overflow: ellipsis;
    }
    .item a
    {
        color: black;
    }
    .item a:hover
    {
        color: red;
    }
    .floatRight
    {
        float: right;
    }
    .tdWhite td
    {
        background-color: white !important;
    }
    .dateedit input
    {
        width: 80px !important;
    }
    .newButton
    {
        margin-left: 250px;
        width: auto;
        border: solid 1px Silver;
        margin: 2px 0px 2px 0px;
        font-family: Verdana, Arial, Helvetica, sans-serif;
        font-size: 12px !important;
        font-weight: bold;
        text-transform: uppercase;
        color: #000000;
        padding: 1px 6px 2px 6px;
        background-color: #00FF00;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
        -webkit-box-shadow: 0 1px 3px 0px #DDDDDD;
        cursor: pointer;
        text-align: center;
    }
    .newButton:hover
    {
        color: red;
    }
    .newButton[disabled] 
    {
        color: #666666;
        background-color: #dddddd;
    }
    .utworzZamowienieButton 
    {
       width: auto !important;
        color: Black !important;
        background-color: #00FF00 !important; 
    }
    .utworzZamowienieButton:hover
    {
        color: red !important;
    }
    .wyslijButton 
    {
        width: auto;
        color: Black !important;
        background-color: #00FF00 !important;
    }
    .wyslijButton:hover
    {
        color: red !important;
    }
    .pozycjeDodajButton 
    {
        color: Black !important;
        width: auto;
        background-color: #00FF00 !important;
    }
    .pozycjeDodajButton:hover
    {
        color: Red !important;
    }
    .pozycjeDodajButton[disabled] 
    {
        color: #666666;
        background-color: #dddddd !important;
    }
    .dodawanieVisible
    {
        visibility: <%= DodawanieVisible() ? "visible" : "hidden" %>;
    }
    .edycjaSciezkiAkceptacjiVisible
    {
        visibility: <%= EdycjaSciezkiAkceptacjiVisible() ? "visible" : "hidden" %>;
    }
    .wyborSciezki
    {
        visibility: <%= WyborSciezki() ? "visible" : "hidden" %>;
    }
    .completionList
    {
        border: solid 1px Gray;
        margin: 0px;
        padding: 3px;
        height: 120px;
        overflow: auto;
        background-color: #FFFFFF;
        cursor: "pointer";
    }
    .listItem
    {
        color: #191919;
    }
    .itemHighlighted
    {
        background-color: #ADD6FF;
    }
    .plik
    {
        float: left;
        margin: 0px 4px;
    }
    .plik img
    {
        margin: -7px -7px -10px -7px;
    }
    .fileupload1
    {
        float: left;
    }
    .fileupload1 div
    {
        width: 100px;
        background: url('~/../../images/buttons/fileupload_button1.png') no-repeat 100% 1px !important;
    }
    .fileupload1 input[type="file"]
    {
        width: 100px;
    }
    .fileupload1 input[type="text"]
    {
        display: none;
    }
    .green
    {
        color: green !important;
    }
    .red
    {
        color: red !important;
    }
    .gray
    {
        color: gray !important;
    }
    .black
    {
        color: black !important;
    }
    .yellow_background
    {
        background-color: yellow !important;
    }
    .gray_row span
    {
        text-decoration: line-through;
    }
    .black_row
    {
        color: black !important;
    }
</style>

<script type="text/javascript">
    Sys.UI.Point = function Sys$UI$Point(x, y) {

        x = Math.round(x);
        y = Math.round(y);

        var e = Function._validateParams(arguments, [
            { name: "x", type: Number, integer: true },
            { name: "y", type: Number, integer: true }
        ]);
        if (e) throw e;
        this.x = x;
        this.y = y;
    }
    function ProduktItemSelected(sender, e) {
        var ProduktHiddenFieldID = "<%= ProduktHiddenField.ClientID %>";
        try {
            var f = $find("ProduktHiddenField");
            if (f == null) {
                f = document.getElementById(ProduktHiddenFieldID);
            }
            f.value = e.get_value() + "_" + Date.now();
        } catch (Error) {
        }
        __doPostBack(ProduktHiddenFieldID, "");
    }
    function ProduktOnListPopulated() {

        var completionList = $find("ProduktAutoCompleteEx").get_completionList();
        completionList.style.width = 'auto';
    }
    function EditWarn() {
        var szczegolyListViewID = "<%= szczegolyListView.ClientID %>";
        var szczegolyListView = document.getElementById(szczegolyListViewID + "_Table1");
        var pozycjeListViewID = "<%= pozycjeListView.ClientID %>";
        var pozycjeListView = document.getElementById(pozycjeListViewID + "_Table1");
        if (szczegolyListView.innerHTML.indexOf('<tr class="eit">') > -1
            || pozycjeListView.innerHTML.indexOf('<tr class="eit">') > -1
            || pozycjeListView.innerHTML.indexOf('<tr class="iit">') > -1) {
            alert("Zanim przejdziesz do innego zamówienia, najpierw zakończ edycję aktualnego.");
            return false;
        } else {
            return true;
        }
    }
</script>

<asp:HiddenField ID="ProduktHiddenField" OnValueChanged="ProduktHiddenField_OnValueChanged"
    runat="server" />
<table class="tabsContent">
    <tr>
        <td class="LeftMenu" media="print">
            <asp:ListView ID="zamowieniaListView" runat="server" DataKeyNames="Id" DataSourceID="zamowieniaDataSource"
                OnSelectedIndexChanged="zamowieniaListView_SelectedIndexChanged" OnItemCommand="zamowieniaListView_ItemCommand"
                OnItemInserting="zamowieniaListView_ItemInserting" OnItemInserted="zamowieniaListView_ItemInserted"
                OnDataBound="zamowieniaListView_DataBound" 
                OnItemCreated="zamowieniaListView_ItemCreated" 
                onlayoutcreated="zamowieniaListView_LayoutCreated">
                <ItemTemplate>
                    <tr class="item">
                        <td>
                            <asp:LinkButton ID="selectButton" CommandName="Select" OnClientClick="return EditWarn();" runat="server">                                            
                                <table class=IPOleftMenuCard>
                                    <tr>
                                        <td class="left">
                                            <%# Eval("Numer") %>
                                        </td>
                                        <td class="right">
                                            <%# Eval("DataUtworzenia", "{0:dd-MM-yyyy}") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="right">
                                            <div class="IPOleftMenuCardRow">
                                                <%# Eval("Wartosc") %>
                                                PLN
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="right">
                                            <div class="IPOleftMenuCardRow"><%# Eval("Pracownik") %></div>           
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="2" class="right">
                                            <div class="IPOleftMenuCardRow"><%# Eval("CCList") %></div>           
                                        </td>
                                    </tr>
                                </table>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
                <SelectedItemTemplate>
                    <tr class="selected item">
                        <td>
                            <table class="IPOleftMenuCard">
                                <tr>
                                    <td class="left">
                                        <%# Eval("Numer") %>
                                    </td>
                                    <td class="right">
                                        <%# Eval("DataUtworzenia", "{0:dd-MM-yyyy}") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="right">
                                        <div class="IPOleftMenuCardRow">
                                            <%# Eval("Wartosc") %>
                                            PLN
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="right">
                                        <div class="IPOleftMenuCardRow">
                                            <%# Eval("Pracownik") %></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="right">
                                        <div class="IPOleftMenuCardRow">
                                            <%# Eval("CCList") %></div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </SelectedItemTemplate>
                <InsertItemTemplate>
                    <tr class="item">
                        <td>
                            <table id="Table1" class="hoverline cntPracownicy2 ListView1 tbBrowser" runat="server"
                                style="width: 300px">
                                <tr id="Tr1" runat="server">
                                    <td id="Td1" colspan="2" runat="server">
                                        <table id="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0"
                                            style="">
                                            <tr id="Tr2" runat="server" style="">
                                            </tr>
                                            <tr class="bottom">
                                            </tr>
                                            <tr class="iit">
                                                <td colspan="6" class="col1">
                                                    <table class="edit">
                                                        <tr>
                                                            <td class="coled1">
                                                                <div class="title">
                                                                    <asp:Label runat="server" Text="Dane podstawowe" />
                                                                </div>
                                                                <span class="label" style="width: 80px;">Klient:</span>
                                                                <asp:TextBox ID="KlientTextBox" runat="server" Text='iQor BYD' Width="160px" />
                                                                <br />
                                                                <span class="label" style="width: 80px;">Magazyn:</span>
                                                                <asp:DropDownList ID="MagazynDropDownList" runat="server" DataTextField="Nazwa" Width="166px"
                                                                    DataValueField="Id" DataSourceID="magazynyDataSource">
                                                                </asp:DropDownList>
                                                                <br />
                                                                <span class="label" style="width: 80px;">CER:</span>
                                                                <asp:CheckBox ID="CERCheckBox" class="check" runat="server" OnCheckedChanged="CERCheckBox_OnCheckedChanged"
                                                                    AutoPostBack="true" /><br />
                                                                <span class="label" style="width: 80px;">CERNr:</span>
                                                                <asp:TextBox ID="CERNrTextBox" runat="server" Width="160px" Enabled="false" />
                                                                <asp:RequiredFieldValidator ID="CERValidator" ControlToValidate="CERNrTextBox" runat="server"
                                                                    SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error"
                                                                    ErrorMessage="*" Enabled="false" />
                                                                <br />   
                                                                <uc3:cntSplityCC ID="cntSplityCC" Mode="1" runat="server" />
                                                                <div style="text-align: center;">
                                                                <asp:Button ID="InsertButton" runat="server" CssClass="utworzZamowienieButton" CommandName="Insert" Text="Utwórz zamówienie" ValidationGroup="vgi"
                                                                     />
                                                                <asp:Button ID="CancelInsertButton" runat="server" CommandName="CancelInsert" Text="Anuluj" Width="60px" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EmptyDataTemplate>
                    <table class="IPOleftMenuCard">
                        <tr>
                            <td class="center">
                                <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Nowe Zamówienie"
                                    CssClass="newButton dodawanieVisible" Visible='<%# DodawanieVisible() %>' />
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" class="IPOleftMenuCard">
                        <tr>
                            <td class="center">
                                <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Nowe Zamówienie"
                                    CssClass="newButton dodawanieVisible" Visible='<%# DodawanieVisible() %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
        </td>
        <td class="LeftMenuContent">
            <asp:ListView ID="szczegolyListView" runat="server" DataSourceID="szczegolyZamowieniaDataSource"
                DataKeyNames="Id" OnItemDeleted="szczegolyListView_OnItemDeleted" Visible="false"
                OnItemCommand="szczegolyListView_ItemCommand" OnItemDataBound="szczegolyListView_OnItemDataBound"
                OnItemUpdating="szczegolyListView_OnItemUpdating">
                <ItemTemplate>
                    <tr class="eit tdWhite">
                        <td colspan="6" class="col1">
                            <table class="edit">
                                <tr>
                                    <td class="coled1">
                                        <div class="title">
                                            <asp:Label ID="Label22" runat="server" Text="Dane podstawowe" />
                                        </div>
                                        <span class="label">Numer:</span>
                                        <asp:Label ID="NumberLabel" runat="server" Text='<%# Eval("Numer") %>' />
                                        <br />
                                        <span class="label">Klient:</span>
                                        <asp:Label ID="KlientLabel" runat="server" Text='<%# Eval("Klient") %>' />
                                        <br />
                                        <span class="label">Magazyn:</span>
                                        <asp:Label ID="MagazynLabel" runat="server" Text='<%# Eval("NazwaMagazynu") %>' />
                                        <br />
                                        <span class="label">Wartość:</span>
                                        <asp:Label ID="Label14" runat="server" Text='<%# Eval("Wartosc") %>' />
                                        <asp:Label ID="Label15" runat="server" Text="PLN" />
                                        <br />
                                    </td>
                                    <td class="coled2">
                                        <div class="title">
                                            <asp:Label ID="Label23" runat="server" Text="Dane dodatkowe" />
                                        </div>
                                        <span class="label">CER:</span>
                                        <asp:CheckBox ID="CERCheckBox" class="check" runat="server" Checked='<%# Bind("CER") %>'
                                            Enabled="false" /><br />
                                        <span class="label">CERNr:</span>
                                        <asp:Label ID="CERLabel" runat="server" Text='<%# Eval("CERNr") %>' Visible='<%# !CEREditVisible((bool)Eval("CER"), (int)Eval("PoziomAkceptacji")) %>' />
                                        <asp:TextBox ID="CERTextBox" runat="server" Text='<%# Eval("CERNr") %>' Visible='<%# CEREditVisible((bool)Eval("CER"), (int)Eval("PoziomAkceptacji")) %>' />
                                        <br />
                                        <span class="label">CC:</span>
                                        <uc3:cntSplityCC ID="cntSplityCC" Mode="0" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="control" style="width: 100px !important;">
                            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" Visible='<%# EdycjaVisible() %>'
                                Width="80px" /><br />
                            <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible='<%# EdycjaVisible() %>'
                                Width="80px" /><br />
                            <br />
                            <asp:Button ID="WyslijButton" CssClass="wyslijButton" runat="server" CommandName="Send" Text="Wyślij do&#x00A;akceptacji" Visible='<%# WysylanieVisible() %>'
                                 /><br />
                            <asp:Button ID="AcceptButton" runat="server" CommandName="Accept" Text="Akceptuj"
                                Visible='<%# AkceptacjaVisible() %>' Width="80px" /><br />
                            <asp:Button ID="RejectButton" runat="server" CommandName="Reject" Text="Odrzuć" Visible='<%# AkceptacjaVisible() %>'
                                Width="80px" />
                            <div runat="server" visible='<%# DrukujVisible() %>'>
                                <a href='PodgladZamowienia.aspx?HashLink=<%# Eval("HashLink") %>' target="_blank">Drukuj</a>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr class="eit">
                        <td colspan="6" class="col1">
                            <table class="edit">
                                <tr>
                                    <td class="coled1">
                                        <div class="title">
                                            <asp:Label ID="Label22" runat="server" Text="Dane podstawowe" />
                                        </div>
                                        <span class="label">Numer:</span>
                                        <asp:TextBox ID="NumerTextBox" runat="server" Text='<%# Bind("Numer") %>' />
                                        <br />
                                        <span class="label">Klient:</span>
                                        <asp:TextBox ID="KlientTextBox" runat="server" Text='<%# Bind("Klient") %>' />
                                        <br />
                                        <span class="label">Magazyn:</span>
                                        <asp:DropDownList ID="MagazynDropDownList" runat="server" DataTextField="Nazwa" Width="246px"
                                            DataValueField="Id" DataSourceID="magazynyDataSource" SelectedValue='<%# Bind("Magazyn") %>'>
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Wartość:</span>
                                        <asp:Label ID="Label14" runat="server" Text='<%# Eval("Wartosc") %>' />
                                        <asp:Label ID="Label15" runat="server" Text="PLN" />
                                        <br />
                                    </td>
                                    <td class="coled2">
                                        <div class="title">
                                            <asp:Label ID="Label23" runat="server" Text="Dane dodatkowe" />
                                        </div>
                                        <span class="label">CER:</span>
                                        <asp:CheckBox ID="CERCheckBox" class="check" runat="server" Checked='<%# Bind("CER") %>' OnCheckedChanged="CEREdit_CheckedChanged" AutoPostBack="true"/><br />
                                        <span class="label">CERNr:</span>
                                        <asp:TextBox ID="CERTextBox" runat="server" Text='<%# Bind("CERNr") %>' />
                                        <asp:RequiredFieldValidator ID="CERValidator" ControlToValidate="CERTextBox" runat="server" SetFocusOnError="True"
                                            Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="*" Enabled="false"/>
                                        <br />
                                        <span class="label">CC:</span>
                                        <uc3:cntSplityCC ID="cntSplityCC" Mode="1" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="control" style="width: 100px !important;">
                            <asp:Button ID="UpdateButton" runat="server" CssClass="utworzZamowienieButton" CommandName="Update" Text="Zapisz" ValidationGroup="vge"
                                Width="80px" /><br />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" Width="80px" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table id="Table1" class="hoverline cntPracownicy2 ListView1 tbBrowser" runat="server">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" colspan="2" runat="server">
                                <table id="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0"
                                    style="">
                                    <tr id="Tr2" runat="server" style="">
                                    </tr>
                                    <tr class="bottom">
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <br />
            <asp:ListView ID="pozycjeListView" runat="server" DataSourceID="pozycjeDataSource"
                DataKeyNames="Id" OnItemCommand="pozycjeListView_ItemCommand" OnItemInserting="pozycjeListView_ItemInserting"
                OnItemInserted="pozycjeListView_ItemInserted" Visible="false" OnItemDataBound="pozycjeListView_OnItemDataBound"
                OnItemUpdated="pozycjeListView_OnItemUpdated">
                <ItemTemplate>
                    <tr class='<%# GetCssPozycji((int)Eval("IdStatusu")) %>' id="trLine" runat="server">
                        <td>
                            <asp:Label runat="server" CssClass="left" Text='<%# Eval("Nazwa") %>' />
                            <br />
                            <span class="line2">
                                <asp:Label runat="server" CssClass="left" Text='<%# Eval("Opis") %>' />
                            </span>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" CssClass="left" Text='<%# Eval("PartNo") %>' />
                            <br />
                            <span class="line2">
                                <asp:Label ID="Label13" runat="server" CssClass="left" Text='<%# Eval("ProductClass") %>' /><br />
                                <asp:Label ID="Label12" runat="server" CssClass="left" Text='<%# Eval("ProductSubclass") %>' />
                            </span>
                        </td>
                        <td>
                            <asp:Label runat="server" CssClass="left" Text='<%# Eval("Ilosc") %>' />
                            <asp:Label runat="server" CssClass="left" Text='<%# Eval("Jednostka") %>' />
                            <br />
                            <span class="line2">
                                <asp:Label runat="server" CssClass="left" Text='<%# Eval("Cena") %>' />
                                <asp:Label runat="server" CssClass="left" Text='<%# Eval("Waluta") %>' />
                            </span><br />
                            <span class="line2">
                                <asp:Label runat="server" CssClass="left" Text='<%# Eval("Wartosc") %>' />
                                <asp:Label runat="server" CssClass="left" Text='<%# Eval("Waluta") %>' />
                            </span>                            
                        </td>
                        <td class="check">
                            <asp:CheckBox class="check" runat="server" Checked='<%# Eval("ProduktStockowy") %>'
                                Enabled="false" />
                        </td>
                        <td class="check">
                            <asp:CheckBox class="check" runat="server" Checked='<%# Eval("RoHS") %>' Enabled="false" />
                        </td>
                        <td>
                            <asp:Label runat="server" CssClass="left" Text='<%# Eval("NazwaRodzajuProduktu") %>' />
                            <asp:Label runat="server" CssClass="floatRight" Text='<%# Eval("KontoKsiegowe") %>' />
                            <br />
                            <span class="line2">
                                <asp:Label ID="Label3" runat="server" CssClass="left" Text='<%# Eval("NazwaDostawcy") %>' />
                                <asp:Label ID="StatusLabel" CssClass="floatRight" runat="server" Text='<%# Eval("Status") %>' /><br />
                                <asp:Label ID="Label16" runat="server" CssClass="left" Text='<%# Eval("DataMaila", "{0:dd-MM-yyyy}") %>' />
                            </span>
                            
                        </td>
                        <td>
                            <asp:Label runat="server" CssClass="left" Text='<%# Eval("PowodZakupu") %>' />
                            <div class="floatRight">
                                <asp:Label ID="DataDostawyLabel" runat="server" Text='<%# Eval("DataDostawy", "{0:dd-MM-yyyy}") %>'
                                    Visible='<%# ((int)Eval("IdStatusu")) > 3 ? true : false %>' />
                            </div>
                            <div class="floatRight zamowiono">
                                <uc3:DateEdit ID="DataDostawyDateEdit" runat="server" Visible='<%# ZamowionoVisible((int)Eval("IdStatusu")) %>' />
                            </div>
                            <br />
                            <span class="line2">
                                <asp:Label runat="server" CssClass="left" Text='<%# Eval("Notatka") %>' />
                                <div class="floatRight">
                                    <asp:Label ID="LokalizacjaOdbioruLabel" runat="server" Text='<%# Eval("LokalizacjaOdbioru") %>'
                                        Visible='<%# ((int)Eval("IdStatusu")) > 4 ? true : false %>' />
                                    <asp:TextBox ID="LokalizacjaOdbioruTextBox" runat="server" Visible='<%# DostarczonoVisible((int)Eval("IdStatusu")) %>'
                                        Width="100px" />
                                </div>
                            </span>
                        </td>
                        <td id="tdControl" runat="server" class="control" style="width: 100px !important;">
                            <div style="text-align: center">
                                <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                <asp:HiddenField ID="hidIdZamowienia" runat="server" Value='<%# Eval("IdZamowienia") %>' />
                                <asp:HiddenField ID="hidIdPozycji" runat="server" Value='<%# Eval("Id") %>' />
                                <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" Style="display: none;"><img alt="" src="../images/uploading.gif" /></asp:Label>
                                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="fileupload1"
                                    ToolTip="Wybierz plik" UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                                    UploaderStyle="Modern" ThrobberID="imgLoader" OnUploadedComplete="FileUploadComplete"
                                    OnUploadedFileError="FileUploadError" />
                                <br />
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <br />
                            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" Visible='<%# EdycjaVisible() %>'
                                Width="80px" /><br />
                            <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible='<%# EdycjaVisible() %>'
                                Width="80px" /><br />
                            <asp:Button ID="ZapiszProduktButton" runat="server" CommandName="ZapiszProdukt" Text="Utrwal"
                                Visible='<%# ZapiszProduktVisible(Eval("IdProduktu")) %>' Width="80px" /><br />
                            <asp:Button ID="RejectButton" runat="server" CommandName="Reject" Text="Odrzuć" Visible='<%# RejectVisible((int)Eval("Odrzuc")) %>'
                                Width="80px" />
                            <asp:Button ID="RevertButton" runat="server" CommandName="Revert" Text="Przywróć"
                                Visible='<%# RevertVisible((int)Eval("Przywroc")) %>' Width="80px" />
                            <asp:Button ID="ZamowionoButton" runat="server" CommandName="Zamowiono" Text="Zamówiono"
                                Visible='<%# ZamowionoVisible((int)Eval("IdStatusu")) %>' Width="80px" />
                            <asp:Button ID="DostarczonoButton" runat="server" CommandName="Dostarczono" Text="Dostarczono"
                                Visible='<%# DostarczonoVisible((int)Eval("IdStatusu")) %>' Width="90px" />
                            <asp:Button ID="OdebranoButton" runat="server" CommandName="Odebrano" Text="Odebrano"
                                Visible='<%# OdebranoVisible((int)Eval("IdStatusu")) %>' Width="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td ID="plikiTD" colspan="8" runat="server" visible='<%# IsPliki((int)Eval("Id")) %>'>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:ListView ID="plikiListView" runat="server" DataSource='<%# GetPlikiDataSource((int)Eval("Id")) %>'
                                        OnItemCommand="pliki_ItemCommand">
                                        <LayoutTemplate>
                                            <div class="pliki" style="display: inline">
                                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                            </div>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <div class="plik">
                                                <asp:Image ID="PlikImage" runat="server" ImageUrl='~/images/fileext/any_icon.gif' />
                                                <asp:LinkButton ID="PlikButton" runat="server" CommandName="Download" CommandArgument="<%# GetFileId(Container.DataItem) %>"
                                                    Text="<%# GetFilename(Container.DataItem)%>" ToolTip="<%# GetTooltip(Container.DataItem)%>"/>
                                                <asp:ImageButton ID="DeleteButton" TabIndex="-1" runat="server" ImageUrl="~/images/buttons/delete.png" CommandArgument="<%# GetFileId(Container.DataItem) %>" CommandName="DeletePlik" AlternateText="Usuń" ToolTip="Usuń Plik" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="plikiListView" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr class="eit">
                        <td colspan="7" class="col1">
                            <table class="edit">
                                <tr>
                                    <td class="coled1">
                                        <div class="title">
                                            <asp:Label runat="server" Text="Dane podstawowe" />
                                        </div>
                                        <span class="label">Nazwa:</span>
                                        <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                                        <asp:RequiredFieldValidator ControlToValidate="NazwaTextBox" runat="server" SetFocusOnError="True"
                                            Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Part No:</span>
                                        <asp:TextBox ID="PartNoTextBox" runat="server" Text='<%# Bind("PartNo") %>' />
                                        <br />
                                        <span class="label">Opis:</span>
                                        <asp:TextBox ID="OpisTextBox" Width="200px" Rows="6" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Opis") %>' />
                                        <asp:RequiredFieldValidator ControlToValidate="OpisTextBox" runat="server" SetFocusOnError="True"
                                            Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Ilość:</span>
                                        <asp:TextBox ID="IloscTextBox" runat="server" Text='<%# Bind("Ilosc") %>' />
                                        <asp:RequiredFieldValidator ControlToValidate="IloscTextBox" runat="server" SetFocusOnError="True"
                                            Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Jednostka:</span>
                                        <asp:TextBox ID="JednostkaTextBox" runat="server" Text='<%# Bind("Jednostka") %>' />
                                        <br />
                                        <span class="label">Cena:</span>
                                        <asp:TextBox ID="CenaTextBox" runat="server" Text='<%# Bind("Cena") %>' />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="CenaTextBox"
                                            runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge"
                                            CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Waluta:</span>
                                        <asp:DropDownList ID="WalutaDropDownList" runat="server" DataTextField="Symbol" Width="206px"
                                            DataValueField="Symbol" DataSourceID="walutyDataSource" SelectedValue='<%# Bind("Waluta") %>'>
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Produkt stockowy:</span>
                                        <asp:CheckBox ID="ProduktStockowyCheckBox" class="check" runat="server" Checked='<%# Bind("ProduktStockowy") %>' /><br />
                                        <span class="label">RoHS:</span>
                                        <asp:CheckBox ID="RoHSCheckBox" class="check" runat="server" Checked='<%# Bind("RoHS") %>' /><br />
                                    </td>
                                    <td class="coled2">
                                        <div class="title">
                                            <asp:Label runat="server" Text="Dane dodatkowe" />
                                        </div>
                                        <span class="label">Product Class:</span>
                                        <asp:TextBox ID="ProductClass" runat="server" Text='<%# Bind("ProductClass") %>' />
                                        <br />
                                        <span class="label">Product Subclass: </span>
                                        <asp:TextBox ID="ProductSubClass" runat="server" Text='<%# Bind("ProductSubclass") %>' />
                                        <br />
                                        <span class="label">Rodzaj produktu:</span>
                                        <asp:DropDownList ID="RodzajProduktuDropDownList" runat="server" DataTextField="Nazwa"
                                            Width="246px" DataValueField="Id" DataSourceID="rodzajeProduktowDataSource" SelectedValue='<%# Bind("RodzajProduktu") %>'>
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Konto księgowe:</span>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("KontoKsiegowe") %>' />
                                        <br />
                                        <span class="label">Dostawca:</span>
                                        <asp:DropDownList ID="DostawcaDropDownList" runat="server" DataTextField="Nazwa"
                                            Width="246px" DataValueField="Id" DataSourceID="dostawcyDataSource" SelectedValue='<%# Bind("Dostawca") %>'>
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Status:</span>
                                        <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' Visible='<%# !EdycjaStatusuPozycjiVisible() %>' />
                                        <asp:DropDownList ID="StatusDropDownList" runat="server" DataTextField="Nazwa" Width="246px"
                                            Visible='<%# EdycjaStatusuPozycjiVisible() %>' DataValueField="Id" DataSourceID="statusyPozycjiDataSource"
                                            SelectedValue='<%# Bind("IdStatusu") %>'>
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Powod zakupu:</span>
                                        <asp:TextBox ID="TextBox1" Width="240px" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("PowodZakupu") %>' />
                                        <br />
                                        <span class="label">Notatka:</span>
                                        <asp:TextBox ID="TextBox3" Width="240px" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Notatka") %>' />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="control" style="width: 100px !important;">
                            <asp:Button ID="UpdateButton1" runat="server" CssClass="utworzZamowienieButton" CommandName="Update" Text="Zapisz pozycję" ValidationGroup="vge"
                                Width="80px" /><br />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" Width="80px" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <tr class="iit">
                        <td colspan="7" class="col1">
                            <table class="edit">
                                <tr>
                                    <td class="coled1">
                                        <div class="title">
                                            <asp:Label ID="Label1" runat="server" Text="Dane podstawowe" />
                                        </div>
                                        <span class="label">Nazwa:</span>
                                        <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                                        <asp:AutoCompleteExtender runat="server" ID="ProduktAutoCompleteExtender" TargetControlID="NazwaTextBox"
                                            ServiceMethod="SzukajProdukty" ServicePath="~/main.asmx" MinimumPrefixLength="1"
                                            FirstRowSelected="true" CompletionInterval="100" CompletionListCssClass="completionList"
                                            CompletionListItemCssClass="listItem" CompletionListHighlightedItemCssClass="itemHighlighted"
                                            OnClientItemSelected="ProduktItemSelected" BehaviorID="ProduktAutoCompleteEx"
                                            OnClientPopulated="ProduktOnListPopulated">
                                        </asp:AutoCompleteExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="NazwaTextBox"
                                            runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge"
                                            CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Part No:</span>
                                        <asp:TextBox ID="PartNoTextBox" runat="server" Text='<%# Bind("PartNo") %>' />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="PartNoTextBox"
                                            runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge"
                                            CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Opis:</span>
                                        <asp:TextBox ID="OpisTextBox" Width="200px" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Opis") %>' />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="OpisTextBox"
                                            runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge"
                                            CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Ilość:</span>
                                        <asp:TextBox ID="IloscTextBox" runat="server" Text='<%# Bind("Ilosc") %>' />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="IloscTextBox"
                                            runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge"
                                            CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Jednostka:</span>
                                        <asp:DropDownList ID="JednostkaDropDownList" runat="server" DataTextField="Symbol"
                                            Width="206px" DataValueField="Symbol" DataSourceID="jednostkiDataSource">
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Cena:</span>
                                        <asp:TextBox ID="CenaTextBox" runat="server" Text='<%# Bind("Cena") %>' />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="CenaTextBox"
                                            runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge"
                                            CssClass="error" ErrorMessage="Błąd" />
                                        <br />
                                        <span class="label">Waluta:</span>
                                        <asp:DropDownList ID="WalutaDropDownList" runat="server" DataTextField="Symbol" Width="206px"
                                            DataValueField="Symbol" DataSourceID="walutyDataSource">
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Produkt stockowy:</span>
                                        <asp:CheckBox ID="ProduktStockowyCheckBox" class="check" runat="server" Checked='<%# Bind("ProduktStockowy") %>' />
                                        <br />
                                        <span class="label">RoHS:</span>
                                        <asp:CheckBox ID="RoHSCheckBox" class="check" runat="server" Checked='<%# Bind("RoHS") %>' />
                                        <br />
                                    </td>
                                    <td class="coled2">
                                        <div class="title">
                                            <asp:Label ID="Label2" runat="server" Text="Dane dodatkowe" />
                                        </div>
                                        <span class="label">Product Class:</span>
                                        <asp:TextBox ID="ProductClass" runat="server" Text='<%# Bind("ProductClass") %>' />
                                        <br />
                                        <span class="label">Product Subclass: </span>
                                        <asp:TextBox ID="ProductSubClass" runat="server" Text='<%# Bind("ProductSubclass") %>' />
                                        <br />
                                        <span class="label">Rodzaj produktu:</span>
                                        <asp:DropDownList ID="RodzajProduktuDropDownList" runat="server" DataTextField="Nazwa"
                                            Width="246px" DataValueField="Id" DataSourceID="rodzajeProduktowDataSource">
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Konto księgowe:</span>
                                        <asp:TextBox ID="KontoKsiegoweTextBox" runat="server" Text='<%# Bind("KontoKsiegowe") %>' />
                                        <br />
                                        <span class="label">Dostawca:</span>
                                        <asp:DropDownList ID="DostawcaDropDownList" runat="server" DataTextField="Nazwa"
                                            Width="246px" DataValueField="Id" DataSourceID="dostawcyDataSource">
                                        </asp:DropDownList>
                                        <br />
                                        <span class="label">Powod zakupu:</span>
                                        <asp:TextBox ID="TextBox1" Width="240px" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("PowodZakupu") %>' />
                                        <br />
                                        <span class="label">Notatka:</span>
                                        <asp:TextBox ID="TextBox3" Width="240px" Rows="5" TextMode="multiline" class="textbox"
                                            MaxLength="500" runat="server" Text='<%# Bind("Notatka") %>' />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="control" style="width: 100px !important;">
                            <asp:Button ID="InsertButton1" runat="server" CommandName="Insert" CssClass="pozycjeDodajButton" Text="Zapisz pozycję" ValidationGroup="vgi"
                                 /><br />
                            <asp:Button ID="CancelInsertButton" runat="server" CommandName="CancelInsert" Text="Anuluj"
                                Width="80px" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EmptyDataTemplate>
                    <table id="Table1" class="hoverline cntPracownicy2 ListView1 tbBrowser" runat="server">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" colspan="2" runat="server">
                                <table id="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0"
                                    style="">
                                    <tr id="Tr2" runat="server" style="">
                                        <th id="Th1" rowspan="2" runat="server">
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Nazwa"
                                                Text="Nazwa" />
                                            <div class="line2">
                                                <asp:LinkButton ID="LinkButton2" CssClass="left" runat="server" CommandName="Sort"
                                                    CommandArgument="Opis" Text="Opis" />
                                            </div>
                                        </th>
                                        <th id="Th2" rowspan="2" runat="server">
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Ilosc"
                                                Text="Ilość" />
                                            <div class="line2">
                                                <asp:LinkButton ID="LinkButton4" CssClass="left" runat="server" CommandName="Sort"
                                                    CommandArgument="Cena" Text="Cena" />
                                            </div>
                                        </th>
                                        <th id="Th3" rowspan="2" runat="server" class="check">
                                            <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="ProduktStockowy"
                                                Text="Stock" />
                                        </th>
                                        <th id="Th4" rowspan="2" runat="server" class="check">
                                            <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="RoHS"
                                                Text="RoHS" />
                                        </th>
                                        <th id="Th5" rowspan="2" runat="server">
                                            <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="RodzajProduktu"
                                                Text="Rodzaj" />
                                            <asp:LinkButton ID="LinkButton8" runat="server" CssClass="floatRight" CommandName="Sort"
                                                CommandArgument="KontoKsiegowe" Text="Konto księgowe" />
                                            <div class="line2">
                                                <asp:LinkButton ID="LinkButton9" CssClass="left" runat="server" CommandName="Sort"
                                                    CommandArgument="Dostawca" Text="Dostawca" />
                                                <asp:LinkButton ID="LinkButton12" CssClass="floatRight" runat="server" CommandName="Sort"
                                                    CommandArgument="Status" Text="Status" /><br />
                                                <asp:LinkButton ID="LinkButton22" runat="server" CssClass="left" CommandName="Sort"
                                                CommandArgument="DataMaila" Text="Data wysyłki maila" />
                                            </div>
                                        </th>
                                        <th id="Th6" rowspan="2" runat="server">
                                            <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="PowodZakupu"
                                                Text="Powod zakupu" />
                                            <asp:LinkButton ID="LinkButton13" runat="server" CssClass="floatRight" CommandName="Sort"
                                                CommandArgument="DataDostawy" Text="Data Dostawy" />
                                            <div class="line2">
                                                <asp:LinkButton ID="LinkButton11" CssClass="left" runat="server" CommandName="Sort"
                                                    CommandArgument="Notatka" Text="Notatka" />
                                                <asp:LinkButton ID="LinkButton14" runat="server" CssClass="floatRight" CommandName="Sort"
                                                    CommandArgument="LokalizacjaOdbioru" Text="Lokalizacja Odbioru" />
                                            </div>
                                        </th>
                                        <th id="Th7" class="control" rowspan="2" runat="server" style="width: 100px !important;">
                                            <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj Pozycję"
                                                ToolTip="Dodaj nowy rekord" CssClass="pozycjeDodajButton dodawanieVisible" Visible='<%# DodawanieVisible() %>'
                                                 />
                                        </th>
                                    </tr>
                                    <tr class="bottom">
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table id="Table1" class="hoverline cntPracownicy2 ListView1 tbBrowser" runat="server">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" colspan="2" runat="server">
                                <table id="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0"
                                    style="">
                                    <tr id="Tr2" runat="server" style="">
                                        <th rowspan="2" runat="server">
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" />
                                            <div class="line2">
                                                <asp:LinkButton CssClass="left" runat="server" CommandName="Sort" CommandArgument="Opis"
                                                    Text="Opis" />
                                            </div>
                                        </th>
                                        <th id="Th12" rowspan="2" runat="server">
                                            <asp:LinkButton ID="LinkButton16" runat="server" CommandName="Sort" CommandArgument="Part Number"
                                                Text="Part Number" />
                                            <div class="line2">
                                                <asp:LinkButton ID="LinkButton20" CssClass="left" runat="server" CommandName="Sort"
                                                    CommandArgument="ProductClass" Text="Product Class" />
                                            </div>
                                            <div class="line2">
                                                <asp:LinkButton ID="LinkButton18" CssClass="left" runat="server" CommandName="Sort"
                                                    CommandArgument="ProductSubclass" Text="Product Subclass" />
                                            </div>
                                        </th>
                                        <th rowspan="2" runat="server">
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="Ilosc" Text="Ilość" />
                                            <div class="line2">
                                                <asp:LinkButton CssClass="left" runat="server" CommandName="Sort" CommandArgument="Cena"
                                                    Text="Cena Jedn." />
                                            </div>
                                            <div class="line2">
                                                <asp:LinkButton ID="LinkButton21" CssClass="left" runat="server" CommandName="Sort" CommandArgument="Wartosc"
                                                    Text="Wartosc" />
                                            </div>
                                        </th>
                                        <th rowspan="2" runat="server" class="check">
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="ProduktStockowy"
                                                Text="Stock" />
                                        </th>
                                        <th rowspan="2" runat="server" class="check">
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="RoHS" Text="RoHS" />
                                        </th>
                                        <th rowspan="2" runat="server">
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="RodzajProduktu"
                                                Text="Rodzaj" />
                                            <asp:LinkButton runat="server" CssClass="floatRight" CommandName="Sort" CommandArgument="KontoKsiegowe"
                                                Text="Konto księgowe" />
                                            <div class="line2">
                                                <asp:LinkButton CssClass="left" runat="server" CommandName="Sort" CommandArgument="Dostawca"
                                                    Text="Dostawca" />
                                                <asp:LinkButton ID="LinkButton12" CssClass="floatRight" runat="server" CommandName="Sort"
                                                    CommandArgument="Status" Text="Status" /><br />
                                                    <asp:LinkButton ID="LinkButton22" runat="server" CssClass="left" CommandName="Sort"
                                                CommandArgument="DataMaila" Text="Data wysyłki maila" />
                                            </div>
                                        </th>
                                        <th rowspan="2" runat="server">
                                            <asp:LinkButton runat="server" CommandName="Sort" CommandArgument="PowodZakupu" Text="Powod zakupu" />
                                            <asp:LinkButton ID="LinkButton13" runat="server" CssClass="floatRight" CommandName="Sort"
                                                CommandArgument="DataDostawy" Text="Data Dostawy" />
                                            <div class="line2">
                                                <asp:LinkButton CssClass="left" runat="server" CommandName="Sort" CommandArgument="Notatka"
                                                    Text="Notatka" />
                                                <asp:LinkButton ID="LinkButton14" runat="server" CssClass="floatRight" CommandName="Sort"
                                                    CommandArgument="LokalizacjaOdbioru" Text="Lokalizacja Odbioru" />
                                            </div>
                                        </th>
                                        <th class="control" rowspan="2" runat="server">
                                            <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj pozycję"
                                                ToolTip="Dodaj nowy rekord" CssClass="pozycjeDodajButton dodawanieVisible" Visible='<%# DodawanieVisible() %>'
                                                 />
                                        </th>
                                    </tr>
                                    <tr class="bottom">
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <br />
            <asp:ListView ID="sciezkaAkceptacjiListView" runat="server" DataSourceID="SciezkaAkceptacjiDataSource"
                OnItemDataBound="sciezkaAkceptacjiListView_OnItemDataBound" DataKeyNames="Id"
                Visible="false" OnItemUpdating="sciezkaAkceptacjiListView_OnItemUpdating" OnItemInserting="sciezkaAkceptacjiListView_ItemInserting"
                OnItemCommand="sciezkaAkceptacjiListView_ItemCommand" OnItemInserted="sciezkaAkceptacjiListView_ItemInserted">
                <ItemTemplate>
                    <tr class="it" id="trLine" runat="server">
                        <td>
                            <asp:Label ID="PoziomAkceptacji" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("PoziomAkceptacji") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Pracownik" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("Pracownik") %>' />
                        </td>
                        <td>
                            <asp:Label ID="NazwaStatusu" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("NazwaStatusu") %>' />
                        </td>
                        <td id="tdControl" runat="server" class="control" style="width: 100px !important;">
                            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" Visible='<%# EdycjaSciezkiAkceptacjiVisible() %>'
                                Width="80px" /><br />
                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr class="eit">
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass="left" Text='<%# Eval("PoziomAkceptacji") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" CssClass="left" Text='<%# Eval("Pracownik") %>' />
                        </td>
                        <td>
                            <asp:DropDownList ID="StatusAkceptacjiDropDownList" runat="server" DataTextField="Nazwa"
                                Width="250px" DataValueField="Status" DataSourceID="StatusyAkceptacjiDataSource">
                            </asp:DropDownList>
                        </td>
                        <td class="control" style="width: 100px !important;">
                            <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" Width="80px" /><br />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" Width="80px" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <tr class="iit">
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass="left" Text="Dodane przez Administratora" />
                        </td>
                        <td>
                            <asp:DropDownList ID="PracownicyDropDownList" runat="server" DataTextField="Pracownik"
                                Width="250px" DataValueField="Id" DataSourceID="PracownicyDataSource">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" CssClass="left" Text="Oczekuje na akceptację" />
                        </td>
                        <td class="control" style="width: 100px !important;">
                            <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" Width="80px" /><br />
                            <asp:Button ID="CancelInsertButton" runat="server" CommandName="CancelInsert" Text="Anuluj"
                                Width="80px" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EmptyDataTemplate>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table id="Table1" class="hoverline cntPracownicy2 ListView1 tbBrowser" runat="server">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" colspan="2" runat="server">
                                <table id="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0"
                                    style="">
                                    <tr id="Tr2" runat="server" style="">
                                        <th id="Th8" rowspan="2" runat="server">
                                            <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="PoziomAkceptacji"
                                                Text="Poziom Akceptacji" />
                                        </th>
                                        <th id="Th9" rowspan="2" runat="server">
                                            <asp:LinkButton ID="LinkButton17" runat="server" CommandName="Sort" CommandArgument="Pracownik"
                                                Text="Pracownik" />
                                        </th>
                                        <th id="Th10" rowspan="2" runat="server" class="check">
                                            <asp:LinkButton ID="LinkButton19" runat="server" CommandName="Sort" CommandArgument="Status"
                                                Text="Status" />
                                        </th>
                                        <th id="Th11" class="control" runat="server" style="width: 100px !important;">
                                            <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj"
                                                ToolTip="Dodaj nowy rekord" CssClass="edycjaSciezkiAkceptacjiVisible" Visible='<%# EdycjaSciezkiAkceptacjiVisible() %>'
                                                Width="80px" />
                                        </th>
                                    </tr>
                                    <tr class="bottom">
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <br />
            <br />
            <table style="width: 100%; margin-left: -1px; border-collapse: collapse;" class="wyborSciezki">
                <tr>
                    <td colspan="2" style="text-align: center; color: Red;">
                        <asp:Label ID="Label10" runat="server" Text="Zmiany w zamówieniu spowodowały wygenerowanie nowej ścieżki akceptacji"
                            Visible='<%# WyborSciezki() %>' />
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 5px; text-align: center;">
                        <asp:Label ID="Label7" runat="server" Text="Poprzednia ścieżka akceptacji" Visible='<%# WyborSciezki() %>' />
                    </td>
                    <td style="padding-left: 5px; text-align: center;">
                        <asp:Label ID="Label8" runat="server" Text="Nowa ścieżka akceptacji" Visible='<%# WyborSciezki() %>' />
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 5px;">
                        <asp:ListView ID="staraSciezkaAkceptacjiListView" runat="server" DataSourceID="staraSciezkaAkceptacjiDataSource"
                            DataKeyNames="Id">
                            <ItemTemplate>
                                <tr class="it" id="trLine" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("PoziomAkceptacji") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("Pracownik") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("NazwaStatusu") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="Table1" class="hoverline cntPracownicy2 ListView1 tbBrowser" runat="server">
                                    <tr id="Tr1" runat="server">
                                        <td id="Td1" colspan="2" runat="server">
                                            <table id="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0"
                                                style="">
                                                <tr id="Tr2" runat="server" style="">
                                                    <th id="Th8" rowspan="2" runat="server">
                                                        <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="PoziomAkceptacji"
                                                            Text="Poziom Akceptacji" />
                                                    </th>
                                                    <th id="Th9" rowspan="2" runat="server">
                                                        <asp:LinkButton ID="LinkButton17" runat="server" CommandName="Sort" CommandArgument="Pracownik"
                                                            Text="Pracownik" />
                                                    </th>
                                                    <th id="Th10" rowspan="2" runat="server" class="check">
                                                        <asp:LinkButton ID="LinkButton19" runat="server" CommandName="Sort" CommandArgument="Status"
                                                            Text="Status" />
                                                    </th>
                                                </tr>
                                                <tr class="bottom">
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </td>
                    <td style="padding-left: 5px;">
                        <asp:ListView ID="nowaSciezkaAkceptacjiListView" runat="server" DataSourceID="nowaSciezkaAkceptacjiDataSource"
                            DataKeyNames="Id">
                            <ItemTemplate>
                                <tr class="it" id="trLine" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("PoziomAkceptacji") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("Pracownik") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" CssClass='<%# Eval("CSS") %>' Text='<%# Eval("NazwaStatusu") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="Table1" class="hoverline cntPracownicy2 ListView1 tbBrowser" runat="server">
                                    <tr id="Tr1" runat="server">
                                        <td id="Td1" colspan="2" runat="server">
                                            <table id="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0"
                                                style="">
                                                <tr id="Tr2" runat="server" style="">
                                                    <th id="Th8" rowspan="2" runat="server">
                                                        <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="PoziomAkceptacji"
                                                            Text="Poziom Akceptacji" />
                                                    </th>
                                                    <th id="Th9" rowspan="2" runat="server">
                                                        <asp:LinkButton ID="LinkButton17" runat="server" CommandName="Sort" CommandArgument="Pracownik"
                                                            Text="Pracownik" />
                                                    </th>
                                                    <th id="Th10" rowspan="2" runat="server" class="check">
                                                        <asp:LinkButton ID="LinkButton19" runat="server" CommandName="Sort" CommandArgument="Status"
                                                            Text="Status" />
                                                    </th>
                                                </tr>
                                                <tr class="bottom">
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 5px; text-align: center;">
                        <asp:Button ID="btNewRecord" runat="server" Text="Wybierz poprzednią ścieżkę" CssClass="newButton"
                            Width="200px" Visible='<%# WyborSciezki() %>' OnClick="StaraSciezka_OnClick" />
                    </td>
                    <td style="padding-left: 5px; text-align: center;">
                        <asp:Button ID="Button1" runat="server" Text="Wybierz nową ścieżkę" CssClass="newButton"
                            Width="200px" Visible='<%# WyborSciezki() %>' OnClick="NowaSciezka_OnClick" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:SqlDataSource ID="magazynyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Nazwa FROM IPO_Magazyny ORDER BY Nazwa"></asp:SqlDataSource>
<asp:SqlDataSource ID="dostawcyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Nazwa FROM IPO_Dostawcy ORDER BY Nazwa"></asp:SqlDataSource>
<asp:SqlDataSource ID="rodzajeProduktowDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Nazwa FROM IPO_RodzajeProduktow ORDER BY Id"></asp:SqlDataSource>
<asp:SqlDataSource ID="walutyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Lp, Symbol FROM IPO_Waluty ORDER BY Lp, Symbol"></asp:SqlDataSource>
<asp:SqlDataSource ID="jednostkiDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Symbol FROM IPO_Jednostki ORDER BY Id"></asp:SqlDataSource>
<asp:SqlDataSource ID="statusyPozycjiDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Nazwa FROM IPO_Statusy ORDER BY Id"></asp:SqlDataSource>
<asp:SqlDataSource ID="StatusyAkceptacjiDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id AS Status, Nazwa, Edytowalny FROM IPO_StatusyAkceptacji WHERE Edytowalny = 1">
</asp:SqlDataSource>
<asp:SqlDataSource ID="PracownicyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT Id, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik FROM Pracownicy WHERE Status != -1 
                    -- AND KadryId &lt; 80000
                    ORDER BY Pracownik"></asp:SqlDataSource>
<asp:SqlDataSource ID="CCDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="IF EXISTS (SELECT 1 FROM IPO_ccPrawa WHERE UserId = @UserId AND IdCC = 0)
                       SELECT DISTINCT Id, cc FROM CC ORDER BY cc
                   ELSE
                       SELECT DISTINCT IdCC AS Id, CC AS cc FROM IPO_ccPrawa WHERE UserId = @UserId ORDER BY cc                   
                   ">
    <SelectParameters>
        <asp:Parameter Name="UserId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="CCOptDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="IF EXISTS (SELECT Id FROM IPO_ccPrawa WHERE UserId = @UserId AND IdCC = 0)
                       SELECT 0 AS Sort, NULL as Id, ' ' AS cc UNION SELECT DISTINCT 1 AS Sort, Id, cc FROM CC ORDER BY Sort, cc
                   ELSE
                       SELECT 0 AS Sort, NULL as Id, ' ' AS cc UNION SELECT DISTINCT 1 AS Sort, IdCC AS Id, CC AS cc FROM IPO_ccPrawa WHERE UserId = @UserId ORDER BY Sort, cc                   
                   ">
    <SelectParameters>
        <asp:Parameter Name="UserId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="zamowieniaDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>" CancelSelectOnNullParameter="false" OnInserted="SqlDataSource1_Inserted"
    SelectCommand="
declare @emp varchar(1)
declare @sep varchar(10)
declare @nodata varchar(50)
set @emp = ''
set @sep = ', '
set @nodata = ''
   
    SELECT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient, 
    IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, 
    IPO_Zamowienia.CERNr AS CERNr, IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, 
    IPO_Zamowienia.DataUtworzenia, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
     (
select  distinct
ISNULL(STUFF((
select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
from IPO_CCZamowienia W
left join CC on CC.Id = W.IdCC
where W.IdZamowienia = S.IdZamowienia
order by W.Udzial desc, CC.cc
for XML PATH('')
), 1, 1, @emp), @nodata) 
from IPO_CCZamowienia S
where S.IdZamowienia = IPO_Zamowienia.Id
) as CCList
    FROM IPO_Zamowienia 
    JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    ORDER BY Id DESC"
    
        InsertCommand="INSERT INTO IPO_Zamowienia(IdPracownika, Klient, IdMagazynu, CER, CERNr, IdStatusu) 
        VALUES(@Pracownik, @Klient, @Magazyn, @CER, @CERNr, @Status) 
        
         SET @LastId = SCOPE_IDENTITY()
        

        ">
 
    
    
 <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Status1" Type="Int32" />
        <asp:Parameter Name="Status2" Type="Int32" />
</SelectParameters>
<InsertParameters>
    <asp:Parameter Name="LastId" Type="Int32" Direction="Output" DefaultValue="0"/>
    <asp:Parameter Name="Pracownik" Type="Int32" />
    <asp:Parameter Name="Klient" Type="String" />
    <asp:Parameter Name="Magazyn" Type="Int32" />
    <asp:Parameter Name="CER" Type="Boolean" />
    <asp:Parameter Name="CERNr" Type="String" />
    <asp:Parameter Name="Status" Type="Int32" />
</InsertParameters>    
    </asp:SqlDataSource>
<asp:SqlDataSource ID="zamawiajacyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
declare @emp varchar(1)
declare @sep varchar(10)
declare @nodata varchar(50)
set @emp = ''
set @sep = ', '
set @nodata = ''

    
    SELECT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient, 
    IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr, 
    IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
    Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,   
 (
select  distinct
ISNULL(STUFF((
select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
from IPO_CCZamowienia W
left join CC on CC.Id = W.IdCC
where W.IdZamowienia = S.IdZamowienia
order by W.Udzial desc, CC.cc
for XML PATH('')
), 1, 1, @emp), @nodata) 
from IPO_CCZamowienia S
where S.IdZamowienia = IPO_Zamowienia.Id
) as CCList
    FROM IPO_Zamowienia 
    JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    WHERE IPO_Zamowienia.IdPracownika = @IdPracownika AND IPO_Zamowienia.IdStatusu = @Status 
    ORDER BY Id DESC" 
        InsertCommand="INSERT INTO IPO_Zamowienia(IdPracownika, Klient, IdMagazynu, CER, CERNr, IdStatusu) 
        VALUES(@Pracownik, @Klient, @Magazyn, @CER, @CERNr, @Status) 
        
         SET @LastId = SCOPE_IDENTITY()"
        

       >
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="LastId" Type="Int32" Direction="Output" DefaultValue="0"/>
        <asp:Parameter Name="Pracownik" Type="Int32" />
        <asp:Parameter Name="Klient" Type="String" />
        <asp:Parameter Name="Magazyn" Type="Int32" />
        <asp:Parameter Name="CER" Type="Boolean" />
        <asp:Parameter Name="CERNr" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="CC1" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="CC2" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="CC3" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="Udzial1" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="Udzial2" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="Udzial3" Type="Int32" DefaultValue="0" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="zamawiajacyZODataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
    SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
    FROM IPO_Zamowienia 
    JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    JOIN IPO_PozycjeZamowien ON IPO_PozycjeZamowien.IdZamowienia = IPO_Zamowienia.Id AND (IPO_PozycjeZamowien.IdStatusu = @Status1 OR IPO_PozycjeZamowien.IdStatusu = @Status2)
    WHERE IPO_Zamowienia.IdPracownika = @IdPracownika
    ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status1" Type="Int32" />
        <asp:Parameter Name="Status2" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="akceptujacy1DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
    SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
    FROM IPO_Zamowienia JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
    JOIN IPO_SciezkaAkceptacji ON IPO_Zamowienia.Id = IPO_SciezkaAkceptacji.IdZamowienia 
    WHERE IPO_SciezkaAkceptacji.UserId = @IdPracownika AND IPO_SciezkaAkceptacji.Status = 0 AND IPO_SciezkaAkceptacji.PoziomAkceptacji = IPO_Zamowienia.PoziomAkceptacji AND IPO_Zamowienia.IdStatusu = @Status ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="akceptujacy2DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
    SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
 FROM IPO_Zamowienia JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika
  JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
  JOIN IPO_SciezkaAkceptacji ON IPO_Zamowienia.Id = IPO_SciezkaAkceptacji.IdZamowienia 
  WHERE IPO_SciezkaAkceptacji.UserId = @IdPracownika AND IPO_SciezkaAkceptacji.Status &gt; 0 
  AND IPO_Zamowienia.IdStatusu IN (2, 3, 4, 5) ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="akceptujacyZOSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
    SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
    FROM IPO_Zamowienia JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
    JOIN IPO_SciezkaAkceptacji ON IPO_Zamowienia.Id = IPO_SciezkaAkceptacji.IdZamowienia
    JOIN IPO_PozycjeZamowien ON IPO_PozycjeZamowien.IdZamowienia = IPO_Zamowienia.Id AND IPO_PozycjeZamowien.IdStatusu = @Status
    WHERE IPO_SciezkaAkceptacji.UserId = @IdPracownika ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="notyfikowanyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
SELECT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
    FROM IPO_Zamowienia 
    JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
    JOIN IPO_ccPrawa ON IPO_CCZamowienia.IdCC = IPO_ccPrawa.IdCC OR IPO_ccPrawa.IdCC = 0
    WHERE IPO_ccPrawa.UserId = @IdPracownika 
    AND IPO_ccPrawa.RolaId = 4 
    AND IPO_Zamowienia.IdStatusu = @Status 
    ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="notyfikowanyZODataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
    SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
    FROM IPO_Zamowienia 
    JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
    JOIN IPO_ccPrawa ON IPO_CCZamowienia.IdCC = IPO_ccPrawa.IdCC OR IPO_ccPrawa.IdCC = 0
    JOIN IPO_PozycjeZamowien ON IPO_PozycjeZamowien.IdZamowienia = IPO_Zamowienia.Id AND (IPO_PozycjeZamowien.IdStatusu = @Status1 OR IPO_PozycjeZamowien.IdStatusu = @Status2)
    WHERE IPO_ccPrawa.UserId = @IdPracownika 
    AND IPO_ccPrawa.RolaId = 4 
    ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status1" Type="Int32" />
        <asp:Parameter Name="Status2" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="kupiecDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
    SELECT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
       FROM IPO_Zamowienia JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
       JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
       JOIN IPO_ccPrawa ON IPO_CCZamowienia.IdCC = IPO_ccPrawa.IdCC OR IPO_ccPrawa.IdCC = 0 
       WHERE IPO_ccPrawa.UserId = @IdPracownika AND IPO_ccPrawa.RolaId = 3 
       AND (IPO_Zamowienia.IdStatusu = @Status1 OR IPO_Zamowienia.IdStatusu = @Status2) 
       ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status1" Type="Int32" />
        <asp:Parameter Name="Status2" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="kupiecZODataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
    SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
    FROM IPO_Zamowienia 
    JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
    JOIN IPO_ccPrawa ON IPO_CCZamowienia.IdCC = IPO_ccPrawa.IdCC OR IPO_ccPrawa.IdCC = 0
    JOIN IPO_PozycjeZamowien ON IPO_PozycjeZamowien.IdZamowienia = IPO_Zamowienia.Id AND (IPO_PozycjeZamowien.IdStatusu = @Status1 OR IPO_PozycjeZamowien.IdStatusu = @Status2 OR IPO_PozycjeZamowien.IdStatusu = @Status3)
    WHERE IPO_ccPrawa.UserId = @IdPracownika AND IPO_ccPrawa.RolaId = 3 ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status1" Type="Int32" />
        <asp:Parameter Name="Status2" Type="Int32" />
        <asp:Parameter Name="Status3" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="administratorDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient,
     IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr,
      IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia,
       Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
        (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
 FROM IPO_Zamowienia JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika
  JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia
   JOIN IPO_ccPrawa ON IPO_CCZamowienia.IdCC = IPO_ccPrawa.IdCC OR IPO_ccPrawa.IdCC = 0
    WHERE IPO_ccPrawa.UserId = @IdPracownika AND IPO_ccPrawa.RolaId = 5 
    AND (IPO_Zamowienia.IdStatusu = @Status1 OR IPO_Zamowienia.IdStatusu = @Status2) 
    ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status1" Type="Int32" />
        <asp:Parameter Name="Status2" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="administratorZODataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="
    declare @emp varchar(1)
    declare @sep varchar(10)
    declare @nodata varchar(50)
    set @emp = ''
    set @sep = ', '
    set @nodata = ''
SELECT DISTINCT IPO_Zamowienia.Id AS Id, IPO_Zamowienia.Klient AS Klient, 
    IPO_Zamowienia.IdMagazynu AS Magazyn, IPO_Zamowienia.CER AS CER, IPO_Zamowienia.CERNr AS CERNr, 
    IPO_Zamowienia.Numer AS Numer, IPO_Zamowienia.Wartosc AS Wartosc, IPO_Zamowienia.DataUtworzenia, 
    Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
     (
        select  distinct
        ISNULL(STUFF((
        select @sep + CC.cc + '(' + convert(varchar, W.Udzial) + ')'
        from IPO_CCZamowienia W
        left join CC on CC.Id = W.IdCC
        where W.IdZamowienia = S.IdZamowienia
        order by W.Udzial desc, CC.cc
        for XML PATH('')
        ), 1, 1, @emp), @nodata) 
        from IPO_CCZamowienia S
        where S.IdZamowienia = IPO_Zamowienia.Id
        ) as CCList
    FROM IPO_Zamowienia 
    JOIN Pracownicy ON Pracownicy.Id = IPO_Zamowienia.IdPracownika 
    JOIN IPO_CCZamowienia ON IPO_Zamowienia.Id = IPO_CCZamowienia.IdZamowienia 
    JOIN IPO_ccPrawa ON IPO_CCZamowienia.IdCC = IPO_ccPrawa.IdCC OR IPO_ccPrawa.IdCC = 0 
    JOIN IPO_PozycjeZamowien ON IPO_PozycjeZamowien.IdZamowienia = IPO_Zamowienia.Id AND (IPO_PozycjeZamowien.IdStatusu = @Status1 OR IPO_PozycjeZamowien.IdStatusu = @Status2)
    WHERE IPO_ccPrawa.UserId = @IdPracownika AND IPO_ccPrawa.RolaId = 5 ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status1" Type="Int32" />
        <asp:Parameter Name="Status2" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="szczegolyZamowieniaDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="WITH ccz AS (
	SELECT ROW_NUMBER () OVER (ORDER BY IPO_CCZamowienia.Udzial) AS Lp,
		IPO_CCZamowienia.IdCC,
		CC.cc AS CC,
		IPO_CCZamowienia.Udzial AS Udzial
	FROM IPO_CCZamowienia 
	JOIN CC ON CC.Id = IPO_CCZamowienia.IdCC
	WHERE IPO_CCZamowienia.IdZamowienia = @Id)
SELECT IPO_Zamowienia.Id AS Id, Numer, Klient, IPO_Magazyny.Id AS Magazyn, IPO_Magazyny.Nazwa AS NazwaMagazynu, CER, CERNr, IPO_Zamowienia.IdStatusu AS Status, IPO_Statusy.Nazwa AS NazwaStatusu, Wartosc, PoziomAkceptacji, IPO_Zamowienia.HashLink,
(SELECT IdCC FROM ccz WHERE Lp = 1) AS IdCC1,
(SELECT CC FROM ccz WHERE Lp = 1) AS CC1,
(SELECT Udzial FROM ccz WHERE Lp = 1) AS Udzial1,
(SELECT IdCC FROM ccz WHERE Lp = 2) AS IdCC2,
(SELECT CC FROM ccz WHERE Lp = 2) AS CC2,
(SELECT Udzial FROM ccz WHERE Lp = 2) AS Udzial2,
(SELECT IdCC FROM ccz WHERE Lp = 3) AS IdCC3,
(SELECT CC FROM ccz WHERE Lp = 3) AS CC3,
(SELECT Udzial FROM ccz WHERE Lp = 3) AS Udzial3
FROM IPO_Zamowienia 
JOIN IPO_Statusy ON IPO_Statusy.Id = IPO_Zamowienia.IdStatusu 
JOIN IPO_Magazyny ON IPO_Zamowienia.IdMagazynu = IPO_Magazyny.Id 
WHERE IPO_Zamowienia.Id = @Id" UpdateCommand="UPDATE IPO_Zamowienia SET Numer = @Numer, Klient = @Klient, IdMagazynu = @Magazyn, CER = @CER, CERNr = @CERNr WHERE Id = @Id

 "
    DeleteCommand="DELETE FROM IPO_Zamowienia WHERE Id = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="Id" PropertyName="SelectedValue"
            Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Numer" Type="String" />
        <asp:Parameter Name="Klient" Type="String" />
        <asp:Parameter Name="Magazyn" Type="Int32" />
        <asp:Parameter Name="CER" Type="Boolean" />
        <asp:Parameter Name="CERNr" Type="String" />
        <asp:Parameter Name="CC1" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="CC2" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="CC3" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="Udzial1" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="Udzial2" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="Udzial3" Type="Int32" DefaultValue="0" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="pozycjeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT IPO_PozycjeZamowien.Id AS Id, IPO_PozycjeZamowien.IdZamowienia AS IdZamowienia,
                IPO_PozycjeZamowien.Nazwa AS Nazwa, IPO_PozycjeZamowien.Opis, Ilosc, Cena, ProduktStockowy,
                RoHS, IdRodzajuProduktu AS RodzajProduktu, IdDostawcy AS Dostawca,
                IPO_RodzajeProduktow.Nazwa AS NazwaRodzajuProduktu, IPO_Dostawcy.Nazwa AS NazwaDostawcy,
                KontoKsiegowe, Waluta, PowodZakupu, Notatka, IdStatusu, IPO_Statusy.Nazwa AS Status,
                DataDostawy, LokalizacjaOdbioru, Jednostka, IPO_PozycjeZamowien.IdProduktu, IPO_PozycjeZamowien.PartNo,
                IPO_PozycjeZamowien.ProductClass, IPO_PozycjeZamowien.ProductSubclass,
	                CASE 
		                WHEN IPO_PozycjeZamowien.IdStatusu = 2 THEN 1
		                WHEN IPO_PozycjeZamowien.IdStatusu != 2 THEN 0
		                ELSE 0
	                END AS Odrzuc,
	                CASE 
		                WHEN IPO_PozycjeZamowien.IdStatusu = 7 AND IPO_PozycjeZamowien.IdOdrzucajacego = @UserId THEN 1
		                WHEN IPO_PozycjeZamowien.IdStatusu != 7 OR IPO_PozycjeZamowien.IdOdrzucajacego != @UserId THEN 0
		                ELSE 0
	                END AS Przywroc,
	            (Ilosc * Cena) as Wartosc,
	            IPO_PozycjeZamowien.DataMaila
                FROM IPO_PozycjeZamowien 
                JOIN IPO_Dostawcy ON IPO_PozycjeZamowien.IdDostawcy = IPO_Dostawcy.Id 
                JOIN IPO_RodzajeProduktow ON IPO_PozycjeZamowien.IdRodzajuProduktu = IPO_RodzajeProduktow.Id
                JOIN IPO_Statusy ON IPO_PozycjeZamowien.IdStatusu = IPO_Statusy.Id 
                WHERE IdZamowienia = @IdZamowienia 
                ORDER BY Id" InsertCommand="INSERT INTO IPO_PozycjeZamowien(IdZamowienia, IdStatusu, Nazwa, Opis, IdRodzajuProduktu, ProduktStockowy, RoHS, Ilosc, Jednostka, Cena, Waluta, KontoKsiegowe, IdDostawcy, PowodZakupu, Notatka, IdProduktu, PartNo, ProductClass, ProductSubclass)
                   VALUES(@IdZamowienia, 1, @Nazwa, @Opis, @RodzajProduktu, @ProduktStockowy, @RoHS, @Ilosc, @Jednostka, @Cena, @Waluta, @KontoKsiegowe, @Dostawca, @PowodZakupu, @Notatka, @IdProduktu, @PartNo, @ProductClass, @ProductSubclass)"
    UpdateCommand="UPDATE IPO_PozycjeZamowien SET Nazwa = @Nazwa, Opis = @Opis, Ilosc = @Ilosc, Jednostka = @Jednostka, Cena = @Cena, Waluta = @Waluta, ProduktStockowy = @ProduktStockowy, RoHS = @RoHS, IdRodzajuProduktu = @RodzajProduktu, KontoKsiegowe = @KontoKsiegowe, IdDostawcy = @Dostawca, PowodZakupu = @PowodZakupu, Notatka = @Notatka, IdStatusu = @IdStatusu, PartNo = @PartNo, ProductClass = @ProductClass, ProductSubclass = @ProductSubclass WHERE Id = @Id"
    DeleteCommand="DELETE FROM IPO_PozycjeZamowien WHERE Id = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="IdZamowienia" PropertyName="SelectedValue"
            Type="Int32" />
        <asp:Parameter Name="UserId" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="IdZamowienia" PropertyName="SelectedValue"
            Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="RodzajProduktu" Type="Int32" />
        <asp:Parameter Name="ProduktStockowy" Type="Boolean" />
        <asp:Parameter Name="RoHS" Type="Boolean" />
        <asp:Parameter Name="Ilosc" Type="Int32" />
        <asp:Parameter Name="Jednostka" Type="String" />
        <asp:Parameter Name="Cena" Type="Decimal" />
        <asp:Parameter Name="Waluta" Type="String" />
        <asp:Parameter Name="KontoKsiegowe" Type="String" />
        <asp:Parameter Name="Dostawca" Type="Int32" />
        <asp:Parameter Name="PowodZakupu" Type="String" />
        <asp:Parameter Name="Notatka" Type="String" />
        <asp:Parameter Name="IdProduktu" Type="Int32" />
        <asp:Parameter Name="PartNo" Type="String" />
        <asp:Parameter Name="ProductClass" Type="String" />
        <asp:Parameter Name="ProductSubclass" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="RodzajProduktu" Type="Int32" />
        <asp:Parameter Name="ProduktStockowy" Type="Boolean" />
        <asp:Parameter Name="RoHS" Type="Boolean" />
        <asp:Parameter Name="Ilosc" Type="Int32" />
        <asp:Parameter Name="Jednostka" Type="String" />
        <asp:Parameter Name="Cena" Type="Decimal" />
        <asp:Parameter Name="Waluta" Type="String" />
        <asp:Parameter Name="KontoKsiegowe" Type="String" />
        <asp:Parameter Name="Dostawca" Type="Int32" />
        <asp:Parameter Name="PowodZakupu" Type="String" />
        <asp:Parameter Name="Notatka" Type="String" />
        <asp:Parameter Name="IdStatusu" Type="Int32" />
        <asp:Parameter Name="PartNo" Type="String" />
        <asp:Parameter Name="ProductClass" Type="String" />
        <asp:Parameter Name="ProductSubclass" Type="String" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SciezkaAkceptacjiDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT IPO_SciezkaAkceptacji.Id, IPO_SciezkaAkceptacji.PoziomAkceptacji AS Poziom, IPO_SciezkaAkceptacji.RodzajAkceptacji AS PoziomAkceptacji, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik, IPO_SciezkaAkceptacji.Status,
                    IPO_StatusyAkceptacji.Nazwa AS NazwaStatusu,
                    CASE 
		                WHEN IPO_SciezkaAkceptacji.Status IN(1,2) THEN 'green'
		                WHEN IPO_SciezkaAkceptacji.Status IN(3,4) THEN 'red'
		                WHEN IPO_SciezkaAkceptacji.DataMaila IS NULL THEN 'gray'
		                WHEN IPO_SciezkaAkceptacji.DataMaila IS NOT NULL AND IPO_SciezkaAkceptacji.DataMaila &lt; GETDATE()-0.5 AND IPO_SciezkaAkceptacji.Status IN(0) THEN 'yellow_background' 
		                ELSE 'black'
	                END AS CSS
                    FROM IPO_SciezkaAkceptacji
                    JOIN IPO_Zamowienia ON IPO_Zamowienia.Id = IPO_SciezkaAkceptacji.IdZamowienia
                    JOIN Pracownicy ON IPO_SciezkaAkceptacji.UserId = Pracownicy.Id
                    JOIN IPO_StatusyAkceptacji ON IPO_SciezkaAkceptacji.Status = IPO_StatusyAkceptacji.Id
                    WHERE IdZamowienia = @IdZamowienia 
                    AND IPO_SciezkaAkceptacji.DataSciezki = IPO_Zamowienia.DataAkceptacji
                    AND IPO_Zamowienia.DataAkceptacji = (SELECT TOP 1 DataAkceptacji FROM IPO_SciezkaAkceptacji WHERE IdZamowienia = @IdZamowienia ORDER BY DataSciezki DESC)
                    ORDER BY  Poziom, IPO_SciezkaAkceptacji.Id DESC" InsertCommand="
                     DECLARE @DataSciezkiIns DateTime
                     SELECT @DataSciezkiIns = DataAkceptacji from IPO_Zamowienia where Id = @IdZamowienia
                     
                     INSERT INTO IPO_SciezkaAkceptacji(IdZamowienia, UserId, PoziomAkceptacji, Status, RodzajAkceptacji, DataSciezki)
                   VALUES(@IdZamowienia, @UserId, 1, 0, 'Dodane przez Administratora', @DataSciezkiIns)"
    UpdateCommand="UPDATE IPO_SciezkaAkceptacji SET Status = @Status WHERE Id = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="IdZamowienia" PropertyName="SelectedValue"
            Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="IdZamowienia" PropertyName="SelectedValue"
            Type="Int32" />
        <asp:Parameter Name="UserId" Type="Int32" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="staraSciezkaAkceptacjiDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT IPO_SciezkaAkceptacji.Id, IPO_SciezkaAkceptacji.PoziomAkceptacji AS Poziom, IPO_SciezkaAkceptacji.RodzajAkceptacji AS PoziomAkceptacji, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik, IPO_SciezkaAkceptacji.Status,
                    IPO_StatusyAkceptacji.Nazwa AS NazwaStatusu,
                    CASE 
		                WHEN IPO_SciezkaAkceptacji.Status IN(1,2) THEN 'green'
		                WHEN IPO_SciezkaAkceptacji.Status IN(3,4) THEN 'red'
		                WHEN IPO_SciezkaAkceptacji.DataMaila IS NULL THEN 'gray'
		                WHEN IPO_SciezkaAkceptacji.DataMaila IS NOT NULL AND IPO_SciezkaAkceptacji.DataMaila &lt; GETDATE()-0.5 THEN 'yellow_background' 
		                ELSE 'black'
	                END AS CSS
                    FROM IPO_SciezkaAkceptacji
                    JOIN IPO_Zamowienia ON IPO_Zamowienia.Id = IPO_SciezkaAkceptacji.IdZamowienia
                    JOIN Pracownicy ON IPO_SciezkaAkceptacji.UserId = Pracownicy.Id
                    JOIN IPO_StatusyAkceptacji ON IPO_SciezkaAkceptacji.Status = IPO_StatusyAkceptacji.Id
                    WHERE IdZamowienia = @IdZamowienia 
                    AND IPO_SciezkaAkceptacji.DataSciezki = IPO_Zamowienia.DataAkceptacji
                    AND IPO_Zamowienia.DataAkceptacji &lt; (SELECT TOP 1 DataSciezki FROM IPO_SciezkaAkceptacji WHERE IdZamowienia = @IdZamowienia ORDER BY DataSciezki DESC)
                    ORDER BY  Poziom, IPO_SciezkaAkceptacji.Id DESC">
    <SelectParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="IdZamowienia" PropertyName="SelectedValue"
            Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="nowaSciezkaAkceptacjiDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT IPO_SciezkaAkceptacji.Id, IPO_SciezkaAkceptacji.PoziomAkceptacji AS Poziom, IPO_SciezkaAkceptacji.RodzajAkceptacji AS PoziomAkceptacji, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik, IPO_SciezkaAkceptacji.Status,
                    IPO_StatusyAkceptacji.Nazwa AS NazwaStatusu,
                    CASE 
		                WHEN IPO_SciezkaAkceptacji.Status IN(1,2) THEN 'green'
		                WHEN IPO_SciezkaAkceptacji.Status IN(3,4) THEN 'red'
		                WHEN IPO_SciezkaAkceptacji.DataMaila IS NULL THEN 'gray'
		                WHEN IPO_SciezkaAkceptacji.DataMaila IS NOT NULL AND IPO_SciezkaAkceptacji.DataMaila &lt; GETDATE()-0.5 THEN 'yellow_background' 
		                ELSE 'black'
	                END AS CSS
                    FROM IPO_SciezkaAkceptacji
                    JOIN IPO_Zamowienia ON IPO_Zamowienia.Id = IPO_SciezkaAkceptacji.IdZamowienia
                    JOIN Pracownicy ON IPO_SciezkaAkceptacji.UserId = Pracownicy.Id
                    JOIN IPO_StatusyAkceptacji ON IPO_SciezkaAkceptacji.Status = IPO_StatusyAkceptacji.Id
                    WHERE IdZamowienia = @IdZamowienia 
                    AND IPO_SciezkaAkceptacji.DataSciezki = (SELECT TOP 1 DataSciezki FROM IPO_SciezkaAkceptacji WHERE IdZamowienia = @IdZamowienia ORDER BY DataSciezki DESC)
                    AND IPO_Zamowienia.DataAkceptacji &lt; (SELECT TOP 1 DataSciezki FROM IPO_SciezkaAkceptacji WHERE IdZamowienia = @IdZamowienia ORDER BY DataSciezki DESC)
                    ORDER BY  Poziom, IPO_SciezkaAkceptacji.Id DESC">
    <SelectParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="IdZamowienia" PropertyName="SelectedValue"
            Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSourceDostawcaFiltr" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    SelectCommand="SELECT IPO_Dostawcy.Id as Id, IPO_Dostawcy.Nazwa as Dostawca FROM IPO_Dostawcy 
                    JOIN IPO_PozycjeZamowien
                        on IPO_PozycjeZamowien.IdDostawcy=IPO_Dostawcy.Id
                    WHERE IPO_PozycjeZamowien.IdZamowienia = @IdZamowienia
                    ORDER BY Dostawca">
    <SelectParameters>
        <asp:ControlParameter ControlID="zamowieniaListView" Name="IdZamowienia" PropertyName="SelectedValue"
            Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
