namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zastepstwa")]
    public partial class Zastepstwa
    {
        public int Id { get; set; }

        public int IdZastepowany { get; set; }

        public int IdZastepujacy { get; set; }

        public DateTime Od { get; set; }

        public DateTime Do { get; set; }
    }
}
