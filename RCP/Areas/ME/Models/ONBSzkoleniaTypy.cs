namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBSzkoleniaTypy")]
    public partial class ONBSzkoleniaTypy
    {
        public int Id { get; set; }

        [StringLength(512)]
        public string Nazwa { get; set; }

        [StringLength(512)]
        public string NazwaEN { get; set; }

        public int? Status { get; set; }

        public int? Typ { get; set; }

        public int? IdGrupy { get; set; }

        public int? CzasTrwania { get; set; }
    }
}
