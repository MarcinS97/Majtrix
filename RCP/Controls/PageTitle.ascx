<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTitle.ascx.cs" Inherits="HRRcp.Controls.PageTitle" %>
<%--
<div class="pagetitle">
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
--%>
<div class="pagetitle">
    <table>
        <tr class="firstrow">
            <td rowspan="4" class="ico">
                <asp:Image ID="Image1" runat="server" />
                <div class="distance"></div>   
            </td>
            <td colspan="2" class="title">
                <asp:Label ID="lbTitle" runat="server" ></asp:Label>
            </td>
            <td rowspan="4" class="rightico">
                <asp:Image ID="Image2" runat="server" Visible="false"/>
            </td>
        </tr>    
        <tr>
            <td class="caption">
                <asp:Label ID="lbSub1" CssClass="caption" runat="server"></asp:Label>    
            </td>
            <td class="value">
                <asp:Label ID="lbValue1" CssClass="value" runat="server"></asp:Label>    
            </td>
        </tr>
        <tr>
            <td class="caption">
                <asp:Label ID="lbSub2" CssClass="caption" runat="server"></asp:Label>    
            </td>
            <td class="value">
                <asp:Label ID="lbValue2" CssClass="value" runat="server"></asp:Label>    
            </td>
        </tr>
        <tr class="lastrow">
            <td class="caption">
                <asp:Label ID="lbSub3" CssClass="caption" runat="server"></asp:Label>    
            </td>
            <td class="value">
                <asp:Label ID="lbValue3" CssClass="value" runat="server"></asp:Label>    
            </td>
        </tr>
    </table>
</div>
