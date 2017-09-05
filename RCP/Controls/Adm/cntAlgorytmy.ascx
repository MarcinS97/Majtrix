<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAlgorytmy.ascx.cs" Inherits="HRRcp.Controls.Adm.cntAlgorytmy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="../TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidPracId" runat="server" />

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="None" 
    onitemcreated="ListView1_ItemCreated" 
    onitemdatabound="ListView1_ItemDataBound" onitemdeleted="ListView1_ItemDeleted" 
    oniteminserted="ListView1_ItemInserted" onitemupdated="ListView1_ItemUpdated" 
    onlayoutcreated="ListView1_LayoutCreated" 
    oniteminserting="ListView1_ItemInserting" 
    onitemupdating="ListView1_ItemUpdating" ondatabound="ListView1_DataBound">
    <ItemTemplate>
        <tr class="it">
            <td class="date">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="date">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <%--
            <td id="tdStrefa" runat="server" class="strefa" >
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Strefa") %>' />
            </td>
            --%>
            <td id="tdAlgorytm" runat="server" class="algorytm">
                <asp:Label ID="AlgorytmLabel" runat="server" Text='<%# Eval("Algorytm") %>' />
            </td>
            <td class="wymiar">
                <asp:Label ID="lbWymiar" runat="server" Text='<%# Eval("WymiarHMM") %>' />
            </td>
            <td class="wymiar">
                <asp:Label ID="lbPrzerwaWliczona" runat="server" Text='<%# Eval("PrzerwaWliczonaHMM") %>' />
            </td>
            <td class="wymiar">
                <asp:Label ID="lbPrzerwaNiewliczona" runat="server" Text='<%# Eval("PrzerwaniewliczonaHMM") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak danych
                    <br />
                    <br />
                    <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />                  
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr style="" class="iit">
            <td class="date">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="vgi" />
            </td>
            <td class="date">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <%--
            <td id="tdStrefa" runat="server" class="strefa" >
                <asp:DropDownList ID="ddlStrefa" runat="server" 
                    DataSourceID="SqlDataSource4"
                    DataTextField="Nazwa"
                    DataValueField="RcpStrefaId">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlStrefa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            --%>
            <td id="tdAlgorytm" runat="server" class="algorytm" >
                <asp:DropDownList ID="ddlAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="RcpAlgorytm">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlAlgorytm" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="wymiar">
                <uc1:TimeEdit ID="teWymiar" runat="server" Seconds='<%# Bind("WymiarCzasu") %>' />
            </td>
            <td class="wymiar">
                <uc1:TimeEdit ID="tePrzerwaWliczona" runat="server" Seconds='<%# Bind("PrzerwaWliczona") %>' />
            </td>
            <td class="wymiar">
                <uc1:TimeEdit ID="tePrzerwaNiewliczona" runat="server" Seconds='<%# Bind("PrzerwaNiewliczona") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                <asp:Button ID="btCancelInsert" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr style="" class="eit">
            <td class="date">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="vge"/>
            </td>
            <td class="date">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <%--
            <td id="tdStrefa" runat="server" class="strefa">
                <asp:DropDownList ID="ddlStrefa" runat="server" 
                    DataSourceID="SqlDataSource4"
                    DataTextField="Nazwa"
                    DataValueField="RcpStrefaId">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge"                     
                    ControlToValidate="ddlStrefa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            --%>
            <td id="tdAlgorytm" runat="server" class="algorytm">
                <asp:DropDownList ID="ddlAlgorytm" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="RcpAlgorytm">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge"                     
                    ControlToValidate="ddlAlgorytm" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="wymiar">
                <uc1:TimeEdit ID="teWymiar" runat="server" Seconds='<%# Bind("WymiarCzasu") %>' />
            </td>
            <td class="wymiar">
                <uc1:TimeEdit ID="tePrzerwaWliczona" runat="server" Seconds='<%# Bind("PrzerwaWliczona") %>' />
            </td>
            <td class="wymiar">
                <uc1:TimeEdit ID="tePrzerwaNiewliczona" runat="server" Seconds='<%# Bind("PrzerwaNiewliczona") %>' />
            </td>
            
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="vge"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbPracParametry tbAlgorytmyRCP">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Od</th>
                            <th runat="server">
                                Do</th>
                            <%--
                            <th id="thStrefa" runat="server">
                                Strefa</th>
                            --%>    
                            <th id="thAlgorytm" runat="server">
                                Algorytm</th>
                            <th id="thWymiar" runat="server">
                                Wymiar czasu pracy<br />
                                <span class="t4n">hh:mm</span>
                            </th>
                            <th id="th1" runat="server">
                                Przerwa wliczona<br />
                                <span class="t4n">hh:mm</span>
                            </th>
                            <th id="th2" runat="server">
                                Przerwa niewliczona<br />
                                <span class="t4n">hh:mm</span>
                            </th>
                            <th runat="server" class="control">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />                  
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager1" runat="server" class="pager">
                <td class="left">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="5">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                    <%--
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" ></asp:DropDownList>
                    --%>
                </td>
            </tr>
            <%--                    
            <tr id="trPager2" runat="server" class="pager">
                <td class="left">
                </td>
                <td class="right">
                    <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
            --%>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [PracownicyParametry] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PracownicyParametry] ([IdPracownika], [Od], [Do], [RcpAlgorytm]
                    ,PrzerwaWliczona, PrzerwaNiewliczona
                    ,WymiarCzasu
                    --,RcpStrefaId
                    ) VALUES (@IdPracownika, @Od, @Do, @RcpAlgorytm
                    ,ISNULL(@PrzerwaWliczona, 0), ISNULL(@PrzerwaNiewliczona, 0)
                    ,ISNULL(@WymiarCzasu, 28800)
                    --,@RcpStrefaId
                    )" 
    SelectCommand="
SELECT R.*, K.Nazwa as Algorytm
    ,R.PrzerwaWliczona,    dbo.ToTimeHMM(R.PrzerwaWliczona) as PrzerwaWliczonaHMM
    ,R.PrzerwaNiewliczona, dbo.ToTimeHMM(R.PrzerwaNiewliczona) as PrzerwaNiewliczonaHMM
    ,R.WymiarCzasu,        dbo.ToTimeHMM(R.WymiarCzasu) as WymiarHMM
    --,S.Nazwa as Strefa 
FROM PracownicyParametry R 
left outer join Kody K on K.Typ = 'ALG' and K.Kod = R.RcpAlgorytm
--left outer join Strefy S on S.Id = R.RcpStrefaId
WHERE R.IdPracownika = @IdPracownika
ORDER BY R.Od DESC" 
    UpdateCommand="UPDATE [PracownicyParametry] SET [IdPracownika] = @IdPracownika, [Od] = @Od, [Do] = @Do, [RcpAlgorytm] = @RcpAlgorytm
                    ,PrzerwaWliczona = ISNULL(@PrzerwaWliczona, 0)
                    ,PrzerwaNiewliczona = ISNULL(@PrzerwaNiewliczona, 0)
                    ,WymiarCzasu = ISNULL(@WymiarCzasu, 28800)
                    --,RcpStrefaId = @RcpStrefaId 
                    WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="RcpAlgorytm" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="WymiarCzasu" Type="Int32" />
        <asp:Parameter Name="PrzerwaWliczona" Type="Int32" />
        <asp:Parameter Name="PrzerwaNiewliczona" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="RcpAlgorytm" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="WymiarCzasu" Type="Int32" />
        <asp:Parameter Name="PrzerwaWliczona" Type="Int32" />
        <asp:Parameter Name="PrzerwaNiewliczona" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT 1 as Sort, [Id], [Nazwa], [Lp], [Kod] as RcpAlgorytm, [Parametr] FROM [Kody] WHERE ([Typ] = 'ALG') 
union 
select 0 as Sort, null as Id, 'wybierz ...' as Nazwa, 0 as Lp, null as RcpAlgorytm, null as Parametr                   
ORDER BY Sort, [Lp]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as RcpStrefaId, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Nazwa, 1 as Sort from Strefy where Aktywna = 1 
order by Sort, Nazwa
    ">
</asp:SqlDataSource>
