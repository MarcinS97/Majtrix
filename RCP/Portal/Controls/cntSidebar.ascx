<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSidebar.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSidebar" %>
<%@ Register Src="~/Portal/Controls/cntSidebarEditModal.ascx" TagPrefix="uc1" TagName="cntSidebarEditModal" %>
<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSidebar" runat="server" class="sidebar sidebar-left">
            <div class="user">
                <uc1:cntAvatar runat="server" id="cntAvatar" />
                <asp:LinkButton ID="lbUserName" runat="server" OnClick="lbUserName_Click" CssClass="name" />
            </div>
            <nav>
                <div id="Div1" runat="server" class="sidebar-title" visible="false" >
                    <asp:Literal ID="Literal1" runat="server" Text="Ubezpieczenia"></asp:Literal>
                </div>
                <div id="paTitlePortal" runat="server" class="sidebar-title" visible="false" >
                    <asp:Literal ID="litTitle" runat="server"></asp:Literal>
                </div>
                <ul class="nav nav-pills nav-stacked">
                    <asp:Repeater ID="rpLeftMenu" runat="server" DataSourceID="dsLeftMenu">
                        <ItemTemplate>
                            <%# Eval("_magic") %>
                            <asp:LinkButton ID="lnkLeftMenuRedirect" runat="server" Text='<%# Eval("MenuText") %>'
                                CommandArgument='<%# Eval("Command") + ";" + Eval("Id") %>' Visible='<%# (int)Eval("Avis") == 0 %>'
                                CssClass='<%# GetLeftMenuSelectedClass(Eval("Id").ToString()) +  Eval("Class") %>'
                                OnClick="lnkLeftMenuRedirect_Click" OnClientClick="return true;" />
                            <%# Eval("_magic2") %>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div id="divSpolecznosc" runat="server" visible="false">
                    <div class="sidebar-title">
                        Społeczność
                    </div>
                    <ul class="nav nav-pills nav-stacked">
                        <asp:Repeater ID="rpLeftMenuSocial" runat="server" DataSourceID="dsLeftMenuSocial">
                            <ItemTemplate>
                                <%# Eval("_magic") %>
                                <asp:LinkButton ID="lnkLeftMenuRedirect" runat="server" Text='<%# Eval("MenuText") %>'
                                    CommandArgument='<%# Eval("Command") + ";" + Eval("Id") %>' Visible='<%# (int)Eval("Avis") == 0 %>'
                                    CssClass='<%# GetLeftMenuSelectedClass(Eval("Id").ToString()) +  Eval("Class") %>'
                                    OnClick="lnkLeftMenuRedirect_Click" OnClientClick="return true;" />
                                <%# Eval("_magic2") %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>

                <asp:HiddenField ID="hidGroup" runat="server" Visible="false" />
                <asp:SqlDataSource ID="dsLeftMenu" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" OnSelecting="dsLeftMenu_Selecting"
                    SelectCommand="select * from fn_GetMenu3('LEFTMENU' + @group, @ids) order by Sort">
                    <SelectParameters>
                        <asp:Parameter Name="ids" Type="String" />
                        <asp:ControlParameter Name="group" Type="String" ControlID="hidGroup" PropertyName="Value" DefaultValue="PRAC" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="dsLeftMenuSocial" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" OnSelecting="dsLeftMenuSocial_Selecting"
                    SelectCommand="select * from fn_GetMenu3('LEFTMENUSOCIAL', @ids) order by Sort">
                    <SelectParameters>
                        <asp:Parameter Name="ids" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="dsLeftMenuItems" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="select * from SqlMenu where Grupa = 'LEFTMENU' + '{0}'"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dsLeftMenuSocialItems" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="select * from SqlMenu where Grupa = 'LEFTMENUSOCIAL'" />

            </nav>
            <div class="sidebar-edit-area">
                <asp:Label ID="lblMode" runat="server" CssClass="lblEditMode" Visible="false" Text="Tryb edycji" />
                <div class="buttons pull-right">
                    <asp:LinkButton ID="lnkAddNewItem" runat="server" CssClass="btn-small btn-success" OnClick="lnkAddNewItem_Click" Visible="false">
                    <i class="fa fa-plus"></i>
                    </asp:LinkButton>

                    <asp:LinkButton ID="lnkSidebarEditMode" runat="server" CssClass="btn-small btn-primary" OnClick="lnkSidebarEditMode_Click" Visible="false">
                    <i class="fa fa-pencil"></i>
                    </asp:LinkButton>

                    <asp:LinkButton ID="lnkSidebarEditModeCancel" runat="server" CssClass="btn-small btn-default" OnClick="lnkSidebarEditModeCancel_Click" Visible="false">
                    <i class="fa fa-ban"></i>
                    </asp:LinkButton>
                </div>
            </div>
            <div style="position: absolute; bottom: 0; left: 50%; transform: translateX(-50%); color: #aaa; font-size: 24px; text-align: center; width: 240px; border-top: solid 1px #eee; padding: 8px; display: none;">
                <i class="fa fa-clock-o" style="margin-right: 8px; color: #aaa;"></i>16:14:32
            </div>
            <%--<img class="sidebar-bottom-thing" runat="server" src="~/Portal/Styles/robot.png" />--%>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<uc1:cntSidebarEditModal runat="server" id="cntSidebarEditModal" onsaved="cntSidebarEditModal_Saved" visible="false" />
