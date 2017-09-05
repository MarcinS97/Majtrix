<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWariantyAdm.ascx.cs" Inherits="HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe.cntWariantyAdm" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>

<div id="ctWariantyAdm" runat="server" class="cntWariantyAdm">


    <div class="input-group">
        <span class="input-group-addon">Rodzaj lokalu</span>
        <asp:DropDownList ID="ddlRodzaje" runat="server" DataSourceID="dsRodzaje" DataValueField="Value" DataTextField="Text" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRodzaje_SelectedIndexChanged" />
        <asp:SqlDataSource ID="dsRodzaje" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
            SelectCommand="select null Value, 'wybierz ...' Text, 0 Sort union all select Id Value, Nazwa Text, 1 Sort from poWnioskiMajatkoweLokalRodzaje order by Sort, Text" />
    </div>

    <hr />

    <h3 runat="server" id="notSelectedText">Wybierz rodzaj lokalu...</h3>

    <asp:ListView ID="lvWarianty" runat="server" DataKeyNames="Id" DataSourceID="dsList" EnableModelValidation="True" InsertItemPosition="LastItem">
        <EditItemTemplate>
            <tr style="">
                <%--<td>
                    <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                </td>--%>
                <td>
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' Width="100px" CssClass="form-control" />
                </td>
                <td>
                    <%--<asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("DateFrom") %>' CssClass="form-control" />--%>
                    <cc:DateEdit ID="deDateFrom" runat="server" Date='<%# Bind("Od") %>' />
                </td>
                <td>
                    <%--<asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("DateTo") %>' CssClass="form-control" />--%>
                    <cc:DateEdit ID="deDateTo" runat="server" Date='<%# Bind("Do") %>' />
                </td>
                <%--<td>
                    <asp:TextBox ID="GrupaTextBox" runat="server" Text='<%# Bind("Grupa") %>' />
                </td>--%>
                <td>
                    <asp:TextBox ID="SumaTextBox" runat="server" Text='<%# Bind("Suma") %>' Width="100px" CssClass="form-control" />
                </td>
                <td>
                    <asp:TextBox ID="SumaPlusTextBox" runat="server" Text='<%# Bind("SumaPlus") %>' Width="100px" CssClass="form-control" />
                </td>
                <td>
                    <asp:TextBox ID="SkladkaTextBox" runat="server" Text='<%# Bind("Skladka") %>' Width="100px" CssClass="form-control" />
                </td>
                <td>
                    <asp:TextBox ID="SkladkaPlusTextBox" runat="server" Text='<%# Bind("SkladkaPlus") %>' Width="100px" CssClass="form-control" />
                </td>
                <%--<td>
                    <asp:TextBox ID="RodzajIdTextBox" runat="server" Text='<%# Bind("RodzajId") %>' />
                </td>--%>
                <td>
                    <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="Update" CssClass="btn-small btn-success">
                        <i class="fa fa-floppy-o"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn-small btn-default">
                        <i class="fa fa-ban"></i>
                    </asp:LinkButton>
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr style="">
                <%--<td>&nbsp;</td>--%>
                <td>
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' Width="100px" CssClass="form-control" MaxLength="8" />
                </td>
                <td>
                    <%--<asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("DateFrom") %>' CssClass="form-control" />--%>
                    <cc:DateEdit ID="deDateFrom" runat="server" Date='<%# Bind("Od") %>' />
                </td>
                <td>
                    <%--<asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("DateTo") %>' CssClass="form-control" />--%>
                    <cc:DateEdit ID="deDateTo" runat="server" Date='<%# Bind("Do") %>' />

                </td>
                <%--<td>
                    <asp:TextBox ID="GrupaTextBox" runat="server" Text='<%# Bind("Grupa") %>' />
                </td>--%>
                <td>
                    <asp:TextBox ID="SumaTextBox" runat="server" Text='<%# Bind("Suma") %>' Width="100px" CssClass="form-control" MaxLength="8" />
                </td>
                <td>
                    <asp:TextBox ID="SumaPlusTextBox" runat="server" Text='<%# Bind("SumaPlus") %>' Width="100px" CssClass="form-control" MaxLength="8" />
                </td>
                <td>
                    <asp:TextBox ID="SkladkaTextBox" runat="server" Text='<%# Bind("Skladka") %>' Width="100px" CssClass="form-control" MaxLength="8" />
                </td>
                <td>
                    <asp:TextBox ID="SkladkaPlusTextBox" runat="server" Text='<%# Bind("SkladkaPlus") %>' Width="100px" CssClass="form-control" MaxLength="8" />
                </td>
                <%--<td>
                    <asp:TextBox ID="RodzajIdTextBox" runat="server" Text='<%# Bind("RodzajId") %>' />
                </td>--%>
                <td>
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" CssClass="btn btn-success" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr style="">
                <%--<td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>--%>
                <td>
                    <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                </td>
                <td>
                    <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("DateFrom") %>' />
                </td>
                <td>
                    <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("DateTo") %>' />
                </td>
                <%--<td>
                    <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("Grupa") %>' />
                </td>--%>
                <td>
                    <asp:Label ID="SumaLabel" runat="server" Text='<%# Eval("Suma") %>' />
                </td>
                <td>
                    <asp:Label ID="SumaPlusLabel" runat="server" Text='<%# Eval("SumaPlus") %>' />
                </td>
                <td>
                    <asp:Label ID="SkladkaLabel" runat="server" Text='<%# Eval("Skladka") %>' />
                </td>
                <td>
                    <asp:Label ID="SkladkaPlusLabel" runat="server" Text='<%# Eval("SkladkaPlus") %>' />
                </td>
                <%--<td>
                    <asp:Label ID="RodzajIdLabel" runat="server" Text='<%# Eval("RodzajId") %>' />
                </td>--%>
                <td>
                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Text="Edit" CssClass="btn-primary btn-small xbtn-pirmary">
                        <i class="fa fa-pencil"></i>
                    </asp:LinkButton>

                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="btn-small btn-danger">
                        <i class="fa fa-trash"></i>
                    </asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <%--<th runat="server">Id</th>--%>
                                <th runat="server">Kolejnosc</th>
                                <th runat="server">Od</th>
                                <th runat="server">Do</th>
                                <%--<th runat="server">Grupa</th>--%>
                                <th runat="server">Suma</th>
                                <th runat="server">SumaPlus</th>
                                <th runat="server">Skladka</th>
                                <th runat="server">SkladkaPlus</th>
                                <%--<th runat="server">RodzajId</th>--%>
                                <th runat="server"></th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
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
                        <span class="count">Ilość wniosków:<asp:Label ID="lbCount" runat="server"></asp:Label></span>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
        DeleteCommand="DELETE FROM poWnioskiMajatkoweParametry WHERE [Id] = @Id"
        InsertCommand="INSERT INTO poWnioskiMajatkoweParametry ([Kolejnosc], [Od], [Do], [Grupa], [Suma], [SumaPlus], [Skladka], [SkladkaPlus], [RodzajId]) 
            VALUES (@Kolejnosc, @Od, @Do, 'HAJS', @Suma, @SumaPlus, @Skladka, @SkladkaPlus, @rodzaj)"
        SelectCommand="SELECT *, convert(varchar(10), Od, 20) DateFrom, convert(varchar(10), Do, 20) DateTo FROM poWnioskiMajatkoweParametry where RodzajId = @rodzaj"
        UpdateCommand="UPDATE [poWnioskiMajatkoweParametry] SET [Kolejnosc] = @Kolejnosc, [Od] = @Od, [Do] = @Do, [Suma] = @Suma, [SumaPlus] = @SumaPlus, [Skladka] = @Skladka, [SkladkaPlus] = @SkladkaPlus WHERE [Id] = @Id">
        <SelectParameters>
            <asp:ControlParameter Name="rodzaj" Type="Int32" ControlID="ddlRodzaje" PropertyName="SelectedValue" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Od" Type="DateTime" />
            <asp:Parameter Name="Do" Type="DateTime" />
            <%--<asp:Parameter Name="Grupa" Type="String" />--%>
            <asp:Parameter Name="Suma" Type="Double" />
            <asp:Parameter Name="SumaPlus" Type="Double" />
            <asp:Parameter Name="Skladka" Type="Double" />
            <asp:Parameter Name="SkladkaPlus" Type="Double" />
            <asp:Parameter Name="RodzajId" Type="Int32" />
            <asp:ControlParameter Name="rodzaj" Type="Int32" ControlID="ddlRodzaje" PropertyName="SelectedValue" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Od" Type="DateTime" />
            <asp:Parameter Name="Do" Type="DateTime" />
            <%--<asp:Parameter Name="Grupa" Type="String" />--%>
            <asp:Parameter Name="Suma" Type="Double" />
            <asp:Parameter Name="SumaPlus" Type="Double" />
            <asp:Parameter Name="Skladka" Type="Double" />
            <asp:Parameter Name="SkladkaPlus" Type="Double" />
            <asp:Parameter Name="RodzajId" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>





</div>
