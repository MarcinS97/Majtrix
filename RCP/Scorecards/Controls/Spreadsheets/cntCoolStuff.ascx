<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCoolStuff.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntCoolStuff" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <div id="ctCoolStuff" runat="server" class="cntCoolStuff cnt cnt2 selectable">
            <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
            <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
            <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
            <asp:HiddenField ID="hidOutsideJob" runat="server" Visible="false" />
            
            <table class="tbDays">
                <asp:Repeater ID="rpDays" runat="server" OnItemDataBound="rpDays_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="godzprod notselected">
                                <span class="prodHours"></span>
                            </td>
<%--                            <td class="prod">
                                <span class="prod"></span>
                            </td>--%>
                            <td class="ilosc notselected">
                                <span class="starsCount"></span>
                            </td>
                            <td data-value='<%# Eval("IloscBledow") %>' class='<%# GetClass("bledy notselected", IsInEdit() && (Eval("State").ToString() == "1") ) %>'>
                                <asp:Label ID="lblErrors" runat="server" Text='<%# Eval("IloscBledow") %>' Visible='<%# !IsInEdit() %>'  />  <%--Visible='<%# (Eval("State").ToString() == "0") %>'--%>
                                <asp:TextBox ID="tbErrors" runat="server" Text='<%# Eval("IloscBledow") %>' Visible='<%# IsInEdit() && (Eval("State").ToString() == "1") %>' MaxLength="10" autocomplete="off" />
                                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" TargetControlID="tbErrors"
                                    FilterType="Custom" ValidChars="0123456789" />
                            </td>
                            <td class="fpy notselected">
                                <span class="fpy"></span>
                            </td>
                            <td class='<%# GetClass("uwagi notselected", IsInEdit() && (Eval("State").ToString() == "1")) %>'>
                                <asp:HiddenField ID="hidState" runat="server" Value='<%# Eval("State") %>' Visible="false" />
                                <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' Visible="false" />
                                <asp:HiddenField ID="hidDate" runat="server" Value='<%# Eval("Date") %>' Visible="false" />
                                <asp:HiddenField ID="hidOldNotes" runat="server" Value='<%# Eval("Uwagi") %>' Visible="false" />
                                <asp:HiddenField ID="hidOldErrors" runat="server" Value='<%# Eval("IloscBledow") %>' Visible="false" />
                                <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("Uwagi") %>' Visible='<%# !IsInEdit() %>' ToolTip='<%# Eval("Uwagi") %>' />
                                <asp:TextBox ID="tbNotes" runat="server" Text='<%# Eval("Uwagi") %>' Visible='<%# IsInEdit() && (Eval("State").ToString() == "1") %>' MaxLength="200" ToolTip='<%# Eval("Uwagi") %>' />
                            </td>
                            <td class="remover notselected" runat="server" visible='<%# IsInEdit() %>'>
                                <asp:LinkButton ID="lnkRemoveRow" runat="server" CssClass="fa fa-times" OnClick="DeleteRow" CommandArgument='<%# Eval("Date") %>'  Visible='<%# IsInEdit() && (Eval("State").ToString() == "1") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
<%--                        <asp:SqlDataSource ID="dsDays" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
--declare @date as datetime = (select Miesiac from scArkusze where Id = @arkusz)
declare @pierwszy as datetime = CONVERT(datetime, Convert(varchar, YEAR(@date)) + '-' + convert(varchar, MONTH(@date)) + '-01' ); --odremowac jak wykorzystane osobno od reszty
with Daiz
as
(
	select @pierwszy  as tDate
	union all
	select DATEADD(D, 1, tDate) from Daiz where DATEADD(D, 1, tDate) &lt; DATEADD(M, 1, @pierwszy)
)
select distinct dz.tDate as Date, case when @oj = 1 then (case when d.Data is not null then 1 else 0 end) else (case when p.IdCommodity is not null then 1 else 0 end) end as State, ISNULL(d.IloscBledow, 0) as IloscBledow, d.Uwagi, d.Id
from Daiz dz
left join scDni d on dz.tDate = d.Data and d.IdTypuArkuszy = @typark and d.IdPracownika = @pracId--d.IdArkusza = @arkusz
--left join Przypisania p on (dz.tDate between p.Od and ISNULL(p.Do, '20990909')) and p.IdPracownika = @pracId
left join Przypisania p on (dz.tDate between p.Od and ISNULL(p.Do, '20990909')) and ((p.IdPracownika = @pracId and IdCommodity = @typark) or (@pracId &lt; 0 and IdCommodity = @typark))
"
                
                >
                    <SelectParameters>
                        <asp:ControlParameter Name="typark" ControlID="hidScorecardTypeId" PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter Name="date" ControlID="hidDate" PropertyName="Value" Type="DateTime" />
                        <asp:ControlParameter Name="pracId" ControlID="hidEmployeeId" PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter Name="oj" ControlID="hidOutsideJob" PropertyName="Value" Type="Int32" />
                    </SelectParameters> 
                </asp:SqlDataSource>--%>
            
        </div>
        
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>