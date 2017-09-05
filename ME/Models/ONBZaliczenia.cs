namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBZaliczenia")]
    public partial class ONBZaliczenia
    {
        public int Id { get; set; }

        public int IdKarty { get; set; }

        public bool? Zaliczyl { get; set; }

        public DateTime? Data { get; set; }

        public int? IdProwadzacego { get; set; }

        [StringLength(512)]
        public string Zalecenia { get; set; }

        public int IdZadania { get; set; }
    }
}
