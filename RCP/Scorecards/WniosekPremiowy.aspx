<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="WniosekPremiowy.aspx.cs" Inherits="HRRcp.Scorecards.WniosekPremiowy" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>

<%@ Register Src="~/Scorecards/Controls/Admin/SpreadsheetsParameters/cntProductivity.ascx" TagPrefix="leet" TagName="Productivity" %>
<%@ Register Src="~/Controls/EliteReports/cntChart.ascx" TagPrefix="leet" TagName="Chart" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        //var canvas = document.getElementById("_CANVAS1337");

//        $(document).on("ready", function() {

//            setTimeout(function() {
//                var dataURL = document.getElementById("_CANVAS1337").toDataURL();
//                $('#divHid input').attr("value", myLineChart.toBase64Image());
//            }, 1000);

//        });
        
        // save canvas image as data url (png format by default)

        // set canvasImg image src to dataURL
        // so it can be saved as an image
//        $(function() {
//         wm.showConfirm("asd", "asdas", "ctl00_ContentPlaceHolder2_btnTest");
//        });

    
    </script>
 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScWnioski">
        <uc1:PageTitle ID="PageTitle1" runat="server"
            Ico="../images/captions/layout_edit.png"
            Title="Wniosek premiowy"
        />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    
                        <leet:Chart ID="Chart" runat="server" Type="Line" Names="name1|name2|name3|name4" Values="10, 50, 30, 40, 30" Width="1000"  />
                    
                         <%--   <asp:Image id="canvasImg" runat="server" CssClass="canvasImg" style="display: none;" />
                            <div id="divHid">
                                <asp:HiddenField id="hidImg" runat="server" />
                            </div>--%>
                        <asp:Button ID="btnPDF" runat="server" OnClick="btnPDF_Click" Text="PDF" />
                        
                        
                        
                        <asp:Button ID="btnTest" runat="server" Text="Test" OnClick="SomeFunction" />
                    
               <%--         <a class="button75"><i class="fa fa-file-pdf-o"></i>asdasd</a>--%>
                        
                        <%--<asp:Button ID="btnasd" runat="server" CssClass="button75" Text="asd" />--%>
                        
                        <%--<leet:Productivity Id="Productivity" runat="server" Visible="true" />--%>
         
                </ContentTemplate> 
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnPDF" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>    
    
    <asp:Button ID="btnYes" runat="server" OnClick="Yes" style="display: none;"  />
    <asp:Button ID="btnNo" runat="server" OnClick="No" style="display: none;"  />
    
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
