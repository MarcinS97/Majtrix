<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDays.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntDays" %>

<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntEmployeesZoom.ascx" TagPrefix="leet" TagName="EmployeeZoom" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<div id="ctDays" runat="server" class="cntDays cnt">

<asp:HiddenField ID="hidReport" runat="server" />

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    
    <div id="divZoomInfo" style="display: none;" class="modalPopup">
        <div class="cntEmployeeZoom">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <leet:EmployeeZoom ID="EmployeeZoom" runat="server" />
                    
                    <div class="bottom_buttons">
                        <asp:Button ID="btExcel" CssClass="button100" runat="server" Text="Excel" OnClick="btExcel_Click" Visible="true"/>
                        <asp:Button ID="btClose2" CssClass="button100" runat="server" Text="Zamknij" OnClick="btClose_Click" />
                    </div>
                    
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btExcel"/>
                </Triggers>
                
            </asp:UpdatePanel>
        </div>
    </div>
    
    

    <div id="divZoom" style="display: none;" class="modalPopup">
        <asp:HiddenField ID="hidSelectedDay" runat="server" Visible="false" />
        <asp:HiddenField ID="hidGenre" runat="server" Visible="false" />
        
        <asp:UpdatePanel ID="upZoom" runat="server">
            <ContentTemplate>
                <asp:ListView ID="lvSpreadsheets" runat="server" DataSourceID="dsSpreadsheets" OnItemCommand="lvSpreadsheets_OnItemCommand">
                    <ItemTemplate>
                        <tr id="Tr1" style="" runat="server" class="it">
                            <td>
                                <asp:Label ID="lnkSpreadsheet" runat="server" Text='<%# Eval("Nazwa") %>' />
                            </td>
                            <td class="check">
                                <span id="asdasdasdsa" class="fa fa-check" runat="server" Visible='<%# GetState(Eval("State").ToString()) %>'></span>
                            </td>
                            <td class="control">
                                <asp:Button ID="btnGo" runat="server" CommandName="Go" CommandArgument='<%# Eval("Id") %>' Text="Przejdź" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table id="Table1" runat="server" class="table0">
                            <tr class="edt">
                                <td>
                                    <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                                    <br />
                                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord"
                                        Text="Dodaj" />
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
                            <tr id="Tr1" runat="server">
                                <td id="Td1" runat="server">
                                    <table id="itemPlaceholderContainer" runat="server">
                                        <tr id="Tr2" runat="server" style="">
                                            <th id="Th3" runat="server">
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Nazwa">Rodzaj</asp:LinkButton>
                                            </th>
                                            <th id="Th1" runat="server">
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="State"></asp:LinkButton>
                                            </th>
                                            <th id="thControl" class="control" runat="server">
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trPager" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
                <asp:SqlDataSource ID="dsSpreadsheets" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select ta.Id, ta.Nazwa, case when (asd.bang &gt; 0) then 1 else 0 end as State
from scTypyArkuszy ta
outer apply (select COUNT(*) as bang from scWartosci d where ta.Id = d.IdTypuArkuszy and @date = d.Data and d.IdPracownika = @pracId) asd
where ta.Aktywny = 1 and ta.Id != @ScorecardTypeId and ta.Rodzaj = @genre">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hidScorecardTypeId" Name="ScorecardTypeId" PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter ControlID="hidSelectedDay" Name="date" PropertyName="Value" Type="DateTime" />
                        <asp:ControlParameter ControlID="hidGenre" Name="genre" PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="hidEmployeeId" Name="pracId" PropertyName="Value" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <div class="bottom_buttons">
                    <asp:Button ID="btClose" CssClass="button100" runat="server" Text="Zamknij" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
    <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidTeamLeader" runat="server" Visible="false" />
    <input id="hidOutsideJob" type="hidden" runat="server" class="hidOutsideJob" />
    <table class="tbDays selectable cnt2">
        <asp:Repeater ID="rpDays" runat="server">
            <ItemTemplate>
                <tr class="" data-value='<%# Eval("Data3") %>' data-state='<%# Eval("State") %>'>
                    <td class="day notselected">
                        <asp:HiddenField ID="hidState" runat="server" Value='<%# Eval("State") %>' Visible="false" />
                        <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' Visible="false" />
                        <asp:HiddenField ID="hidDate" runat="server" Value='<%# Eval("Date") %>' Visible="false" />
                        <asp:HiddenField ID="hidOldValue" runat="server" Value='<%# Eval("CzasNieprod") %>' Visible="false" />
                        <asp:HiddenField ID="hidOldOutside" runat="server" Value='<%# Eval("PracaInnyArkusz") %>' Visible="false" />
                        <asp:HiddenField ID="hidOldCorrection" runat="server" Value='<%# Eval("Korekta") %>' Visible="false" />
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Data") %>' />
                    </td>
                    <td class="aim notselected">
                        <asp:Label ID="lblAimProd" CssClass="aim1" runat="server" Text='<%# Eval("CelProd") %>' />
                    </td>
                    <td class="result notselected">
                        <asp:Label ID="lblResProd" CssClass="result1" runat="server" Text='' />
                    </td>
                    <td class="face notselected">
                        <%--<asp:Label ID="Label1" runat="server" Text='<%# Eval("Mordka1") %>' />--%>
                        <asp:Image ID="img1" CssClass="face1" runat="server" />
                    </td>
                    <td class="aim notselected">
                        <asp:Label ID="Label2" CssClass="aim2" runat="server" Text='<%# Eval("CelJak") %>' />
                    </td>
                    <td class="result notselected">
                        <asp:Label ID="Label3" CssClass="result2" runat="server" Text='' />
                    </td>
                    <td class="face notselected">
                        <%--                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Mordka2") %>' />--%>
                        <asp:Image ID="Image1" CssClass="face2" runat="server" />
                    </td>
                    <td class="war2 notselected">
                        <asp:Label ID="Label11" runat="server" Text='<%# Eval("War2") %>' CssClass="w2" Visible='<%# !IsTeam() || IsOutsideJob() %>' />
                        <asp:LinkButton ID="LinkButton2" runat="server" Text='<%# Eval("War2") %>' CssClass="w2" OnClick="ShowZoom" CommandArgument='<%# Eval("Data") + "|5" %>' Visible='<%# IsTeam() && !IsOutsideJob() %>' />
                    </td>
                    <td class='<%# GetClass("czasnieprod notselected", IsInEdit() && (Eval("State").ToString() == "1") ) %>'>
                        <asp:Label ID="lblUnprod" runat="server" Text='<%# Eval("CzasNieprod") %>' Visible='<%# !IsInEdit() || (Eval("State").ToString() == "0") %>'
                            CssClass="unprod" />
                        <asp:TextBox ID="tbUnprod" runat="server" Text='<%# Eval("CzasNieprod") %>' Visible='<%# IsInEdit() && (Eval("State").ToString() == "1") %>'
                            CssClass="unprod" MaxLength="7" />
                        <asp:FilteredTextBoxExtender ID="tbasd" runat="server" TargetControlID="tbUnprod"
                            FilterType="Custom" ValidChars="0123456789,." />
                    </td>
                    <td class='<%# GetClass("prinnyarkusz notselected prinnyarkuszASD", IsInEdit() && (Eval("State").ToString() == "1") ) %>'
                        data-value='<%# Eval("Lupka") %>'>
                        <asp:Label ID="lblOutsideJob" runat="server" Text='<%# Eval("PracaInnyArkusz") %>'
                            Visible='<%# !IsInEdit() %>'  CssClass="prinnyarkusz value" />   <%--Visible='<%# (Eval("State").ToString() == "0") %>'--%>
                        <asp:TextBox ID="tbOutsideJob" runat="server" Text='<%# Eval("PracaInnyArkusz") %>'
                            Visible='<%# IsInEdit() && (Eval("State").ToString() == "1") %>' CssClass="prinnyarkusz prinnyarkuszedit value"
                            MaxLength="10" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbOutsideJob"
                            FilterType="Custom" ValidChars="0123456789,." />
                    </td>
                    <%--       <td class="prinnyarkusz">
                            <asp:LinkButton ID="Label9" runat="server"  Text='<%# Eval("PracaInnyArkusz") %>' OnClick="OutsideJobClick" CommandArgument='<%# Eval("Data") %>' CssClass="other"  />
                        </td>--%>
                    <td class="prinnyarkuszx notselected" >
                        <asp:LinkButton ID="lnkOutsideJob" runat="server" Text="" OnClick="OutsideJobClick"
                            CommandArgument='<%# Eval("Data") %>' CssClass="fa fa-search-plus other" ToolTip="Uzupełnij arkusz" ValidationGroup="ivg" CausesValidation="true" /> 
                    </td>
                    <td id="tdPlannedTeamSize" runat="server" class="plannedteamsize hid notselected" visible='<%# IsTeam() %>'>
                        <asp:LinkButton ID="lblPlannedTeamSize" runat="server" Text='<%# Eval("TeamSize") %>' OnClick="ShowZoom" CommandArgument='<%# Eval("Data") + "|0" %>' />
                    </td>
<%--                    <td id="tdTeamSizeCorrection" runat="server" class="teamsizecorrection hid notselected" visible='<%# IsTeam() %>'>
                        <asp:Label ID="lblTeamCorrection" runat="server" Text='<%# Eval("Korekta") %>' Visible='<%# (Eval("State").ToString() == "0") %>' />
                        <asp:TextBox ID="tbTeamCorrection" runat="server" Text='<%# Eval("Korekta") %>'
                            Visible='<%# IsInEdit() && (Eval("State").ToString() == "1") %>' CssClass="correction"
                            MaxLength="10" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbTeamCorrection"
                            FilterType="Custom" ValidChars="0123456789-" />
                    </td>--%>
                    <td class="nominal hid notselected">
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("Nominal") %>' CssClass="nom"  />
                        <%--<asp:LinkButton ID="lnkZoomNominal" runat="server" Text='<%# Eval("Nominal") %>' CssClass="nom" OnClick="ShowZoom" Visible='<%# IsTeam() %>' />--%>
                    </td>
                    <td class="nieob hid notselected">
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("GodzNieob") %>' CssClass="abs" Visible='<%# !IsTeam() %>' />
                        <asp:LinkButton ID="lnkZoomNieob" runat="server" Text='<%# Eval("GodzNieob") %>' CssClass="abs" OnClick="ShowZoom" CommandArgument='<%# Eval("Data") + "|2"  %>' Visible='<%# IsTeam() %>' />
                    </td>
                    <td class="kodnieob hid notselected" runat="server" visible='<%# !IsTeam() %>'>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("KodNieob") %>' ToolTip='<%# Eval("NazwaNieob") %>' />
                    </td>
                    <td class="war1 hid notselected">
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("War1") %>' CssClass="w1" Visible='<%# !IsTeam() %>'  />
                        <asp:LinkButton ID="lnkZoomWar1" runat="server" Text='<%# Eval("War1") %>' CssClass="w1" OnClick="ShowZoom" Visible='<%# IsTeam() %>' CommandArgument='<%# Eval("Data") + "|4" %>' />
                    </td>
                    <td class="tlprac hid notselected" runat="server" visible='<%# IsTL() %>'>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("TLPrac") %>' CssClass="tlprac" />
                        <%--<asp:LinkButton ID="LinkButton3" runat="server" Text='<%# Eval("TLPrac") %>' CssClass="tlprac" OnClick="ShowZoom" Visible='<%# IsTeam() %>' CommandArgument='<%# Eval("Data") + "|4" %>' />--%>
                    </td>
                    <td class="nadg hid notselected" >
                        <asp:Label ID="Label12" runat="server" Text='<%# Eval("Nadgodziny") %>' CssClass="nad" Visible='<%# !IsTeam() %>'  />
                        <asp:LinkButton ID="lnkNadg" runat="server" Text='<%# Eval("Nadgodziny") %>' CssClass="nad" OnClick="ShowZoom" CommandArgument='<%# Eval("Data") + "|3" %>' Visible='<%# IsTeam() %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
   

    </ContentTemplate>
</asp:UpdatePanel>

</div>