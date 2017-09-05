namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OcenyNazwy")]
    public partial class OcenyNazwy
    {
        public int? Ocena { get; set; }

        [StringLength(100)]
        public string Nazwa { get; set; }

        [StringLength(200)]
        public string Opis { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(20)]
        public string BkColor { get; set; }

        public int Id { get; set; }

        [StringLength(100)]
        public string NazwaEN { get; set; }

        [StringLength(200)]
        public string OpisEN { get; set; }

        public bool Wybor { get; set; }

        public int? Kolejnosc { get; set; }

        [StringLength(50)]
        public string Procent { get; set; }

        [StringLength(10)]
        public string Symbol { get; set; }

        public double? Wartosc { get; set; }
    }
}
