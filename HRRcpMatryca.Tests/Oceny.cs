using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using HRRcp.Areas.ME.Models.CustomModels;
using HRRcp.Areas.ME.Models.Interfaces;
using HRRcp.Areas.ME.Models;
using HRRcp.Areas.ME.Models.CustomModels.Oceny;
using System.Collections.Generic;

namespace HRRcpMatryca.Tests
{
    [TestClass]
    public class Oceny
    {
        private HRRcp.Areas.ME.Models.Oceny[] oceny =
        {
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 3, Id_Pracownicy = 2 },
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 5, Id_Pracownicy = 2 },
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 1, Id_Pracownicy = 2 },
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 2, Id_Pracownicy = 3 },
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 0, Id_Pracownicy = 3 },
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 6, Id_Pracownicy = 3 },
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 4, Id_Pracownicy = 3 },
            new HRRcp.Areas.ME.Models.Oceny() {Wartosc = 5, Id_Pracownicy = 1 }
        };



        //Pracownik 1: 5
        //Pracownik 2: 9
        //Pracownik 3: 12

        [TestMethod]
        public void Sum_Oceny()
        {

            //przygotowanie
            var target = new WartoscOceny();

            //działanie
            var result = target.getWartoscOceny(oceny);

            //wynik
            Assert.AreEqual(oceny.Sum(w => w.Wartosc), result);


        }




        private Pracownicy[] pracownicy =
        {
            new Pracownicy() {Id_Pracownicy = 1  },
            new Pracownicy() {Id_Pracownicy = 2  },
            new Pracownicy() {Id_Pracownicy = 3  }
        };

        [TestMethod]
        public void Sum_Oceny_From_Pracownicy()
        {

            //przygotowanie
            var target = new WartoscOceny();

            var expected = new Dictionary<int, double>();
            expected.Add(1, 5);
            expected.Add(2, 9);
            expected.Add(3, 12);



            //działanie
            var result = target.getWartoscOceny(oceny,pracownicy);

            //wynik
            foreach (var item in result)
            {
                Assert.AreEqual(expected[item.Key], item.Value);
            }


        }

        [TestMethod]
        public void Sum_Oceny_From_Pracownicy_WithParams()
        {

            //przygotowanie
            var target = new WartoscOceny();

            var expected = new Dictionary<int, double>();
            expected.Add(1, 10);
            expected.Add(2, 18);
            expected.Add(3, 24);



            //działanie MNOŻENIE
            var result = target.getWartoscOceny(oceny, pracownicy, 2, 2);

            //wynik
            foreach (var item in result)
            {
                Assert.AreEqual(expected[item.Key], item.Value);
            }


        }
    }
}
