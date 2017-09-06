using HRRcp.Areas.ME.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.Areas.ME.Models.CustomModels
{

    public class Pracownik
    {
        private IOcenaValue ocenaValue;
        public int Id_Pracownicy { get; set; }

        public string Nr_Ewid { get; set; }

        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public bool APT { get; set; }

        public string Stanowisko { get; set; }

        public string UmowaZatrudnienia { get; set; }

        public string JednostkaMacierzysta { get; set; }

        public int Ocena { get; set; }

        public string Status { get; set; }

        public Pracownik(int Id_Pracownicy, IOcenaValue interfaceParam = null)
        {
            ocenaValue = interfaceParam;
            MatrycaMVC temp = new MatrycaMVC();

            var pracownik = temp.Pracownicy.Find(Id_Pracownicy);

            if (pracownik.Stanowiska != null)
            {

                Stanowisko = pracownik.Stanowiska.Nazwa_Stan;
            }
            else Stanowisko = "-";
            

            var status = temp.Status.Find(pracownik.Status);
            if (status != null)
            {
                Status = status.Nazwa;
            } 
            if (status == null) Status = "-";

            #region jednostkaMacierzysta
            string jedn = "-";

            var jednostka = temp.StrOrg.Find(pracownik.Id_Str_OrgM);
            if (jednostka != null)
            {

                var nadrzedna = temp.StrOrg.Find(jednostka.Id_Parent);
                var organizacyjna = jednostka.Symb_Jedn;

                jedn = organizacyjna.ToString() + " / " + nadrzedna.Symb_Jedn.ToString() + " ";
            }
            var kierownikPracownik = temp.Pracownicy.Find(pracownik.IdKierownika);
            if (kierownikPracownik != null)
            {
                jedn += "<br /><span class=\"grey-text\">"
                    + kierownikPracownik.Imie + " " + kierownikPracownik.Nazwisko + "</span>";
            }

            JednostkaMacierzysta = jedn;



            #endregion

            #region dataUmowy
            DateTime umowaOd = new DateTime();
            DateTime umowaDo = new DateTime();
            if (pracownik.DataZatr != null)
            {
                umowaOd = (DateTime)pracownik.DataZatr.Value;
            }
            if (pracownik.DataUmDo != null)
            {
                umowaDo = (DateTime)pracownik.DataUmDo.Value;
            }



            string daty = umowaOd.Day + "." + umowaOd.Month + "." + umowaOd.Year + " - " +
                umowaDo.Day + "." + umowaDo.Month + "." + umowaDo.Year;

            UmowaZatrudnienia = daty;
            #endregion

            #region oceny
            var ocena = temp.Oceny.Where(x => x.Id_Pracownicy == pracownik.Id_Pracownicy);
            int iOcena = 0;
            if (ocena.Any())
            {
                var tmp = ocenaValue.getWartoscOceny(ocena.ToArray());
                iOcena = (int)tmp;
            }
            Ocena = iOcena;

            #endregion

            Id_Pracownicy = pracownik.Id_Pracownicy;
            Imie = pracownik.Imie;
            Nazwisko = pracownik.Nazwisko;
            Nr_Ewid = pracownik.Nr_Ewid;
            APT = pracownik.APT;

        }
    }
}