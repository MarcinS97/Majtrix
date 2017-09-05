<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReport2.ascx.cs" Inherits="HRRcp.Controls.Reports.cntReport3" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc1" %>
<asp:HiddenField ID="DDLIndex" runat="server" />
<asp:HiddenField ID="PracId" runat="server" />
<asp:HiddenField ID="MenuText" runat="server" />

           
    <div id="divReport" runat="server"  >
    
    <div id="DivZoom_Report" title="Urlop pracownika" style="display:none;" class="PracUrlopy">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

     <div id="divInfo2" runat="server" class="info">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>    
    <uc1:cntReportHeader ID="cntReportHeader1" runat="server" />


            <asp:GridView  ID="gvReport" runat="server"
            CssClass="GridView1"
                DataSourceID="SqlDataSource1" 
                readonly="true"
                PagerStyle-CssClass="pager"
                OnRowCommand="gvReport_RowCommand"
                PagerSettings-Mode="NumericFirstLast"
                AutoGenerateColumns="True" 
                ShowFooter="True" 
                PageSize="20" 
                ondatabinding="gvReport_DataBinding" ondatabound="gvReport_DataBound" 
                 
                 >
<Columns>
<asp:TemplateField ShowHeader="False">
            <ItemTemplate >
                <asp:Button ID="Bt_Modyfikuj" readonly="false" runat="server" CausesValidation="false" CommandName="Modyfikuj"
                    Text="Modyfikuj" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                <asp:Button ID="Bt_Usun" readonly="false" runat="server" CausesValidation="false" CommandName="Usun"
                    Text="Usuń" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>

                <EmptyDataTemplate>
                    <%= GetNoDataInfo("Brak danych")%>
                </EmptyDataTemplate>

            </asp:GridView>
            <asp:Button ID="Bt_Dodaj" runat="server" Text="Dodaj Nowe" 
                onclick="Bt_Dodaj_Click" />
            <asp:Button ID="btExecute" runat="server" Text="Wykonaj" CssClass="button_postback" onclick="btExecute_Click"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div id="paDesc" runat="server" class="report_description" visible="false">
        <asp:Literal ID="ltDesc" runat="server"></asp:Literal>
        
    </div>
    </div>
     <asp:Button  ID="Bt_Dialog_Close2" style="display:none;" runat="server" Text="Button" />
            </ContentTemplate>
        </asp:UpdatePanel>
</div>
</div>



<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false">
    <SelectParameters>
        <asp:SessionParameter DefaultValue="US" Name="lang" SessionField="LNG" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
                onrowdatabound="gvReport_RowDataBound" 
                onprerender="gvReport_PreRender" 
                ondatabound="gvReport_DataBound" 
--%>