<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKartyTymczasowe.ascx.cs" Inherits="HRRcp.Controls.cntKartyTymczasowe" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>

<div id="paKartyTymczasowe" runat="server" class="cntKartyTymczasowe">
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
    <div id="paFilter" runat="server" class="filter" visible="true">
        <div class="filterabs input-group">
            <span class="input-group-addon addon-default">Wyszukaj pracownika / numer karty:</span>
            <asp:TextBox ID="tbSearch" CssClass="textbox form-control" runat="server" ></asp:TextBox>
            <div class="input-group-btn">
                <asp:LinkButton ID="lbtClear" runat="server" CssClass="btn btn-default btn-search-clear" Visible="true" ToolTip="Czyść..." >
                    <i class="glyphicon glyphicon-erase"></i>
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />            
            <div class="tabs">
                <asp:Menu ID="tabMode" runat="server" Orientation="Horizontal" 
                    onmenuitemclick="tabMode_MenuItemClick" >
                    <StaticMenuStyle CssClass="tabsStrip" />
                    <StaticMenuItemStyle CssClass="tabItem" />
                    <StaticSelectedStyle CssClass="tabSelected" />
                    <StaticHoverStyle CssClass="tabHover" />
                    <Items>
                        <asp:MenuItem Text="Wydawanie" Value="1" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="Zwroty" Value="2" ></asp:MenuItem>
                        <asp:MenuItem Text="Historia" Value="3"></asp:MenuItem>
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
            <div class="pracownicy">
                <asp:GridView ID="gvPracownicy" runat="server" DataSourceID="SqlDataSource1" PagerSettings-Mode="NumericFirstLast" PageSize="10" AllowPaging="true" AllowSorting="true" CssClass="GridView1"
                    EmptyDataText="Brak danych"
                    OnLoad="gvPracownicy_Load" >
                </asp:GridView>
                <div class="pager">
                    <div class="pagerabs">
                        <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <span class="count">Pokaż na stronie:</span>
                        <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" CssClass="form-control"
                            OnChange="showAjaxProgress();"
                            OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                            <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:Button ID="gvPracownicyCmd" runat="server" CssClass="button_postback" Text="Button" onclick="gvPracownicyCmd_Click" />
                <asp:HiddenField ID="gvPracownicyCmdPar" runat="server" />
                <asp:HiddenField ID="gvPracownicySelected" runat="server" />
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:cntModal runat="server" ID="cntModalEdit" Backdrop="false" Keyboard="false" ShowFooter="true" CloseButtonText="Anuluj" >
        <HeaderTemplate>
            <h4><asp:Literal ID="ltTitle" runat="server"></asp:Literal></h4>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-4 col-avatar">
                    <uc1:cntAvatar runat="server" ID="cntAvatar" Custom="true" />
                </div>
                <div class="col-md-8">
                    <label>Pracownik:</label>
                    <asp:Label ID="lbPracownik" runat="server" CssClass="value"></asp:Label><br />
                    <label>Nr ewid.:</label>
                    <asp:Label ID="lbNrEwid" runat="server" CssClass="value"></asp:Label><br /><br />
                    <div id="paWydanie" runat="server" class="row1" visible="false">
                        <label title="Nr karty (RcpId)">Nr karty:</label>
                        <asp:TextBox ID="tbNrKarty" runat="server" ValidationGroup="vgSave" MaxLength="8" CssClass="form-control" Visible="false"></asp:TextBox>                
                        <asp:DropDownList ID="ddlNrKarty" runat="server" CssClass="form-control" DataSourceID="dsKartyTmpList" DataTextField="Text" DataValueField="Value" Visible="true"></asp:DropDownList>
                    </div>
                    <div id="paZwrot" runat="server" class="row1" visible="false">
                        <label title="Nr karty (RcpId)">Nr karty:</label>
                        <asp:Label ID="lbNrKarty" runat="server" CssClass="value"></asp:Label><br /><br />                
                        <label>Data zwrotu:</label>
                        <uc1:DateEdit runat="server" ID="deData" ValidationGroup="vgSave" CssClass="form-control"/>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btWydaj" runat="server" Text="Wydaj" ValidationGroup="vgSave" CssClass="btn btn-success" OnClick="btSave_Click"/>
            <asp:Button ID="btZwrot" runat="server" Text="Zwróć" ValidationGroup="vgSave" CssClass="btn btn-success" OnClick="btSave_Click"/>
        </FooterTemplate>
    </uc1:cntModal>  
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    OnSelected="SqlDataSource1_Selected"
    SelectCommand="
/*
declare @mode int  -- 1 wydawanie, 2 zwroty, 3 archiwum
declare @selected varchar(max)
declare @search nvarchar(250)
set @mode = 1
set @search = '516'
*/

declare @s1 nvarchar(250)
declare @s2 nvarchar(250)
select @s1 = items + '%' from dbo.SplitStr(RTRIM(@search), ' ') where idx = 0
select @s2 = items + '%' from dbo.SplitStr(RTRIM(@search), ' ') where idx = 1

declare @data datetime
set @data = dbo.getdate(GETDATE())

select 
  P.Id [pracId:-]
, PK.Id [pkId:-]
, P.Nazwisko [nn:-]
, P.Imie [ii:-]
, P.KadryId [nrew:-]
, W.Nazwisko + ' ' + W.Imie [wystawil:-]
--, SEL.items [Zaznacz:CBH;check|@pracId|Zaznacz osobę]
, P.Nazwisko + ' ' + P.Imie 
+ case when P.DataZwol is not null then ' (Zwolniony: ' + convert(varchar(10), P.DataZwol, 20) + ')' else '' end [Pracownik]
, P.KadryId [Nr ewid.]
, T1.NrKarty [Nr Karty (RcpId)]
, PK.Od [Data wydania:D]
, PK.Do [Data zwrotu:D]
--, PK1.Termin [Termin zwrotu 2]
, PK2.Status [Status]
, PK2.Action [:;controlx|cmd:action @pracId @pkId|]
--, P.Status, P.DataZwol
from Pracownicy P
outer apply (select items from dbo.SplitInt(@selected,',') where items = P.Id) SEL
left join PracownicyKarty PK on PK.IdPracownika = P.Id and PK.Status in (1,2) -- identyfikatory zastępcze
left join Identyfikatorytymczasowe T on T.RcpId = PK.RcpId
outer apply (select convert(varchar, PK.RcpId) RcpId) PK1
outer apply (select T.Numer + ISNULL(' ' + T.Opis, '') 
	+ ' (' + ISNULL(T.NrKarty + ', ' + PK1.RcpId, PK1.RcpId) + ')'	
	NrKarty) T1
left join Pracownicy W on W.Id = PK.WydalId
outer apply (select 
	  case when PK.Do is null then 1 else 0 end Bezterminowo
	, case when PK.Do is null then 'bezterminowo' else convert(varchar(10), PK.Do, 20) end Termin
	, case PK.Status when 1 then 'Wydano' when 2 then 'Zwrócono' else 'Inny: ' + convert(varchar, PK.Status) end Status
    , case @mode when 1 then 'Wydaj' when 2 then 'Zwróć' else null end Action
	) PK2
where 
    (
	(@mode = 1 and ISNULL(P.DataZwol, '20990909') &gt; DATEADD(D, -7, @data))  -- 7 dni po zwolnieniu jeszcze widać
 or (@mode = 2 and PK.Status = 1)
 or (@mode = 3 and PK.Status = 2)
    )
and (@mode != 1 and @search is null
  or @s2 is     null and (P.Nazwisko like @s1 or P.Imie like @s1 or P.KadryId like @s1 or T1.NrKarty like @s1 or PK.NrKarty like @s1 or PK1.RcpId like @s1)
  or @s2 is not null and (P.Nazwisko like @s1 and P.Imie like @s2
                       or P.Nazwisko like @s2 and P.Imie like @s1
					   or P.KadryId like @s1 or T1.NrKarty like @s1 or PK.NrKarty like @s1 or PK1.RcpId like @s1
					   or P.KadryId like @s2 or T1.NrKarty like @s2 or PK.NrKarty like @s2 or PK1.RcpId like @s2					   
					   )
	)
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="tabMode" Name="mode" PropertyName="SelectedValue" Type="String" />
        <asp:ControlParameter ControlID="tbSearch" Name="search" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
    /*
	(@mode = 1)
 or (@mode = 2)
 or (@mode = 3)
    */
    /*
	(@mode = 1 and                                      (P.Nazwisko like @search + '%' or P.KadryId like @search + '%' or ISNULL(convert(varchar, PK.RcpId),'') like @search + '%'))
 or (@mode = 2 and PK.Status = 1 and (@search is null or P.Nazwisko like @search + '%' or P.KadryId like @search + '%' or ISNULL(convert(varchar, PK.RcpId), '') like @search + '%'))
 or (@mode = 3 and PK.Status = 2 and (@search is null or P.Nazwisko like @search + '%' or P.KadryId like @search + '%' or convert(varchar, PK.RcpId) like @search + '%'))
    */
--%>

<asp:SqlDataSource ID="dsKartyTmpList" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @data datetime 
set @data = dbo.getdate(GETDATE())

declare @lok nvarchar(200)
select @lok = NULLIF(Opis, '') from Pracownicy where Id = @userId

select null Value, 'wybierz...' Text, 1 Sort
union all
select 
  convert(varchar, T.Id) + '|' + ISNULL(convert(varchar, T.RcpId), '') + '|' + ISNULL(T.NrKarty, '')
, T2.NrKarty + case when PK.Id is null then '' else ' - wydano: ' + convert(varchar(10), PK.Od, 20) + ISNULL(', ' +  P.Nazwisko + ' ' + P.Imie, '') end Text
, 2 Sort
from IdentyfikatoryTymczasowe T
left join PracownicyKarty PK on @data between PK.Od and ISNULL(PK.Do, '20990909') and (PK.RcpId = T.RcpId or PK.NrKarty = T.NrKarty)
outer apply (select convert(varchar, T.RcpId) RcpId) T1
outer apply (select T.Numer + ISNULL(' ' + T.Opis, '') 
	+ ' (' + ISNULL(T.NrKarty + ', ' + T1.RcpId, T1.RcpId) + ')'	
	NrKarty) T2
left join Pracownicy P on P.Id = PK.IdPracownika
where @lok is null or T.Lokalizacja = @lok or T.Lokalizacja is null 
and PK.Id is null -- bez wydanych
order by Sort, Text
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="userId" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsKartyTmp" runat="server" 
    SelectCommand="
select 
  P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + ')' [Pracownik]
, convert(varchar(10), PK.Od, 20) [Data]
--, PK.RcpId [Karta]
, T1.NrKarty [Karta]
, PK.*
from PracownicyKarty PK 
left join IdentyfikatoryTymczasowe T on T.RcpId = PK.RcpId
outer apply (select convert(varchar, PK.RcpId) RcpId) PK1
outer apply (select T.Numer + ISNULL(' ' + T.Opis, '') 
	+ ' (' + ISNULL(T.NrKarty + ', ' + PK1.RcpId, PK1.RcpId) + ')'	
	NrKarty) T1
left join Pracownicy P on P.Id = PK.IdPracownika
where PK.RcpId = @p0 and dbo.getdate(GETDATE()) between PK.Od and ISNULL(PK.Do,'20990909')
    "
    DeleteCommand="
select 
  P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + ')' [Pracownik]
, convert(varchar(10), PK.Od, 20) [Data]
, T1.NrKarty [Karta]
, PK.*
from PracownicyKarty PK 
left join IdentyfikatoryTymczasowe T on T.RcpId = PK.RcpId
outer apply (select convert(varchar, PK.RcpId) RcpId) PK1
outer apply (select T.Numer + ISNULL(' ' + T.Opis, '') 
	+ ' (' + ISNULL(T.NrKarty + ', ' + PK1.RcpId, PK1.RcpId) + ')'	
	NrKarty) T1
left join Pracownicy P on P.Id = PK.IdPracownika
where PK.Id = @p0
    "
    InsertCommand="
;DISABLE TRIGGER PracownicyKarty_Insert ON PracownicyKarty        
insert into PracownicyKarty (IdPracownika, Od, Do, RcpId, NrKarty, Status, WydalId) values (@p0, dbo.getdate(GETDATE()), null, @p1, NULLIF(@p2,''), 1, @p3)
;ENABLE TRIGGER PracownicyKarty_Insert ON PracownicyKarty
    "
    UpdateCommand="update PracownicyKarty set Do = @p2, Status = 2 where Id = @p1"
    />
