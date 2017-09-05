<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Download.aspx.cs" Inherits="HRApp.Portal.Download" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
<asp:SqlDataSource ID="dsGetFile" runat="server" 
    SelectCommand="
declare @id int 
set @id = {0}
declare @nrew varchar(20)
set @nrew = '{1}'

select f.*, ps.* 
, case when 
     f.Idx = p.Id2 and f.IdSciezki = 1
  or f.Idx = @nrew and f.IdSciezki != 1
  or p1.PortalAdmin = 1 

--  or f.Idx in (select KadryId from CO_HR_DB..fn_GetTree2(p.Id, 0, dbo.getdate(GETDATE()))) 
--  or f.Idx in (select KadryId from DM_HR_DB..fn_GetTree2(p.Id, 0, dbo.getdate(GETDATE()))) 
  or (f.Idx in (select KadryId from {2}..fn_GetTree2(p.Id, 0, dbo.getdate(GETDATE()))) and f.IdSciezki = 1)  -- podlegli ale tylko dla oceny  

  then 1 else 0 end CanDownload
from Pliki f
left join PlikiSciezki ps on ps.ID = f.IdSciezki

--left join CO_HR_DB..Pracownicy p on p.KadryId = @nrew
--left join DM_HR_DB..Pracownicy p on p.KadryId = @nrew
left join {2}..Pracownicy p on p.KadryId = @nrew  -- Id2 jest pobierane

outer apply (select dbo.GetRightId(p.Rights, 25) PortalAdmin) p1
where f.ID = @id
    "/>

    </div>
    </form>
</body>
</html>
