<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeEdit2.ascx.cs" Inherits="HRRcp.Controls.TimeEdit2" %>

<div class="timeeditbox">
    <div id="timeeditbox" runat="server" class="timeeditbox2">    
        <div class="timeedit">
            <asp:TextBox ID="tbTime" CssClass="textbox timeedit" runat="server" Width="60px" MaxLength="5">
        </asp:TextBox></div><div id="tbTimeList" runat="server" class="timeeditlist" >
            <asp:PlaceHolder ID="phTimeListNav" runat="server"></asp:PlaceHolder>
            <asp:PlaceHolder ID="phTimeListHist" runat="server"></asp:PlaceHolder>
            <div id="list1" runat="server" class="timelist1">
                <asp:PlaceHolder ID="phTimeListNav1" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="phTimeList1" runat="server"></asp:PlaceHolder>
            </div>    
            <div id="list2" runat="server" class="timelist2" style="display: none;">
                <asp:PlaceHolder ID="phTimeListNav2" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="phTimeList2" runat="server"></asp:PlaceHolder>
            </div>
            <div id="list3" runat="server" class="timelist3" style="display: none;">
                <asp:PlaceHolder ID="phTimeListNav3" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="phTimeList3" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    <asp:Label ID="lbOpis" CssClass="t4n" runat="server" ></asp:Label>
    <asp:Label ID="lbError" CssClass="t4n error" Visible="false" runat="server" Text="Błąd!"></asp:Label>
</div>
