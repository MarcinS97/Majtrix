using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace HRRcp.BadaniaWstepne.Templates
{
    public class BasicTemplate : ITemplate
    {
        Func<string, Control> LoadCnt;
        public delegate void DCreateFunc(Control container, Func<string, Control> LoadCnt);
        public event DCreateFunc Create;

        public BasicTemplate(Func<string, Control> LoadCnt)
        {
            this.LoadCnt = LoadCnt;
        }

        public void InstantiateIn(Control container)
        {
            if(Create != null)
                Create(container, LoadCnt);
        }
    }
    public class IndexedBasicTemplate : ITemplate
    {
        int Index;
        Func<string, Control> LoadCnt;
        public delegate void DCreateFunc(Control container, Func<string, Control> LoadCnt, int Index);
        public event DCreateFunc Create;

        public IndexedBasicTemplate(Func<string, Control> LoadCnt, int Index)
        {
            this.LoadCnt = LoadCnt;
            this.Index = Index;
        }

        public void InstantiateIn(Control container)
        {
            if (Create != null)
                Create(container, LoadCnt, Index);
        }
    }
    public class HtmlTableCellTh : HtmlTableCell
    {
        public override string TagName
        {
            get
            {
                return "th";
            }
        }
    }
}