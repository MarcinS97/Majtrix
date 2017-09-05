<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSocialBar.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSocialBar" %>
<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>


<div class="cntSocialBar social-bar xopened">
    <asp:HiddenField ID="hidUser" runat="server" Visible="false" />
    <div class="cntFriendsList friends-list">
        <div class="title">
            <%--<i class="fa fa-users"></i>--%>
            <%--Czat--%>
            Czat
        </div>
        <ul class="list-group friends">
            <asp:Repeater ID="rpFriends" runat="server" DataSourceID="dsFriends">
                <ItemTemplate>
                    <li class="list-group-item friend" data-name='<%# Eval("Pracownik") %>' data-id='<%# Eval("Id") %>'>
                        
                        <a href="javascript:">
                        <%--    <div class="avatar" style='<%# GetAvatar(Eval("KadryId").ToString()) %>'></div>--%>
                            <uc1:cntAvatar runat="server" ID="cntAvatar" NrEw="1" Width="32px" Height="32px" />
                            <%# Eval("Pracownik") %>
                            <%--<i class="fa fa-circle online"></i>--%>
                        </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

        <div id="divNoFriends" runat="server" visible="false" class="no-friends xhidden">
            <i class="fa fa-frown-o sad"></i>
            Wygląda na to, że nie masz znajomych. Dodaj kilku wchodząc w <asp:LinkButton ID="lnkFriensRedirect" runat="server" PostBackUrl="~/Portal/Social/Znajomi.aspx">ten</asp:LinkButton> link.
        </div>

    </div>
</div>



<asp:SqlDataSource ID="dsFriends" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
--declare @user int = 2278

select p.Id, p.KadryId, p.Nazwisko + ' ' + p.Imie Pracownik
from poZnajomi z
left join Pracownicy p on p.Id = (case when z.IdPracownika != @user then z.IdPracownika else z.IdZnajomego end)
where z.IdPracownika = @user or z.IdZnajomego = @user
">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUser" PropertyName="Value" Name="user" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
