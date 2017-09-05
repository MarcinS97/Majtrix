<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAkceptacjeOceny.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntAkceptacjeOceny" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<div class="cntAkceptacjeOceny">
    <hr />
    <div class="form-inline">
        <div class="form-group" style="margin-right: 12px;">
            <label>Pracownik: </label>
            <asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownik" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
            <asp:SqlDataSource ID="dsPracownik" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort from Pracownicy where Id in (select IdPracownika from msOceny where GETDATE() between DataOd and ISNULL(DataDo, '20990909') and Status = -2)
order by Sort, Name
            " />
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
where u.Id in (select IdUprawnienia from msOceny where GETDATE() between DataOd and ISNULL(DataDo, '20990909') and Status = -2)
order by Sort, Name
            " />
        </div>
    </div>
    <hr />

    <asp:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id"
        DataSourceID="dsList" CssClass="table" GridLines="None" EmptyDataRowStyle-CssClass="empty">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
            <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" SortExpression="Pracownik" />
            <asp:BoundField DataField="Uprawnienie" HeaderText="Uprawnienie" SortExpression="Uprawnienie" />
            <asp:BoundField DataField="DateFrom" HeaderText="Data od" SortExpression="DateFrom" />
            <asp:BoundField DataField="DateTo" HeaderText="Data do" SortExpression="DateTo" />
            <asp:BoundField DataField="Korektor" HeaderText="Wnoszący o korektę" SortExpression="Korektor" />
            <asp:BoundField DataField="Ocena aktualna" HeaderText="Ocena aktualna" SortExpression="Ocena aktualna" />
            <asp:BoundField DataField="Ocena wyliczona" HeaderText="Ocena wyliczona" SortExpression="Ocena wyliczona" />
            <%--  <asp:BoundField DataField="Korekta" HeaderText="Korekta" SortExpression="Korekta" />
            --%>
            <asp:TemplateField HeaderText="Korekta" ItemStyle-Width="100px">
                <ItemTemplate>
                    <asp:TextBox ID="tbOcena" runat="server" Text='<%# Eval("Korekta") %>' CssClass="form-control" MaxLength="1" />
                    <asp:RequiredFieldValidator ID="reqOcena" runat="server" ControlToValidate="tbOcena" ErrorMessage="Pole wymagane" Display="Dynamic" ValidationGroup="xvg" />
                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbOcena" FilterType="Custom" ValidChars="01234" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Uwagi">
                <ItemTemplate>
                    <asp:TextBox ID="tbUwagi" runat="server" Text='<%# Eval("Uwagi") %>' CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="reqUwagi" runat="server" ControlToValidate="tbUwagi" ErrorMessage="Pole wymagane" Display="Dynamic" ValidationGroup="rvg" />
                </ItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" CommandArgument='<%# Container.DataItemIndex %>' Text="Akceptuj" CssClass="btn btn-sm btn-success" ValidationGroup="xvg" />
                    <asp:Button ID="btnReject" runat="server" OnClick="btnReject_Click" CommandArgument='<%# Container.DataItemIndex %>' Text="Odrzuć" CssClass="btn btn-sm btn-danger" ValidationGroup="rvg" />
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
k.Id
, p.KadryId [Nr Ewid.]
, p.Nazwisko + ' ' + p.Imie [Pracownik]
, u.Nazwa [Uprawnienie]
, case
    when k.Status =  1 then CONVERT(varchar, k.Ocena)
    when k.Status = -2 then CONVERT(varchar, o.Ocena)
  end [Ocena Aktualna]
, CONVERT(varchar, o.Ocena) [Ocena wyliczona]
, k.Ocena [Korekta]
, k.Uwagi as Uwagi
, p2.Nazwisko + ' ' + p2.Imie + ISNULL(' (' + p2.KadryId + ')', '') Korektor
, convert(varchar(10), k.DataOd, 20) DateFrom
, convert(varchar(10), k.DataDo, 20) DateTo
, o.DataAkceptacji
from msOceny o
inner join msOceny k on GETDATE() between k.DataOd and ISNULL(k.DataDo, '20990909') and k.IdPracownika = o.IdPracownika and k.IdUprawnienia = o.IdUprawnienia and k.Ocena != o.Ocena and (k.Status = 1 or k.Status = -2)
left join Pracownicy p on p.Id = o.IdPracownika
left join Pracownicy p2 on p2.Id = k.IdKorektora
left join Uprawnienia u on u.Id = k.IdUprawnienia
where GETDATE() between o.DataOd and ISNULL(o.DataDo, '20990909') and o.Status = 0
union
select
k.Id
, p.KadryId
, p.Nazwisko + ' ' + p.Imie
, u.Nazwa
, 'Brak'
, 'Brak'
, k.Ocena [Korekta]
, k.Uwagi as Uwagi
, p2.Nazwisko + ' ' + p2.Imie + ISNULL(' (' + p2.KadryId + ')', '') Korektor
, convert(varchar(10), k.DataOd, 20) DateFrom
, convert(varchar(10), k.DataDo, 20) DateTo
, k.DataAkceptacji
from msOceny k
left join msOceny o on GETDATE() between o.DataOd and ISNULL(o.DataDo, '20990909') and k.IdPracownika = o.IdPracownika and k.IdUprawnienia = o.IdUprawnienia and k.Ocena != o.Ocena and (k.Status = 1 or k.Status = -2) and (o.Status = 0 /*or o.Status = 2*/)
left join Pracownicy p on p.Id = k.IdPracownika
left join Pracownicy p2 on p2.Id = k.IdKorektora
left join Uprawnienia u on u.Id = k.IdUprawnienia
where GETDATE() between k.DataOd and ISNULL(k.DataDo, '20990909') and k.Status = -2 and o.Id is null
and (k.Id = @pracFilter or @pracFilter is null)
and (k.IdUprawnienia = @szkolFilter or @szkolFilter is null)
order by DataAkceptacji desc
">
        <SelectParameters>
            <%--<asp:ControlParameter Name="Status" Type="Int32" ControlID="hidStatus" PropertyName="Value" />--%>
            <%--<asp:ControlParameter Name="SelectedTab" Type="Int32" ControlID="hidSelectedTab" PropertyName="Value" />--%>
            <asp:ControlParameter Name="pracFilter" Type="Int32" ControlID="ddlPracownik" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="szkolFilter" Type="Int32" ControlID="ddlSzkolenie" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

</div>

<asp:Button ID="btnAcceptConfirm" runat="server" CssClass="button_postback" OnClick="btnAcceptConfirm_Click" ValidationGroup="xvg" />
<asp:Button ID="btnRejectConfirm" runat="server" CssClass="button_postback" OnClick="btnRejectConfirm_Click" ValidationGroup="rvg" />

<asp:SqlDataSource ID="dsAccept" runat="server" SelectCommand="update msOceny set Status = 2, Ocena = {1}, Uwagi = {2}, IdAkceptujacego = {3}, DataAkceptacji = GETDATE() where Id = {0}" />
<asp:SqlDataSource ID="dsReject" runat="server" SelectCommand="update msOceny set Status = -1, Ocena = {1}, Uwagi = {2} where Id = {0}" />
