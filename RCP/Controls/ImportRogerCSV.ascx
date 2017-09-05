<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportRogerCSV.ascx.cs" Inherits="HRRcp.Controls.ImportRogerCSV" %>

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

<div class="fileupload round5">
    <asp:FileUpload ID="FileUpload1" CssClass="fileupload" runat="server" />
    <asp:Button ID="btImport" runat="server" CssClass="button" Text="Import danych RCP z plików CSV" onclick="btImport_Click" OnClientClick="javascript:return checkFile();" /><br />
    Uwaga! Nie powinno się wykonywać importu dni, które nie są jeszcze odczytane z systemu Roger'ów!!!
</div> 