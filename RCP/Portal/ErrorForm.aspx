<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorForm.aspx.cs" Inherits="HRRcp.Portal.ErrorForm" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Wystąpił błąd</title>
    <base target="_self" />
    <link rel="stylesheet" type="text/css" href="../styles/Bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="Styles/Portal3.css" />
    <style>
        body { background-color: #F0F3F3; xfont-family: Lato; }
        .srodek { text-align: center; }
        .robot { padding-top: 100px; text-align: center; }
        .linia { border-top: solid 2px #EBEBEB; width: 500px; margin: auto; }
        .naglowek { color: #555555; font-size: 36px; text-align: center; margin-left: auto; margin-right: auto; margin-top: 16px; }
        .tresc { font-size: 18px; color: #555555; text-align: left; margin: auto; width: 700px; max-height: 400px; overflow: auto; margin-bottom: 15px; margin-top: 15px; padding: 32px; }
        .baton { margin: auto; text-align: center; margin-top: 10px; padding-left: 500px; }
        .baton .btn { min-width: 60px; }
        .wrapper { padding-bottom: 120px; }
    </style>
</head>
<body>
    <form id="form1" class="error_form" runat="server">

        <div class="wrapper">
        <div class="robot">
            <img runat="server" src="~/Portal/Styles/robot.png" />
        </div>
        <div class="naglowek">
            informacja
        </div>
        <div class="linia"></div>

        <div class="tresc">
            <asp:Label ID="lbInfo" runat="server"></asp:Label><br />
            <br />
            <asp:Label ID="lbInfoEx" runat="server"></asp:Label>
            <asp:Label ID="lbInfoOk" runat="server" Visible="false" Text="<br /><br />Po naciśnięciu klawisza Ok zostanie wyświetlona formatka startowa."></asp:Label>
        </div>
        <div class="linia"></div>
        <div class="baton">
            <asp:Button ID="btOk" runat="server" CssClass="btn btn-primary" Visible="false" Text="Ok" PostBackUrl="~/Default.aspx" />
            <asp:Button ID="btBack" runat="server" CssClass="btn btn-primary" Visible="false" Text="Ok" />
            <asp:Button ID="btClose" runat="server" CssClass="btn btn-default" Visible="false" Text="Zamknij" OnClientClick="window.close();" />
        </div>



        <%--      <div class="error_img">
            <img src="images/captions/error.png" alt="" />
        </div>
        <div class="error_title">
            WYSTĄPIŁ BŁĄD
        </div>
        <div class="error_code">
        </div>

        <div class="error_content">
            Informacje zostały zapisane do logu systemowego, a administrator został automatycznie powiadomiony o zaistniałej sytuacji.
                            <asp:Label ID="lbInfoOk" runat="server" Visible="false" Text="<br /><br />Po naciśnięciu klawisza Ok zostanie wyświetlona formatka startowa."></asp:Label>
        </div>--%>
        <%--<div class="bottom_buttons">
          
        </div>--%>



        <%--  <div id="mainformcontent">
        <uc3:HeaderControl ID="HeaderControl1" ShowInfo="false" runat="server" />
                
        <div id="content">
            <div class="center">
		        <div class="content2">
                    <div>
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
        
        <uc3:FooterControl ID="FooterControl1" runat="server" />
    </div>   
    <script type="text/javascript">
        hideFooter();
    </script>--%>
            </div>
    </form>
</body>
</html>


