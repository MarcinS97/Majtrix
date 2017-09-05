<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Pracownicy.aspx.cs" Inherits="HRRcp.Portal.PortalPracownicy" %>
<%@ Register Src="~/Portal/Controls/cntSqlContent2.ascx" TagName="cntSqlContent" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWide" runat="server">
    <div id="pgPortalDane" class="pgPortalPracownicy">
        <div class="page-title"><i class="fa fa-user"></i>Pracownicy</div>
        <div class="container wide" style="position: relative;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div runat="server" id="paPracownicy" class="paSelectPrac">
                        <div class="paSelectPrac1">
                            <span class="t1">Pracownik:</span>
                            <asp:DropDownList ID="ddlPracownicy" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlPracownicy_SelectedIndexChanged" />
                            <div class="divider_ppacc"></div>
                        </div>
                    </div>
                    <uc1:cntSqlContent ID="cntSqlContent1" runat="server" Grupa="DANEK" OnSelectTab="cntSqlContent1_SelectTab" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>



<%--
<script runat="server">
    protected void xxxxPage_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //hidKierId.Value = "38";//HRRcp.App_Code.App.User.Id;
            cntSqlContent1.cntLine.NoDataInfo = "wybierz pracownika...";
            cntSqlContent1.DataBind();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Prepare();
            cntSqlContent1.cntLine.NoDataInfo = "wybierz pracownika...";
        }
    }

    private void Prepare()
    {
        System.Data.DataSet ds = HRRcp.App_Code.db.getDataSet(String.Format(@"
declare @rootId int
set @rootId = {0}
select 
--'-1|-1|-1' as Id, 
'0|0|null' as Id, 
'wybierz ...' as Pracownik, null as SortPath
union
select KadryId + '|' + convert(varchar, IdPracownika) as Id,
--select KadryId + '|' + convert(varchar, IdPracownika) + '|' + ISNULL(KadryId2, 'null') as Id,
replicate('&nbsp;', (Hlevel - 1) * 4) +
Nazwisko + ' ' + Imie + ' (' + ISNULL(KadryId, '???') + ')' as Pracownik, SortPath
from dbo.fn_GetTree(@rootId, GETDATE())    
order by SortPath", HRRcp.App_Code.App.User.Id));

        foreach (System.Data.DataRow dr in HRRcp.App_Code.db.getRows(ds))
            dr[1] = HttpUtility.HtmlDecode(dr[1].ToString());

        HRRcp.App_Code.Tools.BindData(ddlPracownicy, ds, "Pracownik", "Id", false, null);
    }
    
</script>
--%>
