using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.BadaniaWstepne.Templates
{
    public class CrColumn
    {
        public string Header { get; private set; }
        public string SqlName { get; private set; }
        public int Sort { get; private set; }
        public int RightId { get; private set; }
        public CrTemplateGroup TGroup { get; private set; }
        public CrTemplateGroupCreator.AccessTypes AccessType { get; private set; }
        public Dictionary<CrTemplateGroupCreator.TemplateTypes, string> Css { get; private set; }

        public CrColumn(CrColumnCreator Creator, CrTemplateGroupCreator.AccessTypes AccessType)
        {
            this.Header = Creator.Header;
            this.SqlName = Creator.SqlName;
            this.Sort = Creator.Sort;
            this.RightId = Creator.RightId;
            this.TGroup = Creator.TGroup.Compile(Sort, AccessType);
            this.AccessType = AccessType;
            this.Css = new CrTemplateGroupCreator.TemplateTypes[] {
                CrTemplateGroupCreator.TemplateTypes.Item,
                CrTemplateGroupCreator.TemplateTypes.Edit,
                CrTemplateGroupCreator.TemplateTypes.Insert,
                CrTemplateGroupCreator.TemplateTypes.Filter
            }.ToDictionary(a => a, a => Creator.cssClass);
        }
        
    }
}