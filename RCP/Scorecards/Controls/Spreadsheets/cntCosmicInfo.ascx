<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCosmicInfo.ascx.cs"
    Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntCosmicInfo" %>
    
<div id="ctCosmicInfo" runat="server" class="cntCosmicInfo cnt">
    <asp:HiddenField ID="hidOutsideJob" runat="server" Visible="false" />
    <asp:HiddenField ID="hidGenre" runat="server" Visible="false" />
    <asp:HiddenField ID="hidTeamLeader" runat="server" Visible="false" />
    <table id="tbCosmicInfo" class="tbCosmicInfo">
        <tr>
            <td class="ultymat" colspan="11">
                <input id="tbFilter" type="text" class="tbFilter" placeholder="Wyszukaj czynność" />
                <div class="showColumnsTab" id="plusik" runat="server">
                    <a href="javascript://" class="fa fa-plus showColumns"></a>
                </div>
            </td>
        </tr>
        <tr>
            <td runat="server" rowspan="2" class="day">
                <asp:Label ID="Label1" runat="server" Text="Dzień" />
            </td>
            <td runat="server" class="pr" colspan="3">
                <asp:Label ID="lblProd" runat="server" Text="Produktywność" />
            </td>
            <td runat="server" colspan="3">
                <asp:Label ID="Label4" runat="server" Text="Jakość" />
            </td>
            <td runat="server" rowspan="2" class="war2">
                <asp:Label ID="Label13" runat="server" Text="Czas prod. (bez przerwy) - do prod." />
            </td>
            <td runat="server" rowspan="2" class="czasnieprod">
                <asp:Label ID="Label10" runat="server" Text="Czas nieprod." />
            </td>
            <td runat="server" rowspan="2" colspan="2" class="prinnyarkusz">
                <asp:Label ID="Label11" runat="server" Text="Praca na rzecz innego arkusza (godz.)" />
            </td>
            <td id="tdPlannedTeamSize" runat="server" class="plannedteamsize hid" rowspan="2"
                visible="false">
                <asp:Label ID="lblPlannedTeamSize" runat="server" Text="Wielkość zespołu" />
            </td>
  <%--          <td id="tdTeamSizeCorrection" runat="server" class="teamsizecorrection hid" rowspan="2"
                visible="false">
                <asp:Label ID="Label16" runat="server" Text="Korekta wielkości zespołu" />
            </td>--%>
            <td runat="server" rowspan="2" class="nominal hid">
                <asp:Label ID="Label7" runat="server" Text="Nominalny czas pracy" />
            </td>
            <td runat="server" rowspan="2" class="nieob hid">
                <asp:Label ID="Label8" runat="server" Text="Godz. nieob." />
            </td>
            <td id="tdKodNieob" runat="server" rowspan="2" class="kodnieob hid">
                <asp:Label ID="Label9" runat="server" Text="Kod nieob." />
            </td>
            <td runat="server" rowspan="2" class="war1 hid">
                <asp:Label ID="Label12" runat="server" Text="Czas prod." />
            </td>
            <td id="tdTLPrac" runat="server" rowspan="2" class="tlprac hid">
                <asp:Label ID="Label16" runat="server" Text="Praca TL" />
            </td>
            <td runat="server" rowspan="2" class="nadg hid">
                <asp:Label ID="Label14" runat="server" Text="Nadgodziny (godz.)" />
            </td>
        </tr>
        <tr>
            <td class="aim">
                <asp:Label ID="Label2" runat="server" Text="Cel" />
            </td>
            <td class="result">
                <asp:Label ID="Label3" runat="server" Text="Rezultat" />
            </td>
            <td class="face">
            </td>
            <td class="aim">
                <asp:Label ID="Label5" runat="server" Text="Cel" />
            </td>
            <td class="result">
                <asp:Label ID="Label6" runat="server" Text="Rezultat" />
            </td>
            <td class="face">
            </td>
        </tr>
        <tr class="sum">
            <td class="day">
                <asp:Label ID="Label15" runat="server" Text="Suma" />
            </td>
            <td class="aim">
            </td>
            <td class="result">
                <span class="result1"></span>
            </td>
            <td class="face">
            </td>
            <td class="aim">
            </td>
            <td class="result">
                <span class="result2"></span>
            </td>
            <td class="face">
            </td>
            <td class="war2">
                <span class="w2"></span>
            </td>
            <td class="czasnieprod">
                <span class="unprod"></span>
            </td>
            <td class="prinnyarkusz">
                <span class="other"></span>
            </td>
            <td class="prinnyarkuszx">
            </td>
            <td id="tdPlannedTeamSizeSum" runat="server" class="plannedteamsize hid" visible="false">
            </td>
       <%--     <td id="tdTeamSizeCorrectionSum" runat="server" class="teamsizecorrection hid" visible="false">
            </td>--%>
            <td class="nominal hid">
                <span class="nom"></span>
            </td>
            <td class="nieob hid">
                <span class="abs"></span>
            </td>
            <td id="tdKodNieobSum" runat="server" class="kodnieob hid">
            </td>
            <td class="war1 hid">
                <span class="w1"></span>
            </td>
            <td id="tdTLPrac2" runat="server" class="tlprac hid">
                <span class="tlprac"></span>
            </td>
            <td class="nadg hid">
                <span class="over"></span>
            </td>
        </tr>
    </table>
</div>
