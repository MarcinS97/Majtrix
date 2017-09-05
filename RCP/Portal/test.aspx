<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="HRRcp.Portal.test" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../Controls/Portal/cntArtykulyTest.ascx" tagname="cntArtykulyTest" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <hr />
        <uc2:cntArtykulyTest ID="cntArtykulyTest1" runat="server" />
        <hr />
        
        
        
        
        <%--
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Rows="50" Columns="80"></asp:TextBox>
        <asp:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" 
            EnableSanitization="false" TargetControlID="TextBox1" 
            onimageuploadcomplete="HtmlEditorExtender1_ImageUploadComplete" 
            DisplaySourceTab="True">
            <Toolbar>                        
                <asp:Undo />
                <asp:Redo />
                <asp:HorizontalSeparator />
                <asp:Bold />
                <asp:Italic />
                <asp:Underline />
                <asp:StrikeThrough />
                <asp:Subscript />
                <asp:Superscript />
                <asp:HorizontalSeparator />
                <asp:JustifyLeft />
                <asp:JustifyCenter />
                <asp:JustifyRight />
                <asp:JustifyFull />
                <asp:HorizontalSeparator />
                <asp:InsertOrderedList />
                <asp:InsertUnorderedList />
                <asp:CreateLink />
                <asp:UnLink />
                <asp:RemoveFormat />
                <asp:HorizontalSeparator />
                <asp:SelectAll />
                <asp:UnSelect />
                <asp:Delete />
                <asp:Cut />
                <asp:Copy />
                <asp:Paste />
                <asp:HorizontalSeparator />
                <asp:BackgroundColorSelector />
                <asp:ForeColorSelector />
                <asp:FontNameSelector />
                <asp:FontSizeSelector />
                <asp:Indent />
                <asp:Outdent />
                <asp:InsertHorizontalRule />
                <asp:HorizontalSeparator />
                <asp:InsertImage />
            </Toolbar>        
        </asp:HtmlEditorExtender>
        --%>

        <asp:Button ID="Button1" runat="server" Text="Postback" 
            onclick="Button1_Click" />
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
