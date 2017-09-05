namespace HRRcp.Areas.MVC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ObiegDokumentow : DbContext
    {
        public ObiegDokumentow()
            : base("name=PORTAL")
        {
        }

        public virtual DbSet<dicUprawnienia> dicUprawnienia { get; set; }
        public virtual DbSet<obdAkcjeDict> obdAkcjeDict { get; set; }
        public virtual DbSet<obdAkcjeKroki> obdAkcjeKroki { get; set; }
        public virtual DbSet<obdFiltrPoleDict> obdFiltrPoleDict { get; set; }
        public virtual DbSet<obdFiltrPoleWartosci> obdFiltrPoleWartosci { get; set; }
        public virtual DbSet<obdKroki> obdKroki { get; set; }
        public virtual DbSet<obdKrokiDict> obdKrokiDict { get; set; }
        public virtual DbSet<obdObieg> obdObieg { get; set; }
        public virtual DbSet<obdObiegDict> obdObiegDict { get; set; }
        public virtual DbSet<obdPola> obdPola { get; set; }
        public virtual DbSet<obdPolaDict> obdPolaDict { get; set; }
        public virtual DbSet<obdPracownicyKroki> obdPracownicyKroki { get; set; }
        public virtual DbSet<obdRoleDict> obdRoleDict { get; set; }
        public virtual DbSet<obdSzablon> obdSzablon { get; set; }
        public virtual DbSet<obdWartosciPola> obdWartosciPola { get; set; }
        public virtual DbSet<obdWidocznosc> obdWidocznosc { get; set; }
        public virtual DbSet<Pracownicy> Pracownicy { get; set; }
        public virtual DbSet<PracownicyUprawnienia> PracownicyUprawnienia { get; set; }
        public virtual DbSet<SqlContent> SqlContent { get; set; }
        public virtual DbSet<SqlMenu> SqlMenu { get; set; }
        public virtual DbSet<obdPolaZadeklarowane> obdPolaZadeklarowane { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SqlContent>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<SqlMenu>()
                .Property(e => e.Rights)
                .IsUnicode(false);
            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.KadryId)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.KadryId2)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.RightsPortal)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Pass)
                .IsUnicode(false);
        }
    }
}
