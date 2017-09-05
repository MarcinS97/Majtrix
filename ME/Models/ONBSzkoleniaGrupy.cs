namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBSzkoleniaGrupy")]
    public partial class ONBSzkoleniaGrupy
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Nazwa { get; set; }

        [StringLength(128)]
        public string NazwaEN { get; set; }

        [StringLength(1024)]
        public string Link { get; set; }
    }
}
