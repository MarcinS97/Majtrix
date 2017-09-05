<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlMenu3.ascx.cs" Inherits="HRRcp.Controls.Portal.cntSqlMenu3" %>

<asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
<asp:HiddenField ID="hidRights" runat="server" Visible="false"/>

<script type="text/javascript">
   

</script>


<div class="cntSqlMenu3">


    <asp:Literal ID="litMenu" runat="server" />


<%--
    <ul class="sql-menu">
        <li>
            <a href="javascript://" class="toggler">Dane pracownika</a>
            <ul>
                <li><a href="javascript://">Martwy człowiek</a></li>
                <li>
                    <a href="javascript://" class="toggler">Kredens</a>
                    <ul>
                        <li>
                            <a href="javascript://" class="toggler">Śmieszne rzeczy</a>
                            <ul>
                                <li><a href="javascript://">Dobrze</a></li>
                                <li>
                                    <a href="javascript://" class="toggler">Kredens</a>
                                    <ul>
                                        <li>
                                            <a href="javascript://">Hehe</a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
        <li>
            <a href="javascript://">Kwitek płacowy</a>
        </li>
        <li>
            <a href="javascript://">Urlopy i nieobecności</a>
        </li>
    </ul>--%>

    <br />
</div>


<asp:SqlDataSource ID="dsData" runat="server" SelectCommand="
select * into #temp from SqlMenu where Grupa = '{0}'
select * from
(
select * from #temp
union
select * from SqlMenu where ParentId in (select Id from #temp)
) t
where t.Aktywny = 1
order by t.Kolejnosc
" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />