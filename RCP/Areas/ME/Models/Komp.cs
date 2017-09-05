namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Komp")]
    public partial class Komp
    {
        public int Id { get; set; }

        [StringLength(1024)]
        public string Nazwa { get; set; }

        [StringLength(1024)]
        public string NazwaEN { get; set; }

        [StringLength(1024)]
        public string Opis { get; set; }

        [StringLength(1024)]
        public string OpisEN { get; set; }

        public DateTime DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public double? Waga { get; set; }

        public int? Lp { get; set; }

        public bool ShowHeader { get; set; }
    }
}
