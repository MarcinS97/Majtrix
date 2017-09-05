namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pkt")]
    public partial class Pkt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Lp { get; set; }

        public int? Ocena { get; set; }

        [StringLength(255)]
        public string Opis { get; set; }
    }
}
