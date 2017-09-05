<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntFieldParams.ascx.cs" Inherits="HRRcp.Controls.Reports.cntFieldParams" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/Reports/cntField.ascx" tagname="cntField" tagprefix="uc2" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<table id="tbFieldParams" runat="server" class="tbFilterFields tbFieldParams">  <%-- tbFilterFields jak modal jest to div przepina pod update panel wiec powtarzam class tbFilterFields --%>
<%--
    <uc2:cntField ID="Label1" runat="server" Mode="tr" Format="A200r" Label="Label1:" />
    <uc2:cntField ID="ToolTip1" runat="server" Mode="tr" Format="A200r" Label="ToolTip1:" />
    <uc2:cntField ID="Label2" runat="server" Mode="tr" Format="A200r" Label="Label2:" />
    <uc2:cntField ID="ToolTip2" runat="server" Mode="tr" Format="A200r" Label="ToolTip2:" />
    <uc2:cntField ID="Label3" runat="server" Mode="tr" Format="A200r" Label="Label3:" />
    <uc2:cntField ID="ToolTip3" runat="server" Mode="tr" Format="A200r" Label="ToolTip3:" />
--%>
    <tr>
        <td class="label" ><asp:Label ID="lb1" runat="server" Text="Kolumna:"></asp:Label></td>
        <td class="value">    
            <asp:DropDownList ID="Kolumna" runat="server">
                <asp:ListItem Selected="True">1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
            </asp:DropDownList>            
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="lb2" runat="server" Text="Typ:"></asp:Label></td>
        <td class="value">    
            <asp:DropDownList ID="Typ" runat="server" onselectedindexchanged="Typ_SelectedIndexChanged" AutoPostBack="true" >
                <asp:ListItem Value="1">TextBox</asp:ListItem>
                <asp:ListItem Value="2">Multiline</asp:ListItem>
                <asp:ListItem Value="3">DropDownList</asp:ListItem>
                <asp:ListItem Value="9">DropDownList - multiselect</asp:ListItem>
                <asp:ListItem Value="4">EditList</asp:ListItem>
                <asp:ListItem Value="5">Data</asp:ListItem>
                <asp:ListItem Value="6">Data od-do</asp:ListItem>
                <asp:ListItem Value="7">Tytuł</asp:ListItem>
                <asp:ListItem Value="8">CheckBox</asp:ListItem>
            </asp:DropDownList>            
        </td>
    </tr>
    
    <tr id="tr1" runat="server" visible="false">    
        <td class="label" ><asp:Label ID="lb3" runat="server" Text=""></asp:Label></td>
        <td class="value">    

        </td>
    </tr>
    
    <tr>    
        <td class="label" ><asp:Label ID="lb4" runat="server" Text="Label1:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="Label1" runat="server" />            
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="lb5" runat="server" Text="ToolTip1:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="ToolTip1" runat="server" />           
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="lb6" runat="server" Text="Label2:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="Label2" runat="server" />            
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="lb7" runat="server" Text="ToolTip2:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="ToolTip2" runat="server" />           
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="lb8" runat="server" Text="Label3:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="Label3" runat="server" />            
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="lb9" runat="server" Text="ToolTip3:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="ToolTip3" runat="server" />           
        </td>
    </tr>






    <tr id="trLookupSql" runat="server" visible="false" >    
        <td class="label" ><asp:Label ID="Label6" runat="server" Text="LookupSql:"></asp:Label></td>
        <td class="value">                
            <asp:TextBox ID="LookupSql" runat="server" TextMode="MultiLine" Rows="3" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label7" runat="server" Text="Format:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="Format" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label8" runat="server" Text="MaxLen:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="MaxLen" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label9" runat="server" Text="MinValue:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="MinValue" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label10" runat="server" Text="MaxValue:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="MaxValue" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label11" runat="server" Text="InitValue"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="InitValue" runat="server" TextMod="Mulitline" Rows="3"/>
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label12" runat="server" Text="Rodzaj:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="Rodzaj" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label13" runat="server" Text="FormId"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="FormId" runat="server" />
        </td>
    </tr>

<%--
    <tr>    
        <td class="label" ><asp:Label ID="Label14" runat="server" Text=""></asp:Label></td>
        <td class="value">    

        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label15" runat="server" Text=""></asp:Label></td>
        <td class="value">    

        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label16" runat="server" Text=""></asp:Label></td>
        <td class="value">    

        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label17" runat="server" Text=""></asp:Label></td>
        <td class="value">    

        </td>
    </tr>
--%>

    <tr>    
        <td class="label" ><asp:Label ID="Label4" runat="server" Text="RetValue1:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="RetValue1" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label18" runat="server" Text="RetValue2:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="RetValue2" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label19" runat="server" Text="NoValue"></asp:Label></td>
        <td class="value">    
            <asp:CheckBox ID="NoValue" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label20" runat="server" Text="AllowedChars:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="AllowedChars" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label21" runat="server" Text="Required:"></asp:Label></td>
        <td class="value">    
            <asp:CheckBox ID="Required" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label22" runat="server" Text="AutoRefresh:"></asp:Label></td>
        <td class="value">    
            <asp:CheckBox ID="AutoRefresh" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label23" runat="server" Text="Kolejność:"></asp:Label></td>
        <td class="value">    
            <asp:TextBox ID="Kolejnosc" runat="server" />
        </td>
    </tr>
    <tr>    
        <td class="label" ><asp:Label ID="Label24" runat="server" Text="Aktywny:"></asp:Label></td>
        <td class="value">    
            <asp:CheckBox ID="Aktywny" runat="server" />
        </td>
    </tr>
</table>




<%--


<asp:Literal ID="ltTd1c" runat="server" visible="false"></td></asp:Literal>
<asp:Literal ID="ltTd2" runat="server" visible="false"><td class="value" ></asp:Literal>

<div id="paField" runat="server" class="cntField">                
    <asp:Label ID="lbLabel1" runat="server" CssClass="label" ></asp:Label>
    
    <asp:TextBox ID="tbValue" runat="server" CssClass="textbox" Visible="false" ></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="tbValueFTB" runat="server" Enabled="false" 
        TargetControlID="tbValue" 
        FilterType="Custom" 
        ValidChars="0123456789" />

    <asp:DropDownList ID="ddlValue" runat="server" Visible="false"></asp:DropDownList>                

    <div id="paDropDownEdit" runat="server" Visible="false">
        <span id="lbSpace" runat="server" class="label" ></span>                
        <asp:DropDownList ID="ddlEditValue" runat="server" ></asp:DropDownList>
    </div>
    
    <uc1:DateEdit ID="deValue" runat="server" Visible="false"/>
    
    <div id="paDateRange" class="daterange" runat="server" Visible="false">
        <asp:Label ID="lbOd" runat="server" Text="od:" ></asp:Label>
        <uc1:DateEdit ID="deOd" runat="server" />
        <asp:Label ID="lbDo" runat="server" Text="do:" ></asp:Label>
        <uc1:DateEdit ID="deDo" runat="server" />
    </div>

    <asp:CheckBox ID="cbValue" runat="server" Visible="false"></asp:CheckBox>
    
    <asp:RequiredFieldValidator ControlToValidate="tbValue" ValidationGroup="flt" ErrorMessage="Błąd" ID="rfvValue" Enabled="false" runat="server" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
    <asp:Label ID="lbValue" runat="server" Visible="false"></asp:Label>
</div>

<asp:Literal ID="ltTd2c" runat="server" visible="false"></td></asp:Literal>

--%>