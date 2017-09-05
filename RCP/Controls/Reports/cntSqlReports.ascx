<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlReports.ascx.cs" Inherits="HRRcp.Controls.Reports.cntSqlReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
<asp:HiddenField ID="hidRights" runat="server" Visible="false"/>
<asp:HiddenField ID="hidMode" runat="server" Visible="false"/>
<asp:HiddenField ID="hidKatCommand" runat="server" />
<asp:Button ID="btKatSelect" runat="server" CssClass="button_postback" Text="Select" OnClick="btKatSelect_Click"/>

<div id="paSqlReports" runat="server" class="cntSqlReports">
    <asp:Label ID="lbInfo" runat="server" ></asp:Label>
    <ajaxToolkit:Accordion ID="acordionMenu" runat="server" DataSourceID="SqlDataSource3"
        CssClass="mnKategorie"
        ContentCssClass="mnKategorieContent"
        HeaderSelectedCssClass="mnKategoriaSelected"
        OnItemDataBound="acordionMenu_ItemDataBound" 
        OnItemCommand="acordionMenu_ItemCommand" 
        OnDataBinding="acordionMenu_DataBinding" 
        OnPreRender="acordionMenu_PreRender" >    
        <HeaderTemplate>
            <%--
            <asp:LinkButton ID="lbtKategoria" CssClass="button_postback" runat="server" Text="select" CommandName="Select" CommandArgument='<%# Eval("Command") %>' />        
            <asp:Label ID="lbCaption" runat="server" Text='<%# Eval("MenuText")%>' ></asp:Label>
            --%>

            <asp:LinkButton ID="lbtKategoria" CssClass="button" runat="server" CommandName="Select" CommandArgument='<%# Eval("Command") %>' OnClientClick="showAjaxProgress();" >
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/reports/group_go.png"/>
                <div>
                    <b><asp:Literal ID="Literal3" runat="server" Text='<%# Eval("MenuText")%>' ></asp:Literal></b><br />
                    <asp:Literal ID="Literal4" runat="server" Text='<%# Eval("ToolTip")%>' ></asp:Literal>                
                </div>
            </asp:LinkButton>
        </HeaderTemplate>
        <ContentTemplate>
            <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' Visible="false"/>
            <asp:HiddenField ID="hidCmd" runat="server" Value='<%# Eval("Command") %>' Visible="false"/>
            <asp:HiddenField ID="hidPar1" runat="server" Value='<%# Eval("Par1") %>' Visible="false"/>
            <asp:ListView ID="lvPodKategorie" runat="server" DataKeyNames="Id"  
                EnableModelValidation="True"
                OnItemCommand="lvPodKategorie_ItemCommand"
                onitemdatabound="lvPodKategorie_ItemDataBound"
                onselectedindexchanged="lvPodKategorie_SelectedIndexChanged">
                <ItemTemplate>
                    <tr id="Tr1" class="it">
                        <td>
                            <asp:LinkButton ID="lbtPodKategoria" runat="server" Text='<%# Eval("MenuText") %>' CommandName="Select" CommandArgument='<%# Eval("Command") %>'/>
                        </td>
                    </tr>
                </ItemTemplate>

                <SelectedItemTemplate>
                    <tr class="sit">
                        <td>
                            <asp:LinkButton ID="lbtPodKategoria" runat="server" Text='<%# Eval("MenuText") %>' CommandName="Deselect" CommandArgument='<%# Eval("Command") %>'/>
                        </td>
                    </tr>
                </SelectedItemTemplate>

                <EmptyDataTemplate>
                </EmptyDataTemplate>
                
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" border="0" class="mnPodKategorie table0">
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
SELECT 
case when @lng = 'PL' then MenuText else MenuTextEN end as MenuText, 
case when @lng = 'PL' then ToolTip else ToolTipEN end as ToolTip, 
Id, Grupa, ParentId, Command, Kolejnosc, Aktywny, Image, Rights
FROM SqlMenu 
WHERE Grupa = @Grupa and ParentId = @ParentId and Aktywny = 1 and (Rights is null or dbo.CheckRightsExpr(@Rights, Rights) = 1) and (Mode is null or Mode & @Mode != 0)
ORDER BY Kolejnosc, MenuText
                ">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1" Name="ParentId" Type="Int32" />
                    <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" Type="string" />
                    <asp:ControlParameter ControlID="hidRights" Name="Rights" Type="string" />
                    <asp:ControlParameter ControlID="hidMode" Name="Mode" Type="Int32" />
                    <asp:SessionParameter DefaultValue="PL" Name="lng" SessionField="LNG" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            
        </ContentTemplate>
    </ajaxToolkit:Accordion>
</div>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    onselected="SqlDataSource3_Selected"
    SelectCommand="
SELECT
case when @lng = 'PL' then MenuText else MenuTextEN end as MenuText, 
case when @lng = 'PL' then ToolTip else ToolTipEN end as ToolTip, 
Id, Grupa, ParentId, Command, Kolejnosc, Aktywny, Image, Rights, Par1, Par2
FROM SqlMenu 
where Grupa = @Grupa and (ParentId is null or ParentId = 0) and Aktywny = 1 and (Rights is null or dbo.CheckRightsExpr(@Rights, Rights) = 1) and (Mode is null or Mode & @Mode != 0)
order by Kolejnosc, MenuText
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" Type="string" />
        <asp:ControlParameter ControlID="hidRights" Name="Rights" Type="string" />
        <asp:ControlParameter ControlID="hidMode" Name="Mode" Type="Int32" />
        <asp:SessionParameter DefaultValue="PL" Name="lng" SessionField="LNG" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
SELECT
case when @lng = 'PL' then MenuText else MenuTextEN end as MenuText, 
case when @lng = 'PL' then ToolTip else ToolTipEN end as ToolTip, 
Id, Grupa, ParentId, Command, Kolejnosc, Aktywny, Image, Rights, Par1, Par2
FROM SqlMenu 
where Grupa = @Grupa and (ParentId is null or ParentId = 0) and Aktywny = 1 and (Rights is null or dbo.CheckRights(@Rights, Rights) = 1) and (Mode is null or Mode & @Mode != 0)
order by Kolejnosc, MenuText">


--declare @lng varchar(10) = 'PL'
--declare @Grupa varchar(100) = 'REPORT'
--declare @Rights varchar(100) = ''
--declare @Mode int = 3
declare @stmt nvarchar(max)
set @stmt = '
declare 
	@lng varchar(30),
	@Grupa nvarchar(100),
	@Rights varchar(max),
	@Mode int
set @lng = ''' + @lng + ''' 
set @Grupa = ''' + @Grupa + ''' 
set @Rights = ''' + @Rights + ''' 
set @Mode = ' + convert(varchar,@Mode) + ' 
SELECT
case when @lng = ''PL'' then MenuText else MenuTextEN end as MenuText, 
case when @lng = ''PL'' then ToolTip else ToolTipEN end as ToolTip, 
Id, Grupa, ParentId, Command, Kolejnosc, Aktywny, Image, Rights, Par1, Par2
FROM SqlMenu 
--where Grupa = @Grupa and (ParentId is null or ParentId = 0) and Aktywny = 1 and (Rights is null or ' + dbo.CheckRightsExpr('@Rights', Rights)  + ') and (Mode is null or Mode & @Mode != 0)
where Grupa = @Grupa and (ParentId is null or ParentId = 0) and Aktywny = 1 and (Rights is null or dbo.CheckRights(@Rights, Rights) = 1) and (Mode is null or Mode & @Mode != 0)
order by Kolejnosc, MenuText
    '
exec sp_executesql @stmt

--%>



