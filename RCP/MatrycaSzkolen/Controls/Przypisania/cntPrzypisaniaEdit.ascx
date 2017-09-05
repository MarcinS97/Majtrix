<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzypisaniaEdit.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Przypisania.cntPrzypisaniaEdit" %>
<%@ Register src="cntStruktura.ascx" tagname="cntStruktura" tagprefix="uc1" %>
<%@ Register src="cntPrzypisaniaParametry.ascx" tagname="cntPrzypisaniaParametry" tagprefix="uc3" %>

<asp:HiddenField ID="hidData" runat="server" />

<div id="paPzypisaniaEdit" class="cntPrzypisaniaEdit" runat="server">
    <table class="table0">
        <tr id="trTyp" runat="server" class="typ" visible="false">
            <td colspan="2"> 
                <span class="title">Typ wniosku:</span>
                <asp:RadioButtonList ID="rbList" CssClass="check" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbList_SelectedIndexChanged">
                    <asp:ListItem Value="1" Selected="True">Wniosek o oddelegowanie mojego pracownika</asp:ListItem>
                    <asp:ListItem Value="0">Wniosek o oddelegowanie pracownika obcego do mnie</asp:ListItem>
                </asp:RadioButtonList>
                <br />
            </td>
        </tr>
        <tr class="title">
            <td>
                1. Wybierz pracownika do przesunięcia:
            </td>
            <td>
                2. Wybierz docelowego przełożonego i podaj parametry:
            </td>            
        </tr>
        <tr class="struktura">
            <td class="str1" >
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>                        
                        <uc1:cntStruktura ID="cntStruktura1" runat="server" Mode="ALL" ScrollIndex="1" OnSelectedChanged="cntStruktura1_SelectedChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>            
            </td>
            <td class="str2" >
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>                        
                        <uc1:cntStruktura ID="cntStruktura2" runat="server" Mode="KIER" ScrollIndex="2" OnSelectedChanged="cntStruktura2_SelectedChanged" /> 
                    </ContentTemplate>
                </asp:UpdatePanel>            
            </td>            
        </tr>
        <tr class="params">
            <td class="str1">
                <div class="ramka">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always" >
                        <ContentTemplate>                        
                            <uc3:cntPrzypisaniaParametry ID="cntParametry1" Mode="INFO" runat="server" 
                                OnMoveSettings="cnt_MoveSettings" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </div>
            </td>
            <td class="str2">
                <div class="ramka">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" >
                        <ContentTemplate>                        
                            <uc3:cntPrzypisaniaParametry ID="cntParametry2" Mode="ADDKIER" runat="server" 
                                OnStructureChanged="cntParamerty2_StructureChanged" 
                                OnMoveSettings="cnt_MoveSettings" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </div>
            </td>
        </tr>
    </table>
</div>
