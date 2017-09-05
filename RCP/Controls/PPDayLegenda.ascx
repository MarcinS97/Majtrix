<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PPDayLegenda.ascx.cs" Inherits="HRRcp.Controls.PPDayLegenda" %>

<div class="legendabox">
    <span class="t5">Dzień</span><br />
    <div class="legenda PPDayLegenda">
        <table>
            <tr class="line1">
                <td class="col1">
                    <div class="day">
                        Z2<br />
                        
                        <h1>8</h1><br />
                        <h2>+3:30</h2><br />
                        <h3>●7:30</h3>

                        <%--
                        <wt1>8</wt1><br />
                        <wt2>+3:30</wt2><br />
                        <wt3>●7:30</wt3>
                        --%>
                    </div>
                </td>
                <td class="col2" rowspan="2">
                    Symbol zmiany<br />
                    Czas pracy na zmianie [godz.]<br />
                    Nadgodziny łącznie (w dzień i w nocy) [godz.]<br />
                    Czas pracy w nocy [godz.]<br />
                    <br />
                    <span class="triangle"></span> - dane zmodyfikowane przez kierownika<br />
                    + - czas nadgodzin<br />
                    ● - część lub cały czas przypadał w godzinach nocnych<br />
                    <br />
                    Szczegóły czasu dostępne są po kliknięciu w komórkę.<br />
                    <br />
                    Dni wymagające wyjaśnienia - zgłoszone przez system alerty, są oznaczane w tabeli migającym kolorem czerwonym.<br />
                    <br />
                    Kolor bordowy wszystkich napisów oznacza, że w zaakceptowanym dniu występowały alerty.<br />
                    <br />
                    Dni wymagające ponownej akceptacji zaznaczone są kreskowaną obwolutą w kolorze czerwonym.
                </td>
            </tr>
            <tr class="line2">
                <td class="col1">
                    <div class="day dayborder">
                        Z2<br />
                        <h1>8</h1><br />
                        <h2>+3:30</h2><br />
                        <h3>●7:30</h3>


                        <%--                        
                        <wt1>8</wt1><br />
                        <wt2>+3:30</wt2><br />
                        <wt3>●7:30</wt3>
                        --%>
                    </div>
                </td>
            </tr>
            <tr class="line3">
                <td class="col1">
                    <asp:Image ID="imgAccepted" ImageUrl="../images/ok_small_gray.png" runat="server" />
                </td>
                <td class="col2">
                    Dzień zaakceptowany z zamkniętego tygodnia,<br />edycja zablokowana
                </td>
            </tr>
            <tr class="line3">
                <td class="col1">
                    <asp:Image ID="Image1" ImageUrl="../images/ok_small.png" runat="server" />
                </td>
                <td class="col2">
                    Dzień zaakceptowany z bieżącego tygodnia,<br />edycja możliwa
                </td>
            </tr>
            
            
            <tr class="line4">
                <td class="col1">
                    <br />
                    <div class="day noedit">
                        Z2<br />
                        <h1>8</h1><br />
                        <h2>+3:30</h2><br />
                        <h3>●7:30</h3>
                        <%--
                        <wt1>8</wt1><br />
                        <wt2>+3:30</wt2><br />
                        <wt3>●7:30</wt3>
                        --%>
                    </div>
                </td>
                <td class="col2" >
                    <br />
                    Pracownik przeniesiony, brak możliwości edycji
                </td>
            </tr>
            
            <tr class="line5">
                <td class="col1">
                    <div class="day noedit move">
                        Z2<br />
                        <h1>8</h1><br />
                        <h2>+3:30</h2><br />
                        <h3>●7:30</h3>
                        <%--
                        <wt1>8</wt1><br />
                        <wt2>+3:30</wt2><br />
                        <wt3>●7:30</wt3>
                        --%>                        
                    </div>
                </td>
                <td class="col2" >
                    Pracownikowi skończył się okres przeniesienia, nie jestem jego kierownikiem, brak możliwości edycji
                </td>
            </tr>
            
            <tr class="line6">
                <td class="col1">
                    <div class="day move">
                        Z2<br />
                        <h1>8</h1><br />
                        <h2>+3:30</h2><br />
                        <h3>●7:30</h3>
                        <%--
                        <wt1>8</wt1><br />
                        <wt2>+3:30</wt2><br />
                        <wt3>●7:30</wt3>
                        --%>
                    </div>
                </td>
                <td class="col2" >
                    Pracownikowi skończył się okres przeniesienia, jestem jego kierownikiem, edycja możliwa, należy wypełnić wniosek o przeniesienie lub pozostawić pracownika u siebie
                </td>
            </tr>
        </table>
    </div>
</div>