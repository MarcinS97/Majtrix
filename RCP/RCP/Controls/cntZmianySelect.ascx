<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZmianySelect.ascx.cs" Inherits="HRRcp.RCP.Controls.cntZmianySelect" %>

<div id="paZmianySelect" runat="server" class="cntZmianySelect">
    <asp:HiddenField ID="hidEditMode" runat="server" />
    <asp:HiddenField ID="hidMode" runat="server" />
    <asp:ListView ID="lvZmiany" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id" 
        onselectedindexchanged="lvZmiany_SelectedIndexChanged" 
        ondatabound="lvZmiany_DataBound" 
        onitemdatabound="lvZmiany_ItemDataBound" 
        onitemcommand="lvZmiany_ItemCommand" >
        <ItemTemplate>

            <div runat="server" visible="false">
                <asp:Literal ID="ltNewLine" runat="server" Visible="false"><br /></asp:Literal>
            </div>

            <div id="spanZmiana" class="zmiana it" runat="server" data-id='<%# Eval("Id") %>' data-color='<%# Eval("Kolor") %>' data-name='<%# Eval("Symbol") %>'>        
                <table class="zmiana table0">
                    <tr>
                        <td class="col1">
                            <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                        </td>
                        <td class="col2" >
                            <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# Eval("Nazwa") %>' /><br />
                            <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:t}") + " - " %>' /> 
                            <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:t}") %>' />
                            <asp:Label ID="lbMargines" runat="server" Text='<%# Eval("MarginesLabel") %>' />                            
                            <div runat="server" visible="false">
                                <br />
                                <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") + "%<br />" %>' Visible='<%# Eval("StawkaVisible") %>' ToolTip="Stawka w godzinach zmiany"/>
                                <asp:Label ID="lbNadgodziny" CssClass="nadgodziny" runat="server" Text="brak nadgodzin" Visible="false"/>
                                <asp:Label ID="lbBrakZgody" CssClass="brakzgody" runat="server" Text="brak nadgodzin" />
                                <asp:Label ID="lbZgoda" CssClass="zgoda" runat="server" Text="zgoda na nadgodziny" />
                            </div>
                        </td>
                        <td class="col3" >
                            <asp:CheckBox ID="cbSelect" runat="server" CssClass="check" />
                            <asp:Button ID="btSelect" runat="server" CssClass="button_postback" CommandName="Select" Text="Wybierz" />
                        </td>
                    </tr>
                </table>    
<%--                <div class="ico">
                </div>
                <div class="opis">
                </div>
                <div class="control">
                </div>--%>
            </div>
        </ItemTemplate>
        <SelectedItemTemplate>

            <div runat="server" visible="false">
                <asp:Literal ID="ltNewLine" runat="server" Visible="false"><br /></asp:Literal>
            </div>

            <div id="spanZmiana" class="zmiana sit" runat="server" data-id='<%# Eval("Id") %>' data-color='<%# Eval("Kolor") %>' data-name='<%# Eval("Symbol") %>'>
                <table class="zmiana table0">
                    <tr>
                        <td class="col1">
                            <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                        </td>
                        <td class="col2" >
                            <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# Eval("Nazwa") %>' /><br />
                            <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:t}") + " - " %>' /> 
                            <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:t}") %>' />
                            <asp:Label ID="lbMargines" runat="server" Text='<%# Eval("MarginesLabel") %>' />                            
                            <div runat="server" visible="false">
                                <br />
                                <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") + "%<br />" %>' Visible='<%# Eval("StawkaVisible") %>' ToolTip="Stawka w godzinach zmiany"/>
                                <asp:Label ID="lbNadgodziny" CssClass="nadgodziny" runat="server" Text="brak nadgodzin" Visible="false"/>
                                <asp:Label ID="lbBrakZgody" CssClass="brakzgody" runat="server" Text="brak nadgodzin" />
                                <asp:Label ID="lbZgoda" CssClass="zgoda" runat="server" Text="zgoda na nadgodziny" />
                            </div>
                        </td>
                        <td class="col3" >
                            <asp:CheckBox ID="cbSelect" runat="server" CssClass="check" />
                            <asp:Button ID="btSelect" runat="server" CssClass="button_postback" CommandName="Unselect" Text="Wybierz" />
                        </td>
                    </tr>
                </table>    
            </div>
        </SelectedItemTemplate>
        <EmptyDataTemplate>
            <div class="edt">
                <asp:Button ID="btAdd" runat="server" Text="Dodaj zmianę" />
            </div>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div ID="itemPlaceholderContainer" runat="server" class="zmiany3">
                <div ID="itemPlaceholder" runat="server" />


                <div id="paZmianaBrak" class="zmiana zmianaBrak clickable it" runat="server" visible="false">
                    <table class="zmiana table0">
                        <tr>
                            <td class="col1">
                                <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" />
                            </td>
                            <td class="col2" >
                                <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text="Brak zmiany" /><br />
                            </td>
                            <td class="col3" >
                                <asp:CheckBox ID="cbSelect" runat="server" CssClass="check" />
                                <asp:Button ID="btSelect" runat="server" CssClass="button_postback" CommandName="SelectBrak" Text="Wybierz" />
                            </td>
                        </tr>
                    </table>    
                </div>


            </div>
        </LayoutTemplate>
    </asp:ListView>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="
SELECT *,
    --Id,Symbol,Nazwa,Od,Do,Stawka,Visible,Ikona,Kolor,TypZmiany,Nadgodziny,NadgodzinyDzien,NadgodzinyNoc,Margines,ZgodaNadg,Kolejnosc,NowaLinia,Widoczna,HideZgoda,Typ, 1 Sort,
    case when Margines &lt; 0 then '' else
        ' ±' + convert(varchar, Margines) + ' min.'
    end as MarginesLabel,
    cast(case when Stawka = 100 then 0 else 1 end as bit) as StawkaVisible,
    LEFT(convert(varchar, Od, 8),5) as CzasOd,
    LEFT(convert(varchar, Do, 8),5) as CzasDo
FROM Zmiany
where Widoczna=1 and (
    @Mode = 0 or
    @Mode = 1 and Visible = 1 or
    @Mode = 2 and Visible = 0
)
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidMode" Name="Mode" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>



<%--
--declare @Mode int = 2

SELECT
    * 
    --Id,Symbol,Nazwa,Od,Do,Stawka,Visible,Ikona,Kolor,TypZmiany,Nadgodziny,NadgodzinyDzien,NadgodzinyNoc,Margines,ZgodaNadg,Kolejnosc,NowaLinia,Widoczna,HideZgoda,Typ 
  , 1 Sort
  , case when Margines &lt; 0 then '' else
		' ±' + convert(varchar, Margines) + ' min.'
	end as MarginesLabel,
	cast(case when Stawka = 100 then 0 else 1 end as bit) as StawkaVisible,
	LEFT(convert(varchar, Od, 8),5) as CzasOd,
	LEFT(convert(varchar, Do, 8),5) as CzasDo
FROM Zmiany
where Widoczna = 1 and (
    @Mode = 0 or
    @Mode = 1 and Visible = 1 or
    @Mode = 2 and Visible = 0
)

union all

select 
    null, null, 'Usuń', '20000101 8:00', '20000101 16:00', 100, 1, null, '#FFFFFF', 1, null, 150, 200, -1, 0, null, 1, 1, 1, 0
  , 2 Sort
  , null, 0, '8:00', '16:00'
from (select 1 x) x 
where @Mode in (0, 1) and 0 = 1
ORDER BY Sort, Widoczna desc, Visible desc, Kolejnosc, TypZmiany, Symbol









--@Mode = 1 and (Visible = 1 or Id > 16) or
--@Mode = 2 and (Visible = 0 and Id < 16)
SelectCommand="SELECT *, 
                    case when Margines &lt;= 0 then '' else
                        ' ±' + convert(varchar, Margines) + ' min.'
                    end as MarginesLabel,
                    cast(case when Stawka = 100 then 0 else 1 end as bit) as StawkaVisible,
                    LEFT(convert(varchar, Od, 8),5) as CzasOd,
                    LEFT(convert(varchar, Do, 8),5) as CzasDo
                FROM [Zmiany] 
                where Widoczna=1
                ORDER BY Widoczna desc, Visible desc, Kolejnosc, TypZmiany, Symbol" >
--%>
