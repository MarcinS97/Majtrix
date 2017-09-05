using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.BadaniaWstepne.Templates
{
    public class CrColumnCreator
    {
        public string Header { get; private set; }
        public string SqlName { get; private set; }
        public CrTemplateGroupCreator TGroup { get; private set; }
        public int Sort { get; private set; }
        public int RightId { get; private set; }
        public string cssClass { get; private set; }
        public CrColumnCreator(int Sort, int RightId, string Header, string SqlName, CrTemplateGroupCreator TGroup)
        {
            this.Header = Header;
            this.SqlName = SqlName;
            this.TGroup = TGroup;
            this.Sort = Sort;
            this.RightId = RightId;
        }
        public CrColumnCreator(int Sort, int RightId, string Header, string SqlName, CrTemplateGroupCreator TGroup, string cssClass)
            : this(Sort, RightId, Header, SqlName, TGroup)
        {
            this.cssClass = cssClass;
        }

        public CrColumn Compile(CrTemplateGroupCreator.AccessTypes AccessType)
        {
            return new CrColumn(this, AccessType);
        }
    }
}