namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pracownicy_
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Imie { get; set; }

        [Required]
        [StringLength(50)]
        public string Nazwisko { get; set; }

        [StringLength(100)]
        public string Opis { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public bool Mailing { get; set; }

        public int? IdDzialu { get; set; }

        public int? IdStanowiska { get; set; }

        public int? IdKierownika { get; set; }

        public int? IdProjektu { get; set; }

        public bool Admin { get; set; }

        public bool Kierownik { get; set; }

        public bool Raporty { get; set; }

        [StringLength(5)]
        public string KadryId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Stawka { get; set; }

        public int? RcpId { get; set; }

        public int? Status { get; set; }

        public int? RcpStrefaId { get; set; }

        public int? RcpAlgorytm { get; set; }

        public DateTime Created { get; set; }

        public DateTime? DataZatr { get; set; }

        public int? EtatL { get; set; }

        public int? EtatM { get; set; }

        [StringLength(500)]
        public string Info { get; set; }

        [StringLength(200)]
        public string CCInfo { get; set; }

        [StringLength(100)]
        public string Rights { get; set; }

        public int? GrSplitu { get; set; }

        public int? IdLinii { get; set; }

        [StringLength(50)]
        public string Nick { get; set; }

        [StringLength(50)]
        public string Pass { get; set; }

        [StringLength(50)]
        public string NrKarty1 { get; set; }

        [StringLength(50)]
        public string NrKarty2 { get; set; }

        public DateTime? DataZwol { get; set; }

        [StringLength(20)]
        public string KadryId2 { get; set; }

        public DateTime? PassExpire { get; set; }

        public DateTime? OkresProbnyDo { get; set; }
    }
}
