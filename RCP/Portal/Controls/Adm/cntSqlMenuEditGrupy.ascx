<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlMenuEditGrupy.ascx.cs" Inherits="HRRcp.Portal.Controls.Adm.cntSqlMenuEditGrupy" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="uc1" TagName="cntSqlTabs" %>
<%@ Register Src="~/Portal/Controls/Adm/cntImportLogo.ascx" TagPrefix="uc1" TagName="cntImportLogo" %>

<div id="paSqlMenuEditGrupy" runat="server" class="cntSqlMenuEditGrupy" >
    <asp:HiddenField ID="hidGrupa" runat="server" Visible="false" />
    <asp:HiddenField ID="hidParentId" runat="server" Visible="false" />

    <uc1:cntImportLogo runat="server" ID="cntImportLogo2" />
     
    <uc1:cntModal runat="server" ID="cntModal" Backdrop="false" Keyboard="false" ShowFooter="true" Width="900px">
        <HeaderTemplate>
            <h4 class="modal-title">
                <i class="fa fa-link"></i>
                <asp:Label ID="lbTitleNazwa" runat="server" Visible="true"></asp:Label>
                <asp:Literal ID="ltTitleEdit" runat="server" Text="- Edycja" Visible="true" />
                <asp:Literal ID="ltTitleInsert" runat="server" Text="Dodaj nowy" Visible="false"/>
            </h4>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row" id="paFilter" runat="server" visible="false">
                <div class="col-md-12">
                    <label>Grupa:</label>
                    <asp:DropDownList ID="ddlGrupa" runat="server" CssClass="form-control" DataSourceID="dsGrupy" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupa_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Button ID="logo" runat="server" OnClick="logo_Click"   Text="Zmiana Logo" CssClass="btn btn-default btnright" />
                    </div>
            </div>
            <div class="row" id="paParent" runat="server" visible="false">
                <div class="col-md-12">
                    <h4>
                        <asp:Label ID="lbParent" runat="server" ></asp:Label>
                    </h4>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="gvSqlMenu" CssClass="table GridView1" runat="server" DataSourceID="dsSqlMenu" AllowSorting="true" DataKeyNames="id:-"
                        OnDataBinding="gvSqlMenu_DataBinding"
                        OnRowDataBound="gvSqlMenu_RowDataBound"
                        >
                    </asp:GridView>
                    <asp:Button ID="gvSqlMenuCmd" runat="server" CssClass="button_postback" Text="Button" onclick="gvSqlMenuCmd_Click" />
                    <asp:HiddenField ID="gvSqlMenuCmdPar" runat="server" />
                    <asp:HiddenField ID="gvSqlMenuSelected" runat="server" />
                    <asp:HiddenField ID="hidSelectedId" runat="server" />
                </div>
            </div>
            <%-- nie widać ControlParameter jak poza ContentTemplate --%>
            <asp:SqlDataSource ID="dsGrupy" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
                SelectCommand="
/*
select null Value, 'wybierz...' Text, 1 Sort
union all                
*/
select distinct Grupa Value ,Grupa Text, 2 Sort 
from SqlMenu 
where Grupa is not null and Grupa != ''
order by Sort, Text
"/>

                        <asp:SqlDataSource ID="dsSqlMenu" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="False" OnSelected="dsSqlMenu_Selected" 
                SelectCommand="
select 
  Id [id:-]
--, null [:click|cmd:select @id|Zaznacz...]
--, cast(0 as bit) [:check|cmd:select @id|]  --'Wybierz'
, 'Wybierz' [:check;control|cmd:select @id|]
, MenuText [Tekst]	
, m1.ButtonText [:;control|cmd:chld @id|Pokaż elementy podrzędne]
, case when Aktywny = 1 then 'TAK' else 'NIE' end [Aktywny:CB;check]
, case when Aktywny = 1 then '' else 'disabled' end [:class]
--, m.Kolejnosc [Kolejnosc:N]
, 'Edytuj' [:;control|cmd:edit @id|]
from SqlMenu m
outer apply (select top 1 'Pozycje' ButtonText from SqlMenu where ParentId = m.Id) m1
where Grupa = @grupa and ISNULL(ParentId, 0) = ISNULL(@parentId, 0)
order by Kolejnosc, MenuText 
">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hidGrupa" PropertyName="Value" Name="grupa" Type="String" />      
                    <asp:ControlParameter ControlID="hidParentId" PropertyName="Value" Name="parentId" Type="String" />      
                    <asp:ControlParameter ControlID="hidSelectedId" PropertyName="Value" Name="selid" Type="String" />      
                    <asp:ControlParameter ControlID="ddlGrupa" PropertyName="SelectedValue" Name="selgrupa" Type="String" />      
                    <asp:ControlParameter ControlID="gvSqlMenuSelected" Name="selected" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>


            <asp:SqlDataSource ID="dsSqlMenu_TREE" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="False" OnSelected="dsSqlMenu_Selected" 
                SelectCommand="
--declare @grupa varchar(50) = 'MAINMENU'

select ROW_NUMBER() over (order by Kolejnosc) Lp
, *
into #mmm 
from SqlMenu 
where Grupa = @grupa

;with SubTree as
(
select 
	m.*
  , 0 AS HLevel
  , CAST(CAST(m.Lp AS BINARY(4)) AS VARBINARY(8000)) AS SortPath
from #mmm m
where m.ParentId is null or m.ParentId = 0

union all

select 
	n.*
  , st.HLevel + 1 as HLevel
  , CAST(st.SortPath + CAST(n.Lp AS BINARY(4)) AS VARBINARY(8000)) AS SortPath
FROM #mmm n
inner join SubTree st ON st.Id = n.ParentId
)
----- select -----
select 
  Id [id:-]
--, null [:click|cmd:select @id|Zaznacz...]
--, cast(0 as bit) [:check|cmd:select @id|]  --'Wybierz'
, 'Wybierz' [:check;control|cmd:select @id|]
, REPLICATE('&nbsp;', HLevel * 4) + MenuText [Text]	
, case when Aktywny = 1 then 'TAK' else 'NIE' end [Aktywny:CB;check]
, case when Aktywny = 1 then '' else 'disabled' end [:class]
, m.Kolejnosc [Kolejnosc:N]
, 'Edytuj' [:;control|cmd:edit @id|]
from SubTree m
order by SortPath
------------------
drop table #mmm
">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hidGrupa" PropertyName="Value" Name="grupa" Type="String" />      
                    <asp:ControlParameter ControlID="hidSelectedId" PropertyName="Value" Name="selid" Type="String" />      
                    <asp:ControlParameter ControlID="ddlGrupa" PropertyName="SelectedValue" Name="selgrupa" Type="String" />      
                    <asp:ControlParameter ControlID="gvSqlMenuSelected" Name="selected" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btAdd" runat="server" Text="Dodaj nowy" CssClass="btn btn-success pull-left" OnClick="btAdd_Click" Visible="true"/>
            <asp:Button ID="btAddSub" runat="server" Text="Dodaj poniżej" CssClass="btn btn-success pull-left" OnClick="btAddSub_Click" Visible="true"/>
            <asp:LinkButton ID="lbtAdd" runat="server" Text="Dodaj" OnClick="btAdd_Click" class="btn btn-primary btn-small" Visible="false" >
                <i class="fa fa-plus"></i>
            </asp:LinkButton>
            <asp:LinkButton ID="lbtUp" runat="server" Text="W górę" OnClick="lbtUp_Click" class="btn btn-primary btn-small pull-left" >
                <i class="fa fa-caret-up"></i>
            </asp:LinkButton>
             <asp:LinkButton ID="lbtDown" runat="server" Text="W dół" OnClick="lbtDown_Click" class="btn btn-primary btn-small pull-left" >
                <i class="fa fa-caret-down"></i>
            </asp:LinkButton>

<%--            <asp:Button ID="btDelete" runat="server" Text="Usuń" CssClass="btn btn-danger pull-left" OnClick="btDelete_Click" Visible="false"/>
            <asp:Button ID="btSave" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClick="btSave_Click" ValidationGroup="vgSave" Visible="false"/>--%>
        </FooterTemplate>
    </uc1:cntModal>

    <%--
    <uc1:cntModal runat="server" ID="cntShowError" Backdrop="false" Keyboard="false" ShowFooter="true" CloseButtonText="Ok" Title="Wystapił błąd" >
        <ContentTemplate>
            <asp:literal id="ltShowErrorMessage" runat="server" ></asp:literal>                                
        </ContentTemplate>
    </uc1:cntModal>
    --%>

</div>

<%--

--%>