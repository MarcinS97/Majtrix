<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DzialyStrefy.ascx.cs" Inherits="HRRcp.Controls.DzialyStrefy" %>

<%@ Register src="DateTimeEnter.ascx" tagname="DateTimeEnter" tagprefix="uc1" %>

<asp:HiddenField ID="hidDzialId" runat="server" />

<asp:ListView ID="lvDzialyStrefy" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" 
    InsertItemPosition="FirstItem" onitemdatabound="lvDzialyStrefy_ItemDataBound" 
    oniteminserting="lvDzialyStrefy_ItemInserting" 
    onitemupdating="lvDzialyStrefy_ItemUpdating">
    <ItemTemplate>
        <tr class="it">
            <td class="col1">
                <asp:Label ID="ObowiazujeOdLabel" runat="server" Text='<%# Eval("DataOd") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="KierStrefaIdLabel" runat="server" Text='<%# Eval("KierStrefa") %>' />
            </td>
            <td class="col3">
                <asp:Label ID="KierAlgorytmLabel" runat="server" Text='<%# Eval("KierAlgorytmNazwa") %>' />
            </td>
            <td class="col4">
                <asp:Label ID="PracStrefaIdLabel" runat="server" Text='<%# Eval("PracStrefa") %>' />
            </td>
            <td class="col5">
                <asp:Label ID="PracAlgorytmLabel" runat="server" Text='<%# Eval("PracAlgorytmNazwa") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
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
        <tr class="iit">
            <td class="col1">
                    <%--
                <asp:TextBox ID="DataOdTextBox" runat="server" Text='<%# Bind("DataOd") %>' />
                    --%>
                <uc1:DateTimeEnter ID="dteDataOd" runat="server" DateTime='<%# Bind("DataOd") %>' />
            </td>
            <td class="col2">
                <asp:DropDownList ID="ddlKierStrefa" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="col3">
                <asp:DropDownList ID="ddlKierAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
            </td>
            <td class="col4">
                <asp:DropDownList ID="ddlPracStrefa" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="col5">
                <asp:DropDownList ID="ddlPracAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Wyczyść" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbDzialyStrefy" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">Obowiązuje od</th>
                            <th runat="server">Strefa RCP Kierowników</th>
                            <th runat="server">Algorytm Kierownika</th>
                            <th runat="server">Strefa RCP Pracowników</th>
                            <th runat="server">Algorytm Pracownika</th>
                            <th class="control" runat="server"></th>
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
            <td class="col1">
                <%--
                <asp:TextBox ID="DataOdTextBox" runat="server" Text='<%# Bind("DataOd") %>' />
                --%>
                <uc1:DateTimeEnter ID="dteDataOd" runat="server" DateTime='<%# Bind("DataOd") %>' />
            </td>
            <td class="col2">
                <asp:DropDownList ID="ddlKierStrefa" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="col3">
                <asp:DropDownList ID="ddlKierAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
            </td>
            <td class="col4">
                <asp:DropDownList ID="ddlPracStrefa" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="col5">
                <asp:DropDownList ID="ddlPracAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
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
    DeleteCommand="DELETE FROM [DzialyStrefy] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [DzialyStrefy] ([IdDzialu], [DataOd], [KStrefaId], [KAlgorytm], [PStrefaId], [PAlgorytm]) VALUES (@IdDzialu, @DataOd, @KStrefaId, @KAlgorytm, @PStrefaId, @PAlgorytm)" 
    SelectCommand="SELECT D.Id, D.DataOd, D.KStrefaId, D.KAlgorytm, D.PStrefaId, D.PAlgorytm,
	                    K.Nazwa as KierStrefa,
	                    AK.Nazwa as KierAlgorytmNazwa,
	                    P.Nazwa as PracStrefa, 
	                    AP.Nazwa as PracAlgorytmNazwa
                    FROM DzialyStrefy D 
                        left outer join Strefy K on K.Id = D.KStrefaId
                        left outer join Strefy P on P.Id = D.PStrefaId
                        left outer join Kody AK on AK.Kod = D.KAlgorytm and AK.Typ='ALG'
                        left outer join Kody AP on AP.Kod = D.PAlgorytm and AP.Typ='ALG'     
                    WHERE (D.IdDzialu = @IdDzialu) ORDER BY D.DataOd DESC" 
    UpdateCommand="UPDATE [DzialyStrefy] SET [IdDzialu] = @IdDzialu, [DataOd] = @DataOd, [KStrefaId] = @KStrefaId, [KAlgorytm] = @KAlgorytm, [PStrefaId] = @PStrefaId, [PAlgorytm] = @PAlgorytm WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidDzialId" Name="IdDzialu" 
            PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidDzialId" Name="IdDzialu" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="KStrefaId" Type="Int32" />
        <asp:Parameter Name="KAlgorytm" Type="Int32" />
        <asp:Parameter Name="PStrefaId" Type="Int32" />
        <asp:Parameter Name="PAlgorytm" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidDzialId" Name="IdDzialu" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="KStrefaId" Type="Int32" />
        <asp:Parameter Name="KAlgorytm" Type="Int32" />
        <asp:Parameter Name="PStrefaId" Type="Int32" />
        <asp:Parameter Name="PAlgorytm" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT [Id], [Nazwa] FROM [Strefy] WHERE ([Aktywna] = 1) 
                    union 
                    select null as Id, ' wybierz ...' as Nazwa                   
                    ORDER BY [Nazwa]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT [Id], [Nazwa], [Lp], [Kod], [Parametr] FROM [Kody] WHERE ([Typ] = 'ALG') 
                    union 
                    select null as Id, ' wybierz ...' as Nazwa, 0 as Lp, null as Kod, null as Parametr                   
                    ORDER BY [Lp]">
</asp:SqlDataSource>
