<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSplity.ascx.cs" Inherits="HRRcp.Controls.Przypisania.cntSplity" %>
<%@ Register src="../SplitWsp.ascx" tagname="SplitWsp" tagprefix="uc3" %>
<%@ Register src="cntSplityWsp.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="../LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>

<asp:HiddenField ID="hidGrSplitu" runat="server" />

<asp:ListView ID="lvSplity" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem" 
    onitemdatabound="lvSplity_ItemDataBound" 
    onitemupdating="lvSplity_ItemUpdating" 
    ondatabinding="lvSplity_DataBinding" ondatabound="lvSplity_DataBound" 
    onitemediting="lvSplity_ItemEditing" onitemdeleted="lvSplity_ItemDeleted" 
    onitemdeleting="lvSplity_ItemDeleting">
    <ItemTemplate>
        <tr class="it">
            <td class="select">
                <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Pokaż" />
            </td>
            <td class="cc">
                <asp:Label ID="lbCC" runat="server" Text='<%# Eval("cc") %>' />
                <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="0" Type="1" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd", "{0:d}") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo", "{0:d}") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="sit">
            <td class="select">&nbsp;</td>
            <td class="cc">
                <asp:Label ID="lbCC" runat="server" Text='<%# Eval("cc") %>' />
                <uc6:cntSplityWsp ID="cntSplityWsp1sit" Mode="0" Type="1" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd", "{0:d}") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo", "{0:d}") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
        <%--
        <tr class="sit">
            <td></td>
            <td colspan="3">
                Split:<br />
                <uc3:SplitWsp ID="cntSplitWsp" runat="server" IdSplitu='<%# Eval("Id") %>' />                
            </td>
            <td></td>
        </tr>
        --%>
    </SelectedItemTemplate>
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
            <td>&nbsp;</td>
            <td class="cc">
                <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="SqlDataSource3" DataTextField="Nazwa" DataValueField="GrSplitu" 
                    OnDataBound="ddlCC_DataBound"
                    OnSelectedIndexChanged="ddlCC_SelectedIndexChanged" 
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td class="data">
                <asp:Label ID="lbDataOd" runat="server" Text='<%# Bind("DataOd", "{0:d}" ) %>' Visible="false"/>
                <uc1:DateEdit ID="deDataOd" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataDo") %>' Visible="false"/> 
            </td>
            <%--
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
            --%>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>&nbsp;</td>
            <td class="cc">
                <asp:Label ID="lbCC" runat="server" Text='<%# Eval("cc") %>' />
                <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="1" Type="1" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd", "{0:d}") %>' Visible="false"/>
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo", "{0:d}") %>' Visible="false"/>
                <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataDo") %>' />
            </td>
            <%--
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
            --%>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
        <%--
        <tr class="eit">
            <td></td>
            <td colspan="3">
                Split:<br />
                <uc3:SplitWsp ID="cntSplitWsp" runat="server" IdSplitu='<%# Eval("Id") %>' />                
            </td>
            <td></td>
        </tr>
        --%>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbSplity hoverline">
            <tr runat="server">
                <td runat="server" colspan="2">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" class="tbSplity">
                        <tr runat="server" style="">
                            <th runat="server" class="select">
                            </th>
                            <th runat="server">
                                cc</th>
                            <th runat="server">
                                Split Od</th>
                            <th runat="server">
                                Do</th>
                            <th id="Th1" runat="server" class="control">
                            </th>
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

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="
DELETE FROM [Splity] WHERE [Id] = @Id
delete from SplityWsp where IdSplitu = @Id" 
    InsertCommand="
declare @lastId int
declare @id int
set @lastId = (select top 1 Id from Splity where GrSplitu = @GrSplitu order by DataOd desc)
update Splity set DataDo = DATEADD(DAY, -1, @DataOd) where Id = @lastId
insert into [Splity] ([GrSplitu], [Nazwa], [DataOd], [DataDo], [Typ]) VALUES (@GrSplitu, @Nazwa, @DataOd, @DataDo, @Typ)
set @id = (select @@Identity)
insert into SplityWsp
select @id, IdCC, Wsp from SplityWsp where IdSplitu = @lastId"
    SelectCommand="
select S.Id, S.GrSplitu, CC.cc + ' - ' + CC.Nazwa as cc, S.DataOd, S.DataDo,
case when S.DataDo is null then 0 else 1 end as Sort
from Splity S
inner join CC on S.GrSplitu = CC.GrSplitu and CC.Grupa = 1
--order by Sort, CC.cc, S.DataOd desc
order by Sort, S.DataOd desc, CC.cc
" 
    UpdateCommand="UPDATE [Splity] SET [DataOd] = @DataOd, [DataDo] = @DataDo WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Typ" Type="Int32" DefaultValue="0"/>
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT CC.Id, CC.cc + ' - ' + CC.Nazwa as Nazwa, CC.GrSplitu FROM [CC] WHERE ([Grupa] = @Grupa)">
    <SelectParameters>
        <asp:Parameter DefaultValue="true" Name="Grupa" Type="Boolean" />
    </SelectParameters>
</asp:SqlDataSource>




<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConflictDetection="CompareAllValues" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Splity] WHERE [Id] = @original_Id AND [GrSplitu] = @original_GrSplitu AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [DataOd] = @original_DataOd AND (([DataDo] = @original_DataDo) OR ([DataDo] IS NULL AND @original_DataDo IS NULL)) AND [Typ] = @original_Typ" 
    InsertCommand="INSERT INTO [Splity] ([GrSplitu], [Nazwa], [DataOd], [DataDo], [Typ]) VALUES (@GrSplitu, @Nazwa, @DataOd, @DataDo, @Typ)" 
    OldValuesParameterFormatString="original_{0}" 
    SelectCommand="SELECT *, case when GrSplitu = 9999 then 1 else 2 end as Sort FROM [Splity] order by Sort, DataOd desc, Nazwa" 
    UpdateCommand="UPDATE [Splity] SET [GrSplitu] = @GrSplitu, [Nazwa] = @Nazwa, [DataOd] = @DataOd, [DataDo] = @DataDo, [Typ] = @Typ WHERE [Id] = @original_Id AND [GrSplitu] = @original_GrSplitu AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [DataOd] = @original_DataOd AND (([DataDo] = @original_DataDo) OR ([DataDo] IS NULL AND @original_DataDo IS NULL)) AND [Typ] = @original_Typ">
    <DeleteParameters>
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_GrSplitu" Type="Int32" />
        <asp:Parameter Name="original_Nazwa" Type="String" />
        <asp:Parameter Name="original_DataOd" Type="DateTime" />
        <asp:Parameter Name="original_DataDo" Type="DateTime" />
        <asp:Parameter Name="original_Typ" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_GrSplitu" Type="Int32" />
        <asp:Parameter Name="original_Nazwa" Type="String" />
        <asp:Parameter Name="original_DataOd" Type="DateTime" />
        <asp:Parameter Name="original_DataDo" Type="DateTime" />
        <asp:Parameter Name="original_Typ" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Typ" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

