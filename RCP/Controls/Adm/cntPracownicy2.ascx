<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPracownicy2.ascx.cs" Inherits="HRRcp.Controls.cntPracownicy2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>

<asp:HiddenField ID="hidSelectedRcpId" runat="server" />
<asp:HiddenField ID="hidSelectedStrefaId" runat="server" />

<asp:ListView ID="lvPracownicy" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" 
    onlayoutcreated="lvPracownicy_LayoutCreated" 
    onitemdatabound="lvPracownicy_ItemDataBound" 
    onitemcommand="lvPracownicy_ItemCommand" 
    onprerender="lvPracownicy_PreRender" 
    onitemupdating="lvPracownicy_ItemUpdating" 
    onselectedindexchanged="lvPracownicy_SelectedIndexChanged" 
    oniteminserting="lvPracownicy_ItemInserting" 
    onitemcreated="lvPracownicy_ItemCreated" 
    onitemupdated="lvPracownicy_ItemUpdated" >
    <ItemTemplate>
        <tr class="it" id="trLine" runat="server">
            <td class="nazwisko_nrew">
                <asp:LinkButton ID="NazwiskoLinkButton" runat="server" Text='<%# Eval("NazwiskoImie") %>' CommandName="Select" CommandArgument='<%# Eval("RcpId") + "|" + Eval("StrefaId") %>' ></asp:LinkButton><br />
                <span class="line2">
                    <asp:Label ID="KadryIdLabel" runat="server" CssClass="left" Text='<%# Eval("KadryId") %>' />
                    <asp:Label ID="RcpLabel" runat="server" Text='<%# Eval("RcpId") %>' />
                </span>
            </td>
            <td class="check">
                <asp:CheckBox ID="IsKierCheckBox" class="check" runat="server" Checked='<%# Eval("Kierownik") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Eval("Mailing") %>' Enabled="false" />
            </td>

            <%--
            <td class="strefa">
                <asp:Label ID="StrefaLabel" runat="server" Text='<%# Eval("Strefa") %>' ToolTip='<%# Eval("Strefa") %>' /><br />
                <asp:Label ID="AlgLabel" runat="server" CssClass="line2" Text='<%# Eval("Algorytm") %>' />
            </td>
            --%>
            
            <td class="kierownik">
                <asp:Label ID="LiniaLabel" runat="server" Text='<%# Eval("LiniaNazwa") %>' /><br />
                <asp:Label ID="KierownikLabel" runat="server" CssClass="line2" Text='<%# Eval("KierownikNI") %>' />
            </td>

            <%--
            <td class="dzial">
                <asp:Label ID="DzialNameLabel" runat="server" Text='<%# Eval("Dzial") %>' /><br />
                <asp:Label ID="StanowiskoLabel" CssClass="line2" runat="server" Text='<%# Eval("Stanowisko") %>' ToolTip='<%# Eval("Stanowisko") %>' />
            </td>
            --%>
            
            <td class="status">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>

            <td class="check">
                <asp:CheckBox ID="cbAdmin" class="check" runat="server" Checked='<%# Eval("Admin") %>' Enabled="false" />
            </td>

            <td id="tdR1" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR1" runat="server" Enabled="false" />
            </td>
            <td id="tdR2" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR2" runat="server" Enabled="false" />
            </td>
            <td id="tdR3" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR3" runat="server" Enabled="false" />
            </td>

            <td class="check">
                <asp:CheckBox ID="cbRaporty" class="check" runat="server" Checked='<%# Eval("Raporty") %>' Enabled="false" />
            </td>

            <td id="tdR4" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR4" runat="server" Enabled="false" />
            </td>
            <td id="tdR5" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR5" runat="server" Enabled="false" />
            </td>
            <td id="tdR6" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR6" runat="server" Enabled="false" />
            </td>
            <td id="tdR7" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR7" runat="server" Enabled="false" />
            </td>
            <td id="tdR8" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR8" runat="server" Enabled="false" />
            </td>
            <td id="tdR9" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR9" runat="server" Enabled="false" />
            </td>
            <td id="tdR10" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR10" runat="server" Enabled="false" />
            </td>
            <td id="tdR11" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR11" runat="server" Enabled="false" />
            </td>
            <td id="tdR12" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR12" runat="server" Enabled="false" />
            </td>
            <td id="tdR13" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR13" runat="server" Enabled="false" />
            </td>
            <td id="tdR14" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR14" runat="server" Enabled="false" />
            </td>
            <td id="tdR15" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR15" runat="server" Enabled="false" />
            </td>
            <td id="tdR16" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR16" runat="server" Enabled="false" />
            </td>
            <td id="tdR17" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR17" runat="server" Enabled="false" />
            </td>
            <td id="tdR18" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR18" runat="server" Enabled="false" />
            </td>
            <td id="tdR19" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR19" runat="server" Enabled="false" />
            </td>
            <td id="tdR20" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR20" runat="server" Enabled="false" />
            </td>
            <td id="tdR21" runat="server" class="check" visible="false">
                <asp:CheckBox ID="cbR21" runat="server" Enabled="false"  />
            </td>

            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" /><br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>            
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="sit" id="trLine" runat="server">
            <td class="nazwisko_nrew">
                <asp:LinkButton ID="NazwiskoLinkButton" runat="server" Text='<%# Eval("NazwiskoImie") %>' CommandName="Select" CommandArgument='<%# Eval("RcpId") + "|" + Eval("StrefaId") %>' ></asp:LinkButton><br />
                <asp:Label ID="KadryIdLabel" runat="server" CssClass="line2" Text='<%# Eval("KadryId") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="IsKierCheckBox" class="check" runat="server" Checked='<%# Eval("Kierownik") %>' Enabled="false" />
            </td>
            <td class="right">
                <br />
                <asp:Label ID="RcpLabel" runat="server" CssClass="line2" Text='<%# Eval("RcpId") %>' />
            </td>

            <%--
            <td class="col">
                <asp:Label ID="StrefaLabel" runat="server" Text='<%# Eval("Strefa") %>' /><br />
                <asp:Label ID="AlgLabel" runat="server" CssClass="line2" Text='<%# Eval("Algorytm") %>' />
            </td>
            --%>
            
            <td class="col">
                <asp:Label ID="LiniaLabel" runat="server" Text='<%# Eval("LiniaNazwa") %>' /><br />
                <asp:Label ID="KierownikLabel" runat="server" CssClass="line2" Text='<%# Eval("KierownikNI") %>' />
            </td>

            <%--    
            <td class="col">
                <asp:Label ID="DzialNameLabel" runat="server" Text='<%# Eval("Dzial") %>' /><br />
                <asp:Label ID="StanowiskoLabel" CssClass="line2" runat="server" Text='<%# Eval("Stanowisko") %>' />
            </td>
            --%>
            
            <td class="col">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>

            <td class="check">
                <asp:CheckBox ID="cbAdmin" class="check" runat="server" Checked='<%# Eval("Admin") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="cbRaporty" class="check" runat="server" Checked='<%# Eval("Raporty") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Eval("Mailing") %>' Enabled="false" />
            </td>

            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>            
        </tr>
    </SelectedItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td colspan="<%= GetEditColSpan() %>" class="col1">
                <table class="edit">
                    <tr>
                        <td class="coled1">
                            <div class="title">
                                <asp:Label ID="Label2" runat="server" Text="Dane podstawowe" />
                            </div>

                            <span class="label">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                            <span class="label">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" Text='<%# Bind("Imie") %>' /><br />
                            <span class="label">Kadry Id:</span>
                            <asp:TextBox ID="KadryIdTextBox" runat="server" Text='<%# Bind("KadryId") %>' /><br />
                            <span class="label">RCP Id:</span>
                            <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Bind("RcpId") %>' /><br />
                            <span class="label">Login:</span>
                            <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Login") %>' /><br />
                            <span class="label">E-mail:</span>
                            <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' /><br />
                            <span class="label">Mailing:</span>
                            <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Bind("Mailing") %>' /><br />

                            <span class="label">Kierownik:</span>
                            <asp:CheckBox ID="KierownikCheckBox" class="check" runat="server" Checked='<%# Bind("Kierownik") %>' /><br />
                            <%--
                            <span class="label">Admininistrator:</span>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' /><br />
                            <span class="label">Raporty:</span>
                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' /><br />
                            --%>
                            <span class="label">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" ></asp:DropDownList>
                        </td>
                        <td class="coled2">
                            <div class="title">
                                <asp:Label ID="Label1" runat="server" Text="Parametry" />
                            </div>

                            <span class="label">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownik" runat="server"
                                DataSourceID="SqlDataSource6"
                                DataTextField="NazwaK"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Commodity:</span>
                            <asp:DropDownList ID="ddlLinia" runat="server" 
                                DataSourceID="SqlDataSource8"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Dział:</span>
                            <asp:DropDownList ID="ddlDzial" runat="server" 
                                DataSourceID="SqlDataSource4"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server"
                                DataSourceID="SqlDataSource5"
                                DataTextField="NazwaDS"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            
                            <br />
                                            
                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server" 
                                DataSourceID="SqlDataSource2"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            <span class="label">Algorytm RCP:</span>
                            <asp:DropDownList ID="ddlAlgorytm" runat="server" 
                                DataSourceID="SqlDataSource3"
                                DataTextField="Nazwa"
                                DataValueField="Kod">
                            </asp:DropDownList><br />

                            <span class="label">Split:</span>
                            <asp:DropDownList ID="ddlSplit" runat="server" 
                                DataSourceID="SqlDataSource7"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <asp:HiddenField ID="hidRights" runat="server" Value='<%# Bind("Rights") %>'/>

                            <br />
                            <span class="label">&nbsp</span>
                            <asp:Button ID="btKwitekPassReset" runat="server" class="button" Text="Resetuj hasło do kwitka płacowego" CommandName="passreset" CommandArgument='<%# Eval("Id") %>' />
                        </td>
                        <td class="coled3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" />
                            </div>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' Text="<span>A</span> - Administrator" />
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           

                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' Text="<span>R</span> - Raporty" />
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                        </td>                                                        
                    </tr>
                </table>
            </td>

            <td class="control" >
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" /><br /><br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td colspan="<%= GetEditColSpan() %>" class="col1">
                <table class="edit">
                    <tr>
                        <td class="coled1">
                            <div class="title">
                                <asp:Label ID="Label2" runat="server" Text="Dane podstawowe" />
                            </div>

                            <span class="label">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                            <span class="label">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" Text='<%# Bind("Imie") %>' /><br />
                            <span class="label">Kadry Id:</span>
                            <asp:TextBox ID="KadryIdTextBox" runat="server" Text='<%# Bind("KadryId") %>' /><br />
                            <span class="label">RCP Id:</span>
                            <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Bind("RcpId") %>' /><br />
                            <span class="label">Login:</span>
                            <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Login") %>' /><br />
                            <span class="label">E-mail:</span>
                            <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' /><br />
                            <span class="label">Mailing:</span>
                            <asp:CheckBox ID="cbMailing" class="check" runat="server" Checked='<%# Bind("Mailing") %>' /><br />

                            <span class="label">Kierownik:</span>
                            <asp:CheckBox ID="KierownikCheckBox" class="check" runat="server" Checked='<%# Bind("Kierownik") %>' /><br />
                            <%--
                            <span class="label">Admininistrator:</span>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' /><br />
                            <span class="label">Raporty:</span>
                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' /><br />
                            --%>
                            <span class="label">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" ></asp:DropDownList>
                        </td>
                        <td class="coled2">
                            <div class="title">
                                <asp:Label ID="Label1" runat="server" Text="Parametry" />
                            </div>

                            <span class="label">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownik" runat="server"
                                DataSourceID="SqlDataSource6"
                                DataTextField="NazwaK"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Commodity:</span>
                            <asp:DropDownList ID="ddlLinia" runat="server" 
                                DataSourceID="SqlDataSource8"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Dział:</span>
                            <asp:DropDownList ID="ddlDzial" runat="server" 
                                DataSourceID="SqlDataSource4"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server"
                                DataSourceID="SqlDataSource5"
                                DataTextField="NazwaDS"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            
                            <br />
                                            
                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server" 
                                DataSourceID="SqlDataSource2"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            <span class="label">Algorytm RCP:</span>
                            <asp:DropDownList ID="ddlAlgorytm" runat="server" 
                                DataSourceID="SqlDataSource3"
                                DataTextField="Nazwa"
                                DataValueField="Kod">
                            </asp:DropDownList><br />

                            <span class="label">Split:</span>
                            <asp:DropDownList ID="ddlSplit" runat="server" 
                                DataSourceID="SqlDataSource7"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <asp:HiddenField ID="hidRights" runat="server" Value='<%# Bind("Rights") %>'/>

                        </td>
                        <td class="coled3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" />
                            </div>
                            <asp:CheckBox ID="AdminCheckBox" class="check" runat="server" Checked='<%# Bind("Admin") %>' Text="<span>A</span> - Administrator" />
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           

                            <asp:CheckBox ID="RaportyCheckBox" class="check" runat="server" Checked='<%# Bind("Raporty") %>' Text="<span>R</span> - Raporty" />
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                        </td>                                                        
                    </tr>
                </table>
            </td>


            <%--
            <td colspan="<%= GetEditColSpan() %>" class="col1">
                <table class="edit">
                    <tr>
                        <td class="coled1">
                            <span class="label">Nazwisko:</span>
                            <asp:TextBox ID="NazwiskoTextBox" runat="server" Text='<%# Bind("Nazwisko") %>' /><br />
                            <span class="label">Imię:</span>
                            <asp:TextBox ID="ImieTextBox" runat="server" Text='<%# Bind("Imie") %>' /><br />

                            <span class="label">Kadry Id:</span>
                            <asp:TextBox ID="KadryIdTextBox" runat="server" Text='<%# Bind("KadryId") %>' /><br />
                            <span class="label">RCP Id:</span>
                            <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Bind("RcpId") %>' /><br />
                            <span class="label">Login:</span>
                            <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Login") %>' /><br />
                            <span class="label">E-mail:</span>
                            <asp:TextBox ID="EmailTextBox" runat="server" Text='<%# Bind("Email") %>' /><br />
                            <span class="label">Mailing:</span>
                            <asp:CheckBox ID="cbMailing" runat="server" class="check" Checked='<%# Bind("Mailing") %>' /><br />

                            <span class="label">Kierownik:</span>
                            <asp:CheckBox ID="KierownikCheckBox" runat="server" class="check" Checked='<%# Bind("Kierownik") %>' /><br />
                            <span class="label">Admininistrator:</span>
                            <asp:CheckBox ID="AdminCheckBox" runat="server" class="check" Checked='<%# Bind("Admin") %>' /><br />
                            <span class="label">Raporty:</span>
                            <asp:CheckBox ID="RaportyCheckBox" runat="server" class="check" Checked='<%# Bind("Raporty") %>' /><br />
                            <span class="label">Status:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" ></asp:DropDownList>
                        </td>
                        <td class="coled2">
                            <span class="label">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownik" runat="server"
                                DataSourceID="SqlDataSource6"
                                DataTextField="NazwaK"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Commodity:</span>
                            <asp:DropDownList ID="ddlLinia" runat="server" 
                                DataSourceID="SqlDataSource8"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Dział:</span>
                            <asp:DropDownList ID="ddlDzial" runat="server" 
                                DataSourceID="SqlDataSource4"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />

                            <span class="label">Stanowisko:</span>
                            <asp:DropDownList ID="ddlStanowisko" runat="server"
                                DataSourceID="SqlDataSource5"
                                DataTextField="NazwaDS"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            <br />
                            <span class="label">Strefa RCP:</span>
                            <asp:DropDownList ID="ddlStrefa" runat="server" 
                                DataSourceID="SqlDataSource2"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                            <span class="label">Algorytm RCP:</span>
                            <asp:DropDownList ID="ddlAlgorytm" runat="server" 
                                DataSourceID="SqlDataSource3"
                                DataTextField="Nazwa"
                                DataValueField="Kod">
                            </asp:DropDownList><br />
                            
                            <span class="label">Split:</span>
                            <asp:DropDownList ID="ddlSplit" runat="server" 
                                DataSourceID="SqlDataSource7"
                                DataTextField="Nazwa"
                                DataValueField="Id">
                            </asp:DropDownList><br />
                        </td>
                        <td class="col3">
                            <div class="title">
                                <asp:Label ID="lbRightsTitle" runat="server" Text="Uprawnienia" /><br />
                            </div>
                            <asp:CheckBox ID="cbR1" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR2" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR3" CssClass="check" runat="server" Visible="false" />                           
                            <asp:CheckBox ID="cbR4" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR5" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR6" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR7" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR8" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR9" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR10" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR11" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR12" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR13" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR14" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR15" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR16" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR17" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR18" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR19" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR20" CssClass="check" runat="server" Visible="false" />
                            <asp:CheckBox ID="cbR21" CssClass="check" runat="server" Visible="false" /> 
                        </td>                                                        
                    </tr>
                </table>
            </td>
            --%>
            <td class="control" >
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" /><br />
                <asp:Button ID="CancelInsertButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" class="ListView3_edt" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="Table2" class="ListView1 hoverline cntPracownicy2" runat="server">
            <tr id="Tr1" runat="server">
                <td id="Td1" colspan="2" runat="server">
                    <table ID="itemPlaceholderContainer" class="tbPracownicy2" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" rowspan="2" runat="server" class="nazwisko_nrew">
                                <asp:LinkButton ID="LinkButton31" runat="server" CommandName="Sort" CommandArgument="NazwiskoImie" Text="Pracownik" ToolTip="Sortuj" />
                                <div class="line2">
                                    <asp:LinkButton ID="LinkButton33" CssClass="left" runat="server" CommandName="Sort" CommandArgument="KadryId" Text="Nr ew." ToolTip="Sortuj"/>
                                    <asp:LinkButton ID="LinkButton34" runat="server" CommandName="Sort" CommandArgument="RcpId" Text="RCP ID" ToolTip="Sortuj"/>
                                </div>
                            </th>
                            <th id="Th2" rowspan="2" runat="server"><asp:LinkButton ID="LinkButton32" runat="server" CommandName="Sort" CommandArgument="Kierownik"    Text="Kier" ToolTip="Sortuj" /></th>
                            <th id="Th113" rowspan="2" runat="server" class="check"><asp:LinkButton ID="LinkButton43" runat="server" CommandName="Sort" CommandArgument="Mailing" Text="@" ToolTip="Wysyłanie powiadomień - Sortuj"/></th>
                                
                            <%--    
                            <th id="Th5" rowspan="2" runat="server"><asp:LinkButton ID="LinkButton35" runat="server" CommandName="Sort" CommandArgument="Strefa"       Text="Strefa" ToolTip="Sortuj"/> /
                                                                    <asp:LinkButton ID="LinkButton36" runat="server" CommandName="Sort" CommandArgument="Algorytm"     Text="Algorytm" ToolTip="Sortuj"/></th>
                            --%>
                            <th id="Th9" rowspan="2" runat="server"><asp:LinkButton ID="LinkButton44" runat="server" CommandName="Sort" CommandArgument="LiniaNazwa"  Text="Commodity" ToolTip="Sortuj"/> /
                                                                    <asp:LinkButton ID="LinkButton39" runat="server" CommandName="Sort" CommandArgument="KierownikNI"  Text="Kierownik" ToolTip="Sortuj"/></th>
                            <%--    
                            <th id="Th7" rowspan="2" runat="server"><asp:LinkButton ID="LinkButton37" runat="server" CommandName="Sort" CommandArgument="Dzial"        Text="Dział" ToolTip="Sortuj"/> /
                                                                    <asp:LinkButton ID="LinkButton38" runat="server" CommandName="Sort" CommandArgument="Stanowisko"   Text="Stanowisko" ToolTip="Sortuj"/></th>
                            --%>
                            <th id="Th10" rowspan="2" runat="server"><asp:LinkButton ID="LinkButton40" runat="server" CommandName="Sort" CommandArgument="Status"     Text="Status" ToolTip="Sortuj"/></th>
                
                            <%--            
                            --%>
                            
                            
                            <th id="thRights" colspan="21" class="top" runat="server" visible="false">
                                Uprawnienia</th>
                            
                            <th id="Th11" class="control" rowspan="2" runat="server">
                                <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj nowy rekord"/>
                            </th>
                        </tr>
                        <tr class="bottom">
                            <th id="Th111" runat="server" class="check"><asp:LinkButton ID="LinkButton41" runat="server" CommandName="Sort" CommandArgument="Admin" Text="A" ToolTip="Administrator - Sortuj"/></th>

                            <th id="thR1"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton1"  runat="server"></asp:LinkButton></th>
                            <th id="thR2"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton2"  runat="server"></asp:LinkButton></th>
                            <th id="thR3"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton3"  runat="server"></asp:LinkButton></th>

                            <th id="Th112" runat="server" class="check"><asp:LinkButton ID="LinkButton42" runat="server" CommandName="Sort" CommandArgument="Raporty" Text="R" ToolTip="Dostęp do raportów - Sortuj"/></th>
                            <th id="thR4"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton4"  runat="server"></asp:LinkButton></th>
                            <th id="thR5"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton5"  runat="server"></asp:LinkButton></th>
                            <th id="thR6"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton6"  runat="server"></asp:LinkButton></th>
                            <th id="thR7"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton7"  runat="server"></asp:LinkButton></th>
                            <th id="thR8"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton8"  runat="server"></asp:LinkButton></th>
                            <th id="thR9"  runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton9"  runat="server"></asp:LinkButton></th>
                            <th id="thR10" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton10" runat="server"></asp:LinkButton></th>
                            <th id="thR11" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton11" runat="server"></asp:LinkButton></th>
                            <th id="thR12" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton12" runat="server"></asp:LinkButton></th>
                            <th id="thR13" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton13" runat="server"></asp:LinkButton></th>
                            <th id="thR14" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton14" runat="server"></asp:LinkButton></th>
                            <th id="thR15" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton15" runat="server"></asp:LinkButton></th>
                            <th id="thR16" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton16" runat="server"></asp:LinkButton></th>
                            <th id="thR17" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton17" runat="server"></asp:LinkButton></th>
                            <th id="thR18" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton18" runat="server"></asp:LinkButton></th>
                            <th id="thR19" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton19" runat="server"></asp:LinkButton></th>
                            <th id="thR20" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton20" runat="server"></asp:LinkButton></th>
                            <th id="thR21" runat="server" class="check" Visible="false"><asp:LinkButton ID="LinkButton21" runat="server"></asp:LinkButton></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr4" class="pager" runat="server">
                <td id="Td4" class="left" runat="server">
                    <uc1:LetterDataPager ID="LetterDataPager1" TbName="Pracownicy" Letter="LEFT(Nazwisko,1)" runat="server" />
                </td>
                <td class="right">
                    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
            <tr class="pager">
                <td>
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" >
                    </asp:DropDownList>
                    
                    
                    
                    <%--
                    <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true"    
                        OnChange="showAjaxProgress();"
                        OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                        <asp:ListItem Text="15" Value="15" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                    --%>
                </td>
            </tr>
            
            <%--            
            <tr id="Tr3" runat="server">
                <td class="pager" align="left">
                    <uc1:LetterDataPager ID="LetterDataPager1" TbName="Pracownicy" Letter="LEFT(Nazwisko,1)" runat="server" />
                    <div class="spacer8"></div>
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td id="Td2" runat="server" class="pager right" style="">
                </td>
            </tr>
            --%>
            
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select
                        P.Id, 
                        P.Login,
                        P.Imie,
                        P.Nazwisko,
                        RTRIM(P.Nazwisko) + ' ' + RTRIM(P.Imie) as NazwiskoImie, 
                        P.Email,
                        P.Mailing, 
                        P.KadryId,
                        P.RcpId,
                        P.IdDzialu,
                        D.Nazwa as Dzial,
                        P.IdStanowiska,
                        T.Nazwa as Stanowisko,
                        P.IdKierownika,
                        RTRIM(K.Nazwisko) + ' ' + RTRIM(K.Imie) as KierownikNI, 
                        P.Kierownik,
                        ISNULL(P.Admin, 0) as Admin,
                        P.Raporty,
                        P.Status,
                        P.RcpStrefaId,   
                        S.Id as StrefaId, 
                        S.Nazwa as Strefa,
                        P.RcpAlgorytm,
                        KO.Nazwa as Algorytm,
                        P.CCInfo,P.IdLinii,P.GrSplitu,L.Symbol as LiniaSymbol,L.Nazwa as LiniaNazwa,
                        (select top 1 SS.Nazwa from Splity SS where SS.GrSplitu = 
                            case when P.GrSplitu is null then 
                                case when P.Kierownik=1 then L.GrSplituK
                                else L.GrSplituP end
                            else P.GrSplitu end) as Split,
                        P.Rights
                    FROM Pracownicy P 
                    left outer join Pracownicy K on P.IdKierownika = K.Id
                    left outer join Dzialy D on P.IdDzialu = D.Id
                    left outer join Stanowiska T on T.Id = P.IdStanowiska
                    left outer join Strefy S on S.Id = 
                        ISNULL(P.RcpStrefaId, CASE WHEN P.Kierownik=1 THEN D.KierStrefaId ELSE D.PracStrefaId END)
                    left outer join Kody KO on KO.Typ = 'ALG' and KO.Kod = 
                        ISNULL(P.RcpAlgorytm, CASE WHEN P.Kierownik=1 THEN D.KierAlgorytm ELSE D.PracAlgorytm END)
                    left outer join Linie L on L.Id = P.IdLinii"
    UpdateCommand="UPDATE [Pracownicy] SET [Login] = @Login, [Imie] = @Imie, [Nazwisko] = @Nazwisko, [Opis] = @Opis, [Email] = @Email, [Mailing] = @Mailing, [IdDzialu] = @IdDzialu, [IdStanowiska] = @IdStanowiska, [IdKierownika] = @IdKierownika, 
                        [Admin] = @Admin, [Raporty] = @Raporty, [Kierownik] = @Kierownik, [KadryId] = @KadryId, [RcpId] = @RcpId, [Status] = @Status, [RcpStrefaId] = @RcpStrefaId, [RcpAlgorytm] = @RcpAlgorytm, [CCInfo] = @CCInfo, [Rights] = @Rights, [IdLinii] = @IdLinii, [GrSplitu] = @GrSplitu WHERE [Id] = @Id"
    InsertCommand="INSERT INTO [Pracownicy] ([Login], [Imie], [Nazwisko], [Opis], [Email], [Mailing], [IdDzialu], [IdStanowiska], [IdKierownika], [Admin], [Raporty], [Kierownik], [KadryId], [RcpId], [Status], [RcpStrefaId], [RcpAlgorytm], [CCInfo], [Rights], [IdLinii], [GrSplitu]) VALUES 
                        (@Login, @Imie, @Nazwisko, @Opis, @Email, @Mailing, @IdDzialu, @IdStanowiska, @IdKierownika, @Admin, @Raporty, @Kierownik, @KadryId, @RcpId, @Status, @RcpStrefaId, @RcpAlgorytm, @CCInfo, @Rights, @IdLinii, @GrSplitu)" 

    DeleteCommand="DELETE FROM [Pracownicy] WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Login" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Mailing" Type="Boolean" />
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="IdStanowiska" Type="Int32" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="Admin" Type="Boolean" />
        <asp:Parameter Name="Raporty" Type="Boolean" />
        <asp:Parameter Name="Kierownik" Type="Boolean" />
        <asp:Parameter Name="KadryId" Type="String" />
        <asp:Parameter Name="RcpId" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="RcpAlgorytm" Type="Int32" />
        <asp:Parameter Name="CCInfo" Type="String" />
        <asp:Parameter Name="Rights" Type="String" />
        <asp:Parameter Name="IdLinii" Type="Int32" />
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Login" Type="String" />
        <asp:Parameter Name="Imie" Type="String" />
        <asp:Parameter Name="Nazwisko" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Mailing" Type="Boolean" />
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="IdStanowiska" Type="Int32" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="Admin" Type="Boolean" />
        <asp:Parameter Name="Raporty" Type="Boolean" />
        <asp:Parameter Name="Kierownik" Type="Boolean" />
        <asp:Parameter Name="KadryId" Type="String" />
        <asp:Parameter Name="RcpId" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="RcpAlgorytm" Type="Int32" />
        <asp:Parameter Name="CCInfo" Type="String" />
        <asp:Parameter Name="Rights" Type="String" />
        <asp:Parameter Name="IdLinii" Type="Int32" />
        <asp:Parameter Name="GrSplitu" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, [Id], [Nazwa] FROM [Strefy] WHERE ([Aktywna] = 1) 
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as Nazwa                   
                    ORDER BY Aa, [Nazwa]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, [Id], [Nazwa], [Lp], [Kod], [Parametr] FROM [Kody] WHERE ([Typ] = 'ALG') 
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as Nazwa, 0 as Lp, null as Kod, null as Parametr                   
                    ORDER BY Aa, [Lp]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, [Id], [Nazwa] FROM [Dzialy] 
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as Nazwa                   
                    ORDER BY Aa, [Nazwa]">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Aa, S.Id, D.Nazwa + ' - ' + S.Nazwa as NazwaDS FROM Stanowiska S 
                    left outer join Dzialy D on D.Id = S.IdDzialu
                    union 
                    select 0 as Aa, null as Id, 'wybierz ...' as NazwaDS
                    ORDER BY Aa, NazwaDS">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT 1 as Sort, Id, Nazwisko + ' ' + Imie as NazwaK FROM Pracownicy where Kierownik = 1 and Status <> -1
                    union 
                    select 0 as Sort, null as Id, 'wybierz ...' as NazwaK
                    union 
                    select 2 as Sort, '0' as Id, 'Główny poziom struktury' as NazwaK
                    ORDER BY Sort, NazwaK">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource7" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Id, 'wybierz ...' as Nazwa union
                   SELECT 1 as Sort, GrSplitu as Id, Nazwa FROM Splity where ISNULL(DataDo, GETDATE()) &gt;= GETDATE() 
                   ORDER BY Sort, Nazwa">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource8" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Id, 'wybierz ...' as Nazwa union
                   SELECT 1 as Sort, Id, Symbol + ISNULL(' - ' + Nazwa, '') as Nazwa FROM Linie
                   ORDER BY Sort, Nazwa">
</asp:SqlDataSource>
