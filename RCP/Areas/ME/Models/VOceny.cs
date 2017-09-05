namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VOceny")]
    public partial class VOceny
    {
        public int? Id_Zadania { get; set; }

        [StringLength(511)]
        public string NazwaZadania { get; set; }

        public int? Waga { get; set; }

        public bool? Sumowane { get; set; }

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

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Pracownicy { get; set; }

        [StringLength(255)]
        public string Nazwisko { get; set; }

        [StringLength(255)]
        public string Imie { get; set; }

        [StringLength(255)]
        public string Imie2 { get; set; }

        [StringLength(255)]
        public string Nr_Ewid { get; set; }

        public int? Id_Gr_Zatr { get; set; }

        public int? Id_Stanowiska { get; set; }

        public int? Id_Str_OrgM { get; set; }

        public int? x_Id_Str_OrgA { get; set; }

        public int? Id_Status { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        public int? IdKierownika { get; set; }

        public int? Ocena { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime DataOceny { get; set; }

        [Column("Wartość ważona")]
        public int? Wartość_ważona { get; set; }

        [StringLength(511)]
        public string Pracownik { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Oceny { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Aktualna { get; set; }

        public int? IdLinia { get; set; }
    }
}
