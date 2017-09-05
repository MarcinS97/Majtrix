<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="PlanUrlopowKier.aspx.cs" Inherits="HRRcp.Portal.PlanUrlopowKier" %>
<%@ Register src="~/Controls/Kwitek/PracUrlop2.ascx" tagname="PracUrlop" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanUrlopow/cntPlanUrlopow.ascx" tagname="cntPlanUrlopow" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page1200" >
        <div class="spacer16"></div>
        <div class="border">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                <ContentTemplate>
                    <div class="padding">
                        <div runat="server" id="div2" visible="false">  
                            <span class="t1">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownicy4" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy4_SelectedIndexChanged" />
                            <div class="divider_ppacc"></div>
                        </div>
                        <uc1:cntPlanUrlopow ID="cntPlanUrlopow" Mode="0" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
