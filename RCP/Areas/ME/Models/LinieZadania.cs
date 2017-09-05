namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LinieZadania")]
    public partial class LinieZadania
    {
        public int Id { get; set; }

        public int Id_Str_Org { get; set; }

        public int Id_Zadania { get; set; }

        public int? Id_Obszary { get; set; }

        public int? Id_Podobszary { get; set; }

        public int? Id_Maszyny { get; set; }

        public int? Id_Task { get; set; }

        public int? Id_Dzialanie { get; set; }

        public int? Id_Narzedzie { get; set; }

        public int? Id_Rodzaj { get; set; }

        public int? Id_Poziom { get; set; }

        public int? Id_Director { get; set; }

        [StringLength(20)]
        public string ProdAdm { get; set; }

        [StringLength(20)]
        public string Dyr { get; set; }

        public int? Lp { get; set; }

        [StringLength(20)]
        public string StrOrg { get; set; }

        public int? IdZadania1 { get; set; }
    }
}
