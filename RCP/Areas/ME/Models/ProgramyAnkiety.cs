namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProgramyAnkiety")]
    public partial class ProgramyAnkiety
    {
        public int Id { get; set; }

        public int IdProgramu { get; set; }

        public int IdPracownika { get; set; }

        public int? IdStrOrg { get; set; }

        public int? IdPrzelozonego { get; set; }

        public int? Status { get; set; }
    }
}
