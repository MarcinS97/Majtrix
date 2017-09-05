namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CertyfikatySpaw")]
    public partial class CertyfikatySpaw
    {
        public int Id { get; set; }

        public int IdCertyfikatu { get; set; }

        public int IdUprawnienia { get; set; }

        public int IdPracownika { get; set; }

        [StringLength(255)]
        public string SymbolSpawacza { get; set; }

        [StringLength(255)]
        public string Norma { get; set; }

        [StringLength(255)]
        public string Proces { get; set; }

        [StringLength(255)]
        public string TypMaterialu { get; set; }

        [StringLength(255)]
        public string TypZlacza { get; set; }

        [StringLength(255)]
        public string GrMaterialowa15608 { get; set; }

        [StringLength(255)]
        public string MaterialDodatkowy { get; set; }

        [StringLength(255)]
        public string Grubosc { get; set; }

        [StringLength(255)]
        public string GruboscMin { get; set; }

        [StringLength(255)]
        public string GruboscMax { get; set; }

        [StringLength(255)]
        public string Srednica { get; set; }

        [StringLength(255)]
        public string Pozycja { get; set; }

        [StringLength(255)]
        public string NazwaUrzadzenia { get; set; }

        [StringLength(255)]
        public string NrCertyfikatu { get; set; }

        public DateTime? DataWydania { get; set; }

        public DateTime DataWaznosci { get; set; }

        public DateTime? DataWaznosciOstPrzedl { get; set; }

        public DateTime? DataNastPrzedl { get; set; }

        public int? IdMistrza { get; set; }

        [StringLength(255)]
        public string Mistrz { get; set; }

        [StringLength(255)]
        public string SymbolSpawalni { get; set; }

        [StringLength(255)]
        public string Uwagi { get; set; }

        public bool Aktualny { get; set; }

        public int? ImportId { get; set; }

        [StringLength(30)]
        public string Umiejetnosc { get; set; }
    }
}
