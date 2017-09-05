namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBStrSzkolenia")]
    public partial class ONBStrSzkolenia
    {
        public int Id { get; set; }

        public int IdStrOrg { get; set; }

        public int IdSzkolenia { get; set; }
    }
}
