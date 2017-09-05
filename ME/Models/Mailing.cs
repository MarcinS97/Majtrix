namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mailing")]
    public partial class Mailing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [StringLength(25)]
        public string Typ { get; set; }

        [StringLength(25)]
        public string Grupa { get; set; }

        [StringLength(200)]
        public string Opis { get; set; }

        [StringLength(200)]
        public string Temat { get; set; }

        [StringLength(2000)]
        public string Tresc { get; set; }

        public bool Aktywny { get; set; }
    }
}
