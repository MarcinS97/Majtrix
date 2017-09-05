<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntStartBHP.ascx.cs" Inherits="HRRcp.SzkoleniaBHP.Controls.cntStartBHP" %>

<div id="paStart" runat="server" class="cntStartBHP">
    <asp:HiddenField ID="hidKierId" runat="server" Visible="false"/>
    <asp:GridView ID="GridView1" runat="server" CssClass="GridView3 table" AutoGenerateColumns="true" DataSourceID="SqlDataSource1" PageSize="5" AllowPaging="true" AllowSorting="true">
    </asp:GridView>
    <div class="pager">
        <span class="count">Ilość:<asp:Label ID="GridView1Count" runat="server" ></asp:Label></span>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
        SelectCommand="
declare @dzis datetime, @dni int 
set @dzis = dbo.getdate(GETDATE())
set @dni = 30

select 
P.Nazwisko + ' ' + P.Imie [Pracownik], 
P.KadryId [Logo], 
--P.Opis [Lokalizacja], 
K.Nazwisko + ' ' + K.Imie [Przełożony], 
--K.Email [E-mail], 
C.Symbol [Symbol], 
C.NazwaCertyfikatu [Nazwa], 
C.DataWaznosci [Data ważności:D], 
case when W.WygasaDni &lt; 0 then 'niektualne' else convert(varchar, W.WygasaDni) end [Wygasa za (dni):N]
--X.Id as PominId, X.Od as PominOd, X.Do as PominDo, X.Powod, PowodKod
from dbo.fn_GetTree2(case when @kierId = -99 then 0 else @kierId end, 1, GETDATE()) P
left join Pracownicy K on K.Id = P.IdKierownika
left join PlanUrlopowPomin X on X.IdPracownika = P.IdPracownika and @dzis between X.Od and ISNULL(X.Do, '20990909')
left join VCertyfikatyUprawnienie3 C on C.IdPracownika = P.IdPracownika and C.Id = C.Id2 and C.UprId in (select Id from Uprawnienia where Typ = 1024 and KwalifikacjeId = 23 and Aktywne = 1) --BHP-W,O,P
outer apply (select DATEDIFF(D, @dzis, C.DataWaznosci) as WygasaDni) W
where P.KadryId &lt; 80000 and P.Status != -2
and C.DataWaznosci &lt;= DATEADD(D,@dni,@dzis)
order by W.WygasaDni, C.DataWaznosci, Pracownik        
        ">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidKierId" Name="kierId" PropertyName="Value" Type="Int32"/>
        </SelectParameters>
    </asp:SqlDataSource>    
</div>


<%--
declare @dzis datetime, @dni int 
set @dzis = dbo.getdate(GETDATE())
set @dni = 30

select 
P.Nazwisko + ' ' + P.Imie [Pracownik], 
P.KadryId [Logo], 
--P.Opis [Lokalizacja], 
K.Nazwisko + ' ' + K.Imie [Przełożony], 
--K.Email [E-mail], 
C.Symbol [Symbol], 
C.NazwaCertyfikatu [Nazwa], 
C.DataWaznosci [Data ważności:D], 
case when W.WygasaDni &lt; 0 then 'niektualne' else convert(varchar, W.WygasaDni) end [Wygasa za (dni):N]
--X.Id as PominId, X.Od as PominOd, X.Do as PominDo, X.Powod, PowodKod
from VPrzypisaniaNaDzis P
left join Pracownicy K on K.Id = P.KierId
left join PlanUrlopowPomin X on X.IdPracownika = P.Id and @dzis between X.Od and ISNULL(X.Do, '20990909')
left join VCertyfikatyUprawnienie3 C on C.IdPracownika = P.Id and C.Id = C.Id2 and C.UprId in (select Id from Uprawnienia where Typ = 1024 and KwalifikacjeId = 23 and Aktywne = 1) --BHP-W,O,P
outer apply (select DATEDIFF(D, @dzis, C.DataWaznosci) as WygasaDni) W
where P.KierId is not null and P.KadryId &lt; 80000
and C.DataWaznosci &lt;= DATEADD(D,@dni,@dzis)
order by W.WygasaDni, C.DataWaznosci, Pracownik        
--%>