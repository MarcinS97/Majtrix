<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="x_cntPracownicy.ascx.cs" Inherits="HRRcp.Controls.x_cntPracownicy" %>
<%@ Register src="../LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>

<asp:ListView ID="lvPracownicy" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="None" 
    onitemcommand="lvPracownicy_ItemCommand" 
    onitemdatabound="lvPracownicy_ItemDataBound" 
    onitemcreated="lvPracownicy_ItemCreated" 
    onlayoutcreated="lvPracownicy_LayoutCreated" 
    onsorting="lvPracownicy_Sorting" 
    ondatabound="lvPracownicy_DataBound" 
    oniteminserted="lvPracownicy_ItemInserted" 
    oniteminserting="lvPracownicy_ItemInserting" 
    onitemupdated="lvPracownicy_ItemUpdated" 
    onitemupdating="lvPracownicy_ItemUpdating" 
    OnSelectedIndexChanged="lvPracownicy_SelectedChanged"
    onpagepropertieschanged="lvPracownicy_PagePropertiesChanged">
    <ItemTemplate>
        <tr style="">
            <td id="tdSelect" runat="server" class="control1" visible="false">
                <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" />
            </td>
            
            <td class="nazwisko_nrew">
                <asp:Label ID="KierownikLabel" runat="server" Text='<%# PrepNazwisko(Eval("Pracownik")) %>' /><br />
                <asp:Label ID="Nr_EwidLabel" CssClass="line2" runat="server" Text='<%# Eval("KadryId") %>' />
            </td>

            <td class="check">
                <asp:CheckBox ID="IsKierCheckBox" class="check" runat="server" Checked='<%# Eval("Kierownik") %>' Enabled="false" />
            </td>

            <td id="tdMailing" runat="server" class="check">
                <asp:CheckBox ID="cbR1" runat="server" Enabled="false" />
            </td>

            <td id="tdContact" class="mail" runat="server">
                <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email") %>' /><br />
                <asp:Label ID="LoginLabel" CssClass="line2" runat="server" Text='<%# Eval("Login") %>' />
            </td>

            <td class="nazwisko_nrew">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("KierownikNI") %>' /><br />
                <asp:Label ID="Label2" runat="server" CssClass="line2" Text='<%# Eval("KierKadryId") %>' />
            </td>

            <td class="dzial">
                <asp:Label ID="DzialNameLabel" runat="server" Text='<%# Eval("Dzial") %>' /><br />
                <asp:Label ID="StanowiskoLabel" CssClass="line2" runat="server" Text='<%# Eval("Stanowisko") %>' />
            </td>

            <td id="tdR2" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR2" runat="server" Enabled="false" />
            </td>
            <td id="tdR3" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR3" runat="server" Enabled="false" />
            </td>
            <td id="tdR4" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR4" runat="server" Enabled="false" />
            </td>
            <td id="tdR5" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR5" runat="server" Enabled="false" />
            </td>
            <td id="tdR6" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR6" runat="server" Enabled="false" />
            </td>
            <td id="tdR7" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR7" runat="server" Enabled="false" />
            </td>
            <td id="tdR8" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR8" runat="server" Enabled="false" />
            </td>
            <td id="tdR9" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR9" runat="server" Enabled="false" />
            </td>
            <td id="tdR10" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR10" runat="server" Enabled="false" />
            </td>
            <td id="tdR11" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR11" runat="server" Enabled="false" />
            </td>
            <td id="tdR12" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR12" runat="server" Enabled="false" />
            </td>
            <td id="tdR13" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR13" runat="server" Enabled="false" />
            </td>
            <td id="tdR14" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR14" runat="server" Enabled="false" />
            </td>
            <td id="tdR15" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR15" runat="server" Enabled="false" />
            </td>
            <td id="tdR16" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR16" runat="server" Enabled="false" />
            </td>
            <td id="tdR17" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR17" runat="server" Enabled="false" />
            </td>
            <td id="tdR18" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR18" runat="server" Enabled="false" />
            </td>
            <td id="tdR19" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR19" runat="server" Enabled="false" />
            </td>
            <td id="tdR20" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR20" runat="server" Enabled="false" />
            </td>
            <td id="tdR21" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR21" runat="server" Enabled="false"  />
            </td>
            <td class="status">
                <asp:Label ID="lbStatus" runat="server" Text='<%# GetStatus(Eval("Status")) %>' /><br />
            </td>
            <td id="tdControl" runat="server" class="control">
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
    <EditItemTemplate>
        <tr class="eit">
            <td colspan="<%= GetEditColSpan() %>">
                <table class="table0">
                    <tr>
                        <td class="col1">
                            <div class="title">Dane podstawowe</div>
                            <span class="col1">Nazwisko:</span>
                            <asp:TextBox ID="KierownikTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                            <span class="col1">Imię:</span>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Imie") %>' /><br />
                            <span class="col1">Nr ewid.:</span>
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="nrew" Text='<%# Bind("KadryId") %>' /><br />

                            <span class="col1">e-mail:</span>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Email") %>' /><br />
                            <span class="col1">Mailing:</span>
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" /><br />
                            
                            <span class="col1">Login:</span>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Login") %>' /><br />

                        </td>
                        <td class="col2">
                            <div class="title">Organizacja</div>
                            <span class="col1">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" /><br />
                        </td>
                        <td class="col3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" /><br />
                            </div>
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                        </td>                                                        
                    </tr>                                                            
                </table>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" /><br />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td colspan="<%= GetEditColSpan() %>">
                <table class="table0">
                    <tr>
                        <td class="col1">
                            <div class="title">Dane podstawowe</div>
                            <span class="col1">Nazwisko:</span>
                            <asp:TextBox ID="KierownikTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                            <span class="col1">Imię:</span>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Imie") %>' /><br />
                            <span class="col1">Nr ewid.:</span>
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="nrew" Text='<%# Bind("KadryId") %>' /><br />

                            <span class="col1">e-mail:</span>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Email") %>' /><br />
                            <span class="col1">Mailing:</span>
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" /><br />
                            
                            <span class="col1">Login:</span>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Login") %>' /><br />

                        </td>
                        <td class="col2">
                            <div class="title">Organizacja</div>
                            <span class="col1">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" /><br />
                        </td>
                        <td class="col3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" /><br />
                            </div>
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                        </td>
                    </tr>
                </table>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Zapisz" /><br />
                <asp:Button ID="Button1" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
        </tr>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 narrow">
            <tr id="Tr1" runat="server">
                <td id="Td1" colspan="2" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" class="tbPracownicy">
                        <tr id="Tr2" runat="server" style="">
                            <%--
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton16" runat="server" CommandName="Sort" CommandArgument="Pracownik" Text="Pracownik" ToolTip="Sortuj" /> /
                                                        <asp:LinkButton ID="LinkButton17" runat="server" CommandName="Sort" CommandArgument="KadryId" Text="Nr ew." ToolTip="Sortuj"/></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton18" runat="server" CommandName="Sort" CommandArgument="Kierownik" Text="Kier" ToolTip="Sortuj" /></th>

                            <th id="Th9" runat="server"><asp:LinkButton ID="LinkButton22" runat="server" CommandName="Sort" CommandArgument="LiniaNazwa" Text="Commodity" ToolTip="Sortuj"/> /
                                                        <asp:LinkButton ID="LinkButton23" runat="server" CommandName="Sort" CommandArgument="KierownikNI" Text="Kierownik" ToolTip="Sortuj"/></th>
                            <th id="Th7" runat="server"><asp:LinkButton ID="LinkButton24" runat="server" CommandName="Sort" CommandArgument="Dzial" Text="Dział" ToolTip="Sortuj"/> /
                                                        <asp:LinkButton ID="LinkButton25" runat="server" CommandName="Sort" CommandArgument="Stanowisko" Text="Stanowisko" ToolTip="Sortuj"/></th>
                            <th id="Th10" runat="server"><asp:LinkButton ID="LinkButton26" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status" ToolTip="Sortuj"/></th>
                            
                            <th id="Th111" class="col11" runat="server"><asp:LinkButton ID="LinkButton27" runat="server" CommandName="Sort" CommandArgument="Admin" Text="A" ToolTip="Administrator - Sortuj"/></th>
                            <th id="Th112" class="col11" runat="server"><asp:LinkButton ID="LinkButton28" runat="server" CommandName="Sort" CommandArgument="Raporty" Text="R" ToolTip="Dostęp do raportów - Sortuj"/></th>
                            <th id="Th113" class="col11" runat="server"><asp:LinkButton ID="LinkButton29" runat="server" CommandName="Sort" CommandArgument="Mailing" Text="@" ToolTip="Wysyłanie powiadomień - Sortuj"/></th>
                            --%>
                        
                        
                        
                        
                            
                            <th id="thSelect" rowspan="2" class="control1" runat="server" visible="false"></th>

                            <th id="Th4" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton41" runat="server" CommandName="Sort" CommandArgument="Pracownik" Text="Pracownik" ToolTip="Sortuj" /> /
                                <asp:LinkButton ID="LinkButton42" runat="server" CommandName="Sort" CommandArgument="KadryId" Text="Nr ew." ToolTip="Sortuj"/></th>
                            <th id="Th5" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton43" runat="server" CommandName="Sort" CommandArgument="Kierownik" Text="Kier" ToolTip="Sortuj" /></th>

                            
                            
                            <th id="thMailing" class="check" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton1" runat="server" >@</asp:LinkButton></th>
                            <th id="thContact" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton36" runat="server" CommandName="Sort" CommandArgument="Email" Text="E-mail" ToolTip="Sortuj" /> /
                                <asp:LinkButton ID="LinkButton34" runat="server" CommandName="Sort" CommandArgument="Login" Text="Login" ToolTip="Sortuj"/></th>
                            
                            <th id="Th9" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton39" runat="server" CommandName="Sort" CommandArgument="KierownikNI" Text="Kierownik" ToolTip="Sortuj"/> /
                                <asp:LinkButton ID="LinkButton40" runat="server" CommandName="Sort" CommandArgument="KierKadryId" Text="Nr ewid." ToolTip="Sortuj"/></th>

                            <th id="Th1" rowspan="2" runat="server">
                                <asp:LinkButton ID="LinkButton37" runat="server" CommandName="Sort" CommandArgument="Dzial" Text="Dział" ToolTip="Sortuj"/> /
                                <asp:LinkButton ID="LinkButton38" runat="server" CommandName="Sort" CommandArgument="Stanowisko" Text="Stanowisko" ToolTip="Sortuj"/></th>
                                                                        
                            <th id="thRights" colspan="15" class="top" runat="server" visible="false">
                                Uprawnienia</th>
                            <th id="Th3" rowspan="2" runat="server">
                                Status</th>
                                
                            <th id="thControl" rowspan="2" class="control" runat="server">
                                <asp:Button ID="btNewRecord" runat="server" Text="Dodaj" CommandName="NewRecord" />
                            </th>
                        </tr>
                        <tr class="bottom">
                            <th id="thR2"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton2"  runat="server"></asp:LinkButton></th>
                            <th id="thR3"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton3"  runat="server"></asp:LinkButton></th>
                            <th id="thR4"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton4"  runat="server"></asp:LinkButton></th>
                            <th id="thR5"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton5"  runat="server"></asp:LinkButton></th>
                            <th id="thR6"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton6"  runat="server"></asp:LinkButton></th>
                            <th id="thR7"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton7"  runat="server"></asp:LinkButton></th>
                            <th id="thR8"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton8"  runat="server"></asp:LinkButton></th>
                            <th id="thR9"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton9"  runat="server"></asp:LinkButton></th>
                            <th id="thR10" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton10" runat="server"></asp:LinkButton></th>
                            <th id="thR11" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton11" runat="server"></asp:LinkButton></th>
                            <th id="thR12" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton12" runat="server"></asp:LinkButton></th>
                            <th id="thR13" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton13" runat="server"></asp:LinkButton></th>
                            <th id="thR14" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton14" runat="server"></asp:LinkButton></th>
                            <th id="thR15" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton15" runat="server"></asp:LinkButton></th>
                            <th id="thR16" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton16" runat="server"></asp:LinkButton></th>
                            <th id="thR17" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton17" runat="server"></asp:LinkButton></th>
                            <th id="thR18" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton18" runat="server"></asp:LinkButton></th>
                            <th id="thR19" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton19" runat="server"></asp:LinkButton></th>
                            <th id="thR20" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton20" runat="server"></asp:LinkButton></th>
                            <th id="thR21" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton21" runat="server"></asp:LinkButton></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr3" class="pager" runat="server">
                <td id="Td2" class="left" runat="server">
                    <uc1:LetterDataPager ID="LetterDataPager1" TbName="Pracownicy" Letter="LEFT(Nazwisko,1)" runat="server" />
                </td>
                <td class="right">
                    <h3>Pokaż na stronie:&nbsp;&nbsp;&nbsp;</h3>
                    <asp:DropDownList ID="ddlLines" runat="server" >
                    </asp:DropDownList>
                    <%--
                    <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true"    
                        OnChange="showAjaxProgress();"
                        OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                        <asp:ListItem Text="15" Value="15" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                    --%>
                </td>
            </tr>
            <tr class="pager">
                <td>
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Pracownicy] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Pracownicy] ([Nazwisko], [Imie], [KadryId], [Email], [Login], [Rights], [Status]) VALUES (@Nazwisko, @Imie, @KadryId, @Email, @Login, @Rights, @Status)" 
    UpdateCommand="
        if exists (select * from strPracownicyAttr where IdPracownika = @Id)
            UPDATE strPracownicyAttr set Rights = @Rights where IdPracownika = @Id
        else
            INSERT INTO strPracownicyAttr (IdPracownika, Rights) VALUES (@Id, @Rights)" 
    SelectCommand="select 
            P.Id,P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId,	        
	        K.Nazwisko + ' ' + K.Imie as KierownikNI, P.KadryId as KierKadryId,	        
	        P.Nazwisko,P.Imie,
	        P.Login,P.Email,A.Rights,P.Kierownik,
	        P.Status,
            D.Nazwa as Dzial, S.Nazwa as Stanowisko
        from Pracownicy P
        left outer join Pracownicy K on K.Id = P.IdKierownika
        left outer join HR_DB..Dzialy D on D.Id = P.IdDzialu
        left outer join HR_DB..Stanowiska S on S.Id = P.IdStanowiska
        left outer join strPracownicyAttr A on A.IdPracownika = P.Id
        order by Pracownik"
    >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Rights" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="KadryId" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Login" Type="String" />
        <asp:Parameter Name="Rights" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>



<!--
    SelectCommand="select 
            K.Id_Przelozeni,
	        K.Nazwisko + ' ' + K.Imie as Kierownik,
	        K.Imie2, 
	        K.Nr_Ewid,
	        K.Login, 
	        K.Password,
        	
	        K.Email,
	        K.Telefon,

	        K.Id_Priorytet,
	        P.Priorytet,
	        P.Opis,

	        K.Blokada,
	        K.Rights,
        	
	        K.Id_Gr_zatr,
	        G.Rodzaj_Umowy,
        	
	        K.Id_Str_Org,
	        O.Symb_Jedn,
	        O.Nazwa_Jedn,
	        O.ID_Upr_Przel,
	        O.Id_Parent,

	        K.Id_Stanowiska,
	        S.Nazwa_Stan
        from PrzelozżeniN K
        left outer join GrZatr G on G.Id_Gr_Zatr = K.Id_Gr_zatr
        left outer join Stanowiska S on S.Id_Stanowiska = K.Id_Stanowiska
        left outer join StrOrg O on O.Id_Str_Org = K.Id_Str_Org
        left outer join Priorytet P on P.Id_Piorytet = K.Id_Priorytet"    
-->