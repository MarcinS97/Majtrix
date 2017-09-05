<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test1.aspx.cs" Inherits="HRRcp.test1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/DateRange.ascx" tagname="DateRange" tagprefix="uc11" %>
<%@ Register src="~/Controls/SelectZmiana.ascx" tagname="SelectZmiana" tagprefix="uc12" %>
<%@ Register src="~/Controls/PlanPracy.ascx" tagname="PlanPracy" tagprefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>


    <link href="~/styles/master.css" rel="stylesheet" type="text/css" />
    <link href="~/styles/Controls.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="~/../scripts/jquery-1.5.1.min.js"></script>
    <script type="text/javascript" src="~/../scripts/table2CSV.js"></script>
    <script type="text/javascript" src="~/../scripts/common.js"></script>


</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <span class="t1">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownicy" runat="server" AutoPostBack="true" 
                                onselectedindexchanged="ddlKierownicy_SelectedIndexChanged">
                            </asp:DropDownList>    
                        </td>
                        <td>
                            <uc11:DateRange ID="DateRange1" runat="server" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>    
        
        
        <br />
        <br />
        <hr />
        Import osób, działów i stanowisk do PRP, zaciąga strukturę, nie wykonywać bez potrzeby!!!<br />
        <asp:Button ID="btImportKP" runat="server" onclick="btImportKP_Click" Text="Importuj KADRY" />
        <asp:Button ID="btPRP" runat="server" onclick="btPRP_Click" Text="Eksportuj do PRP" />
        <hr>
    </div>
    </form>
</body>
</html>
