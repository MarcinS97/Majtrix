using HRRcp.Areas.ME.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.Areas.ME.Models.CustomModels.Filtr
{
    public class FiltrujPracownikow : IFiltrujPracownikow
    {
        private IWyszukajPracownikow wyszukajPracownikow;

        public FiltrujPracownikow(IWyszukajPracownikow Param)
        {
            wyszukajPracownikow = Param;
        }
        public IEnumerable<Pracownik> Filtr(IEnumerable<Pracownik> pracownicy, string nazwaParam = null, int status = -10)
        {
            List<Pracownik> lista = (List<Pracownik>)wyszukajPracownikow.getByNazwa(pracownicy, nazwaParam);
            if(status != -10)
            {

            }
            return lista;
        }
    }
}