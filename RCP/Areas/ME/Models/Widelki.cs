namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Widelki")]
    public partial class Widelki
    {
        public int Id { get; set; }

        public DateTime DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public int? Od { get; set; }

        public int? Do { get; set; }

        public int Typ { get; set; }

        [Required]
        [StringLength(2)]
        public string Ocena { get; set; }

        public int Id_Stanowiska { get; set; }
    }
}
