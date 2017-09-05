<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PracUrlop.ascx.cs" Inherits="HRRcp.Controls.PracUrlop" %>

<asp:HiddenField ID="hidPracId" runat="server" />

<div class="scroller">
    <asp:GridView ID="gvUrlopy" runat="server" AllowSorting="True" CellPadding="4" 
        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
        <RowStyle BackColor="#EFF3FB" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
</div>    

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT convert(varchar(10), A.DataOd, 20) as Od, convert(varchar(10), A.DataDo, 20) as Do, K.Symbol, K.Nazwa, A.IleDni as [Ilość dni] 
FROM Absencja A
left outer join AbsencjaKody K on K.Kod = A.Kod 
WHERE ([IdPracownika] = @IdPracownika) 
    --and A.Kod in (7,19, 1000090080, 1000, 510, 511) 
    and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '7,19,1000090080,1000,510,511'), ','))
    and A.DataOd &gt;= DATEADD(YEAR, -1, dbo.boy(GETDATE()))
ORDER BY [DataOd] desc">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
