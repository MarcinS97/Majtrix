using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HRRcp.Areas.ME.Models.Interfaces;
using System.Collections.Generic;
using HRRcp.Areas.ME.Models.CustomModels;
using HRRcp.Areas.ME.Models.CustomModels.Filtr;

namespace HRRcpMatryca.Tests
{
    [TestClass]
    public class Filtracja
    {
        List<Pracownik> pracownicy = new List<Pracownik>();

        [TestMethod]
        public void Filtracja_Przez_Nazwe()
        {

            //Przygotowanie
            Mock<IOcenaValue> mockOcena = new Mock<IOcenaValue>();
           
            pracownicy.Add(new Pracownik(1, mockOcena.Object));
            pracownicy.Add(new Pracownik(2, mockOcena.Object));
            pracownicy.Add(new Pracownik(3, mockOcena.Object));

            Mock<IWyszukajPracownikow> mockWyszukaj = new Mock<IWyszukajPracownikow>();
            var target = new FiltrujPracownikow(mockWyszukaj.Object);
            


            //Działanie
            var result = target.Filtr(pracownicy, "Marcin");
            int[] expected = { 1, 2 };

            int i = 0;
            //Wynik
            foreach (var item in result)
            {
                Assert.AreEqual(expected, item.Id_Pracownicy);
                    i++;
            }


        }
    }
}
