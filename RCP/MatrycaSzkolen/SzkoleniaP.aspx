<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="SzkoleniaP.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.SzkoleniaP" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/MatrycaSzkolen/Controls/Uprawnienia/cntUprawnieniaScroll.ascx" tagname="cntUprawnienia" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <div class="pgSzkoleniaBHP">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:cntUprawnienia ID="cntUprawnienia1" runat="server" PracownicyTyp="MSPRODUKCJA" Typ="2048" Title="Szkolenia produkcyjne" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
    <script type="text/javascript">
        $(document).on("ready", function () {

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler1);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);

            function BeginRequestHandler1(sender, args) {
            }

            function EndRequestHandler1(sender, args) {
                $(function () {
                    $(".data-scroller-main").scroll(function () {
                        $('.data-scroller').scrollLeft($(this).scrollLeft());
                    });
                });
            }
        });
    </script>
</asp:Content>
