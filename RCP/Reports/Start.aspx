<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="HRRcp.StartPL" %>

<%@ Register src="../Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">

<uc1:cntReport ID="cntReport1" runat="server" 
    Title="Podział kosztów na CC"
    SQL="select 
    R.DataOd as [Od:-],    
    R.DataDo as [Do:-],    
    convert(varchar,DATEPART(YEAR, R.DataDo)) + ' ' + 
    case DATEPART(MONTH, R.DataDo)
	    when 1 then 'styczeń'
	    when 2 then 'luty'
	    when 3 then 'marzec'
	    when 4 then 'kwiecień'
	    when 5 then 'maj'
	    when 6 then 'czerwiec'
	    when 7 then 'lipiec'
	    when 8 then 'sierpień'
	    when 9 then 'wrzesień'
	    when 10 then 'październik'
	    when 11 then 'listopad'
	    when 12 then 'grudzień'
    end as [Okres rozliczeniowy|RepCzasCC @Od @Do|Podział na cc w okresie rozliczeniowym: @Od - @Do],
    convert(varchar(10), R.DataOd, 120) + ' - ' + convert(varchar(10), R.DataDo, 120) as [Okres rozliczeniowy:-], 
    ISNULL(round(sum(S.vCzasZm),0),0) as [CzasZm:N0], 
    ISNULL(round(sum(S.vNadg50),0),0) as [Nadg 50:N0], 
    ISNULL(round(sum(S.vNadg100),0),0) as [Nadg 100:N0],
    ISNULL(round(sum(S.vNocne),0),0) as [Nocne:N0]
    from 
    OkresyRozl R 
    left outer join DaneMPK S on S.Data BETWEEN R.DataOd AND R.DataDo
    where R.DataOd &gt;= '20121121'
    group by R.DataOd, R.DataDo
    order by R.DataOd"
/>





</asp:Content>
