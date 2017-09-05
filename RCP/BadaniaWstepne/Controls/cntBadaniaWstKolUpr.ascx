<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntBadaniaWstKolUpr.ascx.cs" Inherits="HRRcp.BadaniaWstepne.Controls.cntBadaniaWstKolUpr" %>

<div id="paUprawnienia" runat="server" class="cntBadaniaWstAdm">
    <table class="caption">
        <tr>
            <td class="left">
                <span class="caption4">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/captions/layout_edit.png"/>
                    Badania wstępne - Administracja
                </span>
            </td>
            <td class="left1">
                <asp:Label ID="Label9" runat="server" class="label" Text="Wyszukaj:" />
            </td>
            <td class="middle">
                <asp:TextBox ID="tbSearch" CssClass="search textbox" runat="server" ></asp:TextBox>
                <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" />
            </td>
            <td class="right">
                <asp:Button ID="btBack" runat="server" CssClass="button75" Text="Powrót" OnClientClick="showAjaxProgress();history.back();return false;" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />

            <%-- page --%>
            <div class="pageContent">
                <div class="padding">

                    <asp:GridView ID="gvUprawnienia" runat="server" CssClass="GridView1" 
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="true" 
                        DataKeyNames="pid:-" 
                        DataSourceID="SqlDataSource1" 
                        ondatabound="gvUprawnienia_DataBound">
                    </asp:GridView>
                    <asp:Button ID="gvUprawnieniaCmd" runat="server" CssClass="button_postback" Text="Button" onclick="gvUprawnieniaCmd_Click" />
                    <asp:HiddenField ID="gvUprawnieniaCmdPar" runat="server" />
                    
                    <div class="pager">
                        <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <span class="count">Pokaż na stronie:</span>
                        <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true"    
                            OnChange="showAjaxProgress();"
                            OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                            <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
<asp:HiddenField ID="hidUserAdm" runat="server" Visible="false" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    onload="SqlDataSource1_Load" 
    onselected="SqlDataSource1_Selected"    
    SelectCommand="
SELECT 
B.Id [pid:-], 
B.KadryId [kk:-],
B.Imie [ii:-], 
B.Nazwisko [nn:-], 
B.Nazwisko + ' ' + B.Imie [Pracownik], 
B.KadryId [Nr ew.],
case B.Kierownik when 1 then 'K' else '' end [Kierownik],
case dbo.GetRight(B.Id, 53) when 1 then 'x' else '-' end [Dostęp|js:setPrawaCC(this 1 @pid '53')|Dostęp do rejestru],
case dbo.GetRight(B.Id, 61) when 1 then 'x' else '-' end [Dodawanie|js:setPrawaCC(this 1 @pid '61')|Dodawanie i usuwanie pracowników],
'Ustaw' [Ustaw wszystkie:;control|cmd:set @pid|Ustaw wszystkie uprawnienia],
{0}
,B.Nazwisko + ' ' + B.Imie [Pracownik ] 
FROM BadaniaWstKolUpr A
RIGHT JOIN Pracownicy B on A.IdPrac = B.Id
where B.Status != -1 and (ISNUMERIC(B.KadryId) != 1 or B.KadryId &lt; 80000)
order by B.Kierownik desc, B.Nazwisko, B.Imie, B.KadryId 
    ">
</asp:SqlDataSource>

<%--
--%>