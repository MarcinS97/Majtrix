<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPK.ascx.cs" Inherits="HRRcp.Controls.MPK" %>
<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidPlanId" runat="server" />

<asp:ListView ID="lvMPK" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="LastItem" 
    onitemcreated="lvMPK_ItemCreated" onitemdatabound="lvMPK_ItemDataBound" 
    oniteminserting="lvMPK_ItemInserting" onitemupdating="lvMPK_ItemUpdating" 
    ondatabound="lvMPK_DataBound">
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="MPKLabel" runat="server" Text='<%# Eval("MPK") %>' />
            </td>
            <td></td>
            <td></td>
            <td>
                <asp:Label ID="CzasZmLabel" runat="server" Text='<%# Eval("CzasZm") %>' />
            </td>
            <td>
                <asp:Label ID="NadgodzinyDzienLabel" runat="server" Text='<%# Eval("NadgodzinyDzien") %>' />
            </td>
            <td>
                <asp:Label ID="NadgodzinyNocLabel" runat="server" Text='<%# Eval("NadgodzinyNoc") %>' />
            </td>
            <td>
                <asp:Label ID="NocneLabel" runat="server" Text='<%# Eval("Nocne") %>' />
            </td>
            <td>
                <asp:Label ID="UwagiLabel" runat="server" Text='<%# Eval("Uwagi") %>' />
            </td>
            <td class="control">
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Podziel na centra kosztowe" />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td>
                <asp:TextBox ID="IdMPKTextBox" runat="server" Text='<%# Bind("IdMPK") %>' Visible="false" />
                <asp:DropDownList ID="ddlMPK" runat="server" DataSourceID="SqlDataSource2" DataTextField="CC" DataValueField="Id" ></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="CzasZmTextBox" runat="server" Text='<%# Bind("CzasZm") %>' Visible="false"/>
                <uc1:TimeEdit ID="teWorktime" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td>
                <asp:TextBox ID="NadgodzinyDzienTextBox" runat="server" Text='<%# Bind("NadgodzinyDzien") %>' Visible="false"/>
                <uc1:TimeEdit ID="teNadgDzien" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td>
                <asp:TextBox ID="NadgodzinyNocTextBox" runat="server" Text='<%# Bind("NadgodzinyNoc") %>' Visible="false"/>
                <uc1:TimeEdit ID="teNadgNoc" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>'/>
            </td>
            <td>
                <asp:TextBox ID="NocneTextBox" runat="server" Text='<%# Bind("Nocne") %>' Visible="false"/>
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>'/>
            </td>
            <td rowspan="2" class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
        <%--
        <tr>
            <td colspan="5">
                <asp:TextBox ID="tbUwagi" CssClass="textbox" runat="server" Rows="3" TextMode="MultiLine" Text='<%# Bind("Uwagi") %>' ></asp:TextBox>
            </td>
        </tr>
        --%>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:TextBox ID="IdMPKTextBox" runat="server" Text='<%# Bind("IdMPK") %>' />
                <asp:DropDownList ID="ddlMPK" DataSourceID="SqlDataSource2" runat="server" DataTextField="CC" DataValueField="Id"></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="CzasZmTextBox" runat="server" Text='<%# Bind("CzasZm") %>' />
                <uc1:TimeEdit ID="teWorktime" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td>
                <asp:TextBox ID="NadgodzinyDzienTextBox" runat="server" Text='<%# Bind("NadgodzinyDzien") %>' />
                <uc1:TimeEdit ID="teNadgDzien" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td>
                <asp:TextBox ID="NadgodzinyNocTextBox" runat="server" Text='<%# Bind("NadgodzinyNoc") %>' />
                <uc1:TimeEdit ID="teNadgNoc" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>'/>
            </td>
            <td>
                <asp:TextBox ID="NocneTextBox" runat="server" Text='<%# Bind("Nocne") %>' />
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>'/>
            </td>
            <td rowspan="2" class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:TextBox ID="tbUwagi" CssClass="textbox" runat="server" Rows="3" TextMode="MultiLine" Text='<%# Bind("Uwagi") %>' ></asp:TextBox>
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 tbEnterMPK hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th id="Th2" runat="server">
                                Czas pracy/CC</th>
                            <th id="Th1" runat="server">
                                Czas pracy na zmianie</th>
                            <th runat="server">
                                Nadgodziny dzien</th>
                            <th runat="server">
                                Nadgodziny noc</th>
                            <th runat="server">
                                Praca w nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6"></asp:Label>)</th>
                            <th class="control" runat="server"></th>
                        </tr>
                        <tr class="it">
                            <td>Czas RCP</td>
                            <td class="col2"><asp:Label ID="lbWorktime" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgDzien" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgNoc" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNocne" runat="server" ></asp:Label></td>
                            <td></td>
                        </tr>
                        <tr class="it">
                            <td>Macierzyste CC</td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teWorktime" runat="server" />
                                <asp:Label ID="lbWorktimeVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadgDzien" runat="server" />
                                <asp:Label ID="lbNadgDzienVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadgNoc" runat="server" />
                                <asp:Label ID="lbNadgNocVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNocne" runat="server" />
                                <asp:Label ID="lbNocneVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                            <td></td>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" class="pager">
                <td runat="server" style="">
                    <asp:DataPager ID="DataPager1" runat="server">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="SELECT P.Id, P.IdMPK as IdMPK, CC.cc + ' - ' + CC.Nazwa as MPK, P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne, P.Uwagi FROM PodzialKosztow P
                    left outer join CC on CC.Id = P.IdMPK
                    WHERE ([IdPlanPracy] = @IdPlanPracy) ORDER BY P.Id"
    DeleteCommand="DELETE FROM [PodzialKosztow] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PodzialKosztow] ([IdPlanPracy], [IdMPK], [CzasZm], [NadgodzinyDzien], [NadgodzinyNoc], [Nocne], [Uwagi]) VALUES (@IdPlanPracy, @IdMPK, @CzasZm, @NadgodzinyDzien, @NadgodzinyNoc, @Nocne, @Uwagi)" 
    UpdateCommand="UPDATE [PodzialKosztow] SET [IdPlanPracy] = @IdPlanPracy, [IdMPK] = @IdMPK, [CzasZm] = @CzasZm, [NadgodzinyDzien] = @NadgodzinyDzien, [NadgodzinyNoc] = @NadgodzinyNoc, [Nocne] = @Nocne, [Uwagi] = @Uwagi WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPlanId" Name="IdPlanPracy" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidPlanId" Name="IdPlanPracy" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="IdMPK" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPlanId" Name="IdPlanPracy" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="IdMPK" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select null as Id, 'wybierz ...' as CC, 0 as Sort
                    union 
                   SELECT Id, cc + ' - ' + Nazwa as CC, 1 as Sort FROM [CC] 
                   WHERE DATEDIFF(DAY, 0, GETDATE()) between AktywneOd and ISNULL(AktywneDo, GETDATE()) ORDER BY Sort, cc">
</asp:SqlDataSource>
