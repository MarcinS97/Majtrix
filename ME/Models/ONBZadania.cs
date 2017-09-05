namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBZadania")]
    public partial class ONBZadania
    {
        public int Id { get; set; }

        [StringLength(512)]
        public string Nazwa { get; set; }

        [StringLength(512)]
        public string NazwaEN { get; set; }
    }
}
