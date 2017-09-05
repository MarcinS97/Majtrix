namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cmd")]
    public partial class Cmd
    {
        public int Id { get; set; }

        [Column("Cmd")]
        [Required]
        [StringLength(50)]
        public string Cmd1 { get; set; }

        public int Status { get; set; }

        [StringLength(500)]
        public string Msg1 { get; set; }

        [StringLength(2000)]
        public string Msg2 { get; set; }

        public int? AuthorId { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Executed { get; set; }

        [StringLength(255)]
        public string AuthKey { get; set; }

        [StringLength(500)]
        public string Par1 { get; set; }

        [StringLength(500)]
        public string Par2 { get; set; }

        [StringLength(500)]
        public string Par3 { get; set; }

        [StringLength(500)]
        public string Par4 { get; set; }

        [StringLength(500)]
        public string Par5 { get; set; }

        [StringLength(500)]
        public string Par6 { get; set; }
    }
}
