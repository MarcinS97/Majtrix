using HRRcp.Areas.ME.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRRcp.Areas.ME.Models.Interfaces
{
    public interface IFiltrujPracownikow
    {
        IEnumerable<Pracownik> Filtr(IEnumerable<Pracownik> pracownicy, string nazwaParam = null, int statusParam = -10);
    }
}
