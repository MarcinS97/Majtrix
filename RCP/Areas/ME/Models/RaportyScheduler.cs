namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaportyScheduler")]
    public partial class RaportyScheduler
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public int IdPracownika { get; set; }

        public int IdRaportu { get; set; }

        [StringLength(2000)]
        public string Parametry { get; set; }

        public int Typ { get; set; }

        [StringLength(2000)]
        public string cc { get; set; }

        [StringLength(2000)]
        public string bcc { get; set; }

        public DateTime DataStartu { get; set; }

        public DateTime? DataStopu { get; set; }

        [StringLength(20)]
        public string InterwalTyp { get; set; }

        public int? Interwal { get; set; }

        [StringLength(2000)]
        public string InterwalSql { get; set; }

        public DateTime NextStart { get; set; }

        public int Status { get; set; }

        public bool Aktywny { get; set; }

        public DateTime? LastStart { get; set; }

        public DateTime? LastStop { get; set; }

        [StringLength(1000)]
        public string LastError { get; set; }
    }
}
