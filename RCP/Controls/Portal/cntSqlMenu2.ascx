<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlMenu2.ascx.cs" Inherits="HRRcp.Controls.Portal.cntSqlMenu2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
<asp:HiddenField ID="hidRights" runat="server" Visible="false"/>
<asp:HiddenField ID="hidKatCommand" runat="server" />

<asp:Button ID="btKatSelect" runat="server" CssClass="button_postback" Text="Select" OnClick="btKatSelect_Click"/>

<asp:Label ID="lbInfo" runat="server" ></asp:Label>

<ajaxToolkit:Accordion ID="acordionMenu" runat="server" DataSourceID="SqlDataSource3"
    RequireOpenedPane="false"
    SuppressHeaderPostbacks="false"
    CssClass="mnKategorie"
    ContentCssClass="mnKategorieContent"
    HeaderSelectedCssClass="mnKategoriaSelected"
    OnItemDataBound="acordionMenu_ItemDataBound" 
    OnItemCommand="acordionMenu_ItemCommand" 
    ondatabinding="acordionMenu_DataBinding" 
    onprerender="acordionMenu_PreRender" onload="acordionMenu_Load" >    
    <HeaderTemplate>
        <asp:LinkButton ID="lbtKategoria" CssClass="button_postback" runat="server" Text="select" CommandName="Select" CommandArgument='<%# GetCommandArgument(Container.DataItem) %>' />        
        <asp:Label ID="lbCaption" runat="server" Text='<%# Eval("MenuText")%>' ></asp:Label>
    </HeaderTemplate>
    <ContentTemplate>
<%--
        <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' Visible="false"/>
        <asp:HiddenField ID="hidCmd" runat="server" Value='<%# Eval("Command") %>' Visible="false"/>
        <asp:HiddenField ID="hidPar1" runat="server" Value='<%# Eval("Par1") %>' Visible="false"/>
--%>
        <asp:ListView ID="lvPodKategorie" runat="server" DataKeyNames="Id" 
            OnItemCommand="lvPodKategorie_ItemCommand"
            OnItemDataBound="lvPodKategorie_ItemDataBound"
            OnSelectedIndexChanged="lvPodKategorie_SelectedIndexChanged">
            <ItemTemplate>
                <tr id="Tr1" class="it">
                    <td>
<%--
                        <asp:HiddenField ID="hidPar1" runat="server" Value='<%# Eval("Par1") %>' Visible="false"/>
--%>
                        <asp:LinkButton ID="lbtPodKategoria" runat="server" Text='<%# Eval("MenuText") %>' CommandName="Select" CommandArgument='<%# GetCommandArgument(Container.DataItem) %>'/>
                    </td>
                </tr>
            </ItemTemplate>
            <SelectedItemTemplate>
                <tr class="sit">
                    <td>
                        <asp:LinkButton ID="lbtPodKategoria" runat="server" Text='<%# Eval("MenuText") %>' CommandName="Deselect" CommandArgument='<%# GetCommandArgument(Container.DataItem) %>'/>
                    </td>
                </tr>
            </SelectedItemTemplate>
            <EmptyDataTemplate>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" class="mnPodKategorie table0">
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
            SelectCommand="
SELECT * FROM SqlMenu 
WHERE ParentId = @ParentId and Aktywny = 1 and (Rights is null or dbo.CheckRights(@Rights, Rights) = 1) 
ORDER BY Kolejnosc, MenuText">
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="ParentId" Type="Int32" />
                <asp:ControlParameter ControlID="hidRights" Name="Rights" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
    </ContentTemplate>
</ajaxToolkit:Accordion>

<%--
                <asp:ControlParameter ControlID="hidId" Name="ParentId" Type="int32" />
--%>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    onselected="SqlDataSource3_Selected"
    SelectCommand="
SELECT * FROM SqlMenu 
where Grupa = @Grupa and (ParentId is null or ParentId = 0) and Aktywny = 1 and (Rights is null or dbo.CheckRights(@Rights, Rights) = 1) 
order by Kolejnosc, MenuText">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" Type="string" />
        <asp:ControlParameter ControlID="hidRights" Name="Rights" Type="string" />
    </SelectParameters>
</asp:SqlDataSource>

