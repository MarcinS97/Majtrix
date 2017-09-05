<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntFilter.ascx.cs" Inherits="HRRcp.Controls.EliteReports.cntFilter" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="leet" TagName="DateEdit" %>

<style type="text/css">
    #<%=updMain.ClientID%>
    {
        display: inline-block;

    }

</style>


<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional"  >
    <ContentTemplate >
        
        <asp:Label ID="lblFirst" runat="server" Visible="false" />
        <asp:DropDownList ID="ddlMain" runat="server" Visible="false" />
        <leet:DateEdit id="deMain" runat="server" Visible="false" />
        <asp:Label ID="lblSecond" runat="server" Visible="false" />
        <leet:DateEdit id="deMain2" runat="server" Visible="false" />
        <asp:TextBox ID="tbMain" runat="server" Visible="false" />
        <asp:CheckBox ID="cbMain" runat="server" Visible="false" />
 
        
    </ContentTemplate>
</asp:UpdatePanel>