<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PodzialLudzi.aspx.cs" Inherits="HRRcp.PodzialLudzi" %>
<%@ Register src="~/Controls/PodzialLudzi/cntPodzialLudzi.ascx" tagname="cntPodzialLudzi" tagprefix="uc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function exportExcelClick() {
            return true;
            //return exportExcel('<%=hidReport.ClientID%>');
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <div class="center pgPodzialLudzi_title">
        <table class="caption" >
            <tr>
                <td class="title">
                    <span class="caption4">
                        <img alt="" src="images/captions/layout_edit.png"/>
                        <asp:Label ID="lbTitle" runat="server" Text="Podzia³ Ludzi"></asp:Label>
                    </span>
                </td>
                <td align="right">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <%--
                            <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                            --%>
                            <asp:Button ID="btBackup" class="button" runat="server" Text="Backup danych" onclick="btBackup_Click" />
                            <uc2:dateedit ID="deNaDzien" runat="server" ValidationGroup="vgImport"/>
                            <asp:Button ID="btImport" class="button" runat="server" Text="Import splitów na dzieñ" onclick="btImport_Click" ValidationGroup="vgImport" />
                            <asp:Button ID="btImportFTE" class="button" runat="server" Text="Przelicz FTE" onclick="btImportFTE_Click" ValidationGroup="vgImport" />
                            <asp:Button ID="btExport" class="button" runat="server" Text="Przelicz koszty" onclick="btExport_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                            <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>
                            <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btRepExcel1" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>     
    </div>
    <div class="mwrapper pgPodzialLudzi">
        <div class="mcontent">
            <asp:HiddenField ID="hidReport" runat="server" />
            <uc1:cntPodzialLudzi ID="cntPodzialLudzi" runat="server" />
        </div>
    </div>
    <div class="center">
        <table class="printoff table0">
            <tr>
                <td class="btprint_bottom2" >
                    <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                    <asp:Button ID="btRepPrint2" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false" />
                    <asp:Button ID="btRepExcel2" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
                </td>
            </tr>
        </table>       
    </div>
    <%--
        AssociatedUpdatePanelID="UpdatePanel1" 
    --%>    
    <asp:UpdateProgress ID="updProgress1" runat="server" 
        DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img alt="Indicator" src="images/activity.gif" /> 
                    <span>Trwa przetwarzanie. Proszê czekaæ ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>    
</asp:Content>







<%--
    Title="select 'Split obowi¹zuj¹cy w okresie: ' + convert(varchar(10),'@p1',20) + ' - ' + convert(varchar(10),'@p2',20)"
--%>
<%--
<uc1:cntReport ID="cntReport1" runat="server" Browser="true"
    CssClass=""
    HeaderVisible="false"
    SQL1="
join ccPrawa PC on PC.UserId = @UserId and PC.CC = ISNULL(I.Class, PC.CC)
    "
    SQL3="
join ccPrawa PR on PR.UserId = @UserId and PR.CC = CC.cc
    "
    SQL="
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @stmt nvarchar(max)

select 
    @colsH = isnull(@colsH + ',', '') + '[' + CC.cc + ']',
    @colsD = isnull(@colsD + ', ', '') + 'ISNULL(ROUND([' + CC.cc + '],4),0) as [' + CC.cc + ':N||' + CC.Nazwa + ']'  
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
    P.KadryId as [nrew:-], 
    P.KadryId as [Nr ew.], 
    P.Nazwisko + '' '' + P.Imie as [Pracownik|RepCCPracCC @p1 @p2 * 0 @nrew 1 1|Czas przepracowany na wszystkie cc], 
	D.Nazwa as [Dzia³], ST.Nazwa as Stanowisko, I.Class as Klasyfikacja,
	K.KadryId as [Nr ew. K], K.Nazwisko + '' '' + K.Imie as Kierownik, ' +
	@colsD +
'FROM
(
	SELECT S.GrSplitu, CC.cc, W.Wsp
	FROM Splity S
	left outer join SplityWsp W on W.IdSplitu = S.Id
	left outer join CC on CC.Id = W.IdCC
	@SQL3
	where @dataDo between S.DataOd and ISNULL(S.DataDo, ''20990909'')
) as D
PIVOT
(
	sum(Wsp) FOR D.cc IN (' + @colsH + ')
) as PV
left outer join Pracownicy P on P.GrSplitu = PV.GrSplitu
left outer join PodzialLudziImport I on I.KadryId = P.KadryId and @dataDo between I.OkresOd and ISNULL(I.OkresDo, ''20990909'')
@SQL1
left outer join Pracownicy K on K.Id = P.IdKierownika
left outer join Dzialy D on D.Id = P.IdDzialu
left outer join Stanowiska ST on ST.Id = P.IdStanowiska
where I.Id is not null'

exec sp_executesql @stmt
        "/>

--%>