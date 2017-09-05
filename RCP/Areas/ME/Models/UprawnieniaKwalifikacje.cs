namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UprawnieniaKwalifikacje")]
    public partial class UprawnieniaKwalifikacje
    {
        public int Id { get; set; }

        public int Typ { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazwa { get; set; }

        public int? Kolejnosc { get; set; }

        public bool Aktywna { get; set; }

        [StringLength(100)]
        public string NazwaEN { get; set; }
    }
}
