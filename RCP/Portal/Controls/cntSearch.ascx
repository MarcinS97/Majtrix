<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSearch.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSearch" %>

<style>
    .cntSearch .title{
        font-size: 24px;
        display: inline-block;
    }

    .cntSearch div.text{
        font-size: 18px;
        xdisplay: inline-block;
    }

    .cntSearch i{
        font-size: 24px;
        display: inline-block;
        margin-right: 12px;
    }

    .cntSearch table td {
        padding-bottom: 24px;
    }

    .cntSearch .ico{
    }
    
</style>
<%--visible='< % # IsVisible(Eval("Image")) %>'    GetPath(Eval("Image")) --%>
<div id="paSearch" runat="server" class="cntSearch">
    <asp:HiddenField ID="hidSearch" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidLogId" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidPrac" runat="server" Visible="false"/>
    
    <asp:ListView ID="lvSearch" runat="server" DataSourceID="SqlDataSource1" 
        ondatabound="lvSearch_DataBound" 
        onitemcommand="lvSearch_ItemCommand" 
        onitemdatabound="lvSearch_ItemDataBound" >
        <ItemTemplate>
            <tr class='<%# Eval("css") %>' >
                <td>
                    <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>'/>
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("CmdPar") %>' CommandName='<%# Eval("Cmd") %>' ToolTip='<%# Eval("ToolTip") %>'>
                        <%--<div id="Div1" class="ico" runat="server" >    
                            <i class='<%# GetIco((string)Eval("Image")) %>'></i>
                        </div>--%>
                        <div class="text"> 
                            <i class='<%# GetIco(Eval("Image")) %>'></i>
                            <asp:Label ID="Label2" runat="server" CssClass="title"  Text='<%# Eval("Title") %>' ></asp:Label><br />
                            <asp:Label ID="Label1" runat="server" CssClass="text" Text='<%# Eval("Text") %>' ></asp:Label>
                        </div>            
                    </asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        Brak wyników spełniających kryteria
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="Table2" runat="server" class="tbSearch">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server" colspan="2" class="content" >
                        <table ID="itemPlaceholderContainer" runat="server" class="table0">
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="divider">
                    <td colspan="2"></td>
                </tr>
                <tr class="pager">
                    <td>
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="10">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                <asp:NumericPagerField ButtonType="Link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                    <td class="right">
                        <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <span class="count">Pokaż na stronie:</span>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlLines" runat="server" >
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
        
        SelectCommand="
-- artykuly --
select 2 as Sort, 'artykuly' as css, 'article' as Cmd, M.Command as CmdPar,
A.Id, '../../images/fileext/dok.png' as Image,
dbo.fn_Join(ISNULL(M.MenuText, 'Aktualności'), LEFT(A.Tekst2, 30) + '...', null, ' - ') as Title,   
'...' + SUBSTRING(A.Tekst2, CHARINDEX(@search, A.Tekst2, 1) - 20, 180) + '...' as Text, 
null as ToolTip
from Teksty A
left join SqlMenu M on M.Par1 = A.Grupa
where A.Grupa is not null and GETDATE() &gt; A.DataPublikacji and A.Widoczny = 1 
and A.Tekst2 like '%' + @search + '%'

union all
-- pliki --
select 1 as Sort, 'pliki' as css, 'download' as Cmd, ISNULL(P.Command, ISNULL(PP.Command, PM.Command)) as CmdPar,
case when P.Command is null then case when PP.Command is null then PM.Id else PP.Id end else P.Id end as Id, 
P.Image, 
dbo.fn_Join(case ISNULL(PM.Grupa, ISNULL(PP.Grupa, P.Grupa)) 
	when 'PRAC' then 'Panel Pracownika' 
	when 'KIER' then 'Panel Kierownika'
else ''
end, PM.MenuText, PP.MenuText, ' - ') as Title, P.MenuText as Text, P.ToolTip

from SqlMenu P
left join SqlMenu as PP on PP.Id = P.ParentId
left join SqlMenu as PM on PM.Par1 = ISNULL(PP.Grupa, P.Grupa)
where P.Aktywny=1 and P.MenuText like '%' + @search + '%'
and (@prac = 0 or ISNULL(PM.Grupa, ISNULL(PP.Grupa, P.Grupa)) in 
(
 'CAROUSEL'
,'FILEFORUM'
,'FILEOCENA'
,'FILEP'
,'FILEPKZP'
,'GAZETKA'
,'PRAC'
)
)

order by Sort, Text
        ">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidSearch" Name="search" PropertyName="Value" DbType="String" />
            <asp:ControlParameter ControlID="hidPrac" Name="prac" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>




<%--
                    <div class="ico" runat="server" visible='<%# IsVisible(Eval("Image")) %>'>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                    </div>
                    <div class="link">
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("CmdPar") %>' CommandName='<%# Eval("Cmd") %>' ToolTip='<%# Eval("ToolTip") %>'>
                            <asp:Label ID="Label2" runat="server" CssClass="title" Text='<%# Eval("Title") %>' ></asp:Label>
                            <asp:Label ID="Label1" runat="server" CssClass="text" Text='<%# Eval("Text") %>' ></asp:Label>
                        </asp:LinkButton>
                    </div>            
--%>