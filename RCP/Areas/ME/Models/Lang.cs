namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lang")]
    public partial class Lang
    {
        [Key]
        [Column("Lang", Order = 0)]
        [StringLength(20)]
        public string Lang1 { get; set; }

        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Msg { get; set; }

        [StringLength(2000)]
        public string Trans { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(16)]
        public byte[] MsgHash { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
