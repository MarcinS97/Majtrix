using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HRRcp.BadaniaWstepne.Templates
{
    public class CrColumnsCollection : List<CrColumn>
    {
        public struct CrColTemplKVP
        {
            public CrColumn Column;
            public CrTemplate Template;
            public IndexedBasicTemplate ValidatorTemplate;
            public CrColTemplKVP(CrColumn Column, CrTemplate Template, IndexedBasicTemplate ValidatorTemplate)
            {
                this.Column = Column;
                this.Template = Template;
                this.ValidatorTemplate = ValidatorTemplate;
            }
        };
        public string[] SqlAddColumns { get; private set; }
        public CrColumnsCollection() { }
        public CrColumnsCollection(string[] SqlAddColumns)
        {
            this.SqlAddColumns = SqlAddColumns;
        }
        public IEnumerable<CrColTemplKVP> getColumns(CrTemplateGroupCreator.TemplateTypes TemplateType, Func<string, Control> LoadControl)
        {
            return this
                .Where(a => a.TGroup.ContainsKey(TemplateType))
                .Select(a => {
                    var t = a.TGroup[TemplateType](LoadControl);
                    IndexedBasicTemplate bt = (t != null) ? t.ValidatorTemplate : null;
                    return new CrColTemplKVP(a, t, bt);
                })
                .Where(a => a.Template != null)
                .OrderBy(a => a.Column.Sort);
        }
        public IEnumerable<CrTemplateCreator.UpdInsArgsCnt> getColumnsArgs(CrTemplateGroupCreator.TemplateTypes TemplateType, Func<string, Control> LoadControl, Control container)
        {
            return getColumns(TemplateType, LoadControl).Select(a => new CrTemplateCreator.UpdInsArgsCnt(a.Column, a.Template, container));
        }

        public CrColumn GetByRight(int rightId)
        {
            var c = this.Where(a => a.RightId == rightId).ToArray();
            if (c.Count() == 0)
                return null;
            else
                return c[0];
        }
    }
}