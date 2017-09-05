namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserParams
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Typ { get; set; }

        [StringLength(200)]
        public string Str1 { get; set; }

        [StringLength(500)]
        public string Str2 { get; set; }

        public int? Int1 { get; set; }

        public int? Int2 { get; set; }

        public DateTime? Data1 { get; set; }

        public DateTime? Data2 { get; set; }

        public double? Float1 { get; set; }

        public double? Float2 { get; set; }

        public bool Bool1 { get; set; }

        public bool Bool2 { get; set; }
    }
}
