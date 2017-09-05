<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmUstawienia.ascx.cs" Inherits="HRRcp.Controls.AdmUstawienia" %>

<%@ Register src="DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>

<asp:ListView ID="lvParametry" runat="server" DataKeyNames="Id" 
DataSourceID="SqlDataSource3" onitemdatabound="lvParametry_ItemDataBound" 
    onitemupdating="lvParametry_ItemUpdating" 
    onitemupdated="lvParametry_ItemUpdated">
    <ItemTemplate>
        <table class="table3B">
            <tr>
                <td class="col1">
                    Start systemu:
                </td>
                <td class="col2">
                    <asp:Label ID="StartSystemuLabel" runat="server" Text='<%# Eval("StartSystemu", "{0:d}") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Okres rozliczeniowy od-do:
                </td>
                <td class="col2">
                    <asp:Label ID="OkresOdLabel" runat="server" Text='<%# Eval("OkresOd") %>' /> - 
                    <asp:Label ID="OkresDoLabel" runat="server" Text='<%# Eval("OkresDo") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Godziny nocne od:
                </td>
                <td class="col2">
                    <asp:Label ID="NocneOdLabel" runat="server" Text='<%# Eval("NocneOdHHMM") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Godziny nocne do:
                </td>
                <td class="col2">
                    <asp:Label ID="NocneDoLabel" runat="server" Text='<%# Eval("NocneDoHHMM") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Domyślny czas przerwy podczas zmiany [min]:
                </td>
                <td class="col2">
                    <asp:Label ID="PrzerwaMMLabel" runat="server" Text='<%# Eval("PrzerwaMM") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Domyślny czas przerwy podczas nadgodzin [min]:
                </td>
                <td class="col2">
                    <asp:Label ID="Przerwa2MMLabel" runat="server" Text='<%# Eval("Przerwa2MM") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Domyślny margines ostrzegania [min]: 
                </td>
                <td class="col2">
                    <asp:Label ID="MarginesMMLabel" runat="server" Text='<%# Eval("MarginesMM") %>' />
                </td>
            </tr>

            <tr>
                <td class="col1">
                    Zaokrąglenia czasu pracy dla dni:
                </td>
                <td class="col2">
                    <asp:Label ID="ZaokrLabel" runat="server" Text='<%# Eval("Zaokr") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Typ zaokrągleń dla dni:
                </td>
                <td class="col2">
                    <asp:Label ID="ZaokrTypeLabel" runat="server" Text='<%# Eval("ZaokrType") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Zaokrąglenia czasu pracy dla sum:
                </td>
                <td class="col2">
                    <asp:Label ID="ZaokrSumLabel" runat="server" Text='<%# Eval("ZaokrSum") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Typ zaokrągleń dla sum:
                </td>
                <td class="col2">
                    <asp:Label ID="ZaokrSumTypeLabel" runat="server" Text='<%# Eval("ZaokrSumType") %>' />
                </td>
            </tr>
        </table>
        <div class="buttons">
            <asp:Button ID="EditButton" CssClass="button75" runat="server" CommandName="Edit" Text="Edycja" />
        </div>
    </ItemTemplate>
    <EditItemTemplate>
        <table class="table3B">
            <tr>
                <td class="col1">
                    Start systemu:
                </td>
                <td class="col2">
                    <uc1:DateEdit ID="deSysStart" runat="server" Opis="(rrrr.mm.dd)" Date='<%# Bind("StartSystemu") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Okres rozliczeniowy do:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlOkresDo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Godziny nocne od:
                </td>
                <td class="col2">
                    <uc1:TimeEdit ID="teNocneOd" runat="server" Format="HH:mm" Opis="(hh:mm)" Seconds='<%# Bind("NocneOd") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Godziny nocne do:
                </td>
                <td class="col2">
                    <uc1:TimeEdit ID="teNocneDo" runat="server" Format="HH:mm" Opis="(hh:mm)" Seconds='<%# Bind("NocneDo") %>' />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Domyślny czas przerwy podczas zmiany [min]:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlPrzerwaMM" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Domyślny czas przerwy podczas nadgodzin [min]:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlPrzerwa2MM" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Domyślny margines ostrzegania [min]: 
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlMarginesMM" runat="server" />
                </td>
            </tr>

            <tr>
                <td class="col1">
                    Zaokrąglenia czasu pracy dla dni:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlZaokr" CssClass="timeround" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Typ zaokrągleń dla dni:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlZaokrType" CssClass="timeround" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Zaokrąglenia czasu pracy dla sum:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlZaokrSum" CssClass="timeround" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Typ zaokrągleń dla sum:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlZaokrSumType" CssClass="timeround" runat="server" />
                </td>
            </tr>
        </table>
        <div class="buttons">
            <asp:Button ID="UpdateButton" CssClass="button75" runat="server" CommandName="Update" Text="Zapisz" />
            <asp:Button ID="CancelButton" CssClass="button75" runat="server" CommandName="Cancel" Text="Anuluj" />
        </div>
    </EditItemTemplate>
    <EmptyDataTemplate>
        <span>Brak ustawień</span>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div ID="itemPlaceholderContainer" runat="server" class="cntAdmUstawienia">
            <span ID="itemPlaceholder" runat="server" />
        </div>
        <div style="">
        </div>
    </LayoutTemplate>
</asp:ListView>    

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
DeleteCommand="DELETE FROM [Ustawienia] WHERE [Id] = @Id" 
InsertCommand="INSERT INTO [Ustawienia] ([StartSystemu], [NocneDo], [NocneOd], [Przerwa2MM], [MarginesMM], [PrzerwaMM], [ZaokrSumType], [ZaokrSum], [ZaokrType], [Zaokr], [Id], [OkresOd], [OkresDo]) VALUES (@StartSystemu, @NocneDo, @NocneOd, @Przerwa2MM, @MarginesMM, @PrzerwaMM, @ZaokrSumType, @ZaokrSum, @ZaokrType, @Zaokr, @Id, @OkresOd, @OkresDo)" 
SelectCommand="SELECT *, dbo.ToTimeHMM(NocneOd) as NocneOdHHMM, dbo.ToTimeHMM(NocneDo) as NocneDoHHMM FROM [Ustawienia]"         
UpdateCommand="UPDATE [Ustawienia] SET [StartSystemu] = @StartSystemu, [NocneDo] = @NocneDo, [NocneOd] = @NocneOd, [Przerwa2MM] = @Przerwa2MM, [MarginesMM] = @MarginesMM, [PrzerwaMM] = @PrzerwaMM, [ZaokrSumType] = @ZaokrSumType, [ZaokrSum] = @ZaokrSum, [ZaokrType] = @ZaokrType, [Zaokr] = @Zaokr, [OkresOd] = @OkresOd, [OkresDo] = @OkresDo WHERE [Id] = @Id">
<DeleteParameters>
    <asp:Parameter Name="Id" Type="Int32" />
</DeleteParameters>
<UpdateParameters>
    <asp:Parameter Name="StartSystemu" Type="DateTime" />
    <asp:Parameter Name="NocneDo" Type="Int32" />
    <asp:Parameter Name="NocneOd" Type="Int32" />
    <asp:Parameter Name="Przerwa2MM" Type="Int32" />
    <asp:Parameter Name="MarginesMM" Type="Int32" />
    <asp:Parameter Name="PrzerwaMM" Type="Int32" />
    <asp:Parameter Name="ZaokrSumType" Type="Int32" />
    <asp:Parameter Name="ZaokrSum" Type="Int32" />
    <asp:Parameter Name="ZaokrType" Type="Int32" />
    <asp:Parameter Name="Zaokr" Type="Int32" />
    <asp:Parameter Name="OkresOd" Type="Int32" />
    <asp:Parameter Name="OkresDo" Type="Int32" />
    <asp:Parameter Name="Id" Type="Int32" />
</UpdateParameters>
<InsertParameters>
    <asp:Parameter Name="StartSystemu" Type="DateTime" />
    <asp:Parameter Name="NocneDo" Type="Int32" />
    <asp:Parameter Name="NocneOd" Type="Int32" />
    <asp:Parameter Name="Przerwa2MM" Type="Int32" />
    <asp:Parameter Name="MarginesMM" Type="Int32" />
    <asp:Parameter Name="PrzerwaMM" Type="Int32" />
    <asp:Parameter Name="ZaokrSumType" Type="Int32" />
    <asp:Parameter Name="ZaokrSum" Type="Int32" />
    <asp:Parameter Name="ZaokrType" Type="Int32" />
    <asp:Parameter Name="Zaokr" Type="Int32" />
    <asp:Parameter Name="Id" Type="Int32" />
    <asp:Parameter Name="OkresOd" Type="Int32" />
    <asp:Parameter Name="OkresDo" Type="Int32" />
</InsertParameters>
</asp:SqlDataSource>
