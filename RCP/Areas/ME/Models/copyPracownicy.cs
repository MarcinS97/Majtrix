namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("copyPracownicy")]
    public partial class copyPracownicy
    {
        [Key]
        [Column(Order = 0)]
        public int Id_Pracownicy { get; set; }

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

        public int? Id_Str_OrgM { get; set; }

        public int? x_Id_Str_OrgA { get; set; }

        public int? Id_Status { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Status { get; set; }

        public int? IdKierownika { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool APT { get; set; }

        public int? IdStrumienia { get; set; }

        public DateTime? DataZatr { get; set; }

        public DateTime? DataUmDo { get; set; }

        public DateTime? DataZwol { get; set; }

        public DateTime? ScalData { get; set; }

        public int? ScalActualId { get; set; }

        [StringLength(50)]
        public string Nr_Ewid2 { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Nick { get; set; }

        [StringLength(50)]
        public string Rights { get; set; }

        public int? Id2 { get; set; }
    }
}
