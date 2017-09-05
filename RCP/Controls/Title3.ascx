<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Title3.ascx.cs" Inherits="HRRcp.Controls.Title3" %>

<div class="title3">
    <asp:Image ID="Image1" CssClass="ico" runat="server" />
    <span class="captions">
        <asp:Label ID="lbTitle" CssClass="title" runat="server" ></asp:Label>    
        <span id="sub1" runat="server" visible="false" class="subtitle subtitle1">
            <asp:Label ID="lbSub1" CssClass="caption" runat="server"></asp:Label>    
            <asp:Label ID="lbValue1" CssClass="value" runat="server"></asp:Label>    
        </span>
        <span id="sub2" runat="server" visible="false" class="subtitle subtitle2">
            <asp:Label ID="lbSub2" CssClass="caption" runat="server"></asp:Label>    
            <asp:Label ID="lbValue2" CssClass="value" runat="server"></asp:Label>    
        </span>
        <span id="sub3" runat="server" visible="false" class="subtitle subtitle3">
            <asp:Label ID="lbSub3" CssClass="caption" runat="server"></asp:Label>    
            <asp:Label ID="lbValue3" CssClass="value" runat="server"></asp:Label>    
        </span>
    </span>
</div>