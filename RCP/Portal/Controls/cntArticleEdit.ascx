<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntArticleEdit.ascx.cs" Inherits="HRRcp.Portal.Controls.cntArticleEdit" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<style>
    .cntArticleEdit .config-box .btn { margin-left: 4px; }
    .cntArticleEdit .config-box .btn-cancel { margin-right: 32px; }
    .cntArticleEdit .editor-wrapper { xbackground-color: #fff; xpadding: 16px; xborder: solid 1px #eee; }
    .cntArticleEdit .editor { overflow: auto; xmin-height: 1200px; }
    .cntArticleEdit .buttons { margin-top: 8px; }
    .form-group .r-label { padding-top: 7px; margin-bottom: 0; text-align: right; }
    .form-inline .checkbox { margin-top: 7px; }

    .cntArticleEdit .config-box-inner { border-bottom: solid 1px #ddd; margin-bottom: 4px; }

    /*.mce-panel {border: solid 1px #ddd !important; background-color: #eee;  }*/

    .mce-panel { border: 0 solid #ddd; }
        .mce-panel * { /*font-family: 'Noto Sans', sans-serif;*/ color: #333; }
</style>

<asp:HiddenField ID="hidArticleId" runat="server" Visible="false" />
<input id="hidEditorData" type="hidden" class="hidEditorData" runat="server" />

<div id="ctArticleEdit" runat="server" class="cntArticleEdit">
    <div class="page-title">
        <asp:Label ID="lblTitle" runat="server" />
    </div>
    <div class="container">
        <div class="editor-wrapper">
            <asp:TextBox ID="htmlEditor" runat="server" CssClass="editor" TextMode="MultiLine" />
        </div>
        <hr />
        <div class="config-box box">
            <div class="row">
                <div class="col-md-12">
                    <div class="config-box-inner">
                        <span class="title">Konfiguracja artykułu</span>
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
                            <%--<div class="checkbox">--%>
                            <%--<asp:CheckBox ID="AktywnyCheckBox" runat="server" Text="Aktywny" />--%>
                            <%--</div>--%>
                            <label class="col-md-2 r-label">Wydruk</label>
                            <div class="checkbox">
                                <asp:CheckBox ID="WydrukCheckBox" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="buttons pull-right">
                <asp:Button ID="btnSave" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClientClick="$('.hidEditorData').val($('.html-editor').html());return true;" OnClick="btnSave_Click" />

                <asp:Button ID="btnBack" runat="server" Text="Anuluj" CssClass="btn btn-default btn-cancel" OnClick="btnBack_Click" />

                <asp:Button ID="btnDeleteConfirm" runat="server" Text="Usuń" CssClass="btn btn-danger" OnClick="btnDeleteConfirm_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="hidden" OnClick="btnDelete_Click" />
            </div>
            <div class="clear-fix"></div>
        </div>
    </div>
</div>
