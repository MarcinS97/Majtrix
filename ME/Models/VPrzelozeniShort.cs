namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VPrzelozeniShort")]
    public partial class VPrzelozeniShort
    {
        [Key]
        public int Id_Przelozeni { get; set; }

        [StringLength(511)]
        public string Kierownik { get; set; }
    }
}
