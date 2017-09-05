<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmParamsControl.ascx.cs" Inherits="HRRcp.Controls.AdmParamsControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>

<%@ Register src="AdmUstawienia.ascx" tagname="AdmUstawienia" tagprefix="uc2" %>
<%@ Register src="AdmOkresy.ascx" tagname="AdmOkresy" tagprefix="uc2" %>
<%@ Register src="AdmKierParams.ascx" tagname="AdmKierParams" tagprefix="uc2" %>

<%@ Register src="AdmCC.ascx" tagname="AdmCC" tagprefix="uc3" %>
<%@ Register src="Linie.ascx" tagname="Linie" tagprefix="uc3" %>
<%@ Register src="Splity.ascx" tagname="Splity" tagprefix="uc3" %>

<%@ Register src="AdmKodyAbs.ascx" tagname="AdmKodyAbs" tagprefix="uc1" %>

<%@ Register src="Przypisania/cntDicCommodity.ascx" tagname="cntDicCommodity" tagprefix="uc4" %>
<%@ Register src="Przypisania/cntDicArea.ascx" tagname="cntDicArea" tagprefix="uc4" %>
<%@ Register src="Przypisania/cntDicPosition.ascx" tagname="cntDicPosition" tagprefix="uc4" %>

<%@ Register src="Adm/cntDicMarginesy.ascx" tagname="cntDicMarginesy" tagprefix="uc5" %>
<%@ Register src="Adm/cntDicKody.ascx" tagname="cntDicKody" tagprefix="uc5" %>

<%@ Register src="~/Controls/DzialyControl.ascx" tagname="DzialyControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanUrlopow/cntPominAdm.ascx" tagname="cntPominAdm" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanUrlopow/cntAdmin.ascx" tagname="cntAdmin" tagprefix="uc1" %>
<%@ Register src="~/Controls/Adm/cntGrupyUprawnien.ascx" tagname="cntGrupyUprawnien" tagprefix="uc1" %>

<%--
                    <td class="control">
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                    </td>
                    
                    <td class="control">
                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                        <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
                    </td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" 
                        ControlToValidate="KodTextBox" 
                        SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>

                    <td class="control">
                        <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert" />
                        <asp:Button ID="ClearButton" runat="server" CommandName="Clear" Text="Wyczyść" />
                    </td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" 
                        ControlToValidate="KodTextBox" 
                        SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
--%>



<table class="tabsContent" style="height:580px; background-color:#FFF;">
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" 
                onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Okresy rozliczeniowe" Value="OR" Selected="true" ></asp:MenuItem>
                    <asp:MenuItem Text="Ustawienia kierowników" Value="KP"></asp:MenuItem>
                    <asp:MenuItem Text="Struktura:" Value="DIC3" Selectable="false" Enabled="false" ></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Commodities" Value="vCommodity"></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Areas" Value="vArea"></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Positions" Value="vPosition"></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Działy i stanowiska" Value="vDzialy"></asp:MenuItem>                        
                    <asp:MenuItem Text="Podział kosztów:" Value="DIC1" Selectable="false" Enabled="false" ></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Centra kosztowe" Value="CC"></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Splity" Value="SP"></asp:MenuItem>
                    <asp:MenuItem Text="Słowniki:" Value="DIC2" Selectable="false" Enabled="false" ></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Kody absencji" Value="KA"></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Stawki godzinowe" Value="SG"></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Algorytmy" Value="AL"></asp:MenuItem> 
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Marginesy we/wy" Value="vMarginesy"></asp:MenuItem> 
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Kody" Value="vKody"></asp:MenuItem> 
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Grupy uprawnień" Value="vGrupyUpr"></asp:MenuItem> 
                    <asp:MenuItem Text="Plan urlopów:" Value="DIC4" Selectable="false" Enabled="false" ></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Parametry" Value="PUADM"></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Absencje długotrwałe" Value="ABSDL"></asp:MenuItem>
                    <asp:MenuItem Text="Parametry" Value="PA" ></asp:MenuItem>
                </Items>
            </asp:Menu>
            <asp:Button ID="btLeftMenuSelect" class="button_postback" runat="server" onclick="btLeftMenuSelect_Click" Text="Wybierz" />
        </td>
        
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvParams" runat="server" ActiveViewIndex="0">
                <asp:View ID="pgParams" runat="server" >
                    <uc2:AdmUstawienia ID="cntUstawienia" runat="server" />
                </asp:View>

                <asp:View ID="pgOkresy" runat="server" >
                    <uc2:AdmOkresy ID="cntOkresy" runat="server" />
                </asp:View>

                <asp:View ID="pgStawki" runat="server" >
                    <asp:ListView ID="lvStawki" runat="server" DataSourceID="SqlDataSource4" 
                        DataKeyNames="Stawka"
                        InsertItemPosition="LastItem" 
                        onitemdatabound="lvAlgorytmy_ItemDataBound" >
                        <ItemTemplate>
                            <tr class="it">
                                <td>
                                    <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") %>' /> %
                                </td>
                                <td>
                                    <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
                                </td>
                                <td class="check">
                                    <asp:CheckBox ID="AktywnaCheckBox" runat="server" Checked='<%# Eval("Aktywna") %>' Enabled="false" />
                                </td>
                                <td class="control">
                                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
                                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table id="Table5" runat="server" style="">
                                <tr>
                                    <td>
                                        Brak danych
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="Table6" runat="server" class="ListView1 tbStawki hoverline">
                                <tr id="Tr7" runat="server">
                                    <td id="Td5" runat="server">
                                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                            <tr id="Tr8" runat="server" style="">
                                                <th id="Th13" runat="server">Stawka</th>
                                                <th id="Th14" runat="server">Nazwa</th>
                                                <th id="Th15" runat="server">Aktywna</th>
                                                <th id="Th16" runat="server" class="control"></th>
                                            </tr>
                                            <tr ID="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="Tr9" runat="server">
                                    <td id="Td6" runat="server" style="">
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <EditItemTemplate>
                            <tr class="eit">
                                <td class="num">
                                    <asp:TextBox ID="StawkaTextBox" runat="server" Text='<%# Bind("Stawka") %>' /> %
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ValidationGroup="vge1" 
                                        ControlToValidate="StawkaTextBox" 
                                        ErrorMessage="Błąd" 
                                        SetFocusOnError="True" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                        ValidationGroup="vge1"
                                        ControlToValidate="StawkaTextBox"
                                        ErrorMessage="Błąd" 
                                        MinimumValue=0
                                        MaximumValue=1000
                                        Type=Integer
                                        SetFocusOnError="true" Display="Dynamic">
                                    </asp:RangeValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                                </td>
                                <td class="check">
                                    <asp:CheckBox ID="AktywnaCheckBox" runat="server" Checked='<%# Bind("Aktywna") %>' />
                                </td>
                                <td class="control">
                                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge1" />
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <tr class="iit">
                                <td class="num">
                                    <asp:TextBox ID="StawkaTextBox" runat="server" Text='<%# Bind("Stawka") %>' /> %
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ValidationGroup="vgi1" 
                                        ControlToValidate="StawkaTextBox" 
                                        ErrorMessage="Błąd" 
                                        SetFocusOnError="True" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                        ValidationGroup="vgi1"
                                        ControlToValidate="StawkaTextBox"
                                        ErrorMessage="Błąd" 
                                        MinimumValue=0
                                        MaximumValue=1000
                                        Type=Integer
                                        SetFocusOnError="true" Display="Dynamic">
                                    </asp:RangeValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' /> 
                                </td>
                                <td class="check">
                                    <asp:CheckBox ID="AktywnaCheckBox" runat="server" Checked='<%# Bind("Aktywna") %>' />
                                </td>
                                <td class="control">
                                    <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgi1" />
                                    <asp:Button ID="ClearButton" runat="server" CommandName="Clear" Text="Czyść" />
                                </td>
                            </tr>
                        </InsertItemTemplate>
                    </asp:ListView>

                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                        SelectCommand="SELECT * FROM [Stawki] ORDER BY [Stawka]" 
                        UpdateCommand="UPDATE [Stawki] SET [Nazwa] = @Nazwa, [Aktywna] = @Aktywna WHERE [Stawka] = @original_Stawka AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [Aktywna] = @original_Aktywna"
                        DeleteCommand="DELETE FROM [Stawki] WHERE [Stawka] = @original_Stawka AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [Aktywna] = @original_Aktywna" 
                        InsertCommand="INSERT INTO [Stawki] ([Stawka], [Nazwa], [Aktywna]) VALUES (@Stawka, @Nazwa, @Aktywna)" 
                        ConflictDetection="CompareAllValues" 
                        OldValuesParameterFormatString="original_{0}" >
                            <UpdateParameters>
                                <asp:Parameter Name="Nazwa" Type="String" />
                                <asp:Parameter Name="Aktywna" Type="Boolean" />
                                <asp:Parameter Name="original_Stawka" Type="Int32" />
                                <asp:Parameter Name="original_Nazwa" Type="String" />
                                <asp:Parameter Name="original_Aktywna" Type="Boolean" />
                            </UpdateParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="original_Stawka" Type="Int32" />
                                <asp:Parameter Name="original_Nazwa" Type="String" />
                                <asp:Parameter Name="original_Aktywna" Type="Boolean" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Stawka" Type="Int32" />
                                <asp:Parameter Name="Nazwa" Type="String" />
                                <asp:Parameter Name="Aktywna" Type="Boolean" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                </asp:View>

                <asp:View ID="pgAlgorytmy" runat="server">
                    <asp:ListView ID="lvAlgorytmy" runat="server" DataSourceID="SqlDataSource1" 
                        DataKeyNames="Id"
                        InsertItemPosition="LastItem" 
                        onitemdatabound="lvAlgorytmy_ItemDataBound" >
                        <ItemTemplate>
                            <tr class="it">
                                <td class="num">
                                    <asp:Label ID="Lp" runat="server" Text='<%# Eval("Lp") %>' />
                                </td>
                                <td class="col2">
                                    <asp:Label ID="Nazwa" runat="server" Text='<%# Eval("Nazwa") %>' />
                                </td>
                                <td class="num">
                                    <asp:Label ID="Kod" runat="server" Text='<%# Eval("Kod") %>' />
                                </td>
                                <td class="num">
                                    <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Parametr") %>' />
                                </td>
                                <td class="control">
                                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
                                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table id="Table1" runat="server" style="">
                                <tr>
                                    <td>
                                        Brak danych
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="Table2" class="ListView1 tbAlgorytmy hoverline" runat="server">
                                <tr id="Tr1" runat="server">
                                    <td id="Td1" runat="server">
                                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                            <tr id="Tr2" runat="server" style="">
                                                <th id="Th1" class="col1" runat="server">Lp</th>
                                                <th id="Th2" class="col2" runat="server">Nazwa</th>
                                                <th id="Th4" class="col1" runat="server">Kod</th>
                                                <th id="Th5" class="col1" runat="server">Parametr</th>
                                                <th id="Th3" class="control" runat="server"></th>
                                            </tr>
                                            <tr ID="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <EditItemTemplate>
                            <tr class="eit">
                                <td class="num">
                                    <asp:TextBox ID="TextBox1" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Lp") %>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td class="num">
                                    <asp:TextBox ID="KodTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Kod") %>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td class="num">
                                    <asp:TextBox ID="TextBox2" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Parametr") %>' />
                                </td>
                                <td class="control">
                                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <tr class="iit">
                                <td class="num">
                                    <asp:TextBox ID="KodTextBox" class="textbox" runat="server" Text='<%# Bind("Lp") %>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td class="col2">
                                    <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" Text='<%# Bind("Nazwa") %>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td class="num">
                                    <asp:TextBox ID="TextBox3" class="textbox" runat="server" Text='<%# Bind("Kod") %>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                                <td class="num">
                                    <asp:TextBox ID="TextBox4" class="textbox" runat="server" Text='<%# Bind("Parametr") %>' />
                                </td>
                                <td class="control">
                                    <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert" />
                                </td>
                            </tr>
                        </InsertItemTemplate>
                    </asp:ListView>

                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                        SelectCommand="SELECT * from [Kody] WHERE Typ='ALG' ORDER BY [Lp]" 
                        UpdateCommand="UPDATE Kody SET [Typ] = 'ALG', [Lp] = @Lp, [Nazwa] = @Nazwa, [Kod] = @Kod, [Parametr] = @Parametr WHERE [Id] = @Id"
                        DeleteCommand="DELETE FROM [Kody] WHERE [Id] = @Id" 
                        InsertCommand="INSERT INTO [Kody] ([Typ], [Lp], [Nazwa], [Kod], [Parametr]) VALUES ('ALG', @Lp, @Nazwa, @Kod, @Parametr)" >
                        <UpdateParameters>
                            <asp:Parameter Name="Id" Type="Int32" />
                            <asp:Parameter Name="Typ" Type="String" />
                            <asp:Parameter Name="Lp" Type="String" />
                            <asp:Parameter Name="Nazwa" Type="String" />
                            <asp:Parameter Name="Kod" Type="String" />
                            <asp:Parameter Name="Parametr" Type="String" />
                        </UpdateParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="Id" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Typ" Type="String" />
                            <asp:Parameter Name="Lp" Type="String" />
                            <asp:Parameter Name="Nazwa" Type="String" />
                            <asp:Parameter Name="Kod" Type="String" />
                            <asp:Parameter Name="Parametr" Type="String" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                </asp:View>

                <asp:View ID="vMarginesy" runat="server">
                    <uc5:cntDicMarginesy ID="cntDicMarginesy1" runat="server" />                    
                </asp:View>

                <asp:View ID="pgKodyAbsencji" runat="server">
                    <uc1:AdmKodyAbs ID="AdmKodyAbs" runat="server" />
                </asp:View>

                <asp:View ID="pgKierParams" runat="server" >                    
                    <uc2:AdmKierParams ID="cntAdmKierParams" runat="server" />
                </asp:View>

                <asp:View ID="pgCC" runat="server" >                    
                    <uc3:AdmCC ID="cntDicCC" runat="server" />
                </asp:View>

                <asp:View ID="pgLinie" runat="server" >                    
                    <uc3:Linie ID="cntLinie" runat="server" />
                </asp:View>

                <asp:View ID="pgSplity" runat="server" >                    
                    <uc3:Splity ID="cntSplity" runat="server" />
                </asp:View>

                <asp:View ID="vCommodity" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>                        
                            <uc4:cntDicCommodity ID="cntDicCommodity1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>
                
                <asp:View ID="vArea" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>                        
                            <uc4:cntDicArea ID="cntDicArea" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>
                
                <asp:View ID="vPosition" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>                        
                            <uc4:cntDicPosition ID="cntDicPosition" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>

                <asp:View ID="vKody" runat="server">
                    <uc5:cntDicKody ID="cntDicKody" runat="server" />                    
                </asp:View>

                <asp:View ID="vGrupyUpr" runat="server">
                    <uc1:cntGrupyUprawnien ID="cntGrupyUprawnien" runat="server" />                    
                </asp:View>

                <asp:View ID="vDzialy" runat="server">
                    <uc1:DzialyControl ID="Dzialy" runat="server" />
                </asp:View>

                <asp:View ID="vAbsDl" runat="server">
                    <uc1:cntPominAdm ID="cntPominAdm" runat="server" />
                </asp:View>

                <asp:View ID="vPlanUAdm" runat="server">
                    <uc1:cntAdmin ID="cntAdmin" runat="server" />
                </asp:View>

            </asp:MultiView>

        </td>
    </tr>
</table>