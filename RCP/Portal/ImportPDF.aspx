<%@ Page Title="" ValidateRequest="true" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="ImportPDF.aspx.cs" Inherits="HRRcp.Portal.ImportPDF" %>

<%@ Register Src="~/Portal/Controls/cntImportPDF.ascx" TagPrefix="uc1" TagName="cntImportPDF" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/Adm/cntSciezkiPlikiEdit.ascx" TagPrefix="uc1" TagName="cntSciezkiPlikiEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgImportPDF">
        <div class="page-title">Dokumenty PDF - Administracja</div>
        <%--<hr />--%>
            <div class="page-box container wide">                
                <h4><i class="glyphicon glyphicon-euro"></i>Pasek wynagrodzeń</h4>
                <uc1:cntImportPDF runat="server" ID="cntImportPDF_Kwitek" mode="3"/>
                <hr />
                <h4><i class="fa fa-file-powerpoint-o"></i>Dokumenty PIT</h4>
                <uc1:cntImportPDF runat="server" ID="cntImportPDF_PIT" mode="4"/>
                <hr />
                <h4><i class="fa fa-check"></i>Ocena okresowa</h4>
                <uc1:cntImportPDF runat="server" ID="cntImportPDF_Ocena" mode="1"/>
                <hr />
                <h4><i class="fa fa-file-text-o"></i>Dokumenty RMUA</h4>
                <uc1:cntImportPDF runat="server" ID="cntImportPDF_RMUA" mode="2"/>

                <asp:Button ID="btnEdytuj" runat="server" Text="Edytuj" OnClick="btnEdytuj_Click" />
            </div>
        
            <uc1:cntModal runat="server" ID="cntModal" Keyboard="false" Backdrop="false" Title="Edycja Scieżek" Width="1100px">
                <ContentTemplate>
                    <uc1:cntSciezkiPlikiEdit runat="server" ID="cntSciezkiPlikiEdit" />                
                </ContentTemplate>
            </uc1:cntModal>
        
    </div>
</asp:Content>



