namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Monity")]
    public partial class Monity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Typ { get; set; }

        public int EventId { get; set; }

        public int UserId { get; set; }

        public DateTime Data { get; set; }

        public int Count { get; set; }
    }
}
