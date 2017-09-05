<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.master" AutoEventWireup="true" CodeBehind="Formularz.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Formularz" %>

<%@ Register Src="~/Portal/Controls/cntArticles.ascx" TagPrefix="uc1" TagName="cntArticles" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntLista.ascx" TagPrefix="cc" TagName="cntWnioskiMajatkoweLista" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntWniosekModal.ascx" TagPrefix="cc" TagName="cntWnioskiMajatkoweModal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).on("ready", function () {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler1);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);

            //cntWniosekModal();
            function BeginRequestHandler1(sender, args) {

            }

            function EndRequestHandler1(sender, args) {
                //cntWniosekModal();
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgArticles border">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="page-title">
                    Moje wnioski
                    <asp:LinkButton ID="btFormularz" runat="server" CssClass="btn btn-primary btn-lg pull-right" OnClick="btFormularz_Click">
                    <i class="fa fa-pencil"></i>
                    Wypełnij wniosek
                    </asp:LinkButton>
                </div>
                <div class="container wide">
                    <cc:cntWnioskiMajatkoweLista runat="server" ID="cntWnioskiMajatkoweLista" Mode="0" OnShow="cntWnioskiMajatkoweLista_Show1" OnListDataBound="cntLista_ListDataBound" />
                </div>
                <%--<uc1:cntArticles ID="cntArticles" runat="server" Mode="0" PageSize="0" Grupa="UB_FORMULARZ" Title="_" />--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <cc:cntWnioskiMajatkoweModal runat="server" ID="cntWnioskiMajatkoweModal" OnSaved="cntWnioskiMajatkoweModal_Saved" />
    </div>
</asp:Content>
