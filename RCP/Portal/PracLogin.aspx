<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="PracLogin.aspx.cs" Inherits="HRRcp.Portal.PracLogin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<style>
        .sidebar {
            display: none;
        }

        #main, #footer {
            padding-left: 0 !important;
        }
    </style>--%>
    <style>
        .sidebar { display: none; }
        .nav .sidebar-hide { display: none !important; }
        #main, #footer { padding-left: 0 !important; }   
        /*#footer { display: none !important; }
        #appBar { display: none !important; }
        .navbar-right, .navbar-nav { display: none !important; }*/
    </style>

    <script type="text/javascript">
        $('.btnLogin').focus();
        $(document).keydown(function (e) {
            if (e.keyCode === 13) { // enter
                //alert($('.btnLogin').length);
                $('.btnLogin').click();
                e.defaultPrevented();
                e.stopPropagation();
            }
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-box">
        <div class="login-header">
            <%--Logowanie--%>
            <i class="fa fa-user"></i>
            <asp:HiddenField ID="hidUniqueId" runat="server" Visible="false" />
        </div>
        <div class="login-body">
            <h3>
                <%--Zaloguj się do systemu--%>
                <center class="text-primary" style="font-size: 32px !important;">
                <i class="fa fa-user text-primary"></i>Portal</center>

            </h3>
            <hr />
            <br />
            <div class="hidden">
                <h3>
                    <asp:Label ID="lbInfo1" runat="server" Text="Witamy w Panelu Pracownika."></asp:Label></h3>
                <br />
                <asp:Label ID="lbInfo2" runat="server" Text="W panelu dostępne są informacje dotyczące wypłat oraz wymiaru i wykorzystania urlopu."></asp:Label><br />
                <br />

                <asp:Label ID="lbInfo3" runat="server" Text="Proszę zaloguj się podając swój numer PESEL oraz hasło. Jeżeli nie posiadasz hasła lub masz problem z zalogowaniem, skontaktuj się z działem HR."></asp:Label><br />
                <asp:Label ID="lbInfo3b" runat="server" Visible="false" Text="Proszę zaloguj się podając swój login oraz hasło. Jeżeli nie posiadasz hasła lub masz problem z zalogowaniem, skontaktuj się z działem HR."></asp:Label><br />
                <br />
            </div>

            <div class="form-group" style="position: relative;">
                <%--<label>PESEL:</label>--%>
                <%--<div class="input-group">--%>


                    <%--<span class="input-group-addon">PESEL</span>--%>
                    <asp:Label ID="lbLogin" runat="server" CssClass="form-control icon-text login" Visible="false" />
                    <asp:TextBox ID="tbLogin" runat="server" CssClass="form-control icon-text" MaxLength="11" AutoCompleteType="Disabled" Placeholder="PESEL" />               
                    <span class="fa fa-sign-in"></span>
                    
                    <asp:FilteredTextBoxExtender ID="ftbLogin"
                        runat="server" Enabled="True" TargetControlID="tbLogin"
                        FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>

                    <asp:RequiredFieldValidator ID="rfvLogin" runat="server"
                        ErrorMessage="Pole wymagane" ControlToValidate="tbLogin" CssClass="error"
                        Display="Dynamic" SetFocusOnError="True" ValidationGroup="vgLogin"></asp:RequiredFieldValidator>
                <%--</div>--%>

            </div>
            <div class="form-group" style="position: relative;">
                <%--<label>Hasło:</label>--%>
             <%--   <div class="input-group">--%>
                    <%--<span class="input-group-addon">Hasło</span>--%>
                    <asp:TextBox ID="tbPass" TextMode="Password" runat="server" CssClass="form-control icon-text" MaxLength="20" 
                        AutoCompleteType="Disabled" placeholder="Hasło"></asp:TextBox>
                    <span class="fa fa-key"></span>
                    <asp:RequiredFieldValidator ID="rfvPass" runat="server"
                        ErrorMessage="Pole wymagane" ControlToValidate="tbPass" CssClass="error"
                        Display="Dynamic" SetFocusOnError="True" ValidationGroup="vgLogin"></asp:RequiredFieldValidator>
                    </div>
                <%--</div>--%>

            </div>
            <div class="login-footer">
                <asp:Button ID="btOk1337" runat="server" CssClass="btn btn-primary btnLogin" Text="Zaloguj" OnClick="btOk_Click" ValidationGroup="vgLogin" />
                <asp:Button ID="btBack" runat="server" CssClass="btn btn-default" Text="Wróć" OnClientClick="javascript:history.back();return false;" Visible="true" />
            </div>
        </div>







        <%--  <div class="login_form" style="display: none;">
        <div class="content2">
            <div class="info_img">
                <img src="images/captions/PracNotes.png" alt=""/>
            </div>
            <div class="info_title">
                LOGOWANIE
            </div>
            <div class="info_code">                
                <b><asp:Label ID="lbInfo1" runat="server" Text="Witamy w Panelu Pracownika."></asp:Label></b><br />
                <br />
                <asp:Label ID="lbInfo2" runat="server" Text="W panelu dostępne są informacje dotyczące wypłat oraz wymiaru i wykorzystania urlopu."></asp:Label><br />
                <br />
                <br />
                <asp:Label ID="lbInfo3" runat="server" Text="Proszę zaloguj się podając swój numer PESEL oraz hasło. Jeżeli nie posiadasz hasła lub masz problem z zalogowaniem, skontaktuj się z działem HR."></asp:Label><br />
                <br />
                <br />
                <!-- group box -->
                <asp:HiddenField ID="hidUniqueId" runat="server" Visible="false"/>
                <table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">
                    Logowanie
                    </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
                    <!-- group box content -->
                    <table class="login_data">
                        <tr class="row1">
                            <td class="col1">
                                <asp:Label ID="lbLogin" runat="server" Text="Pesel:"></asp:Label>
                            </td>
                            <td class="col2">
                                <asp:TextBox ID="tbLogin" runat="server" CssClass="textbox" MaxLength="11" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftbLogin" 
                                    runat="server" Enabled="True" TargetControlID="tbLogin"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvLogin" runat="server" 
                                ErrorMessage="Pole wymagane" ControlToValidate="tbLogin" CssClass="error" 
                                Display="Dynamic" SetFocusOnError="True" ValidationGroup="vgLogin"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr class="row2">
                            <td class="col1">
                                Hasło:
                            </td>
                            <td rowspan="2" class="col2">
                                <asp:TextBox ID="tbPass" TextMode="Password" runat="server" CssClass="textbox" MaxLength="20" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPass" runat="server" 
                                ErrorMessage="Pole wymagane" ControlToValidate="tbPass" CssClass="error" 
                                Display="Dynamic" SetFocusOnError="True" ValidationGroup="vgLogin"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>                            
                    <!-- group box end content -->
                    </td><td class="vr" >&nbsp;</td></tr><tr><td class="bl"></td><td class="bh"></td><td class="br"></td></tr>
                </table>
                <!-- group box end -->

                <br />
                <br />
                <br />
            </div>
            <div class="info_content">
            </div>
            <div class="bottom_buttons" >
                <asp:Button ID="btOk" runat="server" CssClass="btn btn-default" Text="Zaloguj" onclick="btOk_Click" ValidationGroup="vgLogin"/>
                <asp:Button ID="btBack" runat="server" CssClass="btn btn-default" Text="Wróć" OnClientClick="javascript:history.back();return false;" Visible="true"/>
            </div>
        </div>
    </div>   --%>
</asp:Content>
