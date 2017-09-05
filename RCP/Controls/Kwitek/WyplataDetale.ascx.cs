using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Kwitek
{
    public partial class WyplataDetale : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GetSql1()
        {
            return String.Format("select 'Wartość' as [Nazwa składnika], * from lp_vv_NaliczeniaNagExtSkl where __InternalId__ = {0}", App.KwitekListaId);
        }

        private string GetSql2()
        {       //-- SZABLON = np. 'LISTA_PLAC'
            return String.Format(@"
select * from  lp_fn_przegladarka_DanePlacowe_allSzablony('20130101','LISTA_PLAC') 
where lpnrlisty = {0} and LpLogo = {1}
order by kolejnosc", App.KwitekListaId, App.KwitekKadryId);
        }

        public void Prepare(string lista, string logo, string dataszabl)
        {
            /*
            ListaId = lista;
            KadryId = logo;
            DataSzabl = dataszabl;
             */
            SqlDataSource1.SelectCommand = GetSql1();
            //DataList1.DataBind();
            dlHeader.DataBind();
            DetailsView1.DataBind();
        }

        //----------------------------------
        /*
AANGAZ,
AANGAZ_K,
AANGAZ_KZ,
AKANGAZ,
ASTALAPREM,
ASTALAPREM_K,
ASTALAPREM_KZ,
ASTALYDODF,
ASTALYDODF_K,
ASTALYDODF_KZ,
AWYP,
AZUSNPODN,
AZUSNPODT,
AZUSNPODTZDROT,
AZUSTPODN,
DOCHOD,
DOCHOD_N,
DODRODZKSZREH,
DODRODZPODNAUK,
DODRODZROZROK,
DODRODZURODZ,
DODRODZWYCHWIEL,
EKWIWURL_D,
EKWIWURL_D_D,
EKWIWURL_D_W,
EKWIWURL_D_Z_D,
EKWIWURL_D_Z_W,
EKWIWURL_G_D,
EKWIWURL_G_W,
EKWIWURL_G_Z_D,
EKWIWURL_G_Z_W,
EKWIWURL_W,
KASA,
KOSZTYINNE,
KOSZTYUP,
KOSZTYUP_N,
NATURBRUTTO,
NATURBRZUSNPODT,
NATURZUSNPODNZN,
NATURZUSTPODN,
NETTO,
PODATEK_N,
POTRACENIA,
PRZE,
PRZYCHOD,
PRZYCHOD_N,
ROR1,
ROR2,
ROR3,
ROR4,
SUMBRUTTO,
SUMSKLADKIPRA,
ULGAPODATK,
URLOP,
WYCH,
WYCHDNI,
WYPLATA,
ZACH,
ZACHDNI,
ZALNAPOD,
ZALNAPOD_N,
ZAMAC,
ZAMACDNI,
ZANIECHANIE,
ZANIECHANIE_N,
ZAOP,
ZAOPDNI,
ZAPIEL,
ZAPIELDNI,
ZARECH,
ZARECHDNI,
ZARODZ,
ZARODZDNI,
ZAWYCH,
ZAWYR,
ZAWYRDNI,
ZPODCHOROB,
ZPODCHOROB_N,
ZPODEMPL,
ZPODEMPL_N,
ZPODEMPLBP,
ZPODEMPR,
ZPODEMPR_N,
ZPODFGSP,
ZPODFGSP_N,
ZPODFP,
ZPODFP_N,
ZPODREPL,
ZPODREPL_N,
ZPODREPLBP,
ZPODREPR,
ZPODREPR_N,
ZPODWYPAD,
ZPODWYPAD_N,
ZPODZDRO,
ZPODZDRO_N,
ZPODZDROBP,
ZSKLCHOROB,
ZSKLCHOROB_N,
ZSKLEBPPLA,
ZSKLEMEPLA,
ZSKLEMEPLA_N,
ZSKLEMEPRA,
ZSKLEMEPRA_N,
ZSKLFGSP,
ZSKLFGSP_N,
ZSKLFP,
ZSKLFP_N,
ZSKLRENPLA,
ZSKLRENPLA_N,
ZSKLRENPLABP,
ZSKLRENPRA,
ZSKLRENPRA_N,
ZSKLWYPADK,
ZSKLWYPADK_N,
ZSKLZDRO,
ZSKLZDRO_N,
ZSKLZDROBP,
ZSKLZDROPR,
ZSKLZDROPR_N,
ZSKLZDROPRBP,
ZSKLFEP,
ZSKLFEP_N,
ZSKLFEP_B,
PoprawneNaliczenie,
__InternalId__,
lp_NaliczeniaNagId,
LpLogo,
UmowaNumer,
Kolejnosc,
NumerRachunku,
NrListy,
Grupa,
Dzial,
OsobaTworzaca,
DataUtworzenia,
DataModyfikacji,
Opis,
Status,
Robotnik,
Stanowisko,
Deklaracja,
Wlasny,
KodUbezpieczenia,
NalLiczycAutomatycznie,
BlokadaNaliczenia,
ListaPlacTyp,
Rok,
Miesiac,
RokK,
MiesiacK,
Data,
ListaPlacRodzaj,
Platnik,
DataKosztowa,
LPNrListy,
ListaTechniczna,
ListaSymulacja,
__COLORBRUSH__,
Firma,
Imie,
Imie2,
Nazwisko,
Pesel,
NumerRachunkuNadany,
lp_KlasyfikacjaId,
DataOd,
DataDo,
DzialKadry,
StrukturaOrganizacyjna,
KategoriaListy,
Uczen,
Mlodociany,
Mianowany,
Student,
Cudzoziemiec,
CudzStaly,
CudzCzasowy,
SystemZmianowy,
Interwencja,
Nauczyciel,
Emeryt,
Rencista,
UrlopNaGodziny,
PierwszaPraca,
SamotnaMatka,
JedyneZrodloDochodu,
PozostajeWeWspolnymGosZPr,
TypAdresu,
UlgaPodatkowa,
KosztyUzPrzy,
Szablon,
PracaNakladcza,
Agent,
CzlonekSpoldzielni,
Wspolwlasciciel,
Wlasciciel,
PomagajacyCzlonekRodziny,
PierwszaPracaUrlop,
PracaZaGranica,
DzialWymiar1,
DzialWymiar2,
DzialWymiar3,
DzialWymiar4,
Wl_Naliczanie_ZUS,
Wl_SklZdro_Ilosc,
NiePokazujNaZest,
PracaChalupnicza,
ZwiekszonyWymiar,
NrEmerytury,
NrRenty,
StatusOsoby,
LPDzial,
LPGrupa,
MultiTasks,
CanEdit,
CanDelete,
Zamknieta,
IloscLogBlad,
CBN_IloscLogBlad,
IloscLogBladKryt,
WyeksportowaneDoBanku,
WyeksportowaneDoBankuIco,
BlokadaNaliczeniaIco,
HINT_IloscLogBlad,
BezPrzeliczenia,
StartAfterDate,
GroupDescription,
NaDzien
         */

        public void PrepareView(DetailsViewRowCollection rows)
        {
            //string fields = "," + NumFields.Value + ","; 
            //string[] Fields = NumFields.Value.Split(',');
            double sum = 0;   
            int cnt = rows.Count;
            int firstField = Tools.StrToInt(FirstField.Value, 0);
            int lastField = Tools.StrToInt(LastField.Value, cnt - 1);
            
            int alt = 0;
            for (int i = 0; i < cnt; i++)
            {
                DetailsViewRow row = rows[i];
                TableCell c1 = row.Cells[0];
                TableCell c2 = row.Cells[1];
                c1.CssClass = "col1";
                c2.CssClass = "col2";

                if (i == 0)  //header
                {
                }
                else if (i >= firstField && i <= lastField)
                {
                    double d;
                    if (Double.TryParse(c2.Text, out d))
                        if (d == 0)
                            row.Visible = false;
                        else
                        {
                            c2.Text = d.ToString("0.00");
                            sum += d;
                        }
                }
                else
                {
                    row.Visible = false;
                }
                    /*
                else 
                {
                    case "D":
                    case "Db":
                        if (c2.Text.Length > 10)
                            c2.Text = c2.Text.Substring(0, 10);
                        break;
                }
                     */

                if (row.Visible)
                {
                    if (i == 0) row.CssClass = "header";
                    else 
                        if ((alt & 1) == 0) row.CssClass = "alt";
                    alt++;
                }
            }
            if (cnt > 0)
            {
                DetailsViewRow row = rows[cnt - 1];
                row.Visible = true;
                row.Cells[1].Text = sum.ToString("0.00");
                row.CssClass = "sum";
            }
        }

        /*

        public void PrepareHeader(TableCellCollection tcc)
        {
            ColTypes = gvWyplatyHeader.Value.Split('|');
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

        public void PrepareCells(TableCellCollection tcc)
        {
            int cnt = ColTypes.Count();
            if (tcc.Count < cnt) cnt = tcc.Count;
            for (int i = 0; i < cnt; i++)
            {
                string css = ColTypes[i];
                if (!String.IsNullOrEmpty(css))
                {
                    TableCell tc = tcc[i];
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
        }
        */

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

        protected void SqlDataSource2_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            // e.
        }
        //------------------
        /*
        public string KadryId
        {
            get { return hidKadryId.Value; }
            set
            {
                hidKadryId.Value = value;
                //gvWyplaty.PageIndex = Int32.MaxValue;
                //gvWyplaty.DataBind();
            }
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
    }
}