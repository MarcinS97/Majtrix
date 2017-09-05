namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Oddelegowania")]
    public partial class Oddelegowania
    {
        public int Id { get; set; }

        public int IdPracownika { get; set; }

        public DateTime Od { get; set; }

        public DateTime Do { get; set; }

        public int IdStruktury { get; set; }

        public int? IdKierownika { get; set; }

        public int? IdKierownikaRq { get; set; }

        public DateTime? DataRq { get; set; }

        [StringLength(200)]
        public string UwagiRq { get; set; }

        public int? IdKierownikaAcc { get; set; }

        public DateTime? DataAcc { get; set; }

        [StringLength(200)]
        public string UwagiAcc { get; set; }

        public int Status { get; set; }

        public int Typ { get; set; }

        public bool MailingKadry { get; set; }

        [StringLength(200)]
        public string Pole1 { get; set; }

        [StringLength(200)]
        public string Pole2 { get; set; }

        [StringLength(200)]
        public string Pole3 { get; set; }

        [StringLength(200)]
        public string Pole4 { get; set; }

        [StringLength(200)]
        public string Pole5 { get; set; }

        [StringLength(200)]
        public string Pole6 { get; set; }

        [StringLength(200)]
        public string Pole7 { get; set; }

        [StringLength(200)]
        public string Pole8 { get; set; }

        [StringLength(500)]
        public string Uwaga1 { get; set; }

        [StringLength(500)]
        public string Uwaga2 { get; set; }

        [StringLength(500)]
        public string Uwaga3 { get; set; }

        public int? StatusKadry { get; set; }
    }
}
