using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp.Scorecards.App_Code
{
    public class Mailing
    {
        public const string maSC_OKRESPROBNY = "SC_OKRESPROBNY";
        public const string maSC_WNACC = "SC_WNACC";
        public const string maSC_WNBACK = "SC_WNBACK";
        public const string maSC_WNREJ = "SC_WNREJ";
        public const string maSC_WNACCHR = "SC_WNACCHR";
        //public const string maSC_WNDEL = "SC_WNDEL";

        //Eventy są generalnie jeszcze do omówienia... bo trzeba określić jak wybrać do kogo je wysłać (KIERAS, ZARZ ?)

        public static DataSet GetWniosekData(string wniosekId, string hashLink, bool test)
        {
            const int cnt = 9;
            string select;
            string link = Tools.GetAppAddr2(null);
            string wnlink = Tools.AddrSetPage(link, hashLink);
            if (test)
                select = String.Format(@"select {1} as COUNT, 
Imie + ' ' + Nazwisko as WNIOSKUJACY, 
'Arkusz Przykładowy' as ARKUSZ, 
convert(varchar(10), GETDATE(), 20) as DATA,
'{2}' as WNLINK,
'{3}' as APPLINK,
{0} as AccId,
0 as Zarzad,
Id as IdPracownika,
Imie + ' ' + Nazwisko as AUTORAKCJI,
0 as Status 
from Pracownicy 
where Id = {0}", App.User.Id, cnt, wnlink, link);
            else
                select = String.Format(@"select {1} as COUNT,
P.Imie + ' ' + P.Nazwisko as WNIOSKUJACY, 
T.Nazwa as ARKUSZ, 
convert(varchar(10),W.Data,20) as DATA,
'{2}' as WNLINK,
'{3}' as APPLINK,
R.IdKierownika as AccId,    -- <<<<<<< to uzależnić od upr ???!!!
case when w.Pacc != -1 then 1 else 0 end as Zarzad,
w.IdPracownika as OwnerId,
P2.Imie + ' ' + P2.Nazwisko as AUTORAKCJI,
w.Status
from scWnioski W
left join scTypyArkuszy T on T.Id = W.IdTypuArkuszy
left join Przypisania R on R.IdPracownika = W.IdPracownika and R.Status = 1 and dbo.eom(W.Data) between R.Od and ISNULL(R.Do,'20990909')
left join Pracownicy P on P.Id = W.IdPracownika
left join Pracownicy P2 on P2.Id = {4}
where w.Id = {0}", wniosekId, cnt, wnlink, link, App.User.Id);
            return db.getDataSet(select);
        }

        public static bool EventWniosekPremiowy(string maTyp, string upr, string hashLink, string idWniosku)
        {
            try
            {
                DataSet mail_data = GetWniosekData(idWniosku, hashLink, false);
                string accId = db.getValue(db.getRow(mail_data), "AccId");
                string ownerId = db.getValue(db.getRow(mail_data), "OwnerId");
                int status = db.getInt(db.getRow(mail_data), "Status", 0);
                bool zarzad = db.getBool(db.getRow(mail_data), "Zarzad", false);
                switch (maTyp)
                {
                    case maSC_WNACC:
                        switch (status)
                        {
                            case 1:
                                SendMail(maTyp, idWniosku, mail_data, accId);
                                break;
                            case 2:
                                SendToGroup(maTyp, idWniosku, mail_data, AppUser.rScorecardsZarz, AppUser.rScorecardsWnAcc);
                                SendMail(maTyp, idWniosku, mail_data, null);
                                break;
                            case 3:
                                //SendToGroup(maTyp, idWniosku, mail_data, AppUser._rScorecardsHR);
                                SendMail(maTyp, idWniosku, mail_data, null);
                                break;
                        }
                        break;
                    case maSC_WNACCHR:
                        //SendToGroup(maTyp, idWniosku, mail_data, AppUser._rScorecardsHR);
                        SendMail(maTyp, idWniosku, mail_data, null);
                        break;
                    case maSC_WNBACK:
                        switch (status)
                        {
                            case 0:
                                SendMail(maTyp, idWniosku, mail_data, ownerId);
                                break;
                            case 1:
                                SendMail(maTyp, idWniosku, mail_data, accId);
                                break;
                        }
                        break;
                    case maSC_WNREJ:
                        SendMail(maTyp, idWniosku, mail_data, ownerId);
                        if (zarzad)
                        {
                            SendToGroup(maTyp, idWniosku, mail_data, AppUser.rScorecardsZarz, AppUser.rScorecardsWnAcc);
                        }
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2SENDMAIL, "EventScWniosek", ex.Message);
                return false;
            }
        }


        static void SendToGroup(string maTyp, string idWniosku, DataSet mail_data, params int[] rights)
        {
            DataSet dsGroup = App.FindUser(false, rights);
            foreach (DataRow drz in db.getRows(dsGroup))
            {
                String IdPracownika = db.getValue(drz, "Id");
                DataSet dsGroupZast = db.getDataSet(String.Format(@"
select P.* from Pracownicy P
inner join Zastepstwa Z on P.Id = Z.IdZastepujacy and Z.IdZastepowany in ({0}) and dbo.getdate(GETDATE()) between Z.Od and Z.Do
                            ", IdPracownika));
                HRRcp.App_Code.Mailing.CheckSendMail2(maTyp, idWniosku, null, dsGroupZast, mail_data, -1); // akceptujący
            }
            HRRcp.App_Code.Mailing.CheckSendMail2(maTyp, idWniosku, dsGroup, null, mail_data, -1);
        }



        static void SendMail(string maTyp, string idWniosku, DataSet mail_data, string accId)
        {
            if (!String.IsNullOrEmpty(accId)) // jak null to tylko admini
            {
                DataSet dsDW = db.getDataSet(String.Format(@"
select P.* from Pracownicy P
inner join Zastepstwa Z on P.Id = Z.IdZastepujacy and Z.IdZastepowany in ({0}) and dbo.getdate(GETDATE()) between Z.Od and Z.Do
                            ", accId));
                HRRcp.App_Code.Mailing.CheckSendMail2(maTyp, idWniosku, AppUser.GetDataDs(accId), dsDW, mail_data, -1); // akceptujący
            }


            DataSet ds = App.FindUser(false, AppUser._rScorecardsAdmin);  // do administratorów sc
            HRRcp.App_Code.Mailing.CheckSendMail2(maTyp, idWniosku, ds, null, mail_data, -1);
        }


    }
}
