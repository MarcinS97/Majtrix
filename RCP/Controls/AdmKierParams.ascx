<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmKierParams.ascx.cs" Inherits="HRRcp.Controls.AdmKierParams" %>

<%@ Register src="DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:ListView ID="lvKierParams" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" ondatabinding="ListView1_DataBinding" 
    onitemdatabound="ListView1_ItemDataBound" 
    onitemupdating="ListView1_ItemUpdating">
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="KierownikLabel" runat="server" Text='<%# Eval("Kierownik") %>' />
            </td>
            <td>
                <asp:Label ID="PrzerwaMMLabel" runat="server" Text='<%# Eval("PrzerwaMM") %>' />
            </td>
            <td>
                <asp:Label ID="Przerwa2MMLabel" runat="server" Text='<%# Eval("Przerwa2MM") %>' />
            </td>
            <td>
                <asp:Label ID="MarginesMMLabel" runat="server" Text='<%# Eval("MarginesMM") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DataAccDoLabel" runat="server" Text='<%# Eval("DataAccDo", "{0:d}") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr style="">
            <td>
                <asp:TextBox ID="KierownikTextBox" runat="server" Text='<%# Eval("Kierownik") %>' />
            </td>
            <td>
                <asp:TextBox ID="PrzerwaMMTextBox" runat="server" Text='<%# Bind("PrzerwaMM") %>' />
            </td>
            <td>
                <asp:TextBox ID="Przerwa2MMTextBox" runat="server" Text='<%# Bind("Przerwa2MM") %>' />
            </td>
            <td>
                <asp:TextBox ID="MarginesMMTextBox" runat="server" Text='<%# Bind("MarginesMM") %>' />
            </td>
            <td class="data">
                <asp:TextBox ID="DataAccDoTextBox" runat="server" Text='<%# Bind("DataAccDo") %>' />
            </td>
            <td>
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Czyść" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbAdmKierParams" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Kierownik</th>
                            <th runat="server">
                                Przerwa zmianowa [min]</th>
                            <th runat="server">
                                Przerwa nadgodziny [min]</th>
                            <th runat="server">
                                Margines ostrzegania [min]</th>
                            <th runat="server">
                                Akceptacja czasu pracy do</th>
                            <th runat="server" class="control">
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:Label ID="KierownikLabel" runat="server" Text='<%# Eval("Kierownik") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlPrzerwa" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlPrzerwa2" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlMargin" runat="server" ></asp:DropDownList>
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit" runat="server" Date='<%# Bind("DataAccDo") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [KierParams] WHERE [IdKierownika] = @IdKierownika" 
    InsertCommand="INSERT INTO [KierParams] ([IdKierownika], [PrzerwaMM], [Przerwa2MM], [MarginesMM], [DataAccDo]) VALUES (@IdKierownika, @PrzerwaMM, @Przerwa2MM, @MarginesMM, @DataAccDo)"     
    SelectCommand="select top 1 1 as Sort, 'Poziom główny struktury' as Kierownik, 0 as Id, K.* 
                   from Pracownicy
                   left outer join KierParams K on K.IdKierownika = 0 
                   union     
                   SELECT 0 as Sort, P.Nazwisko + ' ' + P.Imie as Kierownik, P.Id, K.* 
                   FROM Pracownicy P 
                   left outer join KierParams K on K.IdKierownika = P.Id
                   where P.Kierownik=1
                   order by Sort, Kierownik"  
    UpdateCommand="UPDATE [KierParams] SET [PrzerwaMM] = @PrzerwaMM, [Przerwa2MM] = @Przerwa2MM, [MarginesMM] = @MarginesMM, [DataAccDo] = @DataAccDo WHERE [IdKierownika] = @IdKierownika">
    <DeleteParameters>
        <asp:Parameter Name="IdKierownika" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="PrzerwaMM" Type="Int32" />
        <asp:Parameter Name="Przerwa2MM" Type="Int32" />
        <asp:Parameter Name="MarginesMM" Type="Int32" />
        <asp:Parameter Name="DataAccDo" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="PrzerwaMM" Type="Int32" />
        <asp:Parameter Name="Przerwa2MM" Type="Int32" />
        <asp:Parameter Name="MarginesMM" Type="Int32" />
        <asp:Parameter Name="DataAccDo" Type="DateTime" />
    </InsertParameters>
</asp:SqlDataSource>


<%--
    SelectCommand="SELECT P.Nazwisko + ' ' + P.Imie as Kierownik, K.* FROM KierParams K left outer join Pracownicy P on P.Id = K.IdKierownika order by Kierownik"  
--%>