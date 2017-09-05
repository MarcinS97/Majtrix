<%@ Page Title="" Language="C#" MasterPageFile="~/Kwitek.Master" AutoEventWireup="true" CodeBehind="PracPassChange.aspx.cs" Inherits="HRRcp.PracPassChangeForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login_form">
        <div class="content2">
            <div class="info_img">
                <img src="images/captions/PracNotes.png" alt=""/>
            </div>
            <div class="info_title">
                ZMIANA HASŁA
            </div>
            <div class="info_code">
                <b>Witamy w Panelu Pracownika.</b><br />
                <br />
                <asp:Literal ID="ltForce" runat="server" Text="Ze względów bezpieczeństwa wymagana jest zmiana dotychczasowego hasła." Visible="false"></asp:Literal><br />
                Proszę wprowadź nowe hasło i powtórz je w celu weryfikacji. Po akceptacji hasło zostanie ustawione i będzie aktywne od następnego logowania.                 
                <asp:Literal ID="ltComplexity" runat="server" ></asp:Literal><br />
                <br />
                <br />
                <!-- group box -->
                <asp:HiddenField ID="hidUniqueId" runat="server" />
                <table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">
                    Logowanie
                    </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
                    <!-- group box content -->
                    <table class="login_data">
                        <tr class="row1">
                            <td class="col1">
                                Nowe hasło:
                            </td>
                            <td class="col2">
                                <asp:TextBox ID="tbPass0" TextMode="Password" runat="server" CssClass="textbox" MaxLength="20" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLogin" runat="server" 
                                ErrorMessage="Pole wymagane" ControlToValidate="tbPass0" CssClass="error" 
                                Display="Dynamic" SetFocusOnError="True" ValidationGroup="vgLogin"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr class="row2">
                            <td class="col1">
                                Powtórz:
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
                <asp:Button ID="btOk" runat="server" CssClass="button100" Text="Zmień hasło" onclick="btOk_Click" ValidationGroup="vgLogin"/>
                <asp:Button ID="btGo" runat="server" CssClass="button_postback" onclick="btGo_Click" />
                <asp:Button ID="btBack" runat="server" CssClass="button100" Text="Wróć" OnClientClick="javascript:history.back();return false;" Visible="true"/>
            </div>
        </div>
    </div>   
</asp:Content>
