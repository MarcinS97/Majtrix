<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioskiUrlopoweSelect2.ascx.cs" Inherits="HRRcp.Portal.Controls.cntWnioskiUrlopoweSelect2" %>

    <%--
    InsertItemPosition='<%# GetInsertPosition() %>'
    <asp:HiddenField ID="hidBycMusi" runat="server" Value='<%# Eval("Id") %>' />
    --%>
    
<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
    
<asp:ListView ID="lvTypy" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" 
    GroupItemCount="2" 
    InsertItemPosition="None"
    onitemcommand="lvTypy_ItemCommand" 
    onselectedindexchanged="lvTypy_SelectedIndexChanged">
    <EmptyItemTemplate>
        <td runat="server" />
    </EmptyItemTemplate>
    <ItemTemplate>
        <td runat="server" class="it">
            <div class="it">
                <asp:LinkButton ID="LinkButton1" runat="server" 
                    CommandName="select"
                    CommandArgument='<%# Eval("Id") %>' >
                    <div runat="server" class="typ">
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                        <span class="typ">
                            <asp:Label ID="TypLabel" CssClass="typ1" runat="server" Text='<%# Eval("Typ") %>' />
                            <span class="typ2" runat="server" visible='<%# IsConfig() %>'>
                                <asp:CheckBox ID="AktywnyCheckBox" CssClass="check" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczny" Enabled="false"/>
                                <asp:CheckBox ID="WypelniaPracownikCheckBox" CssClass="check" runat="server" Checked='<%# Bind("WypelniaPracownik") %>' Text="Dla Pracownika" Enabled="false"/>
                                <asp:CheckBox ID="WypelniaKierownikCheckBox" CssClass="check" runat="server" Checked='<%# Bind("WypelniaKierownik") %>' Text="Dla Kierownika" Enabled="false"/>
                            </span>
                        </span>                        
                    </div>
                    <div runat="server" class="text">
                        <asp:Label ID="InfoLabel" CssClass="info" runat="server" Text='<%# GetText(Eval("Info")) %>' />
                    </div>
                    <div id="paDot3" runat="server" class="dot3" visible="true">
                        <span>...</span>
                    </div>
                    <div id="paButtons" class="buttons" runat="server" visible='<%# IsConfig() %>'>
                        <asp:Label ID="KolejnoscLabel" runat="server" CssClass="left" Text='<%# Eval("Kolejnosc") %>' visible='<%# IsConfig() %>'/>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" Visible="false" />
                        <asp:Button ID="EditButton" CssClass="control" runat="server" CommandName="Edit" Text="Edytuj" />
                    </div>        
                </asp:LinkButton>
            </div>
        </td>
    </ItemTemplate>
    <EditItemTemplate>
        <td id="Td1" runat="server" class="eit">
            <div class="a">
                <div class="typ">
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetPath(Eval("Image")) %>' />
                    <span class="typ">
                        <asp:TextBox ID="TypTextBox" runat="server" CssClass="textbox" MaxLength="100" Rows="2" TextMode="SingleLine" Text='<%# Bind("Typ") %>' />
                        <span id="Span1" class="typ2" runat="server" visible='<%# IsConfig() %>'>
                            <asp:CheckBox ID="AktywnyCheckBox" CssClass="check" runat="server" Checked='<%# Bind("Aktywny") %>' Text="Widoczny" />
                            <asp:CheckBox ID="WypelniaPracownikCheckBox" CssClass="check" runat="server" Checked='<%# Bind("WypelniaPracownik") %>' Text="Dla Pracownika" />
                            <asp:CheckBox ID="WypelniaKierownikCheckBox" CssClass="check" runat="server" Checked='<%# Bind("WypelniaKierownik") %>' Text="Dla Kierownika" />
                        </span>
                    </span>                        
                </div>
                <div class="text">
                    <br />
                    <span class="label">Limit maksymalny (dni):</span>
                    <asp:TextBox ID="tbLimit" runat="server" CssClass="limit textbox" MaxLength="3" Text='<%# Bind("Limit") %>' /><br />

                    <span class="label1">Pytanie: <b>Proszę o udzielenie ...</b></span>
                    <asp:TextBox ID="TypNapisTextBox" runat="server" CssClass="textbox" MaxLength="100" Text='<%# Bind("TypNapis") %>' />

                    <span class="label1">Informacja:</span>
                    <asp:TextBox ID="InfoTextBox" runat="server" CssClass="textbox" MaxLength="1000" Rows="3" TextMode="MultiLine" Text='<%# Bind("Info") %>' />

                </div>
                <div id="paButtons" class="buttons" runat="server" visible='<%# IsConfig() %>'>
                    <div class="kolejnosc">
                        <span class="label">Kolejność:</span>
                        <asp:TextBox ID="KolejnoscTextBox" CssClass="kolejnosc textbox" MaxLength="4" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                    </div>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                </div>            
            </div>
        </td>
    </EditItemTemplate>
    <InsertItemTemplate>
        <td runat="server" class="iit">
            Id:
            <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
            <br />
            Typ:
            <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            <br />
            Limit:
            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Limit") %>' />
            <br />
            TypNapis:
            <asp:TextBox ID="TypNapisTextBox" runat="server" 
                Text='<%# Bind("TypNapis") %>' />
            <br />
            Symbol:
            <asp:TextBox ID="SymbolTextBox" runat="server" Text='<%# Bind("Symbol") %>' />
            <br />
            IdKodyAbs:
            <asp:TextBox ID="IdKodyAbsTextBox" runat="server" 
                Text='<%# Bind("IdKodyAbs") %>' />
            <br />
            <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                Checked='<%# Bind("Aktywny") %>' Text="Aktywny" />
            <br />
            Kolejnosc:
            <asp:TextBox ID="KolejnoscTextBox" runat="server" 
                Text='<%# Bind("Kolejnosc") %>' />
            <br />
            Image:
            <asp:TextBox ID="ImageTextBox" runat="server" Text='<%# Bind("Image") %>' />
            <br />
            <asp:CheckBox ID="WypelniaPracownikCheckBox" runat="server" 
                Checked='<%# Bind("WypelniaPracownik") %>' Text="WypelniaPracownik" />
            <br />
            <asp:CheckBox ID="WypelniaKierownikCheckBox" runat="server" 
                Checked='<%# Bind("WypelniaKierownik") %>' Text="WypelniaKierownik" />
            <br />
            Info:
            <asp:TextBox ID="InfoTextBox" runat="server" Text='<%# Bind("Info") %>' />
            <br />
            <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                Text="Insert" />
            <br />
            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                Text="Clear" />
            <br />
        </td>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="cntWnioskiUrlopoweSelect table0">
            <tr runat="server">
                <td runat="server">
                    <table ID="groupPlaceholderContainer" runat="server" border="0" style="">
                        <tr ID="groupPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <GroupTemplate>
        <tr ID="itemPlaceholderContainer" runat="server">
            <td ID="itemPlaceholder" runat="server">
            </td>
        </tr>
    </GroupTemplate>
</asp:ListView>

<%--
    UpdateCommand="UPDATE [poWnioskiUrlopoweTypy] SET [Typ] = @Typ, [TypNapis] = @TypNapis, [Symbol] = @Symbol, [IdKodyAbs] = @IdKodyAbs, [Aktywny] = @Aktywny, [Kolejnosc] = @Kolejnosc, [Image] = @Image, [WypelniaPracownik] = @WypelniaPracownik, [WypelniaKierownik] = @WypelniaKierownik, [Info] = @Info WHERE [Id] = @Id">
    SelectCommand="
SELECT * FROM poWnioskiUrlopoweTypy
WHERE 
    @mode = 0 and WypelniaPracownik = 1 and Aktywny = 1 or
    @mode = 1 and WypelniaKierownik = 1 and Aktywny = 1 or
    @mode = 2 or
    @mode = 3
ORDER BY [Kolejnosc], [Typ]    
--%>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [poWnioskiUrlopoweTypy] WHERE [Id] = @Id" 
    InsertCommand="
INSERT INTO [poWnioskiUrlopoweTypy] ([Id], [Typ], [TypNapis], [Symbol], [IdKodyAbs], [Aktywny], [Kolejnosc], [Image], [WypelniaPracownik], [WypelniaKierownik], [Info], Limit) VALUES 
    (@Id, @Typ, @TypNapis, @Symbol, @IdKodyAbs, @Aktywny, @Kolejnosc, @Image, @WypelniaPracownik, @WypelniaKierownik, @Info, @Limit)" 
    SelectCommand="
--declare @mode int = 0
--declare @pracId int = 413
--declare @pracId int = 414
declare @data datetime 
set @data = GETDATE()

SELECT T.* FROM poWnioskiUrlopoweTypy T
left join UrlopLimity UL on UL.UrlopTyp = T.Typ2 and UL.IdPracownika = @pracId and @data between UL.DataOd and UL.DataDo 
WHERE 
    (
        @mode = 0 and T.WypelniaPracownik = 1 and T.Aktywny = 1 and (T.Typ2 != 'd' or UL.IdPracownika is not null) or
        @mode = 1 and T.WypelniaKierownik = 1 and T.Aktywny = 1 or
        @mode = 2 or
        @mode = 3
    )
    and T.Rodzaj = 0
ORDER BY [Kolejnosc], [Typ]
        " 
    UpdateCommand="UPDATE [poWnioskiUrlopoweTypy] SET [Typ] = @Typ, [TypNapis] = @TypNapis, [Aktywny] = @Aktywny, [Kolejnosc] = @Kolejnosc, [WypelniaPracownik] = @WypelniaPracownik, [WypelniaKierownik] = @WypelniaKierownik, [Info] = @Info, Limit = @Limit WHERE [Id] = @Id">
    <SelectParameters>
        <asp:Parameter Name="mode" Type="Int32" DefaultValue="0" />
        <asp:ControlParameter Name="pracId" ControlID="hidPracId" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="TypNapis" Type="String" />
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="IdKodyAbs" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Limit" Type="Int32" />
        <asp:Parameter Name="Image" Type="String" />
        <asp:Parameter Name="WypelniaPracownik" Type="Boolean" />
        <asp:Parameter Name="WypelniaKierownik" Type="Boolean" />
        <asp:Parameter Name="Info" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="TypNapis" Type="String" />
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="IdKodyAbs" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Limit" Type="Int32" />
        <asp:Parameter Name="Image" Type="String" />
        <asp:Parameter Name="WypelniaPracownik" Type="Boolean" />
        <asp:Parameter Name="WypelniaKierownik" Type="Boolean" />
        <asp:Parameter Name="Info" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<%--
            Id:
            <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            <br />
            Typ:
            <br />
            TypNapis:
            <asp:Label ID="TypNapisLabel" runat="server" Text='<%# Eval("TypNapis") %>' />
            <br />
            Symbol:
            <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
            <br />
            IdKodyAbs:
            <asp:Label ID="IdKodyAbsLabel" runat="server" Text='<%# Eval("IdKodyAbs") %>' />
            <br />
            <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                Checked='<%# Eval("Aktywny") %>' Enabled="false" Text="Aktywny" />
            <br />
            Kolejnosc:
            <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
            <br />
            Image:
            <asp:Label ID="ImageLabel" runat="server" Text='<%# Eval("Image") %>' />
            <br />
            <asp:CheckBox ID="WypelniaPracownikCheckBox" runat="server" 
                Checked='<%# Eval("WypelniaPracownik") %>' Enabled="false" 
                Text="WypelniaPracownik" />
            <br />
            <asp:CheckBox ID="WypelniaKierownikCheckBox" runat="server" 
                Checked='<%# Eval("WypelniaKierownik") %>' Enabled="false" 
                Text="WypelniaKierownik" />
            <br />
            Info:
            <br />
            <asp:Button ID="SelectButton" runat="server" CommandName="select" Text="Wybierz" />
--%>