<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWniosekOdpracowanie.ascx.cs" Inherits="HRRcp.Controls.Raporty.cntWniosekOdpracowanie" %>


<div id="paWniosekWolneZaNadg" runat="server" class="cntWniosekWolneZaNadg">
    <asp:HiddenField ID="hidWniosekId" runat="server" Visible="false" />

    <asp:ListView ID="lvWniosek" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id">
        <ItemTemplate>
            <div class="item">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/RepLogo_Siemens.jpg" /><br />
            
            <span class="right">
                Poznań, dnia ..............................
                <asp:Label ID="Label1" runat="server" CssClass="value" Text='<%# Eval("DataWniosku", "{0:d}") %>' Visible="false"/>
            </span>

            <br />
            <br />
            <br />
            <br />
            <span class="label1">Pracownik:</span>
                <asp:Label ID="PracownikLabel" runat="server" CssClass="value" Text='<%# Eval("Pracownik") %>' /><br />            
            <span class="label1">Nr ewidencyjny:</span>
                <asp:Label ID="PracLogoLabel" CssClass="value" runat="server" Text='<%# Eval("PracLogo") %>' /><br />
            <span class="label1">Stanowisko:</span>
                <asp:Label ID="StanowiskoLabel" CssClass="value" runat="server" Text='<%# Eval("Stanowisko") %>' />
            <br />
            <br />
            <br />
            <span class="right">
                <span class="left">
			        <span class="bold2">SIVANTOS. SP. Z O.O.</span><br />
			        <span class="bold">61–013 Poznań, ul. Bałtycka 6</span><br />
                </span>
            </span>
            <br />
            <br />
            <br />

            <span class="center">
                <span class="title">WNIOSEK O WYRAŻENIE ZGODY NA ZWOLNIENIE OD PRACY W CELU ZAŁATWIENIA SPRAW OSOBISTYCH LUB RODZINNYCH</span>
            </span>
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

<%--

            <span style="">Typ:
            <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            <br />
            TypNapis:
            <asp:Label ID="TypNapisLabel" runat="server" Text='<%# Eval("TypNapis") %>' />
            <br />
            Status:
            <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            <br />
            IdPracownika:
            <asp:Label ID="IdPracownikaLabel" runat="server" 
                Text='<%# Eval("IdPracownika") %>' />
            
            <br />
            GodzinZm:
            <asp:Label ID="GodzinZmLabel" runat="server" 
                Text='<%# Eval("GodzinZm") %>' />
            <br />
            PracLogo:
            Email:
            <asp:Label ID="EmailLabel" runat="server" 
                Text='<%# Eval("Email") %>' />
            <br />
            KierLogo:
            <asp:Label ID="KierLogoLabel" runat="server" Text='<%# Eval("KierLogo") %>' />
            <br />
            Kierownik:
            <asp:Label ID="KierownikLabel" runat="server" 
                Text='<%# Eval("Kierownik") %>' />
            <br />
            Dzial:
            <asp:Label ID="DzialLabel" runat="server" Text='<%# Eval("Dzial") %>' />
            <br />
            Stanowisko:
            
            <br />
            ZastLogo:
            <asp:Label ID="ZastLogoLabel" runat="server" Text='<%# Eval("ZastLogo") %>' />
            <br />
            IdZastepuje:
            <asp:Label ID="IdZastepujeLabel" runat="server" 
                Text='<%# Eval("IdZastepuje") %>' />
            <br />
            Zastepuje:
            <asp:Label ID="ZastepujeLabel" runat="server" 
                Text='<%# Eval("Zastepuje") %>' />
            <br />
            KierAcc:
            <asp:Label ID="KierAccLabel" runat="server" Text='<%# Eval("KierAcc") %>' />
            <br />
            KierAccZast:
            <asp:Label ID="KierAccZastLabel" runat="server" 
                Text='<%# Eval("KierAccZast") %>' />
            <br />
            KadryAcc:
            <asp:Label ID="KadryAccLabel" runat="server" 
                Text='<%# Eval("KadryAcc") %>' />
            <br />
            Id:
            <asp:Label ID="IdLabel" runat="server" 
                Text='<%# Eval("Id") %>' />
            <br />
            TypId:
            <asp:Label ID="TypIdLabel" runat="server" 
                Text='<%# Eval("TypId") %>' />
            <br />
            AutorId:
            <asp:Label ID="AutorIdLabel" runat="server" 
                Text='<%# Eval("AutorId") %>' />
            <br />
            DataWniosku:
            <asp:Label ID="DataWnioskuLabel" runat="server" 
                Text='<%# Eval("DataWniosku") %>' />
            <br />
            IdPracownika1:
            <asp:Label ID="IdPracownika1Label" runat="server" 
                Text='<%# Eval("IdPracownika1") %>' />
            <br />
            IdPrzelozony:
            <asp:Label ID="IdPrzelozonyLabel" runat="server" 
                Text='<%# Eval("IdPrzelozony") %>' />
            <br />
            ProjektDzial:
            <asp:Label ID="ProjektDzialLabel" runat="server" 
                Text='<%# Eval("ProjektDzial") %>' />
            <br />
            Stanowisko1:
            <asp:Label ID="Stanowisko1Label" runat="server" 
                Text='<%# Eval("Stanowisko1") %>' />
            <br />
            Info:
            
            <br />
            UzasadnieniePrac:
            <asp:Label ID="UzasadnieniePracLabel" runat="server" 
                Text='<%# Eval("UzasadnieniePrac") %>' />
            <br />
            Od:
            
            <br />
            Do:
            <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do") %>' />
            <br />
            Godzin:
            
            <br />
            Dni:
            <asp:Label ID="DniLabel" runat="server" Text='<%# Eval("Dni") %>' />
            <br />
            IdZastepuje1:
            <asp:Label ID="IdZastepuje1Label" runat="server" 
                Text='<%# Eval("IdZastepuje1") %>' />
            <br />
            Zastepuje1:
            <asp:Label ID="Zastepuje1Label" runat="server" 
                Text='<%# Eval("Zastepuje1") %>' />
            <br />
            IdKierAcc:
            <asp:Label ID="IdKierAccLabel" runat="server" Text='<%# Eval("IdKierAcc") %>' />
            <br />
            IdKierAccZast:
            <asp:Label ID="IdKierAccZastLabel" runat="server" 
                Text='<%# Eval("IdKierAccZast") %>' />
            <br />
            DataKierAcc:
            <asp:Label ID="DataKierAccLabel" runat="server" 
                Text='<%# Eval("DataKierAcc") %>' />
            <br />
            UzasadnienieKier:
            <asp:Label ID="UzasadnienieKierLabel" runat="server" 
                Text='<%# Eval("UzasadnienieKier") %>' />
            <br />
            IdKadryAcc:
            <asp:Label ID="IdKadryAccLabel" runat="server" 
                Text='<%# Eval("IdKadryAcc") %>' />
            <br />
            DataKadryAcc:
            <asp:Label ID="DataKadryAccLabel" runat="server" 
                Text='<%# Eval("DataKadryAcc") %>' />
            <br />
            UwagiKadry:
            <asp:Label ID="UwagiKadryLabel" runat="server" 
                Text='<%# Eval("UwagiKadry") %>' />
            <br />
            StatusId:
            <asp:Label ID="StatusIdLabel" runat="server" Text='<%# Eval("StatusId") %>' />
            <br />
            <asp:CheckBox ID="WprowadzonyCheckBox" runat="server" 
                Checked='<%# Eval("Wprowadzony") %>' Enabled="false" Text="Wprowadzony" />
            <br />
            DataImportu:
            <asp:Label ID="DataImportuLabel" runat="server" 
                Text='<%# Eval("DataImportu") %>' />
            <br />
            <br />

--%>
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
left join PracownicyStanowiska PS on PS.IdPracownika = W.IdPracownika and W.Od between PS.Od and ISNULL(PS.Do, '20990909')
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