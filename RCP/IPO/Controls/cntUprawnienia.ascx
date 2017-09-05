<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntUprawnienia.ascx.cs" Inherits="HRRcp.IPO.Controls.cntUprawnienia" %>


<script type="text/javascript">
// Funkcja, która wywołuje WebMetode 
function setPrawaIPO(link, ifDataBind, typ, pid, par, rola) {
    showAjaxProgress();
    //var svcUrl = getRootUrl() + '/main.asmx';

    callAjax2(svcUrl, 'UpdateRightIPO', { typ: typ, pid: pid, par: par, rola: rola, currvalue: link.innerHTML, aggCC: ifDataBind  },
        function(r) {
            hideAjaxProgress();
            if (r.d) {

                link.innerHTML = r.d;

            }
            if (ifDataBind == 1) {
                __doPostBack("<%= gvUprawnienia.ClientID %>", "");
            }
        }
    );    
};

function setRegionyIPO(link, pid, par) {

        showAjaxProgress();
        //var svcUrl = getRootUrl() + '/main.asmx';

        callAjax2(svcUrl, 'UpdateRegionyIPO', { pid: pid, par: par, currvalue: link.innerHTML },
        function(r) {
            hideAjaxProgress();
            if (r.d) {

                link.innerHTML = r.d;

            }
        }
    );
    }
</script>

<div id="paUprawnienia" runat="server" class="cntUprawnienia">
    <div id="paFilter" runat="server" class="paFilter tbPracownicy2_filter" visible="true">
        <div class="left">
            <span class="label">Wyszukaj:</span>
            <asp:TextBox ID="tbSearch" CssClass="search textbox" runat="server" ></asp:TextBox>
            <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" />
        </div>
        <div>
             <asp:CheckBox id="cbAll" AutoPostBack="True" Text="Pokaż tylko pracowników mających <br> już nadane uprawnienia" Checked="False" OnCheckedChanged="ch_CheckedChanged" runat="server"/>
        </div>
     </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />
            <asp:GridView ID="gvUprawnienia" runat="server" CssClass="GridView1" 
                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="true" 
                DataKeyNames="nrew:-" DataSourceID="SqlDataSource1" 
                ondatabound="gvUprawnienia_DataBound" OnPageIndexChanged="onInd">
            </asp:GridView>
            <div class="pager">
                <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <span class="count">Pokaż na stronie:</span>
                <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true"    
                    OnChange="showAjaxProgress();"
                    OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                    <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                    <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                </asp:DropDownList>
            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<%--
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
--%>

<asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
<asp:HiddenField ID="hidUserAdm" runat="server" Visible="false" />
<asp:HiddenField ID="hidRola" runat="server" Visible="false" />
<asp:HiddenField ID="hidTab" runat="server" Visible="false" />

<%--
znaki do braku uprawnień:
—

--%>

      
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>" 
    onload="SqlDataSource1_Load" 
    onselected="SqlDataSource1_Selected"    
    SelectCommand="
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @rol nvarchar(max),
    @stmt nvarchar(max)
    


select 
    @rol =  Id
  from IPO_Role
 WHERE Nazwa = @Rola
 
 
if ((select dbo.GetRightId(Rights, 32) from Pracownicy where Id = @UserId) = 1 
OR (select IdCC from IPO_ccPrawa where IdCC =0 and RolaId=5 and UserId = @UserId) = 0) 
begin
select  
    @colsH = isnull(@colsH + ',', '') + '[' + IPO_Regiony.Region + ']',
    @colsD = isnull(@colsD + ',', '') + 'ISNULL([' + IPO_Regiony.Region + '],' +  '''-''' + ') as 
                                                [' + IPO_Regiony.Region + '|'  +  'js:setPrawaIPO(this 1 3 @pid ''' + IPO_Regiony.Region + ''' '+@rol+');' + '|' + IPO_Regiony.Region + ']'  
    from IPO_Regiony        
end

if (select 'Moderator') != @Rola
BEGIN
select 
    @colsH = isnull(@colsH + ',', '') + '[' + x.cc + ']',
    @colsD = isnull(@colsD + ',', '') + 'ISNULL([' + x.cc + '],' +  '''-''' + ') as 
                                                [' + x.cc + '|'  +  'js:setPrawaIPO(this 0 3 @pid ''' + x.cc + ''' '+@rol+');' + '|' + x.Nazwa + ']'  
    from 
    (select 
        distinct(CC.cc),
        CC.Nazwa
        from CC 
        left join IPO_ccPrawa R on R.UserId = @UserId and R.IdCC = CC.Id
	    left join Pracownicy P on P.Id = @UserId
	    left join IPO_Role rol on rol.Id=R.RolaId
 	    where  
 	    dbo.GetRightId(Rights, 32) = '1'
 	    OR (R.UserId =@UserId and rol.Id = 5)
	    OR (select IdCC from IPO_ccPrawa where IdCC =0 and RolaId=5 and UserId = @UserId) = 0
        
    ) AS x
    order by x.cc
END

if (@PokazWszyscy = 'False')
begin
select @stmt = '
SELECT 
    Id as [pid:-],
    KadryId as [nrew:-], 
    Nazwisko as [nn:-],
    Imie as [ii:-],
    KadryId as [Nr ew.], 
    Pracownik as [Pracownik],
    ' + @colsD + '
    ,Pracownik as [Pracownik&nbsp;]
FROM
(
	select 
	P.Id,
	P.KadryId, 
	P.Nazwisko + '' '' + P.Imie + case when P.Admin = 1 then '' (ADM)'' else '''' end as Pracownik,
	P.Nazwisko,
	P.Imie,
    P.Kierownik,
	P.Rights,
	R.CC
	from Pracownicy P
	left outer join IPO_ccPrawa R on R.UserId = P.Id and R.RolaId= ' + @rol + '
	left outer join CC on CC.cc = R.CC
	where P.Status != -1 
  --  and P.KadryId &lt; 80000
   
		
) as D
PIVOT
(
	max(D.CC) FOR D.CC IN (' + @colsH + ')
) as PV
order by Pracownik'
end
ELSE
select @stmt = '
SELECT 
    Id as [pid:-],
    KadryId as [nrew:-], 
    Nazwisko as [nn:-],
    Imie as [ii:-],
    KadryId as [Nr ew.], 
    Pracownik as [Pracownik],
    ' + @colsD + '
    ,Pracownik as [Pracownik&nbsp;]
FROM
(
	select 
	P.Id,
	P.KadryId, 
	P.Nazwisko + '' '' + P.Imie + case when P.Admin = 1 then '' (ADM)'' else '''' end as Pracownik,
	P.Nazwisko,
	P.Imie,
    P.Kierownik,
	P.Rights,
	R.CC
	from Pracownicy P
	left outer join IPO_ccPrawa R on R.UserId = P.Id 
	left outer join CC on CC.cc = R.CC
	where P.Status != -1 
--	and P.KadryId &lt; 80000
    and R.RolaId= ' + @rol + '
		
) as D
PIVOT
(
	max(D.CC) FOR D.CC IN (' + @colsH + ')
) as PV
order by Pracownik'



exec sp_executesql @stmt
    " onfiltering="SqlDataSource1_Filtering" 
    onprerender="SqlDataSource1_PreRender">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="UserId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidRola" Name="Rola" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="cbAll" Name="PokazWszyscy" propertyname="Checked" type="String"  />
    </SelectParameters>
</asp:SqlDataSource>