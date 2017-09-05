<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPodzialLudziMies.ascx.cs" Inherits="HRRcp.Controls.PodzialLudzi.cntPodzialLudziMies" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport2" tagprefix="uc1" %>
<%@ Register src="~/Controls/Przypisania/cntSplityWsp.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>

<%--
/*
0				1		  2		   3			
Brak podziału - Otwarty - Edycja - Zamknięty
Brak podziału - Otwarty - Korekta - Zamknięty

- import
- edycja
			- blokada importu
			- edycja
					- blokada importu
					- blokada edycji
*/					
|◄ ◄ 1 2 3 4 5  ... ► ►| 
--%>




<div id="paPodzialLudziMies" runat="server" class="cntPodzialLudziMies" >
    <asp:HiddenField ID="hidStatusPar" runat="server" />
    <script type="text/javascript">
        function chgStPL(cnt, id, st) {
            if (confirm("Potwierdź zmianę statusu.")) {
                gvCommand(cnt, "status", id + '|' + st);
            }
        }

        function chgStPL_1(id, st) {
            if (confirm("Potwierdź zmianę statusu.")) {
                var par = document.getElementById('<%= hidStatusPar.ClientID %>');
                if (par != null) {
                    par.value = id + '|' + st;
                    doClick('<%= btChangeStatus.ClientID %>');
                }
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>                        
            <uc1:cntReport2 ID="cntMies" runat="server" 
                SQL="
declare @stmt nvarchar(max)
declare @grupy nvarchar(max)
declare @prev nvarchar(max)
declare @next nvarchar(max)
declare @admin int 
set @admin = @SQL1

select @grupy = ISNULL(@grupy,'') + 'case when ''' + convert(varchar(10),CC.AktywneOd,20) + ''' &lt;= R.DataDo and R.DataOd &lt;= ''' + convert(varchar(10),ISNULL(CC.AktywneDo,'20990909'),20) + ''' then ''' + cc + ''' else '''' end [' + case when @grupy is null then 'Splity' else '' end + ':;control|cmd:split @od @status ' + convert(varchar,ISNULL(GrSplitu,0)) + '],' 
from CC where CC.Grupa = 1
order by cc

if @admin = 1 begin 
	set @prev = '
case when ISNULL(N.Status,0) = 0 then 
    case R.StatusPL 
    when 0 then ''''
    when 1 then ''''
    when 2 then ''Otwarty ←''
    else        ''Korekta ←''
    end 
else ''''
--end as [:;control|cmd:stprev @id @statusPL],
end as [:;control prev|js:chgStPL(this @id @statusPL-1)],
	'
	set @next = '
case when ISNULL(N.Status,0) = 0 then 
    case R.StatusPL 
    when 0 then ''→ Otwórz''
    when 1 then ''→ Korekta''
    when 2 then ''→ Zamknięty''
    else        ''''
    end 
else ''''
--end as [:;control|cmd:stnext @id @statusPL],
end as [:;control next|js:chgStPL(this @id @statusPL+1)],
	'
end
else begin
	set @prev = ''
	set @next = ''
end

set @stmt = '
declare @admin int
set @admin = ' + CONVERT(varchar,@admin) + '

select 
R.Id [id:-],
R.DataOd [od:-],
R.DataDo [do:-],
R.StatusPL [statusPL:-],  -- musi być przed status
R.Status [status:-],
CONVERT(varchar(7), R.DataDo, 20) + '' '' + case when R.Status = 1 then ''Zamknięty'' else ''Otwarty'' end [Okres rozliczeniowy],

' + @prev + '
case R.StatusPL 
when 0 then ''Brak podziału''
when 1 then ''Otwarty''
when 2 then ''Korekta''
else        ''Zamknięty''
end as [Status:;status],
' + @next + '

' + @grupy + '

R.DataSplitow [Data splitów:D],
R.DataImportu [Data danych:D],

--case when R.Status = 0 then ''Importuj'' else null end [:;control|cmd:import @od @do],

''Podział'' [:;control|PodzialLudzi @od @do @id]
from OkresyRozl R
left join OkresyRozl N on DATEADD(D,1,R.DataDo) between N.DataOd and N.DataDo
--where R.DataImportu is not null
where (@admin = 1 or R.DataImportu is not null)
order by R.DataOd desc
'

exec sp_executesql @stmt
        "    
                PageSize="12"
                AllowPaging="true"
                OnCommand="cntMies_OnCommand"
            />
            <asp:Button ID="btChangeStatus" runat="server" CssClass="button_postback" Text="Status" onclick="btChangeStatus_Click" />
            <asp:Button ID="btImport" runat="server" CssClass="button_postback" Text="Import" onclick="btImport_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>            
<%--
--%>
<%--
select 
R.DataOd [od:-],
R.DataDo [do:-],
R.Status [status:-],
CONVERT(varchar(7), R.DataDo, 20) as [Miesiąc],
case when R.Status = 1 then 'Zamknięty' else 'Otwarty' end [Status okresu rozliczeniowego],
'019' [Splity:;control|cmd:split @od @status 9999],
'WG LUDZI' [:;control|cmd:split @od @status 10056],
'WG SPACE' [:;control|cmd:split @od @status 10063],
R.DataSplitow [Data splitów:D],
R.DataImportu [Data danych:D],

--case when R.Status = 0 then 'Importuj' else null end [:;control|cmd:import @od @do],

'Podział' [:;control|PodzialLudzi @od @do]
from OkresyRozl R
--where R.DataImportu is not null
order by R.DataOd desc
--%>


    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="paContainer" runat="server">
                <div id="divZoom" style="display:none;" class="modalPopup">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="1" Type="1" NoGroup="1" runat="server" Visible="true"/>
                            <div class="bottom_buttons">
                                <asp:Button ID="btExcel" CssClass="button100" runat="server" Text="Excel" OnClick="btExcel_Click" Visible="false"/>
                                <asp:Button ID="btSave" CssClass="button100" runat="server" Text="Zapisz" OnClick="btSave_Click" />
                                <asp:Button ID="btClose" CssClass="button100" runat="server" Text="Zamknij" OnClick="btClose_Click" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btExcel"/>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>    
</div>