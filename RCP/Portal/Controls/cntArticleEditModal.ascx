<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntArticleEditModal.ascx.cs" Inherits="HRRcp.Portal.Controls.cntArticleEditModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>


<uc1:cntModal runat="server" ID="cntModal" Width="960px" CssClass="article-edit-modal" Keyboard="false" Backdrop="false">
    <HeaderTemplate>
        <h4>Edycja artykułu</h4>
    </HeaderTemplate>
    <ContentTemplate>
        <div class="form-group">
            <%--<h3>Tytuł</h3>--%>
            <%--<label class="col-sm-1 control-label">Tytuł:</label>--%>
            <asp:TextBox ID="tbTitle" runat="server" CssClass="form-control" placeholder="Tytuł..." />
            <%--<div class="col-sm-12">
                
            </div>--%>
        </div>
        <div class="editor-wrapper">
            <%--<h3>Treść</h3>--%>
            <%--<asp:UpdatePanel ID="upEditor" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
            <asp:TextBox ID="htmlEditor" runat="server" CssClass="editor tinymce-editor" TextMode="MultiLine" />
            <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
            <input id="uploader" type="file" class="hidden" />
        </div>
        <hr />
        <div class="form-group form-inline">
            <label class="col-md-2 r-label">Data publikacji</label>
            <uc1:DateEdit runat="server" ID="DateEdit1" />
        </div>
        <div class="form-group xform-inline">
            <label class="col-md-2 r-label">Wyświetlaj na pozycji</label>
            <asp:DropDownList ID="ddlPozycja" runat="server" CssClass="form-control" Width="186px">
                <asp:ListItem Value="" Text="wybierz ..."></asp:ListItem>
                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                <asp:ListItem Value="3" Text="3"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group form-inline">
            <label class="col-md-2 r-label">Aktywny</label>
            <div class="checkbox">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" />
            </div>
        </div>
        <div class="form-group form-inline">
            <label class="col-md-2 r-label">Wydruk</label>
            <div class="checkbox">
                <asp:CheckBox ID="WydrukCheckBox" runat="server" />
            </div>
        </div>
        <%--<div class="buttons pull-right">
                <asp:Button ID="Button1" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClientClick="$('.hidEditorData').val($('.html-editor').html());return true;" OnClick="btnSave_Click" />

                <asp:Button ID="btnBack" runat="server" Text="Anuluj" CssClass="btn btn-default btn-cancel" OnClick="btnBack_Click" />

                <asp:Button ID="btnDeleteConfirm" runat="server" Text="Usuń" CssClass="btn btn-danger" OnClick="btnDeleteConfirm_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="hidden" OnClick="btnDelete_Click" />
            </div>--%>
        <div class="clear-fix"></div>
        
    </ContentTemplate>
    <FooterTemplate>
        <%--<asp:UpdatePanel ID="upButtons" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>

        <input id="hidEditorData" type="hidden" class="hidEditorData" runat="server" />
        <asp:Button ID="btnSaveConfirm" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClick="btnSaveConfirm_Click" />
        <asp:Button ID="btnSave" runat="server" CssClass="hidden" OnClick="btnSave_Click" />

        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </FooterTemplate>
</uc1:cntModal>
