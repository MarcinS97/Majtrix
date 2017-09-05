<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAvatarEdit.ascx.cs" Inherits="HRRcp.Portal.Controls.Social.cntAvatarEdit" %>

<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>





<div id="paOgloszenieEdit" runat="server" class="cntOgloszenieEdit">
    <uc1:cntModal runat="server" ID="modalOgloszenie" ShowCloseButton="false" ShowFooter="false" CssClass="modalOgloszenie" Width="600px" Keyboard="false" Backdrop="false" >
        <HeaderTemplate>
            <h4>
                <asp:Label ID="lbTitleNowe" runat="server" Text="Zmiana avatara"></asp:Label>
            </h4>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="form-horizontal">


                <div class="form-group">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-10">
                                <label><asp:Literal ID="ltZalacz" runat="server" Text="Załącz zdjęcie (opcjonalnie, maksymalny rozmiar: {0}MB)"></asp:Literal></label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 step">
                            </div>
                            <div class="col-md-2 image">
                                <div>
                                    <asp:Image ID="Image1" runat="server" Width="80px"/>
                                </div>
                            </div>                        
                            <div class="col-md-7 paFileUpload">
                                <asp:HiddenField ID="hidImage" runat="server" />
                                <asp:HiddenField ID="hidImageError" runat="server" />
                                <asp:HiddenField ID="hidImageErrorMsg" runat="server" />
                                <asp:HiddenField ID="hidScript" runat="server" Visible="false" Value="
top.document.getElementById('{0}').src='{1}';       //'?' + new Date().getTime();
top.document.getElementById('{2}').value='{3}';     //hidImage
top.document.getElementById('{4}').value='{5}';     //hidError
top.document.getElementById('{6}').value='{8}';     //hidErrorMessage
top.document.getElementById('{7}').innerHTML='{8}'; //lbError
$(top.document.getElementById('{9}')).show();       //btImageDelete
//setTimeout(function() {{ top.document.getElementById('{6}').click(); }}, 250);                                  
                                    "/>
                                <cc1:AsyncFileUpload ID="AsyncFileUpload1" runat="server" 
                                    CssClass="upload"                                    
                                    ThrobberID="imgLoader"  
                                    ToolTip="Wybierz plik" 
                                    OnUploadedComplete="FileUploadComplete" 
                                    OnUploadedFileError="FileUploadError" />
                                <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                                <asp:LinkButton ID="btImageDelete" runat="server" CssClass="btn-delete" OnClick="btImageDelete_Click" Style="display: none;" ToolTip="Usuń zdjęcie"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>       
                                <%--                                
                                <asp:LinkButton ID="btImageDelete" runat="server" CssClass="btn-delete" OnClick="btImageDelete_Click" Visible="false" ToolTip="Usuń zdjęcie"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>       
                                <asp:Button ID="btImage" runat="server" CssClass="button_postback" OnClick="btImage_Click" />                         
                                --%>
                                <br />
                                <asp:Label ID="lbImageError" runat="server" CssClass="error" />
                            </div>
                        </div>
                    </div>
                </div>



            </div>
            <div class="buttons">
                <%--                
                <asp:Button runat="server" ID="modalAccept" CssClass="btn btn-success left" Text="Zaakceptuj"  OnClick="modalAccept_Click" />
                <asp:Button runat="server" ID="modalReject" CssClass="btn btn-danger left" Text="Odrzuć"  OnClick="modalReject_Click" />
                --%>
                <asp:Button runat="server" ID="modalSave"   CssClass="btn btn-default" Text="Zapisz" OnClick="modalSave_Click"  />
                <asp:Button runat="server" ID="modalCancel" CssClass="btn btn-default" Text="Anuluj" CommandName="Cancel" data-dismiss="modal" />
            </div>
        </ContentTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </uc1:cntModal>
</div>
