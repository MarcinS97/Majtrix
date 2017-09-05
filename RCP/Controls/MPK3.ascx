<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPK3.ascx.cs" Inherits="HRRcp.Controls.MPK3" %>


<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>



<asp:HiddenField ID="hidPlanId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />
<asp:HiddenField ID="hidCzasZm3" runat="server" />
<asp:HiddenField ID="hidNadgD3" runat="server" />
<asp:HiddenField ID="hidNadgN3" runat="server" />
<asp:HiddenField ID="hidNocne3" runat="server" />

<asp:ListView ID="lvMPK" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="LastItem" 
    onitemcreated="lvMPK_ItemCreated" onitemdatabound="lvMPK_ItemDataBound" 
    oniteminserting="lvMPK_ItemInserting" onitemupdating="lvMPK_ItemUpdating" 
    ondatabound="lvMPK_DataBound" onlayoutcreated="lvMPK_LayoutCreated" 
    onitemcanceling="lvMPK_ItemCanceling" onitemediting="lvMPK_ItemEditing" 
    onitemupdated="lvMPK_ItemUpdated" ondatabinding="lvMPK_DataBinding">
    <ItemTemplate>
        <tr class="it">
            <td class="col1">
                <asp:Label ID="MPKLabel" runat="server" Text='<%# Eval("MPK") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="CzasZmLabel" runat="server" Text='<%# Eval("CzasZmT") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="NadgodzinyDzienLabel" runat="server" Text='<%# Eval("NadgodzinyDzienT") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="NadgodzinyNocLabel" runat="server" Text='<%# Eval("NadgodzinyNocT") %>' />
            </td>
            <td id="tdLastCol" runat="server" class="col2">
                <asp:Label ID="NocneLabel" runat="server" Text='<%# Eval("NocneT") %>' />
            </td>
            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" class="tbEnterMPKedt">
            <tr>
                <td>
                    Brak podziału
                    <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Podziel na centra kosztowe" Visible="false"/>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr id="trSumI" runat="server" class="it" visible="true">
            <td class="col1">
                <asp:Label ID="lbMPKPrz" runat="server" Text="wg przypisania"/>
            </td>
            <td class="col2">
                <asp:Label ID="lbCzasZmPrz" runat="server" />
            </td>
            <td class="col2">
                <asp:Label ID="lbNadgDPrz" runat="server" />
            </td>
            <td class="col2">
                <asp:Label ID="lbNadgNPrz" runat="server" />
            </td>
            <td id="tdLastCol" runat="server" class="col2">
                <asp:Label ID="lbNocnePrz" runat="server" />
            </td>
            <td id="tdControl" runat="server" class="control">
            </td>
        </tr>
        <tr class="iit">
            <td class="col1">
                <asp:DropDownList ID="ddlMPK" runat="server" DataSourceID="SqlDataSource2" DataTextField="CC" DataValueField="Id" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlMPK"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvMPK" runat="server" Display="Dynamic"
                    ValidationGroup="vgi"
                    ControlToValidate="ddlMPK"
                    OnServerValidate="ddlMPK_ValidateInsert"
                    CssClass="t4n error"
                    ErrorMessage="Powtórzone CC">
                </asp:CustomValidator>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col1">
                <asp:HiddenField ID="hidMPK" runat="server" Value='<%# Eval("IdMPK") %>' />
                <asp:DropDownList ID="ddlMPK" DataSourceID="SqlDataSource2" runat="server" DataTextField="CC" DataValueField="Id"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge" 
                    ControlToValidate="ddlMPK"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvMPK" runat="server" Display="Dynamic"
                    ValidationGroup="vge"
                    ControlToValidate="ddlMPK"
                    OnServerValidate="ddlMPK_ValidateEdit"
                    CssClass="t4n error"
                    ErrorMessage="Powtórzone CC">
                </asp:CustomValidator>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>'/>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>'/>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 tbEnterMPK hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th id="Th2" runat="server">
                                Czas pracy / Centrum kosztowe<br />
                                <span class="t4n">czas z dokładnością do 30 min.</span>
                                </th>
                            <th id="Th1" runat="server">
                                Zmiana</th>
                            <th runat="server">
                                Nadg.<br />w dzień</th>
                            <th runat="server">
                                Nadg.<br />w nocy</th>
                            <th id="thLastCol" runat="server">
                                Praca w<br />nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6"></asp:Label>)</th>
                            <th id="thControl" class="control" runat="server">&nbsp;</th>
                        </tr>
                        <tr runat="server" class="it total">
                            <td class="col1">Czas łączny <span class="t4">(hh:mm)</span></td>
                            <td class="col2"><asp:Label ID="lbCzasZm" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgD" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgN" runat="server" ></asp:Label></td>
                            <td id="thLastCol1" class="col2" runat="server"><asp:Label ID="lbNocne" runat="server" ></asp:Label></td>
                            <td id="thControl1" class="control" runat="server">&nbsp;</td>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <tr id="trSum" runat="server" class="it" visible="false">
                            <td class="col1">
                                <asp:Label ID="lbMPKPrz" runat="server" Text="wg przypisania"/>
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbCzasZmPrz" runat="server" />
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbNadgDPrz" runat="server" />
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbNadgNPrz" runat="server" />
                            </td>
                            <td id="tdLastCol" runat="server" class="col2 lastcol">
                                <asp:Label ID="lbNocnePrz" runat="server" />
                            </td>
                        </tr>                        
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>


<%--
<br />
--%>
<asp:Button ID="btWgPrzypisania" runat="server" CssClass="button" Text="Dodaj/uaktualnij wg przypisania" onclick="btWgPrzypisania_Click" Visible="false"/>
<asp:SqlDataSource ID="dsWgPrzypisania" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"    
    UpdateCommand="
declare @id int, 
	@szm int, @snd int, @snn int, @snoc int, @noclimit int,
	@szm0 int, @snd0 int, @snn0 int, @snoc0 int

select @szm = ISNULL(sum(P.CzasZm),0), @snd = ISNULL(sum(P.NadgodzinyDzien),0), @snn = ISNULL(sum(P.NadgodzinyNoc),0), @snoc = ISNULL(sum(P.Nocne),0)
from PodzialKosztow P
where P.IdPracownika = @IdPracownika and P.Data = @Data and IdMPK != 0

--set @szm0 = case when @czasZm is null then null when @czasZm &gt; @szm then @czasZm - @szm else 0 end 
set @szm0 = null
set @snd0 = case when @nadgD is null then null when @nadgD &gt; @snd then @nadgD - @snd else 0 end
set @snn0 = case when @nadgN is null then null when @nadgN &gt; @snn then @nadgN - @snn else 0 end
set @noclimit = case when @nocne &gt; @snoc then @nocne - @snoc else 0 end   -- limit
set @snoc0 = case when @snn0 &gt; @noclimit then @noclimit else @snn0 end    -- tylko z nn

select * from PodzialKosztow P where P.IdPracownika = @IdPracownika and P.Data = @Data 
select @czasZm as czasZm, @nadgD nadgD, @nadgN nadgN, @nocne nocne, 
	@szm as szm, @snd snd, @snn snn, @snoc snoc, @noclimit as noclimit, @szm0 as szm0, @snd0 snd0, @snn0 snn0, @snoc0 snoc0

select @id = Id from PodzialKosztow where IdPracownika = @IdPracownika and Data = @Data and IdMPK = 0

if @id is null 
	insert into PodzialKosztow values (@IdPracownika, @Data, 0, 0, @szm0, @snd0, @snn0, @snoc0, null)
	--select 'INSERT', @IdPracownika, @Data, 0, 0, @szm0, @snd0, @snn0, @snoc0, null
else
	update PodzialKosztow set /*CzasZm = null,*/ NadgodzinyDzien = @snd0, NadgodzinyNoc = @snn0, Nocne = @snoc0 where Id = @id
	--select 'UPDATE', IdPracownika, Data, IdPlanPracy, IdMPK, @snd0, @snn0, @snoc0, Uwagi, Id from PodzialKosztow where Id = @id
    ">
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidCzasZm3" Name="czasZm" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidNadgD3" Name="nadgD" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidNadgN3" Name="nadgN" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidNocne3" Name="nocne" PropertyName="Value" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>

<%--
SELECT P.Typ, P.Id, P.IdMPK, 
    P.MPK, 
    P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne,
    dbo.ToTimeHMM(P.CzasZm) as CzasZmT, 
    dbo.ToTimeHMM(P.NadgodzinyDzien) as NadgodzinyDzienT, 
    dbo.ToTimeHMM(P.NadgodzinyNoc) as NadgodzinyNocT, 
    dbo.ToTimeHMM(P.Nocne) as NocneT, 
    P.Uwagi,
	1 as Sort 
FROM VPodzialKosztow P
WHERE IdPracownika = @IdPracownika and Data = @Data 
order by Typ, Id
--%>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="
SELECT P.Id, P.IdMPK as IdMPK, 
    case when P.IdMPK = 0 then 'wg przypisania'
    else CC.cc + ' - ' + CC.Nazwa 
    end as MPK, 
    P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne,
    dbo.ToTimeHMM(P.CzasZm) as CzasZmT, 
    dbo.ToTimeHMM(P.NadgodzinyDzien) as NadgodzinyDzienT, 
    dbo.ToTimeHMM(P.NadgodzinyNoc) as NadgodzinyNocT, 
    dbo.ToTimeHMM(P.Nocne) as NocneT, 
    P.Uwagi 
FROM PodzialKosztow P
left join CC on CC.Id = P.IdMPK
WHERE IdPracownika = @IdPracownika and Data = @Data 
ORDER BY P.Id
    "
    DeleteCommand="DELETE FROM [PodzialKosztow] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PodzialKosztow] ([IdPlanPracy], [Data], [IdPracownika], [IdMPK], [CzasZm], [NadgodzinyDzien], [NadgodzinyNoc], [Nocne], [Uwagi]) VALUES (@IdPlanPracy, @Data, @IdPracownika, @IdMPK, @CzasZm, @NadgodzinyDzien, @NadgodzinyNoc, @Nocne, @Uwagi)" 
    UpdateCommand="UPDATE [PodzialKosztow] SET [IdMPK] = @IdMPK, [CzasZm] = @CzasZm, [NadgodzinyDzien] = @NadgodzinyDzien, [NadgodzinyNoc] = @NadgodzinyNoc, [Nocne] = @Nocne, [Uwagi] = @Uwagi WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdMPK" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPlanId" Name="IdPlanPracy" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
        <asp:Parameter Name="IdMPK" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as CC, 0 as Sort
union all 
--xselect 0 as Id, dbo.fn_GetPracLastCC(@IdPracownika, @Data, 5, ',') as CC, 1 as Sort 
--select 0 as Id, 'wg przypisania' as CC, 1 as Sort 
--union all 
SELECT Id, cc + ' - ' + Nazwa as CC, 2 as Sort FROM [CC] 
WHERE @Data between AktywneOd and ISNULL(AktywneDo, @Data) ORDER BY Sort, cc
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

