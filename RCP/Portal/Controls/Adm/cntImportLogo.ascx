<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImportLogo.ascx.cs" Inherits="HRRcp.Portal.Controls.Adm.cntImportLogo" %>


<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

 <uc1:cntModal runat="server" ID="cntModal3" Backdrop="false" Keyboard="false" ShowFooter="true" Width="900px" CloseButtonText="Anuluj" ShowCloseButton="true" >
     <HeaderTemplate>
         <h4 class="modal-title">
             <i class="fa fa-link"></i>
             Zmiana Logo Portalu
         </h4>
     </HeaderTemplate>

     <ContentTemplate>
         <asp:UpdatePanel runat="server" UpdateMode="Always">

             <ContentTemplate>
                 <%--<div class="fileupload round5" runat="server" id="div3">--%>
                                          
                     <div class="row">
                         <div class="col-4 col-md-5">
                             <h6>W celu zmiany logo portalu naciśnij przycisk Wybierz plik a następnie Zapisz</h6>
                             <asp:FileUpload ID="logoFile" CssClass="file" runat="server" />                           
                         </div>
                     </div>
                 <br />
                <%-- </div> --%>               
             </ContentTemplate>              
         </asp:UpdatePanel>

     </ContentTemplate>

     <FooterTemplate>
         <asp:UpdatePanel runat="server" UpdateMode="Always">
             <ContentTemplate>
                 <asp:Button ID="btnLogo" runat="server"  OnClick="btnLogo_Click" CssClass="btn btn-success cntImportLogo" Text="Zapisz" />

             </ContentTemplate>
             <Triggers>
                 <asp:PostBackTrigger ControlID="btnLogo" />
             </Triggers>
         </asp:UpdatePanel>
     </FooterTemplate>
 </uc1:cntModal>