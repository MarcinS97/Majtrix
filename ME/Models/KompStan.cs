namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KompStan")]
    public partial class KompStan
    {
        public int Id { get; set; }

        public int IdPytania { get; set; }

        public int IdStanowiska { get; set; }

        public DateTime DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public double? Poziom { get; set; }
    }
}
