using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using HRRcp.Controls.Reports;
using HRRcp.Controls;
using HRRcp.RCP.Controls.Harmonogram;

namespace HRRcp
{
    /// <summary>
    /// KDR RCP Main Service
    /// </summary>
    [WebService(Namespace = "http://kdrrcp.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.     
    [System.Web.Script.Services.ScriptService]
    public class main : System.Web.Services.WebService
    {
        SqlConnection fcon = null;

        private SqlConnection con
        {
            get 
            {
                if (fcon == null) db.DoConnect(ref fcon);
                return fcon;
            }
        }

        private void dbDisconnect()
        {
            if (fcon != null)
                db.DoDisconnect(ref fcon);
        }
        //-------------------------------
        [WebMethod]
        public string IloscDniPrac(string dataOd, string dataDo)
        {
            DateTime? dOd = Tools.StrToDateTime(dataOd);
            DateTime? dDo = Tools.StrToDateTime(dataDo);
            if (dOd != null && dDo != null)
            {
                int dni = Worktime.GetIloscDniPrac(con, (DateTime)dOd, (DateTime)dDo);
                dbDisconnect();
                return dni.ToString();
            }
            else
                return null;
        }

        [WebMethod]
        public string IloscDniPracDataDo(string dataOd, string ilDni)
        {
            DateTime? d = Tools.StrToDateTime(dataOd);
            int il = Tools.StrToInt(ilDni, -1);
            if (d != null && il != -1)
            {
                DateTime dd = Worktime.GetIloscDniPracDataDo(con, (DateTime)d, il);
                dbDisconnect();
                return Tools.DateToStr(dd);
            }
            else
                return null;
        }

        [WebMethod]
        public string IloscDniPracDataOd(string dataDo, string ilDni)
        {
            DateTime? d = Tools.StrToDateTime(dataDo);
            int il = Tools.StrToInt(ilDni, -1);
            if (d != null && il != -1)
            {
                DateTime dd = Worktime.GetIloscDniPracDataOd(con, (DateTime)d, il);
                dbDisconnect();
                return Tools.DateToStr(dd);
            }
            else
                return null;
        }
        //-----------------------------
        [WebMethod]
        public IloscDniPrac2Ret IloscDniPrac2(string[] par)
        {
            DateTime? d1 = Tools.StrToDateTime(par[0]);
            DateTime? d2 = Tools.StrToDateTime(par[1]);

            int il = Tools.StrToInt(par[2], -1);   //ilość dni, nawet jak na godziny to ilośc dni jest = 1 
            
            string g = par[3];
            IloscDniPrac2Ret ret = new IloscDniPrac2Ret();
            ret.field = 0;
            ret.value = null;
            switch (par[4])
            {
                case "1":
                    if (d1 != null)
                        if (d2 != null) ret.field = 3;      // ilość
                        else if (il != -1) ret.field = 2;   // do
                    break;
                case "11":  //20160504
                    if (d1 != null)
                        if (il != -1) ret.field = 2;        // zawsze do, opieka
                    break;
                case "2":
                    if (d2 != null)
                        if (d1 != null) ret.field = 3;      // ilość
                        else if (il != -1) ret.field = 1;   // od         
                    break;
                case "3":
                    if (il != -1)
                        if (d1 != null) ret.field = 2;      // do 
                        else if (d2 != null) ret.field = 1; // od
                    break;
            }
            switch (ret.field)
            {
                case 1:
                    if (d2 != null && il != -1)
                    {
                        ret.value = Tools.DateToStr(Worktime.GetIloscDniPracDataOd(con, (DateTime)d2, il));
                    }
                    break;
                case 2:
                    if (d1 != null && il != -1)
                    {
                        ret.value = Tools.DateToStr(Worktime.GetIloscDniPracDataDo(con, (DateTime)d1, il));
                    }
                    break;
                case 3:
                    if (d1 != null && d2 != null && (DateTime)d1 <= (DateTime)d2)
                    {
                        ret.value = Worktime.GetIloscDniPrac(con, (DateTime)d1, (DateTime)d2).ToString();
                    }
                    break;
            }
            dbDisconnect();
            return ret;
        }

        public class IloscDniPrac2Ret
        {
            public string value { get; set; }
            public int field { get; set; }
        }




        //----- Podział Ludzi -----
        const string right1 = "x";
        const string right0 = "-";

        private bool UpdateClass(SqlConnection con, bool cc, int pracId, string c, bool set)
        {
            DataRow drUser = db.getDataRow(con, String.Format("select Id, Login, Nazwisko + ' ' + Imie as Pracownik, KadryId from Pracownicy where Id = {0}", pracId));
            if (drUser != null)
            {
                string nrew = db.getValue(drUser, "KadryId");
                DataRow drPrawa = db.getDataRow(con, String.Format("select Id from ccPrawa where UserId={0} and CC='{1}'", pracId, c));
                bool ok = true;
                if (set)
                {
                    if (drPrawa == null)
                    {
                        string idcc = null;
                        if (cc) idcc = db.getScalar(con, String.Format("select Id from CC where cc = '{0}'", c));
                        if (String.IsNullOrEmpty(idcc)) idcc = "0";
                        ok = db.insert(con, "ccPrawa", 0, "UserId,Login,IdCC,CC",
                            db.getInt(drUser, 0),
                            db.strParam(db.getValue(drUser, 1)),
                            idcc,
                            db.strParam(c));
                    }
                }
                else
                {
                    ok = db.execSQL(con, "delete from ccPrawa where Id = " + db.getValue(drPrawa, 0));
                }
                
                if (ok)
                {
                    Log.Info(Log.RIGHTS,
                        set ? "Nadanie uprawnienia CC" : "Usunięcie uprawnienia CC",
                        String.Format("Pracownik: {0} {1} Klasyfikacja: {2}", nrew, db.getValue(drUser, 2), c));
                }
                else
                {
                    Tools.ShowErrorLog(Log.RIGHTS,
                        String.Format("UpdateClassCC({0},'{1}','{2}')", cc ? 1 : 0, nrew, c),
                        "Błąd podczas nadawania / usuwania uprawnienia.");
                }
                return ok;
            }
            Tools.ShowErrorLog(Log.RIGHTS,
                String.Format("UpdateRightsCC({0},{1})", pracId, c),
                "Niepoprawne parametry wywołania.");
            return false;
        }

        private bool UpdateClassAll(SqlConnection con, int pracId)   
        {
            DataRow dr = db.getDataRow(con, String.Format(@"
select top 1 * from CC 
left join ccPrawa R on R.UserId = {0} and R.IdCC = CC.Id 
where R.Id is null
                ", pracId));
            bool set = dr != null;  // znalazł cc bez prawa
            bool ok;
            if (set)
            {
                ok = db.execSQL(con, String.Format(@"
declare @pid int
declare @login nvarchar(100)
set @pid = {0}
select @login = Login from Pracownicy where Id = @pid

insert into ccPrawa
select @pid, @login, CC.Id, CC.CC, 0, 0 from CC 
left join ccPrawa R on R.UserId = @pid and R.IdCC = CC.Id 
where R.Id is null
                ", pracId));
            }
            else
            {
                ok = db.execSQL(con, String.Format("delete from ccPrawa where UserId = {0} and IdCC != 0", pracId));
            }
            if (ok)
            {
                Log.Info(Log.RIGHTS, set ? "Nadanie uprawnienia CC ALL" : "Usunięcie uprawnienia CC ALL", String.Format("Pracownik: {0}", pracId));
            }
            else
            {
                Log.Error(Log.RIGHTS, String.Format("UpdateClassAll({0})", pracId), "Błąd podczas nadawania / usuwania uprawnienia CC ALL.");
                //Tools.ShowErrorLog(Log.RIGHTS,
                //    String.Format("UpdateClassCCAll({0})", pracId),
                //    "Błąd podczas nadawania / usuwania uprawnienia CC ALL.");
            }
            return ok;
        }
        //---------------------------------------------
        private bool BadaniaWstChangeAllUpr(SqlConnection con, int pracId)    // przeniesiona do cmd w cntBadaniaWstKolUpr
        {
            const string BadaniaWstKolUpr = "BadaniaWstKolUpr";
            const int maxCol = 100;

            DataRow dr = db.getDataRow(con, String.Format("select Uprawnienia from BadaniaWstKolUpr where IdPrac = {0}", pracId));
            string r = dr != null ? db.getValue(dr, 0) : null;

            //int len = HRRcp.BadaniaWstepne.Controls.cntBadaniaWst.lastColumnRight;
            if (String.IsNullOrEmpty(r)) r = new String('1', maxCol);
            else
                switch (r[0])
                {
                    case '0':
                        r = new String('1', maxCol);
                        break;
                    case '1':
                        r = new String('2', maxCol);
                        break;
                    case '2':
                        r = new String('3', maxCol);
                        break;
                    default:
                    case '3':
                        r = new String('0', maxCol);
                        break;
                }
            bool ok;
            if (dr == null)
                ok = db.insert(con, BadaniaWstKolUpr, 0, "IdPrac,Uprawnienia", pracId, db.strParam(r));
            else
                ok = db.update(con, BadaniaWstKolUpr, 1, "Uprawnienia", "IdPrac={0}", pracId, db.strParam(r));
            return ok;                     
        }

		private bool UpdateClassUpr(SqlConnection con, bool cc, int pracId, string c, bool set)
        {
            DataRow drUser = db.getDataRow(con, String.Format("select Id, Login, Nazwisko + ' ' + Imie as Pracownik, KadryId from Pracownicy where Id = {0}", pracId));
            if (drUser != null)
            {
                string nrew = db.getValue(drUser, "KadryId");
                DataRow drPrawa = db.getDataRow(con, String.Format("select Id from ccUprawnienia where UserId={0} and CC='{1}'", pracId, c));
                bool ok = true;
                if (set)
                {
                    if (drPrawa == null)
                    {
                        string idcc = null;
                        if (cc) idcc = db.getScalar(con, String.Format("select Id from CC where cc = '{0}'", c));
                        if (String.IsNullOrEmpty(idcc)) idcc = "0";
                        ok = db.insert(con, "ccUprawnienia", 0, "UserId,Login,typ,IdCC,CC",
                            db.getInt(drUser, 0),
                            db.strParam(db.getValue(drUser, 1)),
                            "'Limity'",
                            idcc,
                            db.strParam(c));
                    }
                }
                else
                {
                    ok = db.execSQL(con, "delete from ccUprawnienia where Id = " + db.getValue(drPrawa, 0));
                }


                if (ok)
                {
                    Log.Info(Log.RIGHTS,
                        set ? "Nadanie uprawnienia CC" : "Usunięcie uprawnienia CC",
                        String.Format("Pracownik: {0} {1} Klasyfikacja: {2}", nrew, db.getValue(drUser, 2), c));
                }
                else
                {
                    Tools.ShowErrorLog(Log.RIGHTS,
                        String.Format("UpdateClassCC({0},'{1}','{2}')", cc ? 1 : 0, nrew, c),
                        "Błąd podczas nadawania / usuwania uprawnienia.");
                }
                return ok;
            }
            Tools.ShowErrorLog(Log.RIGHTS,
                String.Format("UpdateRightsCC({0},{1})", pracId, c),
                "Niepoprawne parametry wywołania.");
            return false;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateRightCC(int typ, int pid, string par, string currvalue)  // true ok
        {
            AppUser user = AppUser.CreateOrGetSession();
            string ret = null;
            if (user.HasAccess && user.HasRight(AppUser.rPodzialLudziAdm))
            //if (App.User.HasAccess && App.User.HasRight(AppUser.rPodzialLudziAdm))
            {
                bool set = currvalue == right0;    // stan obecny -> prawo nadane = true
                switch (typ)
                {
                    case 1:     //prawa
                        int r = Tools.StrToInt(par, -1);
                        int v = set ? 1 : 0; 
                        bool ok = db.execSQL(con, String.Format("execute dbo.SetRight {0},{1},{2}", pid, r, v));

                        if (ok)
                        {
                            Log.Info(Log.RIGHTS,
                                set ? "Nadanie uprawnienia CC" : "Usunięcie uprawnienia CC",
                                String.Format("Pracownik: {0} Uprawnienie: {1}", pid, r));
                            ret = set ? right1 : right0;
                        }
                        else
                        {
                            Tools.ShowErrorLog(Log.RIGHTS,
                                String.Format("UpdateRightsCC({0},{1},{2},{3})", typ, pid, par, currvalue),
                                "Błąd podczas nadawania / usuwania uprawnienia.");
                        }
                        break;
                    case 2:     //klasyfikacja
                        if (UpdateClass(con, false, pid, par, set))
                            ret = set ? par: right0;
                        break;
                    case 3:     //cc
                        if (UpdateClass(con, true, pid, par, set))
                            ret = set ? par : right0;
                        break;
                    case 30:     //ALL cc
                        if (UpdateClassAll(con, pid))
                            ret = "...";
                        break;
                    case 4:     //klas - wynagrodzenie
                        if (UpdateClass(con, true, pid, par, set))
                            ret = set ? right1: right0;
                        break;
                    case 5:     // limity NN na cc
                        if (UpdateClassUpr(con, true, pid, par, set))
                            ret = set ? par : right0;
                        break;
                    case 6:     // badania wstępne
                        if (BadaniaWstChangeAllUpr(con, pid))
                            ret = "bind!";
                        break;
                }
            }
            dbDisconnect();
            return ret;
        }



        //----------------------------------------------------------------------------------------------
        //-------------------------------------------- IPO ---------------------------------------------
        //----------------------------------------------------------------------------------------------
        const string rightIPO1 = "x";
        const string rightIPO0 = "-";

        private bool UpdateClassIPO(SqlConnection con, bool cc, int pracId, string c, bool set, string rola)
        {
            DataRow drUser = db.getDataRow(con, String.Format("select Id, Login, Nazwisko + ' ' + Imie as Pracownik, KadryId from Pracownicy where Id = {0}", pracId));
            if (drUser != null)
            {
                string nrew = db.getValue(drUser, "KadryId");
                DataRow drPrawa = db.getDataRow(con, String.Format("select Id from IPO_ccPrawa where UserId={0} and CC='{1}' and RolaId={2}", pracId, c, rola));
                bool ok = true;
                if (set)
                {
                    if (drPrawa == null)
                    {
                        string idcc = null;
                        if (cc) idcc = db.getScalar(con, String.Format("select Id from CC where cc = '{0}'", c));
                        if (String.IsNullOrEmpty(idcc)) idcc = "0";
                        ok = db.insert(con, "IPO_ccPrawa", 0, "UserId,Login,IdCC,CC,RolaId",
                            db.getInt(drUser, 0),
                            db.strParam(db.getValue(drUser, 1)),
                            idcc,
                            db.strParam(c),
                            rola);
                    }
                }
                else
                {
                    ok = db.execSQL(con, "delete from IPO_ccPrawa where Id = " + db.getValue(drPrawa, 0));
                }

                if (ok)
                {
                    Log.Info(Log.RIGHTS,
                        set ? "Nadanie uprawnienia CC" : "Usunięcie uprawnienia CC",
                        String.Format("Pracownik: {0} {1} Klasyfikacja: {2}", nrew, db.getValue(drUser, 2), c));
                }
                else
                {
                    Tools.ShowErrorLog(Log.RIGHTS,
                        String.Format("UpdateClassCC({0},'{1}','{2}')", cc ? 1 : 0, nrew, c),
                        "Błąd podczas nadawania / usuwania uprawnienia.");
                }
                return ok;
            }
            Tools.ShowErrorLog(Log.RIGHTS,
                String.Format("UpdateRightsCC({0},{1})", pracId, c),
                "Niepoprawne parametry wywołania.");
            return false;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateRightIPO(int typ, int pid, string par, string currvalue, string rola)  // true ok
        {
            AppUser user = AppUser.CreateOrGetSession();
            string ret = null;
            if (user.HasAccess)
            //if (App.User.HasAccess && App.User.HasRight(AppUser.rPodzialLudziAdm))
            {
                bool set = currvalue == right0;    // stan obecny -> prawo nadane = true
                switch (typ)
                {
                    case 1:     //prawa
                        int r = Tools.StrToInt(par, -1);
                        int v = set ? 1 : 0;
                        bool ok = db.execSQL(con, String.Format("execute dbo.SetRight {0},{1},{2}", pid, r, v));

                        if (ok)
                        {
                            Log.Info(Log.RIGHTS,
                                set ? "Nadanie uprawnienia CC" : "Usunięcie uprawnienia CC",
                                String.Format("Pracownik: {0} Uprawnienie: {1}", pid, r));
                            ret = set ? right1 : right0;
                        }
                        else
                        {
                            Tools.ShowErrorLog(Log.RIGHTS,
                                String.Format("UpdateRightsCC({0},{1},{2},{3})", typ, pid, par, currvalue),
                                "Błąd podczas nadawania / usuwania uprawnienia.");
                        }
                        break;
                    case 3:     //cc
                        if (UpdateClassIPO(con, true, pid, par, set, rola))
                            ret = set ? par : right0;
                        break;
                }
            }
            dbDisconnect();
            return ret;
        }

        [WebMethod(EnableSession = true)]
        public string[] SzukajProdukty(string prefixText)
        {
            AppUser user = AppUser.CreateOrGetSession();

            DataSet dataSet = db.getDataSet(con, "SELECT TOP 15 IPO_Produkty.Id, IPO_Produkty.Nazwa + ' | ' + IPO_Dostawcy.Nazwa + ' | ' + COALESCE(CAST(IPO_Produkty.Cena AS varchar(50)) + ' ' +IPO_Produkty.Waluta,'') + COALESCE('/' + IPO_Produkty.Jednostka, '') + ' ' AS Label, IPO_Produkty.Nazwa, Opis FROM IPO_Produkty JOIN IPO_Dostawcy ON IPO_Produkty.IdDostawcy = IPO_Dostawcy.Id LEFT JOIN IPO_ProduktyStat ON IPO_Produkty.Id = IPO_ProduktyStat.IdProduktu AND IPO_ProduktyStat.UserId = " + user.Id + "  WHERE IPO_Produkty.Nazwa LIKE '%" + prefixText + "%' ORDER BY IPO_ProduktyStat.Licznik DESC, IPO_Produkty.Nazwa");

            string[] result = new string[dataSet.Tables[0].Rows.Count];
            int i = 0;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["Label"].ToString(), row["Id"].ToString());
                result.SetValue(item, i);
                i++;
            }
            return result;
        }

        //----- Badania Wstępne -----


        [WebMethod(EnableSession = true)]
        public string UpdateRightBadWstKol(int pid, string par, string currValue)  // true ok
        {
            AppUser user = AppUser.CreateOrGetSession();
            string ret = null;
            if (user.HasAccess && user.HasRight(AppUser.rPodzialLudziAdm)) // rPodzialLudziAdm?
            {
                int nextI = (HRRcp.BadaniaWstepne.Templates.CrTemplateGroupCreator.BadWstRightStrs.TakeWhile(a => string.Compare(a, currValue) != 0).Count() + 1) % 4;
                ret = HRRcp.BadaniaWstepne.Templates.CrTemplateGroupCreator.BadWstRightStrs[nextI];
                int i = Tools.StrToInt(par, -1);
                int? bid = db.getScalar<int>(con, string.Format(
                    "select A.id from BadaniaWstKolUpr A RIGHT JOIN Pracownicy B on A.IdPrac = B.Id where B.Id = {0}", pid));
                bool UprExist = bid.HasValue;

                if (!UprExist)
                {
                    string sql = string.Format("INSERT INTO BadaniaWstKolUpr(IdPrac, Uprawnienia) VALUES({0}, '{1}')",
                        pid, Tools.getStringFromNChar("0", i) + nextI.ToString());

                    db.execSQL(con, sql);
                }
                else
                {
                    string str = db.getScalar(con, string.Format("select Uprawnienia from BadaniaWstKolUpr where id = {0}", bid.Value));
                    if (str.Length <= i)
                        str += Tools.getStringFromNChar("0", i - str.Length);
                    string newStr = "{0}";
                    if (i != 0)
                    {
                        newStr = str.Substring(0, i) + newStr;
                    }
                    if (i < str.Length - 1)
                    {
                        newStr += str.Substring(i + 1);
                    }
                    newStr = string.Format(newStr, nextI);
                    db.execSQL(con, string.Format("UPDATE BadaniaWstKolUpr SET Uprawnienia = '{0}' where id = {1}", newStr, bid.Value));
                }


            }
            dbDisconnect();
            return ret;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetcntBadLastId()
        {
            SqlConnection c = new SqlConnection(db.conStr);
            c.Open();
            int? v = db.getScalar<int>(c, "SELECT UpdateId FROM UpdateIncr where id = " + 1); // TODO: zmienic 1 na id z argumentu
            c.Close();
            if (v.HasValue)
                return new JavaScriptSerializer().Serialize(v);
            else
            {
                return new JavaScriptSerializer().Serialize(0);
            }
        }















        /*
        [WebMethod(EnableSession = true)]
        public string UpdateRightBadWstKol(int pid, string par, string currValue)  // true ok
        {
            AppUser user = AppUser.CreateOrGetSession();
            string ret = null;
            if (user.HasAccess && user.HasRight(AppUser.rPodzialLudziAdm)) // rPodzialLudziAdm?
            {
                int nextI = (HRRcp.BadaniaWstepne.Templates.CrTemplateGroupCreator.BadWstRightStrs.TakeWhile(a => string.Compare(a, currValue) != 0).Count() + 1) % 3;
                ret = HRRcp.BadaniaWstepne.Templates.CrTemplateGroupCreator.BadWstRightStrs[nextI];
                int i = Tools.StrToInt(par, -1);

                //int? bid = db.getScalar<int>(con, string.Format(
                //    "select A.id from BadaniaWstKolUpr A RIGHT JOIN Pracownicy B on A.IdPrac = B.Id where B.Id = {0}", pid));
                //bool UprExist = bid.HasValue;

                int bid = Tools.StrToInt(db.getScalar(con, string.Format(
                    "select A.id from BadaniaWstKolUpr A RIGHT JOIN Pracownicy B on A.IdPrac = B.Id where B.Id = {0}", pid)), -1);
                bool UprExist = bid != -1;


                if (!UprExist)
                {
                    string sql = string.Format("INSERT INTO BadaniaWstKolUpr(IdPrac, Uprawnienia) VALUES({0}, '{1}')",
                        pid, Tools.getStringFromNChar("0", i) + nextI.ToString());

                    db.execSQL(con, sql);
                }
                else
                {
                    //string str = db.getScalar(con, string.Format("select Uprawnienia from BadaniaWstKolUpr where id = {0}", bid.Value));
                    string str = db.getScalar(con, string.Format("select Uprawnienia from BadaniaWstKolUpr where id = {0}", bid));
                    if (str.Length <= i)
                        str += Tools.getStringFromNChar("0", i - str.Length);
                    string newStr = "{0}";
                    if (i != 0)
                    {
                        newStr = str.Substring(0, i) + newStr;
                    }
                    if (i < str.Length - 1)
                    {
                        newStr += str.Substring(i + 1);
                    }
                    newStr = string.Format(newStr, nextI);
                    //db.execSQL(con, string.Format("UPDATE BadaniaWstKolUpr SET Uprawnienia = '{0}' where id = {1}", newStr, bid.Value));
                    db.execSQL(con, string.Format("UPDATE BadaniaWstKolUpr SET Uprawnienia = '{0}' where id = {1}", newStr, bid));
                }


            }
            dbDisconnect();
            return ret;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetcntBadLastId()
        {
            SqlConnection c = new SqlConnection(db.conStr);
            c.Open();

            //int? v = db.getScalar<int>(c, "SELECT UpdateId FROM UpdateIncr where id = " + 1); // TODO: zmienic 1 na id z argumentu
            //c.Close();
            //if (v.HasValue)
            //    return new JavaScriptSerializer().Serialize(v);
            //else
            //{
            //    return new JavaScriptSerializer().Serialize(0);
            //}

            int v = Tools.StrToInt(db.getScalar(c, "SELECT UpdateId FROM UpdateIncr where id = 1"), 0);
            c.Close();
            return new JavaScriptSerializer().Serialize(v);
        }        
        */
        
        
        
        
        
        
        
        
        
        
        
        /*
        [WebMethod(EnableSession = true)]
        public string UpdateRightBadWstKol(int pid, string par, string currValue)  // true ok
        {
            AppUser user = AppUser.CreateOrGetSession();
            string ret = null;
            if (user.HasAccess && user.HasRight(AppUser.rPodzialLudziAdm)) // rPodzialLudziAdm?
            {
                int nextI = (Tools.BadWstRightStrs.TakeWhile(a => string.Compare(a, currValue) != 0).Count() + 1) % 3;
                ret = Tools.BadWstRightStrs[nextI];
                int i = Tools.StrToInt(par, -1);
                if (i == -1)
                    throw new Exception();
                //int? bid = db.getScalar<int>(con, string.Format(
                //    "select A.id from BadaniaWstKolUpr A RIGHT JOIN Pracownicy B on A.IdPrac = B.Id where B.Id = {0}", pid));
                //bool UprExist = bid.HasValue;
                int bid = Tools.StrToInt(db.getScalar(con, string.Format(
                    "select A.id from BadaniaWstKolUpr A RIGHT JOIN Pracownicy B on A.IdPrac = B.Id where B.Id = {0}", pid)), -1);
                bool UprExist = bid != -1;
                if (!UprExist)
                {
                    string sql = string.Format("INSERT INTO BadaniaWstKolUpr(IdPrac, Uprawnienia) VALUES({0}, '{1}')",
                        pid, Tools.StrRepeat("0", i) + nextI.ToString());

                    db.execSQL(con, sql);
                }
                else
                {
                    //string str = db.getScalar(con, string.Format("select Uprawnienia from BadaniaWstKolUpr where id = {0}", bid.Value));
                    string str = db.getScalar(con, string.Format("select Uprawnienia from BadaniaWstKolUpr where id = {0}", bid));
                    if (str.Length <= i)
                        str += Tools.StrRepeat("0", i - str.Length);
                    string newStr = "{0}";
                    if (i != 0)
                    {
                        newStr = str.Substring(0, i) + newStr;
                    }
                    if (i < str.Length - 1)
                    {
                        newStr += str.Substring(i + 1);
                    }
                    newStr = string.Format(newStr, nextI);
                    //db.execSQL(con, string.Format("UPDATE BadaniaWstKolUpr SET Uprawnienia = '{0}' where id = {1}", newStr, bid.Value));
                    db.execSQL(con, string.Format("UPDATE BadaniaWstKolUpr SET Uprawnienia = '{0}' where id = {1}", newStr, bid));
                }


            }
            dbDisconnect();
            return ret;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetcntBadLastId()
        {
            SqlConnection c = new SqlConnection(db.conStr);
            c.Open();
            
            //int? v = db.getScalar<int>(c, "SELECT UpdateId FROM UpdateIncr where id = 1");
            //if (v != -1)
            //    return new JavaScriptSerializer().Serialize(v);
            //else
            //{
            //    return new JavaScriptSerializer().Serialize(0);
            //}

            int v = Tools.StrToInt(db.getScalar(c, "SELECT UpdateId FROM UpdateIncr where id = 1"), 0);
            c.Close();
            return new JavaScriptSerializer().Serialize(v);
        }
        */
        //---------------------------
        [WebMethod]
        public void UpdateWnioskiUrlopowe()
        {
            HRRcp.Controls.Portal.cntWnioskiUrlopowe.UpdateEntered();
        }
        //---------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetInfoBoxContent(int id)
        {
            string html1 = null;
            string html2 = null;
            string script = null;
            AppUser user = AppUser.CreateOrGetSession();
            if (user.HasAccess)
            {
                SqlConnection con = db.Connect(db.conStr);
                DataRow dr = db.getDataRow(con, String.Format(@"
declare @id int
declare @Rights varchar(500)
set @id = {0}
set @Rights = '{1}'

select
  A.Id, A.Grupa, A.ParentId, A.Typ, A.Command
, ISNULL(A.Html1, A.Html2) Html1, ISNULL(A.Html2, A.Html1) Html2, A.HtmlEmpty, A.CssClass, A.Css, A.NowaLinia
, A.Sql, A.Par1, A.Par2
, A.Kolejnosc, A.Aktywny, A.Rights
from SqlBoxes A
where A.Aktywny = 1 and (A.Rights is null or dbo.CheckRightsExpr(@Rights, A.Rights) = 1) and A.Id = @id
order by A.Kolejnosc, A.Id
                ", id, user.Rights));
                if (dr != null)
                {
                    string sql  = db.getValue(dr, "Sql");
                    html1 = db.getValue(dr, "Html1");
                    html2 = db.getValue(dr, "Html2");
                    //script = db.getValue(dr, "Script");
                    sql = cntReport2.PrepareStdParams(sql);
                    DataSet ds = db.getDataSet(con, sql);
                    //Tools.PrepareZnaczniki(ref html1, ds);
                    //Tools.PrepareZnaczniki(ref html2, ds);
                    if (db.getCount(ds) > 0)
                    {
                        cntInfoBox.PrepareZnaczniki(ref html1, ds);
                        cntInfoBox.PrepareZnaczniki(ref html2, ds);
                    }
                    else
                    {
                        html1 = null;  // ukryj box
                        html2 = null;
                    }
                }
                db.Disconnect(con);
            }
            var obj = new { id, html1, html2 };
            return new JavaScriptSerializer().Serialize(obj);
        }
        
        /*
        declare @id int
        declare @Rights varchar(500)
        set @id = {0}
        set @Rights = '{1}'

        select
        A.Id, A.Grupa, A.ParentId, A.Command, A.Html, A.CssClass, A.NowaLinia, A.Sql, A.Par1, A.Par2, A.Kolejnosc, A.Aktywny, A.Rights,
        B.Id bId, B.Grupa bGrupa, B.ParentId bParentId, B.Command bCommand, B.Html bHtml, B.CssClass bCssClass, B.NowaLinia bNowaLinia, B.Sql bSql, B.Par1 bPar1, B.Par2 bPar2, B.Kolejnosc bKolejnosc, B.Aktywny bAktywny, B.Rights bRights
        from SqlBoxes A
        left join SqlBoxes B on B.ParentId = A.Id and B.Aktywny = 1
        --where A.Grupa = @Grupa and (A.ParentId is null or A.ParentId = 0) and A.Aktywny = 1 and (A.Rights is null or dbo.CheckRightsExpr(@Rights, A.Rights) = 1) and (A.Mode is null or A.Mode & @Mode != 0)
        where A.Aktywny = 1 and (A.Rights is null or dbo.CheckRightsExpr(@Rights, A.Rights) = 1) and A.Id = @id
         */

        //---------------------------
        [WebMethod]
        public void Test()
        {
            int x = 0;
        }





        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public string[] GetCodes(string prefixText, int count)
        {
            List<String> codes = new List<String>();

            DataTable dt = db.getDataSet(con, "select distinct Kod from rcpKodyPracKody order by Kod").Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                codes.Add(db.getValue(dr, 0));
            }
            dbDisconnect();
            return codes.ToArray();
        }



        //-----------------------------
        // Harmonogram 
        //-----------------------------
        const int tHarmPracAktywny       = 1;
        const string vHarmPracAktywny    = "TAK";
        const string vHarmPracNieaktywny = "NIE";

        [WebMethod(EnableSession = true)]
        public string ajaxHarmonogram(int typ, int pid, string par, string currvalue)  // true ok
        {
            AppUser user = AppUser.CreateOrGetSession();
            string ret = null;
            if (user.HasAccess && user.IsAdmin)
            {
                bool set = Tools.Substring(currvalue, 0, 3) == vHarmPracNieaktywny;    // stan obecny -> ustawić: TAK / NIE yyyy-mm-dd
                switch (typ)
                {
                    case tHarmPracAktywny:     //aktywny
                        int r = Tools.StrToInt(par, -1);
                        int v = set ? 1 : 0;
                        DateTime data = DateTime.Today;
                        string retFmt;
                        bool ok = db.execSQL(con, HarmonogramSQL.SetPracAktywny(pid, v, data, out retFmt));  // retFmt nie jest potrzebny, bo binduje się cały raport, a nie tylko odświeżam pole, docelowo pomysleć nad przekazaniem contentu do dowolnej kontrolki (div, input)
                        if (ok)
                        {
                            Log.Info(Log.HARMONOGRAM,
                                set ? "Aktywowanie pracownika" : "Deaktywacja pracownika",
                                String.Format("Pracownik: {0}", pid));
                            //ret = set ? vHarmPracAktywny : String.Format(retFmt, vHarmPracNieaktywny, Tools.DateToStr(data));
                            ret = set ? vHarmPracAktywny : vHarmPracNieaktywny;
                        }
                        else
                        {
                            //Tools.ShowErrorLog(Log.HARMONOGRAM,
                            Log.Error(Log.HARMONOGRAM,
                                String.Format("ajaxHarmonogram({0},{1},{2},{3})", typ, pid, par, currvalue),
                                "Błąd podczas aktualizacji statusu pracownika.");
                        }
                        break;

                    
                    
                    
                    
                    /*                    
                    case 2:     //klasyfikacja
                        if (UpdateClass(con, false, pid, par, set))
                            ret = set ? par : right0;
                        break;
                    case 3:     //cc
                        if (UpdateClass(con, true, pid, par, set))
                            ret = set ? par : right0;
                        break;
                    case 30:     //ALL cc
                        if (UpdateClassAll(con, pid))
                            ret = "...";
                        break;
                    case 4:     //klas - wynagrodzenie
                        if (UpdateClass(con, true, pid, par, set))
                            ret = set ? right1 : right0;
                        break;
                    case 5:     // limity NN na cc
                        if (UpdateClassUpr(con, true, pid, par, set))
                            ret = set ? par : right0;
                        break;
                    case 6:     // badania wstępne
                        if (BadaniaWstChangeAllUpr(con, pid))
                            ret = "bind!";
                        break;
                    */ 
                }
            }
            dbDisconnect();
            return ret;
        }





    }
}
