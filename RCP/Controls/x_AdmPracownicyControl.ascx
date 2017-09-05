<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="x_AdmPracownicyControl.ascx.cs" Inherits="HRRcp.Controls.x_AdmPracownicyControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>

<asp:ListView ID="lvPracownicy" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" onlayoutcreated="lvPracownicy_LayoutCreated" 
    onsorting="lvPracownicy_Sorting" 
    onitemdatabound="lvPracownicy_ItemDataBound" 
    onitemcommand="lvPracownicy_ItemCommand" 
    onprerender="lvPracownicy_PreRender" 
    onitemupdating="lvPracownicy_ItemUpdating" >
    <ItemTemplate>
        <tr class="it" style="">
            <td class="col1">
                <asp:Label ID="NazwiskoLabel" runat="server" Text='<%# PrepareName(Eval("NazwiskoImie")) %>' />
            </td>
            <td class="col2 right">
                <%--
                <asp:Label ID="KadryIdLabel" runat="server" Text='<%# Eval("NR_EW") %>' />
                --%>
                <asp:LinkButton ID="lbtNazwisko" runat="server" 
                    Text='<%# Eval("NR_EW") %>' 
                    ToolTip="Podgląd danych pracownika"
                    CommandName="ZOOM:AnkietaK" 
                    CommandArgument='<%# Eval("Id") %>'>
                </asp:LinkButton>
            </td>
            <td class="col3">
                <asp:Label ID="LoginLabel" runat="server" Text='<%# Eval("Login") %>' />
            </td>
            <td class="col4">
                <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' />
            </td>

            <td class="col5 textcenter">
                <asp:CheckBox ID="CheckedCheckBox" runat="server" Checked='<%# Eval("Checked") %>' Enabled="false" />
            </td>
            <td class="col6 textcenter">
                <asp:CheckBox ID="KontrolerCheckBox" runat="server" Checked='<%# Eval("Kontroler1") %>' Enabled="false" />
            </td>
            <td class="col7 textcenter">
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Kontroler2") %>' Enabled="false" />
            </td>
            <td class="col8 textcenter">
                <asp:CheckBox ID="OperatorCheckBox" runat="server" Checked='<%# Eval("Operator") %>' Enabled="false" />
            </td>
            <td class="col9 textcenter">
                <asp:CheckBox ID="AdminCheckBox" runat="server" Checked='<%# Eval("Admin") %>' Enabled="false" />
            </td>
            
            <td id="tdStatus" runat="server" class="col10 right">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            
            <td id="tdStanAnkiety" runat="server" class="col11">
                <asp:LinkButton ID="lbtCol10" runat="server" 
                    Text='<%# Eval("StanAnkiety1") %>' 
                    ToolTip="Podgląd ankiety"
                    CommandName="ZOOM:Ankieta" 
                    CommandArgument='<%# Eval("Id") + "|" + Eval("StanAnkiety") %>'>
                </asp:LinkButton>
            </td>
            
            <td id="tdStart" runat="server" class="col12">
                <asp:Label ID="StartLabel" runat="server" Text='<%# Eval("P_Start", "{0:d}") %>' />
            </td>
            
            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" /><br />
                <asp:Button ID="DeleteButton" runat="server" Visible="false" Text="Usuń" />
                <asp:Button ID="AnkietaButton" runat="server" Visible="false" Text="Ankieta" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" class="ListView3_edt" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="Table2" class="ListView1" runat="server">
            <tr id="trPagerTop" runat="server" visible="false">
                <td id="Td3" runat="server" class="pager pager_top" >
                    <asp:DataPager ID="DataPager2" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowNextPageButton="false" ShowLastPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td>
                </td>
            </tr>
            <tr id="Tr1" runat="server">
                <td id="Td1" colspan="2" runat="server">
                    <table ID="itemPlaceholderContainer" class="tbUczestnicy" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="NazwiskoImie" Text="Nazwisko i imię" ToolTip="Sortuj" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="NR_EW" Text="Nr ew." ToolTip="Sortuj"/></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Login" Text="Login" ToolTip="Sortuj"/></th>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Email" Text="E-mail" ToolTip="Sortuj"/></th>
                            <th id="Th5" class="col5" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Checked" Text="W" ToolTip="Sortuj"/></th>
                            <th id="Th6" class="col6" runat="server"><asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Kontroler1" Text="K1" ToolTip="Sortuj"/></th>
                            <th id="Th7" class="col7" runat="server"><asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Kontroler2" Text="K2" ToolTip="Sortuj"/></th>
                            <th id="Th8" class="col8" runat="server"><asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="Operator" Text="O" ToolTip="Sortuj"/></th>
                            <th id="Th9" class="col9" runat="server"><asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Admin" Text="A" ToolTip="Sortuj"/></th>
                            <th id="Th10" runat="server"><asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status" ToolTip="Sortuj"/></th>
                            <th id="Th11" runat="server"><asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="StanAnkiety" Text="Ankieta" ToolTip="Sortuj"/></th>
                            <th id="Th12" class="col12" runat="server"><asp:LinkButton ID="LinkButton12" runat="server" CommandName="Sort" CommandArgument="P_Start" Text="Start" ToolTip="Sortuj"/></th>
                            <th id="Th13" class="control" runat="server">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj nowy rekord"/>                            
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr3" runat="server">
                <td id="Td2" runat="server" class="pager" style="">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="pager" align="right">
                    <uc1:LetterDataPager ID="LetterDataPager1" TbName="Pracownicy" Letter="LEFT(Nazwisko,1)" runat="server" />
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit" style="">
            <td class="col1">
                <asp:TextBox ID="NazwiskoTextBox" class="textbox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd<br>" ValidationGroup="vgPracownik" ControlToValidate="NazwiskoTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBox ID="ImieTextBox" class="textbox" runat="server" Text='<%# Bind("Imie") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<br>Błąd<br>" ValidationGroup="vgPracownik" ControlToValidate="ImieTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2 right">
                <asp:Label ID="KadryIdLabel" runat="server" Text='<%# Eval("NR_EW") %>' />
            </td>
            <td class="col3">
                <asp:TextBox ID="LoginTextBox" class="textbox" runat="server" Text='<%# Bind("Login") %>' />
            </td>
            <td class="col4">
                <asp:TextBox ID="EmailTextBox" class="textbox" runat="server" Text='<%# Bind("Email") %>' />
            </td>
            
            <td class="col56789" colspan="5">
                <asp:CheckBox ID="CheckedCheckBox" runat="server" Text="Weryfikacja" Checked='<%# Bind("Checked") %>' /><br />
                <asp:CheckBox ID="KontrolerCheckBox" runat="server" Text="Kontroler 1" Checked='<%# Bind("Kontroler1") %>' /><br />
                <asp:CheckBox ID="Kontroler2CheckBox" runat="server" Text="Kontroler 2" Checked='<%# Bind("Kontroler2") %>' /><br />
                <asp:CheckBox ID="OperatorCheckBox" runat="server" Text="Operator" Checked='<%# Bind("Operator") %>' /><br />
                <asp:CheckBox ID="AdminCheckBox" runat="server" Text="Administrator" Checked='<%# Bind("Admin") %>' /><br />
            </td>
            
            <td class="col10 right">
                <asp:Label ID="StatusLabel1" runat="server" Text='<%# Eval("Status") %>' />
            </td>

            <td class="col11">
                <asp:DropDownList ID="ddlStanAnkiety" runat="server" SelectedValue='<%#Bind("StanAnkiety")%>' >
                    <asp:ListItem Value="0">Pracownik</asp:ListItem> 
                    <asp:ListItem Value="1">Kontrola</asp:ListItem>
                    <asp:ListItem Value="2">Wyjaśnić</asp:ListItem>
                    <asp:ListItem Value="3">Zatwierdzić</asp:ListItem>
                    <asp:ListItem Value="4">Operator</asp:ListItem>
                    <asp:ListItem Value="5">Wprowadzone</asp:ListItem>
                </asp:DropDownList>
            </td>

            <td id="tdStart" runat="server" class="col12">
                <div style="display: block; white-space: nowrap;">
                    <asp:TextBox ID="StartTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("P_Start", "{0:d}" ) %>' />
                    <asp:ImageButton ID="btDate1" class="ico" runat="server" ImageUrl="~/images/buttons/calendar.png" />
                    <asp:CalendarExtender ID="CalendarExtender" runat="server" 
                        TargetControlID="StartTextBox" DaysModeTitleFormat="yyyy MMMM" PopupPosition="BottomLeft" 
                        TodaysDateFormat="yyyy-MM-dd" CssClass="calendar"></asp:CalendarExtender>
                    <asp:CalendarExtender ID="CalendarExtenderButton" runat="server" 
                        TargetControlID="StartTextBox" DaysModeTitleFormat="yyyy MMMM" PopupButtonID="btDate1" 
                        TodaysDateFormat="yyyy-MM-dd" CssClass="calendar"></asp:CalendarExtender>       
                    <%-- 
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Błąd!"
                        ControlToValidate="StartTextBox" 
                        onservervalidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                    --%> 
                </div>   
            </td>

            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" /><br />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgPracownik" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit" style="">
            <td class="col1">
                <span class="t4">Nazwisko:</span><br />
                <asp:TextBox ID="NazwiskoTextBox" class="textbox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd<br>" ValidationGroup="vgPracownik" ControlToValidate="NazwiskoTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <span class="t4">Imię:</span><br />
                <asp:TextBox ID="ImieTextBox" class="textbox" runat="server" Text='<%# Bind("Imie") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<br>Błąd<br>" ValidationGroup="vgPracownik" ControlToValidate="ImieTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2 right">
            </td>
            <td class="col3">
                <asp:TextBox ID="LoginTextBox" class="textbox" runat="server" Text='<%# Bind("Login") %>' />
            </td>
            <td class="col4">
                <asp:TextBox ID="EmailTextBox" class="textbox" runat="server" Text='<%# Bind("Email") %>' />
            </td>
            
            <td class="col56789" colspan="5">
                <asp:CheckBox ID="CheckedCheckBox" runat="server" Text="Weryfikacja" Checked='<%# Bind("Checked") %>' /><br />
                <asp:CheckBox ID="KontrolerCheckBox" runat="server" Text="Kontroler 1" Checked='<%# Bind("Kontroler1") %>' /><br />
                <asp:CheckBox ID="Kontrolel2CheckBox" runat="server" Text="Kontroler 2" Checked='<%# Bind("Kontroler2") %>' /><br />
                <asp:CheckBox ID="OperatorCheckBox" runat="server" Text="Operator" Checked='<%# Bind("Operator") %>' /><br />
                <asp:CheckBox ID="AdminCheckBox" runat="server" Text="Administrator" Checked='<%# Bind("Admin") %>' /><br />
            </td>
            
            <td class="col10 right">
                <asp:Label ID="StatusLabel1" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td class="col11">
            </td>
            <td class="col12">
            </td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" /><br />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgPracownik" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 
                   RTRIM(Nazwisko) + ' ' + RTRIM(Imie) as NazwiskoImie, 
                   (case StanAnkiety 
                        when 0 then 'Pracownik' 
                        when 1 then 'Kontrola' 
                        when 2 then 'Wyjaśnić' 
                        when 3 then 'Zatwierdzić' 
                        when 4 then 'Operator' 
                        when 5 then 'Wprowadzone' 
                    end) as StanAnkiety1,
                    * FROM [Pracownicy]" 
    UpdateCommand="UPDATE [Pracownicy] SET [Login] = @Login, [Imie] = @Imie, [Nazwisko] = @Nazwisko, [Email] = @Email, [Checked] = @Checked, [Admin] = @Admin, [Kontroler1] = @Kontroler1, [Kontroler2] = @Kontroler2, [Operator] = @Operator, [StanAnkiety] = @StanAnkiety, [P_Start] = @P_Start WHERE [Id] = @Id"
    DeleteCommand="DELETE FROM [Pracownicy] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Pracownicy] ([Imie], [Nazwisko], [Login], [Email], [Checked], [Admin], [Kontroler1], [Kontroler2], [Operator]) VALUES (@Imie, @Nazwisko, @Login, @Email, @Checked, @Admin, @Kontroler1, @Kontroler2, @Operator)" >
    <UpdateParameters>
        <asp:Parameter Name="Login" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Checked" Type="Boolean" />
        <asp:Parameter Name="Admin" Type="Boolean" />
        <asp:Parameter Name="Kontroler1" Type="Boolean" />
        <asp:Parameter Name="Kontroler2" Type="Boolean" />
        <asp:Parameter Name="Operator" Type="Boolean" />        
        <asp:Parameter Name="StanAnkiety" Type="Int32" />
        <asp:Parameter Name="P_Start" Type="DateTime" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Login" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Checked" Type="Boolean" />
        <asp:Parameter Name="Admin" Type="Boolean" />
        <asp:Parameter Name="Kontroler1" Type="Boolean" />
        <asp:Parameter Name="Kontroler2" Type="Boolean" />
        <asp:Parameter Name="Operator" Type="Boolean" />        
    </InsertParameters>
</asp:SqlDataSource>

