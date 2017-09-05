namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wagi")]
    public partial class Wagi
    {
        [Key]
        public int Id_Waga { get; set; }

        public int Waga { get; set; }

        [StringLength(200)]
        public string Waga_Opis { get; set; }

        [StringLength(200)]
        public string Waga_OpisEN { get; set; }
    }
}
