namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VZadania")]
    public partial class VZadania
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Zadania { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(511)]
        public string NazwaZadania { get; set; }

        public int? Waga { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Sumowane { get; set; }

        [StringLength(255)]
        public string SymbolStrumienia { get; set; }

        [StringLength(255)]
        public string NazwaStrumienia { get; set; }

        [StringLength(255)]
        public string SymbolLinii { get; set; }

        [StringLength(255)]
        public string NazwaLinii { get; set; }

        [StringLength(50)]
        public string GrOperacjiSymbol { get; set; }

        [StringLength(255)]
        public string GrOperacjiNazwa { get; set; }

        public int? IdLinia { get; set; }
    }
}
