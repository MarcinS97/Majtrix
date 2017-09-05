using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.Scorecards.App_Code
{
    public class Dictionaries
    {
        public static class Types
        {
            public const String Spreadsheets = "ARK";
            public const String Productivity = "PROD";
            public const String QC = "QC";

            public const String PGIO = "PGIO";
            public const String AdditionalWork = "PREM";
            public const String BreakTime = "SANDWICH";
        }
        public static class Spreadsheets
        {
            public const String Individual = "4";   //"ARKI";
            public const String Team = "5";         //"ARKZ";
        }

        public static class Reports
        {
            public const int RapCC = 1;
            public const int RapProdukcjaRok = 7;
            public const int RapRozdzielnik = 9;
            public const int RapStanOsob = 10;
            public const int RapProduktywnosc = 15;
            public const int RapAbs = 17;
            public const int RapAbsZoom = 19;
            public const int RapPracownicy = 20;
            public const int RapPracownicyDaty = 21;
            public const int RapRozdzielnikBledy = 22;
        }
    }
}
