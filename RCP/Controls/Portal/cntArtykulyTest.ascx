<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntArtykulyTest.ascx.cs" Inherits="HRRcp.Controls.Portal.cntArtykulyTest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<%-- bez tego nie wyświetla buttonów na toolbarze --%>
<div style="xdisplay: none;">
    <asp:TextBox runat="server" ID="tbArtykulGhost" TextMode="MultiLine" Columns="150" Rows="20" />
    <asp:HtmlEditorExtender ID="HtmlEditorExtender1" 
        TargetControlID="tbArtykulGhost"
        EnableSanitization="false"
        runat="server" 
        DisplaySourceTab="false" 
        OnImageUploadComplete="HtmlEditorExtender1_ImageUploadComplete">
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
</div>



        <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
            DataSourceID="SqlDataSource1" InsertItemPosition="None">
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
                <table id="Table1" runat="server" style="">
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
                <table id="Table2" runat="server">
                    <tr id="Tr1" runat="server">
                        <td id="Td1" runat="server">
                            <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr id="Tr2" runat="server" style="">
                                    <th id="Th1" runat="server">
                                    </th>
                                    <th id="Th2" runat="server">
                                        Id</th>
                                    <th id="Th3" runat="server">
                                        Tekst</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="Tr3" runat="server">
                        <td id="Td2" runat="server" style="">
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
    
    
    
    
    
    
