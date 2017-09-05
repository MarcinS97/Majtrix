<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCarousel.ascx.cs" Inherits="HRRcp.Controls.Portal.cntCarousel" %>

<div id="paCarousel" runat="server" class="cntCarousel">    
    <asp:HiddenField ID="hidMenu" runat="server" Visible="false"/>
    <div id='<%= GetCId() %>' >            
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" 
            onitemcommand="Repeater1_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton ID="lbtItem" runat="server" 
                PostBackUrl='<%# GetUrl(Eval("Command")) %>' 
                >
                    <asp:Image ID="img" runat="server" CssClass="cloudcarousel"
                        ToolTip='<%# Eval("MenuText") %>' 
                        AlternateText='<%# Eval("ToolTip") %>' 
                        ImageUrl='<%# GetPath(Eval("Image")) %>' 
                    />
                </asp:LinkButton>
            </ItemTemplate>
        </asp:Repeater>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
            SelectCommand="SELECT * FROM [SqlMenu] WHERE ([Grupa] = @Grupa) ORDER BY [Kolejnosc]">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidMenu" Name="Grupa" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    
        <%--    
        CommandName="click" CommandArgument='<%# Eval("Command") %>' 
                    PostBackUrl='<%# GetUrl(Eval("Command")) %>' 
                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl='<%# Eval("Command2") %>' >
                SelectCommand="SELECT *, substring(Command,5,100) as Command2 FROM [SqlMenu] WHERE ([Grupa] = @Grupa) ORDER BY [Kolejnosc]">


        <a href="http://jabil4u/prp" >
            <img class="cloudcarousel" src="images/portal/3/prp.png" 
                alt="" 
                title="Program Rozwoju Pracowników" />
        </a>
        <a href="http://jabil4u/rcp" >
            <img class = "cloudcarousel" src="images/portal/3/rcp.png" 
                alt="" 
                title="Rejestracja Czasu Pracy" />
        </a>
        <a href="http://jgbapp04/zam" >
            <img class = "cloudcarousel" src="images/portal/3/zam.png" 
                alt="" 
                title="System zamówień wewnętrznych" />
        </a>
        <a href="http://jabil4u/prp" >
            <img class="cloudcarousel" src="images/portal/3/mat.png" 
                alt="" 
                title="Matryca Elastyczności" />
        </a>
        <a href="http://jabil4u/rcp" >
            <img class = "cloudcarousel" src="images/portal/3/rpp.png" 
                alt="" 
                title="Racjonalizatorski Program Punktowy" />
        </a>
        <a href="http://jgbapp04/helpdesk" >
            <img class = "cloudcarousel" src="images/portal/3/hlp.png" 
                alt="" 
                title="IT Helpdesk" />
        </a>
        --%>

    </div>
    <asp:Panel ID="paNavigator" runat="server" CssClass="carousel_navigator" Width="980px" Style="margin:auto" HorizontalAlign="Center">
        <input id="right-but" type="button" value="◄"  />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input id="left-but" type="button" value="►" />
        <p id="title-text" align="center" style="height: 18px; width: 980px"> </p>
        <p id="alt-text" align="center" style="width: 980px; height: 18px;"> </p>
    </asp:Panel>
</div>
