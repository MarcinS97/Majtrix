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
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class ImportRogerCSV2 : System.Web.UI.UserControl
    {
        public event EventHandler ImportFinished;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(@"uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUpload1.SaveAs(savePath);
                int err = ImportData(savePath);
                switch (err)
                {
                    case 0:
                        Tools.ShowMessage("Import zakończony.");
                        break;
                    case -1:
                        Tools.ShowMessage("Niepoprawna struktura pliku.");
                        break;
                    default:
                        Tools.ShowMessage("Wystąpił błąd podczas importu struktury.");
                        break;
                }
                if (ImportFinished != null)
                    ImportFinished(this, EventArgs.Empty);
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }
        //--------------------------------------------------------

        private string PrepareId(string id)
        {
            if (id.ToUpper().StartsWith("PL04"))
                id = id.Substring(4);   // odcinam PL04
            while (id.Length < 5)       // dopełniam 0 z przodu do 5 znaków tak jak to jest w kadrach
                id = "0" + id;
            return id;
        }

        private bool CheckFormat(string line, out string[] sep, out bool quota)
        {   //separatory: , ; tab "," ";" "tab"
            string[] separatory = new string[] { "\";\"", "\",\"", "\"\t\"", ";", ",", "\t" };
            bool[] isQuota = new bool[] { true, true, true, false, false, false };
            for (int i = 0; i < separatory.Length; i++)
            {
                sep = new string[1];
                sep[0] = separatory[i];
                string[] values = line.Split(sep, StringSplitOptions.None);
                if (values.Length >= 6)
                {
                    quota = isQuota[i];
                    return true;
                }
            }
            sep = null;
            quota = false;
            return false;
        }

        // -1 niepoprawny format
        private int ImportFromFile(SqlConnection con, string fileName)
        {
            string line;
            int err = 0;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));

                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    int cnt = 0;
                    //separatory: , ; tab "," ";" "tab"
                    //string[] sep = new string[] { "\",\"" };
                    string[] sep = new string[] { ";" };
                    //string[] sep;
                    bool quota = false;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (cnt == 0) 
                        {
                            if (!CheckFormat(line, out sep, out quota))  // pierwszą linia 
                            {
                                err = -1;
                                break;
                            }
                        }
                        //----- bez nagłówka -----
                        if (/*quota && */line.StartsWith("\"") && line.EndsWith("\""))   // 20130203 zawsze obcinam bo jak cała linia ograniczona to nie robi ...
                            line = line.Substring(1, line.Length - 2);  // odcinam " z początku i z końca
                        string[] values = line.Split(sep, StringSplitOptions.None);
                        //----- pierwsza obowiązująca wersja ok. 06.2012
                        if (values.Length == 13)                        // poprawny plik, na razie tylko tak sprawdzam, csv konczy sie ; wiec +1
                        {
                            string data = values[0].Trim();                     // A
                            string czas = values[1].Trim();                     // B
                            string code = values[2].Trim();                     // C
                            int c = Base.StrToIntDef(code.Substring(1, 3), -1); 
                            //string nazwisko = values[3].Trim();               // D --
                            string nazwa1 = values[4].Trim();                   // E nazwa
                            string rcpId = values[5].Trim();                    // F pracownika
                            // 6                                                // G --
                            string rogerId = values[7].Trim();                  // H rejestratora
                            // 8                                                // I -- pusto
                            // 9                                                // J -- kod
                            string nazwa2 = values[10].Trim();                  // K nazwa rej
                            string typ = values[11].Trim();                     // L Służb. Pryw.

                            Base.execSQL(con, Base.insertSql("tmpImportRCP", 0, 
                                "Czas,RcpId,Nazwa1,Nazwa2,Typ,RogerId,ECCode",
                                Base.strParam(data + ' ' + czas),
                                rcpId, 
                                Base.strParam(nazwa1), 
                                Base.strParam(nazwa2), 
                                Base.strParam(typ), 
                                rogerId,
                                c));
                        }
                        //----- 20121211 nowa wersja importu uwzględniająca nr ewidencyjny pracownika
                        /*  zaremowane  
                        else if (values.Length == 14)                        // poprawny plik, na razie tylko tak sprawdzam, csv konczy sie ; wiec +1
                        {
                            string data = values[0].Trim();                     // A
                            string czas = values[1].Trim();                     // B
                            string code = values[2].Trim();                     // C
                            int c = Base.StrToIntDef(code.Substring(1, 3), -1);
                            //string nazwisko = values[3].Trim();               // D -- nazwisko
                            string nazwa1 = values[4].Trim();                   // E nazwa
                            string rcpId = values[5].Trim();                    // F RcpId pracownika
                            // 6                                                // G -- Komentarz "stanowisko"
                            string rogerId = values[7].Trim();                  // H RogerId rejestratora
                            // 8                                                // I -- komentarz 3
                            string nazwa2 = values[9].Trim();                   // J nazwa rej
                            // 10                                               // K -- kod
                            // 11                                               // L nr_ew
                            string typ = values[12].Trim();                     // M Służb. Pryw.

                            Base.execSQL(con, Base.insertSql("tmpImportRCP", 0,
                                "Czas,RcpId,Nazwa1,Nazwa2,Typ,RogerId,ECCode",
                                Base.strParam(data + ' ' + czas),
                                rcpId,
                                Base.strParam(nazwa1),
                                Base.strParam(nazwa2),
                                Base.strParam(typ),
                                rogerId,
                                c));
                        }
                        /**/
                        //----- 20130120 II nowa wersja importu po crashu, uwzględniająca nr ewidencyjny pracownika
                        /**/
                        
                        
                        else if (values.Length == 14)    // poprawny plik, na razie tylko tak sprawdzam, csv konczy sie ; wiec +1
                        //else if (true)


                        {
                            string data = Tools.PrepareDate(values[0].Trim());  // A
                            string czas = values[1].Trim();                     // B
                            string code = values[2].Trim();                     // C
                            int c = Base.StrToIntDef(code.Substring(1, 3), -1);
                            //string nazwisko = values[3].Trim();               // D -- nazwisko
                            string nazwa1 = values[4].Trim();                   // E nazwa
                            string rcpId = values[5].Trim();                    // F RcpId pracownika
                            // 6                                                // G -- Komentarz "stanowisko"
                            // 7                                                // H -- komentarz #2 (komentarz 3)
                            // 8                                                // I -- kod karty (DEC)
                            string typ = values[9].Trim();                      // J Służb. Pryw.                            
                            string rogerId = values[10].Trim();                 // K RogerId rejestratora
                            string nazwa2 = values[11].Trim();                  // L nazwa rej
                            // 12                                               // M nr_ew
                            
                            Base.execSQL(con, Base.insertSql("tmpImportRCP", 0,
                                "Czas,RcpId,Nazwa1,Nazwa2,Typ,RogerId,ECCode",
                                Base.strParam(data + ' ' + czas),
                                rcpId,
                                Base.strParam(nazwa1),
                                Base.strParam(nazwa2),
                                Base.strParam(typ),
                                rogerId,
                                c));
                        }
                        /**/
                        else
                        {
                            err = -1;
                            break;
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

        //spr czy są nierozpoznane
        public bool CheckLinkedOk(SqlConnection con)  
        {
            string cnt = Base.getScalar(con,
                "select count(*) from tmpImportRCP C " +
                //"C.Czas, C.RcpId, C.Nazwa1, C.Nazwa2, R.Name, R.Id from tmpImportRCP C
                "left outer join Readers R on R.Id = C.RogerId " +
                "where R.Id is null and RcpId < 100000000");
            return cnt == "0";
        }

        public bool CheckLinkedOk(SqlConnection con, out string info)
        {
            string cnt = Base.getScalar(con, @"
select count(*) from tmpImportRCP C 
left outer join Readers R on R.Id = C.RogerId 
where R.Id is null and RcpId < 100000000");
            if (cnt == "0") info = null;
            else
            {
                DataSet ds = Base.getDataSet(con, @"
select distinct C.RogerId from tmpImportRCP C
left outer join Readers R on R.Id = C.RogerId 
where R.Id is null and RcpId < 100000000
            ");
                info = String.Format("Ilość: {0} RogerId: {1}", cnt, db.Join(ds, 0, ", "));
                return false;
            }
            return true;
        }

        public int GetFirstECUniqueId(SqlConnection con, int count)
        {
            const int min = 1000000;
            const int max = 18000000;
            int id = min;
            string sid = Base.getScalar(con, String.Format(
                "select max(ECUniqueId) + 1 from RCP where ECUniqueId between {0} and {1}", min, max));
            if (!String.IsNullOrEmpty(sid))
                id = Base.StrToIntDef(sid, -1);
            if (id == -1 || max - id < count)
                return -1;   // nie ma miejsca
            else
                return id;
        }

        public void DeleteTmpTable(SqlConnection con)
        {
            Base.execSQL(con, "delete from tmpImportRCP");
        }

        public int ImportData(string fileName)
        {
            const string info = "Import danych RCP z plików CSV v.3";
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
                                //Base.execSQL(con, Base.insertSql("RCP", 0,
                                //    "ECUniqueId,Czas,ECCode,ECUserId,ECReaderId,ECDoorType,InOut,Duty",
                                //    ecuid, Base.strParam(czas), code, rcpId, rogerId, door, inout, duty
                                //    ));
                                Base.execSQL(con, Base.insertSql("RCP", 0,
                                    "Czas,ECCode,ECUserId,ECReaderId,ECDoorType,InOut,Duty",
                                    Base.strParam(czas), code, rcpId, rogerId, door, inout, duty
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

    }
}