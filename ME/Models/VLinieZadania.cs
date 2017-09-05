namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VLinieZadania")]
    public partial class VLinieZadania
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Str_Org { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Zadania { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(511)]
        public string NazwaZadania { get; set; }

        public int? Waga { get; set; }
    }
}
