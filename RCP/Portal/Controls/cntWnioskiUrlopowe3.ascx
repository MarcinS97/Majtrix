<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioskiUrlopowe3.ascx.cs" Inherits="HRRcp.Portal.Controls.cntWnioskiUrlopowe3" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc2" %>

<asp:HiddenField ID="hidKierId" runat="server" />

<asp:HiddenField ID="hidTypy" runat="server" Visible="false" />
<asp:HiddenField ID="hidRodzaj" runat="server" Visible="false" />


<div id="paFilter" runat="server" class="paFilter" visible="true">
    <%-- <span class="label">Wyszukaj pracownika:</span>
    <asp:TextBox ID="tbSearch" CssClass="search textbox form-control" runat="server"></asp:TextBox>
    <asp:Button ID="btClear" runat="server" CssClass="button75 btn btn-default" Text="Czyść" /><br />--%>
    <div class="form-group">
        <div class="input-group">
            <span class="input-group-addon" id="basic-addon1">Wyszukaj pracownika:</span>
            <asp:TextBox ID="tbSearch" CssClass="search textbox form-control" runat="server" aria-describedby="basic-addon1"></asp:TextBox>
            <div class="input-group-btn">
                <asp:Button ID="btClear" runat="server" CssClass="button75 btn btn-default" Text="Czyść" />
            </div>
            <%--<input type="text" class="form-control" placeholder="Username" aria-describedby="basic-addon1">--%>
        </div>
    </div>
    <div class="form-group">
        <div class="input-group">
            <span class="input-group-addon">Pracownik:</span>
            <asp:DropDownList ID="ddlPracownik" runat="server" CssClass="form-control" DataSourceID="SqlDataSourcePrac" DataTextField="Pracownik" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="cnt_ChangeFilter">
            </asp:DropDownList>
        </div>
    </div>

    <div class="form-group">
        <div class="input-group">
            <span class="input-group-addon">Typ:</span>
            <asp:DropDownList ID="ddlTyp" runat="server" CssClass="form-control" DataSourceID="SqlDataSourceTyp" DataTextField="Nazwa" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="cnt_ChangeFilter">
            </asp:DropDownList>
        </div>
    </div>

    <div id="paStatus" runat="server">
        <div class="form-group">
            <div class="input-group">
                <span class="input-group-addon">Status:</span>
                <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="SqlDataSourceStatus" DataTextField="Nazwa" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="cnt_ChangeFilter">
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <div class="form-group">
        <div class="input-group">
            <span class="input-group-addon">Okres od:</span>
            <uc2:DateEdit ID="deOd" runat="server" AutoPostBack="true" OnDateChanged="cnt_ChangeFilter" />
            <span class="input-group-addon">do:</span>
            <uc2:DateEdit ID="deDo" runat="server" AutoPostBack="true" OnDateChanged="cnt_ChangeFilter" />
        </div>
    </div>

</div>

<asp:SqlDataSource ID="SqlDataSourcePrac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null as Id, 'wybierz ...' as Pracownik, 0 as Sort
union    
select Id, 
case when Status = -1 then '* ' else '' end +
Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Pracownik, 
case when Status = -1 then 2 else 1 end as Sort
from Pracownicy 
where Status &gt;= -1
order by Sort, Pracownik
    "></asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null as Id, 'wybierz ...' as Kierownik, 0 as Sort
union    
select Id, Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Kierownik, 1 as Sort
from Pracownicy 
where Kierownik=1
and Status &gt;= -1
order by Sort, Kierownik
    "></asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceTyp" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Typ as Nazwa, 1 as Sort from poWnioskiUrlopoweTypy
order by Sort, Nazwa
    "></asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Status as Nazwa, 1 as Sort from poWnioskiUrlopoweStatusy
order by Sort, Id
    "></asp:SqlDataSource>

<div id="paButtonsTop" runat="server" class="cntWnioskiUrlopowe_buttonstop" visible="false">
    <asp:Button ID="btUpdateEntered" runat="server" CssClass="button250 btn btn-primary" Text="Aktualizuj wnioski z BAAN" OnClick="btUpdateEntered_Click" Visible="false" />
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" OnClick="btSearch_Click" />

        <asp:ListView ID="lvWnioski" runat="server" DataKeyNames="Id"
            DataSourceID="SqlDataSource1"
            OnItemDataBound="lvWnioski_ItemDataBound"
            OnItemCommand="lvWnioski_ItemCommand"
            OnLayoutCreated="lvWnioski_LayoutCreated"
            OnDataBound="lvWnioski_DataBound"
            OnItemCreated="lvWnioski_ItemCreated"
            OnItemDeleted="lvWnioski_ItemDeleted"
            OnItemDeleting="lvWnioski_ItemDeleting"
            OnSelectedIndexChanged="lvWnioski_SelectedIndexChanged"
            OnItemUpdating="lvWnioski_ItemUpdating"
            OnItemInserted="lvWnioski_ItemInserted"
            OnItemUpdated="lvWnioski_ItemUpdated"
            OnDataBinding="lvWnioski_DataBinding">
            <ItemTemplate>
                <asp:HiddenField ID="hidWniosekId" runat="server" Value='<%# Eval("Id") %>' />
                <tr id="trLine" runat="server" class='<%# GetRowClass("it", Eval("Wprowadzony")) %>'>
                    <td id="tdPrac" runat="server" class="nazwisko_nrew" visible="false">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Pracownik") %>' />
                        <div class="line2">
                            <asp:Button ID="btSelect1_" CssClass="control" runat="server" CommandName="Select" Text="Wybierz" Visible="false" />
                            <asp:CheckBox ID="btSelect1" CssClass="control" runat="server" Visible="false" />
                            &nbsp;
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("KadryId") %>' />
                        </div>
                    </td>
                    <td id="tdTyp" runat="server" class="typ">
                        <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                    </td>
                    <td id="tdOd" runat="server" class="data">
                        <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
                    </td>
                    <td id="tdDo" runat="server" class="data">
                        <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
                    </td>

                    <td id="tdDni" runat="server" class="num">
                        <asp:Label ID="DniLabel" runat="server" Text='<%# Eval("DniGodz") %>' />
                    </td>
                    <%--
            <td id="tdDni" runat="server" class="num">
                <asp:Label ID="DniLabel" runat="server" Text='<%# Eval("Dni") %>' />
            </td>
            <td id="tdGodzin" runat="server" class="num">
                <asp:Label ID="Label11" runat="server" Text='<%# Eval("Godzin") %>' />
            </td>
                    --%>
                    <td id="tdTypOkres" runat="server" class="typokres" visible="false">
                        <asp:Label ID="Label9" runat="server" CssClass="Typ" Text='<%# Eval("AssecoTyp") %>' />
                        <asp:Label ID="Label10" runat="server" CssClass="Typ" Text='<%# Eval("AssecoPodTyp") %>' />
                        -
                <asp:Label ID="Label8" runat="server" CssClass="Symbol" Text='<%# Eval("Symbol") %>' />
                        -
                <asp:Label ID="Label4" runat="server" CssClass="Typ" Text='<%# Eval("Typ") %>' /><br />
                        <asp:Label ID="Label5" runat="server" CssClass="Od" Text='<%# Eval("Od", "{0:d}") %>' />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label6" runat="server" CssClass="Do" Text='<%# Eval("Do", "{0:d}") %>' />

                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dni: 
                <asp:Label ID="Label7" runat="server" CssClass="Dni" Text='<%# Eval("DniGodz") %>' />
                        <%--            
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dni: 
                <asp:Label ID="Label7" runat="server" CssClass="Dni" Text='<%# Eval("Dni") %>' />
                        --%>
                        <%--            
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;godzin: 
                <asp:Label ID="Label12" runat="server" CssClass="Dni" Text='<%# Eval("Godzin") %>' />
                        --%>
                    </td>
                    <td id="tdStatus" runat="server" class="status">
                        <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                    </td>
                    <td id="tdKier" runat="server" class="nazwisko_nrew" visible="false">
                        <table class="table-sciezka">
                            <tr>
                                <th>Akceptujący</th>
                                <th>Status</th>
                                <th>Powiadomienie</th>
                                <th>Akceptacja</th>
                            </tr>
                            <asp:Repeater ID="rpSciezkaAkceptacji" runat="server" DataSourceID="dsSciezkaAkceptacji">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <span class="<%# Eval("css") %>"><%# Eval("Akceptujacy") %></span>

                                        </td>
                                        <td>
                                            <span class="<%# Eval("css") %>"><%# Eval("Nazwa") %></span>

                                        </td>
                                        <td>
                                            <span class="<%# Eval("css") %>" title="<%# Eval("DataMailaGodz") %>"><%# Eval("DataMaila") %></span>

                                        </td>
                                        <td>
                                            <span class="<%# Eval("css") %>" title="<%# Eval("DataAkceptacjiGodz") %>"><%# Eval("DataAkceptacji") %></span>

                                        </td>

                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                    <td id="tdEntered" runat="server" class="check" visible="false">
                        <asp:CheckBox ID="cbEntered" runat="server" Checked='<%# Bind("Wprowadzony") %>' OnCheckedChanged="cbEntered_CheckedChanged" AutoPostBack="true" />
                    </td>
                    <td class="control">
                        <asp:Button ID="btSelect" CssClass="button_postback" runat="server" CommandName="Select" Text="Wybierz" />
                        <%--<asp:LinkButton ID="btShow" runat="server" CommandName="Select" CommandArgument='<%# Eval("Id") %>' Text="Pokaż" Visible="true" CssClass="btn btn-small">
                    <i class="glyphicon glyphicon-search"></i>
                        </asp:LinkButton>--%>
                        <asp:Button ID="btShow" runat="server" CommandName="Select" CommandArgument='<%# Eval("Id") %>' Text="Pokaż" Visible="true" CssClass="btn btn-default" />
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false" />
                    </td>
                </tr>



                <asp:SqlDataSource ID="dsSciezkaAkceptacji" runat="server"
                    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                    SelectCommand=" 
  select p.Nazwisko + ' ' + p.Imie as Akceptujacy
  , CONVERT(VARCHAR(10),usa.DataDodania,20) as DataDodania
  , CONVERT(VARCHAR(10),usa.DataAkceptacji,20) as DataAkceptacji
  , CONVERT(VARCHAR(10),usa.DataMaila,20) as DataMaila
  , CONVERT(VARCHAR(19),usa.DataAkceptacji,20) as DataAkceptacjiGodz
  , CONVERT(VARCHAR(19),usa.DataMaila,20) as DataMailaGodz
  , uas.Nazwa
  , uas.CSS
  , uas.Nazwa2
   from UrlopSciezkaAkceptacji usa
  left join Pracownicy p on p.Id = usa.IdAkceptujacy
  left join UrlopAkceptacjaStatus uas on uas.Id = usa.IdStatusu
                      where usa.IdWniosku = @Id">

                    <SelectParameters>
                        <asp:ControlParameter Name="Id" Type="Int32" ControlID="hidWniosekId" />
                    </SelectParameters>

                </asp:SqlDataSource>

            </ItemTemplate>
            <SelectedItemTemplate>
                <asp:HiddenField ID="hidWniosekId" runat="server" Value='<%# Eval("Id") %>' />
                <tr id="trLine" runat="server" class='<%# GetRowClass("sit", Eval("Wprowadzony")) %>'>
                    <td id="tdPrac" runat="server" class="nazwisko_nrew" visible="false">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Pracownik") %>' />
                        <div class="line2">
                            <asp:Button ID="btSelect1_" CssClass="control" runat="server" CommandName="Unselect" Text="Wybierz" Visible="false" />
                            <asp:CheckBox ID="btSelect1" CssClass="control" runat="server" Checked="true" Visible="false" />
                            &nbsp;
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("KadryId") %>' />
                        </div>
                    </td>
                    <td id="tdTyp" runat="server" class="typ">
                        <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                    </td>
                    <td id="tdOd" runat="server" class="data">
                        <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
                    </td>
                    <td id="tdDo" runat="server" class="data">
                        <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
                    </td>

                    <td id="tdDni" runat="server" class="num">
                        <asp:Label ID="DniLabel" runat="server" Text='<%# Eval("DniGodz") %>' />
                    </td>
                    <%--        
            <td id="tdDni" runat="server" class="num">
                <asp:Label ID="DniLabel" runat="server" Text='<%# Eval("Dni") %>' />
            </td>
            <td id="tdGodzin" runat="server" class="num">
                <asp:Label ID="Label11" runat="server" Text='<%# Eval("Godzin") %>' />
            </td>
                    --%>
                    <td id="tdTypOkres" runat="server" class="typokres" visible="false">
                        <asp:Label ID="Label9" runat="server" CssClass="Typ" Text='<%# Eval("AssecoTyp") %>' />
                        <asp:Label ID="Label10" runat="server" CssClass="Typ" Text='<%# Eval("AssecoPodTyp") %>' />
                        -
                <asp:Label ID="Label8" runat="server" CssClass="Symbol" Text='<%# Eval("Symbol") %>' />
                        -
                <asp:Label ID="Label4" runat="server" CssClass="Typ" Text='<%# Eval("Typ") %>' /><br />
                        <asp:Label ID="Label5" runat="server" CssClass="Od" Text='<%# Eval("Od", "{0:d}") %>' />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label6" runat="server" CssClass="Do" Text='<%# Eval("Do", "{0:d}") %>' />

                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dni: 
                <asp:Label ID="Label7" runat="server" CssClass="Dni" Text='<%# Eval("DniGodz") %>' />
                        <%--            
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dni: 
                <asp:Label ID="Label7" runat="server" CssClass="Dni" Text='<%# Eval("Dni") %>' />
                        --%>
                        <%--            
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;godzin: 
                <asp:Label ID="Label12" runat="server" CssClass="Dni" Text='<%# Eval("Godzin") %>' />
                        --%>

                        <div class="buttons">
                            <asp:Button ID="btNext" CssClass="button btn btn-default" runat="server" Text="Wprowadzony, zaznacz następny" OnClick="btNext_Click" />
                            <asp:Button ID="btEksport" CssClass="button btn btn-default" runat="server" Text="Eksportuj, zaznacz następny" OnClick="btEksport_Click" Enabled='<%# (int)Eval("ExportTyp") == 2 %>' />
                        </div>
                    </td>
                    <td id="tdStatus" runat="server" class="status">
                        <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                    </td>
                    <td id="tdKier" runat="server" class="nazwisko_nrew" visible="false">
                        <table>
                            <tr>
                                <th>Akceptujący</th>
                                <th>Status</th>
                                <th>Powiadomienie</th>
                                <th>Akceptacja</th>
                            </tr>
                            <asp:Repeater ID="rpSciezkaAkceptacjiSel" runat="server" DataSourceID="dsSciezkaAkceptacjiSel">
                                <ItemTemplate>
                                   
                                    <tr>
                                        <td>
                                            <span class="<%# Eval("css") %>"><%# Eval("Akceptujacy") %></span>

                                        </td>
                                        <td>
                                            <span class="<%# Eval("css") %>"><%# Eval("Nazwa") %></span>

                                        </td>
                                        <td>
                                            <span class="<%# Eval("css") %>" title="<%# Eval("DataMailaGodz") %>"><%# Eval("DataMaila") %></span>

                                        </td>
                                        <td>
                                            <span class="<%# Eval("css") %>" title="<%# Eval("DataAkceptacjiGodz") %>"><%# Eval("DataAkceptacji") %></span>

                                        </td>

                                    </tr>
                         
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                    <td id="tdEntered" runat="server" class="check" visible="false">
                        <asp:CheckBox ID="cbEntered" runat="server" Checked='<%# Bind("Wprowadzony") %>' OnCheckedChanged="cbEntered_CheckedChanged" AutoPostBack="true" />
                    </td>
                    <td class="control">
                        <asp:Button ID="btSelect" CssClass="button_postback" runat="server" CommandName="Unselect" Text="Wybierz" />
                        <%--<asp:LinkButton ID="btShow" runat="server" CommandName="Select" CommandArgument='<%# Eval("Id") %>' Text="Pokaż" Visible="true">
                    <i class="glyphicon glyphicon-search"></i>
                        </asp:LinkButton>--%>
                        <asp:Button ID="btShow" runat="server" CommandName="Select" CommandArgument='<%# Eval("Id") %>' Text="Pokaż" Visible="true" CssClass="btn btn-default" />
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false" />
                    </td>
                </tr>


                <asp:SqlDataSource ID="dsSciezkaAkceptacjiSel" runat="server"
                    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                    SelectCommand=" 
  select p.Nazwisko + ' ' + p.Imie as Akceptujacy
  , CONVERT(VARCHAR(10),usa.DataDodania,20) as DataDodania
  , CONVERT(VARCHAR(10),usa.DataAkceptacji,20) as DataAkceptacji
  , CONVERT(VARCHAR(10),usa.DataMaila,20) as DataMaila
  , CONVERT(VARCHAR(19),usa.DataAkceptacji,20) as DataAkceptacjiGodz
  , CONVERT(VARCHAR(19),usa.DataMaila,20) as DataMailaGodz
  , uas.Nazwa
  , uas.CSS
  , uas.Nazwa2
   from UrlopSciezkaAkceptacji usa
  left join Pracownicy p on p.Id = usa.IdAkceptujacy
  left join UrlopAkceptacjaStatus uas on uas.Id = usa.IdStatusu
                      where usa.IdWniosku = @Id">

                    <SelectParameters>
                        <asp:ControlParameter Name="Id" Type="Int32" ControlID="hidWniosekId" />
                    </SelectParameters>

                </asp:SqlDataSource>
            </SelectedItemTemplate>
            <EmptyDataTemplate>
                <%-- musi być dłuższe niż ok 700px bo inaczej justuje parenta do lewej zamiast center --%>
                <%--<span class="tbWnioskiUrlopowe_nodata">Brak wniosków
                </span>--%>
                <h5><i class="fa fa-ban"></i>Brak wniosków</h5>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table runat="server" class="tbWnioskiUrlopowe narrow">
                    <tr runat="server">
                        <td runat="server">
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="" class="">
                                <tr id="Tr2" runat="server" style="">
                                    <th id="thPrac" runat="server" visible="false">
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Pracownik" Text="Pracownik" />
                                        /
                                <asp:LinkButton ID="LinkButton13" runat="server" CommandName="Sort" CommandArgument="KadryId" Text="Nr ew." />
                                    </th>
                                    <th id="thTyp" runat="server">
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Typ" Text="Typ" />
                                    </th>
                                    <th id="thOd" runat="server">
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Od" Text="Od" />
                                    </th>
                                    <th id="thDo" runat="server">
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Do" Text="Do" />
                                    </th>
                                    <th id="thDni" runat="server" class="dni">
                                        <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Dni" Text="Il.dni" />
                                    </th>
                                    <%--                        
                            <th id="thGodzin" runat="server" class="dni">
                                <asp:LinkButton ID="LinkButton14" runat="server" CommandName="Sort" CommandArgument="Godzin" Text="Godzin"/>                                
                            </th>
                                    --%>
                                    <th id="thTypOkres" runat="server" class="typokres" visible="false">
                                        <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Symbol" Text="Typ" />
                                        /
                                <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Od" Text="Od" />
                                        -
                                <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="Do" Text="Do" />
                                        /
                                <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="Dni" Text="Ilość dni" />
                                        -
                                <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="Godzin" Text="Godzin" />
                                    </th>
                                    <th id="thStatus" runat="server">
                                        <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status" />
                                    </th>
                                    <th id="thKier" runat="server" visible="false">
                                        <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Kierownik" Text="Ścieżka akceptacji" />
                                    </th>
                                    <th id="thEntered" runat="server" visible="false">
                                        <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="Wprowadzony" Text="Ok" />
                                    </th>
                                    <th id="Th6" runat="server" class="control"></th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="pager">
                        <td class="left">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                    <asp:NumericPagerField ButtonType="Link" />
                                    <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                                </Fields>
                            </asp:DataPager>
                        </td>
                        <td class="right">
                            <span class="count">Ilość wniosków:<asp:Label ID="lbCount" runat="server"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>

    </ContentTemplate>
</asp:UpdatePanel>

<div id="paButtons" runat="server" class="cntWnioskiUrlopowe_buttons" visible="false">
    <asp:Button ID="btImport" runat="server" CssClass="button250 btn btn-primary" Text="Import absencji z Asseco HR" ToolTip="Import absencji z systemu AssecoHR, nie są importowane pozostałe dane." OnClick="btImport_Click" />
    <asp:Button ID="btImportBAAN" runat="server" CssClass="button250 btn btn-primary" Text="Import absencji z systemu BAAN" ToolTip="Import absencji z systemu BAAN, nie są importowane pozostałe dane." OnClick="btImport_Click" Visible="false" />
    <asp:Button ID="btImport2" runat="server" CssClass="button_postback btn btn-primary" Text="Import" OnClick="btImport_Click" OnClientClick="javascript:window.setTimeout(function(){showAjaxProgress();}, 1000);return true;" />
    <%--
    <asp:Button ID="btImport" runat="server" CssClass="button250" Text="Import danych z Asseco HR" ToolTip="Import absencji, stawek wynagrodzenia i kalendarza z nowego systemu HR" onclick="btImport_Click" />
    --%>
</div>

<%-- wnioski pracownika --%>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    DeleteCommand="delete from poWnioskiUrlopowe where Id = @Id"
    SelectCommand="
SELECT 
	null as AssecoTyp, null as AssecoPodTyp, 
    W.*, 

    case when W.TypId = 4 and W.Dni = 1 and W.Godzin != 8 then convert(varchar, W.Godzin) + 'h' else convert(varchar, W.Dni) end DniGodz,

	T.Symbol, T.Typ, ST.Status, T.ExportTyp,
    null as Pracownik, null as KadryId,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId --akceptujacy
FROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy K on K.Id = -- pierwszy z uprawnieniem do akceptowania chyba ze juz jest
    case when W.IdKierAcc is null then 
        case when W.TypId = 20 then 
            (select top 1 Id from Pracownicy where dbo.GetRightId(Rights, 117) = 1) 
        else
            (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1))
        end
    else W.IdKierAcc
    end
            /*
    ISNULL(W.IdKierAcc, 
            (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)))
            */
            /*
	        (select top 1 PP.IdPracownika from dbo./*fn_GetUpPrzypisania*/{0}(W.IdPracownika, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))) PP
	         left outer join Pracownicy KK on KK.Id = PP.IdPracownika
	         where PP.IdPracownika &lt;&gt; W.IdPRacownika and SUBSTRING(KK.Rights, 14, 1) = '1'))
	         */
--left outer join Pracownicy P on P.Id = W.IdPracownika
--left outer join Pracownicy Z on Z.Id = W.IdZastepuje
--left outer join Pracownicy KA on KA.Id = W.IdKierAcc
--left outer join Pracownicy KZ on KZ.Id = W.IdKierAccZast
--left outer join Pracownicy A on A.Id = W.DataKadryAcc
--left outer join Dzialy D on D.Id = P.IdDzialu
--left outer join Stanowiska S on S.Id = P.IdStanowiska
WHERE 
    isnull(T.Rodzaj, 0) = isnull(@rodzaj, 0) and
    (@typy is null or w.TypId in (select items from dbo.splitInt(@typy, ',')) )
    and IdPracownika = @IdPracownika
    and (
        @Status = W.StatusId or
        @Status = 9
    ) 
--ORDER BY [DataWniosku] DESC
ORDER BY Od DESC
    ">
    <SelectParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:ControlParameter ControlID="hidTypy" PropertyName="Value" Name="typy" Type="String" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter ControlID="hidRodzaj" PropertyName="Value" Name="rodzaj" Type="Int32" DefaultValue="0" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>

<%-- wnioski kierownika (pracownicy z podstruktury) --%>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    DeleteCommand="delete from poWnioskiUrlopowe where Id = @Id"
    SelectCommand="
SELECT 
    null as AssecoTyp, null as AssecoPodTyp,
    W.*, 

    case when W.TypId = 4 and W.Dni = 1 and W.Godzin != 8 then convert(varchar, W.Godzin) + 'h' else convert(varchar, W.Dni) end DniGodz,

	T.Symbol, T.Typ, ST.Status, T.ExportTyp, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId, P.Nazwisko, P.Imie,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId, 
	case when K.Id = @IdKierownika then 0 else 1 end as MoiSort
FROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = -- pierwszy z uprawnieniem do akceptowania chyba ze juz jest
    /*case when W.IdKierAcc is null then 
        (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1))
    else W.IdKierAcc*/
    case when W.IdKierAcc is null then 
        case when W.TypId = 20 then 
            (select top 1 Id from Pracownicy where dbo.GetRightId(Rights, 117) = 1) 
        else
            (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1))
        end
    else W.IdKierAcc
    end
            /*
    ISNULL(W.IdKierAcc, 
            (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)))
            */
            /*
	        (select top 1 PP.IdPracownika from dbo./*fn_GetUpPrzypisania*/{0}(W.IdPracownika, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))) PP
	         left outer join Pracownicy KK on KK.Id = PP.IdPracownika
	         where PP.IdPracownika &lt;&gt; W.IdPRacownika and SUBSTRING(KK.Rights, 14, 1) = '1'))
	         */
where isnull(T.Rodzaj, 0) = isnull(@rodzaj, 0) and
    (@typy is null or w.TypId in (select items from dbo.splitInt(@typy, ',')) ) and
    (@Status = 8 and W.AutorId = @IdKierownika and W.AutorId &lt;&gt; W.IdPracownika or
      @Status &lt;&gt; 8 and 
         (@rSub = 1 and W.IdPracownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@IdKierownika, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))) or
          @rSub &lt;&gt; 1 and W.IdPracownika in (select IdPracownika from /* Przypisania */ {1} where Status=1 and DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) between Od and ISNULL(Do, '20990909') and IdKierownika = @IdKierownika))
              and (@Status = W.StatusId or
                   @Status = 34 and W.StatusId in (3,4)))
ORDER BY MoiSort, W.Od desc, Pracownik
    ">
    <SelectParameters>
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="rSub" Type="Int32" />
        <asp:ControlParameter ControlID="hidTypy" PropertyName="Value" Name="typy" Type="String" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter ControlID="hidRodzaj" PropertyName="Value" Name="rodzaj" Type="Int32" DefaultValue="0" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>

<%-- wszystkie wnioski dla admina o określonym statusie --%>
<asp:SqlDataSource ID="SqlDataSource3" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    DeleteCommand="delete from poWnioskiUrlopowe where Id = @Id"
    UpdateCommand="update poWnioskiUrlopowe set Wprowadzony = @Wprowadzony where Id = @Id"
    SelectCommand="
SET ARITHABORT OFF    
select W.*, 

    case when W.TypId = 4 and W.Dni = 1 and W.Godzin != 8 then convert(varchar, W.Godzin) + 'h' else convert(varchar, W.Dni) end DniGodz,

	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId, P.Nazwisko, P.Imie,
	case when W.IdKierAcc is null then UP.Kierownik else K.Nazwisko + ' ' + K.Imie end as Kierownik, 
	case when W.IdKierAcc is null then UP.KierKadryId else K.KadryId end as KierKadryId
from 
(
SELECT 
	null as AssecoTyp, null as AssecoPodTyp, 
	--T.Typ2, T.PodTyp2, KO.Nazwa, KO.Nazwa2,
    W.*, 
	T.Symbol, T.Typ, ST.Status, T.ExportTyp
FROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
where isnull(T.Rodzaj, 0) = isnull(@rodzaj, 0) and 
    (
    @Status = 8 and W.AutorId = @IdAdm and W.AutorId &lt;&gt; W.IdPracownika or
      @Status &lt;&gt; 8 and 
         (@Status = W.StatusId or
          @Status = 34 and W.StatusId in (3,4) or
          @Status = 99)
    )
    
) W
outer apply (
	select top 1 T.Id, T.Nazwisko + ' ' + T.Imie as Kierownik, T.KadryId as KierKadryId from (select 1 as Id) A 
	outer apply (select * from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)) T
	where W.IdKierAcc is null 
) UP
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = W.IdKierAcc
--left outer join Absencja A on A.IdPracownika = W.IdPracownika and A.DataOd = W.Od and A.DataDo = W.Do
--left outer join AbsencjaKody AK on AK.Kod = A.Kod
ORDER BY W.Od desc,Pracownik
    "
    OnSelected="SqlDataSource3_Selected"
    OnSelecting="SqlDataSource3_Selecting">
    <SelectParameters>
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="IdAdm" Type="Int32" />
        <asp:ControlParameter ControlID="hidRodzaj" PropertyName="Value" Name="rodzaj" Type="Int32" DefaultValue="0" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Wprowadzony" Type="Boolean" />
    </UpdateParameters>
</asp:SqlDataSource>

<%--
SET ARITHABORT OFF    
SELECT 
	null as AssecoTyp, null as AssecoPodTyp, 
	--T.Typ2, T.PodTyp2, KO.Nazwa, KO.Nazwa2,
    W.*, 
	T.Symbol, T.Typ, ST.Status, T.ExportTyp, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId, P.Nazwisko, P.Imie,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId
FROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika

--outer apply (select Id from dbo.fn_GetUpKierWithRight2(W.IdPracownika, GETDATE(), 13, 1) where W.IdKierAcc is null) UP

outer apply (
	select top 1 T.Id from (select 1 as Id) A 
	outer apply (select * from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)) T
	where W.IdKierAcc is null 
) UP

left outer join Pracownicy K on K.Id = -- pierwszy z uprawnieniem
    case when W.IdKierAcc is null then 
        --(select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1))
        UP.Id
        --0
    else W.IdKierAcc
    end
    /*
    ISNULL(W.IdKierAcc, 
        (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)))
    */    
    /*
	(select top 1 PP.IdPracownika from dbo.fn_GetUpPrzypisania(W.IdPracownika, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))) PP
	 left outer join Pracownicy KK on KK.Id = PP.IdPracownika
	 where PP.IdPracownika &lt;&gt; W.IdPRacownika and SUBSTRING(KK.Rights, 14, 1) = '1')
	 */
left outer join Absencja A on A.IdPracownika = W.IdPracownika and A.DataOd = W.Od and A.DataDo = W.Do
left outer join AbsencjaKody AK on AK.Kod = A.Kod
where @Status = 8 and W.AutorId = @IdAdm and W.AutorId &lt;&gt; W.IdPracownika or
      @Status &lt;&gt; 8 and 
         (@Status = W.StatusId or
          @Status = 34 and W.StatusId in (3,4) or
          @Status = 99)
ORDER BY W.Od desc,Pracownik
--%>

<asp:SqlDataSource ID="SqlDataSourceToEnter" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    DeleteCommand="delete from poWnioskiUrlopowe where Id = @Id"
    UpdateCommand="update poWnioskiUrlopowe set Wprowadzony = @Wprowadzony where Id = @Id"
    SelectCommand="
SELECT 
	ISNULL(LEFT(KO.Nazwa2, 1), T.Typ2) as AssecoTyp, 
	ISNULL(SUBSTRING(KO.Nazwa2, 3, 99), T.PodTyp2) as AssecoPodTyp, 
	--T.Typ2, T.PodTyp2, KO.Nazwa, KO.Nazwa2,
    W.*, 

    case when W.TypId = 4 and W.Dni = 1 and W.Godzin != 8 then convert(varchar, W.Godzin) + 'h' else convert(varchar, W.Dni) end DniGodz,

	T.Symbol, T.Typ, ST.Status, T.ExportTyp, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId, P.Nazwisko, P.Imie,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId
fROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = W.IdKierAcc
left join Kody KO on KO.Typ = 'URLOP_OKOL' and KO.Kod = W.PodTyp
where W.StatusId in (3) and isnull(T.Rodzaj, 0) = isnull(@rodzaj, 0)
ORDER BY W.Od,Pracownik
    ">
    <%--
SELECT W.*, 
	T.Symbol, T.Typ, ST.Status, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId
fROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = W.IdKierAcc
left outer join Absencja A on A.IdPracownika = W.IdPracownika and 
(A.DataOd = W.Od and A.IleDni = W.Dni or
 A.DataDo = W.Do and A.IleDni = W.Dni)
where W.StatusId in (3,4) 
and A.Id is null
ORDER BY W.Od
    --%>
    <SelectParameters>
        <asp:ControlParameter ControlID="hidRodzaj" PropertyName="Value" Name="rodzaj" Type="Int32" DefaultValue="0" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Wprowadzony" Type="Boolean" />
    </UpdateParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceEntered" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    DeleteCommand="delete from poWnioskiUrlopowe where Id = @Id"
    UpdateCommand="update poWnioskiUrlopowe set Wprowadzony = @Wprowadzony where Id = @Id"
    SelectCommand="
SELECT 
	ISNULL(LEFT(KO.Nazwa2, 1), T.Typ2) as AssecoTyp, 
	ISNULL(SUBSTRING(KO.Nazwa2, 3, 99), T.PodTyp2) as AssecoPodTyp, 
	--T.Typ2, T.PodTyp2, KO.Nazwa, KO.Nazwa2,
    W.*, 

    case when W.TypId = 4 and W.Dni = 1 and W.Godzin != 8 then convert(varchar, W.Godzin) + 'h' else convert(varchar, W.Dni) end DniGodz,

	T.Symbol, T.Typ, ST.Status, T.ExportTyp, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId, P.Nazwisko, P.Imie,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId
fROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = W.IdKierAcc
left join Kody KO on KO.Typ = 'URLOP_OKOL' and KO.Kod = W.PodTyp
where W.StatusId in (4) and W.Wprowadzony = 1
and isnull(T.Rodzaj, 0) = isnull(@rodzaj, 0)
ORDER BY W.Od desc,Pracownik
    ">
    <%--
SELECT W.*, 
	T.Symbol, T.Typ, ST.Status, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId
fROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = W.IdKierAcc
left outer join Absencja A on A.IdPracownika = W.IdPracownika and 
(A.DataOd = W.Od and A.IleDni = W.Dni or
 A.DataDo = W.Do and A.IleDni = W.Dni)
where W.StatusId in (3,4) 
and A.Id is not null
ORDER BY W.Od
    --%>
    <SelectParameters>
        <asp:ControlParameter ControlID="hidRodzaj" PropertyName="Value" Name="rodzaj" Type="Int32" DefaultValue="0" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Wprowadzony" Type="Boolean" />
    </UpdateParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceToVerify" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    DeleteCommand="delete from poWnioskiUrlopowe where Id = @Id"
    UpdateCommand="update poWnioskiUrlopowe set Wprowadzony = @Wprowadzony where Id = @Id"
    SelectCommand="
SELECT 
	ISNULL(LEFT(KO.Nazwa2, 1), T.Typ2) as AssecoTyp, 
	ISNULL(SUBSTRING(KO.Nazwa2, 3, 99), T.PodTyp2) as AssecoPodTyp, 
	--T.Typ2, T.PodTyp2, KO.Nazwa, KO.Nazwa2,
    W.*, 

    case when W.TypId = 4 and W.Dni = 1 and W.Godzin != 8 then convert(varchar, W.Godzin) + 'h' else convert(varchar, W.Dni) end DniGodz,

	T.Symbol, T.Typ, ST.Status, T.ExportTyp, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId, P.Nazwisko, P.Imie,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId
fROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = W.IdKierAcc
left join Kody KO on KO.Typ = 'URLOP_OKOL' and KO.Kod = W.PodTyp
where W.StatusId in (3) and W.Wprowadzony = 1   -- nie ustawil sie status 
ORDER BY W.Od,Pracownik
    ">
    <%--
SELECT W.*, 
	T.Symbol, T.Typ, ST.Status, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId
fROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = W.IdKierAcc
left outer join Absencja A on A.IdPracownika = W.IdPracownika and 
(A.DataOd = W.Od and A.IleDni = W.Dni or
 A.DataDo = W.Do and A.IleDni = W.Dni)
where W.StatusId in (3,4) 
and A.Id is null and W.Wprowadzony = 1
ORDER BY W.Od
    --%>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Wprowadzony" Type="Boolean" />
    </UpdateParameters>
</asp:SqlDataSource>













<%--
            <td>
                <asp:Label ID="DataWnioskuLabel" runat="server" Text='<%# Eval("DataWniosku") %>' />
            </td>
            <td>
                <asp:Label ID="GodzinLabel" runat="server" Text='<%# Eval("Godzin") %>' />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>

            <td>
                <asp:Label ID="IdPracownikaLabel" runat="server" 
                    Text='<%# Eval("IdPracownika") %>' />
            </td>
            <td>
                <asp:Label ID="IdZastepujeLabel" runat="server" 
                    Text='<%# Eval("IdZastepuje") %>' />
            </td>
            <td>
                <asp:Label ID="IdPrzelozonyLabel" runat="server" 
                    Text='<%# Eval("IdPrzelozony") %>' />
            </td>
            <td>
                <asp:Label ID="ProjektDzialLabel" runat="server" 
                    Text='<%# Eval("ProjektDzial") %>' />
            </td>
            <td>
                <asp:Label ID="StanowiskoLabel" runat="server" 
                    Text='<%# Eval("Stanowisko") %>' />
            </td>
            <td>
                <asp:Label ID="InfoLabel" runat="server" Text='<%# Eval("Info") %>' />
            </td>
            <td>
                <asp:Label ID="UzasadnienieLabel" runat="server" 
                    Text='<%# Eval("Uzasadnienie") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierAccLabel" runat="server" Text='<%# Eval("IdKierAcc") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierAccZastLabel" runat="server" 
                    Text='<%# Eval("IdKierAccZast") %>' />
            </td>
            <td>
                <asp:Label ID="DataAccLabel" runat="server" Text='<%# Eval("DataAcc") %>' />
            </td>
            <td>
                <asp:Label ID="UzasadnienieKierLabel" runat="server" 
                    Text='<%# Eval("UzasadnienieKier") %>' />
            </td>            
--%>



<%-- wnioski kierownika (pracownicy z podstruktury) --%>
<asp:SqlDataSource ID="dsPracaZdalna" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    DeleteCommand="delete from poWnioskiUrlopowe where Id = @Id"
    SelectCommand="
SELECT 
    null as AssecoTyp, null as AssecoPodTyp,
    W.*, 

    case when W.TypId = 4 and W.Dni = 1 and W.Godzin != 8 then convert(varchar, W.Godzin) + 'h' else convert(varchar, W.Dni) end DniGodz,

	T.Symbol, T.Typ, ST.Status, T.ExportTyp, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId, P.Nazwisko, P.Imie,
	K.Nazwisko + ' ' + K.Imie as Kierownik, K.KadryId as KierKadryId, 
	case when K.Id = @IdKierownika then 0 else 1 end as MoiSort
FROM poWnioskiUrlopowe W
left outer join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left outer join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left outer join Pracownicy P on P.Id = W.IdPracownika
left outer join Pracownicy K on K.Id = -- pierwszy z uprawnieniem do akceptowania chyba ze juz jest
    case when W.IdKierAcc is null then 
        case when W.TypId = 20 then 
            (select top 1 Id from Pracownicy where dbo.GetRightId(Rights, 117) = 1) 
        else
            (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1))
        end
    else W.IdKierAcc
    end
where @Status = W.StatusId and T.Rodzaj = 1
 /*   (@typy is null or w.TypId in (select items from dbo.splitInt(@typy, ',')) ) and
    (@Status = 8 and W.AutorId = @IdKierownika and W.AutorId &lt;&gt; W.IdPracownika or
      @Status &lt;&gt; 8 and 
         (@rSub = 1 and W.IdPracownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@IdKierownika, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))) or
          @rSub &lt;&gt; 1 and W.IdPracownika in (select IdPracownika from /* Przypisania */ {0} where Status=1 and DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) between Od and ISNULL(Do, '20990909') and IdKierownika = @IdKierownika))
              and (@Status = W.StatusId or
                   @Status = 34 and W.StatusId in (3,4)))*/
ORDER BY MoiSort, W.Od desc, Pracownik
    ">
    <SelectParameters>
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />

    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>

