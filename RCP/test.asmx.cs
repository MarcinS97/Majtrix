using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HRRcp
{
    /// <summary>
    /// Summary description for test3
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    
    [System.Web.Script.Services.ScriptService]
    public class test3 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        //public string IlDni(IlDniParams p)
        public string IlDni(string dOd, string dDo)
        {
            /*
            string a = HttpContext.Current.Request.Form["aaa"];
            string b = HttpContext.Current.Request.Form["bbb"];
            return "7 " + a + "-" + b;
             */


            //return "7 " + p.dOd + "-" + p.dDo;
            return "7 " + dOd + "-" + dDo;
        }

        public class IlDniParams
        {
            public string dOd { get; set; }
            public string dDo { get; set; }
        }


        /*
        public class Person
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string department { get; set; }
            public Address address { get; set; }
            public string[] technologies { get; set; }
        }

        public class Address
        {
            public string addressline1 { get; set; }
            public string addressline2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string pin { get; set; }
        }
        */


    }
}
