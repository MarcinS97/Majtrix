namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Akcja")]
    public partial class Akcja
    {
        [Key]
        public int ID_Akcja { get; set; }

        [Column("Akcja")]
        [StringLength(50)]
        public string Akcja1 { get; set; }
    }
}
