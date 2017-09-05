namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdPolaDict")]
    public partial class obdPolaDict
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazwa { get; set; }

        public bool DozwoloneZnaki { get; set; }

        public bool MaxDlugosc { get; set; }

        public bool MinDlugosc { get; set; }
    }
}
