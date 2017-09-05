<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzypisania.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Przypisania.cntPrzypisania" %>
<%@ Register src="~/Controls/LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>
<%@ Register src="cntSplityWsp.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>

<asp:HiddenField ID="hidKierId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidData" runat="server" Visible="false" />
<asp:HiddenField ID="hidStatus" runat="server" Visible="false" />
<asp:HiddenField ID="hidSub" runat="server" Visible="false" />

<div id="paFilter" runat="server" class="paFilter" visible="true">
    <span class="label">Wyszukaj pracownika:</span>
    <asp:TextBox ID="tbSearch" CssClass="search textbox form-control" runat="server" ></asp:TextBox>
    <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" />
    &nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="cbShowAll" runat="server" CssClass="check" Text="Pokaż zaimportowane" AutoPostBack="True" oncheckedchanged="cnt_ChangeFilter"/><br />
    <span class="label">Pracownik:</span>
    <asp:DropDownList ID="ddlPracownik" runat="server" CssClass="form-control" DataSourceID="SqlDataSourcePrac" DataTextField="Pracownik" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="cnt_ChangeFilter" >
    </asp:DropDownList><br />
    <span class="label">Od kierownika:</span>
    <asp:DropDownList ID="ddlKierownikOd" runat="server" CssClass="form-control" DataSourceID="SqlDataSourceKier" DataTextField="Kierownik" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="cnt_ChangeFilter" >
    </asp:DropDownList><br />
    <span class="label">Do kierownika:</span>
    <asp:DropDownList ID="ddlKierownikDo" runat="server" CssClass="form-control" DataSourceID="SqlDataSourceKier" DataTextField="Kierownik" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="cnt_ChangeFilter" >
    </asp:DropDownList><br />
    <span class="label">Data przesunięcia od:</span>
    <uc2:DateEdit ID="deGreaterThan" runat="server" AutoPostBack="true" OnDateChanged="cnt_ChangeFilter" />    
</div>

<asp:SqlDataSource ID="SqlDataSourcePrac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Pracownik, 0 as Sort
union    
select Id, Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Pracownik, 1 as Sort
from Pracownicy 
order by Sort, Pracownik
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Kierownik, 0 as Sort
union    
select Id, Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Kierownik, 1 as Sort
from Pracownicy 
where Kierownik=1
order by Sort, Kierownik
    ">
</asp:SqlDataSource>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />

        <asp:ListView ID="lvPrzypisania" runat="server" DataKeyNames="Id" 
            DataSourceID="SqlDataSource1" InsertItemPosition="None" 
            ondatabound="lvPrzypisania_DataBound" 
            onitemcreated="lvPrzypisania_ItemCreated" 
            onitemdatabound="lvPrzypisania_ItemDataBound" 
            onitemediting="lvPrzypisania_ItemEditing" 
            onitemupdating="lvPrzypisania_ItemUpdating" 
            onitemcommand="lvPrzypisania_ItemCommand" 
            onlayoutcreated="lvPrzypisania_LayoutCreated" 
            onsorting="lvPrzypisania_Sorting" 
            oniteminserted="lvPrzypisania_ItemInserted" 
            onitemdeleting="lvPrzypisania_ItemDeleting" 
            onitemupdated="lvPrzypisania_ItemUpdated" 
            onitemdeleted="lvPrzypisania_ItemDeleted" 
            onselectedindexchanged="lvPrzypisania_SelectedIndexChanged" 
            ondatabinding="lvPrzypisania_DataBinding">
            <ItemTemplate>
                <tr class="it">
                    <td class="nazwisko_nrew">
                        <asp:Label ID="Label1" runat="server" CssClass="bold" Text='<%# Eval("Pracownik") %>' /><br />
                        <asp:Label ID="Label2" runat="server" CssClass="line2" Text='<%# Eval("KadryId") %>' />
                    </td>
                    <td class="przesuniecie">
                        <asp:Label ID="Label4" runat="server" CssClass="gray" Text='<%# Eval("Kierownik1") %>' /> <span class="gray">→</span> 
                        <asp:Label ID="Label3" runat="server" CssClass="bold" Text='<%# Eval("Kierownik2") %>' /><br />
                        <span class="line2 right">
                            <asp:Label ID="Label13" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /> - 
                            <asp:Label ID="Label14" runat="server" Text='<%# BezTerminu(Eval("Do2")) %>' />
                        </span>
                    </td>
                    <td class="kierrq" title='<%# Eval("UwagiRq") %>' >
                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("KierRq") %>' /><br />
                        <asp:Label ID="Label16" runat="server" CssClass="line2" Text='<%# Eval("DataRq", "{0:d}") %>' ToolTip='<%# Eval("DataRq", "{0:T}") %>'/>
                    </td>
                    <td class="kieracc" title='<%# Eval("UwagiAcc") %>'>
                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("KierAcc") %>' /><br />
                        <asp:Label ID="Label18" runat="server" CssClass="line2" Text='<%# Eval("DataAcc", "{0:d}") %>' ToolTip='<%# Eval("DataAcc", "{0:T}") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("StatusNazwa") %>' /><br />
                        <asp:Label ID="Label9" runat="server" CssClass="line2 right" Text='<%# Eval("TypNazwa") %>' />
                    </td>
                    <td id="tdControl" runat="server" class="control" >
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Eval("IdPracownika") %>' />
                        <asp:Button ID="btDetails" runat="server" CommandName="Select" Text="Szczegóły" />
                    </td>
                </tr>
            </ItemTemplate>
            <SelectedItemTemplate>
                <tr class="it selected">
                    <td class="nazwisko_nrew">
                        <asp:Label ID="Label1" runat="server" CssClass="bold" Text='<%# Eval("Pracownik") %>' /><br />
                        <asp:Label ID="Label2" runat="server" CssClass="line2" Text='<%# Eval("KadryId") %>' />
                    </td>
                    <td class="przesuniecie">
                        <asp:Label ID="Label4" runat="server" CssClass="gray" Text='<%# Eval("Kierownik1") %>' /> <span class="gray">→</span> 
                        <asp:Label ID="Label3" runat="server" CssClass="bold" Text='<%# Eval("Kierownik2") %>' /><br />
                        <span class="line2 right">
                            <asp:Label ID="Label13" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /> - 
                            <asp:Label ID="Label14" runat="server" Text='<%# BezTerminu(Eval("Do2")) %>' />
                        </span>
                    </td>
                    <td class="kierrq" title='<%# Eval("UwagiRq") %>' >
                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("KierRq") %>' /><br />
                        <asp:Label ID="Label16" runat="server" CssClass="line2" Text='<%# Eval("DataRq", "{0:d}") %>' ToolTip='<%# Eval("DataRq", "{0:T}") %>'/>
                    </td>
                    <td class="kieracc" title='<%# Eval("UwagiAcc") %>'>
                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("KierAcc") %>' /><br />
                        <asp:Label ID="Label18" runat="server" CssClass="line2" Text='<%# Eval("DataAcc", "{0:d}") %>' ToolTip='<%# Eval("DataAcc", "{0:T}") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("StatusNazwa") %>' /><br />
                        <asp:Label ID="Label9" runat="server" CssClass="line2 right" Text='<%# Eval("TypNazwa") %>' />
                    </td>
                    <td id="tdControl" runat="server" class="control" rowspan="2">
                        <asp:Button ID="Button1" runat="server" CommandName="Deselect" Text="Ukryj"/>
                    </td>
                </tr>
            
                <%--
                <tr class="it selected">
                    <td class="nazwisko_nrew">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Pracownik") %>' /><br />
                        <asp:Label ID="Label2" runat="server" CssClass="line2" Text='<%# Eval("KadryId") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" CssClass="gray" Text='<%# Eval("Kierownik1") %>' /> <span class="gray">→</span> 
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("Kierownik2") %>' /><br />
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /> - 
                        <asp:Label ID="Label8" runat="server" Text='<%# BezTerminu(Eval("Do")) %>' />
                    </td>
                    <td class="kierrq" title='<%# Eval("UwagiRq") %>' >
                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("KierRq") %>' /><br />
                        <asp:Label ID="Label16" runat="server" CssClass="line2" Text='<%# Eval("DataRq", "{0:d}") %>' ToolTip='<%# Eval("DataRq", "{0:T}") %>' />
                    </td>
                    <td class="kieracc" title='<%# Eval("UwagiAcc") %>'>
                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("KierAcc") %>' /><br />
                        <asp:Label ID="Label18" runat="server" CssClass="line2" Text='<%# Eval("DataAcc", "{0:d}") %>' ToolTip='<%# Eval("DataAcc", "{0:T}") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("StatusNazwa") %>' />
                    </td>
                    <td id="tdControl" runat="server" class="control" rowspan="2">
                        <asp:Button ID="Button1" runat="server" CommandName="Deselect" Text="Ukryj"/>
                    </td>
                </tr>
                --%>
                <tr class="it selected selected2">
                    <td colspan="5" class="cntPrzypisaniaParametry">
                        <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("Status") %>' />
                        <asp:HiddenField ID="hidTyp" runat="server" Value='<%# Bind("Typ") %>' />                
                        <asp:HiddenField ID="hidPracId" runat="server" Value='<%# Eval("IdPracownika") %>' />       

                        <span class="label">Od:</span>
                        <asp:Label ID="lbOd" runat="server" CssClass="value" Text='<%# Eval("Od", "{0:d}") %>' />
                        <span class="label1">Do:</span>
                        <asp:Label ID="lbDo" runat="server" CssClass="value" Text='<%# BezTerminu(Eval("Do2")) %>' /><br />
                        
                        <div id="paStrefa" runat="server" visible="false">    
                            <span class="label">Strefa RCP:</span>
                            <asp:Label ID="lbStrefa" runat="server" CssClass="value" Text='<%# Eval("RcpStrefa2")%>' />
                            <br />
                        </div>
                        
                        <span class="label">CC:</span>
                        <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="0" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
                        <br />
                        
                        <div id="paCommodity" runat="server">
                            <asp:Label ID="lbComm" runat="server" CssClass="label" Text="Commodity:" />
                            <asp:Label ID="lbCommodity" runat="server" CssClass="value" Text='<%# Eval("Commodity2")%>' />
                            <br />
                        </div>
                        <div id="paArea" runat="server">    
                            <span class="label">Area:</span>
                            <asp:Label ID="lbArea" runat="server" CssClass="value" Text='<%# Eval("Area2")%>' />
                            <br />
                            
                            <span class="label">Position:</span>
                            <asp:Label ID="lbPosition" runat="server" CssClass="value" Text='<%# Eval("Position2")%>' />
                            <br />
                        </div>
                                
                        <span class="label2">Uzasadnienie wniosku:</span>
                        <asp:Label ID="lbUwagi" runat="server" CssClass="ramka uwagi" Text='<%# Eval("UwagiRq") %>' />
                        
                        <span class="label2">Uzasadnienie odrzucenia / Uwagi:</span>
                        <asp:Label ID="lbUwagiAcc" runat="server" CssClass="ramka uwagi" Text='<%# Eval("UwagiAcc") %>' />
                    </td>
                </tr>
            </SelectedItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" style="">
                    <tr>
                        <td>
                            Brak danych
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <tr class="eit">
                    <td class="nazwisko_nrew">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Pracownik") %>' /><br />
                        <asp:Label ID="Label2" runat="server" CssClass="line2" Text='<%# Eval("KadryId") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" CssClass="gray" Text='<%# Eval("Kierownik1") %>' /> <span class="gray">→</span> 
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("Kierownik2") %>' /><br />
                        <span class="line2 right">
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /> - 
                            <asp:Label ID="Label8" runat="server" Text='<%# BezTerminu(Eval("Do2")) %>' />
                        </span>                        
                    </td>
                    <td class="kierrq" title='<%# Eval("UwagiRq") %>' >
                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("KierRq") %>' /><br />
                        <asp:Label ID="Label16" runat="server" CssClass="line2" Text='<%# Eval("DataRq", "{0:d}") %>' ToolTip='<%# Eval("DataRq", "{0:T}") %>'/>
                    </td>
                    <td class="kieracc" title='<%# Eval("UwagiAcc") %>'>
                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("KierAcc") %>' /><br />
                        <asp:Label ID="Label18" runat="server" CssClass="line2" Text='<%# Eval("DataAcc", "{0:d}") %>' ToolTip='<%# Eval("DataAcc", "{0:T}") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("StatusNazwa") %>' /><br />
                        <asp:Label ID="Label9" runat="server" CssClass="line2 right" Text='<%# Eval("TypNazwa") %>' />
                    </td>
                    <td id="tdControl" runat="server" class="control" visible='<%# IsControlVisible %>' rowspan="2">
                        <asp:Button ID="btUpdate" runat="server" CommandName="Update" CommandArgument="0" Text="Zapisz" />
                        <asp:Button ID="btCancel" runat="server" CommandName="Cancel" Text="Anuluj" />
                        <asp:Button ID="btAccept" runat="server" CommandName="Update" CommandArgument="1" Text="Zaakceptuj" ValidationGroup="evg" />
                        <asp:Button ID="btReject" runat="server" CommandName="Update" CommandArgument="2" Text="Odrzuć" ValidationGroup="xevg" />
                    </td>
                </tr>        
                <tr class="eit">
                    <td colspan="5" class="cntPrzypisaniaParametry">
                        <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("Status") %>' />
                        <asp:HiddenField ID="hidTyp" runat="server" Value='<%# Bind("Typ") %>' />                
                        <asp:HiddenField ID="hidPracId" runat="server" Value='<%# Eval("IdPracownika") %>' />       
                        <asp:HiddenField ID="hidKierAcc" runat="server" Value='<%# Bind("IdKierownikaAcc") %>' />       
                        <asp:HiddenField ID="hidDataAcc" runat="server" Value='<%# Bind("DataAcc") %>' />       

                        <span class="label">Od:</span>
                        <uc2:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' Visible="false"/>
                        <asp:Label ID="lbOd" runat="server" CssClass="value" Text='<%# Eval("Od", "{0:d}") %>' />
                        <span class="label1">Do:</span>
                        <uc2:DateEdit ID="deTo" runat="server" Date='<%# Bind("Do") %>' Visible="false"/>
                        <asp:Label ID="lbDo" runat="server" CssClass="value" Text='<%# BezTerminu(Eval("Do2")) %>' />
                        
                        <asp:Label ID="lbMonit" runat="server" CssClass="label1" Text="Monituj:" Visible="false" />
                        <uc2:DateEdit ID="deMonit" runat="server" Date='<%# Bind("DoMonit") %>' Visible="false" /><br />

                        <div id="paStrefa" runat="server" visible="false">    
                            <span class="label">Strefa RCP:</span>
                            <asp:Label ID="lbStrefa" runat="server" CssClass="value" Text='<%# Eval("RcpStrefa2")%>' Visible="false" />
                            <asp:DropDownList ID="ddlStrefa" runat="server" DataSourceID="SqlDataSource6" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("RcpStrefaId") %>'>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlStrefa" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator5" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                        </div>
                        
                        <span class="label">CC:</span>                
                        <uc6:cntSplityWsp ID="cntSplityWsp2" Mode="1" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
                        <br />
                        
                        <div id="paCommodity" runat="server">
                            <asp:Label ID="lbComm" runat="server" CssClass="label" Text="Commodity:" />
                            <asp:Label ID="lbCommodity" runat="server" CssClass="value" Text='<%# Eval("Commodity2")%>' Visible="false" />
                            <asp:DropDownList ID="ddlCommodity" runat="server" DataSourceID="SqlDataSource3" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("IdCommodity") %>'>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlCommodity" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                        </div>
                        <div id="paArea" runat="server">
                            <span class="label">Area:</span>
                            <asp:Label ID="lbArea" runat="server" CssClass="value" Text='<%# Eval("Area2")%>' Visible="false" />
                            <asp:DropDownList ID="ddlArea" runat="server" DataSourceID="SqlDataSource4" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("IdArea") %>'>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlArea" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                            
                            <span class="label">Position:</span>
                            <asp:Label ID="lbPosition" runat="server" CssClass="value" Text='<%# Eval("Position2")%>' Visible="false" />
                            <asp:DropDownList ID="ddlPosition" runat="server" DataSourceID="SqlDataSource5" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("IdPosition") %>'>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlPosition" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                            <br />
                        </div>
                        
                        <span class="label2">Uzasadnienie wniosku:</span>
                        <asp:TextBox ID="tbUwagi" runat="server" CssClass="textbox uwagi" TextMode="MultiLine" Rows="3" MaxLength="200" Text='<%# Bind("UwagiRq") %>' />
                        <asp:Label ID="lbUwagi" runat="server" CssClass="ramka uwagi" Text='<%# Eval("UwagiRq") %>' Visible="false"/>
                        
                        <asp:Label ID="lbUwagiAccLabel" runat="server" CssClass="label2" Text="Uzasadnienie odrzucenia / Uwagi:" />
                        <asp:TextBox ID="tbUwagiAcc" runat="server" CssClass="textbox uwagi" TextMode="MultiLine" Rows="3" MaxLength="200" Text='<%# Bind("UwagiAcc") %>' />
                        <asp:Label ID="lbUwagiAcc" runat="server" CssClass="ramka uwagi" Text='<%# Eval("UwagiAcc") %>' Visible="false"/>
                    </td>
                </tr>
            </EditItemTemplate>
            <LayoutTemplate>
                <table runat="server" class="table0 narrow tbPrzypisania">
                    <tr runat="server">
                        <td runat="server" colspan="3">
                            <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr runat="server" style="">
                                    <th runat="server" rowspan="2">
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Pracownik" Text="Pracownik" ToolTip="Sortuj" /> /
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="KadryId" Text="Nr ew." ToolTip="Sortuj"/></th>
                                    <th>Przesunięcie</th>    
                                    <th runat="server" rowspan="2">
                                        <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="KierRq" Text="Wnioskujący" ToolTip="Sortuj"/> /
                                        <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="DataRq" Text="Data" ToolTip="Sortuj"/></th>
                                    <th runat="server" rowspan="2">
                                        <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="KierAcc" Text="Akceptujący" ToolTip="Sortuj"/> /
                                        <asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="DataAcc" Text="Data" ToolTip="Sortuj"/></th>
                                    <th runat="server" rowspan="2">
                                        <asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status" ToolTip="Sortuj"/></th>
                                    <th runat="server" id="thControl" class="control" rowspan="2" ></th>
                                </tr>
                                <tr>
                                    <th runat="server">
                                        <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="Kierownik1" Text="Od Kierownika" ToolTip="Sortuj" /> <span class="arrow">→</span> 
                                        <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Kierownik2" Text="Do Kierownika" ToolTip="Sortuj" /> /
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Od" Text="Od" ToolTip="Sortuj"/> -
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Do" Text="Do" ToolTip="Sortuj"/></th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%--
                    <tr class="pager">
                        <td class="left">
                            <uc1:LetterDataPager ID="LetterDataPager1" runat="server" />
                        </td>
                        <td class="right">
                            <h3>Pokaż na stronie:&nbsp;&nbsp;&nbsp;</h3>
                            <asp:DropDownList ID="ddlLines" runat="server" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    --%>
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
                        <td class="middle">
                            <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                            <asp:DropDownList ID="ddlLines" runat="server" />
                        </td>
                        <td class="right">
                            <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>

    </ContentTemplate>
</asp:UpdatePanel>

<%--
    FilterExpression="(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%')" 

    <FilterParameters>
        <asp:ControlParameter ControlID="tbSearch" Name="search" PropertyName="Text" />
    </FilterParameters>

--%>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    DeleteCommand="
declare 
    @dataOd datetime,
    @dataDo datetime,
    @prevId int
select @dataOd = Od, @dataDo = Do from Przypisania where Id = @Id
select @prevId = Id from Przypisania where IdPracownika = @IdPracownika and Status = 1 and Do = DATEADD(DAY, -1, @dataOd)
DELETE FROM [Przypisania] WHERE [Id] = @Id
if @prevId is not null and @dataDo is null begin    
    update Przypisania set Do = null where Id = @prevId
end
" 
    InsertCommand="INSERT INTO [Przypisania] ([IdPracownika], [Od], [DoMonit], [IdKierownika], RcpStrefaId, [IdCC], [IdKierownikaRq], [DataRq], [UwagiRq], [IdKierownikaAcc], [DataAcc], [UwagiAcc], [Status], [Typ]) VALUES (@IdPracownika, @Od, @Do, @IdKierownika, @RcpStrefaId, @IdCC, @IdKierownikaRq, @DataRq, @UwagiRq, @IdKierownikaAcc, @DataAcc, @UwagiAcc, @Status, @Typ)" 
    OnFiltering="SqlDataSource1_Filtering" 
    SelectCommand="
--declare @KierId int = null
--declare @Status int = 99
--declare @Data datetime = '20130510'
SELECT R.Id,
    P.Nazwisko, P.Imie,
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId,
	
	--convert(varchar(10), R.Od, 20) as Od, 
	--xISNULL(convert(varchar(10), R.Do, 20) + ' ', 'bez terminu') as Do,  --długość w ISNULL - obcina 2 parametr
	--ISNULL(convert(varchar(10), ISNULL(R.Do, R.DoMonit), 20) + ' ', 'bez terminu') as Do,  --długość w ISNULL - obcina 2 parametr
	
	R.Od, R.Do as Do, R.DoMonit, ISNULL(R.Do, R.DoMonit) as Do2, 
	R.RcpStrefaId, 
	S2.Nazwa as RcpStrefa2,
	case when R1.IdKierownika = 0 then 'Główny poziom struktury' else K1.Nazwisko + ' ' + K1.Imie end as Kierownik1, K1.KadryId as KadryId1, R1.IdKierownika as IdKierownikaOd,
	case when R.IdKierownika = 0 then 'Główny poziom struktury' else K2.Nazwisko + ' ' + K2.Imie end as Kierownik2, K2.KadryId as KadryId2, R.IdKierownika as IdKierownikaDo,
	CC1.cc + ISNULL(' - ' + CC1.Nazwa, '') as cc1,
	CC2.cc + ISNULL(' - ' + CC2.Nazwa, '') as cc2,
	C1.Commodity as Commodity1,
	C2.Commodity as Commodity2,
	A1.Area as Area1,
	A2.Area as Area2,
	PO1.Position as Position1, 
	PO2.Position as Position2,
	KRQ.Nazwisko + ' ' + KRQ.Imie as KierRq, KRQ.KadryId as KadryIdRq, R.DataRq,
	KAC.Nazwisko + ' ' + KAC.Imie as KierAcc, KAC.KadryId as KadryIdAcc, R.DataAcc,
	R.IdPracownika, R.IdKierownika, R.IdCC, R.IdCommodity, R.IdArea, R.IdPosition, R.IdKierownikaRq, R.IdKierownikaAcc, R.UwagiRq, R.UwagiAcc, R.Status, R.Typ,
	S.Status as StatusNazwa, 
	case R.Typ 
		when 0 then 'Wniosek'
		--when 1 then 'Podstruktura'
		when 1 then ''
		when 2 then 'Import'
		else ''
	end as TypNazwa,
	(select top 1 Id from Przypisania where IdPracownika = R.IdPracownika and Status = 1 and Od &gt; R.Od) as NextId 
FROM Przypisania R
left outer join Pracownicy P on P.Id = R.IdPracownika	-- PracownicyOkresy ?
left outer join Przypisania R1 on R1.Status = 1 and DATEADD(DAY, -1, R.Od) between R1.Od and ISNULL(R1.Do, '20990909') and R1.IdPracownika = R.IdPracownika

left outer join Pracownicy K1 on K1.Id = R1.IdKierownika    -- poprzednie
left outer join CC CC1 on CC1.Id = R1.IdCC
left outer join Commodity C1 on C1.Id = R1.IdCommodity
left outer join Area A1 on A1.Id = R1.IdArea
left outer join Position PO1 on PO1.Id = R1.IdPosition

left outer join Pracownicy K2 on K2.Id = R.IdKierownika     -- nowe
left outer join CC CC2 on CC2.Id = R.IdCC
left outer join Commodity C2 on C2.Id = R.IdCommodity
left outer join Area A2 on A2.Id = R.IdArea
left outer join Position PO2 on PO2.Id = R.IdPosition
left outer join Strefy S2 on S2.Id = R.RcpStrefaId

left outer join Pracownicy KRQ on KRQ.Id = ISNULL(R.IdKierownikaRqZast, R.IdKierownikaRq)

--
outer apply (
	select top 1 T.IdPracownika from (select 1 as Id) A 
	outer apply (select top 1 IdPracownika from dbo.fn_GetUpPrzypisania(R.IdKierownika, GETDATE()) where dbo.GetRight(IdPracownika, 12) != 0) T
	where R.Status = 0 
) UP
--

left outer join Pracownicy KAC on KAC.Id = 
    case when R.Status = 0 then 
        --(select top 1 IdPracownika from dbo.fn_GetUpPrzypisania(R.IdKierownika, GETDATE()) where dbo.GetRight(IdPracownika, 12) &lt;&gt; 0)
        --
        UP.IdPracownika
        --
    else 
        ISNULL(R.IdKierownikaAccZast, R.IdKierownikaAcc)
    end
left outer join PrzypisaniaStatusy S on S.Id = R.Status

where 
    (@Status = 9 and R.IdKierownikaRq = @KierId and R.Typ = 0) or    -- moje wnioski
    (@Status = 99       and (@KierId is null or                      -- all
                            (@KierId = R.IdKierownika or (@Sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@KierId, @Data))) and R.Typ = 0))) or 
    (@Status = 88       and (@KierId is null or                      -- kier all accepted
                            (@KierId = R.IdKierownika or (@Sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@KierId, @Data)))) and R.Status = 1)) or     
    (@Status = R.Status and (@KierId is null or    
                            (@KierId = R.IdKierownika or (@Sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@KierId, @Data))) and R.Typ = 0)))  
ORDER BY R.Od desc, Pracownik
    " 
    UpdateCommand="
UPDATE [Przypisania] SET RcpStrefaId = @RcpStrefaId, [IdCC] = @IdCC, [IdCommodity] = @IdCommodity, [IdArea] = @IdArea, [IdPosition] = @IdPosition, 
    Od = @Od, Do = @Do, DoMonit = @DoMonit,
    [UwagiRq] = @UwagiRq, [IdKierownikaAcc] = @IdKierownikaAcc, [DataAcc] = @DataAcc, [UwagiAcc] = @UwagiAcc, [Status] = @Status, [Typ] = @Typ 
WHERE [Id] = @Id" 
    onupdated="SqlDataSource1_Updated" oninit="SqlDataSource1_Init">

<%--
where 
--    (@Status = 9 and R.IdKierownikaRq = @KierId and R.Status = 0) or    -- moje
    (@Status = 9 and R.IdKierownikaRq = @KierId and R.Typ = 1) or    -- moje wnioski
    (@Status = 99       and (@KierId is null or                      -- all
                            (@KierId = R.IdKierownika or R.IdKierownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@KierId, @Data))) and R.IdKierownikaRq is not null)) or 
    (@Status = 88       and (@KierId = R.IdKierownika or R.IdKierownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@KierId, @Data)))) or 
    (@Status = R.Status and (@KierId is null or    
                            (@KierId = R.IdKierownika or R.IdKierownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@KierId, @Data))) and R.IdKierownikaRq is not null))  
--%>    

    <SelectParameters>
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKierId" Name="KierId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidStatus" Name="Status" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidSub" Name="Sub" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="DoMonit" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="DoMonit" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Commodity + case when Aktywne = 1 then '' else ' (nieaktywne)' end as Nazwa, 
case when Aktywne = 1 then 1 else 2 end as Sort 
from Commodity order by Sort, Nazwa
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Area + case when Aktywne = 1 then '' else ' (nieaktywny)' end as Nazwa, 
case when Aktywne = 1 then 1 else 2 end as Sort 
from Area order by Sort, Nazwa
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Position + case when Aktywne = 1 then '' else ' (nieaktywna)' end as Nazwa, 
case when Aktywne = 1 then 1 else 2 end as Sort 
from Position order by Sort, Nazwa
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Nazwa + case when Aktywna = 1 then '' else ' (nieaktywna)' end as Nazwa, 
case when Aktywna = 1 then 1 else 2 end as Sort 
from Strefy order by Sort, Nazwa
    ">
</asp:SqlDataSource>





















<%--

    UpdateCommand="UPDATE [Przypisania] SET [IdPracownika] = @IdPracownika, [Od] = @Od, [Do] = @Do, [IdKierownika] = @IdKierownika, [IdCC] = @IdCC, [IdKierownikaRq] = @IdKierownikaRq, [DataRq] = @DataRq, [UwagiRq] = @UwagiRq, [IdKierownikaAcc] = @IdKierownikaAcc, [DataAcc] = @DataAcc, [UwagiAcc] = @UwagiAcc, [Status] = @Status, [Typ] = @Typ WHERE [Id] = @Id">

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, null as cc, 0 as Sort
union    
select Id, cc + ISNULL(' - ' + Nazwa, '') as Nazwa, cc, 1 as Sort
from CC 
where Wybor = 1
order by Sort, cc
    ">
</asp:SqlDataSource>


--%>


                    
                    <%--
                    <td>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("Kierownik2") %>' /><br />
                        <asp:Label ID="Label4" runat="server" CssClass="line2" Text='<%# Eval("Kierownik1") %>' />
                    </td>

                    <td>
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("cc2") %>' /><br />
                        <asp:Label ID="Label6" runat="server" CssClass="line2" Text='<%# Eval("cc1") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Commodity2") %>' /><br />
                        <asp:Label ID="Label8" runat="server" CssClass="line2" Text='<%# Eval("Commodity1") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("Area2") %>' /><br />
                        <asp:Label ID="Label10" runat="server" CssClass="line2" Text='<%# Eval("Area1") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text='<%# Eval("Position2") %>' /><br />
                        <asp:Label ID="Label12" runat="server" CssClass="line2" Text='<%# Eval("Position1") %>' />
                    </td>

                    <td>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /><br />
                        <asp:Label ID="Label14" runat="server" CssClass="line2" Text='<%# BezTerminu(Eval("Do")) %>' />
                    </td>

                    --%>
      
                        <%--
                        <asp:Label ID="lbCC" runat="server" CssClass="value" Text='<%# Eval("cc2")%>' />
                        --%>                

                    <%--
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("Kierownik2") %>' /><br />
                        <asp:Label ID="Label4" runat="server" CssClass="line2" Text='<%# Eval("Kierownik1") %>' />

                    <td>
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("cc2") %>' /><br />
                        <asp:Label ID="Label6" runat="server" CssClass="line2" Text='<%# Eval("cc1") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Commodity2") %>' /><br />
                        <asp:Label ID="Label8" runat="server" CssClass="line2" Text='<%# Eval("Commodity1") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("Area2") %>' /><br />
                        <asp:Label ID="Label10" runat="server" CssClass="line2" Text='<%# Eval("Area1") %>' />
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text='<%# Eval("Position2") %>' /><br />
                        <asp:Label ID="Label12" runat="server" CssClass="line2" Text='<%# Eval("Position1") %>' />
                    </td>

                    <td>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("Od", "{0:d}") %>' /><br />
                        <asp:Label ID="Label14" runat="server" CssClass="line2" Text='<%# BezTerminu(Eval("Do")) %>' />
                    </td>

                    --%>
                    <%--
                        CssClass="button" 
                        CssClass="button" 
                        CssClass="button" 
                        CssClass="button" 
                    --%>
                        <%--
                        <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="0" runat="server" IdPrzypisania='<%# Eval("Id") %>' Visible="false" />
                        
                        <asp:Label ID="lbCC" runat="server" CssClass="value" Text='<%# Eval("cc2")%>' Visible="false" />
                        <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="SqlDataSource2" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("IdCC") %>'>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddlCC" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                        --%>

                                    <%--
                                    <th>Kierownik</th>    

                                    <th>CC</th>    
                                    <th>Commodity</th>    
                                    <th>Area</th>    
                                    <th>Position</th>
                                    --%>
                                    <%--

                                    <th runat="server">
                                        <asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Kierownik2" Text="Docelowy" ToolTip="Sortuj" /> /
                                        <asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="Kierownik1" Text="Poprzedni" ToolTip="Sortuj" /></th>
                                    <th runat="server">
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Od" Text="od" ToolTip="Sortuj"/> /
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Do" Text="do" ToolTip="Sortuj"/></th>


                                    <th id="Th1" runat="server">
                                        <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="cc2" Text="Docelowe" ToolTip="Sortuj" /> /
                                        <asp:LinkButton ID="LinkButton13" runat="server" CommandName="Sort" CommandArgument="cc1" Text="Obecne" ToolTip="Sortuj" /></th>
                                    <th id="Th2" runat="server">
                                        <asp:LinkButton ID="LinkButton14" runat="server" CommandName="Sort" CommandArgument="Commodity2" Text="Docelowy" ToolTip="Sortuj" /> /
                                        <asp:LinkButton ID="LinkButton15" runat="server" CommandName="Sort" CommandArgument="Commodity1" Text="Obecny" ToolTip="Sortuj" /></th>
                                    <th id="Th3" runat="server">
                                        <asp:LinkButton ID="LinkButton16" runat="server" CommandName="Sort" CommandArgument="Area2" Text="Docelowa" ToolTip="Sortuj" /> /
                                        <asp:LinkButton ID="LinkButton17" runat="server" CommandName="Sort" CommandArgument="Area1" Text="Obecna" ToolTip="Sortuj" /></th>
                                    <th id="Th4" runat="server">
                                        <asp:LinkButton ID="LinkButton18" runat="server" CommandName="Sort" CommandArgument="Position2" Text="Docelowa" ToolTip="Sortuj" /> /
                                        <asp:LinkButton ID="LinkButton19" runat="server" CommandName="Sort" CommandArgument="Position1" Text="Obecna" ToolTip="Sortuj" /></th>
                                    --%>

            <%--
            <td>
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdPracownikaTextBox" runat="server" Text='<%# Bind("IdPracownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("Od") %>' />
            </td>
            <td>
                <asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("Do") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaTextBox" runat="server" Text='<%# Bind("IdKierownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdCCTextBox" runat="server" Text='<%# Bind("IdCC") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaRqTextBox" runat="server" Text='<%# Bind("IdKierownikaRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataRqTextBox" runat="server" Text='<%# Bind("DataRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiRqTextBox" runat="server" Text='<%# Bind("UwagiRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaAccTextBox" runat="server" Text='<%# Bind("IdKierownikaAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataAccTextBox" runat="server" Text='<%# Bind("DataAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiAccTextBox" runat="server" Text='<%# Bind("UwagiAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
            --%>
            
            <%--
    <InsertItemTemplate>
        <tr style="">
            <td colspan="10">
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="IdPracownikaTextBox" runat="server" Text='<%# Bind("IdPracownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("Od") %>' />
            </td>
            <td>
                <asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("Do") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaTextBox" runat="server" Text='<%# Bind("IdKierownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdCCTextBox" runat="server" Text='<%# Bind("IdCC") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaRqTextBox" runat="server" Text='<%# Bind("IdKierownikaRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataRqTextBox" runat="server" Text='<%# Bind("DataRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiRqTextBox" runat="server" Text='<%# Bind("UwagiRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaAccTextBox" runat="server" Text='<%# Bind("IdKierownikaAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataAccTextBox" runat="server" Text='<%# Bind("DataAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiAccTextBox" runat="server" Text='<%# Bind("UwagiAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
        </tr>
    </InsertItemTemplate>
            --%>