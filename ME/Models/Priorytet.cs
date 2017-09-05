namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Priorytet")]
    public partial class Priorytet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Piorytet { get; set; }

        [Column("Priorytet")]
        public int? Priorytet1 { get; set; }

        [Required]
        [StringLength(255)]
        public string Opis { get; set; }
    }
}
