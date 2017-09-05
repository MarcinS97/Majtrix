using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PracUrlop2 : System.Web.UI.UserControl
    {
        public const int moUrlopWyp  = 0;    // tak jak kwitek
        public const int moChorobowe = 1;   
        public const int moUrlopy    = 2;

        int FMode = moUrlopWyp;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GetSql()
        {
            /*
    0 as Id, 
    P.Id, as IdPracownika, 
    A.LpLogo as NR_EW, 
    A.DataOd, 
    A.DataDo, 
    A.Kod, 
    A.IleDni, 
    sum(A.Godzin) as Godzin
            */
            const string select = @"
select 
    convert(varchar(10), A.DataOd, 20) as Od, 
    convert(varchar(10), A.DataDo, 20) as Do, 
    K.Symbol, 
    K.Nazwa, 
    A.IleDni as [Ilość dni]
    --,sum(A.Godzin) as Godzin
from bufAbsencja A 
left outer join AbsencjaKody K on K.Kod = A.Kod 
where A.Typ = '{0}' and A.IleDni > 0 and A.LpLogo = '{1}' and A.DataOd >= DATEADD(YEAR, -1, dbo.boy(GETDATE())) 
group by A.LpLogo, A.DataOd, A.DataDo, A.Kod, A.IleDni, K.Symbol, K.Nazwa
order by A.DataOd desc";

            switch (FMode)
            {
                case moChorobowe:   //<<<<<<< na razie z bufAbsencja !!! potem dodac do Absencja kod U/Z
                    return String.Format(select, "Z", App.KwitekKadryId);
                case moUrlopy:      
                    return String.Format(select, "U", App.KwitekKadryId);
                default:
                    return String.Format(@"
SELECT convert(varchar(10), A.DataOd, 20) as Od, convert(varchar(10), A.DataDo, 20) as Do, K.Symbol, K.Nazwa, A.IleDni as [Ilość dni] FROM Absencja A
left outer join AbsencjaKody K on K.Kod = A.Kod WHERE A.NR_EW = '{0}' 
    --and A.Kod in (7,19, 1000090080, 1000) 
    and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '7,19,1000090080,1000'), ','))
and A.DataOd >= DATEADD(YEAR, -1, dbo.boy(GETDATE()))
ORDER BY [DataOd] desc", App.KwitekKadryId);
            }
        }

        public void Prepare(string pracId)
        {
            SqlDataSource1.SelectCommand = GetSql();
            gvUrlopy.DataBind();
        }

        /*
        public void Show(string pracId, string panelId, string parent)
        {
            Prepare(pracId);
            string nazwisko = db.getScalar("select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' from Pracownicy where Id = " + pracId);
            string up = Parent.Parent.ClientID; // update panel tu jest 
            up = parent;
            Tools.MakeButton(btClose, "javascript: $('#divZoom').dialog('close'); return true;");
            Tools.ExecOnStart2("scZoomUrlop", "zoomPracUrlop('Urlopy pracownika: " + nazwisko + "','" + up + "');");
        }
        */

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }
    }
}