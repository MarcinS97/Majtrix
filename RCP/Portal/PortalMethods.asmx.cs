using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

using Newtonsoft.Json;


namespace HRRcp.Portal
{
    /// <summary>
    /// Summary description for Portal
    /// </summary>
    [WebService(Namespace = "http://kdrrcp.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.     
    [System.Web.Script.Services.ScriptService]
    public class PortalMethods : System.Web.Services.WebService
    {

        SqlConnection fcon = null;

        private SqlConnection con
        {
            get
            {
                if (fcon == null)
                {
                    fcon = new SqlConnection(db.PORTAL);
                    fcon.Open();
                }
                return fcon;
            }
        }

        private void dbDisconnect()
        {
            if (fcon != null)
                db.DoDisconnect(ref fcon);
        }

        public class ChatMessage
        {
            public ChatMessage() { }
            public ChatMessage(DataRow dr)
            {
                id = db.getValue(dr, "Id");
                employeeId = db.getValue(dr, "IdPracownika");
                friendId = db.getValue(dr, "IdZnajomego");
                msg = db.getValue(dr, "Msg");
                sentDate = db.getValue(dr, "DataWyslania");
            }

            public string id { get; set; }
            public string employeeId { get; set; }
            public string friendId { get; set; }
            public string msg { get; set; }
            public string sentDate { get; set; }
        }

        [WebMethod]
        public string SendMessage(ChatMessage data)
        {
            DateTime dt = DateTime.Now;
            string dts = dt.ToString();
            db.execSQL(con, String.Format("insert into poChat (IdPracownika, IdZnajomego, Msg, DataWyslania) values ({0}, {1}, '{2}', '{3}')", data.employeeId, data.friendId, data.msg, dts));
            return dts;
        }

        public class ChatMsgsRequest
        {
            public string employeeId { get; set; }
            public string friendId { get; set; }
            public string lastId { get; set; }
        }

        [WebMethod]
        public string GetAllMessages(List<ChatMsgsRequest> requests)
        {
            List<ChatMessage> msgs = new List<ChatMessage>();

            foreach (ChatMsgsRequest req in requests)
            {
                DataTable dt = db.getDataSet(con, String.Format(@"select * from poChat where Id > {0} 
                    and ((IdPracownika = {1} and IdZnajomego = {2}) or (IdPracownika = {2} and IdZnajomego = {1}))
                    order by Id", req.lastId, req.employeeId, req.friendId)).Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    msgs.Add(new ChatMessage(dr));
                }
            }
            dbDisconnect();
            return JsonConvert.SerializeObject(msgs);
        }

        [WebMethod]
        public string GetMessages(String lastId, String employeeId, String friendId)
        {
            DataTable dt = db.getDataSet(con, String.Format(@"select * from poChat where Id > {0} 
                    and ((IdPracownika = {1} and IdZnajomego = {2}) or (IdPracownika = {2} and IdZnajomego = {1}))
                    order by Id", lastId, employeeId, friendId)).Tables[0];
            dbDisconnect();
            List<ChatMessage> msgs = new List<ChatMessage>();

            foreach (DataRow dr in dt.Rows)
            {
                msgs.Add(new ChatMessage(dr));
            }
            return JsonConvert.SerializeObject(msgs);
        }

        [WebMethod]
        public string GetInitialMessages(String employeeId, String friendId)
        {
            DataTable dt = db.getDataSet(con, String.Format(@"select top 100 * from poChat where ((IdPracownika = {0} and IdZnajomego = {1}) or (IdPracownika = {1} and IdZnajomego = {0}))
                    order by Id desc", employeeId, friendId)).Tables[0];
            dbDisconnect();
            List<ChatMessage> msgs = new List<ChatMessage>();

            foreach (DataRow dr in dt.Rows)
            {
                msgs.Add(new ChatMessage(dr));
            }
            return JsonConvert.SerializeObject(msgs);
        }

        [WebMethod]
        public string GetOldMessages(String firstId, String employeeId, String friendId)
        {
            DataTable dt = db.getDataSet(con, String.Format(@"select top 100 * from poChat where Id < {0} 
                    and ((IdPracownika = {1} and IdZnajomego = {2}) or (IdPracownika = {2} and IdZnajomego = {1}))
                    order by Id", firstId, employeeId, friendId)).Tables[0];
            dbDisconnect();
            List<ChatMessage> msgs = new List<ChatMessage>();
            foreach (DataRow dr in dt.Rows)
            {
                msgs.Add(new ChatMessage(dr));
            }
            return JsonConvert.SerializeObject(msgs);
        }

    }
}
