using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRRcp.Areas.ME.Models.CustomModels;

namespace HRRcp.Areas.ME.Models.Interfaces
{
    public interface IToDoGet
    {
        Pracownik getPracownik(int id);
        IEnumerable<Linie> getLinie();

        bool setToDo();
    }
}
