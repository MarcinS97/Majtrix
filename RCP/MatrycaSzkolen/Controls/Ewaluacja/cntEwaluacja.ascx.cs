using HRRcp.App_Code;
using HRRcp.MatrycaSzkolen.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Ewaluacja
{
    public partial class cntEwaluacja : System.Web.UI.UserControl
    {

        public static class Type
        {
            public const String AnkietaPrac = "0";
            public const String AnkietaKier = "1";
            public const String AnkietaKier2 = "2";
        }


        public static class Role
        {
            public const String rAdmin = "1";
            public const String rMistrz = "2";
            public const String rTrener = "3";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                hidUserId.Value = App.User.Id;

            }
        }

        protected void Button_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument as String;

            if(!String.IsNullOrEmpty(id))
            {  
                switch(e.CommandName)
                {
                    case "EmployeeSurvey":
                        RedirectToEmployeeSurvey(id);
                        break;
                    case "PrintEmployeeSurvey":
                        PrintEmployeeSurvey(id);
                        break;
                    case "SuperiorSurvey":
                        RedirectToSuperiorSurvey(id);
                        break;
                    case "SuperiorSurvey2":
                        RedirectToSuperiorSurvey(id);
                        break;
                    case "PrintSuperiorSurvey":
                        PrintSuperiorSurvey(id);
                        break;

                }
            }
        }

        void RedirectToEmployeeSurvey(string id)
        {
            App.Redirect(String.Format("MatrycaSzkolen/AnkietaP.aspx?p={0}", id));
        }

        void RedirectToSuperiorSurvey(string id)
        {
            App.Redirect(String.Format("MatrycaSzkolen/AnkietaK.aspx?p={0}", id));
        }

        void PrintEmployeeSurvey(string id)
        {
            Dictionary<String, object> Data = new Dictionary<string, object>();
            DataRow dr = db.Select.Row(dsData, id);
            Data["Pracownik"] = db.getValue(dr, "PracownikCert", String.Empty);
            Data["Stanowisko"] = db.getValue(dr, "Stanowisko", String.Empty);
            Data["Temat"] = db.getValue(dr, "TematSzkolenia", String.Empty);
            Data["Organizator"] = db.getValue(dr, "Organizator", String.Empty);
            Data["Prowadzacy"] = db.getValue(dr, "ProwadzacySzkolenie", String.Empty);
            Data["Miejsce"] = db.getValue(dr, "MiejsceSzkolenia", String.Empty);
            Data["CzasTrwania"] = db.getValue(dr, "CzasTrwania", String.Empty);
            Data["DataSzkolenia"] = db.getDateTime(dr, "DataSzkolenia", DateTime.Now);

            Data["Ocena1"] = db.getValue(dr, "Odp1", String.Empty);
            Data["Ocena2"] = db.getValue(dr, "Odp2", String.Empty);
            Data["Ocena3"] = db.getValue(dr, "Odp3", String.Empty);
            Data["Ocena4"] = db.getValue(dr, "Odp4", String.Empty);
            Data["Ocena5"] = db.getValue(dr, "Odp5", String.Empty);
            Data["Ocena6"] = db.getValue(dr, "Odp6", String.Empty);
            Data["Ocena7"] = db.getValue(dr, "Odp7", String.Empty);
            Data["Ocena8"] = db.getValue(dr, "Odp8", String.Empty);
            Data["Ocena9"] = db.getValue(dr, "Odp9", String.Empty);
            Data["Ocena10"] = db.getValue(dr, "Odp10", String.Empty);
            Data["Ocena11"] = db.getValue(dr, "Odp11", String.Empty);
            Data["Ocena12"] = db.getValue(dr, "Odp12", String.Empty);
            Data["Ocena13"] = db.getValue(dr, "Odp13", String.Empty);
            Data["Ocena14"] = db.getValue(dr, "Odp14", String.Empty);

            Data["Uwagi"] = db.getValue(dr, "Uwagi", String.Empty);

            Data["DataUtworzenia"] = DateTime.Now.ToShortDateString();
            

            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aou.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }


            foreach (KeyValuePair<String, Object> item in Data)
            {
                string key = item.Key;
                object o = item.Value;
                string value = string.Empty;
                if (o is DateTime)
                    value = Tools.GetDateTime(o, DateTime.Now).ToShortDateString();
                else
                    value = Tools.GetStr(item.Value, string.Empty);

                text = text.Replace(String.Format("%{0}%", key), value);


            }

            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aou.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();

        }

        void ReplaceRTF(Dictionary<String, object> data, ref string text)
        {
            foreach (KeyValuePair<String, Object> item in data)
            {
                string key = item.Key;
                object o = item.Value;
                string value = string.Empty;
                if (o is DateTime)
                    value = Tools.GetDateTime(o, DateTime.Now).ToShortDateString();
                else
                    value = Tools.GetStr(item.Value, string.Empty);

                if (key == "Odp1") value = (value == "0") ? "Tak" : "Nie";
                else if (key == "Odp3") value = (value == "0") ? "Tak" : "Nie";
                else if (key == "Odp5") value = ((value == "0") ? "Tak" : (value == "1") ? "Nie" : "Nie jest to wymagane");

                text = text.Replace(String.Format("%{0}%", key), value);
            }  
        }


        void PrintSuperiorSurvey(string id)
        {
            Dictionary<String, object> Data = new Dictionary<string, object>();
            DataRow dr = db.Select.Row(dsData, id);
            Data["Pracownik"] = db.getValue(dr, "PracownikCert", String.Empty);
            Data["Temat"] = db.getValue(dr, "TematSzkolenia", String.Empty);
            Data["DataSzkolenia"] = db.getDateTime(dr, "DataSzkolenia", DateTime.Now);
            Data["CelSzkolenia"] = db.getValue(dr, "CelSzkolenia", String.Empty);
            Data["MonitDni"] = db.getValue(dr, "MonitDni", String.Empty);

            Data["Odp1"] = db.getValue(dr, "Odp1", String.Empty);
            Data["Odp2"] = db.getValue(dr, "Odp2", String.Empty);
            Data["Odp3"] = db.getValue(dr, "Odp3", String.Empty);
            Data["Odp4"] = db.getValue(dr, "Odp4", String.Empty);
            Data["Odp5"] = db.getValue(dr, "Odp5", String.Empty);
            Data["Odp6"] = db.getValue(dr, "Odp6", String.Empty);

            Data["Tekst1"] = db.getValue(dr, "Tekst1", String.Empty);
            Data["Tekst2"] = db.getValue(dr, "Tekst2", String.Empty);
            Data["Tekst3"] = db.getValue(dr, "Tekst3", String.Empty);

            Data["DataUtworzenia"] = DateTime.Now.ToShortDateString();
           

            string text;
            string text1 = "Odpowiedź: %Odp1%, w stopniu: %Odp2%";
            string text2 = "Odpowiedź: %Odp3%, w stopniu: %Odp4%";
            string text3 = "Odpowiedź: %Odp5%";

            ReplaceRTF(Data, ref text1);
            Data["Odpowiedz1"] = text1;

            ReplaceRTF(Data, ref text2);
            Data["Odpowiedz2"] = text2;

            ReplaceRTF(Data, ref text3);
            Data["Odpowiedz3"] = text3;


            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aop.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            ReplaceRTF(Data, ref text);


            //foreach (KeyValuePair<String, Object> item in Data)
            //{
            //    string key = item.Key;
            //    object o = item.Value;
            //    string value = string.Empty;
            //    if (o is DateTime)
            //        value = Tools.GetDateTime(o, DateTime.Now).ToShortDateString();
            //    else
            //        value = Tools.GetStr(item.Value, string.Empty);

            //    //if (key == "Odp1") value = (value == "0") ? "Tak" : "Nie";
            //    //else if (key == "Odp3") value = (value == "0") ? "Tak" : "Nie";
            //    //else if (key == "Odp5") value = ((value == "0") ? "Tak" : (value == "1") ? "Nie" : "Nie jest to wymagane");

            //    text = text.Replace(String.Format("%{0}%", key), value);
            //}


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aop.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();

        }

        protected void PrintEmployeeSurveyTemplate(object sender, EventArgs e)
        {
            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aou.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            text = text.Replace("%Pracownik%", "");
            text = text.Replace("%Stanowisko%", "");
            text = text.Replace("%Temat%", "");
            text = text.Replace("%Organizator%", "");
            text = text.Replace("%Prowadzacy%", "");
            text = text.Replace("%Miejsce%", "");
            text = text.Replace("%CzasTrwania%", "");
            text = text.Replace("%DataSzkolenia%", "");

            text = text.Replace("%Ocena1%", "");
            text = text.Replace("%Ocena2%", "");
            text = text.Replace("%Ocena3%", "");
            text = text.Replace("%Ocena4%", "");
            text = text.Replace("%Ocena5%", "");
            text = text.Replace("%Ocena6%", "");
            text = text.Replace("%Ocena7%", "");
            text = text.Replace("%Ocena8%", "");
            text = text.Replace("%Ocena9%", "");
            text = text.Replace("%Ocena10%", "");
            text = text.Replace("%Ocena11%", "");
            text = text.Replace("%Ocena12%", "");
            text = text.Replace("%Ocena13%", "");
            text = text.Replace("%Ocena14%", "");

            text = text.Replace("%DataUtworzenia%", DateTime.Now.ToShortDateString());
            text = text.Replace("%Uwagi%", "……………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………….");


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aou.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();

        }

        protected void PrintSuperiorSurveyTemplate(object sender, EventArgs e)
        {
            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aop.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            text = text.Replace("%Pracownik%", "");
            text = text.Replace("%Temat%", "");
            text = text.Replace("%DataSzkolenia%", "");
            text = text.Replace("%CelSzkolenia%", "");
            text = text.Replace("%MonitDni%", "");
            text = text.Replace("%DataUtworzenia%", DateTime.Now.ToShortDateString());


            text = text.Replace("%Tekst1%", "……………………………………………………………………………….");
            text = text.Replace("%Tekst2%", "……………………………………………………………………………….");
            text = text.Replace("%Tekst3%", "……………………………………………………………………………….");
            text = text.Replace("%Odp6%", "…………………………………………………………………………………………………………………………………………………………….");

            text = text.Replace("%Odp6%", "…………………………………………………………………………………………………………………………………………………………….");

            text = text.Replace("%Odpowiedz1%", "");
            text = text.Replace("%Odpowiedz2%", "");
            text = text.Replace("%Odpowiedz3%", "");


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aop.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();

        }

        protected void btnAddEmployeeSurvey_Click(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                btnAddEmployeeSurveyConfirm.CommandArgument = id;
                Tools.ShowConfirm("Czy na pewno chcesz dodać ankietę pracownika?", btnAddEmployeeSurveyConfirm);
            }
        }

        protected void btnAddEmployeeSurveyConfirm_Click(object sender, EventArgs e)
        {
            string id = btnAddEmployeeSurveyConfirm.CommandArgument;
            if(!String.IsNullOrEmpty(id))
            {
                db.Execute(dsCreateEmployeeSurvey, id);
                string newId = GetAnkietaId(id, Type.AnkietaPrac);
                AnkietPracMail(newId);
                gvList.DataBind();
            }
        }

        public bool IsAdmin()
        {
            return App.User.IsMSAdmin;
        }

        protected void btnAddSuperiorSurvey_Click(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                btnAddSuperiorSurveyConfirm.CommandArgument = id;
                Tools.ShowConfirm("Czy na pewno chcesz dodać ankietę przełożonego?", btnAddSuperiorSurveyConfirm);
            }
        }

        protected void btnAddSuperiorSurvey2_Click(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                btnAddSuperiorSurveyConfirm2.CommandArgument = id;
                Tools.ShowConfirm("Czy na pewno chcesz dodać drugą ankietę przełożonego?", btnAddSuperiorSurveyConfirm2);
            }
        }

        protected void btnAddSuperiorSurveyConfirm_Click(object sender, EventArgs e)
        {
            string id = btnAddSuperiorSurveyConfirm.CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                db.Execute(dsCreateSuperiorSurvey, id);
                string newId = GetAnkietaId(id, Type.AnkietaKier);
                AnkietaKierMail(newId);
                gvList.DataBind();
            }
        }

        protected void btnAddSuperiorSurveyConfirm2_Click(object sender, EventArgs e)
        {
            string id = btnAddSuperiorSurveyConfirm2.CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                db.Execute(dsCreateSuperiorSurvey2, id);
                gvList.DataBind();
            }
        }

        public string GetAnkietaId(string certId, string typ)
        {
            return db.Select.Scalar(dsGetAnkietaId, certId, typ);
        }

        protected void Roles_SelectTab(object sender, EventArgs e)
        {

        }

        public bool CanEmployee()
        {
            return App.User.HasRight(AppUser.rMSAnkietyPodgladP) || App.User.HasRight(AppUser.rMSAnkietyEdycjaP);
        }

        public bool CanSuperior()
        {
            return App.User.HasRight(AppUser.rMSAnkietyPodgladK) || App.User.HasRight(AppUser.rMSAnkietyEdycjaK);
        }

        protected void Roles_DataBound(object sender, EventArgs e)
        {
            if (!App.User.IsMSAdmin)
                Tools.RemoveMenu(Roles.Tabs, Role.rAdmin);
            if (!App.User.HasRight(AppUser.rMSMeister) && !App.User.IsMSAdmin)
                Tools.RemoveMenu(Roles.Tabs, Role.rMistrz);
            if (!App.User.HasRight(AppUser.rMSTrener) && !App.User.IsMSAdmin)
                Tools.RemoveMenu(Roles.Tabs, Role.rTrener);
            if (Roles.Tabs.Items.Count > 0)
            {
                if (Roles.Tabs.Items.Count == 1)
                    divRoles.Visible = false;
                else
                    divRoles.Visible = true;
                    
                    Roles.Tabs.Items[0].Selected = true;
            }
            else
                App.ShowNoAccess("", App.User);
        }

        #region MAILING

        public static void AnkietPracMail(string ankietaId)
        {
            MSMailing.EventMS(MSMailing.maMS_ANK_PRAC_CREATE, ankietaId, null, GetPracId(ankietaId));
            Log.Info(1337, "Ankieta Pracownik - Mailing", ankietaId);
        }

        public static void AnkietaKierMail(string ankietaId)
        {
            MSMailing.EventMS(MSMailing.maMS_ANK_KIER_CREATE, ankietaId, null, GetKierId(ankietaId));
            Log.Info(1337, "Ankieta Przełożony - Mailing", ankietaId);
        }

        public static string GetPracId(string ankietaId)
        {
            return db.Select.Scalar("select IdPracownika from Certyfikaty where Id = (select IdCertyfikatu from msAnkiety where Id = {0})", ankietaId);
        }

        public static string GetKierId(string ankietaId)
        {
            return db.Select.Scalar(
                @"
declare @pracId int = (select IdPracownika from Certyfikaty where Id = (select IdCertyfikatu from msAnkiety where Id = {0}))
select IdKierownika from Przypisania r where r.Status = 1 and GETDATE() between r.Od and isnull(r.Do, '20990909') and  r.IdPracownika = @pracId
", ankietaId);
        }


        #endregion
    }
}