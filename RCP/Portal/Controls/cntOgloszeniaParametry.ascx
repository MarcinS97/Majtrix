<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOgloszeniaParametry.ascx.cs" Inherits="HRRcp.Portal.Controls.cntOgloszeniaParametry" %>
<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Portal" TagPrefix="cc1" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div id="paOgloszeniaParametry" runat="server" class="cntOgloszeniaParametry">
    <h1>Ustawienia</h1>
    <div class="fields">
        <uc1:dbField runat="server" ID="MaxOgloszenia" Type="tb" Min="0" Max="100" MaxLength="3" ValidChars="0123456789" Rq="true" StVisible="3" 
            Label="Maksymalna ilość wystawionych jednocześnie ogłoszeń:"/>
        <uc1:dbField runat="server" ID="MaxDni" Type="tb" Min="0" Max="366" MaxLength="3" ValidChars="0123456789" Rq="true" StVisible="3" 
            Label="Maksymalna czas wystawienia ogłoszenia (dni):"/>
        <uc1:dbField runat="server" ID="MaxFileSizeMB" Type="tb" Min="0" Max="99" MaxLength="3" ValidChars="0123456789" Rq="true" StVisible="3" 
            Label="Maksymalny rozmiar zdjęcia (MB):"/>
    </div>
    <div class="buttons">
        <cc1:WnButton ID="wbtSave"   CssClass="btn btn-default" runat="server" Text="Zapisz" onclick="wbtSave_Click" StVisible="3" />
        <cc1:WnButton ID="wbtCancel" CssClass="btn btn-default" runat="server" Text="Anuluj" onclick="wbtCancel_Click" StVisible="3" />
        <cc1:WnButton ID="wbtEdit"   CssClass="btn btn-default" runat="server" Text="Edycja" onclick="wbtEdit_Click" StVisible="2" />        
        <%--    
        <asp:Button ID="btEdit" runat="server" CssClass="btn btn-default" Text="Edycja" OnClick="btEdit_Click" />
        <asp:Button ID="btSave" runat="server" Text="Zapisz" CssClass="btn btn-default" Visible="false" OnClick="btSave_Click" />
        <asp:Button ID="btCancel" runat="server" Text="Anuluj" CssClass="btn btn-default" Visible="false" OnClick="btCancel_Click" />
        --%>
    </div>
    <hr />
    <h1>Regulamin</h1>
    <div class="regulamin">
        <%--
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
        <br />
        <asp:Label ID="lbImageError" runat="server" CssClass="error" />
        --%>

        <script type="text/javascript">
            function checkFileRegulamin() {
                fu = document.getElementById('<%=FileUpload1.ClientID%>');
                if (fu != null) {
                    if (!fu.value) {
                        alert("Brak pliku do załadowania.");
                        return false;
                    }
                }
                return true;
            }
        </script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <span class="label1">Bieżący regulamin:</span>
                <a Title="Pobierz regulamin" target="_blank" href="<%= UrlRegulamin %>" ><%= FileRegulamin %></a>
                <div>
                    <asp:FileUpload ID="FileUpload1" CssClass="fileupload" runat="server" />
                    <asp:Button ID="btRegulaminUpload" runat="server" CssClass="btn btn-default" Text="Aktualizuj regulamin" OnClick="btRegulaminUpload_Click" OnClientClick='javascript:if (checkFileRegulamin()) {{showAjaxProgress();}} else return false;' />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btRegulaminUpload"/>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>

<asp:SqlDataSource ID="dsParametry" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
    SelectCommand="select top 1 * from poOgloszeniaParametry"
    >
</asp:SqlDataSource>
