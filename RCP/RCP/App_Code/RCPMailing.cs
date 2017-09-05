using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace HRRcp.RCP.App_Code
{
    public class RCPMailing
    {
        //public const string maMS_ANK_PRAC_CREATE = "MS_ANK_PRAC_CREATE"; // Utworzono ankiete pracownika
        //public const string maMS_ANK_KIER_CREATE = "MS_ANK_KIER_CREATE"; // Utworzono ankiete kierownika

        public const string maRCP_PP_ACC = "RCP_PP_ACC";
        public const string maRCP_NADG_WN = "RCP_NADG_WN";

        const bool TEST = false;

        public static bool EventRCP(string maTyp, string id, string akcja, string[] userIds)
        {
            bool ok = true;
            foreach (String userId in userIds)
            {
                ok &= EventRCP(maTyp, id, akcja, userId);
            }
            return ok;
        }

        //public static bool EventB2B(string maTyp, string id, string akcja, string userId)
        //{
        //    return EventMS(maTyp, id, akcja, userId, null, "");
        //}

        public static bool EventRCP(string maTyp, string id, string akcja, string userId/*, PDF att, string html*/) // id = idZamowienia lub idPozycji
        {
            try
            {
                String Filename = "";
                DataSet mail_data = null;
                FileStream fs = null;
                switch (maTyp)
                {
                    case maRCP_PP_ACC:
                        mail_data = GetDanePlanPracy(id, akcja, false);
                        Mailing.CheckSendMail(maTyp, null, AppUser.GetData(userId), mail_data);
                        break;
                    
                    case maRCP_NADG_WN:
                        mail_data = GetDaneWniosekNadg(id, false);
                        Mailing.CheckSendMail(maTyp, null, AppUser.GetData(userId), mail_data);
                        break;

                    //case maMS_ANK_PRAC_CREATE:
                    //    mail_data = GetDaneAnkieta(id, TEST);
                    //    Mailing.CheckSendMail(maTyp, null, AppUser.GetData(userId), mail_data);
                    //    break;

                    //case maMS_ANK_KIER_CREATE:
                    //    mail_data = GetDaneAnkieta(id, TEST);
                    //    Mailing.CheckSendMail(maTyp, null, AppUser.GetData(userId), mail_data);
                    //    break;

                    //case maB2B_RPRZ_PAR:
                    //case maB2B_RUZN_PAR:
                    //case maB2B_RODR_PAR:
                    //    Mailing.CheckSendMail(maTyp, null, AppUser._GetData(userId), null, mail_data);
                    //    break;
                    //case maB2B_ZPZA_PAR:
                    //case maB2B_ZPZM_PAR:
                    //    /* case maB2B_ZWYS_PAR: */
                    //    mail_data = GetDanePozycji(id, false);
                    //    Mailing.CheckSendMail(maTyp, null, AppUser._GetData(userId), null, mail_data);
                    //    break;
                    //case maB2B_ZWYS_PAR: /* if tylko zam */
                    //case maB2B_ZZAT_PAR:
                    //    mail_data = GetDaneZamowienia(id, false);
                    //    Mailing.CheckSendMail(maTyp, null, AppUser._GetData(userId), null, mail_data);
                    //    break;
                    //case maB2B_ZAPUTW_PRA:
                    //case maB2B_ZAPODR_PAR:
                    //    mail_data = GetDaneZapytania(id, false);
                    //    Mailing.CheckSendMail(maTyp, null, AppUser._GetData(userId), null, mail_data);
                    //    break;
                    //case maB2B_OODR_PRA:
                    //case maB2B_OWYS_PAR:
                    //case maB2B_OZAM_PRA:
                    //case maB2B_ONEG_PRA:
                    //    mail_data = GetDaneOferty(id, false);
                    //    Filename = att.Download(html, System.Web.HttpContext.Current.Server, "Oferta_" + id);
                    //    fs = new FileStream(Filename, FileMode.Open);
                    //    Mailing.CheckSendMail(maTyp, null, AppUser._GetData(userId), null, mail_data, fs, String.Format("Oferta_{0}.pdf", id), "application/pdf");
                    //    break;
                    //case maB2B_UKAR_PAR:
                    //    mail_data = GetDaneKary(id, false);
                    //    Filename = att.Download(html, System.Web.HttpContext.Current.Server, "KaraUmowna_" + id);
                    //    fs = new FileStream(Filename, FileMode.Open);
                    //    Mailing.CheckSendMail(maTyp, null, AppUser._GetData(userId), null, mail_data, fs, String.Format("KaraUmowna_{0}.pdf", id), "application/pdf");
                    //    break;
                    default:
                        break;

                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventRCP", ex.Message);
                return false;
            }
        }

        public static DataSet GetDanePlanPracy(String Id, String Action, Boolean Test)
        {
            return db.Select.Set(@"
select
  2 COUNT
, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') PRZELOZONY
, (select dbo.cat(p2.Nazwisko + ' ' + p2.Imie + ISNULL(' (' + p2.KadryId + ')', ''), '
', 0) from Pracownicy p2 where p2.Id in ({1})) PRACOWNICY
from Pracownicy p
where p.Id = {0}
", Id, Action);
        }

        public static DataSet GetDaneWniosekNadg(String Id, Boolean Test)
        {
            return db.Select.Set(@"
select
  1 COUNT
, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') PRACOWNIK
from rcpNadgodzinyWnioski nw
left join Pracownicy p on p.Id = nw.IdPracownika
where nw.Id = {0}
",  Id);
        }

//        public static DataSet GetDaneAnkieta(String Id, Boolean Test)
//        {
//            return db.Select.Set(@"
//select
//5 COUNT
//, p.Nazwisko + ' ' + p.Imie +  isnull(' (' + p.KadryId + ')', '') PRACOWNIK
//, u.Nazwa UPRAWNIENIE
//, case when a.Typ = 0 then 'Ankieta uczestnika' else 'Ankieta przełożonego' end as ANKIETATYP
//, a.Id ANKIETAID
//, '{1}' APPLINK
//from msAnkiety a
//left join Certyfikaty c on c.Id = a.IdCertyfikatu
//left join Uprawnienia u on u.Id = c.IdUprawnienia
//left join Pracownicy p on p.Id = c.IdPracownika
//where a.Id = {0}", Id, Tools.GetAppAddr2(null));
//        }

//        public static DataSet GetDaneReklamacji(String IdReklamacji, Boolean Test)
//        {
//            if (Test) return Base.getDataSet(@"select 1 COUNT, '1337' NRFAK");
//            else return Base.getDataSet(String.Format(@"
//select 1 COUNT
//, NrFaktury NRFAK
//from REKL_Reklamacje r
//where r.Id = {0}
//", IdReklamacji));
//        }

//        public static DataSet GetDanePozycji(String IdPozycji, Boolean Test)
//        {
//            if (Test) return Base.getDataSet(@"select 7 COUNT, '1000 10' KOD, 'szpiralborer' NAZWA, 'Irson Incorporated' DOSTAWCA, GETDATE() DATADOST, '1337' NRZAM, '8001' NRZAMC, 'Irson Incorporated' KONTRAHENT");
//            else return Base.getDataSet(String.Format(@"
//select 7 COUNT
//, zp.NrKatalogowy + ' ' + zp.Rozmiar KOD
//, zp.NazwaSkrocona NAZWA
//, zp.DostawcaWyswietlanyPartnerowi DOSTAWCA
//, zp.DataDostawy DATADOST
//, z.NrZamowienia NRZAM
//, z.NrZamowieniaKlienta NRZAMC
//, k.NazwaSkrocona KONTRAHENT
//from
//ZAM_ZamowieniaPozycje zp
//left join ZAM_Zamowienia z on z.Id = zp.ZamowieniaId
//left join Kontrahenci k on k.Id = z.KontrahentId
///*where zp.Id in ({0}) --odremowac na wypadek wypisywania kilku elementow
//                       --poza tym: - dodac dbo.cat() do wszystkich stringow i MAX() do stalych elementow liczbowych
//                       --          - odremowac string buildery kolo zmiennych universe
//--*/where zp.Id = {0}
//", IdPozycji));
//        }

//        public static DataSet GetDaneZamowienia(String IdZamowienia, Boolean Test)
//        {
//            if (Test) return Base.getDataSet(@"select 3 COUNT, '1337' NRZAM, '8001' NRZAMC, 'Irson Incorporated' KONTRAHENT");
//            else return Base.getDataSet(String.Format(@"
//select 3 COUNT
//, z.NrZamowienia NRZAM
//, z.NrZamowieniaKlienta NRZAMC
//, k.NazwaSkrocona KONTRAHENT
//from ZAM_Zamowienia z
//left join Kontrahenci k on k.Id = z.KontrahentId
//where z.Id = {0}
//", IdZamowienia));
//        }

//        public static DataSet GetDaneZapytania(String IdZapytania, Boolean Test)
//        {
//            if (Test) return Base.getDataSet("select 3 COUNT, '1337' NRZAP, '8001' NROFERTY, 'Irson Incorporated' KONTRAHENT");
//            else return Base.getDataSet(String.Format(@"
//select 3 COUNT,
//zo.NrZapytania NRZAP,
//zo.NrOferty NROFERTY,
//k.NazwaSkrocona KONTRAHENT
//from ZAM_ZapytaniaOfertowe zo
//left join Kontrahenci k on k.Id = zo.KontrahentId
//where zo.Id = {0}
//", IdZapytania));
//        }

//        public static DataSet GetDaneOferty(String IdOferty, Boolean Test)
//        {
//            if (Test) return Base.getDataSet(@"select 3 COUNT, '1337', '8001', 'nope'");
//            else return Base.getDataSet(String.Format(@"
//select 3 COUNT
//, NrOferty NROFERTY
//, NrZamowienia NRZAM
//, PowodOdrzucenia POWOD
//from ZAM_Oferty o
//where o.Id = {0}
//", IdOferty));
//        }

//        public static DataSet GetDaneKary(String IdKary, Boolean Test)
//        {
//            if (Test) return Base.getDataSet(@"select 3 COUNT, '1337', '8001', 'nope'");
//            else return Base.getDataSet(String.Format(@"
//select 3 COUNT
//, kon.NazwaSkrocona KONTRAHENT
//, f.NrFaktury NRZAM
//, k.WysokoscKary WYSOKOSC
//from KON_KaryUmowne k
//join ZAM_Faktury f on f.Id = k.FakturyId
//join KON_Umowy u  on u.Id = k.UmowaId
//join Kontrahenci kon on f.KontrahentId = kon.Id
//where k.Id = {0}
//", IdKary));
//        }


    }
}