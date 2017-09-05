using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Configuration;
using HRRcp.App_Code;



namespace HRRcp.Controls.WniosekZmianaDanych
{
    public partial class cntzoom : System.Web.UI.UserControl
    {
        bool v = false;
        private String PracId;
        private String Typ;
        private String PracName;
        private DateTime Start = DateTime.Parse("1995-01-01") ;
        private DateTime End = DateTime.Parse("1995-01-01");
        public String Status2;
        public string Status { get; set; }
        /* Status   1 - Do akceptacji
         *          2 - Odrzucony
         *          3 - Zaakceptowany
         *          4 - Zaakceptowany - wprowadzony
         *          W - wszystkie
         *          AZ - Zoom przy dodawaniu wniosku (administracja)
         *               
         */
        public string Lvl { get; set; }
        /* user     1 - pracownik
         *          2 - Admin HR
         *               
         */
        
        //protected void GetId(object sender, CommandEventArgs e)
        //{

        //    if (e.CommandArgument == null)
        //    {
        //        ////
        //    }
        //    else
        //    {
        //        App_Code.Tools.ShowMessage(e.CommandArgument.ToString());
        //        Listview2.Zoom(e.CommandArgument.ToString());
        //        Listview2.Visible = true;
        //    }
        //}
        protected void Del(object sender, EventArgs e)
        {
            ListView1.DataBind();
            ListView1.Visible=true;
            Listview20.Visible = false;
           // App_Code.Tools.ShowMessage("Wniosek został skasowany !");
        }
        protected void Close(object sender, EventArgs e)
        {
            ListView1.DataBind(); /// ja pracownik zamyka nowy to trzeba odsiezyc liste
            ListView1.Visible = true;
            //Listview2.Visible = false;
        }
        protected void DateChanged(object sender, EventArgs e)
        {
            GetSelectCommand();
        }
        public void SetPracId(string id) {
            PracIdGetName.Value = id;
        }
        protected void zmbind(object sender, EventArgs e)
        {
            ListView1.DataBind();
            ListView1.Visible = true ;

        }
        public String prepare(String SQL)
        {
            
            String Sql = SQL;

            if (Status == "W") { DivStatus.Visible = true; LabelPrac.Visible = true; DropDownList1.Visible = true; } else { DivStatus.Visible = false;  }
            if (Status2 == "W") { DivStatus.Visible = true; LabelPrac.Visible = true; DropDownList1.Visible = true; } else { DivStatus.Visible = false;  }
            if (Status=="AZ" || Status=="W") {
                
                Sql = Sql.Replace("@StatusId@", "WO.StatusId is not null "); }
            else
            {
                
                Sql = Sql.Replace("@StatusId@", "WO.StatusId=" + Status);
                
            }
            if (string.IsNullOrEmpty(PracId)) { Sql = Sql.Replace("@PracId@", " "); }
            else
            {
                Sql = Sql.Replace("@PracId@", " and PR.Id=" + PracId);
                
            }
            if (string.IsNullOrEmpty(Typ)) { Sql = Sql.Replace("@Typ@", " "); }
            else
            {
                Sql = Sql.Replace("@Typ@", " and WO.Typ=" + Typ);
                
            }
            if (string.IsNullOrEmpty(PracName)) { Sql = Sql.Replace("@PracName@", " "); }
            else
            {
                Sql = Sql.Replace("@PracName@", " and (PR.Imie+PR.Nazwisko) LIKE '%" + PracName + "%'");
                
            }
            if (Start == DateTime.Parse("1995-01-01")) { Sql = Sql.Replace("@Start@", " "); }
            else
            {
                Sql = Sql.Replace("@Start@", " and WO.DataDod between '" + Start + "'");
                
            }


            if (End == DateTime.Parse("1995-01-01"))
            {

                if (Start == DateTime.Parse("1995-01-01"))
                {
                    Sql = Sql.Replace("@End@", " ");
                }
                else
                {
                    
                    Sql = Sql.Replace("@End@", "and '2999-01-01' ");

                }
            }
            else
            {
                if (Start == DateTime.Parse("1995-01-01")) { Sql = Sql.Replace("@End@", String.Format(" and WO.DataDod between '2001-01-01' and ") + End); }
                else
                {
                    Sql = Sql.Replace("@End@", "and '" + End + "'");
                   

                }
            }

           
            return Sql;
        }
        public void GetSelectCommand()
        {

            switch (Lvl)
            {


                case "1": SqlDataSource1.SelectCommand = @"select 
WO.DataDod, 
WO.Id,
WU.Status ,
WT.Nazwa as typ,
(PR.Nazwisko + ' ' + PR.Imie) as imie 
from poWnioskiDaneOsoboweInfo WO
left outer join Pracownicy PR on PR.Id=WO.Autor 
left outer join poWnioskiUrlopoweStatusy WU on WO.StatusId=WU.Id 
left outer join poWnioskiDaneOsoboweTyp WT on WT.StatusId=WO.Typ 
where Autor= 
"  + App.User.Id + " order by PR.Nazwisko ";
                    

                   
                    break;
                case "2":
                    bool DO = false;
                    if (Status == "AZ") {

                        if (PracIdGetName.Value != "H")
                        {
                            tbsearchdiv.Visible = false;
                            if (Status2!="W") LabelPrac.Visible = false;
                            if (Status2 != "W") DropDownList1.Visible = false;
                            if (PracIdGetName.Value != "0") PracId = PracIdGetName.Value;
                            DO = true;
                            if (!string.IsNullOrEmpty((Start1.FindControl("tbDate") as TextBox).Text))
                            {
                                Start = DateTime.Parse(((Start1.FindControl("tbDate")) as TextBox).Text);
                            }
                            if (!string.IsNullOrEmpty((End1.FindControl("tbDate") as TextBox).Text))
                            {
                                End = DateTime.Parse(((End1.FindControl("tbDate")) as TextBox).Text);
                            }

                            //if (DropDownList1.SelectedIndex != 0) PracId = PracIdGetName.Value;
                            if (DropDownList2.SelectedIndex != 0) Typ = DropDownList2.SelectedValue;
                            //if (!string.IsNullOrEmpty(tbSearch.Text))
                           // {
                            //    tbSearch.Text = tbSearch.Text.Replace(" ", "");
                             //   PracName = tbSearch.Text.Replace("'", "''");
                           // }
                        



                        }
                        else {
                            DO = false;
                        }
                    
                    }
                    else
                    {
                        DO = true;
                        if (!string.IsNullOrEmpty((Start1.FindControl("tbDate") as TextBox).Text))
                        {
                            Start = DateTime.Parse(((Start1.FindControl("tbDate")) as TextBox).Text);
                        }
                        if (!string.IsNullOrEmpty((End1.FindControl("tbDate") as TextBox).Text))
                        {
                            End = DateTime.Parse(((End1.FindControl("tbDate")) as TextBox).Text);
                        }
                        if (Status == "W") Status2 = "W";
                       // if (DropDownList1.SelectedIndex != 0) PracId = DropDownList1.SelectedValue;
                        if (DropDownList3.SelectedIndex != 0) { Status = DropDownList3.SelectedValue; Status2 = "W"; } else { if (Status=="W") Status2 = "W";  }
                        if (DropDownList2.SelectedIndex != 0) Typ = DropDownList2.SelectedValue;
                        if (!string.IsNullOrEmpty(tbSearch.Text))
                        {
                            tbSearch.Text = tbSearch.Text.Replace(" ", "");
                            PracName = tbSearch.Text.Replace("'", "''");
                        }
                    }
                   if (DO==true) SqlDataSource1.SelectCommand = prepare(String.Format(@"
select
WO.DataDod, 
WO.Id,
WU.Status,
WT.Nazwa as typ,
(PR.Nazwisko + ' ' + PR.Imie) as imie
from poWnioskiDaneOsoboweInfo WO
left outer join poWnioskiUrlopoweStatusy WU on WO.StatusId=WU.Id
left outer join Pracownicy PR on PR.Id=WO.Autor 
left outer join poWnioskiDaneOsoboweTyp WT on WT.StatusId=WO.Typ
where @StatusId@ @PracId@ @PracName@ @Typ@ @Start@  @End@ order by PR.Nazwisko "));
                   // Label1.Text = SqlDataSource1.SelectCommand;
                    
                   break;
                   
            }
            ListView1.DataBind();
        }

        protected void DDL3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList3.SelectedIndex == 0) Status = "AZ";
            GetSelectCommand();
        }

        protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSelectCommand();
        }
        protected void DDL_Typ(object sender, EventArgs e)
        {
            GetSelectCommand();
            
        }




        protected void Page_Load(object sender, EventArgs e)
        {


            
            if (Lvl == "2")
            {
                
                
                paFilter.Visible = true;
                tbsearchdiv.Visible = true;
                Button3.Visible = true;
                Listview20.Lvl = "2";
                Listview20.cntnr = "1";

            }
            else {
                Button3.Visible = false;
                paFilter.Visible = false;
                tbsearchdiv.Visible = false;
                Listview20.Lvl = "1";
                Listview20.cntnr = "1";
                

            }
            if (!IsPostBack && Lvl=="2")
            {
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("Wybierz", String.Empty));
                DropDownList1.SelectedIndex = 0;
                DropDownList3.DataBind();
                DropDownList3.Items.Insert(0, new ListItem("Wybierz", String.Empty));
                DropDownList3.SelectedIndex = 0;
            }
            if (IsPostBack)
            {
                v = Visible;
            }
            if (Lvl == "1") paFilter.Visible = false;

            Start1.DateChanged += new EventHandler(DateChanged);
            End1.DateChanged += new EventHandler(DateChanged);
            Listview20.Close1 += new EventHandler(Close);
            Listview20.del += new EventHandler(Del);
            Listview20.Zoombind2 += new EventHandler(zmbind);
            GetSelectCommand();
        }
        public void bind() {
            ListView1.DataBind();
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            //if ((Visible && Visible != v) || (Lvl=="1" && IsPostBack)) 
            Tools.ExecOnStart2("searchwnU", String.Format("startSearch('{0}','{1}');",
                 tbSearch.ClientID,
                 Button1.ClientID));
            if (Lvl=="2")tbSearch.Focus();
          
                
            base.OnPreRender(e);

           
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            

            Tools.PrepareSorting(ListView1, 1, 15);
            Tools.PrepareDicListView(ListView1, 0, true, false , true);
            Tools.PrepareSorting(ListView1, 1, 15);
            
        }

        protected void GetId(object sender, CommandEventArgs e) 
        {
            if (e.CommandArgument == null)
            {
               
                ////
            }
            else
            {
                //ListView1.Visible = false;
                Listview20.Zoom(e.CommandArgument.ToString());
                Listview20.Visible = true;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GetSelectCommand();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Typ = null;
            PracName = null;
            PracId = null;
            Start = DateTime.Parse("1995-01-01") ;
            End = DateTime.Parse("1995-01-01");
            DropDownList1.SelectedIndex = 0;
            DropDownList2.SelectedIndex = 0;
            tbSearch.Text = null;
            (Start1.FindControl("tbDate") as TextBox).Text = null;
            (End1.FindControl("tbDate") as TextBox).Text = null;
            GetSelectCommand();
            ListView1.DataBind();
        }

        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            

        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            
        }
        protected void DataPager1_OnLoad(object sender, EventArgs e)
        {
            DataPager pager = ListView1.FindControl("DataPager1") as DataPager;
            if (pager != null)
            {
                if(pager.TotalRowCount>0){
                    pager.PageSize = pager.TotalRowCount;
                    if (Lvl == "1") pager.PageSize = 5;
                    if (Lvl == "2") pager.PageSize = 10;
                    pager.SetPageProperties(0 * pager.PageSize, pager.MaximumRows, true);
                }
            }
        }

        public SqlDataSource DataSource
        {
            get { return SqlDataSource1; }
        }
 
    }
}