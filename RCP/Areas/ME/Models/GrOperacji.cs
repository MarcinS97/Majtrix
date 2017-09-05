namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrOperacji")]
    public partial class GrOperacji
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Symbol { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }

        [StringLength(255)]
        public string NazwaEN { get; set; }
    }
}
