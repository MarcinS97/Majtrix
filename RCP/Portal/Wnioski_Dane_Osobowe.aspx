<%@ Page Title="" Language="C#" Debug="true" MasterPageFile="~/Portal.Master" CodeBehind="Wnioski_Dane_Osobowe.aspx.cs" Inherits="HRRcp.Portal.PortalWnioski_Dane_Osobowe" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/Wniosek_dane_osobowe.ascx" TagName="Wniosek" TagPrefix="muc1" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/GetName.ascx" TagName="GetName" TagPrefix="muc1" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/cntzoom.ascx" TagName="Zoom" TagPrefix="muc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="caption" >
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="/images/captions/Struktura.png"/>
                    Wnioski Dane Osobowe
                </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>    

    <div id="cntWnioski_Dane_Osobowe" class="tabsContent">
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
           <muc1:Wniosek ID="Wniosek1" runat="server" />                  
        </ContentTemplate>
        </asp:UpdatePanel>

        

    </div>
    
  
</asp:Content>

