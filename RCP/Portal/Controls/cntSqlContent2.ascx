<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlContent2.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSqlContent2" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntDetails.ascx" tagname="cntDetails" tagprefix="uc2" %>
<%@ Register src="cntSqlEdit.ascx" tagname="cntSqlEdit" tagprefix="uc3" %>

<div id="paSqlContent" runat="server" class="cntSqlContent">
    <asp:Menu ID="tabContent" runat="server" Orientation="Horizontal"   onmenuitemclick="tabContent_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
    </asp:Menu>
    <div class="tabsContent">
        <div id="paEdit" runat="server" class="edit xpull-right" visible="false" style="margin-bottom: 64px;">
            <asp:Button ID="btEdit" runat="server" CssClass="button100 btn btn-primary pull-right" Text="Edycja" onclick="btEdit_Click" />            
            <uc3:cntSqlEdit ID="cntSqlEdit1" runat="server" OnClose="cntSqlEdit1_Close" Visible="false"/>            
        </div>    
        <div id="paEdit2" runat="server" class="edit2" visible="true">
            <asp:HiddenField ID="hidTabId" runat="server" Visible="false"/>
            <asp:HiddenField ID="hidTabType" runat="server" Visible="false"/>
            <asp:Button ID="btWniosek" runat="server" Text="Wniosek o zmianę danych" onclick="btWniosek_Click" />
        </div>
        <uc1:cntReport ID="cntMasterLines" runat="server" Visible="false"
            AllowPaging="true"
            PageSize="20"
            AllowQueryString="false"
            GridCssClass="GridView1 table table-striped table-bordered"
        />
        <uc2:cntDetails ID="cntMasterScreen" runat="server" Visible="true"
            AllowQueryString="false"
        />
        <div id="paButtons" runat="server" class="bottom_buttons" visible="false">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btExcel" runat="server" Text="Eksport Excel" CssClass="button" onclick="btExcel_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btExcel"/>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>

<asp:SqlDataSource ID="dsTabs" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
    SelectCommand="
declare @grupa varchar(200)
declare @rights varchar(max)
set @grupa = '{1}'
set @rights = '{2}'

select * from {0}..SqlContent 
where Grupa = @grupa and Aktywny = 1 
and (Rights is null or dbo.CheckRightsExpr(isnull(@rights, ''), Rights) = 1)    
order by Kolejnosc, MenuText
    "/>

<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        tabContent.MenuItemClick += new MenuEventHandler(tabContent_MenuItemClick2);
    }

    protected void tabContent_MenuItemClick2(object sender, MenuEventArgs e)
    {
        this.paButtons.Visible = tabContent.SelectedValue == "26"   //Absencje
                              || tabContent.SelectedValue == "27"  //Absencje - statystyka
                              || tabContent.SelectedValue == "28"  //Chorobowe miesięcznie
                              || tabContent.SelectedValue == "29";  //Absencje w roku
        this.paButtons.Visible = true;
    }

    protected void btExcel_Click(object sender, EventArgs ea)
    {
        string tab = tabContent.SelectedItem.Text;
        tab = Regex.Replace(tab, @"<(.|\n)*?>", string.Empty);        
        string filename = tab.Replace(" ", "_");
        this.cntMasterLines.ExportCSV(filename, false);
        //ExportCSV(filename, cntMasterLines.DataSourceSql, null, null, true, false);
    }
</script>
