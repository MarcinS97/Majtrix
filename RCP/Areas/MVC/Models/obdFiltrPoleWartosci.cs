namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdFiltrPoleWartosci")]
    public partial class obdFiltrPoleWartosci
    {
        public int Id { get; set; }

        public int IdRoliDict { get; set; }

        public int IdFiltrPoleDict { get; set; }

        [Required]
        [StringLength(100)]
        public string Wartosc { get; set; }

        public string WarunekAlterSQL { get; set; }
    }
}
