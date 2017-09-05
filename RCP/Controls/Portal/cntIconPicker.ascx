<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntIconPicker.ascx.cs" Inherits="HRRcp.Controls.Portal.cntIconPicker" %>

<%--
    
STARA WERSJA - SPR CZY UŻYWANA    
    
--%>

<div id="ctIconPicker" runat="server" class="cntIconPicker">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Repeater ID="rpIcons" runat="server">
                <ItemTemplate>
                    <div class='<%# "wrapper" + GetSelectedClass(Container.DataItem.ToString())  %>'>
                        <asp:LinkButton ID="lnkIcon" runat="server" CssClass='<%# "fa " + Container.DataItem.ToString() %>' OnClick="lnkIcon_Click" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>    
</div>
