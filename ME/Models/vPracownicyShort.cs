namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vPracownicyShort")]
    public partial class vPracownicyShort
    {
        [StringLength(511)]
        public string Pracownik { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Pracownicy { get; set; }

        public int? LiniaId { get; set; }

        [StringLength(255)]
        public string LiniaSymbol { get; set; }

        public int? StrId { get; set; }

        [StringLength(255)]
        public string StrSymbol { get; set; }
    }
}
