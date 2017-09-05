namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdWidocznosc")]
    public partial class obdWidocznosc
    {
        public int Id { get; set; }

        public int Wartosc { get; set; }

        public int IdKrokuDict { get; set; }

        public int IdPola { get; set; }

        public int IdRoliDict { get; set; }
    }
}
