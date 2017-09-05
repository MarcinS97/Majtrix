namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Frekwencja")]
    public partial class Frekwencja
    {
        [Key]
        public int Id_Frekwencja { get; set; }

        public int? Id_Pracownicy { get; set; }

        public int? Id_Akcja { get; set; }

        public int? Id_Str_Org { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Data_Akcji { get; set; }
    }
}
