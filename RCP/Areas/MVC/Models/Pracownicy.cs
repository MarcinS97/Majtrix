namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.SqlClient;

    [Table("Pracownicy")]
    public partial class Pracownicy
    {
        ObiegDokumentow ob = new ObiegDokumentow();

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Imie { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Nazwisko { get; set; }

        [StringLength(100)]
        public string Opis { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Mailing { get; set; }

        public int? IdDzialu { get; set; }

        public int? IdStanowiska { get; set; }

        public int? IdKierownika { get; set; }

        public int? IdProjektu { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Kierownik { get; set; }

        [StringLength(20)]
        public string KadryId { get; set; }

        [StringLength(20)]
        public string KadryId2 { get; set; }

        public int? Status { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Admin { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool Raporty { get; set; }

        [StringLength(200)]
        public string Rights { get; set; }

        [StringLength(50)]
        public string RightsPortal { get; set; }

        [StringLength(50)]
        public string Nick { get; set; }

        [StringLength(50)]
        public string Pass { get; set; }

        [StringLength(250)]
        public string Avatar { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoginType { get; set; }

        public DateTime? PassExpire { get; set; }

        public DateTime? DataZatr { get; set; }

        public DateTime? DataZwol { get; set; }

        [StringLength(200)]
        public string Stanowisko { get; set; }

        [StringLength(200)]
        public string NrKarty1 { get; set; }

        public void ZdejmijUprawnienie(int IdUprawnienia)
        {
            PracownicyUprawnienia aktualne = PracownicyUprawnienia.PobierzAktualne().Find( it => it.IdPracownika == Id && it.IdUprawnienia == IdUprawnienia );
            aktualne = ob.PracownicyUprawnienia.ToList().Find( it => it.Id == aktualne.Id );
            if(aktualne != null)
            {
                aktualne.Do = DateTime.Now;
                ob.SaveChanges();
            }
        }

        public string AktualneStanowisko
        {
            get
            {
                string query = @"select
	s.Nazwa
from Pracownicy pr
left join DM_HR_DB..PracownicyStanowiska ps on ps.IdPracownika = pr.Id
left join DM_HR_DB..Stanowiska s on s.Id = ps.IdStanowiska
where dbo.getdate(GETDATE()) between ps.Od and ISNULL(Do, '20990909') and s.Aktywne = 1 and pr.Id = @IdPracownika";

                string[] stanowiska = ob.Database.SqlQuery<string>(query, new SqlParameter("IdPracownika", Id)).ToArray();
                if(stanowiska.Length > 0)
                    return stanowiska[0];

                return null;
            }
        }
    }
}
