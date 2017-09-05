using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Kwitek
{
    public partial class WyplataDetale3 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GetSql3()
        {
            return @"
select top 1 
LpLogo, Imie, Nazwisko, Pesel, 
--Rok, Miesiac, 
RokK as Rok, MiesiacK as Miesiac, 
LpNrListy, KategoriaListy,
Data, Zamknieta
from lp_vv_NaliczeniaNagExtSkl where
NrListy = @lista and LpLogo = @logo
order by Data desc";
        }

        /*
        private string GetSql2()
        {   //-- SZABLON = np. 'LISTA_PLAC'
            return @"
select  Nazwa as [Nazwa składnika], Wartosc as [Wartość] from  lp_fn_przegladarka_DanePlacowe_allSzablony('20130101','LISTA_PLAC') 
WHERE NrListy = @lista and LpLogo = @logo 
order by kolejnosc";
        }
        */

        private string GetSql2()
        {   //-- SZABLON = np. 'LISTA_PLAC'
            return @"
select 
case 
	when A.Kod = 'ASZCZEPIONKA' then 'Badania' 
	when A.Kod = 'WYPLATA' then 'Do wypłaty'
	--else S.Nazwa 
else REPLACE(REPLACE(S.Nazwa, 
	'Wynagrodzenie w naturze', 'świadczenie rzeczowe'),
	'Natura', 'świadczenie rzeczowe')
end as [Nazwa składnika],
A.Wartosc as [Wartość]
--,A.Nazwa 
from  lp_fn_przegladarka_DanePlacowe_allSzablony('20130101','LISTA_PLAC') A
left outer join lp_skladnikiDefinicje S on S.Kod = A.Kod
WHERE A.NrListy = @lista and A.LpLogo = @logo 
and A.Kod not in (
'ANAGRODARZECZ',
'ANAGRODARZECZ',
'ANAGRZECZ_NARZ',
'ANAGRZECZ_NARZ',
'APREMIA',
'APREMIA',
'PNAGRODARZECZ')
and A.Nazwa not in (
'Brutto',
'Suma Prac.',
'Skł.emer.Z',
'Skł.rent..Z',
'Skł.wyp..Z',
'Skł.FP',
'Skł.FGSP',
'Suma Zakład',
'Netto',
'ROR1',
'Pakiet Benefit',
'Zwrot ZUS',
'Premia')
order by A.kolejnosc";
        }

        private void PrepareSql2()
        {
            const string pat = "from Listy where";
            string sql = SqlDataSource2.SelectCommand;
            if (sql.Contains(pat))
            {
                SqlDataSource2.SelectCommand = sql.Replace(pat, @"
from lp_fn_przegladarka_DanePlacowe_allSzablony('20130101','LISTA_PLAC') A 
left outer join lp_skladnikiDefinicje S on S.Kod = A.Kod 
WHERE A.NrListy = @lista and A.LpLogo = @logo 
and ");
            }
        }

        public void _Prepare(string listaid)   //, string logo, string dataszabl)
        {
            App.KwitekListaId = listaid;

            PrepareSql2();
            //SqlDataSource2.SelectCommand = GetSql2();
            if (SqlDataSource2.SelectParameters.Count > 0)
                SqlDataSource2.SelectParameters.Clear();
            SqlDataSource2.SelectParameters.Add("logo", TypeCode.String, App.KwitekKadryId);
            SqlDataSource2.SelectParameters.Add("lista", TypeCode.String, listaid);

            SqlDataSource3.SelectCommand = GetSql3();
            if (SqlDataSource3.SelectParameters.Count > 0)
                SqlDataSource3.SelectParameters.Clear();
            SqlDataSource3.SelectParameters.Add("logo", TypeCode.String, App.KwitekKadryId);
            SqlDataSource3.SelectParameters.Add("lista", TypeCode.String, listaid);

            dlHeader.DataBind();
            GridView1.DataBind();
        }

        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {
            //PrepareView(DetailsView1.Rows);
        }

        protected void DetailsView1_ItemCreated(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            // e.
        }
        //------------------
        /*
        public string KadryId
        {
            get { return hidKadryId.Value; }
            set { hidKadryId.Value = value; }
        }

        public string ListaId
        {
            get { return hidListaId.Value; }
            set { hidListaId.Value = value; }
        }

        public string DataSzabl
        {
            get { return hidDataSzabl.Value; }
            set { hidDataSzabl.Value = value; }
        }
        */



        public string[] ColTypes = null;

        public void PrepareHeader(TableCellCollection tcc)
        {
            ColTypes = gvHeader.Value.Split('|');
            int cnt = ColTypes.Count();
            if (tcc.Count < cnt) cnt = tcc.Count;
            //for (int i = cnt-1; i >= 0; i--)
            for (int i = 0; i < cnt; i++)
            {
                TableCell tc = tcc[i];
                string css = ColTypes[i];
                switch (css)
                {
                    case "-":
                        //tc.Visible = false;
                        //tcc.RemoveAt(i);
                        tc.CssClass = "hidden";
                        break;
                    default:
                        tc.CssClass = css;
                        break;
                }
            }
        }

        //public void PrepareCells(TableCellCollection tcc)
        public void PrepareCells(GridViewRow row)
        {
            int cnt = ColTypes.Count();
            if (row.Cells.Count < cnt) cnt = row.Cells.Count;
            for (int i = 0; i < cnt; i++)
            {
                string css = ColTypes[i];
                if (!String.IsNullOrEmpty(css))
                {
                    TableCell tc = row.Cells[i];
                    tc.CssClass = css;
                    switch (css)
                    {
                        case "-":
                            tc.CssClass = "hidden";
                            break;
                        case "D":
                        case "Db":
                            if (tc.Text.Length > 10)
                                tc.Text = tc.Text.Substring(0, 10);
                            break;
                        case "N2":
                        case "N2b":
                            double d;
                            if (Double.TryParse(tc.Text, out d))
                                tc.Text = d.ToString("0.00");
                            break;
                    }
                }
            }

            if (rcnt % 2 == 0)
                row.CssClass = "alt";

            switch (row.Cells[0].Text)
            {
                case "Brutto":
                case "Suma Prac.":
                case "Skł.emer.Z":
                case "Skł.rent..Z":
                case "Skł.wyp..Z":
                case "Skł.FP":
                case "Skł.FGSP":
                case "Suma Zakład":
                case "Netto":
                case "ROR1":
                case "Pakiet Benefit":
                case "Zwrot ZUS":
                case "Premia":
                    row.Visible = false;
                    break;
                case "Do wypłaty":
                    if (rcnt % 2 == 0)
                        row.CssClass = "alt b";
                    else
                        row.CssClass = "b";
                    break;
            }
            if (row.Visible) rcnt++;
        }

        private int rcnt = 0;

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.EmptyDataRow:
                    break;
                case DataControlRowType.Header:
                    /*
                    e.Row.Cells[1].Visible = false;
                    object o = e.Row.Cells[0];
                    e.Row.Cells.RemoveAt(0);
                    e.Row.Cells.Add((TableCell)o);
                    */
                    PrepareHeader(e.Row.Cells);
                    //PrepareHeader(gvWyplaty.HeaderRow.Cells);
                    break;
                case DataControlRowType.DataRow:
                    /*
                    e.Row.Cells[1].Visible = false;
                    o = e.Row.Cells[0];
                    e.Row.Cells.RemoveAt(0);
                    e.Row.Cells.Add((TableCell)o);
                    */
                    PrepareCells(e.Row);
                    break;
                case DataControlRowType.Footer:
                    break;
                case DataControlRowType.Separator:
                    break;
                case DataControlRowType.Pager:
                    break;
            }
        }
    }
}