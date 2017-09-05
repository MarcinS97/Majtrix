namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZadaniaGrupy")]
    public partial class ZadaniaGrupy
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nazwa { get; set; }

        [Required]
        [StringLength(200)]
        public string NazwaEN { get; set; }

        public int? Kolejnosc { get; set; }

        public int Typ { get; set; }

        public bool Aktywna { get; set; }
    }
}
