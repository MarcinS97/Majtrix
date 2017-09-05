<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true"
    CodeBehind="Slowniki.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.Slowniki" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntTypes.ascx" TagPrefix="cc" TagName="cntTypes" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntTypesId.ascx" TagPrefix="cc" TagName="cntTypesId" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScParametry">
        <uc1:PageTitle ID="PageTitle1" runat="server" Ico="../images/captions/layout_edit.png"
            Title="Matryca Szkoleń - Słowniki" />
        <div class="pageContent inline">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc:cntTypes ID="Types" runat="server" TableName="msLinie" Title="Linie" Subtitle="Linie i ich nazwy" OnDeleting="Types_Deleting" />
        <%--            <cc:cntTypes ID="Uprawnienia" runat="server" TableName="msUprawnienia" Title="Uprawniennia"
                        Subtitle="Uprawnienia hehe" />
                    <cc:cntTypes ID="Stanowiska" runat="server" TableName="msStanowiska" Title="Stanowiska"
                        Subtitle="Stanowiska" />--%>
                    <cc:cntTypes ID="TypyBadan" runat="server" TableName="msTypyBadan" Title="Typy Badań" Subtitle="Typy Badań" />
                    <cc:cntTypesId ID="CertyfikatyStatusy" runat="server" TableName="msCertyfikatyStatus" Title="Certyfikaty - Statusy" Subtitle="Statusy certyfikatów" />
                    <cc:cntTypesId ID="AnkietyStatusy" runat="server" TableName="msAnkietyStatus" Title="Ankiety - Statusy" Subtitle="Statusy ankiet" />
                    <cc:cntTypesId ID="OcenyStatusy" runat="server" TableName="msOcenyStatus" Title="Oceny - Statusy" Subtitle="Statusy ocen" />
    <%--                <cc:cntTypes ID="LinieImport" runat="server" TableName="msLinieImport" Title="Linie Import"
                        Subtitle="Linie Import" />--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>



    <asp:SqlDataSource ID="dsLineExists" runat="server" SelectCommand="select count(*) from msLinieStanowiska where IdLinii = {0}" />



</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
