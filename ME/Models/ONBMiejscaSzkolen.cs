namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBMiejscaSzkolen")]
    public partial class ONBMiejscaSzkolen
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Nazwa { get; set; }

        [StringLength(128)]
        public string NazwaEn { get; set; }
    }
}
