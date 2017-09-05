<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCUprawnienia.aspx.cs" Inherits="HRRcp.Reports.RepCCUprawnienia" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">

    <script type="text/javascript">
        function setPrawa(mode, nrew, par) {
            var c = document.getElementById('<%=hidP1.ClientID%>');
            if (c != null) c.value = mode;
            c = document.getElementById('<%=hidP2.ClientID%>');
            if (c != null) c.value = nrew;
            c = document.getElementById('<%=hidP3.ClientID%>');
            if (c != null) c.value = par;
            doClick('<%=btChange.ClientID%>')            
        }
    </script>
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 - 1 019 podzielone, 0 niepodzielone
p4 - 1 dope³nienia, 0 bez dope³nieñ
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <asp:HiddenField ID="hidP1" runat="server" />
    <asp:HiddenField ID="hidP2" runat="server" />
    <asp:HiddenField ID="hidP3" runat="server" />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>

    <asp:Button ID="btChange" runat="server" Text="Zmieñ" CssClass="button_postback" onclick="btChange_Click" />

    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Uprawnienia do raportów podzia³u czasu pracy na CC"
        CssClass="report_page RepCCUprawnienia"
        AllowPaging="false"
        PageSize="30"
        SQL="
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @stmt nvarchar(max)

select  
    @colsH = isnull(@colsH + ',', '') + '[' + Class + ']',
    --@colsD = isnull(@colsD + ', ', '') + 'ISNULL([' + Class + '],''-'') as [' + Class + '|RepCCUprawnienia 2 @nrew ' + Class + '|Dodaj / usuñ dostêp]'  
	@colsD = isnull(@colsD + ', ', '') + 'ISNULL([' + Class + '],''-'') as [' + Class + '|javascript:setPrawa(2,''@nrew'',''' + Class + ''');|Dodaj / usuñ dostêp]'  
	from PodzialLudziImport
	group by Class 
	order by Class

select 
    @colsH = isnull(@colsH + ',', '') + '[' + CC.cc + ']',
    --@colsD = isnull(@colsD + ',', '') + 'ISNULL([' + CC.cc + '],''-'') as [' + CC.cc + '|RepCCUprawnienia 3 @nrew ' + CC.cc + '|' + CC.Nazwa + ']'  
    @colsD = isnull(@colsD + ',', '') + 'ISNULL([' + CC.cc + '],''-'') as [' + CC.cc + '|javascript:setPrawa(3,''@nrew'',''' + CC.cc + ''');|' + CC.Nazwa + ']'  
    from CC 
    order by CC.cc

--select @colsH
--select @colsD

select @stmt = '
SELECT 
    Sort as [Sort:-],
    KadryId as [nrew:-], 
    KadryId as [Nr ew.], 
    Pracownik as [Kierownik],
	--ISNULL(SUBSTRING(Rights, 7, 1), 0) as [Dostêp|RepCCUprawnienia 1 @nrew 6|Dodaj / usuñ dostêp],
	--ISNULL(SUBSTRING(Rights, 8, 1), 0) as [Koszty|RepCCUprawnienia 1 @nrew 7|Dodaj / usuñ dostêp],
	--ISNULL(SUBSTRING(Rights, 9, 1), 0) as [Pe³ny dostêp|RepCCUprawnienia 1 @nrew 8|Dodaj / usuñ dostêp],
	
	--ISNULL(SUBSTRING(Rights, 1, 1), 0) as [Dostêp|javascript:setPrawa(1,''@nrew'',0);| - Dodaj / usuñ prawo],
	
	ISNULL(SUBSTRING(Rights, 1, 1), 0) as [Admin|javascript:setPrawa(1,''@nrew'',0);|Uprawnienia administratora - Dodaj / usuñ prawo],
	ISNULL(SUBSTRING(Rights, 2, 1), 0) as [Prawa|javascript:setPrawa(1,''@nrew'',1);|Nadawanie uprawnieñ - Dodaj / usuñ prawo],
	ISNULL(SUBSTRING(Rights, 3, 1), 0) as [Tester|javascript:setPrawa(1,''@nrew'',2);|Dostêp NoDataInfo funkcjonalnoœci w fazie testów - Dodaj / usuñ prawo],
	ISNULL(SUBSTRING(Rights, 4, 1), 0) as [Kwitek|javascript:setPrawa(1,''@nrew'',3);|Administrator kwitków p³acowych - Dodaj / usuñ prawo],
	ISNULL(SUBSTRING(Rights, 5, 1), 0) as [Czas RCP|javascript:setPrawa(1,''@nrew'',4);|Dostêp do raportu czasu pracy z RCP - Dodaj / usuñ dostêp],
	ISNULL(SUBSTRING(Rights, 6, 1), 0) as [Inni kier|javascript:setPrawa(1,''@nrew'',5);|Mo¿liwoœæ podgl¹du kierowników spoza struktury - Dodaj / usuñ prawo],
	ISNULL(SUBSTRING(Rights, 11, 1), 0) as [Param. Kier|javascript:setPrawa(1,''@nrew'',10);|Mo¿liwoœæ ustawienia czasy przerw i marginesu ostrzegania - Dodaj / usuñ dostêp],
	
	ISNULL(SUBSTRING(Rights, 12, 1), 0) as [Struktura przesuñ|javascript:setPrawa(1,''@nrew'',11);|Przesuniêcia pracowników w strukturze - Dodaj / usuñ dostêp],
	ISNULL(SUBSTRING(Rights, 13, 1), 0) as [Struktura akceptuj|javascript:setPrawa(1,''@nrew'',12);|Akceptowanie przesuniêæ pracowników w strukturze - Dodaj / usuñ dostêp],
	
	ISNULL(SUBSTRING(Rights, 7, 1), 0) as [Podzia³ CC|javascript:setPrawa(1,''@nrew'',6);|Dostêp do raportów podzia³u czasu pracy na cc - Dodaj / usuñ dostêp],
	ISNULL(SUBSTRING(Rights, 8, 1), 0) as [Koszty CC|javascript:setPrawa(1,''@nrew'',7);|Dostêp do podzia³u kwot na cc - Dodaj / usuñ dostêp],
	ISNULL(SUBSTRING(Rights, 9, 1), 0) as [Ca³a klasyfikacja|javascript:setPrawa(1,''@nrew'',8);|Dostêp do wszystkich klasyfikacji - Dodaj / usuñ dostêp],
    ' + @colsD +
'FROM
(
	select 
	P.KadryId, P.Nazwisko + '' '' + P.Imie as Pracownik,
	P.Rights,
	--ISNULL(SUBSTRING(P.Rights, 7, 1), 0) as r6,
	--ISNULL(SUBSTRING(P.Rights, 8, 1), 0) as r7,
	--ISNULL(SUBSTRING(P.Rights, 9, 1), 0) as r8,
	R.CC,
	case when 
		SUBSTRING(P.Rights, 7, 1) = ''1'' or	
		SUBSTRING(P.Rights, 8, 1) = ''1'' or
		SUBSTRING(P.Rights, 9, 1) = ''1'' or
		R.Id is not null
	then 
	    case 
	        --when SUBSTRING(P.Rights, 9, 1) = ''1'' then 1 
	        when SUBSTRING(P.Rights, 7, 1) = ''1'' then 2	
		else 9 end	
	else 10 end as Sort
	from Pracownicy P
	left outer join ccPrawa R on R.UserId = P.Id
	left outer join CC on CC.cc = R.CC
	where 
	    P.Kierownik = 1 or P.Admin = 1 or P.Raporty = 1 or
		SUBSTRING(P.Rights, 7, 1) = ''1'' or	
		SUBSTRING(P.Rights, 8, 1) = ''1'' or
		SUBSTRING(P.Rights, 9, 1) = ''1'' or
		R.Id is not null
) as D
PIVOT
(
	max(D.CC) FOR D.CC IN (' + @colsH + ')
) as PV
order by Sort, Pracownik'

exec sp_executesql @stmt
"/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
