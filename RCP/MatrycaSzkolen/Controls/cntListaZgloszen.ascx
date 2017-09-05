<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntListaZgloszen.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntListaZgloszen" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/cntKartaZgloszenie2.ascx" TagPrefix="uc1" TagName="cntKartaZgloszenie2" %>



<h3>Lista zgłoszeń</h3>

<hr />

<asp:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Id"
    DataSourceID="dsList" CssClass="table" GridLines="None" EmptyDataRowStyle-CssClass="empty">
    <PagerStyle CssClass="pagination-ys" />
    <PagerSettings Mode="NumericFirstLast" />
    <Columns>
         <asp:BoundField DataField="Temat" HeaderText="Temat" SortExpression="Temat" />
                    <%--<asp:BoundField DataField="Pracownik" HeaderText="Pracownik" SortExpression="Pracownik" />--%>
                    <asp:BoundField DataField="Organizator" HeaderText="Organizator" SortExpression="Organizator" />
                    <asp:BoundField DataField="Data" HeaderText="Data" SortExpression="Data zgłoszenia" />
                    <asp:BoundField DataField="Planowana" HeaderText="Planowana" SortExpression="Planowana data szkolenia" />
    
    <asp:TemplateField>
        <ItemTemplate>
            <asp:Button ID="ModalButton" runat="server" CssClass="btn btn-primary btn-sm" Text="Szczegóły" OnClick="ModalButton_Click" CommandArgument='<%# Eval("Id") %>' />
            <asp:Button ID="DeleteButton" runat="server" CssClass="btn btn-danger btn-sm" CommandName="Delete" Text="Usuń" />
            <%--<asp:Button ID="EditButton" runat="server" CssClass="btn bnt-primary btn-sm" CommandName="Edit" Text="Edytuj" />--%>
        </ItemTemplate>
    </asp:TemplateField>
    </Columns>


    <EmptyDataTemplate>
        <span>Brak zgłoszeń</span>
    </EmptyDataTemplate>
</asp:GridView>

      <asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                SelectCommand="
select
  z.Id
, z.Temat
/*, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') Pracownik*/
, k.Nazwisko + ' ' + k.Imie + ISNULL(' (' + k.KadryId + ')', '') Organizator
, CONVERT(varchar(10), z.DataZgloszenia, 20) [Data]
, CONVERT(varchar(10), z.DataSzkolenia, 20) [Planowana]
from msZgloszenia z
/*left join Pracownicy p on p.Id = z.IdPracownika*/
left join Pracownicy k on k.Id = z.IdPracownika

          "

          DeleteCommand="
delete from msZgloszenia where Id = @Id
delete from msZgloszeniaPracownicy where IdZgloszenia = @Id
"

          />

<uc1:cntModal runat="server" ID="cntModal" WidthType="Large">
    <ContentTemplate>
        <uc1:cntKartaZgloszenie2 runat="server" ID="cntKartaZgloszenie2" />
    </ContentTemplate>
</uc1:cntModal>
