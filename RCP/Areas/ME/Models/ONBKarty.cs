namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBKarty")]
    public partial class ONBKarty
    {
        public int Id { get; set; }

        public int IdPracownika { get; set; }

        public int? IdOpiekuna { get; set; }

        public int? IdPrzelozonego { get; set; }

        public DateTime DataZalozenia { get; set; }

        public int Status { get; set; }

        public DateTime? DataKoncaOP { get; set; }

        public DateTime? DataKoncaPW { get; set; }

        [StringLength(1024)]
        public string Uwagi { get; set; }
    }
}
