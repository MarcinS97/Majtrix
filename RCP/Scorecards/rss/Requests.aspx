<%@ Page Title="" Language="C#" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="HRRcp.Scorecards.rss.Requests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:SqlDataSource ID="dsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select
  '&lt;![CDATA[' + ISNULL(ISNULL(ta.Nazwa, w.Nazwa), 'NULL') + ' (' + LEFT(CONVERT(varchar, w.Data, 20), 7) + ')' + ']]&gt;' title
, 'http://localhost:50675/Scorecards/Wnioski.aspx' link
, CONVERT(varchar, w.Id) guid
, 'To cos daje?' comments
, CONVERT(varchar, w.DataUtworzenia, 20) pubDate
, dbo.GetStatus(w.Status, w.Kacc, w.Pacc) description
from scWnioski w
left join scTypyArkuszy ta on w.IdTypuArkuszy = ta.Id
where w.IdPracownika = {0}
order by pubDate desc
">
</asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
