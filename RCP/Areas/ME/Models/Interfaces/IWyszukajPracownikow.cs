using HRRcp.Areas.ME.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRRcp.Areas.ME.Models.Interfaces
{
    public interface IWyszukajPracownikow
    {
        IEnumerable<Pracownik> getByNazwa(IEnumerable<Pracownik> pracownicy, string nazwaParam);
    }
}
