<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntArticles.ascx.cs" Inherits="HRRcp.Portal.Controls.cntArticles" %>
<%@ Register Src="~/Portal/Controls/cntArticleEdit.ascx" TagPrefix="uc1" TagName="cntArticleEdit" %>
<%@ Register Src="~/Portal/Controls/cntArticleEditModal.ascx" TagPrefix="uc1" TagName="cntArticleEditModal" %>

<asp:HiddenField ID="hidGrupa" runat="server" Visible="false" />
<asp:HiddenField ID="hidEdit" runat="server" Visible="false" />

<div class="cntArticles">
    <asp:UpdatePanel ID="upArticles" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="page-title">
                <%--<i class="fa fa-server"></i>--%>
                <asp:Label ID="lblTitle" runat="server" />
                
                <asp:LinkButton ID="btEdit" runat="server" CssClass="pull-right btn btn-primary pull-right" OnClick="btEdit_Click"><i class="fa fa-pencil"></i>Edytuj</asp:LinkButton>

                <asp:LinkButton ID="InsertButton" runat="server" CssClass="btn-success btn btn-inline pull-right" OnClick="InsertButton_Click" Visible="true">
                    <i class="fa fa-plus"></i>
                    Dodaj nowy
                </asp:LinkButton>
                <div id="paTopButtons" runat="server" class="top-buttons pull-right hidden" visible="false">
                    <asp:LinkButton ID="btCancelEdit" runat="server" CssClass="btn-default btn btn-inline" OnClick="btCancelEdit_Click">
                        <i class="fa fa-close"></i>
                        Zakończ tryb edycji
                    </asp:LinkButton>
                </div>
            </div>
            <div class="container wide">
                <asp:ListView ID="lvArtykuly" runat="server" DataKeyNames="Id" DataSourceID="dsArticles"
                    OnItemDataBound="lvArtykuly_ItemDataBound"
                    OnItemInserting="lvArtykuly_ItemInserting"
                    OnItemUpdating="lvArtykuly_ItemUpdating"
                    OnItemCreated="lvArtykuly_ItemCreated"
                    OnDataBound="lvArtykuly_DataBound"
                    OnItemCanceling="lvArtykuly_ItemCanceling"
                    OnItemCommand="lvArtykuly_ItemCommand"
                    OnItemDeleted="lvArtykuly_ItemDeleted"
                    OnItemEditing="lvArtykuly_ItemEditing"
                    OnItemInserted="lvArtykuly_ItemInserted"
                    OnItemUpdated="lvArtykuly_ItemUpdated">
                    <ItemTemplate>
                        <div class="article">
                            <div class="article-bubbles">
                                <a class="format-bubble">
                                    <i class="fa fa-file-text-o"></i>
                                </a>
                            </div>
                            <h2><%# Eval("Tytul") %></h2>
                            <div class="date">
                                <i class="fa fa-calendar"></i>
                                <%# Eval("CreateDate", "{0:d}") %>
                            </div>
                            <div id="paFuncButtons" runat="server" class="func-buttons" visible='<%# CanEdit %>'>
                                <div class="dropdown">
                                    <a class="dropdown-toggle" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                        <i class="fa fa-cog"></i>
                                    </a>
                                    <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                        <li>
                                            <asp:LinkButton ID="lnkEditArticle" runat="server" Text="Edytuj" OnClick="lnkEditArticle_Click" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkRemoveArticleConfirm" runat="server" Text="Usuń" OnClick="lnkRemoveArticleConfirm_Click" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                                <%--<a class="favorite">
                                    <i class="fa fa-heart"></i>
                                </a>--%>
                            </div>
                            <hr class="article-header-separator" />
                            <div class="art">
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                <div class="printbutton printoff">
                                    <asp:LinkButton ID="btPrint" runat="server" CssClass="button100 btn btn-primary" Visible='<%# IsPrintVisible(Eval("Wydruk")) %>' OnClientClick="javascript:window.print();">
                                        <i class="fa fa-print"></i>
                                        Drukuj
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div id="paEdit" runat="server" class="edit" visible='<%# IsEditable %>'>
                                <div class="buttons">
                                    <asp:LinkButton ID="EditButton" runat="server" CssClass="btn-small btn-primary" CommandName="Edit2" Text="Edytuj"><i class="fa fa-pencil"></i></asp:LinkButton>
                                    <asp:LinkButton ID="DeleteButton" runat="server" CssClass="btn-small btn-danger" CommandName="Delete" Text="Usuń" Visible="true"><i class="fa fa-trash"></i></asp:LinkButton>
                                </div>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                    </ItemTemplate>

                    <InsertItemTemplate>
                    </InsertItemTemplate>

                    <EmptyDataTemplate>
                        <asp:LinkButton ID="InsertButton" runat="server" CssClass="btn btn-success" CommandName="NewRecord2">
                            <i class="glyphicon glyphicon-plus"></i>
                            Dodaj artykuł
                        </asp:LinkButton>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div id="itemPlaceholderContainer" runat="server" class="list">
                            <span id="itemPlaceholder" runat="server" />
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
            </div>
            <asp:Button ID="btnRemoveArticle" runat="server" CssClass="hidden" OnClick="btnRemoveArticle_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional" Visible="false" >
        <ContentTemplate>
            <uc1:cntArticleEditModal runat="server" ID="cntArticleEditModal" OnSaved="cntArticleEditModal_Saved" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<asp:SqlDataSource ID="dsArticles" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    OnInserted="dsArticles_Inserted"
    SelectCommand="
select *, 1 Sort, Pozycja RowNum, null Tytul
--select *, 1 Sort, Pozycja RowNum
from Teksty
where Grupa = @Grupa and (@edit = 1 or Widoczny = 1) and Pozycja is not null
union all
select *, 2 Sort, ROW_NUMBER() OVER (Order by DataPublikacji desc) AS RowNum, null Tytul
from Teksty
where Grupa = @Grupa and (@edit = 1 or Widoczny = 1) and Pozycja is null
order by RowNum, Sort, DataPublikacji
        
/*SELECT * FROM [Teksty] where Grupa = @Grupa and (@edit = 1 or Widoczny = 1) 
order by DataPublikacji desc
*/"
    DeleteCommand="DELETE FROM [Teksty] WHERE [Id] = @Id"
    InsertCommand="
--set @Typ = 'ART' + convert(varchar, (select count(*) + 1 from Teksty where Grupa = @Grupa))      
set @Typ = 'ART' + convert(varchar, (select count(*) + 1 from Teksty))      
set @Opis = ''
INSERT INTO [Teksty] ([Typ], [Opis], [Tekst], Grupa, Widoczny, IdAutora, DataPublikacji, Image) 
        VALUES (@Typ, @Opis, @Tekst, @Grupa, @Widoczny, @IdAutora, @DataPublikacji, @Image)"
    UpdateCommand="UPDATE [Teksty] SET [Tekst] = @Tekst, Widoczny = @Widoczny, IdAutora = @IdAutora, DataPublikacji = @DataPublikacji
        WHERE [Id] = @Id">
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

<asp:SqlDataSource ID="dsRemoveArticle" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="delete from Teksty where Id = {0}" />
