<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderControl.ascx.cs" Inherits="HRRcp.Controls.HeaderControl" %>

<div id="header">
	<div class="center">

        <div class="topLine">
            <div id="program">
                <asp:Label ID="lbProgram" runat="server" ></asp:Label>
            </div>
            <div id="user">
                <asp:Literal ID="ltWelcome" runat="server" Text="Witaj:"></asp:Literal>
                <asp:Label ID="lbUser" runat="server" Text="Niezalogowany" ></asp:Label>
            </div>
        </div>

        <div id="paLogo" class="logo" runat="server" visible="false">
            <asp:LinkButton ID="lbtLogo" runat="server" onclick="lbtLogo_Click">
	            <asp:Image ID="imgLogo" ImageUrl="~/images/RepLogo155x45.png" runat="server" />    		       
	        </asp:LinkButton>
	    </div>
        
        <div id="topMenuContainer" runat="server" class="topMenuContainer">
            <div id="topInfo" class="topInfo" runat="server">
                <asp:Literal ID="ltTopInfo" runat="server"></asp:Literal>
            </div>
        </div>                
              
	</div>
</div>





