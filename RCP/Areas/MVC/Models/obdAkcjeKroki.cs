namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdAkcjeKroki")]
    public partial class obdAkcjeKroki
    {
        public int Id { get; set; }

        public int IdKrokuDict { get; set; }

        public int IdAkcjiDict { get; set; }

        public int Typ { get; set; }
    }
}
