<%@ Control Language="C#" AutoEventWireup="true"  CodeBehind="cntzoom.ascx.cs" Inherits="HRRcp.Controls.WniosekZmianaDanych.cntzoom" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/cntlistview.ascx" TagName="listview" TagPrefix="muc1" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="muc1" %>



<div id="cntzoom">
<div runat="server" id="tbsearchdiv">
<span style="display: inline-block; xmargin-bottom: 2px; color: Gray; width: 150px;" class="label">Wyszukaj pracownika</span>
<asp:TextBox ID="tbSearch" runat="server" style=" width: 350px; margin: 0px; "  CssClass="search textbox"></asp:TextBox>
<br />
</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<div id="paFilter" runat="server" class="paFilter">



<span id="LabelPrac" runat="server" class="label">Pracownik :</span>
 <asp:DropDownList ID="DropDownList1" DataSourceID="SqlDataSource2" runat="server"
                            AutoPostBack="true"
                            DataTextField="name" DataValueField="Id" 
                            onselectedindexchanged="ddlLogin_SelectedIndexChanged" >
                            
                            
            </asp:DropDownList>
            <br />
  <div runat="server" id="DivStatus">

<span  class="label">Status :</span>
 <asp:DropDownList ID="DropDownList3" DataSourceID="SqlDataSource3" runat="server"
                            AutoPostBack="true"
                            DataTextField="Status" DataValueField="Id" 
                            onselectedindexchanged="DDL3_SelectedIndexChanged" >
                            
                            
            </asp:DropDownList>  
            </div>
           
 


 <span class="label">Typ wniosku :</span> 
             <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true"
            onselectedindexchanged="DDL_Typ" >
                <asp:ListItem Value="100" Text="Wybierz" Selected="True"></asp:ListItem>
                <asp:ListItem Value="0" Text="Nowe informacje" ></asp:ListItem>
                <asp:ListItem Value="1" Text="Modyfikacja"></asp:ListItem>
                <asp:ListItem Value="2" Text="Kasowanie informacji"></asp:ListItem>  
            </asp:DropDownList>
            <br />
       <span class="label">Okres Od :</span> 
            <muc1:DateEdit ID="Start1" AutoPostBack="true" runat="server" /> do : <muc1:DateEdit ID="End1" AutoPostBack="true" runat="server" />
   </div>

   
 <asp:Button ID="Button1" runat="server" Text="Szukaj" style="display:none;" CssClass="button75" onclick="Button1_Click" />
<asp:Button ID="Button3" runat="server" Text="Czyść" CssClass="button75" 
        onclick="Button3_Click" />




    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
    <ContentTemplate>



<asp:ListView ID="ListView1"  DataSourceID="SqlDataSource1" 
        runat="server" ondatabinding="ListView1_DataBinding" 
            ondatabound="ListView1_DataBound" >
    <ItemTemplate>
        <tr class="it">
        <td>
                <asp:Label ID="imie" runat="server" Text='<%# Eval("imie") %>' />
            </td>
            <td>
                <asp:Label ID="DataDodLabel" runat="server" Text='<%# Eval("DataDod") %>' />
            </td>
            <td>
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td>
                <asp:Label ID="Typ" runat="server" Text='<%# Eval("typ") %>' />
            </td>
           <td class="control">
                <asp:Button ID="Button2" OnCommand="GetId" CommandArgument='<%# Eval("Id") %>' runat="server" Text="Zobacz" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    No data was returned.</td>
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
                         <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="imie" Text="Pracownik"/>
                        
                        </th>
                            <th runat="server">
                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="DataDod" Text="Data dodania"/>
                                </th>
                            <th runat="server">
                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status"/>
                                </th>
                        <th runat="server">
                        <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="typ" Text="Typ"/>
                                </th>
                        <th runat="server">
                                      </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                    <asp:DataPager ID="DataPager1" OnLoad="DataPager1_OnLoad" runat="server" >
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                 
            </tr>

        </table>
    </LayoutTemplate>
    </asp:ListView>
    
     </ContentTemplate>
    </asp:UpdatePanel> 

     </ContentTemplate>
    </asp:UpdatePanel> 


    <asp:HiddenField ID="PracIdGetName" runat="server" />
</div>





            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
    <muc1:listview ID="Listview20" Lvl="1" cntnr="1" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" ></asp:SqlDataSource>
           <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="

                 select 
(Imie + ' ' + Nazwisko + ' (' + KadryId + ') ') as name ,
Id
from Pracownicy order by Nazwisko" ></asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select * from poWnioskiUrlopoweStatusy"></asp:SqlDataSource>
