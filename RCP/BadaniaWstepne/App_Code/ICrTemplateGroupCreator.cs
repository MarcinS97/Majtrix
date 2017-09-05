using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRRcp.BadaniaWstepne.Templates
{
    public interface ICrTemplateGroupCreator
    {
        void Filtering(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values);
        void IsEmpty(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values);
        string OrderBy(CrColumn Column, CrTemplate Template);
        void Creating(IList<string> Columns, IList<string> Joins);
        void Creating2(IList<string> Columns, IList<string> Joins, string header);
        //        void Creating2(IList<string> Columns, IList<string> Joins, CrColumn col);  // T: dodaje nagłowki jak w tabeli
    }
}
