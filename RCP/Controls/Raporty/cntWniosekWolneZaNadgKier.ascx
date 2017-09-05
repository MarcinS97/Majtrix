<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWniosekWolneZaNadgKier.ascx.cs" Inherits="HRRcp.Controls.Raporty.cntWniosekWolneZaNadg" %>

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
            <span class="left">
                <span class="left">
			        <span class="firm">SIVANTOS. SP. Z O.O.</span><br />
			        <span>61–013 Poznań, ul. Bałtycka 6</span><br />
                </span>
            </span>

            <br />
            <br />
            <br />
            <span class="right">
                <span class="left">
                    <span>Szanowny Pan/Szanowna Pani</span><br />
                        <asp:Label ID="PracownikLabel" runat="server" CssClass="value" Text='<%# Eval("Pracownik") %>' /><br /><br />
                    <span class="label1">Nr ewidencyjny:</span>
                        <asp:Label ID="PracLogoLabel" CssClass="value" runat="server" Text='<%# Eval("PracLogo") %>' /><br />
                    <span class="label1">Stanowisko:</span>
                        <asp:Label ID="StanowiskoLabel" CssClass="value" runat="server" Text='<%# Eval("Stanowisko") %>' />
                </span>
            </span>
            <br />
            <br />
            <br />

            <span class="center">
                <span class="title">UDZIELENIE CZASU WOLNEGO W ZAMIAN ZA PRACĘ W GODZINACH NADLICZBOWYCH</span>
            </span>
            <br />
            <br />
            <br />


            W związku z wykonywaniem przez Pana/Panią na rzecz Pracodawcy pracy w godzinach nadliczbowych w dniu/dniach:<br />
	        <asp:Label ID="InfoLabel" CssClass="value" runat="server" Text='<%# Eval("Info") %>' />, w wymiarze 
	        <asp:Label ID="GodzinLabel" CssClass="value" runat="server" Text='<%# Eval("NadgHMM") %>' /> godzin:minut,
            udzielam Panu/Pani za tą pracę czasu wolnego od pracy w wymiarze o połowę wyższym tj. w wymiarze 
            <asp:Label ID="Label3" CssClass="value" runat="server" Text='<%# Eval("GodzinHMM") %>' /> godzin:minut, w terminie 
            <asp:Label ID="Label2" CssClass="value" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />. 
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <span class="right">
                <span class="center">
                    ......................................................<br />
                    podpis pracodawcy
                </span>
            </span>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
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
    ,dbo.ToTimeHMM(Godzin) as GodzinHMM
    ,dbo.ToTimeHMM(Godzin2) as NadgHMM
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
            <asp:ControlParameter ControlID="hidWniosekId" Name="wniosekId" 
                PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>