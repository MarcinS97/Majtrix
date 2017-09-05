<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="PracownicyHarm.aspx.cs" Inherits="HRRcp.RCP.Adm.PracownicyHarm" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register src="~/RCP/Controls/Harmonogram/cntPracownicyHarm.ascx" tagname="cntPracownicyHarm" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Aktywni Pracownicy" SubText1="Ustawianie widoczności pracowników" />
    <div class="form-page pgPracownicyHarm">
        <uc1:cntPracownicyHarm ID="cntPracownicyHarm" runat="server" />
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        $(document).on("ready", function () {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler1);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);

            function BeginRequestHandler1(sender, args) {
            }

            function EndRequestHandler1(sender, args) {
            }
        });
    </script>
</asp:Content>
