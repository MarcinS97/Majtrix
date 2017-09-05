namespace HRRcp.Areas.ME.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MatrycaMVC : DbContext
    {
        public MatrycaMVC()
            : base("name=HRConnectionString")
        {
        }

        public virtual DbSet<Absencja> Absencja { get; set; }
        public virtual DbSet<AbsencjaKody> AbsencjaKody { get; set; }
        public virtual DbSet<AbsencjeDlugotrwale> AbsencjeDlugotrwale { get; set; }
        public virtual DbSet<Akcja> Akcja { get; set; }
        public virtual DbSet<Certyfikaty> Certyfikaty { get; set; }
        public virtual DbSet<CertyfikatySpaw> CertyfikatySpaw { get; set; }
        public virtual DbSet<Cmd> Cmd { get; set; }
        public virtual DbSet<Frekwencja> Frekwencja { get; set; }
        public virtual DbSet<GrOperacji> GrOperacji { get; set; }
        public virtual DbSet<GrZatr> GrZatr { get; set; }
        public virtual DbSet<ImportData> ImportData { get; set; }
        public virtual DbSet<Kody> Kody { get; set; }
        public virtual DbSet<Komp> Komp { get; set; }
        public virtual DbSet<KompOdpowiedzi> KompOdpowiedzi { get; set; }
        public virtual DbSet<KompStan> KompStan { get; set; }
        public virtual DbSet<KompWskazniki> KompWskazniki { get; set; }
        public virtual DbSet<KompWskaznikiOceny> KompWskaznikiOceny { get; set; }
        public virtual DbSet<KryteriaOceny> KryteriaOceny { get; set; }
        public virtual DbSet<Lang> Lang { get; set; }
        public virtual DbSet<LinieZadania> LinieZadania { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Mailing> Mailing { get; set; }
        public virtual DbSet<MailingGrupy> MailingGrupy { get; set; }
        public virtual DbSet<MailingZnaczniki> MailingZnaczniki { get; set; }
        public virtual DbSet<MDzialania> MDzialania { get; set; }
        public virtual DbSet<MMaszyny> MMaszyny { get; set; }
        public virtual DbSet<MNarzedzia> MNarzedzia { get; set; }
        public virtual DbSet<MObszary> MObszary { get; set; }
        public virtual DbSet<Monity> Monity { get; set; }
        public virtual DbSet<MPodobszary> MPodobszary { get; set; }
        public virtual DbSet<MPoziomy> MPoziomy { get; set; }
        public virtual DbSet<MRodzaje> MRodzaje { get; set; }
        public virtual DbSet<MZadania> MZadania { get; set; }
        public virtual DbSet<Oceny> Oceny { get; set; }
        public virtual DbSet<OcenyNazwy> OcenyNazwy { get; set; }
        public virtual DbSet<Oddelegowania> Oddelegowania { get; set; }
        public virtual DbSet<ONBKartaSekcje> ONBKartaSekcje { get; set; }
        public virtual DbSet<ONBKarty> ONBKarty { get; set; }
        public virtual DbSet<ONBMiejscaSzkolen> ONBMiejscaSzkolen { get; set; }
        public virtual DbSet<ONBStrSzkolenia> ONBStrSzkolenia { get; set; }
        public virtual DbSet<ONBStrZadania> ONBStrZadania { get; set; }
        public virtual DbSet<ONBSzkoleniaGrupy> ONBSzkoleniaGrupy { get; set; }
        public virtual DbSet<ONBSzkoleniaPracownicy> ONBSzkoleniaPracownicy { get; set; }
        public virtual DbSet<ONBSzkoleniaTerminy> ONBSzkoleniaTerminy { get; set; }
        public virtual DbSet<ONBSzkoleniaTrenerzy> ONBSzkoleniaTrenerzy { get; set; }
        public virtual DbSet<ONBSzkoleniaTypy> ONBSzkoleniaTypy { get; set; }
        public virtual DbSet<ONBZadania> ONBZadania { get; set; }
        public virtual DbSet<ONBZaliczenia> ONBZaliczenia { get; set; }
        public virtual DbSet<PassSkills> PassSkills { get; set; }
        public virtual DbSet<Pkt> Pkt { get; set; }
        public virtual DbSet<Pracownicy> Pracownicy { get; set; }
        public virtual DbSet<Pracownicy_> Pracownicy_ { get; set; }
        public virtual DbSet<PracownicyHistoria> PracownicyHistoria { get; set; }
        public virtual DbSet<PracownicyStrOrg> PracownicyStrOrg { get; set; }
        public virtual DbSet<Priorytet> Priorytet { get; set; }
        public virtual DbSet<Produktywnosc> Produktywnosc { get; set; }
        public virtual DbSet<Programy> Programy { get; set; }
        public virtual DbSet<ProgramyAnkiety> ProgramyAnkiety { get; set; }
        public virtual DbSet<ProgramyOceny> ProgramyOceny { get; set; }
        public virtual DbSet<ProgramyZadania> ProgramyZadania { get; set; }
        public virtual DbSet<Przelozeni_x> Przelozeni_x { get; set; }
        public virtual DbSet<RaportyScheduler> RaportyScheduler { get; set; }
        public virtual DbSet<Scheduler> Scheduler { get; set; }
        public virtual DbSet<ServerCommands> ServerCommands { get; set; }
        public virtual DbSet<SqlContent> SqlContent { get; set; }
        public virtual DbSet<ME_SqlMenu> SqlMenu { get; set; }
        public virtual DbSet<Stanowiska> Stanowiska { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<StatusPrac> StatusPrac { get; set; }
        public virtual DbSet<StrOrg> StrOrg { get; set; }
        public virtual DbSet<StrukturaPrzelozeni> StrukturaPrzelozeni { get; set; }
        public virtual DbSet<Teksty> Teksty { get; set; }
        public virtual DbSet<tmpImportData> tmpImportData { get; set; }
        public virtual DbSet<Uprawnienia> Uprawnienia { get; set; }
        public virtual DbSet<UprawnieniaGrupySpecyfikacja> UprawnieniaGrupySpecyfikacja { get; set; }
        public virtual DbSet<UprawnieniaKwalifikacje> UprawnieniaKwalifikacje { get; set; }
        public virtual DbSet<UprawnieniaTypy> UprawnieniaTypy { get; set; }
        public virtual DbSet<UserParams> UserParams { get; set; }
        public virtual DbSet<Ustawienia> Ustawienia { get; set; }
        public virtual DbSet<Wagi> Wagi { get; set; }
        public virtual DbSet<Widelki> Widelki { get; set; }
        public virtual DbSet<Zadania> Zadania { get; set; }
        public virtual DbSet<ZadaniaGrupy> ZadaniaGrupy { get; set; }
        public virtual DbSet<ZadaniaGrupyTypy> ZadaniaGrupyTypy { get; set; }
        public virtual DbSet<ZadaniaGrupyZadania> ZadaniaGrupyZadania { get; set; }
        public virtual DbSet<ZadaniaTypy> ZadaniaTypy { get; set; }
        public virtual DbSet<ZadaniaUprawnienia> ZadaniaUprawnienia { get; set; }
        public virtual DbSet<Zastepstwa> Zastepstwa { get; set; }
        public virtual DbSet<copyPracownicy> copyPracownicy { get; set; }
        public virtual DbSet<copyPrzelozeni> copyPrzelozeni { get; set; }
        public virtual DbSet<copyStanowiska> copyStanowiska { get; set; }
        public virtual DbSet<copyStrOrg> copyStrOrg { get; set; }
        public virtual DbSet<copyStrukturaPrzelozeni> copyStrukturaPrzelozeni { get; set; }

        public virtual DbSet<ZadaniaStanowiska> ZadaniaStanowiska { get; set; }
        public virtual DbSet<Przelozeni> Przelozeni { get; set; }
        public virtual DbSet<VCertyfikaty> VCertyfikaty { get; set; }
        public virtual DbSet<VCertyfikatyUprawnienie> VCertyfikatyUprawnienie { get; set; }
        public virtual DbSet<VCertyfikatyUprawnienie2> VCertyfikatyUprawnienie2 { get; set; }
        public virtual DbSet<VLinie> VLinie { get; set; }
        public virtual DbSet<VLinieOddelegowania> VLinieOddelegowania { get; set; }
        public virtual DbSet<VLinieZadania> VLinieZadania { get; set; }
        public virtual DbSet<VLinieZadania3> VLinieZadania3 { get; set; }
        public virtual DbSet<VMaszynyZadania> VMaszynyZadania { get; set; }
        public virtual DbSet<VOceny> VOceny { get; set; }
        public virtual DbSet<VOddelegowaniaNaDzis> VOddelegowaniaNaDzis { get; set; }
        public virtual DbSet<VPracOceny> VPracOceny { get; set; }
        public virtual DbSet<VPracOcenyLinia> VPracOcenyLinia { get; set; }
        public virtual DbSet<VPracOcenyLinia_v1> VPracOcenyLinia_v1 { get; set; }
        public virtual DbSet<VPracOcenyStr> VPracOcenyStr { get; set; }
        public virtual DbSet<VPracOddelegowania> VPracOddelegowania { get; set; }
        public virtual DbSet<vPracownicy> vPracownicy { get; set; }
        public virtual DbSet<vPracownicyShort> vPracownicyShort { get; set; }
        public virtual DbSet<VPrzelozeniLinie> VPrzelozeniLinie { get; set; }
        public virtual DbSet<VPrzelozeniOceny> VPrzelozeniOceny { get; set; }
        public virtual DbSet<VPrzelozeniShort> VPrzelozeniShort { get; set; }
        public virtual DbSet<VPrzelozeniStr> VPrzelozeniStr { get; set; }
        public virtual DbSet<VPrzelozeniStrLinie> VPrzelozeniStrLinie { get; set; }
        public virtual DbSet<VZadania> VZadania { get; set; }
        public virtual DbSet<VZadaniaOceny> VZadaniaOceny { get; set; }
        public virtual DbSet<VZadaniaOcenyStrumienie> VZadaniaOcenyStrumienie { get; set; }
        public virtual DbSet<VZadaniaPracownicy> VZadaniaPracownicy { get; set; }
        public virtual DbSet<VZadaniaTyp> VZadaniaTyp { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbsencjaKody>()
                .Property(e => e.Kolor)
                .IsUnicode(false);

            modelBuilder.Entity<AbsencjaKody>()
                .Property(e => e.KolorPU)
                .IsUnicode(false);

            modelBuilder.Entity<AbsencjaKody>()
                .Property(e => e.Typ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AbsencjaKody>()
                .Property(e => e.Kod2)
                .IsUnicode(false);

            modelBuilder.Entity<Akcja>()
                .Property(e => e.Akcja1)
                .IsUnicode(false);

            modelBuilder.Entity<Cmd>()
                .Property(e => e.Cmd1)
                .IsUnicode(false);

            modelBuilder.Entity<Cmd>()
                .Property(e => e.AuthKey)
                .IsUnicode(false);

            modelBuilder.Entity<GrOperacji>()
                .Property(e => e.Symbol)
                .IsUnicode(false);

            modelBuilder.Entity<GrOperacji>()
                .Property(e => e.Nazwa)
                .IsUnicode(false);

            modelBuilder.Entity<ImportData>()
                .Property(e => e.Typ)
                .IsUnicode(false);

            modelBuilder.Entity<Lang>()
                .Property(e => e.Lang1)
                .IsUnicode(false);

            modelBuilder.Entity<LinieZadania>()
                .Property(e => e.ProdAdm)
                .IsUnicode(false);

            modelBuilder.Entity<LinieZadania>()
                .Property(e => e.Dyr)
                .IsUnicode(false);

            modelBuilder.Entity<LinieZadania>()
                .Property(e => e.StrOrg)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Par)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Mailing>()
                .Property(e => e.Typ)
                .IsUnicode(false);

            modelBuilder.Entity<Mailing>()
                .Property(e => e.Grupa)
                .IsUnicode(false);

            modelBuilder.Entity<MailingGrupy>()
                .Property(e => e.Grupa)
                .IsUnicode(false);

            modelBuilder.Entity<MailingZnaczniki>()
                .Property(e => e.Znacznik)
                .IsUnicode(false);

            modelBuilder.Entity<MDzialania>()
                .Property(e => e.IdParents)
                .IsUnicode(false);

            modelBuilder.Entity<MMaszyny>()
                .Property(e => e.IdParents)
                .IsUnicode(false);

            modelBuilder.Entity<MMaszyny>()
                .Property(e => e.IdParents2)
                .IsUnicode(false);

            modelBuilder.Entity<MNarzedzia>()
                .Property(e => e.IdParents)
                .IsUnicode(false);

            modelBuilder.Entity<MNarzedzia>()
                .Property(e => e.IdParents2)
                .IsUnicode(false);

            modelBuilder.Entity<Monity>()
                .Property(e => e.Typ)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MPodobszary>()
                .Property(e => e.IdParents)
                .IsUnicode(false);

            modelBuilder.Entity<MPoziomy>()
                .Property(e => e.IdParents)
                .IsUnicode(false);

            modelBuilder.Entity<OcenyNazwy>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<OcenyNazwy>()
                .Property(e => e.BkColor)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy>()
                .Property(e => e.Plec)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.KadryId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.Stawka)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.Pass)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.NrKarty1)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.NrKarty2)
                .IsUnicode(false);

            modelBuilder.Entity<Pracownicy_>()
                .Property(e => e.KadryId2)
                .IsUnicode(false);

            modelBuilder.Entity<Priorytet>()
                .Property(e => e.Opis)
                .IsUnicode(false);

            modelBuilder.Entity<Przelozeni_x>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<Przelozeni_x>()
                .Property(e => e.x_IdsStrOrg)
                .IsUnicode(false);

            modelBuilder.Entity<Przelozeni_x>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<RaportyScheduler>()
                .Property(e => e.InterwalTyp)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.Typ)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.Grupa)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.Wersja)
                .IsUnicode(false);

            modelBuilder.Entity<ME_SqlMenu>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<Stanowiska>()
                .Property(e => e.Nazwa_Stan)
                .IsUnicode(false);

            modelBuilder.Entity<StatusPrac>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<StatusPrac>()
                .HasMany(e => e.Pracownicy)
                .WithOptional(e => e.StatusPrac)
                .HasForeignKey(e => e.Id_Status);

            modelBuilder.Entity<StrOrg>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<StrOrg>()
                .HasMany(e => e.Pracownicy)
                .WithOptional(e => e.StrOrg)
                .HasForeignKey(e => e.Id_Str_OrgM);

            modelBuilder.Entity<Teksty>()
                .Property(e => e.Typ)
                .IsUnicode(false);

            modelBuilder.Entity<tmpImportData>()
                .Property(e => e.Typ)
                .IsUnicode(false);

            modelBuilder.Entity<UserParams>()
                .Property(e => e.Typ)
                .IsUnicode(false);

            modelBuilder.Entity<Zadania>()
                .Property(e => e.ProdAdm1)
                .IsUnicode(false);

            modelBuilder.Entity<Zadania>()
                .Property(e => e.Dyr1)
                .IsUnicode(false);

            modelBuilder.Entity<Zadania>()
                .Property(e => e.StrOrg1)
                .IsUnicode(false);

            modelBuilder.Entity<copyPracownicy>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<copyPracownicy>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<copyPracownicy>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<copyPracownicy>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<copyPracownicy>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<copyPrzelozeni>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<copyPrzelozeni>()
                .Property(e => e.x_IdsStrOrg)
                .IsUnicode(false);

            modelBuilder.Entity<copyPrzelozeni>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<copyStanowiska>()
                .Property(e => e.Nazwa_Stan)
                .IsUnicode(false);


            modelBuilder.Entity<Przelozeni>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Przelozeni>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<Przelozeni>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Przelozeni>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<Przelozeni>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<VMaszynyZadania>()
                .Property(e => e.IdParents)
                .IsUnicode(false);

            modelBuilder.Entity<VMaszynyZadania>()
                .Property(e => e.IdParents2)
                .IsUnicode(false);

            modelBuilder.Entity<VMaszynyZadania>()
                .Property(e => e.ProdAdm1)
                .IsUnicode(false);

            modelBuilder.Entity<VMaszynyZadania>()
                .Property(e => e.Dyr1)
                .IsUnicode(false);

            modelBuilder.Entity<VMaszynyZadania>()
                .Property(e => e.StrOrg1)
                .IsUnicode(false);

            modelBuilder.Entity<VOceny>()
                .Property(e => e.GrOperacjiSymbol)
                .IsUnicode(false);

            modelBuilder.Entity<VOceny>()
                .Property(e => e.GrOperacjiNazwa)
                .IsUnicode(false);

            modelBuilder.Entity<VPracOddelegowania>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<VPracOddelegowania>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<VPracOddelegowania>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<VPracOddelegowania>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<VPracOddelegowania>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<vPracownicy>()
                .Property(e => e.Nazwa_Stan)
                .IsUnicode(false);

            modelBuilder.Entity<vPracownicy>()
                .Property(e => e.Status_obecności)
                .IsUnicode(false);

            modelBuilder.Entity<VZadania>()
                .Property(e => e.GrOperacjiSymbol)
                .IsUnicode(false);

            modelBuilder.Entity<VZadania>()
                .Property(e => e.GrOperacjiNazwa)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaOceny>()
                .Property(e => e.GrOperSymbol)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaOceny>()
                .Property(e => e.GrOperNazwa)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaOcenyStrumienie>()
                .Property(e => e.GrOperSymbol)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaOcenyStrumienie>()
                .Property(e => e.GrOperNazwa)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.Nick)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.Rights)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.ProdAdm)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.Dyr)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaPracownicy>()
                .Property(e => e.StrOrg)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaTyp>()
                .Property(e => e.ProdAdm1)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaTyp>()
                .Property(e => e.Dyr1)
                .IsUnicode(false);

            modelBuilder.Entity<VZadaniaTyp>()
                .Property(e => e.StrOrg1)
                .IsUnicode(false);
        }
    }
}
