namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PracownicyStrOrg")]
    public partial class PracownicyStrOrg
    {
        public int Id { get; set; }

        public int IdPracownika { get; set; }

        public int? IdStrOrg { get; set; }

        public int? IdParent { get; set; }

        public int? IdParentNew { get; set; }

        public DateTime Data { get; set; }
    }
}
