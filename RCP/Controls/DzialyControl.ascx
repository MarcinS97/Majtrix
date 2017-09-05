<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DzialyControl.ascx.cs" Inherits="HRRcp.Controls.DzialyControl" %>

<%@ Register src="Adm/cntStanowiskaAdm.ascx" tagname="cntStanowiskaAdm" tagprefix="uc1" %>

<asp:ListView ID="lvDzialy" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" 
    InsertItemPosition="LastItem" 
    onitemdeleting="lvDzialy_ItemDeleting" 
    oniteminserting="lvDzialy_ItemInserting" 
    onitemupdating="lvDzialy_ItemUpdating" 
    onitemdeleted="lvDzialy_ItemDeleted" oniteminserted="lvDzialy_ItemInserted" 
    onitemupdated="lvDzialy_ItemUpdated">
    <%--
    onlayoutcreated="lvDzialy_LayoutCreated" 
    onsorting="lvDzialy_Sorting" 
    --%>
    <ItemTemplate>
        <tr class="it">
            <td class="nazwa">
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <%--
            <td class="cc">
                <asp:Label ID="KierCCInfoLabel" runat="server" Text='< %# Eval("KierCCInfo") %>' />
            </td>
            --%>
            <td class="strefa">
                <asp:Label ID="KierStrefaIdLabel" runat="server" Text='<%# Eval("KierStrefa") %>' /><br />
                <asp:Label ID="KierAlgorytmLabel" runat="server" CssClass="line2" Text='<%# Eval("KierAlgorytmNazwa") %>' />
            </td>
            <%--
            <td class="cc">
                <asp:Label ID="PracCCInfoLabel" runat="server" Text='< %# Eval("PracCCInfo") %>' />
            </td>
            --%>
            <td class="strefa">
                <asp:Label ID="PracStrefaIdLabel" runat="server" Text='<%# Eval("PracStrefa") %>' /><br />
                <asp:Label ID="PracAlgorytmLabel" runat="server" CssClass="line2" Text='<%# Eval("PracAlgorytmNazwa") %>' />
            </td>
            
            <td id="tdStatus" runat="server" class="status" visible="false">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>
            <td id="tdAktywny" runat="server" class="check">
                <asp:CheckBox ID="cbAktywny" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
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
            <td>
                <span class="label">Nazwa:</span>
                <asp:TextBox ID="NazwaTextBox" runat="server" CssClass="textbox" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" CssClass="error" ValidationGroup="vgi" 
                    ControlToValidate="NazwaTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td colspan="1">
                <%--
                <span class="label">CC:</span>
                <asp:TextBox ID="KierCCInfoTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='<%# Bind("KierCCInfo") %>' /><br />
                --%>
                <span class="label">Strefa RCP:</span>
                <asp:DropDownList ID="ddlKierStrefa" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic" CssClass="error" ValidationGroup="vgi" 
                    ControlToValidate="ddlKierStrefa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <br />
                <span class="label">Algorytm:</span>
                <asp:DropDownList ID="ddlKierAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic" CssClass="error" ValidationGroup="vgi" 
                    ControlToValidate="ddlKierAlgorytm" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td colspan="1">
                <%--
                <span class="label">CC:</span>
                <asp:TextBox ID="PracCCInfoTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='<%# Bind("PracCCInfo") %>' /><br />
                --%>
                <span class="label">Strefa RCP:</span>
                <asp:DropDownList ID="ddlPracStrefa" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic" CssClass="error" ValidationGroup="vgi" 
                    ControlToValidate="ddlPracStrefa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <br />
                <span class="label">Algorytm:</span>
                <asp:DropDownList ID="ddlPracAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" SetFocusOnError="True" Display="Dynamic" CssClass="error" ValidationGroup="vgi" 
                    ControlToValidate="ddlPracAlgorytm" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>                
            </td>
            <td id="tdStatus" runat="server" visible="false">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>
            <td id="tdAktywny" runat="server" class="check">
                <asp:CheckBox ID="cbAktywny" runat="server" Checked="true" />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgi"/><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Wyczyść" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="nazwa">
                <span class="label">&nbsp;</span>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" 
                    ControlToValidate="NazwaTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td colspan="1" class="dane">
<%--                <span class="label">CC:</span>
                <asp:TextBox ID="KierCCInfoTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='< %# Bind("KierCCInfo") %>' /><br />
--%>
                <span class="label">Strefa RCP:</span>
                <asp:DropDownList ID="ddlKierStrefa" runat="server" 
                    SelectedValue='<%# Bind("KierStrefaId") %>' 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" 
                    ControlToValidate="ddlKierStrefa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <br />
                <span class="label">Algorytm:</span>
                <asp:DropDownList ID="ddlKierAlgorytm" runat="server" 
                    SelectedValue='<%# Bind("KierAlgorytm") %>' 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" 
                    ControlToValidate="ddlKierAlgorytm" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td colspan="1" class="dane">
<%--                <span class="label">CC:</span>
                <asp:TextBox ID="PracCCInfoTextBox" runat="server" CssClass="textbox" MaxLength="200" Text='< %# Bind("PracCCInfo") %>' /><br />
--%>
                <span class="label">Strefa RCP:</span>
                <asp:DropDownList ID="ddlPracStrefa" runat="server" 
                    SelectedValue='<%#Bind("PracStrefaId")%>' 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" 
                    ControlToValidate="ddlPracStrefa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <br />
                <span class="label">Algorytm:</span>
                <asp:DropDownList ID="ddlPracAlgorytm" runat="server" 
                    SelectedValue='<%# Bind("PracAlgorytm") %>' 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="Kod">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="vge" 
                    ControlToValidate="ddlPracAlgorytm" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td id="tdStatus" runat="server" class="status" visible="false">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>
            <td id="tdAktywny" runat="server" class="check">
                <asp:CheckBox ID="cbAktywny" runat="server" Checked='<%# Eval("Aktywny") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbDzialy" runat="server" border="0" >
                        <tr runat="server">
                            <th rowspan="2"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Nazwa"  Text="Nazwa"        ToolTip="Sortuj" /></th>
                            <th colspan="1" class="title">Kierownicy</th>
                            <th colspan="1" class="title">Pracownicy</th>
                            <th rowspan="2">
                                <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status" Visible="false" ToolTip="Sortuj" />
                                <asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Aktywny" Text="Aktywny" ToolTip="Sortuj" />
                            </th>
                            <th rowspan="2" class="control" runat="server"></th>
                        </tr>
                        <tr runat="server">                    
                            <%--                            
                            <th><asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="KierCCInfo"         Text="CC"           ToolTip="Centrum kosztowe - Sortuj" /></th>
                            --%>                    
                            <th><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="KierStrefa"         Text="Strefa RCP"   ToolTip="Sortuj" /> /
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="KierAlgorytmNazwa"  Text="Algorytm"     ToolTip="Sortuj" /></th>
                            <%--                            
                            <th><asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="PracCCInfo"         Text="CC"           ToolTip="Centrum kosztowe - Sortuj" /></th>
                            --%>
                            <th><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="PracStrefa"         Text="Strefa RCP"   ToolTip="Sortuj" /> /
                                <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="PracAlgorytmNazwa"  Text="Algorytm"     ToolTip="Sortuj" /></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" ></asp:DropDownList>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                </td>
                <td class="right">
                    <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Dzialy] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Dzialy] ([Nazwa], [KierCCInfo], [KierStrefaId], [PracCCInfo], [PracStrefaId], [KierAlgorytm], [PracAlgorytm], Status) VALUES (@Nazwa, @KierCCInfo, @KierStrefaId, @PracCCInfo, @PracStrefaId, @KierAlgorytm, @PracAlgorytm, @Status)" 
    SelectCommand="select D.Id, D.Nazwa, D.KierCCInfo, D.KierStrefaId, D.KierAlgorytm, D.PracCCInfo, D.PracStrefaId, D.PracAlgorytm,
	                    K.Nazwa as KierStrefa,
	                    AK.Nazwa as KierAlgorytmNazwa,
	                    P.Nazwa as PracStrefa, 
	                    AP.Nazwa as PracAlgorytmNazwa,
	                    D.Status,
	                    cast(case when D.Status &gt;= 0 then 1 else 0 end as bit) as Aktywny
                    from Dzialy D
                    left outer join Strefy K on K.Id = D.KierStrefaId
                    left outer join Strefy P on P.Id = D.PracStrefaId
                    left outer join Kody AK on AK.Kod = D.KierAlgorytm and AK.Typ='ALG'
                    left outer join Kody AP on AP.Kod = D.PracAlgorytm and AP.Typ='ALG'
                    order by D.Nazwa" 
    UpdateCommand="UPDATE [Dzialy] SET [Nazwa] = @Nazwa, [KierCCInfo] = @KierCCInfo, [KierStrefaId] = @KierStrefaId, [PracCCInfo] = @PracCCInfo, [PracStrefaId] = @PracStrefaId, [KierAlgorytm] = @KierAlgorytm, [PracAlgorytm] = @PracAlgorytm, Status=@Status WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="KierCCInfo" Type="String" />
        <asp:Parameter Name="KierStrefaId" Type="Int32" />
        <asp:Parameter Name="PracCCInfo" Type="String" />
        <asp:Parameter Name="PracStrefaId" Type="Int32" />
        <asp:Parameter Name="KierAlgorytm" Type="Int32" />
        <asp:Parameter Name="PracAlgorytm" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="KierCCInfo" Type="String" />
        <asp:Parameter Name="KierStrefaId" Type="Int32" />
        <asp:Parameter Name="PracCCInfo" Type="String" />
        <asp:Parameter Name="PracStrefaId" Type="Int32" />
        <asp:Parameter Name="KierAlgorytm" Type="Int32" />
        <asp:Parameter Name="PracAlgorytm" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT [Id], [Nazwa] FROM [Strefy] WHERE ([Aktywna] = 1) 
                    union 
                    select null, ' wybierz ...'
                    ORDER BY [Nazwa]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT [Id], [Nazwa], [Lp], [Kod], [Parametr] FROM [Kody] WHERE ([Typ] = 'ALG') 
                    union 
                    select null, ' wybierz ...', 0 as Lp, null as Kod, null as Parametr                   
                    ORDER BY [Lp]">
</asp:SqlDataSource>


<br />
<span><b>Stanowiska:</b></span>
<br />
<uc1:cntStanowiskaAdm ID="cntStanowiskaAdm1" runat="server" />

