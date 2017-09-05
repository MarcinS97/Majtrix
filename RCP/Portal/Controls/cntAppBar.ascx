<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAppBar.ascx.cs" Inherits="HRRcp.Portal.Controls.cntAppBar" %>

<div id="appBar">
    <asp:HiddenField ID="hidGroup" runat="server" Visible="false" />
    <asp:Repeater ID="rpApps" runat="server" DataSourceID="dsApps">
        <ItemTemplate>
            <%--<div class="app-box">--%>
                <asp:LinkButton ID="lnkApp" runat="server" CssClass="xapp-box-inner app-link row" CommandArgument='<%# Eval("Command") %>'
                    OnClick="lnkApp_Click" >
                    <span class="col-left" Style='<%# "background-color: " + Eval("Par2") %>'>
                        <i class='<%# Eval("Image") %>'></i>
                    </span>
                    <span class="col-right" Style='<%# "background-color: " + Eval("Par2") %>'>
                        <span class="title"><%# Eval("MenuText") %></span>          
                    </span> 
                </asp:LinkButton>
            <%--</div>--%>
        </ItemTemplate>
    </asp:Repeater>
    <asp:SqlDataSource ID="dsApps" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
        SelectCommand="select * from SqlMenu where Grupa = 'APP' + @group and Aktywny = 1  order by Kolejnosc">
        <SelectParameters>
            <asp:ControlParameter Name="group" Type="String" ControlID="hidGroup" PropertyName="Value" DefaultValue="PRAC" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>
