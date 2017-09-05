<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlEdit.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSqlEdit" %>

<div id="paSqlEdit" runat="server" class="cntSqlEdit">
    <div class="buttons pull-right" >
        <asp:Button ID="btSave" runat="server" CssClass="button75 btn btn-success" Text="Zapisz" onclick="btSave_Click" />
        <asp:Button ID="btCancel" runat="server" CssClass="button75 btn btn-default " Text="Anuluj" onclick="btCancel_Click" />
    </div>
    <asp:TextBox ID="tbSql" Rows="50" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
</div>
