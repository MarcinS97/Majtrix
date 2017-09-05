namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdObieg")]
    public partial class obdObieg
    {
        public int Id { get; set; }

        public int IdObieguDict { get; set; }

        public int IdTworzacego { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataZlozenia { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataZakonczenia { get; set; }

        public int? CzasTrwania { get; set; }

        public int Status { get; set; }
    }
}
