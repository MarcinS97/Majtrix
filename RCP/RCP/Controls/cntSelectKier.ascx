<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSelectKier.ascx.cs" Inherits="HRRcp.RCP.Controls.cntSelectKier" %>

<div id="paSelectKier" runat="server" class="cntSelectKier">
    <asp:HiddenField ID="hidKierId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
    <asp:DropDownList ID="ddlKier" runat="server" CssClass="form-control ddlKier" DataSourceID="dsKier" DataValueField="Value" DataTextField="Text" AutoPostBack="true" OnDataBound="ddlKier_DataBound" OnSelectedIndexChanged="ddlKier_SelectedIndexChanged" Visible="true" />
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
">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>




