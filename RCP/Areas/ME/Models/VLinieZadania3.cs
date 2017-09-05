namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VLinieZadania3
    {
        public int? Id { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Str_Org { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Zadania2 { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Zadania { get; set; }

        public int? Id_Gr_Oper { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(511)]
        public string NazwaZadania { get; set; }

        [StringLength(511)]
        public string NazwaZadaniaEN { get; set; }

        public int? Waga { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Sumowane { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool APT { get; set; }

        public int? Global { get; set; }

        public int? Typ { get; set; }

        public int? Id_Obszary { get; set; }

        public int? Id_Podobszary { get; set; }

        public int? Id_Maszyny { get; set; }

        public int? Id_Task { get; set; }

        public int? Id_Dzialanie { get; set; }

        public int? Id_Narzedzie { get; set; }

        public int? Id_Rodzaj { get; set; }

        public int? Id_Poziom { get; set; }

        public bool? Aktywny { get; set; }

        public int? Kolejnosc { get; set; }

        [StringLength(100)]
        public string TypNazwa { get; set; }

        [StringLength(100)]
        public string TypNazwaEN { get; set; }

        public bool? Dodatkowe { get; set; }

        public bool? Wybor { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool Onboarding { get; set; }
    }
}
