<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="test2.aspx.cs" Inherits="HRRcp.test2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/SqlControl.ascx" tagname="SqlControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc2" %>
<%@ Register src="~/Controls/ImportRogerCSV2.ascx" tagname="ImportRogerCSV2" tagprefix="uc3" %>
<%@ Register src="~/Controls/AssecoRCP.ascx" tagname="AssecoRCP" tagprefix="uc4" %>
<%@ Register src="~/Controls/Przypisania/cntSplityWsp.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>


<%@ Register src="Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc5" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*
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
        */
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <uc5:DateEdit ID="deDay" runat="server" />
    <asp:Button ID="btTestMailingOkres" runat="server" Text="Testuj mailing okres" 
        onclick="btTestMailingOkres_Click" />
    
    <asp:Button ID="Button22" runat="server" Text="Mail testowy (Sabina, DW:Tomek)" 
        onclick="Button22_Click" />
    
    <br />
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc6:cntSplityWsp ID="cntSplityWsp1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <hr />



    <div style="padding: 8px;" >
        Zaloguj jako:<br />
        <asp:DropDownList ID="ddlLogins" runat="server" onselectedindexchanged="ddlUsers_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
        <br />
        <br />
        <asp:HyperLink ID="HyperLink0" runat="server" NavigateUrl="Default.aspx" Text="START"></asp:HyperLink><br />
        <br />
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="AdminForm.aspx" Text="AdminForm"></asp:HyperLink><br /> 
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="UstawieniaForm.aspx" Text="UstawieniaForm"></asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="KierownikForm.aspx" Text="KierownikForm"></asp:HyperLink><br /> 
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="WynikiForm.aspx" Text="WynikiForm"></asp:HyperLink><br /> 
        <br />        
        <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="~/Login.aspx" >LoginForm</asp:LinkButton><br />
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">ErrorForm</asp:LinkButton>
        <br />
        <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="~/Service.aspx" >Service</asp:LinkButton><br />
        <br />
        <br />
        <hr />
        <asp:Button ID="Button21" runat="server" Text="Import RCP" onclick="Button21_Click" />
        <asp:Button ID="Button2" runat="server" Text="Import Readers" onclick="Button2_Click" />
        <asp:Button ID="Button1" runat="server" Text="Import KP" onclick="Button1_Click" />
        <asp:Button ID="Button3" runat="server" Text="Import KP 2" onclick="Button3_Click" />
        <asp:Button ID="Button4" runat="server" Text="Update pracownicy RcpId z KP" onclick="Button4_Click" />
        <asp:Button ID="Button5" runat="server" Text="Update pracownicy RcpId z ROGER" onclick="Button5_Click" />
        <hr />
        <uc2:SelectOkres ID="cntSelectOkres" runat="server" /><br />
        <asp:DropDownList ID="ddlKierownicy" runat="server" /><br />       
        <asp:Button ID="Button7" runat="server" Text="Czy można zamknąć ?" onclick="Button7_Click" />
        <asp:Button ID="Button6" runat="server" Text="Analizuj zatrzaśnięte wartości" onclick="Button6_Click" />
        <asp:Button ID="Button8" runat="server" Text="Zamknij bez sprawdzania" onclick="Button8_Click" />
        <asp:Button ID="Button10" runat="server" Text="Uaktualnij alerty i n50 n100 na PP" onclick="Button10_Click" />
        <asp:Button ID="Button11" runat="server" Text="Export Rcp In Out do tmpRcpInOut" onclick="Button11_Click" />
        <asp:Button ID="Button13" runat="server" Text="Export Rcp Analize do tmpRcpAnalize" onclick="Button13_Click" />
        <asp:Button ID="Button14" runat="server" Text="Eksport do Asseco 1 pracownik test DaneMPK" onclick="Button14_Click" />
        <asp:Button ID="Button15" runat="server" Text="Podział Ludzi - AutoImport" onclick="Button15_Click" />
        <asp:Button ID="Button16" runat="server" Text="Eksport do Asseco zamkniętego tygodnia" onclick="Button16_Click" />
        <asp:Button ID="Button17" runat="server" Text="Saturn.net WS test" OnClick="Button17_Click" />
        <asp:Button ID="Button18" runat="server" Text="Saturn.net WS test2" OnClick="Button18_Click" />
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:CheckBox ID="cbDaneRCP" Checked="true" runat="server" Text="Dane RCP"/><br />
                    <asp:CheckBox ID="cbSumyRCP" Checked="true" runat="server" Text="Sumy RCP"/><br />
                    <asp:CheckBox ID="cbDaneMPK" Checked="true" runat="server" Text="Podział na CC"/><br />
                    <asp:CheckBox ID="cbStruct" Checked="true" runat="server" Text="Struktura"/>
                </td>
                <td style="vertical-align: top;">
                    <asp:Button ID="Button9" runat="server" Text="Eksport danych z okresu do Asseco" onclick="Button9_Click" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <asp:DropDownList ID="ddlRoki" runat="server">
            <asp:ListItem>2014</asp:ListItem>
            <asp:ListItem>2015</asp:ListItem>
            <asp:ListItem Selected="True">2016</asp:ListItem>
            <asp:ListItem>2017</asp:ListItem>
            <asp:ListItem>2018</asp:ListItem>
            <asp:ListItem>2019</asp:ListItem>
            <asp:ListItem>2020</asp:ListItem>
            <asp:ListItem>2021</asp:ListItem>
            <asp:ListItem>2022</asp:ListItem>
            <asp:ListItem>2023</asp:ListItem>
            <asp:ListItem>2024</asp:ListItem>
            <asp:ListItem>2025</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="Button12" runat="server" Text="Import UrlopZBIOR na rok" onclick="Button12_Click" />
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


        <%--
        <uc4:AssecoRCP ID="AssecoRCP1" runat="server" />
        --%>
    </div>

    <asp:SqlDataSource ID="dsRcpAnalize" runat="server"
        SelectCommand="
declare 
	@od datetime,
	@do datetime,
	@kierId int,
	@plist bit
set @od = '{0}'
set @do = '{1}'
set @kierId = {2}

-- tylko dla wybranych pracowników - długo trwa !!!
select IdPracownika into #ppp from tmpRcpAnalize where Data = '20000101'
set @plist = case when (select count(*) from #ppp) != 0 then 1 else 0 end

select distinct IdPracownika, Nazwisko + ' ' + Imie as Pracownik, KadryId 
from dbo.fn_GetTreeOkres(@kierId, @od, @do, @do) 
where @plist = 0 or IdPracownika in (select IdPracownika from #ppp)
order by Pracownik, KadryId
        ">
    </asp:SqlDataSource>
</asp:Content>
