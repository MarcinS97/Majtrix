<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PPSumyLegenda.ascx.cs" Inherits="HRRcp.Controls.PPSumyLegenda" %>

<div class="legendabox">
    <span class="t5">Podsumowanie</span><br />
    <div class="legenda PPSumyLegenda">
        <table class="legenda PPSumyLegenda">
            <tr>
                <td colspan="2">
                    Kolumna I - Czas pracy [godz.]
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <span class="suma">
                        Zm.<br />
                        Czas<br />
                        [h]
                    </span>
                </td>
                <td class="col2">
                    czas wynikający z zaplanowanych / skorygowanych zmian<br />
                    suma czasu pracy na zmianach<br />
                    <h5>? nn</h5> - czas pracy, który należy wyjasnić
                    <%--                    
                    <str>? nn</str> - czas pracy, który należy wyjasnić
                    --%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    Kolumna II - Nadgodziny [godz.]
                </td>
            </tr>
            <tr class="line2">
                <td class="col1">
                    <span class="suma">
                        Nad.<br />
                        50%<br />
                        100%
                    </span>
                </td>
                <td class="col2">
                    <br />
                    nadgodziny w dzień - 50<br />
                    nadgodziny w nocy (lub w dni wolne) - 100
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    Kolumna III - Czas pracy w nocy [godz.]
                </td>
            </tr>
            <tr class="line3">
                <td class="col1">
                    <span class="suma">
                        Noc<br />
                        <br />
                        [h]                    
                    </span>
                </td>
                <td class="col2">
                    <br />
                    <br />
                    czas pracy w nocy
                </td>
            </tr>
        </table>
    </div>
</div>