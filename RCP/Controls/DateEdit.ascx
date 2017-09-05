<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateEdit.ascx.cs" Inherits="HRRcp.Controls.DateEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="ctDateEdit" runat="server" class="dateedit">
    <asp:TextBox ID="tbDate" runat="server" CssClass="textbox dateedit" MaxLength="10" ontextchanged="tbDate_TextChanged" Visible="true" ></asp:TextBox>
    <asp:ImageButton ID="btDate" runat="server" CssClass="ico dateico" ImageUrl="~/images/buttons/calendar.png" Visible="true" />
    
    
    <asp:TextBox ID="tbDate2" runat="server" CssClass="datepicker textbox dateedit form-control" MaxLength="10" OnTextChanged="tbDate_TextChanged" Visible="true" AutoPostBack="false"></asp:TextBox>
    <%--<asp:LinkButton ID="btDate2" runat="server" class="fa fa-calendar datepickero" PostBackUrl="javascript://" Visible="true"  />--%>
    
</div>
<asp:CalendarExtender ID="CalendarExtender" runat="server" 
    TargetControlID="tbDate" DaysModeTitleFormat="yyyy MMMM" 
    
    PopupPosition="BottomRight" 
    
    TodaysDateFormat="yyyy-MM-dd" CssClass="calendar"></asp:CalendarExtender>
<asp:CalendarExtender ID="CalendarExtenderButton" runat="server" 
    TargetControlID="tbDate" DaysModeTitleFormat="yyyy MMMM" PopupButtonID="btDate"
    
    PopupPosition="BottomRight" 
     
    TodaysDateFormat="yyyy-MM-dd" CssClass="calendar"></asp:CalendarExtender>
<asp:Label ID="lbOpis" CssClass="t4n" runat="server" ></asp:Label>
<asp:Label ID="lbError" CssClass="t4n error" Visible="false" runat="server" Text="Błąd!"></asp:Label>

<asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbDate" 
    FilterType="Custom" 
    ValidChars="0123456789-" />
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="false" Display="Dynamic" CssClass="error"
    Enabled="false"
    ControlToValidate="tbDate" 
    ErrorMessage="Pole wymagane" >
</asp:RequiredFieldValidator>
