<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCosmicInfo2.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntCosmicInfo2" %>


<%--<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <div id="ctCosmicInfo2" runat="server" class="cntCosmicInfo2 cnt">
            <table id="tbCosmicInfo2">
                <tr>
                    <td colspan="6" class="ultymat">
                        
                    </td>
   <%--                 <td rowspan="3" class="remover">
                    
                    </td>--%>
                </tr>
                <tr>    
                    <td rowspan="2" class="godzprod">
                        <asp:Label ID="lblProd" runat="server" Text="Ilość godz. prod." />
                    </td>
<%--                    <td rowspan="2" class="prod">
                        <asp:Label ID="Label1" runat="server" Text="Produktywność" />
                    </td>--%>
                    <td colspan="3" class="qc">
                        <asp:Label ID="Label2" runat="server" Text="QC" />
                    </td>
                    <td rowspan="2" class="uwagi" colspan="2">
                        <asp:Label ID="Label3" runat="server" Text="Uwagi" />
                    </td>
                </tr>
                <tr>
                    <td class="ilosc">
                        <asp:Label ID="Label4" runat="server" Text="Ilość" />
                    </td>
                    <td class="bledy">
                        <asp:Label ID="Label5" runat="server" Text="Błędy" />
                    </td>
                    <td class="fpy">
                        <asp:Label ID="Label6" runat="server" Text="FPY" />
                    </td>
                </tr>
                <tr class="sum">
                    <td class="godzprod">
                        <span class="prodHours"></span>
                    </td>
<%--                    <td class="prod">
                        
                    </td>--%>
                    <td class="ilosc">
                        <span class="count"></span>
                    </td>
                    <td class="bledy">
                        <span class="errors"></span>
                    </td>
                    <td class="fpy">
                        <span class="fpyr"></span>
                    </td>
                    <td class="uwagi">
                    </td>
                    <td class="remover" runat="server" id="rowRemover">
                    </td>
                </tr>
            </table>
        </div>
<%--    </ContentTemplate>
</asp:UpdatePanel>
--%>