<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTransfer.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Uprawnienia.cntTransfer" %>

<asp:HiddenField ID="hidUprId" runat="server" Visible="false" />
 
<div class="cntTransfer">

    <div class="form-group">
        <asp:DropDownList ID="ddlKwalifikacje" runat="server" DataSourceID="dsKwalifikacje" DataValueField="Id" DataTextField="Name" AutoPostBack="true" CssClass="form-control" />
        <asp:SqlDataSource ID="dsKwalifikacje" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
            SelectCommand="

select null as Id, 'wybierz kwalifikację..' as Name, 0 as Sort
union all
select uk.Id, ut.TypNazwa + ' - ' + uk.Nazwa, 1 as Sort
from UprawnieniaKwalifikacje uk
left join UprawnieniaTypy ut on ut.Id = uk.Typ
order by Sort, Name

" />
    </div>

    <div class="form-group">
        <asp:DropDownList ID="ddlGrupy" runat="server" DataSourceID="dsGrupy" DataValueField="Id" DataTextField="Name" CssClass="form-control" />
        <asp:SqlDataSource ID="dsGrupy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
            SelectCommand="
select null Id, 'wybierz grupę (opcjonalne)... ' Name, 0 Sort
union all
select Id, Nazwa Name, 1 Sort 
from UprawnieniaGrupy 
where IdKwalifikacji = @kwalId
order by Sort, Name
">
            <SelectParameters>
                <asp:ControlParameter Name="kwalId" Type="Int32" ControlID="ddlKwalifikacje" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>

    </div>

    <asp:Button ID="btnTransfer" runat="server" Text="Przenieś" CssClass="btn btn-sm btn-primary" OnClick="btnTransfer_Click" />

</div>

<asp:SqlDataSource ID="dsTransfer" runat="server" 
    SelectCommand="update Uprawnienia set Typ = (select Typ from UprawnieniaKwalifikacje where Id = {1}), KwalifikacjeId = {1}, IdGrupy = {2} where Id = {0}" />