<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReportScheduler.ascx.cs" Inherits="HRRcp.Controls.Reports.cntReportScheduler" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<div id="paReportScheduler" runat="server" class="cntReportScheduler">
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidReportId" runat="server" Visible="false"/>

    <asp:Label ID="Label1" CssClass="title" runat="server" Text="Subskrypcje raportów" />
    <asp:ListView ID="lvScheduler" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" InsertItemPosition="None" OnItemCreated="lvScheduler_ItemCreated" OnDataBound="lvScheduler_DataBound" OnItemDataBound="lvScheduler_ItemDataBound" OnItemInserting="lvScheduler_ItemInserting" OnItemUpdating="lvScheduler_ItemUpdating">
        <ItemTemplate>
            <tr class="it">
                <td class="button_insert"></td>    
                <td id="tdRaport" runat="server" class="raport">
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("RaportNazwa") %>' />
                </td>
                <td id="tdUser" runat="server" class="user">
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("UserName") %>' />
                </td>
                <td class="email">
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("UserEmail") %>' />
                </td>
                <td class="data">
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("DataStartu", "{0:yyyy-MM-dd HH:mm}") %>' />
                </td>
                <td class="data">
                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("DataStopu", "{0:yyyy-MM-dd HH:mm}") %>' />
                </td>
                <td class="interwal">
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("InterwalOpis") %>' />
                </td>
                <td class="data">
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("NextStart", "{0:yyyy-MM-dd HH:mm}") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                </td>
                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                </td>



                <%--            
                <td>
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="UserIdLabel" runat="server" Text='<%# Eval("UserId") %>' />
                </td>
                <td>
                    <asp:Label ID="IdPracownikaLabel" runat="server" Text='<%# Eval("IdPracownika") %>' />
                </td>
                <td>
                    <asp:Label ID="IdRaportuLabel" runat="server" Text='<%# Eval("IdRaportu") %>' />
                </td>
                <td>
                    <asp:Label ID="ParametryLabel" runat="server" Text='<%# Eval("Parametry") %>' />
                </td>
                <td>
                    <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                </td>
                <td>
                    <asp:Label ID="ccLabel" runat="server" Text='<%# Eval("cc") %>' />
                </td>
                <td>
                    <asp:Label ID="bccLabel" runat="server" Text='<%# Eval("bcc") %>' />
                </td>
                <td>
                    <asp:Label ID="DataStartuLabel" runat="server" Text='<%# Eval("DataStartu") %>' />
                </td>
                <td>
                    <asp:Label ID="InterwalTypLabel" runat="server" Text='<%# Eval("InterwalTyp") %>' />
                </td>
                <td>
                    <asp:Label ID="InterwalLabel" runat="server" Text='<%# Eval("Interwal") %>' />
                </td>
                <td>
                    <asp:Label ID="InterwalSqlLabel" runat="server" Text='<%# Eval("InterwalSql") %>' />
                </td>
                <td>
                    <asp:Label ID="NextStartLabel" runat="server" Text='<%# Eval("NextStart") %>' />
                </td>
                <td>
                    <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                </td>
                <td>
                    <asp:Label ID="LastStartLabel" runat="server" Text='<%# Eval("LastStart") %>' />
                </td>
                <td>
                    <asp:Label ID="LastStopLabel" runat="server" Text='<%# Eval("LastStop") %>' />
                </td>
                <td>
                    <asp:Label ID="LastErrorLabel" runat="server" Text='<%# Eval("LastError") %>' />
                </td>
                --%>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" class="tbReportScheduler_edt">
                <tr>
                    <td>
                        <asp:Button ID="InsertButton" runat="server" CssClass="button" CommandName="NewRecord" Text="Dodaj subskrypcję" />
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table runat="server" class="tbReportScheduler">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th class="button_insert">
                                    <div class="button_insert">
                                        <div>
                                            <asp:Button ID="InsertButton" CssClass="button" runat="server" CommandName="NewRecord" Text="Dodaj subskrybcję" />
                                        </div>    
                                    </div>
                                </th>
                                <th runat="server" id="thRaport">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="RaportNazwa">Raport</asp:LinkButton></th>
                                <th runat="server" id="thUser">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="UserName">Użytkownik</asp:LinkButton></th>
                                <th runat="server"><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Email">E-Mail</asp:LinkButton></th>
                                <th runat="server"><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="DataStartu">Czas rozpoczęcia</asp:LinkButton></th>
                                <th runat="server"><asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="DataStopu">Czas zakończenia</asp:LinkButton></th>
                                <th runat="server"><asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="InterwalOpis">Uruchamiany co</asp:LinkButton></th>
                                <th runat="server"><asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="NextStart">Następne uruchomienie</asp:LinkButton></th>
                                <th runat="server"><asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Aktywny">Aktywna</asp:LinkButton></th>
                                <th class="control"></th>
                                <%--                            
                                <th runat="server">Id</th>
                                <th runat="server">UserId</th>
                                <th runat="server">IdPracownika</th>
                                <th runat="server">IdRaportu</th>
                                <th runat="server">Parametry</th>
                                <th runat="server">Typ</th>
                                <th runat="server">cc</th>
                                <th runat="server">bcc</th>
                                <th runat="server">DataStartu</th>
                                <th runat="server">InterwalTyp</th>
                                <th runat="server">Interwal</th>
                                <th runat="server">InterwalSql</th>
                                <th runat="server">NextStart</th>
                                <th runat="server">Status</th>
                                <th runat="server">Aktywny</th>
                                <th runat="server">LastStart</th>
                                <th runat="server">LastStop</th>
                                <th runat="server">LastError</th>
                                --%>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" class="pager">
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="15" class="pager">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="navi" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" RenderNonBreakingSpacesBetweenControls="false" FirstPageText="««" PreviousPageText="«" />
                                <asp:NumericPagerField ButtonType="Link" CurrentPageLabelCssClass="navicurr" NumericButtonCssClass="navipage" NextPreviousButtonCssClass="navi" RenderNonBreakingSpacesBetweenControls="false"/>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="navi" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" RenderNonBreakingSpacesBetweenControls="false" NextPageText="»" LastPageText="»»" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <EditItemTemplate>
            <tr class="iit">
                <td class="button_insert"></td>    
                <td id="tdRaport" runat="server" class="raport">
                    <asp:Label ID="lbRaport" runat="server" />
                    <asp:DropDownList ID="ddlRaport" runat="server" DataSourceID="SqlDataSource2" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRaport" ValidationGroup="evg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                </td>
                <td id="tdUser" runat="server" class="user">
                    <asp:Label ID="lbUser" runat="server" />
                    <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="SqlDataSource3" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddlUserE_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlUser" ValidationGroup="evg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                </td>
                <td class="email">
                    <asp:Label ID="lbEmail" runat="server" Visible="true" Text='<%# Eval("UserEmail") %>'/>
                   
                    <%--                
                    <asp:TextBox ID="EMailTextBox" runat="server" Text='<%# Bind("Email") %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="EMailTextBox" ValidationGroup="ivg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                    --%>

                </td>
                <td class="data">
                    <uc1:DateEdit ID="deStart" runat="server" Date='<%# Bind("DataStartu") %>' TimeFormat="HH:mm" ValidationGroup="evg"/>
                </td>
                <td class="data">
                    <uc1:DateEdit ID="deStop" runat="server" Date='<%# Bind("DataStopu") %>' />
                </td>
                <td class="interwal">
                    <asp:DropDownList ID="ddlInterwalTyp" runat="server" DataSourceID="SqlDataSource4" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddlInterwalTypE_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvInterwalTyp" runat="server" ControlToValidate="ddlInterwalTyp" ValidationGroup="evg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                    <asp:TextBox ID="InterwalTextBox" runat="server" Text='<%# Bind("Interwal") %>' Visible='<%# IsIntervalVisible(Eval("InterwalTyp")) %>' />
                    <asp:FilteredTextBoxExtender ID="ftbInterwal" runat="server"  
                        TargetControlID="InterwalTextBox" 
                        FilterType="Custom" 
                        ValidChars="0123456789" />
                    <asp:RequiredFieldValidator ID="rfvInterwal" runat="server" ControlToValidate="InterwalTextBox" ValidationGroup="evg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                </td>
                <td class="data">
                </td>
                <td class="check">
                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Aktywny") %>'  />
                </td>
                <td class="control">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                </td>


                <%--            
                <td>
                </td>
                <td>
                    <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:TextBox ID="UserIdTextBox" runat="server" Text='<%# Bind("UserId") %>' />
                </td>
                <td>
                    <asp:TextBox ID="IdPracownikaTextBox" runat="server" Text='<%# Bind("IdPracownika") %>' />
                </td>
                <td>
                    <asp:TextBox ID="IdRaportuTextBox" runat="server" Text='<%# Bind("IdRaportu") %>' />
                </td>
                <td>
                    <asp:TextBox ID="ParametryTextBox" runat="server" Text='<%# Bind("Parametry") %>' />
                </td>
                <td>
                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
                </td>
                <td>
                    <asp:TextBox ID="ccTextBox" runat="server" Text='<%# Bind("cc") %>' />
                </td>
                <td>
                    <asp:TextBox ID="bccTextBox" runat="server" Text='<%# Bind("bcc") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DataStartuTextBox" runat="server" Text='<%# Bind("DataStartu") %>' />
                </td>
                <td>
                    <asp:TextBox ID="InterwalTypTextBox" runat="server" Text='<%# Bind("InterwalTyp") %>' />
                </td>
                <td>
                    <asp:TextBox ID="InterwalTextBox" runat="server" Text='<%# Bind("Interwal") %>' />
                </td>
                <td>
                    <asp:TextBox ID="InterwalSqlTextBox" runat="server" Text='<%# Bind("InterwalSql") %>' />
                </td>
                <td>
                    <asp:TextBox ID="NextStartTextBox" runat="server" Text='<%# Bind("NextStart") %>' />
                </td>
                <td>
                    <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
                </td>
                <td>
                    <asp:TextBox ID="LastStartTextBox" runat="server" Text='<%# Bind("LastStart") %>' />
                </td>
                <td>
                    <asp:TextBox ID="LastStopTextBox" runat="server" Text='<%# Bind("LastStop") %>' />
                </td>
                <td>
                    <asp:TextBox ID="LastErrorTextBox" runat="server" Text='<%# Bind("LastError") %>' />
                </td>
                --%>
            </tr>
        </EditItemTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td class="button_insert"></td>    
                <td id="tdRaport" runat="server" class="raport">
                    <asp:Label ID="lbRaport" runat="server" />
                    <asp:DropDownList ID="ddlRaport" runat="server" DataSourceID="SqlDataSource2" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRaport" ValidationGroup="ivg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                </td>
                <td id="tdUser" runat="server" class="user">
                    <asp:Label ID="lbUser" runat="server" />
                    <asp:DropDownList ID="ddlUser" runat="server" DataSourceID="SqlDataSource3" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddlUserI_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlUser" ValidationGroup="ivg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                </td>
                <td class="email">
                    <asp:Label ID="lbEmail" runat="server" Visible="true" />
                   
                    <%--                
                    <asp:TextBox ID="EMailTextBox" runat="server" Text='<%# Bind("Email") %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="EMailTextBox" ValidationGroup="ivg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                    --%>

                </td>
                <td class="data">
                    <uc1:DateEdit ID="deStart" runat="server" Date='<%# Bind("DataStartu") %>' TimeFormat="HH:mm" ValidationGroup="ivg"/>
                </td>
                <td class="data">
                    <uc1:DateEdit ID="deStop" runat="server" Date='<%# Bind("DataStopu") %>' />
                </td>
                <td class="interwal">
                    <asp:DropDownList ID="ddlInterwalTyp" runat="server" DataSourceID="SqlDataSource4" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddlInterwalTypI_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvInterwalTyp" runat="server" ControlToValidate="ddlInterwalTyp" ValidationGroup="ivg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                    <asp:TextBox ID="InterwalTextBox" runat="server" Text='<%# Bind("Interwal") %>' Visible="false"/>
                    <asp:FilteredTextBoxExtender ID="ftbInterwal" runat="server"  
                        TargetControlID="InterwalTextBox" 
                        FilterType="Custom" 
                        ValidChars="0123456789" />
                    <asp:RequiredFieldValidator ID="rfvInterwal" runat="server" ControlToValidate="InterwalTextBox" ValidationGroup="ivg" ErrorMessage="Błąd" CssClass="error" SetFocusOnError="True" Display="Dynamic" ></asp:RequiredFieldValidator>
                </td>
                <td class="data">
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>'  />
                </td>
                <td class="control">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Clear" />
                </td>


                <%--
                <td>
                    <asp:TextBox ID="UserIdTextBox" runat="server" Text='<%# Bind("UserId") %>' />
                </td>
                <td>
                    <asp:TextBox ID="IdPracownikaTextBox" runat="server" Text='<%# Bind("IdPracownika") %>' />
                </td>
                <td>
                    <asp:TextBox ID="IdRaportuTextBox" runat="server" Text='<%# Bind("IdRaportu") %>' />
                </td>
                <td>
                    <asp:TextBox ID="ParametryTextBox" runat="server" Text='<%# Bind("Parametry") %>' />
                </td>
                <td>
                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
                </td>
                <td>
                    <asp:TextBox ID="ccTextBox" runat="server" Text='<%# Bind("cc") %>' />
                </td>
                <td>
                    <asp:TextBox ID="bccTextBox" runat="server" Text='<%# Bind("bcc") %>' />
                </td>
                <td>
                    <asp:TextBox ID="DataStartuTextBox" runat="server" Text='<%# Bind("DataStartu") %>' />
                </td>
                <td>
                    <asp:TextBox ID="InterwalTypTextBox" runat="server" Text='<%# Bind("InterwalTyp") %>' />
                </td>
                <td>
                    <asp:TextBox ID="InterwalTextBox" runat="server" Text='<%# Bind("Interwal") %>' />
                </td>
                <td>
                    <asp:TextBox ID="InterwalSqlTextBox" runat="server" Text='<%# Bind("InterwalSql") %>' />
                </td>
                <td>
                    <asp:TextBox ID="NextStartTextBox" runat="server" Text='<%# Bind("NextStart") %>' />
                </td>
                <td>
                    <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
                </td>
                <td>
                    <asp:TextBox ID="LastStartTextBox" runat="server" Text='<%# Bind("LastStart") %>' />
                </td>
                <td>
                    <asp:TextBox ID="LastStopTextBox" runat="server" Text='<%# Bind("LastStop") %>' />
                </td>
                <td>
                    <asp:TextBox ID="LastErrorTextBox" runat="server" Text='<%# Bind("LastError") %>' />
                </td>
                --%>
            </tr>
        </InsertItemTemplate>
    </asp:ListView>
    <%--
    <div class="bottom_buttons">
        <asp:Button ID="InsertButton" runat="server" CssClass="button" Text="Dodaj przypomnienie" />
    </div>                        
    --%>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="true"
        DeleteCommand="DELETE FROM [RaportyScheduler] WHERE [Id] = @Id" 
        InsertCommand="
declare @next datetime
set @next = @DataStartu
while @next &lt; GETDATE()
	set @next = case ISNULL(@InterwalTyp,'') 
		when 'HH' then DATEADD(HH, @Interwal, @next)
		when 'DD' then DATEADD(DD, @Interwal, @next)
		when 'WW' then DATEADD(WW, @Interwal, @next)
		when 'MM' then DATEADD(MM, @Interwal, @next)
        else GETDATE()
		end
INSERT INTO [RaportyScheduler] ([UserId], [IdPracownika], [IdRaportu], [Parametry], [Typ], [cc], [bcc], [DataStartu], [InterwalTyp], [Interwal], [InterwalSql], [NextStart], [Status], [Aktywny], [LastStart], [LastStop], [LastError]) 
    VALUES (@UserId, 
        0,--@IdPracownika, 
        @IdRaportu, @Parametry, 
        0,--@Typ, 
        @cc, @bcc, @DataStartu, @InterwalTyp, @Interwal, @InterwalSql, 
        @next,--@NextStart, 
        0,--@Status, 
        @Aktywny, @LastStart, @LastStop, @LastError)
        " 
        SelectCommand="
SELECT S.*, 
--U.LastName + ' ' + U.FirstName as UserName, U.Email as UserEmail,
U.Nazwisko + ' ' + U.Imie as UserName, U.Email as UserEmail,
R.MenuText as RaportNazwa, R.ToolTip as RaportOpis,
case 
when InterwalTyp is null then 'Jednorazowo'
when InterwalTyp in ('HH','HOUR') then 'Godzin: ' + convert(varchar, S.Interwal)
when InterwalTyp in ('D','DD','DAY') then 'Dni: ' + convert(varchar, S.Interwal)
when InterwalTyp in ('WW','WEEK') then 'Tygodnie: ' + convert(varchar, S.Interwal)
when InterwalTyp in ('M','MM','MONTH') then 'Miesiące: ' + convert(varchar, S.Interwal)
else InterwalTyp + ': ' + convert(varchar, S.Interwal)
end as InterwalOpis
from RaportyScheduler S 
--left join AspNetUsers U on U.Id = S.UserId
left join Pracownicy U on U.Id = S.UserId
left join SqlMenu R on R.Id = S.IdRaportu
where (@UserId = 'all' or S.UserId = @UserId) 
  and (@IdRaportu = -99 or S.IdRaportu = @IdRaportu)
        " 
        UpdateCommand="
declare @next datetime
set @next = @DataStartu
while @next &lt; GETDATE()
	set @next = case ISNULL(@InterwalTyp, '') 
		when 'HH' then DATEADD(HH, @Interwal, @next)
		when 'DD' then DATEADD(DD, @Interwal, @next)
		when 'WW' then DATEADD(WW, @Interwal, @next)
		when 'MM' then DATEADD(MM, @Interwal, @next)
        else GETDATE()
		end
UPDATE [RaportyScheduler] SET [UserId] = @UserId, 
    [IdPracownika] = 0,--@IdPracownika, 
    [IdRaportu] = @IdRaportu, [Parametry] = @Parametry, 
    [Typ] = 0,--@Typ, 
    [cc] = @cc, [bcc] = @bcc, 
    [DataStartu] = @DataStartu, [InterwalTyp] = @InterwalTyp, [Interwal] = @Interwal, [InterwalSql] = @InterwalSql, 
    [NextStart] = @next,--@NextStart, 
    [Status] = 0,--@Status, 
    [Aktywny] = @Aktywny, 
    [LastStart] = @LastStart, [LastStop] = @LastStop, 
    --[LastError] = @LastError 
    LastError = @InterwalTyp
WHERE [Id] = @Id
        ">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="UserId" Type="String" />
            <asp:Parameter Name="IdPracownika" Type="Int32" />
            <asp:Parameter Name="IdRaportu" Type="Int32" />
            <asp:Parameter Name="Parametry" Type="String" />
            <asp:Parameter Name="Email" Type="String" />
            <asp:Parameter Name="Typ" Type="Int32" />
            <asp:Parameter Name="cc" Type="String" />
            <asp:Parameter Name="bcc" Type="String" />
            <asp:Parameter Name="DataStartu" Type="DateTime" />
            <asp:Parameter Name="InterwalTyp" Type="String" />
            <asp:Parameter Name="Interwal" Type="Int32" />
            <asp:Parameter Name="InterwalSql" Type="String" />
            <asp:Parameter Name="NextStart" Type="DateTime" />
            <asp:Parameter Name="Status" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="LastStart" Type="DateTime" />
            <asp:Parameter Name="LastStop" Type="DateTime" />
            <asp:Parameter Name="LastError" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="hidUserId" Name="UserId" PropertyName="Value" Type="String" />
            <asp:ControlParameter ControlID="hidReportId" Name="IdRaportu" PropertyName="Value" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="UserId" Type="String" />
            <asp:Parameter Name="IdPracownika" Type="Int32" />
            <asp:Parameter Name="IdRaportu" Type="Int32" />
            <asp:Parameter Name="Parametry" Type="String" />
            <asp:Parameter Name="UserEmail" Type="String" />
            <asp:Parameter Name="Typ" Type="Int32" />
            <asp:Parameter Name="cc" Type="String" />
            <asp:Parameter Name="bcc" Type="String" />
            <asp:Parameter Name="DataStartu" Type="DateTime" />
            <asp:Parameter Name="InterwalTyp" Type="String" />
            <asp:Parameter Name="Interwal" Type="Int32" />
            <asp:Parameter Name="InterwalSql" Type="String" />
            <asp:Parameter Name="NextStart" Type="DateTime" />
            <asp:Parameter Name="Status" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="LastStart" Type="DateTime" />
            <asp:Parameter Name="LastStop" Type="DateTime" />
            <asp:Parameter Name="LastError" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</div>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'wybierz ...' as Text, null as Value, 1 as Sort, 1 as Aktywny, null as Kolejnosc
    union all
select case when Aktywny = 1 then 'MENU: ' else 'ZOOM: ' end + MenuText as Text, Id as Value, 2 as Sort, Aktywny, Kolejnosc 
from SqlMenu
where Grupa = 'RAPORTY' 
order by Sort, Aktywny desc, Kolejnosc, Text
    ">
    <SelectParameters>
        <asp:Parameter DefaultValue="RAPORT" Name="Grupa" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3_b2b" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'wybierz ...' as Text, null as Value, 1 as Sort, 1 as Aktywny
    union all
select case when Aktywny = 1 then '' else '* ' end + ISNULL(FirstName,'') + ISNULL(' ' + LastName,'') as Text, Id + '|' + ISNULL(Email,'') as Value, 2 as Sort, Aktywny
from AspNetUsers 
order by Sort, Aktywny desc, Text
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'wybierz ...' as Text, null as Value, 1 as Sort
    union all
select case when Status in (0,1) then '' else when Status = -2 then ' ' else '* ' end + Nazwisko + ' ' + Imie + ISNULL(' (' + Email + ')','') as Text, Id + '|' + ISNULL(Email,'') as Value,  
case Status in (0,1) then 2 when Status = -1 then 3 else 4 end as Sort
from Pracownicy
order by Sort, Text
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'wybierz ...' as Text, null as Value
union all select 'Jednorazowo', '1'
union all select 'Godziny', 'HH'
union all select 'Dni', 'DD'
union all select 'Tygodnie', 'WW'
union all select 'Miesiące', 'MM'
    ">
</asp:SqlDataSource>



