namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Oceny")]
    public partial class Oceny
    {
        [Key]
        public int Id_Oceny { get; set; }

        public int Id_Pracownicy { get; set; }

        public int Id_Zadania { get; set; }

        public int? Ocena { get; set; }

        public int Id_Przelozony { get; set; }

        public DateTime DataOceny { get; set; }

        public bool Aktualna { get; set; }

        [StringLength(200)]
        public string Uwagi { get; set; }

        public int? IdStruktury { get; set; }

        public int? IdStrParent { get; set; }

        public int? ScalIdPrac { get; set; }

        public int? Nr_Ark { get; set; }

        public int? IdZadania1 { get; set; }

        public int? Typ { get; set; }

        public double? Wartosc { get; set; }
    }
}
