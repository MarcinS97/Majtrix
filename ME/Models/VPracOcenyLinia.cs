namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VPracOcenyLinia")]
    public partial class VPracOcenyLinia
    {
        [StringLength(511)]
        public string Pracownik { get; set; }

        [Column("Nr ew.")]
        [StringLength(255)]
        public string Nr_ew_ { get; set; }

        [Column("Strumień macierzysty")]
        [StringLength(255)]
        public string Strumień_macierzysty { get; set; }

        [StringLength(255)]
        public string StrNazwa { get; set; }

        [Column("Linia macierzysta")]
        [StringLength(255)]
        public string Linia_macierzysta { get; set; }

        [StringLength(255)]
        public string LiniaNazwa { get; set; }

        [Column("Suma ocen")]
        public int? Suma_ocen { get; set; }

        [Column("Wielkość ważona")]
        public int? Wielkość_ważona { get; set; }

        [Column("Ocena 3")]
        public int? Ocena_3 { get; set; }

        [Column("Ocena 2")]
        public int? Ocena_2 { get; set; }

        [Column("Ocena 1")]
        public int? Ocena_1 { get; set; }

        [Column("Ocena 0")]
        public int? Ocena_0 { get; set; }

        [Column("Brak oceny")]
        public int? Brak_oceny { get; set; }

        [Column("Ilość czynności")]
        public int? Ilość_czynności { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Pracownicy { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool APT { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        public int? IdKierownika { get; set; }

        public int? StrId { get; set; }

        public int? LiniaId { get; set; }
    }
}
