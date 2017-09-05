using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls
{
    public partial class cntNadgodzinyWnioski : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvPracownicy);
        }    

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();
            }
        }

        void Prepare()
        {
            hidUserId.Value = (App.User.IsAdmin) ? "0" : App.User.Id;
            Okres ok = Okres.Current(db.con);
            
            
            deDataOd.Date = ok.DateFrom;
            deDataDo.Date = ok.DateTo;



            //cntSelectOkres.Prepare(DateTime.Today, true);
        }

        protected void cntSelectOkres_OkresChanged(object sender, EventArgs e)
        {

        }

        protected void btnAddRequest_Click(object sender, EventArgs e)
        {
            string sel = gvPracownicySelected.Value;
            if (!String.IsNullOrEmpty(sel))
            {
                string[] p = sel.Substring(1, sel.Length - 2).Split(',');
                for (int i = 0; i < p.Length; i++)
                {
                    int aa = Tools.StrToInt(p[i], -1);
                    if (aa == -1)
                    {
                        Tools.ShowErrorLog(Log.WNIOSKINADG, p[i], "Błędne dane zaznaczenia pracowników.");
                        return;
                    }
                    else p[i] = aa.ToString();   // zeby nie było wstrzyknięcia kodu 
                }
                string list = String.Join(",", p);
                DataSet ds = db.select(dsPracownicy.InsertCommand, list);
                list = db.Join(ds, 0, "|");

                cntNadgodzinyWnioskiModal.Show(list);
            }
            else
                Tools.ShowMessage("Proszę zaznaczyć pracowników.");






            //cntNadgodzinyWnioskiModal.Show(null, null, null, null, "100", "13", RCP.Controls.cntNadgodzinyWnioskiModal.EType.DoWyplaty, "uwagi", "powód");
        }

        protected void cntWnioskiNadgodzinyModal_Sent(object sender, EventArgs e)
        {
            gvList.DataBind();
        }

        protected void cbSelect_CheckedChanged(object sender, EventArgs e)
        {
            SetButtonsVisible();
            bool allChecked = true;
            foreach (GridViewRow row in gvList.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null)
                    if(!cb.Checked)
                    {
                        allChecked = false;
                        break;
                    }
            }

            if(allChecked)
            {
                CheckBox cbAll = (CheckBox)gvList.HeaderRow.FindControl("cbSelectAll");
                if (cbAll != null)
                    cbAll.Checked = true;
            }

        }

        protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool check = (sender as CheckBox).Checked;

            // Iterate through the Products.Rows property
            foreach (GridViewRow row in gvList.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null)
                    cb.Checked = check;
            }
            SetButtonsVisible();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        bool Print()
        {
            StringBuilder sb = new StringBuilder();

            DataTable dt = db.Select.Table(dsPrint, db.strParam(String.Join(",", GetSelected())));
            sb.Append("<div class='wnouter'>");
            string lastPracId = "";
            int counter = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string pracId = db.getStr(dr["IdPracownika"]);
                if (pracId != lastPracId)
                {
                    if (counter > 0)
                        sb.Append("</table></div>");

                    sb.Append("<div class='wninner break pdf-page pdf-landscape'>");
                    sb.Append("<span class='kartanadg pdf-title'>KARTA NADGODZIN</span>");

                    sb.AppendFormat(@"<table class='tablenadg'>
                                        <tr class='row1'>
                                            <td class='col1'></td>
                                            <td class='nazwim' colspan='10'><span class='l'>NAZWISKO I IMIĘ:</span><span class='r'>{0}</span></td>
                                            <td class='mies' colspan='4'><span class='l'>MIESIĄC:</span><span class='r'>{1}</span></td></tr>"
                        , db.getValue(dr, "Pracownik")
                        , Tools.MonthFriendlyName(db.getInt(dr, "MonthNo").Value)
                        );


                    sb.AppendFormat(@"<tr class='row2'>
                                        <td rowspan='2' class='col1'></td>
                                        <td rowspan='2' class='col2'>DATA</td>
                                        <td rowspan='2' class='col3'>DZIEŃ TYGODNIA</td>
                                        <td rowspan='2' class='col4'>ZMIANA</td>
                                        <td rowspan='2' class='col5'>OD godziny</td>
                                        <td rowspan='2' class='col6'>DO godziny</td>
                                        <td rowspan='2' class='col7'>LICZBA GODZIN 50% GODZIN</td>
                                        <td rowspan='2' class='col8'>LICZBA GODZIN 100% GODZIN</td>
                                        <td rowspan='2' class='col9'>LICZBA GODZIN NOCNYCH GODZIN</td>
                                        <td rowspan='1' colspan='2' class='col10'>Typ nadgodzin</td>
                                        <td rowspan='2' class='col11'>powód - czynności wykonywane</td>
                                        <td rowspan='2' class='col12'>PODPIS PRACOWNIKA</td>
                                        <td rowspan='2' class='col13'>PODPIS ZLECAJĄCEGO</td>
                                        <td rowspan='2' class='col14'>Podpis Dyrektora LUB OSOBY UPOWAŻNIONEJ</td>
                                    
                                    </tr>");

                    sb.AppendFormat(@"<tr class='row3'>                                     
                                        <td rowspan='1' class='col1'>do odebrania</td>
                                        <td rowspan='1' class='col2'>do zapłaty</td></tr>");

                    sb.Append("</div>");
                }
                lastPracId = pracId;

                sb.AppendFormat(@"<tr class='rowx'>
                                        <td class='col1'>Pozwolenie na nadgodziny w  dniu:</td>
                                        <td class='col2'>{0}</td>
                                        <td class='col3'>{1}</td>
                                        <td class='col4'>{2}</td>
                                        <td class='col5'>{3}</td>
                                        <td class='col6'>{4}</td>
                                        <td class='col7'>{5}</td>
                                        <td class='col8'>{6}</td>
                                        <td class='col9'>{7}</td>
                                        <td class='col10'>{8}</td>
                                        <td class='col11'>{9}</td>
                                        <td class='col12'>{10}</td>
                                        <td class='col13'>{11}</td>
                                        <td class='col14'>{12}</td>
                                        <td class='col15'>{13}</td>
                                    </tr>"
                                    , db.getValue(dr, "Data")
                                    , db.getValue(dr, "WeekDay")
                                    , db.getValue(dr, "Zmiana")
                                    , db.getValue(dr, "Od")
                                    , db.getValue(dr, "Do")
                                    , db.getValue(dr, "Nadg50")
                                    , db.getValue(dr, "Nadg100")
                                    , db.getValue(dr, "Noc")
                                    , ""
                                    , ""
                                    , db.getValue(dr, "Powod")
                                    , ""
                                    , ""
                                    , "");

                sb.AppendFormat(@"<tr class='rowx2'><td class='col1'></td><td class='col2'>Uwagi</td><td class='col3' colspan='13'>{0}</td></tr>", db.getValue(dr, "Uwagi"));
                counter++;
            }
            sb.Append("</table></div></div>");

            PDF PDF = new PDF();
            PDF.CssClasses[0] = "../styles/master3.css";
            PDF.Options = "-O landscape";
            if (PDF.Download(PDF.Prepare(sb.ToString(), Request), Server, Response, Request, "Karta nadgodzin") != 0)
                return false;
            return true;
        }

        protected void btnAcceptConfirm_Click(object sender, EventArgs e)
        {
            string errorMsg = "";
            string[] selected = GetSelected();

            DataTable dt = db.Select.Table(dsAccError, db.strParam(String.Join(",", selected)));


            StringBuilder sb = new StringBuilder();
            sb.Append("Czy na pewno chcesz zaakceptować wybrane wnioski?");
            if (dt.Rows.Count > 0)
            {
                //sb.Append("<br />Liczba nadgodzin na wniosku nie zgadza się z tymi na Planie Pracy u podanych pracowników: <br />");
                sb.Append("<br />Liczba nadgodzin na wniosku nie zgadza się z nadgodzinami wypracowanymi u pracowników: <br />");

                foreach (DataRow dr in dt.Rows)
                {
                    sb.AppendFormat("{0}<br />", db.getValue(dr, "Error"));
                }
            }
            Tools.ShowConfirm(sb.ToString(), btnAccept);
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvList.Rows)
            {
                string id = gvList.DataKeys[row.RowIndex].Value.ToString();
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null && cb.Checked)
                {
                    db.Execute(dsAccept, App.User.Id, App.User.OriginalId, id);
                }
            }
            gvList.DataBind();
        }

        protected void btnRejectConfirm_Click(object sender, EventArgs e)
        {
            cntRejectModal.Show();
            //Tools.ShowConfirm("Czy na pewno chcesz odrzucić wybrane wnioski?", btnReject);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvList.Rows)
            {
                string id = gvList.DataKeys[row.RowIndex].Value.ToString();
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null && cb.Checked)
                {
                    db.Execute(dsReject, App.User.Id, App.User.OriginalId, id, tbRejectReason.Text);
                }
            }
            gvList.DataBind();
        }

        private void SelectTab()
        {
            switch (cntSqlTabs.SelectedValue)
            {
                case "2":
                    divAcc.Visible = true;
                    break;
                default:
                    btnAcceptConfirm.Enabled = btnAccept.Enabled = false;
                    btnRejectConfirm.Enabled = btnReject.Enabled = false;
                    btnPrint.Enabled = false;
                    btnRejectAccepted.Enabled = false;
                    divAcc.Visible = false;
                    break;
            }
            //---
            bool p = cntSqlTabs.SelectedValue == "-88";
            gvList.Visible = !p;
            paStatus.Visible = !p;
            paPracownicy.Visible = p;
            btnAddRequest.Visible = p;
            paButtons.Visible = !p;
            btnRejectAccepted.Visible = App.User.IsAdmin && App.User.HasRight(AppUser.rWnNadgAccAll) && cntSqlTabs.SelectedValue == "3";
        }

        protected void cntSqlTabs_SelectTab(object sender, EventArgs e)
        {
            SelectTab();
        }

        void SetButtonsVisible()
        {
            bool anyChecked = false;

            foreach (GridViewRow row in gvList.Rows)
            {
                // Access the CheckBox
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null && cb.Checked)
                {
                    anyChecked = true;
                    break;
                }
            }
            btnAcceptConfirm.Enabled = btnAccept.Enabled = btnReject.Enabled = btnRejectConfirm.Enabled = anyChecked && (cntSqlTabs.SelectedValue == "2");
            btnPrint.Enabled = anyChecked;
            btnRejectAccepted.Enabled = anyChecked;

            //divAdmin.Visible = App.User.IsAdmin && anyChecked;
        }

        string[] GetSelected()
        {
            List<string> list = new List<string>();
            foreach (GridViewRow row in gvList.Rows)
            {
                string id = gvList.DataKeys[row.RowIndex].Value.ToString();
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                if (cb != null && cb.Checked)
                    list.Add(id);
            }
            return list.ToArray();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = Tools.GetText(e.Row, "hidStatus");
                switch(status)
                {
                    case "2":
                        e.Row.CssClass = "success";
                        break;
                    case "-1":
                        e.Row.CssClass = "danger";
                        break;
                    default:
                        break;
                }
            }
        }

        protected void ddlPracownik_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cntSqlTabs_DataBound(object sender, EventArgs e)
        {
            if(!App.User.IsAdmin)
            {
                Tools.RemoveMenu(cntSqlTabs.Tabs, "-99");
            }

            if(!App.User.HasRight(AppUser.rWnNadgAccAll))
            {
                Tools.RemoveMenu(cntSqlTabs.Tabs, "2");
                Tools.RemoveMenu(cntSqlTabs.Tabs, "3");
                Tools.RemoveMenu(cntSqlTabs.Tabs, "4");
            }

            if (cntSqlTabs.Tabs.Items.Count > 0)
            {
                if(cntSqlTabs.Tabs.Items.Count == 1)
                {
                    cntSqlTabs.Visible = false;
                }
                else
                {
                    cntSqlTabs.Tabs.Items[0].Selected = true;
                    SelectTab();
                }
            }
            else
                App.ShowNoAccess("", App.User);
        }

        //protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        //{
        //    Tools.ShowConfirm("Czy na pewno chcesz usunąć wybrane wnioski?", btnDelete);
        //}

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach(string id in GetSelected())
            {
                db.Execute(dsDelete, id);
            }
            gvList.DataBind();
            SetButtonsVisible();
        }

        protected void ddlKierownik_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void gvPracownicyCmd_Click(object sender, EventArgs e)
        {

        }

        protected void ddlKierownik_DataBound(object sender, EventArgs e)
        {
            Tools.SelectItem((DropDownList)sender, App.User.Id);
        }

        protected void btnRejectAccepted_Click(object sender, EventArgs e)
        {
            string[] selected = GetSelected();
            string command = "";
            foreach(string id in selected)
            {
                command += string.Format(dsReject.DeleteCommand, db.sqlPut(id)) + "\n";
            }
            db.Execute(command);
            gvList.DataBind();
        }
    }
}