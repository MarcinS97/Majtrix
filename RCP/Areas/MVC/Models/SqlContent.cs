namespace HRRcp.Areas.MVC.Models
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

        [StringLength(100)]
        public string ConStr { get; set; }

        public string Sql { get; set; }

        public string Opis { get; set; }

        public int? Kolejnosc { get; set; }

        public bool Aktywny { get; set; }

        public bool Wnioski { get; set; }

        [StringLength(250)]
        public string Rights { get; set; }

        [StringLength(200)]
        public string EmptyInfo { get; set; }
    }
}
