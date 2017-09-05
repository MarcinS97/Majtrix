<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Parametry.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Majatkowe.Parametry" %>

<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntWariantyAdm.ascx" TagPrefix="uc1" TagName="cntWariantyAdm" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgUbezpAdministracja">
        <div class="page-title">Ubezpieczenia majątkowe - Parametry</div>
        
        <div class="xpage-box container wide">
            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <uc1:cntWariantyAdm runat="server" ID="cntWariantyAdm" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
