<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntErrorsPanel.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntErrorsPanel" %>

<asp:HiddenField ID="hidEmployees" runat="server" Visible="false" />
<asp:HiddenField ID="hidDataOd" runat="server" Visible="false" />
<asp:HiddenField ID="hidDataDo" runat="server" Visible="false" />


<div id="errors" class="errors-panel side-popout xxxh420">
    <span class="popout-trigger error-panel-trigger"><i class="fa fa-exclamation-circle"></i></span>
    <div class="errors-panel-content">
        <div class="panel panel-primary panel-wrapper">
            <div class="panel-heading">
                <i class="fa fa-exclamation-circle" style="float: left; margin-right: 8px;"></i>
                <span class="errors-panel-title">Błędy</span>
            </div>
            <div style="position: relative; background-color: #fff; min-width: 350px;">
                <%--<div id="overlay">
                    <div id="circularG" class="img-load">
                        <div id="circularG_1" class="circularG"></div>
                        <div id="circularG_2" class="circularG"></div>
                        <div id="circularG_3" class="circularG"></div>
                        <div id="circularG_4" class="circularG"></div>
                        <div id="circularG_5" class="circularG"></div>
                        <div id="circularG_6" class="circularG"></div>
                        <div id="circularG_7" class="circularG"></div>
                        <div id="circularG_8" class="circularG"></div>
                    </div>
                </div>--%>
                <div class="panel-body errors-panel-body">
                </div>
                <div class="panel-footer errors-panel-footer" style="text-align: right;">
                    <asp:UpdatePanel ID="upBtn" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <a class="btn-refresh-errors btn btn-primary"><i class="fa fa-refresh" style="margin-right: 6px;"></i>Odśwież</a>
                            <asp:LinkButton ID="btnExportExcel" runat="server" CssClass="btn btn-success keep-as btn-export-errors" OnClick="btnExportExcel_Click">
                                <i class="fa fa-file-excel-o" style="margin-right: 6px;"></i>Eksport do Excela</asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                    
                </div>
            </div>
        </div>
    </div>
</div>
