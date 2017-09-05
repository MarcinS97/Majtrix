namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SqlContent")]
    public partial class SqlContent
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Grupa { get; set; }

        public int Typ { get; set; }

        [StringLength(200)]
        public string MenuText { get; set; }

        [StringLength(200)]
        public string MenuTextEN { get; set; }

        [StringLength(100)]
        public string ConStr { get; set; }

        public string Sql { get; set; }

        public string Opis { get; set; }

        public int? Kolejnosc { get; set; }

        public bool Aktywny { get; set; }
    }
}
