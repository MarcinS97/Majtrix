<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKierSelect.ascx.cs" Inherits="HRRcp.SzkoleniaBHP.Controls.cntKierSelect" %>

<div id="paKierSelect" runat="server" class="cntKierSelect">
    <asp:Label ID="Label1" runat="server" Text="Kierownik:"></asp:Label>
    
    
<%--    <asp:DropDownList ID="ddlKier" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Nazwisko" DataValueField="Id" >
    </asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="SELECT [Id], [Nazwisko] FROM [Pracownicy]" 
        onselected="SqlDataSource1_Selected">
    </asp:SqlDataSource>
--%>    
    
    
    
    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hidLoginTyp" runat="server" Visible="false" Value="0"/>
            <asp:HiddenField ID="hidRootId" runat="server" Visible="false" />
            <asp:DropDownList ID="ddlKier" runat="server" DataSourceID="SqlDataSource1" AutoPostBack="true" DataTextField="Value" DataValueField="Id" 
                OnSelectedIndexChanged="ddlKier_SelectedIndexChanged" 
                OnDataBound="ddlKier_DataBound" >
            </asp:DropDownList>
            <asp:CheckBox ID="cbShowSub" runat="server" 
                Text="Pokaż pracowników z podstruktury" AutoPostBack="True" 
                oncheckedchanged="cbShowSub_CheckedChanged" />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                SelectCommand="
--declare @Typ int
--set @Typ = -1

declare @kierId int
set @kierId = 0
declare @Data datetime
set @Data = GETDATE()
-----
select 'Wszyscy kierownicy' as Value, null as Id, null as SortPath
union all
select 'pokaż listę ...' as Value, '0' as Id, null as SortPath from (select 1 X) X where @Typ = -1 
union all    
select 'pokaż podległości ...' as Value, '-1' as Id, null as SortPath from (select 1 X) X where @Typ = 0 
union all    

select Pracownik as Value, 
convert(varchar, IdPracownika) + '|' + case when Rola = '' then 'poza' when TL = 1 then 'acc' else '' end as Id,   -- css
SortPath
from 
(
select * from
(
select 
REPLICATE('&nbsp;', Hlevel * -4 * @Typ) +
S.Nazwisko + ' ' + S.Imie + ISNULL(' (' + S.KadryId + ')','') as Pracownik,
S.IdPracownika, S.SortPath
from dbo.fn_GetTree2(@kierId, 1, @Data) S
where S.Kierownik = 1
) D1
where @Typ = -1 
) D
order by SortPath
               ">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidLoginTyp" Name="Typ" PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="hidRootId" Name="rootId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
</div>