<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRequests.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Requests.cntRequests" %>

<%@ Register Src="~/Scorecards/Controls/Requests/cntRequest.ascx" TagPrefix="leet" TagName="Request" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/Scorecards/Controls/Requests/cntCreateRequest.ascx" TagPrefix="leet" TagName="CreateRequest" %>


<div id="ctRequests" runat="server" class="cntRequests">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
        
<%--        <div id="divZoomEmp" style="display: none;" class="modalPopup employeeList">
            <asp:UpdatePanel ID="upZoomEmp" runat="server">
                <ContentTemplate>
                    <leet:EmployeeList id="EmployeeList" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>--%>
        
<%--        <div id="divZoomCreate" style="display: none;" class="modalPopup tb">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <leet:CreateRequest id="CreateRequest" runat="server" OnCreated="RequestCreated" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>--%>

            <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />
            <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
            <asp:HiddenField ID="hidMode" runat="server" Visible="false" />
            <asp:HiddenField ID="hidUpr" runat="server" Visible="false" />
           
            
            <div id="debug" runat="server" visible="false">
                <asp:Button ID="btnTL" runat="server" OnCommand="DebugClick" CommandName="TL" Text="Team Leader" Enabled="false" CssClass="button100" />
                <asp:Button ID="btnKier" runat="server" OnCommand="DebugClick" CommandName="KIER" Text="Kieras" CssClass="button100" />
                <asp:Button ID="btnPrez" runat="server" OnCommand="DebugClick" CommandName="PREZ" Text="Prezes" CssClass="button100" />
            </div>
            
            <div id="divZoom" style="display: none;" class="modalPopup">
                <asp:UpdatePanel ID="upZoom" runat="server">
                    <ContentTemplate>
                        <leet:Request ID="Request" runat="server" OnSomethingChanged="Request_SomethingChanged" Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:ListView ID="lvRequests" runat="server" DataSourceID="dsRequests" DataKeyNames="Id"
                InsertItemPosition="None" OnDataBound="lvRequests_DataBound" >
                <ItemTemplate>
                    <tr id="" class='<%# GetColorClass("it", (int)Eval("color")) %>'>
                        <td class="">
                            <asp:HiddenField ID="hidStatus" runat="server" Visible="false" Value='<%# Eval("Status") %>' />
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                        <td class="">
                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("TL") %>' />
                        </td>
                        <td class="">
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("Kierownik") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Date") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("DataWyplaty") %>' />
                        </td>
                        <td class="num">
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("BilansOtwarcia") %>' />
                        </td>
                        <td class="num">
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("IloscPracownikow") %>' />
                        </td>
                        <td runat="server" visible='<%# !IsStartForm() %>'>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# GetStatus((int)Eval("Status"), (int)Eval("Kacc"), (int)Eval("Pacc")) %>' />
                        </td>
                        <td class="control">
                            <asp:Button ID="btnShow" runat="server" Text="Pokaż" OnClick="ShowRequest" CommandArgument='<%# Eval("IdWniosku") + ";" + Eval("Status") + ";" + Eval("Type") + ";" + Eval("Rodzaj") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table id="Table1" runat="server" class="table0">
                        <tr class="edt">
                            <td>
                                <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr class="iit">
                        <td>
                        </td>
                        <td id="tdControl" class="control">
                            <asp:Button ID="btSave" runat="server" CommandName="Insert" Text="Zapisz" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <tr class="eit">
                        <td>
                        </td>
                        <td id="tdControl" class="control">
                            <asp:Button ID="btSave" runat="server" CommandName="Update" Text="Zapisz" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <LayoutTemplate>
                    <table id="Table1" runat="server" class="ListView1 ListView4 tbZastepstwa tbRequests hoverline table0">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" runat="server">
                                <table id="itemPlaceholderContainer" runat="server">
                                    <tr id="Tr2" runat="server" style="">
                                        <th id="th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa wniosku" CommandName="Sort" CommandArgument="Name" /></th>
                                        <th id="th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Team Leader" CommandName="Sort" CommandArgument="TL" /></th>
                                        <th id="th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Kierownik" CommandName="Sort" CommandArgument="Kierownik" /></th>
                                        <th id="th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Miesiąc wypracowania premii" CommandName="Sort" CommandArgument="Date" /></th>
                                        <th id="th5" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" Text="Miesiąc wypłaty" CommandName="Sort" CommandArgument="DataWyplaty" /></th>
                                        <th id="th6" runat="server"><asp:LinkButton ID="LinkButton6" runat="server" Text="Bilans Otwarcia" CommandName="Sort" CommandArgument="BilansOtwarcia" /></th>
                                        <th id="th7" runat="server"><asp:LinkButton ID="LinkButton7" runat="server" Text="Ilość Pracowników" CommandName="Sort" CommandArgument="IloscPracownikow" /></th>
                                        <th id="th8" runat="server"><asp:LinkButton ID="LinkButton8" runat="server" Text="Status" CommandName="Sort" CommandArgument="Status" /></th>
                                        <th></th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trPager" runat="server">
                            <td id="Td2" runat="server" class="pager">
                                <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true"
                                            ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false"
                                            FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                        <asp:NumericPagerField ButtonType="Link" />
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false"
                                            ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true"
                                            NextPageText="Następna" LastPageText="Ostatnia" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                        <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            
            <div class="bottom_buttons">
                <asp:Button ID="btnNewRequest" runat="server" Text="Dodaj wniosek" OnClick="ShowNewRequest" CssClass="button150 left" />
            </div>
            
            
            <asp:SqlDataSource ID="dsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
 --declare @upr as int = 1
--declare @mode as int = 0
--declare @observerId int = 1
                
select
  ta.Id
, ISNULL(w.Nazwa, ta.Nazwa) as Name
, LEFT(CONVERT(varchar, w.Data, 20), 7) as Date
, w.Id as IdWniosku
, w.Status
, w.Kacc
, w.Pacc
, LEFT(CONVERT(varchar, w.DataWyplaty, 20), 7) as DataWyplaty
, convert(varchar, w.BilansOtwarcia) + ' zł' as BilansOtwarcia
, oa.HeadCount as IloscPracownikow --, w.IloscPracownikow
, case when ta.Rodzaj = 4 then 0 else 1 end as Rodzaj
, case when w.IdTypuArkuszy = -1337 then 1 else 0 end as Type
, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') + ISNULL(' [' + p3.Nazwisko + ' ' + p3.Imie + ']', '') as TL
, p2.Nazwisko + ' ' + p2.Imie + isnull(' (' + p2.KadryId + ')', '') as Kierownik
, case when w.Status = 0 then
		w.Kacc
	else
		case when w.Status = 1 then
			w.Pacc
		else
			case when w.Status = 3 then
				case when @upr = -1 then
					2
				else
					-1337
				end
			else
				-1
			end
		end
	end as Color
from scWnioski w
left join scTypyArkuszy ta on w.IdTypuArkuszy = ta.Id
left join Pracownicy p on p.Id = w.IdPracownika
left join Przypisania prz on prz.IdPracownika = w.IdPracownika and prz.Status = 1 and dbo.eom(w.Data) between prz.Od and isnull(prz.Do, '20990909')
left join Pracownicy p2 on p2.Id = prz.IdKierownika
left join Pracownicy p3 on p3.Id = w.IdAkceptujacego and w.IdAkceptujacego != w.IdPracownika
outer apply (select COUNT(*) as HeadCount from scPremie prem where prem.IdWniosku = w.Id and prem._do is null and prem.IdPracownika != w.IdPracownika and prem.IdPracownika &gt; 0) oa
where
	(
		(
			@mode = 0
			and
			(
			    (w.IdPracownika = @observerId and @observerId != 0)
			    or
			    (@ObserverId = 0)
			)
			--and
			--w.Status = 0
		)
		or
		(
			(
				@mode = 1
				or
				@mode = 2
				or
				@mode = 3
				or
				@mode = 4
			)
			and
			(
				(
					(
						@upr = 1
					)
					and
					(
					    (w.IdPracownika in (select IdPracownika from Przypisania p where p.Status = 1 and p.IdKierownika = @observerId and (dbo.eom(w.Data) between p.Od and ISNULL(p.Do, '20990909'))) and @ObserverId != 0)
					    or
					    (@ObserverId = 0)
					)
					and
					(
						(
							@mode = 1
							and
							w.Status = 1
							and
							w.Kacc = -1
						)
						or
						(
							@mode = 2
							and
							(
								w.Status = 2
								or
								w.Status = 3
							)
							and
							(
								w.Kacc = 1
							)
						)
						or
						(
							@mode = 3
							and
							w.Status = 0
							and
							w.Kacc = 0
						)
						or
						(
						    @mode = 4
						    and
						    w.Status = -1
						)
					)
				)
				or
				(
				    (
					    @upr = 2
					    and
						(
							(
								@mode = 1
								and
								(
								    w.Status = 2
								    /*or
								    w.Status = 1*/
								)
								and
								w.Pacc = -1
							)
							or
							(
								@mode = 2
								and
								(
									w.Status = 3
								)
								and
								(
									w.Pacc = 1
								)
							)
							or
							(
								@mode = 3
								and
								(
									w.Status = 1
									or
									w.Status = 0
								)
								and
								(
									w.Pacc = 0
								)
							)
							or
						    (
						        @mode = 4
						        and
						        w.Status = -1
						    )
						)
					)
					or
					(
					    @upr = 3 ---1
					    and
					    (
					        /*w.Status = 0
					        or
					        w.Status = 1
					        or
					        w.Status = 2
							or*/
							w.Status = 3
					    )
					)
				)
			)
		)
		--or
		--(
		--	0 = 0
		--)
	)

" >
                <SelectParameters>
                    <asp:ControlParameter Name="observerId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
                    <asp:ControlParameter Name="upr" Type="Int32" ControlID="hidUpr" PropertyName="Value" />
                    <asp:ControlParameter Name="mode" Type="Int32" ControlID="hidMode" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
                     
            
            
        <asp:SqlDataSource ID="dsCreateRequest" runat="server" SelectCommand="insert into scWnioski (IdTypuArkuszy, IdPracownika, Data, DataWyplaty, BilansOtwarcia, DataAkceptacji, IdAkceptujacego, IloscPracownikow, Status, Kacc, Pacc, Nazwa, DataUtworzenia) values (-1337, {0}, GETDATE() /*TODO: Skiddy, to z DDL kiedys*/, GETDATE(), null, null, null, null, 0, -1, -1, null, GETDATE())" />
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<asp:SqlDataSource ID="dsIsEmpty" runat="server" SelectCommand="
declare @diesel int = {0}

select case when COUNT(*) &gt; 0 then 1 else 0 end from scWnioski w
outer apply (select COUNT(Id) c from scPremie p where w.Id = p.IdWniosku) oa
where w.IdPracownika = @diesel and w.IdTypuArkuszy = -1337 and oa.c = 0 and w.Status != -1" />
