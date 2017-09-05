using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Data.SqlClient;

//http://www.ianatkinson.net/computing/adcsharp.htm

namespace HRRcp.App_Code
{
    public class AD
    {
        public static DirectoryEntry GetAD(DirectoryEntry de, string objectClass, string name)
        {
            DirectorySearcher dSearch = new DirectorySearcher(de);
            dSearch.Filter = "(&(objectClass=" + objectClass + ")(" + name + "))";
            SearchResultCollection sr = dSearch.FindAll();
            if (sr.Count > 0)
                return sr[0].GetDirectoryEntry();  // biorę pierwszy, jak będzie więcej to na razie nie ma obsługi
            else
                return null;
        }

        public static DirectoryEntry GetAD(DirectoryEntry de, string organizationalUnit)
        {
            return GetAD(de, "organizationalUnit", "OU=" + organizationalUnit);
        }

        public static DirectoryEntry GetADUsers(string adControler, string path)  // JGS\RegionEurope\JGSBydgoszcz\Users
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://" + adControler);
            if (de != null)
            {
                string[] items = path.Split('\\');
                int cnt = items.Count();
                for (int i = 0; i < cnt; i++)
                    if (!String.IsNullOrEmpty(items[i]))    // jesli zaczyna lub konczy się na \
                    {
                        de = GetAD(de, items[i]);
                        if (de == null) break;
                    }
            }
            return de;
        }

        //---------------------
        public static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
                return searchResult.Properties[PropertyName][0].ToString();
            else
                return string.Empty;
        }

        public static string GetProperty(DirectoryEntry de, string PropertyName)
        {
            if (de.Properties.Contains(PropertyName))
                return de.Properties[PropertyName][0].ToString();
            else
                return string.Empty;
        }
        //---------------------
        public static bool IsUser(DirectoryEntry de)
        {
            return de.SchemaClassName.ToUpper().Equals("USER");
        }

        public static bool GetUserData(DirectoryEntry de, out string Login, out string Mail, out string Nazwisko, out string Imie, out string CN)
        {
            if (IsUser(de))
            {
                Login = GetProperty(de, "sAMAccountName");
                Mail = GetProperty(de, "mail");
                Nazwisko = GetProperty(de, "SN");
                Imie = GetProperty(de, "givenName");
                CN = GetProperty(de, "cn");
                //Manager = GetProperty(de, "manager");
                return true;
            }
            else
            {
                Login = null;
                Mail = null;
                Nazwisko = null;
                Imie = null;
                CN = null;
                return false;
            }
        }

        public static List<string> GetUserData(DirectoryEntry de)
        {
            if (IsUser(de))
            {
                List<string> data = new List<string>();
                data.Add(GetProperty(de, "sAMAccountName"));    // login
                data.Add(GetProperty(de, "mail"));              // mail
                data.Add(GetProperty(de, "SN"));                // Nazwisko
                data.Add(GetProperty(de, "givenName"));         // Imie
                data.Add(GetProperty(de, "cn"));                // Imie, nazwisko
                //data.Add(GetProperty(de, "manager"));
                return data;
            }
            else 
                return null;
        }

        public static List<string> GetUserDataFull(DirectoryEntry de)
        {
            if (IsUser(de))
            {
                List<string> data = new List<string>();
                data.Add(de.Path);
                foreach (string propertyName in de.Properties.PropertyNames)
                    data.Add(propertyName + ":" + de.Properties[propertyName][0].ToString());
                return data;
            }
            else
                return null;
        }

        //-------------------------------------------------------------------------------------
        public static int ImportAD(SqlConnection con)
        {
            //StartProgress("select count(*) from AD");
            Base.execSQL(con, "delete from AD");

            //DirectoryEntry de = AD.GetADUsers("jgbdc01", @"JGS\RegionEurope\JGSBydgoszcz\Users");
            DirectoryEntry de = AD.GetADUsers("jgbdc02", @"JGS\RegionEurope\JGSBydgoszcz\Users");
            int cnt = 0;
            if (de != null)
            {
                string Login, Mail, Tel, Nazwisko, Imie, CN, Path, Prop;
                foreach (DirectoryEntry c in de.Children)
                    if (AD.GetUserData(c, out Login, out Mail, out Nazwisko, out Imie, out CN))
                    {
                        Path = c.Path;
                        Prop = "";
                        Tel = null;
                        foreach (string propertyName in c.Properties.PropertyNames)
                        {
                            string v = c.Properties[propertyName][0].ToString();
                            if (propertyName.ToLower() == "telephonenumber")
                            {
                                Tel = Tools.PreparePhoneNo(v);
                                /*if (Tel.Length == 4)
                                    Tel = "52 525 " + Tel;*/
                            }
                            Prop += propertyName + ":" + v + " | ";
                        }

                        Base.execSQL(con,
                            "insert into AD (Login , Nazwisko, Imie, Email, Tel, CN, Path, Properties) " +
                            "values (" +
                                Base.insertStrParam(Login) +
                                Base.insertStrParam(Nazwisko) +
                                Base.insertStrParam(Imie) +
                                Base.insertStrParam(Mail) +
                                Base.insertStrParam(Tel) +
                                Base.insertStrParam(CN) +
                                Base.insertStrParam(Path.Replace("'", "''")) +
                                Base.insertStrParamLast(Prop.Replace("'", "''")) +
                            ")");
                        cnt++;
                        //if (cnt > 5) break;
                    }
            }
            return cnt;
        }
    }
}
