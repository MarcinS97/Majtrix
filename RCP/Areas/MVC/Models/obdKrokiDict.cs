namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdKrokiDict")]
    public partial class obdKrokiDict
    {
        public int Id { get; set; }

        public int IdRoliDict { get; set; }

        public int IdObieguDict { get; set; }

        [StringLength(500)]
        public string Opis { get; set; }

        public int MinAcc { get; set; }

        public int MaxRej { get; set; }

        public int Kolejnosc { get; set; }

        public int? CzasTrwania { get; set; }
    }
}
