namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KryteriaOceny")]
    public partial class KryteriaOceny
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Klucz0 { get; set; }

        [Required]
        [StringLength(2)]
        public string Klucz1 { get; set; }

        [Required]
        [StringLength(2)]
        public string Wartosc { get; set; }

        public DateTime Od { get; set; }

        public DateTime? Do { get; set; }

        public int? Id_Stanowiska { get; set; }
    }
}
