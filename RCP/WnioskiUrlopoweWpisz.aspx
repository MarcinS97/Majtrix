<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WnioskiUrlopoweWpisz.aspx.cs" Inherits="HRRcp.WnioskiUrlopoweWpisz" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWnioskiUrlopowe.ascx" tagname="cntWnioskiUrlopowe" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWniosekUrlopowy.ascx" tagname="cntWniosekUrlopowy" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <div class="paWnioskiUrlopoweWpisz">
        <div class="paWnioskiUrlopoweWpisz2">

            <table class="caption" >
                <tr>
                    <td>
                        <span class="caption4">
                            <img alt="" src="images/captions/layout_edit.png"/>
                            Wprowadzanie wniosków urlopowych
                        </span>
                    </td>
                    <td align="right">
                    </td>
                </tr>
            </table>     

            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <asp:Menu ID="tabWnioski" runat="server" Orientation="Horizontal" 
                        onmenuitemclick="tabWnioski_MenuItemClick" >
                        <StaticMenuStyle CssClass="tabsStrip" />
                        <StaticMenuItemStyle CssClass="tabItem" />
                        <StaticSelectedStyle CssClass="tabSelected" />
                        <StaticHoverStyle CssClass="tabHover" />
                        <Items>
                            <asp:MenuItem Text="Wnioski do wprowadzenia" Value="vDoWprowadzenia" Selected="True"></asp:MenuItem>
                            <asp:MenuItem Text="Wprowadzone" Value="vWprowadzone"></asp:MenuItem>
                            <asp:MenuItem Text="Do wyjaśnienia" Value="vDoWyjasnienia"></asp:MenuItem>
                            <%--
                            <asp:MenuItem Text="Absencje bez wniosków" Value="vBezWnioskow"></asp:MenuItem>
                            --%>
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
                    
                    <div class="tabsContent" >
                        <asp:MultiView ID="mvWnioski" runat="server" ActiveViewIndex="0">
                            
                            <asp:View ID="vDoWprowadzenia" runat="server" onactivate="vDoWprowadzenia_Activate">
                                <div class="padding">
                                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe1" Status="3" Mode="21" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" OnDataBound="cntWnioskiUrlopowe1_DataBound" OnChanged="cntWnioskiUrlopowe1_Changed" runat="server" />
                                    <asp:Button ID="btNext" CssClass="button" runat="server" Text="Wprowadzony, zaznacz następny" onclick="btNext_Click" Visible="false"/>
                                    <%--
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    --%>
                                </div>
                            </asp:View>

                            <asp:View ID="vWprowadzone111" runat="server" onactivate="vWprowadzone_Activate">
                                <div class="padding">
                                </div>
                            </asp:View>

                            <asp:View ID="vDoWprowadzenia111" runat="server" onactivate="vWprowadzone_Activate">
                                <div class="padding">
                                </div>
                            </asp:View>

                            <asp:View ID="vWprowadzone" runat="server" onactivate="vWprowadzone_Activate">
                                <div class="padding">                                    
                                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe2" Mode="22" Filter="1" OnShow="cntWnioskiUrlopowe1_Show" OnDataBound="cntWnioskiUrlopowe1_DataBound" OnChanged="cntWnioskiUrlopowe1_Changed" runat="server" />
                                </div>
                            </asp:View>
                            
                            <asp:View ID="vDoWyjasnienia" runat="server" onactivate="vDoWyjasnienia_Activate">
                                <div class="padding">
                                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe3" Mode="23" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" OnDataBound="cntWnioskiUrlopowe1_DataBound" OnChanged="cntWnioskiUrlopowe1_Changed" runat="server" />
                                </div>
                            </asp:View>

                        </asp:MultiView>
                        <div class="rozpychacz"></div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btPopup" runat="server" style="display: none;" />
                    <asp:ModalPopupExtender ID="extWniosekPopup" runat="server" 
                        TargetControlID="btPopup"
                        PopupControlID="paWniosekPopup" 
                        BackgroundCssClass="wnModalBackground" >
                        <Animations>
                            <OnShown>
                                <ScriptAction Script="popupEventHandler();" />
                            </OnShown>
                        </Animations>
                    </asp:ModalPopupExtender>                        
                    <asp:Panel ID="paWniosekPopup" runat="server" CssClass="wnModalPopup wniosekPopup" style="display: none;" >
                        <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
    
            <asp:UpdateProgress ID="updProgress1" runat="server" DisplayAfter="10">
                <ProgressTemplate>
                    <div class="updProgress1">
                        <div class="center">
                            <img alt="Indicator" src="images/activity.gif" /> 
                            <span>Trwa przetwarzanie. Proszę czekać ...</span>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>
    </div>
</asp:Content>

























































<%--


    <div class="page960 startform">
        <div class="spacer16"></div>

        <div id="divPrzesuniecia" runat="server" class="border fiszka" visible="false">
            <uc1:PageTitle ID="PageTitle1" runat="server"
                Ico="../images/captions/layout_edit.png"
                Title="Przesunięcia pracowników do akceptacji"
            />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <uc2:cntPrzypisania ID="cntPrzypisania" Status="0" Mode="0" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="divWnioskiAdm" runat="server" class="border fiszka" visible="false">
            <uc1:PageTitle ID="PageTitle3" runat="server"
                Ico="../images/captions/layout_edit.png"
                Title="Wnioski urlopowe do wprowadzenia"
            />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopoweAdm" Status="3" Mode="2" PageSize="5" OnShow="cntWnioskiUrlopoweAdm_Show" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="buttons">
                <asp:Button ID="btEnter" CssClass="button250" runat="server" 
                    Text="Wprowadź wnioski" onclick="btEnter_Click" />
            </div>
        </div>

        <div id="divWnioskiKier" runat="server" class="border fiszka" visible="false">
            <uc1:PageTitle ID="PageTitle4" runat="server"
                Ico="../images/captions/layout_edit.png"
                Title="Wnioski urlopowe do akceptacji"
            />
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopoweKier" Status="1" Mode="1" PageSize="5" OnShow="cntWnioskiUrlopoweKier_Show" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btPopup" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="extWniosekPopup" runat="server" 
                    TargetControlID="btPopup"
                    PopupControlID="paWniosekPopup" 
                    BackgroundCssClass="wnModalBackground" >
                    <Animations>
                        <OnShown>
                            <ScriptAction Script="popupEventHandler();" />
                        </OnShown>
                    </Animations>
                </asp:ModalPopupExtender>                        
                    
                <asp:Panel ID="paWniosekPopup" runat="server" CssClass="wnModalPopup wniosekPopup" style="display: none;" >
                    <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />                        
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="divZastepstwa" runat="server" class="border fiszka" visible="false">
            <uc1:PageTitle ID="PageTitle2" runat="server"
                Ico="../images/captions/layout_edit.png"
                Title="Kierownicy, których aktualnie możesz zastąpić"
            />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc2:Zastepstwa ID="Zastepstwa" Mode="0" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="divNothingToDo" runat="server" class="border fiszka smile" visible="false">
            Brak czynności do wykonania
            <%--
            <uc1:PageTitle ID="PageTitle5" runat="server"
                Ico="../images/captions/layout_edit.png"
                Title="Informacja dla Ciebie"
            />
            <hr />
            <div class="info" >
                <img src="images/smile.png" alt=""/>
                <span>
                    Dzisiaj jest jeden z tych nielicznych dni,<br />kiedy nie masz nic zaplanowanego do zrobienia w systemie.
                </span>
            </div>
            
            - -%>
        </div>
    </div>
    --%>