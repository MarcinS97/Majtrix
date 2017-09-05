<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntLoginAs.ascx.cs" Inherits="HRRcp.Portal.Controls.cntLoginAs" %>


<asp:PlaceHolder runat="server" ID="jsLoginTest" Visible="false">
    <script type="text/javascript">
        /* przenioslem z mastera bo smietnik sie tam robil - domyslnie mozna wrzucic do common.js */
        function testLoginUser(ddl, loginUrl) {
            var a = ddl.value.split('|');
            if (a.length > 2) {
                if (a[2] == '')
                    $('#kartaRCP').val(a[1]);
                else {
                    //$('#login').val(a[1]);
                    $('#login').val(a[0]);
                    $('#pass').val(a[2]);
                }

                //alert(a[0] + ' ' + a[1] + ' ' + a[2]);

                var objForm = document.forms[0]
                if (objForm) {
                    var hid__VIEWSTATE = objForm.elements['__VIEWSTATE'];
                    var hid__EVENTARGUMENT = objForm.elements['__EVENTARGUMENT'];
                    var hid__EVENTTARGET = objForm.elements['__EVENTTARGET'];
                    if (hid__VIEWSTATE) {
                        hid__VIEWSTATE.disabled = true;
                    }
                    if (hid__EVENTARGUMENT) {
                        hid__EVENTARGUMENT.disabled = true;
                    }
                    if (hid__EVENTTARGET) {
                        hid__EVENTTARGET.disabled = true;
                    }
                }

                document.forms[0].method = 'post';
                //document.forms[0].action = '<%# ResolveUrl("~/Kiosk/Login.aspx") %>';
                document.forms[0].action = loginUrl;
                document.forms[0].submit();
                return true;
            }
        }
    </script>
</asp:PlaceHolder>

<div id="paKioskLogin" runat="server" class="paKioskLogin" visible="false" style="display: none;">
    <div class="float">
        <input type="hidden" id="login" name="login" />
        <input type="hidden" id="pass" name="pass" />
        <input type="hidden" id="kartaRCP" name="kartaRCP" />
    </div>
</div>

<asp:DropDownList ID="ddlLogin" runat="server" DataSourceID="dsLogin" AutoPostBack="true"
    DataTextField="Pracownik" DataValueField="Id" CssClass="form-control ddlLogin"
    OnSelectedIndexChanged="ddlLogin_SelectedIndexChanged"
    OnDataBound="ddlLogin_DataBound">
</asp:DropDownList>
<asp:SqlDataSource ID="dsLogin" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select 'zaloguj jako ...' as Pracownik, null as Id, 1 as Sort
union all                            
select Nazwisko + ' ' + Imie + ' | ' + ISNULL(KadryId, '-') + ' | ' + ISNULL(Login, '-') + ' | ' + ISNULL(NrKarty1, '-') + ' | ' + 
case when Kierownik = 1 then ' K' else '' end as Pracownik, 

convert(varchar, Id) + '|' + 
ISNuLL(NrKarty1 + '|', ISNULL(KadryId,'') + '|kdr123') as Id

--case when NrKarty1 is null then ISNULL(KadryId,'') + '|kdr123' 
--else NrKarty1 + '|' 
--end as Id 
,2 as Sort
from Pracownicy where Status &gt;= 0 or Admin = 1 or (Status = -2 and Kierownik = 1)
order by Sort, Pracownik
"></asp:SqlDataSource>
