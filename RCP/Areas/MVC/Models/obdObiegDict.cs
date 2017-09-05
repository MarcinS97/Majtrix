namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdObiegDict")]
    public partial class obdObiegDict
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazwa { get; set; }

        [Required]
        [StringLength(500)]
        public string Opis { get; set; }

        [StringLength(500)]
        public string Uwagi { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataOd { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataDo { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataUtworzenia { get; set; }

        public int? CzasTrwania { get; set; }

        public bool Aktywny { get; set; }
    }
}
