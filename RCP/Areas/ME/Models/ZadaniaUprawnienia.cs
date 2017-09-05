namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZadaniaUprawnienia")]
    public partial class ZadaniaUprawnienia
    {
        public int Id { get; set; }

        public int IdZadania { get; set; }

        public int IdUprawnienia { get; set; }

        public DateTime Od { get; set; }

        public DateTime? Do { get; set; }
    }
}
