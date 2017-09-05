<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWarianty.ascx.cs" Inherits="HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe.cntWarianty" %>

<asp:HiddenField ID="hidExcludedId" runat="server" Visible="false" />
<asp:HiddenField ID="hidRodzaj" runat="server" Visible="false" />

<table class="tbWarianty table table-bordered">
    <tr>
        <th colspan="2">
            <label>Bezpieczny</label>
        </th>
        <th colspan="2">
            <label>Bezpieczny Plus*</label>
        </th>
        <th rowspan="2">
            <label>Składka miesięczna łącznie (zł)</label>
        </th>
    </tr>
    <tr>
        <th>
            <label>Suma ubezpieczenia (zł)</label>
        </th>
        <th>
            <label>Składka miesięczna (zł)</label>
        </th>
        <th>
            <label>Suma ubezpieczenia (zł)</label>
        </th>
        <th>
            <label>Składka miesięczna (zł)</label>
        </th>
    </tr>



    <asp:Repeater ID="rpItems" runat="server" DataSourceID="dsItems">
        <ItemTemplate>
            <tr runat="server">
                <td>
                    <asp:HiddenField ID="hidId" runat="server" Visible="true" Value='<%# Eval("Id") %>' />
                    <label><%# Eval("Suma") %></label></td>
                <td class="tdSkladka" data-value='<%# Eval("Skladka") %>'>
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="cbSkladka" runat="server" Enabled='<%# Edit %>' /><%# Eval("Skladka") %></label>
                    </div>
                </td>
                <td>
                    <label><%# Eval("SumaPlus") %></label></td>
                <td class="tdSkladkaPlus" data-value='<%# Eval("SkladkaPlus") %>'>
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="cbSkladkaPlus" runat="server" Enabled='<%# Edit %>' />
                            <%--<input type="checkbox" value="">--%>  + <%# Eval("SkladkaPlus") %></label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                </td>
                <td>
                    <label class="sum"><%# Eval("SkladkaSum") %></label></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>


    <asp:SqlDataSource ID="dsItems" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
        SelectCommand="select *, Skladka + SkladkaPlus SkladkaSum from poWnioskiMajatkoweParametry where getdate() between Od and isnull(do, '20990909') and RodzajId = @rodzaj order by Kolejnosc" >
        <SelectParameters>
            <asp:ControlParameter Name="excluded" Type="Int32" PropertyName="Value" ControlID="hidExcludedId" DefaultValue="-1" />
            <asp:ControlParameter Name="rodzaj" Type="Int32" PropertyName="Value" ControlID="hidRodzaj" />
        </SelectParameters>
    </asp:SqlDataSource>





</table>
