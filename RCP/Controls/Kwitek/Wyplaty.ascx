<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Wyplaty.ascx.cs" Inherits="HRRcp.Controls.Kwitek.Wyplaty" %>

<asp:GridView ID="gvWyplaty" runat="server" DataSourceID="SqlDataSource1" 
    AllowSorting="false" 
    CellPadding="4" 
    CssClass="GridView1 gvWyplaty"
    DataKeyNames="NrListy" 
    onrowcommand="gvWyplaty_RowCommand" 
    onrowdatabound="gvWyplaty_RowDataBound" 
    AllowPaging="True" PageSize="5" 
    ondatabound="gvWyplaty_DataBound" 
    onselectedindexchanged="gvWyplaty_SelectedIndexChanged" 
    OnPageIndexChanging="gvWyplaty_PageIndexChanging"
    OnPageIndexChanged="gvWyplaty_PageIndexChanged"
    onsorted="gvWyplaty_Sorted">
    <PagerSettings Mode="NumericFirstLast" />
    <RowStyle BackColor="#EFF3FB" />
    <Columns>
        <asp:CommandField ShowSelectButton="True" SelectText="Pokaż" />
        <asp:ButtonField CommandName="Details" Text="Szczegóły" Visible="false"/>
    </Columns>
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" CssClass="pager" Font-Overline="False" Font-Strikeout="False" />
    <SelectedRowStyle Font-Bold="False" CssClass="selected" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#2461BF" />
    <AlternatingRowStyle BackColor="White" />
    <EmptyDataTemplate>
        Brak danych
    </EmptyDataTemplate>
</asp:GridView>

<asp:HiddenField ID="gvWyplatyHeader" runat="server" Value="control|-|-|-|b||N2||N2b|N2|D|||" Visible="false"/>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ASSECO %>" >
</asp:SqlDataSource>







