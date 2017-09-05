<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAplikacjeMenu.ascx.cs" Inherits="HRRcp.Controls.Portal.cntAplikacjeMenu" %>

<div id="paAplikacjeMenu" runat="server" class="cntAplikacjeMenu">    
    <asp:HiddenField ID="hidMenu" runat="server" Visible="false"/>
<%--
    <div id='< % = GetCId() %>' >            
--%>
    <div>
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" 
            onitemcommand="Repeater1_ItemCommand" 
            onitemdatabound="Repeater1_ItemDataBound">
            <ItemTemplate>
<%--
                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="click">
                    <div class="img">
                        <asp:Image ID="Image1" runat="server" CssClass="img"
                        />
                    </div>            
                    <div class="text">
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("MenuText") %>'></asp:Label>
                    </div>
                </asp:LinkButton>
                
                
--%>                
<%--
                <asp:HyperLink ID="HyperLink1" runat="server"  NavigateUrl='<%# GetUrl(Eval("Command")) %>'>
                    <div class="img">
                        <asp:Image ID="img" runat="server" CssClass="img"
                            ToolTip='<%# Eval("MenuText") %>' 
                            AlternateText='<%# Eval("ToolTip") %>' 
                            ImageUrl='<%# GetPath(Eval("Image")) %>' 
                        />
                    </div>
                    <div class="text">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("MenuText") %>'></asp:Label>
                    </div>
                </asp:HyperLink>
                
                
                PostBackUrl='<%# GetUrl(Eval("Command")) %>'
--%>                
                <asp:LinkButton ID="lbtItem" runat="server" CommandName="click" CommandArgument='<%# GetUrl(Eval("Command")) %>' >
                    <div class="img">
                        <asp:Image ID="img" runat="server" CssClass="img"
                            ToolTip='<%# Eval("MenuText") %>' 
                            AlternateText='<%# Eval("ToolTip") %>' 
                            ImageUrl='<%# GetPath(Eval("Image")) %>' 
                        />
                    </div>
                    <div class="text">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("MenuText") %>'></asp:Label>
                    </div>
                </asp:LinkButton>

            </ItemTemplate>
        </asp:Repeater>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
            SelectCommand="SELECT * FROM [SqlMenu] WHERE ([Grupa] = @Grupa) ORDER BY [Kolejnosc]">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidMenu" Name="Grupa" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</div>
