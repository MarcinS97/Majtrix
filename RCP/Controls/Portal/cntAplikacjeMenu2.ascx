<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAplikacjeMenu2.ascx.cs" Inherits="HRRcp.Controls.Portal.cntAplikacjeMenu2" %>

<div id="paAplikacjeMenu" runat="server" class="cntAplikacjeMenu">    
    <asp:HiddenField ID="hidMenu" runat="server" Visible="false"/>
    
    <asp:ListView ID="lvMenu" runat="server" DataSourceID="SqlDataSource1" 
        DataKeyNames="Id" onitemcommand="lvMenu_ItemCommand" 
        onitemdatabound="lvMenu_ItemDataBound">
        <ItemTemplate>
        <%--
PostBackUrl='<%# GetUrl(Eval("Command")) %>'         



testy ...
        --%>
            <asp:LinkButton ID="lbtItem" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="click">
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
        <LayoutTemplate>
            <div ID="itemPlaceholderContainer" runat="server" style="">
                <span ID="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
    </asp:ListView>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
        SelectCommand="SELECT * FROM [SqlMenu] WHERE ([Grupa] = @Grupa) ORDER BY [Kolejnosc]">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidMenu" Name="Grupa" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>