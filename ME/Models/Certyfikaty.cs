namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Certyfikaty")]
    public partial class Certyfikaty
    {
        public int Id { get; set; }

        public int? IdUprawnienia { get; set; }

        public int? IdPracownika { get; set; }

        [StringLength(200)]
        public string Numer { get; set; }

        public DateTime? DataWaznosci { get; set; }

        [StringLength(200)]
        public string Kategoria { get; set; }

        public DateTime? DataZdobyciaUprawnien { get; set; }

        public DateTime? DataWaznosciPsychotestow { get; set; }

        public DateTime? DataWaznosciBadanLekarskich { get; set; }

        public DateTime? DataWaznosciUmowy { get; set; }

        public bool UmowaLojalnosciowa { get; set; }

        public int? ImportId { get; set; }

        public bool DataZdobyciaUprawnienOk { get; set; }

        public bool DataWaznosciPsychotestowOk { get; set; }

        public bool DataWaznosciBadanLekarskichOk { get; set; }

        public bool UmowaLojalnosciowaOk { get; set; }

        public bool DataWaznosciSet { get; set; }

        public bool Aktualny { get; set; }

        [StringLength(500)]
        public string Uwagi { get; set; }
    }
}
