<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlMenuEdit.ascx.cs" Inherits="HRRcp.Portal.Controls.Adm.cntSqlMenuEdit" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="uc1" TagName="cntSqlTabs" %>

<div id="paSqlMenuEdit" runat="server" class="cntSqlMenuEdit" >
    <asp:HiddenField ID="hidId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidGrupa" runat="server" Visible="false" />
    <uc1:cntModal runat="server" ID="cntModal" Backdrop="false" Keyboard="false" ShowFooter="true" CloseButtonText="Anuluj" Width="900px" >
        <HeaderTemplate>
            <h4 class="modal-title">
                <i class="fa fa-link"></i>
                <asp:Label ID="lbTitleNazwa" runat="server" Visible="false"></asp:Label>
                <asp:Literal ID="ltTitleEdit" runat="server" Text="- Edycja" Visible="false" />
                <asp:Literal ID="ltTitleInsert" runat="server" Text="Dodaj nowy" />
            </h4>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
<%--  
                    <uc1:cntSqlTabs runat="server" ID="cntSqlTabs" Items="Parametry|PAR|Wejścia|WE|Wyjścia|WY" OnSelectTab="cntSqlTabs_SelectTab" />
                    <asp:MultiView ID="mvData" runat="server">
                        <asp:View ID="PAR" runat="server">
                        </asp:View>
                        <asp:View ID="WE" runat="server">

                        </asp:View>
                        <asp:View ID="WY" runat="server">

                        </asp:View>
                    </asp:MultiView>
--%>
                    <uc1:dbField runat="server" ID="lbPola"         Label="Pola:"           Type="lb"    StVisible="1" />

                    <uc1:dbField runat="server" ID="ddlGrupa"       Label="Grupa:"          Type="ddl"   StVisible="3" Fields="Grupa,GrupaNazwa" Rq="true" DataSourceID="dsGrupy" />
                    <uc1:dbField runat="server" ID="ddlParent"      Label="Parent:"         Type="ddl"   StVisible="3" Fields="ParentId,ParentMenuText" DataSourceID="dsParent" />
                    <uc1:dbField runat="server" ID="tbMenuText"     Label="Text:"           Type="tb"    StVisible="3" MaxLength="200" Rq="true" />
                    <uc1:dbField runat="server" ID="tbMenuTextEN"   Label="Text EN:"        Type="tb"    StVisible="3" MaxLength="200" />
                    <uc1:dbField runat="server" ID="tbToolTip"      Label="ToolTip:"        Type="tb"    StVisible="3" MaxLength="500" />
                    <uc1:dbField runat="server" ID="tbToolTipEN"    Label="ToolTip EN:"     Type="tb"    StVisible="3" MaxLength="500" />
                    <uc1:dbField runat="server" ID="tbCommand"      Label="Command:"        Type="tb"    StVisible="3" MaxLength="500" />
                                                                                                                            
                    <uc1:dbField runat="server" ID="tbKolejnosc"    Label="Kolejnosc:"      Type="tb"    StVisible="3" MaxLength="10" ValidChars="0123456789" />
                    <uc1:dbField runat="server" ID="cbAktywny"      Label="Aktywny"         Type="check" StVisible="3" />
                                                                                                                            
                    <uc1:dbField runat="server" ID="tbImage"        Label="Image:"          Type="tb"    StVisible="3" MaxLength="255" />
                    <uc1:dbField runat="server" ID="tbRights"       Label="Rights:"         Type="tb"    StVisible="3" MaxLength="200" />
                    <uc1:dbField runat="server" ID="tbPar1"         Label="Par1:"           Type="tb"    StVisible="3" MaxLength="200" />
                    <uc1:dbField runat="server" ID="tbPar2"         Label="Par2:"           Type="tb"    StVisible="3" MaxLength="200" />
                                                                                                                            
                    <uc1:dbField runat="server" ID="cbWydruk"       Label="Wydruk"          Type="check" StVisible="3" />
                                                                                                                            
                    <uc1:dbField runat="server" ID="tbSql"          Label="Sql:"            Type="tb"    StVisible="3" TextMode="MultiLine" Rows="10" />
                    <uc1:dbField runat="server" ID="tbSqlParams"    Label="SqlParams:"      Type="tb"    StVisible="3" TextMode="MultiLine" Rows="10" />
                                                                                                                            
                    <uc1:dbField runat="server" ID="tbMode"         Label="Mode:"           Type="tb"    StVisible="3" MaxLength="1" ValidChars="0123" />
                                                                                                                            
                    <uc1:dbField runat="server" ID="tbJavascript"   Label="Javascript:"     Type="tb"    StVisible="3" TextMode="MultiLine" Rows="10" />
                    <uc1:dbField runat="server" ID="tbClass"        Label="Class:"          Type="tb"    StVisible="3" MaxLength="200" />
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btDelete" runat="server" Text="Usuń" CssClass="btn btn-danger pull-left" OnClick="btDelete_Click" Visible="false"/>
            <asp:Button ID="btPola" runat="server" Text="Widoczne pola" CssClass="btn btn-primary pull-left" OnClick="btPola_Click" Visible="true"/>
            <asp:Button ID="btSave" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClick="btSave_Click" ValidationGroup="vgSave"/>
        </FooterTemplate>
    </uc1:cntModal>

    <uc1:cntModal runat="server" ID="cntModalPola" Backdrop="false" Keyboard="false" ShowFooter="true" Title="Widoczność pól" CloseButtonText="Anuluj" Width="400px" >
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <label>0-niewidoczne, 1-widoczne, 2-edycja</label>
                    <asp:TextBox ID="tbPola" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="21"></asp:TextBox>
                    <asp:TextBox ID="tbPolaDefault" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="21" Visible="false" Text="
Id=0
Grupa=2
ParentId=2
MenuText=2
MenuTextEN=2
ToolTip=2
ToolTipEN=2
Command=2
Kolejnosc=2
Aktywny=2
Image=2
Rights=2
Par1=2
Par2=2
Wydruk=2
Sql=2
SqlParams=2
Mode=2
Javascript=2
Class=2
                        ">
                    </asp:TextBox>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btPolaDefault" runat="server" Text="Pokaż wszystkie" CssClass="btn btn-primary pull-left" OnClick="btPolaDefault_Click" />
            <asp:Button ID="btPolaSave" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClick="btPolaSave_Click" ValidationGroup="vgPolaSave"/>
        </FooterTemplate>
    </uc1:cntModal>










    <%-- wywala się - do sprawdzenia --%>
    <uc1:cntModal runat="server" ID="cntModalPola1" Backdrop="false" Keyboard="false" ShowFooter="true" Title="Widoczność pól" Width="400px" Visible="false">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="gvPola" CssClass="table GridView1" runat="server" DataSourceID="dsPola" AllowSorting="true" >
                    </asp:GridView>
                    <asp:Button ID="gvPolaCmd" runat="server" CssClass="button_postback" onclick="gvPolaCmd_Click" />
                    <asp:HiddenField ID="gvPolaCmdPar" runat="server" />
                    <asp:HiddenField ID="gvPolaSelected" runat="server" />
                    <asp:HiddenField ID="gvPolaSelectedId" runat="server" />
                    <asp:HiddenField ID="hidPola" runat="server" Visible="false" />
                </div>
            </div>
            <%-- nie widać ControlParameter jak poza ContentTemplate --%>
            <asp:SqlDataSource ID="dsPola" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="False" 
                SelectCommand="
--declare @grupa nvarchar(50)
--declare @pola nvarchar(max)
--select @pola = Pola from SqlMenuGrupy where Grupa = @grupa

--set @pola = @pola + ',Pola=1'

select 
  c.name [sort:-]
, c.column_id [cid:-]
, c1.status [st:-]
, c1.StNext [stnext:-]
, c.name [Kolumna]
, c1.StVisible [Status:;control|cmd:status @cid @st @stnext|Zmień widoczność pola]
--, s.pole, s.status, s.items
from sys.tables t
left join sys.columns c on c.object_id = t.object_id
left join (
	select 
      s.items
    , s2.pole
    , s2.status
	from CO_HR_DB.dbo.SplitStr(@pola, ',') s
	outer apply (select LTRIM(RTRIM(items)) items) s1
	outer apply (select LEFT(s1.items, LEN(s1.items) - 2) pole, RIGHT(s1.items, 1) status) s2 
) s on s.pole = c.name
outer apply (select ISNULL(s.status, '2') status) s3  -- status domyślny edycja
outer apply (select 
    s3.status
  , case s3.status 
    when '0' then 'Ukryj'
    when '1' then 'Pokaż'
    when '2' then 'Edycja'
    when '3' then 'Edycja2'
    else s.status
    end StVisible
  , case s3.status
    when '0' then '1'
    when '1' then '2'
    when '2' then '3'
    when '3' then '0'
    else s3.status
    end StNext
) c1
where t.name = 'SqlMenu'
order by 2
">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hidGrupa" PropertyName="Value" Name="grupa" Type="String" />      
                    <asp:ControlParameter ControlID="hidPola" Name="pola" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="dsPolaUpdate" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="False" 
                SelectCommand="
declare @cid int
set @cid = {0}

select c.name
-- , c.object_id
from sys.tables t
left join sys.columns c on c.object_id = t.object_id
where t.name = 'SqlMenu' and c.column_id = @cid
"
                UpdateCommand="
declare @grupa nvarchar(50), @pola nvarchar(max)
set @grupa = '{0}'
set @pola  = '{1}'

if exists(select * from SqlMenuGrupy where Grupa = @grupa)
    update SqlMenuGrupy set Pola = @pola where Grupa = @grupa
else
    insert into SqlMenuGrupy (Grupa, Pola) values (@grupa, @pola)
">
            </asp:SqlDataSource>
        </ContentTemplate>
    </uc1:cntModal>


    <%--
    <uc1:cntModal runat="server" ID="cntShowError" Backdrop="false" Keyboard="false" ShowFooter="true" CloseButtonText="Ok" Title="Wystapił błąd" >
        <ContentTemplate>
            <asp:literal id="ltShowErrorMessage" runat="server" ></asp:literal>                                
        </ContentTemplate>
    </uc1:cntModal>
    --%>
</div>

<asp:SqlDataSource ID="dsSqlMenu" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
    SelectCommand="
declare @id int
declare @pid int
declare @grupa nvarchar(200)
set @id = {0}
set @pid = {1}
set @grupa = {2}

select
  m1.ParentId
, m1.Grupa
, m.*
, mp.MenuText [ParentMenuText]
, g.Pola
from (select 1 X) X   -- zeby 1 sql pobranie danych do create załatwić
left join SqlMenu m on m.Id = @id
outer apply (select 
	ISNULL(m.ParentId, ISNULL(@pid, 0)) ParentId
  , ISNULL(m.Grupa, @grupa) Grupa
) m1
left join SqlMenu mp on mp.Id = m1.ParentId
outer apply (select top 1 * from SqlMenuGrupy where Grupa = m1.Grupa) g
    " 
    UpdateCommand=""
    InsertCommand=""
    DeleteCommand="delete from SqlMenu where Id = {0}"
    />

<asp:SqlDataSource ID="dsGrupy" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
    SelectCommand="
select null Grupa, 'wybierz...' GrupaNazwa, 1 Sort
union all                
select distinct Grupa, Grupa GrupaNazwa, 2 Sort 
from SqlMenu 
where Grupa is not null and Grupa != ''
order by Sort, GrupaNazwa
"/>

<asp:SqlDataSource ID="dsParent" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null ParentId, 'wybierz ...' ParentMenuText, 0 Sort, null Kolejnosc
union all
select 0, 'poziom główny', 1 Sort, null Kolejnosc
union all
select Id, MenuText + case when Aktywny = 0 then ' (nieaktywny)' else '' end, 2 Sort, Kolejnosc 
from SqlMenu 
where Grupa = @grupa and (@id is null or Id != @id)
order by Sort, Kolejnosc, ParentMenuText
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidId" PropertyName="Value" Name="id" Type="Int32" />
        <asp:ControlParameter ControlID="hidGrupa" PropertyName="Value" Name="grupa" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsTryb" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null Tryb, 'wybierz ...' Nazwa, 0 Sort, 1 Aktywny, 0 Kolejnosc, 0 Id
union all
select A.Id, A.Nazwa + Act Nazwa, 1 Sort, A.Aktywny, A.Kolejnosc, A.Id
from scKafleTypy A
outer apply (select case when A.Aktywny = 1 then '' else ' (nieaktywne)' end Act) A1
order by Sort, Aktywny desc, Kolejnosc, Id  --, Nazwa
    "/>

<asp:SqlDataSource ID="dsRodzaje" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null Rodzaj, 'wybierz ...' Nazwa, 0 Sort
union all
select 0, 'Rejestracja kodu zlecenia', 1
union all
select 1, 'Rejestracja ilości', 2
union all
select 2, 'Rejestracja kodu zlecenia - Błędy Wiring', 3
union all
select 3, 'Rejestracja kodu zlecenia - Wyjaśnienia', 4
    "/>

<asp:SqlDataSource ID="dsPotwierdzenia" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select 0 Potwierdzenia, 'Potwierdzaj duplikaty' Nazwa, 0
union all
select 1, 'Potwierdzaj wszystko', 1
union all
select 2, 'Bez potwierdzania', 2
    "/>



