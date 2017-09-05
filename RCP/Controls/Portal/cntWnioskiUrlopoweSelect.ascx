<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioskiUrlopoweSelect.ascx.cs" Inherits="HRRcp.Controls.Portal.cntWnioskiUrlopoweSelect" %>

<div id="paWnioskiUrlopoweSelect" runat="server" class="cntWnioskiUrlopoweSelect">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" 
        onitemcommand="Repeater1_ItemCommand" >
        <ItemTemplate><asp:LinkButton ID="LinkButton1" runat="server" 
                CommandName="select"
                CommandArgument='<%# Eval("Id") %>' ><table><tr><td><%# Eval("Typ") %></td></tr></table>
        </asp:LinkButton></ItemTemplate>   <%--spacje--%>
    </asp:Repeater>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat ="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT * FROM [poWnioskiUrlopoweTypy] 
        WHERE Aktywny = 1 and ( 
            @mode = 0 and WypelniaPracownik = 1 or
            @mode = 1 and WypelniaKierownik = 1
            )
        ORDER BY [Kolejnosc], [Typ]">
    <SelectParameters>
        <asp:Parameter Name="mode" Type="Int32" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>
