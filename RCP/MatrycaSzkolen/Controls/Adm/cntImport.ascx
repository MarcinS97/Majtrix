<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImport.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.cntImport" %>

<div class="cntImport">

    <script type="text/javascript">
        function checkFileABSENCJA() {
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
        <asp:FileUpload ID="FileUpload1" CssClass="fileupload" runat="server" Style="display: inline-block;" />
        <asp:Button ID="btImport" runat="server" CssClass="button200" Text="Import wartości" OnClick="btImport_Click" OnClientClick="javascript:if (checkFileABSENCJA()) {showAjaxProgress();return true;} else return false;" />
        <%--<span class="info">Miesiąc rozliczeniowy:</span>--%>
        <asp:Label ID="lblInfo" runat="server" CssClass="info" Text="Import za rok:" />
        <asp:DropDownList ID="ddlMiesiac" runat="server" DataSourceID="SqlDataSource1" DataTextField="Data" DataValueField="Value"></asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
            SelectCommand="
declare @today datetime = GETDATE()
select CONVERT(varchar(4), rok, 20) as Data, CONVERT(varchar(10), dbo.boy(rok), 20) + '|' + CONVERT(varchar(10), dbo.eoy(rok), 20) as Value
from
(
select DATEADD(YEAR, -Lp, dbo.boy(dbo.getdate(@today))) as rok from dbo.GetDates2('20000101',DATEADD(D, YEAR(@today) - 2015, '20000101'))
) D
        "></asp:SqlDataSource>
    </div>


    <asp:SqlDataSource ID="dsListaPlac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
declare @today datetime 
set @today = DATEADD(D,-15,GETDATE())
select M.Data, M.Data + '-01' as Value from dbo.GetDates2(@today, @today + 1) D
outer apply (select convert(varchar(7),DATEADD(M,-Lp,@today),20) as Data) M
    " />


</div>
