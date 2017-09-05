<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntLogin.ascx.cs" Inherits="HRRcp.Portal.Controls.cntLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

<uc1:cntModal runat="server" ID="cntModal" ShowCloseButton="false" ShowFooter="false" Title="Logowanie" >
    <ContentTemplate>

<div class="login-box">
    <div class="login-header">
        <%--Logowanie--%>
        <i class="fa fa-user"></i>
        <asp:HiddenField ID="hidUniqueId" runat="server" Visible="false" />
    </div>
    <div class="login-body">
        <%--<h3>Zaloguj się do systemu</h3>
            <hr />
            <br />--%>
        <div class="hidden">
            <h3>
                <asp:Label ID="lbInfo1" runat="server" Text="Witamy w Panelu Pracownika."></asp:Label></h3>
            <br />
            <asp:Label ID="lbInfo2" runat="server" Text="W panelu dostępne są informacje dotyczące wypłat oraz wymiaru i wykorzystania urlopu."></asp:Label><br />
            <br />

            <asp:Label ID="lbInfo3" runat="server" Text="Proszę zaloguj się podając swój numer PESEL oraz hasło. Jeżeli nie posiadasz hasła lub masz problem z zalogowaniem, skontaktuj się z działem HR."></asp:Label><br />
            <br />
        </div>

        <div class="form-group ">
            <%--<label>PESEL:</label>--%>
            <asp:TextBox ID="tbLogin" runat="server" CssClass="form-control xinline" MaxLength="11" AutoCompleteType="Disabled" placeholder="Login..."></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="ftbLogin"
                runat="server" Enabled="True" TargetControlID="tbLogin"
                FilterType="Numbers">
            </asp:FilteredTextBoxExtender>
            <asp:RequiredFieldValidator ID="rfvLogin" runat="server"
                ErrorMessage="Pole wymagane" ControlToValidate="tbLogin" CssClass="error"
                Display="Dynamic" SetFocusOnError="True" ValidationGroup="vgLogin"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group ">
            <%--<label>Hasło:</label>--%>
            <asp:TextBox ID="tbPass" TextMode="Password" runat="server" CssClass="form-control xinline" MaxLength="20" AutoCompleteType="Disabled" placeholder="Hasło..."></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPass" runat="server"
                ErrorMessage="Pole wymagane" ControlToValidate="tbPass" CssClass="error"
                Display="Dynamic" SetFocusOnError="True" ValidationGroup="vgLogin"></asp:RequiredFieldValidator>
        </div>

        <asp:Button ID="btOk" runat="server" CssClass="btn btn-primary btn-sm pull-right" Text="Zaloguj" OnClick="btOk_Click" ValidationGroup="vgLogin" />
    </div>
    <div class="login-footer hidden">

        <asp:Button ID="btBack" runat="server" CssClass="btn btn-default" Text="Wróć" OnClientClick="javascript:history.back();return false;" Visible="false" />
    </div>
</div>
        

    </ContentTemplate>
</uc1:cntModal>