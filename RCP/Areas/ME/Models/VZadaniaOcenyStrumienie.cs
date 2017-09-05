namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VZadaniaOcenyStrumienie")]
    public partial class VZadaniaOcenyStrumienie
    {
        public int? StrId { get; set; }

        [StringLength(255)]
        public string StrSymbol { get; set; }

        [StringLength(255)]
        public string StrNazwa { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Zadania { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(511)]
        public string NazwaZadania { get; set; }

        public int? Waga { get; set; }

        [StringLength(50)]
        public string GrOperSymbol { get; set; }

        [StringLength(255)]
        public string GrOperNazwa { get; set; }

        public int? Ocena3 { get; set; }

        public int? Ocena3sm { get; set; }

        public int? Ocena2 { get; set; }

        public int? Ocena2sm { get; set; }

        public int? Ocena1 { get; set; }

        public int? Ocena1sm { get; set; }

        public int? Ocena0 { get; set; }

        public int? Ocena0sm { get; set; }

        public int? IloscNaStr { get; set; }
    }
}
