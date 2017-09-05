using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Adm
{
    public partial class AdmInfoControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Info.CheckUpdate();
            }
        }
        //---------------------
        protected void lvTexts_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Test")
            {
                string typ = e.CommandArgument.ToString();
                if (!String.IsNullOrEmpty(typ))
                {
                    switch (typ.Substring(0,1))
                    {
                        case "I":
                            Info.ShowInfo(typ, Info.ibBack);
                            break;
                        case "L":
                            Info.ShowInfo2(typ, Info.ibBack);
                            break;
                        case "H":
                            Info.SetHelp(typ, true);
                            break;
                    }
                }
            }
        }

        protected void lvTexts_DataBound(object sender, EventArgs e)
        {           // rozwiazanie z: http://forums.asp.net/t/1327007.aspx/1?Postbacktrigger+for+button+in+an+updatepanel+in+a+listview
            /*
            foreach (ListViewDataItem i in lvTexts.Items)
            {
                Button button = (Button)i.FindControl("TestButton");
                PostBackTrigger trigger = new PostBackTrigger();
                trigger.ControlID = button.UniqueID;
                UpdatePanel1.Triggers.Add(trigger);
                ScriptManager.GetCurrent(Page).RegisterPostBackControl(button);
            }
             */
        }
        //---------------------------------------
        public void ImportData(string fileName)
        {
            string line;
            if (File.Exists(fileName))
            {
                StreamReader file = null;
                SqlConnection con = null;
                try
                {
                    file = new StreamReader(fileName);
                    con = Base.Connect();
                    int cnt = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (cnt > 0)  // opiszczam pierwszą linię nagłówka
                        {
                            string[] values = line.Split(Tools.chTAB);
                            if (values.Length >= 3)
                            {
                                string typ = values[0];
                                string opis = Tools.TextToCtrl(values[1]);
                                string tekst = Tools.TextToCtrl(values[2]);
                                bool b = Base.execSQL(
                                    "update Teksty set " +
                                    Base.updateStrParam("Opis", opis) +
                                    Base.updateStrParamLast("Tekst", tekst) +
                                    "where Typ = " + Base.strParam(typ));
                                if (!b)
                                    Base.execSQL(
                                        "insert into Teksty (Typ, Opis, Tekst) " +
                                        "values (" +
                                            Base.strParam(typ) + "," +
                                            Base.strParam(opis) + "," +
                                            Base.strParam(tekst) +
                                        ")");
                            }
                        }
                        cnt++;
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                    Base.Disconnect(con);
                }
            }
        }

        public ListView List
        {
            get { return lvTexts; }
        }
    }
}