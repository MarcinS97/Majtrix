namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;

    [Table("obdPola")]
    public partial class obdPola
    {
        public ObiegDokumentow dbCtx = new ObiegDokumentow();

        public int Id { get; set; }

        public int IdSzablonu { get; set; }

        public int IdPolaDict { get; set; }

        public int IdObieguDict { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazwa { get; set; }

        [StringLength(100)]
        public string Grupa { get; set; }

        public int? MaxDlugosc { get; set; }

        public int? MinDlugosc { get; set; }

        [StringLength(500)]
        public string DozwoloneZnaki { get; set; }



    }
}
