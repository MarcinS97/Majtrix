<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlikiEdit.ascx.cs" Inherits="HRRcp.Portal.Controls.cntPlikiEdit" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Src="~/Portal/Controls/cntIconPicker.ascx" TagPrefix="uc1" TagName="cntIconPicker" %>

<asp:HiddenField ID="hidGroupId" runat="server" Visible="false" />

<div class="cntPlikiEdit">
    <uc1:cntModal runat="server" ID="cntModal" Title="Edytuj grupę" Backdrop="false">
        <ContentTemplate>
            <uc1:dbField runat="server" ID="MenuText" Type="tb" Label="Nazwa:"  />
            <uc1:dbField runat="server" ID="Tooltip" Type="tb" Label="Informacja:"  />
            <uc1:dbField runat="server" ID="Kolejnosc" Type="tb" Label="Kolejność:" />
            <uc1:dbField runat="server" ID="Aktywny" Type="check" Label="Widoczna:" />
            <uc1:cntIconPicker runat="server" id="cntIconPicker" Columns="8" />
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Zapisz" OnClick="btnSave_Click" />
        </FooterTemplate>
    </uc1:cntModal>
</div>

<asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="select top 1 * from SqlMenu where Id = {0}" />