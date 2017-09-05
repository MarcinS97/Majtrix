<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntUprawnieniaScroll.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Uprawnienia.cntUprawnieniaScroll" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>
<%@ Register src="~/Controls/Portal/cntSqlTabs.ascx" tagname="cntSqlTabs" tagprefix="uc3" %>

<%@ Register src="~/SzkoleniaBHP/Controls/LiniaSelect2.ascx" tagname="LiniaSelect2" tagprefix="uc4" %>
<%@ Register src="~/SzkoleniaBHP/Controls/PracownikDetails2.ascx" tagname="PracownikDetails" tagprefix="uc5" %>

<%@ Register src="cntCertyfikat3.ascx" tagname="cntCertyfikat9" tagprefix="uc6" %>
<%@ Register src="cntCertyfikatAdd.ascx" tagname="cntCertyfikatAdd" tagprefix="uc6" %>

<%@ Register src="cntPracownikUprawnienia.ascx" tagname="cntPracownikUprawnienia" tagprefix="uc6" %>
<%@ Register src="cntUprPracownicyTotal.ascx" tagname="cntUprPracownicyTotal" tagprefix="uc6" %>
<%@ Register src="cntUprawnieniePracownicy.ascx" tagname="cntUprawnieniePracownicy" tagprefix="uc6" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Uprawnienia/cntOceny.ascx" TagPrefix="cc" TagName="cntOceny" %>

<%--
<%@ Register src="cntCertyfikatySpaw4.ascx" tagname="3Spaw" tagprefix="uc6" %>
<%@ Register src="cntUprawnienieCzynnosci.ascx" tagname="cntUprCzynnosciHeader" tagprefix="uc6" %>
--%>

<%--
Uwaga !!!
żeby działały buttony z jQuery modal trzeba im ustawić UseSubmitBehavior na false !!!
albo położyć UpdatePanel na formatce !!! a nie w kontrolce
--%>

<%--
<script type="text/javascript">
    function <%=ClientID%>_clearFilter() {
        var s = $('#<%=tbSearch.ClientID%>');
        var postback = s.val() == "";
        s.val('');
        $('#<%=ddlKier.ClientID%> option')[0].selected = true;
        $('#<%=ddlStatus.ClientID%> option')[0].selected = true;
        $('#<%=ddlCC.ClientID%> option')[0].selected = true;
        $('#<%=cbShowSub.ClientID%>').prop('checked',false);
        $('#<%=cbWithOnly.ClientID%>').prop('checked',false);
        if (postback) doClick('<%=btSearch.ClientID%>');
        return false;
    }
</script>
--%>


<%--
    <asp:UpdatePanel ID="paFilter" runat="server" UpdateMode="Conditional"> 
        <ContentTemplate>
--%>
    <div class="caption4 caption">
        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/captions/layout_edit.png" />
        <asp:Label ID="lbTitle" CssClass="title" runat="server" Text=""></asp:Label>
             
        <div class="right">
            <asp:LinkButton ID="btAdmin" CssClass="button100 btn btn-sm btn-default" Text="Administracja" runat="server" PostBackUrl="~/MatrycaSzkolen/SzkoleniaAdmin.aspx" Visible="false" />
            <asp:LinkButton ID="btAdd" Text="Dodaj szkolenie" CssClass="btAdd button150" runat="server" OnClick="btAdd_Click" Visible="false" />
        </div>
        
    </div>
    <div class="filters">
        <table>
            <%-- na dzień --%>
            <tr>
                <td class="col1">
                    <asp:Label ID="lbNaDzien" CssClass="label" runat="server" Text="Na dzień:"></asp:Label>
                </td>
                <td class="col2a">
                    <uc2:DateEdit ID="deNaDzien" runat="server" AutoPostBack="true" OnDateChanged="deNaDzien_DateChanged" />
                </td>
                <td class="col2b">
                </td>
                <td class="col3">
                    <asp:Label ID="lbMonit" CssClass="label" runat="server" Text="Ostrzeganie [dni]:"></asp:Label>
                    <asp:TextBox ID="tbMonit" CssClass="textbox form-control" runat="server" Width="30" MaxLength="3"
                        AutoPostBack="true" OnTextChanged="tbMonit_TextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbMonit"
                        FilterType="Custom" ValidChars="0123456789" />
                </td>
            </tr>
            <%-- kierownik --%>
            <tr>
                <td class="col1">
                    <asp:Label ID="Label12" CssClass="label" runat="server" Text="Kierownik:"></asp:Label>
                </td>
                <td colspan="2" class="col2">
                    <asp:DropDownList ID="ddlKier" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceKier" CssClass="form-control input-xsm"
                        DataTextField="Value" DataValueField="Id" OnSelectedIndexChanged="ddlKier_SelectedIndexChanged"
                        OnDataBound="ddlKier_DataBound">
                    </asp:DropDownList>
                </td>
                <td class="col3">
                    <asp:CheckBox ID="cbShowSub" runat="server" CssClass="check" Text="Pokaż pracowników z podstruktury"
                        AutoPostBack="True" OnCheckedChanged="cbShowSub_CheckedChanged" />
                </td>
            </tr>
            <%-- pracownik --%>
            <tr>
                <td class="col1">
                    <asp:Label ID="Label13" CssClass="label" runat="server" Text="Pracownicy:"></asp:Label>
                </td>
                <td colspan="2" class="col2">
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceStatus" CssClass="form-control input-xsm"
                        DataTextField="Value" DataValueField="Id" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="col3">
                    <asp:CheckBox ID="cbWithOnly" runat="server" CssClass="checkbox-inline" Text="Pokaż tylko posiadających uprawnienia" AutoPostBack="true" OnCheckedChanged="cbWithOnly_CheckedChanged" />
                    <asp:CheckBox ID="cbPassSpaw" runat="server" CssClass="checkbox-inline" Text="Pokaż tylko spawaczy" AutoPostBack="true" Visible="false" OnCheckedChanged="cbPassSpaw_CheckedChanged" />
                </td>
            </tr>
            <%-- firma --%>
            <tr>
                <td class="col1">
                    <asp:Label ID="Label14" CssClass="label" runat="server" Text="Firmy:"></asp:Label>
                </td>
                <td colspan="2" class="col2">
                    <asp:DropDownList ID="ddlFirma" runat="server" AutoPostBack="True" DataSourceID="dsFirma" CssClass="form-control input-xsm"
                        DataTextField="Name" DataValueField="Id" >
                    </asp:DropDownList>
                </td>
            </tr>
            <%-- cc --%>
            <tr id="trCC" runat="server" visible="false">
                <td class="col1">
                    <asp:Label ID="Label15" CssClass="label" runat="server" Text="CC:"></asp:Label>
                </td>
                <td colspan="2" class="col2">
                    <asp:DropDownList ID="ddlCC" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceCC" CssClass="form-control input-xsm"
                        DataTextField="Value" DataValueField="Id" OnSelectedIndexChanged="ddlCC_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="col3">
                </td>
            </tr>
            <%-- status szkoleń --%>
            <tr>
                <td class="col1">
                    <asp:Label ID="Label2" CssClass="label" runat="server" Text="Status:"></asp:Label>
                </td>
                <td colspan="2" class="col2">
                    <asp:DropDownList ID="ddlStatusSzk" runat="server" AutoPostBack="True" DataSourceID="dsStatusSzk" CssClass="form-control input-xsm"
                        DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlStatusSzk_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="col3">
                </td>
            </tr>
            <%-- wyszukaj --%>
            <tr>
                <td class="col1">
                    <asp:Label ID="lbSearch" CssClass="label search" runat="server" Text="Wyszukaj:"></asp:Label>
                </td>
                <td colspan="2" class="col2">
                    <asp:TextBox ID="tbSearch" CssClass="search textbox form-control" runat="server"></asp:TextBox>
                </td>
                <td class="col3">
                    <asp:Button ID="btClear" runat="server" CssClass="btn btn-sm btn-default" Text="Czyść" OnClick="btClear_Click" />
                    <asp:Button ID="btShow" runat="server" CssClass="btn btn-sm btn-default" Text="Pokaż" OnClick="btShow_Click" />
         <%--           <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Button ID="btExcelF" runat="server" CssClass="button75" Text="Excel" OnClick="btExcelF_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btExcelF" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
    </div>
<%-- panel wyszukiwania --%>
            <table class="xcaption" style="xdisplay: none;">
                <tr>
<%--                    <td class="left" style="width: 500px;" >
                        <span class="caption4">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/captions/layout_edit.png"/>
                            <asp:Label ID="lbTitle2" CssClass="title" runat="server" Text=""></asp:Label>                                                    
                        </span>
                    </td>--%>
                    <td class="left" rowspan="2">
                    

                        <asp:HiddenField ID="hidKierId" runat="server" Visible="false"/>
                        <asp:SqlDataSource ID="SqlDataSourceKier" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                            SelectCommand="
select -99 as Id, 'Wszyscy pracownicy' as Value, 1 as Sort from (select 1 x) x where @kierId = -99
union all            
select 0 as Id, 'Poziom główny struktury' as Value, 2 as Sort from (select 1 x) x where @kierId = -99
union all            
--select convert(varchar,Id) as Id, 
--select Id,
select IdPracownika as Id,
case when Status = -1 then '*' else '' end +
Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')','') as Value,
case 
when Status = -1 then 5 
when IdPracownika = @kierId then 3
else 4 end as Sort
--from Pracownicy
from dbo.fn_GetTree2(case when @kierId = -99 then 0 else @kierId end, 1, GETDATE()) 
where Kierownik = 1 and Status != -2
order by Sort, Value
                            ">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hidKierId" Name="kierId" PropertyName="Value" Type="Int32"/>
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceStatus" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                            SelectCommand="
select 'pa' as Id, 'Zatrudnieni - Aktywni' as Value, 10 as Sort
union all            
select 'p' as Id, 'Zatrudnieni' as Value, 20 as Sort
union all            
select 'k' as Id, 'Przełożeni' as Value, 30 as Sort
union all            
select 'n' as Id, 'Nowi / Nieprzypisani' as Value, 40 as Sort
union all            
select 'z' as Id, 'Zwolnieni' as Value, 50 as Sort
union all            
select 'a' as Id, 'APT' as Value, 60 as Sort
union all
select 'o' as Id, 'Osadzeni' as Value, 70 as Sort
union all
select 'all' as Id, 'Wszyscy' as Value, 80 as Sort
                            ">
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceCC" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                            SelectCommand="
select -99 as Id, 'Wszystkie CC' as Value, null as IsOld, null as cc
union all
select CC.Id as Id, IsOld + CC.cc + ' - ' + CC.Nazwa as Value, A.IsOld, CC.cc
from CC 
outer apply (select case when @data between AktywneOd and ISNULL(AktywneDo,'20990909') then '' else '*' end as IsOld) A
order by IsOld, cc
                            ">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hidData" Name="data" PropertyName="Value" Type="DateTime"/>
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="dsStatusSzk" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                            SelectCommand="
select -99 as Id, 'Wszyscy pracownicy' as Name
union all
select 1 as Id, 'Aktualne szkolenia' as Name
union all
select 2 as Id, 'Wygasające' as Name
union all
select 3 as Id, 'Nieaktualne' as Name
union all
select 4 as Id, 'Brak' as Name
                            " />

                        
                        <asp:SqlDataSource ID="dsFirma" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                            SelectCommand="select '-99' Id, 'Wszystkie firmy' Name, 0 Sort
union all
select Nazwa Id, Nazwa Name, 1 Sort
from Kody 
where Typ = 'PRACKLAS' and Aktywny = 1 
order by Sort, Name"
                            />
 

                    </td>
                    <td class="right" rowspan="2">
<%--
                        <asp:Button ID="btAdmin" CssClass="button100" Text="Administracja" runat="server" OnClick="btAdmin_Click" />
--%>
<%--                        <asp:Button ID="btImport" CssClass="button" Text="Import z Asseco" runat="server" onclick="btImport_Click" />
                        <asp:Button ID="btAbsDl" CssClass="button" Text="Absencje długotrwałe" runat="server" PostBackUrl="~/AbsencjeDlugotrwale.aspx" />--%>
                       
                       
                       
                       <%-- <asp:Button ID="btAdmin" CssClass="button100" Text="Administracja" runat="server" PostBackUrl="~/MatrycaSzkolen/SzkoleniaAdmin.aspx" />
                        <asp:Button ID="btAdd" Text="Dodaj szkolenie" CssClass="btAdd button150" runat="server" OnClick="btAdd_Click"/>
--%>
                    </td>
                </tr>
                <tr>
                    <td class="paButtontAdd">
                    
                        <%--<asp:Button ID="btAdd" Text="Dodaj szkolenie" CssClass="btAdd button150" runat="server" OnClick="btAdd_Click"/>--%>
                    </td>
                </tr>
            </table>
            <div id="paSzkoleniaBHP" runat="server" class="cntSzkoleniaBHP">
            <%-- szkolenia page --%>
            <div class="pageContent">
                <div class="padding">
                    <div class="secondary-buttons pull-right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                            
                                <a class="icon-black" href="#" onclick="$('.filters').slideToggle(800, resize);" title="Pokaż filtry" ><i class="fa fa-filter"></i></a>
                            
                                <asp:LinkButton ID="LinkButton13" runat="server" CssClass="icon-blue">
                                         <i class="fa fa-print"></i>
                                </asp:LinkButton>
                            
                                <asp:LinkButton ID="btExcelF" runat="server" CssClass="btn-excel"
                                    OnClick="btExcelF_Click">
                                <i class="fa fa-file-excel-o"></i>
                                </asp:LinkButton>
                                
                   <%--             <asp:LinkButton ID="LinkButton12" runat="server" CssClass="btn-pdf">
                                         <i class="fa fa-file-pdf-o"></i>
                                </asp:LinkButton>--%>
                
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btExcelF" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>

<asp:HiddenField ID="hidTyp" runat="server" Visible="false"/>
<asp:HiddenField ID="hidFilter" runat="server" Visible="false"/>
<asp:HiddenField ID="hidKwal" runat="server" Visible="false"/>
<asp:HiddenField ID="hidData" runat="server" Visible="false"/>
<asp:HiddenField ID="hidMonit" runat="server" Visible="false"/>

<asp:HiddenField ID="hidGrupa" runat="server" Visible="false" Value="-99" />
<asp:HiddenField ID="hidPracownicyTyp" runat="server" Visible="false" />


<asp:HiddenField ID="hidTwoColumn" runat="server" Visible="false" Value="" />


<%--
<asp:HiddenField ID="hidStrOrg" runat="server" Visible="false"/>
<asp:HiddenField ID="hidStatus" runat="server" Visible="false"/>
<asp:HiddenField ID="hidKierId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidShowStrKier" runat="server" Visible="false"/>
--%>

<asp:HiddenField ID="hidReport" runat="server" />
<asp:HiddenField ID="hidReportTitle" runat="server" />



<div class="tbUprawnienia_topmenu">
    <asp:Menu ID="tabFilter" runat="server" Orientation="Horizontal" 
        onmenuitemclick="tabFilter_MenuItemClick" Visible="false" >
        <StaticMenuStyle CssClass="tabsStrip mnFilter" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <Items>
            <asp:MenuItem Text="Wszyscy pracownicy" Value="-99" Selected="True"></asp:MenuItem>
            <asp:MenuItem Text="Aktualne uprawnienia" Value="1" ></asp:MenuItem>
            <asp:MenuItem Text="Wygasające" Value="2"></asp:MenuItem>
            <asp:MenuItem Text="Przeterminowane" Value="3"></asp:MenuItem>            
            
<%--            <asp:MenuItem Text="Brak uprawnień" Value="-1"></asp:MenuItem>            
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
    
    <uc3:cntSqlTabs ID="cntSqlKwalifikacje" runat="server" 
        AddCssClass="tabKwal"
        DataTextField="Nazwa" 
        DataValueField="Id" 
        OnSelectTab="cntSqlKwalifikacje_SelectTab"/>
        
</div>

    <uc3:cntSqlTabs ID="cntSqlGrupy" runat="server" 
        AddCssClass="tabKwal"
        DataTextField="Nazwa" 
        DataValueField="Id" 
        OnSelectTab="cntSqlGrupy_SelectTab"/>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />
        <div class="upr-wrapper" runat="server" id="uprWrapper">
<asp:ListView ID="lvUprawnienia" runat="server" DataSourceID="SqlDataSource1" 
    ondatabinding="lvUprawnienia_DataBinding" 
    ondatabound="lvUprawnienia_DataBound" 
    onitemcreated="lvUprawnienia_ItemCreated" 
    onitemdatabound="lvUprawnienia_ItemDataBound" 
    onlayoutcreated="lvUprawnienia_LayoutCreated" 
    onsorted="lvUprawnienia_Sorted" 
    onsorting="lvUprawnienia_Sorting" 
    onitemcommand="lvUprawnienia_ItemCommand" 
    oninit="lvUprawnienia_Init" 
    onprerender="lvUprawnienia_PreRender">
    <ItemTemplate>

        <tr class='it <%# Eval("Css") %>'>
            <td class="nazwisko">
                <asp:LinkButton ID="lbtNazwisko" runat="server" Visible='<%# Eval("IsKierownik") %>' Text='<%# Eval("Pracownik") %>' CommandName="zoomKier" CommandArgument='<%# Eval("IdPracownika") %>' ToolTip='<%# Eval("ToolTip") %>' ></asp:LinkButton>            
                <asp:Label ID="lbPracownik" runat="server" Visible='<%# !(bool)Eval("IsKierownik") %>' Text='<%# Eval("Pracownik") %>' />
                <asp:Label ID="lbApt" CssClass="apt" Text="APT" runat="server" Visible="false"/>
            </td>
            <td class="nrew" id="tdNrEw2" runat="server" visible="false">
                <asp:Label ID="lbNrErid2" Text='<%# Eval("Nr_Ewid2") %>' runat="server" />
            </td>

            <td class="nrew" id="td1" runat="server" visible='<%# GetStrKierVisible() %>'>
                <asp:Label ID="Label10" Text='<%# Eval("Nr_Ewid") %>' runat="server" />
            </td>

            <td class="date" id="td5" runat="server">
                <asp:Label ID="Label11" runat="server" Text='<%# Eval("DataZatr", "{0:d}") %>' />
            </td>
       <%--      <td class="date" id="td6" runat="server">
                <asp:Label ID="Label14" runat="server" Text='<%# Eval("DataZwol", "{0:d}") %>' />
            </td>

           <td class="lokalizacja" id="td7" runat="server" >
                <asp:Label ID="Label16" Text='<%# Eval("Lokalizacja") %>' runat="server" />
            </td>

            <td class="cc" id="td3" runat="server">
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("CC") %>' ToolTip='<%# Eval("CC") %>' />
            </td>--%>

            <td class="stanowisko" id="td2" runat="server" >
                <asp:Label ID="Label7" Text='<%# Eval("Stanowisko") %>' runat="server" />
            </td>

            <td class='<%# GetKierCss() %> divR'>            
<%--
                <asp:Label ID="lbNrEwid" Text='<%# Eval("Nr_Ewid") %>' runat="server" visible='<%# !GetStrKierVisible() %>'/>
                <asp:Label ID="Label9" Text='<%# Eval("Kierownik") %>' runat="server" visible='<%# GetStrKierVisible() %>'/>
--%>
                <asp:Label ID="Label9" runat="server" Text='<%# Eval("Kierownik") %>' visible='<%# (bool)Eval("Ja") %>'/>
                <asp:LinkButton ID="LinkButton14" runat="server" Text='<%# Eval("Kierownik") %>' Visible='<%# !(bool)Eval("Ja") %>' CommandName="zoomKier" CommandArgument='<%# Eval("IdKierownika") %>' ></asp:LinkButton>            

                
      <%--      <asp:Repeater ID="rpLine" runat="server" OnItemCommand="rpUprawnienia_ItemCommand" OnInit="rpUprawnienia_Init">
                <ItemTemplate>
                    </td><td class='data <%# GetDataCss(Container.DataItem) %>'>
                    <asp:LinkButton ID="lbtCertyfikat" runat="server" CommandName="zoomCert" CommandArgument='<%# GetDataCmd(Container.DataItem) %>' Text='<%# GetData(Container.DataItem) %>' Visible='<%# GetVisible(Container.DataItem) %>' ></asp:LinkButton>
                        <% if(TwoColumn) { %>
                            </td><td class='ocena <%# GetDataCss(Container.DataItem) %>'>
                            <asp:LinkButton ID="LinkButton16" runat="server" CommandName="zoomOcena" CommandArgument='<%# GetDataCmd(Container.DataItem) %>' Text='<%# GetData2(Container.DataItem) %>' Visible='<%# GetVisible(Container.DataItem) %>'></asp:LinkButton>
                        <% } %>
                </ItemTemplate>
            </asp:Repeater> --%>
            </td>
            <td class="tdData"><div class="data-scroller"><asp:Repeater ID="rpLine" runat="server" OnItemCommand="rpUprawnienia_ItemCommand" OnInit="rpUprawnienia_Init"><ItemTemplate><div class='data <%# GetDataCss(Container.DataItem) %>'><asp:LinkButton ID="lbtCertyfikat" runat="server" CommandName="zoomCert" CommandArgument='<%# GetDataCmd(Container.DataItem) %>' Text='<%# GetData(Container.DataItem) %>' Visible='<%# GetVisible(Container.DataItem) %>'></asp:LinkButton></div><% if (TwoColumn){%><div class='ocena <%# GetDataCss(Container.DataItem) %>'><asp:LinkButton ID="LinkButton16" runat="server" CommandName="zoomOcena" CommandArgument='<%# GetDataCmd(Container.DataItem) %>' Text='<%# GetData2(Container.DataItem) %>' Visible='<%# GetVisible(Container.DataItem) %>'></asp:LinkButton></div><% } %></ItemTemplate></asp:Repeater></div>
            </td>
            <td class="suma" colspan="1">
                <asp:LinkButton ID="lbtSumA" runat="server" CssClass='<%# GetNumClass(Eval("Aktualne")) %>' CommandName="zoomLinia" CommandArgument='<%# "A|" + Eval("IdPracownika") %>' Text='<%# Eval("Aktualne") %>'></asp:LinkButton>
            </td>
            <td class="suma" colspan="1">
                <asp:LinkButton ID="lbtSumW" runat="server" CssClass='<%# GetNumClass(Eval("Wygasajace")) %>' CommandName="zoomLinia" CommandArgument='<%# "W|" + Eval("IdPracownika") %>' Text='<%# Eval("Wygasajace") %>'></asp:LinkButton>
            </td>
            <td class="suma" colspan="1">
                <asp:LinkButton ID="lbtSumP" runat="server" CssClass='<%# GetNumClass(Eval("Przeterm")) %>' CommandName="zoomLinia" CommandArgument='<%# "P|" + Eval("IdPracownika") %>' Text='<%# Eval("Przeterm") %>' ></asp:LinkButton>            
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" class="edt">
            <tr>
                <td>
                    <div id="paKierUp" runat="server" class="kierUp" visible="false">
                        <asp:ImageButton ID="ibtUp" runat="server" ImageUrl="~/images/buttons/upitems.png" CssClass="img" CommandName="zoomKierUp" ToolTip="Cofnij" />
                        <asp:LinkButton ID="lbtUp" runat="server" Text="Cofnij" CommandName="zoomKierUp" ToolTip="Cofnij" ></asp:LinkButton>
                    </div>
<%--                    <asp:Label ID="NoDataLabel" Text="Brak danych" runat="server" />--%>
                        <div class="well well-sm">Brak danych</div>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbUprawnienia xnarrow">
            <tr runat="server">
                <td >
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr class="firsts">
                            <th colspan="2" rowspan="1" class="ups divR" id="thUPS" runat="server">                
                                <div id="paKierUp" runat="server" class="kierUp" visible="false">
                                    <asp:ImageButton ID="ibtUp" runat="server" ImageUrl="~/images/buttons/upitems.png" CssClass="img" CommandName="zoomKierUp" ToolTip="Cofnij" />
                                    <asp:LinkButton ID="lbtUp" runat="server" Text="Cofnij" CommandName="zoomKierUp" ToolTip="Cofnij" ></asp:LinkButton>
                                </div>
                                
                                
                                                                
                                <%--
                                <asp:Repeater ID="rpPath" runat="server" >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtKier" runat="server" Text='<%# Eval("Kierownik") %>' CommandName="zoomKier2" CommandArgument='<%# Eval("IdKierownika") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                                --%>
                                
                                
                                
                                
                                <asp:Label ID="Label1" Text="Uprawnienie" runat="server" /><br />
<%--
                                <asp:Label ID="Label2" Text="Poziom" runat="server" /><br />
--%>
                                <asp:Label ID="Label3" Text="Symbol" runat="server" /></th>
                            <th class="tdData">

                                <div class="data-scroller">
                                    <asp:Repeater ID="rpHeader1" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                        <ItemTemplate><div class='data1 xvert<%# GetHeaderClass() %>' colspan='<%# GetColspan %>'>
                                                <div title='<%# Eval("Nazwa") %>'>
                                                    <asp:LinkButton ID="lbtCertyfikat" runat="server" CommandName="zoomUpr" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Nazwa") %>'></asp:LinkButton>
                                                </div></div></ItemTemplate>
                                    </asp:Repeater></div></th><th rowspan="2" class="suma vert" colspan="1"><div><span><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Aktualne" Text="Aktualne"></asp:LinkButton></span></div></th><th rowspan="2" class="suma vert" colspan="1"><div><span><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Wygasajace" Text="Wygasające"></asp:LinkButton></span></div></th><th rowspan="2" class="suma vert" colspan="1">
                                <div>
                                    <span>
                                        <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Przeterm" Text="Przeterm."></asp:LinkButton>
                                    </span>
                                </div>
                            </th>
                        </tr>
                        <%--
                        <tr>
                            <th class="data2">    
                            <asp:Repeater ID="rpHeader2" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                <ItemTemplate>
                                    <div>
                                        <asp:Label ID="lbVal" runat="server" Text='<%# Eval("Poziom") %>' />    
                                    </div>
                                </ItemTemplate>                            
                                <SeparatorTemplate>
                                    </th><th class="data2">
                                </SeparatorTemplate>
                            </asp:Repeater>                                
                            </th>
                        </tr>                        
                        <tr>
                            <th class="data2">    
                            <asp:Repeater ID="rpHeader3" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                <ItemTemplate>
                                    <div>
                                        <asp:Label ID="lbVal" runat="server" Text='<%# Eval("Symbol") %>' />    
                                    </div>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    </th><th class="data2">
                                </SeparatorTemplate>
                            </asp:Repeater>                                
                            </th>
                        </tr>
                        --%>
                        <tr runat="server" class="header">
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Pracownik" Text="Pracownik" ></asp:LinkButton>            
                            </th>
                            <th id="thNrEw2" runat="server" visible="false">
                                <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Nr_Ewid2" Text="Nr Spaw." ></asp:LinkButton>            
                            </th>

                            <th id="th1" runat="server" visible="false">
                                <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Nr_Ewid" Text="Nr Ew." ></asp:LinkButton>            
                            </th>

                            <th id="th3" runat="server" visible="true">
                                <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="DataZatr" Text="Data zatrudnienia" ></asp:LinkButton>            
                            </th>
                          <%--   <th id="th4" runat="server" visible="true">
                                <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="DataZwol" Text="Status / Data&nbsp;zwolnienia" ></asp:LinkButton>            
                            </th>

                           <th id="th6" runat="server" visible="true">
                                <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="Lokalizacja" Text="Lokalizacja" ></asp:LinkButton>            
                            </th>

                            <th id="th5" runat="server" visible="true">
                                <asp:LinkButton ID="LinkButton13" runat="server" CommandName="Sort" CommandArgument="CC" Text="CC" ></asp:LinkButton>            
                            </th>--%>
                            <th id="th2" runat="server" visible="false">
                                <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="Stanowisko" Text="Stanowisko" ></asp:LinkButton>            
                            </th>
                            
                            <th runat="server" class="divR" >
                                <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Nr_Ewid" Text="Nr Ew." Visible="true"></asp:LinkButton>            
                                <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Kierownik" Text="Przełożony" Visible="false"></asp:LinkButton>            
<%--
                                <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Nr_Ewid2" Text="#Spaw." ></asp:LinkButton>            
--%></th>
                            <th class="tdData">
                                <div class="data-scroller">
                                    <asp:Repeater ID="rpHeader4" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                        <ItemTemplate><div class='uprtitle<%# GetHeaderClass() %>'><asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument='<%# "[" + Eval("Id") + "]" %>' Text='<%# Eval("Symbol") %>'></asp:LinkButton></div><% if (TwoColumn)
                                               { %>
                                            <div class='ocena<%# GetHeaderClass() %>'>
                                                <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort1" CommandArgument='<%# "[" + Eval("Id") + "]" %>'>
                                        <i class="fa fa-check"></i>
                                                </asp:LinkButton>

                                            </div>
                                            <% } %></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </th>
<%--
'< % # "[" + Eval("Id") + "]" %>'
                            </th>
                            <th id="thUprTitle" runat="server" class="uprtitle">
                                Daty obowiązywania uprawnień
                            </th>
--%>                            
                        </tr>
                        <%----- daty obowiązywania -----%>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <%----- navigator -----%>
                        <tr id="trNav" runat="server" class="navigator" visible="false">
                            <td colspan="2"></td>
                            <td id="tdNav" runat="server">
                            
                            </td>
                            <td id="tdNavS" colspan="3"></td>
                        </tr>
                        <%----- sumy -----%>
                    <%--    <tr class="sumy it">
                            <td class="suma_title divR" colspan="1" id="tdSum1" runat="server"><asp:Label ID="Label4" Text="Aktualne" runat="server" />
                            
                                <asp:Repeater ID="rpSumA" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                <ItemTemplate>
                                    </td><td class='suma<%# GetSumClass() %>' colspan='<%# GetColspan2() %>' >
                                    <asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "A|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>' ></asp:LinkButton>            
                                </ItemTemplate>
                            </asp:Repeater>                                
                            </td>                        
                        </tr>
                        <tr class="sumy it">
                            <td class="suma_title divR" colspan="1" id="tdSum2" runat="server"><asp:Label ID="Label5" Text="Wygasające" runat="server" />
                            <asp:Repeater ID="rpSumW" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                <ItemTemplate>
                                    </td><td class='suma<%# GetSumClass() %>' colspan='<%# GetColspan2() %>' >
                                     <asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "W|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>' ></asp:LinkButton>            
                                </ItemTemplate>
                            </asp:Repeater>                                
                            </td>                        
                        </tr>
                        <tr class="sumy it">
                            <td class="suma_title divR" colspan="1" id="tdSum3" runat="server"><asp:Label ID="Label6" Text="Przeterminowane" runat="server" />
                            <asp:Repeater ID="rpSumP" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                <ItemTemplate>
                                    </td><td class='suma<%# GetSumClass() %>' colspan='<%# GetColspan2() %>' >
                                    <asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "P|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>' ></asp:LinkButton>            
                                </ItemTemplate>
                            </asp:Repeater>                                
                            </td>                        
                        </tr>
                        <tr class="sumy it" id="trSumB" runat="server" visible="false">
                            <td class="suma_title divR" colspan="1" id="tdSum4" runat="server"><asp:Label ID="Label8" Text="Brak" runat="server" />
                            <asp:Repeater ID="rpSumB" runat="server" OnItemCommand="rpUprawnienia_ItemCommand">
                                <ItemTemplate>
                                    </td><td class='suma<%# GetSumClass() %>' colspan='<%# GetColspan2() %>' >
                                    <asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "B|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>' ></asp:LinkButton>            
                                </ItemTemplate>
                            </asp:Repeater>                                
                            </td>                        
                        </tr>--%>



                        <tr class="sumy it"><td class="suma_title divR" colspan="1" id="tdSum1" runat="server"><asp:Label ID="Label4" Text="Aktualne" runat="server" /></td><td class="tdData"><div class="data-scroller"><asp:Repeater ID="rpSumA" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "A|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></div></td><td class="tdTotal" colspan="3"><asp:Repeater ID="rpSumATotal" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "A|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></td></tr><tr class="sumy it"><td class="suma_title divR" colspan="1" id="tdSum2" runat="server"><asp:Label ID="Label5" Text="Wygasające" runat="server" /></td><td class="tdData"><div class="data-scroller"><asp:Repeater ID="rpSumW" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "W|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></div></td><td class="tdTotal" colspan="3"><asp:Repeater ID="rpSumWTotal" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "W|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></td></tr><tr class="sumy it"><td class="suma_title divR" colspan="1" id="tdSum3" runat="server"><asp:Label ID="Label6" Text="Przeterminowane" runat="server" /></td><td class="tdData"><div class="data-scroller"><asp:Repeater ID="rpSumP" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "P|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></div></td><td class="tdTotal" colspan="3"><asp:Repeater ID="rpSumPTotal" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "P|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></td></tr><tr class="sumy it"><td class="suma_title divR" colspan="1" id="tdSum4" runat="server"><asp:Label ID="Label7" Text="Brak" runat="server" /></td><td class="tdData"><div class="data-scroller-main"><asp:Repeater ID="rpSumB" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "B|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></div></td><td class="tdTotal" colspan="3"><asp:Repeater ID="rpSumBTotal" runat="server" OnItemCommand="rpUprawnienia_ItemCommand"><ItemTemplate><div class='suma<%# GetSumClass() %>'><asp:LinkButton ID="lbtSum" runat="server" CssClass='<%# GetNumClass(GetItem(Container.DataItem, 1)) %>' CommandName="zoomSumy" CommandArgument='<%# "B|" + GetItem(Container.DataItem, 0) %>' Text='<%# GetItem(Container.DataItem, 1) %>'></asp:LinkButton></div></ItemTemplate></asp:Repeater></td></tr>



                    </table>
                </td>
            </tr>

            <tr id="Tr4" class="pager" runat="server" visible="false">
                <td id="Td4" class="left" runat="server">
                    <uc1:LetterDataPager ID="LetterDataPager1" Letter="NazwiskoLetter" runat="server" />
                </td>
                <td class="right">
<%--                    <span class="count"><asp:Literal ID="lbCountLabel" runat="server" Text="Pracowników:" /><asp:Label ID="lbCount" runat="server" ></asp:Label></span>--%>
                </td>
            </tr>
            
            <tr class="pager">
                <td>
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15" OnLoad="DataPager1_Load">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="«" PreviousPageText="‹" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="›" LastPageText="»" />
                        
                            <%--
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                            --%>
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count"><asp:Literal ID="lbCountLabel" runat="server" Text="Pracowników:" /><asp:Label ID="lbCount" runat="server" ></asp:Label></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbPageSize" runat="server" CssClass="count" Text="Pokaż na stronie:"></asp:Label>&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlLines" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>
</div>

    </ContentTemplate>
</asp:UpdatePanel>

<%-- zoomy --%>
<div id="divZoom" style="display:none;" class="modalPopup">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <uc5:PracownikDetails ID="cntPracownik" runat="server" Visible="false"/>
<%--            <uc6:cntCertyfikat ID="cntCertyfikat" runat="server" Mode="0"  Visible="false" />                        
            <uc6:cntCertyfikat ID="cntCertyfikatAdd" runat="server" Mode="1" Visible="false" />       --%>
            
            <uc6:cntCertyfikat9 ID="Certyfikat" runat="server" Visible="false" />       
            <uc6:cntCertyfikat9 ID="CertyfikatAdd" runat="server" Visible="false" />                 
            <%--
            <uc6:cntCertyfikatSpaw ID="cntCertyfikatSpaw" runat="server" Visible="false" Mode="1"/>            
            --%>            
            <uc6:cntPracownikUprawnienia ID="cntPracownikUprawnienia" runat="server" Visible="false"/>
            <uc6:cntUprawnieniePracownicy ID="cntUprawnieniePracownicy" runat="server" Visible="false"/>
            <uc6:cntUprPracownicyTotal ID="cntUprPracownicyTotal" runat="server" Visible="false"/>
            <%--
            <uc6:cntUprCzynnosciHeader ID="cntUprCzynnosciHeader" runat="server" Visible="false"/>
            --%>
            <div class="bottom_buttons">
    
                 <asp:Button ID="btnKarta" runat="server" Visible="false" CssClass="btn btn-sm btn-primary" Text="Karta szkolenia" OnClick="btnKarta_Click" />
                <asp:Button ID="btClose" CssClass="btn btn-default btn-sm pull-right" runat="server" Text="Zamknij" OnClick="btClose_Click" />
                <asp:Button ID="btExcel" CssClass="btn btn-success btn-sm pull-right" runat="server" Text="Excel" OnClick="btExcel_Click" Visible="false" style="margin-right: 5px !important;" />
<%--
                <asp:Button ID="btExcel" CssClass="button100" runat="server" Text="Excel" OnClick="btExcel_Click" Visible="true" UseSubmitBehavior="false" />
                <asp:Button ID="btClose" CssClass="button100" runat="server" Text="Zamknij" OnClick="btClose_Click" UseSubmitBehavior="false"/>
--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel"/>
            <asp:PostBackTrigger ControlID="btnKarta"/>
        </Triggers>
    </asp:UpdatePanel>
</div>


<div id="divZoomOceny" style="display:none;" class="modalPopup">
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <uc5:PracownikDetails ID="PracownikDetails1" runat="server" Visible="false"/>
            <cc:cntOceny id="Oceny" runat="server" Visible="false" />
            <div class="bottom_buttons">
                <asp:Button ID="btCloseOcena" CssClass="btn btn-default btn-sm pull-right" runat="server" Text="Zamknij" OnClick="btClose_Click"  />
                <%--<asp:Button ID="Button2" CssClass="btn btn-success btn-sm pull-right" runat="server" Text="Excel" OnClick="btExcel_Click" Visible="true" style="margin-right: 5px !important;" />--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel"/>
        </Triggers>
    </asp:UpdatePanel>
</div>




                </div>
            </div>
            <%-- szkolenia page END --%>
<%--        
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcelF" />
        </Triggers>
    </asp:UpdatePanel>
--%>
</div>


<%--
Tools.GetLineParams(item.ToString(), 
out data,	-> x -> do pokazania "bezterminowo" 
out cid, 	-> jest certyfikat lub x, par cmd	
out css, 	
out pid,	-> par cmd (pracownik) 
out uid, 	-> par cmd (uprawnienie)
out u1, 		-> poziom id
out u2, 		-> poziom
out data2,	-> do pokazania 
out skip1, 
out skip2);
            
            if (cid != "x")
            {
                return data == "x" ? L.p("bezterminowo") : data2;
            }
            else
            {
                return L.p("Dodaj");
            }
--%>


<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
--declare @t datetime = GETDATE()

--declare @typ int = 1024
--declare @data datetime = '20150901'
--declare @monit int = 30   -- dni
--declare @kwal int = -99
--declare @filter int = -99
--declare @status varchar(20) = 'p'  -- p-zatrudnieni, n-nowi/nieprzypisani, z-zwolnieni, all
--declare @withonly bit = 0
--declare @spawonly bit = 0
--declare @kierId int = 4
----declare @showK bit = 1
--declare @sub bit = 1

--select * from VPrzypisaniaNaDzis where KierId is not null and KadryId &lt; 80000 and Status != -2 order by id
--select * from Pracownicy

declare @showK bit 
set @showK = 1

declare 
    @colsH nvarchar(max), 
    @colsHcsv nvarchar(max), 
    @colsD nvarchar(max), 
    @uprList nvarchar(max), 
    @stmt1 nvarchar(max),
    @stmt2 nvarchar(max),
    @stmt3 nvarchar(max),
    @ucount int,
    @s0 nvarchar(max),
    @s1 nvarchar(max),
    @s1csv nvarchar(max),
    @s2 nvarchar(max)
    
set @ucount = 0
select 
    --@colsH = isnull(@colsH + ',', '') + '[' + convert(varchar, Id) + '][' + Symbol + ']'
    --@colsH = isnull(@colsH + ',', '') + '[' + convert(varchar, Id) + '][U' + convert(varchar, Id) + ']'
    @colsH = isnull(@colsH + ',', '') + '[' + convert(varchar, Id) + ']'
   ,@colsD = isnull(@colsD + ',', '') + '[' + convert(varchar, Id) + ']'
   ,@colsHcsv = isnull(@colsHcsv + ',', '') + '[' + convert(varchar, Id) + '][' + Symbol + ' - ' + Nazwa + ']'  --lang
   ,@uprList = isnull(@uprList + ',', '') + convert(varchar, Id)
   ,@ucount = @ucount + 1
from Uprawnienia 
where (@typ = -99 or not Typ & @typ = 0)
  and (@kwal = -99 or KwalifikacjeId = @kwal)
  and (@grupa = -99  or IdGrupy = @grupa)
  and Grupa = 0 and Aktywne = 1
order by Kolejnosc

--select @colsH, @colsD, @colsHcsv, @uprList, @ucount

if @showK = 1 begin
    set @s0 = ''
    set @s1 = ''
    set @s1csv = ''    
    set @s2 = ''
end
else begin
    set @s0 = ''
    set @s1 = ''
    set @s1csv = ''
    set @s2 = ''
end

--select @typ, @kwal, @filter, @data, @monit, @ucount, @status, @withonly, @spawonly, @kierId
--select @s0, @s1, @s2, @s1csv

set @stmt1 = '

declare @od datetime = ''20050101''
declare @do datetime = GETDATE()

declare 
	@typ2 int,
	@kwal2 int,
	@kierId int,
	@filter2 int,
	@data2 datetime,
	@dataW datetime,
	@ucount2 int,
	@status2 varchar(10),
	@cc int,
	@withonly bit,
	@spawonly bit,
	@sub bit,
	@grupa int,
	@pracownicytyp nvarchar(40),
    @firma nvarchar(60)
	
set @typ2 = ' + CONVERT(varchar, @typ) + '
set @kwal2 = ' + CONVERT(varchar, @kwal) + '
set @grupa = ' + CONVERT(varchar, @grupa) + '
set @filter2 = ' + CONVERT(varchar, @filter) + '
set @data2 = ''' + CONVERT(varchar(10), @data, 20) + '''
set @pracownicytyp = ''' + @pracownicytyp + '''
set @dataW = ''' + CONVERT(varchar(10), DATEADD(D, @monit, @data), 20) + '''
set @ucount2 = ' + CONVERT(varchar, @ucount) + '
set @status2 = ''' + @status + '''
set @cc = ' + CONVERT(varchar, @cc) + '
set @withonly = ' + CONVERT(varchar, @withonly) + '
set @spawonly = ' + CONVERT(varchar, @spawonly) + '
set @kierId = ' + ISNULL(CONVERT(varchar, @kierId), 'null') + '
set @sub =  ' + CONVERT(varchar, @sub) + '
set @firma = ''' + CONVERT(varchar, @firma) + '''

' + @s0 + '    

select
 
--:CUT
PV.IdPracownika, PV.Pracownik as Pracownik, PV.Nazwisko, PV.Imie, PV.Nr_Ewid, PV.Nr_Ewid2, PV.APT, PV.NazwiskoLetter, 
' + @colsH + '
,S.Aktualne, S.Wygasajace, S.Przeterm, S.Brak
,ISNULL(K.Nazwisko + '' '' + K.Imie + ISNULL('' ('' + K.KadryId + '')'',''''),'''') [Kierownik], PV.IdKierownika, cast(case when PV.IdKierownika = @kierId then 1 else 0 end as bit) as Ja
,ST.Nazwa as Stanowisko
,PV.DataZatr

,PV.DataZwol

, PV.Status, PV.Kierownik as IsKierownik, PV.Opis as Lokalizacja
--,dbo.fn_GetPracLastCCPodzialLudzi(PV.GrSplitu, @data2, 5, '', '') CC
,dbo.fn_GetCC(PV.PrzId, 5, '', '') CC
--,dbo.fn_GetPracLastCCPodzialLudzi(PV.GrSplitu, @data2, 5, '', '') + '' | '' + dbo.fn_GetCC(PV.PrzId, 5, '', '') CC

,'''' [Css]
,'''' [ToolTip]
,'''' [StrOrg]

' + @s1 + '    

/*:CSV
PV.Pracownik, PV.Nr_Ewid [Nr ew.]
,PV.Opis [Lokalizacja]
,convert(varchar(10),PV.DataZatr,20) [Data zatrudnienia]

--,convert(varchar(10),PV.DataZwol,20) [Data zwolnienia]
,PV.DataZwol [Data zwolnienia]

--,dbo.fn_GetPracLastCCPodzialLudzi(PV.GrSplitu, @data2, 5, '', '') CC
,dbo.fn_GetCC(PV.PrzId, 5, '', '') CC

,ST.Nazwa [Stanowisko]
,ISNULL(K.Nazwisko + '' '' + K.Imie + ISNULL('' ('' + K.KadryId + '')'',''''),'''') [Przełożony]
' + @s1csv + '    
,' + @colsHcsv + '
*/
from
(
select 
	P.Id as IdPracownika, P.Nazwisko + '' '' + P.Imie as Pracownik, P.Nazwisko, P.Imie, P.KadryId as Nr_Ewid, P.KadryId2 as Nr_Ewid2, 0 as APT, LEFT(LTRIM(P.Nazwisko), 1) as NazwiskoLetter,
	null as Id_Str_OrgM, R.IdKierownika, R.Id as PrzId, R.Do as PrzDo, P.DataZatr, 
	--P.DataZwol, 
    ISNULL(I.DataZwolStatus, convert(varchar(10),P.DataZwol,20)) as DataZwol,	
	P.Status, P.GrSplitu, P.Kierownik, P.Opis,

    --:CUT
	case
        when ISNULL(C2.Status, C.Status) = 1 then ''y''
        when ISNULL(C2.Status, C.Status) = -1 then ''x''
        else ISNULL(CONVERT(varchar(10), ISNULL(C2.DataWaznosci, C.DataWaznosci), 20), ''x'')
    end + ''|'' +		
	--ISNULL(CONVERT(varchar, ISNULL(C2.Id, C.Id)), ''x'') + ''|'' + 
	case when ISNULL(C2.Status, C.Status) = -1 then ''z'' else ISNULL(CONVERT(varchar, C.Id), ''x'') end + ''|'' + 

	case
        when ISNULL(C2.Status, C.Status) is not null and ISNULL(C2.Status, C.Status) = -1 then ''odrz''
        when ISNULL(C2.Status, C.Status) is not null and ISNULL(C2.Status, C.Status) = 1 then ''aoi''
		when ISNULL(C2.Id, C.Id) is not null and ISNULL(C2.DataWaznosci, C.DataWaznosci) is null then ''bezterm '' /*+ CONVERT(varchar, ISNULL(C2.Status, C.Status))*/ /* debug */
		when ISNULL(C2.DataWaznosci, C.DataWaznosci) between @data2 and @dataW then ''monit''
		when ISNULL(C2.DataWaznosci, C.DataWaznosci) &gt;= @data2 then ''ok''
		when ISNULL(C2.DataWaznosci, C.DataWaznosci) &lt; @data2 then ''po''
	--else ''brak''
	else '''' /*+ CONVERT(varchar, ISNULL(C2.Status, C.Status))*/ /* nie odremowywac */
	end	+
	case when ((C.Id is null) /*or (ISNULL(C2.Status, C.Status) = -1)*/) then case
        when dbo.GetRightId(P.Rights, 95) = 1 and U.NazwaEN = ''WOZEK'' then '' po'' /* if wozek widlowy then na czerwono */
        when U.IdGrupy = (select top 1 Id from UprawnieniaGrupy where NazwaEN = ''0'' and Typ = 2048)  then '' brak''
        else '' brak control''
        end else '''' end + ''|'' +  --- UWAGA ZMIENIć W PRZYPADKU ZMIANY ID
	ISNULL(CONVERT(varchar, P.Id), '''') + ''|'' + 
	ISNULL(CONVERT(varchar, U.Id), '''') + ''|'' +
	
	ISNULL(CONVERT(varchar, U.PoziomId), '''') + ''|'' +
	ISNULL(CONVERT(varchar, U.PoziomPoziom), '''') + ''|'' +
   
    --aacase when C.UprId = C.IdUprawnienia then ISNULL(CONVERT(varchar(10), C.DataWaznosci, 20), ''x'') else '''' end
    --xxISNULL(CONVERT(varchar(10), C.DataWaznosci, 20), ''x'')
    case when U.Id = C.IdUprawnienia then ISNULL(CONVERT(varchar(10), C.DataWaznosci, 20), ''x'') else '''' end
    
    /*:CSV
    case
        when ISNULL(C2.Status, C.Status) is not null and ISNULL(C2.Status, C.Status) = 1 then ''w trakcie''
        when C.Id is null then ''''   --brak
        when ISNULL(C2.DataWaznosci, C.DataWaznosci) is null then ''bezterminowo''
    else ISNULL(CONVERT(varchar(10), C.DataWaznosci,20), '''')    
    end
    */
    + ''|'' + CONVERT(varchar, ISNULL(w.Ocena, 0))
	as Certyfikat,
	U.Id
	
from Pracownicy P




--left join PodzialLudziImport I on I.KadryId = P.KadryId and @data2 between I.OkresOd and ISNULL(I.OkresDo,''20990909'')
left join PodzialLudziImport IX on IX.KadryId = P.KadryId and @data2 between IX.OkresOd and ISNULL(IX.OkresDo,''20990909'')
left join PlanUrlopowPomin PU on PU.IdPracownika = P.Id and @data2 between PU.Od and ISNULL(PU.Do,''20990909'')
left join Kody PUK on PUK.Typ = ''ABSDL'' and PUK.Kod = PU.PowodKod
--outer apply (select IX.DataZwolStatus) I1
outer apply (select 
	case when PU.Id is null then IX.DataZwolStatus 
	else ISNULL(PUK.Nazwa + '' - '' + PUK.Nazwa2 + ISNULL('' ('' + PU.Powod + '')'', ''''), '' '' + ISNULL(PU.Powod, ''absencja długotrwała'')) --spacja żeby nie A!
	end as DataZwolStatus) I


    '

set @stmt2 = '


--left join Przypisania R1 on R1.IdPracownika = P.Id and @data2 between R1.Od and ISNULL(R1.Do,''20990909'') and R1.Status = 1
--left join Przypisania R on R.IdPracownika = P.Id and @data2 between R.Od and ISNULL(R.Do,''20990909'') and R.Status = 1
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @data2 and Status = 1 order by Od desc) R

left join Uprawnienia U on (@typ2 = -99 or not U.Typ & @typ2 = 0) and (@kwal2 = -99 or U.KwalifikacjeId = @kwal2) and U.Grupa = 0 and U.Aktywne = 1
--20170315
left join UprawnieniaKwalifikacje UK on UK.Id = U.KwalifikacjeId
outer apply (select case when UK.NazwaEN in (''EDU'', ''UPR'') then 1 else 0 end SprzedZatr) UK1

--left join Certyfikaty C on C.IdUprawnienia = U.Id and C.IdPracownika = P.Id and C.Aktualny = 1

/*left join
(
    select
      u.Id
    , o.IdPracownika
    , o.Ocena
    from Uprawnienia u
    left join msOceny o on o.IdUprawnienia = u.Id
    inner join
    (
	    select
	      o0.IdPracownika
	    , MAX(o0.Status) Status
	    from msOceny o0
	    where o0.Status &gt; -1
	    group by IdPracownika
    ) o2 on o2.IdPracownika = o.IdPracownika and o2.Status = o.Status and GETDATE() between o.DataOd and ISNULL(o.DataDo, ''20990909'')
) w on w.Id = U.Id and w.IdPracownika = P.Id
*/
outer apply ( select top 1 * from msOceny o where o.IdPracownika = P.Id and o.IdUprawnienia = U.Id and @data2 between o.DataOd and ISNULL(o.DataDo, ''20990909'') and o.Status &gt; -1 order by Status desc, DataOd desc) w
left join PracownicyStanowiska ps on ps.IdPracownika = P.Id and GETDATE() between ps.Od and ISNULL(ps.Do, ''20990909'')

outer apply (select top 1 * from Certyfikaty 
            where IdUprawnienia = U.Id and IdPracownika = P.Id 
    
            --20170315        
            --and ISNULL(DataZdobyciaUprawnien, ''20990909'') &gt;= DATEADD(D,-7,P.DataZatr)
            and (UK1.SprzedZatr = 1 or ISNULL(DataZdobyciaUprawnien, ''20990909'') &gt;= DATEADD(D,-7,P.DataZatr))
            
    order by DataWaznosci desc) C
outer apply (select top 1 C.* ) C2

/*outer apply (select top 1 * from VCertyfikatyUprawnienieMS
            where U.PoziomId is not null 
            and Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId and IdPracownika = P.Id and Aktualny = 1 
            and PoziomId = U.PoziomId and PoziomPoziom &gt;= U.PoziomPoziom 
            order by PoziomPoziom) C1					    --uzupełnienie pustych
outer apply (select top 1 * from VCertyfikatyUprawnienieMS
            where U.PoziomId is not null 
            and Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId and IdPracownika = P.Id and Aktualny = 1 
            and PoziomId = U.PoziomId --and PoziomPoziom &lt;= C1.PoziomPoziom 
            and DataWaznosci &gt;= C1.DataWaznosci
            order by PoziomPoziom desc, DataWaznosci desc) C2					--nowsze 20151020 dodanie Datawaznosci desc !!! dla szkoleń */
--where ((@typ2 = -99 or not U.Typ & @typ2 = 0) and (@kwal2 = -99 or U.KwalifikacjeId = @kwal2) and U.Grupa = 0) and

'
set @stmt3 = '
where --P.KadryId between 0 and 80000-1 -- CETOR
--and P.IdStanowiska in (select items from dbo.SplitInt((select Parametr from Kody where Typ = @pracownicytyp and Aktywny = 1), '','')) or @pracownicytyp = ''DEFAULT''
--and 
(
    @status2 = ''p''  and P.Status != -2 and ISNULL(P.DataZwol,''20990909'') &gt; @data2 or	--zatrudnieni
    @status2 = ''pa'' and P.Status != -2 and ISNULL(P.DataZwol,''20990909'') &gt; @data2 and LEFT(ISNULL(I.DataZwolStatus,''A''),1) in (''A'',''2'') or	--zatrudnieni
    @status2 = ''k''  and P.Status != -2 and ISNULL(P.DataZwol,''20990909'') &gt; @data2 and P.Kierownik = 1 or	--kierownicy
    --@status2 = ''n'' and (P.Status = 1 or P.Status = 0 and P.DataZwol is null) or			--nowi/nieprzypisani
    @status2 = ''n''  and (P.Status = 1 or P.Status = 0 and (R.Id is null or ISNULL(R.Do,''20990909'') &lt; @data2)) or			--nowi/nieprzypisani
    --@status2 = ''n'' and (P.Status = 1 or P.Status = 0 and R1.Id is null) or			--nowi/nieprzypisani
    @status2 = ''z''  and P.Status != -2 and ISNULL(P.DataZwol,''20990909'') &lt; @data2 or	--zwolnieni
    /*@status2 = ''a'' and P.Status != -2 and ISNULL(P.DataZwol,''20990909'') &gt; @data2 and ISNULL(ps.Klasyfikacja, ''DBW'') != ''DBW'' and ISNULL(ps.Klasyfikacja, ''ARESZT'') != ''ARESZT'' or*/
    /*@status2 = ''o'' and P.Status != -2 and ISNULL(P.DataZwol,''20990909'') &gt; @data2 and ISNULL(ps.Klasyfikacja, '''') = ''ARESZT'' or*/
	@status2 = ''all'' and P.Status != -2		--wszyscy, oprócz pomijanych
    )
and (
    @kierId = -99 or
	@sub = 0 and R.IdKierownika = @kierId or
	@sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 1, @data2) where Kierownik = 1)
    )
and 
(
    @firma = ''-99'' or
    @firma = ISNULL(ps.Klasyfikacja, ''-1'')
)
and
(
    /*
    @pracownicytyp = ''DEFAULT'' or
    (@pracownicytyp = ''MSPRODUKCJA'' and (ISNULL(ps.Rodzaj, ''-1'') = ''P'' or ps.Rodzaj is null)) or
    (@pracownicytyp = ''MSADMINISTRACJA'' and (ISNULL(ps.Rodzaj, ''-1'') = ''A'' or ps.Rodzaj is null))
    */
    @pracownicytyp = ''DEFAULT'' or
    (@pracownicytyp = ''MSPRODUKCJA'' and (ISNULL(ps.Grupa, ''-1'') != ''ADMIN'')) or
    (@pracownicytyp = ''MSADMINISTRACJA'' and (ISNULL(ps.Grupa, ''-1'') = ''ADMIN''))
)
and (@withonly = 0 or P.Id in (select distinct U.IdPracownika from VCertyfikatyUprawnienieMS U where U.Aktualny = 1 and (@typ2 = -99 or not U.Typ & @typ2 = 0) and (@kwal2 = -99 or U.KwalifikacjeId = @kwal2)

-- tu bylem


and (@grupa = -99 or U.IdGrupy = @grupa)


))
--and (@spawonly = 0 or P.Nr_Ewid2 is not null)
--and (@cc = -99 or R.Id in (select IdPrzypisania from SplityWspP where IdCC = @cc))
) as D
PIVOT
(
	max(D.Certyfikat) for Id in (' + @colsD + ')  
) as PV
outer apply (
    /*--1 sumy wg certyfikatów
	select 
	ISNULL(SUM(case when DataWaznosci is null or DataWaznosci &gt;= @data2 then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when DataWaznosci between @data2 and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when DataWaznosci &lt; @data2 then 1 else 0 end), 0) [Przeterm],  
	ISNULL(@ucount2 - count(*), 0) [Brak]  
	from Certyfikaty where IdPracownika = PV.IdPracownika and Aktualny = 1 and IdUprawnienia in (' + @uprList + ')
    */
    /*--2 sumy wg Poziom
	select 
	ISNULL(SUM(case when Id2 is not null and DataWaznosci2 is null or DataWaznosci2 &gt;= @data2 then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when DataWaznosci2 between @data2 and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when DataWaznosci2 &lt; @data2 then 1 else 0 end), 0) [Przeterm],  
	ISNULL(@ucount2 - count(*), 0) [Brak]  
	from VCertyfikatyUprawnienieMS3 where IdPracownika = PV.IdPracownika and Aktualny = 1 and IdUprawnienia in (' + @uprList + ')
	*/
	--3 sumy wg Poziom - Szkolenia
    select	
    ISNULL(SUM(case when Id is not null and DataWaznosci is null or DataWaznosci &gt;= @data2 then 1 else 0 end), 0) [Aktualne],  
    ISNULL(SUM(case when DataWaznosci between @data2 and @dataW then 1 else 0 end), 0) [Wygasajace],  
    ISNULL(SUM(case when DataWaznosci &lt; @data2 then 1 else 0 end), 0) [Przeterm],
    ISNULL(@ucount2 - count(*), 0) [Brak]  
    from (
    select distinct Id, DataWaznosci
    from VCertyfikatyUprawnienieMS where IdPracownika = PV.IdPracownika and Aktualny = 1 and IdUprawnienia in (' + @uprList + ')
	) D
) S
left join Pracownicy K on K.Id = PV.IdKierownika
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = PV.IdPracownika and Od &lt;= @data2 order by Od desc) PS
left join Stanowiska ST on ST.Id = PS.IdStanowiska 

' + @s2 + '

where 
    @filter2 = -99 or
    @filter2 = 1 and PV.IdPracownika in (
        -- select C2.IdPracownika from VCertyfikatyUprawnienieMS2 C2 where (@typ2 = -99 or not C2.Typ & @typ2 = 0) and (@kwal2 = -99 or C2.KwalifikacjeId = @kwal2) and (@grupa = -99 or C2.IdGrupy = @grupa) 
        --    and C2.Aktualny = 1 and (C2.DataWaznosci2 is null or C2.DataWaznosci2 &gt;= @data2) 
	select C2.IdPracownika from VCertyfikatyUprawnienieMS C2 where (@typ2 = -99 or not C2.Typ & @typ2 = 0) and (@kwal2 = -99 or C2.KwalifikacjeId = @kwal2) and (@grupa = -99 or C2.IdGrupy = @grupa) 
			and C2.Aktualny = 1 and (C2.DataWaznosci is null or C2.DataWaznosci &gt;= @data2) 

    ) or
    @filter2 = 2 and PV.IdPracownika in (
  --select C2.IdPracownika from VCertyfikatyUprawnienieMS2 C2 where (@typ2 = -99 or not C2.Typ & @typ2 = 0) and (@kwal2 = -99 or C2.KwalifikacjeId = @kwal2) and (@grupa = -99 or C2.IdGrupy = @grupa)  
        --    and C2.Aktualny = 1 and (C2.DataWaznosci2 between @data2 and @dataW)
		select C2.IdPracownika from VCertyfikatyUprawnienieMS C2 where (@typ2 = -99 or not C2.Typ & @typ2 = 0) and (@kwal2 = -99 or C2.KwalifikacjeId = @kwal2) and (@grupa = -99 or C2.IdGrupy = @grupa)  
            and C2.Aktualny = 1 and (C2.DataWaznosci between @data2 and @dataW)  
    ) or
    /*
    @filter2 = 3 and PV.IdPracownika in (
        select C2.IdPracownika from VCertyfikatyUprawnienieMS2 C2 where (@typ2 = -99 or not C2.Typ & @typ2 = 0) and (@kwal2 = -99 or C2.KwalifikacjeId = @kwal2) and (@grupa = -99 or C2.IdGrupy = @grupa)  
            and C2.Aktualny = 1 and (C2.DataWaznosci2 &lt; @data2)         
    ) or
    */
    @filter2 = 3 and PV.IdPracownika in (
        select C2.IdPracownika from VCertyfikatyUprawnienieMS3 C2 where (@typ2 = -99 or not C2.Typ & @typ2 = 0) and (@kwal2 = -99 or C2.KwalifikacjeId = @kwal2) and (@grupa = -99 or C2.IdGrupy = @grupa)  
            and C2.Aktualny = 1 and (C2.DataWaznosci2 &lt; @data2)         
    ) or
    @filter2 = -1 and PV.IdPracownika not in (
        select C1.IdPracownika from VCertyfikatyUprawnienieMS C1 where (@typ2 = -99 or not C1.Typ & @typ2 = 0) and (@kwal2 = -99 or C1.KwalifikacjeId = @kwal2) and (@grupa = -99 or C1.IdGrupy = @grupa)  
		    and C1.Aktualny = 1
    )
order by Pracownik, PV.Nr_Ewid

/*
OPTION (OPTIMIZE FOR (
@typ2 = 1024,
@kwal2 = -99,
@kierId = -99,
@filter2 = -99,
@status2 = ''p'',
@sub = 1
))
*/
'

--select @stmt1 + @stmt2
declare @stmt nvarchar(max) 
set @stmt = @stmt1 + @stmt2 + @stmt3
exec sp_executesql @stmt   

--select @t, GETDATE(), DATEDIFF(MS, @t, GETDATE())
--select @stmt
" 
        onselected="SqlDataSource1_Selected" onselecting="SqlDataSource1_Selecting">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidTyp" Name="typ" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidFilter" Name="filter" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidKwal" Name="kwal" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidData" Name="data" PropertyName="Value" Type="DateTime"/>
        <asp:ControlParameter ControlID="hidMonit" Name="monit" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="ddlStatus" Name="status" PropertyName="SelectedValue" Type="String"/>
        <asp:ControlParameter ControlID="ddlCC" Name="cc" PropertyName="SelectedValue" Type="String" DefaultValue="-99"/>
        <asp:ControlParameter ControlID="cbWithOnly" Name="withonly" PropertyName="Checked" Type="Boolean"/>
        <asp:ControlParameter ControlID="cbPassSpaw" Name="spawonly" PropertyName="Checked" Type="Boolean"/>
        <asp:ControlParameter ControlID="ddlKier" Name="kierId" PropertyName="SelectedValue" Type="Int32"/>
        <asp:ControlParameter ControlID="cbShowSub" Name="sub" PropertyName="Checked" Type="Boolean"/>

        <asp:ControlParameter ControlID="ddlFirma" Name="firma" PropertyName="SelectedValue" Type="String"/>
        
        
        <asp:ControlParameter ControlID="hidGrupa" Name="grupa" PropertyName="Value" Type="Int32" DefaultValue="-99" />
        <asp:ControlParameter ControlID="hidPracownicyTyp" Name="pracownicytyp" PropertyName="Value" Type="String" DefaultValue="DEFAULT"/>

    </SelectParameters>
</asp:SqlDataSource>

<%--
        <asp:ControlParameter ControlID="hidShowStrKier" Name="showK" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidStrOrg" Name="strorg" PropertyName="Value" Type="String"/>
        <asp:ControlParameter ControlID="hidStatus" Name="status" PropertyName="Value" Type="String"/>
        <asp:ControlParameter ControlID="hidKierId" Name="kierId" PropertyName="Value" Type="Int32"/>
--%>
<%--
1 - wersja sumujaca certyfikaty
2 - sumowanie wg daty PoziomPoziom - wymaga zmiany w Zoomach!!!
3 - testy (nieudane)
    /* --3
	select 
	ISNULL(SUM(case when DataWaznosci2 is null or DataWaznosci2 &gt;= @data2 then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when DataWaznosci2 between @data2 and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when DataWaznosci2 &lt; @data2 then 1 else 0 end), 0) [Przeterm],  
	ISNULL(@ucount2 - count(*), 0) [Brak]  
	from 
	(
	select distinct DataWaznosci2
	from VCertyfikatyUprawnienieMS2 where IdPracownika = PV.IdPracownika and Aktualny = 1 and IdUprawnienia in (' + @uprList + ')
	) D
	*/
--%>


<%--
where ' +
case when @strorg is null then '' else 'P.Id_Str_OrgM in (' + @strorg + ') and ' end
+ ' 
--%>


<asp:SqlDataSource ID="SqlDataSourceHeader" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    onselected="SqlDataSource2_Selected"
    SelectCommand="
select 
    case when @lang = 'PL' then Nazwa else NazwaEN end as Nazwa, 
    case when @lang = 'PL' then Poziom else PoziomEN end as Poziom, 
    * 
from Uprawnienia 
where (@typ = -99 or not Typ & @typ = 0) and (@kwal = -99 or KwalifikacjeId = @kwal) and (@grupa = -99 or IdGrupy = @grupa) and Grupa = 0 and Aktywne = 1
order by Kolejnosc, 2, Symbol, 1
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidTyp" Name="typ" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidFilter" Name="filter" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidKwal" Name="kwal" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidData" Name="data" PropertyName="Value" Type="DateTime"/>
        <asp:ControlParameter ControlID="hidMonit" Name="monit" PropertyName="Value" Type="Int32"/>
        <asp:SessionParameter DefaultValue="PL" Name="lang" SessionField="LNG" Type="String" />
        
        <asp:ControlParameter ControlID="hidGrupa" Name="grupa" PropertyName="Value" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceSum" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="SqlDataSourceSum_Selected"
    SelectCommand="
--declare @monit int = 30
--declare @data datetime = GETDATE()
--declare @status2 varchar(10) = 'p'
--declare @kierId int = -99
--declare @typ int = -99
--declare @kwal int = -99
--declare @sub bit = 0
--declare @lang varchar(20) = 'PL'

declare @dataW datetime
set @dataW = DATEADD(D, @monit, @data)

select U.Id, 
	ISNULL(SUM(case when C.Id is not null and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when C.DataWaznosci between @data and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when C.DataWaznosci &lt; @data then 1 else 0 end), 0) [Przeterm],
	ISNULL(SUM(case when C.Id is null then 1 else 0 end), 0) [Brak],
	U.KwalifikacjeId, U.PoziomId, U.PoziomPoziom
from Uprawnienia U
left join Pracownicy P on P.Status != -2 and P.KadryId between 0 and 80000-1 -- CETOR

--left join PodzialLudziImport I on I.KadryId = P.KadryId and @data between I.OkresOd and ISNULL(I.OkresDo,'20990909')
left join PodzialLudziImport IX on IX.KadryId = P.KadryId and @data between IX.OkresOd and ISNULL(IX.OkresDo,'20990909')
left join PlanUrlopowPomin PU on PU.IdPracownika = P.Id and @data between PU.Od and ISNULL(PU.Do,'20990909')
left join Kody PUK on PUK.Typ = 'ABSDL' and PUK.Kod = PU.PowodKod
--outer apply (select IX.DataZwolStatus) I1
outer apply (select 
	case when PU.Id is null then IX.DataZwolStatus 
	else ISNULL(PUK.Nazwa + ' - ' + PUK.Nazwa2 + ISNULL(' (' + PU.Powod + ')', ''), ' ' + ISNULL(PU.Powod, 'absencja długotrwała')) --spacja żeby nie A!
	end as DataZwolStatus) I



--left join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @data and Status = 1 order by Od desc) R
--left join Certyfikaty C on C.IdUprawnienia = U.Id and C.IdPracownika = P.Id and C.Aktualny = 1
outer apply (select top 1 * from VCertyfikatyUprawnienieMS   --najnowsze z tego samego lub wyższego poziomu
			where IdPracownika = P.Id and Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId 
			and (U.PoziomId is null and UprId = U.Id or PoziomId = U.PoziomId and PoziomPoziom &gt;= U.PoziomPoziom) and Aktualny = 1 order by DataWaznosci desc) C
where (@typ = -99 or not U.Typ & @typ = 0) and (@kwal = -99 or U.KwalifikacjeId = @kwal)  and (@grupa = -99 or U.IdGrupy = @grupa) and U.Grupa = 0 and U.Aktywne = 1

and (
    @status2 = 'p'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data or	--zatrudnieni
    @status2 = 'pa' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and LEFT(ISNULL(I.DataZwolStatus,'A'),1) in ('A','2') or	--zatrudnieni
    @status2 = 'k'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and P.Kierownik = 1 or	--kierownicy
    --@status2 = 'n' and (P.Status = 1 or P.Status = 0 and P.DataZwol is null) or			--nowi/nieprzypisani
    @status2 = 'n'  and (P.Status = 1 or P.Status = 0 and (R.Id is null or ISNULL(R.Do,'20990909') &lt; @data)) or			--nowi/nieprzypisani
    @status2 = 'z'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &lt; @data or	--zwolnieni
	@status2 = 'all' and P.Status != -2		--wszyscy, oprócz pomijanych
	)
and (
    @kierId = -99 or
	@sub = 0 and R.IdKierownika = @kierId or
	@sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 1, @data) where Kierownik = 1)
    )    
and (@cc = -99 or R.Id in (select IdPrzypisania from SplityWspP where IdCC = @cc))
group by U.Id, U.Kolejnosc, U.Poziom, U.PoziomEN, U.symbol, U.Nazwa, u.NazwaEN, U.KwalifikacjeId, U.PoziomId, U.PoziomPoziom, U.IdGrupy
order by 
	U.Kolejnosc, 
    case when @lang = 'PL' then U.Poziom else U.PoziomEN end, 
	U.Symbol, 
    case when @lang = 'PL' then U.Nazwa else U.NazwaEN end
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidTyp" Name="typ" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidFilter" Name="filter" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidKwal" Name="kwal" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidData" Name="data" PropertyName="Value" Type="DateTime"/>
        <asp:ControlParameter ControlID="hidMonit" Name="monit" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="ddlStatus" Name="status2" PropertyName="SelectedValue" Type="String"/>
        <asp:ControlParameter ControlID="ddlCC" Name="cc" PropertyName="SelectedValue" Type="Int32" DefaultValue="-99" />
        <asp:ControlParameter ControlID="ddlKier" Name="kierId" PropertyName="SelectedValue" Type="Int32"/>
        <asp:ControlParameter ControlID="cbShowSub" Name="sub" PropertyName="Checked" Type="Boolean"/>
        <asp:SessionParameter DefaultValue="PL" Name="lang" SessionField="LNG" Type="String" />
        
        <asp:ControlParameter ControlID="hidGrupa" Name="grupa" PropertyName="Value" Type="Int32" DefaultValue="-99"/>
    </SelectParameters>
</asp:SqlDataSource>
<%--
    
    
    
--declare @monit int = 30
--declare @data datetime = GETDATE()
--declare @status2 varchar(10) = 'p'
--declare @kierId int = -99
--declare @typ int = -99
--declare @kwal int = -99
--declare @sub bit = 0
--declare @lang varchar(20) = 'PL'

declare @dataW datetime
set @dataW = DATEADD(D, @monit, @data)
select U.Id, 
	ISNULL(SUM(case when C.Id is not null and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when C.DataWaznosci between @data and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when C.DataWaznosci &lt; @data then 1 else 0 end), 0) [Przeterm],
	ISNULL(SUM(case when C.Id is null then 1 else 0 end), 0) [Brak]
from Uprawnienia U
left join Pracownicy P on P.Status != -2 and P.KadryId &lt; 80000 -- CETOR
--left join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @data and Status = 1 order by Od desc) R
left join Certyfikaty C on C.IdUprawnienia = U.Id and C.IdPracownika = P.Id and C.Aktualny = 1
where (@typ = -99 or not U.Typ & @typ = 0) and (@kwal = -99 or U.KwalifikacjeId = @kwal) and U.Grupa = 0 and U.Aktywne = 1

and (
    @status2 = 'p' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data or	--zatrudnieni
    @status2 = 'k' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and P.Kierownik = 1 or	--kierownicy
    @status2 = 'n' and (P.Status = 1 or P.Status = 0 and P.DataZwol is null) or			--nowi/nieprzypisani
    @status2 = 'z' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &lt; @data or	--zwolnieni
	@status2 = 'all' and P.Status != -2		--wszyscy, oprócz pomijanych

    /*
    @status2 = 'p' and P.Status &gt;= 0 or	--zatrudnieni
    @status2 = 'n' and P.Status &gt;= 0 or	--nowi/nieprzypisani
    @status2 = 'z' and P.Status = -1 or	--zwolnieni
	@status2 = 'all' and P.Status != -2	--wszyscy, oprócz pomijanych
	*/
    )
and (
    @kierId = -99 or
	@sub = 0 and R.IdKierownika = @kierId or
	@sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 1, @data) where Kierownik = 1)
    )
    
group by U.Id, U.Kolejnosc, U.Poziom, U.PoziomEN, U.symbol, U.Nazwa, u.NazwaEN
order by 
	U.Kolejnosc, 
    case when @lang = 'PL' then Poziom else PoziomEN end, 
	U.Symbol, 
    case when @lang = 'PL' then Nazwa else NazwaEN end



        <asp:ControlParameter ControlID="hidStatus" Name="status2" PropertyName="Value" Type="String"/>
        <asp:ControlParameter ControlID="hidKierId" Name="kierId" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidStrOrg" Name="strorg" PropertyName="Value" Type="String"/>
--%>

<asp:SqlDataSource ID="SqlDataSourceTotal" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="SqlDataSourceTotal_Selected"
    SelectCommand="
--declare @monit int = 30
--declare @data datetime = GETDATE()
--declare @status2 varchar(10) = 'p'
--declare @kierId int = -99
--declare @typ int = -99
--declare @kwal int = -99
--declare @sub bit = 0
--declare @lang varchar(20) = 'PL'

declare @dataW datetime 
set @dataW = DATEADD(D, @monit, @data)

select  
	ISNULL(SUM(case when C.Id is not null and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when C.DataWaznosci between @data and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when C.DataWaznosci &lt; @data then 1 else 0 end), 0) [Przeterm]
	--,ISNULL(SUM(case when C.Id is null then 1 else 0 end), 0) [Brak]
from (
	select distinct 
	ISNULL(U2.Id, U.Id) as Id,
	U.Typ, U.KwalifikacjeId, U.PoziomId, 
	ISNULL(U2.Grupa, U.Grupa) as Grupa,   
	ISNULL(U2.PoziomPoziom, U.PoziomPoziom) as PoziomPoziom,
	U.Aktywne,
	U.IdGrupy
	from Uprawnienia U
	outer apply (select top 1 * from Uprawnienia 
				where Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId
				and (U.PoziomId is null and Id = U.Id or PoziomId = U.PoziomId) and Aktywne = 1 order by PoziomPoziom asc) U2
	where U.Aktywne = 1
) U 
left join Pracownicy P on P.Status != -2 and P.KadryId between 0 and 80000-1 -- CETOR

--left join PodzialLudziImport I on I.KadryId = P.KadryId and @data between I.OkresOd and ISNULL(I.OkresDo,'20990909')
left join PodzialLudziImport IX on IX.KadryId = P.KadryId and @data between IX.OkresOd and ISNULL(IX.OkresDo,'20990909')
left join PlanUrlopowPomin PU on PU.IdPracownika = P.Id and @data between PU.Od and ISNULL(PU.Do,'20990909')
left join Kody PUK on PUK.Typ = 'ABSDL' and PUK.Kod = PU.PowodKod
--outer apply (select IX.DataZwolStatus) I1
outer apply (select 
	case when PU.Id is null then IX.DataZwolStatus 
	else ISNULL(PUK.Nazwa + ' - ' + PUK.Nazwa2 + ISNULL(' (' + PU.Powod + ')', ''), ' ' + ISNULL(PU.Powod, 'absencja długotrwała')) --spacja żeby nie A!
	end as DataZwolStatus) I



--left join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @data and Status = 1 order by Od desc) R
--left join Certyfikaty C on C.IdUprawnienia = U.Id and C.IdPracownika = P.Id and C.Aktualny = 1
outer apply (select top 1 * from VCertyfikatyUprawnienieMS   --najnowsze z tego samego lub wyższego poziomu
			where IdPracownika = P.Id and Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId
			and (U.PoziomId is null and UprId = U.Id or PoziomId = U.PoziomId and PoziomPoziom &gt;= U.PoziomPoziom) and Aktualny = 1 order by DataWaznosci desc) C
where (@typ = -99 or not U.Typ & @typ = 0) and (@kwal = -99 or U.KwalifikacjeId = @kwal) and (@grupa = -99 or U.IdGrupy = @grupa) and U.Grupa = 0 and U.Aktywne = 1
and (
    @status2 = 'p'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data or	--zatrudnieni
    @status2 = 'pa' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and LEFT(ISNULL(I.DataZwolStatus,'A'),1) in ('A','2') or	--zatrudnieni
    @status2 = 'k'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and Kierownik = 1 or	--zatrudnieni
    --@status2 = 'n' and (P.Status = 1 or P.Status = 0 and P.DataZwol is null) or			--nowi/nieprzypisani
    @status2 = 'n'  and (P.Status = 1 or P.Status = 0 and (R.Id is null or ISNULL(R.Do,'20990909') &lt; @data)) or			--nowi/nieprzypisani
    @status2 = 'z'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &lt; @data or	--zwolnieni
	@status2 = 'all' and P.Status != -2		--wszyscy, oprócz pomijanych
    )
and (
    @kierId = -99 or
	@sub = 0 and R.IdKierownika = @kierId or
	@sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 1, GETDATE()) where Kierownik = 1)
    )
and (@cc = -99 or R.Id in (select IdPrzypisania from SplityWspP where IdCC = @cc))
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidTyp" Name="typ" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidFilter" Name="filter" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidKwal" Name="kwal" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidData" Name="data" PropertyName="Value" Type="DateTime"/>
        <asp:ControlParameter ControlID="hidMonit" Name="monit" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="ddlStatus" Name="status2" PropertyName="SelectedValue" Type="String"/>
        <asp:ControlParameter ControlID="ddlCC" Name="cc" PropertyName="SelectedValue" Type="String" DefaultValue="-99"/>
        <asp:ControlParameter ControlID="ddlKier" Name="kierId" PropertyName="SelectedValue" Type="Int32"/>
        <asp:ControlParameter ControlID="cbShowSub" Name="sub" PropertyName="Checked" Type="Boolean"/>
        
        <asp:ControlParameter ControlID="hidGrupa" Name="grupa" PropertyName="Value" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>

<%--

    
    
    
--declare @monit int = 30
--declare @data datetime = GETDATE()
--declare @status2 varchar(10) = 'p'
--declare @kierId int = -99
--declare @typ int = -99
--declare @kwal int = -99
--declare @sub bit = 0
--declare @lang varchar(20) = 'PL'

declare @dataW datetime 
set @dataW = DATEADD(D, @monit, @data)
select 
	ISNULL(SUM(case when C.Id is not null and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when C.DataWaznosci between @data and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when C.DataWaznosci &lt; @data then 1 else 0 end), 0) [Przeterm]
	--,ISNULL(SUM(case when C.Id is null then 1 else 0 end), 0) [Brak]
from Uprawnienia U 
left join Pracownicy P on P.Status != -2 and P.KadryId &lt; 80000 -- CETOR
--left join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @data and Status = 1 order by Od desc) R
--left join Certyfikaty C on C.IdUprawnienia = U.Id and C.IdPracownika = P.Id and C.Aktualny = 1
outer apply (select top 1 * from VCertyfikatyUprawnienieMS   --najnowsze z tego samego lub wyższego poziomu
			where IdPracownika = P.Id and Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId 
			and (U.PoziomId is null and UprId = U.Id or PoziomId = U.PoziomId and PoziomPoziom &gt;= U.PoziomPoziom) and Aktualny = 1 order by DataWaznosci desc) C
where (@typ = -99 or not U.Typ & @typ = 0) and (@kwal = -99 or U.KwalifikacjeId = @kwal) and U.Grupa = 0 and U.Aktywne = 1
and (
    @status2 = 'p' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data or	--zatrudnieni
    @status2 = 'k' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and Kierownik = 1 or	--zatrudnieni
    @status2 = 'n' and (P.Status = 1 or P.Status = 0 and P.DataZwol is null) or			--nowi/nieprzypisani
    @status2 = 'z' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &lt; @data or	--zwolnieni
	@status2 = 'all' and P.Status != -2		--wszyscy, oprócz pomijanych
    )
and (
    @kierId = -99 or
	@sub = 0 and R.IdKierownika = @kierId or
	@sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 1, GETDATE()) where Kierownik = 1)
    )



        <asp:ControlParameter ControlID="hidStatus" Name="status2" PropertyName="Value" Type="String"/>
        <asp:ControlParameter ControlID="hidStrOrg" Name="strorg" PropertyName="Value" Type="String"/>
        <asp:ControlParameter ControlID="hidKierId" Name="kierId" PropertyName="Value" Type="Int32"/>
--%>


<%--
/*
select * from ASSECO..lp_vv_KursyExt
where DataRozpoczecia != DataZakonczenia

delete from Certyfikaty where Id <= 0
select * from Certyfikaty
alter table Certyfikaty add DataRozpoczecia datetime
alter table Certyfikaty add DataZakonczenia datetime
alter table Certyfikaty add Nazwa nvarchar(200)
alter table Certyfikaty add DodatkoweWarunki nvarchar(200)

alter table Uprawnienia add Wygasa nvarchar(200)

select * from ASSECO..lp_vv_KursyExt
select distinct Uwagi from ASSECO..lp_vv_KursyExt
select distinct Opis from ASSECO..lp_vv_KursyExt

select * from ASSECO..lp_vv_KursyExt where 
KursTyp = 'BHP' and 
(Opis is not null or Certyfikat is not null or DataCertyfikatu is not null or NumerZaswiadczenia is not null or DodatkoweWarunki is not null )

select * from Certyfikaty
select * from ASSECO..lp_vv_KursyExt where Uwagi is not null and KursTyp = 'BHP'

*/


/*
drop table #aaa
drop table #uuu
drop table #kkk
delete from Uprawnienia
select * from Uprawnienia
select * from Certyfikaty order by Id
select * from Certyfikaty order by Id desc
select * from Certyfikaty where Aktualny = 1
select * from Certyfikaty where Aktualny = 0
select * from Uprawnienia_Asseco
select * from ASSECO..lp_vv_KursyDefinicjeExt
select * from ASSECO..lp_vv_KursyExt


update Uprawnienia set Nazwa = 'asdf' where Id = 1000082951
update Uprawnienia set Aktywne = 0 where Id = 1000082951
delete from Uprawnienia where Id = 1000082951
update Uprawnienia set Nazwa = 'asdf' where Id = 1000082950

BEGIN TRANSACTION IMPORT_BHP
BEGIN TRY
    COMMIT TRANSACTION IMPORT_BHP
END TRY
BEGIN CATCH
  ROLLBACK TRANSACTION IMPORT_BHP
END CATCH 


DBCC CHECKIDENT ('Certyfikaty', RESEED, -1000) 
DBCC CHECKIDENT ('Certyfikaty', RESEED, 0) 


SELECT CHECKSUM_AGG(CHECKSUM(*)) FROM Pracownicy
SELECT CHECKSUM_AGG(CHECKSUM(*)) FROM ASSECO..lp_vv_KursyExt where KursTyp = 'BHP'



update Pracownicy set Nazwisko = 'Wujciów' where Id = 1
update Pracownicy set Nazwisko = 'Wujciów1' where Id = 1
1863028365

-622262685
-622262685
-83312499
-83312499

*/
--%>
<asp:SqlDataSource ID="AssecoImportSql" runat="server"
    SelectCommand="
/*
drop table #kkk
drop table #kka
drop table #aaa
drop table #uuu
select * from Certyfikaty
select * from Certyfikaty where Id &lt; 0
select * from Certyfikaty where Id = 10001
select * from Certyfikaty where Aktualny = 1
select * from Certyfikaty where Aktualny = 0
*/
-------------------------------
-- PARAMETRY
-------------------------------
declare @nadzien datetime
declare @logIdmain nvarchar(200)
declare @logTyp2 int
declare @parentId int
declare @logId nvarchar(200)
declare @OK nvarchar(20)
declare @ERROR nvarchar(20)
declare @c1 int, @c2 int, @c3 int
declare @e1 bit, @e2 bit, @e3 bit

declare @mode int
--set @mode = 1  -- delete + insert
set @mode = 2  -- update, insert, delete

set @nadzien = dbo.getdate(GETDATE())
set @logIdmain = 'Asseco.Import.SzkoleniaBHP'
set @logTyp2 = 3010
set @OK = ' - OK'
set @ERROR = ' - Błąd'
set @e1 = 0 -- 
set @e2 = 0	-- 
set @e3 = 0 -- 

/*
 ERROR_NUMBER() AS ErrorNumber
,ERROR_SEVERITY() AS ErrorSeverity
,ERROR_STATE() AS ErrorState
,ERROR_PROCEDURE() AS ErrorProcedure
,ERROR_LINE() AS ErrorLine
,ERROR_MESSAGE() AS ErrorMessage;
*/
-------------------------------
-- 
-------------------------------


-------------------------------
-- START
-------------------------------
declare @BHP varchar(10)
declare @BHPID int
set @BHP = 'BHP'
set @BHPID = 1024

insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),0,0,0,@logTyp2,@logIdmain,'------------------------')
set @parentId = @@IDENTITY

----- UPRAWNIENIA -----------
set @logId = @logIdmain + '.Uprawnienia'
--insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,0,@logTyp2,@logId,'')

-----------------------------
-- tabele tymczasowe
--select * into #aaa from ASSECO..lp_vv_KursyDefinicjeExt with (nolock)
--select * into #kkk from ASSECO..lp_vv_KursyExt with (nolock) where KursTyp = @BHP
select * into #aaa from JGBHR01.sl_hr_jabilgs.dbo.lp_vv_KursyDefinicjeExt with (nolock)
select * into #kkk from JGBHR01.sl_hr_jabilgs.dbo.lp_vv_KursyExt with (nolock) where KursTyp = @BHP

--select *, cast(0 as bit) as Aktualny into #kkk from ASSECO..lp_vv_KursyExt with (nolock) where KursTyp = @BHP
--select *, cast(0 as bit) as Aktualny into #kkk from ASSECO..lp_vv_KursyExt with (nolock) where KursTyp = 'BHP'
--select *, cast(0 as bit) as Aktualny into #kkk from JGBHR01.sl_hr_jabilgs.dbo.lp_vv_KursyExt with (nolock) where KursTyp = @BHP
select * into #uuu from Uprawnienia

BEGIN TRANSACTION @logId
BEGIN TRY
-----------------------------
-- zerowanie aktywności
update Uprawnienia set Aktywne = 0
set @c1 = @@ROWCOUNT

-- aktualizacja --
update Uprawnienia set Nazwa = A.Nazwa, Opis = A.Uwagi, Aktywne = O.Aktywne -- aktywność z kopii, zeby nie nadpisac jak wyłączę
--select * 
from Uprawnienia U 
join #aaa A on A.lp_KursyDefinicjeId = U.Id
join #uuu O on O.Id = U.Id
where A.KursTyp = @BHP
set @c2 = @@ROWCOUNT

-- nowe --
insert into Uprawnienia (Id, Typ, Symbol, Nazwa, NazwaEN, Poziom, PoziomEN, Opis, Pola, Kolejnosc, Aktywne, Produkcyjne, Nieprodukcyjne, PaszportSpawacza, KwalifikacjeId, PoziomId, PoziomPoziom, Grupa)
select A.lp_KursyDefinicjeId, T.Typ, 
A.KursDefinicja, A.Nazwa, A.Nazwa, null, null, A.Uwagi, '0011000000000111111', null, 1,  
0, 0, 0, UK.Id, null, null, 0
from #aaa A
outer apply (select case ISNULL(KursTyp,'') 
	when @BHP then 1024
	when 'BADANIA' then 4096
	when 'INS' then 8192
	when '' then 16384
	else 0
end as Typ) T
outer apply (select top 1 * from UprawnieniaKwalifikacje where Typ = T.Typ order by Kolejnosc) UK
where KursTyp = @BHP
and A.lp_KursyDefinicjeId not in (select Id from Uprawnienia)
set @c3 = @@ROWCOUNT
-----------------------------
	COMMIT TRANSACTION @logId
	insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,0,@logTyp2,@logId + @OK,convert(varchar,@c1) + '/' + convert(varchar,@c2) + '/' + convert(varchar,@c3))
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION @logId
	insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,1,@logTyp2,@logId + @ERROR,convert(varchar, ERROR_NUMBER()) + ISNULL(' - ' + ERROR_MESSAGE(),''))
	set @e1 = 1
END CATCH 

----- CERTYFIKATY -----------
set @logId = @logIdmain + '.Certyfikaty'
--insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,0,@logTyp2,@logId,'')

-- Aktualny
/* aktualizuje certyfikaty
if @mode != 1 begin -- update, insert, delete
select K.lp_KursyId into #kka from #kkk K
outer apply (select top 1 * from #kkk AA where AA.lp_KursyDefinicjeId = K.lp_KursyDefinicjeId and AA.LpLogo = K.LpLogo order by AA.DataWaznosci desc) A
where A.lp_KursyId = K.lp_KursyId

update #kkk set Aktualny = 1 where lp_KursyId in (select lp_KursyId from #kka)
drop table #kka

--!!! nie działa !!! - 3 odwołania do tabeli #, normalnie sql zwraca błąd
--update #kkk set Aktualny = 1
--from #kkk K
--outer apply (select top 1 * from #kkk AA where AA.lp_KursyDefinicjeId = K.lp_KursyDefinicjeId and AA.LpLogo = K.LpLogo order by AA.DataWaznosci desc) A
--where A.lp_KursyId = K.lp_KursyId
end
*/

/*
update #kkk set Aktualny = 0

select K.lp_KursyId, K.Aktualny, A.lp_KursyId, K.*, A.*
from #kkk K
outer apply (select top 1 * from #kkk where lp_KursyDefinicjeId = K.lp_KursyDefinicjeId and LpLogo = K.LpLogo order by DataWaznosci desc) A
where A.lp_KursyId = K.lp_KursyId
*/



BEGIN TRANSACTION @logId
BEGIN TRY
-----------------------------
;
disable trigger Certyfikaty_insert on Certyfikaty;
disable trigger Certyfikaty_delete on Certyfikaty;
disable trigger Certyfikaty_update on Certyfikaty;

if @mode = 1 begin -- delete + insert

-- 20151019 - pierwsza wersja 
delete from Certyfikaty where Id &gt; 0
set @c1 = @@ROWCOUNT

SET IDENTITY_INSERT Certyfikaty ON 
insert into Certyfikaty
--(Id,IdUprawnienia,IdPracownika,Numer,DataWaznosci,Kategoria,DataZdobyciaUprawnien,DataWaznosciPsychotestow,DataWaznosciBadanLekarskich,DataWaznosciUmowy,UmowaLojalnosciowa,ImportId,DataZdobyciaUprawnienOk,DataWaznosciPsychotestowOk,DataWaznosciBadanLekarskichOk,UmowaLojalnosciowaOk,DataWaznosciSet,Aktualny,Uwagi)
  (Id,IdUprawnienia,IdPracownika,Numer,DataWaznosci,Kategoria,DataZdobyciaUprawnien,DataWaznosciPsychotestow,DataWaznosciBadanLekarskich,DataWaznosciUmowy,UmowaLojalnosciowa,Aktualny,Uwagi,
  DataRozpoczecia,DataZakonczenia,NazwaCertyfikatu,DodatkoweWarunki)
select distinct 
K.lp_KursyId as Id,
K.lp_KursyDefinicjeId as IdUprawnienia,
P.Id as IdPracownika,	
K.NumerZaswiadczenia as Numer,
K.DataWaznosci as DataWaznosci,
null as Kategoria,	
K.DataRozpoczecia as DataZdobyciaUprawnien,	
null as DataWaznosciPsychotestow,	
null as DataWaznosciBadanLekarskich,	
null as DataWaznosciUmowy,	
0 as UmowaLojalnosciowa,	
--null as ImportId,	
--null as DataZdobyciaUprawnienOk,	
--null as DataWaznosciPsychotestowOk,	
--null as DataWaznosciBadanLekarskichOk,	
--null as UmowaLojalnosciowaOk,	
--null as DataWaznosciSet,	
--cast(0 as bit) as Aktualny,
cast(case when A.lp_KursyId = K.lp_KursyId then 1 else 0 end as bit) as Aktualny,	
K.Uwagi as Uwagi,
K.DataRozpoczecia, K.DataZakonczenia, K.Nazwa, K.DodatkoweWarunki
from #kkk K
inner join Pracownicy P on P.KadryId = K.LpLogo
outer apply (select top 1 * from #kkk where lp_KursyDefinicjeId = K.lp_KursyDefinicjeId and LpLogo = K.LpLogo order by DataWaznosci desc) A

set @c2 = @@ROWCOUNT

SET IDENTITY_INSERT Certyfikaty OFF;

end
else begin
-- update 
update Certyfikaty set 
IdUprawnienia = K.lp_KursyDefinicjeId,
IdPracownika = P.Id,
Numer = K.NumerZaswiadczenia,
DataWaznosci = K.DataWaznosci,
DataZdobyciaUprawnien = K.DataRozpoczecia,
--Aktualny = K.Aktualny,
Uwagi = K.Uwagi,
DataRozpoczecia = K.DataRozpoczecia,
DataZakonczenia = K.DataZakonczenia,
NazwaCertyfikatu = K.Nazwa,
DodatkoweWarunki = K.DodatkoweWarunki,
DataModyfikacji = GETDATE()
--select * 
from Certyfikaty C 
left join #kkk K on K.lp_KursyId = C.Id
inner join Pracownicy P on P.KadryId = K.LpLogo
where 
   C.IdUprawnienia != K.lp_KursyDefinicjeId
or C.IdPracownika != P.Id
or ISNULL(C.Numer,'') != ISNULL(K.NumerZaswiadczenia,'')
or ISNULL(C.DataWaznosci,'') != ISNULL(K.DataWaznosci,'')
or ISNULL(C.DataZdobyciaUprawnien,'') != ISNULL(K.DataRozpoczecia,'')
or ISNULL(C.Uwagi,'') != ISNULL(K.Uwagi,'')
or ISNULL(C.DataRozpoczecia,'') != ISNULL(K.DataRozpoczecia,'')
or ISNULL(C.DataZakonczenia,'') != ISNULL(K.DataZakonczenia,'')
or ISNULL(C.NazwaCertyfikatu,'') != ISNULL(K.Nazwa,'')
or ISNULL(C.DodatkoweWarunki,'') != ISNULL(K.DodatkoweWarunki,'')
--or C.Aktualny != K.Aktualny




/*
--select * from Certyfikaty
--select * from Uprawnienia
--select * from #kkk where Aktualny = 1
--select * from Certyfikaty where Id = 1004695430
--select * from #kkk where lp_KursyId = 1004695430
--select * from Certyfikaty where Id = 1022011699
--select * from #kkk where lp_KursyId = 1022011699
declare @id int = 1006804309
select Id, IdUprawnienia, IdPracownika, Numer, DataWaznosci, DataZdobyciaUprawnien, Aktualny, Uwagi, DataRozpoczecia, DataZakonczenia, NazwaCertyfikatu, DodatkoweWarunki from Certyfikaty where Id = @id
select K.lp_KursyId as Id,
K.lp_KursyDefinicjeId as IdUprawnienia,
P.Id as IdPracownika,
K.NumerZaswiadczenia as Numer,
K.DataWaznosci as DataWaznosci,
K.DataRozpoczecia as DataZdobyciaUprawnien,
K.Aktualny,
K.Uwagi,
K.DataRozpoczecia,
K.DataZakonczenia,
K.Nazwa as NazwaCertyfikatu,
K.DodatkoweWarunki
from #kkk K 
inner join Pracownicy P on P.KadryId = K.LpLogo
where lp_KursyId = @id
--select * from #kkk where lp_KursyId = @id
*/




set @c1 = @@ROWCOUNT

-- insert
SET IDENTITY_INSERT Certyfikaty ON 
insert into Certyfikaty (Id, IdUprawnienia, IdPracownika, Numer, DataWaznosci, DataZdobyciaUprawnien, Aktualny, Uwagi, DataRozpoczecia, DataZakonczenia, NazwaCertyfikatu, DodatkoweWarunki, DataModyfikacji)
select 
K.lp_KursyId,
K.lp_KursyDefinicjeId,
P.Id,
K.NumerZaswiadczenia,
K.DataWaznosci,
K.DataRozpoczecia,
0,
K.Uwagi,
K.DataRozpoczecia,
K.DataZakonczenia,
K.Nazwa,
K.DodatkoweWarunki,
GETDATE()
from #kkk K
inner join Pracownicy P on P.KadryId = K.LpLogo
where K.lp_KursyId not in (select Id from Certyfikaty)

set @c2 = @@ROWCOUNT

SET IDENTITY_INSERT Certyfikaty OFF

--delete
delete
--select * 
from Certyfikaty where Id &gt; 0 and Id not in (
select lp_KursyId
from #kkk K
inner join Pracownicy P on P.KadryId = K.LpLogo
)

set @c3 = @@ROWCOUNT

--aktualizacja Aktualny
select C.Id 
into #cca 
from Certyfikaty C
outer apply (select top 1 * from Certyfikaty where IdUprawnienia = C.IdUprawnienia and IdPracownika = C.IdPracownika order by DataWaznosci desc) A
where A.Id = C.Id

update Certyfikaty set Aktualny = case when (A.Id is null or C.DataZdobyciaUprawnien &lt; DATEADD(D,-7,P.DataZatr)) then 0 else 1 end -- tylko z bieżącego zatrudnienia
from Certyfikaty C
left join Pracownicy P on P.Id = C.IdPracownika
left join #cca A on A.Id = C.Id

drop table #cca

end -- mode = 2
;
enable trigger Certyfikaty_update on Certyfikaty;
enable trigger Certyfikaty_delete on Certyfikaty;
enable trigger Certyfikaty_insert on Certyfikaty;
-----------------------------
	COMMIT TRANSACTION @logId
	insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,0,@logTyp2,@logId + @OK,convert(varchar,@c1) + '/' + convert(varchar,@c2) + '/' + convert(varchar,@c3))
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION @logId
	insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,1,@logTyp2,@logId + @ERROR,convert(varchar, ERROR_NUMBER()) + ISNULL(' - ' + ERROR_MESSAGE(),''))
	set @e2 = 1
END CATCH 

-- sprzątanie --
drop table #aaa
drop table #uuu
drop table #kkk

-------------------------------
-- ZAPIS LOG 
-------------------------------
if @e1 = 0 and @e2 = 0 and @e3 = 0
	insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,0,310,@logIdmain + @OK,'------------------------')
else
	insert into Log (DataCzas,ParentId,Status,Typ,Typ2,Info,Info2) values (GETDATE(),@parentId,0,1,310,@logIdmain + @ERROR,'------------------------')
-------------------------------
-- KONIEC 
-------------------------------
--select top 100 * from Log order by 1 desc
--drop table #kkk
--drop table #aaa
--delete from Certyfikaty
    ">
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsCertyfikatTitle" runat="server" SelectCommand=
    "
declare @pracId int = {0}
declare @uprId int = {1}

declare @prac nvarchar(200) = (select Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') from Pracownicy where Id = @pracId)
declare @upr nvarchar(200) = (select Nazwa from Uprawnienia where Id = @uprId)
select  @prac + ' - ' + @upr
    " />



<asp:SqlDataSource ID="dsKartaVisible" runat="server" SelectCommand="select case when u.Typ = 2048 and uk.NazwaEN = 'STAN' then 1 else 0 end from Uprawnienia u left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId where u.Id = 690" />
