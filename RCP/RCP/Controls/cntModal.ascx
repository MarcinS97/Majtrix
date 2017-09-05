<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntModal.ascx.cs" Inherits="HRRcp.RCP.Controls.cntModal" %>

<asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="cntModalDiv" runat="server" class="modal fade" role="dialog">
            <div class="modal-dialog" runat="server" id="modalDialog">
                <asp:UpdatePanel ID="upModalInner" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header" runat="server" id="divHeader">
                                <button id="btnXCloseButton" runat="server" type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title"><asp:Literal ID="lblTitle" runat="server"></asp:Literal></h4>
                                <%--<asp:Label ID="lblTitle" runat="server" CssClass="title" />--%>
                                <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                            </div>
                            <div class="modal-body">
                                <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
                            </div>
                            <div class="modal-footer" id="divFooter" runat="server">
                                <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
                                <asp:Button id="btnClose" runat="server" type="button" class="btn btn-default" OnClick="btnClose_Click" Text="Zamknij" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
