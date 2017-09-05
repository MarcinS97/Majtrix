namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailingGrupy")]
    public partial class MailingGrupy
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Grupa { get; set; }

        [StringLength(200)]
        public string Opis { get; set; }

        [StringLength(2000)]
        public string ZnacznikiSql { get; set; }

        [StringLength(2000)]
        public string ZnacznikiSqlTest { get; set; }
    }
}
