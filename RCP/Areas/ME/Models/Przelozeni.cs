namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Przelozeni")]
    public partial class Przelozeni
    {
        [Key]
        public int Id_Przelozeni { get; set; }

        [StringLength(255)]
        public string Nazwisko { get; set; }

        [StringLength(255)]
        public string Imie { get; set; }

        [StringLength(255)]
        public string Imie2 { get; set; }

        [StringLength(255)]
        public string Nr_Ewid { get; set; }

        public int? Id_Gr_Zatr { get; set; }

        public int? Id_Stanowiska { get; set; }

        public int? Id_Str_Org { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        public double? Telefon { get; set; }

        public int? Id_Priorytet { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public bool? Blokada { get; set; }

        [StringLength(50)]
        public string Rights { get; set; }

        public int? Status { get; set; }

        public int? x_IdsStrOrg { get; set; }

        [StringLength(50)]
        public string Nick { get; set; }

        public int? Id2 { get; set; }

        public DateTime? DataZatr { get; set; }

        public DateTime? DataUmDo { get; set; }

        public DateTime? DataZwol { get; set; }

        [StringLength(50)]
        public string Nr_Ewid2 { get; set; }
    }
}
