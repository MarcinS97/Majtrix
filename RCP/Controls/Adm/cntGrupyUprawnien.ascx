<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntGrupyUprawnien.ascx.cs" Inherits="HRRcp.Controls.Adm.cntGrupyUprawnien" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div id="paGrupyUprawnien" runat="server" class="cntGrupyUprawnien">
            <asp:HiddenField ID="hfEditingGroup" runat="server" />
            <asp:HiddenField ID="hfNewGroup" Value="0" runat="server" />
            <asp:DropDownList ID="ddlGroups" AutoPostBack="true" OnSelectedIndexChanged="ddlGroups_SelectedIndexChanged" OnDataBound="ddlGroups_DataBound" DataTextField="Nazwa" DataSourceID="SqlDataSource1" DataValueField="Value" runat="server" ></asp:DropDownList>
            <asp:TextBox ID="tbNazwa" Visible="false" runat="server" ></asp:TextBox>
            <asp:Button ID="editButton" OnClick="editButton_Click" runat="server" Text="Edytuj" />
            <asp:Button ID="newButton" OnClick="newButton_Click" runat="server" Text="Nowa" /><br />
            <asp:Button ID="deleteButton" OnClick="deleteButton_Click" runat="server" Text="Usuń" Visible="true" />
            <asp:Button ID="saveButton" OnClick="saveButton_Click" runat="server" Text="Zapisz" Visible="false" />
            <asp:Button ID="cancelButton" OnClick="cancelButton_Click" runat="server" Text="Anuluj" Visible="false" /><br />
            
            <asp:Button ID="confirmEditButton" OnClick="confirmEditButton_Click" CssClass="hidden" runat="server" Text="" />
            <asp:Button ID="confirmDeleteButton" OnClick="confirmDeleteButton_Click" CssClass="hidden" runat="server" Text="" />

            <asp:Repeater ID="cbRepeater" runat="server">
                <ItemTemplate>
                    <asp:CheckBox ID="cbR" CssClass="check" runat="server" Enabled="false" Text="<%# ((string)Container.DataItem).Split('|')[1] %>" Checked="<%# IsChecked(Container.DataItem) %>" /><br />
                    <asp:HiddenField ID="hidRightId" Value="<%# ((string)Container.DataItem).Split('|')[0] %>" runat="server" />
                </ItemTemplate>
            </asp:Repeater>
            
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:SqlDataSource ID="dsPowiazaniZGrupa" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand =" declare @GroupId int = case when '{0}' = '' then -1 else CONVERT(int, '{0}') end
    select
      p.*
    from RightsGrupy rg
    inner join Pracownicy p on p.IdRightsGrupy = rg.Id
    where rg.Id = @GroupId"
    
    UpdateCommand="declare @GroupId int = case when '{0}' = '' then -1 else CONVERT(int, '{0}') end
declare @NewName nvarchar(50) = '{1}'

select count(*) value from
(
	select
	  Id
	from RightsGrupy
	where Id != ISNULL(@GroupId, -1) and Nazwa = @NewName
) a"

    InsertCommand="update Pracownicy set Rights = '{1}' where Id = {0}"
    >

    
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select Id, Nazwa, CONVERT(varchar(100), Id) + '|' + Rights as Value from RightsGrupy order by Nazwa"
    UpdateCommand="declare @GroupId int = {0}
    update RightsGrupy set Rights = '{1}', Nazwa = '{2}' where Id = @GroupId"
    InsertCommand="insert RightsGrupy(Rights, Nazwa) values('{0}', '{1}')" 
    DeleteCommand="update Pracownicy set IdRightsGrupy = null where IdRightsGrupy = {0}
    delete from RightsGrupy where Id = {0}" >

    <SelectParameters>
    </SelectParameters>
    <DeleteParameters>
    </DeleteParameters>
    <UpdateParameters>
    </UpdateParameters>
    <InsertParameters>

    </InsertParameters>
</asp:SqlDataSource>

        <!--<asp:Parameter Name="Email" Type="String" />
        <asp:Parameter Name="Mailing" Type="Boolean" />
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="IdStanowiska" Type="Int32" />
        <asp:Parameter Name="Admin" Type="Boolean" />
        <asp:Parameter Name="Raporty" Type="Boolean" />
        <asp:Parameter Name="Kierownik" Type="Boolean" />
        <asp:Parameter Name="KadryId" Type="String" />
        <asp:Parameter Name="RcpId" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />-->