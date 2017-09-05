namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ONBStrZadania")]
    public partial class ONBStrZadania
    {
        public int Id { get; set; }

        public int IdZadania { get; set; }

        public int IdStrOrg { get; set; }
    }
}
