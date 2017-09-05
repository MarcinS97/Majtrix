<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKartyRcpPopup.ascx.cs" Inherits="HRRcp.Controls.cntKartyRcpPopup" %>

<%@ Register src="cntKartyRcp.ascx" tagname="cntKartyRcp" tagprefix="uc1" %>

<div runat="server" id="paModalPopup" class="cntModalPopup cntPracParametryPopup" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div>
                <uc1:cntKartyRcp ID="cntKartyRcp1" runat="server" />
            </div>
            <div class="buttons">   
                <div class="left">
                </div>
                <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Zamknij" onclick="btCancel_Click" Visible="false"/>
                <asp:Button ID="btCancel2" runat="server" CssClass="button75" Text="Zamknij" onclick="btCancel2_Click" />
<%--
                <asp:Button ID="btUpdate" runat="server" CssClass="button_postback" onclick="btUpdate_Click" />
--%>            
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>