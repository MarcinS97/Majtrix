using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntImportALARMUS : System.Web.UI.UserControl
    {
        public event EventHandler ImportFinished;
        public const string WY = " (WY)";
        public bool UpdateReaders = false;

        const string LogInfo = "Import danych RCP z pliku";

        const string extBAK = ".bak";

        const int idALARMUS1    = 1;
        const int idALARMUS2    = 2;
        const int idADB         = 3;    // AUTOID
        const int idAUTOID_SIE  = 4;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if SIEMENS         // zajmuje czas ...
                UpdateLastData();
                FileUpload1.Visible = false;
                lbInfo.Visible = false;
                lbInfoAUTOID.Visible = true;
                btImport.Visible = false;
#else
                FileUpload1.Visible = true;
                lbInfo.Visible = true;
                lbInfoAUTOID.Visible = false;
                btImport.Visible = true;
#endif
            }
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
#if SIEMENS
            //ImportData1(idALARMUS);     //SIEMENS
            ImportData1(idAUTOID_SIE);     //AUTOID
#endif
#if IQOR
            ImportData1(idADB);     //ADB
#endif
        }

        //-------------------------------
        private static string GetErrorMsg(int err)
        {
            switch (err)
            {
                case 0:
                    return "Import zakończony.";
                case -1:
                    return "Niepoprawna struktura pliku.";
                case -2:
                    return "Błąd podczas otwierania pliku.";
                /*case -3:
                    Tools.ShowMessage("Błąd podczas zapisu do bazy.");
                    break;*/
                default:
                    return String.Format("Wystąpił błąd podczas importu danych. Kod: {0}.", err);
            }
        }

        private void UpdateLastData()
        {
            //lbLastData.Text = db.getScalar("select ISNULL(convert(varchar, max(Czas), 120), 'brak danych') from RCP");    // 3 sek Jabil
            lbLastData.Text = db.getScalar(@"select ISNULL(convert(varchar, max(D.Czas), 120), 'brak danych') from (select top 10000 Czas from RCP order by ECUniqueId desc) D");   // przyspieszyło znacznie ..., załozenie upraszczające 10000 powinno zawierać ostatnie, nawet jakbym ładował z ręki w odwrotnej kolejności
            lbLastDataAUTOID.Text = lbLastData.Text;  // póki co ...
        }
        //--------------------------------------------------------
        protected void ImportData1(int typ)  // 1
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(@"uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUpload1.SaveAs(savePath);
                int err = ImportData(typ, savePath, out UpdateReaders);
                string msg = GetErrorMsg(err);
                if (err == 0) UpdateLastData();
                Tools.ShowMessage(msg);
                if (ImportFinished != null)
                    ImportFinished(this, EventArgs.Empty);
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }

        public static void ImportData2()
        {
            string fileNameALARMUS = Tools.GetStr(ConfigurationSettings.AppSettings["RCPImportFile"]);
            if (!String.IsNullOrEmpty(fileNameALARMUS) && File.Exists(fileNameALARMUS))
            {
                string path = Path.GetDirectoryName(fileNameALARMUS);
                string ext = Path.GetExtension(fileNameALARMUS);
                string name = Path.GetFileNameWithoutExtension(fileNameALARMUS) + DateTime.Now.ToString("_yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                string fileName = Path.Combine(path, name + ext);
                try
                {
                    System.IO.File.Move(fileNameALARMUS, fileName);
                    bool skip;
                    //----------------------
                    int err = ImportData(2, fileName, out skip);
                    //----------------------
                    if (err == 0)
                    {
                        string fileNameBAK = Path.Combine(path, name + extBAK);
                        System.IO.File.Move(fileName, fileNameBAK);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(Log.t2APP_IMPORTRCP, LogInfo + (char)13 + fileNameALARMUS + (char)13 + fileName, ex.Message);
                }
            }
        }

        //--------------------------------------------------------
        private static string getValue(string line, ref int from, int count, int space)
        {
            string ret = Tools.Substring(line, from, count);
            from += count + space;
            return ret;
        }

        private static int ImportFromFile1(SqlConnection con, string fileName)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            string line;
            int err = -2;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));

                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    err = -1;
                    int cnt = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        if      (cnt == 0 && !line.ToLower().StartsWith("wydruk zdarzeń obiektu: audiosat")) break;
                        else if (cnt == 5 && !line.StartsWith("--------------------")) break;
                        else if (cnt >= 6) 
                        {
                            if (line.StartsWith("- KONIEC WYDRUKU -"))
                            {
                                err = 0;
                                break;
                            }
                            else
                            {
/*
Wydruk zdarzeń obiektu: audiosat
Zdarzenia wybrane: kontrola dostępu, w okresie: 23-09-2013 ÷ 01-10-2013; 
Data wydruku: 2013-10-02 09:29:41

          1         2         3         4         5         6         7         8         9        10        11        12              
0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
 Nr     Data    Godz.   Zdarzenie                                      Szczegóły                                     
------------------------------------------------------------------------------------------------------------------------
                      12345678901234567890123456789012345678901234567890
9999 23.09.2013  7:50 - Dostęp użytkownika                             K:czytnik piwnica    U:Jurek Krzysztof         
10000 23.09.2013  7:50 - Dostęp użytkownika                             K:czytnik hangar     U:MagdaMermela 
  15 16.10.2013 15:07 - Wyjście użytkownika                            K:czytnik piwnica    U:JĘDRZEJEWSKI            
*/
                                int len = line.Length;
                                if (len < 20) break;
                                int lplen = 4;
                                for (int i = lplen; i < 10; i++)
                                    if (line.Substring(i, 1) != " ")
                                        lplen++;
                                    else
                                        break;
                                int idx = 0;
                                string lp = getValue(line, ref idx, lplen, 1).Trim();  // 01234567890123456

                                string czas = getValue(line, ref idx, 16, 1).Trim();   // 01.01.2013 01:00
                                if (czas.Length == 0)
                                {
                                    if (Tools.Substring(line, idx, 100).Trim() != "POCZĄTEK PAMIĘCI ZDARZEŃ")
                                        break;
                                }
                                else
                                {
                                    string ev = getValue(line, ref idx, 48, 1).Trim();
                                    bool we = ev == "- Dostęp użytkownika";
                                    bool wy = ev == "- Wyjście użytkownika";

                                    if (we || wy)
                                    {
                                        if (czas.Length == 15) czas = "0" + czas;
                                        czas = czas.Substring(6, 4) + czas.Substring(3, 2) + czas.Substring(0, 2) + czas.Substring(10, 6);
                                        string czytnik = getValue(line, ref idx, 20, 1).Trim() + (wy ? WY : null);
                                        string prac = getValue(line, ref idx, 30, 1).Trim();

                                        bool ok = Base.execSQL(con, Base.insertSql("tmpImportRCP", 0,
                                            "Czas,RcpId,Nazwa1,Nazwa2,Typ,RogerId,ECCode",
                                            Base.strParam(czas),
                                            Tools.StrToInt(lp),
                                            Base.strParam(czytnik),
                                            Base.strParam(prac),
                                            db.NULL,
                                            db.NULL,
                                            db.NULL));
                                        if (!ok)
                                        {
                                            err = -3;
                                            break;
                                        }
                                    }
                                    /*
                                    else
                                    {
                                        int x = 0;
                                    }
                                    */ 
                                }
                            }
                        }
                        cnt++;
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            return err;
        }

        //-------------------
        private static int ImportFromFile2(SqlConnection con, string fileName)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            const string evIN  = "- Dostęp użytkownika";
            const string evOUT = "- Wyjście użytkownika";
            const string evIN_en  = "- User access";
            const string evOUT_en = "- User exit";

            string line;
            int err = -2;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));

                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    err = 0;
                    int cnt = 0;
                    while ((line = file.ReadLine()) != null)
                    {
/*        
          1         2         3         4         5         6         7         8         9        10        11        12              
0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789 
                123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789 
                                                                                123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789 
                                                                                                     123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789_123456789 
27.11.2013 15:12 - User exit                                                    K:czytnik poz.0      U:Burdzy Marcin   
27.11.2013 15:17 - User access                                                  K:czytnik produkcj   U:Karolina Piotrow                                
 */
                        int idx = 0;
                        string czas = getValue(line, ref idx, 16, 0).Trim();    // ' 1.01.2013 01:00'
                        if (czas.Length == 15) czas = "0" + czas;               
                        string ev = getValue(line, ref idx, 64, 0).Trim();
                        bool we = false;
                        bool wy = false;
                        if (ev == evIN || ev == evIN_en) we = true;
                        else if (ev == evOUT || ev == evOUT_en) wy = true;

                        if (we || wy)
                        {
                            string czytnik = getValue(line, ref idx, 21, 0).Trim() + (wy ? WY : null);
                            string prac = getValue(line, ref idx, 30, 0).Trim();
                            czas = czas.Substring(6, 4) + czas.Substring(3, 2) + czas.Substring(0, 2) + czas.Substring(10, 6);

                            if (String.IsNullOrEmpty(czas.Trim()) || String.IsNullOrEmpty(czytnik) || String.IsNullOrEmpty(prac))
                            {
                                err = -1;
                                break;
                            }
                            
                            bool ok = Base.execSQL(con, Base.insertSql("tmpImportRCP", 0,
                                "Czas,RcpId,Nazwa1,Nazwa2,Typ,RogerId,ECCode",
                                Base.strParam(czas),
                                cnt,
                                Base.strParam(czytnik),
                                Base.strParam(prac),
                                db.NULL,
                                db.NULL,
                                db.NULL));
                            if (!ok)
                            {
                                err = -3;
                                break;
                            }
                        }
                        cnt++;
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            return err;
        }

        //--------------------------------------------------------------------------------------------------
        // ADB
        //-----
        private static int ImportFromFileADB(SqlConnection con, string fileName)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            string line;
            int err = -2;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));
                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    err = 0;
                    int cnt = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (!line.StartsWith("Date"))
                        {
                            //Date,      Time,    Terminal ID,User ID,Name,   Employee ID,Class,Mode,  Type,Result,Property,Pass Count
                            //2014-09-01,04:56:47,0038,       0459,   Solid03,soli03,     User, Access,Card,Success
                            string[] data = line.Split(',',';');
                            int len = data.Length;
                            if (len >= 10 && len <= 12)
                            {
                                string d = data[0].Trim();          // Date
                                string t = data[1].Trim();          // Time
                                string rid = data[2].Trim();        // Terminal ID
                                string uid = data[3].Trim();        // User ID <<< RcpId
                                string name = data[4].Trim();       // NAme
                                string pid = data[5].Trim();        // Employee ID <<< IdPracownika
                                string c = data[6].Trim();          // Class
                                string m = data[7].Trim();          // Mode <<<
                                string typ = data[8].Trim();        // Type    
                                string res = data[9].Trim();        // Result <<<
                                string prop = null;                 // Property
                                string passcnt = null;              // PassCount
                                if (len == 11)
                                    prop = data[10].Trim();
                                if (len == 12)
                                    passcnt = data[11].Trim();

                                if (uid == "158")
                                {
                                    int x = 0;
                                }



                                const string moLeave = "Leave";
                                const string moAttend = "Attend";
                                const string moAccess = "Access";
                                const string moOut = "Out";
                                const string moF1 = "F1";
                                const string moF2 = "F2";
                                const string moF3 = "F3";

                                const string resNotMatched = "Not Matched";
                                const string resSuccess = "Success";
                                const string resNoPermission = "No Permission";

                                const string wyPryw = "Pryw.";
                                const string wySluzb = "Służb.";

                                if (res == resSuccess)          // nie zapisuje odczytów failed <<< ec ???
                                {
                                    string duty = wyPryw;
                                    int ec = 0;

                                    switch (m)
                                    {
                                        case moLeave:
                                        case moOut:
                                            ec = 0;
                                            break;
                                        case moAccess:
                                        case moAttend:
                                            ec = 1;
                                            break;
                                        case moF1:
                                            ec = 0;
                                            duty = wySluzb;
                                            break;

                                    }

                                    bool ok = Base.execSQL(con, Base.insertSql("tmpImportRCP", 0,
                                        "Czas,RcpId,Nazwa1,Nazwa2,Typ,RogerId,ECCode",
                                        Base.strParam(d + " " + t),     // Czas
                                        Tools.StrToInt(uid),            // RcpId
                                        Base.strParam(name),            // Nazwa1
                                        Base.strParam(pid),             // Nazwa2
                                        Base.strParam(duty),            // Typ
                                        Tools.StrToInt(rid),            // RogerId
                                        ec));                           // ECCode


                                    if (!ok)
                                    {
                                        err = -3;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                err = -1;
                                break;
                            }
                        }
                        cnt++;
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            return err;
        }
        
        //--------------------------------------------------------------------------------------------------
        public static void DeleteTmpTable(SqlConnection con)
        {
            Base.execSQL(con, "delete from tmpImportRCP");
        }

        public static int ImportData(int typ, string fileName, out bool updateReaders)
        {
            updateReaders = false;
            int lid = Log.Info(Log.t2APP_IMPORTRCP, LogInfo, String.Format("START, typ: {0}, plik: {1}", typ, fileName), Log.PENDING);
            SqlConnection con = Base.Connect();
            DeleteTmpTable(con);
            int err;
            switch (typ)
            {
                case idALARMUS1:     //SIEMENS
                    err = ImportFromFile1(con, fileName);
                    break;
                case idALARMUS2:     //SIEMENS
                    err = ImportFromFile2(con, fileName);
                    break;
                case idADB:     //ADB
                    err = ImportFromFileADB(con, fileName);
                    break;
                case idAUTOID_SIE:
                    err = ImportFromFileADB(con, fileName);
                    //err = ImportFromFileAUTOID_SIE(con, fileName);
                    break;
                default:
                    err = -999;
                    break;
            }
            if (err == 0)
            {
                try
                {
                    bool ok;
                    updateReaders = false;
                    switch (typ)
                    {
                        case idALARMUS1:
                        case idALARMUS2:
                            //----- nowe rejestratory -----
                            ok = db.execSQL(con, String.Format(@"                    
insert into Readers
select distinct I.Nazwa1, I.Nazwa1, null, 32, 0, 32, 
case when RIGHT(I.Nazwa1, {0}) = '{1}' then 1 else 0 end, 0, 1
from tmpImportRCP I
left outer join Readers R on R.Name = I.Nazwa1
where R.Id is null
update Readers set ZoneId = Id where ZoneId is null
                            ", WY.Length, WY));
                            updateReaders = ok;
                            //----- dane RCP -----
                            ok = db.execSQL(con, @"                    
insert into RCP 
select distinct 
I.Czas, 1, PK.IdPracownika, R.Id, R.Type, R.InOut, 0
from tmpImportRCP I
left outer join Readers R on R.Name = I.Nazwa1
left outer join PracownicyKarty PK on PK.NrKarty = I.Nazwa2 and I.Czas between PK.Od and ISNULL(PK.Do, '20990909')
left outer join RCP C on C.ECUserId = PK.IdPracownika and C.Czas = I.Czas 
                    and C.ECReaderId = R.Id
where C.ECUniqueId is null and PK.Id is not null and R.Id is not null
                            ");
                            break;
                        case idADB:   //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ADB tymczas !!!!!!!!!!!!!!!!!!!!!!!!!!
                            //----- dane RCP -----
                            ok = db.execSQL(con, @"                    
declare @start int
select @start = ISNULL(MAX(ECUserId),0) from RCP

--select @start	--536870927

insert into RCP 
SELECT ROW_NUMBER() OVER (ORDER BY D.Czas) AS RowNum, *  
from
(
select distinct 
I.Czas, 1 as ECCode, 
--PK.IdPracownika as ECUserId, 
PK.RcpId + 20000 as ECUserId, 
I.RogerId + 20000 as ECReaderId, 32 as ECDoorType, 0 as InOut, 0 as Duty
from tmpImportRCP I
left outer join PracownicyKarty PK on PK.RcpId = I.RcpId and I.Czas between PK.Od and ISNULL(PK.Do, '20990909')
left outer join RCP C on C.ECUserId = PK.IdPracownika and C.Czas = I.Czas 
                    and C.ECReaderId = I.RogerId + 20000
where C.ECUniqueId is null 
and PK.RcpId is not null
) D
                            ");
                             break;
                        case idAUTOID_SIE: 
                             //----- dane RCP -----
                             /*
                             ok = db.execSQL(con, @"                    
declare @start int
select @start = ISNULL(MAX(ECUserId),0) from RCP

--select @start	--536870927

insert into RCP 
SELECT ROW_NUMBER() OVER (ORDER BY D.Czas) AS RowNum, *  
from
(
select distinct 
I.Czas, 1 as ECCode, 
--PK.IdPracownika as ECUserId, 
PK.RcpId + 20000 as ECUserId, 
I.RogerId + 20000 as ECReaderId, 32 as ECDoorType, 0 as InOut, 0 as Duty
from tmpImportRCP I
left outer join PracownicyKarty PK on PK.RcpId = I.RcpId and I.Czas between PK.Od and ISNULL(PK.Do, '20990909')
left outer join RCP C on C.ECUserId = PK.IdPracownika and C.Czas = I.Czas 
                    and C.ECReaderId = I.RogerId + 20000
where C.ECUniqueId is null 
and PK.RcpId is not null
) D
                            ");
                            */
                             break;
                    }
                    Log.Update(lid, Log.OK);
                    Log.Info(Log.t2APP_IMPORTRCP, LogInfo, "END", Log.OK);
                }
                catch (Exception ex)
                {
                    Log.Update(lid, Log.ERROR);
                    Log.Error(Log.t2APP_IMPORTRCP, lid, LogInfo, ex.Message);
                    if (con != null)
                        Base.Disconnect(con);
                    throw;
                }
            }
            else
            {
                string msg = GetErrorMsg(err);
                Log.Error(Log.t2APP_IMPORTRCP, lid, LogInfo, msg);
            }
            if (con != null)
                Base.Disconnect(con);
            return err;
        }



























        /*
                public int ImportData(string fileName)
                {
                    const string info = "Import danych RCP z pliku";
                    int lid = Log.Info(Log.t2APP_IMPORTRCP, info, "START, plik: " + fileName, Log.PENDING);
                    SqlConnection con = Base.Connect();
                    DeleteTmpTable(con);
                    int err = ImportFromFile(con, fileName);
                    if (err == 0)
                    {
                        try
                        {
                            //----- nowe rejestratory -----
                            bool ok = db.execSQL(con, String.Format(@"                    
        insert into Readers
        select distinct I.Nazwa1, I.Nazwa1, null, 32, 0, 32, 
        case when RIGHT(I.Nazwa1, {0}) = '{1}' then 1 else 0 end, 0, 1
        from tmpImportRCP I
        left outer join Readers R on R.Name = I.Nazwa1
        where R.Id is null
        update Readers set ZoneId = Id where ZoneId is null
                            ", WY.Length, WY));
                            UpdateReaders = ok;
                            //----- dane RCP -----
                            ok = db.execSQL(con, @"                    
        insert into RCP 
        select distinct 
        I.Czas, 1, PK.IdPracownika, R.Id, R.Type, R.InOut, 0
        from tmpImportRCP I
        left outer join Readers R on R.Name = I.Nazwa1
        left outer join PracownicyKarty PK on PK.NrKarty = I.Nazwa2 and I.Czas between PK.Od and ISNULL(PK.Do, '20990909')
        left outer join RCP C on C.ECUserId = PK.IdPracownika and C.Czas = I.Czas
        where C.ECUniqueId is null and PK.Id is not null and R.Id is not null
                            ");
                            Log.Update(lid, Log.OK);
                            Log.Info(Log.t2APP_IMPORTRCP, info, "END", Log.OK);
                        }
                        catch (Exception ex)
                        {
                            Log.Update(lid, Log.ERROR);
                            Log.Error(Log.t2APP_IMPORTSTRUCT, lid, info, ex.Message);
                            if (con != null)
                                Base.Disconnect(con);
                            throw;
                        }
                    }
                    if (con != null)
                        Base.Disconnect(con);
                    if (ImportFinished != null)
                        ImportFinished(this, EventArgs.Empty);
                    return err;
                }
         */
        //------------------------------------------------------









        /*
        insert into RCP 
        select distinct 
        I.Czas, 1, P.Id, R.Id, R.Type, R.InOut, 0
        from tmpImportRCP I
        left outer join Readers R on R.Name = I.Nazwa1
        left outer join Pracownicy P on P.NrKarty2 = I.Nazwa2
        left outer join RCP C on C.Czas = I.Czas
        where C.ECUniqueId is null 
        and P.Id is not null and R.Id is not null
         */




        /*
                public int ImportData(string fileName)
                {
                    const string info = "Import danych RCP z pliku";
                    int lid = Log.Info(Log.t2APP_IMPORTRCP, info, "START, plik: " + fileName, Log.PENDING);
                    SqlConnection con = Base.Connect();
                    DeleteTmpTable(con);
                    int err = ImportFromFile(con, fileName);
                    if (err == 0)
                    {
                        try
                        {
                            string linfo;
                            if (!CheckLinkedOk(con, out linfo))
                            {
                                //Log.Update(lid, Log.ERROR);
                                //Log.Error(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV v.2", "Istnieją odczyty, którym nie można przypisać rejestratorów po RogerId", Log.OK);
                                //Tools.ShowMessage("Istnieją odczyty, którym nie można przypisać rejestratorów po RogerId - sprawdź w 'importRogerCSV.sql'. Import przerwany.");
                                Log.Error(Log.t2APP_IMPORTRCP, info, "Istnieją odczyty, którym nie można przypisać rejestratorów po RogerId, " + linfo, Log.OK);
                            }

                            DataSet ds = Base.getDataSet(con,
                                "select C.Czas, C.RcpId, C.RogerId, C.ECCode, " +
                                "case when C.Typ = 'Służb.' then 1 when C.Typ = 'Pryw.' then 0 else -1 end as Duty " +
                                "from tmpImportRCP C " +
                                "where C.RcpId < 100000000");

                            int ecuid = GetFirstECUniqueId(con, Base.getRows(ds).Count);
                            if (ecuid != -1)
                            {
                                int startid = ecuid;
                                string door = "-1";
                                string inout = Base.NULL;
                                int cnt = 0;
                                foreach (DataRow dr in Base.getRows(ds))
                                {
                                    string czas = Base.getValue(dr, 0);
                                    string rcpId = Base.getValue(dr, 1);
                                    string rogerId = Base.getValue(dr, 2);
                                    string uid = Base.getScalar(con, String.Format(
                                        "select ECUniqueId from RCP where Czas='{0}' and ECUserId={1} and ECReaderId={2}",
                                        czas, rcpId, rogerId));
                                    if (String.IsNullOrEmpty(uid))  //dodaję bo nie ma
                                    {
                                        string code = Base.getValue(dr, 3);
                                        string duty = Base.getValue(dr, 4);
                                        Base.execSQL(con, Base.insertSql("RCP", 0,
                                            "ECUniqueId,Czas,ECCode,ECUserId,ECReaderId,ECDoorType,InOut,Duty",
                                            ecuid, Base.strParam(czas), code, rcpId, rogerId, door, inout, duty
                                            ));
                                        ecuid++;
                                        cnt++;
                                    }
                                }
                                Log.Update(lid, Log.OK);
                                Log.Info(Log.t2APP_IMPORTRCP, info, String.Format("END zakres nowych ECUniqueId: {0} - {1}, dodano odczytów: {2}", startid, cnt == 0 ? 0 : ecuid - 1, cnt), Log.OK);
                            }
                            else
                            {
                                Log.Update(lid, Log.ERROR);
                                Log.Error(Log.t2APP_IMPORTRCP, info, "Brak miejsca w zakresie numeracji odczytów RCP", Log.OK);
                                Tools.ShowMessage("Brak miejsca w zakresie numeracji odczytów RCP. Import przerwany.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Update(lid, Log.ERROR);
                            Log.Error(Log.t2APP_IMPORTSTRUCT, lid, info, ex.Message);
                            if (con != null)
                                Base.Disconnect(con);
                            throw;
                        }
                    }
                    if (con != null)
                        Base.Disconnect(con);
                    if (ImportFinished != null)
                        ImportFinished(this, EventArgs.Empty);
                    return err;
                }
         */






    }
}