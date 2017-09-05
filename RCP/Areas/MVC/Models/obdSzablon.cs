namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("obdSzablon")]
    public partial class obdSzablon
    {

        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name="IdObieguDict")]
        public int IdObieguDict { get; set; }


        [StringLength(100)]
        [Display(Name = "Nazwa")]
        public string Nazwa { get; set; }


        [StringLength(500)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [AllowHtml]
        [Display(Name = "ContentHTML")]
        public string ContentHTML { get; set; }


    }
}
