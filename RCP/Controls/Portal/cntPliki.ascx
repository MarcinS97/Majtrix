<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPliki.ascx.cs" Inherits="HRRcp.Controls.Portal.cntPliki" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="cntPlikiPliki.ascx" tagname="cntPlikiPliki" tagprefix="uc1" %>

<div id="paPliki" runat="server" class="cntPliki">
    <div id="paEditButton" runat="server" class="paEditButton" visible="false" >
        <asp:Button ID="btEdit" runat="server" CssClass="button75" Text="Edycja" onclick="btEdit_Click" />
    </div>

<%--        
    InsertItemPosition='<%# GetInsertItemPosition() %>' 
--%>

    <asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
    <asp:ListView ID="lvNaglowki" runat="server" DataKeyNames="Id" 
        DataSourceID="SqlDataSource1" 
        InsertItemPosition="None"
        ondatabinding="lvNaglowki_DataBinding" ondatabound="lvNaglowki_DataBound" 
        onitemcommand="lvNaglowki_ItemCommand" 
        onitemdatabound="lvNaglowki_ItemDataBound" 
        onitemdeleted="lvNaglowki_ItemDeleted" onitemdeleting="lvNaglowki_ItemDeleting" 
        oniteminserted="lvNaglowki_ItemInserted" 
        oniteminserting="lvNaglowki_ItemInserting" 
        onitemupdated="lvNaglowki_ItemUpdated" 
        onitemupdating="lvNaglowki_ItemUpdating" onitemcreated="lvNaglowki_ItemCreated">
        <ItemTemplate>
            <div class="grupa grupa_it">
                <div runat="server" id="paIco" visible='<%# IsValue(Eval("Image")) %>' class="ico">
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                </div>

<%--
                <asp:Label ID="MenuTextLabel" runat="server" CssClass="title" Text='<%# Eval("MenuText") %>' ToolTip='<%# Eval("ToolTip") %>'/>   
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
--%>
                <div class="tekst">
                    <asp:Label ID="MenuTextLabel" runat="server" CssClass="title" Text='<%# Eval("MenuText") %>' />
                    <asp:Literal ID="Literal1" runat="server" Text="<br />" visible='<%# IsValue(Eval("ToolTip")) %>'></asp:Literal>
                    <asp:Label ID="Label4" runat="server" CssClass="info" Text='<%# Eval("ToolTip") %>' />   
                </div>
                <div id="paEdit" runat="server" class="lnbuttons" visible='<%# IsEditable %>'>
                    <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ToolTip="Edytuj" ImageUrl="../../images/buttons/edit.png" />
                    <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete" ToolTip="Usuń" ImageUrl="../../images/buttons/delete.png"/>
                </div>
                <uc1:cntPlikiPliki ID="cntPlikiPliki1" runat="server" ParentId='<%# Eval("Id") %>' Grupa='<%# Grupa %>' Mode='<%# Mode %>'/>    
                <div id="paButtons" runat="server" class="gbuttons" visible='<%# IsEditable %>'>        
                    <div class="left">
                        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                        <asp:Label ID="Label7" runat="server" CssClass="title" Text='<%# Eval("Kolejnosc") %>' />   
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Text="Widoczna" Enabled="false" />
                    </div>
                    <asp:Button ID="AddFileButton" runat="server" CssClass="button100" CommandName="AddFile" Text="Dodaj link" />
                </div>            
            </div>
        </ItemTemplate>
        <EditItemTemplate>
            <div class="grupa grupa_eit">
                <asp:Label ID="Label5" runat="server" CssClass="edit_info" Text="Edycja" Visible="false"></asp:Label>
                <div class="ico">
                    <asp:Image ID="Image1" runat="server" CssClass="img" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                    <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="fileupload_img32" 
                        ToolTip="Wybierz plik" Width="32" Height="32" 
                        UploadingBackColor="Transparent" 
                        CompleteBackColor="Transparent" 
                        UploaderStyle="Modern" ThrobberID="imgLoader"                         
                        OnUploadedComplete="FileUploadComplete" 
                        OnUploadedFileError="FileUploadError" 
                        />
                    <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                    <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("Image") %>'/>
                </div>

                <asp:TextBox ID="MenuTextTextBox" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("MenuText") %>' />
                <asp:RequiredFieldValidator ControlToValidate="MenuTextTextBox" ValidationGroup="evg" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                <br />

                <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                <asp:TextBox ID="ToolTipTextBox" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("ToolTip") %>' />

                <div class="gbuttons">        
                    <div class="left">
                        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                        <asp:TextBox ID="KolejnoscTextBox" CssClass="textbox kolejnosc" MaxLength="5" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                        <asp:FilteredTextBoxExtender TargetControlID="KolejnoscTextBox" ValidChars="0123456789" ID="tbFilter" runat="server" Enabled="true" FilterType="Custom" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczna" />
                    </div>
                    <asp:Button ID="UpdateButton" runat="server" CssClass="button75" CommandName="Update" Text="Zapisz" />
                    <asp:Button ID="CancelButton" runat="server" CssClass="button75" CommandName="Cancel" Text="Anuluj" />
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
                    <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="fileupload_img32" 
                        ToolTip="Wybierz plik" Width="32" Height="32" 
                        UploadingBackColor="Transparent" 
                        CompleteBackColor="Transparent" 
                        UploaderStyle="Modern" ThrobberID="imgLoader"                         
                        OnUploadedComplete="FileUploadComplete" 
                        OnUploadedFileError="FileUploadError" 
                        />
                    <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                    <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("Image") %>'/>
                </div>

                <asp:TextBox ID="MenuTextTextBox" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("MenuText") %>' />
                <asp:RequiredFieldValidator ControlToValidate="MenuTextTextBox" ValidationGroup="evg" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                <br />

                <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                <asp:TextBox ID="ToolTipTextBox" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("ToolTip") %>' />

                <div class="gbuttons">        
                    <div class="left">
                        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                        <asp:TextBox ID="KolejnoscTextBox" CssClass="textbox kolejnosc" MaxLength="5" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                        <asp:FilteredTextBoxExtender TargetControlID="KolejnoscTextBox" ValidChars="0123456789" ID="tbFilter" runat="server" Enabled="true" FilterType="Custom" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczna" />
                    </div>
                    <asp:Button ID="Button1" runat="server" CssClass="button75" CommandName="Insert" Text="Dodaj" />
                    <asp:Button ID="Button2" runat="server" CssClass="button75" CommandName="Cancel" Text="Czyść" />
    <%--
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Cancel" />
    --%>
                </div>            
            </div>            
        </InsertItemTemplate>
        
        
        
        <EmptyDataTemplate>
            <asp:Label ID="Label2" runat="server" CssClass="label" Text="Brak danych"></asp:Label>
            <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" CssClass="button" Text="Dodaj grupę" Visible='<%# IsEditable %>'/>
        </EmptyDataTemplate>
        
        
        
        <LayoutTemplate>
            <div ID="itemPlaceholderContainer" runat="server" class="lvPliki">
                <div id="paTopButtons" runat="server" class="topbuttons" visible="false" >
                    <asp:Button ID="InsertButton" runat="server" CssClass="button" CommandName="NewRecord" Text="Dodaj grupę" />
                    <asp:Button ID="btCancelEdit" runat="server" CssClass="button" Text="Zakończ tryb edycji" OnClick="btCancelEdit_Click"/>
                </div>
                <div ID="itemPlaceholder" runat="server" />
            </div>

<%--
            <div class="buttons">
                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Insert" />            
h                <asp:Button ID="btCancelEdit" runat="server" CssClass="button125" Text="Zakończ edycję" OnClick="btCancelEdit_Click"/>
            </div>
--%>
        </LayoutTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
        DeleteCommand="DELETE FROM [SqlMenu] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [SqlMenu] ([Grupa], [ParentId], [MenuText], [ToolTip], [Command], [Kolejnosc], [Aktywny], [Image], [Rights]) VALUES (@Grupa, @ParentId, @MenuText, @ToolTip, @Command, @Kolejnosc, @Aktywny, @Image, @Rights)" 
        SelectCommand="SELECT * FROM [SqlMenu] WHERE ([Grupa] = @Grupa) and ParentId is null ORDER BY [Kolejnosc]" 
        UpdateCommand="UPDATE [SqlMenu] SET [Grupa] = @Grupa, [ParentId] = @ParentId, [MenuText] = @MenuText, [ToolTip] = @ToolTip, [Command] = @Command, [Kolejnosc] = @Kolejnosc, [Aktywny] = @Aktywny, [Image] = @Image, [Rights] = @Rights WHERE [Id] = @Id">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
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