using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntPracInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Prepare(string pracId, string data)
        {
            int nom14min = 10; // tyle powinien mieć przysługującego urlopu, żeby wymusić 14 dni, jeżeli mniej to tylko ostrzegam, dodać do konfiguracji !!! - ustalilismy (Jarek,Asia) wartosc graniczna kiedy musi lub moze zaplanowac, jezeli mniej to warning ale moze zapisac 
            PracId = pracId;   // >>> zmienic na GetPracInfo1
            DataRow dr = Base.getDataRow(String.Format(@"select * from dbo.fn_GetPlanUrlopow({0},'{1}')", PracId, data));

            //DateTime? dt = Base.getDateTime(dr, "DataZatr");
            int? etatL = Base.getInt(dr, "EtatL");
            int? etatM = Base.getInt(dr, "EtatM");
            /*
            string etat = null;
            if (etatL != null && etatM != null)
            {
                if ((int)etatL > 0 && (int)etatM > 0)
                    Etat = 8 * (int)etatL / (int)etatM;
                else
                    Etat = 0;
                if (etatL == etatM) etat = "pełny";
                else etat = etatL.ToString() + "/" + etatM.ToString();
            }
            else Etat = 0;
            */
            //lbDataZatrudnienia.Text = dt == null ? null : Base.DateToStr(dt);
            string d1 = Tools.DateToStr(dr["DataZatr"]);
            string d2 = Tools.DateToStr(dr["UmowaDo"]);
            //string d2 = db.ISNULL(Tools.DateToStr(dr["UmowaDo"]), "bez terminu");
            lbOkres.Text = String.Format("{0} ... {1}", d1, String.IsNullOrEmpty(d2) ? "bez terminu" : d2);
            UmowaOd = String.IsNullOrEmpty(d1) ? data.Substring(0, 4) + "0101" : d1;
            UmowaDo = String.IsNullOrEmpty(d2) ? data.Substring(0, 4) + "1231" : d2;

            double nom = db.getDouble(dr, "UrlopNom", 0);
            double zal = db.getDouble(dr, "UrlopZaleg", 0);

            double dod = db.getDouble(dr, "UrlopDodatkowy", 0);
            bool vd = dod > 0;
            trDodatkowy.Visible = vd;
            if (vd) lbDodatkowy.Text = dod.ToString();

            DateTime? dataZwiekszenia = db.getDateTime(dr, "DataZwiekszenia");
            bool vz = dataZwiekszenia != null;
            trDataZwiekszenia.Visible = vz;
            if (vz) lbDataZwiekszenia.Text = Tools.DateToStr((DateTime)dataZwiekszenia);

            //lbWymiar.Text = db.getValue(dr, "UrlopNom");
            //lbZalegly.Text = db.getValue(dr, "UrlopZaleg");
            lbWymiar.Text = nom.ToString();
            lbZalegly.Text = zal.ToString();
            lbRazem.Text = (nom + zal + dod).ToString();

            lbVal4.Text = db.getValue(dr, "Zaplanowany");
            lbVal1.Text = db.getValue(dr, "Pozostalo");

            int nom14 = db.getInt(dr, "nom14", 0);
            int plan14 = db.getInt(dr, "plan14", 0);
            string nom14fmt;
            switch (nom14)
            {
                case 0:
                case 1:
                    nom14fmt = "14 kolejnych dni wolnych, zaplanowano:";
                    lb14.Enabled = false;
                    break;
                case 2:
                case 3:
                case 4:
                    nom14fmt = "{0} kolejne dni wolne, zaplanowano:";
                    break;
                default:
                    nom14fmt = "{0} kolejnych dni wolnych, zaplanowano:";
                    break;
            }
            lb14.Text = String.Format(nom14fmt, nom14);
            //hidNom14.Value = nom14.ToString();
            if ((nom + zal) >= nom14min)    // 10 - ustalilismy (Jarek,Asia) wartosc graniczna kiedy musi lub moze zaplanowac, jezeli mniej to warning ale moze zapisac 
                hidNom14.Value = nom14.ToString();
            else
                hidNom14.Value = "-" + nom14.ToString();
            
            lbVal2.Text = plan14.ToString();

            /* 20140706 wyłączam
            int _nom812 = db.getInt(dr, "nom812", 0);
            int nom812zal = _nom812 + Convert.ToInt32(zal);
            int plan812 = db.getInt(dr, "plan812", 0);
//            lb812.Text = String.Format("8/12 wymiaru + zaległy ({0}+{1} {2}) do końca sierpnia:", nom812zal, nom812zal == 1 ? "dzień" : "dni");
            lb812.Text = String.Format("8/12 wymiaru + zaległy ({0}+{1}) do końca sierpnia:", _nom812, zal);//, nom812zal == 1 ? "dzień" : "dni");
            hidNom812.Value = nom812zal.ToString();
            lbVal3.Text = plan812.ToString();
            */


            int planZal = db.getInt(dr, "planZal", 0);
            lbZaleg.Text = String.Format("Urlop zaległy ({0} {1}) do końca września:", zal, zal == 1 ? "dzień" : "dni");
            hidZaleg.Value = zal.ToString();
            lbVal5.Text = planZal.ToString();

            //lbWymiarEtatu.Text = etat;
            //Tools.ExecOnStart2("scpuval", String.Format("getPlanUrlopowSumy();", ));
        }


        /*
declare @plan int
declare @etatL int
declare @etatM int
declare @pracId int
declare @data datetime
declare @nrew varchar(20)
declare @dataZatr datetime
declare @umowaDo datetime
declare @plan812 int
set @pracId = {0}
set @data = '{1}'

select @nrew = KadryId, @etatL = EtatL, @etatM = EtatM from Pracownicy where Id = @pracId
select @plan = COUNT(*) from PlanUrlopow where IdPracownika = @pracId and Data between dbo.boy(@data) and dbo.eoy(@data) and Do is null;
select @plan812 = COUNT(*) from PlanUrlopow where IdPracownika = @pracId and Data between dbo.boy(@data) and dateadd(d, -1, dateadd(m, 8, dbo.boy(@data))) and Do is null;
select top 1 @dataZatr = DataZatrudnienia, @umowaDo = UmowaDo from PracownicyUmowy where IdPracownika = @pracId and UmowaOd <= dbo.eoy(@data) order by UmowaOd desc 

select EtatL, EtatM,
    @dataZatr as DataZatr, @umowaDo as UmowaDo,
    UrlopNom,
    UrlopZaleg,
    Zaplanowany,
    UrlopNom + UrlopZaleg - Zaplanowany as Pozostalo,
    
    14 as nom14,
    0 as plan14,

    round(UrlopNom * cast(8 as float)/12, 0, 1) + 1 as nom812,
    @plan812 as plan812
from
(
    SELECT 
        ISNULL(PO.EtatL, P.EtatL) as EtatL,
        ISNULL(PO.EtatL, P.EtatL) as EtatM,
        round(U.UrlopNom / ((cast(ISNULL(PO.EtatL, P.EtatL) as float) / ISNULL(PO.EtatM, P.EtatM)) * 8),2) as UrlopNom, 
        round(U.UrlopZaleg / ((cast(ISNULL(PO.EtatL, P.EtatL) as float) / ISNULL(PO.EtatM, P.EtatM)) * 8),2) as UrlopZaleg, 
        @plan as Zaplanowany  
    FROM UrlopZbior U 
    left join Pracownicy P on P.Id = @pracId
    left join OkresyRozl O on dbo.eoy(@data) between O.DataOd and O.DataDo 
    left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = @pracId
    WHERE U.KadryId = @nrew and U.Rok = YEAR(@data)
) D
         */



        //--------------------------
        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string UmowaOd  // data zatrudnienia tak na prawde
        {
            set { hidUmowaOd.Value = value; }
            get { return hidUmowaOd.Value; }
        }

        public string UmowaDo
        {
            set { hidUmowaDo.Value = value; }
            get { return hidUmowaDo.Value; }
        }

        /*
                public int Etat
                {
                    get { return Tools.GetViewStateInt(ViewState[ID + "_etat"], 0); }
                    set { ViewState[ID + "_etat"] = value; }
                }
                 */ 
    }
}


/*

declare @plan int
declare @plan812 int
declare @planZal int
declare @plan9 int
declare @etatL int
declare @etatM int
declare @pracId int
declare @data datetime
declare @nrew varchar(20)
declare @dataZatr datetime
declare @umowaDo datetime
declare @boy datetime
declare @eoy datetime
declare @koniec8 datetime
declare @pocz9 datetime
declare @koniec9 datetime
set @pracId = {0}
set @data = '{1}'
-----
set @boy = dbo.boy(@data)
set @eoy = dbo.eoy(@data)
set @koniec8 = dateadd(d, -1, dateadd(m, 8, dbo.boy(@data)))
set @pocz9   = dateadd(m, 8, dbo.boy(@data))
set @koniec9 = dateadd(d, -1, dateadd(m, 9, dbo.boy(@data)))
select @nrew = KadryId, @etatL = EtatL, @etatM = EtatM from Pracownicy where Id = @pracId
select @plan = COUNT(*) from PlanUrlopow where IdPracownika = @pracId and Data between @boy and @eoy and Do is null and KodUrlopu is not null;
select @plan812 = COUNT(*) from PlanUrlopow where IdPracownika = @pracId and Data between @boy and @koniec8 and Do is null and KodUrlopu is not null;
select @planZal = COUNT(*) from PlanUrlopow where IdPracownika = @pracId and Data between @boy and @koniec9 and Do is null and KodUrlopu is not null;
select @plan9 = COUNT(*) from PlanUrlopow where IdPracownika = @pracId and Data between @pocz9 and @koniec9 and Do is null and KodUrlopu is not null;
select top 1 @dataZatr = DataZatrudnienia, @umowaDo = UmowaDo from PracownicyUmowy where IdPracownika = @pracId and UmowaOd <= @eoy	order by UmowaOd desc -- koniec roku
-----
--declare @rob8 int
declare @robRok int
declare @umowa8 int
declare @d8a datetime, @d8b datetime
set @d8a = dbo.MaxDate2(@boy, @dataZatr)
set @d8b = dbo.MinDate2(@koniec8, @umowaDo) 
if @d8a <= @d8b begin
	--select @rob8 = DATEDIFF(DAY, @boy, @koniec8) + 1 - COUNT(*) from Kalendarz where Data between @boy and @koniec8 and Rodzaj in (0,1,2)
	select @robRok = DATEDIFF(DAY, @boy, @eoy) + 1 - COUNT(*) from Kalendarz where Data between @boy and @eoy and Rodzaj in (0,1,2)
	select @umowa8 = DATEDIFF(DAY, @d8a, @d8b) + 1 - COUNT(*) from Kalendarz where Data between @d8a and @d8b and Rodzaj in (0,1,2)
end	
else begin
	--set @rob8 = 0
	set @robRok = 0
	set @umowa8 = 0
end
-----
declare @cnt int
declare @plan14 int
declare @dates varchar(max)
declare @uw bit
set @uw = 0
set @cnt = 0
set @plan14 = 0
set @dates = ''

select 
	@cnt = case when (PU.Data is not null or K.Data is not null) and (@cnt = 0 or CONVERT(varchar(10), DATEADD(D, -1, D.Data), 20) = RIGHT(@dates, 10))
		then @cnt + 1 else 0 end,
	@uw = case 
			when PU.Data is not null then 1 
			when @cnt = 0 then 0 
		  else @uw 
		  end,	
	@plan14 = case when @uw = 1 and @cnt > @plan14 then @cnt else @plan14 end,
	@dates = @dates + case when PU.Data is not null or K.Data is not null 
		then CONVERT(varchar(10), D.Data, 20) else '' end
from dbo.GetDates2(dbo.boy(@data), dbo.eoy(@data)) D
left join PlanUrlopow PU on PU.IdPracownika = @pracId and PU.Data = D.Data and PU.Do is null and KodUrlopu is not null
left join Kalendarz K on K.Data = D.Data and K.Rodzaj in (0,1,2)
-----
select EtatL, EtatM,
    @dataZatr as DataZatr, @umowaDo as UmowaDo,
    UrlopNom,
	UrlopZaleg,

    0 as UrlopDodatkowy,
    null as DataZwiekszenia,

	Zaplanowany,
	UrlopNom + UrlopZaleg - Zaplanowany as Pozostalo,
    WymiarRok,

    /*
    case 
        when WymiarRok = 0 then 0
        when round(UrlopNom * cast(14 as float) / WymiarRok, 0, 1) + 1 > 14 then 14 
    else     round(UrlopNom * cast(14 as float) / WymiarRok, 0, 1) + 1
    end as nom14,
    * /
    14 as nom14,

    @plan14 as plan14,

	--@umowa8, @rob8, @robRok, @d8a, @d8b,
	case when (WymiarRok > 0 and @robRok > 0) then 
		round(cast(@umowa8 as float) * WymiarRok / @robRok, 0, 1) + 1 
	else 0
	end as nom812,
	
    @plan812 as plan812,
    @plan9 as plan9,

    case when @planZal > UrlopZaleg then UrlopZaleg else @planZal end as planZal
from
(
	SELECT 
		ISNULL(PO.EtatL, P.EtatL) as EtatL,
		ISNULL(PO.EtatL, P.EtatL) as EtatM,
		ISNULL(round(U.UrlopNom / ((cast(ISNULL(PO.EtatL, P.EtatL) as float) / ISNULL(PO.EtatM, P.EtatM)) * 8),2), 0) as UrlopNom, 
		ISNULL(round(U.UrlopZaleg / ((cast(ISNULL(PO.EtatL, P.EtatL) as float) / ISNULL(PO.EtatM, P.EtatM)) * 8),2), 0) as UrlopZaleg, 
        ISNULL(U.WymiarRok, 0) as WymiarRok,
        @plan as Zaplanowany
    FROM Pracownicy P
    left join UrlopZbior U on U.KadryId = @nrew and U.Rok = YEAR(@data)
	left join OkresyRozl O on dbo.eoy(@data) between O.DataOd and O.DataDo 
	left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = @pracId
	WHERE P.Id = @pracId 
) D
            ", PracId, data));
 
 */