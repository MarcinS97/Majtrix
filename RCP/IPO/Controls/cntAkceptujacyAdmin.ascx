<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAkceptujacyAdmin.ascx.cs" Inherits="HRRcp.IPO.Controls.cntAkceptujacyAdmin" %>
<%@ Register src="~/IPO/Controls/cntUprawnienia.ascx" tagname="cntUprawnienia" tagprefix="uc1" %>

<div id="paUprawnienia" runat="server" class="cntUprawnienia">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
         <asp:Menu ID="IPOTabAdmin" runat="server" Orientation="Horizontal" 
                onmenuitemclick="IPOtabAdmin_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>

                </Items>
                <StaticItemTemplate>
                    <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />                    
                </StaticItemTemplate>
            </asp:Menu>

            
            <div id="IPOadmTabsContent" class="tabsContent paAdmin" style="background-color:#FFF; width: auto; ">
                <asp:MultiView ID="mvAdministracja" runat="server" ActiveViewIndex="0" >
                    
                    <asp:View ID="Template" runat="server">

                    </asp:View>
   

                </asp:MultiView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    

</div>

