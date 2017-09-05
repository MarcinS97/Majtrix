<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPracownicy3portal.ascx.cs" Inherits="HRRcp.Controls.cntPracownicy3portal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc3" %>
<%@ Register src="~/Controls/TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>
<%@ Register src="cntKartyRcpPopup.ascx" tagname="cntKartyRcpPopup" tagprefix="uc2" %>
<%@ Register src="cntAlgorytmyPopup.ascx" tagname="cntAlgorytmyPopup" tagprefix="uc2" %>
<%@ Register src="cntStanowiskaPopup.ascx" tagname="cntStanowiskaPopup" tagprefix="uc2" %>
<%@ Register src="~/Controls/Przypisania/cntSplityWsp.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>

<asp:HiddenField ID="hidSelectedRcpId" runat="server" />
<asp:HiddenField ID="hidSelectedStrefaId" runat="server" />
<asp:HiddenField ID="hidNaDzien" runat="server" />

<div id="paFilter" runat="server" class="paFilter tbPracownicy2_filter" visible="true">
    <%--<div class="left">
        <span class="label">Wyszukaj:</span>
        <asp:TextBox ID="tbSearch" CssClass="search textbox form-control form-inline" runat="server" ></asp:TextBox>
        <asp:Button ID="btClear" runat="server" CssClass="button75 btn btn-default" Text="Czyść" />
    </div>--%>
    <div class="form-group">
        <div class="input-group">
            <span class="input-group-addon">Wyszukaj:</span>
            <asp:TextBox ID="tbSearch" CssClass="search textbox form-control form-inline" runat="server"></asp:TextBox>
            <div class="input-group-btn">
                <asp:Button ID="btClear" runat="server" CssClass="button75 btn btn-default" Text="Czyść" />
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false">
        <ContentTemplate>
            <asp:Button ID="btPrzesuniecia" runat="server" CssClass="button125" Text="► Przesunięcia" Enabled="false" onclick="btPrzesuniecia_Click" />
            <asp:Button ID="btPlanPracy" runat="server" CssClass="button125" Text="► Plan pracy" Enabled="false" onclick="btPlanPracy_Click" />
            <asp:Button ID="btAkceptacja" runat="server" CssClass="button125" Text="► Akceptacja" Enabled="false" onclick="btAkceptacja_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>    
</div>

<div class="tbPracownicy2_topmenu">
    <asp:Menu ID="tabFilter" runat="server" Orientation="Horizontal" 
        onmenuitemclick="tabFilter_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip mnFilter" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <Items>
            <asp:MenuItem Text="Zatrudnieni" Value="Status >= 0" Selected="True"></asp:MenuItem>
            <asp:MenuItem Text="Przełożeni" Value="Status >= 0 and Kierownik = 1" ></asp:MenuItem>
            <asp:MenuItem Text="Nieprzypisani" Value="Status >= 0 and IdPrzypisania is null"></asp:MenuItem>
            <asp:MenuItem Text="Brak RcpId" Value="Status >= 0 and RcpId is null"></asp:MenuItem>            
            <asp:MenuItem Text="Zwolnieni" Value="Status = -1"></asp:MenuItem> 
            <asp:MenuItem Text="Pomijani" Value="Status = -2"></asp:MenuItem>
            <asp:MenuItem Text="Wszyscy" Value="  "></asp:MenuItem>
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

    <asp:Menu ID="tabContent" runat="server" Orientation="Horizontal" 
        onmenuitemclick="tabContent_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip mnContent" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <Items>
            <asp:MenuItem Text="Przypisanie" Value="0" Selected="True"></asp:MenuItem>
            <asp:MenuItem Text="Uprawnienia" Value="1"></asp:MenuItem>
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
</div>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />

<asp:ListView ID="lvPracownicy" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" 
    onlayoutcreated="lvPracownicy_LayoutCreated" 
    onitemdatabound="lvPracownicy_ItemDataBound" 
    onitemcommand="lvPracownicy_ItemCommand" 
    onprerender="lvPracownicy_PreRender" 
    onitemupdating="lvPracownicy_ItemUpdating" 
    onselectedindexchanged="lvPracownicy_SelectedIndexChanged" 
    oniteminserting="lvPracownicy_ItemInserting" 
    onitemcreated="lvPracownicy_ItemCreated" 
    onitemupdated="lvPracownicy_ItemUpdated" 
    ondatabinding="lvPracownicy_DataBinding" 
    ondatabound="lvPracownicy_DataBound" 
    onitemediting="lvPracownicy_ItemEditing" 
    oniteminserted="lvPracownicy_ItemInserted" 
    onitemdeleting="lvPracownicy_ItemDeleting" 
            onselectedindexchanging="lvPracownicy_SelectedIndexChanging" >
    <ItemTemplate>
        <tr class="it" id="trLine" runat="server">
            <%-- po zmianach przenieść do SelectedItemTemplate od tąd, zmienić CommandName na DeSelect --%>
            <td class="nazwisko_nrew">
                <asp:LinkButton ID="NazwiskoLinkButton" runat="server" Text='<%# Eval("NazwiskoImie") %>' CommandName="Select" CommandArgument='<%# Eval("RcpId") + "|" + Eval("StrefaId") %>' ></asp:LinkButton><br />
                <span class="line2">
                    <asp:Label ID="KadryIdLabel" runat="server" CssClass="left" Text='<%# Eval("KadryId") %>' />&nbsp;
                    <asp:Label ID="RcpLabel" runat="server" Text='<%# Eval("RcpId") %>' ToolTip='<%# GetOdDo(Eval("KartaOd"), Eval("KartaDo")) %>' />
                    <asp:Label ID="RcpLabel2" runat="server" Text='<%# Eval("NrKarty") %>' ToolTip='<%# GetOdDo(Eval("KartaOd"), Eval("KartaDo")) %>' Visible="false" />
                    <asp:ImageButton ID="ibtEditRcpId" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editrcpid" Visible="false"/>
                </span>
            </td>
            <td class="check">
                <asp:CheckBox ID="IsKierCheckBox" class="check" runat="server" Checked='<%# Eval("Kierownik") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Eval("Mailing") %>' Enabled="false" ToolTip='<%# Eval("Email") %>' />
            </td>

            <td class="status">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' /><br />
                <asp:Label ID="lbDataZatr" runat="server" CssClass="line2 zatr" Text='<%# Eval("DataZatr", "{0:d}") %>' />
                <asp:Label ID="lbDataZwol" runat="server" CssClass="line2 zwol" Text='<%# Eval("DataZwol", "{0:d}") %>' />
            </td>

            <td class="strefa" id="tdStrefa" runat="server" >
                <asp:Label ID="StrefaLabel" runat="server" Text='<%# Eval("Strefa") %>' ToolTip='<%# Eval("Strefa") %>' /><br />
                <asp:Label ID="AlgLabel" runat="server" CssClass="line2" Text='<%# Eval("Algorytm") %>' ToolTip='<%# GetOdDo(Eval("AlgorytmOd"), Eval("AlgorytmDo")) %>' />
            </td>
            
            <td class="kierownik" id="tdKierownik" runat="server" >
                <asp:Label ID="KierownikLabel" CssClass="line1" runat="server" Text='<%# Eval("KierownikNI") %>' />&nbsp;<br />
                <span class="line2" id="tdKierownikLine2" runat="server">
                    <asp:Label ID="lbCommodity" runat="server" Text='<%# Eval("Commodity") %>' /> <span class="divider">•</span>
                    <asp:Label ID="lbArea" runat="server" Text='<%# Eval("Area") %>' /> <span class="divider">•</span>
                    <asp:Label ID="lbPosition" runat="server" Text='<%# Eval("Position") %>' />
                </span>
            </td>
            <%--
            •►→
            --%>
            <td class="dzial" id="tdDzial" runat="server" >
                <asp:Label ID="DzialNameLabel" runat="server" Text='<%# Eval("Dzial") %>' /><br />
                <asp:Label ID="StanowiskoLabel" CssClass="line2" runat="server" Text='<%# Eval("Stanowisko") %>' ToolTip='<%# Eval("Stanowisko") %>' />
            </td>
            
            <td id="td111" runat="server" class="check">
                <asp:CheckBox ID="cbAdmin" class="check" runat="server" Checked='<%# Eval("Admin") %>' Enabled="false" />
            </td>

            <td id="tdR1" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR1" runat="server" Enabled="false" />
            </td>
            <td id="tdR2" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR2" runat="server" Enabled="false" />
            </td>
            <td id="tdR3" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR3" runat="server" Enabled="false" />
            </td>

            <td id="td112" runat="server" class="check">
                <asp:CheckBox ID="cbRaporty" class="check" runat="server" Checked='<%# Eval("Raporty") %>' Enabled="false" />
            </td>

            <td id="tdR4" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR4" runat="server" Enabled="false" />
            </td>
            <td id="tdR5" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR5" runat="server" Enabled="false" />
            </td>
            <td id="tdR6" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR6" runat="server" Enabled="false" />
            </td>
            <td id="tdR7" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR7" runat="server" Enabled="false" />
            </td>
            <td id="tdR8" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR8" runat="server" Enabled="false" />
            </td>
            <td id="tdR9" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR9" runat="server" Enabled="false" />
            </td>
            <td id="tdR10" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR10" runat="server" Enabled="false" />
            </td>
            <td id="tdR11" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR11" runat="server" Enabled="false" />
            </td>
            <td id="tdR12" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR12" runat="server" Enabled="false" />
            </td>
            <td id="tdR13" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR13" runat="server" Enabled="false" />
            </td>
            <td id="tdR14" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR14" runat="server" Enabled="false" />
            </td>
            <td id="tdR15" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR15" runat="server" Enabled="false" />
            </td>
            <td id="tdR16" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR16" runat="server" Enabled="false" />
            </td>
            <td id="tdR17" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR17" runat="server" Enabled="false" />
            </td>
            <td id="tdR18" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR18" runat="server" Enabled="false" />
            </td>
            <td id="tdR19" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR19" runat="server" Enabled="false" />
            </td>
            <td id="tdR20" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR20" runat="server" Enabled="false" />
            </td>
            <td id="tdR21" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR21" runat="server" Enabled="false"  />
            </td>
            <td id="tdR22" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR22" runat="server" Enabled="false"  />
            </td>
            <td id="tdR23" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR23" runat="server" Enabled="false"  />
            </td>
            <td id="tdR24" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR24" runat="server" Enabled="false"  />
            </td>
            <td id="tdR25" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR25" runat="server" Enabled="false"  />
            </td>
            <td id="tdR26" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR26" runat="server" Enabled="false" />
            </td>
            <td id="tdR27" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR27" runat="server" Enabled="false" />
            </td>
            <td id="tdR28" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR28" runat="server" Enabled="false" />
            </td>
            <td id="tdR29" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR29" runat="server" Enabled="false" />
            </td>
            <td id="tdR30" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR30" runat="server" Enabled="false" />
            </td>
            <td id="tdR31" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR31" runat="server" Enabled="false"  />
            </td>
            <td id="tdR32" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR32" runat="server" Enabled="false"  />
            </td>
            <td id="tdR33" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR33" runat="server" Enabled="false"  />
            </td>
            <td id="tdR34" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR34" runat="server" Enabled="false"  />
            </td>
            <td id="tdR35" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR35" runat="server" Enabled="false"  />
            </td>
            <td id="tdR36" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR36" runat="server" Enabled="false" />
            </td>
            <td id="tdR37" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR37" runat="server" Enabled="false" />
            </td>
            <td id="tdR38" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR38" runat="server" Enabled="false" />
            </td>
            <td id="tdR39" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR39" runat="server" Enabled="false" />
            </td>
            <td id="tdR40" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR40" runat="server" Enabled="false" />
            </td>
            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" Text="Edytuj" CssClass="btn btn-default" CommandName="Edit" />
                <asp:Button ID="btEdit" runat="server" CssClass="button_postback" CommandName="Edit" Text="Edytuj" />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false" CssClass="btn btn-danger" />
            </td>            
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="sit" id="trLine" runat="server">
            <td class="nazwisko_nrew">
                <asp:LinkButton ID="NazwiskoLinkButton" runat="server" Text='<%# Eval("NazwiskoImie") %>' CommandName="DeSelect" CommandArgument='<%# Eval("RcpId") + "|" + Eval("StrefaId") %>' ></asp:LinkButton><br />
                <span class="line2">
                    <asp:Label ID="KadryIdLabel" runat="server" CssClass="left" Text='<%# Eval("KadryId") %>' />
                    <asp:Label ID="RcpLabel" runat="server" Text='<%# Eval("RcpId") %>' ToolTip='<%# GetOdDo(Eval("KartaOd"), Eval("KartaDo")) %>' />
                    <asp:Label ID="RcpLabel2" runat="server" Text='<%# Eval("NrKarty") %>' ToolTip='<%# GetOdDo(Eval("KartaOd"), Eval("KartaDo")) %>' Visible="false"/>
                </span>
            </td>
            <td class="check">
                <asp:CheckBox ID="IsKierCheckBox" class="check" runat="server" Checked='<%# Eval("Kierownik") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Eval("Mailing") %>' Enabled="false" />
            </td>

            <td class="status">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' /><br />
                <asp:Label ID="lbDataZatr" runat="server" CssClass="line2 zatr" Text='<%# Eval("DataZatr", "{0:d}") %>' />
                <asp:Label ID="lbDataZwol" runat="server" CssClass="line2 zwol" Text='<%# Eval("DataZwol", "{0:d}") %>' />
            </td>

            <td class="strefa" id="tdStrefa" runat="server" >
                <asp:Label ID="StrefaLabel" runat="server" Text='<%# Eval("Strefa") %>' ToolTip='<%# Eval("Strefa") %>' /><br />
                <asp:Label ID="AlgLabel" runat="server" CssClass="line2" Text='<%# Eval("Algorytm") %>' ToolTip='<%# GetOdDo(Eval("AlgorytmOd"), Eval("AlgorytmDo")) %>' />
            </td>
            
            <td class="kierownik" id="tdKierownik" runat="server" >
                <asp:Label ID="KierownikLabel" CssClass="line1" runat="server" Text='<%# Eval("KierownikNI") %>' />&nbsp;<br />
                <span class="line2" id="tdKierownikLine2" runat="server">
                    <asp:Label ID="lbCommodity" runat="server" Text='<%# Eval("Commodity") %>' /> <span class="divider">•</span>
                    <asp:Label ID="lbArea" runat="server" Text='<%# Eval("Area") %>' /> <span class="divider">•</span>
                    <asp:Label ID="lbPosition" runat="server" Text='<%# Eval("Position") %>' />
                </span>
            </td>

            <td class="dzial" id="tdDzial" runat="server" >
                <asp:Label ID="DzialNameLabel" runat="server" Text='<%# Eval("Dzial") %>' /><br />
                <asp:Label ID="StanowiskoLabel" CssClass="line2" runat="server" Text='<%# Eval("Stanowisko") %>' ToolTip='<%# Eval("Stanowisko") %>' />
            </td>
            
            <td id="td111" runat="server" class="check">
                <asp:CheckBox ID="cbAdmin" class="check" runat="server" Checked='<%# Eval("Admin") %>' Enabled="false" />
            </td>

            <td id="tdR1" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR1" runat="server" Enabled="false" />
            </td>
            <td id="tdR2" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR2" runat="server" Enabled="false" />
            </td>
            <td id="tdR3" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR3" runat="server" Enabled="false" />
            </td>

            <td id="td112" runat="server" class="check">
                <asp:CheckBox ID="cbRaporty" class="check" runat="server" Checked='<%# Eval("Raporty") %>' Enabled="false" />
            </td>

            <td id="tdR4" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR4" runat="server" Enabled="false" />
            </td>
            <td id="tdR5" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR5" runat="server" Enabled="false" />
            </td>
            <td id="tdR6" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR6" runat="server" Enabled="false" />
            </td>
            <td id="tdR7" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR7" runat="server" Enabled="false" />
            </td>
            <td id="tdR8" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR8" runat="server" Enabled="false" />
            </td>
            <td id="tdR9" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR9" runat="server" Enabled="false" />
            </td>
            <td id="tdR10" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR10" runat="server" Enabled="false" />
            </td>
            <td id="tdR11" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR11" runat="server" Enabled="false" />
            </td>
            <td id="tdR12" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR12" runat="server" Enabled="false" />
            </td>
            <td id="tdR13" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR13" runat="server" Enabled="false" />
            </td>
            <td id="tdR14" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR14" runat="server" Enabled="false" />
            </td>
            <td id="tdR15" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR15" runat="server" Enabled="false" />
            </td>
            <td id="tdR16" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR16" runat="server" Enabled="false" />
            </td>
            <td id="tdR17" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR17" runat="server" Enabled="false" />
            </td>
            <td id="tdR18" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR18" runat="server" Enabled="false" />
            </td>
            <td id="tdR19" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR19" runat="server" Enabled="false" />
            </td>
            <td id="tdR20" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR20" runat="server" Enabled="false" />
            </td>
            <td id="tdR21" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR21" runat="server" Enabled="false"  />
            </td>
            <td id="tdR22" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR22" runat="server" Enabled="false"  />
            </td>
            <td id="tdR23" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR23" runat="server" Enabled="false"  />
            </td>
            <td id="tdR24" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR24" runat="server" Enabled="false"  />
            </td>
            <td id="tdR25" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR25" runat="server" Enabled="false"  />
            </td>
            <td id="tdR26" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR26" runat="server" Enabled="false" />
            </td>
            <td id="tdR27" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR27" runat="server" Enabled="false" />
            </td>
            <td id="tdR28" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR28" runat="server" Enabled="false" />
            </td>
            <td id="tdR29" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR29" runat="server" Enabled="false" />
            </td>
            <td id="tdR30" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR30" runat="server" Enabled="false" />
            </td>
            <td id="tdR31" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR31" runat="server" Enabled="false"  />
            </td>
            <td id="tdR32" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR32" runat="server" Enabled="false"  />
            </td>
            <td id="tdR33" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR33" runat="server" Enabled="false"  />
            </td>
            <td id="tdR34" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR34" runat="server" Enabled="false"  />
            </td>
            <td id="tdR35" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR35" runat="server" Enabled="false"  />
            </td>
            <td id="tdR36" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR36" runat="server" Enabled="false" />
            </td>
            <td id="tdR37" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR37" runat="server" Enabled="false" />
            </td>
            <td id="tdR38" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR38" runat="server" Enabled="false" />
            </td>
            <td id="tdR39" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR39" runat="server" Enabled="false" />
            </td>
            <td id="tdR40" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR40" runat="server" Enabled="false" />
            </td>
            <td id="tdControl" runat="server" class="control">
<%--                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" /><br />
--%>
                <asp:Button ID="EditButton" runat="server" Text="Edytuj" CssClass="btn btn-default" CommandName="Edit" />
                <asp:Button ID="btEdit" runat="server" CssClass="button_postback" CommandName="Edit" Text="Edytuj" />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false" CssClass="btn btn-danger" />
            </td>            
        </tr>
    </SelectedItemTemplate>
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    <EditItemTemplate>
        <tr class="eit">
            <td colspan="<%= GetEditColSpan() %>" class="col1">
                <table class="edit">
                    <tr>
                        <td class="coled1">
                            <div class="title">
                                <asp:Label ID="Label2" runat="server" Text="Dane podstawowe" />
                            </div>

                            <span class="label">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' CssClass="form-control" />
                            <asp:RequiredFieldValidator ControlToValidate="NazwiskoTextBox" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            <span class="label">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" Text='<%# Bind("Imie") %>' CssClass="form-control" />
                            <asp:RequiredFieldValidator ControlToValidate="ImieTextBox" ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                       
                            <span class="label">Nr ewid.:</span>
                            <asp:TextBox ID="KadryIdTextBox" runat="server" Text='<%# Bind("KadryId") %>' CssClass="form-control" />
                            <asp:RequiredFieldValidator ControlToValidate="KadryIdTextBox" ID="RequiredFieldValidator5" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            
                            <div id="paKadryId2" runat="server" visible="false">
                                <span class="label">Nr ewid. #2:</span>
                                <asp:TextBox ID="tbKadryId2" runat="server" Text='<%# Bind("KadryId2") %>' CssClass="form-control" />
                                <br />
                            </div>
                            
                            <span class="label">RCP Id:</span>                            
                            <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Eval("RcpId") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("KartaOd"), Eval("KartaDo")) %>' CssClass="form-control"/>
                            <asp:TextBox ID="RcpIdTextBox2" runat="server" Text='<%# Eval("NrKarty") %>' Visible="false" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("KartaOd"), Eval("KartaDo")) %>' CssClass="form-control" />
                            <asp:ImageButton ID="ibtEditRcpId" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editrcpid" ToolTip="Zmień"/><br />
                            
                            <span class="label">Login:</span>
                            <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Login") %>' CssClass="form-control" /><br />
                            <span class="label">E-mail:</span>
                            <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" /><br />
                            <span class="label">Mailing:</span>
                            <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Bind("Mailing") %>' /><br />

                            <span class="label">Kierownik:</span>
                            <asp:CheckBox ID="KierownikCheckBox" class="check" runat="server" Checked='<%# Bind("Kierownik") %>' /><br />
                            <%--
                            <span class="label">Admininistrator:</span>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' /><br />
                            <span class="label">Raporty:</span>
                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' /><br />
                            --%>
                            <span class="label">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" ></asp:DropDownList><br />
                            <span class="label">Data zatrudnienia:</span>
                            <uc3:DateEdit ID="deZatr" runat="server" Date='<%# Bind("DataZatr") %>' ValidationGroup="vge" /><br />
                            <span class="label">Data zwolnienia:</span>
                            <uc3:DateEdit ID="deZwol" runat="server" Date='<%# Bind("DataZwol") %>'/><br />
                            <asp:HiddenField ID="hidDataZwol" runat="server" Visible="false" Value='<%# Bind("prevDataZwol") %>'/>
                            <br />
                            <div class="kwitek">
                                <asp:Button ID="btKwitekPassReset" runat="server" class="button btn btn-default" Text="Resetuj hasło do kwitka płacowego" CommandName="passreset" CommandArgument='<%# Eval("Id") %>' />
                            </div>
                        </td>
                        <td class="coled2">
                            <div class="title">
                                <asp:Label ID="Label1" runat="server" Text="Przypisanie" />
                            </div>

                            <span class="label">Dział:</span>
                            <asp:TextBox ID="DzialTextBox" runat="server" Text='<%# Eval("Dzial") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("StanowiskoOd"), Eval("StanowiskoDo")) %>' CssClass="form-control" />
                            <asp:ImageButton ID="ImageButton2" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editstan" ToolTip="Zmień" /><br />
                            <span class="label">Stanowisko:</span>
                            <asp:TextBox ID="StanowiskoTextBox" runat="server" Text='<%# Eval("Stanowisko") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("StanowiskoOd"), Eval("StanowiskoDo")) %>' CssClass="form-control" />
                            <asp:ImageButton ID="ImageButton3" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editstan" ToolTip="Zmień" Visible="false" /><br />
                            <span class="label">Grupa:</span>
                            <asp:TextBox ID="GrupaTextBox" runat="server" Text='<%# Eval("Grupa") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("StanowiskoOd"), Eval("StanowiskoDo")) %>' CssClass="form-control"/>
                            <asp:ImageButton ID="ImageButton10" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editstan" ToolTip="Zmień" Visible="false" /><br />
                            <span class="label">Klasyfikacja:</span>
                            <asp:TextBox ID="KlasyfikacjaTextBox" runat="server" Text='<%# Eval("Klasyfikacja") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("StanowiskoOd"), Eval("StanowiskoDo")) %>' CssClass="form-control"/>
                            <asp:ImageButton ID="ImageButton9" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editstan" ToolTip="Zmień" Visible="false" /><br />
                            <br />

                        
                            <asp:HiddenField ID="hidIdPrzypisania" runat="server" Value='<%# Eval("IdPrzypisania") %>' />
                            <span class="label">Przypisanie:</span>
                            <asp:Label ID="Label3" runat="server" Text='<%# GetOdDo(Eval("PrzypisanieOd"), Eval("PrzypisanieDo")) %>'></asp:Label>
                            <br />
                            
                            <span class="label">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownik" runat="server"
                                DataTextField="PrzKierownik"
                                DataValueField="Id" CssClass="form-control">
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ControlToValidate="ddlKierownik" ID="RequiredFieldValidator6" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" CssClass="error" ErrorMessage="Błąd" 
                                Enabled="false" />
                            <br />

                            <div id="paCommodity" runat="server">
                                <span class="label">Commodity:</span>
                                <asp:DropDownList ID="ddlCommodity" runat="server"
                                    DataTextField="PrzCommodity"
                                    DataValueField="Id" CssClass="form-control">
                                </asp:DropDownList><br />

                                <span class="label">Area:</span>
                                <asp:DropDownList ID="ddlArea" runat="server"
                                    DataTextField="PrzArea"
                                    DataValueField="Id" CssClass="form-control">
                                </asp:DropDownList><br />

                                <span class="label">Position:</span>
                                <asp:DropDownList ID="ddlPosition" runat="server"
                                    DataTextField="PrzPosition"
                                    DataValueField="Id" CssClass="form-control">
                                </asp:DropDownList><br />
                            </div>

                            <span class="label">CC:</span>                
                            <uc6:cntSplityWsp ID="cntSplityWsp2" Mode="1" runat="server" IdPrzypisania='<%# Eval("IdPrzypisania") %>' />
                            <br />

                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server"
                                DataTextField="Nazwa"
                                DataValueField="RcpStrefaId" CssClass="form-control">
                            </asp:DropDownList><br />

                            <br />
                            <span class="label">Algorytm RCP:</span>
                            <asp:TextBox ID="AlgorytmTextBox" runat="server" Text='<%# Eval("Algorytm") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("AlgorytmOd"), Eval("AlgorytmDo")) %>'/>
                            <asp:ImageButton ID="ImageButton1" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editalg" ToolTip="Zmień"/><br />

                            <span class="label label2">Wymiar czasu:</span>
                            <uc1:TimeEdit ID="teWymiar" runat="server" Seconds='<%# Eval("WymiarCzasu") %>' Opis="hh:mm" ReadOnly="true" /><br />
                            <span class="label label2">Dodatkowa przerwa wliczona:</span>
                            <uc1:TimeEdit ID="tePrzerwaWliczona" runat="server" Seconds='<%# Eval("PrzerwaWliczona") %>' Opis="hh:mm" ReadOnly="true" /><br />                            
                            <span class="label label2">Przerwa niewliczona:</span>
                            <uc1:TimeEdit ID="tePrzerwaNiewliczona" runat="server" Seconds='<%# Eval("PrzerwaNiewliczona") %>' Opis="hh:mm" ReadOnly="true" />                            
                            
                            <%--    
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("WymiarHMM") %>' Visible="true" ReadOnly="true" Width="60" ToolTip='<%# GetOdDo(Eval("AlgorytmOd"), Eval("AlgorytmDo")) %>'/>

                            <span class="label">Strefa RCP:</span>
                            <asp:TextBox ID="StrefaTextBox" runat="server" Text='<%# Eval("Strefa") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("AlgorytmOd"), Eval("AlgorytmDo")) %>'/><br />
                            --%>    

                            <%--
                            <span class="label">Kierownik:</span>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Eval("KierownikNI") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("PrzypisanieOd"), Eval("PrzypisanieDo")) %>'/>
                            <asp:ImageButton ID="ImageButton7" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editprzyp" ToolTip="Zmień"/><br />
                            <span class="label">Commodity:</span>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("Commodity") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("PrzypisanieOd"), Eval("PrzypisanieDo")) %>'/>
                            <asp:ImageButton ID="ImageButton4" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editprzyp" ToolTip="Zmień"/><br />
                            <span class="label">Area:</span>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Eval("Area") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("PrzypisanieOd"), Eval("PrzypisanieDo")) %>'/>
                            <asp:ImageButton ID="ImageButton5" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editprzyp" ToolTip="Zmień"/><br />
                            <span class="label">Position:</span>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Eval("Position") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("PrzypisanieOd"), Eval("PrzypisanieDo")) %>'/>
                            <asp:ImageButton ID="ImageButton6" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editprzyp" ToolTip="Zmień"/><br />
                            <span class="label">Strefa RCP:</span>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Eval("Strefa") %>' Visible="true" ReadOnly="true"  ToolTip='<%# GetOdDo(Eval("PrzypisanieOd"), Eval("PrzypisanieDo")) %>'/>
                            <asp:ImageButton ID="ImageButton8" CssClass="ico" runat="server" ImageUrl="~/images/buttons/edit.png" CommandName="editprzyp" ToolTip="Zmień"/><br />
                            <br />
                            --%>


                            <%--
                            <span class="label">Dział:</span>
                            <asp:DropDownList ID="ddlDzial" runat="server" 
                                DataSourceID="SqlDataSource4"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server"
                                DataSourceID="SqlDataSource5"
                                DataTextField="NazwaDS"
                                DataValueField="Id">
                            </asp:DropDownList><br />


                            <span class="label">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownik" runat="server"
                                DataSourceID="SqlDataSource6"
                                DataTextField="NazwaK"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Commodity:</span>
                            <asp:DropDownList ID="ddlLinia" runat="server" 
                                DataSourceID="SqlDataSource8"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            
                            <br />

                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server" 
                                DataSourceID="SqlDataSource2"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Split:</span>
                            <asp:DropDownList ID="ddlSplit" runat="server" 
                                DataSourceID="SqlDataSource7"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            --%>                                            
                            

                            <asp:HiddenField ID="hidRights" runat="server" Value='<%# Bind("Rights") %>'/>

                        </td>
                        <td class="coled3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" />
                            </div>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' Text="<span>A</span> - Administrator" />
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           

                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' Text="<span>R</span> - Raporty" />
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR22" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR23" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR24" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR25" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR26" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR27" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR28" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR29" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR30" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR31" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR32" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR33" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR34" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR35" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR36" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR37" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR38" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR39" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR40" CssClass="check" runat="server" Visible="false" />
                        </td>                                                        
                    </tr>
                </table>
            </td>

            <td class="control" >
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" CssClass="btn btn-success" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" CssClass="btn btn-default" /><br /><br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" CssClass="btn btn-danger" />
            </td>
        </tr>
    </EditItemTemplate>
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    <InsertItemTemplate>
        <tr class="iit">
            <td colspan="<%= GetEditColSpan() %>" class="col1">
                <table class="edit">
                    <tr>
                        <td class="coled1">
                            <div class="title">
                                <asp:Label ID="Label2" runat="server" Text="Dane podstawowe" />
                            </div>

                            <span class="label">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' CssClass="form-control" />
                            <asp:RequiredFieldValidator ControlToValidate="NazwiskoTextBox" ID="RequiredFieldValidator6" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            <span class="label">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" Text='<%# Bind("Imie") %>' CssClass="form-control" />
                            <asp:RequiredFieldValidator ControlToValidate="ImieTextBox" ID="RequiredFieldValidator7" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            <span class="label">Nr ewid.:</span>
                            <asp:TextBox ID="KadryIdTextBox" runat="server" Text='<%# Bind("KadryId") %>' CssClass="form-control" />
                            <asp:RequiredFieldValidator ControlToValidate="KadryIdTextBox" ID="RequiredFieldValidator8" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            
                            <div id="paKadryId2" runat="server" visible="false">
                                <span class="label">Nr ewid. #2:</span>
                                <asp:TextBox ID="tbKadryId2" runat="server" Text='<%# Bind("KadryId2") %>' CssClass="form-control" />
                                <br />
                            </div>
                            
                            <span class="label">RCP Id:</span>                            
                            <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Bind("RcpId") %>' CssClass="form-control"/>
                            <asp:TextBox ID="RcpIdTextBox2" runat="server" Text='<%# Bind("NrKarty") %>' Visible="false" CssClass="form-control" /><br />
                            
                            <span class="label">Login:</span>
                            <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Login") %>' CssClass="form-control" /><br />
                            <span class="label">E-mail:</span>
                            <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" /><br />
                            <span class="label">Mailing:</span>
                            <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Bind("Mailing") %>' /><br />

                            <span class="label">Kierownik:</span>
                            <asp:CheckBox ID="KierownikCheckBox" class="check" runat="server" Checked='<%# Bind("Kierownik") %>' /><br />
                            <%--
                            <span class="label">Admininistrator:</span>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' /><br />
                            <span class="label">Raporty:</span>
                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' /><br />
                            --%>
                            <span class="label">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList><br />
                            <span class="label">Data zatrudnienia:</span>
                            <uc3:DateEdit ID="deZatr" runat="server" Date='<%# Bind("DataZatr") %>' ValidationGroup="vgi" /><br />
                            <span class="label">Data zwolnienia:</span>
                            <uc3:DateEdit ID="deZwol" runat="server" Date='<%# Bind("DataZwol") %>'/><br />
                        </td>
                        <td class="coled2">
                            <div class="title">
                                <asp:Label ID="Label1" runat="server" Text="Przypisanie" />
                            </div>

                            <span class="label">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server" 
                                DataSourceID="SqlDataSourceStan"
                                DataTextField="Stanowisko"
                                DataValueField="IdStanowiska" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlStanowisko" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            
                            <span class="label">Grupa:</span>
                            <asp:DropDownList ID="ddlGrupa" runat="server" 
                                DataSourceID="SqlDataSourceGrupa"
                                DataTextField="SymGrupa"
                                DataValueField="Grupa" CssClass="form-control">
                            </asp:DropDownList>
                            <br />
                            <span class="label">Klasyfikacja:</span>
                            <asp:DropDownList ID="ddlKlasyfikacja" runat="server" 
                                DataSourceID="SqlDataSourceClass"
                                DataTextField="SymKlasyfikacja"
                                DataValueField="Klasyfikacja" CssClass="form-control">
                            </asp:DropDownList>
                            <br />
                            <br />
                        
                            <span class="label">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownik" runat="server"
                                DataSourceID="SqlDataSourceKier"
                                DataTextField="PrzKierownik"
                                DataValueField="Id" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlKierownik" ID="rfvKierownik" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error" ErrorMessage="Błąd" />
                            <br />

                            <div id="paCommodity" runat="server">
                                <span class="label">Commodity:</span>
                                <asp:DropDownList ID="ddlCommodity" runat="server"
                                    DataSourceID="SqlDataSourceComm"
                                    DataTextField="PrzCommodity"
                                    DataValueField="Id" CssClass="form-control">
                                </asp:DropDownList><br />

                                <span class="label">Area:</span>
                                <asp:DropDownList ID="ddlArea" runat="server"
                                    DataSourceID="SqlDataSourceArea"
                                    DataTextField="PrzArea"
                                    DataValueField="Id" CssClass="form-control">
                                </asp:DropDownList><br />

                                <span class="label">Position:</span>
                                <asp:DropDownList ID="ddlPosition" runat="server"
                                    DataSourceID="SqlDataSourcePos"
                                    DataTextField="PrzPosition"
                                    DataValueField="Id" CssClass="form-control">
                                </asp:DropDownList><br />
                            </div>

                            <span class="label">CC:</span>                
                            <uc6:cntSplityWsp ID="cntSplityWsp2" Mode="1" runat="server" />
                            <br />
                            
                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server"
                                DataSourceID="SqlDataSourceStrefa"
                                DataTextField="Nazwa"
                                DataValueField="RcpStrefaId" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlStrefa" ID="RequiredFieldValidator9" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            <br />
                            
                            <span class="label">Algorytm RCP:</span>
                            <asp:DropDownList ID="ddlAlgorytm" runat="server" 
                                DataSourceID="SqlDataSourceAlg"
                                DataTextField="Nazwa"
                                DataValueField="RcpAlgorytm" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlAlgorytm" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vgi" CssClass="error" ErrorMessage="Błąd" />
                            <br />
                            <%--
                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server" 
                                DataSourceID="SqlDataSourceStrefa"
                                DataTextField="Nazwa"
                                DataValueField="RcpStrefaId">
                            </asp:DropDownList><br />
                            --%>

                            <span class="label label2">Wymiar czasu:</span>
                            <uc1:TimeEdit ID="teWymiar" runat="server" Seconds='<%# Bind("WymiarCzasu") %>' Opis="hh:mm" ValidationGroup="vgi" /><br />
                            <span class="label label2">Dodatkowa przerwa wliczona:</span>
                            <uc1:TimeEdit ID="tePrzerwaWliczona" runat="server" Seconds='<%# Bind("PrzerwaWliczona") %>' Opis="hh:mm" ValidationGroup="vgi" /><br />
                            <span class="label label2">Przerwa niewliczona:</span>
                            <uc1:TimeEdit ID="tePrzerwaNiewliczona" runat="server" Seconds='<%# Bind("PrzerwaNiewliczona") %>' Opis="hh:mm" ValidationGroup="vgi" />                            

                            <asp:HiddenField ID="hidRights" runat="server" Value='<%# Bind("Rights") %>'/>
                        </td>
                        <td class="coled3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" />
                            </div>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' Text="<span>A</span> - Administrator" />
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           

                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' Text="<span>R</span> - Raporty" />
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR22" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR23" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR24" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR25" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR26" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR27" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR28" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR29" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR30" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR31" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR32" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR33" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR34" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR35" CssClass="check" runat="server" Visible="false" /> 
                            <asp:CheckBox ID="cbR36" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR37" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR38" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR39" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR40" CssClass="check" runat="server" Visible="false" />
                        </td>                                                        
                    </tr>
                </table>
            </td>
            <td class="control" >
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" ValidationGroup="vgi" CssClass="btn btn-success"/><br />
                <asp:Button ID="CancelInsertButton" runat="server" CommandName="CancelInsert" CssClass="btn btn-default" Text="Anuluj" />
            </td>
        </tr>
    </InsertItemTemplate>
    
    
    
    
    
    
    
    
    
    
    
    
    <EmptyDataTemplate>
        <table id="Table1" runat="server" class="ListView3_edt" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    
    
    
    
    
    
    
    
    
    
    
    
    <LayoutTemplate>
        <table class="hoverline cntPracownicy2" runat="server">
            <tr id="Tr1" runat="server">
                <td id="Td1" colspan="2" runat="server">
                    <table ID="itemPlaceholderContainer" class="tbPracownicy2 table" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" rowspan="2" runat="server" class="nazwisko_nrew">
                                <asp:LinkButton ID="LinkButton31" runat="server" CommandName="Sort" CommandArgument="NazwiskoImie" Text="Pracownik" ToolTip="Sortuj" />
                                <div class="line2">
                                    <asp:LinkButton ID="LinkButton33" CssClass="left" runat="server" CommandName="Sort" CommandArgument="KadryId" Text="Nr ew." ToolTip="Sortuj"/>
                                    <asp:LinkButton ID="LinkButton34" runat="server" CommandName="Sort" CommandArgument="RcpId" Text="RCP ID" ToolTip="Sortuj"/>
                                </div>
                            </th>
                            <th id="Th2" rowspan="2" runat="server"><asp:LinkButton ID="LinkButton32" runat="server" CommandName="Sort" CommandArgument="Kierownik"    Text="Kier" ToolTip="Sortuj" /></th>
                            <th id="Th113" rowspan="2" runat="server" class="check"><asp:LinkButton ID="LinkButton43" runat="server" CommandName="Sort" CommandArgument="Mailing" Text="@" ToolTip="Wysyłanie powiadomień - Sortuj"/></th>
                                
                            <th id="thStatus" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton40" runat="server" CommandName="Sort" CommandArgument="Status"       Text="Status" ToolTip="Sortuj"/><br />
                                <asp:LinkButton ID="LinkButton47" runat="server" CommandName="Sort" CommandArgument="DataZatr"     Text="Data zatr" ToolTip="Sortuj"/> /
                                <asp:LinkButton ID="LinkButton48" runat="server" CommandName="Sort" CommandArgument="DataZwol"     Text="zwol" ToolTip="Sortuj"/>
                            </th>
                
                            <th id="thStrefa" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton35" runat="server" CommandName="Sort" CommandArgument="Strefa"       Text="Strefa" ToolTip="Sortuj"/> /
                                <asp:LinkButton ID="LinkButton36" runat="server" CommandName="Sort" CommandArgument="Algorytm"     Text="Algorytm" ToolTip="Sortuj"/>
                            </th>
                            
                            <th id="thKierownik" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton39" runat="server" CommandName="Sort" CommandArgument="KierownikNI"  Text="Kierownik" ToolTip="Sortuj"/>
                                <%--
                                <br id="thKierownikBr" runat="server" />
                                •
                                --%>
                                <div id="thKierownikLine2" runat="server">
                                    <asp:LinkButton ID="LinkButton44" runat="server" CommandName="Sort" CommandArgument="Commodity"    Text="Commodity" ToolTip="Sortuj"/> /
                                    <asp:LinkButton ID="LinkButton45" runat="server" CommandName="Sort" CommandArgument="Area"         Text="Area" ToolTip="Sortuj"/> /
                                    <asp:LinkButton ID="LinkButton46" runat="server" CommandName="Sort" CommandArgument="Position"     Text="Position" ToolTip="Sortuj"/>
                                </div>
                            </th>
                            
                            <th id="thDzial" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton37" runat="server" CommandName="Sort" CommandArgument="Dzial"        Text="Dział" ToolTip="Sortuj"/> /
                                <asp:LinkButton ID="LinkButton38" runat="server" CommandName="Sort" CommandArgument="Stanowisko"   Text="Stanowisko" ToolTip="Sortuj"/>
                            </th>
                                                                    
                            <th id="thRights" colspan="21" class="top" runat="server" visible="false">
                                Uprawnienia
                            </th>
                            
                            <th id="Th11" class="control" rowspan="2" runat="server">
                                <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj nowy rekord" CssClass="btn btn-success"/>
                            </th>
                        </tr>
                        <tr class="bottom">
                            <th id="th111" runat="server" class="check"><asp:LinkButton ID="LinkButton41" runat="server" CommandName="Sort" CommandArgument="Admin" Text="A" ToolTip="Administrator - Sortuj"/></th>

                            <th id="thR1"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton1"  runat="server"></asp:LinkButton></th>
                            <th id="thR2"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton2"  runat="server"></asp:LinkButton></th>
                            <th id="thR3"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton3"  runat="server"></asp:LinkButton></th>

                            <th id="th112" runat="server" class="check"><asp:LinkButton ID="LinkButton42" runat="server" CommandName="Sort" CommandArgument="Raporty" Text="R" ToolTip="Dostęp do raportów - Sortuj"/></th>
                            
                            <th id="thR4"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton4"  runat="server"></asp:LinkButton></th>
                            <th id="thR5"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton5"  runat="server"></asp:LinkButton></th>
                            <th id="thR6"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton6"  runat="server"></asp:LinkButton></th>
                            <th id="thR7"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton7"  runat="server"></asp:LinkButton></th>
                            <th id="thR8"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton8"  runat="server"></asp:LinkButton></th>
                            <th id="thR9"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton9"  runat="server"></asp:LinkButton></th>
                            <th id="thR10" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton10" runat="server"></asp:LinkButton></th>
                            <th id="thR11" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton11" runat="server"></asp:LinkButton></th>
                            <th id="thR12" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton12" runat="server"></asp:LinkButton></th>
                            <th id="thR13" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton13" runat="server"></asp:LinkButton></th>
                            <th id="thR14" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton14" runat="server"></asp:LinkButton></th>
                            <th id="thR15" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton15" runat="server"></asp:LinkButton></th>
                            <th id="thR16" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton16" runat="server"></asp:LinkButton></th>
                            <th id="thR17" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton17" runat="server"></asp:LinkButton></th>
                            <th id="thR18" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton18" runat="server"></asp:LinkButton></th>
                            <th id="thR19" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton19" runat="server"></asp:LinkButton></th>
                            <th id="thR20" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton20" runat="server"></asp:LinkButton></th>
                            <th id="thR21" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton21" runat="server"></asp:LinkButton></th>
                            <th id="thR22" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton22" runat="server"></asp:LinkButton></th>
                            <th id="thR23" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton23" runat="server"></asp:LinkButton></th>
                            <th id="thR24" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton24" runat="server"></asp:LinkButton></th>
                            <th id="thR25" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton25" runat="server"></asp:LinkButton></th>
                            <th id="thR26" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton26" runat="server"></asp:LinkButton></th>
                            <th id="thR27" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton27" runat="server"></asp:LinkButton></th>
                            <th id="thR28" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton28" runat="server"></asp:LinkButton></th>
                            <th id="thR29" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton29" runat="server"></asp:LinkButton></th>
                            <th id="thR30" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton30" runat="server"></asp:LinkButton></th>
                            <th id="thR31" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton49" runat="server"></asp:LinkButton></th>
                            <th id="thR32" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton50" runat="server"></asp:LinkButton></th>
                            <th id="thR33" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton51" runat="server"></asp:LinkButton></th>
                            <th id="thR34" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton52" runat="server"></asp:LinkButton></th>
                            <th id="thR35" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton53" runat="server"></asp:LinkButton></th>
                            <th id="thR36" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton54" runat="server"></asp:LinkButton></th>
                            <th id="thR37" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton55" runat="server"></asp:LinkButton></th>
                            <th id="thR38" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton56" runat="server"></asp:LinkButton></th>
                            <th id="thR39" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton57" runat="server"></asp:LinkButton></th>
                            <th id="thR40" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton58" runat="server"></asp:LinkButton></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr4" class="pager" runat="server">
                <td id="Td4" class="left" runat="server">
                    <uc1:LetterDataPager ID="LetterDataPager1" Letter="NazwiskoLetter" runat="server" />
                </td>
                <td class="right">
                    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
            <tr class="pager">
                <td>
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

    </ContentTemplate>
</asp:UpdatePanel>


<%--
Uwaga!
select musi być bezpośrednio za " - dodawane są sortowania po prawach
--%>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select
    P.Id, 
    P.Login,
    P.Imie,
    P.Nazwisko,
    LEFT(LTRIM(P.Nazwisko), 1) as NazwiskoLetter,
    RTRIM(P.Nazwisko) + ' ' + RTRIM(P.Imie) as NazwiskoImie, 
    P.Email,
    P.Mailing, 
    P.KadryId, P.KadryId2,
    
    PK.RcpId,
    convert(varchar, PK.RcpId) as RcpIdTxt,
    PK.NrKarty,
    PK.Od KartaOd,
    PK.Do KartaDo,
    
    PS.Od as StanowiskoOd,
    PS.Do as StanowiskoDo,
    P.IdDzialu, D.Nazwa as Dzial,
    P.IdStanowiska, T.Nazwa as Stanowisko,
    PS.Grupa,
    PS.Klasyfikacja,
    --PL.Class as Klasyfikacja,
    --PL.TypImport as ClassGrp as Grupa,

    R.Id as IdPrzypisania,
    R.IdKierownika,
    K.Nazwisko as KierNazwisko, K.Imie as KierImie,
    case when R.IdKierownika = 0 then ' Główny poziom struktury'
    else RTRIM(K.Nazwisko) + ' ' + RTRIM(K.Imie) end as KierownikNI, 
    P.Kierownik,
    ISNULL(P.Admin, 0) as Admin,
    P.Raporty,
    P.Status,
    
    R.RcpStrefaId,   
    S.Id as StrefaId, 
    S.Nazwa as Strefa,
    R.Od as PrzypisanieOd,
    --R.Do as PrzypisanieDo,
    ISNULL(R.Do, R.DoMonit) as PrzypisanieDo,
    
    PP.RcpAlgorytm,
    KO.Nazwa as Algorytm,
    PP.Od as AlgorytmOd,
    PP.Do as AlgorytmDo,
    
    PP.WymiarCzasu, dbo.ToTimeHMM(PP.WymiarCzasu) as WymiarHMM,
    PP.PrzerwaWliczona, dbo.ToTimeHMM(PP.PrzerwaWliczona) as PrzerwaWliczonaHMM,
    PP.PrzerwaNiewliczona, dbo.ToTimeHMM(PP.PrzerwaNiewliczona) as PrzerwaNiewliczonaHMM, 
    
    --P.CCInfo,P.IdLinii,L.Symbol as LiniaSymbol,L.Nazwa as LiniaNazwa,
    C.Commodity, A.Area, PO.Position,
    R.IdCommodity, R.IdArea, R.IdPosition,
    
    P.DataZatr, P.DataZwol, P.DataZwol as prevDataZwol, 
    case when ISNULL(PK.Do, '20990909') &gt;= @NaDzien then 1 else 0 end as RcpIdOK,
    case when ISNULL(PP.Do, '20990909') &gt;= @NaDzien then 1 else 0 end as ParametryOK,
    case when ISNULL(R.Do, '20990909') &gt;= @NaDzien then 1 else 0 end as PrzypisanieOK,
    P.Rights
FROM Pracownicy P 

outer apply (select top 1 * from PracownicyKarty where IdPracownika = P.Id and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as PK
outer apply (select top 1 * from PracownicyParametry where IdPracownika = P.Id and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as PP
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as PS
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Status = 1 and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as R

left outer join Pracownicy K on K.Id = R.IdKierownika

left outer join Stanowiska T on T.Id = PS.IdStanowiska
left outer join Dzialy D on D.Id = PS.IdDzialu

left outer join Commodity C on C.id = R.IdCommodity
left outer join Area A on A.Id = R.IdArea
left outer join Position PO on PO.Id = R.IdPosition
left outer join Strefy S on S.Id = R.RcpStrefaId
left outer join Kody KO on KO.Typ = 'ALG' and KO.Kod = PP.RcpAlgorytm

--outer apply (select top 1 * from PodzialLudziImport where KadryId = P.KadryId order by Miesiac desc) as PL

order by P.Nazwisko, P.Imie
                        "
    UpdateCommand="
UPDATE [Pracownicy] SET [Login] = @Login, [Imie] = @Imie, [Nazwisko] = @Nazwisko, [Opis] = @Opis, [Email] = @Email, [Mailing] = @Mailing,  
                        [Admin] = @Admin, [Raporty] = @Raporty, [Kierownik] = @Kierownik, [KadryId] = @KadryId, [KadryId2] = @KadryId2, [Status] = @Status, [Rights] = @Rights, DataZatr = @DataZatr, DataZwol = @DataZwol 
                        WHERE [Id] = @Id
                        
--insert into Log (ParentId,DataCzas,Typ,Kod,Info,Status) values (0,GETDATE(),9999,@IdPrzypisania,'1',0)

if @IdPrzypisania is not null begin   
    if @IdKierownika is null      --jak nie jest już przełożonym
        update Przypisania set RcpStrefaId = @RcpStrefaId, IdCommodity = @IdCommodity, IdArea = @IdArea, IdPosition = @IdPosition                          
                            where Id = @IdPrzypisania
    else                                             
        update Przypisania set IdKierownika = @IdKierownika, RcpStrefaId = @RcpStrefaId, IdCommodity = @IdCommodity, IdArea = @IdArea, IdPosition = @IdPosition                          
                            where Id = @IdPrzypisania
    declare @od datetime
    declare @do datetime
    select @od = Od, @do = Do from Przypisania where Id = @IdPrzypisania

    --insert into Log (ParentId,DataCzas,Typ,Kod,Info,Status) values (0,GETDATE(),9999,@IdPrzypisania,'2',0)

    if ISNULL(@DataZwol, '20990909') &lt;&gt; ISNULL(@do, '20990909') and @od &lt;= ISNULL(@DataZwol, '20990909') begin
        update Przypisania set Do = @DataZwol where Id = @IdPrzypisania
    end
end
else if @IdKierownika is not null begin
    insert into Przypisania (IdPracownika, Od, IdKierownika, IdCommodity, IdArea, IdPosition, Status, Typ, RcpStrefaId, IdKierownikaRq, IdKierownikaAcc) values 
                            (@Id, @DataZatr, @IdKierownika, @IdCommodity, @IdArea, @IdPosition, 1, 1, @RcpStrefaId, @AutorId, @AutorId)
    set @IdPrzypisania = (select @@Identity)
end

declare @dZ datetime
declare @dZP datetime
set @dZ = ISNULL(@DataZwol, '20990909')
set @dZP = ISNULL(@prevDataZwol, '20990909')

--insert into Log (ParentId,DataCzas,Typ,Kod,Info,Status) values (0,GETDATE(),9999,@IdPrzypisania,@Nazwisko + ' ' + convert(varchar(10),@dZ,20) + ' - ' + convert(varchar(10),@dZP,20),0)

if @dZ != @dZP begin
    update PracownicyKarty      set Do = @DataZwol where IdPracownika = @Id and ISNULL(Do, '20990909') = @dZP and Od &lt;= @dZ
    --update PracownicyStanowiska set Do = @DataZwol where IdPracownika = @Id and ISNULL(Do, '20990909') = @dZP and Od &lt;= @dZ
    --update PracownicyParametry  set Do = @DataZwol where IdPracownika = @Id and ISNULL(Do, '20990909') = @dZP and Od &lt;= @dZ
end
                  "
    InsertCommand="
INSERT INTO [Pracownicy] ([Login], [Imie], [Nazwisko], [Opis], [Email], [Mailing], [IdDzialu], [IdStanowiska], [IdKierownika], [Admin], [Raporty], [Kierownik], [KadryId], KadryId2, [RcpId], [Status], [RcpStrefaId], [RcpAlgorytm], [CCInfo], [Rights], [IdLinii], [GrSplitu], DataZatr, DataZwol, EtatL, EtatM, Stawka) VALUES 
                         (@Login, @Imie, @Nazwisko, @Opis, @Email, @Mailing, @IdDzialu, @IdStanowiska, @IdKierownika, @Admin, @Raporty, @Kierownik, @KadryId, @KadryId2, @RcpId, @Status, @RcpStrefaId, @RcpAlgorytm, @CCInfo, @Rights, @IdLinii, @GrSplitu, @DataZatr, @DataZwol, 1, 1, 0)
set @IdPracownika = (select @@Identity)

if @IdKierownika is not null begin
    insert into Przypisania (IdPracownika, Od, IdKierownika, IdCommodity, IdArea, IdPosition, Status, Typ, RcpStrefaId, IdKierownikaRq, IdKierownikaAcc) values 
                            (@IdPracownika, @DataZatr, @IdKierownika, @IdCommodity, @IdArea, @IdPosition, 1, 1, @RcpStrefaId, @AutorId, @AutorId)
    set @IdPrzypisania = (select @@Identity)
end

if @RcpId is not null or @NrKarty is not null begin
    if @NrKarty is not null and @RcpId is null begin
        set @RcpId = @IdPracownika
        update Pracownicy set RcpId = @RcpId where Id = @IdPracownika
    end    
    insert into PracownicyKarty (IdPracownika, Od, Do, RcpId, NrKarty) values (@IdPracownika, @DataZatr, null, @RcpId, @NrKarty)
end

if @RcpAlgorytm is not null or @WymiarCzasu is not null or @PrzerwaWliczona is not null or @PrzerwaNiewliczona is not null
    insert into PracownicyParametry (IdPracownika, Od, Do, RcpAlgorytm, WymiarCzasu, PrzerwaWliczona, PrzerwaNiewliczona) values (@IdPracownika, @DataZatr, null, @RcpAlgorytm, @WymiarCzasu, @PrzerwaWliczona, @PrzerwaNiewliczona)

if @IdStanowiska is not null or @Grupa is not null or @Klasyfikacja is not null begin
    declare @dzid int
    select @dzid = IdDzialu from Stanowiska where Id = @IdStanowiska
    insert into PracownicyStanowiska values (@IdPracownika, @DataZatr, null, @dzid, @IdStanowiska, @Grupa, @Klasyfikacja)                         
end
                  " 
    DeleteCommand="
DELETE FROM [Pracownicy] WHERE [Id] = @Id
delete from Przypisania where IdPracownika = @Id
delete from PracownicyKarty where IdPracownika = @Id
delete from PracownicyParametry where IdPracownika = @Id
delete from PracownicyStanowiska where IdPracownika = @Id
               " 
    onselected="SqlDataSource1_Selected" oninserted="SqlDataSource1_Inserted">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidNaDzien" Name="NaDzien" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Login" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Mailing" Type="Boolean" />
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="IdStanowiska" Type="Int32" />
        <asp:Parameter Name="Admin" Type="Boolean" />
        <asp:Parameter Name="Raporty" Type="Boolean" />
        <asp:Parameter Name="Kierownik" Type="Boolean" />
        <asp:Parameter Name="KadryId" Type="String" />
        <asp:Parameter Name="RcpId" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />

        <asp:Parameter Name="RcpAlgorytm" Type="Int32" />
        <asp:Parameter Name="WymiarCzasu" Type="Int32" />
        <asp:Parameter Name="PrzerwaWliczona" Type="Int32" />
        <asp:Parameter Name="PrzerwaNiewliczona" Type="Int32" />

        <asp:Parameter Name="CCInfo" Type="String" />
        <asp:Parameter Name="Rights" Type="String" />
        <asp:Parameter Name="IdLinii" Type="Int32" />
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="DataZatr" Type="DateTime" />
        <asp:Parameter Name="DataZwol" Type="DateTime" />
        <asp:Parameter Name="Id" Type="Int32" />
        
        <asp:Parameter Name="IdPrzypisania" Type="Int32" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />

        <asp:Parameter Name="AutorId" Type="String" />
        <asp:Parameter Name="prevDataZwol" Type="DateTime" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" Direction="Output" DefaultValue="0"/>
        <asp:Parameter Name="IdPrzypisania" Type="Int32" Direction="Output" DefaultValue="0"/>
        <asp:Parameter Name="AutorId" Type="String" />
        
        <asp:Parameter Name="Login" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Mailing" Type="Boolean" />
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="IdStanowiska" Type="Int32" />
        <asp:Parameter Name="Admin" Type="Boolean" />
        <asp:Parameter Name="Raporty" Type="Boolean" />
        <asp:Parameter Name="Kierownik" Type="Boolean" />
        <asp:Parameter Name="KadryId" Type="String" />
        <asp:Parameter Name="RcpId" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="CCInfo" Type="String" />
        <asp:Parameter Name="Rights" Type="String" />
        <asp:Parameter Name="IdLinii" Type="Int32" />
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="DataZatr" Type="DateTime" />
        <asp:Parameter Name="DataZwol" Type="DateTime" />

        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />

        <asp:Parameter Name="RcpAlgorytm" Type="Int32" />
        <asp:Parameter Name="WymiarCzasu" Type="Int32" />
        <asp:Parameter Name="PrzerwaWliczona" Type="Int32" />
        <asp:Parameter Name="PrzerwaNiewliczona" Type="Int32" />
        
        <asp:Parameter Name="NrKarty" Type="String" />
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="Klasyfikacja" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>





<asp:SqlDataSource ID="SqlDataSourceKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Sort, Id, Nazwisko + ' ' + Imie as PrzKierownik FROM Pracownicy where Kierownik = 1 and Status != -1
                    union
                    select 0 as Sort, null as Id, 'wybierz ...' 
                    union 
                    select 2 as Sort, '0' as Id, 'Główny poziom struktury' 
                    ORDER BY Sort, PrzKierownik">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceKierEdit" runat="server" CancelSelectOnNullParameter="False"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Sort, Id, Nazwisko + ' ' + Imie as PrzKierownik FROM Pracownicy where Kierownik = 1 and Status != -1
                    union
                   SELECT 1 as Sort, Id, '* ' + Nazwisko + ' ' + Imie as PrzKierownik FROM Pracownicy where Id = @selId and (Kierownik = 0 or Status = -1)
                    union 
                    select 0 as Sort, null as Id, 'wybierz ...' 
                    union 
                    select 2 as Sort, '0' as Id, 'Główny poziom struktury' 
                    ORDER BY Sort, PrzKierownik" 
    ondatabinding="SqlDataSourceKierEdit_DataBinding" 
    onselected="SqlDataSourceKierEdit_Selected" >
    <SelectParameters>
        <asp:Parameter Name="selId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>




<asp:SqlDataSource ID="SqlDataSourceComm" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as PrzCommodity, 0 as Sort
union    
select Id, Commodity as PrzCommodity, 1 as Sort from Commodity where Aktywne = 1 order by Sort, PrzCommodity
    " >
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceCommEdit" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null as Id, 'wybierz ...' as PrzCommodity, 0 as Sort
union    
select Id, Commodity as PrzCommodity, 1 as Sort from Commodity where Aktywne = 1 
union
select Id, '* ' + Commodity as PrzCommodity, 1 as Sort from Commodity where Aktywne = 0 and Id = @selId
order by Sort, PrzCommodity
    ">
    <SelectParameters>
        <asp:Parameter Name="selId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>




<asp:SqlDataSource ID="SqlDataSourceArea" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as PrzArea, 0 as Sort
union    
select Id, Area as PrzArea, 1 as Sort from Area where Aktywne = 1 order by Sort, PrzArea
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceAreaEdit" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" 
    SelectCommand="
select null as Id, 'wybierz ...' as PrzArea, 0 as Sort
union    
select Id, Area as PrzArea, 1 as Sort from Area where Aktywne = 1 
union    
select Id, '* ' + Area as PrzArea, 1 as Sort from Area where Aktywne = 0 and Id = @selId 
order by Sort, PrzArea
    ">
    <SelectParameters>
        <asp:Parameter Name="selId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>





<asp:SqlDataSource ID="SqlDataSourcePos" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as PrzPosition, 0 as Sort
union    
select Id, Position as PrzPosition, 1 as Sort from Position where Aktywne = 1 order by Sort, PrzPosition
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourcePosEdit" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null as Id, 'wybierz ...' as PrzPosition, 0 as Sort
union    
select Id, Position as PrzPosition, 1 as Sort from Position where Aktywne = 1 
union    
select Id, '* ' + Position as PrzPosition, 1 as Sort from Position where Aktywne = 0 and Id = @selId 
order by Sort, PrzPosition
    ">
    <SelectParameters>
        <asp:Parameter Name="selId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>




<asp:SqlDataSource ID="SqlDataSourceStrefa" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as RcpStrefaId, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Nazwa, 1 as Sort from Strefy where Aktywna = 1 
order by Sort, Nazwa">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceStrefaEdit" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null as RcpStrefaId, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Nazwa, 1 as Sort from Strefy where Aktywna = 1 
union    
select Id, '* ' + Nazwa, 1 as Sort from Strefy where Aktywna = 0 and Id = @selId 
order by Sort, Nazwa
    ">
    <SelectParameters>
        <asp:Parameter Name="selId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>



<asp:SqlDataSource ID="SqlDataSourceAlg" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select 0 as Sort, null as Id, 'wybierz ...' as Nazwa, 0 as Lp, null as RcpAlgorytm, null as Parametr                   
union
SELECT 1 as Sort, [Id], [Nazwa], [Lp], Kod as RcpAlgorytm, [Parametr] FROM [Kody] WHERE ([Typ] = 'ALG') 
ORDER BY Sort, [Lp]">
</asp:SqlDataSource>




<asp:SqlDataSource ID="SqlDataSourceStan" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, 1 as Aktywne, null as IdStanowiska, 'wybierz ...' as Stanowisko 
                    union
                    SELECT 1 as Sort, 
                        case when D.Status &gt;= 0 and S.Aktywne = 1 then 1 else 0 end as Aktywne,
                        S.Id as IdStanowiska,
                        --convert(varchar, S.Id) + '|' + convert(varchar, D.Id) as IdStanowiska, 
                        ISNULL(D.Nazwa + ' - ', '') + S.Nazwa + case when D.Status &gt;= 0 and S.Aktywne = 1 then '' else ' (nieaktualne)' end as Stanowisko 
                    FROM Stanowiska S 
                    left outer join Dzialy D on D.Id = S.IdDzialu
                    ORDER BY Sort, Stanowisko">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceGrupa" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Grupa, 'wybierz ...' as SymGrupa
                    union
                    select distinct 1 as Sort, TypImport as Grupa, TypImport as SymGrupa from PodzialLudziImport 
                    union
                    select 1 as Sort, Nazwa as Grupa, Nazwa as SymGrupa from Kody where Typ = 'PRACGRUPA' 
                    order by Sort, SymGrupa">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceClass" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Klasyfikacja, 'wybierz ...' as SymKlasyfikacja
                    union                   
                    select distinct 1 as Sort, Class as Klasyfikacja, Class as SymKlasyfikacja from PodzialLudziImport 
                    union 
                    select 1 as Sort, Nazwa as Klasyfikacja, Nazwa as SymKlasyfikacja from Kody where Typ = 'PRACKLAS' 
                    order by Sort, SymKlasyfikacja">
</asp:SqlDataSource>

<uc2:cntKartyRcpPopup ID="cntKartyRcpPopup" runat="server" OnChanged="cntKartyRcpPopup_Changed"/>
<uc2:cntAlgorytmyPopup ID="cntAlgorytmyPopup" runat="server" OnChanged="cntAlgorytmyPopup_Changed" />
<uc2:cntStanowiskaPopup ID="cntStanowiskaPopup" runat="server" OnChanged="cntStanowiskaPopup_Changed"/>


























<%--
<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, [Id], [Nazwa] FROM [Strefy] WHERE ([Aktywna] = 1) 
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as Nazwa                   
                    ORDER BY Aa, [Nazwa]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, [Id], [Nazwa], [Lp], [Kod], [Parametr] FROM [Kody] WHERE ([Typ] = 'ALG') 
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as Nazwa, 0 as Lp, null as Kod, null as Parametr                   
                    ORDER BY Aa, [Lp]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, [Id], [Nazwa] FROM [Dzialy] 
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as Nazwa                   
                    ORDER BY Aa, [Nazwa]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, S.Id, D.Nazwa + ' - ' + S.Nazwa as NazwaDS FROM Stanowiska S 
                    left outer join Dzialy D on D.Id = S.IdDzialu
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as NazwaDS
                    ORDER BY Aa, NazwaDS">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Sort, Id, Nazwisko + ' ' + Imie as NazwaK FROM Pracownicy where Kierownik = 1 and Status <> -1
                    union 
                    select 0 as Sort, null as Id, 'wybierz ...' as NazwaK
                    union 
                    select 2 as Sort, '0' as Id, 'Główny poziom struktury' as NazwaK
                    ORDER BY Sort, NazwaK">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource7" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Id, 'wybierz ...' as Nazwa union
                   SELECT 1 as Sort, GrSplitu as Id, Nazwa FROM Splity where ISNULL(DataDo, GETDATE()) &gt;= GETDATE() 
                   ORDER BY Sort, Nazwa">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource8" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Id, 'wybierz ...' as Nazwa union
                   SELECT 1 as Sort, Id, Symbol + ISNULL(' - ' + Nazwa, '') as Nazwa FROM Linie
                   ORDER BY Sort, Nazwa">
</asp:SqlDataSource>

--%>











<%--


/*
left outer join PracownicyKarty PK on PK.IdPracownika = P.Id and @NaDzien between PK.Od and ISNULL(PK.Do, '20990909')
left outer join PracownicyParametry PP on PP.IdPracownika = P.Id and @NaDzien between PP.Od and ISNULL(PP.Do, '20990909')
left outer join Przypisania R on R.IdPracownika = P.Id and @NaDzien between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
outer apply (select top 1 * from PracownicyKarty where IdPracownika = P.Id and Od &lt;= @NaDzien order by Od desc) as PKL
outer apply (select top 1 * from PracownicyParametry where IdPracownika = P.Id and Od &lt;= @NaDzien order by Od desc) as PPL
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @NaDzien and Status = 1 order by Od desc) as RL
*/

    SelectCommand="select
                        P.Id, 
                        P.Login,
                        P.Imie,
                        P.Nazwisko,
                        RTRIM(P.Nazwisko) + ' ' + RTRIM(P.Imie) as NazwiskoImie, 
                        P.Email,
                        P.Mailing, 
                        P.KadryId,
                        P.RcpId,
                        P.IdDzialu,
                        D.Nazwa as Dzial,
                        P.IdStanowiska,
                        T.Nazwa as Stanowisko,
                        P.IdKierownika,
                        RTRIM(K.Nazwisko) + ' ' + RTRIM(K.Imie) as KierownikNI, 
                        P.Kierownik,
                        ISNULL(P.Admin, 0) as Admin,
                        P.Raporty,
                        P.Status,
                        P.RcpStrefaId,   
                        S.Id as StrefaId, 
                        S.Nazwa as Strefa,
                        P.RcpAlgorytm,
                        KO.Nazwa as Algorytm,
                        P.CCInfo,P.IdLinii,P.GrSplitu,L.Symbol as LiniaSymbol,L.Nazwa as LiniaNazwa,
                        (select top 1 SS.Nazwa from Splity SS where SS.GrSplitu = 
                            case when P.GrSplitu is null then 
                                case when P.Kierownik=1 then L.GrSplituK
                                else L.GrSplituP end
                            else P.GrSplitu end) as Split,
                        null as Commodity, null as Area, null as Position,
                        P.DataZatr, P.DataZwol, 
                        P.Rights
                    FROM Pracownicy P 
                    left outer join Pracownicy K on P.IdKierownika = K.Id
                    left outer join Dzialy D on P.IdDzialu = D.Id
                    left outer join Stanowiska T on T.Id = P.IdStanowiska
                    left outer join Strefy S on S.Id = 
                        ISNULL(P.RcpStrefaId, CASE WHEN P.Kierownik=1 THEN D.KierStrefaId ELSE D.PracStrefaId END)
                    left outer join Kody KO on KO.Typ = 'ALG' and KO.Kod = 
                        ISNULL(P.RcpAlgorytm, CASE WHEN P.Kierownik=1 THEN D.KierAlgorytm ELSE D.PracAlgorytm END)
                    left outer join Linie L on L.Id = P.IdLinii"
--%>





            <%--
            <td colspan="<%= GetEditColSpan() %>" class="col1">
                <table class="edit">
                    <tr>
                        <td class="coled1">
                            <span class="label">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                            <span class="label">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" Text='<%# Bind("Imie") %>' /><br />

                            <span class="label">Kadry Id:</span>
                            <asp:TextBox ID="KadryIdTextBox" runat="server" Text='<%# Bind("KadryId") %>' /><br />
                            <span class="label">RCP Id:</span>
                            <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Bind("RcpId") %>' /><br />
                            <span class="label">Login:</span>
                            <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Login") %>' /><br />
                            <span class="label">E-mail:</span>
                            <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' /><br />
                            <span class="label">Mailing:</span>
                            <asp:CheckBox ID="cbMailing" runat="server" class="check" Checked='<%# Bind("Mailing") %>' /><br />

                            <span class="label">Kierownik:</span>
                            <asp:CheckBox ID="KierownikCheckBox" runat="server" class="check" Checked='<%# Bind("Kierownik") %>' /><br />
                            <span class="label">Admininistrator:</span>
                            <asp:CheckBox ID="AdminCheckBox" runat="server" class="check" Checked='<%# Bind("Admin") %>' /><br />
                            <span class="label">Raporty:</span>
                            <asp:CheckBox ID="RaportyCheckBox" runat="server" class="check" Checked='<%# Bind("Raporty") %>' /><br />
                            <span class="label">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" ></asp:DropDownList>
                        </td>
                        <td class="coled2">
                            <span class="label">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownik" runat="server"
                                DataSourceID="SqlDataSource6"
                                DataTextField="NazwaK"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Commodity:</span>
                            <asp:DropDownList ID="ddlLinia" runat="server" 
                                DataSourceID="SqlDataSource8"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Dział:</span>
                            <asp:DropDownList ID="ddlDzial" runat="server" 
                                DataSourceID="SqlDataSource4"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server"
                                DataSourceID="SqlDataSource5"
                                DataTextField="NazwaDS"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            <br />
                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server" 
                                DataSourceID="SqlDataSource2"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            <span class="label">Algorytm RCP:</span>
                            <asp:DropDownList ID="ddlAlgorytm" runat="server" 
                                DataSourceID="SqlDataSource3"
                                DataTextField="Nazwa"
                                DataValueField="Kod">
                            </asp:DropDownList><br />
                            
                            <span class="label">Split:</span>
                            <asp:DropDownList ID="ddlSplit" runat="server" 
                                DataSourceID="SqlDataSource7"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                        </td>
                        <td class="col3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" /><br />
                            </div>
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                        </td>                                                        
                    </tr>
                </table>
            </td>
            --%>