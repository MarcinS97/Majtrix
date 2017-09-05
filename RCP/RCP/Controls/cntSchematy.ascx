<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSchematy.ascx.cs" Inherits="HRRcp.RCP.cntSchematy" %>

<%--
<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
--%>

<asp:HiddenField ID="hidSelectedValue" runat="server" Visible="false" />

        <div class="list-group">
            <asp:Repeater ID="rpSchemes" runat="server" DataSourceID="dsSchemes">
                <ItemTemplate>
                    <asp:HiddenField ID="hidID" runat="server" Visible="false" Value='<%# Eval("Id") %>' />
                    <asp:LinkButton ID="lnkScheme" runat="server" Text='<%# Eval("Nazwa") %>' CssClass='<%# GetClass(Eval("Id")) %>'
                        CommandArgument='<%# Eval("Id") %>' OnClick="lnkScheme_Click" Enabled='<%# EditMode %>' />
                </ItemTemplate>
            </asp:Repeater>
            </div>
<asp:SqlDataSource ID="dsSchemes" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select * 
from rcpSchematy 
where Aktywny = 1 
order by Kolejnosc, Nazwa" />

