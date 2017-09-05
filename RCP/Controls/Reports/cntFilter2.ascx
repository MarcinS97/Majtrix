<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntFilter2.ascx.cs" Inherits="HRRcp.Controls.Reports.cntFilter2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntFilterFields.ascx" tagname="cntFilterFields" tagprefix="uc2" %>

<%--
printoff
--%>

<div id="paFilter" runat="server" class="cntFilter" visible="false">
    <table class="tbFilter">
        <tr>
            <td id="tdCol1" runat="server" class="col1" rowspan="2">
                <uc2:cntFilterFields ID="cntFilterFields1" Column="1" runat="server" OnDataBound="cntFilterFields1_DataBound" OnSelectedChanged="cntFilterFields1_SelectedChanged"/>
            </td>
            <td id="tdSep1" runat="server" class="sep" rowspan="2" visible="false" >
            </td>
            <td id="tdCol2" runat="server" class="col2" visible="false" >
                <uc2:cntFilterFields ID="cntFilterFields2" Column="2" runat="server" OnDataBound="cntFilterFields2_DataBound" OnSelectedChanged="cntFilterFields2_SelectedChanged"/>
            </td>
        </tr>
        <tr>
            <td id="tdButtons" runat="server" class="bottom_buttons">
                <div class="left">
                    <asp:Button ID="btEdit" runat="server" CssClass="button75 button_tech" Text="Edytuj" OnClick="btEdit_Click" Visible="false" />
                    <asp:Button ID="btEndEdit" runat="server" CssClass="button button_tech" Text="Zakończ edycję" OnClick="btEndEdit_Click" Visible="false" />
                    <asp:Button ID="btAddField" runat="server" CssClass="button button_tech" Text="Dodaj pole" OnClick="btAddField_Click" Visible="false" />
                    <asp:Button ID="btUp" runat="server" CssClass="button button_tech" Text="▲" OnClick="btUp_Click" Visible="false" />
                    <asp:Button ID="btDn" runat="server" CssClass="button button_tech" Text="▼" OnClick="btDn_Click" Visible="false" />
                </div>
                <asp:Button ID="btWyszukaj" runat="server" CssClass="button" Text="Wyszukaj" OnClick="btWyszukaj_Click" Visible="false"/>
                <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" OnClick="btClear_Click" Visible="false"/>
                <asp:Button ID="btHide" runat="server" CssClass="button75" Text="Ukryj" OnClick="btHide_Click" Visible="false" />
            </td>
        </tr>
    </table></div><%--
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