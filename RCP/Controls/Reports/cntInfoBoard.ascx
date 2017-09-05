<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntInfoBoard.ascx.cs" Inherits="HRRcp.Controls.cntInfoBoard" %>
<%@ Register Src="~/Controls/Reports/cntInfoBox.ascx" TagPrefix="uc1" TagName="cntInfoBox" %>
<%@ Register Src="~/Controls/Reports/cntInfoBoxEdit.ascx" TagPrefix="uc1" TagName="cntInfoBoxEdit" %>

<%--
https://jsfiddle.net/56cd9ya2/
http://tympanus.net/Development/FlipboardPageLayout/?page=0
--%>

<asp:Literal id="scInfoBoard" runat="server">
    <style type="text/css">
        div.cntInfoBoard { margin: 32px; }
        div.cntInfoBox { width: 400px; height: 300px; margin: 16px; display: inline-block; }
        div.cntInfoBox div.infobox { width: 400px; height: 300px; xpadding: 0px 8px 8px 8px; padding: 0px 16px 16px 16px; position: absolute;
            xfont-family: 'Segoe UI';
            font-family: sans-serif;
            font-size: 18px; 
            x-moz-border-radius: 5px;                        
            x-webkit-border-radius: 5px;                         
            x-khtml-border-radius: 5px;                      
            xborder-radius: 5px;                             
            xborder: solid 4px Silver;

            line-height: 1.4;
        }
        /*
        div.cntInfoBox,
        div.cntInfoBox a,
        div.cntInfoBox b,
        div.cntInfoBox span,
        div.cntInfoBox th,
        div.cntInfoBox td
        {
            xfont-family: 'Segoe UI';
            font-family: sans-serif;
            font-size: 18px; 
        }
        */
        div.cntInfoBox div.infobox div.defloading { margin-top: 8px; font-size: 12px; display: inline-block; }
        div.cntInfoBox div.infobox div.defloading img { }

        div.cntInfoBox div.infobox1 { background-color: white; color: #333; display: none; } 
        div.cntInfoBox div.infobox2 { background-color: #0080D5; color: white; display: none; xbackground-color: white; xbackground-color: #D8E0E7; }

        div.cntInfoBox.newline { }
        div.cntInfoBox.undefined { opacity: 0.8; }

        /*----- box template -----*/
        div.cntInfoBox .ibox-title { border-bottom: solid 2px #00A2E8; text-align: right; }
        div.cntInfoBox .ibox-title span { font-weight: bold; float: left; display: inline-block; xborder: solid 1px red; margin-top: 32px; color: #00A2E8; }
        div.cntInfoBox .ibox-title i.glyphicon { font-size: 36px; margin: 0px; padding: 8px; xvertical-align: bottom; color: #00A2E8; }
        div.cntInfoBox table.ibox { width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px; border-width: 0px; }
        div.cntInfoBox table.ibox td.ibox-title { vertical-align: top; padding: 0px; height: 32px; }
        div.cntInfoBox table.ibox td.ibox-content-top { vertical-align: top; padding: 8px 0px 0px 0px; }
        div.cntInfoBox table.ibox td.ibox-content-bottom { vertical-align: bottom; padding: 8px 0px 0px 0px; }
        div.cntInfoBox table.ibox td.ibox-content { vertical-align: middle; padding: 8px 0px 0px 0px; }

        div.cntInfoBox .ibox table { }
        div.cntInfoBox .ibox table th { font-weight: normal; color: gray; xfont-style: italic; }

        div.cntInfoBox .ibox a b { color: black; }

        div.cntInfoBox hr { margin: 4px 0px 4px 0px; border: 0; border-top: solid 1px #00A2E8; }
        div.cntInfoBox .ibox-label,
        div.cntInfoBox .ibox-label-fw { color: gray; }
        div.cntInfoBox .ibox-label-fw { display: inline-block; width: 275px; }
        div.cntInfoBox .num { display: inline-block; text-align: right; width: 80px; }
        div.cntInfoBox div.ibox-show { display: block; }
        div.cntInfoBox span.ibox-show { display: inline-block; }
        div.cntInfoBox table.ibox-show { display: table; }
        div.cntInfoBox .ibox-hide { display: none; }

        div.infobox2 .ibox-title { border-color: #fff; }
        div.infobox2 .ibox a,
        div.infobox2 .ibox a b,
        div.infobox2 .ibox-title span { color: #fff; }
        div.infobox2 .ibox-title i.glyphicon { color: #fff; animation-name: funnyRotate; animation-duration: 1s; }
        div.infobox2 hr { border-color: #fff; }
        div.infobox2 .ibox-label,
        div.infobox2 .ibox-label-fw,
        div.infobox2 .ibox table th { color: #C4E4FF; }
     


        .funnyRotate { animation-name: funnyRotate; animation-duration: 1s; }
        @keyframes funnyRotate { 0%   {transform: rotate(0deg);} 33%  {transform: rotate(30deg);} 66%  {transform: rotate(-30deg);} 100% {transform: rotate(0deg);} }




        /*
            div.infobox1,
        div.infobox1 a,
        div.infobox1 b,
        div.infobox1 p,
        div.infobox1 span,
        div.infobox2,
        div.infobox2 a,
        div.infobox2 b,
        div.infobox2 p, 
        div.infobox2 span { 
          xxxxfont-family: 'Segoe UI';
          xfont-family: sans-serif;
          xfont-size: 18px; 
        }
        
        */










        /*----- edit zoom -----*/
        div.cntInfoBox div.infoboxedit { position: absolute; width: 16px; height: 16px; xpadding-left: 16px; xbackground-color: red; z-index: 1; white-space: nowrap; overflow: hidden; white-space: nowrap; }
        div.cntInfoBox div.infoboxedit input.button { padding: 4px 8px; }
        div.cntInfoBox div.infoboxedit:hover { width: auto; height: auto; padding: 4px 4px 4px 0px; background-color: White; xopacity: 0.9; }  
        div.cntInfoBox div.infoboxedit div.editopen { xbackground-color: white; display: inline-block; vertical-align: top; }
        div.cntInfoBox div.infoboxedit:hover div.editopen { xdisplay: none; position: relative; top: -4px; }
        div.cntInfoBox div.infoboxedit div.pacman16 { width: 0px; height: 0px; border-right: 8px solid transparent; border-top: 8px solid red; border-left: 8px solid red; border-bottom: 8px solid red; border-top-left-radius: 8px; border-top-right-radius: 8px; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px; }
        div.cntInfoBox div.infoboxedit div.diamond16 { width: 0; height: 0; border: 8px solid transparent; border-bottom-color: red; position: relative; top: -8px; } 
        div.cntInfoBox div.infoboxedit div.diamond16:after { content: ''; position: absolute; left: -8px; top: 8px; width: 0; height: 0; border: 8px solid transparent; border-top-color: red; }

        div.cntInfoBox div.infoboxedit div.triangleR16 { width: 0; height: 0; border-top: 8px solid transparent; border-left: 16px solid #F0F0F0; border-bottom: 8px solid transparent; } 
        div.cntInfoBox div.infoboxedit div.triangleD16 { width: 0; height: 0; border-left: 8px solid transparent; border-right: 8px solid transparent; border-top: 16px solid red; }

        div.cntInfoBoxEditZoom { padding: 0px; xborder: solid 1px red; }
        div.cntInfoBoxEdit { line-height: 40px; }
        div.cntInfoBoxEdit table.tabsStrip { margin-bottom: 16px; }
    
        div.cntInfoBoxEdit span.label1 { display: block; line-height: normal; margin-top: 4px; }
        div.cntInfoBoxEdit span.label2 { display: inline-block; width: 120px; }
        div.cntInfoBoxEdit span.label3 { display: inline-block; }

        div.cntInfoBoxEdit input.num { width: 80px; }
        div.cntInfoBoxEdit input.edit400 { width: 400px; }
        div.cntInfoBoxEdit div.bottom_buttons { text-align: right; }
        
        div.cntInfoBoxEdit .form-control { font-size: 14px !important; 
                                           display: inline-block !important;

        }




        div.cntInfoBoxEdit span.check { border: none !important; box-shadow: none; vertical-align: middle; margin: 0px; } 
        div.cntInfoBoxEdit span.check input { width: 16px !important; height: 16px !important; vertical-align: middle; background-color: white; } 
        div.cntInfoBoxEdit input[type="text"],
        div.cntInfoBoxEdit textarea { padding: 2px 4px 2px 4px; } 
    </style>
    <%--
        .label jest nadpisana w MS.css :(
        https://css-tricks.com/examples/ShapesOfCSS/
    --%>
    <script type="text/javascript">
        $(document).ready(function () {
            var d = 100;
            var f = 500;
            var dd = 0;
            $('.cntInfoBox .infobox1').each(function () {
                $(this).delay(dd).fadeIn(f);
                dd += d;
            });
            prepareInfoBoxes();
        });

        function prepareInfoBoxes() {
            var f2 = 500;
            $('.cntInfoBox').mouseenter(function () {
                $(this).find('.infobox1').fadeOut(f2);
                $(this).find('.infobox2').fadeIn(f2);
            });

            $('.cntInfoBox').mouseleave(function () {
                $(this).find('.infobox2').fadeOut(f2);
                $(this).find('.infobox1').fadeIn(f2);
            });
        }

        function showInfoBoxes() {
            $('.cntInfoBox .infobox1').each(function () {
                $(this).show();
            });
        }
        /*
        function getInfoBoxesData() {
            $('.cntInfoBox').each(function () {
                $(this).find('.').fadeOut(f);
                $(this).find('.infobox1').fadeIn(f);
                $(this).find('.infobox2').fadeOut(f);

                UpdateInfoBox()

                $(this).
                dd += d;
            });
        }
        */
    </script>
</asp:Literal>

<div id="paInfoBoard" runat="server" class="cntInfoBoard">
    <asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidRights" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidMode" runat="server" Visible="false"/>    
    <asp:HiddenField ID="hidShowAll" runat="server" Visible="false"/>    
    <asp:Repeater ID="rpBoxes" runat="server" DataSourceID="SqlDataSource3" OnItemCommand="rpBoxes_ItemCommand" >
        <ItemTemplate>
            <uc1:cntInfoBox runat="server" ID="cntInfoBox" Data='<%# Container.DataItem %>'/>
        </ItemTemplate>
    </asp:Repeater>
    <uc1:cntInfoBoxEdit ID="cntInfoBoxEdit" runat="server" Visible="false" OnSave="cntInfoBoxEdit_Save"/>
</div>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    onselected="SqlDataSource3_Selected"
    SelectCommand="
select
  A.Id, A.Grupa, A.ParentId, A.Typ, A.Command
, A.Html1, A.Html2, A.HtmlEmpty, A.CssClass, A.Css, A.Script, A.NowaLinia
--, A.Sql, A.Par1, A.Par2
, A.Kolejnosc, A.Aktywny, A.Rights
from SqlBoxes A
where A.Grupa = @Grupa 
  and (A.ParentId is null or A.ParentId = 0) 
  and (@All = 1 or A.Aktywny = 1) 
  and (@All = 1 or A.Rights is null or dbo.CheckRightsExpr(@Rights, A.Rights) = 1) 
  and (@All = 1 or A.Mode is null or A.Mode & @Mode != 0)
order by A.Kolejnosc, A.Id
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" Type="string" />
        <asp:ControlParameter ControlID="hidRights" Name="Rights" Type="string" />
        <asp:ControlParameter ControlID="hidMode" Name="Mode" Type="Int32" />
        <asp:ControlParameter ControlID="hidShowAll" Name="All" Type="Int32" DefaultValue="0" />
        <asp:SessionParameter DefaultValue="PL" Name="lng" SessionField="LNG" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
select
A.Id, A.Grupa, A.ParentId, A.Command, A.Html, A.CssClass, A.NowaLinia, A.Sql, A.Par1, A.Par2, A.Kolejnosc, A.Aktywny, A.Rights,
B.Id bId, B.Grupa bGrupa, B.ParentId bParentId, B.Command bCommand, B.Html bHtml, B.CssClass bCssClass, B.NowaLinia bNowaLinia, B.Sql bSql, B.Par1 bPar1, B.Par2 bPar2, B.Kolejnosc bKolejnosc, B.Aktywny bAktywny, B.Rights bRights
from SqlBoxes A
left join SqlBoxes B on B.ParentId = A.Id and B.Aktywny = 1
where A.Grupa = @Grupa and (A.ParentId is null or A.ParentId = 0) and A.Aktywny = 1 and (A.Rights is null or dbo.CheckRightsExpr(@Rights, A.Rights) = 1) and (A.Mode is null or A.Mode & @Mode != 0)
order by A.Kolejnosc, A.Id
--%>
