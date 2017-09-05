using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

namespace HRRcp.Controls.Reports
{
    public partial class cntFieldParams : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string PrepareSql(string sql)
        {
            // podmiana UserId itp

            return sql;
        }
        //-----------------------------
        public void SetData(object rpDataItem)
        {
            if (rpDataItem != null)
            {
                DataRowView drv = (DataRowView)rpDataItem;
                Tools.SelectItem(Typ, drv["Typ"]);
                Tools.SetData(tbFieldParams, drv);
                SetFieldsVisible();
            }
        }

        public void GetData(IOrderedDictionary values)
        {
            Tools.GetData(tbFieldParams, values);
        }
        //-----------------------
        protected string IsTyp(params int[] typy)
        {
            int typ = Tools.StrToInt(Typ.SelectedValue, -1);
            return typy.Any(a => a == typ) ? "true" : "false";
        }

        protected bool IsTyp1(params int[] typy)
        {
            int typ = Tools.StrToInt(Typ.SelectedValue, -1);
            return typy.Any(a => a == typ);
        }





        protected bool IsSql(ref string sql)
        {
            string s = Tools.Substring(sql.TrimStart(), 0, 10).ToLower();
            if (s.StartsWith("sql:"))
            {
                sql = sql.TrimStart().Substring(4);
                return true;
            }
            else
                return sql.StartsWith("select") || sql.StartsWith("declare");
        }

        protected string GetInitValue(DataRowView drv)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                return db.getValue(dr, 0);
            }
            else
                return val;
            /*
            cachować w ViewState lub znacznik czy ustawione, 
            druga wartość dla Range
            */
        }

        public static bool Equals(string s, params string[] par)
        {
            return par.Any(a => a == s);
        }

        protected Boolean GetInitValueBool(DataRowView drv)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                return db.getBool(dr, 0, false);
            }
            else
            {
                string v = val.ToLower();
                return Equals(v, "1", "true", "tak", "t", "yes", "y");
            }
        }

        protected DateTime? GetInitValueDT(DataRowView drv)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                return db.getDateTime(dr, 0);
            }
            else
                return Tools.StrToDateTime(val);
        }

        protected void GetInitValueDT(DataRowView drv, DateEdit de1, DateEdit de2)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                de1.Date = db.getDateTime(dr, 0);
                if (dr.ItemArray.Length >= 1)
                    de2.Date = db.getDateTime(dr, 1);
            }
        }


        //------------------------
        private void SetFieldsVisible()
        {
            int typ = Tools.StrToInt(Typ.SelectedValue, -1);
            //trLookupSql.Visible = Tools.IsAny(typ, cntField.tDropDownList, cntField.tDropDownEdit);
            trLookupSql.Visible = typ.IsAny(cntField._tDropDownList, cntField.tDropDownEdit, cntField.tMultiSelect);

        }

        protected void Typ_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFieldsVisible();
        }
        //-----



        //--------------------------
        public int _Typ
        {
            set 
            {
                ViewState["typ"] = value;
            }
            get { return Tools.GetInt(ViewState["typ"], 1); }
        }

        [Bindable(true, BindingDirection.OneWay)]
        public object Data
        {
            set { SetData(value); }
        }
    }
}