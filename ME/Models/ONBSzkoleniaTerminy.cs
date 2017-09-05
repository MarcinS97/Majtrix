namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBSzkoleniaTerminy")]
    public partial class ONBSzkoleniaTerminy
    {
        public int Id { get; set; }

        public int RodzajSzkoleniaId { get; set; }

        public int? TrenerId { get; set; }

        public int Status { get; set; }

        public DateTime? Data { get; set; }

        public int? MiejsceId { get; set; }

        [StringLength(1024)]
        public string Uwagi { get; set; }

        public int? RzCzasTrwania { get; set; }
    }
}
