namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VZadaniaTyp")]
    public partial class VZadaniaTyp
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Zadania { get; set; }

        public int? Id_Gr_Oper { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(511)]
        public string NazwaZadania { get; set; }

        public int? Waga { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Sumowane { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool APT { get; set; }

        public int? Global { get; set; }

        public int? Id_Maszyny1 { get; set; }

        public int? Id_Obszary1 { get; set; }

        public int? Id_Podobszary1 { get; set; }

        public int? Id_Director1 { get; set; }

        public int? Id_Task1 { get; set; }

        public int? Typ { get; set; }

        [StringLength(511)]
        public string NazwaZadaniaEN { get; set; }

        public int? Id_Rodzaj1 { get; set; }

        public int? Id_Poziom1 { get; set; }

        [StringLength(20)]
        public string ProdAdm1 { get; set; }

        [StringLength(20)]
        public string Dyr1 { get; set; }

        public int? Lp1 { get; set; }

        [StringLength(20)]
        public string StrOrg1 { get; set; }

        public int? Id_Dzialanie1 { get; set; }

        public int? Id_Narzedzie1 { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Onboarding { get; set; }

        public int? Id { get; set; }

        [StringLength(100)]
        public string TypNazwa { get; set; }

        public int? Kolejnosc { get; set; }

        public bool? Aktywny { get; set; }

        public bool? Wybor { get; set; }

        [StringLength(100)]
        public string TypNazwaEN { get; set; }

        public bool? Dodatkowe { get; set; }
    }
}
