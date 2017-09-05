namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Uprawnienia")]
    public partial class Uprawnienia
    {
        public int Id { get; set; }

        public int Typ { get; set; }

        [StringLength(200)]
        public string Symbol { get; set; }

        [Required]
        [StringLength(200)]
        public string Nazwa { get; set; }

        [StringLength(200)]
        public string NazwaEN { get; set; }

        [StringLength(200)]
        public string Poziom { get; set; }

        [StringLength(200)]
        public string PoziomEN { get; set; }

        [StringLength(200)]
        public string Opis { get; set; }

        [StringLength(200)]
        public string Pola { get; set; }

        public int? Kolejnosc { get; set; }

        public bool Aktywne { get; set; }

        public bool Produkcyjne { get; set; }

        public bool Nieprodukcyjne { get; set; }

        public bool PaszportSpawacza { get; set; }

        public int? KwalifikacjeId { get; set; }

        public int? PoziomId { get; set; }

        public int? PoziomPoziom { get; set; }

        public bool Grupa { get; set; }

        public bool Wymagane { get; set; }

        public bool Zablokowane { get; set; }
    }
}
