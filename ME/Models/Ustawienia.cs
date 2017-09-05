namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ustawienia")]
    public partial class Ustawienia
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public bool? Konserwacja { get; set; }

        [StringLength(100)]
        public string ADKontroler { get; set; }

        [StringLength(500)]
        public string ADPath { get; set; }

        [StringLength(100)]
        public string SMTPSerwer { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string AppAddr { get; set; }

        public int? IdSupervisor { get; set; }

        public DateTime? StartSystemu { get; set; }

        [StringLength(20)]
        public string Wersja { get; set; }
    }
}
