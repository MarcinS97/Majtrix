<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HarmonogramSQL.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.HarmonogramSQL" %>

<asp:HiddenField ID="hidVer" runat="server" Value="1.0"/>
 
<asp:SqlDataSource ID="dsSetPracAktywny" runat="server"
    UpdateCommand="{0} ({1})"   
    SelectCommand="
declare @pid int
declare @act int
declare @data datetime
set @pid = {0}
set @act = {1}
set @data = DATEADD(D,-1,'{2}')

delete from Przypisania where IdPracownika = @pid and Od &gt; @data

declare @rid int
select top 1 @rid = Id from Przypisania where IdPracownika = @pid and Status = 1 and Od &lt;= @data order by Od desc

if @act = 1 begin
    update Przypisania set Do = null where Id = @rid
    update Pracownicy set Status = 0, DataZwol = null where Id = @pid
end
else begin
    update Przypisania set Do = @data where Id = @rid
    update Pracownicy set Status = -1, DataZwol = @data where Id = @pid
end
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsImport" runat="server"
    SelectCommand="
BEGIN TRANSACTION;
BEGIN TRY
-------------------------------
-------------------------------

-------------------------------
-------------------------------
END TRY
BEGIN CATCH
	select -1 as Error, ERROR_MESSAGE() AS ErrorMessage, @step as Step
	/*
    SELECT 
         ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	*/
    IF @@TRANCOUNT &gt; 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT &gt; 0 BEGIN
    COMMIT TRANSACTION;
    select 0 as Error, null as ErrorMessage, @step as Step
END    
    ">
</asp:SqlDataSource>

