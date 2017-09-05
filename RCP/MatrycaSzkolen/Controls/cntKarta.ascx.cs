using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntKarta : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
            }
        }

        public static string GetRow(string col0, string col1, string col2, string col3, string col4, string col5)
        {
            string row = String.Format(@"
\trowd\trautofit1  
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx1
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx2
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx3
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx4
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx5
\clbrdrt\brdrs\clbrdrl\brdrs\clbrdrb\brdrs\clbrdrr\brdrs
\cellx6
{0}\intbl\cell
{1}\intbl\cell
{2}\intbl\cell
{3}\intbl\cell
{4}\intbl\cell
{5}\intbl\cell
\row", col0, col1, col2, col3, col4, col5);
            return row;
        }

        public static string GetRow(DataRow dr)
        {
            string nazwa = db.getValue(dr, "Nazwa", String.Empty);
            string dataod = db.getValue(dr, "DataOd", String.Empty);
            string datado = db.getValue(dr, "DataDo", String.Empty);
            string trener = db.getValue(dr, "Trener", String.Empty);
            string row = GetRow(nazwa, dataod, datado, trener, "", "");
            return row;
        }

        public static void Print(HttpServerUtility Server, string empId)
        {
            DataTable dt = db.Select.Table(@"declare @pracId int = {0}
    
select
  u.Nazwa
, CONVERT(varchar(10), c.DataRozpoczecia, 20) DataOd
, CONVERT(varchar(10), c.DataZakonczenia, 20) DataDo
, p.Imie + ' ' + p.Nazwisko Trener
, null [Podpis pracownika*]
, null [Podpis kierownika komórki organizacyjnej **]
from Uprawnienia u
left join Certyfikaty c on c.IdUprawnienia = u.Id and c.IdPracownika = @pracId
left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId
left join Pracownicy p on p.Id = c.IdAutora
where u.Typ = 2048 and uk.NazwaEN = 'STAN'
order by u.Kolejnosc ", empId);

            DataRow row = db.Select.Row(@"
select 
Nazwisko + ' ' + Imie Pracownik 
from Pracownicy where Id = {0}
", empId);



            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/karta.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            string tbl = @"{\rtf1\ansi\deff0";

            tbl += GetRow("Zakres szkolenia - wg programów szkoleń"
                , "Data rozpoczęcia"
                , "Data zakończenia"
                , "Imię i nazwisko prowadzącego instruktaż"
                , "Podpis pracownika"
                , "Podpis kierownika komórki organizacyjnej");

            foreach(DataRow dr in dt.Rows)
            {
                tbl += GetRow(dr);
            }

            tbl += "}";


            text = text.Replace("%TABLE%", tbl);
            text = text.Replace("%PRAC%", db.getValue(row, "Pracownik", ""));

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aou.rtf"));
            HttpContext.Current.Response.AddHeader("Content-Length", text.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(1250);
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();



        }
    }
}