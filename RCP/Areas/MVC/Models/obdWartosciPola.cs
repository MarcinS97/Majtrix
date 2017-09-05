namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdWartosciPola")]
    public partial class obdWartosciPola
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Wartosc { get; set; }

        public int IdObiegu { get; set; }

        public int IdPola { get; set; }
    }
}
