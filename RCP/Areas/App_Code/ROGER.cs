using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Collections.Generic;
using HRRcp.App_Code;

//http://msdn.microsoft.com/en-us/library/windows/desktop/ms711711(v=VS.85).aspx
//    IdOffset : integer;    // ustawić na róznice: rcp.max - roger.min + 1

namespace HRRcp.App_Code
{
    public class ROGER
    {
        public static string conStrROGER = ConfigurationManager.ConnectionStrings["ROGERConnectionString"].ConnectionString;
        const int maxErrors = 1;

        public static void GetDoorTypes(OdbcConnection conROGER, out string inT, out string outT, out string dutyT, int LogParentId)
        {
            DataSet dsT = Base.getDataSet(conROGER, "select Type, Duty, Direction from 'Reader Types'");
            inT = null;
            outT = null;
            dutyT = null;
            foreach (DataRow dr in dsT.Tables[0].Rows)
            {
                int type = Base.getInt(dr, 0);
                int duty = Base.getInt(dr, 1);
                int dir = Base.getInt(dr, 2);
                switch (dir)
                {
                    case 0:
                        if (String.IsNullOrEmpty(inT))
                            inT = type.ToString();
                        else
                            inT += "," + type.ToString();
                        break;
                    case 1:
                        if (String.IsNullOrEmpty(inT))
                            outT = type.ToString();
                        else
                            outT += "," + type.ToString();
                        break;
                }
                if (duty == 1)
                    if (String.IsNullOrEmpty(inT))
                        dutyT = type.ToString();
                    else
                        dutyT += "," + type.ToString();
            }
            bool e = false;
            if (String.IsNullOrEmpty(inT))  // wartosci domyslne
            {
                inT = "0";
                e = true;
            }
            if (String.IsNullOrEmpty(outT))
            {
                outT = "16,17";
                e = true;
            }
            if (String.IsNullOrEmpty(dutyT))
            {
                dutyT = "17,32";
                e = true;
            }
            //if (e) Log.Error(types!)
        }

        public static void GetDoorTypes(OdbcConnection conROGER, out List<int> inT, out List<int> outT, out List<int> dutyT, int LogParentId)
        {
            inT = new List<int>();
            outT = new List<int>();
            dutyT = new List<int>();

            //OdbcCommand odbcCmd = new OdbcCommand("select Type, Duty, Direction from 'Reader Types'", conROGER);
            OdbcCommand odbcCmd = new OdbcCommand("select Type, Duty, Direction from `Reader Types`", conROGER);
            OdbcDataReader data = odbcCmd.ExecuteReader();
            try
            {
                while (data.Read())
                {
                    try
                    {
                        int type = data.GetInt32(0);
                        int duty = data.GetInt32(1);
                        int dir = data.GetInt32(2);
                        switch (dir)
                        {
                            case 0:
                                inT.Add(type);
                                break;
                            case 1:
                                outT.Add(type);
                                break;
                        }
                        if (duty == 1)
                            dutyT.Add(type);
                    }
                    catch
                    {
                        //Log.Error(aborted); albo odwinąć transakcję
                        break;
                    }
                }
            }
            finally
            {
                data.Close();
            }
            bool e = false;
            if (inT.Count == 0)  // wartosci domyslne
            {
                inT.Add(0);
                e = true;
            }
            if (outT.Count == 0)
            {
                outT.Add(16);
                outT.Add(17);
                e = true;
            }
            if (dutyT.Count == 0)
            {
                dutyT.Add(17);
                dutyT.Add(32);
                e = true;
            }
            if (e) Log.Error(Log.t2APP_IMPORTRCP, LogParentId, "GetDoorTypes", "");
        }

        public static int ImportROGERData(OdbcConnection conROGER, SqlConnection con, int LogParentId)
        {
            const string sql = "SELECT ECUniqueID, ECDate, ECCode, ECUserID, ECReaderID, ECDoorType, { fn CONVERT(ECTime, SQL_VARCHAR) } as stime FROM EventsCache where ECUniqueID > ";
            
            List<int> inT;
            List<int> outT;
            List<int> dutyT;


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 1", null, Log.OK); // dup index, ale nic sie nie stało

            
            GetDoorTypes(conROGER, out inT, out outT, out dutyT, LogParentId);



            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 2", null, Log.OK); // dup index, ale nic sie nie stało



            string lastUID = Base.getScalar(con, "select max(ECUniqueID) from RCP");
            if (String.IsNullOrEmpty(lastUID)) lastUID = "0";

            int cnt = 0;
            int ecnt = 0;


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 3", null, Log.OK); // dup index, ale nic sie nie stało
            
            
            OdbcCommand odbcCmd = new OdbcCommand(sql + lastUID, conROGER);


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 4", null, Log.OK); // dup index, ale nic sie nie stało
            

            OdbcDataReader data = odbcCmd.ExecuteReader();


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 5", null, Log.OK); // dup index, ale nic sie nie stało
            
            try
            {
                while (data.Read())
                {
                    int uid = -1;
                    try
                    {
                        string date = data.GetString(1);
                        if (!String.IsNullOrEmpty(date))
                        {
                            if (date.Length > 10)
                                date = date.Remove(10);
                            else
                            {
                                int y = 0;
                            }
                        }
                        else
                        {
                            int x = 0;
                        }

                        string time;
                        uid = data.GetInt32(0);
                        int code = data.GetInt32(2);
                        int user = data.GetInt32(3);
                        int reader = data.GetInt32(4);
                        int door = data.GetInt32(5);
                        try
                        {
                            time = data.GetString(6);
                        }
                        catch
                        {
                            time = "00:00:00";  // wyszło ze tylko tak to można zrobić - odczyt nawet GetValue:object się wywala !
                        }
                        DateTime dt = DateTime.Parse(date + " " + time);

                        string inout;
                        if (inT.Contains(door)) inout = "0";  // in
                        else if (outT.Contains(door)) inout = "1";   // out
                        else inout = Base.NULL;

                        string duty = dutyT.Contains(door) ? "1" : "0";  // służbowe true:false

                        bool ok = Base.insertSQL2(con, "insert into RCP (ECUniqueID ,Czas, ECCode, ECUserId, ECReaderId, ECDoorType, InOut, Duty) values (" +
                            Base.insertParam(uid.ToString()) +
                            Base.insertStrParam(Base.DateTimeToStr(dt)) +
                            Base.insertParam(code.ToString()) +
                            Base.insertParam(user.ToString()) +
                            Base.insertParam(reader.ToString()) +
                            Base.insertParam(door.ToString()) +
                            Base.insertParam(inout) +
                            Base.insertParamLast(duty) +
                        ")");

                        if (ok) cnt++;
                        else
                        {
                            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Duplicate index", uid.ToString(), Log.OK); // dup index, ale nic sie nie stało
                        }
                    }
                    catch (Exception ex)
                    {
                        ecnt++;
                        Log.Error(Log.t2APP_IMPORTRCP, LogParentId, "Import data exception #" + ecnt.ToString() + ": " + ex.Message, uid.ToString());
                        if (ecnt >= maxErrors)  //albo odwinąć transakcję?
                        {
                            Log.Error(Log.t2APP_IMPORTRCP, LogParentId, "Import data aborted", null);
                            cnt = -cnt;
                            break;
                        }
                    }
                }
            }
            finally
            {
                data.Close();
            }


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 6", null, Log.OK); // dup index, ale nic sie nie stało

            return cnt;
        }





        public static int ImportReadersData(OdbcConnection conROGER, SqlConnection con, int LogParentId)
        {
            const string sql = "select R.ReaderID, R.Name, R.IsTerminal, R.Type, R.TermRCPType, Z.ZoneName, R.ZoneId " +
                               "from Readers R " +
                               "left outer join Zones Z on Z.ZoneId = R.ZoneId";
            List<int> inT;
            List<int> outT;
            List<int> dutyT;

            GetDoorTypes(conROGER, out inT, out outT, out dutyT, LogParentId);

            int cnt = 0;
            OdbcCommand odbcCmd = new OdbcCommand(sql, conROGER);
            OdbcDataReader data = odbcCmd.ExecuteReader();
            try
            {
                while (data.Read())
                {
                    try
                    {
                        string id = data.GetString(0);
                        string nazwa = KP.pl(data.GetString(1));
                        bool isTerm = data.GetBoolean(2);
                        int door = data.GetInt32(3);
                        int termdoor = data.GetInt32(4);
                        string zone = KP.pl(data.GetString(5));
                        string zoneId = data.GetString(6);

                        int d = isTerm ? termdoor : door;
                        string inout;
                        if (inT.Contains(d)) inout = "0";  // in
                        else if (outT.Contains(d)) inout = "1";   // out
                        else inout = Base.NULL;
                        string duty = dutyT.Contains(door) ? "1" : "0";  // służbowe true:false

                        if (cnt == 0) Base.execSQL(con, "delete from Readers");
                        
                        bool ok = Base.insertSQL2(con, "insert into Readers (Id ,Nazwa, Type, Zone, ZoneId, IsTerminal, TermRCPType, InOut, Duty) values (" +
                            Base.insertParam(id) +
                            Base.insertStrParam(nazwa) +
                            Base.insertParam(door.ToString()) +
                            Base.insertStrParam(zone) +
                            Base.insertParam(zoneId) +
                            Base.insertParam(Base.boolParam(isTerm)) +
                            Base.insertParam(termdoor.ToString()) +
                            Base.insertParam(inout) +
                            Base.insertParamLast(duty) +
                        ")");

                        if (ok) cnt++;
                        else
                        {
                            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import Readers - Duplicate index", id.ToString(), Log.OK); // dup index, ale nic sie nie stało
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Log.t2APP_IMPORTRCP, LogParentId, "Import Readers - Exception", ex.Message);
                        cnt = -cnt;
                        break;
                    }
                }
            }
            finally
            {
                data.Close();
            }
            return cnt;
        }
        //--------------------------------------------------------

        public static int UpdateRcpId()
        {
            int cnt = 0;
            SqlConnection con = Base.Connect();
            OdbcConnection conROGER = Base.odbcConnect(conStrROGER);
            //---------------
            Base.execSQL(con, "delete from RogerUsers");
            DataSet dsR = Base.getDataSet(conROGER, "select UserId, FirstName, LastName, Evidence, Status, GroupId, U.\"Active\", Deleted, Custom1, Custom2 from Users U");
            foreach (DataRow dr in dsR.Tables[0].Rows)
            {
                Base.execSQL(con, String.Format(
                    "insert into RogerUsers (UserId, FirstName, LastName, Evidence, Status, GroupId, Active, Deleted, Custom1, Custom2, State) values " + 
                            "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},0)",
                            Base.getValue(dr, 0),
                            KP.pl(Base.strParam2(Base.getValue(dr, 1))),
                            KP.pl(Base.strParam2(Base.getValue(dr, 2))),
                            KP.pl(Base.strParam2(Base.getValue(dr, 3))), 
                            Base.getValue(dr, 4),                            
                            Base.getValue(dr, 5),                            
                            Base.getBool(dr, 6, false) ? "1" : "0",
                            Base.getBool(dr, 7, false) ? "1" : "0",                            
                            KP.pl(Base.strParam2(Base.getValue(dr, 8))),   
                            KP.pl(Base.strParam2(Base.getValue(dr, 9)))
                        ));
                cnt++;
            }
            //---------------
            Base.Disconnect(conROGER);
            Base.Disconnect(con);
            return cnt;
        }






        //-----------------------------------------------------------
        public static int ImportROGERData_bezWhere(OdbcConnection conROGER, SqlConnection con, int LogParentId)
        {
            //const string sql = "SELECT ECUniqueID, ECDate, ECCode, ECUserID, ECReaderID, ECDoorType, { fn CONVERT(ECTime, SQL_VARCHAR) } as stime FROM EventsCache where ECUniqueID > ";
            const string sql = "SELECT ECUniqueID, ECDate, ECCode, ECUserID, ECReaderID, ECDoorType, { fn CONVERT(ECTime, SQL_VARCHAR) } as stime FROM EventsCache";

            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 1", null, Log.OK); // dup index, ale nic sie nie stało



            List<int> inT;
            List<int> outT;
            List<int> dutyT;
            GetDoorTypes(conROGER, out inT, out outT, out dutyT, LogParentId);

            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 2", null, Log.OK); // dup index, ale nic sie nie stało


            string _lastUID = Base.getScalar(con, "select max(ECUniqueID) from RCP");
            int lUID = Tools.StrToInt(_lastUID, 0);
            if (String.IsNullOrEmpty(_lastUID)) _lastUID = "0";

            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 3", null, Log.OK); // dup index, ale nic sie nie stało


            int cnt = 0;
            int ecnt = 0;
            //OdbcCommand odbcCmd = new OdbcCommand(sql + lastUID, conROGER);

            

            
            OdbcCommand odbcCmd = new OdbcCommand(sql, conROGER);


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 4", null, Log.OK); // dup index, ale nic sie nie stało

            OdbcDataReader data = odbcCmd.ExecuteReader();


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 5", null, Log.OK); // dup index, ale nic sie nie stało


            try
            {
                while (data.Read())
                {
                    int uid = -1;
                    try
                    {
                        uid = data.GetInt32(0);
                        if (uid > lUID)
                        {
                            string date = data.GetString(1);
                            if (!String.IsNullOrEmpty(date))
                            {
                                if (date.Length > 10)
                                    date = date.Remove(10);
                                else
                                {
                                    int y = 0;
                                }
                            }
                            else
                            {
                                int x = 0;
                            }

                            string time;
                            //uid = data.GetInt32(0);
                            int code = data.GetInt32(2);
                            int user = data.GetInt32(3);
                            int reader = data.GetInt32(4);
                            int door = data.GetInt32(5);
                            try
                            {
                                time = data.GetString(6);
                            }
                            catch
                            {
                                time = "00:00:00";  // wyszło ze tylko tak to można zrobić - odczyt nawet GetValue:object się wywala !
                            }
                            DateTime dt = DateTime.Parse(date + " " + time);

                            string inout;
                            if (inT.Contains(door)) inout = "0";  // in
                            else if (outT.Contains(door)) inout = "1";   // out
                            else inout = Base.NULL;

                            string duty = dutyT.Contains(door) ? "1" : "0";  // true:false

                            bool ok = Base.insertSQL2(con, "insert into RCP (ECUniqueID ,Czas, ECCode, ECUserId, ECReaderId, ECDoorType, InOut, Duty) values (" +
                                Base.insertParam(uid.ToString()) +
                                Base.insertStrParam(Base.DateTimeToStr(dt)) +
                                Base.insertParam(code.ToString()) +
                                Base.insertParam(user.ToString()) +
                                Base.insertParam(reader.ToString()) +
                                Base.insertParam(door.ToString()) +
                                Base.insertParam(inout) +
                                Base.insertParamLast(duty) +
                            ")");

                            if (ok) cnt++;
                            else
                            {
                                Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Duplicate index", uid.ToString(), Log.OK); // dup index, ale nic sie nie stało
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ecnt++;
                        Log.Error(Log.t2APP_IMPORTRCP, LogParentId, "Import data exception #" + ecnt.ToString() + ": " + ex.Message, uid.ToString());
                        if (ecnt >= maxErrors)  //albo odwinąć transakcję?
                        {
                            Log.Error(Log.t2APP_IMPORTRCP, LogParentId, "Import data aborted", null);
                            cnt = -cnt;
                            break;
                        }
                    }
                }
            }
            finally
            {
                data.Close();
            }


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 6", null, Log.OK); // dup index, ale nic sie nie stało

            return cnt;
        }
        
        
        //---------------------------------------------
        public static int ImportROGER(SqlConnection con, OdbcConnection conROGER, int LogParentId)
        {
            int cnt = -1;
            bool fcon = con == null;
            bool froger = conROGER == null;
            if (fcon) con = Base.Connect();


            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 0", conStrROGER, Log.OK); 


            if (froger) conROGER = Base.odbcConnect(conStrROGER);



            Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import 0.1", null, Log.OK); 
            
            try
            {
                cnt = ImportROGERData(conROGER, con, LogParentId);
                Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import RCP Data finished", "Count: " + cnt.ToString(), Log.OK);
            }
            finally
            {
                if (froger) Base.Disconnect(conROGER);
                if (fcon) Base.Disconnect(con);
            }
            return cnt;
        }

        public static int ImportROGER(SqlConnection con, int LogParentId)
        {
            return ImportROGER(con, null, LogParentId);
        }

        public static int ImportROGER()
        {
            return ImportROGER(null, null, 0);
        }
        //----------------------------------------------------
        public static int ImportReaders(SqlConnection con, OdbcConnection conROGER, int LogParentId)
        {
            int cnt = -1;
            bool fcon = con == null;
            bool froger = conROGER == null;
            if (fcon) con = Base.Connect();

            if (froger) conROGER = Base.odbcConnect(conStrROGER);
            try
            {
                cnt = ImportReadersData(conROGER, con, LogParentId);
                Log.Info(Log.t2APP_IMPORTRCP, LogParentId, "Import Readers Data finished", "Count: " + cnt.ToString(), Log.OK);
            }
            finally
            {
                if (froger) Base.Disconnect(conROGER);
                if (fcon) Base.Disconnect(con);
            }
            return cnt;
        }

        public static int ImportReaders(SqlConnection con, int LogParentId)
        {
            return ImportReaders(con, null, LogParentId);
        }

        public static int ImportReaders()
        {
            return ImportReaders(null, null, 0);
        }

    }
}
