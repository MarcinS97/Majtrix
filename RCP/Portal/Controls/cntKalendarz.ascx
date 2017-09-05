<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKalendarz.ascx.cs" Inherits="HRRcp.Portal.Controls.cntKalendarz" %>
<asp:Button ID="btnAsd" runat="server" Text="test" OnClick="btnAsd_Click" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click"  Text="Test do sciezki" />
    <div id="calendar">
     <div id="modal"></div>
    </div>

<asp:SqlDataSource ID="dsCalendar" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="
select
  *
from poKalendarz
where IdPracownika = 2168
"></asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL%>" SelectCommand="
select
  *
from PlikiSciezki
"
    InsertCommand="
insert Pliki (IdSciezki, NazwaPliku{2})
select {0}, '{1}'{3}
"></asp:SqlDataSource>
