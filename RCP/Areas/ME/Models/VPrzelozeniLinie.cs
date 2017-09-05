namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VPrzelozeniLinie")]
    public partial class VPrzelozeniLinie
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPrzelozonego { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LiniaId { get; set; }

        public int? StrId { get; set; }

        [StringLength(255)]
        public string LiniaSymbol { get; set; }

        [StringLength(255)]
        public string LiniaNazwa { get; set; }
    }
}
