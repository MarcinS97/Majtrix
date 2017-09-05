namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProgramyZadania")]
    public partial class ProgramyZadania
    {
        public int Id { get; set; }

        public int IdProgramu { get; set; }

        public int IdZadania { get; set; }

        public int? IdStrOrg { get; set; }
    }
}
