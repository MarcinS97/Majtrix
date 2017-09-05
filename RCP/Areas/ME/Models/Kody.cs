namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kody")]
    public partial class Kody
    {
        public int Id { get; set; }

        [StringLength(25)]
        public string Typ { get; set; }

        [StringLength(25)]
        public string Kod { get; set; }

        [StringLength(200)]
        public string Nazwa { get; set; }

        [StringLength(200)]
        public string NazwaEN { get; set; }

        [StringLength(200)]
        public string Nazwa2 { get; set; }

        [StringLength(200)]
        public string Nazwa2EN { get; set; }

        [StringLength(500)]
        public string Str1 { get; set; }

        [StringLength(500)]
        public string Str2 { get; set; }

        [StringLength(500)]
        public string Str3 { get; set; }

        [StringLength(500)]
        public string Str4 { get; set; }

        public int? Int1 { get; set; }

        public int? Int2 { get; set; }

        public int? Int3 { get; set; }

        public int? Int4 { get; set; }

        public DateTime? Data1 { get; set; }

        public DateTime? Data2 { get; set; }

        public DateTime? Data3 { get; set; }

        public DateTime? Data4 { get; set; }

        public double? Float1 { get; set; }

        public double? Float2 { get; set; }

        public double? Float3 { get; set; }

        public double? Float4 { get; set; }

        public int? Kolejnosc { get; set; }

        public bool Aktywny { get; set; }
    }
}
