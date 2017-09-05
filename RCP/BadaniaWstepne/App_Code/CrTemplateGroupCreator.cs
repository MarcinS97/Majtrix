using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.BadaniaWstepne.Templates
{
    public class CrTemplateGroupCreator : ICrTemplateGroupCreator
    {
        public enum TemplateTypes : int
        {
            Undefined = default(int),
            Item = 1,
            Edit = 2,
            Insert = 3,
            Filter = 4
        }
        public enum AccessTypes : int
        {
            Hide = 0,
            Read = 1,
            Write = 2,
            WriteReq = 3
        }
        public static string[] BadWstRightStrs = new string[] { "-", "pokaż", "edycja", "wymagane" };

        public delegate CrTemplate DCrTGC(int Index, Func<string, Control> LoadControl, AccessTypes AcType);
        protected Dictionary<TemplateTypes, DCrTGC> Templates;
        
        protected void AddTemplate<T>(CrTemplateCreator<T> TC, TemplateTypes TemplType, CrTemplateArgs Args)
        {
            Templates.Add(TemplType, (a, b, c) =>
                {
                    var AT = TemplatesPredicate[TemplType](c);
                    return (!AT.CanSee ? null : TC.Compile(Args, a, b, !AT.CanEdit));
                });
        }

        public CrTemplateGroupCreator(Dictionary<TemplateTypes, DCrTGC> Templates)
        {
            this.Templates = Templates;
        }

        public DCrTGC this[TemplateTypes index]
        {
            get { return Templates[index]; }
        }

        public CrTemplateGroup Compile(int Index, AccessTypes AcType)
        {
            return new CrTemplateGroup(this, Templates.ToDictionary(a => a.Key, a => 
                (CrTemplateGroup.DTGC)(b => a.Value(Index, b, AcType))));
        }

        public virtual void Filtering(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values)
        {
            string Column = Args.Column.SqlName;
            object Value = Args.Template.ValueGetter(Args.Container);
            if (Value != null && (!(Value is string) || !string.IsNullOrEmpty((string)Value)))
                Values.Add(string.Format("({0} = {1})", Column, CrTemplateCreator.ToSqlValue(Value)));
        }

        public virtual void IsEmpty(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values)
        {
            Values.Add(string.Format("{0} IS NULL", Args.Column.SqlName));
        }

        public virtual void Creating(IList<string> Columns, IList<string> Joins) { }
        public virtual void Creating2(IList<string> Columns, IList<string> Joins, string header) { }
        //public virtual void Creating(IList<string> Columns, IList<string> Joins, CrColumn col) { }
        
        public virtual string OrderBy(CrColumn Column, CrTemplate Template)
        {
            return Column.SqlName;
        
        }
        public virtual string Validate(CrColumn Column, CrTemplate Template)
        {
            return null;
        }
        
        #region static
        public struct TemplPredKVP
        {
            public bool CanSee;
            public bool CanEdit;
            public bool ReqField;
            public TemplPredKVP(AccessTypes AccessType, Predicate<AccessTypes> CanSee, Predicate<AccessTypes> CanEdit)
            {
                this.CanSee = CanSee(AccessType);
                this.CanEdit = CanEdit(AccessType);
                this.ReqField = AccessType == AccessTypes.WriteReq;
            }
        };
        public static Dictionary<TemplateTypes, Func<AccessTypes, TemplPredKVP>> TemplatesPredicate { get; private set; }

        static CrTemplateGroupCreator()
        {
            TemplatesPredicate = new Dictionary<TemplateTypes, Func<AccessTypes, TemplPredKVP>>()
            {
                { TemplateTypes.Item, b => new TemplPredKVP(b, a => a != AccessTypes.Hide, a => false) },
                { TemplateTypes.Edit, b => new TemplPredKVP(b, a => a != AccessTypes.Hide,  a => a >= AccessTypes.Write) },    //asdf
                //{ TemplateTypes.Edit,   b => new TemplPredKVP(b, a => a >= AccessTypes.Write, a => a >= AccessTypes.Write) },    //asdf testy

                { TemplateTypes.Insert, b => new TemplPredKVP(b, a => a >= AccessTypes.Write, a => true) },
                { TemplateTypes.Filter, b => new TemplPredKVP(b, a => a != AccessTypes.Hide, a => true) }
            };
        }
        #endregion
    }


    //--------------------------------------------
    public class CrTG0_TextBoxAll : CrTemplateGroupCreator
    {
        public CrTG0_TextBoxAll(int MaxLength, bool EditOnly)
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            CrTA1_TextBox ENFArgs = new CrTA1_TextBox();
            ENFArgs.MaxLength = MaxLength;
            if (!EditOnly)
                AddTemplate(CrTemplateCreator.CrTC0_Label, TemplateTypes.Item, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC1_TextBox, TemplateTypes.Edit, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC1_TextBox, TemplateTypes.Insert, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC1_TextBox, TemplateTypes.Filter, ENFArgs);
        }

        public CrTG0_TextBoxAll(int MaxLength)
            : this(MaxLength, false) { }
    }

    public class CrTG1_TextBoxNum : CrTemplateGroupCreator
    {
        public CrTG1_TextBoxNum(int MaxLength, bool canNegative, bool canFloat, bool EditOnly)
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            CrTA1_TextBox ENFArgs = new CrTA1_TextBox();
            ENFArgs.MaxLength = MaxLength;
            ENFArgs.AllowedChars = "0123456789";
            if (canNegative) ENFArgs.AllowedChars += "-";
            if (canFloat) ENFArgs.AllowedChars += ".,";
            if (!EditOnly)
                AddTemplate(CrTemplateCreator.CrTC0_Label, TemplateTypes.Item, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC1_TextBox, TemplateTypes.Edit, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC1_TextBox, TemplateTypes.Insert, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC1_TextBox, TemplateTypes.Filter, ENFArgs);
        }

        public CrTG1_TextBoxNum(int MaxLength, bool canNegative, bool canFloat)
            : this(MaxLength, canNegative, canFloat, false) { }
    }

    public class CrTG2_MultiLineTB : CrTemplateGroupCreator
    {
        public CrTG2_MultiLineTB(int MaxLength, int MaxRows)
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            CrTA3_MultiLineTextBox ENFArgs = new CrTA3_MultiLineTextBox();
            ENFArgs.MaxLength = MaxLength;
            ENFArgs.MaxRows = MaxRows;

            AddTemplate(CrTemplateCreator.CrTC0_Label, TemplateTypes.Item, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC5_MultiLineTextBox, TemplateTypes.Edit, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC5_MultiLineTextBox, TemplateTypes.Insert, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC5_MultiLineTextBox, TemplateTypes.Filter, ENFArgs); // TODO
        }

        public override void Filtering(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values)      // dla wszystkich multiline
        {
            //base.Filtering(Args, Values);

            string Column = Args.Column.SqlName;
            object Value = Args.Template.ValueGetter(Args.Container);
            if (Value != null && (!(Value is string) || !string.IsNullOrEmpty((string)Value)))
                Values.Add(string.Format("({0} like '%{1}%')", Column, Value));
        }
    }

    public class CrTG3_CheckBox : CrTemplateGroupCreator
    {
        public CrTG3_CheckBox()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            AddTemplate(CrTemplateCreator.CrTC6_CheckBox, TemplateTypes.Item, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC6_CheckBox, TemplateTypes.Edit, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC6_CheckBox, TemplateTypes.Insert, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC9_DDLBOOL, TemplateTypes.Filter, CrTemplateArgs.Empty);
        }

        public override void IsEmpty(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values) { }
    }
    
    [Obsolete]
    public class CrTG4_Class : CrTemplateGroupCreator
    {
        public CrTG4_Class()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            CrTemplateArgs ENFArgs = new CrTemplateArgs(1, 1);
            ENFArgs.DataSources[0] = db.getDataSet(@"select id [Value], Nazwa [Text] from Kody where Typ = 'PRACGRUPA' order by Lp").Tables[0].Rows.Cast<DataRow>();
            ENFArgs.Args[0] = "KodText";
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Item, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Edit, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Insert, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Filter, ENFArgs); // TODO
        }
        
        public override void Creating(IList<string> Columns, IList<string> Joins)
        {
            Columns.Add("Kody.Nazwa as KodText");
            Joins.Add("LEFT JOIN Kody on Kody.id = Main.Class");
        }

        //public override void Creating2(IList<string> Columns, IList<string> Joins, CrColumn col)
        //{
        //    Columns.Add("Kody.Nazwa as " + col.Header);
        //    Joins.Add("LEFT JOIN Kody on Kody.id = Main.Class");
        //}




    }
    public class CrTG5_CC : CrTemplateGroupCreator
    {
        public CrTG5_CC()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            CrTA6_DDL FArgs = new CrTA6_DDL();
            CrTA4_DataLabel FArgs2 = new CrTA4_DataLabel();
            FArgs2.Sql =
//@"SELECT CC.cc + ' ' + CC.Nazwa + case when GETDATE() between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909') then '' else ' (zamknięte)' end FROM SplityWspB LEFT JOIN CC on CC.id = IdCC WHERE IdPrzypisania = {0}";
@"SELECT CC.cc FROM SplityWspB LEFT JOIN CC on CC.id = IdCC WHERE IdPrzypisania = {0}";
            FArgs2.Seperator = "<br />";
            FArgs.DataSource = new SqlDataSource(db.conStr,
                @"SELECT Id [Value], CC.cc + ' ' + CC.Nazwa + case when GETDATE() between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909') then '' else ' (zamknięte)' end [Text] FROM CC ORDER BY cc"); //db.getDataSet(
//@"SELECT Id [Value], CC.cc + ' ' + CC.Nazwa + case when GETDATE() between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909') then '' else ' (zamknięte)' end [Text] FROM CC ORDER BY cc").Tables[0].Rows;
            CrTemplateArgs ENArgs = new CrTemplateArgs(1, 0);
            AddTemplate(CrTemplateCreator.CrTC10_DataLabel, TemplateTypes.Item, FArgs2);
            AddTemplate(CrTemplateCreator.CrTC8_SplityWsp, TemplateTypes.Edit, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC8_SplityWsp, TemplateTypes.Insert, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Filter, FArgs); // TODO
        }

        public override void Creating(IList<string> Columns, IList<string> Joins)
        {
            Columns.Add("GSplityWspB.IdCC as CrTG5IdCC");
            Joins.Add("LEFT JOIN (SELECT MIN(IdCC) IdCC, IdPrzypisania FROM SplityWspB GROUP BY IdPrzypisania) GSplityWspB on GSplityWspB.IdPrzypisania = Main.id");
        }

        public override void Creating2(IList<string> Columns, IList<string> Joins, string header)
        {
            Columns.Add(String.Format(@"
ISNULL(STUFF((
select ', ' + CC.cc + case when W.Wsp != 1 then '(' + CONVERT(varchar, round(W.Wsp, 2, 0)) + ')' else '' end
		from SplityWspB W
		left join CC on CC.Id = W.IdCC
		where W.IdPrzypisania = Main.id
		order by W.Wsp desc
for XML PATH('')), 1, 2, ''), '') as [{0}]
                ", header));
        }

        public override void Filtering(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values)
        {
            string sql = "SELECT IdCC FROM SplityWspB WHERE Main.id = IdPrzypisania"; // TODO
            object v = Args.Template.ValueGetter(Args.Container);
            if (v != null)
            {
                string Value = CrTemplateCreator.ToSqlValue(v);
                Values.Add(string.Format("{0} IN ({1})", Value, sql));
            }
        }

        public override void IsEmpty(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values)
        {
            string sql = "SELECT IdPrzypisania FROM SplityWspB"; // TODO
            Values.Add(string.Format("Main.id NOT IN ({0})", sql));
        }

        public override string OrderBy(CrColumn Column, CrTemplate Template)
        {
            return "CrTG5IdCC";
        }
    }

    public class CrTG6_DateEdit : CrTemplateGroupCreator
    {
        public CrTG6_DateEdit()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            AddTemplate(CrTemplateCreator.CrTC4_DateEdit, TemplateTypes.Item, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC4_DateEdit, TemplateTypes.Edit, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC4_DateEdit, TemplateTypes.Insert, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC7_DateZakres, TemplateTypes.Filter, CrTemplateArgs.Empty);
        }
        public override void Filtering(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values)
        {
            string f = Args.Template.FilterValueGetter(Args.Container);
            if(!string.IsNullOrEmpty(f))
                Values.Add("(" + string.Format(f, Args.Column.SqlName) + ")");
        }
    }

    
    
    /*
    public class CrTG7_Stanowiska : CrTemplateGroupCreator
    {
        public CrTG7_Stanowiska()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            CrTA2_DDLTextBox FArgs = new CrTA2_DDLTextBox();
            FArgs.DataSource = db.getDataSet(@"select Id [Value], Nazwa [Text] from Stanowiska where Status >= 0 order by Nazwa").Tables[0].Rows;
            FArgs.MaxLength = 50;
            AddTemplate(CrTemplateCreator.CrTC0_Label, TemplateTypes.Item, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC3_DDLTextBox, TemplateTypes.Edit, FArgs);
            AddTemplate(CrTemplateCreator.CrTC3_DDLTextBox, TemplateTypes.Insert, FArgs);
            AddTemplate(CrTemplateCreator.CrTC3_DDLTextBox, TemplateTypes.Filter, FArgs);
        }
    }
    public class CrTG8_Przel : CrTemplateGroupCreator
    {
        public CrTG8_Przel()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            CrTemplateArgs ENFArgs = new CrTemplateArgs(1, 0);
            ENFArgs.DataSources[0] = db.getDataSet(@"select Id [Value], Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as [Text] from Pracownicy where Kierownik = 1 and Status >= 0 and KadryId < 80000 order by Nazwisko, Imie").Tables[0].Rows.Cast<DataRow>();
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Item, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Edit, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Insert, ENFArgs);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Filter, ENFArgs); // TODO
        }
        public override void Creating(IList<string> Columns, IList<string> Joins)
        {
            Columns.Add("Pracownicy.Nazwisko + ' ' + Pracownicy.Imie + ' (' + Pracownicy.KadryId + ')' as PrzelText");
            Joins.Add("LEFT JOIN Pracownicy on Main.PrzelId = Pracownicy.Id");
        }
    }
     */
    //------------------------------------
    public class CrTemplateGroupCreatorLookup : CrTemplateGroupCreator
    {
        protected string Columns;
        protected string Joins;

        public CrTemplateGroupCreatorLookup(Dictionary<TemplateTypes, DCrTGC> Templates) : base(Templates)
        {            
            //this.Templates = Templates;
        }

        public override void Creating(IList<string> Columns, IList<string> Joins)
        {
            if (!string.IsNullOrEmpty(this.Columns))
                Columns.Add(this.Columns);
            if (!string.IsNullOrEmpty(this.Joins))
                Joins.Add(this.Joins);
        }

        private string ChangeHeader(string h, string newH)
        {
            int p;
            string c = h.TrimEnd();
            if (c.EndsWith("]"))
            {
                p = c.LastIndexOf("[");
            }
            else
            {
                p = c.LastIndexOf(" ") + 1;  // zakładam, ze nazwy nie są w []
            }
            //int p = c.ToLower().IndexOf(" as ");
            return String.Format("{0}[{1}]", c.Remove(p), newH);
        }

        public override void Creating2(IList<string> Columns, IList<string> Joins, string header)
        {
            if (!string.IsNullOrEmpty(this.Columns))
            {
                string h = ChangeHeader(this.Columns, header);
                Columns.Add(h);
            }
            if (!string.IsNullOrEmpty(this.Joins))
                Joins.Add(this.Joins);
        }
    }

    public class CrTG7_DDLTextBox : CrTemplateGroupCreatorLookup
    {
        //string Columns;
        //string Joins;

        public CrTG7_DDLTextBox(string sql)
            : this(sql, null, null) { }

        public CrTG7_DDLTextBox(string sql, string Columns, string Joins)
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            this.Columns = Columns;
            this.Joins = Joins;
            CrTA2_DDLTextBox FArgs = new CrTA2_DDLTextBox();
            FArgs.DataSource = new SqlDataSource(db.conStr, sql);// db.getDataSet(sql).Tables[0].Rows;
            FArgs.MaxLength = 50;
            AddTemplate(CrTemplateCreator.CrTC0_Label, TemplateTypes.Item, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC3_DDLTextBox, TemplateTypes.Edit, FArgs);
            AddTemplate(CrTemplateCreator.CrTC3_DDLTextBox, TemplateTypes.Insert, FArgs);
            AddTemplate(CrTemplateCreator.CrTC3_DDLTextBox, TemplateTypes.Filter, FArgs);
        }
        //public override void Creating(IList<string> Columns, IList<string> Joins)
        //{
        //    if (!string.IsNullOrEmpty(this.Columns))
        //        Columns.Add(this.Columns);
        //    if (!string.IsNullOrEmpty(this.Joins))
        //        Joins.Add(this.Joins);
        //}
    }

    public class CrTG8_NullableDDL : CrTemplateGroupCreatorLookup
    {
        //string Columns;
        //string Joins;

        public CrTG8_NullableDDL(string sql)
            : this(sql, null, null, null, null) { }

        public CrTG8_NullableDDL(string sql, string SqlName, string Columns, string Joins, object DefValue)
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            this.Columns = Columns;
            this.Joins = Joins;
            CrTA6_DDL Args = new CrTA6_DDL();
            Args.DataSource = new SqlDataSource(db.conStr, sql); //db.getDataSet(sql).Tables[0].Rows.Cast<DataRow>();
            Args.SqlName = SqlName;
            Args.DefValue = DefValue;
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Item, Args);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Edit, Args);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Insert, Args);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Filter, Args);
        }
       
        //public override void Creating(IList<string> Columns, IList<string> Joins)
        //{
        //    if(!string.IsNullOrEmpty(this.Columns))
        //        Columns.Add(this.Columns);
        //    if (!string.IsNullOrEmpty(this.Joins))
        //        Joins.Add(this.Joins);
        //}
    }

    public class CrTG9_NazwImie : CrTemplateGroupCreator
    {
        public CrTG9_NazwImie()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            AddTemplate(CrTemplateCreator.CrTC0_Label, TemplateTypes.Item, CrTemplateArgs.Empty);
        }
        public override void Creating(IList<string> Columns, IList<string> Joins)
        {
            Columns.Add("ISNULL(Main.Nazwisko, ' ') + ' ' + ISNULL(Main.Imie, ' ') as CrTG9NazwImie");
        }
    }
    
    public class CrTG10_FileUpload : CrTemplateGroupCreator
    {
        public CrTG10_FileUpload(string path)
            :base(new Dictionary<TemplateTypes,DCrTGC>())
        {
            CrTA5_AFileUpload FuArgs = new CrTA5_AFileUpload();
            FuArgs.Path = path;
            AddTemplate(CrTemplateCreator.CrTC11_AsyncFileUpload, TemplateTypes.Item, FuArgs);
            AddTemplate(CrTemplateCreator.CrTC11_AsyncFileUpload, TemplateTypes.Edit, FuArgs);
            AddTemplate(CrTemplateCreator.CrTC11_AsyncFileUpload, TemplateTypes.Insert, FuArgs);
        }
    }
    
    public class CrTG11_DateEditZakr : CrTemplateGroupCreator
    {
        public CrTG11_DateEditZakr()
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            AddTemplate(CrTemplateCreator.CrTC12_DateEditZakr, TemplateTypes.Edit, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC12_DateEditZakr, TemplateTypes.Insert, CrTemplateArgs.Empty);
            AddTemplate(CrTemplateCreator.CrTC7_DateZakres, TemplateTypes.Filter, CrTemplateArgs.Empty);
        }
        public override void Filtering(CrTemplateCreator.UpdInsArgsCnt Args, IList<string> Values)
        {
            string f = Args.Template.FilterValueGetter(Args.Container);
            if (!string.IsNullOrEmpty(f))
                Values.Add("(" + string.Format(f, Args.Column.SqlName) + ")");
        }
    }
    
    public class CrTG12_DDL : CrTemplateGroupCreatorLookup
    {
        //string Columns;
        //string Joins;

        public CrTG12_DDL(string sql)
            : this(sql, null, null, null, null) { }

        public CrTG12_DDL(string sql, string SqlName, string Columns, string Joins, object DefValue)
            : base(new Dictionary<TemplateTypes, DCrTGC>())
        {
            this.Columns = Columns;
            this.Joins = Joins;
            CrTA6_DDL Args = new CrTA6_DDL();
            Args.DataSource = new SqlDataSource(db.conStr, sql);// db.getDataSet(sql).Tables[0].Rows.Cast<DataRow>();
            Args.SqlName = SqlName;
            Args.canNull = false;
            Args.DefValue = DefValue;


            CrTA6_DDL Args2 = new CrTA6_DDL();
            Args2.DataSource = new SqlDataSource(db.conStr, sql); //db.getDataSet(sql).Tables[0].Rows.Cast<DataRow>();
            Args2.SqlName = SqlName;
            Args2.canNull = true;
            Args2.DefValue = DefValue;


            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Item, Args);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Edit, Args);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Insert, Args);
            AddTemplate(CrTemplateCreator.CrTC2_DropDownList, TemplateTypes.Filter, Args2);  // filtr zawsze z wybierz...
        }
 
        //public override void Creating(IList<string> Columns, IList<string> Joins)
        //{
        //    if (!string.IsNullOrEmpty(this.Columns))
        //        Columns.Add(this.Columns);
        //    if (!string.IsNullOrEmpty(this.Joins))
        //        Joins.Add(this.Joins);
        //}
    }
}
