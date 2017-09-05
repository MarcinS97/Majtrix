<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportStruktura.ascx.cs" Inherits="HRRcp.Controls.ImportStruktura" %>

<script type="text/javascript">
    function checkFile() {
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

<div class="fileupload round5 cntImportStruktura">
    <asp:FileUpload ID="FileUpload1" CssClass="fileupload" runat="server" />
    <asp:Button ID="btImport" runat="server" CssClass="button200" Text="Import struktury organizacyjnej" onclick="btImportStruktura_Click" OnClientClick="javascript:return checkFile();" /><br />
    <asp:Button ID="btBackup" runat="server" CssClass="button150" Text="Kopia bezpieczeństawa" onclick="btBackup_Click" />
    <asp:Button ID="btRestore" runat="server" CssClass="button" Text="Przywróć" onclick="btRestore_Click" />
</div>