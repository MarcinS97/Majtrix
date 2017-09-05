<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntInfoBoxEdit.ascx.cs" Inherits="HRRcp.Controls.Reports.cntInfoBoxEdit" %>

<div id="paInfoBoxEdit" runat="server" class="cntInfoBoxEditZoom">  <%--xxxmodal ten div przepina i class nie ma tu sensu, dlatego w paEdit--%>
    <div id="cntInfoBoxEditZoom" class="modalPopup">
        <asp:UpdatePanel ID="UpdatePanelE" runat="server">
            <ContentTemplate>
                <div id="paEdit" runat="server" visible="false" class="cntInfoBoxEdit" >
                    <asp:Menu ID="tabDane" runat="server" Orientation="Horizontal" OnMenuItemClick="tabDane_MenuItemClick" SkipLinkText="">
                        <StaticMenuStyle CssClass="tabsStrip" />
                        <StaticMenuItemStyle CssClass="tabItem" />
                        <StaticSelectedStyle CssClass="tabSelected" />
                        <StaticHoverStyle CssClass="tabHover" />
                        <Items>
                            <asp:MenuItem Text="Dane podstawowe" Value="vDane" Selected="True"></asp:MenuItem>
                            <asp:MenuItem Text="Css" Value="vCss" ></asp:MenuItem>
                            <asp:MenuItem Text="Script" Value="vScript" ></asp:MenuItem>
                            <asp:MenuItem Text="Html 1" Value="vHtml1" ></asp:MenuItem>
                            <asp:MenuItem Text="Html 2" Value="vHtml2" ></asp:MenuItem>
                            <asp:MenuItem Text="Empty html" Value="vHtmlEmpty"></asp:MenuItem>
                            <asp:MenuItem Text="Sql" Value="vSql"></asp:MenuItem>            
                        </Items>
                        <StaticItemTemplate>
                            <div class="tabCaption">
                                <div class="tabLeft">
                                    <div class="tabRight">
                                        <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                                    </div>
                                </div>
                            </div>
                        </StaticItemTemplate>
                    </asp:Menu>
                    <asp:MultiView ID="mvDane" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vDane" runat="server" >
                            <span class="label2">Id:</span>
                            <asp:TextBox ID="Id" CssClass="form-control num" runat="server"></asp:TextBox>
                            <br />
                            <span class="label2">Nazwa:</span>
                            <asp:TextBox ID="Nazwa" CssClass="form-control edit400" runat="server"></asp:TextBox>
                            <br />
                            <span class="label2">Typ:</span>
                            <asp:TextBox ID="Typ" runat="server" CssClass="form-control num"></asp:TextBox>
                            <br />
                            <span class="label2">Widoczny:</span>
                            <asp:CheckBox ID="Aktywny" CssClass="form-control check" runat="server" />
                            <br />
                            <span class="label2">Nowa linia:</span>
                            <asp:CheckBox ID="NowaLinia" CssClass="form-control check" runat="server" />
                            <br />
                            <span class="label2">Kolejność:</span>
                            <asp:TextBox ID="Kolejnosc" runat="server" CssClass="form-control num"></asp:TextBox>
                            <br />
                            <span class="label2">Mode:</span>
                            <asp:TextBox ID="Mode" runat="server" CssClass="form-control num"></asp:TextBox>
                            <br />
                            <span class="label2">Prawa:</span>
                            <asp:TextBox ID="Rights" CssClass="form-control edit400" runat="server"></asp:TextBox>
                            <br />
                            <span class="label2">OnClick cmd:</span>
                            <asp:TextBox ID="Command" CssClass="form-control edit400" runat="server"></asp:TextBox>
                        </asp:View>
                        <asp:View ID="vCss" runat="server" >
                            <asp:TextBox ID="Css" runat="server" TextMode="MultiLine" Rows="20" CssClass="form-control"></asp:TextBox>                            
                            <span class="label1">Parent css class:</span>
                            <asp:TextBox ID="CssClass" CssClass="form-control" runat="server"></asp:TextBox>
                        </asp:View>
                        <asp:View ID="vScript" runat="server" >
                            <asp:TextBox ID="Script" runat="server" TextMode="MultiLine" Rows="20" CssClass="form-control"></asp:TextBox>                            
                        </asp:View>
                        <asp:View ID="vHtml1" runat="server" >
                            <asp:TextBox ID="Html1" runat="server" TextMode="MultiLine" Rows="20" CssClass="form-control"></asp:TextBox>                            
                        </asp:View>
                        <asp:View ID="vHtml2" runat="server" >
                            <asp:TextBox ID="Html2" runat="server" TextMode="MultiLine" Rows="20" CssClass="form-control"></asp:TextBox>                            
                        </asp:View>
                        <asp:View ID="vHtmlEmpty" runat="server" >
                            <asp:TextBox ID="HtmlEmpty" runat="server" TextMode="MultiLine" Rows="20" CssClass="form-control"></asp:TextBox>                            
                        </asp:View>
                        <asp:View ID="vSql" runat="server" >
                            <asp:TextBox ID="Sql" runat="server" TextMode="MultiLine" Rows="20" CssClass="form-control"></asp:TextBox>                            
                            <br />
                            <span class="label1">Par1:</span>
                            <asp:TextBox ID="Par1" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                            <br />
                            <span class="label1">Par2:</span>
                            <asp:TextBox ID="Par2" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                            <br />
                        </asp:View>
                    </asp:MultiView>



                    <div class="bottom_buttons">
                        <asp:Button ID="btSave" runat="server" CssClass="button75" Text="Zapisz" OnClick="btSave_Click" />
                        <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Anuluj" OnClick="btCancel_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--
    <div id="xxxcntInfoBoxEditZoom" class="modalPopup">
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
    --%>
</div>
