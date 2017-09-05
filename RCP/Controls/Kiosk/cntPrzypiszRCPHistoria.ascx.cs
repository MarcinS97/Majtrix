using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Kiosk
{
    public partial class cntPrzypiszRCPHistoria : System.Web.UI.UserControl
    {
        public event EventHandler Deleted;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, Tools.ListViewMode.Bootstrap);
            Tools.PrepareSorting(ListView1, 1, 6);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //----------------------
        string pid = null;
        string prac = null;
        string nrew = null;
        string prev = null;
        string karta = null;
        string other = null;

        protected void ListView1_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            DataRow dr = db.getDataRow(@"
select R.*, P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId as Nr_Ewid
from PrzypisaniaRCP R 
left outer join Pracownicy P on P.Id = R.IdPracownika
where R.Id = " + Tools.GetDataKey(ListView1, e));//Tools.GetDataKey(ListView1, e.ItemIndex));
            pid = db.getValue(dr, "IdPracownika");
            prac = db.getValue(dr, "Pracownik");
            nrew = db.getValue(dr, "Nr_Ewid");
            prev = db.getValue(dr, "PrevNrKarty");
            karta = db.getValue(dr, "NrKarty");
            other = db.getValue(dr, "OtherPracId");
            Log.Info(Log.PRZYPISZRCP, "Przywrócenie numeru karty RCP", String.Format("Pracownik: {0} ({1}) Karta: [{2}] -> [{3}]", prac, nrew, karta, prev));
        }

        protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            if (e.AffectedRows == 1)
            {
                string data = Tools.DateToStrDb(DateTime.Today);
                string curr = db.getScalar(String.Format("select top 1 NrKarty from PracownicyKarty where IdPracownika = {0} and '{1}' between Od and ISNULL(Do,'20990909')", pid, data));
                if (curr == karta)
                {
                    bool ok = db.update("PracownicyKarty", 0, "NrKarty", 
                        String.Format("IdPracownika={0} and '{1}' between Od and ISNULL(Do,'20990909')", pid, data), 
                        db.strParam(prev));
                    if (ok)
                        Tools.ShowMessageLog(Log.PRZYPISZRCP, String.Format("Pracownik: {0} ({1})", prac, nrew),
                            String.Format("Pracownikowi został ustawiony poprzedni numer karty: [{0}].", String.IsNullOrEmpty(prev) ? "brak numeru" : prev));
                    else
                        Tools.ShowErrorLog(Log.PRZYPISZRCP, String.Format("Pracownik: {0} ({1})", prac, nrew), 
                            "Wystąpił błąd podczas usuwania przypisania karty RCP.");
                    
                    if (Deleted != null)
                        Deleted(this, EventArgs.Empty);
                }
                else
                    Tools.ShowErrorLog(Log.PRZYPISZRCP, null,
                        String.Format("Pracownik: {3} ({4})\nBieżący numer karty RCP [{0}] jest inny niż w usuniętym przypisaniu [{1}]. Przywrócenie numeru [{2}] nie jest możliwe automatycznie.",
                        curr, karta, prev, prac, nrew));
            }
        }
    }
}