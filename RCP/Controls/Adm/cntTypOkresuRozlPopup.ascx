<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTypOkresuRozlPopup.ascx.cs" Inherits="HRRcp.Controls.cntTypOkresuRozlPopup" %>
<%@ Register src="cntTypOkresuRozl.ascx" tagname="cntTypOkresuRozl" tagprefix="uc1" %>

<div runat="server" id="paModalPopup" class="cntModalPopup cntTypOkresuRozlPopup" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div>
                <uc1:cntTypOkresuRozl ID="cntTypOkresuRozl" runat="server" />
            </div>

            <div class="buttons">   
                <div class="left">
                </div>
                <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Zamknij" onclick="btCancel_Click" />
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>