<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWniosekUrlopowy.ascx.cs" Inherits="HRRcp.Controls.Portal.cntWniosekUrlopowy" %>
<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Portal" TagPrefix="cc1" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="../DateRange.ascx" tagname="DateRange" tagprefix="uc2" %>
<%@ Register src="dbField.ascx" tagname="dbField" tagprefix="uc3" %>
<%@ Register src="../zoomUrlopy.ascx" tagname="zoomUrlopy" tagprefix="uc1" %>

<%--
TypVisible:
    public const int wtUW = 1;  // Urlop wypoczynkowy
    public const int wtUZ = 2;  // Urlop na żądanie
    public const int wtUO = 3;  // Urlop okolicznościowy 
    public const int wtO2 = 4;  // Opieka nad zdrowym dzieckiem do lat 14 (art. 188 KP), od 20160502 też na godziny
    public const int wtSZ = 5;  // Urlop szkoleniowy
    public const int wtOD = 6;  // Czas wolny za pracę w godzinach nadliczbowych (151(2) par.1) KP
    public const int wtUB = 7;  // Urlop bezpłatny
    public const int wtNU = 8;  // Nieobecność usprawiedliwiona
    public const int wtNN = 9;  // Nieobecność nieusprawiedliwiona
    public const int wtNN = 10; // odbiór dnia wolnego za święto
    public const int wnUD = 11; // urlop dodatkowy
StVisible:
    pracownik,kierownik,administrator ---> wnioskujacy,akceptujacy,admin
    0 - niewidoczne
    1 - widoczne
    2 - widoczny, do edycji
    3 - widoczny, do edycji po wejściu w tryb edycji danych
  do dodania:  
    4 - niewidoczny, widoczny po wejściu w tryb edycji
    5 - niewidoczny, do edycji po wejściu w tryb edycji 
Status (01234):
    0 -	Nowy wniosek
    1 -	Czeka na akceptację
    2 -	Odrzucony
    3 -	Zaakceptowany
    4 -	Zaakceptowany - wprowadzony
Kto (00000,00000,00000): Prac, Kier, Admin   
--%>

<div id="paWniosekU" runat="server" class="cntWniosekUrlopowy border wniosek">
    <div id="paWniosekU2" runat="server">
        <table class="tbWniosek table0">
            <tr class="data">
                <td>
                    <uc3:dbField ID="Status" CssClass="left" runat="server" />
                    Bydgoszcz, 
                    <uc3:dbField ID="DataWniosku" CssClass="dbfInline" Type="date" runat="server" />
                </td>
            </tr>

            <tr class="pracownik">
                <td>
                    <uc3:dbField runat="server" ID="IdPracownika" ValueField="Pracownik" Label="Imię i Nazwisko:" StVisible="11111,21111,21111" 
                        Type="ddl" Rq="true" DataSourceID="SqlDataSource4" OnChanged="IdPracownika_Changed" />
                    <uc3:dbField runat="server" ID="PracLogo" Label="Numer ewidencyjny:" />
                    <uc3:dbField runat="server" ID="Email" Label="e-mail:" />

                    <%--
                    <uc3:dbField runat="server" ID="ProjektDzial" Label="Projekt/Dział:" />
                    --%>
                    <uc3:dbField runat="server" ID="Dzial" Label="Projekt/Dział:" />

                    <uc3:dbField runat="server" ID="Stanowisko" Label="Stanowisko:" />
                    <uc3:dbField runat="server" ID="Kierownik" Label="Przełożony:" />
                    <div id="div1" runat="server" visible="false"><br /></div>
                    <uc3:dbField runat="server" ID="Autor" Label="Wnioskujący:" />
                    <uc3:dbField runat="server" ID="KierAcc" Label="Akceptujący:" StVisible="11000,11000,11000"/>
                </td>
            </tr>

            <tr class="title">
                <td>
                    <span class="label1">Wniosek o urlop<span class="editinfo"> - edycja danych</span></span>
                </td>
            </tr>
            
            <tr class="dane">
                <td>
                                                                                                         <%-- StVisible="11111,11111,13333" --%>
                    <uc3:dbField runat="server" ID="TypId" ValueField="TypNapis" Label="Proszę o udzielenie:" StVisible="11111,11111,11111" 
                        Type="ddl" Rq="true" DataSourceID="SqlDataSource2" />

                    <uc3:dbField ID="Info" runat="server" Label="<br />Godziny nadliczbowe w dniach (200 znaków):" StVisible="21111,21111,21111" TypVisible="6"
                        Type="tb" Rq="true" TextMode="MultiLine" Rows="3" MaxLength="200" CssClass="dbfLabel1" />
                    <br />
                    
                    <asp:HiddenField ID="hidMnoznik" runat="server" />
                    <asp:HiddenField ID="hidGodz" runat="server" />
                    <%-- urlop okolicznościowy --%>
                    <cc1:WnVisible ID="WnVisible3" runat="server" StControls="paOkolDni" StVisible="21111,21111,21111" TypVisible="3" />
                    <div id="paOkolDni" runat="server" class="okolDni" visible="false">
                        <span class="title">Wybierz powód i ilość dni<asp:Label ID="lbStar1" CssClass="star" runat="server" Text="*" Visible="false"></asp:Label>:</span>
                        <asp:RadioButtonList ID="rblOkolDni" runat="server" AutoPostBack="True" CssClass="rblIloscDni" 
                            onselectedindexchanged="rblOkolDni_SelectedIndexChanged" 
                            DataSourceID="SqlDataSourceOkol" DataTextField="Nazwa" DataValueField="Kod" >
                        </asp:RadioButtonList>
                        <asp:SqlDataSource ID="SqlDataSourceOkol" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                            SelectCommand="SELECT convert(varchar, Kod) + '|' + Parametr as Kod, Nazwa FROM [Kody] WHERE Typ = 'URLOP_OKOL' ORDER BY [Lp]">
                        </asp:SqlDataSource>
                    </div>

<%--
                    <uc3:dbField ID="Od" runat="server" Label="Od dnia:" StVisible="21111,21111,23333" 
                        Type="date" Rq="true" CssClass="dbfLabelR dbfInline" />
                    &nbsp;&nbsp;&nbsp;
                    <uc3:dbField ID="Do" runat="server" Label="Do dnia:" StVisible="21111,21111,23333" 
                        Type="date" Rq="true" CssClass="dbfLabel0 dbfInline" />
                    <br />
                    <br />

                    <uc3:dbField ID="Dni" runat="server" Label="Ilość dni roboczych:" StVisible="21111,21111,23333" 
                        Type="tb" Rq="true" Min="1" Max="366" MaxLength="3" ValidChars="0123456789" CssClass="dbfNum dbfLabelR dbfInline" />
                    &nbsp;&nbsp;&nbsp;
                    <uc3:dbField ID="Godzin" runat="server" Label="Ilość godzin:" StVisible="11111,11111,11111" 
                        Type="tb" MaxLength="5" CssClass="dbfNum dbfLabel0 dbfInline" />
                    <br />
                    <br />
--%>

                    <cc1:WnVisible ID="WnVisible4" runat="server" StControls="Div2" TypVisible="1,2,3,4,5,6,7,8,9,11" />
                    <div id="Div2" runat="server" class="oddo" visible="false">
                        <uc3:dbField ID="Od" runat="server" Label="Od dnia:" StVisible="21111,21111,23333" 
                            Type="date" Rq="true" CssClass="dbfLabelR dbfInline" />
                        <uc3:dbField ID="Do" runat="server" Label="Do dnia:" StVisible="21111,21111,23333" 
                            Type="date" Rq="true" CssClass="dbfLabel0 dbfInline" />
                        
                        
                        <%-- opieka art 188 na godziny --%>
                        <cc1:WnVisible ID="WnVisible8" runat="server" StControls="paOpieka" StVisible="21111,21111,23333" TypVisible="4" />
                        <div id="paOpieka" runat="server" class="okolDni dbfLabelR dbField" visible="false">
                            <span class="label">W ilości dni/godzin<asp:Label ID="lbStar2" CssClass="star" runat="server" Text="*" Visible="false"></asp:Label>:</span>  
                            <asp:Label ID="lbOpiekaDni" CssClass="value" runat="server" Visible="false"></asp:Label>                      
                            <asp:DropDownList ID="ddlOpiekaDni" runat="server" AutoPostBack="True" CssClass="ddlOpiekaDni" OnSelectedIndexChanged="ddlOpiekaDni_SelectedIndexChanged" >
                                <asp:ListItem Selected="True" Value="1">1 dzień</asp:ListItem>
                                <asp:ListItem Value="2">2 dni</asp:ListItem>
                                <asp:ListItem Value="1h1">1 godzina</asp:ListItem>
                                <asp:ListItem Value="1h2">2 godziny</asp:ListItem>
                                <asp:ListItem Value="1h3">3 godziny</asp:ListItem>
                                <asp:ListItem Value="1h4">4 godziny</asp:ListItem>
                                <asp:ListItem Value="1h5">5 godzin</asp:ListItem>
                                <asp:ListItem Value="1h6">6 godzin</asp:ListItem>
                                <asp:ListItem Value="1h7">7 godzin</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                        </div>
                        
                        
                        <div id="paDniGodzin" runat="server" visible="true">
                            <uc3:dbField ID="Dni" runat="server" Label="Ilość dni roboczych:" StVisible="21111,21111,23333" 
                                Type="tb" Rq="true" Min="1" Max="366" MaxLength="3" ValidChars="0123456789" CssClass="dbfNum dbfLabelR dbfInline" />
                            <uc3:dbField ID="Godzin" runat="server" Label="Ilość godzin:" StVisible="11111,11111,11111" TypVisible="1,2,3,4,5,6,7,8,9,11" 
                                Type="tb" MaxLength="5" CssClass="dbfNum dbfLabel0 dbfInline" />
                        </div>
                    </div>
                    
                    <cc1:WnVisible ID="WnVisible5" runat="server" StControls="Div3" TypVisible="10" />
                    <div id="Div3" runat="server" class="oddo" visible="false">
                        <uc3:dbField ID="Odbior" Fields="Od" runat="server" Label="odbiór w dniu:" StVisible="21111,21111,23333" 
                            Type="date" Rq="true" CssClass="dbfLabelR dbfInline" />
                        <br />
                        <br />
                        <uc3:dbField ID="Swieto" Fields="Info" runat="server" Label="za święto w dniu:" StVisible="21111,21111,23333" 
                            Type="date" Rq="true" CssClass="dbfLabelR dbfInline" />
                        <br />
                        <br />
                    </div>


                    <uc3:dbField ID="UzasadnieniePrac" runat="server" Label="Uzasadnienie wniosku (200 znaków):" StVisible="21111,21111,21111" TypVisible="2"
                        Type="tb" Rq="false" TextMode="MultiLine" Rows="3" MaxLength="200" CssClass="dbfLabel1" />
                    <uc3:dbField ID="aaUzasadnieniePrac" runat="server" Label="Uzasadnienie wniosku (200 znaków):" StVisible="21111,21111,21111" TypVisible="7,8"
                        Type="tb" Rq="true" TextMode="MultiLine" Rows="3" MaxLength="200" CssClass="dbfLabel1" />

                    <uc3:dbField ID="DbField3" Fields="IdZastepuje,Zastepuje" Label="Zastępstwo pełnić będzie:" StVisible="21111,22111,23333" 
                        Type="ddl" DataSourceID="SqlDataSource1" CssClass="dbfLabel0" runat="server" 
                        Visible="false"/>
<%--                
                    <uc3:dbField ID="Zastepuje" Label="" StVisible="21111,22111,22333" 
                        Type="tb" MaxLength="100" CssClass="dbfLabel0" runat="server" />
--%>
                    <uc3:dbField ID="Zastepuje" Label="Zastępstwo pełnić będzie:" StVisible="21111,22111,23333" 
                        Type="tb" MaxLength="100" CssClass="dbfLabel0" runat="server" 
                        Visible="true"/>

                    <uc3:dbField ID="DbField2" Fields="IdZastepuje,Zastepuje2" Label="" StVisible="21111,22111,23333" 
                        Type="ddl" DataSourceID="SqlDataSource1" CssClass="dbfLabel0" runat="server" 
                        Visible="false"/>
                </td>
            </tr>

            <cc1:WnVisible ID="WnVisible1" runat="server" StControls="trAcc" StVisible="00111,01111,01111" />
            <tr id="trAcc" class="akceptacja" runat="server" >
                <td>
                    <uc3:dbField ID="UzasadnienieKier" runat="server" Label="Uzasadnienie odrzucenia wniosku (200 znaków):" StVisible="00100,02100,03133" 
                        Type="tb" TextMode="MultiLine" Rows="3" MaxLength="200" CssClass="dbfLabel1" ValidationGroup="vgRej"/>
                        <%--
                    <uc3:dbField ID="KierAcc" CssClass="dbfLabel0 dbfInline" runat="server" Label="Akceptacja przełożonego:" StVisible="01111,00111,11111" />
                    <uc3:dbField ID="DataKierAcc" CssClass="dbfInline" Type="dt" runat="server" StVisible="01111,01111,11111" />
                        --%>

                    <uc3:dbField ID="DbField1" CssClass="dbfLabel0 dbfInline" runat="server" Label="Wniosek zaakceptował:" StVisible="01011,00011,00011" 
                        Fields="KierAccZast,DataKierAcc" Format="{0}<b>, {1}</b>" />
                    <uc3:dbField ID="DbField4" CssClass="dbfLabel0 dbfInline" runat="server" Label="Wniosek odrzucił:" StVisible="00100,00100,00100" 
                        Fields="KierAccZast,DataKierAcc" Format="{0}<b>, {1}</b>" />
                </td>
            </tr>

            <cc1:WnVisible ID="WnVisible2" runat="server" StControls="trUwagiAdm" StVisible="00000,00000,01111" />
            <tr id="trUwagiAdm" class="uwagiadm" runat="server" >
                <td>
                    <uc3:dbField ID="UwagiKadry" runat="server" Label="Uwagi działu HR (200 znaków):" StVisible="00000,00000,03333" 
                        Type="tb" TextMode="MultiLine" Rows="3" MaxLength="200" CssClass="dbfLabel1" />
                </td>
            </tr>
            
            <tr class="buttons">
                <td>                                        <%-- zmienić !!!: 0 - niewidoczny, 1 - disabled, 2 - można kliknąć --%>
                    <cc1:WnButton ID="btAccept" CssClass="button100 left"        runat="server" Text="Akceptuj" onclick="btAccept_Click" StVisible="00000,01000,03000" />
                    <cc1:WnButton ID="btReject" CssClass="button100 left rspace" runat="server" Text="Odrzuć"   onclick="btReject_Click" StVisible="00000,01000,03033" ConfirmMsg="Potwierdź odrzucenie wniosku." ValidationGroup="vgRej" />
                                                            <%--
                                                            00000,01000,01011
                                                            --%>
                    <cc1:WnButton ID="btAbsencje" CssClass="button100 left rspace" runat="server" Text="Pokaż urlopy" onclick="btAbsencje_Click" StVisible="11111,11111,11111" />

                    <div style="display: inline-block; white-space: nowrap;" class="left">
                        <cc1:WnButton ID="btCofnij" CssClass="button100 left" runat="server" Text="Cofnij status" onclick="btCofnij_Click" StVisible="00000,00000,00333" ConfirmMsg="Potwierdź cofnięcie wniosku do poprzedniego statusu." />
                        <cc1:WnButton ID="btEdit" CssClass="button100 left" runat="server" Text="Edytuj" onclick="btEdit_Click" StVisible="00000,00000,02222" />
                        <cc1:WnButton ID="btDelete" CssClass="button100 left" runat="server" Text="Usuń" onclick="btDelete_Click" StVisible="01000,01000,02222" ConfirmMsg="Potwierdź usunięcie wniosku."/>
                        <cc1:WnButton ID="btCancelWniosek" CssClass="button100 left" runat="server" Text="Anuluj wniosek" onclick="btCancelWniosek_Click" StVisible="00000,00000,00000" ConfirmMsg="Potwierdź anulowanie wniosku."/>
                    </div>

                    <cc1:WnButton ID="btSave" CssClass="button100" runat="server" Text="Zapisz" onclick="btSave_Click" StVisible="00000,00000,03333" />
                    <cc1:WnButton ID="btCancelEdit" CssClass="button100" runat="server" Text="Anuluj" onclick="btCancelEdit_Click" StVisible="00000,00000,03333" />

                    <cc1:WnButton ID="btSend" CssClass="button100" runat="server" Text="Wyślij" onclick="btSend_Click" StVisible="10000,10000,10000" />
                    <cc1:WnButton ID="btAcceptSend" CssClass="button100"   runat="server" Text="Zaakceptuj" onclick="btAccept_Click" StVisible="00000,10000,10000" />
                    <cc1:WnButton ID="btClose" CssClass="button100" runat="server" Text="Zamknij" onclick="btClose_Click" StVisible="02222,02222,02222" />    
                    <cc1:WnButton ID="btCancel" CssClass="button100" runat="server" Text="Anuluj" onclick="btCancel_Click" StVisible="10000,10000,10000" />
                    
                    <cc1:WnButton ID="btPrint" CssClass="button100" runat="server" Text="Drukuj" OnClientClick="javascript:window.print();" StVisible="01111,01111,01111" />

                    <asp:Button ID="btSend2" runat="server" CssClass="button_postback" Text="Wyślij" OnClick="btSend2_Click" />
                    <asp:Button ID="btAccept2" runat="server" CssClass="button_postback" Text="Akceptuj" OnClick="btAccept2_Click" />

                    <%--
                    <asp:Button ID="btSave" CssClass="button100 left" runat="server" Text="Zapisz" onclick="btSave_Click" Visible="false" />
                    <asp:Button ID="btCancelEdit" CssClass="button100 left" runat="server" Text="Anuluj" onclick="btCancelEdit_Click" Visible="false" />

                    ConfirmMsg="Potwierdź akceptację wniosku." 
                    <cc1:WnButton ID="btSave" CssClass="button100 left" runat="server" Text="Zapisz" onclick="btSave_Click" StVisible="00000,00000,01111" Visible="false" />
                    <cc1:WnButton ID="btCancelEdit" CssClass="button100 left" runat="server" Text="Anuluj" onclick="btCancelEdit_Click" StVisible="00000,00000,01111" Visible="false" />
                    --%>
                </td>
            </tr>
        </table>
    </div>
    <uc1:zoomUrlopy ID="zoomUrlopy" runat="server" />
</div>



                    <%--
                            <asp:ListItem Value="1|2">2 dni - Twój ślub</asp:ListItem>
                            <asp:ListItem Value="2|2">2 dni - narodziny dziecka</asp:ListItem>
                            <asp:ListItem Value="3|2">2 dni - zgon i pogrzeb małżonka, dziecka, ojca, matki, ojczyma lub macochy</asp:ListItem>
                            <asp:ListItem Value="4|1">1 dzień - ślub Twojego dziecka</asp:ListItem>
                            <asp:ListItem Value="5|1">1 dzień - zgon i pogrzeb siostry, brata, teściowej, teścia, babki, dziadka, a także innej osoby, którą utrzymujesz lub którą się bezpośrednio opiekujesz</asp:ListItem>
                    <div id="paOkolDni_1" runat="server" class="okolDni" visible="false">
                        <span>Wybierz przysługującą ilość dni:</span>
                        <asp:RadioButtonList ID="rblOkolDni_1" runat="server" AutoPostBack="True" CssClass="rblIloscDni" 
                            onselectedindexchanged="rblOkolDni_SelectedIndexChanged" >
                            <asp:ListItem Value="2">
2 dni przysługują w związku z:
<ul>
    <li>Twoim ślubem,</li>
    <li>narodzinami dziecka,</li>
    <li>zgonem i pogrzebem małżonka, dziecka, ojca, matki, ojczyma lub macochy.</li>
</ul>
                            </asp:ListItem>
                            <asp:ListItem Value="1">
1 dzień przysługuje w razie:
<ul>
    <li>ślubu Twojego dziecka,</li>
    <li>zgonu i pogrzebu siostry, brata, teściowej, teścia, babki, dziadka, a także innej osoby, którą utrzymujesz lub którą się bezpośrednio opiekujesz.</li>
</ul>
                            </asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    --%>

<%--
                    <div id="paOkolDni_2" runat="server" class="okolDni" visible="false">
                        <span>Wybierz powód:</span>                        
                        <asp:DropDownList ID="ddlPowod" runat="server">
                        </asp:DropDownList>
                    </div>

2 dni przysługują w związku z:<br />
- Twoim ślubem,<br />
- narodzinami dziecka,<br />
- zgonem i pogrzebem małżonka, dziecka, ojca, matki, ojczyma lub macochy.                            
--%>





<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as TypId, 'wybierz ...' as TypNapis, -1000 as Kolejnosc
union
select Id as TypId, TypNapis, Kolejnosc
from poWnioskiUrlopoweTypy
where Aktywny = 1
order by Kolejnosc">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource1_x" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    CancelSelectOnNullParameter="false"
    SelectCommand="
select null as IdZastepuje, 'wybierz ...' as Zastepuje, 0 as Sort
union
select Id as IdZastepuje, Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')', '') as Zastepuje, 1 as Sort
from Pracownicy 
where Status &gt;= 0 and Id &lt;&gt; ISNULL(@pracId, -1)
order by Sort, Zastepuje">
    <SelectParameters>
        <asp:Parameter Name="pracId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    CancelSelectOnNullParameter="false"
    SelectCommand="
declare @data datetime
set @data = GETDATE()

select null as IdZastepuje, 'wybierz ...' as Zastepuje2, 0 as Sort, null as SortPath
union
select Id as IdZastepuje, Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Zastepuje2, 1 as Sort, null as SortPath from VPrzypisaniaNaDzis
where KierId = (select IdKierownika from Przypisania where IdPracownika = @pracId and @data between Od and ISNULL(Do, '20990909') and Status = 1) 
and Id != @pracId
union all
select top 1 -1, '----- moi pracownicy -----', 2 as Sort, null as SortPath from VPrzypisaniaNaDzis where KierId = @pracId
union all
select IdPracownika, 
--REPLICATE('&nbsp;', (Hlevel-1) * 4) + 
REPLICATE(' ', (Hlevel-1) * 4) + 
Nazwisko + ' ' + Imie + ' (' + KadryId + ')', 3 as Sort, SortPath from dbo.fn_GetTree(@pracId, GETDATE()) where Hlevel &lt; 3
order by Sort, SortPath, 2
    ">
    <SelectParameters>
        <asp:Parameter Name="pracId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>









<asp:SqlDataSource ID="SqlDataSource4xxx" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as IdPracownika, 'wybierz ...' as Pracownik, 0 as SortPath
union
select -99, 'Pokaż wszystkich' as Pracownik, 1 as SortPath
union
select IdPracownika,
--SPACE((Hlevel-1)*4) + 
replicate('&nbsp;', (Hlevel - 1) * 4) +
--replicate('&nbsp;|', Hlevel - 1) + '-' +     
Nazwisko + ' ' + Imie + ' (' + ISNULL(KadryId, '???') + ')' as Pracownik, SortPath
from dbo.fn_GetTree(@rootId, GETDATE())    
order by SortPath
    ">
    <SelectParameters>
        <asp:Parameter Name="rootId" Type="Int32" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
--declare @rootId int = 
--declare @all int = 0 
select null as IdPracownika, 'wybierz ...' as Pracownik, 1 as Sort, null SortPath

union all
select 
    case when @all = 1 then -88 else -99 end, 
    --case when @all = 1 then '- Pokaż moich pracowników' else '- Pokaż wszystkich pracowników' end as Pracownik, 1 as SortPath
    case when @all = 1 then '- Pokaż bezpośrednich pracowników' else '- Pokaż wszystkich pracowników' end as Pracownik, 2 as Sort, null as SortPath
from dbo.GetDates2('20000101', '20000101')
where @all in (0,1)

union all
select IdPracownika,
--SPACE((Hlevel-1)*4) + 
replicate('&nbsp;', (Hlevel - 1) * 4) +
--replicate('&nbsp;|', Hlevel - 1) + '-' +     
Nazwisko + ' ' + Imie + ' (' + ISNULL(KadryId, '???') + ')' as Pracownik, 3 as Sort, SortPath
from dbo.fn_GetTree(@rootId, GETDATE())    
where @all = 1
--from dbo.fn_GetTree(case when @all=2 then 0 else @rootId end, GETDATE())    
--where @all in (0, 2)

union all

select Id as IdPracownika,
Nazwisko + ' ' + Imie + ' (' + ISNULL(KadryId, '???') + ')' as Pracownik, 4 as Sort, null as SortPath
from VPrzypisaniaNaDzis where Status &gt;= 0 and (KierId = @rootId or @all=2) 
and @all in (0,2)
/*
select Id as IdPracownika,
Nazwisko + ' ' + Imie + ' (' + ISNULL(KadryId, '???') + ')' as Pracownik, 2 as SortPath
from Pracownicy where Status &gt;= 0
and @all = 1
*/
order by Sort, SortPath, Pracownik
    ">
    <SelectParameters>
        <asp:Parameter Name="rootId" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="all" Type="Int32" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>




<asp:SqlDataSource ID="dsGetLimit" runat="server" 
    SelectCommand="
select 
  sum(A.LimitDniRazemZGodzin) LimitDni   --2 umowy w tym samym czasie
, sum(A.LimitGodzinRazem) LimitGodz
from Pracownicy P
inner join UrlopZbiorAsseco A on A.LpLogo = P.KadryId and A.UrlopTyp = '{1}'
where P.Id = {0}
    ">
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsWniosek" runat="server" 
    InsertCommand="
declare @pracId int
declare @autorId int
declare @status int
declare @wtyp int
declare @data varchar(20)
set @pracId = '{0}'
set @status = {1}
set @wtyp = {2}
set @data = {3}
set @autorId = {4}
select @wtyp as TypId, T.Typ, T.TypNapis, @status as StatusId, ST.Status
    ,@pracId as IdPracownika, ISNULL(8 * P.EtatL / P.EtatM, 8) as GodzinZm
    ,P.KadryId as PracLogo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Email
    ,K.Id as KierId
    ,K.KadryId as KierLogo, K.Nazwisko + ' ' + K.Imie as Kierownik
    ,D.Nazwa as ProjektDzial
    ,S.Nazwa as Stanowisko
    ,@data as DataWniosku
    ,@autorId as AutorId
    ,AT.Nazwisko + ' ' + AT.Imie as Autor
    ,KA.Id as KierAccId
    ,KA.Nazwisko + ' ' + KA.Imie as KierAcc
    ,null as PodTyp
    ,null as Info
    ,null as UzasadnieniePrac
from poWnioskiUrlopoweTypy T
left join poWnioskiUrlopoweStatusy ST on ST.Id = @status
left join Pracownicy P on P.Id = @pracId
left join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1 
left join Pracownicy K on K.Id = R.IdKierownika
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= @data order by Od desc) PS
left join Dzialy D on D.Id = PS.IdDzialu
left join Stanowiska S on S.Id = PS.IdStanowiska
left join Pracownicy AT on AT.Id = @autorId
outer apply (select * from dbo.fn_GetUpKierWithRight(@pracId, GETDATE(), 13, 1)) KA
where T.Id = @wtyp
    "
    UpdateCommand="
declare @wnid int
set @wnid = {0}
select T.Typ, T.TypNapis, ST.Status
    ,W.IdPracownika, ISNULL(8 * P.EtatL / P.EtatM, 8) as GodzinZm
    ,P.KadryId as PracLogo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Email
    ,K.Id as KierId
    ,K.KadryId as KierLogo, K.Nazwisko + ' ' + K.Imie as Kierownik
    ,D.Nazwa as Dzial
    ,S.Nazwa as Stanowisko
    ,Z.KadryId as ZastLogo 
    ,W.IdZastepuje, ISNULL(Zastepuje, Z.Nazwisko + ' ' + Z.Imie + ISNULL(' (' + Z.KadryId + ')', '')) as Zastepuje
    ,KA.Id as KierAccId
    ,KA.Nazwisko + ' ' + KA.Imie as KierAcc
    ,KZ.Nazwisko + ' ' + KZ.Imie as KierAccZast
    ,A.Nazwisko + ' ' + A.Imie as KadryAcc
    ,AT.Nazwisko + ' ' + AT.Imie as Autor
    ,W.*
from poWnioskiUrlopowe W
left join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left join Pracownicy P on P.Id = W.IdPracownika
left join Przypisania R on R.IdPracownika = P.Id and W.Od between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1 
left join Pracownicy K on K.Id = R.IdKierownika
left join Pracownicy Z on Z.Id = W.IdZastepuje
left join Pracownicy KA on KA.Id = ISNULL(W.IdKierAcc, (select Id from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)))
left join Pracownicy KZ on KZ.Id = W.IdKierAccZast
left join Pracownicy A on A.Id = W.DataKadryAcc
left join Pracownicy AT on AT.Id = W.AutorId
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= W.Od order by Od desc) PS
left join Dzialy D on D.Id = PS.IdDzialu
left join Stanowiska S on S.Id = PS.IdStanowiska
where W.Id = @wnid
    ">
</asp:SqlDataSource>



<%--
WITH SubTree AS 
(
SELECT P.Id,
P.IdPracownika,
P.IdKierownika,
1 AS HLevel,
CAST(CAST(P.IdPracownika AS BINARY(4)) AS VARBINARY(8000)) AS SortPath
FROM Przypisania P
WHERE IdKierownika = @rootId and P.Status = 1 and @data between P.Od and ISNULL(P.Do, '20990909')
UNION ALL 
SELECT R.Id,
R.IdPracownika,
R.IdKierownika,
ST.HLevel + 1 AS HLevel,
CAST(ST.SortPath + CAST(R.IdPracownika AS BINARY(4)) AS VARBINARY(8000)) AS SortPath
FROM Przypisania R
INNER JOIN SubTree ST ON ST.IdPracownika = R.IdKierownika
where R.Status = 1 and @data between R.Od and ISNULL(R.Do, '20990909')
)
SELECT A.IdPracownika, 
--A.Id, A.IdKierownika, 
Pracownik = 
--SPACE((Hlevel-1)*4) + 
replicate('&nbsp;', (Hlevel - 1) * 4) +
--replicate('&nbsp;|', Hlevel - 1) + '-' +     
B.Nazwisko + ' ' + B.Imie + ' (' + B.KadryId + ')' ,SortPath
FROM SubTree A
left outer join Pracownicy B on B.Id = A.IdPracownika
union
select null as IdPracownika, 'wybierz ...' as Pracownik, null as SortPath
ORDER BY A.SortPath--%>

                    <%--
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                        Enabled="false"
                        ValidationGroup="vgAccept" 
                        ErrorMessage="Błąd" >
                    </asp:RequiredFieldValidator>
                    --%>
<%--
                <span class="label">Imię i Nazwisko:</span>
                <asp:Label ID="lbIdPracownika" CssClass="value" runat="server" ></asp:Label>
                <asp:DropDownList ID="edIdPracownika" runat="server" DataSourceID="SqlDataSource4" 
                    DataTextField="Pracownik" DataValueField="IdPracownika">
                </asp:DropDownList>

                <span class="label">Numer ewidencyjny:</span>
                <asp:Label ID="lbPracLogo" CssClass="value" runat="server" ></asp:Label><br />
                
                <span class="label">Projekt/Dział:</span>
                <asp:Label ID="lbProjektDzial" CssClass="value" runat="server" ></asp:Label><br />

                <span class="label">Stanowisko:</span>
                <asp:Label ID="lbStanowisko" CssClass="value" runat="server" ></asp:Label><br />

                <span class="label">Przełożony:</span>
                <asp:Label ID="lbKierownik" CssClass="value" runat="server" ></asp:Label><br />

                <span class="label">Proszę o udzielenie:</span>
                <asp:Label ID="lbTypNapis" CssClass="value" runat="server" ></asp:Label><br />    
                <asp:DropDownList ID="edTypId" runat="server" 
                    DataSourceID="SqlDataSource2" DataTextField="TypNapis" DataValueField="Id">
                </asp:DropDownList>
                
                <div id="paInfo" runat="server">
                    <span class="label1">Godziny nadliczbowe w dniach (200 znaków):</span>
                    <asp:Label ID="lbInfo" CssClass="textbox value" runat="server" ></asp:Label>    
                    <asp:TextBox ID="edInfo" CssClass="textbox" MaxLength="200" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                </div>


                <span class="labelR">Od dnia:</span>
                <asp:Label ID="lbOd" CssClass="value" runat="server" ></asp:Label>    
                <uc1:DateEdit ID="edOd" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <span class="label0">Do dnia:</span>
                <asp:Label ID="lbDo" CssClass="value" runat="server" ></asp:Label>    
                <uc1:DateEdit ID="edDo" runat="server" />

                <span class="labelR">Ilość dni:</span>
                <asp:Label ID="lbDni" CssClass="value" runat="server" ></asp:Label>    
                <asp:TextBox ID="edDni" CssClass="textbox num" MaxLength="5" runat="server"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;
                <span class="label0">Ilość godzin:</span>
                <asp:Label ID="lbGodzin" CssClass="value" runat="server" ></asp:Label>    
                <asp:TextBox ID="edGodzin" CssClass="textbox num" MaxLength="5" runat="server"></asp:TextBox>

                <div id="paUzasadnieniePrac" runat="server">
                    <span class="label1">Uzasadnienie wniosku (200 znaków):</span>
                    <asp:Label ID="lbUzasadnieniePrac" CssClass="textbox value" runat="server" ></asp:Label>    
                    <asp:TextBox ID="edUzasadnieniePrac" CssClass="textbox" MaxLength="200" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                </div>

                <span class="label0">Zastępstwo pełnić będzie Pan/Pani:</span>
                <asp:Label ID="lbIdZastepuje" CssClass="value" runat="server" ></asp:Label>    
                <asp:TextBox ID="edZastepuje" CssClass="textbox" MaxLength="100" runat="server"></asp:TextBox>
                <asp:DropDownList ID="edIdZastepuje" runat="server" 
                    DataSourceID="SqlDataSource1" DataTextField="Pracownik" DataValueField="Id">
                </asp:DropDownList>

                <div id="paUzasadnienie" runat="server">
                    <span class="label1">Uzasadnienie odrzucenia wniosku (200 znaków):</span>
                    <asp:Label ID="lbUzasadnienie" CssClass="textbox value" runat="server" ></asp:Label>    
                    <asp:TextBox ID="tbUzasadnienie" CssClass="textbox" MaxLength="200" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                </div>
                
                <div id="paKierAcc" runat="server">
                    <span class="label">Akceptacja kierownika:</span>
                    <asp:Label ID="lbKierAcc" CssClass="value" runat="server" ></asp:Label>    
                    <asp:Label ID="lbDataKierAcc" CssClass="value" runat="server" ></asp:Label>    
                </div>
                
                <div id="paUwagiAdm" runat="server">
                    <span class="label1">Uwagi administratora (200) znaków:</span>
                    <asp:Label ID="lbUwagiAdm" CssClass="textbox value" runat="server" ></asp:Label>    
                    <asp:TextBox ID="edUwagiAdm" CssClass="textbox" MaxLength="200" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                </div>
                






--%>