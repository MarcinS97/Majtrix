namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KompWskazniki")]
    public partial class KompWskazniki
    {
        public int Id { get; set; }

        public int? Lp { get; set; }

        [StringLength(1024)]
        public string Nazwa { get; set; }

        [StringLength(1024)]
        public string NazwaEN { get; set; }

        public int IdKomp { get; set; }

        public double? Waga { get; set; }

        public DateTime? DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public bool ShowGrade { get; set; }
    }
}
