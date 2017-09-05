<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntArticles4.ascx.cs" Inherits="HRRcp.Controls.Portal.cntArticles4" %>

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
    <div id="paEditButton" runat="server" class="paEditButton printoff" visible="false" >
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
                <asp:Literal ID="Literal1" runat="server" ></asp:Literal>
                <div class="printbutton printoff">
                    <asp:Button ID="btPrint" runat="server" CssClass="button100" Text="Drukuj" Visible='<%# IsPrintVisible(Eval("Wydruk")) %>' OnClientClick="javascript:window.print();" />    
                </div>
            </div>
            <div id="paEdit" runat="server" class="edit" visible='<%# IsEditable %>'>
                <div class="buttons">
                    <div class="left">
                        <asp:Label ID="Label1" CssClass="label" runat="server" Text="Data publikacji:" />
                        <asp:Label ID="Label2" CssClass="value" runat="server" Text='<%# Eval("DataPublikacji", "{0:d}") %>' />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Widoczny") %>' Text="Widoczny po dacie publikacji" Enabled="false" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Wydruk") %>' Text="Wydruk" Enabled="false" Visible='<%# IsWydruk %>'/>
                    </div>
                    <asp:Button ID="EditButton" runat="server" CssClass="button75" CommandName="Edit2" Text="Edytuj" />
                    <asp:Button ID="DeleteButton" runat="server" CssClass="button75" CommandName="Delete" Text="Usuń" Visible="false"/>
                </div>
            </div>    
        </div>
    </ItemTemplate>
    
    <InsertItemTemplate>
    </InsertItemTemplate>

    <EmptyDataTemplate>
        <asp:Button ID="InsertButton" runat="server" CssClass="button100" CommandName="NewRecord2" Text="Dodaj artykuł" />        
    </EmptyDataTemplate>


    <LayoutTemplate>
        <div ID="itemPlaceholderContainer" runat="server" class="list">
            <div id="paTopButtons" runat="server" class="topbuttons" visible="false" >
                <div class="left">
                    <asp:Button ID="btCancelEdit" runat="server" CssClass="button" Text="Zakończ tryb edycji" OnClick="btCancelEdit_Click"/>
                </div>
                <asp:Button ID="InsertButton" runat="server" CssClass="button125" CommandName="NewRecord2" Text="Nowy artykuł" />
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
        <asp:ControlParameter ControlID="hidEdit" Name="edit" PropertyName="Value" Type="Int32" />
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

</div>

