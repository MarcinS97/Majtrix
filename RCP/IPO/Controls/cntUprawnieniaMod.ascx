<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntUprawnieniaMod.ascx.cs"
    Inherits="HRRcp.Controls.IPO.cntUprawnieniaMod" %>



<div id="paUprawnienia" runat="server" class="cntUprawnieniaMod">
    <div id="paFilter" runat="server" class="paFilter tbPracownicy2_filter" visible="true">
        <div class="left">
            <span class="label">Wyszukaj:</span>
            <asp:TextBox ID="tbSearch" CssClass="search textbox" runat="server"></asp:TextBox>
            <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj"
                OnClick="btSearch_Click" />
            <asp:GridView ID="gvUprawnienia" runat="server" CssClass="GridView1" AllowPaging="True"
                AllowSorting="True" AutoGenerateColumns="true" DataSourceID="SqlDataSource1"
                OnDataBound="gvUprawnienia_DataBound">
            </asp:GridView>
            <div class="pager">
                <span class="count">Ilość:<asp:Label ID="lbCount" runat="server"></asp:Label></span>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span class="count">Pokaż na stronie:</span>
                <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" OnChange="showAjaxProgress();"
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
<asp:HiddenField ID="hidTab" runat="server" Visible="false" />
<%--
znaki do braku uprawnień:
—

--%>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:IPO %>"
    OnLoad="SqlDataSource1_Load" OnSelected="SqlDataSource1_Selected" SelectCommand="
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @stmt nvarchar(max)

select 
    @colsH = isnull(@colsH + ',', '') + '[' + x.cc + ']',
    @colsD = isnull(@colsD + ',', '') + 'ISNULL([' + x.cc + '],' +  '''-''' + ') as 
                                                [' + x.cc + '|'  +  'js:setRegionyIPO(this @pid ''' + x.cc + ''');' + '|' + x.Nazwa + ']'  
    from 
    (select 
        distinct(CC.cc),
        CC.Nazwa
        from CC        
    ) AS x
    order by x.cc


select @stmt = '
SELECT 
    Id as [pid:-],
    Region as [Region],
    ' + @colsD + '
FROM
(
	select 
	Reg.Id as Id,
	Reg.Region,
	CC.CC
	from IPO_Regiony Reg
	left outer join IPO_CCRegiony ccr on CCR.IdRegionu = Reg.Id
	left outer join CC on CC.Id = ccr.IdCC
		
) as D
PIVOT
(
	max(D.CC) FOR D.CC IN (' + @colsH + ')
) as PV
order by Region'



exec sp_executesql @stmt
    " OnFiltering="SqlDataSource1_Filtering" OnPreRender="SqlDataSource1_PreRender">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="UserId" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
