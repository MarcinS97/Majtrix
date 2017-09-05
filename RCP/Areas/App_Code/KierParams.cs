using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace HRRcp.App_Code
{
    public class KierParams
    {
        const string sql = "select * from KierParams where IdKierownika = ";

        DataRow FData = null;
        string FKierId = null;
        Ustawienia FSettings;

        /*
        public KierParams(string kierId, string pracId, Ustawienia settings)  // kierId podstawic null !!!
        {
            FKierId = Base.getScalar("select IdKierownika from Pracownicy where Id = " + pracId);
            FSettings = settings;
        }
        */
        public KierParams(string kierId, Ustawienia settings)       
        {
            FKierId = kierId;
            FSettings = settings;
        }

        public KierParams(string kierId)       
        {
            FKierId = kierId;
            FSettings = Ustawienia.CreateOrGetSession();
        }

        public KierParams()       // jak settings == null to def bierze
        {
            FKierId = AppUser.CreateOrGetSession().Id;
            FSettings = Ustawienia.CreateOrGetSession();
        }
        //----------------------
        public void GetAccDates(out DateTime accOd, out DateTime accDo)
        {
            accOd = FSettings.SystemStartDate;
            DateTime? dt = DataAccDo;
            if (dt == null) accDo = accOd.AddDays(-1); // 1 dzien wczesniej to nic nie zaznaczy
            else accDo = (DateTime)dt;
        }
        //----------------------
        public static bool Update(string kierId, string przerwa, string przerwa2, string margines)
        {
            bool b = Base.execSQL(Base.updateSql("KierParams", 1,
                    "PrzerwaMM, Przerwa2MM, MarginesMM",
                    "IdKierownika={0}",
                    kierId,
                    Base.nullParam(przerwa), Base.nullParam(przerwa2), Base.nullParam(margines)
                ));     // true jesli dodał
            if (!b)
                b = Base.execSQL(Base.insertSql("KierParams", 0,
                    "IdKierownika, PrzerwaMM, Przerwa2MM, MarginesMM",
                    kierId,
                    Base.nullParam(przerwa), Base.nullParam(przerwa2), Base.nullParam(margines)
                ));
            return b;
        }

        public static bool Update(string kierId, string przerwa, string przerwa2, string margines, string dataAccDo)
        {
            bool b = Base.execSQL(Base.updateSql("KierParams", 1,
                    "PrzerwaMM, Przerwa2MM, MarginesMM, DataAccDo", 
                    "IdKierownika={0}",
                    kierId,
                    Base.nullParam(przerwa), Base.nullParam(przerwa2), Base.nullParam(margines), Base.nullDateParam(dataAccDo)
                ));     // true jesli dodał
            if (!b)
                b = Base.execSQL(Base.insertSql("KierParams", 0,
                    "IdKierownika, PrzerwaMM, Przerwa2MM, MarginesMM, DataAccDo",
                    kierId,
                    Base.nullParam(przerwa), Base.nullParam(przerwa2), Base.nullParam(margines), Base.nullDateParam(dataAccDo)
                ));
            return b;
        }

        public static bool Update(string kierId, DateTime accDo)
        {
            bool b = db.update("KierParams", 1,
                    "DataAccDo",
                    "IdKierownika={0}",
                    kierId, Base.strParam(Base.DateToStr(accDo))
                );     // true jesli dodał
            if (!b)
                b = db.insert("KierParams", 0,
                    "IdKierownika, DataAccDo",
                    kierId, Base.strParam(Base.DateToStr(accDo))
                );
            return b;
        }
        //----------------------
        public DataRow Data
        {
            get 
            {
                if (FData == null && !String.IsNullOrEmpty(FKierId))
                    FData = Base.getDataRow(sql + FKierId);
                return FData;
            }
        }

        public Ustawienia Settings
        {
            get { return FSettings; }
        }

        public bool IsKierParams
        {
            get { return Data != null; }
        }

        public string KierId
        {
            get { return FKierId; }
        }
        //-----
        public int? KierPrzerwa
        {
            get { return Base.getInt(Data, "PrzerwaMM"); }
        }

        public int? KierPrzerwa2
        {
            get { return Base.getInt(Data, "Przerwa2MM"); }
        }

        public int? KierMargines
        {
            get { return Base.getInt(Data, "MarginesMM"); }
        }
        //-----
        public int Przerwa
        {
            get
            {
                int? p = KierPrzerwa;
                if (p == null) p = FSettings.Przerwa;
                return (int)p;
            }
        }

        public int Przerwa2
        {
            get
            {
                int? p = KierPrzerwa2;
                if (p == null) p = FSettings.Przerwa2;
                return (int)p;
            }
        }

        public int Margines
        {
            get
            {
                int? p = KierMargines;
                if (p == null) p = FSettings.Margines;
                return (int)p;
            }
        }
        //-----
        public DateTime? DataAccDo
        {
            get { return Base.getDateTime(Data, "DataAccDo"); }
        }
    }
}
