<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="ErrorForm.aspx.cs" Inherits="HRRcp.ErrorForm" %>
<%@ Register src="~/Controls/HeaderControl.ascx" tagname="HeaderControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/FooterControl.ascx" tagname="FooterControl" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Wystąpił błąd</title>
    
    <link href="~/styles/master.css" rel="stylesheet" type="text/css" />
    <link href="~/styles/mainmenu_SIE.css" rel="stylesheet" type="text/css" runat="server" id="css1" visible="false"/>
    <link href="~/styles/Controls.css" rel="stylesheet" type="text/css" />

    <link href="~/styles/Portal_iQor.css" rel="stylesheet" type="text/css" runat="server" id="cssPortal_iQor" visible="false"/>    
    <link href="~/styles/Portal_SPX.css" rel="stylesheet" type="text/css" runat="server" id="cssPortal_SPX" visible="false"/>    
    <link href="~/styles/default/custom.css" rel="stylesheet" type="text/css" runat="server" id="cssPortal" visible="false"/>    

    <link href="~/styles/Portal_KDR.css" rel="Stylesheet" type="text/css" runat="server" id="cssPortal_KDR" visible="false" />    
    
    <link href="~/MatrycaSzkolen/Styles/MS.css" rel="Stylesheet" type="text/css" runat="server" id="cssMS" visible="false" />    



    <link href="~/styles/SPX_DEMO.css" rel="stylesheet" type="text/css" runat="server" id="Link1" visible="true"/>    
<%--
--%>


    <link href="~/styles/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.7.1/jquery.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <script src="scripts/common.js" type="text/javascript" ></script>

    <base target="_self" />
    
    <script type="text/javascript">
        function initialize() {
            resize2();
        }
    </script>
</head>
<body onload="initialize();" onresize="resize();">
<form id="form1" class="error_form" runat="server">
    <div id="mainformcontent">
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
    </script>
</form>
</body>
</html>


