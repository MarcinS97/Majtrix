using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.BadaniaWstepne.Templates
{
    public class CrTemplateArgs
    {
        public object[] Args { get; protected set; }
        public object[] DataSources { get; protected set; }
        public object this[int index]
        {
            get { return (Args == null || Args.Length <= index || index < 0) ? null : Args[index]; }
            set { Args[index] = value; }
        }
        public CrTemplateArgs(CrTemplateArgs Args)
        {
            this.Args = Args.Args;
            this.DataSources = Args.DataSources;
        }
        public CrTemplateArgs(object[] DataSources, object[] Args)
        {
            this.DataSources = DataSources;
            this.Args = Args;
        }
        public CrTemplateArgs(int dsn, int argsn)
        {
            this.DataSources = new object[dsn];
            this.Args = new object[argsn];
        }
        public static CrTemplateArgs Empty { get; private set; } 
        static CrTemplateArgs()
        {
            Empty = new CrTemplateArgs(null, null);
        }
    }
    public class CrTA1_TextBox : CrTemplateArgs
    {
        public int MaxLength
        {
            get { return (int)(base[0] ?? 0); }
            set { base[0] = value; }
        }
        public string AllowedChars
        {
            get { return (string)base[1]; }
            set { base[1] = value; }
        }
        public CrTA1_TextBox()
            : base(0, 2) { }
        public CrTA1_TextBox(int dsn, int argsn)
            : base(dsn, argsn) { }
        public CrTA1_TextBox(CrTemplateArgs Args)
            : base(Args) { }
    }
    public class CrTA2_DDLTextBox : CrTA1_TextBox
    {
        public object DataSource
        {
            get { return base.DataSources[0]; }
            set { base.DataSources[0] = value; }
        }
        /// <summary>
        /// string dla DefaultValue, int dla index'u,
        /// null dla pierwszej wartosci
        /// </summary>
        public object DefValue
        {
            get { return (base[2] ?? 0); }
            set { base[2] = value; }
        }
        public bool canNull
        {
            get { return (bool)(base[3] ?? 0); }
            set { base[3] = value; }
        }
        public CrTA2_DDLTextBox()
            : base(1, 4) { canNull = true; }
        public CrTA2_DDLTextBox(CrTemplateArgs Args)
            : base(Args) { canNull = true; }
    }
    public class CrTA3_MultiLineTextBox : CrTA1_TextBox
    {
        public int MaxRows
        {
            get { return (int)(base[2] ?? 5); }
            set { base[2] = value; }
        }
        public CrTA3_MultiLineTextBox()
            : base(0, 3) { }
        public CrTA3_MultiLineTextBox(CrTemplateArgs Args)
            : base(Args) { }
    }
    public class CrTA4_DataLabel : CrTemplateArgs
    {
        public string Sql
        {
            get { return (string)(base[0] ?? 0); }
            set { base[0] = value; }
        }
        public string Seperator
        {
            get { return (string)(base[1] ?? 0); }
            set { base[1] = value; }
        }
        public CrTA4_DataLabel()
            : base(0, 2) { }
    }
    public class CrTA5_AFileUpload : CrTemplateArgs
    {
        public string Path
        {
            get { return (string)(base[0] ?? 0); }
            set { base[0] = value; }
        }
        public CrTA5_AFileUpload()
            : base(0, 1) { }
    }
    public class CrTA6_DDL : CrTemplateArgs
    {
        public object DataSource
        {
            get { return base.DataSources[0]; }
            set { base.DataSources[0] = value; }
        }
        public string SqlName
        {
            get { return (string)(base[0] ?? 0); }
            set { base[0] = value; }
        }
        /// <summary>
        /// string dla DefaultValue, int dla index'u,
        /// null dla pierwszej wartosci
        /// </summary>
        public object DefValue
        {
            get { return (base[1] ?? 0); }
            set { base[1] = value; }
        }
        public bool canNull
        {
            get { return (bool)(base[2] ?? 0); }
            set { base[2] = value; }
        }
        public CrTA6_DDL()
            : base(1, 3) { canNull = true; }
        public CrTA6_DDL(CrTemplateArgs Args)
            : base(Args) { canNull = true; }
    }
}