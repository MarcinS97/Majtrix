using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using HRRcp.App_Code;
using System.IO;
using System.Text;
using System.Diagnostics;

using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards.Reports
{
    public partial class RapRozdzielnik : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasScAccessAdm && Raport.HasRight(Dictionaries.Reports.RapRozdzielnik))
                {
                    Tools.SetNoCache();
                }
                else
                {
                    App.ShowNoAccess("Raport rozdzielnik kosztowy", App.User);
                }

                //if (!Raport.HasRight(9))
                //    App.ShowNoAccess("Raport - Rozdzielnik kosztowy", App.User);

                ddlYears.DataBind();
                Rep1.SQL1 = ddlYears.SelectedValue;
                PrepareTitle();
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public void btnFilter_Click(object sender, EventArgs e)
        {
            Rep1.SQL1 = ddlYears.SelectedValue;
            Rep1.Prepare();
            PrepareTitle();
        }

        void PrepareTitle()
        {
            Rep1.Title2 = String.Format("Miesiąc rozliczeniowy: {0:d}", ddlYears.SelectedItem.Text);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void CheckErrors(object sender, EventArgs e)
        {
            Raport.Show(Dictionaries.Reports.RapRozdzielnikBledy.ToString(), ddlYears.SelectedValue, 0.ToString());
        }

        public void ExportXML(object sender, EventArgs e)
        {
            //String p = Tools.GetStr(Request.QueryString[0]);
            //p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
            Int32 Counter = 0;
            String Date = ddlYears.SelectedValue;//p.Split('|')[0];

            if (String.IsNullOrEmpty(Date)) return;

            DataTable dt = db.Select.Table(dsXML, Date);

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            //writer.Settings.Encoding = Encoding.GetEncoding(1250);
            //writer.WriteStartDocument(true);
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("eExact");
            writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi:noNamespaceSchemaLocation", "eXact-Schema.xsd");
            writer.WriteStartElement("GLEntries");
            writer.WriteStartElement("GLEntry");

            WriteElementWithAttribute(writer, "Division", "code", "800", false);
            WriteElementWithAttribute(writer, "Journal", "code", "900", true);

            foreach (DataRow dr in dt.Rows)
            {
                ++Counter;

                writer.WriteStartElement("FinEntryLine");
                writer.WriteAttributeString("number", Counter.ToString());
                writer.WriteAttributeString("type", "N");
                writer.WriteAttributeString("subtype", "G");
                writer.WriteElementString("Date", db.getStr(dr["Date"]));
                writer.WriteElementString("DocumentDate", db.getStr(dr["ReportingDate"]));
                WriteElementWithAttribute(writer, "GLAccount", "code", db.getStr(dr["ExactGLaccountcode"]), true);
                writer.WriteElementString("Description", db.getStr(dr["Description"]));
                WriteElementWithAttribute(writer, "Costcenter", "code", db.getStr(dr["CostCenter"]), true);
                WriteElementWithAttribute(writer, "Costunit", "code", db.getStr(dr["CostunitCode"]), true);

                writer.WriteStartElement("Creditor");
                writer.WriteAttributeString("code", db.getStr(dr["Creditor"]));
                writer.WriteAttributeString("number", db.getStr(dr["Creditor"]).Trim());
                writer.WriteAttributeString("type", "S");
                writer.WriteEndElement();   // Creditor

                WriteElementWithAttribute(writer, "Project", "code", db.getStr(dr["Project"]), true);

                writer.WriteStartElement("Amount");
                WriteElementWithAttribute(writer, "Currency", "code", db.getStr(dr["Currency"]), false);
                writer.WriteElementString("Debit", db.getStr(dr["Debit"]));
                writer.WriteElementString("Credit", db.getStr(dr["Credit"]));

                writer.WriteStartElement("VAT");
                writer.WriteAttributeString("code", "0");

                writer.WriteStartElement("Creditor");
                writer.WriteAttributeString("code", db.getStr(dr["Creditor"]));
                writer.WriteAttributeString("number", db.getStr(dr["Creditor"]).Trim());
                writer.WriteAttributeString("type", "S");
                writer.WriteEndElement();   // Creditor
                

                writer.WriteEndElement();   // VAT
                writer.WriteEndElement();   // Amount

                writer.WriteStartElement("FinReferences");
                writer.WriteAttributeString("TransactionOrigin", "N");
                writer.WriteElementString("YourRef", "");
                writer.WriteElementString("DocumentDate", db.getStr(dr["ReportingDate"]));
                writer.WriteEndElement();   // FinReferences

                writer.WriteStartElement("CSFields");
                writer.WriteStartElement("CSExtraField1");
                writer.WriteEndElement();   // CSExtraField1
                writer.WriteEndElement();   // CSFields

                writer.WriteStartElement("Payment");
                writer.WriteStartElement("PaymentCondition");
                writer.WriteAttributeString("code", "");
                writer.WriteEndElement();   // PaymentCondition
                writer.WriteEndElement();   // Payment



                writer.WriteEndElement();   // FinEntryLine
            }
            writer.WriteEndElement();   // GLEntry
            writer.WriteEndElement();   // GLEntries
            writer.WriteEndElement();   // eXact
            writer.WriteEndDocument();
            writer.Close();

            //using(XmlWriter Writer 

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(1250);//Encoding.ASCII; //Encoding.Unicode;
            HttpContext.Current.Response.Charset = "1250";//"unicode";

            //            this.EnableViewState = false;

            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}{1}\"", "IMPF",
                //".xls"
                    ".xml"     // nowy excel wyświetla warninga jak mu się format nie zgadza
                    ));

            //HttpContext.Current.Response.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
            HttpContext.Current.Response.Write(db.RemovePL(sw.ToString().Replace("encoding=\"utf-16\"", String.Empty)));
            HttpContext.Current.Response.End();
        }

        void WriteElementWithAttribute(XmlWriter writer, string element, string attr, string val, bool empty)
        {
            writer.WriteStartElement(element);
            writer.WriteAttributeString(attr, val);
            //if (empty) writer.WriteString("a");
            writer.WriteEndElement();
        }


        protected void DownloadPDF(object sender, EventArgs e)
        {
            PDF PDF = new PDF();
            PDF.Download(Rep1, Server, Response, Request);
        }
    }
}
