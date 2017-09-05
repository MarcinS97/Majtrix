namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;

    [Table("obdKroki")]
    public partial class obdKroki
    {
        ObiegDokumentow ob = new ObiegDokumentow();

        public int Id { get; set; }

        public int IdObiegu { get; set; }

        public int IdKrokuDict { get; set; }

        public int? IdRoliDict { get; set; }

        public void InitKolejnosc()
        {
            string query = @"declare @IloscKrokow int
select @IloscKrokow = Count(Id) from obdKrokiDict kd
where kd.IdObieguDict = @IdObiegu

declare @IloscLiczb int
select @IloscLiczb = Count(Kolejnosc) from
(
	select distinct Kolejnosc from obdKrokiDict kd
	where kd.IdObieguDict = @IdObiegu
) a

if @IloscKrokow != @IloscLiczb
begin
	update obdKrokiDict set
	  Kolejnosc = a.nowaKolejnosc
	from obdKrokiDict kd
	inner join
	(
		select
		  Id
		, ROW_NUMBER() over(order by Id) as nowaKolejnosc
		from obdKrokiDict
		where IdObieguDict = @IdObiegu
	) a on kd.Id = a.Id
end";
            ob.Database.ExecuteSqlCommand(query, new SqlParameter("IdObiegu", IdObiegu));
        }

        public void MoveUp()
        {

        }

        public void MoveDown()
        {
            
        }
    }
}
