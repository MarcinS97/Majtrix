<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepUrlop.ascx.cs" Inherits="HRRcp.Controls.RepUrlop" %>
<%@ Register src="PathControl.ascx" tagname="PathControl" tagprefix="uc1" %>
<%@ Register src="zoomUrlopy.ascx" tagname="zoomUrlopy" tagprefix="uc1" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidKierId" runat="server" />

<uc1:PathControl ID="cntPath" OnSelectPath="OnSelectPath" runat="server" />
<asp:ListView ID="lvUrlopy" runat="server" DataSourceID="SqlDatasource1" DataKeyNames="Id"
    ondatabound="lv_DataBound" 
    onitemdatabound="lv_ItemDataBound" 
    onunload="lv_Unload" 
    ondatabinding="lv_DataBinding" 
    onitemcreated="lv_ItemCreated" 
    onlayoutcreated="lv_LayoutCreated" 
    onitemcommand="lv_ItemCommand" 
    onprerender="lv_PreRender" >
    <ItemTemplate>
        <%--
        <tr class="it">
        --%>
        <tr class='<%# GetLineClass(Eval("Ja")) %>' >
            <td id="tdPracName" class="col1" runat="server">
                <asp:Label ID="PracownikLabel" runat="server" CssClass="nazwisko" Text='<%# Eval("NazwiskoImie") %>' />
                <asp:LinkButton ID="lbtPracownik" runat="server" Text='<%# Eval("NazwiskoImie") %>' CommandName="SubItems" CommandArgument='<%# Eval("Id") %>' />
            </td>
            <td id="td2" class="col2" runat="server">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("KadryId") %>' />
            </td>
            <td id="tdDzial" class="col3" runat="server">
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Dzial") %>' />
            </td>
            <td id="tdKier" class="col1" runat="server" visible="false">
                <asp:LinkButton ID="lbtKierownik" runat="server" Text='<%# Eval("KierownikNI") %>' CommandName="SubItems" CommandArgument='<%# Eval("IdKierownika") %>' />
            </td>            
            <td id="td4" class="col4 num" runat="server">
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("UrlopNom") %>' />
            </td>
            <td id="td19" class="col4 num" runat="server">
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("UrlopZaleg") %>' />
            </td>
            <td id="td20" class="col4 num" runat="server">
                <asp:Label ID="Label5" runat="server" Text='<%# Eval("UrlopWyk") %>' Visible="false"/>
                <asp:LinkButton ID="lbtUrlopWyk" runat="server" Text='<%# Eval("UrlopWyk") %>' CommandName="zoom" CommandArgument='<%# Eval("Id") %>' ></asp:LinkButton>
            </td>
            <td id="td5" class="col4 num" runat="server">
                <asp:Label ID="lbDoWyk" runat="server" />
            </td>
            <td id="td21" class="col5 num" runat="server">
                <asp:Label ID="Label6" runat="server" Text='<%# Eval("WykDoDn") %>' />
            </td>
            <td id="td3" class="col5 num" runat="server">
                <asp:Label ID="Label7" runat="server" Text='<%# Eval("NaZadanie") %>' />
            </td>
            <td id="td6" class="col5 num lastcol" runat="server">
                <asp:Label ID="lbDoWykNaDzien" runat="server" />
            </td>
            <%--
            <td id="td6" class="col4 num" runat="server">
                <asp:Label ID="Label8" runat="server" Text='<%# Eval("WykDoDn1") %>' />
            </td>
            <td id="td7" class="col4 num lastcol" runat="server">
                <asp:Label ID="Label9" runat="server" Text='<%# Eval("NaZadanie1") %>' />
            </td>
            --%>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline" id="lvOuterTable">
            <tr id="Tr1" runat="server">
                <td id="Td1" colspan="2" runat="server">
                    <table id="itemPlaceholderContainer" name="report" class="tbRepUrlopy">  <%-- runat="server" --%>
                        <tr>
                            <th class="pracname">Pracownik</th>
                            <th>Nr ew.</th>
                            <th id="thDzial" runat="server">Dział</th>
                            <th id="thKier" runat= "server" class="pracname" visible="false">Kierownik</th>
                            <th class="tooltip" title="Wymiar urlopu w danym roku">Wymiar<br />urlopu</th>                            
                            <th class="tooltip" title="Ilość dni urlopu, jaki pozostał do wykorzystania z poprzedniego roku">Zaległy</th>                            
                            <th class="tooltip" title="Ilość dni urlopu, jaki został wprowadzony do systemu">Naniesiony<br />w systemie</th>                            
                            <th class="tooltip" title="Urlop pozostały do wykorzystania w danym roku">Pozostały<br />do wybrania</th>                            
                            <th class="tooltip" title="Ilość dni urlopu wykorzystanych do dnia wskazanego w zestawieniu">Stan na dzień</th>                            
                            <th class="tooltip" title="Ilość dni urlopu na żądanie wykorzystanych do dnia wskazanego w zestawieniu">W tym<br />na żądanie</th>                            
                            <th class="tooltip lastcol" title="Urlop pozostały do wykorzystania w danym roku wg stanu na dzień wskazany w zestawieniu" >Pozostały<br />na dzień</th>                            
                            <%--
                            <th>w1</th>                            
                            <th class="lastcol">z1</th>                            
                            --%>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" >
</asp:SqlDataSource>

<uc1:zoomUrlopy ID="zoomUrlopy" runat="server" />

