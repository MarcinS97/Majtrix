<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlikiPliki.ascx.cs" Inherits="HRRcp.Controls.Portal.cntPlikiPliki" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:FileUpload ID="FileUpload1" runat="server" OnChange="doClick(this.id + '_OnSelect');" /><br />
        <asp:Button ID="Button4" runat="server" Text="Button" onclick="Button4_Click" />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="Button4"/>
    </Triggers>
</asp:UpdatePanel>
--%>


<div id="paPlikiPliki" runat="server" class="cntPlikiPliki">
    <asp:HiddenField ID="hidParentId" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
    <asp:ListView ID="lvPliki" runat="server" DataKeyNames="Id" 
        DataSourceID="SqlDataSource1" InsertItemPosition="None" 
        onitemcommand="lvPliki_ItemCommand" 
        onitemdatabound="lvPliki_ItemDataBound" 
        oniteminserted="lvPliki_ItemInserted" 
        oniteminserting="lvPliki_ItemInserting" 
        onitemupdated="lvPliki_ItemUpdated" 
        onitemupdating="lvPliki_ItemUpdating" ondatabound="lvPliki_DataBound">
        <ItemTemplate>
            <div class="plik">
                <div class="ico">
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                </div>
                <div class="link">
                    <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Eval("MenuText") %>' CommandArgument='<%# Eval("Id") %>' CommandName="Download" ToolTip='<%# Eval("ToolTip") %>'></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" Text="Drukuj" CssClass="printbtn" CommandArgument='<%# Eval("Id") %>' CommandName="Print" Visible='<%# IsPrintVisible(Eval("Wydruk")) %>'></asp:LinkButton>
                </div>            
                <div id="paEdit" runat="server" class="lnbuttons" visible='<%# IsEditable() %>'>
                    <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ToolTip="Edytuj" ImageUrl="../../images/buttons/edit.png" />
                    <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete" ToolTip="Usuń" ImageUrl="../../images/buttons/delete.png"/>
                </div>
                <div id="paEditInfo" runat="server" class="info" visible='<%# IsEditable() %>'>        
                    <asp:Label ID="Label4" runat="server" CssClass="label" Text="Plik:"></asp:Label>
                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("Command") %>'></asp:Label><br />
                    <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                    <asp:Label ID="Label9" runat="server" Text='<%# Eval("ToolTip") %>'></asp:Label><br />
                    <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                    <asp:Label ID="Label7" runat="server" CssClass="kolejnosc" Text='<%# Eval("Kolejnosc") %>' />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Text="Widoczny" Enabled="false" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Eval("Wydruk") %>' Text="Wydruk" Enabled="false" Visible='<%# IsWydruk %>'/>
                </div>            
            </div>
        </ItemTemplate>
        <EditItemTemplate>
            <div class="plik plik_eit">
                <asp:Label ID="Label5" runat="server" CssClass="edit_info" Text="Edycja linku do pliku" Visible="false"></asp:Label>

                <asp:Label ID="Label4" runat="server" CssClass="label" Text="Nazwa wyświetlana:"></asp:Label>
                <asp:TextBox ID="FileNameTextBox" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("MenuText") %>' />
                <asp:RequiredFieldValidator ControlToValidate="FileNameTextBox" ValidationGroup="evg" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                <br />

                <div class="plik_line">    
                    <div class="label">  
                        <div class="ico">
                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                        </div>
                        <asp:Label ID="Label6" runat="server" Text="Bieżący plik:"></asp:Label>
                    </div>

                    <asp:TextBox ID="CommandTextBox" CssClass="textbox command" MaxLength="500" runat="server" Text='<%# Bind("Command") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="CommandTextBox" ValidationGroup="evg" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                    <br />


                    <span class="label"></span>                
                    <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="fileupload" 
                        ToolTip="Wybierz plik" 
                        UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                        UploaderStyle="Modern" ThrobberID="imgLoader"                         
                        OnUploadedComplete="FileUploadComplete" 
                        OnUploadedFileError="FileUploadError" 
                        />
                        
                        <%--
                    <div id="paUploadImg" runat="server" Visible='<%# IsWydruk %>'>
                        <span class="label"></span>
                        <div style="display: inline"><asp:Label Text="0" ID="LLPagesCount" runat="server" /></div><br />
                        <span class="label"></span>
                        <asp:AsyncFileUpload ID="AsyncFileUpload2" runat="server" CssClass="fileupload" 
                            ToolTip="Dodaj kolejny plik" 
                            UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                            UploaderStyle="Modern" ThrobberID="imgLoader"
                            OnClientUploadComplete="clearAFUContents"
                            OnUploadedComplete="UploadFileNewPage"
                            OnUploadedFileError="FileUploadError" 
                            />
                        <span class="label"></span>
                        <div style="display: inline"><asp:Button OnClientClick="javascript:return confirm('Czy chcesz usunac wszystkie strony?');" OnClick="DeleteAllPages" ID="ClearImgs" runat="server" CssClass="button" Text="Usuń wszystkie" /></div><br />
                    </div>        
                        --%>
                        
                    <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                    <asp:HiddenField ID="hidImage" runat="server" Value='<%# Bind("Image") %>'/>
                    <asp:HiddenField ID="hidPar1" runat="server" Value='<%# Bind("Par1") %>'/>
                    <asp:HiddenField ID="hidId" runat="server" Value='<%# Bind("Id") %>'/>
                </div>
                <br />
<%--
                    <asp:Label ID="Label10" runat="server" Text='<%# Eval("Command") %>'></asp:Label><br />
                    <asp:HiddenField ID="hidCommand" runat="server" Value='<%# Bind("Command") %>'/>
--%>

                <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                <asp:TextBox ID="TextBox3" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("ToolTip") %>' />

                <div class="gbuttons">        
                    <div class="left">
                        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                        <asp:TextBox ID="KolejnoscTextBox" CssClass="textbox kolejnosc" MaxLength="5" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                        <asp:FilteredTextBoxExtender TargetControlID="KolejnoscTextBox" ValidChars="0123456789" ID="tbFilter" runat="server" Enabled="true" FilterType="Custom" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczny" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="cbWydruk" runat="server" Checked='<%# Bind("Wydruk") %>' Text="Wydruk" Visible='<%# IsWydruk %>'/>
                    </div>
                    <asp:Button ID="Button1" runat="server" CssClass="button75" CommandName="Update" Text="Zapisz" />
                    <asp:Button ID="Button2" runat="server" CssClass="button75" CommandName="Cancel" Text="Anuluj" />
                </div>            
            </div>
        </EditItemTemplate>

        <InsertItemTemplate>
            <div class="plik plik_eit plik_iit">
                <asp:Label ID="Label5" runat="server" CssClass="insert_info" Text="Dodaj plik"></asp:Label>

                <div class="plik_line">    
                    <div class="label">  
                        <div class="ico">
                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                        </div>
                        <asp:Label ID="Label6" runat="server" Text="Plik:"></asp:Label>
                    </div>

                    <asp:TextBox ID="CommandTextBox" CssClass="textbox command" MaxLength="500" runat="server" Text='<%# Bind("Command") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="CommandTextBox" ValidationGroup="ivg" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                    <br />

                    <span class="label"></span>                
                    <asp:AsyncFileUpload ID="AsyncFileUpload1" runat="server" CssClass="fileupload" 
                        ToolTip="Wybierz plik" 
                        UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                        UploaderStyle="Modern" ThrobberID="imgLoader"                         
                        OnUploadedComplete="FileUploadComplete" 
                        OnUploadedFileError="FileUploadError" 
                        />
                    <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                    <asp:HiddenField ID="hidImage" runat="server" Value='<%# Bind("Image") %>'/>
                    <asp:HiddenField ID="hidPar1" runat="server" Value='<%# Bind("Par1") %>'/>
                    <asp:HiddenField ID="hidId" runat="server" Value='<%# Bind("Id") %>'/>
                </div>
                <br />


<%--
                    <asp:HiddenField ID="hidCommand" runat="server" Value='<%# Bind("Command") %>'/>
--%>
                <asp:Label ID="Label4" runat="server" CssClass="label" Text="Nazwa wyświetlana:"></asp:Label>
                <asp:TextBox ID="FileNameTextBox" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("MenuText") %>' />
                <asp:RequiredFieldValidator ControlToValidate="FileNameTextBox" ValidationGroup="ivg" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                <br />


<%--
                <asp:TextBox ID="CommandTextBox" CssClass="textbox" MaxLength="500" runat="server" Text='<%# Bind("Command") %>' />
                <asp:RequiredFieldValidator ControlToValidate="CommandTextBox" ValidationGroup="evg" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ErrorMessage="<br /><span class='label'>&nbsp;</span> Pole wymagane" ></asp:RequiredFieldValidator>
                <br />
--%>

                <asp:Label ID="Label2" runat="server" CssClass="label" Text="Informacja:"></asp:Label>
                <asp:TextBox ID="TextBox3" CssClass="textbox" MaxLength="200" runat="server" Text='<%# Bind("ToolTip") %>' />

                <div class="gbuttons">        
                    <div class="left">
                        <asp:Label ID="Label3" runat="server" CssClass="label" Text="Kolejność:"></asp:Label>
                        <asp:TextBox ID="KolejnoscTextBox" CssClass="textbox kolejnosc" MaxLength="5" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                        <asp:FilteredTextBoxExtender TargetControlID="KolejnoscTextBox" ValidChars="0123456789" ID="tbFilter" runat="server" Enabled="true" FilterType="Custom" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczny" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="cbWydruk" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Wydruk" Visible='<%# IsWydruk %>'/>
                    </div>
                    <asp:Button ID="InsertButton" runat="server" CssClass="button75" CommandName="Insert" Text="Zapisz" />
                    <asp:Button ID="CancelButton" runat="server" CssClass="button75" CommandName="CancelInsert" Text="Anuluj" />
                </div>            
            </div>
        </InsertItemTemplate>
        <EmptyDataTemplate>
<%--
            <asp:Label ID="Label2" runat="server" CssClass="label" Text="Brak danych"></asp:Label>
            <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj plik" />
--%>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div ID="itemPlaceholderContainer" runat="server" style="">
                <div ID="itemPlaceholder" runat="server" />
            </div>
           
<%--
            <div style="">
                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj plik" />            
            </div>
--%>            
        </LayoutTemplate>
    </asp:ListView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
        DeleteCommand="DELETE FROM [SqlMenu] WHERE [Id] = @Id" 
        InsertCommand="
INSERT INTO [SqlMenu] ([Grupa], [ParentId], [MenuText], [ToolTip], [Command], [Kolejnosc], [Aktywny], [Image], [Rights], Par1, Par2) VALUES (@Grupa, @ParentId, @MenuText, @ToolTip, @Command, @Kolejnosc, @Aktywny, @Image, @Rights, @Par1, case when @Wydruk = 1 then 1 else 0 end)
set @Id = (select @@Identity)
        " 
        SelectCommand="SELECT *, cast(case ISNULL(Par2,'0') when '1' then 1 else 0 end as bit) as Wydruk FROM [SqlMenu] WHERE ([ParentId] = @ParentId) ORDER BY [Kolejnosc]" 
        UpdateCommand="UPDATE [SqlMenu] SET [MenuText] = @MenuText, [ToolTip] = @ToolTip, [Command] = @Command, [Kolejnosc] = @Kolejnosc, [Aktywny] = @Aktywny, [Image] = @Image, [Rights] = @Rights, Par1 = @Par1, Par2 = case when @Wydruk = 1 then 1 else 0 end WHERE [Id] = @Id" 
            oninserted="SqlDataSource1_Inserted">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidParentId" Name="ParentId" PropertyName="Value" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="MenuText" Type="String" />
            <asp:Parameter Name="ToolTip" Type="String" />
            <asp:Parameter Name="Command" Type="String" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="Image" Type="String" />
            <asp:Parameter Name="Rights" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
            <asp:Parameter Name="Par1" Type="String" />
            <asp:Parameter Name="Wydruk" Type="Boolean" />
        </UpdateParameters>
        <InsertParameters>
            <asp:ControlParameter ControlID="hidParentId" Name="ParentId" PropertyName="Value" Type="Int32" />
            <asp:Parameter Name="Grupa" Type="String" />
            <asp:Parameter Name="MenuText" Type="String" />
            <asp:Parameter Name="ToolTip" Type="String" />
            <asp:Parameter Name="Command" Type="String" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="Image" Type="String" />
            <asp:Parameter Name="Rights" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" Direction="Output" DefaultValue="0"/>
            <asp:Parameter Name="Par1" Type="String" />
            <asp:Parameter Name="Wydruk" Type="Boolean" />
        </InsertParameters>
    </asp:SqlDataSource>
</div>
