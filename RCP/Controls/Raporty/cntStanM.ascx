<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntStanM.ascx.cs" Inherits="HRRcp.Controls.Raporty.cntStanM" %>

<div class="cntStanM">
    <asp:HiddenField ID="xxxParams" runat="server" Visible="false" />
    <asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
    <div class="row">
        <div class="col-md-12">
            <label>Stanowisko:</label>
            <asp:DropDownList ID="ddlStanM" runat="server" CssClass="form-control" DataSourceID="SqlDataSource1" DataTextField="Nazwa" DataValueField="Id"></asp:DropDownList>
        </div>
    </div>
    <asp:Button ID="Button1" runat="server" Text="Ok1" OnClick="Button1_Click" CssClass="btn btn-default"/>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null Id, 'brak' Nazwa, 1 Sort
union all
select Id, Nazwa, 2 Sort 
from Kody 
where Typ = @typ and Aktywny = 1 
order by Sort, Nazwa"
    UpdateCommand="
if @stanId = -1
    delete from PracownicyAtrybuty where Typ = @typ and IdPracownika = @pracId
else begin
    update PracownicyAtrybuty set Int1 = @stanId where Typ = @typ and IdPracownika = @pracId
    if @@ROWCOUNT = 0
        insert into PracownicyAtrybuty (Typ, IdPracownika, Od, Int1) values (@typ, @pracId, '19000101', @stanId)
end
    ">
    <SelectParameters>
        <asp:Parameter DefaultValue="STANM" Name="typ" Type="String" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter DefaultValue="STANM" Name="typ" Type="String" />
        <asp:ControlParameter ControlID="hidPracId" Name="pracId" Type="Int32"/>
        <asp:ControlParameter ControlID="ddlStanM" Name="stanId" Type="Int32"/>
    </UpdateParameters>
</asp:SqlDataSource>
