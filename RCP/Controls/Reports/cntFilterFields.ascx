<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntFilterFields.ascx.cs" Inherits="HRRcp.Controls.Reports.cntFilterFields" %>
<%@ Register src="~/Controls/Reports/cntField.ascx" tagname="cntField" tagprefix="uc2" %>
<%@ Register src="~/Controls/Reports/cntFieldParams.ascx" tagname="cntFieldParams" tagprefix="uc2" %>

<div class="cntFilterFields">
    <asp:HiddenField ID="hidRepId" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidColumn" runat="server" Visible="false"/> 
    <asp:HiddenField ID="hidEditMode" runat="server" Visible="false"/> 
    <asp:Button ID="btCancelEdit" runat="server" CssClass="button_postback" OnClick="btCancelEdit_Click"  />
    
    <asp:ListView ID="lvFields" runat="server" DataSourceID="SqlDataSource1" 
        DataKeyNames="Id" InsertItemPosition="None" 
        onitemdatabound="lvFields_ItemDataBound" 
        onitemediting="lvFields_ItemEditing" 
        onitemcanceling="lvFields_ItemCanceling" 
        onitemcommand="lvFields_ItemCommand" 
        oniteminserted="lvFields_ItemInserted" 
        onitemupdated="lvFields_ItemUpdated" 
        ondatabound="lvFields_DataBound" 
        oniteminserting="lvFields_ItemInserting" 
        onitemupdating="lvFields_ItemUpdating" 
        onitemcreated="lvFields_ItemCreated" OnDataBinding="lvFields_DataBinding" OnSelectedIndexChanged="lvFields_SelectedIndexChanged">
        <ItemTemplate>
            <tr class='it<%# GetItemCss(Container.DataItem) %>' onclick='<%# getOnClick(Container) %>' >
                <uc2:cntField ID="cntField" Mode="td" runat="server" Data='<%# Container.DataItem %>' />                
                <td id="tdControl" runat="server" class="control" visible='<%# EditMode %>' >
                    <asp:Button ID="btSelect" runat="server" CommandName="Select" CssClass="button_postback"/>
                    <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ToolTip="Edytuj" ImageUrl="~/images/buttons/edit.png" />
                    <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete" ToolTip="Usuń" ImageUrl="~/images/buttons/delete.png"/>
                </td>
            </tr>    
        </ItemTemplate>
        <SelectedItemTemplate>
            <tr class='it it-selected<%# GetItemCss(Container.DataItem) %>' onclick='<%# getOnClick(Container) %>' >
                <uc2:cntField ID="cntField" Mode="td" runat="server" Data='<%# Container.DataItem %>' />                
                <td id="tdControl" runat="server" class="control" visible='<%# EditMode %>' >
                    <asp:Button ID="btSelect" runat="server" CommandName="Unselect" CssClass="button_postback"/>
                    <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ToolTip="Edytuj" ImageUrl="~/images/buttons/edit.png" />
                    <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete" ToolTip="Usuń" ImageUrl="~/images/buttons/delete.png"/>
                </td>
            </tr>    
        </SelectedItemTemplate>
        <EmptyDataTemplate>
            <div class="edt">
                Filtr - brak zdefiniowanych pól
            </div>    
            <div id="paButtons" runat="server" visible='<%# EditMode %>' >
                <asp:Button ID="NewButton" runat="server" CssClass="button" CommandName="NewRecord" Text="Dodaj pole filtru" Visible="false" />
            </div>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td class="iit-td" colspan="2" >
                    <div id="iit-divZoom" class="modalPopup">
                        <asp:UpdatePanel ID="UpdatePanelI" runat="server">
                            <ContentTemplate>
                                <uc2:cntFieldParams ID="cntFieldParamsI" runat="server" />                
                                <div class="buttons">
                                    <asp:Button ID="InsertButton" runat="server" CssClass="button75" CommandName="Insert" Text="Zapisz" />
                                    <asp:Button ID="CancelInsertButton" runat="server" CssClass="button75" CommandName="CancelInsert" Text="Anuluj" />
                                </div>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>                
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="it it-selected">
                <uc2:cntField ID="cntField" Mode="td" runat="server" Data='<%# Container.DataItem %>' />                
                <td class="control">
                    <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit" ToolTip="Edytuj" ImageUrl="~/images/buttons/edit.png" />
                    <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete" ToolTip="Usuń" ImageUrl="~/images/buttons/delete.png" />
                </td>
            </tr>
            <tr class="eit">
                <td class="eit-td" colspan="2" >
                    <div id="eit-divZoom" class="modalPopup">
                        <asp:UpdatePanel ID="UpdatePanelE" runat="server">
                            <ContentTemplate>
                                <uc2:cntFieldParams ID="cntFieldParamsE" runat="server" />                
                                <div class="buttons">
                                    <asp:Button ID="UpdateButton" runat="server" CssClass="button75" CommandName="Update" Text="Update" />
                                    <asp:Button ID="CancelButton" runat="server" CssClass="button75" CommandName="Cancel" Text="Cancel" />
                                    <%--                                
                                    <asp:Button ID="CancelButtonPopup" runat="server" CssClass="button_postback" CommandName="CancelEdit" />
                                    <asp:Button ID="Button1" runat="server" CssClass="button75" Text="Postback" Visible="false"/>
                                    --%>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>   
                </td>
            </tr>
        </EditItemTemplate>
        <LayoutTemplate>
            <table ID="itemPlaceholderContainer" runat="server" border="0" class="tbFilterFields table0">
                <tr ID="itemPlaceholder" runat="server">
                </tr>
            </table>            
        </LayoutTemplate>
    </asp:ListView>
</div>
                
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"     
    SelectCommand="
SELECT * FROM SqlFields
WHERE IdRaportu = @IdRaportu AND Kolumna = @Kolumna and (@EditMode = 1 or Aktywny = 1)
ORDER BY Kolejnosc, Id
    " 
    DeleteCommand="DELETE FROM [SqlFields] WHERE [Id] = @Id" 
    InsertCommand="
INSERT INTO [SqlFields] ([IdRaportu], [Kolumna], [Typ], [Rodzaj], [FormId], [Label1], [ToolTip1], [Label2], [ToolTip2], [Label3], [ToolTip3], [Format], [MaxLen], [MinValue], [MaxValue], [InitValue], [AllowedChars], [NoValue], [Required], [AutoRefresh], [LookupSql], [RetValue1], [RetValue2], [Kolejnosc], [Aktywny]) 
VALUES (@IdRaportu, @Kolumna, @Typ, @Rodzaj, @FormId, @Label1, @ToolTip1, @Label2, @ToolTip2, @Label3, @ToolTip3, @Format, @MaxLen, @MinValue, @MaxValue, @InitValue, @AllowedChars, @NoValue, @Required, @AutoRefresh, @LookupSql, @RetValue1, @RetValue2, @Kolejnosc, @Aktywny)
set @Id = (select @@Identity)
    " 
    UpdateCommand="
UPDATE [SqlFields] SET 
[Kolumna] = @Kolumna, [Typ] = @Typ, [Rodzaj] = @Rodzaj, [FormId] = @FormId
,[Label1] = @Label1, [ToolTip1] = @ToolTip1, [Label2] = @Label2, [ToolTip2] = @ToolTip2, [Label3] = @Label3, [ToolTip3] = @ToolTip3
,[Format] = @Format, [MaxLen] = @MaxLen, [MinValue] = @MinValue, [MaxValue] = @MaxValue, [InitValue] = @InitValue
,[AllowedChars] = @AllowedChars, [NoValue] = @NoValue
,[Required] = @Required, [AutoRefresh] = @AutoRefresh 
,[LookupSql] = @LookupSql, [RetValue1] = @RetValue1, [RetValue2] = @RetValue2
,[Kolejnosc] = @Kolejnosc, [Aktywny] = @Aktywny 
WHERE [Id] = @Id
    " 
    ondatabinding="SqlDataSource1_DataBinding" 
    onselecting="SqlDataSource1_Selecting" OnInserted="SqlDataSource1_Inserted">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidRepId" Name="IdRaportu" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidColumn" Name="Kolumna" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidEditMode" Name="EditMode" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Kolumna" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="Rodzaj" Type="Int32" />
        <asp:Parameter Name="FormId" Type="String" />
        <asp:Parameter Name="Label1" Type="String" />
        <asp:Parameter Name="ToolTip1" Type="String" />
        <asp:Parameter Name="Label2" Type="String" />
        <asp:Parameter Name="ToolTip2" Type="String" />
        <asp:Parameter Name="Label3" Type="String" />
        <asp:Parameter Name="ToolTip3" Type="String" />
        <asp:Parameter Name="Format" Type="String" />
        <asp:Parameter Name="MaxLen" Type="Int32" />
        <asp:Parameter Name="MinValue" Type="String" />
        <asp:Parameter Name="MaxValue" Type="String" />
        <asp:Parameter Name="InitValue" Type="String" />
        <asp:Parameter Name="AllowedChars" Type="String" />
        <asp:Parameter Name="NoValue" Type="Boolean" />
        <asp:Parameter Name="Required" Type="Boolean" />
        <asp:Parameter Name="AutoRefresh" Type="Boolean" />
        <asp:Parameter Name="LookupSql" Type="String" />
        <asp:Parameter Name="RetValue1" Type="String" />
        <asp:Parameter Name="RetValue2" Type="String" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Id" Type="Int32" Direction="Output" DefaultValue="0"/>
        <asp:ControlParameter ControlID="hidRepId" Name="IdRaportu" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="Kolumna" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="Rodzaj" Type="Int32" />
        <asp:Parameter Name="FormId" Type="String" />
        <asp:Parameter Name="Label1" Type="String" />
        <asp:Parameter Name="ToolTip1" Type="String" />
        <asp:Parameter Name="Label2" Type="String" />
        <asp:Parameter Name="ToolTip2" Type="String" />
        <asp:Parameter Name="Label3" Type="String" />
        <asp:Parameter Name="ToolTip3" Type="String" />
        <asp:Parameter Name="Format" Type="String" />
        <asp:Parameter Name="MaxLen" Type="Int32" />
        <asp:Parameter Name="MinValue" Type="String" />
        <asp:Parameter Name="MaxValue" Type="String" />
        <asp:Parameter Name="InitValue" Type="String" />
        <asp:Parameter Name="AllowedChars" Type="String" />
        <asp:Parameter Name="NoValue" Type="Boolean" />
        <asp:Parameter Name="Required" Type="Boolean" />
        <asp:Parameter Name="AutoRefresh" Type="Boolean" />
        <asp:Parameter Name="LookupSql" Type="String" />
        <asp:Parameter Name="RetValue1" Type="String" />
        <asp:Parameter Name="RetValue2" Type="String" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
                




<%--
                <table>
            
                <asp:Repeater ID="rpFilter" runat="server" DataSourceID="SqlDataSource1" 
                    ondatabinding="rpFilter_DataBinding" 
                    onitemcreated="rpFilter_ItemCreated" 
                    onitemdatabound="rpFilter_ItemDataBound" 
                    onload="rpFilter_Load">                    
                    <ItemTemplate>
                        
                        <tr>
                    
                        <uc2:cntField ID="cntField" Mode="td" runat="server" Data='<%# Container.DataItem %>' />
                        
                        </tr>
                        
                    </ItemTemplate>
                </asp:Repeater>                
                
                </table>
--%>                



<%--
            <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            IdRaportu:
            <asp:Label ID="IdRaportuLabel" runat="server" Text='<%# Eval("IdRaportu") %>' />
            <br />
            Kolumna:
            <asp:Label ID="KolumnaLabel" runat="server" Text='<%# Eval("Kolumna") %>' />
            <br />
            Typ:
            <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            <br />
            Rodzaj:
            <asp:Label ID="RodzajLabel" runat="server" Text='<%# Eval("Rodzaj") %>' />
            <br />
            FormId:
            <asp:Label ID="FormIdLabel" runat="server" Text='<%# Eval("FormId") %>' />
            <br />
            Label1:
            <asp:Label ID="Label1Label" runat="server" Text='<%# Eval("Label1") %>' />
            <br />
            ToolTip1:
            <asp:Label ID="ToolTip1Label" runat="server" Text='<%# Eval("ToolTip1") %>' />
            <br />
            Label2:
            <asp:Label ID="Label2Label" runat="server" Text='<%# Eval("Label2") %>' />
            <br />
            ToolTip2:
            <asp:Label ID="ToolTip2Label" runat="server" Text='<%# Eval("ToolTip2") %>' />
            <br />
            Label3:
            <asp:Label ID="Label3Label" runat="server" Text='<%# Eval("Label3") %>' />
            <br />
            ToolTip3:
            <asp:Label ID="ToolTip3Label" runat="server" Text='<%# Eval("ToolTip3") %>' />
            <br />
            Format:
            <asp:Label ID="FormatLabel" runat="server" Text='<%# Eval("Format") %>' />
            <br />
            MaxLen:
            <asp:Label ID="MaxLenLabel" runat="server" Text='<%# Eval("MaxLen") %>' />
            <br />
            MinValue:
            <asp:Label ID="MinValueLabel" runat="server" Text='<%# Eval("MinValue") %>' />
            <br />
            MaxValue:
            <asp:Label ID="MaxValueLabel" runat="server" Text='<%# Eval("MaxValue") %>' />
            <br />
            InitValue:
            <asp:Label ID="InitValueLabel" runat="server" Text='<%# Eval("InitValue") %>' />
            <br />
            AllowedChars:
            <asp:Label ID="AllowedCharsLabel" runat="server" 
                Text='<%# Eval("AllowedChars") %>' />
            <br />
            <asp:CheckBox ID="NoValueCheckBox" runat="server" 
                Checked='<%# Eval("NoValue") %>' Enabled="false" Text="NoValue" />
            <br />
            <asp:CheckBox ID="RequiredCheckBox" runat="server" 
                Checked='<%# Eval("Required") %>' Enabled="false" Text="Required" />
            <br />
            <asp:CheckBox ID="AutoRefreshCheckBox" runat="server" 
                Checked='<%# Eval("AutoRefresh") %>' Enabled="false" Text="AutoRefresh" />
            <br />
            LookupSql:
            <asp:Label ID="LookupSqlLabel" runat="server" Text='<%# Eval("LookupSql") %>' />
            <br />
            RetValue1:
            <asp:Label ID="RetValue1Label" runat="server" Text='<%# Eval("RetValue1") %>' />
            <br />
            RetValue2:
            <asp:Label ID="RetValue2Label" runat="server" Text='<%# Eval("RetValue2") %>' />
            <br />
            Kolejnosc:
            <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
            <br />
            <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                Checked='<%# Eval("Aktywny") %>' Enabled="false" Text="Aktywny" />
            <br />
--%>



<%--            Label1:
            <asp:TextBox ID="Label1TextBox" runat="server"  />
            <br />
            ToolTip1:
            <asp:TextBox ID="ToolTip1TextBox" runat="server" Text='<%# Bind("ToolTip1") %>' />
            <br />
            <span style="">IdRaportu:
            <asp:TextBox ID="IdRaportuTextBox2" runat="server" Text='<%# Bind("IdRaportu") %>' />
            <br />
            Kolumna:
            <asp:TextBox ID="KolumnaTextBox" runat="server" Text='<%# Bind("Kolumna") %>' />
            <br />
            Typ:
            <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            <br />
            Rodzaj:
            <asp:TextBox ID="RodzajTextBox" runat="server" Text='<%# Bind("Rodzaj") %>' />
            <br />
            FormId:
            <asp:TextBox ID="FormIdTextBox" runat="server" Text='<%# Bind("FormId") %>' />
            <br />
            Label2:
            <asp:TextBox ID="Label2TextBox" runat="server" Text='<%# Bind("Label2") %>' />
            <br />
            ToolTip2:
            <asp:TextBox ID="ToolTip2TextBox" runat="server" Text='<%# Bind("ToolTip2") %>' />
            <br />
            Label3:
            <asp:TextBox ID="Label3TextBox" runat="server" Text='<%# Bind("Label3") %>' />
            <br />
            ToolTip3:
            <asp:TextBox ID="ToolTip3TextBox" runat="server" Text='<%# Bind("ToolTip3") %>' />
            <br />
            Format:
            <asp:TextBox ID="FormatTextBox" runat="server" Text='<%# Bind("Format") %>' />
            <br />
            MaxLen:
            <asp:TextBox ID="MaxLenTextBox" runat="server" Text='<%# Bind("MaxLen") %>' />
            <br />
            MinValue:
            <asp:TextBox ID="MinValueTextBox" runat="server" Text='<%# Bind("MinValue") %>' />
            <br />
            MaxValue:
            <asp:TextBox ID="MaxValueTextBox" runat="server" Text='<%# Bind("MaxValue") %>' />
            <br />
            InitValue:
            <asp:TextBox ID="InitValueTextBox" runat="server" Text='<%# Bind("InitValue") %>' />
            <br />
            AllowedChars:
            <asp:TextBox ID="AllowedCharsTextBox" runat="server" Text='<%# Bind("AllowedChars") %>' />
            <br />
            <asp:CheckBox ID="NoValueCheckBox" runat="server" Checked='<%# Bind("NoValue") %>' Text="NoValue" />
            <br />
            <asp:CheckBox ID="RequiredCheckBox" runat="server" Checked='<%# Bind("Required") %>' Text="Required" />
            <br />
            <asp:CheckBox ID="AutoRefreshCheckBox" runat="server" Checked='<%# Bind("AutoRefresh") %>' Text="AutoRefresh" />
            <br />
            LookupSql:
            <asp:TextBox ID="LookupSqlTextBox" runat="server" Text='<%# Bind("LookupSql") %>' />
            <br />
            RetValue1:
            <asp:TextBox ID="RetValue1TextBox" runat="server" Text='<%# Bind("RetValue1") %>' />
            <br />
            RetValue2:
            <asp:TextBox ID="RetValue2TextBox" runat="server" Text='<%# Bind("RetValue2") %>' />
            <br />
            Kolejnosc:
            <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
            <br />
            <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Aktywny" />
            <br />
--%>            
<%--
            Id:
            <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            IdRaportu:
            <asp:TextBox ID="IdRaportuTextBox" runat="server" Text='<%# Bind("IdRaportu") %>' />
            <br />
            Kolumna:
            <asp:TextBox ID="KolumnaTextBox" runat="server" Text='<%# Bind("Kolumna") %>' />
            <br />
            Typ:
            <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            <br />
            Rodzaj:
            <asp:TextBox ID="RodzajTextBox" runat="server" Text='<%# Bind("Rodzaj") %>' />
            <br />
            FormId:
            <asp:TextBox ID="FormIdTextBox" runat="server" Text='<%# Bind("FormId") %>' />
            <br />
            Label1:
            <asp:TextBox ID="Label1TextBox" runat="server" Text='<%# Bind("Label1") %>' />
            <br />
            ToolTip1:
            <asp:TextBox ID="ToolTip1TextBox" runat="server" Text='<%# Bind("ToolTip1") %>' />
            <br />
            Label2:
            <asp:TextBox ID="Label2TextBox" runat="server" Text='<%# Bind("Label2") %>' />
            <br />
            ToolTip2:
            <asp:TextBox ID="ToolTip2TextBox" runat="server" Text='<%# Bind("ToolTip2") %>' />
            <br />
            Label3:
            <asp:TextBox ID="Label3TextBox" runat="server" Text='<%# Bind("Label3") %>' />
            <br />
            ToolTip3:
            <asp:TextBox ID="ToolTip3TextBox" runat="server" Text='<%# Bind("ToolTip3") %>' />
            <br />
            Format:
            <asp:TextBox ID="FormatTextBox" runat="server" Text='<%# Bind("Format") %>' />
            <br />
            MaxLen:
            <asp:TextBox ID="MaxLenTextBox" runat="server" Text='<%# Bind("MaxLen") %>' />
            <br />
            MinValue:
            <asp:TextBox ID="MinValueTextBox" runat="server" Text='<%# Bind("MinValue") %>' />
            <br />
            MaxValue:
            <asp:TextBox ID="MaxValueTextBox" runat="server" Text='<%# Bind("MaxValue") %>' />
            <br />
            InitValue:
            <asp:TextBox ID="InitValueTextBox" runat="server" Text='<%# Bind("InitValue") %>' />
            <br />
            AllowedChars:
            <asp:TextBox ID="AllowedCharsTextBox" runat="server" Text='<%# Bind("AllowedChars") %>' />
            <br />
            <asp:CheckBox ID="NoValueCheckBox" runat="server" Checked='<%# Bind("NoValue") %>' Text="NoValue" />
            <br />
            <asp:CheckBox ID="RequiredCheckBox" runat="server" Checked='<%# Bind("Required") %>' Text="Required" />
            <br />
            <asp:CheckBox ID="AutoRefreshCheckBox" runat="server" Checked='<%# Bind("AutoRefresh") %>' Text="AutoRefresh" />
            <br />
            LookupSql:
            <asp:TextBox ID="LookupSqlTextBox" runat="server" Text='<%# Bind("LookupSql") %>' />
            <br />
            RetValue1:
            <asp:TextBox ID="RetValue1TextBox" runat="server" Text='<%# Bind("RetValue1") %>' />
            <br />
            RetValue2:
            <asp:TextBox ID="RetValue2TextBox" runat="server" Text='<%# Bind("RetValue2") %>' />
            <br />
            Kolejnosc:
            <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
            <br />
            <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Aktywny" />
            <br />
--%>