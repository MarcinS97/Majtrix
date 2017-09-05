<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="HRRcp.Portal.Social.Chat" %>

<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript">
        $(document).on('ready', function () {
            $('.first-friend').click();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="pgChat" runat="server" class="pgChat">
        <asp:HiddenField ID="hidUser" runat="server" Visible="false" />
        <div class="page-title">
            Wiadomości
        </div>
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="cntChat">
                        <div class="friends-list">
                            <span class="box-title">Znajomi</span>
                            <asp:Repeater ID="rpFriends" runat="server" DataSourceID="dsFriends">
                                <ItemTemplate>
                                    
                                    <div class="friend <%# Container.ItemIndex == 0 ? "first-friend" : "" %>" data-name='<%# Eval("Pracownik") %>' data-id='<%# Eval("Id") %>'>
                                        <uc1:cntAvatar runat="server" ID="cntAvatar2" Width="40px" Height="40px" NrEw='<%# Eval("KadryId") %>' Custom="true" />
                                        <div class="friend-data">
                                            <span class="name"><%# Eval("Pracownik") %></span>
                                            <div class="last-msg">Jakaś tam ostatnia wiadomość coś tam</div>
                                            <i class="fa fa-circle online"></i>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:SqlDataSource ID="dsFriends" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
                                SelectCommand="
    select  p.*, p.Nazwisko + ' ' + p.Imie Pracownik 
    from poZnajomi z
    left join Pracownicy p on p.Id = (case when z.IdPracownika != @user then z.IdPracownika else z.IdZnajomego end)
    where (z.IdPracownika = @user or z.IdZnajomego = @user)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hidUser" PropertyName="Value" Name="user" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                            <div class="bottom-panel">
                                <asp:LinkButton ID="lnkSearchFriends" runat="server" Text="Szukaj znajomych..." OnClick="lnkSearchFriends_Click" />
                            </div>
                        </div>
                        <div class="chat-container">
                            <span class="box-title">Czat
                            </span>
                            <div id="content" class="content">
                            </div>
                            <div id="input">
                                <textarea class="ta-input" placeholder="Wpisz wiadomość..."></textarea>
                                <%--<a class="btn-small btn-primary" style="position: absolute; right: 8px; bottom: 8px; width: 40px; height: 40px; line-height: 40px;"><i style="margin: 0 !important;" class="fa fa-send"></i></a>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderWide" runat="server">
</asp:Content>
