<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntFriendsList.ascx.cs" Inherits="HRRcp.Portal.Controls.Social.cntFriendsList" %>
<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>

<style type="text/css">
    .cntSearchFriends { }
        .cntSearchFriends .tb-search { padding: 24px; font-size: 20px; background-color: #f6f6f6; }
        .cntSearchFriends .tb-search-wrapper { background-color: #fff; xpadding: 16px 24px; margin-top: 16px; xborder: solid 1px #eee; }
            .cntSearchFriends .tb-search-wrapper .input-group-btn .btn-search { padding: 14px 32px; }
            .cntSearchFriends .tb-search-wrapper .input-group-btn .btn-erase { padding: 14px 24px; text-align: center; }
                .cntSearchFriends .tb-search-wrapper .input-group-btn .btn-erase i { margin: 0 !important; }
        .cntSearchFriends .search-content { }

        .cntSearchFriends .search-items { margin-top: 16px; }
        .cntSearchFriends .friend-card { padding: 8px; border: solid 1px #eee; height: 190px; margin-bottom: 16px; background-color: #fff; border: solid 1px #eee; border-radius: 2px; }
            .cntSearchFriends .friend-card .avatar { width: 80px; height: 80px; background-position: center; background-size: cover; border-radius: 100%; display: inline-block; vertical-align: middle; margin-right: 4px; margin-top: 8px; }
            .cntSearchFriends .friend-card .info { margin-top: 16px; padding-left: 52px; }
            .cntSearchFriends .friend-card .name { font-size: 18px; color: #333; }
            .cntSearchFriends .friend-card .job { font-size: 14px; color: #777; }
</style>

<div class="cntSearchFriends">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:HiddenField ID="hidUser" runat="server" Visible="false" />
            <div class="search-content">
                <div id="divSearch" runat="server" class="xhidden">
                    <div class="tb-search-wrapper input-group">
                        <asp:TextBox ID="tbSearchFriend" runat="server" CssClass="form-control tb-search" Placeholder="Wyszukaj znajomego ..." />
                        <div class="input-group-btn">
                            <asp:LinkButton ID="btnClear" runat="server" OnClientClick="$('.cntSearchFriends .tb-search').val('');$('.cntSearchFriends .btn-search').click();return false;"
                                CssClass="btn btn-default btn-erase">
                                <i class="fa fa-eraser"></i>
                            </asp:LinkButton>
                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-default btn-search" Text="Wyszukaj" />
                        </div>
                    </div>
                </div>

                <asp:ListView ID="lvFriends" runat="server">
                    <LayoutTemplate>
                        <div class="row search-items">
                            <div id="itemPlaceholder" runat="server">
                                layout
                            </div>
                            <div id="divPager" runat="server" class="col-md-12">
                                <div class="pager pull-right">
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="30">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                            <asp:NumericPagerField ButtonType="Link" />
                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </div>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>

                        <div class="person-wrapper">
                            <uc1:cntAvatar runat="server" ID="cntAvatar" Width="40px" Height="40px" NrEw='<%# Eval("KadryId") %>' Custom="true" />
                            <span class="">
                                <asp:LinkButton ID="lnkProfile" runat="server" Text='<%# Eval("Pracownik") %>' OnClick="lnkProfile_Click" CommandArgument='<%# Eval("Id") %>' />
                                <%--<span class="name"><%# Eval("Pracownik") %></span>--%>


                                <asp:Button Text="Zaproś" runat="server" CssClass="btn btn-primary pull-right" Visible='<%# Mode == EMode.Search && Eval("Invited").ToString() == "0" %>' 
                                    OnClick="btnInviteFriend_Click" CommandArgument='<%# Eval("Id") %>' />

                                <asp:Button Text="Zaakceptuj" runat="server" CssClass="btn btn-primary pull-right" Visible='<%# Mode == EMode.InvitationsToMe %>' 
                                    OnClick="btnAcceptInvitation_Click" CommandArgument='<%# Eval("Id") %>' />

                            </span>
                        </div>

                        <%--<div class="col-md-3">
                            
                           <div class="friend-card">
                                <div class="col-md-2">
                                    <asp:Image ID="imgAvatar" runat="server" CssClass="avatar" ImageUrl='<%# GetAvatar(Eval("KadryId").ToString()) %>' />
                                </div>
                                <div class="col-md-10 info">
                                    <div class="name"><%# Eval("Pracownik") %></div>
                                    <div class="job"><%# Eval("Stanowisko") %></div>
                                </div>
                                <div class="col-md-12">
                                    <hr />
                                </div>
                                <asp:Button ID="btnInviteFriend" runat="server" CssClass="btn btn-default pull-right" Text="Zaproś" Visible='<%# Mode == EMode.Search %>' 
                                    OnClick="btnInviteFriend_Click" CommandArgument='<%# Eval("Id") %>' />
                                <asp:LinkButton ID="btnRemoveFriend" runat="server" CssClass="btn-small btn-danger pull-right" Visible='<%# Mode == EMode.My %>' >
                                    <i class="fa fa-close"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnSendMessage" runat="server" CssClass="btn-small btn-primary pull-right" Visible='<%# Mode == EMode.My %>' >
                                    <i class="fa fa-envelope"></i>
                                </asp:LinkButton>
                                
                                <asp:LinkButton ID="btnAcceptInvitation" runat="server" CssClass="btn-small btn-success pull-right" Visible='<%# Mode == EMode.InvitationsToMe %>' 
                                    OnClick="btnAcceptInvitation_Click" CommandArgument='<%# Eval("Id") %>' >
                                    <i class="fa fa-check"></i>
                                </asp:LinkButton>
                            </div>
                        </div>--%>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<asp:SqlDataSource ID="dsSearch" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="select p.*
    , Nazwisko + ' ' + Imie Pracownik 
    , case when z.Id is null then 0 else 1 end Invited
    from Pracownicy p
    left join poZaproszenia z on z.IdPracownika = @user and z.IdAkceptujacego = p.Id
    where p.Id != @user and 
    (
        @search is null or
        (p.Imie like '%' + @search + '%' or p.Nazwisko like '%' + @search + '%' or p.Stanowisko like '%' + @search + '%')
    )

    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="tbSearchFriend" PropertyName="Text" Name="search" Type="String" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter ControlID="hidUser" PropertyName="Value" Name="user" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsMy" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="select  p.*, p.Nazwisko + ' ' + p.Imie Pracownik 
    from poZnajomi z
    left join Pracownicy p on p.Id = (case when z.IdPracownika != @user then z.IdPracownika else z.IdZnajomego end)
    where (z.IdPracownika = @user or z.IdZnajomego = @user) and 
    ( @search is null or
    (p.Imie like '%' + @search + '%' or p.Nazwisko like '%' + @search + '%' or p.Stanowisko like '%' + @search + '%'))

    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUser" PropertyName="Value" Name="user" Type="String" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter ControlID="tbSearchFriend" PropertyName="Text" Name="search" Type="String" ConvertEmptyStringToNull="true" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsInvitationsToMe" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
    select  z.Id, p.*, p.Nazwisko + ' ' + p.Imie Pracownik
from poZaproszenia z
left join Pracownicy p on p.Id = z.IdPracownika
where (z.IdAkceptujacego = @user and Zaakceptowane = 0) and
    ( @search is null or
    (p.Imie like '%' + @search + '%' or p.Nazwisko like '%' + @search + '%' or p.Stanowisko like '%' + @search + '%'))

    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUser" PropertyName="Value" Name="user" Type="String" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter ControlID="tbSearchFriend" PropertyName="Text" Name="search" Type="String" ConvertEmptyStringToNull="true" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsInvitationsFromMe" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="select  z.Id, p.*, p.Nazwisko + ' ' + p.Imie Pracownik
from poZaproszenia z
left join Pracownicy p on p.Id = z.IdAkceptujacego
where (z.IdPracownika = @user and Zaakceptowane = 0) and
    ( @search is null or
    (p.Imie like '%' + @search + '%' or p.Nazwisko like '%' + @search + '%' or p.Stanowisko like '%' + @search + '%'))

    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUser" PropertyName="Value" Name="user" Type="String" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter ControlID="tbSearchFriend" PropertyName="Text" Name="search" Type="String" ConvertEmptyStringToNull="true" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsInviteFriend" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="insert into poZaproszenia (IdPracownika, IdAkceptujacego) values ({0}, {1})" />


<asp:SqlDataSource ID="dsAcceptInvitation" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="update poZaproszenia set Zaakceptowane = 1, DataAkceptacji = getdate() where Id = {0};
                    insert poZnajomi (IdPracownika, IdZnajomego)
                    select IdPracownika, IdAkceptujacego from poZaproszenia where Id = {0}" />
<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="insert into poZaproszenia (IdPracownika, IdAkceptujacego) values ({0}, {1})" />


