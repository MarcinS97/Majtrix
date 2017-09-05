<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dbField.ascx.cs" Inherits="HRRcp.Portal.Controls.dbField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<%--<div id="paField" class="dbField" runat="server">
    <asp:Label ID="lbLabel" class="label" runat="server" Visible="false" ></asp:Label>
    <asp:RequiredFieldValidator ID="rqValidator" runat="server" SetFocusOnError="True" Display="Dynamic"
        Enabled="false"
        ValidationGroup="vg" 
        ControlToValidate="tbValue" 
        ErrorMessage="Pole wymagane" >
    </asp:RequiredFieldValidator>
    <asp:Label ID="lbValue" class="value" runat="server" Visible="false" ></asp:Label>
    <asp:TextBox ID="tbValue" class="value textbox" runat="server" Visible="false" ></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="false" 
        TargetControlID="tbValue" 
        FilterType="Custom" 
        ValidChars="0123456789" />
    <asp:DropDownList ID="ddlValue" class="value" runat="server" Visible="false" onselectedindexchanged="ddlValue_SelectedIndexChanged"  >
    </asp:DropDownList>
    <div id="paDateEdit" runat="server" class="dateedit" Visible="false">
        <uc1:DateEdit ID="deValue" class="value" runat="server" />   
    </div>
    <asp:CheckBox ID="cbValue" class="value" runat="server" Visible="false" />    
</div>
--%>


<div id="paField" class="dbField form-group" runat="server">
    <asp:Label ID="lbLabel" class="label" runat="server" Visible="false" ></asp:Label>
    <asp:RequiredFieldValidator ID="rqValidator" runat="server" SetFocusOnError="True" Display="Dynamic"
        Enabled="false"
        ValidationGroup="vg" 
        ControlToValidate="tbValue" 
        ErrorMessage="Pole wymagane" >
    </asp:RequiredFieldValidator>
    <asp:Label ID="lbValue" class="value" runat="server" Visible="false" ></asp:Label>
    <asp:TextBox ID="tbValue" class="value textbox form-control" runat="server" Visible="false" ></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="false" 
        TargetControlID="tbValue" 
        FilterType="Custom" 
        ValidChars="0123456789" />
    <asp:DropDownList ID="ddlValue" class="value form-control" runat="server" Visible="false" OnSelectedIndexChanged="ddlValue_SelectedIndexChanged" OnDataBound="ddlValue_DataBound" >
    </asp:DropDownList>
        <asp:RadioButtonList ID="rblValue" class="value" runat="server" Visible="false" onselectedindexchanged="ddlValue_SelectedIndexChanged" AutoPostBack="true">
    </asp:RadioButtonList>
    <div id="paDateEdit" runat="server" class="dateedit" Visible="false">
        <uc1:DateEdit ID="deValue" class="value" runat="server" />   
    </div>
    <asp:CheckBox ID="cbValue" class="value" runat="server" Visible="false"  />
    <asp:CustomValidator runat="server" id="cbValidator" OnServerValidate="custCheck_ServerValidate" Display="Dynamic" CssClass="error"
        ErrorMessage="Pole wymagane" Enabled="false" ClientValidationFunction="ValidateCheckBox" />
</div>