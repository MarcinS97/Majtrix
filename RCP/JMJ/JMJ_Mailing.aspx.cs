using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HRRcp.App_Code;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;

namespace HRRcp.JMJ
{
    public partial class JMJ_Mailing : System.Web.UI.Page
    {
        
        public String SmtpHost = "smtp.jmjbiznes.pl";
        public int Port = 25;
        public String User = "biuro@jmjbiznes.pl";
        public String Password = "BIUjmjB2017";
        public String From = "biuro@jmjbiznes.pl";
        
        /*
        public String SmtpHost = "mail.kdrs.pl";
        public int Port = 25;
        public String User = "admin@kdrs.pl";
        public String Password = "$V8t3c)S!!,)";
        public String From = "admin@kdrs.pl";
        */



        public String AttachmentName_a = "Regulamin nadsyłania prac na IX Zjazd PTN AIDS";
        public String AttachmentMime = "application/msword";

        public String Subject_a = "IX Zjazd PTN AIDS";
        public String Msg_a = @"Szanowni Państwo,
Uprzejmie informuję, że w ramach IX Zjazdu PTN AIDS odbędzie się Sesja Plakatowa. Wśród nadesłanych prac zostaną wyróżnione najlepsze do prezentacji ustnej.
Poniżej przedstawiam regulamin nadsyłania prac.
Prace należy zgłaszać na stronie www.jmjbiznes.pl/PTN/summary
 
serdecznie pozdrawiam
 
Brygida Knysz
Prezes PTN AIDS
";

        public String AttachmentName1_b = "IX Zjazd - Program Ramowy.doc";
        public String AttachmentName2_b = "IX Zjazd - regulamin nadsyłania prac.doc";

        public String Subject_b = "IX Zjazd PTN AIDS";
        public String Msg_b = @"Szanowni Państwo,
w załączeniu przesyłam Regulamin nadsyłania prac na IX Zjazd PTN AIDS oraz Program Zjazdu.
 
z poważaniem,
Małgorzata Machaj
";

        public String Subject = "IX Zjazd PTN AIDS";
        public String Msg_c = @"Szanowni Państwo,
uprzejmie informuję, że termin nadsyłania streszczeń prac na IX Zjazd PTN AIDS został przedłużony do 21 kwietnia 2017.

serdecznie pozdrawiam
Małgorzata Machaj
";

        public string Msg_d = @"Szanowni Państwo,
w imieniu Pani Prof. Brygidy Knysz - Prezesa PTN AIDS w załączeniu przesyłam Zawiadomienie o Walnym Zgromadzeniu.
 
Z poważaniem,
Małgorzata Machaj
";
        public String AttachmentName1_d = "Zawiadomienie i program Walnego Zgromadzenia 2017.pdf";
        
        public bool potwierdzenie_d = false;


        //-------------------------------------
        public string Msg_e = @"Szanowni Państwo,
w załączeniu przesyłam wstępny Program IX Zjazdu PTN AIDS. Ostateczny program zostanie wysłany najpóźniej do dnia 13 czerwca.
W przypadku pytań bardzo proszę o kontakt.

Z poważaniem,
Małgorzata Machaj
";
        public String AttachmentName1_e = "ZJAZD PTN AIDS 2017 _PROGRAM.pdf";
        public bool potwierdzenie_e = false;

        //-------------------------------------
        public string Msg = @"Szanowni Państwo,
w załączeniu przesyłam Program IX Zjazdu PTN AIDS.
Uprzejmie informuję, że w czwartek 22 czerwca kolacja jest w formie grilla poza hotelem. Zaproszenie na kolację znajdziecie Państwo w materiałach zjazdowych. W recepcji zjazdu uzyskacie Państwo informację o godzinie odjazdu autokarów. 
 
W przypadku pytań proszę o kontakt.
 
z poważaniem,
Małgorzata Machaj
";
        public String AttachmentName1 = "ZJAZD PTN AIDS 2017 _PROGRAM_08.06.pdf";
        public bool potwierdzenie = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            //--------------------------------------
            // zabezpieczenie
            //--------------------------------------

            App.Redirect(App.DefaultForm);
        }

        private void Send_a()
        {
            /*
            SqlConnection con = db.Connect(db.conStr);
            DataSet ds = db.getDataSet(con, "select * from Mailing");

            using (var fileStream = new FileStream(Server.MapPath(@"~/JMJ/att.doc"), FileMode.Open))
            {
                foreach (DataRow dr in db.getRows(ds))
                {
                    String to = db.getValue(dr, "Email");
                    if (!String.IsNullOrEmpty(to))
                    {

                        for (int i = 1; i <= 3; i++)  //20110930
                        {
                            int err = 0;
                            try
                            {
                                Mailing.SendMail2(SmtpHost, Port, User, Password, From, to, null, null, Subject, Msg, fileStream, AttachmentName, AttachmentMime);
                                Log.Info(420, to, String.Empty);
                                err = 0;
                            }
                            catch (SmtpException ex)
                            {
                                Log.Error(Log.t2SENDMAIL, "SendMail2 smtp: " + ex.StatusCode + " " + to, ex.Message);
                                err = -2;
                            }
                            catch (Exception ex)
                            {
                                Log.Error(Log.t2SENDMAIL, "SendMail2 " + to, ex.Message);
                                err = -1;
                            }

                            if (err == -1 || err == 0) break;
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
                                        Log.Error(Log.t2SENDMAIL, "Sending mail failed", to);
                                        break;
                                }
                            }
                        }

                    }
                }
            }
            db.Disconnect(con);
            Tools.ExecuteJavascript("alert('ok');");
             */
        }

        //-------------------------------------
        public static void SendMail2(string smtphost, int port, string user, string pass, string from, string to, string cc, string bcc, string subject, string message
            , Stream attachment1, string aFileName1, string mime1
            , Stream attachment2, string aFileName2, string mime2
            )
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
                    if (!String.IsNullOrEmpty(tto) && tto.Contains('@') && !Mailing.MailExists(mail.To, tto))
                        mail.To.Add(tto);
                }
            }

            if (!String.IsNullOrEmpty(cc))
            {
                string[] ccList = cc.Split(';', ',', ' ');
                foreach (string c in ccList)
                {
                    string tcc = c.Trim();
                    if (!String.IsNullOrEmpty(tcc) && tcc.Contains('@') && !Mailing.MailExists(mail.To, tcc) && !Mailing.MailExists(mail.CC, tcc))
                        mail.CC.Add(tcc);
                }
            }
            if (!String.IsNullOrEmpty(bcc))
            {
                string[] bccList = bcc.Split(';', ',', ' ');
                foreach (string bc in bccList)
                {
                    string tbcc = bc.Trim();
                    if (!String.IsNullOrEmpty(tbcc) && tbcc.Contains('@') && !Mailing.MailExists(mail.To, tbcc) && !Mailing.MailExists(mail.CC, tbcc) && !Mailing.MailExists(mail.Bcc, tbcc))
                        mail.Bcc.Add(tbcc);
                }
            }

            mail.Subject = subject;
            mail.Body = message;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.BodyEncoding = Encoding.UTF8;
            if (attachment1 != null)
            {
                //attachment.Seek(0, SeekOrigin.Begin);
                attachment1.Position = 0;
                mail.Attachments.Add(new Attachment(attachment1, aFileName1, mime1));
            }
            if (attachment2 != null)
            {
                //attachment.Seek(0, SeekOrigin.Begin);
                attachment2.Position = 0;
                mail.Attachments.Add(new Attachment(attachment2, aFileName2, mime2));
            }
            SmtpClient smtp = new SmtpClient(smtphost);  // "127.0.0.1"
            if (port > 0) smtp.Port = port;
            if (!String.IsNullOrEmpty(user))
            {
                System.Net.NetworkCredential c = new System.Net.NetworkCredential(user, pass);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = c;
            }

            if (Mailing.IsMailing)
                smtp.Send(mail);
        }


        public static /*bool*/ int SendMail2(string to_email, string cc, string bcc, string subject, string message, Stream attachment1, string filename1, string mime1, Stream attachment2, string filename2, string mime2)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            try
            {
                //SendMail2(settings.SMTPSerwer, 0, null, null,   //20150105
                SendMail2(settings._SMTPSerwer, settings.SMTPPort, settings.SMTPUser, settings.SMTPPass,
                    settings.Email, to_email, cc, bcc, subject, message,
                    attachment1, filename1, mime1,
                    attachment2, filename2, mime2
                    );
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

        //-------------------------------------------
        private void Send_b()
        {
            Tools.ShowMessage("Zdejmij zabezpieczenie");
            return; // zabezpieczenie

            SqlConnection con = db.Connect(db.conStr);
            DataSet ds = db.getDataSet(con, "select * from Mailing");

            using (var fileStream1 = new FileStream(Server.MapPath(@"~/JMJ/" + AttachmentName1_b), FileMode.Open))
            {
                using (var fileStream2 = new FileStream(Server.MapPath(@"~/JMJ/" + AttachmentName2_b), FileMode.Open))
                {
                    foreach (DataRow dr in db.getRows(ds))
                    {
                        String to = db.getValue(dr, "Email");
                        if (!String.IsNullOrEmpty(to))
                        {

                            for (int i = 1; i <= 3; i++)  //20110930
                            {
                                int err = 0;
                                try
                                {
                                    SendMail2(SmtpHost, Port, User, Password, From, to, null, null, Subject, Msg, fileStream1, AttachmentName1_b, AttachmentMime, fileStream2, AttachmentName2_b, AttachmentMime);
                                    Log.Info(420, to, String.Empty);
                                    err = 0;
                                }
                                catch (SmtpException ex)
                                {
                                    Log.Error(Log.t2SENDMAIL, "SendMail2 smtp: " + ex.StatusCode + " " + to, ex.Message);
                                    err = -2;
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(Log.t2SENDMAIL, "SendMail2 " + to, ex.Message);
                                    err = -1;
                                }

                                if (err == -1 || err == 0) break;
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
                                            Log.Error(Log.t2SENDMAIL, "Sending mail failed", to);
                                            break;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            db.Disconnect(con);
            Tools.ExecuteJavascript("alert('ok');");
        }

        private void Send_c()
        {
            Tools.ExecuteJavascript("alert('Zdejmij zabezpieczenie');");
            return; // zabezpieczenie

            SqlConnection con = db.Connect(db.conStr);
            DataSet ds = db.getDataSet(con, "select * from Mailing");

            foreach (DataRow dr in db.getRows(ds))
            {
                String to = db.getValue(dr, "Email");
                if (!String.IsNullOrEmpty(to))
                {

                    for (int i = 1; i <= 3; i++)  //20110930
                    {
                        int err = 0;
                        try
                        {
                            SendMail2(SmtpHost, Port, User, Password, From, to, null, null, Subject, Msg, null, null, null, null, null, null);
                            Log.Info(420, to, String.Empty);
                            err = 0;
                        }
                        catch (SmtpException ex)
                        {
                            Log.Error(Log.t2SENDMAIL, "SendMail2 smtp: " + ex.StatusCode + " " + to, ex.Message);
                            err = -2;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Log.t2SENDMAIL, "SendMail2 " + to, ex.Message);
                            err = -1;
                        }

                        if (err == -1 || err == 0) break;
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
                                    Log.Error(Log.t2SENDMAIL, "Sending mail failed", to);
                                    break;
                            }
                        }
                    }

                }
            }
            db.Disconnect(con);
            Tools.ExecuteJavascript("alert('ok');");
        }


        private void Send(string attachment, bool potwierdzenie)
        {
            //Tools.ExecuteJavascript("alert('Zdejmij zabezpieczenie');");
            //return; // zabezpieczenie

            SqlConnection con = db.Connect(db.conStr);

            
            
            DataSet ds = db.getDataSet(con, "select * from Mailing");
            /*
            DataSet ds = db.getDataSet(con, @"
select * from Mailing where Email not in 
(
select Info from Log 
where DataCzas > '2017-05-31 23:16:48.807'
)
order by 1
");
            */ 
            int cnt = db.getCount(ds);

            using (var fileStream = new FileStream(Server.MapPath(@"~/JMJ/" + attachment), FileMode.Open))
            {
                foreach (DataRow dr in db.getRows(ds))
                {
                    String to = db.getValue(dr, "Email");
                    if (!String.IsNullOrEmpty(to))
                    {

                        for (int i = 1; i <= 3; i++)  //20110930
                        {
                            int err = 0;
                            try
                            {
                                Mailing.SendMail2(SmtpHost, Port, User, Password, From, to, null, null, Subject, Msg, fileStream, attachment, AttachmentMime
                                    , potwierdzenie);
                                Log.Info(420, to, String.Empty);
                                err = 0;
                            }
                            catch (SmtpException ex)
                            {
                                Log.Error(Log.t2SENDMAIL, to + " SendMail2 smtp: " + ex.StatusCode /*+ " " + to*/, ex.Message);
                                err = -2;
                            }
                            catch (Exception ex)
                            {
                                Log.Error(Log.t2SENDMAIL, to /*"SendMail2 " + to*/, ex.Message);
                                err = -1;
                            }

                            if (err == -1 || err == 0) break;
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
                                        Log.Error(Log.t2SENDMAIL, "Sending mail failed", to);
                                        break;
                                }
                            }
                        }

                    }
                }
            }
            db.Disconnect(con);
            Tools.ExecuteJavascript("alert('ok');");
        }



        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            Send(AttachmentName1, potwierdzenie);
        }
    }
}