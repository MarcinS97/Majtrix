<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzypisaniaParametry.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Przypisania.cntPrzypisaniaParametry" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>
<%@ Register src="cntPrzypisaniaMini.ascx" tagname="cntPrzypisaniaMini" tagprefix="uc1" %>
<%@ Register src="cntSplityWsp.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>

<asp:HiddenField ID="hidPrzId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />
<asp:HiddenField ID="hidSelPracId" runat="server" />
<asp:HiddenField ID="hidSelKierId" runat="server" />
<asp:ListView ID="lvPrzypisaniaParametry" runat="server" 
    DataSourceID="SqlDataSource1" DataKeyNames="Id" 
    oniteminserting="lvPrzypisaniaParametry_ItemInserting" 
    oniteminserted="lvPrzypisaniaParametry_ItemInserted" 
    onitemdatabound="lvPrzypisaniaParametry_ItemDataBound" 
    onitemcreated="lvPrzypisaniaParametry_ItemCreated" 
    onitemcommand="lvPrzypisaniaParametry_ItemCommand" 
    ondatabound="lvPrzypisaniaParametry_DataBound">
    <ItemTemplate>
        <table class="table0">
            <tr id="tr1" runat="server" class="dane">
                <td>
                    <span class="label">Pracownik:</span>
                    <asp:Label ID="lbPracownik" runat="server" CssClass="pracownik" Text='<%# Eval("Pracownik") %>'/><br />                    
                    <span class="label">Obecny przełożony:</span>
                    <asp:Label ID="lbKierOd" runat="server" CssClass="kierownik" Text='<%# Eval("KierownikNI") %>'/><br />                    

                    <span class="label">Okres od:</span>
                    <asp:Label ID="lbOd" runat="server" Text='<%# Eval("Od", "{0:d}") %>'/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                   
                    <span class="label1">do:</span>
                    <asp:Label ID="lbDo" runat="server" Text='<%# BezTerminu(Eval("DoM", "{0:d}")) %>' />
                    <asp:Label ID="lbMonit" CssClass="label3" runat="server" Text="(monit)" Visible='<%# Eval("Monit") %>' /><br />                    


                    <div id="paStrefa" runat="server" visible="false">    
                        <span class="label">Strefa RCP:</span>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("RcpStrefa") %>' /><br />
                    </div>                    
                    <div id="paCC" runat="server">
                        <span class="label">CC:</span>
                        <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="0" runat="server" IdPrzypisania='<%# Eval("Id") %>' /><br />
                    </div>
                    <%--
                    <span class="label2">CC:</span>
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("CCNazwa") %>' />
                    --%>

                    <div id="paCommodity" runat="server">
                        <asp:Label ID="lb6" runat="server" CssClass="label" Text="Commodity:"></asp:Label>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Commodity") %>' /><br />
                    </div>
                    <div id="paArea" runat="server">
                        <span class="label">Area:</span>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("Area") %>' /><br />
                        <span class="label">Position:</span>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("Position") %>' /><br />
                    </div>
                    
                    <%--
                    <span class="label2">Przesunięcia:</span>
                    <uc1:cntPrzypisaniaMini ID="cntPrzypisaniaMini1" runat="server" />
                    --%>
                </td>
            </tr>
            <tr id="trButtons" runat="server" visible="false">
                <td class="buttons">
                    <asp:Button ID="EditButton" runat="server" CssClass="button75" CommandName="Edit" Text="Edytuj" visible="true"/>
                    <asp:Button ID="btMoveSettings" runat="server" CssClass="button" CommandName="MoveSettings" Text="Przenieś ustawienia" Visible="false"/>
                </td>
            </tr>
        </table>
    </ItemTemplate>
    <EditItemTemplate>
        <table class="table0">
            <tr id="tr7" runat="server">
                <td>
                    <asp:Label ID="lb1" runat="server" CssClass="label" Text="Pracownik:"/>                    
                    <asp:Label ID="lbPracownik" runat="server" CssClass="pracownik" Text='<%# Eval("Pracownik") %>'/><br />                    
                    <asp:Label ID="lb2" runat="server" CssClass="label" Text="Przełożony:"/>                    
                    <asp:Label ID="lbKierownik" runat="server" CssClass="kierownik" Text='<%# Eval("KierownikNI") %>'/><br />                    

                    <span class="label">Okres od:</span>
                    <asp:Label ID="lbOd" runat="server" Text='<%# Eval("Od", "{0:d}") %>'/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                   
                    <span class="label1">do:</span>
                    <asp:Label ID="lbDo" runat="server" Text='<%# BezTerminu(Eval("DoM", "{0:d}")) %>' />
                    <asp:Label ID="lbMonit" CssClass="label3" runat="server" Text="(monit)" Visible='<%# Eval("Monit") %>' /><br />                    
                    <div id="paStrefa" runat="server" visible="false">    
                        <asp:Label ID="lb9" runat="server" CssClass="label" Text="Strefa RCP:"/>                    
                        <asp:DropDownList ID="ddlStrefa" runat="server" DataSourceID="SqlDataSource6" DataValueField="Id" DataTextField="Nazwa" /><br />
                    </div>
                    <div id="paCC" runat="server">
                        <asp:Label ID="lb5" runat="server" CssClass="label" Text="CC:"/>                    
                        <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="1" runat="server" /><br />
                    </div>                    
                    <div id="paCommodity" runat="server">
                        <asp:Label ID="lb6" runat="server" CssClass="label" Text="Commodity:"/>                    
                        <asp:DropDownList ID="ddlCommodity" runat="server" DataSourceID="SqlDataSource3" DataValueField="Id" DataTextField="Nazwa" /><br />
                    </div>
                    <div id="paArea" runat="server">
                        <asp:Label ID="lb7" runat="server" CssClass="label" Text="Area:"/>                    
                        <asp:DropDownList ID="ddlArea" runat="server" DataSourceID="SqlDataSource4" DataValueField="Id" DataTextField="Nazwa" /><br />

                        <asp:Label ID="lb8" runat="server" CssClass="label" Text="Position:"/>                    
                        <asp:DropDownList ID="ddlPosition" runat="server" DataSourceID="SqlDataSource5" DataValueField="Id" DataTextField="Nazwa" /><br />
                    </div>
                    
                    <span class="label2">Uzasadnienie:</span>
                    <asp:TextBox ID="tbUwagi" runat="server" TextMode="MultiLine" Rows="3" MaxLength="200" Text='<%# Bind("UwagiRq") %>' />
               
               
               
               
                    <%--               
                    <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Bind("Status") %>' />
                    <asp:HiddenField ID="hidTyp" runat="server" Value='<%# Bind("Typ") %>' />                
                    <asp:HiddenField ID="hidPracId" runat="server" Value='<%# Eval("IdPracownika") %>' />       
                    <asp:HiddenField ID="hidKierAcc" runat="server" Value='<%# Bind("IdKierownikaAcc") %>' />       
                    <asp:HiddenField ID="hidDataAcc" runat="server" Value='<%# Bind("DataAcc") %>' />       

                    <span class="label">Od:</span>
                    <uc2:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' Visible="false"/>
                    <asp:Label ID="lbOd" runat="server" CssClass="value" Text='<%# Eval("Od", "{0:d}") %>' />
                    <span class="label1">Do:</span>
                    <uc2:DateEdit ID="deTo" runat="server" Date='<%# Bind("Do") %>' Visible="false"/>
                    <asp:Label ID="lbDo" runat="server" CssClass="value" Text='<%# BezTerminu(Eval("Do2")) %>' />
                    
                    <asp:Label ID="lbMonit" runat="server" CssClass="label1" Text="Monituj:" Visible="false" />
                    <uc2:DateEdit ID="deMonit" runat="server" Date='<%# Bind("DoMonit") %>' Visible="false" /><br />

                    <span class="label">Strefa RCP:</span>
                    <asp:Label ID="lbStrefa" runat="server" CssClass="value" Text='<%# Eval("RcpStrefa2")%>' Visible="false" />
                    <asp:DropDownList ID="ddlStrefa" runat="server" DataSourceID="SqlDataSource6" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("RcpStrefaId") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="ddlStrefa" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator5" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                    <br />

                    <span class="label">CC:</span>                
                    <uc6:cntSplityWsp ID="cntSplityWsp2" Mode="1" runat="server" IdPrzypisania='<%# Eval("Id") %>' />
                    <br />
                    
                    <span class="label">Commodity:</span>
                    <asp:Label ID="lbCommodity" runat="server" CssClass="value" Text='<%# Eval("Commodity2")%>' Visible="false" />
                    <asp:DropDownList ID="ddlCommodity" runat="server" DataSourceID="SqlDataSource3" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("IdCommodity") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="ddlCommodity" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                    <br />
                    
                    <span class="label">Area:</span>
                    <asp:Label ID="lbArea" runat="server" CssClass="value" Text='<%# Eval("Area2")%>' Visible="false" />
                    <asp:DropDownList ID="ddlArea" runat="server" DataSourceID="SqlDataSource4" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("IdArea") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="ddlArea" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                    <br />
                    
                    <span class="label">Position:</span>
                    <asp:Label ID="lbPosition" runat="server" CssClass="value" Text='<%# Eval("Position2")%>' Visible="false" />
                    <asp:DropDownList ID="ddlPosition" runat="server" DataSourceID="SqlDataSource5" DataValueField="Id" DataTextField="Nazwa" SelectedValue='<%# Bind("IdPosition") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="ddlPosition" Enabled="false" Visible="false" ValidationGroup="evg" ErrorMessage="Pole wymagane" ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>                            
                    <br />
                    
                    <span class="label2">Uzasadnienie wniosku:</span>
                    <asp:TextBox ID="tbUwagi" runat="server" CssClass="textbox uwagi" TextMode="MultiLine" Rows="3" MaxLength="200" Text='<%# Bind("UwagiRq") %>' />
                    <asp:Label ID="lbUwagi" runat="server" CssClass="ramka uwagi" Text='<%# Eval("UwagiRq") %>' Visible="false"/>
                    
                    <asp:Label ID="lbUwagiAccLabel" runat="server" CssClass="label2" Text="Uzasadnienie odrzucenia / Uwagi:" />
                    <asp:TextBox ID="tbUwagiAcc" runat="server" CssClass="textbox uwagi" TextMode="MultiLine" Rows="3" MaxLength="200" Text='<%# Bind("UwagiAcc") %>' />
                    <asp:Label ID="lbUwagiAcc" runat="server" CssClass="ramka uwagi" Text='<%# Eval("UwagiAcc") %>' Visible="false"/>                
                    --%>
                    
                
                    <%--
                    <span class="label">Pracownik:</span>
                    <asp:Label ID="lbPracownik" runat="server" CssClass="value" /><br />                    
                    <span class="label">Od kierownika:</span>
                    <asp:Label ID="lbOdKier" runat="server" /><br />                    
                    <span class="label">Do kierownika:</span>
                    <asp:Label ID="lbDoKier" runat="server" CssClass="value" /><br />                    
                    
                    <span class="label">Od:</span>
                    <uc2:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>'/>
                    <asp:Label ID="lbOd" runat="server" CssClass="value" Text='<%# Eval("Od", "{0:d}") %>' />
                    <span class="label1">Do:</span>
                    <uc2:DateEdit ID="deTo" runat="server" Date='<%# Bind("Do") %>' Visible="false"/>
                    <uc2:DateEdit ID="deToMonit" runat="server" Date='<%# Bind("DoMonit") %>' Visible="false"/>
                    <asp:Label ID="lbDo" runat="server" CssClass="value" Text='<%# Eval("Do", "{0:d}") %>' /><br />
                    <span class="label">CC:</span>
                    <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="SqlDataSource2" DataValueField="Id" DataTextField="Nazwa" 
                        SelectedValue='<%# Bind("IdCC") %>'>
                    </asp:DropDownList><br />
                    <span class="label">Commodity:</span>
                    <asp:DropDownList ID="ddlCommodity" runat="server" DataSourceID="SqlDataSource3" DataValueField="Id" DataTextField="Nazwa"
                        SelectedValue='<%# Bind("IdCommodity") %>'>
                    </asp:DropDownList><br />
                    <span class="label">Area:</span>
                    <asp:DropDownList ID="ddlArea" runat="server" DataSourceID="SqlDataSource4" DataValueField="Id" DataTextField="Nazwa"
                        SelectedValue='<%# Bind("IdArea") %>'>
                    </asp:DropDownList><br />
                    <span class="label">Position:</span>
                    <asp:DropDownList ID="ddlPosition" runat="server" DataSourceID="SqlDataSource5" DataValueField="Id" DataTextField="Nazwa"
                        SelectedValue='<%# Bind("IdPosition") %>'>
                    </asp:DropDownList><br />
                    <span class="label2">Uzasadnienie wniosku:</span>
                    <asp:TextBox ID="tbUwagi" runat="server" TextMode="MultiLine" Rows="3" MaxLength="200" Text='<%# Bind("UwagiRq") %>' ReadOnly="true" />
                    <span class="label2">Uzasadnienie :</span>
                    <asp:TextBox ID="tbUwagiAcc" runat="server" TextMode="MultiLine" Rows="3" MaxLength="200" Text='<%# Bind("UwagiAcc") %>' />
                    --%>
                </td>
            </tr>
            <tr id="trButtons" runat="server">
                <td class="buttons">
                    <asp:Button ID="btUpdate" runat="server" CssClass="button" CommandName="Update" CommandArgument="0" Text="Zapisz" />
                    <asp:Button ID="btCancel" runat="server" CssClass="button" CommandName="Cancel" Text="Anuluj" />
                    <asp:Button ID="btAccept" runat="server" CssClass="button" CommandName="Update" CommandArgument="1" Text="Zaakceptuj" Visible="false" />
                    <asp:Button ID="btReject" runat="server" CssClass="button" CommandName="Update" CommandArgument="2" Text="Odrzuć" Visible="false" />
                </td>
            </tr>
        </table>        
    </EditItemTemplate>
    <InsertItemTemplate>
        <table class="table0">
            <tr id="tr7" runat="server" class="dane">
                <td>
                    <%--
                    <span class="label">Od kierownika:</span>
                    <asp:Label ID="lbKierOd" runat="server" /><br />                    
                    --%>
                    <asp:Label ID="lb1" runat="server" CssClass="label" Text="Pracownik:"/>                    
                    <asp:Label ID="lbPracownikHint" runat="server" CssClass="hint" Text="Wybierz pracownika..."/>
                    <asp:Label ID="lbPracownik" runat="server" CssClass="pracownik" Visible="false"/><br />                    

                    <asp:Label ID="lb2" runat="server" CssClass="label" Text="Nowy przełożony:"/>                    
                    <asp:Label ID="lbKierDoHint" runat="server" CssClass="hint" Text="Wybierz kierownika..."/>
                    <asp:Label ID="lbKierDo" runat="server" CssClass="kierownik" Visible="false"/><br />                    
                
                    <asp:Label ID="lb3" runat="server" CssClass="label" Text="Okres od:"/>                    
                    <uc2:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>'/>

                    <asp:Label ID="lb4" runat="server" CssClass="label1" Text="do:"/>                    
                    <uc2:DateEdit ID="deTo" runat="server" Date='<%# Bind("DoMonit") %>'/><br />

                    <div id="paBack" runat="server" visible="false" class="paBack">
                        <asp:Label ID="Label5" runat="server" Text="Automatyczny powrót po zakończeniu oddelegowania"/>                    
                        <asp:CheckBox ID="cbAutoReturn" runat="server" Checked='<%# Bind("AutoPowrot") %>'/>
                    </div>

                    <div id="paStrefa" runat="server" visible="false">    
                        <asp:Label ID="lb9" runat="server" CssClass="label" Text="Strefa RCP:"/>                    
                        <asp:DropDownList ID="ddlStrefa" runat="server" DataSourceID="SqlDataSource6" DataValueField="Id" DataTextField="Nazwa" /><br />
                    </div>
                    <div id="paCC" runat="server">
                        <asp:Label ID="lb5" runat="server" CssClass="label" Text="CC:"/>                    
                        <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="1" runat="server" /><br />
                    </div>
                    

                    <%--
                    <asp:Label ID="lb5" runat="server" CssClass="label2" Text="CC:"/>                    
                    <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="SqlDataSource2" DataValueField="Id" DataTextField="Nazwa">
                    </asp:DropDownList><br />
                    --%>
                    
                    
                    <div id="paCommodity" runat="server">
                        <asp:Label ID="lb6" runat="server" CssClass="label" Text="Commodity:"/>                    
                        <asp:DropDownList ID="ddlCommodity" runat="server" DataSourceID="SqlDataSource3" DataValueField="Id" DataTextField="Nazwa" /><br />
                    </div>
                    <div id="paArea" runat="server">
                        <asp:Label ID="lb7" runat="server" CssClass="label" Text="Area:"/>                    
                        <asp:DropDownList ID="ddlArea" runat="server" DataSourceID="SqlDataSource4" DataValueField="Id" DataTextField="Nazwa" /><br />

                        <asp:Label ID="lb8" runat="server" CssClass="label" Text="Position:"/>                    
                        <asp:DropDownList ID="ddlPosition" runat="server" DataSourceID="SqlDataSource5" DataValueField="Id" DataTextField="Nazwa" /><br />
                    </div>
                    
                    <span class="label2">Uzasadnienie:</span>
                    <asp:TextBox ID="tbUwagi" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" MaxLength="200" Text='<%# Bind("UwagiRq") %>' />
                </td>
            </tr>
            <tr id="trButtons" runat="server" >
                <td class="buttons">
                    <asp:Button ID="btCzysc" runat="server" CssClass="button75 left" CommandName="Cancel1" Text="Czyść" />
                    <asp:Button ID="btMoveSettings" runat="server" CssClass="button left" CommandName="MoveSettings" Text="Przenieś ustawienia" Enabled="false" />
                    <asp:Button ID="btPrzesun" runat="server" CssClass="button" CommandName="Insert" Text="Przesuń pracownika" Enabled="false" />
                    <asp:Button ID="btZmien" runat="server" CssClass="button" CommandName="Insert" Text="Zmień ustawienia" Enabled="false" Visible="false"/>
                    <asp:Button ID="btWnioskuj" runat="server" CssClass="button" CommandName="Insert" Text="Wyślij wniosek" Enabled="false" Visible="false"/>
                </td>
            </tr>
        </table>
    </InsertItemTemplate>
    <EmptyDataTemplate>
        <div class="cntPrzypisaniaParametry">
            <asp:Label ID="lbNoData" CssClass="hint" runat="server" Text="Proszę wskazać pracownika..." ></asp:Label>
            <asp:Label ID="lbNoDataPracLabel" runat="server" CssClass="label" Text="Pracownik:" Visible="false"/>
            <asp:Label ID="lbNoDataPracName" runat="server" CssClass="pracownik"  Visible="false"/>
        </div>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div ID="itemPlaceholderContainer" runat="server" class="cntPrzypisaniaParametry">
            <div ID="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</asp:ListView>

<div id="paPrzypisaniaMini" runat="server" class="paPrzypisaniaMini" visible="false">
    <span class="label2">Przesunięcia:</span>
    <uc1:cntPrzypisaniaMini ID="cntPrzypisaniaMini1" OnSelect="cntPrzypisaniaMini_Select" runat="server" />
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    oninserted="SqlDataSource1_Inserted" 
    oninserting="SqlDataSource1_Inserting"
    UpdateCommand="UPDATE [Przypisania] SET [IdPracownika] = @IdPracownika, [Od] = @Od, [Do] = @Do, [DoMonit] = @DoMonit, [IdKierownika] = @IdKierownika, [IdCC] = @IdCC, [IdCommodity] = @IdCommodity, [IdArea] = @IdArea, [IdPosition] = @IdPosition, [IdKierownikaRq] = @IdKierownikaRq, [IdKierownikaRqZast] = @IdKierownikaRqZast, [DataRq] = @DataRq, [UwagiRq] = @UwagiRq, [IdKierownikaAcc] = @IdKierownikaAcc, [DataAcc] = @DataAcc, [UwagiAcc] = @UwagiAcc, [Status] = @Status, [Typ] = @Typ, RcpStrefaId = @RcpStrefaId WHERE [Id] = @Id"
    InsertCommand="INSERT INTO [Przypisania] ([IdPracownika], [Od], [Do], [DoMonit], [IdKierownika], [IdCC], [IdCommodity], [IdArea], [IdPosition], [IdKierownikaRq], [IdKierownikaRqZast], [DataRq], [UwagiRq], [IdKierownikaAcc], [DataAcc], [UwagiAcc], [Status], [Typ], RcpStrefaId, AutoPowrot) VALUES (@IdPracownika, @Od, @Do, @DoMonit, @IdKierownika, @IdCC, @IdCommodity, @IdArea, @IdPosition, @IdKierownikaRq, @IdKierownikaRqZast, @DataRq, @UwagiRq, @IdKierownikaAcc, @DataAcc, @UwagiAcc, @Status, @Typ, @RcpStrefaId, @AutoPowrot)" 
    DeleteCommand="DELETE FROM [Przypisania] WHERE [Id] = @Id" 
    SelectCommand="
select R.*, 
	ISNULL(R.Do, R.DoMonit) as DoM,
	convert(bit, case when R.Do is null and R.DoMonit is not null then 1 else 0 end) as Monit,
    C.Commodity, A.Area, PO.Position, CC.cc, CC.cc + ISNULL(' - ' + CC.Nazwa, '') as CCNazwa,
    P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') as Pracownik,   
    P.DataZatr, P.Kierownik,
    S.Nazwa as RcpStrefa, S.Id as RcpStrefaId,
    case when R.IdKierownika = 0 then 
        'Główny poziom struktury' 
    else K.Nazwisko + ' ' + K.Imie + ISNULL(' (' + K.KadryId + ')', '') end as KierownikNI
from Przypisania R 
left outer join Pracownicy P on P.Id = R.IdPracownika
left outer join Strefy S on S.Id = R.RcpStrefaId
left outer join Pracownicy K on K.Id = R.IdKierownika
left outer join Commodity C on C.Id = R.IdCommodity
left outer join Area A on A.Id = R.IdArea
left outer join Position PO on PO.Id = R.IdPosition
left outer join CC on CC.Id = R.IdCC
where R.Id = @PrzId
        ">
    <SelectParameters>
        <%--
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
        --%>
        <asp:ControlParameter ControlID="hidPrzId" Name="PrzId" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="DoMonit" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRqZast" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="DoMonit" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRqZast" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="AutoPowrot" Type="Boolean" />
    </InsertParameters>    
</asp:SqlDataSource>










<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, null as cc, 0 as Sort
union    
select Id, cc + ISNULL(' - ' + Nazwa, '') as Nazwa, cc, 1 as Sort
from CC 
where Wybor = 1
order by Sort, cc
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Commodity as Nazwa, 1 as Sort from Commodity where Aktywne = 1 order by Sort, Nazwa
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Area as Nazwa, 1 as Sort from Area where Aktywne = 1 order by Sort, Nazwa
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Position as Nazwa, 1 as Sort from Position where Aktywne = 1 order by Sort, Nazwa
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as Nazwa, 0 as Sort
union    
select Id, Nazwa, 1 as Sort from Strefy where Aktywna = 1 order by Sort, Nazwa
    ">
</asp:SqlDataSource>



















<asp:SqlDataSource ID="x_SqlDataSource1" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    oninserted="SqlDataSource1_Inserted" 
    oninserting="SqlDataSource1_Inserting"
    UpdateCommand="UPDATE [Przypisania] SET [IdPracownika] = @IdPracownika, [Od] = @Od, [Do] = @Do, [DoMonit] = @DoMonit, [IdKierownika] = @IdKierownika, [IdCC] = @IdCC, [IdCommodity] = @IdCommodity, [IdArea] = @IdArea, [IdPosition] = @IdPosition, [IdKierownikaRq] = @IdKierownikaRq, [DataRq] = @DataRq, [UwagiRq] = @UwagiRq, [IdKierownikaAcc] = @IdKierownikaAcc, [DataAcc] = @DataAcc, [UwagiAcc] = @UwagiAcc, [Status] = @Status, [Typ] = @Typ, RcpStrefaId = @RcpStrefaId WHERE [Id] = @Id"
    InsertCommand="INSERT INTO [Przypisania] ([IdPracownika], [Od], [Do], [DoMonit], [IdKierownika], [IdCC], [IdCommodity], [IdArea], [IdPosition], [IdKierownikaRq], [DataRq], [UwagiRq], [IdKierownikaAcc], [DataAcc], [UwagiAcc], [Status], [Typ], RcpStrefaId) VALUES (@IdPracownika, @Od, @Do, @DoMonit, @IdKierownika, @IdCC, @IdCommodity, @IdArea, @IdPosition, @IdKierownikaRq, @DataRq, @UwagiRq, @IdKierownikaAcc, @DataAcc, @UwagiAcc, @Status, @Typ, @RcpStrefaId)" 
    DeleteCommand="DELETE FROM [Przypisania] WHERE [Id] = @Id" 
    SelectCommand="
select R.*, 
	ISNULL(R.Do, R.DoMonit) as DoM,
    C.Commodity, A.Area, PO.Position, CC.cc, CC.cc + ISNULL(' - ' + CC.Nazwa, '') as CCNazwa,
    P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') as Pracownik,   
    P.DataZatr, P.Kierownik,
    S.Nazwa as RcpStrefa, S.Id as RcpStrefaId,
    case when R.IdKierownika = 0 then 
        'Główny poziom struktury' 
    else K.Nazwisko + ' ' + K.Imie + ISNULL(' (' + K.KadryId + ')', '') end as KierownikNI
from Pracownicy P
left outer join Przypisania R on R.IdPracownika = P.Id and @Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left outer join Strefy S on S.Id = R.RcpStrefaId
left outer join Pracownicy K on K.Id = R.IdKierownika
left outer join Commodity C on C.Id = R.IdCommodity
left outer join Area A on A.Id = R.IdArea
left outer join Position PO on PO.Id = R.IdPosition
left outer join CC on CC.Id = R.IdCC
where P.Id = @IdPracownika 
        ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidPrzId" Name="PrzId" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="DoMonit" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="DoMonit" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="RcpStrefaId" Type="Int32" />
    </InsertParameters>    
</asp:SqlDataSource>
    
    
    
    
       <%--

        <span style="">Id:
        <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
        <br />
        IdPracownika:
        <asp:TextBox ID="IdPracownikaTextBox" runat="server" 
            Text='<%# Bind("IdPracownika") %>' />
        <br />
        Od:
        <asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("Od") %>' />
        <br />
        Do:
        <asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("Do") %>' />
        <br />
        IdKierownika:
        <asp:TextBox ID="IdKierownikaTextBox" runat="server" 
            Text='<%# Bind("IdKierownika") %>' />
        <br />
        IdCC:
        <asp:TextBox ID="IdCCTextBox" runat="server" Text='<%# Bind("IdCC") %>' />
        <br />
        IdCommodity:
        <asp:TextBox ID="IdCommodityTextBox" runat="server" 
            Text='<%# Bind("IdCommodity") %>' />
        <br />
        IdArea:
        <asp:TextBox ID="IdAreaTextBox" runat="server" Text='<%# Bind("IdArea") %>' />
        <br />
        IdPosition:
        <asp:TextBox ID="IdPositionTextBox" runat="server" 
            Text='<%# Bind("IdPosition") %>' />
        <br />
        IdKierownikaRq:
        <asp:TextBox ID="IdKierownikaRqTextBox" runat="server" 
            Text='<%# Bind("IdKierownikaRq") %>' />
        <br />
        DataRq:
        <asp:TextBox ID="DataRqTextBox" runat="server" Text='<%# Bind("DataRq") %>' />
        <br />
        UwagiRq:
        <asp:TextBox ID="UwagiRqTextBox" runat="server" Text='<%# Bind("UwagiRq") %>' />
        <br />
        IdKierownikaAcc:
        <asp:TextBox ID="IdKierownikaAccTextBox" runat="server" 
            Text='<%# Bind("IdKierownikaAcc") %>' />
        <br />
        DataAcc:
        <asp:TextBox ID="DataAccTextBox" runat="server" Text='<%# Bind("DataAcc") %>' />
        <br />
        UwagiAcc:
        <asp:TextBox ID="UwagiAccTextBox" runat="server" 
            Text='<%# Bind("UwagiAcc") %>' />
        <br />
        Status:
        <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
        <br />
        Typ:
        <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
        <br />
        <br />
        <br />
        </span>
        --%>
        







        <%--

            <td class="params">
                
                <span class="label">Centrum kosztów:</span>
                <asp:DropDownList ID="ddlCC" runat="server">
                </asp:DropDownList>
                
                
            </td>

            <td class="buttons">
                <asp:Button ID="btZapisz" CssClass="button100" runat="server" Text="Zapisz" onclick="btZapisz_Click" />
                
            </td>

        <span style="">Id:
        <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
        <br />
        IdPracownika:
        <asp:TextBox ID="IdPracownikaTextBox" runat="server" 
            Text='<%# Bind("IdPracownika") %>' />
        <br />
        Od:
        <asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("Od") %>' />
        <br />
        Do:
        <asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("Do") %>' />
        <br />
        IdKierownika:
        <asp:TextBox ID="IdKierownikaTextBox" runat="server" 
            Text='<%# Bind("IdKierownika") %>' />
        <br />
        IdCC:
        <asp:TextBox ID="IdCCTextBox" runat="server" Text='<%# Bind("IdCC") %>' />
        <br />
        IdCommodity:
        <asp:TextBox ID="IdCommodityTextBox" runat="server" 
            Text='<%# Bind("IdCommodity") %>' />
        <br />
        IdArea:
        <asp:TextBox ID="IdAreaTextBox" runat="server" Text='<%# Bind("IdArea") %>' />
        <br />
        IdPosition:
        <asp:TextBox ID="IdPositionTextBox" runat="server" 
            Text='<%# Bind("IdPosition") %>' />
        <br />
        IdKierownikaRq:
        <asp:TextBox ID="IdKierownikaRqTextBox" runat="server" 
            Text='<%# Bind("IdKierownikaRq") %>' />
        <br />
        DataRq:
        <asp:TextBox ID="DataRqTextBox" runat="server" Text='<%# Bind("DataRq") %>' />
        <br />
        UwagiRq:
        <asp:TextBox ID="UwagiRqTextBox" runat="server" Text='<%# Bind("UwagiRq") %>' />
        <br />
        IdKierownikaAcc:
        <asp:TextBox ID="IdKierownikaAccTextBox" runat="server" 
            Text='<%# Bind("IdKierownikaAcc") %>' /> 
        <br />
        DataAcc:
        <asp:TextBox ID="DataAccTextBox" runat="server" Text='<%# Bind("DataAcc") %>' />
        <br />
        UwagiAcc:
        <asp:TextBox ID="UwagiAccTextBox" runat="server" 
            Text='<%# Bind("UwagiAcc") %>' />
        <br />
        Status:
        <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
        <br />
        Typ:
        <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
        <br />
        <br />
        <br />
        </span>
        --%>
        


            <%--
        <span style="">
        <span class="label">Id:</span>
        <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
        <br />
        <span class="label">IdPracownika:</span>
        
        <asp:Label ID="IdPracownikaLabel" runat="server" Text='<%# Eval("IdPracownika") %>' />
        <br />
        <span class="label">Od:</span>
        <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od") %>' />
        <br />
        Do:
        <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do") %>' />
        <br />
        IdKierownika:
        <asp:Label ID="IdKierownikaLabel" runat="server" 
            Text='<%# Eval("IdKierownika") %>' />
        <br />

        IdKierownikaRq:
        <asp:Label ID="IdKierownikaRqLabel" runat="server" 
            Text='<%# Eval("IdKierownikaRq") %>' />
        <br />
        DataRq:
        <asp:Label ID="DataRqLabel" runat="server" Text='<%# Eval("DataRq") %>' />
        <br />
        UwagiRq:
        <asp:Label ID="UwagiRqLabel" runat="server" Text='<%# Eval("UwagiRq") %>' />
        <br />
        IdKierownikaAcc:
        <asp:Label ID="IdKierownikaAccLabel" runat="server" 
            Text='<%# Eval("IdKierownikaAcc") %>' />
        <br />
        DataAcc:
        <asp:Label ID="DataAccLabel" runat="server" Text='<%# Eval("DataAcc") %>' />
        <br />
        UwagiAcc:
        <asp:Label ID="UwagiAccLabel" runat="server" Text='<%# Eval("UwagiAcc") %>' />
        <br />
        Status:
        <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
        <br />
        Typ:
        <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
        <br />
        <br />
        <br />
        </span>
            --%>