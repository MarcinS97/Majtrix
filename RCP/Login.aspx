<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HRRcp.Login" %>
<%@ Register src="~/Controls/HeaderControl.ascx" tagname="HeaderControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/FooterControl.ascx" tagname="FooterControl" tagprefix="uc3" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc3" TagName="PageTitle" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Rejestracja Czasu Pracy</title>

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    
    <link rel="stylesheet" type="text/css" href="~/styles/FontAwesome/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="~/styles/Bootstrap/css/bootstrap.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/jQuery/css/jquery-ui.min.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/jQuery/css/jquery-ui.structure.min.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/jQuery/css/jquery-ui.theme.min.css" />
                           
    <link rel="stylesheet" type="text/css" href="~/styles/Controls.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/master4.css" />
    <link rel="stylesheet" type="text/css" href="~/styles/User/user.css" />
    
    <script type="text/javascript" src="<%# ResolveUrl("~/styles/jQuery/js/jquery-2.1.4.min.js") %>" ></script>
    <script type="text/javascript" src="<%# ResolveUrl("~/styles/jQuery/js/jquery-ui.min.js") %>" ></script>
    <script type="text/javascript" src="<%# ResolveUrl("~/styles/Bootstrap/js/bootstrap.js") %>" ></script>

    <script type="text/javascript" src="<%# ResolveUrl("~/scripts/common.js") %>" ></script>
    <script type="text/javascript" src="<%# ResolveUrl("~/MatrycaSzkolen/Scripts/MS.js") %>" ></script>
    <script type="text/javascript" src="<%# ResolveUrl("~/styles/User/user.js") %>" ></script>

    <base target="_self" />
    
    
    <script type="text/javascript">
        function resize() {
            var wh = getClientSize();
            var h = wh[1];
            if (h < 400) h = 400;

            var c = $('#content');
            c.height('auto');

            var ctop = c.offset().top;
            if (ctop + c.height() + 30 < h) {
                var h2 = h - ctop - 30;
                c.height(h2);

                var b = $('body');
                b.height(h2 + 30);

                var f = $('form');
                b.height(h2 + 30);
            }
            showFooter();
        }

        function initialize() {
            resize();
        }

        

        
    </script>
</head>
<body>
<form id="form1" class="mainform error_form" style="height:98%;" runat="server">
    <div id="content" >
    <div id="mainformcontent" style="display:block;" onload="initialize()">
        <%--        
        <uc3:HeaderControl ID="HeaderControl1" ShowInfo="false" runat="server" />
        --%>
        <nav class="navbar navbar-default navbar-fixed-top">
            <div class="container-fullwidth">
                <div class="navbar-header" >
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#">
                        <asp:Image ImageUrl="~/styles/User/logo.png" runat="server" />
                        <asp:Label ID="lbAppTitle" runat="server" >Rejestracja Czasu Pracy</asp:Label>
                    </a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav pull-right" id="topMenu" runat="server" >
                        <li><a href="#user" id="user-opener"><i class="glyphicon glyphicon-user"></i></a>
                            <div id="user" class="xhidden panel panel-default" style="display: none;">
                                <div class="panel-heading">
                                    <label>Witaj:</label>
                                    <asp:Label ID="lbUser" runat="server" Text="Niezalogowany" Visible="true"></asp:Label>
                                    <%--                                
                                    <asp:LinkButton ID="lbUser" runat="server" Text="Niezalogowany" OnClick="lbUser_Click"></asp:LinkButton>
                                    --%>
                                </div>
                                <div class="panel-body">
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

            <uc3:PageTitle runat="server" ID="PageTitle" Title="Informacja" Visible="true"/>
            <div class="content_center">
		        <div class="content_center2 center960">
                    <div class="pgErrorForm">
                        <div class="error_img">
                            <img src="images/captions/logowanie.png"  width="42" height="42" alt=""/>
                        </div>
                        <div class="error_title">
                            Logowanie
                        </div>
                        <div class="error_code" style="text-align:center">
                                                        <b>Witamy w programie analizy Rejestracji Czasu Pracy.</b><br />
                            <br />
                            <p id="ptext">Proszę zalogować się podając swój login, taki jak do systemu Windows oraz hasło.</p>

                            <asp:HiddenField ID="hidUniqueId" runat="server" />
                                
                             <br />
                            
                             <div class="login" style =" text-align:center;">
                                          <p> <label for="tbLogin">Login:&emsp;    </label>  <asp:TextBox ID="tbLogin" runat="server" CssClass="textbox" MaxLength="20"  AutoCompleteType="Disabled"></asp:TextBox></p> 
                             
                                          <p> <label for="tbLogin">Hasło:&emsp;    </label>  <asp:TextBox ID="tbPass" runat="server" CssClass="textbox" MaxLength="20" AutoCompleteType="Disabled" TextMode="Password"></asp:TextBox></p> 
                         
                             </div>
                                
                            <br />
                            <br />
                            <br />
                            <span id="loginInfo" runat="server" style="display: inline;">
                                Jeżeli jesteś na tym komputerze zalogowany(a) swoim loginem <b>(<asp:Label ID="lbLogin" runat="server"></asp:Label>)</b>, możesz przejść do aplikacji klikając ten 
                                <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">link</asp:LinkButton>.
                            </span>
                            <div class="bottom_buttons" >
                            <asp:Button ID="btOk" runat="server" CssClass="button100" Text="Zaloguj" onclick="btOk_Click"/>
                        </div>
                        </div>
                        
                       
                    </div>    
                </div>   
            </div>
        
        </div>

   
    <script type="text/javascript">
        hideFooter();
    </script>
</form>
    <div id="footer" class="footer printoff" style="display: block;">
	<div class="center">
		<span id="FooterControl1_lbAppName" class="appname">Rejestracja Czasu Pracy</span> v. <span id="FooterControl1_lbVersion">3.0.0.0</span>
        <span id="FooterControl1_lbCopyright" class="copyright">© KDR Solutions Sp. z o.o. '2017</span>
        
        
	</div>
</div>
</body>
</html>


