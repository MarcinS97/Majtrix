namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("copyStanowiska")]
    public partial class copyStanowiska
    {
        [Key]
        public int Id_Stanowiska { get; set; }

        [StringLength(50)]
        public string Nazwa_Stan { get; set; }

        [StringLength(255)]
        public string Nazwa_StanEN { get; set; }
    }
}
