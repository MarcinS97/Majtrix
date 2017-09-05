<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlReportEdit.ascx.cs" Inherits="HRRcp.Controls.Reports.cntSqlReportEdit" %>

<div id="paSqlReportEdit" runat="server" class="cntSqlReportEdit">
    <div id="cntSqlReportEditZoom" class="modalPopup">
        <asp:UpdatePanel ID="UpdatePanelE" runat="server">
            <ContentTemplate>
                <div id="paEdit" runat="server" visible="false" class="paEdit" >
                    <span class="label1">Id:</span>
                    <asp:TextBox ID="Id" CssClass="repid" runat="server"></asp:TextBox>
                    <div class="edit1">
                        <span class="label2">Pokaż w menu:</span>
                        <asp:CheckBox ID="Aktywny" CssClass="check" runat="server" />
                        <span class="label2">Mode:</span>
                        <asp:TextBox ID="Mode" runat="server" CssClass="mode"></asp:TextBox>
                    </div>
                    <br />
                    
                    <span class="label1">Nazwa:</span>
                    <asp:TextBox ID="MenuText" runat="server" CssClass="nazwa"></asp:TextBox>
                    <br />
                    <span class="label1">Opis:</span>
                    <asp:TextBox ID="ToolTip" runat="server" TextMode="MultiLine" Rows="2" CssClass="opis opis2"></asp:TextBox>
                    <br />
                    <span class="label1">Tytuł:</span>
                    <asp:TextBox ID="Par1" runat="server" TextMode="MultiLine" Rows="3" CssClass="opis opis3"></asp:TextBox>
                    <br />
                    <span class="label1">Par2:</span>
                    <asp:TextBox ID="Par2" runat="server" TextMode="MultiLine" Rows="3" CssClass="opis opis3"></asp:TextBox>
                    <br />
                    
                    <span class="label1">Class:</span>
                    <asp:TextBox ID="Class" CssClass="css" runat="server"></asp:TextBox>
                    <br />
                    
                    <span class="label1">Rights:</span>
                    <asp:TextBox ID="Rights" runat="server" CssClass="rights"></asp:TextBox>

                    <br />
                    <span class="label1">Sql:</span><br />
                    <asp:TextBox ID="Sql" runat="server" TextMode="MultiLine" Rows="20" CssClass="w100 sql"></asp:TextBox>
                    <br />
                    <span class="label1">Footer Sql:</span><br />
                    <asp:TextBox ID="SqlParams" runat="server" TextMode="MultiLine" Rows="7" CssClass="w100 par"></asp:TextBox>
                    <br />
                    <span class="label1">Javascript:</span><br />
                    <asp:TextBox ID="Javascript" runat="server" TextMode="MultiLine" Rows="7" CssClass="w100 par"></asp:TextBox>

                    <div class="bottom_buttons">
                        <asp:Button ID="btSave" runat="server" CssClass="button75" Text="Zapisz" OnClick="btSave_Click" />
                        <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Anuluj" OnClick="btCancel_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>