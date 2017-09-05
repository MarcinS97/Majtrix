<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPliki.ascx.cs" Inherits="HRRcp.Portal.Controls.cntPliki" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Portal/Controls/cntPlikiPliki.ascx" TagName="cntPlikiPliki" TagPrefix="uc1" %>
<%@ Register Src="~/Portal/Controls/cntPlikiEdit.ascx" TagPrefix="uc1" TagName="cntPlikiEdit" %>


<%--<asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional"><ContentTemplate>--%>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="page-title">
            <i class="glyphicon glyphicon-folder-open"></i>
            <asp:Label ID="lblTitle" runat="server" Text="Dokumenty" />
            <div id="paEditButton" runat="server" class="paEditButton pull-right" visible="false">
                <asp:LinkButton ID="btEdit" runat="server" CssClass="btn btn-primary" OnClick="btEdit_Click">
             <i class="fa fa-pencil"></i>
             Edytuj
                </asp:LinkButton>

            </div>


            <div id="paTopButtons" runat="server" class="topbuttons pull-right" visible="false">
                <asp:LinkButton ID="InsertButton" runat="server" CssClass="btn btn-default" OnClick="ShowEditModal">
            <i class="fa fa-plus"></i>
            Dodaj grupę
                </asp:LinkButton>
                <asp:Button ID="btCancelEdit" runat="server" CssClass="btn btn-default" Text="Zakończ tryb edycji" OnClick="btCancelEdit_Click" />
            </div>
        </div>

        <asp:HiddenField ID="hidMode" runat="server" Visible="false" />
        
            <div id="paPliki" runat="server" class="cntPliki container wide">
                <%--        
    InsertItemPosition='<%# GetInsertItemPosition() %>' 
                --%>
                <asp:HiddenField ID="hidGrupa" runat="server" Visible="false" />
                <asp:ListView ID="lvNaglowki" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" InsertItemPosition="None" OnDataBinding="lvNaglowki_DataBinding" OnDataBound="lvNaglowki_DataBound" OnItemCommand="lvNaglowki_ItemCommand" OnItemCreated="lvNaglowki_ItemCreated" OnItemDataBound="lvNaglowki_ItemDataBound" OnItemDeleted="lvNaglowki_ItemDeleted" OnItemDeleting="lvNaglowki_ItemDeleting" OnItemInserted="lvNaglowki_ItemInserted" OnItemInserting="lvNaglowki_ItemInserting" OnItemUpdated="lvNaglowki_ItemUpdated" OnItemUpdating="lvNaglowki_ItemUpdating">
                    <ItemTemplate>
                        <div class="grupa grupa_it">
                            <%--  <div runat="server" id="paIco" visible='<%# IsValue(Eval("Image")) %>' class="ico">
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                </div>
                            --%>
                            <div class="tekst">
                                <h4><i runat="server" class='<%# Eval("Image") %>' visible='<%# Eval("Image") != DBNull.Value %>'></i>
                                    <asp:Label ID="MenuTextLabel" runat="server" CssClass="title" Text='<%# Eval("MenuText") %>' />
                                </h4>
                                <%--<asp:Literal ID="Literal1" runat="server" Text="<br />" visible='<%# IsValue(Eval("ToolTip")) %>'></asp:Literal>--%>
                                <asp:Label ID="Label4" runat="server" CssClass="info" Text='<%# Eval("ToolTip") %>' />
                            </div>
                            <div id="paEdit" runat="server" class="buttons" visible="<%# IsEditable %>">
                                <%--<asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ToolTip="Edytuj" ImageUrl="../../images/buttons/edit.png" />
                    <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete" ToolTip="Usuń" ImageUrl="../../images/buttons/delete.png"/>--%>
                                <asp:LinkButton ID="AddFileButton" runat="server" CommandName="AddFile" CssClass="xbtn-green-small btn-small btn-success"
                                    Text="Dodaj link"><i class="fa fa-plus"></i></asp:LinkButton>
                                <asp:LinkButton ID="EditButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="xEdit" CssClass="xbtn-primary-small btn-small btn-primary" OnClick="ShowEditModal"
                                    Text="Edytuj"><i class="fa fa-pencil"></i></asp:LinkButton>
                                <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CssClass="xbtn-red-small btn-small btn-danger" Text="Usuń" Visible="true"><i class="fa fa-trash"></i></asp:LinkButton>
                            </div>
                            <uc1:cntPlikiPliki ID="cntPlikiPliki1" runat="server" Grupa="<%# Grupa %>" Mode="<%# Mode %>" ParentId='<%# Eval("Id") %>' />
                            <%-- <div id="paButtons" runat="server" class="gbuttons" visible='<%# IsEditable %>'>        
                    <div class="left">
                        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                        <asp:Label ID="Label7" runat="server" CssClass="title" Text='<%# Eval("Kolejnosc") %>' />   
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Text="Widoczna" Enabled="false" />
                    </div>
                    <asp:Button ID="AddFileButton" runat="server" CssClass="button100" CommandName="AddFile" Text="Dodaj link" />
                </div>  --%>
                        </div>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <div class="grupa grupa_eit">
                            <asp:Label ID="Label5" runat="server" CssClass="edit_info" Text="Edycja" Visible="false"></asp:Label>
                            <div class="ico">
                                <asp:Image ID="Image1" runat="server" CssClass="img" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CompleteBackColor="Transparent" CssClass="fileupload_img32" Height="32" OnUploadedComplete="FileUploadComplete" OnUploadedFileError="FileUploadError" ThrobberID="imgLoader" ToolTip="Wybierz plik" UploaderStyle="Modern" UploadingBackColor="Transparent" Width="32" />
                                <asp:Label ID="imgLoader" runat="server" CssClass="fileupload" Style="display: none;"><img alt="" src="../images/uploading.gif" /></asp:Label>
                                <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("Image") %>' />
                            </div>
                            <asp:TextBox ID="MenuTextTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='<%# Bind("MenuText") %>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="MenuTextTextBox" Display="Dynamic" ErrorMessage="&lt;br /&gt;&lt;span class='label'&gt;&nbsp;&lt;/span&gt; Pole wymagane" SetFocusOnError="True" ValidationGroup="evg"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                            <asp:TextBox ID="ToolTipTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='<%# Bind("ToolTip") %>' />
                            <div class="gbuttons">
                                <div class="left">
                                    <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                                    <asp:TextBox ID="KolejnoscTextBox" runat="server" CssClass="textbox kolejnosc" MaxLength="5" Text='<%# Bind("Kolejnosc") %>' />
                                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" FilterType="Custom" TargetControlID="KolejnoscTextBox" ValidChars="0123456789" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczna" />
                                </div>
                                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" CssClass="button75" Text="Zapisz" />
                                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" CssClass="button75" Text="Anuluj" />
                            </div>
                        </div>
                    </EditItemTemplate>
                    <%--
                    <asp:Label ID="Label1" runat="server" CssClass="label" Text="Nazwa:"></asp:Label>
                    <asp:RequiredFieldValidator ControlToValidate="MenuTextTextBox" ValidationGroup="evg" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                    <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                    <asp:TextBox ID="ToolTipTextBox" CssClass="textbox" runat="server" Text='<%# Bind("ToolTip") %>' />
                    <br />
                    <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                    <asp:TextBox ID="KolejnoscTextBox" CssClass="textbox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                    <asp:FilteredTextBoxExtender TargetControlID="KolejnoscTextBox" ValidChars="0123456789" ID="tbFilter" runat="server" Enabled="true" FilterType="Custom" />
                    <br />
                    <asp:Label ID="Label4" runat="server" CssClass="label" Text="Aktywny:"></asp:Label>
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Aktywny" />
                    --%>
                    <InsertItemTemplate>
                        <div class="grupa grupa_iit">
                            <asp:Label ID="Label1" runat="server" CssClass="insert_info" Text="Dodaj grupę"></asp:Label>
                            <div class="ico">
                                <asp:Image ID="Image1" runat="server" CssClass="img" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CompleteBackColor="Transparent" CssClass="fileupload_img32" Height="32" OnUploadedComplete="FileUploadComplete" OnUploadedFileError="FileUploadError" ThrobberID="imgLoader" ToolTip="Wybierz plik" UploaderStyle="Modern" UploadingBackColor="Transparent" Width="32" />
                                <asp:Label ID="imgLoader" runat="server" CssClass="fileupload" Style="display: none;"><img alt="" src="../images/uploading.gif" /></asp:Label>
                                <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("Image") %>' />
                            </div>
                            <asp:TextBox ID="MenuTextTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='<%# Bind("MenuText") %>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="MenuTextTextBox" Display="Dynamic" ErrorMessage="&lt;br /&gt;&lt;span class='label'&gt;&nbsp;&lt;/span&gt; Pole wymagane" SetFocusOnError="True" ValidationGroup="evg"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                            <asp:TextBox ID="ToolTipTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='<%# Bind("ToolTip") %>' />
                            <div class="gbuttons">
                                <div class="left">
                                    <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                                    <asp:TextBox ID="KolejnoscTextBox" runat="server" CssClass="textbox kolejnosc" MaxLength="5" Text='<%# Bind("Kolejnosc") %>' />
                                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" FilterType="Custom" TargetControlID="KolejnoscTextBox" ValidChars="0123456789" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczna" />
                                </div>
                                <asp:Button ID="Button1" runat="server" CommandName="Insert" CssClass="button75" Text="Dodaj" />
                                <asp:Button ID="Button2" runat="server" CommandName="Cancel" CssClass="button75" Text="Czyść" />
                                <%--
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Cancel" />
                                --%>
                            </div>
                        </div>
                    </InsertItemTemplate>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label2" runat="server" CssClass="label" Text="Brak danych"></asp:Label>
                        <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" CssClass="button" Text="Dodaj grupę" Visible="<%# IsEditable %>" />
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div id="itemPlaceholderContainer" runat="server" class="lvPliki">
                            <div id="itemPlaceholder" runat="server" />
                        </div>
                        <%--
            <div class="buttons">
                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Insert" />            
h                <asp:Button ID="btCancelEdit" runat="server" CssClass="button125" Text="Zakończ edycję" OnClick="btCancelEdit_Click"/>
            </div>
                        --%>
                    </LayoutTemplate>
                </asp:ListView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
                    DeleteCommand="DELETE FROM [SqlMenu] WHERE [Id] = @Id" 
                    InsertCommand="INSERT INTO [SqlMenu] ([Grupa], [ParentId], [MenuText], [ToolTip], [Command], [Kolejnosc], [Aktywny], [Image], [Rights]) VALUES (@Grupa, @ParentId, @MenuText, @ToolTip, @Command, @Kolejnosc, @Aktywny, @Image, @Rights)" 
                    SelectCommand="
SELECT * FROM [SqlMenu] 
WHERE ([Grupa] = @Grupa) and ParentId is null 
  and (@mode = 1 or Aktywny = 1)
ORDER BY [Kolejnosc]" 
                    UpdateCommand="UPDATE [SqlMenu] SET [Grupa] = @Grupa, [ParentId] = @ParentId, [MenuText] = @MenuText, [ToolTip] = @ToolTip, [Command] = @Command, [Kolejnosc] = @Kolejnosc, [Aktywny] = @Aktywny, [Image] = @Image, [Rights] = @Rights WHERE [Id] = @Id">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="hidMode" Name="mode" PropertyName="Value" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
                        <asp:Parameter Name="ParentId" Type="Int32" />
                        <asp:Parameter Name="MenuText" Type="String" />
                        <asp:Parameter Name="ToolTip" Type="String" />
                        <asp:Parameter Name="Command" Type="String" />
                        <asp:Parameter Name="Kolejnosc" Type="Int32" />
                        <asp:Parameter Name="Aktywny" Type="Boolean" />
                        <asp:Parameter Name="Image" Type="String" />
                        <asp:Parameter Name="Rights" Type="String" />
                        <asp:Parameter Name="Id" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
                        <asp:Parameter Name="ParentId" Type="Int32" />
                        <asp:Parameter Name="MenuText" Type="String" />
                        <asp:Parameter Name="ToolTip" Type="String" />
                        <asp:Parameter Name="Command" Type="String" />
                        <asp:Parameter Name="Kolejnosc" Type="Int32" />
                        <asp:Parameter Name="Aktywny" Type="Boolean" />
                        <asp:Parameter Name="Image" Type="String" />
                        <asp:Parameter Name="Rights" Type="String" />
                    </InsertParameters>
                </asp:SqlDataSource>
            </div>

            <h1></h1>
        </h1>
    </ContentTemplate>
    <%--<Triggers>
        <asp:AsyncPostBackTrigger ControlID="cntPlikiEdit" EventName="Saved" />
    </Triggers>--%>
</asp:UpdatePanel>

<uc1:cntPlikiEdit ID="cntPlikiEdit" runat="server" OnSaved="cntPlikiEdit_Saved" />


<%--    </ContentTemplate></asp:UpdatePanel>--%>
