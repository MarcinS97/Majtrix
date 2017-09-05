<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlEdit.ascx.cs" Inherits="HRRcp.Controls.Portal.cntSqlEdit" %>

<div id="paSqlEdit" runat="server" class="cntSqlEdit">
    <asp:TextBox ID="tbSql" Rows="50" TextMode="MultiLine" runat="server"></asp:TextBox>
    <div class="buttons">
        <asp:Button ID="btSave" runat="server" CssClass="button75" Text="Zapisz" onclick="btSave_Click" />
        <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Anuluj" onclick="btCancel_Click" />
    </div>
</div>
