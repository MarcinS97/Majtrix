<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlMenuEditGrupyChld.ascx.cs" Inherits="HRRcp.Portal.Controls.Adm.cntSqlMenuEditGrupyChld" %>
<%@ Register Src="~/Portal/Controls/Adm/cntSqlMenuEditGrupy.ascx" TagPrefix="uc1" TagName="cntSqlMenuEditGrupy" %>
<%@ Register Src="~/Portal/Controls/Adm/cntSqlMenuEdit.ascx" TagPrefix="uc1" TagName="cntSqlMenuEdit" %>


<div class="cntSqlMenuEditGrupyChld">
    <uc1:cntSqlMenuEditGrupy runat="server" id="cntSqlMenuEditGrupy" OnShowChildren="cntSqlMenuEditGrupy_ShowChildren" OnEdit="cntSqlMenuEditGrupy_Edit" />
    <uc1:cntSqlMenuEditGrupy runat="server" id="cntSqlMenuEditGrupy1" OnShowChildren="cntSqlMenuEditGrupy1_ShowChildren" OnEdit="cntSqlMenuEditGrupy_Edit" />
    <uc1:cntSqlMenuEditGrupy runat="server" id="cntSqlMenuEditGrupy2" OnShowChildren="cntSqlMenuEditGrupy2_ShowChildren" OnEdit="cntSqlMenuEditGrupy_Edit" />
    <uc1:cntSqlMenuEdit runat="server" ID="cntSqlMenuEdit" OnSave="cntSqlMenuEdit_Save" />
   
     
</div>