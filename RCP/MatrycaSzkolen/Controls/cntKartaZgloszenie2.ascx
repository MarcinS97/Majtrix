<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKartaZgloszenie2.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntKartaZgloszenie2" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<div class="cntKartaZgloszenie" style="margin-bottom: 32px;">

    <asp:HiddenField ID="hidIdZgloszenia" runat="server" Visible="false" />

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
                <%--<asp:TextBox ID="tbZglaszajacy" runat="server" CssClass="form-control" MaxLength="100" />--%>
                
                        <asp:DropDownList ID="ddlZglaszajacy" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsEmployee" CssClass="form-control" />
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
                <asp:TextBox ID="tbKosztSzkolenia" runat="server" CssClass="form-control" MaxLength="5" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbKosztSzkolenia" FilterType="Custom" ValidChars="0123456789.," />
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
        <asp:Repeater ID="rpParti" runat="server" OnDataBinding="rpParti_DataBinding" OnItemDataBound="rpParti_ItemDataBound">
        <HeaderTemplate>
            <tr>
                <th>
                    Lp.
                </th>
                <th>
                    Pracownik
                </th>
                <th>
                    CK
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
                        <%--<asp:TextBox id="tbImieNazwisko" runat="server" CssClass="form-control" />--%>

                        <asp:DropDownList ID="ddlEmployee" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsEmployee" CssClass="form-control" />

                    </td>
                    <td style="width: 120px;">
                        <%--<cc:DateEdit ID="deDataUrodzenia" runat="server" />--%>

                        <asp:DropDownList ID="ddlCC" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsCC" CssClass="form-control" />
                    </td>
                  <%--  <td>
                    
                    </td>--%>
                </tr>
            </ItemTemplate>
        </asp:Repeater>

        <asp:SqlDataSource ID="dsEmployee" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
            SelectCommand="
            select null Value, 'pracownik ...' Text, 0 Sort
            union all
            select Id Value, Nazwisko + ' ' + Imie Text, 1 Sort 
            from Pracownicy 
            order by Sort, Text"
            />

        <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
            SelectCommand="
            select null Value, 'ck ...' Text, 0 Sort
            union all
            select Id Value, Nazwa Text, 1 Sort 
            from CC 
            order by Sort, Text"
            />

    </table>
    <sup>1</sup> oddanie wypełnionej i podpisanej karty jest warunkiem rozpoczęcia szkolenia<br />
    <sup>2</sup> szkolenie zewnętrzne organizowane są przez podmiot z zewnątrz, szkolenia wewnętrzne organizowane są przez pracowników firmy<br />
    <sup>3</sup> miejscowość/sala/hotel<br />
    <sup>4</sup> łączny koszt udziału wszystkich uczestników<br />
    <literal ID="ll5" runat="server"><sup>5</sup> koszty noclegu, dojazdu, delegacji<br /></literal>
    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success pull-right" Text="Zapisz" OnClick="btnSave_Click" ValidationGroup="vPrint"/>
    <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary pull-right" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint"  style="margin-right: 8px !important; margin-bottom: 16px !important;"   />


    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnPrint" />
    </Triggers>
</asp:UpdatePanel>
        
</div>

<asp:SqlDataSource ID="dsHeaderSave" runat="server" SelectCommand=
"
insert into msZgloszenia (IdPracownika, DataZgloszenia, RodzajSzkolenia, Organizator, Temat, DataSzkolenia, Miejsce, Koszt) select {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}

select SCOPE_IDENTITY()    
" />

<asp:SqlDataSource ID="dsSave" runat="server" SelectCommand=
"
insert into msZgloszeniaPracownicy (IdZgloszenia, IdPracownika, IdCC) select {0}, {1}, {2}" 
/>

<asp:SqlDataSource ID="dsHeader" runat="server" SelectCommand=
"select * from msZgloszenia where Id = {0}"
/>

<asp:SqlDataSource ID="dsEmployees" runat="server" SelectCommand=
"select * from msZgloszeniaPracownicy where IdZgloszenia = {0}"
/>
