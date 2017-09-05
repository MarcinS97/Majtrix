<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAdmPola.ascx.cs" Inherits="KDR.Controls.Uprawnienia.cntAdmPola" %>

<asp:HiddenField ID="hidUprId" runat="server" Visible="false" />

<div class="cntAdmPola">
    <asp:Repeater ID="rpFields" runat="server">
        <ItemTemplate>
            <div id="itemWrapper" runat="server" visible='<%# Convert.ToBoolean(Eval("Active")) %>' class="row">
                <div class="description">
                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                </div>
                <div class="check">
                    <asp:HiddenField ID="hidNo" runat="server" Visible="false" Value='<%# Eval("No") %>' />
                    <asp:CheckBox ID="cbChecked" runat="server" OnCheckedChanged="cbCheked_CheckedChanged" Checked='<%# Eval("Checked") %>' AutoPostBack="true" Enabled="<%# Editable %>" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    
    <div id="divImport" runat="server">
    <hr />
        <asp:Label ID="lblImport" runat="server" Text="Ustaw na podstawie:" />
        <asp:DropDownList ID="ddlImport" runat="server" CssClass="form-control input-xsm" DataSourceID="dsImportList" DataValueField="Id" DataTextField="Name" OnSelectedIndexChanged="ddlImport_SelectedIndexChanged" AutoPostBack="true" Style="width: 120px;" />
        <asp:SqlDataSource ID="dsImportList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort 
union all 
select u.Id, uk.Nazwa + ' - ' + u.Nazwa as Name, 1 as Sort 
from Uprawnienia u
left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId
where u.Typ = (select top 1 Typ from Uprawnienia where Id = @uprId) and u.Id != @uprId order by Sort, Name
">
            <SelectParameters>
                <asp:ControlParameter Name="uprId" Type="Int32" ControlID="hidUprId" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <asp:SqlDataSource ID="dsImport" runat="server" SelectCommand="update Uprawnienia set Pola = (select Pola from Uprawnienia where Id = {0}) where Id = {1}" />
    <asp:SqlDataSource ID="dsSave" runat="server" SelectCommand="update Uprawnienia set Pola = {1} where Id = {0} " />
    <asp:SqlDataSource ID="dsSelectFields" runat="server" SelectCommand="select Pola from Uprawnienia where Id = {0}" />

</div>