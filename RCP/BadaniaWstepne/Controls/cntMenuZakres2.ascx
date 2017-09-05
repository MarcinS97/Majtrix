<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntMenuZakres2.ascx.cs" Inherits="HRRcp.BadaniaWstepne.Controls.cntMenuZakres2" %>
<%@ Register src="../../Controls/Portal/cntSqlTabs.ascx" tagname="cntSqlTabs" tagprefix="uc1" %>

<div class="tbPracownicy2_topmenu">
<%--
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
--%>    

    
<%--
    <asp:HiddenField ID="hidFirstIdx" runat="server" />
    <uc1:cntSqlTabs ID="Menu1" runat="server" OnSelectTab="cntSqlTabs1_SelectTab" 
        SQL="
select 'Wszystko' as Text, null as Value, 1 as Sort
union all
select distinct convert(varchar(10),Zakr,20) as Text, convert(varchar(10),Zakr,20) as Value, 2 as Sort
from BadaniaWst
union all 
select 'Nowa zakładka', 'ADD', 3 
order by Sort, Zakr
        "/>
--%>
    <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" 
        OnMenuItemClick="Menu1_MenuItemClick" ondatabinding="Menu1_DataBinding">
        <StaticMenuStyle CssClass="tabsStrip mnFilter" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <StaticItemTemplate>
            <div class="tabCaption">
                <div class="tabLeft">
                    <div class="tabRight">
                        <asp:Literal runat="server" ID="Literal1" Text='<%# GetTabText(Container.DataItem) %>' />
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
