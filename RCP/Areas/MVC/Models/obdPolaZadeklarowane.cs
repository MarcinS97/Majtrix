namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdPolaZadeklarowane")]
    public partial class obdPolaZadeklarowane
    {
        public int Id { get; set; }

        public string Nazwa { get; set; }

        public string Sql { get; set; }
    }
}