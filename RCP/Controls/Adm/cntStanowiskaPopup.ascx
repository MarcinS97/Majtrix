<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntStanowiskaPopup.ascx.cs" Inherits="HRRcp.Controls.cntStanowiskaPopup" %>

<%@ Register src="cntStanowiska.ascx" tagname="cntStanowiska" tagprefix="uc1" %>

<div runat="server" id="paModalPopup" class="cntModalPopup cntPracParametryPopup cntStanowiskaPopup" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div>
                <uc1:cntStanowiska ID="cntStanowiska" runat="server" />
            </div>

            <div class="buttons">   
                <div class="left">
                </div>
                <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Zamknij" onclick="btCancel_Click" />
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>