<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="StartForm.aspx.cs" Inherits="HRRcp.StartForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/Zastepstwa.ascx" tagname="Zastepstwa" tagprefix="uc2" %>
<%@ Register src="~/Controls/Przypisania/cntPrzypisania.ascx" tagname="cntPrzypisania" tagprefix="uc2" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWnioskiUrlopowe.ascx" tagname="cntWnioskiUrlopowe" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWniosekUrlopowy.ascx" tagname="cntWniosekUrlopowy" tagprefix="uc2" %>
<%@ Register src="~/SzkoleniaBHP/Controls/cntStartBHP.ascx" tagname="cntStartBHP" tagprefix="uc2" %>

<%--
<%@ Register src="Controls/Pracownicy2.ascx" tagname="Pracownicy" tagprefix="uc1" %>
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page960 startform">
        <div class="spacer16"></div>

        <div id="divPrzesuniecia" runat="server" class="border fiszka" visible="false">
            <uc1:PageTitle ID="PageTitle1" runat="server"
                Ico="../images/captions/layout_edit.png"
                Title="Przesunięcia pracowników do akceptacji" 
            />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <uc2:cntPrzypisania ID="cntPrzypisania" Status="0" Mode="0" Filter="0" runat="server" />
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
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopoweAdm" Status="3" Mode="2" PageSize="5" Filter="0" OnShow="cntWnioskiUrlopoweAdm_Show" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="buttons">
                <asp:Button ID="btEnter" CssClass="button250" runat="server" Text="Wprowadź wnioski" onclick="btEnter_Click" />
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

        <div id="divSzkoleniaBHP" runat="server" class="border fiszka" visible="false">
            <uc1:PageTitle ID="PageTitle5" runat="server"
                Ico="../images/captions/layout_edit.png"
                Title="Wygasające i przeterminowane szkolenia BHP"
            />
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <uc2:cntStartBHP ID="cntStartBHP" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="buttons">
                <asp:Button ID="btStartBHP" CssClass="button250" runat="server" Text="Pokaż szkolenia" onclick="btStartBHP_Click" />
            </div>
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
            
            --%>
        </div>
    </div>
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
</asp:Content>

