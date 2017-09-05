namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KompOdpowiedzi")]
    public partial class KompOdpowiedzi
    {
        public int Id { get; set; }

        public int IdPracownika { get; set; }

        public int IdProgramu { get; set; }

        public int IdPytania { get; set; }

        public int? IdOdpowiedzi { get; set; }
    }
}
