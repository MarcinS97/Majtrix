namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBKartaSekcje")]
    public partial class ONBKartaSekcje
    {
        public int Id { get; set; }

        public int? Typ { get; set; }

        [StringLength(128)]
        public string Nazwa { get; set; }

        [StringLength(128)]
        public string NazwaEN { get; set; }
    }
}
