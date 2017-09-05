namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdAkcjeDict")]
    public partial class obdAkcjeDict
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazwa { get; set; }

        [StringLength(500)]
        public string Opis { get; set; }

        [Required]
        public string Skrypt { get; set; }

        public int Typ { get; set; }
    }
}
