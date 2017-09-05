namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ServerCommands
    {
        public int Id { get; set; }

        public int Cmd { get; set; }

        public int Status { get; set; }

        [StringLength(500)]
        public string Msg1 { get; set; }

        [StringLength(2000)]
        public string Msg2 { get; set; }

        public int? AuthorId { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        [StringLength(500)]
        public string Par1 { get; set; }

        [StringLength(500)]
        public string Par2 { get; set; }

        [StringLength(500)]
        public string Par3 { get; set; }

        [StringLength(500)]
        public string Par4 { get; set; }

        [StringLength(500)]
        public string Par5 { get; set; }

        [StringLength(500)]
        public string Par6 { get; set; }
    }
}
