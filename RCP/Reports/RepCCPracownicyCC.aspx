<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCPracownicyCC.aspx.cs" Inherits="HRRcp.Reports.RepCCPracownicyCC" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 - split 019: 1 podzielony, 0 niepodzielony
p4 - dopełnienia: 1 z dopełnieniami, 0 bez dopełnień (na razie tylko 1)
p5 - class lub *
p6 - nrew kierownika lub *
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">



    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Menu ID="tabWyniki" CssClass="printoff" runat="server" Orientation="Horizontal" 
                Visible="false"
                onmenuitemclick="tabWyniki_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Nadgodziny łącznie" Value="0" Selected="True" ></asp:MenuItem>
                    <asp:MenuItem Text="Koszty łącznie" Value="1" ></asp:MenuItem>
                </Items>
                <StaticItemTemplate>
                    <div class="tabCaption">
                        <div class="tabLeft">
                            <div class="tabRight">
                                <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                            </div>
                        </div>
                    </div>
                </StaticItemTemplate>
            </asp:Menu>

            <div class="tabsContent" style="border-collapse:collapse; background-color:#FFF;">

                <uc2:SelectOkres ID="SelectOkres1" runat="server" Visible="false"/>

                <asp:MultiView ID="mvWyniki" runat="server" ActiveViewIndex="0">
        <%----------------------------------------------------------------------------------%>
                    <asp:View ID="View1" runat="server" >
        <%----------------------------------------------------------------------------------%>
            <uc1:cntReport ID="cntReport2" runat="server" Browser="true"
                Title="select 'Nadgodziny łącznie w okresie: ' + convert(varchar(10),'@p1',20) + ' - ' + convert(varchar(10),'@p2',20)"
                Title2="select
        case when @p3=1 then '019 podzielone' else '019 niepodzielone' end +
        case when @p4=1 then '' else ', bez dopełnień wynikających z zaokrągleń' end
                "
                SQL1="
        join ccPrawa PC on PC.UserId = @UserId and PC.CC = ISNULL(S.Class, PC.CC)
        join ccPrawa PR on PR.UserId = @UserId and PR.CC = CC.cc
                "
                SQL3="
        join ccPrawa PR on PR.UserId = @UserId and PR.CC = CC.cc
                "
                SQL="
        declare 
            @colsH nvarchar(max), 
            @colsD nvarchar(max), 
            @sumCC nvarchar(max), 
            @stmt nvarchar(max)

        select 
            @colsH = isnull(@colsH + ',', '') + '[' + CC.cc + ']',
            @colsD = isnull(@colsD + ', ', '') + 'ISNULL(ROUND([' + CC.cc + '],4),0) as [' + CC.cc + ':NS|RepCCPracDni @p1 @p2 ' + CC.cc + ' 150 @nrew @p3 @p4|' + CC.Nazwa + ']',
            @sumCC = isnull(@sumCC + '+', '') + 'ISNULL([' + CC.cc + '],0)'
            from CC 
            @SQL3
            where '@p2' between AktywneOd and ISNULL(AktywneDo, '20990909') 
            order by CC.cc

        select @stmt = '
        declare @dataDo datetime
        declare @dataOd datetime
        set @dataOd = ''@p1''
        set @dataDo = ''@p2''

        SELECT 
            Logo as [nrew:-],
	        Logo as [Nr ew.], 
	        Pracownik as [Pracownik|RepCCPracCC @p1 @p2 * 0 @nrew @p3 @p4|Czas przepracowany na wszystkie cc], 
	        Dzial as [Dział], Stanowisko, Class as Klasyfikacja,
	        KierLogo as [Nr ew. K], Kierownik,' 
	        --+ @sumCC + ' as [Suma:N0S|RepCCPracDni @p1 @p2 * 150 @nrew @p3 @p4|Dni z nadgodzinami (50+100) przepracowanymi na cc],'
	        + @sumCC + ' as [Suma:N0S|RepCCPracDni @p1 @p2 * 0 @nrew @p3 @p4|Czas przepracowany],'
	        + @colsD +
        'FROM
        (
	        select 
		        S.Logo, S.Pracownik, S.Dzial, S.Stanowisko, S.Class,
		        S.KierLogo, S.Kierownik, 
		        S.cc --,CC.Nazwa 
		        --,SUM(S.vCzasZm) sCzasZm 
		        --,SUM(S.vNadg50) sNadg50
		        --,SUM(S.vNadg50) sNadg100 
		        --,SUM(S.vNocne)  sNocne
        		
		        ,SUM(S.vNadg50) + SUM(S.vNadg100) as sNadgodziny
        		
	        from VrepDaneMPK S
	        left outer join CC on CC.cc = S.CC
	        --@SQL1
	        where S.Data between @dataOd and @dataDo  
            and (
                (@p3=1 and S.cc not in (select cc from CC where GrSplitu is not null))  -- bez 019, podzielone
             or (@p3=0 and S.Typ &lt; 10)    -- z 019, niepodzielone 
            )
            and (@p4=1 or S.Typ not in (3,13))  -- bez dopelnien
	        group by S.Logo, S.Pracownik, S.Dzial, S.Stanowisko, S.Class, S.KierLogo, S.Kierownik, S.cc, CC.Nazwa
        ) as D
        PIVOT
        (
	        --sum(sCzasZm) 
	        --sum(sNadg50) 
	        sum(sNadgodziny)
	        FOR D.cc IN (' + @colsH + ')
        ) as PV
        order by Logo'

        exec sp_executesql @stmt
            "/>
                    </asp:View>
        <%----------------------------------------------------------------------------------%>
                    <asp:View ID="View2" runat="server" >
        <%----------------------------------------------------------------------------------%>
            <uc1:cntReport ID="cntReport3" runat="server" Browser="true"
                Title="select 'Koszty łącznie (50+100+Nocne) w okresie: ' + convert(varchar(10),'@p1',20) + ' - ' + convert(varchar(10),'@p2',20)"
                Title2="select
        case when @p3=1 then '019 podzielone' else '019 niepodzielone' end +
        case when @p4=1 then '' else ', bez dopełnień wynikających z zaokrągleń' end
                "
                SQL1="
        join ccPrawa PC on PC.UserId = @UserId and PC.CC = ISNULL(S.Class, PC.CC)
        join ccPrawa PR on PR.UserId = @UserId and PR.CC = CC.cc
                "
                SQL3="
        join ccPrawa PR on PR.UserId = @UserId and PR.CC = CC.cc
                "
                SQL="
        declare 
            @colsH nvarchar(max), 
            @colsD nvarchar(max), 
            @sumCC nvarchar(max), 
            @stmt nvarchar(max)

        select 
            @colsH = isnull(@colsH + ',', '') + '[' + CC.cc + ']',
            @colsD = isnull(@colsD + ', ', '') + 'ISNULL(ROUND([' + CC.cc + '],4),0) as [' + CC.cc + ':N0.00S|RepCCPracDni @p1 @p2 ' + CC.cc + ' 152 @nrew @p3 @p4|' + CC.Nazwa + ']',
            @sumCC = isnull(@sumCC + '+', '') + 'ISNULL([' + CC.cc + '],0)'
            from CC 
            @SQL3
            where '@p2' between AktywneOd and ISNULL(AktywneDo, '20990909') 
            order by CC.cc

        select @stmt = '
        declare @dataDo datetime
        declare @dataOd datetime
        set @dataOd = ''@p1''
        set @dataDo = ''@p2''

        SELECT 
            Logo as [nrew:-],
	        Logo as [Nr ew.], 
	        Pracownik as [Pracownik|RepCCPracCC @p1 @p2 * 0 @nrew @p3 @p4|Czas przepracowany na wszystkie cc], 
	        Dzial as [Dział], Stanowisko, Class as Klasyfikacja,
	        KierLogo as [Nr ew. K], Kierownik, 
	        StawkaGodz as [Stawka godz.:N0.0000],
	        ROUND(' + @sumCC + ',2) as [Suma:N0.00S|RepCCPracDni @p1 @p2 * 0 @nrew @p3 @p4|Czas przepracowany],'
	        + @colsD +
        'FROM
        (
	        select 
		        S.Logo, S.Pracownik, S.Dzial, S.Stanowisko, S.Class,
		        S.KierLogo, S.Kierownik, 
		        S.StawkaGodz,
		        S.cc,
		        ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) +
                ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2)  ,2),0) +
                ISNULL(round(sum(S.vNocne   * S.StawkaNocna)     ,2),0) as [sKoszty]
	        from VrepDaneMPK S
	        left outer join CC on CC.cc = S.CC
	        @SQL1
	        where S.Data between @dataOd and @dataDo  
            and (
                (@p3=1 and S.cc not in (select cc from CC where GrSplitu is not null))  -- bez 019, podzielone
             or (@p3=0 and S.Typ &lt; 10)    -- z 019, niepodzielone 
            )
            and (@p4=1 or S.Typ not in (3,13))  -- bez dopelnien
	        group by S.Logo, S.Pracownik, S.Dzial, S.Stanowisko, S.Class, S.KierLogo, S.Kierownik, S.StawkaGodz, S.cc
        ) as D
        PIVOT
        (
	        sum(sKoszty)
	        FOR D.cc IN (' + @colsH + ')
        ) as PV
        order by Logo'

        exec sp_executesql @stmt
            "/>
                    </asp:View>
        <%----------------------------------------------------------------------------------%>
                    <asp:View ID="View3" runat="server" >
        <%----------------------------------------------------------------------------------%>
                    </asp:View>
        <%----------------------------------------------------------------------------------%>
                    <asp:View ID="View4" runat="server" >
        <%----------------------------------------------------------------------------------%>
                        <uc1:cntReport ID="cntReport4" runat="server" 
                            Title="Podział kosztów na CC"
                            SQL=""
                        />
                    </asp:View>
        <%----------------------------------------------------------------------------------%>
                    <asp:View ID="View5" runat="server" >
                    </asp:View>
                    <asp:View ID="View6" runat="server" >
                    </asp:View>
                    <asp:View ID="View7" runat="server" >
                    </asp:View>
                    <asp:View ID="View8" runat="server" >
                    </asp:View>
                    <asp:View ID="View9" runat="server" >
                    </asp:View>
                    <asp:View ID="View10" runat="server" >
                        <uc1:cntReport ID="cntReport1" runat="server" 
                            Title="Nadgodziny łącznie na cc"
                            Title2='<%# "Okres rozliczeniowy: " + GetOkres() %>'
                            SQL="select * from OkresyRozl "
                        />
                    </asp:View>

                </asp:MultiView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

