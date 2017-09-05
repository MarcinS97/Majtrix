<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ZmianyControl3.ascx.cs" Inherits="HRRcp.Controls.ZmianyControl3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="ZmianaGodziny3.ascx" TagName="ZmianaGodziny" TagPrefix="uc1" %>
<%@ Register Src="TimeEdit.ascx" TagName="TimeEdit" TagPrefix="uc1" %>

<div id="paZmianyAdm" runat="server" class="cntZmianyAdm">

    <asp:ListView ID="lvZmiany" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id"
        OnSelectedIndexChanged="lvZmiany_SelectedIndexChanged"
        OnItemDataBound="lvZmiany_ItemDataBound"
        OnItemCommand="lvZmiany_ItemCommand" OnItemCreated="lvZmiany_ItemCreated"
        OnItemInserting="lvZmiany_ItemInserting"
        OnItemUpdating="lvZmiany_ItemUpdating"
        OnItemEditing="lvZmiany_ItemEditing" OnPreRender="lvZmiany_PreRender"
        OnItemDeleting="lvZmiany_ItemDeleting" OnDataBound="lvZmiany_DataBound">
        <ItemTemplate>
            <div class="zmiana it round5<%# GetPanelClass(Eval("NowaLinia")) %>">
                <table class="zmiana">
                    <tr class="firstline">
                        <td class="col1">
                            <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                        </td>
                        <td colspan="3">
                            <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# Eval("Nazwa") %>' /><br />
                            <asp:CheckBox ID="VisibleCheckBox" runat="server" CssClass="checkbox" Checked='<%# Eval("Visible") %>' Enabled="false" Text="Wybór" />
                            <asp:CheckBox ID="cbWidoczna" runat="server" CssClass="checkbox" Checked='<%# Eval("Widoczna") %>' Enabled="false" Text="Widoczna" />
                        </td>
                    </tr>
                    <tr class="title" id="tr1" runat="server">
                        <td class="col1"></td>
                        <td class="col2"><span class="t1">Wymiar czasu pracy</span></td>
                        <td class="col3"><span class="t1">Stawka</span></td>
                        <td class="col4"></td>
                    </tr>
                    <tr class="line1" id="tr1b" runat="server">
                        <td class="col1"></td>
                        <td class="col2">
                            <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("CzasOd", "") %>' />
                            -
                        <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("CzasDo", "") %>' />&nbsp;&nbsp;
                        <%--                        <asp:Label ID="Label1" runat="server" Text='<%# GetMargines(Eval("Margines")) %>' />--%>
                        </td>
                        <td class="col3">
                            <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") %>' />%
                        </td>
                        <td class="col4"></td>
                    </tr>


                    <tr id="trNadgodzinyZwykle" runat="server" visible="false" class="line1">
                        <td class="col1"></td>
                        <td class="col2">Nadgodziny w dzień</td>
                        <td class="col3">
                            <asp:Label ID="lbNadgodzinyDzien" runat="server" Text='<%# Eval("NadgodzinyDzien") %>' />%
                        </td>
                        <td class="col4"></td>
                    </tr>
                    <tr id="trNadgodzinyNocne" runat="server" visible="false" class="line nadgodziny">
                        <td class="col1"></td>
                        <td class="col2">Nadgodziny w nocy</td>
                        <td class="col3">
                            <asp:Label ID="lbNadgodzinyNoc" runat="server" Text='<%# Eval("NadgodzinyNoc") %>' />%
                        </td>
                        <td class="col4"></td>
                    </tr>
                    <tr class="title" id="tr2" runat="server">
                        <td class="col1"></td>
                        <td class="col2"><span class="t1">Inne czasy</span></td>
                        <td class="col3"></td>
                        <td class="col4"></td>
                    </tr>
                    <tr class="line1" id="tr3" runat="server">
                        <td class="col1"></td>
                        <td class="col2" colspan="2">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("InneCzasyDisp") %>' /><br />
                            </label>
                        </td>
                        <td class="col4"></td>
                    </tr>



                    <tr class="title" id="trTyp" runat="server">
                        <td class="col1"></td>
                        <td class="col2"><span class="t1">Typ zmiany</span></td>
                        <td class="col3"></td>
                        <td class="col4"></td>
                    </tr>
                    <tr class="line1" id="trTypValue" runat="server">
                        <td class="col1"></td>
                        <td class="col2" colspan="2">
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("TypNazwa") %>' />
                        </td>
                        <td class="col4"></td>
                    </tr>



                </table>
                <div class="buttons">
                    <div class="params">
                        Kolejność:
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Kolejnosc0") %>' />
                    </div>
                    <asp:Button ID="DupButton" CssClass="button" runat="server" Visible="false" CommandName="Duplicate" Text="Duplikuj" />

                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit"><span aria-hidden="true" class="glyphicon glyphicon-edit"></span></asp:LinkButton>
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete"><span aria-hidden="true" class="glyphicon glyphicon-trash c-red"></span></asp:LinkButton>
                </div>
            </div>
        </ItemTemplate>
        <EmptyDataTemplate>
            <span>Brak danych</span>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <div class="fullline">
                <div class="zmiana eit iit round5">
                    <table class="zmiana">
                        <tr class="firstline">
                            <td class="col1">
                                <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                            </td>
                            <td>
                                <asp:HiddenField ID="hidColor" runat="server" Value='<%# Bind("Kolor") %>' />

                                <span class="label">Symbol:</span>
                            </td>
                            <td style="width: 90px;">

                                <asp:TextBox ID="TextBox1" CssClass="form-control input-sm" runat="server" MaxLength="10" Text='<%# Bind("Symbol") %>' />
                            </td>
                            <td>
                                <asp:ImageButton ID="KolorImageButton" CssClass="palette color" runat="Server"
                                    ImageUrl="~/images/buttons/palette.png"
                                    ToolTip="Kliknij aby zmienić kolor zmiany" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <span class="label">Nazwa:</span>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBox2" CssClass="form-control input-sm" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Kolejność:</span>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="tbKolejnosc" CssClass="form-control input-sm" MaxLength="5" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true"
                                    TargetControlID="tbKolejnosc"
                                    FilterType="Custom"
                                    ValidChars="0123456789" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Godziny:</span>
                            </td>

                            <td>
                                <asp:DropDownList ID="ddlCzasOd" runat="server"></asp:DropDownList>
                            </td>

                            <td>
                                <asp:DropDownList ID="ddlCzasDo" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Stawka:</span>
                            </td>

                            <td colspan="2">
                                <asp:DropDownList ID="ddlStawka" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </td>
                        </tr>



                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Rodzaj:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlTypZmiany" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlTypZmiany_SelectedIndexChanged">
                                    <asp:ListItem Text="podstawowa" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="bez nadgodzin" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="absencja" Value="2"></asp:ListItem>
                                    <%--<asp:ListItem Text="kolejne godziny" Value="0"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Stawka '50:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlStawkaZwykle" runat="server"></asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Stawka '100:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlStawkaNocne" runat="server"></asp:DropDownList>
                            </td>

                        </tr>


                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Typ zmiany:</span>
                            </td>

                            <td colspan="2">
                                <asp:DropDownList ID="ddlTyp" runat="server" DataSourceID="SqlDataSourceKody" DataTextField="Nazwa" DataValueField="Kod"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label" title="Margines nadgodzin. Licz nadgodziny od...">Margines:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlMarginesNadgodzin" runat="server" DataSourceID="dsMarginesNadgodzin" DataTextField="Nazwa" DataValueField="Kod"></asp:DropDownList>
                            </td>
                        </tr>



                        <tr>
                            <td>
                                <asp:HiddenField ID="hidInneCzasy" runat="server" Visible="false" Value='<%# Bind("InneCzasy") %>' />
                            </td>

                            <td>
                                <span class="label" title="Inne czasy trwanie zmiany">Inne czasy:</span>
                            </td>


                            <td colspan="2" style="white-space: normal;">
                                <div class="col-md-12">
                                    <asp:Repeater ID="rpZmiany" runat="server" DataSourceID="dsZmianyRep">
                                        <ItemTemplate>

                                            <asp:Label ID="Label6" Text='<%# Eval("Name") %>' runat="server" />

                                            <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CssClass=""
                                                CommandArgument='<%# Eval("Value").ToString() + ";" + Container.ItemIndex.ToString()  %>'>
                                <i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>


                            <asp:SqlDataSource ID="dsZmianyRep" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                SelectCommand="
--declare @InneCzasy nvarchar(MAX)
--set @InneCzasy = (select z.InneCzasy from zmiany z where z.Id = 1)

select dbo.ToTimeHMM(s.Items) Name, s.Items Value

from dbo.SplitIntSort(@InneCzasy,';') s">
                                <SelectParameters>
                                    <asp:ControlParameter Name="InneCzasy" Type="String" ControlID="hidInneCzasy" PropertyName="Value" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <tr>
                                <td></td>
                                <td></td>
                                <td colspan="2">
                                    <div class="col-md-6">
                                        <uc1:TimeEdit ID="teTimeIn" runat="server" Right="true" Format="HH:mm" Opis="(hh:mm)" InLineCount="4" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:LinkButton ID="lnkAddInneCzasy" runat="server" OnClick="lnkAddInneCzasy_Click" CssClass="">
                                <i class="btn btn-sm btn-success"><span class="glyphicon glyphicon-plus"></span></i></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>






                        <tr>
                            <td></td>

                            <td colspan="3">

                                <asp:CheckBox ID="CheckBox2" CssClass="checkbox" runat="server" Checked='<%# Bind("ObetnijOdGory") %>' Text="Zakończ z końcem zmiany" title="Przy spóźnieniu rejestruj czas tylko do końca zmiany" />
                                <asp:CheckBox ID="VisibleCheckBox" CssClass="checkbox" runat="server" Checked='<%# Bind("Visible") %>' Text="Wybór" />
                                <asp:CheckBox ID="CheckBox1" CssClass="checkbox" runat="server" Checked='<%# Bind("Widoczna") %>' Text="Widoczna" />
                            </td>
                        </tr>
                    </table>

                    <div class="buttons">
                        <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Insert"><span aria-hidden="true" class="glyphicon glyphicon-floppy-disk"></span></asp:LinkButton>
                        <asp:LinkButton ID="CancelButton" runat="server" CommandName="CancelInsert"><span aria-hidden="true" class="glyphicon glyphicon-share-alt c-red"></span></asp:LinkButton>
                    </div>



                </div>
            </div>
        </InsertItemTemplate>
        <EditItemTemplate>
            <div>
                <div class="zmiana eit iit round5">
                    <table class="zmiana">
                        <tr class="firstline">
                            <td class="col1">
                                <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                            </td>
                            <td>
                                <asp:HiddenField ID="hidColor" runat="server" Value='<%# Bind("Kolor") %>' />

                                <span class="label">Symbol:</span>
                            </td>
                            <td style="width: 90px;">

                                <asp:TextBox ID="TextBox1" CssClass="form-control input-sm" runat="server" MaxLength="10" Text='<%# Bind("Symbol") %>' />
                            </td>
                            <td>
                                <asp:ImageButton ID="KolorImageButton" CssClass="palette color" runat="Server"
                                    ImageUrl="~/images/buttons/palette.png"
                                    ToolTip="Kliknij aby zmienić kolor zmiany" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <span class="label">Nazwa:</span>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBox2" CssClass="form-control input-sm" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="3">
                                <hr class="divider">
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Godziny:</span>
                            </td>

                            <td>
                                <asp:DropDownList ID="ddlCzasOd" runat="server"></asp:DropDownList>
                            </td>

                            <td>
                                <asp:DropDownList ID="ddlCzasDo" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Stawka:</span>
                            </td>

                            <td colspan="2">
                                <asp:DropDownList ID="ddlStawka" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </td>
                        </tr>


                        <tr>
                            <td></td>
                            <td colspan="3">
                                <hr class="divider">
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Typ zmiany:</span>
                            </td>

                            <td colspan="2">
                                <asp:DropDownList ID="ddlTyp" runat="server" DataSourceID="SqlDataSourceKody" DataTextField="Nazwa" DataValueField="Kod"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Rodzaj:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlTypZmiany" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlTypZmiany_SelectedIndexChanged">
                                    <asp:ListItem Text="podstawowa" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="bez nadgodzin" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="absencja" Value="2"></asp:ListItem>
                                    <%--<asp:ListItem Text="kolejne godziny" Value="0"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Nadgodziny zwykłe:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlStawkaZwykle" runat="server"></asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Nadgodziny nocne:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlStawkaNocne" runat="server"></asp:DropDownList>
                            </td>

                        </tr>

                                                <tr>
                            <td></td>
                            <td colspan="3">
                                <hr class="divider">
                            </td>
                        </tr>

                        <tr>
                            <td></td>

                            <td>
                                <span class="label" title="Margines nadgodzin. Licz nadgodziny od...">Margines:</span>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlMarginesNadgodzin" runat="server" DataSourceID="dsMarginesNadgodzin" DataTextField="Nazwa" DataValueField="Kod"></asp:DropDownList>
                            </td>
                        </tr>





                        <tr>
                            <td>
                                <asp:HiddenField ID="hidInneCzasy" runat="server" Visible="false" Value='<%# Bind("InneCzasy") %>' />
                            </td>

                            <td>
                                <span class="label" title="Inne czasy trwanie zmiany">Inne czasy:</span>
                            </td>


                            <td colspan="2" style="white-space: normal;">
                                <div class="col-md-12">
                                    <asp:Repeater ID="rpZmiany" runat="server" DataSourceID="dsZmianyRep">
                                        <ItemTemplate>

                                            <asp:Label ID="Label6" Text='<%# Eval("Name") %>' runat="server" />

                                            <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CssClass=""
                                                CommandArgument='<%# Eval("Value").ToString() + ";" + Container.ItemIndex.ToString()  %>'>
                                                <i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton>
                                            <br />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>


                            <asp:SqlDataSource ID="dsZmianyRep" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                SelectCommand="
--declare @InneCzasy nvarchar(MAX)
--set @InneCzasy = (select z.InneCzasy from zmiany z where z.Id = 1)

select dbo.ToTimeHMM(s.Items) Name, s.Items Value

from dbo.SplitIntSort(@InneCzasy,';') s">
                                <SelectParameters>
                                    <asp:ControlParameter Name="InneCzasy" Type="String" ControlID="hidInneCzasy" PropertyName="Value" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <tr>
                                <td></td>
                                <td></td>
                                <td colspan="2">
                                    <div class="col-md-6">
                                        <uc1:TimeEdit ID="teTimeIn" runat="server" Right="true" Format="HH:mm" Opis="(hh:mm)" InLineCount="4" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:LinkButton ID="lnkAddInneCzasy" runat="server" OnClick="lnkAddInneCzasy_Click" CssClass="">
                                <i class="btn btn-sm btn-success"><span class="glyphicon glyphicon-plus"></span></i></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>



                        <tr>
                            <td></td>

                            <td colspan="3">

                                <asp:CheckBox ID="CheckBox2" CssClass="checkbox" runat="server" Checked='<%# Bind("ObetnijOdGory") %>' Text="Zakończ z końcem zmiany" title="Przy spóźnieniu rejestruj czas tylko do końca zmiany" />
                                <asp:CheckBox ID="VisibleCheckBox" CssClass="checkbox" runat="server" Checked='<%# Bind("Visible") %>' Text="Wybór" />
                                <asp:CheckBox ID="CheckBox1" CssClass="checkbox" runat="server" Checked='<%# Bind("Widoczna") %>' Text="Widoczna" />
                            </td>
                        </tr>

                        <tr>
                            <td></td>

                            <td>
                                <span class="label">Kolejność:</span>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="tbKolejnosc" CssClass="form-control input-sm" MaxLength="5" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true"
                                    TargetControlID="tbKolejnosc"
                                    FilterType="Custom"
                                    ValidChars="0123456789" />
                            </td>
                        </tr>
                    </table>

                    <div class="buttons">
                        <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update"><span aria-hidden="true" class="glyphicon glyphicon-floppy-disk"></span></asp:LinkButton>
                        <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel"><span aria-hidden="true" class="glyphicon glyphicon-share-alt c-red"></span></asp:LinkButton>
                    </div>



                </div>
            </div>
        </EditItemTemplate>
        <LayoutTemplate>
            <div class="zmiany_buttons">
                <asp:Button ID="InsertButton" runat="server" class="btn btn-success" Text="Nowa zmiana" CommandName="NewRecord" />
            </div>
            <div id="itemPlaceholderContainer" runat="server" style="" class="zmiany">
                <div id="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
    </asp:ListView>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
SELECT 
    ISNULL(STUFF((
	select  ISNULL(';' + dbo.ToTimeHMM(s.Items),'')
		from dbo.SplitIntSort(z.InneCzasy,';') s
 for XML PATH('')
), 1, 1, ''), '') InneCzasyDisp,

    
    Z.*, ISNULL(Z.Kolejnosc, 0) as Kolejnosc0,

--LEFT(convert(varchar, Z.Od, 8),5) as CzasOd,
--LEFT(convert(varchar, Z.Do, 8),5) as CzasDo,
convert(varchar(5), Z.Od, 8) as CzasOd,
convert(varchar(5), Z.Do, 8) as CzasDo,
K.Nazwa as TypNazwa                   
FROM Zmiany Z
left join Kody K on K.Typ = 'ZMIANA.TYP' and K.Kod = Z.Typ 
ORDER BY Widoczna desc, Visible desc, Kolejnosc, TypZmiany, Symbol"
    DeleteCommand="DELETE FROM [Zmiany] WHERE [Id] = @Id"
    InsertCommand="INSERT INTO [Zmiany] ([Symbol], [Nazwa], [Od], [Do], [Stawka], [Visible], [Kolor], [Nadgodziny], [TypZmiany], [NadgodzinyDzien], [NadgodzinyNoc], Margines, ZgodaNadg, Kolejnosc, NowaLinia, Widoczna, Typ, InneCzasy, ObetnijOdGory, MarginesNadgodzin) 
                                  VALUES (@Symbol, @Nazwa, @Od, @Do, @Stawka, @Visible, @Kolor, @Nadgodziny, @TypZmiany, @NadgodzinyDzien, @NadgodzinyNoc, 0, @ZgodaNadg, @Kolejnosc, 0, @Widoczna, @Typ, @InneCzasy, @ObetnijOdGory, @MarginesNadgodzin)"
    UpdateCommand="UPDATE [Zmiany] SET [Symbol] = @Symbol, [Nazwa] = @Nazwa, [Od] = @Od, [Do] = @Do, [Stawka] = @Stawka, [Visible] = @Visible, [Kolor] = @Kolor, [Nadgodziny] = @Nadgodziny, [TypZmiany] = @TypZmiany, [NadgodzinyDzien] = @NadgodzinyDzien, [NadgodzinyNoc] = @NadgodzinyNoc,
                   ZgodaNadg = @ZgodaNadg, Kolejnosc = @Kolejnosc,  Widoczna = @Widoczna, Typ = @Typ, InneCzasy = @InneCzasy, ObetnijOdGory = @ObetnijOdGory, MarginesNadgodzin = @MarginesNadgodzin
                  WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Visible" Type="Boolean" />
        <asp:Parameter Name="Kolor" Type="String" />
        <asp:Parameter Name="Nadgodziny" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="TypZmiany" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="ZgodaNadg" Type="Boolean" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="KoniecLinii" Type="Boolean" />
        <asp:Parameter Name="Widoczna" Type="Boolean" />
        <asp:Parameter Name="InneCzasy" Type="String" />
        <asp:Parameter Name="ObetnijOdGory" Type="Boolean" />
        <asp:Parameter Name="MarginesNadgodzin" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Visible" Type="Boolean" />
        <asp:Parameter Name="Kolor" Type="String" />
        <asp:Parameter Name="Nadgodziny" Type="String" />
        <asp:Parameter Name="TypZmiany" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Margines" Type="Int32" />
        <asp:Parameter Name="ZgodaNadg" Type="Boolean" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="KoniecLinii" Type="Boolean" />
        <asp:Parameter Name="NowaLinia" Type="Boolean" />
        <asp:Parameter Name="Widoczna" Type="Boolean" />
        <asp:Parameter Name="WymiarOd" Type="Double" />
        <asp:Parameter Name="WymiarDo" Type="Double" />
        <asp:Parameter Name="InneCzasy" Type="String" />
        <asp:Parameter Name="ObetnijOdGory" Type="Boolean" />
        <asp:Parameter Name="MarginesNadgodzin" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceKody" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="SELECT [Kod], [Nazwa] FROM [Kody] WHERE (([Typ] = @Typ) AND ([Aktywny] = @Aktywny)) ORDER BY [Lp]">
    <SelectParameters>
        <asp:Parameter DefaultValue="ZMIANA.TYP" Name="Typ" Type="String" />
        <asp:Parameter DefaultValue="true" Name="Aktywny" Type="Boolean" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsMarginesNadgodzin" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select Nazwa, Kod from Kody where Typ = 'MARGINES' and Kod != -1 order by Lp     
    "></asp:SqlDataSource>
