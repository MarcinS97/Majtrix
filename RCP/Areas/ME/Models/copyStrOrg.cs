namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("copyStrOrg")]
    public partial class copyStrOrg
    {
        [Key]
        [Column(Order = 0)]
        public int Id_Str_Org { get; set; }

        public int? Id_Parent { get; set; }

        [StringLength(255)]
        public string Symb_Jedn { get; set; }

        [StringLength(255)]
        public string Nazwa_Jedn { get; set; }

        [StringLength(255)]
        public string Nazwa_JednEN { get; set; }

        public int? ID_Upr_Przel { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Typ { get; set; }
    }
}
