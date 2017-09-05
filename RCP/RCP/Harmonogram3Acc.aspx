<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="Harmonogram3Acc.aspx.cs" Inherits="HRRcp.RCP.Harmonogram3Acc" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntHarmonogramWrapper.ascx" TagPrefix="uc1" TagName="cntHarmonogramWrapper" %>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Plan pracy - Akceptacja" SubText1="Akceptacja planu pracy" />
    <div class="xform-page pgHarmonogram">
        <uc1:cntHarmonogramWrapper runat="server" ID="cntHarmonogramWrapper" Mode="Acc" />
    </div>
</asp:Content>

    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/Harmonogram.js" ></script>
    <style>
        body{
            overflow-y: hidden;
            overflow-x: auto;

        }
        div#content{
            background-color:#FAFCFE ;
        }

    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        $(document).on("ready", function () {

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler1);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);

            function BeginRequestHandler1(sender, args) {
            }

            function EndRequestHandler1(sender, args) {
                PrepareHarmonogram();
           } 
        });
    </script>
</asp:Content>
