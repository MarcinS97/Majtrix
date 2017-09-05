<%@ Page Language="C#" 
    AutoEventWireup="true" 
    Async="true" 
    AsyncTimeout="90"
    ValidateRequest="false"
    CodeBehind="test.aspx.cs" Inherits="HRRcp.test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/SqlControl.ascx" tagname="SqlControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc2" %>
<%@ Register src="~/Controls/ImportRogerCSV2.ascx" tagname="ImportRogerCSV2" tagprefix="uc3" %>
<%@ Register src="~/Controls/AssecoRCP.ascx" tagname="AssecoRCP" tagprefix="uc4" %>
<%@ Register src="~/Controls/TimeEdit2.ascx" tagname="TimeEdit2" tagprefix="uc5" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<style type="text/css">
    table    
    {
        border-collapse: separate;   border-spacing: 0px; }  
    table td, table th      
    {
        background-color: transparent;     
        overflow: hidden;     
        z-index: 1;     
        border-right: 0;     
        border-bottom: 0; }  
    table th:before, table td:before 
    {     
        content: "";     
        padding: 0;     
        height: 1px;     
        line-height: 1px;     
        width: 1px;     
        margin: -4px -994px -996px -6px;     
        display: block;     
        border: 0;     
        z-index: -1;     
        position:relative;     
        top: -500px; }  
    table th:before      
    {
        border-top: 999px solid #c9c9c9;     
        border-left: 999px solid #c9c9c9; 
    }  
    table td:before      
    {
        border-top: 999px solid #eeeeee;     
        border-left: 999px solid #eeeeee; 
    } 
</style>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <META HTTP-EQUIV="X-UA-COMPATIBLE" CONTENT="IE=EmulateIE7" />
    <title></title>
    <link href="~/styles/master.css" rel="stylesheet" type="text/css" />
    <link href="~/styles/Controls.css" rel="stylesheet" type="text/css" />

    <link href="styles/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.7.1/jquery.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="~/../scripts/common.js"></script>
    
    <script type="text/javascript">
        function UpdateTime() {
            PageMethods.GetCurrentDate("asdf", GetCurrentDate_Succeeded, GetCurrentDate_Failed); 
        }

        function GetCurrentDate_Succeeded(result, userContext, methodName) {
            $get('Label1').innerHTML = result; 
        }

        function GetCurrentDate_Failed(error, userContext, methodName) {
            $get('Label1').innerHTML = "An error occured.";
        }

        function btclick(bt) {
            alert(1);
            alert(bt.id); 
        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>

    <div style="padding: 8px;" >
        <%--
        <asp:Button ID="Button14" runat="server" Text="test on click" /><br /><br />
        --%>
        Zaloguj jako:<br />
        <asp:DropDownList ID="ddlLogins" runat="server" onselectedindexchanged="ddlUsers_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
        <br />
        <br />
        <asp:LinkButton ID="LinkButton5" runat="server" onclick="LinkButton5_Click">START</asp:LinkButton><br />
        <%--
        <asp:HyperLink ID="HyperLink0" runat="server" NavigateUrl="Default.aspx" Text="START"></asp:HyperLink><br />
        --%>
        <br />
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="AdminForm.aspx" Text="AdminForm"></asp:HyperLink><br /> 
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="UstawieniaForm.aspx" Text="UstawieniaForm"></asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="KierownikForm.aspx" Text="KierownikForm"></asp:HyperLink><br /> 
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="WynikiForm.aspx" Text="WynikiForm"></asp:HyperLink><br /> 
        <br />        
        <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="~/Login.aspx" >LoginForm</asp:LinkButton><br />
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">ErrorForm</asp:LinkButton><br />
        <br />
        <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="~/Service.aspx" >Service</asp:LinkButton><br />
        <asp:LinkButton ID="LinkButton7" runat="server" PostBackUrl="~/Service.aspx?mode=AUTOID" >Service - import danych</asp:LinkButton>&nbsp;&nbsp;&nbsp;z bazy RCP<br />
        <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/Service.aspx?mode=SCHEDULER" >Scheduler</asp:LinkButton>&nbsp;&nbsp;&nbsp;Uruchomienie konkretnego tasku: Service.aspx?mode=SCHEDULER&amp;p=1
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <uc5:TimeEdit2 ID="TimeEdit21" runat="server" InLineCount="12" Right="true" Interval="5" />
        <br />
        <hr />
        <asp:Button ID="Button10" runat="server" Text="Test msg" onclick="Button10_Click" />
        <asp:Button ID="Button11" runat="server" Text="Test msg ToScript" onclick="Button11_Click" />
        
        <asp:Button ID="Button12" runat="server" Text="Web Method Test" OnClientClick="UpdateTime();return false;" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        
        <hr />
        <asp:Button ID="Button21" runat="server" Text="Import RCP" onclick="Button21_Click" />
        <asp:Button ID="Button2" runat="server" Text="Import Readers" onclick="Button2_Click" />
        <asp:Button ID="Button1" runat="server" Text="Import KP" onclick="Button1_Click" />
        <asp:Button ID="Button3" runat="server" Text="Import KP 2" onclick="Button3_Click" />
        <asp:Button ID="Button9" runat="server" Text="Import KP Nick,Pass" onclick="Button9_Click" />
        <asp:Button ID="Button4" runat="server" Text="Update pracownicy RcpId z KP" onclick="Button4_Click" />
        <asp:Button ID="Button5" runat="server" Text="Update pracownicy RcpId z ROGER" onclick="Button5_Click" />
        <hr />
        <uc2:SelectOkres ID="cntSelectOkres" runat="server" /><br />
        <asp:DropDownList ID="ddlKierownicy" runat="server" /><br />       
        <asp:Button ID="Button7" runat="server" Text="Czy można zamknąć ?" onclick="Button7_Click" />
        <asp:Button ID="Button6" runat="server" Text="Analizuj zatrzaśnięte wartości" onclick="Button6_Click" />
        <asp:Button ID="Button8" runat="server" Text="Zamknij bez sprawdzania" onclick="Button8_Click" />
        <br />
        <br />

        <asp:Button ID="sattest" runat="server" Text="saturn test" OnClick="sattest_Click" />

        <hr />
        SQL<br />
        <uc1:SqlControl ID="SqlControl1" runat="server" />
        <hr />
        <uc3:ImportRogerCSV2 ID="ImportRogerCSV2" runat="server" />

        <%--
        <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px" 
            AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" 
            AutoGenerateInsertButton="True" Caption="aaaaa" DataKeyNames="Id" 
            DataSourceID="SqlDataSource1">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                    ReadOnly="True" SortExpression="Id" />
                <asp:BoundField DataField="IdPracownika" HeaderText="IdPracownika" 
                    SortExpression="IdPracownika" />
                <asp:BoundField DataField="Data" HeaderText="Data" SortExpression="Data" />
                <asp:BoundField DataField="CzasIn" HeaderText="CzasIn" 
                    SortExpression="CzasIn" />
                <asp:BoundField DataField="CzasOut" HeaderText="CzasOut" 
                    SortExpression="CzasOut" />
                <asp:BoundField DataField="Czas" HeaderText="Czas" SortExpression="Czas" />
                <asp:CheckBoxField DataField="Acc" HeaderText="Acc" SortExpression="Acc" />
                <asp:BoundField DataField="Uwagi" HeaderText="Uwagi" SortExpression="Uwagi" />
                <asp:BoundField DataField="Absencja" HeaderText="Absencja" 
                    SortExpression="Absencja" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                    ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>




        <asp:SqlDataSource ID="SqlDataSource1" runat="server"  
            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            DeleteCommand="DELETE FROM [Akceptacja] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Akceptacja] ([IdPracownika], [Data], [CzasIn], [CzasOut], [Czas], [Acc], [Uwagi], [Absencja]) VALUES (@IdPracownika, @Data, @CzasIn, @CzasOut, @Czas, @Acc, @Uwagi, @Absencja)" 
            SelectCommand="SELECT * FROM [Akceptacja]" 
            UpdateCommand="UPDATE [Akceptacja] SET [IdPracownika] = @IdPracownika, [Data] = @Data, [CzasIn] = @CzasIn, [CzasOut] = @CzasOut, [Czas] = @Czas, [Acc] = @Acc, [Uwagi] = @Uwagi, [Absencja] = @Absencja WHERE [Id] = @Id">
            <SelectParameters>
                <asp:Parameter DefaultValue="150" Name="IdPracownika" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="IdPracownika" Type="Int32" />
                <asp:Parameter Name="Data" Type="DateTime" />
                <asp:Parameter Name="CzasIn" Type="DateTime" />
                <asp:Parameter Name="CzasOut" Type="DateTime" />
                <asp:Parameter Name="Czas" Type="DateTime" />
                <asp:Parameter Name="Acc" Type="Boolean" />
                <asp:Parameter Name="Uwagi" Type="String" />
                <asp:Parameter Name="Absencja" Type="Int32" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="IdPracownika" Type="Int32" />
                <asp:Parameter Name="Data" Type="DateTime" />
                <asp:Parameter Name="CzasIn" Type="DateTime" />
                <asp:Parameter Name="CzasOut" Type="DateTime" />
                <asp:Parameter Name="Czas" Type="DateTime" />
                <asp:Parameter Name="Acc" Type="Boolean" />
                <asp:Parameter Name="Uwagi" Type="String" />
                <asp:Parameter Name="Absencja" Type="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>




        <asp:SqlDataSource ID="SqlDataSource2" runat="server"  
            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            DeleteCommand="DELETE FROM [Akceptacja] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Akceptacja] ([IdPracownika], [Data], [CzasIn], [CzasOut], [Czas], [Acc], [Uwagi], [Absencja]) VALUES (@IdPracownika, @Data, @CzasIn, @CzasOut, @Czas, @Acc, @Uwagi, @Absencja)" 
            SelectCommand="SELECT * FROM [Akceptacja]" 
            
            UpdateCommand="UPDATE [Akceptacja] SET [IdPracownika] = @IdPracownika, [Data] = @Data, [CzasIn] = @CzasIn, [CzasOut] = @CzasOut, [Czas] = @Czas, [Acc] = @Acc, [Uwagi] = @Uwagi, [Absencja] = @Absencja WHERE [Id] = @Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="IdPracownika" Type="Int32" />
                <asp:Parameter Name="Data" Type="DateTime" />
                <asp:Parameter Name="CzasIn" Type="DateTime" />
                <asp:Parameter Name="CzasOut" Type="DateTime" />
                <asp:Parameter Name="Czas" Type="DateTime" />
                <asp:Parameter Name="Acc" Type="Boolean" />
                <asp:Parameter Name="Uwagi" Type="String" />
                <asp:Parameter Name="Absencja" Type="Int32" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="IdPracownika" Type="Int32" />
                <asp:Parameter Name="Data" Type="DateTime" />
                <asp:Parameter Name="CzasIn" Type="DateTime" />
                <asp:Parameter Name="CzasOut" Type="DateTime" />
                <asp:Parameter Name="Czas" Type="DateTime" />
                <asp:Parameter Name="Acc" Type="Boolean" />
                <asp:Parameter Name="Uwagi" Type="String" />
                <asp:Parameter Name="Absencja" Type="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>



    <asp:FormView ID="FormView1" runat="server" AllowPaging="True" BackColor="White" 
            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            DataKeyNames="Id" DataSourceID="SqlDataSource2" GridLines="Vertical" 
            onpageindexchanging="FormView1_PageIndexChanging">
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <EditItemTemplate>
            Id:
            <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            IdPracownika:
            <asp:TextBox ID="IdPracownikaTextBox" runat="server" 
                Text='<%# Bind("IdPracownika") %>' />
            <br />
            Data:
            <asp:TextBox ID="DataTextBox" runat="server" Text='<%# Bind("Data") %>' />
            <br />
            CzasIn:
            <asp:TextBox ID="CzasInTextBox" runat="server" Text='<%# Bind("CzasIn") %>' />
            <br />
            CzasOut:
            <asp:TextBox ID="CzasOutTextBox" runat="server" Text='<%# Bind("CzasOut") %>' />
            <br />
            Czas:
            <asp:TextBox ID="CzasTextBox" runat="server" Text='<%# Bind("Czas") %>' />
            <br />
            Acc:
            <asp:CheckBox ID="AccCheckBox" runat="server" Checked='<%# Bind("Acc") %>' />
            <br />
            Uwagi:
            <asp:TextBox ID="UwagiTextBox" runat="server" Text='<%# Bind("Uwagi") %>' />
            <br />
            Absencja:
            <asp:TextBox ID="AbsencjaTextBox" runat="server" 
                Text='<%# Bind("Absencja") %>' />
            <br />
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                CommandName="Update" Text="Update" />
            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                CausesValidation="False" CommandName="Cancel" Text="Cancel" />
        </EditItemTemplate>
        <InsertItemTemplate>
            IdPracownika:
            <asp:TextBox ID="IdPracownikaTextBox" runat="server" 
                Text='<%# Bind("IdPracownika") %>' />
            <br />
            Data:
            <asp:TextBox ID="DataTextBox" runat="server" Text='<%# Bind("Data") %>' />
            <br />
            CzasIn:
            <asp:TextBox ID="CzasInTextBox" runat="server" Text='<%# Bind("CzasIn") %>' />
            <br />
            CzasOut:
            <asp:TextBox ID="CzasOutTextBox" runat="server" Text='<%# Bind("CzasOut") %>' />
            <br />
            Czas:
            <asp:TextBox ID="CzasTextBox" runat="server" Text='<%# Bind("Czas") %>' />
            <br />
            Acc:
            <asp:CheckBox ID="AccCheckBox" runat="server" Checked='<%# Bind("Acc") %>' />
            <br />
            Uwagi:
            <asp:TextBox ID="UwagiTextBox" runat="server" Text='<%# Bind("Uwagi") %>' />
            <br />
            Absencja:
            <asp:TextBox ID="AbsencjaTextBox" runat="server" 
                Text='<%# Bind("Absencja") %>' />
            <br />
            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                CommandName="Insert" Text="Insert" />
            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
                CausesValidation="False" CommandName="Cancel" Text="Cancel" />
        </InsertItemTemplate>
        <ItemTemplate>
            Id:
            <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            IdPracownika:
            <asp:Label ID="IdPracownikaLabel" runat="server" 
                Text='<%# Bind("IdPracownika") %>' />
            <br />
            Data:
            <asp:Label ID="DataLabel" runat="server" Text='<%# Bind("Data") %>' />
            <br />
            CzasIn:
            <asp:Label ID="CzasInLabel" runat="server" Text='<%# Bind("CzasIn") %>' />
            <br />
            CzasOut:
            <asp:Label ID="CzasOutLabel" runat="server" Text='<%# Bind("CzasOut") %>' />
            <br />
            Czas:
            <asp:Label ID="CzasLabel" runat="server" Text='<%# Bind("Czas") %>' />
            <br />
            Acc:
            <asp:CheckBox ID="AccCheckBox" runat="server" Checked='<%# Bind("Acc") %>' 
                Enabled="false" />
            <br />
            Uwagi:
            <asp:Label ID="UwagiLabel" runat="server" Text='<%# Bind("Uwagi") %>' />
            <br />
            Absencja:
            <asp:Label ID="AbsencjaLabel" runat="server" Text='<%# Bind("Absencja") %>' />
            <br />
            <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" 
                CommandName="Edit" Text="Edit" />
            &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" 
                CommandName="Delete" Text="Delete" />
            &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" 
                CommandName="New" Text="New" />
        </ItemTemplate>
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
    </asp:FormView>


        --%>

        <hr />
        <asp:Button ID="btAbsDl" runat="server" Text="Absencje długotrwałe" CssClass="button" OnClick="btAbsDl_Click" />
        <asp:Button ID="Button13" runat="server" Text="Absencje długotrwałe" CssClass="button" PostBackUrl="~/AbsencjeDlugotrwale.aspx" />
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/AbsencjeDlugotrwale.aspx">HyperLink</asp:HyperLink>
        <asp:LinkButton ID="LinkButton6" runat="server" PostBackUrl="~/AbsencjeDlugotrwale.aspx">LinkButton</asp:LinkButton>


        <%-- KDR codeshare --%>
        <hr />
		<asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Rows="50" Columns="132" ></asp:TextBox>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Enabled="true" Interval="2000"></asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            DeleteCommand="DELETE FROM KDR_SHARE..Shared WHERE [ShareId] = @ShareId" 
            InsertCommand="INSERT INTO KDR_SHARE..Shared ([ShareId], [Text]) VALUES (@ShareId, @Text)" 
            SelectCommand="
--declare @shareid nvarchar(100)
--set @shareid = '1'

if @shareid is not null and not exists(select * from KDR_SHARE..Shared where ShareId = @shareid)
	insert into KDR_SHARE..Shared (ShareId) values (@shareid)

select * from KDR_SHARE..Shared where ShareId = @shareid
            " 
            UpdateCommand="UPDATE KDR_SHARE..Shared SET [Text] = @Text WHERE [ShareId] = @ShareId" >
            <DeleteParameters>
                <asp:Parameter Name="ShareId" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ShareId" Type="String" />
                <asp:Parameter Name="Text" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="ShareId" QueryStringField="id" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Text" Type="String" />
                <asp:Parameter Name="ShareId" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        
        <script runat="server">
            public static System.Collections.Generic.Dictionary<string, string> KDR_SHARE = new System.Collections.Generic.Dictionary<string, string>();
            
            protected override void OnLoad(EventArgs e)
            {
                if (!IsPostBack)
		        {
			        string id = Request.QueryString["id"];
			        if (!String.IsNullOrEmpty(id))
			        {
				        Load(id);
			        }
			        else 
			        {
				        string shareid = Generate();
				        Response.Redirect(String.Format("~/test.aspx?id={0}", shareid));
			        }
		        }
                base.OnLoad(e);
            }

	        private string ShareId
	        {
		        set { ViewState["shareid"] = value; }
		        get { return (String)(ViewState["shareid"] ?? ""); }
	        }

	        private string Generate()
	        {
		        return "1";
	        }

	        private void Load(string shareId)
	        {
		        ShareId = shareId;
                if (KDR_SHARE.ContainsKey(shareId))
                    TextBox1.Text = KDR_SHARE[shareId];
                else
                    TextBox1.Text = null;                    
                /*
		        SqlDataSource1.SelectParameters["ShareId"].DefaultValue = shareId;
                System.Data.DataView dv = (System.Data.DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
		        if (dv.Table.Rows.Count > 0)
			        TextBox1.Text = dv.Table.Rows[0]["Text"].ToString();
                */ 
	        }

	        private void Save()
	        {
                KDR_SHARE[ShareId] = TextBox1.Text;
                /*
		        SqlDataSource1.UpdateParameters["ShareId"].DefaultValue = ShareId;
		        SqlDataSource1.UpdateParameters["Text"].DefaultValue = TextBox1.Text;
		        SqlDataSource1.Update();
                */ 
	        }

	        protected void Timer1_Tick(object sender, EventArgs e)
	        {
                Save();
	        }
        </script>
        <%-- KDR codeshare --%>

    </div>
    </form>
</body>
</html>
