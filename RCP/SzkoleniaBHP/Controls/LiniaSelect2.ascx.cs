using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.SzkoleniaBHP.Controls
{
    public partial class LiniaSelect2 : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;

        const int moMatryca = 0;
        const int moUprawnienia = 1;
        int FMode = moMatryca;

        private const int stAll = 255;
        private const int stProd = 1;   // zgodne z cntMatryca2.
        private const int stNieprod = 2;
        private const int stDod = 4;


        public const string ALL = "all";
        public const string DELEG = "deleg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //_KierId = App.User.Id;
                L.p(lbKierCaption);
                //L.p(lbCaption);
                //L.p(lbCaption1);
                L.p(lbZakres);
                L.p(cbSubStr);
            }
        }
        //--------------------------------------
        public int _Prepare(string kierId, bool showAll, bool admin, string data)  // dla kogo przygotować, czy dodać opcję "Wszystkie (moje) linie", czy z panelu admina  //na dzień
        {
            return 0;
            /*RCP
            int cnt;
            if (String.IsNullOrEmpty(data)) data = Tools.DateToStrDb(DateTime.Today);
            Data = data;
            if (admin)
            {
                paKier.Visible = true;

                DataSet dsP = App.GetKierListAll(null, false, true);    // tylko przypięci do struktury !!! bo się wywala cntMatryca
                Tools.BindData(ddlPrzelozony, dsP, "Kierownik", "Id", false, kierId);   // zaznaczy mnie jezeli jestem na liście kier
                ddlPrzelozony.Items.Add(new ListItem(L.p("Wszyscy przełożeni"), ALL));

                if (ddlPrzelozony.SelectedValue != kierId)                              // jednak nie jestem przełożonym                
                    Tools.SelectItem(ddlPrzelozony, ALL);

                cnt = PrepareForKier(ddlPrzelozony.SelectedValue, showAll, data);

                if (admin && cnt == 0)   // jestem adminem, ale nie pomijanym czyli istnieję w ddlPrzelozeni i nie jestem przypięty do strorg -> cnrMatryca zwraca błąd bo @strorg = '' -> jeszcze raz zainicjować jak dla admina <<<< na razie zmieniam ddlPrzelozeni - tylko przypieci do struktury
                {

                }
            }
            else
            {
                cnt = PrepareForKier(kierId, showAll, data);
                if (cnt > 1)
                {    // kierownicy ze wszystkich podstruktur
                    DataSet dsP = db.getDataSet(String.Format(@"
declare @data datetime
declare @kierId int 
set @data = GETDATE()
set @kierId = {0}

select P.Id_Przelozeni as Id, P.Nazwisko + ' ' + P.Imie as Kierownik, 1 as Sort 
from Przelozeni P where P.Id_Przelozeni = @kierId
union
select distinct P.Id_Przelozeni as Id, P.Nazwisko + ' ' + P.Imie as Kierownik, 2 as Sort 
from StrukturaPrzelozeni SP
outer apply (select * from dbo.fn_GetStrOrgTree(SP.IdStruktury, 0, @data)) T
left join StrukturaPrzelozeni SP2 on SP2.IdStruktury = T.Id
left join Przelozeni P on P.Id_Przelozeni = SP2.IdPrzelozonego
where SP.IdPrzelozonego = @kierId
and P.Id_Przelozeni is not null
order by Sort, Kierownik
                    ", kierId));
                    if (db.getCount(dsP) > 1)   // bo i tak 1 jest
                    {
                        paKier.Visible = true;

                        Tools.BindData(ddlPrzelozony, dsP, "Kierownik", "Id", false, kierId);   // zaznaczy mnie jezeli jestem na liście kier
                        //ddlPrzelozony.Items.Add(new ListItem(L.p("Wszyscy przełożeni"), ALL));  // daje dostęp do wszystkich pracoeników

                        if (ddlPrzelozony.SelectedValue != kierId)                              // jednak nie jestem przełożonym                
                            Tools.SelectItem(ddlPrzelozony, ALL);
                    }
                }
            }
            FillZakres(ddlZakres, 0);
            return cnt;
             */ 
        }

        public int PreparePracView(string data)  // dla kogo przygotować, czy dodać opcję "Wszystkie (moje) linie", czy z panelu admina  //na dzień
        {
            return 0;
            /*RCP
            int cnt;
            if (String.IsNullOrEmpty(data)) data = Tools.DateToStrDb(DateTime.Today);
            Data = data;

            DataSet dsL = db.getDataSet(String.Format(@"
declare @pracId int = {0}
declare @strM int = {1}
declare @lng varchar(10) = '{2}'

select 
--S.Symb_Jedn + ISNULL(' - ' + case when @lng = 'PL' then S.Nazwa_Jedn else S.Nazwa_JednEN end, '') +
dbo.same(S.Symb_Jedn, case when @lng = 'PL' then S.Nazwa_Jedn else S.Nazwa_JednEN end, 1) +
ISNULL(' (' +
case when S.Id_Str_Org = @strM then '{3}' 
else '{4}' + ' ' + CONVERT(varchar(10), D.Od, 20) + '...' + ISNULL(CONVERT(varchar(10), D.Do, 20), '')
end + ')', '')
as JednOrg,
S.Id_Str_Org as Id,
S.Typ, 
case when S.Id_Str_Org = @strM then 1 else 2 end as Sort
from 
(
select * from StrOrg where Id_Str_Org = @strM
union
select distinct * from StrOrg where Id_Str_Org in (select IdStruktury from Oddelegowania where IdPracownika = @pracId)
) S
outer apply (select top 1 * from Oddelegowania where IdPracownika = @pracId and IdStruktury = S.Id_Str_Org and Status = 2 order by Od desc) D 
order by Sort, D.Od desc
                ",
                 App.User.Id, db.nullParam(App.User.Data["Id_Str_Org"].ToString()),
                 L.Lang,
                 L.p("jednostka macierzysta"),
                 L.p("oddelegowanie:")
                 ));
            cnt = db.getCount(dsL);

            bool l1 = false;
            if (cnt <= 1) l1 = true;   // brak przypisań

            lbCaption.Visible = !l1;
            ddlLinie.Visible = !l1;
            paSubStr.Visible = false;
            lbCaption1.Visible = l1;
            lbLinia1.Visible = l1;

            lbZakres.Visible = false;
            ddlZakres.Visible = false;

            if (l1)
            {
                if (cnt == 0)
                {
                    lbLinia1.Text = L.p("brak przypisań");
                }
                else
                {
                    DataRow dr = db.getRows(dsL)[0];
                    string id = db.getValue(dr, "Id");
                    string nazwa = db.getValue(dr, "JednOrg");
                    lbLinia1.Text = nazwa;
                    lbLinia1.ToolTip = GetUpTreeStr(id, data);
                    StrOrgListAll = id;
                    StrTypS = db.getInt(dr, "Typ", stAll) | stDod;
                }
            }
            else
            {
                Tools.BindData(ddlLinie, dsL, "JednOrg", "Id", false, null);
                ddlLinie.SelectedIndex = 0;
                SetddlLiniaHint();
                StrTypS = GetStrTypS() | stDod;
            }
            FillZakres(ddlZakres, 0);
            StrTypP = 0;
            return cnt;
            */ 
        }


        /*
        private string GetUpTreeStr(string strId, string data)
        {
            const int colJedn = 0;
            const string sqlUpTree = @"
select 
replicate('&nbsp;', Hlevel * 4) +
Symbol + ' - ' + {2} as JednOrg, Id 
from dbo.fn_GetStrOrgTreeUp({0}, 1, '{1}')";
            DataSet dsUp = db.getDataSet(String.Format(sqlUpTree, strId, data, L.Lang == L.lngPL ? "Nazwa" : "NazwaEN"));
            Tools.PrepareHTML(dsUp, colJedn);
            return db.Join(dsUp, colJedn, "\n");
        }
         */ 
        //--------------------------------------
        private int PrepareForKier(string kierId, bool showAll, string data)     // id kier lub "all" - wszyscy
        {
            return 0;
            /*RCP

            const string _sqlSubTree = @"
select 
--SPACE(Hlevel*4) + 
--replicate('&nbsp;|', Hlevel) + '-' +     
replicate('&nbsp;', Hlevel * 4) +
dbo.same(Symbol,{2},1) as JednOrg, Id 
from dbo.fn_GetStrOrgTree({0}, 1, '{1}')";
            
            KierId = kierId;
            //StrOrgListAll = null;
            DataSet dsL = null;
            if (!String.IsNullOrEmpty(kierId))
            {
                bool l1 = false;
                int cntJedn = 1;           // ilosc jednostek do których jest przypięty, ilość root'ów
                if (kierId == ALL)
                {
                    dsL = db.getDataSet(String.Format(_sqlSubTree, "0", data, L.Lang == L.lngPL ? "Nazwa" : "NazwaEN"));
                    StrTypP = stAll;
                }
                else
                {
                    DataSet ds = db.getDataSet(String.Format(@"
select SP.*, S.Typ 
from StrukturaPrzelozeni SP 
left join StrOrg S on S.Id_Str_Org = SP.IdStruktury 
where IdPrzelozonego = {0}
order by S.Symb_Jedn", kierId));  // and data ...
                    cntJedn = db.getCount(ds);

                    int styp = stDod;   // wszyscy mają dodatkowe
                    foreach (DataRow dr in db.getRows(ds))
                    {
                        string sid = db.getValue(dr, "IdStruktury");
                        styp |= db.getInt(dr, "Typ", 0);
                        //if (dsL == null || dsL.Tables[0].Select("Id=" + sid).Length == 0)  // start lub nie istnieje <<< wyłączam niech da wszystko od root, lub zmienić algorytm: wszystkie rooty, zbudować podstrukturę, od dołu zbudować drzewko, duzo roboty
                        {
                            if (dsL == null) dsL = db.getDataSet(String.Format(_sqlSubTree, sid, data, L.Lang == L.lngPL ? "Nazwa" : "NazwaEN"));
                            else
                            {
                                DataSet dsA = db.getDataSet(String.Format(_sqlSubTree, sid, data, L.Lang == L.lngPL ? "Nazwa" : "NazwaEN"));
                                dsL.Merge(dsA);
                            }
                            l1 = cntJedn == 1 && db.getCount(dsL) == 1;  // jeden element struktury i 
                        }
                    }
                    if (styp == 0)
                        StrTypP = stAll;
                    else
                        StrTypP = styp;
                }
                if (dsL == null) l1 = true;   // brak przypisań

                lbCaption.Visible = !l1;
                ddlLinie.Visible = !l1;
                paSubStr.Visible = !l1;
                lbCaption1.Visible = l1;
                lbLinia1.Visible = l1;
                if (l1)
                {
                    if (dsL == null)
                    {
                        lbLinia1.Text = L.p("brak przypisań");
                    }
                    else
                    {
                        DataRow dr = db.getRows(dsL)[0];
                        lbLinia1.Text = db.getValue(dr, 0);
                        lbLinia1.ToolTip = GetUpTreeStr(db.getValue(dr, 1), data);
                        //lbLinia1.ToolTip = db.getValue(dr, "StrumienNazwa") + " / " + db.getValue(dr, "LiniaNazwa");
                        StrOrgListAll = db.getValue(dr, "Id");
                    }
                    StrTypS = 0; // z pracownika weźmie
                }
                else
                {
                    StrOrgListAll = db.Join(dsL, "Id", ",");
                    Tools.PrepareHTML(dsL, 0);
                    Tools.BindData(ddlLinie, dsL, "JednOrg", "Id", false, null);
                    if (cntJedn > 1)
                        //ddlLinie.Items.Insert(0, new ListItem(L.p("Cała struktura"), ALL));
                        ddlLinie.Items.Insert(0, new ListItem(L.p("Wszystkie jednostki"), ALL));

                    ddlLinie.SelectedIndex = 0;
                    SetddlLiniaHint();

                    //if (showAll)// && kierId == ALL)                                           
                    //    Tools.SelectItem(ddlLinie, ALL);                                    // na starcie wszyscy


                    //StrTyp = stAll;
                    StrTypS = GetStrTypS();
                }
            }
            else StrTypP = stAll;   // kierId == null
            return dsL == null ? 0 : db.getCount(dsL);
             */ 
        }

        private void FillZakres(DropDownList ddl, int selected)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(L.p("Zatrudnieni"),               "p"));
            ddl.Items.Add(new ListItem(L.p("Przydelegowani"),            "d")); 
            ddl.Items.Add(new ListItem(L.p("Zwolnieni"),                 "z"));
            ddl.Items.Add(new ListItem(L.p("Wszyscy"),                   "all"));
/*
            ddl.Items.Add(new ListItem("Zatrudnieni",               "p"));
            ddl.Items.Add(new ListItem("Zatrudnieni - pracownicy",  "pp"));
            ddl.Items.Add(new ListItem("Zatrudnieni - APT",         "pa"));
            ddl.Items.Add(new ListItem("Przydelegowani",            "d"));
            //ddl.Items.Add(new ListItem("Pracujący",                 "p"));
            //ddl.Items.Add(new ListItem("Pracujący - pracownicy",    "pp"));
            //ddl.Items.Add(new ListItem("Pracujący - APT",           "pa"));
            ddl.Items.Add(new ListItem("Zwolnieni",                 "z"));
            ddl.Items.Add(new ListItem("Zwolnieni - pracownicy",    "zp"));
            ddl.Items.Add(new ListItem("Zwolnieni - APT",           "za"));
            ddl.Items.Add(new ListItem("Wszyscy",                   "all"));
            ddl.Items.Add(new ListItem("Archiwum",                  "arch"));
             */
            ddl.SelectedIndex = selected;
        }

        //----------------------------------------
        protected void ddlPrzelozony_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareForKier(ddlPrzelozony.SelectedValue, true, Data);
            //StrOrgList3 = null;
            TriggerSelectedChanged();
        }

        protected void cbSubStr_CheckedChanged(object sender, EventArgs e)
        {
            //StrOrgList3 = null;
            TriggerSelectedChanged();
        }

        protected void ddlZakres_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerSelectedChanged();
        }

        private void TriggerSelectedChanged()
        {
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }
        //----------------------------------------
        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        public string KierId
        {
            set { ViewState["kierid"] = value; }
            get { return Tools.GetStr(ViewState["kierid"]); }
        }

        public string Data
        {
            set { ViewState["data"] = value; }
            get { return Tools.GetStr(ViewState["data"]); }
        }

        public string Zakres
        {
            get { return ddlZakres.SelectedValue; }
        }

        public int StrTypP
        {
            set { ViewState["strtypP"] = value; }
            get { return Tools.GetInt(ViewState["strtypP"], 255); }
        }

        public int StrTypS
        {
            set { ViewState["strtypS"] = value; }
            get { return Tools.GetInt(ViewState["strtypS"], 255); }
        }

        public int StrTyp
        {
            get { return StrTypP | StrTypS; }
        }

        //----- do Pracownicy3 ---------------------------------
        public string KierId3
        {
            get 
            {
                if (ddlPrzelozony.SelectedIndex != -1)//(paKier.Visible)  // ddlPrzelozony.Items.Count > 0    // is Admin
                    if (ddlPrzelozony.SelectedValue == ALL)
                        return null;
                    else
                        return ddlPrzelozony.SelectedValue;
                else
                    return KierId;
            }
        }

        public bool SubStr3
        {
            get { return cbSubStr.Checked; }
        }

    }
}