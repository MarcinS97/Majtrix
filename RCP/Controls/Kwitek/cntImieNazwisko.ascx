<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImieNazwisko.ascx.cs" Inherits="HRRcp.Controls.Kwitek.cntImieNazwisko" %>
<%@ Register src="PracUrlop2.ascx" tagname="PracUrlop" tagprefix="uc1" %>
<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>


<asp:DataList ID="dlHeader" runat="server" DataSourceID="SqlDataSource1" 
    CssClass="kwitek_header" onitemdatabound="dlHeader_ItemDataBound">
    <ItemTemplate>
        <div class="ramka urlop">
            <div>
                <div id="paDaneOsobowe" runat="server">
                    <uc1:cntAvatar runat="server" ID="cntAvatar" Width="32px" Height="32px" NrEw='<%# Eval("LpLogo")  %>'  />
                    <asp:Label ID="LpLogoLabel" runat="server" CssClass="nrew" Text='<%# Eval("LpLogo") %>' />&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="ImieLabel" runat="server" cssclass="nazwisko" Text='<%# Eval("Imie") %>' />
                    <asp:Label ID="NazwiskoLabel" runat="server" cssclass="nazwisko" Text='<%# Eval("Nazwisko") %>' /><br />
                    <div id="paPesel" runat="server" visible='<%# IsPeselVisible %>'>
                        <span class="col1">Pesel:</span> <asp:Label ID="PeselLabel" CssClass="pesel" runat="server" Text='<%# Eval("Pesel") %>' /><br />
                    </div>
            </div>
        </div>
    </ItemTemplate>
</asp:DataList>
<br />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" >
</asp:SqlDataSource>




