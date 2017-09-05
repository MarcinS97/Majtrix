namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UprawnieniaGrupySpecyfikacja")]
    public partial class UprawnieniaGrupySpecyfikacja
    {
        public int Id { get; set; }

        public int IdGrupy { get; set; }

        public int IdUprawnienia { get; set; }

        public DateTime Od { get; set; }

        public DateTime? Do { get; set; }
    }
}
