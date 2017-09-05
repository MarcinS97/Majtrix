<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="InfoForm.aspx.cs" Inherits="HRRcp.InfoForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="info_content" class="pageContentWrapper InfoForm">       <!-- ie6 dokłada odstęp po caption a z tym div nie -->
        <div>                                                <!-- ie6 dokłada odstęp przed content a z tym div nie -->
            <div class="error_img">
                <img src="images/captions/info.png" alt=""/>
            </div>
            <div class="error_title">
                Informacja
            </div>

            <div class="error_code">
                <div style="min-height: 300px; display: block; "> 
                    <asp:Label ID="lbNoData" runat="server" Text="Brak danych" ></asp:Label>
                    <asp:Literal ID="ltInfo" runat="server" Visible="False"></asp:Literal>
                </div>
            </div>

            <div id="divButtons" class="bottom_buttons" style="padding-top: 20px;" visible="true" runat="server">
                <asp:Button ID="btBack" runat="server" CssClass="button100" Text="Powrót"/>
            </div>
        </div>
    </div>
</asp:Content>
