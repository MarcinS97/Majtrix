namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("obdPracownicyKroki")]
    public partial class obdPracownicyKroki
    {
        public int Id { get; set; }

        public int IdPracownika { get; set; }

        public int? IdZast { get; set; }

        public int IdKroku { get; set; }

        public int IdRoliDict { get; set; }

        public int? AccRej { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataAcc { get; set; }
    }
}
