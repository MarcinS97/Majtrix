<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImportABSENCJA.ascx.cs" Inherits="HRRcp.Controls.cntImportABSENCJA" %>

<script type="text/javascript">
    function <%=ClientID%>_chk() {
        fu = document.getElementById('<%=FileUpload1.ClientID%>');
        if (fu != null) {
            if (!fu.value) {
                alert("Brak pliku do importu.");
                return false;
            }
        }
        return true;
    }
</script>

<div class="fileupload round5 cntImportALARMUS">
    <asp:FileUpload ID="FileUpload1" CssClass="fileupload" runat="server" />
    <asp:Button ID="btImport" runat="server" CssClass="button200" Text="Import absencji" onclick="btImport_Click" OnClientClick="javascript:if ({0}_chk()) {showAjaxProgress();return true;} else return false;" />
<%--
    <asp:Button ID="Button1" runat="server" CssClass="button200" Text="Import absencji" onclick="btImport_Click" OnClientClick="javascript:if (checkFile()) {showAjaxProgress();return true;} else return false;" />
--%>
    <span class="info">Import za rok:</span>
    <asp:DropDownList ID="ddlMiesiac" runat="server" DataSourceID="SqlDataSource1" DataTextField="Data" DataValueField="Value"></asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
declare @today datetime = GETDATE()
select CONVERT(varchar(4), rok, 20) as Data, CONVERT(varchar(10), dbo.boy(rok), 20) + '|' + CONVERT(varchar(10), dbo.eoy(rok), 20) as Value
from
(
select DATEADD(YEAR, -Lp, dbo.boy(dbo.getdate(@today))) as rok from dbo.GetDates2('20000101',DATEADD(D, YEAR(@today) - 2015, '20000101'))
) D
        ">
    </asp:SqlDataSource>
</div> 

<%--
    <span class="info">Import za miesiąc:</span><asp:DropDownList ID="ddlMiesiac" runat="server" DataSourceID="SqlDataSource1" DataTextField="Data" DataValueField="Value"></asp:DropDownList>

select CONVERT(varchar(7), DataDo, 20) as Data, CONVERT(varchar(10), dbo.bom(DataDo), 20) + '|' + CONVERT(varchar(10), dbo.eom(DataDo), 20) as Value
from
(
select DATEADD(M, -Lp, dbo.eom(dbo.getdate(GETDATE()))) as DataDo 
from dbo.GetDates2('20000101','20000112')
) D


select top 12 CONVERT(varchar(7), DataDo, 20) as Data, CONVERT(varchar(10), dbo.bom(DataDo), 20) + '|' + CONVERT(varchar(10), dbo.eom(DataDo), 20) as Value
from OkresyRozl 
order by DataOd desc
--%>