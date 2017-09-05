namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MRodzaje")]
    public partial class MRodzaje
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nazwa { get; set; }

        [StringLength(255)]
        public string NazwaEN { get; set; }

        public int? IdParent { get; set; }
    }
}
