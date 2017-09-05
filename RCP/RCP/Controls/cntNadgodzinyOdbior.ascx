<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntNadgodzinyOdbior.ascx.cs" Inherits="HRRcp.RCP.Controls.cntNadgodzinyOdbior" %>

<div id="ctNadgodzinyOdbior" runat="server" class="cntNadgodzinyOdbior">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div style="max-width: 1400px;">
                <asp:GridView ID="gvList" runat="server" DataSourceID="dsList" CssClass="table" GridLines="None" DataKeyNames="Id" OnRowDataBound="gvList_RowDataBound"
                    AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" EmptyDataRowStyle-CssClass="empty">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hidStatus" runat="server" Visible="false" Value='<%# Eval("StatusId") %>' />
                                <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelect_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NrEwid" HeaderText="Nr Ewid." SortExpression="NrEwid" />
                        <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" SortExpression="Pracownik" />
                        <asp:BoundField DataField="DataWysylki" HeaderText="Data wysyłki" SortExpression="DataWysylki" />
                        <asp:BoundField DataField="Data" HeaderText="Data" SortExpression="Data" />
                        <asp:BoundField DataField="Zmiana" HeaderText="Zmiana" SortExpression="Zmiana" />
                        <asp:BoundField DataField="Nadg50" HeaderText="Nadgodziny 50" SortExpression="Nadg50" />
                        <asp:BoundField DataField="Nadg100" HeaderText="Nadgodziny 100" SortExpression="Nadg100" />
                        <asp:BoundField DataField="Nadg" HeaderText="Nadgodziny" SortExpression="Nadg" />
                        <asp:BoundField DataField="Noc" HeaderText="Noc" SortExpression="Noc" />
                        <asp:BoundField DataField="Powod" HeaderText="Powód" SortExpression="Powod" />
                        <asp:BoundField DataField="Uwagi" HeaderText="Uwagi" SortExpression="Uwagi" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnCollect" runat="server" OnClick="btnCollect_Click" CommandArgument='<%# Eval("Id") %>' Text="Odbierz" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="well well-sm">Brak danych</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>


            <div id="divCollect" runat="server">
                <asp:Button ID="btnCollectMany" runat="server" OnClick="btnCollectMany_Click" Text="Odbierz wnioski" CssClass="btn btn-default" Enabled="false" />
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</div>




<asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
--declare @filter int = 1

/*select /*p.Nazwisko + ' ' + p.Imie Pracownik, p.KadryId,  */
convert(varchar(10), ha.Data, 20) [data:-]
, p.KierId [kierId:-]
, p.KierownikNI Przelozony, p2.Nazwisko + ' ' + p2.Imie Wnioskujacy, ha.Data [Data:D], count(*) Ilość
, 'Pokaż' [:;control|cmd:edit @kierId @data|Pokaż plan]
from rcpHarmonogramAcc ha
left join VPrzypisaniaNaDzis p on p.Id = ha.IdPracownika
left join Pracownicy p2 on p2.Id = ha.IdTworzacego
where (@filter is null or ha.Status = @filter)
group by p.KierId, p.KierownikNI, p2.Nazwisko, p2.Imie, ha.Data*/










select 
  nw.Id
, p.KadryId NrEwid
, p.Nazwisko + ' ' + p.Imie Pracownik
, convert(varchar(10), nw.Data, 20) Data
, Powod
, nws.Nazwa Status
, Nadg50
, Nadg100
, Nadg
, z.Nazwa Zmiana
, Uwagi
, Noc
, nw.Status StatusId
, nw.DataUtworzenia DataWysylki
from rcpNadgodzinyWnioski nw
left join Pracownicy p on p.Id = nw.IdPracownika
left join rcpNadgodzinyWnioskiStatus nws on nws.Id = nw.Status
left join Zmiany z on z.Id = nw.IdZmiany
where nw.RodzajId = 2 and nw.Status = 2
" />
