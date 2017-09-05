<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzypiszRCP.ascx.cs" Inherits="HRRcp.Controls.Kiosk.cntPrzypiszRCP" %>

<%--
<%@ Register src="~/Controls/Adm/cntPracownicy3.ascx" tagname="Pracownicy" tagprefix="uc1" %>
--%>
<%@ Register src="~/Controls/Kiosk/Pracownicy2.ascx" tagname="Pracownicy" tagprefix="uc1" %>




<script type="text/javascript">
    function cardPush() {
        $('#source').val('z ręki');
        document.forms[0].method = 'post';
        document.forms[0].action = 'Login.aspx';
        document.forms[0].submit();
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table class="table0 points">
            <tr>
                <td class="col1">
                    <asp:Label ID="lbPoint1" runat="server" CssClass="point">
                        1. Wyszukaj i wybierz pracownika
                        <i class="fa fa-user not-good"></i>
                              <i class="fa fa-check good"></i>

                    </asp:Label>
                  
                    <div class="img">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/captions/tick.png" Visible="false" />
                    </div>
                </td>
                <td class="col2">
                    <div class="label">
                        <asp:Label ID="lbPoint2" runat="server" CssClass="point point_disabled">
                            2. Przyłóż kartę RCP do czytnika - przypisanie zostanie dokonane automatycznie
                            <i class="fa fa-id-card-o"></i>
                        </asp:Label>
                        <asp:Label ID="lbError" runat="server" CssClass="point point_error" Text="Wystąpił błąd podczas przypisywania. Szczegóły znajdują się w logu systemowym."></asp:Label>
                        <asp:Label ID="lbInfo" runat="server" CssClass="point point_info" Text="OK! Przypisanie zostało wykonane poprawnie"></asp:Label>
                    </div>
                    <div class="img">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/captions/tick.png" Visible="false" />
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<div id="paTest" runat="server" class="paEnterRCP">
    <div class="input-group">
        <span class="input-group-addon addon-default">Alternatywnie podaj numer karty RCP:</span>
        <input type="text" id="kartaRCPprz" name="kartaRCPprz" class="textbox form-control" />
        <input type="hidden" id="source" name="zrodlo" value="czytnik" />
        <div class="input-group-btn">
            <asp:Button ID="btPrzypisz" runat="server" CssClass="button100 btn btn-default" Text="Przypisz" OnClientClick="javascript:cardPush();return false;" />
            <asp:Button ID="btLogin" runat="server" CssClass="button100 btn btn-default" Text="Połóż kartę" OnClientClick="javascript:cardPush();return false;" Visible="false" />
            <asp:Button ID="btLogout" runat="server" CssClass="button100 btn btn-default" Text="Usuń kartę" OnClientClick="javascript:window.location='Logout.aspx';return false;" Visible="false" />

        </div>
    </div>
</div>

<uc1:Pracownicy ID="cntPracownicy" runat="server" Mode="6" OnSelectedChanged="cntPracownicy_SelectedChanged" />


<%--
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cntPracownicy" />
    </Triggers>
--%>

