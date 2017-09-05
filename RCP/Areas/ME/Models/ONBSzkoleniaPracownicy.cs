namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBSzkoleniaPracownicy")]
    public partial class ONBSzkoleniaPracownicy
    {
        public int Id { get; set; }

        public int IdKarty { get; set; }

        public int? SzkolenieId { get; set; }

        public bool? Zaliczyl { get; set; }

        public int TypId { get; set; }
    }
}
