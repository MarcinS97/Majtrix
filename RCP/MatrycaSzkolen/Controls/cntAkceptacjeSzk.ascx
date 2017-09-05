<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAkceptacjeSzk.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntAkceptacjeSzk" %>

<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="cc" TagName="cntSqlTabs" %>

<asp:HiddenField ID="hidStatus" runat="server" Visible="false" Value="1" />
<asp:HiddenField ID="hidSelectedTab" runat="server" Visible="false" />

<div class="cntAkceptacjeSzk">
    <cc:cntSqlTabs ID="Tabs" runat="server"
        AddCssClass="tabKwal"
        DataTextField="Name"
        DataValueField="Id"
        SQL="
select 1 as Id, 'W trakcie szkolenia' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 2 as Sort from (select 1 x) x
outer apply (select nullif(count(*), 0) c from Certyfikaty c where c.Status = 1) oa
union all
select 2 as Id, 'Przeszkoleni - niezatwierdzone' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 3 as Sort from (select 1 x) x 
outer apply (select nullif(count(*), 0) c from Certyfikaty c where c.Status = 2) oa
union all
select 3 as Id, 'Przeszkoleni' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 4 as Sort from (select 1 x) x
outer apply (select nullif(count(*), 0) c from Certyfikaty c where c.Status = 3) oa
union all 
select -1 as Id, 'Odrzuceni' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 5 as Sort from (select 1 x) x
outer apply (select nullif(count(*), 0) c from Certyfikaty c where c.Status = -1) oa
order by Sort, Id
"
        OnSelectTab="Tabs_SelectTab"
        OnDataBound="Tabs_DataBound" />

    <hr />

    <div class="form-inline">
        <div class="form-group" style="margin-right: 12px;">
            <label>Pracownik: </label>
            <asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownik" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
            <asp:SqlDataSource ID="dsPracownik" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort from Pracownicy where Id in (select IdPracownika from Certyfikaty where Status = @SelectedTab)
order by Sort, Name
            " >
                <SelectParameters>
                    <asp:ControlParameter Name="SelectedTab" Type="Int32" ControlID="hidSelectedTab" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <div class="form-group" style="margin-right: 12px;">
            <label>Szkolenie: </label>
            <asp:DropDownList ID="ddlSzkolenie" runat="server" DataSourceID="dsSzkolenie" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
            <asp:SqlDataSource ID="dsSzkolenie" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select u.Id, u.Nazwa Name, 1 as Sort 
from Uprawnienia u
left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId
where u.Id in (select IdUprawnienia from Certyfikaty where Status = @SelectedTab)
order by Sort, Name
            " >
                <SelectParameters>
                    <asp:ControlParameter Name="SelectedTab" Type="Int32" ControlID="hidSelectedTab" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
                <div class="form-group">
                    <label>Trener: </label>
                    <asp:DropDownList ID="ddlTrener" runat="server" DataSourceID="dsTrener" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
                    <asp:SqlDataSource ID="dsTrener" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort from Pracownicy where dbo.GetRightId(Rights, 82) = 1
order by Sort, Name
            " ></asp:SqlDataSource>
                </div>
    </div>
    <hr />



    <%--<h4>Certyfikaty do potwierdzenia</h4>--%>
    <asp:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id"
        DataSourceID="dsList" CssClass="table" GridLines="None" EmptyDataRowStyle-CssClass="empty">
        <Columns>
            <asp:BoundField DataField="KadryId" HeaderText="Nr Ewid." InsertVisible="False" ReadOnly="True" SortExpression="KadrySort" />
            <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" SortExpression="Pracownik" />
            <asp:BoundField DataField="Szkolenie" HeaderText="Szkolenie" SortExpression="Szkolenie" />
            <asp:BoundField DataField="Trener" HeaderText="Trener" SortExpression="Trener" />
            <asp:BoundField DataField="DataRozp" HeaderText="Data rozpoczęcia" SortExpression="DataRozp" />
            <asp:BoundField DataField="DataWazn" HeaderText="Data ważności" SortExpression="DataWazn" />
            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />

            <asp:TemplateField ItemStyle-Width="200px">
                <ItemTemplate>
                    <asp:Button ID="ButtonYes" runat="server" CommandName="Yes" OnCommand="Button_Command"
                        CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("ButtonYesName") %>'
                        CssClass="btn btn-sm btn-primary" Visible='<%# Convert.ToBoolean(Eval("ButtonYesVisible")) %>' />
                    <asp:Button ID="ButtonNo" runat="server" CommandName="No" OnCommand="Button_Command" 
                        CommandArgument='<%# Eval("Id") %>' Text="Odrzuć" CssClass="btn btn-sm btn-danger" 
                        Visible='<%# Convert.ToBoolean(Eval("ButtonNoVisible")) %>'
                        />
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:ButtonField ButtonType="Button" Text='<%# Eval("StatusName") %>' ControlStyle-CssClass="btn btn-sm btn-success" />--%>
        </Columns>
        <EmptyDataTemplate>
            <div class="well well-sm">Brak danych</div>
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
        SelectCommand="

SELECT 
 c.Id
, p.KadryId
, CONVERT(int, p.KadryId) KadrySort
, c.IdPracownika
, c.Status
, c.DataRozpoczecia
, c.DataWaznosci
, p.Nazwisko + ' ' + p.Imie Pracownik 
, CONVERT(VARCHAR(10), c.DataRozpoczecia, 20) DataRozp
, CONVERT(VARCHAR(10), c.DataWaznosci, 20) DataWazn
, cs.Nazwa as StatusName
, case when c.Status != 3 then 1 else 0 end as ButtonYesVisible
, case when c.Status in (2, 3) then 1 else 0 end as ButtonNoVisible
, case 
    when c.Status = 0 then 'Akceptuj'
    when c.Status = 1 then 'Przeszkolono' 
    when c.Status = 2 then 'Akceptuj'
    when c.Status = 3 then 'Zatwierdź' 
    when c.Status = -1 then 'Przywróć'
    else 'Akceptuj' end as ButtonYesName
, u.Nazwa Szkolenie
, t.Nazwisko + ' ' + t.Imie Trener
FROM Certyfikaty c
left join Pracownicy p on p.Id = c.IdPracownika
left join Pracownicy t on t.Id = c.IdTrenera
left join Uprawnienia u on u.Id = c.IdUprawnienia
left join msCertyfikatyStatus cs on cs.Id = c.Status
WHERE 
(c.Status = @SelectedTab) 
and (p.Id = @pracFilter or @pracFilter is null)
and (c.IdUprawnienia = @szkolFilter or @szkolFilter is null)
and (c.IdTrenera = @trenerFilter or @trenerFilter is null)
ORDER BY [DataRozpoczecia] DESC">
        <SelectParameters>
            <%--<asp:ControlParameter Name="Status" Type="Int32" ControlID="hidStatus" PropertyName="Value" />--%>
            <asp:ControlParameter Name="SelectedTab" Type="Int32" ControlID="hidSelectedTab" PropertyName="Value" />
            <asp:ControlParameter Name="pracFilter" Type="Int32" ControlID="ddlPracownik" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="szkolFilter" Type="Int32" ControlID="ddlSzkolenie" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="trenerFilter" Type="Int32" ControlID="ddlTrener" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

</div>

<asp:Button ID="btnYesConfirm" runat="server" CssClass="button_postback" OnClick="btnYesConfirm_Click" />
<asp:Button ID="btnNoConfirm" runat="server" CssClass="button_postback" OnClick="btnNoConfirm_Click" />


<asp:SqlDataSource ID="dsAccept" runat="server" SelectCommand="update Certyfikaty set Status = {0} where Id = {1}" />
<asp:SqlDataSource ID="dsReject" runat="server" SelectCommand="update Certyfikaty set Status = {0} where Id = {1}" />
<asp:SqlDataSource ID="dsRemoveSurveys" runat="server" SelectCommand="update msAnkiety set Status = -1 where IdCertyfikatu = {0}" />

<asp:SqlDataSource ID="dsAcceptTraining" runat="server"
    SelectCommand="
declare @ankietaPrac bit
declare @ankietaKier bit
declare @ankietaKier2 bit
declare @uprId int
declare @id int

set @id = {0}

set @uprId = (select top 1 IdUprawnienia from Certyfikaty where Id = @id)

update Certyfikaty set DataAkceptacji2 = {1}, IdAkceptujacego2 = {2}, IdAkceptujacegoZast2 = {3} where Id = {0}

set @ankietaPrac = (select top 1 EwaluacjaPrac from Uprawnienia where Id = @uprId)
set @ankietaKier = (select top 1 EwaluacjaKier from Uprawnienia where Id = @uprId)
set @ankietaKier2 = (select top 1 EwaluacjaKier2 from Uprawnienia where Id = @uprId)


if @ankietaPrac = 1
    insert into msAnkiety (IdCertyfikatu, Typ, Status, DataRozpoczecia) values (@id, 0, 0, GETDATE())

if @ankietaKier = 1
    insert into msAnkiety (IdCertyfikatu, Typ, Status, DataRozpoczecia) values (@id, 1, 0, GETDATE())

if @ankietaKier2 = 1
    insert into msAnkiety (IdCertyfikatu, Typ, Status, DataRozpoczecia) values (@id, 2, 0, GETDATE())
" />


<asp:SqlDataSource ID="dsSurveys" runat="server" SelectCommand="select * from msAnkiety where IdCertyfikatu = {0}" />