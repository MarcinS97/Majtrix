namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AbsencjeDlugotrwale")]
    public partial class AbsencjeDlugotrwale
    {
        public int Id { get; set; }

        public DateTime Rok { get; set; }

        public int IdPracownika { get; set; }

        [StringLength(500)]
        public string Powod { get; set; }

        public int? PowodKod { get; set; }

        public int? AutorId { get; set; }

        public DateTime DataWpisu { get; set; }

        public DateTime Od { get; set; }

        public DateTime? Do { get; set; }
    }
}
