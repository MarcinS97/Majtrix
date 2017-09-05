using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.IO;
using HRRcp.App_Code;

namespace HRRcp.App_Code
{
    public class Mailing
    {
        //public const bool NoMails = true;           // do testów, normalnie false !!!
        public const bool NoMails = false;          // maile wychodzą
        
        public const string ADMIN = "ADMIN";        // wysyłka maili - do kogo 
        public const string PRAC = "PRAC";          // wszyscy pracownicy <<< zrobić jak będzie potrzeba !!! 
        public const string KIER = "KONTR";         //
        public const string ALL = "ALL";            // wszyscy w strukturze (kier <> null), np start, stop programu

        private static void CheckUpdateMail(string typ, string grupa, string opis, string subject, string body)
        {
            string t = db.strParam(typ);
            DataRow dr = db.getDataRow("select Grupa, Opis from Mailing where Typ = " + t);
            if (dr == null)
                db.insert("Mailing", 0, "Typ,Grupa,Opis,Temat,Tresc,Aktywny",
                            db.strParam(typ),
                            db.strParam(grupa),
                            db.strParam(opis),
                            db.strParam(subject),
                            db.strParam(body),
                            1);
            else
                if (dr[0].ToString() != grupa || dr[1].ToString() != opis)
                    db.update("Mailing", 0, "Grupa,Opis", "Typ = " + t,
                            db.strParam(grupa),
                            db.strParam(opis));
        }

        public static DataRow GetData(string typ)
        {
            return db.getDataRow("select * from Mailing where Typ = " + db.strParam(typ));
        }

        //-----------------------------------------------------------------------------------
        public static void PrepareMailText(ref string text, DataSet data)
        {
            if (data != null)
            {
                DataRow dr = data.Tables[0].Rows[0];
                int c = data.Tables[0].Columns.Count;
                int count = Base.getInt(dr, 0, c);
                if (count > c) count = c;
                for (int i = 1; i <= count; i++)  // na 0 jest ilosć
                {
                    DataColumn dc = data.Tables[0].Columns[i];
                    text = text.Replace("%" + dc.Caption + "%", dr[i].ToString());
                }
            }
        }

        public static void PrepareMailTextLog(ref string text, DataSet data)   // zapis do logu - usuwam wrażliwe informacje jak np hasło
        {
            if (data != null)
            {
                DataRow dr = data.Tables[0].Rows[0];
                int c = data.Tables[0].Columns.Count;
                int count = Base.getInt(dr, 0, c);
                if (count > c) count = c;
                for (int i = 1; i <= count; i++)  // na 0 jest ilosć
                {
                    DataColumn dc = data.Tables[0].Columns[i];
                    string tag = dc.Caption;
                    if (tag == "PASS" ||            // na razie tak ...
                        tag == "PASSWORD" ||
                        tag == "HASLO")
                        text = text.Replace("%" + tag + "%", "**********");
                    else
                        text = text.Replace("%" + tag + "%", dr[i].ToString());
                }
            }
        }

        public static void PrepareMailText3(ref string text, DataColumnCollection columns, int start, DataRow drMail)  // start od 0
        {
            for (int i = start; i < columns.Count; i++)  
            {
                text = text.Replace("%" + columns[i].Caption + "%", drMail[i].ToString());
            }
        }

        //------------------------------------------------------------------------------------
        public static int SendMail2(string to_email, string cc, string bcc, string subject, string message)  // 0 ok, -1 end, -2 retry
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            try
            {
                //SendMail2(settings.SMTPSerwer, 0, null, null,     //20150105
                SendMail2(settings._SMTPSerwer, settings.SMTPPort, settings.SMTPUser, settings.SMTPPass, 
                    settings.Email, to_email, cc, bcc, subject, message, null, null, null);
                return 0;
            }
            catch (SmtpException ex)
            {
                Log.Error(Log.t2SENDMAIL, "SendMail2 smtp: " + ex.StatusCode, ex.Message);
                return -2;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "SendMail2", ex.Message);
                return -1;
            }
        }

        //public static bool SendMail2(string to_email, string cc, string bcc, string subject, string message, Stream attachment, string filename, string mime)
        //{
        //    Ustawienia settings = Ustawienia.CreateOrGetSession();
        //    try
        //    {
        //        //SendMail2(settings.SMTPSerwer, 0, null, null,   //20150105
        //        SendMail2(settings._SMTPSerwer, settings.SMTPPort, settings.SMTPUser, settings.SMTPPass,
        //            settings.Email, to_email, cc, bcc, subject, message,
        //            attachment, filename, mime);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Log.t2SENDMAIL, "SendMail2", ex.Message);
        //        return false;
        //    }
        //}



        /* 20160401 standaryzacja */
        public static /*bool*/ int SendMail2(string to_email, string cc, string bcc, string subject, string message, Stream attachment, string filename, string mime)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            try
            {
                //SendMail2(settings.SMTPSerwer, 0, null, null,   //20150105
                SendMail2(settings._SMTPSerwer, settings.SMTPPort, settings.SMTPUser, settings.SMTPPass,
                    settings.Email, to_email, cc, bcc, subject, message,
                    attachment, filename, mime);
                return /*true*/0;
            }
            catch (SmtpException ex)
            {
                Log.Error(Log.t2SENDMAIL, "SendMail2 smtp: " + ex.StatusCode, ex.Message);
                return -2;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "SendMail2", ex.Message);
                return /*false*/-1;
            }
        }


        //-------------
        public static bool IsMailing
        {
            get
            {
                if (App.IsMailing)
                    return true;
                else
                {
                    Log.Info(Log.t2SENDMAIL, "--- mailing wyłączony ---", null);
                    return false;
                }
            }
        }
        //-------------
        public static void SendMail2(string smtphost, int port, string user, string pass, string from, string to, string subject, string message, string attachment)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);  // "me@mycompany.com"
            mail.To.Add(to);                    // "you@yourcompany.com"
            mail.Subject = subject;
            mail.Body = message;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            if (!String.IsNullOrEmpty(attachment))
                mail.Attachments.Add(new Attachment(attachment));  // "c:\\temp\\example.txt"
            SmtpClient smtp = new SmtpClient(smtphost);
            if (port > 0) smtp.Port = port;
            if (!String.IsNullOrEmpty(user))
            {
                System.Net.NetworkCredential c = new System.Net.NetworkCredential(user, pass);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = c;
            }

            if (IsMailing)
                smtp.Send(mail);
        }

        public static bool MailExists(MailAddressCollection adr, string email)
        {
            string emailL = email.ToLower();
            foreach (MailAddress a in adr)
                if (a.Address.ToLower() == emailL)
                    return true;
            return false;
        }

        public static void SendMail2(string smtphost, int port, string user, string pass, string from, string to, string cc, string bcc, string subject, string message, Stream attachment, string aFileName, string mime)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);  // "me@mycompany.com"

            //mail.To.Add(to);                    // "you@yourcompany.com"  


            if (!String.IsNullOrEmpty(to))      // 20140215
            {
                string[] toList = to.Split(';', ',', ' ');
                foreach (string c in toList)
                {
                    string tto = c.Trim();
                    if (!String.IsNullOrEmpty(tto) && tto.Contains('@') && !MailExists(mail.To, tto))
                        mail.To.Add(tto);
                }
            }

            if (!String.IsNullOrEmpty(cc))
            {
                string[] ccList = cc.Split(';', ',', ' ');
                foreach (string c in ccList)
                {
                    string tcc = c.Trim();
                    if (!String.IsNullOrEmpty(tcc) && tcc.Contains('@') && !MailExists(mail.To, tcc) && !MailExists(mail.CC, tcc))
                        mail.CC.Add(tcc);
                }
            }
            if (!String.IsNullOrEmpty(bcc))
            {
                string[] bccList = bcc.Split(';', ',', ' ');
                foreach (string bc in bccList)
                {
                    string tbcc = bc.Trim();
                    if (!String.IsNullOrEmpty(tbcc) && tbcc.Contains('@') && !MailExists(mail.To, tbcc) && !MailExists(mail.CC, tbcc) && !MailExists(mail.Bcc, tbcc))
                        mail.Bcc.Add(tbcc);
                }
            }

            mail.Subject = subject;
            mail.Body = message;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            if (attachment != null)
            {
                //attachment.Seek(0, SeekOrigin.Begin);
                attachment.Position = 0;
                mail.Attachments.Add(new Attachment(attachment, aFileName, mime));
            }
            SmtpClient smtp = new SmtpClient(smtphost);  // "127.0.0.1"
            if (port > 0) smtp.Port = port;
            if (!String.IsNullOrEmpty(user))
            {
                System.Net.NetworkCredential c = new System.Net.NetworkCredential(user, pass);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = c;
            }

            if (IsMailing)
                smtp.Send(mail);
        }

        //wysyłka z potwierdzeniem 
        public static void SendMail2(string smtphost, int port, string user, string pass, string from, string to, string cc, string bcc, string subject, string message, Stream attachment, string aFileName, string mime, bool readConfirm)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);  // "me@mycompany.com"

            //mail.To.Add(to);                    // "you@yourcompany.com"  


            if (!String.IsNullOrEmpty(to))      // 20140215
            {
                string[] toList = to.Split(';', ',', ' ');
                foreach (string c in toList)
                {
                    string tto = c.Trim();
                    if (!String.IsNullOrEmpty(tto) && tto.Contains('@') && !MailExists(mail.To, tto))
                        mail.To.Add(tto);
                }
            }

            if (!String.IsNullOrEmpty(cc))
            {
                string[] ccList = cc.Split(';', ',', ' ');
                foreach (string c in ccList)
                {
                    string tcc = c.Trim();
                    if (!String.IsNullOrEmpty(tcc) && tcc.Contains('@') && !MailExists(mail.To, tcc) && !MailExists(mail.CC, tcc))
                        mail.CC.Add(tcc);
                }
            }
            if (!String.IsNullOrEmpty(bcc))
            {
                string[] bccList = bcc.Split(';', ',', ' ');
                foreach (string bc in bccList)
                {
                    string tbcc = bc.Trim();
                    if (!String.IsNullOrEmpty(tbcc) && tbcc.Contains('@') && !MailExists(mail.To, tbcc) && !MailExists(mail.CC, tbcc) && !MailExists(mail.Bcc, tbcc))
                        mail.Bcc.Add(tbcc);
                }
            }

            mail.Subject = subject;
            mail.Body = message;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            if (attachment != null)
            {
                //attachment.Seek(0, SeekOrigin.Begin);
                attachment.Position = 0;
                mail.Attachments.Add(new Attachment(attachment, aFileName, mime));
            }
            SmtpClient smtp = new SmtpClient(smtphost);  // "127.0.0.1"
            if (port > 0) smtp.Port = port;
            if (!String.IsNullOrEmpty(user))
            {
                System.Net.NetworkCredential c = new System.Net.NetworkCredential(user, pass);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = c;
            }

            if (readConfirm)
            {
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;
                mail.Headers.Add("Disposition-Notification-To", from);
            }

            if (IsMailing)
                smtp.Send(mail);
        }

        //------------------------------------------------------------------------------------
        /*
        public static bool SendMail(DataRow prac, DataRow mail, DataSet mail_data)
        {
            bool mailing = Base.getBool(prac, "Mailing", false);  // jak null to nie wysyłam
            string email = prac["Email"].ToString().Trim();
            if (mailing && !String.IsNullOrEmpty(email))
            {
                string typ = mail["Typ"].ToString().Trim();
                string subject = mail["Temat"].ToString();
                string body = mail["Tresc"].ToString();
                PrepareMailText(ref subject, mail_data);  // <<< pozniej dac to jako jeden z parametrów zapytania
                PrepareMailText(ref body, mail_data);
                Log.Info(Log.t2MAILTOSEND, email + (char)13 + subject, body, Log.OK);
                /*----- TESTY -----* /
                //return true;
                /*-----------------* /
                for (int i = 1; i <= 3; i++)  //20110930
                {
                    if (Tools.SendMail(email, subject, body))
                        return true;
                    else
                    {
                        switch (i)
                        {
                            case 1:
                                Tools.Delay(1000);
                                break;
                            case 2:
                                Tools.Delay(5000);
                                break;
                            case 3:
                                int p = Base.getInt(prac, "Id", 0);
                                Log.AddJob(Log.t2SENDMAIL, typ, p, 0, subject, body);
                                return false;
                        }
                    }
                }
                return false;   // mail nie został wysłany bo błąd
            }
            else return true;   // mail nie został wysłany bo bez wysyłki więc wszystko ok?
        }
        */
        public static bool SendMail(DataRow prac, DataSet dsCC, DataRow mail, DataSet mail_data)
        {
            bool mailing = db.getBool(prac, "Mailing", false);  // jak null to nie wysyłam
            //bool mailing = AppUser.HasRight(db.getValue(prac, "Rights"), AppUser.rMailing);
            string email = prac["Email"].ToString().Trim();
            
            string cc = null;
            string bcc = null;   // na razie bez obsługi
            if (dsCC != null)
            {
                foreach (DataRow drCC in db.getRows(dsCC))
                {
                    bool ccmailing = db.getBool(drCC, "Mailing", false);  // jak null to nie wysyłam
                    //bool mailing = AppUser.HasRight(db.getValue(prac, "Rights"), AppUser.rMailing);
                    if (ccmailing)
                    {
                        string mm = db.getValue(drCC, "Email");
                        if (!String.IsNullOrEmpty(mm))
                            if (String.IsNullOrEmpty(cc))
                                cc = mm;
                            else
                                cc += ";" + mm;
                    }
                }
            }

            if (mailing && !String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(cc) || !String.IsNullOrEmpty(bcc))
            {
                if (!mailing) email = null;   // odbiorca nie ma mailingu, ale jest cc
                    
                string typ = mail["Typ"].ToString().Trim();
                string subject = mail["Temat"].ToString();
                string body = mail["Tresc"].ToString();
                string subjectLog = subject;
                string bodyLog = body;

                PrepareMailText(ref subject, mail_data);  // <<< pozniej dac to jako jeden z parametrów zapytania
                PrepareMailText(ref body, mail_data);
                PrepareMailTextLog(ref subjectLog, mail_data);    // usuwa wrażliwe dane
                PrepareMailTextLog(ref bodyLog, mail_data);

                string email_cc = !String.IsNullOrEmpty(cc) ? "DW: " + cc + (char)13 : null;
                string email_bcc = !String.IsNullOrEmpty(bcc) ? "UDW: " + bcc + (char)13 : null;
                
                Log.Info(Log.t2MAILTOSEND, 
                    email + "|" + 
                    email_cc + "|" + 
                    email_bcc + (char)13 +
                    subjectLog, 
                    bodyLog, 
                    Log.OK);
                
                
                

                /*----- TESTY -----*/
                if (NoMails)
                {
                    //    Log.Info(Log.t2MAILTOSEND, String.Format("To: {0}\n{1} {2}\n{3} {4}", pp, mailing ? "1" : "0", email, typ, aktywny ? "1" : "0"),
                    //        "Brak wysyłki maila - TESTOWANIE", Log.OK);
                    return true;
                }
                /*-----------------*/
                
                
                
                
                for (int i = 1; i <= 3; i++)  //20110930
                {
                    int err = SendMail2(email, cc, bcc, subject, body);
                    if (err == 0) return true;
                    else if (err == -1) return false;
                    else //-2
                    {
                        switch (i)
                        {
                            case 1:
                                Tools.Delay(1000);
                                break;
                            case 2:
                                Tools.Delay(5000);
                                break;
                            case 3:
                                //int p = db.getInt(prac, "Id", 0);
                                //Log.AddJob(Log.SENDMAIL, typ, p, 0, subject, body);
                                Log.Error(Log.t2SENDMAIL, "Sending mail failed", email);
                                return false;
                        }
                    }
                }
                return false;   // mail nie został wysłany bo błąd
            }
            else return true;   // mail nie został wysłany bo bez wysyłki więc wszystko ok?
        }

        //---------------------------------------------------------

        public static bool SendMail(DataSet dsTo, DataSet dsCC, DataRow mail, DataSet mail_data, int rMailing)  // -1 domyślnie nie wysyła
        {
            string to = null;
            if (dsTo != null)
            {
                foreach (DataRow drTo in db.getRows(dsTo))
                {
                    bool m = db.getBool(drTo, "Mailing", false) || (rMailing > -1 && AppUser.HasRight(db.getValue(drTo, "Rights"), rMailing));  // jak null to nie wysyłam
                    //bool m = AppUser.HasRight(db.getValue(prac, "Rights"), AppUser.rMailing);
                    if (m)
                    {
                        string mm = db.getValue(drTo, "Email");
                        if (!String.IsNullOrEmpty(mm))
                            if (String.IsNullOrEmpty(to))
                                to = mm;
                            else
                                to += ";" + mm;
                    }
                }
            }

            string cc = null;
            string bcc = null;   // na razie bez obsługi
            if (dsCC != null)
            {
                foreach (DataRow drCC in db.getRows(dsCC))
                {
                    bool ccmailing = db.getBool(drCC, "Mailing", false) || (rMailing > -1 && AppUser.HasRight(db.getValue(drCC, "Rights"), rMailing));  // jak null to nie wysyłam
                    //bool mailing = AppUser.HasRight(db.getValue(prac, "Rights"), AppUser.rMailing);
                    if (ccmailing)
                    {
                        string mm = db.getValue(drCC, "Email");
                        if (!String.IsNullOrEmpty(mm))
                            if (String.IsNullOrEmpty(cc))
                                cc = mm;
                            else
                                cc += ";" + mm;
                    }
                }

            }

            if (!String.IsNullOrEmpty(to) || !String.IsNullOrEmpty(cc) || !String.IsNullOrEmpty(bcc))
            {
                string typ = mail["Typ"].ToString().Trim();
                string subject = mail["Temat"].ToString();
                string body = mail["Tresc"].ToString();
                PrepareMailText(ref subject, mail_data);  // <<< pozniej dac to jako jeden z parametrów zapytania
                PrepareMailText(ref body, mail_data);

                string email_to = !String.IsNullOrEmpty(to) ? "To: " + to + (char)13 : null;
                string email_cc = !String.IsNullOrEmpty(cc) ? "DW: " + cc + (char)13 : null;
                string email_bcc = !String.IsNullOrEmpty(bcc) ? "UDW: " + bcc + (char)13 : null;
                Log.Info(Log.t2MAILTOSEND,
                    email_to + "|" +
                    email_cc + "|" +
                    email_bcc + (char)13 +
                    subject,
                    body,
                    Log.OK);




                /*----- TESTY -----*/
                if (NoMails)
                    return true;
                /*-----------------*/




                for (int i = 1; i <= 3; i++)  //20110930
                {
                    int err = SendMail2(to, cc, bcc, subject, body);
                    if (err == 0) return true;
                    else if (err == -1) return false;
                    else //-2
                    {
                        switch (i)
                        {
                            case 1:
                                Tools.Delay(1000);
                                break;
                            case 2:
                                Tools.Delay(5000);
                                break;
                            case 3:
                                //int p = db.getInt(prac, "Id", 0);
                                //Log.AddJob(Log.SENDMAIL, typ, p, 0, subject, body);
                                Log.Error(Log.t2SENDMAIL, "Sending mail failed", to);
                                return false;
                        }
                    }
                }
                return false;   // mail nie został wysłany bo błąd
            }
            else return true;   // mail nie został wysłany bo bez wysyłki więc wszystko ok?
        }
        
        
        
        //-------------------
        /* StartData        0 - index od 0 mail_data, 8; pierwsza kolumna, moze byc bez nazwy !!!
         * Mailing          1
         * Email            2
         * cc               3
         * bcc              4
         * Zalaczniki       5
         * Aktywny          
         * Temat            6
         * Tresc            7
         * dane mail_data   8
         */
        //public static bool SendMail3(DataSet dsMails)   // żeby były nazwy kolumn i więcej maili na raz
        //{
        //    bool ok = true;
        //    foreach (DataRow drMail in db.getRows(dsMails))
        //    {
        //        bool mailing = db.getBool(drMail, "Mailing", false);  // jak null to nie wysyłam
        //        string email = db.getValue(drMail, "Email");
        //        string cc = db.getValue(drMail, "cc");
        //        string bcc = db.getValue(drMail, "bcc");
        //        string typ = db.getValue(drMail, "Typ");
        //        bool aktywny = db.getBool(drMail, "Aktywny", false); 
        //        if (aktywny && (mailing && !String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(cc) || !String.IsNullOrEmpty(bcc)))
        //        {
        //            if (!mailing) email = null;   // odbiorca nie ma mailingu, ale jest cc

        //            string subject = db.getValue(drMail, "Temat");
        //            string body = db.getValue(drMail, "Tresc");
        //            string zal = db.getValue(drMail, "Zalaczniki");
        //            int start = db.getInt(drMail, 0, 0);    // StartData - pierwsza kolumna !!!

        //            PrepareMailText3(ref subject, dsMails.Tables[0].Columns, start, drMail);  // <<< pozniej dac to jako jeden z parametrów zapytania
        //            PrepareMailText3(ref body, dsMails.Tables[0].Columns, start, drMail);

        //            string email_cc = !String.IsNullOrEmpty(cc) ? "\nDW: " + cc + (char)13 : null;
        //            string email_bcc = !String.IsNullOrEmpty(bcc) ? "\nUDW: " + bcc + (char)13 : null;
        //            Log.Info(Log.t2MAILTOSEND,
        //                typ +
        //                "\nTo:" + email +
        //                (!String.IsNullOrEmpty(cc) ? "\ncc: " + cc : null) +
        //                (!String.IsNullOrEmpty(bcc) ? "\nbcc: " + bcc : null) +
        //                (!String.IsNullOrEmpty(zal) ? "\nattachment: " + zal : null) +
        //                "\n" + subject,
        //                body, Log.OK);




        //            /*----- TESTY -----*/
        //            if (NoMails)
        //            {
        //                string prac = db.getValue(drMail, "Nazwisko") + " " + db.getValue(drMail, "Imie");
        //                Log.Info(Log.t2MAILTOSEND, String.Format("To: {0}\n{1} {2}\n{3} {4}", prac, mailing ? "1" : "0", email, typ, aktywny ? "1" : "0"),
        //                    "Brak wysyłki maila - TESTOWANIE", Log.OK);
        //                return true;    // do pierwszego, który wyśle !!!
        //            }
        //            /*-----------------*/




        //            int err = -1;
        //            for (int i = 1; i <= 3; i++)  //20110930
        //            {
        //                err = SendMail2(email, cc, bcc, subject, body);    // <<<< dołożyć załączniki !!!!
        //                if (err == 0) break;
        //                else if (err == -1) break;
        //                else //-2 i inne - powtarzam
        //                {
        //                    if (i == 1) Tools.Delay(1000);
        //                    else if (i == 2) Tools.Delay(5000);
        //                    else if (i == 3) break;
        //                }
        //            }
        //            switch (err)
        //            {
        //                case 0:
        //                    break;
        //                case -1:
        //                default:
        //                    ok = false;
        //                    //int p = db.getInt(prac, "Id", 0);
        //                    //Log.AddJob(Log.SENDMAIL, typ, p, 0, subject, body);
        //                    Log.Error(Log.t2SENDMAIL, "Sending mail failed", email);
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            string prac = db.getValue(drMail, "Nazwisko") + " " + db.getValue(drMail, "Imie");
        //            Log.Info(Log.t2MAILTOSEND, String.Format("To: {0}\n{1} {2}\n{3} {4}", prac, mailing ? "1" : "0", email, typ, aktywny ? "1" : "0"),
        //                "Brak wysyłki maila", Log.OK);
        //        }
        //    }
        //    return ok;
        //}
        //-------------------


        public static bool SendMail3(DataSet dsMails) //, string schId)   // żeby były nazwy kolumn i więcej maili na raz
        {
            bool ok = true;
            bool repOne = false;
            foreach (DataRow drMail in db.getRows(dsMails))
            {
                bool mailing = db.getBool(drMail, "Mailing", false);  // jak null to nie wysyłam
                string email = db.getValue(drMail, "Email");
                string cc = db.getValue(drMail, "cc");
                string bcc = db.getValue(drMail, "bcc");
                string typ = db.getValue(drMail, "Typ");
                bool aktywny = db.getBool(drMail, "Aktywny", false);
                if (aktywny && (mailing && !String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(cc) || !String.IsNullOrEmpty(bcc)))
                {
                    if (!mailing) email = null;   // odbiorca nie ma mailingu, ale jest cc

                    string subject = db.getValue(drMail, "Temat");
                    string body = db.getValue(drMail, "Tresc");
                    string zal = db.getValue(drMail, "Zalaczniki");
                    int start = db.getInt(drMail, 0, 0);    // StartData - pierwsza kolumna !!!

                    PrepareMailText3(ref subject, dsMails.Tables[0].Columns, start, drMail);  // <<< pozniej dac to jako jeden z parametrów zapytania
                    PrepareMailText3(ref body, dsMails.Tables[0].Columns, start, drMail);

                    string email_cc = !String.IsNullOrEmpty(cc) ? "\nDW: " + cc + (char)13 : null;
                    string email_bcc = !String.IsNullOrEmpty(bcc) ? "\nUDW: " + bcc + (char)13 : null;
                    Log.Info(Log.t2MAILTOSEND,
                        typ +
                        "\nTo:" + email +
                        (!String.IsNullOrEmpty(cc) ? "\ncc: " + cc : null) +
                        (!String.IsNullOrEmpty(bcc) ? "\nbcc: " + bcc : null) +
                        (!String.IsNullOrEmpty(zal) ? "\nattachment: " + zal : null) +
                        "\n" + subject,
                        body, Log.OK);




                    /*----- TESTY -----*/
                    if (NoMails)
                    {
                        string prac = db.getValue(drMail, "Nazwisko") + " " + db.getValue(drMail, "Imie");
                        Log.Info(Log.t2MAILTOSEND, String.Format("To: {0}\n{1} {2}\n{3} {4}", prac, mailing ? "1" : "0", email, typ, aktywny ? "1" : "0"),
                            "Brak wysyłki maila - TESTOWANIE", Log.OK);
                        return true;    // do pierwszego, który wyśle !!!
                    }
                    /*-----------------*/
                    const string defReportName = "report.csv";
                    const string defReportPath = "~/Export";

                    string reportId = null;
                    string repSQL   = null;
                    string repName  = defReportName;
                    Stream repStr   = null;
                    string conStr   = null;
                    string repPath  = null;
                    if (!repOne)   // dla grupy maili można przygotować ten sam raport ReportOne = 1 dla każdego rekordu
                    {
                        foreach (var item in drMail.Table.Columns)
                        {
                            string c = item.ToString();
                            switch (c)
                            {
                                case "ReportId":
                                    reportId = db.getValue(drMail, c);
                                    repSQL = db.getScalar("select sql from SqlMenu where Id = " + db.getValue(drMail, "ReportId"));
                                    break;
                                case "ReportName":
                                    repName = db.getValue(drMail, c);
                                    break;
                                case "ReportOne":
                                    repOne = db.getBool(drMail, c, false);
                                    break;
                                case "ReportConStr":
                                    conStr = db.getValue(drMail, c);
                                    break;
                                case "ReportPath":
                                    repPath = db.getValue(drMail, c);
                                    break;
                            }
                        }
                        if (!String.IsNullOrEmpty(reportId) && !String.IsNullOrEmpty(repSQL))
                        {
                            if (String.IsNullOrEmpty(repName)) repName = defReportName;
                            if (String.IsNullOrEmpty(repPath)) repPath = defReportPath;
                            repStr = Report.ExportStreamCSV(repSQL, "");
                            try
                            {
                                //string id;  <<< na później
                                //db.insert(out id, "RaportyWysylki", "IdRaportyScheduler,Data,Plik,Nazwa,Email,Status", );

                                Report.WriteStreamToFile(repStr, HttpContext.Current.Server.MapPath(Tools.Slash(repPath) + repName));   //Path.Combine ... ?
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    /*-------------------*/

                    int err = -1;
                    for (int i = 1; i <= 3; i++)  //20110930
                    {
                        err = SendMail2(email, cc, bcc, subject, body,  repStr, repName, null);    // <<<< dołożyć załączniki !!!! Dołożono !!!!!
                        if (err == 0) break;
                        else if (err == -1) break;
                        else //-2 i inne - powtarzam
                        {
                            if (i == 1) Tools.Delay(1000);
                            else if (i == 2) Tools.Delay(5000);
                            else if (i == 3) break;
                        }
                    }
                    switch (err)
                    {
                        case 0:
                            break;
                        case -1:
                        default:
                            ok = false;
                            //int p = db.getInt(prac, "Id", 0);
                            //Log.AddJob(Log.SENDMAIL, typ, p, 0, subject, body);
                            Log.Error(Log.t2SENDMAIL, "Sending mail failed", email);
                            break;
                    }
                }
                else
                {
                    string prac = db.getValue(drMail, "Nazwisko") + " " + db.getValue(drMail, "Imie");
                    Log.Info(Log.t2MAILTOSEND, String.Format("To: {0}\n{1} {2}\n{3} {4}", prac, mailing ? "1" : "0", email, typ, aktywny ? "1" : "0"),
                        "Brak wysyłki maila", Log.OK);
                }
            }
            return ok;
        }
        public static int GetMonitCount(string typ, string eventId, string userId) // eventId = mailId - identyfikator np. zastepstwa lub 0
        {
            DataRow dr = db.getDataRow(String.Format(
                        "select * from Monity where Typ='{0}' and EventId={1} and UserId={2}",
                        typ, eventId, userId));
            if (dr == null)
                return 0;
            else
                return db.getInt(dr, "Count", 0);
        }

        public static bool UpdateMonit(string typ, string eventId, string userId) // mailId - identyfikator np. zastepstwa lub 0
        {
            if (!String.IsNullOrEmpty(eventId))
            {
                string t = db.strParam(typ);
                bool ok = db.insert("Monity", 0, "Typ,EventId,UserId,Count,Data", t, eventId, userId, 1, "GETDATE()");
                if (!ok)
                    ok = db.update("Monity", 3, "Count,Data", "Typ = {0} and EventId = {1} and UserId = {2}",
                                   t, eventId, userId, "Count+1", "GETDATE()");
                return ok;
            }
            return true;
        }
        //---------------------
        public static bool SendMail(string typ, string mailId, string mailPracId, DataSet mail_data)
        {
            DataRow mail = Mailing.GetData(typ);
            DataRow prac = AppUser.GetData(mailPracId);
            bool ok = SendMail(prac, null, mail, mail_data);
            if (ok) UpdateMonit(typ, mailId, mailPracId);
            return ok;
        }

        public static bool CheckSendMail(string typ, string eventId, DataRow drPrac, DataSet mail_data)  // eventId - identyfikator z ktorym mail ma byc powiazany jak null to UpdatEMonit nie zapisze
        {
            return CheckSendMail(typ, eventId, drPrac, null, mail_data);
        }

        /*
        public static bool CheckSendMail(string typ, string eventId, DataRow drPrac, DataRow drPracCC, DataSet mail_data)  // eventId - identyfikator z ktorym mail ma byc powiazany jak null to UpdatEMonit nie zapisze
        {
            bool ok;
            //string mailPracId = db.getValue(drPrac, "Id_Przelozeni");
            string mailPracId = db.getValue(drPrac, "Id");
            DataRow mail = Mailing.GetData(typ);        // pobieramy dane maila
            if (db.getBool(mail, "Aktywny", false))     // jak nie ma definicji maila to null->false
            {
                ok = SendMail(drPrac, drPracCC, mail, mail_data);
                if (ok) UpdateMonit(typ, eventId, mailPracId);
            }
            else
            {
                //return errNoMail;
                ok = false;
                UpdateMonit(typ, eventId, mailPracId);
            }
            return ok;
        }
        */

        public static bool CheckSendMail(string typ, string eventId, DataRow drPrac, DataSet dsCC, DataSet mail_data)  // eventId - identyfikator z ktorym mail ma byc powiazany jak null to UpdatEMonit nie zapisze
        {
            bool ok;
            //string mailPracId = db.getValue(drPrac, "Id_Przelozeni");
            string mailPracId = db.getValue(drPrac, "Id");
            DataRow mail = Mailing.GetData(typ);        // pobieramy dane maila
            if (db.getBool(mail, "Aktywny", false))     // jak nie ma definicji maila to null->false
            {
                ok = SendMail(drPrac, dsCC, mail, mail_data);
                if (ok) UpdateMonit(typ, eventId, mailPracId);
            }
            else
            {
                //return errNoMail;
                ok = false;
                UpdateMonit(typ, eventId, mailPracId);
            }
            return ok;
        }

        public static bool CheckSendMail2(string typ, string eventId, DataSet dsTo, DataSet dsCC, DataSet mail_data, int rMailing)  // eventId - identyfikator z ktorym mail ma byc powiazany jak null to UpdatEMonit nie zapisze, rMailing dodatkowe prawo Mailing lub to prawo - do powiadomienia kierownictwa o wnioskach urlopowych, -1 to nie 
        {
            bool ok;
            DataRow mail = Mailing.GetData(typ);        // pobieramy dane maila
            if (db.getBool(mail, "Aktywny", false))     // jak nie ma definicji maila to null->false
            {
                ok = SendMail(dsTo, dsCC, mail, mail_data, rMailing);
                if (ok)
                    if (dsTo != null)
                        foreach (DataRow dr in db.getRows(dsTo))
                            UpdateMonit(typ, eventId, db.getValue(dr, "Id"));
            }
            else
            {
                //return errNoMail;
                ok = false;
                if (dsTo != null)
                    foreach (DataRow dr in db.getRows(dsTo))
                        UpdateMonit(typ, eventId, db.getValue(dr, "Id"));
                /*20151108 - czy to dawać ???
                if (dsCC != null)
                    foreach (DataRow dr in db.getRows(dsCC))
                        UpdateMonit(typ, eventId, db.getValue(dr, "Id"));
                */ 
            }
            return ok;
        }
        //-----------------------------------------------------------------------------------
        //public const string = "";
        //OKRES
        public const string grOKRES         = "OKRES";        
        public const string maOKRESENDING   = "MONIT";      // zbliża się koniec okresu
        public const string maOKRESACC      = "AKCEPTUJ";   // okres rozliczeniowy się zakończył prosze dokonać weryfikacji i ostatecznej akceptacji czasu pracy
        public const string maWEEKENDING    = "MONIT7";     // przypomnienie o konieczności zaakceptowania - piątek
        public const string maWEEKACC       = "ACC7";       // przypomnienie o konieczności zaakceptowania piątek, poniedziałek
        public const string maNOPP          = "NOPP";       // przypomnienie o braku planu pracy i konieczności zaplanowania - w bieżącym (ostatnim otwartym) i 10 dni przed nowym okresem rozliczeniowym
        
        //ZASTEPSTWA
        public const string grZASTEPSTWA    = "ZAST";
        public const string maZAST_ADD      = "ZAST_A";     // ustanowienie zastępstwa
        public const string maZAST_END      = "ZAST_D";     // usunięcie zastępstwa
        public const string maZAST_MONIT    = "ZAST_M";     // zastępstwo monit
        /*
        public const string idZASTEPOWANY   = "ZASTEPOWANY";
        public const string idZASTEPUJACY   = "ZASTEPUJACY";
        public const string idOD            = "OD";
        public const string idDO            = "DO";
        public const string idAKCJA         = "AKCJA";
        public const string idAPPLINK       = "APPLINK";
        */

        //PRZESUNIĘCIA
        public const string grPRZESUNIECIA  = "PRZE";
        public const string maPRZES_K       = "PRZE_K";     // mail do kierownika bieżącego i docelowego o przesunięciu (bez akceptacji: admin/własna struktura)
        public const string maPRZES_P       = "PRZE_P";     // mail do pracownika o przesunięciu
        public const string maPRZES_WN      = "PRZE_WN";    // mail do kierownika docelowego (lub pierwszego powyżej z uprawnieniem) że ma wniosek do zzakceptowania przyjęcia pracownika
        public const string maPRZES_ACC     = "PRZE_ACC";   // mail do kierownika wnioskującego o akceptacji przesunięcia pracownika
        public const string maPRZES_REJ     = "PRZE_REJ";   // mail do kierownika wnioskującego o odrzuceniu przesunięcia pracownika
        public const string maPRZES_START_K = "PRZE_S_K";   // mail do kierownika bieżącego i docelowego, że rozpoczyna się przesuniecie
        public const string maPRZES_START_P = "PRZE_S_P";   // mail do pracownika, że rozpoczyna się przesuniecie
        public const string maPRZES_MONIT   = "PRZE_MONIT"; // mail do kierownika docelowego, że kończy się zakładany okres przsunięcia i powinno się pracownika przesunąć spowrotem
        public const string maPRZES_DEL_K   = "PRZE_DEL_K"; // usunięto
        public const string maPRZES_DEL_P   = "PRZE_DEL_P"; // usunięto do pracownika
        public const string maPRZES_UPD     = "PRZE_UPD";   // zmieniono dane od - do

        public const string maPRZES_CCOLD   = "PRZE_CCREMOVE";// stare CC
        public const string maPRZES_CCNEW   = "PRZE_CCADD";   // nowe CC


        public const string idPRACOWNIK     = "PRACOWNIK";  //
        public const string idKIEROWNIK     = "KIEROWNIK";  // kierownik bieżący
        public const string idDOCELOWY      = "DOCELOWY";   // kierownik docelowy
        public const string idWNIOSKUJACY   = "WNIOSKUJACY";// kierownik wnioskujący
        public const string idAKCEPTUJACY   = "AKCEPTUJACY";
        public const string idWNIOSEK       = "WNIOSEK";
        public const string idODPOWIEDZ     = "ODPOWIEDZ";
        public const string idFORMLINK      = "FORMLINK";
        public const string idPRACLOGO      = "NREWID";

        public const string idLISTACCOLD    = "LISTACCOLD";
        public const string idLISTACCNEW    = "LISTACCNEW";
        public const string idLISTACCDIFF   = "LISTACCDIFF";


        const string przes_params =
            "%PRACOWNIK%\n" +
            "%KIEROWNIK%\n" +
            "%DOCELOWY%\n" +
            "%WNIOSKUJACY%\n" +
            "%AKCEPTUJACY%\n" +
            "%OD%\n" +
            "%DO%\n" +
            "%WNIOSEK%\n" +
            "%ODPOWIEDZ%\n" +
            "%FORMLINK%\n" +
            "%APPLINK%\n" +
            "%NREWID%\n";

        //WNIOSKI URLOPOWE
        public const string grWNIOSKIURLOPOWE = "WNURLOP";
        public const string maWU_SENT        = "WUSENT";     // mail do przełożonego akceptującego i bezpośredniego przełożonego (DW), pracownika (DW) - pracownik złożył wniosek urlopowy
        public const string maWU_DEL         = "WUDEL";      // mail do przełożonego akecptujacego i bezpośredniego przełożonego - pracownik usunął wniosek urlopowy
        public const string maWU_CHANGED     = "WUCHG";      // mail do przełożonego akceptującego i bezpośredniego przełożonego - wniosek został zmodyfikowany
        public const string maWU_ACCEPTED    = "WUACC";      // mail do admina, pracownika, bezpośredniego przełożonego - wniosek został zaakceptowany i czeka na wprowadzenie
        public const string maWU_REJECTED    = "WUREJ";      // mail do pracownika, bezpośredniego przełożonego         - wniosek został odrzucony
        public const string maWU_MONIT       = "WUMONIT";    // mail do przełożonego akceptującego - przypomnienie o konieczności zaakceptowania wniosku

        //public const string idPRACOWNIK     = "PRACOWNIK";
        //public const string idKIEROWNIK     = "KIEROWNIK";
        public const string idTYP           = "TYP";
        //public const string idOD            = "OD";	
        //public const string idDO            = "DO";	
        public const string idILEDNI        = "ILDNI";	
        public const string idUZADADNIENIE  = "UZASADNIENIE";	
        public const string idZASTEPUJE     = "ZASTEPUJE";
        //public const string idAKCEPTUJACY   = "AKCEPTUJACY";	
        public const string idPOWOD         = "POWOD";	
        //public const string idFORMLINK      = "FORMLINK";
        //public const string idAPPLINK       = "APPLINK";

        //PLAN URLOPÓW
        public const string grPLANURLOPOW   = "";
        public const string maPU_           = "";
    
        //SYSTEM
        public const string grSYSTEM        = "SYSTEM";
        public const string maSYS_PASSRESET = "PASSRESET";

        // PRACA ZDALNA
        public const string grPRACAZDALNA = "WNZDALNA";
        public const string maWZ_SENT = "WZSENT";
        public const string maWZ_ACCEPTED = "WZACC";
        public const string maWZ_REJECTED = "WZREJ";

#if SCARDS
        public const string grSCARDS        = "SCARDS";
#endif

        //-----------------------------------------------------

        public static void x_CheckUpdate()
        {
            const string app = "Rejestracja Czasu Pracy";
            const string header =
                "Witam\n\n";
            const string footer =
                "Pozdrawiamy serdecznie.\n" +
                "Dział HR.\n\n" +
                "Mail został wygenerowany automatycznie, prosimy na niego nie odpowiadać.";

            const string okres_params =
                "%OD%\n" +
                "%DO%\n" +
                "%APPLINK%\n";

            CheckUpdateMail(maOKRESENDING, grOKRES,
                        "1.1 OKRES ROZLICZENIOWY - Mail przypominający o zbliżającym sie <b>zamknięciu okresu rozliczeniowego</b>",
                        app + " - przypomnienie",
                        "Zbliża się termin zamknięcia okresu rozliczeniopwego.\n" +
                        "Proszę zaakceptować czas pracy pracowników.\n" +
                        "Program Rejestracji Czasu Pracy znajduje się pod adresem: http://jgbapp04/rcp.\n\n" +
                        footer);
            CheckUpdateMail(maOKRESACC, grOKRES,
                        "1.2. OKRES ROZLICZENIOWY - Mail informujący o <b>zakończeniu okresu rozliczeniowego</b>",
                        app + " - proszę zaakceptować czas pracy pracowników",
                        "Zakończył się okres rozliczeniowy.\n" +
                        "Proszę zaakceptować czas pracy pracowników.\n" +
                        "Program Rejestracji Czasu Pracy znajduje się pod adresem: http://jgbapp04/rcp.\n\n" +
                        footer);
            CheckUpdateMail(maWEEKENDING, grOKRES,
                        "1.3. OKRES ROZLICZENIOWY - Mail <b>przypominający o tygodniowej</b> akceptacji czasu pracy",
                        app + " - przypomnienie",
                        header + 
                        "Przypominamy o konieczności tygodniowego akceptowania czasu pracy pracowników.\n" +
                        "Czas powinien zostac zaakceptowany w ciągu dwóch pierwszych godzin poniedziałkowej zmiany.\n\n" +
                        "Program Rejestracji Czasu Pracy znajduje się pod adresem: %APPLINK%.\n\n" +
                        footer);
            CheckUpdateMail(maWEEKACC, grOKRES,
                        "1.4. OKRES ROZLICZENIOWY - Mail o <b>braku tygodniowej akceptacji</b> czasu pracy",
                        app + " - proszę zaakceptować czas pracy pracowników",
                        header + 
                        "Przypominamy o konieczności zaakceptowania tygodniowego czasu pracy pracowników.\n" +
                        "Czas powinien zostac zaakceptowany w ciągu dwóch pierwszych godzin poniedziałkowej zmiany.\n\n" +
                        "Program Rejestracji Czasu Pracy znajduje się pod adresem: %APPLINK%.\n\n" +
                        footer);

            const string zast_params =
                "%ZASTEPOWANY%\n" +
                "%ZASTEPUJACY%\n" +
                "%OD%\n" +
                "%DO%\n" +
                "%AKCJA%\n" +
                "%APPLINK%\n";

            /*
            const string deleg_params =
                "%WNIOSKUJACY%\n" +
                "%PRACOWNIK%\n" +
                "%SKAD%\n" +
                "%DOKAD%\n" +
                "%OD%\n" +
                "%DO%\n" +
                "%UWAGI%\n" +
                "%APPLINK%\n";

            const string przyp_params =
                "%APPLINK%\n";
            */
            CheckUpdateMail(maZAST_ADD, grZASTEPSTWA,
                        "2.1. ZASTĘPSTWA - Mail o <b>ustanowieniu</b> zastępstwa wysyłany do kierownika zastępowanego i zastępującego w dniu ustanowienia, rozpoczęcia i modyfikacji zastępstwa",
                            app + " - %AKCJA%",
                            header +
                            zast_params +
                            footer);
            CheckUpdateMail(maZAST_END, grZASTEPSTWA,
                        "2.2. ZASTĘPSTWA - Mail o <b>zakończeniu</b> zastępstwa wysyłany do kierownika zastępowanego i zastępującego w ostatnim dniu zastępstwa lub po usunięciu zastępstwa",
                            app + " - %AKCJA%",
                            header +
                            zast_params +
                            footer);
            CheckUpdateMail(maZAST_MONIT, grZASTEPSTWA,
                        "2.3. ZASTĘPSTWA - Mail informujący o <b>zbliżającym się zakończeniu</b> zastępstwa wysyłany do kierownika zastępowanego i zastępującego na 3 dni przed wygaśnięciem zastępstwa",
                            app + " - %AKCJA%",
                            header +
                            zast_params +
                            footer);
            //----------------------------------
            /*
            CheckUpdateMail(maPRZES_K, grPRZESUNIECIA,
                        "3.1. PRZESUNIĘCIA - Mail do <b>kierownika</b> bieżącego i docelowego <b>o przesunięciu</b> (bez akceptacji: admin/własna struktura)",
                            app + " - przesunięcie pracownika",
                            header +
                            przes_params +
                            footer);
            CheckUpdateMail(maPRZES_P, grPRZESUNIECIA,
                        "3.2. PRZESUNIĘCIA - Mail do <b>pracownika</b> o zmianie przełożoneg (przesunięciu)",
                            app + " - zmiana przełożonego",
                            header +
                            przes_params +
                            footer);

            CheckUpdateMail(maPRZES_WN, grPRZESUNIECIA,
                        "3.3. PRZESUNIĘCIA - Mail do <b>kierownika</b> docelowego (lub pierwszego powyżej z uprawnieniem do akceptacji przesunięć) o wniosku do <b>zaakceptowania / odrzucenia</b> o przyjęcie pracownika",
                            app + " - zaakceptuj lub odrzuć przesunięcie pracownika",
                            header +
                            przes_params +
                            footer);
            CheckUpdateMail(maPRZES_ACC, grPRZESUNIECIA,
                        "3.4. PRZESUNIĘCIA - Mail do kierownika <b>wnioskującego</b> o <b>akceptacji</b> przesunięcia pracownika",
                            app + " - kierownik %AKCEPTUJACY% zaakceptował przesunięcie pracownika %PRACOWNIK%",
                            header +
                            przes_params +
                            footer);
            CheckUpdateMail(maPRZES_REJ, grPRZESUNIECIA,
                        "3.5. PRZESUNIĘCIA - Mail do kierownika <b>wnioskującego</b> o <b>odrzuceniu</b> przesunięcia pracownika",
                            app + " - kierownik %AKCEPTUJACY% odrzucił przesunięcie pracownika %PRACOWNIK%",
                            header +
                            przes_params +
                            footer);

            CheckUpdateMail(maPRZES_START_K, grPRZESUNIECIA,
                        "3.6. PRZESUNIĘCIA - Mail do <b>kierownika</b> bieżącego i docelowego o rozpoczynającym się przesunięciu pracownika, wysyłany <b>w dniu</b> rozpoczęcia",
                            app + " - rozpoczęcie przesunięcia pracownika",
                            header +
                            przes_params +
                            footer);
            CheckUpdateMail(maPRZES_START_P, grPRZESUNIECIA,
                        "3.7. PRZESUNIĘCIA - Mail do <b>pracownika</b> o rozpoczynającym się przesunięciu, wysyłany <b>w dniu</b> rozpoczęcia",
                            app + " - rozpoczęcie przesunięcia pracownika",
                            header +
                            przes_params +
                            footer);

            CheckUpdateMail(maPRZES_MONIT, grPRZESUNIECIA,
                        "3.8. PRZESUNIĘCIA - Mail do <b>kierownika</b> (lub pierwszego powyżej z uprawnieniem do przesuwania) <b>przypominający o kończącym się</b> okresie przesunięcia pracownika",
                            app + " - przypomnienie o kończącym się przesunięciu pracownika",
                            header +
                            przes_params +
                            footer);
             */ 
        }

        //-----------------------------------------------------------------------------------
        private static void addZnacznik(PlaceHolder ph, string defaultTextBoxId, string znacznik, string opis)
        {
            Label z = new Label();
            if (!znacznik.StartsWith("%")) znacznik = "%" + znacznik;
            if (!znacznik.EndsWith("%")) znacznik += "%";
            //z.Text = "%" + znacznik + "% - " + opis;
            z.Text = znacznik + " - " + opis;
            //z.Attributes.Add("onclick", "selectZnaczniki('" + defaultTextBoxId + "','%" + znacznik + "% ');");
            //z.Attributes.Add("onclick", "insertZnacznik('" + defaultTextBoxId + "','%" + znacznik + "% ');");
            z.Attributes.Add("onclick", "insertZnacznik('" + defaultTextBoxId + "','" + znacznik + " ');");
            z.CssClass = "znacznik";
            Tools.AddControl(ph, null, z, "<br />");
            //ph.Controls.Add(z);
        }

        public static string AsZnacznik(string zn)
        {
            if (!zn.StartsWith("%")) zn = "%" + zn;
            if (!zn.EndsWith("%")) zn += "%";
            return zn;
        }

        public static string znacznikOnClick(string tbMailClientID, string znacznik)
        {
            return String.Format("insertZnacznik('{0}','{1}');", tbMailClientID, AsZnacznik(znacznik));
        }

        public static string znacznikOnClick2(string znacznik)
        {
            return String.Format("insertMailZnacznik('{0}');", AsZnacznik(znacznik));
        }

        public static void FillZnaczniki(ListViewItem Item, string grupa)
        {
            PlaceHolder ph = (PlaceHolder)Item.FindControl("phZnaczniki");
            if (ph != null)
            {
                TextBox tb = (TextBox)Item.FindControl("TrescTextBox");
                string tbID = tb != null ? tb.ClientID : "";

                DataSet ds = db.getDataSet(String.Format("select * from MailingZnaczniki where Grupa = '{0}'", grupa));
                if (db.getCount(ds) > 0)
                    foreach (DataRow dr in db.getRows(ds))
                        addZnacznik(ph, tbID, db.getValue(dr, "Znacznik"), db.getValue(dr, "Opis"));
                else
                    Tools.SetControlVisible(Item, "rowZnaczniki", false);
            }
        }

        //-----------------------------------------------------------------------------------
        public static void FillZnaczniki_old(ListViewItem Item, string grupa)
        {
            PlaceHolder ph = (PlaceHolder)Item.FindControl("phZnaczniki");
            if (ph != null)
            {
                TextBox tb = (TextBox)Item.FindControl("TrescTextBox");
                string tbID = tb != null ? tb.ClientID : "";

                switch (grupa)
                {
                    case grOKRES:
                        addZnacznik(ph, tbID, idOD, "Data rozpoczęcia");
                        addZnacznik(ph, tbID, idDO, "Data zakończenia");
                        addZnacznik(ph, tbID, idAPPLINK, "Link do aplikacji");
                        break;
                    case grZASTEPSTWA:
                        addZnacznik(ph, tbID, idZASTEPOWANY, "Nazwisko i imię kierownika zastępowanego");
                        addZnacznik(ph, tbID, idZASTEPUJACY, "Nazwisko i imię kierownika zastępującego");
                        addZnacznik(ph, tbID, idOD, "Data rozpoczęcia");
                        addZnacznik(ph, tbID, idDO, "Data zakończenia");
                        addZnacznik(ph, tbID, idAPPLINK, "Link do aplikacji");
                        addZnacznik(ph, tbID, idAKCJA, "Ustanowienie, Modyfikacja, Początek, Przypomnienie, Wygaśnięcie, Usunięcie zastępstwa");
                        break;
                    case grPRZESUNIECIA:
                        addZnacznik(ph, tbID, idPRACOWNIK,  "Nazwisko i imię pracownika");
                        addZnacznik(ph, tbID, idKIEROWNIK,  "Nazwisko i imię kierownika bieżącego (na dzień przesunięcia)");
                        addZnacznik(ph, tbID, idDOCELOWY,   "Nazwisko i imię kierownika docelowego");
                        addZnacznik(ph, tbID, idWNIOSKUJACY,"Nazwisko i imię kierownika wnioskującego");
                        addZnacznik(ph, tbID, idAKCEPTUJACY,"Nazwisko i imię kierownika akceptującego");
                        addZnacznik(ph, tbID, idOD,         "Data rozpoczęcia");
                        addZnacznik(ph, tbID, idDO,         "Data zakończenia");
                        addZnacznik(ph, tbID, idWNIOSEK,    "Treść wniosku (pole uwagi)");
                        addZnacznik(ph, tbID, idODPOWIEDZ,  "Treść odpowiedzi przy akceptacji / odrzuceniu wniosku");
                        addZnacznik(ph, tbID, idFORMLINK,   "Link do formatki z wnioskiem");
                        addZnacznik(ph, tbID, idAPPLINK,    "Link do aplikacji");
                        break;

                    /*
                case Mailing.grPROGRAM:
                    addZnacznik(ph, tbID, Mailing.idSTART, "Data rozpoczęcia programu");
                    addZnacznik(ph, tbID, Mailing.idSTOP, "Data zakończenia programu");
                    addZnacznik(ph, tbID, Mailing.idTERMIN, "Termin wypełnienia ankiety przez pracownika");
                    addZnacznik(ph, tbID, Mailing.idLINK_PRP, "Link do aplikacji");

                    addZnacznik(ph, tbID, Mailing.idDniP, "Ilość dni na wypełnienie ankiety przez pracownika");
                    addZnacznik(ph, tbID, Mailing.idDataP, "Termin wypełniania ankiety przez pracownika");
                    addZnacznik(ph, tbID, Mailing.idDniK, "Ilość dni na wypełnienie ankiety przez kierownika");
                    addZnacznik(ph, tbID, Mailing.idDataK, "Termin wypełniania ankiety przez kierownika");
                    addZnacznik(ph, tbID, Mailing.idDniPK, "Ilość dni na spotkanie kierownika z pracownikiem");
                    addZnacznik(ph, tbID, Mailing.idDataPK, "Termin przeprowadzenia spotkania kierownika z pracownikiem");
                    addZnacznik(ph, tbID, Mailing.idDniA, "Ilość dni na akceptacje ankiety przez pracownika");
                    addZnacznik(ph, tbID, Mailing.idDataA, "Termin akceptacji ankiety przez pracownika");
                    addZnacznik(ph, tbID, Mailing.idDniSK, "Ilość dni na wypełnienie ścieżki kariery pracownika przez kierownika");
                    addZnacznik(ph, tbID, Mailing.idDataSK, "Termin wypełnienia ścieżki kariery pracownika przez kierownika");
                    break;
                case Mailing.grANKIETA:
                    addZnacznik(ph, tbID, Mailing.idPRACOWNIK, "Imię i nazwisko pracownika");
                    addZnacznik(ph, tbID, Mailing.idKIEROWNIK, "Imię i nazwisko kierownika");
                    addZnacznik(ph, tbID, Mailing.idDATA, "Data spotkania");
                    addZnacznik(ph, tbID, Mailing.idTERMIN, "Termin wypełnienia/akceptacji ankiety przez pracownika lub kierownika");
                    addZnacznik(ph, tbID, Mailing.idPOWOD, "Powód zamknięcia ankiety");
                    addZnacznik(ph, tbID, Mailing.idSTART, "Data rozpoczęcia programu");
                    addZnacznik(ph, tbID, Mailing.idSTOP, "Data zakończenia programu");
                    addZnacznik(ph, tbID, Mailing.idLINK_PRP, "Link do aplikacji");
                    addZnacznik(ph, tbID, Mailing.idLINK_ANKIETA, "Link do ankiety pracownika");
                    addZnacznik(ph, tbID, Mailing.idLINK_KIEROWNIK, "Link do panelu kierownika");
                    break;
                case Mailing.grZAST:
                    addZnacznik(ph, tbID, Mailing.idZASTEPOWANY, "Imię i nazwisko zastępowanego kierownika");
                    addZnacznik(ph, tbID, Mailing.idZASTEPUJACY, "Imię i nazwisko kierownika, który zastępuje");
                    addZnacznik(ph, tbID, Mailing.idDATA, "Data obowiązywania zastępstwa");
                    addZnacznik(ph, tbID, Mailing.idLINK_PRP, "Link do aplikacji");
                    addZnacznik(ph, tbID, Mailing.idLINK_KIEROWNIK, "Link do panelu kierownika");
                    //addZnacznik(ph, tbID, Mailing., "");
                    break;
                     */
                    default:
                        HtmlTableRow row = (HtmlTableRow)Item.FindControl("rowZnaczniki");
                        if (row != null)
                            row.Visible = false;
                        break;
                }
            }
        }

        //--------------------------------------------------------
        public const string idZASTEPOWANY = "ZASTEPOWANY";
        public const string idZASTEPUJACY = "ZASTEPUJACY";
        public const string idOD = "OD";
        public const string idDO = "DO";
        public const string idAKCJA = "AKCJA";
        public const string idAPPLINK = "APPLINK";



        //----- akcja zastępstwa -----
        public const string zaSet = "Ustanowienie zastępstwa";
        public const string zaModify = "Modyfikacja zastępstwa";
        public const string zaStart = "Początek zastępstwa";
        public const string zaMonit = "Przypomnienie o wygaśnięciu zastępstwa";
        public const string zaStop = "Wygaśnięcie zastęstwa";
        public const string zaDel = "Usunięcie zastępstwa";

        public const string zaUserBegin = "Rozpoczęcie zastępstwa";  // przez kierownika, w AppUser.LoginAsUser
        public const string zaUserEnd = "Zakończenie zastępstwa";

        //----- okres rozliczeniowy ------------------------------------------------------------------------------
        public static DataSet GetOkresData(Okres ok, bool test)
        {
            if (ok == null) ok = Okres.Current(db.con);
            string link = Tools.GetAppAddr2(null);
            string select = "select {0} as COUNT, " +
                        db.strParam(Tools.DateToStr(ok.DateFrom)) + " as {1}, " +
                        db.strParam(Tools.DateToStr(ok.DateTo)) + " as {2}, " +
                        db.strParam(link) + " as {3}";
            return db.getDataSet(String.Format(select, 3,
                    idOD,
                    idDO,
                    idAPPLINK));
        }

        public static bool EventOkres(string type, DataRow drPrac, Okres ok)
        {
            try
            {
                DataSet mail_data = GetOkresData(ok, false);
                CheckSendMail(type, null, drPrac, mail_data); //bez zapisu do Monity
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventOkres: " + type, ex.Message);
                return false;
            }
        }

        //----- zastępstwa -----
        public static DataSet GetZastData(string zastepowanyId, string zastepujacyId, string dataOd, string dataDo, string akcja, bool test)
        {
            string select;
            string link = Tools.GetAppAddr2(null);
            //string link_kierownik = Tools.AddrSetPage(link, formKierownik);
            if (test)
                select = "select {0} as COUNT, " +
                            "Imie + ' ' + Nazwisko as {1}, " +
                            "Imie + ' ' + Nazwisko as {2}, " +
                            db.sqlGetDate("GETDATE()") + " as {3}, " +
                            db.sqlGetDate("GETDATE()") + " as {4}, " +
                            db.strParam(akcja) + " as {5}, " +
                            db.strParam(link) + " as {6} " +
                    "from Pracownicy " +
                    "where Id = " + zastepowanyId;
            else
                select = "select {0} as COUNT, " +
                            "P.Imie + ' ' + P.Nazwisko as {1}, " +
                            "Z.Imie + ' ' + Z.Nazwisko as {2}, " +
                            db.strParam(dataOd) + " as {3}, " +
                            db.strParam(dataDo) + " as {4}, " +
                            db.strParam(akcja) + " as {5}, " +
                            db.strParam(link) + " as {6} " +
                    "from Pracownicy P left outer join Pracownicy Z on Z.Id = " + zastepujacyId +
                    " where P.Id = " + zastepowanyId;
            return db.getDataSet(String.Format(select, 6,
                    idZASTEPOWANY,
                    idZASTEPUJACY,
                    idOD,
                    idDO,
                    idAKCJA,
                    idAPPLINK));
        }

        public static bool EventZastepstwo(string maTyp, string idZastepstwa, string zastepowanyId, string zastepujacyId, string dataOd, string dataDo, string akcja)
        {
            try
            {
                DataSet mail_data = GetZastData(zastepowanyId, zastepujacyId, dataOd, dataDo, akcja, false);
                switch (maTyp)
                {
                    case maZAST_ADD:
                        CheckSendMail(maTyp, idZastepstwa, AppUser.GetData(zastepowanyId), mail_data); // zastępowany
                        CheckSendMail(maTyp, idZastepstwa, AppUser.GetData(zastepujacyId), mail_data); // zastępujący
                        break;
                    case maZAST_END:
                        CheckSendMail(maTyp, idZastepstwa, AppUser.GetData(zastepowanyId), mail_data); // zastępowany
                        CheckSendMail(maTyp, idZastepstwa, AppUser.GetData(zastepujacyId), mail_data); // zastępujący
                        break;
                    case maZAST_MONIT:
                        CheckSendMail(maTyp, idZastepstwa, AppUser.GetData(zastepowanyId), mail_data); // zastępowany
                        CheckSendMail(maTyp, idZastepstwa, AppUser.GetData(zastepujacyId), mail_data); // zastępujący
                        //RemoveMonit(null, null, zastepowanyId, zaZASTZ_M); // usuwamy jak nowy/zmnieniamy i jak sie konczy - zeby nastepne zastepstwo tez moglo wywolac 
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventZastepstwo", ex.Message);
                return false;
            }
        }

        //----- przesuniecia -----
        public static DataSet GetPrzesData(string przId, bool test)
        {
            string select;
            string link = Tools.GetAppAddr2(null);
            string form = Tools.AddrSetPage(link, App.KierownikForm);
            if (test)
                select = @"
select {0} as COUNT, 
P.Imie + ' ' + P.Nazwisko as {1}, 
P.Imie + ' ' + P.Nazwisko as {2}, 
P.Imie + ' ' + P.Nazwisko as {3}, 
P.Imie + ' ' + P.Nazwisko as {4}, 
P.Imie + ' ' + P.Nazwisko as {5}, 
convert(varchar(10), GETDATE(), 20) as {6},
convert(varchar(10), GETDATE(), 20) as {7},
'Proszę o zaakaceptowanie przesunięcia pracownika w związku z ...' as {8},
'W nawiązaniu do wniosku o przesunięcie pracownika odpowiadam ...' as {9}," +
db.strParam(link) + " as {10}," +
db.strParam(form) + " as {11}," +
"'12345' as {12}, " + @"
'001-(0.5), 002-(0,5)' as LISTACCOLD,
'003-(0.5), 004-(0,5)' as LISTACCNEW,
'Commodity-przykład' as COMMODITY, 
'Area-przykład' as AREA, 
'Position-przykład' as POSITION
from Pracownicy P where P.Id = " + App.User.Id;
            else
                select = @"
select {0} as COUNT, 
P.Imie + ' ' + P.Nazwisko as {1}, 
ISNULL(KC.Imie + ' ' + KC.Nazwisko, 'nowy pracownik') as {2}, 
K.Imie + ' ' + K.Nazwisko as {3}, 
RQ.Imie + ' ' + RQ.Nazwisko as {4},
AC.Imie + ' ' + AC.Nazwisko as {5},
convert(varchar(10), R.Od, 20) as {6}, 
case when R.DoMonit is null then 'bez terminu' else convert(varchar(10), R.DoMonit, 20) end as {7}, 
R.UwagiRq as {8}, 
R.UwagiAcc as {9}," +
db.strParam(link) + " as {10}," +
db.strParam(form) + " as {11}," +
"P.KadryId as {12}," +
@"
dbo.fn_GetCC(RC.Id, 3, ', ') as LISTACCOLD,
dbo.fn_GetCC(R.Id, 3, ', ') as LISTACCNEW,
C.Commodity as COMMODITY, A.Area as AREA, PO.Position as POSITION,
R.IdPracownika, RC.IdKierownika, R.IdKierownika as IdDocelowy, K.Rights, R.IdKierownikaRq, R.IdKierownikaAcc, R.Status, R.DoMonit, R.Do as _Do, RQ.Email as EmailRq, AC.Email as EmailAcc, RC.Id as IdPrzypisania1
from Przypisania R
left outer join Pracownicy P on P.Id = R.IdPracownika
left outer join Pracownicy K on K.Id = R.IdKierownika
left outer join Pracownicy RQ on RQ.Id = R.IdKierownikaRq
left outer join Pracownicy AC on AC.Id = R.IdKierownikaAcc
left outer join Przypisania RC on DATEADD(DAY, -1, R.Od) between RC.Od and ISNULL(RC.Do, '20990909') and RC.Status = 1 and RC.IdPracownika = R.IdPracownika
left outer join Pracownicy KC on KC.Id = RC.IdKierownika
left join Commodity C on C.Id = R.IdCommodity
left join Area A on A.Id = R.IdArea
left join Position PO on PO.Id = R.IdPosition
where R.Id = " + przId;
            return db.getDataSet(String.Format(select, 
                    12+5,
                    idPRACOWNIK,
                    idKIEROWNIK,
                    idDOCELOWY,
                    idWNIOSKUJACY,
                    idAKCEPTUJACY,
                    idOD,
                    idDO,
                    idWNIOSEK,
                    idODPOWIEDZ,
                    idAPPLINK,
                    idFORMLINK,
                    idPRACLOGO));
        }

        public static bool EventPrzesuniecie(string maTyp, string przId)  // 
        {
            DataSet mail_data = GetPrzesData(przId, false);
            return EventPrzesuniecie(maTyp, przId, mail_data);
        }

        /*
        Event Przesuniecie:
         * wniosek 
            - do akceptującego, DW do składającego, obecnego, docelowego
         * akceptacja
            - do wnioskującego, DW do obecnego, docelowego
            - do pracownika
         * odrzucenie
            - do wnioskującego, DW do obecnego, docelowego
 
         na razie event=mail
        */


        private static bool IsPrzesActive(DataSet mail_data)   // trwa lub w przyszłości
        {
            if (mail_data != null)
            {
                DataRow dr = db.getRow(mail_data);
                int status = db.getInt(dr, "Status", -1);
                DateTime d = DateTime.Today;
                DateTime dDo = db.getDateTime(dr, "_Do", db.getDateTime(dr, "DoMonit", DateTime.MaxValue));     // Do jest zajęte przez znacznik i moze miec wartosc "bez terminu"
                if (status == Controls.Przypisania.cntPrzypisania.stAccepted && d <= dDo)   // trwa lub w przyszłości
                    return true;
            }
            return false;
        }

        private static DataSet GetKierCC(string idR1, string idR2, int status)
        {
            return db.getDataSet(String.Format(@"
declare @oldId int 
declare @newId int
set @oldId = {0} 
set @newId = {1}

select A.IdCC, CC.cc, CC.Nazwa, sum(A.Status) as Status 
into #ccc
from
(
select IdCC, -1 as Status from SplityWspP where IdPrzypisania = @oldId
union all
select IdCC, 1 as Status from SplityWspP where IdPrzypisania = @newId
) A
left join CC on CC.Id = A.IdCC
group by IdCC, cc, Nazwa

select distinct P.* from ccPrawa R
inner join #ccc C on C.IdCC = R.IdCC and C.Status = {2}
inner join Pracownicy P on P.Id = R.UserId and dbo.GetRightId(P.Rights, {3}) = 1
where R.IdCC != 0 

drop table #ccc
                ", db.nullParam(idR1), idR2, status, AppUser.rPLMailingPrzesCC));
        }

        public static bool EventPrzesuniecie(string maTyp, string przId, DataSet mail_data)  // 
        {
            try
            {
                DataRow dr = db.getRow(mail_data);
                string idPrac = db.getValue(dr, "IdPracownika");
                string idKier = db.getValue(dr, "IdKierownika");
                string kRights = db.getValue(dr, "Rights");
                string idDocelowy = db.getValue(dr, "IdDocelowy");
                string idKierRq = db.getValue(dr, "IdKierownikaRq");
                string idKierAcc = db.getValue(dr, "IdKierownikaAcc");
                string idR1 = db.getValue(dr, "IdPrzypisania1");  // poprzednie przypisanie

                // dodać spr który kierownik ma uprawnienia do akceptacji i pozmieniać id do wysyłki maili !!!
                string uid = App.User.OriginalId;
                bool m1 = uid != idKier;        // nie będę wysyłać do siebie
                bool m2 = uid != idDocelowy;
                switch (maTyp)
                {
                    case maPRZES_K:
                        if (!String.IsNullOrEmpty(idKier))      // nowi, nie ma obecnego przełożonego
                            CheckSendMail(maTyp, przId, AppUser.GetData(idKier), mail_data); 
#if SIEMENS
                        CheckSendMail(maTyp, przId, AppUser.GetData(idDocelowy), App.FindAdmin(AppUser.rPrzesuniecia), mail_data);
#else
                        CheckSendMail(maTyp, przId, AppUser.GetData(idDocelowy), mail_data);
                        //---- powiadomienia odpowiedzialnych za cc
                        DataSet dsCC = GetKierCC(idR1, przId, 1);  //wchodzi w cc
                        CheckSendMail2(maPRZES_CCNEW, przId, dsCC, null, mail_data, -1);
                        dsCC = GetKierCC(idR1, przId, -1);  //wypada z cc
                        CheckSendMail2(maPRZES_CCOLD, przId, dsCC, null, mail_data, -1);
#endif
                        break;
                    case maPRZES_P:  // przesunięcie pracowownika - bez wniosku
                        CheckSendMail(maTyp, przId, AppUser.GetData(idPrac), mail_data);
                        break;
                    case maPRZES_WN:
                        string kid;
                        kid = App.FindKierUp(idDocelowy, DateTime.Today, AppUser.rPrzesunieciaAcc);
                        if (String.IsNullOrEmpty(kid))
                        {
                            DataSet ds = App.FindAdmin(AppUser.rPrzesunieciaAcc);
                            foreach (DataRow adr in db.getRows(ds))
                            {
                                string cc = db.getValue(db.getRow(mail_data), "EmailRq");
                                CheckSendMail(maTyp, przId,
                                              AppUser.GetData(db.getValue(adr, "Id")),
                                              AppUser.GetDataDs(idKierRq),
                                              mail_data);
                            }
                        }
                        else
                            CheckSendMail(maTyp, przId, 
                                      AppUser.GetData(kid),
                                      kid != idKierRq ? AppUser.GetDataDs(idKierRq) : null,
                                      mail_data);
                        break;

                    case maPRZES_ACC:
#if SIEMENS                        
                        CheckSendMail(maTyp, przId, AppUser.GetData(idDocelowy), App.FindAdmin(AppUser.rPrzesuniecia), mail_data);
#else             
                        CheckSendMail(maTyp, przId, AppUser.GetData(idKierRq), mail_data);
#endif                   
                        break;
                    case maPRZES_REJ:
                        CheckSendMail(maTyp, przId, AppUser.GetData(idKierRq), mail_data);
                        break;

                    case maPRZES_MONIT:     // wygasające
                        kid = App.FindKierUp(idDocelowy, DateTime.Today, AppUser.rPrzesuniecia);   // pierwszy ktory moze przesunac, zaczyna od idDocelowy
                        if (String.IsNullOrEmpty(kid))
                        {
                            DataSet ds = App.FindAdmin(AppUser.rPrzesuniecia);  //admin i ma uprawnienie
                            foreach (DataRow adr in db.getRows(ds))
                                CheckSendMail(maTyp, przId, 
                                    AppUser.GetData(db.getValue(adr, "Id")),
                                    AppUser.GetDataDs(idDocelowy),
                                    mail_data);
                        }
                        else
                            CheckSendMail(maTyp, przId, 
                                AppUser.GetData(kid),
                                kid != idDocelowy ? AppUser.GetDataDs(idDocelowy) : null,   // do wiadomości bieżącego przełożonego
                                mail_data);
                        break;




#if SIEMENS
                    case maPRZES_START_K:
                        if (!String.IsNullOrEmpty(idKier))      // nowi, nie ma obecnego przełożonego
                            CheckSendMail(maTyp, przId, AppUser.GetData(idKier), mail_data);
                        CheckSendMail(maTyp, przId, AppUser.GetData(idDocelowy), App.FindAdmin(AppUser.rPrzesuniecia), mail_data);
                        break;
#else
                    case maPRZES_START_K:
                        if (!String.IsNullOrEmpty(idKier))      // nowi, nie ma obecnego przełożonego
                            CheckSendMail(maTyp, przId, AppUser.GetData(idKier), mail_data);
                        CheckSendMail(maTyp, przId, AppUser.GetData(idDocelowy), mail_data);
                        break;
#endif



                    case maPRZES_START_P:
                        CheckSendMail(maTyp, przId, AppUser.GetData(idPrac), mail_data);
                        break;

                    case maPRZES_DEL_K:
                        if (IsPrzesActive(mail_data))
                        {
                            if (m1 && !String.IsNullOrEmpty(idKier))      // nowi, nie ma obecnego przełożonego
                                CheckSendMail(maTyp, przId, AppUser.GetData(idKier), mail_data);
                            if (m2) CheckSendMail(maTyp, przId, AppUser.GetData(idDocelowy), mail_data);
#if SIEMENS
                            CheckSendMail2(maTyp, przId, App.FindAdmin(AppUser.rPrzesuniecia), null, mail_data, -1);
#endif
                        }
                        break;
                    case maPRZES_DEL_P:
                        if (IsPrzesActive(mail_data))
                        {
                            CheckSendMail(maTyp, przId, AppUser.GetData(idPrac), mail_data);
                        }
                        break;

                    case maPRZES_UPD:
                        if (IsPrzesActive(mail_data))
                        {
                            if (m1 && !String.IsNullOrEmpty(idKier))      // nowi, nie ma obecnego przełożonego
                                CheckSendMail(maTyp, przId, AppUser.GetData(idKier), mail_data);
                            if (m2) CheckSendMail(maTyp, przId, AppUser.GetData(idDocelowy), mail_data);

                            //CheckSendMail(maTyp, przId, AppUser.GetData(idPrac), mail_data);  // nie wysyłamy do pracownika, bo i tak nie ma wglądu w parametry
#if SIEMENS
                            CheckSendMail2(maTyp, przId, App.FindAdmin(AppUser.rPrzesuniecia), null, mail_data, -1);
#endif
                        }
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventPrzesuniecie", ex.Message);
                return false;
            }
        }

        //----- portal - wnioski urlopowe -----
        public static DataSet GetMailData(bool test, string grupa, params object[] par)
        {
            DataRow dr = db.getDataRow(String.Format("select * from MailingGrupy where Grupa = '{0}'", grupa));
            if (test)
            {
                string sql = db.getValue(dr, "ZnacznikiSqlTest");
                return db.select(sql, par);
            }
            else
            {
                string sql = db.getValue(dr, "ZnacznikiSql");
                return db.select(sql, par);
            }
        }

        public static DataSet GetWniosekUrlopowyData(string wnId, bool test)
        {
            string link = Tools.GetAppAddr2(null);
            string form = Tools.AddrSetPage(link, App._StartForm);
            return GetMailData(test, grWNIOSKIURLOPOWE, test ? App.User.Id : wnId, link, form);
        }

        public static DataSet x_GetWniosekUrlopowyData(string wnId, bool test)
        {
            string select;
            string link = Tools.GetAppAddr2(null);
            string form = Tools.AddrSetPage(link, App.KierownikForm);
            if (test)
            {
                select = @"
declare @pracId int
declare @applink nvarchar(250)
declare @formlink nvarchar(250)
set @pracId = {0}
set @applink = '{1}'
set @formlink = '{2}'

select 15 as COUNT 
    ,P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + ')' as PRACOWNIK 
    ,P.Nazwisko + ' ' + P.Imie as KIEROWNIK
    ,'Urlop wypoczynkowy' as TYP
    ,'urlopu wypoczynkowego' as OUDZIELENIE
    ,convert(varchar(10), GETDATE(), 20) as OD
    ,convert(varchar(10), GETDATE(), 20) as DO
    ,1 as ILDNI
    ,'Informacja dotyczące wniosku ...' as INFO
    ,'Uzasadnienie pracownika ...' as UZASADNIENIE
    ,P.Nazwisko + ' ' + P.Imie as ZASTEPUJE 
    ,P.Nazwisko + ' ' + P.Imie as AKCEPTUJACY
    ,P.Nazwisko + ' ' + P.Imie as AUTOR
    ,'Powód odrzucenia / uwagi przełożonego ...' as POWOD
    ,@applink as APPLINK
    ,@formlink as FORMLINK
from Pracownicy P 
where P.Id = @pracId";
                return db.select(select, App.User.Id, link, form);
            }
            else
            {
                select = @"
select 15 as COUNT
    ,P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + ')' as PRACOWNIK
    ,K.Nazwisko + ' ' + K.Imie as KIEROWNIK
    ,T.Typ as TYP
    ,T.TypNapis as OUDZIELENIE
    ,convert(varchar(10), W.Od, 20) as OD
    ,convert(varchar(10), W.Do, 20) as DO
    ,W.Dni as ILDNI
    ,W.Info as INFO
    ,W.UzasadnieniePrac as UZASADNIENIE
    ,ISNULL(W.Zastepuje, Z.Nazwisko + ' ' + Z.Imie) as ZASTEPUJE
    ,KA.Nazwisko + ' ' + KA.Imie as AKCEPTUJACY
    ,AT.Nazwisko + ' ' + AT.Imie as AUTOR
    ,W.UzasadnienieKier as POWOD
    ,'{1}' as APPLINK
    ,'{2}' as FORMLINK
    ,W.IdPracownika
    ,R.IdKierownika
    ,ACC.Id as IdKierAcc
    ,W.AutorId
from poWnioskiUrlopowe W
left join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left join Pracownicy P on P.Id = W.IdPracownika
left join Przypisania R on R.IdPracownika = P.Id and W.Od between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1 
left join Pracownicy K on K.Id = R.IdKierownika
left join Pracownicy Z on Z.Id = W.IdZastepuje
left join Pracownicy KA on KA.Id = W.IdKierAcc
left join Pracownicy KZ on KZ.Id = W.IdKierAccZast
left join Pracownicy A on A.Id = W.DataKadryAcc
left join Pracownicy AT on AT.Id = W.AutorId
--left join Dzialy D on D.Id = P.IdDzialu
--left join Stanowiska S on S.Id = P.IdStanowiska
outer apply (select * from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)) ACC
where W.Id = {0}";
                return db.select(select, wnId, link, form);
            }
        }

        public static bool EventWniosekUrlopowy(string maTyp, string wnId)  // 20140704 dodanie dw do kierowników zastępujących
        {
            DataSet mail_data = GetWniosekUrlopowyData(wnId, false);
            DataRow dr = db.getRow(mail_data);
            string idAcc = db.getValue(dr, "IdKierAcc");
            string idKier = db.getValue(dr, "IdKierownika");
            string idPrac = db.getValue(dr, "IdPracownika");
            string idAutor = db.getValue(dr, "AutorId");
            try
            {
                switch (maTyp)
                {
                    case maWU_SENT:     //4.1. mail do przełożonego akceptującego i bezpośredniego przełożonego (DW), pracownika (DW) - pracownik złożył wniosek urlopowy
                    case maWU_DEL:      //4.2. mail do przełożonego akecptujacego i bezpośredniego przełożonego - pracownik usunął wniosek urlopowy
                    case maWU_CHANGED:  //4.3. mail do przełożonego akceptującego i bezpośredniego przełożonego - wniosek został zmodyfikowany
                        bool noAcc = String.IsNullOrEmpty(idAcc);
                        if (String.IsNullOrEmpty(idKier)) idKier = "0";  //20160504 jak nie ma kier to się wywalał - jak no Acc to dostanie admin

                        //DataSet dsDW = db.getDataSet(String.Format("select * from Pracownicy where Id in ({0},{1})", idKier, idPrac));
                        DataSet dsDW = db.getDataSet(String.Format(@"
select * from Pracownicy where Id in ({0},{1})
union 
select P.* from Pracownicy P
inner join Zastepstwa Z on P.Id = Z.IdZastepujacy and Z.IdZastepowany in ({2}, {0}) and dbo.getdate(GETDATE()) between Z.Od and Z.Do                        
                            ", idKier, idPrac, db.ISNULL(idAcc, "0")));     // do wiadomości kierownika (jeżeli nie akceptuje), pracownika oraz zastepujących kierownika i akceptującego

                        if (noAcc)
                        {
                            //DataSet ds = App.FindAdmin(AppUser.rWnioskiUrlopoweAcc);
                            DataSet ds = App.FindUser(false, AppUser.rWnioskiUrlopoweAdm, AppUser.rWnioskiUrlopoweAcc);  // do administratorów wniosków z uprawnieniem do akceptacji wniosków urlopowych
                            CheckSendMail2(maTyp, wnId, ds, dsDW, mail_data, AppUser.rWnioskiUrlopoweNoAccMail);
                        }
                        else
                        {
                            CheckSendMail2(maTyp, wnId, AppUser.GetDataDs(idAcc), dsDW, mail_data, -1);
                        }
                        break;
                    case maWU_ACCEPTED: //4.4. mail do admina, pracownika, bezpośredniego przełożonego - wniosek został zaakceptowany i czeka na wprowadzenie  <<< do admina nie ... bo za dużo było
                    case maWU_REJECTED: //4.5. mail do pracownika, bezpośredniego przełożonego         - wniosek został odrzucony
                        
                        //dsDW = db.getDataSet(String.Format("select * from Pracownicy where Id in ({0})", idKier));
                        dsDW = db.getDataSet(String.Format(@"
select * from Pracownicy where Id in ({0}) and Id <> {1}
union 
select P.* from Pracownicy P
inner join Zastepstwa Z on P.Id = Z.IdZastepujacy and Z.IdZastepowany in ({0}) and dbo.getdate(GETDATE()) between Z.Od and Z.Do                        
                            ", idKier, db.ISNULL(idAcc, "0")));                        
                        
                        CheckSendMail2(maTyp, wnId, AppUser.GetDataDs(idPrac), dsDW, mail_data, -1);
                        break;
                    case maWU_MONIT:    //4.6. mail do przełożonego akceptującego - przypomnienie o konieczności zaakceptowania wniosku
                        if (String.IsNullOrEmpty(idAcc))
                        {
                            //DataSet ds = App.FindAdmin(AppUser.rWnioskiUrlopoweAcc);
                            DataSet ds = App.FindUser(false, AppUser.rWnioskiUrlopoweAdm, AppUser.rWnioskiUrlopoweAcc);  // do administratorów wniosków z uprawnieniem do akceptacji wniosków urlopowych
                            CheckSendMail2(maTyp, wnId, ds, null, mail_data, -1);
                        }
                        else
                        {
                            dsDW = db.getDataSet(String.Format(@"
select P.* from Pracownicy P
inner join Zastepstwa Z on P.Id = Z.IdZastepujacy and Z.IdZastepowany in ({0}) and dbo.getdate(GETDATE()) between Z.Od and Z.Do
                            ", idAcc));                        

                            CheckSendMail2(maTyp, wnId, AppUser.GetDataDs(idAcc), dsDW, mail_data, -1);
                        }
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventWniosekUrlopowy", ex.Message);
                return false;
            }
        }

        public static bool EventWniosekUrlopowy(string maTyp, string wnId, DataRow prevdr)  // przy aktualizacji danych 
        {
            return EventWniosekUrlopowy(maTyp, wnId);
            /*    
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventWniosekUrlopowy", ex.Message);
                return false;
            }
            */
        }


        //----- portal - PLAN URLPOW -----
        public static DataSet GetPlanUrlopowData(string pracId, bool test)
        {
            return null;
            /*




            string select;
            string link = Tools.GetAppAddr2(null);
            string form = Tools.AddrSetPage(link, App.KierownikForm);
            if (test)
                select = @"
select 14 as COUNT 
    ,P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + ')' as PRACOWNIK, 
    ,P.Nazwisko + ' ' + P.Imie as KIEROWNIK, 
    ,'Wniosek urlopowy' as TYP, 
    ,convert(varchar(10), GETDATE(), 20) as OD,
    ,convert(varchar(10), GETDATE(), 20) as DO,
    ,1 as ILDNI
    ,'Informacja dotyczące wniosku ...' as INFO
    ,'Uzasadnienie pracownika ...' as UZASADNIENIE
    ,P.Nazwisko + ' ' + P.Imie as ZASTEPUJE 
    ,P.Nazwisko + ' ' + P.Imie as AKCEPTUJACY
    ,P.Nazwisko + ' ' + P.Imie as AUTOR
    ,'Powód odrzucenia / uwagi przełożonego ...' as POWOD
    ,'{1}' as APPLINK
    ,'{2}' as FORMLINK";
            else
                select = @"
select 14 as COUNT
    ,P.Nazwisko + ' ' + P.Imie + ' (' + P.KadryId + ')' as PRACOWNIK
    ,K.Nazwisko + ' ' + K.Imie as KIEROWNIK
    ,T.Typ as TYP
    ,convert(varchar(10), W.Od, 20) as OD
    ,convert(varchar(10), W.Do, 20) as DO
    ,W.Dni as ILDNI
    ,W.Info as INFO
    ,W.UzasadnieniePrac as UZASADNIENIE
    ,ISNULL(W.Zastepuje, Z.Nazwisko + ' ' + Z.Imie) as ZASTEPUJE
    ,KA.Nazwisko + ' ' + KA.Imie as AKCEPTUJACY
    ,AT.Nazwisko + ' ' + AT.Imie as AUTOR
    ,W.UzasadnienieKier as POWOD
    ,'{1}' as APPLINK
    ,'{2}' as FORMLINK
    ,W.IdPracownika
    ,R.IdKierownika
    ,ACC.Id as IdKierAcc
    ,W.AutorId
from poWnioskiUrlopowe W
left join poWnioskiUrlopoweTypy T on T.Id = W.TypId
left join poWnioskiUrlopoweStatusy ST on ST.Id = W.StatusId
left join Pracownicy P on P.Id = W.IdPracownika
left join Przypisania R on R.IdPracownika = P.Id and W.Od between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1 
left join Pracownicy K on K.Id = R.IdKierownika
left join Pracownicy Z on Z.Id = W.IdZastepuje
left join Pracownicy KA on KA.Id = W.IdKierAcc
left join Pracownicy KZ on KZ.Id = W.IdKierAccZast
left join Pracownicy A on A.Id = W.DataKadryAcc
left join Pracownicy AT on AT.Id = W.AutorId
--left join Dzialy D on D.Id = P.IdDzialu
--left join Stanowiska S on S.Id = P.IdStanowiska
outer apply (select * from dbo.fn_GetUpKierWithRight(W.IdPracownika, GETDATE(), 13, 1)) ACC
where W.Id = {0}";
            return db.getDataSet(String.Format(select, wnId, link, form));
            */
        }

        public static bool EventPlanUrlopow(string maTyp, string wnId)  // 
        {
            DataSet mail_data = GetPlanUrlopowData(wnId, false);
            DataRow dr = db.getRow(mail_data);


            string idAcc = db.getValue(dr, "IdKierAcc");
            string idKier = db.getValue(dr, "IdKierownika");
            string idPrac = db.getValue(dr, "IdPracownika");
            string idAutor = db.getValue(dr, "AutorId");
            try
            {
                /*
                switch (maTyp)
                {
                    case maWU_SENT:     //4.1. mail do przełożonego akceptującego i bezpośredniego przełożonego (DW), pracownika (DW) - pracownik złożył wniosek urlopowy
                    case maWU_DEL:      //4.2 mail do przełożonego akecptujacego i bezpośredniego przełożonego - pracownik usunął wniosek urlopowy
                    case maWU_CHANGED:  //4.3. mail do przełożonego akceptującego i bezpośredniego przełożonego - wniosek został zmodyfikowany
                        DataSet dsDW = db.getDataSet(String.Format("select * from Pracownicy where Id in ({0},{1})", idKier, idPrac));
                        if (String.IsNullOrEmpty(idAcc))
                        {
                            DataSet ds = App.FindAdmin(AppUser.rWnioskiUrlopoweAcc);
                            CheckSendMail(maTyp, wnId, ds, dsDW, mail_data);
                        }
                        else
                            CheckSendMail(maTyp, wnId, AppUser.GetDataDs(idAcc), dsDW, mail_data);
                        break;
                    case maWU_ACCEPTED: //4.4. mail do admina, pracownika, bezpośredniego przełożonego - wniosek został zaakceptowany i czeka na wprowadzenie
                    case maWU_REJECTED: //4.5. mail do pracownika, bezpośredniego przełożonego         - wniosek został odrzucony
                        dsDW = db.getDataSet(String.Format("select * from Pracownicy where Id in ({0})", idKier));
                        CheckSendMail(maTyp, wnId, AppUser.GetDataDs(idPrac), dsDW, mail_data);
                        break;
                    case maWU_MONIT:    //4.6. mail do przełożonego akceptującego - przypomnienie o konieczności zaakceptowania wniosku
                        if (String.IsNullOrEmpty(idAcc))
                        {
                            DataSet ds = App.FindAdmin(AppUser.rWnioskiUrlopoweAcc);
                            CheckSendMail(maTyp, wnId, ds, null, mail_data);
                        }
                        else
                            CheckSendMail(maTyp, wnId, AppUser.GetDataDs(idAcc), null, mail_data);
                        break;
                }
                */
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventWniosekUrlopowy", ex.Message);
                return false;
            }
        }

        public static bool EventPlanUrlopow(string maTyp, string wnId, DataRow prevdr)  // przy aktualizacji danych 
        {
            return EventWniosekUrlopowy(maTyp, wnId);
            /*    
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventWniosekUrlopowy", ex.Message);
                return false;
            }
            */
        }

        //----------------------------------------------------
        public static DataSet GetPassResetData(string pracId, string pracNI, string nrew, string user, string pass)
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("COUNT", typeof(int));
            dt.Columns.Add("PRACOWNIK", typeof(string));
            dt.Columns.Add("NREW", typeof(string));
            dt.Columns.Add("USER", typeof(string));
            dt.Columns.Add("PASS", typeof(string));
            dt.Rows.Add(dt.Columns.Count - 1, pracNI, nrew, user, pass);
            return ds;
        }

        public static bool EventPassReset(string pracId, string pass)
        {
            try
            {
                DataRow drP = AppUser.GetData(pracId);
                string prac = db.getValue(drP, "Nazwisko") + " " + db.getValue(drP, "Imie");
                string nrew = db.getValue(drP, "KadryId");
                string nick = db.getValue(drP, "Nick");
                DataSet mail_data = GetPassResetData(pracId, prac, nrew, nick, pass);

                return CheckSendMail(maSYS_PASSRESET, null, drP, mail_data);   // jak mailing wyłączony to false
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventPassReset", ex.Message);
                return false;
            }
        }

        //----- wnioski majatkowe -----
        public static DataSet GetWniosekMajData(string wnId, bool test)
        {
            string select;
            string link = Tools.GetAppAddr2(null);
            string form = Tools.AddrSetPage(link, "Portal/Ubezpieczenia/Majatkowe/MojePolisy.aspx");
            if (test)
                select = @"";
            else
                select = String.Format(@"
select {0} as COUNT, 
  wm.Id IdWniosku
,  p.Nazwisko + ' ' + p.Imie PRACOWNIK
, p.KadryId KADRYID
, convert(varchar(10), wm.DataOd, 20) DATAOD
, convert(varchar(10), wm.DataUtworzenia, 20) DATAUTW
, wm.Skladka + isnull(wm.SkladkaPlus, 0) SKLADKA
, oasum.s SKLADKASUM
, '{1}' APPLINK
, '{2}' APPFORM
, wm.ZglaszajacyId IdPracownika
from poWnioskiMajatkowe wm
left join Pracownicy p on p.Id = wm.ZglaszajacyId
outer apply (select sum(Skladka + isnull(SkladkaPlus, 0)) s from poWnioskiMajatkowe where ZglaszajacyId = wm.ZglaszajacyId and Status > -1 and dbo.getdate(getdate()) between DataOd and isnull(DataZakonczenia, '20990909')) oasum
where wm.Id = {3}", 9, link, form, wnId);
            return db.getDataSet(db.conP, select);
        }

        public const String maUBEZP_POTW = "UBEZP_POTW";
        public const String maUBEZP_POTW_ADM = "UBEZP_POTW_ADM";

        // składnik płacowy
        public const String maUBEZP_ERR = "UBEZP_ERR";

        // zmiana wariantu
        public const String maUBEZP_ZMIANA_POTW = "UBEZP_ZMIANA_POTW";
        public const String maUBEZP_ZMIANA_POTW_ADM = "UBEZP_ZMIANA_POTW_ADM";

        // zmiana danych
        public const String maUBEZP_ZMIANAD_POTW = "UBEZP_ZMIANAD_POTW";
        public const String maUBEZP_ZMIANAD_POTW_ADM = "UBEZP_ZMIANAD_POTW_ADM";

        // zakończenie polisy
        public const String maUBEZP_KONIEC_POTW = "UBEZP_KONIEC_POTW";
        public const String maUBEZP_KONIEC_POTW_ADM = "UBEZP_KONIEC_POTW_ADM";

        public static bool EventUbezpieczeniaMajatkowe(string maTyp, string wnId)  // 
        {
            DataSet mail_data = GetWniosekMajData(wnId, false);
            string pracId = db.getValue(db.getRow(mail_data), "IdPracownika");

            switch (maTyp)
            {
                case maUBEZP_POTW:
                case maUBEZP_ZMIANA_POTW:
                case maUBEZP_ZMIANAD_POTW:
                case maUBEZP_KONIEC_POTW:
                    if (!String.IsNullOrEmpty(pracId))
                        return CheckSendMail(maTyp, wnId, AppUser.GetData(pracId), mail_data);
                    break;
                case maUBEZP_ERR:
                case maUBEZP_POTW_ADM:
                case maUBEZP_ZMIANA_POTW_ADM:
                case maUBEZP_ZMIANAD_POTW_ADM:
                case maUBEZP_KONIEC_POTW_ADM:
                    DataSet dsAdmins = App.FindUser(false, AppUser.rUbezpieczeniaAdm);

                    bool b = true;
                    foreach (DataRow adr in db.getRows(dsAdmins))
                    {
                        b &= CheckSendMail(maTyp, wnId,
                            //AppUser.GetData(db.getValue(adr, "Id")),
                            adr,
                            mail_data);

                        // wysyłam do zastępujących o tym samym uprawnieniu
                        DataSet dsZast = App.FindZast(adr, AppUser.rUbezpieczeniaAdm);

                        
                        foreach (DataRow zast in db.getRows(dsZast))
                        {

                            if (!AlreadySent(dsAdmins, zast))
                            {
                                b &= CheckSendMail(maTyp, wnId,
                                    //AppUser.GetData(db.getValue(adr, "Id")),
                                    zast,
                                    mail_data);
                            }
                        }
                    }
                    return b;
            }
            
            return false;
            //return EventPrzesuniecie(maTyp, przId, mail_data);
        }

        public static bool AlreadySent(DataSet dsPrevious, DataRow current)
        {
            string currentId = db.getValue(current, "Id");
            foreach(DataRow dr in db.getRows(dsPrevious))
            {
                string id = db.getValue(dr, "Id");
                if(id == currentId)
                    return true;
            }
            return false;
        }

        #region WNIOSKI_PRACA_ZDALNA

        public static bool EventWniosekPracaZdalna(string maTyp, string wnId)  // 20140704 dodanie dw do kierowników zastępujących
        {
            DataSet mail_data = GetWniosekUrlopowyData(wnId, false);
            DataRow dr = db.getRow(mail_data);
            string idAcc = db.getValue(dr, "IdKierAcc");
            string idKier = db.getValue(dr, "IdKierownika");
            string idPrac = db.getValue(dr, "IdPracownika");
            string idAutor = db.getValue(dr, "AutorId");
            try
            {
                switch (maTyp)
                {
                    case maWZ_SENT:     //4.1. mail do przełożonego akceptującego i bezpośredniego przełożonego (DW), pracownika (DW) - pracownik złożył wniosek urlopowy
                        DataSet dwAcc = App.FindUser(false, AppUser.rWnioskiZdalnaAcc);
                        
                        // dla acc
                        CheckSendMail2(maTyp, wnId, dwAcc, null, mail_data, -1);

                        /*
                        // dla pracownika
                        DataRow drPrac = db.getDataRow("select * from Pracownicy where Id = " + idPrac);
                        CheckSendMail(maTyp, wnId, drPrac, mail_data);

                        // dla zast pracownika
                        //DataSet dsPracZast = App.FindZast(drPrac);
                        //foreach(DataRow drPracZast in db.getRows(dsPracZast))
                        //    CheckSendMail(maTyp, wnId, drPracZast, mail_data);
                        */

                        // zastępujący acc
                        foreach (DataRow drZast in db.getRows(dwAcc))
                        {
                            DataSet dwZast = App.FindZast(drZast, AppUser.rWnioskiZdalnaAcc);
                            CheckSendMail2(maTyp, wnId, dwZast,null, mail_data, -1);
                        }
                        break;
                    case maWZ_ACCEPTED: //4.4. mail do admina, pracownika, bezpośredniego przełożonego - wniosek został zaakceptowany i czeka na wprowadzenie  <<< do admina nie ... bo za dużo było
                    case maWZ_REJECTED: //4.5. mail do pracownika, bezpośredniego przełożonego         - wniosek został odrzucony
                        // dla pracownika
                        DataRow drPrac = db.getDataRow("select * from Pracownicy where Id = " + idPrac);
                        CheckSendMail(maTyp, wnId, drPrac, mail_data);

                        // dla zast pracownika chyba nielogiczne
                        //DataSet dsPracZast = App.FindZast(drPrac);
                        //foreach(DataRow drPracZast in db.getRows(dsPracZast))
                        //    CheckSendMail(maTyp, wnId, drPracZast, mail_data);

                        DataSet dsAutorKier = db.getDataSet(String.Format(@"
                            select * from Pracownicy where Id = {0}
                            union
                            select * from Pracownicy where Id = {1}
                        ", idAutor, idKier));

                        foreach (DataRow drAutorKier in db.getRows(dsAutorKier))
                        {
                            CheckSendMail(maTyp, wnId, drAutorKier, mail_data);

                            DataSet dwZast = App.FindZast(drAutorKier, null);
                            CheckSendMail2(maTyp, wnId, dwZast, null, mail_data, -1);
                        }
                        
                    
                        //dla autora
                        //DataRow drAutor = db.getDataRow("select * from Pracownicy where Id = " + idAutor);
                        //CheckSendMail(maTyp, wnId, drAutor, mail_data);

                        ////dla kierasa
                        //DataRow drKier = db.getDataRow("select * from Pracownicy where Id = " + idKier);
                        //CheckSendMail(maTyp, wnId, drKier, mail_data);
                        



                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventWniosekPracaZdalna", ex.Message);
                return false;
            }
        }

        #endregion



    }
}
        