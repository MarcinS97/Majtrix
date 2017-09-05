<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAplikacjeMenu3.ascx.cs" Inherits="HRRcp.Controls.Portal.cntAplikacjeMenu3" %>

<%@ Register Src="~/Controls/cntModalPopup2.ascx" TagPrefix="uc1" TagName="cntPopup" %>

<div id="paAplikacjeMenu" runat="server" class="cntAplikacjeMenu">    
    <asp:HiddenField ID="hidMenu" runat="server" Visible="false"/>
    <%--<input type="button" ID="btnAddApp" runat="server" class="app-adder button" data-toggle="popup" popup-selector=".pp1337" visible="false" value="Nowa aplikacja" />--%>
    <asp:Button ID="btnAddApp" runat="server" Text="Nowa aplikacja" CssClass="app-adder button" Visible="false" OnClick="ShowNewApp" />
    <div>
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" 
            onitemcommand="Repeater1_ItemCommand" 
            onitemdatabound="Repeater1_ItemDataBound">
            <ItemTemplate>
              <div class="app-wrapper">
                  <asp:LinkButton ID="lbtItem" runat="server" CommandName="click" CommandArgument='<%# GetUrl(Eval("Command")) %>' >
                
                    <div id="Div1" runat="server" class="img">
                        <asp:Image ID="img" runat="server" CssClass="img"
                            ToolTip='<%# Eval("MenuText") %>' 
                            AlternateText='<%# Eval("ToolTip") %>' 
                            ImageUrl='<%# GetPath(Eval("Image")) %>' 
                        />
                    </div>
                    <div class="text">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("MenuText") %>'></asp:Label>
                    </div>
                </asp:LinkButton>
                
                <asp:LinkButton ID="lnkShowEditApp" runat="server" OnClick="ShowEditApp" CssClass="fa fa-pencil app-editor" CommandArgument='<%# Eval("Id") %>' Visible='<%# IsInEditMode() %>'  />
                <asp:LinkButton ID="lnkRemoveApp" runat="server" Text="" CssClass="fa fa-remove app-remover" Visible='<%# IsInEditMode() %>' OnClick="RemoveAppConfirm" CommandArgument='<%# Eval("Id") %>'  />
                <%--<asp:Button ID="btnRemoveApp" runat="server" CssClass="button_postback" OnClick="RemoveAppX" OnClientClick="alert('clicked');"  />--%>
            </div>
            </ItemTemplate>
        </asp:Repeater>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
            SelectCommand="SELECT * FROM [SqlMenu] WHERE ([Grupa] = @Grupa) ORDER BY [Kolejnosc]">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidMenu" Name="Grupa" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</div>

<asp:Button ID="btnRemoveApp" runat="server" OnClick="RemoveApp" CssClass="button_postback" />

<uc1:cntPopup ID="paAddApp" runat="server" Title="Dodaj aplikację" Visible="true" CssClass="pp1337">
    <ContentTemplate>
        <p>
            <asp:Label id="lblAppName" runat="server" Text="Nazwa aplikacji:" />
            <asp:TextBox ID="tbAppName" runat="server" CssClass="" />
        </p>
        <p>
            <asp:Label id="Label2" runat="server" Text="Link do aplikacji:" />
            <asp:TextBox ID="tbAppLink" runat="server" CssClass="" />
        </p>
        <p>
            <asp:Label id="Label4" runat="server" Text="Hint:" />
            <asp:TextBox ID="tbAppHint" runat="server" CssClass="" />
        </p>
        <p>
            <asp:Label id="Label5" runat="server" Text="Kolejność:" />
            <asp:TextBox ID="tbOrder" runat="server" CssClass="" />
        </p>
        <p>
            <asp:Label id="Label3" runat="server" Text="Zdjęcie:" />
            <asp:FileUpload ID="fuAppImage" runat="server"  />
            
            <%--<asp:TextBox ID="TextBox1" runat="server" CssClass="categoryDescription" />--%>
        </p>
        
        <asp:HiddenField ID="hidLastImage" runat="server" Visible="false" />
<%--        <asp:UpdatePanel ID="upIcon" runat="server" UpdateMode="Conditional" style="display: inline-block;">
            <ContentTemplate>
                <uc1:cntIconPicker runat="server" ID="cntIconPicker" />
            </ContentTemplate>
        </asp:UpdatePanel>--%>


    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnCancel" runat="server" CssClass="popupCloser button" Text="Anuluj" />
        <%--<input type="button" id="btnCancel" class="popupCloser button" value="Anuluj"  />--%>
        <asp:Button ID="btnInsertApp" runat="server" Text="Dodaj" CssClass="button" OnClick="AddApp" Visible="false"  />
        <asp:Button ID="btnSaveApp" runat="server" Text="Zapisz" CssClass="button" OnClick="EditApp" Visible="false" />
    </FooterTemplate>
</uc1:cntPopup>



<asp:SqlDataSource ID="dsInsertApp" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"  SelectCommand="
insert into SqlMenu
(Grupa, ParentId, MenuText, ToolTip, Command, Kolejnosc, Aktywny, Image, Wydruk)
values
({0}, null, {1}, {2}, {3}, {5}, 1, {4}, 1)
" />


<asp:SqlDataSource ID="dsUpdateApp" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"  SelectCommand="
update SqlMenu
set Grupa = {0}, ParentId = null, MenuText = {1}, ToolTip = {2}, Command = {3}, Kolejnosc = {5}, Aktywny = 1, Image = {4}, Wydruk = 1
where ID = {6}
" />


<asp:SqlDataSource ID="dsRemoveApp" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"  SelectCommand="
delete from SqlMenu where Id = {0}
" />



<asp:SqlDataSource ID="dsAppData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
SelectCommand="
select MenuText as AppName, ToolTip as AppHint, Command as AppLink, Image as AppImage, Kolejnosc as AppOrder from SqlMenu where Id = {0}
"

 />