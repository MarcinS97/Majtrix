namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBSzkoleniaTrenerzy")]
    public partial class ONBSzkoleniaTrenerzy
    {
        public int Id { get; set; }

        public int IdTrenera { get; set; }

        public int IdSzkolenia { get; set; }
    }
}
