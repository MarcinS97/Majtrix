using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using HRRcp.App_Code;
using System.IO;

namespace HRRcp.IPO.App_Code
{
    public class IPO_Mailing
    {
        public static void SendPDFToVendor(string zamId, string vendorId, Stream attachment)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
                
            string userEmail;
            string mailKupiecDostawca = IPO_db.getScalar("SELECT Wartosc FROM IPO_Konfiguracja WHERE Nazwa = 'MailKupiecDostawca'");
            if ("1".Equals(mailKupiecDostawca))
            {
                userEmail = App.User.EMail;
            }
            else
            {
                userEmail = settings.Email;
            }
            string vendorEmail = IPO_db.getScalar("SELECT Email FROM IPO_Dostawcy WHERE Id = " + vendorId);

            DataRow mail = Mailing.GetData("IPO_DOSTAWCA");
            DataSet mail_data = GetDaneZamowienia(zamId, false);
            string nrZam = ""+mail_data.Tables[0].Rows[0]["NR_ZAM"];
            string filename = "zamowienie_" + nrZam.Replace('/', '_') + ".pdf";
            if (db.getBool(mail, "Aktywny", false))
            {
                string subject = mail["Temat"].ToString();
                string body = mail["Tresc"].ToString();
                Mailing.PrepareMailText(ref subject, mail_data);
                Mailing.PrepareMailText(ref body, mail_data);
                try
                {
                    Mailing.SendMail2(settings._SMTPSerwer, settings.SMTPPort, settings.SMTPUser, settings.SMTPPass,
                        userEmail, vendorEmail, userEmail, null, subject, body,
                        attachment, filename, "application/pdf");
                    IPO_db.execSQL("UPDATE IPO_PozycjeZamowien SET DataMaila = GETDATE() WHERE IdZamowienia = " + zamId + " AND IdDostawcy = " + vendorId);
                }
                catch (Exception ex)
                {
                    Log.Error(Log.t2SENDMAIL, "SendMail2", ex.Message);
                    AppError.Show("Błąd podczas wysyłki maila:" + ex.Message);
                }
                //Mailing.SendMail2(vendorEmail, userEmail, null, subject, body, attachment, filename, "application/pdf");
            }
            else
            {
                AppError.Show("Wysyłanie maila do dostawcy jest nieaktywne");
            }
        }

        public static bool EventIPO(string maTyp, string id, string akcja) // id = idZamowienia lub idPozycji
        {
            try
            {
                DataSet prac = null;
                DataSet mail_data = null;
                string UserId = null;
                switch (maTyp)
                {
                    case "IPO_ZAM_UTW": //Informacja do zamawiającego o tym że utworzył zamówienie (NR_ZAM, APP_LINK)
                    case "IPO_ZAM_ACC": // Informacja do zamawiającego o kolejnych akceptacjach (NR_ZAM, APP_LINK, USER_AKC)
                    case "IPO_ZAM_RLZ": // Informacja do zamawiającego że jego zamowienie przeszło do realziacji (NR_ZAM, APP_LINK)  
                        mail_data = GetDaneZamowienia(id, false);
                        UserId = IPO_db.getScalar("Select IdPracownika from IPO_Zamowienia where IPO_Zamowienia.Id=" + id);
                        Mailing.CheckSendMail(maTyp, null, AppUser.GetData(UserId), mail_data);                   
                        break;
                    case "IPO_ZAM_SPD":  // Informacja do zamawiającego że jego pozycja zamowienia ma nadaną spodziewaną datę dostawy (NR_ZAM, APP_LINK, DATA_DOST)
                    case "IPO_ZAM_ODB": // Informacja do zamawiającego że jego pozycja zamowienia jest do odbioru(NR_ZAM, APP_LINK, MAG)
                        mail_data = GetDanePozycjiZamowienia(id, false);
                        UserId = IPO_db.getScalar(@"Select IPO_Zamowienia.IdPracownika from IPO_PozycjeZamowien 
                                                            JOIN IPO_Zamowienia ON IPO_Zamowienia.Id = IPO_PozycjeZamowien.IdZamowienia
                                                            WHERE IPO_PozycjeZamowien.Id=" + id);
                        Mailing.CheckSendMail(maTyp, null, AppUser.GetData(UserId), mail_data);  
                        break;
                    case "IPO_AKC_ACC": // Informacja do akceptującego że ma do zaakceptowania zamowienie (NR_ZAM, APP_LINK)
                        mail_data = GetDaneZamowienia(id, false);
                        prac = IPO_db.getDataSet(@"select IPO_SciezkaAkceptacji.UserId, IPO_SciezkaAkceptacji.Id
                                                        from IPO_SciezkaAkceptacji
                                                        join IPO_Zamowienia ON IPO_SciezkaAkceptacji.IdZamowienia = IPO_Zamowienia.Id
                                                        WHERE IPO_SciezkaAkceptacji.Status = 0
                                                        AND IPO_SciezkaAkceptacji.DataSciezki = IPO_Zamowienia.DataAkceptacji 
                                                        AND IPO_SciezkaAkceptacji.PoziomAkceptacji = IPO_Zamowienia.PoziomAkceptacji
                                                        AND IPO_SciezkaAkceptacji.DataMaila is null
                                                        AND IPO_Zamowienia.Id =" + id);

                        foreach (DataRow dr in prac.Tables[0].Rows)
                        {
                            Mailing.CheckSendMail(maTyp, null, AppUser.GetData(dr["UserId"].ToString()), mail_data);
                            IPO_db.execSQL(@"UPDATE IPO_SciezkaAkceptacji SET DataMaila = GETDATE()
                                                WHERE id=" + dr["Id"]);
                        }
                        break;
                    case "IPO_KUP_UTW": // Informacja do kupca że zostało stworzone zamówienie (NR_ZAM, APP_LINK, USER_ZAM)
                    case "IPO_KUP_ACC": // Infomacja do kupca że zamówienie zostało zaakceptowane (NR_ZAM, APP_LINK, USER_AKC)
                    case "IPO_KUP_RLZ": // Informacja do kupca że zamówienie przeszło do realizacji (NR_ZAM, APP_LINK)
                    case "IPO_KUP_EDI": // Informacja o zmianie specyfikacji zamówienia powyżej +/- 10% (NR_ZAM, APP_LINK)
                    case "IPO_KUP_SCN": // Informacja o wyborze nowej ściezki akceptacji (NR_ZAM, APP_LINK)
                    case "IPO_KUP_SCS": // Informacja o wyborze starej ściezki akceptacji (NR_ZAM, APP_LINK)
                        mail_data = GetDaneZamowienia(id, false);
                        prac = IPO_db.getDataSet(@"SELECT distinct(IPO_CCPrawa.UserId) FROM IPO_Zamowienia
	                                            JOIN IPO_CCZamowienia
		                                            ON IPO_Zamowienia.Id=IPO_CCZamowienia.IdZamowienia
	                                            JOIN IPO_CCPrawa
		                                            ON IPO_CCPrawa.IdCC=IPO_CCZamowienia.IdCC
	                                            WHERE IPO_CCPrawa.RolaId=3
		                                            AND IPO_Zamowienia.Id=" + id);

                        foreach (DataRow dr in prac.Tables[0].Rows)
                        {
                            Mailing.CheckSendMail(maTyp, null, AppUser.GetData(dr["UserId"].ToString()), mail_data);
                        }
                        break;
                    case "IPO_FIN_PRO": // Informacja o dodaniu/edycji nowego produktu (PRODUKT, APP_LINK)

                        break;
                    case "IPO_FIN_NKK": // Informacja o dodaniu numeru księgowego (PRODUKT, APP_LINK)

                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventIPO", ex.Message);
                return false;
            }
        }

        //----- Pobieranie danych zamówienia -----
        public static DataSet GetDaneZamowienia(string idZamowienia, bool test)
        {
            string select;
            string link = Tools.GetAppAddr2(null);
            //string link_kierownik = Tools.AddrSetPage(link, formKierownik);
            if (test)
                select = @"select 4 as COUNT, 
                           1234 as NR_ZAM,
                            '{0}' as APP_LINK,
                            '{2}' as USER_ZAM,
                            '{2}' as USER_AKC ";
            else
                select = @"select 4 as COUNT,
                            Numer as NR_ZAM,
                            '{0}' as APP_LINK,
                            idPracownika as USER_ZAM,
                            '{2}' as USER_AKC           
                            FROM IPO_Zamowienia
                            WHERE Id= '{1}'
                            ";
            return IPO_db.getDataSet(String.Format(select,
                    link,
                    idZamowienia,
                    App.User.Id));
        }

        //---- Pobieranie danych pozycji zamowienia
        public static DataSet GetDanePozycjiZamowienia(string idPozycjiZamowienia, bool test)
        {
            string select;
            string link = Tools.GetAppAddr2(null);
            if (test)
                select = @"select 6 as COUNT, 
                           1234 as NR_ZAM,
                            '{0}' as APP_LINK,
                            '{2}' as USER_ZAM,
                            '{2}' as USER_AKC,
                            '12-34-2035' as DATA_DOST,
                            'Ksiezyc' as MAG ";
            else
                select = @"select 6 as COUNT,
                            IPO_Zamowienia.Numer as NR_ZAM,
                            '{0}' as APP_LINK,
                            IPO_Zamowienia.idPracownika as USER_ZAM,
                            '{2}' as USER_AKC,
                            IPO_PozycjeZamowien.LokalizacjaOdbioru as MAG,
                            IPO_PozycjeZamowien.DataDostawy as DATA_DOST         
                            FROM IPO_PozycjeZamowien
                            JOIN IPO_Zamowienia on IPO_Zamowienia.Id=IPO_PozycjeZamowien.IdZamowienia
                            WHERE Id= '{1}'
                            ";
            return IPO_db.getDataSet(String.Format(select,
                    link,
                    idPozycjiZamowienia,
                    App.User.Id));
        }
    }
}
