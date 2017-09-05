namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VLinie")]
    public partial class VLinie
    {
        public int? StrId { get; set; }

        [StringLength(255)]
        public string StrSymbol { get; set; }

        [StringLength(255)]
        public string StrNazwa { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LiniaId { get; set; }

        [StringLength(255)]
        public string LiniaSymbol { get; set; }

        [StringLength(255)]
        public string LiniaNazwa { get; set; }
    }
}
