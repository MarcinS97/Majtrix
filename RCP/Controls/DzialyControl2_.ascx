<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DzialyControl2_.ascx.cs" Inherits="HRRcp.Controls.DzialyControl2" %>

<%@ Register src="Dzialy.ascx" tagname="Dzialy" tagprefix="uc1" %>

<%@ Register src="DzialyStrefy.ascx" tagname="DzialyStrefy" tagprefix="uc2" %>

<table class="DzialyControl">
    <tr>
        <th class="dzialy">
            <span class="t5">Działy</span>
        </th>
        <th class="strefy">
            <span class="t5">Strefy i algorytmy</span>           
        </th>
    </tr>
    <tr>
        <td class="dzialy">
            <uc1:Dzialy ID="cntDzialy" DetailsList="cntDzialyStrefy" runat="server" />
        </td>
        <td class="strefy">
            <uc2:DzialyStrefy ID="cntDzialyStrefy" runat="server" />
        </td>
    </tr>
</table>