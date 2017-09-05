namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Absencja")]
    public partial class Absencja
    {
        public int Id { get; set; }

        public int? IdPracownika { get; set; }

        [Required]
        [StringLength(20)]
        public string NR_EW { get; set; }

        public DateTime DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public int Kod { get; set; }

        public int? IleDni { get; set; }

        public double? Godzin { get; set; }
    }
}
