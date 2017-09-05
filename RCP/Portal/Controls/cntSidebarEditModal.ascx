<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSidebarEditModal.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSidebarEditModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Src="~/Portal/Controls/cntIconPicker.ascx" TagPrefix="uc1" TagName="cntIconPicker" %>



<uc1:cntModal runat="server" ID="cntModal" Title="Edycja menu">
    <ContentTemplate>
        <asp:HiddenField ID="hidGroup" runat="server" Visible="false" />
        <uc1:dbField runat="server" ID="ParentId" ValueField="ParentIdText" DataSourceID="dsParentId" Type="ddl" Label="Poziom:" />

        <asp:SqlDataSource ID="dsParentId" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
            SelectCommand="select 0 ParentId, 'Poziom główny ...' ParentIdText, 0 Sort 
                                           union all
                                           select Id ParentId, MenUtext ParentIdText, 1 Sort 
                                            from SqlMenu where Grupa = 'LEFTMENU' + @grupa order by Sort, ParentIdText">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidGroup" PropertyName="Value" Name="grupa" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

        <uc1:dbField runat="server" ID="MenuText" Label="Nazwa:" Type="tb" Rq="true" ValidationGroup="vgSave" />
        <uc1:dbField runat="server" ID="Command" Label="Link:" Type="tb" Rq="true" ValidationGroup="vgSave" />
        <uc1:dbField runat="server" ID="Kolejnosc" Label="Kolejność:" Type="tb" Rq="true" ValidationGroup="vgSave" />
        <uc1:dbField runat="server" ID="Aktywny" Label="Aktywny:" Type="check" />

        <uc1:dbField runat="server" ID="Rights" Label="Uprawnienia:" Type="tb" />


        <uc1:cntIconPicker runat="server" ID="cntIconPicker" Columns="8" Visible="true" />



    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnDeleteConfirm" runat="server" CssClass="btn btn-danger" OnClick="btnDeleteConfirm_Click" Text="Usuń" />
        <asp:Button ID="btnDelete" runat="server" CssClass="hidden" OnClick="btnDelete_Click" />

        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click" Text="Zapisz" ValidationGroup="vgSave" />
    </FooterTemplate>
</uc1:cntModal>

<asp:HiddenField ID="hidMenuId" runat="server" Visible="false" />

<asp:SqlDataSource ID="dsData" runat="server" SelectCommand="select * from SqlMenu where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />
<asp:SqlDataSource ID="dsDelete" runat="server" SelectCommand="delete from SqlMenu where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />