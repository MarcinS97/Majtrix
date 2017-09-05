<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCCMiesiace2.ascx.cs" Inherits="HRRcp.Controls.cntCCMiesiace2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/PodzialLudzi/cntSelectRokMiesiac.ascx" TagPrefix="uc1" TagName="SelectRokMiesiac" %>

<div id="paCCMiesiace2" runat="server" class="cntCCMiesiace2">
    <uc1:SelectRokMiesiac runat="server" id="SelectRokMiesiac1" canBackAll="true" canNextAll="true"
        OnBackAll="SelectRokMiesiac1_BackAll"
        OnNextAll="SelectRokMiesiac1_NextAll"
        OnValueChanged="SelectRokMiesiac1_ValueChanged"/>
    <asp:HiddenField ID="HFMiesiac" runat="server" />
    <asp:HiddenField ID="HFShowAll" Value='<%# currentUser.IsAdmin?"1":"0" %>' runat="server" />
    <asp:HiddenField ID="HFUserId" Value='<%# currentUser.Id.ToString() %>' runat="server" />
    <asp:ListView ID="ListView1" DataSourceID="SqlDataSource1" runat="server" InsertItemPosition="None"
        DataKeyNames="ccId"
        OnItemInserting="ListView1_ItemInserting"
        OnItemUpdating="ListView1_ItemUpdating"
        OnItemUpdated="ListView1_ItemUpdated"
        OnDataBinding="ListView1_DataBinding"
        >
        <ItemTemplate>
            <tr class="it">
                <td class="cc">
                    <%# Eval("Nazwa") %>
                </td>
                <td class="num">
                    <%# Eval("Limit") %>
                </td>
                <td class="control" runat="server" visible='<%# canEdit %>'>
                    <asp:Button ID="EditButton" Text="Edytuj" CommandName="Edit" runat="server" />
                    <asp:Button ID="DeleteButton" Text="Usun" CommandName="Delete" runat="server" />
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td class="cc">
                    <%# Eval("Nazwa") %>
                </td>
                <td class="num">
                    <asp:TextBox ID="ETBLimit2" Text='<%# Bind("Limit") %>' CssClass="textbox" MaxLength="10" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="true" Display="Dynamic"
                        ControlToValidate="ETBLimit2"
                        ErrorMessage="Wypełnij pole"
                        ValidationGroup="evg"
                         />
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                        TargetControlID="ETBLimit2" 
                        FilterType="Custom" 
                        ValidChars="-0123456789.,"
                         />
                </td>
                <td class="control">
                    <asp:Button ID="UpdateButton" Text="Zapisz" CommandName="Update" runat="server" ValidationGroup="evg" />
                    <asp:Button ID="CancelButton" Text="Anuluj" CommandName="Cancel" runat="server" />
                </td>
            </tr>
        </EditItemTemplate>
        <InsertItemTemplate>
            <tr style="" class="iit">
                <td class="cc">
                    <asp:DropDownList ID="DDL1" DataSourceID="SqlDataSource2" DataTextField="Nazwa" DataValueField="Id" AppendDataBoundItems="true" runat="server" >
                        <asp:ListItem Text="Wybierz" Value="" />
                    </asp:DropDownList>
                </td>
                <td class="num" >
                    <asp:TextBox ID="ETBLimit" Text='<%# Bind("Limit") %>' CssClass="textbox" MaxLength="10" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="true" Display="Dynamic"
                        ControlToValidate="ETBLimit"
                        ErrorMessage="Wypełnij pole"
                        ValidationGroup="ivg"
                         />
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                        TargetControlID="ETBLimit" 
                        FilterType="Custom" 
                        ValidChars="-0123456789.,"
                         />
                </td>
                <td class="control">
                    <asp:Button ID="InsertButton" Text="Dodaj" CommandName="Insert" runat="server" ValidationGroup="ivg" />
                    <asp:Button ID="btCancelInsert" runat="server" CommandName="CancelInsert" Text="Anuluj" />
                </td>
            </tr>
        </InsertItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server">
                            <tr>
                                <th>CC</th>
                                <th>Limit [godz.]</th>
                                <th id="thCnts" class="control" runat="server">
                                    <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />
                                </th>
                            </tr>
                            <tr id="itemPlaceHolder" runat="server" />
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <EmptyDataTemplate>
            <table class="ListView1" runat="server">
                <tr runat="server">
                    <td runat="server">
                        Brak limitów na dany miesiąc
                        <div id="EDTInsert" runat="server">
                            <asp:Button ID="InsertButton" runat="server" CssClass="button200" CommandName="NewRecord" Text="Dodaj limit" />
                        </div>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
SELECT L3.id, L3.id2, L3.ccId, C.cc + ' ' + C.Nazwa as Nazwa, L3.miesiac, L3.isLast,
    (case L3.miesiac when @miesiac then L3.Limit else NULL end) as Limit from ccLimity L3
INNER JOIN CC C on C.id = L3.ccId
where (@showAll = 1 OR 
    L3.ccId IN(SELECT idCC as ccId FROM ccPrawa where UserId = @userId AND idCC != 0)) AND
    Limit IS NOT NULL AND 
    L3.id in (SELECT max(id) from ccLimity L2
                INNER JOIN (SELECT ccId, miesiac from ccLimity
                where (miesiac = @miesiac OR miesiac = DATEADD(mm,-1,@miesiac))
                GROUP BY ccId, miesiac) L1 on L1.ccId = L2.ccId AND L1.miesiac = L2.miesiac
                GROUP BY L1.ccId)
        ORDER BY C.id
        "
        InsertCommand="INSERT INTO ccLimity(ccId, miesiac, Limit) VALUES(@ccId, @miesiac, @Limit)"
        DeleteCommand="INSERT INTO ccLimity(ccId, miesiac) VALUES(@ccId, @miesiac)"
        UpdateCommand="INSERT INTO ccLimity(ccId, miesiac, Limit) VALUES(@ccId, @miesiac, @Limit)"
        >
        <SelectParameters>
            <asp:ControlParameter Name="miesiac" ControlID="HFMiesiac" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="showAll" ControlID="HFShowAll" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="userId" ControlID="HFUserId" PropertyName="Value" Type="String" />
        </SelectParameters>
        <UpdateParameters>
            <asp:ControlParameter Name="miesiac" ControlID="HFMiesiac" PropertyName="Value" Type="String" />
            <asp:Parameter Name="ccId" Type="Int32" />
            <asp:Parameter Name="Limit" Type="Double" />
        </UpdateParameters>
        <InsertParameters>
            <asp:ControlParameter Name="miesiac" ControlID="HFMiesiac" PropertyName="Value" Type="String" />
            <asp:Parameter Name="ccId" Type="Int32" />
            <asp:Parameter Name="Limit" Type="Double" />
        </InsertParameters>
        <DeleteParameters>
            <asp:ControlParameter Name="miesiac" ControlID="HFMiesiac" PropertyName="Value" Type="String" />
            <asp:Parameter Name="ccId" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
            SELECT Id, (cc + ' ' + Nazwa) as Nazwa FROM CC
            WHERE (@showAll = 1 OR
            Id IN(SELECT idCC as ccId FROM ccPrawa 
            where UserId = @userId AND idCC != 0))
            AND Id NOT IN(
            SELECT L3.ccId from ccLimity L3
            where Limit IS NOT NULL AND L3.id in (SELECT max(id) from ccLimity L2
                INNER JOIN (SELECT ccId, miesiac from ccLimity
                where (miesiac = @miesiac OR miesiac = DATEADD(mm,-1,@miesiac))
                GROUP BY ccId, miesiac) L1 on L1.ccId = L2.ccId AND L1.miesiac = L2.miesiac
                GROUP BY L1.ccId))
            ORDER BY Id
        ">
        <SelectParameters>
            <asp:ControlParameter Name="showAll" ControlID="HFShowAll" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="userId" ControlID="HFUserId" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="miesiac" ControlID="HFMiesiac" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
            <%--<asp:Button Text="asd" OnClick="Unnamed_Click" runat="server" />--%>
    
</div>    



<%--
    WITH A AS 
    (SELECT id, ccId, ROW_NUMBER() OVER(PARTITION BY ccId ORDER BY miesiac DESC) as rn
    FROM ccLimity
    where (miesiac = @miesiac OR miesiac = DATEADD(mm,-1,@miesiac)) AND isLast = 1)
    SELECT L.id, L.id2, L.ccId, CC.cc, CC.Nazwa, L.miesiac, 
    (case miesiac when @miesiac then L.Limit else NULL end) as Limit
    FROM A 
    JOIN ccLimity as L on A.id = L.id 
    LEFT JOIN CC on L.ccId = CC.id
    WHERE (@showAll = 1 OR 
    A.ccId IN(SELECT idCC as ccId FROM ccPrawa where UserId = @userId AND idCC != 0)) AND
    rn = 1 AND NOT (miesiac = @miesiac AND Limit IS NULL) AND Limit IS NOT NULL
--%>