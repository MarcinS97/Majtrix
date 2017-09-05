namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Programy")]
    public partial class Programy
    {
        public int Id { get; set; }

        public DateTime DataOd { get; set; }

        public DateTime DataDo { get; set; }

        public DateTime? DataOcenyOd { get; set; }

        public DateTime? DataOcenyDo { get; set; }

        public int Status { get; set; }

        [StringLength(512)]
        public string Nazwa { get; set; }

        [StringLength(512)]
        public string NazwaEN { get; set; }

        public int? OkresKarencji { get; set; }
    }
}
