using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HRRcp.BadaniaWstepne.Templates
{
    public class CrTemplateGroup
    {
        public delegate CrTemplate DTGC(Func<string, Control> LoadControl);
        protected Dictionary<CrTemplateGroupCreator.TemplateTypes, DTGC> Templates;
        public ICrTemplateGroupCreator Creator { get; private set; }

        public CrTemplateGroup(ICrTemplateGroupCreator Creator, Dictionary<CrTemplateGroupCreator.TemplateTypes, DTGC> Templates)
        {
            this.Templates = Templates;
            this.Creator = Creator;
        }

        public bool ContainsKey(CrTemplateGroupCreator.TemplateTypes key)
        {
            return Templates.ContainsKey(key);
        }

        public DTGC this[CrTemplateGroupCreator.TemplateTypes index]
        {
            get { return Templates[index]; }
        }
    }

}