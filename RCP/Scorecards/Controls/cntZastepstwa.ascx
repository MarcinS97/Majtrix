<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZastepstwa.ascx.cs" Inherits="HRRcp.Scorecards.Controls.cntZastepstwa" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidKierId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />

<asp:ListView ID="lvZastepstwa" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="None" 
    onitemdatabound="lvZastepstwa_ItemDataBound" 
    onlayoutcreated="lvZastepstwa_LayoutCreated" 
    onitemcommand="lvZastepstwa_ItemCommand" 
    ondatabound="lvZastepstwa_DataBound" 
    onitemcreated="lvZastepstwa_ItemCreated" 
    oniteminserting="lvZastepstwa_ItemInserting" 
    onitemupdating="lvZastepstwa_ItemUpdating" 
    onsorting="lvZastepstwa_Sorting" 
    onpagepropertieschanged="lvZastepstwa_PagePropertiesChanged" 
    oniteminserted="lvZastepstwa_ItemInserted" 
    ondatabinding="lvZastepstwa_DataBinding" 
    onitemupdated="lvZastepstwa_ItemUpdated" 
    onitemdeleting="lvZastepstwa_ItemDeleting" 
    onitemdeleted="lvZastepstwa_ItemDeleted">
    <ItemTemplate>
        <tr style="" runat="server" class="it">
            <td id="tdZastepowany" runat="server" class="kierownik">
                <asp:HiddenField ID="hidIdZastepowany" runat="server" Value='<%# Eval("IdZastepowany") %>' />
                <asp:Label ID="lbZastepowany" runat="server" Text='<%# Eval("Zastepowany") %>' />
            </td>
            <td id="tdZastepujacy" runat="server" class="kierownik">
                <asp:HiddenField ID="hidIdZastepujacy" runat="server" Value='<%# Eval("IdZastepujacy") %>' />
                <asp:Label ID="lbZastepujacy" runat="server" Text='<%# Eval("Zastepujacy") %>' />
            </td>
            <td class="data">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <td id="tdControl" class="control" runat="server">
                <asp:HiddenField ID="hidLogin" runat="server" Value='<%# Eval("Login") %>' />
                &nbsp;
                <asp:Button ID="btZastap" runat="server" CommandName="Zastap" Text="Zastąp" ToolTip="Przejdź do panelu zastępowanego kierownika" Visible="false"/>
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                &nbsp;
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" class="table0">
            <tr class="edt">
                <td>
                    <asp:Label ID="lbNoData" runat="server" Text="Brak ustanowionych zastępstw" /><br /><br />
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td id="tdZastepowany" runat="server" class="kierownik">
                <asp:DropDownList ID="ddlZastepowany" runat="server" ></asp:DropDownList>
            </td>
            <td id="tdZastepujacy" runat="server" class="kierownik">
                <asp:DropDownList ID="ddlZastepujacy" runat="server" 
                    AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlZastepujacy_SelectedIndexChanged" 
                ></asp:DropDownList>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:HiddenField ID="hidSaveConfirm" runat="server" />
                <asp:Button ID="btSave" runat="server" CommandName="Insert" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td id="tdZastepowany" runat="server" class="kierownik">
                <asp:HiddenField ID="hidIdZastepowany" runat="server" Value='<%# Eval("IdZastepowany") %>' />
                <asp:Label ID="lbZastepowany" runat="server" Text='<%# Eval("Zastepowany") %>' />
            </td>
            <td id="tdZastepujacy" runat="server" class="kierownik">
                <asp:HiddenField ID="hidIdZastepujacy" runat="server" Value='<%# Eval("IdZastepujacy") %>' />
                <asp:Label ID="lbZastepujacy" runat="server" Text='<%# Eval("Zastepujacy") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:HiddenField ID="hidSaveConfirm" runat="server" />
                <asp:Button ID="btSave" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr2" runat="server" style="">
                            <th id="thZastepowany" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Zastepowany">
                                Kierownik zastępowany</asp:LinkButton></th>
                            <th id="thZastepujacy" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Zastepujacy">
                                Zastępujący</asp:LinkButton></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Od">
                                Od dnia</asp:LinkButton></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Do">
                                Do dnia</asp:LinkButton></th>
                            <th id="thControl" class="control" runat="server">
                                <%--
                                <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj zastępstwo"/>
                                --%>
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server" >
                <td id="Td2" runat="server" class="pager">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>
            <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Zastepstwa] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Zastepstwa] ([IdZastepowany], [IdZastepujacy], [Od], [Do]) VALUES (@IdZastepowany, @IdZastepujacy, @Od, @Do)" 
    UpdateCommand="UPDATE [Zastepstwa] SET [Od] = @Od, [Do] = @Do WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidKierId" Name="IdKierownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdZastepowany" Type="Int32" />
        <asp:Parameter Name="IdZastepujacy" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
    </InsertParameters>
</asp:SqlDataSource>
