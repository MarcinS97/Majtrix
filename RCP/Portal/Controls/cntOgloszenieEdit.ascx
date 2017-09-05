<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOgloszenieEdit.ascx.cs" Inherits="HRRcp.Portal.Controls.cntOgloszenieEdit" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>

<div id="paOgloszenieEdit" runat="server" class="cntOgloszenieEdit">
    <uc1:cntModal runat="server" ID="modalOgloszenie" ShowCloseButton="false" ShowFooter="false" CssClass="modalOgloszenie" Width="880px" Keyboard="false" Backdrop="false" >
        <HeaderTemplate>
            <h4>
                <asp:Label ID="lbTitleNowe" runat="server" Text="Nowe ogłoszenie"></asp:Label>
                <asp:Label ID="lbTitleEdycja" runat="server" Text="Edycja ogłoszenia" Visible="false" ></asp:Label>
                <asp:Label ID="lbTitleResend" runat="server" Text="Wystaw ponownie" Visible="false" ></asp:Label>
            </h4>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="form-horizontal">

                <div class="form-group">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-1 step step1">
                                <div class="number" >
                                    <span>1</span>
                                </div>
                                <div class="grip">
                                </div>
                            </div>
                            <div class="col-md-1 label1">
                                <label>Kategoria:</label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlModalKategoria" runat="server" DataSourceID="dsKategorieModal" DataTextField="Kategoria" DataValueField="Id" CssClass="form-control" ></asp:DropDownList>
<%--                                <asp:RequiredFieldValidator ControlToValidate="ddlModalKategoria" ValidationGroup="vgSave" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Static" 
                                    CssClass="error" ErrorMessage="Pole wymagane" ></asp:RequiredFieldValidator>
--%>                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-1 step step2">
                                <div class="number" >
                                    <span>2</span>
                                </div>
                                <div class="grip">
                                </div>
                            </div>
                            <div class="col-md-10">
                                <label>Treść ogłoszenia (do 500 znaków)</label>
                                <asp:TextBox ID="tbTresc" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="500" Rows="5"></asp:TextBox>
<%--                                <asp:RequiredFieldValidator ControlToValidate="tbTresc" ValidationGroup="vgSave" ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                                    ErrorMessage="Pole wymagane" ></asp:RequiredFieldValidator>
--%>                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-1 step step3">
                                <div class="number">
                                    <span>3</span>
                                </div>
                                <div class="grip">
                                </div>
                            </div>
                            <div class="col-md-10">
                                <label><asp:Literal ID="ltZalacz" runat="server" Text="Załącz zdjęcie (opcjonalnie, maksymalny rozmiar: {0}MB)"></asp:Literal></label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 step">
                            </div>
                            <div class="col-md-2 image">
                                <div>
                                    <asp:Image ID="Image1" runat="server" />
                                </div>
                            </div>                        
                            <div class="col-md-7 paFileUpload">
                                <asp:HiddenField ID="hidImage" runat="server" />
                                <asp:HiddenField ID="hidImageError" runat="server" />
                                <asp:HiddenField ID="hidImageErrorMsg" runat="server" />
                                <asp:HiddenField ID="hidScript" runat="server" Visible="false" Value="
top.document.getElementById('{0}').src='{1}';       //'?' + new Date().getTime();
top.document.getElementById('{2}').value='{3}';     //hidImage
top.document.getElementById('{4}').value='{5}';     //hidError
top.document.getElementById('{6}').value='{8}';     //hidErrorMessage
top.document.getElementById('{7}').innerHTML='{8}'; //lbError
$(top.document.getElementById('{9}')).show();       //btImageDelete
//setTimeout(function() {{ top.document.getElementById('{6}').click(); }}, 250);                                  
                                    "/>
                                <cc1:AsyncFileUpload ID="AsyncFileUpload1" runat="server" 
                                    CssClass="upload"                                    
                                    ThrobberID="imgLoader"  
                                    ToolTip="Wybierz plik" 
                                    OnUploadedComplete="FileUploadComplete" 
                                    OnUploadedFileError="FileUploadError" />
                                <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../images/uploading.gif" /></asp:Label>                        
                                <asp:LinkButton ID="btImageDelete" runat="server" CssClass="btn-delete" OnClick="btImageDelete_Click" Style="display: none;" ToolTip="Usuń zdjęcie"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>       
                                <%--                                
                                <asp:LinkButton ID="btImageDelete" runat="server" CssClass="btn-delete" OnClick="btImageDelete_Click" Visible="false" ToolTip="Usuń zdjęcie"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>       
                                <asp:Button ID="btImage" runat="server" CssClass="button_postback" OnClick="btImage_Click" />                         
                                --%>
                                <br />
                                <asp:Label ID="lbImageError" runat="server" CssClass="error" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-1 step step4">
                                <div class="number">
                                    <span>4</span>
                                </div>
                                <div class="grip">
                                </div>
                            </div>
                            <div class="col-md-2 label1">
                                <label>Data zakończenia:</label>                                
                            </div>
                            <div class="col-md-3">
                                <div class="input-group">                               
                                    <uc2:DateEdit ID="deDataZakonczenia" runat="server" />
                                    <%--<asp:TextBox Placeholder="Data (yyyy-mm-dd)" runat="server" ID="tbDataZakonczenia" CssClass="form-control datepicker" />
                                    <span class="input-group-addon datepickero">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>--%>
<%--                                    <asp:RequiredFieldValidator ControlToValidate="tbDataZakonczenia" ValidationGroup="vgSave" ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"
                                        ErrorMessage="Pole wymagane" ></asp:RequiredFieldValidator>
--%>                                </div>
                            </div>
                            <div class="col-md-3">
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group" id="paStatus" runat="server" visible="false" >
                    <div class="container">
                        <row>
                            <div class="col-md-1 step step5">
                                <div class="number" >
                                    <span>5</span>
                                </div>
                                <div class="grip">
                                </div>
                            </div>
                            <div class="col-md-2 label1">
                                <label>Status:</label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlModalStatus" runat="server" DataTextField="Status" DataValueField="Id" DataSourceID="dsStatusy" CssClass="btn btn-default" ></asp:DropDownList>
<%--                                <asp:RequiredFieldValidator ControlToValidate="ddlModalStatus" ValidationGroup="vgSave" ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic" Enabled="false"
                                    ErrorMessage="Pole wymagane" ></asp:RequiredFieldValidator>
--%>                            </div>
                        </row>
                    </div>
                </div>

                <div class="form-group">                
                    <div class="col-md-1 step stepinfo">
                        <div class="number">
                            <span>!</span>
                        </div>
                    </div>                
                    <div class="col-md-10">
                        Uwaga!<br />
                        Twoje ogłoszenie zostanie zapisane i będzie opublikowane po akceptacji Moderatora.<br />
                        Po zapisaniu nie będzie możliwa edycja ogłoszenia, ale można je będzie usunąć.<br />                        
                        <div id="paRegAcc" runat="server" visible="true" class="paRegAcc">
                            <asp:CheckBox ID="cbRegAcc" runat="server" AutoPostBack="true" OnCheckedChanged="cbRegAcc_CheckedChanged"/>                                                   
                            Akceptuję warunki zamieszczania ogłoszeń określone w <a Title="Pobierz regulamin" target="_blank"
                                href="<%= UrlRegulamin %>" 
                                >regulaminie</a>.                            
                            <%--
                                ../Portal/Ogloszenia/Regulamin zamieszczania ogłoszeń przez pracowników iQora na firmowym Portalu Pracownika.pdf
                            --%>    
                            <%-- na razie się nie da - nie działa cntModal ani Tools.ShowDialog (jquery-ui)--%>
                            <asp:LinkButton ID="lbtRegulamin" runat="server" Text="Regulamin" OnClick="lbtRegulamin_Click" Visible="false"/>


                            <%--PostBackUrl="~/Portal/Ogloszenia/Regulamin zamieszczania ogłoszeń przez pracowników iQora na firmowym Portalu Pracownika.pdf">regulaminie</asp:LinkButton>.--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="buttons">
                <%--                
                <asp:Button runat="server" ID="modalAccept" CssClass="btn btn-success left" Text="Zaakceptuj"  OnClick="modalAccept_Click" />
                <asp:Button runat="server" ID="modalReject" CssClass="btn btn-danger left" Text="Odrzuć"  OnClick="modalReject_Click" />
                --%>
                <asp:Button runat="server" ID="modalDelete" CssClass="btn btn-danger left" Text="Usuń z bazy" OnClick="modalDelete_Click" />
                <asp:Button runat="server" ID="modalSave"   CssClass="btn btn-default" Text="Zapisz" OnClick="modalSave_Click" ValidationGroup="vgSave" Enabled="false" ToolTip="Zaakceptuj regulamin" />
                <asp:Button runat="server" ID="modalCancel" CssClass="btn btn-default" Text="Anuluj" CommandName="Cancel" data-dismiss="modal" />
            </div>
        </ContentTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </uc1:cntModal>
</div>

<asp:SqlDataSource ID="dsKategorieModal" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null Id, 'wybierz...' Kategoria, null Kolejnosc, 1 Sort
union all
select K.Id, K.Kategoria, K.Kolejnosc, 2 Sort
from poOgloszeniaKategorie K
where Aktywna = 1
order by Sort, Kolejnosc, Kategoria
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsOgloszenie" runat="server" 
    SelectCommand="select *, convert(varchar(10), DataZakonczenia, 20) DataZakonczeniaStr from poOgloszenia where Id = {0}"
    >
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsCheck" runat="server" 
    SelectCommand="select count(*) from poOgloszenia where IdPracownika = {0} and Status in (1,2) and DataDodania &lt; GETDATE() and dbo.getdate(GETDATE()) &lt;= ISNULL(DataZakonczenia,'20990909')" 
    >
</asp:SqlDataSource>

<%-- jest też na cntOgloszenia --%>
<asp:SqlDataSource ID="dsStatusy" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false"  
    SelectCommand="
select null Id, 'Wszystkie statusy' Status, 1 Sort
union all
select Id, Status, 2 Sort
from poOgloszeniaStatusy
where Id in (1,2,3,4)  -- bez odrzucone usuniete
order by Sort, Id
    ">
</asp:SqlDataSource>
    
