<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="test1.aspx.cs" Inherits="HRRcp.Portal.test1" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    
        <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
            DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
            <ItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                            Text="Delete" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    </td>
                    <td>
                        <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:Label ID="TekstLabel" runat="server" Text='<%# Eval("Tekst") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                            Text="Delete" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    </td>
                    <td>
                        <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:Label ID="TekstLabel" runat="server" Text='<%# Eval("Tekst") %>' />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" style="">
                    <tr>
                        <td>
                            No data was returned.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                            Text="Insert" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                            Text="Clear" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:TextBox ID="TekstTextBox" runat="server" Text='<%# Bind("Tekst") %>' TextMode="MultiLine" Rows="20" Columns="150" />
                        <asp:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" 
                            EnableSanitization="false" TargetControlID="TekstTextBox" 
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
                    </td>
                </tr>
            </InsertItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr runat="server" style="">
                                    <th runat="server">
                                    </th>
                                    <th runat="server">
                                        Id</th>
                                    <th runat="server">
                                        Tekst</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="">
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <EditItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                            Text="Update" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                            Text="Cancel" />
                    </td>
                    <td>
                        <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="TekstTextBox" runat="server" Text='<%# Bind("Tekst") %>' />
                    </td>
                </tr>
            </EditItemTemplate>
            <SelectedItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                            Text="Delete" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    </td>
                    <td>
                        <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:Label ID="TekstLabel" runat="server" Text='<%# Eval("Tekst") %>' />
                    </td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
    
    
    
    
    
    
    
    
    
    
    
    
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
            DeleteCommand="DELETE FROM [Teksty] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Teksty] ([Tekst]) VALUES (@Tekst)" 
            SelectCommand="SELECT [Id], [Tekst] FROM [Teksty]" 
            UpdateCommand="UPDATE [Teksty] SET [Tekst] = @Tekst WHERE [Id] = @Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Tekst" Type="String" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Tekst" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
    
    
    
    
    
    
    
    
    
    
    
    
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
