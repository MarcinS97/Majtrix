namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;
    using System.Linq;

    [Table("dicUprawnienia")]
    public partial class dicUprawnienia
    {
        ObiegDokumentow ob = new ObiegDokumentow();
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nazwa { get; set; }

        [StringLength(500)]
        public string Opis { get; set; }

        [NotMapped]
        public bool? Dotyczy { get; set; }

        public void NadajPracownikowi(int IdPracownika)
        {
            PracownicyUprawnienia aktualne = PracownicyUprawnienia.PobierzAktualne().Find( it => it.IdPracownika == IdPracownika && it.IdUprawnienia == Id );
            if(aktualne == null)
            {
                PracownicyUprawnienia uprawnienia = new PracownicyUprawnienia();
                uprawnienia.IdPracownika = IdPracownika;
                uprawnienia.IdUprawnienia = Id;
                uprawnienia.Od = DateTime.Now;
                ob.PracownicyUprawnienia.Add(uprawnienia);
                ob.SaveChanges();
            }
        }
    }
}
