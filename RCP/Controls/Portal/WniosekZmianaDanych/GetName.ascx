<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetName.ascx.cs" Inherits="HRRcp.Controls.WnioseZmianaDanych.GetName" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/cntSqlContent2.ascx" TagName="SqlContent" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/GetID.ascx" TagName="GetID" TagPrefix="muc1" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/cntlistview.ascx" TagName="Listview" TagPrefix="muc1" %>
<div id="cntGetName">

<div style="width:300px;">
<div id="paFilter" runat="server" class="paFilter">
    <asp:HiddenField ID="PracId" runat="server" />
    <asp:HiddenField ID="Nrewid" runat="server" />

<span class="label">Pracownik :</span>
 <asp:DropDownList ID="DropDownList1" DataSourceID="SqlDataSource3" runat="server"
            AutoPostBack="true"
                            DataTextField="name" DataValueField="Id" 
                            onselectedindexchanged="ddlLogin_SelectedIndexChanged"
                            
                           
                            >
                            
                            
            </asp:DropDownList>
</div>
    <asp:ListView ID="ListView1"  runat="server" DataSourceID="SqlDataSource1"> 
        <ItemTemplate>
            <tr class="it">
                <td>
                    <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
                </td>
                <td class="control">
                <asp:Button ID="Button2" OnCommand="GetId" CommandArgument='<%# Eval("Id") %>' runat="server" Text="Nowy Wniosek" />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>
                       Wybierz opcję !</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table class="ListView1 hoverline" runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server">
                                    Nazwa</th>
                                <th runat="server">
                                    </th>
                                                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
    </div>
    


    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
    
<muc1:Listview ID="Listview2" Lvl="1" cntnr="2" runat="server" />
</ContentTemplate>
    </asp:UpdatePanel>
    
       <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate> 
<muc1:GetID ID="GetID" runat="server" Visible="false"  />
    </ContentTemplate>
    </asp:UpdatePanel> 
</div>
 
<div>
    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
<uc1:SqlContent ID="SqlContent" runat="server" Visible="false" />

    </ContentTemplate>
    </asp:UpdatePanel>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    ></asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" ></asp:SqlDataSource>
           <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="

                select 
(Imie + ' ' + Nazwisko + ' (' + KadryId + ') ') as name ,
Id
from Pracownicy order by Nazwisko" ></asp:SqlDataSource>