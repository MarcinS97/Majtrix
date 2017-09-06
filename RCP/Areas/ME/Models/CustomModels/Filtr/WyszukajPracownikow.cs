using HRRcp.Areas.ME.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.Areas.ME.Models.CustomModels.Filtr
{
    public class WyszukajPracownikow : IWyszukajPracownikow
    {
        MatrycaMVC bazaMatryca = new MatrycaMVC();
        public IEnumerable<Pracownik> getByNazwa(IEnumerable<Pracownik> pracownicyParam, string nazwaParam)
        {
            string[] imie = nazwaParam.Split(' ');
            List<Pracownik> lista = (List < Pracownik > )pracownicyParam.Where(m => m.Imie.Contains(imie[0]));

            return lista;

        }
    }
}