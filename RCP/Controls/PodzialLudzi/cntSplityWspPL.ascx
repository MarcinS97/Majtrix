<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSplityWspPL.ascx.cs" Inherits="HRRcp.Controls.Przypisania.cntSplityWspPL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--
<asp:HiddenField ID="hidPrevValues" runat="server" />
--%>

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
                <asp:Label ID="lbWsp" runat="server" Text='<%# Eval("Wsp") %>' Visible='<%# IsReadOnly %>' />
                <asp:TextBox ID="WspTextBox" runat="server" CssClass="sumwsp textbox" MaxLength="5" Text='<%# Eval("Wsp") %>' Visible='<%# !IsReadOnly %>'/>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled='<%# !IsReadOnly %>' 
                    TargetControlID="WspTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789.," />
            </td>
            <td id="tdControl" class="control" runat="server" visible='<%# !IsReadOnly %>'>
                <%--
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                --%>
                <asp:ImageButton ID="ibtDelete" TabIndex="-1" runat="server" ImageUrl="~/images/buttons/delete.png" CommandName="Delete" AlternateText="Usuń z listy" ToolTip="Usuń z listy" />
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
    SelectCommand="select 0 as Sort, null as Id, 'wybierz ...' as Nazwa union
                   SELECT 1 as Sort, Id, cc + ' - ' + Nazwa as Nazwa FROM CC where ISNULL(AktywneDo, GETDATE()) &gt;= GETDATE() 
                   ORDER BY Sort, Nazwa">
</asp:SqlDataSource>
