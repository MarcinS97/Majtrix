<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="testPracownikCzasControl2.ascx.cs" Inherits="HRRcp.Controls.testPracownikCzasControl2" %>

<asp:HiddenField ID="hidRcpId" runat="server" />
<asp:HiddenField ID="hidRogersA" runat="server" />
<asp:HiddenField ID="hidRogersC" runat="server" />

<asp:ListView ID="ListView1" runat="server" 
    DataKeyNames="ECUniqueID" 
    DataSourceID="SqlDataSource1">
    <ItemTemplate>
        <tr style="" class="it">
            <td>
                <asp:Label ID="ECUserIdLabel" runat="server" Text='<%# Eval("ECUserId") %>' />
            </td>
            <td>
                <asp:Label ID="TimeInLabel" runat="server" Text='<%# Eval("TimeIn") %>' />
            </td>
            <td>
                <asp:Label ID="TimeOutLabel" runat="server" Text='<%# Eval("TimeOut") %>' />
            </td>
            <td>
                <asp:Label ID="CzasPracyLabel" runat="server" Text='<%# Eval("CzasPracy") %>' />
            </td>
            <td>
                <asp:Label ID="CzasPracy1Label" runat="server" Text='<%# Eval("CzasPracy1") %>' />
            </td>
            <td>
                <asp:Label ID="ReaderInLabel" runat="server" Text='<%# Eval("ReaderIn") %>' />
            </td>
            <td>
                <asp:Label ID="ReaderOutLabel" runat="server" Text='<%# Eval("ReaderOut") %>' />
            </td>
            <%--
            <td>
                <asp:Label ID="ECUniqueIdLabel" runat="server" Text='<%# Eval("ECUniqueId") %>' />
            </td>
            <td>
                <asp:Label ID="CzasLabel" runat="server" Text='<%# Eval("Czas") %>' />
            </td>
            <td>
                <asp:Label ID="ECCodeLabel" runat="server" Text='<%# Eval("ECCode") %>' />
            </td>
            <td>
                <asp:Label ID="ECUserId1Label" runat="server" Text='<%# Eval("ECUserId1") %>' />
            </td>
            <td>
                <asp:Label ID="ECReaderIdLabel" runat="server" Text='<%# Eval("ECReaderId") %>' />
            </td>
            <td>
                <asp:Label ID="ECDoorTypeLabel" runat="server" Text='<%# Eval("ECDoorType") %>' />
            </td>
            <td>
                <asp:CheckBox ID="InOutCheckBox" runat="server" Checked='<%# Eval("InOut") %>' Enabled="false" />
            </td>
            <td>
                <asp:CheckBox ID="DutyCheckBox" runat="server" Checked='<%# Eval("Duty") %>' 
                    Enabled="false" />
            </td>
            <td>
                <asp:Label ID="ECUniqueId1Label" runat="server" 
                    Text='<%# Eval("ECUniqueId1") %>' />
            </td>
            <td>
                <asp:Label ID="Czas1Label" runat="server" Text='<%# Eval("Czas1") %>' />
            </td>
            <td>
                <asp:Label ID="ECCode1Label" runat="server" Text='<%# Eval("ECCode1") %>' />
            </td>
            <td>
                <asp:Label ID="ECUserId2Label" runat="server" Text='<%# Eval("ECUserId2") %>' />
            </td>
            <td>
                <asp:Label ID="ECReaderId1Label" runat="server" Text='<%# Eval("ECReaderId1") %>' />
            </td>
            <td>
                <asp:Label ID="ECDoorType1Label" runat="server" Text='<%# Eval("ECDoorType1") %>' />
            </td>
            <td>
                <asp:Label ID="InOut1CheckBox" runat="server" Text='<%# Eval("InOut1") %>' />
            </td>
            <td>
                <asp:Label ID="Duty1CheckBox" runat="server" Text='<%# Eval("Duty1") %>' />
            </td>
            --%>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    No data was returned.</td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbCzasPracy" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">ECUserId</th>
                            <th runat="server">TimeIn</th>
                            <th runat="server">TimeOut</th>
                            <th runat="server">CzasPracy</th>
                            <th runat="server">CzasPracy1</th>
                            <th runat="server">ReaderIn</th>
                            <th runat="server">ReaderOut</th>
                            <%--
                            <th runat="server">ECUniqueId</th>
                            <th runat="server">Czas</th>
                            <th runat="server">ECCode</th>
                            <th runat="server">ECUserId1</th>
                            <th runat="server">ECReaderId</th>
                            <th runat="server">ECDoorType</th>
                            <th runat="server">InOut</th>
                            <th runat="server">Duty</th>
                            <th runat="server">ECUniqueId1</th>
                            <th runat="server">Czas1</th>
                            <th runat="server">ECCode1</th>
                            <th runat="server">ECUserId2</th>
                            <th runat="server">ECReaderId1</th>
                            <th runat="server">ECDoorType1</th>
                            <th runat="server">InOut1</th>
                            <th runat="server">Duty1</th>
                            --%>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select A.ECUserId,
                    case 
	                    when A.InOut = 1 and B.InOut = 1 then null
	                    else A.Czas
                    end as TimeIn,
                    case 
	                    when A.InOut = 0 and B.InOut = 0 then null
	                    else B.Czas
                    end as TimeOut,

                    ISNULL(case 
	                    when A.InOut = B.InOut then '0 00:00:00'
	                    else convert(varchar,DATEDIFF(SECOND, A.Czas, B.czas)/86400) + ' ' + convert(varchar, B.Czas - A.Czas, 8)
                    end, '0 00:00:00') as CzasPracy,	

                    case 
	                    when A.InOut = B.InOut then NULL
	                    else convert(varchar,DATEDIFF(SECOND, A.Czas, B.czas)/86400) + ' ' + convert(varchar, B.Czas - A.Czas, 8)
                    end as CzasPracy1,	

                    case 
	                    when A.InOut = 1 and B.InOut = 1 then null
	                    else A.ECReaderID
                    end as ReaderIn,

                    case 
	                    when A.InOut = 0 and B.InOut = 0 then null
	                    else B.ECReaderID
                    end as ReaderOut,

                    *
                    from RCP A left outer join RCP B
                    on B.ECUniqueID = (select top 1 C.ECUniqueID 
					                    from RCP C 
					                    where C.ECUserId=A.ECUserId 
					                    and C.ECReaderID in (29,1029)
					                    and C.InOut is not null
					                    and C.ECUniqueID > A.ECUniqueID)
                    where 
                    A.ECUserID=@RcpId and 
                    A.InOut is not null
                    
                    and A.ECReaderID in (29,1029)

                    and not (A.InOut = 1 and B.InOut = 0)
                    order by A.ECUserId, A.ECUniqueID"
                    
                >
    
    <SelectParameters>
        <asp:ControlParameter ControlID="hidRcpId" Name="RcpId" PropertyName="Value" Type="Int32" />
        <%--
        <asp:ControlParameter ControlID="hidRogersA" Name="andRogersA" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidRogersC" Name="andRogersC" PropertyName="Value" Type="String" />
        --%>
    </SelectParameters>
</asp:SqlDataSource>
