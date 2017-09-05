<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="x_AcceptControl.ascx.cs" Inherits="HRRcp.Controls.x_AcceptControl" %>

<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />

<asp:FormView ID="fvData" runat="server" DataKeyNames="Id" 
    DefaultMode="Insert"
    HeaderText="Akceptacja danych"
    DataSourceID="SqlDataSource1" onpageindexchanging="FormView1_PageIndexChanging">
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
</asp:FormView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Akceptacja] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Akceptacja] ([IdPracownika], [Data], [CzasIn], [CzasOut], [Czas], [Acc], [Uwagi], [Absencja]) VALUES (@IdPracownika, @Data, @CzasIn, @CzasOut, @Czas, @Acc, @Uwagi, @Absencja)" 
    SelectCommand="SELECT * FROM [Akceptacja] WHERE (([IdPracownika] = @IdPracownika) AND ([Data] = @Data))" 
    UpdateCommand="UPDATE [Akceptacja] SET [IdPracownika] = @IdPracownika, [Data] = @Data, [CzasIn] = @CzasIn, [CzasOut] = @CzasOut, [Czas] = @Czas, [Acc] = @Acc, [Uwagi] = @Uwagi, [Absencja] = @Absencja WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
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

