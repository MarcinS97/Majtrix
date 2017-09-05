<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntParameters.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntParameters" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntTypes.ascx" TagPrefix="leet" TagName="Types" %>

<div id="ctParametersMenu" runat="server" class="cntParametersMenu">
    <table class="tabsContent" >
        <tr>
            <td class="LeftMenu">
                <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                    <StaticMenuStyle CssClass="menu" />
                    <StaticSelectedStyle CssClass="selected" />
                    <StaticMenuItemStyle CssClass="item" />
                    <StaticHoverStyle CssClass="hover" />
                    <Items>
                        <asp:MenuItem Text="Słowniki rodzajów" Value="vMY" ></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </td>                            
            <td class="LeftMenuContent">
                <asp:MultiView ID="mvWnioski" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vMY" runat="server" >
                        <leet:Types Id="Types" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>
</div>