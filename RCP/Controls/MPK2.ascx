<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPK2.ascx.cs" Inherits="HRRcp.Controls.MPK2" %>


<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>



<asp:HiddenField ID="hidPlanId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />
<asp:HiddenField ID="hidCzasZm" runat="server" />
<asp:HiddenField ID="hidNadgD" runat="server" />
<asp:HiddenField ID="hidNadgN" runat="server" />
<asp:HiddenField ID="hidNocne" runat="server" />

<asp:ListView ID="lvMPK" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="LastItem" 
    onitemcreated="lvMPK_ItemCreated" onitemdatabound="lvMPK_ItemDataBound" 
    oniteminserting="lvMPK_ItemInserting" onitemupdating="lvMPK_ItemUpdating" 
    ondatabound="lvMPK_DataBound" onlayoutcreated="lvMPK_LayoutCreated" 
    onitemcanceling="lvMPK_ItemCanceling" onitemediting="lvMPK_ItemEditing" 
    onitemupdated="lvMPK_ItemUpdated">
    <ItemTemplate>
        <tr class="it">
            <td class="col1">
                <asp:Label ID="MPKLabel" runat="server" Text='<%# Eval("MPK") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="CzasZmLabel" runat="server" Text='<%# Eval("CzasZmT") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="NadgodzinyDzienLabel" runat="server" Text='<%# Eval("NadgodzinyDzienT") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="NadgodzinyNocLabel" runat="server" Text='<%# Eval("NadgodzinyNocT") %>' />
            </td>
            <td id="tdLastCol" runat="server" class="col2">
                <asp:Label ID="NocneLabel" runat="server" Text='<%# Eval("NocneT") %>' />
            </td>
            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" class="tbEnterMPKedt">
            <tr>
                <td>
                    Brak podziału
                    <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Podziel na centra kosztowe" Visible="false"/>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td class="col1">
                <asp:DropDownList ID="ddlMPK" runat="server" DataSourceID="SqlDataSource2" DataTextField="CC" DataValueField="Id" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlMPK"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvMPK" runat="server" Display="Dynamic"
                    ValidationGroup="vgi"
                    ControlToValidate="ddlMPK"
                    OnServerValidate="ddlMPK_ValidateInsert"
                    CssClass="t4n error"
                    ErrorMessage="Powtórzone CC">
                </asp:CustomValidator>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col1">
                <asp:HiddenField ID="hidMPK" runat="server" Value='<%# Eval("IdMPK") %>' />
                <asp:DropDownList ID="ddlMPK" DataSourceID="SqlDataSource2" runat="server" DataTextField="CC" DataValueField="Id"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge" 
                    ControlToValidate="ddlMPK"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvMPK" runat="server" Display="Dynamic"
                    ValidationGroup="vge"
                    ControlToValidate="ddlMPK"
                    OnServerValidate="ddlMPK_ValidateEdit"
                    CssClass="t4n error"
                    ErrorMessage="Powtórzone CC">
                </asp:CustomValidator>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>'/>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>'/>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
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
                                Czas pracy / Centrum kosztowe<br />
                                <span class="t4n">czas z dokładnością do 30 min.</span>
                                </th>
                            <th id="Th1" runat="server">
                                Zmiana</th>
                            <th runat="server">
                                Nadg.<br />w dzień</th>
                            <th runat="server">
                                Nadg.<br />w nocy</th>
                            <th id="thLastCol" runat="server">
                                Praca w<br />nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6"></asp:Label>)</th>
                            <th id="thControl" class="control" runat="server">&nbsp;</th>
                        </tr>
                        <tr runat="server" class="it total">
                            <td class="col1">Czas łączny <span class="t4">(hh:mm)</span></td>
                            <td class="col2"><asp:Label ID="lbCzasZm" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgD" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgN" runat="server" ></asp:Label></td>
                            <td id="thLastCol1" class="col2" runat="server"><asp:Label ID="lbNocne" runat="server" ></asp:Label></td>
                            <td id="thControl1" class="control" runat="server">&nbsp;</td>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="SELECT P.Id, P.IdMPK as IdMPK, CC.cc + ' - ' + CC.Nazwa as MPK, 
                        P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne,
                        dbo.ToTimeHMM(P.CzasZm) as CzasZmT, 
                        dbo.ToTimeHMM(P.NadgodzinyDzien) as NadgodzinyDzienT, 
                        dbo.ToTimeHMM(P.NadgodzinyNoc) as NadgodzinyNocT, 
                        dbo.ToTimeHMM(P.Nocne) as NocneT, 
                        P.Uwagi FROM PodzialKosztow P
                    left outer join CC on CC.Id = P.IdMPK
                    WHERE ([IdPracownika] = @IdPracownika and [Data] = @Data) ORDER BY P.Id"
    DeleteCommand="DELETE FROM [PodzialKosztow] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PodzialKosztow] ([IdPlanPracy], [Data], [IdPracownika], [IdMPK], [CzasZm], [NadgodzinyDzien], [NadgodzinyNoc], [Nocne], [Uwagi]) VALUES (@IdPlanPracy, @Data, @IdPracownika, @IdMPK, @CzasZm, @NadgodzinyDzien, @NadgodzinyNoc, @Nocne, @Uwagi)" 
    UpdateCommand="UPDATE [PodzialKosztow] SET [IdMPK] = @IdMPK, [CzasZm] = @CzasZm, [NadgodzinyDzien] = @NadgodzinyDzien, [NadgodzinyNoc] = @NadgodzinyNoc, [Nocne] = @Nocne, [Uwagi] = @Uwagi WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
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
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
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
                   WHERE @Data between AktywneOd and ISNULL(AktywneDo, @Data) ORDER BY Sort, cc">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
