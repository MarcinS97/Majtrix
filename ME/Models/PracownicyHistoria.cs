namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PracownicyHistoria")]
    public partial class PracownicyHistoria
    {
        public int Id { get; set; }

        public int IdPracownika { get; set; }

        public DateTime? Od { get; set; }

        public DateTime? Do { get; set; }

        public int? IdStrOrg { get; set; }

        public int? IdKierownika { get; set; }

        public int? IdStanowiska { get; set; }
    }
}
