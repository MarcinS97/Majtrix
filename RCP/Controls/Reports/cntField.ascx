<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntField.ascx.cs" Inherits="HRRcp.Controls.Reports.cntField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Custom" TagPrefix="cc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:Literal ID="ltTr" runat="server" visible="false"><tr></asp:Literal>
<asp:Literal ID="ltTd1" runat="server" visible="false"><td class="label" ></asp:Literal>
<asp:Literal ID="ltTd1title" runat="server" visible="false"><td class="label title" colspan="2"></asp:Literal>
    <asp:Label ID="lbLabelTd" runat="server" Visible="false" ></asp:Label>
<asp:Literal ID="ltTd1c" runat="server" visible="false"></td></asp:Literal>
<asp:Literal ID="ltTd2" runat="server" visible="false"><td class="value" ></asp:Literal>

<div id="paField" runat="server" class="cntField">                
    <asp:Label ID="lbLabel1" runat="server" CssClass="label" ></asp:Label>
    
    <asp:TextBox ID="tbValue" runat="server" CssClass="textbox" Visible="false" ></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="tbValueFTB" runat="server" Enabled="false"
        TargetControlID="tbValue" 
        FilterType="Custom" 
        ValidChars="0123456789" />

    <asp:DropDownList ID="ddlValue" runat="server" Visible="false" OnSelectedIndexChanged="ddlValue_SelectedIndexChanged"></asp:DropDownList>                
    <cc1:cntMultiSelect ID="msValue" runat="server" Visible="false" SelectionMode="Multiple" OnSelectedIndexChanged="msValue_SelectedIndexChanged" />

    <div id="paDropDownEdit" runat="server" Visible="false">
        <span id="lbSpace" runat="server" class="label" ></span>                
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
    
    <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="tbValue" ValidationGroup="flt" ErrorMessage="Błąd" CssClass="error" Enabled="false" SetFocusOnError="True" Display="Dynamic" Visible="false"></asp:RequiredFieldValidator>
    <asp:Label ID="lbValue" runat="server" Visible="false"></asp:Label>
</div>
<asp:Literal ID="ltTd2c" runat="server" visible="false"></td></asp:Literal>
<asp:Literal ID="ltTrc" runat="server" visible="false"></tr></asp:Literal>

