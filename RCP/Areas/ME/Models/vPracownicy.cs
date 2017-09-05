namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vPracownicy")]
    public partial class vPracownicy
    {
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

        [StringLength(50)]
        public string Rodzaj_Umowy { get; set; }

        [StringLength(50)]
        public string Nazwa_Stan { get; set; }

        public int? Id_Str_OrgM { get; set; }

        public int? x_Id_Str_OrgA { get; set; }

        public int? Id_Status { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        public int? IdKierownika { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool APT { get; set; }

        public int? IdStrumienia { get; set; }

        [StringLength(511)]
        public string Pracownik { get; set; }

        [Column("Jednostka org. macierzysta")]
        [StringLength(255)]
        public string Jednostka_org__macierzysta { get; set; }

        [Column("Status obecności")]
        [StringLength(50)]
        public string Status_obecności { get; set; }

        [StringLength(511)]
        public string Kierownik { get; set; }

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

        [Column("Wielkość ważona")]
        public int? Wielkość_ważona { get; set; }

        [Column("Symbol strumienia")]
        [StringLength(255)]
        public string Symbol_strumienia { get; set; }
    }
}
