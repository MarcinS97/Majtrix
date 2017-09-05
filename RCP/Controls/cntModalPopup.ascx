<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntModalPopup.ascx.cs" Inherits="HRRcp.Controls.cntModalPopup" %>

<div runat="server" id="paModalPopup" class="cntModalPopup" style="display:none;" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div>
                Hello World!
            </div>

            <div class="buttons">   
                <div class="left">
                    <asp:Button ID="btOption1" runat="server" CssClass="button120" Text="Opcja" onclick="btOption_Click" />
                </div>
                <asp:Button ID="btUpdate" runat="server" CssClass="button75" Text="Zapisz" onclick="btUpdate_Click" ValidationGroup="vgupdate"/>
                <asp:Button ID="btCancel" runat="server" CssClass="button75" Text="Anuluj" onclick="btCancel_Click" />
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>