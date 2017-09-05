<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntInfoBox.ascx.cs" Inherits="HRRcp.Controls.cntInfoBox" %>

<asp:Literal ID="br" runat="server" visible="false"><br /></asp:Literal>
<div id="paInfoBox" runat="server" class="cntInfoBox">
    <asp:Literal ID="ltWaiting" runat="server" Visible="false">
        <div class="defloading">
            <img src="images/uploading.gif" title="Ładowanie zawartości..." />
            <%--            
            <span>Ładowanie...</span>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/uploading.gif" ToolTip="Ładowanie zawartości..."/>
                ▲►▼◄←↑→↓
            --%>
        </div>
    </asp:Literal>
    <asp:Literal ID="ltCss" runat="server"></asp:Literal>
    <asp:Literal ID="ltScript" runat="server"></asp:Literal>
    <asp:Button ID="btGet" runat="server" Text="Get" CssClass="button_postback" OnClick="btGet_Click"/>
    <asp:LinkButton ID="lbtBox" runat="server" OnClick="lbtBox_Click" CommandName="BOXCLICK">
        <div id="pa1" runat="server" class="infobox infobox1">    
            <asp:Literal ID="html1" runat="server"></asp:Literal>
        </div>
        <div id="pa2" runat="server" class="infobox infobox2">
            <asp:Literal ID="html2" runat="server"></asp:Literal>
        </div>
        <div id="paEdit" runat="server" class="infoboxedit printoff" visible="true">
            <div class="editopen xtriangleD16 triangleR16 xdiamond16 xpacman16"></div>        
            <asp:Label ID="lbInfo" runat="server" CssClass="info" Visible="false" ></asp:Label>
            <asp:Button ID="btEdit" runat="server" CssClass="button button_tech first" Text="Edytuj" OnClick="btEdit_Click" CommandName="BOXEDIT"/>
            <asp:Button ID="btNew" runat="server" CssClass="button button_tech" Text="Dodaj" OnClick="btNew_Click" CommandName="BOXNEW"/>
            <asp:Button ID="btDelete" runat="server" CssClass="button button_tech" Text="Usuń" OnClick="btDelete_Click" CommandName="BOXDELETE"/>                    
            <asp:Button ID="btPrev" runat="server" CssClass="button button_tech" Text="◄" CommandName="BOXPREV"/>                    
            <asp:Button ID="btNext" runat="server" CssClass="button button_tech" Text="►" CommandName="BOXNEXT"/>                    
            <asp:Button ID="btUp" runat="server" CssClass="button button_tech" Text="▲" CommandName="BOXUP"/>                    
            <asp:Button ID="btDown" runat="server" CssClass="button button_tech" Text="▼" CommandName="BOXDOWN"/>                    
            <asp:Button ID="btShowAll" runat="server" CssClass="button button_tech" Text="All" CommandName="BOXSHOWALL"/>                    
        </div>
    </asp:LinkButton>
</div>