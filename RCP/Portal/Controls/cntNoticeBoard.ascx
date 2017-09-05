<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntNoticeBoard.ascx.cs" Inherits="NoticeBoard.Controls.cntNoticeBoard" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div id="paNoticeBoard" runat="server" class="cntNoticeBoard">
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false"/>


<!-- ARCHIVE MODAL -->
<uc1:cntModal runat="server" ID="archiveModal" CssClass="large">
    <HeaderTemplate>
        <h4>Archiwum ogłoszeń</h4>
    </HeaderTemplate>
    <ContentTemplate>
        <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
            <ContentTemplate>
                <div class="container">
                    <div class="row">
                        <asp:ListView ID="ListView4" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource4" >
                            <EmptyDataTemplate>
                                Brak archiwalnych ogłoszeń.
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <div class="item list-group-item col-lg-3 col-xs-3 <%# (bool)Eval("Wyroznione") ? "highlight" : "" %>">
                                    <div class="thumbnail">
                                        <img style='<%# GetNoticeFullImage((int)Eval("Id")) == "null" ? "display: none; opacity: 0.0; position: absolute;" : "" %>' data-full="<%# GetNoticeFullImage((int)Eval("Id")) %>" class="group grid-group-image" src="<%# GetNoticeImageThumbnail((int)Eval("Id")) %>" alt="" />
                                        <div class="caption">
                                            <h4 class="group inner list-group-item-heading" style='<%= ViewSwitch ? "width: 80%;" : "width: 100%;" %>'>
                                                <asp:Label Text='<%# Eval("Tytul") %>' runat="server" ID="TytulLabel" />
                                                <br />
                                                <span class="label_cena <%# Eval("Cena") == DBNull.Value ? "null": "" %>">
                                                    <asp:Label Text='<%# Eval("cena") %>' runat="server" ID="CenaLabel" />
                                                    zł</span>
                                            </h4>
                                            <p class="group inner list-group-item-text">
                                                <asp:Label Text='<%# Eval("Opis") %>' runat="server" ID="OpisLabel" />
                                            </p>
                                            <div class="row">
                                                <div class="col-xs-12 col-md-6">
                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>

                            <LayoutTemplate>
                                <div runat="server" id="itemPlaceholderContainer" style=""><span runat="server" id="itemPlaceholder" /></div>
                            </LayoutTemplate>

                        </asp:ListView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
    <FooterTemplate>

    </FooterTemplate>
</uc1:cntModal>
<!-- /ARCHIVE MODAL -->

<!-- IMAGE MODAL -->
<uc1:cntModal runat="server" ID="imageModal" CssClass="ImageModalClass">
    <HeaderTemplate>
        <h4>Podgląd zdjęcia</h4>
    </HeaderTemplate>
    <ContentTemplate>
        <img src="Content/img/sprzedamopla.jpg" style="max-width: 100%;" />
    </ContentTemplate>
    <FooterTemplate>
    </FooterTemplate>
</uc1:cntModal>
<!-- /IMAGE MODAL -->


<uc1:cntModal runat="server" ID="modalAlerts" ShowHeader="false" ShowFooter="false">
    <ContentTemplate>
        <div class="container">
            <div class="row">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <h3>Nie możesz tego zrobić !</h3>
                <h5><asp:Label runat="server" ID="alertText"></asp:Label></h5>
            </div>
        </div>
    </ContentTemplate>
</uc1:cntModal>

<uc1:cntModal runat="server" ID="modalConfirm" ShowHeader="false" ShowFooter="false">
    <ContentTemplate>
        <div class="container">
            <div class="row">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <h3>Potwierdzenie</h3>
                <h5><asp:Label runat="server" ID="Label1"></asp:Label></h5>
            </div>
        </div>
    </ContentTemplate>
</uc1:cntModal>

<uc1:cntModal runat="server" ID="cntModal" ShowCloseButton="false" ShowFooter="false">
    <HeaderTemplate>
        <h4>Tworzenie ogłoszenia</h4>
    </HeaderTemplate>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:TextBox runat="server" ID="modal_Id" visible="false" ReadOnly="true"/>
            </div>

            <div class="form-group">
                <div class="col-md-4">
                    <asp:DropDownList ID="modal_Status" runat="server" DataTextField="Nazwa" DataValueField="Id" CssClass="btn btn-default" Visible="false"></asp:DropDownList>
                </div>
            </div>

            <div class="form-group">
                <div class="container">
                    <asp:TextBox Placeholder="Tytuł ogłoszenia" runat="server" ID="modal_Tytul" CssClass="form-control" />
                </div>
            </div>

            <div class="form-group">
                <div class="container">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="input-group">
                                <asp:Label Placeholder="Data dodania" runat="server" ID="modal_DataDodania" CssClass="form-control"/>
                                <span class="input-group-addon">
                                    <span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="input-group">
                                <asp:TextBox Placeholder="Data zakończenia" runat="server" ID="modal_DataZakonczenia" CssClass="form-control" />
                                <span class="input-group-addon">
                                    <span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <div class="container">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="input-group">
                                <asp:TextBox Placeholder="Cena" runat="server" ID="modal_Cena" CssClass="form-control" />
                                <span class="input-group-addon">zł</span>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="modal_Kategoria" runat="server" DataTextField="Nazwa" DataValueField="Id" CssClass="btn btn-default"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Label ID="tooBigFile" runat="server" Text="Rozmiar pliku jest zbyt duży!" Visible="false" />
            <cc1:AsyncFileUpload ID="AsyncFileUpload1" runat="server" OnUploadedComplete="FileUploadComplete" /> <br />

            <div class="form-group">
                <div class="container">
                    <asp:TextBox ID="modal_Opis" runat="server" Wrap="True" TextMode="MultiLine" Width="100%" Rows="15"></asp:TextBox>
                </div>
            </div>

        </div>
        <asp:Button runat="server" OnClick="modalAccept_Click" CssClass="btn btn-defualt" Text="Zatwierdź" ID="modalAccept" CausesValidation="True" />&nbsp;<asp:Button runat="server" CssClass="btn btn-default" Text="Cancel" CommandName="Cancel" data-dismiss="modal" ID="InsertCancelButton" CausesValidation="False" />
    </ContentTemplate>
    <FooterTemplate>
    </FooterTemplate>
</uc1:cntModal>



<div id="body_content">
    <div class="container">
        <div class="well well-sm">
            <div class="btn-group">
                <asp:LinkButton OnClick="setViewSwitch" class="btn btn-default btn-sm" ID="list" runat="server"><span class="glyphicon glyphicon-th-list"></span></asp:LinkButton>
                <asp:LinkButton OnClick="setViewSwitch" class="btn btn-default btn-sm" ID="grid" runat="server"><span class="glyphicon glyphicon-th"></span></asp:LinkButton>
                <asp:LinkButton OnClick="addNotice_Click" class="btn btn-warning btn-sm" ID="addNotice" runat="server">Dodaj ogłoszenie</asp:LinkButton>
                <% if (assigned(lPermissions.showArchive)) { %>
                    <asp:LinkButton OnClick="archive_Click" CssClass="btn btn-default btn-sm" ID="showArchive" runat="server">Archiwum ogłoszeń</asp:LinkButton>
                <% } %>
            </div>
            <div class="btn-group left_side">
                <div class="btn-group">
                    <asp:TextBox CssClass="form-control search-control" ID="TextBoxSearch" runat="server" />
                </div>
                <asp:DropDownList ID="search_Dropdown" runat="server" DataTextField="Nazwa" DataValueField="Id" CssClass="dropdown_l btn btn-default"></asp:DropDownList>

                <button onserverClick="OnSearchButtonClick" id="srchBtn" type="submit" runat="server" class="btn btn-default"><span class="glyphicon glyphicon-search"></span></button>

            </div>
        </div>

        <!-- ADMIN NOTICES -->
        <% if (assigned(lPermissions.showQueue)) { %>
        <div class="well well-sm admin-notices" style="position: relative;">
            <span>Ogłoszenia oczekujące</span>
            <div style="position: absolute; top: 5px; right: 10px;">
                <button onclick="return false;" data-target="#admin-queue-row" data-toggle="collapse" class="btn btn-default btn-sm">Zamknij</button>
            </div>
            <div class="row collapse" id="admin-queue-row">
                <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:ListView ID="ListView3" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource3" >
                            <EmptyDataTemplate>
                                Brak ogłoszeń do zaakceptowania.
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <div class="item <%= ViewSwitch ? "list-group-item" : "grid-group-item sticky" %> col-lg-3 col-xs-3 <%# (bool)Eval("Wyroznione") ? "highlight" : "" %>">
                                    <div class="expire-info">
                                                <asp:Label ID="lblExpire" Text='<%# "wygasa: " + Eval("DataZakonczenia") %>' runat="server" />
                                            </div>
                                    
                                    <div class="thumbnail">
                                        <img style='<%# GetNoticeFullImage((int)Eval("Id")) == "null" ? "display: none; opacity: 0.0; position: absolute;" : "" %>' data-full="<%# GetNoticeFullImage((int)Eval("Id")) %>" class="group grid-group-image" src="<%# GetNoticeImageThumbnail((int)Eval("Id")) %>" alt="" />
                                        <div class="caption">
                                            <div class="group inner list-group-item-tools">
                                                <% if (assigned(lPermissions.editNotices)) { %>
                                                    <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Zaakceptuj notatkę" CssClass="glyphicon glyphicon-plus notice-nav" runat="server" OnClick="setAsActive" CommandArgument='<%# Eval("Id") %>' ID="LinkButton4" />
                                                    <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Odrzuć notatkę" CssClass="glyphicon glyphicon-remove notice-nav" runat="server" OnClick="removeNotice_Click" CommandArgument='<%# Eval("Id") %>' ID="LinkButton5" />
                                                <% } %>
                                            </div>

                                             <h4 class="group inner list-group-item-heading" style='<%= ViewSwitch ? "width: 80%;" : "width: 100%;" %>'>
                                                <asp:Label Text='<%# Eval("Tytul") %>' runat="server" ID="TytulLabel" />
                                                <br />
                                                <span class="label_cena <%# Eval("Cena") == DBNull.Value ? "null": "" %>">
                                                    <asp:Label Text='<%# Eval("cena") %>' runat="server" ID="CenaLabel" />
                                                    zł</span>
                                            </h4>
                                            <p class="group inner list-group-item-text">
                                                <asp:Label Text='<%# Eval("Opis") %>' runat="server" ID="OpisLabel" />
                                            </p>
                                            <div class="row">
                                                <div class="col-xs-12 col-md-6">
                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <div runat="server" id="itemPlaceholderContainer" style=""><span runat="server" id="itemPlaceholder" /></div>
                            </LayoutTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <% } %>
        <!-- /ADMIN NOTICES -->

        <!-- USER NOTICES -->
        <div class="well well-sm user-notices" style="position: relative;">
            <span>Moje ogłoszenia</span>
            <div style="position: absolute; top: 5px; right: 10px;">
                <button onclick="return false;" data-target="#user-notices-row" data-toggle="collapse" class="btn btn-default btn-sm">Zamknij</button></div>
            <div class="row collapse" id="user-notices-row">
                <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:ListView ID="ListView2" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource2" >
                            <EmptyDataTemplate>
                                Nie masz żadnych aktualnych ogłoszeń.
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <div class="item <%= ViewSwitch ? "list-group-item" : "grid-group-item sticky" %> col-lg-3 col-xs-3 <%# (bool)Eval("Wyroznione") ? "highlight" : "" %> <%# (int)Eval("Status") == 1 ? "unconfirmed" : "" %>">
                                    <div class="expire-info">
                                                <asp:Label ID="lblExpire" Text='<%# "wygasa: " + Eval("DataZakonczenia") %>' runat="server" />
                                            </div>
                                    
                                    <div class="thumbnail">
                                        <img style='<%# GetNoticeFullImage((int)Eval("Id")) == "null" ? "display: none; opacity: 0.0; position: absolute;" : "" %>' data-full="<%# GetNoticeFullImage((int)Eval("Id")) %>" class="group grid-group-image" src="<%# GetNoticeImageThumbnail((int)Eval("Id")) %>" alt="" />
                                        <div class="caption" style="position: relative;">
                                            <div class="group inner list-group-item-tools <%# (ViewSwitch ? "kebab-w-centrum" : "") %>">
                                                <% if (assigned(lPermissions.setSpecial)) { %>
                                                <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Oznacz jako wyróżnione" CssClass="glyphicon glyphicon-asterisk notice-nav" runat="server" OnClick="setSpecial" CommandArgument='<%# Eval("Id") %>' ID="LinkButton1" />
                                                <% } %>

                                                <% if (assigned(lPermissions.editNotices)) { %>
                                                <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Edytuj notatkę" CssClass="glyphicon glyphicon-pencil notice-nav" runat="server" OnClick="editNotice_Click" CommandArgument='<%# Eval("Id") %>' ID="LinkButton3" />
                                                <% } %>

                                                <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Usuń notatkę" CssClass="glyphicon glyphicon-remove notice-nav" runat="server" OnClick="removeNotice_Click" CommandArgument='<%# Eval("Id") %>' ID="LinkButton2" />
                                            </div>


                                            <h4 class="group inner list-group-item-heading" style='<%= ViewSwitch ? "width: 80%;" : "width: 100%;" %>'>
                                                <asp:Label Text='<%# Eval("Tytul") %>' runat="server" ID="TytulLabel" />
                                                <br />
                                                <span class="label_cena <%# Eval("Cena") == DBNull.Value ? "null": "" %>">
                                                    <asp:Label Text='<%# Eval("cena") %>' runat="server" ID="CenaLabel" />
                                                    zł</span>
                                            </h4>
                                            <p class="group inner list-group-item-text">
                                                <asp:Label Text='<%# Eval("Opis") %>' runat="server" ID="OpisLabel" />
                                            </p>
                                            <div class="row">
                                                <div class="col-xs-12 col-md-6">
                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <div runat="server" id="itemPlaceholderContainer" style=""><span runat="server" id="itemPlaceholder" /></div>
                            </LayoutTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- /USER NOTICES -->

        <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
            <ContentTemplate>
                <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" >
                    <EmptyDataTemplate>
                        <span>Nie zostały zwrócone żadne dane.</span>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <div class="item <%= ViewSwitch ? "list-group-item" : "grid-group-item sticky" %> col-lg-3 col-xs-3 <%# (bool)Eval("Wyroznione") ? "highlight" : "" %>">
                           
                                <div class="expire-info">
                                                <asp:Label ID="lblExpire" Text='<%# "wygasa: " + Eval("DataZakonczenia") %>' runat="server" />
                                    </div>
                             <div class="thumbnail">
                                <img style='<%# GetNoticeFullImage((int)Eval("Id")) == "null" ? "display: none; opacity: 0.0; position: absolute;" : "" %>' data-full="<%# GetNoticeFullImage((int)Eval("Id")) %>" class="group grid-group-image" src="<%# GetNoticeImageThumbnail((int)Eval("Id")) %>" alt="" />
                                <div class="caption">
                                    <div class="group inner list-group-item-tools">
                                        <% if (assigned(lPermissions.setSpecial))
                                            { %>
                                        <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Wyróżnij" CssClass="glyphicon glyphicon-asterisk notice-nav" runat="server" OnClick="setSpecial" CommandArgument='<%# Eval("Id") %>' ID="LinkButton1" />
                                        <% } %>

                                        <% if (assigned(lPermissions.editNotices))
                                            { %>
                                        <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Edytuj notatkę" CssClass="glyphicon glyphicon-pencil notice-nav" runat="server" OnClick="editNotice_Click" CommandArgument='<%# Eval("Id") %>' ID="LinkButton3" />
                                        <% } %>

                                        <% if (assigned(lPermissions.removeNotices))
                                            { %>
                                        <asp:LinkButton data-toggle="tooltip" data-placement="top" title="Usuń notatkę" CssClass="glyphicon glyphicon-remove notice-nav" runat="server" OnClick="removeNotice_Click" CommandArgument='<%# Eval("Id") %>' ID="LinkButton2" />
                                        <% } %>
                                    </div>

                                    <h4 class="group inner list-group-item-heading" style='<%= ViewSwitch ? "width: 80%;" : "width: 100%;" %>'>
                                        <asp:Label Text='<%# Eval("Tytul") %>' runat="server" ID="TytulLabel" />
                                        <br />
                                        <span class="label_cena <%# Eval("Cena") == DBNull.Value ? "null": "" %>">
                                            <asp:Label Text='<%# Eval("cena") %>' runat="server" ID="CenaLabel" />
                                            zł</span>
                                    </h4>
                                    <p class="group inner list-group-item-text">
                                        <asp:Label Text='<%# Eval("Opis") %>' runat="server" ID="OpisLabel" />
                                    </p>
                                    <div class="row">
                                        <div class="col-xs-12 col-md-6">
                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <div runat="server" id="itemPlaceholderContainer" style=""><span runat="server" id="itemPlaceholder" /></div>
                        <div id="PagerOriginal" style="display: none;">
                            <asp:DataPager runat="server" ID="DataPager1">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonCssClass="btn btn-default" FirstPageText="&laquo;" ButtonType="Button" ShowFirstPageButton="True" RenderNonBreakingSpacesBetweenControls="True" RenderDisabledButtonsAsLabels="False" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                    <asp:NumericPagerField NumericButtonCssClass="btn btn-default" ButtonType="Button"></asp:NumericPagerField>
                                    <asp:NextPreviousPagerField ButtonCssClass="btn btn-default" LastPageText="&raquo;" ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
            </ContentTemplate>
        </asp:UpdatePanel>


        <!-- PAGER CONTAINER -->
        <div class="row">
            <div id="PagerContainer" style="width: 100%; margin-top: 10px; margin-bottom: 25px; text-align: center;">
                <br style="clear: both;" />
            </div>
        </div>
    </div>
</div>


</div>

<%-- ogłoszenia --%>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
SELECT O.[Id], O.[Tytul], O.[Kategoria], O.[Cena], O.[Opis], U.[Id] as [IdUzytkownika], U.Nazwisko + ' ' + U.Imie as [Uzytkownik], O.[DataDodania], O.[DataZakonczenia], O.[Wyroznione], O.[Status] 
FROM [poOgloszenia] as O 
INNER JOIN [Pracownicy] as U ON U.[Id] = O.[Uzytkownik] 
where O.Status = 2 
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>

<%-- moje --%>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
SELECT O.[Id], O.[Tytul], O.[Kategoria], O.[Cena], O.[Opis], U.[Id] as [IdUzytkownika], U.Nazwisko + ' ' + U.Imie as [Uzytkownik], O.[DataDodania], O.[DataZakonczenia], O.[Wyroznione], O.[Status] 
FROM [poOgloszenia] as O 
INNER JOIN [Pracownicy] as U ON U.[Id] = O.[Uzytkownik] 
where U.Id = @userId
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>

<%-- oczekujące --%>
<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
SELECT O.[Id], O.[Tytul], O.[Kategoria], O.[Cena], O.[Opis], U.[Id] as [IdUzytkownika], U.Nazwisko + ' ' + U.Imie as [Uzytkownik], O.[DataDodania], O.[DataZakonczenia], O.[Wyroznione], O.[Status] 
FROM [poOgloszenia] as O 
INNER JOIN [Pracownicy] as U ON U.[Id] = O.[Uzytkownik] 
where O.Status = 1
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>

<%-- arch --%>
<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
SELECT O.[Id], O.[Tytul], O.[Kategoria], O.[Cena], O.[Opis], U.[Id] as [IdUzytkownika], U.Nazwisko + ' ' + U.Imie as [Uzytkownik], O.[DataDodania], O.[DataZakonczenia], O.[Wyroznione], O.[Status] 
FROM [poOgloszenia] as O 
INNER JOIN [Pracownicy] as U ON U.[Id] = O.[Uzytkownik] 
where O.Status = 4
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>




