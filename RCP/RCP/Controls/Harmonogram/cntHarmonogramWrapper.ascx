<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntHarmonogramWrapper.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntHarmonogramWrapper" %>
<%@ Register Src="~/Controls/RepHeader.ascx" TagName="RepHeader" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>
<%@ Register Src="~/Controls/SelectOkres.ascx" TagName="SelectOkres" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntZmianySelect.ascx" TagName="SelectZmiana" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntPlanPracy2.ascx" TagName="PlanPracy" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntSchematy.ascx" TagPrefix="cc" TagName="cntSchematy" %>
<%@ Register Src="~/RCP/Controls/Adm/cntSchematy.ascx" TagPrefix="uc1" TagName="cntSchematy" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="cc" TagName="cntModal" %>
<%@ Register Src="~/Controls/ZmianyControl3.ascx" TagPrefix="cc" TagName="ZmianyControl3" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntHarmonogram.ascx" TagPrefix="cc" TagName="cntHarmonogram" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntZmiany.ascx" TagPrefix="cc" TagName="cntZmiany" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntParametryPracownika.ascx" TagPrefix="cc" TagName="cntParametryPracownika" %>
<%@ Register Src="~/RCP/Controls/Adm/cntZmianyList.ascx" TagPrefix="cc" TagName="cntZmianyList" %>
<%@ Register Src="~/RCP/Controls/Adm/cntZmianyEditModal.ascx" TagPrefix="cc" TagName="cntZmianyEditModal" %>


<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Custom" TagPrefix="cc2" %>




<div id="paPlanPracyZmiany" runat="server" class="cntPlanPracyZmiany cntHarmonogramWrapper">
    <asp:HiddenField ID="hidKierId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />

    <div class="paZmiany" runat="server" visible="false">
        <asp:UpdatePanel ID="upBtn" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnAddOkresModal" runat="server" Visible="false" OnClick="btnAddOkresModal_Click" CssClass="btn btn-success" Text="Otwórz okres rozliczeniowy" Style="margin-bottom: 10px; width: 100%;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="paPlan">
        <%-- nie rozdzielać!!! to są inline-block obok siebie --%>

        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
            <ContentTemplate>


                <div class="row menu-box">
                    <%--<div id="divZmiany" runat="server" class="col-md-2" >
                        <cc:cntZmiany runat="server" ID="cntZmiany" Min="1" Max="12" Visible="false"  />
                    </div>--%>
                    <div class="col-md-12" style="position: static;">
                        <div class="zmiany-hd" style="border-radius: 0px;" >
                            <div class="">
                                <div class="filter row">
                                    <div class="col-md-9">
                                    <asp:DropDownList ID="ddlCom" runat="server" CssClass="form-control ddlKier" DataSourceID="dsCom" DataValueField="Value" DataTextField="Text" AutoPostBack="false"
                                        OnSelectedIndexChanged="ddlCom_SelectedIndexChanged" Visible="false" Width="280px" />
                                    <cc2:cntMultiSelect ID="msEntities" runat="server" SelectionMode="Multiple" DataSourceID="dsEntities" DataValueField="Value" Visible="false"
                                        DataTextField="Text" CssClass="" AutoPostBack="false" OnSelectedIndexChanged="msEntities_SelectedIndexChanged" 
                                        DisableIfEmpty="true" NonSelectedText="jednostka ..." ButtonWidth="200px" IncludeSelectAllOption="true"   />

                                    
                                    
                                    
                                    
                                    <asp:SqlDataSource ID="dsCom" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                                        SelectCommand="select null Value, 'jednostka ...' Text, 0 Sort union all select Id Value, Nazwa Text, 1 Sort from Dzialy where Status >= 0 order by Sort, Text" />
                                    <asp:SqlDataSource ID="dsEntities" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                                        SelectCommand="select Id Value, Nazwa Text, 1 Sort from Dzialy where Status >= 0 order by Sort, Text" />



                                    <asp:DropDownList ID="ddlKier" runat="server" CssClass="form-control ddlKier" DataSourceID="dsKier" DataValueField="Value"
                                        DataTextField="Text" AutoPostBack="false" OnSelectedIndexChanged="ddlKier_SelectedIndexChanged" Visible="true" Width="230px" />

                                    <asp:DropDownList ID="ddlDzialy" runat="server" CssClass="form-control ddlKier" DataSourceID="dsDzialy" DataValueField="Value"
                                        DataTextField="Text" AutoPostBack="false" OnSelectedIndexChanged="ddlDzialy_SelectedIndexChanged" Width="230px" />
                                    <asp:SqlDataSource ID="dsDzialy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                                        SelectCommand="select null Value, 'jednostka ...' Text, 0 Sort union all select Id Value, Nazwa Text, 1 Sort from Dzialy where Status >= 0 order by Sort, Text" />

                                    <asp:DropDownList ID="ddlStanowiska" runat="server" CssClass="form-control ddlKier" DataSourceID="dsStanowiska" DataValueField="Value"
                                        DataTextField="Text" AutoPostBack="false" OnSelectedIndexChanged="ddlStanowiska_SelectedIndexChanged" Width="230px" />
                                    <asp:SqlDataSource ID="dsStanowiska" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                                        SelectCommand="select null Value, 'stanowisko ...' Text, 0 Sort union all select Id Value, Nazwa Text, 1 Sort from Stanowiska where Aktywne = 1 order by Sort, Text" />

                                    <asp:DropDownList ID="ddlKlasyfikacja" runat="server" CssClass="form-control ddlKier" DataSourceID="dsKlasyfikacje" DataValueField="Value"
                                        DataTextField="Text" AutoPostBack="false" OnSelectedIndexChanged="ddlKlasyfikacja_SelectedIndexChanged" Width="200px" Visible="false" />
                                    <asp:SqlDataSource ID="dsKlasyfikacje" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                                        SelectCommand="select null Value, 'rodzaj pracownika ...' Text, 0 Sort union all select distinct Klasyfikacja Value, Klasyfikacja Text, 1 Sort from PracownicyStanowiska order by Sort, Text" />


                                        <cc2:cntMultiSelect ID="msKlasyfikacje" runat="server" SelectionMode="Multiple" DataSourceID="dsKlasyfikacje2" DataValueField="Value" Visible="true"
                                            DataTextField="Text" CssClass="" AutoPostBack="false" OnSelectedIndexChanged="msKlasyfikacje_SelectedIndexChanged"
                                            DisableIfEmpty="true" NonSelectedText="klasyfikacja ..." ButtonWidth="200px" IncludeSelectAllOption="false" />

                                        <asp:SqlDataSource ID="dsKlasyfikacje2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                                            SelectCommand="
                                                --select null Value, 'rodzaj pracownika ...' Text, 0 Sort union all 
                                                select distinct Klasyfikacja Value, Klasyfikacja Text, 1 Sort from PracownicyStanowiska union all 
                                                select 'all' Value, 'Wszystkie APT' Text, 2 Sort    
                                                order by Sort, Text" />


                                        <asp:LinkButton ID="btnFilter" runat="server" CssClass="btn btn-default btn-search pull-left" Visible="true" OnClick="btnFilter_Click" ToolTip="Wyszukaj...">
                                            <i class="fa fa-search"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnClearFilter" runat="server" CssClass="btn btn-default btn-search-clear pull-left" style="margin-left: 4px;" Visible="true" OnClick="btnClearFilter_Click" ToolTip="Czyść...">
                                            <i class="glyphicon glyphicon-erase"></i>
                                        </asp:LinkButton>

                                    </div>
                                    <div id="paSearch" runat="server" class="xsearch m-search col-md-3 pull-right">
                                        <div class="input-group search-group">
                                            <%--<asp:Label ID="Label1" runat="server" Text="Wyszukaj pracownika:" Visible="false"></asp:Label>--%>
                                               
                                        <div class="mode-container">
                                            <i id="iModePreview" runat="server" class="glyphicon glyphicon-eye-open text-primary mode-icon xhidden" title="Tryb podglądu" data-toggle="tooltip"></i>
                                            <i id="iModeEdit" runat="server" class="glyphicon glyphicon-pencil text-success mode-icon xhidden" title="Tryb edycji" data-toggle="tooltip" visible="false"></i>
                                        </div>
                                            <asp:Button ID="btSearch" runat="server" Text="Wyszukaj" CssClass="button_postback" OnClick="btSearch_Click" />
                                            <asp:TextBox ID="tbSearch" runat="server" CssClass="form-control tb-search" MaxLength="250" Placeholder="Wyszukaj pracownika ..." Width="300px"></asp:TextBox>
                                            <div class="input-group-btn">
                                                

                                                <asp:LinkButton ID="btnSearchX" runat="server" CssClass="btn btn-default btn-search" Visible="true" OnClick="btnSearchX_Click" ToolTip="Wyszukaj..." >
                                                    <i class="fa fa-search"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btClear" runat="server" CssClass="btn btn-default btn-search-clear" Visible="true" OnClick="btClear_Click" ToolTip="Czyść..." >
                                                    <i class="glyphicon glyphicon-erase"></i>
                                                </asp:LinkButton>

                                            </div>

                                        </div>
                                    </div>
                                </div>

                                <div class="row right-toolbox">
                                    <div class="col-md-4 xcolleft">
                                        <div runat="server" visible="false">
                                            <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Plan pracy"></asp:Label>
                                            <asp:Label ID="lbPlanE" runat="server" CssClass="t5" Visible="false" Text="2) Kliknij w dzień i ustaw zmianę:"></asp:Label>
                                        </div>

                                             <cc:cntZmiany runat="server" ID="cntZmiany" Min="1" Max="12" Visible="false"  />
                                    </div>
                                    <div class="col-md-4 colmiddle">
                                        <uc1:SelectOkres ID="cntSelectOkres" StoreInSession="true" ControlID="cntHarmonogram" OnOkresChanged="cntSelectOkres_Changed" runat="server" />

                                    </div>
                                    <div class="col-md-4 colright toolbox">
                                        <div id="divTools" runat="server" class="pull-right">
                                        
                                                <asp:UpdatePanel ID="upTools" runat="server" UpdateMode="Always">
                                                <ContentTemplate>



                                                   <%-- <div class="btn-group">
                                                        <asp:LinkButton ID="btnCheckInfo" runat="server" CssClass="btn btn-default"><i class="fa fa-info-circle"></i></asp:LinkButton>
                                                        <asp:Button ID="btCheckPP" runat="server" CssClass="btn btn-default" OnClick="btCheckPP_Click" Text="Sprawdź" />
                                                    </div>--%>

                                                    <div class="btn-group">
                                                            <button class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" style=""
                                                                aria-expanded="false">
                                                                <i class="fa fa-cog" style="margin-right: 8px;"></i>Narzędzia
                                                                <span class="caret" style="margin-left: 4px;"></span>
                                                            </button>
                                                        
                                                        <ul runat="server" class="dropdown-menu">
                                                            <%--<li class="dropdown-header">Administracja</li>--%>

                                                            <%--<li class="dropdown-header">Narzędzia</li>--%>
                                                            <li id="li2" runat="server">
                                                                <asp:LinkButton ID="btnCopyFromModal" runat="server" CssClass="" Text="Kopiuj z..." OnClick="btnCopyFromModal_Click" /></li>
                                                            <li id="li3" runat="server">
                                                                <asp:LinkButton ID="btSetScheme" runat="server" CssClass="" Text="Ustaw schemat" OnClick="btScheme_Click" /></li>
                                                            <%--<li id="li4" runat="server">
                                                                <asp:LinkButton ID="btCheckPP" runat="server" CssClass="" OnClick="btCheckPP_Click">Sprawdź</asp:LinkButton>
                                                            </li>--%>

                                                            <li role="separator" class="divider"></li>
                                                            <%--<li class="dropdown-header">Wydruki</li>--%>
                                                            <li id="li5" runat="server">
                                                                <asp:LinkButton ID="btnPrintHar" runat="server" CssClass="" Text="Wydrukuj Harmonogram" OnClick="btnPrintHar_Click" />
                                                            </li>
                                                            <li id="liPrintEmptyHar" runat="server" visible="false">
                                                                <asp:LinkButton ID="lbtnPrintEmptyHar" runat="server" CssClass="" Text="Wydrukuj Pusty Harmonogram" OnClick="lbtnPrintEmptyHar_Click" />
                                                            </li>
                                                            <li id="li6" runat="server">
                                                                <asp:LinkButton ID="btnPrintList" runat="server" CssClass="" Text="Wydrukuj Listę obecności" OnClick="btnPrintList_Click" /></li>
                                                            <li role="separator" class="divider"></li>
                                                            <%--<li><asp:LinkButton ID="btnSendToAcc" runat="server" CssClass="button_postback" OnClick="btnSendToAcc_Click" /></li>--%>
                                                            <li id="li7" runat="server">
                                                                <asp:LinkButton ID="btnEditZmiany" runat="server" CssClass="" Text="Edytuj zmiany" OnClick="btnEditZmiany_Click" /></li>
                                                            <li id="li1" runat="server">
                                                                <asp:LinkButton ID="lnkEditSchemesModal" runat="server" CssClass="" Text="Edytuj schematy" OnClick="lnkEditSchemesModal_Click" /></li>

                                                        </ul>
                                                    </div>

                                                    <div class="btn-group">
                                                        
                                                        <%--<asp:LinkButton ID="btnCheckInfo" runat="server" CssClass="btn btn-default"><i class="fa fa-info-circle"></i></asp:LinkButton>--%>
                                                        <asp:Button ID="btCheckPP" runat="server" CssClass="btn btn-default" OnClick="btCheckPP_Click" Text="Sprawdź" />
                                                    </div>
                                                    <div class="btn-group">
                                                        <asp:Button ID="btnSendToAccConfirm" runat="server" CssClass="btn btn-default" Text="Wyślij do akceptacji" OnClick="btnSendToAccConfirm_Click" />
                                                        <asp:Button ID="btnSendToAcc" runat="server" CssClass="button_postback" OnClick="btnSendToAcc_Click" />
                                                        
                                                        <asp:Button ID="btnAccConfirm" runat="server" CssClass="btn btn-default" Text="Zaakceptuj" OnClick="btnAccConfirm_Click" />

                                                        <asp:Button ID="btnAcc" runat="server" CssClass="button_postback" OnClick="btnAcc_Click" Visible="false" />

                                                        <%--<asp:Button ID="btnReject" runat="server" CssClass="button_postback" OnClick="btnReject_Click1" Visible="false" />--%>
                                                        <asp:Button ID="btnRejectConfirm" runat="server" CssClass="btn btn-danger" Text="Odrzuć" OnClick="btnRejectConfirm_Click" Visible="false" />
                                                    </div>



                                                    <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server"></asp:Label>
                                                    
                                                    
                                                    <div class="btn-group">
                                                        <asp:Button ID="btEditPP" runat="server" Text="Edycja" CssClass="btn btn-success" OnClick="btEditPP_Click" />
                                                        <asp:Button ID="btSavePP" runat="server" Text="Zapisz" Visible="false" CssClass="btn btn-success" OnClick="btSavePP_Click" />
                                                        <asp:Button ID="btCancelPP" runat="server" Text="Anuluj" Visible="false" CssClass="btn btn-default" OnClick="btCancelPP_Click" />

                                                        <asp:Button ID="btPrint" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false" />
                                                    </div>



                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <cc:cntHarmonogram runat="server" ID="cntHarmonogram" Editable="true" DaysIndex="7" SumSize="14" OnParametryPracownikaShow="cntHarmonogram_ParametryPracownikaShow"  />

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrintHar" />
                <asp:PostBackTrigger ControlID="btnPrintList" />
                <asp:PostBackTrigger ControlID="lbtnPrintEmptyHar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <%-- parametry pracownika --%>

    <cc:cntParametryPracownika runat="server" ID="cntParametryPracownika" OnSaved="cntParametryPracownika_Saved" />

    <%-- odrzucanie --%>

    <cc:cntModal runat="server" ID="cntRejectModal" Title="Odrzuć">
        <ContentTemplate>
            <asp:HiddenField ID="hidRejectEmp" runat="server" Visible="false" />
            <div class="form-group">
                <label>Powód odrzucenia: </label>
                <asp:TextBox ID="tbReject" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnReject" runat="server" Text="Odrzuć" CssClass="btn btn-danger" OnClick="btnReject_Click1" />
        </FooterTemplate>
    </cc:cntModal>

    <%----- schematy -----%>

    <cc:cntModal runat="server" ID="cntSetSchemesModal" Title="Ustaw schemat">
        <ContentTemplate>
            <asp:HiddenField ID="hidSchemeEmp" runat="server" Visible="false" />
            <asp:HiddenField ID="hidSchemeDays" runat="server" Visible="false" />
            <div class="form-group" runat="server" visible="true">
                <label>Schematy:</label>
                <asp:DropDownList ID="ddlScheme" runat="server" DataValueField="Id" DataTextField="Name" CssClass="form-control" DataSourceID="dsSchemes" />
            </div>
            <div class="form-group">
                <label>Data od:</label>
                <cc:DateEdit ID="deLeft" runat="server" />

                <label>Data do:</label>
                <cc:DateEdit ID="deRight" runat="server" />

            </div>
            <div class="form-group">
                <label>Pierwszy dzień:</label>
                <asp:DropDownList ID="ddlPamietamTeDawneDni" runat="server" DataValueField="Id" DataTextField="Name" DataSourceId="dsPamietamTeDawneDni" 
                    CssClass="form-control"  />
            </div>

            <%--<asp:Button ID="btnShowPlan" runat="server" OnClick="btnShowPlan_Click" Text="Pokaż plan" CssClass="btn btn-default" />--%>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnSaveScheme" runat="server" CssClass="btn btn-success" OnClick="btnSaveScheme_Click" Text="Zapisz" />
        </FooterTemplate>
    </cc:cntModal>


    <%-- copy from  --%>

    <cc:cntModal ID="cntCopyModal" runat="server" Title="Kopiuj z">
        <ContentTemplate>
            <asp:HiddenField ID="hidCopyEmp" runat="server" Visible="false" />
            <asp:HiddenField ID="hidCopyDays" runat="server" Visible="false" />

            <div class="form-group">
                <label>Pracownik:</label>
                <asp:DropDownList ID="ddlPrac2" runat="server" DataSourceID="dsPrac2" DataValueField="Value" DataTextField="Text" CssClass="form-control" />
<%--                <asp:SqlDataSource ID="dsPrac2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                    SelectCommand="
                    select null Id, 'wszyscy pracownicy ...' Name, 0 Sort 
                    union all
                    select IdPracownika Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 Sort 
                    --from dbo.fn_GetTree2(@kierId, 0, GETDATE())
                    --where IdPracownika not in (select items from dbo.SplitInt(@emp, ',')) 
                    from Pracownicy p
                    inner join Przypisania r on r.Status = 1 and getdate() between r.Od and isnull(r.Do, '20990909') and r.IdKierownika = @kierId and r.IdPracownika = p.Id
                    
                    order by Sort, Name
                    ">
                    <SelectParameters>
                        <asp:ControlParameter Name="emp" Type="String" ControlID="hidCopyEmp" PropertyName="Value" />
                        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="ddlKier" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
                <asp:SqlDataSource ID="dsPrac2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                    CancelSelectOnNullParameter="false" OnSelecting="dsPrac2_Selecting"
                    SelectCommand="
declare @empIds nvarchar(MAX) = @ids
select null Value, 'wybierz ...' Text, 0 Sort
union all
select Id Value, Nazwisko  + ' '+ Imie + isnull(' (' + KadryId + ')', '') Text, 1 Sort
from dbo.SplitInt(@empIds, ',') a
inner join Pracownicy p on p.Id = a.items
order by Sort, Text
                    ">
                    <SelectParameters>
                        <asp:Parameter Name="ids" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div class="form-group">
                <label>Data od:</label>
                <cc:DateEdit ID="deCopyLeft" runat="server" />

                <label>Data do:</label>
                <cc:DateEdit ID="deCopyRight" runat="server" />
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnCopyFrom" runat="server" CssClass="btn btn-success" OnClick="btnCopyFrom_Click" Text="Zapisz" />
        </FooterTemplate>
    </cc:cntModal>

    <%-- edycja schematów --%>

    <cc:cntModal runat="server" ID="cntModal" Title="Schematy" WidthType="Large">
        <ContentTemplate>
            <uc1:cntSchematy runat="server" ID="cntSchematy" OnChanged="cntSchematy_Changed" />
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnShowSchemeInsert" runat="server" CssClass="btn-success pull-left" OnClick="btnShowSchemeInsert_Click" Text="Dodaj nowy schemat" />
        </FooterTemplate>
    </cc:cntModal>

    <%-- edycja zmian --%>

    <cc:cntModal runat="server" ID="cntModalZmiany" Width="1200px" Title="Edycja zmian">
        <ContentTemplate>
<%--            <cc:ZmianyControl3 runat="server" ID="ZmianyControl3" />--%>
            <cc:cntZmianyList runat="server" id="cntZmianyList" onEditClick="cntModalZmiany_EditClick"/>

        </ContentTemplate>
    </cc:cntModal>

       
    <cc:cntZmianyEditModal runat="server" ID="cntZmianyEditModal" onSaved="cntZmianyEditModal_Saved"/>


</div>

<asp:SqlDataSource ID="dsSchemes" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null Id, 'wybierz schemat ...' Name, 0 Sort
union all
select Id, Nazwa Name, Kolejnosc Sort
from rcpSchematy 
where Aktywny = 1 
order by Sort, Name" />

<asp:SqlDataSource ID="dsPamietamTeDawneDni" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select 0 Id, 'Poniedziałek' Name
union all
select 6 Id, 'Wtorek' Name
union all
select 5 Id, 'Środa' Name
union all
select 4 Id, 'Czwartek' Name
union all
select 3 Id, 'Piątek' Name
union all
select 2 Id, 'Sobota' Name
union all
select 1 Id, 'Niedziela' Name
" />

<asp:SqlDataSource ID="dsKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare @od datetime
set @od = dbo.getdate(GETDATE())
select P.IdPracownika as Value, 
REPLICATE('&nbsp;', Hlevel * 4) +
P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') as Text, P.Nazwisko, P.Imie, P1.Sort 
from dbo.fn_GetTree2(@userId, 1, @od) P
outer apply (select case when P.IdPracownika = @userId then 1 else 2 end Sort) P1
left join Przypisania r on r.Id = P.IdPrzypisania
where Kierownik = 1 and (@com is null or r.IdCommodity = @com)
--order by Sort, Nazwisko, Imie
order by P.SortPath
">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
        <asp:ControlParameter Name="com" Type="Int32" ControlID="ddlCom" PropertyName="SelectedValue" />

    </SelectParameters>
</asp:SqlDataSource>

<%--
UWAGA: póki co "na dziś" kierownicy powinni się pokazać na dzień planowania - może lepsza byłaby fn_GetTreeOkres
        <asp:ControlParameter Name="od" ControlID="hidFrom" PropertyName="Value" Type="DateTime" />

declare @data datetime
set @data = dbo.getdate(GETDATE())
select null as Value, 'wybierz przełożonego ...' as Text, null as Nazwisko, null as Imie, -1 as Sort
union all
select P.Id as Value, case when K1.Aktywny = 2 then '*' else '' end + P.Nazwisko + ' ' + P.Imie as Text, P.Nazwisko, P.Imie, K1.Aktywny as Sort
from Pracownicy P 
outer apply (select top 1 * from Przypisania where IdKierownika = P.Id and Status = 1 and Od &lt;= @data order by Od desc) K
outer apply (select case when @data between ISNULL(K.Od, '19000101') and ISNULL(K.Do,'20990909') then 1 else 2 end Aktywny) K1
where K.Id is not null
union all
select 0 as Value, 'Poziom główny struktury' as Text, null as Nazwisko, null as Imie, 3 as Sort 
order by Sort, Nazwisko, Imie
--%>

<asp:SqlDataSource ID="dsKierAll" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare @data datetime
set @data = dbo.getdate(GETDATE())
select null as Value, 'wybierz przełożonego ...' as Text, null as Nazwisko, null as Imie, -1 as Sort
union all
select P.Id as Value, case when K1.Aktywny = 2 then '*' else '' end + P.Nazwisko + ' ' + P.Imie as Text, P.Nazwisko, P.Imie, K1.Aktywny as Sort
from Pracownicy P 
outer apply (select top 1 * from Przypisania where IdKierownika = P.Id and Status = 1 and Od &lt;= @data order by Od desc) K
/*outer apply (select case when @data between ISNULL(K.Od, '19000101') and ISNULL(K.Do,'20990909') then 1 else 2 end Aktywny) K1*/
outer apply (select case when k.Id is not null then 1 else 2 end Aktywny) K1
where K.Id is not null or Kierownik = 1
union all
select 0 as Value, 'Poziom główny struktury' as Text, null as Nazwisko, null as Imie, 3 as Sort 
order by Sort, Nazwisko, Imie
">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

<%-- lista pracowników
<asp:DropDownList ID="ddlPrac" runat="server" DataSourceID="dsPrac" DataValueField="Id" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlPrac_SelectedIndexChanged" />
<asp:SqlDataSource ID="dsPrac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null Id, 'wszyscy pracownicy ...' Name, 0 Sort 
union all 
select IdPracownika Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 Sort 
from dbo.fn_GetTree2(@kierId, 0, GETDATE())
order by Sort, Name
">
    <SelectParameters>
        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidKierId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
--%>

<asp:SqlDataSource ID="dsApplyScheme" runat="server" SelectCommand="
declare @prac_list nvarchar(MAX) = {2} 
--declare @date_list nvarchar(341) = {3}

--select items into #ddd from dbo.SplitStr(@date_list, ',')

declare @schId int = {0}
declare @od datetime = {3}
declare @do datetime = {4}
declare @accId int = {1}
declare @nadzien datetime = GETDATE()
declare @mod int = {5} /* modyfikator dnia zaczynajacego */

declare @sch nvarchar(512)
declare @dow int

select @sch = Schemat from rcpSchematy where Id = @schId

--select @od = MIN(items) from #ddd
--select @do = MAX(items) from #ddd

select @dow = dbo.dow(@od) + @mod

select
  /*@pracId*/ poa.items IdPracownika
, d.Data Data
, s.items IdZmiany
, @nadzien DataZm
into #aaa
from dbo.GetDates2(@od, @do) d
outer apply (select (d.Lp + @dow) / 7 % (select COUNT(items) from dbo.SplitIntSort(@sch, ',')) c) oa
left join dbo.SplitIntSort(@sch, ',') s on s.idx = oa.c
left join Kalendarz k on k.Data = d.Data
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
--where d.Data in (select items from #ddd)
where ISNULL(k.Rodzaj, -1) not in (/*0, 1,*/ 2) and (d.Lp + @dow) % 7 not in (5,6)

/*
select a.*, h.Id OriginalId into #bbb
from #aaa a
left join rcpHarmonogram h on h.IdPracownika = a.IdPracownika and h.Data = a.Data
where h._do is null and a.IdZmiany != ISNULL(h.IdZmiany, -1)
*/

select a.*, /*h*/pp.Id OriginalId into #bbb
from #aaa a
/*left join rcpHarmonogram h on h.IdPracownika = a.IdPracownika and h.Data = a.Data*/
left join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
/*where h._do is null and a.IdZmiany != ISNULL(h.IdZmiany, -1)*/
where ISNULL(a.IdZmiany, -1) != ISNULL(pp.IdZmianyPlan, -1)

/*
update rcpHarmonogram set
  _do = @nadzien
from rcpHarmonogram h
inner join #bbb a on /*a.IdPracownika = h.IdPracownika and a.Data = h.Data*/ a.OriginalId = h.Id and h._do is null
*/

update PlanPracy set
  IdZmianyPlan = b.IdZmiany
, WymiarPlan = 28800
from PlanPracy pp
inner join #bbb b on b.OriginalId = pp.Id

/*
insert into rcpHarmonogram (Id, _od, IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select OriginalId, @nadzien, a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #bbb a
*/

insert into PlanPracy (IdPracownika, Data, IdZmianyPlan, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc, WymiarPlan)
select a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId, 28800
from #bbb a
where a.OriginalId is null


drop table #aaa
drop table #bbb
--drop table #ddd

" />

<asp:SqlDataSource ID="dsCopyFrom" runat="server" SelectCommand="
declare @prac_list nvarchar(MAX) = {2}
--declare @date_list nvarchar(341) = {3}

declare @od datetime = {3}
declare @do datetime = {4}

--select items into #ddd from dbo.SplitStr(@date_list, ',')

declare @accId int = {1}
declare @nadzien datetime = GETDATE()

declare @pracId int = {0}
    /*
select
  /*@pracId*/ poa.items IdPracownika
, h.Data
, h.IdZmiany
, @nadzien DataZm
into #aaa
from rcpHarmonogram h
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
where h.Data /*in (select items from #ddd) */ between @od and @do and h.IdPracownika = @pracId and h._do is null
    */

select
  /*@pracId*/ poa.items IdPracownika
, /*h*/pp.Data
, /*h*/pp.IdZmianyPlan IdZmiany
, pp.WymiarPlan Wymiar
, @nadzien DataZm
into #aaa
/*from rcpHarmonogram h*/
from PlanPracy pp
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
/*where h.Data /*in (select items from #ddd) */ between @od and @do and h.IdPracownika = @pracId and h._do is null*/
where pp.Data between @od and @do and pp.IdPracownika = @pracId

/*
select
  /*@pracId*/ poa.items IdPracownika
, d.Data Data
, s.items IdZmiany
, @nadzien DataZm
into #aaa
from dbo.GetDates2(@od, @do) d
outer apply (select (d.Lp + @dow) / 7 % (select COUNT(items) from dbo.SplitIntSort(@sch, ',')) c) oa
left join dbo.SplitIntSort(@sch, ',') s on s.idx = oa.c
/*left join Kalendarz k on k.Data = d.Data*/
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
/*where ISNULL(k.Rodzaj, -1) not in (0, 1, 2)*/
*/
 
select a.*, /*h*/pp.Id OriginalId into #bbb
from #aaa a
/*left join rcpHarmonogram h on h.IdPracownika = a.IdPracownika and h.Data = a.Data*/
left join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
/*where h._do is null and a.IdZmiany != ISNULL(h.IdZmiany, -1)*/
where ISNULL(a.IdZmiany, -1) != ISNULL(pp.IdZmianyPlan, -1)

/*
update rcpHarmonogram set
  _do = @nadzien
from rcpHarmonogram h
inner join #bbb a on /*a.IdPracownika = h.IdPracownika and a.Data = h.Data*/ a.OriginalId = h.Id and h._do is null
*/

update PlanPracy set
  IdZmianyPlan = b.IdZmiany
, WymiarPlan = b.Wymiar
from PlanPracy pp
inner join #bbb b on b.OriginalId = pp.Id

/*
insert into rcpHarmonogram (Id, _od, IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select OriginalId, @nadzien, a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #bbb a
*/

insert into PlanPracy (IdPracownika, Data, IdZmianyPlan, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #bbb a
where a.OriginalId is null

drop table #aaa
drop table #bbb
--drop table #ddd
" />

<asp:SqlDataSource ID="dsPrint" runat="server"
    SelectCommand="
/*
 * DBW - PLAN PRACY
 *
 */

declare @kierId varchar(12) = {0}

declare @colsH nvarchar(MAX)
declare @colsD nvarchar(MAX)
declare @colsA nvarchar(MAX)
declare @colsA2 nvarchar(MAX)

declare @stmt nvarchar(MAX)

declare @od datetime = {1}
declare @do datetime = {2}

declare @pracIds nvarchar(MAX) = {3}

select
  @colsH = isnull(@colsH + ',', '') + 'ISNULL([' + convert(varchar(10), d.Data, 20) + '], ' + case when k.Rodzaj in (0, 1, 2) then '''|holiday''' else '''''' end + ') [' + convert(varchar, DAY(d.Data)) + ']'
, @colsD = isnull(@colsD + ',', '') + '[' + convert(varchar(10), d.Data, 20) + ']'
, @colsA = isnull(@colsA + ',', '') + '''' + case dbo.dow(d.Data)
    when 0 then 'pn'
    when 1 then 'wt'
    when 2 then 'śr'
    when 3 then 'czw'
    when 4 then 'pt'
    when 5 then 'sb'
    when 6 then 'nd'
    else 'aoe'
end + '|holiday'''
, @colsA2 = isnull(@colsA2 + ',', '') + '''' + convert(varchar, DAY(d.Data)) + '|holiday'' [' + convert(varchar, DAY(d.Data)) + ']'
from dbo.GetDates2(@od, @do) d
left join Kalendarz k on k.Data = d.Data

select @stmt = '
declare @kierId int = ' + @kierId +  '
declare @pracIds nvarchar(MAX) = ''' + @pracIds + '''

select
'''' RowClass
, ''|col1'' [0]
, ' + @colsA2 + '
, ''|last-col'' [-1]
union all
select
'''' RowClass
, ''PRZEŁOŻONY|col1''
, ' + @colsA + '
, ''Data i podpis pracownika|last-col''
union all
select
'''' RowClass
,  p.Nazwisko + '' '' + p.Imie + ''|col1'' [MISTRZ]
, ' + @colsH + '
, ''|last-col'' [Data i podpis pracownika]
from Pracownicy p
left join
(
    select * from
    (
        select pp.IdPracownika, z.Symbol + case when k.Rodzaj in (0, 1, 2) then ''|holiday'' else '''' end Symbol, pp.Data
        from PlanPracy pp
        left join Zmiany z on z.Id = pp.IdZmiany
        left join Kalendarz k on k.Data = pp.Data
    ) pp
    PIVOT
    (
        MAX(pp.Symbol) for pp.Data in (' + @colsD + ')
    ) PV
) t on t.IdPracownika = p.Id
where p.Id = @kierId


select
  '''' RowClass
,  p.Nazwisko + '' '' + p.Imie + ''|col1'' [Pracownik]
, ' + @colsH + '
, ''|last-col'' [Data i podpis pracownika]
/*from dbo.fn_GetTree2(@kierId, 0, GETDATE())*/ from dbo.SplitInt(@pracIds, '','') tr
left join Pracownicy p on p.Id = tr./*IdPracownika*/ items
left join
(
    select * from
    (
        select pp.IdPracownika, z.Symbol + case when k.Rodzaj in (0, 1, 2) then ''|holiday'' else '''' end Symbol, pp.Data
        from PlanPracy pp
        left join Zmiany z on z.Id = pp.IdZmiany
        left join Kalendarz k on k.Data = pp.Data
    ) pp
    PIVOT
    (
        MAX(pp.Symbol) for pp.Data in (' + @colsD + ')
    ) PV
) t on t.IdPracownika = tr./*IdPracownika*/items
order by [Pracownik]
'

exec sp_executesql @stmt

" />

<asp:SqlDataSource ID="dsPrintHarmonogramKeeeper" runat="server"
    SelectCommand="
/*
 * KEEEPER - HARMONOGRAM
 *
 */

declare @kierId varchar(12) = {0}

declare @colsH nvarchar(MAX)
declare @colsD nvarchar(MAX)
declare @colsA nvarchar(MAX)
declare @colsA2 nvarchar(MAX)

declare @stmt nvarchar(MAX)

declare @od datetime = {1}
declare @do datetime = {2}

declare @pracIds nvarchar(MAX) = {3}
declare @empty bit = {4}

select
  @colsH = isnull(@colsH + ',', '') + 'ISNULL([' + convert(varchar(10), d.Data, 20) + '], ' + 
  case k.Rodzaj 
  when 0 then '''|saturday''' 
  when 1 then '''|sunday''' 
  when 2 then '''|holiday'''
  else '''''' end + ') [' + convert(varchar, DAY(d.Data)) + ']'
, @colsD = isnull(@colsD + ',', '') + '[' + convert(varchar(10), d.Data, 20) + ']'
, @colsA = isnull(@colsA + ',', '') + '''' + case dbo.dow(d.Data)
    when 0 then 'pn'
    when 1 then 'wt'
    when 2 then 'śr'
    when 3 then 'czw'
    when 4 then 'pt'
    when 5 then 'sb'
    when 6 then 'nd'
    else 'aoe'
end + case k.Rodzaj 
  when 0 then '|saturday'''
  when 1 then '|sunday'''
  when 2 then '|holiday'''
  else '''' end

, @colsA2 = isnull(@colsA2 + ',', '') + '''' + convert(varchar, DAY(d.Data)) +
     case k.Rodzaj 
  when 0 then '|saturday'' ['
  when 1 then '|sunday'' ['
  when 2 then '|holiday'' ['
  else ''' [' end
     + convert(varchar, DAY(d.Data)) + ']'
from dbo.GetDates2(@od, @do) d
left join Kalendarz k on k.Data = d.Data

select @stmt = '
declare @empty int = ' +isnull(CONVERT(varchar, @empty), 'null') + '
declare @pracIds nvarchar(MAX) = ''' + @pracIds + '''
declare @do datetime = ''' + convert(nvarchar(50), @do) + '''


select
'''' RowClass
, ''|lp'' [0]
, ''|col1'' [lp]
--, ''|col2'' [jedn]
, ' + @colsA2 + '
--, ''|last-col'' [-1]
union all
select
'''' RowClass
, ''Lp|lp''
, ''Pracownik|col1''
--, ''Jednostka|jedn''
, ' + @colsA + '
--, ''Data i podpis pracownika|last-col''
select
  '''' RowClass
, convert(varchar(100), (row_number() over (order by tr.idx/*p.Nazwisko + '' '' + p.Imie*/))) + ''|lp'' [Lp]
,  p.Nazwisko + '' '' + p.Imie + ''|col1'' [Pracownik]
--,  d.Nazwa + ''|jedn'' [Jednostka]
, ' + @colsH + '
--, ''|last-col'' [Data i podpis pracownika]
/*from dbo.fn_GetTree2(@kierId, 0, GETDATE())*/ from dbo.SplitIntSort(@pracIds, '','') tr
left join Pracownicy p on p.Id = tr./*IdPracownika*/ items
left join PracownicyStanowiska ps on ps.IdPracownika = p.Id and @do between ps.Od and isnull(ps.Do, ''20990909'')
left join Dzialy d on ps.IdDzialu = d.Id
left join
(
    select * from
    (
        select pp.IdPracownika, case when @empty = 1 then case when z.Symbol in (''WS'', ''WN'', ''DW'') then z.Symbol else '''' end else z.Symbol end + 
			case k.Rodzaj
			when 0 then ''|saturday''
			when 1 then ''|sunday''
			when 2 then ''|holiday''
			else 
				case z.Symbol
				    when ''WS'' then ''|ws''
				    when ''WN'' then ''|wn''
				    when ''DW'' then ''|dw''
				    else ''''
				end
			end Symbol
		
		, pp.Data
        from PlanPracy pp
        left join Zmiany z on z.Id = pp.IdZmiany
        left join Kalendarz k on k.Data = pp.Data
    ) pp
    PIVOT
    (
        MAX(pp.Symbol) for pp.Data in (' + @colsD + ')
    ) PV
) t on t.IdPracownika = tr./*IdPracownika*/items
--order by [Pracownik]
'

exec sp_executesql @stmt

" />

<asp:SqlDataSource ID="dsPrint2" runat="server"
    SelectCommand="
declare @od datetime = {0}
declare @do datetime = {1}
declare @pracId int = {2}


select 
  'header' RowClass
, 'lp.|lp' Lp
, 'data|data' Data
, 'ZMIANA|zmiana' Zmiana
, 'godzina rozpoczęcia pracy|godzrozp' GodzRozp
, 'godzina zakończenia pracy|godzzak' GodzZak
, 'suma godzin|sumagodz' SumaGodz
, 'podpis pracownika  lub rodzaj nieobecności|podpis' Podpis
, 'ilość godzin normalnych|godznorm' GodzNorm
, 'ilość godzin nocnych|godznoc' GodzNoc
, 'ilość godzin nadliczbowych 50%|nad50' GodzNadliczb50
, 'ilość godzin nadliczbowych 100%|nad100' GodzNadliczb100
union all
select 
  case when k.Data is null then '' else 'holiday' end RowClass
, convert(varchar, Lp + 1) + '|lp' Lp
, case dbo.dow(d.Data)
    when 0 then 'pn'
    when 1 then 'wt'
    when 2 then 'śr'
    when 3 then 'czw'
    when 4 then 'pt'
    when 5 then 'sb'
    when 6 then 'nd'
    else 'aoe' end + '|data' Data
, isnull(z.Symbol,'') + '|zmiana' + '^background-color: ' + isnull(z.Kolor, '#fff') Zmiana
, '|godzrozp' GodzRozp
, '|godzzak' GodzZak
, '|sumagodz' SumaGodz
, '|podpis' Podpis
, '|godznorm' GodzNorm
, '|godznoc' GodzNoc
, '|nad50' GodzNadliczb50
, '|nad100' GodzNadliczb100
from dbo.GetDates2(@od, @do) d
left join PlanPracy pp on pp.Data = d.Data and pp.IdPracownika = @pracId
left join Zmiany z on z.Id = pp.IdZmianyPlan
left join Kalendarz k on k.Data = d.Data
" />


<asp:SqlDataSource ID="dsPrint2Keeeper" runat="server"
    SelectCommand="
declare @od datetime = {0}
declare @do datetime = {1}
declare @pracId int = {2}


select 
  'header' RowClass
, 'lp.|lp' Lp
, 'data|data' Data
, 'ZMIANA|zmiana' Zmiana
, 'godzina rozpoczęcia pracy|godzrozp' GodzRozp
, 'godzina zakończenia pracy|godzzak' GodzZak
, 'suma godzin|sumagodz' SumaGodz
, 'podpis pracownika  lub rodzaj nieobecności|podpis' Podpis
, 'ilość godzin normalnych|godznorm' GodzNorm
, 'ilość godzin nocnych|godznoc' GodzNoc
, 'ilość godzin nadliczbowych 50%|nad50' GodzNadliczb50
, 'ilość godzin nadliczbowych 100%|nad100' GodzNadliczb100
, 'podpis pracownika' PodpisPracw
, 'podpis pracodawcy' PodpisPracd
union all
select 
  case when pp.IdZmianyPlan is not null then '' else 'holiday' end RowClass
, convert(varchar, Lp + 1) + '|lp' Lp
, case dbo.dow(d.Data)
    when 0 then 'pn'
    when 1 then 'wt'
    when 2 then 'śr'
    when 3 then 'czw'
    when 4 then 'pt'
    when 5 then 'sb'
    when 6 then 'nd'
    else 'aoe' end + '|data' Data
, isnull(z.Symbol,'') + '|zmiana' + '^background-color: ' + isnull(z.Kolor, '#fff') Zmiana
, '|godzrozp' GodzRozp
, '|godzzak' GodzZak
, '|sumagodz' SumaGodz
, '|podpis' Podpis
, '|godznorm' GodzNorm
, '|godznoc' GodzNoc
, '|nad50' GodzNadliczb50
, '|nad100' GodzNadliczb100
, '|podppracw' PodpisPracw
, '|podppracd' PodpisPracd
from dbo.GetDates2(@od, @do) d
left join PlanPracy pp on pp.Data = d.Data and pp.IdPracownika = @pracId
left join Zmiany z on z.Id = pp.IdZmianyPlan
left join Kalendarz k on k.Data = d.Data
" />



<asp:SqlDataSource ID="dsPrint2Prac" runat="server" SelectCommand="select Imie + ' ' + Nazwisko + ISNULL(' (' + KadryId + ')','') Pracownik from Pracownicy where Id = {0}" />
<asp:SqlDataSource ID="dsPrint2Comp" runat="server" SelectCommand="select isnull(Klasyfikacja, '') Klasyfikacja from PracownicyStanowiska where IdPracownika = {0} and {1} between Od and ISNULL(Do, '20990909')" />

<asp:SqlDataSource ID="dsSendToAcc2" runat="server"
    SelectCommand="
declare @pracIds nvarchar(MAX) = {0}

declare @date datetime = {1}
declare @kierId int = {2}

declare @nadzien datetime = GETDATE()

/*declare @nId int*/

insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego) select items, @date, 1, @nadzien, @kierId from dbo.SplitInt(@pracIds, ',')

/*set @nId = SCOPE_IDENTITY()*/

update rcpHarmonogram set
  IdNaglowka = ha.Id
/*select **/
from rcpHarmonogram h
inner join dbo.SplitInt(@pracIds, ',') p on p.items = h.IdPracownika
left join rcpHarmonogramAcc ha on ha.Data = dbo.bom(h.Data) and ha.IdPracownika = h.IdPracownika and ha.Status = 1
/*inner join rcpHarmonogramAcc ha on ha.IdPracownika = h.IdPracownika and ha.Data = dbo.bom(h.Data) and ha.Status = 1*/
where h._do is null and h.IdNaglowka is null and dbo.bom(h.Data) = @date 
" />

<asp:SqlDataSource ID="dsSendToAcc" runat="server"
    SelectCommand="
/*declare @pracIds nvarchar(MAX) = {0}

declare @date datetime = {1}
declare @kierId int = {2}

declare @nadzien datetime = GETDATE()
insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego) select items, @date, 1, @nadzien, @kierId from dbo.SplitInt(@pracIds, ',')
*/

declare @od datetime = {1}
declare @kierId int = {2}

declare @nadzien datetime = GETDATE()
insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego)
select
  s.items
, @od
, 1
, GETDATE()
, @kierId
from dbo.SplitInt('{0}', ',') s
left join rcpHarmonogramAcc ha on ha.IdPracownika = s.items and ha.Data = @od
where ha.Id is null

update rcpHarmonogramAcc set
  Status = 1
, Uwagi = 'Powód odrzucenia: ' + Uwagi + ''
from rcpHarmonogramAcc ha
where ha.IdPracownika in ({0}) and ha.Data = @od
" />

<asp:SqlDataSource ID="dsToAccList" runat="server"
    SelectCommand="
select Id from rcpHarmonogramAcc where Status = 1 and Data = {0}
" />

<%--<asp:SqlDataSource ID="dsAccept" runat="server"
    SelectCommand="
declare @nIds nvarchar(MAX) = {0}

select
  h.*
into #aaa
from rcpHarmonogram h
inner join dbo.SplitInt(@nIds, ',') n on n.items = h.IdNaglowka
/*where h.IdNaglowka = @nId*/

update PlanPracy set
/*select pp.IdZmiany, a.IdZmiany*/
  IdZmiany = a.IdZmiany
from PlanPracy pp
inner join #aaa a on a.IdPracownika = pp.IdPracownika and a.Data = pp.Data

insert into PlanPracy (IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select
  a.IdPracownika
, a.Data
, a.IdZmiany
, a.DataZm
, a.IdKierownikaZm
, a.Akceptacja
, a.DataAcc
, a.IdKierownikaAcc
from #aaa a
left join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
where pp.Id is null

update rcpHarmonogramAcc set
  Status = 2
from rcpHarmonogramAcc ha
inner join dbo.SplitInt(@nIds, ',') n on n.items = ha.Id

drop table #aaa

    
    " />--%>


<asp:SqlDataSource ID="dsAccept" runat="server"
    SelectCommand="
declare @od datetime = {1}
declare @do datetime = {2}

update PlanPracy set
  IdZmiany = IdZmianyPlan
, Wymiar = WymiarPlan
from PlanPracy pp 
where pp.IdPracownika in ({0}) and pp.Data between @od and @do

declare @nadzien datetime = GETDATE()
insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego)
select
  s.items
, @od
, 1
, GETDATE()
, -1
from dbo.SplitInt('{0}', ',') s
left join rcpHarmonogramAcc ha on ha.IdPracownika = s.items and ha.Data = @od
where ha.Id is null

update rcpHarmonogramAcc set
  Status = 2
, Uwagi = null
from rcpHarmonogramAcc ha
where ha.IdPracownika in ({0}) and ha.Data = @od

insert rcpHarmonogramHistoria (IdPracownika, Data, IdZmiany, Wymiar, IdNaglowka, Wersja)
select
  pp.IdPracownika
, pp.Data
, pp.IdZmiany
, pp.Wymiar
, ha.Id
, @nadzien
from PlanPracy pp
left join rcpHarmonogramAcc ha on ha.IdPracownika = pp.IdPracownika and ha.Data = @od
where pp.IdPracownika in ({0}) and pp.Data between @od and @do

/*
declare @nIds nvarchar(MAX) = {0}

select
  h.*
into #aaa
from rcpHarmonogram h
inner join dbo.SplitInt(@nIds, ',') n on n.items = h.IdNaglowka
/*where h.IdNaglowka = @nId*/

update PlanPracy set
/*select pp.IdZmiany, a.IdZmiany*/
  IdZmiany = a.IdZmiany
, DataEksportuHarmonogramu = null
from PlanPracy pp
inner join #aaa a on a.IdPracownika = pp.IdPracownika and a.Data = pp.Data

insert into PlanPracy (IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select
  a.IdPracownika
, a.Data
, a.IdZmiany
, a.DataZm
, a.IdKierownikaZm
, a.Akceptacja
, a.DataAcc
, a.IdKierownikaAcc
from #aaa a
left join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
where pp.Id is null

update rcpHarmonogramAcc set
  Status = 2
from rcpHarmonogramAcc ha
inner join dbo.SplitInt(@nIds, ',') n on n.items = ha.Id

drop table #aaa
*/
" />

<asp:SqlDataSource ID="dsReject" runat="server"
    SelectCommand="
declare @od datetime = {1}
declare @do datetime = {2}

declare @reason nvarchar(MAX) = {3}

/*update PlanPracy set
  IdZmiany = IdZmianyPlan
, Wymiar = WymiarPlan
from PlanPracy pp 
where pp.IdPracownika in ({0}) and pp.Data between @od and @do*/

declare @nadzien datetime = GETDATE()
insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego, Uwagi)
select
  s.items
, @od
, -1
, GETDATE()
, -1
, @reason
from dbo.SplitInt('{0}', ',') s
left join rcpHarmonogramAcc ha on ha.IdPracownika = s.items and ha.Data = @od
where ha.Id is null

update rcpHarmonogramAcc set
  Status = -1
, Uwagi = @reason
from rcpHarmonogramAcc ha
where ha.IdPracownika in ({0}) and ha.Data = @od
" />

<asp:SqlDataSource ID="dsPracOrder" runat="server"
    SelectCommand="
select
  p.Id
from Pracownicy p
inner join PracownicyStanowiska ps on ps.IdPracownika = p.Id and {1} between ps.Od and ISNULL(ps.Do, '20990909')
where p.Id in ({0})
order by ps.Klasyfikacja, p.Nazwisko, p.Imie
" />

<cc:cntModal runat="server" ID="cntModalOkres" Title="Dodaj okres">
    <ContentTemplate>
        <%--   <div class="form-group">
                <label>Typ okresu:</label>
                <asp:DropDownList ID="ddlTypOkresu" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsTypOkresu" OnSelectedIndexChanged="ddlTypOkresu_SelectedIndexChanged"
                    CssClass="form-control" AutoPostBack="true" />
                <asp:SqlDataSource ID="dsTypOkresu" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select null Value, 'wybierz ...' Text, 0 Sort
union all
select Id Value, Nazwa Text, 1 Sort
from rcpOkresyRozliczenioweTypy
order by Sort, Text                    
" />
            </div>
            <div class="form-group">
                <label>Okres:</label>
                <asp:DropDownList ID="ddlOkresy" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsOkresy" CssClass="form-control"
                    OnSelectedIndexChanged="ddlOkresy_SelectedIndexChanged" AutoPostBack="true" />
                <asp:SqlDataSource ID="dsOkresy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
--declare @typ int = 1;
with o as
(
    select DATEADD(MONTH, 1, ISNULL(MAX(DataOd), dbo.bom(GETDATE()))) Data from OkresyRozliczeniowe where Typ = @typ
    union all
    select DATEADD(MONTH, ort.IloscMiesiecy, Data ) from o inner join rcpOkresyRozliczenioweTypy ort on ort.Id = @typ
)
select top 10
  CONVERT(varchar(10), dbo.bom(Data), 20) + ';' + CONVERT(varchar(10), DATEADD(MONTH, ort.IloscMiesiecy - 1, dbo.eom(Data)), 20) Value
, CONVERT(varchar(10), dbo.bom(Data), 20) + ' - ' + CONVERT(varchar(10), DATEADD(MONTH, ort.IloscMiesiecy - 1, dbo.eom(Data)), 20) Text
from o inner join rcpOkresyRozliczenioweTypy ort on ort.Id = @typ

">
                    <SelectParameters>
                        <asp:ControlParameter Name="typ" Type="Int32" ControlID="ddlTypOkresu" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div>
                <asp:Repeater ID="rpNom" runat="server">
                    <ItemTemplate>
                        <asp:HiddenField ID="hidDate" runat="server" Visible="false" Value='<%# Eval("Date") %>' />
                        <div class="row form-group">
                            <div class="col-sm-6">
                                <label>Czas nominalny - <%# Eval("Friendly") %></label>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="tbNom" runat="server" CssClass="form-control" Width="90px" MaxLength="5" />

                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:SqlDataSource ID="dsNom" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
declare @mies int = (select IloscMiesiecy from rcpOkresyRozliczenioweTypy where Id = @typ)
;with a as
(
    select 1 b
    union all
    select b + 1 from a where b &lt; @mies
)
select b from a
">
                    <SelectParameters>
                        <asp:ControlParameter Name="typ" Type="Int32" ControlID="ddlTypOkresu" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>--%>
    </ContentTemplate>
    <FooterTemplate>
        <%--<asp:Button ID="btnAddOkres" runat="server" CssClass="btn btn-success" OnClick="btnAddOkres_Click" Text="Zapisz" />--%>
    </FooterTemplate>
</cc:cntModal>

<asp:SqlDataSource ID="dsInsert" runat="server" SelectCommand="insert into OkresyRozliczeniowe (DataOd, DataDo, Status, Typ) values ({0}, {1}, 0, {2}); select SCOPE_IDENTITY();"></asp:SqlDataSource>
<asp:SqlDataSource ID="dsAddCzas" runat="server" SelectCommand="insert into CzasNom (Data, DniPrac, IdOkresu) values ({0}, {1}, {2})" />
<asp:SqlDataSource ID="dsNewOkres" runat="server" SelectCommand="select top 1 dbo.bom(dateadd(month, 1, DataOd)) DateFrom, dbo.eom(dateadd(month, 1, DataOd)) DateTo from OkresyRozl order by DataOd desc" />
