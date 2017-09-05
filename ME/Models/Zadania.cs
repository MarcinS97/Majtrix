namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zadania")]
    public partial class Zadania
    {
        [Key]
        public int Id_Zadania { get; set; }

        public int? Id_Gr_Oper { get; set; }

        [Required]
        [StringLength(511)]
        public string NazwaZadania { get; set; }

        public int? Waga { get; set; }

        public bool Sumowane { get; set; }

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

        public bool Onboarding { get; set; }
    }
}
