<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPDFViewer.ascx.cs" Inherits="HRRcp.Portal.Controls.cntPDFViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="paPDFViewer" runat="server" class="paPDFViewer">
    <div id="paAddPlik" runat="server" visible="false" class="paAddPlik">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="paButton" runat="server" class="paEditButton form-group">
                    <asp:Button ID="btEdit" runat="server" CssClass="button btn btn-default" Text="Dodaj plik" onclick="btEdit_Click" />
                </div>
                <%-- jak nie jest widoczny od razu to musi byc dodatkowy z display:none zeby zadziałał --%>
                <div style="display: none;">
                    <asp:AsyncFileUpload ID="AsyncFileUpload2" runat="server" CssClass="fileupload"                                                                     
                        ToolTip="Wybierz plik" 
                        UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                        UploaderStyle="Modern" ThrobberID="imgLoader"                         
                        OnUploadedComplete="FileUploadComplete" 
                        OnUploadedFileError="FileUploadError" 
                        />
                </div>
                <div id="paEdit" runat="server" class="paEdit" visible="false">
                    <asp:Label ID="Label5" runat="server" CssClass="insert_info" Text="Dodaj plik pdf"></asp:Label>                
                    <asp:Label ID="Label1" runat="server" CssClass="label" Text="Bieżący plik:"></asp:Label>
                    <asp:Label ID="lbFile" runat="server" CssClass="value" Text="brak"></asp:Label><br />
                    
                    <div class="">
                        <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="btn btn-default btn-file"                                                                     
                        ToolTip="Wybierz plik" 
                        OnUploadedComplete="FileUploadComplete" 
                        OnUploadedFileError="FileUploadError" 
                        />
                    </div>
                    <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>    
                    <div class="buttons">
                        <asp:Button ID="btCancel" runat="server" CssClass="button100 btn btn-default" Text="Anuluj" onclick="btCancel_Click" />
                        <asp:Button ID="btUpdate" runat="server" CssClass="button_postback" Text="Anuluj" onclick="btUpdate_Click" />
                    </div>                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:Literal ID="Literal1" runat="server" Visible="true"></asp:Literal>
    
    
<%--    <iframe src="http://docs.google.com/gview?url=http://localhost:50675/Portal/Gazetka/Jabil Family nr 31.pdf&embedded=true"
    style="width:718px; height:700px;" frameborder="0"></iframe>
--%>    
    
<%--    <div id="pdf">
          <iframe src="/Portal/Gazetka/pdf.html" style="width: 100%; height: 100%;" frameborder="0" scrolling="no">
               <p>It appears your web browser doesn't support iframes.</p>
          </iframe>
   </div> 
--%>    
    
   
<%--<iframe src="/portal/gazetka/Jabil Family nr 31.pdf" style="width: 100%; height: 100%;" frameborder="0" scrolling="no" type="application/pdf" >
        <p>It appears your web browser doesn't support iframes.</p>
   </iframe>
--%>   
       
<%--    <embed src="http://localhost:50675/Portal/MS_Win8-v.5_tcm3-163290.pdf" class="viewer" >    
--%>

<%--    <embed src="/Portal/MS_Win8-v.5_tcm3-163290.pdf" class="viewer" >    
--%>
</div>
