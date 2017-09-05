<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOgloszeniaBoard.ascx.cs" Inherits="HRRcp.Portal.Controls.cntOgloszeniaBoard" %>
<%@ Register Src="cntOgloszenia.ascx" TagName="cntOgloszenia" TagPrefix="uc1" %>
<%@ Register src="cntOgloszeniaKategorie.ascx" tagname="cntOgloszeniaKategorie" tagprefix="uc2" %>
<%@ Register Src="cntOgloszeniaParametry.ascx" TagName="cntOgloszeniaParametry" TagPrefix="uc1" %>
<%@ Register Src="cntOgloszenieEdit.ascx" TagPrefix="uc1" TagName="cntOgloszenieEdit" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register src="~/Controls/Portal/cntArticles4.ascx" tagname="cntArticles" tagprefix="uc1" %>

<div id="paOgloszeniaBoard" runat="server" class="cntOgloszeniaBoard">
    <div class="panel-group" id="accordion">
    
        <div class="panel panel-default doakceptacji" id="paDoAkceptacji" runat="server" visible="false" >
            <div class="panel-heading">
                <h4 class="panel-title">
                    <i class="glyphicon glyphicon-ok"></i>
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">
                        <asp:Label ID="lbTitle1" runat="server" Text="Ogłoszenia do akceptacji"></asp:Label>                         
                    </a>                   
                    &nbsp;
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline" >
                        <ContentTemplate>
                            <asp:Label ID="lbCount1" CssClass="count" runat="server" ></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </h4>
            </div>
            <div id="collapse1" class="panel-collapse collapse<%= RozwinCss(1) %>">
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <uc1:cntOgloszenia ID="cntDoAkceptacji" runat="server" Mode="1" OnDataBound="cntDoAkceptacji_DataBound" OnRefresh="cntOgloszenia_Refresh" OnEdit="cntOgloszenia_Edit"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <i class="glyphicon glyphicon-eye-open"></i>
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">
                        <asp:Label ID="Label1" runat="server" Text="Aktualne"></asp:Label>                         
                    </a>
                </h4>
            </div>
            <div id="collapse2" class="panel-collapse collapse<%= RozwinCss(0) %>">
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <uc1:cntOgloszenia ID="cntOgloszenia" runat="server" Mode="0" OnRefresh="cntOgloszenia_Refresh" OnEdit="cntOgloszenia_Edit"/>                
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        
        <div class="panel panel-default" id="paMoje" runat="server" visible="true" >
            <div class="panel-heading">
                <h4 class="panel-title">
                    <i class="glyphicon glyphicon glyphicon-user"></i>
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">
                        <asp:Label ID="Label2" runat="server" Text="Moje ogłoszenia"></asp:Label>                         
                    </a>
                </h4>
            </div>
            <div id="collapse3" class="panel-collapse collapse<%= RozwinCss(2) %>">
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <uc1:cntOgloszenia ID="cntMoje" runat="server" Mode="2" OnRefresh="cntOgloszenia_Refresh" OnEdit="cntOgloszenia_Edit"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        
        <div class="panel panel-default" id="paArchiwum" runat="server" visible="false" >
            <div class="panel-heading">
                <h4 class="panel-title">
                    <i class="glyphicon glyphicon-floppy-disk"></i>
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse4">
                        <asp:Label ID="Label3" runat="server" Text="Archiwum"></asp:Label>                         
                    </a>
                </h4>
            </div>
            <div id="collapse4" class="panel-collapse collapse<%= RozwinCss(3) %>">
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <uc1:cntOgloszenia ID="cntArchiwum" runat="server" Mode="3" OnRefresh="cntOgloszenia_Refresh" OnEdit="cntOgloszenia_Edit"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="panel panel-default" id="paAdministracja" runat="server" visible="false" >
            <div class="panel-heading">
                <h4 class="panel-title">
                    <i class="glyphicon glyphicon-cog"></i>
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse5">
                        <asp:Label ID="Label4" runat="server" Text="Administracja"></asp:Label>                         
                    </a>                   
                </h4>
            </div>
            <div id="collapse5" class="panel-collapse collapse<%= RozwinCss(99) %>">
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                        <ContentTemplate>
                            <uc2:cntOgloszeniaKategorie ID="cntOgloszeniaKategorie" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" >
                        <ContentTemplate>
                            <uc1:cntOgloszeniaParametry runat="server" id="cntOgloszeniaParametry" OnChanged="cntOgloszeniaParametry_Changed" OnRegulamin="cntOgloszeniaParametry_Regulamin"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <uc1:cntOgloszenieEdit runat="server" id="cntOgloszenieEdit" OnSave="cntOgloszenieEdit_Save" OnRegulamin="cntOgloszenieEdit_Regulamin"/>
    <%-- na razie nie można modala na modalu pokazać
    <uc1:cntModal runat="server" id="cntRegulamin" Title="Regulamin" CssClass="regulamin" Backdrop="false" Keyboard="false" >
        <ContentTemplate>
            <uc1:cntArticles ID="cntArticles" runat="server" Mode="0" Grupa="OGLREGULAMIN" PageSize="0" />
        </ContentTemplate>
    </uc1:cntModal>
    --%>
    <%-- problem jest przy zamykaniu - wyskakuje błąd z asyncfileupload mimo ze nie było nic ładowane, nie działa z cntModal - edycja ogłoszenia
    <asp:UpdatePanel ID="upRegulamin" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <div id="paRegulamin" style="display:none;" class="modalPopup paRegulamin">
                <uc1:cntArticles ID="cntArticles" runat="server" Mode="0" Grupa="OGLREGULAMIN" PageSize="0" />
                <div class="buttons">
                    <asp:LinkButton ID="btCloseRegulamin" runat="server" CssClass="btn btn-default">Zamknij</asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    --%>
    <div id="zoomImage" class="zoomImage modal fade" style="display: none;">
        <img id="zoomImg" src="#"/>
        <span id="zoomClose" >&times;</span>
    </div>
</div>
