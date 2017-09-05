using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Kwitek
{
    public partial class Wyplaty : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
        int FMode = 0;       // 0 - pracownik, 1 - admin form


        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = GetSql();
            if (!IsPostBack)
            {
            }
        }
        //---------------
        private string GetSql()
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            string dataDo = Tools.DateToStrDb(settings.KwitekDo);
            bool adm = FMode == 1 && App.User.HasRight(AppUser.rKwitekAdm);
            string admWhere = adm ? null : String.Format("and s.Status = 'Z' and s.Data <= '{0}'", dataDo);
            //string admSelect = adm ? String.Format(",case when s.Data <= '{0}' then 'widoczna' else 'niewidoczna' end as Widoczna", dataDo) : null;
            //string admSelect = adm ? String.Format(",case when s.Data <= '{0}' then 'TAK' else 'NIE' end as Widoczna", dataDo) : null;
            string admSelect = adm ? String.Format(",s.Status,convert(bit, case when s.Status = 'Z' and s.Data <= '{0}' then 1 else 0 end) as Widoczna", dataDo) : null;
            return String.Format(@"
select 
s.LpLogo,
s.NrListy,
--s.LpNrListy,
--convert(varchar, s.Rok) + '-' + RIGHT('00' + convert(varchar, s.Miesiac), 2) as Miesiąc, 
convert(varchar, s.RokK) + '-' + RIGHT('00' + convert(varchar, s.MiesiacK), 2) as Miesiąc, 
s.LpNrListy + ', ' + KategoriaListy as Lista,
--dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'AANGAZ') AS [AANGAZ],
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ABRUTTO_B') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'AANGAZ_NOM') AS [Umowa],
'' as [Do wypłaty:],
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR1_B') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR1') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR2') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR3') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR4') AS [ROR],
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'KASA') AS [Kasa],
s.Data as [Data wypłaty] 
{2}
FROM dbo.[lp_vv_NaliczeniaNagExt] as s WITH (NOLOCK)

where s.Rok >= 2013
and
(
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR1_B') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR1') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR2') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR3') +
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'ROR4') > 0 or
dbo.lp_fn_SkladnikiWartoscKodWNaliczeniu (s.lp_NaliczeniaNagId, 'KASA') > 0
)
and s.ListaTechniczna = 0 and ListaSymulacja = 0
and s.LpLogo = '{0}'
{1}
--order by s.Rok, s.Miesiac
order by s.RokK, s.MiesiacK
", App.KwitekKadryId, admWhere, admSelect);
        }

        //---------------
        protected void gvWyplaty_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                case "Details":
                    //Tools.ShowMessage(gvWyplaty.SelectedValue.ToString());
                    //Tools.ShowMessage(gvWyplaty.SelectedRow.Cells[2].Text)

                    int idx = Convert.ToInt32(e.CommandArgument);
                    //Tools.ShowMessage(gvWyplaty.DataKeys[idx].Value.ToString());

                    //HttpContext.Current.Response.Redirect(App.KwitekDetaleForm + "?p=" + gvWyplaty.DataKeys[idx].Value.ToString());
                    //Tools.Redirect(App.KwitekDetaleForm, gvWyplaty.DataKeys[idx].Value.ToString(), null, null, null, null);
                    
                    //KwitekDetale.Show(gvWyplaty.DataKeys[idx].Value.ToString());
                    SelectedListaId = gvWyplaty.DataKeys[idx].Value.ToString();
                    
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                    break;
            }
        }

        private void TriggerSelectedChanged()
        {
            
            if (gvWyplaty.SelectedDataKey != null)      // jak ustawiam w PageLoad w Kwitek.aspx to tego jeszcze niema
                SelectedListaId = gvWyplaty.SelectedDataKey.Value.ToString();
            else
                SelectedListaId = null;
            /*
            int idx = gvWyplaty.SelectedIndex;
            if (idx != -1)      // jak ustawiam w PageLoad w Kwitek.aspx to tego jeszcze niema
                SelectedListaId = gvWyplaty.DataKeys[idx].Value.ToString();
            else
                SelectedListaId = null;
            */
            
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }

        protected void gvWyplaty_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerSelectedChanged();
        }

        protected void gvWyplaty_Sorted(object sender, EventArgs e)
        {

        }
        //------------------
        public string[] ColTypes = null;

        public void PrepareHeader_x(TableCellCollection tcc)
        {
            ColTypes = new string[tcc.Count];
            for (int i = 0; i < tcc.Count; i++)
            {
                TableCell tc = tcc[i];
                DataControlFieldHeaderCell hc = (DataControlFieldHeaderCell)tcc[i];
                //string t = tc.Text;
                string t = hc.ContainingField.HeaderText;
                int p = t.IndexOf('|');
                string css = null;
                if (p >= 0)
                {
                    //tc.Text = t.Substring(0, p);
                    //hc.ContainingField.HeaderText = t.Substring(0, p);

                    css = t.Substring(p + 1);
                    tc.CssClass = css;
                    hc.ContainingField.HeaderText = t.Substring(0, p);
                    hc.ContainingField.SortExpression = t;
                }
                ColTypes[i] = css;
            }
        }

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

        protected void gvWyplaty_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    PrepareCells(e.Row.Cells);
                    break;
                case DataControlRowType.Footer:
                    break;
                case DataControlRowType.Separator:
                    break;
                case DataControlRowType.Pager:
                    break;
            }
        }

        public static void gvMoveColumns(GridView gv)
        {
            List<DataControlField> cols = new List<DataControlField>();
            foreach (DataControlField col in gv.Columns)
                cols.Add(col);
            //----- reorganize -----
            if (cols.Count > 0)
            {
                object o = cols[0];
                cols.RemoveAt(0);
                cols.Add((DataControlField)o);
                //----------------------
                gv.Columns.Clear();
                foreach (DataControlField col in cols)
                    gv.Columns.Add(col);
            }
        }

        bool gotoLast = true;
        bool trigSelect = false;

        protected void gvWyplaty_PageIndexChanging(object sender, EventArgs e)
        {
            gotoLast = false;
        }

        protected void gvWyplaty_PageIndexChanged(object sender, EventArgs e)
        {
            trigSelect = true;
        }

        protected void gvWyplaty_DataBound(object sender, EventArgs e)
        {
            if (gotoLast)
            {
                if (gvWyplaty.PageCount > 0)
                    gvWyplaty.PageIndex = gvWyplaty.PageCount - 1;
                if (gvWyplaty.Rows.Count > 0)
                    gvWyplaty.SelectedIndex = gvWyplaty.Rows.Count - 1;
            }
            else
                if ((gvWyplaty.SelectedIndex == -1 || gvWyplaty.SelectedIndex >= gvWyplaty.Rows.Count) && gvWyplaty.Rows.Count > 0)
                    gvWyplaty.SelectedIndex = gvWyplaty.Rows.Count - 1;    // na ostatni, tak sie dzieje jak np mam zaznaczony ostatni i przechodzę na ostatni page gdzie jest kilka rekordów mniej - domyslnie nic nie jest zaznaczone
            if (trigSelect)
                TriggerSelectedChanged();
        }
        //------------------------------
        public void Prepare()
        {
            SqlDataSource1.SelectCommand = GetSql();
            gvWyplaty.PageIndex = Int32.MaxValue;
            gvWyplaty.DataBind();
            gvWyplaty.SelectedIndex = gvWyplaty.Rows.Count - 1;
            TriggerSelectedChanged();
        }
        /*
        public string _KadryId
        {
            get { return App.KwitekKadryId; }
            set
            {
                App.KwitekKadryId = value;
                SqlDataSource1.SelectCommand = GetSql();
                gvWyplaty.PageIndex = Int32.MaxValue;
                gvWyplaty.DataBind();
                gvWyplaty.SelectedIndex = gvWyplaty.Rows.Count - 1;
                TriggerSelectedChanged();
            }
        }
        */
        public string SelectedListaId
        {
            get { return App.KwitekListaId; }
            set { App.KwitekListaId = value; }
        }

        public GridView Grid
        {
            get { return gvWyplaty; }
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

    }
}


/*
select 
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
from lp_vv_NaliczeniaNagExtSkl where 
 * 
 * 
 * 
 * 
 * 
 * --select * from lp_vv_NaliczeniaNagExtSkl where LpLogo = '01320'
--select * from lp_vv_NaliczeniaNagExtSkl where KASA <> 0

select distinct LpLogo + ' ' + Nazwisko + ' ' + Imie as Nazwisko, LpLogo as KadryId from lp_vv_NaliczeniaNagExtSkl 


select 
LpLogo as [Nr ewidencyjny], 
convert(varchar, Rok) + '-' + RIGHT('00' + convert(varchar, Miesiac), 2) as Miesiąc, --RokK, miesiacK, 
LPNrListy + ', ' + KategoriaListy as Lista,
PRZYCHOD,
ZALNAPOD,
ZALNAPOD_N,

URLOP,
ULGAPODATK,

ROR1,

Imie, Imie2, Nazwisko, Pesel, 
Data, DataKosztowa
from lp_vv_NaliczeniaNagExtSkl where 
ListaTechniczna = 0 and ListaSymulacja = 0

and LpLogo = '01320'
order by Rok, Miesiac

/*
różnica między RokK, MiesiacK, a Rok i Miesiac 
UlgaPodatkowa = 1
KosztyUzPrz = 2 ???


*/
