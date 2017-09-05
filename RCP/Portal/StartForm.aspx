<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="StartForm.aspx.cs" Inherits="HRRcp.Portal.StartForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/Zastepstwa.ascx" TagName="Zastepstwa" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Przypisania/cntPrzypisania.ascx" TagName="cntPrzypisania" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc1" %>
<%@ Register Src="~/Portal/Controls/cntWnioskiUrlopowe.ascx" TagName="cntWnioskiUrlopowe" TagPrefix="uc1" %>
<%@ Register Src="~/Portal/Controls/cntWniosekUrlopowy.ascx" TagName="cntWniosekUrlopowy" TagPrefix="uc2" %>
<%@ Register Src="~/SzkoleniaBHP/Controls/cntStartBHP.ascx" TagName="cntStartBHP" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page960 startform container wide">
        <div class="spacer16"></div>

        <div id="divPrzesuniecia" runat="server" class="border fiszka" visible="false">
            <h2>Przesunięcia pracowników do akceptacji</h2>
            <hr />

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <uc2:cntPrzypisania ID="cntPrzypisania" Status="0" Mode="0" Filter="0" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="divWnioskiAdm" runat="server" class="border fiszka" visible="false">
            <h2>Wnioski urlopowe do wprowadzenia</h2>
            <hr />

            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopoweAdm" Status="3" Mode="2" PageSize="5" Filter="0" OnShow="cntWnioskiUrlopoweAdm_Show" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="buttons" style="text-align: right;">
                <asp:Button ID="btEnter" CssClass="button250 btn btn-primary xpull-right" runat="server" Text="Wprowadź wnioski" OnClick="btEnter_Click" />
            </div>
        </div>

        <div id="divWnioskiKier" runat="server" class="border fiszka" visible="false">
            <h2>Wnioski urlopowe do akceptacji</h2>
            <hr />
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopoweKier" Status="1" Mode="1" PageSize="5" OnShow="cntWnioskiUrlopoweKier_Show" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="divWnioskiZdalna" runat="server" class="border fiszka" visible="false">
            <h2>Wnioski o pracę zdalną do akceptacji</h2>
            <hr />
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiPracaZdalna" Status="1" Mode="1337" PageSize="5" OnShow="cntWnioskiUrlopoweKier_Show" runat="server" Typy="12" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="divZastepstwa" runat="server" class="border fiszka" visible="false">
            <h2>Kierownicy, których aktualnie możesz zastąpić</h2>
            <hr />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc2:Zastepstwa ID="Zastepstwa" Mode="0" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="divSzkoleniaBHP" runat="server" class="border fiszka" visible="false">
            <h2>Wygasające i przeterminowane szkolenia BHP</h2>
            <hr />
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <uc2:cntStartBHP ID="cntStartBHP" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="buttons pull-right">
                <asp:Button ID="btStartBHP" CssClass="button250 btn btn-primary" runat="server" Text="Pokaż szkolenia" OnClick="btStartBHP_Click" />
            </div>
        </div>

        <div id="divNothingToDo" runat="server" class="border fiszka smile" visible="false">
            <h1><i class="fa fa-smile-o"></i>Brak czynności do wykonania</h1>
            <hr />
        </div>
    </div>
</asp:Content>

