namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZadaniaTypy")]
    public partial class ZadaniaTypy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TypNazwa { get; set; }

        public int? Kolejnosc { get; set; }

        public bool Aktywny { get; set; }

        public bool Wybor { get; set; }

        [StringLength(100)]
        public string TypNazwaEN { get; set; }

        public bool Dodatkowe { get; set; }
    }
}
