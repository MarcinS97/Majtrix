using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HRRcp.BadaniaWstepne.Templates
{
    public abstract class CrTemplate 
    {
        public CrTemplateArgs Args { get; protected set; }
        public IndexedBasicTemplate Template { get; protected set; }
        public IndexedBasicTemplate ValidatorTemplate { get; protected set; }
        public Func<Control, object> ValueGetter;
        public Func<Control, string> FilterValueGetter;
        public Action<Control, object> ValueSetter;
        public Func<Control, string> ValidatorFunc;
        public Func<Control, bool> IsEmptyFunc;
        public bool isReadOnly { get; private set; }
        public ICrTemplateCreator Creator { get; private set; }

        public CrTemplate(IndexedBasicTemplate Template, IndexedBasicTemplate ValidatorTemplate, CrTemplateArgs Args, bool isReadOnly, ICrTemplateCreator Creator)
        {
            this.Template = Template;
            this.ValidatorTemplate = ValidatorTemplate;
            this.Args = Args;
            this.isReadOnly = isReadOnly;
            this.Creator = Creator;
        }
    }

    public class CrTemplate<T> : CrTemplate
    {
        new public ICrTemplateCreator<T> Creator { get; private set; }
        public CrTemplate(ICrTemplateCreator<T> Creator, CrTemplateArgs Args, int Index, Func<string, Control> LoadCnt, bool isReadOnly)
            : base(new IndexedBasicTemplate(LoadCnt, Index), new IndexedBasicTemplate(LoadCnt, Index), Args, isReadOnly, Creator)
        {
            Template.Create += (isReadOnly) ? Creator.CreateTemplateReadOnly(Args) : Creator.CreateTemplate(Args);
            ValidatorTemplate.Create += Creator.CreateValidator(Args);
            FilterValueGetter = a => Creator.GetFilterValue(a, Args, Index);
            ValueGetter = (isReadOnly) ? (Func<Control, object>)(a => Creator.GetValueReadOnly(a, Args, Index))
                : (Func<Control, object>)(a => Creator.GetValue(a, Args, Index));
            ValueSetter = (isReadOnly) ? (Action<Control, object>)((a, b) => Creator.SetValueReadOnly(a, Args, Index, (T)b))
                : (Action<Control, object>)((a, b) => Creator.SetValue(a, Args, Index, (T)b));
            ValidatorFunc = a => Creator.Validate(a, Args, Index);
            this.IsEmptyFunc = a => Creator.IsEmpty(a, Args, Index);
            this.Creator = Creator;
        }

        public string Validate(Control container)
        {
            return base.ValidatorFunc(container);
        }

        public string GetFilterValue(Control container)
        {
            return base.FilterValueGetter(container);
        }

        public T GetValue(Control container)
        {
            return (T)base.ValueGetter(container);
        }

        public void SetValue(Control container, T Value)
        {
            base.ValueSetter(container, Value);
        }

    }
}