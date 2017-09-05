namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Przelozeni_20150124
    {
        [Key]
        [Column(Order = 0)]
        public int Id_Przelozeni { get; set; }

        [StringLength(255)]
        public string Nazwisko { get; set; }

        [StringLength(255)]
        public string Imie { get; set; }

        [StringLength(255)]
        public string Imie2 { get; set; }

        [StringLength(255)]
        public string Nr_Ewid { get; set; }

        public int? Id_Gr_zatr { get; set; }

        public int? Id_Stanowiska { get; set; }

        public int? Id_Str_Org { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public double? Telefon { get; set; }

        public int? Id_Priorytet { get; set; }

        [StringLength(255)]
        public string Login { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Blokada { get; set; }

        [StringLength(50)]
        public string Rights { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        [StringLength(200)]
        public string x_IdsStrOrg { get; set; }

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
