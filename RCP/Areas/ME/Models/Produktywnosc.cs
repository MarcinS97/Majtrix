namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Produktywnosc")]
    public partial class Produktywnosc
    {
        public int Id { get; set; }

        public DateTime? Data { get; set; }

        public int? IdPracownika { get; set; }

        public double? Wartosc { get; set; }
    }
}
