<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImportALARMUS.ascx.cs" Inherits="HRRcp.Controls.cntImportALARMUS" %>

<script type="text/javascript">
    function checkFileALARMUS() {
        fu = document.getElementById('<%=FileUpload1.ClientID%>');
        if (fu != null) {
            if (!fu.value) {
                alert("Brak pliku do importu.");
                return false;
            }
        }
        return true;
    }
</script>

<div class="fileupload round5 cntImportALARMUS">
    <asp:FileUpload ID="FileUpload1" CssClass="fileupload" runat="server" Visible="false"/>
    <asp:Button ID="btImport" runat="server" CssClass="button200" Text="Import danych wejść/wyjść" onclick="btImport_Click" OnClientClick="javascript:if (checkFileALARMUS()) {showAjaxProgress();return true;} else return false;" Visible="false"/>

    <span id="lbInfo" runat="server" class="info" visible="false" >Ostatnie dane w systemie: <b><asp:Label ID="lbLastData" runat="server" Text="brak danych"></asp:Label></b></span>
    <span id="lbInfoAUTOID" runat="server" class="info2" visible="false" >Ostatnie dane z systemu AutoID: <b><asp:Label ID="lbLastDataAUTOID" runat="server" Text="brak danych"></asp:Label></b></span>
</div> 