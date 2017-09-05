<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntlistview.ascx.cs" Inherits="HRRcp.Controls.WnioseZmianaDanych.cntlistview" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="muc1" %>
<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
<ContentTemplate>



<div runat="server" id="cntListview">

    
<asp:Literal ID="ZoomDiv" runat="server"></asp:Literal>
    <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
    <ContentTemplate>

   
     
        <div id="divInfo" runat="server" class="info">
 <asp:HiddenField ID="WniosekId"  runat="server" />              
              
            
<div id="Dane_Pracownika" visible="false" runat="server">
    <asp:Label ID="Label3" runat="server" Text="Status:"></asp:Label>
    <asp:Label ID="Status_Wniosku" runat="server" Text="Status"></asp:Label><br />
    <asp:Label ID="Label1" runat="server" Text="Imię i Nazwisko: "></asp:Label>
    <asp:Label ID="Imie_Nazwisko" runat="server" Text="Imię i Nazwisko"></asp:Label><br />
    <asp:Label ID="Label2" runat="server" Text="Numer ewidencyjny:"></asp:Label>
    <asp:Label ID="Nrewid" runat="server" Text="Numer ewidencyjny"></asp:Label>
    <br />
    <b><asp:Label ID="Temat" runat="server" Text="Wniosek"></asp:Label></b>
</div>
<br />                                      
            
              
    <asp:ListView ID="ListView1" DataSourceID="SqlDataSource1" runat="server"  
        onitemdatabound="ListView1_ItemDataBound" 
        ondatabinding="ListView1_DataBinding" ondatabound="ListView1_DataBound" >
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="Nazwa_polaLabel" runat="server" 
                        Text='<%# Eval("Nazwa_pola") %>' />
                </td>
                <td>
                    <asp:Label ID="Obecna_wartoscLabel" runat="server" 
                        Text='<%# Eval("Obecna_wartosc") %>' />
                </td>
                <td>

                <muc1:DateEdit ID="DateEditCnt" runat="server" />
                <asp:TextBox ID="TextBoxCnt" MaxLength="240" runat="server" ></asp:TextBox>
                <asp:Label ID="LabelCnt" runat="server" Text=""></asp:Label>
                
                   </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="Table2" runat="server">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th1" runat="server">
                                    Pole</th>
                                <th id="Th2"  runat="server">
                                    <asp:Label ID="ObWartosclb" runat="server" Text="Obecna Wartość"></asp:Label></th>
                                <th id="Th3" runat="server">
                                <asp:Label ID="NwWartosclb" runat="server" Text="Nowa Warość"></asp:Label></th>
                                </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr3" runat="server">
                    <td id="Td2" runat="server" style="">
                    </td>
                </tr>
            </table>
<div runat="server" id="uzasadnienieDiv">
    
<b>Uzasadnienie :</b>
 <asp:TextBox ID="Uzasadnienie" Height="60" onkeypress="return isMaxLen(this, 200);" onpaste="cutMaxLen(this, 200);" onblur="return checkMaxLen(this, 200);" runat="server"></asp:TextBox>
</div>
            <br>
</br>
            <asp:Button ID="Bt_Wyslij" runat="server" CssClass="button100" Text="Wyślij Wniosek" style="width:auto;" OnClick="Button1_Click" />
            <asp:Button ID="Bt_Usun" CssClass="button100" runat="server" Text="Anuluj" OnClick="Button2_Click" />
            <asp:Button ID="Bt_Zapisz" CssClass="button100" runat="server" Text="Zapisz" OnClick="Button3_Click" />
            <asp:Button ID="Bt_Akceptuj" CssClass="button100" runat="server" Text="Akceptuj" OnClick="Bt_Akceptuj" />
            <asp:Button ID="Bt_AW" CssClass="button100" runat="server" style="width:auto;" Text="Akceptuj-Wprowadzone" OnClick="Bt_AW" />
            <asp:Button ID="Bt_Odrzuc" CssClass="button100" runat="server" Text="Odrzuć" OnClientClick="return confirm('Odrzucić wniosek ?');" OnClick="Bt_Odrzuc" />
            <asp:Button ID="Bt_Cofnij" CssClass="button100" runat="server" Text="Cofnij Status" style="width:auto;" OnClientClick="return confirm('Cofnąć status ?');" OnClick="Bt_Cofnij" />
            <asp:Button ID="Bt_Wprowadzone" CssClass="button100" runat="server" Text="Wprowadzone" OnClick="Bt_Wprowadzone" />
            <asp:Button ID="Bt_Zamknij" CssClass="button100" runat="server" Text="Zamknij" OnClick="Bt_Zamknij" />
            
        </LayoutTemplate>
       
    </asp:ListView>
    <br />
    
            
</div>
 <asp:Button  ID="Bt_Dialog_Close" style="display:none;" runat="server" Text="Button" 
            onclick="Bt_Dialog_Close_Click" />
        <asp:Button ID="TAK" style="display:none;" runat="server" Text="Button" onclick="TAK_Click" />
        <asp:Button ID="NIE" style="display:none;" runat="server" Text="Button" onclick="NIE_Click" />
        
</ContentTemplate>
    </asp:UpdatePanel>

     </div>
   
</div>
</ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="HiddenField1" runat="server" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    ></asp:SqlDataSource>
