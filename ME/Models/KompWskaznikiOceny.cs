namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KompWskaznikiOceny")]
    public partial class KompWskaznikiOceny
    {
        public int Id { get; set; }

        public int? Lp { get; set; }

        public int IdWskaznika { get; set; }

        [StringLength(512)]
        public string Nazwa { get; set; }

        [StringLength(512)]
        public string NazwaEN { get; set; }

        public double? Wartosc { get; set; }
    }
}
