namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Log")]
    public partial class Log
    {
        public int Id { get; set; }

        public DateTime DataCzas { get; set; }

        public int ParentId { get; set; }

        public int Typ { get; set; }

        public int? Typ2 { get; set; }

        public int? PracId { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(10)]
        public string Par { get; set; }

        public int? Kod { get; set; }

        public int? Kod2 { get; set; }

        public int? Kod3 { get; set; }

        public int? Kod4 { get; set; }

        [StringLength(1800)]
        public string Info { get; set; }

        [StringLength(2000)]
        public string Info2 { get; set; }

        public int Status { get; set; }
    }
}
