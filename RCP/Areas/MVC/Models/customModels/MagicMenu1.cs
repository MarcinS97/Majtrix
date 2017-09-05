namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class MagicMenu1
    {

        [StringLength(500)]
        public string _magic { get; set; }

        [StringLength(500)]
        public string _magic2 { get; set; }

        [StringLength(500)]
        public string command { get; set; }

        [StringLength(500)]
        public string Class { get; set; }

        [StringLength(500)]
        public string MenuText { get; set; }

        public int Level { get; set; }

        public int Avis { get; set; }

        public int Id { get; set; }

        [StringLength(500)]
        public string Par2 { get; set; }

        public int Sort { get; set; }

        [NotMapped]
        public SelectList LogList { get; set; }
    }
}
