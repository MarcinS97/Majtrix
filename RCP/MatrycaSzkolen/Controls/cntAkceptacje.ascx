<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAkceptacje.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntAkceptacje" %>

<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="cc" TagName="cntSqlTabs" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/cntAkceptacjeSzk.ascx" TagPrefix="cc" TagName="cntAkceptacjeSzk" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/cntAkceptacjeEw.ascx" TagPrefix="cc" TagName="cntAkceptacjeEw" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/cntAkceptacjeOceny.ascx" TagPrefix="cc" TagName="cntAkceptacjeOceny" %>

<asp:HiddenField ID="hidSelectedType" runat="server" Visible="false" />

<div class="cntAkceptacje">
    <div id="divTypes" runat="server">
        <cc:cntSqlTabs ID="Types" runat="server"
            AddCssClass="tabKwal"
            DataTextField="Name"
            DataValueField="Id"
            SQL="
select 1 as Id, 'Szkolenia' as Name, 0 as Sort
union all
select 2 as Id, 'Ankiety' as Name, 1 as Sort
union all
select 3 as Id, 'Oceny' as Name, 2 as Sort
        "
            OnSelectTab="Types_SelectTab"
            OnDataBound="Types_DataBound" />
    </div>

    <asp:MultiView ID="mvContent" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <cc:cntAkceptacjeSzk ID="cAkceptacjeSzk" runat="server" />
        </asp:View>
        <asp:View ID="View2" runat="server">

            <cc:cntAkceptacjeEw ID="cAkceptacjeEw" runat="server" />

        </asp:View>
        <asp:View ID="View3" runat="server">

            <cc:cntAkceptacjeOceny ID="cAkceptacjeOceny" runat="server" />

        </asp:View>


    </asp:MultiView>

</div>

<%--select 0 as Id, 'W przygotowaniu' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 0 as Sort from (select 1 x) x
outer apply (select nullif(count(*), 0) c from Certyfikaty c where c.Status = 0) oa
union all
select 1 as Id, 'Do przeszkolenia' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 1 as Sort from (select 1 x) x
outer apply (select nullif(count(*), 0) c from Certyfikaty c where c.Status = 1) oa
union all--%>