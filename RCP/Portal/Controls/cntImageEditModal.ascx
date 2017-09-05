<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImageEditModal.ascx.cs" Inherits="HRRcp.Portal.Controls.cntImageEditModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>

<uc1:cntModal runat="server" ID="cntModal" CssClass="cntImageEdit" Title="Edycja zdjęcia">
 <HeaderTemplate>
            <h4>
                <asp:Label ID="lbTitleNowe" runat="server" Text="Dodaj nowe zdjęcie"></asp:Label>
            </h4>
        </HeaderTemplate>
        <ContentTemplate>
         
                                <cc1:AsyncFileUpload ID="AsyncFileUpload1" runat="server" 
                                    CssClass="upload"                                    
                                    ThrobberID="imgLoader"  
                                    ToolTip="Wybierz plik" 
                                    OnUploadedComplete="FileUploadComplete" 
                                    OnUploadedFileError="FileUploadError" />
                              
        </ContentTemplate>
        <FooterTemplate>
        </FooterTemplate>

</uc1:cntModal>
