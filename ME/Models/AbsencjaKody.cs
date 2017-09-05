namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AbsencjaKody")]
    public partial class AbsencjaKody
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Kod { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazwa { get; set; }

        [StringLength(10)]
        public string Symbol { get; set; }

        public bool Widoczny { get; set; }

        public int? Status { get; set; }

        public int GodzinPracy { get; set; }

        public bool DniWolne { get; set; }

        [StringLength(50)]
        public string Kolor { get; set; }

        [StringLength(50)]
        public string KolorPU { get; set; }

        public bool PokazSymbolPU { get; set; }

        public bool WyborPU { get; set; }

        public bool WidocznyPU { get; set; }

        public int? Kolejnosc { get; set; }

        public bool NowaLinia { get; set; }

        [StringLength(500)]
        public string Opis { get; set; }

        public int Typ { get; set; }

        [StringLength(1)]
        public string Typ2 { get; set; }

        [StringLength(20)]
        public string Kod2 { get; set; }
    }
}
