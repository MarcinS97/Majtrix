<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZmianyEditModal.ascx.cs" Inherits="HRRcp.RCP.Controls.Adm.cntZmianyEditModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Src="~/Controls/TimeEdit.ascx" TagPrefix="uc1" TagName="TimeEdit" %>



<uc1:cntModal runat="server" ID="cntModal" Title="Edycja Zmian">
    <ContentTemplate>


        <div class="row">
            <div class="col-md-6" title="Symbol zmiany">
                <uc1:dbField runat="server" ID="Symbol" Label="Symbol zmiany" CssClass="tbSymbol" Type="tb" MaxLength="4" />

            </div>
            <div class="col-md-1" title="Symbol zmiany">
                <br />
                <table class="zmiana">
                    <tr>
                        <td>
                            <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round edit" ToolTip="Symbol zmiany" Text="" BackColor='<%# GetColorNull("#000000") %>' />
                            <asp:HiddenField ID="hidColor" runat="server" />

                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-md-5">
                <br />

                <a href="#" class="btn btn-default colorpicker-element" id="cp4"><span class="fa fa-paint-brush"></span></a>
                <%--<input type="image" id="ColorImageButton" class="palette color" src="../../../images/buttons/palette.png" title="Kliknij aby zmienić kolor zmiany" 
                    onclick="jscolor.init2(this); return false;"  runat="server" />--%>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <uc1:dbField runat="server" ID="Nazwa" Label="Nazwa zmiany" Type="tb" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <uc1:dbField runat="server" ID="ddlTyp" Label="Typ zmiany" Type="ddl" DataSourceID="dsTypyZmian" ValueField="Nazwa" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <uc1:dbField runat="server" ID="ddlTypZmiany" Label="Rodzaj nadgodzin" Type="ddl" DataSourceID="dsRodzajZmian" ValueField="Nazwa" OnChanged="ddlTypZmiany_Changed" AutoPostBack="true" />
            </div>
        </div>

        <div id="dvCzasStawka" runat="server" class="row">
            <div class="col-md-4">
                <uc1:dbField runat="server" ID="ddlOd" Label="Czas od" Type="ddl" />
            </div>
            <div class="col-md-4">
                <uc1:dbField runat="server" ID="ddlDo" Label="Czas do" Type="ddl" />
            </div>
            <div class="col-md-4">
                <uc1:dbField runat="server" ID="ddlStawka" Label="Stawka" Type="ddl" />
            </div>
        </div>
        <div id="dvNadgodziny" runat="server">
            <div class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label3" runat="server" ToolTip="Stawka na nadgodzinach zwykłych" Text="Nadgodziny zwykłe" CssClass="label" ForeColor="Gray" />
                </div>
                <div class="col-md-4">
                    <uc1:dbField runat="server" ID="ddlNadgodzinyDzien" Label="" Type="ddl" />
                </div>
            </div>
            <div id="Div1" runat="server" class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                    <asp:Label ID="Label2" runat="server" ToolTip="Stawka na nadgodzinach nocnych" Text="Nadgodziny nocne" CssClass="label" ForeColor="Gray" />
                </div>

                <div class="col-md-4">
                    <uc1:dbField runat="server" ID="ddlNadgodzinyNoc" Label="" Type="ddl" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <uc1:dbField runat="server" ID="ddlMargines" Label="Margines wejścia (+/- minut)" Type="ddl" DataSourceID="dsMargines" ValueField="Nazwa" />
            </div>

            <div class="col-md-6">
                <uc1:dbField runat="server" ID="ddlMarginesNadgodzin" Label="Nadgodziny liczone od (minut)" Type="ddl" DataSourceID="dsMarginesNadgodzin" ValueField="Nazwa" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                  <uc1:dbField runat="server" ID="ddlNazwaEN" Label="ZnacznikZmiany" Type="ddl" DataSourceID="dsZmianyRPN" ValueField="Nazwa" />
            </div>
            <div class="col-md-6">
                <uc1:dbField runat="server" ID="Kolejnosc" Label="Kolejność" Type="tb" DataSourceID="dsTypyZmian" ValueField="Kolejnosc" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 dbField">
                <span class="label" title="Inne czasy trwanie zmiany">Niestandardowe czasy zmiany</span>
            </div>
        </div>
        <div class="row">

            <div class="col-md-2">
                <uc1:TimeEdit ID="teTimeIn" runat="server" Right="true" Format="HH:mm" Opis="(hh:mm)" InLineCount="4" />

            </div>
            <div class="col-md-10">
                <asp:LinkButton ID="lnkAddInneCzasy" runat="server" OnClick="lnkAddInneCzasy_Click" CssClass="">
                 <i class="btn btn-sm btn-success"><span class="glyphicon glyphicon-plus"></span></i></asp:LinkButton>
                <asp:Repeater ID="rpZmiany" runat="server" DataSourceID="dsZmianyRep">
                    <ItemTemplate>

                        <asp:Label ID="Label6" Text='<%# Eval("Name") %>' runat="server" />

                        <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CssClass=""
                            CommandArgument='<%# Eval("Value").ToString() + ";" + Container.ItemIndex.ToString()  %>'>
                                <i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton>

                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <uc1:dbField runat="server" ID="Widoczna" Label="Widoczna" Type="check" />
            </div>
            <div class="col-md-4">
                <uc1:dbField runat="server" ID="Visible" Label="Wybór" Type="check" />
            </div>
            <div class="col-md-4">
                <uc1:dbField runat="server" ID="ObetnijOdGory" Label="Obcinaj od góry" Type="check" />
            </div>
        </div>


    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" OnClick="btnDelete_Click" Text="Usuń zmianę" />
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click" Text="Zapisz" />
    </FooterTemplate>

</uc1:cntModal>


<asp:HiddenField ID="hidInneCzasy" runat="server" Visible="false" Value="3" />
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



<asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
SELECT 
    convert(varchar(5), Z.Od, 8) as Od,
convert(varchar(5), Z.Do, 8) as Do,
    Z.*, ISNULL(Z.Kolejnosc, 0) as Kolejnosc0,


K.Nazwa as TypNazwa                   
FROM Zmiany Z
left join Kody K on K.Typ = 'ZMIANA.TYP' and K.Kod = Z.Typ
where Z.Id = {0}
ORDER BY Widoczna desc, Visible desc, Kolejnosc, TypZmiany, Symbol
"></asp:SqlDataSource>



<asp:SqlDataSource ID="dsTypyZmian" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="SELECT Kod as Typ, [Nazwa] FROM [Kody] WHERE (([Typ] = @Typ) AND ([Aktywny] = @Aktywny)) ORDER BY [Lp]">
    <SelectParameters>
        <asp:Parameter DefaultValue="ZMIANA.TYP" Name="Typ" Type="String" />
        <asp:Parameter DefaultValue="true" Name="Aktywny" Type="Boolean" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsRodzajZmian" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select Nazwa, Kod as TypZmiany from Kody where Typ = 'ZMIANA.RODZAJ' AND ([Aktywny] = 1) ORDER BY [Lp]"></asp:SqlDataSource>

<asp:SqlDataSource ID="dsZmianyRPN" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select 'brak' Nazwa, null NazwaEN, -1 Sort union select Nazwa, Nazwa2 as NazwaEN, Lp from Kody where Typ = 'ZMIANA.RPN' AND ([Aktywny] = 1) ORDER BY [Sort]"></asp:SqlDataSource>


<asp:SqlDataSource ID="dsMarginesNadgodzin" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select Nazwa, Kod as MarginesNadgodzin from Kody where Typ = 'MARGINES' order by Lp  
    "></asp:SqlDataSource>

<asp:SqlDataSource ID="dsMargines" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select Nazwa, Kod as Margines from Kody where Typ = 'MARGINES' order by Lp  
    "></asp:SqlDataSource>


<asp:SqlDataSource ID="dsUpdateInneCzasy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    UpdateCommand="
update Zmiany set InneCzasy = {0} where Id = {1}
    "></asp:SqlDataSource>

