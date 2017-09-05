using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.BadaniaWstepne.Templates
{
    public interface ICrTemplateCreator
    {
        IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args);
        IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args);
        IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args);
        void Updating(CrTemplateCreator.UpdInsArgsCnt Args, IDictionary Values);
        void Updated(CrTemplateCreator.UpdInsArgsCnt Args, ListViewUpdatedEventArgs e);
        void Inserting(CrTemplateCreator.UpdInsArgsCnt Args, IDictionary Values);
        void Inserted(CrTemplateCreator.UpdInsArgsCnt Args, DataRow Item);
        void Deleting(CrTemplateCreator.UpdInsArgsCnt Args, DataRow Item);
        void Deleted(CrTemplateCreator.UpdInsArgsCnt Args, DataRow Item);
        void SetControlValue(CrTemplateCreator.UpdInsArgsCnt Args, DataRowView Item);
        string GetFilterValue(Control container, CrTemplateArgs Args, int Index);
        string Validate(Control container, CrTemplateArgs Args, int Index);
        bool IsEmpty(Control container, CrTemplateArgs Args, int Index);
    }
    public interface ICrTemplateCreator<T> : ICrTemplateCreator
    {
        T GetValue(Control container, CrTemplateArgs Args, int Index);
        void SetValue(Control container, CrTemplateArgs Args, int Index, T Value);
        T GetValueReadOnly(Control container, CrTemplateArgs Args, int Index);
        void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, T Value);
    }
}
