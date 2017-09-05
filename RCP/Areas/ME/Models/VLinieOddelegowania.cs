namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VLinieOddelegowania")]
    public partial class VLinieOddelegowania
    {
        [StringLength(255)]
        public string StrSymbol { get; set; }

        [StringLength(255)]
        public string StrNazwa { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LiniaId { get; set; }

        [StringLength(255)]
        public string LiniaSymbol { get; set; }

        [StringLength(255)]
        public string LiniaNazwa { get; set; }

        [StringLength(255)]
        public string Nazwisko { get; set; }

        [StringLength(255)]
        public string Imie { get; set; }

        [StringLength(255)]
        public string Nr_Ewid { get; set; }

        public int? Status { get; set; }

        public int? StatusOdd { get; set; }

        [StringLength(255)]
        public string KierNazwisko { get; set; }

        [StringLength(255)]
        public string KierImie { get; set; }

        public bool? APT { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Macierzysty { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Przydelegowany { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Oddelegowany { get; set; }

        public int? IdOdd { get; set; }

        public DateTime? Od { get; set; }

        public DateTime? Do { get; set; }

        public int? IdStruktury { get; set; }

        [StringLength(255)]
        public string StrSymbolOdd { get; set; }

        [StringLength(255)]
        public string StrNazwaOdd { get; set; }

        [StringLength(255)]
        public string LiniaSymbolOdd { get; set; }

        [StringLength(255)]
        public string LiniaNazwaOdd { get; set; }

        [StringLength(255)]
        public string KierNazwiskoOdd { get; set; }

        [StringLength(255)]
        public string KierImieOdd { get; set; }
    }
}
