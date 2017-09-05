<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAkceptacjeEw.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntAkceptacjeEw" %>

<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="cc" TagName="cntSqlTabs" %>


<asp:HiddenField ID="hidSelectedTab" runat="server" Visible="false" />

<div class="cntAkceptacjeEw">

    <cc:cntSqlTabs ID="Tabs" runat="server"
        AddCssClass="tabKwal"
        DataTextField="Name"
        DataValueField="Id"
        SQL="
select 1 as Id, 'Niezatwierdzone' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 1 as Sort from (select 1 x) x
outer apply (select nullif(count(*), 0) c from msAnkiety c where c.Status = 1) oa
union all
select 2 as Id, 'Zatwierdzone' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 2 as Sort from (select 1 x) x 
outer apply (select nullif(count(*), 0) c from msAnkiety c where c.Status = 2) oa
union all
select -1 as Id, 'Odrzucone' + isnull(' (' + convert(varchar, oa.c) + ')', '') as Name, 3 as Sort from (select 1 x) x 
outer apply (select nullif(count(*), 0) c from msAnkiety c where c.Status = -1) oa
order by Sort, Id
"
        OnSelectTab="Tabs_SelectTab" />
    <hr />
        <div class="form-inline">
        <div class="form-group" style="margin-right: 12px;">
            <label>Pracownik: </label>
            <asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownik" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
            <asp:SqlDataSource ID="dsPracownik" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort from Pracownicy where Id in (select c.IdPracownika from msAnkiety a left join Certyfikaty c on c.Id = a.IdCertyfikatu where a.Status = @SelectedTab)
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
where u.Id in (select c.IdUprawnienia from msAnkiety a left join Certyfikaty c on c.Id = a.IdCertyfikatu where a.Status = @SelectedTab)
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
    <asp:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id"
        DataSourceID="dsList" CssClass="table" GridLines="None" EmptyDataRowStyle-CssClass="empty">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
            <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" SortExpression="Pracownik" />
            <asp:BoundField DataField="Nazwa" HeaderText="Nazwa szkolenia" SortExpression="Nazwa" />
            <asp:BoundField DataField="TypName" HeaderText="Typ ankiety" SortExpression="TypName" />

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="bntAccept" runat="server" Text="Akceptuj" OnClick="bntAccept_Click" CssClass="btn btn-sm btn-success" CommandArgument='<%# Eval("Id") %>' Visible='<%# Convert.ToBoolean(Eval("AcceptVisible")) %>' />
                    <asp:Button ID="btnReject" runat="server" Text="Odrzuć" OnClick="btnReject_Click" CssClass="btn btn-sm btn-danger" CommandArgument='<%# Eval("Id") %>' Visible='<%# Convert.ToBoolean(Eval("RejectVisible")) %>' />
                    
                    
                    <asp:LinkButton ID="btnPreview" runat="server" Text="Podgląd" OnClick="btnPreview_Click" CssClass="btn btn-sm btn-default" CommandArgument='<%# Eval("Id") + ";" + Eval("Typ") %>' />

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
select 
	a.Id
, c.IdPracownika
, p.Nazwisko + ' ' + p.Imie Pracownik
, c.NazwaCertyfikatu Nazwa
, a.Typ
, case 
	when a.Typ = 0 then 'Ankieta pracownika'
	when a.Typ = 1 then 'Ankieta kierownika'
	when a.Typ = 2 then 'Ankieta kierownika 2'
end as TypName
, case when a.Status = 1 then 1 else 0 end as AcceptVisible
, case when a.Status = 1 then 1 else 0 end as RejectVisible
from msAnkiety a
left join Certyfikaty c on c.Id = a.IdCertyfikatu
left join Pracownicy p on p.Id = c.IdPracownika
where 
a.Status = @SelectedTab        
and (p.Id = @pracFilter or @pracFilter is null)
and (c.IdUprawnienia = @szkolFilter or @szkolFilter is null)
and (c.IdTrenera = @trenerFilter or @trenerFilter is null)
        ">
        <SelectParameters>
            <%--<asp:ControlParameter Name="Status" Type="Int32" ControlID="hidStatus" PropertyName="Value" />--%>
            <asp:ControlParameter Name="SelectedTab" Type="Int32" ControlID="hidSelectedTab" PropertyName="Value" />
            <asp:ControlParameter Name="pracFilter" Type="Int32" ControlID="ddlPracownik" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="szkolFilter" Type="Int32" ControlID="ddlSzkolenie" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="trenerFilter" Type="Int32" ControlID="ddlTrener" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>


</div>

<asp:Button ID="btnAcceptConfirm" runat="server" OnClick="bntAcceptConfirm_Click" CssClass="button_postback" />
<asp:Button ID="btnRejectConfirm" runat="server" OnClick="btnRejectConfirm_Click" CssClass="button_postback" />
<%--<asp:Button ID="bntAcceptConfirm" runat="server" OnClick="bntAcceptConfirm_Click" CssClass="button_postback" />--%>
<asp:SqlDataSource ID="dsAccept" runat="server" SelectCommand="update msAnkiety set Status = 2 where Id = {0}" />
<asp:SqlDataSource ID="dsReject" runat="server" SelectCommand="update msAnkiety set Status = -1 where Id = {0}" />

