namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailingZnaczniki")]
    public partial class MailingZnaczniki
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Grupa { get; set; }

        [Required]
        [StringLength(30)]
        public string Znacznik { get; set; }

        [StringLength(200)]
        public string Opis { get; set; }

        public int? Kolejnosc { get; set; }
    }
}
