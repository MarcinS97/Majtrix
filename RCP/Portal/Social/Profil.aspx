<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Profil.aspx.cs" Inherits="HRRcp.Portal.Social.Profil" %>

<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>
<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="uc1" TagName="cntSqlTabs" %>
<%@ Register Src="~/Portal/Controls/Social/cntAvatarEdit.ascx" TagPrefix="uc1" TagName="cntAvatarEdit" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.btn-send-msg').on('click', function () {
                var self = $(this);
                var id = self.attr('data-id');
                var name = self.attr('data-name');
                socialBar.createChat(id, name);
            });
        });

        function getEditor(retId, edId)
        {
            //console.log(1);
            var ed = tinyMCE.get(edId);
            var t = tinymce.DOM.encode(ed.getContent());
            //console.log(t);
            $('#' + retId).val(t);
            $('#' + edId).val('');
            var t = tinymce.DOM.encode(ed.getContent());
            //console.log(t);
            //console.log(2);
        }

        /* każdy postback moze czyscic mce, oprócz wywołanego z btSave - do oprogramowania
        var gretId;
        var gedId;
        function initEditorRq(sender, args) {
            getEditor(gretId, gedId);
        }

        function initEditor(retId, edId)
        {
            gretId = retId;
            gedId = edId;
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(initEditorRq);
        }

        /*
        prepareMaster();
        //hideFooter();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function BeginRequestHandler(sender, args) {
        }

        function EndRequestHandler(sender, args) {
            prepareTinyMce('.tinymce-editor');
        }
        */
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderWide" runat="server">
    <div class="pgProfile profile container wide">
        <%--<h1>Profil użytkownika</h1>--%>
        <%--<hr />--%>
        <div class="profile-header">
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:UpdatePanel ID="UpAvatar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="box box-avatar">
                            <asp:HiddenField ID="hidUser" runat="server" Visible="false" />
                            <uc1:cntAvatar runat="server" ID="cntAvatar" Width="100px" Height="100px" />
                            <%--<span class="name">Elton John</span>--%>
                            <asp:Label ID="lblName" runat="server" CssClass="name" />
                            <asp:Button ID="btnChangeAvatar" runat="server" Text="Zmień avatar" CssClass="btn btn-primary" OnClick="btnChangeAvatar_Click" />
                            <div class="info">
                                <p>
                                    <strong>Imię i nazwisko:</strong>
                                    <asp:Label ID="lblImieNazwisko" runat="server" />
                                </p>
                                <p>
                                    <strong>Email:</strong>
                                    <asp:Label ID="lblEmail" runat="server" />
                                </p>
                                <p>
                                    <strong>Nr tel:</strong>
                                    <asp:Label ID="lblPhone" runat="server" />
                                </p>
                                <p>
                                    <strong>Stanowisko:</strong>
                                    <asp:Label ID="lblStanowisko" runat="server" />
                                </p>
                            </div>
                        </div>
                            <uc1:cntAvatarEdit runat="server" id="cntAvatarEdit"/>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="box box-connections">
                    <span class="title">Znajomi</span>

                    <asp:Repeater ID="rpFriends" runat="server" DataSourceID="dsFriends">
                        <ItemTemplate>
                            <div class="friend">
                                <uc1:cntAvatar runat="server" ID="cntAvatar1" Width="40px" Height="40px" NrEw='<%# Eval("KadryId") %>' Custom="true" />
                                <span class="name"><%# Eval("Pracownik") %></span>
                                <asp:LinkButton ID="lnkSendMsg" runat="server" CssClass="btn-small btn-primary pull-right btn-send-msg" OnClick="lnkSendMsg_Click" data-name='<%# Eval("Pracownik") %>' data-id='<%# Eval("Id") %>'>
                                    <i class="fa fa-envelope"></i></asp:LinkButton>
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
                    <div class="more">
                        <asp:LinkButton ID="lnkSearchFriends" runat="server" Text="Szukaj znajomych..." OnClick="lnkSearchFriends_Click" />
                    </div>
                </div>
            </div>
            <div class="col-md-9">
                <div class="box box-content" style="padding: 0;">
                    <asp:UpdatePanel ID="upContent" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--
                                Items="O mnie|JA|Zainteresowania|ZAINT|Plany na przyszłość|PLANY"                                
                                --%>
                            <uc1:cntSqlTabs runat="server" ID="cntSqlTabs" DataTextField="Nazwa" DataValueField="Id" SQL="select Nazwa, Id from {0}..poProfileTypy where Aktywny = 1 order by Kolejnosc, Id" 
                                OnDataBound="cntSqlTabs_DataBound"
                                OnSelectTab="cntSqlTabs_SelectTab" />
                            <div class="tab-content">

                                    
<%--<img src="http://images2.fanpop.com/image/photos/9100000/kitty-kitties-9109284-500-460.jpg" />--%>
           
<%--
<img src="http://quotesideas.com/wp-content/uploads/2015/08/Autumn-leaves-background-hd.jpg" alt="Alternate Text" height="400" />
<br />
Labore voluptates qui aspernatur et ex laboriosam. Fugiat quia excepturi earum repellendus optio. Quod ea repellat explicabo. Perferendis in nobis dolorem vel animi.
Laboriosam quasi esse veniam enim qui sit enim. Deserunt officia perspiciatis aut iure in assumenda quis illum. Laborum sit id vero eveniet id nam.
Ut similique enim harum enim ut. Sit saepe ab ea natus eos. Dolore eaque voluptas vel. Aliquid odio et distinctio ullam reiciendis nam occaecati eos.
Quia et aut autem tenetur tempore doloremque quisquam quia. Magnam earum animi enim et dolor accusamus nobis. Sint autem id qui dolorem recusandae quo.
Dolorem enim soluta et deleniti est laborum porro. Eum dolor rem ut. Voluptates id tenetur adipisci sapiente officiis. Unde blanditiis id in placeat doloremque quam iure explicabo. Ratione facere reiciendis dolorum aut cum explicabo iusto. Sit qui ut officia.
Omnis laborum tenetur adipisci non dolorem cupiditate et omnis. Ullam molestiae fugit placeat quis fuga laboriosam at omnis. Earum fuga blanditiis natus iusto a aliquid dolor vitae. Earum odio commodi voluptatum.
Dolorem nisi aliquid veniam est inventore est. Maiores consequatur et dolorem. Voluptas similique corporis fugiat qui tempore. Sit adipisci maxime iure minus sunt et perferendis velit.
Sint temporibus excepturi omnis temporibus quo deleniti. Sunt similique modi et suscipit eius ea aut. Repellat quae possimus unde ut sapiente. Inventore qui quidem doloribus eaque labore. Soluta nam ut non sed reiciendis.
Similique est laboriosam adipisci cum quo. Aut quam eaque voluptatem aut sequi voluptatem. Magni vel voluptate minima molestiae tempora in molestiae sunt. Voluptatem nihil voluptatibus delectus explicabo exercitationem et at. Consequatur non iste fugiat eligendi sint quasi exercitationem eos. Illo enim et perferendis autem.
Fugiat ad fugit optio ad culpa corrupti accusamus. Id atque accusamus dolorem odio similique vero asperiores. Eius quia earum ex necessitatibus qui accusantium dolores. Qui et officia rem sed dignissimos autem. Sunt sed aut illo ducimus pariatur tempora possimus ipsum. Unde ullam nobis a et modi aut culpa.
--%>
 


                                <%--<input id="hidEditorData" type="hidden" class="hidEditorData" runat="server" />--%>
                                
                                <asp:HiddenField ID="hidEditorData" runat="server" />                                
                                <asp:Literal ID="litContent" runat="server" ></asp:Literal>
                                <asp:TextBox ID="htmlEditor" runat="server" CssClass="editor tinymce-editor" TextMode="MultiLine" Visible="false" />
                            </div>
                            <div class="tab-footer">
                                <asp:Button ID="btnEditContent" runat="server" Text="Edytuj" OnClick="btnEditContent_Click" CssClass="btn btn-success" />
                                <asp:Button ID="btSave" runat="server" Text="Zapisz" OnClick="btSave_Click" CssClass="btn btn-success" Visible="false"/>
                                <asp:Button ID="btCancel" runat="server" Text="Anuluj" OnClick="btCancel_Click" CssClass="btn btn-default" Visible="false"/>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <%--<img src="https://wallpaperscraft.com/image/grass_motion_blur_plant_87028_1920x1080.jpg" class="img-responsive" />--%>
                    <%--<img src="https://wallpaperscraft.com/image/bokeh_macro_branch_foliage_autumn_87042_1920x1080.jpg" class="img-responsive" />--%>
                </div>
            </div>

        </div>
        <%--     <div class="row">
            <div class="col-md-12">
                <div class="box" style="margin-top: 16px;">

                </div>
            </div>
        </div>--%>
    </div>

    <asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
        SelectCommand="select Imie + ' ' + Nazwisko ImieNazwisko, Stanowisko Stan, * from Pracownicy where Id = {0}" />

</asp:Content>
