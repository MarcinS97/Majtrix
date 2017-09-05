<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntUbezpieczeniaParametry.ascx.cs" Inherits="HRRcp.Portal.Controls.cntUbezpieczeniaParametry" %>
<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Portal" TagPrefix="cc1" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Reports/cntReportSchedulerEdit.ascx" TagPrefix="uc1" TagName="cntReportSchedulerEdit" %>


<div id="paOgloszeniaParametry" runat="server" class="cntOgloszeniaParametry">
    <div class="fields">
        <div id="paParams" runat="server">
<%--            <uc1:dbField runat="server" ID="Email" Type="tb" MaxLength="500" Rq="true" StVisible="3" 
                Label="Adres e-mail wysyłki eksportu listy ubezpieczeń majątkowych:"/>
            <uc1:dbField runat="server" ID="StartEksportu" Type="date" Rq="true" StVisible="3" 
                Label="Dzień eksportu listy:"/>--%>
            <uc1:dbField runat="server" ID="Skladnik" ValueField="SkladnikOpis" Type="ddl" Rq="true" StVisible="3" DataSourceID="dsSkladniki" Wybierz="true" 
                Label="Nazwa składnika płacowego:"/>
        </div>
        <uc1:cntReportSchedulerEdit runat="server" id="cntReportSchedulerEdit" Connection="PORTAL" Grupa="UBEZPIECZENIA" Mode="1"/>
    </div>
    <div id="paButtons" runat="server" class="buttons">
        <cc1:WnButton ID="wbtSave"   CssClass="btn btn-default" runat="server" Text="Zapisz" onclick="wbtSave_Click" StVisible="3" />
        <cc1:WnButton ID="wbtCancel" CssClass="btn btn-default" runat="server" Text="Anuluj" onclick="wbtCancel_Click" StVisible="3" />
        <cc1:WnButton ID="wbtEdit"   CssClass="btn btn-default" runat="server" Text="Edycja" onclick="wbtEdit_Click" StVisible="2" />        
        <%--    
        <asp:Button ID="btEdit" runat="server" CssClass="btn btn-default" Text="Edycja" OnClick="btEdit_Click" />
        <asp:Button ID="btSave" runat="server" Text="Zapisz" CssClass="btn btn-default" Visible="false" OnClick="btSave_Click" />
        <asp:Button ID="btCancel" runat="server" Text="Anuluj" CssClass="btn btn-default" Visible="false" OnClick="btCancel_Click" />
        --%>
    </div>
</div>

<asp:SqlDataSource ID="dsParametry" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
    SelectCommand="select top 1 *, Skladnik SkladnikOpis from poUbezpieczeniaParametry"
    >
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsSkladniki" runat="server" ConnectionString="<%$ ConnectionStrings:ASSECO %>" 
    SelectCommand="
select distinct ISNULL(Kod, SkladnikRodzaj) + ' - ' + Opis [SkladnikOpis], ISNULL(Kod, SkladnikRodzaj) [Skladnik]
from lp_vv_SkladnikiPracownikaRodz
where Aktywny = 1
order by 1
    ">
</asp:SqlDataSource>
