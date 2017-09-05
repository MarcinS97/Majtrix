<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlanPracyAccept.ascx.cs" Inherits="HRRcp.RCP.Controls.cntPlanPracyAccept" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controls/SelectOkres.ascx" TagName="SelectOkres" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntPlanPracy.ascx" TagName="PlanPracy" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/AcceptControl4.ascx" TagName="AcceptControl2" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/AbsencjaLegenda.ascx" TagName="AbsencjaLegenda" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/PPDayLegenda.ascx" TagName="PPDayLegenda" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/PPSumyLegenda.ascx" TagName="PPSumyLegenda" TagPrefix="uc2" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

<%@ Register Src="~/RCP/Controls/cntNadgodzinyWnioskiModal.ascx" TagPrefix="uc1" TagName="cntNadgodzinyWnioskiModal" %>

<%--
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register src="~/Controls/SelectZmiana.ascx" tagname="SelectZmiana" tagprefix="uc1" %>
--%>

<div id="paPlanPracyAccept" runat="server" class="cntPlanPracyAccept">
    <asp:HiddenField ID="hidKierId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />

    <div id="paSearch" runat="server" class="search">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Wyszukaj pracownika:" Visible="false"></asp:Label>


            <%-- T:nie działa - wyłączam, do zrobienia !!! --%>
            <asp:TextBox ID="tbSearch" runat="server" CssClass="form-control" MaxLength="250" Placeholder="Wyszukaj pracownika ..." Visible="false" ></asp:TextBox>
            <asp:Button ID="btClear" runat="server" CssClass="btn-default" Text="Czyść" Visible="false" />
            <%----%>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="filter">
                <asp:DropDownList ID="ddlKier" runat="server" CssClass="form-control ddlKier" DataSourceID="dsKier" DataValueField="Value" DataTextField="Text" AutoPostBack="true" OnSelectedIndexChanged="ddlKier_SelectedIndexChanged" Visible="true" />

                <asp:DropDownList ID="ddlDzialy" runat="server" CssClass="form-control ddlKier" DataSourceID="dsDzialy" DataValueField="Value"
                    DataTextField="Text" AutoPostBack="true" OnSelectedIndexChanged="ddlDzialy_SelectedIndexChanged" Width="230px" />
                <asp:SqlDataSource ID="dsDzialy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                    SelectCommand="select null Value, 'jednostka ...' Text, 0 Sort union all select Id Value, Nazwa Text, 1 Sort from Dzialy where Status >= 0 order by Sort, Text" />

                <asp:DropDownList ID="ddlStanowiska" runat="server" CssClass="form-control ddlKier" DataSourceID="dsStanowiska" DataValueField="Value"
                    DataTextField="Text" AutoPostBack="true" OnSelectedIndexChanged="ddlStanowiska_SelectedIndexChanged" Width="230px" />
                <asp:SqlDataSource ID="dsStanowiska" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                    SelectCommand="select null Value, 'stanowisko ...' Text, 0 Sort union all select Id Value, Nazwa Text, 1 Sort from Stanowiska where Aktywne = 1 order by Sort, Text" />
            </div>
            <asp:Button ID="btSearch" runat="server" Text="Wyszukaj" CssClass="button_postback" OnClick="btSearch_Click" />
            <div class="PlanPracyAccept">
                <table class="okres_navigator">
                    <tr>
                        <td class="colleft">
                            <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Akceptacja czasu pracy" Visible="false"></asp:Label>
                        </td>
                        <td class="colmiddle">
                            <uc1:SelectOkres ID="cntSelectOkres" ControlID="cntPlanPracy" StoreInSession="true" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
                        </td>
                        <td class="colright">
                            <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server"></asp:Label>
                            <asp:Label ID="lbAccept" runat="server" Text="Akceptuj do:"></asp:Label>
                            <uc1:DateEdit ID="deDataAccept" runat="server" />
                            <asp:Button ID="btAccept" runat="server" Text="Akceptuj" CssClass="button75" OnClick="btAcceptPP_Click" />
                            <asp:Button ID="btAcceptConfirm" runat="server" Text="" CssClass="hidden" OnClick="btAcceptConfirm_Click" />
                            <asp:Button ID="btCheck" runat="server" Text="Sprawdź" Visible="false" CssClass="button75" OnClick="btCheckPP_Click" />
                        </td>
                    </tr>
                </table>
                <uc1:PlanPracy ID="cntPlanPracy" Mode="1" OnSelectDay="OnSelectDay" runat="server" StatusVisible="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="legenda_col1">
        <uc2:PPDayLegenda ID="PPDayLegenda" runat="server" />
        <%--<br />--%>
        <uc2:PPSumyLegenda ID="PPSumyLegenda" runat="server" />
    </div>
    <uc2:AbsencjaLegenda ID="AbsencjaLegenda" runat="server" />

    <uc1:cntModal runat="server" ID="cntModal" Title="Akceptacja czasu pracy" Width="1024px" ShowCloseButton="false" Backdrop="false" ShowHeader="false" ShowFooter="false" >
        <ContentTemplate>
            <uc1:AcceptControl2 ID="cntAccept2"
                OnAcceptChanges="cntAccept_AcceptChanges"
                OnCancelChanges="cntAccept_CancelChanges"
                OnNadgodzinyWnioskiModalShow="cntAccept2_OnNadgodzinyWnioskiModalShow"
                runat="server" />
        </ContentTemplate>
    </uc1:cntModal>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:cntNadgodzinyWnioskiModal runat="server" ID="cntNadgodzinyWnioskiModal" Mode="PostAccept" />
        </ContentTemplate>
    </asp:UpdatePanel>

<%--    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="tmpBtModalPopup" runat="server" Style="display: none;" />
            <asp:Panel ID="paAccept" CssClass="modalPopupAcc" Style="display: none;" runat="server" DefaultButton="tmpBtModalPopup">
            </asp:Panel>
            <asp:ModalPopupExtender ID="paAccept_ModalPopupExtender" runat="server"
                Enabled="true"
                BackgroundCssClass="modalBackground"
                PopupControlID="paAccept"
                TargetControlID="tmpBtModalPopup"
                BehaviorID="mpe">
                <Animations>
                    <OnShown>
                        <ScriptAction Script="setRcpScrollPos();" />
                    </OnShown>    
                </Animations>
            </asp:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <%--    
    <uc1:cntModal runat="server" ID="cntModal" WidthType="Large">
        <ContentTemplate>
        </ContentTemplate>
    </uc1:cntModal>
    --%>
</div>



<asp:SqlDataSource ID="dsKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare @od datetime
set @od = dbo.getdate(GETDATE())
select P.IdPracownika as Value, 
REPLICATE('&nbsp;', Hlevel * 4) +
P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') as Text, P.Nazwisko, P.Imie, P1.Sort 
from dbo.fn_GetTree2(@userId, 1, @od) P
outer apply (select case when P.IdPracownika = @userId then 1 else 2 end Sort) P1
where Kierownik = 1
--order by Sort, Nazwisko, Imie
order by P.SortPath
">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
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
/*declare @data datetime
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
order by Sort, Nazwisko, Imie*/

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









<asp:SqlDataSource ID="dsConditions" runat="server" SelectCommand="
select * from rcpWarunki where getdate() between DataOd and isnull(DataDo, '20990909') and Wymagany in (1, {0}) and Grupa = 'PPACC' order by Kolejnosc
" />