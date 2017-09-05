<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimeEnter.ascx.cs" Inherits="HRRcp.Controls.DateTimeEnter" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:TextBox ID="tbDate" CssClass="textbox" runat="server" Width="90"></asp:TextBox>
<asp:ImageButton ID="btDate" class="ico" width="16" runat="server" ImageUrl="~/images/buttons/calendar.png" />
<asp:CalendarExtender ID="CalendarExtender" runat="server" 
    TargetControlID="tbDate" DaysModeTitleFormat="yyyy MMMM" PopupPosition="BottomLeft" 
    TodaysDateFormat="yyyy-MM-dd" CssClass="calendar" 
    OnClientDateSelectionChanged="CalendarChanged"></asp:CalendarExtender>
<asp:CalendarExtender ID="CalendarExtenderButton" runat="server" 
    TargetControlID="tbDate" DaysModeTitleFormat="yyyy MMMM" PopupButtonID="btDate" 
    TodaysDateFormat="yyyy-MM-dd" CssClass="calendar"
    OnClientDateSelectionChanged="CalendarChanged"></asp:CalendarExtender>
<asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="False" 
    TargetControlID="tbDate" FilterType="Numbers" />

<asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
    TargetControlID="tbDate" 
    Mask="9999-99-99"
    MessageValidatorTip="false"
    OnFocusCssClass="MaskedEditFocus"
    OnInvalidCssClass="MaskedEditError"
    ClearMaskOnLostFocus="false"
    MaskType="None"
    AcceptAMPM="True"
    ErrorTooltipEnabled="True" />
<asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server"
    ControlExtender="MaskedEditExtender1"
    ControlToValidate="tbDate"
    IsValidEmpty="true"
    EmptyValueMessage=""
    InvalidValueMessage="Niepoprawny format"
    Display="Dynamic"
    TooltipMessage="Podaj datę"
    EmptyValueBlurredText=""
    InvalidValueBlurredMessage="Niepoprawna wartość"
    ValidationGroup="MKE"/>










<asp:TextBox ID="tbTime" CssClass="textbox timeedit" runat="server" Width="60" ValidationGroup="MKE" ></asp:TextBox>

<asp:MaskedEditExtender ID="MaskedEditExtender" runat="server"
    TargetControlID="tbTime" 
    Mask="99:99"
    MessageValidatorTip="false"
    OnFocusCssClass="MaskedEditFocus"
    OnInvalidCssClass="MaskedEditError"
    MaskType="None"
    ClearMaskOnLostFocus="false"
    AcceptAMPM="false"
    ErrorTooltipEnabled="True" />
<asp:MaskedEditValidator ID="MaskedEditValidator" runat="server"
    ControlExtender="MaskedEditExtender"
    ControlToValidate="tbTime"
    IsValidEmpty="true"
    EmptyValueMessage=""
    InvalidValueMessage="Niepoprawny format"
    Display="Dynamic"
    TooltipMessage="Podaj czas"
    EmptyValueBlurredText=""
    InvalidValueBlurredMessage="Niepoprawna wartość"
    ValidationGroup="MKE"/>
