using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

/*-------------------------------------------------------------------------
 
 
 
 -------------------------------------------------------------------------*/
namespace HRRcp.App_Code
{
    public static class Base
    {
        public static string conStr = ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString;

        public const string TRUE = "True";
        public const string FALSE = "False";
        public const string NULL = "null";
        public const string bTRUE = "1";
        public const string bFALSE = "0";

        public const string DateMinValueStr = "1900-01-01";
        public static DateTime DateMinValue = DateTime.Parse(DateMinValueStr);

        //----- mode -----
        public const char moEdit = 'E';     // dozwolona edycja i wstawianie
        public const char moQuery = 'Q';    // tylko podgląd
        public const char moPrint = 'P';    // formatowanie do wydruku

        //public const string sesEditMode = "editMode";

        public const int emQuery = 0;
        public const int emEdit  = 1;
        public const int emCreate = 2;

        //public static bool CreateOrGetSession(Programs p 

        //---------------------------------------------
        public delegate void ResultEventHandler(object sender, ResultEventArgs ea);

        public class ResultEventArgs : EventArgs
        {
            //public static readonly ResultEventArgs True;
            //public static readonly ResultEventArgs False;
            public ResultEventArgs(bool success)
            {
                this.success = success;
                this.error = 0;
                this.message = null;
            }
            public ResultEventArgs(bool success, int error)
            {
                this.success = success;
                this.error = error;
                this.message = null;
            }
            public ResultEventArgs(bool success, int error, string message)
            {
                this.success = success;
                this.error = error;
                this.message = message;
            }
            public bool success;
            public int error;
            public string message;
        }	

        //---------------------------------------------
        public static SqlConnection Connect()
        {
            return Connect(conStr);
        }

        public static SqlConnection Connect(string conStr)
        {
            SqlConnection sqlCon = new SqlConnection(conStr);
            sqlCon.Open();
            return sqlCon;
        }

        public static void Disconnect(SqlConnection sqlCon)
        {
            sqlCon.Close();
        }

        public static void Disconnect(SqlTransaction sqlTrans)
        {
            SqlConnection c = sqlTrans.Connection;
            sqlTrans.Commit();
            c.Close();
        }

        //----------------------------------------------
        public static OdbcConnection odbcConnect(string conStr)
        {
            OdbcConnection odbcCon = new OdbcConnection(conStr);
            odbcCon.Open();
            return odbcCon;
        }

        public static void Disconnect(OdbcConnection odbcCon)
        {
            odbcCon.Close();
        }
        //----------------------------------------------
        public static DataSet getDataSet(SqlConnection sqlCon, string sql)
        {
            return getDataSet(sqlCon, sql, 300);
        }

        public static DataSet getDataSet(SqlConnection sqlCon, string sql, int tout)
        {
            DataSet ds;
            try
            {
                bool f = sqlCon == null;
                if (f) sqlCon = Connect();
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);

                //sqlCmd.CommandTimeout = 300;
                sqlCmd.CommandTimeout = tout;   //20161102

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                ds = new DataSet();
                da.Fill(ds);
                if (f) Disconnect(sqlCon);
            }
            catch (Exception ex)// zamysl jest taki zeby nie trzeba bylo wywolywac try finally close to zamykamy w przypadku bledu 
            {
                sqlCon.Close();
                Log.Error(Log.t2SQL, sql, ex.Message);
                throw;
            }
            return ds;
        }

        public static DataSet getDataSet(OdbcConnection odbcCon, string sql)
        {
            DataSet ds;
            try
            {
                OdbcCommand odbcCmd = new OdbcCommand(sql, odbcCon);
                OdbcDataAdapter da = new OdbcDataAdapter(odbcCmd);
                ds = new DataSet();
                da.Fill(ds);
            }
            catch (Exception ex) // zamysl jest taki zeby nie trzeba bylo wywolywac try finally close to zamykamy w przypadku bledu 
            {
                odbcCon.Close();
                Log.Error(Log.t2SQL, sql, ex.Message); 
                throw;
            }
            return ds;
        }

        
        
        
        
        





        public static DataSet getDataSet(SqlTransaction tr, string sql)
        {
            if (tr == null)
                return getDataSet(sql);    // tak jest bardziej uniwersalnie ...
            else
            {
                DataSet ds;
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(sql, tr.Connection, tr);
                    SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                    ds = new DataSet();
                    da.Fill(ds);
                }
                catch (Exception ex) // zamysl jest taki zeby nie trzeba bylo wywolywac try finally close to zamykamy w przypadku bledu 
                {
                    tr.Rollback();
                    tr.Connection.Close();
                    Log.Error(Log.t2SQL, sql, ex.Message);
                    throw;
                }
                return ds;
            }
        }

        public static DataSet getDataSet(string sql)
        {
            SqlConnection c = Connect();
            DataSet ds = getDataSet(c, sql);
            Disconnect(c);
            return ds;
        }
        /*
        {
            Connection c = new Connection();
            DataSet ds = c.getDataSet(sql);
            c.Disconnect();
            return ds;
        }
        */

        public static DataRow getDataRow(SqlConnection con, string sql)
        {
            DataSet ds = getDataSet(con, sql);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }

        public static DataRow getDataRow(SqlTransaction tr, string sql)
        {
            DataSet ds = getDataSet(tr, sql);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }

        public static DataRow getDataRow(string sql)
        {
            SqlConnection c = Connect();
            DataRow dr = getDataRow(c, sql);
            Disconnect(c);
            return dr;
        }

        public static DataRow getDataRow(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }

        public static DataRow getDataRow(DataSet ds, int index)
        {
            if (index < ds.Tables[0].Rows.Count)
                return ds.Tables[0].Rows[index];
            else
                return null;
        }
        //------------------
        public static string getScalar(string sql)  // zwraca pierwsza wartosc 
        {
            return getScalar(sql, true);
        }

        public static string getScalar(SqlConnection con, string sql)  // zwraca pierwsza wartosc 
        {
            return getScalar(con, sql, true);
        }

        public static string getScalar(SqlConnection con, string sql, bool logError)  // zwraca pierwsza wartosc 
        {
            string s;
            try
            {
                bool f = con == null;
                if (f) con = Connect();
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                s = Convert.ToString(sqlCmd.ExecuteScalar());
                if (f) Disconnect(con);
            }
            catch (Exception ex)
            {
                con.Close();
                if (logError) Log.Error(Log.t2SQL, sql, ex.Message);
                throw;
            }
            return s;
        }

        public static string getScalar(string sql, bool logError)  // zwraca pierwsza wartosc 
        {
            SqlConnection c = Connect();
            string s = getScalar(c, sql, logError);
            Disconnect(c);
            return s;
        }

        public static string getScalar(SqlTransaction sqlTrans, string sql)  // zwraca pierwsza wartosc 
        {
            string s;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, sqlTrans.Connection, sqlTrans);
                s = Convert.ToString(sqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                sqlTrans.Rollback();
                if (sqlTrans.Connection != null)
                    sqlTrans.Connection.Close();
                Log.Error(Log.t2SQL, sql, ex.Message);
                throw;
            }
            return s;
        }

        /*
                public static DataSet getDataSet(string sql)
                {
                    DataSet ds = null;
                    SqlConnection sqlCon = new SqlConnection(conStr);
                    sqlCon.Open();
                    try
                    {
                        SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                        ds = new DataSet();
                        da.Fill(ds);
                    }
                    finally`
                    {
                        sqlCon.Close();
                    }
                    return ds;
                }

                public static DataRow getDataRow(string sql)
                {
                    DataSet ds = getDataSet(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                        return ds.Tables[0].Rows[0];
                    else
                        return null;
                }

                public static string getScalar(string sql)  // zwraca pierwsza wartosc 
                {
                    string s = null;
                    SqlConnection sqlCon = new SqlConnection(conStr);
                    sqlCon.Open();
                    try
                    {
                        SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                        s = Convert.ToString(sqlCmd.ExecuteScalar());
                        //Convert.ToInt32(
                    }
                    finally
                    {
                        sqlCon.Close();
                    }
                    return s;
                }
 
         */

        public static DataSet getDataSetIfNull(DataSet ds, bool forceExecute, string sql)
        {
            if (ds == null || forceExecute)
                ds = Base.getDataSet(sql);
            return ds;
        }

        public static DataSet getDataSet(ref DataSet ds, ref string strErr, bool forceExecute, string sql)
        {
            if (ds == null || forceExecute)
            {
                try
                {
                    ds = Base.getDataSet(sql);
                }
                catch (Exception ex)
                {
                    if (strErr != null) 
                        strErr = ex.Message;
                }
            }
            return ds;
        }
        //-------------------------------------------
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

        public static int getCount(DataSet ds)
        {
            return getRows(ds).Count;
        }
        //-------------------------------------------
        /*
        public static string getDataStr(SqlConnection con, string sql)
        {
            DataSet ds = getDataSet(con, sql);
            DataRow dr = getDataRow(ds);
            if (dr != null)
            {
                string[] sa = new string[ds.Tables[0].Columns.Count];
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    sa[i] = dr[i].ToString().Trim();
                return String.Join(",", sa);
            }
            else return null;
        }
        */
        public static string getDataStr(SqlConnection con, string sql)
        {
            string[] sa = getDataStringArray(con, sql);
            if (sa != null)
                return String.Join(",", sa);
            else
                return null;
        }

        public static string[] getDataStringArray(SqlConnection con, string sql)
        {
            DataSet ds;
            return getDataStringArray(con, sql, out ds);
        }

        public static string[] getDataStringArray(SqlConnection con, string sql, out DataSet ds)
        {
            ds = getDataSet(con, sql);
            DataRow dr = getDataRow(ds);
            if (dr != null)
            {
                string[] sa = new string[ds.Tables[0].Columns.Count];
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    sa[i] = dr[i].ToString().Trim();
                return sa;
            }
            else return null;
        }

        public static string getDataCompare(SqlConnection con, string sql, string[] prev)  // porównuje z prev (getDataStringArray) i pokazuje zmiany, null jak bez zmian
        {
            string ret = null;
            DataSet ds;
            string[] sa = getDataStringArray(con, sql, out ds);
            if (sa != null)
                for (int i = 0; i < sa.Length; i++)
                {
                    string p = prev != null && i < prev.Length ? prev[i] : "";
                    if (p != sa[i]) 
                        ret += ds.Tables[0].Columns[i].Caption + ": " + p + " -> " + sa[i] + "\n";
                }
            return ret;
        }
        //-------------------------------------------
        public static string getValue(DataSet ds, int fieldNo)  // z pierwszego wiersza
        {
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][fieldNo].ToString();
            else
                return null;
        }

        public static string getValue(DataSet ds, string fieldName)
        {
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][fieldName].ToString();
            else
                return null;
        }

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

        public static bool getBool(DataSet ds, string fieldName, bool nullValue)
        {
            return getBool(getDataRow(ds), fieldName, nullValue);
        }
        //-------
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

        //-------
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

        public static DateTime getDateTime(DataSet ds, string fieldName, DateTime nullValue)
        {
            return getDateTime(getDataRow(ds), fieldName, nullValue);
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
                    catch 
                    { 
                    }
            }
            return null;
        }

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

        public static bool isNull(DataSet ds, string fieldName)
        {
            return isNull(getDataRow(ds), fieldName);
        }
        //-------------------------------------------
        public static int execSQLEx(string sql)
        {
            return execSQLEx((SqlConnection)null, sql);
        }

        public static int execSQLEx(SqlConnection sqlCon, string sql)  // ilość przetworzonych rekordów lub -1 jak duplicate key
        {
            int ret = 0;
            bool f = sqlCon == null;    
            try
            {
                if (f) sqlCon = Connect();
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                ret = sqlCmd.ExecuteNonQuery();  // >= 1 sukces - ilość przetworzonych rekordów
                if (f) Disconnect(sqlCon);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                switch (ex.Number)  //http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlexception(v=vs.85).aspx
                {
                    case 2627:
                    case 2601:
                        ret = -1;
                        if (f) Disconnect(sqlCon);
                        break;
                    default:
                        Disconnect(sqlCon);
                        Log.Error(Log.t2SQL, sql);
                        throw;
                }
            }
            return ret;
        }
        //-------------------------------------------
        /*
        public static bool execSQL(string sql)
        {
            SqlConnection c = Connect();
            bool ret = execSQL(c, sql);
            Disconnect(c);
            return ret;
        }
        */

        public static bool execSQL(string sql)
        {
            return execSQL((SqlConnection)null, sql);
        }

        public static bool execSQL(SqlConnection sqlCon, string sql)
        {
            int success = 0;
            try
            {
                bool f = sqlCon == null;    //20110529 zeby wyzej nie trzeba bylo
                if (f) sqlCon = Connect();
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
                if (f) Disconnect(sqlCon);
            }
            catch (Exception ex)
            {
                //sqlCon.Close();  to samo co poniżej
                Disconnect(sqlCon);
                Log.Error(Log.t2SQL, sql, ex.Message);
                throw;
            }
            return success >= 1;
        }

        public static bool execSQL2(SqlConnection sqlCon, string sql) //siedzi cicho, do updatów po których insert jeśli false; mozna by jeszcze sie upewnić i spr exception czy wlasciwy (record not found)
        {
            int success = 0;
            try
            {
                bool f = sqlCon == null;    //20110529 zeby wyzej nie trzeba bylo
                if (f) sqlCon = Connect();
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
                if (f) Disconnect(sqlCon);
            }
            catch
            {
            }
            return success >= 1;
        }

        public static bool execSQL2tr(SqlTransaction sqlTrans, string sql) //siedzi cicho, do updatów po których insert jeśli false; mozna by jeszcze sie upewnić i spr exception czy wlasciwy (record not found)
        {
            if (sqlTrans == null)
                return execSQL2(null, sql);    // tak jest bardziej uniwersalnie ...
            else
            {
                int success = 0;
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(sql, sqlTrans.Connection, sqlTrans);
                    success = sqlCmd.ExecuteNonQuery();  // 1 sukces
                }
                catch (Exception ex)
                {
                }
                return success >= 1;
            }
        }

        public static int execSQLcnt(SqlConnection sqlCon, string sql)
        {
            int success = 0;
            try
            {
                bool f = sqlCon == null;    //20110529 zeby wyzej nie trzeba bylo
                if (f) sqlCon = Connect();
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
                if (f) Disconnect(sqlCon);
            }
            catch (Exception ex)
            {
                //sqlCon.Close();  to samo co poniżej
                Disconnect(sqlCon);
                Log.Error(Log.t2SQL, sql, ex.Message);
                throw;
            }
            return success;
        }

        public static bool execSQL(SqlTransaction sqlTrans, string sql)
        {
            if (sqlTrans == null)
                return execSQL(sql);    // tak jest bardziej uniwersalnie ...
            else
            {
                int success = 0;
                try
                {
                    //----- TESTY -----
                    #if TEST
                    if (sqlTrans.Connection == null)
                    {
                        int x = 5;  // tu załóżyć breakpointa i czekac ...
                    }
                    #endif
                    //----- TESTY -----
                    SqlCommand sqlCmd = new SqlCommand(sql, sqlTrans.Connection, sqlTrans);
                    success = sqlCmd.ExecuteNonQuery();  // 1 sukces
                }
                catch (Exception ex)
                {
                    sqlTrans.Rollback();
                    if (sqlTrans.Connection != null)
                        sqlTrans.Connection.Close();
                    Log.Error(Log.t2SQL, sql, ex.Message);
                    throw;
                }
                return success >= 1;
            }
        }

        public static bool execSQL(string sql, string par1)
        {
            int success = 0;
            SqlConnection sqlCon = new SqlConnection(conStr);
            sqlCon.Open();
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.Parameters.AddWithValue("@1", par1);
                success = sqlCmd.ExecuteNonQuery();  // 1 sukces
            }
            finally
            {
                sqlCon.Close();
            }
            return success >= 1;
        }

        public static bool execSQL(string sql, ref string strErr) // sp jak str == "" czy to nie jest null
        {
            int success = 0;
            try
            {
                SqlConnection sqlCon = new SqlConnection(conStr);
                sqlCon.Open();
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                    success = sqlCmd.ExecuteNonQuery();  // 1 sukces
                }
                finally
                {
                    sqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                if (strErr != null)
                    strErr = ex.Message;
            }
            return success >= 1;
        }

        public static int insertSQL(string sql) // zwraca last Id lub -1 jak duplicate index
        {
            SqlConnection sqlCon = Connect();
            int ret = insertSQL(sqlCon, sql, true, true, null);
            Disconnect(sqlCon);
            return ret;
        }

        public static int insertSQL(string sql, bool logError) // zwraca last Id lub -1 jak duplicate index
        {
            SqlConnection sqlCon = Connect();
            int ret = insertSQL(sqlCon, sql, logError, true, null);
            Disconnect(sqlCon);
            return ret;
        }

        public static int insertSQL(SqlConnection sqlCon, string sql) // zwraca last Id lub -1 jak duplicate index
        {
            return insertSQL(sqlCon, sql, true, true, null);
        }

        public static bool insertSQL2(SqlConnection sqlCon, string sql) // nie pobiera identity; jak dup to false
        {
            return insertSQL(sqlCon, sql, true, false, null) == 1;
        }

        public static int insertSQL(SqlConnection sqlCon, string sql, AppUser user) // zwraca last Id lub -1 jak duplicate index
        {
            return insertSQL(sqlCon, sql, true, true, user);
        }

        public static int insertSQL(SqlConnection sqlCon, string sql, bool logError, bool getIdentity, AppUser user) // zwraca last Id lub -1 jak duplicate index
        {
            int ret = -1;
            int success = 0;

            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                try
                {
                    success = sqlCmd.ExecuteNonQuery();  // 1 sukces, ilość przetworzonych wierszy
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
                            throw;
                    }
                }
                if (success == 1)    // ilosc przetworzonych wierszy == 1 - z regóły wstawiamy 1 rekord 
                    if (getIdentity)
                    {
                        sqlCmd.CommandText = "select @@Identity";
                        ret = Convert.ToInt32(sqlCmd.ExecuteScalar());
                    }
                    else ret = 1;
            }
            catch (Exception ex)
            {
                sqlCon.Close();
                if (logError) 
                    if (user != null)
                        Log.Error(user, Log.t2SQL, sql, ex.Message);
                    else 
                        Log.Error(Log.t2SQL, sql, ex.Message);
                throw;
            }
            return ret;
        }

        public static int insertSQL(SqlTransaction tr, string sql) // zwraca last Id lub -1 jak duplicate index
        {
            int ret = -1;
            int success = 0;

            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, tr.Connection, tr);
                try
                {
                    success = sqlCmd.ExecuteNonQuery();  // 1 sukces, ilość przetworzonych wierszy
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
                            throw;
                    }
                }
                if (success == 1)    // ilosc przetworzonych wierszy == 1 - z regóły wstawiamy 1 rekord 
                {
                    sqlCmd.CommandText = "select @@Identity";
                    ret = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                tr.Rollback();
                if (tr.Connection != null)
                    tr.Connection.Close();
                Log.Error(Log.t2SQL, sql, ex.Message);
                throw;
            }
            return ret;
        }
        
        //--------------------------------------------
        public static void DropIfExists(SqlConnection con, string tbName)
        {
            execSQL(con, "IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tbName + "') DROP TABLE " + tbName);
        }

        public static void DropIfExists(SqlTransaction tr, string tbName)
        {
            execSQL(tr, "IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tbName + "') DROP TABLE " + tbName);
            /*
             IF OBJECT_ID('t1') IS NOT NULL
                DROP TABLE t1
             */
        }
        //--------------------------------------------
        public static string UniqueId()
        {
            return DateTime.Now.ToString("fff.yyyyMMddHHmmss") + HttpContext.Current.Session.SessionID;
        }

        public static string UniqueId(int maxLen)
        {
            string uid = UniqueId();
            if (uid.Length > maxLen)
                return uid.Substring(0, maxLen);
            else
                return uid;
        }
        //--------------------------------------------
        /*
        public static SqlDataReader getDataReader(string sql)
        {
            DataSet ds = null;
            SqlConnection sqlCon = new SqlConnection(conStr);
            sqlCon.Open();
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                SqlDataReader data = sqlCmd.ExecuteReader();

                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                ds = new DataSet();
                da.Fill(ds);
            }
            finally
            {
                sqlCon.Close();
            }
            return ds;
        }
        */
 
        //--------------------------------------------
        /*
        public static void SetEditMode(int em)
        {
            HttpContext.Current.Session[sesEditMode] = em;
        }

        public static bool IsEditMode(int em)
        {
            int sem;
            if (HttpContext.Current.Session[sesEditMode] == null)
                return em == emQuery;
            else
            {
                sem = (int)HttpContext.Current.Session[sesEditMode];
                return em == sem;
            }
        }

        public static void SetQueryMode()
        {
            SetEditMode(emQuery);
        }

        public static void SetEditMode()
        {
            SetEditMode(emEdit);
        }

        public static void SetCreateMode()
        {
            SetEditMode(emCreate);
        }

        public static bool IsQueryMode()
        {
            return IsEditMode(emQuery);
        }

        public static bool IsEditMode()
        {
            return IsEditMode(emEdit);
        }

        public static bool IsCreateMode()
        {
            return IsEditMode(emCreate);
        }
         */
        //------------------------------------------------
        public static string DateToStr(object d, string format)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            if (d.Equals(DBNull.Value))
                return null;
            else
                return Convert.ToDateTime(d).ToString(format);
        }

        public static string DateToStr(object d)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            if (d == null || d.Equals(DBNull.Value))
                return null;
            else
                return DateToStr(Convert.ToDateTime(d));
        }

        public static string DateTimeToStr(object d, string format)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            if (d.Equals(DBNull.Value))
                return null;
            else
                return ((DateTime)d).ToString(format);
            //return d.ToString();  // plomba
            //return DateToStr(d, "yyyy-MM-dd HH:mm:ss");
        }


        public static string DateTimeToStr(object d)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            return DateTimeToStr(d, "yyyy-MM-dd HH:mm:ss");
        }

        public static string DateTimeToStrHHMM(object d)  // przy pobieraniu daty z bazy, d -> DataRow["data"]
        {
            return DateTimeToStr(d, "yyyy-MM-dd HH:mm");
        }

        public static string DateToStr(DateTime date)   // dla zmiennej typu data
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string TimeToStr(DateTime time)   
        {
            return time.ToString("HH:mm:ss");
        }

        public static string SqlStrToDate(string d)  // przy zapisie do bazy 
        {
            if (String.IsNullOrEmpty(d))
                return Base.NULL;
            else
                return strParam(d);
        }

        public static int StrToIntDef(string value, int def)
        {
            try
            {
                return Int32.Parse(value);
            }
            catch
            {
                return def;
            }
        }
        //----------------
        public static DateTime StrToDateTime(string date)
        {
            DateTime dt;
            StrToDateTime(date, out dt);
            return dt;
        }
        /*
        public static bool StrToDateTime(string date, out DateTime dt)
        {
            try
            {
                dt = Convert.ToDateTime(date);
                return true;
            }
            catch (Exception ex)
            {
                dt = DateTime.MinValue;
                return false;
            }
        }
        */
        public static bool StrToDateTime(string date, out DateTime dt)
        {
            return DateTime.TryParse(date, out dt);
        }

        public static TimeSpan StrToTimeSpan(string time)
        {
            TimeSpan ts;
            if (StrToTimeSpan(time, out ts))
                return ts;
            else
                return TimeSpan.MinValue;
        }

        public static bool StrToTimeSpan(string time, out TimeSpan ts)
        {
            return TimeSpan.TryParse(time, out ts);
        }
        //--------------------------------------------------------------------
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

        public static string sqlCutDate(string field)  // odcina czas z daty, zwraca DateTime do wstawnienia w zapytanie sql dlatego na stringu                           
        {
            return "convert(datetime," + sqlGetDate(field) + ")";                // yyyy-MM-DD as SQL DateTime
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

        //------------------------------------------------
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
        //------------------------------------------------
        public static string updateSql(string tbName, int start, string fields, string where, params object[] list)
        {
            return updateSql2(tbName, start, fields, null, where, list);
        }

        public static string insertSql(string tbName, int start, string fields, params object[] list)
        {
            return insertSql2(tbName, start, fields, null, null, list);
        }

        public static string updateSql2(string tbName, int start, string fields, string extraSet, string where, params object[] list)  // update tbName set f1={0},f2={1} [,xx=vv] where...
        {
            return String.Format("update " + tbName + " set " + PrepareUpdateParams(start, fields) + extraSet +
                        (!String.IsNullOrEmpty(where) ? " where " + where : null), list);
        }

        public static string insertSql2(string tbName, int start, string fields, string extraFields, string extraValues, params object[] list)  // update tbName set f1={0},f2={1} where...
        {
            string[] ff = fields.Split(',');
            return String.Format("insert into " + tbName + " (" + fields + extraFields + ") values (" + PrepareInsertParams(start, ff.Count()) + extraValues + ")", list);
        }

        //-------------------------------------------------
        public static string strParam(string param)
        {
            return "'" + param + "'";
        }

        public static string strParam2(string param)  //20110826, do ankiety
        {
            return "'" + param.Replace("'", "''") + "'";
        }

        public static string dateParam(DateTime date)
        {
            return strParam(DateToStr(date));
        }

        public static string boolParam(bool value)
        {
            return value ? "1" : "0";
        }

        public static bool getBoolParam(string boolParam)
        {
            return boolParam == "1";
        }

        public static string updateStrParam(string field, string param)    // do polecenia sql update ... składa pole = wartosc uwzgledniajac null
        {
            return field + " = " + strParam(param) + ",";
        }

        public static string updateStrParamLast(string field, string param)
        {
            return field + " = " + strParam(param) + " "; 
        }

        public static string updateDateParam(string field, string date)    // do polecenia sql update ... składa pole = wartosc uwzgledniajac null
        {
            if (String.IsNullOrEmpty(date))
                return field + " = null,";
            else
                return field + " = " + strParam(date) + ",";
        }

        public static string updateDateParamLast(string field, string date)
        {
            if (String.IsNullOrEmpty(date))
                return field + " = null ";
            else
                return field + " = " + strParam(date) + " ";
        }

        /*
        public static string updateStrParam(string field, string param)    // do polecenia sql update ... składa pole = wartosc uwzgledniajac null
        {
            if (param == null)  // tu ma byc null a nie String.IsNullOrEmpty, jak bedzie pusty string to zapisze string
                return field + " = null,";
            else
                return field + " = " + strParam(param) + ",";
        }

        public static string updateStrParamLast(string field, string param)
        {
            if (param == null)  // tu ma byc null a nie String.IsNullOrEmpty, jak bedzie pusty string to zapisze string
                return field + " = null";
            else
                return field + " = " + strParam(param);
        }
        */

        public static string updateParam(string field, string param)
        {
            if (String.IsNullOrEmpty(param))    // tu ma byc IsNullOrEmpty
                param = NULL;
            return field + " = " + param + ",";
        }

        public static string updateParamLast(string field, string param)
        {
            if (String.IsNullOrEmpty(param))
                param = NULL;
            return field + " = " + param + " ";
        }

        public static string updateParam(string field, bool param)
        {
            return field + " = " + (param ? "1" : "0") + ",";
        }

        public static string updateParamLast(string field, bool param)
        {
            return field + " = " + (param ? "1" : "0") + " "; 
        }

        public static string updateParam(string field, int? param)
        {
            return field + " = " + (param == null ? NULL : param.ToString())+ ",";
        }

        public static string updateParamLast(string field, int? param)
        {
            return field + " = " + (param == null ? NULL : param.ToString()) + " ";
        }

        /*
        public static string nullParam(string param)
        {
            if (String.IsNullOrEmpty(param))
                return NULL;
            else
                return param;
        }

        public static string nullParam(int? param)
        {
            if (param == null)
                return NULL;
            else
                return param.ToString();
        }
        */

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

        public static string nullStrParam(string param)
        {
            if (String.IsNullOrEmpty(param))
                return NULL;
            else
                return strParam(param);
        }

        public static string nullDateParam(string date)
        {
            if (String.IsNullOrEmpty(date))
                return NULL;
            else
                return strParam(date);
        }
        //----------------------------------------
        public static string insertParam(string param)
        {
            if (String.IsNullOrEmpty(param))    // tu ma byc IsNullOrEmpty
                param = NULL;
            return param + ",";
        }

        public static string insertParamLast(string param)
        {
            if (String.IsNullOrEmpty(param))
                param = NULL;
            return param + " ";
        }

        public static string insertStrParam(string param)
        {
            if (param == null)    // tu ma byc IsNullOrEmpty
                param = NULL;
            else param = strParam(param);
            return param + ",";
        }

        public static string insertStrParamLast(string param)
        {
            if (param == null)
                 param = NULL;
            else param = strParam(param);
            return param + " ";
        }

        //--------------
        public static string insertParam(DataRow dr, string paramName)
        {
            string param = getValue(dr, paramName);
            if (String.IsNullOrEmpty(param))    // tu ma byc IsNullOrEmpty
                param = NULL;
            return param + ",";
        }

        public static string insertParamLast(DataRow dr, string paramName)
        {
            string param = getValue(dr, paramName);
            if (String.IsNullOrEmpty(param))    // tu ma byc IsNullOrEmpty
                param = NULL;
            return param + " ";
        }

        public static string insertStrParam(DataRow dr, string paramName)
        {
            string param = getValue(dr, paramName);
            if (String.IsNullOrEmpty(param))    // tu ma byc IsNullOrEmpty
                param = NULL;
            else param = strParam(param);
            return param + ",";
        }

        public static string insertStrParamLast(DataRow dr, string paramName)
        {
            string param = getValue(dr, paramName);
            if (String.IsNullOrEmpty(param))    // tu ma byc IsNullOrEmpty
                param = NULL;
            else param = strParam(param);
            return param + " ";
        }

        public static string insertDateParam(DataRow dr, string paramName)
        {
            string param = getValue(dr, paramName);
            if (String.IsNullOrEmpty(param))  
                param = NULL;
            else param = strParam(param);
            return param + ",";
        }

        public static string insertDateParamYYYYMMDD(DataRow dr, string paramName)
        {
            string param = DateToStr(dr[paramName]);
            if (String.IsNullOrEmpty(param))
                param = NULL;
            else param = strParam(param);
            return param + ",";
        }

        public static string insertDateParam2(DataRow dr, string paramName) // przetestować !!! 
        {
            string param;
            object o = dr[paramName];
            if (o.Equals(DBNull.Value))
                param = NULL;
            else if (Convert.ToInt32(o) == 0)  // to sie moze wywalic ...
                param = NULL;
            else
                param = strParam(o.ToString());
            return param + ",";
        }

        public static string insertFloatParam(DataRow dr, string paramName)
        {
            string param = getValue(dr, paramName);
            if (String.IsNullOrEmpty(param))    // tu ma byc IsNullOrEmpty
                param = NULL;
            return param.Replace(',','.') + ",";
        }

        //----------------------------------------
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

        public static string sqlGet(string s)
        {
            return s;
        }

        //----------------------------------------
        public static string MonthName(int month)   // takie sobie rozwiazanie ale prosciej niz za kazdym razem robić array bo wtglada na to ze nie mozna jako stała zadeklarować
        {
            switch (month)
            {
                case 1: return "styczeń";
                case 2: return "luty";
                case 3: return "marzec";
                case 4: return "kwiecień";
                case 5: return "maj";
                case 6: return "czerwiec";
                case 7: return "lipiec";
                case 8: return "sierpień";
                case 9: return "wrzesień";
                case 10: return "październik";
                case 11: return "listopad";
                case 12: return "grudzień";
                default: return month.ToString();
            }
        }

        public static string AddYear(object d, int no)      // DataRow[n]
        {
            if (d.Equals(DBNull.Value))
                return null;
            else
                return DateToStr(Convert.ToDateTime(d).AddYears(no));
        }
        //------------------------------------------
        public static bool ErrEmpty(string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool ErrDate(string date)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(date);
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
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
                    tr = Connect().BeginTransaction("IMP1");    // tylko jak są jakieś dane zaczynam transakcję
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
                    Disconnect(tr);
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
                                tr = Connect().BeginTransaction("RB1");         // tylko jak są jakieś dane zaczynam transakcję
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
                        Disconnect(tr);
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

        //-------------------------------------------
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
-------------------------------------------------------------------------------
    <!-- group box -->
    <table class="GroupBox1" width="600">
        <tr>
            <td class="tl" ></td>
            <td class="th" >
                <div class="title">INFORMACJE</div>
            </td>
            <td class="tr" ></td>
        </tr>
        <tr>
            <td class="vl"></td>
            <td>
            <!-- group box content -->
            
            <!-- group box end content -->
            </td>
            <td class="vr" ></td>
        </tr>
        <tr>
            <td class="bl"></td>
            <td class="bh"></td>
            <td class="br"></td>
        </tr>
    </table>
    <!-- group box end -->
-------------------------------------------------------------------------------*/
