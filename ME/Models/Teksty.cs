namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Teksty")]
    public partial class Teksty
    {
        [Key]
        [StringLength(20)]
        public string Typ { get; set; }

        [Required]
        [StringLength(200)]
        public string Opis { get; set; }

        public string Tekst { get; set; }

        public string TekstEN { get; set; }
    }
}
