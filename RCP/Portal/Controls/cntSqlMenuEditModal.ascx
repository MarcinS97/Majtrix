<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlMenuEditModal.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSqlMenuEditModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>


<style>
    .cntSqlMenuEditModal .form-control { width: 400px; display: inline-block; }
    .cntSqlMenuEditModal .label { width: 150px; }
</style>


<uc1:cntModal runat="server" ID="cntModal" CssClass="cntSqlMenuEditModal" Title="Edycja">
    <ContentTemplate>
        <uc1:dbField runat="server" ID="Grupa" Label="Grupa:" Type="tb" />
        <uc1:dbField runat="server" ID="ParentId" ValueField="ParentIdText" DataSourceID="dsParentId" Type="ddl" Label="ParentId" />

        <asp:SqlDataSource ID="dsParentId" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
            SelectCommand="select null ParentId, 'wybierz ...' ParentIdText, 0 Sort 
                                           union all
                                           select Id ParentId,  isnull(Grupa +  '- ', '') + MenUtext ParentIdText, 1 Sort 
                                            from SqlMenu order by Sort, ParentIdText"></asp:SqlDataSource>
        <uc1:dbField runat="server" ID="MenuText" Label="MenuText:" Type="tb" />
        <uc1:dbField runat="server" ID="MenuTextEN" Label="MenuTextEN:" Type="tb" />
        <uc1:dbField runat="server" ID="ToolTip" Label="ToolTip:" Type="tb" />
        <uc1:dbField runat="server" ID="ToolTipEN" Label="ToolTipEN:" Type="tb" />
        <uc1:dbField runat="server" ID="Command" Label="Command:" Type="tb" />
        <uc1:dbField runat="server" ID="Kolejnosc" Label="Kolejność:" Type="tb" />
        <uc1:dbField runat="server" ID="Aktywny" Label="Aktywny:" Type="check" />
        <uc1:dbField runat="server" ID="Image" Label="Image:" Type="tb" />
        <uc1:dbField runat="server" ID="Rights" Label="Rights:" Type="tb" />
        <uc1:dbField runat="server" ID="Par1" Label="Par1:" Type="tb" />
        <uc1:dbField runat="server" ID="Par2" Label="Par2:" Type="tb" />
        <uc1:dbField runat="server" ID="Wydruk" Label="Wydruk:" Type="check" />
        <uc1:dbField runat="server" ID="Sql" Label="Sql:" Type="tb" TextMode="MultiLine" />
        <uc1:dbField runat="server" ID="SqlParams" Label="SqlParams:" Type="tb" />
        <uc1:dbField runat="server" ID="Mode" Label="Mode:" Type="tb" />
        <uc1:dbField runat="server" ID="Javascript" Label="Javascript:" Type="tb" />
        <uc1:dbField runat="server" ID="Class" Label="Class:" Type="tb" />
    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnDeleteConfirm" runat="server" CssClass="btn btn-danger" OnClick="btnDeleteConfirm_Click" Text="Usuń" />
        <asp:Button ID="btnDelete" runat="server" CssClass="hidden" OnClick="btnDelete_Click" />
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click" Text="Zapisz" />
    </FooterTemplate>
</uc1:cntModal>

<asp:SqlDataSource ID="dsData" runat="server" SelectCommand="select * from SqlMenu where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />
<asp:SqlDataSource ID="dsDelete" runat="server" SelectCommand="delete from SqlMenu where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />