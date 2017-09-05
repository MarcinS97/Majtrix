namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VPrzelozeniOceny")]
    public partial class VPrzelozeniOceny
    {
        [StringLength(511)]
        public string Kierownik { get; set; }

        [StringLength(511)]
        public string Pracownik { get; set; }

        public int? Ocena { get; set; }

        [Key]
        [Column(Order = 0)]
        public DateTime DataOceny { get; set; }

        [StringLength(200)]
        public string Uwagi { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Aktualna { get; set; }

        [StringLength(511)]
        public string NazwaZadania { get; set; }

        public int? Waga { get; set; }

        [StringLength(255)]
        public string LiniaSymbol { get; set; }

        [StringLength(255)]
        public string LiniaNazwa { get; set; }

        [StringLength(255)]
        public string StrSymbol { get; set; }

        [StringLength(255)]
        public string StrNazwa { get; set; }

        public int? Id_Przelozeni { get; set; }
    }
}
