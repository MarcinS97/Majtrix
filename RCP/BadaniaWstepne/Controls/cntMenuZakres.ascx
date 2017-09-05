<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntMenuZakres.ascx.cs" Inherits="HRRcp.BadaniaWstepne.Controls.cntMenuZakres" %>
<div class="tbPracownicy2_topmenu">
    <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" OnMenuItemClick="Menu1_MenuItemClick">
        <StaticMenuStyle CssClass="tabsStrip mnFilter" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <StaticItemTemplate>
            <div class="tabCaption">
                <div class="tabLeft">
                    <div class="tabRight">
                        <asp:Literal runat="server" ID="Literal1" Text='<%# GetTextFromMVal(Container.DataItem) %>' />
                    </div>
                </div>
            </div>
        </StaticItemTemplate>
    </asp:Menu>
    <asp:Menu ID="tabContent" runat="server" Orientation="Horizontal" OnMenuItemClick="tabContent_MenuItemClick">
        <StaticMenuStyle CssClass="tabsStrip mnContent" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticHoverStyle CssClass="tabHover" />
        <Items>
            <asp:MenuItem Text="◄◄" Value="-2" />
            <asp:MenuItem Text="◄" Value="-1" />
            <asp:MenuItem Value="0" />
            <asp:MenuItem Text="►" Value="1" />
            <asp:MenuItem Text="►►" Value="2" />
        </Items>
        <StaticItemTemplate>
            <div class="tabCaption">
                <div class="tabLeft">
                    <div class="tabRight">
                        <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                    </div>
                </div>
            </div>
        </StaticItemTemplate>
    </asp:Menu>
</div>
