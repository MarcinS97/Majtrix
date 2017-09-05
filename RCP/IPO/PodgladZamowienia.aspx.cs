using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using HRRcp.App_Code;
using HRRcp.IPO.App_Code;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Text;

namespace HRRcp.IPO
{
    public partial class PodgladZamowienia : System.Web.UI.Page
    {
        private string id;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
            
            string HashLink = Request.QueryString["HashLink"];
            Regex regex = new Regex("^[A-Fa-f0-9]+$");
            if (HashLink != null && regex.IsMatch(HashLink))
            {
                id = IPO_db.getScalar("SELECT Id FROM IPO_Zamowienia WHERE HashLink = " + IPO_db.paramStr(HashLink));
                if ("".Equals(id))
                {
                    AppError.Show("Nie znaleziono zamówienia");
                }
                else
                {
                    dostawcaDataSource.SelectParameters["IdZamowienia"].DefaultValue = id;
                    pozycjeDataSource.SelectParameters["IdZamowienia"].DefaultValue = id;
                    sciezkaAkceptacjiDataSource.SelectParameters["IdZamowienia"].DefaultValue = id;
                    DataRow dr = IPO_db.getDataRow(@"SELECT IPO_Zamowienia.Numer AS Numer,
                                                            IPO_Magazyny.Nazwa AS Magazyn,
                                                            IPO_Statusy.Nazwa AS Status,
                                                            replace(convert(varchar, IPO_Zamowienia.DataUtworzenia, 111), '/', '-') AS Data,
                                                            IPO_Zamowienia.Wartosc AS Wartosc
                                                        FROM IPO_Zamowienia
                                                        JOIN IPO_Magazyny ON IPO_Zamowienia.IdMagazynu = IPO_Magazyny.Id
                                                        JOIN IPO_Statusy ON IPO_Zamowienia.IdStatusu = IPO_Statusy.Id
                                                        WHERE IPO_Zamowienia.Id = " + id);
                    BillShipTo.Text = dr["Magazyn"].ToString();
                    Buyer.Text = App.User.ImieNazwisko;
                    ShippingInvoice.Text = @"Shipping Invoice, complete with tarriff codes, to be sent with goods for every shipment.
Our Purchase Order Number, Item Number and Part Number must appear on all Shipping Papers, Delivery
Dockets, Invoices, Packages and other correspondance. Deliver the required items to: " + dr["Magazyn"].ToString() + @".
Send completed Invoice(s) directly to the Accounts Payable Department.";
                    
                    PONo.Text = dr["Numer"].ToString();
                    POStatus.Text = dr["Status"].ToString();
                    Date.Text = dr["Data"].ToString();
                    RevisionDate.Text = dr["Data"].ToString();
                    Wartosc.Text = dr["Wartosc"].ToString();
                }
            }
            else
            {
                AppError.Show("Wystąpił nieoczekiwany błąd w iPO");
            }
        }

        protected void dostawca_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            fillVendorData();
        }

        protected void dostawca_OnDataBound(object sender, EventArgs e)
        {
            fillVendorData();
        }

        protected void WszyscyCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            dostawcaDropDownList.Enabled = !WszyscyCheckBox.Checked;
            WyslijLink.Visible = !WszyscyCheckBox.Checked && !SciezkaAkceptaccjiCheckBox.Checked;
            fillVendorData();
        }

        protected void SciezkaAkceptaccjiCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            sciezkaAkceptacji.Visible = SciezkaAkceptaccjiCheckBox.Checked;
            WyslijLink.Visible = !WszyscyCheckBox.Checked && !SciezkaAkceptaccjiCheckBox.Checked;
            fillVendorData();
        }

        protected void CSV_OnClick(object sender, EventArgs e)
        {
            string header = "Zamowienie nr: " + PONo.Text;
            string footer = "Wartość: " + Wartosc.Text;

            StringWriter stringWriter = new StringWriter();

            if (!String.IsNullOrEmpty(header)) stringWriter.WriteLine(header);
            string d;
            string line = null;
            int cnt = pozycje.Columns.Count; // ds.Tables[0].Columns.Count;

            for (int i = 0; i < cnt; i++)
            {
                //if (prepareColumns)
                //    d = Tools.CtrlToText(Report.PrepareColumnTitle(pozycje.Columns[i].HeaderText /*ds.Tables[0].Columns[i].ToString()*/));
                //else
                    d = Tools.CtrlToText(pozycje.Columns[i].HeaderText /*ds.Tables[0].Columns[i].ToString()*/);
                if (i == 0)
                    line = d;
                else
                    line += Tools.TAB + d;
            }
            stringWriter.WriteLine(line);

            foreach (GridViewRow dr in pozycje.Rows)
            {
                for (int i = 0; i < cnt; i++)
                {
                    string v = dr.Cells[i].Text;
                    if ("&nbsp;".Equals(v))
                        v = "";
                    d = Tools.CtrlToText(v);
                    if (i == 0)
                        line = d;
                    else
                        line += Tools.TAB + d;
                }
                stringWriter.WriteLine(line);
            }
            if (!String.IsNullOrEmpty(footer)) stringWriter.WriteLine(footer);


            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Zamowienie " + PONo.Text + (!WszyscyCheckBox.Checked ? " "+VendorName.Text : "")+".csv");
            Response.Write((char)65279);
            Response.Write(stringWriter.ToString());
            Response.Flush();
            Response.End();
        }

        private void renameSendButton()
        {
            string dataMaila = IPO_db.getScalar("SELECT COUNT(*) FROM IPO_PozycjeZamowien WHERE DataMaila IS NOT NULL AND IdZamowienia = " + id + " AND IdDostawcy = " + dostawcaDropDownList.SelectedValue);
            if (!"0".Equals(dataMaila))
                WyslijLink.Text = "Wyślij ponownie";
            else
                WyslijLink.Text = "Wyślij";
        }

        private void fillVendorData()
        {
            vendorTable.Visible = !WszyscyCheckBox.Checked;
            if (WszyscyCheckBox.Checked)
            {

            }
            else
            {
                renameSendButton();
                DataRow dr = IPO_db.getDataRow(@"SELECT Id, Nazwa, Adres1, Adres2, KodPocztowy, Miejscowosc, Telefon, Email
                                                FROM IPO_Dostawcy
                                                WHERE Id = " + dostawcaDropDownList.SelectedValue);
                VendorID.Text = dr["Id"].ToString();
                VendorName.Text = dr["Nazwa"].ToString();
                VendorAddress1.Text = dr["Adres1"].ToString();
                VendorAddress2.Text = dr["Adres2"].ToString();
                VendorAddress3.Text = dr["KodPocztowy"].ToString();
                VendorAddress4.Text = dr["Miejscowosc"].ToString();
                VendorPrimaryContact.Text = dr["Telefon"].ToString();
                VendorEmail.Text = dr["Email"].ToString();

                string wartosc = IPO_db.getScalar(@"SELECT COALESCE(SUM(IPO_PozycjeZamowien.Ilosc*IPO_PozycjeZamowien.Cena*IPO_PozycjeZamowien.KursZlozenia/IPO_PozycjeZamowien.PrzelicznikZlozenia), 0)
									FROM IPO_PozycjeZamowien 
									JOIN IPO_Waluty ON IPO_PozycjeZamowien.Waluta = IPO_Waluty.Symbol 
									WHERE IPO_PozycjeZamowien.IdStatusu != 7 AND IPO_PozycjeZamowien.IdZamowienia = " + pozycjeDataSource.SelectParameters["IdZamowienia"].DefaultValue + @"
                                    AND IPO_PozycjeZamowien.IdDostawcy = " + dostawcaDropDownList.SelectedValue);
                Wartosc.Text = "" + Math.Round(Convert.ToDecimal(wartosc), 2);
            }
        }

        protected void PodgladWydruku_OnValueChanged(object sender, EventArgs e)
        {
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=Wydruk.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string html = PodgladWydruku.Value;
            html = html.Substring(html.IndexOf('|')+1);
            
            int imgIndex = html.IndexOf("img");
            int imgEndIndex = html.IndexOf('>', imgIndex)+1;
            html = html.Substring(0, imgEndIndex) + "</img>" + html.Substring(imgEndIndex);

            Stream ms = new MemoryStream();

            Document document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
            StringReader reader = new StringReader(html);
            //PdfWriter pdfWriter = PdfWriter.GetInstance(document, Response.OutputStream);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, ms);
            document.Open();
            XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, document, reader);
            pdfWriter.CloseStream = false;
            document.Close();
            reader.Close();
            
            IPO_Mailing.SendPDFToVendor(pozycjeDataSource.SelectParameters["IdZamowienia"].DefaultValue, "" + dostawcaDropDownList.SelectedValue, ms);
            renameSendButton();
            //Response.Write(document);
            //Response.End();            
        }
    }
}
