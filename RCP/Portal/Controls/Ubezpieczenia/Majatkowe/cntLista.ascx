<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntLista.ascx.cs" Inherits="HRRcp.Portal.Controls.WnioskiMajatkowe.cntLista" %>

<asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
<asp:HiddenField ID="hidMode" runat="server" Visible="false" />

<style>
    .cntWnioskiMajatkoweLista i { margin-right: 0px !important; }
</style>

<div id="paWnioskiMajatkoweLista" runat="server" class="cntWnioskiMajatkoweLista">
    <asp:ListView ID="lvWnioski" runat="server" DataSourceID="SqlDataSource1" EnableModelValidation="True" DataKeyNames="Id"
        OnItemCommand="lvWnioski_ItemCommand" OnDataBound="lvWnioski_DataBound">
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>
                        <h3>Brak wniosków
                        </h3>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table runat="server" class="tbWnioskiMajatkoweLista">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" class="table xtable-bordered">
                            <tr id="trHeader" runat="server" style="">
                                <th runat="server">Wnioskujący</th>
                                <th runat="server">PESEL</th>
                                <th runat="server">Adres</th>
                                <th runat="server">Data rozpoczęcia ubezpieczenia</th>
                                <th runat="server">Wariant</th>
                                <th runat="server">Suma ubezpieczenia (zł)</th>
                                <th runat="server">Składka miesięczna (zł)</th>
                                <th runat="server" id="thZgoda" visible='<%# ModeVisible(0,99) %>' title="Zgoda na potrącenie składki z wynagrodzenia">Zgoda na potrącenie</th>
                                <th runat="server" id="thStatus" visible='<%# ModeVisible(-1,99) %>'>Status</th>
                                <th runat="server" id="thDataDo" visible='<%# ModeVisible(-1,99) %>'>Data zakończenia</th>
                                <th></th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style=""></td>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class='status<%# Eval("Status") %>'>
                <td>
                    <asp:Label ID="ZglaszajacyLabel" runat="server" Text='<%# Eval("Zglaszajacy") %>' />
                </td>
                <td>
                    <asp:Label ID="PESELLabel" runat="server" Text='<%# Eval("PESEL") %>' />
                </td>
                <td>
                    <asp:Label ID="AdresUlicaDomLokLabel" runat="server" Text='<%# Eval("AdresUlicaDomLok") %>' /><br />
                    <asp:Label ID="AdresKodLabel" runat="server" Text='<%# Eval("AdresKod") %>' />
                </td>
                <td class="data">
                    <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd","{0:d}") %>' />
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Wariant") %>' />
                </td>
                <td class="num">
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Suma") %>' />
                </td>
                <td class="num">
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Skladka") %>' />
                </td>
                <td id="tdZgoda" runat="server" visible='<%# ModeVisible(0,99) %>' class="zgoda">
                    <span id="icoZgoda" runat="server" class="glyphicon glyphicon-ok" visible='<%# Eval("ZgodaNaPotracenie") %>'></span>
                </td>
                <td id="tdStatus" runat="server" visible='<%# ModeVisible(-1,99) %>' class="status">
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("StatusNazwa") %>' />
                </td>
                <td id="tdDataDo" runat="server" visible='<%# ModeVisible(-1,99) %>' class="data">
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("DataZakonczenia","{0:d}") %>' />
                </td>
                <td class="control">
                    <asp:LinkButton ID="lbtPokaz" runat="server" ToolTip="Pokaż szczegóły" CommandName="show" CssClass='<%# GetButtonClass() %>'><i class='<%# GetButtonIcon() %>'></i></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
--declare @ZglaszajacyId int = 1
--declare @Status varchar(max) = '0'
select 
  w.Id 
, P.Nazwisko + ' ' + P.Imie Zglaszajacy
, W.PESEL
--, W.AdresUlicaDomLok
--, W.AdresKod 
, W.AdresUbezpUlica + ISNULL(' ' + W.AdresUbezpDom,'') + ISNULL('/' + W.AdresUbezpLokal,'') AdresUlicaDomLok
, W.AdresUbezpKod + ' ' + W.AdresUbezpMiasto AdresKod
, DataOd
, DataZakonczenia
, case when /*W.Plus = 1*/ W.PlusId is not null then /*WP.SumaPlus*/oa.SumaPlus else WP.Suma end Suma
, case when /*W.Plus = 1*/ W.PlusId is not null then 'Plus' else 'Podstawowy' end Wariant
, case when /*W.Plus = 1*/ W.PlusId is not null then WP.Skladka + /*WP.SkladkaPlus*/oa.SkladkaPlus else WP.Skladka end Skladka
, W.Status
, S.StatusNazwa
, W.ZgodaNaPotracenie 
from poWnioskiMajatkowe W
    inner join
    (
    select MAX(Id) Id, FromId from poWnioskiMajatkowe group by FromId
    ) a on a.FromId = w.FromId and a.Id = w.Id
outer apply (select case when W.Status = -1 then 2 else 1 end Sort) W1
left join poWnioskiMajatkoweStatusy S on S.Id = W.Status
left join Pracownicy P on P.Id = W.ZglaszajacyId
left join poWnioskiMajatkoweParametry WP on WP.Id = W.ParId
outer apply (select * from poWnioskiMajatkoweParametry where Id = W.PlusId) oa
WHERE ([ZglaszajacyId] = @ZglaszajacyId) 
--and (W.Status in (select items from dbo.SplitInt(@Status, ','))) 
and (
    (@mode in (0,11,12) and W.Status = 0)
 or (@mode = -1 and W.Status = -1)
 or (@mode = 99)    
)
ORDER BY W1.Sort, W.DataOd desc
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="ZglaszajacyId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidMode" Name="Mode" PropertyName="Value" Type="String" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>
