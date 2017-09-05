<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAlgorytmyPopup.ascx.cs" Inherits="HRRcp.Controls.cntAlgorytmyPopup" %>

<%@ Register src="cntAlgorytmy.ascx" tagname="cntAlgorytmy" tagprefix="uc1" %>

<div runat="server" id="paModalPopup" class="cntModalPopup cntPracParametryPopup" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div>
                <uc1:cntAlgorytmy ID="cntAlgorytmy" runat="server" />
            </div>

            <div class="buttons">   
                <div class="left">
                </div>
                <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Zamknij" onclick="btCancel_Click" />
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>