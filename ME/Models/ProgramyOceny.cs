namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProgramyOceny")]
    public partial class ProgramyOceny
    {
        public int Id { get; set; }

        public int? IdProgramu { get; set; }

        public int? IdPracownika { get; set; }

        public int? IdZadania { get; set; }

        public int? Ocena { get; set; }

        [StringLength(1024)]
        public string Uwagi { get; set; }
    }
}
