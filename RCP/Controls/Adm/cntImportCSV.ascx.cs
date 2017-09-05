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
    public partial class cntImportCSV : System.Web.UI.UserControl
    {
        public event EventHandler ImportFinished;
        public const string WY = " (WY)";
        public bool UpdateReaders = false;

        const string LogInfo = "Import z pliku CSV";
        const string extBAK  = ".bak";

        const int tSIEAbsencjaOld   = 5;
        const int tSIEAbsencja      = 6;
        const int tSCLP             = 7; // import lista płac (Scorecards)
        const int tSCU              = 8; // import umowy

        const int tOKTPracownicy    = 9; // import pracowników keeeper
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btImport.OnClientClick = String.Format(btImport.OnClientClick, Typ);
                if (!String.IsNullOrEmpty(BtnCssClass)) btImport.CssClass = BtnCssClass;
                switch (Typ)
                {
                    case 0:
                        break;
#if SIEMENS
                    case tSIEAbsencjaOld:
                        break;
                    case tSIEAbsencja:
                        break;
                    case tSCLP:
                        PrepareSCLP();
                        break;
                    case tSCU:
                        PrepareSCU();
                        break;
#endif
                    case tOKTPracownicy:
                        OKT_ImportPracownicyPrepare();
                        break;
                }
            }
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            ImportData1(Typ);  
        }

        public int Typ
        {
            get;
            set;
        }

        public string BtnCssClass
        {
            get;
            set;
        }
        //-------------------------------
        private static string GetErrorMsg(int err, bool noP, bool noK, int lineno)
        {
            switch (err)
            {
                case 0:
                    string msgP = noP ? "\\n- nie znaleziono pracowników" : null;
                    string msgK = noK ? "\\n- nie znaleziono kodów absencji" : null;
                    if (noP || noK)
                        return "Import zakończony, ale nie wszystkie absencje zostały zapisane:" +
                            msgP +
                            msgK +
                            "\\nSzczegóły znajdują się w logu systemowym.";
                    else
                        return "Import zakończony poprawnie.";
                case -1:
                    return String.Format("Niepoprawna struktura pliku. Linia: {0}", lineno);
                case -2:
                    return "Błąd podczas otwierania pliku.";
                /*case -3:
                    Tools.ShowMessage("Błąd podczas zapisu do bazy.");
                    break;*/
                default:
                    return String.Format("Wystąpił błąd podczas importu danych. Kod: {0}.", err);
            }
        }

        //--------------------------------------------------------
        protected void ImportData1(int typ)  // 1
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(@"~\uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUpload1.SaveAs(savePath);
                string dod = null;
                string ddo = null;
                bool noP, noK;
                string sqlAfter = null;
                string headPattern = null;
                switch (Typ)
                {
                    case tSCLP:
                        dod = ddlMiesiac.SelectedValue;
                        ddo = null;
                        break;
                    case tOKTPracownicy:
                        dod = ddlMiesiac.SelectedValue;

                        if (String.IsNullOrEmpty(dod))
                        {
                            Tools.ShowError("Brak wybranego okresu rozliczeniowego!");
                            return;
                        }

                        ddo = null;
                        sqlAfter = dsOKTPracownicy.SelectCommand;
                        headPattern = dsOKTPracownicy.InsertCommand;
                        break;
                    default:
                        Tools.GetLineParams(ddlMiesiac.SelectedValue, out dod, out ddo);
                        break;
                }
                int lineno;
                int err = ImportData(typ, savePath, dod, ddo, out noP, out noK, out lineno, sqlAfter, headPattern);
                string msg = GetErrorMsg(err, noP, noK, lineno);
                Tools.ShowMessage(msg);
                if (ImportFinished != null)
                    ImportFinished(this, EventArgs.Empty);
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }


        /*
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
        */
        //--------------------------------------------------------
        private static string getValue(string line, ref int from, int count, int space)
        {
            string ret = Tools.Substring(line, from, count);
            from += count + space;
            return ret;
        }

        //--------------------------
        // SIVANTOS
        //--------------------------
        private static int ImportFromFileABS(SqlConnection con, string fileName, out int lineno)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            lineno = 0;
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
                    bool header = true;

                    while ((line = file.ReadLine()) != null)
                    {
                        lineno++;

                        string[] data = line.Split(';');
                        int len = data.Length;
                        if (len != 10)
                        {
                            err = -1;    // format
                            break;
                        }
                        else
                        {
                            string naz = data[0].Trim();   //Nazwisko	
                            string imie = data[1].Trim();   //Imię	
                            string nrS = data[2].Trim();   //nr personalny Siemens	
                            string nrR = data[3].Trim();   //nr personalny Randstad	
                            string dOd = data[4].Trim();   //od	
                            string dDo = data[5].Trim();   //do	
                            string kodAbs = data[6].Trim();   //kod absencji 	
                            string nazwaAbs = data[7].Trim();   //Rodzaj nieobecności	
                            string ilDni = data[8].Trim();   //ilość dni roboczych 	
                            string ilGodz = data[9].Trim();   //ilość godzin roboczych
                            if (header)
                            {
                                if (naz != "Nazwisko" ||
                                    imie != "Imię" ||
                                    nrS != "nr personalny Siemens" ||
                                    nrR != "nr personalny Randstad" ||
                                    dOd != "od" ||
                                    dDo != "do" ||
                                    kodAbs != "kod absencji" ||
                                    nazwaAbs != "Rodzaj nieobecności" ||
                                    ilDni != "ilość dni roboczych" ||
                                    ilGodz != "ilość godzin roboczych")
                                {
                                    err = -1;       // format
                                    break;
                                }
                                else
                                {
                                    header = false;
                                }
                            }
                            else
                            {
                                //----- kontrola -----    //20150113
                                if (!Tools.DateIsValid(dOd) || !Tools.DateIsValid(dDo) || String.IsNullOrEmpty(kodAbs))
                                {
                                    err = -1;    // format
                                    break;
                                }
                                //----- dane -----
                                if (!String.IsNullOrEmpty(nrS) || !String.IsNullOrEmpty(nrR))   // tylko jak jes numer !!!
                                {
                                    bool ok = Base.execSQL(con, Base.insertSql("bufAbsencja2", 0,
                                        "Nazwisko,Imie,NrEwSiemens,NrEwRandstad,DataOd,DataDo,KodAbs,NazwaAbs,IleDni,Godzin",
                                        db.strParam(naz),               //Nazwisko	
                                        db.strParam(imie),              //Imię	
                                        db.strParam(nrS),               //nr personalny Siemens	
                                        db.strParam(nrR),               //nr personalny Randstad	
                                        db.strParam(dOd),               //od	
                                        db.strParam(dDo),               //do	
                                        db.strParam(kodAbs),            //kod absencji 	
                                        db.strParam(nazwaAbs),          //Rodzaj nieobecności	
                                        db.strParam(ilDni.Replace(" ", "").Replace(',', '.')),        //ilość dni roboczych 	
                                        db.strParam(ilGodz.Replace(" ", "").Replace(',', '.'))));     //ilość godzin roboczych
                                    if (!ok)
                                    {
                                        err = -3;     // błądz zapisu
                                        break;
                                    }
                                    cnt++;
                                }
                            }
                        }
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

        private static int ImportListaPlac(SqlConnection con, string fileName, out int lineno, string date)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            //const string headPattern = "Numer osobowy;Nazwisko;Imię;Miejsce powst. kosztów; Brutto ; ZUS firma razem ; Koszt firma ";
            const string headPattern = "Numer osobowy;Nazwisko;Imię;Miejsce powst. kosztów;Brutto;ZUS firma razem;Koszt firma";   // 20160802

            const char SEP = ';';

            lineno = 0;
            string line;
            int err = -2;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));
                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    //err = 0;
                    int cnt = 0;
                    int colCount = 0;
                    bool header = true;
                    bool empty = false;    //puste linie na końcu

                    while ((line = file.ReadLine()) != null)
                    {
                        lineno++;
                        string[] data = line.Split(SEP);
                        if (header)
                        {
                            err = 0;
                            if (!CheckColumns(headPattern, data, out colCount))
                            {
                                err = -1;       // format
                                break;
                            }
                            else
                            {
                                header = false;
                            }
                        }
                        else
                        {
                            if (data.Length == 1)   // pusta linia - footer ?
                            {
                                if (!empty)
                                    empty = true;
                            }
                            else if (data.Length >= colCount)
                            {
                                int idx = 0;
                                string kadryId = data[idx++].Trim();       //KadryId
                                string naz = data[idx++].Trim();           //Nazwisko	
                                string imie = data[idx++].Trim();           //Imię	
                                string cc = data[idx++].Trim();
                                string brutto = ToNum(data[idx++].Trim());
                                string zus = ToNum(data[idx++].Trim());
                                string kosztFirma = ToNum(data[idx++].Trim());
                                string dataImport = date;

                                //string nrS        = data[2].Trim();               //nr personalny Siemens	
                                //string nrR = data[idx++].Trim();           //nr personalny Randstad	
                                //string _mpk = data[idx++].Trim();
                                //string _opis = data[idx++].Trim();
                                //string _jorg = data[idx++].Trim();
                                //string _jnazwa = data[idx++].Trim();
                                ////string kodAbs     = ToKod(data[idx++].Trim());    //kod absencji 	
                                //string kodAbs = data[idx++].Trim();           //kod absencji - zgodnie z mailem nie ustawiam
                                //string nazwaAbs = data[idx++].Trim();           //Rodzaj nieobecności	
                                //string dOd = ToDate(data[idx++].Trim());   //od	
                                //string dDo = ToDate(data[idx++].Trim());   //do	
                                //string _ilDniKal = ToNum(data[idx++].Trim());
                                //string ilDni = ToNum(data[idx++].Trim());    //ilość dni roboczych 	
                                //string ilGodz = ToNum(data[idx++].Trim());    //ilość godzin roboczych
                                //string _nrRef = data[idx++].Trim();           //Rodzaj nieobecności	                                
                                //----- kontrola -----    //20150113
                                if (/*!Tools.DateIsValid(dOd) || !Tools.DateIsValid(dDo) || String.IsNullOrEmpty(kodAbs) || String.IsNullOrEmpty(ilDni)*/ false)
                                {
                                    if (!IsEmpty(data))     // jednak coś jest, to error
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                    else                    // wszystko puste - footer ?
                                        if (!empty)
                                            empty = true;
                                }
                                else
                                {
                                    if (empty)              // poprzedni był pusty, a teraz są dane
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                }
                                //----- dane -----
                                //if (!String.IsNullOrEmpty(nrS) || !String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                //if (!String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                {
                                    bool ok = Base.execSQL(con, Base.insertSql("bufListaPlac", 0,
                                        "KadryId,Nazwisko,Imie,CC,Brutto,ZUS,KosztFirma,Data",
                                        db.strParam(kadryId),      //KadryId
                                        db.strParam(naz),          //Nazwisko	
                                        db.strParam(imie),         //Imię	i	
                                        db.strParam(cc),           //CC
                                        db.strParam(brutto),       //Brutto
                                        db.strParam(zus),          //ZUS
                                        db.strParam(kosztFirma),   //KosztFirma
                                        db.strParam(dataImport)    //Data
                                        ));
                                    if (!ok)
                                    {
                                        err = -3;     // błądz zapisu
                                        break;
                                    }
                                    cnt++;
                                }
                            }
                            else                // różna ilość kolumn
                            {
                                err = -1;       // format
                                break;
                            }
                        }
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

        private static int ImportUmowy(SqlConnection con, string fileName, out int lineno, string date)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            const string headPattern = "Nr ewidencyjny;Imię;Nazwisko;Data obowiązującej umowy od;Data obowiązującej umowy do";
            const char SEP = ';';

            lineno = 0;
            string line;
            int err = -2;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));
                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    //err = 0;
                    int cnt = 0;
                    int colCount = 0;
                    bool header = true;
                    bool empty = false;    //puste linie na końcu

                    while ((line = file.ReadLine()) != null)
                    {
                        lineno++;
                        string[] data = line.Split(SEP);
                        if (header)
                        {
                            err = 0;
                            if (!CheckColumns(headPattern, data, out colCount))
                            {
                                err = -1;       // format
                                break;
                            }
                            else
                            {
                                header = false;
                            }
                        }
                        else
                        {
                            if (data.Length == 1)   // pusta linia - footer ?
                            {
                                if (!empty)
                                    empty = true;
                            }
                            else if (data.Length >= colCount)
                            {
                                int idx = 0;
                                string kadryId = data[idx++].Trim();       //KadryId
                                string imie = data[idx++].Trim();           //Imię	
                                string naz = data[idx++].Trim();           //Nazwisko	
                                string dataOd = data[idx++].Trim();
                                string dataDo = data[idx++].Trim();
                                
                                string dataImport = date;

                                //string nrS        = data[2].Trim();               //nr personalny Siemens	
                                //string nrR = data[idx++].Trim();           //nr personalny Randstad	
                                //string _mpk = data[idx++].Trim();
                                //string _opis = data[idx++].Trim();
                                //string _jorg = data[idx++].Trim();
                                //string _jnazwa = data[idx++].Trim();
                                ////string kodAbs     = ToKod(data[idx++].Trim());    //kod absencji 	
                                //string kodAbs = data[idx++].Trim();           //kod absencji - zgodnie z mailem nie ustawiam
                                //string nazwaAbs = data[idx++].Trim();           //Rodzaj nieobecności	
                                //string dOd = ToDate(data[idx++].Trim());   //od	
                                //string dDo = ToDate(data[idx++].Trim());   //do	
                                //string _ilDniKal = ToNum(data[idx++].Trim());
                                //string ilDni = ToNum(data[idx++].Trim());    //ilość dni roboczych 	
                                //string ilGodz = ToNum(data[idx++].Trim());    //ilość godzin roboczych
                                //string _nrRef = data[idx++].Trim();           //Rodzaj nieobecności	                                
                                //----- kontrola -----    //20150113
                                if (/*!Tools.DateIsValid(dOd) || !Tools.DateIsValid(dDo) || String.IsNullOrEmpty(kodAbs) || String.IsNullOrEmpty(ilDni)*/ false)
                                {
                                    if (!IsEmpty(data))     // jednak coś jest, to error
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                    else                    // wszystko puste - footer ?
                                        if (!empty)
                                            empty = true;
                                }
                                else
                                {
                                    if (empty)              // poprzedni był pusty, a teraz są dane
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                }
                                //----- dane -----
                                //if (!String.IsNullOrEmpty(nrS) || !String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                //if (!String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                {
                                    bool ok = Base.execSQL(con, Base.insertSql("bufUmowy", 0,
                                        "KadryId,Imie,Nazwisko,DataOd,DataDo",
                                        db.strParam(kadryId),      //KadryId
                                        db.strParam(imie),         //Imię	
                                        db.strParam(naz),          //Nazwisko	
                                        db.strParam(dataOd),           //CC
                                        db.strParam(dataDo)    //Data
                                        ));
                                    if (!ok)
                                    {
                                        err = -3;     // błądz zapisu
                                        break;
                                    }
                                    cnt++;
                                }
                            }
                            else                // różna ilość kolumn
                            {
                                err = -1;       // format
                                break;
                            }
                        }
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

        //-----------------------
        // ABSENCJA SIEMENS WERSJA II 20150731
        private static bool CheckColumns(string pattern, string[] cols, out int colCount)
        {
            string[] head = pattern.Split(';');
            colCount = head.Length;
            if (cols.Length >= colCount)                // może być więcej kolumn
            {
                for (int i = 0; i < head.Length; i++)
                    if (head[i] != cols[i])             // zgodne nazwy
                        return false;
                return true;
            }
            else return false;
        }

        private static bool IsEmpty(string[] data)
        {
            for (int i = 0; i < data.Length; i++)
                if (!String.IsNullOrEmpty(data[i]))
                    return false;
            return true;
        }

        private static string ToNum(string n)           // wycinam separator tysiąca - ' '
        {
            n = n.Replace(',', '.');        //01234567890123   01234567890
            if (n.IndexOf(' ') != -1 || n.IndexOf((char)0xa0) != -1)       //123 123 123.12   123 123 123 -- JUAN: Dodałem warunek
            {
                int p = n.IndexOf('.');
                if (p == -1) p = n.Length + 1 - 4;
                else p -= 4;
                for (int i = p; i > 0; i = -4)          // ostry warunek wystarczy
                    if (n[i] < '0' || '9' < n[i])       // nie jest cyfrą, powinno być jeszcze sprawdzenie czy jak jest kilka to są takie same ...
                        n = n.Remove(i, 1);
                    else
                        return null;
            }
            return n;
        }

        private static string ToDate(string n)          // format
        {
            const string sep = "-";
            switch (n.Length)
            {
                case 8:
                    return null;            // póki co
                case 10:
                    if (n[2] == n[5])       // dd/mm/yyyy   !!! uwaga !!! nie ma jak rozpoznać US !!! mm/dd/yyyy; w SIEMENSIE dd/mm/yyyy
                        return n.Substring(6, 4) + sep + n.Substring(3, 2) + sep + n.Substring(0, 2);
                    else if (n[5] == n[8])  // yyyy-mm-dd
                        return n.Substring(0, 4) + sep + n.Substring(5, 2) + sep + n.Substring(9, 2);
                    else
                        return null;
                default:
                    return null;
            }
        }

        private static string ToKod(string n)           // dodaję 0 z przodu
        {
            while (n.Length < 4)
                n += "0" + n;
            return n;
        }

        private static int ImportFromFileABS2(SqlConnection con, string fileName, out int lineno)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            const string headPattern = "Nazwisko;Imię;Numer osobowy;MPK;Opis;Jednostka organiz.;Nazwa jedn.;Rodzaj nieobecnosci;Opis nieobecnosci;Od;Do;Dni kalendarzowe;Dni obecn./nieobecn.;Godziny nieobecności;Numer referencyjny";
            const char SEP = ';';

            lineno = 0;
            string line;
            int err = -2;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));
                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    //err = 0;
                    int cnt = 0;
                    int colCount = 0;
                    bool header = true;
                    bool empty = false;    //puste linie na końcu

                    while ((line = file.ReadLine()) != null)
                    {
                        lineno++;
                        string[] data = line.Split(SEP);
                        if (header)
                        {
                            err = 0;
                            if (!CheckColumns(headPattern, data, out colCount))
                            {
                                err = -1;       // format
                                break;
                            }
                            else
                            {
                                header = false;
                            }
                        }
                        else
                        {
                            if (data.Length == 1)   // pusta linia - footer ?
                            {
                                if (!empty)
                                    empty = true;
                            }
                            else if (data.Length >= colCount)
                            {
                                int idx = 0;
                                string naz = data[idx++].Trim();           //Nazwisko	
                                string imie = data[idx++].Trim();           //Imię	
                                //string nrS        = data[2].Trim();               //nr personalny Siemens	
                                string nrR = data[idx++].Trim();           //nr personalny Randstad	
                                string _mpk = data[idx++].Trim();
                                string _opis = data[idx++].Trim();
                                string _jorg = data[idx++].Trim();
                                string _jnazwa = data[idx++].Trim();
                                //string kodAbs     = ToKod(data[idx++].Trim());    //kod absencji 	
                                string kodAbs = data[idx++].Trim();           //kod absencji - zgodnie z mailem nie ustawiam
                                string nazwaAbs = data[idx++].Trim();           //Rodzaj nieobecności	
                                string dOd = ToDate(data[idx++].Trim());   //od	
                                string dDo = ToDate(data[idx++].Trim());   //do	
                                string _ilDniKal = ToNum(data[idx++].Trim());
                                string ilDni = ToNum(data[idx++].Trim());    //ilość dni roboczych 	
                                string ilGodz = ToNum(data[idx++].Trim());    //ilość godzin roboczych
                                string _nrRef = data[idx++].Trim();           //Rodzaj nieobecności	                                
                                //----- kontrola -----    //20150113
                                if (!Tools.DateIsValid(dOd) || !Tools.DateIsValid(dDo) || String.IsNullOrEmpty(kodAbs) || String.IsNullOrEmpty(ilDni))
                                {
                                    if (!IsEmpty(data))     // jednak coś jest, to error
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                    else                    // wszystko puste - footer ?
                                        if (!empty)
                                            empty = true;
                                }
                                else
                                {
                                    if (empty)              // poprzedni był pusty, a teraz są dane
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                }
                                //----- dane -----
                                //if (!String.IsNullOrEmpty(nrS) || !String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                if (!String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                {
                                    bool ok = Base.execSQL(con, Base.insertSql("bufAbsencja2", 0,
                                        "Nazwisko,Imie,NrEwSiemens,NrEwRandstad,DataOd,DataDo,KodAbs,NazwaAbs,IleDni,Godzin",
                                        db.strParam(naz),          //Nazwisko	
                                        db.strParam(imie),         //Imię	
                                        //db.strParam(nrS),        //nr personalny Siemens	
                                        db.NULL,
                                        db.strParam(nrR),          //nr personalny Randstad	
                                        db.strParam(dOd),          //od	
                                        db.strParam(dDo),          //do	
                                        db.strParam(kodAbs),       //kod absencji 	
                                        db.strParam(nazwaAbs),     //Rodzaj nieobecności	
                                        db.strParam(ilDni),        //ilość dni roboczych 	
                                        db.strParam(ilGodz)));     //ilość godzin roboczych
                                    if (!ok)
                                    {
                                        err = -3;     // błądz zapisu
                                        break;
                                    }
                                    cnt++;
                                }
                            }
                            else                // różna ilość kolumn
                            {
                                err = -1;       // format
                                break;
                            }
                        }
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
            Base.execSQL(con, "delete from bufAbsencja2");
        }

        public static int ImportData(int typ, string fileName, string _dataOd, string _dataDo, out bool noP, out bool noK, out int lineno, string sqlAfter, string headerFormat)
        {
            noP = false;
            noK = false;
            int lid = Log.Info(Log.t2APP_IMPORTRCP, LogInfo, String.Format("IMPORT CSV - START, typ: {0}, plik: {1}, od: {2}, do: {3}", typ, fileName, _dataOd, _dataDo), Log.PENDING);
            SqlConnection con = Base.Connect();

            //----- import -----
            int err;
            lineno = 0;
            switch (typ)
            {
                case tSIEAbsencjaOld:     //SIEMENS ABSENCJE
                    DeleteTmpTable(con);
                    err = ImportFromFileABS(con, fileName, out lineno);
                    break;
                case tSIEAbsencja:     //SIEMENS ABSENCJE 2 20150731
                    DeleteTmpTable(con);
                    err = ImportFromFileABS2(con, fileName, out lineno);
                    break;
                case tSCLP:
                    Base.execSQL(con, "delete from bufListaPlac");
                    err = ImportListaPlac(con, fileName, out lineno, _dataOd);
                    break;
                case tSCU:
                    Base.execSQL(con, "delete from bufUmowy");
                    err = ImportUmowy(con, fileName, out lineno, _dataOd);
                    break;
                    break;
                case tOKTPracownicy:
                    err = OKT_ImportPracownicy(con, fileName, out lineno, _dataOd, headerFormat);
                    break;
                default:
                    err = -999;
                    break;
            }
            //----- processing -----
            if (err == 0)
            {
                try
                {
                    bool ok;
                    switch (typ)
                    {
                        case tSIEAbsencjaOld:
                        case tSIEAbsencja:
                            //----- nie znalezieni pracownicy -----
                            /*
                            DataSet ds = db.getDataSet(con, @"
select distinct ISNULL(B.Nazwisko,'') + ' ' + ISNULL(B.Imie,'') + ' [S:' + ISNULL(B.NrEwSiemens,'null') + '] [R:' + ISNULL(B.NrEwRandstad,'null') + ']' as Prac from bufAbsencja2 B
left join Pracownicy P on (P.KadryId3 = ISNULL(B.NrEwSiemens, '') or P.KadryId = ISNULL(B.NrEwRandstad, ''))
where P.Id is null
                                ");
                            */
                            DataSet ds = db.getDataSet(con, @"
select distinct ISNULL(B.Nazwisko,'') + ' ' + ISNULL(B.Imie,'') + ' [R:' + ISNULL(B.NrEwRandstad,'null') + ']' as Prac from bufAbsencja2 B
left join Pracownicy P on P.KadryId = ISNULL(B.NrEwRandstad, 'err')
where P.Id is null
                                ");         // 20150731 zmiana ISNULL '' -> 'err' jak będzie coś z '' to wszystko tam wpadnie ... z drugiej strony numery muszą być
                            if (db.getCount(ds) > 0)
                            {
                                noP = true;
                                Log.Info(Log.t2APP_IMPORTRCP, lid, "IMPORT ABSENCJE - BRAK POWIĄZANIA", db.Join(ds, 0, ", "), Log.OK);
                            }
                            //----- nie znalezione kody  -----
                            ds = db.getDataSet(con, @"
select distinct KodAbs + ISNULL(' - ' + B.NazwaAbs, '') as Absencja from bufAbsencja2 B
left join AbsencjaKody AK on AK.Kod2 = B.KodAbs
where AK.Kod is null
                                ");
                            if (db.getCount(ds) > 0)
                            {
                                noK = true;
                                Log.Info(Log.t2APP_IMPORTRCP, lid, "IMPORT ABSENCJE - BRAK KODU ABSENCJI", db.Join(ds, 0, ", "), Log.OK);
                            }
                            //----- import -----
                            /*
                            ok = db.execSQL(con, String.Format(@"          
declare @od datetime
declare @do datetime
set @od = '{0}'
set @do = '{1}'
delete from Absencja where DataOd <= @do and @do <= DataDo
          
insert into Absencja
select P.Id, P.KadryId, B.DataOd, B.DataDo, AK.Kod, CAST(ROUND(B.IleDni,0) as int), B.Godzin from bufAbsencja2 B
left join AbsencjaKody AK on AK.Kod2 = B.KodAbs
left join Pracownicy P on (P.KadryId3 = ISNULL(B.NrEwSiemens, '') or P.KadryId = ISNULL(B.NrEwRandstad, ''))
where P.Id is not null and AK.Kod is not null
                            ");
                            */

                            DataRow dr = db.getDataRow(String.Format(@"
BEGIN TRANSACTION;

BEGIN TRY
------------------------
declare @od datetime
declare @do datetime
set @od = '{0}'
set @do = '{1}'
delete from Absencja where DataOd <= @do and @od <= DataDo
          
insert into Absencja
select P.Id, P.KadryId, B.DataOd, B.DataDo, AK.Kod, CAST(ROUND(B.IleDni,0) as int), B.Godzin from bufAbsencja2 B
left join AbsencjaKody AK on AK.Kod2 = B.KodAbs
--left join Pracownicy P on (P.KadryId3 != '0' and P.KadryId3 = ISNULL(B.NrEwSiemens, '') or P.KadryId = ISNULL(B.NrEwRandstad, ''))
left join Pracownicy P on P.KadryId = ISNULL(B.NrEwRandstad, 'err')
where P.Id is not null and AK.Kod is not null
------------------------
END TRY
BEGIN CATCH
	select -3 as Error, ERROR_MESSAGE() AS ErrorMessage;
	/*
    SELECT 
         ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	*/
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0 BEGIN
    COMMIT TRANSACTION;
    select 0 as Error, null as ErrorMessage
END    
                            ", _dataOd, _dataDo));   // tu jest początek i koniec roku albo miesiaca zeby nie trzeba bylo modyfikowac
                            err = db.getInt(dr, 0, -3);
                            if (err != 0)
                            {
                                Log.Update(lid, Log.ERROR);
                                Log.Error(Log.t2APP_IMPORTRCP, lid, "IMPORT ABSENCJE - ERROR", db.getValue(dr, 1));
                            }
                            break;
                        case tSCLP:
                            DataRow dr2 = db.getDataRow(String.Format(@"
BEGIN TRANSACTION;

BEGIN TRY
------------------------
declare @data datetime = '{0}'
delete from scListaPlac where Data = @data 
          
insert into scListaPlac
select B.KadryId, B.Nazwisko, B.Imie, B.CC, B.Brutto, B.ZUS, B.KosztFirma, B.Data
from bufListaPlac B
------------------------
END TRY
BEGIN CATCH
	select -3 as Error, ERROR_MESSAGE() AS ErrorMessage;
	/*
    SELECT 
         ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	*/
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0 BEGIN
    COMMIT TRANSACTION;
    select 0 as Error, null as ErrorMessage
END    
                            ", _dataOd));   // tu jest początek i koniec roku albo miesiaca zeby nie trzeba bylo modyfikowac
                            err = db.getInt(dr2, 0, -3);
                            if (err != 0)
                            {
                                Log.Update(lid, Log.ERROR);
                                Log.Error(Log.t2APP_IMPORTRCP, lid, "IMPORT LISTA PŁAC - ERROR", db.getValue(dr2, 1));
                            }
                            break;
                        case tSCU:
                            DataRow dr3 = db.getDataRow(String.Format(@"
BEGIN TRANSACTION;

BEGIN TRY
------------------------
--declare @data datetime = '{0}'
select bu.Id as buId, p.Id as IdPracownika, CONVERT(DATETIME, bu.DataOd) as UmowaOd, CONVERT(DATETIME, bu.DataDo) as UmowaDo, p.KadryId into #aaa from bufUmowy bu
left join Pracownicy p on p.KadryId = bu.KadryId
where p.Id is not null

update PracownicyUmowy set Status = -1

update PracownicyUmowy set
  UmowaOd = a.UmowaOd
, UmowaDo = a.UmowaDo
, LpLogo = a.KadryId
, DataOd = a.UmowaOd
, DataDo = a.UmowaDo
, UmowaTyp = ''
, Status = 0
, lp_UmowyId = buId
, UmowaNumer = buId
from
PracownicyUmowy bu
inner join #aaa a on bu.IdPracownika = a.IdPracownika
select * from #aaa
insert into PracownicyUmowy (IdPracownika, UmowaOd, UmowaDo, LpLogo, DataOd, DataDo, UmowaTyp, Status, lp_UmowyId, UmowaNumer) select IdPracownika, UmowaOd, UmowaDo, KadryId, UmowaOd, UmowaDo, '', 1, buId, buId from #aaa where IdPracownika not in (select IdPracownika from PracownicyUmowy)

drop table #aaa
END TRY
BEGIN CATCH
	select -3 as Error, ERROR_MESSAGE() AS ErrorMessage;
	/*
    SELECT 
         ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	*/
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0 BEGIN
    COMMIT TRANSACTION;
    select 0 as Error, null as ErrorMessage
END    
                            ", _dataOd));   // tu jest początek i koniec roku albo miesiaca zeby nie trzeba bylo modyfikowac


                            break;
                        case tOKTPracownicy:
                            if (!String.IsNullOrEmpty(sqlAfter))
                            {
                                DataRow dr9 = db.getDataRow(String.Format(sqlAfter, _dataOd));
                                err = db.getInt(dr9, 0, -3);
                                if (err != 0)
                                {
                                    Log.Update(lid, Log.ERROR);
                                    Log.Error(Log.t2APP_IMPORTRCP, lid, "IMPORT CSV - ERROR", db.getValue(dr9, 1));
                                }
                            }
                            break;
                    }
                    Log.Update(lid, Log.OK);
                    Log.Info(Log.t2APP_IMPORTRCP, LogInfo, "IMPORT CSV - END", Log.OK);
                }
                catch (Exception ex)
                {
                    err = -3;
                    Log.Update(lid, Log.ERROR);
                    Log.Error(Log.t2APP_IMPORTRCP, lid, "IMPORT - ERROR", ex.Message);
                    if (con != null) Base.Disconnect(con);
                    throw;
                }
            }

            if (err != 0)
            {
                string msg = GetErrorMsg(err, noP, noK, lineno);
                Log.Error(Log.t2APP_IMPORTRCP, lid, "IMPORT CSV - ERROR", msg);
            }

            if (con != null) Base.Disconnect(con);
            return err;
        }


        // sc

        void PrepareSCLP()
        {
            btImport.Text = "Import listy płac";
            lblInfo.Text = "Miesiąc rozliczeniowy:";
            ddlMiesiac.DataSourceID = null;
            ddlMiesiac.DataSource = dsListaPlac;
            ddlMiesiac.DataBind();
        }

        void PrepareSCU()
        {
            btImport.Text = "Import umów";
            lblInfo.Visible = false;
            //lblInfo.Text = "Miesiąc rozliczeniowy:";
            ddlMiesiac.Visible = false;
            //ddlMiesiac.DataSourceID = null;
            //ddlMiesiac.DataSource = dsListaPlac;
            //ddlMiesiac.DataBind();
        }

        public void SelectBind()
        {
            ddlMiesiac.DataSource = dsOkresyRozl;
            ddlMiesiac.DataBind();
        }

        public DropDownList Select
        {
            get { return ddlMiesiac; }
        }
        //-----------------------------------
        // keeeper
        //-----------------------------------
        private void OKT_ImportPracownicyPrepare()
        {
            btImport.Text = "Import pracowników";
            lblInfo.Visible = false;
            lbInfo9.Visible = true;
            ddlMiesiac.DataSourceID = null;
            ddlMiesiac.DataSource = dsOkresyRozl;
            ddlMiesiac.DataBind();
        }

        private static int OKT_ImportPracownicy(SqlConnection con, string fileName, out int lineno, string date, string headPattern)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            //const string headPattern = "Liczba porządkowa;Nazwisko;Imię (pierwsze);Identyfikator;Stanowisko;Dział Wydział;Oddział;Kod funkcyjny;Agencja;Rodzaj zmiany";
            //const string headPattern = "Lp.;Nazwisko;Imi© (pierwsze);Identyfikator;Stanowisko;Dzia Wydzia;Oddzia;Kod funkcyjny;AGENCJA;Rodzaj zmiany;;";
            const string table = "bufPracownicyImport";
            //const string fields = "Lp,Nazwisko,Imie,Identyfikator,Stanowisko,DzialWydzial,Oddzial,KodFunkcyjny,Agencja,RodzajZmiany";
            const string fields = "Lp,Nazwisko,Imie,Identyfikator,DzialWydzial,Funkcja,Kod,KodOpis,Agencja,DataZatrudnienia";
            const char SEP = ';';
            
            //string[] ff = fields.Split(',');   <<<<<<< docelowo zautomatyzować, ale trzeba by określić typy pól, bo może być ToNum itp.
            //int fcount = ff.Length;
            //string[] values = new string[fcount]; 

            lineno = 0;
            string line;
            int err = -2;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));
                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    int cnt = 0;
                    int colCount = 0;
                    bool header = true;
                    bool empty  = false;    //puste linie na końcu

                    db.execSQL(con, String.Format("delete from {0}", table));

                    while ((line = file.ReadLine()) != null)
                    {
                        lineno++;
                        string[] data = line.Split(SEP);
                        if (header)
                        {
                            err = 0;
                            if (!CheckColumns(headPattern, data, out colCount))
                            {
                                err = -1;       // format
                                break;
                            }
                            else
                            {
                                header = false;
                            }
                        }
                        else
                        {
                            if (data.Length == 1)   // pusta linia - footer ?
                            {
                                if (!empty)
                                    empty = true;
                            }
                            else if (data.Length >= colCount)
                            {
                                int idx = 0;
                                //string p0 = data[idx++].Trim();         //lp
                                //string p1 = data[idx++].Trim();        //naz
                                //string p2 = data[idx++].Trim();       //imie
                                //string p3 = data[idx++].Trim();      //ident
                                //string p4 = data[idx++].Trim();       //stan 
                                //string p5 = data[idx++].Trim();      //dzial
                                //string p6 = data[idx++].Trim();    //oddzial
                                //string p7 = data[idx++].Trim();       //kodF 
                                //string p8 = data[idx++].Trim();    //agencja
                                //string p9 = data[idx++].Trim();      //zmany


                                string p0 = data[idx++].Trim();         //lp

                                string nazw = data[idx++].Trim();
                                string[] spl = nazw.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                string p1 = String.Empty;
                                string p2 = String.Empty;
                                string p3 = String.Empty;

                                if (spl.Length > 0)
                                {
                                    p1 = spl[0];         // nazwisko
                                    p2 = spl[1];         // imie
                                    p3 = null;
                                    if (spl.Length == 3)
                                        p3 = spl[2].Substring(1, spl[2].Length - 2); //ident
                                }
                                string p4 = data[idx++].Trim(); // jednostka
                                string p5 = data[idx++].Trim(); // funkcja
                                string p6 = data[idx++].Trim(); // kod
                                string p7 = data[idx++].Trim(); // kod opis
                                string p8 = data[idx++].Trim(); // agencja
                                string p9 = data[idx++].Trim(); // data zatrudnienia 

                                string dataImport = date;

                                //----- kontrola -----    
                                if (/*!Tools.DateIsValid(dOd) || !Tools.DateIsValid(dDo) || String.IsNullOrEmpty(kodAbs) || String.IsNullOrEmpty(ilDni)*/ false)
                                {
                                    if (!IsEmpty(data))     // jednak coś jest, to error
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                    else                    // wszystko puste - footer ?
                                        if (!empty)
                                            empty = true;
                                }
                                else
                                {
                                    if (empty)              // poprzedni był pusty, a teraz są dane
                                    {
                                        err = -1;           // format
                                        break;
                                    }
                                }
                                //----- dane -----
                                //if (!String.IsNullOrEmpty(nrS) || !String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                //if (!String.IsNullOrEmpty(nrR))   // tylko jak jest numer !!!
                                {
                                    bool ok = Base.execSQL(con, Base.insertSql("bufPracownicyImport", 0,
                                        fields
                                        , db.strParam(p0)
                                        , db.strParam(p1)
                                        , db.strParam(p2)
                                        , db.strParam(p3)
                                        , db.strParam(p4)
                                        , db.strParam(p5)
                                        , db.strParam(p6)
                                        , db.nullStrParam(p7)
                                        , db.strParam(p8)
                                        , db.nullStrParam(p9) // null zeby isnulle dzialaly - data zatrudnienia
                                    ));
                                    if (!ok)
                                    {
                                        err = -3;     // błądz zapisu
                                        break;
                                    }
                                    cnt++;
                                }
                            }
                            else                // różna ilość kolumn
                            {
                                err = -1;       // format
                                break;
                            }
                        }
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
    }
}
