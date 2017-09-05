using HRRcp.App_Code;
using HRRcp.RCP.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls
{
    [Serializable]
    public class Employee
    {
        public Employee(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
            this.Checked = true;
        }

        public Employee(string data)
        {
            string[] tabData = data.Split(';');
            this.Id = tabData[0];
            this.Name = tabData[1];
            this.Checked = true;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }

    public partial class cntNadgodzinyWnioskiModal : System.Web.UI.UserControl
    {
        public event EventHandler Sent;

        const bool setZmiana = true; 

        protected void Page_Load(object sender, EventArgs e)
        {
            //ViewState["nadgn"] = NadgN;
            if (!IsPostBack)
            {
                divZmiana.Visible = setZmiana;
            }
            Tools.ExecuteJavascript("cntNagodzinyWnioskiModal();");
        }

        void PrepareModal(string pracList)
        {
            hidUserId.Value = (App.User.IsAdmin) ? "0" : App.User.Id;
            Tools.GenerateDuration(ddlOd, 30, 23, false);
            Tools.GenerateDuration(ddlDo, 30, 23, false);

            Employees.Clear();
            foreach (var item in pracList.Split('|'))
            {
                Employees.Add(new Employee(item));
            }

            rpEmployees.DataSource = Employees;
            rpEmployees.DataBind();

            PrepareMode();
        }

        public void Show(string pracList)
        {
            Show(null, null, null, 0, 0, 0, EType.None, null, null, pracList);
        }

        public void Show(string PracId, string Date, string Nadg, int Nadg50, int Nadg100, int Noc, EType Type, string Uwagi, string Powod, string pracList)
        {
            cntModal.Show(false);
            
            if(string.IsNullOrEmpty(pracList))
            {
                pracList = String.Format("{0};{1}",PracId, AppUser.GetNazwiskoImieNREW(PracId));
            }

            //this.PracId = PracId;
            PrepareModal(pracList);

            //this.PracId = PracId;
            this.Date = Date;
            this.Type = Type;
            this.Powod = Powod;
            this.Uwagi = Uwagi;

            switch (Mode)
            {
                case EMode.PreAccept:
                    this.Nadg = Nadg;
                    break;
                case EMode.PostAccept:
                    this.Nadg50 = Nadg50;
                    this.Nadg100 = Nadg100;
                    this.Noc = Noc;
                    break;
            }

            //ddlPracownik.SelectedValue = "11";
            //cntModal.Update();
        }


        public enum EMode { PreAccept, PostAccept };
        public EMode Mode
        {
            get
            {
                if (ViewState["vMode"] != null)
                    return (EMode)Enum.Parse(typeof(EMode), ViewState["vMode"].ToString());
                return EMode.PostAccept;
            }
            set { ViewState["vMode"] = value; }
        }

        public enum EType { None, DoWyplaty, DoWybrania };
        public EType Type
        {
            get
            {
                string v = rblType.SelectedValue;
                switch (v)
                {
                    case "1":
                        return EType.DoWyplaty;
                        break;
                    case "2":
                        return EType.DoWybrania;
                        break;
                    default:
                        return EType.DoWyplaty;
                        break;
                }
                return EType.DoWybrania;
            }
            set
            {
                switch (value)
                {
                    case EType.DoWyplaty:
                        rblType.SelectedValue = "1";
                        break;
                    case EType.DoWybrania:
                        rblType.SelectedValue = "2";
                        break;
                    default:
                        rblType.SelectedValue = "1";
                        break;
                }
            }
        }

        protected void deDate_DateChanged(object sender, EventArgs e)
        {
            //    LoadData();
        }

        //void LoadData()
        //{
        //    DataRow dr = db.Select.Row(dsData, PracId, db.strParam(Date));
        //    if (dr != null)
        //    {
        //        string idZmiany = db.getStr(dr["IdZmiany"], "");

        //        //divZmiana.Visible = true;
        //        //ddlZmiana.DataBind();
        //        //Tools.SelectItem(ddlZmiana, idZmiany);
        //    }

        //}

        void PrepareMode()
        {
            bool pre = false;

            switch (Mode)
            {
                case EMode.PreAccept:
                    pre = true;
                    break;
                case EMode.PostAccept:
                    pre = false;
                    break;
            }

            divNadgPre.Visible = pre;
            divNadgPost.Visible = !pre;
        }

        protected void ddlPracownik_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        public List<Employee> Employees
        {
            get
            {
                if (ViewState["vEmployees"] == null)
                {
                    ViewState["vEmployees"] = new List<Employee>();
                }
                return ViewState["vEmployees"] as List<Employee>;
            }
            set
            {
                ViewState["vEmployees"] = value;
            }


        }

        //public String PracId
        //{
        //    get { return ddlPracownik.SelectedValue; }
        //    set { ddlPracownik.SelectedValue = value; }
        //}

        public String Date
        {
            get { return deDate.DateStr; }
            set { deDate.Date = value; }
        }

        public String Nadg
        {
            get { return tbNadg.Text; }
            set { tbNadg.Text = value; }
        }

        public double Nadg50
        {
            get { return tbNadg50.Time.TotalSeconds; }
            set { tbNadg50.Seconds = (int)value; }
        }

        public double Nadg100
        {
            get { return tbNadg100.Time.TotalSeconds; }
            set { tbNadg100.Seconds = (int)value; }
        }

        public double Noc
        {
            get { return tbNoc.Time.TotalSeconds; }
            set { tbNoc.Seconds = (int)value; }
        }

        public String Uwagi
        {
            get { return tbUwagi.Text; }
            set { tbUwagi.Text = value; }
        }

        public String Powod
        {
            get { return tbPowod.Text; }
            set { tbPowod.Text = value; }
        }


        //public String ZmianaId
        //{
        //    get { return ddlZmiana.SelectedValue; }
        //}



        protected void btnSendRequestConfirm_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz wysłać wniosek do akceptacji?", btnSendRequest);
        }

        protected void btnSendRequest_Click(object sender, EventArgs e)
        {
            String Value = "";

            foreach (RepeaterItem item in rpEmployees.Items)
            {
                CheckBox cb = item.FindControl("cbChecked") as CheckBox;

                if (cb != null && cb.Checked)
                {
                    string id = Tools.GetText(item, "hidId");
                    if (!string.IsNullOrEmpty(id))
                    {
                        Value = db.Select.Scalar(dsSend
                            , db.strParam(Date), id
                            //, db.nullParam(0)
                            , db.nullParam(Tools.GetLineParam(ddlZmiana.SelectedValue, 0))
                            , tbNadg50.Time.TotalSeconds
                            , tbNadg100.Time.TotalSeconds
                            , tbNoc.Time.TotalSeconds
                            , db.nullStrParam(tbPowod.Text)
                            , db.nullStrParam(tbUwagi.Text)
                            , App.User.OriginalId
                            , db.nullStrParam(tbNadg.Text)
                            , db.nullParam(rblType.SelectedValue)
                            , db.nullStrParam(deCDay.DateStr)
                            );
                    }
                }
            }


            //co z mailingiem nie do wszystkich, tylko do powyżej w strukturze ?
            DataSet ds_users = db.Select.Set("select Id from Pracownicy p where dbo.GetRightId(p.Rights, 98) = 1 and p.Mailing = 1");
            List<String> l_users = new List<String>();

            foreach (DataRow dr in ds_users.Tables[0].Rows)
            {
                l_users.Add(dr[0].ToString());
            }

            String[] users = l_users.ToArray();

            //XAILING
            RCPMailing.EventRCP(RCPMailing.maRCP_NADG_WN, Value, null, users);

            cntModal.Close();
            if (Sent != null)
                Sent(null, EventArgs.Empty);
        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rblType.SelectedValue)
            {
                case "2":
                    deCDay.Visible = true;// odremować jak będzie trzeba true;
                    break;
                default:
                    deCDay.Visible = false;
                    deCDay.Date = null;
                    break;
            }
        }
    }
}