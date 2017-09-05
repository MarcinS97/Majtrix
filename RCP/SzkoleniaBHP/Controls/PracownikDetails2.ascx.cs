using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.SzkoleniaBHP.Controls
{
    public partial class PracownikDetails2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Translate();
            }
        }

        private void Translate()
        {
        }

        public void Prepare(string pracId)
        {
            hidPracId.Value = pracId;
            hidData.Value = Tools.DateToStr(DateTime.Today);  // inicjacja na dziś
            SetSql();
            lvPracownik.DataBind();
        }

        public void Prepare2(string pracId)
        {
            Prepare(pracId);
            Visible = true;
        }

        public void Cancel()
        {
            /*  na razie ...
            if (!String.IsNullOrEmpty(hidPracId.Value))
            {
                hidPracId.Value = null;
                lvPracownik.DataBind();
            }
            */
            Visible = false;
        }
        //------
        public string GetStatus(object st)
        {
            int s;
            try   { s = Convert.ToInt32(st); }
            catch { s = -1; }
            return App.GetStatus(s);
        }

        private void SetSql()
        {
            /*
            string select = "select " +
                    "P.Id_Pracownicy," +
                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie," +
                    "P.Nazwisko," +
                    "P.Imie," +
                    "P.Imie2," +
                    "P.Nr_Ewid," +

                    "P.Id_Gr_Zatr," +
                    "G.Rodzaj_Umowy," +
                    "P.DataZatr, P.DataUmDo, P.DataZwol," +
                    "P.APT, P.Status," +

                    "P.Id_Stanowiska," +
                    "S.Nazwa_Stan," +

                    "M.Id_Parent as IdStrumieniaM," +
                    "MP.Symb_Jedn as SymbolStrumieniaM," +
                    "MP.Nazwa_Jedn as NazwaStrumieniaM," +
                    //"MP.ID_Upr_Przel as IdUprPrzelStrumieniaM," +
                    "MP.Id_Parent as IdParentaStrumieniaM," +

                    "P.Id_Str_OrgM as IdLiniiM," +
                    "M.Symb_Jedn as SymbolLiniiM," +
                    "M.Nazwa_Jedn as NazwaLiniiM," +
                    //"M.ID_Upr_Przel as IdUprPrzelLiniiM," +
                    "MK.Nazwisko + ' ' + MK.Imie as KierownikM, " +

                    "case when O.Id is not null then A.Id_Parent       else M.Id_Parent        end as IdStrumieniaA," +
                    "case when O.Id is not null then AP.Symb_Jedn      else MP.Symb_Jedn       end as SymbolStrumieniaA," +
                    "case when O.Id is not null then AP.Nazwa_Jedn     else MP.Nazwa_Jedn      end as NazwaStrumieniaA," +
                    //"case when O.Id is not null then AP.ID_Upr_Przel   else MP.ID_Upr_Przel    end as IdUprPrzelStrumieniaA," +
                    "case when O.Id is not null then AP.Id_Parent      else MP.Id_Parent       end as IdParentaStrumieniaA," +

                    "case when O.Id is not null then O.IdStruktury     else P.Id_Str_OrgM      end as IdLiniiA," +
                    "case when O.Id is not null then A.Symb_Jedn       else M.Symb_Jedn        end as SymbolLiniiA," +
                    "case when O.Id is not null then A.Nazwa_Jedn      else MP.Nazwa_Jedn      end as NazwaLiniiA," +
                    //"case when O.Id is not null then A.ID_Upr_Przel    else M.ID_Upr_Przel     end as IdUprPrzelLiniiA," +
                    "case when O.Id is not null then AK.Nazwisko + ' ' + AK.Imie " +
                                               "else MK.Nazwisko + ' ' + MK.Imie               end as KierownikA, " +
                    "O.Od, O.Do," +
                    
                    "0 as Ocena, " +
                    "P.Id_Status," +
                    "T.Status as Absencja" +
                "from Pracownicy P " +
                    "left outer join GrZatr G on G.Id_Gr_Zatr = P.Id_Gr_Zatr " +
                    "left outer join Stanowiska S on S.Id_Stanowiska = P.Id_Stanowiska " +
                    "left outer join StrOrg M on M.Id_Str_Org = P.Id_Str_OrgM " +             // Linia
                    "left outer join StrOrg MP on MP.Id_Str_Org = M.Id_Parent " +             // Strumień
                    "left outer join Przelozeni MK on MK.Id_Przelozeni = P.IdKierownika " +
                    "left outer join Oddelegowania O on O.IdPracownika = P.Id_Pracownicy and @Data between O.Od and O.Do and O.Status = " + Wnioski.stZaakceptowany.ToString() + " " +
                    "left outer join StrOrg A on A.Id_Str_Org = O.IdStruktury " +
                    "left outer join StrOrg AP on AP.Id_Str_Org = A.Id_Parent " +
                    "left outer join Przelozeni AK on AK.Id_Przelozeni = O.IdKierownika " +
                    "left outer join StatusPrac T on T.Id_Status_Prac = P.Id_Status " +
                "where Id_Pracownicy = @IdPracownika " + 
                "order by NazwiskoImie";
            
            SqlDataSource1.SelectCommand = select;
            */

            //SqlDataSource1.SelectCommand = String.Format(Pracownicy2._prac_select, "", "where Id_Pracownicy = @IdPracownika");
            SqlDataSource1.SelectCommand = String.Format("select * from Pracownicy {0} {1}", "", "where Id = @IdPracownika");
        }


/*
        
select A.* from Pracownicy A
join Pracownicy B on B.Nazwisko = A.Nazwisko and B.Imie = A.Imie 
and B.Id_Pracownicy <> A.Id_Pracownicy


group by Nazwisko 

select 
        */

        protected void lvPracownicy_LayoutCreated(object sender, EventArgs e)
        {
            SetSql();
        }

        protected void lvPracownicy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    for (int i = 1; i <= 14; i++)
                        L.p(e.Item, "Literal" + i.ToString());
                    break;
                case ListViewItemType.EmptyItem:
                    L.p(lvPracownik.Controls[0], "Literal1");
                    break;
            }
        }

        protected void lvPracownicy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

        public Button CloseButton
        {
            get { return btZoomClose; }
        }
    }
}