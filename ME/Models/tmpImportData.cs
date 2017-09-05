namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tmpImportData")]
    public partial class tmpImportData
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Typ { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column("_Id", Order = 2)]
        public int C_Id { get; set; }

        public DateTime Created { get; set; }

        public int? Id1 { get; set; }

        public int? Id2 { get; set; }

        public int? Id3 { get; set; }

        public int? Id4 { get; set; }

        [StringLength(500)]
        public string Text1 { get; set; }

        [StringLength(500)]
        public string Text2 { get; set; }

        [StringLength(500)]
        public string Text3 { get; set; }

        [StringLength(500)]
        public string Text4 { get; set; }

        public int? Code1 { get; set; }

        public int? Code2 { get; set; }

        public int? Code3 { get; set; }

        public int? Code4 { get; set; }

        public DateTime? Data1 { get; set; }

        public DateTime? Data2 { get; set; }

        public DateTime? Data3 { get; set; }

        public DateTime? Data4 { get; set; }

        public double? Float1 { get; set; }

        public double? Float2 { get; set; }

        public double? Float3 { get; set; }

        public double? Float4 { get; set; }

        public bool? Bool1 { get; set; }

        public bool? Bool2 { get; set; }

        public bool? Bool3 { get; set; }

        public bool? Bool4 { get; set; }
    }
}
