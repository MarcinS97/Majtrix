<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntBadania.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntBadania" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>

<div class="cntBadania">
    <div class="form-group">
        <label>Wybierz pracownika:</label>
        <asp:DropDownList ID="ddlPracownicy" runat="server" DataSourceID="dsPracownicy" DataValueField="Id" DataTextField="Name" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPracownicy_SelectedIndexChanged" />
    </div>
    <div id="divEmployee" runat="server" visible="false">
        <%--<h4>Badania:</h4>--%>


        <asp:ListView ID="lvBadania" runat="server" DataKeyNames="Id" DataSourceID="dsBadania" InsertItemPosition="None" OnItemUpdating="lvBadania_ItemUpdating" OnItemInserting="lvBadania_ItemInserting" OnItemInserted="lvBadania_ItemInserted">
            <ItemTemplate>
                <asp:HiddenField ID="hidId" runat="server" Visible="false" Value='<%# Eval("Id") %>' />
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <asp:Label ID="lblTitle" runat="server" CssClass="" Text='<%# "Badanie z dnia: " %>' />
                        <asp:Label ID="Label3" runat="server" CssClass="" Text='<%# Eval("DateFrom")%>' Font-Bold="true"></asp:Label>

                        <div class="pull-right">
                            <asp:Label ID="Label1" runat="server" Text='<%# "Data ważności badania: " %>' ></asp:Label>
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("DateTo") %>' Font-Bold="true"></asp:Label>
                        </div>
                    </div>

                    <div class="panel-body">
                        <label>Uwagi:</label>
                        <asp:Label ID="lblUwagi" runat="server" Text='<%# Eval("Notes") %>' />

                        <div class="pull-right">
                            <%--                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" CssClass="btn btn-sm btn-default" />
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" CssClass="btn btn-sm btn-danger" />--%>

                            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                            <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CssClass="btn xbtn-sm xbtn-default text-danger"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                        </div>
                    </div>
                    <ul class="list-group">
                        <asp:Repeater ID="rpItems" runat="server" DataSourceID="dsItems">
                            <ItemTemplate>
                                <li class="list-group-item <%# Eval("Disabled") %>">
                                    <%# Eval("Name") %>
                                    <asp:CheckBox ID="cbSelect" runat="server" CssClass="pull-right" Checked='<%# Convert.ToBoolean(Eval("Checked")) %>' Enabled="false" />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <asp:SqlDataSource ID="dsItems" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="
select mb.*, tb.Nazwa as Name, oa.Checked, case when oa.Checked = 0 then 'disabled' else '' end as Disabled
from msTypyBadan tb 
left join msMapaBadan mb on mb.IdTypuBadan = tb.Id and mb.IdBadania = @badId
outer apply ( select case when mb.Id is null then 0 else 1 end Checked ) oa
where tb.Aktywny = 1
order by Checked desc
">
                        <SelectParameters>
                            <asp:ControlParameter Name="badId" Type="Int32" ControlID="hidId" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:HiddenField ID="hidId" runat="server" Visible="false" Value='<%# Eval("Id") %>' />
                <div class="panel panel-primary panel-edit">
                    <div class="panel-heading">
                        <%--<asp:Label ID="lblTitle" runat="server" CssClass="" Text='<%# "Badanie z dnia: " + Eval("DateFrom")%>'></asp:Label>--%>
                        <asp:Label ID="lblTitle" runat="server" CssClass="" Text="Badanie z dnia: " />
                        <cc:DateEdit ID="deDateLeft" runat="server" Date='<%# Bind("DataOd") %>' />

                        <div class="pull-right">
                            <asp:Label ID="Label2" runat="server" CssClass="" Text="Data ważności badania: " />
                            <cc:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataDo") %>' />
                        </div>

                        <%--<asp:Label ID="Label1" runat="server" CssClass="pull-right" Text='<%# "Data ważności do: " + Eval("DateTo") %>'></asp:Label>--%>
                    </div>

                    <div class="panel-body">
                        <label>Uwagi:</label>
                        <%-- <asp:Label ID="lblUwagi" runat="server" Text='<%# Eval("Notes") %>' />--%>
                        <asp:TextBox ID="tbUwagi" runat="server" CssClass="form-control form-inline" Text='<%# Bind("Uwagi") %>' TextMode="MultiLine" />

                    </div>
                    <ul class="list-group">
                        <asp:Repeater ID="rpItems" runat="server" DataSourceID="dsItems">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidId" runat="server" Visible="false" Value='<%# Eval("Id") %>' />
                                <asp:HiddenField ID="hidBadTypId" runat="server" Visible="false" Value='<%# Eval("IdTypuBadan") %>' />
                                <li class="list-group-item">
                                    <%# Eval("Name") %>
                                    <asp:CheckBox ID="cbSelect" runat="server" CssClass="pull-right" Checked='<%# Convert.ToBoolean(Eval("Checked")) %>' />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <asp:SqlDataSource ID="dsItems" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="
select mb.Id, mb.IdBadania, tb.Id as IdTypuBadan, tb.Nazwa as Name, oa.Checked, case when oa.Checked = 0 then 'disabled' else '' end as Disabled
from msTypyBadan tb 
left join msMapaBadan mb on mb.IdTypuBadan = tb.Id and mb.IdBadania = @badId
outer apply ( select case when mb.Id is null then 0 else 1 end Checked ) oa
where tb.Aktywny = 1
order by Checked desc
">
                        <SelectParameters>
                            <asp:ControlParameter Name="badId" Type="Int32" ControlID="hidId" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="panel-body">
                    <div class="pull-right">
                        <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-sm btn-success" />
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-sm btn-default" />
                    </div>
                </div>
            </EditItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" style="">
                    <tr>
                        <td>No data was returned.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                <asp:HiddenField ID="hidId" runat="server" Visible="false" Value='<%# Eval("Id") %>' />
                <div class="panel panel-primary panel-edit">
                    <div class="panel-heading">
                        <%--<asp:Label ID="lblTitle" runat="server" CssClass="" Text='<%# "Badanie z dnia: " + Eval("DateFrom")%>'></asp:Label>--%>
                        <asp:Label ID="lblTitle" runat="server" CssClass="" Text="Badanie z dnia: " />
                        <cc:DateEdit ID="deDateLeft" runat="server" Date='<%# Bind("DataOd") %>' />

                        <div class="pull-right">
                            <asp:Label ID="Label2" runat="server" CssClass="" Text="Data ważności badania: " />
                            <cc:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataDo") %>' />
                        </div>

                        <%--<asp:Label ID="Label1" runat="server" CssClass="pull-right" Text='<%# "Data ważności do: " + Eval("DateTo") %>'></asp:Label>--%>
                    </div>

                    <div class="panel-body">
                        <label>Uwagi:</label>
                        <%-- <asp:Label ID="lblUwagi" runat="server" Text='<%# Eval("Notes") %>' />--%>
                        <asp:TextBox ID="tbUwagi" runat="server" CssClass="form-control form-inline" Text='<%# Bind("Uwagi") %>' TextMode="MultiLine" />

                    </div>
                    <ul class="list-group">
                        <asp:Repeater ID="rpItems" runat="server" DataSourceID="dsItems">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidId" runat="server" Visible="false" Value='<%# Eval("Id") %>' />
                                <asp:HiddenField ID="hidBadTypId" runat="server" Visible="false" Value='<%# Eval("IdTypuBadan") %>' />
                                <li class="list-group-item">
                                    <%# Eval("Name") %>
                                    <asp:CheckBox ID="cbSelect" runat="server" CssClass="pull-right" Checked='<%# Convert.ToBoolean(Eval("Checked")) %>' />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <asp:SqlDataSource ID="dsItems" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                        SelectCommand="
select mb.Id, mb.IdBadania, tb.Id as IdTypuBadan, tb.Nazwa as Name, oa.Checked, case when oa.Checked = 0 then 'disabled' else '' end as Disabled
from msTypyBadan tb 
left join msMapaBadan mb on mb.IdTypuBadan = tb.Id and mb.IdBadania = @badId
outer apply ( select case when mb.Id is null then 0 else 1 end Checked ) oa
where tb.Aktywny = 1
order by Checked desc
">
                        <SelectParameters>
                            <asp:ControlParameter Name="badId" Type="Int32" ControlID="hidId" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="panel-body">
                    <div class="pull-right">
                       <asp:Button ID="btSave" runat="server" CommandName="Insert" Text="Zapisz" CssClass="btn btn-sm btn-success" />
                        <asp:Button ID="btnCancelInsert" runat="server" OnClick="btnCancelInsert_Click" Text="Cancel" CssClass="btn btn-sm btn-default" />
                    </div>
                </div>
            </InsertItemTemplate>
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server" class="panel-group">
                </div>
            </LayoutTemplate>
            <SelectedItemTemplate>
                <tr style="">
                    <td>
                        <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd") %>' />
                    </td>
                    <td>
                        <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo") %>' />
                    </td>
                    <td>
                        <asp:Label ID="UwagiLabel" runat="server" Text='<%# Eval("Uwagi") %>' />
                    </td>
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    </td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
        <asp:SqlDataSource ID="dsBadania" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" OnInserted="dsBadania_Inserted"
            DeleteCommand="DELETE FROM [msBadania] WHERE [Id] = @Id"
            InsertCommand="INSERT INTO [msBadania] (IdPracownika, [DataOd], [DataDo], [Uwagi]) VALUES (@IdPracownika, @DataOd, @DataDo, @Uwagi)
            SELECT @Ind = SCOPE_IDENTITY()
            "
            SelectCommand="SELECT [Id], [DataOd], [DataDo], [Uwagi], convert(varchar(10), DataOd, 20) DateFrom, convert(varchar(10), DataDo, 20) DateTo, case when Uwagi is null then 'Brak uwag' else Uwagi end as Notes FROM [msBadania] WHERE ([IdPracownika] = @IdPracownika)"
            UpdateCommand="UPDATE [msBadania] SET [DataOd] = @DataOd, [DataDo] = @DataDo, [Uwagi] = @Uwagi WHERE [Id] = @Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="DataOd" Type="DateTime" />
                <asp:Parameter Name="DataDo" Type="DateTime" />
                <asp:Parameter Name="Uwagi" Type="String" />
                <asp:ControlParameter ControlID="ddlPracownicy" DefaultValue="2511" Name="IdPracownika" PropertyName="SelectedValue" Type="Int32" />
                <asp:Parameter Direction="Output" Name="Ind" Type="Int32" /> 
            </InsertParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlPracownicy" DefaultValue="2511" Name="IdPracownika" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="DataOd" Type="DateTime" />
                <asp:Parameter Name="DataDo" Type="DateTime" />
                <asp:Parameter Name="Uwagi" Type="String" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>


        <asp:Button ID="btnAdd" runat="server" Text="Dodaj badanie" CssClass="btn btn-sm btn-success pull-right" OnClick="btnAdd_Click" />

    </div>
</div>



<asp:SqlDataSource ID="dsInsert" runat="server" SelectCommand="insert into msMapaBadan (IdBadania, IdTypuBadan) values ({0}, {1})" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" />
<asp:SqlDataSource ID="dsDelete" runat="server" SelectCommand="delete from msMapaBadan where Id = {0}" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" />


<asp:SqlDataSource ID="dsPracownicy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwisko + ' ' + Imie as Name, 1 as Sort from Pracownicy order by Sort, Name" />
