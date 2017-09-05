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
p4 - 1 dope�nienia, 0 bez dope�nie�
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <asp:HiddenField ID="hidP1" runat="server" />
    <asp:HiddenField ID="hidP2" runat="server" />
    <asp:HiddenField ID="hidP3" runat="server" />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>

    <asp:Button ID="btChange" runat="server" Text="Zmie�" CssClass="button_postback" onclick="btChange_Click" />

    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Uprawnienia do raport�w podzia�u czasu pracy na CC"
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
    --@colsD = isnull(@colsD + ', ', '') + 'ISNULL([' + Class + '],''-'') as [' + Class + '|RepCCUprawnienia 2 @nrew ' + Class + '|Dodaj / usu� dost�p]'  
	@colsD = isnull(@colsD + ', ', '') + 'ISNULL([' + Class + '],''-'') as [' + Class + '|javascript:setPrawa(2,''@nrew'',''' + Class + ''');|Dodaj / usu� dost�p]'  
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
	--ISNULL(SUBSTRING(Rights, 7, 1), 0) as [Dost�p|RepCCUprawnienia 1 @nrew 6|Dodaj / usu� dost�p],
	--ISNULL(SUBSTRING(Rights, 8, 1), 0) as [Koszty|RepCCUprawnienia 1 @nrew 7|Dodaj / usu� dost�p],
	--ISNULL(SUBSTRING(Rights, 9, 1), 0) as [Pe�ny dost�p|RepCCUprawnienia 1 @nrew 8|Dodaj / usu� dost�p],
	
	--ISNULL(SUBSTRING(Rights, 1, 1), 0) as [Dost�p|javascript:setPrawa(1,''@nrew'',0);| - Dodaj / usu� prawo],
	
	ISNULL(SUBSTRING(Rights, 1, 1), 0) as [Admin|javascript:setPrawa(1,''@nrew'',0);|Uprawnienia administratora - Dodaj / usu� prawo],
	ISNULL(SUBSTRING(Rights, 2, 1), 0) as [Prawa|javascript:setPrawa(1,''@nrew'',1);|Nadawanie uprawnie� - Dodaj / usu� prawo],
	ISNULL(SUBSTRING(Rights, 3, 1), 0) as [Tester|javascript:setPrawa(1,''@nrew'',2);|Dost�p NoDataInfo funkcjonalno�ci w fazie test�w - Dodaj / usu� prawo],
	ISNULL(SUBSTRING(Rights, 4, 1), 0) as [Kwitek|javascript:setPrawa(1,''@nrew'',3);|Administrator kwitk�w p�acowych - Dodaj / usu� prawo],
	ISNULL(SUBSTRING(Rights, 5, 1), 0) as [Czas RCP|javascript:setPrawa(1,''@nrew'',4);|Dost�p do raportu czasu pracy z RCP - Dodaj / usu� dost�p],
	ISNULL(SUBSTRING(Rights, 6, 1), 0) as [Inni kier|javascript:setPrawa(1,''@nrew'',5);|Mo�liwo�� podgl�du kierownik�w spoza struktury - Dodaj / usu� prawo],
	ISNULL(SUBSTRING(Rights, 11, 1), 0) as [Param. Kier|javascript:setPrawa(1,''@nrew'',10);|Mo�liwo�� ustawienia czasy przerw i marginesu ostrzegania - Dodaj / usu� dost�p],
	
	ISNULL(SUBSTRING(Rights, 12, 1), 0) as [Struktura przesu�|javascript:setPrawa(1,''@nrew'',11);|Przesuni�cia pracownik�w w strukturze - Dodaj / usu� dost�p],
	ISNULL(SUBSTRING(Rights, 13, 1), 0) as [Struktura akceptuj|javascript:setPrawa(1,''@nrew'',12);|Akceptowanie przesuni�� pracownik�w w strukturze - Dodaj / usu� dost�p],
	
	ISNULL(SUBSTRING(Rights, 7, 1), 0) as [Podzia� CC|javascript:setPrawa(1,''@nrew'',6);|Dost�p do raport�w podzia�u czasu pracy na cc - Dodaj / usu� dost�p],
	ISNULL(SUBSTRING(Rights, 8, 1), 0) as [Koszty CC|javascript:setPrawa(1,''@nrew'',7);|Dost�p do podzia�u kwot na cc - Dodaj / usu� dost�p],
	ISNULL(SUBSTRING(Rights, 9, 1), 0) as [Ca�a klasyfikacja|javascript:setPrawa(1,''@nrew'',8);|Dost�p do wszystkich klasyfikacji - Dodaj / usu� dost�p],
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
