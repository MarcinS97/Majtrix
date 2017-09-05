<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzeznaczNadg2.ascx.cs" Inherits="HRRcp.Controls.RozliczenieNadg.cntPrzeznaczNadg2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWniosekUrlopowy.ascx" tagname="cntWniosekUrlopowy" tagprefix="uc2" %>

<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
<asp:HiddenField ID="hidOkresOd" runat="server" Visible="false" />
<asp:HiddenField ID="hidOkresDo" runat="server" Visible="false" />
<asp:HiddenField ID="hidDzien" runat="server" Visible="false" />
<asp:HiddenField ID="hidIsAbsencja" runat="server" Visible="false" />

<asp:HiddenField ID="xx_hidData" runat="server" Visible="false" />

<asp:HiddenField ID="hidPlanId" runat="server" Visible="false" />
<asp:HiddenField ID="hidCzasZm" runat="server" Visible="false" />
<asp:HiddenField ID="hidNadgD" runat="server" Visible="false" />
<asp:HiddenField ID="hidNadgN" runat="server" Visible="false" />
<asp:HiddenField ID="hidNocne" runat="server" Visible="false" />

<asp:ListView ID="lvPodzial" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="None" 
    onitemcreated="lvPodzial_ItemCreated" 
    onitemdatabound="lvPodzial_ItemDataBound" 
    oniteminserting="lvPodzial_ItemInserting" 
    onitemupdating="lvPodzial_ItemUpdating" 
    ondatabound="lvPodzial_DataBound" 
    onlayoutcreated="lvPodzial_LayoutCreated" 
    onitemcanceling="lvPodzial_ItemCanceling" 
    onitemediting="lvPodzial_ItemEditing" 
    onitemupdated="lvPodzial_ItemUpdated" 
    onitemcommand="lvPodzial_ItemCommand" 
    ondatabinding="lvPodzial_DataBinding" 
    onitemdeleted="lvPodzial_ItemDeleted" oniteminserted="lvPodzial_ItemInserted">
    <ItemTemplate>
        <tr class="it" id="trLine" runat="server">
            <td id="tdData" runat="server" class="data col1">
                <asp:Label ID="lbData" runat="server" Text='<%# Eval("Data", "{0:d}") %>' />

                <asp:CheckBox ID="cbSelect" runat="server" Visible="false" />
                
                <asp:HiddenField ID="hidTyp10" runat="server" />
                <asp:HiddenField ID="hidSumNiedomiar" runat="server" />
                <asp:HiddenField ID="hidWymiarEtat" runat="server" Value='<%# Eval("WymiarEtat") %>'/>
                <asp:HiddenField ID="hidIsAbsencja" runat="server" Value='<%# Eval("IsAbsencja") %>'/>
                <asp:HiddenField ID="hidTyp" runat="server" Visible="false" Value='<%# Eval("Typ") %>'/>
            </td>
            <td class="typ">
                <asp:Label ID="lbTyp" runat="server" Text='<%# Eval("TypNazwa") %>' />
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Typ") %>' Visible="false" />
            </td>
            <td class="zadzien">
                <uc1:DateEdit ID="deZaDzien2" runat="server" Date='<%# Eval("ZaDzien") %>' Visible="false" ValidationGroup="vgi"/><br />
                <asp:Label ID="lbZaDzien" runat="server" Text='<%# Eval("ZaDzien", "{0:d}") %>' />
                <asp:Button ID="btWniosek" runat="server" CommandName="wniosek" CommandArgument='<%# "|" + Eval("Data", "{0:d}") %>' Text="Wydrukuj wniosek - wolne" Visible="false" ToolTip="Wydruk wniosku o wolne za nadgodziny"/>
                <asp:Button ID="btWniosekOdpr" runat="server" CommandName="wniosek2" CommandArgument='<%# "|" + Eval("Data", "{0:d}") %>' Text="Wydrukuj wniosek - wyjście" Visible="false" ToolTip="Wydruk wniosku o wcześniejsze wyjście"/>
            </td>
            <td class="time timeNied blthick">
                <asp:Label ID="lbNiedomiar" runat="server" Text='<%# Eval("tNiedomiar") %>' />
            </td>
            <td class="time time50">
                <asp:Label ID="lbN50" runat="server" Text='<%# Eval("tN50") %>' />
            </td>
            <td class="time time100">
                <asp:Label ID="lbN100" runat="server" Text='<%# Eval("tN100") %>' />
            </td>

            <td class="time time50 blthick">
                <asp:Label ID="lbN50WnWypl" runat="server" Text='<%# Eval("tN50WnWypl") %>' />
                <asp:HiddenField ID="lbFN50WnWypl" runat="server" Value='<%# Eval("FN50WnWypl") %>' />
            </td>
            <td class="time time100">
                <asp:Label ID="lbN100WnWypl" runat="server" Text='<%# Eval("tN100WnWypl") %>' />
                <asp:HiddenField ID="lbFN100WnWypl" runat="server" Value='<%# Eval("FN100WnWypl") %>' />
            </td>
            <td class="time time50 blthick">
                <asp:Label ID="lbN50WnPrzezn" runat="server" Text='<%# Eval("tN50WnPrzezn") %>' />
                <asp:HiddenField ID="lbFN50WnPrzezn" runat="server" Value='<%# Eval("FN50WnPrzezn") %>' />
            </td>
            <td class="time time100">
                <asp:Label ID="lbN100WnPrzezn" runat="server" Text='<%# Eval("tN100WnPrzezn") %>' />
                <asp:HiddenField ID="lbFN100WnPrzezn" runat="server" Value='<%# Eval("FN100WnPrzezn") %>' />
            </td>

            <td class="uwagi  blthick lastcol" id="tdLastCol" runat="server">
                <asp:Label ID="lbUwagi" runat="server" Text='<%# Eval("Uwagi") %>' />
            </td>
            <td id="tdControl" runat="server" class="control lastcol">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" Visible="false" />
                <asp:Button ID="EditButton2" runat="server" CommandName="Edit2" Text="Edytuj" Visible="false" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                <asp:Button ID="InsertButton" runat="server" CommandName="daynewrecord" Text="Dodaj" />

                <asp:Button ID="btEditSave" runat="server" CommandName="daysave2" Visible="false" Text="Zapisz" ValidationGroup="vgi"/>
                <asp:Button ID="btEditCancel" runat="server" CommandName="daycancel2" Visible="false" Text="Anuluj" />
            </td>
        </tr>
        <%---- klasyfikacja ----%>
        <tr id="trLineInsert" runat="server" visible="false" class="iit">
            <td class="data col1">
                <asp:HiddenField ID="hidData" runat="server" Visible="false" Value='<%# Eval("Data", "{0:d}") %>'/>            
                <uc1:DateEdit ID="deData" runat="server" Date='<%# Eval("Data") %>' Visible="false"  ValidationGroup="vgi"/>
                <asp:Label ID="lbData2" runat="server" Text='<%# Eval("Data", "{0:d}") %>' Visible="false"/>
            </td>
            <td class="typ">
                <asp:DropDownList ID="ddlKlas" runat="server" DataSourceID="SqlDataSource2" DataTextField="Rodzaj" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlKlas_SelectedIndexChanged" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic" 
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlKlas"
                    CssClass="t4n error"
                    ErrorMessage="Brak klasyfikacji" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvKlas" runat="server" Display="Dynamic"
                    ValidationGroup="vgi"
                    ControlToValidate="ddlKlas"
                    OnServerValidate="ddlKlas_ValidateInsert"
                    CssClass="t4n error"
                    ErrorMessage="Klasyfikacja już istnieje">
                </asp:CustomValidator>
                <asp:Label ID="lbInfox1"  runat="server" CssClass="info" Text="<br />Wartość zostanie oddana 1:1" Visible="false"></asp:Label>
                <asp:Label ID="lbInfox15" runat="server" CssClass="info" Text="<br />Wartość zostanie przemnożona przez 1,5" Visible="false"></asp:Label>
            </td>
            <td class="zadzien">
                <uc1:DateEdit ID="deZaDzien" runat="server" Date='<%# Eval("ZaDzien") %>' Visible="false" ValidationGroup="vgi"/><br />
                <asp:DropDownList ID="ddlZaDzien" runat="server" DataSourceID="SqlDataSource4" DataTextField="ZaDzien" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlZaDzien_SelectedIndexChanged" Visible="false"></asp:DropDownList>
            </td>
            <td class="time timeNied blthick">
                <uc1:TimeEdit ID="teNiedomiar" runat="server" AllowNegative="true" Visible="false" />
            </td>
            <td class="time time50">
                <uc1:TimeEdit ID="teN50" AllowNegative="true" runat="server" />
            </td>
            <td class="time time100">
                <uc1:TimeEdit ID="teN100" AllowNegative="true" runat="server" />                
            </td>
            <td class="blthick"></td><td></td>
            <td class="blthick"></td><td></td><%-- Wnioski --%>
            <td class="uwagi blthick" id="td1" runat="server">
                <asp:TextBox ID="tbUwagi" class="textbox" runat="server" />                
            </td>
            <td id="td2" runat="server" class="control lastcol">
                <asp:Button ID="btSave" runat="server" CommandName="dayinsert" Text="Zapisz" ValidationGroup="vgi"/>
                <asp:Button ID="btCancel" runat="server" CommandName="daycancel" Text="Anuluj" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" class="tbEnterNadgedt">
            <tr>
                <td>
                    Brak danych
                    <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Zaklasyfikuj nadgodziny" Visible="false"/>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="data col1">
            </td>
            <td class="typ">
                <asp:DropDownList ID="ddlKlas" runat="server" DataSourceID="SqlDataSource2" DataTextField="Rodzaj" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlKlasEdit_SelectedIndexChanged" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" 
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlKlas"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic"
                    ValidationGroup="vgi"
                    ControlToValidate="ddlKlas"
                    OnServerValidate="ddlKlas_ValidateInsert"
                    CssClass="t4n error"
                    ErrorMessage="Klasyfikacja już istnieje">
                </asp:CustomValidator>
            </td>
            <td class="zadzien">
                <uc1:DateEdit ID="deZaDzien" runat="server" Date='<%# Bind("ZaDzien") %>' Visible="false"  ValidationGroup="vge"/><br />
                <asp:DropDownList ID="ddlZaDzien" runat="server" DataSourceID="SqlDataSource4" DataTextField="ZaDzien" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlZaDzienEdit_SelectedIndexChanged" Visible="false"></asp:DropDownList>
            </td>
            <td class="time timeNied">
                <uc1:TimeEdit ID="teNiedomiar" runat="server" Seconds='<%# Bind("Niedomiar") %>' Visible="false" />
            </td>
            <td class="time time50">
                <uc1:TimeEdit ID="teN50" runat="server" Seconds='<%# Bind("n50") %>' />
            </td>
            <td class="time time100">
                <uc1:TimeEdit ID="teN100" runat="server" Seconds='<%# Bind("n100") %>' />                
            </td>
            <td class="uwagi" id="td1" runat="server">
                <asp:TextBox ID="TypTextBox" class="textbox" runat="server" Text='<%# Bind("Uwagi") %>' />                
            </td>











            <%--
            <td class="col1" colspan="2">
                <asp:HiddenField ID="hidRodzajId" runat="server" Value='<%# Eval("RodzajId") %>' />
                <asp:HiddenField ID="hidZaDzien" runat="server" Value='<%# Eval("ZaDzien") %>' />
                <asp:DropDownList ID="ddlNadg" DataSourceID="SqlDataSource2" runat="server" DataTextField="Rodzaj" DataValueField="Id"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge" 
                    ControlToValidate="ddlNadg"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvNadg" runat="server" Display="Dynamic"
                    ValidationGroup="vge"
                    ControlToValidate="ddlNadg"
                    OnServerValidate="ddlNadg_ValidateEdit"
                    CssClass="t4n error"
                    ErrorMessage="Klasyfikacja już istnieje">
                </asp:CustomValidator>
            </td>
            <td id="tdCzasZm" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2" id="tdLastCol" runat="server">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>'/>
            </td>
            <td id="tdNocne" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>'/>
            </td>
            --%>
            
            <%--
            <td class="col2">
                <asp:DropDownList ID="ddlZaDzien" runat="server" DataSourceID="SqlDataSource3" DataTextField="ZaDzien" DataValueField="Id" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqvZaDzien" runat="server" SetFocusOnError="True" Display="Dynamic" Enabled="false"
                    ValidationGroup="vge" 
                    ControlToValidate="ddlZaDzien"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            --%>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>











    <InsertItemTemplate>
        <%--
        <tr class="iit">        
            <td class="col1" colspan="2" align="right">
                Pozostało:
            </td>
            <td>
                <asp:Label ID="lbRestD" runat="server" />
            </td>
            <td>
            </td>
        </tr>
        --%>
        <tr class="iit">        
            <td class="col1" colspan="2">
                <asp:DropDownList ID="ddlNadg" runat="server" DataSourceID="SqlDataSource4" DataTextField="Rodzaj" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlNadgInsert_SelectedIndexChanged" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic" 
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlNadg"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvNadg" runat="server" Display="Dynamic"
                    ValidationGroup="vgi"
                    ControlToValidate="ddlNadg"
                    OnServerValidate="ddlKlas_ValidateInsert"
                    CssClass="t4n error"
                    ErrorMessage="Klasyfikacja już istnieje">
                </asp:CustomValidator>
            </td>
            <td id="tdCzasZm" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2" id="tdLastCol" runat="server">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>' />
            </td>
            <td id="tdNocne" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>' />
            </td>
            <%--
            <td class="col2">
                <asp:DropDownList ID="ddlZaDzien" runat="server" DataSourceID="SqlDataSource3" DataTextField="ZaDzien" DataValueField="Id" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqvZaDzien" runat="server" SetFocusOnError="True" Display="Dynamic" Enabled="false"
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlZaDzien"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            --%>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>









    <LayoutTemplate>
        <table runat="server" class="ListView1 tbRozliczenieNadgodzin hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th class="data col1" rowspan="3">
                                Data</th>
                            <th class="typ" rowspan="3">
                                Klasyfikacja</th>
                            <th class="data zadzien" rowspan="3">
                                Za dzień</th>
                            <th class="blthick" colspan="3">Czas pracy</th>
                            <th class="blthick" colspan="4">Wnioski o nadgodziny</th>
                            <th class="uwagi lastcol blthick" id="thLastCol" runat="server" rowspan="3">
                                Uwagi
                            </th>
                            <th class="control lastcol" id="thControl" runat="server" rowspan="3">&nbsp;</th>
                        </tr>
                        <tr>
                            <th class="time timeNied blthick" rowspan="2">
                                Niedomiar</th>
                            <th colspan="2">Nadgodziny</th>
                            <th class="blthick" colspan="2">Do wypłaty</th>
                            <th class="blthick" colspan="2">Do wybrania</th>

                        </tr>
                        <tr runat="server" style="">

                            <th class="time time50">
                                50%</th>
                            <th class="time time100">
                                100%</th>
                            
                            <th class="time time50 blthick">
                                50%</th>
                            <th class="time time100">
                                100%</th>
                            <th class="time time50 blthick">
                                50%</th>
                            <th class="time time100">
                                100%</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <tr id="trLastLine" runat="server" class="lastline">
                            <td colspan="2" align="right">
                                <div id="paWniosekOdpr" runat="server">
                                    Wniosek na dzień:
                                    <uc1:DateEdit ID="deWnNaDzien" runat="server" ValidationGroup="vgw"/><br />
                                </div>
                            </td>
                            <td colspan="9">
<%--                                <asp:Button ID="btWniosekOdpr" runat="server" CommandName="wniosek2a" Text="Wydruk wniosku o wcześniejsze wyjście" ValidationGroup="vgw" />--%>
                                <asp:Button ID="btWniosekOdpr" runat="server" CommandName="wniosek2a" Text="Wydrukuj wniosek - wyjście"  ValidationGroup="vgw" ToolTip="Wydruk wniosku o wcześniejsze wyjście" />
                                <asp:Button ID="btDoWyplatyAll50" runat="server" CommandName="dowyplatyall50" Text="Nadgodziny 50 -> do wypłaty" Visible="false"/>
                                <asp:Button ID="btDoWyplatyAll100" runat="server" CommandName="dowyplatyall100" Text="Nadgodziny 100 -> do wypłaty" Visible="false"/>
                                <asp:Button ID="btDoWyplatyAll" runat="server" CommandName="dowyplatyall" Text="Nadgodziny -> do wypłaty" Visible="false"/>
                            </td>
                            <td id="tdControl" runat="server">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:Button ID="btDoWyplaty50" CssClass="button_postback" runat="server" OnClick="btDoWyplaty50_Click"/>
<asp:Button ID="btDoWyplaty100" CssClass="button_postback" runat="server" OnClick="btDoWyplaty100_Click"/>

            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btPopup" runat="server" style="display: none;" />
                    <asp:ModalPopupExtender ID="extWniosekPopup" runat="server" 
                        TargetControlID="btPopup"
                        PopupControlID="paWniosekPopup" 
                        BackgroundCssClass="wnModalBackground" >
                        <Animations>
                            <OnShown>
                                <ScriptAction Script="popupEventHandler();" />
                            </OnShown>
                        </Animations>
                    </asp:ModalPopupExtender>                        
                    <asp:Panel ID="paWniosekPopup" runat="server" CssClass="wnModalPopup wniosekPopup" style="display: none;" >
                        <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            



<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="
select rnk.* 
    , convert(varchar(10), rnk.Data, 20) as DataStr
    , dbo.ToTimeHMM(rnk.Niedomiar) as tNiedomiar
    , dbo.ToTimeHMM(rnk.N50)  as tN50
    , dbo.ToTimeHMM(rnk.N100) as tN100
    , case when rnk.WymiarEtat = -rnk.Niedomiar then 1 else 0 end as IsAbsencja
    , dbo.ToTimeHMM(rnk.N50WnWypl)    tN50WnWypl
    , dbo.ToTimeHMM(rnk.N100WnWypl)   tN100WnWypl
    , dbo.ToTimeHMM(rnk.N50WnPrzezn)  tN50WnPrzezn
    , dbo.ToTimeHMM(rnk.N100WnPrzezn) tN100WnPrzezn
from VRozliczenieNadgodzinKartoteka rnk
where rnk.IdPracownika = @IdPracownika and rnk.Data between @OkresOd and @OkresDo 

--and (Niedomiar &lt;&gt; 0 or N50 &lt;&gt; 0 or N100 &lt;&gt; 0)

order by Data, Typ
    "
    DeleteCommand="DELETE FROM [PodzialNadgodzin] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PodzialNadgodzin] ([IdPlanPracy], [Data], [IdPracownika], [RodzajId], [CzasZm], [NadgodzinyDzien], [NadgodzinyNoc], [Nocne], [Uwagi], ZaDzien) VALUES 
                                                  (@IdPlanPracy, @Data, @IdPracownika, @RodzajId, @CzasZm, @NadgodzinyDzien, @NadgodzinyNoc, @Nocne, @Uwagi, @ZaDzien)" 
    UpdateCommand="UPDATE [PodzialNadgodzin] SET [RodzajId] = @RodzajId, [CzasZm] = @Niedomiar, [n50] = @n50, [n100] = @n100, [Uwagi] = @Uwagi, ZaDzien = @ZaDzien, DataWpisu = GETDATE(), AutorId = @AutorId WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidOkresOd" Name="OkresOd" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidOkresDo" Name="OkresDo" PropertyName="Value" Type="datetime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="RodzajId" Type="Int32" />
        <asp:Parameter Name="Niedomiar" Type="Int32" />
        <asp:Parameter Name="n50" Type="Int32" />
        <asp:Parameter Name="n100" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="ZaDzien" Type="DateTime" />
        <asp:Parameter Name="AutorId" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPlanId" Name="IdPlanPracy" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
        <asp:Parameter Name="RodzajId" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="n50" Type="Int32" />
        <asp:Parameter Name="n100" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="ZaDzien" Type="DateTime" />
    </InsertParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsWniosekZaDzien" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    UpdateCommand="update rcpNadgodzinyWnioski set Data2 = '{1}' where Id = {0}" />


<%--są nadgodziny--%>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Rodzaj, -1 as Sort
union 
SELECT Kod, 

--convert(varchar, Kod) + ' ' + Nazwa as 

Nazwa, Lp 
FROM Kody 
WHERE Typ = 'PODZNADG' and Kod &lt; 10 and Aktywny = 1 
--and (Kod &lt;&gt; 5 or dbo.dow(@data) = 5)  -- sobota - odpracowanie absencji
--and (Kod &lt;&gt; 6 or @isAbs = 1)          -- odpracowanie w sobotę absencji (całego dnia)
and (not Kod in (5,6) or dbo.dow(@data) in (5,6))  -- sobota - odpracowanie absencji

--and (not Kod in (5,6,7) or Kod in (5,6) and dbo.dow(@data) = 5 or Kod in (7) and dbo.dow(@data) = 6)  -- sobota, niedziela - odpracowanie absencji

ORDER BY Sort
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidDzien" Name="data" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidIsAbsencja" Name="isAbs" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<%--jest niedomiar--%>
<asp:SqlDataSource ID="SqlDataSource2B" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Rodzaj, -1 as Sort
union 
SELECT Kod, 

--convert(varchar, Kod) + ' ' + Nazwa as 

Nazwa, Lp 
FROM Kody 
WHERE Typ = 'PODZNADG' and (Kod &gt; 10 or Kod = 9) and Aktywny = 1 
and (not Kod in (15,16) or @isAbs = 1)     -- wolne za sobotę (cały dzień)
ORDER BY Sort">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidDzien" Name="data" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidIsAbsencja" Name="isAbs" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<%--wybieram niedomiar--%>
<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, '.wybierz ...' as ZaDzien, -1 as Sort, null as Data
    union 
select 
    --convert(varchar, K.Kod) + '|' +
    '0|' +
    convert(varchar(10), PP.Data, 20) + '|' + 
    convert(varchar, PP.Id) + '|' + 
    convert(varchar, -N.Niedomiar) + '|' +
    dbo.ToTimeHMM(-N.Niedomiar) as Id,
    --K.Nazwa + ': ' + 
    convert(varchar(10), PP.Data, 20) + ' (' + 
        case when N.Rozliczony = 0 then '' else 
            dbo.ToTimeHMM(N.Niedomiar) + '/'
        end +  
        '-' + dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0)) +         
        ')' as Rodzaj,
    999999 as Sort, PP.Data
    --,PP.Data, R.WymiarCzasu - PP.CzasZm as Niedomiar, dbo.ToTimeHMM(R.WymiarCzasu - PP.CzasZm) as NiedomiarHMM,
    --,PP.Czas, PP.CzasZm, dbo.ToTimeHMM(PP.Czas), dbo.ToTimeHMM(PP.CzasZm) ,*
	--,MinusNadg
from PlanPracy PP 
left join Zmiany Z on Z.Id = ISNULL(PP.IdZmianyKorekta, PP.IdZmiany)

outer apply (select dbo.gettime(Z.Od) Od, dbo.gettime(Z.Do) Do) Z1
outer apply (select DATEDIFF(S, Z1.Od, Z1.Do) + case when Z1.Od &gt; Z1.Do then 86400 else 0 end WymiarCzasu) R
--left join PracownicyParametry R on R.IdPracownika = PP.IdPracownika and PP.Data between R.Od and ISNULL(R.Do, '20990909')

left join Absencja A on A.IdPracownika = PP.IdPracownika and PP.Data between A.DataOd and A.DataDo
--inner join Kody K on K.Typ = 'PODZNADG' and K.Kod = 3      --zeby mozna bylo wylaczyc
outer apply (
	select 
		--ISNULL(SUM(ISNULL(PN.n50, 0) + ISNULL(PN.n100, 0)), 0) as MinusNadg 
		Niedomiar,
		ISNULL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0) + Niedomiar as Rozliczony		
	from VRozliczenieNadgodzinDzienSuma PN 
	where PN.IdPracownika = PP.IdPracownika     --minus co juz odpracowane
	and PN.Data = PP.Data  
) as N
where 
PP.IdPracownika = @IdPracownika and PP.Akceptacja = 1 

--and Z.Typ = 0 --z dni roboczych
and not Z.Typ in (1,2,3)  --bez zmian sobota niedziela N1

and A.Id is null and ISNULL(PP.CzasZm, 0) &gt;= 0 and ISNULL(PP.NadgodzinyDzien, 0) = 0 and ISNULL(PP.NadgodzinyNoc, 0) = 0 and ISNuLL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0) &gt; 0  -- jest niedomiar
and PP.Data between @dOd and DATEADD(D, -1, @data)
and ISNULL(PP.IdZmianyKorekta, PP.IdZmiany) is not null
and N.Niedomiar &lt;&gt; 0
ORDER BY Sort, Data
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidOkresOd" Name="dOd" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidOkresDo" Name="dDo" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidDzien" Name="data" PropertyName="Value" Type="datetime" />
    </SelectParameters>
</asp:SqlDataSource>
<%--
select null as Id, '.wybierz ...' as ZaDzien, -1 as Sort, null as Data
    union 
select 
    convert(varchar, K.Kod) + '|' +
    convert(varchar(10), PP.Data, 20) + '|' + 
    convert(varchar, PP.Id) + '|' + 
    convert(varchar, ISNULL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0) - N.MinusNadg) + '|' +
    dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0) - N.MinusNadg) as Id,
    --K.Nazwa + ': ' + 
    convert(varchar(10), PP.Data, 20) + ' (' + 
        case when N.MinusNadg = 0 then '' else 
            dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0) - N.MinusNadg) + '/'
        end +  
        '-' + dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0)) +         
        ')' as Rodzaj,
    999999 as Sort, PP.Data
    --,PP.Data, R.WymiarCzasu - PP.CzasZm as Niedomiar, dbo.ToTimeHMM(R.WymiarCzasu - PP.CzasZm) as NiedomiarHMM,
    --,PP.Czas, PP.CzasZm, dbo.ToTimeHMM(PP.Czas), dbo.ToTimeHMM(PP.CzasZm) ,*
	--,MinusNadg
from PlanPracy PP 
left join Zmiany Z on Z.Id = ISNULL(PP.IdZmianyKorekta, PP.IdZmiany)
left join PracownicyParametry R on R.IdPracownika = PP.IdPracownika and PP.Data between R.Od and ISNULL(R.Do, '20990909')
left join Absencja A on A.IdPracownika = PP.IdPracownika and PP.Data between A.DataOd and A.DataDo
inner join Kody K on K.Typ = 'PODZNADG' and K.Kod = 3      --zeby mozna bylo wylaczyc
outer apply (
	select 
		--ISNULL(SUM(ISNULL(PN.n50, 0) + ISNULL(PN.n100, 0)), 0) as MinusNadg 
		ISNULL(SUM(ISNULL(PN.CzasZm, 0)), 0) as MinusNadg
	from PodzialNadgodzin PN 
	where PN.IdPracownika = PP.IdPracownika     --minus co juz odpracowane
	and PN.ZaDzien = PP.Data
) as N
where 
PP.IdPracownika = @IdPracownika and PP.Akceptacja = 1 and Z.Typ = 0 --z dni roboczych
and A.Id is null and ISNULL(PP.CzasZm, 0) &gt;= 0 and ISNULL(PP.NadgodzinyDzien, 0) = 0 and ISNULL(PP.NadgodzinyNoc, 0) = 0 and ISNuLL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0) &gt; 0
and PP.Data between @dOd and DATEADD(D, -1, @data)
and ISNULL(PP.IdZmianyKorekta, PP.IdZmiany) is not null
and ISNULL(R.WymiarCzasu, 28800) - ISNULL(PP.CzasZm, 0) - N.MinusNadg &gt; 0
ORDER BY Sort, Data
--%>



<%--wybieram nadgodziny--%>
<asp:SqlDataSource ID="SqlDataSource4B" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as ZaDzien, null as Data, -1 as Sort
    union 
select 
	'50|' +
    convert(varchar(10), PP.Data, 20) + '|' + 
    convert(varchar, PP.Id) + '|' + 	
    convert(varchar, n50) + '|' +
    dbo.ToTimeHMM(PP.n50 - N.Minus50) as Id,

    convert(varchar(10), PP.Data, 20) + ' (' + 
        case when N.Minus50 = 0 then '' 
        else dbo.ToTimeHMM(PP.n50 - N.Minus50) + '/'
        end +  
        dbo.ToTimeHMM(PP.n50) + ' - 50%)' as Rodzaj,
    PP.Data,
	1 as Sort
from PlanPracy PP 
outer apply (select ISNULL(SUM(n50), 0) as Minus50 from PodzialNadgodzin where IdPracownika = PP.IdPracownika     --minus co juz odpracowane
	and (
	    Data = PP.Data and RodzajId &lt; 10 or --(2,4,5) or
	    ZaDzien = PP.Data and RodzajId &gt; 10 --in (12,14,15)
	)) as N
where PP.IdPracownika = @IdPRacownika and PP.Akceptacja = 1 
and not ISNULL(PP.n50, 0) = 0 
and PP.Data between @dOd and DATEADD(D, -1, @data)
and not PP.n50 - N.Minus50 = 0

union all

select 
	'100|' +
    convert(varchar(10), PP.Data, 20) + '|' + 
    convert(varchar, PP.Id) + '|' + 	
    convert(varchar, n100) + '|' +
    dbo.ToTimeHMM(PP.n100 - N.Minus100) as Id,

    convert(varchar(10), PP.Data, 20) + ' (' + 
        case when N.Minus100 = 0 then '' 
        else dbo.ToTimeHMM(PP.n100 - N.Minus100) + '/'
        end +  
        dbo.ToTimeHMM(PP.n100) + ' - 100%)' as Rodzaj,
    PP.Data,
	2 as Sort  
from PlanPracy PP 
outer apply (select ISNULL(SUM(n100), 0) as Minus100 from PodzialNadgodzin where IdPracownika = PP.IdPracownika     --minus co juz odpracowane
	and (
	    Data = PP.Data and RodzajId &lt; 10 or --in (2,4,5) or 
	    ZaDzien = PP.Data and RodzajId &gt; 10 --in (12,14,15)	    
	)) as N
where PP.IdPracownika = @IdPRacownika and PP.Akceptacja = 1 
and not ISNULL(PP.n100, 0) = 0 
and PP.Data between @dOd and DATEADD(D, -1, @data)
and not PP.n100 - N.Minus100 = 0

order by Data, Sort 
">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidOkresOd" Name="dOd" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidOkresDo" Name="dDo" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidDzien" Name="data" PropertyName="Value" Type="datetime" />
    </SelectParameters>
</asp:SqlDataSource>




<%--niedomiar
<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as ZaDzien, null as Data
    union 
select 
    convert(varchar, PP.Id) + '|' + convert(varchar(10), PP.Data, 20) + '|' + convert(varchar, ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm) as Id,
    convert(varchar(10), PP.Data, 20) + ' (' + dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm) + ')' as ZaDzien,
    PP.Data
    --PP.Data, R.WymiarCzasu - PP.CzasZm as Niedomiar, dbo.ToTimeHMM(R.WymiarCzasu - PP.CzasZm) as NiedomiarHMM,
    --PP.Czas, PP.CzasZm, dbo.ToTimeHMM(PP.Czas), dbo.ToTimeHMM(PP.CzasZm) ,*
from PlanPracy PP 
left outer join PracownicyParametry R on R.IdPracownika = PP.IdPracownika and PP.Data between R.Od and ISNULL(R.Do, '20990909')
left outer join Absencja A on A.IdPracownika = PP.IdPracownika and PP.Data between A.DataOd and A.DataDo
where 
PP.IdPracownika = @IdPRacownika and
PP.Akceptacja = 1 and A.Id is null and CzasZm &gt;= 0 and ISNULL(NadgodzinyDzien, 0) = 0 and ISNULL(NadgodzinyNoc, 0) = 0 and R.WymiarCzasu - CzasZm &gt; 0
and PP.Data between @dOd and @dDo
and ISNULL(PP.IdZmianyKorekta, PP.IdZmiany) is not null
ORDER BY Data">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidOkresOd" Name="dOd" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidOkresDo" Name="dDo" PropertyName="Value" Type="datetime" />
    </SelectParameters>
</asp:SqlDataSource>
--%>




<asp:SqlDataSource ID="dsRozlNadgDoWyplaty" runat="server" 
    SelectCommand="
declare @typn int
declare @ids varchar(max)
declare @od datetime
declare @do datetime
set @typn = {0}
set @ids = '{1}'
set @od  = '{2}'
set @do  = '{3}'

declare @rod datetime
declare @rdo datetime
select @rod = DataOd, @rdo = DataDo from OkresyRozliczeniowe where @do between DataOd and DataDo
if @rod is null begin
	set @rod = @od
	set @rdo = @do
end

declare @pracownicy nvarchar(max)
declare @sp varchar(1) 
declare @emp varchar(1)
declare @sep varchar(10)
declare @nodata varchar(50)
set @sp = ' '
set @emp = ''
set @sep = ', '
set @nodata = null --'brak'

select 
  ' - ' + P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + '): ' 
+ ISNULL(STUFF((
	select top 3 @sep + convert(varchar(10), K.Data, 20)
	from VRozliczenieNadgodzinKartoteka K
	where K.IdPracownika = P.Id and K.Data between @rod and @rdo and K.Typ = 100 and K.Niedomiar &lt; 0
	order by K.Data 
	for XML PATH('')), 1, 2, @emp), @nodata) as Niedomiary
, ' - ' + P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + '): ' 
+ ISNULL(STUFF((
	select top 3 @sep + convert(varchar(10), K.Data, 20)
	from VRozliczenieNadgodzinKartoteka K
	inner join VRozliczenieNadgodzinKartoteka KP on KP.IdPracownika = K.IdPracownika and KP.Data = K.Data and KP.Typ = 1 -- są już do wyplaty, ale pozostały n50/100 do rozliczenia
	where K.IdPracownika = P.Id and K.Data between @od and @do and K.Typ = 100 and (@typn = 50 and K.N50 &gt; 0 or @typn = 100 and K.N100 &gt; 0)
	order by K.Data 
	for XML PATH('')), 1, 2, @emp), @nodata) as DoWyplatyNiezgodne
, P.Id, P.Nazwisko, P.Imie, P.KadryId
from Pracownicy P 
outer apply (select top 1 * from VRozliczenieNadgodzinKartoteka where IdPracownika = P.Id and Data between @od and @do and Typ = 100 and (@typn = 50 and N50 &gt; 0 or @typn = 100 and N100 &gt; 0)) N
where P.Id in (select items from dbo.SplitInt(@ids,',')) 
and N.Id is not null
order by P.Nazwisko, P.Imie, P.KadryId
    "
    UpdateCommand="
declare @typn int
declare @ids varchar(max)
declare @od datetime
declare @do datetime
declare @kierId int
declare @userId int
set @typn = {0}
set @ids = '{1}'
set @od  = '{2}'
set @do  = '{3}'
set @kierId = {4}
set @userId = {5}

insert into PodzialNadgodzin (IdPracownika,Data,RodzajId,Uwagi,ZaDzien,CzasZm,n50,n100,DataWpisu,AutorId)
select K.IdPracownika, K.Data, 1, 'nierozliczone - do wypłaty', null, null, K1.N50, K1.N100, GETDATE(), @userId
from VRozliczenieNadgodzinKartoteka K
outer apply (select top 1 * from VRozliczenieNadgodzinKartoteka where IdPracownika = K.IdPracownika and Data = K.Data and Typ = 1) KP --and (KP.N50 != K.N50 or KP.N100 != K.N100) -- są już do wyplaty, ale niezgodne
outer apply (select 
	case when @typn in (50,150) then K.N50 else 0 end N50, 
	case when @typn in (100,150) then K.N100 else 0 end N100
) K1 
where K.IdPracownika in (select items from dbo.SplitInt(@ids,',')) 
and KP.Id is null
and K.Data between @od and @do
and K.Typ = 100
and (@typn = 50 and K.N50 &gt; 0 or @typn = 100 and K.N100 &gt; 0 or @typn = 150 and (K.N50 &gt; 0 or K.N100 &gt; 0))
order by K.IdPracownika, K.Data
    "/>

