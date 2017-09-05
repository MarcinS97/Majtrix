namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SqlMenu")]
    public partial class SqlMenu
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Grupa { get; set; }

        public int? ParentId { get; set; }

        [StringLength(200)]
        public string MenuText { get; set; }

        [StringLength(200)]
        public string MenuTextEN { get; set; }

        [StringLength(500)]
        public string ToolTip { get; set; }

        [StringLength(500)]
        public string ToolTipEN { get; set; }

        [StringLength(500)]
        public string Command { get; set; }

        public int? Kolejnosc { get; set; }

        public bool Aktywny { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(200)]
        public string Rights { get; set; }

        [StringLength(200)]
        public string Par1 { get; set; }

        [StringLength(200)]
        public string Par2 { get; set; }

        public bool Wydruk { get; set; }

        public string Sql { get; set; }

        public string SqlParams { get; set; }

        public int? Mode { get; set; }

        public string Javascript { get; set; }

        [StringLength(200)]
        public string Class { get; set; }
    }
}
