<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOgloszenia.ascx.cs" Inherits="Portal.Controls.cntOgloszenia" %>

<div id="paOgloszenia" runat="server" class="cntOgloszenia">
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidMode" runat="server" Visible="false" />
    <asp:HiddenField ID="hidSearch" runat="server" Visible="false"/>
            <div>   <%-- na ewentualną ramkę --%>
                <div id="paFilter" runat="server" class="paFilter form-group">
                    <div class="left">
                        <asp:DropDownList ID="ddlPracownik" runat="server" CssClass="form-control" DataSourceID="dsPracownicy" DataValueField="Value" DataTextField="Text" AutoPostBack="true" Visible="false">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlKategoria" runat="server" CssClass="form-control" DataSourceID="dsKategorie" DataValueField="Id" DataTextField="Kategoria" AutoPostBack="true" >
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" DataSourceID="dsStatusy" DataValueField="Id" DataTextField="Status" AutoPostBack="true" Visible="false" >
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-control" DataSourceID="dsSort" DataValueField="Id" DataTextField="Sort" AutoPostBack="true" >
                        </asp:DropDownList>
                        <asp:CheckBox ID="cbMoje" runat="server" CssClass="check" Text="Tylko moje ogłoszenia" AutoPostBack="true"/>
                        <asp:LinkButton ID="btFilterClear" runat="server" CssClass="btn-delete" OnClick="btFilterClear_Click" Visible="false" ToolTip="Czyść filtry"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>       
                    </div>
                    <asp:Button ID="btDodajOgloszenie" runat="server" OnClick="btDodajOgloszenie_Click" CssClass="btn btn-default" Text="Dodaj ogłoszenie" />
                </div>

                <asp:ListView ID="lvOgloszenia" runat="server" DataSourceID="SqlDataSource1" 
                    DataKeyNames="Id" onitemdatabound="lvOgloszenia_ItemDataBound" 
                    onitemcommand="lvOgloszenia_ItemCommand" 
                    ondatabound="lvOgloszenia_DataBound" >        
                    <ItemTemplate>
                        <div class="ogloszenie <%# Eval("Css") %>">
                            <table class="ogloszenie">

                                <tr id="trInfo" runat="server" class="info info-top" visible="false">
                                    <td class="info" colspan='<%# IsZdjecie(Eval("Zdjecie")) ? 2 : 1 %>'>
                                        <asp:Label ID="lbKategoria" runat="server" CssClass="label1" Text="Kategoria:" />
                                        <asp:Label ID="lbKategoriaV" runat="server" CssClass="value"  Text='<%# Eval("Kategoria") %>' />                                            
                                        <asp:Label ID="lbDataDodania" runat="server" CssClass="label1" Text="Data dodania:" />
                                        <asp:Label ID="lbDataDodaniaV" runat="server" CssClass="value" Text='<%# Eval("DataDodania","{0:d}") %>' ToolTip='<%# Eval("DataDodania","{0:T}") %>' /> 
                                    </td>
                                    <td class="status">
                                        <asp:Label ID="lbStatus" runat="server" CssClass="label1" Text="Status:" />
                                        <asp:Label ID="lbStatusV" runat="server" CssClass="value" Text='<%# Eval("StatusNazwa") %>' /> 
                                    </td>
                                </tr>

                                <tr>
                                    <td rowspan="3" class="zdjecie" id="tdZdjecie" runat="server" visible='<%# IsZdjecie(Eval("Zdjecie")) %>' >
                                        <div>
                                            <img class="group grid-group-image" alt="" data-full="<%# GetImageUrlNoCache(Eval("Zdjecie")) %>" src="<%# GetThumbUrlNoCache(Eval("Zdjecie")) %>" onclick="showOgloszenieBig(this);"/>
                                            <%--<asp:Image ID="imgZdjecie" runat="server" ImageUrl />--%>
                                        </div>
                                    </td>
                                    <td colspan="2" class="ogloszenie">
                                        <table class="tresc">
                                            <%--                                            
                                            <tr id="trInfo" runat="server" class="info" visible="false">
                                                <td class="info">
                                                    <asp:Label ID="lbKategoria" runat="server" CssClass="label1" Text="Kategoria:" />
                                                    <asp:Label ID="lbKategoriaV" runat="server" CssClass="value"  Text='<%# Eval("Kategoria") %>' />                                            
                                                    <asp:Label ID="lbDataDodania" runat="server" CssClass="label1" Text="Data dodania:" />
                                                    <asp:Label ID="lbDataDodaniaV" runat="server" CssClass="value" Text='<%# Eval("DataDodania","{0:d}") %>' ToolTip='<%# Eval("DataDodania","{0:T}") %>' /> 
                                                </td>
                                                <td class="status">
                                                    <asp:Label ID="lbStatus" runat="server" CssClass="label1" Text="Status:" />
                                                    <asp:Label ID="lbStatusV" runat="server" CssClass="value" Text='<%# Eval("StatusNazwa") %>' /> 
                                                </td>
                                            </tr>
                                            --%>
                                            <tr class="tresc">
                                                <td class="tresc" colspan="2">
                                                    <asp:Label ID="lbTresc" runat="server" Text='<%# Eval("Tresc") %>' ToolTip='<%# "Kategoria: " + Eval("Kategoria") %>' />                            
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="wystawil">
                                    <td class="wystawil">
                                        <asp:Label ID="lbwystawil" runat="server" CssClass="label1" Text="Kontakt:" />
                                        <asp:Label ID="lbPrac" runat="server" CssClass="value" Text='<%# Eval("Pracownik") %>' />
                                        <i class="glyphicon glyphicon-envelope small"></i>
                                        <a href='mailto:<%# Eval("Email") %>' ><%# Eval("Email") %></a>
                                    </td>
                                    <td class="termin">                                        
                                        <asp:Label ID="Label2" runat="server" CssClass="label1" Text="Aktualne do:" />
                                        <asp:Label ID="lbDo" runat="server" CssClass="value" Text='<%# Eval("DataZakonczeniaOpis") %>' /> 
                                    </td>
                                </tr>
                                <tr class="control">
                                    <td colspan="2" class="control">
                                        <div id="paControl" runat="server" class="buttons" visible="false" >                                       
                                            <asp:Button runat="server" ID="btAccept" CssClass="btn btn-success left" Text="Zaakceptuj" CommandName="accept" />
                                            <asp:Button runat="server" ID="btReject" CssClass="btn btn-danger left"  Text="Odrzuć" CommandName="reject" />
                                            <asp:Button runat="server" ID="btDelete" CssClass="btn btn-danger"  Text="Usuń" CommandName="del" CommandArgument='<%# Eval("Status") %>' />
                                            <asp:Button runat="server" ID="btFinish" CssClass="btn btn-default" Text="Zakończ" CommandName="finish" />
                                            <asp:Button runat="server" ID="btResend" CssClass="btn btn-default" Text="Wystaw ponownie" CommandName="resend" />
                                            <asp:Button runat="server" ID="btEdit"   CssClass="btn btn-default" Text="Edytuj" CommandName="edit" />
                                        </div>
                                    </td>
                                </tr>
                            </table>    
                        </div>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div class="lvOgloszenia">                
                            <asp:Label ID="lbNoData" runat="server" CssClass="edt" Text="Brak ogłoszeń"></asp:Label>
                        </div>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div id="itemPlaceholderContainer" runat="server" class="lvOgloszenia">
                            <div runat="server" id="itemPlaceholder" />
                            <div class="bs-pager">
                                <asp:DataPager ID="DataPager1" runat="server" PageSize="5">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="«" PreviousPageText="‹" />
                                        <asp:NumericPagerField ButtonType="Link" />
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="›" LastPageText="»" />
                                    </Fields>
                                </asp:DataPager>
                                <div class="count">
                                    <span class="count">Ilość:</span>
                                    <asp:Label ID="lbCount" CssClass="value" runat="server" ></asp:Label>
                                    <span class="show">Pokaż na stronie:</span>
                                    <asp:DropDownList ID="ddlLines" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%-- ogłoszenia --%>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
--declare @userId int
--declare @mode int
--set @userId = 1
--set @mode = 0

declare @data datetime, @now datetime
declare @s nvarchar(max)
set @now = GETDATE()
set @data = dbo.getdate(@now)
set @s = '%' + @search + '%'

select O.*
, convert(varchar(10), O.DataZakonczenia, 20) DataZakonczeniaStr
, convert(varchar(10), O.DataZakonczenia, 20) + ' (' + 
    case dbo.dow(O.DataZakonczenia)
    when 0 then 'Poniedziałek'
    when 1 then 'Wtorek'
    when 2 then 'Środa'
    when 3 then 'Czwartek'
    when 4 then 'Piątek'
    when 5 then 'Sobota'
    when 6 then 'Niedziela'
    end + ')' DataZakonczeniaOpis1
, case dbo.dow(O.DataZakonczenia)
    when 0 then 'Poniedziałku'
    when 1 then 'Wtorku'
    when 2 then 'Środy'
    when 3 then 'Czwartku'
    when 4 then 'Piątku'
    when 5 then 'Soboty'
    when 6 then 'Niedzieli'
    end + ', ' +
    convert(varchar(10), O.DataZakonczenia, 20) DataZakonczeniaOpis
, P1.IsArch
, P2.IsArchZwol
, P1.IsZwolniony
, 'status' + convert(varchar, case when O.Status &gt;= 10 then O.Status - 10 else O.Status end) + case when P1.IsArch = 1 then ' arch' else '' end Css
, case when @kat is null then 1 else 0 end KategoriaVisible
, case when @mode in (1,2,3,4,13) 
  then 1 else 0 end StatusVisible
, P1.Moje
, ISNULL(K.Kategoria, 'brak') Kategoria
, K.Kolejnosc
, P1.Pracownik
, P.Email 
, S.Status StatusNazwa
, case when @sort = 0 then O.DataDodania else null end s1
, case when @sort = 1 then O.DataDodania else null end s2
from poOgloszenia O 
left join poOgloszeniaKategorie K on K.Id = O.IdKategoria
left join poOgloszeniaStatusy S on S.Id = O.Status
left join Pracownicy P on P.Id = O.IdPracownika
outer apply (select 
      P.Imie + ' ' + P.Nazwisko Pracownik
    , case when @data &gt; ISNULL(P.DataZwol, '20990909') then 1 else 0 end IsZwolniony
    , case when O.IdPracownika = @userId then 1 else 0 end Moje
    , case when O.DataZakonczenia &lt; @data then 1 else 0 end IsArch  
    ) P1
outer apply (select 
      case when P1.IsZwolniony = 1 or O.DataZakonczenia &lt; @data then 1 else 0 end IsArchZwol   -- widać tylko zatrudnionych 
    ) P2
where
    (
	(@mode = 0 and O.Status = 2 and P2.IsArchZwol = 0)  -- ogloszenia                   --(@moje = 1 or O.Status = 2) and @data &lt;= ISNULL(P.DataZwol, '20990909') and O.DataDodania &lt;= @now and @data &lt;= ISNULL(O.DataZakonczenia, '20990909')
 or (@mode = 1 and O.Status = 1)                        -- do akceptacji
 or (@mode = 2 and O.Status in (1,2,3,4) and P1.Moje = 1) -- moje
 or (@mode = 3 and (O.Status not in (1,2) or P1.IsArch = 1))    -- archiwum 
    )
-- filtry
and (@kat is null or @kat = O.IdKategoria)
and (@status is null 
    or (@status in (1,2,3) and @status = O.Status)
    or (@status in (4) and (O.DataZakonczenia &lt; @data or O.Status = 4))
    )
and (@moje = 0 or P1.Moje = 1)
and (@prac is null or O.IdPracownika = @prac)
and (@search is null or O.Tresc like @s or P1.Pracownik like @s or P.Email like @s)   
order by s1, s2 desc, O.DataAkceptacji desc
    "
    UpdateCommand="update poOgloszenia set Status={0} where Id={1}"    
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32" />
        <asp:ControlParameter ControlID="hidMode" Name="mode" Type="Int32" />
        <asp:ControlParameter ControlID="ddlKategoria" Name="kat" Type="Int32" />
        <asp:ControlParameter ControlID="ddlStatus" Name="status" Type="Int32" />
        <asp:ControlParameter ControlID="ddlPracownik" Name="prac" Type="Int32" />
        <asp:ControlParameter ControlID="ddlSort" Name="sort" Type="Int32" />
        <asp:ControlParameter ControlID="cbMoje" Name="moje" Type="Boolean" />
        <asp:ControlParameter ControlID="hidSearch" Name="search" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>


<%--
0   Ogłoszenia
1   Do akceptacji
2   Moje 
3   Archiwum

1	Oczekujące na akceptację
2	Wystawione (zaakceptowane)
3	Odrzucone
4	Zakończone - jeżeli zostaje w bazie
13  Odrzucone (usunięte)
14  Zakończone (usunięte)

--%>

<asp:SqlDataSource ID="dsKategorie" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare @data datetime, @now datetime
declare @s nvarchar(max)
set @now = GETDATE()
set @data = dbo.getdate(@now)
;
with KK as 
(
select O.IdKategoria, count(*) Ilosc 
from poOgloszenia O
left join Pracownicy P on P.Id = O.IdPracownika
outer apply (select 
      case when @data &gt; ISNULL(P.DataZwol, '20990909') then 1 else 0 end IsZwolniony
    , case when O.IdPracownika = @userId then 1 else 0 end Moje
    , case when O.DataZakonczenia &lt; @data then 1 else 0 end IsArch  
    ) P1
outer apply (select 
      case when P1.IsZwolniony = 1 or O.DataZakonczenia &lt; @data then 1 else 0 end IsArchZwol   -- widać tylko zatrudnionych 
    ) P2
where 
    (
    /*
	(@mode = 0 and O.Status = 2 and O.DataDodania &lt;= @now and @data &lt;= ISNULL(O.DataZakonczenia, '20990909'))
 or (@mode = 1 and O.Status = 1)            -- do akceptacji
 or (@mode = 2 and O.Status in (1,2,3) and IdPracownika = @userId)  -- moje
 or (@mode = 3 and O.Status = 2 and O.DataZakonczenia &lt; @data)   -- archiwum 
    */
	(@mode = 0 and O.Status = 2 and P2.IsArchZwol = 0)  -- ogloszenia                   --(@moje = 1 or O.Status = 2) and @data &lt;= ISNULL(P.DataZwol, '20990909') and O.DataDodania &lt;= @now and @data &lt;= ISNULL(O.DataZakonczenia, '20990909')
 or (@mode = 1 and O.Status = 1)                        -- do akceptacji
 or (@mode = 2 and O.Status in (1,2,3,4) and P1.Moje = 1) -- moje
 or (@mode = 3 and (O.Status not in (1,2) or P1.IsArch = 1))    -- archiwum 
    )
group by IdKategoria
)
--select null Id, 'Wszystkie kategorie (' + (select convert(varchar,sum(Ilosc)) from KK) + ')' Kategoria, null Kolejnosc, 1 Sort --aktywne!
select null Id, 'Wszystkie kategorie' Kategoria, null Kolejnosc, 1 Sort
union all
select K.Id, K.Kategoria + ' (' + convert(varchar, ISNULL(KK.Ilosc, 0)) + ')', K.Kolejnosc, 2 Sort
from poOgloszeniaKategorie K
left join KK on KK.IdKategoria = K.Id 
where Aktywna = 1
order by Sort, Kolejnosc, Kategoria
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32" />
        <asp:ControlParameter ControlID="hidMode" Name="mode" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<%-- jest też na cntOgloszenieEdit --%>
<asp:SqlDataSource ID="dsStatusy" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"  
    SelectCommand="
select null Id, 'Wszystkie statusy' Status, 1 Sort
union all
select Id, Status, 2 Sort
from poOgloszeniaStatusy
where Id in (1,2,3,4)  -- bez odrzucone usuniete
order by Sort, Id
    ">
</asp:SqlDataSource>
    
<asp:SqlDataSource ID="dsSort" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select 1 Id, 'Sortuj od najnowszych' Sort
union all select 2, 'Sortuj od najstarszych'
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsPracownicy" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare @data datetime
set @data = dbo.getdate(GETDATE())

select null Value, 'Wszyscy pracownicy' Text, 0 Zwolniony, 1 Sort
union all
select distinct P.Id, case when P1.Zwolniony = 1 then '*' else '' end + P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + ')', P1.Zwolniony, 2
from poOgloszenia O
left join Pracownicy P on P.Id = O.IdPracownika 
outer apply (select case when @data &gt; ISNULL(P.DataZwol, '20990909') then 1 else 0 end Zwolniony) P1
order by Sort, Zwolniony, Text
    ">
</asp:SqlDataSource>





<%--
                    <ItemTemplate>
                        <div class="ogloszenie <%# Eval("Css") %>">
                            <table class="ogloszenie">
                                <tr id="trInfo" runat="server" class="info" visible="false">
                                    <td colspan="2">
                                        <asp:Label ID="Label5" runat="server" CssClass="label1" Text="Kategoria:" />
                                        <asp:Label ID="Label1" runat="server" CssClass="value"  Text='<%# Eval("Kategoria") %>' />                                            
                                        <asp:Label ID="Label3" runat="server" CssClass="label1" Text="Data dodania:" />
                                        <asp:Label ID="Label4" runat="server" CssClass="value"  Text='<%# Eval("DataDodania","{0:d}") %>' ToolTip='<%# Eval("DataDodania","{0:T}") %>' /> 
                                        <div class="status">    
                                            <asp:Label ID="Label6" runat="server" CssClass="label1" Text="Status:" />
                                            <asp:Label ID="Label7" runat="server" CssClass="value"  Text='<%# Eval("StatusNazwa") %>' /> 
                                        </div>
                                    </td>
                                </tr>
                                <tr class="tresc">
                                    <td class="zdjecie" rowspan="3" id="tdZdjecie" runat="server" visible='<%# IsZdjecie(Eval("Zdjecie")) %>' >
                                        <div>
                                            <img class="group grid-group-image" alt="" data-full="<%# GetImageUrl(Eval("Zdjecie")) %>" src="<%# GetThumbUrlNoCache(Eval("Zdjecie")) %>" />
                                            <%--<asp:Image ID="imgZdjecie" runat="server" ImageUrl />-- % >
                                        </div>
                                    </td>
                                    <td class="tresc" colspan="2">
                                        <asp:Label ID="lbTresc" runat="server" Text='<%# Eval("Tresc") %>' ToolTip='<%# "Kategoria: " + Eval("Kategoria") %>' />                            
                                    </td>
                                </tr>
                                <tr class="wystawil">
                                    <td class="wystawil">
                                        <asp:Label ID="lbwystawil" runat="server" CssClass="label1" Text="Kontakt:" />
                                        <asp:Label ID="lbPrac" runat="server" CssClass="value" Text='<%# Eval("Pracownik") %>' />
                                        <i class="glyphicon glyphicon-envelope small"></i>
                                        <a href='mailto:<%# Eval("Email") %>' ><%# Eval("Email") %></a>
                                    </td>
                                    <td class="termin">                                        
                                        <asp:Label ID="Label2" runat="server" CssClass="label1" Text="Aktualne do:" />
                                        <asp:Label ID="lbDo" runat="server" CssClass="value" Text='<%# Eval("DataZakonczeniaOpis") %>' /> 
                                    </td>
                                </tr>
                                <tr class="control">
                                    <td class="control" colspan="2">
                                        <div id="paControl" runat="server" class="buttons" visible="false" >                                       
                                            <asp:Button runat="server" ID="btAccept" CssClass="btn btn-success left" Text="Zaakceptuj" CommandName="accept" />
                                            <asp:Button runat="server" ID="btReject" CssClass="btn btn-danger left"  Text="Odrzuć" CommandName="reject" />
                                            <asp:Button runat="server" ID="btDelete" CssClass="btn btn-danger"  Text="Usuń" CommandName="del" />
                                            <asp:Button runat="server" ID="btResend" CssClass="btn btn-default" Text="Wystaw ponownie" CommandName="resend" />
                                            <asp:Button runat="server" ID="btEdit"   CssClass="btn btn-default" Text="Edytuj" CommandName="edit" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
    
    
    
    --%>