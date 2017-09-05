<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="PracPassChange.aspx.cs" Inherits="HRRcp.Portal.PracPassChangeForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .sidebar { display: none; }
        .nav .sidebar-hide { display: none !important; }
        .nav .pass-change { display: none;}
        #main, #footer { padding-left: 0 !important; }
        #footer { display: none !important; }
        #appBar { display: none !important; }
        .navbar-right, .navbar-nav { display: none !important; }
        
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-box">
        <div class="login-header">
            <i class="fa fa-exchange"></i>
        </div>
        <div class="login-body">
            <div class="info_code">
                <h3>
                    <i class="fa fa-exchange"></i>
                    Zmiana hasła
                </h3>

                <hr />

                <asp:literal id="ltForce" runat="server" text="Ze względów bezpieczeństwa wymagana jest zmiana dotychczasowego hasła." visible="false"></asp:literal>
                Proszę wprowadź nowe hasło i powtórz je w celu weryfikacji. Po akceptacji hasło zostanie ustawione i będzie aktywne od następnego logowania.                 
                <asp:literal id="ltComplexity" runat="server"></asp:literal>
                <br />
                <br />
                <br />
                <!-- group box -->
                <asp:hiddenfield id="hidUniqueId" runat="server" />
                <div class="form-group">
                    <div class="input-group">
                        <!-- group box content -->

                        <span class="input-group-addon">Obecne hasło</span>
                        <asp:textbox id="tbOldPassword" textmode="Password" runat="server" cssclass="textbox form-control" maxlength="20" autocompletetype="Disabled"></asp:textbox>

                    </div>
                    <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server"
                        errormessage="Pole wymagane" controltovalidate="tbOldPassword" cssclass="error"
                        display="Dynamic" setfocusonerror="True" validationgroup="vgLogin"></asp:requiredfieldvalidator>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <!-- group box content -->

                        <span class="input-group-addon">Nowe hasło</span>
                        <asp:textbox id="tbPass0" textmode="Password" runat="server" cssclass="textbox form-control" maxlength="20" autocompletetype="Disabled"></asp:textbox>

                    </div>
                    <asp:requiredfieldvalidator id="rfvLogin" runat="server"
                        errormessage="Pole wymagane" controltovalidate="tbPass0" cssclass="error"
                        display="Dynamic" setfocusonerror="True" validationgroup="vgLogin"></asp:requiredfieldvalidator>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">Powtórz hasło</span>
                        <asp:textbox id="tbPass" textmode="Password" runat="server" cssclass="textbox form-control" maxlength="20" autocompletetype="Disabled"></asp:textbox>

                    </div>
                    <asp:requiredfieldvalidator id="rfvPass" runat="server"
                        errormessage="Pole wymagane" controltovalidate="tbPass" cssclass="error"
                        display="Dynamic" setfocusonerror="True" validationgroup="vgLogin"></asp:requiredfieldvalidator>
                </div>
            </div>
            <div class="login-footer">
                <asp:button id="btOk" runat="server" cssclass="btn btn-success" text="Zmień hasło" onclick="btOk_Click" validationgroup="vgLogin" />
                <asp:button id="btGo" runat="server" cssclass="button_postback" onclick="btGo_Click" />
                <asp:button id="btBack" runat="server" cssclass="button100 btn btn-default" text="Wróć" onclientclick="javascript:history.back();return false;" visible="true" />
            </div>
        </div>

    </div>

    <asp:sqldatasource id="dsGetOldPassword" runat="server" selectcommand="select Pass from Pracownicy where Id = {0}" />
</asp:Content>
