namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VOddelegowaniaNaDzis
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public int? DId { get; set; }

        public int? IdPracownika { get; set; }

        public DateTime? Od { get; set; }

        public DateTime? Do { get; set; }

        public int? IdStruktury { get; set; }

        public int? DIdKierownika { get; set; }

        public int? IdKierownikaRq { get; set; }

        public DateTime? DataRq { get; set; }

        [StringLength(200)]
        public string UwagiRq { get; set; }

        public int? IdKierownikaAcc { get; set; }

        public DateTime? DataAcc { get; set; }

        [StringLength(200)]
        public string UwagiAcc { get; set; }

        public int? DStatus { get; set; }

        public int? Typ { get; set; }
    }
}
