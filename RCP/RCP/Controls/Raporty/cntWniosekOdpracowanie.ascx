<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWniosekOdpracowanie.ascx.cs" Inherits="HRRcp.RCP.Controls.Raporty.cntWniosekOdpracowanie" %>

<div id="paWniosekWolneZaNadg" runat="server" class="cntWniosekWolneZaNadg">
    <asp:HiddenField ID="hidWniosekId" runat="server" Visible="false" />
    <asp:ListView ID="lvWniosek" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id">
        <ItemTemplate>
            <div class="item" style="width: 815px;">
            <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/images/RepLogo_Siemens.jpg" />--%><br />
            <div style="text-align: right;">
                <div style="float: left;">
                    <%--<div>.............................................</div>--%>
<%--                    Nazwisko i imię pracownika --%>
                    <div style="text-align: left;">
                        <span class="label1">Pracownik:</span>
                        <asp:Label ID="PracownikLabel" runat="server" CssClass="value" Text='<%# Eval("Pracownik") %>' /><br />            
                        <span class="label1">Nr ewidencyjny:</span>
                        <asp:Label ID="PracLogoLabel" CssClass="value" runat="server" Text='<%# Eval("PracLogo") %>' /><br />
                        <span class="label1">Stanowisko:</span>
                        <asp:Label ID="StanowiskoLabel" CssClass="value" runat="server" Text='<%# Eval("Stanowisko") %>' />
                    </div>
                    <br />               
                </div>
                <span style="">
                    .............................................<br />
                    (miejscowość i data)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" CssClass="value" Text='<%# Eval("DataWniosku", "{0:d}") %>' Visible="false"/>
                </span>
            </div>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <span class="center">
                <span class="title">WNIOSEK O WYRAŻENIE ZGODY NA ZWOLNIENIE OD PRACY W CELU ZAŁATWIENIA SPRAW OSOBISTYCH LUB RODZINNYCH</span>
            </span>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />


	        Zwracam się z wnioskiem o wyrażenie zgody na zwolnienie mnie od pracy w dniu 
	        <asp:Label ID="OdLabel" CssClass="value" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />, <br /><br /> 
	        w godzinach od .......... do ..........<br /><br /> 
	        Niniejszy wniosek motywuję koniecznością: <br /><br />
	        .......................................................................................................................................................... <br /><br />
	        Wskazuję przy tym, że załatwienie ww. spraw nie jest możliwe poza godzinami pracy. 
	        Jednocześnie oświadczam, iż jestem świadomy/a, iż aby uzyskać wynagrodzenie za czas powyższego zwolnienia muszę je odpracować w terminie wskazanym przez Pracodawcę, na co niniejszym wyrażam zgodę. 
	        Jestem świadom/a również, że czas odpracowania nie stanowi pracy w godzinach nadliczbowych.   

            <br />
            <br />
            <br />
            <br />
            <span class="right">
                <span class="center">
                    ......................................................<br />
                    podpis pracownika
                </span>
            </span>

            <br />
            <br />
            Wyrażam zgodę/Nie wyrażam zgody*
            <br />
            <br />
            <br />
            <br />
            <span class="left">
                <span class="center">
                    ......................................................<br />
                    podpis Pracodawcy
                </span>
            </span>
            <br />
            <br />
            

            * niepotrzebne skreślić
            <br />
            <br />

            </div>
        </ItemTemplate>
        <EmptyDataTemplate>
            <div>Brak danych</div>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div ID="itemPlaceholderContainer" runat="server" class="container">
                <div ID="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
    </asp:ListView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
select T.Typ, T.TypNapis, ST.Status
    ,W.IdPracownika, ISNULL(8 * P.EtatL / P.EtatM, 8) as GodzinZm
    ,P.KadryId as PracLogo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.Email
    ,K.KadryId as KierLogo, K.Nazwisko + ' ' + K.Imie as Kierownik
    ,D.Nazwa as Dzial
    ,S.Nazwa as Stanowisko
    ,Z.KadryId as ZastLogo 
    ,W.IdZastepuje, ISNULL(Zastepuje, Z.Nazwisko + ' ' + Z.Imie + ISNULL(' (' + Z.KadryId + ')', '')) as Zastepuje
    ,KA.Nazwisko + ' ' + KA.Imie as KierAcc
    ,KZ.Nazwisko + ' ' + KZ.Imie as KierAccZast
    ,A.Nazwisko + ' ' + A.Imie as KadryAcc
    ,W.*
from poWnioskiUrlopowe W
left join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left join Pracownicy P on P.Id = W.IdPracownika
--left join PracownicyStanowiska PS on PS.IdPracownika = W.IdPracownika and W.Od between PS.Od and ISNULL(PS.Do, '20990909')
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = W.IdPracownika and Od &lt;= W.Od order by Od desc) PS
left join Przypisania R on R.IdPracownika = P.Id and W.Od between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1 
left join Pracownicy K on K.Id = R.IdKierownika
left join Pracownicy Z on Z.Id = W.IdZastepuje
left join Pracownicy KA on KA.Id = W.IdKierAcc
left join Pracownicy KZ on KZ.Id = W.IdKierAccZast
left join Pracownicy A on A.Id = W.DataKadryAcc
left join Dzialy D on D.Id = PS.IdDzialu
left join Stanowiska S on S.Id = PS.IdStanowiska
where W.Id = @wniosekId
        ">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidWniosekId" Name="wniosekId" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>
