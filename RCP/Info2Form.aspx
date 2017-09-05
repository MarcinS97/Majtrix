<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info2Form.aspx.cs" Inherits="HRRcp.Info2Form" %>
<%@ Register src="~/Controls/FooterControl.ascx" tagname="FooterControl" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Program Rozwoju Pracowników</title>
    <link href="styles/Controls.css" rel="stylesheet" type="text/css" />
    <link href="styles/master.css" rel="stylesheet" type="text/css" />

    <link href="styles/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.7.1/jquery.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="scripts/common.js"></script>
    <script type="text/javascript">
        function initialize() {
            resize();
        }
    </script>
</head>
<body onload="initialize();" onresize="resize();">
<form id="form2" class="error_form" runat="server">
    <div id="mainformcontent">
        <div id="header">
			<div class="center">
                <div class="logo">
                    <![if gt IE 6]>
                        <img id="logo" src="~/../images/master/PRP_logo_header.png" alt="" />
                    <![endif]>
                    <!--[if lte IE 6]>
                        <div id="logo" style="width: 191px; height: 116px; filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='~/../images/master/PRP_logo_header.png', sizingMethod='scale');" ></div>
                    <![endif]-->
                </div>
            </div>
        </div>
        
        <div id="content">
            <div class="center">
		        <div class="content2">
                    <div>
                        <div class="error_img">
                            <img src="images/captions/info.png" alt=""/>
                        </div>
                        <div class="error_title">
                            INFORMACJA
                        </div>
                        <div class="error_code">
                            <asp:Label ID="lbNoData" runat="server" Text="Brak danych" ></asp:Label>
                            <asp:Literal ID="ltInfo" runat="server" Visible="False"></asp:Literal>
                        </div>
                        
                        <div id="divButtons" class="bottom_buttons" style="padding-top: 20px;" visible="true" runat="server">
                            <asp:Button ID="btBack" runat="server" CssClass="button100" Text="Powrót"/>
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


