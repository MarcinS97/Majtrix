<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeEdit.ascx.cs" Inherits="HRRcp.Controls.TimeEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:TextBox ID="tbTime" CssClass="textbox timeedit form-control" runat="server" Width="60px" MaxLength="5"></asp:TextBox>
<asp:Label ID="lbOpis" CssClass="t4n" runat="server" ></asp:Label>
<asp:Label ID="lbError" CssClass="t4n error" Visible="false" runat="server" Text="Błąd!"></asp:Label>

<asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbTime" 
    FilterType="Custom" 
    ValidChars="0123456789:" />
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="false" Display="Dynamic" CssClass="error"
    Enabled="false"
    ControlToValidate="tbTime" 
    ErrorMessage="Błąd" >
</asp:RequiredFieldValidator>