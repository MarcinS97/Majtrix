<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPracInfo2.ascx.cs" Inherits="HRRcp.Controls.cntPracInfo2" %>

<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
<asp:HiddenField ID="hidUmowaOd" runat="server" Visible="false" />
<asp:HiddenField ID="hidUmowaDo" runat="server" Visible="false" />

<table class="table0 tbPlanUrlopowRok_pracinfo">
    <tr>
        <th class="col1a">
            Informacje
        </th>
        <th class="col2a">
            Weryfikacja
        </th>
    </tr>
    <tr>
        <td class="col1a">

            <table class="table0 cntRepPracInfo" >
                <tr id="trOkres" runat="server" visible="false">
                    <td class="col1"><span class="t1">Okres obowiązywania bieżącej umowy:</span></td>
                    <td class="col23" colspan="2">
                        <asp:Label ID="lbOkres" runat="server" Text="2013-01-01 ... 2013-12-31" ></asp:Label>
                    </td>
                </tr>
                <tr id="trDataZatrudnienia" runat="server" visible="true">
                    <td class="col1"><span class="t1">Data zatrudnienia:</span></td>
                    <td class="col23" colspan="2">
                        <asp:Label ID="lbDataZatrudnienia" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr id="trDataUmowy" runat="server" visible="true">
                    <td class="col1"><span class="t1">Data obowiązywania umowy:</span></td>
                    <td class="col23" colspan="2">
                        <asp:Label ID="lbDataUmowy" runat="server" ></asp:Label>
                    </td>
                </tr>

                <tr id="trDataZwol" runat="server" visible="false">
                    <td class="col1"><span class="t1">Data zwolnienia:</span></td>
                    <td class="col23" colspan="2">
                        <asp:Label ID="lbDataZwol" runat="server" ></asp:Label>
                    </td>
                </tr>

                <tr id="trDataZwiekszenia" runat="server" visible="false">
                    <td class="col1"><span class="t1">Data zwiększenia wymiaru:</span></td>
                    <td class="col2">
                        <asp:Label ID="lbDataZwiekszenia" runat="server" ></asp:Label>
                    </td>
                    <td class="col2s">
                    </td>
                </tr>
                <tr id="trWymiarUmowa" runat="server" visible="true">
                    <td class="col1"><span class="t1">Wymiar urlopu do końca umowy (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbWymiarUmowa" runat="server" ></asp:Label>
                    </td>
                    <td class="col2s">
                    </td>
                </tr>
                <tr id="trWymiarRok" runat="server" visible="true">
                    <td class="col1"><span class="t1">Wymiar urlopu do końca roku (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbWymiar" runat="server" ></asp:Label>
                    </td>
                    <td class="col2s">
                    </td>
                </tr>
                <tr id="trDodatkowy" runat="server" visible="false">
                    <td class="col1"><span class="t1">Urlop dodatkowy (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbDodatkowy" runat="server" ></asp:Label>
                    </td>
                    <td class="col2s">
                    </td>
                </tr>
                <tr>
                    <td class="col1"><span class="t1">Urlop zaległy (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbZalegly" runat="server" ></asp:Label>
                    </td>
                    <td class="col2s">
                    </td>
                </tr>                
                <tr>
                    <td class="col1"><span class="t1">Razem:</span></td>
                    <td class="col2">
                        <asp:Label ID="lbRazem" runat="server" ></asp:Label>
                    </td>
                    <td class="col2s">
                    </td>
                </tr>
                <tr>
                    <td class="col1"><span class="t1">Urlop wykorzystany:</span></td>
                    <td class="col2">
                        <asp:Label ID="lbWykorzystany" runat="server" ></asp:Label>
                    </td>
                    <td class="col2s">
                    </td>
                </tr>
                <%--
                <tr>
                    <td class="col1"><span class="t1">--------------- Weryfikacja ---------------</span></td>
                    <td class="col2"></td>
                </tr>
                --%>
            </table>
        </td>
        <td class="col2a">
            <table class="table0 cntRepPracInfo" >
                <%--
                <tr>
                    <td class="col1"><span class="t1">Urlop zaplanowany zaległy (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbVal6" runat="server" ></asp:Label>
                    </td>
                    <td class="col3">
                        <asp:Image ID="imgOk6" style="display: none;" runat="server" ImageUrl="~/images/ok.png" />
                        <asp:Image ID="imgErr6" style="display: none;" runat="server" ImageUrl="~/images/buttons/delete.png" />
                    </td>
                    <td class="col4">
                        <asp:Label ID="lbErr6" style="display: none;" runat="server" Text="Nie zaplanowano urlopu" ></asp:Label>
                        <asp:HiddenField ID="hidErr6" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="col1"><span class="t1">Urlop zaplanowany bieżący (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbVal7" runat="server" ></asp:Label>
                    </td>
                    <td class="col3">
                        <asp:Image ID="imgOk7" style="display: none;" runat="server" ImageUrl="~/images/ok.png" />
                        <asp:Image ID="imgErr7" style="display: none;" runat="server" ImageUrl="~/images/buttons/delete.png" />
                    </td>
                    <td class="col4">
                        <asp:Label ID="lbErr7" style="display: none;" runat="server" Text="Nie zaplanowano urlopu" ></asp:Label>
                        <asp:HiddenField ID="hidErr7" runat="server" />
                    </td>
                </tr>
                --%>



                <tr>
                    <td class="col1"><span class="t1">Urlop zaplanowany (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbVal4" runat="server" ></asp:Label>
                    </td>
                    <td class="col3">
                        <asp:Image ID="imgOk4" style="display: none;" runat="server" ImageUrl="~/images/ok.png" />
                        <asp:Image ID="imgErr4" style="display: none;" runat="server" ImageUrl="~/images/buttons/delete.png" />
                    </td>
                    <td class="col4">
<%--
                        <asp:Label ID="lbErr4" style="display: none;" runat="server" Text="Nie zaplanowano urlopu" ></asp:Label>
--%>
                        <asp:Label ID="lbErr4" style="display: none;" runat="server" Text="Nie zaplanowano wymaganej ilości dni" ></asp:Label>
                        <asp:HiddenField ID="hidErr4" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="col1"><span class="t1">Pozostało do zaplanowania (dni):</span></td>
                    <td class="col2">
                        <asp:Label ID="lbVal1" runat="server" ></asp:Label>
                    </td>
                    <td class="col3">
                        <asp:Image ID="imgOk1" style="display: none;" runat="server" ImageUrl="~/images/ok.png" />
                        <asp:Image ID="imgErr1" style="display: none;" runat="server" ImageUrl="~/images/buttons/delete.png" />
                    </td>
                    <td class="col4">
                        <%--
                        <asp:Label ID="lbErr1" style="display: none;" runat="server" Text="Nie można zaplanować więcej dni niż przysługuje" ></asp:Label>
                        --%>
                        <asp:Label ID="lbErr1" style="display: none;" runat="server" Text="Zaplanowano więcej dni niż przysługuje" ></asp:Label>
                        <asp:HiddenField ID="hidErr1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">        
                        <asp:Label ID="lb14" class="t1" runat="server" Text="14 kolejnych dni wolnych, zaplanowano:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:Label ID="lbVal2" runat="server" ></asp:Label>
                        <asp:HiddenField ID="hidNom14" runat="server" />
                    </td>
                    <td class="col3">
                        <asp:Image ID="imgOk2" style="display: none;" runat="server" ImageUrl="~/images/ok.png" />
                        <asp:Image ID="imgErr2" style="display: none;" runat="server" ImageUrl="~/images/buttons/delete.png" />
                        <asp:Image ID="imgWarn2" style="display: none;" runat="server" ImageUrl="~/images/error.png" />
                    </td>
                    <td class="col4">
                        <asp:Label ID="lbErr2" style="display: none;" runat="server" Text="Nie zaplanowano wymaganej ilości dni" ></asp:Label>
                        <asp:HiddenField ID="hidErr2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">        
                        <asp:Label ID="lbZaleg" class="t1" runat="server" Text="Urlop zaległy do końca września:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:Label ID="lbVal5" runat="server" ></asp:Label>
                        <asp:HiddenField ID="hidZaleg" runat="server" />
                    </td>
                    <td class="col3">
                        <asp:Image ID="imgOk5" style="display: none;" runat="server" ImageUrl="~/images/ok.png" />
                        <asp:Image ID="imgErr5" style="display: none;" runat="server" ImageUrl="~/images/buttons/delete.png" />
                    </td>
                    <td class="col4">
                        <asp:Label ID="lbErr5" style="display: none;" runat="server" Text="Nie zaplanowano wymaganej ilości dni" ></asp:Label>
                        <asp:HiddenField ID="hidErr5" runat="server" />
                    </td>
                </tr>
                <%--
                <tr>
                    <td class="col1">
                        <asp:Label ID="lb812" class="t1" runat="server" Text="8/12 wymiaru do końca sierpnia:"></asp:Label>
                    </td>
                    <td class="col2">
                        <asp:Label ID="lbVal3" runat="server" ></asp:Label>
                        <asp:HiddenField ID="hidNom812" runat="server" />
                    </td>
                    <td class="col3">
                        <asp:Image ID="imgOk3" style="display: none;" runat="server" ImageUrl="~/images/ok.png" />
                        <asp:Image ID="imgErr3" style="display: none;" runat="server" ImageUrl="~/images/buttons/delete.png" />
                    </td>
                    <td class="col4">
                        <asp:Label ID="lbErr3" style="display: none;" runat="server" Text="Nie zaplanowano wymaganej ilości dni" ></asp:Label>
                        <asp:HiddenField ID="hidErr3" runat="server" />
                    </td>
                </tr>
                --%>                
            </table>
        </td>
    </tr>
</table>

