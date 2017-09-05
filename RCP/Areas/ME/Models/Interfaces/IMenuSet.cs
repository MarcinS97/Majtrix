using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRRcp.Areas.ME.Models.Interfaces
{
    public interface IMenuSet
    {
        bool setMenu();

        IEnumerable<object> getMenuSet();

    }
}
