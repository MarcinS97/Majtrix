using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRRcp.Areas.ME.Models.Interfaces
{
    public interface IOcenaValue
    {
        double getWartoscOceny(Oceny[] oceny, int Integer = 0, int Typ = 0);
        Dictionary<int, double> getWartoscOceny(Models.Oceny[] oceny, Pracownicy[] pracownicy, int Integer = 0, int Typ = 0);
    }
}
