<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntArticleEdit.ascx.cs" Inherits="HRRcp.Controls.cntArticleEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<div id="paArticleEdit" runat="server" class="cntArticles cntArticleEdit">
    <asp:Label ID="lbEdit" runat="server" CssClass="edit_info" Text="Edycja" Visible="false"></asp:Label>
    <asp:Label ID="lbInsert" runat="server" CssClass="insert_info" Text="Nowy artykuł" Visible="false"></asp:Label>
    
    <div class="grupa_eit">
        <div class="image">
        
<%--
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>
--%>            
            <asp:AsyncFileUpload ID="AsyncFileUpload2" runat="server" CssClass="fileupload" 
                ToolTip="Wybierz plik" 
                UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                UploaderStyle="Modern" ThrobberID="imgLoader"                         
                OnUploadedComplete="FileUploadComplete" 
                OnUploadedFileError="FileUploadError" 
                />
            <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
        </div>
        
        <div class="buttons buttons_top">        
            <div class="left">
                <asp:Label ID="Label1" runat="server" CssClass="label" Text="Justowanie:"></asp:Label>
                <asp:Button ID="btLeft" runat="server" CssClass="button" Text="Do lewej" OnClientClick="ajaxEditorImageJust(-1);return false;"/>
                <asp:Button ID="btCenter" runat="server" CssClass="button" Text="Wyśrodkuj" OnClientClick="ajaxEditorImageJust(0);return false;"/>
                <asp:Button ID="btRight" runat="server" CssClass="button" Text="Do prawej" OnClientClick="ajaxEditorImageJust(1);return false;"/>
            </div>
            <asp:Label ID="Label2" runat="server" CssClass="label" Text="Rozmiar:"></asp:Label>
            <asp:Button ID="Button1" runat="server" CssClass="button" Text="Zmniejsz" OnClientClick="ajaxEditorImageSize(-1);return false;"/>
            <asp:Button ID="Button2" runat="server" CssClass="button" Text="Powiększ" OnClientClick="ajaxEditorImageSize(1);return false;"/>
        </div>

        <cc1:Editor ID="Tekst1Editor" runat="server" NoUnicode="True" NoScript="true" CssClass="editor" DesignPanelCssPath="~/styles/ajaxeditor.css" />
        
        <div class="buttons">        
            <div class="left">
                <asp:Label ID="Label4" runat="server" CssClass="label" Text="Data publikacji:"></asp:Label>
                <uc1:DateEdit ID="DateEdit1" runat="server" ValidationGroup="evg" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked="true" Text="Widoczny po dacie publikacji" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="WydrukCheckBox" runat="server" Checked="false" Text="Wydruk" />
            </div>
            <asp:Button ID="UpdateButton" runat="server" CssClass="button75" CommandName="Update" Text="Zapisz" onclick="UpdateButton_Click" ValidationGroup="evg"/>
            <asp:Button ID="CancelButton" runat="server" CssClass="button75" CommandName="Cancel" Text="Anuluj" onclick="CancelButton_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="DeleteButton" runat="server" CssClass="button75" CommandName="Delete" Text="Usuń" onclick="DeleteButton_Click"/>
        </div>            
    </div>
</div>            
