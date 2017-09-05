<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntUbezpieczenia.ascx.cs" Inherits="HRRcp.Portal.Controls.cntUbezpieczenia" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:HiddenField ID="hidTyp" runat="server" Visible="false" />
<asp:HiddenField ID="hidTyp2" runat="server" Visible="false" />

<asp:SqlDataSource ID="dsTitle" runat="server" SelectCommand="select top 1 MenuText from SqlMenu where Grupa = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />

<div id="ctUbezpieczenia" runat="server" class="cntUbezpieczenia">
    <div class="page-title">
        <asp:Literal ID="litTitle" runat="server" Text="" />
    </div>
    <%--<hr />--%>
    <div class="container wide">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="row top-cards">
                    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" OnItemCommand="Repeater1_ItemCommand">
                        <ItemTemplate>
                            <div class="col-md-4">
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass='<%# "card item item" + Eval("Par1") + " " + Eval("Par2") + " " + GetSelectedClass(Eval("Id"))  %>'
                                    CommandName="click" CommandArgument='<%# Eval("Id") %>'>
                                <h3>
                                    <i class='<%# Eval("Image") %>'></i>
                                    <%# GetText(Eval("MenuText"), Eval("Sql")) %>
                                </h3>
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <hr />
        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="pgUbezpieczeniaKafle">
                    <div id="paInfo" runat="server" class="ubInfo ub" visible="true">
                        <asp:Repeater ID="rpItems" runat="server" DataSourceID="dsItems" OnItemCommand="rpItems_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass='<%# "item item" + Eval("Par1") %>' CommandName="click" CommandArgument='<%# Eval("Id") %>'>
                                    <div class="wrapper">
                                        <asp:Label ID="Label1" runat="server" Text='<%# GetText(Eval("MenuText"), Eval("Sql")) %>'></asp:Label>
                                        <i class='<%# Eval("Image") %>'></i>
                                    </div>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Repeater1" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <uc1:cntModal runat="server" ID="modalZgoda" Keyboard="false" Backdrop="false" Title="Pytanie" ShowFooter="false" CssClass="modalZgoda">
        <ContentTemplate>
            <%--<asp:Label ID="lbTresc" runat="server" Text="Czy wyrażasz zgodę na comiesięczne potrącenie z Twojego wynagrodzenia kwoty {0} zł z tytułu ubezpieczenia."></asp:Label>--%>
            <asp:Label ID="lbTresc" runat="server" CssClass="zgoda1" Text="Czy wyrażasz zgodę na comiesięczne potrącenie z Twojego wynagrodzenia kwoty tytułu ubezpieczenia?"></asp:Label>
            <asp:Label ID="lbtresc2" runat="server" CssClass="zgoda2" Text="Jeżeli potwierdzisz, pobrany zostanie druk zgody, który po wypełnieniu prosimy dostarczyć do działu Kadr."></asp:Label>
            <div class="buttons">
                <asp:Button ID="btOk" runat="server" CssClass="btn btn-default" Text="Zgadzam się" OnClick="btOk_Click" />
                <asp:Button ID="btNot" runat="server" CssClass="btn btn-default" Text="Anuluj" OnClick="btNot_Click" />
            </div>
        </ContentTemplate>
    </uc1:cntModal>

    <uc1:cntModal runat="server" ID="modalRemind" Keyboard="false" Backdrop="false" Title="Przypomnienie" CssClass="modalRemind" CloseButtonText="Anuluj">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-xs-1 ico">
                        <span class="glyphicon glyphicon-bell ringring"></span>
                    </div>
                    <div class="col-sm-11 info">
                        <asp:Label ID="lbRemindUstaw" runat="server" CssClass="info" Text="Ustaw datę przypomnienia o wygaśnięciu innej polisy."></asp:Label>
                        <asp:Label ID="lbRemindZmien" runat="server" CssClass="info" Visible="false" Text="Czy chcesz zmienić datę przypomnienia o wygaśnięciu innej polisy?"></asp:Label>
                        <uc1:DateEdit runat="server" ID="deRemind" />
                        <asp:Label ID="lbRemind" runat="server" CssClass="info info2" Text="Przypomnienie będzie wysłane na e-mail: <b>{0}</b> 7 dni przed terminem oraz we wskazanym dniu."></asp:Label>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btRemindSave" runat="server" CssClass="btn btn-default" Text="Zapisz" OnClick="btRemindSave_Click" />
        </FooterTemplate>
    </uc1:cntModal>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:Button ID="btDownload" runat="server" CssClass="button_postback" OnClick="btDownload_Click" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btDownload" />
        </Triggers>
    </asp:UpdatePanel>
</div>

<asp:SqlDataSource ID="dsItems" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="SELECT * FROM SqlMenu where ParentId = @typ and Aktywny = 1 order by Kolejnosc">
    <SelectParameters>
        <asp:ControlParameter Name="typ" Type="Int32" ControlID="hidTyp2" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:HiddenField ID="hidPar" runat="server" Visible="false" Value="UBEZP_ZDROW|UBEZP_ZYC|UBEZP_MAJ" />
<asp:HiddenField ID="hidTitle" runat="server" Visible="false" Value="Ubezpieczenia zdrowotne|Grupowe ubezpieczenia na życie|Ubezpieczenia mieszkaniowe" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="SELECT * FROM SqlMenu where Grupa = @typ and Aktywny = 1 order by Kolejnosc">
    <SelectParameters>
        <asp:ControlParameter Name="typ" Type="String" ControlID="hidTyp" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsRemind" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="select top 1 * FROM RaportyScheduler where Typ = 50 and IdPracownika = {0}"
    DeleteCommand="delete from RaportyScheduler where Typ = 50 and IdPracownika = {0}"
    UpdateCommand="
declare @data datetime
set @data = '{1}'
update RaportyScheduler set DataStartu = @data, NextStart = case when @data &gt; dbo.getdate(GETDATE()) then @data else DATEADD(YEAR,1,@data) end 
where Typ = 50 and IdPracownika = {0}"
    InsertCommand="
declare @data datetime
set @data = '{1}'
insert into RaportyScheduler (UserId,IdPracownika,IdRaportu,Typ,DataStartu,InterwalTyp,Interwal,Status,Aktywny,NextStart) 
values ({0},{0},1,50,@data,'MM',12,1,1,case when @data &gt; dbo.getdate(GETDATE()) then @data else DATEADD(YEAR,1,@data) end)" />

<asp:SqlDataSource ID="dsSelectedItemData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="select * from SqlMenu where Id = {0}" />
