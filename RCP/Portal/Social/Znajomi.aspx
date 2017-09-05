<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Znajomi.aspx.cs" Inherits="HRRcp.Portal.Social.Znajomi" %>

<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="uc1" TagName="cntSqlTabs" %>
<%@ Register Src="~/Portal/Controls/Social/cntSearchFriends.ascx" TagPrefix="uc1" TagName="cntSearchFriends" %>
<%@ Register Src="~/Portal/Controls/Social/cntMyFriends.ascx" TagPrefix="uc1" TagName="cntMyFriends" %>
<%@ Register Src="~/Portal/Controls/Social/cntZaproszenia.ascx" TagPrefix="uc1" TagName="cntZaproszenia" %>
<%@ Register Src="~/Portal/Controls/Social/cntFriendsList.ascx" TagPrefix="uc1" TagName="cntFriendsList" %>






<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .pgZnajomi { background-color: #fff; }
        .inv-title { }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWide" runat="server">
    <div class="">
        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="page-title">
                    <i class="fa fa-users"></i>
                    Znajomi
                </div>
                <div class="container xwide pgZnajomi shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <uc1:cntSqlTabs runat="server" ID="cntSqlTabs" SQL="
select 'View1' as Value, '&lt;i class=''fa fa-search''&gt;&lt;/i&gt;Szukaj znajomych' Text, 0 Sort union all 
select 'View2' Value, '&lt;i class=''fa fa-users''&gt;&lt;/i&gt;Moi znajomi' Text, 1 Sort union all
select 'View3' Value, '&lt;i class=''fa fa-envelope''&gt;&lt;/i&gt;Zaproszenia' Text, 2 Sort 
order by Sort
"
                                OnSelectTab="cntSqlTabs_SelectTab" />
                        </div>
                    </div>
                    <asp:MultiView ID="mvViews" runat="server">
                        <asp:View ID="View1" runat="server">
                            <%--<uc1:cntSearchFriends runat="server" id="cntSearchFriends" />--%>
                            <div class="">
                                <%--<h4>Wyszukaj znajomych ...</h4>--%>
                                <%--<hr />--%>
                                <uc1:cntFriendsList runat="server" ID="cntFriendsListSearch" Mode="Search" />
                            </div>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <%--<uc1:cntMyFriends runat="server" id="cntMyFriends" />--%>
                            <uc1:cntFriendsList runat="server" ID="cntFriendsListMy" Mode="My" />
                        </asp:View>
                        <asp:View ID="View3" runat="server">
                            <span class="inv-title">Do akceptacji</span>
                            <uc1:cntFriendsList runat="server" ID="cntFriendsListInvAcc" Mode="InvitationsToMe" />
                            <span class="inv-title">Wysłane</span>
                            <uc1:cntFriendsList runat="server" ID="cntFriendsListInvSent" Mode="InvitationsFromMe" />
                        </asp:View>
                    </asp:MultiView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
