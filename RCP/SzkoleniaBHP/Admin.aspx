<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="HRRcp.SzkoleniaBHP.AdminUprawnieniaForm" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/SzkoleniaBHP/Controls/cntAdmUprawnienia.ascx" tagname="cntAdmUprawnienia" tagprefix="uc5" %>

<%--
<%@ Register src="~/SzkoleniaBHP/Controls/cntUprawnieniaCzynnosciEdit2.ascx" tagname="cntUprawnieniaCzynnosciEdit" tagprefix="uc5" %>
<%@ Register src="~/SzkoleniaBHP/Controls/cntUprawnieniaGrupy.ascx" tagname="cntUprawnieniaGrupy" tagprefix="uc5" %>

<%@ Register src="~/Controls/Kierownicy.ascx" tagname="Kierownicy" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmPrzelozeni.ascx" tagname="AdmPrzelozeni" tagprefix="uc1" %>

<%@ Register src="~/Controls/Administracja.ascx" tagname="Administracja" tagprefix="uc3" %>

<%@ Register src="~/Controls/AdmLogControl.ascx" tagname="AdmLogControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmStruktura.ascx" tagname="AdmStruktura" tagprefix="uc2" %>
<%@ Register src="~/Controls/Pracownicy2.ascx" tagname="Pracownicy" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmZadania.ascx" tagname="AdmZadania" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmZastepstwa.ascx" tagname="AdmZastepstwa" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmOddelegowania.ascx" tagname="AdmOddelegowania" tagprefix="uc1" %>

<%@ Register src="~/Controls/ScalControl.ascx" tagname="ScalControl" tagprefix="uc4" %>
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="pgAdministracja2" class="mwrapper">
        <div class="mcontent">
            <table class="caption">
                <tr>
                    <td class="left" >
                        <span class="caption4">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/captions/layout_edit.png"/>
                            <asp:Label ID="lbTitle" CssClass="title" runat="server" Text="Administracja - Szkolenia BHP"></asp:Label>                                                    
                        </span>
                    </td>
                    <td class="middle" rowspan="2">
                    </td>
                    <td class="right" rowspan="2">
                        <asp:Button ID="btBack" CssClass="button100" Text="Powrót" runat="server" />
                    </td>
                </tr>
            </table>



            
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Menu ID="mnTabs" runat="server" Orientation="Horizontal" Visible="false"
                        onmenuitemclick="mnTabs_MenuItemClick" >
                        <StaticMenuStyle CssClass="tabsStrip" />
                        <StaticMenuItemStyle CssClass="tabItem" />
                        <StaticSelectedStyle CssClass="tabSelected" />
                        <StaticHoverStyle CssClass="tabHover" />
                        <Items>
                            <asp:MenuItem Text="Uprawnienia" Value="vTypy|"></asp:MenuItem>
                            <asp:MenuItem Text="Grupy uprawnień" Value="vGrupy|"></asp:MenuItem>
                            <asp:MenuItem Text="Uprawnienia - czynności" Value="vUprawnienia|"></asp:MenuItem>
                        </Items>
                        <StaticItemTemplate>
                            <div class="tabCaption">
                                <div class="tabLeft">
                                    <div class="tabRight">
                                        <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                                    </div>
                                </div>
                            </div>
                        </StaticItemTemplate>
                    </asp:Menu>

                    <div id="tabsContent" runat="server" class="pageContent" >
                        <div class="padding">
                            <asp:MultiView ID="mvAdmin" runat="server" ActiveViewIndex="0">

                                <asp:View ID="vTypy" runat="server" onactivate="vTypy_Activate">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>                                
                                            <div class="vUprawnienia">
                                                <uc5:cntAdmUprawnienia ID="cntAdmUprawnienia" runat="server" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>            
                                </asp:View>

                                <asp:View ID="vUprawnienia" runat="server" onactivate="vUprawnienia_Activate">
    <%--                                
                                    <uc5:cntUprawnieniaCzynnosciEdit ID="cntUprawnieniaCzynnosciEdit1" runat="server" />
    --%>
                                </asp:View>

                                <asp:View ID="vGrupy" runat="server" onactivate="vGrupy_Activate">
    <%--
                                    <uc5:cntUprawnieniaGrupy ID="cntUprawnieniaGrupy" runat="server" />
    --%>
                                </asp:View>

                            </asp:MultiView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--
                    <asp:PostBackTrigger ControlID="btExportTeksty" />
                    <asp:PostBackTrigger ControlID="btImportTeksty" />
                    --%>
                </Triggers>
            </asp:UpdatePanel>
            
        </div>    
    </div>

    <asp:UpdateProgress ID="updProgress1" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img id="Img1" alt="Indicator" src="~/images/activity.gif" runat="server"/> 
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>    
</asp:Content>
