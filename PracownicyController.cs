using HRRcp.Areas.ME.Models;
using HRRcp.Areas.ME.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRRcp.Areas.ME.Controllers
{
    public class PracownicyController : Controller
    {
        MatrycaMVC bazaMatryca = new MatrycaMVC();
        // GET: ME/Pracownicy

        public ActionResult PracownicyIndex()
        {

            List<Pracownik> listaPracownikow = new List<Pracownik>();
            // Załadowanie pracowników
            var pracownicy = bazaMatryca.Pracownicy.Take(8);
            foreach (var pracownik in pracownicy)
            {
                listaPracownikow.Add(
                    new Pracownik (pracownik.Id_Pracownicy));
            }

            return View(listaPracownikow);
        }


        //Ajaxowe pobranie danych pracownika
        public ActionResult getDanePracownikaModal(int id)
        {
            var pracownik = bazaMatryca.Pracownicy.Find(id);
            var status = bazaMatryca.Status.Find(pracownik.Status);

            var kierownik = bazaMatryca.Pracownicy.Find(pracownik.IdKierownika);
            string ImieKierownika = kierownik.Imie + " " + kierownik.Nazwisko;

            var nadrzedna = bazaMatryca.StrOrg.Find(pracownik.StrOrg.Id_Parent);
            string Nadrzedna = nadrzedna.Symb_Jedn;

            ViewBag.status = status.Nazwa.ToString();

            return Json(new
            {
                pracownik.Id_Pracownicy,
                pracownik.Imie,
                pracownik.Nazwisko,
                pracownik.Nr_Ewid,
                pracownik.APT,
                pracownik.DataZatr,
                pracownik.DataUmDo,
                pracownik.Stanowiska.Nazwa_Stan,
                pracownik.StrOrg.Symb_Jedn,
                ImieKierownika,
                status.Nazwa,
                Nadrzedna,

            }

                );
        }

        public string getStatusEdycja(int? id)
        {
            var pracownik = bazaMatryca.Pracownicy.Find(id);

            string opt = "<option";
            string optt = "</option>";
            string val = "value=\"";

            return opt + "selected>" + pracownik.Status.ToString();

        }
    }
}