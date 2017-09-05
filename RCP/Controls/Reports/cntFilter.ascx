<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntFilter.ascx.cs" Inherits="HRRcp.Controls.Reports.cntFilter" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<div id="paFilter" runat="server" class="cntFilter">
    <asp:HiddenField ID="hidRodzaj" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidRaport" runat="server" Visible="false"/>
    <table>
        <tr>
            <td class="col1" rowspan="2">
                <asp:Repeater ID="rpFilter" runat="server" DataSourceID="SqlDataSource1" 
                    ondatabinding="rpFilter_DataBinding" onitemcreated="rpFilter_ItemCreated" 
                    onitemdatabound="rpFilter_ItemDataBound" onload="rpFilter_Load">                    
                    <ItemTemplate>
                        <div class='item<%# GetCss(Container.DataItem) %>'>                
                            <asp:Label ID="Label1" runat="server" CssClass="label" Text='<%# Eval("Label1") %>' ToolTip='<%# Eval("ToolTip1") %>'></asp:Label>
                            <asp:TextBox ID="tbValue" runat="server" CssClass="textbox" Visible="false" ></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbValueFTB" runat="server" Enabled="false" 
                                TargetControlID="tbValue" 
                                FilterType="Custom" 
                                ValidChars="0123456789" />

                            <asp:DropDownList ID="ddlValue" runat="server" Visible="false"></asp:DropDownList>                

                            <div id="paDropDownEdit" runat="server" Visible="false">
                                <span id="Span1" runat="server" class="label" ></span>                
                                <asp:DropDownList ID="ddlEditValue" runat="server" ></asp:DropDownList>
                            </div>
                            
                            <uc1:DateEdit ID="deValue" runat="server" Visible="false"/>
                            
                            <div id="paDateRange" class="daterange" runat="server" Visible="false">
                                <asp:Label ID="lbOd" runat="server" Text="od:" ></asp:Label>
                                <uc1:DateEdit ID="deOd" runat="server" />
                                <asp:Label ID="lbDo" runat="server" Text="do:" ></asp:Label>
                                <uc1:DateEdit ID="deDo" runat="server" />
                            </div>

                            <asp:CheckBox ID="cbValue" runat="server" Visible="false"></asp:CheckBox>
                            
                            <asp:RequiredFieldValidator ControlToValidate="tbValue" ValidationGroup="flt" ErrorMessage="Błąd" ID="rfvValue" Enabled="false" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:Label ID="lbValue" runat="server" Visible="false"></asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                    SelectCommand="select * from SqlFields where Aktywny = 1 order by Kolejnosc">
                </asp:SqlDataSource>
            </td>
            <td class="sep" rowspan="2"></td>
            <td class="col2">
            </td>
        </tr>
        <tr>
            <td></td>
            <td class="bottom_buttons">
                <asp:Button ID="btEdit" runat="server" CssClass="button75" Text="Edytuj" OnClick="btEdit_Click" />
                <asp:Button ID="btWyszukaj" runat="server" CssClass="button75" Text="Wyszukaj" OnClick="btWyszukaj_Click" />
                <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" OnClick="btClear_Click"/>
            </td>
        </tr>
    </table>
</div>

<%--
            <div class='item<%# GetCss(Container.DataItem) %>'>                
                <asp:Label ID="Label1" runat="server" CssClass="label" Text='<%# Eval("Label1") %>' ToolTip='<%# Eval("ToolTip1") %>'></asp:Label>
                <asp:TextBox ID="tbValue" runat="server" CssClass="textbox" Visible='<%# IsTyp(Container.DataItem,1,2,4) %>' Text='<%# GetInitValue(Container.DataItem) %>'
                    TextMode='<%# GetTextMode(Container.DataItem) %>' 
                    Rows='<%# GetRows(Container.DataItem) %>' 
                    MaxLength='<%# GetMaxLen(Container.DataItem) %>' >
                </asp:TextBox>
                <asp:DropDownList ID="ddlValue" runat="server" Visible='<%# IsTyp(Container.DataItem,3) %>'></asp:DropDownList>
                
                <div id="paDropDownEdit" runat="server" Visible='<%# IsTyp(Container.DataItem,4) %>'>
                    <span runat="server" class="label" ></span>                
                    <asp:DropDownList ID="ddlEditValue" runat="server" ></asp:DropDownList>
                </div>
    
                <uc1:DateEdit ID="deValue" runat="server" Visible='<%# IsTyp(Container.DataItem,5) %>'/>
                <div id="paDateRange" class="daterange" runat="server" Visible='<%# IsTyp(Container.DataItem,6) %>'>
                    <asp:Label ID="lbOd" runat="server" Text="od:" ></asp:Label>
                    <uc1:DateEdit ID="deOd" runat="server" />
                    <asp:Label ID="lbDo" runat="server" Text="do:" ></asp:Label>
                    <uc1:DateEdit ID="deDo" runat="server" />
                </div>
                <asp:Label ID="lbValue" runat="server" Text='<%# GetInitValue(Container.DataItem) %>' Visible='<%# IsTyp(Container.DataItem,7) %>'></asp:Label>
            </div>
--%>

<%--

Label = Przełożony:
ToolTip = select ...
Typ = ddl
Par = @kierId - 
Type = int
@kierId int
Sql = select Nazwisko + ' ' + Imie as Text, Id as Value from Pracownicy where Kierownik = 1
Column: 1
Width: 300px




Typ
1 - TextBox
2 - Multiline



select * from Pracownicy 
where 
    (
    @kierId is null or IdKierownika = @kierId 
    )
and (
    @dzialId is null or IdDzialu = @dzialId
    )    
and (
    @stanId is null or IdStanowiska = @stanId
    )    


--%>