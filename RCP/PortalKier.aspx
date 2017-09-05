<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="PortalKier.aspx.cs" Inherits="HRRcp.PortalKier" ValidateRequest="false" %>
<%@ Register src="~/Portal/Controls/cntArticles.ascx" tagname="cntArticles" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    
    <script type="text/javascript">
        $(document).ready(function() {
            $("#LeftPanel").css({
                'left': '-1000px',
                'visibility': 'visible'
            });

            $("#RightPanel").css({
                'left': '1000px',
                'visibility': 'visible'
            });

            $("#ctl00_imgLogo").hide();

            $("#LeftPanel").animate({ left: "0px" }, 1000);
            $("#RightPanel").animate({ left: "0px" }, 1000);
            $("#ctl00_imgLogo").delay(1400).show(0);
        });
    </script>
--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgArticles border">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>                
                <uc1:cntArticles ID="cntArticles" runat="server" Grupa="ARTYKULY" Title="Artykuły"/>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>



<%--
<%@ Register src="~/Controls/Portal/cntCarousel.ascx" tagname="cntCarousel" tagprefix="uc1" %>
--%>
<%--
    <uc1:cntCarousel ID="cntCarousel1" cId="carousel1" Menu="CAROUSEL_K" runat="server" />                
--%>
<%--
    <div id="tstButtons">
        <div>
            <asp:Button ID="bt1" runat="server" Text="1" onclick="bt1_Click" />
            <asp:Button ID="bt2" runat="server" Text="2" onclick="bt2_Click" />
            <asp:Button ID="bt3" runat="server" Text="3" onclick="bt3_Click" />
        </div>
    </div>


    
    <div id="pgPortalStart" class="pgPortalStart" runat="server" visible="true">
        <div id="paIFrame" runat="server" >
            <div class="buttons">
                <div>
                    <span>
                        Informacje
                    </span>
                </div>
                <asp:Button ID="btOpen" runat="server" 
                    CssClass="button"
                    Text="Otwórz stronę w nowym oknie" 
                    onclick="btOpen_Click" 
                    OnClientClick="javascript:window.open('https://sites.google.com/a/jabil.com/bydgoszcz-intranet/','_new');return false;" />
            </div>
            <iframe id="frame1" class="iframe-1" runat="server" src="https://sites.google.com/a/jabil.com/bydgoszcz-intranet/" ></iframe>
        </div>
    </div>


    --%>

<%--

    <script type="text/JavaScript" src="scripts/cloud-carousel.1.0.5.js"></script>
    <script type="text/JavaScript" >
        $(document).ready(function() {
            $("#carousel1").CloudCarousel(
		    {
		        xPos: 600,
		        yPos: 160,
		        buttonLeft: $("#left-but"),
		        buttonRight: $("#right-but"),
		        altBox: $("#alt-text"),
		        titleBox: $("#title-text"),

		        xRadius: 600,
		        yRadius: 150,

                reflHeight:50,
                reflOpacity:0.5,
                reflGap:30,
                minScale:0.5,

		        mouseWheel: false,
		        bringToFront: true,
		        autoRotate: 'left',
		        speed: 0.05,

                FPS: 30,
		        autoRotateDelay: 4000		        
		    });
        });
    </script>

    <script type="text/javascript">
        function x_resizeIframe(obj) {
            alert(obj.style.height);
            var h = obj.contentWindow.document.body.scrollHeight;  //+ 'px';
            alert(h);
            obj.style.height = h + 'px';
            alert(obj.style.height);
        }

        function xx_resizeIframe(frame) {
            var maxW = frame.scrollWidth;
            var minW = maxW;
            var FrameH = 100; //IFrame starting height
            frame.style.height = FrameH + "px"

            while (minW == maxW) {
                FrameH = FrameH + 100; //Increment
                frame.style.height = FrameH + "px";
                minW = frame.scrollWidth;
            }
        }

        function iframeLoaded(iframe) {
            /*  Same Origin Policy - brak dostępu
            var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
            alert(iframeDoc.readyState);
            */ 
        }
                
    </script>        
--%>