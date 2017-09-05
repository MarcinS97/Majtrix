namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PassSkills
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Poziom { get; set; }

        [Required]
        [StringLength(200)]
        public string OpisZlacza { get; set; }

        [Required]
        [StringLength(100)]
        public string Kwalifikacje { get; set; }

        public bool Aktywny { get; set; }
    }
}
