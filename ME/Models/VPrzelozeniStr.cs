namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VPrzelozeniStr")]
    public partial class VPrzelozeniStr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPrzelozonego { get; set; }

        public int? StrId { get; set; }

        public int? StrParentId { get; set; }

        [StringLength(255)]
        public string StrSymbol { get; set; }

        [StringLength(255)]
        public string StrNazwa { get; set; }
    }
}
