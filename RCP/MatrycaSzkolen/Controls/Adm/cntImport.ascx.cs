using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Adm
{
    public partial class cntImport : System.Web.UI.UserControl
    {
        public event EventHandler ImportFinished;
        public const string WY = " (WY)";
        public bool UpdateReaders = false;

        const string LogInfo = "Import absencji z pliku";

        const string extBAK = ".bak";


        const int tSCLP = 7;
        const int tSCU = 8;
        const int tMSW = 9;

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareMSW();
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            ImportData1(Typ);
        }

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

        protected void ImportData1(int typ)  // 1
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(@"uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUpload1.SaveAs(savePath);
                string dod, ddo;
                bool noP, noK;

                switch (Typ)
                {
                    case tSCLP:
                        dod = ddlMiesiac.SelectedValue;
                        ddo = null;
                        break;
                    default:
                        Tools.GetLineParams(ddlMiesiac.SelectedValue, out dod, out ddo);
                        break;
                }
                int lineno;
                int err = ImportData(typ, savePath, dod, ddo, out noP, out noK, out lineno);
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

        private static string getValue(string line, ref int from, int count, int space)
        {
            string ret = Tools.Substring(line, from, count);
            from += count + space;
            return ret;
        }




        private static int ImportWartosci(SqlConnection con, string fileName, out int lineno/*, string date*/)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            /*const string headPattern = "Numer osobowy;Nazwisko;Imię;Miejsce powst. kosztów; Brutto ; ZUS firma razem ; Koszt firma ";*/

            const string headPattern = "Date;aoe;Name;Work place;Qunatity [pcs];Shall [pcs];Scrap;Production time;No production time";
            const char SEP = ';';

            DataRow[] patterns = new DataRow[9];

            for (int i = 0; i < 9; ++i)
            {
                patterns[i] = db.Select.Row("select * from Kody where Typ = 'IMPORT' and Lp = {0} and Aktywny = 1", i);
            }

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
                    int dbw_mode = 0;
                    bool header = true;
                    bool empty = false;    //puste linie na końcu
                    file.ReadLine(); /*  pierwsza linia: randomowe liczby      */
                    /*file.ReadLine();*/ /*  druga linia: polskie naglowki         */
                    /*file.ReadLine();*/ /*  chory line break w polowie            */
                    /* wylecialo, bo wycieli angielskie */
                    while ((line = file.ReadLine()) != null && line.Replace(SEP.ToString(), "") != "")
                    {
                        lineno++;
                        string[] data = line.Split(SEP);
                        if (header)
                        {
                            err = 0;
                            /*if (!CheckColumns(headPattern, data, out colCount))
                            {
                                err = -1;       // format
                                break;
                            }
                            else
                            {
                                header = false;
                            }*/

                            if (data[(int)patterns[8]["Kod"]].ToString() == (patterns[8]["Nazwa"]).ToString()) dbw_mode = 1;
                            else if (data[(int)patterns[8]["Kod"]].ToString() == (patterns[8]["Nazwa2"]).ToString()) dbw_mode = 2;
                            else dbw_mode = -1;

                            file.ReadLine(); /* chory line break w polowie */

                            for (int i = 0; i < 8; ++i)
                            {
                                if (!data[(int)patterns[i]["Kod"]].ToString().StartsWith((patterns[i]["Nazwa"]).ToString())) dbw_mode = -1; /* StartsWith, bo ProductionTime+T17720 */
                            }

                            if (dbw_mode != -1)
                            {
                                header = false;
                            }
                            else
                            {
                                err = -1;
                                break;
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
                                /*String date = data[idx++].Trim();
                                String kadryId = data[idx++].Trim();
                                String pracownik = data[idx++].Trim();
                                String stanowisko = data[idx++].Trim();
                                String quantity = data[idx++].Trim();
                                String shall = data[idx++].Trim();
                                String scrap = data[idx++].Trim();
                                String prod = data[idx++].Trim();
                                String nieprod = data[idx++].Trim();*/

                                /* stale whardcodowane na stale, normalnie jechac idx */

                                String date = data[(int)patterns[0]["Kod"]].Trim(); /*A*/
                                String kadryId = data[(int)patterns[1]["Kod"]].Trim(); /*D*/
                                String pracownik = data[(int)patterns[2]["Kod"]].Trim(); /*E*/
                                String stanowisko = data[(int)patterns[3]["Kod"]].Trim(); /*F*/
                                String quantity = data[(int)patterns[4]["Kod"]].Trim(); /*I*/
                                String shall = data[(int)patterns[5]["Kod"]].Trim(); /*J*/
                                String scrap = data[(int)patterns[6]["Kod"]].Trim(); /*N*/
                                String prod = data[(int)patterns[7]["Kod"]].Trim(); /*U*/
                                String nieprod;
                                if (dbw_mode == 1) nieprod = data[(int)patterns[8]["Kod"]].Trim(); /*V*/
                                else nieprod = "00:00";

                                /*string kadryId = data[idx++].Trim();       //KadryId
                                string naz = data[idx++].Trim();           //Nazwisko	
                                string imie = data[idx++].Trim();           //Imię	
                                string cc = data[idx++].Trim();
                                string brutto = ToNum(data[idx++].Trim());
                                string zus = ToNum(data[idx++].Trim());
                                string kosztFirma = ToNum(data[idx++].Trim());*/
                                /*string dataImport = date;*/

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
                                    /*bool ok = Base.execSQL(con, Base.insertSql("bufListaPlac", 0,
                                        "KadryId,Nazwisko,Imie,CC,Brutto,ZUS,KosztFirma,Data",
                                        db.strParam(kadryId),      //KadryId
                                        db.strParam(naz),          //Nazwisko	
                                        db.strParam(imie),         //Imię	i	
                                        db.strParam(cc),           //CC
                                        db.strParam(brutto),       //Brutto
                                        db.strParam(zus),          //ZUS
                                        db.strParam(kosztFirma),   //KosztFirma
                                        db.strParam(dataImport)    //Data
                                        ));*/
                                    bool ok = Base.execSQL(con, Base.insertSql("bufWartosci", 0,
                                        "Data, KadryId, Pracownik, Stanowisko, IloscWyprodukowana, IloscWymagana, Braki, IloscGodzin, CzasNieprod", /*GrupaStanowisk, IndexPrasy",*/
                                        db.strParam(ToDate(date)),
                                        db.strParam(kadryId),
                                        db.strParam(pracownik),
                                        db.strParam(stanowisko),
                                        db.strParam(quantity),
                                        db.strParam(shall),
                                        db.strParam(scrap),
                                        db.strParam(prod),
                                        db.strParam(nieprod)
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






        private static int ImportListaPlac(SqlConnection con, string fileName, out int lineno, string date)   // -1 niepoprawny format, -2 brak pliku, -3 błąd zapisu do bazy
        {
            const string headPattern = "Numer osobowy;Nazwisko;Imię;Miejsce powst. kosztów; Brutto ; ZUS firma razem ; Koszt firma ";
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
                    /*return null;            // póki co*/
                    if (n[2] == n[5])
                        return ((int.Parse(n.Substring(6, 2)) < 70) ? "20" : "19") + n.Substring(0, 2) + sep + n.Substring(3, 2) + sep + n.Substring(6, 2);
                    /* DLA SIECI NEURONOWYCH, KTORE BEDA KIEDYS EDYTOWALY TEN KOD:
                     * APLIKACJA OGOLNIE PADA W 2070
                     * JAKBYSCIE MOGLI, ZROBCIE, ZEBY TO DZIALALO LEPIEJ
                     * POZDRO DLA CIA
                     */
                    else
                        return null;
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

        public static void DeleteTmpTable(SqlConnection con)
        {
            Base.execSQL(con, "delete from bufAbsencja2");
        }

        public static int ImportData(int typ, string fileName, string _dataOd, string _dataDo, out bool noP, out bool noK, out int lineno)
        {
            noP = false;
            noK = false;
            int lid = Log.Info(Log.t2APP_IMPORTRCP, LogInfo, String.Format("IMPORT ABSENCJE - START, typ: {0}, plik: {1}, od: {2}, do: {3}", typ, fileName, _dataOd, _dataDo), Log.PENDING);
            SqlConnection con = Base.Connect();

            DeleteTmpTable(con);

            //----- import -----
            int err;
            lineno = 0;
            switch (typ)
            {
                /*case 5:     //SIEMENS ABSENCJE
                    err = ImportFromFileABS(con, fileName, out lineno);
                    break;
                case 6:     //SIEMENS ABSENCJE 2 20150731
                    err = ImportFromFileABS2(con, fileName, out lineno);
                    break;*/
                case tSCLP:
                    Base.execSQL(con, "delete from bufListaPlac");
                    err = ImportListaPlac(con, fileName, out lineno, _dataOd);
                    break;
                case tMSW:
                    Base.execSQL(con, "delete from bufWartosci");
                    err = ImportWartosci(con, fileName, out lineno);
                    break;
                /*case tSCU:
                    Base.execSQL(con, "delete from bufUmowy");
                    err = ImportUmowy(con, fileName, out lineno, _dataOd);
                    break;
                    break;*/
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
                        case 5:
                        case 6:
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
                        case tMSW:
                            DataRow dr4 = db.getDataRow(String.Format(@"
BEGIN TRANSACTION;

BEGIN TRY

/* IMPORT */

insert into msLinie (Nazwa)
select distinct Stanowisko
from bufWartosci bw
left join msLinie l on l.Nazwa = bw.Stanowisko
where l.Id is null

select
  p.Id IdPracownika
, l.Id IdLinii
, CONVERT(datetime, bw.Data) Data
, (CAST(DATEDIFF(minute, 0, CONVERT(datetime, bw.IloscGodzin)) as float) / 60) - (CAST(DATEDIFF(minute, 0, CONVERT(datetime, bw.CzasNieprod)) as float) / 60) IloscGodzin
, CAST(bw.IloscWyprodukowana as int) IloscWyprodukowana
, CAST(case when bw.IloscWymagana != '' then bw.IloscWymagana else bw.IloscWyprodukowana end as int) IloscWymagana
, CAST(bw.Braki as int) Braki
into #aaa
from bufWartosci bw
/*left join Pracownicy p on UPPER(p.Nazwisko + ' ' + p.Imie) = UPPER(bw.Pracownik)*/
left join Pracownicy p on /*UPPER(p.Nazwisko + ' ' + p.Imie) = UPPER(bw.Pracownik)*/ p.KadryId = /*REPLACE(STR(bw.KadryId, 5), ' ', '0')*/ RIGHT('00000' + bw.KadryId, 5)
left join msLinie l on l.Nazwa = bw.Stanowisko
where p.Id is not null

update msWartosci set
  Aktywny = 0
from msWartosci w
inner join #aaa a on a.IdPracownika = w.IdPracownika and a.IdLinii = w.IdLinii and a.Data = w.Data

insert into msWartosci (IdPracownika, IdLinii, Data, IloscGodzin, IloscWyprodukowana, IloscWymagana, Braki, Aktywny)
select IdPracownika, IdLinii, Data, IloscGodzin, IloscWyprodukowana, IloscWymagana, Braki, 1 from #aaa

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
"));
                            break;
                    }
                    Log.Update(lid, Log.OK);
                    Log.Info(Log.t2APP_IMPORTRCP, LogInfo, "IMPORT UMOWY - END", Log.OK);
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
                Log.Error(Log.t2APP_IMPORTRCP, lid, "IMPORT ABSENCJE - ERROR", msg);
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

        // ms

        void PrepareMSW()
        {
            btImport.Text = "Import wartości";
            lblInfo.Visible = false;
            //lblInfo.Text = "Miesiąc rozliczeniowy:";
            ddlMiesiac.Visible = false;
            //ddlMiesiac.DataSourceID = null;
            //ddlMiesiac.DataSource = dsListaPlac;
            //ddlMiesiac.DataBind();
        }

        public Int32 Typ
        {
            get {return 9;}
            set{}
        }

    }
}