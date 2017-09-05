namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZadaniaGrupyTypy")]
    public partial class ZadaniaGrupyTypy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nazwa { get; set; }

        [Required]
        [StringLength(200)]
        public string NazwaEN { get; set; }

        public bool Wybor { get; set; }
    }
}
