<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CCHeader.ascx.cs" Inherits="HRRcp.Controls.CCHeader" %>

<asp:HiddenField ID="hidData" runat="server" />

<asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
    <ItemTemplate>
        <asp:Label ID="lbNazwa" runat="server" Text='<%# Eval("Nazwa") %>' Visible="false"></asp:Label>
        <asp:Label ID="lbCC" runat="server" Text='<%# Eval("cc") %>'></asp:Label>
    </ItemTemplate>
    <SeparatorTemplate>
        </th><th class="suma">
    </SeparatorTemplate>
</asp:Repeater>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT [Id], [cc], [Nazwa], [Grupa] FROM [CC] WHERE @Data between AktywneOd and ISNULL(AktywneDo, @Data) ORDER BY [cc]">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>

