<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="ErrorForm3.aspx.cs" Inherits="HRRcp.ErrorForm3" %>
<%@ Register src="~/Controls/HeaderControl.ascx" tagname="HeaderControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/cntFooter.ascx" tagname="FooterControl" tagprefix="uc3" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc3" TagName="PageTitle" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title><%# GetAppName %></title>

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    
    <link rel="stylesheet" type="text/css" href="~/styles/FontAwesome/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="~/styles/Bootstrap/css/bootstrap.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/jQuery/css/jquery-ui.min.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/jQuery/css/jquery-ui.structure.min.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/jQuery/css/jquery-ui.theme.min.css" />
                           
    <link rel="stylesheet" type="text/css" href="~/styles/Controls.css" />
    <link rel="Stylesheet" type="text/css" href="~/styles/master3.css" />
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

        $(window).resize(function () {
            resize();
        });

        $(function () {
            initialize();
        });
    </script>
</head>
<body>
<form id="form1" class="mainform error_form" runat="server">
    <div id="mainformcontent">
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
                        <asp:Label ID="lbAppTitle" runat="server" ></asp:Label>
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

        <div id="content">
            <uc3:PageTitle runat="server" ID="PageTitle" Title="Informacja" Visible="true"/>
            <div class="content_center">
		        <div class="content_center2 center960">
                    <div class="pgErrorForm">
                        <div class="error_img">
                            <img src="images/captions/error.png" alt=""/>
                        </div>
                        <div class="error_title">
                            WYSTĄPIŁ BŁĄD
                        </div>
                        <div class="error_code">
                            <asp:Label ID="lbInfo" runat="server" ></asp:Label><br /><br />
                            <asp:Label ID="lbInfoEx" runat="server" ></asp:Label>
                        </div>
                        
                        <div class="error_content">
                            Informacje zostały zapisane do logu systemowego, a administrator został automatycznie powiadomiony o zaistniałej sytuacji.
                            <asp:Label ID="lbInfoOk" runat="server" Visible="false" Text="<br /><br />Po naciśnięciu klawisza Ok zostanie wyświetlona formatka startowa."></asp:Label>
                        </div>
                        <div class="bottom_buttons" >
                            <asp:Button ID="btOk" runat="server" CssClass="button100" Visible="false" Text="Ok" PostBackUrl="~/Default.aspx"/>
                            <asp:Button ID="btBack" runat="server" CssClass="button100" Visible="false" Text="Ok" />
                            <asp:Button ID="btClose" runat="server" CssClass="button100" Visible="false" Text="Zamknij" OnClientClick="window.close();" />  <!-- wymagane dodanie <base target="_self" /> do <head> -->
                        </div>
                    </div>    
                </div>   
            </div>
        </div>
        
        <uc3:FooterControl ID="FooterControl1" runat="server" CopyrightAsLink="false" />
    </div>   
    <script type="text/javascript">
        hideFooter();
    </script>
</form>
</body>
</html>


