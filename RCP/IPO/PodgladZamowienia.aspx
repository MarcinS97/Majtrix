<%@ Page Title="" Language="C#" MasterPageFile="~/IPO/IPO.Master" AutoEventWireup="true"
    CodeBehind="PodgladZamowienia.aspx.cs" Inherits="HRRcp.IPO.PodgladZamowienia"
    ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <style type="text/css">
                .printoff
                {
                    display: none !important;
                }          
            </style>

            <script type="text/javascript">
                function Drukuj() {
                    var title = "Zamowienie";
                    
                    var NumerZamowieniaID = "<%= PONo.ClientID %>";
                    var NumerZamowienia = document.getElementById(NumerZamowieniaID);
                    title += " " + NumerZamowienia.innerHTML;

                    try {
                        var VendorNameID = "<%= VendorName.ClientID %>";
                        var VendorName = document.getElementById(VendorNameID);
                        title += " " + VendorName.innerHTML;
                    } catch (Error) {
                    }
                    
                    document.title = title;
                    window.print();
                }
                function Wyslij() {
                    if (confirm("Czy napewno chcesz wysłać maila do dostawcy?")) {
                        var PodgladWydrukuID = "<%= PodgladWydruku.ClientID %>";
                        try {
                            var PodgladWydruku = document.getElementById(PodgladWydrukuID);
                            PodgladWydruku.value = Date.now() + "|" + document.getElementById("podglad_wydruku").innerHTML;
                        } catch (Error) {
                        }
                    }
                }   
            </script>

            <asp:HiddenField ID="PodgladWydruku" OnValueChanged="PodgladWydruku_OnValueChanged"
                runat="server" />
            <table class="caption" style="width: 1200px; margin: 5px auto;">
                <tr>
                    <td align="left">
                        <span style="text-align: left; font-weight: bold;">Dostawca:</span>
                        <asp:DropDownList ID="dostawcaDropDownList" runat="server" DataSourceID="dostawcaDataSource"
                            DataTextField="Nazwa" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="dostawca_OnSelectedIndexChanged"
                            OnDataBound="dostawca_OnDataBound">
                        </asp:DropDownList>
                        <asp:CheckBox ID="WszyscyCheckBox" runat="server" OnCheckedChanged="WszyscyCheckBox_OnCheckedChanged" AutoPostBack="true" />
                        <span style="text-align: left; font-weight: bold;">Wszyscy dostawcy</span>
                        <asp:CheckBox ID="SciezkaAkceptaccjiCheckBox" runat="server" OnCheckedChanged="SciezkaAkceptaccjiCheckBox_OnCheckedChanged" AutoPostBack="true" />
                        <span style="text-align: left; font-weight: bold;">Ścieżka akceptacji</span>
                    </td>
                    <td align="right">
                        <a onclick="javascript:Drukuj()" class="button">Drukuj</a>
                        <asp:LinkButton ID="WyslijLink" runat="server" Text="Wyślij" OnClientClick="Wyslij()" CssClass="button"/>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="DownloadCSV" runat="server" Text="Eksport do CSV" OnClick="CSV_OnClick" CssClass="button"/>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="DownloadCSV" />
                            </Triggers>
                        </asp:UpdatePanel>                        
                    </td>
                </tr>
            </table>
            <div id="podglad_wydruku" style="width: 1200px; margin: auto;">
            <style type="text/css">
                .pozycje
                {
                    width: 100%;
                    margin: auto;
                    border: 1px solid black;
                }
                .pozycje th
                {
                    padding: 10px 5px;
                }
                .pozycje td
                {
                    padding: 10px 5px;
                }
                .podsumowanie 
                {
                    width: 100%; 
                    margin: 5px auto; 
                    border: 1px solid black;
                    border-collapse: collapse; 
                    page-break-inside: avoid;
                }   
                .podsumowanie td
                {
                    padding: 10px 5px; 
                    border: 1px solid black;
                    vertical-align: bottom;
                }
                .margin-bottom-5px 
                {
                    margin-bottom: 5px;
                }         
            </style>
                <table class="naglowek" style="width: 100%; margin: 5px auto;">
                    <tr>
                        <td colspan="3" style="text-align: center; font-size: 16px; font-weight: bold;">
                            PURCHASE ORDER
                        </td>
                    </tr>
                    <tr>
                        <td width="30%">
                            <table style="width: 100%;">
                                <tr><td style="text-align: center;"><img src="../../images/iQor/iqor1a.png"/></td></tr>
                            </table>
                            <table ID="vendorTable" runat="server" style="width: 100%;">
                                <tr>
                                    <td colspan="2" style="text-align: center; padding: 10px 5px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Vendor ID:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Vendor Name:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorName" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Vendor Address:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorAddress1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 5px;">
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorAddress2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 5px;">
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorAddress3" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 5px;">
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorAddress4" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Primary contact:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorPrimaryContact" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px; text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Email:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="VendorEmail" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Bill/Ship To:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="BillShipTo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Buyer:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="Buyer" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Phone:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="Phone" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        E-mail:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="Email" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="ShippingInvoice" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Forwarder details:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="ForwarderDetails" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Notes:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="Notes" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="20%">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        PO NO.:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="PONo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        PR No.:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="PRNo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        PO Status:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="POStatus" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Date:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="Date" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-weight: bold; padding: 10px 5px;">
                                        Revision Date:
                                    </td>
                                    <td style="padding: 10px 5px;">
                                        <asp:Label ID="RevisionDate" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="pozycje" runat="server" DataSourceID="pozycjeDataSource" AutoGenerateColumns="false"
                    CssClass="pozycje">
                    <Columns>
                        <asp:BoundField DataField="Lp" HeaderText="Line" ItemStyle-Width="4%"/>
                        <asp:BoundField DataField="PartNo" HeaderText="Part No" ItemStyle-Width="8%"/>
                        <asp:BoundField DataField="Nazwa" HeaderText="Part Description /Manufacturer Part No" ItemStyle-Width="30%"/>
                        <asp:BoundField DataField="Dostawca" HeaderText="Vendor" ItemStyle-Width="10%"/>
                        <asp:BoundField DataField="Status" HeaderText="Line Status" ItemStyle-Width="8%"/>
                        <asp:BoundField DataField="DataDostawy" HeaderText="Delivery Date" ItemStyle-Width="8%"/>
                        <asp:BoundField DataField="Ilosc" HeaderText="Quantity" ItemStyle-Width="8%" />
                        <asp:BoundField DataField="Waluta" HeaderText="Currency" ItemStyle-Width="8%" />
                        <asp:BoundField DataField="Cena" HeaderText="Unit Price" ItemStyle-Width="8%" />
                        <asp:BoundField DataField="Wartosc" HeaderText="Amount" ItemStyle-Width="8%"/>
                    </Columns>
                </asp:GridView>
                <table class="podsumowanie">
                    <tr>
                        <td>
                            PAYMENT TERMS
                        </td>
                        <td>
                            PO Total Amount:
                        </td>
                        <td width="8%">
                            <asp:Label ID="Wartosc" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            NET 60
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td width="60%">
                            This is computer generated Purchase Order, no signature is required. The Purchase
                            Order details need to be confirmed in a written format within 48 hours! Our company
                            requires its suppliers to work according to the Electronic Industry Code of Conduct
                            (EICC) standards! To the extent required by Executive Order No. 11,246 and its implementing
                            regulations, this purchase order incorporates by reference the Equal Opportunity
                            Clause, 41 CFR 60-1.4(a).
                        </td>
                        <td colspan="2" width="40%">
                            Vendor Acknowledgement/Confirmation
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="sciezkaAkceptacji" runat="server" DataSourceID="sciezkaAkceptacjiDataSource" AutoGenerateColumns="false"
                    CssClass="pozycje margin-bottom-5px" Visible="false">
                    <Columns>
                        <asp:BoundField DataField="PoziomAkceptacji" HeaderText="Poziom Akceptacji" ItemStyle-Width="30%"/>
                        <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" ItemStyle-Width="40%"/>
                        <asp:BoundField DataField="NazwaStatusu" HeaderText="Status" ItemStyle-Width="30%"/>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img alt="Indicator" src="../../images/activity.gif" />
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="updProgressBlocker" runat="server" TargetControlID="updProgress1"
        BackgroundCssClass="updProgress1back" PopupControlID="updProgress1">
    </asp:ModalPopupExtender>
    <asp:SqlDataSource ID="dostawcaDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
        SelectCommand="SELECT DISTINCT IPO_Dostawcy.Id, IPO_Dostawcy.Nazwa
                        FROM IPO_PozycjeZamowien
                        JOIN IPO_Zamowienia ON IPO_PozycjeZamowien.IdZamowienia = IPO_Zamowienia.Id
                        JOIN IPO_Dostawcy ON IPO_PozycjeZamowien.IdDostawcy = IPO_Dostawcy.Id
                        WHERE IPO_PozycjeZamowien.IdZamowienia = @IdZamowienia AND IPO_PozycjeZamowien.IdStatusu IN (3,4,5,6)
                        ORDER BY IPO_Dostawcy.Nazwa">
        <SelectParameters>
            <asp:Parameter Name="IdZamowienia" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="pozycjeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
        SelectCommand="SELECT  ROW_NUMBER() OVER(ORDER BY IPO_Dostawcy.Nazwa) AS Lp,
                               IPO_PozycjeZamowien.PartNo,
                               IPO_PozycjeZamowien.Nazwa,
                               IPO_Statusy.Nazwa AS Status,
                               IPO_PozycjeZamowien.Ilosc,
                               IPO_PozycjeZamowien.DataDostawy,
                               IPO_PozycjeZamowien.Waluta,
                               IPO_PozycjeZamowien.Cena,
                               IPO_Dostawcy.Nazwa AS Dostawca,
                               IPO_PozycjeZamowien.Ilosc*IPO_PozycjeZamowien.Cena AS Wartosc
                        FROM IPO_PozycjeZamowien
                        JOIN IPO_Zamowienia ON IPO_PozycjeZamowien.IdZamowienia = IPO_Zamowienia.Id
                        JOIN IPO_Dostawcy ON IPO_PozycjeZamowien.IdDostawcy = IPO_Dostawcy.Id
                        JOIN IPO_Statusy ON IPO_PozycjeZamowien.IdStatusu = IPO_Statusy.Id
                        WHERE IPO_PozycjeZamowien.IdZamowienia = @IdZamowienia 
                        AND IPO_PozycjeZamowien.IdStatusu IN (3,4,5,6)
                        AND (IPO_PozycjeZamowien.IdDostawcy = @IdDostawcy OR @WszyscyDostawcy = 1)
                        ORDER BY IPO_Dostawcy.Nazwa">
        <SelectParameters>
            <asp:Parameter Name="IdZamowienia" Type="Int32" />
            <asp:ControlParameter ControlID="dostawcaDropDownList" Name="IdDostawcy" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="WszyscyCheckBox" Name="WszyscyDostawcy" PropertyName="Checked"
                Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sciezkaAkceptacjiDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
        SelectCommand="SELECT IPO_SciezkaAkceptacji.Id,
                              IPO_SciezkaAkceptacji.PoziomAkceptacji AS Poziom,
                              IPO_SciezkaAkceptacji.RodzajAkceptacji AS PoziomAkceptacji,
                              Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS Pracownik,
                              IPO_SciezkaAkceptacji.Status,
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
                        AND IPO_Zamowienia.DataAkceptacji = (SELECT TOP 1 DataAkceptacji FROM IPO_SciezkaAkceptacji WHERE IdZamowienia = @IdZamowienia ORDER BY DataSciezki DESC)
                        ORDER BY  Poziom, IPO_SciezkaAkceptacji.Id DESC">
        <SelectParameters>
            <asp:Parameter Name="IdZamowienia" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
