<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzeznaczNadg2Popup.ascx.cs" Inherits="HRRcp.Controls.cntPrzeznaczNadg2Popup" %>
<%@ Register src="cntPrzeznaczNadg2.ascx" tagname="cntPrzeznaczNadg2" tagprefix="uc2" %>

<div runat="server" id="paModalPopup" class="cntModalPopup cntRozliczenieNadgodzinPopup" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="scrollbox">
                <uc2:cntPrzeznaczNadg2 ID="cntPrzeznaczNadg21" runat="server" />
            </div>

            <div class="buttons">   
                <div class="left">
                </div>
                <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Zamknij" onclick="btCancel_Click" />
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
