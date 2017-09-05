using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

//using System.Reflection;

namespace HRRcp.App_Code
{
    public static class db
    {
        public static string conStr = ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString;
        /*
#if PORTAL
        public static string conStr = ConfigurationManager.ConnectionStrings["PORTAL"].ConnectionString;
#else
        public static string conStr = ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString;
#endif
        */
        //public SqlConnection Fcon; nie można tak tego zrobić bo by to musiała być zmienna statyczna (bo funkcje statyczne), a ta jest 1 dla wszystkich 

#if PORTAL
        public static string PORTAL = ConfigurationManager.ConnectionStrings["PORTAL"].ConnectionString;
#else
        public static string PORTAL = null;     // dla kontrolek portalowych
#endif


        //-----------------------------------------------------------
        public const string NULL    = "null";
        public const string sTRUE   = "True";
        public const string sFALSE  = "False";
        public const string TRUE    = "1";
        public const string FALSE   = "0";

        public const string DateMinValueStr = "1900-01-01";     // tak jak w bazie danych
        public static DateTime DateMinValue = DateTime.Parse(DateMinValueStr);

        //-----------------------------------------------------------
        public static string GetDbName(string conStr)
        {
            var csb = new SqlConnectionStringBuilder(conStr);
            return csb.InitialCatalog;  // baza danych
        }

        public static string GetDbHost(string conStr)
        {
            var csb = new SqlConnectionStringBuilder(conStr);
            return csb.DataSource;    // serwer
        }

        //----- getValues -------------------------------------------
        public static string getValue(DataRow dr, int fieldNo)
        {
            if (dr != null)
                return dr[fieldNo].ToString();
            else
                return null;
        }

        public static string getValue(DataRow dr, string fieldName)
        {
            if (dr != null)
                return dr[fieldName].ToString();
            else
                return null;
        }

        public static string getValue(DataRow dr, string fieldName, string nullValue)
        {
            if (dr != null)
            {
                string v = dr[fieldName].ToString();
                if (!String.IsNullOrEmpty(v))
                    return v;
            }
            return nullValue;
        }
        //-----
        public static string getStr(object o)
        {
            if (!isNull(o))
                return o.ToString();
            else
                return null;
        }

        public static string getStr(object o, string def)
        {
            if (!isNull(o))
                return o.ToString();
            else
                return def;
        }

        //-----
        public static int getInt(DataRow dr, string fieldName, int nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToInt32(o);
            }
            else
                return nullValue;
        }

        public static int? getInt(DataRow dr, string fieldName)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (!o.Equals(DBNull.Value))
                    return Convert.ToInt32(o);
            }
            return null;
        }

        public static int? getInt(DataRow dr, int fieldIndex)
        {
            if (dr != null)
            {
                object o = dr[fieldIndex];
                if (!o.Equals(DBNull.Value))
                    return Convert.ToInt32(o);
            }
            return null;
        }

        public static int getInt(DataRow dr, int fieldIndex, int nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldIndex];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToInt32(o);
            }
            else
                return nullValue;
        }

        public static int getInt(Object o, int nullValue)
        {
            if (o != null)
            {
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToInt32(o);
            }
            else
                return nullValue;
        }
        //-----
        public static bool getBool(DataRow dr, string fieldName, bool nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToBoolean(o);
            }
            else
                return nullValue;
        }

        public static bool getBool(DataRow dr, int fieldIndex, bool nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldIndex];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToBoolean(o);
            }
            else
                return nullValue;
        }

        public static bool getBool(Object o, bool nullValue)
        {
            if (o != null)
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToBoolean(o);
            else
                return nullValue;
        }
        //-----
        public static double? getDouble(DataRow dr, string fieldName)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (!o.Equals(DBNull.Value))
                    return Convert.ToDouble(o);
            }
            return null;
        }

        public static double? getDouble(DataRow dr, int fieldIndex)
        {
            if (dr != null)
            {
                object o = dr[fieldIndex];
                if (!o.Equals(DBNull.Value))
                    return Convert.ToDouble(o);
            }
            return null;
        }

        public static double getDouble(DataRow dr, string fieldName, double nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToDouble(o);
            }
            else
                return nullValue;
        }

        public static double getDouble(DataRow dr, int fieldIndex, double nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldIndex];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToDouble(o);
            }
            else
                return nullValue;
        }
        //-----
        public static DateTime getDateTime(DataRow dr, string fieldName, DateTime nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToDateTime(o);
            }
            else
                return nullValue;
        }

        public static DateTime getDateTime(DataRow dr, int fieldIndex, DateTime nullValue)
        {
            if (dr != null)
            {
                object o = dr[fieldIndex];
                if (o.Equals(DBNull.Value))
                    return nullValue;
                else
                    return Convert.ToDateTime(o);
            }
            else
                return nullValue;
        }

        public static bool getDateTime(DataRow dr, string fieldName, out DateTime dt)
        {
            DateTime? dtn = getDateTime(dr, fieldName);
            if (dtn != null)
            {
                dt = (DateTime)dtn;
                return true;
            }
            else
            {
                dt = DateTime.MinValue;
                return false;
            }
        }

        public static DateTime? getDateTime(DataRow dr, string fieldName)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (!o.Equals(DBNull.Value))
                    try
                    {
                        return Convert.ToDateTime(o);
                    }
                    catch { }
            }
            return null;
        }

        public static DateTime? getDateTime(DataRow dr, int fieldIndex)
        {
            if (dr != null)
            {
                object o = dr[fieldIndex];
                if (!o.Equals(DBNull.Value))
                    try
                    {
                        return Convert.ToDateTime(o);
                    }
                    catch { }
            }
            return null;
        }

        public static DateTime getDateTime(object o, DateTime nullValue)
        {
            if (o == null || o.Equals(DBNull.Value))
                return nullValue;
            else
                return Convert.ToDateTime(o);
        }

        public static DateTime? getDateTime(object o)
        {
            if (o == null || o.Equals(DBNull.Value))
                return null;
            else
                return Convert.ToDateTime(o);
        }
        //-----
        public static string getFloatAsString(DataRow dr, string paramName, string nullValue)
        {
            if (dr != null)
            {
                string param = getValue(dr, paramName);
                if (!String.IsNullOrEmpty(param))
                    return param.Replace(',', '.');
            }
            return nullValue;
        }
        //-------------------------------------------------------
        public static bool isNull(object o)
        {
            if (o == null || o.Equals(DBNull.Value))
                return true;
            else
                return String.IsNullOrEmpty(o.ToString());
        }

        public static bool isNull(DataRow dr, string fieldName)
        {
            if (dr != null)
            {
                object o = dr[fieldName];
                if (o.Equals(DBNull.Value))
                    return true;
                else
                    return String.IsNullOrEmpty(o.ToString());
            }
            else
                return true;
        }

        public static bool isNull(DataRow dr, int index)
        {
            if (dr != null)
            {
                object o = dr[index];
                if (o.Equals(DBNull.Value))
                    return true;
                else
                    return String.IsNullOrEmpty(o.ToString());
            }
            else
                return true;
        }

        public static string ISNULL(string value, string nullValue)
        {
            if (String.IsNullOrEmpty(value))
                return nullValue;
            else
                return value;
        }

        public static int ISNULL(object value, int nullValue)
        {
            if (isNull(value))
                return nullValue;
            else
                return (int)value;
        }
        //-----------------------------------------------------------
        public static string sqlGetDate(string field)
        {
            //return "replace(convert(varchar," + field + ",111),'/','-')";       // yyyy/MM/DD
            return "left(convert(varchar," + field + ",20),10)";                  // yyyy-MM-DD
        }

        public static string sqlGetDateAsDateTime(string field)
        {
            return "convert(datetime," + field + ",20)";                          // yyyy-MM-DD
        }

        public static string sqlGetDateTimeHHMM(string field)
        {
            return "left(convert(varchar," + field + ",20),16)";                  // yyyy-MM-DD HH:MM
        }

        public static string sqlGetDateTimeHHMMAs(string field)
        {
            return "left(convert(varchar," + field + ",20),16) as " + field;      // yyyy-MM-DD HH:MM
        }

        public static string sqlCutTime(string field)  // odcina czas z daty, zwraca DateTime do wstawnienia w zapytanie sql dlatego na stringu                           
        {
            return "convert(datetime," + sqlGetDate(field) + ")";                // yyyy-MM-DD as SQL DateTime
        }

        public static string sqlCutTime2(string field)  // odcina czas z daty, zwraca DateTime do wstawnienia w zapytanie sql dlatego na stringu                           
        {
            return "CAST(FLOOR(CAST(" + field + " AS FLOAT)) AS DATETIME)";
        }

        public static string sqlGetDateAs(string field)
        {
            return sqlGetDate(field) + " as " + field;
        }

        public static string sqlRemovePL(string fieldName)                      // wiem ze straszne ... ale to tylko do importu danych wiec nie stanowi
        {
            return "REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(LOWER(" + fieldName +
                   "),'ą','a'),'ć','c'),'ę','e'),'ł','l'),'ń','n'),'ó','o'),'ś','s'),'ź','z'),'ż','z')";
        }

        public static string RemovePL(string pl)
        {
            char[] chars = pl.ToCharArray();
            for (int i = 0; i < pl.Length; i++)
            {
                int p = "ąćęłńóśźżĄĆĘŁŃÓŚŹŻ".IndexOf(chars[i]);
                if (p > -1)
                    chars[i] = "acelnoszzACELNOSZZ"[p];
            }
            return new string(chars);
        }

        //----- connection ------------------------------------------
        public static void DoConnect(ref SqlConnection c)
        {
            c = new SqlConnection(conStr);
            c.Open();
        }

        public static void DoDisconnect(ref SqlConnection c)
        {
            c.Close();
            c = null;
        }

        public static SqlConnection Connect()
        {
            if (App.Master.con == null)
                DoConnect(ref App.Master.con);
            return App.Master.con;
        }

        public static void Disconnect()
        {
            if (App.Master.con != null)
                DoDisconnect(ref App.Master.con);
        }

        public static SqlConnection con
        {
            get
            {
                if (App.Master.con == null)
                    DoConnect(ref App.Master.con);
                return App.Master.con;
            }
        }

        public static SqlConnection conP // PORTAL
        {
            get
            {
                if (App.Master.conP == null)
                    App.Master.conP = Connect(PORTAL);
                return App.Master.conP;
            }
        }

        //----
        public static SqlConnection Connect(string conStr)  
        {
            SqlConnection c = new SqlConnection(conStr);
                c.Open();
            return c;
        }

        public static void Disconnect(SqlConnection c)
        {
            if (c != null)
            {
                c.Close();
                c = null;
            }
        }
        //-----
        /*
        public static SqlConnection Connect2()
        {
            SqlConnection c = null;
            DoConnect(ref c);
            return c;
        }

        public static void Disconnect2(SqlConnection c)
        {
            DoDisconnect(ref c);
        }
        */
        //----- getDbData ---------------------------------
        public static string getScalar(string sql)
        {
            return getScalar(sql, true);
        }

        public static string getScalar(SqlConnection con, string sql)
        {
            return getScalar(con, sql, true);
        }

        public static string getScalar(SqlConnection con, string sql, bool logError)  // zwraca pierwsza wartosc 
        {
            string s;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                s = Convert.ToString(sqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                if (logError) Log.Error(Log.SQL, sql, ex.Message);
                throw;
            }
            return s;
        }

        public static string getScalar(string sql, bool logError)  // zwraca pierwsza wartosc 
        {
            return getScalar(con, sql, logError);
        }

        public static string getScalar(SqlTransaction tr, string sql)
        {
            string s;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, tr.Connection, tr);
                s = Convert.ToString(sqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Log.ErrorTr(Log.SQL, sql, ex.Message);
                throw;
            }
            return s;
        }

        public static Nullable<T> getScalar<T>(string conStr, string sql)
        where T : struct
        {
            SqlConnection c = Connect(conStr);
            DataRow dr = getDataRow(c, sql);
            Disconnect(c);
            return (dr == null) ? (Nullable<T>)null : (T)dr[0];
        }

        public static Nullable<T> getScalar<T>(string sql)
        where T : struct
        {
            DataRow dr = getDataRow(sql);
            return (dr == null) ? (Nullable<T>)null : (T)dr[0];
        }

        //---------------------
        public static DataSet getDataSet(string sql)
        {
            return getDataSet(con, sql);
        }

        public static DataSet getDataSet(SqlConnection con2, string sql)
        {
            DataSet ds;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, con2);


                sqlCmd.CommandTimeout = 300;
 
                
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                ds = new DataSet();    
                da.Fill(ds);    //<<<< jak da nie zwróci danych to Tables.Count = 0 - zastanowić się czy nie zwracać w takim przypadku null 
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sql, ex.Message);
                throw;
            }
            return ds;
        }

        public static DataSet getDataSet(SqlTransaction tr, string sql)
        {
            DataSet ds;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, tr.Connection, tr);
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                ds = new DataSet();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Log.ErrorTr(Log.SQL, sql, ex.Message);
                throw;
            }
            return ds;
        }

        public static DataRow getDataRow(string sql)
        {
            DataSet ds = getDataSet(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }

        public static DataRow getDataRow(SqlConnection con, string sql)
        {
            DataSet ds = getDataSet(con, sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }
        //----------------------------------------
        public static string selectScalar(string sql, params object[] par)
        {
            return db.getScalar(String.Format(sql, par));
        }

        public static DataRow selectRow(string sql, params object[] par)
        {
            return getDataRow(String.Format(sql, par));
        }

        public static DataSet select(string sql, params object[] par)
        {
            return getDataSet(String.Format(sql, par));
        }
        //----------------------------------------
        public static DataRowCollection getRows(DataSet ds)
        {
            return ds.Tables[0].Rows;
        }

        public static DataRow getRow(DataSet ds)
        {
            DataRowCollection drc = getRows(ds);
            if (drc.Count > 0)
                return drc[0];
            else
                return null;
        }

        public static DataRow getRow(DataSet ds, int index)
        {
            DataRowCollection drc = getRows(ds);
            if (index < drc.Count)
                return drc[index];
            else
                return null;
        }

        public static int getCount(DataSet ds)
        {
            return getRows(ds).Count;
        }
        //----------------------------------------
        public static bool execSQL(string sql)
        {
            int success = 0;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, con);

                sqlCmd.CommandTimeout = 300;
                
                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sql, ex.Message);
                throw;
            }
            return success >= 1;
        }

        public static bool execSQL(SqlConnection con2, string sql)
        {
            int success = 0;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, con2);
                
                sqlCmd.CommandTimeout = 300;

                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sql, ex.Message);
                throw;
            }
            return success >= 1;
        }

        public static bool execSQL(SqlTransaction tr, string sql)
        {
            int success = 0;
            try
            {
                SqlCommand sqlCmd;
                if (tr == null)
                    sqlCmd = new SqlCommand(sql, con);
                else
                    sqlCmd = new SqlCommand(sql, tr.Connection, tr);
                
                sqlCmd.CommandTimeout = 300;

                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
            }
            catch (Exception ex)
            {
                Log.ErrorTr(Log.SQL, sql, ex.Message);
                throw;
            }
            return success >= 1;
        }

        public static int execSQLEx(string sql)  // ilość przetworzonych rekordów lub -1 jak duplicate key
        {
            return execSQLEx(con, sql);
        }

        public static int execSQLEx(SqlConnection con, string sql)  // ilość przetworzonych rekordów lub -1 jak duplicate key
        {
            int ret = 0;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, con);

                sqlCmd.CommandTimeout = 300;

                ret = sqlCmd.ExecuteNonQuery();  // >= 1 sukces - ilość przetworzonych rekordów
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                switch (ex.Number)  //http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlexception(v=vs.85).aspx
                {
                    case 2627:
                    case 2601:
                        ret = -1;
                        break;
                    default:
                        Log.Error(Log.SQL, sql, ex.Message);
                        throw;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sql, ex.Message);
                throw;
            }
            return ret;
        }
        //-----------------------------------
        public static bool execSQL(SqlCommand sqlCmd)
        {
            int success = 0;
            try
            {
                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sqlCmd.CommandText, ex.Message);
                throw;
            }
            return success >= 1;
        }

        public static DataSet selectSQL(SqlCommand sqlCmd)
        {
            DataSet ds;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                ds = new DataSet();
                da.Fill(ds);    //<<<< jak da nie zwróci danych to Tables.Count = 0 - zastanowić się czy nie zwracać w takim przypadku null 
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sqlCmd.CommandText, ex.Message);
                throw;
            }
            return ds;
        }
        //-----------------------------------
        public static bool execSQL(SqlConnection con, string sql, params object[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            if (values != null)
                for (int i = 0; i < values.Length; i++)
                    if (values[i] == null)
                        cmd.Parameters.AddWithValue("p" + i.ToString(), DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("p" + i.ToString(), values[i]);
            return execSQL(cmd);
        }

        public static DataSet selectSQL(SqlConnection con, string sql, params object[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, con);
            if (values != null)
                for (int i = 0; i < values.Length; i++)
                    if (values[i] == null)
                        cmd.Parameters.AddWithValue("p" + i.ToString(), DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("p" + i.ToString(), values[i]);
            return selectSQL(cmd);
        }

        public static bool insert(SqlConnection con, out string id, string table, string fields, params object[] values)
        {
            id = null;
            int len = values.Length;
            string[] par = new string[len];
            for (int i = 0; i < len; i++)
                par[i] = "@p" + i.ToString();
            string sql = String.Format("insert into {0} ({1}) values ({2})", table, fields, String.Join(",", par));
            bool ok = execSQL(con, sql, values);
            if (ok) id = getScalar(con, "select @@Identity");
            return ok;
        }

        public static bool update(SqlConnection con, string table, string where, string fields, params object[] values)   // where można podać np. Id=123 lub Id=@pX, X - numer po wszystkich parametrach, numerowane od @p0
        {
            string[] f = fields.Split(',');
            int len = f.Length;
            string[] upd = new string[len];
            for (int i = 0; i < len; i++)
                upd[i] = String.Format("{0}=@p{1}", f[i], i);
            string sql = String.Format("update {0} set {1} where {2}", table, String.Join(",", upd), where);
            return execSQL(con, sql, values);
        }

        public static bool delete(SqlConnection con, string table, string where)  // do kompletu
        {
            string sql = String.Format("delete from {0} where {1}", table, where);
            return execSQL(con, sql, null);
        }
        //-----
        public static bool execSQL(string sql, params object[] values)
        {
            return execSQL(con, sql, values);
        }

        public static DataSet selectSQL(string sql, params object[] values)
        {
            return selectSQL(con, sql, values);
        }

        public static bool insert(out string id, string table, string fields, params object[] values)
        {
            return insert(con, out id, table, fields, values);
        }

        public static bool update(string table, string where, string fields, params object[] values)   // where można podać np. Id=123 lub Id=@pX, X - numer po wszystkich parametrach, numerowane od @p0
        {
            return update(con, table, where, fields, values);
        }

        public static bool delete(string table, string where)  // do kompletu
        {
            return delete(con, table, where);
        }
        
        //-----------------------------------
        // wymaga testów ... wyglada na to ze trzeba też przekazać parametry wejściowe czyli odpada inline
        public static int execProc_1(string sql)   
        {
            int success = 0;
            try
            {
                //sql = "exec @ret=" + sql;
                sql = "exec @ret=" + sql;
                SqlCommand cmd = new SqlCommand(sql, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                var ret = cmd.Parameters.Add("@ret", SqlDbType.Int);
                ret.Direction = ParameterDirection.ReturnValue;

                success = cmd.ExecuteNonQuery();  // 1 sukces
                return (int)ret.Value;
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sql, ex.Message);
                throw;
            }

        }
        public static int execProc_2(string sql)
        {
            try
            {

                SqlCommand cmd = new SqlCommand(sql, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                
                object s = cmd.ExecuteScalar();  // 1 sukces


                return -9;
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, sql, ex.Message);
                throw;
            }
        }
        
/*        
        using (SqlConnection conn = new SqlConnection(getConnectionString()))
using (SqlCommand cmd = conn.CreateCommand())
{
    cmd.CommandText = parameterStatement.getQuery();
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("SeqName", "SeqNameValue");

    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
    returnParameter.Direction = ParameterDirection.ReturnValue;

    conn.Open();
    cmd.ExecuteNonQuery();
    var result = returnParameter.Value;
}
        */
        //-----------------------------------
        public static string GetColumns(string tbName)   // lista oddzielona , 
        {
            DataSet ds = getDataSet(String.Format(
                "select C.name from sys.columns C " +
                "left join sys.tables T on C.object_id = T.object_id " +
                "where T.name = '{0}' " +
                "order by C.column_id"
                , tbName));
            DataRowCollection rows = ds.Tables[0].Rows;
            if (rows.Count > 0)
            {
                string fields = rows[0][0].ToString();
                for (int i = 1; i < rows.Count; i++)
                    fields += "," + rows[i][0].ToString();
                return fields;
            }
            else return null;
        }

        public static string Join(DataSet ds, string fieldName, string delimiter)
        {
            int cnt = getCount(ds);
            if (cnt > 0)
            {
                string[] data = new string[cnt];
                DataRowCollection rows = ds.Tables[0].Rows;
                for (int i = 0; i < cnt; i++)
                    data[i] = getValue(rows[i], fieldName);
                return String.Join(delimiter, data);
            }
            else return null;
        }

        public static string Join(DataSet ds, int fieldIndex, string delimiter)
        {
            int cnt = getCount(ds);
            if (cnt > 0)
            {
                string[] data = new string[cnt];
                DataRowCollection rows = ds.Tables[0].Rows;
                for (int i = 0; i < cnt; i++)
                    data[i] = getValue(rows[i], fieldIndex);
                return String.Join(delimiter, data);
            }
            else return null;
        }

        //public static string Join(DataSet ds, int fieldIndex, string delimiter)
        //{
        //    return Join(ds, fieldIndex, delimiter, 0, -1);
        //}

        public static string Join(DataSet ds, int fieldIndex, string delimiter, int fromIndex, int count, bool withEmpty)
        {
            int empty = 0;
            int cnt = getCount(ds);
            if (count != -1 && fromIndex + count < cnt)
                cnt = fromIndex + count;
            if (cnt > 0)
            {
                string[] data = new string[cnt];
                DataRowCollection rows = ds.Tables[0].Rows;
                int j = 0;
                for (int i = 0; i < cnt; i++)
                {
                    string v = getValue(rows[i + fromIndex], fieldIndex);
                    if (withEmpty || !String.IsNullOrEmpty(v))
                        data[j++] = v;
                    else
                        empty++;
                }
                return String.Join(delimiter, data, 0, cnt - empty);
            }
            else return null;
        }

        public static string Join(DataSet ds, string fieldName, string delimiter, int fromIndex, int count, bool withEmpty)
        {
            int empty = 0;
            int cnt = getCount(ds);
            if (count != -1 && fromIndex + count < cnt)
                cnt = fromIndex + count;
            if (cnt > 0)
            {
                string[] data = new string[cnt];
                DataRowCollection rows = ds.Tables[0].Rows;
                int j = 0;
                for (int i = 0; i < cnt; i++)
                {
                    string v = getValue(rows[i + fromIndex], fieldName);
                    if (withEmpty || !String.IsNullOrEmpty(v))    
                        data[j++] = v;
                    else
                        empty++;
                }
                return String.Join(delimiter, data, 0, cnt - empty);
            }
            else return null;
        }
        //--------------------------------------
        public static string LastInsertSql = null;
        public static string LastUpdateSql = null;

        public static int insert(string sql, bool getIdentity, bool log) // zwraca last Id lub -1 jak duplicate index
        {
            return insert(con, sql, getIdentity, log);
        }

        public static int insert(string tbName, bool getIdentity, bool log, int start, string fields, params object[] list)
        {
            return insert(insertCmd(tbName, start, fields, list), getIdentity, log);
        }

        public static int insert(SqlConnection con2, string tbName, bool getIdentity, bool log, int start, string fields, params object[] list)
        {
            return insert(con2, insertCmd(tbName, start, fields, list), getIdentity, log);
        }

        public static int insert(SqlConnection con2, string sql, bool getIdentity, bool log) // zwraca last Id lub -1 jak duplicate index
        {
            int ret = -1;
            int success = 0;
            LastInsertSql = sql;

            bool fcon = con2 == null;
            if (fcon) DoConnect(ref con2);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con2);
                try
                {
                    success = cmd.ExecuteNonQuery();  // 1 sukces, ilość przetworzonych wierszy
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    switch (ex.Number)  //http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlexception(v=vs.85).aspx
                    {
                        case 2627:
                        case 2601:
                            ret = -1;
                            break;
                        default:
                            if (log) Log.Error(Log.SQL, sql, ex.Message);  // con2 ?????
                            throw;
                    }
                }
                catch (Exception ex)
                {
                    if (log) Log.Error(Log.SQL, sql, ex.Message);   // con2 ?????
                    throw;
                }
                if (success == 1)    // ilosc przetworzonych wierszy == 1 - z reguły wstawia się 1 rekord 
                    if (getIdentity)
                    {
                        cmd.CommandText = "select @@Identity";
                        ret = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else ret = 1;
                return ret;
            }
            finally
            {
                if (fcon) DoDisconnect(ref con2);
            }
        }

        public static int insert(SqlTransaction tr, string sql, bool getIdentity, bool log) // zwraca last Id lub -1 jak duplicate index
        {
            int ret = -1;
            int success = 0;
            LastInsertSql = sql;

            SqlCommand cmd;
            if (tr == null)
                cmd = new SqlCommand(sql, con);
            else
                cmd = new SqlCommand(sql, tr.Connection, tr);
            try
            {
                success = cmd.ExecuteNonQuery();  // 1 sukces, ilość przetworzonych wierszy
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                switch (ex.Number)  //http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlexception(v=vs.85).aspx
                {
                    case 2627:
                    case 2601:
                        ret = -1;
                        break;
                    default:
                        if (log) Log.ErrorTr(Log.SQL, sql, ex.Message);
                        throw;
                }
            }
            catch (Exception ex)
            {
                if (log) Log.ErrorTr(Log.SQL, sql, ex.Message);
                throw;
            }
            if (success == 1)    // ilosc przetworzonych wierszy == 1 - z reguły wstawia się 1 rekord 
                if (getIdentity)
                {
                    cmd.CommandText = "select @@Identity";
                    ret = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else ret = 1;
            return ret;
        }

        public static int getIdentity(SqlTransaction tr, bool log) // zwraca last Id 
        {
            int ret;
            const string sql = "select @@Identity";
            SqlCommand cmd;
            if (tr == null)
                cmd = new SqlCommand(sql, con);
            else
                cmd = new SqlCommand(sql, tr.Connection, tr);
            try
            {
                ret = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                if (log) Log.ErrorTr(Log.SQL, sql, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                if (log) Log.ErrorTr(Log.SQL, sql, ex.Message);
                throw;
            }
            return ret;
        }

        public static bool insert(string sql) // zwraca false jak dup lub się nie powiodło, true jak dodał
        {
            return insert(sql, false, true) != -1;
        }

        public static bool insert(string tbName, int start, string fields, params object[] list)
        {
            return insert(insertCmd(tbName, start, fields, list), false, true) != -1;
        }

        public static bool insert(SqlConnection con, string tbName, int start, string fields, params object[] list)
        {
            return insert(con, insertCmd(tbName, start, fields, list), false, true) != -1;
        }

        //------------------------------------------------------
        public static bool update(string tbName, int start, string fields, string where, params object[] list)
        {
            string sql = updateCmd(tbName, start, fields, where, list);
            LastUpdateSql = sql;
            return execSQL(sql);
        }

        public static bool update(SqlConnection con, string tbName, int start, string fields, string where, params object[] list)
        {
            string sql = updateCmd(tbName, start, fields, where, list);
            LastUpdateSql = sql;
            return execSQL(con, sql);
        }

        public static bool update(SqlTransaction tr, string tbName, int start, string fields, string where, params object[] list)
        {
            string sql = updateCmd(tbName, start, fields, where, list);
            LastUpdateSql = sql;
            return execSQL(tr, sql);
        }

        //------------------------------------------------------
        public static string PrepareUpdateParams(int start, string fields)  // f1,f2... -> f1={0},f2={1}...
        {
            string[] ff = fields.Split(',');
            for (int i = 0; i < ff.Count(); i++)
                ff[i] = ff[i].Trim() + "={" + (start + i).ToString() + "}";
            return String.Join(",", ff);
        }

        public static string PrepareInsertParams(int start, int count)  // {0},{1}...
        {
            string[] ff = new string[count];
            for (int i = 0; i < count; i++)
                ff[i] = "{" + (start + i).ToString() + "}";
            return String.Join(",", ff);
        }

        public static string updateCmd2(string tbName, int start, string fields, string extraSet, string where, params object[] list)  // update tbName set f1={0},f2={1} [,xx=vv] where...
        {
            return String.Format("update " + tbName + " set " + PrepareUpdateParams(start, fields) + extraSet +
                        (!String.IsNullOrEmpty(where) ? " where " + where : null), list);
        }

        public static string insertCmd2(string tbName, int start, string fields, string extraFields, string extraValues, params object[] list)  // update tbName set f1={0},f2={1} where...
        {
            if (String.IsNullOrEmpty(fields))
            {
                return String.Format("insert into " + tbName + " values (" + PrepareInsertParams(start, list.Count()) + ")", list);
            }
            else
            {
                string[] ff = fields.Split(',');
                return String.Format("insert into " + tbName + " (" + fields + extraFields + ") values (" + PrepareInsertParams(start, ff.Count()) + extraValues + ")", list);
            }
        }

        public static string updateCmd(string tbName, int start, string fields, string where, params object[] list)
        {
            return updateCmd2(tbName, start, fields, null, where, list);
        }

        public static string insertCmd(string tbName, int start, string fields, params object[] list)
        {
            return insertCmd2(tbName, start, fields, null, null, list);
        }

        //------------------------------------------------------
        public static string sqlPut(string s, int len)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            else
            {
                string ret = sqlPut(s);
                if (ret.Length > len)
                {
                    ret = ret.Substring(0, len);
                    if (ret.EndsWith("'") && !ret.EndsWith("''"))
                        ret = ret.Substring(0, ret.Length - 1);    // jesli konczy sie ' to go obcinam bo obcięcie 1800 moze wypasc akurat na '
                }
                return ret;
            }
        }

        public static string sqlPut(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            else
                return s.Replace("'", "''");
        }

        //------------------------------------------------------
        public static string param(string p)
        {
            if (String.IsNullOrEmpty(p))
                return NULL;
            else
                return p;
        }

        public static string paramStr(string p)
        {
            if (String.IsNullOrEmpty(p))
                return NULL;
            else
                return "'" + p + "'";
        }

        public static string nullParam(object param)
        {
            if (param != null)
            {
                string p = param.ToString();
                if (!String.IsNullOrEmpty(p))
                    return p;
            }
            return NULL;
        }

        public static string nullParamStr(object param)
        {
            if (!isNull(param))
                return strParam(param.ToString());
            else
                return NULL;
        }

        public static string nullStrParam(string param)  // old
        {
            if (String.IsNullOrEmpty(param))
                return NULL;
            else
                return strParam(param);
        }

        public static string strParam(string param)
        {
            return "'" + param + "'";
        }

        //-------------------------------------------
        public static string GetColumns(SqlConnection con, string tbName)   // lista oddzielona , 
        {
            DataSet ds = getDataSet(con, String.Format(
                "select C.name from sys.columns C " +
                "left join sys.tables T on C.object_id = T.object_id " +
                "where T.name = '{0}' " +
                "order by C.column_id", tbName));
            DataRowCollection rows = getRows(ds);
            if (rows.Count > 0)
            {
                string fields = rows[0][0].ToString();
                for (int i = 1; i < rows.Count; i++)
                    fields += "," + rows[i][0].ToString();
                return fields;  
            }
            else return null;
        }

        public static string GetColumns(SqlTransaction con, string tbName)   // lista oddzielona , 
        {
            DataSet ds = getDataSet(con, String.Format(
                "select C.name from sys.columns C " +
                "left join sys.tables T on C.object_id = T.object_id " +
                "where T.name = '{0}' " +
                "order by C.column_id",
                tbName));
            DataRowCollection rows = getRows(ds);
            if (rows.Count > 0)
            {
                string fields = rows[0][0].ToString();
                for (int i = 1; i < rows.Count; i++)
                    fields += "," + rows[i][0].ToString();
                return fields;
            }
            else return null;
        }
        //-------------------------------------------
        public static string ToString(string sql, string vfmt, string fldsep, string lnsep)
        {
            if (fldsep == null) fldsep = ",";
            if (lnsep == null) lnsep = "\r\n";
            if (vfmt == null) vfmt = "[{0}]='{1}'";
            DataSet ds = getDataSet(sql);
            int rows = getCount(ds);
            int cols = ds.Tables[0].Columns.Count;
            string[] lnA = new string[rows];
            string[] vvA = new string[cols];
            for (int r = 0; r < rows; r ++)
            {
                for (int c = 0; c < cols; c++)
                {
                    vvA[c] = String.Format(vfmt,
                            ds.Tables[0].Columns[c].ToString(),
                            getValue(getRow(ds, r), c));
                }
                lnA[r] = String.Join(fldsep, vvA);
            }
            return String.Join(lnsep, lnA);
        }

        //-------------------------------------------
        public static void ToFile(string FileName, string sql)
        {
            DataSet ds = getDataSet(sql);
            //File.Delete(FileName);
            //StreamWriter sw = new StreamWriter(FileName, false ,Encoding.GetEncoding("UTF-8"));
            StreamWriter sw = new StreamWriter(FileName, false, Encoding.GetEncoding("UTF-16"));
            sw.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
            try
            {
                int cnt = ds.Tables[0].Columns.Count;
                string[] sa = new string[cnt];
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < cnt; i++)
                        if (ds.Tables[0].Columns[i].DataType == System.Type.GetType("System.String"))
                            sa[i] = "\"" + dr[i].ToString().Trim() + "\"";
                        else
                            sa[i] = dr[i].ToString().Trim();
                    string line = String.Join("\t", sa);
                    sw.WriteLine(line);
                }
            }
            finally
            {
                sw.Close();
            }
        }

        public static void ToFile(SqlConnection con, string FileName, bool newFile, string headerLine, string sql)
        {
            StreamWriter sw = new StreamWriter(FileName, !newFile, Encoding.GetEncoding("UTF-8"));
            DataSet ds = getDataSet(con, sql);
            Type tstring = System.Type.GetType("System.String");
            Type tdouble = System.Type.GetType("System.Double");
            Type tint32 = System.Type.GetType("System.Int32");
            try
            {
                sw.WriteLine(headerLine);
                int cnt = ds.Tables[0].Columns.Count;
                string[] sa = new string[cnt];
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        Type ct = ds.Tables[0].Columns[i].DataType;
                        if (dr[i].Equals(DBNull.Value))
                            sa[i] = NULL;
                        else if (ct == tstring)
                            sa[i] = "'" + prepareLineToFile(dr[i].ToString().Trim()) + "'";
                        else if (ct == tdouble)
                            sa[i] = dr[i].ToString().Replace(',', '.');
                        else if (ct == tint32)
                            sa[i] = dr[i].ToString();
                        else
                            sa[i] = "'" + dr[i].ToString().Trim() + "'";  // wszystko jako string i niech sam konwertuje poźniej
                    }
                    string line = String.Join("\t", sa);
                    sw.WriteLine(line);
                }
            }
            finally
            {
                sw.Close();
            }
        }

        public static void ToFile(SqlConnection con, string FileName, bool newFile, string tbName)
        {
            ToFile(con, FileName, newFile, "[" + tbName + "]", "select * from " + tbName);
        }

        public static string prepareLineToFile(string line)
        {
            return line.Replace("'", "''").Replace("\r",@"\r").Replace("\n",@"\n").Replace("\t",@"\t");
        }

        public static string prepareLineToInsert(string line)
        {
            return line.Replace(@"\t","\t").Replace(@"\n","\n").Replace(@"\r","\r");
        }

        public static void fromFile(string table, string file)
        {
            StreamReader sr = new StreamReader(file, Encoding.GetEncoding("UTF-8"));
            SqlTransaction tr = null;
            string fields = null;
            bool fid = false;
            try
            {
                try
                {
                    tr = con.BeginTransaction("IMP1");    // tylko jak są jakieś dane zaczynam transakcję
                    fields = getScalar(tr,                      // kolumny
                        "select ',' + COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = " +
                        strParam(table) +
                        " order by ORDINAL_POSITION for xml path('')"
                        ).Substring(1);
                    fid = getScalar(tr,                         // czy identity
                        "select COUNT(*) from syscolumns where OBJECT_NAME(id) = " +
                        strParam(table) +
                        " and COLUMNPROPERTY(id, name, 'IsIdentity') = 1"
                        ) == "1";
                    execSQL(tr, "delete from " + table);        // usuwam
                    if (fid) execSQL(tr, "set identity_insert " + table + " ON");
                    int cnt = 0;
                    
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string cmd = "insert into " + table + " (" + fields + ") values (" + prepareLineToInsert(line.Replace("\t", ",")).Replace(",,", ",NULL,").Replace(",,", ",NULL,") + ")";
                        execSQL(tr, cmd);
                        cnt++;
                        line = sr.ReadLine();
                    }
                    if (fid) execSQL(tr, "set identity_insert " + table + " OFF");
                    tr.Commit();
                }
                finally
                {
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        
        /*
            select STUFF(
	            (select ',' + C.COLUMN_NAME         
	             from INFORMATION_SCHEMA.COLUMNS C         
	             where C.TABLE_SCHEMA = T.TABLE_SCHEMA and C.TABLE_NAME = T.TABLE_NAME 
	             order by C.ORDINAL_POSITION
	             for xml path('')), 1, 1, '') 
            from INFORMATION_SCHEMA.TABLES T  
            where TABLE_NAME='Ankiety'

            select STUFF(
	            (select ',' + C.COLUMN_NAME         
	             from INFORMATION_SCHEMA.COLUMNS C         
	             where C.TABLE_NAME = 'Ankiety'
	             order by C.ORDINAL_POSITION
	             for xml path('')), 1, 1, '') 
         */
        
        public static void ImportData(MemoryStream data)
        {
            StreamReader sr = new StreamReader(data, Encoding.GetEncoding("UTF-8"));
            SqlTransaction tr = null;
            string table = null;
            string fields = null;
            bool fid = false;
            try
            {
                try
                {
                    int cnt = 0;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            if (tr == null)
                                tr = con.BeginTransaction("RB1");         // tylko jak są jakieś dane zaczynam transakcję
                            if (table != null)
                                if (fid) execSQL(tr, "set identity_insert " + table + " OFF");
                            table = line.Substring(1, line.Length - 2);         // tabela
                            if (!String.IsNullOrEmpty(table))
                            {
                                fields = getScalar(tr, 
                                    "select ',' + COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = " + 
                                    strParam(table) + 
                                    " order by ORDINAL_POSITION for xml path('')"
                                    ).Substring(1);
                                fid = getScalar(tr,
                                    "select COUNT(*) from syscolumns where OBJECT_NAME(id) = " +
                                    strParam(table) +
                                    " and COLUMNPROPERTY(id, name, 'IsIdentity') = 1"
                                    ) == "1";
                                execSQL(tr, "delete from " + table);
                                if (fid) execSQL(tr, "set identity_insert " + table + " ON");
                                cnt = 0;
                            }
                        }
                        else
                        {
                            string cmd = "insert into " + table + " (" + fields + ") values (" + prepareLineToInsert(line.Replace("\t", ",")) + ")";
                            execSQL(tr, cmd);
                            cnt++;
                        }
                        line = sr.ReadLine();
                    }
                    if (tr != null)  // jak exception to Disconnect sie samo zrobi, set identity jak exception tez chyba nie ma sensu wywoływać...
                    {
                        if (table != null)
                            if (fid) execSQL(tr, "set identity_insert " + table + " OFF");
                        tr.Commit();
                    }
                }
                finally
                {
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                //cnt
                throw;
            }
        }

        //------------------------------------------------ z Matrycy
        public static bool backupTable(string tbName, string bckName)
        {
            if (String.IsNullOrEmpty(bckName)) bckName = "copy" + tbName;
            return db.execSQL(String.Format(
                "IF EXISTS(SELECT name FROM sysobjects WHERE name = '{1}' AND xtype='U') DROP TABLE {1};" +
                "select * into {1} from {0}",
                tbName, bckName));
        }

        public static bool restoreTable(SqlTransaction tr, string bckName, string tbName, bool identity)
        {
            if (String.IsNullOrEmpty(bckName)) bckName = "copy" + tbName;
            string fields = db.GetColumns(tr, tbName);
            return db.execSQL(tr, String.Format(
                "IF EXISTS(SELECT name FROM sysobjects WHERE name = '{0}' AND xtype='U') begin " +
                    "delete from {1};" +
                    (identity ? "set identity_insert {1} ON;" : null) +
                    "insert into {1} ({2}) select {2} from {0};" +
                    (identity ? "set identity_insert {1} OFF;" : null) +
                "end",
                bckName, tbName, fields));
        }

        public static string dropCopyOfTable(string tbName)
        {
            return String.Format(
                "IF EXISTS(SELECT name FROM sysobjects WHERE name = 'copy{0}' AND xtype='U') DROP TABLE copy{0}",
                tbName);
        }

        //------------------------------------------------------------------------------
        // IMPORT DANYCH 
        //------------------------------------------------------------------------------
        public static bool ImportRecord(DataSet ds, int rowindex, string toTable)
        {
            int cnt = ds.Tables[0].Columns.Count;
            string flist = "@f0";
            for (int i = 1; i < cnt; i++)
                flist += ",@f" + i.ToString();
            SqlCommand cmd = new SqlCommand(String.Format(
                "insert into {0} values ({1})", toTable, flist),
                db.con);
            for (int i = 0; i < cnt; i++)
            {
                SqlParameter p = new SqlParameter("f" + i.ToString(), ds.Tables[0].Rows[rowindex][i]);
                cmd.Parameters.Add(p);
            }
            return db.execSQL(cmd);
        }
        
        public static bool ImportRecord(DataRow dr, string toTable)
        {
            int cnt = dr.ItemArray.Count();
            string flist = "@f0";
            for (int i = 1; i < cnt; i++)
                flist += ",@f" + i.ToString();
            SqlCommand cmd = new SqlCommand(String.Format(
                "insert into {0} values ({1})", toTable, flist),
                db.con);
            for (int i = 0; i < cnt; i++)
                cmd.Parameters.Add(new SqlParameter("f" + i.ToString(), dr[i]));
            return db.execSQL(cmd);
        }

        /*
        public static int ImportTable(string toTable, bool deleteAll, SqlConnection conFrom, string sql)
        {
            int cnt = 0;
            int d = 1;
            DataSet ds = Base.getDataSet(conFrom, sql);
            if (deleteAll) db.execSQL("delete from " + toTable);
            foreach (DataRow dr in db.getRows(ds))
            {
                if (!ImportRecord(dr, toTable))
                {
                    d = -1;
                    Log.Error(Log.IMPORT, "Import to table: " + toTable, "Index: " + cnt.ToString());
                }
                cnt++;
            }
            return cnt * d;
        }
        */

        public static int ImportTable(string toTable, bool deleteAll, SqlConnection conFrom, string sql)
        {
            string delSql = deleteAll ? String.Format("delete from {0}", toTable) : null;
            return ImportTable(toTable, delSql, conFrom, sql);
        }
            
        public static int ImportTable(string toTable, string beforeSql, SqlConnection conFrom, string sql)
        {
            int cnt = 0;
            int d = 1;
            DataSet ds = Base.getDataSet(conFrom, sql, 600);
            if (!String.IsNullOrEmpty(beforeSql))
                db.execSQL(beforeSql);
            foreach (DataRow dr in db.getRows(ds))
            {
                if (!ImportRecord(dr, toTable))
                {
                    d = -1;
                    Log.Error(Log.IMPORT, "Import to table: " + toTable, "Index: " + cnt.ToString());
                }
                cnt++;
            }
            return cnt * d;
        }

        public static int ExportTable(string toTable, bool deleteAll, SqlConnection conDest, string sql)
        {
            return CopyTable(db.con, sql, conDest, toTable, deleteAll);
        }
        //----- 
        public static bool CopyRecord(DataRow dr, SqlConnection conDest, string toTable)
        {
            int cnt = dr.ItemArray.Count();
            string flist = "@f0";
            for (int i = 1; i < cnt; i++)
                flist += ",@f" + i.ToString();
            SqlCommand cmd = new SqlCommand(String.Format(
                "insert into {0} values ({1})", toTable, flist), conDest);
            for (int i = 0; i < cnt; i++)
                cmd.Parameters.Add(new SqlParameter("f" + i.ToString(), dr[i]));
            return db.execSQL(cmd);
        }

        public static int CopyTable(SqlConnection conFrom, string sql, 
                                    SqlConnection conDest, string toTable, bool deleteAll)
        {
            int cnt = 0;
            int d = 1;
            DataSet ds = Base.getDataSet(conFrom, sql);
            if (deleteAll) db.execSQL(conDest, "delete from " + toTable);
            foreach (DataRow dr in db.getRows(ds))
            {
                if (!CopyRecord(dr, conDest, toTable))
                {
                    d = -1;
                    Log.Error(Log.IMPORT, "Import to table: " + toTable, "Index: " + cnt.ToString());
                }
                cnt++;
            }
            return cnt * d;
        }
        //---------------------------------------------
        public static string toCSV(DataSet ds)
        {
            StringWriter stringWriter = new StringWriter();

            string d;
            string line = null;
            int cnt = ds.Tables[0].Columns.Count;
            for (int i = 0; i < cnt; i++)
            {
                d = Tools.CtrlToText(ds.Tables[0].Columns[i].ToString());
                if (i == 0)
                    line = d;
                else
                    line += Tools.TAB + d;
            }
            stringWriter.WriteLine(line);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                for (int i = 0; i < cnt; i++)
                {
                    d = Tools.CtrlToText(dr[i].ToString());
                    if (i == 0)
                        line = d;
                    else
                        line += Tools.TAB + d;          // mało optymalne !!! ale to do testów ...
                }
                stringWriter.WriteLine(line);
            }
            return stringWriter.ToString();
        }

        public static string DataSetCompare(DataSet ds1, DataSet ds2)
        {
            string csv1 = null;
            string csv2 = null;
            int c1 = 0;
            int c2 = 0;
            if (ds1 != null)
            {
                c1 = getCount(ds1);
                csv1 = toCSV(ds1); 
            }
            if (ds2 != null)
            {
                c2 = getCount(ds2);
                csv2 = toCSV(ds2);
            }
            bool match = csv1 == csv2;
            string s1 = String.Format("match: {2} ds1_count: {0} ds2_count: {1}", c1, c2, match ? "1" : "0") + "\n";
            s1 += "----------------------------------\n";
            string diff = null;
            string s2 = null;
            s2 += "----------------------------------\n";
            if (c1 > 0) s2 += csv1 + "\n";
            s2 += "----------------------------------\n";
            if (c2 > 0) s2 += csv2 + "\n";

            if (!match)
            {
                int cols = ds1.Tables[0].Columns.Count;
                if (c2 < c1) c1 = c2;
                for (int i = 0; i < c1; i++)
                {
                    DataRow dr1 = db.getRow(ds1, i);
                    DataRow dr2 = db.getRow(ds2, i);
                    bool m = true;
                    for (int c = 0; c < cols; c++)
                        if (db.getValue(dr1, c) != db.getValue(dr2, c))
                        {
                            m = false;
                            break;
                        }
                    if (!m)
                    {
                        string l1 = null;
                        string l2 = null;
                        for (int c = 0; c < cols; c++)
                        {
                            string d1 = Tools.CtrlToText(dr1[c].ToString());
                            l1 += c == 0 ? d1 : (Tools.TAB + d1);          // mało optymalne !!! ale to do testów ...
                            string d2 = Tools.CtrlToText(dr2[c].ToString());
                            l2 += c == 0 ? d2 : (Tools.TAB + d2);          // mało optymalne !!! ale to do testów ...
                        }
                        diff += l1 + Tools.TAB + " --- " + Tools.TAB + l2 + "\n"; 
                    }
                }
                 
            }
            string sss = s1 + diff + s2; 
            if (!match)
            {
                int x = 0;  //<<<< tu załóż brakepoint'a
            }
            return sss;
        }
        //--------
        public static int getColCount(DataRow[] Rows)
        {
            return Rows.Length > 0 ? Rows[0].ItemArray.Length : 0;
        }

        public static string toCSV(DataRow[] Rows)
        {
            StringWriter stringWriter = new StringWriter();

            string d;
            string line = null;
            int cnt = getColCount(Rows);

            foreach (DataRow dr in Rows)
            {
                for (int i = 0; i < cnt; i++)
                {
                    d = Tools.CtrlToText(dr[i].ToString());
                    if (i == 0)
                        line = d;
                    else
                        line += Tools.TAB + d;          // mało optymalne !!! ale to do testów ...
                }
                stringWriter.WriteLine(line);
            }
            return stringWriter.ToString();
        }

        public static string DataRowsCompare(DataRow[] Rows1, DataRow[] Rows2)
        {
            string csv1 = null;
            string csv2 = null;
            int c1 = 0;
            int c2 = 0;
            if (Rows1 != null)
            {
                c1 = Rows1.Length;
                csv1 = toCSV(Rows1);
            }
            if (Rows2 != null)
            {
                c2 = Rows2.Length;
                csv2 = toCSV(Rows2);
            }
            bool match = csv1 == csv2;
            string s1 = String.Format("match: {2} ds1_count: {0} ds2_count: {1}", c1, c2, match ? "1" : "0") + "\n";
            s1 += "----------------------------------\n";
            string diff = null;
            string s2 = null;
            s2 += "----------------------------------\n";
            if (c1 > 0) s2 += csv1 + "\n";
            s2 += "----------------------------------\n";
            if (c2 > 0) s2 += csv2 + "\n";

            if (!match)
            {
                int cols = getColCount(Rows1);
                if (c2 < c1) c1 = c2;
                for (int i = 0; i < c1; i++)
                {
                    DataRow dr1 = Rows1[i];
                    DataRow dr2 = Rows2[i];
                    bool m = true;
                    for (int c = 0; c < cols; c++)
                        if (db.getValue(dr1, c) != db.getValue(dr2, c))
                        {
                            m = false;
                            break;
                        }
                    if (!m)
                    {
                        string l1 = null;
                        string l2 = null;
                        for (int c = 0; c < cols; c++)
                        {
                            string d1 = Tools.CtrlToText(dr1[c].ToString());
                            l1 += c == 0 ? d1 : (Tools.TAB + d1);          // mało optymalne !!! ale to do testów ...
                            string d2 = Tools.CtrlToText(dr2[c].ToString());
                            l2 += c == 0 ? d2 : (Tools.TAB + d2);          // mało optymalne !!! ale to do testów ...
                        }
                        diff += l1 + Tools.TAB + " --- " + Tools.TAB + l2 + "\n";
                    }
                }

            }
            string sss = s1 + diff + s2;
            if (!match)
            {
                int x = 0;  //<<<< tu załóż brakepoint'a
            }
            return sss;
        }






        public static Nullable<T> getScalar<T>(SqlConnection con, string sql)
            where T : struct
        {
            DataRow dr = getDataRow(con, sql);
            return (dr == null || dr[0] is DBNull) ? (Nullable<T>)null : (T)dr[0];
        }
        /*
        public static Nullable<T> getScalar<T>(string sql)
            where T : struct
        {
            DataRow dr = getDataRow(sql);
            return (dr == null || dr[0] is DBNull) ? (Nullable<T>)null : (T)dr[0];
        }    
        */
        //------------------------------------------
        public static bool Lock(string lockID, AppUser user)
        {
            SqlTransaction tr = con.BeginTransaction("LOCK1");    //blokada rekordu
            string l = getScalar(tr, String.Format("select case when TimeOut != 0 and GETDATE() > DATEADD(S,TimeOut,LockData) then 0 else Blokada end from Locks where Typ = '{0}'", lockID));
            bool locked = l == "1"; 
            if (!locked)
                update(tr, "Locks", 1, "Blokada,AutorId,LockData", "Typ='{0}'", lockID, 1, user.Id, "GETDATE()");
            tr.Commit();
            return !locked;
        }

        public static void Unlock(string lockID)
        {
            update("Locks", 1, "Blokada,UnlockData", "Typ='{0}'", lockID, 0, "GETDATE()");
        }

        /* do zapytań sql
declare @locked bit
declare @lockId varchar(25) 
declare @AutorId int, @LockData datetime, @UnlockData datetime
set @lockId = 'IMPORTBHP' 
select @locked = case when TimeOut != 0 and GETDATE() > DATEADD(S,TimeOut,LockData) then 0 else Blokada end, @LockData = LockData, @AutorId = AutorId 
from Locks where Typ = @lockId
if @locked = 0 begin
    update Locks set Blokada = 1, AutorId = 0, LockData = GETDATE() where Typ = @lockId
    ----------
    ----------
    update Locks set Blokada = 0, AutorId = 0, unlockData = GETDATE() where Typ = @lockId
end
else begin
    
end
select @locked
         */

        //------------------------------------------
        // STREFA JUANA I PEDRO
        #region JUAN_PEDRO

        public static Boolean Execute(String Sql, params object[] Args)
        {
            return execSQL(String.Format(Sql, Args));
        }

        public static Boolean Execute(SqlDataSource SqlDs, params object[] Args)
        {
            //string conString = !String.IsNullOrEmpty(SqlDs.ConnectionString) ? SqlDs.ConnectionString : conStr;
            //return execSQL(Connect(conString), String.Format(SqlDs.SelectCommand, Args));

            //20170214 T
            if (!String.IsNullOrEmpty(SqlDs.ConnectionString))
            {
                SqlConnection con2 = Connect(SqlDs.ConnectionString);
                bool ret = execSQL(con2, String.Format(SqlDs.SelectCommand, Args));
                Disconnect(con2);
                return ret;
            }
            else
                return execSQL(String.Format(SqlDs.SelectCommand, Args));
        }

        //T: żeby można było 1 SqlDataSource zastosować
        public static Boolean ExecuteCommand(SqlDataSource SqlDs, string command, params object[] Args)
        {
            //string conString = !String.IsNullOrEmpty(SqlDs.ConnectionString) ? SqlDs.ConnectionString : conStr;
            //return execSQL(Connect(conString), String.Format(command, Args));

            //20170214 T
            if (!String.IsNullOrEmpty(SqlDs.ConnectionString))
            {
                SqlConnection con2 = Connect(SqlDs.ConnectionString);
                bool ret = execSQL(con2, String.Format(command, Args));
                Disconnect(con2);
                return ret;
            }
            else
                return execSQL(String.Format(command, Args));
        }

        public static Boolean ExecuteSelect(SqlDataSource SqlDs, params object[] Args)
        {
            return ExecuteCommand(SqlDs, SqlDs.SelectCommand, Args);
        }

        public static Boolean ExecuteInsert(SqlDataSource SqlDs, params object[] Args)
        {
            return ExecuteCommand(SqlDs, SqlDs.InsertCommand, Args);
        }

        public static Boolean ExecuteUpdate(SqlDataSource SqlDs, params object[] Args)
        {
            return ExecuteCommand(SqlDs, SqlDs.UpdateCommand, Args);
        }

        public static Boolean ExecuteDelete(SqlDataSource SqlDs, params object[] Args)
        {
            return ExecuteCommand(SqlDs, SqlDs.DeleteCommand, Args);
        }
        //-----
        public static class Select
        {
            // SCALAR
            public static String Scalar(String Sql, params object[] Args)
            {
                return getScalar(String.Format(Sql, Args));
            }
            public static String Scalar(SqlDataSource SqlDs, params object[] Args)
            {
                //string conString = !String.IsNullOrEmpty(SqlDs.ConnectionString) ? SqlDs.ConnectionString : conStr;
                //return getScalar(Connect(conString), String.Format(SqlDs.SelectCommand, Args));

                //20170214 T
                if (!String.IsNullOrEmpty(SqlDs.ConnectionString))
                {
                    SqlConnection con2 = Connect(SqlDs.ConnectionString);
                    string ret = getScalar(con2, String.Format(SqlDs.SelectCommand, Args));
                    Disconnect(con2);
                    return ret;
                }
                else
                    return getScalar(String.Format(SqlDs.SelectCommand, Args));
            }
            public static String Scalar(SqlConnection con, String Sql, params object[] Args)
            {
                return getScalar(con, String.Format(Sql, Args));
            }
            // SET
            public static DataSet Set(String Sql, params object[] Args)
            {
                return getDataSet(String.Format(Sql, Args));
            }
            public static DataSet Set(SqlDataSource SqlDs, params object[] Args)
            {
                return getDataSet(String.Format(SqlDs.SelectCommand, Args));
            }
            public static DataSet Set(SqlConnection Con, SqlDataSource SqlDs, params object[] Args)
            {
                return getDataSet(Con, String.Format(SqlDs.SelectCommand, Args));
            }
            public static DataSet Set(SqlConnection Con, String Sql, params object[] Args)
            {
                return getDataSet(Con, String.Format(Sql, Args));
            }

            // TABLE
            public static DataTable Table(int Index, String Sql, params object[] Args)
            {
                return getDataSet(String.Format(Sql, Args)).Tables[Index];
            }
            public static DataTable Table(SqlConnection Con, int Index, String Sql, params object[] Args)
            { 
                return getDataSet(Con, String.Format(Sql, Args)).Tables[Index];
            }
            public static DataTable Table(int Index, SqlDataSource SqlDs, params object[] Args)
            {
                //string conString = !String.IsNullOrEmpty(SqlDs.ConnectionString) ? SqlDs.ConnectionString : conStr;
                //return getDataSet(Connect(conString), String.Format(SqlDs.SelectCommand, Args)).Tables[Index];
                string conString = !String.IsNullOrEmpty(SqlDs.ConnectionString) ? SqlDs.ConnectionString : conStr;
                SqlConnection con = Connect(conString);
                DataTable dt = getDataSet(con, String.Format(SqlDs.SelectCommand, Args)).Tables[Index];
                Disconnect(con);
                return dt;
            }
            public static DataTable Table(SqlConnection Con, int Index, SqlDataSource SqlDs, params object[] Args)
            {
                return getDataSet(Con, String.Format(SqlDs.SelectCommand, Args)).Tables[Index];
            }
            public static DataTable Table(String Sql, params object[] Args)
            {
                return Table(0, Sql, Args);
            }
            public static DataTable Table(SqlDataSource SqlDs, params object[] Args)
            {
                return Table(0, SqlDs, Args);
            }
            public static DataTable TableCon(SqlDataSource SqlDs, params object[] Args)
            {
                //20170214 T
                if (!String.IsNullOrEmpty(SqlDs.ConnectionString))
                {
                    SqlConnection con2 = Connect(SqlDs.ConnectionString);
                    DataTable dt = Table(con2, 0, SqlDs, Args);
                    Disconnect(con2);
                    return dt;
                }
                else
                    return Table(con, 0, SqlDs, Args);
            }
            // ROW
            public static DataRow Row(String Sql, params object[] Args)
            {
                DataTable Tbl = Table(Sql, Args);
                if (Tbl.Rows.Count > 0)
                    return Tbl.Rows[0];
                return null;
            }
            public static DataRow Row(SqlDataSource SqlDs, params object[] Args)
            {
                DataTable Tbl = Table(SqlDs, Args); 
                if(Tbl.Rows.Count > 0)
                    return Tbl.Rows[0];
                return null;
            }
            public static DataRow Row(DataTable Table, String Column, object Value)
            {
                foreach(DataRow Row in Table.Rows)
                {
                    if(Row[Column] == Value)
                        return Row;
                }
                return null;
            }
            public static DataRow Row(SqlConnection con, String Sql, params object[] Args)
            {
                DataTable Tbl = Table(con, 0, Sql, Args);
                if (Tbl.Rows.Count > 0)
                    return Tbl.Rows[0];
                return null;
            }
            /* new */
            public static DataRow RowCon(SqlDataSource SqlDs, params object[] Args)
            {
                return TableCon(SqlDs, Args).Rows[0];
            }
            // VALUE
            public static Object Value(String Column, String Sql, params object[] Args)
            {
                return Row(Sql, Args)[Column];
            }
            public static Object Value(String Column, SqlDataSource SqlDs, params object[] Args)
            {
                return Row(SqlDs, Args)[Column];
            }
            public static Boolean Boolean(SqlDataSource SqlDs, params object[] Args)
            {
                String Value = Row(SqlDs, Args)[0].ToString();
                return Value == "1" || Value == "TRUE" || Value == "true";
            }
            public static Boolean Boolean(String Sql, params object[] Args)
            {
                String Value = Row(Sql, Args)[0].ToString(); 
                return Value == "1" || Value == "TRUE" || Value == "true";
            }
        }
        #endregion

        static public string ToSqlValue(object Value)
        {
            if (Value == null || (Value is string && string.IsNullOrEmpty((string)Value)))
                return "NULL";
            return ToSqlValue(Value, Value.GetType());
        }

        static public string ToSqlValue(object Value, Type type)
        {
            if (Value == null || (Value is string && string.IsNullOrEmpty((string)Value)))
                return "NULL";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return Value.ToString();
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return Value.ToString().Replace(",", ".");
                case TypeCode.String:
                    return string.Format("'{0}'", Value);
                case TypeCode.DateTime:
                    return "'" + Tools.DateToStr((DateTime)Value) + "'";
                case TypeCode.Boolean:
                    return ((bool)Value) ? "1" : "0";
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        return ToSqlValue(Value, Nullable.GetUnderlyingType(type));
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        /* 
         * UWAGA UWAGA - 99 rightsow juz jest 
         * 100 - Admin
         * 101 - Kierownik
         * 102 - Raporty
         */
        public const int RightsPudding = 110;
        public static String PrepareRightsForCheckRightsExpression(AppUser User)
        {
            char[] rights = User.Rights.PadRight(RightsPudding, '0').ToCharArray();
            rights[100] = User.IsAdmin ? '1' : '0';
            rights[101] = User.IsKierownik ? '1' : '0';
            rights[102] = User.IsRaporty ? '1' : '0';
            return new String(rights);
            //return User.Rights.PadRight(RightsPudding, '0') + ((User.IsAdmin) ? "1" : "0") + ((User.IsKierownik) ? "1" : "0") + ((User.IsRaporty) ? "1" : "0");
        }

        const string sqlCheckRights = @"select case when Rights is null then 1 else dbo.CheckRightsExpr(isnull({0}, ''), Rights) end from SqlMenu where Id = {1}"; 

        public static Boolean SqlMenuHasRights(int SqlMenuId, AppUser User)
        {
            //bool b = Convert.ToBoolean(Int32.Parse(db.Select.Scalar(sqlCheckRights, ToSqlValue(PrepareRightsForCheckRightsExpression(User)), SqlMenuId)));
            //return b;
            string s= db.Select.Scalar(sqlCheckRights, ToSqlValue(PrepareRightsForCheckRightsExpression(User)), SqlMenuId);
            return s == "1"; // może być "", wywala się konwersja
        }

        public static Boolean SqlMenuHasRights(SqlConnection con, int SqlMenuId, AppUser User)
        {
            string s = db.getScalar(con, String.Format(sqlCheckRights, ToSqlValue(PrepareRightsForCheckRightsExpression(User)), SqlMenuId));
            //bool b = Convert.ToBoolean(s);
            bool b = s == "1";
            return b;
        }


        public static bool ExecuteClientData(string type, params object[] Args)
        {
            DataRow dr = db.Select.Row("select * from ClientData where Typ = '{0}' and Aktywny = 1", type);
            string cstrName = db.getValue(dr, "ConStr");
            string cstr = String.IsNullOrEmpty(cstrName) ? conStr : ConfigurationManager.ConnectionStrings[cstrName].ConnectionString;
            //cstr = ConfigurationManager.ConnectionStrings["PORTAL"].ConnectionString;

            if (dr != null)
            {
                string sql = db.getValue(dr, "Data");
                if (!String.IsNullOrEmpty(sql))
                {
                    try
                    {
                        SqlConnection con = Connect(cstr);
                        bool b = db.execSQL(con, String.Format(sql, Args));
                        Disconnect(con);
                        return b;
                    }
                    catch(Exception ex)
                    {
                        Log.Error(1337, ex.ToString());
                        return false;
                    }
                }
            }
            return true;
        }

        /* NOWY CHECK RIGHTS */

        private static String EvaluateRights(String Values, String Expression)
        {
            const string T = "true";
            const string F = "false";
            // trzeba pomyśleć nad tym
            if (String.IsNullOrEmpty(Expression))
                return T;
            try
            {
                //return Regex.Replace(Expression, @"\b(\d+)", Right => ((Values[Values.Length - 1 - Convert.ToInt32(Right.Value)] == '1') ? "true" : "false"), RegexOptions.Multiline);
                String ret = Regex.Replace(Expression, @"\b(\d+)", Right => (Convert.ToInt32(Right.Value) < Values.Length ? (Values[Convert.ToInt32(Right.Value)] == '1' ? T : F) : F), RegexOptions.Multiline);
                return String.IsNullOrEmpty(ret) ? F : ret;
            }
            catch
            {
                return F;
            }
        }

        //[SqlFunction(DataAccess = DataAccessKind.Read)]
        //[SqlFunction]
        public static Boolean CheckRightsExpr(String Rights, String Expression)
        {
            DataTable dt = new DataTable();
            try
            {
                return ((Boolean)dt.Compute(EvaluateRights(
                    String.IsNullOrEmpty(Rights) ? "" : Rights,
                    String.IsNullOrEmpty(Expression) ? "" : Expression), ""));
            }
            catch
            {
                //return false;
                throw new Exception("Niepoprawna składnia wyrażenia funkcji CheckRightsExpr");
            }
        }
    }
}

/*-----------------------------------------------------------------------------
Style ID Style Type
0 or 100 mon dd yyyy hh:miAM (or PM)
101 mm/dd/yy
102 yy.mm.dd
103 dd/mm/yy
104 dd.mm.yy
105 dd-mm-yy
106 dd mon yy
107 Mon dd, yy
108 hh:mm:ss
9 or 109 mon dd yyyy hh:mi:ss:mmmAM (or PM)
110 mm-dd-yy
111 yy/mm/dd
112 yymmdd
13 or 113 dd mon yyyy hh:mm:ss:mmm(24h)
114 hh:mi:ss:mmm(24h)
20 or 120 yyyy-mm-dd hh:mi:ss(24h)
21 or 121 yyyy-mm-dd hh:mi:ss.mmm(24h)
126 yyyy-mm-dd Thh:mm:ss.mmm(no spaces)
130 dd mon yyyy hh:mi:ss:mmmAM
131 dd/mm/yy hh:mi:ss:mmmAM
*/







