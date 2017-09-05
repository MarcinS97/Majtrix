namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Scheduler")]
    public partial class Scheduler
    {
        public int Id { get; set; }

        [StringLength(25)]
        public string Typ { get; set; }

        [StringLength(25)]
        public string Grupa { get; set; }

        public string SQL { get; set; }

        public bool Aktywny { get; set; }

        public string Komentarz { get; set; }

        public int? Kolejnosc { get; set; }

        [StringLength(20)]
        public string Wersja { get; set; }

        public DateTime? WersjaData { get; set; }

        public int? AutorId { get; set; }
    }
}
