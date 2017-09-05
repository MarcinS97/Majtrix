<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSplityWsp2.ascx.cs" Inherits="HRRcp.Controls.Przypisania.cntSplityWsp2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--
<asp:HiddenField ID="hidPrevValues" runat="server" />
--%>

<asp:HiddenField ID="hidKierId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidOd" runat="server" Visible="false"/>
<asp:HiddenField ID="hidDo" runat="server" Visible="false"/>

<asp:ListView ID="lvSplityWsp" runat="server" InsertItemPosition='<%# InsertPosition %>'
    onitemcommand="lvSplityWsp_ItemCommand" 
    onitemdeleting="lvSplityWsp_ItemDeleting" 
    oniteminserting="lvSplityWsp_ItemInserting" 
    onitemdeleted="lvSplityWsp_ItemDeleted" 
    onlayoutcreated="lvSplityWsp_LayoutCreated" 
    onitemdatabound="lvSplityWsp_ItemDataBound" 
    ondatabound="lvSplityWsp_DataBound">
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>'/>
                <asp:HiddenField ID="hidIdCC" runat="server" Value='<%# Eval("IdCC") %>'/>
                <asp:Label ID="IdCCLabel" runat="server" Text='<%# Eval("cc") %>' />
            </td>
            <td id="tdWsp" runat="server" class='<%# "num" + IsLastColClass %>'>
                <asp:Label ID="lbWsp" runat="server" CssClass="sumwsp" Text='<%# Eval("Wsp") %>' Visible='<%# IsReadOnly || !GetBool(Eval("MojeCC")) %>' />
                <asp:TextBox ID="WspTextBox" runat="server" CssClass="sumwsp textbox" MaxLength="5" Text='<%# Eval("Wsp") %>' Visible='<%# !IsReadOnly && GetBool(Eval("MojeCC")) %>'/>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled='<%# !IsReadOnly %>' 
                    TargetControlID="WspTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789.," />
            </td>
            <td id="tdControl" class="control" runat="server" visible='<%# !IsReadOnly %>'>
                <%--
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                --%>
                <asp:ImageButton ID="ibtDelete" TabIndex="-1" runat="server" ImageUrl="~/images/buttons/delete.png" CommandName="Delete" AlternateText="Usuń z listy" ToolTip="Usuń z listy" Visible='<%# GetBool(Eval("MojeCC")) %>' />
            </td>
            <td id="tdKier" runat="server" >
                <asp:Label ID="lbKier" runat="server" Text='<%# Eval("KierList") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table2" runat="server" class="ListView1 tbSplityWsp hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th3" runat="server" class="cc" colspan="2">
                                <span class="left">CC</span>
                                Udział
                            </th>
                        </tr>
                        <tr runat="server">
                            <td colspan="2">
                                Brak przypisanych cc
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td>
                <asp:DropDownList ID="ddlCC" runat="server" AutoPostBack="true"
                    DataSourceID="SqlDataSource2" DataTextField="Nazwa" DataValueField="Id"
                    OnSelectedIndexChanged="ddlCC_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="num">
                <%--
                <asp:TextBox ID="WspTextBox" runat="server" />
                --%>
                <asp:Label ID="lbSum" CssClass="sum" runat="server" ></asp:Label>
            </td>
            <td class="control">
                <%--
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" />
                --%>
            </td>
            <td></td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:DropDownList ID="ddlCC" runat="server"  
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td>
                <%--
                <asp:TextBox ID="WspTextBox" runat="server" Text='<%# Bind("Wsp") %>' />
                --%>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table id="Table2" runat="server" class="ListView1 tbSplityWsp hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server" class="cc" colspan="3">
                                <span class="left">CC</span>
                                Udział <b>(0-1)</b>
                            </th>
                            <th>
                                Odpowiedzialny
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
--declare @od datetime
--declare @do datetime
--set @od = '20141101'
--set @do = '20141130'
--declare @kid int 
--set @kid = -99

select 0 as Sort, null as Id, 'wybierz ...' as Nazwa 
union all
SELECT 1 as Sort, 
--convert(varchar, CC.Id) + '|' + ISNULL(dbo.fn_GetccPrawaKierList(CC.Id, 1, ','), 'brak !!!'), 
convert(varchar, CC.Id) + '|', 
CC.cc + ' - ' + CC.Nazwa as Nazwa 
FROM CC 
where @od &lt;= ISNULL(CC.AktywneDo, GETDATE()) and CC.AktywneOd &lt;= @do and CC.Wybor = 1
and (@kid = -99 or CC.Id in (select IdCC from ccPrawa where UserId = @kid))
and CC.Surplus = 0 
ORDER BY Sort, Nazwa
">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidKierId" Name="kid" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidOd" Name="od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidDo" Name="do" PropertyName="Value" Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
