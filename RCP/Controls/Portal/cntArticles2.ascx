<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntArticles2.ascx.cs" Inherits="HRRcp.Controls.Portal.cntArticles2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
<asp:HiddenField ID="hidEdit" runat="server" Visible="false"/>

<%--
    <script type="text/jscript">
        function editorChanged() {
            alert('contents changed');
        }
    </script>
--%>

<div class="cntArticles">
    <div id="paEditButton" runat="server" class="paEditButton" visible="false" >
        <asp:Button ID="btEdit" runat="server" CssClass="button" Text="Edycja" onclick="btEdit_Click" />
    </div>

<asp:ListView ID="lvArtykuly" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" 
        onitemdatabound="lvArtykuly_ItemDataBound" 
        oniteminserting="lvArtykuly_ItemInserting" 
        onitemupdating="lvArtykuly_ItemUpdating" 
        onitemcreated="lvArtykuly_ItemCreated" 
        ondatabound="lvArtykuly_DataBound" 
        onitemcanceling="lvArtykuly_ItemCanceling" 
        onitemcommand="lvArtykuly_ItemCommand" 
        onitemdeleted="lvArtykuly_ItemDeleted" 
        onitemediting="lvArtykuly_ItemEditing" 
        oniteminserted="lvArtykuly_ItemInserted" 
        onitemupdated="lvArtykuly_ItemUpdated">
    <ItemTemplate>
        <div class="article">
            <div class="art">
                <div class="image">
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>'/>
                </div>
                <asp:Literal ID="Literal1" runat="server" ></asp:Literal>
            </div>
            <div id="paEdit" runat="server" class="edit" visible='<%# IsEditable %>'>
                <div class="buttons">
                    <div class="left">
                        <asp:Label ID="Label1" CssClass="label" runat="server" Text="Data publikacji:" />
                        <asp:Label ID="Label2" CssClass="value" runat="server" Text='<%# Eval("DataPublikacji", "{0:d}") %>' />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Widoczny") %>' Text="Widoczny" Enabled="false" />
                    </div>
                    <asp:Button ID="EditButton" runat="server" CssClass="button75" CommandName="Edit" Text="Edytuj" />
                    <asp:Button ID="DeleteButton" runat="server" CssClass="button75" CommandName="Delete" Text="Usuń" Visible="false"/>
                </div>
            </div>    
        </div>
    </ItemTemplate>



    <SelectedItemTemplate>
        <div class="article">
            <div class="image">
                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>'/>
            </div>
            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("Tekst") %>'></asp:Literal>
            <div id="paEdit" runat="server" class="edit" visible='<%# IsEditable %>'>
                <asp:Label ID="Label1" CssClass="label" runat="server" Text="Data publikacji" />
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("DataPublikacji") %>' />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Text="Widoczny" Enabled="false" />
                <div class="buttons">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                </div>
            </div>    
        </div>
    </SelectedItemTemplate>



    <EmptyDataTemplate>
        <asp:Button ID="InsertButton" runat="server" CssClass="button100" CommandName="NewRecord" Text="Dodaj artykuł" />        
    </EmptyDataTemplate>
    


    <EditItemTemplate>
        <div class="grupa grupa_eit">
            <asp:Label ID="Label5" runat="server" CssClass="edit_info" Text="Edycja"></asp:Label>
<%--
            <div class="image">
                <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="fileupload" 
                    ToolTip="Wybierz plik" 
                    UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                    UploaderStyle="Modern" ThrobberID="imgLoader"                         
                    OnUploadedComplete="FileUploadComplete" 
                    OnUploadedFileError="FileUploadError" 
                    />
                <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("Image") %>'/>
            </div>
--%>

<%--            
            <asp:TextBox runat="server" ID="tbArtykul" TextMode="MultiLine" Columns="150" Rows="40" Text='<%# Bind("Tekst") %>' /><br />               
--%>


            <cc1:Editor ID="Tekst1Editor" runat="server" NoUnicode="True" CssClass="editor" />



<%--
            <asp:TextBox runat="server" ID="tbArtykul" TextMode="MultiLine" Columns="150" Rows="40" /><br />               

            <asp:HtmlEditorExtender ID="htmlEditorExtender2" 
                TargetControlID="tbArtykul"
                EnableSanitization="false"
                runat="server" 
                DisplaySourceTab="True" 
                OnImageUploadComplete="htmlEditorExtender2_OnImageUploadComplete">
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
            <div class="info">
                <span>Uwagi</span>                
                W widoku HTML, do tagu <b>&lt;img&gt;</b> dodaj: style="float: left;" lub style="float: right; - opływanie tekstu"
            </div>
            <div class="buttons">        
                <div class="left">
                    <asp:Label ID="Label4" runat="server" CssClass="label" Text="Data publikacji:"></asp:Label>
                    <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataPublikacji") %>'/>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Widoczny") %>' Text="Widoczny" />
                </div>
                <asp:Button ID="DeleteButton" runat="server" CssClass="button75" CommandName="Delete" Text="Usuń" Visible="false"/>
                <asp:Button ID="UpdateButton" runat="server" CssClass="button75" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CssClass="button75" CommandName="Cancel" Text="Anuluj" />
            </div>            
        </div>            
    </EditItemTemplate>



    <InsertItemTemplate>
        <div class="grupa grupa_eit">
            <asp:Label ID="Label5" runat="server" CssClass="insert_info" Text="Nowy artykuł"></asp:Label>
            <div class="image" runat="server" visible="false">
                <asp:Image ID="Image1" runat="server" CssClass="img" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="fileupload" 
                    ToolTip="Wybierz plik" 
                    UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                    UploaderStyle="Modern" ThrobberID="imgLoader"                         
                    OnUploadedComplete="FileUploadComplete" 
                    OnUploadedFileError="FileUploadError" 
                    />
                <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("Image") %>'/>
            </div>

            <%--
                OnClientChange="editorChanged"
            --%>

            <cc1:Editor ID="Tekst1Editor" runat="server" NoUnicode="True" CssClass="editor" />


<%--
            <asp:TextBox runat="server" ID="tbArtykul" TextMode="MultiLine" Columns="150" Rows="40" Text='<%# Bind("Tekst") %>' /><br />                
            <asp:HtmlEditorExtender ID="htmlEditorExtender2" 
                TargetControlID="tbArtykul"
                EnableSanitization="false"
                runat="server" 
                DisplaySourceTab="True" 
                OnImageUploadComplete="htmlEditorExtender2_OnImageUploadComplete" >
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
            <div class="info">
                <span>Uwagi</span>                
                W widoku HTML, do tagu <b>&lt;img&gt;</b> dodaj: style="float: left;" lub style="float: right; - opływanie tekstu"
            </div>
            <div class="buttons">        
                <div class="left">
                    <asp:Label ID="Label4" runat="server" CssClass="label" Text="Data publikacji:"></asp:Label>
                    <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataPublikacji") %>'/>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Widoczny") %>' Text="Widoczny" />
                </div>
                <asp:Button ID="InsertButton" runat="server" CssClass="button75" CommandName="Insert" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CssClass="button75" CommandName="CancelInsert" Text="Anuluj" />
            </div>            
        </div>            
    </InsertItemTemplate>



    <LayoutTemplate>
        <div ID="itemPlaceholderContainer" runat="server" class="list">
            <div id="paTopButtons" runat="server" class="topbuttons" visible="false" >
                <div class="left">
                    <asp:Button ID="btCancelEdit" runat="server" CssClass="button" Text="Zakończ tryb edycji" OnClick="btCancelEdit_Click"/>
                </div>
                <asp:Button ID="InsertButton" runat="server" CssClass="button125" CommandName="NewRecord" Text="Dodaj artykuł" />
            </div>
            <span ID="itemPlaceholder" runat="server" />
        </div>
        <div class="pager">
            <asp:DataPager ID="DataPager1" runat="server" PageSize="3">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                    <asp:NumericPagerField ButtonType="Link" />
                    <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                </Fields>
            </asp:DataPager>
        </div>
    </LayoutTemplate>
</asp:ListView>



<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
    oninserted="SqlDataSource1_Inserted"
    SelectCommand="SELECT * FROM [Teksty] where Grupa = @Grupa and (@edit = 1 or Widoczny = 1) order by DataPublikacji desc" 
    DeleteCommand="DELETE FROM [Teksty] WHERE [Id] = @Id" 
    InsertCommand="
    --set @Typ = 'ART' + convert(varchar, (select count(*) + 1 from Teksty where Grupa = @Grupa))      
    set @Typ = 'ART' + convert(varchar, (select count(*) + 1 from Teksty))      
    set @Opis = ''
    INSERT INTO [Teksty] ([Typ], [Opis], [Tekst], Grupa, Widoczny, IdAutora, DataPublikacji, Image) 
                   VALUES (@Typ, @Opis, @Tekst, @Grupa, @Widoczny, @IdAutora, @DataPublikacji, @Image)" 
    UpdateCommand="UPDATE [Teksty] SET [Tekst] = @Tekst, Widoczny = @Widoczny, IdAutora = @IdAutora, DataPublikacji = @DataPublikacji
                   WHERE [Id] = @Id" 
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidEdit" Name="edit" PropertyName="Value" Type="Boolean" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Tekst" Type="String" />
        <asp:Parameter Name="Typ" Type="String" />
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
        <asp:Parameter Name="Widoczny" Type="Boolean" />
        <asp:Parameter Name="IdAutora" Type="Int32" />
        <asp:Parameter Name="DataPublikacji" Type="DateTime" />
        <asp:Parameter Name="Image" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Tekst" Type="String" />
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
        <asp:Parameter Name="Widoczny" Type="Boolean" />
        <asp:Parameter Name="IdAutora" Type="Int32" />
        <asp:Parameter Name="DataPublikacji" Type="DateTime" />
        <asp:Parameter Name="Image" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<%-- bez tego nie wyświetla buttonów na toolbarze --%>
<%--<div style="display: none;">
    <asp:TextBox runat="server" ID="tbArtykulGhost" TextMode="MultiLine" Columns="150" Rows="40" />
    <asp:HtmlEditorExtender ID="htmlEditorExtender2" 
        TargetControlID="tbArtykulGhost"
        EnableSanitization="false"
        runat="server" 
        DisplaySourceTab="false" 
        OnImageUploadComplete="htmlEditorExtender2_OnImageUploadComplete">
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
--%>

</div>









<%--









        <div class="demoarea">
            <asp:UpdatePanel ID="updatePanel1" runat="server">                    
                <ContentTemplate>                                
                    <asp:TextBox runat="server" ID="txtBox1" TextMode="MultiLine" Columns="150" Rows="20" Text='<%# Bind("Tekst") %>' /><br />
                    
                    <ajaxToolkit:HtmlEditorExtender ID="htmlEditorExtender1" TargetControlID="txtBox1" runat="server" DisplaySourceTab="true" 
                        EnableSanitization="false"
                        >
                    </ajaxToolkit:HtmlEditorExtender>
                    <br />
                    <br />

                    <b>HtmlEditorExtender with a custom toolbar which includes support for uploading an image:</b>
                    <br />                
                    <asp:TextBox runat="server" ID="txtBox2" TextMode="MultiLine" Columns="150" Rows="20"
                        Text="Hello <b>world!</b>" /><br />
                        
                    <ajaxToolkit:HtmlEditorExtender ID="htmlEditorExtender2" TargetControlID="txtBox2"
                        EnableSanitization="false"
                        runat="server" 
                        DisplaySourceTab="True" 
                        OnImageUploadComplete="ajaxFileUpload_OnUploadComplete">
                        <Toolbar>                        
                            <ajaxToolkit:Bold />
                            <ajaxToolkit:Italic />
                            <ajaxToolkit:Underline />
                            <ajaxToolkit:HorizontalSeparator />
                            <ajaxToolkit:JustifyLeft />
                            <ajaxToolkit:JustifyCenter />
                            <ajaxToolkit:JustifyRight />
                            <ajaxToolkit:JustifyFull />
                            <ajaxToolkit:CreateLink />
                            <ajaxToolkit:UnLink />
                            <ajaxToolkit:InsertImage />                        
                        </Toolbar>
                    </ajaxToolkit:HtmlEditorExtender>
                    <br />
                    <br />
                    <asp:Button runat="server" Text="Submit content" ID="submit" OnClick="btnsubmit_click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>










--%>





<%--        <asp:HtmlEditorExtender ID="HtmlEditorExtender1" 
            EnableSanitization="false"
            TargetControlID="TextBox1" DisplaySourceTab="true" 
        OnClientChange="onContentsChange" 
            runat="server"/>
            <Toolbar> 
                <asp:Undo />
                <asp:Redo />
                <asp:Bold />
                <asp:Italic />
                <asp:Underline />
                <asp:StrikeThrough />
                <asp:Subscript />
                <asp:Superscript />
                <asp:JustifyLeft />
                <asp:JustifyCenter />
                <asp:JustifyRight />
                <asp:JustifyFull />
                <asp:InsertOrderedList />
                <asp:InsertUnorderedList />
                <asp:CreateLink />
                <asp:UnLink />
                <asp:RemoveFormat />
                <asp:SelectAll />
                <asp:UnSelect />
                <asp:Delete />
                <asp:Cut />
                <asp:Copy />
                <asp:Paste />
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
        </ajaxToolkit:HtmlEditorExtender>
--%>