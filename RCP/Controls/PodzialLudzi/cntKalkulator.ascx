<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKalkulator.ascx.cs" Inherits="HRRcp.Controls.PodzialLudzi.cntKalkulator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

<div id="paKalkulator" runat="server" class="cntKalkulator">
    <div class="container">
        <div class="row">
            <%-- pracownicy --%>
            <div class="col-md-4 col1">
                <div id="paFilter" runat="server" class="filter" visible="true">
                    <div class="filterabs input-group">
                        <span class="input-group-addon addon-default">Wyszukaj:</span>
                        <asp:TextBox ID="tbSearch" CssClass="textbox form-control" runat="server" ></asp:TextBox>
                        <div id="paRecord" runat="server" class="input-group-btn">
                            <asp:LinkButton ID="lbtRecord" runat="server" CssClass="btn btn-default btn-search-record" ToolTip="Kliknij i zacznij mówić..." >
                                <i class="fa fa-microphone"></i>
                            </asp:LinkButton>
                        </div>
                        <div class="input-group-btn">
                            <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-default btn-search-clear" ToolTip="Czyść..." >
                                <i class="glyphicon glyphicon-erase"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                    <ContentTemplate>
                        <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />            
                        <asp:CheckBox ID="cbHideSelected" runat="server" CssClass="form-control" Text="Ukryj wybranych pracowników" Checked="true" AutoPostBack="true"/>
                        <asp:CheckBox ID="cbAutoAdd" runat="server" CssClass="form-control" Text="Dodaj automatycznie po wyszukaniu" Checked="true" />
                        <asp:GridView ID="gvPracownicy" runat="server" DataSourceID="dsPracownicy" PagerSettings-Mode="NumericFirstLast" PageSize="15" AllowPaging="true" AllowSorting="true" CssClass="GridView1"
                            OnRowDataBound="gvPracownicy_RowDataBound"
                            EmptyDataText="Brak danych">
                        </asp:GridView>
                        <div class="pager">
                            <div class="pagerabs">
                                <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <span class="count">Pokaż na stronie:</span>
                                <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" CssClass="form-control"
                                    OnChange="showAjaxProgress();"
                                    OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                                    <asp:ListItem Text="15" Value="15" Selected="True"></asp:ListItem>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%-- selected --%>
            <div class="col-md-4 col2">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                    <ContentTemplate>
                        <div class="filter input-group">
                            <span class="input-group-addon addon-default">Miesiąc:</span>
                            <asp:DropDownList ID="ddlMiesiac" runat="server" CssClass="form-control" DataSourceID="dsMiesiace" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnDataBound="ddlMiesiac_DataBound"></asp:DropDownList>
                        </div>
                        <asp:GridView ID="gvSelected" runat="server" DataSourceID="dsSelected" AllowSorting="true" CssClass="GridView1"
                            EmptyDataText="Wybierz pracowników...">
                        </asp:GridView>
                        <asp:Button ID="gvSelectedCmd" runat="server" CssClass="button_postback" Text="Button" onclick="gvSelectedCmd_Click" />
                        <asp:HiddenField ID="gvSelectedCmdPar" runat="server" />
                        <asp:HiddenField ID="gvSelectedSelected" runat="server" />
                        <asp:Button ID="btClearSel" runat="server" CssClass="btn btn-default" Text="Usuń wszystkich" onclick="btClearSel_Click" Enabled="false"/>            
                        <asp:Button ID="btPasteShow" runat="server" CssClass="btn btn-default" Text="Wklej" onclick="btPasteShow_Click" />            
                        <asp:Button ID="btExcelSel" runat="server" CssClass="btn btn-default" Text="Excel" onclick="btExcelSel_Click" Visible="false" />            
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btExcelSel"/>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <%-- podział/pracownicy --%>
            <div class="col-md-4 col3">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" >
                    <ContentTemplate>
                        <%--                    
                        <asp:GridView ID="gvResultHeader" runat="server" DataSourceID="dsResultHeader" AllowSorting="true" CssClass="GridView1">
                        </asp:GridView>
                        --%>
                        <div class="filter filter1 input-group">
                            <span class="input-group-addon addon-default">Nazwa:</span>
                            <asp:TextBox ID="tbNazwa" CssClass="textbox form-control" runat="server" AutoPostBack="true" MaxLength="100" Placeholder="Podział..."></asp:TextBox>
                        </div>
                        <div class="filter filter2 input-group">
                            <span class="input-group-addon addon-default">Kwota łączna:</span>
                            <asp:TextBox ID="tbKwota" CssClass="textbox form-control" runat="server" AutoPostBack="true" MaxLength="10" OnTextChanged="tbKwota_TextChanged"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                TargetControlID="tbKwota" 
                                FilterType="Custom" 
                                ValidChars="0123456789.," />
                        </div>
                        <div class="row">
                            <div class="col-md-6"> 
                                <div class="filter filter3 input-group">
                                  <span class="input-group-addon addon-default">Ilość osób:</span>
                                  <span class="input-group-addon addon-default value"><asp:Label ID="lbCountSel" runat="server" ></asp:Label></span>
                                </div>                           
                            </div> 
                            <div class="col-md-6"> 
                                <div class="filter filter3 input-group">
                                    <span class="input-group-addon addon-default ">Kwota na osobę:</span>
                                    <span class="input-group-addon addon-default value"><asp:Label ID="lbKwotaOs" runat="server" ></asp:Label></span>                                 
                                </div>                           
                            </div>
                        </div>

                        
                        
                        
<%--                        <div class="filter filter3 input-group">
                            <span class="input-group-addon addon-default">Ilość osób:</span>
                            <asp:Label ID="lbCountSel" runat="server" ></asp:Label>
                        </div>
                        <div class="filter filter4 input-group">
                            <span class="input-group-addon addon-default">Kwota na osobę:</span>
                            <asp:Label ID="lbKwotaOs" runat="server" ></asp:Label>
                        </div>--%>


                        <asp:Menu ID="tabResult" runat="server" Orientation="Horizontal" 
                            onmenuitemclick="tabResult_MenuItemClick" >
                            <StaticMenuStyle CssClass="tabsStrip" />
                            <StaticMenuItemStyle CssClass="tabItem" />
                            <StaticSelectedStyle CssClass="tabSelected" />
                            <StaticHoverStyle CssClass="tabHover" />
                            <Items>
                                <asp:MenuItem Text="Podział" Value="vPodzial" Selected="true"></asp:MenuItem>
                                <asp:MenuItem Text="Pracownicy" Value="vPracownicy" ></asp:MenuItem>
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
                        <asp:MultiView ID="mvResult" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vPodzial" runat="server" OnActivate="vPodzial_Activate">
                                <asp:GridView ID="gvResultPodzial" runat="server" DataSourceID="dsResultPodzial" AllowSorting="true" CssClass="GridView1"
                                    EmptyDataText="Brak danych">
                                </asp:GridView>
                                <asp:Button ID="btExcelPodz" runat="server" CssClass="btn btn-default" Text="Excel" onclick="btExcelPodz_Click" />            
                            </asp:View>
                            <asp:View ID="vPracownicy" runat="server" OnActivate="vPracownicy_Activate">
                                <asp:GridView ID="gvResultPracownicy" runat="server" DataSourceID="dsResultPracownicy" AllowSorting="true" CssClass="GridView1"
                                    EmptyDataText="Brak danych">
                                </asp:GridView>
                                <asp:Button ID="btExcelPrac" runat="server" CssClass="btn btn-default" Text="Excel" onclick="btExcelPrac_Click" />            
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btExcelPodz"/>
                        <asp:PostBackTrigger ControlID="btExcelPrac"/>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <%-- wklej --%>
    <uc1:cntModal runat="server" ID="cntModalPaste" Backdrop="false" Keyboard="false" ShowFooter="true" CloseButtonText="Anuluj" >
        <HeaderTemplate>
            <h4><asp:Literal ID="ltTitle" runat="server" Text="Wklej numery ewidencyjne pracowników"></asp:Literal></h4>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <asp:TextBox ID="tbPaste" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="20"></asp:TextBox>
                    <asp:CheckBox ID="cbPasteClear" runat="server" CssClass="form-control" Text="Wyczyść listę przed wklejeniem"/>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btPaste" runat="server" Text="Wklej" CssClass="btn btn-success" OnClick="btPaste_Click"/>
        </FooterTemplate>
    </uc1:cntModal>  
    <%-- potwierdź --%>
    <uc1:cntModal runat="server" ID="modalConfirm" Title="Potwierdź wykonanie operacji" CssClass="modalConfirm" Backdrop="false" Keyboard="false" CloseButtonText="Anuluj" >
        <ContentTemplate>
            <asp:Label ID="lbConfirm" runat="server" Text="Label"></asp:Label>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btConfirmOk" runat="server" Text="Ok" CssClass="btn btn-success" OnClick="btConfirmOk_Click"/>
        </FooterTemplate>
    </uc1:cntModal>
</div>

<asp:SqlDataSource ID="dsMiesiace" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @data datetime
declare @bom datetime
set @data = dbo.getdate(GETDATE())
set @bom = dbo.bom(@data)

select 
  LEFT(D2.Data, 7) Text
--, REPLACE(D2.Data, '-', '') + S.Selected Value
, D2.Data + S.Selected Value
from dbo.GetNum(0, 12) D
outer apply (select DATEADD(MONTH, -D.num, @bom) Data) D1 
outer apply (select convert(varchar(10), D1.Data, 20) Data) D2
outer apply (select case when D1.Data = dbo.bom(DATEADD(D, -10, @data)) then '|x' else '' end Selected) S
order by D.num
    "/>

<%-- ► --%>

<asp:SqlDataSource ID="dsPracownicy" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    OnSelected="dsPracownicy_Selected"
    SelectCommand="
    /*
declare @miesiac datetime
declare @selected varchar(max)
declare @search nvarchar(250)
declare @hidesel int
set @miesiac = '20170301'
set @search = 'a'
set @hidesel = 0
    */

declare @data datetime
set @data = dbo.getdate(GETDATE())

declare @od datetime
declare @do datetime
set @od = DATEADD(M, -2, dbo.bom(@miesiac))   -- zatrudniony w okresie ostatnich 3 miesięcy
set @do = dbo.eom(@miesiac)

declare @s1 nvarchar(250)
declare @s2 nvarchar(250)
select @s1 = items from dbo.SplitStr(RTRIM(@search), ' ') where idx = 0
select @s2 = items from dbo.SplitStr(RTRIM(@search), ' ') where idx = 1

select distinct
  P.Id [pracId:-]  --index 0 musi być
, P.Nazwisko [s1:-]
, P.Imie [s2:-]
, P.KadryId [s3:-]
--, @selected
--, SEL.items [Zaznacz:CB;check|@pracId|Zaznacz osobę]

, @od, @do, @miesiac
, P.KadryId [Nr ew.]
, P.Nazwisko + ' ' + P.Imie + case when P.DataZwol &lt;= @data then case when RIGHT(Imie,1) = 'a' then ' (zwolniona)' else ' (zwolniony)' end else '' end [Pracownik]
, '+' [:;controlx|cmd:add @pracId|Dodaj]
, null [:click|cmd:add @pracId|Dodaj]
, S1.rclass [:class]
from Pracownicy P
outer apply (select items from dbo.SplitInt(@selected,',') where items = P.Id) SEL
outer apply (select case when SEL.items is not null then 'selected' else '' end rclass) S1
where P.Id in (select IdPracownika from Przypisania where Status = 1 and Od &lt;= @od and @do &lt;= ISNULL(Do, '20990909'))
and P.Status in (-1,0,1)
    and P.KadryId between 0 and 80000-1
and (@search is null
  or @s2 is null and (P.Nazwisko like @s1 + '%' or P.Imie like @s1 + '%' or P.KadryId like @s1 + '%')
  or @s2 is not null and (P.Nazwisko like @s1 + '%' and P.Imie like @s2 + '%' 
                       or P.Nazwisko like @s2 + '%' and P.Imie like @s1 + '%' 
					   or P.KadryId like @s1 + '%'
					   or P.KadryId like @s2 + '%'					   
					   )
	)
and (@hidesel = 0 or SEL.items is null)
order by 2,3,4
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlMiesiac" Name="miesiac" PropertyName="SelectedValue" Type="DateTime" />
        <asp:ControlParameter ControlID="tbSearch" Name="search" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="cbHideSelected" Name="hidesel" PropertyName="Checked" Type="Boolean" />
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsSelected" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    OnSelected="dsSelected_Selected"
    SelectCommand="
declare @data datetime
set @data = dbo.getdate(GETDATE())

select 
--:CUT
  P.Id [pracId:-]
--, SEL.items [Zaznacz:CB;check|@pracId|Zaznacz osobę]
, P.KadryId [Nr ew.:C]
, P.Nazwisko + ' ' + P.Imie [Pracownik]
, 'x' [:;controlx|cmd:rem @pracId|Usuń]
, null [:click|cmd:rem @pracId|Usuń]
/*:CSV
  P.KadryId [Nr ew.]
, P.Nazwisko + ' ' + P.Imie [Pracownik]
*/
from Pracownicy P
outer apply (select items from dbo.SplitInt(@selected,',') where items = P.Id) SEL
where SEL.items is not null
order by P.Nazwisko, P.Imie, P.KadryId
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsResultHeader" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @kwota_os float 
declare @ilosc int
select @ilosc = NULLIF(count(*), 0.0) from dbo.SplitInt(@selected,',')
set @kwota_os = @kwota / @ilosc

select 
  ISNULL(@nazwa, 'Podział') + ' - ' + convert(varchar(10), @miesiac, 20) [Nazwa]
, ISNULL(@kwota, 0) [Kwota:N0.00]
, ISNULL(@ilosc, 0) [Ilość osób:N]
, ISNULL(@kwota_os, 0) [Kwota na osobę:N0.0000]
--from (select 1 x) x
--where @ilosc &gt; 0
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="ddlMiesiac" Name="miesiac" PropertyName="SelectedValue" Type="DateTime" />
        <asp:ControlParameter ControlID="tbKwota" Name="kwota" PropertyName="Text" Type="Double" />
        <asp:ControlParameter ControlID="tbNazwa" Name="nazwa" PropertyName="Text" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsResultPodzial" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    OnSelected="dsResultPodzial_Selected"
    SelectCommand="
declare @data datetime
declare @kwota_os float 
declare @ilosc int
set @kwota = NULLIF(@kwota, 0.0)
set @data  = dbo.eom(@miesiac)  
set @ilosc = (select NULLIF(count(*), 0) from dbo.SplitInt(@selected,','))
set @kwota_os = @kwota / @ilosc

select 
  D.Dzial [Dział:C] 
, D.cc [cc] 
, ROUND(sum(@kwota_os * D.Wsp) * 100 / @kwota, 4) [Udział %:N0.00S]
, ROUND(sum(@kwota_os * D.Wsp), 4) [Kwota:N0.00S]
from dbo.SplitInt(@selected,',') a
left join Pracownicy P on P.Id = a.items

left outer join Splity S on S.GrSplitu = P.GrSplitu and @data between S.DataOd and ISNULL(S.DataDo, @data)
left outer join SplityWsp W on W.IdSplitu = S.Id 
left outer join CC on CC.Id = W.IdCC

left outer join Splity S1 on S1.GrSplitu = CC.GrSplitu and @data between S1.DataOd and ISNULL(S1.DataDo, @data)
left outer join SplityWsp W1 on W1.IdSplitu = S1.Id
left outer join CC C1 on C1.Id = W1.IdCC

outer apply (select ISNULL(ISNULL(C1.Nazwa, CC.Nazwa), '???') as Dzial, ISNULL(ISNULL(C1.cc, CC.cc), '???') as cc, ISNULL(ISNULL(W1.Wsp * W.Wsp, W.wsp), 0) Wsp) D

group by D.Dzial, D.cc
order by D.cc
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="ddlMiesiac" Name="miesiac" PropertyName="SelectedValue" Type="DateTime" />
        <asp:ControlParameter ControlID="tbKwota" Name="kwota" PropertyName="Text" Type="Double" />
        <asp:ControlParameter ControlID="tbNazwa" Name="nazwa" PropertyName="Text" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsResultPracownicy" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    OnSelected="dsResultPracownicy_Selected"
    SelectCommand="
declare @data datetime
declare @kwota_os float 
declare @ilosc int
set @kwota = NULLIF(@kwota, 0.0)
set @data  = dbo.eom(@miesiac)  
set @ilosc = (select NULLIF(count(*), 0) from dbo.SplitInt(@selected,','))
set @kwota_os = @kwota / @ilosc

select 
  P.KadryId as [Nr ew.:C]
, P.Nazwisko + ' ' + P.Imie [Pracownik]
, D.Dzial [Dział] 
, D.cc [cc] 
, ROUND(D.Wsp, 4) as [Udział %:N0.00S]
, ROUND(@kwota_os * D.Wsp, 4) [Kwota:N0.00S]
from dbo.SplitInt(@selected,',') a
left join Pracownicy P on P.Id = a.items

left outer join Splity S on S.GrSplitu = P.GrSplitu and @data between S.DataOd and ISNULL(S.DataDo, @data)
left outer join SplityWsp W on W.IdSplitu = S.Id 
left outer join CC on CC.Id = W.IdCC

left outer join Splity S1 on S1.GrSplitu = CC.GrSplitu and @data between S1.DataOd and ISNULL(S1.DataDo, @data)
left outer join SplityWsp W1 on W1.IdSplitu = S1.Id
left outer join CC C1 on C1.Id = W1.IdCC

outer apply (select ISNULL(ISNULL(C1.Nazwa, CC.Nazwa), '???') as Dzial, ISNULL(ISNULL(C1.cc, CC.cc), '???') as cc, ISNULL(ISNULL(W1.Wsp * W.Wsp, W.wsp), 0) Wsp) D

order by P.Nazwisko, P.Imie, P.KadryId
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="ddlMiesiac" Name="miesiac" PropertyName="SelectedValue" Type="DateTime" />
        <asp:ControlParameter ControlID="tbKwota" Name="kwota" PropertyName="Text" Type="Double" />
        <asp:ControlParameter ControlID="tbNazwa" Name="nazwa" PropertyName="Text" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsPasteCheck" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @num varchar(max)
set @num = '{0}'

select p.Id, a.items KadryId, a1.Error
from dbo.SplitStr(@num, ',') a
left join Pracownicy p on p.KadryId = a.items
outer apply (select case when p.Id is null then a.items else null end Error) a1
order by a.items
    ">
</asp:SqlDataSource>

<script language="C#" type="text/C#" runat="server">
    protected override bool Recognize(string cmd)
    {
        string rest;
        bool iscmd = false;
        if (!String.IsNullOrEmpty(cmd))
        {
            iscmd = true;
            if (startsWith(cmd, out rest, "cofnij"))
            {
                switch (LastCmd)
                {
                    case cmdAdd:
                        PracRem(LastId);
                        break;
                    case cmdRem:
                        PracAdd(LastId);
                        break;
                }
            }
            else if (IsConfirm() && startsWith(cmd, out rest, btConfirmOk.Text.ToLower(), "tak", "dawaj"))
                Confirm(true);
            else if (IsConfirm() && startsWith(cmd, out rest, modalConfirm.CloseButtonText.ToLower(), "nie", "stop", "zamknij"))
                Confirm(false);
            else if (startsWith(cmd, out rest, "nazwa"))
            {
                tbNazwa.Text = rest;
            }
            else if (startsWith(cmd, out rest, "kwota łączna", "kwota"))
            {
                string[] num = GetNumbers(rest);
                string nn = String.Join("", num);
                tbKwota.Text = nn;
                UpdateKwota();
            }
            else if (startsWith(cmd, out rest, "excel"))
            {
                ExportCsv();
            }
            else if (startsWith(cmd, out rest, "czyść", "cześć"))  // tak rozumie ;)
            {
            }
            else if (startsWith(cmd, out rest, "miesiąc"))
            {
                string[] formats = new string[]
                {
                    "yyyy MMMM"
                    ,"MMMM yyyy"
                    ,"yyyy MM"
                    ,"MM yyyy"        
                    ,"yyyy M"
                    ,"M yyyy"        
                };
                try
                {
                    DateTime dt = DateTime.ParseExact(rest, formats, new System.Globalization.CultureInfo("pl-PL"), System.Globalization.DateTimeStyles.None);
                    SelectMonth(dt);
                }
                catch (Exception ex)
                {
                }
            }
            else if (startsWith(cmd, out rest, "poprzedni miesiąc"))
                PrevMonth();
            else if (startsWith(cmd, out rest, "następny miesiąc"))
                NextMonth();
            else if (startsWith(cmd, out rest, "pokaż na stronie"))
                ShowLines(rest);
            else if (startsWith(cmd, out rest, "sortuj"))
                Sortuj(rest);
            else if (startsWith(cmd, out rest, "usuń wszystkich"))
            {
                LastCmd = cmdConfirm;
                ShowConfirm(cmDeleteAll, "Potwierdź usunięcie wszystkich wybranych pracowników.");
                //ClearAll();
            }
            else if (startsWith(cmd, out rest, "wklej"))
                ShowPaste();
            else if (startsWith(cmd, out rest, "pokaż podział", "podział"))
                SelectTab(vPodzial);
            else if (startsWith(cmd, out rest, "pokaż pracowników", "pokaż pracownicy", "pracownicy"))
                SelectTab(vPracownicy);
            else if (startsWith(cmd, out rest, cbHideSelected.Text.ToLower(), "ukryj wybranych"))
                cbHideSelected.Checked = !cbHideSelected.Checked;
            else if (startsWith(cmd, out rest, cbAutoAdd.Text.ToLower(), "dodaj automatycznie"))
                cbAutoAdd.Checked = !cbAutoAdd.Checked;
            else if (startsWith(cmd, out rest, cbPasteClear.Text.ToLower(), "wyczyść listę"))
                cbPasteClear.Checked = !cbPasteClear.Checked;
            else if (startsWith(cmd, out rest, cbPasteClear.Text.ToLower(), "koniec psot"))
            {
                tbSearch.Text = null;
                HttpContext.Current.Response.Redirect("http://www.kdrsolutions.pl");
            }
            else
                iscmd = false;
        }
        return iscmd;
    }
</script>