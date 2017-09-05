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

namespace HRRcp.Controls.WnioseZmianaDanych
{
    public partial class cntlistview : System.Web.UI.UserControl
    {

        public EventHandler Zoombind2;
        public EventHandler del;
        public EventHandler del2;
        public EventHandler del3;
        public EventHandler Close1;
        public EventHandler Close2;
        public EventHandler Close3;
        public string Uzasadnienie;
        string MenuText;
        


        int Status = 0;
        int Typ = 0;
        public string cntnr { get; set; }
        /*
         * kontrolka  - 1 cntzoom
         *            - 2 getname
         */
        public string Lvl { get; set; }
        /* user     1 - pracownik
         *          2 - Admin HR
         *               
         */

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (Typ == 0) (ListView1.FindControl("ObWartosclb") as Label).Visible=false;

            switch (cntnr.ToString())
            {
                case "1":
                    ZoomDiv.Text = string.Format(@" <div id=""DivZoom_Zoom"" title=""Urlop pracownika"" style=""display:none;"" class=""PracUrlopy"">");
                    break;
                case "2":
                    ZoomDiv.Text = string.Format(@" <div id=""DivZoom_GetName"" title=""Urlop pracownika"" style=""display:none;"" class=""PracUrlopy"">");
                    
                    break;

            }


        }
        protected void Page_Init(object sender, EventArgs e)
        {

            Tools.PrepareDicListView(ListView1, 0, true, false, true);

        }

        public void Show()
        {
          
            DataRow dr = db.getDataRow(String.Format("select Status as Name from poWnioskiUrlopoweStatusy where Id={0}", Status));
            switch (cntnr.ToString())
            {
                case "1":
                    if (Status == 0) { Tools.ShowDialogConfirm(this, "DivZoom_Zoom", null, Bt_Dialog_Close, db.getValue(dr, "Name").ToString(), TAK, NIE, "Zapisać wniosek ?"); }
                    else
                    {
                        Tools.ShowDialog(this, "DivZoom_Zoom", null, Bt_Dialog_Close, db.getValue(dr, "Name").ToString());
                    }
                    break;
                case "2":
                   
                    if (Status == 0) { Tools.ShowDialogConfirm(this, "DivZoom_GetName", null, Bt_Dialog_Close, db.getValue(dr, "Name").ToString(), TAK, NIE, "Zapisać wniosek ?"); }
                    else
                    {
                        Tools.ShowDialog(this, "DivZoom_GetName", null, Bt_Dialog_Close, db.getValue(dr, "Name").ToString());
                    }
                    break;
            }


        }



        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

            var Tb = e.Item.FindControl("TextBoxCnt") as TextBox;
            var De = e.Item.FindControl("DateEditCnt") as DateEdit;
            var Lb = e.Item.FindControl("LabelCnt") as Label;
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            int Type = (int)DataBinder.Eval(dataItem.DataItem, "Typ");
            
            if (Status > 0) // jesli wniosek został złożony.
            {
                De.Visible = false;
                Tb.Visible = false;
                Lb.Visible = true;
                Lb.Text = DataBinder.Eval(dataItem.DataItem, "Nowa_Wartosc").ToString();
            }
            else
            { // tworzenie wniosku

                Lb.Visible = false;
                switch (Type)
                { //// tworzenie kontroli (label lub dateedit) 
                    case 0:
                        De.Visible = false;
                        try
                        {
                            Tb.Text = DataBinder.Eval(dataItem.DataItem, "Nowa_Wartosc").ToString().Replace("''", "'");; // Jeśli wniosek jest cof zeby wypisało star wartosci
                        }
                        catch { }
                        break;
                    case 1:
                        Tb.Visible = false;
                        try
                        {
                            (De.FindControl("tbDate") as TextBox).Text = DataBinder.Eval(dataItem.DataItem, "Nowa_Wartosc").ToString().Replace("''", "'"); ;
                        }
                        catch { }
                        break;

                }


            }

           
            if (Typ == 0) e.Item.FindControl("Obecna_wartoscLabel").Visible = false;
            if (Typ == 2)
            {
                
                e.Item.FindControl("LabelCnt").Visible = false;
                switch (Type)
                {
                    case 0: e.Item.FindControl("TextBoxCnt").Visible = false; break;
                    case 1:
                        e.Item.FindControl("DateEditCnt").Visible = false;
                        break;
                }


            }




        }
        public void Zoom(String Id)
        {
            
            //ListView1.FindControl("Button1").Visible = false; // chowanie przycisku wysyłania wniosku , już jest wysłany
            HiddenField1.Value = "Zoom";
            ListView1.Visible = true;
            Dane_Pracownika.Visible = true;
            DataRow dr = db.getDataRow(String.Format(@"select 
WO.Typ,
WU.Status,
WU.Id,
WO.UwagiKadry as Uwagi ,
WO.MenuText
from poWnioskiDaneOsoboweInfo WO 
left outer join poWnioskiUrlopoweStatusy WU on WU.Id=WO.StatusId 
where WO.Id={0}", Id));// pobranie statusu wniosku 
            Imie_Nazwisko.Text = App.User.ImieNazwisko;
            Nrewid.Text = App.User.NR_EW;
            Status_Wniosku.Text = db.getValue(dr, "Status");
            Status = int.Parse(db.getValue(dr, "Id").ToString());
            Typ = int.Parse(db.getValue(dr, "Typ").ToString());
             MenuText = db.getValue(dr, "MenuText");
            Uzasadnienie = db.getValue(dr, "Uwagi").ToString();
            Uzasadnienie = Uzasadnienie.Replace("''", "'");
            if (Id.ToString() != "") WniosekId.Value = Id.ToString();
            switch (Typ)
            {
                case 0:
                    Temat.Text = "Wniosek o dodanie nowych Danych";
                    break;
                case 1:
                    Temat.Text = "Wniosek o modyfikację obecnych danych";
                    break;
                case 2:
                    Temat.Text = "Wniosek o Skasowanie obecnych danych";
                    break;
            }
            SqlDataSource1.SelectCommand = string.Format(@"
select Nazwa_pola,Obecna_wartosc,Typ,Nowa_Wartosc from poWnioskiDaneOsobowePola where WniosekId={0}", Id);// pobranie pol
            Show();

        }
        public void Pokaz(String id)
        {
            Show();
            HiddenField1.Value = "Pokaz";
            DataRow dr = db.getDataRow(String.Format(@"select 
WO.Typ,
WU.Status,
WU.Id,
WO.MenuText 
from poWnioskiDaneOsoboweInfo WO 
left outer join poWnioskiUrlopoweStatusy WU on WU.Id=WO.StatusId 
where WO.Id={0}", id));// pobranie statusu wniosku
            Typ = int.Parse(db.getValue(dr, "Typ").ToString());
            WniosekId.Value = id.ToString();
            ListView1.Visible = true;
            MenuText = db.getValue(dr, "MenuText");
            SqlDataSource1.SelectCommand = string.Format(@"
select Nazwa_pola,Obecna_wartosc,Typ from poWnioskiDaneOsobowePola where WniosekId={0}", id); // pobranie pol

        }
        public void CloseDialog()
        {
            Tools.DistableButtonDialog();
            Tools.CloseDialog("DivZoom_Report");
            switch (cntnr.ToString())
            {
                case "1":
                    Tools.CloseDialog("DivZoom_Zoom");
                    break;
                case "2":
                    Tools.CloseDialog("DivZoom_GetName");
                    break;
            }

            Close1(1, null);

            //ListView1.Visible = false;

        }
        public string GetWniosekId()
        {
            return WniosekId.Value;
        }


        public void UpdateInfo(int Status)
        {

            String UwagiValue = (ListView1.FindControl("Uzasadnienie") as TextBox).Text;
            UwagiValue = UwagiValue.Replace("'", "''");
           
                if (Status == 100)
                {
                    db.insert(String.Format(@"delete from poWnioskiDaneOsoboweInfo where Id={0};
                                      delete from poWnioskiDaneOsobowePola where WniosekId={0};", WniosekId.Value));
                    //        if (Lvl.ToString() == "1" && cntnr.ToString() == "1") del(1, null);
                    //      if ((Lvl.ToString() == "1" || Lvl.ToString() == "2") && cntnr.ToString() == "2") del2(1, null);

                    // musi byc potwierdzenie kasowania !


                }
                else
                {



                    if (Status == 200)
                    {
                        int Stat = int.Parse(db.getValue(db.getDataRow(String.Format(@"Select StatusId from poWnioskiDaneOsoboweInfo where Id={0};", WniosekId.Value)), "StatusId"));
                        switch (Stat)
                        {
                            case 1:
                                db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=0, UwagiKadry='{1}' , DataAkcep=null , IdAkcept=null  where Id={0};", WniosekId.Value, UwagiValue));
                                break;
                            case 2:
                                db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=1, UwagiKadry='{1}' , DataAkcep=GETDATE() , IdAkcept={2}  where Id={0};", WniosekId.Value, UwagiValue, App.User.Id));
                                break;
                            case 3:
                                db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=1, UwagiKadry='{1}' , DataAkcep=GETDATE() , IdAkcept={2}  where Id={0};", WniosekId.Value, UwagiValue, App.User.Id));
                                break;
                            case 4:
                                db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=3, UwagiKadry='{1}' , DataAkcep=GETDATE() , IdAkcept={2} , DataWpro=null where Id={0};", WniosekId.Value, UwagiValue, App.User.Id));
                                break;
                        }

                    }
                    else
                    {

                        if (Status == 4)
                        {

                            db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId={1} , UwagiKadry='{2}' , DataAkcep=GETDATE() , IdAkcept={3} ,DataWpro=GETDATE() where Id={0};", WniosekId.Value
                                                                                                                 , Status
                                                                                                                 , UwagiValue
                                                                                                                 , App.User.Id));
                        }
                        else
                        {

                            db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId={1} , UwagiKadry='{2}' , DataAkcep=GETDATE() , IdAkcept={3}  where Id={0};", WniosekId.Value
                                                                                                                    , Status
                                                                                                                    , UwagiValue
                                                                                                                    , App.User.Id));

                        }
                    }
                
            }
            (ListView1.FindControl("Uzasadnienie") as TextBox).Text = null;

            CloseDialog();
        }
        protected void Bt_Akceptuj(object sender, EventArgs e) // dodawanie nowych wartosci
        {
            // CloseDialog();
            // db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=3 where Id={0};", WniosekId.Value));
            UpdateInfo(3);

        }
        protected void Bt_Wprowadzone(object sender, EventArgs e) // dodawanie nowych wartosci
        {
            UpdateInfo(4);
            //db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=4 where Id={0};", WniosekId.Value));
            //CloseDialog();
        }
        protected void Bt_AW(object sender, EventArgs e) // dodawanie nowych wartosci
        {
            UpdateInfo(4);
            //db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=4 where Id={0};", WniosekId.Value));
            //CloseDialog();
        }
        protected void Bt_Odrzuc(object sender, EventArgs e) // dodawanie nowych wartosci
        {

            UpdateInfo(2);
            //db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=2 where Id={0};", WniosekId.Value));
            //CloseDialog();

        }
        protected void Bt_Cofnij(object sender, EventArgs e) // dodawanie nowych wartosci
        {

            UpdateInfo(200);




        }
        protected void Bt_Zamknij(object sender, EventArgs e) // dodawanie nowych wartosci
        {
           
            CloseDialog();





        }
        protected void Button3_Click(object sender, EventArgs e) // dodawanie nowych wartosci
        {


           
            CloseDialog();
            // zapis...

            string Value = "";
            
            foreach (ListViewItem item in ListView1.Items)
            {
                var Tb = item.FindControl("TextBoxCnt") as TextBox;
                var De = item.FindControl("DateEditCnt") as DateEdit;
                if (Tb.Visible == true) Value = Tb.Text.ToString();
                if (De.Visible == true) Value = (De.FindControl("tbDate") as TextBox).Text;
                
            }
            DataRow dr = db.getDataRow(string.Format("select Typ from poWnioskiDaneOsoboweInfo where Id={0}", WniosekId.Value));
            Typ = int.Parse(db.getValue(dr, "Typ").ToString());
            


            //CloseDialog();


            foreach (ListViewItem item in ListView1.Items)
            {
                String Name = (item.FindControl("Nazwa_polaLabel") as Label).Text;
                var Tb = item.FindControl("TextBoxCnt") as TextBox;
                var De = item.FindControl("DateEditCnt") as DateEdit;
                if (Tb.Visible == true) Value = Tb.Text.ToString().Replace("'", "''");
                if (De.Visible == true) Value = (De.FindControl("tbDate") as TextBox).Text.Replace("'", "''"); ;
                db.insert(String.Format(@"update poWnioskiDaneOsobowePola set Nowa_Wartosc='{0}' where WniosekId={1} and Nazwa_pola='{2}'", Value
                                                                                                                                           , WniosekId.Value
                                                                                                                                           , Name));
            }

            // ListView1.Visible = false;
           // Zoombind2(1, null); // odswieza zoom'a
           // if (Lvl.ToString() == "1" && cntnr.ToString() == "1") Dane_Pracownika.Visible = false;
            //App_Code.Tools.ShowMessage("Wniosek został Zapisany");
        }


        



        protected void Button2_Click(object sender, EventArgs e) // dodawanie nowych wartosci
        {


            UpdateInfo(100);


        }
        protected void Button1_Click(object sender, EventArgs e) // dodawanie nowych wartosci
        {
            string Value = "";
            bool isnull = true;
            foreach (ListViewItem item in ListView1.Items)
            {
                var Tb = item.FindControl("TextBoxCnt") as TextBox;
                var De = item.FindControl("DateEditCnt") as DateEdit;
                if (Tb.Visible == true) Value = Tb.Text.ToString();
                if (De.Visible == true) Value = (De.FindControl("tbDate") as TextBox).Text;
                if (!string.IsNullOrEmpty(Value)) { isnull = false; break; }
            }
            DataRow dr = db.getDataRow(string.Format("select Typ from poWnioskiDaneOsoboweInfo where Id={0}",WniosekId.Value));
            Typ = int.Parse(db.getValue(dr, "Typ").ToString());
            if (Typ == 2) isnull = false; 
            switch (isnull)
            {
                case false:

                    
                    db.insert(String.Format(@"update poWnioskiDaneOsoboweInfo set StatusId=1 where Id={0};", WniosekId.Value));

                    foreach (ListViewItem item in ListView1.Items)
                    {
                        String Name = (item.FindControl("Nazwa_polaLabel") as Label).Text;
                        var Tb = item.FindControl("TextBoxCnt") as TextBox;
                        var De = item.FindControl("DateEditCnt") as DateEdit;
                        if (Tb.Visible == true) Value = Tb.Text.ToString().Replace("'", "''");
                        if (De.Visible == true) Value = (De.FindControl("tbDate") as TextBox).Text.Replace("'", "''"); ;
                        db.insert(String.Format(@"update poWnioskiDaneOsobowePola set Nowa_Wartosc='{0}' where WniosekId={1} and Nazwa_pola='{2}'", Value
                                                                                                                                                   , WniosekId.Value
                                                                                                                                                   , Name));
                    }

                    // ListView1.Visible = false;
                    Zoombind2(1, null); // odswieza zoom'a
                    if (Lvl.ToString() == "1" && cntnr.ToString() == "1") Dane_Pracownika.Visible = false;
                    CloseDialog();
                    break;
                case true:
                    App_Code.Tools.ShowMessage("Proszę uzupełnić przynajmniej 1 pole");
                    break;
            }
            
            
            App_Code.Tools.ShowMessage("Wniosek został wysłany");

        }

        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            if (HiddenField1.Value == "Zoom") {

                (ListView1.FindControl("Bt_Usun") as Button).Text = "Usuń";
                (ListView1.FindControl("Bt_Usun") as Button).OnClientClick = "return confirm('Usunąć wniosek ?');";
            }
            else
            {
                (ListView1.FindControl("Bt_Usun") as Button).OnClientClick = "";
                (ListView1.FindControl("Bt_Usun") as Button).Text = "Anuluj";
            }

            if (Lvl.ToString() == "2") { ListView1.FindControl("uzasadnienieDiv").Visible = true; }
            else
            {
                if (!string.IsNullOrEmpty(Uzasadnienie))
                {
                    ListView1.FindControl("uzasadnienieDiv").Visible = true;
                    (ListView1.FindControl("Uzasadnienie") as TextBox).ReadOnly = true;
                }
                else
                {

                    ListView1.FindControl("uzasadnienieDiv").Visible = false;
                }

            }
            if (Status == 0) {
                ListView1.FindControl("Bt_Zamknij").Visible = false;
                ListView1.FindControl("Bt_Zapisz").Visible = true;

            }
            else
            {

                ListView1.FindControl("Bt_Zamknij").Visible = true;
                ListView1.FindControl("Bt_Zapisz").Visible = false;
            }
            if (!string.IsNullOrEmpty(Uzasadnienie))
            {
                (ListView1.FindControl("Uzasadnienie") as TextBox).Text = Uzasadnienie;
            }
            if (Typ == 0) { (ListView1.FindControl("ObWartosclb") as Label).Visible = false; } else { (ListView1.FindControl("ObWartosclb") as Label).Visible = true; }

            if (Typ == 2) { (ListView1.FindControl("NwWartosclb") as Label).Visible = false; } else { (ListView1.FindControl("NwWartosclb") as Label).Visible =true; }
            if (Status > 0) { ListView1.FindControl("Bt_Wyslij").Visible = false;
            
            }
            else
            {
                ListView1.FindControl("Bt_Wyslij").Visible = true;
            }
            if (Status > 0) { ListView1.FindControl("Bt_Usun").Visible = false; }
            else
            { if ((Lvl.ToString() == "1")) ListView1.FindControl("Bt_Usun").Visible = true; }
            if (Status == 3 && Lvl.ToString() == "2") { ListView1.FindControl("Bt_Wprowadzone").Visible = true; } else { ListView1.FindControl("Bt_Wprowadzone").Visible = false; }// jesli zaakceptowny pokaż bt Wprowadzone
            if (Status > 0 && Lvl.ToString() == "2") { ListView1.FindControl("Bt_Cofnij").Visible = true; } else { ListView1.FindControl("Bt_Cofnij").Visible = false; } // jeśli admin i po wysłaniu , może cofnąć status
            if (Status == 1 && Lvl.ToString() == "2")
            {
                ListView1.FindControl("Bt_Odrzuc").Visible = true;
                ListView1.FindControl("Bt_Akceptuj").Visible = true;
                ListView1.FindControl("Bt_AW").Visible = true;
            }
            else
            {
                ListView1.FindControl("Bt_Odrzuc").Visible = false;
                ListView1.FindControl("Bt_Akceptuj").Visible = false;
                ListView1.FindControl("Bt_AW").Visible = false;
            }

        }

        protected void Bt_Dialog_Close_Click(object sender, EventArgs e)
        {
            //DataRow dr = db.getDataRow(string.Format("select StatusId from poWnioskiDaneOsoboweInfo where Id={0}", WniosekId.Value));
            //Status = int.Parse(db.getValue(dr, "StatusId").ToString());
           // if (Status==0)
           // {
                //Tools.ShowConfirm("Czy zapisać wniosek ?", TAK, NIE);
            //    HiddenField1.Value = "";
           // }
          ///  else {
                //CloseDialog();
               
           // }

        }

        protected void NIE_Click(object sender, EventArgs e)
        {
            Tools.CloseDialog("DivZoom_Report");
            //Bt_Zamknij(null, null);
        }

        protected void TAK_Click(object sender, EventArgs e)
        {
            Tools.CloseDialog("DivZoom_Report");
            Button3_Click(null, null);
           // App_Code.Tools.ShowMessage("Zapisano wniosek");
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        { 
            Label lb = ListView1.Controls[0].FindControl("NwWartosclb") as Label ;
            if (lb != null) lb.Text = "Nowe " + MenuText;
        }



    }
}