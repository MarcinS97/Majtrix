<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKartaZgloszenie.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntKartaZgloszenie" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>

<div class="cntKartaZgloszenie">
<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>


    <h3 class="text-primary">Karta zgłoszenia na szkolenie</h3>
    <hr />
    <%--<h5 class="">Proszę wypełnić formularz szkoleniowy i oddać do Działu kadr<sup>1</sup></h5>--%>
    <table class="tbHeader table table-bordered">
        <tr>
            <td class="col1">
                <label>Zgłaszający /imię i nazwisko, stanowisko/</label>
            </td>
            <td>
                <asp:TextBox ID="tbZglaszajacy" runat="server" CssClass="form-control" MaxLength="100" />
            </td>
        </tr>
        <tr>
            <td class="col1">
                <label>Data zgłoszenia</label>
            </td>
            <td>
                <cc:DateEdit ID="deDataZgloszenia" runat="server" ValidationGroup="vPrint" />
            </td>
        </tr>
        <tr>
            <td class="col1">
                <label>Rodzaj szkolenia (zewnętrzne lub wewnętrzne)<sup>2</sup></label>
            </td>
            <td>
                <asp:DropDownList ID="ddlRodzajSzkolenia" runat="server" CssClass="form-control" >
                    <asp:ListItem Value="-1" Text="wybierz ..." />
                    <asp:ListItem Value="1" Text="Zewnętrzne" />
                    <asp:ListItem Value="2" Text="Wewnętrzne" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="col1">
                <label>Organizator (wewnętrzne –jak dział organizuje, zewnętrzne- jaka firma)</label>
            </td>
            <td>
                <asp:DropDownList ID="ddlOrganizator" runat="server" CssClass="form-control" >
                    <asp:ListItem Value="-1" Text="wybierz ..." />
                    <asp:ListItem Value="1" Text="Zewnętrzne" />
                    <asp:ListItem Value="2" Text="Wewnętrzne" />
                </asp:DropDownList>
                <asp:SqlDataSource ID="dsFirmy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="select '-1' Value, 'wybierz ...' Text union all select '1', 'Firma 1' union all select '2', 'Firma 2' union all select '3', 'Firma 3'" />
            </td>
        </tr>
        <tr>
            <td class="col1">
                <label>Temat szkolenia</label>
            </td>
            <td>
                <asp:TextBox ID="tbTematSzkolenia" runat="server" TextMode="MultiLine" CssClass="form-control" />
            </td>
        </tr>
        <tr>
            <td class="col1">
                <label ID="lbDataSzkolenia" runat="server">Data szkolenia</label>
            </td>
            <td>
                <cc:DateEdit ID="deDataSzkolenia" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="col1">
                <label>Miejsce szkolenia<sup>3</sup></label>
            </td>
            <td>
                <asp:TextBox ID="tbMiejsceSzkolenia" runat="server" CssClass="form-control" />
            </td>
        </tr>
        <tr>
            <td class="col1">
                <label>Całkowity koszt szkolenia<sup>4</sup></label>
            </td>
            <td>
                <asp:TextBox ID="tbKosztSzkolenia" runat="server" CssClass="form-control" />
            </td>
        </tr>
        <tr ID="trKosztyDodatkowe" runat="server">
            <td class="col1">
                <label>Koszty dodatkowe<sup>5</sup></label>
            </td>
            <td>
                <asp:TextBox ID="tbKosztyDodatkowe" runat="server" CssClass="form-control" />
            </td>
        </tr>
    </table>
    
    <h5 class="">Uczestnicy szkolenia:</h5>
    
    <table class="tbPracownicy table table-bordered">
        <asp:Repeater ID="rpParti" runat="server">
        <HeaderTemplate>
            <tr>
                <th>
                    Lp.
                </th>
                <th>
                    Pracownik
                </th>
                <th>
                    Data urodzenia
                </th>
               <%-- <th>
                    Podpis uczestnika
                </th>--%>
            </tr>
        </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width: 30px;">
                           <%# Container.DataItem %>  
                    </td>
                    <td style="width: 80%;">
                        <asp:TextBox id="tbImieNazwisko" runat="server" CssClass="form-control" />
                    </td>
                    <td style="width: 120px;">
                        <cc:DateEdit ID="deDataUrodzenia" runat="server" />
                    </td>
                  <%--  <td>
                    
                    </td>--%>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <sup>1</sup> oddanie wypełnionej i podpisanej karty jest warunkiem rozpoczęcia szkolenia<br />
    <sup>2</sup> szkolenie zewnętrzne organizowane są przez podmiot z zewnątrz, szkolenia wewnętrzne organizowane są przez pracowników firmy<br />
    <sup>3</sup> miejscowość/sala/hotel<br />
    <sup>4</sup> łączny koszt udziału wszystkich uczestników<br />
    <literal ID="ll5" runat="server"><sup>5</sup> koszty noclegu, dojazdu, delegacji<br /></literal>
    <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary pull-right" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint" />

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnPrint" />
    </Triggers>
</asp:UpdatePanel>
        
</div>
