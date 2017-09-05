<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntEwaluacja.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Ewaluacja.cntEwaluacja" %>

<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="cc" TagName="cntSqlTabs" %>

<div class="cntEwaluacja" style="max-width: 1200px;">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
            <h3 style="width: 400px; display: inline-block;"><i class="glyphicon glyphicon-duplicate"></i>Szkolenia - Ankiety</h3>
            <div id="divRoles" runat="server" class="pull-right">
                <i class="glyphicon glyphicon-eye-open" style="margin-right: 8px;"></i>
                <label>Widok jako:</label>
                <cc:cntSqlTabs ID="Roles" runat="server"
                    AddCssClass="tabKwal"
                    DataTextField="Name"
                    DataValueField="Id"
                    SQL="
select 1 as Id, 'Administrator' as Name, 0 as Sort
union all
select 2 as Id, 'Mistrz' as Name, 1 as Sort
union all
select 3 as Id, 'Trener' as Name, 2 as Sort
        "
                    OnSelectTab="Roles_SelectTab" OnDataBound="Roles_DataBound" />
            </div>
            <hr />
            <div class="form-inline">
                <div class="form-group" style="margin-right: 12px;">
                    <label>Pracownik: </label>
                    <asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownik" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
                    <asp:SqlDataSource ID="dsPracownik" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort from Pracownicy where Id in (select IdPracownika from Certyfikaty where Status = 3)
order by Sort, Name
            " />
                </div>
                <div class="form-group" style="margin-right: 12px;">
                    <label>Szkolenie: </label>
                    <asp:DropDownList ID="ddlSzkolenie" runat="server" DataSourceID="dsSzkolenie" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
                    <asp:SqlDataSource ID="dsSzkolenie" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select u.Id, u.Nazwa Name, 1 as Sort 
from Uprawnienia u
left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId
where 
    uk.Szkolenie = 1 
    and u.Id in (select IdUprawnienia from Certyfikaty where Status = 3)
    and (u.EwaluacjaPrac = 1 or u.EwaluacjaKier = 1 or u.EwaluacjaKier2 = 1) 
order by Sort, Name
            " />
                </div>
                <div class="form-group">
                    <label>Trener: </label>
                    <asp:DropDownList ID="ddlTrener" runat="server" DataSourceID="dsTrener" DataValueField="Id" DataTextField="Name" CssClass="form-control input-sm" AutoPostBack="true" />
                    <asp:SqlDataSource ID="dsTrener" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort from Pracownicy where dbo.GetRightId(Rights, 82) = 1
order by Sort, Name
            " />
                </div>
            </div>
            <hr />
            <asp:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id"
                DataSourceID="dsList" CssClass="table" GridLines="None" EmptyDataRowStyle-CssClass="empty">
                <PagerStyle CssClass="pagination-ys" />
                <PagerSettings Mode="NumericFirstLast" />
                <Columns>
                    <asp:BoundField DataField="KadryId" HeaderText="Nr Ewid." InsertVisible="False" ReadOnly="True" SortExpression="KadrySort" />
                    <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" SortExpression="Pracownik" />
                    <asp:BoundField DataField="Szkolenie" HeaderText="Szkolenie" SortExpression="Szkolenie" />
                    <asp:BoundField DataField="Trener" HeaderText="Trener" SortExpression="Trener" />
                    <%--<asp:BoundField DataField="DataRozp" HeaderText="Data rozpoczęcia" SortExpression="DataRozp" />--%>
                    <asp:BoundField DataField="DataZak" HeaderText="Data ukończenia" SortExpression="DataZak" />
                    <%--<asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" />--%>

                    <asp:TemplateField HeaderText="Ankiety" ItemStyle-Width="" SortExpression="AnkietaPracId">
                        <ItemTemplate>

                            <%--<asp:Label ID="lblStatusPrac" runat="server" Text='<%# Eval("AnkietaPracStatus") %>' Width="180px" />--%>

                            <div class="dropdown" runat="server" style="display: inline-block;"
                                visible='<%# (Convert.ToBoolean(Eval("AnkietaPracVisible")) && CanEmployee()) || (IsAdmin() && !Convert.ToBoolean(Eval("AnkietaPracVisible"))) %>'>

                                <a class="btn btn-default glyphicon glyphicon-cog dropdown-toggle" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                    <span class="caret"></span></a>
                                <%--<button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                    Dropdown--%>
                       
                                </button>
                                <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                    <%--<li role="separator" class="divider"></li>--%>
                                    <li class="dropdown-header">
                                        Ankieta pracownika -
                                        <asp:Label ID="lblPracStatus" runat="server" Text='<%# Eval("AnkietaPracStatus") %>' CssClass='<%# Eval("AnkietaPracClass") %>'  /> 
                                    </li>
                                    <li role="separator" class="divider"></li>
                                    <li>
                                        <asp:LinkButton ID="btnEmployeeSurvey" runat="server" OnCommand="Button_Command" CommandName="EmployeeSurvey" CommandArgument='<%# Eval("AnkietaPracId") %>'
                                            CssClass="xbtn xbtn-sm xbtn-default" Visible='<%# Convert.ToBoolean(Eval("AnkietaPracVisible")) && CanEmployee() %>'><i class="glyphicon glyphicon-list-alt"></i>Ankieta</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnPrintEmployee" runat="server" OnCommand="Button_Command" CommandName="PrintEmployeeSurvey"
                                            CommandArgument='<%# Eval("AnkietaPracId") %>' Visible='<%# Convert.ToBoolean(Eval("AnkietaPracVisible")) && CanEmployee() %>'
                                            CssClass="xbtn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-print"></i>Wydruk</asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="btnAddEmployeeSurvey" runat="server" Text="Dodaj" CssClass="xbtn xbtn-sm xbtn-success text-success"
                                            OnClick="btnAddEmployeeSurvey_Click" Visible='<%# IsAdmin() && !Convert.ToBoolean(Eval("AnkietaPracVisible")) %>' CommandArgument='<%# Eval("Id") %>' />
                                    </li>
                                    <li role="separator" class="divider"></li>
                                    <li class="dropdown-header">
                                        Ankieta kierownika -
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("AnkietaKierStatus") %>' CssClass='<%# Eval("AnkietaKierClass") %>'  /> 
                                    </li>
                                    <li role="separator" class="divider"></li>
                                    <li>
                                        <asp:LinkButton ID="btnSuperiorSurvey" runat="server" OnCommand="Button_Command" CommandName="SuperiorSurvey"
                                            CommandArgument='<%# Eval("AnkietaKierId") %>' CssClass="xbtn xbtn-sm xbtn-default"
                                            Visible='<%# Convert.ToBoolean(Eval("AnkietaKierVisible")) && CanSuperior()  %>'><i class="glyphicon glyphicon-list-alt"></i>Ankieta</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnPrintSuperior2" runat="server" OnCommand="Button_Command"
                                            Visible='<%# Convert.ToBoolean(Eval("AnkietaKierVisible")) && CanSuperior()  %>'
                                            CommandName="PrintSuperiorSurvey" CommandArgument='<%# Eval("AnkietaKierId") %>'
                                            CssClass="xbtn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-print"></i>Wydruk</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnAddSuperiorSurvey" runat="server" Text="Dodaj" CssClass="xbtn xbtn-sm xbtn-success text-success"
                                            OnClick="btnAddSuperiorSurvey_Click" Visible='<%# IsAdmin() && !Convert.ToBoolean(Eval("AnkietaKierVisible")) %>' CommandArgument='<%# Eval("Id") %>' />
                                    </li>
                                    <li role="separator" class="divider"></li>
                                    <li class="dropdown-header">
                                        Ankieta kierownika 2 -
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("AnkietaKier2Status") %>' CssClass='<%# Eval("AnkietaKier2Class") %>'  /> 
                                    </li>
                                    <li role="separator" class="divider"></li>
                                    <li>
                                        <asp:LinkButton ID="btnSuperiorSurvey2" runat="server" Text="Ankieta" OnCommand="Button_Command" CommandName="SuperiorSurvey2"
                                            CommandArgument='<%# Eval("AnkietaKierId2") %>' CssClass="xbtn xbtn-sm xbtn-default" Visible='<%# Convert.ToBoolean(Eval("AnkietaKier2Visible")) && CanSuperior() %>'><i class="glyphicon glyphicon-list-alt"></i>Ankieta</asp:LinkButton>

                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnPrintSuperior" runat="server" OnCommand="Button_Command"
                                            Visible='<%# Convert.ToBoolean(Eval("AnkietaKier2Visible")) && CanSuperior()  %>'
                                            CommandName="PrintSuperiorSurvey" CommandArgument='<%# Eval("Id") %>'
                                            CssClass="xbtn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-print"></i>Wydruk</asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnAddSuperiorSurvey2" runat="server" Text="Dodaj" CssClass="xbtn xbtn-sm xbtn-success text-success"
                                            OnClick="btnAddSuperiorSurvey2_Click" Visible='<%# IsAdmin() && !Convert.ToBoolean(Eval("AnkietaKier2Visible")) %>' CommandArgument='<%# Eval("Id") %>' />
                                    </li>

                                </ul>
                            </div>


<%--                                           <asp:LinkButton
                                            ID="LinkButton1"
                                            runat="server"
                                            OnCommand="Button_Command"
                                            CommandName="PrintEmployeeSurvey"
                                            CommandArgument='<%# Eval("AnkietaPracId") %>'
                                            Visible='<%# Convert.ToBoolean(Eval("AnkietaPracVisible")) && CanEmployee() %>'
                                            CssClass="xbtn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-print"></i></asp:LinkButton></li>--%>




                        </ItemTemplate>
                    </asp:TemplateField>
               
                </Columns>
                <EmptyDataTemplate>
                    <span>Brak certyfikatów</span>
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                SelectCommand="
select 
  c.Id
, p.KadryId
, CONVERT(int, p.KadryId) KadrySort
, c.IdPracownika
, c.Status
, c.DataRozpoczecia
, c.DataWaznosci
, p.Nazwisko + ' ' + p.Imie Pracownik
, k.Nazwisko + ' ' + k.Imie Trener 
--, cs.Nazwa as StatusName
, oa1.Id as AnkietaPracId
, oa2.Id as AnkietaKierId
, oa3.Id as AnkietaKierId2
, case when oa1.Id is null then 0 else 1 end as AnkietaPracVisible
, case when oa2.Id is null then 0 else 1 end as AnkietaKierVisible
, case when oa3.Id is null then 0 else 1 end as AnkietaKier2Visible
, c.NazwaCertyfikatu Szkolenie
, convert(varchar(10), c.DataAkceptacji2, 20) DataZak
, case when oa1.Id is null then 'Brak' else oa1.StatusName end AnkietaPracStatus
, case when oa2.Id is null then 'Brak' else oa2.StatusName end AnkietaKierStatus
, case when oa3.Id is null then 'Brak' else oa3.StatusName end AnkietaKier2Status
, case 
    when oa1.Id is null then '' 
    when oa1.Status = -1 then 'text-danger'
    when oa1.Status = 2 then 'text-success'
    else 'text-primary' 
end AnkietaPracClass
, case 
    when oa2.Id is null then '' 
    when oa2.Status = -1 then 'text-danger'
    when oa2.Status = 2 then 'text-success'
    else 'text-primary'
end AnkietaKierClass
, case 
    when oa3.Id is null then '' 
    when oa3.Status = -1 then 'text-danger'
    when oa3.Status = 2 then 'text-success'
    else 'text-primary' 
end AnkietaKier2Class
FROM Certyfikaty c
left join Uprawnienia u on u.Id = c.IdUprawnienia
left join Pracownicy p on p.Id = c.IdPracownika
left join Pracownicy k on k.Id = c.IdTrenera
--left join msCertyfikatyStatus cs on cs.Id = c.Status
outer apply (select top 1 a.*, mas.Nazwa StatusName from msAnkiety a left join msAnkietyStatus mas on mas.Id = a.Status where a.IdCertyfikatu = c.Id and a.Typ = 0) oa1
outer apply (select top 1 a.*, mas.Nazwa StatusName from msAnkiety a left join msAnkietyStatus mas on mas.Id = a.Status where a.IdCertyfikatu = c.Id and a.Typ = 1) oa2
outer apply (select top 1 a.*, mas.Nazwa StatusName from msAnkiety a left join msAnkietyStatus mas on mas.Id = a.Status where a.IdCertyfikatu = c.Id and a.Typ = 2) oa3
where 
    c.Status is not null
    and c.Status = 3
    and
    (
        @role = 1
        or @role = 2 and p.Id in (select IdPracownika from dbo.fn_GetTree2(@userId, 0, GETDATE()))
        or @role = 3 and p.Id in (select IdPracownika from Certyfikaty where IdTrenera = @userId)
    )
    and (p.Id = @pracFilter or @pracFilter is null)
    and (c.IdUprawnienia = @szkolFilter or @szkolFilter is null)
    and (c.IdTrenera = @trenerFilter or @trenerFilter is null)
    and (u.EwaluacjaPrac = 1 or u.Ewaluacjakier = 1 or u.EwaluacjaKier2 = 1)
--WHERE (c.Status = @SelectedTab) 
order by DataZak desc

">
                <SelectParameters>
                    <%--<asp:ControlParameter Name="Status" Type="Int32" ControlID="hidStatus" PropertyName="Value" />--%>
                    <%--<asp:ControlParameter Name="SelectedTab" Type="Int32" ControlID="hidSelectedTab" PropertyName="Value" />--%>
                    <asp:ControlParameter Name="pracFilter" Type="Int32" ControlID="ddlPracownik" PropertyName="SelectedValue" />
                    <asp:ControlParameter Name="szkolFilter" Type="Int32" ControlID="ddlSzkolenie" PropertyName="SelectedValue" />
                    <asp:ControlParameter Name="trenerFilter" Type="Int32" ControlID="ddlTrener" PropertyName="SelectedValue" />
                    <asp:ControlParameter Name="role" Type="Int32" ControlID="Roles" PropertyName="SelectedValue" DefaultValue="1" />
                    <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>


            <hr />

            <div id="divPrint" class="pull-right">
                <span>Puste szablony ankiet:</span>
                <asp:Button ID="btnPrintEmpTemplate" runat="server" OnClick="PrintEmployeeSurveyTemplate" CssClass="btn btn-sm btn-default" Text="Ankieta pracownika" />
                <asp:Button ID="btnPrintSupTemplate" runat="server" OnClick="PrintSuperiorSurveyTemplate" CssClass="btn btn-sm btn-default" Text="Ankieta kierownika" />
            </div>





            <asp:Button ID="btnAddEmployeeSurveyConfirm" runat="server" CssClass="button_postback" OnClick="btnAddEmployeeSurveyConfirm_Click" />
            <asp:Button ID="btnAddSuperiorSurveyConfirm" runat="server" CssClass="button_postback" OnClick="btnAddSuperiorSurveyConfirm_Click" />
            <asp:Button ID="btnAddSuperiorSurveyConfirm2" runat="server" CssClass="button_postback" OnClick="btnAddSuperiorSurveyConfirm2_Click" />


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvList" />
            <asp:PostBackTrigger ControlID="btnPrintEmpTemplate" />
            <asp:PostBackTrigger ControlID="btnPrintSupTemplate" />

        </Triggers>
    </asp:UpdatePanel>


</div>


<asp:SqlDataSource ID="dsCreateEmployeeSurvey" runat="server" SelectCommand="insert into msAnkiety (IdCertyfikatu, Typ, Status, DataRozpoczecia, TematSzkolenia) /*values ({0}, 0, 0, GETDATE())*/ select {0}, 0, 0, GETDATE(), u.Nazwa from Certyfikaty c left join Uprawnienia u on u.Id = c.IdUprawnienia where c.Id = {0}" />
<asp:SqlDataSource ID="dsCreateSuperiorSurvey" runat="server" SelectCommand="insert into msAnkiety (IdCertyfikatu, Typ, Status, DataRozpoczecia) values ({0}, 1, 0, GETDATE())" />
<asp:SqlDataSource ID="dsCreateSuperiorSurvey2" runat="server" SelectCommand="insert into msAnkiety (IdCertyfikatu, Typ, Status, DataRozpoczecia) values ({0}, 2, 0, GETDATE())" />

<asp:SqlDataSource ID="dsGetAnkietaId" runat="server" SelectCommand="select Id from msAnkiety where IdCertyfikatu = {0} and Typ = {1}" />


<asp:SqlDataSource ID="dsData" runat="server" SelectCommand="
select a.*
, p.Nazwisko + ' ' + p.Imie as PracownikCert
from msAnkiety a
left join msAnkietyStatus s on s.Id = a.Status
left join Certyfikaty c on c.Id = a.IdCertyfikatu
left join Pracownicy p on p.Id = c.IdPracownika

where a.Id = {0}" />
