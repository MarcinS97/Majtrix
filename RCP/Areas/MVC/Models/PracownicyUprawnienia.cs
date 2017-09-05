namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.SqlClient;

    [Table("PracownicyUprawnienia")]
    public partial class PracownicyUprawnienia
    {
        ObiegDokumentow ob = new ObiegDokumentow();
        public int Id { get; set; }
        public int IdPracownika { get; set; }
        public int IdUprawnienia { get; set; }
        public DateTime Od { get; set; }
        public DateTime? Do { get; set; }

        public static List<PracownicyUprawnienia> PobierzAktualne()
        {
            ObiegDokumentow ob = new ObiegDokumentow();
            return ob.Database.SqlQuery<PracownicyUprawnienia>("select * from PracownicyUprawnienia where GETDATE() between Od and ISNULL(Do, '20990909')").ToList();
        }
    }
}
