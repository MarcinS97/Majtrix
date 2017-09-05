<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test3.aspx.cs" Inherits="HRRcp.test31" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        ---3-----------------------------<br />
        
        
        ---4-----------------------------<br />
        
        
        ---5-----------------------------<br />
        <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px" 
            AllowPaging="True" CellPadding="4" DataKeyNames="Id" 
            DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
            <RowStyle BackColor="#EFF3FB" />
            <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <Fields>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                    ShowInsertButton="True" />
            </Fields>
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderTemplate>
                header
            </HeaderTemplate>
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:DetailsView>
        
        ---6-----------------------------<br />
        <asp:DataList ID="DataList1" runat="server" 
        
        
        
            visible="true"
        
        
        
            Caption="<%$ ConnectionStrings:PORTAL.ProviderName %>" CellPadding="4" 
            DataKeyField="Id" DataSourceID="SqlDataSource1" EditItemIndex="0" 
            Font-Bold="False" Font-Italic="False" Font-Overline="False" 
            Font-Strikeout="False" Font-Underline="False" ForeColor="#333333" 
            RepeatColumns="3" UseAccessibleHeader="True">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditItemStyle BackColor="Yellow" Font-Bold="False" Font-Italic="False" 
                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
            <AlternatingItemStyle BackColor="White" />
            <ItemStyle BackColor="#EFF3FB" />
            <SeparatorStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                Font-Strikeout="False" Font-Underline="False" ForeColor="Fuchsia" />
            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SeparatorTemplate>
                -----separator -----
            </SeparatorTemplate>
            <HeaderTemplate>
                header
            </HeaderTemplate>
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <FooterTemplate>
                footer
            </FooterTemplate>
            <ItemTemplate>
                Id:
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                NR_EW:
                <asp:Label ID="NR_EWLabel" runat="server" Text='<%# Eval("NR_EW") %>' />
                <br />
                Nazwa:
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
                <br />
                PROJ_DATE:
                <asp:Label ID="PROJ_DATELabel" runat="server" Text='<%# Eval("PROJ_DATE") %>' />
                <br />
                PROJ_ACT:
                <asp:Label ID="PROJ_ACTLabel" runat="server" Text='<%# Eval("PROJ_ACT") %>' />
                <br />
                IdStrefy:
                <asp:Label ID="IdStrefyLabel" runat="server" Text='<%# Eval("IdStrefy") %>' />
                <br />
                <br />
            </ItemTemplate>
        </asp:DataList>
        
        ---1-----------------------------<br />
        <asp:FormView ID="FormView1" runat="server" AllowPaging="True" CellPadding="4" 
            DataKeyNames="Id" DataSourceID="SqlDataSource1" ForeColor="#333333" 
            GridLines="Vertical">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <EditItemTemplate>
                Id:
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                NR_EW:
                <asp:TextBox ID="NR_EWTextBox" runat="server" Text='<%# Bind("NR_EW") %>' />
                <br />
                Nazwa:
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                <br />
                PROJ_DATE:
                <asp:TextBox ID="PROJ_DATETextBox" runat="server" 
                    Text='<%# Bind("PROJ_DATE") %>' />
                <br />
                PROJ_ACT:
                <asp:TextBox ID="PROJ_ACTTextBox" runat="server" 
                    Text='<%# Bind("PROJ_ACT") %>' />
                <br />
                IdStrefy:
                <asp:TextBox ID="IdStrefyTextBox" runat="server" 
                    Text='<%# Bind("IdStrefy") %>' />
                <br />
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                    CommandName="Update" Text="Update" />
                &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                    CausesValidation="False" CommandName="Cancel" Text="Cancel" />
            </EditItemTemplate>
            <PagerTemplate>
                pager
            </PagerTemplate>
            <InsertItemTemplate>
                Id:
                <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
                <br />
                NR_EW:
                <asp:TextBox ID="NR_EWTextBox" runat="server" Text='<%# Bind("NR_EW") %>' />
                <br />
                Nazwa:
                <asp:TextBox ID="NazwaTextBox" runat="server" 
                    Text='<%# Bind("Nazwa") %>' />
                <br />
                PROJ_DATE:
                <asp:TextBox ID="PROJ_DATETextBox" runat="server" 
                    Text='<%# Bind("PROJ_DATE") %>' />
                <br />
                PROJ_ACT:
                <asp:TextBox ID="PROJ_ACTTextBox" runat="server" 
                    Text='<%# Bind("PROJ_ACT") %>' />
                <br />
                IdStrefy:
                <asp:TextBox ID="IdStrefyTextBox" runat="server" 
                    Text='<%# Bind("IdStrefy") %>' />
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
                NR_EW:
                <asp:Label ID="NR_EWLabel" runat="server" Text='<%# Bind("NR_EW") %>' />
                <br />
                Nazwa:
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Bind("Nazwa") %>' />
                <br />
                PROJ_DATE:
                <asp:Label ID="PROJ_DATELabel" runat="server" Text='<%# Bind("PROJ_DATE") %>' />
                <br />
                PROJ_ACT:
                <asp:Label ID="PROJ_ACTLabel" runat="server" Text='<%# Bind("PROJ_ACT") %>' />
                <br />
                IdStrefy:
                <asp:Label ID="IdStrefyLabel" runat="server" Text='<%# Bind("IdStrefy") %>' />
                <br />
                <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" 
                    CommandName="Edit" Text="Edit" />
&nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="Delete" />
&nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New" 
                    Text="New" />
            </ItemTemplate>
            
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                no data
            </EmptyDataTemplate>
            <FooterTemplate>
                Footer
            </FooterTemplate>
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderTemplate>
                header
            </HeaderTemplate>
            <EditRowStyle BackColor="#2461BF" />
        </asp:FormView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            DeleteCommand="DELETE FROM [Projekty] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Projekty] ([Id], [NR_EW], [Nazwa], [PROJ_DATE], [PROJ_ACT], [IdStrefy]) VALUES (@Id, @NR_EW, @Nazwa, @PROJ_DATE, @PROJ_ACT, @IdStrefy)" 
            SelectCommand="SELECT * FROM [Projekty]" 
            
            
            UpdateCommand="UPDATE [Projekty] SET [NR_EW] = @NR_EW, [Nazwa] = @Nazwa, [PROJ_DATE] = @PROJ_DATE, [PROJ_ACT] = @PROJ_ACT, [IdStrefy] = @IdStrefy WHERE [Id] = @Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="NR_EW" Type="String" />
                <asp:Parameter Name="Nazwa" Type="String" />
                <asp:Parameter Name="PROJ_DATE" Type="DateTime" />
                <asp:Parameter Name="PROJ_ACT" Type="Int32" />
                <asp:Parameter Name="IdStrefy" Type="Int32" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="NR_EW" Type="String" />
                <asp:Parameter Name="Nazwa" Type="String" />
                <asp:Parameter Name="PROJ_DATE" Type="DateTime" />
                <asp:Parameter Name="PROJ_ACT" Type="Int32" />
                <asp:Parameter Name="IdStrefy" Type="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>
        ---2-----------------------------<br />
        <asp:FormView ID="FormView2" runat="server" AllowPaging="True" CellPadding="4" 
            DataKeyNames="Id" DataSourceID="SqlDataSource1" ForeColor="#333333">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <EditItemTemplate>
                Id:
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                NR_EW:
                <asp:TextBox ID="NR_EWTextBox" runat="server" Text='<%# Bind("NR_EW") %>' />
                <br />
                Nazwa:
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                <br />
                PROJ_DATE:
                <asp:TextBox ID="PROJ_DATETextBox" runat="server" 
                    Text='<%# Bind("PROJ_DATE") %>' />
                <br />
                PROJ_ACT:
                <asp:TextBox ID="PROJ_ACTTextBox" runat="server" 
                    Text='<%# Bind("PROJ_ACT") %>' />
                <br />
                IdStrefy:
                <asp:TextBox ID="IdStrefyTextBox" runat="server" 
                    Text='<%# Bind("IdStrefy") %>' />
                <br />
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                    CommandName="Update" Text="Update" />
                &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                    CausesValidation="False" CommandName="Cancel" Text="Cancel" />
            </EditItemTemplate>
            <InsertItemTemplate>
                Id:
                <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
                <br />
                NR_EW:
                <asp:TextBox ID="NR_EWTextBox" runat="server" Text='<%# Bind("NR_EW") %>' />
                <br />
                Nazwa:
                <asp:TextBox ID="NazwaTextBox" runat="server" 
                    Text='<%# Bind("Nazwa") %>' />
                <br />
                PROJ_DATE:
                <asp:TextBox ID="PROJ_DATETextBox" runat="server" 
                    Text='<%# Bind("PROJ_DATE") %>' />
                <br />
                PROJ_ACT:
                <asp:TextBox ID="PROJ_ACTTextBox" runat="server" 
                    Text='<%# Bind("PROJ_ACT") %>' />
                <br />
                IdStrefy:
                <asp:TextBox ID="IdStrefyTextBox" runat="server" 
                    Text='<%# Bind("IdStrefy") %>' />
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
                NR_EW:
                <asp:Label ID="NR_EWLabel" runat="server" Text='<%# Bind("NR_EW") %>' />
                <br />
                Nazwa:
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Bind("Nazwa") %>' />
                <br />
                PROJ_DATE:
                <asp:Label ID="PROJ_DATELabel" runat="server" Text='<%# Bind("PROJ_DATE") %>' />
                <br />
                PROJ_ACT:
                <asp:Label ID="PROJ_ACTLabel" runat="server" Text='<%# Bind("PROJ_ACT") %>' />
                <br />
                IdStrefy:
                <asp:Label ID="IdStrefyLabel" runat="server" Text='<%# Bind("IdStrefy") %>' />
                <br />
                <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" 
                    CommandName="Edit" Text="Edit" />
                &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="Delete" />
                &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" 
                    CommandName="New" Text="New" />
            </ItemTemplate>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
        </asp:FormView>
        

        
        
        
    </div>
    </form>
</body>
</html>

