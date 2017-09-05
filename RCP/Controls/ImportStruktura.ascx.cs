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
    public partial class ImportStruktura : System.Web.UI.UserControl
    {
        public event EventHandler ImportFinished;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.MakeConfirmButton(btBackup, "Potwierdź wykonanie kopii bezpieczeństwa obowiązującej struktury.");
                Tools.MakeConfirmButton(btRestore, "Potwierdź operację przywrócenia danych z kopii i nadpisania obowiązującej struktury.");
            }
        }

        protected void btImportStruktura_Click(object sender, EventArgs e)
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
        private bool backupTable(SqlConnection con, string tbName, string bckName)
        {
            if (String.IsNullOrEmpty(bckName)) bckName = "copy" + tbName;
            return Base.execSQL(String.Format(
                "IF EXISTS(SELECT name FROM sysobjects WHERE name = '{1}' AND xtype='U') DROP TABLE {1};" +
                "select * into {1} from {0}",
                tbName, bckName));
        }

        private bool restoreTable(SqlTransaction tr, string bckName, string tbName)
        {
            if (String.IsNullOrEmpty(bckName)) bckName = "copy" + tbName;
            string fields = Base.GetColumns(tr, tbName);
            return Base.execSQL(String.Format(
                "IF EXISTS(SELECT name FROM sysobjects WHERE name = '{0}' AND xtype='U') begin " +
                    "delete from {1};" +
                    "set identity_insert {1} ON;" +
                    "insert into {1} ({2}) select {2} from {0};" +
                    "set identity_insert {1} OFF;" +
                "end",
                bckName, tbName, fields));
        }

        private string dropCopyOfTable(string tbName)
        {
            return String.Format(
                "IF EXISTS(SELECT name FROM sysobjects WHERE name = 'copy{0}' AND xtype='U') DROP TABLE copy{0}",
                tbName);
        }

        private void MakeBackup(SqlConnection con)
        {
            backupTable(con, "Pracownicy", null);
            backupTable(con, "Dzialy", null);
            backupTable(con, "Stanowiska", null);
        }

        private void RestoreBackup(SqlTransaction tr)
        {
            restoreTable(tr, null, "Pracownicy");
            restoreTable(tr, null, "Dzialy");
            restoreTable(tr, null, "Stanowiska");
        }
        //--------------------------------------------------------
        private void CreateTempTable(SqlConnection con)
        {
            Base.execSQL(con,
                "CREATE TABLE #tmpImport (" +
                    "[PracId] [varchar](10) NOT NULL," +
                    "[PracNI] [nvarchar](100) NOT NULL," +
                    "[KierId] [varchar](10) NULL," +
                    "[KierNI] [nvarchar](100) NULL," +
                    "[Dzial] [nvarchar](100) NULL," +
                    "[Stanowisko] [nvarchar](100) NULL," +
                    "[DzialId] [int] NULL," +
                    "[StanowiskoId] [int] NULL," +
                    "[IdKierownika] [int] NULL)");
        }

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
                    //string[] sep = new string[] { ";" };
                    string[] sep = null;
                    bool quota = false;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (cnt == 0) 
                        {
                            if (!CheckFormat(line, out sep, out quota))  // pierwszą linia nagłówka
                            {
                                err = -1;
                                break;
                            }
                        }
                        else
                        {
                            if (quota && line.StartsWith("\"") && line.EndsWith("\""))
                                line = line.Substring(1, line.Length - 2);  // odcinam " z początku i z końca
                            string[] values = line.Split(sep, StringSplitOptions.None);
                            if (values.Length >= 6)    // pracId - nazwisko_imie - stanowisko - kier_id - kier_nazwisko - dzial
                            {
                                string pracId = PrepareId(values[0]).Trim();
                                string pracNI = Tools.TextToCtrl(values[1]).Trim();
                                string stanowisko = Tools.TextToCtrl(values[2]).Trim();
                                //string obszar = Tools.TextToCtrl(values[3]).Trim();  // skip nie wciągam jeszcze nigdzie
                                string kierId = PrepareId(values[4]).Trim();
                                string kierNI = Tools.TextToCtrl(values[5]).Trim();
                                string dzial = Tools.TextToCtrl(values[6]).Trim();
                                /*
                                Base.execSQL(tr, "insert into #prac values (" +
                                        Base.insertStrParam(pracId) +
                                        Base.insertStrParam(pracNI) +
                                        Base.insertStrParam(kierId) +
                                        Base.insertStrParam(kierNI) +
                                        Base.insertStrParam(dzial) +
                                        Base.insertStrParamLast(stanowisko) +
                                    ")");
                                */

                                Base.execSQL(con, String.Format(
                                    "insert into #tmpImport values ('{0}','{1}','{2}','{3}','{4}','{5}',null,null,null)",
                                        pracId, pracNI, kierId, kierNI, dzial, stanowisko));

                                /*
                                bool b = Base.execSQL(
                                    "update Teksty set " +
                                    Base.updateStrParam("Opis", opis) +
                                    Base.updateStrParamLast("Tekst", tekst) +
                                    "where Typ = " + Base.strParam(typ));
                                if (!b)
                                    Base.execSQL(
                                        "insert into Teksty (Typ, Opis, Tekst) " +
                                        "values (" +
                                            Base.strParam(typ) + "," +
                                            Base.strParam(opis) + "," +
                                            Base.strParam(tekst) +
                                        ")");
                                 */
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

        //private void InsertDzialy(SqlTransaction tr)
        private void InsertDzialy(SqlConnection tr)
        {
            DataSet ds = Base.getDataSet(tr, "select distinct Dzial from #tmpImport");
            foreach (DataRow dr in Base.getRows(ds))
            {
                string nazwa = dr[0].ToString();
                string did = Base.getScalar(tr, "select Id from Dzialy where UPPER(Nazwa) = " + Base.strParam(nazwa.ToUpper()));
                if (String.IsNullOrEmpty(did))  // nowy wpis, Status = stNew
                {
                    int id = Base.insertSQL(tr,
                        "insert into Dzialy (Nazwa, Status) values (" + 
                            Base.insertStrParam(nazwa) +
                            Base.insertParamLast(App.stNew.ToString()) +
                        ")");
                    did = id.ToString();
                }
                else                            // istniejący, Status = stOk, nazwa jest aktualizowana jakby doszło do zmiany wielkości liter
                    Base.execSQL(tr, String.Format(
                        "update Dzialy set Status={0}, Nazwa={1} where Id={2}",
                        App.stCurrent.ToString(), Base.strParam(nazwa), did));

                Base.execSQL(tr, "update #tmpImport set DzialId = " + did + " where Dzial = " + Base.strParam(nazwa));  // aktualizuje id zeby juz bylo 
            }
        }

        //private void InsertStanowiska(SqlTransaction tr)
        private void InsertStanowiska(SqlConnection tr)
        {
            DataSet ds = Base.getDataSet(tr, "select distinct DzialId, Stanowisko from #tmpImport");
            foreach (DataRow dr in Base.getRows(ds))
            {
                string did = dr[0].ToString();
                string nazwa = dr[1].ToString();
                int id = Base.insertSQL(tr,
                    "insert into Stanowiska (IdDzialu, Nazwa) values (" +
                        Base.insertParam(did) +
                        Base.insertStrParamLast(nazwa) +
                    ")");
                Base.execSQL(tr, 
                    "update #tmpImport set StanowiskoId = " + id.ToString() + 
                    " where DzialId = " + did +
                    " and Stanowisko = " + Base.strParam(nazwa));  
            }
        }

        //private void InsertKierownicy(SqlTransaction tr)
        private void InsertKierownicy(SqlConnection tr)
        {
            DataSet ds = Base.getDataSet(tr, "select distinct KierId, KierNI from #tmpImport"); // różni kierkownicy
            foreach (DataRow dr in Base.getRows(ds))
            {
                string nazwisko, imie;
                string kierId = dr[0].ToString();   // z KP
                Tools.GetNazwiskoImie(dr[1].ToString(), out nazwisko, out imie);

                string kid = Base.getScalar(tr, "select Id from Pracownicy where KadryId = " + Base.strParam(kierId));  // spr czy juz nie ma ?
                if (String.IsNullOrEmpty(kid))  // nowy wpis, Status = stNew
                {
                    int id = Base.insertSQL(tr,
                        "insert into Pracownicy (Nazwisko, Imie, KadryId, Kierownik, Admin, Status, IdKierownika) values (" +
                            Base.insertStrParam(nazwisko) +
                            Base.insertStrParam(imie) +
                            Base.insertStrParam(kierId) +
                            Base.insertParam("1") +             // kierownik
                            Base.insertParam("0") +             // admin
                            Base.insertParam(App.stNew.ToString()) +
                            Base.insertParamLast("0") +         // kierownik - na poziom 0, jezeli bedzie pracownikiem to mu import prac. ustawi prawidłowego
                        ")");
                    kid = id.ToString();
                }
                else                            // istniejący, Status = ok, nazwa jest aktualizowana jakby doszło do zmiany wielkości liter i nic więcej
                    Base.execSQL(tr, 
                        "update Pracownicy set " +
                            Base.updateStrParam("Nazwisko", nazwisko) + 
                            Base.updateStrParam("Imie", imie) + 
                            Base.updateParamLast("Status", App.stCurrent.ToString()) + 
                        " where Id = " + kid);

                Base.execSQL(tr, "update #tmpImport set IdKierownika = " + kid + " where KierId = " + Base.strParam(kierId));  // aktualizuje id zeby juz bylo  
            }
        }

        //private void InsertPracownicy(SqlTransaction tr)
        private void InsertPracownicy(SqlConnection tr)
        {
            DataSet ds = Base.getDataSet(tr, "select PracId, PracNI, DzialId, StanowiskoId, IdKierownika from #tmpImport");
            foreach (DataRow dr in Base.getRows(ds))
            {
                string nazwisko, imie;
                string pracId = dr[0].ToString();
                Tools.GetNazwiskoImie(dr[1].ToString(), out nazwisko, out imie);
                string did = dr[2].ToString();
                string sid = dr[3].ToString();
                string kid = dr[4].ToString();

                string pid = Base.getScalar(tr, "select Id from Pracownicy where KadryId = " + Base.strParam(pracId));
                if (String.IsNullOrEmpty(pid))  // nowy wpis, Status = stNew
                {
                    int id = Base.insertSQL(tr,
                        "insert into Pracownicy (Nazwisko, Imie, KadryId, Kierownik, IdDzialu, IdStanowiska, IdKierownika, Admin, Status) values (" +
                            Base.insertStrParam(nazwisko) +
                            Base.insertStrParam(imie) +
                            Base.insertStrParam(pracId) +
                            Base.insertParam("0") +             // nie-kierownik, bo kierownicy juz dodani
                            Base.insertParam(did) +
                            Base.insertParam(sid) +
                            Base.insertParam(kid) +
                            Base.insertParam("0") +
                            Base.insertParamLast(App.stNew.ToString()) +
                        ")");
                    pid = id.ToString();
                }
                else                            // istniejący, Status = 1, nazwa jest aktualizowana jakby doszło do zmiany wielkości liter
                    Base.execSQL(tr,
                        "update Pracownicy set " +
                            Base.updateStrParam("Nazwisko", nazwisko) +
                            Base.updateStrParam("Imie", imie) +
                            Base.updateParam("IdDzialu", did) +
                            Base.updateParam("IdStanowiska", sid) +
                            Base.updateParam("IdKierownika", kid) +
                            Base.updateParamLast("Status", App.stCurrent.ToString()) +
                        " where Id = " + pid);

                //Base.execSQL(tr, "update Pracownicy set Kierownik=0 where Id not in (select distinct IdKierownika from Pracownicy where IdKierownika is not null)");
                //Base.execSQL(tr, "update Pracownicy set Kierownik=1 where Id in (select distinct IdKierownika from Pracownicy where IdKierownika is not null)");

                //----- aktualizacja z uwzględnieniem statusu - jak kier ma starych pracowników to nie jest kierownikiem 
                Base.execSQL(tr, "update Pracownicy set Kierownik=0 where Id not in (select distinct IdKierownika from Pracownicy where IdKierownika is not null and Status >= 0)");
                Base.execSQL(tr, "update Pracownicy set Kierownik=1 where Id in (select distinct IdKierownika from Pracownicy where IdKierownika is not null and Status >= 0)");
            }
        }

        public int ImportData(string fileName)
        {
            int lid = Log.Info(Log.t2APP_IMPORTSTRUCT, "Import struktury", "START", Log.PENDING);
            SqlConnection con = Base.Connect();
            CreateTempTable(con);
            int err = ImportFromFile(con, fileName);
            if (err == 0)
            {
                //SqlTransaction tr = con.BeginTransaction();
                SqlConnection tr = con;
                try
                {
                    Base.execSQL(tr, "update Dzialy set Status=" + App.stOld.ToString());       // do usunięcia 
                    Base.execSQL(tr, "delete from Stanowiska");                                 // usuwam bo to tylko informacja
                    //Base.execSQL(tr, "update Pracownicy set Status=" + App.stOld.ToString() + " where Status <> " + App.stPomin.ToString());   // do usunięcia 
                    Base.execSQL(tr, String.Format(
                        "update Pracownicy set Status={0} where Status not in ({1},{2})",
                            App.stOld, App.stPomin, App.stNew));
                    InsertDzialy(tr);
                    InsertStanowiska(tr);
                    InsertKierownicy(tr);
                    InsertPracownicy(tr);
                    /**/
                    //*---- debug -----
                    Base.DropIfExists(tr, "tmpPracImport");
                    Base.execSQL(tr, "select * into tmpPracImport from #tmpImport");
                    /**/
                    //tr.Commit();
                    Log.Update(lid, Log.OK);
                    Log.Info(Log.t2APP_IMPORTSTRUCT, "Import struktury", "END", Log.OK);
                }
                catch (Exception ex)
                {
                    Log.Update(lid, Log.ERROR);
                    Log.Error(Log.t2APP_IMPORTSTRUCT, lid, "Import struktury", ex.Message);
                    //tr.Rollback();
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

        protected void btBackup_Click(object sender, EventArgs e)
        {
            Log.Info(Log.t2APP, "Wykonanie kopii bezpieczeństwa struktury", null, Log.OK);
            SqlConnection con = Base.Connect();
            MakeBackup(con);
            Tools.ShowMessage("Kopia bezpieczeństwa została wykonana.");
            Base.Disconnect(con);
        }

        protected void btRestore_Click(object sender, EventArgs e)
        {
            Log.Info(Log.t2APP, "Przywacanie danych z kopii bezpieczeństwa struktury", null, Log.OK);
            SqlTransaction tr = Base.Connect().BeginTransaction();
            RestoreBackup(tr);
            Tools.ShowMessage("Dane zostały przywrócone z kopii bezpieczeństwa.");
            Base.Disconnect(tr);
            if (ImportFinished != null)
                ImportFinished(this, EventArgs.Empty);
        }
    }
}