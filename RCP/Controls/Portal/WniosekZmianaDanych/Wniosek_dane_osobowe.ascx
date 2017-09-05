<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Wniosek_dane_osobowe.ascx.cs" Inherits="HRRcp.Controls.WnioseZmianaDanych.Wniosek" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/GetName.ascx" TagName="GetName" TagPrefix="muc1" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/cntzoom.ascx" TagName="Zoom" TagPrefix="muc1" %>

<div runat="server" id="cntWniosek_dane_osobowe">

        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
<muc1:GetName ID="GetName1" runat="server" />
<b>Moje Wnioski :</b>





            <muc1:Zoom Id="Zoom1" Lvl="1"  runat="server" />
    </ContentTemplate>
        </asp:UpdatePanel>


        


        

        






</div>